/* -----------------------------------------------
 * INuGenReflectedImageGenerator.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.ControlReflectorInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenReflectedImageGenerator
	{
		/// <summary>
		/// </summary>
		void BuildReflectedImage(Bitmap ctrlImage, NuGenReflectStyle reflectStyle, int opacity);

		/// <summary>
		/// </summary>
		Bitmap GetControlImage(Control ctrl);
	}
}
