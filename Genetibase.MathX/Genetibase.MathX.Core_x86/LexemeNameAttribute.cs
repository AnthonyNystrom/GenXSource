using System;

namespace Genetibase.MathX.Core
{
	/// <summary>
	/// Summary description for LexemeNameAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field,Inherited=false, AllowMultiple=false)]
	public class LexemeNameAttribute : Attribute
	{
		private string _name;

		public LexemeNameAttribute(string name)
		{
			_name = name;
		}

		public string Name 
		{
			get
			{
				return _name;
			}
		}
  	}
}
