/* -----------------------------------------------
 * NuGenList.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Collections
{
	/// <summary>
	/// Represents a <see cref="T:System.Collections.Generic.List`1"/> that bubbles events.
	/// </summary>
	public sealed class NuGenList<T> : NuGenEventInitiator
	{
		#region Declarations.Fields

		private List<T> _listInternal = new List<T>();

		#endregion

		#region Properties.Public

		/*
		 * Count
		 */

		/// <summary>
		/// </summary>
		public int Count
		{
			get
			{
				Debug.Assert(_listInternal != null, "_listInternal != null");
				return _listInternal.Count;
			}
		}

		/*
		 * Indexer
		 */

		/// <summary>
		/// </summary>
		/// <param name="zeroBasedIndex"></param>
		/// <returns></returns>
		public T this[int zeroBasedIndex]
		{
			get
			{
				Debug.Assert(_listInternal != null, "_listInternal != null");
				return _listInternal[zeroBasedIndex];
			}
			set
			{
				Debug.Assert(_listInternal != null, "_listInternal != null");
				_listInternal[zeroBasedIndex] = value;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * Add
		 */

		/// <summary>
		/// </summary>
		/// <param name="item"></param>
		public void Add(T item)
		{
			Debug.Assert(_listInternal != null, "_listInternal != null");
			_listInternal.Add(item);
			this.OnAdded(new NuGenCollectionEventArgs<T>(_listInternal.Count - 1, item));
		}

		private static readonly object _added = new object();

		/// <summary>
		/// Occurs when a new item is added to the list.
		/// </summary>
		public event EventHandler<NuGenCollectionEventArgs<T>> Added
		{
			add
			{
				this.Events.AddHandler(_added, value);
			}
			remove
			{
				this.Events.RemoveHandler(_added, value);
			}
		}

		private void OnAdded(NuGenCollectionEventArgs<T> e)
		{
			this.InvokeActionT<NuGenCollectionEventArgs<T>>(_added, e);
		}

		/*
		 * AddRange
		 */

		/// <summary>
		/// </summary>
		/// <param name="collectionToAdd"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="collectionToAdd"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public void AddRange(IEnumerable<T> collectionToAdd)
		{
			if (collectionToAdd == null)
			{
				throw new ArgumentNullException("collectionToAdd");
			}

			Debug.Assert(_listInternal != null, "_listInternal != null");

			foreach (T item in collectionToAdd)
			{
				this.Add(item);
			}
		}

		/*
		 * Clear
		 */

		/// <summary>
		/// </summary>
		public void Clear()
		{
			Debug.Assert(_listInternal != null, "_listInternal != null");
			_listInternal.Clear();
		}

		/*
		 * GetEnumerator
		 */

		/// <summary>
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///		Index must be within the bounds of the list.
		/// </exception>
		public IEnumerator<T> GetEnumerator()
		{
			Debug.Assert(_listInternal != null, "_listInternal != null");
			return _listInternal.GetEnumerator();
		}

		/*
		 * Insert
		 */

		/// <summary>
		/// </summary>
		/// <param name="zeroBasedIndex"></param>
		/// <param name="item"></param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		Index must be within the bounds of the list.
		/// </exception>
		public void Insert(int zeroBasedIndex, T item)
		{
			try
			{
				Debug.Assert(_listInternal != null, "_listInternal != null");
				_listInternal.Insert(zeroBasedIndex, item);
				this.OnInserted(new NuGenCollectionEventArgs<T>(zeroBasedIndex, item));
			}
			catch (ArgumentOutOfRangeException e)
			{
				throw e;
			}
		}

		private static readonly object _inserted = new object();

		/// <summary>
		/// Occurs when a new item is inserted into the list.
		/// </summary>
		public event EventHandler<NuGenCollectionEventArgs<T>> Inserted
		{
			add
			{
				this.Events.AddHandler(_inserted, value);
			}
			remove
			{
				this.Events.RemoveHandler(_inserted, value);
			}
		}

		private void OnInserted(NuGenCollectionEventArgs<T> e)
		{
			this.InvokeActionT<NuGenCollectionEventArgs<T>>(_inserted, e);
		}

		/*
		 * Remove
		 */

		/// <summary>
		/// </summary>
		/// <param name="item"></param>
		public void Remove(T item)
		{
			Debug.Assert(_listInternal != null, "_listInternal != null");

			int index = _listInternal.IndexOf(item);

			if (_listInternal.Remove(item))
			{
				this.OnRemoved(new NuGenCollectionEventArgs<T>(index, item));
			}
		}

		private static readonly object _removed = new object();

		/// <summary>
		/// Occurs when an item is removed from this list.
		/// </summary>
		public event EventHandler<NuGenCollectionEventArgs<T>> Removed
		{
			add
			{
				this.Events.AddHandler(_removed, value);
			}
			remove
			{
				this.Events.RemoveHandler(_removed, value);
			}
		}

		private void OnRemoved(NuGenCollectionEventArgs<T> e)
		{
			this.InvokeActionT<NuGenCollectionEventArgs<T>>(_removed, e);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Genetibase.Shared.Collections.NuGenList`1"/> class.
		/// </summary>
		public NuGenList()
		{
		}

		#endregion
	}
}
