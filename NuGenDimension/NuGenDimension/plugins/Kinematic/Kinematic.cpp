// Kinematic.cpp : Defines the initialization routines for the DLL.
//

#include "stdafx.h"
#include "resource.h"
#include <afxdllx.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

unsigned int	 endID = 0;
unsigned int	 startID = 0;

REGIME			 active_regime = REGIME_NONE;

#include "Commands//RotationBody.h"
#include "Commands//PipeBody.h"
#include "Commands//ExtrudeBody.h"
#include "Commands//ScrewBody.h"

extern "C"  AFX_EXTENSION_MODULE KinematicDLL = { NULL, NULL };

extern "C" int APIENTRY
DllMain(HINSTANCE hInstance, DWORD dwReason, LPVOID lpReserved)
{
	// Remove this if you use lpReserved
	UNREFERENCED_PARAMETER(lpReserved);

	if (dwReason == DLL_PROCESS_ATTACH)
	{
		TRACE0("Kinematic.DLL Initializing!\n");
		
		// Extension DLL one-time initialization
		if (!AfxInitExtensionModule(KinematicDLL, hInstance))
			return 0;

		new CDynLinkLibrary(KinematicDLL);
	}
	else if (dwReason == DLL_PROCESS_DETACH)
	{
		TRACE0("Kinematic.DLL Terminating!\n");
		
		AfxTermExtensionModule(KinematicDLL);
	}
	return 1;   // ok
}

extern "C" AFX_EXT_API void GetPluginInfo(PLUGIN_INFO* plInfo)
{
	SWITCH_RESOURCE
	plInfo->plugin_type = PLUGIN_TOOLBAR;
	plInfo->menu_string.LoadString(IDS_MENU_STRING);
	plInfo->show_after_load = true;
	plInfo->in_trial_version = true;
	plInfo->plugin_version = 1;
	plInfo->NuGenDimension_version = 1;
	plInfo->kernel_version = 1;
}

extern "C" AFX_EXT_API void ResetNames()
{
	rotation_name_index = 1;
	screw_name_index = 1;
	pipe_name_index = 1;
	extrude_name_index = 1;
}

extern "C" AFX_EXT_API void GetToolbar(unsigned int start_ID_from_app, 
											   CToolBar* pToolbar, 
											   CWnd* pPar)
{
	SWITCH_RESOURCE
	startID = endID = start_ID_from_app;

	if (!pToolbar->CreateEx(pPar, TBSTYLE_FLAT, WS_CHILD | CBRS_TOP
		| CBRS_GRIPPER | CBRS_TOOLTIPS | CBRS_FLYBY | CBRS_SIZE_DYNAMIC) ||
		!pToolbar->LoadToolBar(MAKEINTRESOURCE(IDR_TOOLBAR)))
	{
		TRACE0("Failed to create toolbar 2D\n");
		return;      // fail to create
	}

	//Load32BitmapOnToolbar(pToolbar,IDB_TOOLBAR32_BMP);

	pToolbar->GetToolBarCtrl().SetCmdID(0,endID++);
	pToolbar->GetToolBarCtrl().SetCmdID(1,endID++);
	pToolbar->GetToolBarCtrl().SetCmdID(2,endID++);
	pToolbar->GetToolBarCtrl().SetCmdID(3,endID++);

	pToolbar->EnableDocking(CBRS_ALIGN_ANY);

	ResetNames();
}


