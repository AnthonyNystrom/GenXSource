#include "stdafx.h"
#include "LineStyleCombo.h"
#include "..//Drawer.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

CLineStyleCombo::CLineStyleCombo():
  CComboBox()
{
}
void CLineStyleCombo::MeasureItem(LPMEASUREITEMSTRUCT lpMIS)
{ 
  //lpMIS->itemWidth = (m_nItemWidth + 2);
  //lpMIS->itemHeight = (m_nItemHeight + 2);
}

void CLineStyleCombo::DrawItem(LPDRAWITEMSTRUCT lpDIS)
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

	Drawer::DrawStylingLine(Drawer::GetLineTypeByIndex(lpDIS->itemID),
		pDC,CPoint(lpDIS->rcItem.left,
			lpDIS->rcItem.top+(lpDIS->rcItem.bottom-lpDIS->rcItem.top)/2),
			lpDIS->rcItem.right - lpDIS->rcItem.left);
	/*pDC->MoveTo(lpDIS->rcItem.left,
		lpDIS->rcItem.top+(lpDIS->rcItem.bottom-lpDIS->rcItem.top)/2);//lpDIS->rcItem.bottom/2);
	pDC->LineTo(lpDIS->rcItem.right,
		lpDIS->rcItem.top+(lpDIS->rcItem.bottom-lpDIS->rcItem.top)/2);//2);
*/
	pDC->SelectObject(pOldPen);
}
//----------------------------------------------------------------------------

#ifdef _DEBUG
void CLineStyleCombo::PreSubclassWindow() 
{
  CComboBox::PreSubclassWindow();

  // ensure some styles are set
  // modifying style here has NO effect!?!
  ASSERT(GetStyle() & CBS_DROPDOWNLIST);
  ASSERT(GetStyle() & CBS_OWNERDRAWFIXED);
  //ASSERT(GetStyle() & CBS_HASSTRINGS);
}
#endif

//----------------------------------------------------------------------------
BEGIN_MESSAGE_MAP(CLineStyleCombo, CComboBox)
	ON_WM_CREATE()
END_MESSAGE_MAP()

int CLineStyleCombo::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CComboBox::OnCreate(lpCreateStruct) == -1)
		return -1;

	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");
	AddString(" sd");


	return 0;
}
