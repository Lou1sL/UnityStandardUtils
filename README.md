﻿# UnityStandardUtils

Unity基础工具库

提供了Unity开发的一些常用功能：

加密编解码，存取档，语言本地化及对应的可视化编辑器，单例MonoBehavior的实现，可以传参的SceneLoader，web请求，键位管理器，玩家背包模拟

使用请打包dll，并和Newtonsoft.Json放在项目Asset目录下

欢迎fork

## 引擎无关功能：（高复用，无关UnityEngine.dll）

### Crypto.cs
加密相关工具函数

```CSharp
//AES加解密
static string RijndaelEncrypt(string pString, string pKey);
static string RijndaelDecrypt(string pString, string pKey);
//Base64编解码
static string Base64Encode(string str);
static string Base64Decode(string str);
//类序列反序列化
static string SerializeObject(object pObject);
static object DeserializeObject(string pString, Type pType);
//计算校验值
static string MD5(string str);
```

### SaveManager.cs
存档功能实现

使用方法：
```CSharp
//假设某任意结构存档类型为TestClass(类名，结构均不限)
TestClass testClass1 = new TestClass();
TestClass testClass2 = new TestClass();

//不进行加密的存取类
SaveManager saveManager = new SaveManager("SavePath", "FileName");
//进行加密的存取类
SaveManager saveManagerEncrypted = new SaveManager("SavePath","FileName","EncryptSeed");

//储存testClass1对象中的数据到文件
saveManager.SetData(testClass1);
//读档，读出的数据会存到testClass2的对象中
SaveManager.GetDataReturnCode rtnCode = saveManager.GetData(ref testClass2);
```
上面的方法将同一存档文件交由一个SaveManager对象管理，这当然是好的，但同样，也可不这么做：
```CSharp
//存档
//此处可为任意类 TestClass只是一个任意的实例类
TestClass testClass1 = new TestClass();
new SaveManager("SavePath", "FileName").SetData(testClass1);

//读档
TestClass testClass2 = new TestClass();
SaveManager.GetDataReturnCode res = new SaveManager("SavePath", "FileName").GetData(ref testClass2);
```


### Localization.cs
语言本地化，基于XML的多国语言支持
解决方案内的LocalizationEditor编译后可以可视化的编辑本地化文档，更加方便

```CSharp
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
```
本地化文档（储存在 ../Localization/ 下）

zh-CN.xml
```XML
<?xml version="1.0" encoding="UTF-8"?>
<Localization language="中文(简体)">
	<LOGO_SKIP str="空格  跳过"/>
</Localization>
```
zh-TW.xml
```XML
<?xml version="1.0" encoding="UTF-8"?>
<Localization language="中文(繁體)">
	<LOGO_SKIP str="空格  跳過"/>
</Localization>
```
en-US.xml
```XML
<?xml version="1.0" encoding="UTF-8"?>
<Localization language="English">
	<LOGO_SKIP str="Skip: Press Space"/>
</Localization>
```


### Web.cs
网络相关

http请求(GET/POST)：
```CSharp
Web.HttpRequest httpRequest = new Web.HttpRequest();
httpRequest.SetUrl("www.revokedstudio.com");

//GET和POST方法
httpRequest.SetRequestType(Web.HttpRequest.RequestType.GET);
//httpRequest.SetRequestType(Web.HttpRequest.RequestType.POST);

//参数添加
httpRequest.AddParam(new Web.HttpRequest.ParamPair("name", "ryubai"));
httpRequest.AddParam("name", "ryubai");
httpRequest.AddParamPairs(new Dictionary<string, string> { {"name","ryubai" },{"age","secret"}});
//清除参数
httpRequest.ClearParams();

//获取返回的内容到string str
httpRequest.SendRequest(ref str);
//或者使用下面的函数下载到文件
httpRequest.Download(Environment.CurrentDirectory + @"/saved.html");

```

### InventoryManager.cs
游戏内物品管理(可用于解谜游戏等)


```CSharp
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
playerBag.Pop(1);
Console.WriteLine(playerBag.ToString());

```


## 引擎相关功能：（仅Unity，using UnityEngine.dll）
由于引擎原因，这里的代码不能在控制台测试，请在游戏脚本中使用

### InputController.cs
键位管理器

```CSharp

//功能键
public enum Func
{
    Pause,
    Left,
    Right,
    Up,
    Down,
    Sprint,
    Fire,
    Reload,
}

//默认键位设置
private static Dictionary<Func, KeyCode> KeyCodeMapDefault = new Dictionary<Func, KeyCode>()
{
        { Func.Left           , KeyCode.A         },
        { Func.Right          , KeyCode.D         },
        { Func.Up             , KeyCode.W         },
        { Func.Down           , KeyCode.S         },
        { Func.Sprint         , KeyCode.LeftShift },
        { Func.Fire           , KeyCode.Mouse0    },
        { Func.Reload         , KeyCode.R         },
        { Func.Pause          , KeyCode.Escape    },
};

//初始化
InputController.InitInputController(KeyCodeMapDefault);
//读取设置文件到当前配置
InputController.LoadSettings();
//设定Pause功能为P键
InputController.SetKeyCodeByFunc(KeyCode.P, Func.Pause);
//设定当前全部功能回到默认配置
InputController.SetToDefault();
//保存当前配置到配置文件
InputController.SaveSettings();
//获得Pause功能对应的按键
KeyCode pauseCode = InputController.GetKeyCodeByFunc(Func.Pause);
//判断Pause功能对应按键是否刚刚按下
bool isPressed = InputController.GetKey(InputController.KeyStatus.Push, Func.Pause);


```

### SceneLoader.cs
可以传参数的SceneManager

```CSharp

//太简单了，看源码吧，就30行，2333

```

### Singleton.cs
单例和单例MonoBehavior的实现，再也不需要Ctrl+C，Ctrl+V了

```CSharp

//脚本继承SingletonMonoBehaviour就行了吧。。。调用Instance使用即可
//凡是继承了的脚本，运行时只要instance被调用就会立马产生一个以该脚本命名的包含本脚本的GameObject

```