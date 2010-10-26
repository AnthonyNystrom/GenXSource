using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NuGenCRBase.SceneFormats.Lwo
{
    class lwPointList : IDisposable
    {
        public int count;
        public int offset;                  /* only used during reading */
        public lwPoint[] pt;                /* array of points */

        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion

        public static bool GetPoints(FileStream file, int cksize, ref lwPointList point, ref int flen)
        {
            byte[] f = new byte[cksize];
            int np;

            if (cksize == 1)
                return true;

            /* extend the point array to hold the new points */

            np = cksize / 12;
            point.offset = point.count;
            point.count += np;
            point.pt = new lwPoint[point.count];

            /* read the whole chunk */

            flen += file.Read(f, 0, cksize);
            lwio.revbytes(ref f, 4, np * 3);

            /* assign position values */

            for (int i = 0, j = 0; i < np; i++, j += 3)
            {
                point.pt[i].pos = new float[3];
                point.pt[i].pos[0] = BitConverter.ToSingle(f, j * 4);
                point.pt[i].pos[1] = BitConverter.ToSingle(f, (j + 1) * 4);
                point.pt[i].pos[2] = BitConverter.ToSingle(f, (j + 2) * 4);
            }

            return true;
        }

        public void GetBoundingBox(ref float[] bbox)
        {
            bbox = new float[6];

            int i, j;

            if (count == 0) return;

            for (i = 0; i < 6; i++)
                if (bbox[i] != 0.0f) return;

            bbox[0] = bbox[1] = bbox[2] = 1e20f;
            bbox[3] = bbox[4] = bbox[5] = -1e20f;
            for (i = 0; i < count; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    if (bbox[j] > pt[i].pos[j])
                        bbox[j] = pt[i].pos[j];
                    if (bbox[j + 3] < pt[i].pos[j])
                        bbox[j + 3] = pt[i].pos[j];
                }
            }
        }

        /// <summary>
        /// Calculate the polygon normals.  By convention, LW's polygon normals
        /// are found as the cross product of the first and last edges.  It's
        /// undefined for one- and two-point polygons.
        /// </summary>
        /// <param name="lwPolygonList"></param>
        public void GetPolyNormals(ref lwPolygonList polygon)
        {
            float[] p1 = new float[3];
            float[] p2 = new float[3];
            float[] pn = new float[3];
            float[] v1 = new float[3];
            float[] v2 = new float[3];

            for (int i = 0; i < polygon.count; i++)
            {
                if (polygon.pol[i].nverts < 3)
                    continue;
                for (int j = 0; j < 3; j++)
                {
                    p1[j] = pt[polygon.pol[i].v[0].index].pos[j];
                    p2[j] = pt[polygon.pol[i].v[1].index].pos[j];
                    pn[j] = pt[polygon.pol[i].v[polygon.pol[i].nverts - 1].index].pos[j];
                }

                for (int j = 0; j < 3; j++)
                {
                    v1[j] = p2[j] - p1[j];
                    v2[j] = pn[j] - p1[j];
                }

                LwVecMath.cross(v1, v2, polygon.pol[i].norm);
                LwVecMath.normalize(polygon.pol[i].norm);
            }
        }

        /// <summary>
        /// For each point, fill in the indexes of the polygons that share the
        /// point.  Returns false if any of the memory allocations fail, otherwise
        /// returns true.
        /// </summary>
        /// <param name="lwPolygonList"></param>
        /// <returns></returns>
        public bool GetPointPolygons(ref lwPolygonList polygon)
        {
            int k;

            /* count the number of polygons per point */

            for (int i = 0; i < polygon.count; i++)
                for (int j = 0; j < polygon.pol[i].nverts; j++)
                    ++pt[polygon.pol[i].v[j].index].npols;

            /* alloc per-point polygon arrays */

            for (int i = 0; i < count; i++)
            {
                if (pt[i].npols == 0)
                    continue;
                pt[i].pol = new int[pt[i].npols];
                if (pt[i].pol == null)
                    return false;
                pt[i].npols = 0;
            }

            /* fill in polygon array for each point */

            for (int i = 0; i < polygon.count; i++)
            {
                for (int j = 0; j < polygon.pol[i].nverts; j++)
                {
                    k = polygon.pol[i].v[j].index;
                    pt[k].pol[pt[k].npols] = i;
                    ++pt[k].npols;
                }
            }
            return true;
        }

        /// <summary>
        /// Calculate the vertex normals.  For each polygon vertex, sum the
        /// normals of the polygons that share the point.  If the normals of the
        /// current and adjacent polygons form an angle greater than the max
        /// smoothing angle for the current polygon's surface, the normal of the
        /// adjacent polygon is excluded from the sum.  It's also excluded if the
        /// polygons aren't in the same smoothing group.
        /// 
        /// Assumes that lwGetPointPolygons(), lwGetPolyNormals() and
        /// lwResolvePolySurfaces() have already been called.
        /// </summary>
        /// <param name="lwPolygonList"></param>
        public void GetVertNormals(ref lwPolygonList polygon)
        {
            int h, p;
            float a;

            for (int j = 0; j < polygon.count; j++)
            {
                for (int n = 0; n < polygon.pol[j].nverts; n++)
                {
                    for (int k = 0; k < 3; k++)
                        polygon.pol[j].v[n].norm[k] = polygon.pol[j].norm[k];

                    if (polygon.pol[j].surf.smooth <= 0)
                        continue;

                    p = polygon.pol[j].v[n].index;

                    for (int g = 0; g < pt[p].npols; g++)
                    {
                        h = pt[p].pol[g];
                        if (h == j)
                            continue;

                        if (polygon.pol[j].smoothgrp != polygon.pol[h].smoothgrp)
                            continue;
                        a = LwVecMath.vecangle(polygon.pol[j].norm, polygon.pol[h].norm);
                        if (a > polygon.pol[j].surf.smooth)
                            continue;

                        for (int k = 0; k < 3; k++)
                            polygon.pol[j].v[n].norm[k] += polygon.pol[h].norm[k];
                    }

                    LwVecMath.normalize(polygon.pol[j].v[n].norm);
                }
            }
        }
    }
}
