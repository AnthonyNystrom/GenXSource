using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using Genetibase.NuGenTransform.Properties;

namespace Genetibase.NuGenTransform
{
    //Encapsulates all operations on a group of pointsets contained within the document
    //
    //Limits the document to only be able to contain an axis set, scale set, and a collection
    // of curve and measure sets
    public class NuGenPointSetCollection
    {

        public static readonly string AxesPointSetName = "Axes";
        public static readonly string ScalePointSetName = "Scale";
        public static readonly string DefaultCurveName = "Curve1";
        public static readonly string DefaultMeasureName = "Measure1";

        public NuGenPointSetCollection()
        {
            axesPointSet = new NuGenPointSet();
            axesPointSet.Style = NuGenDefaultSettings.GetInstance().AxesStyle;
            scalePointSet = new NuGenPointSet();
            scalePointSet.Style = NuGenDefaultSettings.GetInstance().ScaleStyle;
            curveList = new List<NuGenPointSet>();
            measureList = new List<NuGenPointSet>();
        }
        
        // add a curve or measure pointset to the pointset list. the axis and scale bar pointsets 
        // are automatically added during ruction
        public void AddCurve(string name)
        {
            NuGenPointSet pointSet = new NuGenPointSet();
            pointSet.Name = name;

            foreach (NuGenPointSet p in curveList)
            {
                if (p.Name == name)
                    throw new ArgumentException("Can not create another curve set with the name " + name);
            }

            pointSet.Style = NuGenDefaultSettings.GetInstance().DefaultCurveStyle;
            curveList.Add(pointSet);
        }

        //Gets a curve from its name
        public NuGenPointSet GetCurve(string name)
        {
            foreach (NuGenPointSet p in curveList)
            {
                if (p.Name.Equals(name))
                    return p;
            }

            return new NuGenPointSet();
        }

        public void AddMeasure(string name)
        {
            NuGenPointSet pointSet = new NuGenPointSet();
            pointSet.Name = name;

            foreach (NuGenPointSet p in measureList)
            {
                if (p.Name == name)
                    throw new ArgumentException("Can not create another curve set with the name " + name);
            }

            pointSet.Style = NuGenDefaultSettings.GetInstance().DefaultMeasureStyle;
            measureList.Add(pointSet);
        }

        public NuGenPointSet GetMeasure(string name)
        {
            foreach (NuGenPointSet p in measureList)
            {
                if (p.Name.Equals(name))
                    return p;
            }

            return new NuGenPointSet();
        }

        public NuGenPointSet Axes
        {
            get
            {
                return axesPointSet;
            }
        }

        public NuGenPointSet ScaleBar
        {
            get
            {
                return scalePointSet;
            }
        }

        public List<NuGenPointSet> Curves
        {
            get
            {
                return curveList;
            }

            set
            {
                curveList = value;
            }
        }

        public List<NuGenPointSet> Measures
        {
            get
            {
                return measureList;
            }

            set
            {
                measureList = value;
            }
        }

        // add a point to an axes, curve, measure or scale pointset
        public void AddPointAxes(NuGenPoint p)
        {
            axesPointSet.AddPoint(p);
        }

        //Adds the point p to the curve with the name "name"
        public void AddPointCurve(NuGenPoint p, string name)
        {
            foreach (NuGenPointSet pointset in curveList)
            {
                if (pointset.Name.Equals(name))
                {
                    pointset.AddPoint(p);
                }
            }
        }

        //Adds the point p to the measure with the name "name"
        public void AddPointMeasure(NuGenPoint p, string name)
        {
            foreach (NuGenPointSet pointset in measureList)
            {
                if (pointset.Name.Equals(name))
                {
                    pointset.AddPoint(p);
                }
            }
        }

        public void AddPointScale(NuGenPoint p)
        {
            scalePointSet.AddPoint(p);
        }

        public PointSetStyle AxesStyle
        {
            get
            {
                return axesPointSet.Style;                
            }

            set
            {
                axesPointSet.Style = value;
            }
        }

        public PointSetStyle GetCurveStyle(string name)
        {
            foreach(NuGenPointSet curve in curveList)
            {
                if (curve.Name.Equals(name))
                    return curve.Style;
            }

            return new PointSetStyle();
        }

        public void SetCurveStyle(string name, PointSetStyle style)
        {
            foreach (NuGenPointSet curve in curveList)
            {
                if (curve.Name.Equals(name))
                    curve.Style = style;
            }
        }

