// UidevtestppcFrame.h : interface of the CUidevtestppcFrame class
//
/////////////////////////////////////////////////////////////////////////////

#pragma once



class CUidevtestppcFrame : 
	public CFrameWindowImpl<CUidevtestppcFrame>, 
	public CUpdateUI<CUidevtestppcFrame>,
	public CAppWindow<CUidevtestppcFrame>,
	public CMessageFilter, public CIdleHandler
{
public:

	DECLARE_APP_FRAME_CLASS(NULL, IDR_MAINFRAME, L"Software\\WTL\\Uidevtestppc")

	CUidevtestppcFrame();
	virtual ~CUidevtestppcFrame();

	CUidevtestppcView m_view;
	//CWindowOperations m_wndOperations;
	CWindowCredentials m_wndCredentials;

	virtual BOOL PreTranslateMessage(MSG* pMsg);

// CAppWindow operations
	bool AppHibernate( bool bHibernate);

	bool AppNewInstance( LPCTSTR lpstrCmdLine);

	void AppSave();

#ifdef WIN32_PLATFORM_WFSP
	void AppBackKey();
#endif

	virtual BOOL OnIdle();

	BEGIN_UPDATE_UI_MAP(CUidevtestppcFrame)
	END_UPDATE_UI_MAP()

	BEGIN_MSG_MAP(CUidevtestppcFrame)
		MESSAGE_HANDLER(WM_CREATE, OnCreate)
		MESSAGE_HANDLER( WM_COMMAND, OnCommand )
		COMMAND_ID_HANDLER(ID_APP_EXIT, OnFileExit)
		COMMAND_ID_HANDLER(ID_FILE_NEW, OnFileNew)
		COMMAND_ID_HANDLER(ID_ACTION, OnAction)
		COMMAND_ID_HANDLER(ID_APP_ABOUT, OnAppAbout)
		CHAIN_MSG_MAP(CAppWindow<CUidevtestppcFrame>)
		CHAIN_MSG_MAP(CUpdateUI<CUidevtestppcFrame>)
		CHAIN_MSG_MAP(CFrameWindowImpl<CUidevtestppcFrame>)
	END_MSG_MAP()

// Handler prototypes (uncomment arguments if needed):
//	LRESULT MessageHandler(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/)
//	LRESULT CommandHandler(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
//	LRESULT NotifyHandler(int /*idCtrl*/, LPNMHDR /*pnmh*/, BOOL& /*bHandled*/)

	LRESULT OnCreate(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/);
	LRESULT OnCommand(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);

	LRESULT OnFileExit(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);

	LRESULT OnFileNew(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);

	LRESULT OnAction(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);

	LRESULT OnAppAbout(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);

};
