/* -----------------------------------------------
 * ICommandCollection.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;

namespace Genetibase.Windows.Controls.Framework.Commands
{
	/// <summary>
	/// </summary>
	public interface ICommandCollection : ICollection, IEnumerable
	{
		/// <summary>
		/// </summary>
		Boolean Contains(String command);
		/// <summary>
		/// </summary>
		void CopyTo(String[] array, Int32 index);

		/// <summary>
		/// </summary>
		String this[Int32 index]
		{
			get;
		}
	}
}
