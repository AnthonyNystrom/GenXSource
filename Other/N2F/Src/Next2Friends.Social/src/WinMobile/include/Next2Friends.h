#ifndef _NEXT_2_FRIENDS_H_
#define _NEXT_2_FRIENDS_H_

#include "IApplicationCore.h"
#include "Application.h"
#include "ResourceSystem.h"
#include "Graphics.h"
#include "N2FData.h"

//#include "Server.h"
//#include "Storage.h"

//#include "ApplicationManager.h"


uint16 GetVersion();
void InitGraphics(uint32 depth, Application* app);


struct ApplicationData
{
	ApplicationData()
	{
		isValid	=	false;

	}

	bool isValid;

	char16	login[ENP_MAX_LOGIN_SIZE];
	char16	password[ENP_MAX_PASSWORD_SIZE];
	char16	remindEmail[ENP_MAX_EMAIL_SIZE];
	char16	remindPhoneNum[ENP_MAX_PHONE_NUMBER_SIZE];

};

class ApplicationManager;

class Next2Friends : public IApplicationCore
{
private:
	Application			*app;
	ResourceSystem		*grResSys;
	ResourceSystem		*strResSys;
	ApplicationManager	*pAppManager;

	bool isLoaded;


	ApplicationData	*	applicationData;

public:
	Next2Friends(Application * app);
	virtual ~Next2Friends(void);

	ApplicationData		*GetApplicationData()	
	{	
		return applicationData;
	};

	
	ResourceSystem		*GetGraphicResources()
	{
		return grResSys;
	};

	ResourceSystem		*GetStringResources()
	{
		return strResSys;
	};

	ApplicationData		*GetAppData()
	{
		return applicationData;
	}


	virtual void Update();
	virtual void Render();

	virtual void OnResume();

	virtual void OnSuspend()
	{ }

	virtual void OnChar(char16 ch);

	void SaveApplicationData();
	void LoadApplicationData();
};
#endif //_NEXT_2_FRIENDS_H_














