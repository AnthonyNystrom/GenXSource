/* -----------------------------------------------
 * INuGenControlImageManager.cs
 * Copyright � 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Drawing
{
	/// <summary>
	/// Indicates that this class operates a collection of <see cref="NuGenControlImageDescriptor"/> instances.
	/// </summary>
	public interface INuGenControlImageManager
	{
		/// <summary></summary>
		INuGenControlImageDescriptor CreateImageDescriptor();
	}
}
