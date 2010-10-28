#ifndef __NewDialogBar_H_
#define __NewDialogBar_H_

#pragma once


// CEGDialogBar
class CEGDialogBar : public CDialogBar
{
	DECLARE_DYNAMIC(CEGDialogBar)

public:
	CEGDialogBar();
	virtual ~CEGDialogBar();

  public:
  // virtual from base class 
	virtual void DoPaint(CDC* pDC);
  void DrawBorders(CDC* pDC, CRect& rect);
  void DrawGripper(CDC* pDC, const CRect& rect); 
  void PaintToolBarBackGnd(CDC* pDC);
  void PaintCorner(CDC *pDC, LPCRECT pRect, COLORREF color);

//  void OnBarStyleChange(DWORD dwOldStyle, DWORD dwNewStyle);

  // overwritten from baseclass not virtual
  void EraseNonClient();

protected:
  afx_msg void OnNcPaint();
  afx_msg BOOL OnEraseBkgnd(CDC* pDC);
  afx_msg void OnPaint();

protected:
	DECLARE_MESSAGE_MAP()
};

#endif //__NewDialogBar_H_