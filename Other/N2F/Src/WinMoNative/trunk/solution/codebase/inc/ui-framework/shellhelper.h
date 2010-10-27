#pragma once

class ShellHelper
{
public:

	static	void	ShowErrorMessage(CString& caption, CString& text, HWND hWndParent = NULL);
	static	void	ShowMessage(CString& caption, CString& text, HWND hWndParent = NULL);

	

	static	void	ShowNotification(TShellNotificationID id, DWORD dwDurationInSeconds,
		HWND hwndSink, CString& body, CString& caption);
};
