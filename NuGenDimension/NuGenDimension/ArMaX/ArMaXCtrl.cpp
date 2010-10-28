// ArMaXCtrl.cpp : Implementation of the CArMaXCtrl ActiveX Control class.

#include "stdafx.h"
#include "ArMaX.h"
#include "ArMaXCtrl.h"
#include "ArMaXPropPage.h"
#include ".\ArMaXctrl.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#endif


float gAmbient[4];
float gDiffuse[4];
float gEmission[4];
float gSpecular[4];

float gShininnes;


IMPLEMENT_DYNCREATE(CArMaXCtrl, COleControl)



// Message map

BEGIN_MESSAGE_MAP(CArMaXCtrl, COleControl)
	ON_OLEVERB(AFX_IDS_VERB_PROPERTIES, OnProperties)
	ON_WM_CREATE()
	ON_WM_ERASEBKGND()
	ON_WM_SIZE()
	ON_WM_DESTROY()
END_MESSAGE_MAP()



// Dispatch map

BEGIN_DISPATCH_MAP(CArMaXCtrl, COleControl)
	DISP_FUNCTION_ID(CArMaXCtrl, "SetAmbient", dispidSetAmbient, SetAmbient, VT_EMPTY, VTS_R4 VTS_R4 VTS_R4)
	DISP_FUNCTION_ID(CArMaXCtrl, "SetDiffuse", dispidSetDiffuse, SetDiffuse, VT_EMPTY, VTS_R4 VTS_R4 VTS_R4)
	DISP_FUNCTION_ID(CArMaXCtrl, "SetEmission", dispidSetEmission, SetEmission, VT_EMPTY, VTS_R4 VTS_R4 VTS_R4)
	DISP_FUNCTION_ID(CArMaXCtrl, "SetSpecular", dispidSetSpecular, SetSpecular, VT_EMPTY, VTS_R4 VTS_R4 VTS_R4)
	DISP_FUNCTION_ID(CArMaXCtrl, "SetTransparent", dispidSetTransparent, SetTransparent, VT_EMPTY, VTS_R4)
	DISP_FUNCTION_ID(CArMaXCtrl, "SetShininess", dispidSetShininess, SetShininess, VT_EMPTY, VTS_R4)
	DISP_FUNCTION_ID(CArMaXCtrl, "CopyToClipboard", dispidCopyToClipboard, CopyToClipboard, VT_EMPTY, VTS_NONE)
END_DISPATCH_MAP()



// Event map

BEGIN_EVENT_MAP(CArMaXCtrl, COleControl)
END_EVENT_MAP()



// Property pages

// TODO: Add more property pages as needed.  Remember to increase the count!
BEGIN_PROPPAGEIDS(CArMaXCtrl, 1)
	PROPPAGEID(CArMaXPropPage::guid)
END_PROPPAGEIDS(CArMaXCtrl)



// Initialize class factory and guid

IMPLEMENT_OLECREATE_EX(CArMaXCtrl, "ArMaX.ArMaXCtrl.1",
	0x4012f180, 0xaef8, 0x4dfb, 0xa4, 0x5d, 0x56, 0xbf, 0x5b, 0xa8, 0x64, 0x58)



// Type library ID and version

IMPLEMENT_OLETYPELIB(CArMaXCtrl, _tlid, _wVerMajor, _wVerMinor)



// Interface IDs

const IID BASED_CODE IID_DArMaX =
		{ 0x8E636C37, 0xEBE4, 0x46D3, { 0x8C, 0x9E, 0x10, 0xDA, 0x24, 0x64, 0x8E, 0xD4 } };
const IID BASED_CODE IID_DArMaXEvents =
		{ 0xC72A2FDA, 0x8376, 0x4868, { 0x99, 0xFB, 0x58, 0x18, 0x41, 0x38, 0xC1, 0x80 } };



// Control type information

static const DWORD BASED_CODE _dwArMaXOleMisc =
	OLEMISC_ACTIVATEWHENVISIBLE |
	OLEMISC_SETCLIENTSITEFIRST |
	OLEMISC_INSIDEOUT |
	OLEMISC_CANTLINKINSIDE |
	OLEMISC_RECOMPOSEONRESIZE;

IMPLEMENT_OLECTLTYPE(CArMaXCtrl, IDS_ArMaX, _dwArMaXOleMisc)



// CArMaXCtrl::CArMaXCtrlFactory::UpdateRegistry -
// Adds or removes system registry entries for CArMaXCtrl

BOOL CArMaXCtrl::CArMaXCtrlFactory::UpdateRegistry(BOOL bRegister)
{
	// TODO: Verify that your control follows apartment-model threading rules.
	// Refer to MFC TechNote 64 for more information.
	// If your control does not conform to the apartment-model rules, then
	// you must modify the code below, changing the 6th parameter from
	// afxRegApartmentThreading to 0.

	if (bRegister)
		return AfxOleRegisterControlClass(
			AfxGetInstanceHandle(),
			m_clsid,
			m_lpszProgID,
			IDS_ArMaX,
			IDB_ArMaX,
			afxRegApartmentThreading,
			_dwArMaXOleMisc,
			_tlid,
			_wVerMajor,
			_wVerMinor);
	else
		return AfxOleUnregisterClass(m_clsid, m_lpszProgID);
}



