using System;

namespace Genetibase.MathX.Core
{
	/// <summary>Represents a function with one double input and one Point3D output.</summary>
	/// <example>
	/// 	<code lang="CS">
	/// using System;
	/// using Genetibase.MathX.Core;
	///  
	/// namespace Genetibase.MathX.Core.Tests
	/// {
	///     public class Parameter3DFunctionSample
	///     {        
	///         [STAThread]
	///         static void Main(string[] args)
	///         {
	///             // create function
	///             Parameter3DFunction function = new Parameter3DFunction("10*sin(t)","10*cos(t)", "10*tan(t)");
	///  
	///             // calculate function _Function;
	///             for (int i = 0; i &lt; 100; i++)
	///             {
	///                 Console.WriteLine("f({0}) = ({1},{2},{3})",i,function.ValueAt(i).X,function.ValueAt(i).Y, function.ValueAt(i).Z);
	///             }
	///                         
	///         }
	///     }
	/// }
	/// </code>
	/// </example>
	public class Parameter3DFunction : Function
	{
		private Parameter3DFunctionDelegate _function = null;
		private string _expression = null;

		        
		private string _expressionX = null;
        private string _expressionY = null;
        private string _expressionZ = null;
 
        private Function _functionX = null;
        private Function _functionY = null;
		private Function _functionZ = null;
 
		private ExpressionTree _expressionTreeX = null;
		private ExpressionTree _expressionTreeY = null;
		private ExpressionTree _expressionTreeZ = null;
       		
		private Function _derivative = null;
		

		public Parameter3DFunction(Parameter3DFunctionDelegate function)
		{
			if (function == null)
				throw new ArgumentNullException("function");

			RealFunction[] rf = DelegateFactory.CreateRealFunctionFromParameter3DFunctionDelegate(function);

			_functionX = new Explicit2DFunction(rf[0]);
			_functionY = new Explicit2DFunction(rf[1]);
			_functionZ = new Explicit2DFunction(rf[2]);

			_function = function;
			base._delegate = _function;
			base._definitionType = DefinitionType.Numerical;
		}


		public Parameter3DFunction(Function functionX,Function functionY, Function functionZ) 
		{
			if (functionX == null)
				throw new ArgumentNullException("functionX");
			if (functionY == null)
				throw new ArgumentNullException("functionY");
			if (functionZ == null)
				throw new ArgumentNullException("functionZ");

			if (functionX.ArgsCount > 1 || functionX.ReturnType != typeof(double))
				throw new ArgumentException("functionX must have double return type and <= 1 arguments","functionX");
			if (functionY.ArgsCount > 1 || functionY.ReturnType != typeof(double))
				throw new ArgumentException("functionY must have double return type and <= 1 arguments","functionY");
			if (functionZ.ArgsCount > 1 || functionZ.ReturnType != typeof(double))
				throw new ArgumentException("functionZ must have double return type and <= 1 arguments","functionZ");

			if ((functionX.DefinitionType != functionY.DefinitionType)||(functionX.DefinitionType != functionZ.DefinitionType))
				throw new ArgumentException("functionX, functionY and functionZ must have equal DefinitionType");

			_expressionX = functionX.Expression;
			_expressionY = functionY.Expression;
			_expressionZ = functionZ.Expression;

			_expression = (_functionX.DefinitionType == DefinitionType.Analytic) ?
				string.Format("{0};{1};{2}",_expressionX,_expressionY,_expressionZ) : null;

			_function = DelegateFactory.CreateParameter3DFunctionDelegate(
				_functionX.ValueAt,_functionY.ValueAt,_functionZ.ValueAt);

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
		///     public class Parameter3DFunctionSample
		///     {        
		///         [STAThread]
		///         static void Main(string[] args)
		///         {
		///             // create function
		///             Parameter3DFunction function = new Parameter3DFunction("10*sin(t)","10*cos(t)", "10*tan(t)");
		///  
		///             // calculate function _Function;
		///             for (int i = 0; i &lt; 100; i++)
		///             {
		///                 Console.WriteLine("f({0}) = ({1},{2},{3})",i,function.ValueAt(i).X,function.ValueAt(i).Y, function.ValueAt(i).Z);
		///             }
		///                         
		///         }
		///     }
		/// }
		/// </code>
		/// </example>
		/// <summary><para>Initializes a function that defined by expression.</para></summary>
		public Parameter3DFunction(string expressionX,string expressionY,string expressionZ) 
		{
			if (expressionX == null)
				throw new ArgumentNullException("expressionX");
			if (expressionY == null)
				throw new ArgumentNullException("expressionY");
			if (expressionZ == null)
				throw new ArgumentNullException("expressionZ");

			_expressionTreeX = new ExpressionTree(expressionX);
			_expressionTreeY = new ExpressionTree(expressionY);
			_expressionTreeZ = new ExpressionTree(expressionZ);

			if (_expressionTreeX.Variables.Length > 1)
				throw new ArgumentException("expressionX");
			if (_expressionTreeY.Variables.Length > 1)
				throw new ArgumentException("expressionY");			
			if (_expressionTreeZ.Variables.Length > 1)
				throw new ArgumentException("expressionZ");			

			_expressionX = expressionX;
			_expressionY = expressionY;
			_expressionZ = expressionZ;

			_expression = string.Format("{0};{1};{2}",
				_expressionX,_expressionY,_expressionZ);

			_functionX = FunctionFactory.CreateRealFunction(_expressionTreeX);
			_functionY = FunctionFactory.CreateRealFunction(_expressionTreeY);
			_functionZ = FunctionFactory.CreateRealFunction(_expressionTreeZ);
			
			_function = DelegateFactory.CreateParameter3DFunctionDelegate(
				_functionX.ValueAt,_functionY.ValueAt,_functionZ.ValueAt);

			base._delegate = _function;
			base._definitionType = DefinitionType.Analytic;
		}



		public new Parameter3DFunctionDelegate ValueAt
		{
			get
			{
				return _function;
			}			
		}

		private Function CalculateDerivative()
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
