// Standard2D.cpp : Defines the initialization routines for the DLL.
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

#include "Commands//Point.h"
#include "Commands//Line.h"
#include "Commands//Circle.h"
#include "Commands//Arc.h"
#include "Commands//Spline.h"
#include "Commands//Contour.h"
/*#include "Commands//Contour2.h"*/
#include "Commands//Equidi.h"
/*#include "Commands//Equidi2.h"*/

#include "Commands//PointEdit.h"
#include "Commands//LineEdit.h"
#include "Commands//CircleEdit.h"
#include "Commands//ArcEdit.h"
#include "Commands//SplineEdit.h"

extern "C"  AFX_EXTENSION_MODULE Standard2DDLL = { NULL, NULL };

extern "C" int APIENTRY
DllMain(HINSTANCE hInstance, DWORD dwReason, LPVOID lpReserved)
{
	// Remove this if you use lpReserved
	UNREFERENCED_PARAMETER(lpReserved);

	if (dwReason == DLL_PROCESS_ATTACH)
	{
		TRACE0("Standard2D.DLL Initializing!\n");
		
		// Extension DLL one-time initialization
		if (!AfxInitExtensionModule(Standard2DDLL, hInstance))
			return 0;

		new CDynLinkLibrary(Standard2DDLL);
	}
	else if (dwReason == DLL_PROCESS_DETACH)
	{
		TRACE0("Standard2D.DLL Terminating!\n");
		
		AfxTermExtensionModule(Standard2DDLL);
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
	point_name_index = 1;
	line_name_index = 1;
	circle_name_index = 1;
	arc_name_index = 1;
	spline_name_index = 1;
	contour_name_index = 1;
	equidi_name_index = 1;
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
	pToolbar->GetToolBarCtrl().SetCmdID(4,endID++);
	pToolbar->GetToolBarCtrl().SetCmdID(6,endID++);
	pToolbar->GetToolBarCtrl().SetCmdID(8,endID++);
	/*pToolbar->GetToolBarCtrl().SetCmdID(9,endID++);
	pToolbar->GetToolBarCtrl().SetCmdID(10,endID++);*/
	pToolbar->EnableDocking(CBRS_ALIGN_ANY);

	ResetNames();
}

extern "C" AFX_EXT_API   HBITMAP  GetToolbarBitmap(LPWORD wC)
{
	HBITMAP hBitmap = NULL;
	/*if ( NULL == hInst )
		hInst = ::AfxFindResourceHandle( IDB_TOOLBAR32_BMP, RT_BITMAP);*/
	HRSRC hRsrc = ::FindResource(Standard2DDLL.hResource, 
		MAKEINTRESOURCE(IDB_TOOLBAR32_BMP), RT_BITMAP);
	if ( hRsrc ){
		HGLOBAL hglb = LoadResource(Standard2DDLL.hResource, hRsrc);
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
	if (strcmp("{0000000000000-0000-0000-000000000001}",obID)==0)
	{
		hRsrc = ::FindResource(Standard2DDLL.hResource, 
			MAKEINTRESOURCE(IDB_PNT32), RT_BITMAP);
		goto lbl;
	}
	if (strcmp("{0000000000000-0000-0000-000000000002}",obID)==0)
	{
		hRsrc = ::FindResource(Standard2DDLL.hResource, 
			MAKEINTRESOURCE(IDB_LINE32), RT_BITMAP);
		goto lbl;	
	}
	if (strcmp("{0000000000000-0000-0000-000000000003}",obID)==0)
	{
		hRsrc = ::FindResource(Standard2DDLL.hResource, 
			MAKEINTRESOURCE(IDB_CIRC32), RT_BITMAP);
		goto lbl;	
	}
	if (strcmp("{0000000000000-0000-0000-000000000004}",obID)==0)
	{
		hRsrc = ::FindResource(Standard2DDLL.hResource, 
			MAKEINTRESOURCE(IDB_ARC32), RT_BITMAP);
		goto lbl;
	}
	if (strcmp("{0000000000000-0000-0000-000000000005}",obID)==0)
	{
		hRsrc = ::FindResource(Standard2DDLL.hResource, 
			MAKEINTRESOURCE(IDB_SPL32), RT_BITMAP);
		goto lbl;
	}
lbl:
	if ( hRsrc ){
		HGLOBAL hglb = LoadResource(Standard2DDLL.hResource, hRsrc);
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
	objIDs.push_back("{0000000000000-0000-0000-000000000001}");
	objIDs.push_back("{0000000000000-0000-0000-000000000002}");
	objIDs.push_back("{0000000000000-0000-0000-000000000003}");
	objIDs.push_back("{0000000000000-0000-0000-000000000004}");
	objIDs.push_back("{0000000000000-0000-0000-000000000005}");
	//objIDs.push_back(5);
}

extern "C" AFX_EXT_API   void GetButtonState(unsigned int nID, bool& ch, bool& enbl)
{
	enbl = true;
	if ((nID==startID) && (active_regime == REGIME_POINT))
	{
		ch=true;
	    return;
	}
	if ((nID==startID+1) && (active_regime == REGIME_LINE))
	{
		ch=true;
		return;
	}
	if ((nID==startID+2) && (active_regime == REGIME_CIRCLE))
	{
		ch=true;
		return;
	}
	if ((nID==startID+3) && (active_regime == REGIME_ARC))
	{
		ch=true;
		return;
	}
	if ((nID==startID+4) && (active_regime == REGIME_SPLINE))
	{
		ch=true;
		return;
	}
	if ((nID==startID+5) && (active_regime == REGIME_CONTOUR))
	{
		ch=true;
		return;
	}
	if ((nID==startID+6) && (active_regime == REGIME_EQUID))
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
	else if (nID == startID+4)
	{
		temporary_message_str.LoadString(IDS_STBAR_FOURTH);
		return temporary_message_str;
	}
	else if (nID == startID+5)
	{
		temporary_message_str.LoadString(IDS_STBAR_FIVETH);
		return temporary_message_str;
	}
	/*else if (nID == startID+6)
	{
		temporary_message_str.LoadString(IDS_STBAR_SIXTH);
		return temporary_message_str;
	}*/
	else if (nID == startID+6)
	{
		temporary_message_str.LoadString(IDS_STBAR_SEVENTH);
		return temporary_message_str;
	}
	/*else if (nID == startID+8)
	{
		temporary_message_str.LoadString(IDS_STBAR_EIGHTTH);
		return temporary_message_str;
	}*/
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
	else if (nID == startID+4)
	{
		temporary_message_str.LoadString(IDS_TOOLTIP_FOURTH);
		return temporary_message_str;
	}
	else if (nID == startID+5)
	{
		temporary_message_str.LoadString(IDS_TOOLTIP_FIVETH);
		return temporary_message_str;
	}
	/*else if (nID == startID+6)
	{
		temporary_message_str.LoadString(IDS_TOOLTIP_SIXTH);
		return temporary_message_str;
	}*/
	else if (nID == startID+6)
	{
		temporary_message_str.LoadString(IDS_TOOLTIP_SEVETH);
		return temporary_message_str;
	}
	/*else if (nID == startID+8)
	{
		temporary_message_str.LoadString(IDS_TOOLTIP_EIGHTTH);
		return temporary_message_str;
	}*/
	else
		return "Unknown";
}

static   ICommander*   global_current_commander = NULL;

extern "C" AFX_EXT_API   ICommander* GetNewCommander(unsigned int nID, IApplicationInterface* appI)
{
	if (nID == startID)
		{
			active_regime = REGIME_POINT;
			global_current_commander = new PointCommand(appI);
			return global_current_commander;
		}
		else if (nID == startID+1)
		{
			active_regime = REGIME_LINE;
			global_current_commander = new LineCommand(appI);
			return global_current_commander;
		}
		else if (nID == startID+2)
		{
			active_regime = REGIME_CIRCLE;
			global_current_commander = new CircleCommand(appI);
			return global_current_commander;
		}
		else if (nID == startID+3)
		{
			active_regime = REGIME_ARC;
			global_current_commander = new ArcCommand(appI);
			return global_current_commander;
		}
		else if (nID == startID+4)
		{
			active_regime = REGIME_SPLINE;
			global_current_commander = new SplineCommand(appI);
			return global_current_commander;
		}
		else if (nID == startID+5)
		{
			active_regime = REGIME_CONTOUR;
			global_current_commander = new Contour(appI);
			return global_current_commander;
		}
		/*else if (nID == startID+6)
		{
			active_regime = REGIME_CONTOUR_2;
			global_current_commander = new Contour2(appI);
			return global_current_commander;
		}*/
		else if (nID == startID+6)
		{
			active_regime = REGIME_EQUID;
			global_current_commander = new Equidi(appI);
			return global_current_commander;
		}
		/*else if (nID == startID+8)
		{
			active_regime = REGIME_EQUID_2;
			global_current_commander = new Equidi2(appI);
			return global_current_commander;
		}*/
	return NULL;
}

extern "C" AFX_EXT_API   ICommander* GetEditCommander(sgCObject* editableObj, IApplicationInterface* appI)
{
	const char* obID = editableObj->GetUserGeometryID();
	if (strcmp("{0000000000000-0000-0000-000000000001}",obID)==0)
	{
			sgCPoint* edP = reinterpret_cast<sgCPoint*>(editableObj);
			global_current_commander = new CPointEditCommand(edP, appI);
			return global_current_commander;	
	}
	if (strcmp("{0000000000000-0000-0000-000000000002}",obID)==0)
	{
		sgCLine* edL = reinterpret_cast<sgCLine*>(editableObj);
		global_current_commander = new LineEditCommand(edL, appI);
		return global_current_commander;	
	}
	if (strcmp("{0000000000000-0000-0000-000000000003}",obID)==0)
	{
		sgCCircle* edC = reinterpret_cast<sgCCircle*>(editableObj);
		global_current_commander = new CircleEditCommand(edC, appI);
		return global_current_commander;	
	}
	if (strcmp("{0000000000000-0000-0000-000000000004}",obID)==0)
	{
		sgCArc* edA = reinterpret_cast<sgCArc*>(editableObj);
		global_current_commander = new CArcEditCommand(edA, appI);
		return global_current_commander;	
	}
	if (strcmp("{0000000000000-0000-0000-000000000005}",obID)==0)
	{
		sgCSpline* edS = reinterpret_cast<sgCSpline*>(editableObj);
		global_current_commander = new SplineEditCommand(edS, appI);
		return global_current_commander;	
	}
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