// CArMaXCtrl::CArMaXCtrl - Constructor

CArMaXCtrl::CArMaXCtrl()
{
	InitializeIIDs(&IID_DArMaX, &IID_DArMaXEvents);
	
	m_hDC       = NULL;
	m_hRC       = NULL;

	gAmbient[0] = 0.694f;
	gAmbient[1] = 0.36f;
	gAmbient[2] = 1.0f;
	gAmbient[3] = 1.0f;

	gDiffuse[0] = 0.919f;
	gDiffuse[1] = 0.46f;
	gDiffuse[2] = 0.46f;
	gDiffuse[3] = 1.0f;

	gEmission[0] = 0.27f;
	gEmission[1] = 0.0f;
	gEmission[2] = 0.0f;
	gEmission[3] = 1.0f;

	gSpecular[0] = 0.7f;
	gSpecular[1] = 1.0f;
	gSpecular[2] = 0.667f;
	gSpecular[3] = 1.0f;

	gShininnes = 67.0f;
}



// CArMaXCtrl::~CArMaXCtrl - Destructor

CArMaXCtrl::~CArMaXCtrl()
{
}



// CArMaXCtrl::OnDraw - Drawing function

void CArMaXCtrl::OnDraw(
			CDC* pdc, const CRect& rcBounds, const CRect& rcInvalid)
{
	if (!pdc)
		return;

  wglMakeCurrent(m_hDC,m_hRC);

  // Clear the buffers
  glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

  // Start rendering...
  DrawScene();

  glFlush();
  glFinish();
  SwapBuffers(m_hDC);
	
}



// CArMaXCtrl::DoPropExchange - Persistence support

void CArMaXCtrl::DoPropExchange(CPropExchange* pPX)
{
	ExchangeVersion(pPX, MAKELONG(_wVerMinor, _wVerMajor));
	COleControl::DoPropExchange(pPX);

	// TODO: Call PX_ functions for each persistent custom property.
}



// CArMaXCtrl::OnResetState - Reset control to default state

void CArMaXCtrl::OnResetState()
{
	COleControl::OnResetState();  // Resets defaults found in DoPropExchange

	// TODO: Reset any other control state here.
}

BOOL CArMaXCtrl::SetWindowPixelFormat(HDC hDC)
{
  PIXELFORMATDESCRIPTOR pfd;
  pfd.nVersion        = 1;
  pfd.dwFlags         = PFD_DRAW_TO_WINDOW| PFD_SUPPORT_OPENGL| 
    PFD_DRAW_TO_BITMAP| PFD_DOUBLEBUFFER  | PFD_SWAP_EXCHANGE;
  pfd.iPixelType      = PFD_TYPE_RGBA;
  pfd.cColorBits      = 32;
  pfd.cRedBits        = 8;
  pfd.cRedShift       = 16; 
  pfd.cGreenBits      = 8; 
  pfd.cGreenShift     = 8; 
  pfd.cBlueBits       = 8; 
  pfd.cBlueShift      = 0; 
  pfd.cAlphaBits      = 0; 
  pfd.cAlphaShift     = 0; 
  pfd.cAccumBits      = 64;
  pfd.cAccumRedBits   = 16; 
  pfd.cAccumGreenBits = 16; 
  pfd.cAccumBlueBits  = 16; 
  pfd.cAccumAlphaBits = 16; 
  pfd.cDepthBits      = 16; 
  pfd.cStencilBits    = 0; 
  pfd.cAuxBuffers     = 0; 
  pfd.iLayerType      = PFD_MAIN_PLANE;
  pfd.bReserved       = 0;
  pfd.dwLayerMask     = 0; 
  pfd.dwVisibleMask   = 0; 
  pfd.dwDamageMask    = 0; 

  int pixelformat;
  if ( (pixelformat = ChoosePixelFormat(hDC, &pfd)) == 0 ) 
    return FALSE;

  if (SetPixelFormat(hDC, pixelformat, &pfd) == FALSE) 
    return FALSE;

  int n = ::GetPixelFormat(NULL);
  ::DescribePixelFormat(hDC, n, sizeof(pfd), &pfd);

  return TRUE;
}


// CArMaXCtrl message handlers

int CArMaXCtrl::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
	if (COleControl::OnCreate(lpCreateStruct) == -1)
		return -1;

	// Set pixel format
	m_hDC = ::GetDC(m_hWnd);
  if(SetWindowPixelFormat(m_hDC))
  {
    m_hRC = wglCreateContext(m_hDC);
    if(m_hRC)
      wglMakeCurrent (m_hDC, m_hRC);
    else
      return 0;
  }
  
  InitScene();
 
	return 0;
}


