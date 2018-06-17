using UnityEditor;
using UnityEngine;
using UnityStandardUtils;

namespace UnityStandardUtilsEditor
{

    [CustomPropertyDrawer(typeof(PrefabField))]
    public class PrefabFieldPropertyDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);

            var prefabPath = property.FindPropertyRelative("prefabPath");

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            EditorGUI.BeginChangeCheck();

            var value = EditorExtension.GetPrefabPath((GameObject)EditorGUI.ObjectField(position, EditorExtension.LoadPrefabPath(prefabPath.stringValue), typeof(GameObject), false));


            if (EditorGUI.EndChangeCheck())
            {
                prefabPath.stringValue = value;
            }

            EditorGUI.EndProperty();
        }



    }

}
