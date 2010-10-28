using System;

namespace RootFinding
{
	public delegate double UnaryFunction(double x);

	public abstract class RootFinder
	{

	#region Constants
		protected static string m_InvalidRange="Invalid range while finding root";
		protected static string m_AccuracyNotReached="The accuracy couldn't be reached with the specified number of iterations";
		protected static string m_RootNotFound="The algorithm ended without root in the range";
		protected static string m_RootNotBracketed="The algorithm could not start because the root seemed not to be bracketed";
		protected static string m_InadequatedAlgorithm="This algorithm is not able to solve this equation";
		protected static double double_Accuracy=9.99200722162641E-16;
	#endregion Constants

	#region Variables
		private static int m_NbIterDefaultMax=30;
		private static double m_DefaultAccuracy=1.0E-04;
		protected int m_NbIterMax;
		protected double m_xmin, m_xmax;
		protected double m_Accuracy;
		protected UnaryFunction m_f,m_Of;
		private double m_BracketingFactor=1.6;

	#endregion Variables

	#region Construction
		/// <summary>Constructor.</summary>
		/// <param name="f">A continuous function.</param>
		public RootFinder(UnaryFunction f)
			: this(f,m_NbIterDefaultMax,m_DefaultAccuracy) {
		}

		public RootFinder(UnaryFunction f,int niter,double acc) {
			m_f=f;
			m_NbIterMax=niter;
			m_Accuracy=acc;
		}
	#endregion Construction

	#region Properties
		public double BracketingFactor {
			get { return m_BracketingFactor; }
			set { if(value<=0.0) throw new ArgumentOutOfRangeException(); m_BracketingFactor=value; }
		}
		public int Iterations {
			set { if(value<=0) throw new ArgumentOutOfRangeException(); m_NbIterMax = value; }
		}
		public double Accuracy {
			get { return m_Accuracy; }
			set { m_Accuracy=value; }
		}
	#endregion Properties

	#region Methods
		/// <summary>Detect a range containing at least one root.</summary>
		/// <param name="xmin">Lower value of the range.</param>
		/// <param name="xmax">Upper value of the range</param>
		/// <param name="factor">The growing factor of research. Usually 1.6.</param>
		/// <returns>True if the bracketing operation succeeded, else otherwise.</returns>
		/// <remarks>This iterative methods stops when two values with opposite signs are found.</remarks>
		public bool SearchBracketsOutward(ref double xmin, ref double xmax, double factor) {
			// Check the range
			if(xmin>=xmax) throw new RootFinderException(m_InvalidRange,0,new Range(xmin,xmax),0.0);
			double fmin, fmax;
			fmin = m_f(xmin);
			fmax = m_f(xmax);
			int iiter=0;
			do {
				if(Sign(fmin)!=Sign(fmax)) return (true);
				if(Math.Abs(fmin)<Math.Abs(fmax)) fmin=m_f(xmin+=factor*(xmin-xmax));
				else fmax=m_f(xmax+=factor*(xmax-xmin));
			} while(iiter++<m_NbIterMax);
			throw new RootFinderException(m_RootNotFound,iiter,new Range(fmin,fmax),0.0);
		}

		// Algorithmes de résolution
		/// <summary>Prototype algorithm for solving the equation f(x)=0.</summary>
		/// <param name="x1">The low value of the range where the root is supposed to be.</param>
		/// <param name="x2">The high value of the range where the root is supposed to be.</param>
		/// <param name="bracket">Determines whether a bracketing operation is required.</param>
		/// <returns>Returns the root with the specified accuracy.</returns>
		public virtual double Solve(double x1, double x2, bool bracket) {
			if(bracket) SearchBracketsOutward(ref x1,ref x2,m_BracketingFactor);
			m_xmin = x1; m_xmax = x2; return Find();
		}
		/// <summary>Prototype algorithm for solving the equation f(x)=y.</summary>
		/// <param name="x1">The low value of the range where the root is supposed to be.</param>
		/// <param name="x2">The high value of the range where the root is supposed to be.</param>
		/// <param name="y"></param>
		/// <param name="bracket">Determines whether a bracketing operation is required.</param>
		/// <returns>Returns the root with the specified accuracy.</returns>
		public virtual double Solve(double x1,double x2,double y,bool bracket) {
			m_Of=m_f;
			m_f=UnaryFunctions.Substract(m_Of,UnaryFunctions.Constant(y));
			double x=Solve(x1,x2,bracket);
			m_f=m_Of; m_Of=null;
			return x;
		}

		protected abstract double Find();

	#endregion Methods

	#region Helper methods
		protected void Swap(ref double x,ref double y) {
			double t=x; x=y; y=t;
		}
		/// <summary>Helper method useful for preventing rounding errors.</summary>
		/// <returns>a*sign(b)</returns>
		protected double Sign(double a,double b) {
			return b>=0?(a>=0?a:-a):(a>=0?-a:a);
		}
		internal static double Sign(double x) {
			return x>0?1.0:-1.0;
		}
	#endregion Helper methods

	}
}
