using System;

namespace NuGenRenderOptics
{
    class TriangleGroupSceneObject : OpticalSceneObject
    {
        struct Triangle
        {
            public uint P1, P2, P3;

            public Triangle(uint p1, uint p2, uint p3)
            {
                P1 = p1;
                P2 = p2;
                P3 = p3;
            }
        }

        Vector3D[] points;
        Triangle[] triangles;
        Vector3D[] vNormals;
        Vector3D[] fNormals;
        Vector2D[] tCoords;

        public TriangleGroupSceneObject(Vector3D origin, MaterialShader shader, double radius)
            : base(origin, shader, radius)
        {
            // determine scaling factor
            double scale = Math.Sqrt((radius * radius) / 2);
            this.radius *= 2;
            points = new Vector3D[] { new Vector3D(-1, -1, 1), new Vector3D(-1, 1, 1),
                                      new Vector3D(1, 1, 1), new Vector3D(1, -1, 1),
                                      new Vector3D(-1, -1, -1), new Vector3D(-1, 1, -1),
                                      new Vector3D(1, 1, -1), new Vector3D(1, -1, -1)  };
            
            // tansform points
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X *= scale;
                points[i].X += origin.X;
                points[i].Y *= scale;
                points[i].Y += origin.Y;
                points[i].Z *= scale;
                points[i].Z += origin.Z;
            }

            triangles = new Triangle[] { new Triangle(4, 7, 6), new Triangle(6, 5, 4),
                                         new Triangle(2, 3, 0), new Triangle(0, 1, 2),
                                         new Triangle(1, 0, 4), new Triangle(4, 5, 1),
                                         new Triangle(7, 3, 2), new Triangle(2, 6, 7),
                                         new Triangle(0, 3, 7), new Triangle(7, 4, 0),
                                         new Triangle(6, 2, 1), new Triangle(1, 5, 6) };

            tCoords = new Vector2D[] { new Vector2D(0, 0), new Vector2D(0, 1), new Vector2D(1, 1), 
                                       new Vector2D(1, 1), new Vector2D(1, 0), new Vector2D(0, 0), 
            };

            CalcFaceNormals();
            CalcVertexNormals();
        }

        private void CalcVertexNormals()
        {
            ushort[] multiplicities = new ushort[points.Length];
            vNormals = new Vector3D[points.Length];
            // add & count
            for (int i = 0; i < fNormals.Length; i++)
            {
                Triangle tri = triangles[i];
                vNormals[tri.P1] += fNormals[i];
                vNormals[tri.P2] += fNormals[i];
                vNormals[tri.P3] += fNormals[i];

                multiplicities[tri.P1]++;
                multiplicities[tri.P2]++;
                multiplicities[tri.P3]++;
            }

            // divide
            for (int i = 0; i < vNormals.Length; i++)
            {
                vNormals[i] *= 1 / multiplicities[i];
            }
        }

        private void CalcFaceNormals()
        {
            fNormals = new Vector3D[triangles.Length];
            for (int tri = 0; tri < triangles.Length; tri++)
            {
                Vector3D u = points[triangles[tri].P3] - points[triangles[tri].P1];
                Vector3D w = points[triangles[tri].P2] - points[triangles[tri].P1];

                // u x w
                fNormals[tri] = new Vector3D(w.Z * u.Y - w.Y * u.Z, w.X * u.Z - w.Z * u.X, w.Y * u.X - w.X * u.Y);
                fNormals[tri].Normalize();
            }
        }

        public TriangleGroupSceneObject(Vector3D origin, Vector3D scale, MaterialShader shader)
            : base(origin, shader, 0)
        {
            points = new Vector3D[] { new Vector3D(0, 0, 1), new Vector3D(0, 1, 1),
                                      new Vector3D(1, 1, 1), new Vector3D(1, 0, 1),
                                      new Vector3D(0, 0, 0), new Vector3D(0, 1, 0),
                                      new Vector3D(1, 1, 0), new Vector3D(1, 0, 0)  };

            // tansform points
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X *= scale.X;
                points[i].X += origin.X;
                points[i].Y *= scale.Y;
                points[i].Y += origin.Y;
                points[i].Z *= scale.Z;
                points[i].Z += origin.Z;
            }

            triangles = new Triangle[] { new Triangle(4, 7, 6), new Triangle(6, 5, 4),
                                         new Triangle(0, 3, 2), new Triangle(2, 1, 0),
                                         new Triangle(4, 0, 1), new Triangle(1, 5, 4),
                                         new Triangle(7, 3, 2), new Triangle(2, 6, 7),
                                         new Triangle(7, 3, 0), new Triangle(0, 4, 7),
                                         new Triangle(6, 2, 1), new Triangle(1, 5, 6) };

