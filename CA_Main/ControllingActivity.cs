using DLL_Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CA_Main
{
    public class ControllingActivity : BaseActivity
    {
        private string _ipRemote;
        private int _portRemote;

        public ControllingActivity(string ipRemote, int portRemote, string activityName = "Controlling", int sleepingTime = 1) : base(activityName, sleepingTime)
        {
            _ipRemote = ipRemote;
            _portRemote = portRemote;
        }

        override protected void _runner()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            ConsoleKey key = keyInfo.Key;
            char keyChar = keyInfo.KeyChar;
            ConsoleModifiers keyModifiers = keyInfo.Modifiers;

            _logger.Warning("Pressed key {0}", keyChar);
        }

        protected override void _initialize()
        {
        }

        protected override void _error()
        {
        }

        protected override void _cleaning()
        {
        }

    }
}