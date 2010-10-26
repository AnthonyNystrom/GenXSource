#include "StdAfx.h"
#include "ProteinVista.h"
#include "ProteinVistaView.h"
#include "pdb.h"
#include "pdbInst.h"
#include "Interface.h"


CProteinVistaApp*  appInstance;
CProteinVistaApp* GetMainApp()
{
	if(appInstance==NULL)
	{
		appInstance = new CProteinVistaApp();
	}
	return appInstance;
}
CProteinVistaView* GetMainActiveView()
{
	return GetMainApp()->GetActiveProteinVistaView();
}
void RemoveMainApp()
{
	if(appInstance != NULL)
	{
		SAFE_DELETE(appInstance);
		appInstance =NULL;
	}
} 
HWND GetMainHwnd()
{
	return GetMainApp()->m_CanvsHandle;
}

CProteinVistaRenderer * GetProteinVistaRenderer()
{
	CProteinVistaView *pView = GetMainActiveView();
	if ( pView )
	{
		return pView->GetRender();
	}
	return NULL;
}
void MessagePump()
{
	while(1)
	{
		MSG message;
		if ( ::PeekMessage(&message, NULL, 0,0, PM_REMOVE) )
		{

			::TranslateMessage(&message);
			::DispatchMessage(&message);
		}
		else
			break;
	}
}


void PumpMessage()
{
	MessagePump();
}


