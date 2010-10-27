#include "stdafx.h"

#include <windowsplash.h>

#define	SPLASH_WND_TIMER_ELAPSE	(2000)

CWindowSplash::CWindowSplash()
{
	this->SetWindowID( EWndSplash );
	iTimerHandle = 0;
}



LRESULT CWindowSplash::OnCreate( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	RECT rcClient = {0};
	GetClientRect( &rcClient );

	iSplashImage.Create( m_hWnd, &rcClient, NULL, WS_CHILD|WS_VISIBLE, 0, ID_IMAGEHOLDER_SPLASH_IMAGE );
	iSplashImage.SetImageKey( ControllersHost::GetInstance()->ConfigController()->ImageKey( EGCommerceLogo ) );
	iSplashImage.SetImageHolderStyle( IHS_FIT_HOLDER_FOR_IMAGE );

	iWasLaunched = false;

	return TRUE;
}

LRESULT CWindowSplash::OnSize( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	RECT rcClient = {0};
	GetClientRect( &rcClient );

	RECT rcHolder = rcClient;

	rcHolder.left = rcHolder.left + (rcClient.right - rcClient.left)/2 - iSplashImage.GetImageWidth()/2;
	rcHolder.right = rcHolder.left + iSplashImage.GetImageWidth();
	rcHolder.top = rcHolder.top + (rcClient.bottom - rcClient.top)/2 - iSplashImage.GetImageHeight()/2;
	rcHolder.bottom = rcHolder.top + iSplashImage.GetImageHeight();

	iSplashImage.MoveWindow( &rcHolder );

	return 0;
}

LRESULT CWindowSplash::OnPaint( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	CPaintDC dc(m_hWnd);

	SetBkMode( dc, TRANSPARENT );

	RECT rcClient;
	GetClientRect( &rcClient );

	dc.Rectangle( &rcClient );

	return 0;
}

LRESULT CWindowSplash::OnSetFocus( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	if ( (false == iWasLaunched) && ::GetFocus() == m_hWnd )
	{
		LOGMSG("launching...");
		if ( 0 != iTimerHandle )
		{
			KillTimer( iTimerHandle );
			iTimerHandle = 0;
		}

		//SetWindowPos( HWND_TOP, 0, 0, 0, 0, SWP_NOACTIVATE|SWP_NOMOVE|SWP_NOSIZE );

		iTimerHandle = SetTimer( TID_SPLASH_WINDOW, SPLASH_WND_TIMER_ELAPSE, NULL );
		LOGMSG("timer launched %d", iTimerHandle);
		iWasLaunched = true;
	}

	bHandled = FALSE;

	return 0;
}

LRESULT CWindowSplash::OnTimer( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	TTimerID timerID = (TTimerID)wParam;

	if ( TID_SPLASH_WINDOW == timerID )
	{
		LOGMSG("Splash time-outed!");

		this->KillTimer( iTimerHandle );
		iTimerHandle = 0;

		this->ShowWindow( SW_HIDE );
	}

	return 0;
}

void CWindowSplash::GetHelpCaption( CString& caption )
{
	caption.Empty();
}

void CWindowSplash::GetHelpText( CString& text )
{
	text.Empty();
}

