// $Header: /Ink/Note/Samples/Note/NoteApp.cs 48    11/18/04 3:23p Bernd.helzer $
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Agilix.Ink.Note;
using Agilix.Ink;

namespace Sample
{
	/// <summary>
	/// NoteApp is a sample application for Agilix.Ink.Note controls.
	/// It implements a fully functional Journal like application
	/// </summary>
	class NoteApp : System.Windows.Forms.Form
	{
		#region Fields

		/// <summary>
		/// Title for this application
		/// </summary>
		private const string TITLE = "Sample NoteTaking Application";
		/// <summary>
		/// The main (only) control 
		/// </summary>
		private Agilix.Ink.Note.NoteBox fNoteBox;
		/// <summary>
		/// Current file name
		/// </summary>
		private string fFileName;
		/// <summary>
		/// Current file path
		/// </summary>
		private string fFilePath;
		/// <summary>
		/// Number of the file for File..New
		/// </summary>
		private int fFileNumber = 0;
		/// <summary>
		/// Temporary number for printing multiple pages
		/// </summary>
		private int fPrintPage;

		#endregion

		#region Constructor
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			// Use the XP style
			// Make sure you call DoEvents after EnableVisualStyles or you get the
			// nasty SEHException sometime in the future. 
			// Also make sure you call both of these before creating any forms
			Application.EnableVisualStyles();
			Application.DoEvents(); 

			Application.Run(new NoteApp());
		}

