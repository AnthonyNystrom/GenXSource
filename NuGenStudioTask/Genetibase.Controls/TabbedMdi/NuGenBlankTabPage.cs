/* -----------------------------------------------
 * NuGenBlankTabPage.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls.Properties;

using System;
using System.Collections.Generic;

namespace Genetibase.Controls
{
	/// <summary>
	/// </summary>
	class NuGenBlankTabPage : NuGenTabPage
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenBlankTabPage"/> class.
		/// </summary>
		public NuGenBlankTabPage()
		{
			this.TabButtonImage = Resources.Blank;
			this.Text = Resources.BlankText;
		}

		#endregion
	}
}
