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
    public class Frustum
    {
        private Plane[] planes = new Plane[ 6 ];

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="matrix">Combined matrix</param>
        /// <param name="normalize">Normalize?</param>
        public Frustum( Matrix3D matrix, bool normalize )
        {
            // Left clipping plane
            planes[ 0 ].Normal = new Vector3D( matrix.M14 + matrix.M11, matrix.M24 + matrix.M21, matrix.M34 + matrix.M31 );
            planes[ 0 ].D = matrix.M44 + matrix.OffsetX;

            // Right clipping plane
			planes[ 1 ].Normal = new Vector3D( matrix.M14 - matrix.M11, matrix.M24 - matrix.M21, matrix.M34 - matrix.M31 );
            planes[ 1 ].D = matrix.M44 - matrix.OffsetX;

            // Top clipping plane
			planes[ 2 ].Normal = new Vector3D( matrix.M14 - matrix.M12, matrix.M24 - matrix.M22, matrix.M34 - matrix.M32 );
            planes[ 2 ].D = matrix.M44 - matrix.OffsetY;

            // Bottom clipping plane
			planes[ 3 ].Normal = new Vector3D( matrix.M14 + matrix.M12, matrix.M24 + matrix.M22, matrix.M34 + matrix.M32 );
            planes[ 3 ].D = matrix.M44 + matrix.OffsetY;

            // Near clipping plane
			planes[ 4 ].Normal = new Vector3D( matrix.M13, matrix.M23, matrix.M33 );
            planes[ 4 ].D = matrix.OffsetZ;

            // Far clipping plane
			planes[ 5 ].Normal = new Vector3D( matrix.M14 - matrix.M13, matrix.M24 - matrix.M23, matrix.M34 - matrix.M33 );
            planes[ 5 ].D = matrix.M44 - matrix.OffsetZ;

            // Normalize the plane equations, if requested
            if ( normalize )
            {
                for ( int index = 0; index < planes.Length; ++index ) planes[ index ].Normalize();
            }
        }

        /// <summary>
        /// Get planes
        /// </summary>
        public Plane[] Planes { get { return planes; } }

		/// <summary>
		/// Determine distance of point from frustum
		/// </summary>
		/// <param name="point">Point</param>
		/// <returns>Distance</returns>
		public double Distance( Point3D point )
		{
			double maximum = 0;

			foreach ( Plane plane in planes )
			{
				var distance = plane.Distance( point );
				if ( distance < 0 ) maximum = Math.Max( maximum, -distance );
			}

			return maximum;
		}

        /// <summary>
        /// Determine if point is contained by frustum
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>Containment type</returns>
        public ContainmentType Contains( Point3D point )
        {
            foreach ( Plane plane in planes )
            {
                if ( plane.Distance( point ) < 1E-05f ) return ContainmentType.Disjoint;
            }

            return ContainmentType.Contains;
        }

        /// <summary>
        /// Determine if bounding box is contained by frustum
        /// </summary>
        /// <param name="box">Bounding box</param>
        /// <returns>Containment type</returns>
        public ContainmentType Contains( IBounds bounds )
        {
            bool flag = false;

            foreach ( Plane plane in planes )
            {
                switch ( bounds.Intersects( plane ) )
                {
                    case PlaneIntersectionType.Back: return ContainmentType.Disjoint;

                    case PlaneIntersectionType.Intersecting:
                        {
                            flag = true;
                            break;
                        }
                }
            }

            return !flag ? ContainmentType.Contains : ContainmentType.Intersects;
        }
    }
}