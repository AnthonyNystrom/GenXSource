// stdafx.h : include file for standard system include files,
//  or project specific include files that are used frequently, but
//      are changed infrequently
//

#pragma once

// Change this value to use different versions
#define WINVER 0x0420
#include <atlbase.h>

#define _WTL_USE_CSTRING
#include <atlapp.h>

extern CAppModule _Module;

#include <atlwin.h>

#include <tpcshell.h>
#include <aygshell.h>
#pragma comment(lib, "aygshell.lib")

#include <atlframe.h>
#include <atlctrls.h>
#include <atlmisc.h>
#define _WTL_CE_NO_ZOOMSCROLL
#define _WTL_CE_NO_FULLSCREEN
#include <atlwince.h>

#include <n2fcore.h>
#include <n2f-ui-framework.h>

