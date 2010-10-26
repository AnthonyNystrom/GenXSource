/* -----------------------------------------------
 * NuGenFormulaCollection.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.MathX
{
	/// <summary>
	/// </summary>
	public class NuGenFormulaCollection : NuGenFormulaCollectionBase
	{
		/// <summary>
		/// </summary>
		/// <value></value>
		/// <exception cref="ArgumentOutOfRangeException"/>
		public override NuGenFormula this[int index]
		{
			get
			{
				return base[index];
			}
			set
			{
				base[index] = value;
				_owner.Reload();
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="item"/> is <see langword="null"/>.</para>
		/// </exception>
		public override void Add(NuGenFormula val)
		{
			base.Add(val);
			_owner.Reload();
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="items"/> is <see langword="null"/>.</para>
		/// </exception>
		public override void AddRange(NuGenFormula[] vals)
		{
			base.AddRange(vals);
			_owner.Reload();
		}

		/// <summary>
		/// </summary>
		/// <param name="fml"></param>
		public override void Remove(NuGenFormula fml)
		{
			base.Remove(fml);
			_owner.Reload();
		}

		/// <summary>
		/// Performs additional custom processes after inserting a new element into the <see cref="T:System.Collections.CollectionBase"></see> instance.
		/// </summary>
		/// <param name="index">The zero-based index at which to insert value.</param>
		/// <param name="value">The new value of the element at index.</param>
		protected override void OnInsertComplete(int index, object value)
		{
			base.OnInsertComplete(index, value);
			_owner.Reload();
		}

		/// <summary>
		/// Performs additional custom processes after clearing the contents of the <see cref="T:System.Collections.CollectionBase"></see> instance.
		/// </summary>
		protected override void OnClearComplete()
		{
			base.OnClearComplete();
			_owner.Reload();
		}

		/// <summary>
		/// Performs additional custom processes after setting a value in the <see cref="T:System.Collections.CollectionBase"></see> instance.
		/// </summary>
		/// <param name="index">The zero-based index at which oldValue can be found.</param>
		/// <param name="oldValue">The value to replace with newValue.</param>
		/// <param name="newValue">The new value of the element at index.</param>
		protected override void OnSetComplete(int index, object oldValue, object newValue)
		{
			base.OnSetComplete(index, oldValue, newValue);
			_owner.Reload();
		}

		/// <summary>
		/// Performs additional custom processes after removing an element from the <see cref="T:System.Collections.CollectionBase"></see> instance.
		/// </summary>
		/// <param name="index">The zero-based index at which value can be found.</param>
		/// <param name="value">The value of the element to remove from index.</param>
		protected override void OnRemoveComplete(int index, object value)
		{
			base.OnRemoveComplete(index, value);
			_owner.Reload();
		}

		private NuGenVisiPlot2D _owner;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFormulaCollection"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="owner"/> is <see langword="null"/>.</para></exception>
		public NuGenFormulaCollection(NuGenVisiPlot2D owner)
		{
			if (owner == null)
				throw new ArgumentNullException();

			_owner = owner;
		}
	}
}
