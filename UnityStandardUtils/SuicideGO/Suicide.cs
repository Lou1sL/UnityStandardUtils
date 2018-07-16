using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityStandardUtils
{

    public class Suicide : MonoBehaviour
    {
        [HideInInspector]
        public float LifeTime = 15f;

        private void Start()
        {
            Invoke("Goodbye", LifeTime);
        }

        public void Goodbye()
        {
            Destroy(gameObject);
        }
        
    }

}
