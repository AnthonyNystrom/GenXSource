using System;
using System.Collections.Generic;
using System.Text;
using sgCoreWrapper.Structs;
using sgCoreWrapper.Objects;

namespace Demo
{
	public class Camera
	{
		private class BoundingPlane
		{
			public msgVectorStruct vec = new msgVectorStruct();
			public double fDepth;
		}
 
		public Camera()
		{
			x_axe = new msgVectorStruct();
			x_axe.x = 1.0;
			x_axe.y = 0.0;
			x_axe.z = 0.0;

			y_axe = new msgVectorStruct();
			y_axe.x = 0.0;
			y_axe.y = 1.0;
			y_axe.z = 0.0;

			z_axe = new msgVectorStruct();
			z_axe.x = 0.0;
			z_axe.y = 0.0;
			z_axe.z = 1.0;

			zero_p = new msgVectorStruct();
			zero_p.x = 0.0;
			zero_p.y = 0.0;
			zero_p.z = 0.0;

			m_fEyePos = new msgPointStruct();
			m_fLookAtPos = new msgPointStruct();
			m_fUpVector = new msgVectorStruct();
			m_iViewport = new int[4];

			ReInit();
		}

		public void ReInit()
		{
			SetLookAtPos(0.0f, 0.0f, 0.0f);
			SetEyePos(9.0f, 9.0f, 9.0f);
			SetFocalLength(9.0f, true);
			SetRotationAboutLookAt(-30.0f, 0.0f, -30.0f);
			SetUpVector(0.0f, 0.0f, 1.0f);	// Z-Axis is up
			m_fPitch = 0.0f;
			m_fYaw = 0.0f;
			m_fRoll = 0.0f;

			m_fFovY = 45.0f;
			m_fNear = 1.0f;
			m_fFar = 10000.0f;

			m_bPerspective = true;
			m_bBuildLists = true;
			m_iDisplayLists = 0;
			m_bResetClippingPlanes = true;

			// Clear our modelview and projection matrix
			m_dModelViewMatrix = new double[16];
			m_dProjectionMatrix = new double[16];
			m_fFrustum = new double[6][] { 
				new double[4], 
				new double[4], 
				new double[4], 
				new double[4], 
				new double[4], 
				new double[4] };
		}

		public void PositionCamera()
		{
			if (m_bResetClippingPlanes)
			{
				// Reset our cameras clipping plane
				ResetView(0, 0);
				m_bResetClippingPlanes = false;
			}

			if (m_fRoll != 0)
			{
				msgMatrix matx = new msgMatrix();
				msgVectorStruct UpVector;

				matx.Rotate(zero_p, x_axe, m_fPitch / 180.0 * Math.PI);
				matx.Rotate(zero_p, y_axe, m_fRoll / 180.0 * Math.PI);
				matx.Rotate(zero_p, z_axe, -m_fYaw / 180.0 * Math.PI);

				UpVector = z_axe;
				matx.ApplyMatrixToVector(zero_p, UpVector);

				// Position the camera using the newly calculated 'Up' vector
				OpenGLControl.gluLookAt(m_fEyePos.x, m_fEyePos.y, m_fEyePos.z,
							  m_fLookAtPos.x, m_fLookAtPos.y, m_fLookAtPos.z,
							  UpVector.x, UpVector.y, UpVector.z);
			}
			else
			{
				// Since our 'Up' vector has already been calculated, all we need to do is
				// position the camera..
				OpenGLControl.gluLookAt(m_fEyePos.x, m_fEyePos.y, m_fEyePos.z,
							  m_fLookAtPos.x, m_fLookAtPos.y, m_fLookAtPos.z,
							  m_fUpVector.x, m_fUpVector.y, m_fUpVector.z);
			}

			// Save the Model view matrix.  This is used later for
			// conversion of mouse coordinates to world coordinates.
			OpenGLControl.glGetDoublev(OpenGLControl.GL_MODELVIEW_MATRIX, m_dModelViewMatrix);
		}

		public void ResetView(int w, int h)
		{
			if (w != 0 || h != 0)
			{
				m_iScreenWidth = w;
				m_iScreenHeight = h;
			}

			// calculate the aspect ratio of the screen
			if (m_iScreenHeight == 0)
				m_fAspect = m_iScreenWidth;
			else
				m_fAspect = m_iScreenWidth / m_iScreenHeight;

			// Calculate the clipping volume along the y-axis, then set our
			// right, left, top and bottom clipping volumn
			double viewDepth = GetFocalLength();
			double clipY = Math.Tan(Utils.Radiansf(m_fFovY / 2)) * viewDepth;

			if (m_iScreenWidth <= m_iScreenHeight)
			{
				m_fLeft = -clipY;
				m_fRight = clipY;
				m_fBottom = -clipY * m_iScreenHeight / m_iScreenWidth;
				m_fTop = clipY * m_iScreenHeight / m_iScreenWidth;
			}
			else
			{
				m_fLeft = -clipY * m_iScreenWidth / m_iScreenHeight;
				m_fRight = clipY * m_iScreenWidth / m_iScreenHeight;
				m_fBottom = -clipY;
				m_fTop = clipY;
			}

			// Set Viewport to window dimensions
			OpenGLControl.glViewport(0, 0, m_iScreenWidth, m_iScreenHeight);

			// Reset the projection matrix (coordinate system)
			OpenGLControl.glMatrixMode(OpenGLControl.GL_PROJECTION);
			OpenGLControl.glLoadIdentity();

			if (m_bPerspective)
			{
				// Perspective transformations.
				OpenGLControl.gluPerspective(m_fFovY, m_fAspect, m_fNear, m_fFar);
			}
			else
			{
				// Orthographic transformations.
				OpenGLControl.glOrtho(m_fLeft, m_fRight, m_fBottom, m_fTop, m_fNear, m_fFar);
			}

			// Save the Projection matrix.  This is used later for
			// conversion of mouse coordinates to world coordinates.
			OpenGLControl.glGetDoublev(OpenGLControl.GL_PROJECTION_MATRIX, m_dProjectionMatrix);

			// Save the Projection matrix.  This is used later for
			// conversion of mouse coordinates to world coordinates.
			OpenGLControl.glGetIntegerv(OpenGLControl.GL_VIEWPORT, m_iViewport);

			// Reset the ModelView matrix
			OpenGLControl.glMatrixMode(OpenGLControl.GL_MODELVIEW);
			OpenGLControl.glLoadIdentity();
		}

