using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Genetibase.NuGenTransform
{
    public struct GridlineScreen
    {
        public PointValue Start;
        public PointValue Stop;

        // true if line represents a radial line
        public bool R;
    };

    public struct PointValue
    {
        public int X;
        public int Y;
    }

    //Represents a mesh which is comprised of relatively vertical and horizontal lines
    // which make up a coordinate system.
    //
    // Consists of a series of lines (GridlineScreen) which can be cartesion or polar gridlines
    class NuGenGridMesh
    {
        const double pixelSpacing = 4;

        const double angleSpacingDeg = 1.0; // total # canvas lines is 360*#grids/spacing
        const double angleSpacingGrad = angleSpacingDeg * 400.0 / 360.0;
        const double angleSpacingRad = angleSpacingDeg * 3.1415926535 / 180.0;

        //Makes gridlines from the given translation
        public static List<GridlineScreen> MakeGridLines(NuGenScreenTranslate transform, CoordSettings coordSettings, GridMeshSettings gridMeshSettings)
        {
            List<GridlineScreen> list = new List<GridlineScreen>();
            if (transform.ValidAxes)
            {
                if (coordSettings.frame == ReferenceFrame.Cartesian)
                {
                    list.AddRange(MakeGridX(transform, coordSettings, gridMeshSettings));
                    list.AddRange(MakeGridY(transform, coordSettings, gridMeshSettings));
                    return list;
                }
                else
                {
                    list.AddRange(MakeGridTheta(transform, coordSettings, gridMeshSettings));
                    list.AddRange(MakeGridR(transform, coordSettings, gridMeshSettings));
                    return list;
                }
            }

            return list;
        }

        private static List<GridlineScreen> MakeGridTheta(NuGenScreenTranslate transform, CoordSettings coordSettings, GridMeshSettings gridMeshSettings)
        {
            int i;
            int xStartS, yStartS, xStopS, yStopS;
            double thetaG = gridMeshSettings.startX;
            GridlineScreen gridline;
            List<GridlineScreen> gridlines = new List<GridlineScreen>();
            for (i = 0; i < gridMeshSettings.countX; i++)
            {
                transform.XThetaYRToScreen(coordSettings, thetaG, gridMeshSettings.startY, out xStartS, out yStartS);
                transform.XThetaYRToScreen(coordSettings, thetaG, gridMeshSettings.stopY, out xStopS, out yStopS);

                gridline.Start.X = xStartS;
                gridline.Start.Y = yStartS;
                gridline.Stop.X = xStopS;
                gridline.Stop.Y = yStopS;
                gridline.R = false;

                thetaG += gridMeshSettings.stepX;
                gridlines.Add(gridline); //Maybe not?
            }

            return gridlines;
        }

        private static List<GridlineScreen> MakeGridR(NuGenScreenTranslate transform, CoordSettings coordSettings, GridMeshSettings gridMeshSettings)
        {
            int i;
            int xStartS = 0, yStartS = 0, xStopS = 1, yStopS = 1;
            double rG = gridMeshSettings.startY;
            GridlineScreen gridline;
            List<GridlineScreen> gridlines = new List<GridlineScreen>();
            for (i = 0; i < gridMeshSettings.countY; i++)
            {
                gridline = new GridlineScreen();
                // for polar coordinates we simply piecewise define the elliptical arc until motivated
                // to implement a better drawing algorithm. segments will be evenly spaced in angle
                // some pdf documents describing alternative algorithms are found in the doc directory.
                // it would have been elegant to use QCanvasEllipses but those are axis-aligned.
                double delta = AngleSpacing(coordSettings.thetaUnits);
                bool first = true;
                for (double angle = gridMeshSettings.startX; angle < gridMeshSettings.stopX; angle += delta)
                {
                    transform.XThetaYRToScreen(coordSettings, angle, rG, out xStopS, out yStopS);

                    if (first)
                    {
                        xStartS = xStopS;
                        yStartS = yStopS;
                    }
                    else
                    {
                        if (NuGenMath.VectorMagnitude(xStopS - xStartS, yStopS - yStartS, 0.0) >= pixelSpacing)
                        {
                            gridline.Start.X = xStartS;
                            gridline.Start.Y = yStartS;
                            gridline.Stop.X = xStopS;
                            gridline.Stop.Y = yStopS;
                            gridline.R = true;

                            xStartS = xStopS;
                            yStartS = yStopS;
                        }
                    }

                    first = false;
                }

                if (coordSettings.yRScale == Scale.Linear)
                    rG += gridMeshSettings.stepY;
                else
                    rG *= gridMeshSettings.stepY;
            }

            return gridlines;
        }

        private static List<GridlineScreen> MakeGridY(NuGenScreenTranslate transform, CoordSettings coordSettings, GridMeshSettings gridMeshSettings)
        {
            int i;
            int xStartS, yStartS, xStopS, yStopS;
            double yG = gridMeshSettings.startY;
            GridlineScreen gridline;
            List<GridlineScreen> gridlines = new List<GridlineScreen>();
            for (i = 0; i < gridMeshSettings.countY; i++)
            {
                gridline = new GridlineScreen();
                transform.XThetaYRToScreen(coordSettings, gridMeshSettings.startX, yG, out xStartS, out yStartS);
                transform.XThetaYRToScreen(coordSettings, gridMeshSettings.stopX, yG, out xStopS, out yStopS);

                gridline.Start.X = xStartS;
                gridline.Start.Y = yStartS;
                gridline.Stop.X = xStopS;
                gridline.Stop.Y = yStopS;
                gridline.R = false;

                gridlines.Add(gridline);

                if (coordSettings.yRScale == Scale.Linear)
                    yG += gridMeshSettings.stepY;
                else
                    yG *= gridMeshSettings.stepY;
            }

            return gridlines;
        }

        private static List<GridlineScreen> MakeGridX(NuGenScreenTranslate transform, CoordSettings coordSettings, GridMeshSettings gridMeshSettings)
        {
            int i;
            int xStartS, yStartS, xStopS, yStopS;
            double xG = gridMeshSettings.startX;
            GridlineScreen gridline;
            List<GridlineScreen> gridlines = new List<GridlineScreen>();
            for (i = 0; i < gridMeshSettings.countX; i++)
            {
                gridline = new GridlineScreen();
                transform.XThetaYRToScreen(coordSettings, xG, gridMeshSettings.startY, out xStartS, out yStartS);
                transform.XThetaYRToScreen(coordSettings, xG, gridMeshSettings.stopY, out xStopS, out yStopS);

                gridline.Start.X = xStartS;
                gridline.Start.Y = yStartS;
                gridline.Stop.X = xStopS;
                gridline.Stop.Y = yStopS;
                gridline.R = false;

                gridlines.Add(gridline);

                if (coordSettings.xThetaScale == Scale.Linear)
                    xG += gridMeshSettings.stepX;
                else
                    xG *= gridMeshSettings.stepX;
            }

            return gridlines;
        }

        private static double AngleSpacing(ThetaUnits units)
        {
            switch (units)
            {
                case ThetaUnits.ThetaDegrees:
                    return angleSpacingDeg;
                case ThetaUnits.ThetaGradians:
                    return angleSpacingGrad;
                case ThetaUnits.ThetaRadians:
                    return angleSpacingRad;
            }

            return angleSpacingDeg;
        }
    }
}
