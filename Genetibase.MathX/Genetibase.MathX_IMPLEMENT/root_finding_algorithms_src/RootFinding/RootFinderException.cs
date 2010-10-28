using System;
using System.Collections.Generic;
using System.Text;

namespace RootFinding
{
	public struct Range {
		double Min,Max;

		public Range(double min,double max) {
			Min=min; Max=max;
		}
	}
	public class RootFinderException : Exception
	{
		private int m_Iteration;
		private Range m_Range;
		private double m_Accuracy;

		public RootFinderException(string message,int iteration,Range range,double accuracy) : base(message) {
			m_Iteration=iteration;
			m_Range=range;
			m_Accuracy=accuracy;
		}

		public int Iteration {
			get { return m_Iteration; }
			set { m_Iteration = value; }
		}

		public Range Range {
			get { return m_Range; }
			set { m_Range = value; }
		}

		public double Accuracy {
			get { return m_Accuracy; }
			set { m_Accuracy = value; }
		}
	}
}