		public void GetWorldCoord(int ix, int iy, double fz, ref msgVectorStruct coord)
		{
			double x, y, z, winX, winY, winZ;

			// Fix the yPos value.  MS Windows origin 0,0 is upper left
			// while OpenGL windows origin 0,0 is lower left...
			winX = ix;
			winY = m_iViewport[3] - iy;

			// Add the camera's focal length, or distance from 'LookAt' to 'Eye' position
			// to the given 'z' coordinate.
			fz += GetFocalLength();

			// Calculate the winZ coordinate:
			if (m_bPerspective)
				// Compensate for perspective view
				winZ = 0.5 + (((m_fFar + m_fNear) - (2 * m_fFar * m_fNear) / fz)) / (2 * (m_fFar - m_fNear));
			else
				// winZ is linearly interpolated between the Near_Far clipping plane
				winZ = (fz - m_fNear) / (m_fFar - m_fNear);

			// Unproject the point
			unsafe
			{
				fixed (double* m_dModelViewMatrixPtr = m_dModelViewMatrix)
				{
					fixed (double* m_dProjectionMatrixPtr = m_dProjectionMatrix)
					{
						fixed (int* m_iViewportPtr = m_iViewport)
						{
							OpenGLControl.gluUnProject(winX, winY, winZ,
								 m_dModelViewMatrixPtr,
								 m_dProjectionMatrixPtr,
								 m_iViewportPtr,
								 &x, &y, &z);
						}
					}
				}
			}

			coord.x = x;
			coord.y = y;
			coord.z = z;
		}

		public void GetScreenCoord(double wX, double wY, double wZ,
			ref double scrX, ref double scrY, ref double scrZ)
		{
			unsafe
			{
				fixed (double* m_dModelViewMatrixPtr = m_dModelViewMatrix)
				{
					fixed (double* m_dProjectionMatrixPtr = m_dProjectionMatrix)
					{
						fixed (int* m_iViewportPtr = m_iViewport)
						{
							double scrXVal = scrX;
							double scrYVal = scrY;
							double scrZVal = scrZ;
							OpenGLControl.gluProject(wX, wY, wZ,
								m_dModelViewMatrixPtr, m_dProjectionMatrixPtr, m_iViewportPtr,
								&scrXVal, &scrYVal, &scrZVal);
							scrX = scrXVal;
							scrY = scrYVal;
							scrZ = scrZVal;
						}
					}
				}
			}
			scrY = m_iViewport[3] - scrY;
			scrZ += GetFocalLength();
		}

		public void GetEyePos(ref double x, ref double y, ref double z)
		{
			x = m_fEyePos.x;
			y = m_fEyePos.y;
			z = m_fEyePos.z;
		}

		public void GetLookAtPos(ref double x, ref double y, ref double z)
		{
			x = m_fLookAtPos.x;
			y = m_fLookAtPos.y;
			z = m_fLookAtPos.z;
		}

		public void GetUpVector(ref double x, ref double y, ref double z)
		{
			x = m_fUpVector.x;
			y = m_fUpVector.y;
			z = m_fUpVector.z;
		}

		public double GetFocalLength()
		{
			return m_fFocalLength;
		}

		public void SetFocalLength(double length, bool bLookAtPos)
		{
			double rx = 0;
			double ry = 0;
			double rz = 0;

			if (bLookAtPos)
			{
				// Calculate the 'Eye' position by first getting the rotation about
				// the 'LookAt' position, set the new focal length, then set the 
				// rotation about the 'LookAt' position.
				GetRotationAboutLookAt(ref rx, ref ry, ref rz);

				// Set the member variable
				m_fFocalLength = length;

				SetRotationAboutLookAt(rx, ry, rz);
			}
			else
			{
				// Calculate the 'LookAt' position
				// Calculate the 'LookAt' position by first getting the rotation about
				// the 'Eye' position, set the new focal length, then set the rotation
				// about the 'Eye' position.
				GetRotationAboutEye(ref rx, ref ry, ref rz);

				// Set the member variable
				m_fFocalLength = length;

				SetRotationAboutEye(rx, ry, rz);
			}
		}

		public void GetRotationAboutLookAt(ref double x, ref double y, ref double z)
		{
			CalculateFocalLength();

			double ax, ay, az;
			ax = m_fEyePos.x - m_fLookAtPos.x;
			ay = m_fEyePos.y - m_fLookAtPos.y;
			az = m_fEyePos.z - m_fLookAtPos.z;

			// Calculate the Camera Pitch angle
			if (m_fUpVector.z >= 0)
				m_fPitch = Utils.Degreesf(Math.Asin(-az / m_fFocalLength));
			else
				m_fPitch = 180 - Utils.Degreesf(Math.Asin(-az / m_fFocalLength));


			// Calculate the Camera 'Yaw' angle
			if (m_fPitch == 90.0f)
				m_fYaw = Utils.Degreesf(Math.Atan2(-m_fUpVector.x, -m_fUpVector.y));
			else if (m_fPitch == -90.0f)
				m_fYaw = Utils.Degreesf(Math.Atan2(m_fUpVector.x, m_fUpVector.y));
			else if (m_fPitch < 90.0f)
				m_fYaw = Utils.Degreesf(Math.Atan2(-ax, -ay));
			else
				m_fYaw = Utils.Degreesf(Math.Atan2(ax, ay));

			// Pass the values back to the caller
			x = m_fPitch;
			y = m_fRoll;
			z = m_fYaw;
		}

