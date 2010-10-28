#pragma once

class CNewTheme
{
public:
  CNewTheme(void);
  virtual ~CNewTheme(void);

	/*
    // virtual from base class 
  void DrawBorders(CDC* pDC, CRect& rect);
  void DrawGripper(CDC* pDC, const CRect& rect); 

  // Measure an item  
  virtual void MeasureItem(LPMEASUREITEMSTRUCT lpMIS);

    // Drawing: 
  virtual BOOL DrawMenubarItem(CWnd* pWnd,CMenu* pMenu, UINT nItemIndex,UINT nState);
  // Draw an item
  virtual void DrawItem(LPDRAWITEMSTRUCT lpDIS);
  // Draw title of the menu
  virtual void DrawTitle(LPDRAWITEMSTRUCT lpDIS, BOOL bIsMenuBar);

  // Erase the Background of the menu
  virtual BOOL EraseBkgnd(HWND hWnd, HDC hDC);
	*/
};

// Application-level support for theme using
void SetupTheme( int nThemeID, int nSkinUsed, CString strSkin );
void InstallThemeSupport();




