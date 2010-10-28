using System;

namespace Genetibase.MathX.Core
{
	/// <summary>
	/// Represents a node of <see cref="ExpressionTree">ExpressionTree Class</see> binary
	/// tree.
	/// </summary>
	public class ExpressionTreeNode
	{

		private ExpressionTreeNode _left;
		private ExpressionTreeNode _right;
		private ExpressionLexeme _expressionLexeme;

		public ExpressionTreeNode() : 
			this (ExpressionLexeme.EndLexeme, null,null)
		{
		}

		public ExpressionTreeNode(ExpressionLexeme lexeme) : 
			this (lexeme, null,null)
		{
		}

		public ExpressionTreeNode(ExpressionLexeme lexeme,ExpressionTreeNode leftNode) : 
			this (lexeme, leftNode,null)
		{
		}

		public ExpressionTreeNode(ExpressionLexeme lexeme,ExpressionTreeNode leftNode,ExpressionTreeNode rightNode )
		{
			ExpressionLexeme = lexeme;
			Left = leftNode;
			Right = rightNode;
		}

		/// <summary>Gets left child node.</summary>
		public ExpressionTreeNode Left
		{
			get
			{
				return _left;
			}
			set
			{
				_left = value;
			}
		}

		/// <summary><para>Gets right child node.</para></summary>
		public ExpressionTreeNode Right
		{
			get
			{
				return _right;
			}
			set
			{
				_right = value;
			}
		}
		/// <summary>Gets lexeme that node stores.</summary>
		public ExpressionLexeme ExpressionLexeme
		{
			get
			{
				return _expressionLexeme;
			}
			set
			{
				_expressionLexeme = value;
			}
		}

		/// <summary>Checks node and all child nodes for variable entry.</summary>
		public bool HasVariable(string variable)
		{
			if ((ExpressionLexeme.Lexeme == Lexeme.Var)
				&&( 0 == String.Compare(ExpressionLexeme.LexemeName,variable,true)))
				return true;
			if ((Left != null)&&(Left.HasVariable(variable)))
				return true;
			if ((Right != null)&&(Right.HasVariable(variable)))
				return true;

			return false;
		}

		/// <summary>Return code representation of node.</summary>
		public string ToCodeExpression()
		{
			switch (ExpressionLexeme.LexemeType)
			{
				case LexemeType.ltOperator:
				switch (ExpressionLexeme.Lexeme)
				{
					case Lexeme.UMinus:
						return String.Format("(-({0}))",Left.ToCodeExpression());
					case Lexeme.Power:
						return String.Format("Math.Exp(({1})*Math.Log({0}))",
							Left.ToCodeExpression(),
							Right.ToCodeExpression());
					default:
						return String.Format("({0}){1}({2})",
							Left.ToCodeExpression(),
							ExpressionLexeme.LexemeCode,
							Right.ToCodeExpression());
				}
				case LexemeType.ltFunction:
					return String.Format("{0}({1})",
						ExpressionLexeme.LexemeCode,
						Left.ToCodeExpression());
				case LexemeType.ltConstant:
					if (ExpressionLexeme.Lexeme == Lexeme.Number)
						return ExpressionLexeme.LexemeName.Replace(",",".");
					else return ExpressionLexeme.LexemeCode;
				default:
					return "";					
			}
		}

		/// <summary>Return string representation of node.</summary>
		public override string ToString()
		{
			switch (ExpressionLexeme.LexemeType)
			{
				case LexemeType.ltOperator:					
					
					if (ExpressionLexeme.Lexeme == Lexeme.UMinus)
					{
						if (Left.ExpressionLexeme.LexemeType == LexemeType.ltOperator)
							return String.Format("-({0})",Left);
						else
							return String.Format("-{0}",Left);
					}
					else
					{
						int precendence = ExpressionLexeme.LexemePrecedence;

						string leftPart = Left.ToString();
						string rightPart = Right.ToString();

						if ((Left.ExpressionLexeme.LexemeType == LexemeType.ltOperator)
							&&(precendence > Left.ExpressionLexeme.LexemePrecedence))
						{
							leftPart = String.Format("({0})",leftPart);
						}

						if ((Right.ExpressionLexeme.LexemeType == LexemeType.ltOperator)
							&&(precendence > Right.ExpressionLexeme.LexemePrecedence))
						{
							rightPart = String.Format("({0})",rightPart);
						}

						return String.Format("{0}{1}{2}",leftPart,ExpressionLexeme.LexemeName,rightPart);	
					}

				case LexemeType.ltFunction:
					return String.Format("{0}({1})",
						ExpressionLexeme.LexemeName,Left);

				case LexemeType.ltConstant:
					return ExpressionLexeme.LexemeName;

				default:

					break;
			}
			
			return "";
		}
	}
}
