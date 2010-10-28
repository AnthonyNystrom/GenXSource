//This file uses some routines from Numerical Recipes and therefore cannot be published fully.

using System;
using System.Threading;
using System.Windows.Forms;
using FPlotLibrary;


namespace FPlotFit {
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	
	public enum Algorithm {Marquardt, NelderMead, SimulatedAnnealing}

	public delegate void StepEventHandler(Fit fit, bool finished);
	
	public class Fit
	{
		public event StepEventHandler Step;

		private bool[] fitp;
		private float chisq = 0;
		private float[,] covar;
		private Function1D f, f0;
		private DataItem data;
		private bool fitting = false;

		public Fit() {}

		public Fit(DataItem data, Function1D f) {
			Data = data;
			Function = f;
		}

		public Fit(DataItem data, Function1D f, bool[] fitp) {
			Data = data;
			f0 = f;
			this.fitp = (bool[])fitp.Clone();
		}

		public float ChiSquare {
			get {return chisq;}
		}
	
		public float Q {
			get {return gammq(0.5F*(data.Length-f.p.Length), 0.5F*ChiSquare);}
		}

		public float[,] CovarianceMatrix {
			get{
				if (fitting) throw new InvalidOperationException("Covariance Matrix is invalid during fitting");
				return covar;
			}
		}
	
		public Function1D Function {
			get {return f0;}
			set {
				f0 = value;
				fitp = new bool[f0.p.Length];
				for (int i = 0; i < f0.p.Length; i++) {
					fitp[i] = true;
				}
			}
		}

		public bool[] Fitp {
			get {return fitp;}
			set {
				if (fitting) throw new System.InvalidOperationException("cannot change Fitp during calculation.");
				fitp = (bool[])value.Clone();
			}
		}

		public DataItem Data {
			get {return data;}
			set {data = value;}
		}

		public int NEval {
			get {
				if (f != null) return ((FunctionItem)f.code).neval;
				else return 0;
			}
			set {
				if (f != null)((FunctionItem)f.code).neval = value;
			}
		}

		public void Start(Algorithm a)
		{
			fitting = true;
			switch (a) {
			case Algorithm.Marquardt:
				MarquardtStart();
				break;
			case Algorithm.NelderMead:
				NelderMeadStart(true);
				break;
			case Algorithm.SimulatedAnnealing:
				NelderMeadStart(false);
				break;
			}
		}

		#region Gamma function
		static float gammln(float xx) {
		}

		static void gser(out float gamser, float a, float x, out float gln) {
		}

		static void gcf(out float gammcf, float a, float x, out float gln) {
		}

		static float gammq(float a, float x) {
		}
		#endregion

		#region Numerical Derivatives
		public class DerivFunction: Function1D {
			public bool fast;

			void GetH(double x, ref double h, double temp) {
				h = temp - x;
			}

			void FastDeriv(double x) {
				const double EPS = 1e-10;
				int i;
				double h, p0, temp, f0, f1;

				for (i = 0; i < p.Length; i++) {
					p0 = p[i];
					h = p0 * EPS;
					temp = p0 + h;
					GetH(p0, ref h, temp);
					p[i] = p0 + h;
					f0 = ((Function1D)code).F(x);
					p[i] = p0 - h;
					f1 = ((Function1D)code).F(x);
					dfdp[i] = (f0 - f1)/(2*h);
				}
			}

