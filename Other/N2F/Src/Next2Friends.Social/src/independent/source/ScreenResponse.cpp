#include "ScreenResponse.h"
#include "GUIImage.h"
#include "Graphics.h"
#include "stringres.h"
#include "GUIHeader.h"
#include "GUIFooter.h"
#include "GUIListView.h"
#include "GUIMenuItem.h"
#include "GUIText.h"
#include "GUIItemsList.h"
#include "GUIMultiString.h"
#include "GUIPhotoImage.h"

#include "GUILockWindow.h"

#include "SurfacesPool.h"


#include "ScreenAsk.h"
#include "VList.h"

#include "N2FData.h"
#include "N2FNewsManager.h"
#include "N2FMessage.h"

#include "SkinProperties.h"

#include "AskService.h"
#include "AskResponse.h"
#include "AskComment.h"

ScreenResponse::ScreenResponse( const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID )
:BaseScreen(newRect, pAppMng, screenID)
{

	AddListener(EEI_WINDOW_WILL_ACTIVATE, this);
	AddListener(EEI_WINDOW_DID_ACTIVATE, this);
	AddListener(EEI_WINDOW_WILL_DEACTIVATE, this);
	AddListener(EEI_SOFT1_PRESSED, this);
	AddListener(EEI_LOCK_WINDOW_CANCELED, this);

	uint16 iw = GetApplication()->GetGraphicsSystem()->GetWidth();
	iw -= iw / 2;
	//iw += iw / 4;
	uint16 ih = iw * 3 / 4;
	surfPool = new SurfacesPool(1, iw, ih);

	SetStretch(ECS_STRETCH);

	list = new GUIListView(this);


	uint8 plt;
	int32 h = GUISystem::Instance()->GetSkin()->GetFont(EDT_UNSELECTED_TEXT, plt)->GetHeight();
	questionName = new GUIMenuItem(EDT_SELECTED_ITEM, list);
	questionName->SetID(0);
	GUIText *tx = new GUIText((char16*)L"", questionName);
	tx->SetDrawType(EDT_UNSELECTED_TEXT);
	tx->SetID(ECIDS_RESPONSE_ITEM_ID + EMI_QUESTION_TEXT);

	GUIPhotoImage *img = new GUIPhotoImage(IDB_PHOTO_LOADING_BAR, IDB_BAD_PHOTO, surfPool, pManager, list);
	img->SetControlRect(ControlRect(0, 0, iw, ih));
	img->SetAlignType(GUIControl::EA_HCENTRE|GUIControl::EA_VCENTRE);
	img->SetID(ECIDS_RESPONSE_ITEM_ID + EMI_PHOTO);
	img->NeedUnload(false);



	tx = new GUIText((char16*)L"", list);
	tx->SetDrawType(EDT_UNSELECTED_TEXT);
	tx->SetID(ECIDS_RESPONSE_ITEM_ID + EMI_RESPONSE_1);
	tx->SetControlRect(ControlRect(0, 0, ControlRect::MIN_D, h, ControlRect::MAX_D, h));


	tx = new GUIText((char16*)L"", list);
	tx->SetDrawType(EDT_UNSELECTED_TEXT);
	tx->SetID(ECIDS_RESPONSE_ITEM_ID + EMI_RESPONSE_2);
	tx->SetControlRect(ControlRect(0, 0, ControlRect::MIN_D, h, ControlRect::MAX_D, h));



	packetsSent = 0;

	pLastFocus = questionName;

}

ScreenResponse::~ScreenResponse( void )
{
	GUIPhotoImage *img = (GUIPhotoImage *)GetByID(ECIDS_RESPONSE_ITEM_ID + EMI_PHOTO);
	img->CancelEncoding();
	img->SetPhotoItem(NULL);
	SAFE_DELETE(surfPool);
	SAFE_DELETE(resp);
	ClearComments();
	while (commentGUIItems.Size())
	{
		GUIMenuItem *mi = (GUIMenuItem *)(*commentGUIItems.Begin());
		commentGUIItems.Erase(commentGUIItems.Begin());
		SAFE_DELETE(mi);
	}
	SAFE_DELETE(currentComment);
}


