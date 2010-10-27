#pragma once

#include <windowbase.h>

class CImageHolder;

class CWindowSplash:
			//public CWindowImpl<CWindowSplash>
			public CWindowBase
{
public:

	CWindowSplash();

	DECLARE_WND_CLASS(NULL)

	BEGIN_MSG_MAP( CWindowSplash )
		MESSAGE_HANDLER( WM_CREATE, OnCreate )
		MESSAGE_HANDLER( WM_SIZE, OnSize )
		MESSAGE_HANDLER( WM_PAINT, OnPaint )
		MESSAGE_HANDLER( WM_TIMER, OnTimer )
		MESSAGE_HANDLER( WM_SETFOCUS, OnSetFocus )

		CHAIN_MSG_MAP( CWindowBase )

		REFLECT_NOTIFICATIONS()
	END_MSG_MAP()

private:

	LRESULT OnCreate(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnSize(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnPaint(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnTimer(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnSetFocus(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);

	CImageHolder	iSplashImage;

	UINT			iTimerHandle;

	bool			iWasLaunched;

	virtual void	GetHelpCaption(CString& caption);
	virtual void	GetHelpText(CString& text);

	//virtual void GetMenuBarButtonsInfo( TMBBIType requestType, TMBBInfoStruct& info );

};
