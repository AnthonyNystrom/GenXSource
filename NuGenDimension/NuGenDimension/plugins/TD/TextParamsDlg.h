#pragma once

#include "resource.h"
#include "afxwin.h"
#include "afxcmn.h"

#include "FlEd.h"
// CTextParamsDlg dialog

class CTextParamsDlg : public CDialog, public IBaseInterfaceOfGetDialogs
{
	DECLARE_DYNAMIC(CTextParamsDlg)

public:
	CTextParamsDlg(SG_TEXT_STYLE*, IApplicationInterface* appI, CWnd* pParent = NULL);   // standard constructor
	virtual ~CTextParamsDlg();

	virtual  DLG_TYPE  GetType() {return IBaseInterfaceOfGetDialogs::USER_DIALOG;};
	virtual  CWnd*     GetWindow() {return this;};   

	virtual  void      EnableControls(bool) {};


// Dialog Data
	enum { IDD = IDD_TEXT_PARAMS_DLG };
private:
	SG_TEXT_STYLE*    m_cur_text_style;
	IApplicationInterface* m_app;
	
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	virtual BOOL OnInitDialog();

	SG_TEXT_STYLE*   GetTextStyle() {return m_cur_text_style;};

protected:
	virtual void OnOK();
	virtual void OnCancel();
public:
private:
	CFlEd m_sym_height;
	bool    m_inFocus;

public:
	bool    IsInFocus() {return m_inFocus;};

	afx_msg void OnChar(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg void OnEnChangeSymAngleEdit();
private:
	CFlEd           m_angle;
public:
	afx_msg void OnEnChangeSymHeightEdit();
private:
	CFlEd m_lines_space;
	CFlEd m_symb_space;
public:
	afx_msg void OnEnChangeSymbSpaceEdit();
	afx_msg void OnEnChangeLinesSpaceEdit();
private:
	CFlEd m_s_prop;
public:
	afx_msg void OnEnChangeSymbPropoptEdit();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg void OnEnSetfocusTextEdit();
	afx_msg void OnEnKillfocusTextEdit();

};
