#ifndef __CEGToolBar_H_
#define __CEGToolBar_H_

#pragma once 

class GUILIBDLLEXPORT CEGToolBar : public CToolBar
{
  DECLARE_DYNAMIC(CEGToolBar)

public:
  CEGToolBar();
  virtual ~CEGToolBar();

public:
  // take the first pixel top/left for the tranparent-color,
  // when image has more than 16 colors
  BOOL LoadToolBar(LPCTSTR lpszResourceName);
  BOOL LoadToolBar(UINT nIDResource);

  // For replacing the toolbar with a high-color image
  BOOL LoadHiColor(LPCTSTR lpszResourceName,COLORREF transparentColor=CLR_DEFAULT);
  BOOL LoadHiColor(HBITMAP hBMP, LPWORD pwColors);

  bool InsertControl (int nIndex, CWnd* pControl, DWORD_PTR dwData=NULL);

  // Change the toolbar button to a menu button
  bool SetMenuButton (int nIndex);
  bool SetMenuButtonID(UINT nCommandID);

  // show the menu near the toolbar button
  void TrackPopupMenu (UINT nID, CMenu* pMenu);

  CSize GetImageSize(){ return (m_sizeImage); }

public:
  // virtual
  CSize CalcDynamicLayout (int nLength, DWORD dwMode);
  LRESULT DefWindowProc(UINT nMsg, WPARAM wParam, LPARAM lParam);

public:
  // virtual from base class 
  void DrawBorders(CDC* pDC, CRect& rect);
  void DrawGripper(CDC* pDC, const CRect& rect); 

  void OnBarStyleChange(DWORD dwOldStyle, DWORD dwNewStyle);

  // overwritten from baseclass not virtual
  void EraseNonClient();

protected:

  afx_msg void OnNcPaint();
  afx_msg BOOL OnEraseBkgnd(CDC* pDC);
  afx_msg void OnNMCustomdraw(NMHDR *pNMHDR, LRESULT *pResult); 
  afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
  afx_msg void OnPaint();
	afx_msg LRESULT OnIdleUpdateCmdUI(WPARAM wParam, LPARAM lParam);

  DECLARE_MESSAGE_MAP()

protected:
  void PaintToolBarBackGnd(CDC* pDC);
  void PaintCorner(CDC *pDC, LPCRECT pRect, COLORREF color);
  void PaintOrangeState(CDC *pDC, CRect rc, bool bHot);

  void PaintTBButton(LPNMTBCUSTOMDRAW pInfo);
  BOOL PaintHotButton(LPNMTBCUSTOMDRAW lpNMCustomDraw); 


protected:
  BOOL OnMenuInput(MSG msg);

  static CEGToolBar* g_pNewToolBar;
  static HHOOK g_hMsgHook;
  static LRESULT CALLBACK MenuInputFilter(int code, WPARAM wParam, LPARAM lParam);

private:
  int m_ActMenuIndex;
  CEGmageList m_ImageList;
  CEGmageList m_GloomImageList;
  CEGmageList m_ImageListDisabled;
  CMenu* m_pCustomizeMenu;
  DWORD m_DoCheck;
};

/*class GUILIBDLLEXPORT CEGToolBarEx : public CEGToolBar
{
	DECLARE_MESSAGE_MAP()
protected:
	afx_msg LRESULT OnIdleUpdateCmdUI(WPARAM wParam, LPARAM lParam);
};
*/


#endif //__CEGToolBar_H_ 
