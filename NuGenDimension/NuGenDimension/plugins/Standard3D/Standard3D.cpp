// Standard3D.cpp : Defines the initialization routines for the DLL.
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

#include "Commands//Box.h"
#include "Commands//Sphere.h"
#include "Commands//Cylinder.h"
#include "Commands//Cone.h"
#include "Commands//Torus.h"
#include "Commands//Ellipsoid.h"
#include "Commands//SphericBand.h"

#include "Commands//BoxEdit.h"
#include "Commands//SphereEdit.h"
#include "Commands//CylinderEdit.h"
#include "Commands//ConeEdit.h"
#include "Commands//TorusEdit.h"
#include "Commands//EllipsoidEdit.h"
#include "Commands//SphericBandEdit.h"

extern "C"  AFX_EXTENSION_MODULE Standard3DDLL = { NULL, NULL };

extern "C" int APIENTRY
DllMain(HINSTANCE hInstance, DWORD dwReason, LPVOID lpReserved)
{
	// Remove this if you use lpReserved
	UNREFERENCED_PARAMETER(lpReserved);

	if (dwReason == DLL_PROCESS_ATTACH)
	{
		TRACE0("Standard3D.DLL Initializing!\n");
		
		// Extension DLL one-time initialization
		if (!AfxInitExtensionModule(Standard3DDLL, hInstance))
			return 0;

		new CDynLinkLibrary(Standard3DDLL);
	}
	else if (dwReason == DLL_PROCESS_DETACH)
	{
		TRACE0("Standard3D.DLL Terminating!\n");
		
		AfxTermExtensionModule(Standard3DDLL);
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
	box_name_index = 1;
	sphere_name_index = 1;
	cylinder_name_index = 1;
	cone_name_index = 1;
	torus_name_index = 1;
	ellipsoid_name_index = 1;
	spheric_band_name_index = 1;
}

CImageList imageList;

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

	//Load32BitmapOnToolbar(pToolbar,IDB_TLB_BMP);

	pToolbar->GetToolBarCtrl().SetCmdID(0,endID++);
	pToolbar->GetToolBarCtrl().SetCmdID(1,endID++);
	pToolbar->GetToolBarCtrl().SetCmdID(2,endID++);
	pToolbar->GetToolBarCtrl().SetCmdID(3,endID++);
	pToolbar->GetToolBarCtrl().SetCmdID(4,endID++);
	pToolbar->GetToolBarCtrl().SetCmdID(5,endID++);
	pToolbar->GetToolBarCtrl().SetCmdID(6,endID++);
	pToolbar->EnableDocking(CBRS_ALIGN_ANY);

	ResetNames();
}


