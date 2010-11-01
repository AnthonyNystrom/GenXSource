using System;

namespace NuGenRenderOptics
{
    struct Vector3D
    {
        public double X, Y, Z;

        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3D Empty
        {
            get { return new Vector3D(0, 0, 0); }
        }

        public static Vector3D Up
        {
            get { return new Vector3D(0, 1, 0); }
        }

        #region Member Methods

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public void Normalize()
        {
            double mod_v = Length();
            if (!(Math.Abs(mod_v) < 1.0E-10))
            {
                X = X / mod_v;
                Y = Y / mod_v;
                Z = Z / mod_v;
            }
        }

        public double Dot(Vector3D b)
        {
            return (X * b.X) + (Y * b.Y) + (Z * b.Z);
        }

        public Vector3D Cross(Vector3D w)
        {
            // u x w
            return new Vector3D(w.Z * Y - w.Y * Z, w.X * Z - w.Z * X, w.Y * X - w.X * Y);
        }

        public override string ToString()
        {
            return string.Format("X:{0},Y:{1},Z:{2}", X, Y, Z);
        }

        /// <summary>
        /// Returns if the point represented by this vector is between to points
        ///</summary>
        ///<param name="begin">Start point of line</param>
        ///<param name="end">End point of line</param>
        ///<returns> True if between points, false if not. </returns>
        public bool IsBetweenPoints(Vector3D begin, Vector3D end)
        {
            double f = (end - begin).Length();
            return GetDistanceFromSQ(begin) < f &&
                   GetDistanceFromSQ(end) < f;
        }

        /// <summary>
        /// Returns squared distance from an other point.
        /// Here, the vector is interpreted as point in 3 dimensional space.
        /// </summary>
        public double GetDistanceFromSQ(Vector3D other)
        {
            double vx = X - other.X; double vy = Y - other.Y; double vz = Z - other.Z;
            return (vx*vx + vy*vy + vz*vz);
        }

        public void Round(int digits)
        {
            X = Math.Round(X, digits);
            Y = Math.Round(Y, digits);
            Z = Math.Round(Z, digits);
        }
        #endregion

        #region Static Methods

        public static Vector3D operator -(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public static Vector2D operator -(Vector3D left, Vector2D right)
        {
            return new Vector2D(left.X - right.X, left.Z - right.Y);
        }

        public static Vector3D operator -(Vector3D vec)
        {
            return new Vector3D(-vec.X, -vec.Y, -vec.Z);
        }

        public static Vector3D operator *(Vector3D left, double right)
        {
            return new Vector3D(left.X * right, left.Y * right, left.Z * right);
        }

        public static Vector3D operator *(double right, Vector3D left)
        {
            return new Vector3D(left.X * right, left.Y * right, left.Z * right);
        }

        public static Vector3D operator +(Vector3D left, Vector3D right)
        {
            return new Vector3D(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Vector2D operator +(Vector3D left, Vector2D right)
        {
            return new Vector2D(left.X + right.X, left.Z + right.Y);
        }

        public static bool operator !=(Vector3D left, Vector3D right)
        {
            return (left.X != right.X || left.Y != right.Y || left.Z != right.Z);
        }

        public static bool operator ==(Vector3D left, Vector3D right)
        {
            return (left.X == right.X && left.Y == right.Y && left.Z == right.Z);
        }

        public static bool operator >(Vector3D left, Vector3D right)
        {
            return (left.X > right.X && left.Y > right.Y && left.Z > right.Z);
        }

        public static bool operator <(Vector3D left, Vector3D right)
        {
            return (left.X < right.X && left.Y < right.Y && left.Z < right.Z);
        }

        public static bool operator >=(Vector3D left, Vector3D right)
        {
            return (left.X >= right.X && left.Y >= right.Y && left.Z >= right.Z);
        }

        public static bool operator <=(Vector3D left, Vector3D right)
        {
            return (left.X <= right.X && left.Y <= right.Y && left.Z <= right.Z);
        }

        public static double Dot(Vector3D a, Vector3D b)
        {
            return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z);
        }

        public static double modv(Vector3D v)
        {
            return Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);
        }

        public static Vector3D Normalize(Vector3D source)
        {
            double mod_v = modv(source);
            if (Math.Abs(mod_v) < 1.0E-10)
                return Empty;
            return new Vector3D(source.X / mod_v, source.Y / mod_v, source.Z / mod_v);
        }

        public static Vector3D Reflect(Vector3D i, Vector3D n)
        {
            return Normalize(new Vector3D(i.X - (2.0 * n.X * Dot(n, i)),
                                          i.Y - (2.0 * n.Y * Dot(n, i)),
                                          i.Z - (2.0 * n.Z * Dot(n, i))));
        }

        public static Vector3D Cross(Vector3D u, Vector3D w)
        {
            // u x w
            return new Vector3D(w.Z * u.Y - w.Y * u.Z, w.X * u.Z - w.Z * u.X, w.Y * u.X - w.X * u.Y);
        }

        public static double GetCosAngle(Vector3D v1, Vector3D v2)
        {
            /* incident angle
            // inters pt (i)
            double ix, iy, iz;
            ix = px+t*vx;
            iy = py+t*vy;
            iz = pz+t*vz;

            // normal at i
            double nx, ny, nz;
            nx = ix - cx;
            ny = iy - cy;
            nz = iz - cz;
            */
            v1.Normalize();
            v2.Normalize();

            // cos(t) = (v.w) / (|v|.|w|)
            double n = (v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z);
            double d = (modv(v1) * modv(v2));

            if (Math.Abs(d) < 1.0E-10)
                return 0;
            return n / d;
        }

        public static double GetCoord(double i1, double i2,
                                      double w1, double w2,
                                      double p)
        {
            return ((p - i1) / (i2 - i1)) * (w2 - w1) + w1;
        }

        public static Vector3D Refract(double n1, double n2,
                                   Vector3D i,
                                   Vector3D mirror)
        {
            double c1 = -Dot(mirror, i);
            double n = n1 / n2;

            double c2 = Math.Sqrt(1.0 - n * n * (1.0 - c1 * c1));
            return new Vector3D((n * i.X) + (n * c1 - c2) * mirror.X,
                                (n * i.Y) + (n * c1 - c2) * mirror.Y,
                                (n * i.Z) + (n * c1 - c2) * mirror.Z);
        }

        public static void RotX(double angle, ref double y, ref double z)
        {
            double y1 = y * Math.Cos(angle) - z * Math.Sin(angle);
            double z1 = y * Math.Sin(angle) + z * Math.Cos(angle);
            y = y1;
            z = z1;
        }

        public static void RotY(double angle, ref double x, ref double z)
        {
            double x1 = x * Math.Cos(angle) - z * Math.Sin(angle);
            double z1 = x * Math.Sin(angle) + z * Math.Cos(angle);
            x = x1;
            z = z1;
        }

        public static void RotZ(double angle, ref double x, ref double y)
        {
            double x1 = x * Math.Cos(angle) - y * Math.Sin(angle);
            double y1 = x * Math.Sin(angle) + y * Math.Cos(angle);
            x = x1;
            y = y1;
        }
        #endregion
    }
}
