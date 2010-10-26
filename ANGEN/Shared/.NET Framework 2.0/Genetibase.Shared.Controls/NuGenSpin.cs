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
using System.Globalization;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <seealso cref="NumericUpDown"/>
	/// </summary>
	[ToolboxItem(false)]
	[DefaultEvent("ValueChanged")]
	[DefaultProperty("Value")]
	[NuGenSRDescription("Description_Spin")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSpin : NuGenSpinBase
	{
		#region Properties.Appearance

		/*
		 * Value
		 */

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
				return this.Int32ValueConverter.Value;
			}
			set
			{
				this.Int32ValueConverter.Value = value;
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

		private NuGenPositiveInt32 _incrementInternal;

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
				return this.Int32ValueConverter.Maximum;
			}
			set
			{
				this.Int32ValueConverter.Maximum = value;
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
				return this.Int32ValueConverter.Minimum;
			}
			set
			{
				this.Int32ValueConverter.Minimum = value;
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

		#region Properties.Services

		private INuGenInt32ValueConverter _int32ValueConverter;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenInt32ValueConverter Int32ValueConverter
		{
			get
			{
				if (_int32ValueConverter == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_int32ValueConverter = this.ServiceProvider.GetService<INuGenInt32ValueConverter>();

					if (_int32ValueConverter == null)
					{
						throw new NuGenServiceNotFoundException<INuGenInt32ValueConverter>();
					}
				}

				return _int32ValueConverter;
			}
		}

		#endregion

		#region Methods.Public.Overrieden

		/*
		 * OnEditBoxTextChanged
		 */

		/// <summary>
		/// Parses the value entered into the <see cref="Genetibase.Shared.Controls.NuGenSpinBase.EditBox"/>.
		/// </summary>
		public override void OnEditBoxTextChanged()
		{
			this.Int32ValueConverter.Text = this.EditBox.Text;
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

		#region EventHandlers.StringIntegerService

		private void _int32ValueConverter_MaximumChanged(object sender, EventArgs e)
		{
			this.OnMaximumChanged(e);
		}

		private void _int32ValueConverter_MinimumChanged(object sender, EventArgs e)
		{
			this.OnMinimumChanged(e);
		}

		private void _int32ValueConverter_TextChanged(object sender, EventArgs e)
		{
			this.EditBox.Text = this.Int32ValueConverter.Text;
		}

		private void _int32ValueConverter_ValueChanged(object sender, EventArgs e)
		{
			this.OnValueChanged(e);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSpin"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenButtonStateTracker"/></para>
		/// <para><see cref="INuGenControlStateTracker"/></para>
		/// <para><see cref="INuGenInt32ValueConverter"/></para>
		/// <para><see cref="INuGenSpinRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSpin(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.Int32ValueConverter.MaximumChanged += _int32ValueConverter_MaximumChanged;
			this.Int32ValueConverter.MinimumChanged += _int32ValueConverter_MinimumChanged;
			this.Int32ValueConverter.TextChanged += _int32ValueConverter_TextChanged;
			this.Int32ValueConverter.ValueChanged += _int32ValueConverter_ValueChanged;
			this.Int32ValueConverter.Text = this.Value.ToString(CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_int32ValueConverter != null)
				{
					_int32ValueConverter.MaximumChanged -= _int32ValueConverter_MaximumChanged;
					_int32ValueConverter.MinimumChanged -= _int32ValueConverter_MinimumChanged;
					_int32ValueConverter.TextChanged -= _int32ValueConverter_TextChanged;
					_int32ValueConverter.ValueChanged -= _int32ValueConverter_ValueChanged;
					_int32ValueConverter.Dispose();
					_int32ValueConverter = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
