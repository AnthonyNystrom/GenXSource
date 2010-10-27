#include "ScreenViewPhoto.h"
#include "GUIImage.h"
#include "Graphics.h"
#include "stringres.h"
#include "GUIPhotoImage.h"
#include "SurfacesPool.h"
#include "N2FMessage.h"
#include "N2FOutboxManager.h"

#include "GUIFooter.h"

#include "PhotoLibrary.h"
#include "LibPhotoItem.h"

ScreenViewPhoto::ScreenViewPhoto( const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID )
:BaseScreen(newRect, pAppMng, screenID)
{

	AddListener(EEI_WINDOW_WILL_ACTIVATE, this);
	AddListener(EEI_WINDOW_DID_ACTIVATE, this);
	AddListener(EEI_SOFT1_PRESSED, this);


	surfPool = new SurfacesPool(1, (uint16)newRect.minDx, (uint16)newRect.minDy);

	image = new GUIPhotoImage(IDB_PHOTO_LOADING_BAR, IDB_BAD_PHOTO, surfPool, pManager, this);
	ControlRect rect = newRect;
	rect.x = 0;
	rect.y = 0;
	image->SetControlRect(rect);

	oldItem = new LibPhotoItem();

}

ScreenViewPhoto::~ScreenViewPhoto( void )
{
	SAFE_DELETE(surfPool);
	SAFE_DELETE(oldItem);
}


//const char16	* ScreenViewPhoto::PopUpGetTextByIndex( int32 index, int32 &id, int32 prevId )
//{
//	if (index == 0)
//	{
//		if (pManager->GetWorkingMessage()->HasPhoto(pManager->GetWorkingPhoto()))
//		{
//			id = EPI_REMOVE;
//			return pManager->GetStringWrapper()->GetStringText(IDS_REMOVE_PHOTO);
//		}
//		else if (pManager->GetWorkingMessage()->IsPossibleToAttachPhoto())
//		{
//			id = EPI_ATTACH;
//			return pManager->GetStringWrapper()->GetStringText(IDS_ATTACH_PHOTO);
//		}
//		else
//		{
//			id = EPI_BACK;
//			return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
//		}
//	}
//	if (prevId != EPI_BACK)
//	{
//		id = EPI_BACK;
//		return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
//	}
//	return NULL;
//}

//void ScreenViewPhoto::PopUpOnItemSelected( int32 index, int32 id )
//{
//	switch(id)
//	{
//	case EPI_BACK:
//		{
//			BackToPrevSrceen();
//		}
//		break;
//	case EPI_ATTACH:
//		{
//			pManager->GetWorkingMessage()->AttachPhoto(pManager->GetWorkingPhoto());
//			BackToPrevSrceen();
//		}
//		break;
//	case EPI_REMOVE:
//		{
//			pManager->GetWorkingMessage()->RemovePhoto(pManager->GetWorkingPhoto());
//			BackToPrevSrceen();
//		}
//		break;
//	}
//}



bool ScreenViewPhoto::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_SOFT1_PRESSED:
		{
			switch(state)
			{
			case EPI_BACK:
				{
					BackToPrevSrceen();
				}
				break;
			case EPI_ATTACH:
				{
					pManager->GetWorkingMessage()->AttachPhoto(pManager->GetWorkingPhoto());
					BackToPrevSrceen();
				}
				break;
			case EPI_REMOVE:
				{
					pManager->GetWorkingMessage()->RemovePhoto(pManager->GetWorkingPhoto());
					BackToPrevSrceen();
				}
				break;
			case EPI_UPLOAD:
				{
					pManager->GetWorkingMessage()->AttachPhoto(pManager->GetWorkingPhoto());
					pManager->GetWorkingMessage()->SetText(pManager->GetWorkingPhoto()->GetName(), ETT_PHOTO_TITLE);
					//pManager->GetWorkingMessage()->SetOwner(pManager->GetOutboxManager());
					pManager->ChangeWindow(ESN_SEND_SELECTION, false);
				}
				break;
			}
			return true;
		}
		break;
	case EEI_WINDOW_WILL_ACTIVATE:
		{
			if (*oldItem != *pManager->GetWorkingPhoto())
			{
				image->SetPhotoItem(NULL);
			}
		}
		break;
	case EEI_WINDOW_DID_ACTIVATE:
		{
			if (*oldItem != *pManager->GetWorkingPhoto())
			{
				image->SetPhotoItem(pManager->GetWorkingPhoto());
			}
			*oldItem = *pManager->GetWorkingPhoto();
		}
		break;
	}
	bool res = BaseScreen::OnEvent(eventID, pData);
	if (eventID == EEI_WINDOW_DID_ACTIVATE)
	{
		GUIFooter *ft = (GUIFooter*)GUISystem::Instance()->GetByID(ECIDS_FOOTER_ID);
		if (pManager->GetWorkingMessage()->GetType() == EMT_PHOTO)
		{
			state = EPI_UPLOAD;
			ft->SetPositiveText(IDS_UPLOAD);
		}
		else if (pManager->GetWorkingMessage()->HasPhoto(pManager->GetWorkingPhoto()))
		{
			state = EPI_REMOVE;
			ft->SetPositiveText(IDS_REMOVE);
		}
		else if (pManager->GetWorkingMessage()->IsPossibleToAttachPhoto())
		{
			state = EPI_ATTACH;
			ft->SetPositiveText(IDS_ATTACH);
		}
		else
		{
			state = EPI_BACK;
			ft->SetPositiveText(IDS_BACK);
		}
	}
	return res;
}

bool ScreenViewPhoto::PopUpShouldOpen()
{
	return false;
}



