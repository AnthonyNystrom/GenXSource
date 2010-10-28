#pragma once
#include "EGTabCtrl.h"

#include <vector>
using std::vector;

class CEGPageCtrl :
	public CEGTabCtrl
{
protected:
	vector<CPropertyPage*> m_pages;
	
	CPropertyPage* m_pActivePage;
	CRect GetPageRect();
public:
	CEGPageCtrl(void);
	~CEGPageCtrl(void);

	void AddPage( CPropertyPage* pPage, UINT nResourceID, TCHAR* lpszTitle );
	
	BOOL TryFinish(); // Queries child page for OnWizzardFinish

protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnTabChanged( NMHDR * pNotifyStruct, LRESULT * result );
	//afx_msg UINT OnGetDlgCode( );

};

