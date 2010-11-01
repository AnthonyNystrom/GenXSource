using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.NuGenTransform.Properties;
using System.Collections;

namespace Genetibase.NuGenTransform
{

#region Structures and Enumerations Declaration
    public struct DiscretizeSettings
    {
        public DiscretizeMethod discretizeMethod;

        // low threshold is higher than high threshold when range is union of
        // values below high threshold, and values above low threshold
        public int intensityThresholdLow;
        public int intensityThresholdHigh;
        public int foregroundThresholdLow;
        public int foregroundThresholdHigh;
        public int hueThresholdLow;
        public int hueThresholdHigh;
        public int saturationThresholdLow;
        public int saturationThresholdHigh;
        public int valueThresholdLow;
        public int valueThresholdHigh;
    };

    public struct SessionSettings
    {
        public DigitizeState initialDigitizeState;
    }

    public struct ExportSettings
    {
        public ExportPointsSelection pointsSelection;
        public ExportLayout layout;
        public ExportDelimiters delimiters;
        public ExportHeader header;

        public string GetDelimiter()
        {
            switch(delimiters)
            {
                case ExportDelimiters.Commas: return ",";
                case ExportDelimiters.Spaces: return " ";
                case ExportDelimiters.Tabs: return "\t";
            }

            return ",";
        }
    };

    public struct CoordSettings
    {
        public ReferenceFrame frame;
        public Scale xThetaScale;
        public Scale yRScale;
        public ThetaUnits thetaUnits;
    };

    // display and removal gridline meshes are regularly spaced
    public struct GridMeshSettings
    {
        public bool initialized;
        public GridSet gridSetX;
        public GridSet gridSetY;
        public int countX;
        public int countY;
        public double stepX;
        public double stepY;
        public double startX;
        public double startY;
        public double stopX;
        public double stopY;
    };
#endregion

    //Provides an interface between the application and the low level
    // settings class
    class NuGenDefaultSettings
    {

        private static NuGenDefaultSettings instance;

        public static NuGenDefaultSettings GetInstance()
        {
            if (instance == null)
            {
                instance = new NuGenDefaultSettings();
            }

            return instance;
        }

        public NuGenDefaultSettings()
        {
            LoadSettings();
        }

        private CoordSettings coordSettings;
        private ExportSettings exportSettings;
        private SegmentSettings segmentSettings;
        private PointMatchSettings pointMatchSettings;
        private GridRemovalSettings gridRemovalSettings;
        private DiscretizeSettings discretizeSettings;
        private SessionSettings sessionsSettings;

        private List<PointSetStyle> curveStyles = new List<PointSetStyle>();
        private List<PointSetStyle> measureStyles = new List<PointSetStyle>();

        private GridSet gridDisplayGridSetX;
        private GridSet gridDisplayGridSetY;

        private PointSetStyle axesStyle;
        private PointSetStyle scaleStyle;

        private bool viewFileToolbar;
        private bool viewSelectToolbar;
        private bool viewImageScaleToolbar;
        private bool viewDigitizeCurvePointsToolbar;
        private bool viewDigitizeMeasurePointsToolbar;
        private bool viewZoomToolbar;
        private bool viewStatusBar;
        private ViewPointSelection viewPoints;
        private BackgroundSelection viewBackground;
        private bool viewCurveGeometry;
        private bool viewMeasureGeometry;

        private int powerMostSigMax;
        private int powerMostSigMin;

        private double doubleMin;

        private int maxCommands;

        private Size windowCurveSize;
        private Point windowCurvePosition;

          // initial width+x should be less than 800 pixels to fit low resolution displays, and
          // initial height+y should be less than 640 pixels to fit low resolution displays
        private Size windowMainSize;
        private Point windowMainPosition;
        private bool windowMainFontOverride;
        private string windowMainFontName;
        private int windowMainFontSize;

          // initial width+x should be less than 800 pixels to fit low resolution displays, and
          // initial height+y should be less than 640 pixels to fit low resolution displays
        private Size windowMeasureSize;
        private Point windowMeasurePosition;

        private int segmentPointMinSeparation = 4;

        private int pointMatchHighlightDiameter;
        private int pointMatchHighlightLineSize;

