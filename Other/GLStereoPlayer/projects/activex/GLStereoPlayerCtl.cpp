//-----------------------------------------------------------------------------
// GLStereoPlayerCtl.cpp : Implementation of the CGLStereoPlayerCtrl OLE control class.
//
// Copyright (c) 2005 Toshiyuki Takahei All rights reserved.
//
//-----------------------------------------------------------------------------

#include "stdafx.h"
#include "GLStereoPlayer.h"
#include "StereoPlayerXML.h"
#include "GLStereoPlayerCtl.h"

#include <direct.h>

#include <imm.h>
#pragma comment(lib,"imm32.lib")

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#define MODULE_NAME "GLSPCtrl.ocx"

using namespace glsp;

IMPLEMENT_DYNCREATE(CGLStereoPlayerCtrl, COleControl)


/////////////////////////////////////////////////////////////////////////////
// Message map

BEGIN_MESSAGE_MAP(CGLStereoPlayerCtrl, COleControl)
    //{{AFX_MSG_MAP(CGLStereoPlayerCtrl)
    ON_WM_CREATE()
    ON_WM_DESTROY()
    ON_WM_ERASEBKGND()
    ON_WM_SIZE()
    ON_WM_TIMER()
    ON_WM_MOUSEMOVE()
    ON_WM_MBUTTONDOWN()
    ON_WM_MOUSEWHEEL()
    ON_WM_CONTEXTMENU()
    ON_WM_LBUTTONDOWN()
    ON_WM_LBUTTONUP()
    ON_WM_DROPFILES()
    ON_WM_SETFOCUS()
    ON_WM_KILLFOCUS()
    ON_COMMAND(ID_FILE_OPENLEFT, OnFileOpenLeft)
    ON_COMMAND(ID_FILE_OPENRIGHT, OnFileOpenRight)
    ON_COMMAND(ID_FILE_OPENSETTING, OnFileOpenSetting)
    ON_COMMAND(ID_FILE_SAVESETTING, OnFileSaveSetting)
    ON_COMMAND(ID_VIEW_FORMAT_SEPARATED, OnViewFormatSeparated)
    ON_UPDATE_COMMAND_UI(ID_VIEW_FORMAT_SEPARATED, OnUpdateViewFormatSeparated)
    ON_COMMAND(ID_VIEW_FORMAT_HORIZONTAL, OnViewFormatHorizontal)
    ON_UPDATE_COMMAND_UI(ID_VIEW_FORMAT_HORIZONTAL, OnUpdateViewFormatHorizontal)
    ON_COMMAND(ID_VIEW_FORMAT_HORIZONTAL_COMP, OnViewFormatHorizontalComp)
    ON_UPDATE_COMMAND_UI(ID_VIEW_FORMAT_HORIZONTAL_COMP, OnUpdateViewFormatHorizontalComp)
    ON_COMMAND(ID_VIEW_FORMAT_VERTICAL, OnViewFormatVertical)
    ON_UPDATE_COMMAND_UI(ID_VIEW_FORMAT_VERTICAL, OnUpdateViewFormatVertical)
    ON_COMMAND(ID_VIEW_FORMAT_VERTICAL_COMP, OnViewFormatVerticalComp)
    ON_UPDATE_COMMAND_UI(ID_VIEW_FORMAT_VERTICAL_COMP, OnUpdateViewFormatVerticalComp)
    ON_COMMAND(ID_VIEW_CHANGE_FORMAT, OnViewChangeFormat)
    ON_COMMAND(ID_VIEW_STEREO_LEFT, OnViewStereoLeft)
    ON_UPDATE_COMMAND_UI(ID_VIEW_STEREO_LEFT, OnUpdateViewStereoLeft)
    ON_COMMAND(ID_VIEW_STEREO_RIGHT, OnViewStereoRight)
    ON_UPDATE_COMMAND_UI(ID_VIEW_STEREO_RIGHT, OnUpdateViewStereoRight)
    ON_COMMAND(ID_VIEW_STEREO_ANAGRYPH, OnViewStereoAnagryph)
    ON_UPDATE_COMMAND_UI(ID_VIEW_STEREO_ANAGRYPH, OnUpdateViewStereoAnagryph)
    ON_COMMAND(ID_VIEW_STEREO_HORIZONTAL, OnViewStereoHorizontal)
    ON_UPDATE_COMMAND_UI(ID_VIEW_STEREO_HORIZONTAL, OnUpdateViewStereoHorizontal)
    ON_COMMAND(ID_VIEW_STEREO_VERTICAL, OnViewStereoVertical)
    ON_UPDATE_COMMAND_UI(ID_VIEW_STEREO_VERTICAL, OnUpdateViewStereoVertical)
    ON_COMMAND(ID_VIEW_STEREO_HORIZONTAL_INTERLEAVED, OnViewStereoHorizontalInterleaved)
    ON_UPDATE_COMMAND_UI(ID_VIEW_STEREO_HORIZONTAL_INTERLEAVED, OnUpdateViewStereoHorizontalInterleaved)
    ON_COMMAND(ID_VIEW_STEREO_VERTICAL_INTERLEAVED, OnViewStereoVerticalInterleaved)
    ON_UPDATE_COMMAND_UI(ID_VIEW_STEREO_VERTICAL_INTERLEAVED, OnUpdateViewStereoVerticalInterleaved)
    ON_COMMAND(ID_VIEW_STEREO_SHARP3D, OnViewStereoSharp3D)
    ON_UPDATE_COMMAND_UI(ID_VIEW_STEREO_SHARP3D, OnUpdateViewStereoSharp3D)
    ON_COMMAND(ID_VIEW_STEREO_QUADBUFFER, OnViewStereoQuadBuffer)
    ON_UPDATE_COMMAND_UI(ID_VIEW_STEREO_QUADBUFFER, OnUpdateViewStereoQuadbuffer)
    ON_COMMAND(ID_VIEW_CHANGE_STEREOTYPE, OnViewChangeStereoType)
    ON_COMMAND(ID_VIEW_LEFT_COLOR, OnViewLeftColor)
    ON_COMMAND(ID_VIEW_RIGHT_COLOR, OnViewRightColor)
    ON_COMMAND(ID_VIEW_SWAP, OnViewSwap)
    ON_UPDATE_COMMAND_UI(ID_VIEW_SWAP, OnUpdateViewSwap)
    ON_COMMAND(ID_VIEW_KEEPASPECTRATIO, OnViewKeepAspectRatio)
    ON_UPDATE_COMMAND_UI(ID_VIEW_KEEPASPECTRATIO, OnUpdateViewKeepAspectRatio)
    ON_COMMAND(ID_VIEW_OFFSET_INCREASE, OnViewOffsetIncrease)
    ON_COMMAND(ID_VIEW_OFFSET_DECREASE, OnViewOffsetDecrease)
    ON_COMMAND(ID_VIEW_OFFSET_RESET, OnViewOffsetReset)
    ON_COMMAND(ID_VIEW_PAN_LEFT, OnViewPanLeft)
    ON_COMMAND(ID_VIEW_PAN_RIGHT, OnViewPanRight)
    ON_COMMAND(ID_VIEW_PAN_UP, OnViewPanUp)
    ON_COMMAND(ID_VIEW_PAN_DOWN, OnViewPanDown)
    ON_COMMAND(ID_VIEW_ZOOM_UP, OnViewZoomUp)
    ON_COMMAND(ID_VIEW_ZOOM_DOWN, OnViewZoomDown)
    ON_COMMAND(ID_VIEW_PANZOOM_RESET, OnViewPanZoomReset)
    ON_COMMAND(ID_VIEW_PLAYCONTROL, OnViewPlayControl)
    ON_COMMAND(ID_VIEW_PLAYCONTROL_SHOW, OnViewPlayControlShow)
    ON_UPDATE_COMMAND_UI(ID_VIEW_PLAYCONTROL_SHOW, OnUpdateViewPlayControlShow)
    ON_COMMAND(ID_VIEW_PLAYCONTROL_AUTO, OnViewPlayControlAuto)
    ON_UPDATE_COMMAND_UI(ID_VIEW_PLAYCONTROL_AUTO, OnUpdateViewPlayControlAuto)
    ON_COMMAND(ID_VIEW_PLAYCONTROL_HIDE, OnViewPlayControlHide)
    ON_UPDATE_COMMAND_UI(ID_VIEW_PLAYCONTROL_HIDE, OnUpdateViewPlayControlHide)
    ON_COMMAND(ID_VIEW_STATISTICS, OnViewStatistics)
    ON_UPDATE_COMMAND_UI(ID_VIEW_STATISTICS, OnUpdateViewStatistics)
    ON_COMMAND(ID_PLAY_PLAYPAUSE, OnPlayPlayPause)
    ON_COMMAND(ID_PLAY_STOP, OnPlayStop)
    ON_COMMAND(ID_PLAY_PLAYONLOAD, OnPlayPlayOnLoad)
    ON_UPDATE_COMMAND_UI(ID_PLAY_PLAYONLOAD, OnUpdatePlayPlayOnLoad)
    ON_COMMAND(ID_PLAY_SPEED_25, OnPlaySpeed25)
    ON_UPDATE_COMMAND_UI(ID_PLAY_SPEED_25, OnUpdatePlaySpeed25)
    ON_COMMAND(ID_PLAY_SPEED_50, OnPlaySpeed50)
    ON_UPDATE_COMMAND_UI(ID_PLAY_SPEED_50, OnUpdatePlaySpeed50)
    ON_COMMAND(ID_PLAY_SPEED_100, OnPlaySpeed100)
    ON_UPDATE_COMMAND_UI(ID_PLAY_SPEED_100, OnUpdatePlaySpeed100)
    ON_COMMAND(ID_PLAY_SPEED_150, OnPlaySpeed150)
    ON_UPDATE_COMMAND_UI(ID_PLAY_SPEED_150, OnUpdatePlaySpeed150)
    ON_COMMAND(ID_PLAY_SPEED_200, OnPlaySpeed200)
    ON_UPDATE_COMMAND_UI(ID_PLAY_SPEED_200, OnUpdatePlaySpeed200)
    ON_COMMAND(ID_PLAY_FORCESYNC, OnPlayForceSync)
    ON_UPDATE_COMMAND_UI(ID_PLAY_FORCESYNC, OnUpdatePlayForceSync)
    ON_COMMAND(ID_PLAY_LOOP, OnPlayLoop)
    ON_UPDATE_COMMAND_UI(ID_PLAY_LOOP, OnUpdatePlayLoop)
    ON_COMMAND(ID_PLAY_VOLUME_UP, OnPlayVolumeUp)
    ON_COMMAND(ID_PLAY_VOLUME_DOWN, OnPlayVolumeDown)
    ON_COMMAND(ID_PLAY_VOLUME_MUTE, OnPlayVolumeMute)
    ON_COMMAND(ID_SLIDE_FIRST, OnSlideFirst)
    ON_COMMAND(ID_SLIDE_LAST, OnSlideLast)
    ON_COMMAND(ID_SLIDE_PREV, OnSlidePrev)
    ON_COMMAND(ID_SLIDE_NEXT, OnSlideNext)
    ON_COMMAND(ID_SLIDE_FADE, OnSlideFade)
    ON_UPDATE_COMMAND_UI(ID_SLIDE_FADE, OnUpdateSlideFade)
    //}}AFX_MSG_MAP
    ON_OLEVERB(IDS_ACTIVATE, OnEdit)
