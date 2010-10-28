#pragma once

#include "..//..//Controls//FloatEdit.h"
#include "..//..//Controls//ToolTipBitmapButton.h"


// CGetVectorDlg dialog

class CGetVectorDlg : public CDialog, public IGetVectorPanel
{
	DECLARE_DYNAMIC(CGetVectorDlg)

public:
	CGetVectorDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CGetVectorDlg();

// Dialog Data
	enum { IDD = IDD_GET_VECTOR_DLG };

	virtual  DLG_TYPE  GetType();
	virtual  CWnd*     GetWindow();  

	virtual  void      EnableControls(bool);

	virtual VECTOR_TYPE   GetVector(double& x, double& y, double& z);
	virtual void          SetVector(VECTOR_TYPE n_t, 
		double x, double y, double z);
	virtual bool  IsXFixed();
	virtual bool  IsYFixed();
	virtual bool  IsZFixed();
	virtual bool  XFix(bool fix);
	virtual bool  YFix(bool fix);
	virtual bool  ZFix(bool fix);


protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
private:
	bool*      m_enable_history;
	bool       m_was_diasabled;
	CFloatEdit m_x_edit;
	CFloatEdit m_y_edit;
	CFloatEdit m_z_edit;

	CToolTipBitmapButton m_x_lock_btn;
	CToolTipBitmapButton m_y_lock_btn;
	CToolTipBitmapButton m_z_lock_btn;

	VECTOR_TYPE   m_vector_type;
protected:
	virtual void OnOK();
	virtual void OnCancel();
public:
	virtual BOOL OnInitDialog();
	afx_msg void OnChar(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg void OnBnClickedDirXLockBtn();
	afx_msg void OnBnClickedDirYLockBtn();
	afx_msg void OnBnClickedDirZLockBtn();
	afx_msg void OnBnClickedVectorXRadio();
	afx_msg void OnBnClickedVectorYRadio();
	afx_msg void OnBnClickedVectorZRadio();
	afx_msg void OnBnClickedVectorUserRadio();
private:
	void    EnableUserNormalControls(BOOL enbl);
	CButton m_x_radio;
	CButton m_y_radio;
	CButton m_z_radio;
	CButton m_user_radio;
	void    UpdateRadios();
public:
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
};