extern "C" AFX_EXT_API   HBITMAP  GetToolbarBitmap(LPWORD wC)
{
	HBITMAP hBitmap = NULL;
	/*if ( NULL == hInst )
	hInst = ::AfxFindResourceHandle( IDB_TOOLBAR32_BMP, RT_BITMAP);*/
	HRSRC hRsrc = ::FindResource(KinematicDLL.hResource, 
		MAKEINTRESOURCE(IDB_TOOLBAR32_BMP), RT_BITMAP);
	if ( hRsrc ){
		HGLOBAL hglb = LoadResource(KinematicDLL.hResource, hRsrc);
		if ( hglb ){
			// Читаем заголовок
			LPBITMAPINFO pbi = (LPBITMAPINFO)LockResource(hglb);
			if (pbi ) {
				/*if ( lpnWidth )
				*lpnWidth = pbi->bmiHeader.biWidth;
				if ( lpnHeight )
				*lpnHeight = pbi->bmiHeader.biHeight;*/
				if ( wC )
					*wC = pbi->bmiHeader.biBitCount;
				// Читаем данные
				HDC hdc = GetDC( NULL );
				//hBitmap = CreateDIBitmap( hdc, &pbi->bmiHeader, CBM_INIT, pbi->bmiColors, pbi, DIB_RGB_COLORS);
				//				BYTE* pData = (BYTE*)pbi + sizeof(BITMAPINFO) + pbi->bmiHeader.biClrUsed * sizeof(COLORREF);
				BYTE* pData = (BYTE*)pbi + sizeof(BITMAPINFOHEADER) + pbi->bmiHeader.biClrUsed * sizeof(COLORREF);
				hBitmap = CreateDIBitmap( hdc, &pbi->bmiHeader, CBM_INIT, (void*)pData, pbi, DIB_RGB_COLORS);
				// hBitmap = CreateDIBSection( hdc, pbi, DIB_RGB_COLORS, (LPVOID*)&pbi->bmiColors, NULL, NULL );
				::ReleaseDC	(NULL, hdc);
			}
			FreeResource( hglb );
		}	
	}
	return hBitmap;
}


extern "C" AFX_EXT_API   HBITMAP  GetObjectBitmap(const char* obID, LPWORD wC)
{
	HBITMAP hBitmap = NULL;
	HRSRC hRsrc = NULL;
	if (strcmp("{D5E003ED-B53E-40b8-9ABB-2E7B6E72D7DC}",obID)==0)
	{
		hRsrc = ::FindResource(KinematicDLL.hResource, 
			MAKEINTRESOURCE(IDB_ROT_BOD_BMP), RT_BITMAP);
		goto lbl;
	}
	if (strcmp("{2D9BCE0E-4547-4432-8A5C-A4FD115E82F7}",obID)==0)
	{
		hRsrc = ::FindResource(KinematicDLL.hResource, 
			MAKEINTRESOURCE(IDB_EXTR_BOD), RT_BITMAP);
		goto lbl;	
	}
	if (strcmp("{260E8383-41CD-4552-9034-F6C830495EBD}",obID)==0)
	{
		hRsrc = ::FindResource(KinematicDLL.hResource, 
			MAKEINTRESOURCE(IDB_SPIR_BOD_BMP), RT_BITMAP);
		goto lbl;	
	}
	if (strcmp("{C25A14A9-2D10-436c-A581-F3F19AD05EE8}",obID)==0)
	{
		hRsrc = ::FindResource(KinematicDLL.hResource, 
			MAKEINTRESOURCE(IDB_PIP_BOD_BMP), RT_BITMAP);
		goto lbl;
	}
lbl:
	if ( hRsrc ){
		HGLOBAL hglb = LoadResource(KinematicDLL.hResource, hRsrc);
		if ( hglb ){
			// Читаем заголовок
			LPBITMAPINFO pbi = (LPBITMAPINFO)LockResource(hglb);
			if (pbi ) {
				if ( wC )
					*wC = pbi->bmiHeader.biBitCount;
				// Читаем данные
				HDC hdc = GetDC( NULL );
				BYTE* pData = (BYTE*)pbi + sizeof(BITMAPINFOHEADER) + pbi->bmiHeader.biClrUsed * sizeof(COLORREF);
				hBitmap = CreateDIBitmap( hdc, &pbi->bmiHeader, CBM_INIT, (void*)pData, pbi, DIB_RGB_COLORS);
				::ReleaseDC	(NULL, hdc);
			}
			FreeResource( hglb );
		}	
	}
	return hBitmap;
}

extern "C" AFX_EXT_API   void  GetFirstAndLastIDs(unsigned int& stID, unsigned int& eID)
{
	stID = startID;
	eID  = endID-1;
}

