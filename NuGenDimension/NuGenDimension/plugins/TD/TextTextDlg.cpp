// TextTextDlg.cpp : implementation file
//

#include "stdafx.h"
#include "TextTextDlg.h"
#include ".\texttextdlg.h"


// CTextTextDlg dialog

IMPLEMENT_DYNAMIC(CTextTextDlg, CDialog)
CTextTextDlg::CTextTextDlg(IApplicationInterface* appI,CWnd* pParent /*=NULL*/)
	: CDialog(CTextTextDlg::IDD, pParent)
{
	ASSERT(appI);
	m_app=appI;
	m_inFocus = false;
}

CTextTextDlg::~CTextTextDlg()
{
}

void CTextTextDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_TEXT_TEXT_EDIT, m_text_edit);

}


BEGIN_MESSAGE_MAP(CTextTextDlg, CDialog)
	ON_EN_SETFOCUS(IDC_TEXT_TEXT_EDIT, OnEnSetfocusTextEdit)
	ON_EN_KILLFOCUS(IDC_TEXT_TEXT_EDIT, OnEnKillfocusTextEdit)
	ON_WM_SIZE()
	ON_WM_ERASEBKGND()
	ON_EN_CHANGE(IDC_TEXT_TEXT_EDIT, OnEnChangeTextTextEdit)
END_MESSAGE_MAP()


void  CTextTextDlg::GetText(CString& ttt)
{
	//ttt = m_target_text;
	m_text_edit.GetWindowText(ttt);
}

void  CTextTextDlg::SetText(const char* lll)
{
	m_text_edit.SetWindowText(lll);
}

// CTextTextDlg message handlers

void CTextTextDlg::OnSize(UINT nType, int cx, int cy)
{
	__super::OnSize(nType, cx, cy);

	if (::IsWindow(m_text_edit.m_hWnd))
	{
		CRect rrr;
		m_text_edit.GetWindowRect(rrr);
		ScreenToClient(rrr);
		m_text_edit.MoveWindow(rrr.left,rrr.top,cx-2*rrr.left,rrr.Height());
	}
}

BOOL CTextTextDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	return TRUE;//__super::OnEraseBkgnd(pDC);
}


void CTextTextDlg::OnChar(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	if (nChar==VK_RETURN)
	{
		OnOK();
		return;
	}
	CDialog::OnChar(nChar, nRepCnt, nFlags);
}

BOOL CTextTextDlg::OnInitDialog()
{
	__super::OnInitDialog();

	m_text_edit.SetWindowText("Text Text \r\n  Text");
	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CTextTextDlg::OnEnSetfocusTextEdit()
{
	m_inFocus = true;
}

void CTextTextDlg::OnEnKillfocusTextEdit()
{
	m_inFocus = false;
}

void CTextTextDlg::OnEnChangeTextTextEdit()
{
	if (m_app)
		m_app->GetViewPort()->InvalidateViewPort();
}
