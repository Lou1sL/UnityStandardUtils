using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityStandardUtils
{
    public static class SceneLoader
    {
        private static Dictionary<string, object> DataHolder = new Dictionary<string, object>();
        
        public static T GetData<T>(string Name)
        {
            object val = null;
            DataHolder.TryGetValue(Name, out val);
            return (T)val;
        }

        public static Dictionary<string, object> GetAllData()
        {
            return DataHolder;
        }

        public static void LoadScene(string SceneName, Dictionary<string, object> PassData)
        {
            DataHolder.Clear();
            DataHolder = PassData;
            
            SceneManager.LoadScene(SceneName);
        }

        public static AsyncOperation LoadSceneAsync(string SceneName, Dictionary<string, object> PassData)
        {
            DataHolder.Clear();
            DataHolder = PassData;

            return SceneManager.LoadSceneAsync(SceneName);
        }
    }
}
