#include "ApplicationManager.h"
#include "Next2Friends.h"

#include "GUISystem.h"
#include "GUISkinLocal.h"

#include "graphres.h"
#include "stringres.h"

#include "ScreenTitle.h"
#include "ScreenMainMenu.h"
#include "ScreenGo.h"
#include "ScreenCredentials.h"
#include "ScreenRemindPassword.h"
#include "ScreenAsk.h"
#include "ScreenTemplates.h"
#include "ScreenDrafts.h"
#include "ScreenAttachFromFile.h"
#include "ScreenViewPhoto.h"
#include "ScreenCamera.h"
#include "ScreenOutbox.h"
#include "ScreenInbox.h"
#include "ScreenDashboard.h"
#include "ScreenSendSelection.h"
#include "ScreenDashboardRead.h"
#include "ScreenResponse.h"
#include "ScreenWriteComment.h"
#include "ScreenChangeStatus.h"
#include "ScreenSnapUp.h"

#include "GUIHeader.h"
#include "GUIFooter.h"
#include "GUIPopUp.h"
#include "GUIAlert.h"
#include "GUILockWindow.h"

#include "GUIMultiString.h"

#include "StringWrapper.h"

#include "N2FData.h"
#include "N2FMessage.h"
#include "N2FDraftsManager.h"
#include "N2FOutboxManager.h"
#include "N2FInboxManager.h"
#include "N2FNewsManager.h"
#include "LibPhotoItem.h"

#include "Server.h"
#include "MemberService.h"
#include "AskService.h"
#include "DashboardService.h"
#include "SnapUpService.h"


ApplicationManager::ApplicationManager(Next2Friends *pN2F)
: N2FMessageOwner(EOT_APP_MANAGER, NULL)
{

	isReadyToDraw = false;
	pCore = pN2F;

	pCore->LoadApplicationData();

	pServer = new Server(this);
	pMemberService = new MemberService(pServer);
	pAskService = new AskService(pServer);
	pDashboardService = new DashboardService(pServer);
	pSnapUpService = new SnapUpService(pServer);

// ***** PRECREATING *****

	for (int i = 0; i < ENP_MAX_N2F_MESSAGES; i++)
	{
		N2FMessage *msg = new N2FMessage();
		msg->SetOwner(this);
	}
	draftsManager = new N2FDraftsManager(this);
	outboxManager = new N2FOutboxManager(this);
	inboxManager = new N2FInboxManager(this);
	newsManager = new N2FNewsManager(this);


	guiSystem			= new GUISystem();
	guiSystem->SetStringSystem(pCore->GetStringResources());
	guiSystem->SetResourceSystem(pCore->GetGraphicResources());

	guiSystem->SetCycling(false);

	InitSkin();

	strWrapper			= new StringWrapper(pCore->GetStringResources());
	//pScriptLoader		= new GUIScriptLoader(this);

	backSprite			=	pCore->GetGraphicResources()->CreateSprite(IDB_BACK);


//***** CREATE TITLE SCREEN *****
	ControlRect screenRect(0, 0, GetApplication()->GetGraphicsSystem()->GetWidth(), GetApplication()->GetGraphicsSystem()->GetHeight());
	//ControlRect wndRect(0, 20, pApp->GetGraphicsSystem()->GetWidth(), pApp->GetGraphicsSystem()->GetHeight() - pTitle->GetRect().dy - pFooter->GetRect().dy);

	pWindows[ESN_TITLE - ESN_FIRST]					= new ScreenTitle(screenRect, this, ESN_TITLE);
	
//***

//Set first screen
	currentActiveWindow = -1;
	switchToID = -1;
 	ChangeWindow(ESN_TITLE, false);
	SwitchWindow(switchToID, switchIsBack);


#ifdef AEE_SIMULATOR
	simulatorCounter = 0;
#endif

	LoadTemplates();

	encoder = EncoderObject::Create();
	photoLib = PhotoLibrary::Create();
	currentPhoto = new LibPhotoItem();

}

