#pragma once

class CImageHolder;
class CImageButton;

#include <windowbase.h>

struct TSupportedOperation
{
	TControlID	idControl;
	TGraphicsID idIcon;
	TStringID	idCaption;
};

typedef CSimpleArray<TSupportedOperation> TSupportedOperationsList;

class CWindowOperations: 
		//public CWindowImpl<CWindowOperations>
		public CWindowBase,
		public WebServiceN2FSnapUpServiceDeviceUploadMethodDataProvider
{

public:

	CWindowOperations();

	virtual ~CWindowOperations();

	DECLARE_WND_CLASS(NULL)

	BEGIN_MSG_MAP( CWindowOperations )
		MESSAGE_HANDLER( WM_CREATE, OnCreate )
		MESSAGE_HANDLER( WM_SIZE, OnSize )
		MESSAGE_HANDLER( WM_PAINT, OnPaint )
		MESSAGE_HANDLER( WM_OPERATIONS_UPLOADFILE, OnUploadFile )
		COMMAND_CODE_HANDLER( BN_CLICKED, OnButtonClicked )
		COMMAND_ID_HANDLER( EMBBConfirmUploadYes, OnUploadConfirmResult )
		COMMAND_ID_HANDLER( EMBBConfirmUploadNo, OnUploadConfirmResult )

		MESSAGE_HANDLER( WM_COMMAND, OnCommand )
		

		CHAIN_MSG_MAP( CWindowBase )

		REFLECT_NOTIFICATIONS()
	END_MSG_MAP()

	virtual void GetUsername( CString& username );
	virtual void GetPassword( CString& password );

	virtual void GetFilePathToUpload( CString& filePath );
	virtual void GetTimeForUpload( SYSTEMTIME& st );

protected:

	LRESULT OnCreate(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnSize(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnPaint(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnButtonClicked( WORD wNotifyCode, WORD wID, HWND hWnd, BOOL& bHandled);
	LRESULT OnCommand(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);

	LRESULT OnUploadFile(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);

	LRESULT OnUploadConfirmResult(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
	


	void	GetSupportedOperationsList( TSupportedOperationsList& list );
	void	DeleteOperationButtons();
	void	CreateOperationButtons();

	void	OnOperationUpload();
	void	OnOperationCredentials();
	void	OnOperationRecentUploads();
	void	OnOperationExit();

	
	CSimpleArray< CImageButton* >	iOperationsButtons;

	

	HFONT				iCreatedFont;

	virtual void UpdateMenuBar();
	virtual void GetMenuBarButtonsInfo( TMBBIType requestType, TMBBInfoStruct& info );
	virtual void GetPopupMenuItems( TPopupMenuItemsCollection& items );

	bool	DoFileUpload();
	void	ShowNotification();

	CString	iFilePathToUpload;
	CString iFileTitleToUpload;
	SYSTEMTIME	iUploadTime;

	virtual void	GetHelpCaption(CString& caption);
	virtual void	GetHelpText(CString& text);

	CString iUsernameToPass;
	CString iPasswordToPass;

};
