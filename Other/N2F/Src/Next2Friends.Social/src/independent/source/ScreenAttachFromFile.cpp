#include "ScreenAttachFromFile.h"
#include "GUIImage.h"
#include "Graphics.h"
#include "stringres.h"
#include "GUIHeader.h"
#include "GUIFooter.h"
#include "GUIListView.h"
#include "GUIMenuItem.h"
#include "GUIText.h"
#include "GUIItemsList.h"
#include "GUIPhotoImage.h"

#include "GUITabList.h"

#include "ScreenAsk.h"
#include "VList.h"

#include "N2FData.h"
#include "N2FDraftsManager.h"
#include "N2FOutboxManager.h"
#include "N2FMessage.h"

#include "SkinProperties.h"

#include "PhotoLibrary.h"
#include "LibPhotoItem.h"
#include "SurfacesPool.h"

ScreenAttachFromFile::ScreenAttachFromFile( const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID )
:BaseScreen(newRect, pAppMng, screenID)
{


	AddListener(EEI_WINDOW_WILL_ACTIVATE, this);
	AddListener(EEI_WINDOW_DID_ACTIVATE, this);
	AddListener(EEI_WINDOW_WILL_DEACTIVATE, this);
	tabList = new GUITabList(this, newRect);
	tabList->SetHeaderDrawType(EDT_TAB_BACK);
	tabList->SetSelectorDrawType(EDT_TAB_SELECTOR);
	tabList->AddListener(EEI_TAB_PAGE_SWITCHED, this);

	int32 h = GUISystem::Instance()->GetSkin()->GetSprite(EDT_PHOTO_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight();

	//=========  create first page
	GUIImage *pi = new GUIImage(IDB_PHOTO_ICONS);
	pi->SetFrame(1);

	listPhotos[0] = new GUIItemsList(this);
	listPhotos[0]->SetPrescroll(h / 2);
	listPhotos[0]->SetInnerMargins(0, 0, ECP_MENULIST_MARGIN, ECP_MENULIST_MARGIN);


	tabList->AddPage(listPhotos[0], pi, NULL);
	//====================================



	//=========  create second page
	pi = new GUIImage(IDB_PHOTO_ICONS);
	pi->SetFrame(0);

	listPhotos[1] = new GUIItemsList(this);
	listPhotos[1]->SetPrescroll(h / 2);
	listPhotos[1]->SetInnerMargins(0, 0, ECP_MENULIST_MARGIN, ECP_MENULIST_MARGIN);


	tabList->AddPage(listPhotos[1], pi, NULL);
	//====================================


	surfPool = new SurfacesPool((GetApplication()->GetGraphicsSystem()->GetHeight() / h * 2 + 1) * 2, 50, 50);


}

ScreenAttachFromFile::~ScreenAttachFromFile( void )
{
	SAFE_DELETE(surfPool);
}


const char16	* ScreenAttachFromFile::PopUpGetTextByIndex( int32 index, int32 &id, int32 prevId )
{
	if (index == 0)
	{
		int32 ind = tabList->GetActivePageIndex();
		LibPhotoItem *pi = (LibPhotoItem *)listPhotos[ind]->GetFocusedOwner();
		if (pi)
		{
			if (pManager->GetWorkingMessage()->GetType() == EMT_PHOTO)
			{
				id = EPI_UPLOAD;
				return pManager->GetStringWrapper()->GetStringText(IDS_UPLOAD);
			}
			if (pManager->GetWorkingMessage()->HasPhoto(pi))
			{
				id = EPI_REMOVE;
				return pManager->GetStringWrapper()->GetStringText(IDS_REMOVE_PHOTO);
			}
			else if (pManager->GetWorkingMessage()->IsPossibleToAttachPhoto())
			{
				id = EPI_ATTACH;
				return pManager->GetStringWrapper()->GetStringText(IDS_ATTACH_PHOTO);
			}
		}
	}
	if (prevId == EPI_BACK)
	{
		return NULL;
	}

	if (prevId == EPI_VIEW)
	{
		id = EPI_BACK;
		return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
	}

	if (prevId == EPI_INTERNAL_MEMORY || prevId == EPI_EXTERNAL_MEMORY)
	{
		int32 ind = tabList->GetActivePageIndex();
		LibPhotoItem *pi = (LibPhotoItem *)listPhotos[ind]->GetFocusedOwner();
		if (pi)
		{
			id = EPI_VIEW;
			return pManager->GetStringWrapper()->GetStringText(IDS_VIEW);
		}
		else
		{
			id = EPI_BACK;
			return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
		}
	}

	int32 ind = tabList->GetActivePageIndex();
	if (ind == 0)
	{
		id = EPI_EXTERNAL_MEMORY;
		return pManager->GetStringWrapper()->GetStringText(IDS_EXTERNAL_MEMORY);
	}
	else
	{
		id = EPI_INTERNAL_MEMORY;
		return pManager->GetStringWrapper()->GetStringText(IDS_INTERNAL_MEMORY);
	}


	return NULL;
}

void ScreenAttachFromFile::PopUpOnItemSelected( int32 index, int32 id )
{
	switch(id)
	{
	case EPI_BACK:
		{
			BackToPrevSrceen();
		}
		break;
	case EPI_UPLOAD:
		{
			LibPhotoItem *pi = (LibPhotoItem *)listPhotos[tabList->GetActivePageIndex()]->GetFocusedOwner();
			pManager->GetWorkingMessage()->AttachPhoto(pi);
			pManager->GetWorkingMessage()->SetText(pi->GetName(), ETT_PHOTO_TITLE);
			//pManager->GetWorkingMessage()->SetOwner(pManager->GetOutboxManager());
			pManager->ChangeWindow(ESN_SEND_SELECTION, false);
		}
		break;
	case EPI_ATTACH:
		{
			LibPhotoItem *pi = (LibPhotoItem *)listPhotos[tabList->GetActivePageIndex()]->GetFocusedOwner();
            pManager->GetWorkingMessage()->AttachPhoto(pi);
		}
		break;
	case EPI_REMOVE:
		{
			LibPhotoItem *pi = (LibPhotoItem *)listPhotos[tabList->GetActivePageIndex()]->GetFocusedOwner();
			pManager->GetWorkingMessage()->RemovePhoto(pi);
		}
		break;
	case EPI_INTERNAL_MEMORY:
		{
			tabList->SwitchPage(0);
		}
		break;
	case EPI_EXTERNAL_MEMORY:
		{
			tabList->SwitchPage(1);
		}
		break;
	case EPI_VIEW:
		{
			LibPhotoItem *pi = (LibPhotoItem *)listPhotos[tabList->GetActivePageIndex()]->GetFocusedOwner();
			pManager->SetWorkingPhoto(pi);
			pManager->ChangeWindow(ESN_VIEW_PHOTO, false);
		}
		break;
	}
}


void ScreenAttachFromFile::ItemsListOnItem( GUIItemsList *forList, void *owner )
{
	pManager->SetWorkingPhoto((LibPhotoItem*)owner);
	pManager->ChangeWindow(ESN_VIEW_PHOTO, false);
}

bool ScreenAttachFromFile::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_WINDOW_WILL_ACTIVATE:
		{
			listPhotos[tabList->GetActivePageIndex()]->RebuildList();
		}
		break;
	case EEI_WINDOW_SET_MAIN:
	case EEI_WINDOW_DID_ACTIVATE:
		{
			listPhotos[tabList->GetActivePageIndex()]->ActivateFocus();
		}
		break;
	case EEI_WINDOW_LOST_MAIN:
	case EEI_WINDOW_WILL_DEACTIVATE:
		{
			listPhotos[tabList->GetActivePageIndex()]->DeactivateFocus();
		}
		break;
	case EEI_TAB_PAGE_SWITCHED:
		{
			int32 oldPage = 1;
			if (tabList->GetActivePageIndex() == 1)
			{
				oldPage = 0;
			}
			if (listPhotos[oldPage])
			{
				listPhotos[oldPage]->DeactivateFocus();
			}
			listPhotos[tabList->GetActivePageIndex()]->RebuildList();
			listPhotos[tabList->GetActivePageIndex()]->ActivateFocus();
		}
		break;
	}
	return BaseScreen::OnEvent(eventID, pData);
}