ApplicationManager::~ApplicationManager(void)
{

	UTILS_TRACE(" + ScreenManager Destructor");

	SAFE_DELETE(pServer);
	SAFE_DELETE(pMemberService);
	SAFE_DELETE(pAskService);
	SAFE_DELETE(pDashboardService);
	SAFE_DELETE(pSnapUpService);

	SAFE_DELETE(encoder);
	SAFE_DELETE(currentPhoto);
	SAFE_DELETE(photoLib);
	UTILS_TRACE(" libs removed");
 

	SAFE_DELETE(pFooter);
	UTILS_TRACE(" footer removed");
	SAFE_DELETE(pHeader);
	UTILS_TRACE(" header removed");
	SAFE_DELETE(pPopUp);
	UTILS_TRACE(" popup removed");
	SAFE_DELETE(pAlert);
	UTILS_TRACE(" alert removed");
	SAFE_DELETE(pLockWindow);
	UTILS_TRACE(" lock window removed");



	
	SAFE_DELETE(draftsManager);
	SAFE_DELETE(outboxManager);
	SAFE_DELETE(inboxManager);
	SAFE_DELETE(newsManager);





	for (int i = 0; i < ESN_COUNT; i++)
	{
		UTILS_TRACE("remove screen %d", i);
		SAFE_DELETE(pWindows[i]);
		UTILS_TRACE("Done");
	}
	
	SAFE_RELEASE(backSprite);

	//SAFE_DELETE(pScriptLoader);

	while (textTemplates->Size())
	{
		char16 *tx= (char16*)(*(textTemplates->Begin()));
		textTemplates->Erase(textTemplates->Begin());
		SAFE_DELETE(tx);
	}
	SAFE_DELETE(textTemplates);

	SAFE_DELETE(guiSystem);
	SAFE_DELETE(strWrapper);
	UTILS_TRACE(" - ScreenManager Destructor");

}

void ApplicationManager::ChangeWindow(int32 id, bool isBack, int32 backToID/* = -1*/)
{
	switchToID = id;
	switchIsBack = isBack;
	switchBackID = backToID;
}

//void ApplicationManager::RestoreActive()
//{
//	//guiSystem->SetActiveControl(pWindows[currentActiveWindow]);
//}


void ApplicationManager::Update()
{
#ifdef AEE_SIMULATOR
	if (simulatorCounter < SIMULATOR_WAIT_COUNTER)
	{
		//if (simulatorCounter == 15)
		//{
		//	//pServer->CheckUserExists((char16*)L"test user", (char16*)L"test user", NULL);
		//}
		return;
	}
#endif
	//if (isNetError)
	//{
	//	if (GetApplication()->IsKeyUp(Application::EKC_SOFT1)
	//		 || GetApplication()->IsKeyUp(Application::EKC_SOFT2)
	//		 || GetApplication()->IsKeyUp(Application::EKC_CLR)
	//		 || GetApplication()->IsKeyUp(Application::EKC_SELECT))
	//	{
	//		CloseApplication();
	//	}
	//	
	//}

	pServer->Update();

	encoder->Update();

	if (switchToID >= 0)
	{
		SwitchWindow(switchToID, switchIsBack);
	}


	guiSystem->Update();

	OwnerUpdate();
	draftsManager->OwnerUpdate();
	outboxManager->OwnerUpdate();
	newsManager->OwnerUpdate();
	inboxManager->OwnerUpdate();

}

void ApplicationManager::Render()
{
#ifdef AEE_SIMULATOR
	if (simulatorCounter < SIMULATOR_WAIT_COUNTER)
	{
		simulatorCounter++;
		return;
	}
#endif

	guiSystem->Draw();
}

//ScreenBase * ApplicationManager::GetWindowByID(int32 id) const
//{
// 	return pWindows[id];
//}


