/* -----------------------------------------------
 * NuGenMiniBarControlCollection.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public class NuGenMiniBarControlCollection : CollectionBase
	{
		#region variablen
		private NuGenMiniBar _owner;
		private bool _updating;
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMiniBarControlCollection"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="owner"/> is <see langword="null"/>.</para></exception>
		public NuGenMiniBarControlCollection(NuGenMiniBar owner)
		{
			if (owner == null)
				throw new ArgumentNullException();
			_owner = owner;
		}

		/// <summary>
		/// </summary>
		public void Add(NuGenMiniBarControl btn)
		{
			if (btn == null)
				return;
			btn.Owner = _owner;
			this.InnerList.Add(btn);
			this.UpdateOwner();
		}

		/// <summary>
		/// </summary>
		public void AddRange(NuGenMiniBarControl[] btns)
		{
			if (btns == null)
				return;
			for (int i = 0; i < btns.Length; i++)
			{
				if (btns[i] != null)
				{
					btns[i].Owner = _owner;
					this.InnerList.Add(btns[i]);
				}
			}
			this.UpdateOwner();
		}

		/// <summary>
		/// </summary>
		public NuGenMiniBarControl this[int index]
		{
			get
			{
				return (index > -1 && index < this.InnerList.Count) ?
					(NuGenMiniBarControl)this.InnerList[index] : null;
			}
			set
			{
				if (value != null && index > -1 && index < this.InnerList.Count)
				{
					this.InnerList[index] = value;
					this.UpdateOwner();
				}
			}
		}

		/// <summary>
		/// </summary>
		public int IndexOf(object obj)
		{
			if (obj == null)
				return -1;
			return this.InnerList.IndexOf(obj);
		}

		/// <summary>
		/// </summary>
		public void Move(NuGenMiniBarControl ctl, int positions)
		{
			int i = this.InnerList.IndexOf(ctl);
			if (i == -1)
				return;
			this.InnerList.RemoveAt(i);
			i = Math.Max(0, Math.Min(this.InnerList.Count, i + positions));
			this.InnerList.Insert(i, ctl);
			this.UpdateOwner();
		}

		/// <summary>
		/// </summary>
		public void Remove(NuGenMiniBarControl ctl)
		{
			int i = this.IndexOf(ctl);
			if (i == -1)
				return;
			this.InnerList.RemoveAt(i);
			this.OnRemoveComplete(i, ctl);
		}

		/// <summary>
		/// Performs additional custom processes after clearing the contents of the <see cref="T:System.Collections.CollectionBase"></see> instance.
		/// </summary>
		protected override void OnClearComplete()
		{
			base.OnClearComplete();
			this.UpdateOwner();
		}

		/// <summary>
		/// Performs additional custom processes after removing an element from the <see cref="T:System.Collections.CollectionBase"></see> instance.
		/// </summary>
		/// <param name="index">The zero-based index at which value can be found.</param>
		/// <param name="value">The value of the element to remove from index.</param>
		protected override void OnRemoveComplete(int index, object value)
		{
			base.OnRemoveComplete(index, value);
			this.UpdateOwner();
		}

		private void UpdateOwner()
		{
			if (_updating)
				return;
			_owner.MeasureButtons();
			_owner.Refresh();
		}

		/// <summary>
		/// </summary>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool Updating
		{
			get
			{
				return _updating;
			}
			set
			{
				_updating = value;
				this.UpdateOwner();
			}
		}
	}
}
