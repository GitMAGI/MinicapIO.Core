using DLL_Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CA_Main
{
    public class ControllingActivity : BaseActivity
    {
        private string _ipRemote;
        private int _portRemote;                
        private string _pathRemote;

        private ICollection<TouchEvent> _leftEvents;
        private ICollection<TouchEvent> _rightEvents;
        private ICollection<TouchEvent> _nitroEvents;

        private int _errLeft = 0;
        private int _errRight = 0;
        private int _errNitro = 0;
        private int _counterLeft = 0;
        private int _counterRight = 0;
        private int _counterNitro = 0;

        public ControllingActivity(string ipRemote, int portRemote, string pathRemote = "/", string activityName = "Controlling", int sleepingTime = 1) : base(activityName, sleepingTime)
        {
            _ipRemote = ipRemote;
            _portRemote = portRemote;
            _pathRemote = pathRemote;
        }

        override protected void _runner()
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
                            
                            //DelayTouchEvent delayLeft =_leftEvents.First(x => x.GetType() == typeof(DelayTouchEvent)) as DelayTouchEvent;
                            //delayLeft.Value = 200;

                            string leftPayload = JsonConvert.SerializeObject(_leftEvents.ToArray());
                            _errLeft += HttpUtils.JHttpPost(_ipRemote, _portRemote, _pathRemote, leftPayload);
                            _counterLeft++;
                            break;
                        case ConsoleKey.RightArrow:
                            //Console.WriteLine("→");

                            //DelayTouchEvent delayRight = _rightEvents.First(x => x.GetType() == typeof(DelayTouchEvent)) as DelayTouchEvent;
                            //delayRight.Value = 200;

                            string rightPayload = JsonConvert.SerializeObject(_rightEvents.ToArray());
                            _errRight += HttpUtils.JHttpPost(_ipRemote, _portRemote, _pathRemote, rightPayload);
                            _counterRight++;
                            break;
                        case ConsoleKey.UpArrow:
                            //Console.WriteLine("↑");

                            //DelayTouchEvent delayNitro = _nitroEvents.First(x => x.GetType() == typeof(DelayTouchEvent)) as DelayTouchEvent;
                            //delayNitro.Value = 200;

                            string nitroPayload = JsonConvert.SerializeObject(_nitroEvents.ToArray());
                            _errNitro += HttpUtils.JHttpPost(_ipRemote, _portRemote, _pathRemote, nitroPayload);
                            _counterNitro++;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
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
                _counterLeft, _counterLeft + _errLeft, (-1) * _errLeft,
                _counterRight, _counterRight + _errRight, (-1) * _errRight,
                _counterNitro, _counterNitro + _errNitro, (-1) * _errNitro
            );
            _logger.Debug(report);
        }

        protected override void _initialize()
        {
            _leftEvents = new List<TouchEvent>()
                {
                    new DownTouchEvent() { Contact = 0, Pressure = 50, Y = 128, X = 140 }, // When the device is in landscape mode we have to flip [x y]
                    new CommitTouchEvent(),
                    new DelayTouchEvent() { Value = 150 },
                    new UpTouchEvent() { Contact = 0 },
                    new CommitTouchEvent()
                };
            _rightEvents = new List<TouchEvent>()
                {
                    new DownTouchEvent() { Contact = 0, Pressure = 50, Y = 1783, X = 140 }, // When the device is in landscape mode we have to flip [x y]
                    new CommitTouchEvent(),
                    new DelayTouchEvent() { Value = 150 },
                    new UpTouchEvent() { Contact = 0 },
                    new CommitTouchEvent()
                };
            _nitroEvents = new List<TouchEvent>()
                {
                    new DownTouchEvent() { Contact = 0, Pressure = 50, Y = 1783, X = 560 }, // When the device is in landscape mode we have to flip [x y]
                    new CommitTouchEvent(),
                    new DelayTouchEvent() { Value = 150 },
                    new UpTouchEvent() { Contact = 0 },
                    new CommitTouchEvent()
                };
        }

        protected override void _error()
        {
        }

        protected override void _cleaning()
        {
        }

    }
}