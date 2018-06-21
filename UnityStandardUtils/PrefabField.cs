using UnityEngine;
using UnityStandardUtils.Extension;

namespace UnityStandardUtils
{
    [System.Serializable]
    public class PrefabField
    {
        [SerializeField]
        private string prefabPath = string.Empty;


        public PrefabField() { }
        public PrefabField(string path) { prefabPath = path; }
        public PrefabField(GameObject go) { gameObject = go; }

        public static bool operator ==(PrefabField obj1, PrefabField obj2)
        {
            if (object.ReferenceEquals(obj1, null))
            {
                return object.ReferenceEquals(obj2, null);
            }

            return obj1.Equals(obj2);
        }
        public static bool operator !=(PrefabField obj1, PrefabField obj2)
        {
            if (object.ReferenceEquals(obj1, null))
            {
                return !object.ReferenceEquals(obj2, null);
            }
            return obj1.Equals(obj2);
        }

        public static implicit operator string(PrefabField p)
        {
            return p.prefabPath;
        }

        public static implicit operator GameObject(PrefabField p)
        {
            return MonoBehaviourExtension.LoadPrefabPath(p.prefabPath);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return path == ((PrefabField)obj).path;
        }


        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            throw new System.NotImplementedException();
            return base.GetHashCode();
        }
        
        public string path
        {
            get
            {
                return prefabPath;
            }
            set
            {
                prefabPath = value;
            }
        }

        public GameObject gameObject
        {
            get
            {
                return MonoBehaviourExtension.LoadPrefabPath(prefabPath);
            }
            set
            {
                prefabPath = MonoBehaviourExtension.GetPrefabPath(value);
            }
        }

    }

}
