#ifndef __SUPCOMMON_H__
#define __SUPCOMMON_H__

// Default includes
#include <avkon.rsg>
#include <akndef.h>
#include <aknconsts.h>
#include <avkon.mbg>
#include <stringloader.h>
#include <e32std.h>
#include <flogger.h>
#include "Sup.rsg"
#include "sup.mbg"
#include "sup.hrh"

// Application views IDs etc
static const TUid KUidViewSupApp = {0xeb83db8a};
static const TUid KUidSupLoginController = {0x100039CE};
static const TUid KUidSupUploadController = {0xeb83db8a};
static const TUid KLoginViewId = {1};
static const TUid KMainViewId = {2};
static const TUid KUploadImagesViewId = {3};
static const TUid KFileBrowserViewId = {4};
static const TUid KImageCartViewId = {5};
static const TUid KSplashScreenViewId = {6};

// Application multibitmap file
_LIT( KSupMbm, "\\resource\\apps\\sup.mbm" );

// Login service provider settings filename
_LIT(KLoginProviderSettingsFilename, "supsettings.dat");

// Connection manager settings filename
_LIT(KConnManagerSettingsFilename, "supconnsettings.dat");

// Image cart storage file
_LIT(KImageCartSettingsFilename, "supimagecartsettings.dat");

// Web service end point
_LIT8(KServiceEndpointMemberId, "http://services.next2friends.com/N2FWebservices/memberservices.asmx");
_LIT8(KServiceEndpointPhotoOrganise, "http://services.next2friends.com/n2fwebservices/photoorganise.asmx");
_LIT8(KServiceXmlns, "http://tempuri.org/");

// SOAP action HTTP headers
_LIT8(KSoapActionMemberId, "http://tempuri.org/GetMemberID");
_LIT8(KSoapActionUploadPhoto, "http://tempuri.org/DeviceUploadPhoto");

// Literals concerning file browser engine
_LIT(KSlash, "\\");
_LIT(KPngExt, ".png");
_LIT(KJpgExt, ".jpg");

// Listbox item with icon pattern
_LIT(KStringIcon, "%d\t%S\t\t");

// Xml mime type
_LIT8(KXmlMimeType, "text/xml");

// Jpeg mime type
_LIT8(KJpegMimeType,"image/jpeg");

// Default threshold for the image size (64Kb)
const TInt KImageSizeThreshold(0x10000);

// Default image width threshold (640 px)
const TInt KImageWidthThreshold(640);
// Default image height threshold (480 px)
const TInt KImageHeightThreshold(480);

// General features
#define USE_FILEBROWSER_FILTER 1 // Defines whether filtering by files types is used in file browser engine
#define USE_SKIN_SUPPORT 1 // Defines whether skin support is enabled in the application
#define USE_MAINVIEW_LARGE_ICONS 0 // Defines whether we use large or small icons in main view listbox
#define USE_LOGGING 1 // Defines whether we use create log file

// Interface should be implemented by every user of any active object.
// It is called when async service provider completes the request
class MRequestObserver
{
public:
	virtual void HandleRequestCompletedL(const TInt& aError) = 0;
};

// Logging macros
_LIT( KLogsDir, "SupLogDirectory");  // Macros below work only if this folder exists under c:\logs
_LIT( KLogFileName, "log.txt");

#if USE_LOGGING
#define __LOGSTR_TOFILE(S)     {_LIT(KTmpStr, S);RFileLogger::WriteFormat(KLogsDir(), KLogFileName(), EFileLoggingModeAppend, KTmpStr());}
#define __LOGSTR_TOFILE1(S, P) {_LIT(KTmpStr, S);RFileLogger::WriteFormat(KLogsDir(), KLogFileName(), EFileLoggingModeAppend, TRefByValue<const TDesC>(KTmpStr()),P);}
#define __LOGSTR_TOFILE2(S, P0, P1) {_LIT(KTmpStr, S);RFileLogger::WriteFormat(KLogsDir(), KLogFileName(), EFileLoggingModeAppend, TRefByValue<const TDesC>(KTmpStr()),P0,P1);}
#else
#define __LOGSTR_TOFILE(S)     {}
#define __LOGSTR_TOFILE1(S, P) {}
#define __LOGSTR_TOFILE2(S, P0, P1) {}
#endif

#endif