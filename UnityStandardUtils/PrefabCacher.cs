using UnityEngine;
using System.Collections.Generic;

namespace UnityStandardUtils
{
    public class PrefabCacher : ScriptableObject
    {
        [System.Serializable]
        public class PrefabCache
        {
            public string path;
            public GameObject gameObject;
        }

        [SerializeField]
        private List<PrefabCache> AllPrefab = new List<PrefabCache>();


        public GameObject GetPrefab(string path)
        {
            foreach (PrefabCache pc in AllPrefab)
            {
                if (pc.path == path) return pc.gameObject;
            }
            return null;
        }

        public string GetPath(GameObject go)
        {
            foreach (PrefabCache pc in AllPrefab)
            {
                if (ReferenceEquals(go, pc.gameObject)) return pc.path;
            }
            return null;
        }

        public void UpdateCache(List<PrefabCache> pc)
        {
            AllPrefab = pc;
        }

        public List<PrefabCache> GetAllPrefab()
        {
            if (AllPrefab.Count > 0)
                for (int i = 0; i < AllPrefab.Count; i++)
                {
                    if (!AllPrefab[i].gameObject)
                    {
                        AllPrefab.RemoveAt(i);
                        i--;
                    }
                }

            return AllPrefab;
        }
    }



}
