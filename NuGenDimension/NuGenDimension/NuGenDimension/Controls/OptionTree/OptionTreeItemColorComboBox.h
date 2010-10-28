#ifndef OT_ITEMCOLORCOMBOBOX
#define OT_ITEMCOLORCOMBOBOX

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// OptionTreeItemComboBox.h : header file
//

// Added Headers
//#include "CommonRes.h"
#include "OptionTreeDef.h"
#include "OptionTreeItem.h"

// Classes
class COptionTree;

/////////////////////////////////////////////////////////////////////////////
// COptionTreeItemColorComboBox window

class COptionTreeItemColorComboBox : public CComboBox, public COptionTreeItem
{
// Construction
public:
	COptionTreeItemColorComboBox();
	virtual void OnMove();
	virtual void OnRefresh();
	virtual void OnCommit();
	virtual void OnActivate();
	virtual void CleanDestroyWindow();
	virtual void OnDeSelect();
	virtual void OnSelect();
	virtual void DrawAttribute(CDC *pDC, const RECT &rcRect);
// Attributes
private:
	unsigned int   m_color_index;
public:
	unsigned int   GetCurColor() const {return m_color_index;};
	void           SetCurColor(unsigned int ci){m_color_index = ci;};

	void           LostFocus() {m_bFocus=FALSE;};
// Operations
public:

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(COptionTreeItemColorComboBox)
	//}}AFX_VIRTUAL

// Implementation
public:
	long GetDropDownHeight();
	void SetDropDownHeight(long lHeight);
	BOOL CreateComboItem(DWORD dwAddStyle = 0);
	virtual ~COptionTreeItemColorComboBox();

	// Generated message map functions
protected:
	BOOL m_bFocus;
	long m_lDropDownHeight;
	//{{AFX_MSG(COptionTreeItemColorComboBox)
	afx_msg void OnSetFocus(CWnd* pOldWnd);
	afx_msg void OnKillFocus(CWnd* pNewWnd);
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
public:
	virtual int  CompareItem(LPMEASUREITEMSTRUCT /*lpMeasureItemStruct*/) {return 0;};
	virtual void MeasureItem(LPMEASUREITEMSTRUCT /*lpMeasureItemStruct*/);
	virtual void DrawItem(LPDRAWITEMSTRUCT lpDrawItemStruct);

};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !OT_ITEMCOMBOBOX
