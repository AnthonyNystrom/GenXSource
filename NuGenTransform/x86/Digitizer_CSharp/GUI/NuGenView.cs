using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Genetibase.NuGenTransform
{
    //View mode
    public enum ViewPointSelection
    {
        ViewAxesPoints,
        ViewScalePoints,
        ViewCurvePoints,
        ViewMeasurePoints,
        ViewAllPoints
    }

    //What to display on the panel
    public enum BackgroundSelection
    {
        BlankBackground,
        OriginalImage,
        ProcessedImage
    }

    public enum ReferenceFrame
    {
        Cartesian,
        Polar
    };

    // scale that applies separately to x/theta and y/r coordinates of a document
    public enum Scale
    {
        Linear,
        Log
    };

    // units that apply to theta polar graph coordinate of a document
    public enum ThetaUnits
    {
        ThetaDegrees,
        ThetaGradians,
        ThetaRadians
    };

    public enum DigitizeState
    {
        AxisState,
        CurveState,
        MeasureState,
        PointMatchState,
        ScaleState,
        SegmentState,
        SelectState
    };

    internal class InnerPanel : Panel
    {
        public InnerPanel()
        {
            DoubleBuffered = true;
        }
    }

    public class NuGenView : Form, NuGenImageListener
    {   
        private ViewPointSelection viewPointSelection;

        private NuGenDocument doc;

        private bool showGridlines = false;

        //All of the foreground layers
        private Bitmap backBuffer;

        private InnerPanel imagePanel;

        //Indentifies which curves are selected in the combo boxes.
        private string curveSelected;
        private string measureSelected;    

        //The original location of a click and drag command to move multiple points.
        private Point cursorLocation;

        //Is this view's document ready to display?
        private bool initialize = false;

        private List<NuGenPoint> selectedPointGestatingList;
        private List<NuGenPoint> editPointsList;

        public delegate void Delegate_SendStatusMessage(string message);
        private Delegate_SendStatusMessage sendStatusMessage;

        //Delegate for forwarding status messages somewhere

        public Delegate_SendStatusMessage SendStatusMessage
        {
            get
            {
                return sendStatusMessage;
            }

            set
            {
                sendStatusMessage = value;
            }
        }

        protected Graphics Buffer
        {
            get
            {
                return Graphics.FromImage(backBuffer);
            }
        }

        public ViewPointSelection ViewPointSelection
        {
            get
            {
                return viewPointSelection;
            }

            set
            {
                viewPointSelection = value;
            }

        }

        public NuGenView(Form parent, NuGenDocument doc, Delegate_SendStatusMessage sendStatusMessage)
        {
            MdiParent = parent;
            Visible = true;
            WindowState = FormWindowState.Maximized;
            int x = Genetibase.NuGenTransform.Properties.Settings.Default.WINDOW_MEASURE_WIDTH;
            int y = Genetibase.NuGenTransform.Properties.Settings.Default.WINDOW_MEASURE_HEIGHT;

            this.sendStatusMessage = sendStatusMessage;
            imagePanel = new InnerPanel();

            Size = new Size(x, y);
            
            viewPointSelection = NuGenDefaultSettings.GetInstance().ViewPointSelection;

            this.doc = doc;
            imagePanel.Paint += new PaintEventHandler(NuGenView_Paint);
            imagePanel.MouseClick += new MouseEventHandler(NuGenView_MouseClick);
            imagePanel.MouseDown += new MouseEventHandler(NuGenView_MousePress);
            imagePanel.MouseUp += new MouseEventHandler(NuGenView_MouseRelease);
            imagePanel.MouseMove += new MouseEventHandler(NuGenView_MouseMove);
            imagePanel.MouseDoubleClick += new MouseEventHandler(NuGenView_MouseDoubleClick);

            Controls.Add(imagePanel);

            curveSelected = NuGenPointSetCollection.DefaultCurveName;
            measureSelected = NuGenPointSetCollection.DefaultMeasureName;
            doc.ActiveCurveName = curveSelected;
            doc.ActiveMeasureName = measureSelected;

            selectedPointList = new List<NuGenPoint>();
            selectedPointGestatingList = new List<NuGenPoint>();
            editPointsList = new List<NuGenPoint>();

            this.DoubleBuffered = true;
            this.AutoScroll = true;
        }

        void NuGenView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CheckForPointIntersection(e.X, e.Y);

            foreach (NuGenPoint p in selectedPointList)
            {
                if (p.PointSet == doc.PointSets.Axes)
                {
                    AxisPointDialog dlg = new AxisPointDialog();
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        GridMeshSettings settings = doc.GridDisplaySettings;
                        GridRemovalSettings removal = doc.GridRemovalSettings;

                        settings.initialized = false;
                        removal.gridMesh.initialized = false;

                        doc.GridRemovalSettings = removal;
                        doc.GridDisplaySettings = settings;

                        doc.SetAxisPoint(p, dlg.XTheta, dlg.YR);
                    }
                    else
                    {
                        doc.RemovePoint(p);
                    }

                    DrawAll();
                }
                else if (p.PointSet == doc.PointSets.ScaleBar)
                {
                    ScaleBarDialog sdlg = new ScaleBarDialog();
                    if (sdlg.ShowDialog() == DialogResult.OK)
                    {
                        NuGenPoint other = new NuGenPoint(0, 0);

                        //Get the other scale point, if it exists, which it should if you are moving the points
                        foreach (NuGenPoint p2 in doc.PointSets.ScaleBar.Points)
                        {
                            if (p2 != p)
                            {
                                other = p;
                                break;
                            }
                        }

                        GridMeshSettings settings = doc.GridDisplaySettings;
                        GridRemovalSettings removal = doc.GridRemovalSettings;

                        settings.initialized = false;
                        removal.gridMesh.initialized = false;

                        doc.SetScalePoint(other, 0, 0);
                        doc.SetScalePoint(p, sdlg.Length, 0);
                    }

                    DrawAll();
                }

            }
        }        

        private void RejectPoint()
        {
            doc.MatchSet.Current.Reject();
            doc.MatchSet.MovePrev();
        }

        private void AcceptPoint()
        {
            doc.MatchSet.Current.Accept();
            doc.MatchSet.MoveNext();
        }

        //Deletes all of the points in the selected points list        
        private void DeletePoints()
        {
            foreach (NuGenPoint p in selectedPointList)
            {
                doc.RemovePoint(p);
            }

            selectedPointGestatingList.Clear();
            selectedPointList.Clear();

            Graphics g = Buffer;

            DrawCurvePoints(g);
            DrawMeasurePoints(g);
            DrawAxes(g);
            DrawScaleBar(g);
            DrawSelectedPoints(g);
            DrawGridLines(g);

            switch (doc.AxisPointCount)
            {
                case 1:
                    sendStatusMessage("One axis point defined.  Need two more"); break;
                case 2:
                    sendStatusMessage("Two axes points defined.  Need one more"); break;
                case 3:
                    sendStatusMessage("Axes Defined"); break;
                case 0:
                    switch (doc.ScalePointCount)
                    {
                        case 2:
                            sendStatusMessage("Scale Bar Defined"); break;
                        default:
                            sendStatusMessage("Three axis points or the scale bar must be defined."); break;
                    } break;
            }

            Refresh();
        }

        public string ActiveCurveName
        {
            get
            {
                return curveSelected;
            }

            set
            {
                curveSelected = value;
                doc.ActiveCurveName = value;
            }
        }

        public string ActiveMeasureName
        {
            get
            {
                return measureSelected;
            }

            set
            {
                measureSelected = value;
                doc.ActiveMeasureName = value;
            }
        }

        void NuGenView_MouseClick(object sender, MouseEventArgs e)
        {
            Focus();
            cursorLocation = new Point(e.X, e.Y);

            Graphics g = Buffer;

            switch (doc.DigitizeState)
            {
                case DigitizeState.AxisState:
                    {
                        if (doc.AxisPointCount == 3)
                        {
                            MessageBox.Show("You can only create 3 axis points", "Axis Points Defined", MessageBoxButtons.OK);
                            return;
                        }

                        NuGenPoint p = doc.AddPoint(e.X, e.Y);
                        DrawAxes(g);
                        Refresh();

                        AxisPointDialog dlg = new AxisPointDialog();
                        DialogResult result = dlg.ShowDialog();                        

                        if (result == DialogResult.Cancel)
                        {
                            doc.RemovePoint(p);
                            DrawAll();
                            Refresh();
                            return;
                        }

                        if (!doc.SetAxisPoint(p, dlg.XTheta, dlg.YR))
                        {
                            sendStatusMessage("One or more axis points need to be redefined");
                        }
                        else
                        {
                            switch (doc.AxisPointCount)
                            {
                                case 1:
                                    sendStatusMessage("One axis point defined.  Need two more"); break;
                                case 2:
                                    sendStatusMessage("Two axes points defined.  Need one more"); break;
                                case 3:
                                    sendStatusMessage("Axes Defined"); break;
                            }
                        }

                        break;
                    }
                case DigitizeState.CurveState:
                    {
                        if (doc.ProcessedImage.Width < e.X || doc.ProcessedImage.Height < e.Y)
                            return;

                        NuGenPoint p = doc.AddPoint(e.X, e.Y);

                        DrawAll();

                        break;
                    }
                case DigitizeState.MeasureState:
                    {
                        if (doc.PointSets.GetMeasure(doc.ActiveMeasureName).Points.Count != 0)
                            return;

                        if (doc.ProcessedImage.Width < e.X || doc.ProcessedImage.Height < e.Y)
                            return;

                        NuGenPoint p = doc.AddPoint(e.X, e.Y);
                        DrawAll();

                        break;
                    }
                case DigitizeState.PointMatchState:
                    {
                        if(!doc.MatchSamplePoint(new Point(e.X, e.Y)))
                        {
                            MessageBox.Show("You must select a suitable match point.", "Unsuitable Point");
                        }

                        DrawAll();
                        break;
                    }
                case DigitizeState.ScaleState:
                    break;
                case DigitizeState.SegmentState:
                    if (selectedSegment == null)
                        break;

                    foreach (NuGenPoint p in selectedSegment.FillPoints(doc.SegmentSettings))
                    {
                        doc.AddPoint(p);
                    }

                    DrawAll();

                    break;
                case DigitizeState.SelectState:
                    break;
            }
        }

        //Gestating points for the scale bar
        private NuGenPoint scaleStart;
        private NuGenPoint scaleEnd;

        private NuGenPoint gestatingMeasurePoint;

        void NuGenView_MousePress(object sender, MouseEventArgs e)
        {
            cursorLocation = new Point(e.X, e.Y);

            if (doc.ProcessedImage.Width < e.X || doc.ProcessedImage.Height < e.Y)
                return;

            if (doc.DigitizeState == DigitizeState.SelectState)
            {
                //In the select state a mouse click means the user is trying to select a point
                CheckForPointIntersection(e.X, e.Y);
                //DrawSelectedPoints(Buffer);
                //Refresh();
                DrawAll();
                return;
            }

            if (doc.DigitizeState == DigitizeState.MeasureState)
            {
                if (doc.PointSets.GetMeasure(doc.ActiveMeasureName).Points.Count >= 1)
                {
                    gestatingMeasurePoint = doc.AddPoint(e.X, e.Y);

                    DrawMeasurePoints(Buffer);
                    Refresh();
                }
            }

            if (doc.DigitizeState != DigitizeState.ScaleState)
                return;

            if (doc.ValidScale)
            {
                MessageBox.Show("Scale Bar is already defined, to change it use the selection tool");
                return;
            }

            scaleStart = doc.AddPoint(e.X, e.Y);
            scaleEnd = doc.AddPoint(e.X, e.Y);

            DrawScaleBar(Buffer);
            Refresh();
        }

        void NuGenView_MouseRelease(object sender, MouseEventArgs e)
        {
            cursorLocation = new Point(e.X, e.Y);

            if (doc.DigitizeState == DigitizeState.SelectState)
            {
                if (dragged && selectedPointList.Count > 0)
                {
                    //Remove all the original points
                    foreach (NuGenPoint p in selectedPointGestatingList)
                    {
                        doc.RemovePoint(p);
                    }

                    //Clear them as well
                    selectedPointList.Clear();

                    //Add the new points to the list
                    selectedPointList.AddRange(selectedPointGestatingList);
                    selectedPointGestatingList.Clear();

                    List<NuGenPoint> tempList = new List<NuGenPoint>();

                    //And connect them internally
                    foreach (NuGenPoint p in selectedPointList)
                    {
                        NuGenPoint newPoint = doc.AddPoint(p);
                        //Add this point to a temp list so that selected items list stays up to date
                        tempList.Add(newPoint);

                        if (p.PointSet == doc.PointSets.Axes)
                        {                            
                            AxisPointDialog dlg = new AxisPointDialog();
                            if (dlg.ShowDialog() == DialogResult.OK)
                            {
                                GridMeshSettings settings = doc.GridDisplaySettings;
                                GridRemovalSettings removal = doc.GridRemovalSettings;

                                settings.initialized = false;
                                removal.gridMesh.initialized = false;

                                doc.GridRemovalSettings = removal;
                                doc.GridDisplaySettings = settings;

                                doc.SetAxisPoint(newPoint, dlg.XTheta, dlg.YR);
                            }
                            else
                            {
                                doc.RemovePoint(p);
                            }

                            DrawAll();
                        }
                        else if (p.PointSet == doc.PointSets.ScaleBar)
                        {

                            //Tests to see if there are two scale points in the selection
                            bool cont = false;
                            bool found = false;

                            foreach(NuGenPoint pNew in selectedPointList)
                            {
                                if (pNew.PointSet == doc.PointSets.ScaleBar)
                                {
                                    if (found == true)
                                    {
                                        cont = true;
                                        break;
                                    }

                                    found = true;
                                }
                            }

                            //No need to readjust scale point values if both were moved
                            if (cont)
                                continue;

                            ScaleBarDialog sdlg = new ScaleBarDialog();
                            if (sdlg.ShowDialog() == DialogResult.OK)
                            {
                                NuGenPoint other = new NuGenPoint(0,0);

                                //Get the other scale point, if it exists, which it should if you are moving the points
                                foreach (NuGenPoint p2 in doc.PointSets.ScaleBar.Points)
                                {
                                    if (p2 != p)
                                    {
                                        other = p;
                                        break;
                                    }
                                }

                                GridMeshSettings settings = doc.GridDisplaySettings;
                                GridRemovalSettings removal = doc.GridRemovalSettings;

                                settings.initialized = false;
                                removal.gridMesh.initialized = false;

                                doc.SetScalePoint(other, 0, 0);
                                doc.SetScalePoint(p, sdlg.Length, 0);
                                doc.Units = sdlg.Units;
                            }

                            DrawAll();
                        }

                    }

                    selectedPointList = tempList;

                    return;

                }

                if (selectionBox.Size.Width != 0 && selectionBox.Height != 0)
                {                                        
                    SelectRegion(selectionBox);
                    DrawAll();
                }

                dragged = false;
            }

            if (doc.DigitizeState == DigitizeState.MeasureState)
            {
                gestatingMeasurePoint = null;

                DrawAll();
                Refresh();
            }

            if (doc.DigitizeState != DigitizeState.ScaleState)
                return;

            if (doc.ProcessedImage.Width < e.X || doc.ProcessedImage.Height < e.Y)
                return;

            if (scaleStart == null || scaleEnd == null)
                return;

            if (scaleStart.XScreen == e.X && scaleStart.YScreen == e.Y)
            {
                MessageBox.Show("Scale bar is drawn by clicking and dragging. You must drag to a new point");

                doc.RemovePoint(scaleStart);
                doc.RemovePoint(scaleEnd);

                scaleStart = null;
                scaleEnd = null;

                DrawAll();

                return;
            }

            ScaleBarDialog sdlg2 = new ScaleBarDialog();
            sdlg2.ShowDialog();
            double length = sdlg2.Length;

            if (!(doc.SetScalePoint(scaleStart, 0.0, 0.0) && doc.SetScalePoint(scaleEnd, length, 0.0)))
            {
                sendStatusMessage("One or more scale points must be redefined");
            }
            else
            {
                doc.Units = sdlg2.Units;
            }

            scaleStart = null;
            scaleEnd = null;

            if (doc.ValidScale)
            {
                sendStatusMessage("Scale Bar Defined");
            }

            DrawScaleBar(Buffer);
            Refresh();
        }

        private bool dragged;
        public delegate void Show_Coordinates(int x, int y);

        private Show_Coordinates showCoordinates;

        //Delegate method for showing the current graph coordinates
        public Show_Coordinates ShowCoordinates
        {
            get
            {
                return showCoordinates;
            }

            set
            {
                showCoordinates = value;
            }
        }

        //Used primarily for dragging operations, hence the "dragged" boolean flag
        void NuGenView_MouseMove(object sender, MouseEventArgs e)
        {
            showCoordinates(e.X, e.Y);

            Graphics g = Buffer;

            if (doc.DigitizeState == DigitizeState.SelectState && e.Button != MouseButtons.None)
            {
                if (e.X == cursorLocation.X && e.Y == cursorLocation.Y)
                    return;

                dragged = true;

                if (selectedPointList.Count > 0)
                {

                    foreach (NuGenPoint p in selectedPointGestatingList)
                    {
                        doc.RemovePoint(p);
                    }

                    foreach (NuGenPoint p in selectedPointList)
                    {
                        doc.RemovePoint(p);
                    }
                    
                    List<NuGenPoint> clearList = new List<NuGenPoint>();
                    clearList.AddRange(selectedPointGestatingList);
                    selectedPointGestatingList.Clear();

                    int xDiff = e.X - cursorLocation.X;
                    int yDiff = e.Y - cursorLocation.Y;

                    if (xDiff == 0 && yDiff == 0)
                        return;

                    foreach (NuGenPoint p in selectedPointList)
                    {
                        NuGenPoint newPoint = doc.AddPoint(p.XScreen + xDiff, p.YScreen + yDiff, p.PointSet);
                        selectedPointGestatingList.Add(newPoint);
                    }

                    DrawAll();

                    return;
                }
                else
                {
                    if (cursorLocation.X == e.X && cursorLocation.Y == e.Y)
                        return;

                    g.Clear(Color.Transparent);
                    DrawAll(new Region(new Rectangle(0,0,backBuffer.Width, backBuffer.Height)));
                    DrawSelectionRectangle(e.X, e.Y, g);
                    Refresh();
                }
            }

            if (doc.ProcessedImage.Width < e.X || doc.ProcessedImage.Height < e.Y)
                return;

            if (doc.DigitizeState == DigitizeState.SegmentState)
            {
                CheckForSegmentIntersection(e.X, e.Y);
                DrawAll();
                return;
            }

            if (doc.DigitizeState == DigitizeState.PointMatchState)
            {
                doc.HighlightCandidateMatchPoint(new Point(e.X, e.Y));
                DrawAll();
                DrawMatchHighlights(Buffer);
                Refresh();
                return;
            }

            if (doc.DigitizeState == DigitizeState.MeasureState)
            {
                if (gestatingMeasurePoint == null)
                    return;

                doc.RemovePoint(gestatingMeasurePoint);

                gestatingMeasurePoint = doc.AddPoint(e.X, e.Y);

                DrawAll();
            }

            if (scaleStart == null || scaleEnd == null)
                return;            

            if (doc.DigitizeState != DigitizeState.ScaleState)
                return;

            doc.RemovePoint(scaleEnd);

            scaleEnd = doc.AddPoint(e.X, e.Y);

            DrawAll();
        }

        void NuGenView_Paint(object sender, PaintEventArgs e)
        {
            if (initialize == false)
                return;

            e.Graphics.DrawImage(backBuffer, new Point(0, 0));
        }

        //Clear and paint the entire screen
        public void DrawAll()
        {
            Buffer.Clear(Color.Transparent);
            DrawAll(new Region(new Rectangle(0, 0, backBuffer.Width, backBuffer.Height)));

            Refresh();
        }

        /*
         * 
         * All of the methods below are mostly straightforward...
         * 
         * They take the data from the document and draw a visible representation of it
         * 
         * */
        
        private void DrawGridLines(Graphics g)
        {
            if (doc.ValidAxes == false && doc.ValidScale == false)
                return;

            Pen pen = new Pen(Color.Black);

            foreach (GridlineScreen line in doc.MakeGridLines())
            {
                g.DrawLine(pen, line.Start.X, line.Start.Y, line.Stop.X, line.Stop.Y);
            }
        }

        private void DrawScaleBar(Graphics g)
        {
            DrawPoints(doc.PointSets.ScaleBar, g);
        }

        private void DrawMeasurePoints(Graphics g)
        {
            foreach (NuGenPointSet pointSet in doc.PointSets.Measures)
            {
                DrawPoints(pointSet, g);
            }

            if (gestatingMeasurePoint != null && doc.ValidScale && gestatingMeasurePoint.PreviousLine != null)
            {
                Point p1 = gestatingMeasurePoint.PreviousLine.start;
                Point p2 = gestatingMeasurePoint.PreviousLine.end;

                Point midpt = new Point(Math.Min(p1.X, p2.X) + Math.Abs(p1.X - p2.X) / 2, Math.Min(p1.Y,p2.Y) + Math.Abs(p1.Y - p2.Y) / 2);

                double p1xTheta, p1yR;
                doc.ScreenToXThetaYR(p1.X, p1.Y, out p1xTheta, out p1yR);

                double p2xTheta, p2yR;
                doc.ScreenToXThetaYR(p2.X, p2.Y, out p2xTheta, out p2yR);                

                double dist = Math.Sqrt(Math.Pow(p1xTheta - p2xTheta, 2) + Math.Pow(p1yR - p2yR, 2));
                dist = Math.Round(dist, 3);

                String val = dist.ToString() + " " + Abbreviation(doc.Units);
                
                g.FillRectangle(Brushes.LightGray, new Rectangle(midpt.X - 2, midpt.Y - 2, (int)(val.Length * 7), (int)(this.Font.Height)+ 2));
                g.DrawRectangle(Pens.Black, new Rectangle(midpt.X - 2, midpt.Y - 2, (int)(val.Length * 7), (int)(this.Font.Height) + 2));
                g.DrawString(val, this.Font, Brushes.Black, midpt.X, midpt.Y);
            }

            if (gestatingMeasurePoint != null && doc.ValidScale && gestatingMeasurePoint.NextLine != null)
            {
                Point p1 = gestatingMeasurePoint.NextLine.start;
                Point p2 = gestatingMeasurePoint.NextLine.end;

                Point midpt = new Point(Math.Min(p1.X, p2.X) + Math.Abs(p1.X - p2.X) / 2, Math.Min(p1.Y, p2.Y) + Math.Abs(p1.Y - p2.Y) / 2);

                double p1xTheta, p1yR;
                doc.ScreenToXThetaYR(p1.X, p1.Y, out p1xTheta, out p1yR);

                double p2xTheta, p2yR;
                doc.ScreenToXThetaYR(p2.X, p2.Y, out p2xTheta, out p2yR);

                double dist = Math.Sqrt(Math.Pow(p1xTheta - p2xTheta, 2) + Math.Pow(p1yR - p2yR, 2));
                dist = Math.Round(dist, 3);

                String val = dist.ToString() + " " + Abbreviation(doc.Units);

                g.FillRectangle(Brushes.LightGray, new Rectangle(midpt.X - 2, midpt.Y - 2, (int)(val.Length * 7), (int)(this.Font.Height) + 2));
                g.DrawRectangle(Pens.Black, new Rectangle(midpt.X - 2, midpt.Y - 2, (int)(val.Length * 7), (int)(this.Font.Height) + 2));
                g.DrawString(val, this.Font, Brushes.Black, midpt.X, midpt.Y);
            }
        }

        private string Abbreviation(LengthUnits lengthUnits)
        {
            switch (lengthUnits)
            {
                case LengthUnits.Angstrom :
                    return "Å";
                case LengthUnits.AstronomicalUnit:
                    return "AU";
                case LengthUnits.Centimeter :
                    return "cm";
                case LengthUnits.Decimeter :
                    return "dm";
                case LengthUnits.Fathom:
                    return "fathom";
                case LengthUnits.Foot:
                    return "ft";
                case LengthUnits.Inch:
                    return "in";
                case LengthUnits.Kilometer:
                    return "km";
                case LengthUnits.LightYear:
                    return "ly";
                case LengthUnits.Meter:
                    return "m";
                case LengthUnits.Micrometer:
                    return "mµ";
                case LengthUnits.Mile:
                    return "mile";
                case LengthUnits.Milimeter:
                    return "mm";
                case LengthUnits.Nanometer:
                    return "nm";
                case LengthUnits.Parsec:
                    return "parsec";
                case LengthUnits.Picometer:
                    return "pm";
                case LengthUnits.Unitless:
                    return "";
                case LengthUnits.Yard:
                    return "yd";
            }

            return "";
        }

        private void DrawCurvePoints(Graphics g)
        {            
            foreach (NuGenPointSet pointSet in doc.PointSets.Curves)
            {
                DrawPoints(pointSet, g);
            }
        }

        private void DrawPoints(NuGenPointSet pointSet, Graphics g)
        {
            DrawPoints(pointSet.Points, g);
        }

        private void DrawPoints(List<NuGenPoint> points, Graphics g)
        {
            foreach (NuGenPoint p in points)
            {
                PointSetStyle style = p.PointSet.Style;

                Pen pointPen = new Pen(style.pointLineColor, (int)style.pointLineSize);

                Pen linePen = new Pen(style.lineColor, (int)style.lineSize);
                int size = (int)style.pointSize;

                Brush brush = new SolidBrush(style.pointInColor);

                switch (style.pointShape)
                {
                    case PointShape.Cross:
                        DrawCross(g, pointPen, linePen, size, new Point(p.XScreen, p.YScreen));
                        break;
                    case PointShape.Diamond:
                        DrawDiamond(g, pointPen, linePen, size, brush, new Point(p.XScreen, p.YScreen));
                        break;
                    case PointShape.Square:
                        DrawSquare(g, pointPen, linePen, size, brush, new Point(p.XScreen, p.YScreen));
                        break;
                    case PointShape.Triangle:
                        DrawTriangle(g, pointPen, linePen, size, brush, new Point(p.XScreen, p.YScreen));
                        break;
                    case PointShape.X:
                        DrawX(g, pointPen, linePen, size, new Point(p.XScreen, p.YScreen));
                        break;
                }

                if (p.NextLine != null)
                {
                    g.DrawLine(linePen, p.NextLine.start.X, p.NextLine.start.Y, p.NextLine.end.X, p.NextLine.end.Y);
                }
            }
        }

        private void DrawPoints(List<NuGenPoint> points, PointSetStyle style, Graphics g)
        {
            Pen pointPen = new Pen(style.pointLineColor, (int)style.pointLineSize);

            Pen linePen = new Pen(style.lineColor, (int)style.lineSize);
            int size = (int)style.pointSize;

            Brush brush = new SolidBrush(style.pointInColor);

            foreach (NuGenPoint p in points)
            {
                switch (style.pointShape)
                {
                    case PointShape.Cross:
                        DrawCross(g, pointPen, linePen, size, new Point(p.XScreen, p.YScreen));
                        break;
                    case PointShape.Diamond:
                        DrawDiamond(g, pointPen, linePen, size, brush, new Point(p.XScreen, p.YScreen));
                        break;
                    case PointShape.Square:
                        DrawSquare(g, pointPen, linePen, size, brush, new Point(p.XScreen, p.YScreen));
                        break;
                    case PointShape.Triangle:
                        DrawTriangle(g, pointPen, linePen, size, brush, new Point(p.XScreen, p.YScreen));
                        break;
                    case PointShape.X:
                        DrawX(g, pointPen, linePen, size, new Point(p.XScreen, p.YScreen));
                        break;
                }

                if (p.NextLine != null)
                {
                    g.DrawLine(linePen, p.NextLine.start.X, p.NextLine.start.Y, p.NextLine.end.X, p.NextLine.end.Y);
                }
            }
        }

        public static void DrawPoint(Graphics g, Point p, PointSetStyle style)
        {
            Pen pointPen = new Pen(style.pointLineColor, (int)style.pointLineSize);
            Pen linePen = new Pen(style.lineColor, (int)style.lineSize);
            Brush pointBrush = new SolidBrush(style.pointInColor);
            int size = (int)style.pointSize;

            switch (style.pointShape)
            {
                case PointShape.Cross:
                    DrawCross(g, pointPen, linePen, size, p); break;
                case PointShape.Diamond:
                    DrawDiamond(g, pointPen, linePen, size, pointBrush, p); break;
                case PointShape.Square:
                    DrawSquare(g, pointPen, linePen, size, pointBrush, p); break;
                case PointShape.Triangle:
                    DrawTriangle(g, pointPen, linePen, size, pointBrush, p); break;
                case PointShape.X:
                    DrawX(g, pointPen, linePen, size, p); break;
            }
        }

        private static void DrawX(Graphics g, Pen pointPen, Pen linePen, int size, Point p)
        {
            g.DrawLine(pointPen, p.X - 3 * size, p.Y + 3 * size, p.X + 3 * size, p.Y - 3 * size);
            g.DrawLine(pointPen, p.X + 3 * size, p.Y + 3 * size, p.X - 3 * size, p.Y - 3 * size);
        }

        private static void DrawTriangle(Graphics g, Pen pointPen, Pen linePen, int size, Brush brush, Point p)
        {
            Point[] points = {new Point(p.X - 3 * size, p.Y + 3 * size), new Point(p.X, p.Y - 3 * size)
                            , new Point(p.X + 3 * size, p.Y + 3 * size)};

            g.FillPolygon(brush, points);
            g.DrawPolygon(pointPen, points);
        }

        private static void DrawSquare(Graphics g, Pen pointPen, Pen linePen, int size, Brush brush, Point p)
        {
            g.FillRectangle(brush, new Rectangle(p.X - 3 * size, p.Y - 3 * size, 3 * size * 2, 3 * size * 2));
            g.DrawRectangle(pointPen, new Rectangle(p.X - 3 * size, p.Y - 3 * size, 3 * size * 2, 3 * size * 2));
        }

        private static void DrawDiamond(Graphics g, Pen pointPen, Pen linePen, int size, Brush brush, Point p)
        {
            Point[] points = {new Point(p.X - 3 * size, p.Y) , new Point(p.X, p.Y - 3 * size)
                        , new Point(p.X + 3 * size, p.Y) , new Point(p.X, p.Y + 3 * size)};

            g.FillPolygon(brush, points);
            g.DrawPolygon(pointPen, points);
        }

        private static void DrawCross(Graphics g, Pen pointPen, Pen linePen, int size, Point p)
        {
            g.DrawLine(pointPen, p.X - 3 * size, p.Y, p.X + 3 * size, p.Y);
            g.DrawLine(pointPen, p.X, p.Y - 3 * size, p.X, p.Y + 3 * size);
        }

        private void DrawAxes(Graphics g)
        {
            DrawPoints(doc.PointSets.Axes, g);
        }

        private void DrawSegments(Graphics g)
        {
            NuGenSegmentCollection segments = doc.Segments;            

            Pen pen = new Pen(doc.SegmentSettings.lineColor, (int)doc.SegmentSettings.lineSize);

            foreach (NuGenSegment segment in segments.Segments)
            {
                foreach (SegmentLine line in segment.Lines)
                {                    
                    g.DrawLine(pen, line.StartPoint, line.EndPoint);
                }
            }
        }

        private void DrawMatchPoints(Graphics g)
        {
            foreach (MatchPoint p in doc.MatchSet.Matches)
            {
                Pen accepted = new Pen(doc.PointMatchSettings.acceptedColor, 1);
                Pen rejected = new Pen(doc.PointMatchSettings.rejectedColor, 1);

                if (p.Visible)
                {
                    if (p.Accepted)
                        DrawCross(g, accepted, Pens.Transparent, 3, p.Point);
                    else
                        DrawCross(g, rejected, Pens.Transparent, 3, p.Point);
                }
                    
            }

            DrawMatchBoxes(g);
        }

        private void DrawMatchBoxes(Graphics g)
        {            
            if (doc.MatchSet.Matches.Count == 0)
                return;

            Point acceptedPoint = doc.MatchSet.Matches[doc.MatchSet.Matches.IndexOf(doc.MatchSet.Current) - 1].Point;
            Point nextPoint = doc.MatchSet.Current.Point;

            Pen acceptBox = new Pen(doc.PointMatchSettings.acceptedColor, doc.PointMatchHighlightLineSize);
            Pen nextBox = new Pen(doc.PointMatchSettings.rejectedColor, doc.PointMatchHighlightLineSize);

            int offset = doc.PointMatchHighlightDiameter / 2;

            g.DrawRectangle(acceptBox, Rectangle.FromLTRB(acceptedPoint.X - offset, acceptedPoint.Y - offset, acceptedPoint.X + offset, acceptedPoint.Y + offset));
            g.DrawRectangle(nextBox, Rectangle.FromLTRB(nextPoint.X - offset, nextPoint.Y - offset, nextPoint.X + offset, nextPoint.Y + offset));
        }

        private void DrawMatchHighlights(Graphics g)
        {
            if (doc.SamplePointPixels.Count == 0)
                return;

            int left, right, down, up;
            left = int.MaxValue; right = 0; down = int.MaxValue; up = 0;

            foreach (Point p in doc.SamplePointPixels)
            {
                if (p.X < left)
                    left = p.X;
                if (p.X > right)
                    right = p.X;
                if (p.Y < down)
                    down = p.Y;
                if (p.Y > up)
                    up = p.Y;
            }

            //add some padding
            left -= 7;
            right += 5;
            up -= 15;
            down += 10;

            DrawSelectionBounds(g, left, right, up, down);
        }

        private void DrawSelectedGestatingPoints(Graphics g)
        {
            DrawSelectedPoints(selectedPointGestatingList, g);
        }

        private void DrawSelectedPoints(Graphics g)
        {
            DrawSelectedPoints(selectedPointList, g);
        }

        private void DrawSelectedPoints(List<NuGenPoint> selectedPoints, Graphics g)
        {            
            if (selectedPoints.Count > 0)
            {
                int left, right, down, up;
                left = int.MaxValue; right = 0; up = int.MaxValue; down = 0;

                foreach (NuGenPoint p in selectedPoints)
                {
                    if (p.XScreen - (int)p.PointSet.Style.pointSize * 4 < left)
                        left = p.XScreen - (int)p.PointSet.Style.pointSize * 4;
                    if (p.XScreen + (int)p.PointSet.Style.pointSize * 4 > right)
                        right = p.XScreen + (int)p.PointSet.Style.pointSize * 4;
                    if (p.YScreen - (int)p.PointSet.Style.pointSize * 4 < up)
                        up = p.YScreen - (int)p.PointSet.Style.pointSize * 4;
                    if (p.YScreen + (int)p.PointSet.Style.pointSize * 4 > down)
                        down = p.YScreen + (int)p.PointSet.Style.pointSize * 4;                    
                };

                left -= 3;
                up -= 5;

                DrawSelectionBounds(g, left, right, up, down);
            }
        }

        private Rectangle selectionBox;

        private void DrawSelectionRectangle(int x, int y, Graphics g)
        {
            int left = x < cursorLocation.X ? x : cursorLocation.X;
            int right = x > cursorLocation.X ? x : cursorLocation.X;
            int top = y < cursorLocation.Y ? y : cursorLocation.Y;
            int bottom = y > cursorLocation.Y ? y : cursorLocation.Y;
            selectionBox = Rectangle.FromLTRB(left, top, right, bottom);

            Pen p = new Pen(Color.Black);
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            g.DrawRectangle(p, selectionBox);
        }

        private void DrawSelectedSegments(Graphics g)
        {            
            if (selectedSegment != null)
            {
                int left, right, down, up;
                left = int.MaxValue; right = 0; up = int.MaxValue; down = 0;

                foreach (SegmentLine line in selectedSegment.Lines)
                {
                    if (line.StartPoint.X < left)
                        left = line.StartPoint.X;
                    if (line.StartPoint.X > right)
                        right = line.StartPoint.X;
                    if (line.StartPoint.Y < up)
                        up = line.StartPoint.Y;
                    if (line.StartPoint.Y > down)
                        down = line.StartPoint.Y;

                    if (line.EndPoint.X < left)
                        left = line.EndPoint.X;
                    if (line.EndPoint.X > right)
                        right = line.EndPoint.X;
                    if (line.EndPoint.Y < up)
                        up = line.EndPoint.Y;
                    if (line.EndPoint.Y > down)
                        down = line.EndPoint.Y;
                }

                left -= 5;
                right += 5;
                up -= 10;
                down += 5;

                DrawSelectionBounds(g, left, right, up, down);
            }
        }

        private void DrawSelectionBounds(Graphics g, int left, int right, int up, int down)
        {

            Brush selectionBrush = Brushes.Lime;

            g.FillRectangle(selectionBrush, left - 2, up - 2, 4, 4);
            g.FillRectangle(selectionBrush, right + 2, up - 2, 4, 4);
            g.FillRectangle(selectionBrush, left - 2, down + 2, 4, 4);
            g.FillRectangle(selectionBrush, right + 2, down + 2, 4, 4);

            g.FillRectangle(selectionBrush, (right + left) / 2, down + 2, 4, 4);
            g.FillRectangle(selectionBrush, (right + left) / 2, up - 2, 4, 4);
            g.FillRectangle(selectionBrush, right + 2, (up + down) / 2, 4, 4);
            g.FillRectangle(selectionBrush, left - 2, (up + down) / 2, 4, 4);

            Pen selectionPen = Pens.Black;

            g.DrawRectangle(selectionPen, left - 2, up - 2, 4, 4);
            g.DrawRectangle(selectionPen, right + 2, up - 2, 4, 4);
            g.DrawRectangle(selectionPen, left - 2, down + 2, 4, 4);
            g.DrawRectangle(selectionPen, right + 2, down + 2, 4, 4);

            g.DrawRectangle(selectionPen, (right + left) / 2, down + 2, 4, 4);
            g.DrawRectangle(selectionPen, (right + left) / 2, up - 2, 4, 4);
            g.DrawRectangle(selectionPen, right + 2, (up + down) / 2, 4, 4);
            g.DrawRectangle(selectionPen, left - 2, (up + down) / 2, 4, 4);
        }

        //Called by image providers, updates this background image
        public void UpdateImage(Image img)
        {
            this.AutoScrollMinSize = img.Size;
            imagePanel.Size = img.Size;
            backBuffer = new Bitmap(img.Width, img.Height);

            imagePanel.BackgroundImage = new Bitmap(img);
            imagePanel.BackgroundImageLayout = ImageLayout.None;

            initialize = true;

            DrawAll();
        }

        private NuGenSegment selectedSegment;
        private List<NuGenPoint> selectedPointList;

        //Looks for a point intersection at the given mouse coordinates
        private void CheckForPointIntersection(int x, int y)
        {
            foreach (NuGenPoint p in doc.PointSets.AllPoints)
            {
                if (p.Intersects(x, y, 3.0))
                {
                    if (selectedPointList.Contains(p))
                        return;

                    SelectPoint(p);
                    return;
                }
            }

            DeselectPoints();
        }

        //selects the given point and makes it available for move/delete/edit operations
        private void SelectPoint(NuGenPoint p)
        {
            selectedPointList.Clear();
            selectedPointList.Add(p);

            selectedPointGestatingList.Clear();
            selectedPointGestatingList.Add(p);

            DrawSelectedPoints(Buffer);
        }

        //Selects all points in the provided region
        private void SelectRegion(Rectangle region)
        {
            foreach (NuGenPoint p in doc.PointSets.AllPoints)
            {
                if(region.Contains(p.XScreen, p.YScreen))
                {
                    selectedPointList.Add(p);
                    selectedPointGestatingList.Add(p);
                }
            }

            DrawSelectedPoints(Buffer);
        }

        //Clears the selected points
        private void DeselectPoints()
        {
            Point[] points = new Point[selectedPointList.Count];

            int i = 0;

            foreach (NuGenPoint p in selectedPointList)
            {
                points[i++] = new Point(p.XScreen, p.YScreen);
            }

            selectedPointList.Clear();
            selectedPointGestatingList.Clear();
        }

        //Checks for a segment at the given mouse coordinates
        private void CheckForSegmentIntersection(int x, int y)
        {
            foreach (NuGenSegment segment in doc.Segments.Segments)
            {
                foreach (SegmentLine line in segment.Lines)
                {
                    if (line.Intersects(x, y, 2.0))
                    {
                        SelectSegment(segment);
                        return;
                    }
                }
            }

            if(selectedSegment != null)
                DeselectSelectedSegment();            
        }

        private void DeselectSelectedSegment()
        {
            //ClearSelectedSegments();
            selectedSegment = null;
        }

        //Redraws a specific portion of the screen
        private void DrawAll(Region region)
        {
            Graphics g = Buffer;
            g.Clip = region;

            if (doc.DigitizeState == DigitizeState.SelectState)
            {
                DrawSelectedGestatingPoints(g);
            }

            if (doc.DigitizeState == DigitizeState.SegmentState)
            {
                DrawSegments(g);
                DrawSelectedSegments(g);
            }

            if (doc.DigitizeState == DigitizeState.PointMatchState)
            {
                DrawMatchBoxes(g);
                DrawMatchHighlights(g);
                DrawMatchPoints(g);
            }

            if (viewPointSelection == ViewPointSelection.ViewAllPoints)
            {
                DrawAxes(g);
                DrawCurvePoints(g);
                DrawMeasurePoints(g);
                DrawScaleBar(g);
            }
            else if (viewPointSelection == ViewPointSelection.ViewAxesPoints)
            {
                DrawAxes(g);
            }
            else if (viewPointSelection == ViewPointSelection.ViewCurvePoints)
            {
                DrawCurvePoints(g);
            }
            else if (viewPointSelection == ViewPointSelection.ViewMeasurePoints)
            {
                DrawMeasurePoints(g);
            }
            else if (viewPointSelection == ViewPointSelection.ViewScalePoints)
            {
                DrawScaleBar(g);
            }

            if (showGridlines)
            {
                DrawGridLines(g);
            }
        }

        private void SelectSegment(NuGenSegment segment)
        {
            if (selectedSegment != null)
                DeselectSelectedSegment();

            selectedSegment = segment;
            DrawSelectedSegments(Buffer);
        }

        public void Clear()
        {
            imagePanel.BackgroundImage = null;            
        }

        public bool ShowGridLines
        {
            get
            {
                return showGridlines;
            }

            set
            {
                showGridlines = value;
            }
        }

        //Prints everything visible to the default printer
        public void Print(Graphics g)
        {
            if (initialize == false)
                return;

            g.DrawImage(imagePanel.BackgroundImage, new Point(0,0));
            g.DrawImage(backBuffer, new Point(0, 0));
        }

        public void CutPoints()
        {
            editPointsList.Clear();
            editPointsList.AddRange(selectedPointList);
            DeletePoints();
            DrawAll();
        }

        public void CopyPoints()
        {
            editPointsList.Clear();

            foreach (NuGenPoint p in selectedPointList)
            {
                editPointsList.Add(new NuGenPoint(p));
            }            
        }

        public void PastePoints()
        {
            foreach (NuGenPoint p in editPointsList)
            {
                if (p.XScreen > 5)
                    p.XScreen -= 5;
                else if (p.XScreen < Width - 5)
                    p.XScreen += 5;
                else
                    p.XScreen = Width/2 + 5;

                doc.AddPoint(p);
            }

            DrawAll();
            Refresh();
        }

        public void PastePointsAsNew()
        {
            NameDialog dlg = new NameDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {                
                doc.PointSets.AddCurve(dlg.PointSetName);
                NuGenPointSet pointSet = doc.PointSets.GetCurve(dlg.PointSetName);

                foreach (NuGenPoint p in editPointsList)
                {
                    p.PointSet = pointSet;

                    if (p.XScreen > 5)
                        p.XScreen -= 5;
                    else if (p.XScreen < Width - 5)
                        p.XScreen += 5;
                    else
                        p.XScreen = Width / 2 + 5;

                    doc.AddPoint(p);
                }
            }

            DrawAll();
            Refresh();
        }

        public bool HandleKey(Keys keyData)
        {
            if (doc.DigitizeState == DigitizeState.SelectState)
            {
                switch (keyData)
                {
                    case Keys.Delete:
                        DeletePoints();
                        DrawAll(); break;
                }
            }

            if ((!(doc.DigitizeState == DigitizeState.PointMatchState)) || doc.MatchSet.Matches.Count == 0)
                return false;

            switch (keyData)
            {
                case Keys.A:
                    RejectPoint();break;
                case Keys.D:
                    AcceptPoint(); break;
            }

            DrawAll();            

            return true;
        }
    }
}
