using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardUtils
{

    [RequireComponent(typeof(Scrollbar))]
    public class AutoScrollbar : MonoBehaviour
    {

        public bool ReScroll = true;
        public float ScrollTime = 20f;

        private Scrollbar sb;
        private void Awake()
        {
            sb = GetComponent<Scrollbar>();
            sb.value = 1f;
        }

        private void Update()
        {
            sb.value -= Time.deltaTime / ScrollTime;

            if (ReScroll && sb.value <= 0f) sb.value = 1f;
        }

        private void OnEnable()
        {
            sb.value = 1f;
        }

    }

}
