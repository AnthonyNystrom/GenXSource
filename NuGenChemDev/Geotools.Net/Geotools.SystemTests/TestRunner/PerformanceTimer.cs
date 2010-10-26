/*
 *  Copyright (C) 2002 Urban Science Applications, Inc. 
 *
 *  This library is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public
 *  License as published by the Free Software Foundation; either
 *  version 2.1 of the License, or (at your option) any later version.
 *
 *  This library is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 *  Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public
 *  License along with this library; if not, write to the Free Software
 *  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 */
using System;

namespace Geotools.SystemTests.TestRunner
{
	public class Display
	{
		static public void Show(string Message)
		{
			for (int x = 0; x < m_Indent * 4; x++)
				Console.Write(" ");

			Console.WriteLine(Message);
		}

		static public void Show(int PreIndent, string Message, int PostIndent)
		{
			m_Indent += PreIndent;

			if (m_Indent < 0)
				m_Indent = 0;

			int count = m_Indent * 4;

			m_Indent += PostIndent;

			if (m_Indent < 0)
				m_Indent = 0;

			for (int x = 0; x < count; x++)
				Console.Write(" ");

			Console.WriteLine(Message);
		}

		static private int m_Indent = 0;
		static public int Indent
		{
			get {return m_Indent;}
			set {m_Indent = value;}
		}
	}

	public class HighResolutionTimeSpan
	{
		[System.Runtime.InteropServices.DllImport("KERNEL32")]
		private static extern bool QueryPerformanceFrequency(ref long lpFrequency);

		public HighResolutionTimeSpan()
		{
			QueryPerformanceFrequency(ref m_Frequency);
		}

		public HighResolutionTimeSpan(long Ticks)
		{
			QueryPerformanceFrequency(ref m_Frequency);

			m_Miliseconds = 1000 * ((float)Ticks / (float)m_Frequency);
		}

		public override string ToString()
		{
			return String.Format("{0}", m_Miliseconds);
		}

		private long m_Frequency = 0;
		public long Frequency
		{
			get {return m_Frequency;}
		}

		float m_Miliseconds = 0;
		public float Miliseconds
		{
			get {return m_Miliseconds;}
		}
	}

	public class PerformanceTimer
	{
		[System.Runtime.InteropServices.DllImport("KERNEL32")]
		private static extern bool QueryPerformanceCounter(ref long lpPerformanceCount);

		[System.Runtime.InteropServices.DllImport("KERNEL32")]
		private static extern bool QueryPerformanceFrequency(ref long lpFrequency);                     

		private long m_StartCount = 0;
		private long m_StopCount = 0;
		private long m_ElapsedCount = 0;

		public PerformanceTimer()
		{
			m_Frequency = 0;
			QueryPerformanceFrequency(ref m_Frequency);
		}

		public void Start()
		{
			m_StartCount = 0;
			QueryPerformanceCounter(ref m_StartCount);
		}
		
		public void Stop()
		{
			m_StopCount = 0;
			QueryPerformanceCounter(ref m_StopCount);
			m_ElapsedCount = m_StopCount - m_StartCount;
		}

		public void Reset()
		{
			m_StartCount = 0;
			m_StopCount = 0;
			m_ElapsedCount = 0;
		}

		public long Elapsed
		{
			get {return m_ElapsedCount;}
		}

		public float Seconds
		{
			get {return((float)m_ElapsedCount / (float)m_Frequency);}
		}

		private long m_Frequency = 0;
		public long Frequency
		{
			get {return m_Frequency;}
		}

		public HighResolutionTimeSpan ElapsedTime
		{
			get {return new HighResolutionTimeSpan(m_ElapsedCount);}
		}
	}
}
