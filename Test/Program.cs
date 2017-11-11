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
            /**
            //----------SaveManager
            //Settings
            string encryptSeed = "seed";
            string savePath = Environment.CurrentDirectory;
            string fileName = "SaveTest";

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
            

            //----------Web
            Web.HttpRequest httpRequest = new Web.HttpRequest();
            httpRequest.SetUrl("www.revokedstudio.com");
            httpRequest.SetRequestType(Web.HttpRequest.RequestType.GET);
            httpRequest.AddParam(new Web.HttpRequest.ParamPair("name", "ryubai"));
            httpRequest.AddParam(new Web.HttpRequest.ParamPair("age", "ihavenoidea"));

            string str = string.Empty;
            httpRequest.SendRequest(ref str);
            Console.WriteLine(str);

            httpRequest.Download(Environment.CurrentDirectory + @"/saved.html");

            Console.WriteLine("Download Finished");

            //-----------InventoryManager
            const ushort TOTAL_AMOUNT = 3;
            //游戏中一共有哪些物品
            object[,] items = new object[TOTAL_AMOUNT,3]
            {
                //物品名称 物品介绍 物品重量
                { "Item0","Call me item0!",1 },
                { "Item1","Call me item1!",1 },
                { "Item2","Call me item2!",1 },
            };
            
            const ushort COMBINATION_AMOUNT = 2;
            //根据物品数组下标制定合成表
            ushort[,] combines = new ushort[COMBINATION_AMOUNT,3]
            {
                //1与2合成产出0
                { 1,2,0 },
                //2与0合成产出1
                { 2,0,1 },
            };
            
            //建立物品管理器对象并绑定物品及合成表
            InventoryManager ivtMgr = new InventoryManager();
            ivtMgr.AddItem(items);
            ivtMgr.AddCombination(combines);
            //建立玩家的背包对象
            InventoryManager.Bag playerBag = new InventoryManager.Bag(8,ivtMgr);
            //添加一些物品
            playerBag.Push(0);
            playerBag.Push(1);
            playerBag.Push(1);
            playerBag.Push(2);
            playerBag.Push(2);
            playerBag.Push(2);
            Console.WriteLine(playerBag.ToString());
            //删掉特定的物品
            playerBag.PopByGlobalPosition(1);
            Console.WriteLine(playerBag.ToString());
            playerBag.Push(1);
            Console.WriteLine(playerBag.ToString());
            //删除背包的某项物品
            playerBag.Pop(0);
            Console.WriteLine(playerBag.ToString());
            //尝试合成
            playerBag.TryCombineThenPush(2, 3);
            Console.WriteLine(playerBag.ToString());
            playerBag.TryCombineThenPush(2, 1);
            Console.WriteLine(playerBag.ToString());
            playerBag.TryCombineThenPush(0, 1);
            Console.WriteLine(playerBag.ToString());

            /**
            //-----------InputController
            //读取设置文件到当前配置
            InputController.LoadSettings();
            //设定Pause功能为P键
            InputController.SetKeyCodeByMap(KeyCode.P, InputController.KeyCodeMap.Pause);
            //设定当前全部功能回到默认配置
            InputController.SetToDefault();
            //保存当前配置到配置文件
            InputController.SaveSettings();
            //获得Pause功能对应的按键
            KeyCode pauseCode = InputController.GetKeyCodeByMap(InputController.KeyCodeMap.Pause);
            //判断Pause功能对应按键是否刚刚按下
            bool isPressed = InputController.GetKey(InputController.KeyStatus.Push, InputController.KeyCodeMap.Pause);

            **/

            //随机数生成器
            RandomGenerator random = new RandomGenerator();
            double[] res = new double[1000];
            
            double min = 0;
            double max = 1;
            //平均分布
            for (int i = 0; i < res.Length; i++) res[i] = random.AverageRandom(min, max);
            Array.Sort(res);
            //WriteScr(res);

            double lambda = 1;
            //指数分布
            for (int i = 0; i < res.Length; i++) res[i] = random.ExponentialDist(lambda);
            Array.Sort(res);
            //WriteScr(res);
            //负指数分布
            for (int i = 0; i < res.Length; i++) res[i] = random.NegativeExponentialDist(lambda);
            Array.Sort(res);
            //WriteScr(res);

            double miu = 0.001;
            double sigma = 0.2;
            //正态分布
            for (int i = 0; i < res.Length; i++) res[i] = random.GaussianDist(miu, sigma, min, max)*10;
            Array.Sort(res);
            WriteScr(res);
            
            Console.ReadLine();
        }
        
        private static void WriteScr<T>(T[] arr)
        {
            int lastNum = 0;
            foreach (T num in arr)
            {
                int thisNum = (int)(double)(object)num;
                if (thisNum != lastNum) Console.WriteLine();
                Console.Write(thisNum + " ");
                lastNum = thisNum;
            }
            Console.Write("\n\n\n");
        }
    }
}
