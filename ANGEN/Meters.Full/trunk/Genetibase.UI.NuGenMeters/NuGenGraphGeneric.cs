/* -----------------------------------------------
 * NuGenGraphGeneric.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using en = Genetibase.PerformanceCounters.NuGenProcessorCounter;
using pc = Genetibase.PerformanceCounters.Processor;

using Genetibase.Shared;
using Genetibase.UI.NuGenMeters.Design;
using Genetibase.UI.NuGenMeters.ComponentModel;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenMeters
{
	/// <summary>
	/// Defines the base functionality for the graph controls.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenGraphGenericDesigner))]
	[ToolboxItem(true)]
	public class NuGenGraphGeneric : NuGenGraphHost
	{
		#region Properties.Appearance

		/*
		 * IsBar
		 */

		/// <summary>
		/// Gets or sets a value indicating whether to show the graph line as a set of bars.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("IsBarDescription")]
		[DefaultValue(false)]
		public virtual bool IsBar
		{
			get
			{
				return this.pushGraph.GetIsBar(ID);
			}
			set
			{
				if (this.pushGraph.GetIsBar(ID) != value)
				{
					this.pushGraph.SetIsBar(ID, value);
					this.OnIsBarChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventIsBarChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGraphGeneric.IsBar"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("IsBarChangedDescription")]
		public event EventHandler IsBarChanged
		{
			add
			{
				this.Events.AddHandler(EventIsBarChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventIsBarChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGraphGeneric.IsBarChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnIsBarChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventIsBarChanged, e);
		}

		/*
		 * LineColor
		 */

		/// <summary>
		/// Gets or sets the graph line color.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("LineColorDescription")]
		[DefaultValue(typeof(Color), "Yellow")]
		public virtual Color LineColor
		{
			get
			{
				return this.pushGraph.GetLineColor(ID);
			}
			set
			{
				if (this.pushGraph.GetLineColor(ID) != value)
				{
					this.pushGraph.SetLineColor(ID, value);
					this.OnLineColorChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventLineColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGraphGeneric.LineColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("LineColorChangedDescription")]
		public event EventHandler LineColorChanged
		{
			add
			{
				this.Events.AddHandler(EventLineColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventLineColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGraphGeneric.LineColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnLineColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventLineColorChanged, e);
		}

		#endregion

		#region Properties.Protected.Overriden

		/// <summary>
		/// Defines the type of the counter.
		/// </summary>
		private PerformanceCounter counterType = pc.GetCounter(en.PercentProcessorTime);

		/// <summary>
		/// Gets the type of the counter.
		/// </summary>
		protected override PerformanceCounter CounterType
		{
			get
			{
				return this.counterType;
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// The ID of the current graph line in the collection.
		/// </summary>
		private const int ID = 0;

		/// <summary>
		/// Raises the <c>Genetibase.UI.NuGenGenericBase.TimerTick</c> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected override void OnTimerTick(EventArgs e)
		{
			try
			{
				float counterValue = this.CounterType.NextValue();
				this.pushGraph.Push(ID, counterValue);
				this.OnValueChanged(new NuGenTargetEventArgs(this, counterValue));
			}
			finally
			{
				this.pushGraph.Update();
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <c>NuGenGraphBase</c> class.
		/// </summary>
		public NuGenGraphGeneric()
		{
			this.pushGraph.AddLine(ID, Color.Yellow);
		}

		#endregion
	}
}
