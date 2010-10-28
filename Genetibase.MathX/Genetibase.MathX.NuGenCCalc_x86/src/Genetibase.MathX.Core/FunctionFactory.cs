using System;

namespace Genetibase.MathX.Core
{
	/// <summary>
	/// This class supports the MatX.Core infrastructure and is not intended to be used
	/// directly from your code.
	/// </summary>
	public sealed class FunctionFactory
	{

		public static Explicit2DFunction InverseExplicit2DFunction(Explicit2DFunction function)
		{
			if (function == null)
				throw new ArgumentNullException("function");

			switch (function.DefinitionType)
			{
				case DefinitionType.Analytic:
					return new Explicit2DFunction(
						string.Format("1/({0})",function.Expression));
				case DefinitionType.Numerical:
					return new Explicit2DFunction(
						DelegateFactory.InverseFunction(function.ValueAt));					
			}
			return null;
		}

		
		public static Function CreateRealFunction(ExpressionTree tree)
		{
			switch (tree.Variables.Length)
			{
				case 0:
					return new Constant(tree.ToString());
				case 1:
					return new Explicit2DFunction(tree.ToString());
				case 2:
					return new Explicit3DFunction(tree.ToString());
				default:
					return null;
			}
				
		}
	
	}
}
