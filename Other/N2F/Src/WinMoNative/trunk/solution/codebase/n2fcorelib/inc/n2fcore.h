#pragma once

#ifdef N2FCORELIB_EXPORTS
#define N2FCORE_API __declspec(dllexport) 
#else	//#ifdef N2FCORELIB_EXPORTS
#define N2FCORE_API __declspec(dllimport) 
#endif	//#else - #ifdef N2FCORELIB_EXPORTS

// configuration list

#ifdef DEBUG
#	define	USE_FILE_LOGGER	1	//turn-on file logging
#	define	USE_RUNTIME_TRACING_IN_PARALLEL	1	//turn on run-time tracing in parallel to file logging
#	define	OFFLINE_MODE	0
#else
#	define  USE_FILELOGGER	0
#	define	USE_RUNTIME_TRACING_IN_PARALLEL		0
#	define	OFFLINE_MODE	0
#endif	//#ifdef DEBUG

#include <strings_definitions.h>
#include <graphics_definitions.h>
#include <colors_definitions.h>

#include <recentuploadsdata.h>

#include <logger.h>

#include <webservice-n2f-memberservice.h>
#include <webservice-n2f-memberservice-v2.h>
#include <webservice-n2f-memberservice-v3.h>
#include <webservice-n2f-photoorganise.h>
#include <webservice-n2f-photoorganise-v2.h>
#include <webservice-n2f-snapupservice.h>

#include <webservicesdataproviders.h>

#include <imagehelper.h>
#include <controllerutil.h>
#include <controllersettings.h>
#include <controllerconfig.h>
#include <controllerwebservices.h>
#include <controllershost.h>







