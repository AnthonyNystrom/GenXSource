using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace Genetibase.NuGenTransform
{
    //Encapsulates all the operations on one particular collection of points, abstracts
    // the type of pointset so that it works for all pointsets whether it is a set of
    // axes points, scale points, curve points, measure points, or any new type of pointset
    // that could be later defined
    public class NuGenPointSet
    {       
        // user-specified name of this pointset
        private string name;

        // true/false to include/exclude the curve pointset during export
        private bool export;
        
        // shape and line style of this pointset
        private PointSetStyle style;

        // points belonging to this pointset. each point is owned by exactly one pointset, and lives
        // from when the user explicitly creates the point until the user explicitly deletes the point
        private List<NuGenPoint> points;

        // lines belonging to this pointset. points are sorted by ordinate and then linked by lines. all
        // lines are replaced if any of the following occurs (1) a point is added (2) a point is
        // deleted (3) the line style changes (4) the document is loaded. this approach may be a bit
        // slower, but it greatly simplifies the code, and realistically there should not be more
        // than a couple of hundred points per pointset so execution time is trivial
        private List<Line> lines = new List<Line>();


        public NuGenPointSet()
        {
            this.points = new List<NuGenPoint>();
            export = true;
        }
        
        // add point to pointset, not worrying about keeping the pointset single valued (which is for
        // curves only). if new point is on the line between two points then insert it between
        // those two points (desired behavior for curve and measure pointsets, which happens to not affect
        // axes and scale pointsets)
        public void AddPoint(NuGenPoint point)
        {
            // insert point between two other points if it lies on the line between the two points
            const int LineEpsilonPixels = 2;
            int index = 0;
            double x = point.XScreen;
            double y = point.YScreen;
            NuGenPoint pOld = null;
            foreach (NuGenPoint pNew in points)
            {
                double xNew = pNew.XScreen;
                double yNew = pNew.YScreen;

                if (pOld != null)
                {
                    double xOld = pOld.XScreen;
                    double yOld = pOld.YScreen;

                    double xProj, yProj;
                    NuGenMath.ProjectPointOnToLine(x, y, xOld, yOld, xNew, yNew, out xProj, out yProj);

                    double diff = Math.Sqrt((x - xProj) * (x - xProj) + (y - yProj) * (y - yProj));
                    if (diff < LineEpsilonPixels)
                    {
                        points.Insert(index, point);
                        point.PointSet = this;

                        RemoveLine(pOld, pNew);
                        AddLine(pOld, point);
                        AddLine(point, pNew);

                        return;
                    }
                }

                pOld = pNew;
                index++;
            }

            points.Add(point);
            point.PointSet = this;

            if (pOld != null)
                AddLine(pOld, point);
        }

        // remove a point from this pointset
        public void RemovePoint(NuGenPoint p)
        {
            if (!points.Contains(p))
                return;

            NuGenPoint pFirst = points[0];
            NuGenPoint pLast = points[points.Count - 1];
            if (points.Count > 1)
            {
                // remove one of its attached lines. four cases are:
                //   1. solitary point (0 lines->0 lines)
                //   2. leftmost point (1 attached lines->0 attached lines)
                //   3. doubly attached point (2 attached lines->1 attached line)
                //   4. rightmost point (1 attached line->0 attached lines)
                // not case 1
                if ((p == points[0] || (p == points[points.Count - 1])))
                {
                    NuGenPoint pOld = null;
                    foreach (NuGenPoint pNew in points)
                    {
                        if ((p == pOld && p == pFirst) || (p == pNew) && (p == pLast))
                        {
                            RemoveLine(pOld, pNew);
                            break;
                        }

                        pOld = pNew;
                    }
                }
                else
                {
                    NuGenPoint pOlder = null;
                    NuGenPoint pOld = null;
                    foreach (NuGenPoint pNew in points)
                    {
                        if (p == pOld)
                        {
                            if (pOlder != null)
                            {
                                RemoveLine(pOld, pNew);

                                Line pOlderNextLine = new Line();
                                pOlderNextLine.start.X = pOlder.XScreen;
                                pOlderNextLine.start.Y = pOlder.YScreen;
                                pOlderNextLine.end.X = pNew.XScreen;
                                pOlderNextLine.end.Y = pNew.YScreen;

                                pOlder.NextLine = pOlderNextLine;

                                pNew.PreviousLine = pOlder.NextLine;

                                break;
                            }
                        }

                        pOlder = pOld;
                        pOld = pNew;
                    }
                }
            }

            points.Remove(p);
        }
        
        // name of pointset

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public List<NuGenPoint> Points
        {
            get
            {
                return points;
            }
        }

        // style get and set methods. setStyle should be followed by global screen refresh
        public PointSetStyle Style
        {
            get
            {
                return style;
            }

            set
            {
                style = value;
            }
        }

        public bool Export
        {
            get
            {
                return export;
            }
            set
            {
                export = value;
            }
        }

        // return the name of this pointset, filtering out any embedded delimiters for easier parsing
        // downstream (which would be performed by external applications)
        public string ExportCurveHeader(ExportSettings xport)
        {
            return name.Replace(xport.GetDelimiter(), "");
        }
        
        // given an x value, return the corresponding y value, interpolating if necessary
        public string ExportCurvePoint(double x, CoordSettings coord, bool useInterpolation, int yPrecision)
        {
            string yNew = ""; // initial empty value might be returned if interpolation is disabled
  
              if (Points.Count == 0)
                return yNew;
                
              if (!useInterpolation)
              {
                // x value has to exactly match one of the points in this curve
                foreach(NuGenPoint p in Points)
                {
                  if (x == p.XThetaGraph)
                    return Math.Round(p.YRGraph, yPrecision).ToString();
                }

                return yNew;
              }

              if (Points.Count == 1)
              {
                  foreach(NuGenPoint p in Points)
                  {
                      return Math.Round(p.YRGraph, yPrecision).ToString();
                  }
              }

            IEnumerator<NuGenPoint> lEnum = Points.GetEnumerator();
            IEnumerator<NuGenPoint> rEnum = Points.GetEnumerator();
            rEnum.MoveNext();
            rEnum.MoveNext();
            lEnum.MoveNext();

            NuGenPoint pLeft = lEnum.Current;
            NuGenPoint pRight = rEnum.Current;

            while (x > rEnum.Current.XThetaGraph)
            {
                rEnum.MoveNext();
                lEnum.MoveNext();

                if (rEnum.Current != null)
                {
                    pLeft = lEnum.Current;
                    pRight = rEnum.Current;
                }
                else
                    break;
            }

              double leftPointX = pLeft.XThetaGraph;
              double rightPointX = pRight.XThetaGraph;
              double leftPointY = pLeft.YRGraph;
              double rightPointY = pRight.YRGraph;

              if (AdjustForLogScale(coord.xThetaScale, ref leftPointX) &&
                    AdjustForLogScale(coord.xThetaScale, ref rightPointX) &&
                    AdjustForLogScale(coord.xThetaScale, ref x) &&
                    AdjustForLogScale(coord.yRScale, ref leftPointY) &&
                    AdjustForLogScale(coord.yRScale, ref rightPointY))
              { 
                double denominator = rightPointX - leftPointX;
                if (denominator < NuGenDefaultSettings.GetInstance().DoubleMin)
                  yNew = Math.Round(leftPointX, yPrecision).ToString();
                else
                {
                  // if x value is between the points this is an interpolation (0<s<1), otherwise an extrapolation
                  double s = (x - leftPointX) / denominator;

                  // interpolate or extrapolate
                  double otherPoint = (1.0 - s) * leftPointY + s * rightPointY;

                  // adjust for log scale
                  if (coord.yRScale == Scale.Log)
                    otherPoint = Math.Pow((double) 10.0, otherPoint);
                    
                  yNew = Math.Round(otherPoint, yPrecision).ToString();
                }
              }

              return yNew;
        }
        
        // export this pointset with one x-y pair on each line. header and footer are handled elsewhere.
        // the x and y values will be exported with their respective numeric precisions
        public string ExportCurveAll(ExportSettings xport, int xPrecision, int yPrecision)
        {
            string rtn = "";

            // if this curve is a function, then skip points whose exported x value is the same
            // as the previous point so the output can be imported into any sql database that
            // requires unique x values
            string xLast = "", xNew = "", yNew = "";

            foreach (NuGenPoint p in points)
            {                
                xNew = Math.Round(p.XThetaGraph, xPrecision).ToString();
                yNew = Math.Round(p.YRGraph, yPrecision).ToString();

                string delim;

                switch (xport.delimiters)
                {
                    case ExportDelimiters.Commas:
                        delim = ","; break;
                    case ExportDelimiters.Spaces:
                        delim = " "; break;
                    case ExportDelimiters.Tabs:
                        delim = "\t"; break;
                    default:
                        delim = ""; break;
                }

                if ((xLast != xNew) ||
                  (style.lineConnectAs != LineConnectAs.SingleValuedFunction))
                {
                    rtn += xNew;
                    rtn += delim;
                    rtn += yNew;
                    rtn += "\n";
                }

                xLast = xNew;
            }

            return rtn;
        }

        // apply the transformation to set graph coordinates of the points in the curve and measure pointsets
        public void UpdateGraphCoordinates(CoordSettings coord, NuGenScreenTranslate transform)
        {
            foreach (NuGenPoint point in points)
            {
                double xTheta, yR;
                transform.ScreenToXThetaYR(coord, point.XScreen, point.YScreen, out xTheta, out yR);

                point.XThetaGraph = xTheta;
                point.YRGraph = yR;
            }
        }
        
        // return bounds on the coordinates for this pointset, and true if
        // at least one point was found
        public bool PointSetGraphLimits(ref double xThetaMin, ref double xThetaMax, ref double yRMin, ref double yRMax)
        {
            bool found = false;

            foreach (NuGenPoint point in points)
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

            return found;
        }

        // merge the x values of this pointset into a sorted list having unique x values. although
        // this returns numeric x values with full precision, their exported precision must be
        // specified so no adjacent exported x values will have the same value (breaks postprocessing
        // tools such as sql databases)
        public void MergeUniqueXValues(List<double> list, int xPrecision)
        {
            if (points.Count == 0)
            {
                return;
            }

            NuGenPoint p = points[0];
            int index = 0;

            for(int i = 0; i<list.Count; i++)
            {
                double x = list[i];
                for (; p.XThetaGraph < x && index < points.Count; index++)
                {
                    p = points[index];
                    list.Insert(list.IndexOf(x), p.XThetaGraph);
                }

                if (index >= points.Count)
                    break;
            }

            while (index < points.Count)
            {
                p = points[index++];
                list.Add(p.XThetaGraph);                
            }

            string xLast = "", xNew;

            double d = list[0];

            for (int i = 1; i < list.Count; )
            {
                xNew = String.Format("{0}", Math.Round(d, xPrecision).ToString());

                if (xLast.Equals(xNew))
                {
                    list.Remove(d);
                    d = list[i];
                }
                else
                {
                    d = list[i++];
                }

                xLast = xNew;
            }
        }

        // serialize
        public void SerializeWrite(System.Runtime.Serialization.SerializationInfo info, string prefix)
        {            
            info.AddValue(prefix + ".name", name);
              info.AddValue(prefix + ".style.pointShape", this.style.pointShape);
              info.AddValue(prefix + ".style.pointSize", this.style.pointSize);
              info.AddValue(prefix + ".style.pointLineSize", this.style.pointLineSize);
              info.AddValue(prefix + ".style.pointLineColor", this.style.pointLineColor);
              info.AddValue(prefix + ".style.pointInColor", this.style.pointInColor);
              info.AddValue(prefix + ".style.lineSize", this.style.lineSize);
              info.AddValue(prefix + ".style.lineColor", this.style.lineColor);
              info.AddValue(prefix + ".style.lineConnectAs", this.style.lineConnectAs);

              info.AddValue(prefix + ".points.count", this.Points.Count);

              int index = 0;
              foreach (NuGenPoint point in Points)
              {
                  info.AddValue(prefix + ".point" + index + ".xScreen", point.XScreen);
                  info.AddValue(prefix + ".point" + index + ".yScreen", point.YScreen);
                  info.AddValue(prefix + ".point" + index + ".xThetaGraph", point.XThetaGraph);
                  info.AddValue(prefix + ".point" + index + ".yRGraph", point.YRGraph);
                  index++;
              }
        }

        //deserialize
        public void SerializeRead(System.Runtime.Serialization.SerializationInfo info, string prefix)
        {
            name = (string)info.GetValue(prefix + ".name", typeof(string));
            style.pointShape = (PointShape)info.GetValue(prefix + ".style.pointShape", typeof(PointShape));
            style.pointSize = (PointSize)info.GetValue(prefix + ".style.pointSize", typeof(PointSize));
            style.pointLineSize = (PointLineSize)info.GetValue(prefix + ".style.pointLineSize", typeof(PointLineSize));
            style.pointLineColor = (Color)info.GetValue(prefix + ".style.pointLineColor", typeof(Color));
            style.pointInColor = (Color)info.GetValue(prefix + ".style.pointInColor", typeof(Color));
            style.lineSize = (LineSize)info.GetValue(prefix + ".style.lineSize", typeof(LineSize));
            style.lineColor = (Color)info.GetValue(prefix + ".style.lineColor", typeof(Color));
            style.lineConnectAs = (LineConnectAs)info.GetValue(prefix + ".style.lineConnectAs", typeof(LineConnectAs));

            int count = (int)info.GetValue(prefix + ".points.count", typeof(int));
            for(int index = 0; index<count; index++)
            {               
                NuGenPoint point = new NuGenPoint(0,0);
                point.XScreen = (int)info.GetValue(prefix + ".point" + index + ".xScreen", typeof(int));
                point.YScreen = (int)info.GetValue(prefix + ".point" + index + ".yScreen", typeof(int));
                point.XThetaGraph = (double)info.GetValue(prefix + ".point" + index + ".xThetaGraph", typeof(double));
                point.YRGraph = (double)info.GetValue(prefix + ".point" + index + ".yRGraph", typeof(double));
                AddPoint(point);
            }
        }

        // return the geometry info for this pointset
        public void GeometryInfo(bool validTransform, bool cartesian, ref int pNextRow,
          List<NuGenGeometryWindowItem> rInfo, LengthUnits units)
        {
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 0, "Name:"));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 1, name));

            pNextRow++;

            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 0, "Units:"));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 1, validTransform ? units.ToString() + "s" : "Pixels"));

            pNextRow++;

            GeometryInfoArea(ref pNextRow, rInfo);
            GeometryInfoDistance(ref pNextRow, rInfo, cartesian);
        }

        // for curve pointsets connected as Single Valued Functions, since the graph coordinates
        // of all points are updated, we must reconnect any points that were reordered. remember,
        // a single valued function has only a single value per xTheta value, so the lines cannot overlap
        public void ForceSingleValued(CoordSettings coord, NuGenScreenTranslate transform)
        {
            // quick exit if pointset is a closed contour, which is the case for all axis, scale and
            // measure pointsets. curve pointsets may be either single valued functions, or closed contours
            if (style.lineConnectAs != LineConnectAs.SingleValuedFunction)
                return;

            // quick exit if points are already in order
            if (SingleValued(coord, transform))
                return;

            Comparison<NuGenPoint> comparison = new Comparison<NuGenPoint>(this.XThetaSort);                

            // sort the points by xTheta
            points.Sort(comparison);

            // to prevent having to remove N lines and then immediately adding N lines, we only
            // adjust as many lines as necessary
            NuGenPoint pOld = points[points.Count - 1];
            NuGenPoint pOlder = null;

            foreach (NuGenPoint pNew in points)
            {
                if (pOlder != null)
                {
                    if ((pOlder.NextLine != pNew.PreviousLine) || (pNew.PreviousLine == null))
                    {
                        if (pOlder.NextLine == null)
                        {
                            pOlder.NextLine = pOld.NextLine;
                            pOld.NextLine = null;
                        }

                        pOlder.NextLine.start.X = pOlder.XScreen;
                        pOlder.NextLine.start.Y = pOlder.YScreen;
                        pOlder.NextLine.end.X = pNew.XScreen;
                        pOlder.NextLine.end.Y = pNew.YScreen;

                        pNew.PreviousLine = pOlder.NextLine;
                    }
                }
                else
                {
                    pNew.PreviousLine = null;
                }

                pOlder = pNew;
            }
        }

        //delegate method for sorting points by their xtheta values
        public int XThetaSort(NuGenPoint p1, NuGenPoint p2)
        {
            return (int)(p1.XThetaGraph - p2.XThetaGraph);
        }

        // convert an x or y coordinate to log scale if necessary
        public bool AdjustForLogScale(Scale scale, ref double pointXY)
        {
            if (scale == Scale.Log)
            {
                if (pointXY <= 0.0)
                    return false;

                pointXY = Math.Log10(pointXY);
            }

            return true;
        }

        // adding and removing lines involves updating update area and point pointers
        private void AddLine(NuGenPoint pFrom, NuGenPoint pTo)
        {
            Line line = new Line();

            line.start.X = pFrom.XScreen;
            line.start.Y = pFrom.YScreen;
            line.end.X = pTo.XScreen;
            line.end.Y = pTo.YScreen;
            
            lines.Add(line);

            pFrom.NextLine = line;
            pTo.PreviousLine = line;
        }

        private void RemoveLine(NuGenPoint pFrom, NuGenPoint pTo)
        {
            lines.Remove(pFrom.NextLine);

            pFrom.NextLine = null;
            pTo.PreviousLine = null;
        }

        // true if pointset is single valued
        private bool SingleValued(CoordSettings coord, NuGenScreenTranslate transform)
        {
            bool first = true;
            double xThetaLast = 0.0, xTheta, yR;
            foreach(NuGenPoint point in points)
            {
                transform.ScreenToXThetaYR(coord, point.XScreen, point.YScreen, out xTheta, out yR);

                if (!first && (xTheta < xThetaLast))
                    return false;

                xThetaLast = xTheta;
                first = false;
            }

            return true;
        }

        // return the area and distance geometry info for this pointset
        private void GeometryInfoArea(ref int pNextRow, List<NuGenGeometryWindowItem> rInfo)
        {
            double funcArea = 0.0, polyArea = 0.0;
            int i =0, nPoints = points.Count;
            if (nPoints > 2)
            {
                double[] x = new double[nPoints];
                double[] y = new double[nPoints];

                foreach (NuGenPoint point in points)
                {
                    x[i] = point.XThetaGraph;
                    y[i] = point.YRGraph;
                    i++;
                }

                funcArea = NuGenMath.FunctionArea(nPoints, x, y);
                polyArea = NuGenMath.PolygonArea(nPoints, x, y);
            }

            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 0, "FuncArea"));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 1, funcArea.ToString()));

            pNextRow++;

            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 0, "PolyArea"));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 1, polyArea.ToString()));

            pNextRow++;
        }

        //Gets the distance along the curve of each point
        private void GeometryInfoDistance(ref int pNextRow, List<NuGenGeometryWindowItem> rInfo, bool cartesian)
        {
            bool firstPoint;
            double xLast = 0.0, yLast = 0.0, x, y;
            double totalDistance = 0.0, distance;

            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 0, cartesian ? "X" : "Theta"));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 1, cartesian ? "Y" : "R"));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 2, "Index"));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 3, "Distance"));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 4, "Percent"));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 5, "Distance"));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 6, "Percent"));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 7, "Angle"));

            pNextRow++;

            // first pass computes totalDistance and xLast and yLast, second pass outputs one line per point
            for (int pass = 0; pass < 2; pass++)
            {
                firstPoint = true;
                distance = 0.0;
                NuGenPoint point;

                for (int i = 0; i < points.Count; i++)
                {
                    point = points[i];

                    x = point.XThetaGraph;
                    y = point.YRGraph;

                    if (!firstPoint)
                        distance += Math.Sqrt((x - xLast) * (x - xLast) + (y - yLast) * (y - yLast));

                    NuGenPoint next = (i + 1) != points.Count ? points[i + 1] : points[0];

                    if (pass == 1)
                        GeometryInfoDistancePass1(i, next, xLast, yLast, x, y, distance, totalDistance,
                          ref pNextRow, rInfo);

                    firstPoint = false;
                    xLast = x;
                    yLast = y;
                }

                totalDistance = distance;
            }
        }

        private void GeometryInfoDistancePass1(int i, NuGenPoint pointNext,
          double xLast, double yLast, double x, double y,
          double distance, double totalDistance, ref int pNextRow, List<NuGenGeometryWindowItem> rInfo)
        {

            double xNext = pointNext.XThetaGraph;
            double yNext = pointNext.YRGraph;

            double pcDistance;
            if (totalDistance <= 0.0)
                pcDistance = 100.0;
            else
                pcDistance = 100.0 * distance / totalDistance;

            double[] r1 = new double[3];
            double[] r2 = new double[3];

            r1[0] = xLast - x; // xLast from last point of pass 0 is used by first point in pass 1
            r1[1] = yLast - y; // yLast from last point of pass 0 is used by first point in pass 1
            r1[2] = 0.0;
            r2[0] = xNext - x;
            r2[1] = yNext - y;
            r2[2] = 0.0;
            
            double angle = NuGenMath.Angle(r1, r2) * 180.0 / Math.PI;

            double distDiff = totalDistance - distance;
            double pcDiff = 100.0 - pcDistance;

            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 0, x.ToString()));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 1, y.ToString()));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 2, i.ToString()));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 3, distance.ToString()));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 4, pcDistance.ToString()));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 5, distDiff.ToString()));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 6, pcDiff.ToString()));
            rInfo.Add(new NuGenGeometryWindowItem(pNextRow, 7, angle.ToString()));

            pNextRow++;
        }
    }
}
