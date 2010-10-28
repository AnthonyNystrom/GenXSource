using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using FPlotLibrary;

namespace FPlot
{
	/// <summary>
	/// The main form of the Application
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private const string Title = "Function plotter - ";

		class EditMenuItem: MenuItem {
			public PlotItem item;

			public EditMenuItem(MainForm form, PlotItem item) {
				this.item = item;
				this.Click += new EventHandler(form.EditClick);
			}
		}

		private System.Windows.Forms.MenuItem fileItem;
		private System.Windows.Forms.MenuItem editItem;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem exitItem;
		private System.Windows.Forms.MenuItem viewItem;
		private System.Windows.Forms.MenuItem rangeItem;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem bitmapItem;
		private System.Windows.Forms.MenuItem saveAsItem;
		private System.Windows.Forms.MenuItem saveItem;
		private System.Windows.Forms.MenuItem pageSetupItem;
		private System.Windows.Forms.MenuItem printPreviewItem;
		private System.Windows.Forms.MenuItem printItem;
		private System.Windows.Forms.MenuItem optionItem;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.MenuItem openItem;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem newItem;
		private System.Windows.Forms.SaveFileDialog newFileDialog;

		private RangeForm rangeForm;
		private OptionsForm optionsForm;
		private ImageForm imageForm;
		public CompilerOptionsForm compilerForm;
		private LibraryForm libraryForm;
		private FitForm fitForm;
		private AboutForm aboutForm;

		private PageSettings ps = new PageSettings();
		private PrintDocument printDocument;
		private System.Windows.Forms.PrintDialog printDialog;
		private System.Windows.Forms.PrintPreviewDialog printPreviewDialog;
		private System.Windows.Forms.PageSetupDialog pageSetupDialog;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.MenuItem addFunctionItem;
		private System.Windows.Forms.MenuItem editLibraryItem;
		private System.Windows.Forms.MenuItem compilerItem;
		private System.Windows.Forms.MenuItem fitMenu;
		private System.Windows.Forms.MenuItem marquardtItem;
		private FPlotLibrary.GraphControl graph;
		private System.Windows.Forms.TextBox xLabel;
		private System.Windows.Forms.TextBox yLabel;
		private System.Windows.Forms.MenuItem helpMenu;
		private System.Windows.Forms.HelpProvider helpProvider;
		private System.Windows.Forms.MenuItem helpContents;
		private System.Windows.Forms.MenuItem helpAbout;
		private System.Windows.Forms.MenuItem addDataItem;

		public void ResetMenu() {
			for (int i = editItem.MenuItems.Count-1; i > 0; i--) {
				if (editItem.MenuItems[i] is EditMenuItem) editItem.MenuItems.RemoveAt(i);
			}
			EditMenuItem separator = new EditMenuItem(this, null);
			separator.Index = 3;
			separator.Text = "-";

			if (graph.Model.Items.Count > 0) editItem.MenuItems.Add(separator);

			bool data = false, mrqf = false;

			for (int i = 0; i < graph.Model.Items.Count; i++) {
				data = data || graph.Model.Items[i] is DataItem;
				mrqf = mrqf || (graph.Model.Items[i] is Function1D &&
					((Function1D)graph.Model.Items[i]).Fitable());
				EditMenuItem item = new EditMenuItem(this, graph.Model.Items[i]);
				item.Index = i+4;
				item.Text = "Edit " + graph.Model.Items[i].name + "...";
				editItem.MenuItems.Add(item);
			}
			
			marquardtItem.Enabled = data && mrqf;

		}

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			graph.Bar = progressBar;
			graph.xLabel = xLabel;
			graph.yLabel = yLabel;

			printDocument = graph.GetPrintDocument();

