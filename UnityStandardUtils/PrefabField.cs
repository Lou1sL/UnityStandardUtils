using UnityEngine;

namespace UnityStandardUtils
{
    [System.Serializable]
    public class PrefabField
    {
        [SerializeField]
        private string prefabPath = string.Empty;

        public string PrefabPath
        {
            get
            {
                return prefabPath;
            }
        }

        public static implicit operator string(PrefabField p)
        {
            return p.prefabPath;
        }

        public void SetPath(string s)
        {
            prefabPath = s;
        }
    }

}