            CalcFaceNormals();
            CalcVertexNormals();
        }

        public override bool GetIntersect(Vector3D p1, Vector3D p2, Vector3D dir,
                                          out Vector3D iPos, out double iDist, out uint subIdx)
        {
            // Note: for now we assime convex so just use first hit
            // check first by normals, use nearest triangle as return
            double nrDist = double.MaxValue;
            Vector3D nPos = Vector3D.Empty;
            uint nIdx = 0;
            for (int tri = 0; tri < triangles.Length; tri++)
            {
                //double bf = Vector3D.Dot(fNormals[tri], dir);
                // TODO: Check double sided
                //if (bf > 0)
                //{
                    // do real intersection test
                    Vector3D v1 = points[triangles[tri].P3] - p1;
                    Vector3D v2 = p2 - p1;
                    double dot1 = Vector3D.Dot(fNormals[tri], v1);
                    double dot2 = Vector3D.Dot(fNormals[tri], v2);
                    if (!(Math.Abs(dot2) < 1.0E-6))
                    {
                        // ^ division by 0 means parallel
                        double u = dot1 / dot2;
                        // reject length first, before point in triangle test
                        Vector3D rPos = u * v2;
                        double dist = rPos.Length();
                        if (dist < nrDist && PointInTriangle(new Vector3D(p1.X + u * (p2.X - p1.X),
                                                         p1.Y + u * (p2.Y - p1.Y),
                                                         p1.Z + u * (p2.Z - p1.Z)),
                                                         triangles[tri]))
                        {
                            nrDist = dist;
                            nPos = p1 + rPos;
                            nIdx = (uint)tri;
                        }
                    }
                //}
            }
            iDist = nrDist;
            iPos = nPos;
            subIdx = nIdx;
            return (nrDist != double.MaxValue);
        }

        bool PointInTriangle(Vector3D p, Triangle triangle)
        {
            return (SameSide(p, points[triangle.P1], points[triangle.P2], points[triangle.P3]) &&
                    SameSide(p, points[triangle.P2], points[triangle.P1], points[triangle.P3]) &&
                    SameSide(p, points[triangle.P3], points[triangle.P1], points[triangle.P2]));
        }

        public bool SameSide(Vector3D p1, Vector3D p2,
                             Vector3D a, Vector3D b)
        {
            Vector3D cp1, cp2;
            cp1 = Vector3D.Cross(b - a, p1 - a);
            cp2 = Vector3D.Cross(b - a, p2 - a);
            return (Vector3D.Dot(cp1, cp2) >= 0);
        }

        public override Vector3D GetNormal(Vector3D pos, uint subIdx)
        {
            Vector3D u = points[triangles[subIdx].P3] - points[triangles[subIdx].P1];
            Vector3D w = points[triangles[subIdx].P2] - points[triangles[subIdx].P1];

            // u x w
            return new Vector3D(w.Z * u.Y - w.Y * u.Z, w.X * u.Z - w.Z * u.X, w.Y * u.X - w.X * u.Y);
            //return fNormals[subIdx];

           /* Vector3D U = points[triangles[subIdx].P1] - points[triangles[subIdx].P2];
            Vector3D V = points[triangles[subIdx].P3] - points[triangles[subIdx].P2];

            Vector3D N = pos - points[triangles[subIdx].P2];

            double dU = Vector3D.modv(U);
            double dV = Vector3D.modv(V);
            double dN = Vector3D.modv(N);

            N.Normalize();
            U.Normalize();

            double cost = Vector3D.Dot(N, U);
            if (cost < 0)
                cost = 0;
            if (cost > 1)
                cost = 1;

            double t = Math.Acos(cost);

            double distY = 0, distX = 0;
            distX = dN * Math.Cos(t);
            distY = dN * Math.Sin(t);

            double u = distX / dU;
            double v = distY / dV;

            double nx = -((1.0 - (u + v)) * vNormals[triangles[subIdx].P2].X +
                          vNormals[triangles[subIdx].P1].X * u +
                          vNormals[triangles[subIdx].P3].X * v);
            double ny = -((1.0 - (u + v)) * vNormals[triangles[subIdx].P2].Y +
                          vNormals[triangles[subIdx].P1].Y * u +
                          vNormals[triangles[subIdx].P3].Y * v);
            double nz = -((1.0 - (u + v)) * vNormals[triangles[subIdx].P2].Z +
                          vNormals[triangles[subIdx].P1].Z * u +
                          vNormals[triangles[subIdx].P3].Z * v);

            return new Vector3D(nx, ny, nz);*/
        }

        public override Vector2D GetTexCoord(Vector3D p, uint subIdx)
        {
            double dx = 1;// imgBitmap.Width;
            double dy = 1;// imgBitmap.Height;

            Vector3D U;
            Vector3D V;
            Vector3D v2;
            double distp = 0;

            U = points[triangles[subIdx].P3] - points[triangles[subIdx].P2];
            v2 = p - points[triangles[subIdx].P2];

            distp = Vector3D.modv(points[triangles[subIdx].P2] - p);

            V = points[triangles[subIdx].P1] - points[triangles[subIdx].P2];

            double dU = Vector3D.modv(U);
            double dV = Vector3D.modv(V);

            U.Normalize();
            v2.Normalize();
            double cost = Vector3D.Dot(U, v2);
            double t = Math.Acos(cost);

            double distY = 0, distX = 0;
            distY = dU - distp * Math.Cos(t);
            distX = dV - distp * Math.Sin(t);

            double x1 = 0;
            double y1 = 0;

            int tIdx = (int)subIdx * 3;

            y1 = Vector3D.GetCoord(0, dU, tCoords[tIdx + 2].Y, tCoords[tIdx + 1].Y, distY);
            x1 = Vector3D.GetCoord(0, dV, tCoords[tIdx].X, tCoords[tIdx + 1].X, distX);

            return new Vector2D(x1, y1);
        }
    }
}