//const char16	* ScreenOutbox::PopUpGetTextByIndex( int32 index, int32 &id, int32 prevId )
//{
//	if (pManager->GetOutboxManager()->MessagesCount())
//	{
//		switch(index)
//		{
//		case EPI_BACK:
//			{
//				id = EPI_BACK;
//				return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
//			}
//		case EPI_DELETE:
//			{
//				id = EPI_DELETE;
//				return pManager->GetStringWrapper()->GetStringText(IDS_DELETE);
//			}
//		}
//	}
//	else if (index == 0)
//	{
//		id = EPI_BACK;
//		return pManager->GetStringWrapper()->GetStringText(IDS_BACK);
//	}
//	return NULL;
//}
//
//void ScreenOutbox::PopUpOnItemSelected( int32 index, int32 id )
//{
//	switch(id)
//	{
//	case EPI_BACK:
//		{
//			BackToPrevSrceen();
//		}
//		break;
//	case EPI_DELETE:
//		{
//			((N2FMessage*)(list->GetFocusedOwner()))->SetOwner(pManager);
//		}
//		break;
//	}
//}



bool ScreenResponse::OnEvent( eEventID eventID, EventData *pData )
{
	switch(eventID)
	{
	case EEI_LOCK_WINDOW_CANCELED:
		{
			isCancel = true;
			SAFE_DELETE(IDs);
			BackToPrevSrceen();
			return true;
		}
		break;
	case EEI_SOFT1_PRESSED:
		{
			if (GUISystem::Instance()->GetFocus()->GetID() != 0)
			{
				CommentItem *c;
				VList::Iterator cit;
				GUIControl *contr = GUISystem::Instance()->GetFocus();
				for(cit = comments.Begin(); cit != comments.End(); cit++)
				{
					c = (CommentItem *)(*cit);
					if (c->currID == contr->GetID())
					{
						break;
					}
				}
				if (!currentComment)
				{
					currentComment = pManager->GetFreeMessage();
					currentComment->SetOwner(NULL);
				}
				currentComment->InitAsComment();
				currentComment->SetID(c->comm->AskQuestionID);
				currentComment->SetCommentID(c->currID);
				currentComment->SetText(c->comm->Text, ETT_COMMENT_TEXT);
				currentComment->SetText(c->comm->Nickname, ETT_COMMENT_NICKNAME);
				pManager->SetWorkingMesage(currentComment);
			}
			else
			{
				pManager->SetWorkingMesage(currentQuestion);
			}
			pManager->ChangeWindow(ESN_WRITE_COMMENT, false);
		}
		break;
	case EEI_WINDOW_WILL_ACTIVATE:
		{
			isCancel = false;
			N2FMessage *msg = pManager->GetWorkingMessage();
			if (msg && msg->GetType() == EMT_QUESTION_NAME)
			{
				currentQuestion = msg;
				GUIPhotoImage *img = (GUIPhotoImage *)GetByID(ECIDS_RESPONSE_ITEM_ID + EMI_PHOTO);
				img->CancelEncoding();
				img->SetPhotoItem(NULL);
				SAFE_DELETE(resp);
				ClearComments();
			}
		}
		break;
	case EEI_WINDOW_SET_MAIN:
	case EEI_WINDOW_DID_ACTIVATE:
		{

			//list->ActivateFocus();
			N2FMessage *msg = pManager->GetWorkingMessage();
			if (eventID == EEI_WINDOW_DID_ACTIVATE && msg && msg->GetType() == EMT_QUESTION_NAME)
			{
				packetsSent++;
				pManager->GetAskService()->GetResponse(this
					, pManager->GetCore()->GetApplicationData()->login
					, pManager->GetCore()->GetApplicationData()->password
					, pManager->GetWorkingMessage()->GetID());
				pManager->GetLockWindow()->GetText()->SetText(IDS_LOADING_RESP_DATA);
				pManager->GetLockWindow()->Show();
			}
		}
		break;
	case EEI_WINDOW_LOST_MAIN:
	case EEI_WINDOW_WILL_DEACTIVATE:
		{
			//list->DeactivateFocus();
			if (eventID == EEI_WINDOW_WILL_DEACTIVATE)
			{
				//pManager->SetWorkingMesage(NULL);
			}
		}
		break;
	}
	return BaseScreen::OnEvent(eventID, pData);
}






bool ScreenResponse::PopUpShouldOpen()
{
	return false;
}

