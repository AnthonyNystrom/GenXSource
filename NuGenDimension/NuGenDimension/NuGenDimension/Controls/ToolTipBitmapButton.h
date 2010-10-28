#if !defined(AFX_TOOLTIPRESSOURCEBUTTON_H__663049D7_9D25_11D5_8F75_0048546F01E7__INCLUDED_)
#define AFX_TOOLTIPRESSOURCEBUTTON_H__663049D7_9D25_11D5_8F75_0048546F01E7__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// ToolTipBitmapButton.h : header file
//

/////////////////////////////////////////////////////////////////////////////
// CToolTipBitmapButton window

#include "ToolTipButton.h"

#define baseCToolTipBitmapButton CToolTipButton
// if you don't want tooltip, you can put
// #define baseCToolTipBitmapButton CBitmapButton
// or
// #define baseCToolTipBitmapButton CButton


// CToolTipRessourceButton by Jean-Louis GUENEGO
// Thanks to Niek Albers.
// A cool CBitmapButton derived class with 4 states.
class CToolTipBitmapButton : public baseCToolTipBitmapButton
{
// Construction
public:
	CToolTipBitmapButton();

// Attributes
public:
	CBitmap m_Bitmap; // bitmap containing the 4 images (up, down, focused, disabled)
	CSize m_ButtonSize; // width and height of the button
	BOOL m_bHover; // indicates if mouse is over the button (it is like focused on a CBitmapButton)
	BOOL m_bTracking; // true when the mouse just arrives on the button, or leaves the button, false other
private:
	bool m_pressed;

	bool m_mouse_hover_style;
public:
	void SetPressed(bool nPr) {
		m_pressed=nPr;Invalidate();}
	bool IsPressed() {return m_pressed;}

	void SetMouseHoverStyle(bool nMH=true){
		 m_mouse_hover_style=nMH;}

// Operations
public:
	BOOL LoadBitmap(HANDLE hbitmap);

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CToolTipRessourceButton)
	public:
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	virtual void DrawItem(LPDRAWITEMSTRUCT lpDrawItemStruct);
	//}}AFX_VIRTUAL

// Implementation
public:
	virtual ~CToolTipBitmapButton();

	// Generated message map functions
protected:
	//{{AFX_MSG(CToolTipBitmapButton)
	afx_msg void OnMouseMove(UINT nFlags, CPoint point);
	afx_msg LRESULT OnMouseLeave(WPARAM wparam, LPARAM lparam);
	afx_msg LRESULT OnMouseHover(WPARAM wparam, LPARAM lparam) ;
	afx_msg void OnKeyUp(UINT nChar, UINT nRepCnt, UINT nFlags);
	//}}AFX_MSG

	DECLARE_MESSAGE_MAP()
public:
};

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TOOLTIPBitmapBUTTON_H__663049D7_9D25_11D5_8F75_0048546F01E7__INCLUDED_)
