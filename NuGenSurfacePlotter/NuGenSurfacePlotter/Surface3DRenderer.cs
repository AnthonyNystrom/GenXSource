using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using NuGenSurfacePlotter.Colors;
using NuGenCRBase.AvalonBridge;
using Microsoft.DirectX;
using System.Windows.Media.Media3D;
using Microsoft.DirectX.Direct3D;

namespace NuGenSurfacePlotter.Rendering
{
    class Surface3DRenderer
    {
        double screenDistance, sf, cf, st, ct, R, A, B, C, D; //transformations coeficients
        double density = 0.5f;
        Color penColor = Color.Black;
        PointF startPoint = new PointF(-20, -20);
        PointF endPoint = new PointF(20, 20);
        RendererFunction function = defaultFunction;
        ColorSchema colorSchema = ColorSchema.Autumn;
        int[] clrSchemeIcache;

        #region Properties

        /// <summary>
        /// Surface spanning net density
        /// </summary>
        public double Density
        {
            get { return density; }
            set { density = value; }
        }

        /// <summary>
        /// Quadrilateral pen color
        /// </summary>
        public Color PenColor
        {
            get { return penColor; }
            set { penColor = value; }
        }

        public PointF StartPoint
        {
            get { return startPoint; }
            set { startPoint = value; }
        }

        public PointF EndPoint
        {
            get { return endPoint; }
            set { endPoint = value; }
        }

        public RendererFunction Function
        {
            get { return function; }
            set { function = value; }
        }

