using UnityEngine;

namespace UnityStandardUtils
{
    public class Singleton<T> where T : new()
    {
        static protected T sInstance;
        static protected bool IsCreate = false;

        public static T Instance
        {
            get
            {
                if (IsCreate == false)
                {
                    CreateInstance();
                }

                return sInstance;
            }
        }

        public static void CreateInstance()
        {
            if (IsCreate == true)
                return;

            IsCreate = true;
            sInstance = new T();
        }

        public static void ReleaseInstance()
        {
            sInstance = default(T);
            IsCreate = false;
        }
    }

    //abstract?
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        protected static T sInstance = null;
        protected static bool IsCreate = false;
        //这个NeedDestory是不是有点奇怪？
        //是这样滴：在UnityEditor里停止游戏的时候，GameObject会以随机顺序Destory掉
        //这样子，有可能导致使用该单例的脚本所挂载的GameObject已经被Destory掉了，但是调用Instance的另一个脚本还在执行
        //结果就又产生出来了一个Instance
        //从而在控制台产生"Some objects were not cleaned up when closing the scene"的报错
        public static bool NeedDestroy = false;
        public static T Instance
        {
            get
            {
                //
                if (NeedDestroy)
                {
                    return null;
                }
                CreateInstance();
                return sInstance;
            }
        }

        protected virtual void Awake()
        {
            if (sInstance == null)
            {
                sInstance = this as T;
                IsCreate = true;

                Init();
            }
        }

        protected virtual void Init(){}

        protected virtual void OnDestroy()
        {
            sInstance = null;
            IsCreate = false;
            NeedDestroy = true;
        }

        private void OnApplicationQuit()
        {
            sInstance = null;
            IsCreate = false;
            NeedDestroy = true;
        }

        public static void CreateInstance()
        {
            if (IsCreate == true)
                return;

            IsCreate = true;
            T[] managers = GameObject.FindObjectsOfType(typeof(T)) as T[];
            if (managers.Length != 0)
            {
                if (managers.Length == 1)
                {
                    sInstance = managers[0];
                    sInstance.gameObject.name = typeof(T).Name;
                    DontDestroyOnLoad(sInstance.gameObject);
                    return;
                }
                else
                {
                    foreach (T manager in managers)
                    {
                        Destroy(manager.gameObject);
                    }
                }
            }

            GameObject gO = new GameObject(typeof(T).Name, typeof(T));
            sInstance = gO.GetComponent<T>();
            DontDestroyOnLoad(sInstance.gameObject);
        }

        public static void ReleaseInstance()
        {
            if (sInstance != null)
            {
                Destroy(sInstance.gameObject);
                sInstance = null;
                IsCreate = false;
            }
        }
    }

}
