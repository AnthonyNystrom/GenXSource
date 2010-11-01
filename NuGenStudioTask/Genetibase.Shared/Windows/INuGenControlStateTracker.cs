/* -----------------------------------------------
 * INuGenControlStateTracker.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// </summary>
	public interface INuGenControlStateTracker
	{
		/// <summary>
		/// </summary>
		void Enabled(object target);

		/// <summary>
		/// </summary>
		void Disabled(object target);

		/// <summary>
		/// </summary>
		void GotFocus(object target);

		/// <summary>
		/// </summary>
		void LostFocus(object target);

		/// <summary>
		/// </summary>
		/// <returns></returns>
		NuGenControlState GetControlState(object target);
	}
}
