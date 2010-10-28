using System;

namespace Genetibase.MathX.Core
{
	/// <summary>
	/// Summary description for LexemeNameAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field,Inherited=false, AllowMultiple=false)]
	public class LexemeCodeAttribute : Attribute
	{
		private string _code;

		public LexemeCodeAttribute(string code)
		{
			_code = code;
		}

		public string Code
		{
			get
			{
				return _code;
			}
		}
  	}
}
