/* -----------------------------------------------
 * NuGenPageFaultsPerSecMeter.cs
 * Author: Alex Nesterov
 * --------------------------------------------- */

using en  = Genetibase.PerformanceCounters.NuGenProcessCounter;
using cat = Genetibase.PerformanceCounters.Process;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenMeters.Process
{
	/// <summary>
	/// Process -> Page Faults/sec
	/// </summary>
	[ToolboxItem(true)]
	public class NuGenPageFaultsPerSecMeter : NuGenMeterBase
	{
		#region Declarations
		
		private IContainer components = null;

		#endregion

		#region Properties.Overriden

		private PerformanceCounter counter = cat.GetCounter(en.PageFaultsPerSec);

		/// <summary>
		/// Gets the <c>System.Diagnostics.PerformanceCounter</c> object with the specified parameters set.
		/// </summary>
		/// <value></value>
		protected override PerformanceCounter CounterType
		{
			get
			{
				return this.counter;
			}
		}

		/// <summary>
		/// Gets or sets the format for the counter.
		/// </summary>
		/// <value></value>
		public override string CounterFormat
		{
			get
			{
				return "Faults/sec";
			}
			set
			{
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPageFaultsPerSecMeter"/> class.
		/// </summary>
		public NuGenPageFaultsPerSecMeter()
		{
			InitializeComponent();
		}
		
		#endregion

		#region Dispose

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null) components.Dispose();
			}
			
			base.Dispose(disposing);
		}
		
		#endregion

		#region Designer generated code
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		
		#endregion
	}
}