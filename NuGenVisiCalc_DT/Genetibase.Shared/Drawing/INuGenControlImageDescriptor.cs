/* -----------------------------------------------
 * INuGenControlImageDescriptor.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Drawing
{
	/// <summary>
	/// Encapsulates Image, ImageIndex, and ImageList associated data and logic.
	/// </summary>
	public interface INuGenControlImageDescriptor
	{
		/// <summary></summary>
		Image Image
		{
			get;
			set;
		}

		/// <summary></summary>
		event EventHandler ImageChanged;

		/// <summary></summary>
		int ImageIndex
		{
			get;
			set;
		}

		/// <summary></summary>
		event EventHandler ImageIndexChanged;

		/// <summary></summary>
		ImageList ImageList
		{
			get;
			set;
		}

		/// <summary></summary>
		event EventHandler ImageListChanged;
	}
}
