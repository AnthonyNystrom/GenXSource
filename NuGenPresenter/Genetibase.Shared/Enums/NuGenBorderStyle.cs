/* -----------------------------------------------
 * NuGenBorderStyle.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.Shared
{
	/// <summary>
	/// Specifies the style of the background and foreground for the node.
	/// </summary>
	/// <summary>
	/// Specifies the border style for the control.
	/// </summary>
	public enum NuGenBorderStyle
	{
		/// <summary>
		/// No border.
		/// </summary>
		None = 0x0000,

		/// <summary>
		/// The outer border is raised.
		/// </summary>
		RaisedOuter = 0x0001,

		/// <summary>
		/// The outer border is sunken.
		/// </summary>
		SunkenOuter = 0x0002,

		/// <summary>
		/// The inner border is raised.
		/// </summary>
		RaisedInner = 0x0004,

		/// <summary>
		/// Raised border.
		/// </summary>
		Raised = 0x0005,

		/// <summary>
		/// Etched border.
		/// </summary>
		Etched = 0x0006,

		/// <summary>
		/// The inner border is sunken.
		/// </summary>
		SunkenInner = 0x0008,

		/// <summary>
		/// Bump border.
		/// </summary>
		Bump = 0x0009,

		/// <summary>
		/// Sunken border.
		/// </summary>
		Sunken = 0x000A,

		/// <summary>
		/// Flat border.
		/// </summary>
		Flat = 0x0400A,

		/// <summary>
		/// Dashed border.
		/// </summary>
		Dashed = 0x0010,

		/// <summary>
		/// Dotted border.
		/// </summary>
		Dotted = 0x0011,

		/// <summary>
		/// Solid border.
		/// </summary>
		Solid = 0x0012
	}
}
