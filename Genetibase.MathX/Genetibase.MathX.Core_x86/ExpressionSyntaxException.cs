using System;

namespace Genetibase.MathX.Core
{
	/// <summary>
	/// The exception that is thrown when an symbolic methematical expression has syntax
	/// errors.
	/// </summary>
	public class ExpressionSyntaxException : Exception
	{
		private string _expression;
		private string _parsedExpression;

		public ExpressionSyntaxException(string message, string expression, string parsedExpression) : base (message)
		{			
			_expression = expression;
			_parsedExpression = parsedExpression;
		}

		/// <summary>Source expression, which has syntax errors.</summary>
		public string Expression
		{
			get
			{
				return _expression;
			}
		}

		
		/// <summary>
		/// Part of source expression that parsed succesfully and next lexeme has syntax
		/// error.
		/// </summary>
		public string ParsedExpression
		{
			get
			{
				return _parsedExpression;
			}
		}
	}
}
