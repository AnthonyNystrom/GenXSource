
#include "stdafx.h"
#include "3DCamera.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

static  SG_VECTOR   x_axe = {1.0, 0.0, 0.0};
static  SG_VECTOR   y_axe = {0.0, 1.0, 0.0};
static  SG_VECTOR   z_axe = {0.0, 0.0, 1.0};
static  SG_POINT    zero_p = {0.0, 0.0, 0.0};

#define Pi        3.14159265
#define PiOver180   1.74532925199433E-002
#define PiUnder180    5.72957795130823E+001
#define SMALL_NUMBER  0.00001
#define LARGE_NUMBER  1E20


static GLdouble Radiansf(GLdouble Angle)
{
	GLdouble r = (GLdouble)Angle*PiOver180; 
	return r;
}

GLdouble Degreesf(GLdouble Angle)
{
	GLdouble d = (GLdouble)Angle*PiUnder180;
	return d;
}

GLdouble Diff(GLdouble a, GLdouble b)
{
	if(a >= 0 && b >= 0)
	{
		if(a > b)
			return a-b;
		else
			return b-a;
	}
	if(a < 0 && b < 0)
	{
		if( a > b)
			return b-a;
		else
			return a-b;
	}
	if(a >= 0 && b < 0)
		return a-b;
	else
		return b-a;
}


void PrepareMatrix(double Ox,  double Oy,  double Oz,
				   double Rx,  double Ry,  double Rz,
				   double Tx,  double Ty,  double Tz,
				   sgCMatrix& XForm)
{
	XForm.Identity(); 

	SG_VECTOR TransV = {Tx, Ty, Tz};
	XForm.Translate(TransV);

	XForm.Rotate(zero_p, x_axe, Rx);
	XForm.Rotate(zero_p, y_axe, Ry);
	XForm.Rotate(zero_p, z_axe, Rz);

	SG_VECTOR oV = {Ox, Oy, Oz};
	XForm.Translate(oV);
}

void PrepareInvMatrix(double Ox,  double Oy,  double Oz,
					  double Rx,  double Ry,  double Rz,
					  double Tx,  double Ty,  double Tz,
					  sgCMatrix& XForm)
{
	XForm.Identity(); 

	SG_VECTOR oV = {Ox, Oy, Oz};
	XForm.Translate(oV);

	XForm.Rotate(zero_p, z_axe, Rz);
	XForm.Rotate(zero_p, y_axe, Ry);
	XForm.Rotate(zero_p, x_axe, Rx);

	SG_VECTOR TransV = {Tx, Ty, Tz};
	XForm.Translate(TransV);
}



/////////////////////////////////////////////////////////////////////////////
// C3dCamera construction
C3dCamera::C3dCamera()
{
	ReInit();
}

C3dCamera& C3dCamera::operator = (const C3dCamera& oldCam) 
{
	m_bBuildLists				= oldCam.m_bBuildLists;
	m_bResetClippingPlanes		= oldCam.m_bResetClippingPlanes;
	m_iDisplayLists				= oldCam.m_iDisplayLists;
	m_bPerspective				= oldCam.m_bPerspective;
	m_enumCameraPosition		= oldCam.m_enumCameraPosition;
	m_iScreenWidth				= oldCam.m_iScreenWidth;
	m_iScreenHeight				= oldCam.m_iScreenHeight;

	m_fFovY						= oldCam.m_fFovY;		// Y-Axis field of view
	m_fAspect					= oldCam.m_fAspect;		// width(x) to height(y) aspect
	m_fLeft						= oldCam.m_fLeft;
	m_fRight					= oldCam.m_fRight;
	m_fBottom					= oldCam.m_fBottom;
	m_fTop						= oldCam.m_fTop;
	m_fNear						= oldCam.m_fNear;
	m_fFar						= oldCam.m_fFar;

	memcpy(&m_iViewport[0],&(oldCam.m_iViewport[0]),4*sizeof(GLint));
	memcpy(&m_fEyePos,&(oldCam.m_fEyePos),sizeof(SG_POINT));
	memcpy(&m_fLookAtPos,&(oldCam.m_fLookAtPos),sizeof(SG_POINT));
	memcpy(&m_fUpVector,&(oldCam.m_fUpVector),sizeof(SG_VECTOR));
	
	m_fFocalLength				= oldCam.m_fFocalLength;
	m_fPitch					= oldCam.m_fPitch;				// Rotation about the X-Axis
	m_fRoll						= oldCam.m_fRoll;				// Rotation about the Y-Axis
	m_fYaw						= oldCam.m_fYaw;				// Rotation about the Z-Axis

	memcpy(&m_dModelViewMatrix[0],&(oldCam.m_dModelViewMatrix[0]),16*sizeof(GLdouble));
	memcpy(&m_dProjectionMatrix[0],&(oldCam.m_dProjectionMatrix[0]),16*sizeof(GLdouble));
	
	memcpy(&m_fFrustum[0][0],&(oldCam.m_fFrustum[0][0]),6*4*sizeof(GLdouble));

	m_cur_animation_position_index = -1;
	m_timer_ID = NULL;
	return *this;
}

void C3dCamera::ReInit()
{
	SetLookAtPos(0.0f, 0.0f, 0.0f);
	SetEyePos(9.0f, 9.0f, 9.0f);
	SetFocalLength(9.0f);
	SetRotationAboutLookAt(-30.0f, 0.0f, -30.0f);
	SetUpVector(0.0f, 0.0f, 1.0f);	// Z-Axis is up
	m_fPitch = 0.0f;
	m_fYaw   = 0.0f;
	m_fRoll  = 0.0f;

	m_fFovY = 45.0f;
	m_fNear = 1.0f;
	m_fFar  = 10000.0f;

	m_bPerspective	= TRUE;
	m_enumCameraPosition = CP_USER_POSITION;
	m_bBuildLists	= TRUE;
	m_iDisplayLists = 0;
	m_bResetClippingPlanes	= TRUE;

	m_cur_animation_position_index = -1;
	m_timer_ID = NULL;

	// Clear our modelview and projection matrix
	memset(&m_dModelViewMatrix, 0, sizeof(GLdouble)*16);
	memset(&m_dProjectionMatrix, 0, sizeof(GLdouble)*16);
}

