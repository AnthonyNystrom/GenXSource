using System;

namespace Genetibase.MathX.Core
{
	/// <summary>Represents explicit two variable real function.</summary>
	/// <example>
	/// 	<code lang="CS">
	/// using System;
	/// using Genetibase.MathX.Core;
	///  
	/// namespace Genetibase.MathX.Core.Tests
	/// {
	///     public class Explicit3DFunctionSample
	///     {        
	///         [STAThread]
	///         static void Main(string[] args)
	///         {
	///             // create function
	///             Explicit3DFunction function = new Explicit3DFunction("Sin(x)/Cos(y)");
	///  
	///             // calculate function;
	///             for (int x = 0; x &lt; 100; x++)
	///             for (int y = 0; y &lt; 100; y++)
	///             {
	///                 Console.WriteLine("f({0},{1}) = {2}",x,y,function.ValueAt(x,y));
	///             }
	///                         
	///         }
	///     }
	/// }
	/// </code>
	/// </example>
	public class Explicit3DFunction : Function
	{
		private BivariateRealFunction _function = null;
		private string _expression = null; 
		private Function _derivative = null;		
		private ExpressionTree _expressionTree = null;

		/// <summary><para>Initializes a function that defined by delegate.</para></summary>
		public Explicit3DFunction(BivariateRealFunction function)
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
		///     public class Explicit3DFunctionSample
		///     {        
		///         [STAThread]
		///         static void Main(string[] args)
		///         {
		///             // create function
		///             Explicit3DFunction function = new Explicit3DFunction("Sin(x)/Cos(y)");
		///  
		///             // calculate function;
		///             for (int x = 0; x &lt; 100; x++)
		///             for (int y = 0; y &lt; 100; y++)
		///             {
		///                 Console.WriteLine("f({0},{1}) = {2}",x,y,function.ValueAt(x,y));
		///             }
		///                         
		///         }
		///     }
		/// }
		/// </code>
		/// </example>
		/// <summary><para>Initializes a function that defined by expression.</para></summary>
		/// <exception cref="System.ArgumentNullException" caption=""></exception>
		/// <exception cref="System.ArgumentException" caption=""></exception>
		/// <exception cref="ExpressionSyntaxException" caption="ExpressionSyntaxException Class"></exception>
		/// <param name="expression">Expression of two variables.</param>
		public Explicit3DFunction(string expression)
		{
			if (expression == null)
				throw new ArgumentNullException("expression");

			_expressionTree = new ExpressionTree(expression);

			if (_expressionTree.Variables.Length != 2)
				throw new ArgumentException("Explicit 3D function must have two variables in expression","expression");

			_expression = expression;

			ExpressionCompiler compiler = new ExpressionCompiler(_expressionTree);
			_function = (BivariateRealFunction) compiler.CreateDelegate(typeof(BivariateRealFunction));
			
			base._delegate = _function;
			base._definitionType = DefinitionType.Analytic;
		}

		private Explicit3DFunction (ExpressionTree tree)
		{
			_expressionTree = tree;
			_expression = _expressionTree.ToString();

			ExpressionCompiler compiler = new ExpressionCompiler(_expressionTree);
			_function = (BivariateRealFunction) compiler.CreateDelegate(typeof(BivariateRealFunction));
			
			base._delegate = _function;
			base._definitionType = DefinitionType.Analytic;
		}

		private Function CalculateDerivative()
		{
			return null;
		}
		

		public new BivariateRealFunction ValueAt
		{
			get
			{
				return _function;
			}			
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
