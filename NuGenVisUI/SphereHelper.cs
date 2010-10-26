using Microsoft.DirectX;

namespace Genetibase.VisUI.Maths
{
    public class SphereHelper
    {
        public class SphereN
        {
            public Vector3[] Positions;
            public Vector3[] Normals;
            public Vector2[] TexCoords;
        }

        public enum EndType
        {
            Open,
            Flat,
            Rounded
        }

        private static Matrix Vector3ToMatrixRot(Vector3 vector)
        {
            float x, y;
            Vector3ToEulerAngles(vector, out x, out y);
            return Matrix.RotationX(x) * Matrix.RotationY(y);
        }

        private static void Vector3ToEulerAngles(Vector3 vector, out float xAngle, out float yAngle)
        {
            double x = vector.X;
            double y = vector.Y;
            double z = vector.Z;

            double alpha = (z == 0) ? System.Math.PI / 2 : System.Math.Atan(x / z);

            double r = (alpha == 0) ? z : x / System.Math.Sin(alpha);

            float sign = 1f;
            if (z != 0)
                sign *= System.Math.Sign(z);
            else if (x != 0)
                sign *= System.Math.Sign(x);
            if (y != 0) sign *= System.Math.Sign(y);

            double theta = -sign * System.Math.Abs((r == 0) ? System.Math.PI / 2 : System.Math.Atan(y / r));

            xAngle = (float)alpha;
            yAngle = (float)theta;
        }

        public static Vector3[] CalcSphereCorkscrewPointsCCW()
        {
            return null;
        }

        public static Vector3[] CalcSpherePoints(int numSlices, int numStacks, float radius,
                                                 Vector3 center, out Vector2[] texCoords)
        {
            // calc half-profile for stacks (ZY plane)
            int numStackPoints = numStacks + 2;

            Vector2[] profile;
            CircleHelper.CalcArcPointsCCW(numStackPoints, radius, new Vector2(center.Z, center.Y),
                             180f, out profile);

            // create stacks
            Vector3[] sphere = new Vector3[(numStacks * numSlices) + 2];
            texCoords = new Vector2[(numStacks * numSlices) + 2];
            Vector2 center2D = new Vector2(center.Z, center.X);
            int pIdx = 1;
            //float sHeight = (radius * 2f) / ((float)numStacks + 1);
            for (int stack = 0; stack < numStacks; stack++)
            {
                float sRadius = profile[stack + 1].Y - center.Z;
                float vPos = profile[stack + 1].X;//(center.Y + radius) - (sHeight * (stack+1));
                Vector2[] points;
                CircleHelper.CalcCirclePointsCCW(numSlices + 1, sRadius, center2D, out points);
                // copy into 3D
                for (int p = 0; p < numSlices; p++)
                {
                    sphere[pIdx + p] = new Vector3(points[p].Y, vPos, points[p].X);
                    texCoords[pIdx + p].X = p / (float)numSlices;
                    texCoords[pIdx + p].Y = (float)stack / (float)numStacks;
                }
                pIdx += numSlices;
            }
            // top & bottom points
            sphere[0] = new Vector3(center.X, center.Y + radius, center.Z);
            texCoords[0].X = 0.5f; texCoords[0].Y = 0;
            sphere[sphere.Length - 1] = new Vector3(center.X, center.Y - radius, center.Z);
            texCoords[sphere.Length - 1].X = 0.5f; texCoords[sphere.Length - 1].Y = 1;

            return sphere;
        }

        public static Vector3[] CalcSpherePointsTriStrip(int numSlices, int numStacks, float radius,
                                                         Vector3 center)
        {
            return null;
        }

