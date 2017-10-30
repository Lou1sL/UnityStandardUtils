using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            //Settings
            string encryptSeed = "seed";
            string savePath = Environment.CurrentDirectory;
            string fileName = @"/SaveTest";

            // 假设某任意结构存档类型为TestClass(类名，结构均不限)
            TestClass testClass1 = new TestClass();
            testClass1.Int = 5;
            TestClass testClass2 = new TestClass();
            testClass2.Int = 10;

            //不进行加密的存取类
            SaveManager saveManager = new SaveManager(savePath, fileName);
            //进行加密的存取类
            SaveManager saveManagerEncrypted = new SaveManager(savePath,fileName,encryptSeed);

            //储存testClass1对象中的数据到文件

            saveManagerEncrypted.SetData(testClass1);
            //读档，读出的数据会存到testClass2的对象中
            SaveManager.GetDataReturnCode rtnCode = saveManagerEncrypted.GetData(ref testClass2);


            Console.WriteLine("test1:"+testClass1.Int);
            Console.WriteLine("test2:"+testClass2.Int);


            //----------

            Web.HttpRequest httpRequest = new Web.HttpRequest();
            httpRequest.SetUrl("www.revokedstudio.com");
            httpRequest.SetRequestType(Web.HttpRequest.RequestType.POST);
            httpRequest.AddParam(new Web.HttpRequest.ParamPair("name", "ryubai"));
            httpRequest.AddParam(new Web.HttpRequest.ParamPair("age", "ihavenoidea"));

            string str = string.Empty;
            httpRequest.SendRequest(ref str);
            Console.WriteLine(str);

            httpRequest.Download(Environment.CurrentDirectory + @"/saved.html");

            Console.WriteLine("Download Finished");

            Console.ReadLine();
        }
    }
}
