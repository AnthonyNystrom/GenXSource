/* -----------------------------------------------
 * IAsset.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Windows.Controls.CompositionModel
{
	/// <summary>
	/// </summary>
	public interface IAsset<TDelegate>
	{
		/// <summary>
		/// </summary>
		TDelegate Invoke
		{
			get;
		}
		/// <summary>
		/// </summary>
		Boolean IsProducerActive
		{
			get;
		}
	}
}