void CArMaXCtrl::OnDestroy()
{
	_ASSERT(m_hDC);
  wglMakeCurrent(m_hDC, m_hRC);

  KillScene();

  wglMakeCurrent(0, 0);
  wglDeleteContext(m_hRC);
  ::ReleaseDC(m_hWnd,m_hDC);
  m_hDC = 0;

	COleControl::OnDestroy();

}

BOOL CArMaXCtrl::OnEraseBkgnd(CDC* pDC)
{
	return TRUE;
}

void CArMaXCtrl::OnSize(UINT nType, int cx, int cy)
{
	COleControl::OnSize(nType, cx, cy);

	glViewport(0,0,cx,cy);
   glMatrixMode( GL_PROJECTION );
   glLoadIdentity();
   glOrtho(-5,5, -5,5, 1,12);
   gluLookAt( 0,0,5, 0,0,0, 0,1,0 );
   glMatrixMode( GL_MODELVIEW );
}

void CArMaXCtrl::InitScene()
{
// Lights properties
  //float ambientProperties[]  = {0.1f, 0.1f, 0.1f, 1.0f};
  float diffuseProperties[]  = {1.0f, 1.0f, 1.0f, 1.0f};
  float specularProperties[] = {1.0f, 1.0f, 1.0f, 1.0f};

  //glLightfv( GL_LIGHT0, GL_AMBIENT, ambientProperties);
  glLightfv( GL_LIGHT0, GL_DIFFUSE,diffuseProperties);
  glLightfv( GL_LIGHT0, GL_SPECULAR, specularProperties);
  glLightModelf(GL_LIGHT_MODEL_TWO_SIDE, 1.0);

  COLORREF backF = GetSysColor(COLOR_BTNFACE);

  /*glClearColor(GetRValue(backF)/255.0,
				GetGValue(backF)/255.0,
				GetBValue(backF)/255.0,
				1.0f);#WARNING*/
  glClearColor((GLclampf)(GetRValue(backF)/255.0),
				(GLclampf)(GetGValue(backF)/255.0),
				(GLclampf)(GetBValue(backF)/255.0),
				(GLclampf)1.0f);


  glHint(GL_LINE_SMOOTH_HINT,GL_FASTEST);

  // Texture
  glEnable(GL_TEXTURE_2D);
  glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
  glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST);


  // Default : lighting

  glEnable(GL_LIGHTING);
  glEnable(GL_LIGHT0);


  // Default : blending
  glEnable(GL_BLEND);
  glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

  glEnable(GL_DEPTH_TEST);
}

//
// KillScene()
// Called when the OpenGL RC is about to be destroyed.
//
void CArMaXCtrl::KillScene()
{
  // TODO: Use KillScene to free resources.
}

//
// DrawScene()
// Called each time the OpenGL scene has to be drawn.
//
void CArMaXCtrl::DrawScene()
{
glPushAttrib(GL_ENABLE_BIT);
  glEnable( GL_BLEND);
  
  glDisable(GL_TEXTURE_2D);
  glDisable(GL_CULL_FACE);

    glEnable(GL_LIGHTING);
    glEnable(GL_LIGHT0);

    glMaterialfv( GL_FRONT, GL_AMBIENT,   gAmbient);
    glMaterialfv( GL_FRONT, GL_DIFFUSE,   gDiffuse);
    glMaterialfv( GL_FRONT, GL_EMISSION,  gEmission);
    glMaterialfv( GL_FRONT, GL_SPECULAR,  gSpecular);
    glMaterialf ( GL_FRONT, GL_SHININESS, gShininnes);

    GLUquadric* qobj = gluNewQuadric(); 
    gluSphere(qobj,4,64,64); 
    
  glPopAttrib();
}

void CArMaXCtrl::SetAmbient(FLOAT r, FLOAT g, FLOAT b)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	gAmbient[0] = r;
	gAmbient[1] = g;
	gAmbient[2] = b;

	Invalidate();
}

void CArMaXCtrl::SetDiffuse(FLOAT r, FLOAT g, FLOAT b)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	gDiffuse[0] = r;
	gDiffuse[1] = g;
	gDiffuse[2] = b;

	Invalidate();
}

void CArMaXCtrl::SetEmission(FLOAT r, FLOAT g, FLOAT b)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	gEmission[0] = r;
	gEmission[1] = g;
	gEmission[2] = b;

	Invalidate();
}

void CArMaXCtrl::SetSpecular(FLOAT r, FLOAT g, FLOAT b)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	gSpecular[0] = r;
	gSpecular[1] = g;
	gSpecular[2] = b;

	Invalidate();
}

void CArMaXCtrl::SetTransparent(FLOAT transparent)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	gAmbient[3] = 1.0f-transparent;
	gDiffuse[3] = 1.0f-transparent;
	gEmission[3] = 1.0f-transparent;
	gSpecular[3] = 1.0f-transparent;

	Invalidate();
}

void CArMaXCtrl::SetShininess(FLOAT shininess)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	gShininnes = shininess;

	Invalidate();
}

void CArMaXCtrl::CopyToClipboard(void)
{
	AFX_MANAGE_STATE(AfxGetStaticModuleState());

	// TODO: Add your dispatch handler code here
}