		public void GetRotationAboutEye(ref double x, ref double y, ref double z)
		{
			CalculateFocalLength();

			double ax, ay, az;
			ax = m_fEyePos.x - m_fLookAtPos.x;
			ay = m_fEyePos.y - m_fLookAtPos.y;
			az = m_fEyePos.z - m_fLookAtPos.z;

			// Calculate the Camera Pitch angle
			if (m_fUpVector.z >= 0)
				m_fPitch = Utils.Degreesf(Math.Asin(-az / m_fFocalLength));
			else
				m_fPitch = 180 - Utils.Degreesf(Math.Asin(-az / m_fFocalLength));


			// Calculate the Camera 'Yaw' angle
			if (m_fPitch == 90.0f)
				m_fYaw = Utils.Degreesf(Math.Atan2(-m_fUpVector.x, -m_fUpVector.y));
			else if (m_fPitch == -90.0f)
				m_fYaw = Utils.Degreesf(Math.Atan2(m_fUpVector.x, m_fUpVector.y));
			else if (m_fPitch < 90.0f)
				m_fYaw = Utils.Degreesf(Math.Atan2(-ax, -ay));
			else
				m_fYaw = Utils.Degreesf(Math.Atan2(ax, ay));

			// Pass the values back to the caller
			x = m_fPitch;
			y = m_fRoll;
			z = m_fYaw;
		}

		public void SetEyePos(double x, double y, double z)
		{
			m_fEyePos.x = x;
			m_fEyePos.y = y;
			m_fEyePos.z = z;

			CalculateYawPitchRoll();
			CalculateUpVector();
		}

		public void SetLookAtPos(double x, double y, double z)
		{
		}

		public void SetUpVector(double x, double y, double z)
		{
			m_fUpVector.x = x;
			m_fUpVector.y = y;
			m_fUpVector.z = z;
		}

		public void SetRotationAboutLookAt(double x, double y, double z)
		{
			msgMatrix matx = new msgMatrix();
			msgVectorStruct EyePos = new msgVectorStruct();

			matx.Rotate(zero_p, x_axe, x / 180.0 * Math.PI);
			matx.Rotate(zero_p, z_axe, -z / 180.0 * Math.PI);

			EyePos.x = 0.0;
			EyePos.y = -m_fFocalLength;
			EyePos.z = 0.0;
			matx.ApplyMatrixToVector(zero_p, EyePos);

			m_fEyePos.x = EyePos.x + m_fLookAtPos.x;
			m_fEyePos.y = EyePos.y + m_fLookAtPos.y;
			m_fEyePos.z = EyePos.z + m_fLookAtPos.z;

			// Calculate our camera's UpVector using ONLY the 'X' (Pitch) and 'Z' (Yaw)
			// parameters
			m_fUpVector = z_axe;
			matx.ApplyMatrixToVector(zero_p, m_fUpVector);

			// Just save the 'y' value for later use when calculating the camera's 'Up' vector
			// prior to calling gluLookAt()
			m_fRoll = y;
		}

		public void SetRotationAboutEye(double x, double y, double z)
		{
			msgMatrix matx = new msgMatrix();
			msgVectorStruct LookAtPos = new msgVectorStruct();

			matx.Rotate(zero_p, x_axe, x / 180.0 * Math.PI);
			matx.Rotate(zero_p, z_axe, -z / 180.0 * Math.PI);

			LookAtPos.x = 0.0;
			LookAtPos.y = m_fFocalLength;
			LookAtPos.z = 0.0;
			matx.ApplyMatrixToVector(zero_p, LookAtPos);

			m_fLookAtPos.x = m_fEyePos.x + LookAtPos.x;
			m_fLookAtPos.y = m_fEyePos.y + LookAtPos.y;
			m_fLookAtPos.z = m_fEyePos.z + LookAtPos.z;

			// Calculate our camera's UpVector using ONLY the 'X' (Pitch) and 'Z' (Yaw)
			// parameters
			m_fUpVector = z_axe;
			matx.ApplyMatrixToVector(zero_p, m_fUpVector);

			// Just save the 'y' value for later use when calculating the camera's 'Up' vector
			// prior to calling gluLookAt()
			m_fRoll = y;
		}

		public void SetFarClipPlane(double fFar)
		{
			m_fFar = fFar;

			// Reset the projection matrix (coordinate system)
			OpenGLControl.glMatrixMode(OpenGLControl.GL_PROJECTION);
			OpenGLControl.glLoadIdentity();

			if (m_bPerspective)
				// Perspective transformations.
				OpenGLControl.gluPerspective(m_fFovY, m_fAspect, m_fNear, m_fFar);
			else
				// Orthographic transformations.
				OpenGLControl.glOrtho(m_fLeft, m_fRight, m_fBottom, m_fTop, m_fNear, m_fFar);

			// Save the Projection matrix.  This is used later for
			// conversion of mouse coordinates to world coordinates.
			OpenGLControl.glGetDoublev(OpenGLControl.GL_PROJECTION_MATRIX, m_dProjectionMatrix);

			// Reset the ModelView matrix
			OpenGLControl.glMatrixMode(OpenGLControl.GL_MODELVIEW);
		}

