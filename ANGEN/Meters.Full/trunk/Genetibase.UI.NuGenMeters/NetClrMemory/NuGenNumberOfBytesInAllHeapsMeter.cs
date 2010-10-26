/* -----------------------------------------------
 * NuGenNumberOfBytesInAllHeapsMeter.cs
 * Author: Alex Nesterov
 * --------------------------------------------- */

using en  = Genetibase.PerformanceCounters.NuGenNetClrMemoryCounter;
using cat = Genetibase.PerformanceCounters.NetClrMemory;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenMeters.NetClrMemory
{
	/// <summary>
	/// .NET CLR Memory -> NumberOfBytesInAllHeaps
	/// </summary>
	[ToolboxItem(true)]
	public class NuGenNumberOfBytesInAllHeapsMeter : Genetibase.UI.NuGenMeters.NuGenByteMeterBase
	{
		#region Declarations
		
		private IContainer components = null;

		#endregion

		#region Properties.Overriden

		private PerformanceCounter counter = cat.GetCounter(en.NumberOfBytesInAllHeaps);

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
				return "Bytes";
			}
			set
			{
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenNumberOfBytesInAllHeapsMeter"/> class.
		/// </summary>
		public NuGenNumberOfBytesInAllHeapsMeter()
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
