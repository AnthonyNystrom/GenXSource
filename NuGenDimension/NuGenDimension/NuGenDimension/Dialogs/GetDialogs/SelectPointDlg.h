#pragma once
#include "afxcmn.h"

#include "..//..//Controls//SelectingListCtrl.h"

typedef struct  
{
	double pX;
	double pY;
	double pZ;
	CString  pStr;
} PNTS;
// CSelectPointDlg dialog

class CSelectPointDlg : public CDialog, public ISelectPointPanel
{
	DECLARE_DYNAMIC(CSelectPointDlg)

	std::vector<PNTS>  m_points;

public:
	CSelectPointDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CSelectPointDlg();

// Dialog Data
	enum { IDD = IDD_SELECT_POINT_DLG };

	virtual  DLG_TYPE  GetType();
	virtual  CWnd*     GetWindow();   

	virtual  void      EnableControls(bool);

	virtual void   AddPoint(double,double,double);
	virtual void   RemoveAllPoints();
	virtual void   SetCurrentPoint(unsigned int);
	virtual unsigned int    GetCurrentPoint();


protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
private:
	CSelectingListCtrl m_list;
	bool*      m_enable_history;
	bool       m_was_diasabled;
protected:
	virtual void OnCancel();
	virtual void OnOK();
public:
	virtual BOOL OnInitDialog();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnLvnGetdispinfoSelectPointList(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnNMClickSelectPointList(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
	afx_msg void OnBnClickedSelectPointFinishButton();
};
