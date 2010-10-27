// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#pragma comment(linker, "/nodefaultlib:libc.lib")
#pragma comment(linker, "/nodefaultlib:libcd.lib")

// NOTE - this value is not strongly correlated to the Windows CE OS version being targeted
#define WINVER _WIN32_WCE

#define _WIN32_WINNT	0x0501
#define _WIN32_IE	0x0501
#define _RICHEDIT_VER	0x0100

#define _WTL_USE_CSTRING

#include <atlbase.h>
#include <atlapp.h>
#include <atlwin.h>
#include <atlmisc.h>



#include <ceconfig.h>
#if defined(WIN32_PLATFORM_PSPC) || defined(WIN32_PLATFORM_WFSP)
#define SHELL_AYGSHELL
#endif

//#ifdef _CE_DCOM
//#define _ATL_APARTMENT_THREADED
//#endif

//#include <aygshell.h>
//#pragma comment(lib, "aygshell.lib") 


// Windows Header Files:
#include <windows.h>


//#if defined(WIN32_PLATFORM_PSPC) || defined(WIN32_PLATFORM_WFSP)
//#ifndef _DEVICE_RESOLUTION_AWARE
//#define _DEVICE_RESOLUTION_AWARE
//#endif
//#endif
//
//#ifdef _DEVICE_RESOLUTION_AWARE
//#include "DeviceResolutionAware.h"
//#endif

#if _WIN32_WCE < 0x500 && ( defined(WIN32_PLATFORM_PSPC) || defined(WIN32_PLATFORM_WFSP) )
	#pragma comment(lib, "ccrtrtti.lib")
	#ifdef _X86_	
		#if defined(_DEBUG)
			#pragma comment(lib, "libcmtx86d.lib")
		#else
			#pragma comment(lib, "libcmtx86.lib")
		#endif
	#endif
#endif

#include <altcecrt.h>

#include <tchar.h>

// TODO: reference additional headers your program requires here

#include "n2fcore.h"
#include "configuration.h"
#include "logger.h"
