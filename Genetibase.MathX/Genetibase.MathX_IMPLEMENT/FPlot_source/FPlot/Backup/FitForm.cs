using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using FPlotLibrary;

namespace FPlot
{
	/// <summary>
	/// Summary description for MarquardtForm.
	/// </summary>
	public class FitForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ComboBox function;
		private System.Windows.Forms.ComboBox data;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button start;
		private System.Windows.Forms.Button close;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private GraphControl graph;
		private Function1D f = null, oldf = null;
		private DataItem dataItem = null;
		private ParForm parForm = null;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox algorithm;
		private System.Windows.Forms.Button covariance;
		private CovarianceForm covarForm;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox neval;
		private float[,] covar = null;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox chisq;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox Q;
		private SourceGrid2.Grid grid;
		private FPlotFit.Fit fit;
		private int plength = 0;
		private bool[] fitp;

		private delegate void StepInvokeHandler(bool finished);
		private StepInvokeHandler invoke;
		
		public FitForm(GraphControl graph)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.graph = graph;
			parForm = new ParForm();
			covarForm = new CovarianceForm();
			data.DropDownStyle = ComboBoxStyle.DropDownList;
			function.DropDownStyle = ComboBoxStyle.DropDownList;
			fit = new FPlotFit.Fit();
			fit.Step += new FPlotFit.StepEventHandler(Step);
			invoke = new StepInvokeHandler(StepInvoke);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FitForm));
			this.function = new System.Windows.Forms.ComboBox();
			this.data = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.start = new System.Windows.Forms.Button();
			this.close = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.algorithm = new System.Windows.Forms.ComboBox();
			this.covariance = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.neval = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.chisq = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.Q = new System.Windows.Forms.TextBox();
			this.grid = new SourceGrid2.Grid();
			this.SuspendLayout();
			// 
			// function
			// 
			this.function.Location = new System.Drawing.Point(64, 16);
			this.function.Name = "function";
			this.function.Size = new System.Drawing.Size(120, 21);
			this.function.TabIndex = 0;
			// 
			// data
			// 
			this.data.Location = new System.Drawing.Point(232, 16);
			this.data.Name = "data";
			this.data.Size = new System.Drawing.Size(120, 21);
			this.data.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 24);
			this.label1.TabIndex = 2;
			this.label1.Text = "Function:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(192, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 24);
			this.label2.TabIndex = 3;
			this.label2.Text = "Data:";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 80);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 23);
			this.label3.TabIndex = 5;
			this.label3.Text = "Parameters";
			// 
			// start
			// 
			this.start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.start.Location = new System.Drawing.Point(8, 333);
			this.start.Name = "start";
			this.start.TabIndex = 9;
			this.start.Text = "Start";
			this.start.Click += new System.EventHandler(this.startClick);
			// 
			// close
			// 
			this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.close.Location = new System.Drawing.Point(96, 333);
			this.close.Name = "close";
			this.close.TabIndex = 10;
			this.close.Text = "Close";
			this.close.Click += new System.EventHandler(this.closeClick);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.Location = new System.Drawing.Point(288, 333);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 23);
			this.button1.TabIndex = 17;
			this.button1.Text = "Help...";
			this.button1.Click += new System.EventHandler(this.helpClick);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 48);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 24);
			this.label4.TabIndex = 18;
			this.label4.Text = "Algorithm:";
			// 
			// algorithm
			// 
			this.algorithm.Items.AddRange(new object[] {
																									 "Marquardt",
																									 "Nelder & Mead",
																									 "Simulated Annealing"});
			this.algorithm.Location = new System.Drawing.Point(72, 48);
			this.algorithm.Name = "algorithm";
			this.algorithm.Size = new System.Drawing.Size(136, 21);
			this.algorithm.TabIndex = 19;
			this.algorithm.Text = "Marquardt";
			// 
			// covariance
			// 
			this.covariance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.covariance.Location = new System.Drawing.Point(232, 301);
			this.covariance.Name = "covariance";
			this.covariance.Size = new System.Drawing.Size(128, 23);
			this.covariance.TabIndex = 20;
			this.covariance.Text = "Covariance Matrix...";
			this.covariance.Click += new System.EventHandler(this.covarClick);
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label7.Location = new System.Drawing.Point(8, 301);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(112, 23);
			this.label7.TabIndex = 21;
			this.label7.Text = "Function evaluations:";
			// 
			// neval
			// 
			this.neval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.neval.Location = new System.Drawing.Point(120, 301);
			this.neval.Name = "neval";
			this.neval.ReadOnly = true;
			this.neval.Size = new System.Drawing.Size(72, 20);
			this.neval.TabIndex = 22;
			this.neval.Text = "";
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label5.Location = new System.Drawing.Point(16, 269);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 23);
			this.label5.TabIndex = 11;
			this.label5.Text = "Chisquare:";
			// 
			// chisq
			// 
			this.chisq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.chisq.Location = new System.Drawing.Point(80, 269);
			this.chisq.Name = "chisq";
			this.chisq.ReadOnly = true;
			this.chisq.Size = new System.Drawing.Size(112, 20);
			this.chisq.TabIndex = 12;
			this.chisq.Text = "";
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label6.Location = new System.Drawing.Point(208, 269);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(16, 23);
			this.label6.TabIndex = 13;
			this.label6.Text = "Q:";
			// 
			// Q
			// 
			this.Q.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.Q.Location = new System.Drawing.Point(232, 269);
			this.Q.Name = "Q";
			this.Q.ReadOnly = true;
			this.Q.TabIndex = 14;
			this.Q.Text = "";
			// 
			// grid
			// 
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grid.AutoSizeMinHeight = 10;
			this.grid.AutoSizeMinWidth = 10;
			this.grid.AutoStretchColumnsToFitWidth = false;
			this.grid.AutoStretchRowsToFitHeight = false;
			this.grid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.grid.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None;
			this.grid.CustomSort = false;
			this.grid.GridToolTipActive = true;
			this.grid.Location = new System.Drawing.Point(0, 104);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(368, 157);
			this.grid.SpecialKeys = SourceGrid2.GridSpecialKeys.Default;
			this.grid.TabIndex = 23;
			// 
			// FitForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(368, 360);
			this.Controls.Add(this.grid);
			this.Controls.Add(this.neval);
			this.Controls.Add(this.Q);
			this.Controls.Add(this.chisq);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.covariance);
			this.Controls.Add(this.algorithm);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.close);
			this.Controls.Add(this.start);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.data);
			this.Controls.Add(this.function);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(376, 312);
			this.Name = "FitForm";
			this.ShowInTaskbar = false;
			this.Text = "Fit";
			this.ResumeLayout(false);

		}
		#endregion

		public void ResetPar() {
			bool fitpok = true;
			lock(this) {
				GraphModel model = graph.Model;
				string name;
				int i;
				name = (string)function.Text;
				f = null;
				for (i = 0; i < model.Items.Count; i++) {
					if ((model.Items[i].name == name) && (model.Items[i] is Function1D) &&
						(((Function1D)model.Items[i]).Fitable())) {
						f = (Function1D)model.Items[i];
					}
				}
				name = (string)data.Text;
				dataItem = null;
				for (i = 0; i < model.Items.Count; i++) {
					if ((model.Items[i].name == name) && (model.Items[i] is DataItem)) {
						dataItem = (DataItem)model.Items[i];
					}
				}
				if (f != null) {
					if (f != oldf || f.Modified) {
						if (f != oldf) {
							fitp = new bool[f.p.Length];
							for (i = 0; i < f.p.Length; i++) {
								fitp[i] = true;
							}
						}
						grid.ColumnsCount = 4;
						grid.RowsCount = f.p.Length + 1;
						grid[0, 0] = new SourceGrid2.Cells.Real.ColumnHeader("n");
						grid[0, 1] = new SourceGrid2.Cells.Real.ColumnHeader("fit");
						grid[0, 2] = new SourceGrid2.Cells.Real.ColumnHeader("p[n]");
						grid[0, 3] = new SourceGrid2.Cells.Real.ColumnHeader("±Δp[n]");
						for (i = 0; i < f.p.Length; i++) {
							grid[i+1, 0] = new SourceGrid2.Cells.Real.RowHeader(i.ToString());
							grid[i+1, 1] = new SourceGrid2.Cells.Real.CheckBox(fitp[i]);
							grid[i+1, 2] = new SourceGrid2.Cells.Real.Cell(f.p[i], typeof(double));
							if (covar == null) {
								grid[i+1, 3] = new SourceGrid2.Cells.Real.Cell("", typeof(string));
							} else {
								grid[i+1, 3] = new SourceGrid2.Cells.Real.Cell(Math.Sqrt(covar[i, i]), typeof(double));
							}
							grid[i+1, 3].DataModel.EnableEdit = false;
						}
						covar = null;
					}

					plength = f.p.Length;
				}
				if (f == null) {
					grid.RowsCount = 4;
					grid.ColumnsCount = 1;
					grid[0, 0] = new SourceGrid2.Cells.Real.ColumnHeader("n");
					grid[0, 1] = new SourceGrid2.Cells.Real.ColumnHeader("fit");
					grid[0, 2] = new SourceGrid2.Cells.Real.ColumnHeader("p[n]");
					grid[0, 3] = new SourceGrid2.Cells.Real.ColumnHeader("±Δp[n]");

				}
				grid.AutoSize();
				oldf = f;
				Q.Text = "";
				chisq.Text = "";
				covariance.Enabled = covar != null;
				fitpok = false;
				for (i = 0; i < fitp.Length; i++) {
					if (fitp[i]) fitpok = true;
				}
				start.Enabled = (f != null && data != null && fitpok);
				neval.Text = "";
			}
		}

		public void ResetErr() {
			lock(this) {
				int i;
				for (i = 0; i < f.p.Length; i++) {
					grid[i+1, 3].Value = Math.Sqrt(Math.Abs(covar[i, i]));
				}
				grid.AutoSize();
			}
		}
		
		public void Reset() {
			lock(this) {
				GraphModel model = graph.Model;
				function.Items.Clear();
				for (int i = 0; i < model.Items.Count; i++) {
					if ((model.Items[i] is Function1D) && ((Function1D)model.Items[i]).Fitable()) {
						function.Items.Add(model.Items[i].name);
					}
				}
				if (function.Items.Count > 0) {
					function.Text = (string)function.Items[0];
				}
				data.Items.Clear();
				for (int i = 0; i < model.Items.Count; i++) {
					if (model.Items[i] is DataItem) {
						data.Items.Add(model.Items[i].name);
					}
				}
				if (data.Items.Count > 0) {
					data.Text = (string)data.Items[0];
				}
				ResetPar();
			}
		}

		private bool Apply() {
			bool fitpok = false;
			fitp = new bool[f.p.Length];
			for (int i = 0; i < f.p.Length; i++) {
				fitp[i] = (bool)grid[i + 1, 1].Value;
				fitpok = fitpok || fitp[i];
				f.p[i] = (double)grid[i + 1, 2].Value;
			}
			start.Enabled = (f != null && data != null && fitpok);			
			fit.Function = f;
			fit.Data = dataItem;
			fit.Fitp = fitp;
			return fitpok;
		}

		private void StepInvoke(bool finished) {
			chisq.Text = fit.ChiSquare.ToString();
			for (int i = 0; i < f.p.Length; i++) {
				grid[i+1, 2].Value = f.p[i];
			}
			neval.Text = fit.NEval.ToString();
			if (finished) {
				Q.Text = fit.Q.ToString();
				covar = fit.CovarianceMatrix;
				covariance.Enabled = start.Enabled = true;
				ResetErr();
				graph.Invalidate();
			}
		}

		private void Step(FPlotFit.Fit fit, bool finished) {
			Invoke(invoke, new object[1]{finished});
		}
		
		private void startClick(object sender, System.EventArgs e) {

			Apply();
			start.Enabled = false;
			if (algorithm.Text == "Marquardt") {
				fit.Start(FPlotFit.Algorithm.Marquardt);
			} else if (algorithm.Text == "Nelder & Mead") {
				fit.Start(FPlotFit.Algorithm.NelderMead);
			} else if (algorithm.Text == "Simulated Annealing") {
				fit.Start(FPlotFit.Algorithm.SimulatedAnnealing);
			}
		}

		private void closeClick(object sender, System.EventArgs e) {
			this.Hide();
		}

		private void functionChanged(object sender, System.EventArgs e) {
			ResetPar();
		}

		private void helpClick(object sender, System.EventArgs e) {
			Help.ShowHelp(this, "../help/FPlot.chm", "FitForm.html");
		}

		private void covarClick(object sender, System.EventArgs e) {
			if (covarForm.IsDisposed) covarForm = new CovarianceForm();
			if (covar != null) {
				covarForm.Reset(covar);
				covarForm.Show();
				covarForm.BringToFront();
			}
		}

	}
}
