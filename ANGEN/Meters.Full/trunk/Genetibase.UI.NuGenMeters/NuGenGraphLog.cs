/* -----------------------------------------------
 * NuGenGraphLog.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.UI.NuGenMeters.Design;
using Genetibase.UI.NuGenMeters.ComponentModel;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenMeters
{
	/// <summary>
	/// Graph output from multiple <see cref="T:Genetibase.Shared.INuGenCounter"/> sources.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenGraphLogDesigner))]
	[ProvideProperty("GraphLog", typeof(INuGenCounter))]
	[ProvideProperty("LineColor", typeof(INuGenCounter))]
	[ProvideProperty("IsBar", typeof(INuGenCounter))]
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenGraphLog), "Toolbox.NuGen.bmp")]
	public class NuGenGraphLog : NuGenGraphHost, IExtenderProvider
	{
		#region GraphLineParams

		/// <summary>
		/// Defines graph line parameters.
		/// </summary>
		class GraphLineParams
		{
			#region Properties

			/*
			 * Index
			 */

			private int index = 0;

			/// <summary>
			/// Gets the graph line index.
			/// </summary>
			public int Index
			{
				get
				{
					return this.index;
				}
			}

			/*
			 * IsBar
			 */

			private bool isBar = false;

			/// <summary>
			/// Gets or sets a value indicating whether to draw the graph line as a set of bars.
			/// </summary>
			public bool IsBar
			{
				get
				{
					return this.isBar;
				}
				set
				{
					this.isBar = value;
				}
			}

			/*
			 * LineColor
			 */

			private Color lineColor = Color.Empty;

			/// <summary>
			/// Gets or sets the color of the graph line.
			/// </summary>
			public Color LineColor
			{
				get
				{
					return this.lineColor;
				}
				set
				{
					this.lineColor = value;
				}
			}

			/*
			 * ShouldLog
			 */

			private bool shouldLog = false;

			/// <summary>
			/// Gets or sets a value indicating whether to log the graph line.
			/// </summary>
			public bool ShouldLog
			{
				get
				{
					return this.shouldLog;
				}
				set
				{
					this.shouldLog = value;
				}
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="GraphLineParams"/> class.
			/// </summary>
			/// <param name="index">The index of the graph line.</param>
			/// <param name="shouldLog">Indicates whether to log the graph line.</param>
			/// <param name="lineColor">Color of the graph line.</param>
			/// <param name="isBar">Indicates whether to draw the graph line as a set of bars.</param>
			public GraphLineParams(int index, bool shouldLog, Color lineColor, bool isBar)
			{
				this.index = index;
				this.isBar = isBar;
				this.shouldLog = shouldLog;
				this.lineColor = lineColor;
			}

			#endregion
		}

		#endregion

		#region IExtenderProvider Members

		/// <summary>
		/// Specifies whether this object can provide its extender properties to the specified object.
		/// </summary>
		/// <param name="extendee">The Object to receive the extender properties.</param>
		/// <returns>If this object can provide extender properties to the specified object returns <c>true</c>; otherwise, <c>false</c>.</returns>
		public bool CanExtend(object extendee)
		{
			return extendee is INuGenCounter && extendee != this;
		}

		#endregion

		#region Declarations

		/// <summary>
		/// Containes all the meters on the parent form.
		/// </summary>
		private Dictionary<INuGenCounter, GraphLineParams> meters = new Dictionary<INuGenCounter, GraphLineParams>();

		/// <summary>
		/// The ID of the next graph line to add.
		/// </summary>
		private int internalId = 0;

		#endregion

		#region Properties.Extended.Graph

		/// <summary>
		/// Gets the value indicating whether to log the value changes on the meter.
		/// </summary>
		/// <param name="counter">The control to get the property for.</param>
		/// <returns>The value of the property for the specified control.</returns>
		[NuGenSRCategory("GraphLogCategory")]
		[NuGenSRDescription("GraphLogDescription")]
		[DefaultValue(false)]
		public bool GetGraphLog(INuGenCounter counter)
		{
			return this.GetShouldLog(counter);
		}

		/// <summary>
		/// Sets the value indicating whether to log the value changes on the meter.
		/// </summary>
		/// <param name="counter">The control to set the property for.</param>
		/// <param name="value">The value of the property for the specified control.</param>
		public void SetGraphLog(INuGenCounter counter, bool value)
		{
			if (this.meters.ContainsKey(counter))
			{
				this.SetShouldLog(counter, value);
			}
			else
			{
				GraphLineParams glp = new GraphLineParams(internalId, value, this.GetLineColor(counter), this.GetIsBar(counter));
				this.pushGraph.SetIsBar(this.GetIndex(counter), this.GetIsBar(counter));
				this.pushGraph.Update();

				this.meters.Add(counter, glp);
				internalId++;
			}

			if (value)
			{
				this.pushGraph.AddLine(this.GetIndex(counter), this.GetLineColor(counter));
				counter.Disposed += this.counter_Disposed;
			}
			else
			{
				this.pushGraph.RemoveLine(this.GetIndex(counter));
				counter.Disposed -= this.counter_Disposed;
			}
		}

		#endregion

		#region Properties.Extended.GraphLineColor

		/// <summary>
		/// Gets the graph line color used to log the value changes on the meter.
		/// </summary>
		/// <param name="counter">The control to get the property for.</param>
		/// <returns>The value of the property for the specified control.</returns>
		[NuGenSRCategory("GraphLogCategory")]
		[NuGenSRDescription("LineColorDescription")]
		[DefaultValue(typeof(Color), "Yellow")]
		public Color GetLineColor(INuGenCounter counter)
		{
			if (this.meters.ContainsKey(counter))
			{
				return this.meters[counter].LineColor;
			}
			else
			{
				return Color.Yellow;
			}
		}

		/// <summary>
		/// Sets the graph line color used to log the value changes on the meter.
		/// </summary>
		/// <param name="counter">The control to set the property for.</param>
		/// <param name="lineColor">The value of the property for the specified control.</param>
		public void SetLineColor(INuGenCounter counter, Color lineColor)
		{
			if (this.meters.ContainsKey(counter))
			{
				this.meters[counter].LineColor = lineColor;
				this.pushGraph.SetLineColor(this.GetIndex(counter), lineColor);
				this.pushGraph.Update();
			}
		}

		#endregion

		#region Properties.Extended.GraphLineIsBar

		/*
		 * IsBar
		 */

		/// <summary>
		/// Gets the value indicating whether to draw the graph line as a set of bars.
		/// </summary>
		/// <param name="counter">The control to get the property for.</param>
		/// <returns>The value of the property for the specified control.</returns>
		[NuGenSRCategory("GraphLogCategory")]
		[NuGenSRDescription("IsBarDescription")]
		[DefaultValue(false)]
		public bool GetIsBar(INuGenCounter counter)
		{
			if (this.meters.ContainsKey(counter))
			{
				return this.meters[counter].IsBar;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Sets the value indicating whether to draw the graph line as a set of bars.
		/// </summary>
		/// <param name="nuGenMeter">The control to set the property for.</param>
		/// <param name="value">The value of the property for the specified control.</param>
		public void SetIsBar(INuGenCounter nuGenMeter, bool value)
		{
			if (this.meters.ContainsKey(nuGenMeter))
			{
				this.meters[nuGenMeter].IsBar = value;
				this.pushGraph.SetIsBar(this.GetIndex(nuGenMeter), value);
				this.pushGraph.Update();
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGraphLog.TimerTick"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected override void OnTimerTick(EventArgs e)
		{
			foreach (INuGenCounter counter in this.meters.Keys)
			{
				if (this.GetShouldLog(counter))
				{
					float counterValue = 0;

					try
					{
						counterValue = counter.NextValue();
					}
					catch
					{
						continue;
					}

					this.pushGraph.Push(
						this.GetIndex(counter),
						counterValue
					);

					this.OnValueChanged(new NuGenTargetEventArgs(counter, counterValue));
				}
			}

			this.pushGraph.Update();
			base.OnTimerTick(e);
		}

		#endregion

		#region Methods.Private

		private int GetIndex(INuGenCounter counter)
		{
			if (this.meters.ContainsKey(counter))
			{
				return this.meters[counter].Index;
			}
			else
			{
				return -1;
			}
		}

		private bool GetShouldLog(INuGenCounter counter)
		{
			if (this.meters.ContainsKey(counter))
			{
				return this.meters[counter].ShouldLog;
			}
			else
			{
				return false;
			}
		}

		private void SetShouldLog(INuGenCounter counter, bool value)
		{
			if (this.meters.ContainsKey(counter))
			{
				this.meters[counter].ShouldLog = value;
			}
		}

		#endregion

		#region EventHandlers

		private void counter_Disposed(object sender, EventArgs e)
		{
			if (sender is INuGenCounter)
			{
				INuGenCounter counter = (INuGenCounter)sender;

				this.pushGraph.RemoveLine(this.GetIndex(counter));
				this.meters.Remove(counter);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <c>Genetibase.UI.NuGenGraphLog</c> class.
		/// </summary>
		public NuGenGraphLog()
		{
		}

		#endregion
	}
}
