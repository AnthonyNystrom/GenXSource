/* -----------------------------------------------
 * NuGenPropertyInfoCollection.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections;
using System.Diagnostics;

namespace Genetibase.Shared.Serialization
{
	/// <summary>
	/// Contains a collection of <see cref="NuGenPropertyInfo"/> instances.
	/// </summary>
	public class NuGenPropertyInfoCollection : CollectionBase
	{
		#region Declarations.Variables

		private NuGenPropertyInfo _parent = null;

		#endregion

		#region Properties.Public

		/*
		 * Indexer
		 */

		/// <summary>
		/// </summary>
		public NuGenPropertyInfo this[int index]
		{
			get
			{
				return (NuGenPropertyInfo)this.List[index];
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
		/// Gets the items contained in this <see cref="NuGenPropertyInfoCollection"/>.
		/// </summary>
		public NuGenPropertyInfo[] Items
		{
			get
			{
				return (NuGenPropertyInfo[])base.InnerList.ToArray(typeof(NuGenPropertyInfo));
			}
		}

		#endregion

		#region Methods.Public
		
		/*
		 * Add
		 */

		/// <summary>
		/// Adds the specified <see cref="NuGenPropertyInfo"/> to this <see cref="NuGenPropertyInfoCollection"/>.
		/// </summary>
		/// <param name="propertyInfo">Specifies the <see cref="NuGenPropertyInfo"/> to add.</param>
		/// <exception cref="ArgumentNullException"><paramref name="propertyInfo"/> is <see langword="null"/>.</exception>
		public void Add(NuGenPropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentNullException("propertyInfo");
			}

			propertyInfo.Parent = _parent;
			this.List.Add(propertyInfo);
		}

		/*
		 * AddRange
		 */

		/// <summary>
		/// Adds the specified <see cref="NuGenPropertyInfoCollection"/> to this <see cref="NuGenPropertyInfoCollection"/>.
		/// </summary>
		/// <param name="propertyInfoCollection">Specifies the <see cref="NuGenPropertyInfoCollection"/> to add.</param>
		/// <exception cref="ArgumentNullException"><paramref name="propertyInfoCollection"/> is <see langword="null"/>.</exception>
		public void AddRange(NuGenPropertyInfoCollection propertyInfoCollection)
		{
			if (propertyInfoCollection == null)
			{
				throw new ArgumentNullException("propertyInfoCollection");
			}

			foreach (NuGenPropertyInfo propertyInfo in propertyInfoCollection)
			{
				this.Add(propertyInfo);
			}
		}

		/*
		 * AddRange
		 */

		/// <summary>
		/// Adds the specified <see cref="NuGenPropertyInfo"/> array to this <see cref="NuGenPropertyInfoCollection"/>.
		/// </summary>
		/// <param name="propertyInfoCollection">Specifies the <see cref="NuGenPropertyInfo"/> array to add.</param>
		/// <exception cref="ArgumentNullException"><paramref name="propertyInfoCollection"/> is <see langword="null"/>.</exception>
		public void AddRange(NuGenPropertyInfo[] propertyInfoCollection)
		{
			if (propertyInfoCollection == null)
			{
				throw new ArgumentNullException("propertyInfoCollection");
			}

			foreach (NuGenPropertyInfo propertyInfo in propertyInfoCollection)
			{
				this.Add(propertyInfo);
			}
		}

		/*
		 * Contains
		 */

		/// <summary>
		/// Determines whether the specified <see cref="NuGenPropertyInfo"/> is in this <see cref="NuGenPropertyInfoCollection"/>.
		/// </summary>
		/// <param name="propertyInfo">Specifies the <see cref="NuGenPropertyInfo"/> to locate in this
		/// <see cref="NuGenPropertyInfoCollection"/>. The value can be <see langword="null"/>.</param>
		public bool Contains(NuGenPropertyInfo propertyInfo)
		{
			return this.List.Contains(propertyInfo);
		}

		/*
		 * IndexOf
		 */

		/// <summary>
		/// Searches for the specified <see cref="NuGenPropertyInfo"/> and returns the zero-based index of
		/// the first occurrence in this <see cref="NuGenPropertyInfoCollection"/>.
		/// </summary>
		/// <param name="propertyInfo">Specifies the <see cref="NuGenPropertyInfo"/> to locate in this <see cref="NuGenPropertyInfoCollection"/>.</param>
		public int IndexOf(NuGenPropertyInfo propertyInfo)
		{
			return this.List.IndexOf(propertyInfo);
		}

		/*
		 * Insert
		 */

		/// <summary>
		/// Inserts the <see cref="NuGenPropertyInfo"/> at the specified index into this <see cref="NuGenPropertyInfoCollection"/>.
		/// </summary>
		/// <param name="index">Specifes the zero-based index at which the <see cref="T:NuGenPropertyInfo"/> should be inserted.</param>
		/// <param name="propertyInfo">Specifies the <see cref="T:NuGenPropertyInfo"/> to be inserted.</param>
		public void Insert(int index, NuGenPropertyInfo propertyInfo)
		{
			this.List.Insert(index, propertyInfo);
		}

		/*
		 * Remove
		 */

		/// <summary>
		/// Removes the specified <see cref="NuGenPropertyInfo"/> from this <see cref="NuGenPropertyInfoCollection"/>.
		/// </summary>
		/// <param name="propertyInfo">Specifies the <see cref="NuGenPropertyInfo"/> to remove.</param>
		public void Remove(NuGenPropertyInfo propertyInfo)
		{
			this.List.Remove(propertyInfo);
		}

		#endregion

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPropertyInfoCollection"/> class.
		/// </summary>
		public NuGenPropertyInfoCollection()
			: this(null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPropertyInfoCollection"/> class.
		/// </summary>
		/// <param name="parent">Specifies the parent for this <see cref="T:NuGenPropertyInfoCollection"/>.</param>
		public NuGenPropertyInfoCollection(NuGenPropertyInfo parent)
		{
			_parent = parent;
		}
		
		#endregion
	}
}
