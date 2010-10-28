using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.CSharp;
using FPlotLibrary;

namespace FPlot
{
	/// <summary>
	/// Summary description for SourceForm.
	/// </summary>
	public class SourceForm : System.Windows.Forms.Form
	{
		private const string introText = "using System;\n...\ndouble[] p, dfdp;\n";
		private const string introCText = "using System; using System.Drawing;\n...\ndouble[] p, dfdp;\n";

		private const string introSource  = "namespace PlotApp{class PlotFunction:";
		private System.Windows.Forms.Label introLabel;
		private System.Windows.Forms.Label outroLabel;
		private System.Windows.Forms.Button compileButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.RadioButton oneDimension;
		private System.Windows.Forms.TextBox output;
		private System.Windows.Forms.Button helpButton;
		private System.Windows.Forms.RadioButton twoDimensions;
		private System.Windows.Forms.TextBox fSource;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.Button parButton;
		private GraphControl graph;
		private ParForm parForm;
		

		private System.Windows.Forms.RadioButton colorFunction;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox name;
		private System.Windows.Forms.Button deleteButton;
		
		public FunctionItem f, oldItem, paritem;

		MainForm mainform;
		bool deleted = false;
		private System.Windows.Forms.Label colorLabel;
		private System.Windows.Forms.Button colorButton;
		private Pen pen = new Pen(Color.Black, 2);
		private System.Windows.Forms.ComboBox lineStyle;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox lineWidth;
		private System.Windows.Forms.CheckBox rgb;

		static int index = 1;

