using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityStandardUtilsEditor
{
    public static class ScriptableObjectUtility
    {
        public static bool WriteAsset<T>(T so,string path) where T : ScriptableObject
        {
            if (AssetDatabase.Contains(so)) return false;

            AssetDatabase.CreateAsset(so, path);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = so;

            return true;
        }

        public static T ReadAsset<T>(string path)where T : ScriptableObject
        {
            T t =AssetDatabase.LoadAssetAtPath<T>(path);
            if(!t)t = ScriptableObject.CreateInstance<T>();
            return t;
        }
    }
}
