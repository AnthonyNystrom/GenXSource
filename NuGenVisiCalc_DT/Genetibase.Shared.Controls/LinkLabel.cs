/* -----------------------------------------------
 * LinkLabel.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <seealso cref="NuGenToolStripLinkLabel"/>
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public class LinkLabel : NuGenToolStripLinkLabel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LinkLabel"/> class.
		/// </summary>
		public LinkLabel()
			: base(NuGenServiceManager.LabelServiceProvider)
		{
		}
	}
}
