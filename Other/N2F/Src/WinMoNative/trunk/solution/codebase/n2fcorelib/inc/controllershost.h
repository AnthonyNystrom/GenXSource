#pragma once

#include <controllerconfig.h>
#include <controllerwebservices.h>
#include <controllersettings.h>

class ControllersHost
{
private:
	N2FCORE_API ControllersHost();
	N2FCORE_API virtual ~ControllersHost();

public:

	N2FCORE_API static ControllersHost* CreateInstance();
	N2FCORE_API static ControllersHost* GetInstance();
	N2FCORE_API static void DeleteInstance();

	N2FCORE_API bool InitializeHost();

	N2FCORE_API ControllerConfig*	ConfigController();
	N2FCORE_API ControllerWebServices* WebServicesController();
	N2FCORE_API ControllerSettings*	SettingsController();

private:
	N2FCORE_API bool InitConfigController();
	N2FCORE_API bool InitWSController();
	N2FCORE_API bool InitSettingsController();


private:

	static ControllersHost	*iHostInstance;

	ControllerConfig		*iConfigController;
	ControllerWebServices	*iWSController;
	ControllerSettings		*iSettingsController;
};