        public PointSetStyle GetMeasureStyle(string name)
        {
            foreach (NuGenPointSet measure in measureList)
            {
                if (measure.Name.Equals(name))
                    return measure.Style;
            }

            return new PointSetStyle();
        }

        public void SetMeasureStyle(string name, PointSetStyle style)
        {
            foreach (NuGenPointSet measure in measureList)
            {
                if (measure.Name.Equals(name))
                    measure.Style = style;
            }
        }

        public PointSetStyle ScaleStyle
        {
            get
            {
                return scalePointSet.Style;
            }

            set
            {
                scalePointSet.Style = value;
            }
        }     

        // return bounds on the coordinates for just axis, or all, pointsets, and true if
        // there were any coordinates
        public bool AxisSetGraphLimits(CoordSettings coord, NuGenScreenTranslate transform,
          ref double xThetaMin, ref double xThetaMax,
          ref double yRMin, ref double yRMax)
        {
            UpdateGraphCoordinates(coord, transform);

            // get limits
            return axesPointSet.PointSetGraphLimits(ref xThetaMin, ref xThetaMax, ref yRMin, ref yRMax);
        }

        public bool PointSetGraphLimits(CoordSettings coord, NuGenScreenTranslate transform,
          ref double xThetaMin, ref double xThetaMax,
          ref double yRMin, ref double yRMax)
        {
            // update and get axis limits
            bool first = AxisSetGraphLimits(coord, transform, ref xThetaMin, ref xThetaMax, ref yRMin, ref yRMax);
            SinglePointSetGraphLimits(scalePointSet, ref first, ref xThetaMin, ref xThetaMax, ref yRMin, ref yRMax);

            foreach (NuGenPointSet pointset in curveList)
            {
                SinglePointSetGraphLimits(pointset, ref first, ref xThetaMin, ref xThetaMax, ref yRMin, ref yRMax);
            }

            foreach (NuGenPointSet pointset in measureList)
            {
                SinglePointSetGraphLimits(pointset, ref first, ref xThetaMin, ref xThetaMax, ref yRMin, ref yRMax);
            }

            return !first;
        }

        // apply the transformation to set graph coordinates of the points in the curves and 
        // measures. if the operation could cause points to be reordered to keep a curve 
        // single-valued then the optional update list should be used
        public void UpdateGraphCoordinates(CoordSettings coord, NuGenScreenTranslate transform)
        {
            // apply transformation
            foreach (NuGenPointSet pointSet in curveList)
            {
                pointSet.UpdateGraphCoordinates(coord, transform);
                pointSet.ForceSingleValued(coord, transform);
            }

            foreach (NuGenPointSet pointSet in measureList)
            {
                pointSet.UpdateGraphCoordinates(coord, transform);
            }
        }        

        // return coordinate range for specified pointset
        public void SinglePointSetGraphLimits(NuGenPointSet p, ref bool first,
          ref double xThetaMin, ref double xThetaMax, ref double yRMin, ref double yRMax)
        {
            double xtmin = 0.0, xtmax = 0.0, yrmin = 0.0, yrmax = 0.0;

            if (p.PointSetGraphLimits(ref xtmin, ref xtmax, ref yrmin, ref yrmax))
            {
                if (first || (xtmin < xThetaMin))
                    xThetaMin = xtmin;
                if (first || (xtmax > xThetaMax))
                    xThetaMax = xtmax;
                if (first || (yrmin < yRMin))
                    yRMin = yrmin;
                if (first || (yrmax > yRMax))
                    yRMax = yrmax;

                first = false;
            }
        }
          
        // serialize
        public void SerializeWrite(System.Runtime.Serialization.SerializationInfo info)
        {

            axesPointSet.SerializeWrite(info, "axes");
            scalePointSet.SerializeWrite(info, "scale");

            info.AddValue("curves.count", curveList.Count);
            info.AddValue("measures.count", measureList.Count);

            int index = 0;
            foreach(NuGenPointSet p in curveList) 
            {
                p.SerializeWrite(info, "curve" + index++);
            }

            index = 0;
            foreach(NuGenPointSet p in measureList)
            {
                p.SerializeWrite(info, "measure" + index++);
            }
        }

        //deserialize
        public void SerializeRead(System.Runtime.Serialization.SerializationInfo info)
        {
            axesPointSet.SerializeRead(info, "axes");
            scalePointSet.SerializeRead(info, "scale");

            for (int i = 0; i < (int)info.GetValue("curves.count", typeof(int)); i++)
            {
                NuGenPointSet curve = new NuGenPointSet();
                curve.SerializeRead(info, "curve" + i);
                curveList.Add(curve);
            }

            for (int i = 0; i < (int)info.GetValue("measures.count", typeof(int)); i++)
            {
                NuGenPointSet measure = new NuGenPointSet();
                measure.SerializeRead(info, "measure" + i);
                measureList.Add(measure);
            }
        }
        
