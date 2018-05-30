using UnityEngine;
using UnityEngine.Events;

namespace UnityStandardUtils.CC2D
{
    [RequireComponent(typeof(CapsuleCollider2D), typeof(Rigidbody2D), typeof(Animator))]
    public class CharacterController2D : MonoBehaviour
    {
        public enum _FaceToward
        {
            Left, Right
        }
        [System.Serializable]
        public class InteractEvent : UnityEvent<GameObject> { }
        [System.Serializable]
        public class _InputStatus
        {
            [Tooltip("输入一帧左移")]
            public bool Left = false;
            [Tooltip("输入一帧右移")]
            public bool Right = false;
            [Tooltip("输入一帧冲刺状态")]
            public bool Sprint = false;
            [Tooltip("输入一帧蹲行状态")]
            public bool Crouch = false;
            [Tooltip("输入一帧向上攀爬")]
            public bool ClimbUp = false;
            [Tooltip("输入一帧向下攀爬")]
            public bool ClimbDown = false;
            [Tooltip("输入一帧跳跃")]
            public bool Jump = false;
            [Tooltip("输入一帧交互")]
            public bool Interact = false;
        }
        [System.Serializable]
        public class _LayerSetting
        {
            [Tooltip("可以攀爬的Collider的Layer")]
            public LayerMask ClimbableLayer = -1;
            [Tooltip("可以交互的Collider的Layer")]
            public LayerMask InteractableLayer = -1;
        }
        [System.Serializable]
        public class _AnimatorParameter
        {
            [Tooltip("不同动画状态传递给animator的参数名")]
            public string ParamName = "cc2dparam";
            public int Idle = 0;
            public int Walking = 1;
            public int Sprinting = 2;

            public int CrouchIdle = 3;
            public int Crouching = 4;

            public int ClimbIdle = 5;
            public int ClimbUp = 6;
            public int ClimbDown = 7;

            public int Jumping = 8;
            public int Interacting = 9;
        }

        [Tooltip("初始状态下，玩家是朝向哪里的呢？")]
        public _FaceToward DefaultFaceToward = _FaceToward.Right;

        [Tooltip("角色移动的速度")]
        public float MoveSpeed = 100f;

        [Tooltip("角色攀爬的速度")]
        public float ClimbSpeed = 3f;

        [Tooltip("角色在冲刺时，针对移动速度的系数")]
        [Range(1f, 5f)]
        public float SprintSpeedMulti = 1.6f;

        [Tooltip("角色在蹲行时，针对移动速度的系数")]
        [Range(0f, 1f)]
        public float CrouchSpeedMulti = 0.3f;

        [Tooltip("角色在蹲行时，针对碰撞体高度的系数")]
        [Range(0f, 1f)]
        public float CrouchSizeYMulti = 0.6f;

        [Tooltip("角色每次跳跃时，施加给角色向上的力的大小")]
        public float JumpForce = 150f;

        [Tooltip("角色的大小")]
        [SerializeField]
        private Vector2 Size = new Vector2(1f, 2f);

        [Tooltip("一些相关Layer的设置")]
        public _LayerSetting LayerSetting;
        [Tooltip("在每次角色动画更新时，传递给自身animator的参数")]
        public _AnimatorParameter AnimatorParameter;



        //[HideInInspector]
        [Tooltip("角色输入器，是控制整个角色的入口，在每帧开始时通过该输入器控制角色当前帧，并在每帧结束时值置为false")]
        public _InputStatus InputStatus;


        //在角色与物品交互时触发的触发器，可以通过参数获取交互的物品
        public InteractEvent InteractHandler;

        public bool IsOnGround { get; private set; }
        public bool IsClimbing { get; private set; }
        public _FaceToward CurrentFaceToward { get; private set; }


        public new Rigidbody2D rigidbody2D { get; private set; }
        public new CapsuleCollider2D collider2D { get; private set; }
        public Animator animator { get; private set; }
        
        private float defaultScaleX = 1f;
        private GameObject possibleInteracingObject;

        public void SetFaceToward(_FaceToward toward)
        {
            if(!IsClimbing)CurrentFaceToward = toward;

            setPlayerToward();

        }

        void Start()
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
            collider2D = GetComponent<CapsuleCollider2D>();
            animator = GetComponent<Animator>();

            CurrentFaceToward = DefaultFaceToward;
            defaultScaleX = transform.localScale.x;

            rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rigidbody2D.freezeRotation = true;
            collider2D.size = Size;

            collider2D.isTrigger = false;
            collider2D.offset = Vector2.zero;
            collider2D.direction = CapsuleDirection2D.Vertical;
        }

