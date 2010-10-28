#pragma once
#ifndef __CNewProperty_H_
#define __CNewProperty_H_


// CEGPropertyPage dialog

class GUILIBDLLEXPORT CEGPropertyPage : public CEGFrame<CPropertyPage>
{
  DECLARE_DYNAMIC(CEGPropertyPage)

  TCHAR* m_pszTitle;
  TCHAR* m_pszSubTitle;
    
public:
  CEGPropertyPage();
  virtual ~CEGPropertyPage();


#if _MFC_VER < 0x0700 
  CEGPropertyPage(UINT nIDTemplate, UINT nIDCaption = 0);
  CEGPropertyPage(LPCTSTR lpszTemplateName, UINT nIDCaption = 0);
#else  
  CEGPropertyPage(UINT nIDTemplate, UINT nIDCaption = 0, DWORD dwSize = sizeof(PROPSHEETPAGE));
  CEGPropertyPage(LPCTSTR lpszTemplateName, UINT nIDCaption = 0, DWORD dwSize = sizeof(PROPSHEETPAGE));
  
  // extended construction
  CEGPropertyPage(UINT nIDTemplate, UINT nIDCaption,UINT nIDHeaderTitle, UINT nIDHeaderSubTitle = 0, DWORD dwSize = sizeof(PROPSHEETPAGE));
  CEGPropertyPage(LPCTSTR lpszTemplateName, UINT nIDCaption, 	UINT nIDHeaderTitle, UINT nIDHeaderSubTitle = 0, DWORD dwSize = sizeof(PROPSHEETPAGE));
  CEGPropertyPage(UINT nIDTemplate, TCHAR* lpszHeaderTitle, TCHAR* lpszHeaderSubTitle, DWORD dwSize = sizeof(PROPSHEETPAGE) );

  void SetWizardButtons( DWORD dwFlags );
#endif //_MFC_VER < 0x0700
  
protected:
  DECLARE_MESSAGE_MAP()
};

class GUILIBDLLEXPORT CEGStepsPropertyPage: 
	public CEGPropertyPage
{
	int m_nStep;
	HWND m_hList;
	HWND m_hProgress;
public:
	CEGStepsPropertyPage( UINT nIDTemplate, TCHAR* lpszTitle, TCHAR* lpszSubTitle );

	void InitStepControls( UINT nIDListView, UINT nIDProgress, UINT nIDBImages );

	void AddStep( TCHAR* lpszName );

	void SetStepImage(int nIndex, int nImage);
	
	void BeginSubStep(DWORD dwMin, DWORD dwMax);
	void NextSubStep();

	void BeginWork();
	void BeginStep(DWORD dwMin=0, DWORD dwMax=0);
	void FinishStep(BOOL bSucceed);
};

class GUILIBDLLEXPORT CEGPropertySheet : public CEGFrame<CPropertySheet>
{
  DECLARE_DYNAMIC(CEGPropertySheet)
    
public:
  CEGPropertySheet();
  virtual ~CEGPropertySheet();
  
  CEGPropertySheet(UINT nIDCaption, CWnd* pParentWnd = NULL, UINT iSelectPage = 0);
  CEGPropertySheet(LPCTSTR pszCaption, CWnd* pParentWnd = NULL, UINT iSelectPage = 0);
  
#if _MFC_VER < 0x0700

#else
  // extended construction
  CEGPropertySheet(UINT nIDCaption, CWnd* pParentWnd,	UINT iSelectPage, HBITMAP hbmWatermark,	HPALETTE hpalWatermark = NULL, HBITMAP hbmHeader = NULL);
  CEGPropertySheet(LPCTSTR pszCaption, CWnd* pParentWnd,	UINT iSelectPage, HBITMAP hbmWatermark,	HPALETTE hpalWatermark = NULL, HBITMAP hbmHeader = NULL);

  void SetWizard97Mode(  UINT nDBImage );
#endif //_MFC_VER < 0x0700

// Implementation
public:
    // Overridables (special message map entries)
  virtual BOOL OnInitDialog();

  // Jan-18-2005 - Mark P. Peterson - mpp@rhinosoft.com - http://www.RhinoSoft.com/
  // added these calls to return the correct pointer, assuming all pages are derived from CEGPropertyPage
  CEGPropertyPage* GetActivePage() const				{ return ((CEGPropertyPage *) CPropertySheet::GetActivePage()); }
  CEGPropertyPage* GetPage(int nPage) const			{ return ((CEGPropertyPage *) CPropertySheet::GetPage(nPage)); }

protected:
  DECLARE_MESSAGE_MAP()
};

#endif //__CNewProperty_H_
