#pragma once

#include "..//resource.h"
#include "..//Commands//Contour.h"
// CContObjDlg dialog

class CContObjDlg : public CDialog, public IBaseInterfaceOfGetDialogs
{
	DECLARE_DYNAMIC(CContObjDlg)

public:
	CContObjDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CContObjDlg();

// Dialog Data
	enum { IDD = IDD_CONT_OBJ_TYPE };

	virtual  DLG_TYPE  GetType() {return IBaseInterfaceOfGetDialogs::USER_DIALOG;};
	virtual  CWnd*     GetWindow() {return this;};   

	virtual  void      EnableControls(bool) {};

	void     EnableLineType(bool);

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
	afx_msg void OnBnClickedContObjRadio1();
	afx_msg void OnBnClickedContObjRadio2();
	virtual BOOL OnInitDialog();
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
};