        private void OnDrawGizmos()
        {
            if (Size.x <= 0.001f) Size.x = 0.001f;
            if (Size.y <= 0.001f) Size.y = 0.001f;

            //GetComponent<CapsuleCollider2D>().size = Size;

            GizmosTool.Capsule2D(transform.position, Size, transform.lossyScale, new Color(0,0.8f,0));
            GizmosTool.Capsule2D(transform.position, Vector2.Scale(Size,new Vector2(1,CrouchSizeYMulti)), transform.lossyScale, new Color(0, 0.5f, 0));
        }

        private void resetInputStatus()
        {
            InputStatus.Left = false;
            InputStatus.Right = false;
            InputStatus.Sprint = false;
            InputStatus.Crouch = false;
            InputStatus.ClimbUp = false;
            InputStatus.ClimbDown = false;
            InputStatus.Jump = false;
            InputStatus.Interact = false;
        }

        /// <summary>
        /// 判断玩家是否在地面上
        /// 更新 IsOnGround;
        /// </summary>
        private void checkOnGround()
        {
            Vector2 leftRayStart = collider2D.bounds.center;
            Vector2 rightRayStart = collider2D.bounds.center;

            leftRayStart.x -= collider2D.bounds.extents.x;
            rightRayStart.x += collider2D.bounds.extents.x;

            //起点的y值应为中心y值减去高度(底部y值)再加上半径()
            leftRayStart.y -= (collider2D.bounds.extents.y - collider2D.bounds.extents.x * 0.8f);
            rightRayStart.y -= (collider2D.bounds.extents.y - collider2D.bounds.extents.x * 0.8f);

            float rayLen = collider2D.bounds.extents.x;

            RaycastHit2D leftRay = Physics2D.Raycast(leftRayStart, Vector2.down, rayLen);
            RaycastHit2D rightRay = Physics2D.Raycast(rightRayStart, Vector2.down, rayLen);

            Debug.DrawLine(leftRayStart, leftRayStart + (Vector2.down.normalized * rayLen), Color.cyan);
            Debug.DrawLine(rightRayStart, rightRayStart + (Vector2.down.normalized * rayLen), Color.cyan);

            if ((leftRay.collider != null && leftRay.collider.isTrigger == false) || (rightRay.collider != null && rightRay.collider.isTrigger == false)) IsOnGround = true;
            else IsOnGround = false;
        }

        /// <summary>
        /// 判断玩家是否可以攀爬
        /// 并在可攀爬时判断玩家是否进入攀爬状态
        /// 更新IsClimbing;
        /// </summary>
        /// <returns>是否可攀爬</returns>
        private bool checkClimbable()
        {
            Vector2 leftRayStart = collider2D.bounds.center;
            Vector2 rightRayStart = collider2D.bounds.center;
            leftRayStart.x -= (collider2D.bounds.extents.x + 0.01f);
            rightRayStart.x += (collider2D.bounds.extents.x + 0.01f);

            float rayLen = 0.1f;

            RaycastHit2D ladderLeft = Physics2D.Raycast(leftRayStart, Vector2.left, rayLen, LayerSetting.ClimbableLayer);
            RaycastHit2D ladderRight = Physics2D.Raycast(rightRayStart, Vector2.right, rayLen, LayerSetting.ClimbableLayer);


            Debug.DrawLine(leftRayStart, leftRayStart + (Vector2.left.normalized * rayLen), Color.yellow);
            Debug.DrawLine(rightRayStart, rightRayStart + (Vector2.right.normalized * rayLen), Color.yellow);

            //If a ladder was detected
            if (ladderLeft.collider != null || ladderRight.collider != null)
            {
                if (ladderOnInputCondition(ladderLeft.collider, ladderRight.collider))
                {
                    //这个将攀爬的x值控制在绝对的准确位置（即角色的边缘紧贴攀爬物的边缘）

                    transform.position = ladderLeft.collider != null ? (
                        new Vector3(
                            ladderLeft.collider.bounds.center.x + ladderLeft.collider.bounds.extents.x + collider2D.bounds.extents.x,
                            transform.position.y, transform.position.z)
                    ) : (
                        new Vector3(
                            ladderRight.collider.bounds.center.x - ladderRight.collider.bounds.extents.x - collider2D.bounds.extents.x,
                            transform.position.y, transform.position.z)
                    );

                    CurrentFaceToward = ladderLeft.collider != null ? _FaceToward.Left : _FaceToward.Right;
                    enterLadder();
                }

                return true;
            }
            else return false;
        }