/////////////////////////////////////////////////////////////////////////////
// C3dCamera Destructor
C3dCamera::~C3dCamera()
{
	// Delete the DisplayLists
	if(m_iDisplayLists)
	{
		glDeleteLists(m_iDisplayLists, 1);
		m_iDisplayLists = 0;
	}

}

void C3dCamera::PositionCamera()
{
	if(m_bResetClippingPlanes)
	{
		// Reset our cameras clipping plane
		ResetView();
		m_bResetClippingPlanes = FALSE;
	}

	if(m_fRoll)
	{
		sgCMatrix matx;
		SG_VECTOR UpVector;

		matx.Rotate(zero_p, x_axe, m_fPitch/180.0*Pi);
		matx.Rotate(zero_p, y_axe, m_fRoll/180.0*Pi);
		matx.Rotate(zero_p, z_axe, -m_fYaw/180.0*Pi);

		UpVector = z_axe;
		matx.ApplyMatrixToVector(zero_p,UpVector);

		// Position the camera using the newly calculated 'Up' vector
		gluLookAt(m_fEyePos.x,    m_fEyePos.y,    m_fEyePos.z,
			m_fLookAtPos.x, m_fLookAtPos.y, m_fLookAtPos.z,
			UpVector.x,     UpVector.y,     UpVector.z);
	}
	else
	{
		// Since our 'Up' vector has already been calculated, all we need to do is
		// position the camera..
		gluLookAt(m_fEyePos.x,    m_fEyePos.y,    m_fEyePos.z,
			m_fLookAtPos.x, m_fLookAtPos.y, m_fLookAtPos.z,
			m_fUpVector.x,  m_fUpVector.y,  m_fUpVector.z);
	}

	// Save the Model view matrix.  This is used later for
	// conversion of mouse coordinates to world coordinates.
	glGetDoublev(GL_MODELVIEW_MATRIX, m_dModelViewMatrix);
}


void C3dCamera::ResetView(int w, int h)
{
	// Save the screen width and height
	if(w || h) {
		m_iScreenWidth  = (GLsizei)w;
		m_iScreenHeight = (GLsizei)h;
	}

	// calculate the aspect ratio of the screen
	if (m_iScreenHeight==0)
		m_fAspect = (GLdouble)m_iScreenWidth;
	else
		m_fAspect = (GLdouble)m_iScreenWidth/(GLdouble)m_iScreenHeight;

	// Calculate the clipping volume along the y-axis, then set our
	// right, left, top and bottom clipping volumn
	GLdouble viewDepth = (GLdouble)GetFocalLength();
	GLdouble clipY = (GLdouble)tan(Radiansf(m_fFovY/2))*viewDepth;

	if(m_iScreenWidth <= m_iScreenHeight) {
		m_fLeft   = -clipY;
		m_fRight  =  clipY;
		m_fBottom = -clipY*m_iScreenHeight/m_iScreenWidth;
		m_fTop    =  clipY*m_iScreenHeight/m_iScreenWidth;
	}
	else {
		m_fLeft   = -clipY*m_iScreenWidth/m_iScreenHeight;
		m_fRight  =  clipY*m_iScreenWidth/m_iScreenHeight;
		m_fBottom = -clipY;
		m_fTop    =  clipY;
	}

	// Set Viewport to window dimensions
	glViewport(0, 0, m_iScreenWidth, m_iScreenHeight);

	// Reset the projection matrix (coordinate system)
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();

	if(m_bPerspective) {
		// Perspective transformations.
		gluPerspective(m_fFovY, m_fAspect, m_fNear, m_fFar);
	}
	else {
		// Orthographic transformations.
		glOrtho(m_fLeft, m_fRight, m_fBottom, m_fTop, m_fNear, m_fFar); 
	}

	// Save the Projection matrix.  This is used later for
	// conversion of mouse coordinates to world coordinates.
	glGetDoublev(GL_PROJECTION_MATRIX, m_dProjectionMatrix);

	// Save the Projection matrix.  This is used later for
	// conversion of mouse coordinates to world coordinates.
	glGetIntegerv(GL_VIEWPORT, m_iViewport);

	// Reset the ModelView matrix
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
}

void C3dCamera::GetWorldCoord(int ix, int iy, GLdouble fz, SG_VECTOR& coord)
{
	GLdouble x, y, z, winX, winY, winZ;

	// Fix the yPos value.  MS Windows origin 0,0 is upper left
	// while OpenGL windows origin 0,0 is lower left...
	winX = (GLdouble)ix;
	winY = (GLdouble)m_iViewport[3]-iy;

	// Add the camera's focal length, or distance from 'LookAt' to 'Eye' position
	// to the given 'z' coordinate.
	fz += GetFocalLength();

	// Calculate the winZ coordinate:
	if(m_bPerspective)
		// Compensate for perspective view
		winZ = 0.5 + (((m_fFar+m_fNear)-(2*m_fFar*m_fNear)/fz))/(2*(m_fFar-m_fNear));
	else
		// winZ is linearly interpolated between the Near_Far clipping plane
		winZ = (fz-m_fNear)/(m_fFar-m_fNear);

	// Unproject the point
	gluUnProject(winX, winY, winZ,
		m_dModelViewMatrix,
		m_dProjectionMatrix,
		m_iViewport,
		&x, &y, &z);

	// Copmensate for rounding errors
	if(fabs(x) < SMALL_NUMBER)
		x = 0;
	if(fabs(y) < SMALL_NUMBER)
		y = 0;
	if(fabs(z) < SMALL_NUMBER)
		z = 0;

	coord.x = x;
	coord.y = y;
	coord.z = z;
}

