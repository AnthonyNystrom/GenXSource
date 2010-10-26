using System;
using System.Collections.Generic;
using System.Text;
using javax.vecmath;
using System.Collections;
using Microsoft.DirectX;

namespace NuGenJmol
{
    /// <summary>
    /// Implementation of hermite curves for drawing smoothed curves
    /// that pass through specified points.
    /// 
    /// Examples of usage in Jmol include the commands: trace, ribbons and cartoons
    /// 
    /// for some useful background info about hermite curves check out
    /// <a href='http://www.cubic.org/docs/hermite.htm'>http://www.cubic.org/docs/hermite.htm</a>
    /// </summary>
    /// <remarks>Uses code from org.jmol.g3d.Hermite3D<author>Miguel, miguel@jmol.org</author></remarks>
    class NuHermite3D
    {
        NuGraphics3D g3d;
        List<List<Vector3>> triStripBuffer;

        public NuHermite3D(NuGraphics3D g3d)
        {
            this.g3d = g3d;

            for (int i = 16; --i >= 0; )
            {
                pLeft[i] = new Point3i();
                pRight[i] = new Point3i();

                pTopLeft[i] = new Point3f();
                pTopRight[i] = new Point3f();
                pBotLeft[i] = new Point3f();
                pBotRight[i] = new Point3f();
            }
            triStripBuffer = new List<List<Vector3>>();
        }

        readonly Point3i[] pLeft = new Point3i[16];
        readonly Point3i[] pRight = new Point3i[16];

        readonly float[] sLeft = new float[16];
        readonly float[] sRight = new float[16];
        int sp;

        readonly Point3f[] pTopLeft = new Point3f[16];
        readonly Point3f[] pTopRight = new Point3f[16];
        readonly Point3f[] pBotLeft = new Point3f[16];
        readonly Point3f[] pBotRight = new Point3f[16];
        readonly bool[] needToFill = new bool[16];

        public void render(bool tFill, short colix, int tension,
                           int diameterBeg, int diameterMid, int diameterEnd,
                           Point3i p0, Point3i p1, Point3i p2, Point3i p3)
        {
            int x1 = p1.x, y1 = p1.y, z1 = p1.z;
            int x2 = p2.x, y2 = p2.y, z2 = p2.z;
            int xT1 = ((x2 - p0.x) * tension) / 8;
            int yT1 = ((y2 - p0.y) * tension) / 8;
            int zT1 = ((z2 - p0.z) * tension) / 8;
            int xT2 = ((p3.x - x1) * tension) / 8;
            int yT2 = ((p3.y - y1) * tension) / 8;
            int zT2 = ((p3.z - z1) * tension) / 8;
            sLeft[0] = 0;
            pLeft[0].set_Renamed(p1);
            sRight[0] = 1;
            pRight[0].set_Renamed(p2);
            sp = 0;
            int dDiameterFirstHalf = 0;
            int dDiameterSecondHalf = 0;
            if (tFill)
            {
                dDiameterFirstHalf = 2 * (diameterMid - diameterBeg);
                dDiameterSecondHalf = 2 * (diameterEnd - diameterMid);
            }
            else
                g3d.setColix(colix);

            do
            {
                Point3i a = pLeft[sp];
                Point3i b = pRight[sp];
                int dx = b.x - a.x;
                if (dx >= -1 && dx <= 1)
                {
                    int dy = b.y - a.y;
                    if (dy >= -1 && dy <= 1)
                    {
                        // mth 2003 10 13
                        // I tried drawing short cylinder segments here,
                        // but drawing spheres was faster
                        float _s = sLeft[sp];
                        if (tFill)
                        {
                            int d =(_s < 0.5f
                                    ? diameterBeg + (int)(dDiameterFirstHalf * _s)
                                    : diameterMid + (int)(dDiameterSecondHalf * (_s - 0.5f)));
                            g3d.fillSphereCentered(colix, d, a);
                        }
                        //else
                        //    g3d.plotPixelClipped(a);
                    }
                    --sp;
                    continue;
                }
                double s = (sLeft[sp] + sRight[sp]) / 2;
                double s2 = s * s;
                double s3 = s2 * s;
                double h1 = 2 * s3 - 3 * s2 + 1;
                double h2 = -2 * s3 + 3 * s2;
                double h3 = s3 - 2 * s2 + s;
                double h4 = s3 - s2;
                Point3i pMid = pRight[sp + 1];
                pMid.x = (int)(h1 * x1 + h2 * x2 + h3 * xT1 + h4 * xT2);
                pMid.y = (int)(h1 * y1 + h2 * y2 + h3 * yT1 + h4 * yT2);
                pMid.z = (int)(h1 * z1 + h2 * z2 + h3 * zT1 + h4 * zT2);
                pRight[sp + 1] = pRight[sp];
                sRight[sp + 1] = sRight[sp];
                pRight[sp] = pMid;
                sRight[sp] = (float)s;
                ++sp;
                pLeft[sp].set_Renamed(pMid);
                sLeft[sp] = (float)s;
            }
            while (sp >= 0);
        }

