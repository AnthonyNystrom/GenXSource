#pragma once

#include "..//..//Controls//FloatEdit.h"
// CGetNumberDlg dialog

class CGetNumberDlg : public CDialog, public IGetNumberPanel
{
	DECLARE_DYNAMIC(CGetNumberDlg)

public:
	CGetNumberDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CGetNumberDlg();

	virtual  DLG_TYPE  GetType();
	virtual  CWnd*     GetWindow();   

	virtual  void      EnableControls(bool);


	virtual double GetNumber();
	virtual void   SetNumber(double nmbr);

// Dialog Data
	enum { IDD = IDD_GET_NUMBER_DLG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
private:
	CFloatEdit m_number_edit;
	bool       m_was_diasabled;
	bool*      m_enable_history;
protected:
	virtual void OnOK();
	virtual void OnCancel();
public:
	virtual BOOL OnInitDialog();
	afx_msg void OnChar(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg void OnSize(UINT nType, int cx, int cy);
};
