using System;

namespace Genetibase.MathX.Core
{
	/// <summary>Represents implicit function of two variables.</summary>
	/// <example>
	/// 	<code lang="CS">
	/// using System;
	/// using System.Drawing;
	///  
	/// using Genetibase.MathX.Core;
	/// using Genetibase.MathX.Core.Plotters;
	///  
	/// namespace Genetibase.MathX.Core.Tests
	/// {
	///     public class Implicit2DFunctionSample
	///     {        
	///         [STAThread]
	///         static void Main(string[] args)
	///         {
	///             // create function
	///             Implicit2DFunction function = new Implicit2DFunction("Sin(x)/y");
	///  
	///             // create function plotter to plot function in specified range
	///             Implicit2DFunctionPlotter plotter = new Implicit2DFunctionPlotter(function);
	///                 
	///             // write function roots in [0,0 - 10,10] range
	///             foreach (Point2D point in plotter.Plot(new Point2D(0,0), new Point2D(10,10), new Size(100,100)))
	///             {
	///                 Console.WriteLine("Function root at point ({0},{1})", point.X, point.Y);
	///             }
	///         }
	///     }
	/// }
	/// </code>
	/// </example>
	/// <remarks>
	/// To calculate function you must use
	/// <strong>Implicit2DFunctionPlotter</strong>.
	/// </remarks>
	public class Implicit2DFunction : Function
	{
		private BivariateRealFunction _function = null;
		private string _expression = null; 
		private Function _derivative = null;
		private ExpressionTree _expressionTree = null;

		/// <summary><para>Initializes a function that defined by delegate.</para></summary>
		public Implicit2DFunction(BivariateRealFunction function)
		{
			if (function == null)
				throw new ArgumentNullException("function");

			_function = function;

			base._delegate = _function;
			base._definitionType = DefinitionType.Numerical;
		}

		/// <summary>Initializes a function that defined by expression.</summary>
		/// <exception cref="System.ArgumentException" caption=""></exception>
		/// <exception cref="System.ArgumentNullException" caption=""></exception>
		/// <exception cref="ExpressionSyntaxException" caption="ExpressionSyntaxException"></exception>
		public Implicit2DFunction(string expression)
		{
			if (expression == null)
				throw new ArgumentNullException("expression");

			_expressionTree = new ExpressionTree(expression);

			if (_expressionTree.Variables.Length != 2)
				throw new ArgumentException("Implicit 2D function must have two variables in expression","expression");

			_expression = expression;



			ExpressionCompiler compiler = new ExpressionCompiler(_expressionTree);
			_function = (BivariateRealFunction) compiler.CreateDelegate(typeof(BivariateRealFunction));

			base._delegate = _function;
			base._definitionType = DefinitionType.Analytic;
		}

		private Implicit2DFunction (ExpressionTree tree)
		{
			_expressionTree = tree;

			_expression = _expressionTree.ToString();


			ExpressionCompiler compiler = new ExpressionCompiler(_expressionTree);
			_function = (BivariateRealFunction) compiler.CreateDelegate(typeof(BivariateRealFunction));
			
			base._delegate = _function;
			base._definitionType = DefinitionType.Analytic;
		}

		public new BivariateRealFunction ValueAt
		{
			get
			{
				return _function;
			}			
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
			string formula = 
				String.Format("-(({0})/({1}))", 
				AnalyticDifferentiator.Differentiate(_expressionTree,_expressionTree.Variables[0]),
				AnalyticDifferentiator.Differentiate(_expressionTree,_expressionTree.Variables[1]));

			ExpressionTree diffTree = new ExpressionTree(formula);

			switch (diffTree.Variables.Length)
			{
				case 0:
					return new Constant(diffTree.ToString());
				case 1:
					return new Explicit2DFunction(diffTree.ToString());
				case 2:
					return new Implicit2DFunction(diffTree.ToString());
				default:
					throw new InvalidOperationException();
			}

		}

		private Function CalculateNumericalDerivative()
		{
			return null;
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
