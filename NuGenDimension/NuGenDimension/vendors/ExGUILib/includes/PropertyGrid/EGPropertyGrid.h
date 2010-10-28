// PropTree.h : header file
//
//  Copyright (C) 1998-2001 Scott Ramsay
//	sramsay@gonavi.com
//	http://www.gonavi.com
//
//  This material is provided "as is", with absolutely no warranty expressed
//  or implied. Any use is at your own risk.
// 
//  Permission to use or copy this software for any purpose is hereby granted 
//  without fee, provided the above notices are retained on all copies.
//  Permission to modify the code and to distribute modified code is granted,
//  provided the above notices are retained, and a notice that the code was
//  modified is included with the above copyright notice.
// 
//	If you use this code, drop me an email.  I'd like to know if you find the code
//	useful.

#if !defined(AFX_PROPT_H__386AA426_6FB7_4B4B_9563_C4CC045BB0C9__INCLUDED_)
#define AFX_PROPT_H__386AA426_6FB7_4B4B_9563_C4CC045BB0C9__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "EGPropertyGridList.h"
#include "EGPropertyGridInfo.h"

#include "EGPropertyGridItem.h"
#include "EGPropertyGridItemStatic.h"
#include "EGPropertyGridItemEdit.h"
#include "EGPropertyGridItemCombo.h"
#include "EGPropertyGridItemColor.h"


class CEGPropertyGrid;

#include "EGMenu.h"

typedef BOOL (CALLBACK* ENUMPROPITEMPROC)(CEGPropertyGrid*, CEGPropertyGridItem*, LPARAM);

// CEGPropertyGrid window styles
#define PTS_NOTIFY				0x00000001

// CEGPropertyGrid HitTest return codes
#define HTPROPFIRST					50

#define HTLABEL						(HTPROPFIRST + 0)
#define HTCOLUMN					(HTPROPFIRST + 1)
#define HTEXPAND					(HTPROPFIRST + 2)
#define HTATTRIBUTE					(HTPROPFIRST + 3)
#define HTCHECKBOX					(HTPROPFIRST + 4)

// CEGPropertyGrid WM_NOTIFY notification structure
typedef struct _NMPROPTREE
{
    NMHDR			hdr;
	CEGPropertyGridItem*	pItem;
} NMPROPTREE, *PNMPROPTREE, FAR *LPNMPROPTREE;

// CEGPropertyGrid specific Notification Codes
#define PTN_FIRST					(0U-1100U)

#define PTN_INSERTITEM				(PTN_FIRST-1)
#define PTN_DELETEITEM				(PTN_FIRST-2)
#define PTN_DELETEALLITEMS			(PTN_FIRST-3)
#define PTN_ITEMCHANGED				(PTN_FIRST-5)
#define PTN_ITEMBUTTONCLICK			(PTN_FIRST-6)
#define PTN_SELCHANGE				(PTN_FIRST-7)
#define PTN_ITEMEXPANDING			(PTN_FIRST-8)
#define PTN_COLUMNCLICK				(PTN_FIRST-9)
#define PTN_PROPCLICK				(PTN_FIRST-10)
#define PTN_CHECKCLICK				(PTN_FIRST-12)

/////////////////////////////////////////////////////////////////////////////
// CEGPropertyGrid window

class GUILIBDLLEXPORT CEGPropertyGrid : public CWnd
{
// Construction
public:
	CEGPropertyGrid();
	virtual ~CEGPropertyGrid();

	BOOL Create(DWORD dwStyle, const RECT& rect, CWnd* pParentWnd, UINT nID);

// Attributes/Operations
public:
	static CFont* GetNormalFont();
	static CFont* GetBoldFont();

	// Returns the root item of the tree
	CEGPropertyGridItem* GetRootItem();

	// Returns the focused item or NULL for none
	CEGPropertyGridItem* GetFocusedItem();

	// Enumerates an item and all its child items
	BOOL EnumItems(CEGPropertyGridItem* pItem, ENUMPROPITEMPROC proc, LPARAM lParam = 0L);

	// Insert a created CEGPropertyGridItem into the control
	CEGPropertyGridItem* InsertItem(CEGPropertyGridItem* pItem, CEGPropertyGridItem* pParent = NULL);

	// Delete an item and ALL its children
	void DeleteItem(CEGPropertyGridItem* pItem);

	// Delete all items from the tree
	void DeleteAllItems();

