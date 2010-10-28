using System;

namespace Genetibase.MathX.Core
{
	/// <summary>
	/// Summary description for LexemeDiffMaskAttribute.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field,Inherited=false, AllowMultiple=false)]
	public class LexemeDiffMaskAttribute : Attribute
	{
		private string _mask;

		public LexemeDiffMaskAttribute(string mask)
		{
			_mask = mask;
		}

		public string Mask
		{
			get
			{
				return _mask;
			}
		}
  	}
}