		public void SetNearClipPlane(double fNear)
		{
			m_fNear = fNear;

			// Reset the projection matrix (coordinate system)
			OpenGLControl.glMatrixMode(OpenGLControl.GL_PROJECTION);
			OpenGLControl.glLoadIdentity();

			if (m_bPerspective)
				// Perspective transformations.
				OpenGLControl.gluPerspective(m_fFovY, m_fAspect, m_fNear, m_fFar);

			else
				// Orthographic transformations.
				OpenGLControl.glOrtho(m_fLeft, m_fRight, m_fBottom, m_fTop, m_fNear, m_fFar);

			// Save the Projection matrix.  This is used later for
			// conversion of mouse coordinates to world coordinates.
			OpenGLControl.glGetDoublev(OpenGLControl.GL_PROJECTION_MATRIX, m_dProjectionMatrix);

			// Reset the ModelView matrix
			OpenGLControl.glMatrixMode(OpenGLControl.GL_MODELVIEW);
		}

		public void CalculateFocalLength()
		{
			// Calculate a new focal length based on the position of the
			// 'LookAt' and 'Eye' position, where:
			//    length = sqrt(a*a + b*b +c*c);

			double a, b, c, temp;
			a = m_fEyePos.x - m_fLookAtPos.x;
			b = m_fEyePos.y - m_fLookAtPos.y;
			c = m_fEyePos.z - m_fLookAtPos.z;

			temp = Math.Sqrt(a * a + b * b + c * c);
			if (temp == 0.0f)
				m_fFocalLength = 1.0f;
			else
				m_fFocalLength = temp;
		}

		public void CalculateYawPitchRoll()
		{
			CalculateFocalLength();

			double ax, ay, az;
			ax = m_fEyePos.x - m_fLookAtPos.x;
			ay = m_fEyePos.y - m_fLookAtPos.y;
			az = m_fEyePos.z - m_fLookAtPos.z;

			// Calculate the Camera Pitch angle
			if (m_fUpVector.z >= 0)
				m_fPitch = Utils.Degreesf(Math.Asin(-az / m_fFocalLength));
			else
				m_fPitch = 180 - Utils.Degreesf(Math.Asin(-az / m_fFocalLength));

			// Calculate the Camera 'Yaw' angle
			if (m_fPitch == 90.0f)
				m_fYaw = Utils.Degreesf(Math.Atan2(-m_fUpVector.x, -m_fUpVector.y));
			else if (m_fPitch == -90.0f)
				m_fYaw = Utils.Degreesf(Math.Atan2(m_fUpVector.x, m_fUpVector.y));
			else if (m_fPitch < 90.0f)
				m_fYaw = Utils.Degreesf(Math.Atan2(-ax, -ay));
			else
				m_fYaw = Utils.Degreesf(Math.Atan2(ax, ay));
		}

		public void CalculateUpVector()
		{
			msgMatrix matx = new msgMatrix();

			matx.Rotate(zero_p, x_axe, m_fPitch / 180.0 * Math.PI);
			matx.Rotate(zero_p, z_axe, -m_fYaw / 180.0 * Math.PI);

			m_fUpVector = z_axe;
			matx.ApplyMatrixToVector(zero_p, m_fUpVector);
		}

		public void FitBounds(double minX, double minY, double minZ,
				double maxX, double maxY, double maxZ)
		{
			BoundingPlane boundsRight = new BoundingPlane();
			BoundingPlane boundsLeft = new BoundingPlane();
			BoundingPlane boundsTop = new BoundingPlane();
			BoundingPlane boundsBottom = new BoundingPlane();
			BoundingPlane boundsNear = new BoundingPlane();
			BoundingPlane boundsFar = new BoundingPlane();
			msgVectorStruct boundsMax = new msgVectorStruct();
			msgVectorStruct boundsMin = new msgVectorStruct();
			msgVectorStruct vecCenter = new msgVectorStruct();
			msgVectorStruct[] vertices = new msgVectorStruct[8];
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] = new msgVectorStruct();
			}

			msgVectorStruct vecOffset = new msgVectorStruct();
			msgMatrix matrix = new msgMatrix();
			double focalLength = 0;
			double fDepthWidth = 0;
			double fDepthHeight = 0;
			double rx = 0;
			double ry = 0;
			double rz = 0;
			double fTan = 0;
			double fx = 0;
			double fz = 0;

			fTan = (double)Math.Tan((float)Utils.Radiansf(m_fFovY/2));


			// Get the cameras rotatiom about the LookAt position, as we 
			// will use this to restore the rotation values after we move
			// the camera and it's focal length.
			GetRotationAboutLookAt(ref rx, ref ry, ref rz);

			// Copy the bounds to our local variable
			boundsMin.x = minX;	boundsMin.y = minY;	boundsMin.z = minZ;
			boundsMax.x = maxX;	boundsMax.y = maxY;	boundsMax.z = maxZ;

			double spanX, spanY, spanZ;

			spanX = Utils.Diff(boundsMax.x, boundsMin.x);
			spanY = Utils.Diff(boundsMax.y, boundsMin.y);
			spanZ = Utils.Diff(boundsMax.z, boundsMin.z);

			vecCenter.x = boundsMax.x - Math.Abs(spanX)/2;
			vecCenter.y = boundsMax.y - Math.Abs(spanY)/2;
			vecCenter.z = boundsMax.z - Math.Abs(spanZ)/2;