void C3dCamera::GetScreenCoord(GLdouble wX, GLdouble wY, GLdouble wZ,
							   double& scrX, double& scrY, double& scrZ)
{
	gluProject(wX, wY, wZ,
		m_dModelViewMatrix,m_dProjectionMatrix,m_iViewport,
		&scrX,&scrY,&scrZ);
	scrY = (GLdouble)m_iViewport[3]-scrY;
	scrZ += GetFocalLength();
}

void C3dCamera::GetEyePos(GLdouble *x, GLdouble *y, GLdouble *z)
{
	*x = m_fEyePos.x;
	*y = m_fEyePos.y;
	*z = m_fEyePos.z;
}

void C3dCamera::SetEyePos(GLdouble x, GLdouble y, GLdouble z)
{
	m_fEyePos.x = x;
	m_fEyePos.y = y;
	m_fEyePos.z = z;

	CalculateYawPitchRoll();
	CalculateUpVector();
}

void C3dCamera::GetLookAtPos(GLdouble *x, GLdouble *y, GLdouble *z)
{
	*x = m_fLookAtPos.x;
	*y = m_fLookAtPos.y;
	*z = m_fLookAtPos.z;
}

void C3dCamera::SetLookAtPos(GLdouble x, GLdouble y, GLdouble z)
{
	m_fLookAtPos.x = x;
	m_fLookAtPos.y = y;
	m_fLookAtPos.z = z;
}

void C3dCamera::GetUpVector(GLdouble *x, GLdouble *y, GLdouble *z)
{
	*x = m_fUpVector.x;
	*y = m_fUpVector.y;
	*z = m_fUpVector.z;
}

void C3dCamera::SetUpVector(GLdouble x, GLdouble y, GLdouble z)
{
	m_fUpVector.x = x;
	m_fUpVector.y = y;
	m_fUpVector.z = z;
}

void C3dCamera::SetNearClipPlane(GLdouble fNear)
{
	m_fNear = fNear;

	// Reset the projection matrix (coordinate system)
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();

	if(m_bPerspective)
		// Perspective transformations.
		gluPerspective(m_fFovY, m_fAspect, m_fNear, m_fFar);

	else
		// Orthographic transformations.
		glOrtho(m_fLeft, m_fRight, m_fBottom, m_fTop, m_fNear, m_fFar); 

	// Save the Projection matrix.  This is used later for
	// conversion of mouse coordinates to world coordinates.
	glGetDoublev(GL_PROJECTION_MATRIX, m_dProjectionMatrix);

	// Reset the ModelView matrix
	glMatrixMode(GL_MODELVIEW);
}

void C3dCamera::SetFarClipPlane(GLdouble fFar)
{
	m_fFar = fFar;

	// Reset the projection matrix (coordinate system)
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();

	if(m_bPerspective)
		// Perspective transformations.
		gluPerspective(m_fFovY, m_fAspect, m_fNear, m_fFar);
	else
		// Orthographic transformations.
		glOrtho(m_fLeft, m_fRight, m_fBottom, m_fTop, m_fNear, m_fFar); 

	// Save the Projection matrix.  This is used later for
	// conversion of mouse coordinates to world coordinates.
	glGetDoublev(GL_PROJECTION_MATRIX, m_dProjectionMatrix);

	// Reset the ModelView matrix
	glMatrixMode(GL_MODELVIEW);
}

void C3dCamera::CalculateYawPitchRoll()
{
	CalculateFocalLength();

	GLdouble ax, ay, az;
	ax = m_fEyePos.x - m_fLookAtPos.x;
	ay = m_fEyePos.y - m_fLookAtPos.y;
	az = m_fEyePos.z - m_fLookAtPos.z;

	// Compensate for rounding errors
	if(fabs(ax) < SMALL_NUMBER)
		ax = 0.0f;
	if(fabs(ay) < SMALL_NUMBER)
		ay = 0.0f;
	if(fabs(az) < SMALL_NUMBER)
		az = 0.0f;

	// Calculate the Camera Pitch angle
	if(m_fUpVector.z >= 0)
		m_fPitch = Degreesf(asin(-az/m_fFocalLength));
	else
		m_fPitch = 180-Degreesf(asin(-az/m_fFocalLength));

	// Calculate the Camera 'Yaw' angle
	if(m_fPitch == 90.0f)
		m_fYaw = Degreesf(atan2(-m_fUpVector.x, -m_fUpVector.y));
	else if(m_fPitch == -90.0f)
		m_fYaw = Degreesf(atan2(m_fUpVector.x, m_fUpVector.y));
	else if(m_fPitch < 90.0f)
		m_fYaw = Degreesf(atan2(-ax, -ay));
	else
		m_fYaw = Degreesf(atan2(ax, ay));
}

void C3dCamera::CalculateUpVector()
{
	sgCMatrix matx;

	matx.Rotate(zero_p, x_axe, m_fPitch/180.0*Pi);
	matx.Rotate(zero_p, z_axe, -m_fYaw/180.0*Pi);

	m_fUpVector = z_axe;
	matx.ApplyMatrixToVector(zero_p,m_fUpVector);

	// Compensate for rounding errors
	if(fabs(m_fUpVector.x) < SMALL_NUMBER)
		m_fUpVector.x = 0.0f;
	if(fabs(m_fUpVector.y) < SMALL_NUMBER)
		m_fUpVector.y = 0.0f;
	if(fabs(m_fUpVector.z) < SMALL_NUMBER)
		m_fUpVector.z = 0.0f;
}

