// $Header: /Ink/Scribble/Test/Form1.cs 68    9/22/05 7:08p Todd.hardman $
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using Agilix.Ink;
using Agilix.Ink.Scribble;

namespace Sample
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private const string TITLE = "Agilix Scribbler";

		private Agilix.Ink.Scribble.ScribbleBox fScribble;
		private string fFileName;
		private string fFilePath;
		private int fFileNumber = 0;
		private int fPrintPage;
		private int fPrintRectangle;
		private ArrayList fPrintPhysicalRectangles = new ArrayList();
		private System.Windows.Forms.Timer fUpdateFileNameTimer;
		private EventHandler TitlePageChanged;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.Icon = new Icon(typeof(Form1), "App.ico");

			NewFileName();
			Document document = new Document();
			document.Pages.Add(new Page(document, Stationery.CreateStockStationeryWithTitle(StationeryStockType.Standard, 19050)));
			fScribble.Document = document;

			// Add the menu
			System.Windows.Forms.MainMenu menu = new MainMenu();
			menu.MenuItems.Add("File", new MenuItem[] 
				{
					new MenuItem("New", new EventHandler(File_New), Shortcut.CtrlN),
					new MenuItem("Open...", new EventHandler(File_Open), Shortcut.CtrlO),
					new MenuItem("Import...", new EventHandler(File_Import)),
					new MenuItem("-"),
					new MenuItem("Save", new EventHandler(File_Save), Shortcut.CtrlS),
					new MenuItem("Save as...", new EventHandler(File_SaveAs)),
					new MenuItem("-"),
					new MenuItem("Print...", new EventHandler(File_Print), Shortcut.CtrlP),
					new MenuItem("Print Preview 1", new EventHandler(File_PrintPreview1)),
					new MenuItem("Print Preview 2", new EventHandler(File_PrintPreview2)),
					new MenuItem("-"),
					new MenuItem("Exit", new EventHandler(File_Exit))
				});
			menu.MenuItems.Add("Edit", new MenuItem[] 
				{
					new MenuItem("Undo", new EventHandler(Edit_Undo), Shortcut.CtrlZ),
					new MenuItem("Redo", new EventHandler(Edit_Redo), Shortcut.CtrlY),
					new MenuItem("-"),
					new MenuItem("Cut", new EventHandler(Edit_Cut), Shortcut.CtrlX),
					new MenuItem("Copy", new EventHandler(Edit_Copy), Shortcut.CtrlC),
					new MenuItem("Copy as Text...", new EventHandler(Edit_CopyAsText)),
					new MenuItem("Paste", new EventHandler(Edit_Paste), Shortcut.CtrlV),
					new MenuItem("Delete", new EventHandler(Edit_Delete), Shortcut.Del),
					new MenuItem("-"),
					new MenuItem("Find...", new EventHandler(Edit_Find), Shortcut.CtrlF),
					new MenuItem("Find Next", new EventHandler(Edit_FindNext), Shortcut.F3),
					new MenuItem("-"),
					new MenuItem("Define Flags...", new EventHandler(Edit_Flags)),
					new MenuItem("Bring to front", new EventHandler(Edit_BringToFront)),
					new MenuItem("Send to back", new EventHandler(Edit_SendToBack)),
					new MenuItem("Group", new EventHandler(Edit_Group)),
					new MenuItem("Ungroup", new EventHandler(Edit_Ungroup)),
					new MenuItem("-"),
					new MenuItem("Convert to Text", new EventHandler(Edit_ConvertToText)),
					new MenuItem("Format...", new EventHandler(Edit_FormatInk)),
					new MenuItem("-"),
					new MenuItem("Fit to Page", new EventHandler(Edit_FitToPage)),
					new MenuItem("Fit to Width", new EventHandler(Edit_FitToWidth)),
					new MenuItem("Fit to Height", new EventHandler(Edit_FitToHeight))
				});
			menu.MenuItems[1].Popup += new EventHandler(EditMenu_Popup);
			menu.MenuItems.Add("Insert", new MenuItem[]
				{
					new MenuItem("Rectangle", new EventHandler(Insert_Rectangle)),
					new MenuItem("Ellipse", new EventHandler(Insert_Ellipse)),
					new MenuItem("Triangle", new EventHandler(Insert_Triangle)),
					new MenuItem("Yield Sign", new EventHandler(Insert_Yield)),
					new MenuItem("Diamond", new EventHandler(Insert_Diamond)),
					new MenuItem("Star", new EventHandler(Insert_Star)),
					new MenuItem("-"),
					new MenuItem("Line", new EventHandler(Insert_Line)),
					new MenuItem("Arrow", new EventHandler(Insert_Arrow)),
					new MenuItem("-"),
					new MenuItem("Picture...", new EventHandler(Insert_Picture))

				});
			menu.MenuItems.Add("Paper", new MenuItem[] 
				{
					new MenuItem("Blank", new EventHandler(Paper_Blank)),
					new MenuItem("-"),
					new MenuItem("Narrow", new EventHandler(Paper_Narrow)),
					new MenuItem("College", new EventHandler(Paper_College)),
					new MenuItem("Standard", new EventHandler(Paper_Standard)),
					new MenuItem("Wide", new EventHandler(Paper_Wide)),
					new MenuItem("-"),
					new MenuItem("Small Grid", new EventHandler(Paper_SmallGrid)),
					new MenuItem("Grid", new EventHandler(Paper_Grid))
				});
			menu.MenuItems.Add("Help", new MenuItem[] 
				{
					new MenuItem("About Agilix InfiNotes...", new EventHandler(Help_About))
				});

			Menu = menu;

			TitlePageChanged = new EventHandler(This_RecognitionChanged);

			// Check for shortcuts to flags
			fScribble.Scribble.KeyDown += new KeyEventHandler(Scribble_KeyDown);
			fScribble.Scribble.Changed += new EventHandler(Scribble_Changed);
			fScribble.Document.RecognitionChanged += new PageEventHandler(Scribble_RecognitionChanged);

			fUpdateFileNameTimer = new Timer(components);
			fUpdateFileNameTimer.Interval = 1000;
			fUpdateFileNameTimer.Tick += new EventHandler(UpdateFileNameTimer_Tick);

			// Default to mouse mode when not on tablet
			Microsoft.Ink.Recognizers recos = new Microsoft.Ink.Recognizers();
			if (recos.Count == 0)
			{
				fScribble.StylusMode = new MouseMode();
			}

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing"><b>true</b> to release both managed and unmanaged resources; <b>false</b> to release only unmanaged resources.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
			this.components = new System.ComponentModel.Container();
			this.fScribble = new Agilix.Ink.Scribble.ScribbleBox();
			this.SuspendLayout();
			// 
			// fScribble
			// 
			this.fScribble.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.fScribble.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fScribble.Location = new System.Drawing.Point(0, 0);
			this.fScribble.Name = "fScribble";
			this.fScribble.Size = new System.Drawing.Size(735, 566);
			this.fScribble.TabColor = System.Drawing.SystemColors.Control;
			this.fScribble.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(735, 566);
			this.Controls.Add(this.fScribble);
			this.Name = "Form1";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args) 
		{
			// Use XP styles
			Application.EnableVisualStyles();
			Application.DoEvents(); 

			Agilix.Html.HttpProtocolSession.RegisterNameSpace();

			Form1 mainForm = new Form1();

			if (args.Length == 1)
			{
				mainForm.Open(args[0]);
			}
			Application.Run(mainForm);

			Agilix.Html.HttpProtocolSession.UnregisterNameSpace();
		}

		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus (e);
			fScribble.Focus();
		}

		#region Load/Save user settings
		private void Form1_Load(object sender, System.EventArgs e)
		{
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Agilix\Ink\Scribble");
			fScribble.ColorCollectionData = key.GetValue("Colors", "").ToString();
			fScribble.WritingInstrumentsData = key.GetValue("WritingInstruments", "").ToString();
			fScribble.Toolbar.RefreshPenMenu();
			fScribble.FlagsData = key.GetValue("Flags", "").ToString();
			fScribble.Toolbar.RefreshFlagsToolbar();
		}

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Agilix\Ink\Scribble");
			key.SetValue("Colors", fScribble.ColorCollectionData);
			key.SetValue("WritingInstruments", fScribble.WritingInstrumentsData);
			key.SetValue("Flags", fScribble.FlagsData);
			key.Close();

			if (CheckDirty() == DialogResult.Cancel)
			{
				e.Cancel = true;
			}
		}
		#endregion

		#region File Menu
		private void File_New(object sender, System.EventArgs e)
		{
			if (CheckDirty() == DialogResult.Cancel)
			{
				return;
			}
			NewFileName();
			Document document = new Document();
			document.Pages.Add(new Page(document, Stationery.CreateStockStationeryWithTitle(StationeryStockType.Standard, 19050)));
			fScribble.Document = document;
		}
		private void File_Open(object sender, System.EventArgs e)
		{
			if (CheckDirty() == DialogResult.Cancel)
			{
				return;
			}
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Title = "Open " + TITLE + " File";
			dialog.CheckFileExists = true;
			dialog.Filter = TITLE + " files (*.ant)|*.ant|All files (*.*)|*.*" ;
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				Open(dialog.FileName);
			}
		}
		private void File_Import(object sender, System.EventArgs e)
		{
			if (CheckDirty() == DialogResult.Cancel)
			{
				return;
			}
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Title = "Import File";
			dialog.CheckFileExists = true;
//			dialog.Filter = "Windows Journal Note (*.jnt)|*.jnt|Ink Serialized Format (*.isf)|*.isf|Image|*.bmp;*.gif;*.png;*.jpg;*.wmf|Rich text (*.rtf)|*.rtf|Plain text (*.txt)|*.txt|Print Jobs (*.spl)|*.spl|HTML (*.htm; *.html)|*.htm;*.html|MHTML (*.mht)|*.mht";
			dialog.Filter = "Windows Journal Note (*.jnt)|*.jnt|Ink Serialized Format (*.isf)|*.isf|Image|*.bmp;*.gif;*.png;*.jpg;*.wmf|Rich text (*.rtf)|*.rtf|Plain text (*.txt)|*.txt|Print Jobs (*.spl)|*.spl";
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				Import(dialog.FileName, dialog.FilterIndex);
			}
		}
		private void File_Save(object sender, System.EventArgs e)
		{
			if (fFilePath == null || fFilePath.Length == 0)
			{
				File_SaveAs(sender, e);
			}
			else
			{
				Save(fFilePath);
			}
		}
		private void File_SaveAs(object sender, System.EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.Title = "Save As";
			dialog.OverwritePrompt = true;
			dialog.CheckPathExists = true;
			dialog.DefaultExt = "ant";
			if (fFilePath == null || fFilePath.Length == 0)
			{
				fScribble.Document.WaitForBackgroundAnalysis();
				UpdateFileName();
				dialog.FileName = fFileName;
			}
			else
			{
				dialog.FileName = fFilePath;
			}
			dialog.Filter = TITLE + " file (*.ant)|*.ant|Ink Serialized Format (*.isf)|*.isf|MHTML (*.mht)|*.mht|Image (*.png)|*.png|Rich text (*.rtf)|*.rtf|Plain text (*.txt)|*.txt";
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				fFileName = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName);
				Text = fFileName + " - " + TITLE;

				if (dialog.FilterIndex > 1)
				{
					Export(dialog.FileName, dialog.FilterIndex);
				}
				else
				{
					fFilePath = dialog.FileName;
					Save(fFilePath);
				}
			}
		}
		private void File_Print(object sender, System.EventArgs e)
		{
			System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();
			doc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PrintPage);
			fScribble.Document.WaitForBackgroundAnalysis();
			UpdateFileName();
			doc.DocumentName = fFileName;
			doc.PrinterSettings.FromPage = 1;
			doc.PrinterSettings.ToPage = fScribble.Document.Pages.Count;
			doc.PrinterSettings.MaximumPage = fScribble.Document.Pages.Count;
			
			PrintDialog dialog = new PrintDialog();
			dialog.AllowSelection = false;
			dialog.AllowSomePages = true;
			dialog.ShowNetwork = true;
			dialog.Document = doc;
			doc.OriginAtMargins = true;
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				fPrintPhysicalRectangles.Clear();
				fPrintPage = dialog.PrinterSettings.FromPage-1;
				fPrintRectangle = 0;
				doc.Print();
			}
		}

		private void File_PrintPreview1(object sender, EventArgs e)
		{
			Genetibase.UI.NuGenScribblePreviewer previewer = 
				new Genetibase.UI.NuGenScribblePreviewer();

			previewer.ShowPrintDialog = true;
			previewer.DoPreviewDialog(fScribble.Scribble);
		}

		private void File_PrintPreview2(object sender, EventArgs e)
		{
			Genetibase.UI.NuGenC1ScribblePreviewer previewer = 
				new Genetibase.UI.NuGenC1ScribblePreviewer();

			previewer.ShowPrintDialog = true;
			previewer.DoPreviewDialog(fScribble.Scribble);
		}

		private void File_Exit(object sender, System.EventArgs e)
		{
			Close();
		}

		#endregion

		#region Edit Menu

		private void EditMenu_Popup(object sender, EventArgs e)
		{
			Menu menu = sender as Menu;
			if (menu == null)
				return;

			menu.MenuItems[0].Enabled = fScribble.Scribble.GetCommandStatus(new UndoCommand());
			menu.MenuItems[1].Enabled = fScribble.Scribble.GetCommandStatus(new RedoCommand());
			// 2 - Seperator
			menu.MenuItems[3].Enabled = fScribble.Scribble.GetCommandStatus(new CutCommand());
			menu.MenuItems[4].Enabled = fScribble.Scribble.GetCommandStatus(new CopyCommand());
			menu.MenuItems[5].Enabled = menu.MenuItems[4].Enabled;
			menu.MenuItems[6].Enabled = fScribble.Scribble.GetCommandStatus(new PasteCommand());
			menu.MenuItems[7].Enabled = fScribble.Scribble.GetCommandStatus(new DeleteCommand());
			// 8 - Seperator
			// 9 - Find, always enabled
			// 10 - Find next, always enabled
			// 11 - Seperator
			// 12 - Define Flags, always enabled
			menu.MenuItems[13].Enabled = fScribble.Scribble.GetCommandStatus(new BringToFrontCommand());
			menu.MenuItems[14].Enabled = fScribble.Scribble.GetCommandStatus(new SendToBackCommand());
			menu.MenuItems[15].Enabled = fScribble.Scribble.GetCommandStatus(new GroupCommand());
			menu.MenuItems[16].Enabled = fScribble.Scribble.GetCommandStatus(new UngroupCommand());
			menu.MenuItems[17].Enabled = fScribble.Scribble.GetCommandStatus(new FormatCommand());
		}

		private void Edit_Undo(object sender, System.EventArgs e)
		{
			fScribble.Scribble.ExecuteCommand(new UndoCommand());
		}
		private void Edit_Redo(object sender, System.EventArgs e)
		{
			fScribble.Scribble.ExecuteCommand(new RedoCommand());
		}
		
		private void Edit_Cut(object sender, System.EventArgs e)
		{
			fScribble.Scribble.ExecuteCommand(new CutCommand());
		}
		
		private void Edit_Copy(object sender, System.EventArgs e)
		{
			fScribble.Scribble.ExecuteCommand(new CopyCommand());
		}
		
		private void Edit_CopyAsText(object sender, System.EventArgs e)
		{
			fScribble.Scribble.CopyAsText();
		}
		
		private void Edit_Paste(object sender, System.EventArgs e)
		{
			fScribble.Scribble.ExecuteCommand(new PasteCommand());
		}
		
		private void Edit_Delete(object sender, System.EventArgs e)
		{
			fScribble.Scribble.ExecuteCommand(new DeleteCommand());
		}
		
		private void Edit_Find(object sender, System.EventArgs e)
		{
			fScribble.Scribble.Find();
		}
		
		private void Edit_FindNext(object sender, System.EventArgs e)
		{
			fScribble.Scribble.FindNext();
		}

		private void Edit_Flags(object sender, System.EventArgs e)
		{
			Agilix.Ink.Dialogs.DefineFlags dialog = new Agilix.Ink.Dialogs.DefineFlags();
			dialog.Colors = fScribble.Toolbar.ColorCollection;
			dialog.Flags = fScribble.Toolbar.Flags;
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				fScribble.Toolbar.RefreshFlagsToolbar();
				// Save the new flags
			}
		}

		private void Edit_BringToFront(object sender, System.EventArgs e)
		{
			fScribble.Scribble.BringElementsToFront();
		}

		private void Edit_SendToBack(object sender, System.EventArgs e)
		{
			fScribble.Scribble.SendElementsToBack();
		}

		private void Edit_Group(object sender, System.EventArgs e)
		{
			fScribble.Scribble.Group();
		}

		private void Edit_Ungroup(object sender, System.EventArgs e)
		{
			fScribble.Scribble.Ungroup();
		}

		private void Edit_ConvertToText(object sender, System.EventArgs e)
		{
			fScribble.Scribble.ConvertToText();
		}

		private void Edit_FormatInk(object sender, System.EventArgs e)
		{
			fScribble.Scribble.Format();
		}

		private void Edit_FitToPage(object sender, System.EventArgs e)
		{
			fScribble.Scribble.FitToPage();
		}

		private void Edit_FitToWidth(object sender, System.EventArgs e)
		{
			fScribble.Scribble.FitToWidth();
		}

		private void Edit_FitToHeight(object sender, System.EventArgs e)
		{
			fScribble.Scribble.FitToHeight();
		}

		#endregion

		#region Paper Menu

		private void Paper_Blank(object sender, System.EventArgs e)
		{
			fScribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Blank, 19050);
		}
		private void Paper_Narrow(object sender, System.EventArgs e)
		{
			fScribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Narrow, 19050);
		}
		private void Paper_Standard(object sender, System.EventArgs e)
		{
			fScribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Standard, 19050);
		}
		private void Paper_College(object sender, System.EventArgs e)
		{
			fScribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.College, 19050);
		}
		private void Paper_Wide(object sender, System.EventArgs e)
		{
			fScribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Wide, 19050);
		}
		private void Paper_Grid(object sender, System.EventArgs e)
		{
			fScribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Grid, 19050);
		}
		private void Paper_SmallGrid(object sender, System.EventArgs e)
		{
			fScribble.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.SmallGrid, 19050);
		}

		#endregion

		#region Insert shape

		private void Insert_Rectangle(object sender, System.EventArgs e)
		{
			InsertShape(new RectangleElement());
		}
		private void Insert_Ellipse(object sender, System.EventArgs e)
		{
			InsertShape(new EllipseElement());
		}
		private void Insert_Triangle(object sender, System.EventArgs e)
		{
			InsertShape(new TriangleElement(TriangleType.IsoscelesUp));
		}
		private void Insert_Yield(object sender, System.EventArgs e)
		{
			InsertShape(new TriangleElement(TriangleType.IsoscelesDown));
		}
		private void Insert_Diamond(object sender, System.EventArgs e)
		{
			InsertShape(new DiamondElement());
		}
		private void Insert_Star(object sender, System.EventArgs e)
		{
			InsertShape(new StarElement());
		}
		private void Insert_Line(object sender, System.EventArgs e)
		{
			InsertShape(new LineElement());
		}
		private void Insert_Arrow(object sender, System.EventArgs e)
		{
			LineElement line = new LineElement();
			line.EndArrow = true;
			InsertShape(line);
		}

		private void InsertShape(Agilix.Ink.ShapeElement shape)
		{
			FormatCommand format = new FormatCommand();
			fScribble.Scribble.GetCommandStatus(format);
			if (format.State.ColorValid)
			{
				shape.LineColor = format.State.Color;
			}
			fScribble.StylusMode = new InsertShapeMode(shape);
		}

		private void Insert_Picture(object sender, System.EventArgs e)
		{
			fScribble.Scribble.InsertPicture();
		}

		#endregion

		#region Help Menu
		private void Help_About(object sender, System.EventArgs e)
		{
			new Agilix.Ink.Dialogs.About().ShowDialog(this);
		}
		#endregion

		#region Private Methods
		private void Save(string filename)
		{
			try
			{
				using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
				{
					fScribble.Document.Save(fs);
					fScribble.Modified = false;
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(this, "Unable to save file: " + filename + "\n" + e.Message, "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void Open(string filename)
		{
			try
			{
				using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
				{
					Document document = new Document();
					document.Load(fs);
					fScribble.Document = document;
					//fScribble.Scribble.Highlight(new object[] { "the" });
					fFilePath = filename;
					fFileName = System.IO.Path.GetFileNameWithoutExtension(filename);
					Text = fFileName + " - " + TITLE;
					fScribble.Modified = false;
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(this, "Unable to open file: " + filename + "\n" + e.Message, "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private class ImportExportParameters
		{
			private string fFilename;
			private int fType;
			private Progress fProgress;

			public ImportExportParameters(string filename, int type, Progress progress)
			{
				fFilename = filename;
				fType = type;
				fProgress = progress;
			}

			public string Filename { get { return fFilename; } }
			public int Type { get { return fType; } }
			public Progress Progress { get { return fProgress ; } }
		}

		private void Import(string filename, int type)
		{
			Progress dialog = new Progress();
			dialog.Text = "Importing...";
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(DoImport),new ImportExportParameters(filename,type,dialog));
			dialog.ShowDialog(this);
		}

		private void DoImport(object status)
		{
			ImportExportParameters parameters = status as ImportExportParameters;

			try
			{
				Document document = null;
				// Wait for dialog
				while (parameters.Progress.Visible == false)
				{
					System.Threading.Thread.Sleep(100);
				}

				using (FileStream fs = new FileStream(parameters.Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					switch (parameters.Type)
					{
						case 1:	document = Agilix.Ink.Converters.Journal.ImportScribble(fs,parameters.Progress.GetProgressCallback()); break;
						case 2:	document = new Document();
							document.Pages.Add(new Page(document));
							Agilix.Ink.Converters.Isf.ImportPage(document.Pages[0], fs,parameters.Progress.GetProgressCallback()); 
							break;
						case 3: document = new Document();
							document.Pages.Add(new Page(document));
							Agilix.Ink.Converters.Image.ImportPage(document.Pages[0], fs,parameters.Progress.GetProgressCallback());
							break;
						case 4: document = Agilix.Ink.Converters.RichText.ImportScribble(fs, new Size(18850, 25000),parameters.Progress.GetProgressCallback()); break;
						case 5: document = Agilix.Ink.Converters.PlainText.ImportScribble(fs, new Size(18850, 25000),parameters.Progress.GetProgressCallback()); break;
						case 6: document = Agilix.Ink.Converters.PrintJob.ImportScribble(fs,parameters.Progress.GetProgressCallback()); break;
//						case 7:
//							document = new Document();
//							document.Pages.Add(Agilix.Ink.Converters.Html.ImportPage(document, fs));
//							break;
//						case 8:
//							document = new Document();
//							document.Pages.Add(Agilix.Ink.Converters.Mhtml.ImportPage(document, fs));
//							break;
					}

					fs.Close();

					if (!parameters.Progress.Cancel)
					{
						fScribble.Document = document;
						fFilePath = null;
						fFileName = System.IO.Path.GetFileNameWithoutExtension(parameters.Filename);
						Text = fFileName + " - " + TITLE;
						fScribble.Modified = true;
					}
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(this, "Unable to import file: " + parameters.Filename + "\n" + e.Message, "Error importing file", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			parameters.Progress.Close();
		}

		private void Export(string filename, int type)
		{
			Progress dialog = new Progress();
			dialog.Text = "Exporting...";
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(DoExport),new ImportExportParameters(filename,type,dialog));
			dialog.ShowDialog(this);
		}
		
		private void DoExport(object status)
		{
			ImportExportParameters parameters = status as ImportExportParameters;

			try
			{
				// Wait for dialog
				while (parameters.Progress.Visible == false)
				{
					System.Threading.Thread.Sleep(100);
				}

				using (FileStream fs = new FileStream(parameters.Filename, FileMode.Create, FileAccess.ReadWrite))
				{
					switch (parameters.Type)
					{
						case 2:	Agilix.Ink.Converters.Isf.ExportPage(fScribble.Document, fScribble.PageIndex, fs);	break;
						case 3: Agilix.Ink.Converters.Mhtml.ExportScribble(fScribble.Document, fs); break;
						case 4: Agilix.Ink.Converters.Image.ExportPage(fScribble.Document, fScribble.PageIndex, fs); break;
						case 5: Agilix.Ink.Converters.RichText.ExportScribble(fScribble.Document, fs); break;
						case 6: Agilix.Ink.Converters.PlainText.ExportScribble(fScribble.Document, fs); break;
					}
					fs.Close();
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(this, "Unable to save file: " + parameters.Filename + "\n" + e.Message, "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			parameters.Progress.Close();
		}

		private void NewFileName()
		{
			fFilePath = null;
			fFileName = "Note" + (++fFileNumber).ToString();
			Text = fFileName + " - " + TITLE;
		}

		private bool IsSaved
		{
			get 
			{
				return (fFilePath != null && fFilePath.Length > 0);
			}
		}

		private void UpdateFileName()
		{
			fUpdateFileNameTimer.Stop();

			if (IsSaved || fScribble.Document.IsAnalysisRunning)
				return;

			Stationery stationery = fScribble.Document.Pages[0].Stationery;
			if(stationery.TitleRectangleStyle != StationeryRectangleStyle.None &&
				!stationery.TitleRectangle.IsEmpty)
			{
				StringBuilder newName = new StringBuilder();
				ArrayList elements = new ArrayList();
				Rectangle titleRectangle = stationery.TitleRectangle;
				titleRectangle.Inflate(-200, -200);
				elements.AddRange(fScribble.Document.Pages[0].DrawElementsInRectangle(titleRectangle, 0.00000000000000000001f));
				elements.AddRange(fScribble.Document.Pages[0].InkElementsInRectangle(titleRectangle, 90.0f));
				elements.Sort();
				foreach(Element element in elements)
				{
					if(element is TextElement)
					{
						string plainText = (element as TextElement).GetPlainText();
						int i = plainText.IndexOf("\r\n");
						if(i > 0)
						{
							plainText = plainText.Substring(0, i);
						}
						newName.Append(plainText);
						newName.Append(" ");
					}
					else if(element is InkParagraphElement)
					{
						foreach(InkLineElement line in element.Elements)
						{
							foreach(InkWordElement word in line.Elements)
							{
								newName.Append(word.Text);
								newName.Append(" ");
							}
							newName.Append(" ");
						}
					}
					else if(element is InkLineElement)
					{
						foreach(InkWordElement word in element.Elements)
						{
							newName.Append(word.Text);
							newName.Append(" ");
						}
						break;
					}
					else if(element is InkWordElement)
					{
						newName.Append((element as InkWordElement).Text);
						newName.Append(" ");
					}
				}
				if(newName.Length > 0)
				{
					fFileName = newName.ToString().Trim();
					foreach (char c in System.IO.Path.InvalidPathChars)
					{
						fFileName = fFileName.Replace(c, '_');
					}
					fFileName = fFileName.Replace('.', '_');
					fFileName = fFileName.Replace(Path.DirectorySeparatorChar, '_');
					fFileName = fFileName.Replace(Path.VolumeSeparatorChar, '-');
					fFileName = fFileName.Replace(Path.AltDirectorySeparatorChar, '_');
					if (fFileName.Length > 40)
					{
						fFileName = fFileName.Substring(0,40);
					}
					Text = fFileName + " - " + TITLE;
				}
			}
		}

		private DialogResult CheckDirty()
		{
			DialogResult result = DialogResult.Yes;
			fScribble.Document.WaitForBackgroundAnalysis();
			if (fScribble.Modified)
			{
				UpdateFileName();
				result = MessageBox.Show(this, "Do you want to save the changes to \"" + fFileName + "\"?",
					TITLE, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

				switch (result)
				{
					case DialogResult.Yes:
						File_Save(this, new EventArgs());
						break;
					case DialogResult.No:
					case DialogResult.Cancel:
						// Do nothing
						break;
				}
			}

			return result;
		}

		private void PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			PrintDocument doc = (sender as PrintDocument);

			// Convert to HiMetric
			Rectangle paper = new Rectangle(0,
											0,
											e.MarginBounds.Width * 2540 / 100,
											e.MarginBounds.Height * 2540 / 100);

			Renderer renderer = new Renderer(fScribble.Document);

			// Paginate the first time through
			if (fPrintPhysicalRectangles.Count == 0)
			{
				for(int page=doc.PrinterSettings.FromPage-1; page<Math.Min(fScribble.Document.Pages.Count, doc.PrinterSettings.ToPage); page++)
				{
					// Adjust the size of the margins
					Rectangle pagePaper = GetAdjustedSize(fScribble.Document.Pages[page],paper);
					pagePaper.Location = new Point(0,0);

					fPrintPhysicalRectangles.Add(renderer.GetPhysicalPages(page, pagePaper.Size, pagePaper.Size));
				}
			}

			if (fPrintPage >= Math.Min(fScribble.Document.Pages.Count, doc.PrinterSettings.ToPage))
				return;

			Rectangle[] clipRectangles = ((Rectangle[])fPrintPhysicalRectangles[fPrintPage-(doc.PrinterSettings.FromPage-1)]); 
			Rectangle clipRectangle = clipRectangles[fPrintRectangle];

			Rectangle adjustedPaper = GetAdjustedSize(fScribble.Document.Pages[fPrintPage],paper);

			GraphicsState state = e.Graphics.Save();

			// The clipRectangle may be larger than what we asked for,
			// As the GetPhysicalPages call automatically scales to fit the width
			float scale = (float)adjustedPaper.Width/(float)clipRectangle.Width;

			// Set the graphics context to HiMetric, and translate so that the print origin is at (0,0)
			SizeF offset = new SizeF(e.Graphics.Transform.OffsetX,e.Graphics.Transform.OffsetY);
			e.Graphics.TranslateTransform(-offset.Width,-offset.Height);
			e.Graphics.PageUnit = GraphicsUnit.Pixel;
			e.Graphics.ScaleTransform(e.Graphics.DpiX/2540f*scale, e.Graphics.DpiY/2540f*scale);
			e.Graphics.TranslateTransform((offset.Width * 25.4f + adjustedPaper.X)/scale,(offset.Height * 25.4f + adjustedPaper.Y)/scale,MatrixOrder.Prepend);
			e.Graphics.TranslateTransform(0, -clipRectangle.Top);

			renderer.Draw(e.Graphics, fPrintPage, clipRectangle, fScribble.Document.Pages[fPrintPage].Size);

			e.Graphics.Restore(state);

			if(++fPrintRectangle != clipRectangles.Length)
			{
				e.HasMorePages = true;
			}
			else
			{
				fPrintRectangle = 0;
				if(++fPrintPage < Math.Min(fScribble.Document.Pages.Count, doc.PrinterSettings.ToPage))
				{
					e.HasMorePages = true;	
				}
				else
				{
					e.HasMorePages = false;
					fPrintPage = 0;
				}
			}
		}

		/// <summary>
		/// Adjust the size of the margins if needed
		/// </summary>
		/// <param name="page">Page to adjust</param>
		/// <param name="originalSize">Original margins</param>
		/// <returns>The adjusted size</returns>
		/// <remarks>This method adjust the printing for 8 1/2 X 11 inch paper, so that print captured pages print out correctly.</remarks>
		private Rectangle GetAdjustedSize(Page page,Rectangle originalSize)
		{
			if (originalSize != new Rectangle(0,0,(int)(6.5*2540),9*2540))
			{
				return originalSize;
			}

			if (page.Width <= originalSize.Width)
			{
				return originalSize;
			}

			if (page.IBinder != null)
			{
				// Return the original IBinder size
				return new Rectangle((int)(-.75*2540),(int)(-.75*2540),8*2540,(int)(10.25*2540));
			}

			// Center the width horizontally
			int newWidth = Math.Min(page.Width,8*2540);
			int offset = (originalSize.Width-newWidth)/2;
			return new Rectangle(offset,originalSize.Y,newWidth,originalSize.Height);
		}

		private void Scribble_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control == true && !e.Handled)
			{
				int flag = -1;
				switch (e.KeyCode)
				{
					case Keys.D1: flag = 0; break;
					case Keys.D2: flag = 1; break;
					case Keys.D3: flag = 2; break;
					case Keys.D4: flag = 3; break;
					case Keys.D5: flag = 4; break;
					case Keys.D6: flag = 5; break;
					case Keys.D7: flag = 6; break;
					case Keys.D8: flag = 7; break;
					case Keys.D9: flag = 8; break;
					case Keys.D0: flag = 9; break;
				}

				// Apply the flag
				if (flag != -1 && flag < fScribble.Flags.Count)
				{
					e.Handled = true;
					fScribble.Scribble.ApplyFlag(fScribble.Flags[flag]);
				}
			}
		}

		private void Scribble_Changed(object sender, EventArgs e)
		{
			fUpdateFileNameTimer.Stop();
			if (!IsSaved)
				fUpdateFileNameTimer.Start();
		}

		private void This_RecognitionChanged(object sender, EventArgs e)
		{
			fUpdateFileNameTimer.Stop();
			if (!IsSaved)
				fUpdateFileNameTimer.Start();
		}

		private void Scribble_RecognitionChanged(object sender, PageEventArgs e)
		{
			if(e.Page == fScribble.Document.Pages[0])
			{
				BeginInvoke(TitlePageChanged, new object[] { sender, new EventArgs() });	
			}
		}

		private void UpdateFileNameTimer_Tick(object sender, EventArgs e)
		{
			fUpdateFileNameTimer.Stop();
			UpdateFileName();
		}

		#endregion
	}
}
