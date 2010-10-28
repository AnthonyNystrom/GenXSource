using System;
using System.ComponentModel;

namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>
	/// Specifies the plot mode for MathX plotters
	/// </summary>
	public enum PlotMode
	{
		/// <summary>
		/// Specify "By points number" plot mode
		/// </summary>
		[Description("By points number")] ByNumPoints,
		/// <summary>
		/// Specify "By step" plot mode
		/// </summary>
		[Description("By step")] ByStep,
		/// <summary>
		/// Specify "By client area" plot mode
		/// </summary>
		///[Description("By client area")] ByClientArea
	}
}
