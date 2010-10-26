/* -----------------------------------------------
 * ExpressionTreeNode.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.NuGenVisiCalc.Operators;
using System.Globalization;

namespace Genetibase.NuGenVisiCalc.Expression
{
	internal sealed class ExpressionTreeNode
	{
		private ExpressionTreeNode _leftNode;

		public ExpressionTreeNode LeftNode
		{
			get
			{
				return _leftNode;
			}
			set
			{
				_leftNode = value;
			}
		}

		private ExpressionTreeNode _rightNode;

		public ExpressionTreeNode RightNode
		{
			get
			{
				return _rightNode;
			}
			set
			{
				_rightNode = value;
			}
		}

		private ExpressionToken _token;

		public ExpressionToken Token
		{
			get
			{
				return _token;
			}
		}

		public Int32 GetDeepLevel()
		{
			return GetDeepLevelInternal(this, 0);
		}

		private Int32 GetDeepLevelInternal(ExpressionTreeNode node, Int32 level)
		{
			if (node == null)
			{
				return level;
			}

			level++;

			return Math.Max(GetDeepLevelInternal(node.LeftNode, level), GetDeepLevelInternal(node.RightNode, level));
		}

		private String PartToString(ExpressionTreeNode node, Int32 currentPrecedence)
		{
			Debug.Assert(node != null, "node != null");
			String part = node.ToString();

			if (node.Token.IsOperator)
			{
				switch (node.Token.OperatorDescriptor.PrimitiveOperator)
				{
					case PrimitiveOperator.Add:
					case PrimitiveOperator.Sub:
					case PrimitiveOperator.Mul:
					case PrimitiveOperator.Div:
					{
						if (currentPrecedence > LeftNode.Token.OperatorPrecedence)
						{
							part = String.Format(CultureInfo.CurrentCulture, "({0})", part);
						}

						break;
					}
				}
			}

			return part;
		}

		public override string ToString()
		{
			if (Token.IsOperator)
			{
				switch (Token.OperatorDescriptor.PrimitiveOperator)
				{
					case PrimitiveOperator.Add:
					case PrimitiveOperator.Sub:
					case PrimitiveOperator.Mul:
					case PrimitiveOperator.Div:
					{
						Int32 precedence = Token.OperatorPrecedence;

						String leftPart = PartToString(LeftNode, precedence);
						String rightPart = PartToString(RightNode, precedence);

						return String.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", leftPart, Token.TokenBody, rightPart);
					}
				}
			}

			if (Token.IsOneVariableFunction)
			{
				return String.Format(CultureInfo.CurrentCulture, "{0}({1})", Token.TokenBody, LeftNode);
			}

			if (Token.IsTwoVariableFunction)
			{
				return String.Format(CultureInfo.CurrentCulture, "{0}({1},{2})", Token.TokenBody, LeftNode, RightNode);
			}

			if (Token.IsVariable)
			{
				return Token.TokenBody;
			}

			if (Token.IsNumber)
			{
				return Token.TokenBody.Replace(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator, ".");
			}

			throw new NotSupportedException(String.Format("Token \"{0}\" is not supported.", Token.TokenBody));
		}

		public ExpressionTreeNode(ExpressionToken token)
			: this(token, null, null)
		{
		}

		public ExpressionTreeNode(ExpressionToken token, ExpressionTreeNode leftNode)
			: this(token, leftNode, null)
		{
		}

		public ExpressionTreeNode(ExpressionToken token, ExpressionTreeNode leftNode, ExpressionTreeNode rightNode)
		{
			_token = token;
			_leftNode = leftNode;
			_rightNode = rightNode;
		}
	}
}
