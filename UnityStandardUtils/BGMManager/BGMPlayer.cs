using UnityEngine;
using UnityStandardUtils.Extension;

namespace UnityStandardUtils
{
    [RequireComponent(typeof(AudioSource))]
    public class BGMPlayer : SingletonMonoBehaviour<BGMPlayer>
    {
        [SerializeField]
        private BGMListCacher BGMList;
        private AudioSource au;
        
        protected override void Init()
        {
            BGMList = Resources.Load("BGMCache") as BGMListCacher;
            au = GetComponent<AudioSource>();
            au.clip = null;
            au.playOnAwake = true;
            au.loop = true;
        }

        public static void Play(string tag)
        {
            foreach (BGMListCacher.BGMCache bc in Instance.BGMList.BGM)
            {
                if (bc.Tag == tag)
                {
                    Instance.au.clip = bc.Audio;
                    Stop();
                    Play();
                    return;
                }
            }

            Debug.LogError("BGM Tag Not Found!");
        }

        public static void Play()
        {
            Instance.au.Play();
        }

        public static void Pause()
        {
            Instance.au.Pause();
        }

        public static void Stop()
        {
            Instance.au.Stop();
        }

        public static float Volume
        {
            get { return Instance.au.volume; }
            set { Instance.au.volume = value; }
        }

        public static AudioSource audioSource
        {
            get { return Instance.au; }
        }
    }
}