        // export the curves into a text stream
        public void ExportToFile(Stream str, CoordSettings coord, GridMeshSettings grid,
          ExportSettings xport)
        {
            int xPrecision, yPrecision;

            SelectXYPrecisionsForExport(coord, out xPrecision, out yPrecision);

            if (xport.layout == ExportLayout.AllCurvesOnEachLine)
                ExportToStreamAllCurvesTogether(str, coord, grid, xport, xPrecision, yPrecision);
            else
                ExportToStreamEachCurveSeparately(str, coord, grid, xport, xPrecision, yPrecision);
        }
        
        // lists of curves to be included and excluded from export
        public List<string> ExportIncluded()
        {
            List<string> curves = new List<string>();

            foreach (NuGenPointSet pointSet in curveList)
            {
                if (pointSet.Export)
                    curves.Add(pointSet.Name);
            }

            return curves;
        }

        //Gets the list of curves which are included in the export
        public List<string> ExportExcluded()
        {
            List<string> curves = new List<string>();

            foreach (NuGenPointSet pointSet in curveList)
            {
                if (!pointSet.Export)
                    curves.Add(pointSet.Name);
            }

            return curves;
        }

        // include and exclude a curve from export
        public void ExportInclude(string curve)
        {
            GetCurve(curve).Export = true;
        }

        public void ExportExclude(string curve)
        {
            GetCurve(curve).Export = false;
        }

        //Returns a list of all points in all contained pointsets
        public List<NuGenPoint> AllPoints
        {
            get
            {
                List<NuGenPoint> points = new List<NuGenPoint>();

                foreach (NuGenPoint p in axesPointSet.Points)
                {
                    points.Add(p);
                }

                foreach (NuGenPoint p in scalePointSet.Points)
                {
                    points.Add(p);
                }

                foreach (NuGenPointSet pointSet in curveList)
                {
                    foreach (NuGenPoint p in pointSet.Points)
                    {
                        points.Add(p);
                    }
                }

                foreach (NuGenPointSet pointSet in measureList)
                {
                    foreach (NuGenPoint p in pointSet.Points)
                    {
                        points.Add(p);
                    }
                }

                return points;
            }
        }

        // return the geometry info for a particular curve or measure pointset
        public void GeometryInfoCurve(bool validTransform, bool cartesian, string name,
          List<NuGenGeometryWindowItem> rInfo, LengthUnits units)
        {
            int pNextRow = 0;
            foreach (NuGenPointSet pointset in curveList)
            {
                if (pointset.Name.Equals(name))
                    pointset.GeometryInfo(validTransform, cartesian, ref pNextRow, rInfo, units);
            }
        }

        public void GeometryInfoMeasure(bool validTransform, bool cartesian, string name,
          List<NuGenGeometryWindowItem> rInfo, LengthUnits units)
        {
            int pNextRow = 0;
            foreach (NuGenPointSet pointset in measureList)
            {
                if (pointset.Name.Equals(name))
                    pointset.GeometryInfo(validTransform, cartesian, ref pNextRow, rInfo, units);
            }
        }

        // return point coordinates of specified curve
        public List<Point> CurveCoordinates(string name)
        {
            List<Point> points = new List<Point>();
            foreach (NuGenPointSet pointset in curveList)
            {
                if (pointset.Name == name)
                {
                    foreach (NuGenPoint p in pointset.Points)
                    {
                        Point result = new Point(p.XScreen, p.YScreen);
                        points.Add(result);
                    }

                    return points;
                }
            }

            return null;
        }                     

        // first part of each header has optional gnuplot delimiter, and 'x' or 'theta'
        private string ExportHeaderPrefix(CoordSettings coord, ExportSettings xport)
        {
            string prefix = "";

            if (xport.header == ExportHeader.HeaderGnuplot)
                prefix += "#";

            if (coord.frame == ReferenceFrame.Cartesian)
                prefix += "x";
            else
                prefix += "theta";

            return prefix;
        }

