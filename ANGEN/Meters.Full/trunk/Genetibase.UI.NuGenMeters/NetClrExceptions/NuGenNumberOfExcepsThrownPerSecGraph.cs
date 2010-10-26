/* -----------------------------------------------
 * NuGenNumberOfExcepsThrownPerSecGraph.cs
 * Author: Alex Nesterov
 * --------------------------------------------- */

using en =  Genetibase.PerformanceCounters.NuGenNetClrExceptionsCounter;
using cat = Genetibase.PerformanceCounters.NetClrExceptions;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenMeters.NetClrExceptions
{
	/// <summary>
	/// .NET CLR Exceptions -> NumberOfExcepsThrown/sec
	/// </summary>
	[ToolboxItem(true)]
	public class NuGenNumberOfExcepsThrownPerSecGraph : NuGenGraphBase
	{
		#region Declarations
		
		private IContainer components = null;

		#endregion

		#region Properties.Overriden

		private PerformanceCounter counter = cat.GetCounter(en.NumberOfExcepsThrownPerSec);

		/// <summary>
		/// Gets the type of the counter.
		/// </summary>
		/// <value></value>
		protected override PerformanceCounter CounterType
		{
			get
			{
				return this.counter;
			}
		}


		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenNumberOfExcepsThrownPerSecGraph"/> class.
		/// </summary>
		public NuGenNumberOfExcepsThrownPerSecGraph()
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
