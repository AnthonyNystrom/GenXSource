#include "stdafx.h"

#include <windowsmanager.h>

#include <windowsplash.h>
#include <windowcredentials.h>
#include <windowoperations.h>
#include <windowrecentuploads.h>

WindowsManager *WindowsManager::iManagerInstance = NULL;


WindowsManager::WindowsManager()
{
	
}

WindowsManager::~WindowsManager()
{
	for ( int i = 0; i < iWindows.GetSize(); ++i )
	{
		CWindowBase*& item = iWindows.GetValueAt( i );
		if ( NULL != item )
		{
			//item->DestroyWindow();
			delete item;
			item = NULL;
		}
	}

	iWindows.RemoveAll();
}

WindowsManager* WindowsManager::CreateInstance()
{
	if ( NULL != iManagerInstance )
		WindowsManager::DeleteInstance();

	iManagerInstance = new WindowsManager();
	ASSERT( NULL != iManagerInstance );
	return iManagerInstance;
}

WindowsManager* WindowsManager::GetInstnace()
{
	ASSERT( NULL != iManagerInstance );

	return iManagerInstance;
}

void WindowsManager::DeleteInstance()
{
	if ( NULL != iManagerInstance )
	{
		delete iManagerInstance;
		iManagerInstance = NULL;
	}
}

bool WindowsManager::InitializeManager( HWND hwndParent, RECT *pInitialRect )
{
	bool result = false;

	result = this->CreateAllKnownWindows( hwndParent, pInitialRect );

	iHwndParent = hwndParent;

	ASSERT( true == result );
	return result;
}

bool WindowsManager::CreateAllKnownWindows( HWND hwndParent, RECT *pInitialRect )
{
	bool result = true;
	HWND hwndResult = NULL;

	ASSERT( iWindows.GetSize() == 0 );

	LOGMSG("Initial window rect: %d, %d, %d, %d", pInitialRect->left, pInitialRect->top, 
		pInitialRect->right, pInitialRect->bottom );

	RECT rcWindow = *pInitialRect;
	RECT rcWindowSaved = rcWindow;

	iWindows.RemoveAll();

	CWindowSplash *wndSplash = new CWindowSplash();
	if ( wndSplash )
	{
		hwndResult = wndSplash->Create( hwndParent, &rcWindow, _T("N2F_WindowSplash"), WS_POPUP|WS_VISIBLE );
		ASSERT( hwndResult != NULL );
		if ( hwndResult != NULL )
		{
			iWindows.Add( EWndSplash, wndSplash );
		}
		else
		{
			result = false;
		}
	}

	rcWindow = rcWindowSaved;
	CWindowCredentials *wndCredentials = new CWindowCredentials();
	if ( wndCredentials )
	{
		hwndResult = wndCredentials->Create( hwndParent, &rcWindow, _T("N2F_WindowCredentials"), WS_CHILD|WS_VISIBLE|WS_CLIPSIBLINGS|WS_CLIPCHILDREN );
		ASSERT( hwndResult != NULL );
		if ( hwndResult != NULL )
		{
			iWindows.Add( EWndCredentials, wndCredentials );
		}
		else
		{
			result = false;
		}
	}
	
	rcWindow = rcWindowSaved;
	CWindowOperations *wndOperations = new CWindowOperations();
	if ( wndOperations )
	{
		::OffsetRect(&rcWindow, 0, -26);	// ??? - unknown reason offsets rect 26 pixs lower ;(
		hwndResult = wndOperations->Create( hwndParent, &rcWindow, _T("N2F_WindowOperations"), WS_CHILD|WS_VISIBLE|WS_CLIPSIBLINGS|WS_CLIPCHILDREN );
		ASSERT( hwndResult != NULL );
		if ( hwndResult != NULL )
		{
			iWindows.Add( EWndOperations, wndOperations );
		}
		else
		{
			result = false;
		}
	}

	rcWindow = rcWindowSaved;
	CWindowRecentUploads *wndRecentUploads = new CWindowRecentUploads();
	if ( wndRecentUploads )
	{
		::OffsetRect(&rcWindow, 0, -26);	// ??? - unknown reason offsets rect 26 pixs lower ;(
		hwndResult = wndRecentUploads->Create( hwndParent, &rcWindow, _T("N2F_WindowRecentUploads"), WS_CHILD|WS_VISIBLE|WS_CLIPSIBLINGS|WS_CLIPCHILDREN );
		ASSERT( hwndResult != NULL );
		if ( NULL != hwndResult )
		{
			iWindows.Add( EWndRecentUploads, wndRecentUploads );
		}
		else
		{
			result = false;
		}
	}

	for ( int i = 0; i < iWindows.GetSize(); ++i )
	{
		CWindowBase *item = iWindows.GetValueAt( i );
		item->SetWindowID( iWindows.GetKeyAt( i ) );

		TCHAR text[200] = {0};
		::GetWindowText( item->m_hWnd, text, 199 );
		LOGMSG( "Window text: %s, key: %d", text, item->GetWindowID() );
	}

	return result;
}

CWindowBase* WindowsManager::GetWindow( TWindowID id )
{
	CWindowBase* result = NULL;

	int idx = iWindows.FindKey( id );
	if ( -1 != idx )
	{
		result = iWindows.GetValueAt( idx );
	}

	ASSERT( NULL != result );
	return result;
}

void WindowsManager::ShowWindow( TWindowID id, int swCommand/* = SW_SHOW*/ )
{
	CWindowBase *wnd = this->GetWindow( id );

	if ( NULL != wnd )
	{
		LOGMSG("ShowWindow command for: %d", id);
		BOOL res = ::ShowWindow( wnd->m_hWnd, swCommand );
		wnd->Invalidate();
		wnd->UpdateWindow();

		if ( res != TRUE )
		{
			DWORD err = ::GetLastError();
			LOGMSG( "error: %d", err );
		}

		//ASSERT( res == TRUE );

		//wnd->BringWindowToTop();
		//wnd->SetFocus();
	}

	LOGMSG("ShowWindow done for id: %d", id);
}

void WindowsManager::HideWindow( TWindowID id )
{
	this->ShowWindow( id, SW_HIDE );
}

void WindowsManager::BringWindowToTop( TWindowID id )
{
	CWindowBase *wnd = this->GetWindow( id );

	if ( NULL != wnd )
	{
		BOOL res = ::BringWindowToTop( wnd->m_hWnd );

		if ( res != TRUE )
		{
			DWORD err = ::GetLastError();
			LOGMSG( "error: %d", err );
		}

		ASSERT( res == TRUE );

		::UpdateWindow( wnd->m_hWnd );
	}
}

void WindowsManager::SendMessageToWindows( UINT uMsg, WPARAM wParam, LPARAM lParam )
{
	LOGMSG("broadcasting msg %d", uMsg);
	for ( int i = 0; i < iWindows.GetSize(); ++i )
	{
		if ( iWindows.GetValueAt(i)->IsWindowVisible() )
			iWindows.GetValueAt(i)->SendMessage(uMsg, wParam, lParam);
	}
}