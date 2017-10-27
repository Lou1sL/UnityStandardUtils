using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace UnityStandardUtils
{
    public class Web
    {

        /// <summary>
        /// 标准Http请求
        /// </summary>
        public class HttpRequest
        {
            
            private string URL = string.Empty;
            private RequestType requestType = RequestType.GET;
            private List<ParamPair> paramPairs = new List<ParamPair>();
            private CookieContainer cookie = new CookieContainer();

            public void SetUrl(string url)
            {
                if(url.StartsWith("http://"))URL = url;
                else URL = "http://" + url;
            }

            public void SetRequestType(RequestType rt)
            {
                requestType = rt;
            }

            /// <summary>
            /// 添加一个键值对，如果是空的就没必要添加了
            /// </summary>
            /// <param name="p"></param>
            public void AddParam(ParamPair p)
            {
                if(!p.IsEmpty())paramPairs.Add(p);
            }

            
            /// <summary>
            /// 发送请求
            /// </summary>
            /// <returns></returns>
            public string SendRequest()
            {
                if (URL == string.Empty) return "Error:URL didn't set";
                
                switch (requestType)
                {
                    case RequestType.GET:return HttpGet(URL, GetSerializedParams(requestType));
                    case RequestType.POST:return HttpPost(URL, GetSerializedParams(requestType));
                    default:return string.Empty;
                }

            }

            /// <summary>
            /// 获得序列化后的键值对组，如果没有键值对，直接返回空就行了
            /// </summary>
            /// <param name="t">请求类型</param>
            /// <returns></returns>
            private string GetSerializedParams(RequestType t)
            {
                string ret = string.Empty;
                if (paramPairs.Count == 0) return ret;

                if (RequestType.GET == t) ret += "?";

                foreach (ParamPair p in paramPairs)
                {
                    ret += p.GetPair();
                    if (paramPairs.Last() != p) ret += "&";
                }

                return ret;
            }

            private string HttpPost(string Url, string postDataStr)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
                request.CookieContainer = cookie;
                Stream myRequestStream = request.GetRequestStream();
                StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
                myStreamWriter.Write(postDataStr);
                myStreamWriter.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                response.Cookies = cookie.GetCookies(response.ResponseUri);
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }

            private string HttpGet(string Url, string postDataStr)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();

                return retString;
            }



            /// <summary>
            /// 请求类型
            /// </summary>
            public enum RequestType
            {
                GET,
                POST,
            }


            /// <summary>
            /// 请求参数键值对
            /// </summary>
            public class ParamPair
            {
                private string LeftVal;
                private string RightVal;

                public ParamPair(string LVal, string RVal)
                {
                    LeftVal = LVal;
                    RightVal = RVal;
                }

                public string GetPair()
                {
                    return LeftVal + "=" + RightVal;
                }

                public bool IsEmpty()
                {
                    if (LeftVal == string.Empty || RightVal == string.Empty)
                        return true;

                    return false;
                }
            }


        }



        


    }
}
