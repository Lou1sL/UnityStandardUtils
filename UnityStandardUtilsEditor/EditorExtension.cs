using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace UnityStandardUtilsEditor
{


    public static class EditorExtension
    {
        private static string GetPrefebPath(GameObject go)
        {
            if (!go) return string.Empty;
            return AssetDatabase.GetAssetPath(go);
        }
        private static GameObject LoadPrefebPath(string path)
        {
            if (path == string.Empty || Path.GetExtension(path) != ".prefab") return null;
            GameObject go = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            return go;
        }
    }
}
