using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Genetibase.FactCube
{
	public class BSPCube : IBSPItem
	{
		private Matrix3D matrix;
		private double materialOffset;
		private MeshGeometry3D geometry;
        private Point3D position;
        private List<Point3D> positions;
        private BoundingBox bounds;
		private string url;
		private static Matrix3D rotateY90 = new Matrix3D();

		/// <summary>
		/// Constructor )
		/// </summary>
		/// <param name="model">Model</param>
		public BSPCube( Matrix3D matrix, double materialOffset, MeshGeometry3D geometry, string url )
		{
			if ( rotateY90.IsIdentity ) rotateY90.Rotate( new Quaternion( new Vector3D( 0, 3, 0 ), 90 ) );
			this.matrix = matrix;
			this.materialOffset = materialOffset;
			this.geometry = geometry;
			this.url = url;

            position = new Point3D( matrix.OffsetX, matrix.OffsetY, matrix.OffsetZ );
            positions = new List<Point3D>( Transform( geometry, matrix ) );
            bounds = new BoundingBox( positions );
        }

        /// <summary>
        /// Transform mesh geometry
        /// </summary>
        /// <param name="geometry">Geometry</param>
        /// <param name="matrix">Matrix</param>
        /// <returns>Transformed vertices</returns>
        public static IEnumerable<Point3D> Transform( MeshGeometry3D geometry, Matrix3D matrix )
        {
            foreach ( Point3D point in geometry.Positions ) yield return matrix.Transform( point );
        }

		/// <summary>
		/// Classify position relative to plane
		/// </summary>
		/// <param name="plane">Plane</param>
		/// <param name="frontItem">Front item if split required</param>
		/// <param name="backItem">Back item if split required</param>
		/// <returns>Intersection classification</returns>
		public PlaneIntersectionType Intersects( Plane plane, out IBSPItem frontItem, out IBSPItem backItem )
		{
			frontItem = null;
			backItem = null;

            return bounds.Intersects( plane );
		}

        /// <summary>
        /// Get bounds
        /// </summary>
        public IBounds Bounds { get { return bounds; } }

		/// <summary>
		/// Get vertices
		/// </summary>
		public Point3D Position { get { return position; } }

        /// <summary>
        /// Get transformation matrix
        /// </summary>
        public Matrix3D Matrix { get { return matrix; } }

		/// <summary>
		/// Get material offset
		/// </summary>
        public double MaterialOffset { get { return materialOffset; } }

		/// <summary>
		/// Get URL
		/// </summary>
		public string Url { get { return url; } }

		/// <summary>
		/// Get flat (square) points facing camera
		/// </summary>
		/// <param name="cameraPos">Camera position</param>
		/// <param name="displayMatrix">View + projection + display matrix</param>
		/// <returns>Points</returns>
		protected Point3D[] GetFlatPoints( Point3D cameraPos, Matrix3D displayMatrix )
		{
			Vector3D perpendicular = Vector3D.CrossProduct( (Vector3D)position, (Vector3D)cameraPos );
			perpendicular.Normalize();

			Vector3D distance = bounds.Positions[ 0 ] - position;
			double radius = Math.Max( Math.Max( Math.Abs( distance.X ), Math.Abs( distance.Y ) ), Math.Abs( distance.Z ) );

			Point3D[] points = new Point3D[] { position, position + ( perpendicular * radius ) };
			displayMatrix.Transform( points );

			Point3D center = points[ 0 ];
			double half = ( points[ 1 ] - points[ 0 ] ).Length;

			points = new Point3D[] {
					new Point3D( center.X - half, center.Y - half, center.Z ),
					new Point3D( center.X + half, center.Y - half, center.Z ),
					new Point3D( center.X + half, center.Y + half, center.Z ),
					new Point3D( center.X - half, center.Y + half, center.Z ) };

			displayMatrix.Invert();
			displayMatrix.Transform( points );

			return points;
		}

		/// <summary>
		/// Render thumbnail
		/// </summary>
		/// <param name="target">Target mesh</param>
		/// <param name="cameraPos">Camera position</param>
		/// <param name="alpha">Alpha blend amount</param>
		/// <param name="displayMatrix">View + projection + display matrix</param>
		/// <param name="thumbnails">Thumbnail manager</param>
		public void RenderThumbnail( MeshGeometry3D target, Point3D cameraPos, double alpha, Matrix3D displayMatrix, ThumbnailManager thumbnails )
		{
			Point3D[] points = GetFlatPoints( cameraPos, displayMatrix );

			Rect texture = thumbnails.Get( url );

			AddRectangleIndices( target );

			target.Positions.Add( points[ 0 ] );
			target.TextureCoordinates.Add( new Point( texture.Left, texture.Top ) );
			target.Positions.Add( points[ 1 ] );
			target.TextureCoordinates.Add( new Point( texture.Right, texture.Top ) );
			target.Positions.Add( points[ 2 ] );
			target.TextureCoordinates.Add( new Point( texture.Right, texture.Bottom ) );
			target.Positions.Add( points[ 3 ] );
			target.TextureCoordinates.Add( new Point( texture.Left, texture.Bottom ) );
		}

		/// <summary>
		/// Add mesh to model
		/// </summary>
		public void Render( MeshGeometry3D target, Point3D cameraPos, double alpha )
		{
			for ( int posIndex = 0; posIndex < geometry.Positions.Count; posIndex += 4 )
			{
				// Just get first vertex normal (assume all the same)
				Plane plane = new Plane( positions[ posIndex ], geometry.Normals[ posIndex ] );

				if ( plane.Classify( cameraPos ) == Halfspace.Positive )
				{
					AddRectangleIndices( target );

					for ( int i = 0; i < 4; ++ i )
					{
						// Add vertex positions
						target.Positions.Add( positions[ posIndex + i ] );
						target.TextureCoordinates.Add( new System.Windows.Point( ( ( materialOffset - 0.5 ) * alpha ) + 0.5, 0 ) );
					}
				}
			}
		}

		/// <summary>
		/// Add square to model
		/// </summary>
		public void RenderFlat( MeshGeometry3D target, Point3D cameraPos, Matrix3D displayMatrix, Rect texture )
		{
			Point3D[] points = GetFlatPoints( cameraPos, displayMatrix );

			AddRectangleIndices( target );

			target.Positions.Add( points[ 0 ] );
			target.TextureCoordinates.Add( new Point( texture.Left, texture.Top ) );
			target.Positions.Add( points[ 1 ] );
			target.TextureCoordinates.Add( new Point( texture.Right, texture.Top ) );
			target.Positions.Add( points[ 2 ] );
			target.TextureCoordinates.Add( new Point( texture.Right, texture.Bottom ) );
			target.Positions.Add( points[ 3 ] );
			target.TextureCoordinates.Add( new Point( texture.Left, texture.Bottom ) );
		}

		/// <summary>
		/// Add rectangle indices
		/// </summary>
		/// <param name="target">Target geometry</param>
		private void AddRectangleIndices( MeshGeometry3D target )
		{
			int baseInd = target.Positions.Count;

			// Add triangle indices for cube face
			target.TriangleIndices.Add( baseInd );
			target.TriangleIndices.Add( baseInd + 3 );
			target.TriangleIndices.Add( baseInd + 2 );
			target.TriangleIndices.Add( baseInd + 2 );
			target.TriangleIndices.Add( baseInd + 1 );
			target.TriangleIndices.Add( baseInd );
		}
/*
        /// <summary>
        /// Calculate matrix for circle / sphere
        /// </summary>
        /// <param name="cameraPos">Camera position</param>
        /// <param name="displayMatrix">View, projection and display matrix</param>
        /// <returns>Sphere matrix</returns>
        public Matrix SphereMatrix( Point3D cameraPos, Matrix3D displayMatrix )
        {
			Vector3D perpendicular = Vector3D.CrossProduct( (Vector3D)position, (Vector3D)cameraPos );
			perpendicular.Normalize();

            Vector3D distance = bounds.Positions[ 0 ] - position;
            double radius = Math.Max( Math.Max( Math.Abs( distance.X ), Math.Abs( distance.Y ) ), Math.Abs( distance.Z ) ) * 1.1;

            Point3D[] points = new Point3D[] { position, position + ( perpendicular * radius ) };
            displayMatrix.Transform( points );

            double diameter = ( points[ 1 ] - points[ 0 ] ).Length * 2;

            Matrix sphereMatrix = new Matrix();
            sphereMatrix.Scale( diameter, diameter );
            sphereMatrix.Translate( points[ 0 ].X - ( diameter / 2 ), points[ 0 ].Y - ( diameter / 2 ) );

            return sphereMatrix;
        }*/
    }
}