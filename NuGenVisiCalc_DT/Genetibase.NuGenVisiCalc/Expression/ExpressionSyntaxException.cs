/* -----------------------------------------------
 * ExpressionSyntaxException.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Genetibase.NuGenVisiCalc.Expression
{
	[Serializable]
	internal class ExpressionSyntaxException : Exception
	{
		private String _expression;

		public String Expression
		{
			get
			{
				return _expression;
			}
		}

		private String _parsedExpression;

		public String ParsedExpression
		{
			get
			{
				return _parsedExpression;
			}
		}

		public ExpressionSyntaxException()
		{
		}

		public ExpressionSyntaxException(String message, String expression, String parsedExpression) : base(message)
		{
			_expression = expression;
			_parsedExpression = parsedExpression;
		}

		public ExpressionSyntaxException(String message, String expression, String parsedExpression, Exception inner) : base(message, inner)
		{
			_expression = expression;
			_parsedExpression = parsedExpression;
		}

		protected ExpressionSyntaxException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
