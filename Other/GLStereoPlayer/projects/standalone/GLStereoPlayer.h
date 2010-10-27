//-----------------------------------------------------------------------------
// GLStereoPlayer.h : Main header file for the GLStereoPlayer application
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#if !defined(AFX_GLSTEREOPLAYER_H__F9AF75BA_81D0_43AC_B57F_BF12F4E5ACE2__INCLUDED_)
#define AFX_GLSTEREOPLAYER_H__F9AF75BA_81D0_43AC_B57F_BF12F4E5ACE2__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#ifndef __AFXWIN_H__
    #error include 'stdafx.h' before including this file for PCH
#endif

#include "resource.h"       // main symbols

#include "StereoPlayer.h"
#include "SlideShow.h"

/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerApp:
// See GLStereoPlayer.cpp for the implementation of this class
//

class CGLStereoPlayerApp : public CWinApp
{
public:
    CGLStereoPlayerApp();

    inline glsp::StereoPlayer* GetPlayer() { return m_stereoPlayer; }   // Get StereoPlayer instance.
    inline glsp::SlideShow* GetSlideShow() { return m_slideShow; }   // Get SlideShow instance.

// Overrides
    // ClassWizard generated virtual function overrides
    //{{AFX_VIRTUAL(CGLStereoPlayerApp)
    public:
    virtual BOOL InitInstance();
    virtual int ExitInstance();
    //}}AFX_VIRTUAL

// Implementation
    //{{AFX_MSG(CGLStereoPlayerApp)
    afx_msg void OnAppAbout();
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()

private:
    HINSTANCE m_hLangDLL;

    glsp::StereoPlayer* m_stereoPlayer;   // StereoPlayer class instance
    glsp::SlideShow*    m_slideShow;      // SlideShow class instance
};


/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_GLSTEREOPLAYER_H__F9AF75BA_81D0_43AC_B57F_BF12F4E5ACE2__INCLUDED_)
