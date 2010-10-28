#pragma once

#include "..//resource.h"
#include "..//Commands//Contour.h"
// CContScenarDlg dialog

class CContScenarDlg : public CDialog, public IBaseInterfaceOfGetDialogs
{
	DECLARE_DYNAMIC(CContScenarDlg)

public:
	CContScenarDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CContScenarDlg();

// Dialog Data
	enum { IDD = IDD_CONT_SCENARS_DLG };

	virtual  DLG_TYPE  GetType() {return IBaseInterfaceOfGetDialogs::USER_DIALOG;};
	virtual  CWnd*     GetWindow() {return this;};   

	virtual  void      EnableControls(bool) {};

private:
	Contour*       m_commander;
public:
	void   SetCommander(Contour* nC) {ASSERT(nC); m_commander=nC;};
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
	virtual void OnOK() {};
	virtual void OnCancel() {};
public:
	afx_msg void OnBnClickedContScenarsRadio1();
	afx_msg void OnBnClickedContScenarsRadio2();
	virtual BOOL OnInitDialog();
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
};
