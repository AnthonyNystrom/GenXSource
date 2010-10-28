#pragma once
#include "afxcmn.h"

#include "..//resource.h"
// CSplinePointsDlg dialog

class CSplinePointsDlg : public CDialog, public IBaseInterfaceOfGetDialogs
{
	DECLARE_DYNAMIC(CSplinePointsDlg)

public:
	CSplinePointsDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CSplinePointsDlg();

// Dialog Data
	enum { IDD = IDD_SPLINE_POINTS_DLG };

	virtual  DLG_TYPE  GetType() {return IBaseInterfaceOfGetDialogs::USER_DIALOG;};
	virtual  CWnd*     GetWindow() {return this;};   

	virtual  void      EnableControls(bool) {};

	void     AddPoint(const SG_POINT&);
	void     RemoveAllPoints();

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
	virtual void OnOK();
	virtual void OnCancel();
public:
	afx_msg void OnSize(UINT nType, int cx, int cy);
private:
	CListCtrl m_list;
public:
	virtual BOOL OnInitDialog();
};
