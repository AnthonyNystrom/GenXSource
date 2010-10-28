// ToolTipBitmapButton.cpp : implementation file
//

#include "stdafx.h"
#include "ToolTipButton.h"
#include ".\tooltipbutton.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CToolTipButton

CToolTipButton::CToolTipButton()
{
}

CToolTipButton::~CToolTipButton()
{
}


BEGIN_MESSAGE_MAP(CToolTipButton, baseCToolTipButton)
	//{{AFX_MSG_MAP(CToolTipButton)
		// NOTE - the ClassWizard will add and remove mapping macros here.
	//}}AFX_MSG_MAP
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CToolTipButton message handlers

// Set the tooltip with a CString
void CToolTipButton::SetToolTipText(LPCTSTR spText, BOOL bActivate)
{
	// We cannot accept NULL pointer
	if (spText == NULL) return;

	// Initialize ToolTip
	InitToolTip();

	// If there is no tooltip defined then add it
	if (m_ToolTip.GetToolCount() == 0)
	{
		CRect rectBtn; 
		GetClientRect(rectBtn);
		m_ToolTip.AddTool(this, spText, rectBtn, 1);
	}

	// Set text for tooltip
	m_ToolTip.UpdateTipText(spText, this, 1);
	m_ToolTip.Activate(bActivate);
}

void CToolTipButton::InitToolTip()
{
	if (m_ToolTip.m_hWnd == NULL)
	{
		// Create ToolTip control
		m_ToolTip.Create(this);
		// Create active
		m_ToolTip.Activate(TRUE);
	}
} // End of InitToolTip


BOOL CToolTipButton::PreTranslateMessage(MSG* pMsg) 
{
	try { //#try
		// TODO: Add your specialized code here and/or call the base class
		InitToolTip();
		m_ToolTip.RelayEvent(pMsg);		
	}
	catch(...){
	}
	return baseCToolTipButton::PreTranslateMessage(pMsg);
}



BOOL CToolTipButton::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	return /*TRUE;//*/CButton::OnEraseBkgnd(pDC);
}
