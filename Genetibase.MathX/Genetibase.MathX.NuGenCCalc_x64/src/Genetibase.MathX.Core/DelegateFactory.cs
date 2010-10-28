using System;

namespace Genetibase.MathX.Core
{
	/// <summary>
	/// This class supports the MatX.Core infrastructure and is not intended to be used
	/// directly from your code.
	/// </summary>
	internal class DelegateFactory
	{

		private class InverseExplicit2D
		{
			private RealFunction _x;
			public InverseExplicit2D(RealFunction x)
			{
				_x = x;
			}

			public RealFunction CreateDelegate()
			{
				return new RealFunction(Evaluate);
			}

			private double Evaluate(double x)
			{
				return 1/_x(x);
			}
		}


		private class Parameter2DToExplicit2D
		{
			private Parameter2DFunctionDelegate _function;

			public Parameter2DToExplicit2D(Parameter2DFunctionDelegate function)
			{
				_function = function;
			}

			public RealFunction CreateDelegateX()
			{
				return new RealFunction(FunctionX);
			}

			public RealFunction CreateDelegateY()
			{
				return new RealFunction(FunctionY);
			}

			private double FunctionX(double t)
			{
				return _function(t).X;
			}

			private double FunctionY(double t)
			{
				return _function(t).Y;
			}		
		}

		private class Parameter3DToExplicit2D
		{
			private Parameter3DFunctionDelegate _function;

			public Parameter3DToExplicit2D(Parameter3DFunctionDelegate function)
			{
				_function = function;
			}

			public RealFunction CreateDelegateX()
			{
				return new RealFunction(FunctionX);
			}

			public RealFunction CreateDelegateY()
			{
				return new RealFunction(FunctionY);
			}

			public RealFunction CreateDelegateZ()
			{
				return new RealFunction(FunctionY);
			}

			private double FunctionX(double t)
			{
				return _function(t).X;
			}

			private double FunctionY(double t)
			{
				return _function(t).Y;
			}		

			private double FunctionZ(double t)
			{
				return _function(t).Z;
			}		
		}

		private class ToParametric2D
		{
			private RealFunction _funcX;
			private RealFunction _funcY;

            private ConstantFunction _cFuncX;
            private ConstantFunction _cFuncY;
            

			public ToParametric2D(MulticastDelegate funcX,MulticastDelegate funcY)
			{
				_funcX = funcX as RealFunction;
				_funcY = funcY as RealFunction;	
		
				_cFuncX = funcX as ConstantFunction;
				_cFuncY = funcY as ConstantFunction;	
			}

			public Parameter2DFunctionDelegate CreateDelegate()
			{
				if ((_funcX != null)&&(_funcY != null))
					return new Parameter2DFunctionDelegate(funcRR);

				if ((_cFuncX != null)&&(_cFuncY != null))
					return new Parameter2DFunctionDelegate(funcCC);

				if ((_funcX != null)&&(_cFuncY != null))
					return new Parameter2DFunctionDelegate(funcRC);

				if ((_cFuncX != null)&&(_funcY != null))
					return new Parameter2DFunctionDelegate(funcCR);

				return null;
			}

			private Point2D funcRR(double t) {return new Point2D(_funcX(t),_funcY(t));}
			private Point2D funcCC(double t) {return new Point2D(_cFuncX(),_cFuncY());}
			private Point2D funcRC(double t) {return new Point2D(_funcX(t),_cFuncY());}
			private Point2D funcCR(double t) {return new Point2D(_cFuncX(),_funcY(t));}

		}

		private class ToParametric3D
		{
			private RealFunction _funcX;
			private RealFunction _funcY;
            private RealFunction _funcZ;

			private ConstantFunction _cFuncX;
			private ConstantFunction _cFuncY;
			private ConstantFunction _cFuncZ;
            

			public ToParametric3D(MulticastDelegate funcX,MulticastDelegate funcY, MulticastDelegate funcZ)
			{
				_funcX = funcX as RealFunction;
				_funcY = funcY as RealFunction;	
				_funcZ = funcZ as RealFunction;	
		
				_cFuncX = funcX as ConstantFunction;
				_cFuncY = funcY as ConstantFunction;	
				_cFuncZ = funcZ as ConstantFunction;	
			}

			public Parameter3DFunctionDelegate CreateDelegate()
			{
				if ((_funcX != null)&&(_funcY != null)&&(_funcZ != null))
					return new Parameter3DFunctionDelegate(funcRRR);

				if ((_cFuncX != null)&&(_cFuncY != null)&&(_cFuncZ != null))
					return new Parameter3DFunctionDelegate(funcCCC);


				if ((_funcX != null)&&(_funcY != null)&&(_cFuncZ != null))
					return new Parameter3DFunctionDelegate(funcRRC);

				if ((_funcX != null)&&(_cFuncY != null)&&(_funcZ != null))
					return new Parameter3DFunctionDelegate(funcRCR);

				if ((_funcX != null)&&(_cFuncY != null)&&(_cFuncZ != null))
					return new Parameter3DFunctionDelegate(funcRCC);

				if ((_cFuncX != null)&&(_funcY != null)&&(_cFuncZ != null))
					return new Parameter3DFunctionDelegate(funcCRC);

				if ((_cFuncX != null)&&(_cFuncY != null)&&(_funcZ != null))
					return new Parameter3DFunctionDelegate(funcCCR);


				return null;
			}

			private Point3D funcRRR(double t) {return new Point3D(_funcX(t),_funcY(t),_funcZ(t));}
			private Point3D funcCCC(double t) {return new Point3D(_cFuncX(),_cFuncY(),_cFuncZ());}

			private Point3D funcRRC(double t) {return new Point3D(_funcX(t),_funcY(t),_cFuncZ());}
			private Point3D funcRCR(double t) {return new Point3D(_funcX(t),_cFuncY(),_funcZ(t));}
			private Point3D funcRCC(double t) {return new Point3D(_funcX(t),_cFuncY(),_cFuncZ());}
			private Point3D funcCRC(double t) {return new Point3D(_cFuncX(),_funcY(t),_cFuncZ());}
			private Point3D funcCCR(double t) {return new Point3D(_cFuncX(),_cFuncY(),_funcZ(t));}

		}



		private DelegateFactory()
		{

		}

		public static Parameter2DFunctionDelegate CreateParameter2DFunctionDelegate(MulticastDelegate x, MulticastDelegate y)
		{
			return new ToParametric2D(x,y).CreateDelegate();
		}

		public static Parameter3DFunctionDelegate CreateParameter3DFunctionDelegate(MulticastDelegate x, MulticastDelegate y,MulticastDelegate z)
		{
			return new ToParametric3D(x,y,z).CreateDelegate();
		}

		public static RealFunction[] CreateRealFunctionFromParameter2DFunctionDelegate(Parameter2DFunctionDelegate function)
		{
			Parameter2DToExplicit2D p = new Parameter2DToExplicit2D(function);
			return new RealFunction[2]{p.CreateDelegateX(),p.CreateDelegateY()};
		}

		public static RealFunction[] CreateRealFunctionFromParameter3DFunctionDelegate(Parameter3DFunctionDelegate function)
		{
			Parameter3DToExplicit2D p = new Parameter3DToExplicit2D(function);
			return new RealFunction[3]{p.CreateDelegateX(),p.CreateDelegateY(),p.CreateDelegateZ()};
		}

		public static RealFunction InverseFunction(RealFunction function)
		{
			return new InverseExplicit2D(function).CreateDelegate();
		}
	}
}
