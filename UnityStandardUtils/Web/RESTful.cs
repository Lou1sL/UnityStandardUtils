using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Text;
using UnityStandardUtils.Extension;

namespace UnityStandardUtils.Web
{
    public class RESTful : SingletonMonoBehaviour<RESTful>
    {

        public void JsonRequest(string API, Action<JContainer> callback, JObject postdata = null, RESTEncoding encode = RESTEncoding.UTF8)
        {
            Request(API, Headers.JSON, postdata, callback, encode);
        }

        public void Request<Q, T>(string API, Q postdata, Action<T> callback, RESTEncoding encode = RESTEncoding.UTF8)
        {
            Request(API, Headers.Defult, postdata, callback, encode);
        }

        public void Request<Q, T>(string API, Dictionary<string, string> headers, Q postdata, Action<T> callback, RESTEncoding encode = RESTEncoding.UTF8)
        {

            if (postdata == null)
                StartCoroutine(DoRequest(API, headers, null, callback, encode));
            else if (typeof(Q) == typeof(byte[]))
            {
                if (encode == RESTEncoding.UTF8)
                    StartCoroutine(DoRequest(API, headers, (byte[])(object)postdata, callback, encode));
                if (encode == RESTEncoding.GBK)
                    StartCoroutine(DoRequest(API, headers, Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("GBK"), (byte[])(object)postdata), callback, encode));
            }
            else if (typeof(Q) == typeof(string))
                StartCoroutine(DoRequest(API, headers, Encoding.UTF8.GetBytes((string)(object)postdata), callback, encode));
            else if (typeof(Q) == typeof(JObject))
            {
                if (encode == RESTEncoding.UTF8)
                    StartCoroutine(DoRequest(API, headers, Encoding.UTF8.GetBytes(((JObject)(object)postdata).ToString()), callback, encode));
                if (encode == RESTEncoding.GBK)
                    StartCoroutine(DoRequest(API, headers, Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding("GBK"), Encoding.UTF8.GetBytes(((JObject)(object)postdata).ToString())), callback, encode));
            }
            else
                Debug.LogError("UnityStandardUtils RESTful only support byte[] string JContainer request currently.");
        }


        private IEnumerator DoRequest<T>(string uri, Dictionary<string, string> headers, byte[] postdata, Action<T> callback, RESTEncoding encode)
        {
            var www = postdata == null ? new WWW(uri) : new WWW(uri, postdata, headers ?? Headers.Defult);
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                if (typeof(T) == typeof(byte[]))
                {
                    if (encode == RESTEncoding.UTF8)
                        callback((T)(object)www.bytes);
                    if (encode == RESTEncoding.GBK)
                        callback((T)(object)Encoding.Convert(Encoding.GetEncoding("GBK"), Encoding.UTF8, www.bytes));
                }
                else if (typeof(T) == typeof(string))
                {
                    if (encode == RESTEncoding.UTF8)
                        callback((T)(object)www.text);
                    if (encode == RESTEncoding.GBK)
                        callback((T)(object)Encoding.UTF8.GetString(Encoding.Convert(Encoding.GetEncoding("GBK"), Encoding.UTF8, www.bytes)));
                }
                else if (typeof(T) == typeof(JContainer))
                {
                    if (encode == RESTEncoding.UTF8)
                        callback((T)JsonConvert.DeserializeObject(www.text));
                    if (encode == RESTEncoding.GBK)
                        callback((T)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(Encoding.Convert(Encoding.GetEncoding("GBK"), Encoding.UTF8, www.bytes))));

                }
                else
                    Debug.LogError("UnityStandardUtils RESTful only support byte[] string JContainer callback currently.");
            }
            else
            {
                Debug.LogError(www.error);
            }

        }

        public enum RESTEncoding
        {
            UTF8,
            GBK
        }

        public static class Headers
        {
            public static readonly Dictionary<string, string> Defult =
                new Dictionary<string, string> { { "Content-Type", "text/html" } };

            public static readonly Dictionary<string, string> JSON =
                new Dictionary<string, string> { { "Content-Type", "application/json" } };


        }
    }
}
