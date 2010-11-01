using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

namespace Genetibase.FactCube
{
    public class DepthPlanes
    {
        private List<Plane> planes = new List<Plane>();

        public DepthPlanes( IEnumerable<Plane> planes )
        {
            this.planes.AddRange( planes );
        }

        public DepthPlanes( ProjectionCamera camera, BoundingBox bounds, double step )
        {
            // Find closest visible point to camera
            Point3D position = camera.Transform.Transform( camera.Position );
            Vector3D direction = camera.Transform.Transform( camera.LookDirection );
            Point3D lookAt = position + ( direction * camera.NearPlaneDistance );
            double xDir = Normalize( direction.X );
            double yDir = Normalize( direction.Y );
            double zDir = Normalize( direction.Z );

            double biggest = Math.Max( Math.Max( Math.Abs( lookAt.X ), Math.Abs( lookAt.Y ) ), Math.Abs( lookAt.Z ) );

            if ( xDir != 0 ) planes.Add( new Plane( new Vector3D( -xDir, 0, 0 ), -Distance( lookAt.X, biggest, bounds.Max.X, step ) ) );
            if ( yDir != 0 ) planes.Add( new Plane( new Vector3D( 0, -yDir, 0 ), -Distance( lookAt.Y, biggest, bounds.Max.Y, step ) ) );
            if ( zDir != 0 ) planes.Add( new Plane( new Vector3D( 0, 0, -zDir ), -Distance( lookAt.Z, biggest, bounds.Max.Z, step ) ) );
        }

        private double Normalize( double value )
        {
            return ( value < -0.1e-5 ) ? -1 : ( value > 0.1e-5 ) ? 1 : 0;
        }

        private double Distance( double value, double biggest, double max, double depth )
        {
            double abs = Math.Abs( value );
            return ( abs < biggest && abs < max ) ? max : Math.Min( max - depth, abs );
        }

        public bool Includes( IBounds bounds )
        {
            bool visible = false;

            foreach ( Plane plane in planes )
            {
                if ( bounds.Intersects( plane ) != PlaneIntersectionType.Back )
                {
                    visible = true;
                    break;
                }
            }

            return visible;
        }

        public bool Includes( Point3D point )
        {
            bool visible = false;

            foreach ( Plane plane in planes )
            {
                if ( plane.Classify( point ) != Halfspace.Negative )
                {
                    visible = true;
                    break;
                }
            }

            return visible;
        }
    }
}