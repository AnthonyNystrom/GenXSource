using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NuGenSVisualLib.UI.Shading;
using Microsoft.DirectX.Direct3D;

namespace NuGenSVisualLib.UI
{
	/// <summary>
	/// Summary description for Shading.
	/// </summary>
	public class ShadingConf : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
		private System.Windows.Forms.CheckBox checkBox4;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox6;

		ShadingDesc shading;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.RadioButton radioButton6;
		private System.Windows.Forms.RadioButton radioButton7;
		private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.ListBox listBox2;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ColorDialog colorDialog1;
        Device device;
        private GroupBox groupBox4;
        private CheckBox checkBox3;
        private CheckBox checkBox7;
        private CheckBox checkBox9;
		private System.Windows.Forms.RadioButton radioButton5;
		private System.Windows.Forms.RadioButton radioButton9;
		private System.Windows.Forms.RadioButton radioButton10;
		private System.Windows.Forms.RadioButton radioButton11;

		Size bgImgSize;

		public ShadingConf()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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

		public ShadingDesc ShadingDescription
		{
			get
			{
				return shading;
			}
			set
			{
				shading = value;
				SetupControls();
			}
		}

		public Device RenderDevice
		{
			set
			{
				device = value;
			}
		}

		private void SetupControls()
		{
			checkBox4.Checked = shading.AntiAliasing;
			checkBox2.Checked = shading.AtomHighlighting;

			radioButton5.Checked = (shading.DrawMode == ShadingDesc.DrawModes.Ball_Stick);
			radioButton9.Checked = (shading.DrawMode == ShadingDesc.DrawModes.Fill_Space);
			radioButton10.Checked = (shading.DrawMode == ShadingDesc.DrawModes.Sticks);
			radioButton11.Checked = (shading.DrawMode == ShadingDesc.DrawModes.Sprites);

			switch (shading.GeneralShading)
			{
				case ShadingDesc._Shading.Flat:
					radioButton3.Checked = true;
					break;
				case ShadingDesc._Shading.Smooth:
					radioButton4.Checked = true;
					break;
			}

			radioButton1.Checked = shading.BondSolid;
			radioButton2.Checked = !shading.BondSolid;

			checkBox5.Checked = shading.AtomWireframe;

			panel1.BackColor = shading.BackgroundColor;

			radioButton8.Checked = (shading.BackgroundImgMode == ShadingDesc.BgTextureMode.Centre);
			radioButton7.Checked = (shading.BackgroundImgMode == ShadingDesc.BgTextureMode.Stretch);
			radioButton6.Checked = (shading.BackgroundImgMode == ShadingDesc.BgTextureMode.Tile);

			if (listBox2.Items.Count == 0)
				listBox2.Items.Add("(None)");

            checkBox3.Checked = shading.LowDetail;
            checkBox7.Checked = shading.SymbolText;
            checkBox6.Checked = shading.WireframeOverride;
            checkBox9.Checked = shading.AtomGlow;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkBox9 = new System.Windows.Forms.CheckBox();
			this.checkBox5 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.radioButton4 = new System.Windows.Forms.RadioButton();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.checkBox6 = new System.Windows.Forms.CheckBox();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.listBox2 = new System.Windows.Forms.ListBox();
			this.radioButton8 = new System.Windows.Forms.RadioButton();
			this.radioButton7 = new System.Windows.Forms.RadioButton();
			this.radioButton6 = new System.Windows.Forms.RadioButton();
			this.button5 = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.checkBox7 = new System.Windows.Forms.CheckBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.radioButton5 = new System.Windows.Forms.RadioButton();
			this.radioButton9 = new System.Windows.Forms.RadioButton();
			this.radioButton10 = new System.Windows.Forms.RadioButton();
			this.radioButton11 = new System.Windows.Forms.RadioButton();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioButton2);
			this.groupBox1.Controls.Add(this.radioButton1);
			this.groupBox1.Location = new System.Drawing.Point(6, 45);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(178, 45);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Bond Options";
			// 
			// radioButton2
			// 
			this.radioButton2.Location = new System.Drawing.Point(64, 20);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(88, 19);
			this.radioButton2.TabIndex = 2;
			this.radioButton2.Text = "Translucent";
			// 
			// radioButton1
			// 
			this.radioButton1.Location = new System.Drawing.Point(8, 24);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(56, 19);
			this.radioButton1.TabIndex = 1;
			this.radioButton1.Text = "Solid";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkBox9);
			this.groupBox2.Controls.Add(this.checkBox5);
			this.groupBox2.Controls.Add(this.checkBox2);
			this.groupBox2.Location = new System.Drawing.Point(6, 107);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(284, 45);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Atom Options";
			// 
			// checkBox9
			// 
			this.checkBox9.Location = new System.Drawing.Point(192, 21);
			this.checkBox9.Name = "checkBox9";
			this.checkBox9.Size = new System.Drawing.Size(50, 17);
			this.checkBox9.TabIndex = 3;
			this.checkBox9.Text = "Glow";
			// 
			// checkBox5
			// 
			this.checkBox5.Location = new System.Drawing.Point(94, 20);
			this.checkBox5.Name = "checkBox5";
			this.checkBox5.Size = new System.Drawing.Size(80, 19);
			this.checkBox5.TabIndex = 2;
			this.checkBox5.Text = "Wireframe";
			// 
			// checkBox2
			// 
			this.checkBox2.Location = new System.Drawing.Point(8, 20);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(80, 19);
			this.checkBox2.TabIndex = 0;
			this.checkBox2.Text = "Highlights";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.checkBox4);
			this.groupBox3.Controls.Add(this.radioButton4);
			this.groupBox3.Controls.Add(this.radioButton3);
			this.groupBox3.Controls.Add(this.checkBox6);
			this.groupBox3.Location = new System.Drawing.Point(8, 13);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(344, 66);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "General Shading";
			// 
			// checkBox4
			// 
			this.checkBox4.Location = new System.Drawing.Point(216, 20);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(112, 19);
			this.checkBox4.TabIndex = 3;
			this.checkBox4.Text = "Anti-Aliasing";
			// 
			// radioButton4
			// 
			this.radioButton4.Location = new System.Drawing.Point(8, 39);
			this.radioButton4.Name = "radioButton4";
			this.radioButton4.Size = new System.Drawing.Size(120, 20);
			this.radioButton4.TabIndex = 1;
			this.radioButton4.Text = "Smooth Shading";
			// 
			// radioButton3
			// 
			this.radioButton3.Location = new System.Drawing.Point(8, 20);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(104, 19);
			this.radioButton3.TabIndex = 0;
			this.radioButton3.Text = "Flat Shading";
			// 
			// checkBox6
			// 
			this.checkBox6.Location = new System.Drawing.Point(216, 39);
			this.checkBox6.Name = "checkBox6";
			this.checkBox6.Size = new System.Drawing.Size(104, 20);
			this.checkBox6.TabIndex = 3;
			this.checkBox6.Text = "Wireframe";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(248, 200);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(80, 28);
			this.button1.TabIndex = 3;
			this.button1.Text = "Apply";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(348, 200);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(78, 28);
			this.button2.TabIndex = 4;
			this.button2.Text = "Cancel";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.panel1);
			this.groupBox5.Controls.Add(this.label1);
			this.groupBox5.Controls.Add(this.listBox2);
			this.groupBox5.Controls.Add(this.radioButton8);
			this.groupBox5.Controls.Add(this.radioButton7);
			this.groupBox5.Controls.Add(this.radioButton6);
			this.groupBox5.Controls.Add(this.button5);
			this.groupBox5.Location = new System.Drawing.Point(8, 85);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(344, 91);
			this.groupBox5.TabIndex = 6;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Background";
			// 
			// panel1
			// 
			this.panel1.Location = new System.Drawing.Point(296, 48);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(32, 32);
			this.panel1.TabIndex = 6;
			this.panel1.Click += new System.EventHandler(this.panel1_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(256, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Color:";
			// 
			// listBox2
			// 
			this.listBox2.Location = new System.Drawing.Point(8, 20);
			this.listBox2.Name = "listBox2";
			this.listBox2.Size = new System.Drawing.Size(240, 43);
			this.listBox2.TabIndex = 4;
			// 
			// radioButton8
			// 
			this.radioButton8.Location = new System.Drawing.Point(168, 65);
			this.radioButton8.Name = "radioButton8";
			this.radioButton8.Size = new System.Drawing.Size(72, 20);
			this.radioButton8.TabIndex = 3;
			this.radioButton8.Text = "Centre";
			// 
			// radioButton7
			// 
			this.radioButton7.Location = new System.Drawing.Point(79, 65);
			this.radioButton7.Name = "radioButton7";
			this.radioButton7.Size = new System.Drawing.Size(72, 20);
			this.radioButton7.TabIndex = 2;
			this.radioButton7.Text = "Stretch";
			// 
			// radioButton6
			// 
			this.radioButton6.Location = new System.Drawing.Point(8, 65);
			this.radioButton6.Name = "radioButton6";
			this.radioButton6.Size = new System.Drawing.Size(56, 20);
			this.radioButton6.TabIndex = 1;
			this.radioButton6.Text = "Tile";
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(256, 19);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(75, 19);
			this.button5.TabIndex = 0;
			this.button5.Text = "Browse";
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.DefaultExt = "jpg";
			this.openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.tga|All Files|*.*";
			this.openFileDialog1.Title = "Select Background Image";
			// 
			// colorDialog1
			// 
			this.colorDialog1.AnyColor = true;
			this.colorDialog1.FullOpen = true;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.radioButton11);
			this.groupBox4.Controls.Add(this.radioButton10);
			this.groupBox4.Controls.Add(this.radioButton9);
			this.groupBox4.Controls.Add(this.radioButton5);
			this.groupBox4.Controls.Add(this.checkBox7);
			this.groupBox4.Controls.Add(this.checkBox3);
			this.groupBox4.Controls.Add(this.groupBox1);
			this.groupBox4.Controls.Add(this.groupBox2);
			this.groupBox4.Location = new System.Drawing.Point(358, 13);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(298, 163);
			this.groupBox4.TabIndex = 7;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "CML Shading";
			// 
			// checkBox7
			// 
			this.checkBox7.Location = new System.Drawing.Point(108, 22);
			this.checkBox7.Name = "checkBox7";
			this.checkBox7.Size = new System.Drawing.Size(84, 17);
			this.checkBox7.TabIndex = 3;
			this.checkBox7.Text = "Symbol Text";
			// 
			// checkBox3
			// 
			this.checkBox3.Location = new System.Drawing.Point(14, 22);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(76, 17);
			this.checkBox3.TabIndex = 2;
			this.checkBox3.Text = "Low Detail";
			// 
			// radioButton5
			// 
			this.radioButton5.Location = new System.Drawing.Point(200, 16);
			this.radioButton5.Name = "radioButton5";
			this.radioButton5.Size = new System.Drawing.Size(88, 24);
			this.radioButton5.TabIndex = 4;
			this.radioButton5.Text = "Ball && Stick";
			// 
			// radioButton9
			// 
			this.radioButton9.Location = new System.Drawing.Point(200, 40);
			this.radioButton9.Name = "radioButton9";
			this.radioButton9.Size = new System.Drawing.Size(80, 24);
			this.radioButton9.TabIndex = 5;
			this.radioButton9.Text = "Fill Space";
			// 
			// radioButton10
			// 
			this.radioButton10.Location = new System.Drawing.Point(200, 64);
			this.radioButton10.Name = "radioButton10";
			this.radioButton10.Size = new System.Drawing.Size(80, 24);
			this.radioButton10.TabIndex = 6;
			this.radioButton10.Text = "Sticks";
			// 
			// radioButton11
			// 
			this.radioButton11.Location = new System.Drawing.Point(200, 88);
			this.radioButton11.Name = "radioButton11";
			this.radioButton11.Size = new System.Drawing.Size(64, 24);
			this.radioButton11.TabIndex = 7;
			this.radioButton11.Text = "Sprites";
			// 
			// ShadingConf
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(665, 239);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.groupBox3);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ShadingConf";
			this.Text = "Shading Options";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			// apply setting to config
			shading.AntiAliasing = checkBox4.Checked;
			shading.AtomHighlighting = checkBox2.Checked;

			if (radioButton5.Checked)
				shading.DrawMode = ShadingDesc.DrawModes.Ball_Stick;
			else if (radioButton9.Checked)
				shading.DrawMode = ShadingDesc.DrawModes.Fill_Space;
			else if (radioButton10.Checked)
				shading.DrawMode = ShadingDesc.DrawModes.Sticks;
			else if (radioButton11.Checked)
				shading.DrawMode = ShadingDesc.DrawModes.Sprites;


			if (radioButton3.Checked)
				shading.GeneralShading = ShadingDesc._Shading.Flat;
			else if (radioButton4.Checked)
				shading.GeneralShading = ShadingDesc._Shading.Smooth;

			shading.BondSolid = radioButton1.Checked;

			shading.AtomWireframe = checkBox5.Checked;
			shading.BackgroundColor = panel1.BackColor;

			if (radioButton6.Checked)
				shading.BackgroundImgMode = ShadingDesc.BgTextureMode.Tile;
			else if (radioButton7.Checked)
				shading.BackgroundImgMode = ShadingDesc.BgTextureMode.Stretch;
			else if (radioButton8.Checked)
				shading.BackgroundImgMode = ShadingDesc.BgTextureMode.Centre;

			if (listBox2.SelectedIndex > 0)
				shading.BackgroundImage = LoadTexture((string)listBox2.Items[listBox2.SelectedIndex]);
			else
				shading.BackgroundImage = null;

			shading.BackgroundImgSize = bgImgSize;
            shading.LowDetail = checkBox3.Checked;
            shading.SymbolText = checkBox7.Checked;
            shading.WireframeOverride = checkBox6.Checked;
            shading.AtomGlow = checkBox9.Checked;


			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private Texture LoadTexture(string fileName)
		{
			try
			{
				ImageInformation info = TextureLoader.ImageInformationFromFile(fileName);

				int finalWidth = info.Width;
				int finalHeight = info.Height;

				// resize if needed
				if (info.Height > shading.Caps.deviceCaps.MaxTextureHeight ||
					info.Width > shading.Caps.deviceCaps.MaxTextureWidth)
				{
					int x = info.Width - shading.Caps.deviceCaps.MaxTextureWidth;
					int y = info.Height - shading.Caps.deviceCaps.MaxTextureHeight;

					if (x > y)
					{
						float scale = (float)shading.Caps.deviceCaps.MaxTextureWidth / (float)info.Width;
						finalWidth = shading.Caps.deviceCaps.MaxTextureWidth;
						finalHeight = (int)((float)finalHeight * scale);
					}
					else
					{
						float scale = (float)shading.Caps.deviceCaps.MaxTextureHeight / (float)info.Height;
						finalHeight = shading.Caps.deviceCaps.MaxTextureHeight;
						finalWidth = (int)((float)finalWidth * scale);
					}
				}
					
				Texture bgImg = TextureLoader.FromFile(device, fileName,
					finalWidth, finalHeight, 0, Usage.None, Format.R8G8B8, Pool.Managed, Filter.None, Filter.None, 0);

				bgImgSize = new Size(finalWidth, finalHeight);

				return bgImg;
			}
			catch (Exception)
			{
				MessageBox.Show("Failed to load image");
			}
			return null;
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			// browse for image file
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				// attempt to load image
				if (LoadTexture(openFileDialog1.FileName) != null)
					listBox2.Items.Add(openFileDialog1.FileName);	// add to list
			}
		}

		private void panel1_Click(object sender, System.EventArgs e)
		{
			colorDialog1.Color = panel1.BackColor;
			if (colorDialog1.ShowDialog() == DialogResult.OK)
			{
				panel1.BackColor = colorDialog1.Color;
			}
		}
	}
}