			boundsMax.x = spanX/2;
			boundsMax.y = spanY/2;
			boundsMax.z = spanZ/2;

			boundsMin.x = -spanX/2;
			boundsMin.y = -spanY/2;
			boundsMin.z = -spanZ/2;

			// Given the bounding box, fill in the missing vertices to complete our
			// cube
			vertices[0] = boundsMax;	// Left
			vertices[1].x = boundsMax.x; vertices[1].y = boundsMax.y; vertices[1].z = boundsMin.x;
			vertices[2].x = boundsMax.x; vertices[2].y = boundsMin.y; vertices[2].z = boundsMin.x;
			vertices[3].x = boundsMax.x; vertices[3].y = boundsMin.y; vertices[3].z = boundsMax.x;
		
			vertices[4] = boundsMin;
			vertices[5].x = boundsMin.x; vertices[5].y = boundsMin.y; vertices[5].z = boundsMax.x;
			vertices[6].x = boundsMin.x; vertices[6].y = boundsMax.y; vertices[6].z = boundsMax.x;
			vertices[7].x = boundsMin.x; vertices[7].y = boundsMax.y; vertices[7].z = boundsMin.x;

			// Get the cameras rotation matrix
			GetRotationMatrix(ref matrix);

			for(int i=0; i<8; i++)
			{
				// Transform the vertice by the camera rotation matrix.  Since we define the 
				// default 'Up' camera position as Z-axis Up, the coordinates map as follows:
				//		X maps to Width,
				//		Y maps to Depth
				//		Z mpas to Height
				zero_p.x  = zero_p.y  =zero_p.z  =0.0;
				matrix.ApplyMatrixToVector(zero_p, vertices[i]);

				// Calculate the focal length needed to fit the near bounding plane
				fDepthWidth  = (Math.Abs(vertices[i].x)/fTan/m_fAspect)-vertices[i].y;
				fDepthHeight = (Math.Abs(vertices[i].z) / fTan) - vertices[i].y;


				// Calculate the Near clipping bounds.  This will be used to fit Isometric views and
				// for calculating the Near/Far clipping m_fFrustum.
				if(vertices[i].y<0)
				{
					if (Math.Abs(vertices[i].x) > Math.Abs(boundsNear.vec.x) ||
					   (Math.Abs(vertices[i].x) == boundsNear.vec.x && Math.Abs(vertices[i].y) > Math.Abs(boundsNear.vec.z)))
					{
						boundsNear.vec.x = Math.Abs(vertices[i].x);
						boundsNear.vec.z = Math.Abs(vertices[i].y);
					}

					if (Math.Abs(vertices[i].z) > Math.Abs(boundsNear.vec.y) ||
					   (Math.Abs(vertices[i].z) == boundsNear.vec.y))
					{
						boundsNear.vec.y = Math.Abs(vertices[i].z);
						//boundsNear.vec[W] = fabs(vertices[i].y);
					}

					// Get the bounding depth closest to the viewer
					if(fDepthWidth < boundsNear.fDepth || boundsNear.fDepth == 0)
						boundsNear.fDepth = fDepthWidth;
					if(fDepthHeight < boundsNear.fDepth || boundsNear.fDepth == 0)
						boundsNear.fDepth = fDepthHeight;
				}
				else
				{
					if( Math.Abs(vertices[i].x) > Math.Abs(boundsFar.vec.x) ||
					   (Math.Abs(vertices[i].x) == boundsFar.vec.x && Math.Abs(vertices[i].y) < Math.Abs(boundsFar.vec.z)) )
					{
						boundsFar.vec.x = vertices[i].x;
						boundsFar.vec.z = vertices[i].y;
					}

					if( Math.Abs(vertices[i].z) > Math.Abs(boundsFar.vec.y) ||
					   (Math.Abs(vertices[i].z) == Math.Abs(boundsFar.vec.y)) )
					{
						boundsFar.vec.y = vertices[i].z;
						//boundsFar.vec[W] = vertices[i].y;
					}

					// Get the bounding depth furtherest from the viewer
					if(fDepthWidth > boundsFar.fDepth)
						boundsFar.fDepth = fDepthWidth;
					if(fDepthHeight > boundsFar.fDepth)
						boundsFar.fDepth = fDepthHeight;
				}


				// Calculate the Right, Left, Top and Bottom clipping bounds.  This will be used to fit
				// Perspective views.
				if(vertices[i].x > 0)
				{
					if(fDepthWidth > boundsRight.fDepth)
					{
						boundsRight.fDepth = fDepthWidth;
						boundsRight.vec.x = vertices[i].x;
						//boundsRight.vec[W] = vertices[i].y;
					}
				}
				if(vertices[i].x <= 0)
				{
					if(fDepthWidth > boundsLeft.fDepth)
					{
						boundsLeft.fDepth = fDepthWidth;
						boundsLeft.vec.x = vertices[i].x;
						//boundsLeft.vec[W] = vertices[i].y;
					}
				}
				if(vertices[i].z > 0)
				{
					if(fDepthHeight > boundsTop.fDepth)
					{
						boundsTop.fDepth = fDepthHeight;
						boundsTop.vec.x = vertices[i].x;
						//boundsTop.vec[W] = vertices[i].y;
					}
				}
				if(vertices[i].z <= 0)
				{
					if(fDepthHeight > boundsBottom.fDepth)
					{
						boundsBottom.fDepth = fDepthHeight;
						boundsBottom.vec.x = vertices[i].x;
						//boundsBottom.vec[W] = vertices[i].y;
					}
				}
			}

