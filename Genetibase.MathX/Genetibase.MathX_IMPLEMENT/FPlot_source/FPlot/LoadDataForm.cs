using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FPlotLibrary;

namespace FPlot
{
	/// <summary>
	/// Summary description for LoadDataForm.
	/// </summary>
	public class LoadDataForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox source;
		private System.Windows.Forms.Label intro;
		private System.Windows.Forms.Button loadButton;
		private System.Windows.Forms.Label outro;
		private System.Windows.Forms.Button okButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private DataItem item;
		private System.Windows.Forms.Button help;
		private System.Windows.Forms.Label length;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox separators;
		private System.Windows.Forms.RadioButton floatbutton;
		private System.Windows.Forms.RadioButton doublebutton;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage asciiPage;
		private System.Windows.Forms.TabPage binaryPage;
		private System.Windows.Forms.RadioButton sbytebutton;
		private System.Windows.Forms.RadioButton int16button;
		private System.Windows.Forms.RadioButton int32button;
		private System.Windows.Forms.RadioButton int64button;
		private System.Windows.Forms.RadioButton uint64button;
		private System.Windows.Forms.RadioButton uint32button;
		private System.Windows.Forms.RadioButton uint16button;
		private System.Windows.Forms.RadioButton bytebutton;
		private System.Windows.Forms.RadioButton bigendian;
		private System.Windows.Forms.RadioButton littleendian;
		private System.Windows.Forms.GroupBox byteordering;
		DataForm data;

