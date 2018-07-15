using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityStandardUtilsEditor.Extension
{
    public static class ScriptableObjectExtension
    {
        public static bool DeleteAsset(string path)
        {
            return AssetDatabase.DeleteAsset(path);
        }


        public static bool WriteAsset<T>(T so,string path) where T : ScriptableObject
        {
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

        public static void SaveAsset()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            AssetDatabase.Refresh();
        }
    }
}
