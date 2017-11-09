using System;
using System.IO;
using System.Security.Cryptography;

namespace UnityStandardUtils
{
    /// <summary>
    /// 存档工具类
    /// </summary>
    public class SaveManager
    {

        private string Path;
        private string FileName;
        private string PassSeed;

        public enum GetDataReturnCode
        {
            Success,
            FileNotExist,
            WrongDecryptCode,
        }

        /// <summary>
        /// 不加密的存档保存
        /// </summary>
        /// <param name="path">存档路径</param>
        /// <param name="fileName">存档文件名</param>
        public SaveManager(string path, string fileName)
        {
            Path = path;
            if (Path[Path.Length - 1].ToString() != @"/") Path += @"/";
            FileName = fileName;
            PassSeed = string.Empty;
        }

        /// <summary>
        /// 支持AES加密的存档储存
        /// </summary>
        /// <param name="path">存档路径</param>
        /// <param name="fileName">存档文件名</param>
        /// <param name="passSeed">密码种子</param>
        public SaveManager(string path, string fileName, string passSeed)
        {
            Path = path;
            if (Path[Path.Length - 1].ToString() != @"/") Path += @"/";
            FileName = fileName;
            PassSeed = passSeed;
        }

        /// <summary>
        /// 保存存档
        /// </summary>
        /// <param name="pObject">数据对象</param>
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

        /// <summary>
        /// 读取存档
        /// </summary>
        /// <typeparam name="T">存档类</typeparam>
        /// <param name="defaultObj">数据对象</param>
        /// <returns>
        /// </returns>
        public GetDataReturnCode GetData<T>(ref T defaultObj)
        {
            //存档不存在
            if (!File.Exists(Path + FileName)) return GetDataReturnCode.FileNotExist;

            StreamReader streamReader = File.OpenText(Path + FileName);
            string data = streamReader.ReadToEnd();
            //对数据进行解密，32位解密密钥
            if (PassSeed != string.Empty)
            {
                try
                {
                    data = Crypto.RijndaelDecrypt(data, Crypto.MD5(PassSeed));
                }
                catch (CryptographicException e)
                {
#if DEBUG
                    Console.WriteLine(e);
#endif
                    //密码错误
                    return GetDataReturnCode.WrongDecryptCode;
                }
            }
            streamReader.Close();
            defaultObj = (T)Crypto.DeserializeObject(data, typeof(T));

            return GetDataReturnCode.Success;
        }



    }
}