        // return list of x values in ascending order
        private List<double> AscendingXValuesList(CoordSettings coord,
          GridMeshSettings grid, ExportSettings xport, int xPrecision)
        {
            List<double> dList = new List<double>();

            switch (xport.pointsSelection)
            {
                case ExportPointsSelection.XFromAllCurves:
                case ExportPointsSelection.XYFromAllCurves:
                    {
                        foreach (NuGenPointSet pointset in curveList)
                        {
                            pointset.MergeUniqueXValues(dList, xPrecision);
                        }
                    }
                    break;
                case ExportPointsSelection.XFromFirstCurve:
                    // for simplicity for the user and in code, we use x values from the
                    // first curve, not the first exported curve according to the export flags
                    (curveList.ToArray().GetValue(0) as NuGenPointSet).MergeUniqueXValues(dList, xPrecision);
                    break;
                case ExportPointsSelection.XFromGridLines:
                    {
                        double x = grid.startX;
                        for (int i = 0; i < grid.countX; i++)
                        {
                            dList.Add(x);

                            if (coord.xThetaScale == Scale.Linear)
                                x += grid.stepX;
                            else
                                x *= grid.stepX;
                        }
                    }
                    break;
            }

            return dList;
        }

        // curves can be exported together in multiple columns, or one after the other in the same column
        private void ExportToStreamAllCurvesTogether(Stream str, CoordSettings coord, GridMeshSettings grid,
          ExportSettings xport, int xPrecision, int yPrecision)
        {
                          // list of x values in ascending order
              List<double> xUsed = AscendingXValuesList(coord, grid, xport, xPrecision);

            StreamWriter writer = new StreamWriter(str);

              if (xport.header != ExportHeader.HeaderNone)
              {
                // header
                writer.Write(ExportHeaderPrefix(coord, xport));
                  foreach(NuGenPointSet pointset in curveList)
                  {
                      if(pointset.Export)
                      {
                          writer.Write(xport.GetDelimiter());
                          writer.Write(pointset.ExportCurveHeader(xport));
                      }
                  }
                writer.Write("\n");
              }

              // if only allowing raw data, then forgo interpolation
              bool useInterpolation = (xport.pointsSelection != ExportPointsSelection.XYFromAllCurves);

              foreach (double itrX in xUsed)
              {
                  writer.Write(Math.Round(itrX, xPrecision).ToString());
                  foreach (NuGenPointSet pointset in curveList)
                  {
                      if (pointset.Export)
                      {
                          writer.Write(xport.GetDelimiter());
                          writer.Write(pointset.ExportCurvePoint(itrX, coord, useInterpolation, yPrecision));
                      }
                  }

                  writer.Write("\n");
              }

              writer.Flush();
              writer.Close();
        }

        //Exports all of the curve data to the given stream with each curve having its own line
        private void ExportToStreamEachCurveSeparately(Stream str, CoordSettings coord, GridMeshSettings grid,
          ExportSettings xport, int xPrecision, int yPrecision)
        {
              // list of x values in ascending order
              List<double> xUsed = AscendingXValuesList(coord, grid, xport, xPrecision);

            StreamWriter writer = new StreamWriter(str);

            foreach(NuGenPointSet pointset in curveList)
            {
                if(pointset.Export)
                {
                    if(xport.header != ExportHeader.HeaderNone)
                    {
                        writer.Write(ExportHeaderPrefix(coord, xport));
                        writer.Write(xport.GetDelimiter());
                        writer.Write(pointset.ExportCurveHeader(xport));
                        writer.Write("\n");
                    }

                    if (xport.pointsSelection == ExportPointsSelection.XYFromAllCurves)
                        writer.Write(pointset.ExportCurveAll(xport, xPrecision, yPrecision));
                    else
                    {
                        bool useInterpolation = true;

                        foreach (double itrX in xUsed)
                        {
                            writer.Write(Math.Round(itrX, xPrecision));
                            writer.Write(xport.GetDelimiter());
                            writer.Write(pointset.ExportCurvePoint(itrX, coord, useInterpolation, yPrecision));
                            writer.Write("\n");
                        }

                        if (xport.header == ExportHeader.HeaderGnuplot)
                        {
                            writer.Write("\n\n");
                        }
                    }
                }
            }

            writer.Flush();
            writer.Close();
        }

