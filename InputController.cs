using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardUtils
{
    public class InputController
    {
        public class InputSetting
        {
            public Dictionary<KeyCodeMap, KeyCode> KeyCodeSet = new Dictionary<KeyCodeMap, KeyCode>(KeyCodeMapDefault);
        }
        
        //键盘修改键位设置
        //该死的手柄(问题太多),加不加入支持将来再说
        private static Dictionary<KeyCodeMap, KeyCode> KeyCodeMapDefault = new Dictionary<KeyCodeMap, KeyCode>()
        {
            { KeyCodeMap.Pause          , KeyCode.Escape    },

            { KeyCodeMap.SwitchCharactor, KeyCode.Q         },

            { KeyCodeMap.Left           , KeyCode.A         },
            { KeyCodeMap.Right          , KeyCode.D         },
            { KeyCodeMap.Run            , KeyCode.LeftShift },

            { KeyCodeMap.Interact       , KeyCode.E         },
            { KeyCodeMap.Invent         , KeyCode.Tab       },

            { KeyCodeMap.Torch          , KeyCode.LeftAlt   },
            { KeyCodeMap.HIH            , KeyCode.F         },
            //Xiaoai
            { KeyCodeMap.Crouch         , KeyCode.C         },
            //Xiaofan
            { KeyCodeMap.ClimbUp        , KeyCode.W         },
            { KeyCodeMap.ClimbDown      , KeyCode.S         },
            { KeyCodeMap.Jump           , KeyCode.Space     },

            { KeyCodeMap.GunMode        , KeyCode.Mouse1    },
            { KeyCodeMap.Shoot          , KeyCode.Mouse0    },
            { KeyCodeMap.Reload         , KeyCode.R         },

        };

        private static Dictionary<KeyCodeMap, KeyCode> KeyCodeSetMap = new Dictionary<KeyCodeMap, KeyCode>(KeyCodeMapDefault);

        public enum KeyCodeMap
        {
            Pause,
            SwitchCharactor,
            Left,
            Right,
            Run,
            Interact,
            Invent,
            Torch,
            HIH,
            //Xiaoai
            Crouch,
            //Xiaofan
            ClimbUp,
            ClimbDown,
            Jump,
            GunMode,
            Shoot,
            Reload,

        }

        public enum KeyStatus
        {
            /// <summary>
            /// 按下瞬间的状态
            /// </summary>
            Push,
            /// <summary>
            /// 松开瞬间的状态
            /// </summary>
            Release,
            /// <summary>
            /// 在被按住的状态
            /// </summary>
            On
        }

        /// <summary>
        /// 判断一个功能的对应键是否处于某个状态
        /// </summary>
        /// <param name="ks">状态</param>
        /// <param name="kc">功能</param>
        /// <returns></returns>
        public static bool GetKey(KeyStatus ks, KeyCodeMap kc)
        {
            KeyCode inputKey;
            bool isContainThisKey = KeyCodeSetMap.TryGetValue(kc, out inputKey);
            bool result = false;

            if (isContainThisKey)
            {
                switch (ks)
                {
                    case KeyStatus.Push: result = Input.GetKeyDown(inputKey); break;
                    case KeyStatus.Release: result = Input.GetKeyUp(inputKey); break;
                    case KeyStatus.On: result = Input.GetKey(inputKey); break;
                    default: break;
                }
            }
            return result;
        }
        /// <summary>
        /// 获取当前设置下某功能对应的按键
        /// </summary>
        /// <param name="kc"></param>
        /// <returns></returns>
        public static KeyCode GetKeyCodeByMap(KeyCodeMap kc)
        {
            KeyCode inputKey;
            bool isContainThisKey = KeyCodeSetMap.TryGetValue(kc, out inputKey);
            return inputKey;
        }
        /// <summary>
        /// 设置某功能的对应键位
        /// </summary>
        /// <param name="k">按键</param>
        /// <param name="kc">功能</param>
        /// <returns>是否成功（如果该键已被使用则返回false）</returns>
        public static bool SetKeyCodeByMap(KeyCode k, KeyCodeMap kc)
        {

            bool isContainThisValue = KeyCodeSetMap.ContainsValue(k);
            if (isContainThisValue) return false;

            KeyCodeSetMap[kc] = k;
            return true;
        }
        /// <summary>
        /// 将当前配置还原成默认键位
        /// </summary>
        public static void SetToDefault()
        {
            KeyCodeSetMap = new Dictionary<KeyCodeMap, KeyCode>(KeyCodeMapDefault);
        }
        /// <summary>
        /// 储存当前配置到存档文件
        /// </summary>
        public static void SaveSettings()
        {
            InputSetting setting = new InputSetting();
            setting.KeyCodeSet = KeyCodeSetMap;

            SaveManager settingSaved = new SaveManager(Application.persistentDataPath,"InputSetting.save");
            settingSaved.SetData(setting);
        }
        /// <summary>
        /// 从存档文件中读取配置
        /// </summary>
        public static void LoadSettings()
        {
            InputSetting setting = new InputSetting();

            SaveManager settingSaved = new SaveManager(Application.persistentDataPath, "InputSetting.save");
            settingSaved.GetData(ref setting);

            KeyCodeSetMap = setting.KeyCodeSet;
        }

    }
}
