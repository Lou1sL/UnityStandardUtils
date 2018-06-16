using UnityEngine;


namespace UnityStandardUtils
{
    public class VecTool
    {

        public static Matrix4x4 ScreenToWorldMatrix(Camera cam)
        {
            // Make a matrix that converts from
            // screen coordinates to clip coordinates.
            var rect = cam.pixelRect;
            var viewportMatrix = Matrix4x4.Ortho(rect.xMin, rect.xMax, rect.yMin, rect.yMax, -1, 1);

            // The camera's view-projection matrix converts from world coordinates to clip coordinates.
            var vpMatrix = cam.projectionMatrix * cam.worldToCameraMatrix;

            // Setting column 2 (z-axis) to identity makes the matrix ignore the z-axis.
            // Instead you get the value on the xy plane!
            vpMatrix.SetColumn(2, new Vector4(0, 0, 1, 0));

            // Going from right to left:
            // convert screen coords to clip coords, then clip coords to world coords.
            return vpMatrix.inverse * viewportMatrix;
        }

        public static Vector2 ScreenToWorldPointPerspective(Vector2 point)
        {
            return ScreenToWorldMatrix(Camera.main).MultiplyPoint(point);
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
