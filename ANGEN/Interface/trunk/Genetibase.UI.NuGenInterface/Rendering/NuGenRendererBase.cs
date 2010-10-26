/* -----------------------------------------------
 * NuGenRendererBase.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.UI.NuGenInterface.Rendering
{
	/// <summary>
	/// Represents base class for renderers to imitate Microsoft Office 2007 style.
	/// </summary>
	public abstract class NuGenRendererBase
	{
		/// <summary>
		/// Gets or sets the <see cref="NuGenColorTable"/> applied to this <see cref="NuGenRendererBase"/>.
		/// </summary>
		public abstract NuGenColorTable ColorTable
		{
			get;
			set;
		}
	}
}
