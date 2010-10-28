using System;

namespace Genetibase.MathX.Core
{
	/// <summary>Represents a function with two double inputs and one Point3D output.</summary>
	/// <example>
	/// 	<code lang="CS">
	/// using System;
	/// using Genetibase.MathX.Core;
	///  
	/// namespace Genetibase.MathX.Core.Tests
	/// {
	///     public class ParametricSurfaceSample
	///     {        
	///         [STAThread]
	///         static void Main(string[] args)
	///         {
	///             // create function
	///             ParametricSurface function = new ParametricSurface("u+v","u-v","u*v");
	///  
	///             // calculate function _Function;
	///             for (int u = 0; u &lt; 100; u++)
	///             for (int v = 0; v &lt; 100; v++)
	///             {
	///                 Console.WriteLine("f({0},{1}) = ({2},{3},{4})",
	///                     u,v,function.ValueAt(u,v).X,function.ValueAt(u,v).Y, 
	///                     function.ValueAt(u,v).Z);
	///             }
	///                         
	///         }
	///     }
	/// }
	/// </code>
	/// </example>
	public class ParametricSurface : Function
	{
		private ParametricSurfaceDelegate _function = null;
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
		

		public ParametricSurface(ParametricSurfaceDelegate function)
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
		///     public class ParametricSurfaceSample
		///     {        
		///         [STAThread]
		///         static void Main(string[] args)
		///         {
		///             // create function
		///             ParametricSurface function = new ParametricSurface("u+v","u-v","u*v");
		///  
		///             // calculate function _Function;
		///             for (int u = 0; u &lt; 100; u++)
		///             for (int v = 0; v &lt; 100; v++)
		///             {
		///                 Console.WriteLine("f({0},{1}) = ({2},{3},{4})",
		///                     u,v,function.ValueAt(u,v).X,function.ValueAt(u,v).Y, 
		///                     function.ValueAt(u,v).Z);
		///             }
		///                         
		///         }
		///     }
		/// }
		/// </code>
		/// </example>
		public ParametricSurface(string expressionX,string expressionY,string expressionZ) 
		{
			if (expressionX == null) throw new ArgumentNullException("expressionX");
			if (expressionY == null) throw new ArgumentNullException("expressionY");
			if (expressionZ == null) throw new ArgumentNullException("expressionZ");

			_expressionTreeX = new ExpressionTree(expressionX);
			_expressionTreeY = new ExpressionTree(expressionY);
			_expressionTreeZ = new ExpressionTree(expressionZ);

			if (_expressionTreeX.Variables.Length > 2) throw new ArgumentException("expressionX");
			if (_expressionTreeY.Variables.Length > 2) throw new ArgumentException("expressionY");			
			if (_expressionTreeZ.Variables.Length > 2) throw new ArgumentException("expressionZ");			

			_expressionX = expressionX;
			_expressionY = expressionY;
			_expressionZ = expressionZ;

			_expression = string.Format("{0};{1};{2}",
				_expressionX,_expressionY,_expressionZ);

			ExpressionCompiler compiler = 
				new ExpressionCompiler(new ExpressionTree[] {
																_expressionTreeX,
																_expressionTreeY,
																_expressionTreeZ
															});
			
			_functionX = FunctionFactory.CreateRealFunction(_expressionTreeX);
			_functionY = FunctionFactory.CreateRealFunction(_expressionTreeY);
			_functionZ = FunctionFactory.CreateRealFunction(_expressionTreeZ);
			
			_function = (ParametricSurfaceDelegate)compiler.CreateDelegate(typeof(ParametricSurfaceDelegate),
				"return new Point3D({0},{1},{2});");

			base._delegate = _function;
			base._definitionType = DefinitionType.Analytic;
		}

		public new ParametricSurfaceDelegate ValueAt
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
