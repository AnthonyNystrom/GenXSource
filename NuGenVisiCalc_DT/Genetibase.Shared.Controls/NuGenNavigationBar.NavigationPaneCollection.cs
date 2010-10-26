/* -----------------------------------------------
 * NuGenNavigationBar.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls
{
	partial class NuGenNavigationBar
	{
		/// <summary>
		/// </summary>
		public class NavigationPaneCollection : IList
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
			/// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"></see> cannot be cast automatically to the type of the destination array. </exception>
			/// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero. </exception>
			/// <exception cref="T:System.ArgumentException">array is multidimensional.-or- index is equal to or greater than the length of array.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"></see> is greater than the available space from index to the end of the destination array. </exception>
			void ICollection.CopyTo(Array array, int index)
			{
				this.CopyTo((NuGenNavigationPane[])array, index);
			}

			/*
			 * Count
			 */

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

			private readonly object _syncRoot = new object();

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

			/// <summary>
			/// Returns an enumerator that iterates through a collection.
			/// </summary>
			/// <returns>
			/// An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
			/// </returns>
			IEnumerator IEnumerable.GetEnumerator()
			{
				Debug.Assert(_list != null, "_list != null");
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
			///		<paramref name="value"/> cannot be cast automatically to <see cref="NuGenNavigationPane"/>.
			/// </para>
			///	</exception>
			int IList.Add(object value)
			{
				return this.Add((NuGenNavigationPane)value);
			}

			/*
			 * Clear
			 */

			/// <summary>
			/// Removes all items from the collection.
			/// </summary>
			public void Clear()
			{
				Debug.Assert(_list != null, "_list != null");
				List<NuGenNavigationPane> buffer = new List<NuGenNavigationPane>(_list);

				foreach (NuGenNavigationPane navigationPane in buffer)
				{
					this.Remove(navigationPane);
				}

				buffer.Clear();
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
			///		<paramref name="value"/> cannot be cast automatically to <see cref="NuGenNavigationPane"/>.
			/// </para>
			/// </exception>
			bool IList.Contains(object value)
			{
				return this.Contains((NuGenNavigationPane)value);
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
				return this.IndexOf((NuGenNavigationPane)value);
			}

			/*
			 * Insert
			 */

			/// <summary>
			/// Inserts an item to the <see cref="T:System.Collections.IList"></see> at the specified index.
			/// </summary>
			/// <param name="index"></param>
			/// <param name="value">The <see cref="T:System.Object"></see> to insert into the <see cref="T:System.Collections.IList"></see>.</param>
			/// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.IList"></see>. </exception>
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
			///		<paramref name="value"/> cannot be cast automatically to <see cref="NuGenNavigationPane"/>.
			/// </para>
			/// </exception>
			void IList.Insert(int index, object value)
			{
				this.Insert(index, (NuGenNavigationPane)value);
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
			///		<paramref name="value"/> cannot be cast automatically to <see cref="NuGenNavigationPane"/>.
			/// </para>
			/// </exception>
			void IList.Remove(object value)
			{
				this.Remove((NuGenNavigationPane)value);
			}

			/*
			 * RemoveAt
			 */

			/// <summary>
			/// Removes the <see cref="T:System.Collections.IList"></see> item at the specified index.
			/// </summary>
			/// <param name="index">The zero-based index of the item to remove.</param>
			/// <exception cref="ArgumentOutOfRangeException">
			///		<paramref name="zeroBasedIndex"/> should be within the bounds of the collection.
			/// </exception>
			public void RemoveAt(int index)
			{
				NuGenNavigationPane navigationPane = this[index];
				this.Remove(navigationPane);
			}

			/*
			 * Indexer
			 */

			/// <summary>
			/// </summary>
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

			#region Properties.Indexer

			/// <summary>
			/// </summary>
			/// <exception cref="ArgumentOutOfRangeException">
			///		<paramref name="zeroBasedIndex"/> should be within the bounds of the collection.
			/// </exception>
			public NuGenNavigationPane this[int zeroBasedIndex]
			{
				get
				{
					NuGenNavigationPane navigationPane = null;

					try
					{
						navigationPane = _list[zeroBasedIndex];
					}
					catch (ArgumentOutOfRangeException e)
					{
						throw e;
					}

					return navigationPane;
				}
			}

			#endregion

			#region Properties.Internal

			/*
			 * ListInternal
			 */

			internal List<NuGenNavigationPane> ListInternal
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
			public NuGenNavigationPane Add(string text)
			{
				return this.Add(text, null);
			}

			/// <summary>
			/// </summary>
			/// <param name="text"></param>
			/// <param name="navigationButtonImage">Can be <see langword="null"/>.</param>
			public NuGenNavigationPane Add(string text, Image navigationButtonImage)
			{
				Debug.Assert(_navigationBar != null, "_navigationBar != null");
				Debug.Assert(_navigationBar.ServiceProvider != null, "_navigationBar.ServiceProvider != null");

				NuGenNavigationPane navigationPane = new NuGenNavigationPane(_navigationBar.ServiceProvider);
				navigationPane.NavigationButtonImage = navigationButtonImage;
				this.Add(navigationPane);
				navigationPane.Text = text;

				return navigationPane;
			}

			/// <summary>
			/// </summary>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="navigationPaneToAdd"/> is <see langword="null"/>.</para>
			/// </exception>
			public int Add(NuGenNavigationPane navigationPaneToAdd)
			{
				if (navigationPaneToAdd == null)
				{
					throw new ArgumentNullException("navigationPaneToAdd");
				}

				Debug.Assert(_navigationBar != null, "_navigationBar != null");
				_list.Add(navigationPaneToAdd);
				_navigationBar.AddNavigationPane(navigationPaneToAdd);

				return _list.Count - 1;
			}

			/*
			 * Contains
			 */

			/// <summary>
			/// </summary>
			public bool Contains(NuGenNavigationPane navigationPane)
			{
				return _list.Contains(navigationPane);
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
			public void CopyTo(NuGenNavigationPane[] destination, int zeroBasedIndexToStartAt)
			{
				_list.CopyTo(destination, zeroBasedIndexToStartAt);
			}

			/*
			 * GetEnumerator
			 */

			/// <summary>
			/// </summary>
			public IEnumerator<NuGenNavigationPane> GetEnumerator()
			{
				Debug.Assert(_list != null, "_list != null");
				return _list.GetEnumerator();
			}

			/*
			 * IndexOf
			 */

			/// <summary>
			/// </summary>
			public int IndexOf(NuGenNavigationPane navigationPane)
			{
				return _list.IndexOf(navigationPane);
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
			public NuGenNavigationPane Insert(int index, string text)
			{
				return this.Insert(index, text, null);
			}

			/// <summary>
			/// </summary>
			/// <param name="index"></param>
			/// <param name="text"></param>
			/// <param name="navigationButtonImage">Can be <see langword="null"/>.</param>
			/// <exception cref="ArgumentOutOfRangeException">
			/// <para>
			///		<paramref name="index"/> must be within the bounds of the collection.
			/// </para>
			/// </exception>
			public NuGenNavigationPane Insert(int index, string text, Image navigationButtonImage)
			{
				Debug.Assert(_navigationBar != null, "_navigationBar != null");
				Debug.Assert(_navigationBar.ServiceProvider != null, "_navigationBar.ServiceProvider != null");

				NuGenNavigationPane navigationPane = new NuGenNavigationPane(_navigationBar.ServiceProvider);
				navigationPane.NavigationButtonImage = navigationButtonImage;
				this.Insert(index, navigationPane);
				navigationPane.Text = text;

				return navigationPane;
			}

			/// <summary>
			/// </summary>
			/// <exception cref="ArgumentOutOfRangeException">
			/// <para>
			///		<paramref name="index"/> must be within the bounds of the collection.
			/// </para>
			/// </exception>
			/// <exception cref="ArgumentNullException">
			/// <para>
			///		<paramref name="navigationPaneToInsert"/> is <see langword="null"/>.
			/// </para>
			/// </exception>
			public void Insert(int index, NuGenNavigationPane navigationPaneToInsert)
			{
				if (navigationPaneToInsert == null)
				{
					throw new ArgumentNullException("navigationPaneToInsert");
				}

				try
				{
					_list.Insert(index, navigationPaneToInsert);
					_navigationBar.InsertNavigationPane(index, navigationPaneToInsert);
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
			public bool Remove(NuGenNavigationPane navigationPaneToRemove)
			{
				if (_list.Contains(navigationPaneToRemove))
				{
					Debug.Assert(_navigationBar != null, "_navigationBar != null");
					_navigationBar.Controls.Remove(navigationPaneToRemove);
					return _list.Remove(navigationPaneToRemove);
				}

				return false;
			}

			#endregion

			private List<NuGenNavigationPane> _list;
			private NuGenNavigationBar _navigationBar;

			/// <summary>
			/// Initializes a new instance of the <see cref="NavigationPaneCollection"/> class.
			/// </summary>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="navigationBar"/> is <see langword="null"/>.</para>
			/// </exception>
			public NavigationPaneCollection(NuGenNavigationBar navigationBar)
			{
				if (navigationBar == null)
				{
					throw new ArgumentNullException("navigationBar");
				}

				_list = new List<NuGenNavigationPane>();
				_navigationBar = navigationBar;
			}
		}
	}
}