void ApplicationManager::InitSkin()
{
	GUISkinLocal *pSkin = guiSystem->GetSkin();

	Font *pFont = GetGraphicRes()->CreateFont(IDB_FONT1, IDB_CHARTABLE);
	pSkin->SetFont(EDT_DEFAULT, pFont, EF1_BLACK);
	pSkin->SetFont(EDT_SELECTED_TEXT, pFont, EF1_WHITE);
	pSkin->SetFont(EDT_UNSELECTED_TEXT, pFont, EF1_BLACK);
	pSkin->SetFont(EDT_SELECTED_ITEM, pFont, EF1_WHITE);
	pSkin->SetFont(EDT_FOOTER_BUTTON_RIGHT, pFont, EF1_WHITE);
	pSkin->SetFont(EDT_FOOTER_BUTTON_LEFT, pFont, EF1_WHITE);
	pFont->Release();
	pFont = GetGraphicRes()->CreateFont(IDB_FONT_SMALL_BLACK, IDB_CHARTABLE);
	pSkin->SetFont(EDT_POPUP_UNSELECTED_TEXT, pFont, EFS_BLACK);
	pSkin->SetFont(EDT_POPUP_SELECTED_TEXT, pFont, EFS_WHITE);
	pFont->Release();
	pFont = GetGraphicRes()->CreateFont(IDB_FONT2_BLUE, IDB_CHARTABLE);
	pSkin->SetFont(EDT_CAPS_TEXT, pFont, 0);
	pFont->Release();
}

// void ScreenManager::DeinitSkin()
// {
// 
// }




