/* -----------------------------------------------
 * NuGenGraph.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Meters.Design;
using Genetibase.Meters.ComponentModel;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Meters
{
	/// <summary>
	/// Defines the base functionality for the graph controls.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenGraphGenericDesigner))]
	[ToolboxItem(true)]
	public class NuGenGraph : NuGenGraphHost
	{
		#region Properties.Appearance

		/*
		 * IsBar
		 */

		/// <summary>
		/// Gets or sets a value indicating whether to show the graph line as a set of bars.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_IsBar")]
		[DefaultValue(false)]
		public virtual Boolean IsBar
		{
			get
			{
				return _PushGraph.GetIsBar(ID);
			}
			set
			{
				if (_PushGraph.GetIsBar(ID) != value)
				{
					_PushGraph.SetIsBar(ID, value);
					this.OnIsBarChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Object _isBarChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGraphGeneric.IsBar"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_IsBarChanged")]
		public event EventHandler IsBarChanged
		{
			add
			{
				this.Events.AddHandler(_isBarChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_isBarChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGraphGeneric.IsBarChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnIsBarChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_isBarChanged, e);
		}

		/*
		 * LineColor
		 */

		/// <summary>
		/// Gets or sets the graph line color.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_LineColor")]
		[DefaultValue(typeof(Color), "Yellow")]
		public virtual Color LineColor
		{
			get
			{
				return _PushGraph.GetLineColor(ID);
			}
			set
			{
				if (_PushGraph.GetLineColor(ID) != value)
				{
					_PushGraph.SetLineColor(ID, value);
					this.OnLineColorChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Object _lineColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGraphGeneric.LineColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_LineColorChanged")]
		public event EventHandler LineColorChanged
		{
			add
			{
				this.Events.AddHandler(_lineColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_lineColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGraphGeneric.LineColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnLineColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_lineColorChanged, e);
		}

		#endregion

		private PerformanceCounter _counterType;

		/// <summary>
		/// Gets the type of the counter.
		/// </summary>
		protected override PerformanceCounter CounterType
		{
			get
			{
				if (_counterType == null)
				{
					_counterType = new PerformanceCounter();
				}

				return _counterType;
			}
		}

		/// <summary>
		/// The ID of the current graph line in the collection.
		/// </summary>
		private const Int32 ID = 0;

		/// <summary>
		/// Raises the <c>Genetibase.UI.NuGenGenericBase.TimerTick</c> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected override void OnTimerTick(EventArgs e)
		{
			float counterValue = 0.0f;

			try
			{
				counterValue = this.CounterType.NextValue();
			}
			catch
			{
				return;
			}

			_PushGraph.Push(ID, counterValue);
			this.OnValueChanged(new NuGenTargetEventArgs(this, counterValue));
			_PushGraph.Update();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenGraph"/> class.
		/// </summary>
		public NuGenGraph()
		{
			_PushGraph.AddLine(ID, Color.Yellow);
		}
	}
}
