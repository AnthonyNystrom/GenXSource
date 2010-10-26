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
		void Deselect();
		
		/// <summary>
		/// </summary>
		void MouseEnter();
		
		/// <summary>
		/// </summary>
		void MouseLeave();
		
		/// <summary>
		/// </summary>
		void Select();
	}
}
