using System.Collections;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace UnityStandardUtils
{
    public class SaveManager
    {
        
        private string Path;
        private string FileName;
        private string PassSeed;

        public SaveManager(string path, string fileName)
        {
            Path = path;
            FileName = fileName;
            PassSeed = string.Empty;
        }
        public SaveManager(string path, string fileName, string passSeed)
        {
            Path = path;
            FileName = fileName;
            PassSeed = passSeed;
        }

        public void SetData(object pObject)
        {
            //生成存档路径
            if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);

            //将对象序列化为字符串
            string toSave = Crypto.SerializeObject(pObject);
            //对字符串进行加密,32位加密密钥
            if(PassSeed!= string.Empty) toSave = Crypto.RijndaelEncrypt(toSave, Crypto.MD5(PassSeed));
            StreamWriter streamWriter = File.CreateText(Path + FileName);
            streamWriter.Write(toSave);
            streamWriter.Close();
        }

        public int GetData(ref object defaultObj)
        {
            //存档不存在
            if (!File.Exists(Path + FileName))
            {
                return -1;
            }

            StreamReader streamReader = File.OpenText(Path + FileName);
            string data = streamReader.ReadToEnd();
            //对数据进行解密，32位解密密钥
            if (PassSeed != string.Empty) data = Crypto.RijndaelDecrypt(data,Crypto.MD5(PassSeed));
            streamReader.Close();
            defaultObj = Crypto.DeserializeObject(data, defaultObj.GetType());
            return 0;
        }



    }
}
