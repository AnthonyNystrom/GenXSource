using System;

namespace Genetibase.MathX.Core
{	
	/// <summary>Differentiates symbolic expression by specified variable.</summary>
	public sealed class AnalyticDifferentiator
	{
		private AnalyticDifferentiator()
		{}

		/// <summary>Differentiates symbolic expression by specified variable.</summary>
		/// <returns>bynary tree that represents derivative of source tree</returns>
		/// <param name="tree"><strong>ExpressionTree</strong> binary tree of expression.</param>
		/// <param name="diffVar">variable of differentiation</param>
		public static ExpressionTree Differentiate(ExpressionTree tree, string diffVar)
		{
			if (tree == null) throw new ArgumentNullException("tree");
			if (diffVar == null) throw new ArgumentNullException("diffVar");

			ExpressionTree diffTree = new ExpressionTree(df(tree.RootNode,diffVar));			
			
			diffTree.Simplify();
			
			return new ExpressionTree(diffTree.ToString());		
		}

		private static string df(ExpressionTreeNode node, string diffVar)
		{
			if (!node.HasVariable(diffVar)) return "0";

			bool leftDiffVar = (node.Left != null) && (node.Left.HasVariable(diffVar));
			bool rightDiffVar = (node.Right != null) && (node.Right.HasVariable(diffVar));

			switch (node.ExpressionLexeme.LexemeType)
			{
				case LexemeType.ltConstant:					
				switch (node.ExpressionLexeme.Lexeme)
				{
					case Lexeme.Var: return "1";
					default: return "0";
				}
					
				case LexemeType.ltOperator:					
				switch (node.ExpressionLexeme.Lexeme)
				{
					case Lexeme.Plus:
					case Lexeme.Minus:
						return String.Format("({0}{1}{2})",
							df(node.Left,diffVar),
							node.ExpressionLexeme.LexemeName,
							df(node.Right,diffVar));
				
					case Lexeme.UMinus:
						return String.Format("(-({0}))",df(node.Left,diffVar));

					case Lexeme.Multiply:
						return String.Format("(({0})*({1})+({2})*({3}))", 
							df(node.Left, diffVar), 
							node.Right, 
							df(node.Right, diffVar), 
							node.Left);

					case Lexeme.Divide:
						return String.Format("(({0})*({1})-({2})*({3}))/({4})^2", 
							df(node.Left, diffVar), 
							node.Right, 
							df(node.Right, diffVar), 
							node.Left, 
							node.Right);
				
					case Lexeme.Power:
						// diff f(x)^g(y)
						if (leftDiffVar && !rightDiffVar)
						{
							return String.Format("((({0})^({1})*({1})*({2}))/({0}))",
								node.Left, node.Right, df(node.Left, diffVar));
						}
						// diff f(y)^g(x)
						if (!leftDiffVar && rightDiffVar)                                     
						{
							return String.Format("((({0})^({1}))*({2})*log({0})))", 
								node.Left, node.Right, df(node.Right, diffVar));           
						}
						// diff f(x)^g(x)
						return String.Format("((({0})^({1}))*(({3})*log({0})+({1})*({2})/({0})))", 
							node.Left, node.Right, 
							df(node.Left, diffVar), df(node.Right, diffVar));
					default: throw new Exception();
				}

				case LexemeType.ltFunction:										
					return String.Format("(("+node.ExpressionLexeme.LexemeDiffMask+")*({1}))",
						node.Left, df(node.Left, diffVar));

				default:
					throw new Exception();
			}
		}



		
	}
}
