//-----------------------------------------------------------------------------
// GLStereoPlayerView.h : Interface of the CGLStereoPlayerView class
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#if !defined(AFX_GLSTEREOPLAYERVIEW_H__74F753AC_0CED_4A15_A02A_2BC1B5368B5D__INCLUDED_)
#define AFX_GLSTEREOPLAYERVIEW_H__74F753AC_0CED_4A15_A02A_2BC1B5368B5D__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#include "StereoPlayer.h"
#include "SlideShow.h"

class CGLStereoPlayerView : public CView
{
protected: // create from serialization only
    CGLStereoPlayerView();
    DECLARE_DYNCREATE(CGLStereoPlayerView)

// Attributes
public:
    CGLStereoPlayerDoc* GetDocument();

// Operations
public:

// Overrides
    // ClassWizard generated virtual function overrides
    //{{AFX_VIRTUAL(CGLStereoPlayerView)
    public:
    virtual void OnDraw(CDC* pDC);  // overridden to draw this view
    virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
    virtual BOOL PreTranslateMessage(MSG* pMsg);
    protected:
    //}}AFX_VIRTUAL

// Implementation
public:
    virtual ~CGLStereoPlayerView();
#ifdef _DEBUG
    virtual void AssertValid() const;
    virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
    //{{AFX_MSG(CGLStereoPlayerView)
    afx_msg int OnCreate(LPCREATESTRUCT lpCreateStruct);
    afx_msg void OnDestroy();
    afx_msg BOOL OnEraseBkgnd(CDC* pDC);
    afx_msg void OnSize(UINT nType, int cx, int cy);
    afx_msg void OnTimer(UINT nIDEvent);
    afx_msg void OnMouseMove(UINT nFlags, CPoint point);
    afx_msg void OnMButtonDown(UINT nFlags, CPoint point);
    afx_msg BOOL OnMouseWheel(UINT nFlags, short zDelta, CPoint pt);
    afx_msg void OnContextMenu(CWnd* pWnd, CPoint point);
    afx_msg void OnLButtonDown(UINT nFlags, CPoint point);
    afx_msg void OnLButtonUp(UINT nFlags, CPoint point);
    afx_msg void OnDropFiles(HDROP hDropInfo);
    afx_msg void OnSetFocus(CWnd* pOldWnd);
    afx_msg void OnKillFocus(CWnd* pNewWnd);
    afx_msg void OnFileOpenLeft();
    afx_msg void OnFileOpenRight();
    afx_msg void OnFileOpenSetting();
    afx_msg void OnFileSaveSetting();
    afx_msg void OnViewFormatSeparated();
    afx_msg void OnUpdateViewFormatSeparated(CCmdUI* pCmdUI);
    afx_msg void OnViewFormatHorizontal();
    afx_msg void OnUpdateViewFormatHorizontal(CCmdUI* pCmdUI);
    afx_msg void OnViewFormatHorizontalComp();
    afx_msg void OnUpdateViewFormatHorizontalComp(CCmdUI* pCmdUI);
    afx_msg void OnViewFormatVertical();
    afx_msg void OnUpdateViewFormatVertical(CCmdUI* pCmdUI);
    afx_msg void OnViewFormatVerticalComp();
    afx_msg void OnUpdateViewFormatVerticalComp(CCmdUI* pCmdUI);
    afx_msg void OnViewChangeFormat();
    afx_msg void OnViewStereoLeft();
    afx_msg void OnUpdateViewStereoLeft(CCmdUI* pCmdUI);
    afx_msg void OnViewStereoRight();
    afx_msg void OnUpdateViewStereoRight(CCmdUI* pCmdUI);
    afx_msg void OnViewStereoAnagryph();
    afx_msg void OnUpdateViewStereoAnagryph(CCmdUI* pCmdUI);
    afx_msg void OnViewStereoHorizontal();
    afx_msg void OnUpdateViewStereoHorizontal(CCmdUI* pCmdUI);
    afx_msg void OnViewStereoVertical();
    afx_msg void OnUpdateViewStereoVertical(CCmdUI* pCmdUI);
    afx_msg void OnViewStereoHorizontalInterleaved();
    afx_msg void OnUpdateViewStereoHorizontalInterleaved(CCmdUI* pCmdUI);
    afx_msg void OnViewStereoVerticalInterleaved();
    afx_msg void OnUpdateViewStereoVerticalInterleaved(CCmdUI* pCmdUI);
    afx_msg void OnViewStereoSharp3D();
    afx_msg void OnUpdateViewStereoSharp3D(CCmdUI* pCmdUI);
    afx_msg void OnViewStereoQuadBuffer();
    afx_msg void OnUpdateViewStereoQuadbuffer(CCmdUI* pCmdUI);
    afx_msg void OnViewChangeStereoType();
    afx_msg void OnViewLeftColor();
    afx_msg void OnViewRightColor();
    afx_msg void OnViewSwap();
    afx_msg void OnUpdateViewSwap(CCmdUI* pCmdUI);
    afx_msg void OnViewKeepAspectRatio();
    afx_msg void OnUpdateViewKeepAspectRatio(CCmdUI* pCmdUI);
    afx_msg void OnViewOffsetIncrease();
    afx_msg void OnViewOffsetDecrease();
    afx_msg void OnViewOffsetReset();
    afx_msg void OnViewPanLeft();
    afx_msg void OnViewPanRight();
    afx_msg void OnViewPanUp();
    afx_msg void OnViewPanDown();
    afx_msg void OnViewZoomUp();
    afx_msg void OnViewZoomDown();
    afx_msg void OnViewPanZoomReset();
    afx_msg void OnViewStatistics();
    afx_msg void OnUpdateViewStatistics(CCmdUI* pCmdUI);
    afx_msg void OnViewPlayControl();
    afx_msg void OnViewPlayControlShow();
    afx_msg void OnUpdateViewPlayControlShow(CCmdUI* pCmdUI);
    afx_msg void OnViewPlayControlAuto();
    afx_msg void OnUpdateViewPlayControlAuto(CCmdUI* pCmdUI);
    afx_msg void OnViewPlayControlHide();
    afx_msg void OnUpdateViewPlayControlHide(CCmdUI* pCmdUI);
    afx_msg void OnPlayPlayPause();
    afx_msg void OnPlayStop();
    afx_msg void OnPlayPlayOnLoad();
    afx_msg void OnUpdatePlayPlayOnLoad(CCmdUI* pCmdUI);
    afx_msg void OnPlaySpeed25();
    afx_msg void OnUpdatePlaySpeed25(CCmdUI* pCmdUI);
    afx_msg void OnPlaySpeed50();
    afx_msg void OnUpdatePlaySpeed50(CCmdUI* pCmdUI);
    afx_msg void OnPlaySpeed100();
    afx_msg void OnUpdatePlaySpeed100(CCmdUI* pCmdUI);
    afx_msg void OnPlaySpeed150();
    afx_msg void OnUpdatePlaySpeed150(CCmdUI* pCmdUI);
    afx_msg void OnPlaySpeed200();
    afx_msg void OnUpdatePlaySpeed200(CCmdUI* pCmdUI);
    afx_msg void OnPlayForceSync();
    afx_msg void OnUpdatePlayForceSync(CCmdUI* pCmdUI);
    afx_msg void OnPlayLoop();
    afx_msg void OnUpdatePlayLoop(CCmdUI* pCmdUI);
    afx_msg void OnPlayVolumeUp();
    afx_msg void OnPlayVolumeDown();
    afx_msg void OnPlayVolumeMute();
    afx_msg void OnSlideFirst();
    afx_msg void OnSlideLast();
    afx_msg void OnSlidePrev();
    afx_msg void OnSlideNext();
    afx_msg void OnSlideFade();
    afx_msg void OnUpdateSlideFade(CCmdUI* pCmdUI);
    //}}AFX_MSG
    DECLARE_MESSAGE_MAP()

public:

