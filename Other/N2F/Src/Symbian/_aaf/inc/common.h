#ifndef __AAFCOMMON_H__
#define __AAFCOMMON_H__

#include <avkon.rsg>
#include <akndef.h>
#include <aknconsts.h>
#include <avkon.mbg>
#include <stringloader.h>
#include <e32std.h>
#include <flogger.h>
#include <aaf.rsg>
#include <aaf.hrh>
#include <aaf.pan>
#include <aaf.mbg>

// Application views IDs etc
static const TUid KUidViewSupApp = {0xe2536f82};
static const TUid KUidAafLoginController = {0x100039CE};
static const TUid KUidAafUploadController = {0xe2536f82};
static const TUid KUidAafPQuestionsController = {0xe2536f85};
static const TUid KUidAafUserQuestionsController = {0xe2536f86};
static const TUid KLoginViewId = {1};
static const TUid KMainViewId = {2};
static const TUid KUploadImagesViewId = {3};
static const TUid KFileBrowserViewId = {4};
static const TUid KImageCartViewId = {5};
static const TUid KPQuestionsViewId = {6};
static const TUid KMyQuestionsViewId = {7};
static const TUid KQuestionCommentsViewId = {8};
static const TUid KSplashScreenViewId = {9};
static const TUid KCameraViewId = {10};


// Application multibitmap file
_LIT( KAafMbm, "\\resource\\apps\\aaf.mbm" );

// Snap image camera sound
_LIT(KSnapSoundFile, "Camera_2_8kHz.wav");

// Login service provider settings filename
_LIT(KLoginProviderSettingsFilename, "aafsettings.dat");

// Connection manager settings filename
_LIT(KConnManagerSettingsFilename, "aafconnsettings.dat");

// Image cart storage file
_LIT(KImageCartSettingsFilename, "aafimagecartsettings.dat");

// For 2nd Edition emulator only
#ifdef __WINS__
	_LIT(KEmulatorPath, "c:\\system\\Apps\\aaf\\");
#endif

// N2F Ask a friend web-service end point
const char N2F_ASKAFRIEND_ENDPOINT[] = "http://services.next2friends.com/n2fwebservices/askafriend.asmx";

// Web service end point
_LIT8(KServiceEndpointMemberId, "http://services.next2friends.com/N2FWebservices/memberservices.asmx");
_LIT8(KServiceEndpointPhotoOrganise, "http://services.next2friends.com/n2fwebservices/photoorganise.asmx");
_LIT8(KServiceEndpointAAF, "http://services.next2friends.com/n2fwebservices/askafriend.asmx");
_LIT8(KServiceXmlns, "http://tempuri.org/");

// SOAP action HTTP headers
_LIT8(KSoapActionMemberId, "http://tempuri.org/GetMemberID");
_LIT8(KSoapActionPrivateQuestions, "http://tempuri.org/GetPrivateAAFQuestion");
_LIT8(KSoapActionMyQuestions, "http://tempuri.org/GetMyAAFQuestions");
_LIT8(KSoapActionComments, "http://tempuri.org/GetAAFComments");
_LIT8(KSoapActionSubmitQuestion, "http://tempuri.org/SubmitQuestion");
_LIT8(KSoapActionAttachPhoto, "http://tempuri.org/AttachPhoto");
_LIT8(KSoapActionUploadPhoto, "http://tempuri.org/DeviceUploadPhoto");

// Literals concerning file browser engine
_LIT(KSlash, "\\");
_LIT(KPngExt, ".png");
_LIT(KJpgExt, ".jpg");

// Date string format
_LIT(KDateString, "%D%M%Y%/0%1%/1%2%/2%3%/3 %-B%:0%J%:1%T%:2%S%:3%+B");

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

// File browser
// Icon and name (should be add later)
_LIT(KListboxWithIconPattern, "%d\t%S\t\t");

_LIT(KListboxWithoutIconPattern, "\t%S\t\t");

_LIT(KDoubleListboxWithoutIconPattern, "\t%S\t%S %S\t");

_LIT(KDoubleListboxWithIconPattern, "%d\t%S\t%S %S\t");

// Upper level
_LIT(KStringUp, "%d\t...\t\t");


// General features
#define USE_FILEBROWSER_FILTER 1 // Defines whether filtering by files types is used in file browser engine
#define USE_SKIN_SUPPORT 1 // Defines whether skin support is enabled in the application
#define USE_MAINVIEW_LARGE_ICONS 0 // Defines whether we use large or small icons in main view listbox

// Defines whether on-board camera functionality is included (for device target only)
#ifdef __WINSCW__
	#define USE_CAMERA 0
#else
	#define USE_CAMERA 1
#endif

#define USE_LOGGING 1 // Defines whether we use create log file

// Interface should be implemented by every user of any active object.
// It is called when async service provider completes the request
class MRequestObserver
{
public:
	virtual void HandleRequestCompletedL(const TInt& aError) = 0;
};

// Currently used camera
enum TActiveCamera
{
	KNoneCamera = -1,
	KMainCamera,
	KSecondaryCamera
};

// Defines question description
class _ns1__QuestionData
{
public:
	// Default constructor
	_ns1__QuestionData()
	{
		iQuestionText.Zero();
		iQuestionText = KNullDesC;
		iCustomResponseA = KNullDesC;
		iCustomResponseB = KNullDesC;
		iQuestionType = 0;
		iQuestionDuration = 0;
		iPrivateMark = EFalse;
	}

	// Constructor with params
	_ns1__QuestionData(const TDesC &aQuestionText, TInt aQuestionType, TInt aQuestionDuration, TBool aPrivateMark, const TDesC &aCustomResponseA = KNullDesC, const TDesC &aCustomResponseB = KNullDesC)
	{
		// Question text
		iQuestionText = aQuestionText;

		// Question type
		iQuestionType = aQuestionType;

		// Question duration
		iQuestionDuration = aQuestionDuration;

		// Private mark
		iPrivateMark = aPrivateMark;

		// Custom responses if required
		iCustomResponseA = aCustomResponseA;

		iCustomResponseB = aCustomResponseB;
	}

	// Data members
	TBuf<256> iQuestionText; // Question text value

	TBuf<256> iCustomResponseA; // Custom response A, if required

	TBuf<256> iCustomResponseB; // Custom response B, if required

	TInt iQuestionType; // Index of question type pop up index

	TInt iQuestionDuration; // Index of question duration pop up index

	TBool iPrivateMark; // Indicates whether private mark is used
};

// Thread entry point argument interface (used in gSOAP threads etc)

// FORWARD DECLARATION
class CAafLoginServiceProvider;
class CAafConnectionManager;
class RSocket;

class MGSoapData
{
public:
	virtual CAafLoginServiceProvider* GetLoginProvider() const = 0;

	virtual CAafConnectionManager* GetConnectionManager() const = 0;

	virtual RSocket* GetSocketInstance() = 0;
};

// Logging macros
_LIT( KLogsDir, "AafLogDirectory");  // Macros below work only if this folder exists under c:\logs
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

#endif // __AAFCOMMON_H__