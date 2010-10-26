/* -----------------------------------------------
 * NuGenSwitcher.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls
{
	partial class NuGenSwitcher
	{
		/// <summary>
		/// </summary>
		public sealed class SwitchPageCollection : IList
		{
			#region ICollection Members

			void ICollection.CopyTo(Array array, int index)
			{
				this.CopyTo((NuGenSwitchPage[])array, index);
			}

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
				return this.Add((NuGenSwitchPage)value);
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
				List<NuGenSwitchPage> buffer = new List<NuGenSwitchPage>(_list);

				foreach (NuGenSwitchPage switchPageToRemove in buffer)
				{
					this.Remove(switchPageToRemove);
				}

				buffer.Clear();
			}

			bool IList.Contains(object value)
			{
				return this.Contains((NuGenSwitchPage)value);
			}

			int IList.IndexOf(object value)
			{
				return this.IndexOf((NuGenSwitchPage)value);
			}

			void IList.Insert(int index, object value)
			{
				this.Insert(index, (NuGenSwitchPage)value);
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
				this.Remove((NuGenSwitchPage)value);
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
			public NuGenSwitchPage this[int zeroBasedIndex]
			{
				get
				{
					return _list[zeroBasedIndex];
				}
			}

			#endregion

			#region Properties.Internal

			internal List<NuGenSwitchPage> ListInternal
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
			public NuGenSwitchPage Add(string text)
			{
				return this.Add(text, null);
			}

			/// <summary>
			/// </summary>
			/// <param name="text"></param>
			/// <param name="switchButtonImage">Can be <see langword="null"/>.</param>
			/// <returns></returns>
			public NuGenSwitchPage Add(string text, Image switchButtonImage)
			{
				Debug.Assert(_switcher != null, "_switcher != null");

				NuGenSwitchPage switchPage = new NuGenSwitchPage();
				switchPage.SwitchButtonImage = switchButtonImage;
				this.Add(switchPage);
				switchPage.Text = text;

				return switchPage;
			}

			/// <summary>
			/// </summary>
			/// <param name="switchPageToAdd"></param>
			/// <returns></returns>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="switchPageToAdd"/> is <see langword="null"/>.</para>
			/// </exception>
			public int Add(NuGenSwitchPage switchPageToAdd)
			{
				if (switchPageToAdd == null)
				{
					throw new ArgumentNullException("switchPageToAdd");
				}

				Debug.Assert(_switcher != null, "_switcher != null");
				_list.Add(switchPageToAdd);
				_switcher.AddSwitchPage(switchPageToAdd);

				return _list.Count - 1;
			}

			/*
			 * Contains
			 */

			/// <summary>
			/// </summary>
			public bool Contains(NuGenSwitchPage switchPage)
			{
				return _list.Contains(switchPage);
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
			public void CopyTo(NuGenSwitchPage[] destination, int zeroBasedIndexToStartAt)
			{
				_list.CopyTo(destination, zeroBasedIndexToStartAt);
			}

			/*
			 * GetEnumerator
			 */

			/// <summary>
			/// </summary>
			/// <returns></returns>
			public IEnumerator<NuGenSwitchPage> GetEnumerator()
			{
				Debug.Assert(_list != null, "_list != null");
				return _list.GetEnumerator();
			}

			/*
			 * IndexOf
			 */

			/// <summary>
			/// </summary>
			/// <returns></returns>
			public int IndexOf(NuGenSwitchPage switchPage)
			{
				return _list.IndexOf(switchPage);
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
			public NuGenSwitchPage Insert(int index, string text)
			{
				return this.Insert(index, text, null);
			}

			/// <summary>
			/// </summary>
			/// <param name="index"></param>
			/// <param name="text"></param>
			/// <param name="switchButtonImage">Can be <see langword="null"/>.</param>
			/// <exception cref="ArgumentOutOfRangeException">
			/// <para>
			///		<paramref name="index"/> must be within the bounds of the collection.
			/// </para>
			/// </exception>
			public NuGenSwitchPage Insert(int index, string text, Image switchButtonImage)
			{
				Debug.Assert(_switcher != null, "_switcher != null");

				NuGenSwitchPage switchPage = new NuGenSwitchPage();
				switchPage.SwitchButtonImage = switchButtonImage;
				this.Insert(index, switchPage);
				switchPage.Text = text;

				return switchPage;
			}

			/// <summary>
			/// </summary>
			/// <exception cref="ArgumentNullException">
			/// <para>
			///		<paramref name="switchPageToInsert"/> is <see langword="null"/>.
			/// </para>
			/// </exception>
			/// <exception cref="ArgumentOutOfRangeException">
			/// <para>
			///		<paramref name="index"/> must be within the bounds of the collection.
			/// </para>
			/// </exception>
			public void Insert(int index, NuGenSwitchPage switchPageToInsert)
			{
				if (switchPageToInsert == null)
				{
					throw new ArgumentNullException("switchPageToInsert");
				}

				Debug.Assert(_switcher != null, "_switcher != null");
				_list.Insert(index, switchPageToInsert);
				_switcher.InsertSwitchPage(index, switchPageToInsert);
			}

			/*
			 * Remove
			 */

			/// <summary>
			/// </summary>
			public bool Remove(NuGenSwitchPage switchPageToRemove)
			{
				Debug.Assert(_list != null, "_list != null");

				if (_list.Contains(switchPageToRemove))
				{
					Debug.Assert(_switcher != null, "_switcher != null");
					_switcher.Controls.Remove(switchPageToRemove);
					return _list.Remove(switchPageToRemove);
				}

				return false;
			}

			#endregion

			private List<NuGenSwitchPage> _list;
			private NuGenSwitcher _switcher;

			/// <summary>
			/// Initializes a new instance of the <see cref="SwitchPageCollection"/> class.
			/// </summary>
			/// <exception cref="ArgumentNullException">
			/// <para>
			///		<paramref name="switcher"/> is <see langword="null"/>.
			/// </para>
			/// </exception>
			public SwitchPageCollection(NuGenSwitcher switcher)
			{
				if (switcher == null)
				{
					throw new ArgumentNullException("switcher");
				}

				_list = new List<NuGenSwitchPage>();
				_switcher = switcher;
			}
		}
	}
}