			void SlowDeriv(double x) {
				// adapted from gsl_diff_central
				const double GSL_SQRT_DBL_EPSILON = 1.4901161193847656e-08;

				/* Construct a divided difference table with a fairly large step
					size to get a very rough estimate of f'''.  Use this to estimate
					the step size which will minimize the error in calculating f'. */

				int i, k, j;
				double h = GSL_SQRT_DBL_EPSILON, a3, f0, f1, p0, temp;
				double[] a = new double[4], d = new double[4];

				for (j = 0; j < p.Length; j++) {

					/* Algorithm based on description on pg. 204 of Conte and de Boor
						(CdB) - coefficients of Newton form of polynomial of degree 3. */
				  
					for (i = 0; i < 4; i++)	{
						a[i] = x + (i - 2) * h;
						p0 = p[j];
						p[j] = a[i];
						d[i] = ((Function1D)code).F(x);
						p[j] = p0;
					}

					for (k = 1; k < 5; k++)	{
						for (i = 0; i < 4 - k; i++)	{
							d[i] = (d[i + 1] - d[i]) / (a[i + k] - a[i]);
						}
					}

					/* Adapt procedure described on pg. 282 of CdB to find best
						value of step size. */

					a3 = Math.Abs(d[0] + d[1] + d[2] + d[3]);

					if (a3 < 100 * GSL_SQRT_DBL_EPSILON) {
						a3 = 100 * GSL_SQRT_DBL_EPSILON;
					}

					h = Math.Pow(GSL_SQRT_DBL_EPSILON / (2 * a3), 1.0 / 3.0);

					if (h > 100 * GSL_SQRT_DBL_EPSILON) {
						h = 100 * GSL_SQRT_DBL_EPSILON;
					}

					p0 = p[j];
					temp = p0 + h;
					GetH(p0, ref h, temp);
					p[j] = p0 + h;
					f0 = ((Function1D)code).F(x);
					p[j] = p0 - h;
					f1 = ((Function1D)code).F(x);
					p[j] = p0;
					dfdp[j] = (f0 - f1)/(2*h);				
				}			
			}

			public override double F(double x) {
				if (fast) FastDeriv(x);
				else SlowDeriv(x);
				return ((Function1D)code).F(x);
			}

			public DerivFunction(Function1D f) {
				code = f.code;
				p = f.p;
				if (f.dfdp.Length == f.p.Length) {
					dfdp = f.dfdp;
					fast = true;
					this.f = f.f;
				} else {
					dfdp = new SmallData();
					fast = true;
					dfdp.Length = p.Length;
					this.f = new Code(F);
				}
			}
		}
		#endregion

		#region Marquardt algorithm

		private int mfit = 0;
		private float ochisq = 0;
		private float[] beta, da;
		private double[] ptemp;
		private float[,] oneda;

		float[,] alpha;
		float oldchisq, alamda;

		private void MarquardtThread(object state) {
			const float BIG = 1e28F;
			try {
				mrqmin(fitp, ref covar, ref alpha, ref chisq, ref alamda); 
				while (alamda < BIG && (chisq >= oldchisq || oldchisq - chisq > chisq*1e-5F)) {
					mrqmin(fitp, ref covar, ref alpha, ref chisq, ref alamda);
					Step(this, false);
					oldchisq = chisq;
				}
				if (f is DerivFunction) {
					((DerivFunction)f).fast = false;
					mrqmin(fitp, ref covar, ref alpha, ref chisq, ref alamda);
				}
				alamda = 0;
				mrqmin(fitp, ref covar, ref alpha, ref chisq, ref alamda);
			} catch (Exception ex) {
				MessageBox.Show("Error during Marquardt-Fit: " + ex.Message);
			}
			fitting = false;
			Step(this, true);
		}

		private void MarquardtStart() {
			chisq = 0; oldchisq = -1e10F; alamda = -1;
			NEval = 0;
			if (f0.p.Length > 0 && f0.dfdp.Length == 0) {
				f = new DerivFunction(f0);
			} else f = f0;
			ThreadPool.QueueUserWorkItem(new WaitCallback(MarquardtThread));
			//Do();
		}

		private void mrqmin(bool[] ip, ref float[,] covar, ref float[,] alpha,
			ref float chisq, ref float alamda) {
		}

		private void mrqcof(bool[] ip, ref float[,] alpha, ref float[] beta, ref float chisq) {
		}

		private void gaussj(float[,] a, int n, float[,] b, int m) {
		}

		private void covsrt(float[,] covar, int ma, bool[] ip, int mfit) {
		}
		#endregion

		#region Nelder & Mead algorithm

		private delegate float ChiSqr(float[] p);

		const float BETA = 3, GAMMA = 0.6F, DELTA = 1, FTOL = 0.001F;
		const int ALPHA = 100, N = 15;
		float[,] p;
		float[] y, pb;
		float yb, T, Tnew;
		int ndim, iter, n;
		ChiSqr chifunc;

