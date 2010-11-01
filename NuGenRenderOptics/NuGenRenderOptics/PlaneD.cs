namespace NuGenRenderOptics
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>http://irrlicht.cvs.sourceforge.net/irrlicht/Irrlicht/Irrlicht.NET/Plane3D.h?revision=1.2&view=markup</remarks>
    struct PlaneD
    {
        readonly Vector3D Normal;
        readonly double D;

        public PlaneD(Vector3D normal, double distance)
        {
            Normal = normal;
            D = distance;
        }

        public Vector3D PlaneNormal
        {
            get { return Normal; }
        }

        public static PlaneD FromPoints(Vector3D p1, Vector3D p2, Vector3D p3)
        {
            Vector3D Normal = (p2 - p1).Cross(p3 - p1);
            Normal.Normalize();
            return new PlaneD(Normal, -p1.Dot(Normal));
        }

        public static PlaneD FromPointNormal(Vector3D point, Vector3D normal)
        {
            return new PlaneD(normal, -point.Dot(normal));
        }

        public static PlaneD FromPointNormal(Vector3D point, PlaneD normal)
        {
            return new PlaneD(normal.Normal, -point.Dot(normal.Normal));
        }

        public double DistanceToPoint(Vector3D point)
        {
            return point.Dot(Normal) + D;
        }

        /// <summary>
        /// Returns an intersection with a 3d line.
        /// </summary>
        /// <param name="lineVect"> Vector of the line to intersect with.</param>
        /// <param name="linePoint"> Point of the line to intersect with.</param>
        /// <param name="outIntersection"> Place to store the intersection point, if there is one.</param>
        /// <returns> Returns true if there was an intersection, false if there was not.</returns>
        public bool GetIntersectionWithLine(Vector3D linePoint, Vector3D lineVect,
                                            out Vector3D outIntersection)
        {
            double t2 = Normal.Dot(lineVect);

            if (t2 == 0)
            {
                outIntersection = Vector3D.Empty;
                return false;
            }
            double t = -(Normal.Dot(linePoint) + D) / t2;
            outIntersection = linePoint + (lineVect * t);
            return true;
        }

        /// <summary>
        /// Returns an intersection with a 3d line.
        /// </summary>
        /// <param name="lineVect"> Vector of the line to intersect with.</param>
        /// <param name="linePoint"> Point of the line to intersect with.</param>
        /// <param name="outIntersection"> Place to store the intersection point, if there is one.</param>
        /// <returns> Returns true if there was an intersection, false if there was not.</returns>
        public bool GetIntersectionWithLineIgnoreBackfacing(Vector3D linePoint, Vector3D lineVect,
                                                            out Vector3D outIntersection)
        {
            if (!IsFrontFacting(lineVect))
            {
                double t2 = Normal.Dot(lineVect);
                if (t2 == 0)
                {
                    outIntersection = Vector3D.Empty;
                    return false;
                }
                double t = -(Normal.Dot(linePoint) + D) / t2;
                outIntersection = linePoint + (lineVect * t);
            }
            else
            {
                double t2 = -Normal.Dot(lineVect);
                if (t2 == 0)
                {
                    outIntersection = Vector3D.Empty;
                    return false;
                }
                double t = -(-Normal.Dot(linePoint) + D) / t2;
                outIntersection = linePoint + (lineVect * t);
            }
            return true;
        }

        /// <summary>
        /// Returns an intersection with a 3d line, limited between two 3d points.
        /// </summary>
        /// <param name="linePoint1">Point 1 of the line.</param>
        /// <param name="linePoint2">Point 2 of the line.</param>
        /// <param name="dir"></param>
        /// <param name="outIntersection">Place to store the intersection point, if there is one.</param>
        /// <returns> Returns true if there was an intersection, false if there was not.</returns>
        public bool GetIntersectionWithLimitedLine(Vector3D linePoint1, Vector3D linePoint2, Vector3D dir,
                                                   out Vector3D outIntersection)
        {
            return (GetIntersectionWithLine(linePoint1, dir, out outIntersection) &&
                    outIntersection.IsBetweenPoints(linePoint1, linePoint2));
        }

        /// <summary>
        /// Returns if the plane is front of backfacing. Note that this only
        /// works if the normal is Normalized.
        /// </summary>
        /// <param name="lookDirection"> Look direction.</param>
        /// <returns> Returns true if the plane is front facing, which mean it would
        /// be visible, and false if it is backfacing.</returns>
        public bool IsFrontFacting(Vector3D lookDirection)
        {
            return Normal.Dot(lookDirection) <= 0.0f;
        }
    }
}