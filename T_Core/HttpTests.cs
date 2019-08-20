using DLL_Core;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace T_Core
{
    [TestClass]
    public class HttpTests
    {
        [TestMethod]
        public void SynchronousCall()
        {
            string methodName = "SynchronousCall";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            string ipPort = "http://localhost:9889/";
            /*
            string json = "[ ";
            json += "{ \"type\": \"down\", \"contact\": 0, \"x\": 500, \"y\": 500, \"pressure\": 50 }, ";
            json += "{ \"type\": \"commit\" }, ";
            json += "{ \"type\": \"delay\", \"value\": 1100 }, ";
            json += "{ \"type\": \"up\", \"contact\": 0 }, ";
            json += "{ \"type\": \"commit\" } ";
            json += "]";
            */

            ICollection<TouchEvent> events = new List<TouchEvent>
            {
                new DownTouchEvent() { Contact = 0, Pressure = 50, X = 500, Y = 500 },
                new CommitTouchEvent(),
                new DelayTouchEvent() { Value = 1100 },
                new UpTouchEvent() { Contact = 0 },
                new CommitTouchEvent()
            };

            string json = JsonConvert.SerializeObject(events.ToArray());

            string httpMethodName = "HTTP Call";
            Stopwatch httpwatch = new Stopwatch();
            httpwatch.Start();
            HttpUtils.JHttpPost(ipPort, json);
            httpwatch.Stop();
            
            Console.WriteLine(string.Format("{0} Completed in {1}", httpMethodName, Utils.ElapsedTime(httpwatch.Elapsed)));

            stopwatch.Stop();
            Console.WriteLine(string.Format("{0} Completed in {1}", methodName, Utils.ElapsedTime(stopwatch.Elapsed)));
        }


        [TestMethod]
        public void MultipleSynchronousCalls()
        {
            int maxCalls = 30;
            string methodName = "MultipleSynchronousCalls";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string ipPort = "http://localhost:9889/";

            Random r = new Random();
            for (int i=0; i < maxCalls; i++)
            {
                uint pressure = 50;
                uint x = (uint)r.Next(100, 800);
                uint y = (uint)r.Next(100, 800);
                uint contact = 0;
                uint value = (uint)r.Next(100, 1100);

                ICollection<TouchEvent> events = new List<TouchEvent>()
                {
                    new DownTouchEvent() { Contact = contact, Pressure = pressure, X = x, Y = y },
                    new CommitTouchEvent(),
                    new DelayTouchEvent() { Value = value },
                    new UpTouchEvent() { Contact = contact },
                    new CommitTouchEvent(),
                };
                
                string json = JsonConvert.SerializeObject(events.ToArray());
                
                string httpMethodName = string.Format("HTTP Call n.{0}", i);
                Console.WriteLine(string.Format("{0} Submitting JSON: {1}", httpMethodName, json));
                Stopwatch httpwatch = new Stopwatch();
                httpwatch.Start();
                HttpUtils.JHttpPost(ipPort, json);
                httpwatch.Stop();
                Console.WriteLine(string.Format("{0} Completed in {1}", httpMethodName, Utils.ElapsedTime(httpwatch.Elapsed)));
            }

            stopwatch.Stop();
            Console.WriteLine(string.Format("{0} Completed in {1}", methodName, Utils.ElapsedTime(stopwatch.Elapsed)));
        }

    }
}