GLdouble C3dCamera::GetFocalLength()
{
	return(m_fFocalLength);
}

void C3dCamera::SetFocalLength(GLdouble length, BOOL bLookAtPos)
{
	GLdouble rx, ry, rz;

	if(bLookAtPos)
	{
		// Calculate the 'Eye' position by first getting the rotation about
		// the 'LookAt' position, set the new focal length, then set the 
		// rotation about the 'LookAt' position.
		GetRotationAboutLookAt(&rx, &ry, &rz);

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
		GetRotationAboutEye(&rx, &ry, &rz);

		// Set the member variable
		m_fFocalLength = length;

		SetRotationAboutEye(rx, ry, rz);
	}
}

void C3dCamera::CalculateFocalLength()
{
	// Calculate a new focal length based on the position of the
	// 'LookAt' and 'Eye' position, where:
	//    length = sqrt(a*a + b*b +c*c);

	GLdouble a, b, c, temp;
	a = m_fEyePos.x-m_fLookAtPos.x;
	b = m_fEyePos.y-m_fLookAtPos.y;
	c = m_fEyePos.z-m_fLookAtPos.z;

	temp = sqrt(a*a + b*b + c*c);
	if(temp == 0.0f)
		m_fFocalLength = 1.0f;
	else
		m_fFocalLength = temp;
}

void C3dCamera::GetRotationAboutEye(GLdouble *x, GLdouble *y, GLdouble *z)
{
	CalculateFocalLength();

	GLdouble ax, ay, az;
	ax = m_fEyePos.x - m_fLookAtPos.x;
	ay = m_fEyePos.y - m_fLookAtPos.y;
	az = m_fEyePos.z - m_fLookAtPos.z;

	// Compensate for rounding errors
	if(fabs(ax) < SMALL_NUMBER)
		ax = 0.0f;
	if(fabs(ay) < SMALL_NUMBER)
		ay = 0.0f;
	if(fabs(az) < SMALL_NUMBER)
		az = 0.0f;

	// Calculate the Camera Pitch angle
	if(m_fUpVector.z >= 0)
		m_fPitch = Degreesf(asin(-az/m_fFocalLength));
	else
		m_fPitch = 180-Degreesf(asin(-az/m_fFocalLength));


	// Calculate the Camera 'Yaw' angle
	if(m_fPitch == 90.0f)
		m_fYaw = Degreesf(atan2(-m_fUpVector.x, -m_fUpVector.y));
	else if(m_fPitch == -90.0f)
		m_fYaw = Degreesf(atan2(m_fUpVector.x, m_fUpVector.y));
	else if(m_fPitch < 90.0f)
		m_fYaw = Degreesf(atan2(-ax, -ay));
	else
		m_fYaw = Degreesf(atan2(ax, ay));

	// Pass the values back to the caller
	*x = m_fPitch;
	*y = m_fRoll;
	*z = m_fYaw;
}

void C3dCamera::SetRotationAboutEye(GLdouble x, GLdouble y, GLdouble z)
{
	sgCMatrix matx;
	SG_VECTOR LookAtPos;

	matx.Rotate(zero_p, x_axe, x/180.0*Pi);
	matx.Rotate(zero_p, z_axe, -z/180.0*Pi);

	LookAtPos.x = 0.0;
	LookAtPos.y = m_fFocalLength;
	LookAtPos.z = 0.0;
	matx.ApplyMatrixToVector(zero_p,LookAtPos);

	m_fLookAtPos.x = m_fEyePos.x + LookAtPos.x;
	m_fLookAtPos.y = m_fEyePos.y + LookAtPos.y;
	m_fLookAtPos.z = m_fEyePos.z + LookAtPos.z;

	// Compensate for rounding errors
	if(fabs(m_fLookAtPos.x) < SMALL_NUMBER)
		m_fLookAtPos.x = 0.0f;
	if(fabs(m_fLookAtPos.y) < SMALL_NUMBER)
		m_fLookAtPos.y = 0.0f;
	if(fabs(m_fLookAtPos.z) < SMALL_NUMBER)
		m_fLookAtPos.z = 0.0f;

	// Calculate our camera's UpVector using ONLY the 'X' (Pitch) and 'Z' (Yaw)
	// parameters
	m_fUpVector = z_axe;
	matx.ApplyMatrixToVector(zero_p,m_fUpVector);

	// Compensate for rounding errors
	if(fabs(m_fUpVector.x) < SMALL_NUMBER)
		m_fUpVector.x = 0.0f;
	if(fabs(m_fUpVector.y) < SMALL_NUMBER)
		m_fUpVector.y = 0.0f;
	if(fabs(m_fUpVector.z) < SMALL_NUMBER)
		m_fUpVector.z = 0.0f;

	// Just save the 'y' value for later use when calculating the camera's 'Up' vector
	// prior to calling gluLookAt()
	m_fRoll = y;
}

void C3dCamera::GetRotationAboutLookAt(GLdouble *x, GLdouble *y, GLdouble *z)
{
	CalculateFocalLength();

	GLdouble ax, ay, az;
	ax = m_fEyePos.x - m_fLookAtPos.x;
	ay = m_fEyePos.y - m_fLookAtPos.y;
	az = m_fEyePos.z - m_fLookAtPos.z;

	// Compensate for rounding errors
	if(fabs(ax) < SMALL_NUMBER)
		ax = 0.0f;
	if(fabs(ay) < SMALL_NUMBER)
		ay = 0.0f;
	if(fabs(az) < SMALL_NUMBER)
		az = 0.0f;

	// Calculate the Camera Pitch angle
	if(m_fUpVector.z >= 0)
		m_fPitch = Degreesf(asin(-az/m_fFocalLength));
	else
		m_fPitch = 180-Degreesf(asin(-az/m_fFocalLength));


	// Calculate the Camera 'Yaw' angle
	if(m_fPitch == 90.0f)
		m_fYaw = Degreesf(atan2(-m_fUpVector.x, -m_fUpVector.y));
	else if(m_fPitch == -90.0f)
		m_fYaw = Degreesf(atan2(m_fUpVector.x, m_fUpVector.y));
	else if(m_fPitch < 90.0f)
		m_fYaw = Degreesf(atan2(-ax, -ay));
	else
		m_fYaw = Degreesf(atan2(ax, ay));

	// Pass the values back to the caller
	*x = m_fPitch;
	*y = m_fRoll;
	*z = m_fYaw;
}

