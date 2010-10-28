// InfoBar.cpp : implementation file
//

#include "stdafx.h"
#include "InfoBar.h"
#include ".\infobar.h"


// CInfoBar

IMPLEMENT_DYNAMIC(CInfoBar, CDialogBar)
CInfoBar::CInfoBar()
{
	m_type = INFO_TEXT;
	m_timer_ID  =0;
	m_timers_count = 0;
	m_bk_color = ::GetSysColor(COLOR_WINDOW);
	m_text_color = RGB(0,0,0);
	m_cur_string.Empty();
	m_reserve_string.Empty();
}

CInfoBar::~CInfoBar()
{
}

void  CInfoBar::SetInfoStyle(INFO_TYPE newType)
{
	m_type = newType;
	switch(m_type) {
	case INFO_TEXT:
		m_progress.ShowWindow(SW_HIDE);
		break;
	case INFO_PROGRESS:
		m_progress.ShowWindow(SW_SHOW);
		break;
	default:
		ASSERT(0);
		break;
	}
}


BEGIN_MESSAGE_MAP(CInfoBar, CDialogBar)
	ON_WM_PAINT()
	ON_WM_CREATE()
	ON_WM_SIZE()
	ON_WM_TIMER()
	ON_WM_ERASEBKGND()
END_MESSAGE_MAP()

void   CInfoBar::SetMessageString(const char* str)
{
	if (!str)
		return;

	if (m_type==INFO_TEXT)
	{
		m_bk_color = ::GetSysColor(COLOR_WINDOW);
		m_text_color = RGB(0,0,0);
		m_cur_string = CString(str);
		if (m_timer_ID)
		{
			m_timers_count = 0;
			KillTimer(m_timer_ID);
			m_timer_ID=0;
		}
		Invalidate();
	}
}

void   CInfoBar::SetWarningString(const char* str, const char* str2)
{
	if (!str)
		return;

	if (m_type==INFO_TEXT)
	{
		m_bk_color = RGB(0,255,0);
		m_text_color = RGB(0,0,0);
		if (str2)
			m_reserve_string = CString(str2);
		else
			m_reserve_string = m_cur_string;
		m_cur_string = CString(str);
		if (m_timer_ID)
		{
			m_timers_count = 0;
			KillTimer(m_timer_ID);
			m_timer_ID=0;
		}
		Invalidate();
	}
}

void   CInfoBar::SetErrorString(const char* str, const char* str2)
{
	if (!str)
		return;

	if (m_type==INFO_TEXT)
	{
		m_bk_color = RGB(255,0,0);
		m_text_color = RGB(0,0,0);
		if (str2)
			m_reserve_string = CString(str2);
		else
			if (m_timer_ID==0)
				m_reserve_string = m_cur_string;

		m_cur_string = CString(str);
		if (m_timer_ID)
		{
			m_timers_count = 0;
			KillTimer(m_timer_ID);
			m_timer_ID=0;
		}
		m_timer_ID = SetTimer(1, 1, 0);
		m_timers_count = 0;
		Invalidate();
	}
}

void  CInfoBar::Progress(int perc)
{
	if (m_type==INFO_PROGRESS)
		m_progress.SetPos(perc);
}

int CInfoBar::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (CDialogBar::OnCreate(lpCreateStruct) == -1)
		return -1;

	m_frame.CreateEx(WS_EX_CLIENTEDGE/*|WS_EX_STATICEDGE*/,NULL,NULL,
		WS_CHILD|WS_VISIBLE|BS_GROUPBOX, CRect(0,0,0,0), 
		this, 0 );
	m_frame.EnableWindow();

	m_progress.CreateEx(WS_EX_CLIENTEDGE,
		WS_CHILD|WS_VISIBLE, CRect(0,0,0,0), 
		this,1003);
	m_progress.ShowWindow(SW_HIDE);

	m_progress.SetRange32(0,100);
	m_progress.SetPos(30);



	return 0;
}


CSize CInfoBar::CalcDynamicLayout(int nLength, DWORD nMode)
{
	CSize ress = CControlBar::CalcDynamicLayout(nLength,nMode);

	CDC* pdc = GetDC();
	if (pdc)
	{
		CSize sss = pdc->GetTextExtent("Sdfasdf");
		ress.cy = sss.cy+2;
		ReleaseDC(pdc);
	}
	return ress;

}


void CInfoBar::OnPaint()
{
	CPaintDC dc(this); // device context for painting

	if (m_type==INFO_TEXT)
	{
		CRect clR;
		GetWindowRect(clR);
		ScreenToClient(clR);
	#define  RSZ  2
		clR.top+=RSZ;clR.bottom-=RSZ;
		clR.left+=RSZ;clR.right-=RSZ;	
		if (m_bk_color==::GetSysColor(COLOR_WINDOW))
			themeData.DrawThemedRect(&dc,&clR,FALSE);
		else
			dc.FillSolidRect(clR,m_bk_color);

		CFont* font = CFont::FromHandle((HFONT)::GetStockObject(DEFAULT_GUI_FONT));
		SetFont(font);
		dc.SetBkMode(TRANSPARENT);
		dc.SetTextColor(m_text_color);
		dc.SelectObject(font);
		dc.DrawText("    "+m_cur_string,clR,DT_LEFT);
	}
}
void CInfoBar::OnSize(UINT nType, int cx, int cy)
{
	CDialogBar::OnSize(nType, cx, cy);

	if (::IsWindow(m_frame.m_hWnd))
	{
		m_frame.MoveWindow(0,0,cx,cy);
		m_progress.MoveWindow(0,0,cx,cy);
	}
}

void CInfoBar::OnTimer(UINT nIDEvent)
{
	m_timers_count++;
	if (m_timers_count==125)
	{
		m_timers_count = 0;
		KillTimer(m_timer_ID);
		m_timer_ID=0;
		m_bk_color = ::GetSysColor(COLOR_WINDOW);
		m_cur_string = m_reserve_string;
		Invalidate();
	}
	else
	{
		m_bk_color = RGB(GetRValue(m_bk_color),
							GetGValue(m_bk_color)+2,
							GetBValue(m_bk_color)+2);
		Invalidate();
	}
	CDialogBar::OnTimer(nIDEvent);
}

BOOL CInfoBar::OnEraseBkgnd(CDC* pDC)
{
	// TODO: Add your message handler code here and/or call default

	return TRUE;//CDialogBar::OnEraseBkgnd(pDC);
}