        //        public static Vector3[] CalcSphereTriangles(int numSlices, float radius, Vector3 center,
        //                                                    ref Vector3[] normalsOut, bool calcNormals,
        //                                                    ref Vector2[] texCoordsOut, bool texCoords)
        //        {
        //            double theta1, theta2, theta3;
        //            Vector3 e, p;
        //
        //            if (r < 0)
        //                r = -r;
        //            if (n < 0)
        //                n = -n;
        //
        //            int vIdx = 0;
        //            for (int j = 0; j < n / 2; j++)
        //            {
        //                theta1 = j * TWOPI / n - PID2;
        //                theta2 = (j + 1) * TWOPI / n - PID2;
        //
        //                for (int i = 0; i <= n; i++)
        //                {
        //                    theta3 = i * TWOPI / n;
        //
        //                    e.X = (float)(Math.Cos(theta2) * Math.Cos(theta3));
        //                    e.Y = (float)Math.Sin(theta2);
        //                    e.Z = (float)(Math.Cos(theta2) * Math.Sin(theta3));
        //                    p.X = (float)(c.X + r * e.X);
        //                    p.Y = (float)(c.Y + r * e.Y);
        //                    p.Z = (float)(c.Z + r * e.Z);
        //
        //                    points[vIdx].Normal = new Vector3(e.X, e.Y, e.Z);
        //                    points[vIdx].Tu = (float)(i / (double)n);
        //                    points[vIdx].Tv = (float)(2 * (j + 1) / (double)n);
        //
        //                    points[vIdx].Position = new Vector3(p.X, p.Y, p.Z);
        //                    vIdx++;
        //
        //                    e.X = (float)(Math.Cos(theta1) * Math.Cos(theta3));
        //                    e.Y = (float)Math.Sin(theta1);
        //                    e.Z = (float)(Math.Cos(theta1) * Math.Sin(theta3));
        //                    p.X = (float)(c.X + r * e.X);
        //                    p.Y = (float)(c.Y + r * e.Y);
        //                    p.Z = (float)(c.Z + r * e.Z);
        //
        //                    points[vIdx].Normal = new Vector3(e.X, e.Y, e.Z);
        //                    points[vIdx].Tu = (float)(i / (double)n);
        //                    points[vIdx].Tv = (float)(2 * j / (double)n);
        //
        //                    points[vIdx].Position = new Vector3(p.X, p.Y, p.Z);
        //                    vIdx++;
        //                }
        //            }
        //        }

