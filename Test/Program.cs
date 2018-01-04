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

            Localization localization = new Localization(Environment.CurrentDirectory + "/Localization/","en-US");

            foreach(Localization.LanguageSet set in localization.GetAllSupportedLanguages)
            {
                Console.WriteLine(set.FileName+":"+set.Language);
            }

            Console.WriteLine();
            Console.WriteLine(localization.Call("LOGO_SKIP"));
            localization.SetLanguage("zh-CN");
            Console.WriteLine(localization.Call("LOGO_SKIP"));
            localization.SetLanguage("zh-TW");
            Console.WriteLine(localization.Call("LOGO_SKIP"));
            Console.ReadLine();
        }
        
    }
}
