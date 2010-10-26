/* -----------------------------------------------
 * NuGenSpin.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenSpinBase
	{
		/// <summary>
		/// </summary>
		protected sealed class SpinEdit : TextBox
		{
			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="SpinEdit"/> class.
			/// </summary>
			public SpinEdit()
			{
				this.BorderStyle = BorderStyle.None;
			}

			#endregion
		}
	}
}