        // select smallest possible precision values, to reduce clutter in export, while still
        // including the significant digits
        private void SelectXYPrecisionsForExport(CoordSettings coord, out int xPrecision, out int yPrecision)
        {
            // these defaults are the same as QString and QTextStream. however, they are inadequate when
            // the resolution is much smaller than the range, like when numeric date on the x axis
            // ranges from 38242 (9/12/2004) to 38243 (9/13/2004) and the resolution is 0.0033 days/pixel    

            xPrecision = yPrecision = 6;

            if ((coord.xThetaScale == Scale.Linear) ||
              (coord.yRScale == Scale.Linear))
            {
                bool first = true;
                double xThetaMin = 0, xThetaMax = 0, yRMin = 0, yRMax = 0;

                // increase the precision if necessary so all significant digits are included
                //This is the first subroutine to determine the optimal values
                // for xThetaMin, xThetaMax, yRMin and yRmax
                #region First Accuracy Optimization Subroutine
                foreach (NuGenPointSet pointset in curveList)
                {
                    double xtmin = 0, xtmax = 0, yrmin = 0, yrmax = 0;

                    bool found = false;

                    foreach(NuGenPoint point in pointset.Points)
                    {
                        if (!found || (point.XThetaGraph < xThetaMin))
                            xThetaMin = point.XThetaGraph;
                        if (!found || (point.XThetaGraph > xThetaMax))
                            xThetaMax = point.XThetaGraph;
                        if (!found || (point.YRGraph < yRMin))
                            yRMin = point.YRGraph;
                        if (!found || (point.YRGraph > yRMax))
                            yRMax = point.YRGraph;

                        found = true;
                    }

                    if (found)
                    {
                        if (first || (xtmin < xThetaMin))
                            xThetaMin = xtmin;
                        if (first || (xtmax > xThetaMax))
                            xThetaMax = xtmax;
                        if (first || (yrmin < yRMin))
                            yRMin = yrmin;
                        if (first || (yrmax > yRMax))
                            yRMax = yrmax;

                        first = false;
                    }
                }
                #endregion

                //reset the search

                first = true;

                double xMin = 0, xMax = 0, yMin = 0, yMax = 0;//Second Calculation Variables
                //This second subroutine caluclations the values xMin, xMax, yMin and yMax
                #region Second Accuracy Optimization Subroutine
                foreach (NuGenPointSet pointset in curveList)
                {
                    double xmin = 0, xmax = 0, ymin = 0, ymax = 0;//calculation helper vars

                    bool found = false;

                    foreach(NuGenPoint point in pointset.Points)
                    {
                      if (!found || (point.XScreen) < xMin)
                        xmin = point.XScreen;
                      if (!found || (point.XScreen > xMax))
                        xmax = point.XScreen;
                      if (!found || (point.YScreen < yMin))
                        ymin = point.YScreen;
                      if (!found || (point.YScreen > yMax))
                        ymax = point.YScreen;

                      found = true;
                    }


                    if (found)
                    {
                        if (first || (xmin < xMin))
                            xMin = xmin;
                        if (first || (xmax > xMax))
                            xMax = xmax;
                        if (first || (ymin < yMin))
                            yMin = ymin;
                        if (first || (ymax > yMax))
                            yMax = ymax;

                        first = false;
                    }
                }
                #endregion

                AdjustXOrYPrecisionForExport(coord.xThetaScale, xMin, xMax, xThetaMin, xThetaMax, ref xPrecision);
                AdjustXOrYPrecisionForExport(coord.yRScale, yMin, yMax, yRMin, yRMax, ref yPrecision);
            }
        }            

        // adjust, if necessary, the precision in the x or y direction
        private void AdjustXOrYPrecisionForExport(Scale scale, double vScreenMin, double vScreenMax,
          double vGraphMin, double vGraphMax, ref int precision)
        {
            if ((scale == Scale.Linear) && (vScreenMax - vScreenMin > 0))
            {
                double vLargestMagnitude = (Math.Abs(vGraphMax) > Math.Abs(vGraphMin)) ?
                  Math.Abs(vGraphMax) :
                  Math.Abs(vGraphMin);

                double vResolution = (vGraphMax - vGraphMin) / (vScreenMax - vScreenMin);

                // minimum resolution is computed so that moving by a single pixel (which corresponds to
                // jump in graph coordinates of 1-pixel x vResolution) should cause a change in at least
                // one digit of the largest possible exported value (which is vLargestMagnitude)
                int minPrecision = 2 + (int)Math.Log10(vLargestMagnitude / vResolution);

                if (minPrecision > precision)
                    precision = minPrecision;             
            }
        }
      
        // single axis pointset
        private NuGenPointSet axesPointSet;

        // single scale bar pointset
        private NuGenPointSet scalePointSet;

        // pointset list containing one or more curve pointsets
        private List<NuGenPointSet> curveList;

        // pointset list containing one or more measure pointsets
        private List<NuGenPointSet> measureList;
    }
}