//  ON_OLEVERB(AFX_IDS_VERB_EDIT, OnEdit)
//  ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
END_MESSAGE_MAP()


/////////////////////////////////////////////////////////////////////////////
// Dispatch map

BEGIN_DISPATCH_MAP(CGLStereoPlayerCtrl, COleControl)
    //{{AFX_DISPATCH_MAP(CGLStereoPlayerCtrl)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "LeftFile", GetLeftFile, SetLeftFile, VT_BSTR)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "RightFile", GetRightFile, SetRightFile, VT_BSTR)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "Format", GetFormat, SetFormat, VT_I4)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "StereoType", GetStereoType, SetStereoType, VT_I4)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "LeftColor", GetLeftColor, SetLeftColor, VT_COLOR)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "RightColor", GetRightColor, SetRightColor, VT_COLOR)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "Swap", GetSwap, SetSwap, VT_BOOL)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "Offset", GetOffset, SetOffset, VT_R4)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "PanX", GetPanX, SetPanX, VT_R4)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "PanY", GetPanY, SetPanY, VT_R4)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "Zoom", GetZoom, SetZoom, VT_R4)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "Statistics", GetStatistics, SetStatistics, VT_BOOL)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "PlayControl", GetPlayControl, SetPlayControl, VT_I4)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "Speed", GetSpeed, SetSpeed, VT_R4)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "ForceSync", GetForceSync, SetForceSync, VT_BOOL)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "Loop", GetLoop, SetLoop, VT_BOOL)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "Volume", GetVolume, SetVolume, VT_I4)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "SourceWidth", GetSourceWidth, SetNotSupported, VT_I4)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "SourceHeight", GetSourceHeight, SetNotSupported, VT_I4)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "SourceDuration", GetSourceDuration, SetNotSupported, VT_R4)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "ClientWidth", GetClientWidth, SetNotSupported, VT_I4)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "ClientHeight", GetClientHeight, SetNotSupported, VT_I4)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "IsPlaying", GetIsPlaying, SetNotSupported, VT_BOOL)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "PlayOnLoad", GetPlayOnLoad, SetPlayOnLoad, VT_BOOL)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "StatisticsFont", GetStatisticsFont, SetStatisticsFont, VT_DISPATCH)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "StatisticsColor", GetStatisticsColor, SetStatisticsColor, VT_COLOR)
    DISP_PROPERTY_EX(CGLStereoPlayerCtrl, "BaseColor", GetBaseColor, SetBaseColor, VT_COLOR)
    DISP_FUNCTION(CGLStereoPlayerCtrl, "OpenLeftFile", OpenLeftFile, VT_BOOL, VTS_BSTR)
    DISP_FUNCTION(CGLStereoPlayerCtrl, "OpenRightFile", OpenRightFile, VT_BOOL, VTS_BSTR)
    DISP_FUNCTION(CGLStereoPlayerCtrl, "OpenSetting", OpenSetting, VT_BOOL, VTS_BSTR)
    DISP_FUNCTION(CGLStereoPlayerCtrl, "SaveSetting", SaveSetting, VT_BOOL, VTS_BSTR)
    DISP_FUNCTION(CGLStereoPlayerCtrl, "Play", Play, VT_EMPTY, VTS_NONE)
    DISP_FUNCTION(CGLStereoPlayerCtrl, "Pause", Pause, VT_EMPTY, VTS_NONE)
    DISP_FUNCTION(CGLStereoPlayerCtrl, "Stop", Stop, VT_EMPTY, VTS_NONE)
    DISP_FUNCTION(CGLStereoPlayerCtrl, "SetPosition", SetPosition, VT_EMPTY, VTS_R4)
    DISP_FUNCTION(CGLStereoPlayerCtrl, "GetPosition", GetPosition, VT_R4, VTS_NONE)
    //}}AFX_DISPATCH_MAP
    DISP_FUNCTION_ID(CGLStereoPlayerCtrl, "AboutBox", DISPID_ABOUTBOX, AboutBox, VT_EMPTY, VTS_NONE)
END_DISPATCH_MAP()


/////////////////////////////////////////////////////////////////////////////
// Event map

BEGIN_EVENT_MAP(CGLStereoPlayerCtrl, COleControl)
    //{{AFX_EVENT_MAP(CGLStereoPlayerCtrl)
    EVENT_CUSTOM("NeedsResize", FireNeedsResize, VTS_NONE)
    //}}AFX_EVENT_MAP
END_EVENT_MAP()


/////////////////////////////////////////////////////////////////////////////
// Property pages
/*
BEGIN_PROPPAGEIDS(CGLStereoPlayerCtrl, 1)
    PROPPAGEID(CGLStereoPlayerPropPage::guid)
END_PROPPAGEIDS(CGLStereoPlayerCtrl)
*/

/////////////////////////////////////////////////////////////////////////////
// Initialize class factory and guid

IMPLEMENT_OLECREATE_EX(CGLStereoPlayerCtrl, "GLSTEREOPLAYER.GLStereoPlayerCtrl.1",
    0x64f33e49, 0xc865, 0x434b, 0x84, 0xe1, 0xaf, 0x4b, 0xa5, 0x73, 0x3d, 0xd7)


/////////////////////////////////////////////////////////////////////////////
// Type library ID and version

IMPLEMENT_OLETYPELIB(CGLStereoPlayerCtrl, _tlid, _wVerMajor, _wVerMinor)


/////////////////////////////////////////////////////////////////////////////
// Interface IDs

const IID BASED_CODE IID_DGLStereoPlayer =
        { 0x3266d81f, 0x3cd4, 0x420e, { 0x9b, 0x5, 0x81, 0xfa, 0x7d, 0xe5, 0x58, 0x74 } };
const IID BASED_CODE IID_DGLStereoPlayerEvents =
        { 0x29a74f8f, 0x40e5, 0x4930, { 0x9f, 0xf0, 0x32, 0x86, 0xf5, 0x74, 0x99, 0xbb } };


/////////////////////////////////////////////////////////////////////////////
// Control type information

static const DWORD BASED_CODE _dwGLStereoPlayerOleMisc =
    OLEMISC_ACTIVATEWHENVISIBLE |
//  OLEMISC_IGNOREACTIVATEWHENVISIBLE |
    OLEMISC_SETCLIENTSITEFIRST |
    OLEMISC_INSIDEOUT |
    OLEMISC_CANTLINKINSIDE |
    OLEMISC_RECOMPOSEONRESIZE;

IMPLEMENT_OLECTLTYPE(CGLStereoPlayerCtrl, IDS_GLSTEREOPLAYER, _dwGLStereoPlayerOleMisc)


