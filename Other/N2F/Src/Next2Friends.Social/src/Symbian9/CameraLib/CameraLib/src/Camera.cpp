#include "Camera.h"
#include "CameraSis9.h"


Camera * Camera::Create()
{
	return new CameraSis();
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

