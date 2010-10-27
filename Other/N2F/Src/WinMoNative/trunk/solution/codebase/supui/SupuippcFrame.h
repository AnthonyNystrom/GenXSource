// SupuippcFrame.h : interface of the CSupuippcFrame class
//
/////////////////////////////////////////////////////////////////////////////

#pragma once

class CSupuippcFrame : 
	public CFrameWindowImpl<CSupuippcFrame>, 
	public CUpdateUI<CSupuippcFrame>,
	public CAppWindow<CSupuippcFrame>,
	public CMessageFilter, public CIdleHandler
{
public:
	DECLARE_APP_FRAME_CLASS(NULL, IDR_MAINFRAME, L"Software\\WTL\\Supuippc")

	virtual BOOL PreTranslateMessage(MSG* pMsg);

// CAppWindow operations
	bool AppHibernate( bool bHibernate);

	bool AppNewInstance( LPCTSTR lpstrCmdLine);

	void AppSave();

#ifdef WIN32_PLATFORM_WFSP
	void AppBackKey();
#endif

	virtual BOOL OnIdle();

	BEGIN_UPDATE_UI_MAP(CSupuippcFrame)
	END_UPDATE_UI_MAP()

	BEGIN_MSG_MAP(CSupuippcFrame)
		MESSAGE_HANDLER(WM_CREATE, OnCreate)
		MESSAGE_HANDLER(WM_COMMAND, OnCommand)
		CHAIN_MSG_MAP(CAppWindow<CSupuippcFrame>)
		CHAIN_MSG_MAP(CUpdateUI<CSupuippcFrame>)
		CHAIN_MSG_MAP(CFrameWindowImpl<CSupuippcFrame>)
	END_MSG_MAP()

// Handler prototypes (uncomment arguments if needed):
//	LRESULT MessageHandler(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/)
//	LRESULT CommandHandler(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
//	LRESULT NotifyHandler(int /*idCtrl*/, LPNMHDR /*pnmh*/, BOOL& /*bHandled*/)

	LRESULT OnCreate(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/);

	LRESULT OnCommand(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);

	
};
