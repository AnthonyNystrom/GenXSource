// stdafx.h : include file for standard system include files,
//  or project specific include files that are used frequently, but
//      are changed infrequently
//

#if !defined(AFX_STDAFX_H__B0E95656_703F_4413_B286_52BB827BF058__INCLUDED_)
#define AFX_STDAFX_H__B0E95656_703F_4413_B286_52BB827BF058__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#define VC_EXTRALEAN		// Exclude rarely-used stuff from Windows headers

#include <afxwin.h>         // MFC core and standard components
#include <afxext.h>         // MFC extensions

#ifndef _AFX_NO_OLE_SUPPORT
#include <afxole.h>         // MFC OLE classes
#include <afxodlgs.h>       // MFC OLE dialog classes
#include <afxdisp.h>        // MFC Automation classes
#endif // _AFX_NO_OLE_SUPPORT


#ifndef _AFX_NO_DB_SUPPORT
#include <afxdb.h>			// MFC ODBC database classes
#endif // _AFX_NO_DB_SUPPORT

#ifndef _AFX_NO_DAO_SUPPORT
#include <afxdao.h>			// MFC DAO database classes
#endif // _AFX_NO_DAO_SUPPORT

#include <afxdtctl.h>		// MFC support for Internet Explorer 4 Common Controls
#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>			// MFC support for Windows Common Controls
#endif // _AFX_NO_AFXCMN_SUPPORT

#include <memory.h>
#include <vector>


#include "NuGenDimension/CommonStructures.h"

extern "C"  AFX_EXTENSION_MODULE Standard2DDLL;

class DllInstanceSwitcher
{
public:
	DllInstanceSwitcher()
	{
		m_hInst = AfxGetResourceHandle();
		AfxSetResourceHandle(Standard2DDLL.hResource);
	}
	~DllInstanceSwitcher()
	{
		AfxSetResourceHandle(m_hInst);
	}
private:
	HINSTANCE m_hInst;
};

#define SWITCH_RESOURCE  DllInstanceSwitcher __SwitchInstance;


extern unsigned int endID;
extern unsigned int startID;

typedef enum
{
	REGIME_NONE,
	REGIME_POINT,
	REGIME_LINE,
	REGIME_CIRCLE,
	REGIME_ARC,
	REGIME_SPLINE,
	REGIME_CONTOUR,
	REGIME_EQUID
} REGIME;

extern REGIME  active_regime;

extern int     point_name_index;
extern int     line_name_index;
extern int     circle_name_index;
extern int     arc_name_index;
extern int     spline_name_index;
extern int     contour_name_index;
extern int     equidi_name_index;

//#include "..//GetDialogs//GetDialogs.h"

#include <afxdlgs.h>

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STDAFX_H__B0E95656_703F_4413_B286_52BB827BF058__INCLUDED_)
