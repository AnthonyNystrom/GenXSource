// n2fcorelib.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include <windows.h>
#include <commctrl.h>

#include <imagehelper.h>

BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
	if ( DLL_PROCESS_ATTACH == ul_reason_for_call )
	{
		::CoInitializeEx( NULL, COINIT_MULTITHREADED );
	}
	else if ( DLL_PROCESS_DETACH == ul_reason_for_call )
	{
		ImageHelper::DeleteInstance();
		::CoUninitialize();
	}

    return TRUE;
}

