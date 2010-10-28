using System;

namespace Genetibase.MathX.Core
{
	/// <summary>Represents a implicit function of three variables.</summary>
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
	///     public class Implicit3DFunctionSample
	///     {        
	///         [STAThread]
	///         static void Main(string[] args)
	///         {
	///             // create function
	///             Implicit3DFunction function = new Implicit3DFunction("x^2 + y^2 + z^2 - 10^2");
	///  
	///             // create function plotter to plot function in specified range
	///             Implicit3DFunctionPlotter plotter = new Implicit3DFunctionPlotter(function);
	///                 
	///             // write function roots in [0,0 - 10,10] range
	///             foreach (Point3D point in plotter.Plot(new Point3D(-20,-20,-20), new Point3D(20,20,20), 1,1,1))
	///             {
	///                 Console.WriteLine("Function root at point ({0},{1},{2})", point.X, point.Y, point.Z);
	///             }
	///         }
	///     }
	/// }
	/// </code>
	/// </example>
	public class Implicit3DFunction : Function
	{
		private TrivariateRealFunction _function = null;
		private string _expression = null;
		private Function _derivative = null;
		private ExpressionTree _expressionTree = null;

		/// <summary>Initializes a function that defined by delegate.</summary>
		public Implicit3DFunction(TrivariateRealFunction function) 
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
		/// using System.Drawing;
		///  
		/// using Genetibase.MathX.Core;
		/// using Genetibase.MathX.Core.Plotters;
		///  
		/// namespace Genetibase.MathX.Core.Tests
		/// {
		///     public class Implicit3DFunctionSample
		///     {        
		///         [STAThread]
		///         static void Main(string[] args)
		///         {
		///             // create function
		///             Implicit3DFunction function = new Implicit3DFunction("x^2 + y^2 + z^2 - 10^2");
		///  
		///             // create function plotter to plot function in specified range
		///             Implicit3DFunctionPlotter plotter = new Implicit3DFunctionPlotter(function);
		///                 
		///             // write function roots in [0,0 - 10,10] range
		///             foreach (Point3D point in plotter.Plot(new Point3D(-20,-20,-20), new Point3D(20,20,20), 1,1,1))
		///             {
		///                 Console.WriteLine("Function root at point ({0},{1},{2})", point.X, point.Y, point.Z);
		///             }
		///         }
		///     }
		/// }
		/// </code>
		/// </example>
		/// <exception cref="System.ArgumentException" caption=""></exception>
		/// <exception cref="System.ArgumentNullException" caption=""></exception>
		/// <exception cref="ExpressionSyntaxException" caption="ExpressionSyntaxException"></exception>
		/// <summary>Initializes a function that defined by expression.</summary>
		public Implicit3DFunction(string expression) 
		{
			if (expression == null)
				throw new ArgumentNullException("expression");

			_expressionTree = new ExpressionTree(expression);
			
			if (_expressionTree.Variables.Length != 3)
				throw new ArgumentException("Implicit 3D function must have three variables in expression","expression");

			_expression = expression;

			ExpressionCompiler compiler = new ExpressionCompiler(_expressionTree);
			_function = (TrivariateRealFunction) compiler.CreateDelegate(typeof(TrivariateRealFunction));
		
			base._delegate = _function;
			base._definitionType = DefinitionType.Analytic;
		}

		public new TrivariateRealFunction ValueAt
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
			return null;

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
