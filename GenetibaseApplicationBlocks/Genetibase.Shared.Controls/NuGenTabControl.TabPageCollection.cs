/* -----------------------------------------------
 * NuGenTabControl.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls
{
	partial class NuGenTabControl
	{
		/// <summary>
		/// </summary>
		public class TabPageCollection : IList
		{
			#region ICollection Members

			/*
			 * CopyTo
			 */

			/// <summary>
			/// Copies the elements of the <see cref="T:System.Collections.ICollection"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
			/// </summary>
			/// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
			/// <param name="index">The zero-based index in array at which copying begins.</param>
			/// <exception cref="T:System.ArgumentNullException">array is null. </exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero. </exception>
			/// <exception cref="T:System.ArgumentException">array is multidimensional.-or- index is equal to or greater than the length of array.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"></see> is greater than the available space from index to the end of the destination array. </exception>
			/// <exception cref="T:System.InvalidCastException">The type of the source <see cref="T:System.Collections.ICollection"></see> cannot be cast automatically to the type of the destination array. </exception>
			void ICollection.CopyTo(Array array, int index)
			{
				this.CopyTo((NuGenTabPage[])array, index);
			}

			/*
			 * Count
			 */

			/// <summary>
			/// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.
			/// </summary>
			/// <value></value>
			/// <returns>The number of elements contained in the <see cref="T:System.Collections.ICollection"></see>.</returns>
			int ICollection.Count
			{
				get
				{
					return this.Count;
				}
			}

			/*
			 * IsSynchronized
			 */

			/// <summary>
			/// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe).
			/// </summary>
			/// <value></value>
			/// <returns>true if access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe); otherwise, false.</returns>
			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			/*
			 * SyncRoot
			 */

			private static readonly object _syncRoot = new object();

			/// <summary>
			/// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.
			/// </summary>
			/// <value></value>
			/// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"></see>.</returns>
			object ICollection.SyncRoot
			{
				get
				{
					return _syncRoot;
				}
			}

			#endregion

			#region IEnumerable Members

			/*
			 * GetEnumerator
			 */

			/// <summary>
			/// </summary>
			/// <returns></returns>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return _list.GetEnumerator();
			}

			#endregion

			#region IList Members

			/*
			 * Add
			 */

			/// <summary>
			/// Adds an item to the <see cref="T:System.Collections.IList"></see>.
			/// </summary>
			/// <param name="value">The <see cref="T:System.Object"></see> to add to the <see cref="T:System.Collections.IList"></see>.</param>
			/// <returns>
			/// The position into which the new element was inserted.
			/// </returns>
			/// <exception cref="ArgumentNullException">
			///		<paramref name="value"/> is <see langword="null"/>.
			/// </exception>
			/// <exception cref="InvalidCastException">
			/// <para>
			///		<paramref name="value"/> cannot be cast automatically to <see cref="NuGenTabPage"/>.
			/// </para>
			///	</exception>
			int IList.Add(object value)
			{
				return this.Add((NuGenTabPage)value);
			}

			/*
			 * Clear
			 */

			/// <summary>
			/// Removes all items from the <see cref="T:System.Collections.IList"></see>.
			/// </summary>
			void IList.Clear()
			{
				this.Clear();
			}

			/*
			 * Contains
			 */

			/// <summary>
			/// Determines whether the <see cref="T:System.Collections.IList"></see> contains a specific value.
			/// </summary>
			/// <param name="value">The <see cref="T:System.Object"></see> to locate in the <see cref="T:System.Collections.IList"></see>.</param>
			/// <returns>
			/// true if the <see cref="T:System.Object"></see> is found in the <see cref="T:System.Collections.IList"></see>; otherwise, false.
			/// </returns>
			/// <exception cref="InvalidCastException">
			/// <para>
			///		<paramref name="value"/> cannot be cast automatically to <see cref="NuGenTabPage"/>.
			/// </para>
			/// </exception>
			bool IList.Contains(object value)
			{
				return this.Contains((NuGenTabPage)value);
			}

			/*
			 * IndexOf
			 */

			/// <summary>
			/// Determines the index of a specific item in the <see cref="T:System.Collections.IList"></see>.
			/// </summary>
			/// <param name="value">The <see cref="T:System.Object"></see> to locate in the <see cref="T:System.Collections.IList"></see>.</param>
			/// <returns>
			/// The index of value if found in the list; otherwise, -1.
			/// </returns>
			/// <exception cref="InvalidCastException">
			/// <para>
			///		<paramref name="value"/> cannot be cast automatically to <see cref="NuGenTabPage"/>.
			/// </para>
			/// </exception>
			int IList.IndexOf(object value)
			{
				return this.IndexOf((NuGenTabPage)value);
			}

			/*
			 * Insert
			 */

			/// <summary>
			/// Inserts an item to the <see cref="T:System.Collections.IList"></see> at the specified index.
			/// </summary>
			/// <param name="index">The zero-based index at which value should be inserted.</param>
			/// <param name="value">The <see cref="T:System.Object"></see> to insert into the <see cref="T:System.Collections.IList"></see>.</param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			///	<para>
			///		<paramref name="index"/> should be within the bounds of this collection.
			/// </para>
			/// </exception>
			/// <exception cref="ArgumentNullException">
			///		<paramref name="value"/> is <see langword="null"/>.
			/// </exception>
			/// <exception cref="InvalidCastException">
			/// <para>
			///		<paramref name="value"/> cannot be cast automatically to <see cref="NuGenTabPage"/>.
			/// </para>
			/// </exception>
			void IList.Insert(int index, object value)
			{
				this.Insert(index, (NuGenTabPage)value);
			}

			/*
			 * IsFixedSize
			 */

			/// <summary>
			/// Gets a value indicating whether the <see cref="T:System.Collections.IList"></see> has a fixed size.
			/// </summary>
			/// <value></value>
			/// <returns>true if the <see cref="T:System.Collections.IList"></see> has a fixed size; otherwise, false.</returns>
			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			/*
			 * IsReadOnly
			 */

			/// <summary>
			/// Gets a value indicating whether the <see cref="T:System.Collections.IList"></see> is read-only.
			/// </summary>
			/// <value></value>
			/// <returns>true if the <see cref="T:System.Collections.IList"></see> is read-only; otherwise, false.</returns>
			bool IList.IsReadOnly
			{
				get
				{
					return false;
				}
			}

			/*
			 * Remove
			 */

			/// <summary>
			/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"></see>.
			/// </summary>
			/// <param name="value">The <see cref="T:System.Object"></see> to remove from the <see cref="T:System.Collections.IList"></see>.</param>
			/// <exception cref="InvalidCastException">
			/// <para>
			///		<paramref name="value"/> cannot be cast automatically to <see cref="NuGenTabPage"/>.
			/// </para>
			/// </exception>
			void IList.Remove(object value)
			{
				this.Remove((NuGenTabPage)value);
			}

			/*
			 * RemoveAt
			 */

			/// <summary>
			/// Removes the <see cref="T:System.Collections.IList"></see> item at the specified index.
			/// </summary>
			/// <param name="index">The zero-based index of the item to remove.</param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.IList"></see>. </exception>
			void IList.RemoveAt(int index)
			{
				this.RemoveAt(index);
			}

			/*
			 * Indexer
			 */

			/// <summary>
			/// Gets or sets the <see cref="Object"/> at the specified index.
			/// </summary>
			/// <value></value>
			/// <exception cref="NotSupportedException">
			///		Set operation is not supported for this indexer.
			/// </exception>
			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					throw new NotSupportedException();
				}
			}

			#endregion

			#region Declarations.Fields

			private List<NuGenTabPage> _list = new List<NuGenTabPage>();
			private NuGenTabControl _tabControl = null;

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
					Debug.Assert(_list != null, "_list != null");
					return _list.Count;
				}
			}

			/*
			 * Indexer
			 */

			/// <summary>
			/// </summary>
			/// <param name="zeroBasedIndex"></param>
			/// <returns></returns>
			/// <exception cref="ArgumentOutOfRangeException">
			///		<paramref name="zeroBasedIndex"/> should be within the bounds of the collection.
			/// </exception>
			public NuGenTabPage this[int zeroBasedIndex]
			{
				get
				{
					NuGenTabPage tabPage = null;

					try
					{
						tabPage = _list[zeroBasedIndex];
					}
					catch (ArgumentOutOfRangeException e)
					{
						throw e;
					}

					return tabPage;
				}
			}

			#endregion

			#region Properties.Protected.Internal

			/*
			 * ListInternal
			 */

			/// <summary>
			/// </summary>
			protected internal List<NuGenTabPage> ListInternal
			{
				get
				{
					return _list;
				}
			}

			#endregion

			#region Methods.Public

			/*
			 * Add
			 */

			/// <summary>
			/// </summary>
			/// <param name="text"></param>
			/// <returns></returns>
			/// <exception cref="ArgumentNullException">
			/// <para>
			///		<paramref name="text"/> is <see langword="null"/>.
			/// </para>
			/// </exception>
			public NuGenTabPage Add(string text)
			{
				if (text == null)
				{
					throw new ArgumentNullException("text");
				}

				return this.Add(text, Properties.Resources.Blank);
			}

			/// <summary>
			/// </summary>
			/// <param name="text"></param>
			/// <param name="tabButtonImage">Can be <see langword="null"/>.</param>
			/// <returns></returns>
			/// <exception cref="ArgumentNullException">
			///	<para>
			///		<paramref name="text"/> is <see langword="null"/>.
			/// </para>
			/// </exception>
			public NuGenTabPage Add(string text, Image tabButtonImage)
			{
				Debug.Assert(_tabControl != null, "_tabControl != null");
				Debug.Assert(_tabControl.ServiceProvider != null, "_tabControl.ServiceProvider != null");

				NuGenTabPage tabPage = new NuGenTabPage(_tabControl.ServiceProvider);
				tabPage.TabButtonImage = tabButtonImage;
				this.Add(tabPage);
				tabPage.Text = text;

				return tabPage;
			}

			/// <summary>
			/// </summary>
			/// <exception cref="ArgumentNullException">
			/// <para>
			///		<paramref name="tabPageToAdd"/> is <see langword="null"/>.
			/// </para>
			/// </exception>
			public int Add(NuGenTabPage tabPageToAdd)
			{
				if (tabPageToAdd == null)
				{
					throw new ArgumentNullException("tabPageToAdd");
				}

				Debug.Assert(_tabControl != null, "_tabControl != null");

				_list.Add(tabPageToAdd);
				_tabControl.AddTabPage(tabPageToAdd);

				return _list.Count - 1;
			}

			/*
			 * Clear
			 */

			/// <summary>
			/// </summary>
			public void Clear()
			{
				List<NuGenTabPage> buffer = new List<NuGenTabPage>(_list);

				foreach (NuGenTabPage tabPageToRemove in buffer)
				{
					this.Remove(tabPageToRemove);
				}

				buffer.Clear();
			}

			/*
			 * Contains
			 */

			/// <summary>
			/// </summary>
			/// <param name="tabPage"></param>
			/// <returns></returns>
			public bool Contains(NuGenTabPage tabPage)
			{
				return _list.Contains(tabPage);
			}

			/*
			 * CopyTo
			 */

			/// <summary>
			/// Copies the entire contents of this collection to a compatible one-dimensional
			/// <see cref="T:System.Array"></see>, starting at the specified index of the target array.
			/// </summary>
			/// <exception cref="T:System.InvalidCastException">
			/// The type of the source element cannot be cast automatically to the type of array.
			/// </exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">
			/// index is less than 0.
			/// </exception>
			/// <exception cref="T:System.ArgumentException">
			/// <para>
			///		<paramref name="destination"/> is multidimensional.
			/// </para>
			/// -or-
			/// <para>
			///		<paramref name="zeroBasedIndexToStartAt"/> is equal to or greater than the length of
			///		the destination array.
			/// </para>
			/// -or-
			/// <para>
			///		The number of elements in the source collection is greater
			///		than the available space from index to the end of the destination array.
			/// </para>
			/// </exception>
			/// <exception cref="T:System.ArgumentNullException">
			/// <paramref name="destination"/> is <see langword="null"/>.
			/// </exception>
			public void CopyTo(NuGenTabPage[] destination, int zeroBasedIndexToStartAt)
			{
				_list.CopyTo(destination, zeroBasedIndexToStartAt);
			}

			/*
			 * GetEnumerator
			 */

			/// <summary>
			/// </summary>
			/// <returns></returns>
			public IEnumerator<NuGenTabPage> GetEnumerator()
			{
				Debug.Assert(_list != null, "_list != null");
				return _list.GetEnumerator();
			}

			/*
			 * IndexOf
			 */

			/// <summary>
			/// </summary>
			/// <param name="tabPage"></param>
			/// <returns></returns>
			public int IndexOf(NuGenTabPage tabPage)
			{
				return _list.IndexOf(tabPage);
			}

			/*
			 * Insert
			 */

			/// <summary>
			/// </summary>
			/// <param name="index"></param>
			/// <param name="text"></param>
			/// <returns></returns>
			/// <exception cref="ArgumentOutOfRangeException">
			/// <para>
			///		<paramref name="index"/> must be within the bounds of the collection.
			/// </para>
			/// </exception>
			public NuGenTabPage Insert(int index, string text)
			{
				return this.Insert(index, text, null);
			}

			/// <summary>
			/// </summary>
			/// <param name="index"></param>
			/// <param name="text"></param>
			/// <param name="tabButtonImage"></param>
			/// <returns></returns>
			/// <exception cref="ArgumentNullException">
			/// <para>
			///		<paramref name="text"/> is <see langword="null"/>.
			/// </para>
			/// </exception>
			/// <exception cref="ArgumentOutOfRangeException">
			/// <para>
			///		<paramref name="index"/> must be within the bounds of the collection.
			/// </para>
			/// </exception>
			public NuGenTabPage Insert(int index, string text, Image tabButtonImage)
			{
				if (text == null)
				{
					throw new ArgumentNullException("text");
				}

				Debug.Assert(_tabControl != null, "_tabControl != null");
				Debug.Assert(_tabControl.ServiceProvider != null, "_tabControl.ServiceProvider != null");

				NuGenTabPage tabPage = new NuGenTabPage(_tabControl.ServiceProvider);
				tabPage.TabButtonImage = tabButtonImage;
				this.Insert(index, tabPage);
				tabPage.Text = text;

				return tabPage;
			}

			/// <summary>
			/// </summary>
			/// <param name="index"></param>
			/// <param name="tabPageToInsert"></param>
			/// <exception cref="ArgumentNullException">
			/// <para>
			///		<paramref name="tabPageToInsert"/> is <see langword="null"/>.
			/// </para>
			/// </exception>
			/// <exception cref="ArgumentOutOfRangeException">
			/// <para>
			///		<paramref name="index"/> must be within the bounds of the collection.
			/// </para>
			/// </exception>
			public void Insert(int index, NuGenTabPage tabPageToInsert)
			{
				if (tabPageToInsert == null)
				{
					throw new ArgumentNullException("tabPageToInsert");
				}

				Debug.Assert(_tabControl != null, "_tabControl != null");

				try
				{
					_list.Insert(index, tabPageToInsert);
					_tabControl.InsertTabPage(index, tabPageToInsert);
				}
				catch (ArgumentOutOfRangeException e)
				{
					throw e;
				}
			}

			/*
			 * Remove
			 */

			/// <summary>
			/// </summary>
			/// <param name="tabPageToRemove"></param>
			/// <returns></returns>
			public bool Remove(NuGenTabPage tabPageToRemove)
			{
				if (_list.Contains(tabPageToRemove))
				{
					Debug.Assert(_tabControl != null, "_tabControl != null");
					_tabControl.Controls.Remove(tabPageToRemove);
					return _list.Remove(tabPageToRemove);
				}

				return false;
			}

			/*
			 * RemoveAt
			 */

			/// <summary>
			/// </summary>
			/// <param name="zeroBasedIndex"></param>
			/// <exception cref="ArgumentOutOfRangeException">
			///		<paramref name="zeroBasedIndex"/> should be within the bounds of the collection.
			/// </exception>
			public void RemoveAt(int zeroBasedIndex)
			{
				NuGenTabPage tabPage = this[zeroBasedIndex];
				this.Remove(tabPage);
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="TabPageCollection"/> class.
			/// </summary>
			/// <exception cref="ArgumentNullException">
			/// <para>
			///		<paramref name="tabControl"/> is <see langword="null"/>.
			/// </para>
			/// </exception>
			public TabPageCollection(NuGenTabControl tabControl)
			{
				if (tabControl == null)
				{
					throw new ArgumentNullException("tabControl");
				}

				_tabControl = tabControl;
			}

			#endregion
		}
	}
}