/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerCtrl::CGLStereoPlayerCtrlFactory::UpdateRegistry -
// Adds or removes system registry entries for CGLStereoPlayerCtrl

BOOL CGLStereoPlayerCtrl::CGLStereoPlayerCtrlFactory::UpdateRegistry(BOOL bRegister)
{
    if (bRegister)
        return AfxOleRegisterControlClass(
            AfxGetInstanceHandle(),
            m_clsid,
            m_lpszProgID,
            IDS_GLSTEREOPLAYER,
            IDB_GLSTEREOPLAYER,
            afxRegInsertable | afxRegApartmentThreading,
            _dwGLStereoPlayerOleMisc,
            _tlid,
            _wVerMajor,
            _wVerMinor);
    else
        return AfxOleUnregisterClass(m_clsid, m_lpszProgID);
}


/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerCtrl::CGLStereoPlayerCtrl - Constructor

CGLStereoPlayerCtrl::CGLStereoPlayerCtrl() : m_statisticsFont( &m_xFontNotification )
{
    InitializeIIDs(&IID_DGLStereoPlayer, &IID_DGLStereoPlayerEvents);

    m_stereoPlayer = new StereoPlayer;
    m_slideShow = new SlideShow;
    m_slideShow->setPlayer(m_stereoPlayer);

    // Set the ActiveX module path as a default home directory
    char path[_MAX_PATH], drive[_MAX_DRIVE], dir[_MAX_DIR];
    char fname[_MAX_FNAME], ext[_MAX_EXT], homeDir[_MAX_PATH];
    GetModuleFileName(GetModuleHandle(MODULE_NAME), path, _MAX_PATH);
    _splitpath(path, drive, dir, fname, ext);
    StringCchPrintf(homeDir, _MAX_PATH, "%s%s", drive, dir);
    m_stereoPlayer->setHomeDir(homeDir);

    // Initialize variables
    m_hRC = NULL;
    m_pDC = NULL;

    m_lastMouseX = 0;
    m_lastMouseY = 0;
    m_lastPivotX = 0;
    m_lastPivotY = 0;
    m_lastIMEStatus = FALSE;

    // Load the  context menu resource
    m_contextMenu.LoadMenu(IDR_CONTEXTMENU);
    // Load the accelerator resource
    m_accel = LoadAccelerators(AfxGetInstanceHandle(), MAKEINTRESOURCE(IDR_ACCELERATOR));

    // Initial control size
    SetInitialSize(640, 480);
}


/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerCtrl::~CGLStereoPlayerCtrl - Destructor

CGLStereoPlayerCtrl::~CGLStereoPlayerCtrl()
{
	delete m_slideShow;
    delete m_stereoPlayer;
}


/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerCtrl::OnDraw - Drawing function

void CGLStereoPlayerCtrl::OnDraw(
            CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid)
{
    BeginGLDraw();

    if (m_stereoPlayer)
    {
        m_stereoPlayer->render();
        SwapBuffers();
    }

    EndGLDraw();
}


/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerCtrl::DoPropExchange - Persistence support

void CGLStereoPlayerCtrl::DoPropExchange(CPropExchange* pPX)
{
    ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
    COleControl::DoPropExchange(pPX);

    // Retrieve default property values from the StereoPlayer instance
    float r, g, b;
	float version = m_stereoPlayer->getVersion();
    CString slideShowFileName = m_slideShow->getFileName();
    CString leftFileName = m_stereoPlayer->getLeftFileName();
    CString rightFileName = m_stereoPlayer->getRightFileName();
    long format = m_stereoPlayer->getFormat();
    long type = m_stereoPlayer->getType();
    m_stereoPlayer->getAnagryphColor(TEXTURE_LEFT, &r, &g, &b);
    OLE_COLOR leftColor = RGB(255*r, 255*g, 255*b);
    m_stereoPlayer->getAnagryphColor(TEXTURE_RIGHT, &r, &g, &b);
    OLE_COLOR rightColor = RGB(255*r, 255*g, 255*b);
    BOOL swap = m_stereoPlayer->getSwap();
    BOOL keepAspectRatio = m_stereoPlayer->getKeepAspectRatio();
    float offset = m_stereoPlayer->getOffset();
    float panX = m_stereoPlayer->getPanX();
    float panY = m_stereoPlayer->getPanY();
    float zoom = m_stereoPlayer->getZoom();
    BOOL statistics = m_stereoPlayer->getStatistics();
    long playControl = m_stereoPlayer->getPlayControl();
    CString cstrStatisticsFontName = m_stereoPlayer->getStatisticsFontName();
    FONTDESC statisticsFontDesc = { sizeof(FONTDESC), cstrStatisticsFontName.AllocSysString(),
        m_stereoPlayer->getStatisticsFontSize()*10000, FW_NORMAL, ANSI_CHARSET, FALSE, FALSE, FALSE };
    m_stereoPlayer->getStatisticsColor(&r, &g, &b);
    OLE_COLOR statisticsColor = RGB(255*r, 255*g, 255*b);
    m_stereoPlayer->getBaseColor(&r, &g, &b);
    OLE_COLOR baseColor = RGB(255*r, 255*g, 255*b);    
    float speed = (float)m_stereoPlayer->getRate();
    BOOL forceSync = m_stereoPlayer->getForceSync();
    BOOL loop = m_stereoPlayer->getLoop();
    long volume = m_stereoPlayer->getVolume();
    BOOL playOnLoad = m_stereoPlayer->getPlayOnLoad();

    // Store/restore property values
    PX_Float(pPX, _T("Version"), version);
    PX_String(pPX, _T("SlideShowFileName"), slideShowFileName);
    PX_String(pPX, _T("LeftFileName"), leftFileName);
    PX_String(pPX, _T("RightFileName"), rightFileName);
    PX_Long(pPX, _T("Format"), format);
    PX_Long(pPX, _T("Type"), type);
    PX_Color(pPX, _T("LeftColor"), leftColor);
    PX_Color(pPX, _T("RightColor"), rightColor);
    PX_Bool(pPX, _T("Swap"), swap);
    PX_Bool(pPX, _T("KeepAspectRatio"), keepAspectRatio);
    PX_Float(pPX, _T("Offset"), offset);
    PX_Float(pPX, _T("PanX"), panX);
    PX_Float(pPX, _T("PanY"), panY);
    PX_Float(pPX, _T("Zoom"), zoom);
    PX_Bool(pPX, _T("Statistics"), statistics);
    PX_Long(pPX, _T("PlayControl"), playControl);
    PX_Float(pPX, _T("Speed"), speed);
    PX_Bool(pPX, _T("ForceSync"), forceSync);
    PX_Bool(pPX, _T("Loop"), loop);
    PX_Long(pPX, _T("Volume"), volume);
    PX_Bool(pPX, _T("PlayOnLoad"), playOnLoad);
    PX_Font(pPX, _T("StatisticsFont"), m_statisticsFont, &statisticsFontDesc);
    PX_Color(pPX, _T("StatisticsColor"), statisticsColor);
    PX_Color(pPX, _T("BaseColor"), baseColor);

    if (pPX->IsLoading())
    {
        // Set property values to the StereoPlayer instance 
        COLORREF color;
        m_slideShow->loadFile(slideShowFileName, FALSE);
        m_stereoPlayer->loadLeftFile(leftFileName);
        m_stereoPlayer->loadRightFile(rightFileName);
        m_stereoPlayer->setFormat(format);
        m_stereoPlayer->setType(type);
        color = TranslateColor(leftColor);
        m_stereoPlayer->setAnagryphColor(TEXTURE_LEFT,
                                         (float)GetRValue(color)/255.0f,
                                         (float)GetGValue(color)/255.0f,
                                         (float)GetBValue(color)/255.0f);
        color = TranslateColor(rightColor);
        m_stereoPlayer->setAnagryphColor(TEXTURE_RIGHT,
                                         (float)GetRValue(color)/255.0f,
                                         (float)GetGValue(color)/255.0f,
                                         (float)GetBValue(color)/255.0f);
        m_stereoPlayer->setSwap(swap);
        m_stereoPlayer->setKeepAspectRatio(keepAspectRatio);
        m_stereoPlayer->setOffset(offset);
        m_stereoPlayer->setPanX(panX);
        m_stereoPlayer->setPanY(panY);
        m_stereoPlayer->setZoom(zoom);
        m_stereoPlayer->setStatistics(statistics);
        m_stereoPlayer->setPlayControl(playControl);
        m_stereoPlayer->setRate(speed);
        m_stereoPlayer->setForceSync(forceSync);
        m_stereoPlayer->setLoop(loop);
        m_stereoPlayer->setVolume(volume);
        m_stereoPlayer->setPlayOnLoad(playOnLoad);
        color = TranslateColor(statisticsColor);
        m_stereoPlayer->setStatisticsFont(m_statisticsFont.GetFontHandle());
        m_stereoPlayer->setStatisticsColor((float)GetRValue(color)/255.0f,
                                           (float)GetGValue(color)/255.0f,
                                           (float)GetBValue(color)/255.0f);
        color = TranslateColor(baseColor);
        m_stereoPlayer->setBaseColor((float)GetRValue(color)/255.0f,
                                           (float)GetGValue(color)/255.0f,
                                           (float)GetBValue(color)/255.0f);

        // Change of source media and its format requires resize of the window (may be ignored)
        FireNeedsResize();
    }
}


