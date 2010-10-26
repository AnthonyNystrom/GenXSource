/* -----------------------------------------------
 * NuGenPinpointWindow.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.Shared.Controls
{
	partial class NuGenPinpointWindow
	{
		/// <summary>
		/// </summary>
		[ListBindable(false)]
		public class ObjectCollection : IList
		{
			#region ICollection Members

			/// <summary>
			/// Copies the elements of the <see cref="T:System.Collections.ICollection"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
			/// </summary>
			/// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in array at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">array is null. </exception>
			/// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"></see> cannot be cast automatically to the type of the destination array. </exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero. </exception>
			/// <exception cref="T:System.ArgumentException">array is multidimensional.-or- index is equal to or greater than the length of array.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"></see> is greater than the available space from index to the end of the destination array. </exception>
			public void CopyTo(Array array, int index)
			{
				_list.CopyTo(array, index);
			}

			/// <summary>
			/// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
			/// </summary>
			/// <value></value>
			/// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
			public int Count
			{
				get
				{
					return _list.Count;
				}
			}

			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			private readonly object _syncRoot = new object();

			object ICollection.SyncRoot
			{
				get
				{
					return _syncRoot;
				}
			}

			#endregion

			#region IEnumerable Members

			IEnumerator IEnumerable.GetEnumerator()
			{
				return _list.GetEnumerator();
			}

			#endregion

			#region IList Members

			/// <summary>
			/// Adds an item to the <see cref="T:System.Collections.IList"></see>.
			/// </summary>
			/// <param name="value">The <see cref="T:System.Object"></see> to add to the <see cref="T:System.Collections.IList"></see>.</param>
			/// <returns>
			/// The position into which the new element was inserted.
			/// </returns>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="value"/> is <see langword="null"/>.</para>
			/// </exception>
			public int Add(object value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				_list.Add(value);
				_owner.Refresh();
				return _list.Count - 1;
			}

			/// <summary>
			/// Removes all items from the <see cref="T:System.Collections.IList"></see>.
			/// </summary>
			/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only. </exception>
			public void Clear()
			{
				_list.Clear();
				_owner.Refresh();
			}

			/// <summary>
			/// Determines whether the <see cref="T:System.Collections.IList"></see> contains a specific value.
			/// </summary>
			/// <param name="value">The <see cref="T:System.Object"></see> to locate in the <see cref="T:System.Collections.IList"></see>.</param>
			/// <returns>
			/// true if the <see cref="T:System.Object"></see> is found in the <see cref="T:System.Collections.IList"></see>; otherwise, false.
			/// </returns>
			public bool Contains(object value)
			{
				return _list.Contains(value);
			}

			/// <summary>
			/// Determines the index of a specific item in the <see cref="T:System.Collections.IList"></see>.
			/// </summary>
			/// <param name="value">The <see cref="T:System.Object"></see> to locate in the <see cref="T:System.Collections.IList"></see>.</param>
			/// <returns>
			/// The index of value if found in the list; otherwise, -1.
			/// </returns>
			public int IndexOf(object value)
			{
				return _list.IndexOf(value);
			}

			/// <summary>
			/// Inserts an item to the <see cref="T:System.Collections.IList"></see> at the specified index.
			/// </summary>
			/// <param name="index"></param>
			/// <param name="value">The <see cref="T:System.Object"></see> to insert into the <see cref="T:System.Collections.IList"></see>.</param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.IList"></see>. </exception>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="value"/> is <see langword="null"/>.</para>
			/// </exception>
			public void Insert(int index, object value)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				_list.Insert(index, value);
				_owner.Refresh();
			}

			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			bool IList.IsReadOnly
			{
				get
				{
					return false;
				}
			}

			/// <summary>
			/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"></see>.
			/// </summary>
			/// <param name="value">The <see cref="T:System.Object"></see> to remove from the <see cref="T:System.Collections.IList"></see>.</param>
			public void Remove(object value)
			{
				_list.Remove(value);
				_owner.Refresh();
			}

			/// <summary>
			/// Removes the <see cref="T:System.Collections.IList"></see> item at the specified index.
			/// </summary>
			/// <param name="index">The zero-based index of the item to remove.</param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.IList"></see>. </exception>
			public void RemoveAt(int index)
			{
				_list.RemoveAt(index);
				_owner.Refresh();
			}

			/// <summary>
			/// Gets or sets the <see cref="Object"/> at the specified index.
			/// </summary>
			/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.IList"></see>. </exception>
			public object this[int index]
			{
				get
				{
					return _list[index];
				}
				set
				{
					_list[index] = value;
					_owner.Refresh();
				}
			}

			#endregion

			/// <summary>
			/// Adds the contents of the specified collection to the end of the <see cref="ObjectCollection"/>.
			/// </summary>
			/// <param name="list">Adds only those items from the collecton that are not <see langword="null"/>.</param>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="list"/> is <see langword="null"/>.</para>
			/// </exception>
			public void AddRange(IList list)
			{
				if (list == null)
				{
					throw new ArgumentNullException("list");
				}

				foreach (object obj in list)
				{
					if (obj != null)
					{
						_list.Add(obj);
					}
				}

				_owner.Refresh();
			}

			/// <summary>
			/// Adds the specified array of objects to the end of the <see cref="ObjectCollection"/>.
			/// </summary>
			/// <param name="items">Adds only those items that are not <see langword="null"/>.</param>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="items"/> is <see langword="null"/>.</para>
			/// </exception>
			public void AddRange(object[] items)
			{
				if (items == null)
				{
					throw new ArgumentNullException("items");
				}

				foreach (object obj in items)
				{
					if (obj != null)
					{
						_list.Add(obj);
					}
				}

				_owner.Refresh();
			}

			private ArrayList _list;
			private NuGenPinpointWindow _owner;

			/// <summary>
			/// Initializes a new instance of the <see cref="ObjectCollection"/> class.
			/// </summary>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="owner"/> is <see langword="null"/>.</para>
			/// </exception>
			public ObjectCollection(NuGenPinpointWindow owner)
			{
				if (owner == null)
				{
					throw new ArgumentNullException("owner");
				}

				_owner = owner;
				_list = new ArrayList();
			}
		}
	}
}
