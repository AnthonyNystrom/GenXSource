#pragma once

// ArMaX.h : main header file for ArMaX.DLL

#if !defined( __AFXCTL_H__ )
#error include 'afxctl.h' before including this file
#endif

#include "resource.h"       // main symbols


// CArMaXApp : See ArMaX.cpp for implementation.

class CArMaXApp : public COleControlModule
{
public:
	BOOL InitInstance();
	int ExitInstance();
};

extern const GUID CDECL _tlid;
extern const WORD _wVerMajor;
extern const WORD _wVerMinor;

extern float gAmbient[4];
extern float gDiffuse[4];
extern float gEmission[4];
extern float gSpecular[4];

extern float gShininnes;
