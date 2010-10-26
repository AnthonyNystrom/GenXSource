using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace NuGenSVisualLib.Maths.Volumes
{
    class MeshOptimzer
    {
        public static void BlendPoints(List<Vector3> vertices, out Vector3[] blendedPoints)
        {
            List<Vector3> points = new List<Vector3>();
            foreach (Vector3 point in vertices)
            {
                // check to see if exists
                foreach (Vector3 p in points)
                {
                    if (p.X == point.X &&
                        p.Y == point.Y &&
                        p.Z == point.Z)
                    {
                        continue;
                    }
                }
                points.Add(point);
            }
            blendedPoints = points.ToArray();
        }

        public static void BlendTriangles(int[] triangles, Vector3[] vertices, out int[] oTriangles,
                                          out Vector3[] oVertices, float threshold)
        {
            int[] vMoves = new int[vertices.Length];
            List<Vector3> nVerts = new List<Vector3>();
            for (int vert = 0; vert < vertices.Length; vert++)
            {
                // check to see if moved
                Vector3 vertex;
                if (vMoves[vert] != 0)
                    vertex = nVerts[vMoves[vert] - 1];
                else
                    vertex = vertices[vert];

                Vector3 vmin = new Vector3(vertex.X - threshold, vertex.Y - threshold, vertex.Z - threshold);
                Vector3 vmax = new Vector3(vertex.X + threshold, vertex.Y + threshold, vertex.Z + threshold);
                for (int v = vert + 1; v < vertices.Length; v++)
                {
                    // check to see if moved or share same new point
                    Vector3 vertex2;
                    if (vMoves[v] != 0)
                    {
                        if (vMoves[vert] != 0)//vMoves[v] == vMoves[vert] || vMoves[ve)
                            continue;
                        vertex2 = nVerts[vMoves[v] - 1];
                    }
                    else
                        vertex2 = vertices[v];

                    if (vertices[v].X > vmin.X && vertices[v].X < vmax.X &&
                        vertices[v].Y > vmin.Y && vertices[v].Y < vmax.Y &&
                        vertices[v].Z > vmin.Z && vertices[v].Z < vmax.Z)
                    {
                        // calc new point
                        Vector3 newPos = (vertex + vertices[v]) * 0.5f;
                        // create new or move point
                        if (vMoves[vert] != 0)
                        {
                            nVerts[vMoves[vert] - 1] = newPos;
                            vMoves[v] = vMoves[vert];
                        }
                        else
                        {
                            nVerts.Add(newPos);
                            vMoves[v] = vMoves[vert] = nVerts.Count;
                        }
                    }
                }
            }

            // stick vertices back together
            int numVerts = 0;
            for (int v = 0; v < vertices.Length; v++)
            {
                if (vMoves[v] == 0)
                    numVerts++;
            }
            numVerts += nVerts.Count;
            oVertices = new Vector3[numVerts];
            // remap for new array
            for (int v = 0; v < nVerts.Count; v++)
            {
                oVertices[v] = nVerts[v];
            }
            int vIdx = nVerts.Count;
            for (int v = 0; v < vertices.Length; v++)
            {
                if (vMoves[v] == 0)
                {
                    vMoves[v] = vIdx;
                    oVertices[vIdx++] = vertices[v];
                }
                else
                {
                    vMoves[v]--;
                }
            }

            // remap triangles
            List<int> nTriangles = new List<int>();
            for (int t = 0; t < triangles.Length; t+=3)
            {
                // decide if still a valid triangle
                int np1 = vMoves[triangles[t]];
                int np2 = vMoves[triangles[t + 1]];
                int np3 = vMoves[triangles[t + 2]];

                if (np1 == np2 || np2 == np3 || np3 == np1)
                    continue;
                nTriangles.Add(np1);
                nTriangles.Add(np2);
                nTriangles.Add(np3);
            }
            oTriangles = nTriangles.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="triangles"></param>
        /// <param name="vertices"></param>
        /// <param name="threshhold">Range: 0.0 - 1.0</param>
        /// <param name="oTriangles"></param>
        /// <param name="oVertices"></param>
        public static void BlendTriangles(int[] triangles, Vector3[] vertices, float threshhold,
                                          float maxDim, out int[] oTriangles,
                                          out Vector3[] oVertices)
        {
            // count how many triangles share each vertex
            int[] vTriShared = new int[vertices.Length];
            for (int tIdx = 0; tIdx < triangles.Length; tIdx++)
            {
                vTriShared[triangles[tIdx]]++;
            }
            // link vertices with triangles
            int numTriangles = triangles.Length / 3;
            int[][] vertTriRefs = new int[vertices.Length][];
            int[] vertTriRefsIdx = new int[vertices.Length];

            int idx = 0;
            for (int tri = 0; tri < numTriangles; tri++)
            {
                int v1 = triangles[idx];
                int v2 = triangles[idx + 1];
                int v3 = triangles[idx + 2];

                if (vertTriRefs[v1] == null)
                    vertTriRefs[v1] = new int[vTriShared[v1]];
                vertTriRefs[v1][vertTriRefsIdx[v1]++] = tri;

                if (vertTriRefs[v2] == null)
                    vertTriRefs[v2] = new int[vTriShared[v2]];
                vertTriRefs[v2][vertTriRefsIdx[v2]++] = tri;

                if (vertTriRefs[v3] == null)
                    vertTriRefs[v3] = new int[vTriShared[v3]];
                vertTriRefs[v3][vertTriRefsIdx[v3]++] = tri;

                idx += 3;
            }
            // TODO: Need to process cuts at check-time

            // check each triangle sharing each vertex for proximity
            bool[] vCuts = new bool[vertices.Length];
            bool[] tCuts = new bool[triangles.Length];
            List<int> cuts = new List<int>();
            for (int vIdx = 0; vIdx < vertices.Length; vIdx++)
            {
                if (vCuts[vIdx])
                    continue;
                // check all connected tris
                for (int tIdx = 0; tIdx < vertTriRefsIdx[vIdx]; tIdx++)
                {
                    int tri = vertTriRefs[vIdx][tIdx];
                    if (tCuts[tri])
                        continue;
                    int triIdx = tri * 3;

                    // work out which 2 verts are *not* the one we are focusing on
                    // then measure their length to the 3rd vertex
                    int end = triIdx + 3;
                    for (; triIdx < end; triIdx++)
                    {
                        int vertexIdx = triangles[triIdx];
                        if (vertices[vertexIdx] != vertices[vIdx])
                        {
                            float x, y, z;
                            x = vertices[vIdx].X - vertices[vertexIdx].X;
                            y = vertices[vIdx].Y - vertices[vertexIdx].Y;
                            z = vertices[vIdx].Z - vertices[vertexIdx].Z;

                            if (Math.Sqrt((x * x) + (y * y) + (z * z)) < threshhold)
                            {
                                // add tri+vertex to be cut
                                vCuts[vertexIdx] = true;
                                tCuts[tri] = true;
                                cuts.Add(tri);
                                cuts.Add(vIdx);
                                cuts.Add(vertexIdx);
                                break;
                            }
                        }
                    }
                }
            }

            // process cuts
            if (cuts.Count > 0)
            {
                for (int cut = 0; cut < cuts.Count; cut+=3)
                {
                    int cTri = cuts[cut];
                    int v1 = cuts[cut + 1];
                    int v2 = cuts[cut + 2];

                    // TODO: Move to half way between?
                    // reasign triangle refs from v2 to v1
                    for (int tIdx = 0; tIdx < vertTriRefsIdx[v2]; tIdx++)
                    {
                        int tri = vertTriRefs[v2][cTri];
                        if (tri == cTri || tCuts[tri])
                            continue;
                        int triIdx = tri * 3;
                        // find vetex to reassign
                        for (int i = 0; i < 3; i++)
                        {
                            if (triangles[triIdx + i] == v1)
                            {
                                // triangle is cut also
                                tCuts[tri] = true;
                                break;
                            }
                            if (triangles[triIdx + i] == v2)
                            {
                                triangles[triIdx + i] = v1;
                            }
                        }
                    }
                }
                oTriangles = null;
                oVertices = null;
            }
            else
            {
                oTriangles = triangles;
                oVertices = vertices;
            }
        }

        public static float FindMaxDimension(Vector3[] values)
        {
            float min = values[0].X;
            float max = values[0].X;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].X < min)
                    min = values[i].X;
                else if (values[i].X > max)
                    max = values[i].X;

                if (values[i].Y < min)
                    min = values[i].Y;
                else if (values[i].Y > max)
                    max = values[i].Y;

                if (values[i].Z < min)
                    min = values[i].Z;
                else if (values[i].Z > max)
                    max = values[i].Z;
            }
            if (min < 0)
                min = -min;

            if (max > min)
                return max;
            return min;
        }

        public static void IndexRawTriangles(Vector3[] triangles, out int[] oIndices,
                                             out Vector3[] oVertices, bool merge)
        {
            // index each unique vertex
            oIndices = new int[triangles.Length];
            //foreach (int index in indexed) index = -1;
            List<Vector3> vertices = new List<Vector3>();
            for (int tri = 0; tri < triangles.Length; tri++)
            {
                // look for existing vertex
                bool indexedVert = false;
                for (int vIdx = 0; vIdx < vertices.Count; vIdx++)
                {
                    if (vertices[vIdx] == triangles[tri])
                    {
                        oIndices[tri] = vIdx;
                        indexedVert = true;
                        break;
                    }
                }
                if (!indexedVert)
                {
                    // index new vertex
                    vertices.Add(triangles[tri]);
                    oIndices[tri] = vertices.Count;
                }
            }
            oVertices = vertices.ToArray();
            vertices.Clear();
        }

        public static void IndexRawTriangles(List<Vector3> triangles, out int[] oIndices,
                                             out Vector3[] oVertices, bool merge)
        {
            // round all verts
            for (int tri = 0; tri < triangles.Count; tri++)
            {
                float mx = (float)Math.Round(triangles[tri].X, 4);
                float my = (float)Math.Round(triangles[tri].Y, 4);
                float mz = (float)Math.Round(triangles[tri].Z, 4);
                triangles[tri] = new Vector3(mx, my, mz);
            }
            // index each unique vertex
            oIndices = new int[triangles.Count];
            //foreach (int index in indexed) index = -1;
            List<Vector3> vertices = new List<Vector3>();
            for (int tri = 0; tri < triangles.Count; tri++)
            {
                // look for existing vertex
                bool indexedVert = false;
                for (int vIdx = 0; vIdx < vertices.Count; vIdx++)
                {
                    //int numDone = tri % 3;
                    //int tPos = tri - numDone;
                    //bool ok = true;
                    //for (int i = 0; i < numDone; i++)
                    //{
                    //    if (vIdx == oIndices[tPos + i])
                    //    {
                    //        ok = false;
                    //        break;
                    //    }
                    //}
                    //if (ok)
                    //{
                        //if (vertices[vIdx].X > triangles[tri].X - 0.05f && vertices[vIdx].X < triangles[tri].X + 0.05f &&
                        //    vertices[vIdx].Y > triangles[tri].Y - 0.05f && vertices[vIdx].Y < triangles[tri].Y + 0.05f &&
                        //    vertices[vIdx].Z > triangles[tri].Z - 0.05f && vertices[vIdx].Z < triangles[tri].Z + 0.05f)
                        if (vertices[vIdx].X == triangles[tri].X &&
                            vertices[vIdx].Y == triangles[tri].Y &&
                            vertices[vIdx].Z == triangles[tri].Z)
                        {
                            oIndices[tri] = vIdx;
                            indexedVert = true;
                            break;
                        }
                    //}
                }
                if (!indexedVert)
                {
                    // index new vertex
                    oIndices[tri] = vertices.Count;
                    vertices.Add(triangles[tri]);
                }
            }
            oVertices = vertices.ToArray();
            vertices.Clear();
        }

        public static void GenerateTriPointNormals(int[] triangles, Vector3[] vertices, out Vector3[] normals)
        {
            // calc all triangle normals
            normals = new Vector3[vertices.Length];
            int[] multi = new int[vertices.Length];
            for (int tri = 0; tri < triangles.Length; tri+=3)
            {
                int p1 = triangles[tri];
                int p2 = triangles[tri + 1];
                int p3 = triangles[tri + 2];

                bool zeroSum = false;
                if (vertices[p1].X == vertices[p2].X &&
                    vertices[p1].Y == vertices[p2].Y &&
                    vertices[p1].Z == vertices[p2].Z)
                {
                    zeroSum = true;
                }
                else if (vertices[p1].X == vertices[p3].X &&
                         vertices[p1].Y == vertices[p3].Y &&
                         vertices[p1].Z == vertices[p3].Z)
                {
                    zeroSum = true;
                }
                else if (vertices[p2].X == vertices[p3].X &&
                         vertices[p2].Y == vertices[p3].Y &&
                         vertices[p2].Z == vertices[p3].Z)
                {
                    zeroSum = true;
                }

                if (!zeroSum)
                {
                    Vector3 v0 = vertices[p1];
                    Vector3 v1 = vertices[p2];
                    Vector3 v2 = vertices[p3];

                    Vector3 e1 = v1 - v0, e2 = v2 - v0;
                    Vector3 normal = Vector3.Normalize(Vector3.Cross(e1, e2));

                    normals[p1] += normal;
                    normals[p2] += normal;
                    normals[p3] += normal;

                    multi[p1]++;
                    multi[p2]++;
                    multi[p3]++;
                }
            }

            for (int vert = 0; vert < vertices.Length; vert++)
            {
                normals[vert] *= 1f / multi[vert];
            }
        }
    }
}
