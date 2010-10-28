using System;


namespace RootFinding
{
	public class RidderRootFinder : RootFinder {
		public RidderRootFinder(UnaryFunction f)
			: base(f) {
		}
		public RidderRootFinder(UnaryFunction f,int niter,double pres)
			: base(f,niter,pres) {
		}
		protected override double Find() {
			// Vérifications d'usage
			if(m_xmin>=m_xmax) throw new RootFinderException(m_InvalidRange,0,new Range(m_xmin,m_xmax),0.0);						// Mauvaise plage de recherche
			double ans=-1.11e30,fh,fl,fm,fnew,s,xh=m_xmax,xl=m_xmin,xm,xnew;
			fl=m_f(m_xmin);
			fh=m_f(m_xmax);
			int iiter=0;
			if(Sign(fl)!=Sign(fh)) {
				for(;iiter<m_NbIterMax; iiter++) {
					// Compute the mid point
					xm=0.5*(xl+xh);
					fm=m_f(xm);
					s=Math.Sqrt(fm*fm-fl*fh);
					if(s==0.0) return (ans);
					// Updating formula
					xnew=xm+(xm-xl)*((fl>=fh?1.0:-1.0)*fm/s);
					// Maybe the new value is the good one
					if(Math.Abs(xnew-ans)<=m_Accuracy) return (ans);
					// Store this new point
					ans=xnew;
					fnew=m_f(ans);
					if(Sign(fm,fnew)!=fm) {
						xl=xm;
						fl=fm;
						xh=ans;
						fh=fnew;
					} else if(Sign(fl,fnew)!=fl) {
						xh=ans;
						fh=fnew;
					} else if(Sign(fh,fnew)!=fh) {
						xl=ans;
						fl=fnew;
					} else throw new Exception();
					if(Math.Abs(xh-xl)<=m_Accuracy) return (ans);
				}
				throw new RootFinderException(m_AccuracyNotReached,iiter,new Range(m_xmin,m_xmax),Math.Abs(xh-xl));									// nombre d'itérations autorisé dépassé
			} else {
				if(fl==0.0) return (m_xmin);
				if(fh==0.0) return (m_xmax);
			}
			throw new RootFinderException(m_AccuracyNotReached,iiter,new Range(m_xmin,m_xmax),Math.Abs(xh-xl));									// nombre d'itérations autorisé dépassé
		}
	}
}
