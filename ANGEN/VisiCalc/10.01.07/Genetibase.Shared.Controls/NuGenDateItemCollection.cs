/* -----------------------------------------------
 * NuGenDateItemCollection.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.CalendarInternals;

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a collection of <see cref="NuGenDateItem"/> instances.
	/// </summary>
	public class NuGenDateItemCollection : CollectionBase 
	{
		/// <summary>
		/// </summary>
		public event EventHandler DateItemModified;

		#region Properties

		/// <summary>
		/// </summary>
		public virtual NuGenDateItem this[int index]
		{
			get
			{
				return this.List[index] as NuGenDateItem;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// </summary>
		public void ModifiedEvent()
		{
			if (DateItemModified != null)
				DateItemModified(this, new EventArgs());
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="value"/> is <see langword="null"/>.</para></exception>
		public void Add(NuGenDateItem value)
		{
			int index;
			if (value == null)
				throw new ArgumentNullException("value");

			if ((NuGenCalendar)value.Calendar == null)
				value.Calendar = _owner;

			index = this.IndexOf(value);
			if (index == -1)
				this.List.Add(value);
			else
				this.List[index] = value;
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="dateItems"/> is <see langword="null"/>.</para></exception>
		public void AddRange(NuGenDateItem[] dateItems)
		{
			if (dateItems == null)
				throw new ArgumentNullException("dateItems");

			for (int i = 0; i < dateItems.Length; i++)
			{
				dateItems[i].Calendar = _owner;
				this.Add(dateItems[i]);
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="dateItesm"/> is <see langword="null"/>.</para></exception>
		public void Add(NuGenDateItemCollection dateItems)
		{
			if (dateItems == null)
				throw new ArgumentNullException("dateItems");

			for (int i = 0; i < dateItems.Count; i++)
			{
				this.Add(dateItems[i]);
			}
		}

		/// <summary>
		/// </summary>
		public new void Clear()
		{
			while (this.Count > 0)
			{
				this.RemoveAt(0);
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="dateItem"/> is <see langword="null"/>.</para></exception>
		public bool Contains(NuGenDateItem dateItem)
		{
			if (dateItem == null)
				throw new ArgumentNullException("dateItem");

			return (this.IndexOf(dateItem) != -1);
		}

		/// <summary>
		/// </summary>
		public int IndexOf(DateTime date)
		{
			NuGenDateItem[] d;

			d = DateInfo(date);
			if (d.Length > 0)
				return d[0].Index;
			else
				return -1;
		}

		/// <summary>
		/// </summary>
		public NuGenDateItem[] DateInfo(DateTime dt)
		{
			NuGenDateItem[] ret = new NuGenDateItem[0];
			ret.Initialize();
			for (int i = 0; i < this.Count; i++)
			{
				if (((this[i].Date <= dt) && (this[i].Range >= dt)))
				{
					switch (this[i].Pattern)
					{
						case NuGenRecurrence.None:
						{
							if (this[i].Date.ToShortDateString() == dt.ToShortDateString())
							{
								this[i].Index = i;
								ret = AddInfo(this[i], ret);
							}
							break;
						}

						case NuGenRecurrence.Daily:
						{
							this[i].Index = i;
							ret = AddInfo(this[i], ret);
							break;
						}
						case NuGenRecurrence.Weekly:
						{
							if ((this[i].Date.DayOfWeek == dt.DayOfWeek))
							{
								this[i].Index = i;
								ret = AddInfo(this[i], ret);
							}
							break;
						}
						case NuGenRecurrence.Monthly:
						{
							if ((this[i].Date.Day == dt.Day))
							{
								this[i].Index = i;
								ret = AddInfo(this[i], ret);
							}
							break;
						}
						case NuGenRecurrence.Yearly:
						{
							if (this[i].Date.ToShortDateString().Substring(5) ==
								dt.ToShortDateString().Substring(5))
							{
								this[i].Index = i;
								ret = AddInfo(this[i], ret);
							}
							break;
						}
					}

				}
			}
			return ret;
		}

		/// <summary>
		/// </summary>
		public NuGenDateItem[] AddInfo(NuGenDateItem dt, NuGenDateItem[] old)
		{
			int l = old.Length;
			int i;
			NuGenDateItem[] n = new NuGenDateItem[l + 1];
			n.Initialize();
			for (i = 0; i < l; i++)
			{
				n[i] = old[i];
			}
			n[i] = dt;
			return n;
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="dateItem"/> is <see langword="null"/>.</para></exception>
		public int IndexOf(NuGenDateItem dateItem)
		{
			if (dateItem == null)
				throw new ArgumentNullException("dateItem");

			for (int i = 0; i < this.Count; i++)
			{
				if (this[i] == dateItem)
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="value"/> is <see langword="null"/>.</para></exception>
		public void Remove(NuGenDateItem value)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			this.List.Remove(value);

		}

		/// <summary>
		/// </summary>
		public new void RemoveAt(int index)
		{
			this.Remove(this[index]);
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="value"/> is <see langword="null"/>.</para></exception>
		public void Move(NuGenDateItem value, int index)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			if (index < 0)
			{
				index = 0;
			}
			else if (index > this.Count)
			{
				index = this.Count;
			}

			if (!this.Contains(value) || this.IndexOf(value) == index)
			{
				return;
			}

			this.List.Remove(value);

			if (index > this.Count)
			{
				this.List.Add(value);
			}
			else
			{
				this.List.Insert(index, value);
			}

		}

		/// <summary>
		/// </summary>
		public void MoveToTop(NuGenDateItem value)
		{
			this.Move(value, 0);
		}

		/// <summary>
		/// </summary>
		public void MoveToBottom(NuGenDateItem value)
		{
			this.Move(value, this.Count);
		}

		#endregion

		private NuGenCalendar _owner;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDateItemCollection"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="owner"/> is <see langword="false"/>.</para>
		/// </exception>
		public NuGenDateItemCollection(NuGenCalendar owner) : base()
		{
			if (owner == null)
				throw new ArgumentNullException("owner");
							
			_owner = owner;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDateItemCollection"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="owner"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="dateItems"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenDateItemCollection(NuGenCalendar owner, NuGenDateItemCollection dateItems) : this(owner)
		{
			this.Add(dateItems);
		}
	}
}
