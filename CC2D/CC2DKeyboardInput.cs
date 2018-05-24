using UnityEngine;

namespace UnityStandardUtils.CC2D
{
    [RequireComponent(typeof(CharacterController2D))]
    public class CC2DKeyboardInput : MonoBehaviour
    {
        /// <summary>
        /// 键位设置
        /// </summary>
        [System.Serializable]
        public class _KeySetting
        {
            public KeyCode Left = KeyCode.A;
            public KeyCode Right = KeyCode.D;

            public KeyCode Sprint = KeyCode.LeftShift;
            public KeyCode Crouch = KeyCode.LeftControl;

            public KeyCode ClimbUp = KeyCode.W;
            public KeyCode ClimbDown = KeyCode.S;

            public KeyCode Jump = KeyCode.Space;

            public KeyCode Interact = KeyCode.E;
        }

        [Tooltip("将对应按键与当前物品的CharactorController2D的输入器进行绑定")]
        public _KeySetting KeySetting;

        private CharacterController2D cc2d;


        private void readKeyboardInput()
        {
            cc2d.InputStatus.Left = Input.GetKey(KeySetting.Left);
            cc2d.InputStatus.Right = Input.GetKey(KeySetting.Right);

            cc2d.InputStatus.Sprint = Input.GetKey(KeySetting.Sprint);
            cc2d.InputStatus.Crouch = Input.GetKey(KeySetting.Crouch);

            cc2d.InputStatus.ClimbUp = Input.GetKey(KeySetting.ClimbUp);
            cc2d.InputStatus.ClimbDown = Input.GetKey(KeySetting.ClimbDown);

            cc2d.InputStatus.Jump = Input.GetKeyDown(KeySetting.Jump);
            cc2d.InputStatus.Interact = Input.GetKeyDown(KeySetting.Interact);
        }

        private void Start()
        {
            cc2d = GetComponent<CharacterController2D>();
        }
        private void Update()
        {
            readKeyboardInput();
        }
    }

}
