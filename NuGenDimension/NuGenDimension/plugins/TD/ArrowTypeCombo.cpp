// ArrowTypeCombo.cpp : implementation file
//

#include "stdafx.h"
#include "ArrowTypeCombo.h"

#include "resource.h"

// CArrowTypeCombo

IMPLEMENT_DYNAMIC(CArrowTypeCombo, CComboBox)
CArrowTypeCombo::CArrowTypeCombo()
{
	m_left_to_right = true;
}

CArrowTypeCombo::~CArrowTypeCombo()
{
}

void  CArrowTypeCombo::SetType(bool l_To_r)
{
	SWITCH_RESOURCE
	m_left_to_right = l_To_r;
	if (m_left_to_right)
	{
		m_bitmaps[0].LoadBitmap(IDB_BITMAP2);
		m_bitmaps[1].LoadBitmap(IDB_BITMAP3);
		m_bitmaps[2].LoadBitmap(IDB_BITMAP5);
		m_bitmaps[3].LoadBitmap(IDB_BITMAP7);
		m_bitmaps[4].LoadBitmap(IDB_BITMAP9);
		m_bitmaps[5].LoadBitmap(IDB_BITMAP11);
		m_bitmaps[6].LoadBitmap(IDB_BITMAP13);
		m_bitmaps[7].LoadBitmap(IDB_BITMAP15);
		m_bitmaps[8].LoadBitmap(IDB_BITMAP17);
		m_bitmaps[9].LoadBitmap(IDB_BITMAP19);
		m_bitmaps[10].LoadBitmap(IDB_BITMAP21);
		m_bitmaps[11].LoadBitmap(IDB_BITMAP23);
		m_bitmaps[12].LoadBitmap(IDB_BITMAP25);
		m_bitmaps[13].LoadBitmap(IDB_BITMAP27);
		m_bitmaps[14].LoadBitmap(IDB_BITMAP29);
		m_bitmaps[15].LoadBitmap(IDB_BITMAP31);
	}
	else
	{
		m_bitmaps[0].LoadBitmap(IDB_BITMAP2);
		m_bitmaps[1].LoadBitmap(IDB_BITMAP4);
		m_bitmaps[2].LoadBitmap(IDB_BITMAP6);
		m_bitmaps[3].LoadBitmap(IDB_BITMAP8);
		m_bitmaps[4].LoadBitmap(IDB_BITMAP10);
		m_bitmaps[5].LoadBitmap(IDB_BITMAP12);
		m_bitmaps[6].LoadBitmap(IDB_BITMAP14);
		m_bitmaps[7].LoadBitmap(IDB_BITMAP16);
		m_bitmaps[8].LoadBitmap(IDB_BITMAP18);
		m_bitmaps[9].LoadBitmap(IDB_BITMAP20);
		m_bitmaps[10].LoadBitmap(IDB_BITMAP22);
		m_bitmaps[11].LoadBitmap(IDB_BITMAP24);
		m_bitmaps[12].LoadBitmap(IDB_BITMAP26);
		m_bitmaps[13].LoadBitmap(IDB_BITMAP28);
		m_bitmaps[14].LoadBitmap(IDB_BITMAP30);
		m_bitmaps[15].LoadBitmap(IDB_BITMAP32);
	}
}

void CArrowTypeCombo::MeasureItem(LPMEASUREITEMSTRUCT lpMIS)
{ 
	//lpMIS->itemWidth = (m_nItemWidth + 2);
	//lpMIS->itemHeight = (m_nItemHeight + 2);
}
static void DrawBitmap(const CBitmap *bitmap, const CDC *pDC, const CPoint &point)
{
	BITMAP bm; ((CBitmap*)bitmap)->GetBitmap(&bm);
	int w = bm.bmWidth; 
	int h = bm.bmHeight;
	CDC memDC; memDC.CreateCompatibleDC((CDC*)pDC);
	CBitmap *pBmp = memDC.SelectObject((CBitmap*)bitmap);
	((CDC*)pDC)->BitBlt(point.x, point.y, w, h, &memDC, 0, 0, SRCCOPY);
	memDC.SelectObject(pBmp);
}
static void DrawBitmap(const CBitmap *bitmap, const CDC *pDC, const CRect &rect)
{
	BITMAP bm; ((CBitmap*)bitmap)->GetBitmap(&bm);
	int w = bm.bmWidth; 
	int h = bm.bmHeight;
	CPoint point;
	point.x = rect.left + ((rect.right - rect.left) / 2) - (w / 2);
	point.y = rect.top + ((rect.bottom - rect.top) / 2) - (h / 2);
	DrawBitmap(bitmap, pDC, point);
}

void CArrowTypeCombo::DrawItem(LPDRAWITEMSTRUCT lpDIS)
{
	CDC* pDC = CDC::FromHandle(lpDIS->hDC);

	COLORREF	crNormal = RGB(255,255,255);

	pDC->SetBkColor( crNormal );					// Set BG To Highlight Color
	pDC->FillSolidRect( &lpDIS->rcItem, crNormal );	// Erase Item

	if( lpDIS -> itemState & ODS_FOCUS )								// If Item Has The Focus
	{
		pDC->DrawFocusRect( &lpDIS->rcItem );				// Draw Focus Rect
	}

	/*Drawer::DrawStylingLine(Drawer::GetLineTypeByIndex(lpDIS->itemID),
		pDC,CPoint(lpDIS->rcItem.left,
		lpDIS->rcItem.top+(lpDIS->rcItem.bottom-lpDIS->rcItem.top)/2),
		lpDIS->rcItem.right - lpDIS->rcItem.left);*/
	int  rL = lpDIS->rcItem.left;
	int  rR = lpDIS->rcItem.left;
	int  vertMid = lpDIS->rcItem.top+(lpDIS->rcItem.bottom-lpDIS->rcItem.top)/2;
	switch(lpDIS->itemID) {
	case 0:
	case 1:
	case 2:
	case 3:
	case 4:
	case 5:
	case 6:
	case 7:
	case 8:
	case 9:
	case 10:
	case 11:
	case 12:
	case 13:
	case 14:
	case 15:
		DrawBitmap(&m_bitmaps[lpDIS->itemID],pDC,lpDIS->rcItem);
		break;
	default:
		ASSERT(0);
	}

}
//----------------------------------------------------------------------------

#ifdef _DEBUG
void CArrowTypeCombo::PreSubclassWindow() 
{
	CComboBox::PreSubclassWindow();

	// ensure some styles are set
	// modifying style here has NO effect!?!
	ASSERT(GetStyle() & CBS_DROPDOWNLIST);
	ASSERT(GetStyle() & CBS_OWNERDRAWFIXED);
	//ASSERT(GetStyle() & CBS_HASSTRINGS);
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	SetCurSel(0);
}
#endif

//----------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CArrowTypeCombo, CComboBox)
END_MESSAGE_MAP()



// CArrowTypeCombo message handlers

