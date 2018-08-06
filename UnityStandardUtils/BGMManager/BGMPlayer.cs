using UnityEngine;
using UnityStandardUtils.Extension;

namespace UnityStandardUtils
{
    [RequireComponent(typeof(AudioSource))]
    public class BGMPlayer : SingletonMonoBehaviour<BGMPlayer>
    {
        public enum PlayerStatus
        {
            Playing,
            Paused,
            Stopped,
        }
        public static PlayerStatus Status { get; private set; } = PlayerStatus.Stopped;
        public static string PlayingTag { get; private set; } = string.Empty;
        [SerializeField]
        private BGMListCacher BGMList;
        private AudioSource au;
        
        protected override void Init()
        {
            BGMList = Resources.Load("BGMCache") as BGMListCacher;
            au = GetComponent<AudioSource>();
            
            au.clip = null;
            au.playOnAwake = false;
            au.loop = true;
            au.Stop();
            au.volume = 0f;

        }

        public static void Play(string tag,bool isReplayIfSame = true)
        {


            foreach (BGMListCacher.BGMCache bc in Instance.BGMList.BGM)
            {
                if (bc.Tag == tag)
                {
                    if (!isReplayIfSame)
                    {
                        if (Instance.au.clip == bc.Audio && bc.Audio != null)
                        {
                            Debug.Log("Calling same BGM,not going to replay it");
                            return;
                        }
                    }
                    Instance.au.volume = 0f;
                    Instance.au.clip = bc.Audio;
                    Status = PlayerStatus.Playing;

                    PlayingTag = tag;

                    return;
                }
            }

            Debug.LogError("BGM Tag Not Found!");
        }

        /// <summary>
        /// 继续播放
        /// </summary>
        public static void Play()
        {
            Status = PlayerStatus.Playing;
        }

        /// <summary>
        /// 暂停播放
        /// </summary>
        public static void Pause()
        {
            Status = PlayerStatus.Paused;
        }

        /// <summary>
        /// 停止播放并清理当前音乐clip
        /// </summary>
        public static void Stop()
        {
            Status = PlayerStatus.Stopped;
        }

        //Editor Crash
        //public static float Volume
        //{
        //    get { return Volume; }
        //    set { Volume = value; Instance.au.volume = value; }
        //}
        private static float _Volume;
        public static float Volume
        {
            get { return _Volume; }
            set { _Volume = value; }
        }

        public static AudioSource audioSource
        {
            get { return Instance.au; }
        }

        private void Update()
        {
            switch (Status)
            {
                case PlayerStatus.Playing:

                    if (!au.isPlaying) au.Play();
                    au.volume = _Volume;

                    break;

                case PlayerStatus.Paused:

                    if (au.volume > 0f) au.volume -= 0.5f * Time.deltaTime;
                    else
                    {
                        au.Pause();
                        au.volume = 0f;
                    }

                    break;

                case PlayerStatus.Stopped:

                    PlayingTag = string.Empty;

                    if (au.volume > 0f) au.volume -= 0.5f * Time.deltaTime;
                    else
                    {
                        au.Stop();
                        au.clip = null;
                        au.volume = 0f;
                    }


                    break;
            }
        }
    }
}