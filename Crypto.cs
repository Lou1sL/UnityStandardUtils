using System;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace UnityStandardUtils
{
    /// <summary>
    /// 加解密类，所有编码采用UTF8
    /// </summary>
    public static class Crypto
    {

        /// <summary>
        /// Rijndael加密算法
        /// </summary>
        /// <param name="pString">待加密的明文</param>
        /// <param name="pKey">密钥,长度可以为:64位(byte[8]),128位(byte[16]),192位(byte[24]),256位(byte[32])</param>
        /// <param name="iv">iv向量,长度为128（byte[16])</param>
        /// <returns></returns>
        public static string RijndaelEncrypt(string pString, string pKey)
        {
            //密钥
            byte[] keyArray = Encoding.UTF8.GetBytes(pKey);
            //待加密明文数组
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(pString);

            //Rijndael解密算法
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();

            //返回加密后的密文
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// Rijndael解密算法
        /// </summary>
        /// <param name="pString">待解密的密文</param>
        /// <param name="pKey">密钥,长度可以为:64位(byte[8]),128位(byte[16]),192位(byte[24]),256位(byte[32])</param>
        /// <param name="iv">iv向量,长度为128（byte[16])</param>
        /// <returns></returns>
        public static string RijndaelDecrypt(string pString, string pKey)
        {
            //解密密钥
            byte[] keyArray = Encoding.UTF8.GetBytes(pKey);
            //待解密密文数组
            byte[] toEncryptArray = Convert.FromBase64String(pString);

            //Rijndael解密算法
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();

            //返回解密后的明文
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }
        

        /// <summary>
        /// 将一个对象序列化为字符串
        /// </summary>
        /// <returns>The object.</returns>
        /// <param name="pObject">对象</param>
        public static string SerializeObject(object pObject)
        {
            //序列化后的字符串
            string serializedString = string.Empty;
            //使用Json.Net进行序列化
            serializedString = JsonConvert.SerializeObject(pObject);
            return serializedString;
        }

        /// <summary>
        /// 将一个字符串反序列化为对象
        /// </summary>
        /// <returns>The object.</returns>
        /// <param name="pString">字符串</param>
        /// <param name="pType">对象类型</param>
        public static object DeserializeObject(string pString, Type pType)
        {
            //反序列化后的对象
            object deserializedObject = null;
            //使用Json.Net进行反序列化
            deserializedObject = JsonConvert.DeserializeObject(pString, pType);
            return deserializedObject;
        }


        /// <summary>
        /// 计算MD5校验值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5(string str)
        {
            string cl = str;
            String pwd = "";
            System.Security.Cryptography.MD5 md5 =  System.Security.Cryptography.MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                pwd = pwd + s[i].ToString("x2");
            }
            return pwd;
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Encode(string str)
        {
            if (str == string.Empty) return string.Empty;
            byte[] b = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(b);
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Base64Decode(string str)
        {
            if (str == string.Empty) return string.Empty;
            byte[] b = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(b);
        }


    }



}
