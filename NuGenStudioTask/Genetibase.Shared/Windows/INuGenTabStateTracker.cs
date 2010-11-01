/* -----------------------------------------------
 * INuGenTabStateTracker.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// </summary>
	public interface INuGenTabStateTracker : INuGenControlStateTracker
	{
		/// <summary>
		/// </summary>
		/// <param name="targetControl"></param>
		void Deselect(Control targetControl);
		
		/// <summary>
		/// </summary>
		/// <param name="targetControl"></param>
		void MouseEnter(Control targetControl);
		
		/// <summary>
		/// </summary>
		/// <param name="targetControl"></param>
		void MouseLeave(Control targetControl);
		
		/// <summary>
		/// </summary>
		/// <param name="targetControl"></param>
		void Select(Control targetControl);
	}
}
