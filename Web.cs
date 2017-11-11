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
        public sealed class HttpRequest
        {
            
            private string URL = string.Empty;
            private RequestType requestType = RequestType.GET;
            private List<ParamPair> paramPairs = new List<ParamPair>();
            private CookieContainer cookie = new CookieContainer();
            public string SwapFileLocation = Environment.CurrentDirectory + @"/webRequest.swap";

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
            /// <param name="data">返回的字符串</param>
            /// <returns></returns>
            public ReturnStatus SendRequest(ref string data)
            {
                ReturnStatus status = new ReturnStatus();

                if (URL == string.Empty)
                {
                    status.exception = new ArgumentNullException();
                    return status;
                }

                FileStream fs = new FileStream(SwapFileLocation, FileMode.Create);
                
                switch (requestType)
                {
                    case RequestType.GET: status = HttpGet(false,URL, GetSerializedParams(requestType), ref fs,ref data); break;
                    case RequestType.POST: status = HttpPost(false,URL, GetSerializedParams(requestType), ref fs,ref data); break;
                    default: break;
                }
                
                fs.Flush();
                fs.Close();
                fs.Dispose();

                try { File.Delete(SwapFileLocation); } catch (Exception e) { }


                return status;

            }

            /// <summary>
            /// 下载文件
            /// </summary>
            /// <param name="path">文件下载的路径及文件名</param>
            public ReturnStatus Download(string path)
            {
                ReturnStatus status = new ReturnStatus();

                if (URL == string.Empty || path == string.Empty)
                {
                    status.exception = new ArgumentNullException();
                    return status;
                }

                FileStream fs = new FileStream(path, FileMode.Create);

                string str = string.Empty;
                switch (requestType)
                {
                    case RequestType.GET: status = HttpGet(true,URL, GetSerializedParams(requestType), ref fs,ref str); break;
                    case RequestType.POST: status = HttpPost(true,URL, GetSerializedParams(requestType), ref fs,ref str); break;
                    default: break;
                }

                fs.Flush();
                fs.Close();
                fs.Dispose();

                if (!status.IsSuccess()) try { File.Delete(path); }catch(Exception e) { }

                return status;
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
            

            private ReturnStatus HttpPost(bool isSave,string Url, string postDataStr,ref FileStream fs,ref string writeTo)
            {
                ReturnStatus status = new ReturnStatus();

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    //总是小3？？？
                    //request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
                    request.CookieContainer = cookie;
                    Stream myRequestStream = request.GetRequestStream();
                    StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("utf-8"));
                    myStreamWriter.Write(postDataStr);
                    myStreamWriter.Close();

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    response.Cookies = cookie.GetCookies(response.ResponseUri);

                    if (isSave)
                    {
                        ReadResponseToFileStream(ref fs, response);
                    }
                    else
                    {
                        ReadResponseToString(ref writeTo, response);
                    }

                    status.statusCode = response.StatusCode;
                }
                catch(Exception e)
                {
                    status.exception = e;
#if DEBUG
                    Console.WriteLine(e);
#endif
                }
                return status;
            }

            private ReturnStatus HttpGet(bool isSave,string Url, string postDataStr, ref FileStream fs, ref string writeTo)
            {

                ReturnStatus status = new ReturnStatus();

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
                    request.Method = "GET";
                    request.ContentType = "text/html;charset=UTF-8";

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    response.Cookies = cookie.GetCookies(response.ResponseUri);

                    if (isSave)
                    {
                        ReadResponseToFileStream(ref fs, response);
                    }
                    else
                    {
                        ReadResponseToString(ref writeTo, response);
                    }

                    status.statusCode = response.StatusCode;
                }
                catch(Exception e)
                {
                    status.exception = e;
#if DEBUG
                    Console.WriteLine(e);
#endif
                }

                return status;
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
            public sealed class ParamPair
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

            public sealed class ReturnStatus
            {
                public HttpStatusCode statusCode = HttpStatusCode.ExpectationFailed;
                public Exception exception = null;

                public bool IsSuccess()
                {
                    if (exception == null && statusCode == HttpStatusCode.OK) return true;
                    return false;
                }
            }
        }
    }
}
