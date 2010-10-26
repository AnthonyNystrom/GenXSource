using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Genetibase.Windows;

namespace WindowsApplication1
{
	/// <summary>
	/// Summary description for Form2.
	/// </summary>
	public class Form2 : System.Windows.Forms.Form
	{
		private NuGenUISnap stickyWindow;

		private System.Windows.Forms.CheckBox checkStickToScreen;
		private System.Windows.Forms.CheckBox checkStickToOthers;
		private System.Windows.Forms.CheckBox checkStickOnResize;
		private System.Windows.Forms.CheckBox checkStickOnMove;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form2()
		{
			InitializeComponent();
            stickyWindow = new NuGenUISnap(this);
			checkStickOnMove.Checked	= stickyWindow.StickOnMove;
			checkStickOnResize.Checked	= stickyWindow.StickOnResize;
			checkStickToOthers.Checked	= stickyWindow.StickToOther;
			checkStickToScreen.Checked	= stickyWindow.StickToScreen;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.checkStickToScreen = new System.Windows.Forms.CheckBox();
			this.checkStickToOthers = new System.Windows.Forms.CheckBox();
			this.checkStickOnResize = new System.Windows.Forms.CheckBox();
			this.checkStickOnMove = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// checkStickToScreen
			// 
			this.checkStickToScreen.Location = new System.Drawing.Point(22, 26);
			this.checkStickToScreen.Name = "checkStickToScreen";
			this.checkStickToScreen.TabIndex = 0;
			this.checkStickToScreen.Text = "Stick to Screen";
			this.checkStickToScreen.CheckedChanged += new System.EventHandler(this.checkStickToScreen_CheckedChanged);
			// 
			// checkStickToOthers
			// 
			this.checkStickToOthers.Location = new System.Drawing.Point(22, 58);
			this.checkStickToOthers.Name = "checkStickToOthers";
			this.checkStickToOthers.TabIndex = 1;
			this.checkStickToOthers.Text = "Stick to Others";
			this.checkStickToOthers.CheckedChanged += new System.EventHandler(this.checkStickToOthers_CheckedChanged);
			// 
			// checkStickOnResize
			// 
			this.checkStickOnResize.Location = new System.Drawing.Point(22, 122);
			this.checkStickOnResize.Name = "checkStickOnResize";
			this.checkStickOnResize.TabIndex = 2;
			this.checkStickOnResize.Text = "Stick on Resize";
			this.checkStickOnResize.CheckedChanged += new System.EventHandler(this.checkStickOnResize_CheckedChanged);
			// 
			// checkStickOnMove
			// 
			this.checkStickOnMove.Location = new System.Drawing.Point(22, 154);
			this.checkStickOnMove.Name = "checkStickOnMove";
			this.checkStickOnMove.TabIndex = 3;
			this.checkStickOnMove.Text = "Stick On Move";
			this.checkStickOnMove.CheckedChanged += new System.EventHandler(this.checkStickOnMove_CheckedChanged);
			// 
			// Form2
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.checkStickOnMove);
			this.Controls.Add(this.checkStickOnResize);
			this.Controls.Add(this.checkStickToOthers);
			this.Controls.Add(this.checkStickToScreen);
			this.Name = "Form2";
			this.Text = "Form2";
			this.Load += new System.EventHandler(this.Form2_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void Form2_Load(object sender, System.EventArgs e)
		{
		}

		private void checkStickToScreen_CheckedChanged(object sender, System.EventArgs e)
		{
			stickyWindow.StickToScreen = checkStickToScreen.Checked;
		}

		private void checkStickToOthers_CheckedChanged(object sender, System.EventArgs e)
		{
			stickyWindow.StickToOther = checkStickToOthers.Checked;
		}

		private void checkStickOnResize_CheckedChanged(object sender, System.EventArgs e)
		{
			stickyWindow.StickOnResize = checkStickOnResize.Checked;
		}

		private void checkStickOnMove_CheckedChanged(object sender, System.EventArgs e)
		{
			stickyWindow.StickOnMove = checkStickOnMove.Checked;
		}
	}
}
