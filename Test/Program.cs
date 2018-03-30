using System;
using System.Collections.Generic;
using UnityStandardUtils.Web;

namespace Test
{
    class Program
    {


        static void Main(string[] args)
        {
            HttpRequest httpRequest = new HttpRequest();
            httpRequest.SetUrl("www.revokedstudio.com");

            //GET和POST方法
            httpRequest.SetRequestType(HttpRequest.RequestType.GET);
            //httpRequest.SetRequestType(Web.HttpRequest.RequestType.POST);

            //参数添加
            httpRequest.AddParam(new HttpRequest.ParamPair("name", "ryubai"));
            httpRequest.AddParam("name", "ryubai");
            httpRequest.AddParamPairs(new Dictionary<string, string> { {"name","ryubai" },{"age","secret"}});
            //清除参数
            httpRequest.ClearParams();


            //获取返回的内容到string str
            string str = string.Empty;
            httpRequest.SendRequest(ref str);
            //或者使用下面的函数下载到文件
            httpRequest.Download(Environment.CurrentDirectory + @"/saved.html");


        }

    }
}
