using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace NuGenSVisualLib
{
    /// <summary>
    /// Provides helper code for generating points to represent circles & spheres
    /// </summary>
    public class SphereMathHelper
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

            double alpha = (z == 0) ? Math.PI / 2 : Math.Atan(x / z);

            double r = (alpha == 0) ? z : x / Math.Sin(alpha);

            float sign = 1f;
            if (z != 0)
                sign *= Math.Sign(z);
            else if (x != 0)
                sign *= Math.Sign(x);
            if (y != 0) sign *= Math.Sign(y);

            double theta = -sign * Math.Abs((r == 0) ? Math.PI / 2 : Math.Atan(y / r));

            xAngle = (float)alpha;
            yAngle = (float)theta;
        }

        #region Circle

        public static void CalcCirclePointsCCW(int numPoints, float radius, Vector2 center, out Vector2[] points)
        {
            CalcArcPointsCCW(numPoints, radius, center, 360f, out points);
        }

        public static void CalcArcPointsCCW(int numPoints, float radius, Vector2 center, float arcAngle,
                                                 out Vector2[] points)
        {
            // calc intervals
            float intervalDeg = arcAngle / ((float)numPoints-1.0f);
            float intervalRad = (float)Math.PI * (intervalDeg / 180.0f);

            // calc positions for all points
            points = new Vector2[numPoints - 1];
            for (int p = 0; p < numPoints - 1; p++)
            {
                float x = center.X + ((float)Math.Cos(intervalRad * p) * radius);
                float y = center.Y + ((float)Math.Sin(intervalRad * p) * radius);
                points[p] = new Vector2(x, y);
            }
        }

        #endregion

        #region Sphere

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
            CalcArcPointsCCW(numStackPoints, radius, new Vector2(center.Z, center.Y),
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
                CalcCirclePointsCCW(numSlices + 1, sRadius, center2D, out points);
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

        #endregion

        #region Cylinders

        /// <summary>
        /// Calculates the points for an open ended cylinder
        /// </summary>
        /// <param name="numSlices"></param>
        /// <param name="numStacks"></param>
        /// <param name="radius"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        /// <remarks at="08-01-07">Needs testing</remarks>
        public static Vector3[] CalcCyclinderPoints(int numSlices, int numStacks, float radius,
                                                    Vector3 startPoint, Vector3 endPoint)
        {
            // calc end(s) circle
            Vector2[] endCircle;
            CalcCirclePointsCCW(numSlices + 1, radius, new Vector2(0, 0), out endCircle);
            RoundPoints(endCircle);

            // copy to 3d
            Vector3[] endCircle3 = new Vector3[endCircle.Length];
            for (int point = 0; point < endCircle.Length; point++)
            {
                endCircle3[point] = new Vector3(endCircle[point].X, endCircle[point].Y, 0);
            }

            // transform points into template end/diameter points
            Vector3 cylinderDist = endPoint - startPoint;
            Vector3 cylinderPathUV = Vector3.Normalize(cylinderDist);

            Matrix rot = Vector3ToMatrixRot(cylinderPathUV);
            Vector4[] templatePoints4 = Vector3.Transform(endCircle3, rot);

            // calc length of each stack
            float stackSize = cylinderDist.Length() / numStacks;

            // translate template to start (startPoint)
            //for (int point = 0; point < templatePoints4.Length; point++)
            //{
            //    float x = templatePoints4[point].X;
            //    float y = templatePoints4[point].Z;
            //    float z = templatePoints4[point].Y;

            //    templatePoints4[point].X = x;//+= startPoint.X;
            //    templatePoints4[point].Y = y;//+= startPoint.Y;
            //    templatePoints4[point].Z = z;//+= startPoint.Z;
            //}

            // translate template for each stack
            Vector3[] points = new Vector3[numSlices * (numStacks + 1)];
            for (int stack = 0; stack < numStacks + 1; stack++)
            {
                int pIdx = stack * numSlices;
                // calc position
                Vector3 rPos = cylinderPathUV * (stack * stackSize);

                // translate template points
                for (int point = 0; point < templatePoints4.Length; point++)
                {
                    points[pIdx + point] = new Vector3(templatePoints4[point].X + rPos.X,
                                                       templatePoints4[point].Y + rPos.Y,
                                                       templatePoints4[point].Z + rPos.Z);
                }
            }
            return points;
        }

        private static void RoundPoints(Vector2[] points)
        {
            for (int p = 0; p < points.Length; p++)
            {
                points[p].X = (float)Math.Round(points[p].X, 5);
                points[p].Y = (float)Math.Round(points[p].Y, 5);
            }
        }

        public static void CalcCylinderTriangles(int numSlices, int numStacks, float radius,
                                                 Vector3 startPoint, Vector3 endPoint,
                                                 bool calcNormals, EndType endType, float endOffSet,
                                                 out Vector3[] pipePoints, out Vector3[] pipeNormals,
                                                 out int[] pipeTris, out Vector3[] endPoints1,
                                                 out Vector3[] endNormals1, out int[] endTris1,
                                                 out Vector3[] endPoints2, out Vector3[] endNormals2,
                                                 out int[] endTris2)
        {
            // get points for cylinder
            pipePoints = CalcCyclinderPoints(numSlices, numStacks, radius, startPoint, endPoint);

            int numTriangles = numSlices * numStacks * 2;

            // build stacks
            int vIdx = 0;
            int pIdx = 0;
            pipeTris = new int[numTriangles * 3];
            int[] pointMulti = new int[pipePoints.Length];
            for (int stack = 0; stack < numStacks; stack++)
            {
                for (int slice = 0; slice < numSlices; slice++)
                {
                    // FIXME: Move multi??
                    pointMulti[pipeTris[vIdx] = pIdx + slice]++;
                    pointMulti[pipeTris[vIdx + 1] = pIdx + slice + 1]++;
                    pointMulti[pipeTris[vIdx + 2] = pIdx + slice + numSlices]++;

                    pointMulti[pipeTris[vIdx + 3] = pIdx + slice]++;
                    pointMulti[pipeTris[vIdx + 4] = pIdx + slice + numSlices]++;
                    pointMulti[pipeTris[vIdx + 5] = pIdx + slice + numSlices - 1]++;

                    vIdx += 6;
                }
                pIdx += numSlices;
            }

            // ends
            if (endType == EndType.Flat)
            {
                int numEndTriangles = numSlices;
                endTris1 = new int[numEndTriangles * 3];
                endPoints1 = new Vector3[numSlices + 1];

                // calc mid points for both ends
                Vector3 cylinderUV = endPoint - startPoint;
                cylinderUV.Normalize();

                Vector3 endP1 = startPoint - (cylinderUV * endOffSet);
                Vector3 endP2 = startPoint/*endPoint*/ + (cylinderUV * endOffSet);

                // copy points
                for (int point = 0; point < endPoints1.Length - 1; point++)
                {
                    endPoints1[point] = pipePoints[point];
                }
                endPoints1[endPoints1.Length - 1] = endP1;

                // first end
                int triIdx = 0;
                int endPIdx = endPoints1.Length - 1;
                for (int point = 0; point < numSlices; point++)
                {
                    endTris1[triIdx] = endPIdx;
                    endTris1[triIdx + 2] = point;
                    if (point == numSlices - 1)
                        endTris1[triIdx + 1] = 0;
                    else
                        endTris1[triIdx + 1] = point + 1;
                    triIdx += 3;
                }

                endTris2 = new int[numEndTriangles * 3];
                endPoints2 = new Vector3[numSlices + 1];

                // copy points
                for (int point = 0; point < endPoints2.Length - 1; point++)
                {
                    endPoints2[point] = pipePoints[point];
                }
                endPoints2[endPoints1.Length - 1] = endP2;
                
                // second end
                for (int point = 0; point < endTris2.Length; point += 3)
                {
                    endTris2[point] = endTris1[point];
                    endTris2[point + 1] = endTris1[point + 2];
                    endTris2[point + 2] = endTris1[point + 1];
                }

                if (calcNormals)
                {
                    endNormals1 = new Vector3[endPoints1.Length];
                    int idx = 0;
                    for (int tri = 0; tri < numEndTriangles; tri++)
                    {
                        Vector3 v0 = endPoints1[endTris1[idx]];
                        Vector3 v1 = endPoints1[endTris1[idx + 2]];
                        Vector3 v2 = endPoints1[endTris1[idx + 1]];

                        Vector3 e1 = v1 - v0, e2 = v2 - v0;
                        Vector3 fNormal = Vector3.Normalize(Vector3.Cross(e1, e2));

                        endNormals1[endTris1[idx]] += fNormal;
                        endNormals1[endTris1[idx + 2]] += fNormal;
                        endNormals1[endTris1[idx + 1]] += fNormal;
                        idx += 3;
                    }
                    for (int point = 0; point < endNormals1.Length - 1; point++)
                    {
                        endNormals1[point] *= 0.5f;//1f / (float)//pointMulti[point];
                    }
                    endNormals1[endNormals1.Length - 1] *= numSlices;

                    endNormals2 = new Vector3[endPoints2.Length];
                    Array.Copy(endNormals1, endNormals2, endNormals1.Length);
                    for (int point = 0; point < endNormals1.Length; point++)
                    {
                        endNormals2[point].Z = - endNormals2[point].Z;
                    }
                    //mult = new int[endPoints2.Length];
                    //idx = 0;
                    //for (int tri = 0; tri < numEndTriangles; tri++)
                    //{
                    //    Vector3 v0 = endPoints2[endTris2[idx]];
                    //    Vector3 v1 = endPoints2[endTris2[idx + 1]];
                    //    Vector3 v2 = endPoints2[endTris2[idx + 2]];

                    //    Vector3 e1 = v1 - v0, e2 = v2 - v0;
                    //    Vector3 fNormal = Vector3.Normalize(Vector3.Cross(e1, e2));

                    //    endNormals2[endTris2[idx]] += fNormal;
                    //    endNormals2[endTris2[idx + 1]] += fNormal;
                    //    endNormals2[endTris2[idx + 2]] += fNormal;
                    //    idx += 3;
                    //}
                    //for (int point = 0; point < endNormals2.Length; point++)
                    //{
                    //    endNormals2[point] *= 1f / (float)pointMulti[point];
                    //}
                }
                else
                {
                    endNormals1 = null;
                    endNormals2 = null;
                }
            }
            else if (endType == EndType.Rounded)
            {
                // build start end cup
                int numEndTriangles = numSlices * 6;
                endPoints1 = new Vector3[(numSlices * 2) + 1];

                // build profile - only heights needed? then maybe scale cylinder outline?
                Vector2[] profile;
                CalcArcPointsCCW(3, 1, new Vector2(0, 0), 90, out profile);

                // extract template to scale from points
                Vector2[] cylinderTemplate = new Vector2[numSlices];
                for (int point = 0; point < numSlices; point++)
                {
                    cylinderTemplate[point] = new Vector2(pipePoints[point].X, pipePoints[point].Y);
                }

                // build points by scale & translate cylinder
                Vector3[] endPoints = new Vector3[numSlices];
                for (int level = 0; level < 1; level++)
                {
                    float scale = profile[level + 1].X;
                    float translation = endOffSet * -profile[level + 1].Y;
                    int epIdx = level * numSlices;
                    for (int point = 0; point < numSlices; point++)
                    {
                        // scale 'x' & 'y', translate 'z'
                        endPoints[point + epIdx] = new Vector3(cylinderTemplate[point].X * scale,
                                                               cylinderTemplate[point].Y * scale,
                                                               translation);
                    }
                }

                // calc mid points for both ends
                Vector3 cylinderUV = endPoint - startPoint;
                cylinderUV.Normalize();

                Vector3 endP1 = startPoint - (cylinderUV * endOffSet);

                // build points
                for (int point = 0; point < numSlices; point++)
                {
                    endPoints1[point] = endPoints[point];
                }
                pIdx = numSlices;
                for (int point = 0; point < numSlices; point++)
                {
                    endPoints1[pIdx++] = pipePoints[point];
                }
                endPoints1[endPoints1.Length - 1] = endP1;

                // flip for other end
                endPoints2 = new Vector3[endPoints1.Length];
                for (int point = 0; point < endPoints2.Length; point++)
                {
                    endPoints2[point] = endPoints1[point];
                    endPoints2[point].Z = -endPoints2[point].Z;
                }

                // build triangles
                endTris1 = new int[numEndTriangles * 3];

                // top cap
                int triIdx = 0;
                int endIdx = endPoints1.Length - 1;
                for (int point = 0; point < numSlices; point++)
                {
                    endTris1[triIdx] = endIdx;
                    if (point < numSlices - 1)
                        endTris1[triIdx + 1] = point + 1;
                    else
                        endTris1[triIdx + 1] = 0;
                    endTris1[triIdx + 2] = point;
                    triIdx += 3;
                }
                // sections
                //for (int section = 0; section < 1; section++)
                //{
                for (int point = 0; point < numSlices; point++)
                {
                    endTris1[triIdx] = point;
                    if (point < numSlices - 1)
                        endTris1[triIdx + 1] = point + numSlices + 1;
                    else
                        endTris1[triIdx + 1] = point + 1;
                    endTris1[triIdx + 2] = point + numSlices;

                    endTris1[triIdx + 3] = point;
                    if (point < numSlices - 1)
                    {
                        endTris1[triIdx + 4] = point + 1;
                        endTris1[triIdx + 5] = point + numSlices + 1;
                    }
                    else
                    {
                        endTris1[triIdx + 4] = 0;
                        endTris1[triIdx + 5] = numSlices;
                    }

                    triIdx += 6;
                }
                //}

                // duplicate for end2
                endTris2 = new int[endTris1.Length];
                for (int point = 0; point < endTris2.Length; point+=3)
                {
                    endTris2[point] = endTris1[point];
                    endTris2[point + 1] = endTris1[point + 2];
                    endTris2[point + 2] = endTris1[point + 1];
                }

                // calc normals
                if (calcNormals)
                {
                    // run multiplicites
                    //int[] multi = new int[endPoints1.Length];
                    //for (int tri = 0; tri < numEndTriangles * 3; tri += 3)
                    //{
                    //    multi[endTris1[tri]]++;
                    //    multi[endTris1[tri + 1]]++;
                    //    multi[endTris1[tri + 2]]++;
                    //}

                    // calc normals
                    endNormals1 = new Vector3[endPoints1.Length];
                    for (int tri = 0; tri < numEndTriangles * 3; tri += 3)
                    {
                        Vector3 v0 = endPoints1[endTris1[tri]];
                        Vector3 v1 = endPoints1[endTris1[tri + 1]];
                        Vector3 v2 = endPoints1[endTris1[tri + 2]];

                        Vector3 e1 = v1 - v0, e2 = v2 - v0;
                        Vector3 fNormal = Vector3.Normalize(Vector3.Cross(e1, e2));

                        endNormals1[endTris1[tri]] += fNormal;
                        endNormals1[endTris1[tri + 1]] += fNormal;
                        endNormals1[endTris1[tri + 2]] += fNormal;
                    }

                    // normalize
                    for (int normal = 0; normal < endNormals1.Length - 1; normal++)
                    {
                        endNormals1[normal] *= 1f / (float)pointMulti[normal];
                    }

                    // copy for flipside and invert Z
                    endNormals2 = new Vector3[endNormals1.Length];
                    Array.Copy(endNormals1, endNormals2, endNormals1.Length);
                    for (int normal = 0; normal < endNormals2.Length; normal++)
                    {
                        endNormals1[normal].Z = -endNormals1[normal].Z;
                    }
                    // NOTE: Does not blend with rest or cylinder - problem?
                }
                else
                {
                    endNormals1 = endNormals2 = null;
                }
            }
            else
            {
                endPoints1 = null;
                endTris1 = null;
                endNormals1 = null;
                endPoints2 = null;
                endTris2 = null;
                endNormals2 = null;
            }

            // calc normals
            if (calcNormals)
            {
                pipeNormals = new Vector3[pipePoints.Length];
                int idx = 0;
                for (int tri=0; tri < numTriangles; tri++)
                {
                    Vector3 v0 = pipePoints[pipeTris[idx]];
                    Vector3 v1 = pipePoints[pipeTris[idx + 1]];
                    Vector3 v2 = pipePoints[pipeTris[idx + 2]];

                    Vector3 e1 = v1 - v0, e2 = v2 - v0;
                    Vector3 fNormal = Vector3.Normalize(Vector3.Cross(e1, e2));

                    pipeNormals[pipeTris[idx]] += fNormal;
                    pipeNormals[pipeTris[idx + 1]] += fNormal;
                    pipeNormals[pipeTris[idx + 2]] += fNormal;

                    idx += 3;
                }
                for (int point = 0; point < pipeNormals.Length; point++)
                {
                    pipeNormals[point] *= 1f / (float)pointMulti[point];
                }
            }
            else
            {
                pipeNormals = null;
                endNormals1 = null;
                endNormals2 = null;
            }
        }

        #endregion

        #region New Sphere Generation code

        static double TWOPI = Math.PI * 2;
        static double PID2 = Math.PI / 2;

        public static void CreateSphereTriangleStrips(Vector3 centre, double radius, int numSegments,
                                        out CustomVertex.PositionNormalTextured[] points,
                                        out int numStrips, out int vertsPerStrip, out int primsPerStrip)
        {
            int numVerts = numSegments * (numSegments / 2) * 3;
            points = new CustomVertex.PositionNormalTextured[numVerts];
            numStrips = numSegments / 2;
            vertsPerStrip = (numSegments * 2) + 2;
            primsPerStrip = numSegments * 2;

            int i, j;
            double theta1, theta2, theta3;
            Vector3 e, p;

            if (radius < 0)
                radius = -radius;
            if (numSegments < 0)
                numSegments = -numSegments;

            int vIdx = 0;
            for (j = 0; j < numSegments / 2; j++)
            {
                theta1 = j * TWOPI / numSegments - PID2;
                theta2 = (j + 1) * TWOPI / numSegments - PID2;

                for (i = 0; i <= numSegments; i++)
                {
                    theta3 = i * TWOPI / numSegments;

                    e.X = (float)(Math.Cos(theta2) * Math.Cos(theta3));
                    e.Y = (float)Math.Sin(theta2);
                    e.Z = (float)(Math.Cos(theta2) * Math.Sin(theta3));
                    p.X = (float)(centre.X + radius * e.X);
                    p.Y = (float)(centre.Y + radius * e.Y);
                    p.Z = (float)(centre.Z + radius * e.Z);

                    points[vIdx].Normal = new Vector3(e.X, e.Y, e.Z);
                    points[vIdx].Tu = (float)(i / (double)numSegments);
                    points[vIdx].Tv = (float)(2 * (j + 1) / (double)numSegments);

                    points[vIdx].Position = new Vector3(p.X, p.Y, p.Z);
                    vIdx++;

                    e.X = (float)(Math.Cos(theta1) * Math.Cos(theta3));
                    e.Y = (float)Math.Sin(theta1);
                    e.Z = (float)(Math.Cos(theta1) * Math.Sin(theta3));
                    p.X = (float)(centre.X + radius * e.X);
                    p.Y = (float)(centre.Y + radius * e.Y);
                    p.Z = (float)(centre.Z + radius * e.Z);

                    points[vIdx].Normal = new Vector3(e.X, e.Y, e.Z);
                    points[vIdx].Tu = (float)(i / (double)numSegments);
                    points[vIdx].Tv = (float)(2 * j / (double)numSegments);

                    points[vIdx].Position = new Vector3(p.X, p.Y, p.Z);
                    vIdx++;
                }
            }
        }

        public static void CreateSphereTriangles(Vector3 centre, double radius, int numSegments,
                                   out CustomVertex.PositionNormalTextured[] points)
        {
            int numVerts = numSegments * (numSegments / 2) * 6;
            points = new CustomVertex.PositionNormalTextured[numVerts];

            double theta1, theta2, theta3;
            Vector3 e, p;

            if (radius < 0)
                radius = -radius;
            if (numSegments < 0)
                numSegments = -numSegments;

            int vIdx = 0;
            for (int j = 0; j < numSegments / 2; j++)
            {
                theta1 = j * TWOPI / numSegments - PID2;
                theta2 = (j + 1) * TWOPI / numSegments - PID2;

                for (int i = 0; i <= numSegments; i++)
                {
                    theta3 = i * TWOPI / numSegments;

                    if (i > 0)
                    {
                        e.X = (float)(Math.Cos(theta2) * Math.Cos(theta3));
                        e.Y = (float)Math.Sin(theta2);
                        e.Z = (float)(Math.Cos(theta2) * Math.Sin(theta3));
                        p.X = (float)(centre.X + radius * e.X);
                        p.Y = (float)(centre.Y + radius * e.Y);
                        p.Z = (float)(centre.Z + radius * e.Z);

                        // end of T1
                        points[vIdx].Normal = new Vector3(e.X, e.Y, e.Z);
                        points[vIdx].Tu = (float)(i / (double)numSegments);
                        points[vIdx].Tv = (float)(2 * (j + 1) / (double)numSegments);
                        points[vIdx].Position = new Vector3(p.X, p.Y, p.Z);
                        vIdx++;

                        e.X = (float)(Math.Cos(theta1) * Math.Cos(theta3));
                        e.Y = (float)Math.Sin(theta1);
                        e.Z = (float)(Math.Cos(theta1) * Math.Sin(theta3));
                        p.X = (float)(centre.X + radius * e.X);
                        p.Y = (float)(centre.Y + radius * e.Y);
                        p.Z = (float)(centre.Z + radius * e.Z);

                        // T2
                        points[vIdx].Normal = points[vIdx - 1].Normal;
                        points[vIdx].Tu = points[vIdx - 1].Tu;
                        points[vIdx].Tv = points[vIdx - 1].Tv;
                        points[vIdx].Position = points[vIdx - 1].Position;
                        vIdx++;

                        points[vIdx].Normal = new Vector3(e.X, e.Y, e.Z);
                        points[vIdx].Tu = (float)(i / (double)numSegments);
                        points[vIdx].Tv = (float)(2 * j / (double)numSegments);
                        points[vIdx].Position = new Vector3(p.X, p.Y, p.Z);
                        vIdx++;

                        points[vIdx].Normal = points[vIdx - 5].Normal;
                        points[vIdx].Tu = points[vIdx - 5].Tu;
                        points[vIdx].Tv = points[vIdx - 5].Tv;
                        points[vIdx].Position = points[vIdx - 5].Position;
                        vIdx++;

                        // start of T1
                        if (i < numSegments /*- 1*/)
                        {
                            points[vIdx].Normal = points[vIdx - 2].Normal;
                            points[vIdx].Tu = points[vIdx - 2].Tu;
                            points[vIdx].Tv = points[vIdx - 2].Tv;
                            points[vIdx].Position = points[vIdx - 2].Position;
                            vIdx++;

                            points[vIdx].Normal = points[vIdx - 4].Normal;
                            points[vIdx].Tu = points[vIdx - 4].Tu;
                            points[vIdx].Tv = points[vIdx - 4].Tv;
                            points[vIdx].Position = points[vIdx - 4].Position;
                            vIdx++;
                        }
                    }
                    else
                    {
                        e.X = (float)(Math.Cos(theta1) * Math.Cos(theta3));
                        e.Y = (float)Math.Sin(theta1);
                        e.Z = (float)(Math.Cos(theta1) * Math.Sin(theta3));
                        p.X = (float)(centre.X + radius * e.X);
                        p.Y = (float)(centre.Y + radius * e.Y);
                        p.Z = (float)(centre.Z + radius * e.Z);

                        points[vIdx].Normal = new Vector3(e.X, e.Y, e.Z);
                        points[vIdx].Tu = (float)(i / (double)numSegments);
                        points[vIdx].Tv = (float)(2 * j / (double)numSegments);
                        points[vIdx].Position = new Vector3(p.X, p.Y, p.Z);
                        vIdx++;

                        e.X = (float)(Math.Cos(theta2) * Math.Cos(theta3));
                        e.Y = (float)Math.Sin(theta2);
                        e.Z = (float)(Math.Cos(theta2) * Math.Sin(theta3));
                        p.X = (float)(centre.X + radius * e.X);
                        p.Y = (float)(centre.Y + radius * e.Y);
                        p.Z = (float)(centre.Z + radius * e.Z);

                        points[vIdx].Normal = new Vector3(e.X, e.Y, e.Z);
                        points[vIdx].Tu = (float)(i / (double)numSegments);
                        points[vIdx].Tv = (float)(2 * (j + 1) / (double)numSegments);
                        points[vIdx].Position = new Vector3(p.X, p.Y, p.Z);
                        vIdx++;
                    }
                }
            }
        }

        public static void CreateSphereTriangles(Vector3 centre, double radius, int numSegments,
                                                 out Vector3[] triangles, out Vector3[] normals,
                                                 out Vector2[] texCoords)
        {
            int numVerts = numSegments * (numSegments / 2) * 6;
            triangles = new Vector3[numVerts];
            normals = new Vector3[numVerts];
            texCoords = new Vector2[numVerts];

            double theta1, theta2, theta3;
            Vector3 e, p;

            if (radius < 0)
                radius = -radius;
            if (numSegments < 0)
                numSegments = -numSegments;

            int vIdx = 0;
            for (int j = 0; j < numSegments / 2; j++)
            {
                theta1 = j * TWOPI / numSegments - PID2;
                theta2 = (j + 1) * TWOPI / numSegments - PID2;

                for (int i = 0; i <= numSegments; i++)
                {
                    theta3 = i * TWOPI / numSegments;

                    if (i > 0)
                    {
                        e.X = (float)(Math.Cos(theta2) * Math.Cos(theta3));
                        e.Y = (float)Math.Sin(theta2);
                        e.Z = (float)(Math.Cos(theta2) * Math.Sin(theta3));
                        p.X = (float)(centre.X + radius * e.X);
                        p.Y = (float)(centre.Y + radius * e.Y);
                        p.Z = (float)(centre.Z + radius * e.Z);

                        // end of T1
                        normals[vIdx] = new Vector3(e.X, e.Y, e.Z);
                        texCoords[vIdx] = new Vector2((float)(i / (double)numSegments), (float)(2 * (j + 1) / (double)numSegments));
                        triangles[vIdx] = new Vector3(p.X, p.Y, p.Z);
                        vIdx++;

                        e.X = (float)(Math.Cos(theta1) * Math.Cos(theta3));
                        e.Y = (float)Math.Sin(theta1);
                        e.Z = (float)(Math.Cos(theta1) * Math.Sin(theta3));
                        p.X = (float)(centre.X + radius * e.X);
                        p.Y = (float)(centre.Y + radius * e.Y);
                        p.Z = (float)(centre.Z + radius * e.Z);

                        // T2
                        normals[vIdx] = normals[vIdx - 1];
                        texCoords[vIdx] = texCoords[vIdx - 1];
                        triangles[vIdx] = triangles[vIdx - 1];
                        vIdx++;

                        normals[vIdx] = new Vector3(e.X, e.Y, e.Z);
                        texCoords[vIdx] = new Vector2((float)(i / (double)numSegments), (float)(2 * j / (double)numSegments));
                        triangles[vIdx] = new Vector3(p.X, p.Y, p.Z);
                        vIdx++;

                        normals[vIdx] = normals[vIdx - 5];
                        texCoords[vIdx] = texCoords[vIdx - 5];
                        triangles[vIdx] = triangles[vIdx - 5];
                        vIdx++;

                        // start of T1
                        if (i < numSegments /*- 1*/)
                        {
                            normals[vIdx] = normals[vIdx - 2];
                            texCoords[vIdx] = texCoords[vIdx - 2];
                            triangles[vIdx] = triangles[vIdx - 2];
                            vIdx++;

                            normals[vIdx] = normals[vIdx - 4];
                            texCoords[vIdx] = texCoords[vIdx - 4];
                            triangles[vIdx] = triangles[vIdx - 4];
                            vIdx++;
                        }
                    }
                    else
                    {
                        e.X = (float)(Math.Cos(theta1) * Math.Cos(theta3));
                        e.Y = (float)Math.Sin(theta1);
                        e.Z = (float)(Math.Cos(theta1) * Math.Sin(theta3));
                        p.X = (float)(centre.X + radius * e.X);
                        p.Y = (float)(centre.Y + radius * e.Y);
                        p.Z = (float)(centre.Z + radius * e.Z);

                        normals[vIdx] = new Vector3(e.X, e.Y, e.Z);
                        texCoords[vIdx] = new Vector2((float)(i / (double)numSegments), (float)(2 * j / (double)numSegments));
                        triangles[vIdx] = new Vector3(p.X, p.Y, p.Z);
                        vIdx++;

                        e.X = (float)(Math.Cos(theta2) * Math.Cos(theta3));
                        e.Y = (float)Math.Sin(theta2);
                        e.Z = (float)(Math.Cos(theta2) * Math.Sin(theta3));
                        p.X = (float)(centre.X + radius * e.X);
                        p.Y = (float)(centre.Y + radius * e.Y);
                        p.Z = (float)(centre.Z + radius * e.Z);

                        normals[vIdx] = new Vector3(e.X, e.Y, e.Z);
                        texCoords[vIdx] = new Vector2((float)(i / (double)numSegments), (float)(2 * (j + 1) / (double)numSegments));
                        triangles[vIdx] = new Vector3(p.X, p.Y, p.Z);
                        vIdx++;
                    }
                }
            }
        }
        #endregion
    }
}