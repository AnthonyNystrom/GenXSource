/* -----------------------------------------------
 * NuGenMiniBarLabel.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[DefaultProperty("Text")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenMiniBarLabel : NuGenMiniBarSpace
	{
		private string _str = "";

		/// <summary>
		/// </summary>
		[DefaultValue("")]
		public string Text
		{
			get
			{
				return _str;
			}
			set
			{
				if (value == null)
					value = "";
				_str = value;
				if (this.Owner != null)
				{
					this.Owner.MeasureButtons();
					this.Owner.Refresh();
				}
			}
		}
	}
}
