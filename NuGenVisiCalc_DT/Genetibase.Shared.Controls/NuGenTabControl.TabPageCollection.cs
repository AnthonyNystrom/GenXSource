/* -----------------------------------------------
 * NuGenTabPageCollection.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
		public sealed class TabPageCollection : IList
		{
			#region ICollection Members

			void ICollection.CopyTo(Array array, int index)
			{
				this.CopyTo((NuGenTabPage[])array, index);
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
				Debug.Assert(_list != null, "_list != null");
				return _list.GetEnumerator();
			}

			#endregion

			#region IList Members

			int IList.Add(object value)
			{
				return this.Add((NuGenTabPage)value);
			}

			bool IList.Contains(object value)
			{
				return this.Contains((NuGenTabPage)value);
			}

			int IList.IndexOf(object value)
			{
				return this.IndexOf((NuGenTabPage)value);
			}

			void IList.Insert(int index, object value)
			{
				this.Insert(index, (NuGenTabPage)value);
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

			void IList.Remove(object value)
			{
				this.Remove((NuGenTabPage)value);
			}

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

			#region Properties

			/// <summary>
			/// Gets the number of elements contained in the collection.
			/// </summary>
			public int Count
			{
				get
				{
					Debug.Assert(_list != null, "_list != null");
					return _list.Count;
				}
			}

			/// <summary>
			/// </summary>
			/// <exception cref="ArgumentOutOfRangeException">
			///		<paramref name="zeroBasedIndex"/> should be within the bounds of the collection.
			/// </exception>
			public NuGenTabPage this[int zeroBasedIndex]
			{
				get
				{
					Debug.Assert(_list != null, "_list != null");
					return _list[zeroBasedIndex];
				}
			}

			internal List<NuGenTabPage> ListInternal
			{
				get
				{
					Debug.Assert(_list != null, "_list != null");
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
			public NuGenTabPage Add(string text)
			{
				return this.Add(text, null);
			}

			/// <summary>
			/// </summary>
			/// <param name="text"></param>
			/// <param name="tabButtonImage">Can be <see langword="null"/>.</param>
			/// <returns></returns>
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
				Debug.Assert(_list != null, "_list != null");

				_list.Add(tabPageToAdd);
				_tabControl.AddTabPage(tabPageToAdd);

				return _list.Count - 1;
			}

			/*
			 * Clear
			 */

			/// <summary>
			/// Removes all items for the collection.
			/// </summary>
			public void Clear()
			{
				Debug.Assert(_list != null, "_list != null");
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
				Debug.Assert(_list != null, "_list != null");
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
				Debug.Assert(_list != null, "_list != null");
				_list.CopyTo(destination, zeroBasedIndexToStartAt);
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
				Debug.Assert(_list != null, "_list != null");
				return _list.IndexOf(tabPage);
			}

			/*
			 * Insert
			 */

			/// <summary>
			/// </summary>
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
			/// <param name="tabButtonImage">Can be <see langword="null"/>.</param>
			/// <exception cref="ArgumentOutOfRangeException">
			/// <para>
			///		<paramref name="index"/> must be within the bounds of the collection.
			/// </para>
			/// </exception>
			public NuGenTabPage Insert(int index, string text, Image tabButtonImage)
			{
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

				Debug.Assert(_list != null, "_list != null");
				Debug.Assert(_tabControl != null, "_tabControl != null");

				_list.Insert(index, tabPageToInsert);
				_tabControl.InsertTabPage(index, tabPageToInsert);
			}

			/*
			 * Remove
			 */

			/// <summary>
			/// </summary>
			public bool Remove(NuGenTabPage tabPageToRemove)
			{
				Debug.Assert(_list != null, "_list != null");

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
				this.Remove(this[zeroBasedIndex]);
			}

			#endregion

			private List<NuGenTabPage> _list;
			private NuGenTabControl _tabControl;

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

				_list = new List<NuGenTabPage>();
				_tabControl = tabControl;
			}
		}
	}
}