        void render2x(bool fill, short colix, int tension,
                      //top strand segment
                      Point3i p0, Point3i p1, Point3i p2, Point3i p3,
                      //bottom strand segment
                      Point3i p4, Point3i p5, Point3i p6, Point3i p7)
        {
            Point3i[] endPoints = { p2, p1, p6, p5 };
            // stores all points for top+bottom strands of 1 segment
            List<Point3i> points = new List<Point3i>(10);
            int whichPoint = 0;

            int numTopStrandPoints = 2; //first and last points automatically included
            float numPointsPerSegment = 5.0f;//use 5 for mesh

            // could make it so you can set this from script command
            if (fill)
                numPointsPerSegment = 10.0f;

            float interval = (1.0f / numPointsPerSegment);
            float currentInt = 0.0f;

            int x1 = p1.x, y1 = p1.y, z1 = p1.z;
            int x2 = p2.x, y2 = p2.y, z2 = p2.z;
            int xT1 = ((x2 - p0.x) * tension) / 8;
            int yT1 = ((y2 - p0.y) * tension) / 8;
            int zT1 = ((z2 - p0.z) * tension) / 8;
            int xT2 = ((p3.x - x1) * tension) / 8;
            int yT2 = ((p3.y - y1) * tension) / 8;
            int zT2 = ((p3.z - z1) * tension) / 8;
            sLeft[0] = 0;
            pLeft[0].set_Renamed(p1);
            sRight[0] = 1;
            pRight[0].set_Renamed(p2);
            sp = 0;
            g3d.setColix(colix); // set colix here for entire routine

            for (int strands = 2; strands > 0; strands--)
            {
                if (strands == 1)
                {
                    x1 = p5.x; y1 = p5.y; z1 = p5.z;
                    x2 = p6.x; y2 = p6.y; z2 = p6.z;
                    xT1 = ( (x2 - p4.x) * tension) / 8;
                    yT1 = ( (y2 - p4.y) * tension) / 8;
                    zT1 = ( (z2 - p4.z) * tension) / 8;
                    xT2 = ( (p7.x - x1) * tension) / 8;
                    yT2 = ( (p7.y - y1) * tension) / 8;
                    zT2 = ( (p7.z - z1) * tension) / 8;
                    sLeft[0] = 0;
                    pLeft[0].set_Renamed(p5);
                    sRight[0] = 1;
                    pRight[0].set_Renamed(p6);
                    sp = 0;
                }

                points.Add(endPoints[whichPoint++]);
                currentInt = interval;
                do
                {
                    Point3i a = pLeft[sp];
                    Point3i b = pRight[sp];
                    int dx = b.x - a.x;
                    int dy = b.y - a.y;
                    int dist2 = dx * dx + dy * dy;
                    if (dist2 <= 2)
                    {
                        // mth 2003 10 13
                        // I tried drawing short cylinder segments here,
                        // but drawing spheres was faster
                        float s = sLeft[sp];

                        g3d.fillSphereCentered(colix, 3, a);
                        //draw outside edges of mesh

                        if (s < 1.0f - currentInt)
                        { //if first point over the interval
                            Point3i temp = new Point3i();
                            temp.set_Renamed(a);
                            points.Add(temp); //store it
                            currentInt += interval; // increase to next interval
                            if (strands == 2)
                            {
                                numTopStrandPoints++;
                            }
                        }
                        --sp;
                    }
                    else
                    {
                        double s = (sLeft[sp] + sRight[sp]) / 2;
                        double s2 = s * s;
                        double s3 = s2 * s;
                        double h1 = 2 * s3 - 3 * s2 + 1;
                        double h2 = -2 * s3 + 3 * s2;
                        double h3 = s3 - 2 * s2 + s;
                        double h4 = s3 - s2;
                        Point3i pMid = pRight[sp + 1];
                        pMid.x = (int)(h1 * x1 + h2 * x2 + h3 * xT1 + h4 * xT2);
                        pMid.y = (int)(h1 * y1 + h2 * y2 + h3 * yT1 + h4 * yT2);
                        pMid.z = (int)(h1 * z1 + h2 * z2 + h3 * zT1 + h4 * zT2);
                        pRight[sp + 1] = pRight[sp];
                        sRight[sp + 1] = sRight[sp];
                        pRight[sp] = pMid;
                        sRight[sp] = (float)s;
                        ++sp;
                        pLeft[sp].set_Renamed(pMid);
                        sLeft[sp] = (float)s;
                    }
                }
                while (sp >= 0);

                points.Add(endPoints[whichPoint++]);
            } //end of for loop - processed top and bottom strands
            int size = points.Count;
            if (fill)
            {   //RIBBONS
                Point3i t1 = null;
                Point3i b1 = null;
                Point3i t2 = null;
                Point3i b2 = null;
                int top = 1;

                Vector3[] strip = new Vector3[numTopStrandPoints * 2];
                int stIdx = 0;
                for (; top < numTopStrandPoints && (top + numTopStrandPoints) < size; top++)
                {
                    t1 = points[top - 1];
                    b1 = points[numTopStrandPoints + (top - 1)];

                    strip[stIdx++] = new Vector3(t1.X, t1.y, t1.z);
                    strip[stIdx++] = new Vector3(b1.x, b1.y, b1.z);
                }
                if ((numTopStrandPoints * 2) != size)
                {   //BUG(DC09_MAY_2004): not sure why but
                    //sometimes misses triangle at very start of segment
                    //temp fix - will inestigate furture
                    g3d.fillTriangle(p1, p5, t2);
                    g3d.fillTriangle(b2, t2, p5);
                }
                g3d.FillTriangleStrip(strip);
            }
            else
            {   //MESH
                for (int top = 0;
                     top < numTopStrandPoints && (top + numTopStrandPoints) < size; top++)
                {
                    g3d.drawLine(points[top],
                                 points[top + numTopStrandPoints]);
                }
            }
        }

