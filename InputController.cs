﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardUtils
{
    public class InputController:MonoBehaviour
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

        private static Dictionary<KeyCodeMap, KeyCode> KeyCodeSet = new Dictionary<KeyCodeMap, KeyCode>()
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
            Push,
            Release,
            On
        }
        //获取按键状态
        public static bool getKey(KeyStatus ks, KeyCodeMap kc)
        {
            KeyCode inputKey;
            bool isContainThisKey = KeyCodeSet.TryGetValue(kc, out inputKey);
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
        //获得功能对应键
        public static KeyCode getKeyCodeByMap(KeyCodeMap kc)
        {
            KeyCode inputKey;
            bool isContainThisKey = KeyCodeSet.TryGetValue(kc, out inputKey);
            return inputKey;
        }
        //设置功能对应键
        public static bool setKeyCodeByMap(KeyCode k, KeyCodeMap kc)
        {

            bool isContainThisValue = KeyCodeSet.ContainsValue(k);
            if (isContainThisValue) return false;

            KeyCodeSet[kc] = k;
            return true;
        }
        //设置默认
        public static void setToDefault()
        {
            KeyCodeSet = new Dictionary<KeyCodeMap, KeyCode>(KeyCodeMapDefault);
        }
        //储存
        public static void saveSettings(string path)
        {
            InputSetting setting = new InputSetting();
            setting.KeyCodeSet = KeyCodeSet;

            SaveManager settingSaved = new SaveManager(path,"InputSetting.save");
            settingSaved.SetData(setting);
        }
        //读取
        public static void loadSettings(string path)
        {
            InputSetting setting = new InputSetting();

            SaveManager settingSaved = new SaveManager(path, "InputSetting.save");
            settingSaved.GetData(ref setting);

            KeyCodeSet = setting.KeyCodeSet;
        }

    }
}