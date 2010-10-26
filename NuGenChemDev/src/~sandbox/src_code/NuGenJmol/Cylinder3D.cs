/* $RCSfile$
* $Author: migueljmol $
* $Date: 2006-03-25 13:27:02 -0500 (Sat, 25 Mar 2006) $
* $Revision: 4700 $
*
* Copyright (C) 2003-2006  Miguel, Jmol Development, www.jmol.org
*
* Contact: miguel@jmol.org
*
*  This library is free software; you can redistribute it and/or
*  modify it under the terms of the GNU Lesser General Public
*  License as published by the Free Software Foundation; either
*  version 2.1 of the License, or (at your option) any later version.
*
*  This library is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
*  Lesser General Public License for more details.
*
*  You should have received a copy of the GNU Lesser General Public
*  License along with this library; if not, write to the Free Software
*  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using NuGenJmol;

namespace Org.Jmol.G3d
{
    /// <summary><p>
    /// Draws shaded cylinders in 3D.
    /// </p>
    /// <p>
    /// Cylinders are used to draw bonds.
    /// </p>
    /// 
    /// </summary>
    /// <author>  Miguel, miguel@jmol.org
    /// </author>
    class Cylinder3D
    {
        internal NuGraphics3D g3d;

        public Cylinder3D(NuGraphics3D g3d)
        {
            this.g3d = g3d;
        }

        //private short colixA, colixB;
        //private int[] shadesA;
        //private bool isScreenedA;
        //private int[] shadesB;
        //private bool isScreenedB;
        //private int xA, yA, zA;
        //private int dxB, dyB, dzB;
        //private bool tEvenDiameter;
        ////private int evenCorrection;
        //private int diameter;
        //private sbyte endcaps;
        //private bool tEndcapOpen;
        //private int xEndcap, yEndcap, zEndcap;
        //private int argbEndcap;
        //private short colixEndcap;
        //private int intensityEndcap;

        //private float radius, radius2, cosTheta, cosPhi, sinPhi;

        //internal int sampleCount;
        ////private float[] samples = new float[32];

        //public virtual void render(short colixA, short colixB, sbyte endcaps, int diameter, int xA, int yA, int zA, int xB, int yB, int zB)
        //{
        //    if (isFullyClipped(diameter, xA, yA, zA, xB, yB, zB))
        //        return;
        //    this.dxB = xB - xA; this.dyB = yB - yA; this.dzB = zB - zA;
        //    if (diameter <= 1)
        //    {
        //        g3d.plotLineDelta(colixA, colixB, xA, yA, zA, dxB, dyB, dzB);
        //        return;
        //    }
        //    this.diameter = diameter;
        //    this.xA = xA; this.yA = yA; this.zA = zA;

        //    this.shadesA = g3d.getShades(this.colixA = colixA);
        //    this.shadesB = g3d.getShades(this.colixB = colixB);
        //    this.isScreenedA = (colixA & Graphics3D.TRANSLUCENT_MASK) != 0;
        //    this.isScreenedB = (colixB & Graphics3D.TRANSLUCENT_MASK) != 0;

        //    this.endcaps = endcaps;
        //    calcArgbEndcap(true);

        //    generateBaseEllipse();

        //    if (endcaps == Graphics3D.ENDCAPS_FLAT)
        //        renderFlatEndcap(true);
        //    for (int i = rasterCount; --i >= 0; )
        //        plotRaster(i);
        //    if (endcaps == Graphics3D.ENDCAPS_SPHERICAL)
        //        renderSphericalEndcaps();
        //}

        //public virtual void generateBaseEllipse()
        //{
        //    tEvenDiameter = (diameter & 1) == 0;
        //    //    System.out.println("diameter=" + diameter);
        //    radius = diameter / 2.0f;
        //    radius2 = radius * radius;
        //    int mag2d2 = dxB * dxB + dyB * dyB;
        //    if (mag2d2 == 0)
        //    {
        //        cosTheta = 1;
        //        cosPhi = 1;
        //        sinPhi = 0;
        //    }
        //    else
        //    {
        //        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        //        float mag2d = (float)System.Math.Sqrt(mag2d2);
        //        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        //        float mag3d = (float)System.Math.Sqrt(mag2d2 + dzB * dzB);
        //        /*
        //        System.out.println("dxB=" + dxB + " dyB=" + dyB + " dzB=" + dzB +
        //        " mag2d=" + mag2d + " mag3d=" + mag3d);
        //        */
        //        cosTheta = dzB / mag3d;
        //        cosPhi = dxB / mag2d;
        //        sinPhi = dyB / mag2d;
        //    }

        //    calcRotatedPoint(0f, 0);
        //    calcRotatedPoint(0.5f, 1);
        //    calcRotatedPoint(1f, 2);
        //    rasterCount = 3;
        //    interpolate(0, 1);
        //    interpolate(1, 2);
        //}

        //public virtual bool isFullyClipped(int diameter, int xA, int yA, int zA, int xB, int yB, int zB)
        //{
        //    // miguel 2006 03 25
        //    // probably this should use the Cohen-Sutherland line clipping
        //    // in Line3D
        //    // but right now it seems too difficult to me to figure out
        //    // where all the edges are on the bonds
        //    int slab = g3d.slab;
        //    int depth = g3d.depth;
        //    int width = g3d.width;
        //    int height = g3d.height;

        //    int r = diameter / 2 + 1;
        //    int xMinA = xA - r, xMaxA = xA + r;
        //    int xMinB = xB - r, xMaxB = xB + r;
        //    if (xMaxA < 0 && xMaxB < 0 || xMinA >= width && xMinB >= width)
        //        return true;
        //    int yMinA = yA - r, yMaxA = yA + r;
        //    int yMinB = yB - r, yMaxB = yB + r;
        //    if (yMaxA < 0 && yMaxB < 0 || yMinA >= height && yMinB >= height)
        //        return true;
        //    int zMinA = zA - r, zMaxA = zA + r;
        //    int zMinB = zB - r, zMaxB = zB + r;
        //    if (zMaxA < slab && zMaxB < slab || zMinA >= depth && zMinB >= depth)
        //        return true;
        //    return false;
        //}

        //public virtual void interpolate(int iLower, int iUpper)
        //{
        //    int dx = xRaster[iUpper] - xRaster[iLower];
        //    if (dx < 0)
        //        dx = -dx;
        //    int dy = yRaster[iUpper] - yRaster[iLower];
        //    if (dy < 0)
        //        dy = -dy;
        //    /*
        //    System.out.println("interpolate(" + iLower + "," + iUpper + ")" + " -> " +
        //    "dx=" + dx + " dy=" + dy);
        //    */
        //    if ((dx + dy) <= 1)
        //        return;
        //    float tLower = tRaster[iLower];
        //    float tUpper = tRaster[iUpper];
        //    int iMid = allocRaster();
        //    for (int j = 4; --j >= 0; )
        //    {
        //        float tMid = (tLower + tUpper) / 2;
        //        calcRotatedPoint(tMid, iMid);
        //        if ((xRaster[iMid] == xRaster[iLower]) && (yRaster[iMid] == yRaster[iLower]))
        //        {
        //            fp8IntensityUp[iLower] = (fp8IntensityUp[iLower] + fp8IntensityUp[iMid]) / 2;
        //            tLower = tMid;
        //        }
        //        else if ((xRaster[iMid] == xRaster[iUpper]) && (yRaster[iMid] == yRaster[iUpper]))
        //        {
        //            fp8IntensityUp[iUpper] = (fp8IntensityUp[iUpper] + fp8IntensityUp[iMid]) / 2;
        //            tUpper = tMid;
        //        }
        //        else
        //        {
        //            interpolate(iLower, iMid);
        //            interpolate(iMid, iUpper);
        //            return;
        //        }
        //    }
        //    xRaster[iMid] = xRaster[iLower];
        //    yRaster[iMid] = yRaster[iUpper];
        //}

        //public virtual void plotRaster(int i)
        //{
        //    int fp8Up = fp8IntensityUp[i];
        //    //int iUp = fp8Up >> 8;
        //    //int iUpNext = iUp < Shade3D.shadeLast ? iUp + 1 : iUp;
        //    /*
        //    System.out.println("plotRaster " + i + " (" + xRaster[i] + "," +
        //    yRaster[i] + "," + zRaster[i] + ")" +
        //    " iUp=" + iUp);
        //    */
        //    int x = xRaster[i];
        //    int y = yRaster[i];
        //    int z = zRaster[i];
        //    if (tEndcapOpen)
        //    {
        //        g3d.plotPixelClipped(argbEndcap, xEndcap + x, yEndcap + y, zEndcap - z - 1);
        //        g3d.plotPixelClipped(argbEndcap, xEndcap - x, yEndcap - y, zEndcap + z - 1);
        //    }
        //    g3d.plotLineDelta(shadesA, isScreenedA, shadesB, isScreenedB, fp8Up, xA + x, yA + y, zA - z, dxB, dyB, dzB);
        //    if (endcaps == Graphics3D.ENDCAPS_OPEN)
        //    {
        //        g3d.plotLineDelta(shadesA[0], isScreenedA, shadesB[0], isScreenedB, xA - x, yA - y, zA + z, dxB, dyB, dzB);
        //    }
        //}

        //public virtual int[] realloc(int[] a)
        //{
        //    int[] t;
        //    t = new int[a.Length * 2];
        //    Array.Copy(a, 0, t, 0, a.Length);
        //    return t;
        //}

        //public virtual float[] realloc(float[] a)
        //{
        //    float[] t;
        //    t = new float[a.Length * 2];
        //    Array.Copy(a, 0, t, 0, a.Length);
        //    return t;
        //}

        //public virtual int allocRaster()
        //{
        //    if (rasterCount == xRaster.Length)
        //    {
        //        xRaster = realloc(xRaster);
        //        yRaster = realloc(yRaster);
        //        zRaster = realloc(zRaster);
        //        tRaster = realloc(tRaster);
        //        fp8IntensityUp = realloc(fp8IntensityUp);
        //    }
        //    return rasterCount++;
        //}

        //internal int rasterCount;

        //internal float[] tRaster = new float[32];
        //internal int[] xRaster = new int[32];
        //internal int[] yRaster = new int[32];
        //internal int[] zRaster = new int[32];
        //internal int[] fp8IntensityUp = new int[32];

        //public virtual void calcRotatedPoint(float t, int i)
        //{
        //    tRaster[i] = t;
        //    double tPI = t * System.Math.PI;
        //    double xT = System.Math.Sin(tPI) * cosTheta;
        //    double yT = System.Math.Cos(tPI);
        //    double xR = radius * (xT * cosPhi - yT * sinPhi);
        //    double yR = radius * (xT * sinPhi + yT * cosPhi);
        //    double z2 = radius2 - (xR * xR + yR * yR);
        //    double zR = (z2 > 0 ? System.Math.Sqrt(z2) : 0);

        //    if (tEvenDiameter)
        //    {
        //        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        //        xRaster[i] = (int)(xR - 0.5);
        //        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        //        yRaster[i] = (int)(yR - 0.5);
        //    }
        //    else
        //    {
        //        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        //        xRaster[i] = (int)(xR);
        //        //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        //        yRaster[i] = (int)(yR);
        //    }
        //    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        //    zRaster[i] = (int)(zR + 0.5);

        //    //UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
        //    fp8IntensityUp[i] = Shade3D.calcFp8Intensity((float)xR, (float)yR, (float)zR);

        //    /*
        //    System.out.println("calcRotatedPoint(" + t + "," + i + ")" + " -> " +
        //    xRaster[i] + "," + yRaster[i] + "," + zRaster[i]);
        //    */
        //}

        //internal int yMin, yMax;
        //internal int xMin, xMax;
        //internal int zXMin, zXMax;

        //public virtual void findMinMaxY()
        //{
        //    yMin = yMax = yRaster[0];
        //    for (int i = rasterCount; --i > 0; )
        //    {
        //        int y = yRaster[i];
        //        if (y < yMin)
        //            yMin = y;
        //        else if (y > yMax)
        //            yMax = y;
        //        else
        //        {
        //            y = -y;
        //            if (y < yMin)
        //                yMin = y;
        //            else if (y > yMax)
        //                yMax = y;
        //        }
        //    }
        //}

        //public virtual void findMinMaxX(int y)
        //{
        //    xMin = System.Int32.MaxValue;
        //    xMax = System.Int32.MinValue;
        //    for (int i = rasterCount; --i >= 0; )
        //    {
        //        if (yRaster[i] == y)
        //        {
        //            int x = xRaster[i];
        //            if (x < xMin)
        //            {
        //                xMin = x;
        //                zXMin = zRaster[i];
        //            }
        //            if (x > xMax)
        //            {
        //                xMax = x;
        //                zXMax = zRaster[i];
        //            }
        //            //if (y == 0) {
        //            //}
        //        }
        //        if (yRaster[i] == -y)
        //        {
        //            // 0 will run through here too
        //            int x = -xRaster[i];
        //            if (x < xMin)
        //            {
        //                xMin = x;
        //                zXMin = -zRaster[i];
        //            }
        //            if (x > xMax)
        //            {
        //                xMax = x;
        //                zXMax = -zRaster[i];
        //            }
        //        }
        //    }
        //}

        //public virtual void renderFlatEndcap(bool tCylinder)
        //{
        //    if (dzB == 0 || (!tCylinder && dzB < 0))
        //        return;
        //    int xT = xA, yT = yA, zT = zA;
        //    if (dzB < 0)
        //    {
        //        xT += dxB; yT += dyB; zT += dzB;
        //    }

        //    findMinMaxY();
        //    for (int y = yMin; y <= yMax; ++y)
        //    {
        //        findMinMaxX(y);
        //        /*
        //        System.out.println("endcap y="+y+" xMin="+xMin+" xMax="+xMax);
        //        */
        //        int count = xMax - xMin + 1;
        //        g3d.setColorNoisy(colixEndcap, intensityEndcap);
        //        g3d.plotPixelsClipped(count, xT + xMin, yT + y, zT - zXMin - 1, zT - zXMax - 1, null, null);
        //    }
        //}

        //public virtual void renderSphericalEndcaps()
        //{
        //    g3d.fillSphereCentered(colixA, diameter, xA, yA, zA + 1);
        //    g3d.fillSphereCentered(colixB, diameter, xA + dxB, yA + dyB, zA + dzB + 1);
        //}

        //internal int xTip, yTip, zTip;

        //public virtual void renderCone(short colix, sbyte endcap, int diameter, int xA, int yA, int zA, int xTip, int yTip, int zTip)
        //{
        //    dxB = (this.xTip = xTip) - (this.xA = xA);
        //    dyB = (this.yTip = yTip) - (this.yA = yA);
        //    dzB = (this.zTip = zTip) - (this.zA = zA);

        //    this.colixA = colix;
        //    this.shadesA = g3d.getShades(colix);
        //    this.isScreenedA = (colixA & Graphics3D.TRANSLUCENT_MASK) != 0;
        //    int intensityTip = Shade3D.calcIntensity(dxB, dyB, -dzB);
        //    g3d.plotPixelClipped(shadesA[intensityTip], isScreenedA, xTip, yTip, zTip);

        //    this.diameter = diameter;
        //    if (diameter <= 1)
        //    {
        //        if (diameter == 1)
        //            g3d.plotLineDelta(colixA, isScreenedA, colixA, isScreenedA, xA, yA, zA, dxB, dyB, dzB);
        //        return;
        //    }
        //    this.endcaps = endcap;
        //    calcArgbEndcap(false);
        //    generateBaseEllipse();
        //    if (endcaps == Graphics3D.ENDCAPS_FLAT)
        //        renderFlatEndcap(false);
        //    for (int i = rasterCount; --i >= 0; )
        //        plotRasterCone(i);
        //}

        //public virtual void plotRasterCone(int i)
        //{
        //    int x = xRaster[i];
        //    int y = yRaster[i];
        //    int z = zRaster[i];
        //    int xUp = xA + x, yUp = yA + y, zUp = zA - z;
        //    int xDn = xA - x, yDn = yA - y, zDn = zA + z;

        //    if (tEndcapOpen)
        //    {
        //        g3d.plotPixelClipped(argbEndcap, isScreenedA, xUp, yUp, zUp);
        //        g3d.plotPixelClipped(argbEndcap, isScreenedA, xDn, yDn, zDn);
        //    }
        //    /*
        //    System.out.println("plotRaster " + i + " (" + xRaster[i] + "," +
        //    yRaster[i] + "," + zRaster[i] + ")" +
        //    " iUp=" + iUp);
        //    */
        //    int fp8Up = fp8IntensityUp[i];
        //    g3d.plotLineDelta(shadesA, isScreenedA, shadesA, isScreenedA, fp8Up, xUp, yUp, zUp, xTip - xUp, yTip - yUp, zTip - zUp);
        //    if (!(endcaps == Graphics3D.ENDCAPS_FLAT && dzB > 0))
        //    {
        //        int argb = shadesA[0];
        //        g3d.plotLineDelta(argb, isScreenedA, argb, isScreenedA, xDn, yDn, zDn, xTip - xDn, yTip - yDn, zTip - zDn);
        //    }
        //}

        //public virtual void calcArgbEndcap(bool tCylinder)
        //{
        //    tEndcapOpen = false;
        //    if ((endcaps == Graphics3D.ENDCAPS_SPHERICAL) || (dzB == 0) || (!tCylinder && dzB < 0))
        //        return;
        //    xEndcap = xA; yEndcap = yA; zEndcap = zA;
        //    int[] shadesEndcap;
        //    if (dzB >= 0)
        //    {
        //        intensityEndcap = Shade3D.calcIntensity(-dxB, -dyB, dzB);
        //        colixEndcap = colixA;
        //        shadesEndcap = shadesA;
        //        //      System.out.println("endcap is A");
        //    }
        //    else
        //    {
        //        intensityEndcap = Shade3D.calcIntensity(dxB, dyB, -dzB);
        //        colixEndcap = colixB;
        //        shadesEndcap = shadesB;
        //        xEndcap += dxB; yEndcap += dyB; zEndcap += dzB;
        //        //      System.out.println("endcap is B");
        //    }
        //    // limit specular glare on endcap
        //    if (intensityEndcap > Graphics3D.intensitySpecularSurfaceLimit)
        //        intensityEndcap = Graphics3D.intensitySpecularSurfaceLimit;
        //    argbEndcap = shadesEndcap[intensityEndcap];
        //    tEndcapOpen = (endcaps == Graphics3D.ENDCAPS_OPEN);
        //}
    }
}