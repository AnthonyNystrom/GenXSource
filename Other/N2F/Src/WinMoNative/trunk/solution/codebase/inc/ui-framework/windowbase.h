#pragma once

enum TMBBIType 
{
	EBITUnknown = 0,

	EBITLeftButton,
	EBITRightButton
};

struct TMBBInfoStruct
{
	TMBBInfoStruct()
	{
		biButtonID = EMBBUnknown;
	}

	CString				biButtonName;
	TMenuBarButtonID	biButtonID;

};

struct TPopupMenuItem
{
	UINT	id;
	bool	isSeparator;
	CString	caption;

};

typedef CSimpleArray<TPopupMenuItem>	TPopupMenuItemsCollection;

class CWindowBase: public CWindowImpl< CWindowBase >
{
public:

	CWindowBase();

	virtual ~CWindowBase();
	virtual BOOL	PreTranslateMessage(MSG *pMsg);

	BEGIN_MSG_MAP( CWindowBase )
		MESSAGE_HANDLER( WM_CREATE, OnCreate )
		MESSAGE_HANDLER( WM_PAINT, OnPaint )
		MESSAGE_HANDLER( WM_SETFOCUS, OnSetFocus )
		
	END_MSG_MAP()

	LRESULT OnCreate(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnPaint(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnSetFocus(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);

	void		SetWindowID( TWindowID id );
	TWindowID	GetWindowID();

protected:

	virtual void UpdateMenuBar();
	virtual void GetMenuBarButtonsInfo( TMBBIType requestType, TMBBInfoStruct& info );
	virtual void GetPopupMenuItems( TPopupMenuItemsCollection& items );

	bool IsLeftButton( int id );

	virtual void	DoExitApplication();
	virtual void	DoHelp();

	virtual void	GetHelpCaption(CString& caption) = 0;
	virtual void	GetHelpText(CString& text) = 0;

private:

	void CreateEmptyMenuBar();
	void UpdateMenuBarButton( HWND hMenuBar, int id, TMBBInfoStruct& btnInfo );


	TWindowID		iWindowID;

	int			iLeftButtonID;
	int			iRightButtonID;
};
