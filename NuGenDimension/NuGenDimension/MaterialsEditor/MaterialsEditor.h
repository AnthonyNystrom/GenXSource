// MaterialsEditor.h : main header file for the MaterialsEditor application
//
#pragma once

#ifndef __AFXWIN_H__
	#error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols


// CMaterialsEditorApp:
// See MaterialsEditor.cpp for the implementation of this class
//

class CMaterialsEditorApp : public CWinApp
{
public:
	CMaterialsEditorApp();


// Overrides
public:
	virtual BOOL InitInstance();

// Implementation
	afx_msg void OnAppAbout();
	DECLARE_MESSAGE_MAP()
};

extern CMaterialsEditorApp theApp;