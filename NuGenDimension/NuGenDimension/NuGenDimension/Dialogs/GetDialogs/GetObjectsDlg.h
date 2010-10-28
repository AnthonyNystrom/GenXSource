#pragma once
#include "afxcmn.h"

#include "..//..//Controls//SelectingListCtrl.h"
// CGetObjectsDlg dialog

typedef struct  
{
	sgCObject*  objc;
	CString     obName;
} OBJCTS;

class CGetObjectsDlg : public CDialog, public IGetObjectsPanel
{
	DECLARE_DYNAMIC(CGetObjectsDlg)

public:
	CGetObjectsDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CGetObjectsDlg();

// Dialog Data
	enum { IDD = IDD_GET_OBJECTS_DLG };

	virtual  DLG_TYPE  GetType();
	virtual  CWnd*     GetWindow();   

	virtual void  RemoveAllObjects();
	virtual void  SetMultiselectMode(bool);
	virtual void  FillList(LPFUNC_FILL_OBJECTS_LIST isAdd=NULL);

	virtual void  AddObject(sgCObject*,bool);
	virtual void  RemoveObject(sgCObject*);
	virtual void  SelectObject(sgCObject*, bool);

	virtual  void      EnableControls(bool);

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
private:
	LPFUNC_FILL_OBJECTS_LIST    m_fill_function;

	bool*      m_enable_history;
	bool       m_was_diasabled;

	std::vector<OBJCTS>   m_objcts;
	
protected:
	virtual void OnOK();
	virtual void OnCancel();
public:
	virtual BOOL OnInitDialog();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg BOOL OnEraseBkgnd(CDC* pDC);
private:
	CSelectingListCtrl m_list;
public:
	afx_msg void OnLvnGetdispinfoGetObjectsList(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnNMClickGetObjectsList(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnBnClickedGetObjectsFinishButton();
};
