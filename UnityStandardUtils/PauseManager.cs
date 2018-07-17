using UnityEngine;
using UnityStandardUtils.Extension;

namespace UnityStandardUtils
{
    public class PauseManager:SingletonMonoBehaviour<PauseManager>
    {

        private bool IsPause()
        {
            return Time.timeScale == 0f;
        }

        private void Pause()
        {
            Time.timeScale = 0f;
        }
        private void Resume()
        {
            Time.timeScale = 1f;
        }

        public static void Pause(bool? Status = null)
        {
            if (Status == null)
            {
                if (Instance.IsPause()) Instance.Resume(); 
                else Instance.Pause();

                return;
            }

            if (Status == true) Instance.Pause();
            if (Status == false) Instance.Resume();
        }

        public static bool IsPaused
        {
            get { return Instance.IsPause(); }
        }
    }
}
