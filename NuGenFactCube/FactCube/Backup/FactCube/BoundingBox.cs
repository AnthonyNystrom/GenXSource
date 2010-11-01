using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

namespace Genetibase.FactCube
{
	public class BoundingBox : IBounds
	{
        private bool initialized;
		private Point3D min;
		private Point3D max;

        /// <summary>
        /// Constructor
        /// </summary>
        public BoundingBox()
        {
        }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="min">Minimum extent</param>
		/// <param name="max">Maximum extent</param>
		public BoundingBox( Point3D min, Point3D max )
		{
			this.min = min;
			this.max = max;
            initialized = true;
		}

        /// <summary>
        /// Construct from point list
        /// </summary>
        /// <param name="positions">Positions</param>
        public BoundingBox( IEnumerable<Point3D> positions )
        {
            Merge( positions );
        }

		/// <summary>
		/// Get corners
		/// </summary>
		/// <returns>Array of corners</returns>
		public IList<Point3D> Positions
		{
            get
            {
                if ( !initialized ) return null;

			    return new Point3D[]
				    {
					    new Point3D( min.X, max.Y, max.Z ),
					    new Point3D( max.X, max.Y, max.Z ),
					    new Point3D( max.X, min.Y, max.Z ),
					    new Point3D( min.X, min.Y, max.Z ),
					    new Point3D( min.X, max.Y, min.Z ),
					    new Point3D( max.X, max.Y, min.Z ),
					    new Point3D( max.X, min.Y, min.Z ),
					    new Point3D( min.X, min.Y, min.Z )
				    };
            }
		}

		/// <summary>
		/// Determine if box intersects plane
		/// </summary>
		/// <param name="plane">Plane</param>
		/// <returns>Intersection type</returns>
		/// <remarks>Taken from XNA source via Reflector</remarks>
		public PlaneIntersectionType Intersects( Plane plane )
		{
			Point3D vector = new Point3D();
			Point3D vector2 = new Point3D();

			vector2.X = ( plane.Normal.X >= 0f ) ? min.X : max.X;
			vector2.Y = ( plane.Normal.Y >= 0f ) ? min.Y : max.Y;
			vector2.Z = ( plane.Normal.Z >= 0f ) ? min.Z : max.Z;
			vector.X = ( plane.Normal.X >= 0f ) ? max.X : min.X;
			vector.Y = ( plane.Normal.Y >= 0f ) ? max.Y : min.Y;
			vector.Z = ( plane.Normal.Z >= 0f ) ? max.Z : min.Z;

			double num = ( ( plane.Normal.X * vector2.X ) + ( plane.Normal.Y * vector2.Y ) ) + ( plane.Normal.Z * vector2.Z );
			if ( ( num + plane.D ) > 0f ) return PlaneIntersectionType.Front;

			num = ( ( plane.Normal.X * vector.X ) + ( plane.Normal.Y * vector.Y ) ) + ( plane.Normal.Z * vector.Z );
			if ( ( num + plane.D ) < 0f ) return PlaneIntersectionType.Back;

			return PlaneIntersectionType.Intersecting;
		}

        /// <summary>
        /// Get union of bounds
        /// </summary>
        /// <param name="bounds">Bounds</param>
        /// <returns>Union of bounds</returns>
        public void Merge( IEnumerable<Point3D> positions )
        {
            foreach ( Point3D p in positions )
            {
                if ( !initialized )
                {
                    min = p;
                    max = p;
                    initialized = true;
                }
                else
                {
                    if ( p.X < min.X ) min.X = p.X;
                    if ( p.Y < min.Y ) min.Y = p.Y;
                    if ( p.Z < min.Z ) min.Z = p.Z;
                    if ( p.X > max.X ) max.X = p.X;
                    if ( p.Y > max.Y ) max.Y = p.Y;
                    if ( p.Z > max.Z ) max.Z = p.Z;
                }
            }
        }

        /// <summary>
        /// Get union of bounds
        /// </summary>
        /// <param name="bounds">Bounds</param>
        /// <returns>Union of bounds</returns>
        public void Merge( IBounds bounds )
        {
            Merge( bounds.Positions );
        }

        /// <summary>
        /// Get minimum
        /// </summary>
		public Point3D Min { get { return min; } }

        /// <summary>
        /// Get maximum
        /// </summary>
		public Point3D Max { get { return max; } }
	}
}