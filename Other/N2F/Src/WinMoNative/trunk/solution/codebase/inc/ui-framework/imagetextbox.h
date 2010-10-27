#pragma once


class CImageTextbox:
	public CWindowImpl< CImageTextbox, CEdit, CControlWinTraits >
{
public:

	//typedef CImageTextboxImpl< T, TBase, TWinTraits > thisClass;

	//DECLARE_WND_SUPERCLASS( NULL, CEdit::GetWndClassName() )
	//DECLARE_WND_CLASS( NULL )

	BOOL SubclassWindow( HWND hWnd )
	{
		//BOOL bRet = CWindowImpl< T, TBase, TWinTraits >::SubclassWindow( hWnd );
		BOOL bRet = CWindowImpl< CImageTextbox, CEdit >::SubclassWindow( hWnd );
		if ( bRet )
			_Init();

		return bRet;
	}

	BEGIN_MSG_MAP( thisClass )
		MESSAGE_HANDLER( WM_CREATE, OnCreate )
		MESSAGE_HANDLER( WM_SIZE, OnSize )
		MESSAGE_HANDLER( WM_SETFOCUS, OnSetFocus )
		MESSAGE_HANDLER( WM_KILLFOCUS, OnKillFocus )
		MESSAGE_HANDLER( OCM_CTLCOLOREDIT, OnCtlColorEdit )
		//MESSAGE_HANDLER( WM_ERASEBKGND, OnEraseBackground )
		MESSAGE_HANDLER( WM_PAINT, OnPaint )
		DEFAULT_REFLECTION_HANDLER()
	END_MSG_MAP()

	void SetBackgroundImageKey( IMAGE_KEY_TYPE keyBgImage, IMAGE_KEY_TYPE keyAltBgImage = INVALID_IMAGE_KEY_VALUE )
	{
		ATLASSERT( ::IsWindow( m_hWnd ) );
		iBackImageKey = keyBgImage;
		if ( INVALID_IMAGE_KEY_VALUE == keyAltBgImage )
			iAltBackImageKey = keyBgImage;
		else
			iAltBackImageKey = keyAltBgImage;

		_CreatePatternBrushes();

		Invalidate();
	}

	BOOL IsFocused()
	{
		ATLASSERT( ::IsWindow(m_hWnd) );

		return ( (m_hWnd == ::GetFocus())? TRUE: FALSE );
	}


	LRESULT OnCreate(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
	{
		LRESULT lRes = DefWindowProc();
		_Init();

		_PrintOutAllFontFamilies();

		return lRes;
	}

	LRESULT OnSize(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
	{
		//LRESULT lRes = DefWindowProc();

		DefWindowProc();

		IMAGE_KEY_TYPE key = iBackImageKey;
		if ( FALSE == IsFocused() )
			key = iAltBackImageKey;

		RECT rcClient = {0};
		GetClientRect( &rcClient );
		int clientHeight = rcClient.bottom - rcClient.top;
		int clientWidth = rcClient.right - rcClient.left;

		if ( clientWidth == 0 || clientHeight == 0 )
			return 0;

		SIZE backImgSize = {0};
		ImageHelper::GetInstance()->GetImageSize( key, backImgSize );

		if ( clientWidth == backImgSize.cx && 
				clientHeight == backImgSize.cy )
		{
			// do nothing
			//LOGMSG("RECT: %d %d %d %d", rcClient.left, rcClient.top, rcClient.right, rcClient.bottom);
		}
		else
		{
			RECT rcNew = rcClient, rcWnd = {0};
			

			GetWindowRect(&rcWnd);

			rcNew.left = rcWnd.left;
			rcNew.right = rcNew.left + backImgSize.cx;
			rcNew.top = rcWnd.top;
			rcNew.bottom = rcNew.top + backImgSize.cy;

			//LOGMSG("Moving to RECT: %d %d %d %d", rcNew.left, rcNew.top, rcNew.right, rcNew.bottom);

			::AdjustWindowRectEx( &rcNew, this->GetStyle(), FALSE, this->GetExStyle() );
			MoveWindow( &rcNew );

			int w = rcNew.right - rcNew.left;
			int h = rcNew.bottom - rcNew.top;


			rcNew.left = rcWnd.left;
			rcNew.right = rcNew.left + w;
			rcNew.top = rcWnd.top;
			rcNew.bottom = rcNew.top + h;


			bHandled = TRUE;

		}



		return 0;
	}

	LRESULT OnKillFocus(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
	{
		LRESULT lRes = DefWindowProc();

		if ( iAltBackBrush != iCurrentBackBrush )
		{
			iCurrentBackBrush = iAltBackBrush;
			Invalidate();
		}
		

		return 0;
	}

	LRESULT OnSetFocus(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
	{
		LRESULT lRes = DefWindowProc();

		if ( iBackBrush != iCurrentBackBrush )
		{
			iCurrentBackBrush = iBackBrush;
			Invalidate();
		}

		return 0;
	}

	//LRESULT OnEraseBackground(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
	//{
	//	LRESULT lRes = 0;
	//	HDC dc = (HDC)wParam;

	//	SetBkMode(dc, TRANSPARENT);

	//	LOGMSG("this is me!");

	//	if ( iBackImageKey != INVALID_IMAGE_KEY_VALUE )
	//	{
	//		RECT rc = {0};
	//		GetClientRect( &rc );

	//		IMAGE_KEY_TYPE keyToDraw = iBackImageKey;

	//		if ( FALSE == IsFocused() )
	//			keyToDraw = iAltBackImageKey;

	//		ImageHelper::GetInstance()->DrawImageInDC( keyToDraw, dc, rc );
	//		lRes = TRUE;
	//	}

	//	return lRes;
	//}

	LRESULT OnCtlColorEdit(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
	{
		//LRESULT lRes = DefWindowProc();
		//HDC dc = (HDC)wParam;

		////LOGMSG("OnCtlColorEdit");

		//if ( iBackImageKey != INVALID_IMAGE_KEY_VALUE )
		//{
		//	SetBkMode( dc, TRANSPARENT );
		//	RECT rc = {0};
		//	GetClientRect( &rc );

		//	
		//	IMAGE_KEY_TYPE keyToDraw = iBackImageKey;

		//	if ( FALSE == IsFocused() )
		//		keyToDraw = iAltBackImageKey;

		//	ImageHelper::GetInstance()->DrawImageInDC( keyToDraw, dc, rc );
		//	//lRes = TRUE;
		//}

		////return lRes;

		HDC dc = (HDC)wParam;
		SetBkMode( dc, TRANSPARENT );

		//HBRUSH brhResult = IsFocused()? iBackBrush: iAltBackBrush;

		//Invalidate();
		//HBRUSH brhResult = iAltBackBrush;

		return (LRESULT)iCurrentBackBrush;
	}

	LRESULT OnPaint(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
	{
		
		POINT pervOrig = {0};
		HDC dc = (HDC)wParam;
		
		POINT pt;
		pt.x = 0;
		pt.y = 5;
		::MapWindowPoints(m_hWnd, this->GetParent(), &pt, 1);
		::SetViewportOrgEx(dc, pt.x, pt.y, &pervOrig);

		LRESULT lres = DefWindowProc();

		::SetViewportOrgEx(dc, -pervOrig.x, -pervOrig.y, NULL);

		return 0;
	}

protected:

	IMAGE_KEY_TYPE	iBackImageKey;
	IMAGE_KEY_TYPE	iAltBackImageKey;

	CBrush	iBackBrush, iAltBackBrush;
	HBRUSH	iCurrentBackBrush;

	UINT	iTextLimit;

	void _Init()
	{
		//ModifyStyle( WS_BORDER, 0, 0 );
		//ModifyStyle(0, WS_BORDER, 0);

		iBackImageKey = iAltBackImageKey = INVALID_IMAGE_KEY_VALUE;
		iCurrentBackBrush = NULL;

		iTextLimit = 100;

		
		//this->SendMessage( EM_SETMARGINS, EC_LEFTMARGIN|EC_RIGHTMARGIN, MAKELONG(10, 10));
		this->SetMargins(10, 10);
		this->SetLimitText( iTextLimit );

	}

	void _CreatePatternBrushes()
	{
		CBitmap hbmBack = ImageHelper::GetInstance()->HBITMAPFromImage(iBackImageKey, RGB(255, 255, 255));
		CBitmap hbmAltBack = ImageHelper::GetInstance()->HBITMAPFromImage(iAltBackImageKey, RGB(255, 255, 255));

		iBackBrush = CreatePatternBrush(hbmBack);
		iAltBackBrush = CreatePatternBrush(hbmAltBack);

		iCurrentBackBrush = IsFocused()? iBackBrush: iAltBackBrush;
	}

	static int CALLBACK MyFontEnumerator2(CONST LOGFONT *lplf, CONST TEXTMETRIC *lptm, 
		DWORD dwFontType, LPARAM lParam)
	{
		if (lplf )
		{
			LOGMSG("Face name: %s", lplf->lfFaceName);
		}

		return 1;
	}

	void _PrintOutAllFontFamilies()
	{
		CWindowDC dc(m_hWnd);

		//LOGFONT lf;
		//lf.lfFaceName[0] = 0;
		//lf.lfCharSet = DEFAULT_CHARSET;

		//EnumFontFamiliesEx(dc, &lf, FontEnumerator, 0, 0);

		//EnumFonts( dc, _T("Tahoma"), CImageTextbox::MyFontEnumerator2, 0);
		
	}

};


