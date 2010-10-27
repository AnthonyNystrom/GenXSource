#include "Camera.h"
#include "CameraBrew.h"


Camera * Camera::Create()
{
	return new CameraBrew();
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

