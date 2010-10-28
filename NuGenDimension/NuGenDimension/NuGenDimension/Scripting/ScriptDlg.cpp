// ScriptDlg.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "..//NuGenDimensionView.h"
#include "ScriptDlg.h"
#include ".\scriptdlg.h"

ILoger*   global_loger = NULL;

IMPLEMENT_DYNAMIC(CMyRichEdit, CRichEditCtrl)
CMyRichEdit::CMyRichEdit()
{
}

CMyRichEdit::~CMyRichEdit()
{
}


BEGIN_MESSAGE_MAP(CMyRichEdit, CRichEditCtrl)
	ON_CONTROL_REFLECT(EN_SETFOCUS, OnEnSetfocus)
	ON_CONTROL_REFLECT(EN_KILLFOCUS, OnEnKillfocus)
END_MESSAGE_MAP()


void CMyRichEdit::OnEnSetfocus()
{
	translate_messages_through_app = true;
}

void CMyRichEdit::OnEnKillfocus()
{
	translate_messages_through_app = false;
}






// File interaction, 

DWORD CALLBACK readFunction(DWORD dwCookie, LPBYTE lpBuf, LONG nCount,  LONG* nRead)
{
	CFile* fp = (CFile *)dwCookie;
	*nRead = fp->Read(lpBuf,nCount);
	return 0;
}

DWORD CALLBACK writeFunction(DWORD dwCookie, LPBYTE pbBuff, LONG cb, LONG *pcb)
{
	CFile* pFile = (CFile*) dwCookie;
	pFile->Write(pbBuff, cb);
	*pcb = cb;
	return 0;
}




// CScriptDlg dialog

IMPLEMENT_DYNAMIC(CScriptDlg, CDialog)
CScriptDlg::CScriptDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CScriptDlg::IDD, pParent)
{
	m_file_name.Empty();
	m_was_modified = false;
	global_loger = this;
	m_error_brush = NULL;
	m_active_error_regime = false;
}

CScriptDlg::~CScriptDlg()
{
	global_loger = NULL;
	if (m_error_brush)
		DeleteObject(m_error_brush);
}

void CScriptDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_SCRIPT_RICHEDIT, m_rich_edit);
	DDX_Control(pDX, IDC_SCRIPT_ERRORS_LOG, m_errors_log);
	DDX_Control(pDX, IDC_SCRIPT_TRACE_LOG, m_trace_log);
}


BEGIN_MESSAGE_MAP(CScriptDlg, CDialog)
	ON_WM_SIZE()
	ON_EN_CHANGE(IDC_SCRIPT_RICHEDIT, OnEnChangeScriptRichedit)
	ON_WM_CTLCOLOR()
END_MESSAGE_MAP()


void  CScriptDlg::SetFileName(CString& newFN)
{
	m_file_name = newFN;
	CString   newTitle;
	GetWindowText(newTitle);
	SetWindowText(newTitle+" "+newFN);
}

// CScriptDlg message handlers

void CScriptDlg::OnSize(UINT nType, int cx, int cy)
{
	CDialog::OnSize(nType, cx, cy);

	if(m_rich_edit.GetSafeHwnd())
	{
		RECT r;

		//set coordinates for rich edit box
		r.left = 0; 
		r.top = 0;
		r.right = cx;
		r.bottom = 2*cy/3-1;
		m_rich_edit.MoveWindow(&r);

		r.top = 2*cy/3-1;
		r.bottom = cy;
		r.right = cx/2-1;
		m_errors_log.MoveWindow(&r);

		r.bottom = cy;
		r.left = cx/2+1;
		r.right = cx;
		m_trace_log.MoveWindow(&r);
	}
}

BOOL CScriptDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	long mask = m_rich_edit.GetEventMask();
	m_rich_edit.SetEventMask(mask |= ENM_CHANGE );

	//reconfigure CSyntaxColorizer's default keyword groupings
	LPTSTR sKeywords = "for,else,Point,Line,Circle,Box,Sphere,"
		"Cone,Cylinder,Torus,SphericBand,Ellipsoid,false,"
		"true,"
		"if,"
		"while";
	/*LPTSTR sDirectives = "#define,#elif,#else,#endif,#error,#ifdef,"
	"#ifndef,#import,#include,#line,#pragma,#undef";
	LPTSTR sPragmas = "comment,optimize,auto_inline,once,warning,"
	"component,pack,function,intrinsic,setlocale,hdrstop,message";*/

	m_syntax_colorizer.ClearKeywordList();
	m_syntax_colorizer.AddKeyword(sKeywords,RGB(0,0,255),GRP_KEYWORD);
	//m_syntax_colorizer.AddKeyword(sDirectives,RGB(0,0,255),GRP_DIRECTIVE);
	//m_syntax_colorizer.AddKeyword(sPragmas,RGB(0,0,255),GRP_PRAGMA);
	m_error_brush = (HBRUSH)CreateSolidBrush(RGB(255,157,157));

	return TRUE;  // return TRUE unless you set the focus to a control
	// EXCEPTION: OCX Property Pages should return FALSE
}


void CScriptDlg::readFile(CString sFileName)
{
	m_rich_edit.SetRedraw(FALSE);

	CFile file(sFileName,CFile::modeRead);
	m_es.dwCookie = (DWORD)&file;
	m_es.pfnCallback = readFunction;
	m_rich_edit.StreamIn(SF_TEXT,m_es);
	file.Close();

	parse();

}

