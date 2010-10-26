/* -----------------------------------------------
 * NuGenThumbnailContainer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenThumbnailContainer
	{
		/// <summary>
		/// </summary>
		public sealed class ImageCollection : IList
		{
			#region ICollection Members

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
				return this.Add((Image)value);
			}

			bool IList.Contains(object value)
			{
				Image image = value as Image;

				if (image != null)
				{
					return this.Contains(image);
				}

				return false;
			}

			void ICollection.CopyTo(Array array, int index)
			{
				this.CopyTo((Image[])array, index);
			}

			int IList.IndexOf(object value)
			{
				return this.IndexOf((Image)value);
			}

			void IList.Insert(int index, object value)
			{
				this.Insert(index, (Image)value);
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
					return true;
				}
			}

			void IList.Remove(object value)
			{
				this.Remove((Image)value);
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

			#region Properties.Public

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
			public Image this[int zeroBasedIndex]
			{
				get
				{
					Debug.Assert(_list != null, "_list != null");
					return _list[zeroBasedIndex];
				}
			}

			internal List<Image> ListInternal
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
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="imageToAdd"/> is <see langword="null"/>.</para>
			/// </exception>
			public int Add(Image imageToAdd)
			{
				if (imageToAdd == null)
				{
					throw new ArgumentNullException("imageToAdd");
				}

				Debug.Assert(_list != null, "_list != null");
				_list.Add(imageToAdd);
				_imageTracker.AddImage(imageToAdd);
				return _list.Count - 1;
			}

			/*
			 * AddRange
			 */

			/// <summary>
			/// </summary>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="imagesToAdd"/> is <see langword="null"/>.</para>
			/// </exception>
			public void AddRange(Image[] imagesToAdd)
			{
				if (imagesToAdd == null)
				{
					throw new ArgumentNullException("imagesToAdd");
				}

				foreach (Image img in imagesToAdd)
				{
					this.Add(img);
				}
			}

			/// <summary>
			/// </summary>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="imagesToAdd"/> is <see langword="null"/>.</para>
			/// </exception>
			public void AddRange(IList<Image> imagesToAdd)
			{
				if (imagesToAdd == null)
				{
					throw new ArgumentNullException("imagesToAdd");
				}

				foreach (Image img in imagesToAdd)
				{
					this.Add(img);
				}
			}

			/*
			 * Clear
			 */

			/// <summary>
			/// </summary>
			public void Clear()
			{
				Debug.Assert(_list != null, "_list != null");
				List<Image> buffer = new List<Image>(_list);

				foreach (Image imgToRemove in buffer)
				{
					this.Remove(imgToRemove);
				}

				buffer.Clear();
			}

			/*
			 * Contains
			 */

			/// <summary>
			/// </summary>
			public bool Contains(Image image)
			{
				Debug.Assert(_list != null, "_list != null");
				return _list.Contains(image);
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
			public void CopyTo(Image[] destination, int zeroBasedIndexToStartAt)
			{
				Debug.Assert(_list != null, "_list != null");
				_list.CopyTo(destination, zeroBasedIndexToStartAt);
			}

			/*
			 * IndexOf
			 */

			/// <summary>
			/// </summary>
			public int IndexOf(Image image)
			{
				Debug.Assert(_list != null, "_list != null");
				return _list.IndexOf(image);
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
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="imageToInsert"/> is <see langword="null"/>.</para>
			/// </exception>
			public void Insert(int index, Image imageToInsert)
			{
				_list.Insert(index, imageToInsert);
				_imageTracker.InsertImage(index, imageToInsert);
			}

			/*
			 * Remove
			 */

			/// <summary>
			/// Removes the first occurence of the specified <see cref="Image"/>.
			/// </summary>
			public bool Remove(Image imageToRemove)
			{
				Debug.Assert(_list != null, "_list != null");

				if (_list.Contains(imageToRemove))
				{
					_imageTracker.RemoveImage(imageToRemove);
					return _list.Remove(imageToRemove);
				}

				return false;
			}

			/*
			 * RemoveAt
			 */

			/// <summary>
			/// </summary>
			public void RemoveAt(int zeroBasedIndex)
			{
				this.Remove(this[zeroBasedIndex]);
			}

			#endregion

			private List<Image> _list;
			private ImageTracker _imageTracker;

			/// <summary>
			/// Initializes a new instance of the <see cref="ImageCollection"/> class.
			/// </summary>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="imageTracker"/> is <see langword="null"/>.</para>
			/// </exception>
			internal ImageCollection(ImageTracker imageTracker)
			{
				if (imageTracker == null)
				{
					throw new ArgumentNullException("imageTracker");
				}

				_list = new List<Image>();
				_imageTracker = imageTracker;
			}
		}
	}
}
