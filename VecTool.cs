using UnityEngine;


namespace UnityStandardUtils
{
    public class VecTool
    {
        public static Vector2 GetMouseDirection(Vector3 position)
        {
            Vector3 v3 = Camera.main.WorldToScreenPoint(position); //将世界坐标转化为屏幕坐标
            Vector2 v2 = new Vector2(v3.x, v3.y);
            Vector2 input = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //取得鼠标点击的屏幕坐标
            return ((input - v2)).normalized;
        }

        public static Vector2 Vector2Up(Vector2 src)
        {
            Vector2 up;
            up.x =
                src.x > 0 ?
                (src.y > 0 ? -src.x : src.x) :
                (src.y > 0 ? src.x : -src.x)
                ;
            up.y = -((src.x * up.x) / src.y);
            return up.normalized;
        }


        public static float DirectionToRotationZ(Vector2 direction)
        {
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }

        public static Quaternion DirectionToRotation(Vector2 direction)
        {
            Quaternion q = Quaternion.AngleAxis(DirectionToRotationZ(direction), Vector3.forward);
            return q;
        }

        public static Vector3 DirectionToRotation(Vector3 direction)
        {
            return Quaternion.LookRotation(direction).eulerAngles;
        }
        public static Vector3 RotationToDirection(Vector3 eularAngles, Vector3? basic = null)
        {
            return Quaternion.Euler(eularAngles) * (basic == null ? Vector3.right : (Vector3)basic);
        }

        public static void SetLossyScale(Transform transform, Vector3 lossyScale)
        {
            Vector3 scale = lossyScale;

            Transform p = transform.parent;
            while (p)
            {
                //TODO
            }

        }


    }
}