void ApplicationManager::CreateScreens(int32 screenNum)
{
	if(!pHeader)
	{
		pHeader = new GUIHeader();
		//pHeader->ShowNetInidicator(GUIHeader::ENI_OUTBOX, true);
		//pHeader->ShowNetInidicator(GUIHeader::ENI_INBOX, true);
		//pHeader->ShowNetInidicator(GUIHeader::ENI_DASHBOARD, true);
	}
	if (!pFooter)
	{
		pFooter = new GUIFooter();
	}
	if (!pPopUp)
	{
		pPopUp = new GUIPopUp(GetApplication()->GetGraphicsSystem()->GetHeight()
			- pFooter->GetRect().dy
			+ (guiSystem->GetSkin()->GetSprite(EDT_POPUP_BORDER, GUISkinLocal::ESS9_BOTTOM_CENTER)->GetHeight() >> 1));
	}
	if (!pAlert)
	{
		pAlert = new GUIAlert();
	}
	if (!pLockWindow)
	{
		pLockWindow = new GUILockWindow();
	}
	//***** CREATE ALL SCREENS *****
	//ControlRect screenRect(0, 0, pApp->GetGraphicsSystem()->GetWidth(), pApp->GetGraphicsSystem()->GetHeight());
	ControlRect wndRect(0
		, pHeader->GetRect().dy
		, GetApplication()->GetGraphicsSystem()->GetWidth()
		, GetApplication()->GetGraphicsSystem()->GetHeight() - pHeader->GetRect().dy - pFooter->GetRect().dy);
	//ControlRect borderedRect(9, 20 + 7, pApp->GetGraphicsSystem()->GetWidth() - 9 - 8, pApp->GetGraphicsSystem()->GetHeight() - pTitle->GetRect().dy - pFooter->GetRect().dy - 7 - 7);
	//ControlRect notifRect(25, 45, pApp->GetGraphicsSystem()->GetWidth()-50, pApp->GetGraphicsSystem()->GetHeight() - pTitle->GetRect().dy - pFooter->GetRect().dy-50);

	//int32 x = pApp->GetGraphicsSystem()->GetWidth() - 9 - 8;
	//int32 y = pApp->GetGraphicsSystem()->GetHeight() - pTitle->GetRect().dy - pFooter->GetRect().dy - 7 - 7;

	UTILS_LOG(EDMP_DEBUG, "ScreenManager::CreateScreens: screenNum = %d", screenNum);

	switch(screenNum + ESN_FIRST)
	{
	case ESN_MAIN_MENU:
		{
			pWindows[screenNum]					= new ScreenMainMenu(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	case ESN_GO:
		{
			pWindows[screenNum]					= new ScreenGo(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	case ESN_CREDENTIALS:
		{
			pWindows[screenNum]					= new ScreenCredentials(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	case ESN_REMIND_PASSWORD:
		{
			pWindows[screenNum]					= new ScreenRemindPassword(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	case ESN_ASK:
		{
			pWindows[screenNum]					= new ScreenAsk(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	case ESN_TEMPLATES:
		{
			pWindows[screenNum]					= new ScreenTemplates(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	case ESN_DRAFTS:
		{
			pWindows[screenNum]					= new ScreenDrafts(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	case ESN_ATTACH_FROM_FILE:
		{
			pWindows[screenNum]					= new ScreenAttachFromFile(wndRect, this, screenNum + ESN_FIRST);
			photoLib->GetFirstPhoto(EPST_PHONE);//scan for photo files
		}
		break;
	case ESN_VIEW_PHOTO:
		{
			pWindows[screenNum]					= new ScreenViewPhoto(wndRect, this, screenNum + ESN_FIRST);
			photoLib->GetFirstPhoto(EPST_CARD);//scan for photo files
		}
		break;
	case ESN_CAMERA:
		{
			pWindows[screenNum]					= new ScreenCamera(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	case ESN_OUTBOX:
		{
			pWindows[screenNum]					= new ScreenOutbox(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	case ESN_INBOX:
		{
			pWindows[screenNum]					= new ScreenInbox(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	case ESN_DASHBOARD:
		{
			pWindows[screenNum]					= new ScreenDashboard(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	case ESN_SEND_SELECTION:
		{
			pWindows[screenNum]					= new ScreenSendSelection(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	case ESN_DASHBOARD_READ:
		{
			pWindows[screenNum]					= new ScreenDashboardRead(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	case ESN_RESPONSE:
		{
			pWindows[screenNum]					= new ScreenResponse(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	case ESN_WRITE_COMMENT:
		{
			pWindows[screenNum]					= new ScreenWriteComment(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	case ESN_CHANGE_STATUS:
		{
			pWindows[screenNum]					= new ScreenChangeStatus(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	case ESN_SNAP_UP:
		{
			pWindows[screenNum]					= new ScreenSnapUp(wndRect, this, screenNum + ESN_FIRST);
		}
		break;
	}

}


//bool ScreenManager::IsPopupWindow(int32 windowID)
//{
//// 	if(		ESN_NOTIFICATION	==	windowID
//// 		||	ESN_HELP_POPUP		==	windowID)
//// 	{
//// 		return	true;
//// 	}
//// 	return	false;
//
//	return (ESN_NOTIFICATION	==	windowID	||	ESN_HELP_POPUP		==	windowID);
//}

//ApplicationData	* ApplicationManager::GetApplicationData()
//{
//	return	((AmazingRace*)(GetApplication()->GetApplicationCore()))->GetApplicationData();
//}



//bool ScreenManager::IsPrevWindowPopup()
//{
//	return IsPopupWindow(prevWindow);
//}


void ApplicationManager::OnResume()
{
	//if (guiSystem)
	//{
	//	guiSystem->ForceInvalidate();
	//}
}

void ApplicationManager::SwitchWindow( int32 toID, bool isBack )
{
	GUISystem::Instance()->GetApp()->ClearKeys();

	int32 prevWindow = currentActiveWindow;

	currentActiveWindow = toID;


	if (prevWindow != -1)
	{
		eTransitionType tt = ETT_FROM_LEFT;
		if (!isBack)
		{
			tt = ETT_FROM_RIGHT;
			if (switchBackID != -1)
			{
				pWindows[currentActiveWindow - ESN_FIRST]->SetPrevScreen(switchBackID);
			}
			else
			{
				pWindows[currentActiveWindow - ESN_FIRST]->SetPrevScreen(prevWindow);
			}
		}
		if (prevWindow == ESN_TITLE)
		{
			tt = ETT_NONE;
		}
		//if (	IsPopupWindow(currentActiveWindow))
		//{
		//	guiSystem->FreezeBackground(Rect(0, 20, pApp->GetGraphicsSystem()->GetWidth(), pApp->GetGraphicsSystem()->GetHeight() - pTitle->GetRect().dy - pFooter->GetRect().dy));
		//}
		//else if (IsPopupWindow(prevWindow))
		//{
		//	guiSystem->UnfreezeBackground();
		//}
		//else
		{
			//if (isBack)
			//{
			//	tt = pWindows[prevWindow]->GetExitType();
			//}
			//if (tt == ETT_NONE)
			//{
			//	tt = pWindows[currentActiveWindow]->GetEnterType();
			//}
		}
		guiSystem->ChangeControl(pWindows[prevWindow - ESN_FIRST], pWindows[currentActiveWindow - ESN_FIRST], tt);
	}
	else
	{
		guiSystem->ChangeControl(NULL, pWindows[currentActiveWindow - ESN_FIRST]);
	}



	//if(GetPopup()->IsPopupOpened())
	//{
	//	GetPopup()->Close();
	//}

	switchToID = -1;
}

void ApplicationManager::CloseApplication()
{
	GetApplication()->CloseApplication();
}

void ApplicationManager::LoadTemplates()
{
	textTemplates = new VList(ENP_MAX_TEMPLATES);
	for (int i = 0; i < 6; i++)
	{
		const char16* text = strWrapper->GetStringText(IDS_TEMPLATE_1 + i);

		char16 *tt = new char16[Utils::WStrLen(text) + 1];
		Utils::WStrCpy(tt, text);
		textTemplates->PushBack(tt);
	}
}


void ApplicationManager::OnMessageChanged( N2FMessage *pMsg )
{

}

void ApplicationManager::OnMessageWillSwitchOwner( N2FMessage *pMsg )
{
	if (pMsg == currentMessage)
	{
		currentMessage = NULL;
	}
}

void ApplicationManager::OnMessageAdded( N2FMessage *pMsg )
{
	pMsg->Clear();
}

void ApplicationManager::SetWorkingPhoto( LibPhotoItem *photoItem )
{
	*currentPhoto = *photoItem;
}

void ApplicationManager::SetWorkingMesage( N2FMessage *workingMessage )
{
	currentMessage = workingMessage;
}

N2FMessage			* ApplicationManager::GetFreeMessage()
{
	VList::Iterator it = GetMessagesList()->Begin();
	while (it != GetMessagesList()->End())
	{
		N2FMessage *msg = (N2FMessage *)(*it);
		if (msg != currentMessage)
		{
			return msg;
		}

		it++;
	}
	return NULL;
}

void ApplicationManager::OnApplicationLoaded()
{
	ChangeWindow(ESN_CREDENTIALS, true);
	//ChangeWindow(ESN_RESPONSE, true);
	
}

void ApplicationManager::OnServerError()
{
	pLockWindow->Hide();
	ChangeWindow(ESN_MAIN_MENU, true);
	isNetError = true;
	needToShowErrorAlert = true;
}

bool ApplicationManager::IsUserLoggedIn()
{
	return !isNetError && pCore->GetApplicationData()->isValid;
}

void ApplicationManager::ShowOutbox( bool show )
{
	pHeader->ShowNetInidicator(GUIHeader::ENI_OUTBOX, show);
}

void ApplicationManager::ShowInbox( bool show )
{
	pHeader->ShowNetInidicator(GUIHeader::ENI_INBOX, show);
}

void ApplicationManager::ShowDashboard( bool show )
{
	pHeader->ShowNetInidicator(GUIHeader::ENI_DASHBOARD, show);
}

void ApplicationManager::OnChar( char16 ch )
{
	if (guiSystem)
	{
		guiSystem->OnChar(ch);
	}
}