extern "C" AFX_EXT_API   void GetSupportedObjectIDs(std::vector<const char*>& objIDs)
{
	objIDs.push_back("{D5E003ED-B53E-40b8-9ABB-2E7B6E72D7DC}"); //rotation
	objIDs.push_back("{2D9BCE0E-4547-4432-8A5C-A4FD115E82F7}"); // exrtude
	objIDs.push_back("{260E8383-41CD-4552-9034-F6C830495EBD}");
	objIDs.push_back("{C25A14A9-2D10-436c-A581-F3F19AD05EE8}");
}

extern "C" AFX_EXT_API   void GetButtonState(unsigned int nID, bool& ch, bool& enbl)
{
	enbl = (sgGetScene()->GetObjectsList()->GetCount()>0);
	ch=false;
	if ((nID==startID) && (active_regime == REGIME_ROTATION))
	{
		ch=true;
	    return;
	}
	if ((nID==startID+1) && (active_regime == REGIME_EXTRUDE))
	{
		ch=true;
		return;
	}
	if ((nID==startID+2) && (active_regime == REGIME_SCREW))
	{
		ch=true;
		return;
	}
	if ((nID==startID+3) && (active_regime == REGIME_PIPE))
	{
		ch=true;
		return;
	}
}

CString temporary_message_str;
extern "C" AFX_EXT_API   LPCTSTR  GetStatusBarMessage(unsigned int nID)
{
	SWITCH_RESOURCE
	if (nID == startID)
	{
		temporary_message_str.LoadString(IDS_STBAR_ZERO);
		return temporary_message_str;
	}
	else if (nID == startID+1)
	{
		temporary_message_str.LoadString(IDS_STBAR_FIRST);
		return temporary_message_str;
	}
	else if (nID == startID+2)
	{
		temporary_message_str.LoadString(IDS_STBAR_SECOND);
		return temporary_message_str;
	}
	else if (nID == startID+3)
	{
		temporary_message_str.LoadString(IDS_STBAR_THIRD);
		return temporary_message_str;
	}
	else
		return "Unknown";
}

extern "C" AFX_EXT_API   LPCTSTR  GetTooltipMessage(unsigned int nID)
{
	SWITCH_RESOURCE
	if (nID == startID)
	{
		temporary_message_str.LoadString(IDS_TOOLTIP_ZERO);
		return temporary_message_str;
	}
	else if (nID == startID+1)
	{
		temporary_message_str.LoadString(IDS_TOOLTIP_FIRST);
		return temporary_message_str;
	}
	else if (nID == startID+2)
	{
		temporary_message_str.LoadString(IDS_TOOLTIP_SECOND);
		return temporary_message_str;
	}
	else if (nID == startID+3)
	{
		temporary_message_str.LoadString(IDS_TOOLTIP_THIRD);
		return temporary_message_str;
	}
	else
		return "Unknown";
}

static   ICommander*   global_current_commander = NULL;

extern "C" AFX_EXT_API   ICommander* GetNewCommander(unsigned int nID,IApplicationInterface* appI)
{
	if (nID == startID)
		{
			active_regime = REGIME_ROTATION;
			global_current_commander = new RotationCommand(appI);
			return global_current_commander;
		}
		else if (nID == startID+1)
		{
			active_regime = REGIME_EXTRUDE;
			global_current_commander = new ExtrudeCommand(appI);
			return global_current_commander;
		}
		else if (nID == startID+2)
		{
			active_regime = REGIME_SCREW;
			global_current_commander = new ScrewCommand(appI);
			return global_current_commander;
		}
		else if (nID == startID+3)
		{
			active_regime = REGIME_PIPE;
			global_current_commander = new PipeCommand(appI);
			return global_current_commander;
		}
		/*else if (nID == startID+4)
		{
			active_regime = REGIME_SECTION;
			global_current_commander = new SectionCommand(appI);
			return global_current_commander;
		}*/
		
	return NULL;
}

extern "C" AFX_EXT_API   ICommander* GetEditCommander(sgCObject* editableObj, IApplicationInterface* appI)
{
	return NULL;
}

extern "C" AFX_EXT_API   bool   FreeCommander(ICommander* cmndr)
{
	if ((global_current_commander!=NULL) && (global_current_commander==cmndr))
	{
		delete global_current_commander;
		global_current_commander = NULL;
		cmndr = NULL;
		active_regime = REGIME_NONE;
		return true;
	}
	else
	{
		ASSERT(0);
		return false;
	}
}