void ScreenResponse::OnSuccess( int32 packetId )
{
	packetsSent--;
	if (packetsSent > 0)
	{
		return;
	}
	if (isCancel)
	{
		return;
	}
	switch(packetId)
	{
	case AskService::EPS_GETRESPONSE:
		{
			resp = pManager->GetAskService()->OnGetResponse();
			GUIText *tx = (GUIText *)GetByID(ECIDS_RESPONSE_ITEM_ID + EMI_QUESTION_TEXT);
			tx->SetText(resp->Question);
			GUIPhotoImage *img = (GUIPhotoImage *)GetByID(ECIDS_RESPONSE_ITEM_ID + EMI_PHOTO);
			img->SetPhotoBuffer((uint8*)resp->PhotoBase64Binary.pBuffer, resp->PhotoBase64Binary.length);

			switch(resp->ResponseType)
			{
			case EIT_QUESTION_YES_NO:
				{
					char16 temp[300];
					tx = (GUIText *)GetByID(ECIDS_RESPONSE_ITEM_ID + EMI_RESPONSE_1);
					Utils::WSPrintf(temp, 300 * 2, pManager->GetStringWrapper()->GetStringText(IDS_YES_D), resp->ResponseValues.pBuffer[0]);
					tx->SetText(temp);
					tx = (GUIText *)GetByID(ECIDS_RESPONSE_ITEM_ID + EMI_RESPONSE_2);
					Utils::WSPrintf(temp, 300 * 2, pManager->GetStringWrapper()->GetStringText(IDS_NO_D), resp->ResponseValues.pBuffer[1]);
					tx->SetText(temp);
				}
				break;
			case EIT_QUESTION_A_OR_B:
				{
					char16 temp[300];
					tx = (GUIText *)GetByID(ECIDS_RESPONSE_ITEM_ID + EMI_RESPONSE_1);
					Utils::WSPrintf(temp, 300 * 2, (char16*)L"%s: %d", resp->CustomResponses.ppStrings[0], resp->ResponseValues.pBuffer[0]);
					tx->SetText(temp);
					tx = (GUIText *)GetByID(ECIDS_RESPONSE_ITEM_ID + EMI_RESPONSE_2);
					Utils::WSPrintf(temp, 300 * 2, (char16*)L"%s: %d", resp->CustomResponses.ppStrings[1], resp->ResponseValues.pBuffer[1]);
					tx->SetText(temp);
				}
				break;
			case EIT_QUESTION_RATE:
				{
					char16 temp[300];
					tx = (GUIText *)GetByID(ECIDS_RESPONSE_ITEM_ID + EMI_RESPONSE_1);
					tx->SetText(IDS_AVERAGE);
					tx = (GUIText *)GetByID(ECIDS_RESPONSE_ITEM_ID + EMI_RESPONSE_2);
					int32 val = 0;
					int32 cnt = 0;
					for (int i = 0; i < resp->ResponseValues.length; i++)
					{
						val += (i + 1) * resp->ResponseValues.pBuffer[i];
						cnt += resp->ResponseValues.pBuffer[i];
					}
					if (val > 0)
					{
						Utils::WSPrintf(temp, 300 * 2, (char16*)L"%d.%.2d", val / cnt, (val * 100) / cnt - (val / cnt) * 100);
					}
					else
					{
						Utils::WSPrintf(temp, 300 * 2, pManager->GetStringWrapper()->GetStringText(IDS_NO_ANSWERS));
					}

					tx->SetText(temp);
				}
				break;
			}
			packetsSent++;
			pManager->GetAskService()->GetCommentIDs(this
				, pManager->GetCore()->GetApplicationData()->login
				, pManager->GetCore()->GetApplicationData()->password
				, pManager->GetWorkingMessage()->GetID()
				, 0);
			//pManager->GetLockWindow()->Hide();
		}
		break;
	case AskService::EPS_GETCOMMENTIDS:
		{
			IDs = pManager->GetAskService()->OnGetCommentIDs();
			currComment = 0;
			ReciveNextComment();
		}
		break;
	case AskService::EPS_GETCOMMENT:
		{
			AskComment *comment = pManager->GetAskService()->OnGetComment();
			CommentItem *itm = new CommentItem();
			itm->comm = comment;
			itm->currID = IDs->pBuffer[currComment];
			comments.PushBack(itm);
			currComment++;
			ReciveNextComment();
		}
		break;

	}
}