GUIControl* ScreenAttachFromFile::ItemsListCreateListItem(GUIItemsList *forList)
{
	GUIMenuItem *pItem = new GUIMenuItem(EDT_PHOTO_ITEM);
	GUIPhotoImage *img = new GUIPhotoImage(IDB_PHOTO_LOADING_BAR, IDB_BAD_PHOTO, surfPool, pManager, pItem);
	img->SetID(ECIDS_TEXT_ID + 2);
	GUILayoutBox *lay = new GUILayoutBox(true, pItem);
	GUIText *text = new GUIText((char16*)L" ", lay);
	text->SetID(ECIDS_TEXT_ID);
	text->SetTextMargins(0, GUISystem::Instance()->GetSkin()->GetSprite(EDT_PHOTO_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() >> 3, 0, 0);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_BOTTOM);
	text->SetDrawType(EDT_POPUP_UNSELECTED_TEXT);
	text = new GUIText((char16*)L" ", lay);
	text->SetID(ECIDS_TEXT_ID + 1);
	text->SetTextMargins(0, GUISystem::Instance()->GetSkin()->GetSprite(EDT_PHOTO_ITEM, GUISkinLocal::ESSH3_CENTER)->GetHeight() >> 3, 0, 0);
	text->SetTextAlign(Font::EAP_LEFT | Font::EAP_VCENTER);
	text->SetDrawType(EDT_POPUP_UNSELECTED_TEXT);
	return pItem;
}

