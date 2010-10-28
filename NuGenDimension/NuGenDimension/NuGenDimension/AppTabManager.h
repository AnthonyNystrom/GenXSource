#pragma once
#include "EGTabCtrl.h"
typedef struct
{
	CDocTemplate* rtClass;
	int  nTabId;
	TCHAR szText[101];
} TAB_STRUCT;
// CAppTabManager

class CAppTabManager : public CControlBar
{
	DECLARE_DYNAMIC(CAppTabManager)

	std::vector<TAB_STRUCT*> m_tab_infos; 
	CEGTabCtrl	 m_tabs;
	CFrameWnd* m_pFrameWnd;

public:
	CAppTabManager();
	virtual ~CAppTabManager();

	virtual void OnUpdateCmdUI(CFrameWnd* pTarget, BOOL bDisableIfNoHndler);
	virtual CSize CalcFixedLayout(BOOL bStretch, BOOL bHorz);
	virtual CSize CalcDynamicLayout(int nLength, DWORD nMode);

	BOOL CreateAppTabManager( CFrameWnd* pTarget );

	int TabIdByRT( CDocTemplate* rtClass );
	void AddTab( CDocTemplate* rtClass , const char* str);
	void RemoveTab( CDocTemplate* rtClass );
	void ActivateTab( CDocTemplate* rtClass );

protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
	afx_msg void OnSize(UINT nType, int cx, int cy);
	afx_msg void OnTabChanged( NMHDR * pNotifyStruct, LRESULT * result );
};