/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerCtrl::GetControlFlags -
// Flags to customize MFC's implementation of ActiveX controls.
DWORD CGLStereoPlayerCtrl::GetControlFlags()
{
    DWORD dwFlags = COleControl::GetControlFlags();

    dwFlags &= ~clipPaintDC;
    dwFlags |= noFlickerActivate;
//  dwFlags |= pointerInactive;
    return dwFlags;
}


/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerCtrl::OnResetState - Reset control to default state

void CGLStereoPlayerCtrl::OnResetState()
{
    COleControl::OnResetState();  // Resets defaults found in DoPropExchange
}


/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerCtrl::AboutBox - Display an "About" box to the user

void CGLStereoPlayerCtrl::AboutBox()
{
    CDialog dlgAbout(IDD_ABOUTBOX_GLSTEREOPLAYER);
    dlgAbout.DoModal();
}

/////////////////////////////////////////////////////////////////////////////
// CGLStereoPlayerCtrl message handlers

void CGLStereoPlayerCtrl::OnDrawMetafile(CDC* pDC, const CRect& rcBounds) 
{
    // Called in the design mode and the beginning of showing (in PowerPoint)
    CBrush baseBrush;
    float r, g, b;
    m_stereoPlayer->getBaseColor(&r, &g, &b);
    baseBrush.CreateSolidBrush(RGB(255*r, 255*g, 255*b));
    pDC->FillRect(rcBounds, &baseBrush);
    
    COleControl::OnDrawMetafile(pDC, rcBounds);
}


/////////////////////////////////////////////////////////////////////////////
// Standalone Doc/View Application version (GLStereoPlayerView.cpp) needs From Here

BOOL CGLStereoPlayerCtrl::PreCreateWindow(CREATESTRUCT& cs) 
{
    cs.style |= WS_CLIPSIBLINGS | WS_CLIPCHILDREN;
    
    return COleControl::PreCreateWindow(cs);
}

int CGLStereoPlayerCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
    if (COleControl::OnCreate(lpCreateStruct) == -1)
        return -1;

    // Create an OpenGL context
    InitializeOpenGL(this);

    BeginGLDraw();

    // Initialize the OpenGL context and load the files again as needed.
    // Because some clients such as PowerPoint creates and destroys
    // the Display context and the OpenGL context for each presentations,
    // we have to restore the settings and the OpenGL context at that time.
    m_stereoPlayer->initializeGL();

    // Continuous rendering
    SetTimer(1, 10, NULL);

    EndGLDraw();

    // Accept Drag and Drop
    DragAcceptFiles(TRUE);

    return 0;
}

void CGLStereoPlayerCtrl::OnDestroy() 
{
    COleControl::OnDestroy();

    // Stop the continuous rendering
    KillTimer(1);

//  BeginGLDraw();

    // Delete the loaded files and internal OpenGL objects
    if (m_stereoPlayer)
        m_stereoPlayer->terminateGL();

//  EndGLDraw();

    // Delete the OpenGL context
    TerminateOpenGL();
}

BOOL CGLStereoPlayerCtrl::OnEraseBkgnd(CDC* pDC) 
{
    //return COleControl::OnEraseBkgnd(pDC);
    return TRUE;  // Erase background by OpenGL functions
}

void CGLStereoPlayerCtrl::OnSize(UINT nType, int cx, int cy) 
{
    COleControl::OnSize(nType, cx, cy);
    
    if (cx<=0 || cy<=0) return;

    BeginGLDraw();

    // Resize the rendering area
    m_stereoPlayer->reshape(cx, cy);

    EndGLDraw();
}

void CGLStereoPlayerCtrl::OnTimer(UINT nIDEvent) 
{
    // Update and render the media
    Invalidate(FALSE);
    
    COleControl::OnTimer(nIDEvent);
}

void CGLStereoPlayerCtrl::ShowError(long errorID)
{
    // Show an error message specified by string resource ID
    CString cstrErr;
    cstrErr.LoadString(errorID);
    AfxMessageBox(cstrErr, MB_OK | MB_ICONSTOP);
}

BOOL CGLStereoPlayerCtrl::InitializeOpenGL(CWnd* pWnd)
{
    // Create an OpenGL context

    m_pDC = new CClientDC(pWnd);
    if (NULL == m_pDC) {
        ShowError(ID_GLERROR_UNABLE_TO_GET_CD);
        return FALSE;
    }
    if (!SetupPixelFormat()) return FALSE;
    if ((m_hRC = ::wglCreateContext(m_pDC->GetSafeHdc())) == 0) {
        ShowError(ID_GLERROR_WGLCREATECONTEXT);
        return FALSE;
    }

    if (wglMakeCurrent( m_pDC->GetSafeHdc(), m_hRC) == FALSE) {
        ShowError(ID_GLERROR_WGLMAKECURRENTFAILD);
        return FALSE;
    }

#ifndef _WINDLL
    // Activate the OpenGL context
    if (m_pDC && wglMakeCurrent(m_pDC->GetSafeHdc(), m_hRC) == FALSE)
        ShowError(ID_GLERROR_WGLMAKECURRENTFAILD);
#endif //_WINDLL

    return TRUE;
}

BOOL CGLStereoPlayerCtrl::SetupPixelFormat()
{
    // Select Pixel Format which we need
    PIXELFORMATDESCRIPTOR pfd = 
    {
        sizeof(PIXELFORMATDESCRIPTOR),
        1,
        PFD_DRAW_TO_WINDOW | PFD_SUPPORT_OPENGL | PFD_DOUBLEBUFFER | PFD_STEREO |
        PFD_DEPTH_DONTCARE,
        PFD_TYPE_RGBA,
        32,
        0, 0, 0, 0, 0, 0,
        8,  // Alpha buffer needed
        0,
        0,
        0, 0, 0, 0,
        16, // Depth buffer is not important
        1,  // Stencil buffer needed
        0,
        PFD_MAIN_PLANE,
        0,
        0, 0, 0
    };
    int pixelformat;
    if ((pixelformat = ChoosePixelFormat(m_pDC->GetSafeHdc(), &pfd)) == 0) {
        ShowError(ID_GLERROR_CHOOSEPIXELFORMAT);
        return FALSE;
    }

    // If the quad buffer is available, make the StereoPlayer's quad buffer rendering selectable
    ZeroMemory(&pfd, sizeof(pfd));
    DescribePixelFormat(m_pDC->GetSafeHdc(), pixelformat, sizeof(PIXELFORMATDESCRIPTOR), &pfd);
    if (pfd.dwFlags & PFD_STEREO)
        m_stereoPlayer->setStereoEnabled(TRUE);

    if (SetPixelFormat(m_pDC->GetSafeHdc(), pixelformat, &pfd) == FALSE) {
        ShowError(ID_GLERROR_SELECTPIXELFORMAT);
        return FALSE;
    }

    return TRUE;
}

BOOL CGLStereoPlayerCtrl::TerminateOpenGL()
{
    // Destroy an OpenGL context

#ifndef _WINDLL
    // Deactivate the OpenGL context
    if (m_pDC && wglMakeCurrent(m_pDC->GetSafeHdc(), NULL) == FALSE)
        ShowError(ID_GLERROR_WGLMAKECURRENTFAILD);
#endif //_WINDLL

    EndGLDraw();
    if (wglDeleteContext(m_hRC) == FALSE)
		ShowError(ID_GLERROR_WGLDELETECONTEXT);
	delete m_pDC;
    m_hRC = NULL;
    m_pDC = NULL;

    return TRUE;
}

void CGLStereoPlayerCtrl::BeginGLDraw()
{
#ifdef _WINDLL
    // Activate the OpenGL context
    if (m_pDC && wglMakeCurrent(m_pDC->GetSafeHdc(), m_hRC) == FALSE)
        ShowError(ID_GLERROR_WGLMAKECURRENTFAILD);
#endif //_WINDLL
}

void CGLStereoPlayerCtrl::EndGLDraw()
{
#ifdef _WINDLL
    // Deactivate the OpenGL context
    if (m_pDC && wglMakeCurrent(m_pDC->GetSafeHdc(), NULL) == FALSE)
        ShowError(ID_GLERROR_WGLMAKECURRENTFAILD);
#endif //_WINDLL
}

void CGLStereoPlayerCtrl::SwapBuffers()
{
    // Swap the OpenGL double buffers
    if (m_pDC && ::SwapBuffers(m_pDC->GetSafeHdc()) == FALSE)
        ShowError(ID_GLERROR_SWAPBUFFERS);
}

