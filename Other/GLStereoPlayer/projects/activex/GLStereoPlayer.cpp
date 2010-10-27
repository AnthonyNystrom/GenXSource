//-----------------------------------------------------------------------------
// GLStereoPlayer.cpp : Implementation of CGLStereoPlayerApp and DLL registration.
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#include "stdafx.h"
#include "GLStereoPlayer.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


CGLStereoPlayerApp NEAR theApp;

const GUID CDECL BASED_CODE _tlid =
        { 0x6e4d5a3, 0x70df, 0x4462, { 0xbf, 0xeb, 0x63, 0x93, 0x2e, 0x2b, 0xd7, 0x9a } };
const WORD _wVerMajor = 1;
const WORD _wVerMinor = 0;


////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerApp::InitInstance - DLL initialization

BOOL CGLStereoPlayerApp::InitInstance()
{
    BOOL bInit = COleControlModule::InitInstance();

    if (bInit)
    {
        // Load a localizing module
        char path[_MAX_PATH], drive[_MAX_DRIVE], dir[_MAX_DIR];
        char fname[_MAX_FNAME], ext[_MAX_EXT], dllPath[_MAX_EXT];
        GetModuleFileName(GetModuleHandle("GLSPCtrl.ocx"), path, _MAX_PATH);
        _splitpath(path, drive, dir, fname, ext);
        sprintf(dllPath, "%s%s%s", drive, dir, "localize.dll");
        m_hLangDLL = LoadLibrary(dllPath);
        if (m_hLangDLL)
            AfxSetResourceHandle(m_hLangDLL);
    }

    return bInit;
}


////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerApp::ExitInstance - DLL termination

int CGLStereoPlayerApp::ExitInstance()
{
    if (m_hLangDLL)
        FreeLibrary(m_hLangDLL);

    return COleControlModule::ExitInstance();
}


/////////////////////////////////////////////////////////////////////////////
// DllRegisterServer - Adds entries to the system registry

STDAPI DllRegisterServer(void)
{
    AFX_MANAGE_STATE(_afxModuleAddrThis);

    if (!AfxOleRegisterTypeLib(AfxGetInstanceHandle(), _tlid))
        return ResultFromScode(SELFREG_E_TYPELIB);

    if (!COleObjectFactoryEx::UpdateRegistryAll(TRUE))
        return ResultFromScode(SELFREG_E_CLASS);

    return NOERROR;
}


/////////////////////////////////////////////////////////////////////////////
// DllUnregisterServer - Removes entries from the system registry

STDAPI DllUnregisterServer(void)
{
    AFX_MANAGE_STATE(_afxModuleAddrThis);

    if (!AfxOleUnregisterTypeLib(_tlid, _wVerMajor, _wVerMinor))
        return ResultFromScode(SELFREG_E_TYPELIB);

    if (!COleObjectFactoryEx::UpdateRegistryAll(FALSE))
        return ResultFromScode(SELFREG_E_CLASS);

    return NOERROR;
}