void C3dCamera::SetRotationAboutLookAt(GLdouble x, GLdouble y, GLdouble z)
{

	sgCMatrix matx;
	SG_VECTOR EyePos;

	matx.Rotate(zero_p, x_axe, x/180.0*Pi);
	matx.Rotate(zero_p, z_axe, -z/180.0*Pi);

	EyePos.x = 0.0;
	EyePos.y = -m_fFocalLength;
	EyePos.z = 0.0;
	matx.ApplyMatrixToVector(zero_p,EyePos);

	m_fEyePos.x = EyePos.x + m_fLookAtPos.x;
	m_fEyePos.y = EyePos.y + m_fLookAtPos.y;
	m_fEyePos.z = EyePos.z + m_fLookAtPos.z;

	// Compensate for rounding errors
	if(fabs(m_fEyePos.x) < SMALL_NUMBER)
		m_fEyePos.x = 0.0f;
	if(fabs(m_fEyePos.y) < SMALL_NUMBER)
		m_fEyePos.y = 0.0f;
	if(fabs(m_fEyePos.z) < SMALL_NUMBER)
		m_fEyePos.z = 0.0f;

	// Calculate our camera's UpVector using ONLY the 'X' (Pitch) and 'Z' (Yaw)
	// parameters
	m_fUpVector = z_axe;
	matx.ApplyMatrixToVector(zero_p,m_fUpVector);

	// Compensate for rounding errors
	if(fabs(m_fUpVector.x) < SMALL_NUMBER)
		m_fUpVector.x = 0.0f;
	if(fabs(m_fUpVector.y) < SMALL_NUMBER)
		m_fUpVector.y = 0.0f;
	if(fabs(m_fUpVector.z) < SMALL_NUMBER)
		m_fUpVector.z = 0.0f;

	// Just save the 'y' value for later use when calculating the camera's 'Up' vector
	// prior to calling gluLookAt()
	m_fRoll = y;
}

void C3dCamera::GetRotationMatrix(sgCMatrix& XformMatrix)
{
	// Calculate the cameras' rotation matrix
	PrepareMatrix(0.0f, 0.0f, 0.0f,       // Origin
		-m_fPitch,  m_fRoll,  m_fYaw, // Rotation
		0.0f, 0.0f, 0.0f,       // Translation
		XformMatrix);
}

void C3dCamera::GetInvRotationMatrix(sgCMatrix& XformMatrix)
{
	// Calculate the cameras' inverse rotation matrix
	PrepareInvMatrix(0.0f, 0.0f, 0.0f,        // Origin
		-m_fPitch,  m_fRoll,  m_fYaw,  // Rotation
		0.0f, 0.0f, 0.0f,        // Translation
		XformMatrix);
}

void C3dCamera::GetVectorsForRayTracingCamera(SG_VECTOR& eye_loc,
											  SG_VECTOR& eye_look_at,
											  SG_VECTOR& eye_y,
											  SG_VECTOR& eye_x)
{
	GetEyePos(&eye_loc.x, &eye_loc.y, &eye_loc.z);
	GetLookAtPos(&eye_look_at.x, &eye_look_at.y, &eye_look_at.z);

	SG_VECTOR p1, p2;
	GetWorldCoord(m_iScreenWidth/2,m_iScreenHeight/2,0.0,p1);
	GetWorldCoord(m_iScreenWidth/2,0,0.0, p2);

	eye_y = sgSpaceMath::VectorsSub(p2,p1);

	sgSpaceMath::NormalVector(eye_y);

	GetWorldCoord(m_iScreenWidth, m_iScreenHeight/2,0.0,p2);

	eye_x = sgSpaceMath::VectorsSub(p2,p1);
	sgSpaceMath::NormalVector(eye_x);

}

typedef struct 
{
	SG_VECTOR       vec;
	GLdouble    fDepth;
} boundingPlane;

