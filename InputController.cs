using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardUtils
{
    public class InputController
    {

        private static Dictionary<int, KeyCode> DefaultKeymap = new Dictionary<int, KeyCode>();
        private static Dictionary<int, KeyCode> CurrrentKeymap = new Dictionary<int, KeyCode>(DefaultKeymap);



        public static void InitInputController<T>(Dictionary<T, KeyCode> defaultMap)
        {
            DefaultKeymap.Clear();
            CurrrentKeymap.Clear();

            foreach (KeyValuePair<T, KeyCode> kvp in defaultMap)
            {
                if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for Key!");


                int Func = (int)(object)kvp.Key;
                KeyCode KC = kvp.Value;

                DefaultKeymap.Add(Func, KC);
            }

            CurrrentKeymap = new Dictionary<int, KeyCode>(DefaultKeymap);
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
        public static bool GetKey<T>(KeyStatus ks, T kc)
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for Key!");

            KeyCode inputKey;
            bool isContainThisKey = CurrrentKeymap.TryGetValue((int)(object)kc, out inputKey);
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
        public static KeyCode GetKeyCodeByFunc<T>(T kc)
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for Key!");

            KeyCode inputKey;
            CurrrentKeymap.TryGetValue((int)(object)kc, out inputKey);
            return inputKey;
        }
        /// <summary>
        /// 设置某功能的对应键位
        /// </summary>
        /// <param name="k">按键</param>
        /// <param name="kc">功能</param>
        /// <returns>是否成功（如果该键已被使用则返回false）</returns>
        public static bool SetKeyCodeByFunc<T>(KeyCode k, T kc)
        {
            if (!typeof(T).IsEnum) throw new System.ArgumentException("Please use Enum for Key!");

            bool isContainThisValue = CurrrentKeymap.ContainsValue(k);
            if (isContainThisValue) return false;

            CurrrentKeymap[(int)(object)kc] = k;
            return true;
        }
        /// <summary>
        /// 将当前配置还原成默认键位
        /// </summary>
        public static void SetToDefault()
        {
            CurrrentKeymap = new Dictionary<int, KeyCode>(DefaultKeymap);
        }


        /// <summary>
        /// 配置文件格式
        /// </summary>
        private class InputSetting
        {
            public Dictionary<int, KeyCode> SaveKeymap = new Dictionary<int, KeyCode>(DefaultKeymap);
        }

        /// <summary>
        /// 储存当前配置到存档文件
        /// </summary>
        public static void SaveSettings()
        {
            InputSetting setting = new InputSetting();
            setting.SaveKeymap = CurrrentKeymap;

            SaveManager settingSaved = new SaveManager(Application.persistentDataPath, "InputSetting.save");
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


            if (setting.SaveKeymap.Keys.Count == DefaultKeymap.Keys.Count) CurrrentKeymap = setting.SaveKeymap;
            else
            {
                throw new System.ArrayTypeMismatchException("The saved key functions mismatch current key functions");
            }
        }
    }
}