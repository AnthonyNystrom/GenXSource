using System;
using System.Drawing;
using NuGenRenderOptics;

namespace Genetibase.NuGenRenderOptics.MDX1.HeightFields
{
    class HeightFieldObject : OpticalSceneObject
    {
        Vector2D areaStart;
        Vector2D areaEnd;
        Vector2D areaExtent;

        Bitmap bitmapSrc;
        HeightField hField;

        Rectangle3D[] rectangles;

        float zTop, zMin;

        Vector2D onePx;

        public HeightFieldObject(Vector3D origin, MaterialShader shader, Vector2D size, Bitmap bitmapSrc)
            : base(origin, shader, -1)
        {
            this.bitmapSrc = bitmapSrc;
            // calculate bounds
            areaExtent = size;
            Vector2D halfSize = size * 0.5;
            areaStart = origin - halfSize;
            areaEnd = origin + halfSize;
            double szLen = size.Length();
            radius = Math.Sqrt((szLen * szLen) + (1.1 * 1.1)) / 2;

            hField = HeightField.FromBitmap(bitmapSrc);

            onePx = new Vector2D(areaExtent.X / hField.Width, areaExtent.Y / hField.Height);

            this.origin.Y = 0.5;

            zMin = 0;
            zTop = 1;

            rectangles = new Rectangle3D[]
            {
                new Rectangle3D(new Vector3D(areaStart.X, zTop, areaStart.Y), new Vector3D(areaEnd.X, zTop, areaEnd.Y),
                                new PlaneD(new Vector3D(0, 1, 0), -zTop)),           // top
                new Rectangle3D(new Vector3D(areaStart.X, zMin, areaStart.Y), new Vector3D(areaEnd.X, zMin, areaEnd.Y),
                                new PlaneD(new Vector3D(0, -1, 0), zMin)),          // bottom
                new Rectangle3D(new Vector3D(areaStart.X, zMin, areaStart.Y), new Vector3D(areaStart.X, zTop, areaEnd.Y), 
                                new PlaneD(new Vector3D(-1, 0, 0), halfSize.X)),    // left
                new Rectangle3D(new Vector3D(areaEnd.X, zMin, areaStart.Y), new Vector3D(areaEnd.X, zTop, areaEnd.Y),
                                new PlaneD(new Vector3D(1, 0, 0), -halfSize.X)),     // right
                new Rectangle3D(new Vector3D(areaStart.X, zMin, areaStart.Y), new Vector3D(areaEnd.X, zTop, areaStart.Y),
                                new PlaneD(new Vector3D(0, 0, -1), halfSize.Y)),    // front 
                new Rectangle3D(new Vector3D(areaStart.X, zMin, areaEnd.Y), new Vector3D(areaEnd.X, zTop, areaEnd.Y),
                                new PlaneD(new Vector3D(0, 0, 1), -halfSize.Y)),     // back
            };
        }
        // TODO: Need some advanced way of passing normal etc. back at intersection point on advanced objects?

        public override bool GetIntersect(Vector3D p1, Vector3D p2, Vector3D dirUv, out Vector3D iPos, out double iDist,
                                          out uint subIdx)
        {
            // intersect test with 6 planes
            Vector3D lowIntersection = Vector3D.Empty, highIntersection = Vector3D.Empty;
            double lowIntersectDist = double.MaxValue, highIntersectDist = double.MinValue;
            int iCount = 0;
            for (int i = 0; i < rectangles.Length; i++)
            {
                Vector3D intersection;
                if (rectangles[i].IntersectWithLine(p1, dirUv, out intersection))
                {
                    iCount++;
                    // see if beats lowest or highest
                    double dist = (intersection - p1).Length();
                    if (dist < lowIntersectDist)
                    {
                        lowIntersectDist = dist;
                        lowIntersection = intersection;
                    }
                    if (dist > highIntersectDist)
                    {
                        highIntersectDist = dist;
                        highIntersection = intersection;
                    }
                }
            }

            if (iCount == 1)
            {
                // see if 1 of the points is already within the volume
                if (p1.X >= areaStart.X && p1.X <= areaEnd.X &&
                    p1.Y >= 0 && p1.Y <= 1 &&
                    p1.Z >= areaStart.Y && p1.Z <= areaEnd.Y)
                {
                    lowIntersectDist = 0;
                    lowIntersection = p1;
                }
                else if (p2.X >= areaStart.X && p2.X <= areaEnd.X &&
                    p2.Y >= 0 && p2.Y <= 1 &&
                    p2.Z >= areaStart.Y && p2.Z <= areaEnd.Y)
                {
                    highIntersectDist = (p2 - p1).Length();
                    highIntersection = p2;
                }
                iCount++;
            }
            if (iCount > 1)
            {
                // just do straight lookups for now -- slow way!
                subIdx = 0;
                bool result = hField.SampleViaRay(new Vector3D((lowIntersection.X - areaStart.X) / areaExtent.X,
                                                               lowIntersection.Y,
                                                               (lowIntersection.Z - areaStart.Y) / areaExtent.Y),
                                                               lowIntersectDist, dirUv,
                                                  new Vector3D((highIntersection.X - areaStart.X) / areaExtent.X,
                                                               highIntersection.Y,
                                                               (highIntersection.Z - areaStart.Y) / areaExtent.Y),
                                                  highIntersectDist, out iPos, out iDist);
                if (result)
                {
                    //iPos.X = (iPos.X * areaExtent.X) + areaStart.X;
                    //iPos.Z = (iPos.Z * areaExtent.Y) + areaStart.Y;
                    iDist += lowIntersectDist;
                    iPos = p1 - (iDist * dirUv);
                    return true;
                }

                // find first intersection through field tree
                //hField.

                /*iPos = lowIntersection;
                iDist = lowIntersectDist;
                subIdx = 0;
                return true;*/
            }
            iDist = -1;
            iPos = Vector3D.Empty;
            subIdx = 0;
            return false;
        }

        public override Vector3D GetNormal(Vector3D pos, uint subIdx)
        {
            Vector2D coord = new Vector2D(pos.X - areaStart.X, pos.Z - areaStart.Y);
            coord.X /= areaExtent.X;
            coord.Y /= areaExtent.Y;
            
            // calc normal from surrounding pixels
            float val = hField.Sample(coord);
            float valxmin = hField.SampleShiftX(coord.X, coord.Y, -1);
            float valxmax = hField.SampleShiftX(coord.X, coord.Y, 1);

            Vector3D rightAngle = new Vector3D(0, 0, -1);
            Vector3D p1 = new Vector3D(onePx.X, valxmax - val, 0).Cross(rightAngle);
            Vector3D p2 = new Vector3D(-onePx.X, valxmin - val, 0).Cross(rightAngle);
            Vector3D xDir = Vector3D.Normalize(p1 - p2);

            float valymin = hField.SampleShiftY(coord.X, coord.Y, -1);
            float valymax = hField.SampleShiftY(coord.X, coord.Y, 1);

            rightAngle = new Vector3D(1, 0, 0);
            p1 = new Vector3D(0, valymax - val, onePx.Y).Cross(rightAngle);
            p2 = new Vector3D(0, valymin - val, -onePx.Y).Cross(rightAngle);
            Vector3D yDir = Vector3D.Normalize(p1 - p2);
            
            return Vector3D.Normalize(xDir + yDir);
        }

        public override Vector2D GetTexCoord(Vector3D p, uint subIdx)
        {
            Vector2D coord = new Vector2D(p.X - areaStart.X, p.Z - areaStart.Y);
            coord.X /= areaExtent.X;
            coord.Y /= areaExtent.Y;
            return coord;
        }
    }
}