#pragma once
#include "afxwin.h"

#include "resource.h"
#include "ArrowTypeCombo.h"

#include "FlEd.h"

class CLinearDimDlg : public CDialog, public IBaseInterfaceOfGetDialogs
{
	DECLARE_DYNAMIC(CLinearDimDlg)
	ICommandPanel* m_com_pan;
public:
	CLinearDimDlg(ICommandPanel* comPan, SG_DIMENSION_STYLE* ds, 
		IApplicationInterface* appI,bool withInvert,CWnd* pParent = NULL);   // standard constructor
	virtual ~CLinearDimDlg();

	virtual  DLG_TYPE  GetType() {return IBaseInterfaceOfGetDialogs::USER_DIALOG;};
	virtual  CWnd*     GetWindow() {return this;};   

	virtual  void      EnableControls(bool) {};


// Dialog Data
	enum { IDD = IDD_LINEAR_DIM_DLG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
private:
	IApplicationInterface*   m_app;
	SG_DIMENSION_STYLE*  m_dim_style_pointer;
	bool                     m_with_invert;

	CArrowTypeCombo m_left_end_type_combo;
	CArrowTypeCombo m_right_end_type_combo;
public:
	virtual BOOL OnInitDialog();
protected:
	virtual void OnOK() {};
	virtual void OnCancel() {};
private:
	BOOL m_razm_lin_check;
	BOOL m_left_dim_check;
	BOOL m_right_dim_check;
	BOOL m_left_end_out_check;
	BOOL m_right_end_out_check;
public:
	afx_msg void OnBnClickedLdRazmLinCheck();
	afx_msg void OnBnClickedLdLeftDimCheck();
	afx_msg void OnBnClickedLdRightDimCheck();
	afx_msg void OnBnClickedLdLeftEndOutCheck();
	afx_msg void OnBnClickedLdRightEndOutCheck();
	afx_msg void OnCbnSelchangeLdLeftEndTypeCombo();
	afx_msg void OnCbnSelchangeLdRightEndTypeCombo();
private:
public:
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg void OnBnClickedLinDimAutoArr();
private:
	BOOL m_auto_arr;
	bool    m_inFocus;

public:
	bool    IsInFocus() {return m_inFocus;};

	afx_msg void OnSize(UINT nType, int cx, int cy);
private:
	CFlEd m_vinos_size;
	CFlEd m_arrow_size;
public:
	afx_msg void OnEnChangeLdVinosEdit();
	afx_msg void OnEnSetfocusLdVinosEdit();
	afx_msg void OnEnKillfocusLdVinosEdit();
	afx_msg void OnEnChangeLdEndSizeEdit();
	afx_msg void OnEnSetfocusLdEndSizeEdit();
	afx_msg void OnEnKillfocusLdEndSizeEdit();
	afx_msg void OnBnClickedLinDimInvert();
};
