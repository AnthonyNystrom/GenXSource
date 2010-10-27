using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Next2Friends.Swoosher.Media3D
{
	internal static class M3DUtil
	{
		// Methods
/*		private static void AddPointToBounds( ref Point3D point, ref Rect3D bounds )
		{
			if ( point.X < bounds.X )
			{
				bounds.SizeX += bounds.X - point.X;
				bounds.X = point.X;
			}
			else if ( point.X > ( bounds.X + bounds.SizeX ) )
			{
				bounds.SizeX = point.X - bounds.X;
			}
			if ( point.Y < bounds.Y )
			{
				bounds.SizeY += bounds.Y - point.Y;
				bounds.Y = point.Y;
			}
			else if ( point.Y > ( bounds.Y + bounds.SizeY ) )
			{
				bounds.SizeY = point.Y - bounds.Y;
			}
			if ( point.Z < bounds.Z )
			{
				bounds.SizeZ += bounds.Z - point.Z;
				bounds.Z = point.Z;
			}
			else if ( point.Z > ( bounds.Z + bounds.SizeZ ) )
			{
				bounds.SizeZ = point.Z - bounds.Z;
			}
		}

		internal static Rect3D ComputeAxisAlignedBoundingBox( Point3DCollection positions )
		{
			if ( positions != null )
			{
				FrugalStructList<Point3D> list = positions._collection;
				if ( list.Count != 0 )
				{
					Point3D point = list[ 0 ];
					Rect3D bounds = new Rect3D( point.X, point.Y, point.Z, 0, 0, 0 );
					for ( int i = 1; i < list.Count; i++ )
					{
						point = list[ i ];
						AddPointToBounds( ref point, ref bounds );
					}
					return bounds;
				}
			}
			return Rect3D.Empty;
		}

		internal static Rect3D ComputeTransformedAxisAlignedBoundingBox( ref Rect3D originalBox, Transform3D transform )
		{
			if ( ( transform == null ) || ( transform == Transform3D.Identity ) )
			{
				return originalBox;
			}
			Matrix3D matrix = transform.Value;
			return ComputeTransformedAxisAlignedBoundingBox( ref originalBox, ref matrix );
		}

		internal static Rect3D ComputeTransformedAxisAlignedBoundingBox( ref Rect3D originalBox, ref Matrix3D matrix )
		{
			if ( originalBox.IsEmpty )
			{
				return originalBox;
			}
			if ( matrix.IsAffine )
			{
				return ComputeTransformedAxisAlignedBoundingBoxAffine( ref originalBox, ref matrix );
			}
			return ComputeTransformedAxisAlignedBoundingBoxNonAffine( ref originalBox, ref matrix );
		}

		internal static Rect3D ComputeTransformedAxisAlignedBoundingBoxAffine( ref Rect3D originalBox, ref Matrix3D matrix )
		{
			double num15 = originalBox.X + originalBox.SizeX;
			double num14 = originalBox.Y + originalBox.SizeY;
			double num13 = originalBox.Z + originalBox.SizeZ;
			double offsetX = matrix.OffsetX;
			double num6 = matrix.OffsetX;
			double num12 = matrix.M11 * originalBox.X;
			double num11 = matrix.M11 * num15;
			if ( num11 > num12 )
			{
				offsetX += num12;
				num6 += num11;
			}
			else
			{
				offsetX += num11;
				num6 += num12;
			}
			num12 = matrix.M21 * originalBox.Y;
			num11 = matrix.M21 * num14;
			if ( num11 > num12 )
			{
				offsetX += num12;
				num6 += num11;
			}
			else
			{
				offsetX += num11;
				num6 += num12;
			}
			num12 = matrix.M31 * originalBox.Z;
			num11 = matrix.M31 * num13;
			if ( num11 > num12 )
			{
				offsetX += num12;
				num6 += num11;
			}
			else
			{
				offsetX += num11;
				num6 += num12;
			}
			double offsetY = matrix.OffsetY;
			double num5 = matrix.OffsetY;
			double num10 = matrix.M12 * originalBox.X;
			double num9 = matrix.M12 * num15;
			if ( num9 > num10 )
			{
				offsetY += num10;
				num5 += num9;
			}
			else
			{
				offsetY += num9;
				num5 += num10;
			}
			num10 = matrix.M22 * originalBox.Y;
			num9 = matrix.M22 * num14;
			if ( num9 > num10 )
			{
				offsetY += num10;
				num5 += num9;
			}
			else
			{
				offsetY += num9;
				num5 += num10;
			}
			num10 = matrix.M32 * originalBox.Z;
			num9 = matrix.M32 * num13;
			if ( num9 > num10 )
			{
				offsetY += num10;
				num5 += num9;
			}
			else
			{
				offsetY += num9;
				num5 += num10;
			}
			double offsetZ = matrix.OffsetZ;
			double num4 = matrix.OffsetZ;
			double num8 = matrix.M13 * originalBox.X;
			double num7 = matrix.M13 * num15;
			if ( num7 > num8 )
			{
				offsetZ += num8;
				num4 += num7;
			}
			else
			{
				offsetZ += num7;
				num4 += num8;
			}
			num8 = matrix.M23 * originalBox.Y;
			num7 = matrix.M23 * num14;
			if ( num7 > num8 )
			{
				offsetZ += num8;
				num4 += num7;
			}
			else
			{
				offsetZ += num7;
				num4 += num8;
			}
			num8 = matrix.M33 * originalBox.Z;
			num7 = matrix.M33 * num13;
			if ( num7 > num8 )
			{
				offsetZ += num8;
				num4 += num7;
			}
			else
			{
				offsetZ += num7;
				num4 += num8;
			}
			return new Rect3D( offsetX, offsetY, offsetZ, num6 - offsetX, num5 - offsetY, num4 - offsetZ );
		}

		internal static Rect3D ComputeTransformedAxisAlignedBoundingBoxNonAffine( ref Rect3D originalBox, ref Matrix3D matrix )
		{
			double x = originalBox.X;
			double y = originalBox.Y;
			double z = originalBox.Z;
			double num4 = originalBox.X + originalBox.SizeX;
			double num3 = originalBox.Y + originalBox.SizeY;
			double num2 = originalBox.Z + originalBox.SizeZ;
			Point3D[] points = new Point3D[] { new Point3D( x, y, z ), new Point3D( x, y, num2 ), new Point3D( x, num3, z ), new Point3D( x, num3, num2 ), new Point3D( num4, y, z ), new Point3D( num4, y, num2 ), new Point3D( num4, num3, z ), new Point3D( num4, num3, num2 ) };
			matrix.Transform( points );
			Point3D point = points[ 0 ];
			Rect3D bounds = new Rect3D( point.X, point.Y, point.Z, 0, 0, 0 );
			for ( int i = 1; i < points.Length; i++ )
			{
				point = points[ i ];
				AddPointToBounds( ref point, ref bounds );
			}
			return bounds;
		}

		internal static double DegreesToRadians( double degrees )
		{
			return ( degrees * 0.017453292519943295 );
		}

		internal static double GetAspectRatio( Size viewSize )
		{
			return ( viewSize.Width / viewSize.Height );
		}

		internal static Matrix GetHomogeneousToViewportTransform( Rect viewport )
		{
			double num2 = viewport.Width / 2;
			double num = viewport.Height / 2;
			double offsetX = viewport.X + num2;
			return new Matrix( num2, 0, 0, -num, offsetX, viewport.Y + num );
		}

		internal static Matrix3D GetHomogeneousToViewportTransform3D( Rect viewport )
		{
			double num2 = viewport.Width / 2;
			double num = viewport.Height / 2;
			double offsetX = viewport.X + num2;
			return new Matrix3D( num2, 0, 0, 0, 0, -num, 0, 0, 0, 0, 1, 0, offsetX, viewport.Y + num, 0, 1 );
		}

		internal static Point GetNormalizedPoint( Point point, Size size )
		{
			return new Point( ( ( 2 * point.X ) / size.Width ) - 1, -( ( ( 2 * point.Y ) / size.Height ) - 1 ) );
		}

		internal static Matrix3D GetWorldToViewportTransform3D( Camera camera, Rect viewport )
		{
			return ( ( camera.GetViewMatrix() * camera.GetProjectionMatrix( GetAspectRatio( viewport.Size ) ) ) * GetHomogeneousToViewportTransform3D( viewport ) );
		}

		internal static Matrix3D GetWorldTransformationMatrix( Visual3D visual )
		{
			Viewport3DVisual visual2;
			return GetWorldTransformationMatrix( visual, out visual2 );
		}

		internal static Matrix3D GetWorldTransformationMatrix( Visual3D visual3DStart, out Viewport3DVisual viewport )
		{
			DependencyObject reference = visual3DStart;
			Matrix3D identity = Matrix3D.Identity;
			while ( reference != null )
			{
				Visual3D visuald = reference as Visual3D;
				if ( visuald == null )
				{
					break;
				}
				Transform3D transformd = (Transform3D)visuald.GetValue( Visual3D.TransformProperty );
				if ( transformd != null )
				{
					transformd.Append( ref identity );
				}
				reference = VisualTreeHelper.GetParent( reference );
			}
			if ( reference != null )
			{
				viewport = (Viewport3DVisual)reference;
				return identity;
			}
			viewport = null;
			return identity;
		}

		internal static Point3D Interpolate( ref Point3D v0, ref Point3D v1, ref Point3D v2, ref Point barycentric )
		{
			double x = barycentric.X;
			double y = barycentric.Y;
			double num3 = ( 1 - x ) - y;
			return new Point3D( ( ( num3 * v0.X ) + ( x * v1.X ) ) + ( y * v2.X ), ( ( num3 * v0.Y ) + ( x * v1.Y ) ) + ( y * v2.Y ), ( ( num3 * v0.Z ) + ( x * v1.Z ) ) + ( y * v2.Z ) );
		}

		internal static bool IsPointInTriangle( Point p, Point[] triUVVertices, Point3D[] tri3DVertices, out Point3D inters3DPoint )
		{
			double num = 0;
			inters3DPoint = new Point3D();
			double num7 = triUVVertices[ 0 ].X - triUVVertices[ 2 ].X;
			double num6 = triUVVertices[ 1 ].X - triUVVertices[ 2 ].X;
			double num9 = triUVVertices[ 2 ].X - p.X;
			double num5 = triUVVertices[ 0 ].Y - triUVVertices[ 2 ].Y;
			double num4 = triUVVertices[ 1 ].Y - triUVVertices[ 2 ].Y;
			double num8 = triUVVertices[ 2 ].Y - p.Y;
			num = ( num7 * num4 ) - ( num6 * num5 );
			if ( num == 0 )
			{
				return false;
			}
			double num3 = ( ( num6 * num8 ) - ( num9 * num4 ) ) / num;
			num = ( num6 * num5 ) - ( num7 * num4 );
			if ( num == 0 )
			{
				return false;
			}
			double num2 = ( ( num7 * num8 ) - ( num9 * num5 ) ) / num;
			if ( ( ( num3 < 0 ) || ( num3 > 1 ) ) || ( ( ( num2 < 0 ) || ( num2 > 1 ) ) || ( ( num3 + num2 ) > 1 ) ) )
			{
				return false;
			}
			inters3DPoint = (Point3D)( ( ( num3 * ( (Vector3D)tri3DVertices[ 0 ] ) ) + ( num2 * ( (Vector3D)tri3DVertices[ 1 ] ) ) ) + ( ( ( 1 - num3 ) - num2 ) * ( (Vector3D)tri3DVertices[ 2 ] ) ) );
			return true;
		}
*/
		internal static double RadiansToDegrees( double radians )
		{
			return ( radians * 57.295779513082323 );
		}
/*
		internal static bool TryTransformToViewport3DVisual( Visual3D visual3D, out Viewport3DVisual viewport, out Matrix3D matrix )
		{
			matrix = GetWorldTransformationMatrix( visual3D, out viewport );
			if ( viewport != null )
			{
				matrix *= GetWorldToViewportTransform3D( viewport.Camera, viewport.Viewport );
				return true;
			}
			return false;
		}*/
	}
}