        public ColorSchema ColorSchema
        {
            get { return colorSchema; }
            set { colorSchema = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Surface3DRenderer"/> class. Calculates transformations coeficients.
        /// </summary>
        /// <param name="obsX">Observator's X position</param>
        /// <param name="obsY">Observator's Y position</param>
        /// <param name="obsZ">Observator's Z position</param>
        /// <param name="xs0">X coordinate of screen</param>
        /// <param name="ys0">Y coordinate of screen</param>
        /// <param name="screenWidth">Drawing area width in pixels.</param>
        /// <param name="screenHeight">Drawing area height in pixels.</param>
        /// <param name="screenDistance">The screen distance.</param>
        /// <param name="screenWidthPhys">Width of the screen in meters.</param>
        /// <param name="screenHeightPhys">Height of the screen in meters.</param>
        public Surface3DRenderer(double obsX, double obsY, double obsZ, int xs0, int ys0, int screenWidth, int screenHeight, double screenDistance, double screenWidthPhys, double screenHeightPhys)
        {
            ReCalculateTransformationsCoeficients(obsX, obsY, obsZ, xs0, ys0, screenWidth, screenHeight, screenDistance, screenWidthPhys, screenHeightPhys);
        }

        public void ReCalculateTransformationsCoeficients(double obsX, double obsY, double obsZ, int xs0, int ys0, int screenWidth, int screenHeight, double screenDistance, double screenWidthPhys, double screenHeightPhys)
        {
            double r1, a;

            if (screenWidthPhys <= 0)//when screen dimensions are not specified
                screenWidthPhys = screenWidth * 0.0257 / 72.0;        //0.0257 m = 1 inch. Screen has 72 px/inch
            if (screenHeightPhys <= 0)
                screenHeightPhys = screenHeight * 0.0257 / 72.0;

            r1 = obsX * obsX + obsY * obsY;
            a = Math.Sqrt(r1);//distance in XY plane
            R = Math.Sqrt(r1 + obsZ * obsZ);//distance from observator to center
            if (a != 0) //rotation matrix coeficients calculation
            {
                sf = obsY / a;//sin( fi)
                cf = obsX / a;//cos( fi)
            }
            else
            {
                sf = 0;
                cf = 1;
            }
            st = a / R;//sin( teta)
            ct = obsZ / R;//cos( teta)

            //linear tranfrormation coeficients
            A = screenWidth / screenWidthPhys;
            B = xs0 + A * screenWidthPhys / 2.0;
            C = -(double)screenHeight / screenHeightPhys;
            D = ys0 - C * screenHeightPhys / 2.0;

            this.screenDistance = screenDistance;
        }

        /// <summary>
        /// Performs projection. Calculates screen coordinates for 3D point.
        /// </summary>
        /// <param name="x">Point's x coordinate.</param>
        /// <param name="y">Point's y coordinate.</param>
        /// <param name="z">Point's z coordinate.</param>
        /// <returns>Point in 2D space of the screen.</returns>
        public PointF Project(double x, double y, double z)
        {
            double xn, yn, zn;//point coordinates in computer's frame of reference

            //transformations
            xn = -sf * x + cf * y;
            yn = -cf * ct * x - sf * ct * y + st * z;
            zn = -cf * st * x - sf * st * y - ct * z + R;

            if (zn == 0) zn = 0.01;

            //Tales' theorem
            return new PointF((float)(A * xn * screenDistance / zn + B), (float)(C * yn * screenDistance / zn + D));
        }

        public void RenderSurface(Graphics graphics)
        {
            // need opt
            SolidBrush[] brushes = new SolidBrush[colorSchema.Length];
            for (int i = 0; i < brushes.Length; i++)
                brushes[i] = new SolidBrush(colorSchema[i]);

            double z1, z2;
            PointF[] polygon = new PointF[4];

            double xi = startPoint.X, yi, minZ = double.PositiveInfinity, maxZ = double.NegativeInfinity;
            double[,] mesh = new double[(int)((endPoint.X - startPoint.X) / density + 1), (int)((endPoint.Y - startPoint.Y) / density + 1)];
            PointF[,] meshF = new PointF[mesh.GetLength(0), mesh.GetLength(1)];
            for (int x = 0; x < mesh.GetLength(0); x++)
            {
                yi = startPoint.Y;
                for (int y = 0; y < mesh.GetLength(1); y++)
                {
                    double zz = function(xi, yi);
                    mesh[x, y] = zz;
                    meshF[x, y] = Project(xi, yi, zz);
                    yi += density;

                    if (minZ > zz) minZ = zz;
                    if (maxZ < zz) maxZ = zz;
                }
                xi += density;
            }

            double cc = (maxZ - minZ) / (brushes.Length - 1.0);

            using (Pen pen = new Pen(penColor))
                for (int x = 0; x < mesh.GetLength(0) - 1; x++)
                {
                    for (int y = 0; y < mesh.GetLength(1) - 1; y++)
                    {
                        z1 = mesh[x, y];
                        z2 = mesh[x, y + 1];

                        polygon[0] = meshF[x, y];
                        polygon[1] = meshF[x, y + 1];
                        polygon[2] = meshF[x + 1, y + 1];
                        polygon[3] = meshF[x + 1, y];

                        graphics.SmoothingMode = SmoothingMode.None;
                        graphics.FillPolygon(brushes[(int)(((z1 + z2) / 2.0 - minZ) / cc)], polygon);

                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        graphics.DrawPolygon(pen, polygon);
                    }
                }
            for (int i = 0; i < brushes.Length; i++)
                brushes[i].Dispose();
        }

        public static RendererFunction GetFunctionHandle(string formula)
        {
            CompiledFunction fn = FunctionCompiler.Compile(2, formula);
            return new RendererFunction(delegate(double x, double y)
            {
                return fn(x, y);
            });
        }

        public void SetFunction(string formula)
        {
            function = GetFunctionHandle(formula);
        }

        private static double defaultFunction(double a, double b)
        {
            double an = a, bn = b, anPlus1;
            short iter = 0;
            do
            {
                anPlus1 = (an + bn) / 2.0;
                bn = Math.Sqrt(an * bn);
                an = anPlus1;
                if (iter++ > 1000) return an;
            } while (Math.Abs(an - bn)<0.1);
            return an;
        }

        public ABModel3D[] ToAvalonBridgeModel()
        {
            ABModel3D[] models = new ABModel3D[2];
            models[0] = new ABModel3D();
            
            // build geometry
            double xi = startPoint.X, yi, minZ = double.PositiveInfinity, maxZ = double.NegativeInfinity;

            int numPointsX = (int)((endPoint.X - startPoint.X) / density + 1);
            int numPointsY = (int)((endPoint.Y - startPoint.Y) / density + 1);

            ABGeometry3D geometry = models[0].Geometry = new ABGeometry3D();
            geometry.PrimType = PrimitiveType.TriangleList;
            int numVerts = numPointsX * numPointsY;

            int vIdx = 0;
            Vector3[] points = new Vector3[numVerts];
            for (int x = 0; x < numPointsX; x++)
            {
                yi = startPoint.Y;
                for (int y = 0; y < numPointsY; y++)
                {
                    double zz = function(xi, yi);
                    points[vIdx++] = new Vector3((float)xi, (float)zz, (float)yi);

                    yi += density;
                    if (minZ > zz) minZ = zz;
                    if (maxZ < zz) maxZ = zz;
                }
                xi += density;
            }
            //e1 = verts[vIdx-2].Position - verts[vIdx-3].Position;
            //e2 = verts[vIdx-1].Position - verts[vIdx-3].Position;
            //faceNormal = Vector3::Normalize(Vector3::Cross(e1, e2));
            
            // tri indices
            int numTris = (numPointsX - 1) * (numPointsY - 1) * 2;
            geometry.Vertices = new Vector3[numTris * 3];
            geometry.VertexClrs = new int[geometry.Vertices.Length];
            int tIdx = 0;
            vIdx = 0;
            float z1, z2;
            double cc = (maxZ - minZ) / (colorSchema.Length - 1.0);
            int cacheIdx = 0;
            clrSchemeIcache = new int[numTris];
            for (int x = 0; x < numPointsX - 1; x++)
            {
                for (int y = 0; y < numPointsY - 1; y++)
                {
                    z1 = points[tIdx].Y;
                    z2 = points[tIdx + 1].Y;
                    int clrIdx = clrSchemeIcache[cacheIdx++] = (int)(((z1 + z2) / 2.0 - minZ) / cc);
                    int triClr = colorSchema[clrIdx].ToArgb();

                    //Vector3 e1 = points[tIdx + 1] - points[tIdx];
                    //Vector3 e2 = points[tIdx + 1 + numPointsX] - points[tIdx];
                    //Vector3 faceNormal = Vector3.Normalize(Vector3.Cross(e1, e2));

                    //geometry.Normals[vIdx] = faceNormal;
                    geometry.VertexClrs[vIdx] = triClr;
                    geometry.Vertices[vIdx++] = points[tIdx];
                    //geometry.Normals[vIdx] = faceNormal;
                    geometry.VertexClrs[vIdx] = triClr;
                    geometry.Vertices[vIdx++] = points[tIdx + 1];
                    //geometry.Normals[vIdx] = faceNormal;
                    geometry.VertexClrs[vIdx] = triClr;
                    geometry.Vertices[vIdx++] = points[tIdx + 1 + numPointsY];

                    //z1 = points[tIdx].Y;
                    //z2 = points[tIdx + 1 + numPointsX].Y;
                    //triClr = colorSchema[(int)(((z1 + z2) / 2.0 - minZ) / cc)].ToArgb();

                    //e1 = points[tIdx + 1 + numPointsX] - points[tIdx];
                    //e2 = points[tIdx + numPointsX] - points[tIdx];
                    //faceNormal = Vector3.Normalize(Vector3.Cross(e1, e2));

                    //geometry.Normals[vIdx] = faceNormal;
                    geometry.VertexClrs[vIdx] = triClr;
                    geometry.Vertices[vIdx++] = points[tIdx];
                    //geometry.Normals[vIdx] = faceNormal;
                    geometry.VertexClrs[vIdx] = triClr;
                    geometry.Vertices[vIdx++] = points[tIdx + 1 + numPointsY];
                    //geometry.Normals[vIdx] = faceNormal;
                    geometry.VertexClrs[vIdx] = triClr;
                    geometry.Vertices[vIdx++] = points[tIdx + numPointsY];

                    tIdx++;
                }
                tIdx++;
            }

            // now outlines
            models[1] = new ABModel3D();
            geometry = models[1].Geometry = new ABGeometry3D();
            geometry.PrimType = PrimitiveType.LineStrip;
            geometry.Vertices = new Vector3[numVerts * 2];
            geometry.Clr = Color.Black.ToArgb();

            // TODO: Use tri strips??
            vIdx = 0;
            int pos = 0;
            // do x strips
            for (int y = 0; y < numPointsY; y++)
            {
                for (int x = 0; x < numPointsX; x++)
                {
                    geometry.Vertices[vIdx] = points[pos];
                    pos += numPointsY;
                    vIdx++;
                }
                y++;
                if (y < numPointsY)
                {
                    pos -= numPointsY;
                    pos++;
                    for (int x = 0; x < numPointsX; x++)
                    {
                        geometry.Vertices[vIdx] = points[pos];
                        pos -= numPointsY;
                        vIdx++;
                    }
                    pos = y + 1;
                }
            }
            // do y strips
            pos = 0;
            for (int x = numPointsX - 1; x >= 0; x--)
            {
                pos = ((x + 1) * numPointsY) - 1;
                for (int y = 0; y < numPointsY; y++)
                {
                    geometry.Vertices[vIdx] = points[pos--];
                    vIdx++;
                }
                x--;
                if (x >= 0)
                {
                    pos -= numPointsY - 1;
                    for (int y = 0; y < numPointsY; y++)
                    {
                        //Vector3 p = points[pos++];
                        geometry.Vertices[vIdx++] = points[pos++];
                    }
                }
            }

            return models;
        }

        public void UpdateAvalonBridgeModelClrs(ABModel3D[] models)
        {
            int[] clrs = models[0].Geometry.VertexClrs;
            int cacheIdx = 0;
            for (int i = 0; i < clrs.Length; i += 6)
            {
                clrs[i] = clrs[i + 1] = clrs[i + 2] = clrs[i + 3] = clrs[i + 4] = clrs[i + 5] = colorSchema[clrSchemeIcache[cacheIdx++]].ToArgb();
            }
        }
    }

    public delegate double RendererFunction(double x, double y);

    public struct Point3D
    {
        public double x, y, z;

        public Point3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
}