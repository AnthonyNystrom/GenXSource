/* -----------------------------------------------
 * INuGenWindowStateTracker.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Use <see cref="SetWindowState"/> to set the current window state
	/// (e.g. in <see cref="M:System.Windows.Forms.Form.OnSizeChanged"/> method).
	/// Use <see cref="GetWindowState"/> to retrieve the actual window state
	/// if the form is in normal or maximized state, or the previous window state if
	/// the form is minimized.
	/// </summary>
	public interface INuGenWindowStateTracker
	{
		/// <summary>
		/// Returns the actual window state if the form is in normal or maximized state, or the previous
		/// window state if the form is minimized.
		/// </summary>
		/// <param name="targetForm"></param>
		/// <returns></returns>
		FormWindowState GetWindowState(Form targetForm);
		
		/// <summary>
		/// </summary>
		/// <param name="targetForm"></param>
		void SetWindowState(Form targetForm);
	}
}
