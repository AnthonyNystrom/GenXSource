using System;
using System.Collections.Generic;
using System.Text;

namespace Next2Friends.Data
{
    public partial class Trace
    {
        public static void Tracer(string Message)
        {
            Trace trace = new Trace();
            trace.DTCreated = DateTime.Now;
            trace.Text = Message;
            trace.Save();
        }

        public static void Tracer(string Message, string Source )
        {
            string MachineName = "anon";

            Trace trace = new Trace();
            trace.DTCreated = DateTime.Now;
            trace.Text = MachineName+ " Said:" + Message;
            trace.Source = Source;
            trace.Save();
        }

        public static void Tracer(string Message, string Source, string MachineName )
        {
            Trace trace = new Trace();
            trace.DTCreated = DateTime.Now;
            trace.Text = MachineName+ " Said:" + Message;
            trace.Source = Source;
            trace.Save();
        }
    }
}
