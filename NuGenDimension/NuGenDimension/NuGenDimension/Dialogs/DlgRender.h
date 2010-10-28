#pragma once

#include "..//OpenGLView//3dCamera.h"
#include "afxwin.h"
#include "afxcmn.h"
#include "..//Controls//ColorPickerCB.h"

// finestra di dialogo CDlgRender

class CDlgRender : public CDialog
{
	DECLARE_DYNAMIC(CDlgRender)

public:
	C3dCamera*  m_camera;
	CString m_csPathBmp;

	CDC   *m_FrameDC;
	CDlgRender(C3dCamera*  cam, CWnd* pParent = NULL);   // costruttore standard
	virtual ~CDlgRender();

	enum { IDD = IDD_RTRENDER };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // Supporto DDX/DDV

	DECLARE_MESSAGE_MAP()
	afx_msg void OnDestroy();
	afx_msg void OnSize(UINT nType,int cx,int cy);
	afx_msg void OnBnClickedStartRender();
	afx_msg void OnBnClickedStopRender();
	
public:
	BOOL m_bAntiAlias;
	CStatic m_Preview;
	int m_iSize;
public:
	virtual BOOL OnInitDialog();

	CColorPickerCB m_cmbColorBackground;
public:
	int m_iColor;
};
