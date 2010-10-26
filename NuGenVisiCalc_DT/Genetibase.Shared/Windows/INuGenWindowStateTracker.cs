/* -----------------------------------------------
 * INuGenWindowStateTracker.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Drawing;
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
		/// Returns the actual window location if the form is in the normal state, or restore bounds location otherwise.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="targetForm"/> is <see langword="null"/>.</para>
		/// </exception>
		Point GetLocation(Form targetForm);

		/// <summary>
		/// Returns the actual window size if the form is in the normal state, or restore bounds size otherwise.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="targetForm"/> is <see langword="null"/>.</para>
		/// </exception>
		Size GetSize(Form targetForm);

		/// <summary>
		/// Returns the actual window state if the form is in normal or maximized state, or the previous
		/// window state if the form is minimized.
		/// </summary>
		/// <param name="targetForm"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="targetForm"/> is <see langword="null"/>.</para>
		/// </exception>
		FormWindowState GetWindowState(Form targetForm);
		
		/// <summary>
		/// </summary>
		/// <param name="targetForm"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="targetForm"/> is <see langword="null"/>.</para>
		/// </exception>
		void SetWindowState(Form targetForm);
	}
}
