using System;
using System.Runtime.InteropServices;

namespace TestCSharp
{
	/// <summary>
	/// Summary description for Timer.
	/// </summary>
	public class Timer
	{
		private Int64 mTimerStart;
		private Int64 mTimerFreq;

		public Timer() 
		{
			// Start Performance Timer
			if (QueryPerformanceCounter(ref mTimerStart)) 
			{
				QueryPerformanceFrequency(ref mTimerFreq);
			}
		}

		// Performance Timer--------------------------------------------
		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceCounter(ref Int64 X);

		[DllImport("Kernel32.dll")]
		private static extern bool QueryPerformanceFrequency(ref Int64 X);

		public Int64 ms() 
		{
			Int64 TimerEnd=0;
			if (QueryPerformanceCounter(ref TimerEnd)) 
			{
				double diff=(TimerEnd - mTimerStart)/ (mTimerFreq / 1000.0);
				return (int)(diff);
			}
			else 
			{
				return 0;
			}
		}
	}
}
