/* -----------------------------------------------
 * ExpressionToken.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Genetibase.NuGenVisiCalc.Operators;
using Genetibase.Shared.ComponentModel;

namespace Genetibase.NuGenVisiCalc.Expression
{
	internal sealed class ExpressionToken
	{
		#region Properties.Public

		public Boolean IsComma
		{
			get
			{
				if (_isEmpty)
				{
					return false;
				}

				return String.CompareOrdinal(TokenBody, ",") == 0;
			}
		}

		public Boolean IsLeftBracket
		{
			get
			{
				if (_isEmpty)
				{
					return false;
				}

				return String.CompareOrdinal(TokenBody, "(") == 0;
			}
		}

		public Boolean IsNumber
		{
			get
			{
				if (_isEmpty)
				{
					return false;
				}

				Double result;
				return Double.TryParse(TokenBody, out result);
			}
		}

		public Boolean IsOneVariableFunction
		{
			get
			{
				if (_isEmpty)
				{
					return false;
				}

				if (!IsOperator)
				{
					return false;
				}

				return OperatorDescriptor.InputParameterCount == 1;
			}
		}

		public Boolean IsOperator
		{
			get
			{
				if (_isEmpty)
				{
					return false;
				}

				if (Char.IsDigit(TokenBody[0]))
				{
					return false;
				}

				return OperatorsCache.Operators.ContainsKey(TokenBody);
			}
		}

		public Boolean IsRightBracket
		{
			get
			{
				if (_isEmpty)
				{
					return false;
				}

				return String.CompareOrdinal(TokenBody, ")") == 0;
			}
		}

		public Boolean IsTwoVariableFunction
		{
			get
			{
				if (_isEmpty)
				{
					return false;
				}

				if (!IsOperator)
				{
					return false;
				}

				return OperatorDescriptor.InputParameterCount == 2;
			}
		}

		public Boolean IsVariable
		{
			get
			{
				if (_isEmpty)
				{
					return false;
				}

				return Char.IsLetter(TokenBody[0]) && !IsOperator;
			}
		}

		public Boolean IsUnknownToken
		{
			get
			{
				if (_isEmpty)
				{
					return false;
				}

				return !(IsOperator || IsNumber || IsServiceToken || IsVariable);
			}
		}

		public Int32 OperatorPrecedence
		{
			get
			{
				if (_isEmpty)
				{
					return Int32.MinValue;
				}

				return OperatorDescriptor.Precedence;
			}
		}

		public OperatorDescriptor OperatorDescriptor
		{
			get
			{
				if (_isEmpty)
				{
					return null;
				}

				return OperatorsCache.Operators[TokenBody];
			}
		}

		private String _tokenBody;

		public String TokenBody
		{
			get
			{
				return _tokenBody;
			}
		}

		private Boolean IsServiceToken
		{
			get
			{
				if (_isEmpty)
				{
					return false;
				}

				return IsComma || IsLeftBracket || IsRightBracket;
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

		private OperatorsCache _operatorsCache;

		private OperatorsCache OperatorsCache
		{
			get
			{
				if (_operatorsCache == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_operatorsCache = this.ServiceProvider.GetService<OperatorsCache>();

					if (_operatorsCache == null)
					{
						throw new NuGenServiceNotFoundException<OperatorsCache>();
					}
				}

				return _operatorsCache;
			}
		}

		#endregion

		#region Methods.Public.Overridden

		public override Boolean Equals(Object obj)
		{
			if (obj is ExpressionToken)
			{
				return String.Compare(TokenBody, ((ExpressionToken)obj).TokenBody, StringComparison.OrdinalIgnoreCase) == 0;
			}

			return false;
		}

		public override Int32 GetHashCode()
		{
			return TokenBody.GetHashCode();
		}

		public override String ToString()
		{
			return TokenBody;
		}

		#endregion

		#region Methods.Public.Static

		/// <summary>
		/// </summary>
		/// <param name="expression"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="expression"/> is <see langword="null"/>.</para>
		/// </exception>
		public static ExpressionToken[] GetTokens(INuGenServiceProvider serviceProvider, String expression)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			if (expression == null)
			{
				throw new ArgumentNullException("expression");
			}

			List<ExpressionToken> result = new List<ExpressionToken>();
			Char[] chars = expression.ToLower().Replace(" ", "").ToCharArray();
			Int32 pos = 0;

			while (pos < chars.Length)
			{
				String item = "";

				// Number
				if (Char.IsDigit(chars[pos]))
				{
					while ((pos < chars.Length) && (Char.IsDigit(chars[pos]) || chars[pos] == '.'))
					{
						item += chars[pos++];
					}

					item = item.Replace(".", NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
					result.Add(new ExpressionToken(serviceProvider, item));

					continue;
				}

				// Variable or function
				if (Char.IsLetter(chars[pos]))
				{
					while (pos < chars.Length && Char.IsLetterOrDigit(chars[pos]))
					{
						item += chars[pos++];
					}

					result.Add(new ExpressionToken(serviceProvider, item));
					continue;
				}

				result.Add(new ExpressionToken(serviceProvider, Char.ToString(chars[pos])));
				pos++;
			}

			return result.ToArray();
		}

		#endregion

		private Boolean _isEmpty;
		public static readonly ExpressionToken Empty = new ExpressionToken();

		private ExpressionToken()
		{
			_isEmpty = true;
			_tokenBody = "\0";
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExpressionToken"/> class.
		/// </summary>
		/// <param name="tokenBody"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="tokenBody"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="tokenBody"/> is an empty string.</para>
		/// </exception>
		public ExpressionToken(INuGenServiceProvider serviceProvider, String tokenBody)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			if (String.IsNullOrEmpty(tokenBody))
			{
				throw new ArgumentNullException("tokenBody");
			}

			_serviceProvider = serviceProvider;
			_tokenBody = tokenBody;
		}
	}
}
