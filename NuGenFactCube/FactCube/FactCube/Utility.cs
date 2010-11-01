using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

namespace Genetibase.FactCube
{
	public class Utility
	{
		public struct SortableModel : IComparable<SortableModel>
		{
			public double Distance;
			public GeometryModel3D Model;

			/// <summary>
			/// Construct triangle
			/// </summary>
			/// <param name="model">Model</param>
			public SortableModel( double distance, GeometryModel3D model )
			{
				Distance = distance;
				Model = model;
			}

			#region IComparable<double> Members

			/// <summary>
			/// Compare distance
			/// </summary>
			/// <param name="other">Other distance</param>
			/// <returns>-1, 0 or 1</returns>
			public int CompareTo( SortableModel other )
			{
				return ( Distance < other.Distance ) ? -1 : ( Distance > other.Distance ) ? 1 : 0;
			}

			#endregion
		}

		/// <summary>
		/// Sort models
		/// </summary>
		/// <param name="models">Models</param>
		/// <param name="transform">Camera transform</param>
		/// <returns>Sorted models</returns>
		public static IEnumerable<SortableModel> Sort( IList<GeometryModel3D> models, Matrix3D transform, int maximum )
		{
			List<SortableModel> sorted = new List<SortableModel>();
			Point3D center = new Point3D();

			foreach ( GeometryModel3D model in models )
			{
				sorted.Add( new SortableModel( transform.Transform( model.Transform.Transform( center ) ).Z, model ) );
			}

			sorted.Sort();

			double minDistance = 0.98;
			double maxDistance = 10;

			List<SortableModel> visible = new List<SortableModel>();

			foreach ( SortableModel model in sorted )
			{
//				if ( model.Distance > minDistance && model.Distance < maxDistance )
				{
					visible.Add( model );
					if ( visible.Count >= maximum ) break;
				}
			}

			for ( int index = visible.Count - 1; index >= 0; -- index )
			{
				yield return visible[ index ];
			}
		}

		/// <summary>
		/// Get distance between vectors, squared
		/// </summary>
		/// <param name="value1">Vector 1</param>
		/// <param name="value2">Vector 2</param>
		/// <returns>Distance</returns>
		public static double DistanceSquared( Point3D value1, Point3D value2 )
		{
			double num3 = value1.X - value2.X;
			double num2 = value1.Y - value2.Y;
			double num = value1.Z - value2.Z;
			return ( ( num3 * num3 ) + ( num2 * num2 ) ) + ( num * num );
		}
/*
		/// <summary>
		/// Determine point of line/plane intersection
		/// </summary>
		/// <param name="pPlane">Plane position</param>
		/// <param name="vPlane">Plane normal</param>
		/// <param name="pLine">Line position</param>
		/// <param name="vLine">Line normal</param>
		/// <returns></returns>
		public static double LinePlaneIntersect( Vector3D pPlane, Vector3D vPlane, Vector3D pLine, Vector3D vLine, out Vector3D intersection )
		{
			intersection = pLine + ( Vector3D.DotProduct( pPlane - pLine, vPlane ) / Vector3D.DotProduct( vLine, vPlane ) ) * vLine;

			return Distance( pLine, intersection );
		}*/
	}
}