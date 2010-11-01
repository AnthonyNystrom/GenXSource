/* -----------------------------------------------
 * NuGenNavigationButtonCollection.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public sealed class NuGenNavigationButtonCollection : CollectionBase
	{
		#region Declarations.Fields

		private NuGenNavigationBar _owner;

		#endregion

		#region Properties.Public

		/*
		 * Indexer
		 */

		/// <summary>
		/// </summary>
		public NuGenNavigationButton this[int index]
		{
			get
			{
				return (NuGenNavigationButton)this.List[index];
			}
		}

		/// <summary>
		/// </summary>
		public NuGenNavigationButton this[string text]
		{
			get
			{
				foreach (NuGenNavigationButton button in this.List)
				{
					if (button.Text == text)
					{
						return button;
					}
				}

				return null;
			}
		}

		/// <summary>
		/// Retrieves a <see cref="NuGenNavigationButton"/> at the specified location.
		/// Returns <see langword="null"/> if no buttons found.
		/// </summary>
		/// <param name="mouseLocation">Client coordinates are expected.</param>
		/// <returns></returns>
		public NuGenNavigationButton this[Point mouseLocation]
		{
			get
			{
				foreach (NuGenNavigationButton button in this.List)
				{
					if (button.Bounds.Contains(mouseLocation))
					{
						return button;
					}
				}

				return null;
			}
		}

		/// <summary>
		/// Retrieves a <see cref="NuGenNavigationButton"/> at the specified location.
		/// Returns <see langword="null"/> if no buttons found.
		/// </summary>
		/// <param name="x">Client coordinates are expected.</param>
		/// <param name="y">Client coordinates are expected.</param>
		/// <returns></returns>
		public NuGenNavigationButton this[int x, int y]
		{
			get
			{
				return this[new Point(x, y)];
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
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="item"/> is <see langword="null"/>.</para>
		/// </exception>
		public int Add(NuGenNavigationButton item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}

			return this.List.Add(item);
		}

		/*
		 * AddRange
		 */

		/// <summary>
		/// </summary>
		/// <param name="items"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="items"/> is <see langword="null"/>.</para>
		/// </exception>
		public void AddRange(NuGenNavigationButtonCollection items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}

			foreach (NuGenNavigationButton button in items)
			{
				this.Add(button);
			}
		}

		/*
		 * Contains
		 */

		/// <summary>
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(NuGenNavigationButton item)
		{
			return this.List.Contains(item);
		}

		/*
		 * GetVisibleCount
		 */

		/// <summary>
		/// Returns the amount of buttons that are visible and allowed.
		/// </summary>
		/// <returns></returns>
		public int GetVisibleCount()
		{
			int count = 0;

			foreach (NuGenNavigationButton button in this.List)
			{
				if (button.Visible && button.Allowed)
				{
					count++;
				}
			}

			return count;
		}

		/*
		 * IndexOf
		 */

		/// <summary>
		/// Returns -1 if the specified <paramref name="item"/> was not found within this collection.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(NuGenNavigationButton item)
		{
			return this.List.IndexOf(item);
		}

		/*
		 * Insert
		 */

		/// <summary>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="item"/> is <see langword="null"/>.</para>
		/// </exception>
		public void Insert(int index, NuGenNavigationButton item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}

			this.List.Insert(index, item);
		}

		/*
		 * Remove
		 */

		/// <summary>
		/// </summary>
		/// <param name="item"></param>
		public void Remove(NuGenNavigationButton item)
		{
			this.List.Remove(item);
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnValidate
		 */

		/// <summary>
		/// Performs additional custom processes when validating a value.
		/// </summary>
		/// <param name="value">The object to validate.</param>
		/// <exception cref="T:System.ArgumentNullException">value is null.</exception>
		protected override void OnValidate(object value)
		{
			if (!typeof(NuGenNavigationButton).IsAssignableFrom(value.GetType()))
			{
				throw new ArgumentException();
			}

			base.OnValidate(value);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenNavigationButtonCollection"/> class.
		/// </summary>
		/// <param name="owner"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="owner"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenNavigationButtonCollection(NuGenNavigationBar owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}

			_owner = owner;
		}

		#endregion
	}
}
