using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using SourceGrid2;
using FPlotLibrary;

namespace FPlot
{
	/// <summary>
	/// Summary description for DataForm.
	/// </summary>
	public class DataForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button deleteButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.TextBox name;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label colorLabel;
		private System.Windows.Forms.Button colorButton;
		private System.Windows.Forms.ColorDialog colorDialog;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		MainForm mainform;
		DataItem item, temp;
		GraphControl graph;
		static int index;
		bool deleted = false, newitem = false;
		LoadDataForm loadForm;
		private System.Windows.Forms.CheckBox line;
		private System.Windows.Forms.Button fileButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox lineWidth;
		private System.Windows.Forms.ComboBox lineStyle;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox marks;
		private System.Windows.Forms.Button help;
		private FPlot.DataGrid grid;
		private System.Windows.Forms.GroupBox groupBox1;
		private Pen pen = new Pen(Color.Black, 2);

		public DataForm(GraphControl graph, DataItem item, MainForm mainform)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.graph = graph;
			if (item == null) {
				newitem = true;
				item = new DataItem();
				item.name = "Data" + index++;
				graph.Model.Items.Add(item);
			}
			this.item = item;
			temp = new DataItem();
			temp.model = graph.Model;
			this.mainform = mainform;
			loadForm = new LoadDataForm(this, item);
			mainform.ResetMenu();
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
				if(components != null) {
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DataForm));
			this.deleteButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.applyButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.name = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.colorLabel = new System.Windows.Forms.Label();
			this.colorButton = new System.Windows.Forms.Button();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.line = new System.Windows.Forms.CheckBox();
			this.marks = new System.Windows.Forms.CheckBox();
			this.fileButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.lineWidth = new System.Windows.Forms.TextBox();
			this.lineStyle = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.help = new System.Windows.Forms.Button();
			this.grid = new FPlot.DataGrid();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// deleteButton
			// 
			this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.deleteButton.Location = new System.Drawing.Point(216, 476);
			this.deleteButton.Name = "deleteButton";
			this.deleteButton.Size = new System.Drawing.Size(64, 24);
			this.deleteButton.TabIndex = 26;
			this.deleteButton.Text = "Delete";
			this.deleteButton.Click += new System.EventHandler(this.deleteClick);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancelButton.Location = new System.Drawing.Point(144, 476);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(64, 24);
			this.cancelButton.TabIndex = 25;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelClick);
			// 
			// applyButton
			// 
			this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.applyButton.Location = new System.Drawing.Point(72, 476);
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size(64, 24);
			this.applyButton.TabIndex = 24;
			this.applyButton.Text = "Apply";
			this.applyButton.Click += new System.EventHandler(this.applyClick);
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.okButton.Location = new System.Drawing.Point(0, 476);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(64, 24);
			this.okButton.TabIndex = 23;
			this.okButton.Text = "Ok";
			this.okButton.Click += new System.EventHandler(this.okClick);
			// 
			// name
			// 
			this.name.Location = new System.Drawing.Point(48, 8);
			this.name.Name = "name";
			this.name.Size = new System.Drawing.Size(224, 20);
			this.name.TabIndex = 28;
			this.name.Text = "";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 23);
			this.label1.TabIndex = 27;
			this.label1.Text = "Name:";
			// 
			// colorLabel
			// 
			this.colorLabel.Location = new System.Drawing.Point(72, 16);
			this.colorLabel.Name = "colorLabel";
			this.colorLabel.Size = new System.Drawing.Size(24, 24);
			this.colorLabel.TabIndex = 31;
			// 
			// colorButton
			// 
			this.colorButton.Location = new System.Drawing.Point(8, 16);
			this.colorButton.Name = "colorButton";
			this.colorButton.Size = new System.Drawing.Size(56, 24);
			this.colorButton.TabIndex = 30;
			this.colorButton.Text = "Color...";
			this.colorButton.Click += new System.EventHandler(this.colorClick);
			// 
			// line
			// 
			this.line.Location = new System.Drawing.Point(8, 72);
			this.line.Name = "line";
			this.line.Size = new System.Drawing.Size(136, 24);
			this.line.TabIndex = 34;
			this.line.Text = "Join points with a line";
			// 
			// marks
			// 
			this.marks.Location = new System.Drawing.Point(8, 48);
			this.marks.Name = "marks";
			this.marks.Size = new System.Drawing.Size(112, 24);
			this.marks.TabIndex = 35;
			this.marks.Text = "Draw error marks";
			// 
			// fileButton
			// 
			this.fileButton.Location = new System.Drawing.Point(8, 152);
			this.fileButton.Name = "fileButton";
			this.fileButton.Size = new System.Drawing.Size(96, 23);
			this.fileButton.TabIndex = 36;
			this.fileButton.Text = "Load from file...";
			this.fileButton.Click += new System.EventHandler(this.loadClick);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(160, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 23);
			this.label2.TabIndex = 40;
			this.label2.Text = "Line Width:";
			// 
			// lineWidth
			// 
			this.lineWidth.Location = new System.Drawing.Point(224, 40);
			this.lineWidth.Name = "lineWidth";
			this.lineWidth.Size = new System.Drawing.Size(40, 20);
			this.lineWidth.TabIndex = 39;
			this.lineWidth.Text = "";
			// 
			// lineStyle
			// 
			this.lineStyle.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.lineStyle.Location = new System.Drawing.Point(224, 16);
			this.lineStyle.Name = "lineStyle";
			this.lineStyle.Size = new System.Drawing.Size(80, 21);
			this.lineStyle.TabIndex = 38;
			this.lineStyle.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.DrawItem);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(160, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 23);
			this.label3.TabIndex = 41;
			this.label3.Text = "Line Style:";
			// 
			// help
			// 
			this.help.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.help.Location = new System.Drawing.Point(288, 476);
			this.help.Name = "help";
			this.help.TabIndex = 42;
			this.help.Text = "Help...";
			this.help.Click += new System.EventHandler(this.helpClick);
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
			this.grid.GridToolTipActive = true;
			this.grid.Location = new System.Drawing.Point(0, 184);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(368, 288);
			this.grid.SpecialKeys = SourceGrid2.GridSpecialKeys.Default;
			this.grid.TabIndex = 43;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.lineWidth);
			this.groupBox1.Controls.Add(this.colorLabel);
			this.groupBox1.Controls.Add(this.lineStyle);
			this.groupBox1.Controls.Add(this.colorButton);
			this.groupBox1.Controls.Add(this.line);
			this.groupBox1.Controls.Add(this.marks);
			this.groupBox1.Location = new System.Drawing.Point(8, 40);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(352, 104);
			this.groupBox1.TabIndex = 44;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Style";
			// 
			// DataForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(368, 500);
			this.Controls.Add(this.grid);
			this.Controls.Add(this.help);
			this.Controls.Add(this.name);
			this.Controls.Add(this.fileButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.deleteButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.applyButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(560, 2000);
			this.MinimumSize = new System.Drawing.Size(376, 352);
			this.Name = "DataForm";
			this.Text = "Edit data ";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
		
		public void Reset() {
			name.Text = item.name;
			colorLabel.BackColor = item.Color;
			colorDialog.Color = item.Color;
			lineStyle.SelectedIndex = (int)item.lineStyle;
			lineWidth.Text = item.lineWidth.ToString();
			line.Checked = item.lines;
			marks.Checked = item.marks;
			item.x.deepCopy = item.y.deepCopy = item.dx.deepCopy = item.dy.deepCopy = true;
			grid.LoadDataSource(item);
			//grid.AutoSize(); // this was too slow for big amounts of data.
		}

		public void Apply() {
			item.name = name.Text;
			item.Color = colorLabel.BackColor;
			item.lineStyle = (DashStyle)lineStyle.SelectedIndex;
			try {
				item.lineWidth = float.Parse(lineWidth.Text);
			} catch {item.lineWidth = 1;}
			item.lines = line.Checked;
			item.marks = marks.Checked;
			item.Compile(true);
			Reset();
			if (!deleted) graph.Invalidate();
			mainform.ResetMenu();
		}

		private void okClick(object sender, System.EventArgs e) {
			Apply();
			this.Hide();
		}

		private void applyClick(object sender, System.EventArgs e) {
			Apply();
		}

		private void cancelClick(object sender, System.EventArgs e) {
			if (newitem) {
				graph.Model.Items.Remove(item);
				mainform.ResetMenu();
			}
			graph.Invalidate();
			this.Hide();
		}

		private void deleteClick(object sender, System.EventArgs e) {
			graph.Model.Items.Remove(item);
			Apply();
			deleted = true;
		}

		private void colorClick(object sender, System.EventArgs e) {
			colorDialog.ShowDialog();
			colorLabel.BackColor = colorDialog.Color;
			lineStyle.Invalidate();
		}

		private void loadClick(object sender, System.EventArgs e) {
			Apply();
			if (loadForm.IsDisposed) loadForm = new LoadDataForm(this, item);
			loadForm.Reset();
			loadForm.Show();
			loadForm.BringToFront();
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
			Help.ShowHelp(this, "../help/FPlot.chm", "DataForm.html");
		}

	}

	public class DataGrid : SourceGrid2.GridVirtual {
		private SourceGrid2.Cells.Virtual.CellVirtual colHeaderCell, rowHeaderCell, headerCell, x, y, dx, dy,
			formula;
		private DataItem data;

		public virtual void LoadDataSource(DataItem data) {
			this.data = data;

			Redim(data.Length + M + 1, 5);

			//Col Header Cell Template
			colHeaderCell = new CellColumnHeaderTemplate();
			colHeaderCell.BindToGrid(this);

			//Row Header Cell Template
			rowHeaderCell = new CellRowHeaderTemplate();
			rowHeaderCell.BindToGrid(this);

			//Header Cell Template (0,0 cell)
			headerCell = new CellHeaderTemplate();
			headerCell.BindToGrid(this);

			//Data Cell Template
			x = new CellXDataTemplate(data);
			y = new CellYDataTemplate(data);
			dx = new CellDXDataTemplate(data);
			dy = new CellDYDataTemplate(data);
			x.BindToGrid(this);
			y.BindToGrid(this);
			dx.BindToGrid(this);
			dy.BindToGrid(this);

			//Formula Cell Template
			formula = new CellFormulaTemplate(data);
			formula.BindToGrid(this);

			RefreshCellStyle();
		}

		public override SourceGrid2.Cells.ICellVirtual GetCell(int row, int col) {
			try {
				if (data != null) {
					if (row < 1 && col < 1) return headerCell;
					else if (row < 1) return colHeaderCell;
					else if (col < 1) return rowHeaderCell;
					else if (row < 2) return formula;
					else if (col == 1) return x;
					else if (col == 2) return y;
					else if (col == 3) return dx;
					else if (col == 4) return dy;
					else return null;
				}
				else
					return null;
			}
			catch(Exception err) {
				System.Diagnostics.Debug.Assert(false, err.Message);
				return null;
			}		
		}

		private void RefreshCellStyle() {
			x.DataModel.EnableEdit = data.x.source == null;
			y.DataModel.EnableEdit = data.y.source == null;
			dx.DataModel.EnableEdit = data.dx.source == null;
			dy.DataModel.EnableEdit = data.dy.source == null;
		}


		#region Cell class

		const int M = 2;

		public class CellXDataTemplate : SourceGrid2.Cells.Virtual.CellVirtual {
			private DataItem data;
			public CellXDataTemplate(DataItem data) {
				this.data = data;
				DataModel = SourceGrid2.Utility.CreateDataModel(typeof(double));
			}
			public override object GetValue(SourceGrid2.Position pos) {
				if (pos.Column == 1) {
					if (pos.Row == Grid.RowsCount-1) return null;
					else return data.x[pos.Row-M];
				} else throw new IndexOutOfRangeException("invalid column index: " + pos.Column);
			}

			public override void SetValue(SourceGrid2.Position pos, object x) {
				if (pos.Column == 1) {
					if (pos.Row == Grid.RowsCount-1) {
						data.Length = Grid.RowsCount-M;
						Grid.RowsCount += 1;
					}
					data.x[pos.Row-M] = (double)x;
				} else throw new IndexOutOfRangeException("invalid column index: " + pos.Column);
				OnValueChanged(new SourceGrid2.PositionEventArgs(pos, this));
			}
		}

		public class CellYDataTemplate : SourceGrid2.Cells.Virtual.CellVirtual {
			private DataItem data;
			public CellYDataTemplate(DataItem data) {
				this.data = data;
				DataModel = SourceGrid2.Utility.CreateDataModel(typeof(double));
			}
			public override object GetValue(SourceGrid2.Position pos) {
				if (pos.Column == 2) {
					if (pos.Row == Grid.RowsCount-1) return null;
					else return data.y[pos.Row-M];
				}
				else throw new IndexOutOfRangeException("invalid column index: " + pos.Column);
			}

			public override void SetValue(SourceGrid2.Position pos, object x) {
				if (pos.Column == 2) {
					if (pos.Row == Grid.RowsCount-1) {
						data.Length = Grid.RowsCount-M;
						Grid.RowsCount += 1;
					}
					data.y[pos.Row-M] = (double)x;
				} else throw new IndexOutOfRangeException("invalid column index: " + pos.Column);
				OnValueChanged(new SourceGrid2.PositionEventArgs(pos, this));
			}
		}
		public class CellDXDataTemplate : SourceGrid2.Cells.Virtual.CellVirtual {
			private DataItem data;
			public CellDXDataTemplate(DataItem data) {
				this.data = data;
				DataModel = SourceGrid2.Utility.CreateDataModel(typeof(double));
			}
			public override object GetValue(SourceGrid2.Position pos) {
				if (pos.Column == 3) {
					if (pos.Row == Grid.RowsCount-1) return null;
					else return data.dx[pos.Row-M];
				} else throw new IndexOutOfRangeException("invalid column index: " + pos.Column);
			}

			public override void SetValue(SourceGrid2.Position pos, object x) {
				if (pos.Column == 3) {
					if (pos.Row == Grid.RowsCount-1) {
						data.Length = Grid.RowsCount-M;
						Grid.RowsCount += 1;
					}
					data.dx[pos.Row-M] = (double)x;
				} else throw new IndexOutOfRangeException("invalid column index: " + pos.Column);
				OnValueChanged(new SourceGrid2.PositionEventArgs(pos, this));
			}
		}
		public class CellDYDataTemplate : SourceGrid2.Cells.Virtual.CellVirtual {
			private DataItem data;
			public CellDYDataTemplate(DataItem data) {
				this.data = data;
				DataModel = SourceGrid2.Utility.CreateDataModel(typeof(double));
			}
			public override object GetValue(SourceGrid2.Position pos) {
				if (pos.Column == 4) {
					if (pos.Row == Grid.RowsCount-1) return null;
					else return data.dy[pos.Row-M];
				} else throw new IndexOutOfRangeException("invalid column index: " + pos.Column);
			}

			public override void SetValue(SourceGrid2.Position pos, object x) {
				if (pos.Column == 4) {
					if (pos.Row == Grid.RowsCount-1) {
						data.Length = Grid.RowsCount-M;
						Grid.RowsCount += 1;
					} 
					data.dy[pos.Row-M] = (double)x;
				} else throw new IndexOutOfRangeException("invalid column index: " + pos.Column);
				OnValueChanged(new SourceGrid2.PositionEventArgs(pos, this));
			}
		}

		public class CellFormulaTemplate : SourceGrid2.Cells.Virtual.CellVirtual {
			private DataItem data;
			public CellFormulaTemplate(DataItem data) {
				this.data = data;
				DataModel = SourceGrid2.Utility.CreateDataModel(typeof(string));
			}

			bool EmptyString(string s) {
				if (s == null) return true; 
				bool b = true;
				for (int i = 0; i < s.Length; i++) {
					b = b && (s[i] == ' ');
				}
				return b;
			}

			string Source(string s) {
				if (s == null) return "";
				else return s;
			}

			public override object GetValue(SourceGrid2.Position pos) {
				switch (pos.Column) {
				case 1:
					return Source(data.x.source);
				case 2:
					return Source(data.y.source);
				case 3:
					return Source(data.dx.source);
				case 4:
					return Source(data.dy.source);
				default:
					throw new IndexOutOfRangeException("invalid column index: " + pos.Column);
				}
			}

			private void Compile() {
				data.Compile(true);
				if (!data.compiled) {
					string msg = "There are errors in the formulas:\n";
					for (int i = 0; i < data.errors.Length; i++) {
						msg += data.errors[i] + "\n";
					}
					MessageBox.Show(msg, "Compile error"); 
				}
			}

			public override void SetValue(SourceGrid2.Position pos, object x) {
				string s = (string)x;
				if (EmptyString(s)) s = null;
				switch (pos.Column) {
				case 1:
					data.x.source = s;
					Compile();
					break;
				case 2:
					data.y.source = s;
					Compile();
					break;
				case 3:
					data.dx.source = s;
					Compile();
					break;
				case 4:
					data.dy.source = s;
					Compile();
					break;
				default:
					throw new IndexOutOfRangeException("invalid column index: " + pos.Column);
				}
				((DataGrid)Grid).RefreshCellStyle();
				OnValueChanged(new SourceGrid2.PositionEventArgs(pos, this));
			}
		}

		private class CellColumnHeaderTemplate: SourceGrid2.Cells.Virtual.ColumnHeader {
			public override object GetValue(SourceGrid2.Position pos) {
				switch (pos.Column) {
				case 1:
					return "x[n]";
				case 2:
					return "y[n]";
				case 3:
					return "dx[n]";
				case 4:
					return "dy[n]";
				default:
					throw new IndexOutOfRangeException("invalid column index: " + pos.Column);
				}
			}
			public override void SetValue(SourceGrid2.Position p_Position, object p_Value) {
				throw new ApplicationException("Cannot change this kind of cell");
			}

			public override SourceGrid2.SortStatus GetSortStatus(SourceGrid2.Position p_Position) {
				return new SourceGrid2.SortStatus (SourceGrid2.GridSortMode.None, false);
			}

			public override void SetSortMode(SourceGrid2.Position p_Position, SourceGrid2.GridSortMode p_Mode) {
			}		
		}

		private class CellRowHeaderTemplate: SourceGrid2.Cells.Virtual.RowHeader {	
			public override object GetValue(SourceGrid2.Position pos) {
				if (pos.Row == 1) return "=";
				else if (pos.Row == Grid.RowsCount-1) return "*";
				else return pos.Row-2;
			}
			public override void SetValue(SourceGrid2.Position p_Position, object p_Value) {
				throw new ApplicationException("Cannot change this kind of cell");
			}		
		}

		private class CellHeaderTemplate: SourceGrid2.Cells.Virtual.Header {
			public override object GetValue(SourceGrid2.Position p_Position) {
				return "n";
			}
			public override void SetValue(SourceGrid2.Position p_Position, object p_Value) {
				throw new ApplicationException("Cannot change this kind of cell");
			}		
		}
		#endregion
	}
}
