# UnityStandardUtils

Unity基础工具库

该dll实现了一些Unity开发一般会用到的功能

欢迎fork

## 引擎无关功能：（高复用，无关UnityEngine.dll）

### Crypto.cs
加密相关工具函数，全部static

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
语言本地化

```CSharp
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
httpRequest.AddParam(new Web.HttpRequest.ParamPair("name","ryubai"));
httpRequest.AddParam(new Web.HttpRequest.ParamPair("age", "ihavenoidea"));

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
#### !!! 请先修改KeyCodeMap(功能键)和KeyCodeMapDefault(功能键对应的默认键设置)再打包dll !!!

```CSharp
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


```