using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardUtils
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
    }
}
