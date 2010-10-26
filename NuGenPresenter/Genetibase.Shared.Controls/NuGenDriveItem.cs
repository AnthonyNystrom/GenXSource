/* -----------------------------------------------
 * NuGenDriveItem.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents an item for <see cref="NuGenDriveCombo"/>.
	/// </summary>
	public class NuGenDriveItem
	{
		private string _drive;

		/// <summary>
		/// </summary>
		public string Drive
		{
			get
			{
				return _drive;
			}
			set
			{
				_drive = value;
			}
		}

		private string _text;

		/// <summary>
		/// </summary>
		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
			}
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			return this.Text;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDriveItem"/> class.
		/// </summary>
		public NuGenDriveItem(string drive, string text)
		{
			_drive = drive;
			_text = text;
		}
	}
}
