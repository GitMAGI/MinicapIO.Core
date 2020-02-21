using DLL_Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace T_Main
{
    class Program
    {
        public static readonly string AppName = "MinicapIO.Core.T_Main";

        static void Main(string[] args)
        {
            string methodName = "T_Main";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                MultithreadKeyboardEventTests multithreadKeyboardEventTests = new MultithreadKeyboardEventTests();

                // Test 1
                //multithreadKeyboardEventTests.KeyPress();

                // Test 2
                List<TouchEvent> lefts = new List<TouchEvent>()
                {
                    new DownTouchEvent() { Contact = 0, Pressure = 50, Y = 128, X = 140 }, // When the device is in landscape mode we have to flip [x y]
                    new CommitTouchEvent(),
                    new DelayTouchEvent() { Value = 150 },
                    new UpTouchEvent() { Contact = 0 },
                    new CommitTouchEvent()
                };
                List<TouchEvent> rights = new List<TouchEvent>()
                {
                    new DownTouchEvent() { Contact = 0, Pressure = 50, Y = 1783, X = 140 }, // When the device is in landscape mode we have to flip [x y]
                    new CommitTouchEvent(),
                    new DelayTouchEvent() { Value = 150 },
                    new UpTouchEvent() { Contact = 0 },
                    new CommitTouchEvent()
                };
                List<TouchEvent> nitros = new List<TouchEvent>()
                {
                    new DownTouchEvent() { Contact = 0, Pressure = 50, Y = 1783, X = 560 }, // When the device is in landscape mode we have to flip [x y]
                    new CommitTouchEvent(),
                    new DelayTouchEvent() { Value = 150 },
                    new UpTouchEvent() { Contact = 0 },
                    new CommitTouchEvent()
                };
                multithreadKeyboardEventTests.TouchRowDirections(lefts.ToArray(), rights.ToArray(), nitros.ToArray());
            }
            catch(Exception ex)
            {
                Console.WriteLine("Test Failed! Error found: " + ex.Message);
                Console.WriteLine(ex.ToString());
            }
            finally
            {

            }

            stopwatch.Stop();
            Console.WriteLine(string.Format("{0} Completed in {1}", methodName, Utils.ElapsedTime(stopwatch.Elapsed)));
        }
    }
}
