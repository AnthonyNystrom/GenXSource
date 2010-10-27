#pragma once

#include <imagehelper.h>

// ImageListBox mask flags
#define ILBIF_TEXT               0x0001
#define ILBIF_SUBTEXT            0x0002
#define ILBIF_IMAGE              0x0004
#define ILBIF_SELIMAGE           0x0008
#define ILBIF_STYLE              0x0010
#define ILBIF_FORMAT             0x0020
#define ILBIF_PARAM              0x0040
#define ILBIF_DOESNTOWNSIMGS	 0x0080

// ImageListBox styles
#define ILBS_IMGLEFT             0x0000
#define ILBS_IMGRIGHT            0x0001
#define ILBS_IMGTOP              0x0002
#define ILBS_SELNORMAL           0x0000
#define ILBS_SELROUND            0x0010


// ImageListBox item structure
typedef struct
{
	int		iItem;				// index
	UINT	mask;				// mask (ILBIF_xxx flags)
	UINT	style;				// item styles (ILBS_xxx flags)
	UINT	format;				// text format (DT_xxx flags)
	LPTSTR	pszText;			// title text
	int		cchMaxText;
	LPTSTR	pszSubText;			// subtext
	int		cchMaxSubText;
	//int		iImage;				// image
	//int		iSelImage;			// selected image

	IMAGE_KEY_TYPE	iImage;		// image
	IMAGE_KEY_TYPE	iSelImage;	// selected image

	LPARAM	lParam;				// user-defined parameter
} ILBITEM, *PILBITEM;

// ImageListBox ImageList constants
#define ILSIL_NORMAL    0
#define ILSIL_SELECTED  1

// ImageListBox settings
typedef struct
{
	COLORREF	clrText;
	COLORREF	clrBackground;
	COLORREF	clrHighlite;
	COLORREF	clrHighliteText;
	COLORREF	clrHighliteBorder;
	SIZE		sizeMargin;
	SIZE		sizeIndent;
	SIZE		sizeSubIndent;
	POINT		ptArc;
} ILBSETTINGS, *PILBSETTINGS;


template< class T, class TBase = CListViewCtrl, class TWinTraits = CControlWinTraits >

