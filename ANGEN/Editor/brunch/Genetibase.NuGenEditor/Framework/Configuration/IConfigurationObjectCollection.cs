/* -----------------------------------------------
 * IConfigurationObjectCollection.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Windows.Controls.Framework.Configuration
{
	/// <summary>
	/// </summary>
	public interface IConfigurationObjectCollection : ICollection, IEnumerable
	{
		/// <summary>
		/// </summary>
		void Add(IConfigurationObject value);
		/// <summary>
		/// </summary>
		void Clear();
		/// <summary>
		/// </summary>
		void Insert(Int32 index, IConfigurationObject value);
		/// <summary>
		/// </summary>
		void Remove(IConfigurationObject value);
		/// <summary>
		/// </summary>
		void RemoveAt(Int32 index);

		/// <summary>
		/// </summary>
		IConfigurationObject this[Int32 index]
		{
			get;
			set;
		}
	}
}
