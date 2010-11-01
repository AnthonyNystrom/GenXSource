/* -----------------------------------------------
 * NuGenCollectionEventArgs.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.Shared.Collections
{
	/// <summary>
	/// </summary>
	public class NuGenCollectionEventArgs<T> : EventArgs
	{
		#region Properties.Public

		/*
		 * Index
		 */

		private int _index;

		/// <summary>
		/// Read-only.
		/// </summary>
		public int Index
		{
			get
			{
				return _index;
			}
		}

		/*
		 * Item
		 */

		private T _item;

		/// <summary>
		/// Read-only.
		/// </summary>
		public T Item
		{
			get
			{
				return _item;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Genetibase.Shared.Collections.NuGenCollectionEventArgs`1"/> class.
		/// </summary>
		public NuGenCollectionEventArgs(int index, T item)
		{
			_index = index;
			_item = item;
		}

		#endregion
	}
}
