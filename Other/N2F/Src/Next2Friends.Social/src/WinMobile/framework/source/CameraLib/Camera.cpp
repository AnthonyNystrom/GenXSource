#include "Camera.h"
#include "CameraWM.h"


Camera * Camera::Create()
{
	return new CameraWM();
}

void Camera::SetListener( CameraListener *listener )
{
	pListener = listener;
}

Camera::Camera()
{

}
Camera::~Camera()
{

}

