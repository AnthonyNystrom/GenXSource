// TextParamsDlg.cpp : implementation file
//

#include "stdafx.h"
#include "TextParamsDlg.h"
#include ".\textparamsdlg.h"

// CTextParamsDlg dialog

IMPLEMENT_DYNAMIC(CTextParamsDlg, CDialog)
CTextParamsDlg::CTextParamsDlg(SG_TEXT_STYLE* txt_stl,
							   IApplicationInterface* appI, 
							   CWnd* pParent /*=NULL*/)
	: CDialog(CTextParamsDlg::IDD, pParent)
{
	ASSERT(txt_stl);
	ASSERT(appI);
	m_app = appI;
	m_cur_text_style = txt_stl;
	m_inFocus = false;
}

CTextParamsDlg::~CTextParamsDlg()
{
}

void CTextParamsDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_SYM_HEIGHT_EDIT, m_sym_height);
	DDX_Control(pDX, IDC_SYM_ANGLE_EDIT, m_angle);
	DDX_Control(pDX, IDC_LINES_SPACE_EDIT, m_lines_space);
	DDX_Control(pDX, IDC_SYMB_SPACE_EDIT, m_symb_space);
	DDX_Control(pDX, IDC_SYMB_PROPOPT_EDIT, m_s_prop);
}


BEGIN_MESSAGE_MAP(CTextParamsDlg, CDialog)
	ON_WM_CHAR()
	ON_EN_CHANGE(IDC_SYM_ANGLE_EDIT, OnEnChangeSymAngleEdit)
	ON_EN_CHANGE(IDC_SYM_HEIGHT_EDIT, OnEnChangeSymHeightEdit)
	ON_EN_CHANGE(IDC_SYMB_SPACE_EDIT, OnEnChangeSymbSpaceEdit)
	ON_EN_CHANGE(IDC_LINES_SPACE_EDIT, OnEnChangeLinesSpaceEdit)
	ON_EN_CHANGE(IDC_SYMB_PROPOPT_EDIT, OnEnChangeSymbPropoptEdit)
	ON_WM_SIZE()
	ON_WM_ERASEBKGND()
	ON_WM_CTLCOLOR()
	
	ON_EN_SETFOCUS(IDC_SYM_ANGLE_EDIT, OnEnSetfocusTextEdit)
	ON_EN_KILLFOCUS(IDC_SYM_ANGLE_EDIT, OnEnKillfocusTextEdit)
	ON_EN_SETFOCUS(IDC_SYM_HEIGHT_EDIT, OnEnSetfocusTextEdit)
	ON_EN_KILLFOCUS(IDC_SYM_HEIGHT_EDIT, OnEnKillfocusTextEdit)
	ON_EN_SETFOCUS(IDC_SYMB_SPACE_EDIT, OnEnSetfocusTextEdit)
	ON_EN_KILLFOCUS(IDC_SYMB_SPACE_EDIT, OnEnKillfocusTextEdit)
	ON_EN_SETFOCUS(IDC_LINES_SPACE_EDIT, OnEnSetfocusTextEdit)
	ON_EN_KILLFOCUS(IDC_LINES_SPACE_EDIT, OnEnKillfocusTextEdit)
	ON_EN_SETFOCUS(IDC_SYMB_PROPOPT_EDIT, OnEnSetfocusTextEdit)
	ON_EN_KILLFOCUS(IDC_SYMB_PROPOPT_EDIT, OnEnKillfocusTextEdit)
END_MESSAGE_MAP()


// CTextParamsDlg message handlers

static DWORD GetTextExtent(HDC hDC, LPCSTR s, int len)
{
	SIZE dim;
	DWORD dw;
	GetTextExtentPoint32(hDC, s, len, &dim);
	dw = ((dim.cy << 16) & 0xFFFF0000)| dim.cx;
	return dw;
}

BOOL CTextParamsDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	/*m_Previw_wnd.SetTextStylePointer(&m_cur_text_style);
	m_Previw_wnd.SetCurFont(m_cur_font);
	CRect prR;
	GetDlgItem(IDC_PREVIEW_FRAME)->GetWindowRect(prR);
	ScreenToClient(prR);
	InflateRect(&prR,-3,-3);
	m_Previw_wnd.Create(NULL,NULL,WS_CHILD|WS_VISIBLE,prR,this,101);*/
	
	if (!sgFontManager::GetFont(sgFontManager::GetCurrentFont()))
		return false;

	m_sym_height.SetValue((float)m_cur_text_style->height);
	m_angle.SetValue((float)m_cur_text_style->angle);
	m_lines_space.SetValue((float)m_cur_text_style->vert_space_proportion);
	m_symb_space.SetValue((float)m_cur_text_style->horiz_space_proportion);
	m_s_prop.SetValue((float)m_cur_text_style->proportions);

	
	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CTextParamsDlg::OnOK()
{
}

