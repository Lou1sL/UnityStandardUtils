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
            if (!go) return null;
            string s = AssetDatabase.GetAssetPath(go);
            if (s == string.Empty) return null;
            return s;
        }
        private static GameObject LoadPrefebPath(string path)
        {
            if (path == null || path == string.Empty || Path.GetExtension(path) != ".prefab") return null;
            GameObject go = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            return go;
        }
    }
}
