using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleHttpProxy
{
    static class Logger
    {
        public delegate void LogEventHandler(string format, params object[] args);
        public delegate void LogCharEventHandler(char value);
        public static event LogEventHandler OnLogMessage;
        public static event LogCharEventHandler OnLogChar;

        public static void Log(string format, params object[] args)
        {
            if (OnLogMessage != null)
                OnLogMessage(format, args);  
        }

        public static void LogChar(char value)
        {
            if (OnLogChar != null)
                OnLogChar(value);
        }

    }
}
