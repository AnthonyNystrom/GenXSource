/* -----------------------------------------------
 * INuGenControlStateTracker.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// </summary>
	public interface INuGenControlStateTracker
	{
		/// <summary>
		/// </summary>
		/// <param name="value"><see langword="true"/> to enable; <see langword="false"/> to disable.</param>
		void Enabled(bool value);

		/// <summary>
		/// </summary>
		void GotFocus();

		/// <summary>
		/// </summary>
		void LostFocus();

		/// <summary>
		/// </summary>
		/// <returns></returns>
		NuGenControlState GetControlState();
	}
}
