using System;

namespace RootFinding
{
	public interface IUnaryFunction {
		double Eval(double x);
	}

	public static class UnaryFunctions
	{
		public static UnaryFunction Identity() {
			return new UnaryFunction(delegate(double x) { return x; });
		}
		public static UnaryFunction Constant(double a) {
			return new UnaryFunction(delegate(double x) { return a; });
		}
	
		static public UnaryFunction Add(UnaryFunction f1,UnaryFunction f2) {
			return new UnaryFunction(delegate(double x) { return f1(x)+f2(x); });
		}
		static public UnaryFunction Multiply(UnaryFunction f,double lambda) {
			return new UnaryFunction(delegate(double x) { return lambda*f(x); });
		}
		static public UnaryFunction Minus(UnaryFunction f) {
			return new UnaryFunction(delegate(double x) { return -f(x); });
		}
		static public UnaryFunction Substract(UnaryFunction f1,UnaryFunction f2) {
			return new UnaryFunction(delegate(double x) { return f1(x)-f2(x); });
		}
		static public UnaryFunction Compound(UnaryFunction f1,UnaryFunction f2) {
			return new UnaryFunction(delegate(double x) { return f1(f2(x)); });
		}

	}
}