void ScreenResponse::OnFailed( int32 packetId, char16 *errorString )
{
	packetsSent--;
	if (packetsSent > 0)
	{
		return;
	}
	if (isCancel)
	{
		return;
	}
	pManager->GetLockWindow()->Hide();
	BackToPrevSrceen();
}

void ScreenResponse::ReciveNextComment()
{
	if (currComment >= IDs->length)
	{
		SAFE_DELETE(IDs);
		pManager->GetLockWindow()->Hide();
		RebuildCommentsList();
		return;
	}

	packetsSent++;
	pManager->GetAskService()->GetComment(this
		, pManager->GetCore()->GetApplicationData()->login
		, pManager->GetCore()->GetApplicationData()->password
		, IDs->pBuffer[currComment]);
	pManager->GetWorkingMessage()->SetCommentID(IDs->pBuffer[currComment]);

}

void ScreenResponse::ClearComments()
{
	while (comments.Size())
	{
		CommentItem *c = (CommentItem *)(*comments.Begin());
		SAFE_DELETE(c);
		comments.Erase(comments.Begin());
	}
	
	VList *cgl = list->GetListChilds();
	while (true)
	{
		VList::Iterator cit = cgl->Begin();
		GUIControl *mi = (GUIControl *)(*cit);
		do 
		{
			cit++;
			mi = (GUIControl *)(*cit);
		} while(mi->GetID() != ECIDS_RESPONSE_ITEM_ID + EMI_RESPONSE_2);
		cit++;
		if (cit == cgl->End())
		{
			break;
		}
		mi = (GUIControl *)(*cit);
		list->RemoveChild(mi);
		ReturnMenuItem((GUIMenuItem *)mi);
	}
	RebuildCommentsList();
}

void ScreenResponse::RebuildCommentsList()
{
	if (comments.Empty())
	{
		return;
	}
	pLastFocus = NULL;
	AddChildsFor(0, 0);
	if (!pLastFocus)
	{
		pLastFocus = questionName;
	}
	GUISystem::Instance()->SetFocus(pLastFocus);
}

GUIMenuItem * ScreenResponse::GetMenuItem()
{
	if (commentGUIItems.Size())
	{
		GUIMenuItem *mi = (GUIMenuItem *)(*commentGUIItems.Begin());
		commentGUIItems.Erase(commentGUIItems.Begin());
		return mi;
	}
	GUIMenuItem *mi = new GUIMenuItem(EDT_SELECTED_ITEM, NULL, ControlRect(), true);
	GUIText *tx = new GUIText((char16*)L"", mi);
	tx->SetTextAlign(Font::EAP_RIGHT | Font::EAP_VCENTER);
	tx->SetMargins(0, 3, 0, 0);
	tx->SetDrawType(EDT_POPUP_UNSELECTED_TEXT);
	tx->SetID(ECIDS_TEXT_ID + 1);
	tx = new GUIText((char16*)L"", mi);
	tx->SetTextAlign(Font::EAP_LEFT | Font::EAP_VCENTER);
	tx->SetDrawType(EDT_UNSELECTED_TEXT);
	tx->SetID(ECIDS_TEXT_ID);
	tx->SetMargins(3, 0, 0, 0);
    return mi;
}

void ScreenResponse::ReturnMenuItem( GUIMenuItem *mi )
{
	commentGUIItems.PushBack(mi);
}

void ScreenResponse::AddChildsFor( int32 commentID, int32 tab )
{
	VList::Iterator cit;
	for(cit = comments.Begin(); cit != comments.End(); cit++)
	{
		CommentItem *c = (CommentItem *)(*cit);
		if ( (c->comm->ID == commentID && c->comm->ID != c->currID)
			|| (commentID == 0 && c->comm->ID == c->currID) )
		{
			GUIMenuItem *mi = GetMenuItem();
			mi->SetMargins(tab * 4, 0, 0, 0);
			GUIText *tx = (GUIText*)(mi->GetByID(ECIDS_TEXT_ID));
			tx->SetText(c->comm->Text);
			tx = (GUIText*)(mi->GetByID(ECIDS_TEXT_ID + 1));
			tx->SetText(c->comm->Nickname);
			list->AddChild(mi);
			mi->SetID(c->currID);
			if (!pLastFocus)
			{
				pLastFocus = mi;
			}
			AddChildsFor(c->currID, tab + 1);
		}
	}
}








CommentItem::~CommentItem()
{
	SAFE_DELETE(comm);
}