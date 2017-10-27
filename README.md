# UnityStandardUtils

Unity基础工具库
该dll实现了一些Unity开发一般会用到的功能
欢迎fork

##引擎无关功能：（高复用，无关UnityEngine.dll）

###Crypto.cs
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

###SaveManager.cs
存档功能实现

使用方法：
```CSharp
//假设某任意结构存档类型为TestClass(类名，结构均不限)
TestClass testClass1 = new TestClass();
TestClass testClass2 = new TestClass();

//不进行加密的存取类
SaveManager saveManager = new SaveManager("SavePath(+/)", "FileName");
//进行加密的存取类
SaveManager saveManagerEncrypted = new SaveManager("SavePath(+/)","FileName","EncryptSeed");

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
new SaveManager("SavePath(+/)", @"FileName").SetData(testClass1);

//读档
TestClass testClass2 = new TestClass();
SaveManager.GetDataReturnCode res = new SaveManager("SavePath(+/)", @"FileName").GetData(ref testClass2);
```

###Web.cs
网络相关

http请求(GET/POST)：
```CSharp
Web.HttpRequest httpRequest = new Web.HttpRequest();
httpRequest.SetUrl("www.revokedstudio.com");
httpRequest.SetRequestType(Web.HttpRequest.RequestType.GET);
httpRequest.AddParam(new Web.HttpRequest.ParamPair("name","ryubai"));
httpRequest.AddParam(new Web.HttpRequest.ParamPair("age", "ihavenoidea"));
string str = httpRequest.SendRequest();
```


##引擎相关功能：（仅Unity，using UnityEngine.dll）
	