	// Return the splitter position
	LONG GetColumn();

	// Set the splitter position
	void SetColumn(LONG nColumn);

	// Sets the focused item
	void SetFocusedItem(CEGPropertyGridItem* pItem);

	// Show or hide the info text
	void ShowInfoText(BOOL bShow = TRUE);

	// Returns TRUE if the item is visible (its parent is expanded)
	BOOL IsItemVisible(CEGPropertyGridItem* pItem);

	// Ensures that an item is visible
	void EnsureVisible(CEGPropertyGridItem* pItem);

	// do a hit test on the control (returns a HTxxxx code)
	LONG HitTest(const POINT& pt);

	// find an item by a location
	CEGPropertyGridItem* FindItem(const POINT& pt);

	// find an item by item id
	CEGPropertyGridItem* FindItem(UINT nCtrlID);

protected:
	// Actual tree control
	CEGPropertyGridList	m_List;

	// Descriptive control
	CEGPropertyGridInfo	m_Info;

	// TRUE to show info control
	BOOL			m_bShowInfo;

	// Height of the info control
	LONG			m_nInfoHeight;

	// Root level tree item
	CEGPropertyGridItem	m_Root;

	// Linked list of visible items
	CEGPropertyGridItem*	m_pVisbleList;

	// Pointer to the focused item (selected)
	CEGPropertyGridItem*	m_pFocus;

	// PropTree scroll position. x = splitter position, y = vscroll position
	CPoint			m_Origin;

	// auto generated last created ID
	UINT			m_nLastUID;

	// Number of CEGPropertyGrid controls in the current application
	static UINT		s_nInstanceCount;

	static CFont*	s_pNormalFont;
	static CFont*	s_pBoldFont;

	BOOL			m_bDisableInput;

	// Used for enumeration
	static CEGPropertyGridItem*	s_pFound;

public:
	//
	// functions used by CEGPropertyGridItem (you normally dont need to call these directly)
	//

	void AddToVisibleList(CEGPropertyGridItem* pItem);
	void ClearVisibleList();

	void SetOriginOffset(LONG nOffset);
	void UpdatedItems();
	void UpdateMoveAllItems();
	void RefreshItems(CEGPropertyGridItem* pItem = NULL);

	// enable or disable tree input
	void DisableInput(BOOL bDisable = TRUE);
	BOOL IsDisableInput();

	BOOL IsSingleSelection();

	CEGPropertyGridItem* GetVisibleList();
	CWnd* GetCtrlParent();

	const POINT& GetOrigin();

	void SelectItems(CEGPropertyGridItem* pItem, BOOL bSelect = TRUE);

	// Focus on the first visible item
	CEGPropertyGridItem *FocusFirst();

	// Focus on the last visible item
	CEGPropertyGridItem *FocusLast();

	// Focus on the previous item
	CEGPropertyGridItem *FocusPrev();
	
	// Focus on the next item
	CEGPropertyGridItem *FocusNext();

	LRESULT SendNotify(UINT nNotifyCode, CEGPropertyGridItem* pItem = NULL);

protected:
	// Resize the child windows to fit the exact dimensions the CEGPropertyGrid control
	void ResizeChildWindows(int cx, int cy);

	// Initialize global resources, brushes, fonts, etc.
	void InitGlobalResources();

	// Free global resources, brushes, fonts, etc.
	void FreeGlobalResources();

	// Recursive version of DeleteItem
	void Delete(CEGPropertyGridItem* pItem);

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CEGPropertyGrid)
	//}}AFX_VIRTUAL

// Implementation
private:
	static BOOL CALLBACK EnumFindItem(CEGPropertyGrid* pProp, CEGPropertyGridItem* pItem, LPARAM lParam);
	static BOOL CALLBACK EnumSelectAll(CEGPropertyGrid*, CEGPropertyGridItem* pItem, LPARAM lParam);
	static BOOL CALLBACK EnumMoveAll(CEGPropertyGrid*, CEGPropertyGridItem* pItem, LPARAM);
	static BOOL CALLBACK EnumRefreshAll(CEGPropertyGrid*, CEGPropertyGridItem* pItem, LPARAM);

	// Generated message map functions
protected:
	//{{AFX_MSG(CEGPropertyGrid)
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnEnable(BOOL bEnable);
	afx_msg void OnSysColorChange();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_PROPT_H__386AA426_6FB7_4B4B_9563_C4CC045BB0C9__INCLUDED_)
