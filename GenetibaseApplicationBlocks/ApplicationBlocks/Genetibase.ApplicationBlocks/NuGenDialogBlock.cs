/* -----------------------------------------------
 * NuGenDialogBlock.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.ApplicationBlocks.Design;

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	/// <summary>
	/// Ok/Cancel/... block.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(true)]
	[Designer(typeof(NuGenDialogBlockDesigner))]
	public class NuGenDialogBlock : UserControl
	{
		#region Declarations
	
		private Container components = null;
		private NuGenBevel bevel = null;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDialogBlock"/> class.
		/// </summary>
		public NuGenDialogBlock()
		{
			InitializeComponent();
			this.Dock = DockStyle.Bottom;
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
		
		#region Component Designer generated code
		
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.bevel = new Genetibase.Shared.Controls.NuGenBevel();
			this.SuspendLayout();
			// 
			// bevel
			// 
			this.bevel.Dock = System.Windows.Forms.DockStyle.Top;
			this.bevel.Location = new System.Drawing.Point(0, 0);
			this.bevel.Name = "bevel";
			this.bevel.Size = new System.Drawing.Size(416, 2);
			this.bevel.TabIndex = 0;
			this.bevel.TabStop = false;
			// 
			// NuGenDialogBlock
			// 
			this.Controls.Add(this.bevel);
			this.Name = "NuGenDialogBlock";
			this.Size = new System.Drawing.Size(416, 40);
			this.ResumeLayout(false);

		}
		
		#endregion
	}
}
