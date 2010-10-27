// UidevtestppcView.h : interface of the CUidevtestppcView class
//
/////////////////////////////////////////////////////////////////////////////

#pragma once

#include <control-ids.h>

#include <imagebutton.h>
#include <imageholder.h>


class CUidevtestppcView : 
	public CWindowImpl<CUidevtestppcView>
{
public:
	DECLARE_WND_CLASS(NULL)

	BOOL PreTranslateMessage(MSG* pMsg);

	BEGIN_MSG_MAP(CUidevtestppcView)
		MESSAGE_HANDLER(WM_CREATE, OnCreate)
		MESSAGE_HANDLER(WM_SIZE, OnSize)
		MESSAGE_HANDLER(WM_PAINT, OnPaint)
		COMMAND_HANDLER(ID_BUTTON_TEST, BN_CLICKED, OnTestButtonClicked)
		REFLECT_NOTIFICATIONS()
	END_MSG_MAP()

// Handler prototypes (uncomment arguments if needed):
//	LRESULT MessageHandler(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/)
//	LRESULT CommandHandler(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/)
//	LRESULT NotifyHandler(int /*idCtrl*/, LPNMHDR /*pnmh*/, BOOL& /*bHandled*/)

	LRESULT OnCreate(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnSize(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);

	LRESULT OnPaint(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);

	LRESULT OnTestButtonClicked(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);

protected:

	CImageButton	iImageButton;
	CImageHolder	iImageHolder;

};

