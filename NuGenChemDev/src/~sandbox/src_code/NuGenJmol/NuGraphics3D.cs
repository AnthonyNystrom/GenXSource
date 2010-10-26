using System;
using System.Collections.Generic;
using System.Text;
using javax.vecmath;
using Org.Jmol.G3d;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace NuGenJmol
{
    class NuGraphics3D
    {
        NuHermite3D hermite3d;
        Cylinder3D cylinder3d;

        bool inGreyscaleMode;

        short colixCurrent;
        int[] shadesCurrent;
        int argbCurrent;
        bool isTranslucent;
        int argbNoisyUp, argbNoisyDn;

        /* entries 0 through 3 are reserved and are special
           TRANSLUCENT and OPAQUE are used to inherit
           the underlying color, but change the translucency

           Note that colors are not actually translucent. Rather,
           they are 'screened' where every-other pixel is turned
           on. 
        */
        public static readonly short TRANSLUCENT_MASK = 0x4000;
        public static readonly short OPAQUE_MASK = (short)~TRANSLUCENT_MASK;
        public static readonly short CHANGABLE_MASK = -32768; // negative
        public static readonly short UNMASK_CHANGABLE_TRANSLUCENT = 0x3FFF;

        public static readonly short NULL_COLIX   = 0;
        public static readonly short TRANSLUCENT  = 1;
        public static readonly short OPAQUE       = 2;
        public static readonly short UNRECOGNIZED = 3;
        public static readonly short SPECIAL_COLIX_MAX = 4;

        public static readonly short BLACK       = 4;
        public static readonly short ORANGE      = 5;
        public static readonly short PINK        = 6;
        public static readonly short BLUE        = 7;
        public static readonly short WHITE       = 8;
        public static readonly short CYAN        = 9;
        public static readonly short RED         = 10;
        public static readonly short GREEN       = 11;
        public static readonly short GRAY        = 12;
        public static readonly short SILVER      = 13;
        public static readonly short LIME        = 14;
        public static readonly short MAROON      = 15;
        public static readonly short NAVY        = 16;
        public static readonly short OLIVE       = 17;
        public static readonly short PURPLE      = 18;
        public static readonly short TEAL        = 19;
        public static readonly short MAGENTA     = 20;
        public static readonly short YELLOW      = 21;
        public static readonly short HOTPINK     = 22;
        public static readonly short GOLD        = 23;

        /****************************************************************
         * changable colixes
         * give me a short ID and a color, and I will give you a colix
         * later, you can reassign the color if you want
         ****************************************************************/

        short[] changableColixMap = new short[16];


        public const sbyte ENDCAPS_NONE = 0;
        public const sbyte ENDCAPS_OPEN = 1;
        public const sbyte ENDCAPS_FLAT = 2;
        public const sbyte ENDCAPS_SPHERICAL = 3;


        Device gDevice;
        NuSceneBuffer3D currentSceneBuffer;

        public NuGraphics3D(Device gDevice)
        {
            this.gDevice = gDevice;

            currentSceneBuffer = new NuSceneBuffer3D();

            hermite3d = new NuHermite3D(this);
            cylinder3d = new Cylinder3D(this);
        }

        public NuSceneBuffer3D SceneBuffer
        {
            get { return currentSceneBuffer; }
            set { currentSceneBuffer = value; }
        }

        /// <summary> Return a greyscale rgb value 0-FF using NTSC color luminance algorithm
        /// <p>
        /// the alpha component is set to 0xFF. If you want a value in the
        /// range 0-255 then & the result with 0xFF;
        /// 
        /// </summary>
        /// <param name="rgb">the rgb value
        /// </param>
        /// <returns> a grayscale value in the range 0 - 255 decimal
        /// </returns>
        public static int calcGreyscaleRgbFromRgb(int rgb)
        {
            int grey = ((2989 * ((rgb >> 16) & 0xFF)) + (5870 * ((rgb >> 8) & 0xFF)) + (1140 * (rgb & 0xFF)) + 5000) / 10000;
            int greyRgb = (grey << 16) | (grey << 8) | grey | unchecked((int)0xFF000000);
            return greyRgb;
        }

        /// <summary>
        /// sets current color from colix color index
        /// </summary>
        /// <param name="colix">the color index</param>
        public void setColix(short colix)
        {
            colixCurrent = colix;
            shadesCurrent = getShades(colix);
            argbCurrent = argbNoisyUp = argbNoisyDn = getColixArgb(colix);
            isTranslucent = (colix & TRANSLUCENT_MASK) != 0;
        }

        public int getColixArgb(short colix)
        {
            if (colix < 0)
                colix = changableColixMap[colix & UNMASK_CHANGABLE_TRANSLUCENT];
            if (!inGreyscaleMode)
                return Colix.getArgb(colix);
            return Colix.getArgbGreyscale(colix);
        }

        public int[] getShades(short colix)
        {
            if (colix < 0)
                colix = changableColixMap[colix & UNMASK_CHANGABLE_TRANSLUCENT];
            if (!inGreyscaleMode)
                return Colix.getShades(colix);
            return Colix.getShadesGreyscale(colix);
        }



        /// <summary>
        /// fills a solid sphere
        /// </summary>
        /// <param name="colix">the color index</param>
        /// <param name="diameter">pixel count</param>
        /// <param name="center">javax.vecmath.Point3i defining the center</param>
        public void fillSphereCentered(short colix, int diameter, Point3i center)
        {
            fillSphereCentered(colix, diameter, center.X, center.Y, center.Z);
        }

        /// <summary>
        /// fills a solid sphere
        /// </summary>
        /// <param name="colix">the color index</param>
        /// <param name="diameter">pixel count</param>
        /// <param name="center">a javax.vecmath.Point3f ... floats are casted to ints</param>
        public void fillSphereCentered(short colix, int diameter, Point3f center)
        {
            fillSphereCentered(colix, diameter, center.x, center.y, center.z);
        }

        /// <summary>
        /// fills a solid sphere
        /// </summary>
        /// <param name="colix">the color index</param>
        /// <param name="diameter">pixel count</param>
        /// <param name="x">center x</param>
        /// <param name="y">center y</param>
        /// <param name="z">center z</param>
        public void fillSphereCentered(short colix, float diameter, float x, float y, float z)
        {
            if (diameter <= 1)
            {
            //    plotPixelClipped(colix, x, y, z);
            }
            else
            {
                NuSceneBuffer3D.NuBufferMeshItem mesh = new NuSceneBuffer3D.NuBufferMeshItem();
                mesh.mesh = Mesh.Sphere(gDevice, /*diameter*/1.0f, 3, 3);
                mesh.translation = new Vector3(x, y, z);
                currentSceneBuffer.meshes.Add(mesh);
                //sphere3d.render(getShades(colix), ((colix & TRANSLUCENT_MASK) != 0), diameter, x, y, z);
            }
        }

        //public void plotPixelClipped(Point3i a)
        //{
        //    throw new Exception("The method or operation is not implemented.");
        //}

        //public void plotPixelClipped(short colix, int x, int y, int z)
        //{
        //    if (x < 0 || x >= width || y < 0 || y >= height || z < slab || z > depth)
        //        return;
        //    int offset = y * width + x;
        //    if (z < zbuf[offset])
        //    {
        //        zbuf[offset] = z;
        //        pbuf[offset] = getColixArgb(colix);
        //    }
        //}

        #region Triangles

        public void fillTriangle(Point3f screenA, Point3f screenB, Point3f screenC)
        {
            currentSceneBuffer.triangles.Add(new Vector3(screenA.x, screenA.y, screenA.z));
            currentSceneBuffer.triangles.Add(new Vector3(screenB.x, screenB.y, screenB.z));
            currentSceneBuffer.triangles.Add(new Vector3(screenC.x, screenC.y, screenC.z));
        }

        public void fillTriangle(Point3i screenA, Point3i screenB, Point3i screenC)
        {
            currentSceneBuffer.triangles.Add(new Vector3(screenA.x, screenA.y, screenA.z));
            currentSceneBuffer.triangles.Add(new Vector3(screenB.x, screenB.y, screenB.z));
            currentSceneBuffer.triangles.Add(new Vector3(screenC.x, screenC.y, screenC.z));
        }

        public void fillTriangle(short colix, Point3i screenA, Point3i screenB, Point3i screenC)
        {
            fillTriangle(screenA, screenB, screenC);
        }

        public void FillTriangleStrip(Vector3[] points)
        {
            currentSceneBuffer.triangleStrips.Add(points);
        }

        #endregion

        public void drawLine(Point3i pointA, Point3i pointB)
        {
            currentSceneBuffer.lines.Add(new Vector3(pointA.x, pointA.y, pointA.z));
            currentSceneBuffer.lines.Add(new Vector3(pointB.x, pointB.y, pointB.z));
        }

        public void fillQuadrilateral(short colix, Point3f screenA, Point3f screenB, Point3f screenC, Point3f screenD)
        {
            //setColorNoisy(colix, calcIntensityScreen(screenA, screenB, screenC));
            fillTriangle(screenA, screenB, screenC);
            fillTriangle(screenA, screenC, screenD);
        }

        public void drawHermite(bool fill, bool border, short colix, int tension,
                                Point3i s0, Point3i s1, Point3i s2, Point3i s3,
                                Point3i s4, Point3i s5, Point3i s6, Point3i s7)
        {
            hermite3d.render2(fill, border, colix, tension, s0, s1, s2, s3, s4, s5, s6, s7);
        }

        public void fillHermite(short colix, int tension, int diameterBeg, int diameterMid, int diameterEnd, Point3i s0, Point3i s1, Point3i s2, Point3i s3)
        {
            hermite3d.render(true, colix, tension, diameterBeg, diameterMid, diameterEnd, s0, s1, s2, s3);
        }

        #region Cylinders

        //public void fillCylinder(short colixA, short colixB, sbyte endcaps, int diameter, int xA, int yA, int zA, int xB, int yB, int zB)
        //{
        //    //cylinder3d.render(colixA, colixB, endcaps, diameter, xA, yA, zA, xB, yB, zB);
        //}

        //public void fillCylinder(short colix, sbyte endcaps, int diameter, int xA, int yA, int zA, int xB, int yB, int zB)
        //{
        //    //cylinder3d.render(colix, colix, endcaps, diameter, xA, yA, zA, xB, yB, zB);
        //}

        //public void fillCylinder(short colix, sbyte endcaps, int diameter, Point3i screenA, Point3i screenB)
        //{
        //    //cylinder3d.render(colix, colix, endcaps, diameter, screenA.x, screenA.y, screenA.z, screenB.x, screenB.y, screenB.z);
        //}

        #endregion

        public void Flush()
        {
            hermite3d.Flush();
        }

        public void Clear()
        {
            currentSceneBuffer = new NuSceneBuffer3D();
        }
    }
}