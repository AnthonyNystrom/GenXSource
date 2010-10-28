#pragma once
#include "afxcmn.h"

#include "EGHeaderControl.h"
#include "EGMenu.h"

class CEGListCtrl :
	public CListCtrl
{
	CEGHeaderControl m_ctlHeader;
	CEGMenu* m_pMenuContext;
	CWnd* m_pWndMenuHandler;

public:
	CEGListCtrl(void);
	~CEGListCtrl(void);

	void SetMenu( CEGMenu *pMenu, CWnd* pWnd = NULL );

	// Generated message map functions
protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnContextMenu( CWnd* pWnd, CPoint pt);
};
