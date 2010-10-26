/* -----------------------------------------------
 * INuGenFormStateStore.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenInterface
{
	/// <summary>
	/// Provides methods to persist from state.
	/// </summary>
	public interface INuGenFormStateStore
	{
		/// <summary>
		/// </summary>
		void RestoreFormState(Form formToRestore);

		/// <summary>
		/// </summary>
		void StoreFormState(Form formToStore);
	}
}
