// CursorSheetDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "CursorSheetDlg.h"
#include ".\cursorsheetdlg.h"


// CCursorSheetDlg dialog

IMPLEMENT_DYNAMIC(CCursorSheetDlg, CDialog)
CCursorSheetDlg::CCursorSheetDlg(CCursorer* crsrr, CWnd* pParent /*=NULL*/)
	: CDialog(CCursorSheetDlg::IDD, pParent)
	, m_cur_type(FALSE)
	, m_cursorer(crsrr)
{
}

CCursorSheetDlg::~CCursorSheetDlg()
{
}

void CCursorSheetDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_CURSOR_SIZE_COMBO, m_size_combo);
	DDX_Radio(pDX, IDC_CURSOR_TYPE_RADIO1, m_cur_type);
	DDX_Control(pDX, IDC_CURSOR_IN_COL, m_in_color);
	DDX_Control(pDX, IDC_CURSOR_OUT_COL, m_out_col);
}


BEGIN_MESSAGE_MAP(CCursorSheetDlg, CDialog)
	ON_BN_CLICKED(IDC_CURSOR_TYPE_RADIO1, OnBnClickedCursorTypeRadio1)
	ON_BN_CLICKED(IDC_CURSOR_TYPE_RADIO2, OnBnClickedCursorTypeRadio2)
	ON_CBN_SELCHANGE(IDC_CURSOR_SIZE_COMBO, OnCbnSelchangeCursorSizeCombo)
	ON_CBN_SELCHANGE(IDC_CURSOR_OUT_COL, OnCbnSelchangeCursorOutCol)
	ON_CBN_SELCHANGE(IDC_CURSOR_IN_COL, OnCbnSelchangeCursorInCol)
	ON_WM_PAINT()
END_MESSAGE_MAP()


// CCursorSheetDlg message handlers

BOOL CCursorSheetDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	CRect rrr;
	m_size_combo.GetWindowRect(rrr);

	m_in_color.SetItemHeight(-1,rrr.Height()-6);
	m_in_color.InitializeDefaultColors();
	m_out_col.SetItemHeight(-1,rrr.Height()-6);
	m_out_col.InitializeDefaultColors();

	for (int i=2;i<MAX_CURSOR_SIZE;i++)
	{
		CString sss;
		sss.Format("%i",i);
		m_size_combo.AddString(sss);
	}
	CURSOR_STRUCTURE *c = m_cursorer->GetCursorStructure();
	m_cur_type = !(c->isRound);
	m_in_color.SetCurSel(c->insideColor);
	m_out_col.SetCurSel(c->outsideColor);
	m_size_combo.SetCurSel(c->size-2);	

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}

void CCursorSheetDlg::OnCancel()
{
}

void CCursorSheetDlg::OnOK()
{
}

void CCursorSheetDlg::OnBnClickedCursorTypeRadio1()
{
	m_cur_type = FALSE;
	CRect rrr;
	GetDlgItem(IDC_CURSOR_PREVIEW)->GetWindowRect(rrr);
	ScreenToClient(rrr);

	InvalidateRect(rrr,FALSE);
}

void CCursorSheetDlg::OnBnClickedCursorTypeRadio2()
{
	m_cur_type = TRUE;
	CRect rrr;
	GetDlgItem(IDC_CURSOR_PREVIEW)->GetWindowRect(rrr);
	ScreenToClient(rrr);

	InvalidateRect(rrr,FALSE);
}

void CCursorSheetDlg::OnCbnSelchangeCursorSizeCombo()
{
	CRect rrr;
	GetDlgItem(IDC_CURSOR_PREVIEW)->GetWindowRect(rrr);
	ScreenToClient(rrr);

	InvalidateRect(rrr,FALSE);
}

void CCursorSheetDlg::OnCbnSelchangeCursorOutCol()
{
	// TODO: Add your control notification handler code here
}

void CCursorSheetDlg::OnCbnSelchangeCursorInCol()
{
	// TODO: Add your control notification handler code here
}

void CCursorSheetDlg::OnPaint()
{
	CPaintDC dc(this); // device context for painting
	
	CRect rrr;
	GetDlgItem(IDC_CURSOR_PREVIEW)->GetWindowRect(rrr);
	ScreenToClient(rrr);
/*
  COLORREF OutsideColor = RGB((int)(Drawer::GetColorByIndex(m_out_col.GetCurSel())[0]*255.0f),
		(int)(Drawer::GetColorByIndex(m_out_col.GetCurSel())[1]*255.0f),
		(int)(Drawer::GetColorByIndex(m_out_col.GetCurSel())[2]*255.0f));
  COLORREF InsideColor = RGB((int)(Drawer::GetColorByIndex(m_in_color.GetCurSel())[0]*255.0f),
	  (int)(Drawer::GetColorByIndex(m_in_color.GetCurSel())[1]*255.0f),
	  (int)(Drawer::GetColorByIndex(m_in_color.GetCurSel())[2]*255.0f));

  if (m_in_color.GetCurSel()==0)
	  InsideColor = RGB(1,1,1);
  if (m_out_col.GetCurSel()==0)
	  OutsideColor = RGB(1,1,1);
  HPEN    hPen    = ::CreatePen(PS_SOLID,1,OutsideColor);
  HPEN    hPen1   = ::CreatePen(PS_SOLID,1,InsideColor);
  HPEN    hOldPen   = (HPEN)::SelectObject(dc,hPen);
*/

	dc.FillSolidRect(rrr,GetSysColor(COLOR_BTNSHADOW));

	short cenX = (short)(rrr.left+rrr.Width()/2);
	short cenY = (short)(rrr.top+rrr.Height()/2);

	short rad = (short)(m_size_combo.GetCurSel()+5);

	if (!m_cur_type)
	{
		dc.Ellipse(cenX-rad,cenY-rad,cenX+rad+1,cenY+rad+1);
	}
	else
	{
		dc.Rectangle(cenX-rad,cenY-rad,cenX+rad+1,cenY+rad+1);
	}

	dc.SetPixel(cenX,cenY,RGB(0,0,0));
//	::SelectObject(dc,hOldPen);
}
