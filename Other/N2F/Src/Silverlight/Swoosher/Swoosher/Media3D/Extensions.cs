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
	public static class Extensions
	{
		public static Quaternion CreateQuaternion( this Matrix3D matrix )
		{
			double num8 = ( matrix.M11 + matrix.M22 ) + matrix.M33;
			Quaternion quaternion = new Quaternion();
			if ( num8 > 0f )
			{
				float num = (float)Math.Sqrt( (double)( num8 + 1f ) );
				quaternion.W = num * 0.5f;
				num = 0.5f / num;
				quaternion.X = ( matrix.M23 - matrix.M32 ) * num;
				quaternion.Y = ( matrix.M31 - matrix.M13 ) * num;
				quaternion.Z = ( matrix.M12 - matrix.M21 ) * num;
				return quaternion;
			}
			if ( ( matrix.M11 >= matrix.M22 ) && ( matrix.M11 >= matrix.M33 ) )
			{
				float num7 = (float)Math.Sqrt( (double)( ( ( 1f + matrix.M11 ) - matrix.M22 ) - matrix.M33 ) );
				float num4 = 0.5f / num7;
				quaternion.X = 0.5f * num7;
				quaternion.Y = ( matrix.M12 + matrix.M21 ) * num4;
				quaternion.Z = ( matrix.M13 + matrix.M31 ) * num4;
				quaternion.W = ( matrix.M23 - matrix.M32 ) * num4;
				return quaternion;
			}
			if ( matrix.M22 > matrix.M33 )
			{
				float num6 = (float)Math.Sqrt( (double)( ( ( 1f + matrix.M22 ) - matrix.M11 ) - matrix.M33 ) );
				float num3 = 0.5f / num6;
				quaternion.X = ( matrix.M21 + matrix.M12 ) * num3;
				quaternion.Y = 0.5f * num6;
				quaternion.Z = ( matrix.M32 + matrix.M23 ) * num3;
				quaternion.W = ( matrix.M31 - matrix.M13 ) * num3;
				return quaternion;
			}
			float num5 = (float)Math.Sqrt( (double)( ( ( 1f + matrix.M33 ) - matrix.M11 ) - matrix.M22 ) );
			float num2 = 0.5f / num5;
			quaternion.X = ( matrix.M31 + matrix.M13 ) * num2;
			quaternion.Y = ( matrix.M32 + matrix.M23 ) * num2;
			quaternion.Z = 0.5f * num5;
			quaternion.W = ( matrix.M12 - matrix.M21 ) * num2;
			return quaternion;
		}

		public static Matrix3D LookAt( this Point3D position, Point3D target, Vector3D up )
		{
			var vector = (Vector3D)position - (Vector3D)target;
			vector.Normalize();
			var vector2 = Vector3D.CrossProduct( up, vector );
			vector2.Normalize();
			var vector3 = Vector3D.CrossProduct( vector, vector2 );

			var matrix = new Matrix3D();
			matrix.M11 = vector2.X;
			matrix.M12 = vector3.X;
			matrix.M13 = vector.X;
			matrix.M14 = 0f;
			matrix.M21 = vector2.Y;
			matrix.M22 = vector3.Y;
			matrix.M23 = vector.Y;
			matrix.M24 = 0f;
			matrix.M31 = vector2.Z;
			matrix.M32 = vector3.Z;
			matrix.M33 = vector.Z;
			matrix.M34 = 0f;
			matrix.OffsetX = -Vector3D.DotProduct( vector2, (Vector3D)position );
			matrix.OffsetY = -Vector3D.DotProduct( vector3, (Vector3D)position );
			matrix.OffsetZ = -Vector3D.DotProduct( vector, (Vector3D)position );
			matrix.M44 = 1f;
			return matrix;
		}
	}
}