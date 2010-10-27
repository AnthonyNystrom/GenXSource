#include "ScreenCamera.h"
#include "GUIImage.h"
#include "Graphics.h"
#include "stringres.h"
#include "GUIHeader.h"
#include "GUIFooter.h"
#include "GUIListView.h"
#include "GUIMenuItem.h"
#include "GUIText.h"

#include "Camera.h"

#include "SkinProperties.h"

#include "ScreenAsk.h"
#include "N2FMessage.h"


ScreenCamera::ScreenCamera( const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID )
:BaseScreen(newRect, pAppMng, screenID)
{
	camera = Camera::Create();
	AddListener(EEI_WINDOW_DID_ACTIVATE, this);
	AddListener(EEI_WINDOW_WILL_DEACTIVATE, this);
	AddListener(EEI_SOFT1_PRESSED, this);

	camera->SetSnapshotWidth(640);
	prevWidth = 216;
	int32 h = camera->SetPreviewWidth(prevWidth);
	camera->SetPreviewPos(newRect.x + (newRect.minDx - prevWidth) / 2, newRect.y + (newRect.minDy - h) / 2);

	camera->SetListener(this);

	GetApplication()->GetFileSystem()->Remove("photo.jpg");
	//File *fl = GetApplication()->GetFileSystem()->Open("photo.jpg", File::EFM_CREATE);
	camera->SetFileForSnapshot("photo.jpg");



}

ScreenCamera::~ScreenCamera( void )
{
	SAFE_DELETE(camera);
}



bool ScreenCamera::PopUpShouldOpen()
{
	return false;
}

bool ScreenCamera::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_WINDOW_DID_ACTIVATE:
		{
			camera->StartPreview();
		}
		break;
	case EEI_WINDOW_WILL_DEACTIVATE:
		{
			camera->StopPreview();
		}
		break;
	case EEI_SOFT1_PRESSED:
		{
			camera->MakeSnapshot();
		}
		break;
	}
	return BaseScreen::OnEvent(eventID, pData);
}

void ScreenCamera::OnCameraFailed()
{
	BackToPrevSrceen();
}

void ScreenCamera::OnSnaphotCreated()
{
	pManager->SetWorkingPhoto(pManager->GetPhotoLib()->MoveToLibrary(EPST_PHONE, "photo.jpg"));
	pManager->ChangeWindow(ESN_VIEW_PHOTO, false, ESN_ASK);
}

void ScreenCamera::OnPreviewStarted()
{

}

void ScreenCamera::OnPreviewStopped()
{

}

void ScreenCamera::OnCameraCancelled()
{
	BackToPrevSrceen();
}