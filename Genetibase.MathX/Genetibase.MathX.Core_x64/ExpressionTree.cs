using System;
using System.Collections;

namespace Genetibase.MathX.Core
{
	/// <summary>
	/// Represents symbolic expression in binary tree of
	/// <see cref="ExpressionTreeNode">ExpressionTreeNode Class</see> nodes.
	/// </summary>
	public class ExpressionTree
	{
			
		ExpressionTreeNode _root;
		string _expression;		
		string _parsedExpression;
		ExpressionLexeme _curLexeme;
		Queue _lexemesQueue;
				
		private string[] _variables;

		private void CreateTree(ExpressionLexeme[] lexemes)
		{
			_root = null;
			_lexemesQueue = new Queue (lexemes);
			NextLexeme();

			_root = ExprLevel1();
			if (CurrentLexeme.Lexeme != ExpressionLexeme.EndLexeme.Lexeme)
				throw new ExpressionSyntaxException("operator expected", _expression, _parsedExpression);
		}

		// + - operators
		private ExpressionTreeNode ExprLevel1()
		{
			ExpressionTreeNode temp = ExprLevel2();
			ExpressionLexeme lex;

			while (true)
				switch (CurrentLexeme.Lexeme)
				{
					case Lexeme.Minus:
					case Lexeme.Plus:
						lex = CurrentLexeme;
						NextLexeme();
						ExpressionTreeNode right = ExprLevel2();
						if (right == null)
							throw new ExpressionSyntaxException("expression expected", _expression, _parsedExpression);
						temp = new ExpressionTreeNode(lex,temp,right);
						break;
					default:
						return temp;
				}
		}

		// * / operators
		private ExpressionTreeNode ExprLevel2()
		{
			ExpressionTreeNode temp = ExprLevel3();
			ExpressionLexeme lex;

			while (true)
				switch (CurrentLexeme.Lexeme)
				{
					case Lexeme.Multiply:
					case Lexeme.Divide:		
						if (temp == null) throw new ExpressionSyntaxException("expression expected", _expression, _parsedExpression);						
						lex = CurrentLexeme;
						NextLexeme();
						ExpressionTreeNode right = ExprLevel2();
						if (right == null) throw new ExpressionSyntaxException("expression expected", _expression, _parsedExpression);						
						temp = new ExpressionTreeNode(lex,temp,right);
						break;
					default:
						return temp;
				}
		}

		// ^ operator
		private ExpressionTreeNode ExprLevel3()
		{
			ExpressionTreeNode temp = ExprLevel4();
			ExpressionLexeme lex;

			while (true)
				switch (CurrentLexeme.Lexeme)
				{
					case Lexeme.Power:
						if (temp == null) throw new ExpressionSyntaxException("expression expected", _expression, _parsedExpression);												
						lex = CurrentLexeme;
						NextLexeme();
						ExpressionTreeNode right = ExprLevel3();
						if (right == null) throw new ExpressionSyntaxException("expression expected", _expression, _parsedExpression);												
						temp = new ExpressionTreeNode(lex,temp,right);
						break;
					default:
						return temp;
				}
		}

		// unary + - operators
		private ExpressionTreeNode ExprLevel4()
		{
			ExpressionTreeNode temp = null;
			ExpressionLexeme lex;

			switch (CurrentLexeme.Lexeme)
			{
				case Lexeme.Plus:
					NextLexeme();
					temp = ExprLevel5();
					break;
				case Lexeme.Minus:
					lex = new ExpressionLexeme("",Lexeme.UMinus);
					NextLexeme();
					ExpressionTreeNode right = ExprLevel5();						
					if (right == null) throw new ExpressionSyntaxException("expression expected", _expression, _parsedExpression);																		
					temp = new ExpressionTreeNode(lex,right);
					break;

				default:
					temp = ExprLevel5();
					break;
			}

			return temp;
		}