void CScriptDlg::parse()
{
	//turn off response to onchange events
	long mask = m_rich_edit.GetEventMask();
	m_rich_edit.SetEventMask(mask ^= ENM_CHANGE );

	//set redraw to false to reduce flicker, and to speed things up
	m_rich_edit.SetRedraw(FALSE);

	//call the colorizer
	m_syntax_colorizer.Colorize(0,-1,&m_rich_edit);

	//do some cleanup

	//Bug fix by HOMO_PROGRAMMATIS <homo_programmatis@rt.mipt.ru>
	/**/  //m_cSyntax.SetSel(0,0);
	//end of changes by HOMO_PROGRAMMATIS

	m_rich_edit.SetRedraw(TRUE);
	m_rich_edit.RedrawWindow();

	m_rich_edit.SetEventMask(mask |= ENM_CHANGE );
}

void CScriptDlg::parse2()
{
	//get the current line of text from the control
	int len = m_rich_edit.LineLength();
	int start = m_rich_edit.LineIndex();

	//call the colorizer
	m_syntax_colorizer.Colorize(start,start + len,&m_rich_edit);
}


void CScriptDlg::OnEnChangeScriptRichedit()
{
	CHARRANGE cr;
	m_rich_edit.GetSel(cr);
	parse2();
	m_rich_edit.SetSel(cr);
	m_was_modified = true;
}

void CScriptDlg::OnNew()
{
}

void CScriptDlg::OnOpen()
{
	if (m_was_modified)
	{
		CString lab;
		lab.LoadString(IDS_DO_YOU_WANT_SAVE_SCRIPT);
		lab+=m_file_name+"?";
		switch(AfxMessageBox(lab,MB_ICONQUESTION|MB_YESNOCANCEL)) 
		{
		case IDYES:
			OnSave();
			break;
		case IDCANCEL:
			return;
		case IDNO:
		default:
			break;
		}
	}

	CString			Path;

	CFileDialog dlg(
	TRUE,
	NULL,								// Open File Dialog
	_T(""),							// Default extension
	OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT,	// No default filename
	_T("Script files (*.nds)|*.nds||"));// Filter string

	if (dlg.DoModal() != IDOK)
	return;
	Path = dlg.GetPathName();

	readFile(Path);

	m_rich_edit.SetFocus();

	SetFileName(Path);

	m_was_modified = false;
}

void CScriptDlg::OnSave()
{
	if (!m_file_name.IsEmpty())
	{
	CFile cFile(m_file_name, CFile::modeCreate|CFile::modeWrite);
	m_es.dwCookie = (DWORD) &cFile;
	m_es.pfnCallback = writeFunction; 
	m_rich_edit.StreamOut(SF_TEXT, m_es);
	cFile.Close();
	}
	else
	{
	OnSaveAs();
	}
	m_was_modified = false;

}

void CScriptDlg::OnSaveAs()
	{
		CString			Path;

		CFileDialog dlg(
		FALSE,
		NULL,								// Open File Dialog
		_T("Script.sgs"),							// Default extension
		OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT,	// No default filename
		_T("Script files (*.nds)|*.nds||"));// Filter string

		if (dlg.DoModal() != IDOK)
		return;
		Path = dlg.GetPathName();
		if (Path.Right(4)!=".sgs")
		Path+=".sgs";

		CFile cFile(Path, CFile::modeCreate|CFile::modeWrite);
		m_es.dwCookie = (DWORD) &cFile;
		m_es.pfnCallback = writeFunction; 
		m_rich_edit.StreamOut(SF_TEXT, m_es);
		cFile.Close();
		SetFileName(Path);
		m_was_modified = false;
	}

void CScriptDlg::OnRun()
{
	m_active_error_regime = false;
	m_errors_log.SetWindowText("");
	m_trace_log.SetWindowText(""); 
	OnSave();
	BeginWaitCursor();
	sgGetScene()->StartUndoGroup();
	LuaRunScript(m_file_name);
	sgGetScene()->EndUndoGroup();
	EndWaitCursor();
	if (global_opengl_view)
		global_opengl_view->Invalidate();
}

void CScriptDlg::SetMessage(const char *message)
{
	CString editing_message(message);
	int er_pos = editing_message.Find("error");
	editing_message.Replace(_T("\n"), _T("\r\n"));
	if (er_pos==0)
	{
		m_active_error_regime = true;
		CString fl_name = PathFindFileName(m_file_name);
		editing_message.Replace(m_file_name, fl_name);
		m_errors_log.SetWindowText(editing_message);
		m_errors_log.Invalidate();
	}
	else
	{
		m_active_error_regime = false;
		m_errors_log.SetWindowText(" ");
		CString old_text;
		m_trace_log.GetWindowText(old_text);
		old_text+=editing_message;
		m_trace_log.SetWindowText(old_text);
	}
}

HBRUSH CScriptDlg::OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor)
{
	HBRUSH hbr = __super::OnCtlColor(pDC, pWnd, nCtlColor);

	if (pWnd==&m_errors_log)
	{
		pDC->SetTextColor(0);
		//pDC->SetBkColor(RGB(255,0,0));
		pDC->SetBkMode(TRANSPARENT);
		if (m_active_error_regime)
			return m_error_brush;
		else
			hbr = (HBRUSH)GetStockObject(HOLLOW_BRUSH);
	}

	return hbr;
}
