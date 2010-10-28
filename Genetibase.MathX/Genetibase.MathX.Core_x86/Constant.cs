using System;

namespace Genetibase.MathX.Core
{
	/// <summary>Represents a function without inputs and one doulble output.</summary>
	/// <remarks>
	/// 	<strong>Constant</strong> function is function like "5+3". It can be a result of
	/// derivativation Explicit2DFunction.
	/// </remarks>
	public class Constant : Function
	{
		private ConstantFunction _function = null;
		private string _expression = null; 
		private Function _derivative = null;		
		private ExpressionTree _expressionTree = null;

		public Constant(string expression) 
		{
			if (expression == null)
				throw new ArgumentNullException("expression");

			_expressionTree = new ExpressionTree(_expression);

			if (_expressionTree.Variables.Length != 0)
				throw new ArgumentException("Constant function cant have any variable in expression","expression");

			_expression = expression;

			ExpressionCompiler compiler = new ExpressionCompiler(_expressionTree);
			_function = (ConstantFunction) compiler.CreateDelegate(typeof(ConstantFunction));			

			base._delegate = _function;
			base._definitionType = DefinitionType.Analytic;
		}

		public Constant(ConstantFunction function) 
		{
			if (function == null)
				throw new ArgumentNullException("function");

			_function = function;		
			base._delegate = _function;
			base._definitionType = DefinitionType.Numerical;
		}
	

		public new ConstantFunction ValueAt
		{
			get
			{
				return _function;
			}			
		}

		private Function CalculateDerivative()
		{
			return new Constant("0");		
		}

		public override string Expression
		{
			get
			{				
				return _expression;
			}
		}

		public override Function Derivative
		{
			get
			{
				if (_derivative == null) _derivative = CalculateDerivative();
				return _derivative;
			}
		}


	}
}
