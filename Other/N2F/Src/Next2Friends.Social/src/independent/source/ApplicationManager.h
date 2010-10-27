#ifndef _APPLICATION_MANAGER_H_
#define _APPLICATION_MANAGER_H_


#include "BaseTypes.h"
#include "Next2Friends.h"
#include "GUIData.h"
#include "N2FData.h"
#include "N2FMessageOwner.h"
#include "EncoderObject.h"
#include "PhotoLibrary.h"
#include "Server.h"

class Application;
class Next2Friends;

class GUISystem;
class ResourceSystem;

class BaseScreen;

class Sprite;

class GUIHeader;
class GUIFooter;
class GUIPopUp;
class GUIAlert;
class GUILockWindow;

class StringWrapper;

class VList;


class N2FMessage;
class N2FDraftsManager;
class N2FOutboxManager;
class N2FInboxManager;
class N2FNewsManager;

class Server;
class MemberService;
class AskService;
class DashboardService;
class SnapUpService;


// enum eUserGloblaEvent
// {
// 	EUGE_FIRST					=	EGE_USER,
// 	EUGE_SELECT_AVATAR		
// };

struct ApplicationData; 



enum eScreenNames
{
	ESN_ROOT					= -1,
	ESN_FIRST					= ECIDS_FIRST_SCREEN_ID,

	ESN_TITLE					= ESN_FIRST,
	ESN_MAIN_MENU,
	ESN_GO,
	ESN_CREDENTIALS,
	ESN_REMIND_PASSWORD,
	ESN_ASK,
	ESN_TEMPLATES,
	ESN_DRAFTS,
	ESN_ATTACH_FROM_FILE,
	ESN_VIEW_PHOTO,
	ESN_CAMERA,
	ESN_OUTBOX,
	ESN_INBOX,
	ESN_DASHBOARD,
	ESN_SEND_SELECTION,
	ESN_DASHBOARD_READ,
	ESN_RESPONSE,
	ESN_WRITE_COMMENT,
	ESN_CHANGE_STATUS,
	ESN_SNAP_UP,


	ESN_LAST,
	ESN_COUNT = ESN_LAST - ESN_FIRST
};

#define SIMULATOR_WAIT_COUNTER		5

class ApplicationManager : public N2FMessageOwner, public ServerListener
{
public:
	ApplicationManager(Next2Friends *pN2F);
	virtual ~ApplicationManager(void);

	void			Update();
	void			Render();
	void			OnResume();

	void OnChar(char16 ch);

	void CloseApplication();

	void OnApplicationLoaded();

	Next2Friends	*GetCore()
	{
		return pCore;
	};

	Server			*GetServer()
	{
		return pServer;
	};

	MemberService	*GetMemberService()
	{
		return pMemberService;
	};

	AskService		*GetAskService()
	{
		return pAskService;
	};
	
	DashboardService	*GetDashboardService()
	{
		return pDashboardService;
	};

	SnapUpService	*GetSnapUpService()
	{
		return pSnapUpService;
	}

	void			SetNetError(bool isError)
	{
		isNetError = isError;
	}
	bool			IsNetError()
	{
		return isNetError;
	}
	bool			IsUserLoggedIn();

	ResourceSystem	*GetGraphicRes()
	{
		return pCore->GetGraphicResources();
	};
	ResourceSystem	*GetStringRes()
	{
		return pCore->GetStringResources();
	};

	GUISystem		*GetGUISystem()
	{
		return guiSystem;
	};

	Sprite			*GetBackSprite()
	{
		return backSprite;
	};

	GUIHeader		*GetHeader()
	{
		return pHeader;
	};

	GUIFooter		*GetFooter()
	{
		return pFooter;
	};

	GUIPopUp		*GetPopUp()
	{
		return pPopUp;
	};

	GUIAlert		*GetAlert()
	{
		return pAlert;
	};

	GUILockWindow		*GetLockWindow()
	{
		return pLockWindow;
	};

	StringWrapper	*GetStringWrapper()
	{
		return strWrapper;
	};

	VList			*GetTemplates()
	{
		return textTemplates;
	};

	BaseScreen		*GetScreen(eScreenNames screen)
	{
		return pWindows[screen - ESN_FIRST];
	};

	N2FDraftsManager *GetDraftsManager()
	{
		return draftsManager;
	};

	N2FOutboxManager *GetOutboxManager()
	{
		return outboxManager;
	};
	N2FNewsManager *GetNewsManager()
	{
		return newsManager;
	};
	N2FInboxManager *GetInboxManager()
	{
		return inboxManager;
	};

	EncoderObject	*GetEncoder()
	{
		return encoder;
	};

	PhotoLibrary	*GetPhotoLib()
	{
		return photoLib;
	}

	const LibPhotoItem	*GetWorkingPhoto()
	{
		return currentPhoto;
	}

	void			SetWorkingPhoto(LibPhotoItem *photoItem);

	N2FMessage		*GetWorkingMessage()
	{
		return currentMessage;
	}

	void			SetWorkingMesage(N2FMessage *workingMessage);




	void				CreateScreens(int32 screenNum);

	void				ChangeWindow(int32 id, bool isBack, int32 backToID = -1);

	N2FMessage			*GetFreeMessage();

	//ApplicationData	*	GetApplicationData();

	virtual void OnServerError();

	void ShowOutbox(bool show);
	void ShowInbox(bool show);
	void ShowDashboard(bool show);


	bool NeedToShowErrorAlert(bool needShow)
	{
		bool old = needToShowErrorAlert;
		needToShowErrorAlert = needShow;
		return old;
	}


protected:
	virtual void OnMessageChanged(N2FMessage *pMsg);
	virtual void OnMessageWillSwitchOwner(N2FMessage *pMsg);
	virtual void OnMessageAdded(N2FMessage *pMsg);



private:

	void				LoadTemplates();
	void				SwitchWindow(int32 toID, bool isBack);

	StringWrapper	* strWrapper;

	void			InitSkin();

	Next2Friends	*pCore;
	GUISystem		*guiSystem;

	Sprite			*backSprite;
	BaseScreen		*pWindows[ESN_COUNT];

	GUIHeader		*pHeader;
	GUIFooter		*pFooter;

	GUIPopUp		*pPopUp;

	GUIAlert		*pAlert;
	GUILockWindow	*pLockWindow;

	int32			switchToID;
	bool			switchIsBack;
	int32			switchBackID;

	VList			*textTemplates;

	N2FDraftsManager	*draftsManager;
	N2FOutboxManager	*outboxManager;
	N2FNewsManager		*newsManager;
	N2FInboxManager		*inboxManager;

	EncoderObject		*encoder;
	PhotoLibrary		*photoLib;


//--------   working data
	LibPhotoItem	*currentPhoto;
	N2FMessage	*currentMessage;
//-----------------------

	Server				*pServer;
	MemberService		*pMemberService;
	AskService			*pAskService;
	DashboardService	*pDashboardService;
	SnapUpService		*pSnapUpService;


	bool isNetError;
	bool needToShowErrorAlert;
	



//***** UI STATES *****
	int32			currentActiveWindow;
//***

#ifdef AEE_SIMULATOR
	int32 simulatorCounter;
#endif

	bool isReadyToDraw;
};

#endif//_APPLICATION_MANAGER_H_