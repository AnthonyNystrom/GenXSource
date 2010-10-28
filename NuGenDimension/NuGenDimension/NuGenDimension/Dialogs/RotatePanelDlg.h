#pragma once

#include "..//resource.h"
#include "afxwin.h"

#include "..//Controls//FloatEdit.h"

class RotateCommand;

// CRotatePanelDlg dialog

class CRotatePanelDlg : public CDialog, public IBaseInterfaceOfGetDialogs
{
	DECLARE_DYNAMIC(CRotatePanelDlg)

public:
	CRotatePanelDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CRotatePanelDlg();

	virtual  DLG_TYPE  GetType() {return IBaseInterfaceOfGetDialogs::USER_DIALOG;};
	virtual  CWnd*     GetWindow() {return this;};   

	virtual  void      EnableControls(bool);


// Dialog Data
	enum { IDD = IDD_ROTATE_PANEL };
private:
	bool*      m_enable_history;
	bool       m_was_diasabled;
	RotateCommand*   m_commander;
public:
	void SetCommander( RotateCommand*  nC) {ASSERT(nC);m_commander=nC;};
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
	virtual void OnOK();
	virtual void OnCancel();
public:
	void  SetAngles(const SG_VECTOR&);
	void  GetAngles(SG_VECTOR&);
	bool  IsDouble();
	bool  IsSelectNew();
	int   GetCopiesCount();
	int   GetRotateType() {return m_rotate_around;};

private:
	int m_rotate_type;
	int m_rotate_around;
public:
	afx_msg void OnBnClickedRotateGlobalZeroRadio();
	afx_msg void OnBnClickedRotateObjCentersRadio();
	afx_msg void OnBnClickedRotWithoutDubleRadio();
	afx_msg void OnBnClickedRadio2();
	virtual BOOL OnInitDialog();
private:
	BOOL m_sel_new;
	int        m_tabs_counter;
	
	CSpinButtonCtrl m_cnt_spin;
	int m_copies_cnt;
public:
	afx_msg void OnDeltaposRotCopiesCntSpin(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnBnClickedRotSelNewCheck();

	afx_msg void OnSize(UINT nType, int cx, int cy);
private:
	CFloatEdit m_x_rot_ang;
	CFloatEdit m_y_rot_ang;
	CFloatEdit m_z_rot_ang;
public:
	afx_msg void OnChar(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
};
