using System;

namespace RootFinding
{
	public class NewtonRootFinder : RootFinder
	{
		protected UnaryFunction m_df;
		public NewtonRootFinder(UnaryFunction f,UnaryFunction df)
			: base(f) {
			m_df=df;
		}
		public NewtonRootFinder(UnaryFunction f,UnaryFunction df,int niter,double pres)
			: base(f,niter,pres) {
			m_df=df;
		}

		protected override double Find() {
			double dx=0.0,x;
			const double h=1.0e-5,xacc=1.0e-5;
			if(m_xmin>=m_xmax) throw new ArgumentException("Mauvaise plage de recherche");
			x=0.5*(m_xmin+m_xmax);
			int iiter=0;
			for(; iiter<m_NbIterMax; iiter++) {
				dx=m_f(x)/m_df(x);
				x-=dx;
				if(Sign(m_xmin-x)!=Sign(x-m_xmax)) throw new RootFinderException(m_InvalidRange,iiter,new Range(x,x+dx),Math.Abs(dx));
				if(Math.Abs(dx)<xacc) return x;
			}
			// L'algorithme a dépassé le nombre d'itérations autorisé
			throw new RootFinderException(m_InvalidRange,iiter,new Range(x,x+dx),Math.Abs(dx));
		}
	}
}
