using System;
using System.ComponentModel;

namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>
	/// Specifies the source type for NugenCCalc component
	/// </summary>
	public enum SourceType
	{
		/// <summary>
		/// Specify equation as source
		/// </summary>
		[Description("Equation")] Equation,
		/// <summary>
		/// Specify code expression as source
		/// </summary>
		[Description("Code expression")] CodeExpression
	}
}
