using System.Collections.Generic;
using System.Drawing;
using Genetibase.NuGenRenderOptics.MDX1.Rasterization;

namespace NuGenRenderOptics
{
    class SimpleSceneManager : ISceneManager
    {
        List<IOpticalSceneObject> objects;
        Light[] lights;
        RGBA_D ambient;

        public SimpleSceneManager(RGBA_D ambient)
        {
            this.ambient = ambient;
            objects = new List<IOpticalSceneObject>();
            lights = new Light[] {
                            new PointLight(Color.White, new Vector3D(10, 10, -10), 1, 200, 0.022, 0.0019),
                            //new PointLight(Color.Red, new Vector3D(-10, 10, 10), 1, 200, 0.022, 0.0019)
            };
        }

        #region ISceneManager Members

        public Light[] Lights
        {
            get { return lights; }
        }

        public RGBA_D Ambient
        {
            get { return ambient; }
        }

        public bool GetFirstIntersection(Vector3D origin, Vector3D dir, double maxLength, out IOpticalSceneObject obj,
                                         out Vector3D iPos, out double iDistance, out uint subIdx)
        {
            // intersect all bounds
            Vector3D endPt = origin + (dir * maxLength);
            double nDist = double.MaxValue;
            int nIdx = -1;
            Vector3D nPt = Vector3D.Empty;
            subIdx = 0;
            for (int i = 0; i < objects.Count; i++)
            {
                double dist = 0;
                if (objects[i].Radius == -1 ||
                    (SphereSceneObject.IntersectRaySphere(origin, dir, objects[i].Origin, objects[i].Radius * objects[i].Radius,
                    out dist) && dist < nDist))
                {
                    // try actual intesection
                    Vector3D pt;
                    if (objects[i].GetIntersect(origin, endPt, dir, out pt, out dist, out subIdx) && dist < nDist && dist > 0.01)
                    {
                        nDist = dist;
                        nIdx = i;
                        nPt = pt;
                    }
                    /*else
                    {
                        nDist = dist;
                        nIdx = i;
                        nPt = pt;
                        subIdx = 3;
                    }*/
                }
            }

            // pass back intersection if falls within rays remaining length
            if (nIdx != -1 && nDist < maxLength)
            {
                obj = objects[nIdx];
                iPos = nPt; // even needed?
                iDistance = nDist;
                return true;
            }
            obj = null;
            iPos = Vector3D.Empty;
            iDistance = double.NaN;
            return false;
        }

        public void AddObject(IOpticalSceneObject obj)
        {
            objects.Add(obj);
        }

        public void RemoveObject(IOpticalSceneObject obj)
        {
            objects.Remove(obj);
        }

        public bool TestForContents(CameraView view, Rectangle area, double length)
        {
            // TODO: Not working when y is high and other axis are low
            double xScale = 0.5 / view.Area.Width;//area.Width;
            double yScale = 0.5 / view.Area.Height;//area.Height;

            // project a simple rectangle out (6xplane)
            double xShift = -0.25 + (xScale * area.Left);
            double yShift = 0;
            Vector3D offset = ((view.XUV * xShift) + (view.YUV * yShift));
            Vector3D rayDir = offset + view.Direction;
            rayDir.Normalize();
            PlaneD left = PlaneD.FromPointNormal(view.Centre, rayDir.Cross(Vector3D.Up));

            xShift = -0.25 + (xScale * area.Right);
            offset = ((view.XUV * xShift) + (view.YUV * yShift));
            rayDir = offset + view.Direction;
            rayDir.Normalize();
            PlaneD right = PlaneD.FromPointNormal(view.Centre, -rayDir.Cross(Vector3D.Up));

            xShift = 0;
            yShift = -0.25 + (yScale * area.Top);
            offset = ((view.XUV * xShift) + (view.YUV * yShift));
            rayDir = offset + view.Direction;
            rayDir.Normalize();
            PlaneD top = PlaneD.FromPointNormal(view.Centre, -rayDir.Cross(view.XUV));

            yShift = -0.25 + (yScale * area.Bottom);
            offset = (view.XUV * xShift) + (view.YUV * yShift);
            rayDir = offset + view.Direction;
            rayDir.Normalize();
            PlaneD bottom = PlaneD.FromPointNormal(view.Centre, rayDir.Cross(view.XUV));

            // test all objects until we get a hit
            foreach (IOpticalSceneObject obj in objects)
            {
                if (obj.Radius != -1)
                {
                    double leftD = left.DistanceToPoint(obj.Origin);
                    double rightD = right.DistanceToPoint(obj.Origin);
                    double topD = top.DistanceToPoint(obj.Origin);
                    double bottomD = bottom.DistanceToPoint(obj.Origin);
                    if (leftD - obj.Radius <= 0 &&
                        rightD - obj.Radius <= 0 &&
                        topD - obj.Radius <= 0 &&
                        bottomD - obj.Radius <= 0)
                        return true;
                }
                else
                    return true;
            }
            return false;
        }

//        public bool TestForContents(Vector3D origin, Vector3D uv, Vector2D extentStart, Vector2D extentEnd, double length, CameraView view)
//        {
//            // scale view frustum
//            PlaneD[] frustum = new PlaneD[4];
//            frustum[0] = PlaneD.FromPointNormal(origin, view.Frustum[0]);
//            frustum[1] = PlaneD.FromPointNormal(origin + (view.XUV * extentEnd.X), view.Frustum[1]);
//            frustum[2] = PlaneD.FromPointNormal(origin, view.Frustum[2]);
//            frustum[3] = PlaneD.FromPointNormal(origin + (view.YUV * extentEnd.Y), view.Frustum[3]);
//
//            foreach (IOpticalSceneObject obj in objects)
//            {
//                if (frustum[0].DistanceToPoint(obj.Origin) + obj.Radius > 0 &&
//                    frustum[1].DistanceToPoint(obj.Origin) + obj.Radius > 0 &&
//                    frustum[2].DistanceToPoint(obj.Origin) + obj.Radius > 0 &&
//                    frustum[3].DistanceToPoint(obj.Origin) + obj.Radius > 0)
//                    return true;
//            }
//            return false;
//        }
        #endregion

//        /// <summary>
//        /// http://www.devmaster.net/wiki/Ray-sphere_intersection
//        /// </summary>
//        static bool IntersectRaySphere(Vector3D rOrigin, Vector3D rUV, Vector3D sOrigin,
//                                       double radius2, out double distance)
//        {
//            Vector3D dst = rOrigin - sOrigin;
//            double B = Vector3D.Dot(dst, rUV);
//            double C = Vector3D.Dot(dst, dst) - radius2;
//            double D = B * B - C;
//            if (D > 0)
//            {
//                distance = -B - Math.Sqrt(D);
//                return true;
//            }
//            distance = -1;
//            return false;
//        }
    }
}