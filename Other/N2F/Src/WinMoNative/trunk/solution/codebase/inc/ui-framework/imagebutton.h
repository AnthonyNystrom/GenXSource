#pragma once

//#include <imagehelper.h>

#define IBS_NO_3D	0x0000
#define	IBS_3D_BORDER	0x0001
#define IBS_3D_DOUBLEBORDER	0x0002

template< class T, class TBase = CButton, class TWinTraits = CControlWinTraits>
class CImageButtonImpl:
	public CWindowImpl< T, TBase, TWinTraits >,
	public COwnerDraw< T >
{
public:
	typedef CImageButtonImpl< T, TBase, TWinTraits > thisClass;

	DECLARE_WND_SUPERCLASS( NULL, TBase::GetWndClassName() )

	BOOL SubclassWindow(HWND hWnd)
	{
		BOOL bRet = CWindowImpl< T, TBase, TWinTraits >::SubclassWindow(hWnd);
		if ( bRet )
			_Init();
		return bRet;
	}

	void SetButtonTextColor(COLORREF clrBtnText, COLORREF clrAltBtnText = clrBtnText )
	{
		ATLASSERT(::IsWindow(m_hWnd));
		iClrText = clrBtnText;
		iClrAltText = clrAltBtnText;
		Invalidate();
	}

	void SetImageKey( IMAGE_KEY_TYPE key )
	{
		ATLASSERT( ::IsWindow(m_hWnd) );
		iUsedImageKey = key;
		ImageHelper::GetInstance()->GetImageSize( iUsedImageKey, iUsedImageSize );
		Invalidate();
	}

	void SetImageOffset( int offX )
	{
		ATLASSERT( ::IsWindow(m_hWnd) );
		iOffsetX = offX;
		Invalidate();
	}

	void SetBackImageKey( IMAGE_KEY_TYPE key, IMAGE_KEY_TYPE altKey = INVALID_IMAGE_KEY_VALUE )
	{
		ATLASSERT( ::IsWindow(m_hWnd) );
		iBackImageKey = key;
		iAltBackImageKey = altKey;
		ImageHelper::GetInstance()->GetImageSize( iBackImageKey, iBackImageSize );
		ImageHelper::GetInstance()->GetImageSize( iAltBackImageKey, iAltBackImageSize );
		Invalidate();
	}

	int GetBackImageHeight()
	{
		ATLASSERT( ::IsWindow(m_hWnd) );
		return iBackImageSize.cy;
	}

	void SetBorderStyle( int borderStyle = IBS_NO_3D )
	{
		iBorderStyle = borderStyle;
		if ( ::IsWindow(m_hWnd) )
			Invalidate();
	}

	void SetShouldBoldTextOnFocus(BOOL shouldBold = TRUE)
	{
		ATLASSERT( ::IsWindow(m_hWnd) );
		iShouldBoldTextOnFocus = shouldBold;

		_UpdateCurrentFont(IsFocused());

		Invalidate();
	}
	

	BEGIN_MSG_MAP(thisClass)
		MESSAGE_HANDLER(WM_CREATE, OnCreate)
		MESSAGE_HANDLER(WM_LBUTTONDBLCLK, OnLButtonDblClick)
		MESSAGE_HANDLER(WM_SETFOCUS, OnFocus)
		MESSAGE_HANDLER(WM_KILLFOCUS, OnFocus)
		REFLECTED_COMMAND_CODE_HANDLER(BN_CLICKED, OnClicked)
		CHAIN_MSG_MAP_ALT( COwnerDraw< T >, 1 )
		DEFAULT_REFLECTION_HANDLER()
	END_MSG_MAP()

	BOOL IsFocused()
	{
		ATLASSERT( ::IsWindow(m_hWnd) );
		return ( (m_hWnd == ::GetFocus())? TRUE: FALSE );
	}

	LRESULT OnCreate(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
	{
		LRESULT lRes = DefWindowProc();
		_Init();

		return lRes;
	}

	LRESULT OnFocus(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
	{
		LRESULT lRes = DefWindowProc();
		if ( iShouldBoldTextOnFocus )
		{
			_UpdateCurrentFont( (WM_SETFOCUS == uMsg)? TRUE: FALSE );
		}
		return 0;
	}

	LRESULT OnLButtonDblClick(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
	{
		return DefWindowProc(WM_LBUTTONDOWN, wParam, lParam);
	}

	LRESULT OnClicked(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
	{
		//ATLTRACE(_T("CImageButton::OnClicked()\n"));

		bHandled = FALSE;
		return 0;
	}

	// Owner draw methods

	void DrawItem( LPDRAWITEMSTRUCT lpDIS )
	{
		CDCHandle dc = lpDIS->hDC;
		RECT rc = lpDIS->rcItem;
		UINT& state = lpDIS->itemState;

		bool isPressed = (0 != (state & ODS_SELECTED));
		bool isFocused = (0 != (state & ODS_FOCUS));
		//bool isFocused = (this->IsFocused() == TRUE) ;
		bool isDisabled = (0 != (state & ODS_DISABLED));
		
		if ( iBorderStyle & IBS_3D_BORDER)
			dc.DrawEdge(&rc, isPressed? BDR_SUNKENOUTER: BDR_RAISEDINNER, BF_RECT);
		else if ( iBorderStyle & IBS_3D_DOUBLEBORDER )
			dc.DrawEdge(&rc, isPressed? BDR_SUNKEN: BDR_RAISED, BF_RECT);
		else	// no border
			dc.Rectangle(&rc);

		IMAGE_KEY_TYPE keyTemp = INVALID_IMAGE_KEY_VALUE;
		if ( false == isFocused )
		{
			keyTemp = iBackImageKey;
		}
		else
		{
			keyTemp = iAltBackImageKey;
		}

		if ( INVALID_IMAGE_KEY_VALUE != keyTemp )
			_DrawImage( dc, keyTemp, rc );

		if ( isPressed )
		{
			//RECT rcTemp = rc;
			//::InflateRect(&rcTemp, -2 * ::GetSystemMetrics(SM_CXEDGE), -2 * ::GetSystemMetrics(SM_CYEDGE));
			//dc.DrawFocusRect( &rcTemp );

			// TODO: draw pressed state
		}

		RECT rcText = rc;
		RECT rcIcon = {0};

		_GetImageDrawingRect( dc, iUsedImageKey, rcIcon );

		_DrawImage(dc, iUsedImageKey, rcIcon);

		//rcText.left = rcIcon.right + 15;
		rcText.left = 40;

		int textLength = GetWindowTextLength();
		TCHAR *buttonText = new TCHAR[textLength+1];

		if ( NULL == buttonText )
		{
			return;
		}

		ZeroMemory( buttonText, sizeof(TCHAR)*(textLength + 1));
		GetWindowText(buttonText, textLength + 1);

		if ( _tcslen(buttonText) > 0 )
		{
			dc.SetBkMode(TRANSPARENT);
			HFONT hOldFont = dc.SelectFont(GetFont());
			COLORREF clrToSet;
			if ( false == isDisabled )
			{
				if ( false == isFocused )
					clrToSet = iClrText;
				else
					clrToSet = iClrAltText;
			}
			else
				clrToSet = ::GetSysColor(COLOR_GRAYTEXT);

			COLORREF oldColor = dc.SetTextColor(clrToSet);

			//dc.DrawText( buttonText, -1, &rcText, DT_CENTER|DT_VCENTER|DT_SINGLELINE );1
			dc.DrawText( buttonText, -1, &rcText, DT_VCENTER|DT_SINGLELINE );


			dc.SetTextColor(oldColor);
			dc.SelectFont(hOldFont);

		}

		delete [] buttonText;
	}

protected:

	int				iBorderStyle;

	COLORREF		iClrText;
	COLORREF		iClrAltText;

	IMAGE_KEY_TYPE	iUsedImageKey;
	SIZE			iUsedImageSize;
	IMAGE_KEY_TYPE	iBackImageKey;
	SIZE			iBackImageSize;
	IMAGE_KEY_TYPE	iAltBackImageKey;
	SIZE			iAltBackImageSize;

	int				iOffsetX;

	BOOL			iShouldBoldTextOnFocus;

	void _Init()
	{
		ModifyStyle(WS_BORDER, BS_OWNERDRAW);

		iClrText = iClrAltText = ::GetSysColor(COLOR_WINDOWTEXT);

		iOffsetX = 5;
		iBorderStyle = IBS_3D_BORDER;
		iUsedImageKey = INVALID_IMAGE_KEY_VALUE;
		iUsedImageSize.cx = iUsedImageSize.cy = 0;
		iBackImageKey = INVALID_IMAGE_KEY_VALUE;
		iBackImageSize.cx = iBackImageSize.cy = 0;
		iAltBackImageKey = INVALID_IMAGE_KEY_VALUE;
		iAltBackImageSize.cx = iAltBackImageSize.cy = 0;

		iShouldBoldTextOnFocus = TRUE;
	}

	void _GetImageDrawingRect( CDCHandle& dc, IMAGE_KEY_TYPE keyImage, RECT& rcIcon )
	{
		RECT rcClient = {0};
		rcIcon = rcClient;
		GetClientRect( &rcClient );

		if ( INVALID_IMAGE_KEY_VALUE == keyImage )
			return;

		SIZE szImage = {0};
		ImageHelper::GetInstance()->GetImageSize( keyImage, szImage );

		int clientWidth = rcClient.right - rcClient.left;
		int clientHeight = rcClient.bottom - rcClient.top;

		if ( szImage.cy > clientHeight )
			szImage.cy = clientHeight;

		if ( szImage.cx > (clientWidth-iOffsetX) )
			szImage.cx = clientWidth - iOffsetX;

		rcClient.left = iOffsetX;
		rcClient.right = rcClient.left + szImage.cx;
		rcClient.top = rcClient.top + clientHeight/2 - szImage.cy/2;
		rcClient.bottom = rcClient.top + szImage.cy;

		rcIcon = rcClient;
	}

	void _DrawImage( CDCHandle& dc, IMAGE_KEY_TYPE keyImage, RECT& rcDrawAt )
	{	
		ImageHelper::GetInstance()->DrawImageInDC( keyImage, dc, rcDrawAt );
	}

	void _UpdateCurrentFont( BOOL isFocused )
	{
		HFONT hfOld;
		LOGFONT lf;
		HFONT hfNew;

		hfOld = this->GetFont();
		::GetObject(hfOld, sizeof(lf), &lf);
		lf.lfWeight = isFocused? FW_BOLD: FW_MEDIUM;
		hfNew = ::CreateFontIndirect(&lf);

		this->SetFont(hfNew);

		//DeleteObject( hfOld );
	}

};

class CImageButton: public CImageButtonImpl<CImageButton>
{
public:
	DECLARE_WND_SUPERCLASS(_T("WTL_N2FImageButton"), GetWndClassName())
};
