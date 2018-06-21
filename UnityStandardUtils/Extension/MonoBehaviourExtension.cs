using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace UnityStandardUtils.Extension
{
    public static class MonoBehaviourExtension
    {
        public delegate bool RTQuitCondition();
        public static void RunThread(this MonoBehaviour monoBehaviour, Action function, RTQuitCondition quitcondition, float delay = 0.02f, Action onend = null)
        {
            monoBehaviour.StartCoroutine(ExecuteAfterTime(monoBehaviour, function, quitcondition, delay, onend));
        }
        private static IEnumerator ExecuteAfterTime(this MonoBehaviour monoBehaviour, Action function, RTQuitCondition quit, float delay = 0.02f, Action onend = null)
        {
            yield return new WaitForSeconds(delay);
            function();
            if (!quit()) monoBehaviour.StartCoroutine(ExecuteAfterTime(monoBehaviour, function, quit, delay, onend));
            else onend?.Invoke();
        }

        public static string GetPrefabPath(GameObject go)
        {
            if (!go) return null;

            PrefabCacher pc = Resources.Load("PrefabLocationCache") as PrefabCacher;
            if (!pc)
            {
                Debug.LogError("Please generate Prefab Cache in editor first!");
            }

            string s = pc.GetPath(go);
            if(s==null)Debug.LogError("Can't find Prefab!");
            return s;
        }

        public static GameObject LoadPrefabPath(string path)
        {
            if (path == null || path == string.Empty) return null;

            string ResourcesPath = "Assets/Resources/";
            string PrefabExtension = ".prefab";
            if (!path.StartsWith(ResourcesPath) || !path.EndsWith(PrefabExtension))
            {
                Debug.LogError("In Build,Unity only support dynamic load in "+ResourcesPath+" folder");
                return null;
            }
            path = path.Replace(ResourcesPath, "");
            path = path.Replace(PrefabExtension, "");


            GameObject go = Resources.Load(path) as GameObject;
            return go;
        }
    }
}
