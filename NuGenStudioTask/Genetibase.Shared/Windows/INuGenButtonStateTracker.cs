/* -----------------------------------------------
 * INuGenButtonStateTracker.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// </summary>
	public interface INuGenButtonStateTracker : INuGenControlStateTracker
	{
		/// <summary>
		/// </summary>
		void MouseDown(object target);

		/// <summary>
		/// </summary>
		void MouseEnter(object target);
		
		/// <summary>
		/// </summary>
		void MouseLeave(object target);
		
		/// <summary>
		/// </summary>
		void MouseUp(object target);
	}
}
