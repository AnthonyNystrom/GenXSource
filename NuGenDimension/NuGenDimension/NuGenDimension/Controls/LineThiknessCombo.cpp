#include "stdafx.h"
#include "LineThiknessCombo.h"
#include ".\linethiknesscombo.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

CLineThiknessCombo::CLineThiknessCombo():
  CComboBox()
{
}

void CLineThiknessCombo::MeasureItem(LPMEASUREITEMSTRUCT lpMIS)
{ 
  //lpMIS->itemWidth = 100;//(m_nItemWidth + 2);
  //lpMIS->itemHeight = 20;//(m_nItemHeight + 2);
}

void CLineThiknessCombo::DrawItem(LPDRAWITEMSTRUCT lpDIS)
{
  CDC* pDC = CDC::FromHandle(lpDIS->hDC);

  COLORREF	crNormal = GetSysColor( COLOR_WINDOW );
 
  pDC->SetBkColor( crNormal );					// Set BG To Highlight Color
  pDC->FillSolidRect( &lpDIS->rcItem, crNormal );	// Erase Item
  
  if( lpDIS -> itemState & ODS_FOCUS )								// If Item Has The Focus
  {
	  pDC->DrawFocusRect( &lpDIS->rcItem );				// Draw Focus Rect
  }
  
    CPen penHighlight(PS_SOLID, lpDIS->itemID+1, RGB(0,0,0));
    CPen* pOldPen = pDC->SelectObject(&penHighlight);

    pDC->MoveTo(lpDIS->rcItem.left,
				lpDIS->rcItem.top+(lpDIS->rcItem.bottom-lpDIS->rcItem.top)/2);//lpDIS->rcItem.bottom/2);
	pDC->LineTo(lpDIS->rcItem.right,
				lpDIS->rcItem.top+(lpDIS->rcItem.bottom-lpDIS->rcItem.top)/2);//2);
  
    pDC->SelectObject(pOldPen);
 
}
//----------------------------------------------------------------------------

#ifdef _DEBUG
void CLineThiknessCombo::PreSubclassWindow() 
{
  CComboBox::PreSubclassWindow();

  ASSERT(GetStyle() & CBS_DROPDOWNLIST);
  ASSERT(GetStyle() & CBS_OWNERDRAWFIXED);
}
#endif

//----------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CLineThiknessCombo, CComboBox)
	ON_WM_CREATE()
END_MESSAGE_MAP()

int CLineThiknessCombo::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CComboBox::OnCreate(lpCreateStruct) == -1)
		return -1;

	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	
	return 0;
}
