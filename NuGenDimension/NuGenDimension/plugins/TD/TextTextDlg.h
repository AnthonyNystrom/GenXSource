#pragma once

#include "resource.h"
// CTextTextDlg dialog

class CTextTextDlg : public CDialog, public IBaseInterfaceOfGetDialogs
{
	DECLARE_DYNAMIC(CTextTextDlg)

public:
	CTextTextDlg(IApplicationInterface* appI, CWnd* pParent = NULL);   // standard constructor
	virtual ~CTextTextDlg();

	virtual  DLG_TYPE  GetType() {return IBaseInterfaceOfGetDialogs::USER_DIALOG;};
	virtual  CWnd*     GetWindow() {return this;};   

	virtual  void      EnableControls(bool) {};

	bool    IsInFocus() {return m_inFocus;};
	bool    IsEditorInFocus() {return (GetFocus()==&m_text_edit);};

	void    GetText(CString&);
	void    SetText(const char*);

	enum { IDD = IDD_TEXT_TEXT_DLG };

protected:
	virtual void OnOK() 
	{
		m_text_edit.SendMessage(WM_CHAR,'\r',0);
		m_text_edit.SendMessage(WM_CHAR,'\n',0);
		m_text_edit.SetFocus();
		return;
	};
	virtual void OnCancel() {};
private:
	CEdit m_text_edit;
	bool  m_inFocus;
	IApplicationInterface* m_app;

// Dialog Data
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	virtual BOOL OnInitDialog();
	afx_msg void OnChar(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg void OnEnSetfocusTextEdit();
	afx_msg void OnEnKillfocusTextEdit();

	afx_msg void OnEnChangeTextTextEdit();
};
