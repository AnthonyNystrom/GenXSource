// supui-ppc.cpp : main source file for supui-ppc.exe
//

#include "stdafx.h"

#ifdef WIN32_PLATFORM_PSPC
#include "resourceppc.h"
#else
#include "resourcesp.h"
#endif


#include "SupuippcFrame.h"

#include <controllershost.h>
#include <windowsmanager.h>

CAppModule _Module;

int WINAPI _tWinMain(HINSTANCE hInstance, HINSTANCE /*hPrevInstance*/, LPTSTR lpstrCmdLine, int nCmdShow)
{
	HRESULT hRes = CSupuippcFrame::ActivatePreviousInstance(hInstance, lpstrCmdLine);

	if(FAILED(hRes) || S_FALSE == hRes)
	{
		return hRes;
	}

	hRes = ::CoInitializeEx(NULL, COINIT_MULTITHREADED);
	ATLASSERT(SUCCEEDED(hRes));

	AtlInitCommonControls(ICC_DATE_CLASSES);
	SHInitExtraControls();

	ControllersHost::GetInstance()->InitializeHost();
	WindowsManager::CreateInstance();

	hRes = _Module.Init(NULL, hInstance);
	ATLASSERT(SUCCEEDED(hRes));

	int nRet = CSupuippcFrame::AppRun(lpstrCmdLine, nCmdShow);

	WindowsManager::DeleteInstance();
	ControllersHost::DeleteInstance();

	_Module.Term();
	::CoUninitialize();

	return nRet;
}

