#include "windows.h"
#include "Next2Friends.h"
#include "Utils.h"
#include "ApplicationManager.h"

#include "..\n2f.bid"
#include "graphres.h"
#include "stringres.h"
#include <aygshell.h>

uint32 GetClsID()
{
	return AEECLSID_N2F;
}

IApplicationCore* CreateApplication(Application * app)
{
	IApplicationCore * n2f = new Next2Friends(app);
#ifdef FULL_KEYBOARD_ENTER
	SHSetImeMode(NULL, SHIME_MODE_SPELL );
#else
	SHSetImeMode(NULL, SHIME_MODE_NUMBERS );
#endif
	return n2f;
}

bool ReleaseApplication(IApplicationCore * appCore)
{
	SAFE_DELETE(appCore);
	return true;
}

uint16 GetVersion()
{
	return 1;
}

void InitGraphics( uint32 depth, Application* app )
{
	app->InitGraphicSystem(depth);
	app->GetGraphicsSystem()->InitBltModes();
}

Next2Friends::Next2Friends(Application * app)
:isLoaded(false)
{
	this->app		= app;
	grResSys		= app->CreateResourceSystem();
	grResSys->Open(GRAPHRES_RES_FILE);

	strResSys		= app->CreateResourceSystem();
	strResSys->Open(STRINGRES_RES_FILE);

	applicationData = new ApplicationData();
	
	GetApplication()->ClearKeys();
}

Next2Friends::~Next2Friends(void)
{
	SaveApplicationData();
	SAFE_DELETE(applicationData);

	SAFE_DELETE(pAppManager);


	SAFE_DELETE(applicationData);
	
	SAFE_RELEASE(grResSys);
	SAFE_RELEASE(strResSys);
}

void Next2Friends::Update()
{
	if (!isLoaded)
	{
		pAppManager	= new ApplicationManager(this);
		isLoaded = true;
	}
	pAppManager->Update();
}

void Next2Friends::Render()
{
	pAppManager->Render();
}

void Next2Friends::SaveApplicationData()
{
	Utils::StorageSetData(GetClsID(), GetVersion(), (void *)applicationData, sizeof(ApplicationData));
}

void Next2Friends::LoadApplicationData()
{
	Utils::StorageGetData(GetClsID(), GetVersion(), (void *)applicationData, sizeof(ApplicationData));

//#ifndef RESET_STORAGE
//	if(Utils::WStrLen(applicationData->login)	==	0 && applicationData->needDisclaimer)
//#endif
//	{
//		applicationData->SetDefaults();
//	}
//
//	if(		applicationData->ShowTutorial(ESN_LOGIN) 
//		&&	Utils::WStrLen(applicationData->login))
//	{
//		applicationData->SetTutorial(ESN_LOGIN, false);
//	}
}

void Next2Friends::OnResume()
{
	if(pAppManager)
	{
		pAppManager->OnResume();
	}
}


void Next2Friends::OnChar( char16 ch )
{
	if (pAppManager)
	{
		pAppManager->OnChar(ch);
	}
}
//////////////////////////////////////////////////////////////////////////
//ApplicationData

//void ApplicationData::SetDefaults()
//{
//}

