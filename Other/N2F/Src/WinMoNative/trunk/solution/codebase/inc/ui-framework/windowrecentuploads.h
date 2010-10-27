#pragma once

#include <imagelistbox.h>

class CWindowRecentUploads:
				public CWindowBase
{
public:

	CWindowRecentUploads();
	virtual ~CWindowRecentUploads();

	DECLARE_WND_CLASS( NULL );

	BEGIN_MSG_MAP( CWindowRecentUploads )
		MESSAGE_HANDLER( WM_CREATE, OnCreate )
		MESSAGE_HANDLER( WM_SIZE, OnSize )
		MESSAGE_HANDLER( WM_PAINT, OnPaint )

		MESSAGE_HANDLER( WM_SETFOCUS, OnSetFocus )
		MESSAGE_HANDLER( WM_COMMAND, OnCommand )

		MESSAGE_HANDLER( WM_RECENTUPLOADS_REFRESHLIST, OnRefreshList )

		CHAIN_MSG_MAP( CWindowBase )

		REFLECT_NOTIFICATIONS()
	END_MSG_MAP()

	LRESULT OnCreate(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnSize(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnPaint(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnCommand(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnSetFocus(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);

	LRESULT OnRefreshList(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);

protected:

	virtual void GetMenuBarButtonsInfo( TMBBIType requestType, TMBBInfoStruct& info );
	virtual void GetPopupMenuItems( TPopupMenuItemsCollection& items );

	void	DoBackToPrevView();
	void	DoReUpload();

	void	UpdateListboxContent();
	void	UpdateListboxItemsArray();

	void	UninitializeListboxItemsArray();

	CImageListbox		iImgListbox;

	CFont				iFontBold;
	CFont				iFontSmall;

	CImageList			iImages;

	CSimpleArray< ILBITEM >	iListboxItems;

	UINT				iLBImageMaxWidth, iLBImageMaxHeight;

	virtual void	GetHelpCaption(CString& caption);
	virtual void	GetHelpText(CString& text);

};