			//graph.setRange(-5, -5, 5, 5);
			this.Controls.Add(graph);
			this.Text = Title + System.IO.Path.GetFileNameWithoutExtension(graph.Model.Filename);
			rangeForm = new RangeForm(graph);
			optionsForm = new OptionsForm(graph);
			imageForm = new ImageForm(graph);
			compilerForm = new CompilerOptionsForm(graph);
			libraryForm = new LibraryForm(graph, this);
			fitForm = new FitForm(graph);
			aboutForm = new AboutForm();
			ResetMenu();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )	{
			if( disposing )	{
				imageForm.Dispose();
				if (components != null)	{
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.fileItem = new System.Windows.Forms.MenuItem();
			this.newItem = new System.Windows.Forms.MenuItem();
			this.openItem = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.saveItem = new System.Windows.Forms.MenuItem();
			this.saveAsItem = new System.Windows.Forms.MenuItem();
			this.bitmapItem = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.pageSetupItem = new System.Windows.Forms.MenuItem();
			this.printPreviewItem = new System.Windows.Forms.MenuItem();
			this.printItem = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.exitItem = new System.Windows.Forms.MenuItem();
			this.editItem = new System.Windows.Forms.MenuItem();
			this.addFunctionItem = new System.Windows.Forms.MenuItem();
			this.addDataItem = new System.Windows.Forms.MenuItem();
			this.editLibraryItem = new System.Windows.Forms.MenuItem();
			this.viewItem = new System.Windows.Forms.MenuItem();
			this.rangeItem = new System.Windows.Forms.MenuItem();
			this.optionItem = new System.Windows.Forms.MenuItem();
			this.compilerItem = new System.Windows.Forms.MenuItem();
			this.fitMenu = new System.Windows.Forms.MenuItem();
			this.marquardtItem = new System.Windows.Forms.MenuItem();
			this.helpMenu = new System.Windows.Forms.MenuItem();
			this.helpContents = new System.Windows.Forms.MenuItem();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.newFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.printDialog = new System.Windows.Forms.PrintDialog();
			this.printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
			this.pageSetupDialog = new System.Windows.Forms.PageSetupDialog();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.graph = new FPlotLibrary.GraphControl();
			this.xLabel = new System.Windows.Forms.TextBox();
			this.yLabel = new System.Windows.Forms.TextBox();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.helpAbout = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																						 this.fileItem,
																																						 this.editItem,
																																						 this.viewItem,
																																						 this.fitMenu,
																																						 this.helpMenu});
			// 
			// fileItem
			// 
			this.fileItem.Index = 0;
			this.fileItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																						 this.newItem,
																																						 this.openItem,
																																						 this.menuItem8,
																																						 this.saveItem,
																																						 this.saveAsItem,
																																						 this.bitmapItem,
																																						 this.menuItem5,
																																						 this.pageSetupItem,
																																						 this.printPreviewItem,
																																						 this.printItem,
																																						 this.menuItem6,
																																						 this.exitItem});
			this.fileItem.Text = "&File";
			// 
			// newItem
			// 
			this.newItem.Index = 0;
			this.newItem.Text = "&New...";
			this.newItem.Click += new System.EventHandler(this.newItem_Click);
			// 
			// openItem
			// 
			this.openItem.Index = 1;
			this.openItem.Text = "&Open...";
			this.openItem.Click += new System.EventHandler(this.openItem_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 2;
			this.menuItem8.Text = "-";
			// 
			// saveItem
			// 
			this.saveItem.Index = 3;
			this.saveItem.Text = "&Save";
			this.saveItem.Click += new System.EventHandler(this.saveItem_Click);
			// 
			// saveAsItem
			// 
			this.saveAsItem.Index = 4;
			this.saveAsItem.Text = "Save &as...";
			this.saveAsItem.Click += new System.EventHandler(this.saveAsItem_Click);
			// 
			// bitmapItem
			// 
			this.bitmapItem.Index = 5;
			this.bitmapItem.Text = "Save as &Image...";
			this.bitmapItem.Click += new System.EventHandler(this.bitmapClick);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 6;
			this.menuItem5.Text = "-";
			// 
			// pageSetupItem
			// 
			this.pageSetupItem.Index = 7;
			this.pageSetupItem.Text = "Page set&up...";
			this.pageSetupItem.Click += new System.EventHandler(this.pageSetupClick);
			// 
			// printPreviewItem
			// 
			this.printPreviewItem.Index = 8;
			this.printPreviewItem.Text = "Print pre&view...";
			this.printPreviewItem.Click += new System.EventHandler(this.printPreviewClick);
			// 
			// printItem
			// 
			this.printItem.Index = 9;
			this.printItem.Text = "&Print...";
			this.printItem.Click += new System.EventHandler(this.printClick);
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 10;
			this.menuItem6.Text = "-";
			// 
			// exitItem
			// 
			this.exitItem.Index = 11;
			this.exitItem.Text = "E&xit";
			this.exitItem.Click += new System.EventHandler(this.exitClick);
			// 
			// editItem
			// 
			this.editItem.Index = 1;
			this.editItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																						 this.addFunctionItem,
																																						 this.addDataItem,
																																						 this.editLibraryItem});
			this.editItem.Text = "&Edit";
			// 
			// addFunctionItem
			// 
			this.addFunctionItem.Index = 0;
			this.addFunctionItem.Text = "Add &function...";
			this.addFunctionItem.Click += new System.EventHandler(this.addFunctionClick);
			// 
			// addDataItem
			// 
			this.addDataItem.Index = 1;
			this.addDataItem.Text = "Add &data...";
			this.addDataItem.Click += new System.EventHandler(this.dataClick);
			// 
			// editLibraryItem
			// 
			this.editLibraryItem.Index = 2;
			this.editLibraryItem.Text = "&Edit library...";
			this.editLibraryItem.Click += new System.EventHandler(this.libraryClick);
			// 
			// viewItem
			// 
			this.viewItem.Index = 2;
			this.viewItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																						 this.rangeItem,
																																						 this.optionItem,
																																						 this.compilerItem});
			this.viewItem.Text = "&Options";
			// 
			// rangeItem
			// 
			this.rangeItem.Index = 0;
			this.rangeItem.Text = "Set view &range...";
			this.rangeItem.Click += new System.EventHandler(this.setRangeClick);
			// 
			// optionItem
			// 
			this.optionItem.Index = 1;
			this.optionItem.Text = "&Options...";
			this.optionItem.Click += new System.EventHandler(this.optionItem_Click);
			// 
			// compilerItem
			// 
			this.compilerItem.Index = 2;
			this.compilerItem.Text = "&Compiler options...";
			this.compilerItem.Click += new System.EventHandler(this.compilerClick);
			// 
			// fitMenu
			// 
			this.fitMenu.Index = 3;
			this.fitMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																						this.marquardtItem});
			this.fitMenu.Text = "Fit &data";
			// 
			// marquardtItem
			// 
			this.marquardtItem.Index = 0;
			this.marquardtItem.Text = "&Fit...";
			this.marquardtItem.Click += new System.EventHandler(this.marquardtClick);
			// 
			// helpMenu
			// 
			this.helpMenu.Index = 4;
			this.helpMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																						 this.helpContents,
																																						 this.helpAbout});
			this.helpMenu.Text = "&Help";
			// 
			// helpContents
			// 
			this.helpContents.Index = 0;
			this.helpContents.Text = "&Contents...";
			this.helpContents.Click += new System.EventHandler(this.helpContentsClick);
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "*.fplot";
			this.openFileDialog.Filter = "Plot files (*.fplot)|*.fplot";
			this.openFileDialog.Title = "Open Function";
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "*.fplot";
			this.saveFileDialog.Filter = "Plot files (*.fplot)|*.fplot";
			this.saveFileDialog.Title = "Save Function";
			// 
			// newFileDialog
			// 
			this.newFileDialog.DefaultExt = "*.fplot";
			this.newFileDialog.Filter = "Function files (*.fplot)|*.fplot";
			this.newFileDialog.Title = "New Function";
			// 
			// printPreviewDialog
			// 
			this.printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.printPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
			this.printPreviewDialog.Enabled = true;
			this.printPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog.Icon")));
			this.printPreviewDialog.Location = new System.Drawing.Point(633, 17);
			this.printPreviewDialog.MinimumSize = new System.Drawing.Size(375, 250);
			this.printPreviewDialog.Name = "printPreviewDialog";
			this.printPreviewDialog.TransparencyKey = System.Drawing.Color.Empty;
			this.printPreviewDialog.Visible = false;
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 418);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(688, 22);
			this.statusBar.TabIndex = 0;
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.Location = new System.Drawing.Point(537, 424);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(136, 16);
			this.progressBar.Step = 1;
			this.progressBar.TabIndex = 1;
			// 
			// graph
			// 
			this.graph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.graph.BackColor = System.Drawing.Color.White;
			this.graph.Border = true;
			this.graph.Cursor = System.Windows.Forms.Cursors.Cross;
			this.graph.FixYtoX = false;
			this.graph.Legend = false;
			this.graph.Location = new System.Drawing.Point(0, 0);
			this.graph.Name = "graph";
			this.graph.Size = new System.Drawing.Size(688, 419);
			this.graph.TabIndex = 8;
			this.graph.x0 = -1;
			this.graph.x1 = 1;
			this.graph.xAxis = false;
			this.graph.xGrid = false;
			this.graph.xRaster = true;
			this.graph.xScale = true;
			this.graph.y0 = -1;
			this.graph.y1 = 1;
			this.graph.yAxis = false;
			this.graph.yGrid = false;
			this.graph.yRaster = true;
			this.graph.yScale = true;
			this.graph.z0 = -1;
			this.graph.z1 = 1;
			this.graph.zScale = true;
			// 
			// xLabel
			// 
			this.xLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.xLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.xLabel.Location = new System.Drawing.Point(0, 423);
			this.xLabel.Name = "xLabel";
			this.xLabel.ReadOnly = true;
			this.xLabel.Size = new System.Drawing.Size(133, 13);
			this.xLabel.TabIndex = 9;
			this.xLabel.Text = "x = 0";
			this.xLabel.WordWrap = false;
			// 
			// yLabel
			// 
			this.yLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.yLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.yLabel.Location = new System.Drawing.Point(140, 423);
			this.yLabel.Name = "yLabel";
			this.yLabel.ReadOnly = true;
			this.yLabel.Size = new System.Drawing.Size(133, 13);
			this.yLabel.TabIndex = 10;
			this.yLabel.Text = "y = 0";
			this.yLabel.WordWrap = false;
			// 
			// helpAbout
			// 
			this.helpAbout.Index = 1;
			this.helpAbout.Text = "&About...";
			this.helpAbout.Click += new System.EventHandler(this.helpAbout_Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(688, 440);
			this.Controls.Add(this.yLabel);
			this.Controls.Add(this.xLabel);
			this.Controls.Add(this.graph);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.statusBar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.MinimumSize = new System.Drawing.Size(416, 152);
			this.Name = "MainForm";
			this.Text = "Function Plotter";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[MTAThread]
		static void Main(string[] args) {
			MainForm form = new MainForm();
			if (args.Length > 0 && form.graph.LoadFromFile(args[0])) {
				form.Text = Title + System.IO.Path.GetFileNameWithoutExtension(args[0]);
				form.ResetMenu();
			}
			Application.Run(form);
		}

		private void exitClick(object sender, System.EventArgs e) {
			Application.Exit();
		}

		private void setRangeClick(object sender, System.EventArgs e) {
			if (rangeForm.IsDisposed) {rangeForm = new RangeForm(graph);}
			rangeForm.Reset();
			rangeForm.Show();
			rangeForm.BringToFront();
		}

		private void optionItem_Click(object sender, System.EventArgs e) {
			if (optionsForm.IsDisposed) {optionsForm = new OptionsForm(graph);}
			optionsForm.Reset();
			optionsForm.Show();
			optionsForm.BringToFront();
		}

		private void saveAsItem_Click(object sender, System.EventArgs e) {
			saveFileDialog.FileName = graph.Model.Filename;
			DialogResult res = saveFileDialog.ShowDialog();
			if (res == DialogResult.OK) {
				graph.Model.Filename = saveFileDialog.FileName;
				graph.SaveToFile();
				this.Text = Title + System.IO.Path.GetFileNameWithoutExtension(graph.Model.Filename);
			}
		}

		private void openItem_Click(object sender, System.EventArgs e) {
			DialogResult res = openFileDialog.ShowDialog();
			if (res == DialogResult.OK) {
				if (graph.LoadFromFile(openFileDialog.FileName)) {
					this.Text = Title + System.IO.Path.GetFileNameWithoutExtension(graph.Model.Filename);
					ResetMenu();
				}
			}
		}

		private void newItem_Click(object sender, System.EventArgs e) {
			DialogResult res = newFileDialog.ShowDialog();
			if (res == DialogResult.OK) {
				graph.Model = new GraphModel();
				graph.Model.Filename = newFileDialog.FileName;
				graph.Invalidate();
				this.Text = Title + System.IO.Path.GetFileNameWithoutExtension(graph.Model.Filename);
			}
		}

		private void saveItem_Click(object sender, System.EventArgs e) {
			graph.SaveToFile();
		}

		private void addFunctionClick(object sender, System.EventArgs e) {
			SourceForm sourceForm = new SourceForm(graph, null, this);
			sourceForm.Reset();
			sourceForm.Show();
		}

		private void EditClick(object sender, System.EventArgs e) {
			if (((EditMenuItem)sender).item is FunctionItem) {
				SourceForm sourceForm = new SourceForm(graph,
					(FunctionItem)((EditMenuItem)sender).item, this);
				sourceForm.Reset();
				sourceForm.Show();
			} else if (((EditMenuItem)sender).item is DataItem) {
				DataForm dataForm = new DataForm(graph, (DataItem)((EditMenuItem)sender).item, this);
				dataForm.Reset();
				dataForm.Show();
			}
		}

		private void pageSetupClick(object sender, System.EventArgs e) {
			if (System.Globalization.RegionInfo.CurrentRegion.IsMetric) {
				PageSettings settings = printDocument.DefaultPageSettings;
				settings.Margins.Left = settings.Margins.Left * 254 / 100;
				settings.Margins.Top = settings.Margins.Top * 254 / 100;
				settings.Margins.Right = settings.Margins.Right * 254 / 100;
				settings.Margins.Bottom = settings.Margins.Bottom * 254 / 100;
			}
			pageSetupDialog.Document = printDocument;
			//pageSetupDialog.PageSettings = ps;
			DialogResult res = pageSetupDialog.ShowDialog();

		}

		private void printPreviewClick(object sender, System.EventArgs e) {
			printPreviewDialog.Document = printDocument;
			DialogResult res = printPreviewDialog.ShowDialog();
		}

		private void printClick(object sender, System.EventArgs e) {
			printDialog.Document = printDocument;
			DialogResult res = printDialog.ShowDialog();
			if (res == DialogResult.OK) {
				printDocument.Print();
			}
		}

		private void bitmapClick(object sender, System.EventArgs e) {
			if (imageForm.IsDisposed) {imageForm = new ImageForm(graph);}
			imageForm.Reset();
			imageForm.Show();
			imageForm.BringToFront();
		}

		private void libraryClick(object sender, System.EventArgs e) {
			if (libraryForm.IsDisposed) libraryForm = new LibraryForm(graph, this);
			libraryForm.Reset();
			libraryForm.Show();
			libraryForm.BringToFront();
		}

		private void dataClick(object sender, System.EventArgs e) {
			DataForm dataForm = new DataForm(graph, null, this);
			dataForm.Reset();
			dataForm.Show();
		}

		private void compilerClick(object sender, System.EventArgs e) {
			if (compilerForm.IsDisposed) compilerForm = new CompilerOptionsForm(graph);
			compilerForm.Reset();
			compilerForm.Show();
			compilerForm.BringToFront();
		}

		private void marquardtClick(object sender, System.EventArgs e) {
			if (fitForm.IsDisposed) fitForm = new FitForm(graph);
			fitForm.Reset();
			fitForm.Show();
			fitForm.BringToFront();
		}

		private void helpContentsClick(object sender, System.EventArgs e) {
			Help.ShowHelp(this, "../help/FPlot.chm");
		}

		private void helpAbout_Click(object sender, System.EventArgs e) {
			if (aboutForm.IsDisposed) aboutForm = new AboutForm();
			aboutForm.Show();
			aboutForm.BringToFront();
		}

	}
}
