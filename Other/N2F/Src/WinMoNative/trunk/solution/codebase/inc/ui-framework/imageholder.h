#pragma once

//#include <imagehelper.h>

#define IHS_ALIGN_CENTER	0x0001
#define IHS_FIT_HOLDER_FOR_IMAGE	0x0002
#define IHS_FIT_IMAGE_FOR_HOLDER	0x0004


template< class T, class TBase = CButton, class TWinTraits = CControlWinTraits>
class CImageHolderImpl: 
			public CWindowImpl< T, TBase, TWinTraits >,
			public COwnerDraw< T >
{
public:

	typedef CImageHolderImpl< T, TBase, TWinTraits > thisClass;

	DECLARE_WND_SUPERCLASS( NULL, TBase::GetWndClassName() )

	BEGIN_MSG_MAP(thisClass)
		MESSAGE_HANDLER( WM_CREATE, OnCreate )
		MESSAGE_HANDLER( WM_SIZE, OnSize )
		REFLECTED_COMMAND_CODE_HANDLER( BN_CLICKED, OnClicked )
		CHAIN_MSG_MAP_ALT( COwnerDraw< T >, 1 )
		DEFAULT_REFLECTION_HANDLER()
	END_MSG_MAP()

	BOOL SubclassWindow( HWND hWnd )
	{
		BOOL bRet = CWindowImpl< T, TBase, TWinTraits >::SubclassWindow(hWnd);
		if ( bRet )
			_Init();

		return bRet;
	}

	void SetImageKey( IMAGE_KEY_TYPE key )
	{
		ATLASSERT( ::IsWindow(m_hWnd) );

		_FreeResizedImage();

		iOriginalImageKey = key;
		ImageHelper::GetInstance()->GetImageSize( iOriginalImageKey, iOriginalImageSize );

		RECT rcClient = {0};
		GetClientRect( &rcClient );
		int clientWidth = rcClient.right - rcClient.left;
		int clientHeight = rcClient.bottom - rcClient.top;

		bool bWindowFitsImage = false;
		if ( iOriginalImageSize.cx == clientWidth && 
			iOriginalImageSize.cy == clientHeight )
		{
			iUsedImageKey = iOriginalImageKey;
			iUsedImageSize = iOriginalImageSize;
			bWindowFitsImage = true;
		}

		SIZE szNewSize = {0};

		
		if ( false == bWindowFitsImage && iImageHolderStyle & IHS_FIT_HOLDER_FOR_IMAGE )
		{
			RECT rcNew = rcClient;
			rcNew.right = rcNew.left + iOriginalImageSize.cx;
			rcNew.bottom = rcNew.top + iOriginalImageSize.cy;

			// calculate new window rect based on new client rect
			::AdjustWindowRectEx( &rcNew, this->GetStyle(), FALSE, this->GetExStyle());

			MoveWindow(&rcNew);

			bWindowFitsImage = true;

			iUsedImageKey = iOriginalImageKey;
			iUsedImageSize = iOriginalImageSize;
		}

		if ( false == bWindowFitsImage && iImageHolderStyle & IHS_FIT_IMAGE_FOR_HOLDER )
		{
			szNewSize.cx = clientWidth;
			szNewSize.cy = clientHeight;
			_UpdateResizedImage( szNewSize );
		}
		
		Invalidate();
	}


	void SetImageHolderStyle( UINT ihSytle )
	{
		ATLASSERT( ::IsWindow(m_hWnd) );
		iImageHolderStyle = ihSytle;

		// update images
		SetImageKey( iOriginalImageKey );

		Invalidate();
	}

	int GetImageHeight()
	{
		return iUsedImageSize.cy;
	}

	int GetImageWidth()
	{
		return iUsedImageSize.cx;
	}

	LRESULT OnCreate(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
	{
		LRESULT lRes = DefWindowProc();
		_Init();

		return lRes;
	}

	LRESULT OnSize(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
	{
		LRESULT lRes = DefWindowProc();
		
		SetImageKey( iOriginalImageKey );

		return lRes;
	}

	LRESULT OnClicked(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
	{
		//ATLTRACE(_T("CImageHolderImpl::OnClicked()\n"));

		bHandled = FALSE;
		return 0;
	}

	// Owner draw methods

	void DrawItem( LPDRAWITEMSTRUCT lpDIS )
	{
		if ( INVALID_IMAGE_KEY_VALUE == iUsedImageKey )
			return;

		CDCHandle dc = lpDIS->hDC;
		RECT rc = lpDIS->rcItem;
		UINT& state = lpDIS->itemState;

		RECT rcImage = rc;

		SIZE szImage = iUsedImageSize;

		int clientWidth = rcImage.right - rcImage.left;
		int clientHeight = rcImage.bottom - rcImage.top;

		if ( szImage.cx > clientWidth )
			szImage.cx = clientWidth;

		if ( szImage.cy > clientHeight )
			szImage.cy = clientHeight;

		if ( iImageHolderStyle & IHS_ALIGN_CENTER )
		{
			rcImage.left = rcImage.left + clientWidth/2 - szImage.cx/2;
			rcImage.top = rcImage.top + clientHeight/2 - szImage.cy/2;
		}
		
		rcImage.right = rcImage.left + szImage.cx;
		rcImage.bottom = rcImage.top + szImage.cy;

		//LOGMSG("Image rect: %d %d %d %d", rcImage.left, rcImage.top, rcImage.right, rcImage.bottom);

		BOOL res = ImageHelper::GetInstance()->DrawImageInDC( iUsedImageKey, dc, rcImage );
		ATLASSERT( res == TRUE );
	}

protected:
	
	void _Init()
	{
		ModifyStyle(0xff, BS_OWNERDRAW, 0);
		iOriginalImageKey = iUsedImageKey = INVALID_IMAGE_KEY_VALUE;
		iUsedImageSize.cx = iUsedImageSize.cy = 0;
		iOriginalImageSize.cx = iOriginalImageSize.cy = 0;
		iImageHolderStyle = IHS_ALIGN_CENTER;
	}

	void _FreeResizedImage()
	{
		if ( iOriginalImageKey != iUsedImageKey && INVALID_IMAGE_KEY_VALUE != iUsedImageKey )
			ImageHelper::GetInstance()->FreeImage( iUsedImageKey );

		iUsedImageKey = INVALID_IMAGE_KEY_VALUE;
		iUsedImageSize.cx = iUsedImageSize.cy = 0;
	}

	void _UpdateResizedImage( SIZE& newSize)
	{
		_FreeResizedImage();

		IMAGE_KEY_TYPE keyResizedImage = INVALID_IMAGE_KEY_VALUE;
		UINT width = newSize.cx;
		UINT height = newSize.cy;
		ImageHelper::GetInstance()->GetThumbnailImageForImage( iOriginalImageKey, width, height, keyResizedImage );
		
		if ( INVALID_IMAGE_KEY_VALUE != keyResizedImage )
		{
			iUsedImageKey = keyResizedImage;
			ImageHelper::GetInstance()->GetImageSize( iUsedImageKey, iUsedImageSize );
		}
	}


	IMAGE_KEY_TYPE	iUsedImageKey;
	SIZE			iUsedImageSize;

	IMAGE_KEY_TYPE	iOriginalImageKey;
	SIZE			iOriginalImageSize;

	UINT			iImageHolderStyle;
	
};

class CImageHolder: public CImageHolderImpl<CImageHolder>
{
public:
	DECLARE_WND_SUPERCLASS(_T("WTL_N2FImageHolder"), GetWndClassName())
};

