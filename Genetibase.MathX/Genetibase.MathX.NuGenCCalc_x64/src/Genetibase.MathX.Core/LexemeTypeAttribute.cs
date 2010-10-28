using System;

namespace Genetibase.MathX.Core
{
	/// <summary>
	/// Summary description for LexemeNameAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field,Inherited=false, AllowMultiple=false)]
	public class TypeOfLexemeAttribute : Attribute
	{
		private LexemeType _lexemeType;

		public TypeOfLexemeAttribute(LexemeType lexemeType)
		{
			_lexemeType = lexemeType;
		}

		public LexemeType LexemeType
		{
			get
			{
				return _lexemeType;
			}
		}
  	}
}
