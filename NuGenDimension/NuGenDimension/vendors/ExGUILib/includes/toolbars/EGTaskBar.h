#pragma once

#include "EGCtrlBar.h"

class CEGTaskBar :
	public CEGControlBar
{
  BOOL m_bActive;
	HWND m_hActiveTask;
	
	TCHAR* m_pszCaption;
	HFONT m_fntCaption;
	HICON m_hIcon;
	BOOL m_bAutoFree;

	void GetCaptionRect( CRect& rc );
	void InvalidateCaption();

public:
	CEGTaskBar(void);
	~CEGTaskBar(void);

// Operations
	void SetActiveTask( HWND hTask, TCHAR* pszTitle );
	HWND GetActiveTask( );

	BOOL SetIcon( UINT nIDResource, HINSTANCE hInst = NULL );
	BOOL SetIcon( LPCTSTR lpszResourceName, HINSTANCE hInst = NULL );
	BOOL SetIcon( HICON hIcon, BOOL bAutoFree = FALSE );

// Overrides
protected:
	virtual void OnDraw( CDC * pDC, CRect& rc );
	virtual void OnResize( CRect& rc );
	virtual void OnUpdateCmdUI(CFrameWnd* pTarget, BOOL bDisableIfNoHndler);


protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnSetFocus( CWnd* pOldWnd );
};
