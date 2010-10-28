#pragma once

#include "ExGuiLib_res.h"

#include <vector>
#include <set>

using std::vector;
using std::set;

/*
** CEGParamsPage dialog
** Border = None, Style = Child, Control = TRUE
*/
class CEGParamsDlg;
class CEGParamsPage :  public CDialog
{
protected:
	CEGParamsDlg* m_pParamsDlg;
	BOOL m_bIsDirty;
	TCHAR* m_pszName;
	TCHAR* m_pszKey;
	UINT m_nResourceID;

public:
	CEGParamsPage ( UINT nIDResource, TCHAR* lpszName, TCHAR* lpszKey, CEGParamsDlg* pParamsDlg );
	~CEGParamsPage ();
	
	void SetDirty( BOOL bIsDirty );
	afx_msg void OnSetDirty();
	
	BOOL IsDirty(){ return m_bIsDirty; }
	virtual BOOL Save() = 0;

	// optional event for make some after-post actions
	virtual void OnBeforeClose(){};

	TCHAR* GetKey(){ return m_pszKey; }
	TCHAR* GetName(){ return m_pszName; }
	UINT GetResourceID(){ return m_nResourceID; }
};

/*
** CEGParamsDlg dialog
*/
class CEGParamsDlg : public CDialog
{
	DECLARE_DYNAMIC(CEGParamsDlg)

	CImageList m_ilParams;
	HWND m_hTree;
	HWND m_hCurWnd;
	HTREEITEM m_hCursor;
	CRect m_rcParam;

	vector<CEGParamsPage*> m_pages;
	set<CEGParamsPage*>	m_dirty_pages;
public:
	CEGParamsDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CEGParamsDlg();

	void AddPage( CEGParamsPage* page );
	void SetPageDirty( CEGParamsPage* pPage, BOOL bDirty);

	// Dialog Data
	enum { IDD = IDD_PARAMS };
	
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
public:
	virtual BOOL OnInitDialog();
	CTreeCtrl m_tree;
	afx_msg void OnSelChanged(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnItemExpanded(NMHDR *pNMHDR, LRESULT *pResult);
	afx_msg void OnApply();
protected:
	virtual void OnOK();
public:
	afx_msg HBRUSH OnCtlColor(CDC* pDC, CWnd* pWnd, UINT nCtlColor);
};
