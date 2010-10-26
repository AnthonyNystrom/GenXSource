/* -----------------------------------------------
 * IPropertyOwner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Windows.Controls.Data.Text
{
	/// <summary>
	/// </summary>
	public interface IPropertyOwner
	{
		/// <summary>
		/// </summary>
		IEnumerable<KeyValuePair<Object, Object>> Properties
		{
			get;
		}

		/// <summary>
		/// </summary>
		void AddProperty(Object key, Object property);

		/// <summary>
		/// </summary>
		Boolean RemoveProperty(Object key);

		/// <summary>
		/// </summary>
		Boolean TryGetProperty<TProperty>(Object key, out TProperty property);
	}
}
