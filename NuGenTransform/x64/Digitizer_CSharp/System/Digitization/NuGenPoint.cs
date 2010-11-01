using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Genetibase.NuGenTransform
{
    public class Line
    {
        public Point start;
        public Point end;
    }

    //A single point in any pointset in the application
    public class NuGenPoint
    {
        private int xScreen;
        private int yScreen;
        private double xThetaGraph;
        private double yRGraph;

        private NuGenPointSet pointSet;
        private Line linePrev;
        private Line lineNext;
        private bool xThetaDefined;
        private bool yRDefined;

        public NuGenPoint(int xScreen, int yScreen, double xThetaGraph, double yRGraph)
        {
            this.xScreen = xScreen;
            this.yScreen = yScreen;
            this.xThetaGraph = xThetaGraph;
            this.yRGraph = yRGraph;
            xThetaDefined = true;
            yRDefined = true;
            pointSet = new NuGenPointSet();
        }

        public NuGenPoint(NuGenPoint other)
        {
            this.xScreen = other.xScreen;
            this.yScreen = other.yScreen;
            xThetaGraph = other.XThetaGraph;
            yRGraph = other.yRGraph;
            xThetaDefined = other.xThetaDefined;
            yRDefined = other.yRDefined;
            pointSet = new NuGenPointSet();
        }

        public NuGenPoint(int xScreen, int yScreen)
        {
            this.xScreen = xScreen;
            this.yScreen = yScreen;
            xThetaGraph = 0.0;
            yRGraph = 0.0;
            xThetaDefined = false;
            yRDefined = false;
            pointSet = new NuGenPointSet();
        }

        public NuGenPointSet PointSet
        {
            get
            {
                return pointSet;
            }
            set
            {
                pointSet = value;
            }
        }

        public int XScreen
        {
            get
            {
                return xScreen;
            }

            set
            {
                xScreen = value;
            }
        }

        public int YScreen
        {
            get
            {
                return yScreen;
            }

            set
            {
                yScreen = value;
            }
        }

        public double XThetaGraph
        {
            get
            {
                return xThetaGraph;
            }

            set
            {
                xThetaGraph = value;
                xThetaDefined = true;
            }
        }

        public double YRGraph
        {
            get
            {
                return yRGraph;
            }

            set
            {
                yRGraph = value;
                yRDefined = true;
            }
        }

        public Line NextLine
        {
            get
            {
                return lineNext;
            }
            set
            {
                lineNext = value;
            }
        }

        public Line PreviousLine
        {
            get
            {
                return linePrev;
            }

            set
            {
                linePrev = value;
            }
        }



        public bool GraphCoordsAreDefined()
        {
            return xThetaDefined && yRDefined;
        }

        //Tells whether this point intersects another point within
        // the specified tolerance
        public bool Intersects(int x, int y, double p)
        {
            if (Math.Abs(xScreen - x) < p && Math.Abs(yScreen - y) < p)
                return true;

            return false;
        }
    }
}
