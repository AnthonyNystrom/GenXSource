using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;

namespace Genetibase.Debug
{
	public class NuGenTraceListener : System.Diagnostics.TraceListener
	{
		public delegate void TraceDelegate(string s);

		public event TraceDelegate TraceHappened;

		public override void Write(string s)
		{
			OnTraceHappened(s);			
		}

		public override void WriteLine(string message)
		{
			OnTraceHappened(message);
		}

		private void OnTraceHappened(string s)
		{
			if (TraceHappened != null)
				TraceHappened(s);
		}
	}
}