		/// <summary>
		/// Create the form
		/// </summary>
		public NoteApp()
		{
			// Adding a menu for the application
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
					new MenuItem("Format Ink...", new EventHandler(Edit_FormatInk))
				});
			menu.MenuItems[1].Popup += new EventHandler(EditMenu_Popup);
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

			// Create a NoteBox and assign to the form
			fNoteBox = new Agilix.Ink.Note.NoteBox();
			fNoteBox.Dock = DockStyle.Fill;
			fNoteBox.Document.Stationery = Stationery.CreateStockStationeryWithTitle(StationeryStockType.Standard, 19050);

			// Set some default form variables
			this.Icon = new Icon(typeof(NoteApp), "App.ico");
			this.Menu = menu;
			this.Controls.Add(fNoteBox);
			this.Width = 735;
			this.Height = 700;
			this.Text = TITLE;
			this.Load += new EventHandler(Application_Load);
			this.Closing += new System.ComponentModel.CancelEventHandler(Application_Closing);

			NewFileName();
			fNoteBox.Modified = false;
		}

		#endregion

		#region Private Methods
		
		private void Application_Load(object sender, System.EventArgs e)
		{
			// Retrieve the settings for the pens and custom colors on load
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Agilix\Ink\Sample");
			fNoteBox.ColorCollectionData = key.GetValue("Colors", "").ToString();
			fNoteBox.WritingInstrumentsData = key.GetValue("WritingInstruments", "").ToString();
			fNoteBox.Toolbar.RefreshPenMenu();
			key.Close();
		}

		private void Application_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Store the current pen and custom colors on closing
			Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Agilix\Ink\Sample");
			key.SetValue("Colors", fNoteBox.ColorCollectionData);
			key.SetValue("WritingInstruments", fNoteBox.WritingInstrumentsData);
			key.Close();

			// Give the user the chance to save or cancel if note is dirty
			if (CheckDirty() == DialogResult.Cancel)
			{
				e.Cancel = true;
			}
		}

		/// <summary>
		/// Save the current note to a file
		/// </summary>
		/// <param name="path">Path for note</param>
		private void Save(string path)
		{
			try
			{
				using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
				{
					fNoteBox.Document.Save(fs);
					fNoteBox.Modified = false;
					fFilePath = path;
					fFileName = System.IO.Path.GetFileNameWithoutExtension(path);
					Text = fFileName + " - " + TITLE;
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(this, "Unable to save file: " + path + "\n" + e.Message, "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Open an existing file 
		/// </summary>
		/// <param name="path">Path for note</param>
		private void Open(string path)
		{
			try
			{
				using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
				{
					Document document = new Document();
					document.Load(fs);
					fNoteBox.Document = document;
					fFilePath = path;
					fFileName = System.IO.Path.GetFileNameWithoutExtension(path);
					Text = fFileName + " - " + TITLE;
					fNoteBox.Modified = false;
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(this, "Unable to open file: " + path + "\n" + e.Message, "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// Import a Journal note file
		/// </summary>
		/// <param name="path">path of journal note</param>
		private void Import(string filename, int type)
		{
			System.Windows.Forms.Cursor currentCursor = Cursor.Current;
			Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
			try
			{
				using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
				{
					switch (type)
					{
						case 1:	fNoteBox.Document = Agilix.Ink.Converters.Journal.ImportNote(fs,null); break;
						case 2:	fNoteBox.Document = Agilix.Ink.Converters.Isf.ImportNote(fs,null); break;
						case 3: fNoteBox.Document = Agilix.Ink.Converters.Image.ImportNote(fs,null); break;
					}

					fFilePath = null;
					fFileName = System.IO.Path.GetFileNameWithoutExtension(filename);
					Text = fFileName + " - " + TITLE;
					fNoteBox.Modified = true;
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(this, "Unable to import file: " + filename + "\n" + e.Message, "Error importing file", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Cursor.Current = currentCursor;
		}

		/// <summary>
		/// Export a file
		/// </summary>
		/// <param name="filename">Filename</param>
		/// <param name="type">Type of export</param>
		private void Export(string filename, int type)
		{
			try
			{
				using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
				{
					switch (type)
					{
						case 2:	Agilix.Ink.Converters.Isf.ExportNote(fNoteBox.Document, fs);	break;
						case 3: Agilix.Ink.Converters.Mhtml.ExportNote(fNoteBox.Document, fs); break;
						case 4: Agilix.Ink.Converters.Image.ExportNote(fNoteBox.Document, fs); break;
						case 5: Agilix.Ink.Converters.RichText.ExportNote(fNoteBox.Document, fs); break;
						case 6: Agilix.Ink.Converters.PlainText.ExportNote(fNoteBox.Document, fs); break;
					}
				}
			}
			catch (Exception e)
			{
				MessageBox.Show(this, "Unable to save file: " + filename + "\n" + e.Message, "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}



		/// <summary>
		/// Create a new filename for an empty note
		/// </summary>
		private void NewFileName()
		{
			fFilePath = null;
			fFileName = "Note" + (++fFileNumber).ToString();
			Text = fFileName + " - " + TITLE;
		}

		/// <summary>
		/// Check the dirty flag and prompt the user to save if empty
		/// </summary>
		/// <returns>DialogResult.Cancel if the user wants to cancel the operation</returns>
		private DialogResult CheckDirty()
		{
			DialogResult result = DialogResult.Yes;
			if (fNoteBox.Modified)
			{
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

		[System.Runtime.InteropServices.DllImport("gdi32.dll")]
		public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

		/// <summary>
		/// Prints a page of the current note
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			// Translate the graphics by the printable margin
			IntPtr hDC = e.Graphics.GetHdc();
			const int PHYSICALOFFSETX = 112; /* Physical Printable Area x margin         */
			const int PHYSICALOFFSETY = 113; /* Physical Printable Area y margin         */
			int offsetX = GetDeviceCaps(hDC, PHYSICALOFFSETX);
			int offsetY = GetDeviceCaps(hDC, PHYSICALOFFSETY);
			e.Graphics.ReleaseHdc( hDC );

			e.Graphics.PageUnit = GraphicsUnit.Pixel;

			// Build paper clip rectangle
			Rectangle paper = new Rectangle(
				(int)((e.Graphics.DpiX/4)-offsetX),
				(int)((e.Graphics.DpiY/4)-offsetY),
				(int)((e.PageBounds.Width*e.Graphics.DpiX/100)-(e.Graphics.DpiX/2)),
				(int)((e.PageBounds.Height*e.Graphics.DpiY/100)-(e.Graphics.DpiY*3/4)));

			// Convert width and height to hi-metric
			int pageWidth = (int)(paper.Width*2540/e.Graphics.DpiX);
			int pageHeight = (int)(paper.Height*2540/e.Graphics.DpiY);

			// Adjust document position if it is smaller than clip rectangle
			int offset = Math.Min(Math.Max(pageWidth-fNoteBox.Document.Stationery.MinWidth, 0)/2, 2540*3/4);
			int dx = (int)(offset*e.Graphics.DpiX/2540);
			int dy = (int)(offset*e.Graphics.DpiY/2540);
			paper.Inflate(-dx, -dy);

			// Transform graphics to map hi-metric units to paper pixels
			e.Graphics.TranslateTransform(paper.X, paper.Y);
			e.Graphics.ScaleTransform(e.Graphics.DpiX/2540f, e.Graphics.DpiY/2540f);
			e.Graphics.TranslateTransform(0, -pageHeight*fPrintPage);

			// Convert paper rectangle to hi-metric for clip rectangle
			Point[] pts = { paper.Location, new Point(paper.Right, paper.Bottom) };
			Matrix invert = e.Graphics.Transform;
			invert.Invert();
			invert.TransformPoints(pts);
			Rectangle clipRectangle = Rectangle.FromLTRB(pts[0].X, pts[0].Y, pts[1].X, pts[1].Y);

			// Render a physical page of the note
			Renderer renderer = new Renderer(fNoteBox.Document);
			renderer.Draw(e.Graphics, clipRectangle, fNoteBox.Document.Size);

			// Check to see if the whole note has printed or if it requires more physical pages
			if ((fPrintPage+1)*pageHeight < fNoteBox.Document.Height)
			{
				e.HasMorePages = true;
				fPrintPage++;
			}
		}
		#endregion

		#region File Menu
		private void File_New(object sender, EventArgs e)
		{
			if (CheckDirty() == DialogResult.Cancel)
			{
				return;
			}
			NewFileName();
			
			// Create a new Document, use the current Paper
			fNoteBox.Document = new Agilix.Ink.Note.Document(fNoteBox.Document.Stationery.Clone() as Stationery);
		}
		private void File_Open(object sender, EventArgs e)
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
		private void File_Import(object sender, EventArgs e)
		{
			if (CheckDirty() == DialogResult.Cancel)
			{
				return;
			}
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Title = "Import File";
			dialog.CheckFileExists = true;
			dialog.Filter = "Windows Journal Note (*.jnt)|*.jnt|Ink Serialized Format (*.isf)|*.isf|Image|*.bmp;*.gif;*.png;*.jpg;*.wmf";
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				Import(dialog.FileName, dialog.FilterIndex);
			}
		}
		private void File_Save(object sender, EventArgs e)
		{
			if (fFilePath == null || fFilePath.Length == 0)
			{
				File_SaveAs(sender, e);
			}
			else
			{
				Save(fFileName);
			}
		}
		private void File_SaveAs(object sender, EventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.Title = "Save As";
			dialog.OverwritePrompt = true;
			dialog.CheckPathExists = true;
			dialog.DefaultExt = "ant";
			if (fFilePath == null || fFilePath.Length == 0)
			{
				dialog.FileName = fFileName;
			}
			else
			{
				dialog.FileName = fFilePath;
			}
			dialog.Filter = TITLE + " file (*.ant)|*.ant|Ink Serialized Format (*.isf)|*.isf|Mhtml (*.mht)|*.mht|Image (*.png)|*.png|Rich text (*.rtf)|*.rtf|Plain text (*.txt)|*.txt";
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
		private void File_Print(object sender, EventArgs e)
		{
			System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();
			doc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PrintPage);
			doc.DocumentName = fFileName;
			
			PrintDialog dialog = new PrintDialog();
			dialog.AllowSelection = false;
			dialog.AllowSomePages = false;
			dialog.ShowNetwork = true;
			dialog.Document = doc;
			if (dialog.ShowDialog(this) == DialogResult.OK)
			{
				fPrintPage = 0;
				doc.Print();
			}
		}

		private void File_PrintPreview1(object sender, EventArgs e)
		{
			Genetibase.UI.NuGenNotePreviewer previewer = 
							new Genetibase.UI.NuGenNotePreviewer();

			previewer.DoPreviewDialog(fNoteBox.Note);
		}

		private void File_PrintPreview2(object sender, EventArgs e)
		{
			Genetibase.UI.NuGenC1NotePreviewer previewer = 
				new Genetibase.UI.NuGenC1NotePreviewer();

			previewer.DoPreviewDialog(fNoteBox.Note);
		}


		private void File_Exit(object sender, EventArgs e)
		{
			Close();
		}
		#endregion

		#region Edit Menu
		private void EditMenu_Popup(object sender, EventArgs e)
		{
			// Enable the items based on the state of the control

			MenuItem editMenu = Menu.MenuItems[1];
			
			editMenu.MenuItems[0].Enabled = fNoteBox.Note.CanUndo();
			editMenu.MenuItems[1].Enabled = fNoteBox.Note.CanRedo();
			// -
			editMenu.MenuItems[3].Enabled = fNoteBox.Note.CanCut();
			editMenu.MenuItems[4].Enabled = fNoteBox.Note.CanCopy();
			editMenu.MenuItems[5].Enabled = fNoteBox.Note.CanCopy();
			editMenu.MenuItems[6].Enabled = fNoteBox.Note.CanPaste();
			editMenu.MenuItems[7].Enabled = fNoteBox.Note.CanDelete();
			// -
			// Find - always enabled
			// Find Next - always enabled
			// -
			editMenu.MenuItems[12].Enabled = fNoteBox.Note.HasSelection;
		}

		private void Edit_Undo(object sender, EventArgs e)
		{
			fNoteBox.Note.Undo();
		}
		private void Edit_Redo(object sender, EventArgs e)
		{
			fNoteBox.Note.Redo();
		}
		private void Edit_Cut(object sender, EventArgs e)
		{
			fNoteBox.Note.Cut();
		}
		private void Edit_Copy(object sender, EventArgs e)
		{
			fNoteBox.Note.Copy();
		}
		private void Edit_CopyAsText(object sender, EventArgs e)
		{
			fNoteBox.Note.CopyAsText();
		}
		private void Edit_Paste(object sender, EventArgs e)
		{
			fNoteBox.Note.Paste();
		}
		private void Edit_Delete(object sender, EventArgs e)
		{
			fNoteBox.Note.Delete();
		}
		private void Edit_Find(object sender, EventArgs e)
		{
			fNoteBox.Note.Find();
		}
		private void Edit_FindNext(object sender, EventArgs e)
		{
			fNoteBox.Note.FindNext();
		}
		private void Edit_FormatInk(object sender, EventArgs e)
		{
			fNoteBox.Note.Format();
		}
		#endregion
		
		#region Paper Menu
		private void Paper_Blank(object sender, EventArgs e)
		{
			fNoteBox.Stationery = Agilix.Ink.Stationery.CreateStockStationeryWithTitle(Agilix.Ink.StationeryStockType.Blank, 19050);
		}
		private void Paper_Narrow(object sender, EventArgs e)
		{
			fNoteBox.Stationery = Agilix.Ink.Stationery.CreateStockStationeryWithTitle(Agilix.Ink.StationeryStockType.Narrow, 19050);
		}
		private void Paper_College(object sender, EventArgs e)
		{
			fNoteBox.Stationery = Agilix.Ink.Stationery.CreateStockStationeryWithTitle(Agilix.Ink.StationeryStockType.College, 19050);
		}
		private void Paper_Standard(object sender, EventArgs e)
		{
			fNoteBox.Stationery = Agilix.Ink.Stationery.CreateStockStationeryWithTitle(Agilix.Ink.StationeryStockType.Standard, 19050);
		}
		private void Paper_Wide(object sender, EventArgs e)
		{
			fNoteBox.Stationery = Agilix.Ink.Stationery.CreateStockStationeryWithTitle(Agilix.Ink.StationeryStockType.Wide, 19050);
		}
		private void Paper_SmallGrid(object sender, EventArgs e)
		{
			fNoteBox.Stationery = Agilix.Ink.Stationery.CreateStockStationeryWithTitle(Agilix.Ink.StationeryStockType.SmallGrid, 19050);
		}
		private void Paper_Grid(object sender, EventArgs e)
		{
			fNoteBox.Stationery = Agilix.Ink.Stationery.CreateStockStationeryWithTitle(Agilix.Ink.StationeryStockType.Grid, 19050);
		}
		#endregion
		
		#region Help Menu
		private void Help_About(object sender, EventArgs e)
		{
			new Agilix.Ink.Dialogs.About().ShowDialog(this);
		}
		#endregion

	}
}
