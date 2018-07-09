using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace UnityStandardUtils
{
    public static class Localization
    {
        private static string Extension = ".xml";

        private static List<LanguageSet> AllSet;
        private static LanguageSet CurrentLanguage;


        static Localization()
        {
            try
            {
                Init(Environment.CurrentDirectory + "/StreamingAssets/Localization/", "zh-CN");
            }catch(Exception e)
            {
                Console.WriteLine("Localization default Init FAILED\nPlease Init manually:)\n"+e.Message);
            }
            
        }

        public static void Init(string folder,string defaultFile)
        {
            AllSet = new List<LanguageSet>();
            CurrentLanguage = new LanguageSet();

            DirectoryInfo DirInfo = new DirectoryInfo(folder);

            foreach (FileInfo file in DirInfo.GetFiles())
            {
                if (file.Extension == Extension)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file.FullName);
                    XmlNode root = doc.SelectSingleNode("Localization");

                    LanguageSet set = new LanguageSet();
                    set.FileName = Path.GetFileNameWithoutExtension(file.Name);
                    set.Language = root.Attributes["language"].Value;
                    set.Data = root;

                    AllSet.Add(set);
                }
            }
            
            SetLanguage(defaultFile);
        }

        public static List<LanguageSet> GetAllSupportedLanguages
        {
            get
            {
                return AllSet;
            }
        }

        public static void SetLanguage(string filename)
        {
            foreach(LanguageSet set in AllSet)
            {
                if (set.FileName == filename)
                {
                    CurrentLanguage = set;
                    return;
                }
            }
            throw new FileNotFoundException();
        }

        public struct LanguageSet
        {
            public string FileName;
            public string Language;
            public XmlNode Data;

            public override string ToString()
            {
                return FileName + " : " + Language;
            }
        }

        
        public static string Call(string id)
        {
            try
            {
                return CurrentLanguage.Data.SelectSingleNode(id).Attributes["str"].Value;
            }catch(Exception e)
            {
                return "TextCallFailed!@" + e.Message;
            }
        }
        
    }
}
