using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Genetibase.NuGenTransform
{
    #region Enumerations and Settings Declaration
    public enum LineSize
    {
        LineSize1 = 1,
        LineSize2,
        LineSize3,
        LineSize4,
        LineSize5,
        LineSize6,
        LineSize7,
        LineSize8,
        MaxLineSize
    };

    // Connect As method determines how points are connected by lines
    public enum LineConnectAs
    {
        SingleValuedFunction = 1,
        Contour,
        MaxConnectAsMethod
    };

    public enum GridSet
    {
        AllButCount,
        AllButStart,
        AllButStep,
        AllButStop
    };

    public enum ExportPointsSelection
    {
        XFromAllCurves, // y from interpolating all curve
        XFromFirstCurve, // y from interpolating all curves
        XFromGridLines, // y from interpolating all curves
        XYFromAllCurves // no interpolation is performed
    };

    public enum PointShape
    {
        Cross = 0,
        X,
        Diamond,
        Square,
        Triangle,
        MaxPointShape
    };

    public enum PointSize
    {
        PointSize1 = 1,
        PointSize2,
        PointSize3,
        PointSize4,
        PointSize5,
        PointSize6,
        PointSize7,
        PointSize8,
        MaxPointSize
    };

    public enum PointLineSize
    {
        PointLineSize1 = 1,
        PointLineSize2,
        PointLineSize3,
        PointLineSize4,
        PointLineSize5,
        PointLineSize6,
        PointLineSize7,
        PointLineSize8,
        MaxPointLineSize
    };

    // multicurve layout in exported files
    public enum ExportLayout
    {
        AllCurvesOnEachLine,
        OneCurveOnEachLine
    };

    // value separators in exported files
    public enum ExportDelimiters
    {
        Commas,
        Spaces,
        Tabs
    };

    public enum ExportHeader
    {
        HeaderNone,
        HeaderSimple,
        HeaderGnuplot
    };

    public enum DiscretizeMethod
    {
        DiscretizeNone,
        DiscretizeIntensity,
        DiscretizeForeground,
        DiscretizeHue,
        DiscretizeSaturation,
        DiscretizeValue
    };

    public struct PointSetStyle
    {
        // cross, diamond, ...
        public PointShape pointShape;

        // width and height of point shape. this unitless value is mapped into a pixel value that
        // assumes the point line has a width of one pixel
        public PointSize pointSize;

        // width of point line, in pixels
        public PointLineSize pointLineSize;

        // color of point shape
        public Color pointLineColor;

        // color inside point shape
        public Color pointInColor;

        // width of line between points, in pixels
        public LineSize lineSize;

        // line color
        public Color lineColor;

        // connect as method
        public LineConnectAs lineConnectAs;
    };

    // gridline removal is performed by any combination of three approaches
    public struct GridRemovalSettings
    {
        // thinline removal approach
        public bool removeThinLines;
        public double thinThickness;

        // regular gridline removal approach
        public bool removeGridlines;
        public GridMeshSettings gridMesh;
        public double gridDistance;

        // color gridline removal approach
        public bool removeColor;
        public Color color;
        public int foregroundThresholdLow;
        public int foregroundThresholdHigh;

        // after gridlines are removed, gaps smaller than this value are connected
        public double gapSeparation;
    };

    public enum GridType
    {
        GridTypeRemoval,
        GridTypeDisplay
    };

    public struct SegmentSettings
    {
        // minimum length of segment in points. segments with fewer points are not displayed.
        // smaller value usually causes many more segments to be drawn, which can lengthen
        // drawing time and collision finding (hidden canvas items are never considered during
        // collision finding). memory usage is not affected since segments of all sizes are
        // initially created, so we never have to go back and rescan when minPoints and
        // pointSeparation are modified
        public int minPoints;

        // when a segment is digitized, the curve points will be separated by this value in pixels
        // although if fillCorners is on then sometimes the separations will be less
        public int pointSeparation;

        // true to have a point at every corner. this option is useful for piecewise-linear graphs
        // because all important information is captured, but smoothly varying graphs can
        // become too cluttered with no benefit gained
        public bool fillCorners;

        // width of line between points, in pixels
        public LineSize lineSize;

        // line color
        public Color lineColor;
    };

    public struct PointMatchSettings
    {
        // each new point cannot be closer to an existing point than this distance. this
        // prevents successive point matchings from creating duplicates
        public int pointSeparation;

        // each point is limited in extent to this maximum width and height, in pixels, since
        // otherwise time is wasted (by n-squared searches) looking at huge areas that could
        // not possibly be acceptable sample points
        public int pointSize;

        // each matched point is either accepted or rejected
        public Color acceptedColor;
        public Color rejectedColor;
    };
    #endregion

    [Serializable()]
    public class NuGenDocument : NuGenImageProvider, ISerializable
    {

        #region Member Variables

        // pointsets owned by this document. each pointset has any number of points
        NuGenPointSetCollection pointSets;

        // segments that have been scanned in from the original image
        NuGenSegmentCollection segments;

        // match points produced and used in point match mode. this is cleared when leaving point match mode
        NuGenMatchSet matchSet;

        // gridlines for display. gridlines for removal are implicit in m_processedPixmap
        List<GridlineScreen> gridDisplay;

        string title;
        string savePath; // this is set to import path with appropriate file extension as default during import
        string exportPath;

        // the first save after import is confirmed so user can select a new directory and edit the filename
        bool saveFileExists;

        // the first export after import is confirmed so user can select a new directory and edit the filename
        bool exportFileExists;

        // original scanned image is processed by gridline removal code, and available for m_canvas display
        // (in either original or processed form).
        //
        // m_originalImage and m_processedImage are Image versions of m_originalPixmap and
        // m_processedPixmap, and should not be considered a waste of memory since
        // Images are created anyway during the processing that creates the pixmaps
        BackgroundSelection backgroundSelection;
        
        
        //The original image, unaltered
        Image originalImage;

        //The processed image, discretized and or grid removal settings applied
        Image processedImage;

        private NuGenImageProcessor imageProcessor;

        // digitize state is determined by the digitize toolbar
        DigitizeState digitizeState;

        // for the active document, these mirror the contents of the curve and measure combobox
        string currentCurveName;
        string currentMeasureName;

        // coordinates settings
        CoordSettings coordSettings;

        // export settings determine the format of exported files
        ExportSettings exportSettings;

        // gridMeshSettings removal line settings
        GridRemovalSettings gridRemovalSettings;

        // gridMeshSettings display line settings
        GridMeshSettings gridDisplaySettings;

        // segment settings
        SegmentSettings segmentSettings;

        // point match settings
        PointMatchSettings pointMatchSettings;

        // transform owned by this document
        NuGenScreenTranslate transform;

        // discretize settings
        DiscretizeSettings discretizeSettings;

        private Color bgColor;

        // true if axes or scale transformation is out of date. without
        // these flags, the transformations would be recomputed at least
        // once every event (including focusIn). since any popup warning
        // made by the transformation code causes a focusIn event while
        // returning to the main window, an infinite loop would otherwise
        // occur
        bool dirtyAxesTransformation;
        bool dirtyScaleTransformation;

        private LengthUnits units;

        // candidate sample point for point match, represented as a set of pixels in original image
        List<Point> samplePointPixels = new List<Point>();

        #endregion

        //Constructs the document and places it in the provided state
        public NuGenDocument(DigitizeState state)
        {
            listeners = new List<NuGenImageListener>();

            pointSets = new NuGenPointSetCollection();
            segments = new NuGenSegmentCollection();
            transform = new NuGenScreenTranslate(this);
            gridDisplay = new List<GridlineScreen>();
            digitizeState = state;
            matchSet = new NuGenMatchSet(pointMatchSettings);

            //load all of the settings
            NuGenDefaultSettings rSettings = NuGenDefaultSettings.GetInstance();

            coordSettings = rSettings.CoordSettings;
            exportSettings = rSettings.ExportSettings;
            segmentSettings = rSettings.SegmentSettings;
            pointMatchSettings = rSettings.PointMatchSettings;

            gridRemovalSettings = rSettings.GridRemovalSettings;
            gridDisplaySettings.initialized = false;
            gridDisplaySettings.gridSetX = rSettings.GridDisplayGridSetX;
            gridDisplaySettings.gridSetY = rSettings.GridDisplayGridSetY;

            discretizeSettings = rSettings.DiscretizeSettings;
            backgroundSelection = rSettings.BackgroundSelection;
        }

        //Deserialization constructor.
        public NuGenDocument(SerializationInfo info, StreamingContext ctxt)
        {
            backgroundSelection = (BackgroundSelection)info.GetValue("backgroundSelection", typeof(BackgroundSelection));
            originalImage = (Image)info.GetValue("originalImage", typeof(Image));

            title = (string)info.GetValue("title", typeof(string));

            digitizeState = (DigitizeState)info.GetValue("digitizeState", typeof(DigitizeState));

            currentCurveName = (string)info.GetValue("curveCmbText", typeof(string));
            currentMeasureName = (string)info.GetValue("measureCmbText", typeof(string));

            coordSettings.frame = (ReferenceFrame)info.GetValue("coordSettings.frame", typeof(ReferenceFrame));
            coordSettings.thetaUnits = (ThetaUnits)info.GetValue("coordSettings.thetaUnits", typeof(ThetaUnits));
            coordSettings.xThetaScale = (Scale)info.GetValue("coordSettings.xThetaScale", typeof(Scale));
            coordSettings.yRScale = (Scale)info.GetValue("coordSettings.yRScale", typeof(Scale));

            exportSettings.delimiters = (ExportDelimiters)info.GetValue("exportSettings.delimiters", typeof(ExportDelimiters));
            exportSettings.layout = (ExportLayout)info.GetValue("exportSettings.layout", typeof(ExportLayout));
            exportSettings.pointsSelection = (ExportPointsSelection)info.GetValue("exportSettings.pointsSelection", typeof(ExportPointsSelection));
            exportSettings.header = (ExportHeader)info.GetValue("exportSettings.header", typeof(ExportHeader));

            gridRemovalSettings.removeThinLines = (bool)info.GetValue("gridRemovalSettings.removeThinLines", typeof(bool));
            gridRemovalSettings.thinThickness = (double)info.GetValue("gridRemovalSettings.thinThickness", typeof(double));
            gridRemovalSettings.removeGridlines = (bool)info.GetValue("gridRemovalSettings.removeGridlines", typeof(bool));
            gridRemovalSettings.gridMesh.initialized = (bool)info.GetValue("gridRemovalSettings.gridMesh.initialized", typeof(bool));
            gridRemovalSettings.gridMesh.countX = (int)info.GetValue("gridRemovalSettings.gridMesh.countX", typeof(int));
            gridRemovalSettings.gridMesh.countY = (int)info.GetValue("gridRemovalSettings.gridMesh.countY", typeof(int));
            gridRemovalSettings.gridMesh.gridSetX = (GridSet)info.GetValue("gridRemovalSettings.gridMesh.gridSetX", typeof(GridSet));
            gridRemovalSettings.gridMesh.gridSetY = (GridSet)info.GetValue("gridRemovalSettings.gridMesh.gridSetY", typeof(GridSet));
            gridRemovalSettings.gridMesh.startX = (double)info.GetValue("gridRemovalSettings.gridMesh.startX", typeof(double));
            gridRemovalSettings.gridMesh.startY = (double)info.GetValue("gridRemovalSettings.gridMesh.startY", typeof(double));
            gridRemovalSettings.gridMesh.stepX = (double)info.GetValue("gridRemovalSettings.gridMesh.stepX", typeof(double));
            gridRemovalSettings.gridMesh.stepY = (double)info.GetValue("gridRemovalSettings.gridMesh.stepY", typeof(double));
            gridRemovalSettings.gridMesh.stopX = (double)info.GetValue("gridRemovalSettings.gridMesh.stopX", typeof(double));
            gridRemovalSettings.gridMesh.stopY = (double)info.GetValue("gridRemovalSettings.gridMesh.stopY", typeof(double));
            gridRemovalSettings.gridDistance = (double)info.GetValue("gridRemovalSettings.gridDistance", typeof(double));
            gridRemovalSettings.removeColor = (bool)info.GetValue("gridRemovalSettings.removeColor", typeof(bool));
            gridRemovalSettings.color = (Color)info.GetValue("gridRemovalSettings.color", typeof(Color));
            gridRemovalSettings.foregroundThresholdLow = (int)info.GetValue("gridRemovalSettings.foregroundThresholdLow", typeof(int));
            gridRemovalSettings.foregroundThresholdHigh = (int)info.GetValue("gridRemovalSettings.foregroundThresholdHigh", typeof(int));
            gridRemovalSettings.gapSeparation = (double)info.GetValue("gridRemovalSettings.gapSeparation", typeof(double));

            gridDisplaySettings.initialized = (bool)info.GetValue("gridDisplaySettings.initialized", typeof(bool));
            gridDisplaySettings.countX = (int)info.GetValue("gridDisplaySettings.countX", typeof(int));
            gridDisplaySettings.countY = (int)info.GetValue("gridDisplaySettings.countY", typeof(int));
            gridDisplaySettings.gridSetX = (GridSet)info.GetValue("gridDisplaySettings.gridSetX", typeof(GridSet));
            gridDisplaySettings.gridSetY = (GridSet)info.GetValue("gridDisplaySettings.gridSetY", typeof(GridSet));
            gridDisplaySettings.startX = (double)info.GetValue("gridDisplaySettings.startX", typeof(double));
            gridDisplaySettings.startY = (double)info.GetValue("gridDisplaySettings.startY", typeof(double));
            gridDisplaySettings.stepX = (double)info.GetValue("gridDisplaySettings.stepX", typeof(double));
            gridDisplaySettings.stepY = (double)info.GetValue("gridDisplaySettings.stepY", typeof(double));
            gridDisplaySettings.stopX = (double)info.GetValue("gridDisplaySettings.stopX", typeof(double));
            gridDisplaySettings.stopY = (double)info.GetValue("gridDisplaySettings.stopY", typeof(double));

            segmentSettings.minPoints = (int)info.GetValue("segmentSettings.minPoints", typeof(int));
            segmentSettings.pointSeparation = (int)info.GetValue("segmentSettings.pointSeparation", typeof(int));
            segmentSettings.lineSize = (LineSize)info.GetValue("segmentSettings.lineSize", typeof(LineSize));
            segmentSettings.lineColor = (Color)info.GetValue("segmentSettings.lineColor", typeof(Color));

            pointMatchSettings.pointSeparation = (int)info.GetValue("pointMatchSettings.pointSeparation", typeof(int));
            pointMatchSettings.pointSize = (int)info.GetValue("pointMatchSettings.pointSize", typeof(int));
            pointMatchSettings.acceptedColor = (Color)info.GetValue("pointMatchSettings.acceptedColor", typeof(Color));
            pointMatchSettings.rejectedColor = (Color)info.GetValue("pointMatchSettings.rejectedColor", typeof(Color));

            discretizeSettings.discretizeMethod = (DiscretizeMethod)info.GetValue("discretizeSettings.discretizeMethod", typeof(DiscretizeMethod));
            discretizeSettings.intensityThresholdLow = (int)info.GetValue("discretizeSettings.intensityThresholdLow", typeof(int));
            discretizeSettings.intensityThresholdHigh = (int)info.GetValue("discretizeSettings.intensityThresholdHigh", typeof(int));
            discretizeSettings.foregroundThresholdLow = (int)info.GetValue("discretizeSettings.foregroundThresholdLow", typeof(int));
            discretizeSettings.foregroundThresholdHigh = (int)info.GetValue("discretizeSettings.foregroundThresholdHigh", typeof(int));
            discretizeSettings.hueThresholdLow = (int)info.GetValue("discretizeSettings.hueThresholdLow", typeof(int));
            discretizeSettings.hueThresholdHigh = (int)info.GetValue("discretizeSettings.hueThresholdHigh", typeof(int));
            discretizeSettings.saturationThresholdLow = (int)info.GetValue("discretizeSettings.saturationThresholdLow", typeof(int));
            discretizeSettings.saturationThresholdHigh = (int)info.GetValue("discretizeSettings.saturationThresholdHigh", typeof(int));
            discretizeSettings.valueThresholdLow = (int)info.GetValue("discretizeSettings.valueThresholdLow", typeof(int));
            discretizeSettings.valueThresholdHigh = (int)info.GetValue("discretizeSettings.valueThresholdHigh", typeof(int));

            pointSets = new NuGenPointSetCollection();

            PointSets.SerializeRead(info);

            saveFileExists = true;            
            dirtyAxesTransformation = true;
            dirtyScaleTransformation = true;

            listeners = new List<NuGenImageListener>();

            segments = new NuGenSegmentCollection();
            transform = new NuGenScreenTranslate(this);
            gridDisplay = new List<GridlineScreen>();
            matchSet = new NuGenMatchSet(pointMatchSettings);

            ProcessOriginialImage();
        }
                
        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("backgroundSelection", this.backgroundSelection);
            info.AddValue("originalImage", this.originalImage);

            info.AddValue("title", this.title);

            info.AddValue("digitizeState", this.digitizeState);

            info.AddValue("curveCmbText", this.currentCurveName);
            info.AddValue("measureCmbText", this.currentMeasureName);

            info.AddValue("coordSettings.frame", this.coordSettings.frame);
            info.AddValue("coordSettings.thetaUnits", this.coordSettings.thetaUnits);
            info.AddValue("coordSettings.xThetaScale", this.coordSettings.xThetaScale);
            info.AddValue("coordSettings.yRScale", this.coordSettings.yRScale);

            info.AddValue("exportSettings.delimiters", this.exportSettings.delimiters);
            info.AddValue("exportSettings.layout", this.exportSettings.layout);
            info.AddValue("exportSettings.pointsSelection", this.exportSettings.pointsSelection);
            info.AddValue("exportSettings.header", this.exportSettings.header);

            info.AddValue("gridRemovalSettings.removeThinLines", this.gridRemovalSettings.removeThinLines);
            info.AddValue("gridRemovalSettings.thinThickness", this.gridRemovalSettings.thinThickness);
            info.AddValue("gridRemovalSettings.removeGridlines", this.gridRemovalSettings.removeGridlines);
            info.AddValue("gridRemovalSettings.gridMesh.initialized", this.gridRemovalSettings.gridMesh.initialized);
            info.AddValue("gridRemovalSettings.gridMesh.countX", this.gridRemovalSettings.gridMesh.countX);
            info.AddValue("gridRemovalSettings.gridMesh.countY", this.gridRemovalSettings.gridMesh.countY);
            info.AddValue("gridRemovalSettings.gridMesh.gridSetX", this.gridRemovalSettings.gridMesh.gridSetX);
            info.AddValue("gridRemovalSettings.gridMesh.gridSetY", this.gridRemovalSettings.gridMesh.gridSetY);
            info.AddValue("gridRemovalSettings.gridMesh.startX", this.gridRemovalSettings.gridMesh.startX);
            info.AddValue("gridRemovalSettings.gridMesh.startY", this.gridRemovalSettings.gridMesh.startY);
            info.AddValue("gridRemovalSettings.gridMesh.stepX", this.gridRemovalSettings.gridMesh.stepX);
            info.AddValue("gridRemovalSettings.gridMesh.stepY", this.gridRemovalSettings.gridMesh.stepY);
            info.AddValue("gridRemovalSettings.gridMesh.stopX", this.gridRemovalSettings.gridMesh.stopX);
            info.AddValue("gridRemovalSettings.gridMesh.stopY", this.gridRemovalSettings.gridMesh.stopY);
            info.AddValue("gridRemovalSettings.gridDistance", this.gridRemovalSettings.gridDistance);
            info.AddValue("gridRemovalSettings.removeColor", this.gridRemovalSettings.removeColor);
            info.AddValue("gridRemovalSettings.color", this.gridRemovalSettings.color);
            info.AddValue("gridRemovalSettings.foregroundThresholdLow", this.gridRemovalSettings.foregroundThresholdLow);
            info.AddValue("gridRemovalSettings.foregroundThresholdHigh", this.gridRemovalSettings.foregroundThresholdHigh);
            info.AddValue("gridRemovalSettings.gapSeparation", this.gridRemovalSettings.gapSeparation);

            info.AddValue("gridDisplaySettings.initialized", this.gridDisplaySettings.initialized);
            info.AddValue("gridDisplaySettings.countX", this.gridDisplaySettings.countX);
            info.AddValue("gridDisplaySettings.countY", this.gridDisplaySettings.countY);
            info.AddValue("gridDisplaySettings.gridSetX", this.gridDisplaySettings.gridSetX);
            info.AddValue("gridDisplaySettings.gridSetY", this.gridDisplaySettings.gridSetY);
            info.AddValue("gridDisplaySettings.startX", this.gridDisplaySettings.startX);
            info.AddValue("gridDisplaySettings.startY", this.gridDisplaySettings.startY);
            info.AddValue("gridDisplaySettings.stepX", this.gridDisplaySettings.stepX);
            info.AddValue("gridDisplaySettings.stepY", this.gridDisplaySettings.stepY);
            info.AddValue("gridDisplaySettings.stopX", this.gridDisplaySettings.stopX);
            info.AddValue("gridDisplaySettings.stopY", this.gridDisplaySettings.stopY);

            info.AddValue("segmentSettings.minPoints", this.segmentSettings.minPoints);
            info.AddValue("segmentSettings.pointSeparation", this.segmentSettings.pointSeparation);
            info.AddValue("segmentSettings.lineSize", this.segmentSettings.lineSize);
            info.AddValue("segmentSettings.lineColor", this.segmentSettings.lineColor);

            info.AddValue("pointMatchSettings.pointSeparation", this.pointMatchSettings.pointSeparation);
            info.AddValue("pointMatchSettings.pointSize", this.pointMatchSettings.pointSize);
            info.AddValue("pointMatchSettings.acceptedColor", this.pointMatchSettings.acceptedColor);
            info.AddValue("pointMatchSettings.rejectedColor", this.pointMatchSettings.rejectedColor);
              
            info.AddValue("discretizeSettings.discretizeMethod", this.discretizeSettings.discretizeMethod);
            info.AddValue("discretizeSettings.intensityThresholdLow", this.discretizeSettings.intensityThresholdLow);
            info.AddValue("discretizeSettings.intensityThresholdHigh", this.discretizeSettings.intensityThresholdHigh);
            info.AddValue("discretizeSettings.foregroundThresholdLow", this.discretizeSettings.foregroundThresholdLow);
            info.AddValue("discretizeSettings.foregroundThresholdHigh", this.discretizeSettings.foregroundThresholdHigh);
            info.AddValue("discretizeSettings.hueThresholdLow", this.discretizeSettings.hueThresholdLow);
            info.AddValue("discretizeSettings.hueThresholdHigh", this.discretizeSettings.hueThresholdHigh);
            info.AddValue("discretizeSettings.saturationThresholdLow", this.discretizeSettings.saturationThresholdLow);
            info.AddValue("discretizeSettings.saturationThresholdHigh", this.discretizeSettings.saturationThresholdHigh);
            info.AddValue("discretizeSettings.valueThresholdLow", this.discretizeSettings.valueThresholdLow);
            info.AddValue("discretizeSettings.valueThresholdHigh", this.discretizeSettings.valueThresholdHigh);            

            PointSets.SerializeWrite(info);
        }

        #region Image Listener Stuff

        List<NuGenImageListener> listeners;

        //Adds a listener to this objects image
        public void RegisterListener(NuGenImageListener listener)
        {
            listeners.Add(listener);
        }

        //Updates all listeners with the provided image
        public void UpdateListenersImage(Image img)
        {
            if (img == null)
            {
                foreach (NuGenImageListener listener in listeners)
                {
                    listener.Clear();
                }
            }
            else
            {
                foreach (NuGenImageListener listener in listeners)
                {
                    listener.UpdateImage(img);
                }
            }
        }

        #endregion

        //Updates all listeners based on the background selection
        public void UpdateListeners()
        {
            switch (backgroundSelection)
            {
                case BackgroundSelection.OriginalImage: UpdateListenersImage(originalImage); return;
                case BackgroundSelection.BlankBackground: UpdateListenersImage(null); return;
                case BackgroundSelection.ProcessedImage: UpdateListenersImage(processedImage); return;
            }
        }

        // imported documents need to be setup with defaults
        public void InitDefaults() {
            pointSets.AddCurve(NuGenPointSetCollection.DefaultCurveName);
            pointSets.AddMeasure(NuGenPointSetCollection.DefaultMeasureName);

            UpdateListeners();
        }

        // initializes a new document by importing an image file
        public bool ImportFile(string filename) 
        {

            originalImage = Image.FromFile(filename);

            ProcessOriginialImage();

            NuGenDiscretize discretize = new NuGenDiscretize(originalImage, discretizeSettings);
            gridRemovalSettings.color = discretize.GetBackgroundColor();            

            saveFileExists = false;
            SavePath = filename;

            UpdateListenersImage(originalImage);

            return true;
        }

        // exports the document
        public void ExportDocument(string filename)
        {
            FileStream f = File.Open(filename, FileMode.Create);

            pointSets.ExportToFile(f, coordSettings, gridDisplaySettings, exportSettings);

            ExportPath = filename;
        }

        public string SavePath
        {
            get
            {
                return savePath;
            }

            set
            {
                if (File.Exists(value))
                {
                    savePath = value;
                }
                else
                {
                    throw new FileNotFoundException("Couldn't load file");
                }
            }
        }

        public string ExportPath
        {
            get
            {
                return exportPath;
            }
            set
            {
                exportPath = value;
                exportFileExists = true;
            }
        }

        public NuGenSegmentCollection Segments
        {
            get
            {
                return segments;
            }

            set
            {
                segments = value;
            }
        }

        public bool ExportFileExists
        {
            get
            {
                return this.exportFileExists;
            }
        }

        public bool SaveFileExists
        {
            get
            {
                return this.saveFileExists;
            }
        }

        // returns pointsets container belonging to this document
        public NuGenPointSetCollection PointSets
        {
            get
            {
                return pointSets;
            }
        }

        public int AxisPointCount
        {
            get
            {
                return PointSets.Axes.Points.Count;
            }
        }

        public int ScalePointCount
        {
            get
            {
                return PointSets.ScaleBar.Points.Count;
            }
        }

        public bool ValidAxes
        {
            get
            {
                return transform.ValidAxes;
            }
        }

        public bool ValidScale
        {
            get
            {
                return transform.ValidScale;
            }
        }

        // convert screen into graph coordinates, using screen/graph, cartesian/polar, linear/log transformations
        public void ScreenToXThetaYR(int xScreen, int yScreen, out double xTheta, out double yR)
        {
            transform.ScreenToXThetaYR(coordSettings, xScreen, yScreen, out xTheta, out yR);
        }

        public DigitizeState DigitizeState
        {
            get
            {
                return digitizeState;
            }

            set
            {
                digitizeState = value;

                UpdateListeners();
            }
        }

        public NuGenPoint AddPoint(NuGenPoint p)
        {
            return AddPoint(p.XScreen, p.YScreen, p.PointSet);
        }

        public NuGenPoint AddPoint(int xScreen, int yScreen)
        {
            return AddPoint(xScreen, yScreen, null);
        }
            
        // add axis or curve point, depending on state. axis point graph coordinates are set later by
        // setAxisPoint so user can see where the new axis point lies while editing the graph coordinatees
        public NuGenPoint AddPoint(int xScreen, int yScreen, NuGenPointSet destination)
        {
            double xThetaGraph = 0.0, yRGraph = 0.0;

            NuGenPoint p;

            if ((digitizeState == DigitizeState.CurveState) ||
              (digitizeState == DigitizeState.MeasureState))
            {
                ScreenToXThetaYR(xScreen, yScreen, out xThetaGraph, out yRGraph);

                p = new NuGenPoint(xScreen, yScreen, xThetaGraph, yRGraph);
            }
            else
            {
                p = new NuGenPoint(xScreen, yScreen);
            }

            switch (digitizeState)
            {
                case DigitizeState.AxisState:
                    dirtyAxesTransformation = true;
                    pointSets.AddPointAxes(p);
                    break;
                case DigitizeState.CurveState:
                case DigitizeState.SegmentState:
                    pointSets.AddPointCurve(p, currentCurveName);
                    break;
                case DigitizeState.MeasureState:
                    pointSets.AddPointMeasure(p, currentMeasureName);
                    break;
                case DigitizeState.ScaleState:
                    dirtyScaleTransformation = true;
                    pointSets.AddPointScale(p);
                    break;
                case DigitizeState.SelectState: //Could be any type of point, so decide
                    if (destination != null)
                    {
                        destination.AddPoint(p);
                    }
                    break;
            }

            PointSets.UpdateGraphCoordinates(coordSettings, transform);

            return p;
        }

        public void UpdateGraphCoordinates()
        {
            PointSets.UpdateGraphCoordinates(coordSettings, transform);
        }

        // change graph coordinates of an axis or scale bar point
        public bool SetAxisPoint(NuGenPoint p, double xThetaGraph, double yRGraph)
        {
            dirtyAxesTransformation = true;

            p.XThetaGraph = xThetaGraph;
            p.YRGraph = yRGraph;

            int result = ComputeTransformation();

            switch (result)
            {
                case NuGenMath.SUCCESS:
                    return true;
                case NuGenMath.NO_SPREAD:
                    MessageBox.Show("Invalid axis point definition for a log scale, redefine points"); break;
                case NuGenMath.NONPOSITIVE_COORDINATE:
                    MessageBox.Show("Invalid axis point definition for a log scale, redefine points"); break;
                case NuGenMath.BAD_GRAPH_COORDINATES:
                    MessageBox.Show("Axis points can not be colinear or colocated"); break;
                case NuGenMath.BAD_SCREEN_COORDINATES:
                    MessageBox.Show("Axis points can not be colinear or colocated"); break;
            }

            return false;
        }

        //Sets a gestating point to a full scale point so that it takes place in 
        // scale computations
        public bool SetScalePoint(NuGenPoint p, double x, double y)
        {
            dirtyScaleTransformation = true;

            p.XThetaGraph = x;
            p.YRGraph = y;

            int result = ComputeTransformation();

            switch (result)
            {
                case NuGenMath.SUCCESS:
                    return true;
                case NuGenMath.NO_SPREAD:
                    MessageBox.Show("Invalid scale point definition for a log scale, redefine points"); break;
                case NuGenMath.NONPOSITIVE_COORDINATE:
                    MessageBox.Show("Invalid scale point definition for a log scale, redefine points"); break;
                case NuGenMath.BAD_GRAPH_COORDINATES:
                    MessageBox.Show("Scale points can not be colinear or colocated"); break;
                case NuGenMath.BAD_SCREEN_COORDINATES:
                    MessageBox.Show("Scale points can not be colinear or colocated"); break;
            }

            return false;
        }

        //Highlights a point in point match mode that is a candidate for the start of 
        // a point match sequence
        public void HighlightCandidateMatchPoint(Point p)
        {
            Bitmap b = new Bitmap(processedImage);
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, b.PixelFormat);

            if (NuGenDiscretize.ProcessedPixelIsOn(bmData, p.X, p.Y))
            {
                // pixel is on
                bool found = (samplePointPixels.Count > 0);
                if (found)
                {
                    foreach (Point sample in samplePointPixels)
                    {
                        if (sample.X == p.X && sample.Y == p.Y)
                        {
                            found = true;
                            break;
                        }

                        found = false;
                    }
                }

                if (!found)
                {
                    NuGenPointMatch.IsolateSampleMatchPoint(samplePointPixels,
                      bmData, pointMatchSettings,
                      p.X, p.Y, p.X, p.Y);
                }
            }
            else
            {
                samplePointPixels.Clear();
            }

            b.UnlockBits(bmData);
        }

        public List<Point> SamplePointPixels
        {
            get
            {
                return samplePointPixels;
            }
        }

        public int PointMatchHighlightLineSize
        {
            get
            {
                return NuGenDefaultSettings.GetInstance().PointMatchHighlightLineSize;
            }
        }

        public int PointMatchHighlightDiameter
        {
            get
            {
                return NuGenDefaultSettings.GetInstance().PointMatchHighlightDiameter;
            }
        }

        public NuGenMatchSet MatchSet
        {
            get
            {
                return matchSet;
            }
        }

        //Matches a point on the screen with the sample match pixels
        public bool MatchSamplePoint(Point p)
        {
            bool found = false;

            foreach(Point sample in samplePointPixels)
            {
                if(sample.X == p.X && sample.Y == p.Y)
                    found = true;
            }

            if(!found)
                return false;

            List<Point> pointsExisting = PointSets.CurveCoordinates(currentCurveName);        

          List<PointMatchTriplet> pointsCreated = new List<PointMatchTriplet>();
          NuGenPointMatch.MatchSamplePoint(processedImage, pointMatchSettings,
            samplePointPixels, pointsExisting, pointsCreated);

          if (pointsCreated.Count < 1)
          {
              MessageBox.Show("No points successfully matched the sample point!\nSkipping the matching of sample points using arrow keys", "Point Match Error!");
          }
          else
          {
              matchSet.AddCreatedPoints(pointsCreated, p);
          }
          
          return true;
        }        

        //Removes a point from the document based on the documents current digitize state
        public void RemovePoint(NuGenPoint p)
        {
            switch (DigitizeState)
            {
                case DigitizeState.AxisState:
                    pointSets.Axes.RemovePoint(p); break;
                case DigitizeState.CurveState:
                    foreach (NuGenPointSet pointSet in pointSets.Curves)
                    {
                        pointSet.RemovePoint(p);
                    } break;
                case DigitizeState.MeasureState:
                    foreach (NuGenPointSet pointSet in pointSets.Measures)
                    {
                        pointSet.RemovePoint(p);
                    } break;
                case DigitizeState.ScaleState:
                    pointSets.ScaleBar.RemovePoint(p); break;
                case DigitizeState.SelectState: //find and remove the point since it is generic
                    {
                        foreach (NuGenPointSet pointSet in pointSets.Curves)
                        {
                            pointSet.RemovePoint(p);
                        }

                        foreach (NuGenPointSet pointSet in pointSets.Measures)
                        {
                            pointSet.RemovePoint(p);
                        }
                        pointSets.Axes.RemovePoint(p);
                        pointSets.ScaleBar.RemovePoint(p);

                        gridDisplaySettings.initialized = false;
                        gridRemovalSettings.gridMesh.initialized = false;

                        ComputeTransformation();
                    } break;
            }
        }

        public CoordSettings CoordSettings
        {
            get
            {
                return coordSettings;
            }

            set
            {
                coordSettings = value;
                NuGenDefaultSettings.GetInstance().CoordSettings = value;                
                transform.ComputeAxesTransformation(coordSettings, pointSets.Axes);
                transform.ComputeScaleTransformation(coordSettings, pointSets.ScaleBar);
            }
        }

        public PointSetStyle AxesStyle
        {
            get
            {
                return pointSets.AxesStyle;
            }

            set
            {
                pointSets.AxesStyle = value;
            }
        }

        public PointSetStyle ScaleStyle
        {
            get
            {
                return PointSets.ScaleBar.Style;
            }

            set
            {
                PointSets.ScaleBar.Style = value;
            }
        }

        // get and set methods for export settings
        public ExportSettings ExportSettings
        {
            get
            {
                return exportSettings;
            }

            set
            {
                exportSettings = value;
            }
        }

        public GridRemovalSettings GridRemovalSettings
        {
            get
            {
                return gridRemovalSettings;
            }

            set
            {
                gridRemovalSettings = value;
            }
        }

        public DiscretizeSettings DiscretizeSettings
        {
            get
            {
                return discretizeSettings;
            }

            set
            {
                discretizeSettings = value;
            }
        }

        public GridMeshSettings GridDisplaySettings
        {
            get
            {
                return gridDisplaySettings;
            }

            set
            {
                gridDisplaySettings = value;
            }
        }

        public SegmentSettings SegmentSettings
        {
            get
            {
                return segmentSettings;
            }

            set
            {
                segmentSettings = value;
            }
        }

        public PointMatchSettings PointMatchSettings
        {
            get
            {
                return pointMatchSettings;
            }

            set
            {
                pointMatchSettings = value;
            }
        }

        public NuGenScreenTranslate Transform
        {
            get
            {
                return transform;
            }
        }

        public BackgroundSelection BackgroundSelection
        {
            get
            {
                return backgroundSelection;
            }

            set
            {
                backgroundSelection = value;
                UpdateListeners();
            }
        }

        // geometry info for the active curve or measure pointset
        public void GeometryInfoCurve(List<NuGenGeometryWindowItem> rInfo)
        {
            ComputeTransformation();
            pointSets.GeometryInfoCurve(ValidAxes || ValidScale, true, currentCurveName, rInfo, units);
        }

        public void GeometryInfoMeasure(List<NuGenGeometryWindowItem> rInfo)
        {
            ComputeTransformation();
            pointSets.GeometryInfoMeasure(ValidAxes || ValidScale, true, currentMeasureName, rInfo, units);
        }

        public Image OriginalImage
        {
            get
            {
                return this.originalImage;
            }
        }

        public Image ProcessedImage
        {
            get
            {
                return this.processedImage;
            }

            set
            {
                this.processedImage = value;
                UpdateListeners();
            }
        }
        
        // remove gridlines from original image and discretize, then break processed image into segments
        public void ProcessOriginialImage()
        {
            if (originalImage == null)
            {
                throw new InvalidOperationException("There is no original image to process");
            }

            imageProcessor = new NuGenImageProcessor(this);

            imageProcessor.Process();
            processedImage = imageProcessor.ProcessedImage;

            bgColor = imageProcessor.BackgroundColor;

            UpdateListeners();
        }

        public Color BackgroundColor
        {
            get
            {
                if (bgColor != null)
                    return bgColor;
            }
        }

        // compute pleasing gridMeshSettings display line settings, returning true if successful
        public void InitGridMesh(ref GridMeshSettings gridSettings)
        {
            gridSettings.initialized = false;

            if (!transform.ValidAxes)
                return;

            gridSettings.gridSetX = GridSet.AllButStep;
            gridSettings.gridSetY = GridSet.AllButStep;

            double xThetaMin = 0.0, xThetaMax = 0.0, yRMin = 0.0, yRMax = 0.0;

            pointSets.PointSetGraphLimits(coordSettings, transform,
              ref xThetaMin, ref xThetaMax, ref yRMin, ref yRMax);

            NuGenMath.AxisScale(xThetaMin, xThetaMax, (coordSettings.xThetaScale == Scale.Linear),
                out gridSettings.startX,
                out gridSettings.stopX,
                out gridSettings.stepX,
                out gridSettings.countX);
            NuGenMath.AxisScale(yRMin, yRMax, (coordSettings.yRScale == Scale.Linear),
                out gridSettings.startY,
                out gridSettings.stopY,
                out gridSettings.stepY,
                out gridSettings.countY);

            gridSettings.initialized = true;
        }

        public void InitGridRemovalMesh()
        {
            InitGridMesh(ref gridRemovalSettings.gridMesh);
        }

        public void InitGridDisplayMesh()
        {
            InitGridMesh(ref gridDisplaySettings);
        }

        public string ActiveCurveName
        {
            get
            {
                return currentCurveName;
            }

            set
            {
                currentCurveName = value;
            }
        }

        public string ActiveMeasureName
        {
            get
            {
                return currentMeasureName;
            }

            set
            {
                currentMeasureName = value;
            }
        }
        
        // update the axes and scale bar transformations, and use whichever works. if there is incomplete
        // (insufficient points) or inconsistent information (collocated or collinear points), this exits
        // gracefully. if successful, all curve and measure point coordinates are updated and the gridMeshSettings lines are moved
        public int ComputeTransformation()
        {
            NuGenPointSet pointSet = pointSets.Axes;

            int result = NuGenMath.SUCCESS;

            if (dirtyAxesTransformation)
            {
                dirtyAxesTransformation = false;
                result = transform.ComputeAxesTransformation(coordSettings, pointSet);
            }

            bool success = transform.ValidAxes;
            if (!success)
            {
                // axes transformation failed so try scale bar transformation
                pointSet = pointSets.ScaleBar;

                if (dirtyScaleTransformation)
                {
                    dirtyScaleTransformation = false;
                    result = transform.ComputeScaleTransformation(coordSettings, pointSet);
                }
                success = transform.ValidScale;
            }

            if (success)
            {
                if (!gridRemovalSettings.gridMesh.initialized)
                    InitGridRemovalMesh();

                if (!gridDisplaySettings.initialized)
                    InitGridDisplayMesh();
            }

            // graph coordinates are updated even if transform is invalid, since graph coordinates
            // are just set to screen coordinates
            pointSets.UpdateGraphCoordinates(CoordSettings, transform);

            return result;
        }

        public List<GridlineScreen> MakeGridLines()
        {
            return NuGenGridMesh.MakeGridLines(transform, coordSettings, gridDisplaySettings);
        }

        //Saves and adds all the points that are in the matchset from point matching
        public void SaveMatchedPoints()
        {
            DigitizeState old = DigitizeState;
            DigitizeState = DigitizeState.CurveState;
            foreach (MatchPoint p in matchSet.Matches)
            {
                if(p.Accepted)
                    AddPoint(p.Point.X, p.Point.Y);
            }
            DigitizeState = old;
        }

        //Clears all points in the matchset
        public void DiscardMatchedPoints()
        {
            MatchSet.Matches.Clear();
        }

        public void DrawSegments()
        {
            segments.MakeSegments(processedImage, segmentSettings);
        }

        public LengthUnits Units
        {
            get
            {
                return units;
            }

            set
            {
                units = value;
            }
        }
    }
}
