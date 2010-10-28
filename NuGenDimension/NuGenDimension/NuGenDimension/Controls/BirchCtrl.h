#if !defined(AFX_BIRCHCTRL_H__A9777424_A55F_4A55_BE9D_5308028DB601__INCLUDED_)
#define AFX_BIRCHCTRL_H__A9777424_A55F_4A55_BE9D_5308028DB601__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// StaticTreeCtrl.h : header file
//
/////////////////////////////////////////////////////////////////////////////
#include <vector>

#if defined( _UNICODE )
	#define STRCPY(x,y)				wcscpy(x,y)
#else
	#define STRCPY(x,y)				strcpy(x,y)
#endif

/////////////////////////////////////////////////////////////////////////////
// CTreeNode class

class CTreeNode
{
public:
	CTreeNode()
	{
		csLabel.Empty();
		rNode.SetRectEmpty();

		bUseDefaultTextColor = true;

		bOpen		= false;
		bEditable   = false;
		bVisible    = true;

		pParent		= NULL;
		iNodeLevel  = 0;
		pNextSibling= NULL;
		pPrevSibling= NULL;
		pFirstChild	= NULL;
		pLastChild  = NULL;

		userData    = NULL;
		bAutoDeleteUserData = false;
	}

	virtual ~CTreeNode()
	{
		csLabel.Empty();
		item_bitmaps.clear();
		if (userData && bAutoDeleteUserData)
			delete userData;
		userData = NULL;
	}

	CString		csLabel;
	CRect		rNode;
	
	COLORREF	crText;
	bool		bUseDefaultTextColor;

	bool		bOpen;
	bool        bVisible;

	bool        bEditable;

	CTreeNode*	pParent;

	unsigned int iNodeLevel;

	CTreeNode*	pNextSibling;
	CTreeNode*  pPrevSibling;

	CTreeNode*	pFirstChild;
	CTreeNode*  pLastChild;

	std::vector<CBitmap32*>   item_bitmaps;

	void*       userData;
	bool        bAutoDeleteUserData;
};

#define    HTREENODE   CTreeNode*
#define    HTOPNODE    ( (HTREENODE) -0x10000 )

class  CBirch 
{
public:
	CBirch();
	~CBirch();
protected:
	HTREENODE		m_pTopNode;
	HTREENODE		m_pSelected;
	CWnd*           m_wnd;

public:

	HTREENODE       GetTopNode() {return m_pTopNode;};

	HTREENODE		InsertSibling	(	HTREENODE pInsertAfter, const CString& csLabel,
											COLORREF crText = 0, BOOL bUseDefaultTextColor = TRUE);
	HTREENODE		InsertChild		(	HTREENODE pParent, const CString& csLabel,
											COLORREF crText = 0, BOOL bUseDefaultTextColor = TRUE);
	void			DeleteNode		( HTREENODE pNode);

	void			ToggleNode		( HTREENODE pNode);
	void			SetNodeColor	( HTREENODE pNode, COLORREF crText);

	void			DeleteSubTree	( HTREENODE pNode );		// Recursive delete

	void     SetWnd(CWnd* wnd) {m_wnd = wnd;};

};



/////////////////////////////////////////////////////////////////////////////
// CTreeItemEdit
class CBirchCtrl;

class CTreeItemEdit : public CEdit
{
	DECLARE_DYNAMIC(CTreeItemEdit)

private:
	CFont			m_font;
	CBirchCtrl*     m_birch_ctrl;
public:
	BOOL			CreateFont(LONG lfHeight, LPCTSTR lpszFaceName);
	void            SetBirch(CBirchCtrl* nb) {m_birch_ctrl=nb;};

public:
	CTreeItemEdit();
	virtual ~CTreeItemEdit();

protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnKeyUp(UINT nChar, UINT nRepCnt, UINT nFlags);
	afx_msg void OnKillFocus(CWnd* pNewWnd);
};
/////////////////////////////////////////////////////////////////////////////
// CBirchCtrl window

class CBirchCtrl : public CWnd
{
    friend class CTreeItemEdit;
public:
	CBirchCtrl();

// Attributes
protected:
	LOGFONT			m_lgFont;
	CFont			m_Font;
	COLORREF		m_crDefaultTextColor;
	COLORREF		m_crConnectingLines;

	BOOL        	m_bShowLines;

	int				m_iDocHeight;
	BOOL			m_bScrollBarMessage;

	int				m_iLineHeight;
	int             m_iLineHeightByFont;
	int				m_iIndent;
	int				m_iPadding;

	CBirch*         m_birch;

	CTreeItemEdit*  m_edit;

	HTREENODE       m_editable_item;
	
// Operations
public:
	virtual void   SetLineHeight(int newLH)
	{
		if (newLH>=m_iLineHeightByFont)
			m_iLineHeight = newLH;
		else
			m_iLineHeight = m_iLineHeightByFont;
	};

	void           SetBirch(CBirch* newB)   {m_birch = newB;};

	virtual CBirchCtrl&	SetTextFont			( LONG nHeight, BOOL bBold, BOOL bItalic, const CString& csFaceName );	
	virtual CBirchCtrl&	SetDefaultTextColor	( COLORREF crText );
protected:
	int				DrawNodesRecursive	( CDC* pDC, HTREENODE pNode, int y, 
							CPen* linesPen, CPen* marksPen, CRect rFrame );
	
	void			ResetScrollBar		();

	HTREENODE		FindNodeByPoint		( const CPoint& point, HTREENODE pNode );

	virtual   void  StartEditLabel(HTREENODE pNode, CString& newLabel) {};
	virtual   void  FinishEditLabel(HTREENODE pNode, CString& newLabel) {};
	virtual   void  ClickOnIcon(HTREENODE pNode, unsigned int iconNumb) {};

private:
	void    FireMessageFromEdit();
// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CBirchCtrl)
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CBirchCtrl();

	// Generated message map functions
protected:
	//{{AFX_MSG(CBirchCtrl)
	afx_msg void OnPaint();
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnVScroll(UINT nSBCode, UINT nPos, CScrollBar* pScrollBar);
	afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
	afx_msg BOOL OnMouseWheel(UINT nFlags, short zDelta, CPoint pt);
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

#endif // !defined(AFX_BIRCHCTRL_H__A9777424_A55F_4A55_BE9D_5308028DB601__INCLUDED_)

