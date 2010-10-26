/* -----------------------------------------------
 * NuGenGraph.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Diagnostics;

namespace Genetibase.Shared.Serialization
{
	/// <summary>
	/// Represents a graph of objects being serialized.
	/// </summary>
	public class NuGenGraph
	{
		#region Declarations.Variables

		private Hashtable _hashCode = new Hashtable();
		private Hashtable _hashObj = new Hashtable();

		#endregion

		#region Properties.Public

		/*
		 * Indexer
		 */

		/// <summary>
		/// Gets or sets the code by the associated object.
		/// </summary>
		public int this[object obj]
		{
			get
			{
				Debug.Assert(_hashObj != null, "_hashObj != null");
				object index = _hashObj[obj];

				if (index == null)
				{
					return -1;
				}

				return (int)index;
			}
			set
			{
				Debug.Assert(_hashCode != null, "_hashCode != null");
				_hashCode[obj] = value;
			}
		}

		/*
		 * Indexer
		 */

		/// <summary>
		/// Gets or sets the object by the associated code.
		/// </summary>
		public object this[int index]
		{
			get
			{
				Debug.Assert(_hashCode != null, "_hashCode != null");
				return _hashCode[index];
			}
			set
			{
				Debug.Assert(_hashCode != null, "_hashCode != null");
				_hashCode[index] = value;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * Add
		 */

		/// <summary>
		/// Adds an element to this <see cref="NuGenGraph"/>.
		/// </summary>
		/// <param name="obj">Specifies the key of the element to add.</param>
		public void Add(object obj)
		{
			Debug.Assert(_hashCode != null, "_hashCode != null");

			if (_hashCode[obj] == null)
			{
				_hashCode.Add(_hashCode.Count, obj);
				_hashObj.Add(obj, _hashObj.Count);
			}
		}

		/*
		 * Add
		 */

		/// <summary>
		/// Adds an element with the specified key and value to this <see cref="NuGenGraph"/>.
		/// </summary>
		/// <param name="obj">Specifies the key of the element to add.</param>
		/// <param name="index">Specifies the value of the element to add.</param>
		public void Add(object obj, int index)
		{
			if (_hashCode[obj] == null) 
			{
				_hashCode.Add(index, obj);
				_hashObj.Add(obj, index);
			}
		}

		#endregion

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenGraph"/> class.
		/// </summary>
		public NuGenGraph()
		{
		}
		
		#endregion
	}
}
