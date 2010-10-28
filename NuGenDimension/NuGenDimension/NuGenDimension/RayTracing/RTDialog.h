#if !defined(AFX_INPUTBOX_H__0BE6B01B_C74A_45FE_AF35_D6E8E4B65A1B__INCLUDED_)
#define AFX_INPUTBOX_H__0BE6B01B_C74A_45FE_AF35_D6E8E4B65A1B__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "..//OpenGLView//3dCamera.h"

class CRTDialog  
{
    static HFONT m_hFont;
    static HWND  m_hWndDialog;
    static HWND  m_hWndParent;
    static HWND  m_hWndStart;
    static HWND  m_hWndStop;
    static HWND  m_hWndFrame;
	static HDC   m_FrameDC;

    static HINSTANCE m_hInst;

	static C3dCamera*  m_camera;
	
    static LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam);
	static void OnSize(int cx, int cy);
	static void OnDestroy();
	static void OnStart();
	static void OnStop();
public: 
	
	CRTDialog(HWND hWndParent, C3dCamera*  cam);
	virtual ~CRTDialog();

	BOOL DoModal();

};

#endif // !defined(AFX_INPUTBOX_H__0BE6B01B_C74A_45FE_AF35_D6E8E4B65A1B__INCLUDED_)
