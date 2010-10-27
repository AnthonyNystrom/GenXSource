// uidevtestppc.cpp : main source file for uidevtestppc.exe
//

#include "stdafx.h"

#ifdef WIN32_PLATFORM_PSPC
#include "resourceppc.h"
#else
#include "resourcesp.h"
#endif

#include "UidevtestppcView.h"
#include "AboutDlg.h"
#include "UidevtestppcFrame.h"

#include <controllershost.h>

CAppModule _Module;

int WINAPI _tWinMain(HINSTANCE hInstance, HINSTANCE /*hPrevInstance*/, LPTSTR lpstrCmdLine, int nCmdShow)
{
	HRESULT hRes = CUidevtestppcFrame::ActivatePreviousInstance(hInstance, lpstrCmdLine);

	if(FAILED(hRes) || S_FALSE == hRes)
	{
		return hRes;
	}

	hRes = ::CoInitializeEx(NULL, COINIT_MULTITHREADED);
	ATLASSERT(SUCCEEDED(hRes));


	AtlInitCommonControls(ICC_DATE_CLASSES);
	SHInitExtraControls();

	ControllersHost::GetInstance()->InitializeHost();

	hRes = _Module.Init(NULL, hInstance);
	ATLASSERT(SUCCEEDED(hRes));

	int nRet = CUidevtestppcFrame::AppRun(lpstrCmdLine, nCmdShow);

	_Module.Term();

	ControllersHost::DeleteInstance();

	::CoUninitialize();

	return nRet;
}

