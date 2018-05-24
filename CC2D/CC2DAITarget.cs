using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardUtils.CC2D
{
    [RequireComponent(typeof(CharacterController2D))]
    public class CC2DAITarget : MonoBehaviour
    {
        [System.Serializable]
        public class _AITargetObject
        {
            public GameObject Target;
            [Tooltip("基础意愿值")]
            [Range(0f, 1f)]
            public float BasicWilling = 1f;
            [Tooltip("距离从最大距离的百分比多少开始影响意愿？")]
            [Range(0f, 1f)]
            public float DistancePercentage = 0f;
            [Tooltip("到了位置做什么呢？")]
            public CharacterController2D.InteractEvent ToDo;
        }

        [Tooltip("最大的目标需求度大于该值时，AI将夺取玩家控制权，在目标完成/无法达成时，释放控制权。" +
            "当然了，前提是玩家有控制权（即CC2DKeyboardInput）")]
        [Range(0f, 1f)]
        public float TakeOverWilling = 0.8f;

        [Tooltip("最大距离，目标的距离大于该值就彻底失去兴趣了")]
        public float DistanceMax = 10f;

        [Tooltip("等待喂养AI的序列")]
        [SerializeField]
        private List<_AITargetObject> AITargetObject;
        private CharacterController2D cc2d;


        private void Start()
        {
            cc2d = GetComponent<CharacterController2D>();
        }
        private void OnDrawGizmos()
        {
            GizmosTool.Circle(DistanceMax, transform.position);

            if (AITargetObject != null && AITargetObject.Count > 0)
                foreach (_AITargetObject target in AITargetObject)
                {
                    if (target.Target!=null)
                        GizmosTool.Text(GUI.skin, GetWillingValue(target) + "", target.Target.transform.position, Color.cyan, 25, 0);
                }
        }

        public _AITargetObject GetTarget()
        {
            if (AITargetObject.Count == 0) return null;

            AITargetObject.Sort((a, b) => { return -GetWillingValue(a).CompareTo(GetWillingValue(b)); });
            CC2DKeyboardInput kb = GetComponent<CC2DKeyboardInput>();
            if ((kb != null && kb.enabled) && GetWillingValue(AITargetObject[0]) < TakeOverWilling) return null;


            return AITargetObject[0];
        }

        public void AddTarget(_AITargetObject target)
        {
            AITargetObject.Add(target);
        }

        public void CleanTarget()
        {
            AITargetObject.Clear();
        }


        /// <summary>
        /// 获取实际意愿值
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public float GetWillingValue(_AITargetObject target)
        {
            if (DistanceMax <= 0f) DistanceMax = 0.001f;

            if (target.BasicWilling < 0f) target.BasicWilling = 0f;
            if (target.DistancePercentage < 0f) target.DistancePercentage = 0f;
            if (target.DistancePercentage >= 1f) target.DistancePercentage = 0.999f;


            float distance = Vector3.Distance(transform.position, target.Target.transform.position);

            if (distance >= DistanceMax) return 0f;




            float distancePercent = distance / DistanceMax;

            float distanceUnWilling = distancePercent > target.DistancePercentage ?
                Mathf.Pow(
                        ((distancePercent - target.DistancePercentage) *
                        (1f / (1f - target.DistancePercentage)))
                    , 2) :
                0f;

            if (distanceUnWilling > 1f) distanceUnWilling = 1f;
            if (distanceUnWilling < 0f) distanceUnWilling = 0f;

            return target.BasicWilling * (1f - distanceUnWilling);
        }
    }

}
