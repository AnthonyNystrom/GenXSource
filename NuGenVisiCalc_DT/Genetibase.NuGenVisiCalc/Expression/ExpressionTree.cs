/* -----------------------------------------------
 * ExpressionTree.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.NuGenVisiCalc.Operators;
using Genetibase.NuGenVisiCalc.Properties;
using Genetibase.Shared.ComponentModel;

namespace Genetibase.NuGenVisiCalc.Expression
{
	/// <summary>
	/// Represents  a symbolic expression in a binary tree with <see cref="ExpressionTreeNode"/> instances as nodes.
    /// </summary>
	internal sealed class ExpressionTree
	{
		#region Properties.Public

		/// <summary>Gets root node of binary tree.</summary>
		public ExpressionTreeNode RootNode
		{
			get
			{
				return _root;
			}
		}

		/// <summary>Gets array of varibles, containig in expression.</summary>
		public String[] Variables
		{
			get
			{
				return _variables;
			}
		}

		#endregion

		#region Properties.Services

		private INuGenServiceProvider _serviceProvider;

		private INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		#endregion

		#region Properties.Private

		private ExpressionToken _currentToken;

		private ExpressionToken CurrentToken
		{
			get
			{
				if (_currentToken == null)
				{
					_currentToken = ExpressionToken.Empty;
				}

				return _currentToken;
			}
		}

		#endregion

		#region Methods.Private

		private void CreateTree(ExpressionToken[] tokens)
		{
			_root = null;
			_tokensQueue = new Queue(tokens);
			NextToken();

			_root = ExprLevel1();

			if (CurrentToken != ExpressionToken.Empty)
			{
				throw new ExpressionSyntaxException(Resources.ExpressionSyntax_OperatorExpected, _expression, _parsedExpression);
			}
		}

		// +- operators
		private ExpressionTreeNode ExprLevel1()
		{
			ExpressionTreeNode temp = ExprLevel2();
			ExpressionToken token;

			while (true)
			{
				if (CurrentToken.IsOperator)
				{
					switch (CurrentToken.OperatorDescriptor.PrimitiveOperator)
					{
						case PrimitiveOperator.Sub:
						case PrimitiveOperator.Add:
						{
							token = CurrentToken;
							NextToken();
							ExpressionTreeNode right = ExprLevel2();

							if (right == null)
							{
								throw new ExpressionSyntaxException(Resources.ExpressionSyntax_ExpressionExpected, _expression, _parsedExpression);
							}

							temp = new ExpressionTreeNode(token, temp, right);
							break;
						}
						default:
						{
							return temp;
						}
					}
				}
				else
				{
					return temp;
				}
			}
		}

		// * / operators
		private ExpressionTreeNode ExprLevel2()
		{
			ExpressionTreeNode temp = ExprLevel3();
			ExpressionToken token;

			while (true)
			{
				if (CurrentToken.IsOperator)
				{
					switch (CurrentToken.OperatorDescriptor.PrimitiveOperator)
					{
						case PrimitiveOperator.Mul:
						case PrimitiveOperator.Div:
						{
							if (temp == null)
							{
								throw new ExpressionSyntaxException(Resources.ExpressionSyntax_ExpressionExpected, _expression, _parsedExpression);
							}

							token = CurrentToken;
							NextToken();
							ExpressionTreeNode right = ExprLevel2();

							if (right == null)
							{
								throw new ExpressionSyntaxException(Resources.ExpressionSyntax_ExpressionExpected, _expression, _parsedExpression);
							}

							temp = new ExpressionTreeNode(token, temp, right);
							break;
						}
						default:
						{
							return temp;
						}
					}
				}
				else
				{
					return temp;
				}
			}
		}

		// ^ operator
		private ExpressionTreeNode ExprLevel3()
		{
			return ExprLevel4();
		}

		// Unary +- operators
		private ExpressionTreeNode ExprLevel4()
		{
			ExpressionTreeNode temp = null;

			if (CurrentToken.IsOperator)
			{
				switch (CurrentToken.OperatorDescriptor.PrimitiveOperator)
				{
					case PrimitiveOperator.Add:
					{
						NextToken();
						temp = ExprLevel5();
						break;
					}
					case PrimitiveOperator.Sub:
					{
						NextToken();

						ExpressionTreeNode left = new ExpressionTreeNode(new ExpressionToken(ServiceProvider, "-1"));
						ExpressionTreeNode right = ExprLevel5();

						if (right == null)
						{
							throw new ExpressionSyntaxException(Resources.ExpressionSyntax_ExpressionExpected, _expression, _parsedExpression);
						}

						temp = new ExpressionTreeNode(new ExpressionToken(ServiceProvider, "*"), left, right);
						break;
					}
					default:
					{
						temp = ExprLevel5();
						break;
					}
				}
			}
			else
			{
				temp = ExprLevel5();
			}

			return temp;
		}

		// Math functions
		private ExpressionTreeNode ExprLevel5()
		{
			ExpressionTreeNode left = null;
			ExpressionTreeNode right = null;
			ExpressionTreeNode temp = null;

			if (CurrentToken.IsOneVariableFunction)
			{
				temp = new ExpressionTreeNode(CurrentToken, null, null);
				NextToken();

				if (!CurrentToken.IsLeftBracket)
				{
					throw new ExpressionSyntaxException(Resources.ExpressionSyntax_LeftBracketExpected, _expression, _parsedExpression);
				}

				NextToken();

				left = ExprLevel1();

				if (left == null)
				{
					throw new ExpressionSyntaxException(Resources.ExpressionSyntax_ExpressionExpected, _expression, _parsedExpression);
				}

				temp.LeftNode = left;
				
				if (!CurrentToken.IsRightBracket)
				{
					throw new ExpressionSyntaxException(Resources.ExpressionSyntax_RightBracketExpected, _expression, _parsedExpression);
				}

				NextToken();
			}
			else
			{
				if (CurrentToken.IsTwoVariableFunction)
				{
					temp = new ExpressionTreeNode(CurrentToken, null, null);
					NextToken();
					if (!CurrentToken.IsLeftBracket)
					{
						throw new ExpressionSyntaxException(Resources.ExpressionSyntax_LeftBracketExpected, _expression, _parsedExpression);
					}

					NextToken();

					left = ExprLevel1();
					if (left == null)
					{
						throw new ExpressionSyntaxException(Resources.ExpressionSyntax_ExpressionExpected, _expression, _parsedExpression);
					}

					temp.LeftNode = left;

					if (!CurrentToken.IsComma)
					{
						throw new ExpressionSyntaxException(Resources.ExpressionSyntax_CommaExpected, _expression, _parsedExpression);
					}

					NextToken();
					right = ExprLevel1();

					if (right == null)
					{
						throw new ExpressionSyntaxException(Resources.ExpressionSyntax_ExpressionExpected, _expression, _parsedExpression);
					}

					temp.RightNode = right;

					if (!CurrentToken.IsRightBracket)
					{
						throw new ExpressionSyntaxException(Resources.ExpressionSyntax_RightBracketExpected, _expression, _parsedExpression);
					}

					NextToken();
				}
				else
				{
					temp = ExprLevel6();
				}
			}

			return temp;
		}

		// Numbers, constants, variables and brakets.
		private ExpressionTreeNode ExprLevel6()
		{
			ExpressionTreeNode temp = null;

			if (CurrentToken.IsNumber || CurrentToken.IsVariable)
			{
				temp = new ExpressionTreeNode(CurrentToken, null, null);
				NextToken();
			}
			else
				if (CurrentToken.IsLeftBracket)
				{
					NextToken();
					temp = ExprLevel1();

					if (!CurrentToken.IsRightBracket)
					{
						throw new ExpressionSyntaxException(Resources.ExpressionSyntax_RightBracketExpected, _expression, _parsedExpression);
					}

					NextToken();
				}

			return temp;
		}

		private SortedList FindVariables(ExpressionTreeNode node)
		{
			SortedList list = new SortedList();

			if (node.Token.IsVariable)
			{
				list.Add(node.Token.TokenBody.ToLower(), "");
			}

			if (node.LeftNode != null)
			{
				foreach (DictionaryEntry e in FindVariables(node.LeftNode))
				{
					if (!list.ContainsKey(e.Key))
					{
						list.Add(e.Key, e.Value);
					}
				}
			}

			if (node.RightNode != null)
			{
				foreach (DictionaryEntry e in FindVariables(node.RightNode))
				{
					if (!list.ContainsKey(e.Key))
					{
						list.Add(e.Key, e.Value);
					}
				}
			}

			return list;
		}

		private void NextToken()
		{
			if (_tokensQueue.Count > 0)
			{
				_parsedExpression += CurrentToken.TokenBody;
				_currentToken = (ExpressionToken)_tokensQueue.Dequeue();
			}
			else
			{
				_currentToken = ExpressionToken.Empty;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>Returns String representation of binary tree.</summary>
		public override String ToString()
		{
			return _root.ToString();
		}

		#endregion

		private String _expression;
		private String _parsedExpression;
		private ExpressionTreeNode _root;
		private Queue _tokensQueue;
		private String[] _variables;

		/// <summary>
		/// Initializes a new instance of the <see cref="ExpressionTree"/> class.
		/// </summary>
		/// <param name="expression">Symbolic expression (e.g. "sin(x^2) + 10/x").</param>
		/// <exception cref="ArgumentException">
		/// <para><paramref name="expression"/> has unknown tokens.</para>
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="expression"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="expression"/> is an empty string.</para>
		/// </exception>
		/// <exception cref="ExpressionSyntaxException">Specified <paramref name="expression"/> has syntax errors.</exception>
		public ExpressionTree(INuGenServiceProvider serviceProvider, String expression)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			if (String.IsNullOrEmpty(expression))
			{
				throw new ArgumentNullException("expression");
			}

			_serviceProvider = serviceProvider;
			_expression = expression;

			ExpressionToken[] tokens = ExpressionToken.GetTokens(ServiceProvider, expression);

			foreach (ExpressionToken token in tokens)
			{
				if (token.IsUnknownToken)
				{
					throw new ArgumentException(String.Format(Resources.Argument_UnknownToken, _expression, token.TokenBody));
				}
			}

			_parsedExpression = "";
			CreateTree(tokens);

			_variables = (String[])new ArrayList(FindVariables(_root).Keys).ToArray(typeof(String));
		}
	}
}
