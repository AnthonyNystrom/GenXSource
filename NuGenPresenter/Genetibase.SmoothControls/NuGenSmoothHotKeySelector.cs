/* -----------------------------------------------
 * NuGenSmoothHotKeySelector.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="NuGenHotKeySelector"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothHotKeySelector), "Resources.NuGenIcon.png")]
	public class NuGenSmoothHotKeySelector : NuGenHotKeySelector
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothHotKeySelector"/> class.
		/// </summary>
		public NuGenSmoothHotKeySelector()
			: base(NuGenSmoothServiceManager.HotKeyPopupServiceProvider)
		{
		}
	}
}
