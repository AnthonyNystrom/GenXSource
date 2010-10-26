/* -----------------------------------------------
 * NuGenToolStripProgressBar.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.ProgressBarInternals;
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
	/// Represents a progress bar with custom renderer that <see cref="ToolStrip"/> and the inheritors can host.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
	public class NuGenToolStripProgressBar : NuGenToolStripControlHost
	{
		#region Properties.Behavior

		/*
		 * MarqueeAnimationSpeed
		 */

		/// <summary>
		/// Gets or sets the speed of the marquee animation in milliseconds.
		/// </summary>
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ProgressBar_MarqueeAnimationSpeed")]
		public int MarqueeAnimationSpeed
		{
			get
			{
				return this.ProgressBar.MarqueeAnimationSpeed;
			}
			set
			{
				this.ProgressBar.MarqueeAnimationSpeed = value;
			}
		}

		private static readonly object _marqueeAnimationSpeedChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="MarqueeAnimationSpeed"/> property changes.
		/// </summary>
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressBar_MarqueeAnimationSpeedChanged")]
		public event EventHandler MarqueeAnimationSpeedChanged
		{
			add
			{
				this.Events.AddHandler(_marqueeAnimationSpeedChanged, value);	
			}
			remove
			{
				this.Events.RemoveHandler(_marqueeAnimationSpeedChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="MarqueeAnimationSpeedChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMarqueeAnimationSpeedChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_marqueeAnimationSpeedChanged, e);
		}

		/*
		 * Maximum
		 */

		/// <summary>
		/// Gets or sets the maximum value this progress bar accepts.
		/// </summary>
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ProgressBar_Maximum")]
		public int Maximum
		{
			get
			{
				return this.ProgressBar.Maximum;
			}
			set
			{
				this.ProgressBar.Maximum = value;
			}
		}

		private static readonly object _maximumChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Maximum"/> property changes.
		/// </summary>
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressBar_MaximumChanged")]
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
		/// <param name="e"></param>
		protected virtual void OnMaximumChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_maximumChanged, e);
		}

		/*
		 * Minimum
		 */

		/// <summary>
		/// Gets or sets the minimum value this progress bar accepts.
		/// </summary>
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ProgressBar_Minimum")]
		public int Minimum
		{
			get
			{
				return this.ProgressBar.Minimum;
			}
			set
			{
				this.ProgressBar.Minimum = value;
			}
		}

		private static readonly object _minimumChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Minimum"/> property changes.
		/// </summary>
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressBar_MinimumChanged")]
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
		/// <param name="e"></param>
		protected virtual void OnMinimumChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_minimumChanged, e);
		}

		/*
		 * Step
		 */

		/// <summary>
		/// </summary>
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ProgressBar_Step")]
		public int Step
		{
			get
			{
				return this.ProgressBar.Step;
			}
			set
			{
				this.ProgressBar.Step = value;
			}
		}

		private static readonly object _stepChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Step"/> property changes.
		/// </summary>
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressBar_StepChanged")]
		public event EventHandler StepChanged
		{
			add
			{
				this.Events.AddHandler(_stepChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_stepChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="StepChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnStepChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_stepChanged, e);
		}

		/*
		 * Style
		 */

		/// <summary>
		/// </summary>
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ProgressBar_Style")]
		public NuGenProgressBarStyle Style
		{
			get
			{
				return this.ProgressBar.Style;
			}
			set
			{
				this.ProgressBar.Style = value;
			}
		}

		private static readonly object _styleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Style"/> property changes.
		/// </summary>
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressBar_StyleChanged")]
		public event EventHandler StyleChanged
		{
			add
			{
				this.Events.AddHandler(_styleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_styleChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="StyleChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnStyleChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_styleChanged, e);
		}

		/*
		 * Value
		 */

		/// <summary>
		/// Gets or sets the current value for the progress bar.
		/// </summary>
		/// <exception cref="ArgumentException"/>
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ProgressBar_Value")]
		public int Value
		{
			get
			{
				return this.ProgressBar.Value;
			}
			set
			{
				this.ProgressBar.Value = value;
			}
		}

		private static readonly object _valueChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Value"/> property changes.
		/// </summary>
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressBar_ValueChanged")]
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
		/// <param name="e"></param>
		protected virtual void OnValueChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_valueChanged, e);
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

		/*
		 * ProgressBar
		 */

		/// <summary>
		/// Gets the associated <see cref="NuGenProgressBar"/>.
		/// </summary>
		protected NuGenProgressBar ProgressBar
		{
			get
			{
				return (NuGenProgressBar)this.Control;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnSubscribeControlEvents
		 */

		/// <summary>
		/// Subscribes events from the hosted control.
		/// </summary>
		/// <param name="control">The control from which to subscribe events.</param>
		protected override void OnSubscribeControlEvents(Control control)
		{
			base.OnSubscribeControlEvents(control);

			NuGenProgressBar progressBar = (NuGenProgressBar)control;

			progressBar.MarqueeAnimationSpeedChanged += progressBar_MarqueeAnimationSpeedChanged;
			progressBar.MaximumChanged += progressBar_MaximumChanged;
			progressBar.MinimumChanged += progressBar_MinimumChanged;
			progressBar.StepChanged += progressBar_StepChanged;
			progressBar.StyleChanged += progressBar_StyleChanged;
			progressBar.ValueChanged += progressBar_ValueChanged;
		}

		/*
		 * OnUnsubscribeControlEvents
		 */

		/// <summary>
		/// Unsubscribes events from the hosted control.
		/// </summary>
		/// <param name="control">The control from which to unsubscribe events.</param>
		protected override void OnUnsubscribeControlEvents(Control control)
		{
			base.OnUnsubscribeControlEvents(control);

			NuGenProgressBar progressBar = (NuGenProgressBar)control;

			progressBar.MarqueeAnimationSpeedChanged -= progressBar_MarqueeAnimationSpeedChanged;
			progressBar.MaximumChanged -= progressBar_MaximumChanged;
			progressBar.MinimumChanged -= progressBar_MinimumChanged;
			progressBar.StepChanged -= progressBar_StepChanged;
			progressBar.StyleChanged -= progressBar_StyleChanged;
			progressBar.ValueChanged -= progressBar_ValueChanged;
		}

		#endregion

		#region EventHandlers

		private void progressBar_MarqueeAnimationSpeedChanged(object sender, EventArgs e)
		{
			this.OnMarqueeAnimationSpeedChanged(e);
		}

		private void progressBar_MaximumChanged(object sender, EventArgs e)
		{
			this.OnMaximumChanged(e);
		}

		private void progressBar_MinimumChanged(object sender, EventArgs e)
		{
			this.OnMinimumChanged(e);
		}

		private void progressBar_StepChanged(object sender, EventArgs e)
		{
			this.OnStepChanged(e);
		}

		private void progressBar_StyleChanged(object sender, EventArgs e)
		{
			this.OnStyleChanged(e);
		}

		private void progressBar_ValueChanged(object sender, EventArgs e)
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

			NuGenProgressBar progressBar = new NuGenProgressBar(serviceProvider);
			progressBar.Size = new Size(100, 20);
			progressBar.MinimumSize = new Size(10, 5);

			return progressBar;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenToolStripProgressBar"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenProgressBarRenderer"/><para/>
		/// <see cref="INuGenControlStateTracker"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenToolStripProgressBar(INuGenServiceProvider serviceProvider)
			: base(CreateControlInstance(serviceProvider))
		{
			this.AutoSize = false;
			this.Padding = new Padding(1);
		}
	}
}