void C3dCamera::FitBounds(double minX, double minY, double minZ, 
						  double maxX, double maxY,double maxZ)
{
	boundingPlane boundsRight;
	boundingPlane boundsLeft;
	boundingPlane boundsTop;
	boundingPlane boundsBottom;
	boundingPlane boundsNear;
	boundingPlane boundsFar;
	SG_VECTOR boundsMax;
	SG_VECTOR boundsMin;
	SG_VECTOR vecCenter;
	SG_VECTOR vertices[8];
	SG_VECTOR vecOffset;
	sgCMatrix matrix;
	GLdouble focalLength;
	GLdouble fDepthWidth, fDepthHeight;
	GLdouble rx, ry, rz;
	GLdouble fTan;
	GLdouble fx, fz;

	// Initialize our function variables
	boundsRight.fDepth  = 0.0;
	boundsLeft.fDepth = 0.0;
	boundsTop.fDepth  = 0.0;
	boundsBottom.fDepth = 0.0;
	boundsNear.fDepth = 0.0;
	boundsFar.fDepth  = 0.0;
	memset(&(boundsRight.vec),0,sizeof(SG_VECTOR));
	memset(&(boundsLeft.vec),0,sizeof(SG_VECTOR));
	memset(&(boundsTop.vec),0,sizeof(SG_VECTOR));
	memset(&(boundsBottom.vec),0,sizeof(SG_VECTOR));
	memset(&(boundsNear.vec),0,sizeof(SG_VECTOR));
	memset(&(boundsFar.vec),0,sizeof(SG_VECTOR));
	memset(&(vecOffset),0,sizeof(SG_VECTOR));
	memset(&(boundsMin),0,sizeof(SG_VECTOR));
	memset(&(boundsMax),0,sizeof(SG_VECTOR));
	memset(&(vecCenter),0,sizeof(SG_VECTOR));

	fTan = (double)tanf((float)Radiansf(m_fFovY/2));


	// Get the cameras rotatiom about the LookAt position, as we 
	// will use this to restore the rotation values after we move
	// the camera and it's focal length.
	GetRotationAboutLookAt(&rx, &ry, &rz);

	// Copy the bounds to our local variable
	boundsMin.x = minX; boundsMin.y = minY; boundsMin.z = minZ;
	boundsMax.x = maxX; boundsMax.y = maxY; boundsMax.z = maxZ;

	double spanX, spanY, spanZ;

	spanX = Diff(boundsMax.x, boundsMin.x);
	spanY = Diff(boundsMax.y, boundsMin.y);
	spanZ = Diff(boundsMax.z, boundsMin.z);

	vecCenter.x = boundsMax.x - fabs(spanX)/2;
	vecCenter.y = boundsMax.y - fabs(spanY)/2;
	vecCenter.z = boundsMax.z - fabs(spanZ)/2;

	boundsMax.x = spanX/2;
	boundsMax.y = spanY/2;
	boundsMax.z = spanZ/2;

	boundsMin.x = -spanX/2;
	boundsMin.y = -spanY/2;
	boundsMin.z = -spanZ/2;

	// Given the bounding box, fill in the missing vertices to complete our
	// cube
	vertices[0] = boundsMax;  // Left
	vertices[1].x = boundsMax.x; vertices[1].y = boundsMax.y; vertices[1].z = boundsMin.x;
	vertices[2].x = boundsMax.x; vertices[2].y = boundsMin.y; vertices[2].z = boundsMin.x;
	vertices[3].x = boundsMax.x; vertices[3].y = boundsMin.y; vertices[3].z = boundsMax.x;

	vertices[4] = boundsMin;
	vertices[5].x = boundsMin.x; vertices[5].y = boundsMin.y; vertices[5].z = boundsMax.x;
	vertices[6].x = boundsMin.x; vertices[6].y = boundsMax.y; vertices[6].z = boundsMax.x;
	vertices[7].x = boundsMin.x; vertices[7].y = boundsMax.y; vertices[7].z = boundsMin.x;

	// Get the cameras rotation matrix
	GetRotationMatrix(matrix);

	for(int i=0; i<8; i++)
	{
		// Transform the vertice by the camera rotation matrix.  Since we define the 
		// default 'Up' camera position as Z-axis Up, the coordinates map as follows:
		//    X maps to Width,
		//    Y maps to Depth
		//    Z mpas to Height
		zero_p.x  = zero_p.y  =zero_p.z  =0.0;
		matrix.ApplyMatrixToVector(zero_p, vertices[i]);

		// Calculate the focal length needed to fit the near bounding plane
		fDepthWidth  = (fabs(vertices[i].x)/fTan/m_fAspect)-vertices[i].y;
		fDepthHeight = (fabs(vertices[i].z)/fTan)-vertices[i].y;


		// Calculate the Near clipping bounds.  This will be used to fit Isometric views and
		// for calculating the Near/Far clipping m_fFrustum.
		if(vertices[i].y<0)
		{
			if( fabs(vertices[i].x) > fabs(boundsNear.vec.x) ||
				(fabs(vertices[i].x) == boundsNear.vec.x && fabs(vertices[i].y) > fabs(boundsNear.vec.z)) )
			{
				boundsNear.vec.x = fabs(vertices[i].x);
				boundsNear.vec.z = fabs(vertices[i].y);
			}

			if( fabs(vertices[i].z) > fabs(boundsNear.vec.y) ||
				(fabs(vertices[i].z) == boundsNear.vec.y) )
			{
				boundsNear.vec.y = fabs(vertices[i].z);
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
			if( fabs(vertices[i].x) > fabs(boundsFar.vec.x) ||
				(fabs(vertices[i].x) == boundsFar.vec.x && fabs(vertices[i].y) < fabs(boundsFar.vec.z)) )
			{
				boundsFar.vec.x = vertices[i].x;
				boundsFar.vec.z = vertices[i].y;
			}

			if( fabs(vertices[i].z) > fabs(boundsFar.vec.y) ||
				(fabs(vertices[i].z) == fabs(boundsFar.vec.y)) )
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
	if(m_bPerspective)
	{
		sgCMatrix invMatrix;

		if(boundsRight.fDepth == boundsLeft.fDepth &&
			boundsTop.fDepth == boundsBottom.fDepth )
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
			fx = (boundsRight.fDepth + boundsLeft.fDepth)/2;
			fz = (boundsTop.fDepth + boundsBottom.fDepth)/2;

			// Calculate the offset necessary to center the bounding box.  Note that we
			// use a scaling factor for centering the non-limiting bounds to achieve a
			// more visually appealing center.
			if(fx > fz)
			{
				GLdouble fScale = sqrt(boundsTop.fDepth/boundsBottom.fDepth);
				GLdouble fTop = fTan*fx - fTan*boundsTop.fDepth;
				GLdouble fBottom = fTan*fx - fTan*boundsBottom.fDepth;

				vecOffset.x = (fTan*m_fAspect*boundsRight.fDepth - fTan*m_fAspect*fx);
				vecOffset.z = (fBottom-fTop*fScale)/2;
			}
			else
			{
				GLdouble fScale = sqrt(boundsLeft.fDepth/boundsRight.fDepth);
				GLdouble fRight = fTan*m_fAspect*fz - fTan*m_fAspect*boundsRight.fDepth;
				GLdouble fLeft  = fTan*m_fAspect*fz - fTan*m_fAspect*boundsLeft.fDepth;

				vecOffset.z = (fTan*boundsTop.fDepth - fTan*fz);
				vecOffset.x = (fLeft - fRight*fScale)/2; 
			}
		}

		// Now that we have the offsets necessary to center the bounds, we must rotate
		// the vertices (camera coordinates) by the cameras inverse rotation matrix to
		// convert the offsets to world coordinates.
		GetInvRotationMatrix(invMatrix);
		zero_p.x  = zero_p.y  =zero_p.z  =0.0;

		invMatrix.ApplyMatrixToVector(zero_p,vecOffset);

	}
	else
	{
		// Isometric View
		// Calculate the focal length needed to fit the near bounding plane
		if(m_iScreenWidth <= m_iScreenHeight)
		{
			fx = boundsNear.vec.x/tanf((float)Radiansf(m_fFovY/2));
			fz = boundsNear.vec.y/tanf((float)Radiansf(m_fFovY/2))/((GLdouble)m_iScreenHeight/(GLdouble)m_iScreenWidth);
		}
		else
		{
			fx = boundsNear.vec.x/tanf((float)Radiansf(m_fFovY/2))/m_fAspect;
			fz = boundsNear.vec.y/tanf((float)Radiansf(m_fFovY/2));
		}
	}

	// Set the focal length equal to the largest length required to fit either the 
	// Width (Horizontal) or Height (Vertical)
	focalLength = (fx > fz? fx : fz);

	// Set the camera's new LookAt position to focus on the center
	// of the bounding box.
	SetLookAtPos(vecCenter.x+vecOffset.x, vecCenter.y+vecOffset.y, vecCenter.z+vecOffset.z);

	// Set the camera focal Length
	if(focalLength > m_fNear)
		SetFocalLength(focalLength, TRUE);

	// Adjust the Near clipping plane if necessary
	//  if((boundsNear.fDepth/2) > 0.5f)
	//    m_fNear = boundsNear.fDepth/2;

	// Adjust the Far clipping plane if necessary
	if(focalLength+boundsFar.fDepth > m_fFar)
		m_fFar = focalLength+boundsFar.fDepth;

	// Recalculate the camera view m_fFrustum;
	ResetView();

	// Restore the cameras rotation about the LookAt position
	SetRotationAboutLookAt(rx, ry, rz);
}

void C3dCamera::ExtractFrustum()
{
	// Extracts The Current View m_fFrustum Plane Equations

	float proj[16]; // For Grabbing The PROJECTION Matrix
	float modl[16]; // For Grabbing The MODELVIEW Matrix
	double  clip[16]; // Result Of Concatenating PROJECTION and MODELVIEW
	double  t;      // Temporary Work Variable

	glGetFloatv( GL_PROJECTION_MATRIX, proj );      // Grab The Current PROJECTION Matrix
	glGetFloatv( GL_MODELVIEW_MATRIX, modl );     // Grab The Current MODELVIEW Matrix

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
	t = (double) sqrt( m_fFrustum[0][0] * m_fFrustum[0][0] + m_fFrustum[0][1] * m_fFrustum[0][1] + m_fFrustum[0][2] * m_fFrustum[0][2] );
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
	t = (double) sqrt( m_fFrustum[1][0] * m_fFrustum[1][0] + m_fFrustum[1][1] * m_fFrustum[1][1] + m_fFrustum[1][2] * m_fFrustum[1][2] );
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
	t = (double) sqrt( m_fFrustum[2][0] * m_fFrustum[2][0] + m_fFrustum[2][1] * m_fFrustum[2][1] + m_fFrustum[2][2] * m_fFrustum[2][2] );
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
	t = sqrt( m_fFrustum[3][0] * m_fFrustum[3][0] + m_fFrustum[3][1] * m_fFrustum[3][1] + m_fFrustum[3][2] * m_fFrustum[3][2] );
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
	t = sqrt( m_fFrustum[4][0] * m_fFrustum[4][0] + m_fFrustum[4][1] * m_fFrustum[4][1] + m_fFrustum[4][2] * m_fFrustum[4][2] );
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
	t = (double) sqrt( m_fFrustum[5][0] * m_fFrustum[5][0] + m_fFrustum[5][1] * m_fFrustum[5][1] + m_fFrustum[5][2] * m_fFrustum[5][2] );
	m_fFrustum[5][0] /= t;
	m_fFrustum[5][1] /= t;
	m_fFrustum[5][2] /= t;
	m_fFrustum[5][3] /= t;
}

bool C3dCamera::ProjectCenterOfScreenOnPlane(double plD, SG_VECTOR& plN, SG_POINT& resP)
{
	SG_POINT wP;
	SG_VECTOR tmpVec;
	
	GetWorldCoord(m_iScreenWidth/2, m_iScreenHeight/2, 0.0 , tmpVec);
	wP.x = tmpVec.x;	wP.y = tmpVec.y;  wP.z = tmpVec.z;
	SG_VECTOR viewNorm;
	viewNorm.x = (double)(m_fLookAtPos.x-m_fEyePos.x);
	viewNorm.y = (double)(m_fLookAtPos.y-m_fEyePos.y);
	viewNorm.z = (double)(m_fLookAtPos.z-m_fEyePos.z);
	sgSpaceMath::NormalVector(viewNorm);

	if (sgSpaceMath::IntersectPlaneAndLine(plN,plD,wP,viewNorm,resP)!=1)
		return false;
	return true;
}

void  C3dCamera::StartAnimatePosition(CWnd* wnd,CAMERA_POSITION newPos)
{
	if (m_timer_ID)
		return; 

	ASSERT(wnd);

	SG_POINT oldPosPnt;
	oldPosPnt.x = m_fEyePos.x;
	oldPosPnt.y = m_fEyePos.y;
	oldPosPnt.z = m_fEyePos.z;

	SG_POINT targPosPnt;
	targPosPnt.x = m_fLookAtPos.x;
	targPosPnt.y = m_fLookAtPos.y;
	targPosPnt.z = m_fLookAtPos.z;

	double  dist = sgSpaceMath::PointsDistance(oldPosPnt,targPosPnt);

	SG_POINT newPosPnt = targPosPnt;

	switch(newPos) 
	{
	case CP_USER_POSITION:
		return;
	case CP_FRONT:
		newPosPnt.x+=dist;
		break;
	case CP_BACK:
		newPosPnt.x-=dist;
		break;
	case CP_TOP:
		newPosPnt.z+=dist;
		break;
	case CP_BOTTOM:
		newPosPnt.z-=dist;
		break;
	case CP_LEFT:
		newPosPnt.y+=dist;
		break;
	case CP_RIGHT:
		newPosPnt.y-=dist;
		break;
	case CP_ISO_FRONT:
		newPosPnt.x+=dist*0.5773502691f;
		newPosPnt.y+=dist*0.5773502691f;
		newPosPnt.z+=dist*0.5773502691f;
		break;
	case CP_ISO_BACK:
		newPosPnt.x-=dist*0.5773502691f;
		newPosPnt.y-=dist*0.5773502691f;
		newPosPnt.z+=dist*0.5773502691f;
		break;
	default:
		ASSERT(0);
		break;
	}

	SG_VECTOR begV;
	begV.x = oldPosPnt.x-targPosPnt.x;
	begV.y = oldPosPnt.y-targPosPnt.y;
	begV.z = oldPosPnt.z-targPosPnt.z;
	SG_VECTOR endV;
	endV.x = newPosPnt.x-targPosPnt.x;
	endV.y = newPosPnt.y-targPosPnt.y;
	endV.z = newPosPnt.z-targPosPnt.z;
	sgSpaceMath::NormalVector(begV);
	sgSpaceMath::NormalVector(endV);

	double  alfa = acos(begV.x*endV.x+begV.y*endV.y+begV.z*endV.z);

	size_t stepsCount;

	if (alfa>1.57)
		stepsCount = 10;
	else
	    stepsCount = 6;
	double  alfa_step = alfa/(double)stepsCount;

	SG_VECTOR normal; double Pld;
	
	normal.x=normal.y=normal.z=0.0;
	if (!sgSpaceMath::PlaneFromPoints(targPosPnt,oldPosPnt,newPosPnt,normal,Pld))
	{
		switch(newPos) 
		{
		case CP_FRONT:
		case CP_BACK:
		case CP_TOP:
		case CP_BOTTOM:
			normal.y = 1.0;
			break;
		case CP_LEFT:
		case CP_RIGHT:
			normal.z = 1.0;
			break;
		case CP_ISO_FRONT:
		case CP_ISO_BACK:
		case CP_USER_POSITION:
		default:
			ASSERT(0);
			break;
		}
	}

	m_animation_positions.clear();
	m_animation_positions.reserve(stepsCount);

	sgCMatrix matr;
	matr.Rotate(targPosPnt,normal,alfa_step);

	for (size_t i=0;i<stepsCount;i++)
	{
		matr.ApplyMatrixToVector(targPosPnt,begV);
		oldPosPnt.x=targPosPnt.x+begV.x*dist;
		oldPosPnt.y=targPosPnt.y+begV.y*dist;
		oldPosPnt.z=targPosPnt.z+begV.z*dist;
		m_animation_positions.push_back(oldPosPnt);
	}
	m_cur_animation_position_index = 0;
	m_timer_ID = wnd->SetTimer(1, 50, 0);
	m_enumCameraPosition = newPos;
}

void  C3dCamera::AnimatePosition(CWnd* wnd)
{
	SetEyePos(m_animation_positions[m_cur_animation_position_index].x,
				m_animation_positions[m_cur_animation_position_index].y,
				m_animation_positions[m_cur_animation_position_index].z);
	//ResetView();
	wnd->Invalidate();
	m_cur_animation_position_index++;
	
	if (m_cur_animation_position_index==m_animation_positions.size())
	{
		wnd->KillTimer(m_timer_ID);
		m_timer_ID = NULL;
		m_cur_animation_position_index = 0;
		m_animation_positions.clear();
	}
}



double  RoundGrid(double oldVal, float gridSz)
{
	int  celoeColichestvoGrids = (int)(oldVal/gridSz);

	if (oldVal>0)
	{
		if (fabs(oldVal-celoeColichestvoGrids*gridSz)<
			fabs(oldVal-(celoeColichestvoGrids+1)*gridSz))
			return (celoeColichestvoGrids*gridSz);
		else
			return ((celoeColichestvoGrids+1)*gridSz);
	}
	else
	{
		if (fabs(oldVal-celoeColichestvoGrids*gridSz)<
			fabs(oldVal-(celoeColichestvoGrids-1)*gridSz))
			return (celoeColichestvoGrids*gridSz);
		else
			return ((celoeColichestvoGrids-1)*gridSz);
	}

}

double  FloorGrid(double oldVal, float gridSz)
{
	int  celoeColichestvoGrids = (int)(oldVal/gridSz);

	if (oldVal>0)
		return (double)(celoeColichestvoGrids*gridSz);
	else
		return (double)((celoeColichestvoGrids-1)*gridSz);
}