        public void LoadSettings()
        {
            sessionsSettings.initialDigitizeState = Settings.Default.SESSIONS_INITIALDIGITZESTATE;

            coordSettings.frame = Settings.Default.COORD_FRAME;
            coordSettings.xThetaScale = Settings.Default.COORD_XTHETASCALE;
            coordSettings.yRScale = Settings.Default.COORD_YRSCALE;
            coordSettings.thetaUnits = Settings.Default.COORD_THETAUNITS;

            exportSettings.pointsSelection = Settings.Default.EXPORT_POINTSSELECTION;
            exportSettings.layout = Settings.Default.EXPORT_LAYOUT;
            exportSettings.delimiters = Settings.Default.EXPORT_DELIMITERS;
            exportSettings.header = Settings.Default.EXPORT_HEADER;

            viewFileToolbar = Settings.Default.VIEW_FILETOOLBAR;
            viewSelectToolbar = Settings.Default.VIEW_SELECTTOOLBAR;
            viewImageScaleToolbar = Settings.Default.VIEW_IMAGESCALETOOLBAR;
            viewDigitizeCurvePointsToolbar = Settings.Default.VIEW_DIGITIZECURVEPOINTSTOOLBAR;
            viewDigitizeMeasurePointsToolbar = Settings.Default.VIEW_DIGITIZEMEASUREPOINTSTOOLBAR;
            viewZoomToolbar = Settings.Default.VIEW_ZOOMTOOLBAR;
            viewStatusBar = Settings.Default.VIEW_STATUSBAR;
            viewPoints = Settings.Default.VIEW_POINTS;
            viewBackground = Settings.Default.VIEW_BACKGROUND;
            viewCurveGeometry = Settings.Default.VIEW_CURVEGEOMETRY;
            viewMeasureGeometry = Settings.Default.VIEW_MEASUREGEOMETRY;

            powerMostSigMax = Settings.Default.MATH_POWERMOSTSIGMAX;
            powerMostSigMin = Settings.Default.MATH_POWERMOSTSIGMIN;
            doubleMin = Settings.Default.MATH_DOUBLEMIN;
            maxCommands = Settings.Default.MATH_MAXCOMMANDS;

            windowCurveSize.Width = Settings.Default.WINDOW_CURVE_WIDTH;
            windowCurveSize.Height = Settings.Default.WINDOW_CURVE_HEIGHT;
            windowCurvePosition.X = Settings.Default.WINDOW_CURVE_X;
            windowCurvePosition.Y = Settings.Default.WINDOW_CURVE_Y;

            windowMainSize.Width = Settings.Default.WINDOW_MAIN_WIDTH;
            windowMainSize.Height = Settings.Default.WINDOW_MAIN_HEIGHT;
            windowMainPosition.X = Settings.Default.WINDOW_MAIN_X;
            windowMainPosition.Y = Settings.Default.WINDOW_MAIN_Y;
            windowMainFontOverride = Settings.Default.WINDOW_MAIN_FONT_OVERRIDE;
            windowMainFontName = Settings.Default.WINDOW_MAIN_FONT_NAME;
            windowMainFontSize = Settings.Default.WINDOW_MAIN_FONT_SIZE;

            windowMeasureSize.Width = Settings.Default.WINDOW_MEASURE_WIDTH;
            windowMeasureSize.Height = Settings.Default.WINDOW_MEASURE_HEIGHT;
            windowMeasurePosition.X = Settings.Default.WINDOW_MEASURE_X;
            windowMeasurePosition.Y = Settings.Default.WINDOW_MEASURE_Y;

            segmentPointMinSeparation = Settings.Default.SEGMENT_POINTMINSEPARATION;
            segmentSettings.minPoints = Settings.Default.SEGMENT_MINPOINTS;
            segmentSettings.pointSeparation = Settings.Default.SEGMENT_POINTDEFAULTSEPARATION;
            segmentSettings.fillCorners = Settings.Default.SEGMENT_FILLCORNERS;
            segmentSettings.lineSize = Settings.Default.SEGMENT_LINESIZE;
            segmentSettings.lineColor = Settings.Default.SEGMENT_LINECOLOR;

            gridRemovalSettings.gridMesh.gridSetX = Settings.Default.GRID_REMOVAL_GRIDSETX;
            gridRemovalSettings.gridMesh.gridSetY = Settings.Default.GRID_REMOVAL_GRIDSETY;
            gridRemovalSettings.thinThickness = Settings.Default.GRID_REMOVAL_THINTHICKNESS;
            gridRemovalSettings.gridDistance = Settings.Default.GRID_REMOVAL_GRIDDISTANCE;
            gridRemovalSettings.color = Settings.Default.GRID_REMOVAL_COLOR;
            gridRemovalSettings.gapSeparation = Settings.Default.GRID_REMOVAL_GAPSEPARATION;
            gridRemovalSettings.foregroundThresholdLow = Settings.Default.GRID_REMOVAL_FOREGROUNDTHRESHOLDLOW;
            gridRemovalSettings.foregroundThresholdHigh = Settings.Default.GRID_REMOVAL_FOREGROUNDTHRESHOLDHIGH;

            gridDisplayGridSetX = Settings.Default.GRID_DISPLAY_GRIDSETX;
            gridDisplayGridSetY = Settings.Default.GRID_DISPLAY_GRIDSETY;

            pointMatchHighlightDiameter = Settings.Default.POINTMATCH_HIGHLIGHTDIAMETER;
            pointMatchHighlightLineSize = Settings.Default.POINTMATCH_HIGHLIGHTLINESIZE;
            pointMatchSettings.pointSeparation = Settings.Default.POINTMATCH_SEPARATIONDEFAULT;
            pointMatchSettings.pointSize = Settings.Default.POINTMATCH_SIZEDEFAULT;
            pointMatchSettings.acceptedColor = Settings.Default.POINTMATCH_ACCEPTEDCOLOR;
            pointMatchSettings.rejectedColor = Settings.Default.POINTMATCH_REJECTEDCOLOR;

            discretizeSettings.discretizeMethod = Settings.Default.DISCRETIZE_METHODDEFAULT;
            discretizeSettings.intensityThresholdLow = Settings.Default.DISCRETIZE_INTENSITY_THRESHOLDLOW;
            discretizeSettings.intensityThresholdHigh = Settings.Default.DISCRETIZE_INTENSITY_THRESHOLDHIGH;
            discretizeSettings.foregroundThresholdLow = Settings.Default.DISCRETIZE_FOREGROUND_THRESHOLDLOW;
            discretizeSettings.foregroundThresholdHigh = Settings.Default.DISCRETIZE_FOREGROUND_THRESHOLDHIGH;
            discretizeSettings.hueThresholdLow = Settings.Default.DISCRETIZE_HUE_THRESHOLDLOW;
            discretizeSettings.hueThresholdHigh = Settings.Default.DISCRETIZE_HUE_THRESHOLDHIGH;
            discretizeSettings.saturationThresholdLow = Settings.Default.DISCRETIZE_SATURATION_THRESHOLDLOW;
            discretizeSettings.saturationThresholdHigh = Settings.Default.DISCRETIZE_SATURATION_THRESHOLDHIGH;
            discretizeSettings.valueThresholdLow = Settings.Default.DISCRETIZE_VALUE_THRESHOLDLOW;
            discretizeSettings.valueThresholdHigh = Settings.Default.DISCRETIZE_VALUE_THRESHOLDHIGH;

            axesStyle.pointShape = Settings.Default.POINTSET_AXES_POINTSHAPE;
            axesStyle.pointSize = Settings.Default.POINTSET_AXES_POINTSIZE;
            axesStyle.pointLineSize = Settings.Default.POINTSET_AXES_POINTLINESIZE;
            axesStyle.pointLineColor = Settings.Default.POINTSET_AXES_POINTLINECOLOR;
            axesStyle.pointInColor = Settings.Default.POINTSET_AXES_POINTINCOLOR;
            axesStyle.lineSize = Settings.Default.POINTSET_AXES_LINESIZE;
            axesStyle.lineColor = Settings.Default.POINTSET_AXES_LINECOLOR;
            axesStyle.lineConnectAs = Settings.Default.POINTSET_AXES_LINECONNECTAS;

            scaleStyle.pointShape = Settings.Default.POINTSET_SCALE_POINTSHAPE;
            scaleStyle.pointSize = Settings.Default.POINTSET_SCALE_POINTSIZE;
            scaleStyle.pointLineSize = Settings.Default.POINTSET_SCALE_POINTLINESIZE;
            scaleStyle.pointLineColor = Settings.Default.POINTSET_SCALE_POINTLINECOLOR;
            scaleStyle.pointInColor = Settings.Default.POINTSET_SCALE_POINTINCOLOR;
            scaleStyle.lineSize = Settings.Default.POINTSET_SCALE_LINESIZE;
            scaleStyle.lineColor = Settings.Default.POINTSET_SCALE_LINECOLOR;
            scaleStyle.lineConnectAs = Settings.Default.POINTSET_SCALE_LINECONNECTAS;

            try
            {
                foreach (PointSetStyle style in Settings.Default.POINTSET_CURVES)
                {
                    curveStyles.Add(style);
                }

                foreach (PointSetStyle style in Settings.Default.POINTSET_MEASURES)
                {
                    measureStyles.Add(style);
                }
            }
            catch (InvalidCastException e)
            {
                //There were no default pointsets loaded.
            }
        }

