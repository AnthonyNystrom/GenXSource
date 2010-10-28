#pragma once
#include "afxwin.h"


// CGetOneDlg dialog

class CGetOneDlg : public CDialog, public IComboPanel
{
	DECLARE_DYNAMIC(CGetOneDlg)

public:
	CGetOneDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CGetOneDlg();

	virtual  DLG_TYPE  GetType();
	virtual  CWnd*     GetWindow();   

	virtual  void      EnableControls(bool);

	virtual void            AddString(const char*);
	virtual void            RemoveAllStrings();
	virtual void            SetCurString(unsigned int);
	virtual unsigned int    GetCurString();


// Dialog Data
	enum { IDD = IDD_GET_ONE_DGL };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
private:
	bool       m_enable_history;
	bool       m_was_diasabled;
	CComboBox m_combo;
protected:
	virtual void OnOK();
	virtual void OnCancel();
public:
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnCbnSelchangeGetOneCombo();
};
