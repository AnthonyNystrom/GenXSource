#pragma once

enum	TTimerID
{
	TID_BASE_VALUE			= 10000,

	TID_SPLASH_WINDOW,

};

enum	TWindowID
{
	EWndUnknown		= 0,

	EWndSplash,
	EWndCredentials,
	EWndOperations,
	EWndRecentUploads
};

enum	TMenuBarButtonID
{
	EMBBUnknown		= 0x2100,

	EMBBSplashLeft,
	EMBBSplashRight,
	EMBBCredentialsMenu,
	EMBBCredentialsLogin,
	EMBBOperationsLeft,
	EMBBOperationsMenu,
	EMBBConfirmUploadYes,
	EMBBConfirmUploadNo,
	EMBBRecentUploadsBack,
	EMBBRecentUploadsMenu,

};

enum	TMenuItemID
{
	EMIUnknown		= 0x1000,

	EMICredentialsHelp,
	EMICredenitalsSeparator1,
	EMICredentialsExitApplication,

	EMIOperationsHelp,
	EMIOperationsSeparator1,
	EMIOperationsExitApplication,

	EMIRUploadsHelp,
	EMIRUploadsSeparator1,
	EMIRUploadsUpload,
	EMIRUploadsExitApplication,
};

enum	TShellNotificationID
{
	ESNUnknown		= 0x4000,

	ESNConfirmUpload,
	ESNHelpInformation
};

