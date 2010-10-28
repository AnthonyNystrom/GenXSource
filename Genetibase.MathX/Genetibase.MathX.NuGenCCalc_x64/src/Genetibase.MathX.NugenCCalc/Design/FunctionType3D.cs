using System;
using System.ComponentModel;

namespace Genetibase.MathX.NugenCCalc.Design
{
	/// <summary>
	/// Specifies the function type
	/// </summary>
	public enum FunctionType3D
	{
		/// <summary>
		/// Specify explicit type of 3D function 
		/// </summary>
		[Description("Explicit")] Explicit,
		/// <summary>
		/// Specify implicit type of 3D function 
		/// </summary>
		[Description("Implicit")] Implicit,
		/// <summary>
		/// Specify parametric type of 3D function 
		/// </summary>
		[Description("Parametric")] Parametric,
		/// <summary>
		/// Specify parametric surface type of 3D function 
		/// </summary>
		[Description("Parametric Surface")] ParametricSurface
	}
}
