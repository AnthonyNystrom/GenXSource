#pragma once

#include "EGParamsDlg.h"
#include "EGMenu.h"

// CEGAppParamsPage dialog

class CEGAppParamsPage : public CEGParamsPage
{
	DECLARE_DYNAMIC(CEGAppParamsPage)
	UINT m_nOldThemeID;
	UINT m_nOldSkinUsed;
	CString m_strOldSkin;
	UINT m_nNewThemeID;
	UINT m_nNewSkinUsed;
	CString m_strNewSkin;
	CEGMenu* m_pDefaultNewMenu;
public:
	CEGAppParamsPage( CEGParamsDlg* pParamsDlg, CEGMenu* pDefaultNewMenu, TCHAR* lpszKey );   // standard constructor
	virtual ~CEGAppParamsPage();

// Dialog Data
	enum { IDD = IDD_PARAMS_GUI_PAGE };
	virtual BOOL Save();
	virtual void OnBeforeClose();

	HWND m_hSkins;
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL OnInitDialog();

protected:
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnEnableSkins();
	afx_msg void OnSelectTheme();
};


