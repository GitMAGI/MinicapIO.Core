using DLL_Core;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace T_Core
{
    [TestClass]
    public class HttpTests
    {
        [TestMethod]
        public void SynchronousCall()
        {
            int nrOfCalls = 1;
            int nrOfUnsuccessfulCalls = 0;
            string methodName = "SynchronousCall";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            string ipPort = "http://localhost:9889/";
            /*
            string json = "[ ";
            json += "{ \"type\": \"down\", \"contact\": 0, \"x\": 500, \"y\": 500, \"pressure\": 50 }, ";
            json += "{ \"type\": \"commit\" }, ";
            json += "{ \"type\": \"delay\", \"value\": 154 }, ";
            json += "{ \"type\": \"up\", \"contact\": 0 }, ";
            json += "{ \"type\": \"commit\" } ";
            json += "]";
            */

            ICollection<TouchEvent> events = new List<TouchEvent>
            {
                new DownTouchEvent() { Contact = 0, Pressure = 50, X = 500, Y = 500 },
                new CommitTouchEvent(),
                new DelayTouchEvent() { Value = 154 },
                new UpTouchEvent() { Contact = 0 },
                new CommitTouchEvent()
            };

            string json = JsonConvert.SerializeObject(events.ToArray());

            string httpMethodName = "HTTP Call";
            Stopwatch httpwatch = new Stopwatch();
            httpwatch.Start();
            nrOfUnsuccessfulCalls += HttpUtils.JHttpPost(ipPort, json);
            httpwatch.Stop();
            
            Console.WriteLine(string.Format("{0} Completed in {1}", httpMethodName, Utils.ElapsedTime(httpwatch.Elapsed)));

            Console.WriteLine(string.Format("Calls Report => Total: {0} | OK: {1} | KO: {2}", nrOfCalls, nrOfCalls - nrOfUnsuccessfulCalls, nrOfUnsuccessfulCalls));

            stopwatch.Stop();
            Console.WriteLine(string.Format("{0} Completed in {1}", methodName, Utils.ElapsedTime(stopwatch.Elapsed)));
        }
        
        [TestMethod]
        public void MultipleSynchronousCalls()
        {
            int nrOfCalls = 30;
            int nrOfUnsuccessfulCalls = 0;
            string methodName = "MultipleSynchronousCalls";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string ipPort = "http://localhost:9889/";

            Random r = new Random();
            for (int i=0; i < nrOfCalls; i++)
            {
                uint pressure = 50;
                uint x = (uint)r.Next(100, 800);
                uint y = (uint)r.Next(100, 800);
                uint contact = 0;
                uint value = 5;

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
                nrOfUnsuccessfulCalls += HttpUtils.JHttpPost(ipPort, json);
                httpwatch.Stop();
                Console.WriteLine(string.Format("{0} Completed in {1}", httpMethodName, Utils.ElapsedTime(httpwatch.Elapsed)));
            }

            Console.WriteLine(string.Format("Calls Report => Total: {0} | OK: {1} | KO: {2}", nrOfCalls, nrOfCalls - nrOfUnsuccessfulCalls, nrOfUnsuccessfulCalls));

            stopwatch.Stop();
            Console.WriteLine(string.Format("{0} Completed in {1}", methodName, Utils.ElapsedTime(stopwatch.Elapsed)));
        }

        [TestMethod]
        public void SynchronousCallOverSocket()
        {
            int nrOfCalls = 1;
            int nrOfUnsuccessfulCalls = 0;
            string methodName = "SynchronousCallOverSocket";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string _remoteIP = "127.0.0.1";
            int _remotePort = 9889;

            try
            {
                ICollection<TouchEvent> events = new List<TouchEvent>
                {
                    new DownTouchEvent() { Contact = 0, Pressure = 50, X = 500, Y = 500 },
                    new CommitTouchEvent(),
                    new DelayTouchEvent() { Value = 154 },
                    new UpTouchEvent() { Contact = 0 },
                    new CommitTouchEvent()
                };

                string json = JsonConvert.SerializeObject(events.ToArray());

                string httpMethodName = "HTTP Call";
                Stopwatch httpwatch = new Stopwatch();
                httpwatch.Start();
                nrOfUnsuccessfulCalls += HttpUtils.JHttpPost(_remoteIP, _remotePort, "/", json);
                httpwatch.Stop();

                Console.WriteLine(string.Format("{0} Completed in {1}", httpMethodName, Utils.ElapsedTime(httpwatch.Elapsed)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

            Console.WriteLine(string.Format("Calls Report => Total: {0} | OK: {1} | KO: {2}", nrOfCalls, nrOfCalls - nrOfUnsuccessfulCalls, nrOfUnsuccessfulCalls));

            stopwatch.Stop();
            Console.WriteLine(string.Format("{0} Completed in {1}", methodName, Utils.ElapsedTime(stopwatch.Elapsed)));
        }

        [TestMethod]
        public void MultipleSynchronousCallOverSocket()
        {
            int nrOfCalls = 30;
            int nrOfUnsuccessfulCalls = 0;
            string methodName = "MultipleSynchronousCallOverSocket";
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string _remoteIP = "127.0.0.1";
            int _remotePort = 9889;

            try
            {
                Random r = new Random();
                for (int i = 0; i < nrOfCalls; i++)
                {
                    uint pressure = 50;
                    uint x = (uint)r.Next(100, 800);
                    uint y = (uint)r.Next(100, 800);
                    uint contact = 0;
                    uint value = 5;

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
                    nrOfUnsuccessfulCalls += HttpUtils.JHttpPost(_remoteIP, _remotePort, "/", json);
                    httpwatch.Stop();
                    Console.WriteLine(string.Format("{0} Completed in {1}", httpMethodName, Utils.ElapsedTime(httpwatch.Elapsed)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

            Console.WriteLine(string.Format("Calls Report => Total: {0} | OK: {1} | KO: {2}", nrOfCalls, nrOfCalls - nrOfUnsuccessfulCalls, nrOfUnsuccessfulCalls));

            stopwatch.Stop();
            Console.WriteLine(string.Format("{0} Completed in {1}", methodName, Utils.ElapsedTime(stopwatch.Elapsed)));
        }
    }
}
