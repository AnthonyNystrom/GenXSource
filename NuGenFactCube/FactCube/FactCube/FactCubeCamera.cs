using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Genetibase.FactCube
{
	public class FactCubeCamera
	{
		/// <summary>
		/// Create a MatrixCamera from an existing ProjectionCamera
		/// </summary>
		/// <param name="camera">Camera</param>
		/// <param name="width">Viewport height</param>
		/// <param name="height">Viewport width</param>
		/// <returns>MatrixCamera</returns>
		public static MatrixCamera CreateMatrixCamera( ProjectionCamera camera, double width, double height )
		{
			MatrixCamera matrixCamera = new MatrixCamera(
				CreateViewMatrix( camera ),
				CreateProjectionMatrix( camera, width, height, camera.NearPlaneDistance, camera.FarPlaneDistance ) );
			matrixCamera.Transform = camera.Transform;

			return matrixCamera;
		}

		/// <summary>
		/// Create a MatrixCamera from an existing ProjectionCamera
		/// </summary>
		/// <param name="camera">Camera</param>
		/// <param name="width">Viewport height</param>
		/// <param name="height">Viewport width</param>
		/// <param name="near">Near plane distance</param>
		/// <param name="far">Far plane distance</param>
		/// <returns>MatrixCamera</returns>
		public static MatrixCamera CreateMatrixCamera( ProjectionCamera camera, double width, double height, double near, double far )
		{
			MatrixCamera matrixCamera = new MatrixCamera(
				CreateViewMatrix( camera ),
				CreateProjectionMatrix( camera, width, height, near, far ) );
			matrixCamera.Transform = camera.Transform;

			return matrixCamera;
		}

		/// <summary>
		/// Create a View matrix from an existing ProjectionCamera
		/// (code acquired with Reflector)
		/// </summary>
		/// <param name="camera">Camera</param>
		/// <returns>View matrix</returns>
		public static Matrix3D CreateViewMatrix( ProjectionCamera camera )
		{
			Vector3D vectord2 = -camera.LookDirection;
			vectord2.Normalize();
			Vector3D vectord3 = Vector3D.CrossProduct( camera.UpDirection, vectord2 );
			vectord3.Normalize();
			Vector3D vectord4 = Vector3D.CrossProduct( vectord2, vectord3 );
			Vector3D vectord = (Vector3D)camera.Position;
			double offsetX = -Vector3D.DotProduct( vectord3, vectord );
			double offsetY = -Vector3D.DotProduct( vectord4, vectord );
			double offsetZ = -Vector3D.DotProduct( vectord2, vectord );
			Matrix3D viewMatrix = new Matrix3D( vectord3.X, vectord4.X, vectord2.X, 0, vectord3.Y, vectord4.Y, vectord2.Y, 0, vectord3.Z, vectord4.Z, vectord2.Z, 0, offsetX, offsetY, offsetZ, 1 );
			PrependInverseTransform( camera.Transform, ref viewMatrix );
			return viewMatrix;
		}

		/// <summary>
		/// Create a Projection matrix from an existing ProjectionCamera and view size
		/// (code acquired with Reflector)
		/// </summary>
		/// <param name="camera">Camera</param>
		/// <param name="width">Width</param>
		/// <param name="height">Height</param>
		public static Matrix3D CreateProjectionMatrix( ProjectionCamera camera, double width, double height, double near, double far )
		{
			double aspectRatio = width / height;
			double zn = near;
			double zf = far;

			double fov = ( camera is PerspectiveCamera ) ? ( camera as PerspectiveCamera ).FieldOfView : 60;

			double num2 = Math.Tan( DegreesToRadians( fov ) / 2 );
			double num5 = aspectRatio / num2;
			double num4 = 1 / num2;
			double num = ( zf != double.PositiveInfinity ) ? ( zf / ( zn - zf ) ) : -1;
			return new Matrix3D( num4, 0, 0, 0, 0, num5, 0, 0, 0, 0, num, -1, 0, 0, zn * num, 0 );
		}

		/// <summary>
		/// Get display matrix
		/// </summary>
		/// <param name="camera">Camera</param>
		/// <param name="width">Viewport width</param>
		/// <param name="height">Viewport height</param>
		/// <returns>Display matrix</returns>
		public static Matrix3D GetDisplayMatrix( MatrixCamera camera, double width, double height )
		{
			return ( camera.ViewMatrix * camera.ProjectionMatrix ) * GetHomogeneousToViewportTransform3D( width, height );
		}

		#region Internal methods

		internal static void PrependInverseTransform( Transform3D transform, ref Matrix3D viewMatrix )
		{
			if ( ( transform != null ) && ( transform != Transform3D.Identity ) )
			{
				PrependInverseTransform( transform.Value, ref viewMatrix );
			}
		}

		internal static void PrependInverseTransform( Matrix3D matrix, ref Matrix3D viewMatrix )
		{
			matrix.Invert();
			viewMatrix.Prepend( matrix );
		}

		internal static double DegreesToRadians( double degrees )
		{
			return ( degrees * 0.017453292519943295 );
		}

		public static Matrix3D GetHomogeneousToViewportTransform3D( double width, double height )
		{
			double halfWidth = width / 2;
			double halfHeight = height / 2;
			return new Matrix3D( halfWidth, 0, 0, 0, 0, -halfHeight, 0, 0, 0, 0, 1, 0, halfWidth, halfHeight, 0, 1 );
		}

		#endregion
	}
}