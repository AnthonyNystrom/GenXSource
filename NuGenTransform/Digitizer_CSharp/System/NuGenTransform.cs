using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Genetibase.NuGenTransform.Properties;
using System.Drawing;

namespace Genetibase.NuGenTransform
{
    //A simple Map class which manages documents and views
    //A 1-1 relationship is maintained from document to view without
    //the classes knowing about each other
    class DocumentViewMap
    {
        List<NuGenDocument> docs;
        List<NuGenView> views;

        public DocumentViewMap()
        {
            docs = new List<NuGenDocument>();
            views = new List<NuGenView>();
        }

        public void Add(NuGenDocument doc, NuGenView view)
        {
            docs.Add(doc);
            views.Add(view);
            doc.RegisterListener(view);
        }

        //Enumerates through documents and returns the view at the same position
        public NuGenDocument GetDocument(NuGenView view)
        {
            IEnumerator<NuGenDocument> iDocs = docs.GetEnumerator();
            IEnumerator<NuGenView> iViews = views.GetEnumerator();

            while(iViews.MoveNext() & iDocs.MoveNext())
            {
                if (iViews.Current == view)
                {
                    return iDocs.Current;
                }
            }

            return null;
        }

        //Enumerates through views and returns the document at the same position
        public NuGenView GetView(NuGenDocument doc)
        {
            IEnumerator<NuGenDocument> iDocs = docs.GetEnumerator();
            IEnumerator<NuGenView> iViews = views.GetEnumerator();

            while (iViews.MoveNext() & iDocs.MoveNext())
            {
                if (iDocs.Current == doc)
                {
                    return iViews.Current;
                }
            }

            return null;
        }

        public void Remove(NuGenDocument d)
        {
            views.Remove(GetView(d));
            docs.Remove(d);            
        }

        public void Remove(NuGenView v)
        {
            docs.Remove(GetDocument(v));
            views.Remove(v);
        }

        public int Count
        {
            get
            {
                //This should never happen anyways, but just in case..
                if (docs.Count != views.Count)
                {
                    throw new Exception("Structure is out of sync");
                }

                return docs.Count;
            }
        }

        public List<NuGenDocument> Documents
        {
            get
            {
                return docs;
            }
        }

        public List<NuGenView> Views
        {
            get
            {
                return views;
            }
        }
    }

    public class NuGenTransform : NuGenEventHandler
    {
        //The form which contains all views
        private NuGenForm form;

        //The view we are currently manipulating
        private NuGenView activeView;

        //The default locations and sizes (from settings) for the windows to open up at
        private Point mainWindowPos;
        private Size mainWindowSize;
        private Point curveWindowPos;
        private Size curveWindowSize;
        private Point measureWindowPos;
        private Size measureWindowSize;
        private Point otherWindowPos;
        private Size otherWindowSize;

        private DocumentViewMap docViewMap = new DocumentViewMap();

        public NuGenDocument ActiveDocument
        {
            get
            {
                return docViewMap.GetDocument(activeView);
            }
        }

        [STAThread()]
        public static void Main()
        {
#if DEBUG
            UnitTests.Test();
#else
            new NuGenTransform();
#endif
        }

        public NuGenTransform()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            form = new NuGenForm(this);
            form.IsMdiContainer = true;
            form.CurveCombo.Items.Add(NuGenPointSetCollection.DefaultCurveName);
            form.CurveCombo.Enabled = false;
            form.CurveCombo.SelectedIndex = 0;
            form.MeasureCombo.Items.Add(NuGenPointSetCollection.DefaultMeasureName);
            form.MeasureCombo.SelectedIndex = 0;
            form.MeasureCombo.Enabled = false;
            form.CurveCombo.SelectedIndexChanged += new EventHandler(CurveCombo_SelectedIndexChanged);
            form.MeasureCombo.SelectedIndexChanged += new EventHandler(MeasureCombo_SelectedIndexChanged);

            NuGenDefaultSettings settings = NuGenDefaultSettings.GetInstance();

