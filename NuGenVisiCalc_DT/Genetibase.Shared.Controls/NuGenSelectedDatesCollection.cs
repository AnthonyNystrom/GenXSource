/* -----------------------------------------------
 * NuGenSelectedDatesCollection.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
	/// Represents a collection of <see cref="NuGenDateItem"/> objects.
	/// </summary>
	public class NuGenSelectedDatesCollection : ReadOnlyCollectionBase 
	{
		#region Class Data

		/// <summary>
		/// The Calendar that owns this DateItemCollection
		/// </summary>
		private NuGenCalendar owner;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSelectedDatesCollection"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="owner"/> is <see langword="null"/>.</para></exception>
		public NuGenSelectedDatesCollection(NuGenCalendar owner) : base()
		{
			if (owner == null)
				throw new ArgumentNullException("owner");
							
			this.owner = owner;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSelectedDatesCollection"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="owner"/> is <see langword="null"/>.</para></exception>	
		public NuGenSelectedDatesCollection(NuGenCalendar owner, NuGenSelectedDatesCollection dates) : this(owner)
		{			
			this.Add(dates);
		}

		#endregion

		#region Methods

		/// <summary>
		/// </summary>
		public void Add(DateTime value)
		{
			int index;
	
			index = this.IndexOf(value);
			if (index == -1)
				this.InnerList.Add(value);
			else
				this.InnerList[index] = value;
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="dates"/> is <see langword="null"/>.</para></exception>
		public void AddRange(DateTime[] dates)
		{
			if (dates == null)
				throw new ArgumentNullException("dates");
			
			for (int i=0; i<dates.Length; i++)
			{				
				this.Add(dates[i]);
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="dates"/> is <see langword="null"/>.</para></exception>
		public void Add(NuGenSelectedDatesCollection dates)
		{
			if (dates == null)
				throw new ArgumentNullException("dates");
			
			for (int i=0; i<dates.Count; i++)
			{
				this.Add(dates[i]);
			}
		}

		/// <summary>
		/// </summary>
		public void Clear()
		{
			while (this.Count > 0)
			{
				this.RemoveAt(0);
			}
		}

		/// <summary>
		/// </summary>
		public bool Contains(DateTime date)
		{
			return (this.IndexOf(date) != -1);
		}

		/// <summary>
		/// </summary>
		public int IndexOf(DateTime date)
		{					
			for (int i=0; i<this.Count; i++)
			{
				if (this[i] == date)
				{
					return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// </summary>
		public void Remove(DateTime value)
		{
			this.InnerList.Remove(value);
		}

		/// <summary>
		/// </summary>
		public void RemoveAt(int index)
		{
			this.Remove(this[index]);
		}

		/// <summary>
		/// </summary>
		public void Move(DateTime value, int index)
		{
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

			this.InnerList.Remove(value);

			if (index > this.Count)
			{
				this.InnerList.Add(value);
			}
			else
			{
				this.InnerList.Insert(index, value);
			}

		}

		/// <summary>
		/// </summary>
		public void MoveToTop(DateTime value)
		{
			this.Move(value, 0);
		}

		/// <summary>
		/// </summary>
		public void MoveToBottom(DateTime value)
		{
			this.Move(value, this.Count);
		}

		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		public virtual DateTime this[int index]
		{
			get
			{
				DateTime d = (DateTime)this.InnerList[index];
				return d;
			}
		}

		#endregion
	}
}