		public LoadDataForm(DataForm data, DataItem item)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.item = item;
			this.data = data;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LoadDataForm));
			this.source = new System.Windows.Forms.TextBox();
			this.intro = new System.Windows.Forms.Label();
			this.loadButton = new System.Windows.Forms.Button();
			this.outro = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.help = new System.Windows.Forms.Button();
			this.length = new System.Windows.Forms.Label();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.asciiPage = new System.Windows.Forms.TabPage();
			this.separators = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.binaryPage = new System.Windows.Forms.TabPage();
			this.uint64button = new System.Windows.Forms.RadioButton();
			this.uint32button = new System.Windows.Forms.RadioButton();
			this.uint16button = new System.Windows.Forms.RadioButton();
			this.bytebutton = new System.Windows.Forms.RadioButton();
			this.int64button = new System.Windows.Forms.RadioButton();
			this.int32button = new System.Windows.Forms.RadioButton();
			this.int16button = new System.Windows.Forms.RadioButton();
			this.doublebutton = new System.Windows.Forms.RadioButton();
			this.floatbutton = new System.Windows.Forms.RadioButton();
			this.sbytebutton = new System.Windows.Forms.RadioButton();
			this.byteordering = new System.Windows.Forms.GroupBox();
			this.bigendian = new System.Windows.Forms.RadioButton();
			this.littleendian = new System.Windows.Forms.RadioButton();
			this.tabControl.SuspendLayout();
			this.asciiPage.SuspendLayout();
			this.binaryPage.SuspendLayout();
			this.byteordering.SuspendLayout();
			this.SuspendLayout();
			// 
			// source
			// 
			this.source.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.source.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.source.Location = new System.Drawing.Point(16, 224);
			this.source.Multiline = true;
			this.source.Name = "source";
			this.source.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.source.Size = new System.Drawing.Size(448, 264);
			this.source.TabIndex = 2;
			this.source.Text = "";
			this.source.WordWrap = false;
			// 
			// intro
			// 
			this.intro.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.intro.Location = new System.Drawing.Point(0, 152);
			this.intro.Name = "intro";
			this.intro.Size = new System.Drawing.Size(352, 80);
			this.intro.TabIndex = 3;
			this.intro.Text = "using System; using System.IO;\n...\nint Length;\ndouble[] x, y, dx, dy\npublic void " +
				"Load(Stream stream) {";
			// 
			// loadButton
			// 
			this.loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.loadButton.Location = new System.Drawing.Point(0, 521);
			this.loadButton.Name = "loadButton";
			this.loadButton.Size = new System.Drawing.Size(72, 24);
			this.loadButton.TabIndex = 4;
			this.loadButton.Text = "Load file...";
			this.loadButton.Click += new System.EventHandler(this.fileClick);
			// 
			// outro
			// 
			this.outro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.outro.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.outro.Location = new System.Drawing.Point(0, 489);
			this.outro.Name = "outro";
			this.outro.Size = new System.Drawing.Size(104, 24);
			this.outro.TabIndex = 7;
			this.outro.Text = "}";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.okButton.Location = new System.Drawing.Point(88, 521);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 8;
			this.okButton.Text = "Ok";
			this.okButton.Click += new System.EventHandler(this.okClick);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "All files (*.*)|*.*";
			// 
			// help
			// 
			this.help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.help.Location = new System.Drawing.Point(376, 521);
			this.help.Name = "help";
			this.help.TabIndex = 9;
			this.help.Text = "Help...";
			this.help.Click += new System.EventHandler(this.helpClick);
			// 
			// length
			// 
			this.length.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.length.Location = new System.Drawing.Point(328, 200);
			this.length.Name = "length";
			this.length.Size = new System.Drawing.Size(128, 16);
			this.length.TabIndex = 6;
			this.length.Text = "Length:";
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.asciiPage);
			this.tabControl.Controls.Add(this.binaryPage);
			this.tabControl.Location = new System.Drawing.Point(8, 8);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(400, 136);
			this.tabControl.TabIndex = 10;
			this.tabControl.Click += new System.EventHandler(this.SetText);
			// 
			// asciiPage
			// 
			this.asciiPage.Controls.Add(this.separators);
			this.asciiPage.Controls.Add(this.label1);
			this.asciiPage.Location = new System.Drawing.Point(4, 22);
			this.asciiPage.Name = "asciiPage";
			this.asciiPage.Size = new System.Drawing.Size(392, 110);
			this.asciiPage.TabIndex = 0;
			this.asciiPage.Text = "ASCII";
			// 
			// separators
			// 
			this.separators.Location = new System.Drawing.Point(96, 8);
			this.separators.Name = "separators";
			this.separators.Size = new System.Drawing.Size(72, 20);
			this.separators.TabIndex = 1;
			this.separators.Text = ",;:|";
			this.separators.TextChanged += new System.EventHandler(this.SetText);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.TabIndex = 0;
			this.label1.Text = "Separator chars:";
			// 
			// binaryPage
			// 
			this.binaryPage.Controls.Add(this.byteordering);
			this.binaryPage.Controls.Add(this.uint64button);
			this.binaryPage.Controls.Add(this.uint32button);
			this.binaryPage.Controls.Add(this.uint16button);
			this.binaryPage.Controls.Add(this.bytebutton);
			this.binaryPage.Controls.Add(this.int64button);
			this.binaryPage.Controls.Add(this.int32button);
			this.binaryPage.Controls.Add(this.int16button);
			this.binaryPage.Controls.Add(this.doublebutton);
			this.binaryPage.Controls.Add(this.floatbutton);
			this.binaryPage.Controls.Add(this.sbytebutton);
			this.binaryPage.Location = new System.Drawing.Point(4, 22);
			this.binaryPage.Name = "binaryPage";
			this.binaryPage.Size = new System.Drawing.Size(392, 110);
			this.binaryPage.TabIndex = 1;
			this.binaryPage.Text = "Binary";
			// 
			// uint64button
			// 
			this.uint64button.Location = new System.Drawing.Point(88, 80);
			this.uint64button.Name = "uint64button";
			this.uint64button.Size = new System.Drawing.Size(64, 24);
			this.uint64button.TabIndex = 20;
			this.uint64button.Text = "UInt64";
			this.uint64button.CheckedChanged += new System.EventHandler(this.SetText);
			// 
			// uint32button
			// 
			this.uint32button.Location = new System.Drawing.Point(88, 56);
			this.uint32button.Name = "uint32button";
			this.uint32button.Size = new System.Drawing.Size(64, 24);
			this.uint32button.TabIndex = 19;
			this.uint32button.Text = "UInt32";
			this.uint32button.CheckedChanged += new System.EventHandler(this.SetText);
			// 
			// uint16button
			// 
			this.uint16button.Checked = true;
			this.uint16button.Location = new System.Drawing.Point(88, 32);
			this.uint16button.Name = "uint16button";
			this.uint16button.Size = new System.Drawing.Size(64, 24);
			this.uint16button.TabIndex = 18;
			this.uint16button.TabStop = true;
			this.uint16button.Text = "UInt16";
			this.uint16button.CheckedChanged += new System.EventHandler(this.SetText);
			// 
			// bytebutton
			// 
			this.bytebutton.Location = new System.Drawing.Point(88, 8);
			this.bytebutton.Name = "bytebutton";
			this.bytebutton.Size = new System.Drawing.Size(64, 24);
			this.bytebutton.TabIndex = 17;
			this.bytebutton.Text = "Byte";
			this.bytebutton.CheckedChanged += new System.EventHandler(this.SetText);
			// 
			// int64button
			// 
			this.int64button.Location = new System.Drawing.Point(8, 80);
			this.int64button.Name = "int64button";
			this.int64button.Size = new System.Drawing.Size(64, 24);
			this.int64button.TabIndex = 16;
			this.int64button.Text = "Int64";
			this.int64button.CheckedChanged += new System.EventHandler(this.SetText);
			// 
			// int32button
			// 
			this.int32button.Location = new System.Drawing.Point(8, 56);
			this.int32button.Name = "int32button";
			this.int32button.Size = new System.Drawing.Size(64, 24);
			this.int32button.TabIndex = 15;
			this.int32button.Text = "Int32";
			this.int32button.CheckedChanged += new System.EventHandler(this.SetText);
			// 
			// int16button
			// 
			this.int16button.Location = new System.Drawing.Point(8, 32);
			this.int16button.Name = "int16button";
			this.int16button.Size = new System.Drawing.Size(64, 24);
			this.int16button.TabIndex = 14;
			this.int16button.Text = "Int16";
			this.int16button.CheckedChanged += new System.EventHandler(this.SetText);
			// 
			// doublebutton
			// 
			this.doublebutton.Location = new System.Drawing.Point(168, 8);
			this.doublebutton.Name = "doublebutton";
			this.doublebutton.Size = new System.Drawing.Size(56, 24);
			this.doublebutton.TabIndex = 13;
			this.doublebutton.Text = "double";
			this.doublebutton.CheckedChanged += new System.EventHandler(this.SetText);
			// 
			// floatbutton
			// 
			this.floatbutton.Location = new System.Drawing.Point(168, 32);
			this.floatbutton.Name = "floatbutton";
			this.floatbutton.Size = new System.Drawing.Size(56, 24);
			this.floatbutton.TabIndex = 12;
			this.floatbutton.Text = "float";
			this.floatbutton.CheckedChanged += new System.EventHandler(this.SetText);
			// 
			// sbytebutton
			// 
			this.sbytebutton.Location = new System.Drawing.Point(8, 8);
			this.sbytebutton.Name = "sbytebutton";
			this.sbytebutton.Size = new System.Drawing.Size(64, 24);
			this.sbytebutton.TabIndex = 8;
			this.sbytebutton.Text = "SByte";
			this.sbytebutton.CheckedChanged += new System.EventHandler(this.SetText);
			// 
			// byteordering
			// 
			this.byteordering.Controls.Add(this.littleendian);
			this.byteordering.Controls.Add(this.bigendian);
			this.byteordering.Location = new System.Drawing.Point(248, 16);
			this.byteordering.Name = "byteordering";
			this.byteordering.Size = new System.Drawing.Size(120, 80);
			this.byteordering.TabIndex = 21;
			this.byteordering.TabStop = false;
			this.byteordering.Text = "Byte ordering";
			// 
			// bigendian
			// 
			this.bigendian.Location = new System.Drawing.Point(8, 16);
			this.bigendian.Name = "bigendian";
			this.bigendian.TabIndex = 0;
			this.bigendian.Text = "Big Endian";
			this.bigendian.CheckedChanged += new System.EventHandler(this.SetText);
			// 
			// littleendian
			// 
			this.littleendian.Checked = true;
			this.littleendian.Location = new System.Drawing.Point(8, 40);
			this.littleendian.Name = "littleendian";
			this.littleendian.TabIndex = 1;
			this.littleendian.TabStop = true;
			this.littleendian.Text = "Little Endian";
			this.littleendian.CheckedChanged += new System.EventHandler(this.SetText);
			// 
			// LoadDataForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(464, 550);
			this.Controls.Add(this.length);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.source);
			this.Controls.Add(this.help);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.outro);
			this.Controls.Add(this.loadButton);
			this.Controls.Add(this.intro);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(424, 384);
			this.Name = "LoadDataForm";
			this.ShowInTaskbar = false;
			this.Text = "LoadDataForm";
			this.tabControl.ResumeLayout(false);
			this.asciiPage.ResumeLayout(false);
			this.binaryPage.ResumeLayout(false);
			this.byteordering.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public void Reset() {
			source.Text = item.loadsource;
			length.Text = "Length: " + item.Length;
		}

		public void Apply() {
			item.loadsource = source.Text;
			item.Compile(true);
			if (!item.compiled) {
				string text = "Compile Errors:\n";
				for (int i = 0; i < item.errors.Length; i++) {
					text += item.errors[i] + "\n";
				}
				MessageBox.Show(text);
			}
		}

		private string ReadText() {
			if (sbytebutton.Checked) return "(double)r.ReadSByte()";
			else if (int16button.Checked) return "(double)r.ReadInt16()";
			else if (int32button.Checked) return "(double)r.ReadInt32()";
			else if (int64button.Checked) return "(double)r.ReadInt64()";
			else if (bytebutton.Checked) return "(double)r.ReadByte()";
			else if (uint16button.Checked) return "(double)r.ReadUInt16()";
			else if (uint32button.Checked) return "(double)r.ReadUInt32()";
			else if (uint64button.Checked) return "(double)r.ReadUInt64()";
			else if (floatbutton.Checked) return "(double)r.ReadSingle()";
			else if (doublebutton.Checked) return "(double)r.ReadDouble()";
			else return "";
		}

		private void ReadTextBig(string var, string[] lines, ref int n) {
			if (sbytebutton.Checked) lines[n++] = "        " + var + "[n] = (double)r.ReadSByte();";
			else if (int16button.Checked) {
				lines[n++] = "        byte[] b; byte t;";
				lines[n++] = "        b = BitConverter.GetBytes(r.ReadInt16());";
				lines[n++] = "        t = b[0]; b[0] = b[1]; b[1] = t; //Invert byte ordering";
				lines[n++] = "        " + var + "[n] = (double)BitConverter.ToInt16(b, 0);";
			} else if (int32button.Checked) {
				lines[n++] = "        byte[] b; byte t;";
				lines[n++] = "        b = BitConverter.GetBytes(r.ReadInt32());";
				lines[n++] = "        for (int i = 0; i < b.Length/2; i++) { //Invert byte ordering";
				lines[n++] = "          t = b[i]; b[i] = b[b.Length-i-1]; b[b.Length-i-1] = t;";
				lines[n++] = "        }";
				lines[n++] = "        " + var + "[n] = (double)BitConverter.ToInt32(b, 0);";
			} else if (int64button.Checked) {
				lines[n++] = "        byte[] b; byte t;";
				lines[n++] = "        b = BitConverter.GetBytes(r.ReadInt64());";
				lines[n++] = "        for (int i = 0; i < b.Length/2; i++) { //Invert byte ordering";
				lines[n++] = "          t = b[i]; b[i] = b[b.Length-i-1]; b[b.Length-i-1] = t;";
				lines[n++] = "        }";
				lines[n++] = "        " + var + "[n] = (double)BitConverter.ToInt64(b, 0);";
			} else if (bytebutton.Checked) lines[n++] = "        " + var + "[n] = (double)r.ReadByte();";
			else if (uint16button.Checked) {
				lines[n++] = "        byte[] b; byte t;";
				lines[n++] = "        b = BitConverter.GetBytes(r.ReadUInt16());";
				lines[n++] = "        t = b[0]; b[0] = b[1]; b[1] = t; //Invert byte ordering";
				lines[n++] = "        " + var + "[n] = (double)BitConverter.ToUInt16(b, 0);";
			} else if (uint32button.Checked) {
				lines[n++] = "        byte[] b; byte t;";
				lines[n++] = "        b = BitConverter.GetBytes(r.ReadUInt32());";
				lines[n++] = "        for (int i = 0; i < b.Length/2; i++) { //Invert byte ordering";
				lines[n++] = "          t = b[i]; b[i] = b[b.Length-i-1]; b[b.Length-i-1] = t;";
				lines[n++] = "        }";
				lines[n++] = "        " + var + "[n] = (double)BitConverter.ToUInt32(b, 0);";
			} else if (uint64button.Checked) {
				lines[n++] = "        byte[] b; byte t;";
				lines[n++] = "        b = BitConverter.GetBytes(r.ReadUInt64());";
				lines[n++] = "        for (int i = 0; i < b.Length/2; i++) { //Invert byte ordering";
				lines[n++] = "          t = b[i]; b[i] = b[b.Length-i-1]; b[b.Length-i-1] = t;";
				lines[n++] = "        }";
				lines[n++] = "        " + var + "[n] = (double)BitConverter.ToUInt64(b, 0);";
			} else if (floatbutton.Checked) lines[n++] = "        " + var + "[n] = (double)r.ReadSingle();";
			else if (doublebutton.Checked) lines[n++] = "        " + var + "[n] = r.ReadDouble();";
		}

		public void SetText(object sender, System.EventArgs e) {
			byteordering.Enabled = littleendian.Enabled = bigendian.Enabled =
				(!sbytebutton.Checked && !bytebutton.Checked && !floatbutton.Checked && !doublebutton.Checked);
			string[] lines;
			if (tabControl.SelectedTab == asciiPage) {
				lines = new string[25];
				int n = 0;
				lines[n++]  = "using (StreamReader r = new StreamReader(stream)) {";
				lines[n++]  = "  int n = 0; string[] tokens; string line;";
				lines[n++]  = "  char[] separators = \"" + separators.Text + "\".ToCharArray();";
				lines[n++]  = "  while ((line = r.ReadLine()) != null) {";
				lines[n++]  = "    tokens = line.Split(separators);";
				lines[n++]  = "    int m = 0;";
				lines[n++]  = "    if (x.source == null) { // no formula entered for x";
				lines[n++]  = "      try {x[n] = double.Parse(tokens[m++]);}";
				lines[n++]  = "      catch {x[n] = 0;}";
				lines[n++]  = "    }";
				lines[n++] = "    if (y.source == null) { // no formula entered for y";
				lines[n++] = "      try {y[n] = double.Parse(tokens[m++]);}";
				lines[n++] = "      catch {y[n] = 0;}";
				lines[n++] = "    }";
				lines[n++] = "    if (dx.source == null) { // no formula entered for dx";
				lines[n++] = "      try {dx[n] = double.Parse(tokens[m++]);}";
				lines[n++] = "      catch {dx[n] = 0;}";
				lines[n++] = "    }";
				lines[n++] = "    if (dy.source == null) { // no formula entered for dy";
				lines[n++] = "      try {dy[n] = double.Parse(tokens[m++]);}";
				lines[n++] = "      catch {dy[n] = 0;}";
				lines[n++] = "    }";
				lines[n++] = "    n++;";
				lines[n++] = "  }";
				lines[n++] = "}";
				source.Lines = lines;
			} else if (littleendian.Checked) {
				lines = new string[26];
				int n = 0;
				lines[n++] = "using (BinaryReader r = new BinaryReader(stream)) {";
				lines[n++] = "  int n = 0;";
				lines[n++] = "  while(true) {";
				lines[n++] = "    try {";
				lines[n++] = "      if (x.source == null) { // no formula entered for x";
				lines[n++] = "        x[n] = " + ReadText() + ";";
				lines[n++] = "      }";
				lines[n++] = "    } catch {break;}";
				lines[n++] = "    try {";
				lines[n++] = "      if (y.source == null) { // no formula entered for y";
				lines[n++] = "        y[n] = " + ReadText() + ";";
				lines[n++] = "      }";
				lines[n++] = "    } catch {break;}";
				lines[n++] = "    try {";
				lines[n++] = "      if (dx.source == null) { // no formula entered for dx";
				lines[n++] = "        dx[n] = " + ReadText() + ";";
				lines[n++] = "      }";
				lines[n++] = "    } catch {break;}";
				lines[n++] = "    try {";
				lines[n++] = "      if (dy.source == null) { // no formula entered for dy";
				lines[n++] = "        dy[n] = " + ReadText() + ";";
				lines[n++] = "      }";
				lines[n++] = "    } catch {break;}";
				lines[n++] = "    n++;";
				lines[n++] = "  }";
				lines[n++] = "}";
				source.Lines = lines;
			} else {
				lines = new string[50];
				int n = 0;
				lines[n++] = "using (BinaryReader r = new BinaryReader(stream)) {";
				lines[n++] = "  int n = 0;";
				lines[n++] = "  while(true) {";
				lines[n++] = "    try {";
				lines[n++] = "      if (x.source == null) { // no formula entered for x";
				ReadTextBig("x", lines, ref n);
				lines[n++] = "      }";
				lines[n++] = "    } catch {break;}";
				lines[n++] = "    try {";
				lines[n++] = "      if (y.source == null) { // no formula entered for y";
				ReadTextBig("y", lines, ref n);
				lines[n++] = "      }";
				lines[n++] = "    } catch {break;}";
				lines[n++] = "    try {";
				lines[n++] = "      if (dx.source == null) { // no formula entered for dx";
				ReadTextBig("dx", lines, ref n);
				lines[n++] = "      }";
				lines[n++] = "    } catch {break;}";
				lines[n++] = "    try {";
				lines[n++] = "      if (dy.source == null) { // no formula entered for dy";
				ReadTextBig("dy", lines, ref n);
				lines[n++] = "      }";
				lines[n++] = "    } catch {break;}";
				lines[n++] = "    n++;";
				lines[n++] = "  }";
				lines[n++] = "}";
				while (n < 50) lines[n++] = "";
				source.Lines = lines;
			}
		}


		private void fileClick(object sender, System.EventArgs e) {
			Apply();
			if (item.compiled) {
				DialogResult res = openFileDialog.ShowDialog();
				if (res == DialogResult.OK) {
					item.LoadFromFile(openFileDialog.FileName);
					Reset();
				}
			}
			data.Reset();
		}

		private void okClick(object sender, System.EventArgs e) {
			Apply();
			this.Hide();
		}

		private void helpClick(object sender, System.EventArgs e) {
			Help.ShowHelp(this, "../help/FPlot.chm", "LoadForm.html");
		}

		private void SetText() {
		
		}

	}
}
