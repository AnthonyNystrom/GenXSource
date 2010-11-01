/* -----------------------------------------------
 * NuGenSpin.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <seealso cref="NumericUpDown"/>
	/// </summary>
	[DefaultEvent("ValueChanged")]
	[DefaultProperty("Value")]
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSpin : NuGenSpinBase
	{
		#region Properties.Appearance

		/*
		 * Value
		 */

		private int _previousValue = 0;

		private NuGenInt32 _valueInternal = null;

		/// <summary>
		/// </summary>
		protected NuGenInt32 ValueInternal
		{
			get
			{
				if (_valueInternal == null)
				{
					_valueInternal = new NuGenInt32(this.Minimum, this.Maximum);
					_valueInternal.Value = _previousValue;
				}

				return _valueInternal;
			}
		}

		/// <summary>
		/// Gets or sets the current <see cref="NuGenSpin"/> value.
		/// </summary>
		/// <exception cref="ArgumentException">
		///	<para>
		///		<paramref name="value"/> should be between <see cref="Minimum"/> and <see cref="Maximum"/>.
		/// </para>
		/// </exception>
		[Browsable(true)]
		[DefaultValue(0)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Spin_Value")]
		public int Value
		{
			get
			{
				int value = 0;

				if (int.TryParse(this.EditBox.Text, out value))
				{
					value = Math.Min(Math.Max(value, this.Minimum), this.Maximum);
					_previousValue = value;
				}
				else
				{
					value = _previousValue;
				}

				return value;
			}
			set
			{
				if (this.ValueInternal.Value != value)
				{
					this.ValueInternal.Value = value;
					this.OnValueChanged(EventArgs.Empty);

					_previousValue = value;
				}

				this.EditBox.Text = value.ToString();
			}
		}

		private static readonly object _valueChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Value"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Spin_ValueChanged")]
		public event EventHandler ValueChanged
		{
			add
			{
				this.Events.AddHandler(_valueChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_valueChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ValueChanged"/> event.
		/// </summary>
		protected virtual void OnValueChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_valueChanged, e);
		}

		#endregion

		#region Properties.Data

		/*
		 * Increment
		 */

		private NuGenPositiveInt32 _incrementInternal = null;

		/// <summary>
		/// </summary>
		protected NuGenPositiveInt32 IncrementInternal
		{
			get
			{
				if (_incrementInternal == null)
				{
					_incrementInternal = new NuGenPositiveInt32();
					_incrementInternal.Value = 1;
				}

				return _incrementInternal;
			}
		}

		/// <summary>
		/// Gets or sets the amount to icrement or decrement on each button click.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <para>
		///		Only non-negative values are accepted.
		/// </para>
		/// </exception>
		[Browsable(true)]
		[DefaultValue(1)]
		[NuGenSRCategory("Category_Data")]
		[NuGenSRDescription("Description_Spin_Increment")]
		public int Increment
		{
			get
			{
				Debug.Assert(this.IncrementInternal != null, "this.IncrementInternal != null");
				return this.IncrementInternal.Value;
			}
			set
			{
				Debug.Assert(this.IncrementInternal != null, "this.IncrementInternal != null");

				if (this.IncrementInternal.Value != value)
				{
					this.IncrementInternal.Value = value;
					this.OnIncrementChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _incrementChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Increment"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Spin_IncrementChanged")]
		public event EventHandler IncrementChanged
		{
			add
			{
				this.Events.AddHandler(_incrementChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_incrementChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="IncrementChanged"/> event.
		/// </summary>
		protected virtual void OnIncrementChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_incrementChanged, e);
		}

		/*
		 * Maximum
		 */

		private int _maximum = 100;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(100)]
		[NuGenSRCategory("Category_Data")]
		[NuGenSRDescription("Description_Spin_Maximum")]
		public int Maximum
		{
			get
			{
				return _maximum;
			}
			set
			{
				if (_maximum != value)
				{
					_maximum = value;

					Debug.Assert(this.ValueInternal != null, "this.ValueInternal != null");
					this.ValueInternal.Maximum = _maximum;

					this.OnMaximumChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _maximumChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Maximum"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Spin_MaximumChanged")]
		public event EventHandler MaximumChanged
		{
			add
			{
				this.Events.AddHandler(_maximumChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_maximumChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="MaximumChanged"/> event.
		/// </summary>
		protected virtual void OnMaximumChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_maximumChanged, e);
		}

		/*
		 * Minimum
		 */

		private int _minimum = 0;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(0)]
		[NuGenSRCategory("Category_Data")]
		[NuGenSRDescription("Description_Spin_Minimum")]
		public int Minimum
		{
			get
			{
				return _minimum;
			}
			set
			{
				if (_minimum != value)
				{
					_minimum = value;

					Debug.Assert(this.ValueInternal != null, "this.ValueInternal != null");
					this.ValueInternal.Minimum = _minimum;

					this.OnMinimumChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _minimumChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Minimum"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Spin_MinimumChanged")]
		public event EventHandler MinimumChanged
		{
			add
			{
				this.Events.AddHandler(_minimumChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_minimumChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="MinimumChanged"/> event.
		/// </summary>
		protected virtual void OnMinimumChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_minimumChanged, e);
		}

		#endregion

		#region Methids.Public.Overrieden

		/*
		 * OnEditBoxTextChanged
		 */

		/// <summary>
		/// Parses the value entered into the <see cref="Genetibase.Shared.Controls.NuGenSpinBase.EditBox"/>.
		/// </summary>
		public override void OnEditBoxTextChanged()
		{
			int value = 0;

			if (int.TryParse(this.EditBox.Text, out value) && value != this.ValueInternal.Value)
			{
				this.OnValueChanged(EventArgs.Empty);
			}
		}

		/*
		 * OnDownButtonClick
		 */

		/// <summary>
		/// Decrements the value of the spin box.
		/// </summary>
		public override void OnDownButtonClick()
		{
			int newValue = this.Value - this.Increment;
			newValue = Math.Max(newValue, this.Minimum);

			this.Value = newValue;
		}

		/*
		 * OnUpButtonClick
		 */

		/// <summary>
		/// Increments the value of the spin box.
		/// </summary>
		public override void OnUpButtonClick()
		{
			int newValue = this.Value + this.Increment;
			newValue = Math.Min(newValue, this.Maximum);

			this.Value = newValue;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSpin"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenSpinRenderer"/><para/>
		/// <see cref="INuGenButtonStateTracker"/><para/>
		/// <see cref="INuGenControlStateTracker"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSpin(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.EditBox.Text = this.Value.ToString();
		}

		#endregion
	}
}
