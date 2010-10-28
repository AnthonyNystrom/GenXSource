using System;

namespace RootFinding
{
	public class SecantRootFinder : RootFinder
	{
		public SecantRootFinder(UnaryFunction f)
			: base(f) {
		}
		public SecantRootFinder(UnaryFunction f,int niter,double accuracy)
			: base(f,niter,accuracy) {
		}
		protected override double Find() {
			double x1=m_xmin,x2=m_xmax,fl=m_f(x1),f=m_f(x2),dx,xl,rts;
			if(Math.Abs(fl)<Math.Abs(f)) {
				rts=x1;
				xl=x2;
				base.Swap(ref fl,ref f);
			} else {
				xl=x1;
				rts=x2;
			}
			int iiter=0;
			for(;iiter<m_NbIterMax;iiter++) {
				if(f==fl) throw new RootFinderException(m_InadequatedAlgorithm,iiter,new Range(x1,x2),Math.Abs(x1-x2));
				dx=(xl-rts)*f/(f-fl);
				xl=rts;
				fl=f;
				rts += dx;
				f=m_f(rts);
				if(Math.Abs(dx)<m_Accuracy||f==0.0) return rts;
			}
			throw new RootFinderException(m_RootNotFound,iiter,new Range(xl,x2),m_Accuracy);
		}
	}
}
