#include "stdafx.h"

#include <shellhelper.h>


void ShellHelper::ShowErrorMessage( CString& caption, CString& text, HWND hWndParent /*= NULL*/ )
{
	::MessageBox( hWndParent, text, caption, MB_ICONERROR|MB_OK );
}

void ShellHelper::ShowMessage( CString& caption, CString& text, HWND hWndParent /*= NULL*/ )
{
	::MessageBox( hWndParent, text, caption, MB_OK );
}

void ShellHelper::ShowNotification( TShellNotificationID id, DWORD dwDurationInSeconds, HWND hwndSink, CString& body, CString& caption )
{
	LOGME();

	SHNOTIFICATIONDATA nd = {0};
	nd.cbStruct = sizeof(nd);
	nd.dwID = id;
	nd.npPriority = SHNP_INFORM;
	nd.csDuration = dwDurationInSeconds;
	nd.grfFlags = SHNF_FORCEMESSAGE|SHNF_DISPLAYON;
	nd.hwndSink = hwndSink;
	nd.pszHTML = body;
	nd.pszTitle = caption;

	if ( ERROR_SUCCESS != ::SHNotificationAdd( &nd ) )
	{
		DWORD err = ::GetLastError();
		LOGMSG("notification id: %d -> error: %d", id, err);
		ASSERT( FALSE );
	}
}
