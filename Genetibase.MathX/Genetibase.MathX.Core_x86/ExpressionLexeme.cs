using System;
using System.Collections;
using System.Reflection;

namespace Genetibase.MathX.Core
{

	/// <summary><para>Represents lexeme in expression.</para></summary>
	/// <seealso cref="Lexeme">Lexeme Enumeration</seealso>
	public struct ExpressionLexeme
	{
		private string _lexemeBody;
		private Lexeme _lexeme;

		public static readonly ExpressionLexeme EndLexeme = new ExpressionLexeme("",Lexeme.End);

		public ExpressionLexeme(string lexemeBody) : 
			this(lexemeBody,GetLexeme (lexemeBody))
		{
		}

		public ExpressionLexeme(string lexemeBody, Lexeme lexeme)
		{
			_lexemeBody = lexemeBody;
			_lexeme = lexeme;
		}

		/// <summary>Gets lexeme body.</summary>
		public string LexemeName
		{
			get
			{
				return this._lexemeBody;
			}
		}

		/// <summary>
		/// Gets <see cref="Lexeme">Lexeme Enumeration</see> specified with current lexeme in
		/// expression.
		/// </summary>
		public Lexeme Lexeme
		{
			get
			{
				return this._lexeme;
			}
		}

		/// <summary>Gets type of lexeme.</summary>
		/// <remarks>Lexeme in expression can be "operator", "function" or "contstant".</remarks>
		public LexemeType LexemeType
		{
			get
			{
				return GetLexemeType(_lexeme);
			}
		
		}

		/// <summary>Gets code representation of lexeme.</summary>
		public string LexemeCode
		{
			get
			{
				string code = GetLexemeCode(_lexeme);
				return code != null ? code : _lexemeBody;				
			}
		
		}

		/// <summary>Gets derivative mask of lexeme.</summary>
		public string LexemeDiffMask
		{
			get
			{
				return GetLexemeDiffMask(_lexeme);
			}
		
		}


		public static ExpressionLexeme[] GetLexemes(string expression)
		{
			ArrayList result = new ArrayList();

			char[] chars = expression.Replace(" ","").ToCharArray();
			int pos = 0;
			
			while (pos < chars.Length)
			{
				string item = "";

				// number
				if (char.IsDigit(chars[pos]))
				{
					while ((pos < chars.Length) && (char.IsDigit(chars[pos]) 
						|| chars[pos] == '.' || chars[pos] == ','))
						item += chars[pos++];

					item = item.Replace(".",System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
					item = item.Replace(",",System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);

					result.Add(new ExpressionLexeme(item));
					continue;
				}

				// variable or function
				if (char.IsLetter(chars[pos]))
				{
					while ( pos < chars.Length  
						&& char.IsLetterOrDigit(chars[pos]))
						item += chars[pos++];
					result.Add(new ExpressionLexeme(item));
					continue;
				}

				result.Add(new ExpressionLexeme(char.ToString(chars[pos])));
				pos++;
			}

			return (ExpressionLexeme[])result.ToArray(typeof(ExpressionLexeme));
		}

		public int LexemePrecedence
		{
			get
			{
				int level_base = 0;
				switch (_lexeme)
				{
					case Lexeme.Minus:
					case Lexeme.Plus:
						return level_base + 0;
					case Lexeme.Multiply:
					case Lexeme.Divide:
						return level_base + 1;
					case Lexeme.Power:
						return level_base + 2;
					case Lexeme.UMinus:
						return level_base + 3;
					default: 
						return level_base;
				}
			}
		
		}

		private static string GetLexemeCode(Lexeme lexeme)
		{			
			return (string)ExpressionLexemeInfo.LexemeCodes[lexeme];
		}

		private static string GetLexemeDiffMask(Lexeme lexeme)
		{			
			return (string)ExpressionLexemeInfo.LexemeDiffMasks[lexeme];
		}

		private static LexemeType GetLexemeType(Lexeme lexeme)
		{			
			if (ExpressionLexemeInfo.LexemeTypes.ContainsKey(lexeme))
				return (LexemeType)ExpressionLexemeInfo.LexemeTypes[lexeme];
			else
				return LexemeType.ltUndefined;
		}

		private static Lexeme GetLexeme(string lexeme)
		{			
			lexeme = lexeme.ToLower();

			if (ExpressionLexemeInfo.LexemeNames.ContainsKey(lexeme))
				return (Lexeme)ExpressionLexemeInfo.LexemeNames[lexeme];
			
			if (char.IsDigit(lexeme[0]))
			{
				try
				{
					double.Parse(lexeme);
				}
				catch
				{
					return Lexeme.Unknown;
				}
				return Lexeme.Number;
			}

			if (char.IsLetter(lexeme[0]))
			{
				// variable;
				foreach (char token in lexeme)
					if (!char.IsLetterOrDigit(token)) 
						return Lexeme.Unknown;
				return Lexeme.Var;
			}

			return Lexeme.Unknown;
		}

		

	


	}
}
