using System;

namespace Genetibase.MathX.Core
{
	/// <summary>Represent explicit one variable real function.</summary>
	/// <example>
	/// 	<code lang="CS" source="">
	/// 	</code>
	/// 	<code lang="CS">
	/// using System;
	/// using Genetibase.MathX.Core;
	///  
	/// namespace Genetibase.MathX.Core.Tests
	/// {
	///     public class Explicit2DFunctionSample
	///     {        
	///         [STAThread]
	///         static void Main(string[] args)
	///         {
	///             // create function
	///             Explicit2DFunction function = new Explicit2DFunction("Sin(x)/x");
	///  
	///             // calculate function _Function;
	///             for (int i = 0; i &lt; 100; i++)
	///             {
	///                 Console.WriteLine("f({0}) = {1}",i,function.ValueAt(i));
	///             }
	///                         
	///         }
	///     }
	/// }
	/// </code>
	/// </example>
	public class Explicit2DFunction : Function
	{
		private RealFunction _function = null;
		private string _expression = null; 
		private Function _derivative = null;		
		private ExpressionTree _expressionTree = null;

		/// <summary>
		/// Creates function instance by specified <strong>RealFunction</strong>
		/// delegate.
		/// </summary>
		/// <param name="function"><strong>RealFunction</strong> delegate.</param>
		public Explicit2DFunction(RealFunction function) 
		{
			if (function == null)
				throw new ArgumentNullException("function");

			_function = function;
			base._delegate = _function;

			base._definitionType = DefinitionType.Numerical;
		}

		/// <example>
		/// 	<code lang="CS">
		/// using System;
		/// using Genetibase.MathX.Core;
		///  
		/// namespace Genetibase.MathX.Core.Tests
		/// {
		///     public class Explicit2DFunctionSample
		///     {        
		///         [STAThread]
		///         static void Main(string[] args)
		///         {
		///             // create function
		///             Explicit2DFunction function = new Explicit2DFunction("Sin(x)/x");
		///  
		///             // calculate function _Function;
		///             for (int i = 0; i &lt; 100; i++)
		///             {
		///                 Console.WriteLine("f({0}) = {1}",i,function.ValueAt(i));
		///             }
		///                         
		///         }
		///     }
		/// }
		/// </code>
		/// </example>
		/// <summary>Creates function instance by specified expression.</summary>
		/// <exception cref="System.ArgumentNullException" caption=""></exception>
		/// <exception cref="System.ArgumentException" caption=""></exception>
		/// <exception cref="ExpressionSyntaxException" caption="ExpressionSyntaxException"></exception>
		/// <param name="expression">An mathematical expression with one variable.</param>
		public Explicit2DFunction(string expression)
		{
			if (expression == null)
				throw new ArgumentNullException("expression");

			_expressionTree = new ExpressionTree(expression);
			
			if (_expressionTree.Variables.Length != 1)
				throw new ArgumentException("Explicit 2D function must have one variable in expression","expression");

			_expression = expression;

			ExpressionCompiler compiler = new ExpressionCompiler(_expressionTree);
			_function = (RealFunction) compiler.CreateDelegate(typeof(RealFunction));

			base._delegate = _function;
			base._definitionType = DefinitionType.Analytic;
		}

		private Explicit2DFunction (ExpressionTree tree)
		{
			_expressionTree = tree;
			_expression = tree.ToString();

			ExpressionCompiler compiler = new ExpressionCompiler(_expressionTree);			
			_function = (RealFunction) compiler.CreateDelegate(typeof(RealFunction));
			base._delegate = _function;
			
			base._definitionType = DefinitionType.Analytic;
		}

		private Function CalculateDerivative()
		{
			switch (_definitionType)
			{
				case DefinitionType.Analytic:
					return CalculateAnalyticDerivative();					
				case DefinitionType.Numerical:
					return CalculateNumericalDerivative();
				default:
					return null;
			}		
		}

		private Function CalculateAnalyticDerivative()
		{
			ExpressionTree diffTree = AnalyticDifferentiator.Differentiate(_expressionTree,_expressionTree.Variables[0]);

			switch (diffTree.Variables.Length)
			{
				case 0:
					return new Constant(diffTree.ToString());
				case 1:
					return new Explicit2DFunction(diffTree);
				default:
					throw new InvalidOperationException();
			}			
		}

		private Function CalculateNumericalDerivative()
		{
			return new Explicit2DFunction(
				NumericalDifferentiator.CreateDelegate(_function));
		}

		/// <summary>Gets <strong>RealFunction</strong> delegate of function.</summary>
		public new RealFunction ValueAt
		{
			get
			{
				return _function;
			}			
		}
		
		/// <summary>
		/// Gets expression of function if function is analiticaly defined. Otherwise returns
		/// null.
		/// </summary>
		public override string Expression
		{
			get
			{				
				return _expression;
			}
		}

		/// <summary><para>Gets derivative of function.</para></summary>
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