void CGLStereoPlayerCtrl::OnMouseMove(UINT nFlags, CPoint point) 
{
    if (nFlags == MK_MBUTTON)
    {
        // Panning in the view area
        float deltaX = (float)(point.x - m_lastMouseX);
        float deltaY = (float)(point.y - m_lastMouseY);
        RECT rect;
        GetClientRect(&rect);
        if (rect.right>0 && rect.bottom>0) {
            if (m_stereoPlayer->getType() == STEREO_TYPE_HORIZONTAL) {
                m_stereoPlayer->setPanX(m_lastPivotX-deltaX/rect.right*2.0f/m_stereoPlayer->getZoom()*2.0f);
                m_stereoPlayer->setPanY(m_lastPivotY-deltaY/rect.bottom/m_stereoPlayer->getZoom()*2.0f);
            } else if (m_stereoPlayer->getType() == STEREO_TYPE_VERTICAL) {
                m_stereoPlayer->setPanX(m_lastPivotX-deltaX/rect.right/m_stereoPlayer->getZoom()*2.0f);
                m_stereoPlayer->setPanY(m_lastPivotY-deltaY/rect.bottom*2.0f/m_stereoPlayer->getZoom()*2.0f);
            } else {
                m_stereoPlayer->setPanX(m_lastPivotX-deltaX/rect.right/m_stereoPlayer->getZoom()*2.0f);
                m_stereoPlayer->setPanY(m_lastPivotY-deltaY/rect.bottom/m_stereoPlayer->getZoom()*2.0f);
            }
        }
    }

    // Handle the mouse hover event
    m_stereoPlayer->mouseMove(point.x, point.y, (nFlags==MK_LBUTTON));
    
    COleControl::OnMouseMove(nFlags, point);
}

void CGLStereoPlayerCtrl::OnLButtonDown(UINT nFlags, CPoint point) 
{
    // Handle the mouse hover and left button down events
    m_stereoPlayer->mouseMove(point.x, point.y, (nFlags==MK_LBUTTON));

    COleControl::OnLButtonDown(nFlags, point);
}

void CGLStereoPlayerCtrl::OnLButtonUp(UINT nFlags, CPoint point) 
{
    // Handle the mouse hover and left button up (click) events
    m_stereoPlayer->mouseMove(point.x, point.y, (nFlags==MK_LBUTTON));
    m_stereoPlayer->mouseUp(point.x, point.y);
    
    COleControl::OnLButtonUp(nFlags, point);
}

void CGLStereoPlayerCtrl::OnMButtonDown(UINT nFlags, CPoint point) 
{
    // Handle the mouse mddle button up event

    // Store a pivot point
    m_lastMouseX = point.x;
    m_lastMouseY = point.y;
    m_lastPivotX = m_stereoPlayer->getPanX();
    m_lastPivotY = m_stereoPlayer->getPanY();
    
    COleControl::OnMButtonDown(nFlags, point);
}

BOOL CGLStereoPlayerCtrl::OnMouseWheel(UINT nFlags, short zDelta, CPoint pt) 
{
    // Zooming in the view area
#ifdef _WINDLL
    if (nFlags == MK_CONTROL)
#endif //_WINDLL
    {
        if (zDelta > 0)
            m_stereoPlayer->setZoom(m_stereoPlayer->getZoom()*1.1f);
        else
            m_stereoPlayer->setZoom(m_stereoPlayer->getZoom()/1.1f);
    }

    return COleControl::OnMouseWheel(nFlags, zDelta, pt);
}

void CGLStereoPlayerCtrl::OnContextMenu(CWnd* pWnd, CPoint point) 
{
    // Get the context menu resource
    CMenu* pContextMenu = m_contextMenu.GetSubMenu(0);
    ASSERT(pContextMenu != NULL);

    // Show the context menu
    pContextMenu->TrackPopupMenu(TPM_LEFTALIGN | TPM_RIGHTBUTTON, point.x, point.y, this);
}

BOOL CGLStereoPlayerCtrl::PreTranslateMessage(MSG* pMsg) 
{
    // Override for capturing the keyboard shortcut
    if (TranslateAccelerator(pMsg->hwnd, m_accel, pMsg))
        return TRUE;

    return COleControl::PreTranslateMessage(pMsg);
}

void CGLStereoPlayerCtrl::OnSetFocus(CWnd* pOldWnd) 
{
    COleControl::OnSetFocus(pOldWnd);

    // Disable IME for capturing keyboard shortcuts
    HIMC hIMC = ImmGetContext(m_hWnd);
    m_lastIMEStatus = ImmGetOpenStatus(hIMC);
    if (m_lastIMEStatus)
        ImmSetOpenStatus(hIMC, FALSE);
    ImmReleaseContext(m_hWnd, hIMC);
}

void CGLStereoPlayerCtrl::OnKillFocus(CWnd* pNewWnd) 
{
    COleControl::OnKillFocus(pNewWnd);
    
    // Restore IME
    HIMC hIMC = ImmGetContext(m_hWnd);
    if (m_lastIMEStatus)
        ImmSetOpenStatus(hIMC, m_lastIMEStatus);
    ImmReleaseContext(m_hWnd, hIMC);
}

void CGLStereoPlayerCtrl::OnDropFiles(HDROP hDropInfo) 
{
    char pBuffer[1024];
    unsigned int numDroppedFiles = ::DragQueryFile(hDropInfo, 0xFFFFFFFF, pBuffer, 1024);

    // Accept the first file only
    if (numDroppedFiles > 0) {
        ::DragQueryFile(hDropInfo, 0, pBuffer, 1024);
        if (::GetAsyncKeyState(VK_CONTROL)<0 || ::GetAsyncKeyState(VK_SHIFT)<0 )
            OpenRightFile(pBuffer);
        else
	        OpenFile(pBuffer);
    }
    
    COleControl::OnDropFiles(hDropInfo);
}


BOOL CGLStereoPlayerCtrl::OpenFile(LPCTSTR fileName)
{
    if (!fileName) return FALSE;

    // Open the specified file as its file type determined by its extention
    CString cstrFileName = fileName;
    if (cstrFileName.Right(4).CompareNoCase(".gsp")==0 || cstrFileName.Right(4).CompareNoCase(".xml")==0)
        return OpenSetting(cstrFileName);
	if (cstrFileName.Right(4).CompareNoCase(".gsl")==0 || cstrFileName.Right(4).CompareNoCase(".txt")==0) {
		BeginGLDraw();
		bool result = m_slideShow->loadFile(cstrFileName);
		EndGLDraw();
		return result;
	}

    return OpenLeftFile(cstrFileName);
}

BOOL CGLStereoPlayerCtrl::SaveFile(LPCTSTR fileName)
{
    // Save the settings as the specified file
    if (!fileName) return FALSE;

    return SaveSetting(fileName);
}

void CGLStereoPlayerCtrl::OnFileOpenLeft()
{
    // Open a left media file with a file dialog
    OpenLeftFile(NULL);
}

BOOL CGLStereoPlayerCtrl::OpenLeftFile(LPCTSTR fileName)
{
    CString curFileName = fileName;
    if (!fileName)
    {
        // If the fileName is not given, ask a file name to open as a left media
        CString cstrExt;
        cstrExt.LoadString(IDS_SOURCE_FILE_EXT);
        CFileDialog fileDlg(TRUE, NULL, NULL, OFN_HIDEREADONLY | OFN_FILEMUSTEXIST, cstrExt, this);
        if (fileDlg.DoModal() == IDOK)
            curFileName = fileDlg.GetPathName();
        else
            return FALSE;
    }

    BeginGLDraw();
    // Load the left media file
    BOOL result = m_stereoPlayer->loadLeftFile(curFileName);
    if (result)
        FireNeedsResize();
    EndGLDraw();

    SetModifiedFlag();

    return result;
}

void CGLStereoPlayerCtrl::OnFileOpenRight()
{
    // Open a right media file with a file dialog
    OpenRightFile(NULL);
}

BOOL CGLStereoPlayerCtrl::OpenRightFile(LPCTSTR fileName) 
{
    CString curFileName = fileName;
    if (!fileName)
    {
        // If the fileName is not given, ask a file name to open as a right media
        CString cstrExt;
        cstrExt.LoadString(IDS_SOURCE_FILE_EXT);
        CFileDialog fileDlg(TRUE, NULL, NULL, OFN_HIDEREADONLY | OFN_FILEMUSTEXIST, cstrExt);
        if (fileDlg.DoModal() == IDOK)
            curFileName = fileDlg.GetPathName();
        else
            return FALSE;
    }

    BeginGLDraw();
    // Load the right media file
    BOOL result = m_stereoPlayer->loadRightFile(curFileName);
    if (result)
        FireNeedsResize();
    EndGLDraw();

    SetModifiedFlag();

    return result;
}

void CGLStereoPlayerCtrl::OnFileOpenSetting() 
{
    // Open a setting file with a file dialog
    OpenSetting(NULL);
}

