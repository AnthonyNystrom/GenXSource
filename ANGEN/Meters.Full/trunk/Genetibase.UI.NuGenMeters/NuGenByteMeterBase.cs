/* -----------------------------------------------
 * NuGenByteMeterBase.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using Genetibase.Shared;
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
	/// Extends the <c>NuGenMeterBase</c> class and provides a property to convert byte value to KB and MB.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(false)]
	public class NuGenByteMeterBase : NuGenMeterBase
	{
		#region Properties.Overriden

		/*
		 * Divider
		 */

		/// <summary>
		/// Determines the divider for the counter value.
		/// </summary>
		private float divider = 1.0f;

		/// <summary>
		/// Gets or sets a divider for the counter value.
		/// </summary>
		/// <value></value>
		protected override float Divider
		{
			get
			{
				return this.divider;
			}
			set
			{
				this.divider = value;
			}
		}

		/*
		 * Prefix
		 */

		/// <summary>
		/// Determines the counter format prefix.
		/// </summary>
		private string prefix = "";

		/// <summary>
		/// Gets or sets the counter format prefix.
		/// </summary>
		/// <value></value>
		protected override string Prefix
		{
			get
			{
				return this.prefix;
			}
			set
			{
				this.prefix = value;
			}
		}

		#endregion

		#region Properties.PerformanceCounter
		
		/// <summary>
		/// Determines the value representation of the counter.
		/// </summary>
		private NuGenValueRepresentation valueRepresentation = NuGenValueRepresentation.Bytes;

		/// <summary>
		/// Gets or sets the value representation of the counter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PerformanceCounterCategory")]
		[NuGenSRDescription("ValueRepresentationDescription")]
		public NuGenValueRepresentation ValueRepresentation
		{
			get { return this.valueRepresentation; }
			set 
			{
				if (this.valueRepresentation != value) 
				{
					switch (value) 
					{
						case NuGenValueRepresentation.Bytes:
							this.Divider = 1;
							this.Prefix = "";
							break;
						case NuGenValueRepresentation.GigaBytes:
							this.Divider = 1024 * 1024 * 1024;
							this.Prefix = Properties.Resources.GigaPrefix;
							break;
						case NuGenValueRepresentation.KiloBytes:
							this.Divider = 1024;
							this.Prefix = Properties.Resources.KiloPrefix;
							break;
						case NuGenValueRepresentation.MegaBytes:
							this.Divider = 1024 * 1024;
							this.Prefix = Properties.Resources.MegaPrefix;
							break;
					}

					this.valueRepresentation = value;
					this.OnValueRepresentationChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventValueRepresentationChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenByteMeterBase.ValueRepresentation"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ValueRepresentationChangedDescription")]
		public event EventHandler ValueRepresentationChanged
		{
			add
			{
				this.Events.AddHandler(EventValueRepresentationChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventValueRepresentationChanged, value);
			}
		}


		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenByteMeterBase.ValueRepresentationChanged"/>
		/// event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough]
		protected virtual void OnValueRepresentationChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventValueRepresentationChanged, e);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenByteMeterBase"/> class.
		/// </summary>
		public NuGenByteMeterBase()
		{
		}
		
		#endregion
	}
}