bool ScreenAttachFromFile::ItemsListIsOwnerValid( GUIItemsList *forList, void *owner )
{
	PhotoLibrary *pLib = pManager->GetPhotoLib();
	const LibPhotoItem *pi;
	if (forList == listPhotos[0])
	{
		pi = pLib->GetFirstPhoto(EPST_PHONE);
	}
	else
	{
		pi = pLib->GetFirstPhoto(EPST_CARD);
	}
	while (pi)
	{
		if (pi == owner)
		{
			return true;
		}
		pi = pLib->GetNextPhoto();
	}
	return false;
}

void *ScreenAttachFromFile::ItemsListGetFirstOwner(GUIItemsList *forList)
{
	if (forList == listPhotos[0])
	{
		return (void*)pManager->GetPhotoLib()->GetFirstPhoto(EPST_PHONE);
	}
	else
	{
		return (void*)pManager->GetPhotoLib()->GetFirstPhoto(EPST_CARD);
	}
}

void *ScreenAttachFromFile::ItemsListGetNextOwner(GUIItemsList *forList)
{
	return (void*)pManager->GetPhotoLib()->GetNextPhoto();
}

void ScreenAttachFromFile::ItemsListTuneItemByOwner( GUIItemsList *forList, GUIControl *item, void *owner )
{
	GUIText *t = (GUIText*)(item->GetByID(ECIDS_TEXT_ID));
	t->SetText(((LibPhotoItem*)owner)->GetName());
	
	t = (GUIText*)(item->GetByID(ECIDS_TEXT_ID + 1));
	char16 dateText[100];
	char16 timeMod[3];
	int32 hr = ((LibPhotoItem*)owner)->GetDate()->hour;
	if (hr < 12)
	{
		if (hr == 0)
		{
			hr = 12;
		}
		Utils::WSPrintf(timeMod, 3 * 2, (char16*)L"AM");
	}
	else
	{
		hr = hr - 12;
		if (hr == 0)
		{
			hr = 12;
		}

		Utils::WSPrintf(timeMod, 3 * 2, (char16*)L"AM");
	}
	Utils::WSPrintf(dateText, 100 * 2, pManager->GetStringWrapper()->GetStringText(IDS_DATE_FORMAT)
		, ((LibPhotoItem*)owner)->GetDate()->month
		, ((LibPhotoItem*)owner)->GetDate()->day
		, ((LibPhotoItem*)owner)->GetDate()->year
		, hr
		, ((LibPhotoItem*)owner)->GetDate()->minute
		, timeMod);
	t->SetText(dateText);

	GUIPhotoImage *pi = (GUIPhotoImage*)(item->GetByID(ECIDS_TEXT_ID + 2));
	pi->SetPhotoItem((LibPhotoItem*) owner);

}

void ScreenAttachFromFile::ItemsListCorrectItemByOwner( GUIItemsList *forList, GUIControl *item, void *owner )
{
	ItemsListTuneItemByOwner(forList, item, owner);
}


