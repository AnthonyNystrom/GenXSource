using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NuGenSVisualLib.UI.Lighting;
using Microsoft.DirectX.Direct3D;
using System.IO;

namespace NuGenSVisualLib.UI
{
	/// <summary>
	/// Summary description for Shading.
	/// </summary>
	public class LightingConfWin : System.Windows.Forms.Form
    {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        #region Components
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
        private System.Windows.Forms.PropertyGrid lightPropertyGrid;
        private System.Windows.Forms.CheckBox checkBox7;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.CheckBox checkBox3;
		private LightPreview lightPreview1;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TrackBar trackBar2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TrackBar trackBar3;
		private System.Windows.Forms.CheckBox checkBox9;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Button button5;
        private ListBox listBox2;
        private CheckBox checkBox1;
        private OpenFileDialog openFileDialog1;
        private Button button6;
        #endregion

        LightingConf currentLighting;
        LightingConf editLighting;
        int maxNumLights;
        ArrayList setups;
        ArrayList setupFilenames;
        bool changedEditing;
        private Button button7;
        private SaveFileDialog saveFileDialog1;
        private CheckBox checkBox2;
        int editIndex = 0;

        public LightingConfWin(int maxNumLights)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            this.maxNumLights = maxNumLights;
            listBox2.Items.Add("<Current>");

            setups = new ArrayList();
            setupFilenames = new ArrayList();
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

		public LightingConf LightingConfig
		{
			get
			{
				return currentLighting;
			}
			set
			{
				currentLighting = value;
                editLighting = (LightingConf)value.Clone();
				SetupLights();
			}
		}

