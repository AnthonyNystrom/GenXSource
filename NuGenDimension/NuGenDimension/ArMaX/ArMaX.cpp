// ArMaX.cpp : Implementation of CArMaXApp and DLL registration.

#include "stdafx.h"
#include "ArMaX.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


CArMaXApp NEAR theApp;

const GUID CDECL BASED_CODE _tlid =
		{ 0xFBB21704, 0xA554, 0x42C7, { 0xAA, 0xCB, 0xFE, 0xAE, 0x92, 0xE, 0x8D, 0xAC } };
const WORD _wVerMajor = 1;
const WORD _wVerMinor = 0;



// CArMaXApp::InitInstance - DLL initialization

BOOL CArMaXApp::InitInstance()
{
	BOOL bInit = COleControlModule::InitInstance();

	if (bInit)
	{
		// TODO: Add your own module initialization code here.
	}

	return bInit;
}



// CArMaXApp::ExitInstance - DLL termination

int CArMaXApp::ExitInstance()
{
	// TODO: Add your own module termination code here.

	return COleControlModule::ExitInstance();
}



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
