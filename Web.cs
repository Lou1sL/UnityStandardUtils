using System;
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
            /// 发送请求，返回字符串
            /// </summary>
            /// <param name="saveToPath">可选，保存返回二进制到文件路径</param>
            /// <returns></returns>
            public string SendRequest()
            {
                if (URL == string.Empty) return "Error:URL didn't set";

                FileStream fs = new FileStream(Environment.CurrentDirectory+@"/webRequest.swap", FileMode.Create);

                string res = string.Empty;

                switch (requestType)
                {
                    case RequestType.GET: res = HttpGet(false,URL, GetSerializedParams(requestType), ref fs); break;
                    case RequestType.POST: res = HttpPost(false,URL, GetSerializedParams(requestType), ref fs); break;
                    default: break;
                }
                
                fs.Flush();
                fs.Close();
                fs.Dispose();

                return res;



            }

            /// <summary>
            /// 下载文件
            /// </summary>
            /// <param name="path"></param>
            public void Download(string path)
            {
                if (URL == string.Empty) return;
                if (path == string.Empty) return;

                FileStream fs = new FileStream(path, FileMode.Create);
                
                switch (requestType)
                {
                    case RequestType.GET: HttpGet(true,URL, GetSerializedParams(requestType), ref fs); break;
                    case RequestType.POST: HttpPost(true,URL, GetSerializedParams(requestType), ref fs); break;
                    default: break;
                }

                fs.Flush();
                fs.Close();
                fs.Dispose();

                return;
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

            private void HttpRequestManager(string url,RequestType type,ref string response,bool isSave,ref FileStream fs)
            {

            }


            private string HttpPost(bool isSave,string Url, string postDataStr,ref FileStream fs)
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


                string retString = string.Empty;

                if (isSave)
                {
                    ReadResponseToFileStream(ref fs, response);
                }
                else
                {
                    ReadResponseToString(ref retString, response);

                }
                
                return retString;
            }

            private string HttpGet(bool isSave,string Url, string postDataStr, ref FileStream fs)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();


                string retString = string.Empty;

                if (isSave)
                {
                    ReadResponseToFileStream(ref fs, response);
                }
                else
                {
                    ReadResponseToString(ref retString, response);
                }

                return retString;
                
            }


            private void ReadResponseToFileStream(ref FileStream fs, HttpWebResponse response)
            {
                Stream receiveStream = response.GetResponseStream();

                byte[] buffer = new byte[1024];
                int numBytesToRead = (int)response.ContentLength;

                int len = 0;
                while ((len = receiveStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fs.Write(buffer, 0, len);
                }
            }

            private void ReadResponseToString(ref string str,HttpWebResponse response)
            {
                response.Cookies = cookie.GetCookies(response.ResponseUri);
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                str = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
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
