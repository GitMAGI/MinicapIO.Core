using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DLL_Core
{
    public static class HttpUtils
    {
        public static int JHttpPost(string ipPort, string json, HttpStatusCode success = HttpStatusCode.OK)
        {
            HttpWebResponse response = JHttpRequest(ipPort, "POST", json);
            HttpStatusCode code = response.StatusCode;
            if (code == success)
                return 0;
            else
                return -1;
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

        public static int JHttpPost(string remoteIP, int remotePort, string path, string payload, HttpStatusCode success = HttpStatusCode.OK)
        {
            string response = JHttpRequest(remoteIP, remotePort, "POST", path, payload);
            HttpStatusCode code = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), response.Split(' ')[1]);
            if (code == success)
                return 0;
            else
                return -1;
        }

        private static string JHttpRequest(string remoteIP, int remotePort, string method, string path, string payload)
        {
            IPAddress remoteAddr = IPAddress.Parse(remoteIP);
            IPEndPoint remoteEndPoint = new IPEndPoint(remoteAddr, remotePort);
            Socket socket = new Socket(remoteAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            string.Format("Starting TCP Connection to {0}:{1} ...", remoteIP, remotePort);
            socket.Connect(remoteEndPoint);
            string.Format("TCP Connection to {0}:{1} successfully established!", remoteIP, remotePort);

            string httpResponse = "";

            try
            {
                List<string> headers = new List<string>()
                {
                    string.Format("{0} {1} HTTP/1.1", method, path),
                    string.Format("{0}: {1}:{2}", "Host", remoteIP, remotePort),
                    string.Format("{0}: {1}", "Content-Type", "application/json"),
                    string.Format("{0}: {1}", "cache-control", "no-cache"),
                    string.Format("{0}: {1}", "Content-Length", Encoding.UTF8.GetByteCount(payload))
                };
                string request = string.Join("\r\n", headers) + "\r\n" + "\r\n" + payload;
                byte[] dataToSend = Encoding.UTF8.GetBytes(request);

                int bytesSent = socket.Send(dataToSend, dataToSend.Length, SocketFlags.None);

                int bytesReceived = 0;
                byte[] dataReceived = new byte[10000];
                do
                {
                    bytesReceived = socket.Receive(dataReceived, dataReceived.Length, SocketFlags.None);
                    httpResponse = httpResponse + Encoding.ASCII.GetString(dataReceived, 0, bytesReceived);
                }
                while (bytesReceived > 0);
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                string.Format("Shutting down TCP socket {0}:{1} ..", remoteIP, remotePort);
                socket.Shutdown(SocketShutdown.Both);
                string.Format("Socket shut down");
                string.Format("Closing TCP socket {0}:{1} ..", remoteIP, remotePort);
                socket.Close();
                string.Format("Socket closed");
            }

            return httpResponse;
        }
    }
}