/* -----------------------------------------------
 * MainForm.Designer.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System.ComponentModel;

namespace Genetibase.UI.NuGenInterface.Demo
{
	partial class MainForm
	{
		#region Declarations

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private IContainer _Components = null;
		
		#endregion

		#region Dispose

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (_Components != null))
			{
				_Components.Dispose();
			}

			base.Dispose(disposing);
		}

		#endregion

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._RibbonManager = new Genetibase.UI.NuGenInterface.NuGenRibbonManager();
			this.SuspendLayout();
			// 
			// nuGenRibbonManager1
			// 
			this._RibbonManager.HostForm = this;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(574, 352);
			this.DoubleBuffered = true;
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.ResumeLayout(false);

		}

		#endregion

		private NuGenRibbonManager _RibbonManager;
	}
}
