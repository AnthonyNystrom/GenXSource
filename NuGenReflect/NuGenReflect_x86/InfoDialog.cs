/***
 * 
 *  ASMEX by RiskCare Ltd.
 * 
 * This source is copyright (C) 2002 RiskCare Ltd. All rights reserved.
 * 
 * Disclaimer:
 * This code is provided 'as is', with absolutely no warranty expressed or
 * implied.  Any use of this code is at your own risk.
 *   
 * You are hereby granted the right to redistribute this source unmodified
 * in its original archive. 
 * You are hereby granted the right to use this code, or code based on it,
 * provided that you acknowledge RiskCare Ltd somewhere in the documentation
 * of your application. 
 * You are hereby granted the right to distribute changes to this source, 
 * provided that:
 * 
 * 1 -- This copyright notice is retained unchanged 
 * 2 -- Your changes are clearly marked 
 * 
 * Enjoy!
 * 
 * --------------------------------------------------------------------
 * 
 * If you use this code or have comments on it, please mail me at 
 * support@jbrowse.com or ben.peterson@riskcare.com
 * 
 */

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace Genetibase.Debug
{
	/// <summary>
	/// A dialog that displays some amount of text.
	/// For reasons unknown to me, the text is sometimes truncated at about 30000 characters, which
	/// seems to be a problem with 'appendtext'.
	/// This is really really irritating and I can't be bothered to work round it.
	/// </summary>
	internal class InfoDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox tbInfo;
		private System.Windows.Forms.Label lblTopic;
		private System.ComponentModel.Container components = null;
		private Janus.Windows.EditControls.UIButton btnSave;
		private DevComponents.DotNetBar.PanelEx panelEx1;
		private Janus.Windows.EditControls.UIButton btnbutton1;

		string _actualFuckingInfo;

		public InfoDialog(string capt, string topic, string info)
		{
			InitializeComponent();

			Text = capt;
			lblTopic.Text = topic;
			Info = info;
		}

		public InfoDialog(string capt, string topic)
		{
			InitializeComponent();

			Text = capt;
			lblTopic.Text = topic;
		}

		public void AddLine(string s)
		{
			tbInfo.AppendText(s);
		}

		public string Info
		{
			get{return _actualFuckingInfo;}
			set
			{
				_actualFuckingInfo = value;

				if (value.Length > 100000)
					tbInfo.Text = value.Substring(0,100000);
				else
					tbInfo.Text = value;
				
			}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoDialog));
            this.tbInfo = new System.Windows.Forms.TextBox();
            this.lblTopic = new System.Windows.Forms.Label();
            this.btnSave = new Janus.Windows.EditControls.UIButton();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.btnbutton1 = new Janus.Windows.EditControls.UIButton();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbInfo
            // 
            this.tbInfo.BackColor = System.Drawing.SystemColors.Info;
            this.tbInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbInfo.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.tbInfo.Location = new System.Drawing.Point(0, 40);
            this.tbInfo.MaxLength = 1000000;
            this.tbInfo.Multiline = true;
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbInfo.Size = new System.Drawing.Size(542, 576);
            this.tbInfo.TabIndex = 0;
            this.tbInfo.Text = "Info";
            this.tbInfo.WordWrap = false;
            // 
            // lblTopic
            // 
            this.lblTopic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTopic.BackColor = System.Drawing.Color.Transparent;
            this.lblTopic.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.lblTopic.Location = new System.Drawing.Point(8, 8);
            this.lblTopic.Name = "lblTopic";
            this.lblTopic.Size = new System.Drawing.Size(374, 24);
            this.lblTopic.TabIndex = 1;
            this.lblTopic.Text = "Info";
            this.lblTopic.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.Location = new System.Drawing.Point(390, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(72, 24);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "&Save";
            this.btnSave.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panelEx1
            // 
            this.panelEx1.Controls.Add(this.btnbutton1);
            this.panelEx1.Controls.Add(this.lblTopic);
            this.panelEx1.Controls.Add(this.btnSave);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(542, 40);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
            this.panelEx1.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 3;
            // 
            // btnbutton1
            // 
            this.btnbutton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnbutton1.BackColor = System.Drawing.SystemColors.Control;
            this.btnbutton1.Location = new System.Drawing.Point(462, 8);
            this.btnbutton1.Name = "btnbutton1";
            this.btnbutton1.Size = new System.Drawing.Size(72, 24);
            this.btnbutton1.TabIndex = 3;
            this.btnbutton1.Text = "Cancel";
            this.btnbutton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2003;
            this.btnbutton1.Click += new System.EventHandler(this.btnbutton1_Click);
            // 
            // InfoDialog
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(542, 616);
            this.Controls.Add(this.tbInfo);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InfoDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Disassembly";
            this.panelEx1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			SaveFileDialog dlg = new SaveFileDialog();

			try
			{
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					FileStream fs = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write, FileShare.None, 1024, false);

					StreamWriter sw = new StreamWriter(fs);

					sw.Write(_actualFuckingInfo);
					sw.Close();
					fs.Close();

					MessageBox.Show("File saved.");
					return;
				}
			}
			catch(Exception ee)
			{
				MessageBox.Show("Unable to save file", ee.Message, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

        private void btnbutton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
	}
}