        public void SaveSettings()
        {
            Settings.Default.SESSIONS_INITIALDIGITZESTATE = sessionsSettings.initialDigitizeState;

            Settings.Default.COORD_FRAME = coordSettings.frame;
            Settings.Default.COORD_XTHETASCALE = coordSettings.xThetaScale;
            Settings.Default.COORD_YRSCALE = coordSettings.yRScale;
            Settings.Default.COORD_THETAUNITS = coordSettings.thetaUnits;

            Settings.Default.EXPORT_POINTSSELECTION = exportSettings.pointsSelection;
            Settings.Default.EXPORT_LAYOUT = exportSettings.layout;
            Settings.Default.EXPORT_DELIMITERS = exportSettings.delimiters;
            Settings.Default.EXPORT_HEADER = exportSettings.header;

            Settings.Default.VIEW_FILETOOLBAR = viewFileToolbar;
            Settings.Default.VIEW_SELECTTOOLBAR = viewSelectToolbar;
            Settings.Default.VIEW_IMAGESCALETOOLBAR = viewImageScaleToolbar;
            Settings.Default.VIEW_DIGITIZECURVEPOINTSTOOLBAR = viewDigitizeCurvePointsToolbar;
            Settings.Default.VIEW_DIGITIZEMEASUREPOINTSTOOLBAR = viewDigitizeMeasurePointsToolbar;
            Settings.Default.VIEW_ZOOMTOOLBAR = viewZoomToolbar;
            Settings.Default.VIEW_STATUSBAR = viewStatusBar;
            Settings.Default.VIEW_POINTS = viewPoints;
            Settings.Default.VIEW_BACKGROUND = viewBackground;
            Settings.Default.VIEW_CURVEGEOMETRY = viewCurveGeometry;
            Settings.Default.VIEW_MEASUREGEOMETRY = viewMeasureGeometry;

            Settings.Default.MATH_POWERMOSTSIGMAX = powerMostSigMax;
            Settings.Default.MATH_POWERMOSTSIGMIN = powerMostSigMin;
            Settings.Default.MATH_DOUBLEMIN = doubleMin;
            Settings.Default.MATH_MAXCOMMANDS = (int)maxCommands;

            Settings.Default.WINDOW_CURVE_WIDTH = windowCurveSize.Width;
            Settings.Default.WINDOW_CURVE_HEIGHT = windowCurveSize.Height;
            Settings.Default.WINDOW_CURVE_X = windowCurvePosition.X;
            Settings.Default.WINDOW_CURVE_Y = windowCurvePosition.Y;

            Settings.Default.WINDOW_MAIN_WIDTH = windowMainSize.Width;
            Settings.Default.WINDOW_MAIN_HEIGHT = windowMainSize.Height;
            Settings.Default.WINDOW_MAIN_X = windowMainPosition.X;
            Settings.Default.WINDOW_MAIN_Y = windowMainPosition.Y;
            Settings.Default.WINDOW_MAIN_FONT_OVERRIDE = windowMainFontOverride;
            Settings.Default.WINDOW_MAIN_FONT_NAME = windowMainFontName;
            Settings.Default.WINDOW_MAIN_FONT_SIZE = windowMainFontSize;

            Settings.Default.WINDOW_MEASURE_WIDTH = windowMeasureSize.Width;
            Settings.Default.WINDOW_MEASURE_HEIGHT = windowMeasureSize.Height;
            Settings.Default.WINDOW_MEASURE_X = windowMeasurePosition.X;
            Settings.Default.WINDOW_MEASURE_Y = windowMeasurePosition.Y;

            Settings.Default.SEGMENT_POINTMINSEPARATION = segmentPointMinSeparation;
            Settings.Default.SEGMENT_MINPOINTS = segmentSettings.minPoints;
            Settings.Default.SEGMENT_POINTDEFAULTSEPARATION = segmentSettings.pointSeparation;
            Settings.Default.SEGMENT_FILLCORNERS = segmentSettings.fillCorners;
            Settings.Default.SEGMENT_LINESIZE = segmentSettings.lineSize;
            Settings.Default.SEGMENT_LINECOLOR = segmentSettings.lineColor;

            Settings.Default.GRID_REMOVAL_GRIDSETX = gridRemovalSettings.gridMesh.gridSetX;
            Settings.Default.GRID_REMOVAL_GRIDSETY = gridRemovalSettings.gridMesh.gridSetY;
            Settings.Default.GRID_REMOVAL_THINTHICKNESS = gridRemovalSettings.thinThickness;
            Settings.Default.GRID_REMOVAL_GRIDDISTANCE = gridRemovalSettings.gridDistance;
            Settings.Default.GRID_REMOVAL_COLOR = gridRemovalSettings.color;
            Settings.Default.GRID_REMOVAL_GAPSEPARATION = gridRemovalSettings.gapSeparation;
            Settings.Default.GRID_REMOVAL_FOREGROUNDTHRESHOLDLOW = gridRemovalSettings.foregroundThresholdLow;
            Settings.Default.GRID_REMOVAL_FOREGROUNDTHRESHOLDHIGH = gridRemovalSettings.foregroundThresholdHigh;

            Settings.Default.GRID_DISPLAY_GRIDSETX = gridDisplayGridSetX;
            Settings.Default.GRID_DISPLAY_GRIDSETY = gridDisplayGridSetY;

            Settings.Default.POINTMATCH_HIGHLIGHTDIAMETER = pointMatchHighlightDiameter;
            Settings.Default.POINTMATCH_HIGHLIGHTLINESIZE = pointMatchHighlightLineSize;
            Settings.Default.POINTMATCH_SEPARATIONDEFAULT = pointMatchSettings.pointSeparation;
            Settings.Default.POINTMATCH_SIZEDEFAULT = pointMatchSettings.pointSize;
            Settings.Default.POINTMATCH_ACCEPTEDCOLOR = pointMatchSettings.acceptedColor;
            Settings.Default.POINTMATCH_REJECTEDCOLOR = pointMatchSettings.rejectedColor;

            Settings.Default.DISCRETIZE_METHODDEFAULT = discretizeSettings.discretizeMethod;
            Settings.Default.DISCRETIZE_INTENSITY_THRESHOLDLOW = discretizeSettings.intensityThresholdLow;
            Settings.Default.DISCRETIZE_INTENSITY_THRESHOLDHIGH = discretizeSettings.intensityThresholdHigh;
            Settings.Default.DISCRETIZE_FOREGROUND_THRESHOLDLOW = discretizeSettings.foregroundThresholdLow;
            Settings.Default.DISCRETIZE_FOREGROUND_THRESHOLDHIGH = discretizeSettings.foregroundThresholdHigh;
            Settings.Default.DISCRETIZE_HUE_THRESHOLDLOW = discretizeSettings.hueThresholdLow;
            Settings.Default.DISCRETIZE_HUE_THRESHOLDHIGH = discretizeSettings.hueThresholdHigh;
            Settings.Default.DISCRETIZE_SATURATION_THRESHOLDLOW = discretizeSettings.saturationThresholdLow;
            Settings.Default.DISCRETIZE_SATURATION_THRESHOLDHIGH = discretizeSettings.saturationThresholdHigh;
            Settings.Default.DISCRETIZE_VALUE_THRESHOLDLOW = discretizeSettings.valueThresholdLow;
            Settings.Default.DISCRETIZE_VALUE_THRESHOLDHIGH = discretizeSettings.valueThresholdHigh;

            Settings.Default.POINTSET_AXES_POINTSHAPE = axesStyle.pointShape;
            Settings.Default.POINTSET_AXES_POINTSIZE = axesStyle.pointSize;
            Settings.Default.POINTSET_AXES_POINTLINESIZE = axesStyle.pointLineSize;
            Settings.Default.POINTSET_AXES_POINTLINECOLOR = axesStyle.pointLineColor;
            Settings.Default.POINTSET_AXES_POINTINCOLOR = axesStyle.pointInColor;
            Settings.Default.POINTSET_AXES_LINESIZE = axesStyle.lineSize;
            Settings.Default.POINTSET_AXES_LINECOLOR = axesStyle.lineColor;
            Settings.Default.POINTSET_AXES_LINECONNECTAS = axesStyle.lineConnectAs;

            Settings.Default.POINTSET_SCALE_POINTSHAPE = scaleStyle.pointShape;
            Settings.Default.POINTSET_SCALE_POINTSIZE = scaleStyle.pointSize;
            Settings.Default.POINTSET_SCALE_POINTLINESIZE = scaleStyle.pointLineSize;
            Settings.Default.POINTSET_SCALE_POINTLINECOLOR = scaleStyle.pointLineColor;
            Settings.Default.POINTSET_SCALE_POINTINCOLOR = scaleStyle.pointInColor;
            Settings.Default.POINTSET_SCALE_LINESIZE = scaleStyle.lineSize;
            Settings.Default.POINTSET_SCALE_LINECOLOR = scaleStyle.lineColor;
            Settings.Default.POINTSET_SCALE_LINECONNECTAS = scaleStyle.lineConnectAs;

            Settings.Default.POINTSET_CURVES = new ArrayList(curveStyles);
            Settings.Default.POINTSET_MEASURES = new ArrayList(measureStyles);                

            Settings.Default.Save();
        }

