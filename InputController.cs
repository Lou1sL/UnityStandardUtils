using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardUtils
{
    public class InputController
    {
        //功能键
        public enum Func
        {
            Left,
            Right,
        }

        //默认键位设置
        private static Dictionary<Func, KeyCode> KeyCodeMapDefault = new Dictionary<Func, KeyCode>()
        {
            { Func.Left           , KeyCode.A         },
            { Func.Right          , KeyCode.D         },
        };


        //当前配置
        private static Dictionary<Func, KeyCode> KeyCodeSetMap = new Dictionary<Func, KeyCode>(KeyCodeMapDefault);

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
        public static bool GetKey(KeyStatus ks, Func kc)
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
        public static KeyCode GetKeyCodeByFunc(Func kc)
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
        public static bool SetKeyCodeByFunc(KeyCode k, Func kc)
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
            KeyCodeSetMap = new Dictionary<Func, KeyCode>(KeyCodeMapDefault);
        }



        public class InputSetting
        {
            public Dictionary<Func, KeyCode> KeyCodeSet = new Dictionary<Func, KeyCode>(KeyCodeMapDefault);
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
