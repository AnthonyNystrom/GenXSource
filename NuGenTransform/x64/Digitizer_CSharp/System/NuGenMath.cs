using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.NuGenTransform
{
    static class NuGenMath
    {
        //Constants defining the different results from mathematical calculations
        public const int SUCCESS = 0;
        public const int BAD_GRAPH_COORDINATES = 1;
        public const int BAD_SCREEN_COORDINATES = 2;
        public const int NONPOSITIVE_COORDINATE = 3;
        public const int NO_SPREAD = 4;

        /********************************************************************************/
        public static double DistanceToLine(double x, double y, double x1, double y1,
            double x2, double y2)
        {
            // project (x,y) onto line between (x1,y1) and (x2,y2), then return
            // distance between (x,y) and the projected point (xP,yP)
            double xP, yP;
            ProjectPointOnToLine(x, y, x1, y1, x2, y2,  out xP,  out yP);
            return Math.Sqrt((xP - x) * (xP - x) + (yP - y) * (yP - y));
        }

        /*******************************************************************************/
        public static void ProjectPointOnToLine(double xCenter, double yCenter, double xStart,
          double yStart, double xStop, double yStop, out double xProjection, out double yProjection)
        /* find closest point to (x, y) on the line segment from (xStart, yStart) to
           (xStop, yStop). closest point is kept between the start and stop point */
        {
            if (Math.Abs(yStart - yStop) < 0.000001)
            {
                /* special case - line segment is vertical */
                yProjection = yStart;
                double s = (xCenter - xStart) / (xStop - xStart);
                if (s < 0)
                    xProjection = xStart;
                else if (s > 1)
                    xProjection = xStop;
                else
                    xProjection = (1.0 - s) * xStart + s * xStop;
            }
            else
            {
                /* general case - compute slope and intercept of line through
                   (xCenter, yCenter) */
                double slope = (xStop - xStart) / (yStart - yStop);
                double yintercept = yCenter - slope * xCenter;

                /* intersect center point line (slope-intercept form) with start-stop 
                   line (parametric form x=(1-s)*x1+s*x2, y=(1-s)*y1+s*y2) */
                double s = (slope * xStart + yintercept - yStart) /
                  (yStop - yStart + slope * (xStart - xStop));

                if (s < 0)
                {
                    xProjection = xStart;
                    yProjection = yStart;
                }
                else if (s > 1)
                {
                    xProjection = xStop;
                    yProjection = yStop;
                }
                else
                {
                    xProjection = (1.0 - s) * xStart + s * xStop;
                    yProjection = (1.0 - s) * yStart + s * yStop;
                }
            }
        }

        /*******************************************************************************/
        public static int ScreenToGraph(double[][] r_graph, double[,] r_screen, double[,] a, double[,] a_inverse)
        // computes the transformation to get graph coordinates given
        // screen coordinates, where
        //    G      S
        //   r  = T r
        //
        // since this transformation is defined using three points (each in both
        // graph and screen coordinates), we are solving
        //      G   G   G         S   S   S
        //   (r1  r2  r3 ) = T (r1  r2  r3 )
        // so
        //          -1
        //   T = G S
        //
        // we will assume that the computed transformation has all of the appropriate
        // y sign convention transformations, skews, scalings, rotations and translations
        //
        // note that we are NOT using (x,y,z) coordinates where z=constant. instead, so
        // that 3x3 transformations can perform translation, we ARE using (x,y,1) coordinates
        {

            int[] iworkn = new int[3];
            double[] workn = new double[3];
            double[,] worknn = new double[3,3];
            double[,] r_screen_inverse = new double[3,3];

            if(!Inverse(r_screen, 3, 3, iworkn, r_screen_inverse, workn, worknn))
                return BAD_SCREEN_COORDINATES;

            MatrixMultiply3x3(a, r_graph, r_screen_inverse);

            if(!Inverse(a, 3, 3, iworkn, a_inverse, workn, worknn))
                return BAD_SCREEN_COORDINATES;

            return SUCCESS;
        }


        /*    multiply 3x3 matrix, x, by vector y, into vector z */
        public static void MatrixMultiply3x1(double[] z, double[,] x, double[] y)
        {
            if (z.Length != 3 || y.Length != 3 || x.Length != 9)
            {
                throw new InvalidOperationException("Wrong sized matrices passed to matrix multiplyer");
            }
            double sum;
            int nrow, nentry;

            for (nrow = 0; nrow < 3; nrow++)
            {
                sum = 0.0;
                for (nentry = 0; nentry < 3; nentry++)
                {
                    sum = sum + x[nrow,nentry] * y[nentry];
                }
                z[nrow] = sum;
            }
        }

                /*******************************************************************************/
        public static void MatrixMultiply3x3(double[,] z, double[][] x, double[,] y)
        /*    multiply 2 3x3 matrices, x and y, into z */
        {
            int nrow, ncol, nentry;
            double sum;

            for (nrow = 0; nrow < 3; nrow++)
                for (ncol = 0; ncol < 3; ncol++)
                {
                    sum = 0.0;
                    for (nentry = 0; nentry < 3; nentry++)
                        sum = sum + x[nrow][nentry] * y[nentry,ncol];
                    z[nrow,ncol] = sum;
                }
        }

                /*******************************************************************************/
        public static void VectorCrossProduct(double[] r1, double[] r2, double[] r3)
        /*    cross product of first vector with second, into third  */
        {
              r3 [0] = r1 [1] * r2 [2] - r1 [2] * r2 [1];
              r3 [1] = r1 [2] * r2 [0] - r1 [0] * r2 [2];
              r3 [2] = r1 [0] * r2 [1] - r1 [1] * r2 [0];
        }

        //Gets the angular units from a coordinate space
        public static MMUnits mmUnitize(CoordSettings coord)
        {
            if (coord.frame == ReferenceFrame.Cartesian)
                return MMUnits.mmCartesian;
            else
            {
                // polar coordinates
                if (coord.thetaUnits == ThetaUnits.ThetaDegrees)
                    return MMUnits.mmDegrees;
                else if (coord.thetaUnits == ThetaUnits.ThetaGradians)
                    return MMUnits.mmGradians;
                else
                    return MMUnits.mmRadians;
            }
        }

        //Returns the magnitute of a vector
        public static double VectorMagnitude(double x, double y, double z)
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public static void Test()
        {
            double[,] r_screen = new double[3, 3];
            double[][] r_graph = { new double[3], new double[3], new double[3] };


            r_screen[0,0] = 69;
            r_screen[1,0] = 68;
            r_screen[2,0] = 626;
            r_screen[0,1] = 577;
            r_screen[1,1] = 84;
            r_screen[2,1] = 576;
            r_screen[0,2] = 1;
            r_screen[1,2] = 1;
            r_screen[2,2] = 1;

            r_graph[0][0] = 0;
            r_graph[1][0] = 0;
            r_graph[2][0] = 30;
            r_graph[0][1] = -1;
            r_graph[1][1] = 1;
            r_graph[2][1] = -1;
            r_graph[0][2] = 1;
            r_graph[1][2] = 1;
            r_graph[2][2] = 1;

            double[,] s2g = new double[3, 3];
            double[,] g2s = new double[3, 3];

            ScreenToGraph(r_graph, r_screen, s2g, g2s);

            double[] rScreen = new double[3];
            double[] rGraph = new double[3];

            rScreen[0] = 300;
            rScreen[1] = 300;
            rScreen[2] = 1.0;

            NuGenMath.MatrixMultiply3x1(rGraph, s2g, rScreen);

            double xGraph = rGraph[0];
            double yGraph = rGraph[1];        

        }

        //Gets the inverse of a matrix
        private static bool Inverse(double[,] a, int n, int np, int[] iworkn, double[,] y, double[] workn, double[,] worknn)
        {
            double[] column = new double[3];

            for(int i = 0; i<n; i++)
            {
                for(int j = 0; j<n; j++)
                {
                    worknn[i,j] = a[i,j];
                }
            }

            //Decomposes the matrix
            if(!MatrixDecompose(worknn, n, np, iworkn, workn))
                return false;

            for (int j = 0; j < n; j++)
            {
                column[0] = 0;
                column[1] = 0;
                column[2] = 0;

                column[j] = 1;

                if (!VectorSolve(worknn, n, np, iworkn, column))
                    return false;

                y[0, j] = column[0];
                y[1, j] = column[1];
                y[2, j] = column[2];
            }

            return true;
        }

        private static bool VectorSolve(double[,] a, int n, int np, int[] index, double[] b)

           //     solves a*x=b. returns true if successful
        //     input
        //        a           lu decomposition matrix from dludcmp. order is nxn
        //        n           used size of vectors and matrix a
        //        np          dimensioned size of vectors and matrix a
        //        index       permutation vector from dludcmp
        //        b           b vector in a*x=b
        //     output
        //        b           x vector in a*x=b
        {
            double sum;
            int i, j, k, ipermuted;

            k = -1;
            for (i = 0; i < n; i++)
            {
                ipermuted = index[i];
                sum = b[ipermuted];
                b[ipermuted] = b[i];
                if (k != -1)
                    for (j = k; j < i; j++)
                        sum -= a[i, j] * b[j];
                else
                    if (sum != 0.0)
                        k = i;
                b[i] = sum;
            }

            for (i = n - 1; i >= 0; i--)
            {
                sum = b[i];
                for (j = i + 1; j < n; j++)
                    sum -= a[i, j] * b[j];

                if (a[i, i] == 0.0)
                    return false;
                b[i] = sum / a[i, i];
            }

            return true;
        }

        private static bool MatrixDecompose(double[,] a, int n, int np, int[] index, double[] workn)
        //     returns lu decomposition of nxn matrix a, and true if successful
        //     input
        //        a           matrix to decompose
        //        n           used size of vectors and matrix a
        //        np          dimensioned size of vectors and matrix a
        //     output
        //        index       permutation vector
        //     internal
        //        workn       temporary storage matrix
        {
            double tiny = 1.0e-12; // info at http://www.math.byu.edu/~schow/work/IEEEFloatingPoint.htm
            double alargest, term, sum;
            int i, j, k, ilargest;

            for (i = 0; i < n; i++)
            {
                alargest = 0.0;
                for (j = 0; j < n; j++)
                    alargest = Math.Max(alargest, Math.Abs(a[i, j]));
                if (alargest == 0.0)
                    return false;
                workn[i] = 1.0 / alargest;
            }
            for (j = 0; j < n; j++)
            {
                for (i = 0; i < j; i++)
                {
                    sum = a[i, j];
                    for (k = 0; k < i; k++)
                        sum -= a[i, k] * a[k, j];
                    a[i, j] = sum;
                }
                alargest = 0.0;
                ilargest = j;
                for (i = j; i < n; i++)
                {
                    sum = a[i, j];
                    for (k = 0; k < j; k++)
                        sum -= a[i, k] * a[k, j];
                    a[i, j] = sum;
                    term = workn[i] * Math.Abs(sum);
                    if (term >= alargest)
                    {
                        ilargest = i;
                        alargest = term;
                    }
                }
                if (j != ilargest)
                {
                    for (k = 0; k < n; k++)
                    {
                        term = a[ilargest, k];
                        a[ilargest, k] = a[j, k];
                        a[j, k] = term;
                    }
                    workn[ilargest] = workn[j];
                }
                index[j] = ilargest;
                if (a[j, j] == 0.0)
                    a[j, j] = tiny;
                if (j != n - 1)
                {
                    term = 1.0 / a[j, j];
                    for (i = j + 1; i < n; i++)
                        a[i, j] *= term;
                }
            }

            return true;
        }

        public static void AxisScale(double x_min_in, double x_max_in, bool linearAxis,
            out double x_start, out double x_stop, out double x_delta, out int x_count)
        {
            double x_min, x_max;
            const double range_epsilon = 0.00000000001;
            double x_average, x_average_rounded_up, x_range;
            int ndigit_range;

            // define number of digits of precision. although value of 10 seems
            // desirable, the sprintf statements elsewhere in this file, which
            // operate on values with the specified precision, just lose it
            // for more than 8 digits. example '%.7lg' on 40.000005 gives 40.00001
            const int nDigitsPrecision = 8;

            // sort the input values
            if (x_min_in > x_max_in)
            {
                x_min = x_max_in;
                x_max = x_min_in;
            }
            else
            {
                x_min = x_min_in;
                x_max = x_max_in;
            }

            // scale the coordinates logarithmically if log flag is set
            if (!linearAxis)
            {
                x_min = Math.Log10(x_min);
                x_max = Math.Log10(x_max);
            }

            // round off average to first significant digit of range
            x_average = (x_min + x_max) / 2.0;
            x_range = x_max - x_min;
            if (x_range == 0)
                x_range = Math.Abs(x_average / 10.0); // for null range use arbitrary range
            ndigit_range = ValuePower(x_range);
            x_delta = Math.Pow((double)10.0, (double)ndigit_range);
            x_average_rounded_up = x_delta * Math.Floor((x_average + x_delta / 2.0) / x_delta);


            if (x_range > range_epsilon)
            {
                // adjust stepsize if more points are needed, accounting for roundoff
                while (Math.Abs(x_range / x_delta) <= 2.000001)
                    x_delta /= 2.0;
            }

            // go down until min point is included
            x_start = x_average_rounded_up;
            while (x_start > x_min)
                x_start -= x_delta;

            // go up until max point is included
            x_stop = x_average_rounded_up;
            while (x_stop < x_max)
                x_stop += x_delta;

            x_count = 1 + (int)Math.Floor((x_stop - x_start) / x_delta + 0.5);

            if (!linearAxis)
            {
                // convert from log scale back to linear scale
                x_start = Math.Pow((double)10.0, x_start);
                x_stop = Math.Pow((double)10.0, x_stop);
                x_delta = Math.Pow((double)10.0, x_delta);
            }

            // roundoff to eliminate epsilons of 10^-10
            int roundoffPower = ValuePower(x_delta) - nDigitsPrecision;
            x_start = RoundToPower(x_start, roundoffPower);
            x_stop = RoundToPower(x_stop, roundoffPower);
            x_delta = RoundToPower(x_delta, roundoffPower);
        }

        public static int ValuePower(double value)
        {
            // compute power of 10 for input value, rounding down to nearest
            // integer solution of value>=10**solution
            const int minPower = -30;

            double avalue = Math.Abs(value);
            if (avalue < Math.Pow(10.0, minPower))
                return minPower;
            else
                return (int)Math.Floor(Math.Log10(avalue));
        }

        //Rounds a number "arg" to the specified power
        public static double RoundToPower(double arg, int roundOffPower)
        {
            double powerOf10 = Math.Pow((double)10, roundOffPower);
            return powerOf10 * Math.Floor(arg / powerOf10 + 0.5);
        }

        // Gets the area under the curve of a function
        // This is actually an integral estimation, in a calculus application
        //  this would compute the integral of a function defined by the curve 
        //  with increasing accuracy as the number of points plotted aproaches
        //  infinity.  Similar to a reimann sum except there is no starting function
        //  that is being approximated, just a series of points.
        public static double FunctionArea(int nPoints, double[] x, double[] y)
        {
            double sum = 0.0;
            for (int i = 0; i < nPoints - 1; i++)
                sum += (x[i + 1] - x[i]) * (y[i + 1] + y[i]) / 2.0;

            return sum;
        }

        static int nPointsLast = 0;
        static double[] xLast = null;
        static double[] yLast = null;
        static double areaLast = 0.0;

        internal static double PolygonArea(int nPoints, double[] x, double[] y)
        {

            // have the inputs changed?
            bool changed = (nPoints != nPointsLast);
            if (!changed)
            {
                for (int i = 0; i < nPoints; i++)
                    if ((x[i] != xLast[i]) ||
                      (y[i] != yLast[i]))
                    {
                        changed = true;
                        break;
                    }
            }

            if (changed)
            {
                // remove previous allocation
                if (xLast != null)
                {
                    xLast = null;
                }
                if (yLast != null)
                {
                    yLast = null;
                }

                // save new inputs, reallocating memory
                nPointsLast = nPoints;
                areaLast = 0.0;
                if (nPointsLast > 0)
                {
                    xLast = new double[nPointsLast];
                    yLast = new double[nPointsLast];
                    for (int i = 0; i < nPointsLast; i++)
                    {
                        xLast[i] = x[i];
                        yLast[i] = y[i];
                    }

                    // compute new area
                    areaLast = PolygonAreaRecurse(nPointsLast, xLast, yLast, 0);
                }
            }

            return areaLast;
        }

        private static double PolygonAreaRecurse(int nPoints, double[] x, double[] y, int level)
        {
            /* break up polygon into simply connected smaller parts, if the polygon is
     not already simply connected. note - this function closes the polygon
     internally - so the last point should NOT be the same as the first point.
     example, nPoints = 3/4 for a triangle/square

     this function no longer uses memcpy since negative lengths cause crashes in osx

     this function is slow, and since it is called for window focus event that can cause
     sluggish responses. in fact, this effect is probably the cause of crashes in osx.
     therefore, the polygonareaoptimizer wrapper function should be used */
            const double NEGATIVE_AREA = -1.0;

            //  QString inputs = QString("level: %1, points: %2\n").arg(level).arg(nPoints);
            //  for (int iii = 0; iii < nPoints; iii++)
            //  {
            //    inputs += QString("[%1,%2]").arg(x[iii]).arg(y[iii]);
            //    if ((iii % 10 == 9) && (iii != nPoints - 1))
            //      inputs += QString("\n");
            //  }
            //  QMessageBox::critical(0, QString("polygonarea"), inputs);
            //  fprintf (stderr,  "%s\n\n", inputs.latin1());

            if (nPoints < 3)
                return 0.0;

            for (int linea = 0; linea < nPoints - 1; linea++)
                for (int lineb = linea + 1; lineb < nPoints; lineb++)
                {
                    int linebp1 = lineb + 1;
                    if (linebp1 >= nPoints)
                        linebp1 -= nPoints;

                    double sLineaInt, sLinebInt;
                    if (IntersectTwoLines(x[linea], y[linea], x[linea + 1],
                      y[linea + 1], x[lineb], y[lineb], x[linebp1],
                      y[linebp1], out sLineaInt, out sLinebInt) && (0 < sLineaInt) &&
                      (sLineaInt < 1) && (0 < sLinebInt) && (sLinebInt < 1))
                    {
                        /* lines between points linea and linea+1, and lineb and
                           lineb+1, cross each other. break up each line to create
                           two new polygons */

                        /* a pathological case occurs when the number of points in either of
                           the two subareas equals the nPoints (nPointsArea1 or nPointsArea2 is zero).
                           in this case, this function will recurse forever because the smaller subareas
                           are not actually smaller. this infinite recursion indeed happens on the mac powerpc
                           for the corners.png sample file. applying some algebra to the constraints that
                           neither nPointsArea1 nor nPointsArea2 can equal nPoints gives the interesting
                           result that the difference lineb-linea can never be 1 nor nPoints-1. after some
                           reflection, this makes sense for two reasons:
                           1) if nPointsArea1 equals nPoints then nPointsArea2 equals two, or
                              if nPointsArea2 equals nPoints then nPointsArea1 equals two, and we
                              can only subdivide the areas into triangles
                           2) this case corresponds to a vertex, and not the intersection of two closed polygons */
                        int nPointsArea1 = (linea + 1) + (1) + (nPoints - lineb - 1);
                        int nPointsArea2 = (lineb - linea) + (1);

                        if ((nPoints != nPointsArea1) &&
                          (nPoints != nPointsArea2))
                        {
                            /* recurse*/
                            int iFrom, iTo;

                            /* recurse for first area */
                            double area1 = NEGATIVE_AREA;
                            double[] xArea1 = new double[nPointsArea1];
                            double[] yArea1 = new double[nPointsArea1];
                            if ((xArea1.Length > 0) && (yArea1.Length > 0))
                            {
                                for (iFrom = 0, iTo = 0; iTo < linea + 1; iFrom++, iTo++)
                                {
                                    xArea1[iTo] = x[iFrom];
                                    yArea1[iTo] = y[iFrom];
                                }
                                xArea1[linea + 1] = (1.0 - sLineaInt) * x[linea] + sLineaInt * x[linea + 1];
                                yArea1[linea + 1] = (1.0 - sLineaInt) * y[linea] + sLineaInt * y[linea + 1];
                                for (iFrom = lineb + 1, iTo = linea + 2; iTo < nPointsArea1; iFrom++, iTo++)
                                {
                                    xArea1[iTo] = x[iFrom];
                                    yArea1[iTo] = y[iFrom];
                                }
                                area1 = PolygonAreaRecurse(nPointsArea1, xArea1, yArea1, level + 1);
                            }

                            /* recurse for second area */
                            double area2 = NEGATIVE_AREA;
                            double[] xArea2 = new double[nPointsArea2];
                            double[] yArea2 = new double[nPointsArea2];
                            if ((xArea1.Length > 0) && (yArea1.Length > 0) &&
                              (xArea2.Length > 0) && (yArea2.Length > 0))
                            {
                                for (iFrom = linea + 1, iTo = 0; iTo < lineb - linea; iFrom++, iTo++)
                                {
                                    xArea2[iTo] = x[iFrom];
                                    yArea2[iTo] = y[iFrom];
                                }
                                xArea2[lineb - linea] = xArea1[linea + 1];
                                yArea2[lineb - linea] = yArea1[linea + 1];
                                area2 = PolygonAreaRecurse(nPointsArea2, xArea2, yArea2, level + 1);
                            }

                            if ((area1 < 0.0) || (area2 < 0.0))
                                return NEGATIVE_AREA;

                            return area1 + area2;
                        }
                    }
                }

            // area of a general simply-connected polygon
            double columnleft = 0.0, columnright = 0.0;
            int i, ip1;
            for (i = 0; i < nPoints; i++)
            {
                ip1 = (i + 1) % nPoints;

                // cast the integers to double so integer overflow does not give negative column values
                columnleft += ((double)x[ip1]) * ((double)y[i]);
                columnright += ((double)x[i]) * ((double)y[ip1]);
            }

            return Math.Abs(columnleft - columnright) / 2.0;
        }

        public static bool IntersectTwoLines(double xLine1a, double yLine1a, double xLine1b, double yLine1b,
            double xLine2a, double yLine2a, double xLine2b, double yLine2b, out double sLine1Int,
            out double sLine2Int)
        {
            /* parameterize the two-point lines as x=(1-s)*xa+s*xb, y=(1-s)*ya+s*yb and
            then intersect to get s = numerator / denominator */
            double denominator, numeratorLine1, numeratorLine2;

            denominator =
              (yLine2b - yLine2a) * (xLine1b - xLine1a) -
              (yLine1b - yLine1a) * (xLine2b - xLine2a);

            if (Math.Abs(denominator) < 0.000001)
            {
                sLine1Int = 0.0;
                sLine2Int = 0.0;
                return false; // either zero or an infinite number of points intersect
            }

            numeratorLine1 =
              (yLine1a - yLine2a) * (xLine2b - xLine2a) -
              (yLine2b - yLine2a) * (xLine1a - xLine2a);
            numeratorLine2 =
              (yLine1b - yLine1a) * (xLine2a - xLine1a) -
              (yLine2a - yLine1a) * (xLine1b - xLine1a);

            sLine1Int = numeratorLine1 / denominator;
            sLine2Int = numeratorLine2 / denominator;

            return true;
        }

        //Gets the angle between two vectors
        public static double Angle(double[] r1, double[] r2)
        {
            double r1mag, r2mag, rdot12, cosine, value;

            r1mag = VectorMagnitude(r1);
            r2mag = VectorMagnitude(r2);
            rdot12 = DotProduct(r1, r2);
            cosine = rdot12 / (r1mag * r2mag);
            if (Math.Abs(cosine) < 1.0)
                value = Math.Acos(cosine);
            else
                if (cosine > 1.0)
                    value = 0.0;
                else
                    value = Math.PI;

            return (value);
        }

        //overloaded convienience method
        public static double VectorMagnitude(double[] r)
        {
            return VectorMagnitude(r[0], r[1], r[2]);
        }

        //overloaded convienience method
        public static double DotProduct(double[] r1, double[] r2)
        {
            return (r1[0] * r2[0] + r1[1] * r2[1] + r1[2] * r2[2]);
        }
    }
}