        /// <summary>
        /// 咱可不能让人在管道里站起来，要不就卡出去啦！
        /// </summary>
        private bool checkStandable()
        {

            if (collider2D.size == Size) return true;

            Vector2 rayStart = collider2D.bounds.center;
            rayStart.y += (collider2D.bounds.extents.y + 0.01f);

            float crouchY = Size.y * transform.localScale.y * CrouchSizeYMulti;
            float rayLen = Size.y * transform.localScale.y - crouchY;
            if (crouchY < Size.x * Mathf.Abs(transform.localScale.x)) rayLen = Size.x * Mathf.Abs(transform.localScale.x);



            Debug.DrawLine(rayStart, rayStart + (Vector2.up.normalized * rayLen), Color.blue);
            RaycastHit2D check = Physics2D.Raycast(rayStart, Vector2.up, rayLen);
            if (check.collider != null && check.collider.isTrigger == false) return false;
            return true;
        }

        /// <summary>
        /// 基本的操作处理及其动画触发
        /// </summary>
        private void basicMovement()
        {
            float vx = 0f;
            animator.SetInteger(AnimatorParameter.ParamName, AnimatorParameter.Idle);

            bool moveLeft = InputStatus.Left && !InputStatus.Right;
            bool moveRight = InputStatus.Right && !InputStatus.Left;

            if (moveLeft)
            {
                CurrentFaceToward = _FaceToward.Left;
                animator.SetInteger(AnimatorParameter.ParamName, AnimatorParameter.Walking);
                vx -= Time.deltaTime * MoveSpeed;
            }

            if (moveRight)
            {
                CurrentFaceToward = _FaceToward.Right;
                animator.SetInteger(AnimatorParameter.ParamName, AnimatorParameter.Walking);
                vx += Time.deltaTime * MoveSpeed;
            }

            bool crouch = (InputStatus.Crouch && !InputStatus.Sprint) || (!checkStandable() && IsOnGround);
            bool sprint = InputStatus.Sprint && !crouch;

            if (sprint && (moveLeft || moveRight))
            {
                animator.SetInteger(AnimatorParameter.ParamName, AnimatorParameter.Sprinting);
                vx *= SprintSpeedMulti;
            }
            if (crouch)
            {
                collider2D.size = new Vector3(Size.x, Size.y * CrouchSizeYMulti);
                if (moveLeft || moveRight)
                {
                    animator.SetInteger(AnimatorParameter.ParamName, AnimatorParameter.Crouching);
                    vx *= CrouchSpeedMulti;
                }
                else
                {
                    animator.SetInteger(AnimatorParameter.ParamName, AnimatorParameter.CrouchIdle);
                }
            }
            else collider2D.size = Size;
            
            //注意！！！
            //velocity属于物理动作，理应存在于FixedUpdate之中
            //鉴于这里的行为基于信号，又必须在Update中调用
            //所以折中，利用两者的帧率比值做系数
            rigidbody2D.velocity = new Vector2(vx*(Time.fixedDeltaTime/Time.deltaTime), rigidbody2D.velocity.y);

            if (IsOnGround && InputStatus.Jump)
                rigidbody2D.AddForce(JumpForce * Vector3.up);

            if (!IsOnGround)
                animator.SetInteger(AnimatorParameter.ParamName, AnimatorParameter.Jumping);

        }



        /// <summary>
        /// 判断输入是否是进入攀爬状态
        /// </summary>
        /// <param name="isLeftLadder">梯子在哪边?</param>
        /// <returns></returns>
        private bool ladderOnInputCondition(Collider2D l, Collider2D r)
        {
            //梯子在哪边? true:左 false:右
            bool isLeftDirection = l != null;

            //判断是否是背景梯
            bool isBackGroundLadder = isLeftDirection ? (l.isTrigger) : (r.isTrigger);

            //是否在怼?
            bool isDuiing = isLeftDirection ? InputStatus.Left : InputStatus.Right;

            //是否在用无论何时都有效的按键来爬上去?
            bool isTriggerKey = InputStatus.ClimbUp || InputStatus.ClimbDown || InputStatus.Interact;

            return
                (
                    //判断是否有效怼墙
                    (
                        //判断是否是背景梯
                        isBackGroundLadder ?
                            //是背景
                            (
                                //背景梯不能在地上靠怼来爬
                                IsOnGround ?
                                false :
                                //不在地上(空中)的话，又是否在怼?
                                isDuiing
                            ) :
                            //不是背景，直接判断是否在怼
                            isDuiing
                    ) || isTriggerKey
                )
                && !IsClimbing;

        }
        /// <summary>
        /// 判断输入是否是离开攀爬状态
        /// </summary>
        /// <returns></returns>
        private bool ladderOffInputCondition()
        {
            bool isMovingAway =
                (CurrentFaceToward == _FaceToward.Left ? InputStatus.Right : InputStatus.Left);

            return (InputStatus.Interact || InputStatus.Jump || isMovingAway);
        }