            mainWindowPos = settings.WindowMainPosition;
            mainWindowSize = settings.WindowMainSize;
            curveWindowPos = settings.WindowCurvePosition;
            curveWindowSize = settings.WindowCurveSize;
            measureWindowPos = settings.WindowMeasurePosition;
            measureWindowSize = settings.WindowMeasureSize;
            otherWindowPos = curveWindowPos;
            otherWindowSize = curveWindowSize;            

            System.Windows.Forms.Application.Run(form);
        }

        public bool ProcessDialogKey(Keys keyData)
        {
            if(activeView != null)
                return activeView.HandleKey(keyData);

            return false;
        }

        void MeasureCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeView.ActiveMeasureName = (string)form.MeasureCombo.SelectedItem;            
        }

        void CurveCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeView.ActiveCurveName = (string)form.CurveCombo.SelectedItem;
        }

        //Initializes a new document and its corresponding view when importing or opening a document
        public NuGenDocument NewDocument()
        {
             NuGenDocument doc = null;


             if (docViewMap.Count == 0)
             {
                 form.EnableControls();
             }

             if (ActiveDocument != null) {
               // this document is not the first so start up in same state as previous document for continuity
                 doc = new NuGenDocument(ActiveDocument.DigitizeState);
             }
             else
             {
                 doc = new NuGenDocument(NuGenDefaultSettings.GetInstance().SessionsSettings.initialDigitizeState);
             }

             NuGenView view = new NuGenView(form, doc, this.SendStatusMessage);
             activeView = view;
             activeView.Focus();
             view.Activate();
             view.FormClosing += new FormClosingEventHandler(View_Closing);
             docViewMap.Add(doc, view);
             doc.UpdateListeners();
             form.StatusPermanentMessage("Three axis points or the scale bar must be defined.");

             activeView.ShowCoordinates += new NuGenView.Show_Coordinates(this.Show_Coordinates);
            
             return doc;
        }

        //delegate for view to allow communication with form's status bar
        public void SendStatusMessage(string message)
        {
            form.StatusPermanentMessage(message);
        }

        //Creates a new document by copying an old document
        public NuGenDocument NewDocument(NuGenDocument doc)
        {
            if (docViewMap.Count == 0)
            {
                form.EnableControls();
            }

            NuGenView view = new NuGenView(form, doc, this.SendStatusMessage);
            activeView = view;
            activeView.Focus();
            view.Activate();
            view.FormClosing += new FormClosingEventHandler(View_Closing);
            docViewMap.Add(doc, view);
            form.StatusPermanentMessage("Three axis points or the scale bar must be defined.");

            return doc;
        }

        //Takes an image file and sets up a document to host it
        private void ImportImageFile(string filename)
        {
              form.StatusNormalMessage("Importing File...");
              Cursor.Current = Cursors.WaitCursor;

            //create the document and set up its view
              NuGenDocument doc = NewDocument();

              if (!doc.ImportFile(filename))
              {

                  Cursor.Current = Cursors.Arrow;

                  MessageBox.Show("Could not import image!");
                  RemoveDocument(doc);

                  return;
              }

              doc.InitDefaults();

              form.EnableControls();
              form.CheckedBackgroundOption(doc.BackgroundSelection);
              form.CheckedPointViewOption(docViewMap.GetView(doc).ViewPointSelection);
            form.StatusNormalMessage("Ready.");
            Cursor.Current = Cursors.Arrow;
        }

        //Open up a serialized document from file
        private void OpenDocumentFile(string filename)
        {
          form.StatusNormalMessage("Opening File...");
          Cursor.Current = Cursors.WaitCursor;

          // check, if document already open. If yes, set the focus to the first view
          foreach (NuGenDocument doc in docViewMap.Documents)
          {
              if (doc.SavePath == filename)
              {
                  docViewMap.GetView(doc).Activate();
                  Cursor.Current = Cursors.Arrow;
                  return;
              }
          }

          Stream s = null;            

          NuGenDocument finalDoc = null;
          try
          {
              s = File.Open(filename, FileMode.Open);
              BinaryFormatter formatter = new BinaryFormatter();
              finalDoc = NewDocument((NuGenDocument)formatter.Deserialize(s));
              finalDoc.SavePath = filename;
              finalDoc.UpdateListeners();
              finalDoc.ComputeTransformation();

              if (finalDoc.ValidAxes || finalDoc.ValidAxes)
                  form.StatusNormalMessage("Axes Defined");

              s.Close();

              if (docViewMap.Count == 1)
              {
                  form.EnableControls();
              }
          }
          catch (Exception e)
          {
              if(finalDoc != null)
                docViewMap.GetView(finalDoc).Close();     
         
              MessageBox.Show("Could not open document!");
              if(s!=null)
                s.Close();
          }

          form.CheckedBackgroundOption(finalDoc.BackgroundSelection);
          form.CheckedPointViewOption(docViewMap.GetView(finalDoc).ViewPointSelection);
          form.EnableControls();

          activeView.ShowCoordinates += new NuGenView.Show_Coordinates(this.Show_Coordinates);
          activeView.DrawAll();
          Cursor.Current = Cursors.Arrow;
        }

        //Removes a document and its corresponding view
        private void RemoveDocument(NuGenDocument doc)
        {
            if (doc != null)
            {
                if (docViewMap.GetView(doc) != null)
                {
                    if (docViewMap.GetView(doc).Visible)
                        docViewMap.GetView(doc).Close();

                    docViewMap.Remove(doc);

                    if (docViewMap.Count < 1)
                    {
                        activeView = null;
                    }
                }
            }
        }

        private void View_Closing(object sender, System.EventArgs args)
        {
            ((NuGenView)sender).Visible = false;
            RemoveDocument(docViewMap.GetDocument((NuGenView)sender));
            if (docViewMap.Count <1)
            {
                form.InitializeDefaults();
            }
        }

        //Saves a doucment by serializing it
        private void SaveDocument(NuGenDocument doc)
        {

            if (doc != null)
            {
              if (!doc.SaveFileExists)
                SaveDocumentAs(doc);
              else
              {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        Stream s = File.Open(doc.SavePath, FileMode.Create);
                        BinaryFormatter formatter = new BinaryFormatter();

                        formatter.Serialize(s, doc);
                        s.Close();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error saving file");
                    }

                    Cursor.Current = Cursors.Arrow;
              }
            }
        }

        //Shows the user a file dialog which they use to select a save path
        private void SaveDocumentAs(NuGenDocument doc)
        {
            if (doc != null)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.InitialDirectory = Directory.GetCurrentDirectory();
                dlg.Filter = dlg.Filter = "NuGenTransform Files (*.ngt)|*.ngt";
                dlg.DefaultExt = doc.SavePath;
                dlg.ShowDialog();

                string filename = dlg.FileName;

              if (filename.Length > 0)
              {
                    Cursor.Current = Cursors.WaitCursor;
                    try
                    {
                        Stream s = File.Open(filename, FileMode.Create);
                        BinaryFormatter formatter = new BinaryFormatter();

                        formatter.Serialize(s, doc);
                        s.Close();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error loading file");
                    }

                    Cursor.Current = Cursors.Arrow;
              }
            }
        }

        //Opens up the geometry view for the current curve
        private void UpdateCurveGeometry()
        {
            if (ActiveDocument != null)
            {
                List<NuGenGeometryWindowItem> info = new List<NuGenGeometryWindowItem>();
               
                ActiveDocument.GeometryInfoCurve(info);
                DisplayCurveGeometry(info);
            }
            else
                NoGeometryInfo();
        }

        //Opens up the geometry view for the current measure
        private void UpdateMeasureGeometry()
        {
            if (ActiveDocument != null)
            {
                List<NuGenGeometryWindowItem> info = new List<NuGenGeometryWindowItem>();

                ActiveDocument.GeometryInfoMeasure(info);
                DisplayMeasureGeometry(info);
            }
            else
                NoGeometryInfo();
        }
        
        private void DisplayCurveGeometry(List<NuGenGeometryWindowItem> info)
        {
            GeometryDisplayDialog.Show(info, curveWindowPos, curveWindowSize);
        }

        private void DisplayMeasureGeometry(List<NuGenGeometryWindowItem> info)
        {
            GeometryDisplayDialog.Show(info, measureWindowPos, measureWindowSize);
        }

        private void NoGeometryInfo()
        {
            List<NuGenGeometryWindowItem> info = new List<NuGenGeometryWindowItem>();
            info.Add(new NuGenGeometryWindowItem(0, 0, "No Geometry information yet"));

            DisplayCurveGeometry(info);
            DisplayMeasureGeometry(info);
        }

        #region Event Handlers

        #region File Menu
        //Shows the user a file select dialog to pick an image to import
        public void Import_Click(object sender, System.EventArgs args)
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.Filter = "Images |*.bmp;*.gif;*.jpg;*.png;*.pnm;|All Files |*.*";
            dlg.InitialDirectory = Directory.GetCurrentDirectory();

            if (dlg.ShowDialog() == DialogResult.Cancel)
            {
                  return;
            }

            form.Refresh();
            if (activeView != null)
            {
                activeView.DrawAll();
                activeView.Refresh();
            }

            ImportImageFile(dlg.FileName);

            form.StatusNormalMessage("Ready.");
        }

        //Shows the user a file dialog to pick the serialized document to open
        public void Open_Click(object sender, System.EventArgs args)
        {
            form.StatusNormalMessage("Opening document file...");
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "NuGenTransform Files (*.ngt)|*.ngt";
            dlg.ShowDialog();

            form.Refresh();
            if (activeView != null)
            {
                activeView.DrawAll();
                activeView.Refresh();
            }

              if (dlg.FileName !=null && dlg.FileName != "")
              {
                OpenDocumentFile(dlg.FileName);
              }

              form.StatusNormalMessage("Ready.");
        }

        public void Close_Click(object sender, System.EventArgs args)
        {
            form.StatusNormalMessage("Closing File...");
            RemoveDocument(ActiveDocument);
            form.StatusNormalMessage("Ready.");
        }

        public void Save_Click(object sender, System.EventArgs args)
        {
            form.StatusNormalMessage("Saving Document...");
            SaveDocument(ActiveDocument);
            form.StatusNormalMessage("Ready.");
        }

        public void SaveAs_Click(object sender, System.EventArgs args)
        {
            form.StatusNormalMessage("Saving Document...");
            SaveDocumentAs(ActiveDocument);
            form.StatusNormalMessage("Ready.");
        }

        public void Export_Click(object sender, System.EventArgs args)
        {
            if (ActiveDocument == null)
                return;

            if (ActiveDocument.ExportFileExists)
            {
                ActiveDocument.ExportDocument(ActiveDocument.ExportPath);
            }
            else
            {
                ExportAs_Click(sender, args);
            }
        }

        //Shows a file dialog to pick the export file to write curve data to
        public void ExportAs_Click(object sender, System.EventArgs args)
        {
            if (ActiveDocument == null)
                return;

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Title = "Export As";
            dlg.Filter = "Text Files (*.txt)|*.txt";
            dlg.InitialDirectory = Directory.GetCurrentDirectory();

            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                string filename = dlg.FileName;

                ActiveDocument.ExportDocument(filename);
            }
        }

        public void Exit_Click(object sender, System.EventArgs args)
        {            
            Application.Exit();
        }

        public void Print_Click(object sender, System.EventArgs args)
        {
            System.Drawing.Printing.PrintDocument doc = new System.Drawing.Printing.PrintDocument();
            doc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(doc_PrintPage);

            doc.Print();
        }

        void doc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            activeView.Print(e.Graphics);
        }

        #endregion

        #region EditMenu

        public void Cut_Click(object sender, System.EventArgs args)
        {
            activeView.CutPoints();
        }

        public void Copy_Click(object sender, System.EventArgs args)
        {
            activeView.CopyPoints();
        }

        public void Paste_Click(object sender, System.EventArgs args)
        {
            activeView.PastePoints();
        }

        public void PasteAsNew_Click(object sender, System.EventArgs args)
        {
            activeView.PastePointsAsNew();

            UpdateCurveList();

            activeView.DrawAll();
            activeView.Refresh();
        }

        #endregion

        #region View Menu

        public void AxesPointsView_Click(object sender, System.EventArgs args)
        {
            docViewMap.GetView(ActiveDocument).ViewPointSelection = ViewPointSelection.ViewAxesPoints;
            NuGenDefaultSettings.GetInstance().ViewPointSelection = ViewPointSelection.ViewAxesPoints;
        }
        public void ScaleBarView_Click(object sender, System.EventArgs args)
        {
            docViewMap.GetView(ActiveDocument).ViewPointSelection = ViewPointSelection.ViewScalePoints;
            NuGenDefaultSettings.GetInstance().ViewPointSelection = ViewPointSelection.ViewScalePoints;
        }
        public void CurvePointsView_Click(object sender, System.EventArgs args)
        {
            docViewMap.GetView(ActiveDocument).ViewPointSelection = ViewPointSelection.ViewCurvePoints;
            NuGenDefaultSettings.GetInstance().ViewPointSelection = ViewPointSelection.ViewCurvePoints;
        }
        public void MeasurePointsView_Click(object sender, System.EventArgs args)
        {
            docViewMap.GetView(ActiveDocument).ViewPointSelection = ViewPointSelection.ViewMeasurePoints;
            NuGenDefaultSettings.GetInstance().ViewPointSelection = ViewPointSelection.ViewMeasurePoints;
        }
        public void AllPointsView_Click(object sender, System.EventArgs args)
        {
            docViewMap.GetView(ActiveDocument).ViewPointSelection = ViewPointSelection.ViewAllPoints;
            NuGenDefaultSettings.GetInstance().ViewPointSelection = ViewPointSelection.ViewAllPoints;
        }

        public void NoBackground_Click(object sender, System.EventArgs args)
        {
            ActiveDocument.BackgroundSelection = BackgroundSelection.BlankBackground;
            NuGenDefaultSettings.GetInstance().BackgroundSelection = BackgroundSelection.BlankBackground;
        }
        public void OriginalBackground_Click(object sender, System.EventArgs args)
        {
            ActiveDocument.BackgroundSelection = BackgroundSelection.OriginalImage;
            NuGenDefaultSettings.GetInstance().BackgroundSelection = BackgroundSelection.OriginalImage;
        }
        public void ProcessedImage_Click(object sender, System.EventArgs args)
        {            
            ActiveDocument.BackgroundSelection = BackgroundSelection.ProcessedImage;
            NuGenDefaultSettings.GetInstance().BackgroundSelection = BackgroundSelection.ProcessedImage;
        }

        public void GridlinesDisplay_Click(object sender, System.EventArgs args)
        {
            if (ActiveDocument.ValidAxes || ActiveDocument.ValidScale)
            {
                activeView.ShowGridLines = !activeView.ShowGridLines;
                activeView.DrawAll();
            }
            else
            {
                MessageBox.Show("Must define axes points in order to use grid lines");
            }
        }

        public void CurveGeometryInfo_Click(object sender, System.EventArgs args)
        {
            UpdateCurveGeometry();
        }

        public void MeasureGeometryInfo_Click(object sender, System.EventArgs args)
        {
            UpdateMeasureGeometry();
        }

        #endregion

        #region Digitize Menu

        public void Select_Click(object sender, System.EventArgs args)
        {
            if (ActiveDocument.DigitizeState == DigitizeState.PointMatchState && ActiveDocument.MatchSet != null && ActiveDocument.MatchSet.Matches.Count > 0)
            {
                if (MessageBox.Show("Would you like to save your matched points?", "Point Match complete") == DialogResult.OK)
                {
                    ActiveDocument.SaveMatchedPoints();
                    activeView.DrawAll();
                }
                else
                {
                    ActiveDocument.DiscardMatchedPoints();
                }
            }
            ActiveDocument.DigitizeState = DigitizeState.SelectState;
            activeView.Cursor = CursorFromState(DigitizeState.SelectState);
            activeView.Focus();

            form.EnableEditMenu();
        }

        public void AxisPoint_Click(object sender, System.EventArgs args)
        {
            if (ActiveDocument.DigitizeState == DigitizeState.PointMatchState && ActiveDocument.MatchSet != null && ActiveDocument.MatchSet.Matches.Count > 0)
            {
                if (MessageBox.Show("Would you like to save your matched points?", "Point Match complete", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    ActiveDocument.SaveMatchedPoints();
                    activeView.DrawAll();
                }
                else
                {
                    ActiveDocument.DiscardMatchedPoints();
                }
            }
            ActiveDocument.DigitizeState = DigitizeState.AxisState;
            activeView.Cursor = CursorFromState(DigitizeState.AxisState);
            activeView.Focus();

            form.DisableEditMenu();
        }

        public void ScaleBar_Click(object sender, System.EventArgs args)
        {
            if (ActiveDocument.DigitizeState == DigitizeState.PointMatchState && ActiveDocument.MatchSet != null && ActiveDocument.MatchSet.Matches.Count > 0)
            {
                if (MessageBox.Show("Would you like to save your matched points?", "Point Match complete", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    ActiveDocument.SaveMatchedPoints();
                    activeView.DrawAll();
                }
                else
                {
                    ActiveDocument.DiscardMatchedPoints();
                }
            }
            ActiveDocument.DigitizeState = DigitizeState.ScaleState;
            activeView.Cursor = CursorFromState(DigitizeState.ScaleState);
            activeView.Focus();

            form.DisableEditMenu();
        }

        public void CurvePoint_Click(object sender, System.EventArgs args)
        {
            if (ActiveDocument.DigitizeState == DigitizeState.PointMatchState && ActiveDocument.MatchSet != null && ActiveDocument.MatchSet.Matches.Count > 0)
            {
                ActiveDocument.DigitizeState = DigitizeState.CurveState;
                activeView.Cursor = CursorFromState(DigitizeState.CurveState);

                if (MessageBox.Show("Would you like to save your matched points?", "Point Match complete", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    ActiveDocument.SaveMatchedPoints();
                    activeView.DrawAll();
                }
                else
                {
                    ActiveDocument.DiscardMatchedPoints();
                }
            }
            ActiveDocument.DigitizeState = DigitizeState.CurveState;
            activeView.Cursor = CursorFromState(DigitizeState.CurveState);
            activeView.Focus();

            form.DisableEditMenu();
        }

        public void SegmentFill_Click(object sender, System.EventArgs args)
        {
            if (ActiveDocument.DigitizeState == DigitizeState.PointMatchState && ActiveDocument.MatchSet != null && ActiveDocument.MatchSet.Matches.Count > 0)
            {
                if (MessageBox.Show("Would you like to save your matched points?", "Point Match complete", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    ActiveDocument.SaveMatchedPoints();
                    activeView.DrawAll();
                }
                else
                {
                    ActiveDocument.DiscardMatchedPoints();
                }
            }
            ActiveDocument.DigitizeState = DigitizeState.SegmentState;
            activeView.Cursor = CursorFromState(DigitizeState.SegmentState);
            activeView.Focus();

            form.DisableEditMenu();
        }

        public void PointMatch_Click(object sender, System.EventArgs args)
        {
            ActiveDocument.DigitizeState = DigitizeState.PointMatchState;
            activeView.Cursor = CursorFromState(DigitizeState.PointMatchState);
            activeView.Focus();

            form.DisableEditMenu();
        }

        public void MeasurePoint_Click(object sender, System.EventArgs args)
        {
            if (ActiveDocument.DigitizeState == DigitizeState.PointMatchState && ActiveDocument.MatchSet.Matches.Count > 0)
            {
                ActiveDocument.DigitizeState = DigitizeState.MeasureState;
                activeView.Cursor = CursorFromState(DigitizeState.MeasureState);

                if (MessageBox.Show("Would you like to save your matched points?", "Point Match complete", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    ActiveDocument.SaveMatchedPoints();
                    activeView.DrawAll();
                }
                else
                {
                    ActiveDocument.DiscardMatchedPoints();
                }

                return;
            }

            ActiveDocument.DigitizeState = DigitizeState.MeasureState;
            activeView.Cursor = CursorFromState(DigitizeState.MeasureState);
            activeView.Focus();

            form.DisableEditMenu();
        }

        private Cursor CursorFromState(DigitizeState state)
        {
            switch (state)
            {
                case DigitizeState.AxisState: return Cursors.Cross;
                case DigitizeState.CurveState: return Cursors.Cross;
                case DigitizeState.MeasureState: return Cursors.Cross;
                case DigitizeState.PointMatchState: return Cursors.Arrow;
                case DigitizeState.ScaleState: return Cursors.Cross;
                case DigitizeState.SegmentState: return Cursors.Arrow;
                case DigitizeState.SelectState: return Cursors.Arrow;
            }

            return Cursors.Default;
        }

        #endregion

        #region Settings Menu
        

        /*
         * All the following methods work in a similar way.
         * 
         * The GUI/Dialogs directory contains all dialogs which take a settings
         * object and stores it.  The user manipulates GUI widgets which update
         * these settings in the dialog's state.  On the condition that the user 
         * presses the OK the settings from the dialog are stored into the active
         * document's setting.  Settings are persisted between saved documents,
         * the serialization of documents saves their settings as well as the point
         * data.
         * 
        */

        public void Coordinates_Click(object sender, System.EventArgs args)
        {
            CoordinatesDialog dlg = new CoordinatesDialog(ActiveDocument.CoordSettings);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                CoordSettings newSettings = ActiveDocument.CoordSettings;

                newSettings.frame = dlg.Frame;
                newSettings.thetaUnits = dlg.Units;
                newSettings.xThetaScale = dlg.XThetaScale;
                newSettings.yRScale = dlg.YRScale;

                ActiveDocument.CoordSettings = newSettings;
            }
        }

        public void Axes_Settings_Click(object sender, System.EventArgs args)
        {
            PointSettingsDialog dlg = new PointSettingsDialog(ActiveDocument.AxesStyle, true);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ActiveDocument.AxesStyle = dlg.Style;
                activeView.DrawAll();
                activeView.Refresh();
            }

        }

        public void ScaleBar_Settings_Click(object sender, System.EventArgs args)
        {
            PointSettingsDialog dlg = new PointSettingsDialog(ActiveDocument.ScaleStyle, true);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ActiveDocument.ScaleStyle = dlg.Style;
                activeView.DrawAll();
                activeView.Refresh();
            }
        }

        public void Curves_Settings_Click(object sender, System.EventArgs args)
        {
            PointsetsDialog dlg = new PointsetsDialog(ActiveDocument.PointSets.Curves, true);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                dlg.SetPointsets(ActiveDocument.PointSets.Curves);

                UpdateCurveList();
         
                activeView.DrawAll();
                activeView.Refresh();
            }
        }

        private void UpdateCurveList()
        {
            foreach (NuGenPointSet pointSet in ActiveDocument.PointSets.Curves)
            {
                form.AddCurve(pointSet.Name);
            }
        }

        private void UpdateMeasureList()
        {
            foreach (NuGenPointSet pointSet in ActiveDocument.PointSets.Measures)
            {
                form.AddMeasure(pointSet.Name);
            }
        }

        public void Segments_Settings_Click(object sender, System.EventArgs args)
        {
            SegmentsDialog dlg = new SegmentsDialog(ActiveDocument.SegmentSettings);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ActiveDocument.SegmentSettings = dlg.SegmentSettings;
                ActiveDocument.DrawSegments();
                activeView.DrawAll();
                activeView.Refresh();
            }
        }

        public void PointMatch_Settings_Click(object sender, System.EventArgs args)
        {
            PointMatchDialog dlg = new PointMatchDialog(ActiveDocument.PointMatchSettings);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ActiveDocument.PointMatchSettings = dlg.PointMatchSettings;
                activeView.DrawAll();
                activeView.Refresh();
            }
        }
        public void Measures_Settings_Click(object sender, System.EventArgs args)
        {
            PointsetsDialog dlg = new PointsetsDialog(ActiveDocument.PointSets.Measures, false);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                dlg.SetPointsets(ActiveDocument.PointSets.Measures);

                UpdateMeasureList();

                activeView.DrawAll();
                activeView.Refresh();
            }
        }

        public void Discretize_Settings_Click(object sender, System.EventArgs args)
        {
            Cursor prev = activeView.Cursor;
            activeView.Cursor = Cursors.WaitCursor;

            form.Refresh();
            activeView.Refresh();

            DiscretizeSettingsDialog dlg = new DiscretizeSettingsDialog(ActiveDocument.DiscretizeSettings, ActiveDocument.OriginalImage);

            activeView.Cursor = prev;            

            if(dlg.ShowDialog() == DialogResult.OK)
            {
                NuGenDiscretize discretize = new NuGenDiscretize(ActiveDocument.OriginalImage, dlg.DiscretizeSettings);

                discretize.Discretize();
                ActiveDocument.ProcessedImage = discretize.GetImage();

                ActiveDocument.DiscretizeSettings = dlg.DiscretizeSettings;

                ActiveDocument.Segments.MakeSegments(ActiveDocument.ProcessedImage, ActiveDocument.SegmentSettings);

                activeView.DrawAll();
                activeView.Refresh();
            }
        }

        public void GridRemoval_Settings_Click(object sender, System.EventArgs args)
        {
            Cursor prev = activeView.Cursor;
            activeView.Cursor = Cursors.WaitCursor;

            form.Refresh();
            activeView.Refresh();

            GridRemovalSettingsDialog dlg = new GridRemovalSettingsDialog(ActiveDocument);            

            activeView.Cursor = prev;     

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ActiveDocument.GridRemovalSettings = dlg.GridRemovalSettings;

                ActiveDocument.ProcessOriginialImage();

                activeView.DrawAll();
                activeView.Refresh();
            }
        }

        public void GridDisplay_Settings_Click(object sender, System.EventArgs args)
        {
            if (!(ActiveDocument.ValidScale || ActiveDocument.ValidAxes))
            {
                MessageBox.Show("Must define axes points in order to use grid lines");
                return;
            }

            GridSettingsDialog dlg = new GridSettingsDialog(ActiveDocument.GridDisplaySettings, ActiveDocument.CoordSettings);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ActiveDocument.GridDisplaySettings = dlg.Settings;

                ActiveDocument.MakeGridLines();

                activeView.DrawAll();
                activeView.Refresh();
            }
        }

        public void Export_Settings_Click(object sender, System.EventArgs args)
        {
            ExportSettingsDialog dlg = new ExportSettingsDialog(ActiveDocument.ExportSettings, ActiveDocument, ActiveDocument.PointSets.ExportIncluded(), ActiveDocument.PointSets.ExportExcluded());

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ActiveDocument.ExportSettings = dlg.Settings;
            }
        }

        public void Sessions_Settings_Click(object sender, System.EventArgs args) { }

        #endregion

        #region Window Menu
        public void Cascade_Click(object sender, System.EventArgs args)
        {
            form.LayoutMdi(MdiLayout.Cascade);
        }

        public void Tile_Click(object sender, System.EventArgs args)
        {
            form.LayoutMdi(MdiLayout.TileHorizontal);
        }
        #endregion

        //Forwarded from activeView, shows coordinates in the status bar
        public void Show_Coordinates(int x, int y)
        {
            double xTheta, yR;
            ActiveDocument.ScreenToXThetaYR(x, y, out xTheta, out yR);

            form.StatusCoordMessage(String.Format("{0},{1})", Math.Round(xTheta, 3), Math.Round(yR, 3)));
        }

        public void View_Activated(object sender, System.EventArgs args)
        {
            activeView = (NuGenView)sender;

            foreach (Form f in form.MdiChildren)
            {
                f.Text = "Inactive";
            }

            if (activeView != null)
            {
                activeView.Text = "Active";
            }
        }

        public void Refresh()
        {
            form.Refresh();
        }

        #endregion
    }
}