		public SourceForm(GraphControl graph, FunctionItem old, MainForm mainform)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.graph = graph;
			parForm = new ParForm();
			paritem = new FunctionItem();
			oldItem = old;
			this.mainform = mainform;
			lineStyle.DropDownStyle = ComboBoxStyle.DropDownList;
			for (DashStyle s = DashStyle.Solid; s < DashStyle.Custom; s++) {
				lineStyle.Items.Add(s.ToString());
			}
			lineStyle.SelectedIndex = 0;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SourceForm));
			this.fSource = new System.Windows.Forms.TextBox();
			this.oneDimension = new System.Windows.Forms.RadioButton();
			this.twoDimensions = new System.Windows.Forms.RadioButton();
			this.introLabel = new System.Windows.Forms.Label();
			this.outroLabel = new System.Windows.Forms.Label();
			this.compileButton = new System.Windows.Forms.Button();
			this.output = new System.Windows.Forms.TextBox();
			this.okButton = new System.Windows.Forms.Button();
			this.applyButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.helpButton = new System.Windows.Forms.Button();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.parButton = new System.Windows.Forms.Button();
			this.colorFunction = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.name = new System.Windows.Forms.TextBox();
			this.deleteButton = new System.Windows.Forms.Button();
			this.colorLabel = new System.Windows.Forms.Label();
			this.colorButton = new System.Windows.Forms.Button();
			this.lineStyle = new System.Windows.Forms.ComboBox();
			this.lineWidth = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.rgb = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// fSource
			// 
			this.fSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.fSource.Font = new System.Drawing.Font("Courier New", 8.747663F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.fSource.Location = new System.Drawing.Point(16, 160);
			this.fSource.Multiline = true;
			this.fSource.Name = "fSource";
			this.fSource.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.fSource.Size = new System.Drawing.Size(552, 142);
			this.fSource.TabIndex = 0;
			this.fSource.Text = "";
			// 
			// oneDimension
			// 
			this.oneDimension.Checked = true;
			this.oneDimension.Location = new System.Drawing.Point(16, 72);
			this.oneDimension.Name = "oneDimension";
			this.oneDimension.TabIndex = 2;
			this.oneDimension.TabStop = true;
			this.oneDimension.Text = "One dimension";
			this.oneDimension.CheckedChanged += new System.EventHandler(this.dimensionChanged);
			// 
			// twoDimensions
			// 
			this.twoDimensions.Location = new System.Drawing.Point(144, 72);
			this.twoDimensions.Name = "twoDimensions";
			this.twoDimensions.TabIndex = 3;
			this.twoDimensions.Text = "Two dimensions";
			this.twoDimensions.CheckedChanged += new System.EventHandler(this.dimensionChanged);
			// 
			// introLabel
			// 
			this.introLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.introLabel.Font = new System.Drawing.Font("Courier New", 8.747663F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.introLabel.Location = new System.Drawing.Point(0, 96);
			this.introLabel.Name = "introLabel";
			this.introLabel.Size = new System.Drawing.Size(568, 64);
			this.introLabel.TabIndex = 5;
			// 
			// outroLabel
			// 
			this.outroLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.outroLabel.Font = new System.Drawing.Font("Courier New", 8.747663F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.outroLabel.Location = new System.Drawing.Point(0, 302);
			this.outroLabel.Name = "outroLabel";
			this.outroLabel.Size = new System.Drawing.Size(568, 16);
			this.outroLabel.TabIndex = 6;
			this.outroLabel.Text = "}";
			// 
			// compileButton
			// 
			this.compileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.compileButton.Location = new System.Drawing.Point(16, 328);
			this.compileButton.Name = "compileButton";
			this.compileButton.Size = new System.Drawing.Size(232, 23);
			this.compileButton.TabIndex = 7;
			this.compileButton.Text = "Compile";
			this.compileButton.Click += new System.EventHandler(this.compileClick);
			// 
			// output
			// 
			this.output.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.output.Location = new System.Drawing.Point(0, 360);
			this.output.Multiline = true;
			this.output.Name = "output";
			this.output.ReadOnly = true;
			this.output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.output.Size = new System.Drawing.Size(568, 64);
			this.output.TabIndex = 8;
			this.output.Text = "";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.okButton.Location = new System.Drawing.Point(0, 430);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(96, 24);
			this.okButton.TabIndex = 9;
			this.okButton.Text = "Ok";
			this.okButton.Click += new System.EventHandler(this.okClick);
			// 
			// applyButton
			// 
			this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.applyButton.Location = new System.Drawing.Point(112, 430);
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size(96, 24);
			this.applyButton.TabIndex = 10;
			this.applyButton.Text = "Apply";
			this.applyButton.Click += new System.EventHandler(this.applyClick);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancelButton.Location = new System.Drawing.Point(224, 430);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(96, 24);
			this.cancelButton.TabIndex = 11;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelClick);
			// 
			// helpButton
			// 
			this.helpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.helpButton.Location = new System.Drawing.Point(488, 430);
			this.helpButton.Name = "helpButton";
			this.helpButton.TabIndex = 14;
			this.helpButton.Text = "Help...";
			this.helpButton.Click += new System.EventHandler(this.helpClick);
			// 
			// parButton
			// 
			this.parButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.parButton.Location = new System.Drawing.Point(448, 128);
			this.parButton.Name = "parButton";
			this.parButton.Size = new System.Drawing.Size(104, 24);
			this.parButton.TabIndex = 18;
			this.parButton.Text = "Parameters (p)...";
			this.parButton.Click += new System.EventHandler(this.parClick);
			// 
			// colorFunction
			// 
			this.colorFunction.Location = new System.Drawing.Point(272, 72);
			this.colorFunction.Name = "colorFunction";
			this.colorFunction.TabIndex = 19;
			this.colorFunction.Text = "Color function";
			this.colorFunction.CheckedChanged += new System.EventHandler(this.dimensionChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 23);
			this.label1.TabIndex = 20;
			this.label1.Text = "Name:";
			// 
			// name
			// 
			this.name.Location = new System.Drawing.Point(48, 8);
			this.name.Name = "name";
			this.name.Size = new System.Drawing.Size(312, 20);
			this.name.TabIndex = 21;
			this.name.Text = "Function1";
			// 
			// deleteButton
			// 
			this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.deleteButton.Location = new System.Drawing.Point(336, 430);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(96, 24);
			this.deleteButton.TabIndex = 22;
			this.deleteButton.Text = "Delete";
			this.deleteButton.Click += new System.EventHandler(this.deleteClick);
			// 
			// colorLabel
			// 
			this.colorLabel.Location = new System.Drawing.Point(72, 40);
			this.colorLabel.Name = "colorLabel";
			this.colorLabel.Size = new System.Drawing.Size(24, 24);
			this.colorLabel.TabIndex = 33;
			// 
			// colorButton
			// 
			this.colorButton.Location = new System.Drawing.Point(8, 40);
			this.colorButton.Name = "colorButton";
			this.colorButton.Size = new System.Drawing.Size(56, 24);
			this.colorButton.TabIndex = 32;
			this.colorButton.Text = "Color...";
			this.colorButton.Click += new System.EventHandler(this.colorClick);
			// 
			// lineStyle
			// 
			this.lineStyle.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lineStyle.Location = new System.Drawing.Point(160, 40);
			this.lineStyle.Name = "lineStyle";
			this.lineStyle.Size = new System.Drawing.Size(80, 21);
			this.lineStyle.TabIndex = 34;
			this.lineStyle.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.DrawItem);
			// 
			// lineWidth
			// 
			this.lineWidth.Location = new System.Drawing.Point(320, 40);
			this.lineWidth.Name = "lineWidth";
			this.lineWidth.Size = new System.Drawing.Size(40, 20);
			this.lineWidth.TabIndex = 35;
			this.lineWidth.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(256, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 23);
			this.label2.TabIndex = 36;
			this.label2.Text = "Line Width:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(104, 40);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 23);
			this.label3.TabIndex = 37;
			this.label3.Text = "Line Style:";
			// 
			// rgb
			// 
			this.rgb.Location = new System.Drawing.Point(400, 40);
			this.rgb.Name = "rgb";
			this.rgb.Size = new System.Drawing.Size(56, 24);
			this.rgb.TabIndex = 38;
			this.rgb.Text = "RGB";
			this.rgb.CheckedChanged += new System.EventHandler(this.dimensionChanged);
			// 
			// SourceForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(568, 459);
			this.Controls.Add(this.parButton);
			this.Controls.Add(this.rgb);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lineWidth);
			this.Controls.Add(this.name);
			this.Controls.Add(this.output);
			this.Controls.Add(this.fSource);
			this.Controls.Add(this.lineStyle);
			this.Controls.Add(this.colorLabel);
			this.Controls.Add(this.colorButton);
			this.Controls.Add(this.deleteButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.colorFunction);
			this.Controls.Add(this.helpButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.applyButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.compileButton);
			this.Controls.Add(this.outroLabel);
			this.Controls.Add(this.introLabel);
			this.Controls.Add(this.twoDimensions);
			this.Controls.Add(this.oneDimension);
			this.Controls.Add(this.label3);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(544, 416);
			this.Name = "SourceForm";
			this.ShowInTaskbar = false;
			this.Text = "Edit function";
			this.ResumeLayout(false);

		}
		#endregion

		private void CreateF() {
			if (oneDimension.Checked) { f = new Function1D(graph.Model); }
			else if (twoDimensions.Checked) {
				f = new Function2D(graph.Model);
				((Function2D)f).rgb = rgb.Checked;
			}	else { f = new FunctionColor(graph.Model); }
			f.name = name.Text;
			f.source = fSource.Text;
			f.Color = colorLabel.BackColor;
			f.lineStyle = (DashStyle)(lineStyle.SelectedIndex);
			try {
				f.lineWidth = float.Parse(lineWidth.Text);
			} catch {f.lineWidth = 1;}
			f.p = paritem.p;
		}

		private void compileClick(object sender, System.EventArgs e) {
			CreateF();
			f.Compile(false);
			output.Lines = f.errors;
		}

		public void Reset() {
			if (oldItem == null) {
				oneDimension.Checked = true;
				name.Text = "Function" + index++;
				fSource.Text = "return 0;";
				colorLabel.BackColor = Color.Black;
				colorDialog.Color = Color.Black;
				lineStyle.SelectedIndex = 0;
				lineWidth.Text = "1";
				paritem.p.Length = 0;
				lineStyle.Enabled = lineWidth.Enabled = true;
				rgb.Checked = true;
				rgb.Enabled = false;
			} else {
				rgb.Checked = true;
				if (oldItem is Function1D) {
					oneDimension.Checked = true;
				} else if (oldItem is Function2D) {
					twoDimensions.Checked = true;
					rgb.Checked = ((Function2D)oldItem).rgb;
				} else if (oldItem is FunctionColor) {
					colorFunction.Checked = true;
				} else throw new System.Exception("invalid item-type");
				name.Text = oldItem.name;
				fSource.Text = oldItem.source;
				colorLabel.BackColor = oldItem.Color;
				colorDialog.Color = oldItem.Color;
				paritem.p = oldItem.p.Clone();
				lineStyle.SelectedIndex = (int)oldItem.lineStyle;
				lineWidth.Text = oldItem.lineWidth.ToString();
				lineStyle.Enabled = lineWidth.Enabled = oneDimension.Checked;
				rgb.Enabled = twoDimensions.Checked;
				colorButton.Enabled = !(twoDimensions.Checked && rgb.Checked);
				
			}

			dimensionChanged(null, null);
		}

		private void Apply()
		{
			if (!deleted) {
				CreateF();
				f.Compile(true);
				output.Lines = f.errors;

				if (oldItem == null) graph.Model.Items.Add(f);
				else {
					int index = graph.Model.Items.IndexOf(oldItem);
					graph.Model.Items.Remove(oldItem);
					graph.Model.Items.Insert(index, f);
				}

				oldItem = f;

				graph.Invalidate();
				mainform.ResetMenu();
			}
		}

		private void cancelClick(object sender, System.EventArgs e)
		{
			oldItem = null;
			this.Hide();
		}

		private void applyClick(object sender, System.EventArgs e)
		{
			Apply();
			Reset();
		}

		private void okClick(object sender, System.EventArgs e)
		{
			Apply();
			oldItem = null;
			this.Hide();
		}

		private void dimensionChanged(object sender, System.EventArgs e)
		{
			if (oneDimension.Checked) introLabel.Text = introText + "double f(double x) {";
			if (twoDimensions.Checked) introLabel.Text = introText + "double f(double x, double y) {";
			if (colorFunction.Checked) introLabel.Text = introCText + "Color f(double x, double y) {";
			lineStyle.Enabled = lineWidth.Enabled = oneDimension.Checked;
			rgb.Enabled = twoDimensions.Checked;
			colorButton.Enabled = !twoDimensions.Checked || !rgb.Checked;
		}

		private void colorClick(object sender, System.EventArgs e) {
			colorDialog.ShowDialog();
			colorLabel.BackColor = colorDialog.Color;
			lineStyle.Invalidate();
		}

		private void parClick(object sender, System.EventArgs e) {
			CreateF();
			f.Compile(true);
			output.Lines = f.errors;
			if (parForm.IsDisposed) parForm = new ParForm();
			parForm.Reset(paritem);
			parForm.Show();
			parForm.BringToFront();
		}

		private void deleteClick(object sender, System.EventArgs e) {
			deleted = true;
			if (oldItem != null) {
				graph.Model.Items.Remove(oldItem);
				graph.Invalidate();
				mainform.ResetMenu();
			}
		}

		private void DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e) {
			e.DrawBackground();
			pen.Color = colorLabel.BackColor;
			Graphics g = e.Graphics;
		string style = (string)lineStyle.Items[e.Index];
			for (DashStyle s = DashStyle.Solid; s < DashStyle.Custom; s++) {
				if (style == s.ToString()) pen.DashStyle = s;
			}
			g.DrawLine(pen, e.Bounds.X, e.Bounds.Y + e.Bounds.Height/2, e.Bounds.X + e.Bounds.Width,
				e.Bounds.Y + e.Bounds.Height/2);
			e.DrawFocusRectangle();
		}

		private void helpClick(object sender, System.EventArgs e) {
			Help.ShowHelp(this, "../help/FPlot.chm", "FunctionForm.html");
		}

	}
}
