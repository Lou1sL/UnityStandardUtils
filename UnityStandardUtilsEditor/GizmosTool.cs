using UnityEngine;
using UnityEditor;

namespace UnityStandardUtilsEditor
{

    public class GizmosTool
    {
        public static void Text(GUISkin guiSkin, string text, Vector3 position, Color? color = null, int fontSize = 0, float yOffset = 0)
        {
            var prevSkin = GUI.skin;
            if (guiSkin == null)
                Debug.LogWarning("editor warning: guiSkin parameter is null");
            else
                GUI.skin = guiSkin;

            GUIContent textContent = new GUIContent(text);

            GUIStyle style = (guiSkin != null) ? new GUIStyle(guiSkin.GetStyle("Label")) : new GUIStyle();
            if (color != null)
                style.normal.textColor = (Color)color;
            if (fontSize > 0)
                style.fontSize = fontSize;

            Vector2 textSize = style.CalcSize(textContent);
            Vector3 screenPoint = Camera.current.WorldToScreenPoint(position);

            if (screenPoint.z > 0) // checks necessary to the text is not visible when the camera is pointed in the opposite direction relative to the object
            {
                var worldPosition = Camera.current.ScreenToWorldPoint(new Vector3(screenPoint.x - textSize.x * 0.5f, screenPoint.y + textSize.y * 0.5f + yOffset, screenPoint.z));
                
                
                Handles.Label(worldPosition, textContent, style);
            }
            GUI.skin = prevSkin;
        }
        public static void Circle(float r,Vector3 center, Color? color=null)
        {
            Gizmos.color = color==null?Color.white:(Color)color;

            Vector3 beginPoint = Vector3.zero;
            Vector3 firstPoint = Vector3.zero;
            for (float theta = 0; theta < 2 * Mathf.PI; theta += 0.1f)
            {
                float x = r * Mathf.Cos(theta) + center.x;
                float y = r * Mathf.Sin(theta) + center.y;
                Vector3 endPoint = new Vector3(x, y, 0);
                if (theta == 0)
                    firstPoint = endPoint;
                else Gizmos.DrawLine(beginPoint, endPoint);
                beginPoint = endPoint;
            }

            Gizmos.DrawLine(firstPoint, beginPoint);
        }
        public static void Capsule2D(Vector3 center,Vector2 size,Vector3 scale, Color? color = null)
        {
            Gizmos.color = color == null ? Color.white : (Color)color;

            float hx = size.x / 2f;
            float hy = size.y / 2f - size.x / 2f;
            if (hx < 0f) hx = 0f;
            if (hy < 0f) hy = 0f;
            hx *= Mathf.Abs(scale.x);
            hy *= Mathf.Abs(scale.y);

            Vector3 RUp = new Vector3(center.x + hx, center.y + hy, center.z);
            Vector3 RLow = new Vector3(center.x + hx, center.y - hy, center.z);

            Vector3 LLow = new Vector3(center.x - hx, center.y - hy, center.z);
            Vector3 LUp = new Vector3(center.x - hx, center.y + hy, center.z);

            //Right Line
            Gizmos.DrawLine(RUp, RLow);
            //Left Line
            Gizmos.DrawLine(LLow, LUp);


            //Upper Half Circle
            Vector3 beginPoint = Vector3.zero;
            for (float theta = 0; theta < Mathf.PI; theta += 0.1f)
            {
                float x = hx * Mathf.Cos(theta) + center.x;
                float y = hx * Mathf.Sin(theta) + center.y + hy;
                Vector3 endPoint = new Vector3(x, y, center.z);
                if(theta != 0) Gizmos.DrawLine(beginPoint, endPoint);
                beginPoint = endPoint;
            }
            Gizmos.DrawLine(LUp, beginPoint);

            //Lower Half Circle
            beginPoint = RLow;
            for (float theta = Mathf.PI; theta < 2 * Mathf.PI; theta += 0.1f)
            {
                float x = hx * Mathf.Cos(theta) + center.x;
                float y = hx * Mathf.Sin(theta) + center.y - hy;
                Vector3 endPoint = new Vector3(x, y, center.z);
                if (theta != Mathf.PI) Gizmos.DrawLine(beginPoint, endPoint);
                beginPoint = endPoint;
            }
            Gizmos.DrawLine(RLow, beginPoint);
        }

    }
}
