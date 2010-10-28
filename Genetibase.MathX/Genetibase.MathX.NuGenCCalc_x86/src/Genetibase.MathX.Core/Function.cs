using System;

namespace Genetibase.MathX.Core
{
	
	/// <summary>Represents base abstract class for mathematical functions.</summary>
	public abstract class Function
	{
		protected MulticastDelegate _delegate;
		protected DefinitionType _definitionType;
		
		/// <summary>Returns function, which represents derivative of current function.</summary>
		/// <remarks>
		/// In derived classes, you must ovverride this property to calculate derivatives. If
		/// you function type doesn't have derivatives, simply return null.
		/// </remarks>
		public abstract Function Derivative	{get;}		
		/// <summary>
		/// Gets string expression that represents analytic representation of
		/// function.
		/// </summary>
		/// <remarks>
		/// In derived classes, you must ovverride this property to return analityc
		/// representation of function. If function is numerical defined, you must return
		/// null.
		/// </remarks>
		public abstract string Expression {get;}


		/// <summary>Gets definition type of function.</summary>
		/// <remarks>
		/// Functions can be defined in two ways - analytic (by expression) or numerical (by
		/// delegate). <strong>DefinitionType</strong> property, represents how function is
		/// defined.
		/// </remarks>
		public DefinitionType DefinitionType
		{
			get
			{
				return _definitionType;
			}
		}

		/// <summary>Gets delegate, which calcilates function.</summary>
		/// <value>Delegate of function.</value>
		/// <remarks>
		/// In derived classes, you can override this property with keyword
		/// <strong>new</strong> to concretize delegate type. For example, in
		/// <a href="Genetibase.MathX.Core~Genetibase.MathX.Core.Explicit2DFunction.html">Explicit2DFunction
		/// Class</a>, <strong>ValueAt</strong> property concretized to
		/// <a href="Genetibase.MathX.Core~Genetibase.MathX.Core.RealFunction.html">RealFunction
		/// Delegate</a>.
		/// </remarks>
		public MulticastDelegate ValueAt
		{
			get
			{
				return _delegate;
			}
		}

		/// <summary>Gets count of argumets in function delegate.</summary>
		public int ArgsCount
		{
			get
			{
				return _delegate.Method.GetParameters().Length;
			}
		}

		/// <summary><para>Gets type of return value.</para></summary>
		public Type ReturnType
		{
			get
			{
				return _delegate.Method.ReturnType;
			}
		}
	}
}
