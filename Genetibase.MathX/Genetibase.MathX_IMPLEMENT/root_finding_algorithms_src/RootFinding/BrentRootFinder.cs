using System;

namespace RootFinding
{
	public class BrentRootFinder : RootFinder
	{
		public BrentRootFinder(UnaryFunction f)
			: base(f) {
		}
		public BrentRootFinder(UnaryFunction f,int niter,double pres)
			: base(f,niter,pres) {
		}

		protected override double Find() {
			double a=m_xmin,b=m_xmax,c=m_xmax,d=0.0,e=0.0,min1,min2;
			double fa=m_f(a),fb=m_f(b),fc=fb,p,q,r,s,tol1,xm=double.NaN;

			// Vérifications d'usage
			if(m_xmin>=m_xmax||Sign(fa)==Sign(fb)) throw new RootFinderException(m_InvalidRange,0,new Range(m_xmin,m_xmax),0.0);
			int iiter=0;
			for(;iiter<m_NbIterMax; iiter++) {
				if(Sign(fb)==Sign(fc)) { c=a; fc=fa; e=d=b-a; }
				if(Math.Abs(fc)<Math.Abs(fb)) { a=b; fa=fb; b=c; fb=fc; c=a; fc=fa; }
				tol1=2.0*double_Accuracy*Math.Abs(b)+0.5*m_Accuracy;
				xm=0.5*(c-b);
				if(Math.Abs(xm)<=tol1||fb==0.0) return (b);
				if(Math.Abs(e)>=tol1&&Math.Abs(fa)>=Math.Abs(fa)) {
					s=fb/fa;
					if(a==c) { p=2.0*xm*s; q=1.0-s; } else {
						q=fa/fc;
						r=fb/fc;
						p=s*(2.0*xm*q*(q-r)-(b-a)*(r-1.0));
						q=(q-1.0)*(r-1.0)*(s-1.0);
					}
					if(p>0.0) q=-q;
					p=Math.Abs(p);
					min1=3.0*xm*q-Math.Abs(tol1*q);
					min2=Math.Abs(e*q);
					if(2.0*p<Math.Min(min1,min2)) {
						// On applique l'interpolation
						e=d;
						d=p/q;
					} else {
						// L'interpolation a échoué; on applique la méthode de bisection
						d=xm;
						e=d;
					}
				} else {
					// La décroissance est trop lente, on applique la méthode de bisection
					d=xm;
					e=d;
				}
				a=b; fa=fb;
				b+=(Math.Abs(d)>tol1?d:tol1*Sign(xm));
				/*
						if(Math.Abs(d)>tol1) b+=d;
						else b+=sign(tol1,xm);
				*/
				fb=m_f(b);
			}
			// L'algorithme a dépassé le nombre d'itérations autorisé
			throw new RootFinderException(m_AccuracyNotReached,iiter,new Range(a,b),Math.Abs(xm));
		}
	}
}
