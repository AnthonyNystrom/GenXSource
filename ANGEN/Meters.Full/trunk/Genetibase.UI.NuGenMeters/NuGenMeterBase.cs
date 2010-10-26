/* -----------------------------------------------
 * NuGenMeterBase.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using Genetibase.UI.NuGenMeters.Design;

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenMeters
{
	/// <summary>
	/// Defines a flexible meter control.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenMeterBaseDesigner))]
	[ToolboxItem(false)]
	public class NuGenMeterBase : NuGenMeterGeneric
	{
		#region Declarations

		private IContainer components = null;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMeterBase"/> class.
		/// </summary>
		public NuGenMeterBase()
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

