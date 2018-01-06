using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityStandardUtils;

namespace LocalizationEditor
{
    public static class Editor
    {
        public struct LocalizationDataPair
        {
            public string ID;
            public string Text;
        }


        public static Localization.LanguageSet LoadSetFromFile(string file)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(file);
            XmlNode root = doc.SelectSingleNode("Localization");

            Localization.LanguageSet set = new Localization.LanguageSet();
            set.FileName = Path.GetFileNameWithoutExtension(file);
            set.Language = root.Attributes["language"].Value;
            set.Data = root;

            return set;
        }

        public static void SaveSetToFile(List<LocalizationDataPair> data,string language,string file)
        {
            XmlDocument xmlDoc = new XmlDocument();
            
            XmlNode rootNode = xmlDoc.CreateElement("Localization");

            XmlAttribute attr = xmlDoc.CreateAttribute("language");
            attr.Value = language;

            rootNode.Attributes.Append(attr);

            xmlDoc.AppendChild(rootNode);
            xmlDoc.InsertBefore(xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null),rootNode);
            foreach (LocalizationDataPair p in data)
            {
                XmlNode cn = xmlDoc.CreateElement(p.ID);

                XmlAttribute cns = xmlDoc.CreateAttribute("str");
                cns.Value = p.Text;
                cn.Attributes.Append(cns);

                rootNode.AppendChild(cn);
            }

            
            xmlDoc.Save(file);

        }


        public static List<LocalizationDataPair> GetAllData(XmlNode root)
        {
            List<LocalizationDataPair> l = new List<LocalizationDataPair>();

            foreach (XmlNode cn in root.ChildNodes)
            {
                LocalizationDataPair data = new LocalizationDataPair();
                data.ID = cn.Name;
                data.Text = cn.Attributes["str"].Value;
                l.Add(data);
            }
            return l;
        }

        
        public static string FindText(XmlNode root,string id)
        {
            try
            {
                return root.SelectSingleNode(id).Attributes["str"].Value;
            }catch
            {
                return string.Empty;
            }
        }

    }
}
