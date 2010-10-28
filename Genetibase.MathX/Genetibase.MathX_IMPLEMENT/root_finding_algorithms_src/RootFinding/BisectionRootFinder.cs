using System;


namespace RootFinding
{
	public class BisectionRootFinder : RootFinder
	{
		public BisectionRootFinder(UnaryFunction f)
			: base(f) {
		}
		public BisectionRootFinder(UnaryFunction f,int niter,double pres)
			: base(f,niter,pres) {
		}
		protected override double Find() {
			double dx,xmid,x,f=m_f(m_xmin),fmid=m_f(m_xmax);
			if(m_xmin>=m_xmax||Sign(f)==Sign(fmid)) throw new RootFinderException(m_InvalidRange,0,new Range(m_xmin,m_xmax),0.0);
			if(f<0.0) { dx=m_xmax-m_xmin; x=m_xmin; } else { dx=m_xmin-m_xmax; x=m_xmax; };
			int iiter=0;
			do {
				fmid=m_f(xmid=x+(dx*=0.5));
				if(fmid<=0.0) x=xmid;
				if(Math.Abs(dx)<m_Accuracy||fmid==0.0) return (x);
			} while(iiter++<m_NbIterMax);

			// L'algorithme a dépassé le nombre d'itérations autorisé
			throw new RootFinderException(m_AccuracyNotReached,iiter,new Range(Math.Min(xmid,x),Math.Max(xmid,x)),Math.Abs(dx));
		}
	}
}
