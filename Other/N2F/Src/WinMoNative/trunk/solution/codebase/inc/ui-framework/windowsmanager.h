#pragma once

#include <windowbase.h>

class WindowsManager
{
private:
	WindowsManager();
	virtual ~WindowsManager();

public:

	static WindowsManager*	CreateInstance();
	static WindowsManager*	GetInstnace();
	static void	DeleteInstance();

	bool	InitializeManager( HWND hwndParent, RECT *pInitialRect );

	CWindowBase*	GetWindow( TWindowID id );

	void			ShowWindow( TWindowID id, int swCommand = SW_SHOW );
	void			HideWindow( TWindowID id );

	void			BringWindowToTop( TWindowID id );

	void			SendMessageToWindows( UINT uMsg, WPARAM wParam, LPARAM lParam );

private:

	static	WindowsManager*	iManagerInstance;

	bool	CreateAllKnownWindows( HWND hwndParent, RECT *pInitialRect );

	CSimpleMap< TWindowID, CWindowBase* >	iWindows;

	HWND	iHwndParent;


};