extern "C" AFX_EXT_API   HBITMAP  GetToolbarBitmap(LPWORD wC)
{
	HBITMAP hBitmap = NULL;
	/*if ( NULL == hInst )
	hInst = ::AfxFindResourceHandle( IDB_TOOLBAR32_BMP, RT_BITMAP);*/
	HRSRC hRsrc = ::FindResource(Standard3DDLL.hResource, 
		MAKEINTRESOURCE(IDB_TLB_BMP), RT_BITMAP);
	if ( hRsrc ){
		HGLOBAL hglb = LoadResource(Standard3DDLL.hResource, hRsrc);
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
	if (strcmp("{0000000000000-0000-0001-000000000009}",obID)==0)
	{
		hRsrc = ::FindResource(Standard3DDLL.hResource, 
			MAKEINTRESOURCE(IDB_CUBE), RT_BITMAP);
		goto lbl;
	}
	if (strcmp("{0000000000000-0000-0002-000000000009}",obID)==0)
	{
		hRsrc = ::FindResource(Standard3DDLL.hResource, 
			MAKEINTRESOURCE(IDB_SPHERE), RT_BITMAP);
		goto lbl;	
	}
	if (strcmp("{0000000000000-0000-0003-000000000009}",obID)==0)
	{
		hRsrc = ::FindResource(Standard3DDLL.hResource, 
			MAKEINTRESOURCE(IDB_CYL), RT_BITMAP);
		goto lbl;	
	}
	if (strcmp("{0000000000000-0000-0004-000000000009}",obID)==0)
	{
		hRsrc = ::FindResource(Standard3DDLL.hResource, 
			MAKEINTRESOURCE(IDB_CONE), RT_BITMAP);
		goto lbl;
	}
	if (strcmp("{0000000000000-0000-0005-000000000009}",obID)==0)
	{
		hRsrc = ::FindResource(Standard3DDLL.hResource, 
			MAKEINTRESOURCE(IDB_TOR), RT_BITMAP);
		goto lbl;
	}
	if (strcmp("{0000000000000-0000-0008-000000000009}",obID)==0)
	{
		hRsrc = ::FindResource(Standard3DDLL.hResource, 
			MAKEINTRESOURCE(IDB_ELL), RT_BITMAP);
		goto lbl;
	}
	if (strcmp("{0000000000000-0000-0009-000000000009}",obID)==0)
	{
		hRsrc = ::FindResource(Standard3DDLL.hResource, 
			MAKEINTRESOURCE(IDB_SB), RT_BITMAP);
		goto lbl;
	}
lbl:
	if ( hRsrc ){
		HGLOBAL hglb = LoadResource(Standard3DDLL.hResource, hRsrc);
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
	objIDs.push_back("{0000000000000-0000-0001-000000000009}");
	objIDs.push_back("{0000000000000-0000-0002-000000000009}");
	objIDs.push_back("{0000000000000-0000-0003-000000000009}");
	objIDs.push_back("{0000000000000-0000-0004-000000000009}");
	objIDs.push_back("{0000000000000-0000-0005-000000000009}");
	objIDs.push_back("{0000000000000-0000-0008-000000000009}");
	objIDs.push_back("{0000000000000-0000-0009-000000000009}");
	//objIDs.push_back(5);
}

extern "C" AFX_EXT_API   void GetButtonState(unsigned int nID, bool& ch, bool& enbl)
{
	enbl = true;
	if ((nID==startID) && (active_regime == REGIME_BOX))
	{
		ch=true;
		return;
	}
	if ((nID==startID+1) && (active_regime == REGIME_SPHERE))
	{
		ch=true;
		return;
	}
	if ((nID==startID+2) && (active_regime == REGIME_CYLINDER))
	{
		ch=true;
		return;
	}
	if ((nID==startID+3) && (active_regime == REGIME_CONE))
	{
		ch=true;
		return;
	}
	if ((nID==startID+4) && (active_regime == REGIME_TORUS))
	{
		ch=true;
		return;
	}
	if ((nID==startID+5) && (active_regime == REGIME_ELLIPSOID))
	{
		ch=true;
		return;
	}
	if ((nID==startID+6) && (active_regime == REGIME_SPHERIC_BAND))
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
	else if (nID == startID+6)
	{
		temporary_message_str.LoadString(IDS_STBAR_SIXTH);
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
	else if (nID == startID+6)
	{
		temporary_message_str.LoadString(IDS_TOOLTIP_SIXTH);
		return temporary_message_str;
	}
	else
		return "Unknown";
}

static   ICommander*   global_current_commander = NULL;

extern "C" AFX_EXT_API   ICommander* GetNewCommander(unsigned int nID, IApplicationInterface* appI)
{
	if (nID == startID)
		{
			active_regime = REGIME_BOX;
			global_current_commander = new BoxCommand(appI);
			return global_current_commander;
		}
		else if (nID == startID+1)
		{
			active_regime = REGIME_SPHERE;
			global_current_commander = new SphereCommand(appI);
			return global_current_commander;
		}
		else if (nID == startID+2)
		{
			active_regime = REGIME_CYLINDER;
			global_current_commander = new CylinderCommand(appI);
			return global_current_commander;
		}
		else if (nID == startID+3)
		{
			active_regime = REGIME_CONE;
			global_current_commander = new ConeCommand(appI);
			return global_current_commander;
		}
		else if (nID == startID+4)
		{
			active_regime = REGIME_TORUS;
			global_current_commander = new TorusCommand(appI);
			return global_current_commander;
		}
		else if (nID == startID+5)
		{
			active_regime = REGIME_ELLIPSOID;
			global_current_commander = new EllipsoidCommand(appI);
			return global_current_commander;
		}
		else if (nID == startID+6)
		{
			active_regime = REGIME_SPHERIC_BAND;
			global_current_commander = new SphericBandCommand(appI);
			return global_current_commander;
		}
	return NULL;
}

extern "C" AFX_EXT_API   ICommander* GetEditCommander(sgCObject* editableObj,  IApplicationInterface* appI)
{
	const char* obID = editableObj->GetUserGeometryID();
	if (strcmp("{0000000000000-0000-0001-000000000009}",obID)==0)
	{
			sgCBox* edB = reinterpret_cast<sgCBox*>(editableObj);
			global_current_commander = new CBoxEditCommand(edB, appI);
			return global_current_commander;	
	}
	if (strcmp("{0000000000000-0000-0002-000000000009}",obID)==0)
	{
		sgCSphere* edS = reinterpret_cast<sgCSphere*>(editableObj);
		global_current_commander = new CSphereEditCommand(edS, appI);
		return global_current_commander;	
	}
	if (strcmp("{0000000000000-0000-0003-000000000009}",obID)==0)
	{
		sgCCylinder* edC = reinterpret_cast<sgCCylinder*>(editableObj);
		global_current_commander = new CCylinderEditCommand(edC, appI);
		return global_current_commander;	
	}
	if (strcmp("{0000000000000-0000-0004-000000000009}",obID)==0)
	{
		sgCCone* edC = reinterpret_cast<sgCCone*>(editableObj);
		global_current_commander = new CConeEditCommand(edC, appI);
		return global_current_commander;	
	}
	if (strcmp("{0000000000000-0000-0005-000000000009}",obID)==0)
	{
		sgCTorus* edT = reinterpret_cast<sgCTorus*>(editableObj);
		global_current_commander = new CTorusEditCommand(edT, appI);
		return global_current_commander;	
	}
	if (strcmp("{0000000000000-0000-0008-000000000009}",obID)==0)
	{
		sgCEllipsoid* edE = reinterpret_cast<sgCEllipsoid*>(editableObj);
		global_current_commander = new CEllipsoidEditCommand(edE, appI);
		return global_current_commander;	
	}
	if (strcmp("{0000000000000-0000-0009-000000000009}",obID)==0)
	{
		sgCSphericBand* edSb = reinterpret_cast<sgCSphericBand*>(editableObj);
		global_current_commander = new CSphericBandEditCommand(edSb, appI);
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

