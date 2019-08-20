using DLL_Core;


using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace T_Core
{
    [TestClass]
    public class HttpTests
    {
        [TestMethod]
        public void SynchronousCall()
        {
            string ipPort = "http://localhost:9889/";
            string json = "[ ";
            json += "{ \"type\": \"down\", \"contact\": 0, \"x\": 500, \"y\": 500, \"pressure\": 50 }, ";
            json += "{ \"type\": \"commit\" }, ";
            json += "{ \"type\": \"delay\", \"value\": 1100 }, ";
            json += "{ \"type\": \"up\", \"contact\": 0 }, ";
            json += "{ \"type\": \"commit\" } ";
            json += "]";

            HttpUtils.JHttpPost(ipPort, json);
        }

        [TestMethod]
        public void AsynchronousCall()
        {
            string ipPort = "http://localhost:9889/";
            string json = "[ ";
            json += "{ \"type\": \"down\", \"contact\": 0, \"x\": 500, \"y\": 500, \"pressure\": 50 }, ";
            json += "{ \"type\": \"commit\" }, ";
            json += "{ \"type\": \"delay\", \"value\": 1100 }, ";
            json += "{ \"type\": \"up\", \"contact\": 0 }, ";
            json += "{ \"type\": \"commit\" } ";
            json += "]";

            HttpUtils.JHttpPostAsync(ipPort, json);
        }
    }
}
