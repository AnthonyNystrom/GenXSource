// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#ifndef VC_EXTRALEAN
#define VC_EXTRALEAN		// Exclude rarely-used stuff from Windows headers
#endif

// Modify the following defines if you have to target a platform prior to the ones specified below.
// Refer to MSDN for the latest info on corresponding values for different platforms.
#ifndef WINVER				// Allow use of features specific to Windows 95 and Windows NT 4 or later.
#define WINVER 0x0500		// Change this to the appropriate value to target Windows 98 and Windows 2000 or later.
#endif

#ifndef _WIN32_WINNT		// Allow use of features specific to Windows NT 4 or later.
#define _WIN32_WINNT 0x0400		// Change this to the appropriate value to target Windows 98 and Windows 2000 or later.
#endif						

#ifndef _WIN32_WINDOWS		// Allow use of features specific to Windows 98 or later.
#define _WIN32_WINDOWS 0x0410 // Change this to the appropriate value to target Windows Me or later.
#endif

#ifndef _WIN32_IE			// Allow use of features specific to IE 4.0 or later.
#define _WIN32_IE 0x0400	// Change this to the appropriate value to target IE 5.0 or later.
#endif

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS	// some CString constructors will be explicit

// turns off MFC's hiding of some common and often safely ignored warning messages
#define _AFX_ALL_WARNINGS

#include <afxwin.h>         // MFC core and standard components
#include <afxext.h>         // MFC extensions
#include <afxdisp.h>        // MFC Automation classes
#include <afxpriv.h>				// For MFC internals

#include <afxdtctl.h>		// MFC support for Internet Explorer 4 Common Controls
#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>			// MFC support for Windows Common Controls
#endif // _AFX_NO_AFXCMN_SUPPORT

#include "CommonStructures.h"

#include "..//CxImage//xImage.h"

#include <new>
#include <vector>
#include <list>
#include <algorithm>

#include <gl\gl.h>
#include <gl\glu.h>

#include "..//ExGUILib//Includes//ExGUILib.h"
#include "..//sgCore//rtEngine.h"


#include <afxdhtml.h>



/************РАБОТА С БИТАМИ****************/

//проверка n-го бита в числе x


#define NTH_BIT(n) (1 << n)
#define TEST_ANY_BITS(x, y) (((x) & (y)) != 0)
#define TEST_ALL_BITS(x, y) (((x) & (y)) == (y))
#define TEST_NTH_BIT(x, n) TEST_ANY_BITS(x, NTH_BIT(n))

//////////////////////////
// Задание двоичных констант
#define BIN___(x)                                        \
	(                                                \
	((x / 01ul) % 010)*(2>>1) +                      \
	((x / 010ul) % 010)*(2<<0) +                     \
	((x / 0100ul) % 010)*(2<<1) +                    \
	((x / 01000ul) % 010)*(2<<2) +                   \
	((x / 010000ul) % 010)*(2<<3) +                  \
	((x / 0100000ul) % 010)*(2<<4) +                 \
	((x / 01000000ul) % 010)*(2<<5) +                \
	((x / 010000000ul) % 010)*(2<<6)                 \
	)

#define BIN8(x) BIN___(0##x)

#define BIN16(x1,x2) \
	((BIN8(x1)<<8)+BIN8(x2))
#define BIN24(x1,x2,x3) \
	((BIN8(x1)<<16)+(BIN8(x2)<<8)+BIN8(x3))
#define BIN32(x1,x2,x3,x4) \
	((BIN8(x1)<<24)+(BIN8(x2)<<16)+(BIN8(x3)<<8)+BIN8(x4))

/************************************************************************/
/* Пример:
char  i1 = BIN8 (101010);
short i2 = BIN16(10110110,11101110);
long  i3 = BIN24(10110111,00010111,01000110);
long  i4 = BIN32(11101101,01101000,01010010,10111100);
/************************************************************************/



extern "C"
{
#include "../../lua/lualib.h"
#include "../../lua/lauxlib.h"
}

int sc_printf(const char *message,...);

extern lua_State* lua_state;
void  LuaRegister();
void  LuaUnregister();
void  LuaRunScript(const char* file_name);
