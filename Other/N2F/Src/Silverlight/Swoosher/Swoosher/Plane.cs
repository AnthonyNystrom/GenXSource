using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Next2Friends.Swoosher.Media3D;

namespace Next2Friends.Swoosher
{
    /// <summary>
    /// Plane structure
    /// </summary>
    public struct Plane
    {
        private Vector3D normal;
        private double d;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="normal">Plane normal</param>
        /// <param name="d">Distance of plane from origin</param>
        public Plane( Vector3D point, Vector3D normal )
            : this( normal, -Vector3D.DotProduct( normal, (Vector3D)point ) )
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="normal">Plane normal</param>
        /// <param name="d">Distance of plane from origin</param>
        public Plane( Vector3D normal, double d )
        {
            this.normal = normal;
            this.d = d;
        }

        public Vector3D Normal { get { return normal; } set { normal = value; } }
        public double D { get { return d; } set { d = value; } }

        /// <summary>
        /// Calculate distance to point
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>Distance</returns>
		public double Distance( Point3D point )
        {
            return normal.X * point.X + normal.Y * point.Y + normal.Z * point.Z + d;
        }

        /// <summary>
        /// Classify point relative to plane
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>Intersection classification</returns>
		public Halfspace Classify( Point3D point )
        {
            return Classify( Distance( point ) );
        }

        /// <summary>
        /// Classify point relative to plane
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>Intersection classification</returns>
        public Halfspace Classify( double distance )
        {
            return ( distance < 0 ) ? Halfspace.Negative : ( distance > 0 ) ? Halfspace.Positive : Halfspace.Coincident;
            //			return ( distance < -0.1e-10 ) ? Halfspace.Negative : ( distance > 0.1e-10 ) ? Halfspace.Positive : Halfspace.Coincident;
        }

        /// <summary>
        /// Normalize plane
        /// </summary>
        public void Normalize()
        {
            float mag = (float)Math.Sqrt( normal.X * normal.X + normal.Y * normal.Y + normal.Z * normal.Z );
            normal /= mag;
            d /= mag;
        }
    }
}