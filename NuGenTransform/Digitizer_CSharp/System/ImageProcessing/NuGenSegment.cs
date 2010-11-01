using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Genetibase.NuGenTransform
{
    /*
     * A single line segment within a scanned document     
     * */
    public class NuGenSegment
    {
        private double length;
        private int yLast;

        private List<SegmentLine> lines = new List<SegmentLine>();

        public NuGenSegment(int yLast)
        {
            this.yLast = yLast;
            length = 0.0;
        }

        public double Length
        {
            get
            {
                return length;
            }
        }

        public void AppendColumn(int x, int y, SegmentSettings seg)
        {
            SegmentLine line = new SegmentLine(this);
            line.StartPoint = new Point(x - 1, yLast);
            line.EndPoint = new Point(x, y);

            // do not show this line or its segment. this is handled later
            lines.Add(line);

            // update total length using distance formula
            length += Math.Sqrt((1.0) * (1.0) + (y - yLast) * (y - yLast));

            yLast = y;
        }

        void CreateAcceptablePoint(ref bool pFirst, List<NuGenPoint> pList,
          ref double xPrev, ref double yPrev, double x, double y)
        {
            int iOld = (int)(xPrev + 0.5);
            int jOld = (int)(yPrev + 0.5);
            int i = (int)(x + 0.5);
            int j = (int)(y + 0.5);

            if (pFirst || (iOld != i) || (jOld != j))
            {
                xPrev = x;
                yPrev = y;

                pList.Add(new NuGenPoint(i, j));
            }

            pFirst = false;
        }

        public void RemoveUnneededLines()
        {
            // pathological case is y=0.001*x*x, since the small slope can fool a naive algorithm
            // into optimizing away all but one point at the origin and another point at the far right.
            // from this we see that we cannot simply throw away points that were optimized away since they
            // are needed later to see if we have diverged from the curve
            SegmentLine lineOlder = null;
            List<Point> removedPoints = new List<Point>();

            foreach (SegmentLine line in lines)
            {
                if (lineOlder != null)
                {
                    double xLeft = lineOlder.StartPoint.X;
                    double yLeft = lineOlder.StartPoint.Y;
                    double xInt = lineOlder.StartPoint.X;
                    double yInt = lineOlder.StartPoint.Y;

                    double xRight = line.EndPoint.X;
                    double yRight = line.EndPoint.Y;

                    if (PointIsCloseToLine(xLeft, yLeft, xInt, yInt, xRight, yRight) &&
                    (PointsAreCloseToLine(xLeft, yLeft, removedPoints, xRight, yRight)))
                    {
                        // remove intermediate point, by removing older line and stretching new line to first point
                        removedPoints.Add(new Point((int)(xInt + 0.5), (int)(yInt + 0.5)));

                        lines.Remove(lineOlder);
                        line.StartPoint = new Point((int)(xLeft + 0.5), (int)(yLeft + 0.5));
                        line.EndPoint = new Point((int)(xRight + 0.5), (int)(yRight + 0.5));
                    }
                    else
                    {
                        //keeping this intermediate point and clear out the removed points list
                        removedPoints.Clear();
                    }
                }

                lineOlder = line;
            }
        }

        bool PointIsCloseToLine(double xLeft, double yLeft, double xInt, double yInt,
          double xRight, double yRight)
        {
            double xProj, yProj;
            NuGenMath.ProjectPointOnToLine(xInt, yInt, xLeft, yLeft, xRight, yRight, out xProj, out yProj);

            return (
              (xInt - xProj) * (xInt - xProj) +
              (yInt - yProj) * (yInt - yProj) < 0.5 * 0.5);
        }

        bool PointsAreCloseToLine(double xLeft, double yLeft, List<Point> removedPoints,
          double xRight, double yRight)
        {
            foreach (Point p in removedPoints)
            {
                if (!PointIsCloseToLine(xLeft, yLeft, p.X, p.Y, xRight, yRight))
                {
                    return true;
                }
            }

            return false;
        }

        public List<SegmentLine> Lines
        {
            get
            {
                return lines;
            }
        }

        public List<NuGenPoint> FillPoints(SegmentSettings seg)
        {
            if (seg.fillCorners)
                return FillPointsFillingCorners(seg);
            else
                return FillPointsWithoutFillingCorners(seg);
        }

        List<NuGenPoint> FillPointsFillingCorners(SegmentSettings seg)
        {
            List<NuGenPoint> list = new List<NuGenPoint>();

            if (lines.Count > 0)
            {
                double xLast = lines[0].StartPoint.X;
                double yLast = lines[0].StartPoint.Y;
                double x, y;

                // variables for createAcceptablePoint
                double xPrev = lines[0].StartPoint.X;
                double yPrev = lines[0].StartPoint.Y;

                bool firstPointOfLineSegment;

                foreach (SegmentLine line in lines)
                {
                    firstPointOfLineSegment = true;

                    double xNext = line.EndPoint.X;
                    double yNext = line.EndPoint.Y;

                    //good ol pythagorus.. or however you spell it
                    double segmentLength = Math.Sqrt((xNext - xLast) * (xNext - xLast) + (yNext - yLast) * (yNext - yLast));

                    double distanceLeft = segmentLength;
                    double s = 0.0;

                    do
                    {
                        //coordinates of new point
                        x = (1.0 - s) * xLast + s * xNext;
                        y = (1.0 - s) * yLast + s * yNext;

                        CreateAcceptablePoint(ref firstPointOfLineSegment, list, ref xPrev, ref yPrev, x, y);

                        distanceLeft -= seg.pointSeparation;

                        s += seg.pointSeparation / segmentLength;
                    } while (distanceLeft >= seg.pointSeparation);

                    xLast = xNext;
                    yLast = yNext;
                }

                firstPointOfLineSegment = true;
                CreateAcceptablePoint(ref firstPointOfLineSegment, list, ref xPrev, ref yPrev, xLast, yLast);
            }

            return list;
        }

        List<NuGenPoint> FillPointsWithoutFillingCorners(SegmentSettings seg)
        {
            List<NuGenPoint> list = new List<NuGenPoint>();

            if (lines.Count > 0)
            {
                double xLast = lines[0].StartPoint.X;
                double yLast = lines[0].StartPoint.Y;
                double x, xNext;
                double y, yNext;
                double distanceCompleted = 0.0;

                // variables for createAcceptablePoint
                bool firstPoint = true;
                double xPrev = lines[0].StartPoint.X;
                double yPrev = lines[0].StartPoint.Y;

                foreach (SegmentLine line in lines)
                {
                    xNext = line.EndPoint.X;
                    yNext = line.EndPoint.Y;

                    // distance formula
                    double segmentLength = Math.Sqrt((xNext - xLast) * (xNext - xLast) + (yNext - yLast) * (yNext - yLast));
                    if (segmentLength > 0.0)
                    {
                        while (distanceCompleted <= segmentLength)
                        {
                            double s = distanceCompleted / segmentLength;

                            x = (1.0 - s) * xLast + s * xNext;
                            y = (1.0 - s) * yLast + s * yNext;

                            CreateAcceptablePoint(ref firstPoint, list, ref xPrev, ref yPrev, x, y);

                            distanceCompleted += seg.pointSeparation;
                        }

                        distanceCompleted -= segmentLength;
                    }

                    xLast = xNext;
                    yLast = yNext;
                }
            }

            return list;
        }
    }

    //This class encapsulates the points of a segment and creates
    // a convienient way for it to be drawn to the screen by giving it point values
    public class SegmentLine
    {
        private NuGenSegment segment;
        private Point startPoint;
        private Point endPoint;

        public SegmentLine(NuGenSegment segment)
        {
            this.segment = segment;
        }

        public NuGenSegment Segment
        {
            get
            {
                return segment;
            }
        }

        public Point StartPoint
        {
            get
            {
                return startPoint;
            }

            set
            {
                startPoint = value;
            }
        }

        public Point EndPoint
        {
            get
            {
                return endPoint;
            }
            set
            {
                endPoint = value;
            }
        }

        //Tells if a screen point intersects this segment within a particular
        // tolerence.  Allows segments to be highlighting by use mouse input.
        public bool Intersects(int x, int y, double tolerance)
        {
            Point p1 = endPoint.X < startPoint.X ? endPoint : startPoint;
            Point p2 = p1 == endPoint ? startPoint : endPoint;

            double difference = 0.0;

            if (p1.X == p2.X || p1.Y == p2.Y)
            {
                difference = (Math.Abs(p1.X - x) + Math.Abs(p1.Y - y)) / 2;
            }
            else
            {
                double slope = (p2.Y - p1.Y) / (p2.X - p1.X);

                double yLine = slope * (p2.X - x) + p1.Y;
                double yDifference = Math.Abs(yLine - y);

                double xLine = (y - p1.Y) / slope + p1.X;
                double xDifference = Math.Abs(xLine - x);

                difference = (yDifference + xDifference) / 2;
            }

            if (difference <= tolerance)
            {
                return true;
            }

            return false;
        }
    }
}
