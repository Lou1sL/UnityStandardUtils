using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardUtils
{
    public class BGMListCacher : ScriptableObject
    {
        [System.Serializable]
        public class BGMCache
        {
            public AudioClip Audio;
            public string Tag;
        }
        public List<BGMCache> BGM = new List<BGMCache>();
    }
}