			// Now that we have the view clipping bounds, we can calculate the focal depth
			// required to fit the volumn and the offset necessary to center the volumn.
			if (m_bPerspective)
			{
				msgMatrix invMatrix = new msgMatrix();

				if (boundsRight.fDepth == boundsLeft.fDepth &&
				   boundsTop.fDepth == boundsBottom.fDepth)
				{
					// Front, Side or Top view

					//  Since the bounds are symetric, just use the Right and Top focal depth.
					fx = boundsRight.fDepth;
					fz = boundsTop.fDepth;

					// No offset necessary
					vecOffset.x = vecOffset.y = vecOffset.z = 0.0;
				}
				else
				{
					// Calculate the average focal length needed to fit the bounding box
					fx = (boundsRight.fDepth + boundsLeft.fDepth) / 2;
					fz = (boundsTop.fDepth + boundsBottom.fDepth) / 2;

					// Calculate the offset necessary to center the bounding box.  Note that we
					// use a scaling factor for centering the non-limiting bounds to achieve a
					// more visually appealing center.
					if (fx > fz)
					{
						double fScale = Math.Sqrt(boundsTop.fDepth / boundsBottom.fDepth);
						double fTop = fTan * fx - fTan * boundsTop.fDepth;
						double fBottom = fTan * fx - fTan * boundsBottom.fDepth;

						vecOffset.x = (fTan * m_fAspect * boundsRight.fDepth - fTan * m_fAspect * fx);
						vecOffset.z = (fBottom - fTop * fScale) / 2;
					}
					else
					{
						double fScale = Math.Sqrt(boundsLeft.fDepth / boundsRight.fDepth);
						double fRight = fTan * m_fAspect * fz - fTan * m_fAspect * boundsRight.fDepth;
						double fLeft = fTan * m_fAspect * fz - fTan * m_fAspect * boundsLeft.fDepth;

						vecOffset.z = (fTan * boundsTop.fDepth - fTan * fz);
						vecOffset.x = (fLeft - fRight * fScale) / 2;
					}
				}

				// Now that we have the offsets necessary to center the bounds, we must rotate
				// the vertices (camera coordinates) by the cameras inverse rotation matrix to
				// convert the offsets to world coordinates.
				GetInvRotationMatrix(ref invMatrix);
				zero_p.x = zero_p.y = zero_p.z = 0.0;

				invMatrix.ApplyMatrixToVector(zero_p, vecOffset);
			}
			else
			{
				// Isometric View
				// Calculate the focal length needed to fit the near bounding plane
				if (m_iScreenWidth <= m_iScreenHeight)
				{
					fx = boundsNear.vec.x / Math.Tan((float)Utils.Radiansf(m_fFovY / 2));
					fz = boundsNear.vec.y / Math.Tan((float)Utils.Radiansf(m_fFovY / 2)) / ((double)m_iScreenHeight / (double)m_iScreenWidth);
				}
				else
				{
					fx = boundsNear.vec.x / Math.Tan((float)Utils.Radiansf(m_fFovY / 2)) / m_fAspect;
					fz = boundsNear.vec.y / Math.Tan((float)Utils.Radiansf(m_fFovY / 2));
				}
			}

			// Set the focal length equal to the largest length required to fit either the 
			// Width (Horizontal) or Height (Vertical)
			focalLength = (fx > fz ? fx : fz);

			// Set the camera's new LookAt position to focus on the center
			// of the bounding box.
			SetLookAtPos(vecCenter.x + vecOffset.x, vecCenter.y + vecOffset.y, vecCenter.z + vecOffset.z);

			// Set the camera focal Length
			if (focalLength > m_fNear)
				SetFocalLength(focalLength, true);

			// Adjust the Near clipping plane if necessary
			//	if((boundsNear.fDepth/2) > 0.5f)
			//		m_fNear = boundsNear.fDepth/2;

			// Adjust the Far clipping plane if necessary
			if (focalLength + boundsFar.fDepth > m_fFar)
				m_fFar = focalLength + boundsFar.fDepth;

			// Recalculate the camera view m_fFrustum;
			ResetView(0, 0);