    void ShowError(long errorID);       // Show an error message specified by string resource ID.

    BOOL InitializeOpenGL(CWnd* pWnd);  // Create an OpenGL context.
    BOOL SetupPixelFormat();            // Select a Pixel Format which we need.
    BOOL TerminateOpenGL();             // Destroy the OpenGL context.
    void SwapBuffers();                 // Swap the OpenGL double buffers.
    void BeginGLDraw();                 // Activate the OpenGL context.
    void EndGLDraw();                   // Deactivate the OpenGL context.

    BOOL OpenFile(LPCTSTR fileName);    // Open a file depends on its extension.
    BOOL SaveFile(LPCTSTR fileName);    // Save the setting as the specified file.
    BOOL OpenLeftFile(LPCTSTR fileName);    // Open a left side source file.
    BOOL OpenRightFile(LPCTSTR fileName);   // Open a right side source file.
    BOOL OpenSetting(LPCTSTR fileName);     // Open a setting file.
    BOOL SaveSetting(LPCTSTR fileName);     // Save the setting file.

    void FireNeedsResize() {
        // Resize the main frame to fit to the source
        if (AfxGetMainWnd())
            AfxGetMainWnd()->SendMessage(WM_COMMAND, ID_VIEW_RESIZE_ORIGINAL, 0);
    }
    void SetModifiedFlag(BOOL flag=TRUE) {
        // For compatibility of SetModifiedFlag() function between ActiveX and Doc/View application
        GetDocument()->SetModifiedFlag(flag);
    }

private:

    CDC*  m_pDC;    // Display Context of the control
    HGLRC m_hRC;    // Rendering Context of the control

    glsp::StereoPlayer* m_stereoPlayer;   // StereoPlayer class instance
    glsp::SlideShow*    m_slideShow;      // SlideShow class instance

    int   m_lastMouseX; // The last mouse position
    int   m_lastMouseY; // The last mouse position
    float m_lastPivotX; // Middle mouse drag pivot point
    float m_lastPivotY; // Middle mouse drag pivot point

    BOOL m_lastIMEStatus;           // IME status before the control got the focus

    CMenu  m_contextMenu;   // Context menu resource handle
    HACCEL m_accel;         // Accelerator resource handle
};

#ifndef _DEBUG  // debug version in mlgreenView.cpp
inline CGLStereoPlayerDoc* CGLStereoPlayerView::GetDocument()
   { return (CGLStereoPlayerDoc*)m_pDocument; }
#endif

/////////////////////////////////////////////////////////////////////////////

//{{AFX_INSERT_LOCATION}}
// Microsoft Developer Studio will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_GLSTEREOPLAYERVIEW_H__74F753AC_0CED_4A15_A02A_2BC1B5368B5D__INCLUDED_)
