// UidevtestppcView.cpp : implementation of the CUidevtestppcView class
//
/////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#ifdef WIN32_PLATFORM_PSPC
#include "resourceppc.h"
#else
#include "resourcesp.h"
#endif


#include "UidevtestppcView.h"

BOOL CUidevtestppcView::PreTranslateMessage(MSG* pMsg)
{
	pMsg;
	return FALSE;
}

LRESULT CUidevtestppcView::OnCreate( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	iImageButton.Create( m_hWnd, NULL, _T("wtlImageButtonTest"), WS_CHILD|WS_VISIBLE, NULL, ID_BUTTON_TEST );
	iImageButton.SetWindowText(_T("I'm image button!"));
	iImageButton.SetBorderStyle( IBS_3D_DOUBLEBORDER );

	CString csImage("\\temp\\settings.png");
	IMAGE_KEY_TYPE imageKey = INVALID_IMAGE_KEY_VALUE;
	ImageHelper::GetInstance()->GetImageFromFile(csImage, imageKey);

	ASSERT( INVALID_IMAGE_KEY_VALUE != imageKey );

	iImageButton.SetImageKey( imageKey );

	csImage = _T("\\temp\\commercelogo.png");
	IMAGE_KEY_TYPE logoKey = INVALID_IMAGE_KEY_VALUE;
	ImageHelper::GetInstance()->GetImageFromFile(csImage, logoKey);

	ASSERT( INVALID_IMAGE_KEY_VALUE != logoKey );

	iImageHolder.Create( m_hWnd, NULL, _T("wtImageHolderTest"), WS_CHILD|WS_VISIBLE, NULL, ID_IMAGEHOLDER_TEST );
	iImageHolder.SetImageHolderStyle( IHS_FIT_HOLDER_FOR_IMAGE );
	iImageHolder.SetImageKey( logoKey );

	return TRUE;
}

LRESULT CUidevtestppcView::OnSize( UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled )
{
	RECT rc;

	GetClientRect(&rc);

	RECT rcButton = rc;
	//rcButton.left += 20;
	//rcButton.right -= 20;
	//rcButton.top += 20;
	//rcButton.bottom = rcButton.top + 50;

	::InflateRect( &rcButton, -20, -60 );
	::OffsetRect( &rcButton, 0, 50 );

	iImageButton.MoveWindow(&rcButton);

	RECT rcHolder = {0};
	iImageHolder.GetWindowRect(&rcHolder);

	int dx = (rc.right - rc.left)/2 - iImageHolder.GetImageWidth()/2;
	::OffsetRect( &rcHolder, dx, 10);

	return 0;
}

LRESULT CUidevtestppcView::OnPaint(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& /*bHandled*/)
{
	CPaintDC dc(m_hWnd);

	//TODO: Add your drawing code here

	return 0;
}

LRESULT CUidevtestppcView::OnTestButtonClicked( WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled )
{
	ATLTRACE(_T("OnTestButtonClicked - check passed!\n"));

	bHandled = TRUE;
	return 0;
}
