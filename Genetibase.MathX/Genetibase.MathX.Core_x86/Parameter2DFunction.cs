using System;

namespace Genetibase.MathX.Core
{
	/// <summary>Represents a function with one double input and one Plot2D output</summary>
	/// <example>
	/// 	<code lang="CS">
	/// using System;
	/// using Genetibase.MathX.Core;
	///  
	/// namespace Genetibase.MathX.Core.Tests
	/// {
	///     public class Parameter2DFunctionSample
	///     {        
	///         [STAThread]
	///         static void Main(string[] args)
	///         {
	///             // create function
	///             Parameter2DFunction function = new Parameter2DFunction("10*sin(t)","10*cos(t)");
	///  
	///             // calculate function _Function;
	///             for (int i = 0; i &lt; 100; i++)
	///             {
	///                 Console.WriteLine("f({0}) = ({1},{2})",i,function.ValueAt(i).X,function.ValueAt(i).Y);
	///             }
	///                         
	///         }
	///     }
	/// }
	/// </code>
	/// </example>
	public class Parameter2DFunction : Function
	{
		private Parameter2DFunctionDelegate _function = null;
		private string _expression = null;         

		private string _expressionX = null; 
        private string _expressionY = null;
 
        private Function _functionX = null;
        private Function _functionY = null;
 
		private ExpressionTree _expressionTreeX = null;
		private ExpressionTree _expressionTreeY = null;
       		
		private Function _derivative = null;
		
		public Parameter2DFunction(Parameter2DFunctionDelegate function)
		{
			if (function == null)
				throw new ArgumentNullException("function");

			RealFunction[] rf = DelegateFactory.CreateRealFunctionFromParameter2DFunctionDelegate(function);

			_functionX = new Explicit2DFunction(rf[0]);
			_functionY = new Explicit2DFunction(rf[1]);

			_function = function;
			base._delegate = _function;
			base._definitionType = DefinitionType.Numerical;
		}

		public Parameter2DFunction(Function functionX,Function functionY)
		{
			if (functionX == null)
				throw new ArgumentNullException("functionX");
			if (functionY == null)
				throw new ArgumentNullException("functionY");

			if (functionX.ArgsCount > 1 || functionX.ReturnType != typeof(double))
				throw new ArgumentException("functionX must have double return type and <= 1 arguments","functionX");
			if (functionY.ArgsCount > 1 || functionY.ReturnType != typeof(double))
				throw new ArgumentException("functionY must have double return type and <= 1 arguments","functionY");

			if (functionX.DefinitionType != functionY.DefinitionType)
				throw new ArgumentException("functionX and functionY must have equal DefinitionType");

			_functionX = functionX;
			_functionY = functionY;

			_expressionX = _functionX.Expression;
			_expressionY = _functionY.Expression;

			_expression = (_functionX.DefinitionType == DefinitionType.Analytic) ?
				string.Format("{0};{1}",_expressionX,_expressionY) : null;

			_function = DelegateFactory.CreateParameter2DFunctionDelegate(
				_functionX.ValueAt,_functionY.ValueAt);

			base._delegate = _function;
			base._definitionType = functionX.DefinitionType;
		}

		/// <example>
		/// 	<code lang="CS">
		/// using System;
		/// using Genetibase.MathX.Core;
		///  
		/// namespace Genetibase.MathX.Core.Tests
		/// {
		///     public class Parameter2DFunctionSample
		///     {        
		///         [STAThread]
		///         static void Main(string[] args)
		///         {
		///             // create function
		///             Parameter2DFunction function = new Parameter2DFunction("10*sin(t)","10*cos(t)");
		///  
		///             // calculate function _Function;
		///             for (int i = 0; i &lt; 100; i++)
		///             {
		///                 Console.WriteLine("f({0}) = ({1},{2})",i,function.ValueAt(i).X,function.ValueAt(i).Y);
		///             }
		///                         
		///         }
		///     }
		/// }
		/// </code>
		/// </example>
		/// <summary>Initializes a function that defined by delegate.</summary>
		public Parameter2DFunction(string expressionX,string expressionY)
		{
			if (expressionX == null)
				throw new ArgumentNullException("expressionX");
			if (expressionY == null)
				throw new ArgumentNullException("expressionY");

			_expressionTreeX = new ExpressionTree(expressionX);
			_expressionTreeY = new ExpressionTree(expressionY);

			if (_expressionTreeX.Variables.Length > 1)
				throw new ArgumentException("expressionX");
			if (_expressionTreeY.Variables.Length > 1)
				throw new ArgumentException("expressionY");			

			_expressionX = expressionX;
			_expressionY = expressionY;

			_expression = string.Format("{0};{1}",_expressionX,_expressionY);

			ExpressionCompiler compilerX = new ExpressionCompiler(_expressionTreeX);
			ExpressionCompiler compilerY = new ExpressionCompiler(_expressionTreeY);

			if (_expressionTreeX.Variables.Length == 0)
				_functionX = new Constant(_expressionTreeX.ToString());
			else
				_functionX = new Explicit2DFunction(_expressionTreeX.ToString());

			if (_expressionTreeY.Variables.Length == 0)
				_functionY = new Constant(_expressionTreeY.ToString());
			else
				_functionY = new Explicit2DFunction(_expressionTreeY.ToString());
			
			_function = DelegateFactory.CreateParameter2DFunctionDelegate(
				_functionX.ValueAt,_functionY.ValueAt);

			base._delegate = _function;
			base._definitionType = DefinitionType.Analytic;
		}



		public new Parameter2DFunctionDelegate ValueAt
		{
			get
			{
				return _function;
			}			
		}

		private Function CalculateDerivative()
		{
			return new Parameter2DFunction(_functionX.Derivative,_functionY.Derivative);	
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
