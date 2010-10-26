/* -----------------------------------------------
 * NuGenValueTracker.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// </summary>
	public sealed class NuGenValueTracker : INuGenValueTracker
	{
		#region INuGenValueTracker Members

		/*
		 * LargeChange
		 */

		private NuGenInt32 _largeChange;

		/// <summary>
		/// </summary>
		private NuGenInt32 LargeChangeInternal
		{
			get
			{
				if (_largeChange == null)
				{
					_largeChange = new NuGenInt32(1, int.MaxValue);
				}

				return _largeChange;
			}
		}

		/// <summary>
		/// </summary>
		public int LargeChange
		{
			get
			{
				return this.LargeChangeInternal.Value;
			}
			set
			{
				this.LargeChangeInternal.Value = value;
			}
		}

		/*
		 * Maximum
		 */

		/// <summary>
		/// </summary>
		public int Maximum
		{
			get
			{
				return this.ValueInternal.Maximum;
			}
			set
			{
				this.ValueInternal.Maximum = value;
			}
		}

		/*
		 * Minimum
		 */

		/// <summary>
		/// </summary>
		public int Minimum
		{
			get
			{
				return this.ValueInternal.Minimum;
			}
			set
			{
				this.ValueInternal.Minimum = value;
			}
		}

		/*
		 * SmallChange
		 */

		private NuGenInt32 _smallChange;

		/// <summary>
		/// </summary>
		private NuGenInt32 SmallChangeInternal
		{
			get
			{
				if (_smallChange == null)
				{
					_smallChange = new NuGenInt32(1, int.MaxValue);
				}

				return _smallChange;
			}
		}

		/// <summary>
		/// </summary>
		public int SmallChange
		{
			get
			{
				return this.SmallChangeInternal.Value;
			}
			set
			{
				this.SmallChangeInternal.Value = value;
			}
		}

		/*
		 * Value
		 */

		private NuGenInt32 _value;

		/// <summary>
		/// </summary>
		private NuGenInt32 ValueInternal
		{
			get
			{
				if (_value == null)
				{
					_value = new NuGenInt32(0, 100);
				}

				return _value;
			}
		}

		/// <summary>
		/// </summary>
		public int Value
		{
			get
			{
				return this.ValueInternal.Value;
			}
			set
			{
				this.ValueInternal.Value = value;
			}
		}

		/*
		 * LargeChangeDown
		 */

		/// <summary>
		/// </summary>
		public void LargeChangeDown()
		{
			this.SetValue(this.Value - this.LargeChange);
		}

		/*
		 * LargeChangeUp
		 */

		/// <summary>
		/// </summary>
		public void LargeChangeUp()
		{
			this.SetValue(this.Value + this.LargeChange);
		}

		/*
		 * SmallChangeDown
		 */

		/// <summary>
		/// </summary>
		public void SmallChangeDown()
		{
			this.SetValue(this.Value - this.SmallChange);
		}

		/*
		 * SmallChangeUp
		 */

		/// <summary>
		/// </summary>
		public void SmallChangeUp()
		{
			this.SetValue(this.Value + this.SmallChange);
		}

		#endregion

		#region Methods.Private

		/*
		 * SetValue
		 */

		private void SetValue(int value)
		{
			this.Value = Math.Max(this.Minimum, Math.Min(this.Maximum, value));
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenValueTracker"/> class.
		/// </summary>
		public NuGenValueTracker()
		{
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (_largeChange != null)
			{
				_largeChange.Dispose();
				_largeChange = null;
			}

			if (_smallChange != null)
			{
				_smallChange.Dispose();
				_smallChange = null;
			}

			if (_value != null)
			{
				_value.Dispose();
				_value = null;
			}
		}
	}
}
