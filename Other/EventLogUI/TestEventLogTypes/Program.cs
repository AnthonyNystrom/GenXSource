using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Collections;

namespace TestEventLogTypes
{
	class Program
	{
		static void Main(string[] args)
		{
			EventLogEntryType[] list = (EventLogEntryType[]) Enum.GetValues(typeof(EventLogEntryType));

			for (int i = 0; i< list.Length; i++) {
				EventLog.WriteEntry("TestTypes", ((EventLogEntryType)list[i]).ToString(), (EventLogEntryType)list[i]);
			}

		}
	}
}
