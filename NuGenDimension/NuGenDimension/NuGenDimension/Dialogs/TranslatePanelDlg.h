#pragma once

#include "..//resource.h"
#include "afxwin.h"
#include "afxcmn.h"

#include "..//Controls//FloatEdit.h"

class TranslateCommand;

class CTranslatePanelDlg : public CDialog, public IBaseInterfaceOfGetDialogs
{
	DECLARE_DYNAMIC(CTranslatePanelDlg)

public:
	CTranslatePanelDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CTranslatePanelDlg();

	virtual  DLG_TYPE  GetType() {return IBaseInterfaceOfGetDialogs::USER_DIALOG;};
	virtual  CWnd*     GetWindow() {return this;};   

	virtual  void      EnableControls(bool);


// Dialog Data
	enum { IDD = IDD_TRANSLATE_PANEL };
private:
	bool*      m_enable_history;
	bool       m_was_diasabled;
	TranslateCommand*   m_commander;
	int        m_tabs_counter;
public:
	void SetCommander( TranslateCommand*  nC) {ASSERT(nC);m_commander=nC;};
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
	virtual void OnOK();
	virtual void OnCancel();
public:
	void  SetVector(const SG_VECTOR&);
	void  GetVector(SG_VECTOR&);
	bool  IsDouble();
	bool  IsSelectNew();
	int   GetCopiesCount();

private:
	int m_trans_type;
public:
	afx_msg void OnBnClickedTransWithoutDubleRadio();
	afx_msg void OnBnClickedRadio2();
	virtual BOOL OnInitDialog();
private:
	BOOL m_sel_new;
	CSpinButtonCtrl m_cnt_spin;
	int m_copies_cnt;
public:
	afx_msg void OnDeltaposTransCopiesCntSpin(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnBnClickedTransSelNewCheck();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnChar(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg void OnKeyDown(UINT nChar, UINT nRepCnt, UINT nFlags);
private:
	CFloatEdit m_trans_x_vec;
	CFloatEdit m_trans_y_vec;
	CFloatEdit m_trans_z_vec;
public:
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
};