        static void set(Point3f p3f, Point3i p3i)
        {
            p3f.x = p3i.x;
            p3f.y = p3i.y;
            p3f.z = p3i.z;
        }

        public void render2(bool fill, bool border, short colix, int tension,
                            //top strand segment
                            Point3i p0, Point3i p1, Point3i p2, Point3i p3,
                            //bottom strand segment
                            Point3i p4, Point3i p5, Point3i p6, Point3i p7)
        {
            if (!fill)
            {
                render2x(fill, colix, tension, p0, p1, p2, p3, p4, p5, p6, p7);
                return;
            }

            /*
            System.out.println("------------render2-------------");
            System.out.println("p1=" + p1 + " p2=" + p2 + "\np3=" + p3 + " p4=" + p4);
            */
            int x1 = p1.x, y1 = p1.y, z1 = p1.z;
            int x2 = p2.x, y2 = p2.y, z2 = p2.z;
            int xT1 = ((x2 - p0.x) * tension) / 8;
            int yT1 = ((y2 - p0.y) * tension) / 8;
            int zT1 = ((z2 - p0.z) * tension) / 8;
            int xT2 = ((p3.x - x1) * tension) / 8;
            int yT2 = ((p3.y - y1) * tension) / 8;
            int zT2 = ((p3.z - z1) * tension) / 8;
            set(pTopLeft[0], p1);
            set(pTopRight[0], p2);

            int x5 = p5.x, y5 = p5.y, z5 = p5.z;
            int x6 = p6.x, y6 = p6.y, z6 = p6.z;
            int xT5 = ((x6 - p4.x) * tension) / 8;
            int yT5 = ((y6 - p4.y) * tension) / 8;
            int zT5 = ((z6 - p4.z) * tension) / 8;
            int xT6 = ((p7.x - x5) * tension) / 8;
            int yT6 = ((p7.y - y5) * tension) / 8;
            int zT6 = ((p7.z - z5) * tension) / 8;
            set(pBotLeft[0], p5);
            set(pBotRight[0], p6);

            sLeft[0] = 0;
            sRight[0] = 1;
            needToFill[0] = true;
            sp = 0;

            do
            {
                Point3f a = pTopLeft[sp];
                Point3f b = pTopRight[sp];
                double dxTop = b.x - a.x;
                double dxTop2 = dxTop * dxTop;
                if (dxTop2 < 10)
                {
                    double dyTop = b.y - a.y;
                    double dyTop2 = dyTop * dyTop;
                    if (dyTop2 < 10)
                    {
                        Point3f c = pBotLeft[sp];
                        Point3f d = pBotRight[sp];
                        double dxBot = d.x - c.x;
                        double dxBot2 = dxBot * dxBot;
                        if (dxBot2 < 8)
                        {
                            double dyBot = d.y - c.y;
                            double dyBot2 = dyBot * dyBot;
                            if (dyBot2 < 8)
                            {
                                if (border)
                                {
                                    g3d.fillSphereCentered(colix, 3, a);
                                    g3d.fillSphereCentered(colix, 3, c);
                                }

                                if (needToFill[sp])
                                {
                                    //g3d.fillQuadrilateral(colix, a, b, d, c);

                                    // see if fits on end of any existing
                                    bool appended = false;
                                    for (int strip = 0; strip < triStripBuffer.Count; strip++)
                                    {
                                        int end2Idx = triStripBuffer[strip].Count - 2;
                                        if (triStripBuffer[strip][end2Idx].X == b.x &&
                                            triStripBuffer[strip][end2Idx].Y == b.y &&
                                            triStripBuffer[strip][end2Idx].Z == b.z &&

                                            triStripBuffer[strip][end2Idx + 1].X == d.x &&
                                            triStripBuffer[strip][end2Idx + 1].Y == d.y &&
                                            triStripBuffer[strip][end2Idx + 1].Z == d.z)
                                        {
                                            // append to end
                                            triStripBuffer[strip].Add(new Vector3(a.x, a.y, a.z));
                                            triStripBuffer[strip].Add(new Vector3(c.x, c.y, c.z));
                                            appended = true;
                                        }
                                    }

                                    if (!appended)
                                    {
                                        List<Vector3> strip = new List<Vector3>();
                                        triStripBuffer.Add(strip);

                                        strip.Add(new Vector3(b.x, b.y, b.z));
                                        strip.Add(new Vector3(d.x, d.y, d.z));
                                        strip.Add(new Vector3(a.x, a.y, a.z));
                                        strip.Add(new Vector3(c.x, c.y, c.z));
                                    }

                                    needToFill[sp] = false;
                                }
                                if (dxTop2 + dyTop2 < 2 &&
                                    dxBot2 + dyBot2 < 2)
                                {
                                    --sp;
                                    continue;
                                }
                            }
                        }
                    }
                }
                double s = (sLeft[sp] + sRight[sp]) / 2;
                double s2 = s * s;
                double s3 = s2 * s;
                double h1 = 2 * s3 - 3 * s2 + 1;
                double h2 = -2 * s3 + 3 * s2;
                double h3 = s3 - 2 * s2 + s;
                double h4 = s3 - s2;

                int spNext = sp + 1;
                Point3f pMidTop = pTopRight[spNext];
                pMidTop.x = (float)(h1 * x1 + h2 * x2 + h3 * xT1 + h4 * xT2);
                pMidTop.y = (float)(h1 * y1 + h2 * y2 + h3 * yT1 + h4 * yT2);
                pMidTop.z = (float)(h1 * z1 + h2 * z2 + h3 * zT1 + h4 * zT2);
                Point3f pMidBot = pBotRight[spNext];
                pMidBot.x = (float)(h1 * x5 + h2 * x6 + h3 * xT5 + h4 * xT6);
                pMidBot.y = (float)(h1 * y5 + h2 * y6 + h3 * yT5 + h4 * yT6);
                pMidBot.z = (float)(h1 * z5 + h2 * z6 + h3 * zT5 + h4 * zT6);

                pTopRight[spNext] = pTopRight[sp];
                pTopRight[sp] = pMidTop;
                pBotRight[spNext] = pBotRight[sp];
                pBotRight[sp] = pMidBot;

                sRight[spNext] = sRight[sp];
                sRight[sp] = (float)s;
                needToFill[spNext] = needToFill[sp];
                pTopLeft[spNext].set_Renamed(pMidTop);
                pBotLeft[spNext].set_Renamed(pMidBot);
                sLeft[spNext] = (float)s;
                ++sp;
            } while (sp >= 0);


        }

        public void Flush()
        {
            // convert buffers to real strips
            foreach (List<Vector3> strip in triStripBuffer)
            {
                g3d.FillTriangleStrip(strip.ToArray());
            }
        }
    }
}