void CTextParamsDlg::OnCancel()
{
}

static void setlitext_1(DWORD_PTR i, char *buf) // #NOT USED
{
	buf[0] = buf[1] = '%';
	buf[2] = buf[3] = buf[4] = buf[5] = 0;
	if(i < 100) buf[2] = '0';
	if(i < 10) buf[3] = '0';
	//_itoa(i, &buf[lstrlen(buf)], 10);#OBSOLETE RISK
	_itoa_s(i,&buf[lstrlen(buf)],1024,10);
}

void CTextParamsDlg::OnChar(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	CDialog::OnChar(nChar, nRepCnt, nFlags);
}

void CTextParamsDlg::OnEnChangeSymAngleEdit()
{
	if (::IsWindow(m_angle.m_hWnd))
	{
		m_cur_text_style->angle = m_angle.GetValue();
		if (m_cur_text_style->angle>90.0)
			m_cur_text_style->angle=90.0;
		if (m_cur_text_style->angle<0.0)
			m_cur_text_style->angle=0.0;
		//m_angle.SetValue((float)m_cur_text_style->angle);
	}
	if (m_app)
		m_app->GetViewPort()->InvalidateViewPort();
}

void CTextParamsDlg::OnEnChangeSymHeightEdit()
{
		//UpdateData();
		m_cur_text_style->height = m_sym_height.GetValue();
		//m_Previw_wnd.ChangeStyle();
		if (m_app)
			m_app->GetViewPort()->InvalidateViewPort();
}

void CTextParamsDlg::OnEnChangeSymbSpaceEdit()
{
		//UpdateData();
		m_cur_text_style->horiz_space_proportion = m_symb_space.GetValue();
		if (m_app)
			m_app->GetViewPort()->InvalidateViewPort();
}

void CTextParamsDlg::OnEnChangeLinesSpaceEdit()
{
		//UpdateData();
		m_cur_text_style->vert_space_proportion = m_lines_space.GetValue();
		if (m_app)
			m_app->GetViewPort()->InvalidateViewPort();
}

void CTextParamsDlg::OnEnChangeSymbPropoptEdit()
{

		//UpdateData();
   m_cur_text_style->proportions = m_s_prop.GetValue();
   if (m_app)
	   m_app->GetViewPort()->InvalidateViewPort();
}

void CTextParamsDlg::OnSize(UINT nType, int cx, int cy)
{
	__super::OnSize(nType, cx, cy);

	if (::IsWindow(m_sym_height.m_hWnd))
	{
		CRect rrr;
	
		m_sym_height.GetWindowRect(rrr);
		ScreenToClient(rrr);
		m_sym_height.MoveWindow(rrr.left,rrr.top,cx-2*rrr.left,rrr.Height());

		m_angle.GetWindowRect(rrr);
		ScreenToClient(rrr);
		m_angle.MoveWindow(rrr.left,rrr.top,cx-2*rrr.left,rrr.Height());

		m_lines_space.GetWindowRect(rrr);
		ScreenToClient(rrr);
		m_lines_space.MoveWindow(rrr.left,rrr.top,cx-2*rrr.left,rrr.Height());

		m_symb_space.GetWindowRect(rrr);
		ScreenToClient(rrr);
		m_symb_space.MoveWindow(rrr.left,rrr.top,cx-2*rrr.left,rrr.Height());

		m_s_prop.GetWindowRect(rrr);
		ScreenToClient(rrr);
		m_s_prop.MoveWindow(rrr.left,rrr.top,cx-2*rrr.left,rrr.Height());
	}
}

BOOL CTextParamsDlg::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	return TRUE;//__super::OnEraseBkgnd(pDC);
}

HBRUSH CTextParamsDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	// Call the base class implementation first! Otherwise, it may
	// undo what we are trying to accomplish here.
	HBRUSH hbr = CDialog::OnCtlColor(pDC, pWnd, nCtlColor);

	if (m_angle.m_hWnd==pWnd->m_hWnd ||
		m_s_prop.m_hWnd==pWnd->m_hWnd ||
		m_symb_space.m_hWnd==pWnd->m_hWnd)
			return hbr;

	if (nCtlColor==CTLCOLOR_STATIC && (pWnd!=&m_sym_height))
	{
		pDC->SetTextColor(0);
		//pDC->SetBkColor(RGB(255,255,255));
		pDC->SetBkMode(TRANSPARENT);
		hbr = (HBRUSH)GetStockObject(HOLLOW_BRUSH);
	}

	return hbr;
}


void CTextParamsDlg::OnEnSetfocusTextEdit()
{
	m_inFocus = true;
}

void CTextParamsDlg::OnEnKillfocusTextEdit()
{
	m_inFocus = false;
}