        public static Vector3[] CalcSphereTriangles(int numSlices, int numStacks, float radius, Vector3 center,
                                                    ref Vector3[] normalsOut, bool calcNormals,
                                                    ref Vector2[] texCoordsOut, bool texCoords)
        {
            Vector2[] pointTexCoords = null;
            Vector3[] points = CalcSpherePoints(numSlices, numStacks, radius, center, out pointTexCoords);

            // build triangles
            Vector3[] tris = new Vector3[((numSlices * (numStacks - 1) * 2) + (numSlices * 2)) * 3];

            if (calcNormals)
                normalsOut = new Vector3[tris.Length];

            float slicesMul = 1f / (float)numSlices;
            Vector3[] pointNormals = null;
            int[] pointMult = null;
            int[] triPRefs = null;
            if (calcNormals)
            {
                pointNormals = new Vector3[points.Length];
                pointMult = new int[points.Length];
                triPRefs = new int[tris.Length];
            }

            // top end
            int tIdx = 0;
            for (int slice = 0; slice < numSlices; slice++)
            {
                triPRefs[tIdx] = 0;
                triPRefs[tIdx + 1] = slice + 1;
                if (slice == numSlices - 1)
                    triPRefs[tIdx + 2] = 1;
                else
                    triPRefs[tIdx + 2] = slice + 2;

                tIdx += 3;
            }

            // stacks
            int pIdx = 1;
            for (int stack = 0; stack < numStacks - 1; stack++)
            {
                for (int slice = 0; slice < numSlices - 1; slice++)
                {
                    triPRefs[tIdx] = pIdx;
                    triPRefs[tIdx + 1] = pIdx + numSlices;
                    triPRefs[tIdx + 2] = pIdx + numSlices + 1;

                    triPRefs[tIdx + 3] = pIdx;
                    triPRefs[tIdx + 5] = pIdx + 1;
                    triPRefs[tIdx + 4] = pIdx + numSlices + 1;

                    tIdx += 6;
                    pIdx++;
                }
                // last one (set)
                triPRefs[tIdx] = pIdx;
                triPRefs[tIdx + 1] = pIdx + numSlices;
                triPRefs[tIdx + 2] = pIdx + 1;

                triPRefs[tIdx + 3] = pIdx;
                triPRefs[tIdx + 5] = pIdx - numSlices + 1;
                triPRefs[tIdx + 4] = pIdx + 1;

                tIdx += 6;
                pIdx++;
            }

            // bottom end
            for (int slice = points.Length - numSlices - 1; slice < points.Length - 1; slice++)
            {
                triPRefs[tIdx] = points.Length - 1;
                if (slice == points.Length - 2)
                    triPRefs[tIdx + 1] = points.Length - numSlices - 1;
                else
                    triPRefs[tIdx + 1] = slice + 1;
                triPRefs[tIdx + 2] = slice;

                tIdx += 3;
            }

            texCoordsOut = new Vector2[tris.Length];

            // calc normals
            if (calcNormals)
            {
                for (int tri = 0; tri < tris.Length; tri += 3)
                {
                    int p1 = triPRefs[tri];
                    int p2 = triPRefs[tri + 1];
                    int p3 = triPRefs[tri + 2];

                    Vector3 v0 = points[p1];
                    Vector3 v1 = points[p2];
                    Vector3 v2 = points[p3];

                    tris[tri] = points[p1];
                    texCoordsOut[tri] = pointTexCoords[p1];
                    tris[tri + 1] = points[p2];
                    texCoordsOut[tri + 1] = pointTexCoords[p2];
                    tris[tri + 2] = points[p3];
                    texCoordsOut[tri + 2] = pointTexCoords[p3];

                    Vector3 e1 = v1 - v0, e2 = v2 - v0;
                    Vector3 vNormal = Vector3.Normalize(Vector3.Cross(e1, e2));

                    //triNormals[triIdx++] = vNormal;

                    pointNormals[p1] += vNormal;
                    pointNormals[p2] += vNormal;
                    pointNormals[p3] += vNormal;

                    pointMult[p1]++;
                    pointMult[p2]++;
                    pointMult[p3]++;
                }

                // normalize normal vectors
                for (int point = 0; point < points.Length; point++)
                {
                    // NOTE: Assume no muls are 0! NaN
                    pointNormals[point] *= 1f / pointMult[point];
                }

                // assign normals to tris
                //triIdx = 0;
                for (int tri = 0; tri < tris.Length; tri += 3)
                {
                    int p1 = triPRefs[tri];
                    int p2 = triPRefs[tri + 1];
                    int p3 = triPRefs[tri + 2];

                    normalsOut[tri] = /*triNormals[triIdx];*/pointNormals[p1];
                    normalsOut[tri + 1] = /*triNormals[triIdx];*/pointNormals[p2];
                    normalsOut[tri + 2] = /*triNormals[triIdx];*/pointNormals[p3];
                    //triIdx++;
                }
            }
            else
            {
                for (int tri = 0; tri < tris.Length; tri += 3)
                {
                    int p1 = triPRefs[tri];
                    int p2 = triPRefs[tri + 1];
                    int p3 = triPRefs[tri + 2];

                    tris[tri] = points[p1];
                    texCoordsOut[tri] = pointTexCoords[p1];
                    tris[tri + 1] = points[p2];
                    texCoordsOut[tri + 1] = pointTexCoords[p2];
                    tris[tri + 2] = points[p3];
                    texCoordsOut[tri + 2] = pointTexCoords[p3];
                }
            }

            // calc tex coords
            //if (texCoords)
            //{
            //    double twopi = Math.PI * 2;
            //    texCoordsOut = new Vector2[tris.Length];
            //    for (int i = 0; i < tris.Length; i++)
            //    {
            //        //double angle = 180 * (Math.Atan2(tris[i].X, tris[i].Z) - Math.Atan2(0, 1)) / Math.PI;
            //        //if (angle < 0)
            //        //    angle += 360;
            //        //texCoordsOut[i] = new Vector2((float)angle / 360f,//(float)(Math.Asin(normalsOut[i].Y) / Math.PI + 0.5f),
            //        //                              (tris[i].Y / 2f) + 0.5f);//normalsOut[i].X / 2f + 0.5f);//(Math.Asin(normalsOut[i].X) / Math.PI + 0.5f));

            //        double v = (Math.Acos(tris[i].Y / radius) / Math.PI) / 2;
            //        double u;
            //        if (tris[i].Z >= 0)
            //            u = Math.Acos(tris[i].X / (radius * Math.Sin(Math.PI * v))) / twopi;
            //        else
            //            u = (Math.PI + Math.Acos(tris[i].X / (radius * Math.Sin(Math.PI * v)))) / twopi;

            //        texCoordsOut[i] = new Vector2((float)u / 4, (float)v);
            //    }
            //}

            return tris;
        }

        public static SphereN CalcSphereWNormals(int numSlices, int numStacks, float radius, Vector3 center, bool texCoords)
        {
            SphereN sphere = new SphereN();
            sphere.Positions = CalcSphereTriangles(numSlices, numStacks, radius, center, ref sphere.Normals, true, ref sphere.TexCoords, texCoords);
            return sphere;
        }

        public static SphereN CalcSphere(int numSlices, int numStacks, float radius, Vector3 center)
        {
            SphereN sphere = new SphereN();
            sphere.Positions = CalcSphereTriangles(numSlices, numStacks, radius, center, ref sphere.Normals, false, ref sphere.TexCoords, false);
            return sphere;
        }
    }
}