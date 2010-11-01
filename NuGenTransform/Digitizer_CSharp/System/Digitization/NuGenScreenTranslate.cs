using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Genetibase.NuGenTransform
{
    //Encapsulates operations for translating screen coordinates to graph coordinates
    public class NuGenScreenTranslate
    {

        private bool validAxes;
        private bool validScale;

        private double aX;
        private double aY;
        private double bX;
        private double bY;

        private int axisPointCount;
        private int scalePointCount;

         // matrix to convert vector from screen coordinates to graph coordinates
        private double[,] screenToGraph = new double[3,3];

        // matrix to convert vector from graph coordinates to screen coordinates
        private double [,] graphToScreen = new double[3,3];

        private NuGenDocument doc;

        public NuGenScreenTranslate(NuGenDocument doc)
        {
            this.doc = doc;
        }

        public bool ValidAxes
        {
            get
            {
                return validAxes;
            }

            set
            {
                validAxes = value;
            }
        }

        public bool ValidScale
        {
            get
            {
                return validScale;
            }

            set
            {
                validScale = value;
            }
        }

        //Converts a graph Coordinate to screen coordinate
        public void XThetaYRToScreen(CoordSettings coordSettings, double xTheta, double yR, out int xScreen, out int yScreen)
        {
            double xScreenD, yScreenD;
            XThetaYRToScreen(coordSettings, xTheta, yR, out xScreenD, out yScreenD);

            xScreen = (int)(xScreenD + 0.5);
            yScreen = (int)(yScreenD + 0.5);
        }
        
        public void XThetaYRToScreen(CoordSettings coordSettings, double xTheta, double yR, out double xScreen, out double yScreen)
        {            
            if (ValidAxes || ValidScale)
            {
                LinearToLog(coordSettings, ref xTheta, ref yR);

                double xGraph, yGraph;
                ConvertToCartesian(NuGenMath.mmUnitize(coordSettings), xTheta, yR, out xGraph, out yGraph);

                GraphToScreen(xGraph, yGraph, out xScreen, out yScreen);
            }
            else
            {
                xScreen = xTheta;
                yScreen = yR;
            }
        }

        //Converts a screen coordinate pair to a graph coordinate pair
        public void ScreenToXThetaYR(CoordSettings coordSettings, double xScreen, double yScreen, out double xTheta, out double yR)
        {
            if (ValidAxes || ValidScale)
            {
                double xGraph, yGraph;
                ScreenToGraph(xScreen, yScreen, out xGraph, out yGraph);

                ConvertFromCartesian(NuGenMath.mmUnitize(coordSettings), xGraph, yGraph, out xTheta, out yR);

                LogToLinear(coordSettings, ref xTheta, ref yR);
            }
            else
            {
                // until transform is defined just use identity matrix. this branch is only defined when trying
                // to connect points with lines, and the transform is not currently defined
                xTheta = xScreen;
                yR = yScreen;
            }
        }

        //Converts a graph coordinate pair to cartesian from polar coordinates
        private void ConvertToCartesian(MMUnits units, double xTheta, double yR, out double x, out double y)
        {
            double angleTemp;
            if (units == MMUnits.mmCartesian)
            {
                x = xTheta;
                y = yR;
            }
            else
            {
                angleTemp = xTheta;
                if (units == MMUnits.mmDegrees)
                    angleTemp *= Math.PI / 180.0;
                else if (units == MMUnits.mmGradians)
                    angleTemp *= Math.PI / 200.0;
                x = yR * Math.Cos(angleTemp);
                y = yR * Math.Sin(angleTemp);
            }
        }

        //Converts a cartesian coordinate pair to polar
        private void ConvertFromCartesian(MMUnits units, double x, double y, out double xTheta, out double yR)
        {
            double radiusTemp;
            if (units == MMUnits.mmCartesian)
            {
                xTheta = x;
                yR = y;
            }
            else
            {
                radiusTemp = Math.Sqrt(x * x + y * y);
                xTheta = Math.Atan2(y, x);
                if (units == MMUnits.mmDegrees)
                    xTheta *= 180.0 / Math.PI;
                else if (units == MMUnits.mmGradians)
                    xTheta *= 200.0 / Math.PI;
                yR = radiusTemp;
            }
        }

        //Converts a pair of graph coordinates to screen coordinates
        private void GraphToScreen(double xGraph, double yGraph, out double xScreen, out double yScreen)
        {
              if(!(ValidAxes || ValidScale))
                  throw new InvalidOperationException("No valid axes or scale");

            double[] rScreen = new double[3];
            double[] rGraph = new double[3];

            rGraph[0] = xGraph;
            rGraph[1] = yGraph;
            rGraph[2] = 1.0;

            NuGenMath.MatrixMultiply3x1(rScreen, graphToScreen, rGraph);

            xScreen = rScreen[0];
            yScreen = rScreen[1];
        }

        //Converts a pair of screen coordinates to graph coordinates
        private void ScreenToGraph(double xScreen, double yScreen, out double xGraph, out double yGraph)
        {
            double[] rScreen = new double[3];
            double[] rGraph = new double[3];

            rScreen[0] = xScreen;
            rScreen[1] = yScreen;
            rScreen[2] = 1.0;

            NuGenMath.MatrixMultiply3x1(rGraph, screenToGraph, rScreen);

            xGraph = rGraph[0];
            yGraph = rGraph[1];            
        }

        //Converts a pair of graph coordinates to log scale
        private void LinearToLog(CoordSettings coordSettings, ref double xTheta, ref double yR)
        {
            if (coordSettings.xThetaScale == Scale.Log)
            {
                if(aY == 0 || bY == 0)
                    throw new InvalidOperationException("Values were equal to zero");
                xTheta = Math.Log(xTheta / aX) / bX;
            }
            if (coordSettings.yRScale == Scale.Log)
            {
                if(aY == 0 || bY == 0)
                    throw new InvalidOperationException("Values were equal to zero");
                yR = Math.Log(yR / aY) / bY;
            }
        }

        //Converts a pair of log coordinates to linear scale
        private void LogToLinear(CoordSettings coordSettings, ref double xTheta, ref double yR)
        {
            if(coordSettings.xThetaScale == Scale.Log)
                xTheta = aX * Math.Exp(bX * xTheta);
            if(coordSettings.yRScale == Scale.Log)
                yR = aY * Math.Exp(bY * yR);
        }

        public void XBasisScreen(CoordSettings coordSettings, out double xScreen, out double yScreen)
        {
            // return basis vector in x direction. do not look at screen increment
            // resulting from graph increment of (0,0) to (1,0), since log scale may
            // be in effect which would crash with a zero
            double xBeforeS, yBeforeS;
            double xAfterS, yAfterS;
            XThetaYRToScreen(coordSettings, 1.0, 1.0, out xBeforeS, out yBeforeS);
            XThetaYRToScreen(coordSettings, 10.0, 1.0, out xAfterS, out yAfterS);
            xScreen = xAfterS - xBeforeS;
            yScreen = yAfterS - yBeforeS;
        }

        public void YBasisScreen(CoordSettings coordSettings, out double xScreen, out double yScreen)
        {
            // return basis vector in x direction. do not look at screen increment
            // resulting from graph increment of (0,0) to (1,0), since log scale may
            // be in effect which would crash with a zero
            double xBeforeS, yBeforeS;
            double xAfterS, yAfterS;
            XThetaYRToScreen(coordSettings, 1.0, 1.0, out xBeforeS, out yBeforeS);
            XThetaYRToScreen(coordSettings, 1.0, 10.0, out xAfterS, out yAfterS);
            xScreen = xAfterS - xBeforeS;
            yScreen = yAfterS - yBeforeS;
        }

        //Computes the graph area based on the defined axis points, returns a value corresponding
        // to the result of the calculation
        public int ComputeAxesTransformation(CoordSettings coordSettings, NuGenPointSet pointSet)
        {
            double[][] r_graph = { new double[3], new double[3], new double[3] };
            double[,] r_screen = new double[3,3];

            List<NuGenPoint> points = pointSet.Points;

            axisPointCount = 0;
            validAxes = false;

            foreach(NuGenPoint p in points)
            {
                if(axisPointCount == 3)
                    throw new InvalidOperationException("We have three axis points, can not compute more");

                if(p.GraphCoordsAreDefined())
                {
                    double xGraph, yGraph;
                    ConvertToCartesian(NuGenMath.mmUnitize(coordSettings), p.XThetaGraph, p.YRGraph, out xGraph, out yGraph);

                    r_graph[0][axisPointCount] = xGraph;
                    r_graph[1][axisPointCount] = yGraph;
                    r_graph[2][axisPointCount] = 1.0;

                    r_screen[0, axisPointCount] = p.XScreen;
                    r_screen[1, axisPointCount] = p.YScreen;
                    r_screen[2, axisPointCount] = 1.0;

                    axisPointCount++;
                }
            }

            if(axisPointCount == 3)
            {
                AdjustMidValuedLogCoords(coordSettings, r_graph);

                int rtnS2G = NuGenMath.ScreenToGraph(r_graph, r_screen, screenToGraph, graphToScreen);                

                validAxes = false;

                if(rtnS2G == NuGenMath.SUCCESS)
                {
                    int rtnL2L = ComputeLogToFromLinear(coordSettings, r_graph);

                    switch(rtnL2L)
                    {
                        case NuGenMath.SUCCESS:
                            break;
                        case NuGenMath.NONPOSITIVE_COORDINATE:
                            return NuGenMath.NONPOSITIVE_COORDINATE;
                        case NuGenMath.NO_SPREAD:
                            return NuGenMath.NO_SPREAD;
                    }
                }

                validAxes = (rtnS2G == NuGenMath.SUCCESS);

                switch (rtnS2G)
                {
                    case NuGenMath.SUCCESS:
                        return NuGenMath.SUCCESS;
                    case NuGenMath.BAD_GRAPH_COORDINATES:
                        return NuGenMath.BAD_GRAPH_COORDINATES;
                    case NuGenMath.BAD_SCREEN_COORDINATES:
                        return NuGenMath.BAD_SCREEN_COORDINATES;
                }
            }

            return NuGenMath.SUCCESS;
        }

        private void AdjustMidValuedLogCoords(CoordSettings coordSettings, double[][] r_graph)
        {
            // this function does nothing for linear coordinates. for log coordinates, this
            // function adjusts the middle coordinate since compute_screen_to_graph assumes
            // the coordinates are linear. if the middle log coordinate does not equal the
            // min or max log coordinates, an undesired skew will appear. if two of the axis
            // points share the same log coordinate, the skew will not appear.
            //
            // the only function called after this function that accesses the middle log coordinate
            // will be compute_screen_to_graph
            AdjustMidValuedLogCoord(coordSettings.xThetaScale, ref r_graph[0][0], ref r_graph[0][1], ref r_graph[0][2]);
            AdjustMidValuedLogCoord(coordSettings.yRScale, ref r_graph[1][0], ref r_graph[1][1], ref r_graph[1][2]);
        }

        private void AdjustMidValuedLogCoord(Scale scale, ref double c0, ref double c1, ref double c2)
        {
            if (scale == Scale.Log)
            {
                double cMin = c0;
                double cMax = cMin;
                if (c1 < cMin)
                    cMin = c1;
                if (c1 > cMax)
                    cMax = c1;
                if (c2 < cMin)
                    cMin = c2;
                if (c2 > cMax)
                    cMax = c2;
                if ((cMin < c0) && (c0 < cMax))
                    c0 = cMin + (cMax - cMin) * (Math.Log(c0) - Math.Log(cMin)) / (Math.Log(cMax) - Math.Log(cMin));
                if ((cMin < c1) && (c1 < cMax))
                    c1 = cMin + (cMax - cMin) * (Math.Log(c1) - Math.Log(cMin)) / (Math.Log(cMax) - Math.Log(cMin));
                if ((cMin < c2) && (c2 < cMax))
                    c2 = cMin + (cMax - cMin) * (Math.Log(c2) - Math.Log(cMin)) / (Math.Log(cMax) - Math.Log(cMin));
            }
        }

        //Computes the ax ay bx by values needed in logarithmic to linear conversions
        public int ComputeLogToFromLinear(CoordSettings coordSettings, double[][] r_graph)
        {
            int retVal = ComputeLogToFromLinear(coordSettings.xThetaScale, r_graph[0], out aX, out bX);

            if (retVal != NuGenMath.SUCCESS)
                return retVal;            

            return ComputeLogToFromLinear(coordSettings.yRScale, r_graph[1], out aY, out bY);
        }

        private int ComputeLogToFromLinear(Scale scale, double[] r, out double a, out double b)
        {
            a = 0.0; b = 0.0;
            if (scale == Scale.Log)
            {
                bool first = true;
                double xyMin = 0.0, xyMax = 0.0;
                for (int i = 0; i < 3; i++)
                {
                    if (first || (r[i] < xyMin))
                        xyMin = r[i];
                    if (first || (r[i] > xyMax))
                        xyMax = r[i];
                    first = false;
                }

                if (xyMin == xyMax)
                    return NuGenMath.NO_SPREAD;
                if (xyMin <= 0.0)
                    return NuGenMath.NONPOSITIVE_COORDINATE;

                b = Math.Log(xyMax / xyMin) / (xyMax - xyMin);
                a = xyMin * Math.Exp(-(b) * xyMin);
            }

            return NuGenMath.SUCCESS;
        }

        //Computes the graph area based on the scale bar definition
        public int ComputeScaleTransformation(CoordSettings coordSettings, NuGenPointSet pointSet)
        {
            double[][] r_graph = { new double[3], new double[3], new double[3] };
            double[,] r_screen = new double[3, 3];

            List<NuGenPoint> points = pointSet.Points;

            scalePointCount = 0;
            validScale = false;

            foreach (NuGenPoint point in points)
            {
                if (scalePointCount == 2)
                    throw new InvalidOperationException("Can not define more scale bar points");

                if (point.GraphCoordsAreDefined())
                {
                    double xGraph, yGraph;
                    ConvertToCartesian(NuGenMath.mmUnitize(coordSettings), point.XThetaGraph, point.YRGraph,
                        out xGraph, out yGraph);

                    r_graph[0][scalePointCount] = xGraph;
                    r_graph[1][scalePointCount] = yGraph;
                    r_graph[2][scalePointCount] = 1.0;

                    r_screen[0, scalePointCount] = point.XScreen;
                    r_screen[1, scalePointCount] = point.YScreen;
                    r_screen[2, scalePointCount] = 1.0;

                    scalePointCount++;
                }
            }

            if (scalePointCount == 2)
            {
                // create virtual third point along a virtual axis which is just orthogonal to the virtual
                // axis between the other two points. assumes all points are in same z-value (which is 1.0) plane
                double[] axis1 = new double[3];
                double[] axis2 = new double[3];
                double[] z = { 0.0, 0.0, 1.0 };

                axis1[0] = r_graph[0][1] - r_graph[0][0];
                axis1[1] = r_graph[1][1] - r_graph[1][0];
                axis1[2] = r_graph[2][1] - r_graph[2][0];

                NuGenMath.VectorCrossProduct(z, axis1, axis2);

                r_graph[0][2] = r_graph[0][0] + axis2[0];
                r_graph[1][2] = r_graph[1][0] + axis2[1];
                r_graph[2][2] = 1.0;

                axis1[0] = r_screen[0, 1] - r_screen[0, 0];
                axis1[1] = r_screen[1, 1] - r_screen[1, 0];
                axis1[2] = r_screen[2, 1] - r_screen[2, 0];

                NuGenMath.VectorCrossProduct(z, axis1, axis2);

                r_screen[0, 2] = r_screen[0, 0] + axis2[0];
                r_screen[1, 2] = r_screen[1, 0] + axis2[1];
                r_screen[2, 2] = 1.0;

                int rtnS2G = NuGenMath.ScreenToGraph(r_graph, r_screen,
                  screenToGraph, graphToScreen);

                // log to linear transformation is not allowed when using scale bar since it would
                // be such an extremely rare special case that its benefit would be minimal, especially
                // since it would only confuse the majority of users who would not be expecting it

                validScale = (rtnS2G == NuGenMath.SUCCESS);

                switch (rtnS2G)
                {
                    case NuGenMath.SUCCESS:
                        return NuGenMath.SUCCESS;                        
                    case NuGenMath.BAD_GRAPH_COORDINATES:
                        return NuGenMath.BAD_GRAPH_COORDINATES;                  
                    case NuGenMath.BAD_SCREEN_COORDINATES:
                        return NuGenMath.BAD_SCREEN_COORDINATES;                   
                }
            }

            return NuGenMath.SUCCESS;
        }
    }
}
