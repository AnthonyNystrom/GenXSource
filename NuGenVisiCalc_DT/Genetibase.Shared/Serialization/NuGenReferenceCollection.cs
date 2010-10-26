/* -----------------------------------------------
 * NuGenReferenceCollection.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

namespace Genetibase.Shared.Serialization
{
	/// <summary>
	/// Contains a collection of <see cref="T:NuGenReference"/> instances.
	/// </summary>
	public class NuGenReferenceCollection : CollectionBase
	{
		#region Properties.Public

		/*
		 * Indexer
		 */

		/// <summary>
		/// Gets or sets the <see cref="T:NuGenReference"/> located at the specified index in this
		/// <see cref="T:NuGenReferenceCollection"/>.
		/// </summary>
		public NuGenReference this[int index]
		{
			get
			{
				return (NuGenReference)this.List[index];
			}
			set
			{
				this.List[index] = value;
			}
		}

		/*
		 * Items
		 */

		/// <summary>
		/// Gets the items contained in this <see cref="T:NuGenReferenceCollection"/>.
		/// </summary>
		public NuGenReference[] Items
		{
			get
			{
				return (NuGenReference[])this.InnerList.ToArray(typeof(NuGenReference));
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * Add
		 */

		/// <summary>
		/// Adds the specified <see cref="T:NuGenReference"/> to this <see cref="T:NuGenReferenceCollection"/>.
		/// </summary>
		/// <param name="reference">Specifies the <see cref="T:NuGenReference"/> to add.</param>
		public void Add(NuGenReference reference)
		{
			this.List.Add(reference);
		}

		/*
		 * Add
		 */

		/// <summary>
		/// Adds the specified <see cref="T:NuGenReference"/> to this <see cref="T:NuGenReferenceCollection"/>.
		/// </summary>
		/// <param name="propertyInfo">Specifies the <see cref="T:NuGenPropertyInfo"/> that is used to
		/// initialize a <see cref="T:NuGenReference"/> instance to add.</param>
		public void Add(NuGenPropertyInfo propertyInfo)
		{
			this.List.Add(new NuGenReference(propertyInfo));
		}

		/*
		 * Add
		 */

		/// <summary>
		/// Adds the specified <see cref="T:NuGenReference"/> to this <see cref="T:NuGenReferenceCollection"/>.
		/// </summary>
		/// <param name="propertyInfo">Specifies the <see cref="T:NuGenPropertyInfo"/> that is used to
		/// initialize a <see cref="T:NuGenReference"/> instance to add.</param>
		/// <param name="standardPropertyInfo">Specifies the <see cref="T:PropertyInfo"/> that is used to
		/// initialize a <see cref="T:NuGenReference"/> instance to add.</param>
		/// <param name="obj">Specifies the <see cref="T:Object"/> that is used to initialize a <see cref="T:NuGenReference"/> to add.</param>
		public void Add(
			NuGenPropertyInfo propertyInfo,
			PropertyInfo standardPropertyInfo,
			object obj
			)
		{
			this.List.Add(new NuGenReference(propertyInfo, standardPropertyInfo, obj));
		}

		/*
		 * AddRange
		 */

		/// <summary>
		/// Adds the specified <see cref="T:NuGenReferenceCollection"/> to this <see cref="T:NuGenReferenceCollection"/>.
		/// </summary>
		/// <param name="references">Specifies the <see cref="T:NuGenReferenceCollection"/> to add.</param>
		/// <exception cref="T:ArgumentNullException"><paramref name="references"/> is <see langword="null"/>.</exception>
		public void AddRange(NuGenReferenceCollection references)
		{
			if (references == null)
			{
				throw new ArgumentNullException("references");
			}

			this.InnerList.AddRange(references.Items);
		}

		/*
		 * AddRange
		 */

		/// <summary>
		/// Adds the specified <see cref="T:NuGenReference"/> array to this <see cref="T:NuGenReferenceCollection"/>.
		/// </summary>
		/// <param name="references">Specifies the <see cref="T:NuGenReference"/> array to add.</param>
		/// <exception cref="T:ArgumentNullException"><paramref name="references"/> is <see langword="null"/>.</exception>
		public void AddRange(NuGenReference[] references)
		{
			if (references == null)
			{
				throw new ArgumentNullException("references");
			}

			this.InnerList.AddRange(references);
		}

		/*
		 * Contains
		 */

		/// <summary>
		/// Determines whether the specified <see cref="T:NuGenReference"/> is in this <see cref="T:NuGenReferenceCollection"/>.
		/// </summary>
		/// <param name="reference">Specifies the <see cref="T:NuGenReference"/> to locate in this
		/// <see cref="T:NuGenReferenceCollection"/>. The value can be <see langword="null"/>.</param>
		public bool Contains(NuGenReference reference)
		{
			return this.List.Contains(reference);
		}

		/*
		 * IndexOf
		 */

		/// <summary>
		/// Searches for the specified <see cref="T:NuGenReference"/> and returns the zero-based index of
		/// the first occurrence in this <see cref="T:NuGenReferenceCollection"/>.
		/// </summary>
		/// <param name="reference">Specifies the <see cref="T:NuGenReference"/> to locate in this <see cref="T:NuGenReferenceCollection"/>.</param>
		public int IndexOf(NuGenReference reference)
		{
			return this.List.IndexOf(reference);
		}

		/*
		 * Insert
		 */

		/// <summary>
		/// Inserts the <see cref="T:NuGenReference"/> at the specified index into this <see cref="T:NuGenReferenceCollection"/>.
		/// </summary>
		/// <param name="index">Specifes the zero-based index at which the <see cref="T:NuGenReference"/> should be inserted.</param>
		/// <param name="reference">Specifies the <see cref="T:NuGenReference"/> to be inserted.</param>
		public void Insert(int index, NuGenReference reference)
		{
			this.List.Insert(index, reference);
		}

		/*
		 * Remove
		 */

		/// <summary>
		/// Removes the specified <see cref="T:NuGenReference"/> from this <see cref="T:NuGenReferenceCollection"/>.
		/// </summary>
		/// <param name="reference">Specifies the <see cref="T:NuGenReference"/> to remove.</param>
		public void Remove(NuGenReference reference)
		{
			this.List.Remove(reference);
		}

		#endregion
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenReferenceCollection"/> class.
		/// </summary>
		public NuGenReferenceCollection()
		{
		}		
	}
}
