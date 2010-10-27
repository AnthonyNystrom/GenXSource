#include "stdafx.h"

#include <controllershost.h>

ControllersHost* ControllersHost::iHostInstance = NULL;


N2FCORE_API ControllersHost::ControllersHost()
{
	iConfigController = NULL;
	iWSController = NULL;
}

N2FCORE_API ControllersHost::~ControllersHost()
{
	if ( NULL != iWSController )
		delete iWSController;

	if ( NULL != iConfigController )
		delete iConfigController;
}

N2FCORE_API  ControllersHost* ControllersHost::CreateInstance()
{
	ControllersHost::DeleteInstance();

	iHostInstance = new ControllersHost();
	ATLASSERT( NULL != iHostInstance );

	return iHostInstance;
}

N2FCORE_API  ControllersHost* ControllersHost::GetInstance()
{
	if ( NULL == iHostInstance )
		ControllersHost::CreateInstance();

	return iHostInstance;
}

N2FCORE_API  void ControllersHost::DeleteInstance()
{
	if ( NULL != iHostInstance )
		delete iHostInstance;

	iHostInstance = NULL;
}

N2FCORE_API bool ControllersHost::InitializeHost()
{
	bool result = true;

	result = result & InitSettingsController();
	if ( result )
		result = result & InitConfigController();

	result = result & InitWSController();
	

	return result;
}

N2FCORE_API bool ControllersHost::InitConfigController()
{
	iConfigController = new ControllerConfig();

	ATLASSERT( (NULL != iConfigController)&&(NULL != iSettingsController) );

	if ( NULL == iConfigController || NULL == iSettingsController )
		return false;

	ConfigurationStoragesInitList listInit;

	CString appPath;
	ControllerUtil::GetModuleFolder( appPath );

	CString skinPath, stringsPath, colorsPath;
	iSettingsController->GetClientConfigPathes( skinPath, stringsPath, colorsPath );


	listInit.Add( ECSTGraphics, appPath + skinPath );
	listInit.Add( ECSTStrings, appPath + stringsPath );
	listInit.Add( ECSTColors, appPath + colorsPath );

	bool isIntialized = iConfigController->Initialize( listInit );
	if ( false == isIntialized )
	{
		ASSERT(FALSE);
		delete iConfigController;
		iConfigController = NULL;
	}

	return isIntialized;
}

N2FCORE_API bool ControllersHost::InitWSController()
{
	iWSController = new ControllerWebServices();

	if ( NULL == iWSController )
		return false;

	TWSCutsomEPList listCustomEndPoints;

	bool isInitialized = iWSController->InitializeController( listCustomEndPoints );
	if ( false == isInitialized )
	{
		ASSERT(FALSE);
		delete iWSController;
		iWSController = NULL;
	}

	return isInitialized;
}

N2FCORE_API bool ControllersHost::InitSettingsController()
{
	iSettingsController = new ControllerSettings();

	if ( NULL == iSettingsController )
		return false;

	bool isInitialized = iSettingsController->Initialize();
	return isInitialized;
}

N2FCORE_API ControllerConfig* ControllersHost::ConfigController()
{
	ASSERT( NULL != iConfigController );
	return iConfigController;
}

N2FCORE_API ControllerWebServices* ControllersHost::WebServicesController()
{
	ASSERT( NULL != iWSController );
	return iWSController;
}

N2FCORE_API ControllerSettings* ControllersHost::SettingsController()
{
	ASSERT( NULL != iSettingsController );
	return iSettingsController;
}
