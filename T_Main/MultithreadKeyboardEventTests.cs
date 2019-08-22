using DLL_Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace T_Main
{
    public class MultithreadKeyboardEventTests
    {        
        private bool _keepRunning { get; set; }

        public void KeyPress()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string methodName = "KeyPress";

            _keepRunning = true;
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
            Thread keyListenerThread = new Thread(() =>
            {
                while (_keepRunning)
                {
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        switch (key.Key)
                        {
                            case ConsoleKey.LeftArrow:
                                Console.WriteLine("←");
                                break;
                            case ConsoleKey.RightArrow:
                                Console.WriteLine("→");
                                break;
                            default:
                                break;
                        }
                    }
                    // Do something more useful
                    //Console.WriteLine("I'm doing things! Look at me!");

                    // Do some sleep to let other threads use CPUs
                    Thread.Sleep(1);
                }
            });
            keyListenerThread.Name = "KeyListenerThread";

            // Start the Execution of the delegate passed to the constructor of the thread
            keyListenerThread.Start();

            // To Wait Until keyListenerThread ends. If Omit this execution would be keep going but .... (strange things!!!)
            keyListenerThread.Join();

            stopwatch.Stop();
            Console.WriteLine(string.Format("{0} Completed in {1}", methodName, Utils.ElapsedTime(stopwatch.Elapsed)));
        }

        public void TouchRowDirections(TouchEvent[] leftEvents, TouchEvent[] rightEvents, TouchEvent[] nitroEvents)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string methodName = "TouchRowDirections";

            string touchIPRemote = "127.0.0.1";
            int touchPortRemote = 9889;
            string touchPathRemote = "/";

            int errLeft = 0;
            int errRight = 0;
            int errNitro = 0;
            int counterLeft = 0;
            int counterRight = 0;
            int counterNitro = 0;

            _keepRunning = true;
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
            Thread keyListenerThread = new Thread(() =>
            {
                while (_keepRunning)
                {
                    try
                    {
                        if (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo key = Console.ReadKey(true);
                            switch (key.Key)
                            {
                                case ConsoleKey.LeftArrow:
                                    //Console.WriteLine("←");
                                    string leftPayload = JsonConvert.SerializeObject(leftEvents);
                                    errLeft += HttpUtils.JHttpPost(touchIPRemote, touchPortRemote, touchPathRemote, leftPayload);
                                    counterLeft++;
                                    break;
                                case ConsoleKey.RightArrow:
                                    //Console.WriteLine("→");
                                    string rightPayload = JsonConvert.SerializeObject(rightEvents);
                                    errRight += HttpUtils.JHttpPost(touchIPRemote, touchPortRemote, touchPathRemote, rightPayload);
                                    counterRight++;
                                    break;
                                case ConsoleKey.UpArrow:
                                    //Console.WriteLine("↑");
                                    string nitroPayload = JsonConvert.SerializeObject(nitroEvents);
                                    errNitro += HttpUtils.JHttpPost(touchIPRemote, touchPortRemote, touchPathRemote, nitroPayload);
                                    counterNitro++;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        _keepRunning = false;
                        Console.WriteLine("Error int the Communication Thread: " + ex.Message);
                        Console.WriteLine(ex.ToString());                     
                    }
                    finally
                    {

                    }

                    // Do something more useful                    
                    string report = string.Format(
                        "Left => Total: {0} | OK: {1} | KO: {2} || Right => Total: {3} | OK: {4} | KO: {5} || Nitro => Total: {6} | OK: {7} | KO: {8}",
                        counterLeft, counterLeft + errLeft, (-1) * errLeft,
                        counterRight, counterRight + errRight, (-1) * errRight,
                        counterNitro, counterNitro + errNitro, (-1) * errNitro
                    );
                    Console.Write("\r{0}", report); // Note that if console is smaller than the content of one row, this will produce multilines SHIT!

                    // Do some sleep to let other threads use CPUs
                    Thread.Sleep(1);
                }
            });
            keyListenerThread.Name = "KeyListenerThread";

            // Start the Execution of the delegate passed to the constructor of the thread
            keyListenerThread.Start();

            // To Wait Until keyListenerThread ends. If Omit this execution would be keep going but .... (strange things!!!)
            keyListenerThread.Join();

            stopwatch.Stop();
            Console.WriteLine(string.Format("{0} Completed in {1}", methodName, Utils.ElapsedTime(stopwatch.Elapsed)));
        }

        private void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            _keepRunning = false;
            e.Cancel = true;
            Console.WriteLine(string.Format("Cancelling Execution ..."));
        }
    }
}
