/* -----------------------------------------------
 * NuGenGraphLog.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Meters.Design;
using Genetibase.Meters.ComponentModel;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Meters
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

			private Int32 _index;

			/// <summary>
			/// Gets the graph line index.
			/// </summary>
			public Int32 Index
			{
				get
				{
					return _index;
				}
			}

			/*
			 * IsBar
			 */

			private Boolean _isBar;

			/// <summary>
			/// Gets or sets a value indicating whether to draw the graph line as a set of bars.
			/// </summary>
			public Boolean IsBar
			{
				get
				{
					return _isBar;
				}
				set
				{
					_isBar = value;
				}
			}

			/*
			 * LineColor
			 */

			private Color _lineColor;

			/// <summary>
			/// Gets or sets the color of the graph line.
			/// </summary>
			public Color LineColor
			{
				get
				{
					return _lineColor;
				}
				set
				{
					_lineColor = value;
				}
			}

			/*
			 * ShouldLog
			 */

			private Boolean _shouldLog;

			/// <summary>
			/// Gets or sets a value indicating whether to log the graph line.
			/// </summary>
			public Boolean ShouldLog
			{
				get
				{
					return _shouldLog;
				}
				set
				{
					_shouldLog = value;
				}
			}

			#endregion

			/// <summary>
			/// Initializes a new instance of the <see cref="GraphLineParams"/> class.
			/// </summary>
			/// <param name="index">The index of the graph line.</param>
			/// <param name="shouldLog">Indicates whether to log the graph line.</param>
			/// <param name="lineColor">Color of the graph line.</param>
			/// <param name="isBar">Indicates whether to draw the graph line as a set of bars.</param>
			public GraphLineParams(Int32 index, Boolean shouldLog, Color lineColor, Boolean isBar)
			{
				_index = index;
				_isBar = isBar;
				_shouldLog = shouldLog;
				_lineColor = lineColor;
			}
		}

		#endregion

		#region IExtenderProvider Members

		/// <summary>
		/// Specifies whether this Object can provide its extender properties to the specified Object.
		/// </summary>
		/// <param name="extendee">The Object to receive the extender properties.</param>
		/// <returns>If this Object can provide extender properties to the specified Object returns <c>true</c>; otherwise, <c>false</c>.</returns>
		public Boolean CanExtend(Object extendee)
		{
			return extendee is INuGenCounter && extendee != this;
		}

		#endregion

		#region Properties.Extended.Graph

		/// <summary>
		/// Gets the value indicating whether to log the value changes on the meter.
		/// </summary>
		/// <param name="counter">The control to get the property for.</param>
		/// <returns>The value of the property for the specified control.</returns>
		[NuGenSRCategory("Category_GraphLog")]
		[NuGenSRDescription("Description_GraphLog")]
		[DefaultValue(false)]
		public Boolean GetGraphLog(INuGenCounter counter)
		{
			return this.GetShouldLog(counter);
		}

		/// <summary>
		/// Sets the value indicating whether to log the value changes on the meter.
		/// </summary>
		/// <param name="counter">The control to set the property for.</param>
		/// <param name="value">The value of the property for the specified control.</param>
		public void SetGraphLog(INuGenCounter counter, Boolean value)
		{
			if (_meters.ContainsKey(counter))
			{
				this.SetShouldLog(counter, value);
			}
			else
			{
				GraphLineParams glp = new GraphLineParams(_id, value, this.GetLineColor(counter), this.GetIsBar(counter));
				_PushGraph.SetIsBar(this.GetIndex(counter), this.GetIsBar(counter));
				_PushGraph.Update();

				_meters.Add(counter, glp);
				_id++;
			}

			if (value)
			{
				_PushGraph.AddLine(this.GetIndex(counter), this.GetLineColor(counter));
				counter.Disposed += this.counter_Disposed;
			}
			else
			{
				_PushGraph.RemoveLine(this.GetIndex(counter));
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
		[NuGenSRCategory("Category_GraphLog")]
		[NuGenSRDescription("Description_LineColor")]
		[DefaultValue(typeof(Color), "Yellow")]
		public Color GetLineColor(INuGenCounter counter)
		{
			if (_meters.ContainsKey(counter))
			{
				return _meters[counter].LineColor;
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
			if (_meters.ContainsKey(counter))
			{
				_meters[counter].LineColor = lineColor;
				_PushGraph.SetLineColor(this.GetIndex(counter), lineColor);
				_PushGraph.Update();
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
		[NuGenSRCategory("Category_GraphLog")]
		[NuGenSRDescription("Description_IsBar")]
		[DefaultValue(false)]
		public Boolean GetIsBar(INuGenCounter counter)
		{
			if (_meters.ContainsKey(counter))
			{
				return _meters[counter].IsBar;
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
		public void SetIsBar(INuGenCounter nuGenMeter, Boolean value)
		{
			if (_meters.ContainsKey(nuGenMeter))
			{
				_meters[nuGenMeter].IsBar = value;
				_PushGraph.SetIsBar(this.GetIndex(nuGenMeter), value);
				_PushGraph.Update();
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGraphLog.TimerTick"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected override void OnTimerTick(EventArgs e)
		{
			foreach (INuGenCounter counter in _meters.Keys)
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

					_PushGraph.Push(
						this.GetIndex(counter),
						counterValue
					);

					this.OnValueChanged(new NuGenTargetEventArgs(counter, counterValue));
				}
			}

			_PushGraph.Update();
			base.OnTimerTick(e);
		}

		#endregion

		#region Methods.Private

		private Int32 GetIndex(INuGenCounter counter)
		{
			if (_meters.ContainsKey(counter))
			{
				return _meters[counter].Index;
			}
			else
			{
				return -1;
			}
		}

		private Boolean GetShouldLog(INuGenCounter counter)
		{
			if (_meters.ContainsKey(counter))
			{
				return _meters[counter].ShouldLog;
			}
			else
			{
				return false;
			}
		}

		private void SetShouldLog(INuGenCounter counter, Boolean value)
		{
			if (_meters.ContainsKey(counter))
			{
				_meters[counter].ShouldLog = value;
			}
		}

		#endregion

		#region EventHandlers

		private void counter_Disposed(Object sender, EventArgs e)
		{
			if (sender is INuGenCounter)
			{
				INuGenCounter counter = (INuGenCounter)sender;

				_PushGraph.RemoveLine(this.GetIndex(counter));
				_meters.Remove(counter);
			}
		}

		#endregion

		/// <summary>
		/// Containes all the meters on the parent form.
		/// </summary>
		private Dictionary<INuGenCounter, GraphLineParams> _meters = new Dictionary<INuGenCounter, GraphLineParams>();

		/// <summary>
		/// The ID of the next graph line to add.
		/// </summary>
		private Int32 _id = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenGraphLog"/> class.
		/// </summary>
		public NuGenGraphLog()
		{
		}
	}
}
