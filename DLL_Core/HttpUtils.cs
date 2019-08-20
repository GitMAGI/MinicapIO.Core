using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DLL_Core
{
    public static class HttpUtils
    {
        public static void JHttpPost(string ipPort, string json)
        {
            JHttpRequest(ipPort, "POST", json);
        }

        public static async void JHttpPostAsync(string ipPort, string json)
        {
            await JHttpRequestAsync(ipPort, "POST", json);
        }

        private static HttpWebResponse JHttpRequest(string ipPort, string method, string payload)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(ipPort);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = method;

            payload = payload.Replace("\n", "");
            payload = payload.Replace("\t", "");
            payload = payload.Replace("\r", "");

            byte[] bytes = Encoding.UTF8.GetBytes(payload);
            using (Stream streamWriter = httpWebRequest.GetRequestStream())
            {
                streamWriter.Write(bytes, 0, bytes.Length);
            }
            httpWebRequest.ContentLength = bytes.Length;

            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            return httpResponse;
        }

        private static async Task<HttpWebResponse> JHttpRequestAsync(string ipPort, string method, string payload)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(ipPort);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = method;

            payload = payload.Replace("\n", "");
            payload = payload.Replace("\t", "");
            payload = payload.Replace("\r", "");

            byte[] bytes = Encoding.UTF8.GetBytes(payload);
            using (Stream streamWriter = httpWebRequest.GetRequestStream())
            {
                streamWriter.Write(bytes, 0, bytes.Length);
            }
            httpWebRequest.ContentLength = bytes.Length;

            WebResponse httpResponse = await httpWebRequest.GetResponseAsync();
            return (HttpWebResponse)httpResponse;
        }
    }
}
