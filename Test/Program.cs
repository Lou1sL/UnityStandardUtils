using System;

using UnityStandardUtils;

namespace Test
{
    class Program
    {
        class TestClass
        {
            private int integer = 0;

            public int Int
            {
                get
                {
                    return integer;
                }
                set
                {
                    integer = value;
                }
            }
        }

        static void Main(string[] args)
        {
            Localization.Init(Environment.CurrentDirectory + "/Localization/", "en-US");

            foreach(Localization.LanguageSet set in Localization.GetAllSupportedLanguages)
                Console.WriteLine(set);

            Console.WriteLine(Localization.Call("LOGO_SKIP"));
            Localization.SetLanguage("zh-CN");
            Console.WriteLine(Localization.Call("LOGO_SKIP"));
            Localization.SetLanguage("zh-TW");
            Console.WriteLine(Localization.Call("LOGO_SKIP"));
            //错误的id call不会触发异常，但会返回TextCallFailed!
            Console.WriteLine(Localization.Call("LOGO_SKIPzzz"));
            Console.ReadLine();
        }
        
    }
}