		private void SetupLights()
		{
			listBox1.Items.Clear();

            for (int i = 0; i < editLighting.lights.Length; i++)
			{
                if (editLighting.lights[i] != null)
					listBox1.Items.Add("light" + i.ToString());
			}

            changedEditing = false;
            lightPreview1.Lights = editLighting;
            lightPreview1.DrawPreview();

            checkBox2.Checked = editLighting.enabled;
            checkBox9.Checked = editLighting.rotate;
            checkBox1.Checked = editLighting.specularLighting;
            checkBox7.Checked = editLighting.ambientLighting;
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.trackBar3 = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.lightPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.checkBox9 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.lightPreview1 = new NuGenSVisualLib.UI.LightPreview();
            this.groupBox4.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(229, 330);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 24);
            this.button1.TabIndex = 3;
            this.button1.Text = "Apply";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(360, 330);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 24);
            this.button2.TabIndex = 4;
            this.button2.Text = "Cancel";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button7);
            this.groupBox4.Controls.Add(this.groupBox6);
            this.groupBox4.Controls.Add(this.button4);
            this.groupBox4.Controls.Add(this.button3);
            this.groupBox4.Controls.Add(this.lightPropertyGrid);
            this.groupBox4.Controls.Add(this.listBox1);
            this.groupBox4.Location = new System.Drawing.Point(283, 13);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(400, 309);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Lights Editor";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(8, 279);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 9;
            this.button7.Text = "Save As..";
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.groupBox7);
            this.groupBox6.Controls.Add(this.lightPreview1);
            this.groupBox6.Controls.Add(this.checkBox3);
            this.groupBox6.Location = new System.Drawing.Point(8, 143);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(376, 130);
            this.groupBox6.TabIndex = 8;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Light Preview";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.trackBar3);
            this.groupBox7.Controls.Add(this.label4);
            this.groupBox7.Controls.Add(this.trackBar2);
            this.groupBox7.Controls.Add(this.label3);
            this.groupBox7.Controls.Add(this.label2);
            this.groupBox7.Controls.Add(this.trackBar1);
            this.groupBox7.Enabled = false;
            this.groupBox7.Location = new System.Drawing.Point(8, 39);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(224, 85);
            this.groupBox7.TabIndex = 10;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Edit Light";
            // 
            // trackBar3
            // 
            this.trackBar3.AutoSize = false;
            this.trackBar3.Location = new System.Drawing.Point(24, 59);
            this.trackBar3.Maximum = 200;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Size = new System.Drawing.Size(184, 13);
            this.trackBar3.SmallChange = 10;
            this.trackBar3.TabIndex = 5;
            this.trackBar3.TickFrequency = 10;
            this.trackBar3.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar3.ValueChanged += new System.EventHandler(this.trackBar3_ValueChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Z:";
            // 
            // trackBar2
            // 
            this.trackBar2.AutoSize = false;
            this.trackBar2.Location = new System.Drawing.Point(24, 39);
            this.trackBar2.Maximum = 200;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(184, 13);
            this.trackBar2.SmallChange = 10;
            this.trackBar2.TabIndex = 3;
            this.trackBar2.TickFrequency = 10;
            this.trackBar2.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar2.ValueChanged += new System.EventHandler(this.trackBar2_ValueChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Y:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "X:";
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.Location = new System.Drawing.Point(24, 20);
            this.trackBar1.Maximum = 200;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(184, 13);
            this.trackBar1.SmallChange = 10;
            this.trackBar1.TabIndex = 0;
            this.trackBar1.TickFrequency = 10;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(8, 20);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(136, 19);
            this.checkBox3.TabIndex = 8;
            this.checkBox3.Text = "Preview All Lights";
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(96, 117);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 19);
            this.button4.TabIndex = 6;
            this.button4.Text = "Remove";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(8, 117);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 19);
            this.button3.TabIndex = 5;
            this.button3.Text = "Add";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // lightPropertyGrid
            // 
            this.lightPropertyGrid.HelpVisible = false;
            this.lightPropertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.lightPropertyGrid.Location = new System.Drawing.Point(184, 20);
            this.lightPropertyGrid.Name = "lightPropertyGrid";
            this.lightPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.lightPropertyGrid.Size = new System.Drawing.Size(200, 117);
            this.lightPropertyGrid.TabIndex = 4;
            this.lightPropertyGrid.ToolbarVisible = false;
            this.lightPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.lightPropertyGrid_PropertyValueChanged);
            // 
            // listBox1
            // 
            this.listBox1.Location = new System.Drawing.Point(8, 20);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(168, 95);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedValueChanged += new System.EventHandler(this.listBox1_SelectedValueChanged);
            // 
            // checkBox9
            // 
            this.checkBox9.Location = new System.Drawing.Point(16, 42);
            this.checkBox9.Name = "checkBox9";
            this.checkBox9.Size = new System.Drawing.Size(160, 20);
            this.checkBox9.TabIndex = 9;
            this.checkBox9.Text = "Rotate lights with camera";
            this.checkBox9.CheckedChanged += new System.EventHandler(this.checkBox9_CheckedChanged);
            // 
            // checkBox7
            // 
            this.checkBox7.Location = new System.Drawing.Point(150, 66);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(108, 20);
            this.checkBox7.TabIndex = 4;
            this.checkBox7.Text = "Ambient Lighting";
            this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.checkBox9);
            this.groupBox1.Controls.Add(this.checkBox7);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(265, 93);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            // 
            // checkBox2
            // 
            this.checkBox2.Location = new System.Drawing.Point(16, 19);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(65, 17);
            this.checkBox2.TabIndex = 11;
            this.checkBox2.Text = "Enabled";
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(16, 68);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(108, 17);
            this.checkBox1.TabIndex = 10;
            this.checkBox1.Text = "Specular Lighting";
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button6);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.listBox2);
            this.groupBox2.Location = new System.Drawing.Point(12, 112);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(265, 210);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Light Setups";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(167, 180);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(91, 23);
            this.button6.TabIndex = 2;
            this.button6.Text = "Load/Edit";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(6, 180);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 1;
            this.button5.Text = "Add";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // listBox2
            // 
            this.listBox2.Location = new System.Drawing.Point(6, 23);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(252, 147);
            this.listBox2.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "lightsetup";
            this.openFileDialog1.Filter = "Lighting Setups|*.lightsetup|All Files|*.*";
            this.openFileDialog1.Title = "Open Lighting Setup";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "lightsetup";
            this.saveFileDialog1.Filter = "Lighting Setup|*.lightsetup";
            this.saveFileDialog1.Title = "Save Lighting Setup";
            // 
            // lightPreview1
            // 
            this.lightPreview1.BackColor = System.Drawing.Color.Black;
            this.lightPreview1.EditingLight = null;
            this.lightPreview1.Lights = null;
            this.lightPreview1.Location = new System.Drawing.Point(240, 20);
            this.lightPreview1.Name = "lightPreview1";
            this.lightPreview1.PreviewMode = NuGenSVisualLib.UI.LightPreview.LightMode.PreviewLights;
            this.lightPreview1.Size = new System.Drawing.Size(128, 104);
            this.lightPreview1.TabIndex = 9;
            // 
            // LightingConfWin
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(696, 366);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LightingConfWin";
            this.Text = "Lighting Options";
            this.Load += new System.EventHandler(this.ShadingConf_Load);
            this.groupBox4.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			// apply setting to config
            currentLighting = editLighting;

			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			// add light
			int i=0;
            while (editLighting.lights[i] != null) i++;
            editLighting.lights[i] = new LightWrapper();

			// add to list
			listBox1.Items.Add("light" + i.ToString());

			lightPreview1.Refresh();
            changedEditing = true;
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			// remove light
			if (lightPropertyGrid.SelectedObject != null)
			{
                editLighting.lights[listBox1.SelectedIndex] = null;
				listBox1.Items.RemoveAt(listBox1.SelectedIndex);
				lightPropertyGrid.SelectedObject = null;

				lightPreview1.Refresh();
                changedEditing = true;
			}
		}

		private void ShadingConf_Load(object sender, System.EventArgs e)
		{
			lightPreview1.InitializeGraphics();
            lightPreview1.Lights = editLighting;
		}

		private void checkBox3_CheckedChanged(object sender, System.EventArgs e)
		{
			if (checkBox3.Checked)
			{
				lightPreview1.PreviewMode = LightPreview.LightMode.PreviewLights;
				groupBox7.Enabled = false;
			}
			else
			{
				lightPreview1.PreviewMode = LightPreview.LightMode.EditLight;
				if (listBox1.SelectedIndex != -1)
				{
					groupBox7.Enabled = true;
                    lightPreview1.EditingLight = editLighting.lights[listBox1.SelectedIndex];
				}
			}
			lightPreview1.DrawPreview();
		}

		private void lightPropertyGrid_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			// update preview
			lightPreview1.Refresh();

            // update sliders
            trackBar1.Value = (int)((editLighting.lights[listBox1.SelectedIndex].DirectionX + 1) * 100.0f);
            trackBar2.Value = (int)((editLighting.lights[listBox1.SelectedIndex].DirectionY + 1) * 100.0f);
            trackBar3.Value = (int)((editLighting.lights[listBox1.SelectedIndex].DirectionZ + 1) * 100.0f);

            changedEditing = true;
		}

		private void listBox1_SelectedValueChanged(object sender, System.EventArgs e)
		{
			if (listBox1.SelectedIndex != -1)
			{
                lightPropertyGrid.SelectedObject = editLighting.lights[listBox1.SelectedIndex];

                trackBar1.Value = (int)((editLighting.lights[listBox1.SelectedIndex].DirectionX + 1) * 100.0f);
                trackBar2.Value = (int)((editLighting.lights[listBox1.SelectedIndex].DirectionY + 1) * 100.0f);
                trackBar3.Value = (int)((editLighting.lights[listBox1.SelectedIndex].DirectionZ + 1) * 100.0f);

				if (lightPreview1.PreviewMode == LightPreview.LightMode.EditLight)
				{
					groupBox7.Enabled = true;
                    lightPreview1.EditingLight = editLighting.lights[listBox1.SelectedIndex];
					lightPreview1.DrawPreview();
				}
			}
			else
				groupBox7.Enabled = false;
		}

		private void trackBar1_ValueChanged(object sender, System.EventArgs e)
		{
            editLighting.lights[listBox1.SelectedIndex].DirectionX = ((float)trackBar1.Value / 100.0f) - 1;
			lightPreview1.DrawPreview();

			lightPropertyGrid.Refresh();
            changedEditing = true;
		}

		private void trackBar2_ValueChanged(object sender, System.EventArgs e)
		{
            editLighting.lights[listBox1.SelectedIndex].DirectionY = ((float)trackBar2.Value / 100.0f) - 1;
			lightPreview1.DrawPreview();
			
			lightPropertyGrid.Refresh();
            changedEditing = true;
		}

		private void trackBar3_ValueChanged(object sender, System.EventArgs e)
		{
            editLighting.lights[listBox1.SelectedIndex].DirectionZ = ((float)trackBar3.Value / 100.0f) - 1;
			lightPreview1.DrawPreview();

			lightPropertyGrid.Refresh();
            changedEditing = true;
		}

		private void checkBox7_CheckedChanged(object sender, System.EventArgs e)
		{
            editLighting.ambientLighting = checkBox7.Checked;
			lightPreview1.DrawPreview();
		}

        private void button5_Click(object sender, EventArgs e)
        {
            // load in lighting conf from file
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                LightingConf lighting = LightingConf.FromXmlFile(openFileDialog1.FileName, maxNumLights);
                if (lighting != null)
                {
                    // add to list
                    setupFilenames.Add(openFileDialog1.FileName);
                    setups.Add(lighting);
                    listBox2.Items.Add(Path.GetFileName(openFileDialog1.FileName));
                }
                else
                    MessageBox.Show(this, "Problem parsing lighting setup file", "Lighting Setup", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // load new setup into editor
            if (listBox2.SelectedIndex != -1)
            {
                if (changedEditing && MessageBox.Show(this, "Keep Changes to Setup?", "Setup Modified",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    // copy clone back over original setup
                    if (editIndex < 0)
                        currentLighting = editLighting;
                    else
                    {
                        setups[editIndex] = editLighting;
                        // save over existing setup file
                        editLighting.ToXmlFile((string)setupFilenames[editIndex]);
                    }
                }

                // copy clone into editor
                editIndex = listBox2.SelectedIndex - 1;
                if (editIndex < 0)
                    editLighting = (LightingConf)currentLighting.Clone();
                else
                    editLighting = (LightingConf)((LightingConf)setups[editIndex]).Clone();

                SetupLights();
                groupBox4.Text = "Lights Edit [" + (string)listBox2.SelectedItem + "]";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // save edit setup as new file
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                editLighting.ToXmlFile(saveFileDialog1.FileName);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            editLighting.specularLighting = checkBox1.Checked;
            lightPreview1.DrawPreview();
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            editLighting.rotate = checkBox9.Checked;
            lightPreview1.DrawPreview();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            editLighting.enabled = checkBox2.Checked;
            lightPreview1.DrawPreview();
        }

		protected override void OnClosed(EventArgs e)
		{
			lightPreview1.DisposeDevice();

			base.OnClosed (e);
		}
	}
}
