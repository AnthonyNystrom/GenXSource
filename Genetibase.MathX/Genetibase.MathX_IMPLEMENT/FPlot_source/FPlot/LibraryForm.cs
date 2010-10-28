using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Text;
using FPlotLibrary;

namespace FPlot
{
	/// <summary>
	/// Summary description for LibraryForm.
	/// </summary>
	public class LibraryForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.MenuItem fileMenu;
		private System.Windows.Forms.MenuItem openItem;
		private System.Windows.Forms.MenuItem saveItem;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem closeItem;
		private System.Windows.Forms.MenuItem compileMenu;
		private System.Windows.Forms.MenuItem compileItem;
		private System.Windows.Forms.MenuItem optionsItem;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.MenuItem editMenu;
		private System.Windows.Forms.MenuItem undoItem;
		private System.Windows.Forms.MenuItem cutItem;
		private System.Windows.Forms.MenuItem copyItem;
		private System.Windows.Forms.MenuItem pasteItem;

		private MainForm main;
		private GraphControl graph;
		private System.Windows.Forms.TextBox textBox;
		private ErrorsForm errForm;

		public LibraryForm(GraphControl graph, MainForm main) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.graph = graph;
			this.main = main;
			errForm = new ErrorsForm(graph);
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LibraryForm));
			this.textBox = new System.Windows.Forms.TextBox();
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.fileMenu = new System.Windows.Forms.MenuItem();
			this.openItem = new System.Windows.Forms.MenuItem();
			this.saveItem = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.closeItem = new System.Windows.Forms.MenuItem();
			this.editMenu = new System.Windows.Forms.MenuItem();
			this.undoItem = new System.Windows.Forms.MenuItem();
			this.cutItem = new System.Windows.Forms.MenuItem();
			this.copyItem = new System.Windows.Forms.MenuItem();
			this.pasteItem = new System.Windows.Forms.MenuItem();
			this.compileMenu = new System.Windows.Forms.MenuItem();
			this.compileItem = new System.Windows.Forms.MenuItem();
			this.optionsItem = new System.Windows.Forms.MenuItem();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// textBox
			// 
			this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox.Font = new System.Drawing.Font("Courier New", 8.747663F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textBox.Location = new System.Drawing.Point(0, 0);
			this.textBox.MaxLength = 100000;
			this.textBox.Multiline = true;
			this.textBox.Name = "textBox";
			this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox.Size = new System.Drawing.Size(592, 431);
			this.textBox.TabIndex = 0;
			this.textBox.Text = "";
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																						 this.fileMenu,
																																						 this.editMenu,
																																						 this.compileMenu});
			// 
			// fileMenu
			// 
			this.fileMenu.Index = 0;
			this.fileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																						 this.openItem,
																																						 this.saveItem,
																																						 this.menuItem1,
																																						 this.closeItem});
			this.fileMenu.Text = "&File";
			// 
			// openItem
			// 
			this.openItem.Index = 0;
			this.openItem.Text = "&Open...";
			this.openItem.Click += new System.EventHandler(this.openClick);
			// 
			// saveItem
			// 
			this.saveItem.Index = 1;
			this.saveItem.Text = "&Save as...";
			this.saveItem.Click += new System.EventHandler(this.saveClick);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 2;
			this.menuItem1.Text = "-";
			// 
			// closeItem
			// 
			this.closeItem.Index = 3;
			this.closeItem.Text = "&Close";
			this.closeItem.Click += new System.EventHandler(this.closeClick);
			// 
			// editMenu
			// 
			this.editMenu.Index = 1;
			this.editMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																						 this.undoItem,
																																						 this.cutItem,
																																						 this.copyItem,
																																						 this.pasteItem});
			this.editMenu.Text = "&Edit";
			// 
			// undoItem
			// 
			this.undoItem.Index = 0;
			this.undoItem.Text = "&Undo";
			this.undoItem.Click += new System.EventHandler(this.undoClick);
			// 
			// cutItem
			// 
			this.cutItem.Index = 1;
			this.cutItem.Text = "Cu&t";
			this.cutItem.Click += new System.EventHandler(this.cutClick);
			// 
			// copyItem
			// 
			this.copyItem.Index = 2;
			this.copyItem.Text = "&Copy";
			this.copyItem.Click += new System.EventHandler(this.copyClick);
			// 
			// pasteItem
			// 
			this.pasteItem.Index = 3;
			this.pasteItem.Text = "&Paste";
			this.pasteItem.Click += new System.EventHandler(this.pasteClick);
			// 
			// compileMenu
			// 
			this.compileMenu.Index = 2;
			this.compileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																																								this.compileItem,
																																								this.optionsItem});
			this.compileMenu.Text = "&Compile";
			// 
			// compileItem
			// 
			this.compileItem.Index = 0;
			this.compileItem.Text = "&Compile...";
			this.compileItem.Click += new System.EventHandler(this.compileClick);
			// 
			// optionsItem
			// 
			this.optionsItem.Index = 1;
			this.optionsItem.Text = "Compiler &options...";
			this.optionsItem.Click += new System.EventHandler(this.optionsClick);
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "Source files (*.cs; *.c; *.cpp; *.txt)|*.cs;*.c;*.cpp;*.txt|All files|*.*";
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "cs";
			this.saveFileDialog.Filter = "Source files (*.cs; *.c; *.cpp; *.txt)|*.cs;*.c;*.cpp;*.txt|All files|*.*";
			// 
			// LibraryForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(592, 431);
			this.Controls.Add(this.textBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "LibraryForm";
			this.Text = "Library source";
			this.ResumeLayout(false);

		}
		#endregion

		public void Reset() {
			textBox.Text = graph.Model.Library.source;
		}

		public void Apply() {
			graph.Model.Library.source = textBox.Text;
		}

		private void closeClick(object sender, System.EventArgs e) {
			Apply();
			this.Hide();
		}

		private void openClick(object sender, System.EventArgs e) {
			DialogResult res = openFileDialog.ShowDialog();
			if (res == DialogResult.OK) {
				using (FileStream stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read)) {
					using (StreamReader r = new StreamReader(stream, true)) {
						string line;
						StringBuilder sb;
						ArrayList list = new ArrayList();
						string[] lines;
						while ((line = r.ReadLine()) != null) {
							sb = new StringBuilder(line);
							sb.Replace("\t", "  ");
							list.Add(sb.ToString());
						}
						lines = new string[list.Count];
						for (int i = 0; i < lines.Length; i++) {
							lines[i] = (string)list[i];
						}
						textBox.Lines = lines;
					}
				}
			}
		}

		private void saveClick(object sender, System.EventArgs e) {
			Apply();
			string[] lines = textBox.Lines; 
			saveFileDialog.FileName = "FPlotLibrary.cs";
			DialogResult res = saveFileDialog.ShowDialog();
			if (res == DialogResult.OK) {
				using (FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write)) {
					using (StreamWriter w = new StreamWriter(stream, Encoding.ASCII)) {
						for (int l = 0; l < lines.Length; l++) {
							w.WriteLine(lines[l]);
						}
					}
				}
			}
		}

		private void compileClick(object sender, System.EventArgs e) {
			Apply();
			graph.Model.Library.Compile(true);
			if (errForm.IsDisposed) errForm = new ErrorsForm(graph);
			errForm.Reset();
			errForm.Show();
			errForm.BringToFront();
		}

		private void optionsClick(object sender, System.EventArgs e) {
			CompilerOptionsForm compilerForm = main.compilerForm;
			if (compilerForm.IsDisposed) compilerForm = new CompilerOptionsForm(graph);
			compilerForm.Reset();
			compilerForm.Show();
			compilerForm.BringToFront();
		}

		private void undoClick(object sender, System.EventArgs e) {
			textBox.Undo();
		}

		private void cutClick(object sender, System.EventArgs e) {
			textBox.Cut();
		}

		private void copyClick(object sender, System.EventArgs e) {
			textBox.Copy();
		}

		private void pasteClick(object sender, System.EventArgs e) {
			textBox.Paste();
		}
	}
}