		// math functions
		private ExpressionTreeNode ExprLevel5()
		{
			ExpressionTreeNode temp = null;
			
			switch (CurrentLexeme.LexemeType)
			{									
				case LexemeType.ltFunction:
					temp = new ExpressionTreeNode(CurrentLexeme,null,null);
					NextLexeme();
					if (CurrentLexeme.Lexeme != Lexeme.LeftBracket)
						throw new ExpressionSyntaxException("( excepted", _expression, _parsedExpression);
					NextLexeme();

					ExpressionTreeNode left = ExprLevel1();						
					if (left == null) throw new ExpressionSyntaxException("expression expected", _expression, _parsedExpression);																							
					temp.Left = left;
					if (CurrentLexeme.Lexeme != Lexeme.RightBracket)
						throw new ExpressionSyntaxException(") excepted", _expression, _parsedExpression);
					NextLexeme();
					break;
				default:
					temp = ExprLevel6();
					break;
			}

			return temp;

		}

		// numbes, constants, variables and brakets
		private ExpressionTreeNode ExprLevel6()
		{
			ExpressionTreeNode temp = null;

			switch (CurrentLexeme.Lexeme)
			{
				case Lexeme.Number:
				case Lexeme.Pi:
				case Lexeme.E:
				case Lexeme.Var:
					temp = new ExpressionTreeNode(CurrentLexeme,null,null);
					NextLexeme();
					break;
				case Lexeme.LeftBracket:
					NextLexeme();
					temp = ExprLevel1();
					if (CurrentLexeme.Lexeme != Lexeme.RightBracket)
						throw new ExpressionSyntaxException(") excepted", _expression, _parsedExpression);
					NextLexeme();
					break;
			}

			return temp;
		}

		private void NextLexeme()
		{
			if (_lexemesQueue.Count > 0)
			{
				_parsedExpression += CurrentLexeme.LexemeName;
				_curLexeme = (ExpressionLexeme)_lexemesQueue.Dequeue();
			}
			else
				_curLexeme = ExpressionLexeme.EndLexeme;
		}
		
		private ExpressionLexeme CurrentLexeme
		{
			get
			{
				return this._curLexeme;
			}
		}


		/// <summary>
		/// Creates new instance of <strong>ExpressionTree</strong> with specified
		/// expression.
		/// </summary>
		/// <exception cref="System.ArgumentNullException" caption="">expression is null</exception>
		/// <exception cref="System.ArgumentException" caption="">expression has unknown lexemes</exception>
		/// <exception cref="ExpressionSyntaxException" caption="ExpressionSyntaxException Class">expression has syntax errors</exception>
		/// <example>
		/// 	<code lang="CS">
		/// ExpressionTree tree = new ExpressionTree("sin(x^2) + 10/x");
		/// </code>
		/// </example>
		/// <remarks></remarks>
		/// <param name="expression">An symbpolic mathematical expression.</param>
		public ExpressionTree(string expression)
		{
			if (expression == null) throw new ArgumentNullException("expression");
			if (expression == "") throw new ArgumentException("Blank expression not allowed");
		
			_expression = expression;			
			
			ExpressionLexeme[] lexemes = ExpressionLexeme.GetLexemes(expression);

			foreach (ExpressionLexeme lex in lexemes)
			{
				if (lex.Lexeme == Lexeme.Unknown)
					throw new ArgumentException(
						string.Format("Expression {0} has unknown lexeme \"{1}\"",_expression,lex.LexemeName),
						"expression");
			}
		
			_parsedExpression = "";
			CreateTree(lexemes);

			_variables =(string[]) new ArrayList(FindVariables(_root).Keys).ToArray(typeof(string));
			
		}

		private SortedList FindVariables(ExpressionTreeNode node)
		{
			SortedList list = new SortedList();

			if (node.ExpressionLexeme.Lexeme == Lexeme.Var)
			{
				list.Add(node.ExpressionLexeme.LexemeName.ToLower(),"");
			}

			if (node.Left != null)
				foreach(DictionaryEntry e in FindVariables(node.Left))
				{
					if (!list.ContainsKey(e.Key))
						list.Add(e.Key,e.Value);
				}
		
			if (node.Right != null)
				foreach(DictionaryEntry e in FindVariables(node.Right))
				{
					if (!list.ContainsKey(e.Key))
						list.Add(e.Key,e.Value);
				}

			return list;
		}

		/// <summary>Gets root node of binary tree.</summary>
		public ExpressionTreeNode RootNode
		{
			get
			{
				return this._root;
			}
		}

		/// <summary>Gets array of varibles, containig in expression.</summary>
		public string[] Variables
		{ 
			get
			{
				return _variables;
			}
		
		}
	
