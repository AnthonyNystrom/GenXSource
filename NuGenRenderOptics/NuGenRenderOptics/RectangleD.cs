using System;

namespace NuGenRenderOptics
{
    class Rectangle3D
    {
        Vector3D min, max;
        PlaneD plane;

        public Rectangle3D(Vector3D min, Vector3D max, PlaneD plane)
        {
            this.min = min;
            this.max = max;
            this.plane = plane;
        }

        public PlaneD Plane
        {
            get { return plane; }
        }

        public bool IntersectWithLine(Vector3D linePoint, Vector3D lineDir,
                                      out Vector3D intersectPoint)
        {
            // first intersect with plane
            // TODO: Select if backfacing allowed or not
            if (/*plane.IsFrontFacting(-lineDir) &&*/ plane.GetIntersectionWithLineIgnoreBackfacing(linePoint, lineDir, out intersectPoint))
            {
                // round the intersection off slightly
                intersectPoint.Round(10);
                // check within rectangle area
                if (intersectPoint >= min && intersectPoint <= max)
                {
                    return true;
                }
            }
            intersectPoint = Vector3D.Empty;
            return false;
        }

        public bool IntersectWithLimitedLine(Vector3D linePoint1, Vector3D linePoint2,
                                             Vector3D lineDir, out Vector3D intersectPoint)
        {
            // first intersect with plane
            if (plane.GetIntersectionWithLimitedLine(linePoint1, linePoint2, lineDir, out intersectPoint))
            {
                // check within rectangle area
                if (intersectPoint >= min && intersectPoint <= max)
                {
                    return true;
                }
            }
            return false;
        }
    }
}