			// Restore the cameras rotation about the LookAt position
			SetRotationAboutLookAt(rx, ry, rz);
		}

		public void ExtractFrustum()
		{
			float[]	 proj = new float[16];	// For Grabbing The PROJECTION Matrix
			float[]	 modl = new float[16];	// For Grabbing The MODELVIEW Matrix
			double[] clip = new double[16];	// Result Of Concatenating PROJECTION and MODELVIEW
			double	t;			// Temporary Work Variable

			OpenGLControl.glGetFloatv(OpenGLControl.GL_PROJECTION_MATRIX, proj);			// Grab The Current PROJECTION Matrix
			OpenGLControl.glGetFloatv(OpenGLControl.GL_MODELVIEW_MATRIX, modl);			// Grab The Current MODELVIEW Matrix

			// Concatenate (Multiply) The Two Matricies
			clip[ 0] = modl[ 0] * proj[ 0] + modl[ 1] * proj[ 4] + modl[ 2] * proj[ 8] + modl[ 3] * proj[12];
			clip[ 1] = modl[ 0] * proj[ 1] + modl[ 1] * proj[ 5] + modl[ 2] * proj[ 9] + modl[ 3] * proj[13];
			clip[ 2] = modl[ 0] * proj[ 2] + modl[ 1] * proj[ 6] + modl[ 2] * proj[10] + modl[ 3] * proj[14];
			clip[ 3] = modl[ 0] * proj[ 3] + modl[ 1] * proj[ 7] + modl[ 2] * proj[11] + modl[ 3] * proj[15];

			clip[ 4] = modl[ 4] * proj[ 0] + modl[ 5] * proj[ 4] + modl[ 6] * proj[ 8] + modl[ 7] * proj[12];
			clip[ 5] = modl[ 4] * proj[ 1] + modl[ 5] * proj[ 5] + modl[ 6] * proj[ 9] + modl[ 7] * proj[13];
			clip[ 6] = modl[ 4] * proj[ 2] + modl[ 5] * proj[ 6] + modl[ 6] * proj[10] + modl[ 7] * proj[14];
			clip[ 7] = modl[ 4] * proj[ 3] + modl[ 5] * proj[ 7] + modl[ 6] * proj[11] + modl[ 7] * proj[15];

			clip[ 8] = modl[ 8] * proj[ 0] + modl[ 9] * proj[ 4] + modl[10] * proj[ 8] + modl[11] * proj[12];
			clip[ 9] = modl[ 8] * proj[ 1] + modl[ 9] * proj[ 5] + modl[10] * proj[ 9] + modl[11] * proj[13];
			clip[10] = modl[ 8] * proj[ 2] + modl[ 9] * proj[ 6] + modl[10] * proj[10] + modl[11] * proj[14];
			clip[11] = modl[ 8] * proj[ 3] + modl[ 9] * proj[ 7] + modl[10] * proj[11] + modl[11] * proj[15];

			clip[12] = modl[12] * proj[ 0] + modl[13] * proj[ 4] + modl[14] * proj[ 8] + modl[15] * proj[12];
			clip[13] = modl[12] * proj[ 1] + modl[13] * proj[ 5] + modl[14] * proj[ 9] + modl[15] * proj[13];
			clip[14] = modl[12] * proj[ 2] + modl[13] * proj[ 6] + modl[14] * proj[10] + modl[15] * proj[14];
			clip[15] = modl[12] * proj[ 3] + modl[13] * proj[ 7] + modl[14] * proj[11] + modl[15] * proj[15];


			// Extract the RIGHT clipping plane
			m_fFrustum[0][0] = clip[ 3] - clip[ 0];
			m_fFrustum[0][1] = clip[ 7] - clip[ 4];
			m_fFrustum[0][2] = clip[11] - clip[ 8];
			m_fFrustum[0][3] = clip[15] - clip[12];

			// Normalize it
			t = (double) Math.Sqrt( m_fFrustum[0][0] * m_fFrustum[0][0] + m_fFrustum[0][1] * m_fFrustum[0][1] + m_fFrustum[0][2] * m_fFrustum[0][2] );
			m_fFrustum[0][0] /= t;
			m_fFrustum[0][1] /= t;
			m_fFrustum[0][2] /= t;
			m_fFrustum[0][3] /= t;

			// Extract the LEFT clipping plane
			m_fFrustum[1][0] = clip[ 3] + clip[ 0];
			m_fFrustum[1][1] = clip[ 7] + clip[ 4];
			m_fFrustum[1][2] = clip[11] + clip[ 8];
			m_fFrustum[1][3] = clip[15] + clip[12];

			// Normalize it
			t = (double)Math.Sqrt(m_fFrustum[1][0] * m_fFrustum[1][0] + m_fFrustum[1][1] * m_fFrustum[1][1] + m_fFrustum[1][2] * m_fFrustum[1][2]);
			m_fFrustum[1][0] /= t;
			m_fFrustum[1][1] /= t;
			m_fFrustum[1][2] /= t;
			m_fFrustum[1][3] /= t;


			// Extract the BOTTOM clipping plane
			m_fFrustum[2][0] = clip[ 3] + clip[ 1];
			m_fFrustum[2][1] = clip[ 7] + clip[ 5];
			m_fFrustum[2][2] = clip[11] + clip[ 9];
			m_fFrustum[2][3] = clip[15] + clip[13];

			// Normalize it
			t = (double)Math.Sqrt(m_fFrustum[2][0] * m_fFrustum[2][0] + m_fFrustum[2][1] * m_fFrustum[2][1] + m_fFrustum[2][2] * m_fFrustum[2][2]);
			m_fFrustum[2][0] /= t;
			m_fFrustum[2][1] /= t;
			m_fFrustum[2][2] /= t;
			m_fFrustum[2][3] /= t;


			// Extract the TOP clipping plane
			m_fFrustum[3][0] = clip[ 3] - clip[ 1];
			m_fFrustum[3][1] = clip[ 7] - clip[ 5];
			m_fFrustum[3][2] = clip[11] - clip[ 9];
			m_fFrustum[3][3] = clip[15] - clip[13];

			// Normalize it
			t = Math.Sqrt(m_fFrustum[3][0] * m_fFrustum[3][0] + m_fFrustum[3][1] * m_fFrustum[3][1] + m_fFrustum[3][2] * m_fFrustum[3][2]);
			m_fFrustum[3][0] /= t;
			m_fFrustum[3][1] /= t;
			m_fFrustum[3][2] /= t;
			m_fFrustum[3][3] /= t;


			// Extract the FAR clipping plane
			m_fFrustum[4][0] = clip[ 3] - clip[ 2];
			m_fFrustum[4][1] = clip[ 7] - clip[ 6];
			m_fFrustum[4][2] = clip[11] - clip[10];
			m_fFrustum[4][3] = clip[15] - clip[14];

			// Normalize it
			t = Math.Sqrt(m_fFrustum[4][0] * m_fFrustum[4][0] + m_fFrustum[4][1] * m_fFrustum[4][1] + m_fFrustum[4][2] * m_fFrustum[4][2]);
			m_fFrustum[4][0] /= t;
			m_fFrustum[4][1] /= t;
			m_fFrustum[4][2] /= t;
			m_fFrustum[4][3] /= t;


			// Extract the NEAR clipping plane. 
			m_fFrustum[5][0] = clip[ 3] + clip[ 2];
			m_fFrustum[5][1] = clip[ 7] + clip[ 6];
			m_fFrustum[5][2] = clip[11] + clip[10];
			m_fFrustum[5][3] = clip[15] + clip[14];

			// Normalize it
			t = (double)Math.Sqrt(m_fFrustum[5][0] * m_fFrustum[5][0] + m_fFrustum[5][1] * m_fFrustum[5][1] + m_fFrustum[5][2] * m_fFrustum[5][2]);
			m_fFrustum[5][0] /= t;
			m_fFrustum[5][1] /= t;
			m_fFrustum[5][2] /= t;
			m_fFrustum[5][3] /= t;
		}

		public void GetRotationMatrix(ref msgMatrix XformMatrix)
		{
			PrepareMatrix(0.0f, 0.0f, 0.0f,				// Origin
				  -m_fPitch, m_fRoll, m_fYaw,	// Rotation
				  0.0f, 0.0f, 0.0f,				// Translation
				  ref XformMatrix);
		}

		public void GetInvRotationMatrix(ref msgMatrix XformMatrix)
		{
			PrepareInvMatrix(0.0f, 0.0f, 0.0f,				// Origin
					 -m_fPitch, m_fRoll, m_fYaw,	// Rotation
					 0.0f, 0.0f, 0.0f,				// Translation
					 ref XformMatrix);
		}

		public void GetVectorsForRayTracingCamera(msgVectorStruct eye_loc,
				msgVectorStruct eye_look_at,
				msgVectorStruct eye_y,
				msgVectorStruct eye_x)
		{
			double xval = eye_loc.x;
			double yval = eye_loc.y;
			double zval = eye_loc.z;
			GetEyePos(ref xval, ref yval, ref zval);
			eye_loc.x = xval;
			eye_loc.y = yval;
			eye_loc.z = zval;

			double xval2 = eye_look_at.x;
			double yval2 = eye_look_at.y;
			double zval2 = eye_look_at.z;
			GetLookAtPos(ref xval2, ref yval2, ref zval2);
			eye_look_at.x = xval2;
			eye_look_at.y = yval2;
			eye_look_at.z = zval2;
			
			msgVectorStruct p1 = new msgVectorStruct();
			msgVectorStruct p2 = new msgVectorStruct();
			GetWorldCoord(m_iScreenWidth/2,m_iScreenHeight/2,0.0,ref p1);
			GetWorldCoord(m_iScreenWidth/2,0,0.0, ref p2);

			eye_y = msgSpaceMath.VectorsSub(p2,p1);

			msgSpaceMath.NormalVector(eye_y);
			
			GetWorldCoord(m_iScreenWidth, m_iScreenHeight/2,0.0, ref p2);

			eye_x = msgSpaceMath.VectorsSub(p2,p1);
			msgSpaceMath.NormalVector(eye_x);
		}

		#region Data

		bool m_bBuildLists;
		bool m_bResetClippingPlanes;
		int	m_iDisplayLists;
		bool m_bPerspective;
		int m_iScreenWidth;
		int m_iScreenHeight;

		double m_fFovY;		// Y-Axis field of view
		double m_fAspect;		// width(x) to height(y) aspect
		double m_fLeft;
		double m_fRight;
		double m_fBottom;
		double m_fTop;
		double m_fNear;
		double m_fFar;
		int[] m_iViewport;
		
		msgPointStruct m_fEyePos;
		msgPointStruct m_fLookAtPos;
		msgVectorStruct m_fUpVector;
		double m_fFocalLength;
		double m_fPitch;				// Rotation about the X-Axis
		double m_fRoll;				// Rotation about the Y-Axis
		double m_fYaw;					// Rotation about the Z-Axis
		double[] m_dModelViewMatrix;
		double[] m_dProjectionMatrix;
		double[][] m_fFrustum;

		msgVectorStruct x_axe;
		msgVectorStruct y_axe;
		msgVectorStruct z_axe;
		msgVectorStruct zero_p;

		#endregion

		private void PrepareInvMatrix(double Ox, double Oy, double Oz,
							  double Rx, double Ry, double Rz,
							  double Tx, double Ty, double Tz,
							  ref msgMatrix XForm)
		{
			XForm.Identity();

			msgVectorStruct oV = new msgVectorStruct();
			oV.x = Ox;
			oV.y = Oy;
			oV.z = Oz;
			XForm.Translate(oV);

			XForm.Rotate(zero_p, z_axe, Rz);
			XForm.Rotate(zero_p, y_axe, Ry);
			XForm.Rotate(zero_p, x_axe, Rx);

			msgVectorStruct TransV = new msgVectorStruct();
			TransV.x = Tx;
			TransV.y = Ty;
			TransV.z = Tz;
			XForm.Translate(TransV);
		}

		private void PrepareMatrix(double Ox, double Oy, double Oz,
				   double Rx, double Ry, double Rz,
				   double Tx, double Ty, double Tz,
				   ref msgMatrix XForm)
		{
			XForm.Identity();

			msgVectorStruct TransV = new msgVectorStruct();
			TransV.x = Tx;
			TransV.y = Ty;
			TransV.z = Tz;
			XForm.Translate(TransV);

			XForm.Rotate(zero_p, x_axe, Rx);
			XForm.Rotate(zero_p, y_axe, Ry);
			XForm.Rotate(zero_p, z_axe, Rz);

			msgVectorStruct oV = new msgVectorStruct();
			oV.x = Ox;
			oV.y = Oy;
			oV.z = Oz;
			XForm.Translate(oV);
		}
	}
}
