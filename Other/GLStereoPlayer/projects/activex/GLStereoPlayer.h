//-----------------------------------------------------------------------------
// GLStereoPlayer.h : Main header file for GLSTEREOPLAYER.DLL
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#if !defined( __AFXCTL_H__ )
    #error include 'afxctl.h' before including this file
#endif

#include "resource.h"       // main symbols

/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerApp : See GLStereoPlayer.cpp for implementation.

class CGLStereoPlayerApp : public COleControlModule
{
public:
    BOOL InitInstance();
    int ExitInstance();

private:
    HINSTANCE m_hLangDLL;
};

extern const GUID CDECL _tlid;
extern const WORD _wVerMajor;
extern const WORD _wVerMinor;