		/// <summary>Returns string representation of binary tree.</summary>
		public override string ToString()
		{
			return _root.ToString();
		}

		/// <summary>Returns code representation of binary tree.</summary>
		public string ToCodeExpression()
		{
			return _root.ToCodeExpression();
		}

		/// <summary>Simplifies binary tree of expression.</summary>
		/// <remarks>
		/// 	<para><strong>Simplify</strong> method removes all unneccessary nodes that can be
		/// simplifided. For example, expression "1+(x*0)" will be simplifided to "1" because (x*0)
		/// will always return zero.</para>
		/// 	<para>Other samples of simplification</para>
		/// 	<para>"0/x + x" -&gt; "x"</para>
		/// 	<para>"x/x" -&gt; "1"</para>
		/// </remarks>
		public void Simplify()
		{
			_root = SimplifyNode(_root);		
		}

		private ExpressionTreeNode SimplifyNode(ExpressionTreeNode node)
		{
			if (node.Left != null) node.Left = SimplifyNode(node.Left);
			if (node.Right != null)	node.Right = SimplifyNode(node.Right);

			bool l_null = node.Left == null;
			bool r_null = node.Right == null;

			bool l_number = !l_null && (node.Left.ExpressionLexeme.Lexeme == Lexeme.Number);
			bool r_number = !r_null && (node.Right.ExpressionLexeme.Lexeme == Lexeme.Number);

			double l_value = l_number ? double.Parse(node.Left.ExpressionLexeme.LexemeName) : 0;
			double r_value = r_number ? double.Parse(node.Right.ExpressionLexeme.LexemeName) : 0;

			bool l_zero = l_number && (l_value == 0);
			bool r_zero = r_number && (r_value == 0);
			
			bool l_one = l_number && (l_value == 1);
			bool r_one = r_number && (r_value == 1);

			switch (node.ExpressionLexeme.LexemeType)
			{
				case LexemeType.ltOperator:
						
					bool equal = (!l_null && !r_null) 
						&& (0==String.Compare(node.Left.ToString(),node.Right.ToString()));
					
				switch(node.ExpressionLexeme.Lexeme)
				{
					case Lexeme.Multiply:
						if (l_zero || r_zero) return new ExpressionTreeNode(
												  new ExpressionLexeme("0",Lexeme.Number));
						if (l_one) return node.Right;
						if (r_one) return node.Left;
						if (l_number && r_number) return new ExpressionTreeNode(
													  new ExpressionLexeme(Convert.ToString(l_value*r_value),Lexeme.Number));					
						break;

					case Lexeme.Divide:
						if (l_zero && !r_zero) return new ExpressionTreeNode(new ExpressionLexeme("0",Lexeme.Number));
						if (equal) return new ExpressionTreeNode(new ExpressionLexeme("1",Lexeme.Number));
						if (r_one) return node.Left;
						if (l_number && r_number) 
							return new ExpressionTreeNode(
								new ExpressionLexeme(Convert.ToString(l_value/r_value),Lexeme.Number));					
						break;
					
					case Lexeme.Plus:
						if (l_zero) return node.Right;
						if (r_zero) return node.Left;
						if (l_number && r_number) 
							return new ExpressionTreeNode(
								new ExpressionLexeme(Convert.ToString(l_value+r_value),Lexeme.Number));						
						
						if (node.Right.ExpressionLexeme.Lexeme == Lexeme.UMinus)
							return new ExpressionTreeNode(
								new ExpressionLexeme("-",Lexeme.Minus),node.Left,node.Right.Left);
						break;

					case Lexeme.Minus:
						if (r_zero) return node.Left;
						if (l_zero) return new ExpressionTreeNode(
										new ExpressionLexeme("-",Lexeme.UMinus),node.Right);

						if (equal) return new ExpressionTreeNode(new ExpressionLexeme("0",Lexeme.Number));
						if (l_number && r_number) 
							return new ExpressionTreeNode(
								new ExpressionLexeme(Convert.ToString(l_value-r_value),Lexeme.Number));						
						break;

					case Lexeme.UMinus:
						if (node.Left.ExpressionLexeme.Lexeme == Lexeme.UMinus)
							return node.Left.Left;
						break;
				}
					break;

			}
			
			return node;
		}

	}
}
