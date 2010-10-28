// FloatEdit.cpp : implementation file
//

#include "stdafx.h"
#include "FlEd.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CFlEd

CFlEd::CFlEd()
{
}

CFlEd::~CFlEd()
{
}

void  CFlEd::SetValue(float val)
{
	CString tmpS;
	tmpS.Format("%f",val);
	SetWindowText(tmpS);
	//GetParent()->SetFocus();
	//SendMessage(GET_DIALOGS_MESSAGE_POST_SET_FOCUS);
}

float CFlEd::GetValue()
{
	CString tmpS;
	GetWindowText(tmpS);
	char   *str,*stopstring;
	str = tmpS.GetBuffer();
	float  x;
	x = (float)strtod( str, &stopstring );
	if(str==stopstring)
	{
		SetWindowText("0.0000");
		return 0.0f;
	}
	return x;
}

BEGIN_MESSAGE_MAP(CFlEd, CEdit)
	//{{AFX_MSG_MAP(CFlEd)
	ON_WM_CHAR()
	//}}AFX_MSG_MAP
	ON_WM_SETFOCUS()
	ON_MESSAGE(GET_DIALOGS_MESSAGE_POST_SET_FOCUS, OnPostSetFocus)
	ON_MESSAGE(GET_DIALOGS_MESSAGE_PRE_SET_FOCUS, OnPreSetFocus)
	ON_WM_KEYDOWN()
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CFlEd message handlers

void CFlEd::OnChar(UINT nChar, UINT nRepCnt, UINT nFlags) 
{
	/*switch(nChar)
    { 
		case _T('.'):
		case _T('0'):
		case _T('1'):
		case _T('2'):
		case _T('3'):
		case _T('4'):
		case _T('5'):
		case _T('6'):
		case _T('7'):
		case _T('8'):
		case _T('9'):
		case _T('\b'): //backspace 
			if(m_fix_button)
				m_fix_button->SetPressed(true);
			break;
		default:
			//GetParent()->SendMessage(WM_CHAR,nChar,0);
			return;
       } 
	CEdit::OnChar(nChar,nRepCnt, nFlags);*/

	if(!(nChar >= '0' && nChar <= '9' || nChar == '.' || nChar == '\b'))
		return;
	CString str;
	GetWindowText(str);
	if(nChar == '.' && (str.Find('.') >= 0 || str.IsEmpty()))
		return;
	int nStartChar, nEndChar;
	GetSel(nStartChar, nEndChar);
	//if(/*nChar == '-' && **/(nStartChar != 0)/* || nEndChar != 0)*/)
	//	return;
	if(nChar == '\b' && nStartChar <= 0)
		return;
	CEdit::OnChar(nChar, nRepCnt, nFlags);
}

void CFlEd::OnSetFocus(CWnd* pOldWnd)
{
	CEdit::OnSetFocus(pOldWnd);
	SendMessage(GET_DIALOGS_MESSAGE_POST_SET_FOCUS);
}

LRESULT CFlEd::OnPostSetFocus(WPARAM, LPARAM)
{
	SetSel(0,-1);
	return 0L;
}

LRESULT CFlEd::OnPreSetFocus(WPARAM nChar, LPARAM)
{
	SetFocus();
	switch(nChar)
	{ 
		//case _T('-'):
		case _T('0'):
		case _T('1'):
		case _T('2'):
		case _T('3'):
		case _T('4'):
		case _T('5'):
		case _T('6'):
		case _T('7'):
		case _T('8'):
		case _T('9'):
			SetWindowText((char*)&nChar);
			SetSel(1,2);
			break;
	} 
	return 0L;
}

void CFlEd::OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags)
{
	/*switch(nChar)
	{ 
	//case _T('.'):
	case _T('0'):
	case _T('1'):
	case _T('2'):
	case _T('3'):
	case _T('4'):
	case _T('5'):
	case _T('6'):
	case _T('7'):
	case _T('8'):
	case _T('9'):
	case _T('\b'): //backspace 
	case VK_DELETE:
		if(m_fix_button)
			m_fix_button->SetPressed(true);
		break;
	case VK_LEFT:
	case VK_RIGHT:
		break;
	default:
		return;
	} 
	*/
	CEdit::OnKeyDown(nChar, nRepCnt, nFlags);
}
