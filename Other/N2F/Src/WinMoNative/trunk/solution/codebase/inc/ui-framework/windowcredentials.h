#pragma once

//#include <imagetextbox.h>
#include <imageholder.h>

//#include <windowsplash.h>
#include <windowbase.h>

class CWindowCredentials: 
			//public CWindowImpl<CWindowCredentials>
			public CWindowBase,
			public WebServiceN2FMemberServiceLoginMethodDataProvider
{
public:
	
	CWindowCredentials();
	virtual ~CWindowCredentials();

	DECLARE_WND_CLASS(NULL);

	BOOL PreTranslateMessage(MSG *pMsg);

	BEGIN_MSG_MAP( CWindowCredentials )
		MESSAGE_HANDLER( WM_CREATE, OnCreate )
		MESSAGE_HANDLER( WM_SIZE, OnSize )
		MESSAGE_HANDLER( WM_PAINT, OnPaint )

		COMMAND_CODE_HANDLER( EN_SETFOCUS, OnEditFocusStateChanged )
		COMMAND_CODE_HANDLER( EN_KILLFOCUS, OnEditFocusStateChanged )
		MESSAGE_HANDLER( WM_CTLCOLOREDIT, OnCtlColorEdit )
		MESSAGE_HANDLER( WM_CTLCOLORSTATIC, OnCtlColorStatic )

		MESSAGE_HANDLER( WM_SETFOCUS, OnSetFocus )

		MESSAGE_HANDLER( WM_COMMAND, OnCommand )

		CHAIN_MSG_MAP( CWindowBase )
		
		REFLECT_NOTIFICATIONS()

		

	END_MSG_MAP()

	virtual void GetUsername( CString& username );
	virtual void GetPassword( CString& password );

	

protected:

	IMAGE_KEY_TYPE	GetImageKeyForBack( CEdit *pEditControl );

	LRESULT OnCreate(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnSize(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnPaint(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnCtlColorEdit(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnCtlColorStatic(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnEditFocusStateChanged(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);

	LRESULT OnSetFocus(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnCommand(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);

	bool	DoLogin();
	

	CImageHolder	iTextboxNameFrame;
	CImageHolder	iTextboxPassFrame;

	CEdit			iTextboxName;
	CEdit			iTextboxPass;

	CStatic			iLabelName;
	CStatic			iLabelPass;

	IMAGE_KEY_TYPE	iFocusedBack;
	IMAGE_KEY_TYPE	iUnfocusedBack;

	CBrush			iFocusedBrush;
	CBrush			iUnfocusedBrush;

	CPen			iLabelColoredPen;
	COLORREF		iLabelTextColor;

	HFONT	iFontTextboxes;
	bool	iSplashLaunched;

	//CWindowSplash	iWndSplash;

	virtual void UpdateMenuBar();
	virtual void GetMenuBarButtonsInfo( TMBBIType requestType, TMBBInfoStruct& info );
	virtual void GetPopupMenuItems( TPopupMenuItemsCollection& items );

	virtual void	GetHelpCaption(CString& caption);
	virtual void	GetHelpText(CString& text);

	CString iUsernameToPass;
	CString iPasswordToPass;

	

};
