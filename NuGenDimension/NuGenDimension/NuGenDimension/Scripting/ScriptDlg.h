#pragma once


#include "..//Controls//SyntaxColorizer.h"
#include "afxwin.h"
#include "..//resource.h"

#define GRP_KEYWORD   0
#define GRP_PRAGMA    1
#define GRP_DIRECTIVE 2

class  ILoger
{
public:
	virtual  void   SetMessage(const char *message)   =0;
};
// CScriptDlg dialog
// CMyRichEdit

class CMyRichEdit : public CRichEditCtrl
{
	DECLARE_DYNAMIC(CMyRichEdit)

public:
	CMyRichEdit();
	virtual ~CMyRichEdit();

protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnEnSetfocus();
	afx_msg void OnEnKillfocus();
};


class CScriptDlg : public CDialog, public ILoger
{
	DECLARE_DYNAMIC(CScriptDlg)

public:
	CScriptDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CScriptDlg();

// Dialog Data
	enum { IDD = IDD_SCRIPT_DLG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnSize(UINT nType, int cx, int cy);
	virtual BOOL OnInitDialog();
	afx_msg void OnEnChangeScriptRichedit();
public:
	virtual  void   SetMessage(const char *message);
private:
	CMyRichEdit      m_rich_edit;
	CSyntaxColorizer m_syntax_colorizer;
	EDITSTREAM       m_es;

	void readFile(CString sFileName);
	void parse();
	void parse2();

	CString         m_file_name;
	void            SetFileName(CString& newFN);
	bool            m_was_modified;
public:
	void OnNew();
	void OnOpen();
	void OnSave();
	void OnSaveAs();
	void OnRun();
private:
	CEdit   m_errors_log;
	CEdit   m_trace_log;
	bool    m_active_error_regime;
	HBRUSH  m_error_brush;
public:
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
};

extern ILoger*   global_loger;
