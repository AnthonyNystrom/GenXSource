//-----------------------------------------------------------------------------
// GLStereoPlayerCtl.h : Declaration of the CGLStereoPlayerCtrl OLE control class.
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------
// CGLStereoPlayerCtrl : See GLStereoPlayerCtl.cpp for implementation.

#include "StereoPlayer.h"
#include "SlideShow.h"

class CGLStereoPlayerCtrl : public COleControl
{
    DECLARE_DYNCREATE(CGLStereoPlayerCtrl)

// Constructor
public:
    CGLStereoPlayerCtrl();

// Overrides
    //{{AFX_VIRTUAL(CGLStereoPlayerCtrl)
    public:
    virtual void OnDraw(CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid);
    virtual void DoPropExchange(CPropExchange* pPX);
    virtual void OnResetState();
    virtual DWORD GetControlFlags();
    virtual BOOL PreTranslateMessage(MSG* pMsg);
    virtual void OnHideToolBars();
    protected:
    virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
    virtual void OnDrawMetafile(CDC* pDC, const CRect& rcBounds);
    //}}AFX_VIRTUAL

// Implementation
protected:
    ~CGLStereoPlayerCtrl();

    DECLARE_OLECREATE_EX(CGLStereoPlayerCtrl)   // Class factory and guid
    DECLARE_OLETYPELIB(CGLStereoPlayerCtrl)     // GetTypeInfo
//  DECLARE_PROPPAGEIDS(CGLStereoPlayerCtrl)    // Property page IDs
    DECLARE_OLECTLTYPE(CGLStereoPlayerCtrl)     // Type name and misc status

// Message maps
    //{{AFX_MSG(CGLStereoPlayerCtrl)
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

// Dispatch maps
    //{{AFX_DISPATCH(CGLStereoPlayerCtrl)
    afx_msg BSTR GetLeftFile();
    afx_msg void SetLeftFile(LPCTSTR lpszNewValue);
    afx_msg BSTR GetRightFile();
    afx_msg void SetRightFile(LPCTSTR lpszNewValue);
    afx_msg long GetFormat();
    afx_msg void SetFormat(long nNewValue);
    afx_msg long GetStereoType();
    afx_msg void SetStereoType(long nNewValue);
    afx_msg OLE_COLOR GetLeftColor();
    afx_msg void SetLeftColor(OLE_COLOR nNewValue);
    afx_msg OLE_COLOR GetRightColor();
    afx_msg void SetRightColor(OLE_COLOR nNewValue);
    afx_msg BOOL GetSwap();
    afx_msg void SetSwap(BOOL bNewValue);
    afx_msg float GetOffset();
    afx_msg void SetOffset(float newValue);
    afx_msg float GetPanX();
    afx_msg void SetPanX(float newValue);
    afx_msg float GetPanY();
    afx_msg void SetPanY(float newValue);
    afx_msg float GetZoom();
    afx_msg void SetZoom(float newValue);
    afx_msg BOOL GetStatistics();
    afx_msg void SetStatistics(BOOL bNewValue);
    afx_msg long GetPlayControl();
    afx_msg void SetPlayControl(long nNewValue);
    afx_msg float GetSpeed();
    afx_msg void SetSpeed(float newValue);
    afx_msg BOOL GetForceSync();
    afx_msg void SetForceSync(BOOL bNewValue);
    afx_msg BOOL GetLoop();
    afx_msg void SetLoop(BOOL bNewValue);
    afx_msg long GetVolume();
    afx_msg void SetVolume(long nNewValue);
    afx_msg long GetSourceWidth();
    afx_msg long GetSourceHeight();
    afx_msg float GetSourceDuration();
    afx_msg long GetClientWidth();
    afx_msg long GetClientHeight();
    afx_msg BOOL GetIsPlaying();
    afx_msg BOOL GetPlayOnLoad();
    afx_msg void SetPlayOnLoad(BOOL bNewValue);
    afx_msg LPDISPATCH GetStatisticsFont();
    afx_msg void SetStatisticsFont(LPDISPATCH newValue);
    afx_msg OLE_COLOR GetStatisticsColor();
    afx_msg void SetStatisticsColor(OLE_COLOR nNewValue);
    afx_msg OLE_COLOR GetBaseColor();
    afx_msg void SetBaseColor(OLE_COLOR nNewValue);
    afx_msg BOOL OpenLeftFile(LPCTSTR fileName);
    afx_msg BOOL OpenRightFile(LPCTSTR fileName);
    afx_msg BOOL OpenSetting(LPCTSTR fileName);
    afx_msg BOOL SaveSetting(LPCTSTR fileName);
    afx_msg void Play();
    afx_msg void Pause();
    afx_msg void Stop();
    afx_msg void SetPosition(float position);
    afx_msg float GetPosition();
    //}}AFX_DISPATCH
    DECLARE_DISPATCH_MAP()

    afx_msg void AboutBox();

// Event maps
    //{{AFX_EVENT(CGLStereoPlayerCtrl)
    void FireNeedsResize()
        {FireEvent(eventidNeedsResize,EVENT_PARAM(VTS_NONE));}
    //}}AFX_EVENT
    DECLARE_EVENT_MAP()

// Dispatch and event IDs
public:
    enum {
    //{{AFX_DISP_ID(CGLStereoPlayerCtrl)
    dispidLeftFile = 1L,
    dispidRightFile = 2L,
    dispidFormat = 3L,
    dispidStereoType = 4L,
    dispidLeftColor = 5L,
    dispidRightColor = 6L,
    dispidSwap = 7L,
    dispidOffset = 8L,
    dispidPanX = 9L,
    dispidPanY = 10L,
    dispidZoom = 11L,
    dispidStatistics = 12L,
    dispidPlayControl = 13L,
    dispidSpeed = 14L,
    dispidForceSync = 15L,
    dispidLoop = 16L,
    dispidVolume = 17L,
    dispidSourceWidth = 18L,
    dispidSourceHeight = 19L,
    dispidSourceDuration = 20L,
    dispidClientWidth = 21L,
    dispidClientHeight = 22L,
    dispidIsPlaying = 23L,
    dispidPlayOnLoad = 24L,
    dispidStatisticsFont = 25L,
    dispidStatisticsColor = 26L,
    dispidBaseColor = 27L,
    dispidOpenLeftFile = 28L,
    dispidOpenRightFile = 29L,
    dispidOpenSetting = 30L,
    dispidSaveSetting = 31L,
    dispidPlay = 32L,
    dispidPause = 33L,
    dispidStop = 34L,
    dispidSetPosition = 35L,
    dispidGetPosition = 36L,
    eventidNeedsResize = 1L,
    //}}AFX_DISP_ID
    };

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
    CFontHolder m_statisticsFont;   // Font of the statistics

    CMenu  m_contextMenu;   // Context menu resource handle
    HACCEL m_accel;         // Accelerator resource handle
};
