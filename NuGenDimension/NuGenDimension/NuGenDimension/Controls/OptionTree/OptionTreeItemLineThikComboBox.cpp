#include "stdafx.h"
#include "OptionTreeItemLineThikComboBox.h"

// Added Headers
#include "OptionTree.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// COptionTreeItemLineThikComboBox

COptionTreeItemLineThikComboBox::COptionTreeItemLineThikComboBox()
{
	// Initialize variables
	m_bFocus = FALSE;
	m_lDropDownHeight = OT_COMBO_DROPDOWNHEIGHT;

	m_line_thickness = 0;


	// Set item type
	SetItemType(OT_ITEM_COMBOBOX);
}

COptionTreeItemLineThikComboBox::~COptionTreeItemLineThikComboBox()
{
}


BEGIN_MESSAGE_MAP(COptionTreeItemLineThikComboBox, CLineThiknessCombo)
	//{{AFX_MSG_MAP(COptionTreeItemLineThikComboBox)
	ON_WM_SETFOCUS()
	ON_WM_KILLFOCUS()
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// COptionTreeItemLineThikComboBox message handlers


void COptionTreeItemLineThikComboBox::DrawAttribute(CDC *pDC, const RECT &rcRect)
{
	// If we don't have focus, text is drawn.
	if (m_bFocus == TRUE)
	{
		return;
	}

	// Make sure options aren't NULL
	if (m_otOption == NULL)
	{
		return;
	}

	// Make sure there is a window
	if (!IsWindow(GetSafeHwnd()))
	{
		return;
	}

	// Set window position
	if (IsWindow(GetSafeHwnd()))
	{
		MoveWindow(m_rcAttribute.left, m_rcAttribute.top, m_rcAttribute.Width(), m_rcAttribute.Height());
	}

	COLORREF	crNormal = GetSysColor( COLOR_WINDOW );

	CRect rrr = rcRect;
	pDC->SetBkColor( crNormal );					// Set BG To Highlight Color
	pDC->FillSolidRect( &rrr, crNormal );	// Erase Item

	CPen penHighlight(PS_SOLID, m_line_thickness+1, RGB(0,0,0));
	CPen* pOldPen = pDC->SelectObject(&penHighlight);

	pDC->MoveTo(rcRect.left,
		rcRect.top+(rcRect.bottom-rcRect.top)/2);
	pDC->LineTo(rcRect.right,
		rcRect.top+(rcRect.bottom-rcRect.top)/2);

	pDC->SelectObject(pOldPen);
}

void COptionTreeItemLineThikComboBox::OnCommit()
{
	// Hide edit control
	if (IsWindow(GetSafeHwnd()))
	{
		// -- Show window
		ShowWindow(SW_HIDE);
	}
	m_line_thickness = GetCurSel();
}

void COptionTreeItemLineThikComboBox::OnRefresh()
{
	// Set the window text
	if (IsWindow(GetSafeHwnd()))
	{
		MoveWindow(m_rcAttribute.left, m_rcAttribute.top, m_rcAttribute.Width(), m_rcAttribute.Height());
	}
}

void COptionTreeItemLineThikComboBox::OnMove()
{
	// Set window position
	if (IsWindow(GetSafeHwnd()))
	{
		MoveWindow(m_rcAttribute.left, m_rcAttribute.top, m_rcAttribute.Width(), m_rcAttribute.Height());
	}

	// Hide window
	if (m_bFocus == FALSE && IsWindow(GetSafeHwnd()))
	{
		// -- Show window
		ShowWindow(SW_HIDE);
	}
}

void COptionTreeItemLineThikComboBox::OnActivate()
{
	// Make sure window is valid
	if (IsWindow(GetSafeHwnd()))
	{

		// -- Show window
		ShowWindow(SW_SHOW);

		// -- Set window position
		MoveWindow(m_rcAttribute.left, m_rcAttribute.top, m_rcAttribute.Width(), m_rcAttribute.Height() + m_lDropDownHeight);

		// -- Set focus
		SetFocus();

		SetCurSel(m_line_thickness);
	}
}

void COptionTreeItemLineThikComboBox::OnSetFocus(CWnd* pOldWnd) 
{
	// Mark focus
	m_bFocus = TRUE;

	CLineThiknessCombo::OnSetFocus(pOldWnd);	
}

void COptionTreeItemLineThikComboBox::OnKillFocus(CWnd* pNewWnd) 
{
	// Validate
	if (m_otOption == NULL)
	{
		CLineThiknessCombo::OnKillFocus(pNewWnd);
		return;
	}

	// See if new window is tree of list
	if (m_otOption->IsChild(pNewWnd) == TRUE)
	{
		// -- Mark focus
		m_bFocus = FALSE;

		// -- Commit changes
		CommitChanges();
	}

	CLineThiknessCombo::OnKillFocus(pNewWnd);	
}

BOOL COptionTreeItemLineThikComboBox::CreateComboItem(DWORD dwAddStyle)
{
	// Declare variables
	DWORD dwStyle = WS_CHILD | WS_VISIBLE | 
		CBS_DROPDOWNLIST | CBS_OWNERDRAWFIXED|WS_VSCROLL;
	BOOL bRet = FALSE;

	// Make sure options is not NULL
	if (m_otOption == NULL)
	{
		return FALSE;
	}

	// Create edit control
	if (!IsWindow(GetSafeHwnd()))
	{
		// -- Add style
		if (dwAddStyle != 0)
		{
			dwStyle |= dwAddStyle;
		}
		
		// -- Create the combo box
		bRet = Create(dwStyle, m_rcAttribute, m_otOption->GetCtrlParent(), GetCtrlID());

		// -- Setup combo
		if (bRet == TRUE)
		{
			// -- -- Set font
			SetFont(m_otOption->GetNormalFont(), TRUE);

			// -- -- Set window position
			MoveWindow(m_rcAttribute.left, m_rcAttribute.top, m_rcAttribute.Width(), m_rcAttribute.Height());		

			// -- -- Hide window
			ShowWindow(SW_HIDE);
		}
	}


	return bRet;
}

void COptionTreeItemLineThikComboBox::SetDropDownHeight(long lHeight)
{
	// Save variable
	m_lDropDownHeight = lHeight;
}

long COptionTreeItemLineThikComboBox::GetDropDownHeight()
{
	// Return variable
	return m_lDropDownHeight;
}

void COptionTreeItemLineThikComboBox::CleanDestroyWindow()
{
	// Destroy window
	if (IsWindow(GetSafeHwnd()))
	{
		// -- Destroy window
		DestroyWindow();
	}
}
void COptionTreeItemLineThikComboBox::OnDeSelect()
{
	// Hide window
	if (IsWindow(GetSafeHwnd()))
	{
		// -- Show window
		ShowWindow(SW_HIDE);
	}
}

void COptionTreeItemLineThikComboBox::OnSelect()
{
	// Do nothing here
}