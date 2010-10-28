using System;
using System.Collections.Generic;
using System.Text;

namespace RootFinding
{
	public class FalsePositionRootFinder : RootFinder
	{
		public FalsePositionRootFinder(UnaryFunction f)
			: base(f) {
		}
		public FalsePositionRootFinder(UnaryFunction f,int niter,double pres)
			: base(f,niter,pres) {
		}
		protected override double Find() {
			double x1=m_xmin,x2=m_xmax,fl=m_f(x1),fh=m_f(x2),xl,xh,dx,del,f,rtf;
			if(fl*fh>0.0) throw new RootFinderException(m_RootNotBracketed,0,new Range(x1,x2),0.0);
			if(fl<0.0) {
				xl=x1;
				xh=x2;
			} else {
				xl=x2;
				xh=x1;
				Swap(ref fl, ref fh);
			}
			dx=xh-xl;
			int iiter=0;
			for(;iiter<m_NbIterMax;iiter++) {
				rtf=xl+dx*fl/(fl-fh);
				f=m_f(rtf);
				if(f < 0.0) {
					del=xl-rtf;
					xl=rtf;
					fl=f;
				} else {
					del=xh-rtf;
					xh=rtf;
					fh=f;
				}
				dx=xh-xl;
				if(Math.Abs(del)<m_Accuracy||f==0.0) return rtf;
			}
			throw new RootFinderException(m_RootNotFound,iiter,new Range(xl,xh),m_Accuracy);
		}
	}
}