BOOL CGLStereoPlayerCtrl::OpenSetting(LPCTSTR fileName) 
{
    CString curFileName = fileName;
    if (!fileName)
    {
        // If the fileName is not given, ask a file name to open as a setting file
        CString cstrExt;
        cstrExt.LoadString(IDS_SETTING_FILE_EXT);
        CFileDialog fileDlg(TRUE, NULL, NULL, OFN_HIDEREADONLY|OFN_FILEMUSTEXIST, cstrExt);
        if (fileDlg.DoModal() == IDOK)
            curFileName = fileDlg.GetPathName();
        else
            return FALSE;
    }

    BeginGLDraw();
    // Load the setting file
    BOOL result = LoadSettingFile(m_stereoPlayer, m_stereoPlayer->getHomeDir(), curFileName);
    if (result)
        FireNeedsResize();
    EndGLDraw();

#ifndef _WINDLL
    // In Doc/View architecture, its document file is now changed.
    SetModifiedFlag(FALSE);
    GetDocument()->SetPathName(curFileName);
#endif //_WINDLL

    return result;
}

void CGLStereoPlayerCtrl::OnFileSaveSetting() 
{
    SaveSetting(NULL);
}

BOOL CGLStereoPlayerCtrl::SaveSetting(LPCTSTR fileName) 
{
    CString curFileName = fileName;
    if (!fileName)
    {
        // If the fileName is not given, ask a file name to save as a setting file
        CString cstrExt;
        cstrExt.LoadString(IDS_SETTING_FILE_EXT);
        CFileDialog fileDlg(FALSE, NULL, NULL, OFN_HIDEREADONLY|OFN_OVERWRITEPROMPT, cstrExt);
        if (fileDlg.DoModal() == IDOK)
            curFileName = fileDlg.GetPathName();
        else
            return FALSE;
    }

    // Save the setting file
    BOOL result = SaveSettingFile(m_stereoPlayer, m_stereoPlayer->getHomeDir(), curFileName);

#ifndef _WINDLL
    // In Doc/View architecture, its document file path is now changed.
    SetModifiedFlag(FALSE);
    GetDocument()->SetPathName(curFileName);
#endif //_WINDLL

    return result;
}

void CGLStereoPlayerCtrl::OnViewFormatSeparated() 
{
    m_stereoPlayer->setFormat(STEREO_FORMAT_SEPARATED);

    // Change of stereo format and type requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdateViewFormatSeparated(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getFormat()==STEREO_FORMAT_SEPARATED);
}

void CGLStereoPlayerCtrl::OnViewFormatHorizontal() 
{
    m_stereoPlayer->setFormat(STEREO_FORMAT_HORIZONTAL);

    // Change of stereo format and type requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdateViewFormatHorizontal(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getFormat()==STEREO_FORMAT_HORIZONTAL);
}

void CGLStereoPlayerCtrl::OnViewFormatHorizontalComp() 
{
    m_stereoPlayer->setFormat(STEREO_FORMAT_HORIZONTAL_COMP);

    // Change of stereo format and type requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdateViewFormatHorizontalComp(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getFormat()==STEREO_FORMAT_HORIZONTAL_COMP);
}

void CGLStereoPlayerCtrl::OnViewFormatVertical() 
{
    m_stereoPlayer->setFormat(STEREO_FORMAT_VERTICAL);

    // Change of stereo format and type requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdateViewFormatVertical(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getFormat()==STEREO_FORMAT_VERTICAL);
}

void CGLStereoPlayerCtrl::OnViewFormatVerticalComp() 
{
    m_stereoPlayer->setFormat(STEREO_FORMAT_VERTICAL_COMP);

    // Change of stereo format and type requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdateViewFormatVerticalComp(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getFormat()==STEREO_FORMAT_VERTICAL_COMP);
}

