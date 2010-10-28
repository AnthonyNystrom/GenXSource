// HyperlinkStatic.cpp : implementation file
//

#include "stdafx.h"
#include "EGHyperlink.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CEGHyperlink

CEGHyperlink::CEGHyperlink()
{
	_strCaption = _strHyperlink = _T("");	 
	_bMouseInControl = _bCreateFont = _bGetCaptionSize = false;

#if(WINVER >= 0x0500)
	_hHandCursor = ::LoadCursor(0, MAKEINTRESOURCE(IDC_HAND));
#else
	_hHandCursor = ::LoadCursor(0, MAKEINTRESOURCE(IDC_ARROW));
#endif
	_hArrowCursor = ::LoadCursor(0, MAKEINTRESOURCE(IDC_ARROW));
}

CEGHyperlink::~CEGHyperlink()
{
}

BEGIN_MESSAGE_MAP(CEGHyperlink, CStatic)
	//{{AFX_MSG_MAP(CEGHyperlink)
	ON_WM_LBUTTONDOWN()
	ON_WM_PAINT()
	ON_WM_DESTROY()
	ON_WM_MOUSEMOVE()
	//}}AFX_MSG_MAP
	ON_MESSAGE(WM_MOUSELEAVE, OnMouseLeave)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CEGHyperlink message handlers

void CEGHyperlink::SetHyperlink(CString strHyperlink)
{
	_strHyperlink = strHyperlink;
}

void CEGHyperlink::SetCaption(CString strCaption)
{
	_strCaption = strCaption;
	_bGetCaptionSize = false;	
}

void CEGHyperlink::OnLButtonDown(UINT nFlags, CPoint point) 
{
	if ( _bGetCaptionSize == false )
		GetCaptionSize();
	if (InCaptionRange(point))
		ShellExecute(0, "open", _strHyperlink, 0, 0, SW_SHOWNORMAL);
	CStatic::OnLButtonDown(nFlags, point);
}

void CEGHyperlink::OnPaint() 
{	
	if ( _bCreateFont == false )
		CreateFont();
	CPaintDC dc(this);
	CFont *pOldFont = (CFont*)dc.SelectObject(&_fontCaption);
	dc.SetBkMode(TRANSPARENT);
	dc.SetTextColor(RGB(0,0,255));
	dc.TextOut(0, 0, _strCaption);
	dc.SelectObject(pOldFont);
}

void CEGHyperlink::OnDestroy() 
{
	CStatic::OnDestroy();
	_fontCaption.DeleteObject();
}

void CEGHyperlink::PreSubclassWindow() 
{
	ModifyStyle(0, SS_NOTIFY, TRUE);
	GetWindowText(_strCaption);
	_bGetCaptionSize = false;
	CStatic::PreSubclassWindow();
}

LRESULT CEGHyperlink::OnMouseLeave( WPARAM /* wParam */, LPARAM /* lParam */)
{
	_bMouseInControl = false;
	::SetCursor(_hArrowCursor);	
	return 0;
}

void CEGHyperlink::OnMouseMove(UINT nFlags, CPoint point) 
{
	if ( _bMouseInControl == false ) {
		//Track the mouse leave event
		TRACKMOUSEEVENT tme;
		tme.cbSize = sizeof(tme);
        tme.hwndTrack = GetSafeHwnd();
        tme.dwFlags = TME_LEAVE;        
		_TrackMouseEvent(&tme);
		_bMouseInControl = true;
	}
	else {
		if ( _bGetCaptionSize == false )
			GetCaptionSize();
		::SetCursor((InCaptionRange(point))?_hHandCursor:_hArrowCursor);		
	}
	CStatic::OnMouseMove(nFlags, point);
}

void CEGHyperlink::CreateFont()
{
	CFont* pFontParent = GetParent()->GetFont();	
	if ( pFontParent ) {
		LOGFONT lf;
		pFontParent->GetObject(sizeof(lf), &lf);
		lf.lfUnderline = TRUE;		
		_fontCaption.CreateFontIndirect(&lf);
		_bCreateFont = true;
	}
}

void CEGHyperlink::GetCaptionSize()
{
	if (( _bGetCaptionSize == false ) && ( _bCreateFont )) {
		CClientDC dc(this);
		CFont *pOldFont = dc.SelectObject(&_fontCaption);
		_sizeCaption = dc.GetTextExtent(_strCaption);
		dc.SelectObject(pOldFont);
		_bGetCaptionSize = true;
	}
}

bool CEGHyperlink::InCaptionRange(CPoint &point)
{
	if ( _bGetCaptionSize == false )
		return false;
	return (( point.x >= 0 )&&( point.x < _sizeCaption.cx ) &&
			( point.y >= 0 )&&( point.y < _sizeCaption.cy ));
}
