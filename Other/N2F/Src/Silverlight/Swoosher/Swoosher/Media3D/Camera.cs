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
	public class Camera
	{
		private bool dirtyView = true;
		private bool dirtyProjection = true;
		private bool dirtyRotation = true;
		private bool dirtyFrustum = true;

		private Point3D position;
		private Vector3D lookDirection;
		private Vector3D upDirection = new Vector3D( 0, 1, 0 );
		private Quaternion rotation;
		private Frustum frustum;

		private double aspectRatio;
		private double fov;
		private double near;
		private double far;

		private Matrix3D viewMatrix;
		private Matrix3D projectionMatrix;
		private Matrix3D rotationMatrix;

		public Camera()
		{
		}

		public Point3D Position { get { return position; } set { position = value; dirtyView = true; dirtyFrustum = true; } }
		public Vector3D LookDirection { get { return lookDirection; } set { lookDirection = value; dirtyView = true; dirtyFrustum = true; } }
		public Vector3D UpDirection { get { return upDirection; } set { upDirection = value; dirtyView = true; dirtyFrustum = true; } }
		public Quaternion Rotation { get { return rotation; } set { rotation = value; dirtyView = true; dirtyRotation = true; dirtyFrustum = true; } }

		public double AspectRatio { get { return aspectRatio; } set { aspectRatio = value; dirtyProjection = true; dirtyFrustum = true; } }
		public double Fov { get { return fov; } set { fov = value; dirtyProjection = true; dirtyFrustum = true; } }
		public double Near { get { return near; } set { near = value; dirtyProjection = true; dirtyFrustum = true; } }
		public double Far { get { return far; } set { far = value; dirtyProjection = true; dirtyFrustum = true; } }

		public Matrix3D GetViewMatrix()
		{
			if ( dirtyView )
			{
				dirtyView = false;

				var rot = GetRotationMatrix();

				viewMatrix = CreateViewMatrix(
					rot.Transform( position ),
					rot.Transform( lookDirection ),
					upDirection );
			}

			return viewMatrix;
		}

		public Matrix3D GetProjectionMatrix()
		{
			if ( dirtyProjection )
			{
				dirtyProjection = false;
				projectionMatrix = CreateProjectionMatrix( aspectRatio, fov, near, far );
			}

			return projectionMatrix;
		}

		public Matrix3D GetRotationMatrix()
		{
			if ( dirtyRotation )
			{
				dirtyRotation = false;
				rotationMatrix = new Matrix3D();
				rotationMatrix.Rotate( rotation );
			}

			return rotationMatrix;
		}

		public Frustum GetFrustum()
		{
			if ( dirtyFrustum )
			{
				dirtyFrustum = false;
				frustum = new Frustum( GetViewMatrix() * GetProjectionMatrix(), false );
			}

			return frustum;
		}

		/// <summary>
		/// Get display matrix
		/// </summary>
		/// <param name="width">Viewport width</param>
		/// <param name="height">Viewport height</param>
		/// <returns>Display matrix</returns>
		public Matrix3D CreateDisplayMatrix( double width, double height )
		{
			return CreateDisplayMatrix( GetViewMatrix(), GetProjectionMatrix(), width, height );
		}

		/// <summary>
		/// Create a View matrix from an existing ProjectionCamera
		/// (code acquired with Reflector)
		/// </summary>
		/// <param name="position">Camera position</param>
		/// <param name="lookDirection">Look direction</param>
		/// <param name="upDirection">Up direction</param>
		/// <returns>View matrix</returns>
		public static Matrix3D CreateViewMatrix( Point3D position, Vector3D lookDirection, Vector3D upDirection )
		{
			Vector3D vectord2 = -lookDirection;
			vectord2.Normalize();
			Vector3D vectord3 = Vector3D.CrossProduct( upDirection, vectord2 );
			vectord3.Normalize();
			Vector3D vectord4 = Vector3D.CrossProduct( vectord2, vectord3 );
			Vector3D vectord = (Vector3D)position;
			double offsetX = -Vector3D.DotProduct( vectord3, vectord );
			double offsetY = -Vector3D.DotProduct( vectord4, vectord );
			double offsetZ = -Vector3D.DotProduct( vectord2, vectord );

			return new Matrix3D(
					   vectord3.X, vectord4.X, vectord2.X, 0,
					   vectord3.Y, vectord4.Y, vectord2.Y, 0,
					   vectord3.Z, vectord4.Z, vectord2.Z, 0,
					   offsetX, offsetY, offsetZ, 1 );
		}

		/// <summary>
		/// Create a Projection matrix from an existing ProjectionCamera and view size
		/// (code acquired with Reflector)
		/// </summary>
		/// <param name="near">Near plane distance</param>
		/// <param name="far">Far plane distance</param>
		/// <param name="fov">Field of view</param>
		/// <param name="width">Width</param>
		/// <param name="height">Height</param>
		public static Matrix3D CreateProjectionMatrix( double aspectRatio, double fov, double near, double far )
		{
			double zn = near;
			double zf = far;

			double num2 = Math.Tan( DegreesToRadians( fov ) / 2 );
			double num5 = aspectRatio / num2;
			double num4 = 1 / num2;
			double num = ( zf != double.PositiveInfinity ) ? ( zf / ( zn - zf ) ) : -1;
			return new Matrix3D( num4, 0, 0, 0, 0, num5, 0, 0, 0, 0, num, -1, 0, 0, zn * num, 0 );
		}

		/// <summary>
		/// Get display matrix
		/// </summary>
		/// <param name="view">View matrix</param>
		/// <param name="projection">Projection matrix</param>
		/// <param name="width">Viewport width</param>
		/// <param name="height">Viewport height</param>
		/// <returns>Display matrix</returns>
		public static Matrix3D CreateDisplayMatrix( Matrix3D view, Matrix3D projection, double width, double height )
		{
//			return ( view * projection ) * GetHomogeneousToViewportTransform3D( width, height );
			return view * projection * GetHomogeneousToViewportTransform3D( width, height );
		}

		#region Internal methods

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