void CGLStereoPlayerCtrl::OnViewChangeFormat() 
{
    m_stereoPlayer->toggleFormat();

    // Change of stereo format and type requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnViewStereoLeft() 
{
    m_stereoPlayer->setType(STEREO_TYPE_LEFT);

    // Change of stereo format and type requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdateViewStereoLeft(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getType()==STEREO_TYPE_LEFT);
}

void CGLStereoPlayerCtrl::OnViewStereoRight() 
{
    m_stereoPlayer->setType(STEREO_TYPE_RIGHT);

    // Change of stereo format and type requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdateViewStereoRight(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getType()==STEREO_TYPE_RIGHT);
}

void CGLStereoPlayerCtrl::OnViewStereoAnagryph() 
{
    m_stereoPlayer->setType(STEREO_TYPE_ANAGRYPH);

    // Change of stereo format and type requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdateViewStereoAnagryph(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getType()==STEREO_TYPE_ANAGRYPH);
}

void CGLStereoPlayerCtrl::OnViewStereoHorizontal() 
{
    m_stereoPlayer->setType(STEREO_TYPE_HORIZONTAL);

    // Change of stereo format and type requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdateViewStereoHorizontal(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getType()==STEREO_TYPE_HORIZONTAL);
}

void CGLStereoPlayerCtrl::OnViewStereoVertical() 
{
    m_stereoPlayer->setType(STEREO_TYPE_VERTICAL);

    // Change of stereo format and type requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdateViewStereoVertical(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getType()==STEREO_TYPE_VERTICAL);
}

void CGLStereoPlayerCtrl::OnViewStereoHorizontalInterleaved() 
{
    m_stereoPlayer->setType(STEREO_TYPE_HORIZONTAL_INTERLEAVED);

    // Change of stereo format and type requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdateViewStereoHorizontalInterleaved(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getType()==STEREO_TYPE_HORIZONTAL_INTERLEAVED);
}

void CGLStereoPlayerCtrl::OnViewStereoVerticalInterleaved() 
{
    m_stereoPlayer->setType(STEREO_TYPE_VERTICAL_INTERLEAVED);

    // Change of stereo format and type requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdateViewStereoVerticalInterleaved(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getType()==STEREO_TYPE_VERTICAL_INTERLEAVED);
}

void CGLStereoPlayerCtrl::OnViewStereoSharp3D() 
{
    m_stereoPlayer->setType(STEREO_TYPE_SHARP3D);

    // Change of stereo format and type requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdateViewStereoSharp3D(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getType()==STEREO_TYPE_SHARP3D);
}

void CGLStereoPlayerCtrl::OnViewStereoQuadBuffer() 
{
    m_stereoPlayer->setType(STEREO_TYPE_QUADBUFFER);

    // Change of stereo format and type requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdateViewStereoQuadbuffer(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getType()==STEREO_TYPE_QUADBUFFER);
}

void CGLStereoPlayerCtrl::OnViewChangeStereoType() 
{
    m_stereoPlayer->toggleType();

    // Change of stereo format and type requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnViewLeftColor() 
{
    // Change the left anagryph color
    float r, g, b;
    m_stereoPlayer->getAnagryphColor(TEXTURE_LEFT, &r, &g, &b);
    CColorDialog colorDlg(RGB(255*r, 255*g, 255*b));
    if (colorDlg.DoModal() == IDOK) {
        m_stereoPlayer->setAnagryphColor(TEXTURE_LEFT,
                                         (float)GetRValue(colorDlg.GetColor())/255.0f,
                                         (float)GetGValue(colorDlg.GetColor())/255.0f,
                                         (float)GetBValue(colorDlg.GetColor())/255.0f);

#ifdef _WINDLL
		SetModifiedFlag();
#endif // _WINDLL
    }
}

void CGLStereoPlayerCtrl::OnViewRightColor() 
{
    // Change the right anagryph color
    float r, g, b;
    m_stereoPlayer->getAnagryphColor(TEXTURE_RIGHT, &r, &g, &b);
    CColorDialog colorDlg(RGB(255*r, 255*g, 255*b));
    if (colorDlg.DoModal() == IDOK) {
        m_stereoPlayer->setAnagryphColor(TEXTURE_RIGHT,
                                         (float)GetRValue(colorDlg.GetColor())/255.0f,
                                         (float)GetGValue(colorDlg.GetColor())/255.0f,
                                         (float)GetBValue(colorDlg.GetColor())/255.0f);

#ifdef _WINDLL
		SetModifiedFlag();
#endif // _WINDLL
    }
}

void CGLStereoPlayerCtrl::OnViewSwap() 
{
    // Swap left/right media asignment
    m_stereoPlayer->setSwap(!m_stereoPlayer->getSwap());

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdateViewSwap(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getSwap());
}

void CGLStereoPlayerCtrl::OnViewKeepAspectRatio() 
{
    m_stereoPlayer->setKeepAspectRatio(!m_stereoPlayer->getKeepAspectRatio());

#ifdef _WINDLL
    SetModifiedFlag();
#endif // _WINDLL
}

void CGLStereoPlayerCtrl::OnUpdateViewKeepAspectRatio(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getKeepAspectRatio());
}

void CGLStereoPlayerCtrl::OnViewOffsetIncrease() 
{
    // Amount of the offset increment is depend on the width of the source media
    if (m_stereoPlayer->getWidth() > 0)
        m_stereoPlayer->setOffset(m_stereoPlayer->getOffset()+1.0f/m_stereoPlayer->getWidth());

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnViewOffsetDecrease() 
{
    // Amount of the offset decrement is depend on the width of the source media
    if (m_stereoPlayer->getWidth() > 0)
        m_stereoPlayer->setOffset(m_stereoPlayer->getOffset()-1.0f/m_stereoPlayer->getWidth());

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnViewOffsetReset() 
{
    m_stereoPlayer->setOffset(0.0f);

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnViewPanLeft() 
{
    if (m_stereoPlayer->getZoom() > 0.0f)
        m_stereoPlayer->setPanX(m_stereoPlayer->getPanX()-0.01f/m_stereoPlayer->getZoom());

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnViewPanRight() 
{
    if (m_stereoPlayer->getZoom() > 0.0f)
        m_stereoPlayer->setPanX(m_stereoPlayer->getPanX()+0.01f/m_stereoPlayer->getZoom());

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnViewPanUp() 
{
    if (m_stereoPlayer->getZoom() > 0.0f)
        m_stereoPlayer->setPanY(m_stereoPlayer->getPanY()-0.01f/m_stereoPlayer->getZoom());

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnViewPanDown() 
{
    if (m_stereoPlayer->getZoom() > 0.0f)
        m_stereoPlayer->setPanY(m_stereoPlayer->getPanY()+0.01f/m_stereoPlayer->getZoom());

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnViewZoomUp() 
{
    m_stereoPlayer->setZoom(m_stereoPlayer->getZoom()*1.1f);

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnViewZoomDown() 
{
    m_stereoPlayer->setZoom(m_stereoPlayer->getZoom()/1.1f);

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnViewPanZoomReset() 
{
    m_stereoPlayer->setPanX(0.0f);
    m_stereoPlayer->setPanY(0.0f);
    m_stereoPlayer->setZoom(1.0f);

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnViewStatistics() 
{
    m_stereoPlayer->setStatistics(!m_stereoPlayer->getStatistics());

#ifdef _WINDLL
    SetModifiedFlag();
#endif // _WINDLL
}

void CGLStereoPlayerCtrl::OnUpdateViewStatistics(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getStatistics());
}

void CGLStereoPlayerCtrl::OnViewPlayControl() 
{
    m_stereoPlayer->togglePlayControl();

#ifdef _WINDLL
	SetModifiedFlag();
#endif // _WINDLL
}

void CGLStereoPlayerCtrl::OnViewPlayControlShow() 
{
    m_stereoPlayer->setPlayControl(PLAYCONTROL_SHOW);

#ifdef _WINDLL
	SetModifiedFlag();
#endif // _WINDLL
}

void CGLStereoPlayerCtrl::OnUpdateViewPlayControlShow(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getPlayControl()==PLAYCONTROL_SHOW);
}

void CGLStereoPlayerCtrl::OnViewPlayControlAuto() 
{
    m_stereoPlayer->setPlayControl(PLAYCONTROL_AUTO);

#ifdef _WINDLL
	SetModifiedFlag();
#endif // _WINDLL
}

void CGLStereoPlayerCtrl::OnUpdateViewPlayControlAuto(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getPlayControl()==PLAYCONTROL_AUTO);
}

void CGLStereoPlayerCtrl::OnViewPlayControlHide() 
{
    m_stereoPlayer->setPlayControl(PLAYCONTROL_HIDE);

#ifdef _WINDLL
	SetModifiedFlag();
#endif // _WINDLL
}

void CGLStereoPlayerCtrl::OnUpdateViewPlayControlHide(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getPlayControl()==PLAYCONTROL_HIDE);
}

void CGLStereoPlayerCtrl::OnPlayPlayPause() 
{
    // Toggle play and pause state
    bool play = !m_stereoPlayer->isPlaying();

    if (!m_stereoPlayer->isPlaying())
        m_stereoPlayer->play();
    else
        m_stereoPlayer->pause();
}

void CGLStereoPlayerCtrl::OnPlayStop() 
{
    // Stop and rewind the playback
    m_stereoPlayer->setPosition(0);
    m_stereoPlayer->pause();
}

void CGLStereoPlayerCtrl::OnPlayPlayOnLoad() 
{
    m_stereoPlayer->setPlayOnLoad(!m_stereoPlayer->getPlayOnLoad());

#ifdef _WINDLL
	SetModifiedFlag();
#endif // _WINDLL
}

void CGLStereoPlayerCtrl::OnUpdatePlayPlayOnLoad(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getPlayOnLoad());
}

void CGLStereoPlayerCtrl::OnPlaySpeed25()
{
    m_stereoPlayer->setRate(0.25f);

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdatePlaySpeed25(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getRate()==0.25f);
}

void CGLStereoPlayerCtrl::OnPlaySpeed50() 
{
    m_stereoPlayer->setRate(0.5f);

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdatePlaySpeed50(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getRate()==0.5f);
}

void CGLStereoPlayerCtrl::OnPlaySpeed100() 
{
    m_stereoPlayer->setRate(1.0f);

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdatePlaySpeed100(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getRate()==1.0f);
}

void CGLStereoPlayerCtrl::OnPlaySpeed150() 
{
    m_stereoPlayer->setRate(1.5f);

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdatePlaySpeed150(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getRate()==1.5f);
}

void CGLStereoPlayerCtrl::OnPlaySpeed200() 
{
    m_stereoPlayer->setRate(2.0f);

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnUpdatePlaySpeed200(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getRate()==2.0f);
}

void CGLStereoPlayerCtrl::OnPlayForceSync() 
{
    m_stereoPlayer->setForceSync(!m_stereoPlayer->getForceSync());

#ifdef _WINDLL
	SetModifiedFlag();
#endif // _WINDLL
}

void CGLStereoPlayerCtrl::OnUpdatePlayForceSync(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getForceSync());
}

void CGLStereoPlayerCtrl::OnPlayLoop() 
{
    m_stereoPlayer->setLoop(!m_stereoPlayer->getLoop());

#ifdef _WINDLL
	SetModifiedFlag();
#endif // _WINDLL
}

void CGLStereoPlayerCtrl::OnUpdatePlayLoop(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getLoop());
}

void CGLStereoPlayerCtrl::OnPlayVolumeUp() 
{
    m_stereoPlayer->setVolume(m_stereoPlayer->getVolume()+500);

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnPlayVolumeDown() 
{
    m_stereoPlayer->setVolume(m_stereoPlayer->getVolume()-500);

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnPlayVolumeMute() 
{
    m_stereoPlayer->volumeSilence();

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnSlideFirst() 
{
    BeginGLDraw();
    m_slideShow->first();
    EndGLDraw();
}

void CGLStereoPlayerCtrl::OnSlideLast() 
{
    BeginGLDraw();
    m_slideShow->last();
    EndGLDraw();
}

void CGLStereoPlayerCtrl::OnSlidePrev() 
{
    BeginGLDraw();
    m_slideShow->prev();
    EndGLDraw();
}

void CGLStereoPlayerCtrl::OnSlideNext() 
{
    BeginGLDraw();
    m_slideShow->next();
    EndGLDraw();
}

void CGLStereoPlayerCtrl::OnSlideFade() 
{
	m_stereoPlayer->setTransition(!m_stereoPlayer->getTransition());

#ifdef _WINDLL
	SetModifiedFlag();
#endif // _WINDLL
}

void CGLStereoPlayerCtrl::OnUpdateSlideFade(CCmdUI* pCmdUI) 
{
    pCmdUI->SetCheck(m_stereoPlayer->getTransition());
}

// Standalone Doc/View Application version (GLStereoPlayerView.cpp) needs From Here
/////////////////////////////////////////////////////////////////////////////


/////////////////////////////////////////////////////////////////////////////
// Properties and Methods implementations for Automation

BSTR CGLStereoPlayerCtrl::GetLeftFile() 
{
    CString strResult;
    if (m_stereoPlayer)
        strResult = m_stereoPlayer->getLeftFileName();

    return strResult.AllocSysString();
}

void CGLStereoPlayerCtrl::SetLeftFile(LPCTSTR lpszNewValue) 
{
    // Set the left media file to open it
    OpenLeftFile(lpszNewValue);

    // Change of source media requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

BSTR CGLStereoPlayerCtrl::GetRightFile() 
{
    CString strResult;
    if (m_stereoPlayer)
        strResult = m_stereoPlayer->getRightFileName();

    return strResult.AllocSysString();
}

void CGLStereoPlayerCtrl::SetRightFile(LPCTSTR lpszNewValue) 
{
    // Set the right media file to open it
    OpenRightFile(lpszNewValue);

    // Change of source media requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

long CGLStereoPlayerCtrl::GetFormat() 
{
    return m_stereoPlayer->getFormat();
}

void CGLStereoPlayerCtrl::SetFormat(long nNewValue) 
{
    m_stereoPlayer->setFormat(nNewValue);

    // Change of stereo format requires resize of the window (may be ignored)
    FireNeedsResize();

    SetModifiedFlag();
}

long CGLStereoPlayerCtrl::GetStereoType() 
{
    return m_stereoPlayer->getType();
}

void CGLStereoPlayerCtrl::SetStereoType(long nNewValue) 
{
    m_stereoPlayer->setType(nNewValue);

    SetModifiedFlag();
}

BOOL CGLStereoPlayerCtrl::GetSwap() 
{
    return m_stereoPlayer->getSwap();
}

void CGLStereoPlayerCtrl::SetSwap(BOOL bNewValue) 
{
    m_stereoPlayer->setSwap(bNewValue);

    SetModifiedFlag();
}

float CGLStereoPlayerCtrl::GetOffset() 
{
    return m_stereoPlayer->getOffset();
}

void CGLStereoPlayerCtrl::SetOffset(float newValue) 
{
    m_stereoPlayer->setOffset(newValue);

    SetModifiedFlag();
}

float CGLStereoPlayerCtrl::GetPanX() 
{
    return m_stereoPlayer->getPanX();
}

void CGLStereoPlayerCtrl::SetPanX(float newValue) 
{
    m_stereoPlayer->setPanX(newValue);

    SetModifiedFlag();
}

float CGLStereoPlayerCtrl::GetPanY() 
{
    return m_stereoPlayer->getPanY();
}

void CGLStereoPlayerCtrl::SetPanY(float newValue) 
{
    m_stereoPlayer->setPanY(newValue);

    SetModifiedFlag();
}

float CGLStereoPlayerCtrl::GetZoom() 
{
    return m_stereoPlayer->getZoom();
}

void CGLStereoPlayerCtrl::SetZoom(float newValue) 
{
    m_stereoPlayer->setZoom(newValue);

    SetModifiedFlag();
}

BOOL CGLStereoPlayerCtrl::GetStatistics() 
{
    return m_stereoPlayer->getStatistics();
}

void CGLStereoPlayerCtrl::SetStatistics(BOOL bNewValue) 
{
    m_stereoPlayer->setStatistics(bNewValue);

    SetModifiedFlag();
}

long CGLStereoPlayerCtrl::GetPlayControl() 
{
    return m_stereoPlayer->getPlayControl();
}

void CGLStereoPlayerCtrl::SetPlayControl(long nNewValue) 
{
    m_stereoPlayer->setPlayControl(nNewValue);

    SetModifiedFlag();
}

float CGLStereoPlayerCtrl::GetSpeed() 
{
    return (float)m_stereoPlayer->getRate();
}

void CGLStereoPlayerCtrl::SetSpeed(float newValue) 
{
    m_stereoPlayer->setRate(newValue);

    SetModifiedFlag();
}

BOOL CGLStereoPlayerCtrl::GetForceSync() 
{
    return m_stereoPlayer->getForceSync();
}

void CGLStereoPlayerCtrl::SetForceSync(BOOL bNewValue) 
{
    m_stereoPlayer->setForceSync(bNewValue);

    SetModifiedFlag();
}

BOOL CGLStereoPlayerCtrl::GetLoop() 
{
    return m_stereoPlayer->getLoop();

    return TRUE;
}

void CGLStereoPlayerCtrl::SetLoop(BOOL bNewValue) 
{
    m_stereoPlayer->setLoop(bNewValue);

    SetModifiedFlag();
}

long CGLStereoPlayerCtrl::GetVolume() 
{
    return m_stereoPlayer->getVolume();
}

void CGLStereoPlayerCtrl::SetVolume(long nNewValue) 
{
    m_stereoPlayer->setVolume(nNewValue);

    SetModifiedFlag();
}

BOOL CGLStereoPlayerCtrl::GetIsPlaying() 
{
    return m_stereoPlayer->isPlaying();
}

long CGLStereoPlayerCtrl::GetSourceWidth() 
{
    return m_stereoPlayer->getWidth();
}

long CGLStereoPlayerCtrl::GetSourceHeight() 
{
    return m_stereoPlayer->getHeight();
}

float CGLStereoPlayerCtrl::GetSourceDuration() 
{
    return (float)m_stereoPlayer->getDuration();
}

long CGLStereoPlayerCtrl::GetClientWidth() 
{
    // When the stereo source is horizontally combined, its actual width is half of the source media
    if (m_stereoPlayer->getFormat() == STEREO_FORMAT_HORIZONTAL)
        return m_stereoPlayer->getWidth() / 2;

    return m_stereoPlayer->getWidth();
}

long CGLStereoPlayerCtrl::GetClientHeight() 
{
    // When the stereo source is vertically combined, its actual width is half of the source media
    if (m_stereoPlayer->getFormat() == STEREO_FORMAT_VERTICAL)
        return m_stereoPlayer->getHeight() / 2;

    return m_stereoPlayer->getHeight();
}

void CGLStereoPlayerCtrl::Play() 
{
    m_stereoPlayer->play();
}

void CGLStereoPlayerCtrl::Pause() 
{
    m_stereoPlayer->pause();
}

void CGLStereoPlayerCtrl::Stop() 
{
    m_stereoPlayer->stop();
}

void CGLStereoPlayerCtrl::SetPosition(float position) 
{
    m_stereoPlayer->setPosition(position);
}

float CGLStereoPlayerCtrl::GetPosition() 
{
    return (float)m_stereoPlayer->getPosition();
}

LPDISPATCH CGLStereoPlayerCtrl::GetStatisticsFont()
{
    return m_statisticsFont.GetFontDispatch( );
}

void CGLStereoPlayerCtrl::SetStatisticsFont(LPDISPATCH newValue)
{
    m_statisticsFont.InitializeFont(NULL, newValue);
    m_stereoPlayer->setStatisticsFont(m_statisticsFont.GetFontHandle());

    OnFontChanged();

    SetModifiedFlag( );
}

OLE_COLOR CGLStereoPlayerCtrl::GetStatisticsColor() 
{
    float r, g, b;
    m_stereoPlayer->getStatisticsColor(&r, &g, &b);
    return RGB(r*255, g*255, b*255);
}

void CGLStereoPlayerCtrl::SetStatisticsColor(OLE_COLOR nNewValue) 
{
    COLORREF color = TranslateColor(nNewValue);
    if (m_stereoPlayer)
        m_stereoPlayer->setStatisticsColor((float)GetRValue(color)/255.0f,
                                           (float)GetGValue(color)/255.0f,
                                           (float)GetBValue(color)/255.0f);

    SetModifiedFlag();
}

OLE_COLOR CGLStereoPlayerCtrl::GetBaseColor() 
{
    float r, g, b;
    m_stereoPlayer->getBaseColor(&r, &g, &b);
    return RGB(r*255, g*255, b*255);
}

void CGLStereoPlayerCtrl::SetBaseColor(OLE_COLOR nNewValue) 
{
    COLORREF color = TranslateColor(nNewValue);
    if (m_stereoPlayer)
        m_stereoPlayer->setBaseColor((float)GetRValue(color)/255.0f,
                                     (float)GetGValue(color)/255.0f,
                                     (float)GetBValue(color)/255.0f);
    Refresh();

    SetModifiedFlag();
}

OLE_COLOR CGLStereoPlayerCtrl::GetLeftColor() 
{
    float r, g, b;
    m_stereoPlayer->getAnagryphColor(TEXTURE_LEFT, &r, &g, &b);
    return RGB(r*255, g*255, b*255);
}

void CGLStereoPlayerCtrl::SetLeftColor(OLE_COLOR nNewValue) 
{
    COLORREF color = TranslateColor(nNewValue);
    if (m_stereoPlayer)
        m_stereoPlayer->setAnagryphColor(TEXTURE_RIGHT,
                                         (float)GetRValue(color)/255.0f,
                                         (float)GetGValue(color)/255.0f,
                                         (float)GetBValue(color)/255.0f);

    SetModifiedFlag();
}

OLE_COLOR CGLStereoPlayerCtrl::GetRightColor() 
{
    float r, g, b;
    m_stereoPlayer->getAnagryphColor(TEXTURE_RIGHT, &r, &g, &b);
    return RGB(r*255, g*255, b*255);
}

void CGLStereoPlayerCtrl::SetRightColor(OLE_COLOR nNewValue) 
{
    COLORREF color = TranslateColor(nNewValue);
    if (m_stereoPlayer)
        m_stereoPlayer->setAnagryphColor(TEXTURE_RIGHT,
                                         (float)GetRValue(color)/255.0f,
                                         (float)GetGValue(color)/255.0f,
                                         (float)GetBValue(color)/255.0f);

    SetModifiedFlag();
}

BOOL CGLStereoPlayerCtrl::GetPlayOnLoad() 
{
    return m_stereoPlayer->getPlayOnLoad();
}

void CGLStereoPlayerCtrl::SetPlayOnLoad(BOOL bNewValue) 
{
    m_stereoPlayer->setPlayOnLoad(bNewValue);

    SetModifiedFlag();
}

void CGLStereoPlayerCtrl::OnHideToolBars() 
{
    // When deactivate or page changed, stop the movie
    if (m_stereoPlayer)
        m_stereoPlayer->stop();

    COleControl::OnHideToolBars();
}
