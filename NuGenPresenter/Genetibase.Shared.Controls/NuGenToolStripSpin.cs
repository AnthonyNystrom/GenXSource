/* -----------------------------------------------
 * NuGenToolStripSpin.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a spin with custom renderer that <see cref="ToolStrip"/> and the inheritors can host.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
	public class NuGenToolStripSpin : NuGenToolStripControlHost
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
				return this.Spin.Value;
			}
			set
			{
				this.Spin.Value = value;
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

		#region Properties.Behavior

		/*
		 * InterceptArrowKeys
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_Spin_InterceptArrowKeys")]
		public bool InterceptArrowKeys
		{
			get
			{
				return this.Spin.InterceptArrowKeys;
			}
			set
			{
				this.Spin.InterceptArrowKeys = value;
			}
		}

		private static readonly object _interceptArrowKeysChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="InterceptArrowKeys"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Spin_InterceptArrowKeysChanged")]
		public event EventHandler InterceptArrowKeysChanged
		{
			add
			{
				this.Events.AddHandler(_interceptArrowKeysChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_interceptArrowKeysChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="InterceptArrowKeysChanged"/> event.
		/// </summary>
		protected virtual void OnInterceptArrowKeysChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_interceptArrowKeysChanged, e);
		}

		#endregion

		#region Properties.Data

		/*
		 * Increment
		 */

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
				return this.Spin.Increment;
			}
			set
			{
				this.Spin.Increment = value;
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
				return this.Spin.Maximum;
			}
			set
			{
				this.Spin.Maximum = value;
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
				return this.Spin.Minimum;
			}
			set
			{
				this.Spin.Minimum = value;
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

		#region Properties.Public.Overridden

		/*
		 * BackColor
		 */

		/// <summary>
		/// </summary>
		[DefaultValue(typeof(Color), "Window")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		#endregion

		#region Properties.Hidden

		/*
		 * AutoSize
		 */

		/// <summary>
		/// Gets or sets a value indicating whether the item is automatically sized.
		/// </summary>
		/// <value></value>
		/// <returns>true if the <see cref="T:System.Windows.Forms.ToolStripItem"></see> is automatically sized; otherwise, false. The default value is true.</returns>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		#endregion

		#region Properties.Protected

		/// <summary>
		/// </summary>
		protected NuGenSpin Spin
		{
			get
			{
				return (NuGenSpin)this.Control;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Subscribes events from the hosted control.
		/// </summary>
		/// <param name="control">The control from which to subscribe events.</param>
		protected override void OnSubscribeControlEvents(Control control)
		{
			base.OnSubscribeControlEvents(control);

			NuGenSpin spin = (NuGenSpin)control;

			spin.IncrementChanged += _spin_IncrementChanged;
			spin.InterceptArrowKeysChanged += _spin_InterceptArrowKeysChanged;
			spin.MaximumChanged += _spin_MaximumChanged;
			spin.MinimumChanged += _spin_MinimumChanged;
			spin.ValueChanged += _spin_ValueChanged;
		}

		/// <summary>
		/// Unsubscribes events from the hosted control.
		/// </summary>
		/// <param name="control">The control from which to unsubscribe events.</param>
		protected override void OnUnsubscribeControlEvents(Control control)
		{
			base.OnUnsubscribeControlEvents(control);

			NuGenSpin spin = (NuGenSpin)control;

			spin.IncrementChanged -= _spin_IncrementChanged;
			spin.InterceptArrowKeysChanged -= _spin_InterceptArrowKeysChanged;
			spin.MaximumChanged -= _spin_MaximumChanged;
			spin.MinimumChanged -= _spin_MinimumChanged;
			spin.ValueChanged -= _spin_ValueChanged;
		}

		#endregion

		#region EventHandlers

		private void _spin_IncrementChanged(object sender, EventArgs e)
		{
			this.OnIncrementChanged(e);
		}

		private void _spin_InterceptArrowKeysChanged(object sender, EventArgs e)
		{
			this.OnInterceptArrowKeysChanged(e);
		}

		private void _spin_MaximumChanged(object sender, EventArgs e)
		{
			this.OnMaximumChanged(e);
		}

		private void _spin_MinimumChanged(object sender, EventArgs e)
		{
			this.OnMinimumChanged(e);
		}

		private void _spin_ValueChanged(object sender, EventArgs e)
		{
			this.OnValueChanged(e);
		}

		#endregion

		private static Control CreateControlInstance(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			NuGenSpin spin = new NuGenSpin(serviceProvider);
			spin.Size = new Size(100, 20);
			spin.MinimumSize = new Size(10, 5);

			return spin;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenToolStripSpin"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenSpinRenderer"/></para>
		/// <para><see cref="INuGenButtonStateTracker"/></para>
		/// <para><see cref="INuGenControlStateTracker"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenToolStripSpin(INuGenServiceProvider serviceProvider)
			: base(CreateControlInstance(serviceProvider))
		{
			this.AutoSize = false;
		}
	}
}