        /// <summary>
        /// 攀爬操作处理
        /// </summary>
        private void climbMovement()
        {
            //有的时候可能带着蹲下的状态就爬上去了，动画变量没问题，但是collider2d的状态还是蹲下着的，这里复原下
            collider2D.size = Size;

            Vector3 movement = transform.position;

            animator.SetInteger(AnimatorParameter.ParamName, AnimatorParameter.ClimbIdle);

            bool moveUp = InputStatus.ClimbUp && !InputStatus.ClimbDown;
            bool moveDown = InputStatus.ClimbDown && !InputStatus.ClimbUp;

            if (moveUp)
            {
                movement += Vector3.up * Time.deltaTime * ClimbSpeed;
                animator.SetInteger(AnimatorParameter.ParamName, AnimatorParameter.ClimbUp);
            }

            if (moveDown)
            {
                movement += Vector3.down * Time.deltaTime * ClimbSpeed;
                animator.SetInteger(AnimatorParameter.ParamName, AnimatorParameter.ClimbDown);
            }


            //Leaving ladder from top/bottom
            if (!checkClimbable())
            {
                //From top
                if (moveUp)
                {
                    if (CurrentFaceToward == _FaceToward.Left)
                    {
                        movement.x -= collider2D.bounds.extents.x * 2;
                        movement.y += collider2D.bounds.extents.y;
                    }
                    else
                    {
                        movement.x += collider2D.bounds.extents.x * 2;
                        movement.y += collider2D.bounds.extents.y;
                    }

                    transform.position = movement;
                }
                exitLadder();
                return;
            }
            //Leaving ladder by interrupt
            else if (ladderOffInputCondition())
            {
                if (CurrentFaceToward == _FaceToward.Left)
                    movement.x += (0.03f);
                if (CurrentFaceToward == _FaceToward.Right)
                    movement.x -= (0.03f);
                exitLadder();
            }


            rigidbody2D.MovePosition(movement);
        }

        /// <summary>
        /// 离开攀爬状态时的更新
        /// </summary>
        private void exitLadder()
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.gravityScale = 1f;
            IsClimbing = false;
        }

        /// <summary>
        /// 进入攀爬状态时的更新
        /// </summary>
        private void enterLadder()
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.gravityScale = 0f;
            IsClimbing = true;
        }

        /// <summary>
        /// 更新玩家gameObject的朝向
        /// </summary>
        private void setPlayerToward()
        {
            //if (!IsClimbing)
            //{
            //    Vector2 dir = VecTool.GetMouseDirection(transform);
            //    float rotZ = (VecTool.DirectionToRotationZ(dir) + 90f);
            //    if (rotZ > 0f && rotZ < 180f) SetFaceToward(_FaceToward.Right);
            //    if (rotZ < 0f || rotZ > 180f) SetFaceToward(_FaceToward.Left);
            //}


            if (CurrentFaceToward != DefaultFaceToward)
            {
                transform.localScale = new Vector3(-defaultScaleX, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(defaultScaleX, transform.localScale.y, transform.localScale.z);
            }
        }

        private void interact()
        {
            if (InputStatus.Interact && possibleInteracingObject != null)
            {
                if (LayerSetting.InteractableLayer == (LayerSetting.InteractableLayer | (1 << possibleInteracingObject.layer)))
                {
                    animator.SetInteger(AnimatorParameter.ParamName, AnimatorParameter.Interacting);
                    InteractHandler.Invoke(possibleInteracingObject);
                }
            }
        }

        private void Update()
        {
            setPlayerToward();
            checkOnGround();

            if (!IsClimbing)
            {
                basicMovement();
                interact();
            }
            else climbMovement();

            //把checkClimbable()放在climbMovement()后面，why?
            //因为要错开帧：当传来"交互"信号时，checkClimbable和climbmovement都能收到，导致同一帧内完成了“攀爬，离开攀爬”两个动作
            //所以我们让包含“离开攀爬”的climbMovement()放在前面
            checkClimbable();

            resetInputStatus();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            possibleInteracingObject = collision.gameObject;
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject == possibleInteracingObject) possibleInteracingObject = null;
        }
    }

}