        public PointSetStyle DefaultCurveStyle
        {
            get
            {
                PointSetStyle style = new PointSetStyle();
                style.lineColor = Color.Blue;
                style.lineConnectAs = LineConnectAs.SingleValuedFunction;
                style.lineSize = LineSize.LineSize2;
                style.pointInColor = Color.Transparent;
                style.pointLineColor = Color.Blue;
                style.pointLineSize = PointLineSize.PointLineSize2;
                style.pointShape = PointShape.Cross;
                style.pointSize = PointSize.PointSize4;

                return style;
            }
        }

        public PointSetStyle DefaultMeasureStyle
        {
            get
            {
                PointSetStyle style = new PointSetStyle();
                style.lineColor = Color.Cyan;
                style.lineConnectAs = LineConnectAs.SingleValuedFunction;
                style.lineSize = LineSize.LineSize2;
                style.pointInColor = Color.Transparent;
                style.pointLineColor = Color.Cyan;
                style.pointLineSize = PointLineSize.PointLineSize2;
                style.pointShape = PointShape.Cross;
                style.pointSize = PointSize.PointSize4;

                return style;
            }
        }

        public ViewPointSelection ViewPointSelection
        {
            get
            {
                return viewPoints;
            }

            set
            {
                viewPoints = value;
            }
        }

