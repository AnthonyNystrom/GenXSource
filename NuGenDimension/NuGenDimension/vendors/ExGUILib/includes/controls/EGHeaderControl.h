#pragma once

/*
**	—труктурка дл€ быстрого отображени€ заголовков
*/
typedef struct HEADEREXSHOWDATA
{
	HEADEREXSHOWDATA*	next;
	int		width;
	int		nIndex;
	TCHAR	szName[500];
} *LPHEADEREXSHOWDATA;

/*
**  class CHeaderControlEx
*/
class CEGHeaderControl : public CHeaderCtrl
{
	DECLARE_DYNAMIC(CEGHeaderControl)
	WNDPROC m_oldWndProc;

	int GetLineHeight();
	int m_nPressed;

	DWORD OffsetColor(DWORD color,BYTE offset,BOOL dir);
	int HeaderExTitleHeight();
	BOOL DrawFrameControlEx(HDC hdc, LPRECT lprc, UINT uType, UINT uState);
	BOOL HeaderExVCenter(HDC dc, TCHAR* out, RECT* r, BOOL bPressed);
	void HeaderEx_SetTit(HDC dc, RECT **r, TCHAR** out, LPHEADEREXSHOWDATA* l, int pos, int maxstr);
	void HeaderEx_Title();

public:
	CEGHeaderControl();
	virtual ~CEGHeaderControl();

protected:
	DECLARE_MESSAGE_MAP()

	afx_msg LRESULT OnHDMLayout(WPARAM wParam, LPARAM lParam);
	afx_msg LRESULT OnHDMHitTest(WPARAM wParam, LPARAM lParam);
	afx_msg LRESULT OnWMPaintEx(WPARAM wParam, LPARAM lParam);
	afx_msg LRESULT OnLButtonDown(WPARAM wParam, LPARAM lParam);
	afx_msg LRESULT OnLButtonUp(WPARAM wParam, LPARAM lParam);
	virtual void PreSubclassWindow();
};
