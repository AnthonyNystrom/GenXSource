using System;
using System.Collections;
using System.Reflection;

namespace Genetibase.MathX.Core
{
	/// <summary>
	/// This class supports the MathX.Core infrastructure and is not intended to be used directly from your code. 
	/// </summary>
	internal class ExpressionLexemeInfo
	{
		private static readonly Hashtable _lexemeNames = GetLexemeNameTable();
		private static readonly Hashtable _lexemeTypes = GetLexemeTypeTable();
		private static readonly Hashtable _lexemeCodes = GetLexemeCodeTable();
		private static readonly Hashtable _lexemeDiffMasks = GetLexemeDiffMaskTable();

		public static Hashtable LexemeNames
		{
			get
			{
				return _lexemeNames;
			}
		}

		public static Hashtable LexemeTypes
		{
			get
			{
				return _lexemeTypes;
			}
		}

		public static Hashtable LexemeCodes
		{
			get
			{
				return _lexemeCodes;
			}
		}

		public static Hashtable LexemeDiffMasks
		{
			get
			{
				return _lexemeDiffMasks;
			}
		}

		private static Hashtable GetLexemeNameTable()
		{
			Hashtable result = new Hashtable();
			foreach (FieldInfo field in typeof(Lexeme).GetFields())
			{
				object[] attr = field.GetCustomAttributes(typeof(LexemeNameAttribute),false);
				if (attr.Length > 0)
				{
					result.Add(((LexemeNameAttribute)attr[0]).Name.ToLower(),(Lexeme)field.GetValue(""));
				}			
			}
			return result;
		}

		private static Hashtable GetLexemeTypeTable()
		{
			Hashtable result = new Hashtable();
			foreach (FieldInfo field in typeof(Lexeme).GetFields())
			{
				object[] attr = field.GetCustomAttributes(typeof(TypeOfLexemeAttribute),false);
				if (attr.Length > 0)
				{
					result.Add((Lexeme)field.GetValue(""),((TypeOfLexemeAttribute)attr[0]).LexemeType);
				}		
			}
			return result;
		}

		private static Hashtable GetLexemeCodeTable()
		{
			Hashtable result = new Hashtable();
			foreach (FieldInfo field in typeof(Lexeme).GetFields())
			{
				object[] attr = field.GetCustomAttributes(typeof(LexemeCodeAttribute),false);
				if (attr.Length > 0)
				{
					result.Add((Lexeme)field.GetValue(""),((LexemeCodeAttribute)attr[0]).Code);
				}		
			}
			return result;
		}

		private static Hashtable GetLexemeDiffMaskTable()
		{
			Hashtable result = new Hashtable();
			foreach (FieldInfo field in typeof(Lexeme).GetFields())
			{
				object[] attr = field.GetCustomAttributes(typeof(LexemeDiffMaskAttribute),false);
				if (attr.Length > 0)
				{
					result.Add((Lexeme)field.GetValue(""),((LexemeDiffMaskAttribute)attr[0]).Mask);
				}		
			}
			return result;
		}	

	
	}
}