        public BackgroundSelection BackgroundSelection
        {
            get
            {
                return viewBackground;
            }

            set
            {
                viewBackground = value;
            }
        }

        public double DoubleMin
        {
            get
            {
                return doubleMin;
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
            }
        }

        public ExportSettings ExportSettings
        {
            get
            {
                return exportSettings;
            }
        }

        public SegmentSettings SegmentSettings
        {
            get
            {
                return segmentSettings;
            }
        }

        public int PointMatchHighlightDiameter
        {
            get
            {
                return pointMatchHighlightDiameter;
            }
        }

        public int PointMatchHighlightLineSize
        {
            get
            {
                return pointMatchHighlightLineSize;
            }
        }

        public PointMatchSettings PointMatchSettings
        {
            get
            {
                return pointMatchSettings;
            }
        }

        public GridRemovalSettings GridRemovalSettings
        {
            get
            {
                return gridRemovalSettings;
            }
        }

        public GridSet GridDisplayGridSetX
        {
            get
            {
                return GridSet.AllButStep;
            }
        }

        public GridSet GridDisplayGridSetY
        {
            get
            {
                return GridSet.AllButStep;
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

        public SessionSettings SessionsSettings
        {
            get
            {
                return sessionsSettings;
            }

            set
            {
                sessionsSettings = value;
            }
        }

        public PointSetStyle AxesStyle
        {
            get
            {
                return axesStyle;
            }

            set
            {
                axesStyle = value;
            }
        }

        public PointSetStyle ScaleStyle
        {
            get
            {
                return scaleStyle;
            }

            set
            {
                scaleStyle = value; 
            }
        }

        public Size WindowMeasureSize
        {
            get
            {
                return windowMeasureSize;
            }
        }

        public Point WindowMeasurePosition
        {
            get
            {
                return windowMeasurePosition;
            }
        }

        public Size WindowCurveSize
        {
            get
            {
                return windowCurveSize;
            }
        }

        public Point WindowCurvePosition
        {
            get
            {
                return windowCurvePosition;
            }
        }

        public Point WindowMainPosition
        {
            get
            {
                return windowMainPosition;
            }
        }

        public Size WindowMainSize
        {
            get
            {
                return windowMainSize;
            }
        }

        public bool WindowMainFontOverride
        {
            get
            {
                return windowMainFontOverride;
            }
        }

        public string WindowMainFontString
        {
            get
            {                
                return windowMainFontName;
            }
        }

        public int WindowMainFontSize
        {
            get
            {
                return windowMainFontSize;
            }
        }
    }
}
