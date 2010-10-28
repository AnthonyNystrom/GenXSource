namespace MathXDemo
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpExplicitFunctions = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnCalcInvert = new System.Windows.Forms.Button();
            this.btnPlotInvert = new System.Windows.Forms.Button();
            this.txtInvertFunction = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnPlotSource = new System.Windows.Forms.Button();
            this.txtExplicitFunction = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nudDerivative = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtExplicitResult = new System.Windows.Forms.TextBox();
            this.btnCalcDerivative = new System.Windows.Forms.Button();
            this.btnPlotDerivative = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtYValuleAt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCalcValueAt = new System.Windows.Forms.Button();
            this.nudValueAt = new System.Windows.Forms.NumericUpDown();
            this.tpImplicitFunctions = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnPlotImpl = new System.Windows.Forms.Button();
            this.txtImplicitFunction = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.nudDerivativeImpl = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.txtImplicitResult = new System.Windows.Forms.TextBox();
            this.btnCaclImplDerivative = new System.Windows.Forms.Button();
            this.btnPlotImplDerivative = new System.Windows.Forms.Button();
            this.tpParametricFunctions = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.txtSourceY = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnPlotParamSource = new System.Windows.Forms.Button();
            this.txtSourceX = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.pnlGraph = new System.Windows.Forms.Panel();
            this.lblClearChart = new System.Windows.Forms.LinkLabel();
            this.chart = new ZedGraph.ZedGraphControl();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtXValue = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnCalcParametric = new System.Windows.Forms.Button();
            this.nudtValue = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.txtYValue = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tpExplicitFunctions.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDerivative)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudValueAt)).BeginInit();
            this.tpImplicitFunctions.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDerivativeImpl)).BeginInit();
            this.tpParametricFunctions.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.pnlGraph.SuspendLayout();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudtValue)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpExplicitFunctions);
            this.tabControl1.Controls.Add(this.tpImplicitFunctions);
            this.tabControl1.Controls.Add(this.tpParametricFunctions);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(303, 514);
            this.tabControl1.TabIndex = 0;
            // 
            // tpExplicitFunctions
            // 
            this.tpExplicitFunctions.Controls.Add(this.groupBox7);
            this.tpExplicitFunctions.Controls.Add(this.groupBox3);
            this.tpExplicitFunctions.Controls.Add(this.groupBox2);
            this.tpExplicitFunctions.Controls.Add(this.groupBox1);
            this.tpExplicitFunctions.Location = new System.Drawing.Point(4, 22);
            this.tpExplicitFunctions.Name = "tpExplicitFunctions";
            this.tpExplicitFunctions.Padding = new System.Windows.Forms.Padding(3);
            this.tpExplicitFunctions.Size = new System.Drawing.Size(295, 488);
            this.tpExplicitFunctions.TabIndex = 0;
            this.tpExplicitFunctions.Text = "Explicit Functions";
            this.tpExplicitFunctions.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.btnCalcInvert);
            this.groupBox7.Controls.Add(this.btnPlotInvert);
            this.groupBox7.Controls.Add(this.txtInvertFunction);
            this.groupBox7.Location = new System.Drawing.Point(6, 231);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(283, 90);
            this.groupBox7.TabIndex = 10;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Invert";
            // 
            // btnCalcInvert
            // 
            this.btnCalcInvert.Location = new System.Drawing.Point(9, 61);
            this.btnCalcInvert.Name = "btnCalcInvert";
            this.btnCalcInvert.Size = new System.Drawing.Size(75, 23);
            this.btnCalcInvert.TabIndex = 6;
            this.btnCalcInvert.Text = "Evalute";
            this.btnCalcInvert.UseVisualStyleBackColor = true;
            this.btnCalcInvert.Click += new System.EventHandler(this.btnCalcInvert_Click);
            // 
            // btnPlotInvert
            // 
            this.btnPlotInvert.Location = new System.Drawing.Point(198, 61);
            this.btnPlotInvert.Name = "btnPlotInvert";
            this.btnPlotInvert.Size = new System.Drawing.Size(77, 23);
            this.btnPlotInvert.TabIndex = 5;
            this.btnPlotInvert.Text = "Plot";
            this.btnPlotInvert.UseVisualStyleBackColor = true;
            this.btnPlotInvert.Click += new System.EventHandler(this.btnPlotInvert_Click);
            // 
            // txtInvertFunction
            // 
            this.txtInvertFunction.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtInvertFunction.Location = new System.Drawing.Point(9, 20);
            this.txtInvertFunction.Multiline = true;
            this.txtInvertFunction.Name = "txtInvertFunction";
            this.txtInvertFunction.Size = new System.Drawing.Size(266, 35);
            this.txtInvertFunction.TabIndex = 3;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnPlotSource);
            this.groupBox3.Controls.Add(this.txtExplicitFunction);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(6, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(283, 121);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Source";
            // 
            // btnPlotSource
            // 
            this.btnPlotSource.Location = new System.Drawing.Point(200, 92);
            this.btnPlotSource.Name = "btnPlotSource";
            this.btnPlotSource.Size = new System.Drawing.Size(77, 23);
            this.btnPlotSource.TabIndex = 4;
            this.btnPlotSource.Text = "Plot";
            this.btnPlotSource.UseVisualStyleBackColor = true;
            this.btnPlotSource.Click += new System.EventHandler(this.btnPlotSource_Click);
            // 
            // txtExplicitFunction
            // 
            this.txtExplicitFunction.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtExplicitFunction.Location = new System.Drawing.Point(11, 33);
            this.txtExplicitFunction.Multiline = true;
            this.txtExplicitFunction.Name = "txtExplicitFunction";
            this.txtExplicitFunction.Size = new System.Drawing.Size(266, 55);
            this.txtExplicitFunction.TabIndex = 2;
            this.txtExplicitFunction.Text = "sin(x)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Expression";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.nudDerivative);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtExplicitResult);
            this.groupBox2.Controls.Add(this.btnCalcDerivative);
            this.groupBox2.Controls.Add(this.btnPlotDerivative);
            this.groupBox2.Location = new System.Drawing.Point(6, 327);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(283, 154);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Derivative";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(111, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Derivative Expression";
            // 
            // nudDerivative
            // 
            this.nudDerivative.Location = new System.Drawing.Point(9, 33);
            this.nudDerivative.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudDerivative.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDerivative.Name = "nudDerivative";
            this.nudDerivative.Size = new System.Drawing.Size(80, 21);
            this.nudDerivative.TabIndex = 5;
            this.nudDerivative.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Derivative";
            // 
            // txtExplicitResult
            // 
            this.txtExplicitResult.Location = new System.Drawing.Point(9, 73);
            this.txtExplicitResult.Multiline = true;
            this.txtExplicitResult.Name = "txtExplicitResult";
            this.txtExplicitResult.Size = new System.Drawing.Size(268, 44);
            this.txtExplicitResult.TabIndex = 4;
            // 
            // btnCalcDerivative
            // 
            this.btnCalcDerivative.Location = new System.Drawing.Point(11, 123);
            this.btnCalcDerivative.Name = "btnCalcDerivative";
            this.btnCalcDerivative.Size = new System.Drawing.Size(75, 23);
            this.btnCalcDerivative.TabIndex = 0;
            this.btnCalcDerivative.Text = "Evalute";
            this.btnCalcDerivative.UseVisualStyleBackColor = true;
            this.btnCalcDerivative.Click += new System.EventHandler(this.btnCalcDerivative_Click);
            // 
            // btnPlotDerivative
            // 
            this.btnPlotDerivative.Location = new System.Drawing.Point(200, 123);
            this.btnPlotDerivative.Name = "btnPlotDerivative";
            this.btnPlotDerivative.Size = new System.Drawing.Size(77, 23);
            this.btnPlotDerivative.TabIndex = 3;
            this.btnPlotDerivative.Text = "Plot";
            this.btnPlotDerivative.UseVisualStyleBackColor = true;
            this.btnPlotDerivative.Click += new System.EventHandler(this.btnPlotDerivative_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtYValuleAt);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnCalcValueAt);
            this.groupBox1.Controls.Add(this.nudValueAt);
            this.groupBox1.Location = new System.Drawing.Point(6, 133);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(283, 92);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Value At";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(92, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Y value";
            // 
            // txtYValuleAt
            // 
            this.txtYValuleAt.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtYValuleAt.Location = new System.Drawing.Point(95, 33);
            this.txtYValuleAt.Multiline = true;
            this.txtYValuleAt.Name = "txtYValuleAt";
            this.txtYValuleAt.ReadOnly = true;
            this.txtYValuleAt.Size = new System.Drawing.Size(182, 21);
            this.txtYValuleAt.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "X value";
            // 
            // btnCalcValueAt
            // 
            this.btnCalcValueAt.Location = new System.Drawing.Point(9, 60);
            this.btnCalcValueAt.Name = "btnCalcValueAt";
            this.btnCalcValueAt.Size = new System.Drawing.Size(77, 23);
            this.btnCalcValueAt.TabIndex = 7;
            this.btnCalcValueAt.Text = "Calculate";
            this.btnCalcValueAt.UseVisualStyleBackColor = true;
            this.btnCalcValueAt.Click += new System.EventHandler(this.btnCalcValueAt_Click);
            // 
            // nudValueAt
            // 
            this.nudValueAt.Location = new System.Drawing.Point(9, 33);
            this.nudValueAt.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudValueAt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudValueAt.Name = "nudValueAt";
            this.nudValueAt.Size = new System.Drawing.Size(80, 21);
            this.nudValueAt.TabIndex = 6;
            this.nudValueAt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tpImplicitFunctions
            // 
            this.tpImplicitFunctions.Controls.Add(this.groupBox4);
            this.tpImplicitFunctions.Controls.Add(this.groupBox5);
            this.tpImplicitFunctions.Location = new System.Drawing.Point(4, 22);
            this.tpImplicitFunctions.Name = "tpImplicitFunctions";
            this.tpImplicitFunctions.Padding = new System.Windows.Forms.Padding(3);
            this.tpImplicitFunctions.Size = new System.Drawing.Size(295, 488);
            this.tpImplicitFunctions.TabIndex = 1;
            this.tpImplicitFunctions.Text = "Implicit Functions";
            this.tpImplicitFunctions.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnPlotImpl);
            this.groupBox4.Controls.Add(this.txtImplicitFunction);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(6, 7);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(283, 121);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Source";
            // 
            // btnPlotImpl
            // 
            this.btnPlotImpl.Location = new System.Drawing.Point(200, 92);
            this.btnPlotImpl.Name = "btnPlotImpl";
            this.btnPlotImpl.Size = new System.Drawing.Size(77, 23);
            this.btnPlotImpl.TabIndex = 4;
            this.btnPlotImpl.Text = "Plot";
            this.btnPlotImpl.UseVisualStyleBackColor = true;
            this.btnPlotImpl.Click += new System.EventHandler(this.btnPlotSourceImpl_Click);
            // 
            // txtImplicitFunction
            // 
            this.txtImplicitFunction.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtImplicitFunction.Location = new System.Drawing.Point(11, 33);
            this.txtImplicitFunction.Multiline = true;
            this.txtImplicitFunction.Name = "txtImplicitFunction";
            this.txtImplicitFunction.Size = new System.Drawing.Size(266, 55);
            this.txtImplicitFunction.TabIndex = 2;
            this.txtImplicitFunction.Text = "x*x+y*y-25";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Expression";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.nudDerivativeImpl);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.txtImplicitResult);
            this.groupBox5.Controls.Add(this.btnCaclImplDerivative);
            this.groupBox5.Controls.Add(this.btnPlotImplDerivative);
            this.groupBox5.Location = new System.Drawing.Point(6, 134);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(283, 216);
            this.groupBox5.TabIndex = 11;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Derivative";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(111, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Derivative Expression";
            // 
            // nudDerivativeImpl
            // 
            this.nudDerivativeImpl.Location = new System.Drawing.Point(9, 39);
            this.nudDerivativeImpl.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudDerivativeImpl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDerivativeImpl.Name = "nudDerivativeImpl";
            this.nudDerivativeImpl.Size = new System.Drawing.Size(80, 21);
            this.nudDerivativeImpl.TabIndex = 5;
            this.nudDerivativeImpl.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Derivative";
            // 
            // txtImplicitResult
            // 
            this.txtImplicitResult.Location = new System.Drawing.Point(9, 79);
            this.txtImplicitResult.Multiline = true;
            this.txtImplicitResult.Name = "txtImplicitResult";
            this.txtImplicitResult.Size = new System.Drawing.Size(268, 100);
            this.txtImplicitResult.TabIndex = 4;
            // 
            // btnCaclImplDerivative
            // 
            this.btnCaclImplDerivative.Location = new System.Drawing.Point(9, 185);
            this.btnCaclImplDerivative.Name = "btnCaclImplDerivative";
            this.btnCaclImplDerivative.Size = new System.Drawing.Size(75, 23);
            this.btnCaclImplDerivative.TabIndex = 0;
            this.btnCaclImplDerivative.Text = "Evalute";
            this.btnCaclImplDerivative.UseVisualStyleBackColor = true;
            this.btnCaclImplDerivative.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnPlotImplDerivative
            // 
            this.btnPlotImplDerivative.Location = new System.Drawing.Point(200, 185);
            this.btnPlotImplDerivative.Name = "btnPlotImplDerivative";
            this.btnPlotImplDerivative.Size = new System.Drawing.Size(77, 23);
            this.btnPlotImplDerivative.TabIndex = 3;
            this.btnPlotImplDerivative.Text = "Plot";
            this.btnPlotImplDerivative.UseVisualStyleBackColor = true;
            this.btnPlotImplDerivative.Click += new System.EventHandler(this.btnPlotDerivativeImpl_Click);
            // 
            // tpParametricFunctions
            // 
            this.tpParametricFunctions.Controls.Add(this.groupBox8);
            this.tpParametricFunctions.Controls.Add(this.groupBox6);
            this.tpParametricFunctions.Location = new System.Drawing.Point(4, 22);
            this.tpParametricFunctions.Name = "tpParametricFunctions";
            this.tpParametricFunctions.Size = new System.Drawing.Size(295, 488);
            this.tpParametricFunctions.TabIndex = 2;
            this.tpParametricFunctions.Text = "Parametric Functions";
            this.tpParametricFunctions.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.txtSourceY);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Controls.Add(this.btnPlotParamSource);
            this.groupBox6.Controls.Add(this.txtSourceX);
            this.groupBox6.Controls.Add(this.label9);
            this.groupBox6.Location = new System.Drawing.Point(6, 7);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(283, 161);
            this.groupBox6.TabIndex = 14;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Source";
            // 
            // txtSourceY
            // 
            this.txtSourceY.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtSourceY.Location = new System.Drawing.Point(10, 88);
            this.txtSourceY.Multiline = true;
            this.txtSourceY.Name = "txtSourceY";
            this.txtSourceY.Size = new System.Drawing.Size(266, 35);
            this.txtSourceY.TabIndex = 6;
            this.txtSourceY.Text = "t";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(7, 72);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 13);
            this.label12.TabIndex = 5;
            this.label12.Text = "Expression Y";
            // 
            // btnPlotParamSource
            // 
            this.btnPlotParamSource.Location = new System.Drawing.Point(200, 129);
            this.btnPlotParamSource.Name = "btnPlotParamSource";
            this.btnPlotParamSource.Size = new System.Drawing.Size(77, 23);
            this.btnPlotParamSource.TabIndex = 4;
            this.btnPlotParamSource.Text = "Plot";
            this.btnPlotParamSource.UseVisualStyleBackColor = true;
            this.btnPlotParamSource.Click += new System.EventHandler(this.btnPlotParamSource_Click);
            // 
            // txtSourceX
            // 
            this.txtSourceX.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtSourceX.Location = new System.Drawing.Point(11, 33);
            this.txtSourceX.Multiline = true;
            this.txtSourceX.Name = "txtSourceX";
            this.txtSourceX.Size = new System.Drawing.Size(266, 35);
            this.txtSourceX.TabIndex = 2;
            this.txtSourceX.Text = "t";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Expression X";
            // 
            // pnlGraph
            // 
            this.pnlGraph.Controls.Add(this.lblClearChart);
            this.pnlGraph.Controls.Add(this.chart);
            this.pnlGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGraph.Location = new System.Drawing.Point(303, 0);
            this.pnlGraph.Name = "pnlGraph";
            this.pnlGraph.Size = new System.Drawing.Size(403, 514);
            this.pnlGraph.TabIndex = 1;
            // 
            // lblClearChart
            // 
            this.lblClearChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblClearChart.AutoSize = true;
            this.lblClearChart.Location = new System.Drawing.Point(338, 497);
            this.lblClearChart.Name = "lblClearChart";
            this.lblClearChart.Size = new System.Drawing.Size(60, 13);
            this.lblClearChart.TabIndex = 1;
            this.lblClearChart.TabStop = true;
            this.lblClearChart.Text = "Clear chart";
            this.lblClearChart.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblClearChart_LinkClicked);
            // 
            // chart
            // 
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.Name = "chart";
            this.chart.ScrollGrace = 0;
            this.chart.ScrollMaxX = 0;
            this.chart.ScrollMaxY = 0;
            this.chart.ScrollMaxY2 = 0;
            this.chart.ScrollMinX = 0;
            this.chart.ScrollMinY = 0;
            this.chart.ScrollMinY2 = 0;
            this.chart.Size = new System.Drawing.Size(403, 514);
            this.chart.TabIndex = 0;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.label13);
            this.groupBox8.Controls.Add(this.txtYValue);
            this.groupBox8.Controls.Add(this.label10);
            this.groupBox8.Controls.Add(this.txtXValue);
            this.groupBox8.Controls.Add(this.label11);
            this.groupBox8.Controls.Add(this.btnCalcParametric);
            this.groupBox8.Controls.Add(this.nudtValue);
            this.groupBox8.Location = new System.Drawing.Point(6, 174);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(283, 92);
            this.groupBox8.TabIndex = 15;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Value At";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(92, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "X value";
            // 
            // txtXValue
            // 
            this.txtXValue.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtXValue.Location = new System.Drawing.Point(95, 33);
            this.txtXValue.Multiline = true;
            this.txtXValue.Name = "txtXValue";
            this.txtXValue.ReadOnly = true;
            this.txtXValue.Size = new System.Drawing.Size(82, 21);
            this.txtXValue.TabIndex = 9;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 17);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 13);
            this.label11.TabIndex = 8;
            this.label11.Text = "t value";
            // 
            // btnCalcParametric
            // 
            this.btnCalcParametric.Location = new System.Drawing.Point(9, 60);
            this.btnCalcParametric.Name = "btnCalcParametric";
            this.btnCalcParametric.Size = new System.Drawing.Size(77, 23);
            this.btnCalcParametric.TabIndex = 7;
            this.btnCalcParametric.Text = "Calculate";
            this.btnCalcParametric.UseVisualStyleBackColor = true;
            this.btnCalcParametric.Click += new System.EventHandler(this.btnCalcParametric_Click);
            // 
            // nudtValue
            // 
            this.nudtValue.Location = new System.Drawing.Point(9, 33);
            this.nudtValue.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudtValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudtValue.Name = "nudtValue";
            this.nudtValue.Size = new System.Drawing.Size(80, 21);
            this.nudtValue.TabIndex = 6;
            this.nudtValue.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(180, 17);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(42, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "Y value";
            // 
            // txtYValue
            // 
            this.txtYValue.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
            this.txtYValue.Location = new System.Drawing.Point(183, 33);
            this.txtYValue.Multiline = true;
            this.txtYValue.Name = "txtYValue";
            this.txtYValue.ReadOnly = true;
            this.txtYValue.Size = new System.Drawing.Size(93, 21);
            this.txtYValue.TabIndex = 11;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 514);
            this.Controls.Add(this.pnlGraph);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MathX Demo";
            this.tabControl1.ResumeLayout(false);
            this.tpExplicitFunctions.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDerivative)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudValueAt)).EndInit();
            this.tpImplicitFunctions.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDerivativeImpl)).EndInit();
            this.tpParametricFunctions.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.pnlGraph.ResumeLayout(false);
            this.pnlGraph.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudtValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpExplicitFunctions;
        private System.Windows.Forms.TabPage tpImplicitFunctions;
        private System.Windows.Forms.TabPage tpParametricFunctions;
        private System.Windows.Forms.Panel pnlGraph;
        private System.Windows.Forms.LinkLabel lblClearChart;
        private ZedGraph.ZedGraphControl chart;
        private System.Windows.Forms.TextBox txtExplicitResult;
        private System.Windows.Forms.Button btnPlotDerivative;
        private System.Windows.Forms.TextBox txtExplicitFunction;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCalcDerivative;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudDerivative;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCalcValueAt;
        private System.Windows.Forms.NumericUpDown nudValueAt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtYValuleAt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnPlotSource;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnPlotImpl;
        private System.Windows.Forms.TextBox txtImplicitFunction;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudDerivativeImpl;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtImplicitResult;
        private System.Windows.Forms.Button btnCaclImplDerivative;
        private System.Windows.Forms.Button btnPlotImplDerivative;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.TextBox txtSourceY;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnPlotParamSource;
        private System.Windows.Forms.TextBox txtSourceX;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button btnCalcInvert;
        private System.Windows.Forms.Button btnPlotInvert;
        private System.Windows.Forms.TextBox txtInvertFunction;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtYValue;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtXValue;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnCalcParametric;
        private System.Windows.Forms.NumericUpDown nudtValue;
    }
}