class ATL_NO_VTABLE CImageListboxImpl : 
	public CWindowImpl< T, TBase, TWinTraits >,
	public CCustomDraw< T >
{
public:
	DECLARE_WND_SUPERCLASS(NULL, TBase::GetWndClassName())

	CImageListboxImpl() : iFontMain(NULL), iFontSub(NULL)
	{ 
		::ZeroMemory(&iILBSettings, sizeof(iILBSettings));      
		iILBSettings.ptArc.x = iILBSettings.ptArc.y = 6;
	}

	ILBSETTINGS			iILBSettings;
	//CImageList			iNormalImageList;
	//CImageList			iSelectedImageList;
	CFont				iFontScale;
	HFONT				iFontMain;
	HFONT				iFontSub;

	// Operations

	BOOL SubclassWindow(HWND hWnd)
	{
		ATLASSERT( m_hWnd==NULL );
		ATLASSERT( ::IsWindow(hWnd) );

		BOOL bRet = CWindowImpl< T, TBase, TWinTraits >::SubclassWindow(hWnd);

		if( bRet ) _Init();

		return bRet;
	}

	void SetItemData(int nIndex, LPARAM lParam)
	{
		ATLASSERT(false);
	}

	int InsertItem(const ILBITEM* pItem)
	{
		ATLASSERT(::IsWindow(m_hWnd));
		// Create a copy of the ITEM structure
		PILBITEM pNewItem;      
		ATLTRY(pNewItem = new ILBITEM);
		ATLASSERT(pNewItem);
		::ZeroMemory(pNewItem, sizeof(ILBITEM));
		UINT mask = pItem->mask;
		pNewItem->mask = mask;

		if( mask & ILBIF_TEXT )
		{
			ATLTRY(pNewItem->pszText = new TCHAR[ lstrlen(pItem->pszText) + 1 ] );
			::lstrcpy( pNewItem->pszText, pItem->pszText );
		}

		if( mask & ILBIF_SUBTEXT )
		{
			ATLTRY(pNewItem->pszSubText = new TCHAR[ lstrlen(pItem->pszSubText ) + 1 ] );
			::lstrcpy( pNewItem->pszSubText, pItem->pszSubText );
		}

		if( mask & ILBIF_STYLE )
			pNewItem->style = pItem->style;

		if( mask & ILBIF_FORMAT )
			pNewItem->format = pItem->format;

		if( mask & ILBIF_IMAGE )
			pNewItem->iImage = pItem->iImage;

		if( mask & ILBIF_SELIMAGE )
			pNewItem->iSelImage = pItem->iSelImage;

		if( mask & ILBIF_PARAM )
			pNewItem->lParam = pItem->lParam;

		// Add item to listbox
		int iItem = TBase::InsertItem(pItem->iItem, pNewItem->pszText);
		if( iItem >= 0 )
		{
			TBase::SetItemData(iItem, (LPARAM) pNewItem);
		}

		return iItem;
	}

	BOOL GetItem(ILBITEM *pItem) const
	{
		ATLASSERT( ::IsWindow(m_hWnd) );
		ATLASSERT( pItem );
		PILBITEM pOrgItem = (PILBITEM) TBase::GetItemData(pItem->iItem);

		if( pOrgItem == NULL )
			return FALSE;
		// Copy attributes
		UINT mask = pItem->mask;
		if( mask & ILBIF_TEXT )
			::lstrcpyn( pItem->pszText, pOrgItem->pszText, pItem->cchMaxText );

		if( mask & ILBIF_SUBTEXT )
			::lstrcpyn( pItem->pszSubText, pOrgItem->pszSubText, pItem->cchMaxSubText );

		if( mask & ILBIF_STYLE )
			pItem->style = pOrgItem->style;

		if( mask & ILBIF_FORMAT )
			pItem->format = pOrgItem->format;

		if( mask & ILBIF_IMAGE )
			pItem->iImage = pOrgItem->iImage;

		if( mask & ILBIF_SELIMAGE )
			pItem->iSelImage = pOrgItem->iSelImage;

		if( mask & ILBIF_PARAM )
			pItem->lParam = pOrgItem->lParam;

		return TRUE;
	}

	BOOL SetItem(const ILBITEM *pItem)
	{
		ATLASSERT( ::IsWindow(m_hWnd) );
		ATLASSERT( pItem );     
		// Get original item data and set attributes
		PILBITEM pOrgItem = (PILBITEM) TBase::GetItemData(pItem->iItem);
		if( pOrgItem == NULL )
			return FALSE;

		UINT mask = pItem->mask;
		if( mask & ILBIF_TEXT )
		{
			if( pOrgItem->mask & ILBIF_TEXT ) delete [] pOrgItem->pszText;
			ATLTRY(pOrgItem->pszText = new TCHAR[ lstrlen(pItem->pszText) + 1 ]);
			::lstrcpy( pOrgItem->pszText, pItem->pszText );
		}

		if( mask & ILBIF_SUBTEXT )
		{
			if( pOrgItem->mask & ILBIF_SUBTEXT ) delete [] pOrgItem->pszSubText;
			ATLTRY( pOrgItem->pszSubText = new TCHAR[ lstrlen(pItem->pszSubText) + 1] );
			::lstrcpy( pOrgItem->pszSubText, pItem->pszSubText );
		}

		if( mask & ILBIF_STYLE )
			pOrgItem->style = pItem->style;

		if( mask & ILBIF_FORMAT )
			pOrgItem->format = pItem->format;

		if( mask & ILBIF_IMAGE )
			pOrgItem->iImage = pItem->iImage;

		if( mask & ILBIF_SELIMAGE )
			pOrgItem->iSelImage = pItem->iSelImage;

		if( mask & ILBIF_PARAM )
			pOrgItem->lParam = pItem->lParam;

		pOrgItem->mask |= pItem->mask;
		// Repaint item
		RECT rc;
		GetItemRect(pItem->iItem, &rc, LVIR_BOUNDS);
		InvalidateRect(&rc, TRUE);
		return TRUE;
	}

	//CImageList SetImageList(HIMAGELIST hImageList, int nImageList)
	//{
	//	HIMAGELIST hOldList = NULL;

	//	//switch( nImageList )
	//	//{
	//	//case ILSIL_NORMAL:
	//	//	hOldList = iNormalImageList;
	//	//	iNormalImageList = hImageList;
	//	//	
	//	//	break;
	//	//case ILSIL_SELECTED:
	//	//	hOldList = iSelectedImageList;
	//	//	iSelectedImageList = hImageList;
	//	//	break;
	//	//default:
	//	//	ATLASSERT(false);
	//	//}

	//	return TBase::SetImageList(hImageList, LVSIL_SMALL);;
	//}

	HFONT SetFont(HFONT hFont)
	{
		ATLASSERT(::IsWindow(m_hWnd));
		HFONT hOldFont = iFontMain;
		iFontMain = hFont;
		Invalidate();
		return hOldFont;
	}

	HFONT SetSmallFont(HFONT hFont)
	{
		ATLASSERT(::IsWindow(m_hWnd));
		HFONT hOldFont = iFontSub;
		iFontSub = hFont;
		Invalidate();
		return hOldFont;
	}

	void GetPreferences(PILBSETTINGS pPrefs) const
	{
		ATLASSERT(pPrefs);
		*pPrefs = iILBSettings;
	}

	void SetPreferences(ILBSETTINGS Prefs)
	{
		ATLASSERT(::IsWindow(m_hWnd));
		iILBSettings = Prefs;
		SetBkColor(iILBSettings.clrBackground);
	}

	// Implementation

	void _Init()
	{
		ATLASSERT(::IsWindow(m_hWnd));

		ModifyStyle( 0, LVS_REPORT|LVS_SINGLESEL|LVS_SHOWSELALWAYS|LVS_OWNERDRAWFIXED|LVS_SHAREIMAGELISTS, 0);

		ATLASSERT((GetStyle() & LVS_REPORT)!=0);
		ATLASSERT((GetStyle() & LVS_SINGLESEL)!=0);
		ATLASSERT((GetStyle() & LVS_SHOWSELALWAYS)!=0);
		ATLASSERT((GetStyle() & LVS_OWNERDRAWFIXED)!=0);
		ATLASSERT((GetStyle() & LVS_SHAREIMAGELISTS)!=0);

		SetExtendedListViewStyle(LVS_EX_FULLROWSELECT);

		T* pT = static_cast<T*>(this);
		pT->_InitSettings();

		// Add dummy column for text to actually get displayed!
		AddColumn(_T(""), 0);
	}

	void _InitSettings()
	{
		iILBSettings.clrText = ::GetSysColor(COLOR_WINDOWTEXT);
		iILBSettings.clrBackground = ::GetSysColor(COLOR_WINDOW);
		iILBSettings.clrHighliteText = ::GetSysColor(COLOR_HIGHLIGHTTEXT);
		iILBSettings.clrHighlite = iILBSettings.clrHighliteBorder = ::GetSysColor(COLOR_HIGHLIGHT);
	}

	// Message map and handlers

	BEGIN_MSG_MAP(CImageListboxImpl)
		MESSAGE_HANDLER(WM_CREATE, OnCreate)
		MESSAGE_HANDLER(WM_DESTROY, OnDestroy)
		MESSAGE_HANDLER(WM_SIZE, OnSize)
		REFLECTED_NOTIFY_CODE_HANDLER(LVN_DELETEITEM, OnDeleteItem)
		CHAIN_MSG_MAP_ALT( CCustomDraw<T>, 1 )
	END_MSG_MAP()

	LRESULT OnCreate(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& /*bHandled*/)
	{
		LRESULT lRes = DefWindowProc(uMsg, wParam, lParam);
		_Init();
		return lRes;
	}
	LRESULT OnDestroy(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& bHandled)
	{
		DeleteAllItems(); // Make sure to delete item-data memory
		bHandled = FALSE;
		return 0;
	}
	LRESULT OnSize(UINT /*uMsg*/, WPARAM /*wParam*/, LPARAM /*lParam*/, BOOL& bHandled)
	{
		SetColumnWidth(0, LVSCW_AUTOSIZE_USEHEADER);
		bHandled = FALSE;
		return 0;
	}

	LRESULT OnDeleteItem(int /*idCtrl*/, LPNMHDR pnmh, BOOL& bHandled)
	{
		LPNMLISTVIEW pnmlv = (LPNMLISTVIEW) pnmh;
		PILBITEM pItem = reinterpret_cast<PILBITEM>( TBase::GetItemData(pnmlv->iItem) );
		if( pItem->mask & ILBIF_TEXT )
			delete [] pItem->pszText;

		if( pItem->mask & ILBIF_SUBTEXT )
			delete [] pItem->pszSubText;

		if ( !(pItem->mask & ILBIF_DOESNTOWNSIMGS) &&
			(pItem->mask & ILBIF_IMAGE) )
		{
			ImageHelper::GetInstance()->FreeImage( pItem->iImage );
			if ( pItem->iImage != pItem->iSelImage )
			{
				ImageHelper::GetInstance()->FreeImage( pItem->iSelImage );
			}
		}

		delete pItem;
		return 0;
	}

	// Custom painting

	DWORD OnPrePaint(int /*idCtrl*/, LPNMCUSTOMDRAW /*lpNMCustomDraw*/)
	{
		return CDRF_NOTIFYITEMDRAW;   // We need per-item notifications
	}
	DWORD OnItemPrePaint(int /*idCtrl*/, LPNMCUSTOMDRAW lpNMCustomDraw)
	{
		LPNMLVCUSTOMDRAW lpNMLVCD = (LPNMLVCUSTOMDRAW) lpNMCustomDraw;

		CDCHandle dc = lpNMLVCD->nmcd.hdc;

		PILBITEM pItem = (PILBITEM) lpNMLVCD->nmcd.lItemlParam;
		ATLASSERT(pItem);

		RECT rcItem;
		GetItemRect(lpNMLVCD->nmcd.dwItemSpec, &rcItem, LVIR_BOUNDS);

		rcItem.right -= 1;

		bool bSelected = lpNMLVCD->nmcd.uItemState & CDIS_SELECTED;

		UINT style = (pItem->mask & ILBIF_STYLE) != 0 ? pItem->style : ILBS_IMGLEFT | ILBS_SELNORMAL;

		COLORREF clrFront = bSelected ? iILBSettings.clrHighliteText : iILBSettings.clrText;
		COLORREF clrBack = bSelected ? iILBSettings.clrHighlite : iILBSettings.clrBackground;

		if( bSelected && (style & ILBS_SELROUND) )
		{
			// Draw round-rect selection
			dc.FillSolidRect(&rcItem, iILBSettings.clrBackground);
			::InflateRect(&rcItem, -iILBSettings.sizeMargin.cx, -iILBSettings.sizeMargin.cy);
			CPen pen;
			pen.CreatePen(PS_SOLID, 1, iILBSettings.clrHighliteBorder);
			CBrush brush;
			brush.CreateSolidBrush(iILBSettings.clrHighlite);
			HPEN hOldPen = dc.SelectPen(pen);
			HBRUSH hOldBrush = dc.SelectBrush(brush);
			dc.RoundRect(&rcItem, iILBSettings.ptArc);
			dc.SelectBrush(hOldBrush);
			dc.SelectPen(hOldPen);
		}
		else
		{
			// Fill background
			dc.FillSolidRect(&rcItem, clrBack);
			::InflateRect(&rcItem, -iILBSettings.sizeMargin.cx, -iILBSettings.sizeMargin.cy);
		}

		if( style & ILBS_SELROUND )
			::InflateRect(&rcItem, -iILBSettings.ptArc.x, -iILBSettings.ptArc.y);

		// Get image information
		int iImageWidth = 0, iImageHeight = 0;
		//if( pItem->mask & ILBIF_IMAGE )
		//	iNormalImageList.GetIconSize(iImageWidth, iImageHeight);

		IMAGE_KEY_TYPE	keyImageToDraw = INVALID_IMAGE_KEY_VALUE;
		if ( bSelected && pItem->mask & ILBIF_IMAGE )
		{
			keyImageToDraw = pItem->iSelImage;
		}
		else
		{
			keyImageToDraw = pItem->iImage;
		}

		if ( INVALID_IMAGE_KEY_VALUE != keyImageToDraw )
		{
			SIZE szImage = {0};
			ImageHelper::GetInstance()->GetImageSize(keyImageToDraw, szImage);
			iImageHeight = szImage.cy;
			iImageWidth = szImage.cx;

			iImageHeight = iImageWidth = 60;
		}

		// Prepare draw
		HFONT hOldFont = dc.SelectFont(GetFont());
		dc.SetBkMode(TRANSPARENT);
		dc.SetBkColor(clrBack);
		dc.SetTextColor(clrFront);

		// Draw image (may be aligned left/right/top)
		if( pItem->mask & ILBIF_IMAGE )
		{
			if( (style & ILBS_IMGRIGHT) == 0 )
			{
				// Left- or top-aligned image
				POINT pt;
				if( style & ILBS_IMGTOP )
				{
					pt.x = rcItem.left + ((rcItem.right - rcItem.left) / 2) - (iImageWidth / 2);
					pt.y = rcItem.top;
				}
				else
				{
					pt.x = rcItem.left;
					pt.y = rcItem.top + ((rcItem.bottom - rcItem.top) / 2) - (iImageHeight / 2);
				}

				RECT rcImage = {0};
				SIZE szImage = {0};
				rcImage.left = pt.x;
				rcImage.top = pt.y;
				rcImage.bottom = rcImage.top + iImageHeight;
				rcImage.right = rcImage.left + iImageWidth;

				if ( INVALID_IMAGE_KEY_VALUE != keyImageToDraw )
				{
					ImageHelper::GetInstance()->DrawImageInDC( keyImageToDraw, dc, rcImage );
				}

				//if( bSelected && (pItem->mask & ILBIF_SELIMAGE) )
				//{
				//	//iSelectedImageList.Draw(dc, pItem->iSelImage, pt, ILD_NORMAL);
				//	ImageHelper::GetInstance()->GetImageSize( pItem->iSelImage, szImage );
				//	rcImage.bottom = rcImage.top + szImage.cy;
				//	rcImage.right = rcImage.left + szImage.cx;
				//	ImageHelper::GetInstance()->DrawImageInDC( pItem->iSelImage, dc, rcImage);
				//}
				//else
				//{
				//	//iNormalImageList.Draw(dc, pItem->iImage, pt, ILD_NORMAL);
				//	ImageHelper::GetInstance()->GetImageSize( pItem->iImage, szImage );
				//	rcImage.bottom = rcImage.top + szImage.cy;
				//	rcImage.right = rcImage.left + szImage.cx;
				//	ImageHelper::GetInstance()->DrawImageInDC( pItem->iImage, dc, rcImage);
				//}
				if( style & ILBS_IMGTOP )
				{
					rcItem.top += iImageHeight;
				}
				else
				{
					rcItem.left += iImageWidth;
				}
			}
			else
			{
				// Right-aligned image
				int x = rcItem.right - iImageWidth;
				int y = rcItem.top + ((rcItem.bottom - rcItem.top) / 2) - (iImageHeight / 2);

				RECT rcImage = {0};
				rcImage.left = x;
				rcImage.top = y;
				rcImage.bottom = rcImage.top + iImageHeight;
				rcImage.right = rcImage.left + iImageWidth;

				if ( INVALID_IMAGE_KEY_VALUE != keyImageToDraw )
				{
					ImageHelper::GetInstance()->DrawImageInDC( keyImageToDraw, dc, rcImage );
				}

				//if( bSelected && (pItem->mask & ILBIF_SELIMAGE) )
				//{
				//	iSelectedImageList.Draw(dc, pItem->iSelImage, x,y, ILD_NORMAL);
				//}
				//else
				//{
				//	iNormalImageList.Draw(dc, pItem->iImage, x,y, ILD_NORMAL);
				//}
				rcItem.right -= iImageWidth;
			}
		}

		// Prepare text drawing
		UINT format = DT_LEFT | DT_NOPREFIX;
		if( pItem->mask & ILBIF_FORMAT )
			format = pItem->format;

		// Draw text
		if( pItem->mask & ILBIF_TEXT )
		{
			if( iFontMain != NULL ) dc.SelectFont(iFontMain);
			::InflateRect(&rcItem, -iILBSettings.sizeIndent.cx, -iILBSettings.sizeIndent.cy);
			RECT rcText = { 0 };
			int res = 0;
			res = dc.DrawText(pItem->pszText, -1, &rcText, format | DT_CALCRECT);
			if ( 0 == res )
			{
				DWORD err = ::GetLastError();
				LOGMSG("error: %d", err);
			}

			res = dc.DrawText(pItem->pszText, -1, &rcItem, format);
			if ( 0 == res )
			{
				DWORD err = ::GetLastError();
				LOGMSG("error: %d", err);
			}

			rcItem.top += rcText.bottom-rcText.top;
		}
		// Draw subtext
		if( pItem->mask & ILBIF_SUBTEXT )
		{
			if( iFontSub != NULL ) dc.SelectFont(iFontSub);
			::InflateRect(&rcItem, -iILBSettings.sizeSubIndent.cx, 0);
			rcItem.top += iILBSettings.sizeSubIndent.cy;
			RECT rcText = { 0 };
			int res = 0;
			res = dc.DrawText(pItem->pszSubText, -1, &rcText, format | DT_CALCRECT);
			if ( 0 == res )
			{
				DWORD err = ::GetLastError();
				LOGMSG("error: %d", err);
			}

			res = dc.DrawText(pItem->pszSubText, -1, &rcItem, format);
			if ( 0 == res )
			{
				DWORD err = ::GetLastError();
				LOGMSG("error: %d", err);
			}

			rcItem.top += rcText.bottom-rcText.top;
		}

		dc.SelectFont(hOldFont);

		return CDRF_SKIPDEFAULT;
	}
};

class CImageListbox : public CImageListboxImpl<CImageListbox>
{
public:
	DECLARE_WND_SUPERCLASS(_T("WTL_n2fImageListBox"), GetWndClassName())  
};
