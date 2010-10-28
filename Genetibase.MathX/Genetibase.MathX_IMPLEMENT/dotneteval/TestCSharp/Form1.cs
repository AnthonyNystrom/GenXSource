using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Eval3;

namespace TestCSharp
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form 
	{
    
		// Required by the Windows Form Designer
		private Evaluator ev;
		private System.ComponentModel.IContainer components=null;
		private bool mInitializing;
		private Eval3.opCode mFormula3;
		public event Eval3.iEvalValue.ValueChangedEventHandler ValueChanged;
		
		// Note that these 3 variables are visible from within the evaluator 
		// without needing any assessor 
		public Eval3.EvalVariable A; 
		public Eval3.EvalVariable B;
		public Eval3.EvalVariable C;
		public double[] arr = {1.2, 3.4, 5.6, 7.8};

		// NOTE: The following procedure is required by the Windows Form Designer
		// It can be modified using the Windows Form Designer.  
		// Do not modify it using the code editor.
		internal System.Windows.Forms.TextBox TextBox2;
		internal System.Windows.Forms.ComboBox cbSamples;
		internal System.Windows.Forms.TextBox tbExpression;
		internal System.Windows.Forms.Button btnEvaluate;
		internal System.Windows.Forms.TabControl TabControl1;
		internal System.Windows.Forms.TabPage TabPage1;
		internal System.Windows.Forms.TabPage TabPage2;
		internal System.Windows.Forms.ComboBox ComboBox1;
		internal System.Windows.Forms.Button btnEvaluate2;
		internal System.Windows.Forms.PictureBox PictureBox1;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.TextBox tbExpressionRed;
		internal System.Windows.Forms.TextBox tbExpressionGreen;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.TextBox tbExpressionBlue;
		internal System.Windows.Forms.Label Label4;
		internal System.Windows.Forms.CheckBox cbAuto;
    
		public double X;
    
		public double Y;
		private System.Windows.Forms.TabPage tabPage3;
		internal System.Windows.Forms.Button btnEvaluate3;
		internal System.Windows.Forms.TextBox LogBox3;
		internal System.Windows.Forms.Label Label5;
		internal System.Windows.Forms.NumericUpDown updownA;
		internal System.Windows.Forms.TextBox tbExpression3;
		internal System.Windows.Forms.NumericUpDown updownB;
		internal System.Windows.Forms.NumericUpDown updownC;
		internal System.Windows.Forms.Label Label6;
		internal System.Windows.Forms.Label Label7;
		internal System.Windows.Forms.Label Label12;
		internal System.Windows.Forms.Label Label8;
		internal System.Windows.Forms.Label lblResults3;
    
		public Form1() 
		{
			mInitializing = true;
			ev = new Evaluator(Eval3.eParserSyntax.cSharp,false);
			ev.AddEnvironmentFunctions(this);
			ev.AddEnvironmentFunctions(new EvalFunctions());
			// This call is required by the Windows Form Designer.


			A = new Eval3.EvalVariable(0.0, typeof(double));
			B = new Eval3.EvalVariable(0.0, typeof(double));
			C = new Eval3.EvalVariable(0.0, typeof(double));

			// This call is required by the Windows Form Designer.
			InitializeComponent();

			A.Value = (double)updownA.Value;
			B.Value = (double)updownB.Value;
			C.Value = (double)updownC.Value;

			// Add any initialization after the InitializeComponent() call
			mInitializing = false;
			btnEvaluate_Click(null, null);
			btnEvaluate2_Click(null, null);
			btnEvaluate3_Click(null, null);
		}
    
		public string Description 
		{
			get 
			{
				return "This is form1";
			}
		}
    
		public EvalType EvalType 
		{
			get 
			{
				return EvalType.Object;
			}
		}
    
		public string Name1 
		{
			get 
			{
				return this.Name;
			}
		}
    
		public System.Type SystemType 
		{
			get 
			{
				return this.GetType();
			}
		}
    
		public object Value 
		{
			get 
			{
				return this;
			}
		}
    
		public Form1 me 
		{
			get 
			{
				return this;
			}
		}
    
		public Form1 theForm 
		{
			get 
			{
				return this;
			}
		}
    
    
		// Form overrides dispose to clean up the component list.
		protected override void Dispose(bool disposing) 
		{
			if (disposing) 
			{
				if (!(components == null)) 
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
    
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent() 
		{
			this.tbExpression = new System.Windows.Forms.TextBox();
			this.TextBox2 = new System.Windows.Forms.TextBox();
			this.btnEvaluate = new System.Windows.Forms.Button();
			this.cbSamples = new System.Windows.Forms.ComboBox();
			this.TabControl1 = new System.Windows.Forms.TabControl();
			this.TabPage1 = new System.Windows.Forms.TabPage();
			this.TabPage2 = new System.Windows.Forms.TabPage();
			this.cbAuto = new System.Windows.Forms.CheckBox();
			this.Label1 = new System.Windows.Forms.Label();
			this.PictureBox1 = new System.Windows.Forms.PictureBox();
			this.tbExpressionRed = new System.Windows.Forms.TextBox();
			this.ComboBox1 = new System.Windows.Forms.ComboBox();
			this.btnEvaluate2 = new System.Windows.Forms.Button();
			this.tbExpressionGreen = new System.Windows.Forms.TextBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.Label3 = new System.Windows.Forms.Label();
			this.tbExpressionBlue = new System.Windows.Forms.TextBox();
			this.Label4 = new System.Windows.Forms.Label();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.btnEvaluate3 = new System.Windows.Forms.Button();
			this.LogBox3 = new System.Windows.Forms.TextBox();
			this.Label5 = new System.Windows.Forms.Label();
			this.updownA = new System.Windows.Forms.NumericUpDown();
			this.tbExpression3 = new System.Windows.Forms.TextBox();
			this.updownB = new System.Windows.Forms.NumericUpDown();
			this.updownC = new System.Windows.Forms.NumericUpDown();
			this.Label6 = new System.Windows.Forms.Label();
			this.Label7 = new System.Windows.Forms.Label();
			this.Label12 = new System.Windows.Forms.Label();
			this.Label8 = new System.Windows.Forms.Label();
			this.lblResults3 = new System.Windows.Forms.Label();
			this.TabControl1.SuspendLayout();
			this.TabPage1.SuspendLayout();
			this.TabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.updownA)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.updownB)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.updownC)).BeginInit();
			this.SuspendLayout();
			// 
			// tbExpression
			// 
			this.tbExpression.Location = new System.Drawing.Point(8, 32);
			this.tbExpression.Name = "tbExpression";
			this.tbExpression.Size = new System.Drawing.Size(328, 20);
			this.tbExpression.TabIndex = 0;
			this.tbExpression.Text = "4+5";
			// 
			// TextBox2
			// 
			this.TextBox2.Location = new System.Drawing.Point(8, 56);
			this.TextBox2.Multiline = true;
			this.TextBox2.Name = "TextBox2";
			this.TextBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.TextBox2.Size = new System.Drawing.Size(408, 336);
			this.TextBox2.TabIndex = 0;
			this.TextBox2.Text = "";
			// 
			// btnEvaluate
			// 
			this.btnEvaluate.Location = new System.Drawing.Point(344, 32);
			this.btnEvaluate.Name = "btnEvaluate";
			this.btnEvaluate.Size = new System.Drawing.Size(72, 23);
			this.btnEvaluate.TabIndex = 2;
			this.btnEvaluate.Text = "Evaluate";
			this.btnEvaluate.Click += new System.EventHandler(this.btnEvaluate_Click);
			// 
			// cbSamples
			// 
			this.cbSamples.Items.AddRange(new object[] {
																		 "123+345",
																		 "(2+3)*(4-2)",
																		 "120+5%",
																		 "now",
																		 "Round(now - Date(2006,1,1))+\" days since new year\"",
																		 "anumber*5",
																		 "arr[1]+arr[2]",
																		 "theForm.Controls[0].Name",
																		 "theForm.Left",
																		 "1+2 >= 3",
																		 "\"ab\"+\"c\" = \"abc\"",
																		 "(true?1:2)",
																		 "(false?\"a\":\"b\")",
																		 "MIN(4,3,1,2)",
																		 "MAX(1,2)"});
			this.cbSamples.Location = new System.Drawing.Point(8, 8);
			this.cbSamples.Name = "cbSamples";
			this.cbSamples.Size = new System.Drawing.Size(408, 21);
			this.cbSamples.TabIndex = 3;
			this.cbSamples.Text = "<enter an expression or select a sample>";
			this.cbSamples.SelectedIndexChanged += new System.EventHandler(this.cbSamples_SelectedIndexChanged);
			// 
			// TabControl1
			// 
			this.TabControl1.Controls.Add(this.TabPage1);
			this.TabControl1.Controls.Add(this.TabPage2);
			this.TabControl1.Controls.Add(this.tabPage3);
			this.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TabControl1.Location = new System.Drawing.Point(5, 5);
			this.TabControl1.Name = "TabControl1";
			this.TabControl1.SelectedIndex = 0;
			this.TabControl1.Size = new System.Drawing.Size(432, 422);
			this.TabControl1.TabIndex = 5;
			// 
			// TabPage1
			// 
			this.TabPage1.Controls.Add(this.tbExpression);
			this.TabPage1.Controls.Add(this.cbSamples);
			this.TabPage1.Controls.Add(this.btnEvaluate);
			this.TabPage1.Controls.Add(this.TextBox2);
			this.TabPage1.Location = new System.Drawing.Point(4, 22);
			this.TabPage1.Name = "TabPage1";
			this.TabPage1.Size = new System.Drawing.Size(424, 396);
			this.TabPage1.TabIndex = 0;
			this.TabPage1.Text = "Single expression";
			// 
			// TabPage2
			// 
			this.TabPage2.Controls.Add(this.cbAuto);
			this.TabPage2.Controls.Add(this.Label1);
			this.TabPage2.Controls.Add(this.PictureBox1);
			this.TabPage2.Controls.Add(this.tbExpressionRed);
			this.TabPage2.Controls.Add(this.ComboBox1);
			this.TabPage2.Controls.Add(this.btnEvaluate2);
			this.TabPage2.Controls.Add(this.tbExpressionGreen);
			this.TabPage2.Controls.Add(this.Label2);
			this.TabPage2.Controls.Add(this.Label3);
			this.TabPage2.Controls.Add(this.tbExpressionBlue);
			this.TabPage2.Controls.Add(this.Label4);
			this.TabPage2.Location = new System.Drawing.Point(4, 22);
			this.TabPage2.Name = "TabPage2";
			this.TabPage2.Size = new System.Drawing.Size(424, 396);
			this.TabPage2.TabIndex = 1;
			this.TabPage2.Text = "heavier evaluation";
			// 
			// cbAuto
			// 
			this.cbAuto.Checked = true;
			this.cbAuto.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbAuto.Location = new System.Drawing.Point(344, 64);
			this.cbAuto.Name = "cbAuto";
			this.cbAuto.Size = new System.Drawing.Size(64, 24);
			this.cbAuto.TabIndex = 10;
			this.cbAuto.Text = "Auto";
			// 
			// Label1
			// 
			this.Label1.Location = new System.Drawing.Point(8, 112);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(408, 16);
			this.Label1.TabIndex = 9;
			this.Label1.Text = "Label1";
			// 
			// PictureBox1
			// 
			this.PictureBox1.Location = new System.Drawing.Point(56, 128);
			this.PictureBox1.Name = "PictureBox1";
			this.PictureBox1.Size = new System.Drawing.Size(256, 256);
			this.PictureBox1.TabIndex = 8;
			this.PictureBox1.TabStop = false;
			// 
			// tbExpressionRed
			// 
			this.tbExpressionRed.Location = new System.Drawing.Point(56, 32);
			this.tbExpressionRed.Name = "tbExpressionRed";
			this.tbExpressionRed.Size = new System.Drawing.Size(280, 20);
			this.tbExpressionRed.TabIndex = 4;
			this.tbExpressionRed.Text = "x*15";
			this.tbExpressionRed.TextChanged += new System.EventHandler(this.tbExpressionBlue_TextChanged);
			// 
			// ComboBox1
			// 
			this.ComboBox1.Items.AddRange(new object[] {
																		 "Sample1",
																		 "Sample2",
																		 "Sample3",
																		 "Sample4"});
			this.ComboBox1.Location = new System.Drawing.Point(8, 8);
			this.ComboBox1.Name = "ComboBox1";
			this.ComboBox1.Size = new System.Drawing.Size(408, 21);
			this.ComboBox1.TabIndex = 6;
			this.ComboBox1.Text = "<enter an expression or select a sample>";
			this.ComboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
			// 
			// btnEvaluate2
			// 
			this.btnEvaluate2.Location = new System.Drawing.Point(344, 32);
			this.btnEvaluate2.Name = "btnEvaluate2";
			this.btnEvaluate2.Size = new System.Drawing.Size(72, 23);
			this.btnEvaluate2.TabIndex = 5;
			this.btnEvaluate2.Text = "Evaluate";
			this.btnEvaluate2.Click += new System.EventHandler(this.btnEvaluate2_Click);
			// 
			// tbExpressionGreen
			// 
			this.tbExpressionGreen.Location = new System.Drawing.Point(56, 56);
			this.tbExpressionGreen.Name = "tbExpressionGreen";
			this.tbExpressionGreen.Size = new System.Drawing.Size(280, 20);
			this.tbExpressionGreen.TabIndex = 4;
			this.tbExpressionGreen.Text = "cos(x*y*4900)";
			this.tbExpressionGreen.TextChanged += new System.EventHandler(this.tbExpressionBlue_TextChanged);
			// 
			// Label2
			// 
			this.Label2.Location = new System.Drawing.Point(8, 35);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(40, 16);
			this.Label2.TabIndex = 9;
			this.Label2.Text = "Red";
			this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// Label3
			// 
			this.Label3.Location = new System.Drawing.Point(8, 59);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(40, 16);
			this.Label3.TabIndex = 9;
			this.Label3.Text = "Green";
			this.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tbExpressionBlue
			// 
			this.tbExpressionBlue.Location = new System.Drawing.Point(56, 80);
			this.tbExpressionBlue.Name = "tbExpressionBlue";
			this.tbExpressionBlue.Size = new System.Drawing.Size(280, 20);
			this.tbExpressionBlue.TabIndex = 4;
			this.tbExpressionBlue.Text = "y*15";
			this.tbExpressionBlue.TextChanged += new System.EventHandler(this.tbExpressionBlue_TextChanged);
			// 
			// Label4
			// 
			this.Label4.Location = new System.Drawing.Point(8, 83);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(40, 16);
			this.Label4.TabIndex = 9;
			this.Label4.Text = "Blue";
			this.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.btnEvaluate3);
			this.tabPage3.Controls.Add(this.LogBox3);
			this.tabPage3.Controls.Add(this.Label5);
			this.tabPage3.Controls.Add(this.updownA);
			this.tabPage3.Controls.Add(this.tbExpression3);
			this.tabPage3.Controls.Add(this.updownB);
			this.tabPage3.Controls.Add(this.updownC);
			this.tabPage3.Controls.Add(this.Label6);
			this.tabPage3.Controls.Add(this.Label7);
			this.tabPage3.Controls.Add(this.Label12);
			this.tabPage3.Controls.Add(this.Label8);
			this.tabPage3.Controls.Add(this.lblResults3);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(424, 396);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Dynamic Formulas";
			// 
			// btnEvaluate3
			// 
			this.btnEvaluate3.Location = new System.Drawing.Point(344, 14);
			this.btnEvaluate3.Name = "btnEvaluate3";
			this.btnEvaluate3.Size = new System.Drawing.Size(72, 23);
			this.btnEvaluate3.TabIndex = 19;
			this.btnEvaluate3.Text = "Evaluate";
			this.btnEvaluate3.Click += new System.EventHandler(this.btnEvaluate3_Click);
			// 
			// LogBox3
			// 
			this.LogBox3.Location = new System.Drawing.Point(8, 182);
			this.LogBox3.Multiline = true;
			this.LogBox3.Name = "LogBox3";
			this.LogBox3.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.LogBox3.Size = new System.Drawing.Size(408, 200);
			this.LogBox3.TabIndex = 18;
			this.LogBox3.Text = "Notice how the formula is refreshed only when involved variables are modified.\r\n";
			// 
			// Label5
			// 
			this.Label5.Location = new System.Drawing.Point(32, 46);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(88, 20);
			this.Label5.TabIndex = 16;
			this.Label5.Text = "a";
			this.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// updownA
			// 
			this.updownA.Location = new System.Drawing.Point(128, 46);
			this.updownA.Maximum = new System.Decimal(new int[] {
																					 1000,
																					 0,
																					 0,
																					 0});
			this.updownA.Name = "updownA";
			this.updownA.Size = new System.Drawing.Size(72, 20);
			this.updownA.TabIndex = 11;
			this.updownA.Value = new System.Decimal(new int[] {
																				  23,
																				  0,
																				  0,
																				  0});
			this.updownA.ValueChanged += new System.EventHandler(this.updownA_ValueChanged);
			// 
			// tbExpression3
			// 
			this.tbExpression3.Location = new System.Drawing.Point(128, 14);
			this.tbExpression3.Name = "tbExpression3";
			this.tbExpression3.Size = new System.Drawing.Size(208, 20);
			this.tbExpression3.TabIndex = 8;
			this.tbExpression3.Text = "a+2*b";
			// 
			// updownB
			// 
			this.updownB.Location = new System.Drawing.Point(128, 78);
			this.updownB.Maximum = new System.Decimal(new int[] {
																					 1000,
																					 0,
																					 0,
																					 0});
			this.updownB.Name = "updownB";
			this.updownB.Size = new System.Drawing.Size(72, 20);
			this.updownB.TabIndex = 9;
			this.updownB.Value = new System.Decimal(new int[] {
																				  50,
																				  0,
																				  0,
																				  0});
			this.updownB.ValueChanged += new System.EventHandler(this.updownB_ValueChanged);
			// 
			// updownC
			// 
			this.updownC.Location = new System.Drawing.Point(128, 110);
			this.updownC.Maximum = new System.Decimal(new int[] {
																					 1000,
																					 0,
																					 0,
																					 0});
			this.updownC.Name = "updownC";
			this.updownC.Size = new System.Drawing.Size(72, 20);
			this.updownC.TabIndex = 10;
			this.updownC.Value = new System.Decimal(new int[] {
																				  150,
																				  0,
																				  0,
																				  0});
			this.updownC.ValueChanged += new System.EventHandler(this.updownC_ValueChanged);
			// 
			// Label6
			// 
			this.Label6.Location = new System.Drawing.Point(32, 78);
			this.Label6.Name = "Label6";
			this.Label6.Size = new System.Drawing.Size(88, 20);
			this.Label6.TabIndex = 17;
			this.Label6.Text = "b";
			this.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// Label7
			// 
			this.Label7.Location = new System.Drawing.Point(32, 110);
			this.Label7.Name = "Label7";
			this.Label7.Size = new System.Drawing.Size(88, 20);
			this.Label7.TabIndex = 13;
			this.Label7.Text = "c";
			this.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// Label12
			// 
			this.Label12.Location = new System.Drawing.Point(32, 14);
			this.Label12.Name = "Label12";
			this.Label12.Size = new System.Drawing.Size(88, 20);
			this.Label12.TabIndex = 12;
			this.Label12.Text = "Formula";
			this.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// Label8
			// 
			this.Label8.Location = new System.Drawing.Point(32, 150);
			this.Label8.Name = "Label8";
			this.Label8.Size = new System.Drawing.Size(88, 20);
			this.Label8.TabIndex = 15;
			this.Label8.Text = "Result";
			this.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lblResults3
			// 
			this.lblResults3.Location = new System.Drawing.Point(128, 150);
			this.lblResults3.Name = "lblResults3";
			this.lblResults3.Size = new System.Drawing.Size(288, 20);
			this.lblResults3.TabIndex = 14;
			this.lblResults3.Text = "Label5";
			this.lblResults3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(442, 432);
			this.Controls.Add(this.TabControl1);
			this.DockPadding.All = 5;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "Form1";
			this.Text = "Expression Evaluator 100% managed .net  (C# Version)";
			this.TabControl1.ResumeLayout(false);
			this.TabPage1.ResumeLayout(false);
			this.TabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.updownA)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.updownB)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.updownC)).EndInit();
			this.ResumeLayout(false);

		}
            
		private void tbExpressionBlue_TextChanged(object sender, System.EventArgs e) 
		{
			if (mInitializing) return;
			if (cbAuto.Checked) 
			{
				btnEvaluate2_Click(sender, e);
			}
		}
    
		private void cbAuto_CheckedChanged(object sender, System.EventArgs e) 
		{
			if (mInitializing) return;
			if (cbAuto.Checked) 
			{
				btnEvaluate2_Click(sender, e);
			}
		}

		public static void Main()
		{
			Form1 f=new Form1();
			f.ShowDialog();
		}

		private void btnEvaluate_Click(object sender, System.EventArgs e)
		{
			opCode lCode = null;
			try 
			{
				TextBox2.AppendText((tbExpression.Text + "\r\n"));
				//Timer t = new Timer();
				lCode = ev.Parse(tbExpression.Text);
				//TextBox2.AppendText(("Parsed in " 
				//	+ (t.ms() + (" ms" + "\r\n"))));
			}
			catch (Exception ex) 
			{
				TextBox2.AppendText((ex.Message + "\r\n"));
				return;
			}
			try 
			{
				//Timer t = new Timer();
				object res = lCode.value;
				//TextBox2.AppendText(("Run in " 
				//	+ (t.ms() + (" ms" + "\r\n"))));
				if ((res == null)) 
				{
					TextBox2.AppendText(("Result=" + "\r\n"));
				}
				else 
				{
					TextBox2.AppendText(("Result=" 
						+ (Evaluator.ConvertToString(res) + (" (" 
						+ (res.GetType().Name + (")" + "\r\n"))))));
				}
			}
			catch (Exception ex) 
			{
				TextBox2.AppendText((ex.Message + "\r\n"));
			}
			TextBox2.AppendText("\r\n");
		}
    
		private void cbSamples_SelectedIndexChanged(object sender, System.EventArgs e) 
		{
			tbExpression.Text = cbSamples.Text;
			btnEvaluate_Click(sender,e);
		}
    
		private void btnEvaluate2_Click(object sender, System.EventArgs e)
		{
			opCode lCodeR = null;
			opCode lCodeG = null;
			opCode lCodeB = null;
			try 
			{
				ev.AddEnvironmentFunctions(this);
				ev.AddEnvironmentFunctions(new EvalFunctions());
				lCodeR = ev.Parse(tbExpressionRed.Text);
				lCodeG = ev.Parse(tbExpressionGreen.Text);
				lCodeB = ev.Parse(tbExpressionBlue.Text);
			}
			catch (Exception ex) 
			{
				Label1.Text = ex.Message;
				return;
			}
			try 
			{
				Timer t = new Timer();
				Bitmap bm = (Bitmap)PictureBox1.Image;
				if ((bm == null)) 
				{
					bm = new Bitmap(256, 256);
					PictureBox1.Image = bm;
				}
				double mult = (2 
					* (Math.PI / 256));
				double r=0;
				double g=0;
				double b=0;
				for (int Xi = 0; (Xi <= 255); Xi++) 
				{
					X = ((Xi - 128) 
						* mult);
					for (int Yi = 0; (Yi <= 255); Yi++) 
					{
						Y = ((Yi - 128) 
							* mult);
						try 
						{
							
							r=(double)lCodeR.value;						
							g=(double)lCodeG.value;							
							b=(double)lCodeB.value;
							
							if (((r < 0) 
								|| double.IsNaN(r))) 
							{
								r = 0;
							}
							else if ((r > 1)) 
							{
								r = 1;
							}
							if (((g < 0) 
								|| double.IsNaN(g))) 
							{
								g = 0;
							}
							else if ((g > 1)) 
							{
								g = 1;
							}
							if (((b < 0) 
								|| double.IsNaN(b))) 
							{
								b = 0;
							}
							else if ((b > 1)) 
							{
								b = 1;
							}
						}
						catch 
						{
							//  ignore (same as previous pixel)
						}
						bm.SetPixel(Xi, Yi, Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255)));
					}
					if ((Xi & 7) == 7) 
					{
						PictureBox1.Refresh();
					}
				}
				Label1.Text = ("196,608 evaluations run in " 
					+ (t.ms() + " ms"));
			}
			catch (Exception ex) 
			{
				Label1.Text = ex.Message;
			}
		}

		private void ComboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			mInitializing=true;
			switch (ComboBox1.SelectedIndex) 
			{
				case 1:
					tbExpressionRed.Text = "mod(round(4*x-y*2),2)-x";
					tbExpressionGreen.Text = "mod(abs(x+2*y),0.75)*10+y/5";
					tbExpressionBlue.Text = "round(sin(sqrt(x*x+y*y))*3/5)+x/3";
					break;
				case 2:
					tbExpressionRed.Text = "1-round(x/y*0.5)";
					tbExpressionGreen.Text = "1-round(y/x*0.4)";
					tbExpressionBlue.Text = "round(sin(sqrt(x*x+y*y)*10))";
					break;
				case 3:
					tbExpressionRed.Text = "cos(x/2)/2";
					tbExpressionGreen.Text = "cos(y/2)/3";
					tbExpressionBlue.Text = "round(sin(sqrt(x*x*x+y*y)*10))";
					break;
				case 4:
					tbExpressionRed.Text = "x*15";
					tbExpressionGreen.Text = "cos(x*y*4900)";
					tbExpressionBlue.Text = "y*15";
					break;
				default:
					tbExpressionRed.Text = String.Empty;
					tbExpressionGreen.Text = String.Empty;
					tbExpressionBlue.Text = String.Empty;
					break;
			}
			mInitializing=false;			
			btnEvaluate2_Click(sender, e);
		}

		private void mFormula3_ValueChanged(Object Sender, System.EventArgs e) 
		{
			string v = Evaluator.ConvertToString(mFormula3.value);
			lblResults3.Text = v;
			LogBox3.AppendText(System.DateTime.Now.ToLongTimeString() + ": " + v + "\r\n");
		}

		private Eval3.iEvalValue.ValueChangedEventHandler FormulaHandler=null;


		private void btnEvaluate3_Click(object sender, System.EventArgs e)
		{
			try
			{
				// I am not a C# specialist... is that the right way of doing this?
				if (FormulaHandler!=null) mFormula3.ValueChanged -= FormulaHandler;
				mFormula3 = ev.Parse(tbExpression3.Text);
				FormulaHandler = new Eval3.iEvalValue.ValueChangedEventHandler(mFormula3_ValueChanged);
				mFormula3.ValueChanged += FormulaHandler;
				mFormula3_ValueChanged(null, null);
			}
			catch(Exception ex)
			{
				lblResults3.Text = ex.Message;
			}
		}

		private void updownA_ValueChanged(object sender, System.EventArgs e)
		{
			A.Value = (double)updownA.Value;
		}

		private void updownB_ValueChanged(object sender, System.EventArgs e)
		{
			B.Value = (double)updownB.Value;
		}

		private void updownC_ValueChanged(object sender, System.EventArgs e)
		{
			C.Value = (double)updownC.Value;
		}

	}
	
}
