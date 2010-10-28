#pragma once
#include "afxwin.h"

#include "..//..//Controls//FloatEdit.h"
#include "..//..//Controls//ToolTipBitmapButton.h"


// CGetPointDlg dialog

class CGetPointDlg : public CDialog, public IGetPointPanel
{
	DECLARE_DYNAMIC(CGetPointDlg)

public:
	CGetPointDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CGetPointDlg();

// Dialog Data
	enum { IDD = IDD_GET_POINT_DLG };

	virtual  DLG_TYPE  GetType();
	virtual  CWnd*     GetWindow();  

	virtual  void      EnableControls(bool);

	virtual bool  SetPoint(double x, double y, double z);
	virtual bool  GetPoint(double& x, double& y, double& z);
	virtual bool  IsXFixed();
	virtual bool  IsYFixed();
	virtual bool  IsZFixed();
	virtual bool  XFix(bool fix);
	virtual bool  YFix(bool fix);
	virtual bool  ZFix(bool fix);


protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
	virtual void OnOK();
	virtual void OnCancel();
private:
	bool*      m_enable_history;
	bool       m_was_diasabled;
	int        m_tabs_counter;
	CFloatEdit m_x_edit;
	CFloatEdit m_y_edit;
	CFloatEdit m_z_edit;
	CToolTipBitmapButton m_x_lock_btn;
	CToolTipBitmapButton m_y_lock_btn;
	CToolTipBitmapButton m_z_lock_btn;
public:
	virtual BOOL OnInitDialog();
	afx_msg void OnBnClickedPntXLockBtn();
	afx_msg void OnBnClickedPntYLockBtn();
	afx_msg void OnBnClickedPntZLockBtn();
	afx_msg void OnChar(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
};