		private void NelderMeadThread(object state) {
			try {
			startamo(fitp, out p, out y, out pb, out yb, out ndim, out chifunc); 
			if (T > 0) T = yb*DELTA;
			iter = ALPHA;
			for (n = 0; T > 0 && n < N; n++) {
				amebsa(p, y, ndim, pb, ref yb, FTOL, chifunc, ref iter, T);
				Tnew = BETA*(y[0] - yb);
				if (Tnew < T*GAMMA || Tnew >= T) T *= GAMMA;
				else T = Tnew;
				iter = ALPHA;
			}
			iter = 5000;
			amebsa(p, y, ndim, pb, ref yb, FTOL, chifunc, ref iter, 0);
			stopamo(pb);
			} catch (Exception ex) {
				MessageBox.Show("Error during Nelder Mead Fit: " + ex.Message);
			}
			fitting = false;
			Step(this, true);
		}

		private void NelderMeadStart(bool T0) {
			const int TFAC = 3;
			chisq = 0;
			f = f0;
			if (T0) T = 0;
			else T = data.Length*TFAC;
			NEval = 0;
			ThreadPool.QueueUserWorkItem(new WaitCallback(NelderMeadThread));
			//Do();
		}

		private double[] p0;
		private bool[] ip0;
		private float[] ptry;

		private float chisqr(float[] p) {
			int i, j;
			float chisq, ymod, sig2i, dy;

			for (i = 0, j = 0; i < f.p.Length; i++) {
				p0[i] = f.p[i];
				if (ip0[i]) f.p[i] = p[j++];
			}

			chisq = 0;
			for (i = 0; i < data.Length; i++) {
				ymod = (float)f.f(data.x[i]);
				sig2i = 1/(float)(data.dy[i]*data.dy[i]);
				dy = (float)data.y[i] - ymod;
				chisq += dy*dy*sig2i;
			}
			for (i = 0; i < f.p.Length; i++) f.p[i] = p0[i];

			return chisq;
		}
		
		private void startamo(bool[] ip, out float[,] p, out float[] y, out float[] pb, out float yb,
			out int ndim, out ChiSqr chisq) {

			int i, j, ilo;
			float[] pp;
			ip0 = ip;
			chisq = new ChiSqr(chisqr);
			ndim = 0;
			for (i = 0; i < ip.Length; i++) {
				if (ip[i]) ndim++;
			}

			p0 = new double[ip.Length];
			p = new float[ndim+1, ndim];
			y = new float[ndim+1];
			ptry = new float[ndim];

			for (i = 0, j = 0; i < ip.Length; i++) {
				if (ip[i]) p[0, j++] = (float)f.p[i];
			}
			for (i = 1; i <= ndim; i++) {
				for (j = 0; j < ndim; j++) {
					if (j == i-1) p[i, j] = p[0, j]*2;
					else p[i, j] = p[0, j];
				}
			}
			
			pp = new float[ndim];
			yb = float.MaxValue;
			ilo = 0;
			for (i = 0; i <= ndim; i++) {
				for (j = 0; j < ndim; j++) pp[j] = p[i, j]; 
				y[i] = chisq(pp);
				if (y[i] < yb) {
					ilo = i;
					yb = y[i];
				}
			}

			pb = new float[ndim];
			for (i = 0; i < ndim; i++) pb[i] = p[ilo, i];

		}

		private void stopamo(float[] pb) {
			int i, j;
			covar = new float[0, 0];
			float[,] alpha = new float[0, 0];
			float alamda;
			chisq = 0;
			f = new DerivFunction(f0);
			if (f is DerivFunction) ((DerivFunction)f).fast = false;

			for (i = 0, j = 0; i < f.p.Length; i++) {
				if (ip0[i]) f.p[i] = pb[j++];
			}

			alamda = -1;
			mrqmin(ip0, ref covar, ref alpha, ref chisq, ref alamda);
			alamda = 0;
			mrqmin(ip0, ref covar, ref alpha, ref chisq, ref alamda);

			ip0 = null;
			p0 = null;
			ptry = null;
		}
		
		private void amebsa(float[,] p, float[] y, int ndim, float[] pb, ref float yb, float ftol, ChiSqr f,
			ref int iter, float temptr) {
		}

		private float amotsa(float[,] p, float[] y, float[] psum, int ndim, float[] pb, ref float yb,
			ChiSqr f, int ihi, ref float yhi, float temptr, float fac) {
		}

		private int idum = System.Environment.TickCount;

		private float ran0() {
		}
		#endregion

	}
}
