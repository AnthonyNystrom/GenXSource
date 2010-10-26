// stdafx.h : include file for standard system include files,
//  or project specific include files that are used frequently, but
//      are changed infrequently
//

#if !defined(AFX_STDAFX_H__2A523B18_F6F2_40CC_9854_0D98057F6B5F__INCLUDED_)
#define AFX_STDAFX_H__2A523B18_F6F2_40CC_9854_0D98057F6B5F__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


#ifndef WINVER
#define WINVER 0x0500
#endif

#ifndef _WIN32_WINNT
#define _WIN32_WINNT 0x0501
#endif

#pragma comment( lib, "dxerr.lib" )
#pragma comment( lib, "dxguid.lib" )
#pragma comment( lib, "d3d9.lib" )
#pragma comment( lib, "d3d10.lib" )
#if defined(DEBUG) || defined(_DEBUG)
#pragma comment( lib, "d3dx9d.lib" )
#pragma comment( lib, "d3dx10d.lib" )
#else
#pragma comment( lib, "d3dx9.lib" )
#pragma comment( lib, "d3dx10.lib" )
#endif

#ifdef _DEBUG
#define _CRTDBG_MAP_ALLOC // include Microsoft memory leak detection procedures
#endif
#include <stdlib.h>

#define VC_EXTRALEAN		// Exclude rarely-used stuff from Windows headers

#include <afxwin.h>         // MFC core and standard components
#include <afxext.h>         // MFC extensions
#include <afxdisp.h>        // MFC Automation classes
#include <afxdtctl.h>		// MFC support for Internet Explorer 4 Common Controls
#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>			// MFC support for Windows Common Controls
#endif // _AFX_NO_AFXCMN_SUPPORT

#include <afxinet.h>
#include <vector>

#include <windows.h>
#include <assert.h>
#include <wchar.h>
#include <mmsystem.h>
#include <commctrl.h> // for InitCommonControls() 
//	#include <shellapi.h> // for ExtractIcon()
#include <new.h>      // for placement new
#include <math.h>      
#include <limits.h>      
#include <stdio.h>
#include <afxhtml.h>
#include <gdiplus.h>
#pragma comment(lib, "gdiplus.lib")

// CRT's memory leak detection
#if defined(DEBUG) | defined(_DEBUG)
#include <crtdbg.h>
#endif

#pragma warning( disable : 4100 ) // disable unreference formal parameter warnings for /W4 builds
#pragma warning( disable : 4819 ) // disable unreference formal parameter warnings for /W4 builds


#if (_MSC_VER > 1310) // VS2005
#pragma comment(linker, "\"/manifestdependency:type='Win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='X86' publicKeyToken='6595b64144ccf1df' language='*'\"")
#endif

// strsafe.h deprecates old unsecure string functions.  If you 
// really do not want to it to (not recommended), then uncomment the next line 
#define STRSAFE_NO_DEPRECATE

#ifndef STRSAFE_NO_DEPRECATE
#pragma deprecated("strncpy")
#pragma deprecated("wcsncpy")
#pragma deprecated("_tcsncpy")
#pragma deprecated("wcsncat")
#pragma deprecated("strncat")
#pragma deprecated("_tcsncat")
#endif

#pragma warning( disable : 4996 ) // disable deprecated warning 
#include <strsafe.h>
#pragma warning( default : 4996 ) 

#include <afxdlgs.h>

#if defined(DEBUG) || defined(_DEBUG)
#define D3D_DEBUG_INFO
#endif

// Direct3D includes
#include <WindowsX.h>
#include <d3d9.h>
#include <d3dx9.h>
#include <dxerr.h>

#include "Resource.h"
#include "DXUtil.h"
#include "D3DEnumeration.h"
#include "D3DSettings.h"
#include "D3DApp.h"
#include "D3DFont.h"
#include "D3DFile.h"
#include "D3DUtil.h"

extern	BOOL	g_bRequestRender;
extern	BOOL	g_bDisplayFPS;

typedef CArray<D3DXVECTOR3> CVectorArray;

typedef std::vector < long > CSTLLongArray;
typedef std::vector < double > CSTLDoubleArray;

typedef std::vector < long > CSTLIntArray;
typedef std::vector < double > CSTLDoubleArray;

typedef std::vector<D3DXVECTOR3 *> CSTLArrayVector;
typedef std::vector<D3DXMATRIXA16 *> CSTLArrayMatrix;
typedef std::vector<D3DXMATRIX> CSTLArrayMatrixValue;

typedef std::vector <D3DXVECTOR4> CSTLArrayVector4;
typedef std::vector <D3DXCOLOR> CSTLArrayColor;
typedef std::vector <D3DXVECTOR2> CSTLArrayVector2;
typedef std::vector <BOOL> CSTLArrayBool;

typedef std::vector<D3DXVECTOR3 *> CSTLArrayVector;
typedef std::vector<D3DXVECTOR3> CSTLVectorValueArray;
typedef std::vector<LONG> CSTLLONGArray;
typedef std::vector<LONGLONG> CSTLLONGLONGArray;

typedef std::vector < D3DXVECTOR3 > CSTLVectorValueArray;
typedef std::vector < FLOAT > CSTLFLOATArray;
typedef std::vector < LONG > CSTLLONGArray;

typedef std::vector<D3DXVECTOR3> CSTLArrayD3DXVECTOR3;

typedef std::basic_string<TCHAR> CSTLString;

#define _IN_
#define _OUT_

#if defined(DEBUG) || defined(_DEBUG)
#ifndef V
#define V(x)           { hr = x; if( FAILED(hr) ) { TRACE("%s(%d): %s" , __FILE__, (DWORD)__LINE__, L#x); } }
#endif
#ifndef V_RETURN
#define V_RETURN(x)    { hr = x; if( FAILED(hr) ) { TRACE( "%s(%d): %s" , __FILE__, (DWORD)__LINE__, L#x ); return hr; } }
#endif
#else
#ifndef V
#define V(x)           { hr = x; }
#endif
#ifndef V_RETURN
#define V_RETURN(x)    { hr = x; if( FAILED(hr) ) { return hr; } }
#endif
#endif

#ifndef SAFE_DELETE
#define SAFE_DELETE(p)       { if(p) { delete (p);     (p)=NULL; } }
#endif    
#ifndef SAFE_DELETE_ARRAY
#define SAFE_DELETE_ARRAY(p) { if(p) { delete[] (p);   (p)=NULL; } }
#endif    
#ifndef SAFE_RELEASE
#define SAFE_RELEASE(p)      { if(p) { (p)->Release(); (p)=NULL; } }
#endif

#define SAFE_DELETE_AR(ptr)
//{{AFX_CODEJOCK_PRIVATE
#undef SAFE_DELETE_AR
#define SAFE_DELETE_AR(ptr) \
	if (ptr) { delete [] ptr; ptr = NULL; }

#pragma warning( disable : 4244 )
#pragma warning(disable : 4101 )
#pragma warning(disable : 4018 )
#pragma warning(disable : 4305 )
#pragma warning(disable : 4996 )

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.


#endif // !defined(AFX_STDAFX_H__2A523B18_F6F2_40CC_9854_0D98057F6B5F__INCLUDED_)
 
