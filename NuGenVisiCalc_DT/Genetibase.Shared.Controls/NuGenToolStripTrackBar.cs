/* -----------------------------------------------
 * NuGenToolStripTrackBar.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.TrackBarInternals;
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
	/// Represents a track bar with custom renderer that <see cref="ToolStrip"/> and the inheritors can host.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
	public class NuGenToolStripTrackBar : NuGenToolStripControlHost
	{
		#region Properties.Appearance

		/*
		 * TickStyle
		 */

		/// <summary>
		/// </summary>
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_TrackBar_TickStyle")]
		public TickStyle TickStyle
		{
			get
			{
				return this.TrackBar.TickStyle;
			}
			set
			{
				this.TrackBar.TickStyle = value;
			}
		}

		private static readonly object _tickStyleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="TickStyle"/> property changes.
		/// </summary>
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TrackBar_TickStyleChanged")]
		public event EventHandler TickStyleChanged
		{
			add
			{
				this.Events.AddHandler(_tickStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tickStyleChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TickStyleChanged"/> event.
		/// </summary>
		protected virtual void OnTickStyleChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_tickStyleChanged, e);
		}

		#endregion

		#region Properties.Behavior

		/*
		 * LargeChange
		 */

		/// <summary>
		/// </summary>
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TrackBar_LargeChange")]
		public int LargeChange
		{
			get
			{
				return this.TrackBar.LargeChange;
			}
			set
			{
				this.TrackBar.LargeChange = value;
			}
		}

		private static readonly object _largeChangeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="LargeChange"/> property changes.
		/// </summary>
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TrackBar_LargeChangeChanged")]
		public event EventHandler LargeChangeChanged
		{
			add
			{
				this.Events.AddHandler(_largeChangeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_largeChangeChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="LargeChangeChanged"/> event.
		/// </summary>
		protected virtual void OnLargeChangeChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_largeChangeChanged, e);
		}

		/*
		 * Maximum
		 */

		/// <summary>
		/// </summary>
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TrackBar_Maximum")]
		public int Maximum
		{
			get
			{
				return this.TrackBar.Maximum;
			}
			set
			{
				this.TrackBar.Maximum = value;
			}
		}

		private static readonly object _maximumChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Maximum"/> property changes.
		/// </summary>
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TrackBar_MaximumChanged")]
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
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TrackBar_Minimum")]
		public int Minimum
		{
			get
			{
				return this.TrackBar.Minimum;
			}
			set
			{
				this.TrackBar.Minimum = value;
			}
		}

		private static readonly object _minimumChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Minimum"/> property changes.
		/// </summary>
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TrackBar_MinimumChanged")]
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

		/*
		 * SmallChange
		 */

		/// <summary>
		/// </summary>
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TrackBar_SmallChange")]
		public int SmallChange
		{
			get
			{
				return this.TrackBar.SmallChange;
			}
			set
			{
				this.TrackBar.SmallChange = value;
			}
		}

		private static readonly object _smallChangeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SmallChange"/> property changes.
		/// </summary>
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TrackBar_SmallChangeChanged")]
		public event EventHandler SmallChangeChanged
		{
			add
			{
				this.Events.AddHandler(_smallChangeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_smallChangeChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="SmallChangeChanged"/> event.
		/// </summary>
		protected virtual void OnSmallChangeChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_smallChangeChanged, e);
		}

		/*
		 * Value
		 */

		/// <summary>
		/// </summary>
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TrackBar_Value")]
		public int Value
		{
		    get
		    {
		        return this.TrackBar.Value;
		    }
		    set
		    {
		        this.TrackBar.Value = value;
		    }
		}

		private static readonly object _valueChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Value"/> property changes.
		/// </summary>
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TrackBar_ValueChanged")]
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

		#region Properties.Public.Overridden

		/*
		 * BackColor
		 */
		
		/// <summary>
		/// </summary>
		[DefaultValue(typeof(Color), "Transparent")]
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

		/*
		 * TrackBar
		 */

		/// <summary>
		/// Gets the associated <see cref="NuGenTrackBar"/>.
		/// </summary>
		protected NuGenTrackBar TrackBar
		{
			get
			{
				return (NuGenTrackBar)this.Control;
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

			NuGenTrackBar trackBar = (NuGenTrackBar)control;
			
			trackBar.LargeChangeChanged += _trackBar_LargeChangeChanged;
			trackBar.MaximumChanged += _trackBar_MaximumChanged;
			trackBar.MinimumChanged += _trackBar_MinimumChanged;
			trackBar.SmallChangeChanged += _trackBar_SmallChangeChanged;
			trackBar.TickStyleChanged += _trackBar_TickStyleChanged;
			trackBar.ValueChanged += _trackBar_ValueChanged;
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

			NuGenTrackBar trackBar = (NuGenTrackBar)control;

			trackBar.LargeChangeChanged -= _trackBar_LargeChangeChanged;
			trackBar.MaximumChanged -= _trackBar_MaximumChanged;
			trackBar.MinimumChanged -= _trackBar_MinimumChanged;
			trackBar.SmallChangeChanged -= _trackBar_SmallChangeChanged;
			trackBar.TickStyleChanged -= _trackBar_TickStyleChanged;
			trackBar.ValueChanged -= _trackBar_ValueChanged;
		}

		#endregion

		#region EventHandlers

		private void _trackBar_LargeChangeChanged(object sender, EventArgs e)
		{
			this.OnLargeChangeChanged(e);
		}

		private void _trackBar_MaximumChanged(object sender, EventArgs e)
		{
			this.OnMaximumChanged(e);
		}

		private void _trackBar_MinimumChanged(object sender, EventArgs e)
		{
			this.OnMinimumChanged(e);
		}

		private void _trackBar_SmallChangeChanged(object sender, EventArgs e)
		{
			this.OnSmallChangeChanged(e);
		}

		private void _trackBar_TickStyleChanged(object sender, EventArgs e)
		{
			this.OnTickStyleChanged(e);
		}

		private void _trackBar_ValueChanged(object sender, EventArgs e)
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

			NuGenTrackBar trackBar = new NuGenTrackBar(serviceProvider);
			trackBar.Size = new Size(100, 30);
			trackBar.MinimumSize = new Size(30, 30);

			return trackBar;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenToolStripTrackBar"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenButtonStateTracker"/><para/>
		/// <see cref="INuGenControlStateTracker"/><para/>
		/// <see cref="INuGenTrackBarRenderer"/><para/>
		/// </param>
		public NuGenToolStripTrackBar(INuGenServiceProvider serviceProvider)
			: base(CreateControlInstance(serviceProvider))
		{
			this.AutoSize = false;
		}
	}
}
