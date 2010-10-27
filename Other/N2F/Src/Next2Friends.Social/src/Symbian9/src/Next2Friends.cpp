/*
============================================================================
Name        : Next2Friends.cpp
Author      : 
Version     :
Copyright   : Your copyright notice
Description : Application entry point
============================================================================
*/

// INCLUDE FILES
#include <eikstart.h>

#include "PlatformSymb.h"
#include "Next2Friends.h"
#include "Utils.h"
#include "ApplicationManager.h"

#include "graphres.h"
#include "stringres.h"


LOCAL_C CApaApplication* NewApplication()
{
#undef new
	return new BaseApplication;
#define new new((char8)0)
}

GLDEF_C TInt E32Main()
{
	return EikStart::RunApplication( NewApplication );
}


uint32 GetClsID()
{
	return 0xe2c9889d;
}

IApplicationCore* CreateApplication(Application * app)
{
	IApplicationCore * n2f = new Next2Friends(app);
	return n2f;
}

bool ReleaseApplication(IApplicationCore * appCore)
{
	delete(appCore);
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
	if (isLoaded)
	{
		pAppManager->Update();
		return;
	}
	else
	{
		pAppManager	= new ApplicationManager(this);
		isLoaded = true;
	}
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

//////////////////////////////////////////////////////////////////////////
//ApplicationData

//void ApplicationData::SetDefaults()
//{
//}

