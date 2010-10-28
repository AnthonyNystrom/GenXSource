// OpenGLView.cpp : implementation file
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "OpenGLView.h"

#include "..//Drawer.h"

#include "..//ChildFrm.h"
#include "..//RayTracing//RTMaterials.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


static GLfloat g_fMouseZMult	  = 0.01f;	// Mouse 'Z'-Axis multiplier
static GLfloat g_fMouseRotateMult = 0.5f;	// Mouse 'Rotation' multiplier 
static GLfloat g_fMouseScaleMult  = 0.01f;	// Mouse 'Scale' multiplier

#define		SEL_BUF_SIZE	65535
static GLuint  hits_buffer[SEL_BUF_SIZE];

/////////////////////////////////////////////////////////////////////////////
// COpenGLView

IMPLEMENT_DYNCREATE(COpenGLView, CView)

COpenGLView::COpenGLView()
{
	// OpenGL
	m_hGLContext = NULL;
	m_hDC = NULL;
	m_need_release_DC = false;

	m_background = NULL;
	
	// Colors
	m_ClearColorRed   = 1.0f;//0.7f;//191
	m_ClearColorGreen = 1.0f;//0.7f;//191
	m_ClearColorBlue  = 1.0f;//0.7f;//191

	m_rezPointColorRed   = 1.0f;
	m_rezPointColorGreen = 0.0f;
	m_rezPointColorBlue  = 0.0f;

	m_rezLineColorRed   = 0.0f;
	m_rezLineColorGreen = 1.0f;
	m_rezLineColorBlue  = 0.0f;

	m_rezPointSize = 5.0f;
	m_rezLineWidth = 2.0f;

	m_ScreenLeftButtonDownPoint.x = 0;
	m_ScreenLeftButtonDownPoint.y = 0;	

	m_hand_action = HA_ROTATE;

	m_planes = new CPlanes();

	m_transforming_matrix = NULL;

#ifdef _DEBUG
	m_FPSCounter.SetAveraging(FPS_TIME, 1.0); // усреднять значение FPS каждые 0.5 секунд
#endif
}

COpenGLView::~COpenGLView()
{
	delete m_planes;
}


class ComparePixelFormat
{
public:
	bool bSameAccelerated;
	bool bSameDoubleBuffer;
	bool bSameDrawToWindow;
	bool bSameDrawToBitmap;
	bool bSameColorBits;
	bool bSameDepthBits;
	bool bSameStencilBits;
	int iStencilBits;
	int iColorBits;
	int iDepthBits;

	ComparePixelFormat();
	int Score(void);
	bool IsBetter(ComparePixelFormat *pCPF);
};

ComparePixelFormat::ComparePixelFormat()
{
	bSameAccelerated=false;
	bSameDoubleBuffer=false;
	bSameDrawToWindow=false;
	bSameDrawToBitmap=false;
	bSameColorBits=false;
	bSameDepthBits=false;
	bSameStencilBits=false;
	iColorBits=32;
	iDepthBits=32;
	iStencilBits=32;
}

int ComparePixelFormat::Score(void)
{
	int Score=0;

	if (!bSameAccelerated)
		Score+=256;
	if (!bSameDoubleBuffer)
		Score+=256;
	if (!bSameDrawToWindow)
		Score+=256;
	if (!bSameDrawToBitmap)
		Score+=256;
	if ((!bSameColorBits)&&(iColorBits>0))
		Score+=iColorBits*4;
	if ((!bSameDepthBits)&&(iDepthBits>0))
		Score+=iDepthBits*2;
	if ((!bSameStencilBits)&&(iStencilBits>0))
		Score+=iStencilBits;
	return(Score);
}

bool ComparePixelFormat::IsBetter(ComparePixelFormat *pCPF)
{
	int Score1=Score();
	int Score2=pCPF->Score();
	if (Score1<Score2)
		return(true);
	return(false);
}

int FindOptimalPixelFormat(HDC hdc, PIXELFORMATDESCRIPTOR *ppfd)
{
	PIXELFORMATDESCRIPTOR CurrentPFD, UsedPFD;
	ComparePixelFormat CurrentCompare, UsedCompare;
	int iUsedPFD=0;
	int i;

	int iNPixelFormats=DescribePixelFormat(hdc,0,0,NULL);
	for (i=1;i<=iNPixelFormats;i++)
	{
		DescribePixelFormat(hdc,i,sizeof(CurrentPFD),&CurrentPFD);
		CurrentCompare.bSameAccelerated=((CurrentPFD.dwFlags&PFD_GENERIC_FORMAT)==(ppfd->dwFlags&PFD_GENERIC_FORMAT));
		CurrentCompare.bSameDoubleBuffer=((CurrentPFD.dwFlags&PFD_DOUBLEBUFFER)==(ppfd->dwFlags&PFD_DOUBLEBUFFER));
		if (ppfd->dwFlags&PFD_DRAW_TO_WINDOW)
			CurrentCompare.bSameDrawToWindow=(CurrentPFD.dwFlags&PFD_DRAW_TO_WINDOW)?TRUE:FALSE;
		else
			CurrentCompare.bSameDrawToWindow=true;
		if (ppfd->dwFlags&PFD_DRAW_TO_BITMAP)
			CurrentCompare.bSameDrawToBitmap=(CurrentPFD.dwFlags&PFD_DRAW_TO_BITMAP)?TRUE:FALSE;
		else
			CurrentCompare.bSameDrawToBitmap=true;
		CurrentCompare.bSameColorBits=(ppfd->cColorBits==CurrentPFD.cColorBits);
		CurrentCompare.bSameDepthBits=(ppfd->cDepthBits==CurrentPFD.cDepthBits);
		CurrentCompare.bSameStencilBits=(ppfd->cStencilBits==CurrentPFD.cStencilBits);
		CurrentCompare.iColorBits=ppfd->cColorBits-CurrentPFD.cColorBits;
		CurrentCompare.iDepthBits=ppfd->cDepthBits-CurrentPFD.cDepthBits;
		CurrentCompare.iStencilBits=ppfd->cStencilBits-CurrentPFD.cStencilBits;
		if (CurrentCompare.IsBetter(&UsedCompare))
		{
			UsedPFD=CurrentPFD;
			UsedCompare=CurrentCompare;
			iUsedPFD=i;
		}
	}

	PIXELFORMATDESCRIPTOR UsedPFD2;
	int iUsedPFD2=ChoosePixelFormat(hdc,ppfd);
	DescribePixelFormat(hdc,iUsedPFD2,sizeof(UsedPFD2),&UsedPFD2);

	return(iUsedPFD);
}

//********************************************
// SetWindowPixelFormat
//********************************************
BOOL COpenGLView::SetWindowPixelFormat(HDC hDC)
{
	PIXELFORMATDESCRIPTOR pfd;
	pfd.nVersion        = 1;
	pfd.dwFlags         = PFD_DRAW_TO_WINDOW| PFD_SUPPORT_OPENGL| 
		PFD_GENERIC_ACCELERATED| PFD_DOUBLEBUFFER  | PFD_SWAP_EXCHANGE;
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
	PIXELFORMATDESCRIPTOR *pPFD=&pfd;

	if ((pixelformat=FindOptimalPixelFormat(hDC,pPFD))==0)
	{
		ASSERT(0);
		return FALSE;
	}
	if (SetPixelFormat(hDC,pixelformat,pPFD)==FALSE)
	{
		ASSERT(0);
		return FALSE;
	}

	return TRUE;
}

BEGIN_MESSAGE_MAP(COpenGLView, CView)
	//{{AFX_MSG_MAP(COpenGLView)
	ON_WM_CREATE()
	ON_WM_DESTROY()
	ON_WM_SIZE()
	ON_WM_ERASEBKGND()
	ON_WM_PAINT()
	//}}AFX_MSG_MAP
	ON_MESSAGE( WM_ENTERSIZEMOVE, OnEnterSizeMove) 
	ON_MESSAGE( WM_EXITSIZEMOVE, OnExitSizeMove)
	ON_COMMAND(ID_PARALL_PROJ, OnParallProj)
	ON_UPDATE_COMMAND_UI(ID_PARALL_PROJ, OnUpdateParallProj)
	ON_COMMAND(ID_PERSPECT_PROJ, OnPerspectProj)
	ON_UPDATE_COMMAND_UI(ID_PERSPECT_PROJ, OnUpdatePerspectProj)
	ON_COMMAND(ID_FRONT_VIEW, OnFrontView)
	ON_UPDATE_COMMAND_UI(ID_FRONT_VIEW, OnUpdateFrontView)
	ON_COMMAND(ID_BACK_VIEW, OnBackView)
	ON_UPDATE_COMMAND_UI(ID_BACK_VIEW, OnUpdateBackView)
	ON_COMMAND(ID_TOP_VIEW, OnTopView)
	ON_UPDATE_COMMAND_UI(ID_TOP_VIEW, OnUpdateTopView)
	ON_COMMAND(ID_BOTTOM_VIEW, OnBottomView)
	ON_UPDATE_COMMAND_UI(ID_BOTTOM_VIEW, OnUpdateBottomView)
	ON_COMMAND(ID_LEFT_VIEW, OnLeftView)
	ON_UPDATE_COMMAND_UI(ID_LEFT_VIEW, OnUpdateLeftView)
	ON_COMMAND(ID_RIGHT_VIEW, OnRightView)
	ON_UPDATE_COMMAND_UI(ID_RIGHT_VIEW, OnUpdateRightView)
	ON_COMMAND(ID_ISO_FRONT_VIEW, OnIsoFrontView)
	ON_UPDATE_COMMAND_UI(ID_ISO_FRONT_VIEW, OnUpdateIsoFrontView)
	ON_COMMAND(ID_ISO_BACK_VIEW, OnIsoBackView)
	ON_UPDATE_COMMAND_UI(ID_ISO_BACK_VIEW, OnUpdateIsoBackView)
	ON_WM_TIMER()
	ON_COMMAND(ID_MOVE_EYE_HAND, OnMoveEyeHand)
	ON_UPDATE_COMMAND_UI(ID_MOVE_EYE_HAND, OnUpdateMoveEyeHand)
	ON_COMMAND(ID_ROTATE_EYE_HAND, OnRotateEyeHand)
	ON_UPDATE_COMMAND_UI(ID_ROTATE_EYE_HAND, OnUpdateRotateEyeHand)
	ON_COMMAND(ID_ZOOM_EYE_HAND, OnZoomEyeHand)
	ON_UPDATE_COMMAND_UI(ID_ZOOM_EYE_HAND, OnUpdateZoomEyeHand)
	ON_COMMAND(ID_MOVE_LEFT_AUTO, OnMoveLeftAuto)
	ON_COMMAND(ID_MOVE_RIGHT_AUTO, OnMoveRightAuto)
	ON_COMMAND(ID_MOVE_UP_AUTO, OnMoveUpAuto)
	ON_COMMAND(ID_MOVE_DOWN_AUTO, OnMoveDownAuto)
	ON_COMMAND(ID_ROTATE_X_AUTO, OnRotateXAuto)
	ON_COMMAND(ID_ROTATE_Y_AUTO, OnRotateYAuto)
	ON_COMMAND(ID_ROTATE_Z_AUTO, OnRotateZAuto)
	ON_COMMAND(ID_ZOOM_PLUS_AUTO, OnZoomPlusAuto)
	ON_COMMAND(ID_ZOOM_MINUS_AUTO, OnZoomMinusAuto)
	ON_COMMAND(ID_COPY_AS_RASTR, OnCopyAsRastr)
	ON_COMMAND(ID_COPY_AS_VECTOR, OnCopyAsVector)
	ON_COMMAND(ID_SAVE_AS_RASTR, OnSaveAsRastr)
	ON_COMMAND(ID_SAVE_AS_VECTOR, OnSaveAsVector)
	ON_COMMAND(ID_TRANSLATE, OnTranslate)
	ON_UPDATE_COMMAND_UI(ID_TRANSLATE, OnUpdateTranslate)
	ON_COMMAND(ID_ROTATE, OnRotate)
	ON_UPDATE_COMMAND_UI(ID_ROTATE, OnUpdateRotate)
	ON_COMMAND(ID_ALL_SCENE_VIEW, OnAllSceneView)
	ON_UPDATE_COMMAND_UI(ID_ALL_SCENE_VIEW, OnUpdateAllSceneView)
	ON_COMMAND(ID_CREATE_GROUP, OnCreateGroup)
	ON_UPDATE_COMMAND_UI(ID_CREATE_GROUP, OnUpdateCreateGroup)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// COpenGLView diagnostics

#ifdef _DEBUG
void COpenGLView::AssertValid() const
{
	CView::AssertValid();
}

void COpenGLView::Dump(CDumpContext& dc) const
{
	CView::Dump(dc);
}
#endif //_DEBUG

/////////////////////////////////////////////////////////////////////////////
// COpenGLView message handlers
/*void set_vsync(bool enabled) // true -- VSync включен, false -- выключен
{
	PFNWGLSWAPINTERVALEXTPROC wglSwapInterval = NULL;

	wglSwapInterval = (PFNWGLSWAPINTERVALEXTPROC) wglGetProcAddress("wglSwapIntervalEXT");
	if ( wglSwapInterval ) wglSwapInterval(enabled ? 1 : 0);
}*/

int COpenGLView::OnCreate(LPCREATESTRUCT lpCreateStruct) 
{
	if(CView::OnCreate(lpCreateStruct) == -1)
		return -1;

	m_hDC = ::GetDC(GetSafeHwnd());
	m_need_release_DC = true;

	if(SetWindowPixelFormat(m_hDC))
	{
		m_hGLContext = wglCreateContext(m_hDC);
		if(m_hGLContext)
			wglMakeCurrent (m_hDC, m_hGLContext);
		else
			return -1;
	}
	
	// Default mode
/*	glPolygonMode(GL_FRONT,GL_FILL);
	glPolygonMode(GL_BACK,GL_FILL);*/

//  glShadeModel(GL_FLAT);
//	glEnable(GL_NORMALIZE);
	
	// Lights properties
  //float	ambientProperties[]  = {0.1f, 0.1f, 0.1f, 1.0f};
	float	diffuseProperties[]  = {1.0f, 1.0f, 1.0f, 1.0f};
  float	specularProperties[] = {1.0f, 1.0f, 1.0f, 1.0f};
	
  //glLightfv( GL_LIGHT0, GL_AMBIENT, ambientProperties);
  glLightfv( GL_LIGHT0, GL_DIFFUSE,diffuseProperties);
  glLightfv( GL_LIGHT0, GL_SPECULAR, specularProperties);
  glLightModelf(GL_LIGHT_MODEL_TWO_SIDE, 1.0);

	glClearColor(m_ClearColorRed,m_ClearColorGreen,m_ClearColorBlue,1.0f);

	glHint(GL_LINE_SMOOTH_HINT,GL_FASTEST);

	// Texture
	glEnable(GL_TEXTURE_2D);
	glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
	glHint(GL_PERSPECTIVE_CORRECTION_HINT, GL_NICEST);


	// Default : lighting
	//if(IsExtEnable("GL_ARB_point_parameters"))
	{
		glEnable(GL_LIGHTING);
		glEnable(GL_LIGHT0);
	}
	

	// Default : blending
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	// Default : material
 /* float	MatAmbient[]  = {0.0f, 0.5f, 0.75f, 1.0f};
  float	MatDiffuse[]  = {0.0f, 0.5f, 1.0f, 1.0f};
  float	MatSpecular[]  = {0.2f, 0.2f, 0.2f, 1.0f};
  float	MatShininess[]  = { 64 };
  float	MatEmission[]  = {0.0f, 0.0f, 0.0f, 1.0f};
  	glMaterialfv(GL_FRONT_AND_BACK,GL_AMBIENT,MatAmbient);
	glMaterialfv(GL_FRONT_AND_BACK,GL_DIFFUSE,MatDiffuse);
	glMaterialfv(GL_FRONT_AND_BACK,GL_SPECULAR,MatSpecular);
	glMaterialfv(GL_FRONT_AND_BACK,GL_SHININESS,MatShininess);
	glMaterialfv(GL_FRONT_AND_BACK,GL_EMISSION,MatEmission);*/

	glEnable(GL_DEPTH_TEST);
//	glEnable(GL_CULL_FACE);

	m_background    = new CBackground();

    if (IsExtEnable("GL_ARB_vertex_buffer_object"))
		Drawer::is_VBO_Supported = true;
	else
		Drawer::is_VBO_Supported = false;

	return 1;
}

void COpenGLView::OnDestroy() 
{
	if(wglGetCurrentContext() != NULL)
		wglMakeCurrent(NULL,NULL);
	
	if(m_background)
		delete m_background;

	if(m_hGLContext != NULL)
	{
		wglDeleteContext(m_hGLContext);
		m_hGLContext = NULL;
	}
	if (m_need_release_DC)
		::ReleaseDC(GetSafeHwnd(), m_hDC);

	CView::OnDestroy();
}

LRESULT COpenGLView::OnEnterSizeMove(WPARAM,LPARAM)
{
	return 0L;
}

LRESULT COpenGLView::OnExitSizeMove(WPARAM,LPARAM)
{
	return 0L;
}

void COpenGLView::OnSize(UINT nType, int cx, int cy) 
{
	CView::OnSize(nType, cx, cy);	
	wglMakeCurrent(m_hDC,m_hGLContext);
	m_Camera.ResetView(cx, cy);
}

BOOL COpenGLView::OnEraseBkgnd(CDC* pDC) 
{
	// TODO: Add your message handler code here and/or call default
	return TRUE;
	//return CView::OnEraseBkgnd(pDC);
}

void COpenGLView::OnActivateView(BOOL bActivate, CView* pActivateView, CView* pDeactiveView) 
{
	wglMakeCurrent(m_hDC,m_hGLContext);
	CView::OnActivateView(bActivate, pActivateView, pDeactiveView);
}

void COpenGLView::OnDraw(CDC* pDC) 
{
}

void COpenGLView::ZoomRect(const CRect &rcClient,const CRect& rcZoom)
{

}

void COpenGLView::OnPaint() 
{
	// Device context for painting
	CPaintDC dc(this); 
	
	wglMakeCurrent(m_hDC,m_hGLContext);
	
	// Clear the buffers
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	// Set the matrix mode to MODELVIEW and load
	// the identity matrix
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	// Set our camera position and save its transformation matrix
	m_Camera.PositionCamera();

	if(m_background)
	{	
		/*SBACKGROUND_PARAMS pb;
		m_background->GetParams(&pb);
		pb.texture_file = "C:\\DISCO (D)\\PROJ\\lavori\\Ant\\NuGenDimension\\NuGenDimension\\NuGenDimension\\Tramonto.bmp";
		m_background->SetParams(&pb);*/
		m_background->Display();
	}
	

	// Start rendering...
	DrawScene(GL_RENDER);

	m_planes->DrawWorkPlanes(&m_Camera,(static_cast<CChildFrame*>(GetParentFrame())->m_commander)?true:false);


	glPushAttrib(GL_ENABLE_BIT|GL_LINE_BIT|GL_POINT_BIT|
		GL_CURRENT_BIT|GL_LIGHTING_BIT|GL_COLOR_BUFFER_BIT);
	glDisable(GL_LIGHTING);
	glDisable(GL_TEXTURE_2D);
	glEnable(GL_POINT_SMOOTH);
	glLineWidth((GLfloat)m_rezLineWidth);
	glPointSize((GLfloat)m_rezPointSize);
		DrawFromCommander();
	glPopAttrib();
	
	glPopMatrix();
	glFlush();
	glFinish();
	SwapBuffers(m_hDC);
/*
#ifdef _DEBUG
	m_FPSCounter.AddFrame(); // говорим счетчику, что готов очередной кадр
	double dFPS=m_FPSCounter.GetFPS(); // получаем текущее значение FPS
	CString fps_str;
	fps_str.Format("%f",dFPS);
	(static_cast<CChildFrame*>(GetParentFrame()))->PutMessage(IApplicationInterface::MT_MESSAGE,fps_str);
#endif
	*/
}

int  COpenGLView::ReadPixels(unsigned char** pixels, int* w, int* h, int *rowlen)  
{
	wglMakeCurrent(m_hDC, m_hGLContext);

	unsigned char* pixelPTR = *pixels; // for  framebuffer pixels storage
	int viewportParm[4]={0, 0, 0, 0};

	*w = viewportParm[2];
	*h = viewportParm[3];

	glGetIntegerv(GL_VIEWPORT, viewportParm);
	*rowlen =(viewportParm[2]*3 +3) & ~3;
	if ((viewportParm[2]> *w) ||(viewportParm[3]> *h))
	{
		if (!pixelPTR)
			pixelPTR = (unsigned char *)calloc(*rowlen * viewportParm[3], sizeof(unsigned char));
		else
			pixelPTR = (unsigned char *)realloc(pixelPTR, *rowlen * viewportParm[3]* sizeof(unsigned char));
	}

	*w = viewportParm[2];
	*h = viewportParm[3];

//#ifdef BGR
	glReadPixels(0,0, *w, *h, GL_BGR_EXT, GL_UNSIGNED_BYTE, pixelPTR);
//#else*
	//glReadPixels(0,0, *w, *h, GL_RGB, GL_UNSIGNED_BYTE, pixelPTR);
//#endif */
	*pixels = pixelPTR;
	return 1;
}

int COpenGLView::ReadAndWriteToBmp(CString path)
{
	FILE *nmf;
	char buffer[40];
	int i, c;
	unsigned char *buf;

	unsigned char* m_pixelPTR = NULL; // for  framebuffer pixels storage
	int m_pixel_width;
	int m_pixel_height;
	int rowlen;

	if(ReadPixels(&m_pixelPTR, &m_pixel_width, &m_pixel_height, &rowlen)!=1)
		return 0;

	//nmf = fopen(path, "wb");#OBSOLETE
	fopen_s(&nmf,path, "wb");

	*((short *)&buffer[0])   = 19778;
	//*((long  *)&buffer[2])   = m_pixel_width*m_pixel_height*3 + 54;
	*((long  *)&buffer[2])   = rowlen*m_pixel_height + 54;
	*((short *)&buffer[6])   = 0;
	*((short *)&buffer[8])   = 0;
	*((long  *)&buffer[10]) = 54;

	fwrite(buffer,1,14,nmf);

	*((long  *)&buffer[0])   = 40;
	*((long  *)&buffer[4])   = m_pixel_width;
	*((long  *)&buffer[8])   = m_pixel_height;
	*((short *)&buffer[12])  = 1; // = 1;
	*((short *)&buffer[14])  = 24;
	*((long  *)&buffer[16])  = 0;
	*((long  *)&buffer[20])  = 0;
	*((long  *)&buffer[24])  = 0;
	*((long  *)&buffer[28])  = 0;
	*((long  *)&buffer[32])  = 0;
	*((long  *)&buffer[36])  = 0;

	fwrite(buffer,1,40,nmf);

	c = rowlen % 4;//3*m_pixel_width % 4;
	c = 4 - c;
	if(c == 4)c=0;

	buf = m_pixelPTR;

	for(i=0;i<m_pixel_height;i++)
	{
		// read actual data
		//fwrite(buf, 1, m_pixel_width*3, nmf);
		fwrite(buf, 1, rowlen, nmf);

		// write crap
		if(c)fwrite(buffer, 1, c, nmf);
		// adjust buffer
		buf += rowlen;//m_pixel_width*3;
	}


	fclose(nmf);

	if(m_pixelPTR)
	{
		delete[] m_pixelPTR;
		m_pixelPTR = NULL;
	}

	return 1;
}

/************************************************************************/
/*   IViewPort Interface                                                */
/************************************************************************/

void	COpenGLView::InvalidateViewPort()
{
	Invalidate(FALSE);
}

void	COpenGLView::UnProjectScreenPoint(int scrX,int scrY, double scrZ, SG_POINT& wrldP)
{
	SG_VECTOR tmpVec;
	m_Camera.GetWorldCoord(scrX, scrY, scrZ , tmpVec);
	wrldP.x = tmpVec.x;  
	wrldP.y = tmpVec.y;
	wrldP.z = tmpVec.z;
}

void	COpenGLView::ProjectWorldPoint(const SG_POINT& wrldP, double& scrX,  double& scrY, double& scrZ)
{
	m_Camera.GetScreenCoord(wrldP.x, wrldP.y, wrldP.z, scrX, scrY, scrZ);
}

IViewPort::VIEW_PORT_VIEW_TYPE  COpenGLView::GetViewPortViewType()
{
	switch(m_Camera.m_enumCameraPosition) 
	{
	case CP_USER_POSITION:
	case CP_ISO_FRONT:
	case CP_ISO_BACK:
		return IViewPort::USER_VIEW;
	case CP_FRONT:
		return IViewPort::FRONT_VIEW;
	case CP_BACK:
		return IViewPort::BACK_VIEW;
	case CP_TOP:
		return IViewPort::TOP_VIEW;
	case CP_BOTTOM:
		return IViewPort::BOTTOM_VIEW;
	case CP_LEFT:
		return IViewPort::LEFT_VIEW;
	case CP_RIGHT:
		return IViewPort::RIGHT_VIEW;
	default:
		ASSERT(0);
		return IViewPort::USER_VIEW;
	}
	return IViewPort::USER_VIEW;
}

void	COpenGLView::GetViewPortNormal(SG_VECTOR& viewNorm)
{
	SG_VECTOR kudaSmotrim, otkudaSmotrim;
	m_Camera.GetLookAtPos(&kudaSmotrim.x,&kudaSmotrim.y,&kudaSmotrim.z);
	m_Camera.GetEyePos(&otkudaSmotrim.x,&otkudaSmotrim.y,&otkudaSmotrim.z);
	viewNorm = sgSpaceMath::VectorsSub(kudaSmotrim,otkudaSmotrim);
	sgSpaceMath::NormalVector(viewNorm);
}

int COpenGLView::GetSnapSize()
{
	return  (static_cast<CChildFrame*>(GetParentFrame())->GetSnapSize());
}

SELECT_BUFFER   COpenGLView::GetHitsInRect(const CRect& hitsRect, bool selSubObj)
{
	int			name_background = -1;
	GLint		vp[4], nhits;
	double		CurProj[16];

	int  yPos = ((hitsRect.top+hitsRect.bottom)/2);
	int  xPos = ((hitsRect.left+hitsRect.right)/2);

	memset(hits_buffer,0,SEL_BUF_SIZE*sizeof(GLuint));
	glSelectBuffer(SEL_BUF_SIZE, hits_buffer);

	glGetIntegerv(GL_VIEWPORT, vp);
	glMatrixMode (GL_PROJECTION);
	glGetDoublev(GL_PROJECTION_MATRIX,CurProj);
	glPushMatrix ();
	
	glRenderMode (GL_SELECT);
	glInitNames();
	
	glPushName(name_background);
	yPos = vp[3]-yPos;
	glLoadIdentity ();
	gluPickMatrix(xPos, yPos, hitsRect.Width(), hitsRect.Height(),vp);

	glMultMatrixd(CurProj);

	glMatrixMode(GL_MODELVIEW);

	glScissor(xPos-hitsRect.Width()/2, yPos-hitsRect.Height()/2, hitsRect.Width(), hitsRect.Height());
	glEnable(GL_SCISSOR_TEST);
	DrawScene(GL_SELECT,selSubObj);
	glDisable(GL_SCISSOR_TEST);

	nhits = glRenderMode(GL_RENDER);

	glMatrixMode (GL_PROJECTION);
	glPopMatrix ();
	glMatrixMode(GL_MODELVIEW);

	SELECT_BUFFER resultat;
	memset(&resultat,0,sizeof(SELECT_BUFFER));
	resultat.nhits = nhits;
	resultat.buffer = hits_buffer;
	return resultat;
}

sgCObject*	 COpenGLView::GetTopObject(SELECT_BUFFER hitBuf)
{
	int		  name_background = -1;
	const int pick_maxz = 0xffffffff;
	GLint     hit = name_background;
	GLuint    minz;
	GLint     i,j;
	GLint	  nnames;

	sgCObject* resObj = NULL;

	minz = pick_maxz;
	for (i = j = 0; j < hitBuf.nhits; j++) 
	{
		nnames = hitBuf.buffer[i];
		i++;
		if (hitBuf.buffer[i] < minz) 
		{
			minz = hitBuf.buffer[i];
			hit = (GLint)hitBuf.buffer[i + 1 + nnames];
		} 
		i++;
		i += nnames + 1;
	}
	if (hit>0)
	{
		resObj = reinterpret_cast<sgCObject*>(hit);
	}	

	return resObj;
}

sgCObject*	 COpenGLView::GetTopObjectByType(SELECT_BUFFER hitBuf, SG_OBJECT_TYPE ob_type)
{
	int		  name_background = -1;
	const int pick_maxz = 0xffffffff;
	GLint     hit = name_background;
	GLuint    minz;
	GLint     i,j;
	GLint	  nnames;

	sgCObject* resObj = NULL;

	minz = pick_maxz;
	for (i = j = 0; j < hitBuf.nhits; j++) 
	{
		nnames = hitBuf.buffer[i];
		i++;
		
		hit = (GLint)hitBuf.buffer[i + 1 + nnames];
		if (hit!=name_background)
		{
			sgCObject* tmp_obj = reinterpret_cast<sgCObject*>(hit);
			if (tmp_obj->GetType()==ob_type && 
				hitBuf.buffer[i]<minz)
			{
				resObj = tmp_obj;
				minz = hitBuf.buffer[i];
			}
		}
		 
		i++;
		i += nnames + 1;
	}

	return resObj;
}

bool  COpenGLView::IsOnAnyObject(SELECT_BUFFER hitBuf)
{
	return (hitBuf.nhits>0);	
}

void  COpenGLView::GetPointAfterFixing(int scrX, int scrY, 
									   bool xFix,
									   bool yFix,
									   bool zFix,
									   const SG_POINT& fixP, SG_POINT& wrldP)
{
	ASSERT(xFix || yFix || zFix);

	if (xFix && yFix && zFix)
	{
		wrldP = fixP;
		return;
	}

	SG_POINT  onAxePnt;
	UnProjectScreenPoint(scrX,scrY,0.0,onAxePnt);
	SG_POINT  begAxePnt;
	m_Camera.GetEyePos(&begAxePnt.x,&begAxePnt.y,&begAxePnt.z);
	SG_VECTOR naprCos;
	naprCos.x = onAxePnt.x-begAxePnt.x;
	naprCos.y = onAxePnt.y-begAxePnt.y;
	naprCos.z = onAxePnt.z-begAxePnt.z;
	sgSpaceMath::NormalVector(naprCos);

	if (xFix && !yFix && !zFix)
	{
		SG_VECTOR plN;
		plN.x = 1.0;
		plN.y = plN.z = 0.0;
		sgSpaceMath::IntersectPlaneAndLine(plN, fixP.x, 
												begAxePnt, naprCos,
												wrldP);
		return;
	}

	if (!xFix && yFix && !zFix)
	{
		SG_VECTOR plN;
		plN.y = 1.0;
		plN.x = plN.z = 0.0;
		sgSpaceMath::IntersectPlaneAndLine(plN, fixP.y, 
												begAxePnt, naprCos,
												wrldP);
		return;
	}

	if (!xFix && !yFix && zFix)
	{
		SG_VECTOR plN;
		plN.z = 1.0;
		plN.x = plN.y = 0.0;
		sgSpaceMath::IntersectPlaneAndLine(plN, fixP.z, 
												begAxePnt, naprCos,
												wrldP);
		return;
	}

	if (xFix && yFix && !zFix)
	{
		SG_VECTOR lnNC;
		lnNC.z = 1.0;
		lnNC.x = lnNC.y = 0.0;
		ProjectScreenPointOnLine(scrX,scrY,fixP, lnNC, wrldP);
		/*sgSpaceMath::ProjectPointToLineAndGetDist(const_cast<SG_POINT*>(&fixP), &lnNC, 
													&onAxePnt,	&wrldP);*/
		return;
	}

	if (!xFix && yFix && zFix)
	{
		SG_VECTOR lnNC;
		lnNC.x = 1.0;
		lnNC.y = lnNC.z = 0.0;
		ProjectScreenPointOnLine(scrX,scrY,fixP, lnNC, wrldP);
		/*sgSpaceMath::ProjectPointToLineAndGetDist(const_cast<SG_POINT*>(&fixP), &lnNC, 
													&onAxePnt,	&wrldP);*/
		return;
	}

	if (xFix && !yFix && zFix)
	{
		SG_VECTOR lnNC;
		lnNC.y = 1.0;
		lnNC.x = lnNC.z = 0.0;
		ProjectScreenPointOnLine(scrX,scrY,fixP, lnNC, wrldP);
		/*sgSpaceMath::ProjectPointToLineAndGetDist(const_cast<SG_POINT*>(&fixP), &lnNC, 
													&onAxePnt,	&wrldP);*/
		return;
	}


}

#include "..//ChildFrm.h"
#include ".\openglview.h"

void Okruglenie(SG_POINT& wrldPnt, float precision)
{
	 wrldPnt.x = precision*((int)((wrldPnt.x/precision + ((wrldPnt.x>0)?0.5:-0.5))));
	 wrldPnt.y = precision*((int)((wrldPnt.y/precision + ((wrldPnt.y>0)?0.5:-0.5))));
	 wrldPnt.z = precision*((int)((wrldPnt.z/precision + ((wrldPnt.z>0)?0.5:-0.5))));
}
#include "..//NuGenDimensionDoc.h"
void  COpenGLView::GetWorldPointAfterSnap(const GET_SNAP_IN& in_arg,
										  GET_SNAP_OUT& out_arg)
{
	CChildFrame*  fr = static_cast<CChildFrame*>(GetParentFrame());

	SCENE_SETUPS tmpSS;
	reinterpret_cast<CNuGenDimensionDoc*>(GetDocument())->GetSceneSetups(tmpSS);

	if (in_arg.XFix || in_arg.YFix || in_arg.ZFix)
	{
		GetPointAfterFixing(in_arg.scrX, in_arg.scrY, 
							in_arg.XFix,
							in_arg.YFix,
							in_arg.ZFix,
							in_arg.FixPoint, 
								out_arg.result_point);
		Okruglenie(out_arg.result_point,
			precisions[tmpSS.CurrentPrecision]);
		m_planes->SetUsingPlanePoint(false);
		out_arg.isOnWorkPlane = false;
		return;
	}

	if (in_arg.snapType==SNAP_NO)
	{
		UnProjectScreenPoint(in_arg.scrX, in_arg.scrY, 0.0, out_arg.result_point);
		Okruglenie(out_arg.result_point,
			precisions[tmpSS.CurrentPrecision]);
		m_planes->SetUsingPlanePoint(false);
		out_arg.isOnWorkPlane = false;
		return;
	}


	SG_POINT  point_on_work_plane;
	memset(&point_on_work_plane,0,sizeof(SG_POINT));

	SG_POINT  onAxePnt;
	UnProjectScreenPoint(in_arg.scrX, in_arg.scrY, 0.0, onAxePnt);
	SG_POINT  begAxePnt;
	m_Camera.GetEyePos(&begAxePnt.x,&begAxePnt.y,&begAxePnt.z);
	SG_VECTOR naprCos;
	SG_POINT* snapP = NULL;
	if (m_Camera.m_bPerspective)
	{
		naprCos.x = onAxePnt.x-begAxePnt.x;
		naprCos.y = onAxePnt.y-begAxePnt.y;
		naprCos.z = onAxePnt.z-begAxePnt.z;
		sgSpaceMath::NormalVector(naprCos);
		snapP =m_planes->GetPointOnWorkPlanes(&begAxePnt,&naprCos);
	}
	else
	{
		GetViewPortNormal(naprCos);
		SG_POINT tP;
		tP.x = onAxePnt.x - m_Camera.GetFocalLength()*naprCos.x;
		tP.y = onAxePnt.y - m_Camera.GetFocalLength()*naprCos.y;
		tP.z = onAxePnt.z - m_Camera.GetFocalLength()*naprCos.z;
		snapP =m_planes->GetPointOnWorkPlanes(&tP,&naprCos);
	}

	if (snapP)
		point_on_work_plane = *snapP;
	else
		point_on_work_plane = onAxePnt;

	

	Okruglenie(point_on_work_plane,precisions[tmpSS.CurrentPrecision]);

	SNAP_TYPE  snType = (in_arg.snapType==SNAP_SYSTEM)?fr->GetSnapType():in_arg.snapType;

	if (snType==SNAP_NO)
	{
		out_arg.result_point = point_on_work_plane;
		if (snapP)
		{
			m_planes->SetUsingPlanePoint(true);
			out_arg.isOnWorkPlane = true;
			m_planes->GetCurrentWorkPlane(out_arg.snapWorkPlaneD, 
				out_arg.snapWorkPlaneNormal);
		}
		else
		{
			m_planes->SetUsingPlanePoint(false);
			out_arg.isOnWorkPlane = false;
		}
		return;
	}



	SELECT_BUFFER selBuf;
	sgCObject*    selObj = NULL;
	sgC2DObject*  ob2D = NULL;

	int snapSz = fr->GetSnapSize();

	selBuf = GetHitsInRect(CRect(in_arg.scrX-snapSz, in_arg.scrY-snapSz,
		in_arg.scrX+snapSz, in_arg.scrY+snapSz),true);

	switch(snType) 
	{
	case SNAP_NO:
	case SNAP_SYSTEM:
		ASSERT(0);
		return;
	case SNAP_POINTS:
		selObj = GetTopObjectByType(selBuf,SG_OT_POINT);  // point
		if (selObj==NULL)
		{
			out_arg.result_point = point_on_work_plane;
			if (snapP)
			{
				m_planes->SetUsingPlanePoint(true);
				out_arg.isOnWorkPlane = true;
				m_planes->GetCurrentWorkPlane(out_arg.snapWorkPlaneD, 
					out_arg.snapWorkPlaneNormal);
			}
			else
			{
				m_planes->SetUsingPlanePoint(false);
				out_arg.isOnWorkPlane = false;
			}
			return;
		}
		else
		{
			out_arg.result_point = *(reinterpret_cast<sgCPoint*>(selObj)->GetGeometry());
			m_planes->SetUsingPlanePoint(false);
			out_arg.isOnWorkPlane = false;
			return;
		}
		break;
	case SNAP_ENDS:
		selObj = GetTopObject(selBuf); 
		if (selObj &&
			(selObj->GetType()==SG_OT_LINE ||
			selObj->GetType()==SG_OT_SPLINE ||
			selObj->GetType()==SG_OT_ARC ||
			selObj->GetType()==SG_OT_CONTOUR))
				ob2D = reinterpret_cast<sgC2DObject*>(selObj);
		if (selObj==NULL || ob2D==NULL || (ob2D && ob2D->IsClosed()))
		{
			out_arg.result_point = point_on_work_plane;
			if (snapP)
			{
				m_planes->SetUsingPlanePoint(true);
				out_arg.isOnWorkPlane = true;
				m_planes->GetCurrentWorkPlane(out_arg.snapWorkPlaneD, 
					out_arg.snapWorkPlaneNormal);
			}
			else
			{
				m_planes->SetUsingPlanePoint(false);
				out_arg.isOnWorkPlane = false;
			}
			return;
		}
		else
			{
				GLdouble    winX1,winY1,winZ1;
				GLdouble    winX2,winY2,winZ2;
				SG_POINT    endP1;
				SG_POINT    endP2;
				ASSERT(ob2D);
				endP1 = ob2D->GetPointFromCoefficient(0.0);
				endP2 = ob2D->GetPointFromCoefficient(1.0);
				ProjectWorldPoint(endP1,winX1,winY1,winZ1);
				ProjectWorldPoint(endP2,winX2,winY2,winZ2);
				double d1;
				d1 = sqrt((winX1-in_arg.scrX)*(winX1-in_arg.scrX)+
					(winY1-in_arg.scrY)*(winY1-in_arg.scrY));
				double d2;
				d2 = sqrt((winX2-in_arg.scrX)*(winX2-in_arg.scrX)+
					(winY2-in_arg.scrY)*(winY2-in_arg.scrY));
				if (d1<d2)
					out_arg.result_point = endP1;
				else
					out_arg.result_point = endP2;
				m_planes->SetUsingPlanePoint(false);
				out_arg.isOnWorkPlane = false;
				return;
			}
		break;
	case SNAP_MIDS:
		selObj = GetTopObjectByType(selBuf,SG_OT_LINE);  // line
		if (selObj==NULL)
		{
			out_arg.result_point = point_on_work_plane;
			if (snapP)
			{
				m_planes->SetUsingPlanePoint(true);
				out_arg.isOnWorkPlane = true;
				m_planes->GetCurrentWorkPlane(out_arg.snapWorkPlaneD, 
					out_arg.snapWorkPlaneNormal);
			}
			else
			{
				m_planes->SetUsingPlanePoint(false);
				out_arg.isOnWorkPlane = false;
			}
			return;
		}
		else
		{
			const SG_LINE* lineGeo = reinterpret_cast<sgCLine*>(selObj)->GetGeometry();
			out_arg.result_point.x = (lineGeo->p1.x+lineGeo->p2.x)*0.5;
			out_arg.result_point.y = (lineGeo->p1.y+lineGeo->p2.y)*0.5;
			out_arg.result_point.z = (lineGeo->p1.z+lineGeo->p2.z)*0.5;
			m_planes->SetUsingPlanePoint(false);
			out_arg.isOnWorkPlane = false;
			return;
		}
		break;
	case SNAP_CENTERS:
		selObj = GetTopObject(selBuf);
		if (selObj==NULL || (selObj->GetType()!=SG_OT_CIRCLE && 
							selObj->GetType()!=SG_OT_ARC))
		{
				out_arg.result_point = point_on_work_plane;
				if (snapP)
				{
					m_planes->SetUsingPlanePoint(true);
					out_arg.isOnWorkPlane = true;
					m_planes->GetCurrentWorkPlane(out_arg.snapWorkPlaneD, 
						out_arg.snapWorkPlaneNormal);
				}
				else
				{
					m_planes->SetUsingPlanePoint(false);
					out_arg.isOnWorkPlane = false;
				}
				return;
		}
		else
		{
			if (selObj->GetType()==SG_OT_CIRCLE)
			{
				out_arg.result_point = reinterpret_cast<sgCCircle*>(selObj)->GetGeometry()->center;
				m_planes->SetUsingPlanePoint(false);
				out_arg.isOnWorkPlane = false;
				return;
			}
			if (selObj->GetType()==SG_OT_ARC)
			{
				out_arg.result_point = reinterpret_cast<sgCArc*>(selObj)->GetGeometry()->center;
				m_planes->SetUsingPlanePoint(false);
				out_arg.isOnWorkPlane = false;
				return;
			}
		}
		break;
	default:
		ASSERT(0);
		return;
	}
}

bool  COpenGLView::ProjectScreenPointOnPlane(int scrX, int scrY, 
													SG_VECTOR& plNorm, double plD, 
													SG_POINT& resPnt)
{
	memset(&resPnt,0,sizeof(SG_POINT));

	SG_POINT  onAxePnt;
	UnProjectScreenPoint(scrX, scrY, 0.0, onAxePnt);
	SG_POINT  begAxePnt;
	m_Camera.GetEyePos(&begAxePnt.x,&begAxePnt.y,&begAxePnt.z);
	SG_VECTOR naprCos;
	naprCos.x = onAxePnt.x-begAxePnt.x;
	naprCos.y = onAxePnt.y-begAxePnt.y;
	naprCos.z = onAxePnt.z-begAxePnt.z;
	sgSpaceMath::NormalVector(naprCos);

	if (sgSpaceMath::IntersectPlaneAndLine(plNorm, plD, 
							begAxePnt, naprCos,
								resPnt)==1)
		return true;
	else
		return false;
}

bool COpenGLView::ProjectScreenPointOnLine(int scrX,int scrY,
										   const SG_POINT& lineP, const SG_VECTOR& lineDir, 
										   SG_POINT& resPnt)
{
	memset(&resPnt,0,sizeof(SG_POINT));
    
	SG_POINT  onAxePnt;
	SG_POINT  onAxePnt1;
	SG_POINT  onAxePnt2;

	ProjectWorldPoint(lineP,onAxePnt1.x,onAxePnt1.y,onAxePnt1.z);
	onAxePnt.x = lineP.x+lineDir.x*100.0;
	onAxePnt.y = lineP.y+lineDir.y*100.0;
	onAxePnt.z = lineP.z+lineDir.z*100.0;
	ProjectWorldPoint(onAxePnt,onAxePnt2.x,onAxePnt2.y,onAxePnt2.z);
	onAxePnt1.z = onAxePnt2.z = 0.0;

	double d[2];
	d[0] = fabs(onAxePnt1.x-onAxePnt2.x);
	d[1] = fabs(onAxePnt1.y-onAxePnt2.y);

	if (d[0]<3.0 && d[1]<3.0)
		return false;

	if (d[0]>d[1])
	{
		UnProjectScreenPoint(scrX, scrY, 0.0, onAxePnt);
		UnProjectScreenPoint(scrX, scrY+10, 0.0, onAxePnt1);
		UnProjectScreenPoint(scrX, scrY, 1.0, onAxePnt2);
	}
	else
	{
		UnProjectScreenPoint(scrX, scrY, 0.0, onAxePnt);
		UnProjectScreenPoint(scrX+10, scrY, 0.0, onAxePnt1);
		UnProjectScreenPoint(scrX, scrY, 1.0, onAxePnt2);
	}

	double  plD;
	SG_VECTOR tmpVec;
	SG_VECTOR lD= lineDir;
	sgSpaceMath::NormalVector(lD);
	sgSpaceMath::PlaneFromPoints(onAxePnt, onAxePnt1, onAxePnt2,tmpVec,plD);
	onAxePnt = lineP;
	if (sgSpaceMath::IntersectPlaneAndLine(tmpVec, plD, 
						onAxePnt, lD,
						resPnt)==1)
		return true;
	else
		return false;
}

void   COpenGLView::SetEditableObject(sgCObject* eO)
{
	ASSERT(eO);
	Drawer::CurrentEditableObject = eO;
}

void  COpenGLView::SetHotObject(sgCObject* hO)
{
	Drawer::CurrentHotObject = hO;
}
sgCObject*  COpenGLView::GetHotObject() 
{return Drawer::CurrentHotObject;}

void    COpenGLView::SetCurColor(float r, float g, float b)
{
	glColor3f(r, g, b);
}

void    COpenGLView::SetLineWidth(float lW)
{
	glLineWidth((GLfloat)lW);
}

void    COpenGLView::SetPointWidth(float pS)
{
	glPointSize((GLfloat)pS);
}

void    COpenGLView::GetUserColorLines(float& r ,float& g,float& b)	
{
	r = m_rezLineColorRed;
	g = m_rezLineColorGreen;
	b = m_rezLineColorBlue;
}

void    COpenGLView::GetUserColorPoints(float& r ,float& g,float& b)
{
	r = m_rezPointColorRed;
	g = m_rezPointColorGreen;
	b = m_rezPointColorBlue;
}

void    COpenGLView::SetTransformMatrix(const sgCMatrix* mtrx)
{
	if (m_transforming_matrix)
		glPopMatrix();
    m_transforming_matrix = const_cast<sgCMatrix*>(mtrx);
	if (mtrx)
	{
		glPushMatrix();
		glMultMatrixd(m_transforming_matrix->GetTransparentData());
	}
}


void	COpenGLView::DrawPoint(const SG_POINT& pnt)
{
	glBegin(GL_POINTS);
		glVertex3dv(&pnt.x);
	glEnd();
}

void	COpenGLView::DrawLine(const SG_LINE& ln)
{
	glBegin(GL_LINES);	
		glVertex3dv(&ln.p1.x);
		glVertex3dv(&ln.p2.x);
	glEnd();
}


static  void draw_line22(SG_POINT* p1,SG_POINT* p2)
{
	glBegin(GL_LINES);
		glVertex3d(p1->x,p1->y,p1->z);
		glVertex3d(p2->x,p2->y,p2->z);
	glEnd();
}


void	COpenGLView::DrawCircle(const SG_CIRCLE& circ)
{
	circ.Draw(draw_line22);
}

void	COpenGLView::DrawArc(const SG_ARC& arcg)
{
	arcg.Draw(draw_line22);
}

void	COpenGLView::DrawSpline(SG_SPLINE* spl)
{
	const int pnts_cnt = spl->GetPointsCount();
	const SG_POINT* pnts = spl->GetPoints();
	glBegin(GL_LINE_STRIP);
	for (int j=0;j<pnts_cnt;j++)
		glVertex3dv(&pnts[j].x);
	glEnd();
}

void	COpenGLView::DrawSplineFrame(SG_SPLINE* spl)
{
	const int knts_cnt = spl->GetKnotsCount();
	const SG_POINT* knts = spl->GetKnots();
	glBegin(GL_LINE_STRIP);
	for (int j=0;j<knts_cnt;j++)
		glVertex3dv(&knts[j].x);
	glEnd();
}

void    COpenGLView::DrawSphere(double rad)
{
	SG_CIRCLE circ;
	circ.radius = rad;
	circ.center.x = circ.center.y = circ.center.z = 0.0;
	circ.normal.x = circ.normal.y = circ.normal.z = 0.0;
	circ.normal.x = 1.0;
	DrawCircle(circ);
	circ.normal.x = 0.0;
	circ.normal.y = 1.0;
	DrawCircle(circ);
	circ.normal.y = 0.0;
	circ.normal.z = 1.0;
	DrawCircle(circ);
}

void    COpenGLView::DrawBox(double sz1, double sz2,double sz3)
{
	glBegin(GL_LINES);	
	
	SG_POINT baseP;
	baseP.x = baseP.y = baseP.z = 0.0;
	SG_LINE ln;
	ln.p1 = baseP;
	ln.p2 = baseP;
	ln.p2.x+=sz1;
	
	glVertex3dv(&ln.p1.x);
	glVertex3dv(&ln.p2.x);

	ln.p1.x+=sz1;ln.p1.y+=sz2;
	glVertex3dv(&ln.p1.x);
	glVertex3dv(&ln.p2.x);

	ln.p2.x-=sz1;ln.p2.y+=sz2;
	glVertex3dv(&ln.p1.x);
	glVertex3dv(&ln.p2.x);

	ln.p1 = baseP;
	glVertex3dv(&ln.p1.x);
	glVertex3dv(&ln.p2.x);
	if (fabs(sz3)>0.0001)
	{
		ln.p2 = baseP;
		ln.p2.z+=sz3;
		glVertex3dv(&ln.p1.x);
		glVertex3dv(&ln.p2.x);
		ln.p1.z+=sz3;
		ln.p2.x+=sz1;
		glVertex3dv(&ln.p1.x);
		glVertex3dv(&ln.p2.x);
		ln.p1.x+=sz1;ln.p1.y+=sz2;
		glVertex3dv(&ln.p1.x);
		glVertex3dv(&ln.p2.x);
		ln.p2.x-=sz1;ln.p2.y+=sz2;
		glVertex3dv(&ln.p1.x);
		glVertex3dv(&ln.p2.x);
		ln.p1 = baseP;
		ln.p1.z+=sz3;
		glVertex3dv(&ln.p1.x);
		glVertex3dv(&ln.p2.x);
		ln.p1 = ln.p2;
		ln.p1.z-=sz3;
		glVertex3dv(&ln.p1.x);
		glVertex3dv(&ln.p2.x);
		ln.p2.x+=sz1;
		ln.p1.x+=sz1;
		glVertex3dv(&ln.p1.x);
		glVertex3dv(&ln.p2.x);
		ln.p2.y-=sz2;
		ln.p1.y-=sz2;
		glVertex3dv(&ln.p1.x);
		glVertex3dv(&ln.p2.x);
	}

	glEnd();
}

void    COpenGLView::DrawCone(double rd1, double rd2, double h)
{
	glBegin(GL_LINES);
		glVertex3d(0.0, 0.0, 0.0);
		glVertex3d(0.0, 0.0, h);
	glEnd();

	SG_CIRCLE circ;
	circ.radius = rd1;
	circ.center.x = circ.center.y = circ.center.z = 0.0;
	circ.normal.x = circ.normal.y = circ.normal.z = 0.0;
	circ.normal.z = 1.0;
	DrawCircle(circ);
	circ.center.z = h;
	circ.radius = rd2;
	DrawCircle(circ);
}

void    COpenGLView::DrawCylinder(double rd, double h)
{
	DrawCone(rd,rd,h);
}

void    COpenGLView::DrawEllipsoid(double rd1, double rd2, double rd3)
{
	glBegin(GL_LINES);	

	SG_POINT baseP;
	baseP.x = baseP.y = baseP.z = 0.0;
	SG_LINE ln;
	ln.p1 = baseP;
	ln.p2 = baseP;
	ln.p1.x=-rd1;
	ln.p2.x=rd1;
	glVertex3dv(&ln.p1.x);
	glVertex3dv(&ln.p2.x);

	ln.p1 = baseP;
	ln.p2 = baseP;
	ln.p1.y=-rd2;
	ln.p2.y=rd2;
	glVertex3dv(&ln.p1.x);
	glVertex3dv(&ln.p2.x);

	ln.p1 = baseP;
	ln.p2 = baseP;
	ln.p1.z=-rd3;
	ln.p2.z=rd3;
	glVertex3dv(&ln.p1.x);
	glVertex3dv(&ln.p2.x);

	
	glEnd();
}

void    COpenGLView::DrawTorus(double rd1, double rd2)
{
	
	SG_CIRCLE circ;
	circ.radius = rd1;
	circ.center.x = circ.center.y = circ.center.z = 0.0;
	circ.normal.y = circ.normal.x = 0.0;
	circ.normal.z = 1.0;
	DrawCircle(circ);
	circ.radius += rd2;
	DrawCircle(circ);
	circ.radius -= 2*rd2;
	DrawCircle(circ);

	circ.center.y = rd1;
	circ.normal.y = circ.normal.z = 0.0;
	circ.normal.x = 1.0;
	circ.radius = rd2;
	DrawCircle(circ);

	circ.center.y = -rd1;
	DrawCircle(circ);

	circ.center.y = 0.0;
	circ.center.x = rd1;
	circ.normal.x = circ.normal.z = 0.0;
	circ.normal.y = 1.0;
	DrawCircle(circ);

	circ.center.x = -rd1;
	DrawCircle(circ);

}

void    COpenGLView::DrawSphericBand(double rd, double begC, double endC)
{
	DrawSphere(rd);

	SG_CIRCLE tmpC;
	tmpC.center.x = tmpC.center.y = 0.0;
	tmpC.center.z = rd*begC;
	tmpC.radius = rd*sqrt(fabs(1.0-begC*begC));
	tmpC.normal.y = tmpC.normal.x = 0.0;
	tmpC.normal.z = 1.0;
	DrawCircle(tmpC);
	tmpC.center.z = rd*endC;
	tmpC.radius = rd*sqrt(fabs(1.0-endC*endC));
	tmpC.normal.y = tmpC.normal.x = 0.0;
	tmpC.normal.z = 1.0;
	DrawCircle(tmpC);

}

void   COpenGLView::DrawObject(const sgCObject* objct)
{
	if (!objct)
		return;

	sgCObject* ooo = const_cast<sgCObject*>(objct);

	Drawer::DrawObject(GL_RENDER,ooo,false,false);

}

void  COpenGLView::RotateCamera(CSize sdvig)
{
	double rx, ry, rz;
	double x, y, z;

	rx = ry = rz = 0;

//	Matx4x4  rotMatr;
//	GLdouble oglMatr[16];
//	SG_VECTOR  newVec;
	
	/*switch(m_Camera.m_iViewType) 
	{
	case VT_PERSPECTIVE:*/
		if(1)//pDoc->m_bRotateAboutLookAt)
			m_Camera.GetRotationAboutLookAt(&x, &y, &z);
		else
		m_Camera.GetRotationAboutEye(&x, &y, &z);
	
		rz = z - sdvig.cx*g_fMouseRotateMult;
		rx = x + sdvig.cy*g_fMouseRotateMult;
		ry = y;

		if(1)//pDoc->m_bRotateAboutLookAt)
			m_Camera.SetRotationAboutLookAt(rx, ry, rz);
		else
			m_Camera.SetRotationAboutEye(rx, ry, rz);
	
	/*	break;
	case VT_FRONT:
	case VT_BACK:
		m_Camera.GetUpVector(&x, &y, &z);
		ZeroMatrix(rotMatr);
		Rotate3D(X, sdvig.cx, rotMatr);
		Matx4x4ToglMatrix(rotMatr, oglMatr);
		newVec[X] = x;
		newVec[Y] = y;
		newVec[Z] = z;
		Transformf(newVec, newVec, oglMatr);
		m_Camera.SetUpVector(newVec[X],newVec[Y],newVec[Z]);
		break;
	case VT_TOP:
	case VT_BOTTOM:
		m_Camera.GetUpVector(&x, &y, &z);
		ZeroMatrix(rotMatr);
		Rotate3D(Z, sdvig.cx, rotMatr);
		Matx4x4ToglMatrix(rotMatr, oglMatr);
		newVec[X] = x;
		newVec[Y] = y;
		newVec[Z] = z;
		Transformf(newVec, newVec, oglMatr);
		m_Camera.SetUpVector(newVec[X],newVec[Y],newVec[Z]);
		break;
	case VT_LEFT:
	case VT_RIGHT:
		m_Camera.GetUpVector(&x, &y, &z);
		ZeroMatrix(rotMatr);
		Rotate3D(Y, sdvig.cx, rotMatr);
		Matx4x4ToglMatrix(rotMatr, oglMatr);
		newVec[X] = x;
		newVec[Y] = y;
		newVec[Z] = z;
		Transformf(newVec, newVec, oglMatr);
		m_Camera.SetUpVector(newVec[X],newVec[Y],newVec[Z]);
		break;
	}*/

	m_Camera.m_enumCameraPosition = CP_USER_POSITION;
}

void  COpenGLView::TranslateCamera(SG_VECTOR sdvig)
{
	GLdouble px, py, pz;
	GLdouble x,  y,  z;
	
	// Calculate the camera's new look 'At' position
	m_Camera.GetLookAtPos(&px, &py, &pz);
	x = px - sdvig.x;
	y = py - sdvig.y;
	z = pz - sdvig.z;
	m_Camera.SetLookAtPos(x, y, z);
	
	// Calculate the camera's new 'Eye' position
	m_Camera.GetEyePos(&px, &py, &pz);
	x = px - sdvig.x;
	y = py - sdvig.y;
	z = pz - sdvig.z;
	m_Camera.SetEyePos(x, y, z);
	
	
	// the identity matrix
	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	
	// Build the new ModelView matrix using the new camera coordinates
	m_Camera.PositionCamera();
}

void  COpenGLView::ZoomCamera(CSize sdvig)
{
	// Get the camera's Focal length
	if (m_Camera.m_bPerspective)
	{
		GLdouble fLength;
	
		fLength = m_Camera.GetFocalLength();
		fLength += sdvig.cy*g_fMouseScaleMult*(m_Camera.m_fFocalLength/2);
		if(fLength < 1.0f)
			fLength = 1.0f;
		if(fLength > m_Camera.m_fFar)
			fLength = m_Camera.m_fFar;
			
		// Set the camera's new Focal length
		m_Camera.SetFocalLength(fLength, TRUE);
	}
	else
	{
		m_Camera.m_fFovY+=sdvig.cy;
		m_Camera.ResetView();
	}
}
/*
void  COpenGLView::SetViewType(VIEWTYPE newVT)
{
	switch(newVT) 
	{
	case VT_PERSPECTIVE:
		m_Camera.m_bPerspective = true;
		m_Camera.SetLookAtPos(0.0f, 0.0f, 0.0f);
		m_Camera.SetEyePos(10.0f, 10.0f, 10.0f);
		m_Camera.SetUpVector(0.0f, 0.0f, 1.0f);
		break;
	case VT_FRONT:
		m_Camera.m_bPerspective = false;
		m_Camera.SetLookAtPos(0.0f, 0.0f, 0.0f);
		m_Camera.SetEyePos(10.0f, 0.0f, 0.0f);
		m_Camera.SetUpVector(0.0f, 0.0f, 1.0f);
		break;
	case VT_BACK:
		m_Camera.m_bPerspective = false;
		m_Camera.SetLookAtPos(0.0f, 0.0f, 0.0f);
		m_Camera.SetEyePos(-10.0f, 0.0f, 0.0f);
		m_Camera.SetUpVector(0.0f, 0.0f, 1.0f);
		break;
	case VT_TOP:
		m_Camera.m_bPerspective = false;
		m_Camera.SetLookAtPos(0.0f, 0.0f, 0.0f);
		m_Camera.SetEyePos(0.0f, 0.0f, 10.0f);
		m_Camera.SetUpVector(0.0f, 1.0f, 0.0f);
		break;
	case VT_BOTTOM:
		m_Camera.m_bPerspective = false;
		m_Camera.SetLookAtPos(0.0f, 0.0f, 0.0f);
		m_Camera.SetEyePos(0.0f, 0.0f, -10.0f);
		m_Camera.SetUpVector(0.0f, 1.0f, 0.0f);
		break;
	case VT_LEFT:
		m_Camera.m_bPerspective = false;
		m_Camera.SetLookAtPos(0.0f, 0.0f, 0.0f);
		m_Camera.SetEyePos(0.0f, -10.0f, 0.0f);
		m_Camera.SetUpVector(0.0f, 0.0f, 1.0f);
		break;	
	case VT_RIGHT:
		m_Camera.m_bPerspective = false;
		m_Camera.SetLookAtPos(0.0f, 0.0f, 0.0f);
		m_Camera.SetEyePos(10.0f, 0.0f, 0.0f);
		m_Camera.SetUpVector(0.0f, 0.0f, 1.0f);
		break;
	}
	m_Camera.m_iViewType = newVT;
}*/









/////////////////////////////////////////////////////////////////////
//-----------------------------------------------------------------//
//                    diagnostics functions                        //
//-----------------------------------------------------------------//
/////////////////////////////////////////////////////////////////////

//-------------------------------------------------------------------
// write struct PIXELFORMATDESCRIPTOR and pixelformat to file
// private
void COpenGLView::WritePfdToTxt(CString file, PIXELFORMATDESCRIPTOR *pfd, int pixelformat)
{
	FILE *stream;
	//stream = fopen(file, "w");#OBSOLETE
	fopen_s(&stream,file, "w");
	fprintf(stream,"--------------------------------------------------\n"); 
	fprintf(stream,"pixelformat     = %d \n", pixelformat          );
	fprintf(stream,"PIXELFORMATDESCRIPTOR \n");
	fprintf(stream,"nSize           = %d \n", pfd->nSize           );
	fprintf(stream,"nVersion        = %d \n", pfd->nVersion        ); 
	fprintf(stream,"dwFlags         = %d \n", pfd->dwFlags         ); 

	if(pfd->dwFlags&PFD_DRAW_TO_WINDOW)          fprintf(stream,"    + PFD_DRAW_TO_WINDOW\n"); 
	else                                    fprintf(stream,"    - PFD_DRAW_TO_WINDOW\n"); 

	if(pfd->dwFlags&PFD_DRAW_TO_BITMAP)          fprintf(stream,"    + PFD_DRAW_TO_BITMAP\n"); 
	else                                    fprintf(stream,"    - PFD_DRAW_TO_BITMAP\n"); 

	if(pfd->dwFlags&PFD_SUPPORT_GDI)             fprintf(stream,"    + PFD_SUPPORT_GDI\n"); 
	else                                    fprintf(stream,"    - PFD_SUPPORT_GDI\n"); 

	if(pfd->dwFlags&PFD_SUPPORT_OPENGL)          fprintf(stream,"    + PFD_SUPPORT_OPENGL\n"); 
	else                                    fprintf(stream,"    - PFD_SUPPORT_OPENGL\n"); 

	if(pfd->dwFlags&PFD_GENERIC_ACCELERATED)     fprintf(stream,"    + PFD_GENERIC_ACCELERATED\n"); 
	else                                    fprintf(stream,"    - PFD_GENERIC_ACCELERATED\n"); 

	if(pfd->dwFlags&PFD_GENERIC_FORMAT)          fprintf(stream,"    + PFD_GENERIC_FORMAT\n"); 
	else                                    fprintf(stream,"    - PFD_GENERIC_FORMAT\n"); 

	if(pfd->dwFlags&PFD_NEED_PALETTE)            fprintf(stream,"    + PFD_NEED_PALETTE\n"); 
	else                                    fprintf(stream,"    - PFD_NEED_PALETTE\n"); 

	if(pfd->dwFlags&PFD_NEED_SYSTEM_PALETTE)     fprintf(stream,"    + PFD_NEED_SYSTEM_PALETTE\n"); 
	else                                    fprintf(stream,"    - PFD_NEED_SYSTEM_PALETTE\n"); 

	if(pfd->dwFlags&PFD_DOUBLEBUFFER)            fprintf(stream,"    + PFD_DOUBLEBUFFER\n"); 
	else                                    fprintf(stream,"    - PFD_DOUBLEBUFFER\n"); 

	if(pfd->dwFlags&PFD_STEREO)                  fprintf(stream,"    + PFD_STEREO\n"); 
	else                                    fprintf(stream,"    - PFD_STEREO\n"); 

	if(pfd->dwFlags&PFD_SWAP_LAYER_BUFFERS)      fprintf(stream,"    + PFD_SWAP_LAYER_BUFFERS\n"); 
	else                                    fprintf(stream,"    - PFD_SWAP_LAYER_BUFFERS\n"); 

	if(pfd->dwFlags&PFD_DEPTH_DONTCARE)          fprintf(stream,"    + PFD_DEPTH_DONTCARE\n"); 
	else                                    fprintf(stream,"    - PFD_DEPTH_DONTCARE\n"); 

	if(pfd->dwFlags&PFD_DOUBLEBUFFER_DONTCARE)   fprintf(stream,"    + PFD_DOUBLEBUFFER_DONTCARE\n"); 
	else                                    fprintf(stream,"    - PFD_DOUBLEBUFFER_DONTCARE\n"); 

	if(pfd->dwFlags&PFD_STEREO_DONTCARE)         fprintf(stream,"    + PFD_STEREO_DONTCARE\n"); 
	else                                    fprintf(stream,"    - PFD_STEREO_DONTCARE\n"); 

	if(pfd->dwFlags&PFD_SWAP_COPY)               fprintf(stream,"    + PFD_SWAP_COPY\n"); 
	else                                    fprintf(stream,"    - PFD_SWAP_COPY\n"); 

	if(pfd->dwFlags&PFD_SWAP_EXCHANGE)           fprintf(stream,"    + PFD_SWAP_EXCHANGE\n"); 
	else                                    fprintf(stream,"    - PFD_SWAP_EXCHANGE\n"); 

	if(pfd->iPixelType==PFD_TYPE_RGBA)      fprintf(stream,"iPixelType      = PFD_TYPE_RGBA \n"); 
	else                                    fprintf(stream,"iPixelType      = PFD_TYPE_COLORINDEX \n"); 

	fprintf(stream,"cColorBits      = %d \n", pfd->cColorBits      ); 
	fprintf(stream,"cRedBits        = %d \n", pfd->cRedBits        ); 
	fprintf(stream,"cRedShift       = %d \n", pfd->cRedShift       ); 
	fprintf(stream,"cGreenBits      = %d \n", pfd->cGreenBits      ); 
	fprintf(stream,"cGreenShift     = %d \n", pfd->cGreenShift     ); 
	fprintf(stream,"cBlueBits       = %d \n", pfd->cBlueBits       ); 
	fprintf(stream,"cBlueShift      = %d \n", pfd->cBlueShift      ); 
	fprintf(stream,"cAlphaBits      = %d \n", pfd->cAlphaBits      ); 
	fprintf(stream,"cAlphaShift     = %d \n", pfd->cAlphaShift     ); 
	fprintf(stream,"cAccumBits      = %d \n", pfd->cAccumBits      ); 
	fprintf(stream,"cAccumRedBits   = %d \n", pfd->cAccumRedBits   ); 
	fprintf(stream,"cAccumGreenBits = %d \n", pfd->cAccumGreenBits ); 
	fprintf(stream,"cAccumBlueBits  = %d \n", pfd->cAccumBlueBits  ); 
	fprintf(stream,"cAccumAlphaBits = %d \n", pfd->cAccumAlphaBits ); 
	fprintf(stream,"cDepthBits      = %d \n", pfd->cDepthBits      ); 
	fprintf(stream,"cStencilBits    = %d \n", pfd->cStencilBits    ); 
	fprintf(stream,"cAuxBuffers     = %d \n", pfd->cAuxBuffers     ); 

	if(pfd->iLayerType==PFD_MAIN_PLANE)
		fprintf(stream,"iLayerType      = PFD_MAIN_PLANE \n"); 
	if(pfd->iLayerType==PFD_OVERLAY_PLANE)
		fprintf(stream,"iLayerType      = PFD_OVERLAY_PLANE \n"); 
	if(pfd->iLayerType==PFD_UNDERLAY_PLANE)
		fprintf(stream,"iLayerType      = PFD_UNDERLAY_PLANE \n"); 

	fprintf(stream,"bReserved       = %d \n", pfd->bReserved       ); 
	fprintf(stream,"dwLayerMask     = %d \n", pfd->dwLayerMask     ); 
	fprintf(stream,"dwVisibleMask   = %d \n", pfd->dwVisibleMask   ); 
	fprintf(stream,"dwDamageMask    = %d \n", pfd->dwDamageMask    ); 

	fclose(stream);
}
//-------------------------------------------------------------------
// returns true if extention pName is suported
// private
bool COpenGLView::IsExtEnable(const char * pName)
{
	const char * ch = (const char *)glGetString(GL_EXTENSIONS);

	size_t size = strlen(pName);
	while(ch)
	{
		if(!strncmp(ch,pName,size) && (*(ch+size)==0 || *(ch+size)==' '))
			return true;
		ch=strchr(ch,' ');
		if(ch)
			ch++;
	}

	return false;
}
//-------------------------------------------------------------------
// write to file all extentions that are suported by video card driver
// private
void COpenGLView::WriteExtInfoToFile(CString file)
{
	GLubyte *string1;
	GLubyte *string2;
	GLubyte *string3;
	GLubyte *string4;
	string1 = (GLubyte *)glGetString(    GL_VENDOR       );
	string2 = (GLubyte *)glGetString(    GL_RENDERER     );
	string3 = (GLubyte *)glGetString(    GL_VERSION      );
	string4 = (GLubyte *)glGetString(    GL_EXTENSIONS   );
	FILE *stream;
	//stream = fopen(file, "w");#OBSOLETE
	fopen_s(&stream,file, "w");

	int i;

	for(i=0; string1[i]!='\0';i++)
		fprintf(stream,"%c",string1[i]);
	fprintf(stream,"\n");

	for(i=0; string2[i]!='\0';i++)
		fprintf(stream,"%c",string2[i]);
	fprintf(stream,"\n");

	for(i=0; string3[i]!='\0';i++)
		fprintf(stream,"%c",string3[i]);
	fprintf(stream,"\n");
	fprintf(stream,"\n");

	int n=1;
	fprintf(stream,"1\t");
	for(i=0; string4[i]!='\0';i++)
	{
		if(string4[i]==' ')
		{
			fprintf(stream,"\n");
			n++;
			fprintf(stream,"%d\t",n);
		}
		else
			fprintf(stream,"%c",string4[i]);
	}
	fclose(stream);
}


void COpenGLView::OnParallProj()
{
	m_Camera.m_bPerspective = false;
	m_Camera.ResetView();
	Invalidate();
}

void COpenGLView::OnUpdateParallProj(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(!m_Camera.m_bPerspective);
}

void COpenGLView::OnPerspectProj()
{
	m_Camera.m_bPerspective = true;
	m_Camera.ResetView();
	Invalidate();
}

void COpenGLView::OnUpdatePerspectProj(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_Camera.m_bPerspective);
}

void COpenGLView::OnFrontView()
{
	m_Camera.StartAnimatePosition(this,CP_FRONT);
}

void COpenGLView::OnUpdateFrontView(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_Camera.m_enumCameraPosition==CP_FRONT);
}

void COpenGLView::OnBackView()
{
	m_Camera.StartAnimatePosition(this,CP_BACK);
}

void COpenGLView::OnUpdateBackView(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_Camera.m_enumCameraPosition==CP_BACK);
}

void COpenGLView::OnTopView()
{
	m_Camera.StartAnimatePosition(this,CP_TOP);
}

void COpenGLView::OnUpdateTopView(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_Camera.m_enumCameraPosition==CP_TOP);
}

void COpenGLView::OnBottomView()
{
	m_Camera.StartAnimatePosition(this,CP_BOTTOM);
}

void COpenGLView::OnUpdateBottomView(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_Camera.m_enumCameraPosition==CP_BOTTOM);
}

void COpenGLView::OnLeftView()
{
	m_Camera.StartAnimatePosition(this,CP_LEFT);
}

void COpenGLView::OnUpdateLeftView(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_Camera.m_enumCameraPosition==CP_LEFT);
}

void COpenGLView::OnRightView()
{
	m_Camera.StartAnimatePosition(this,CP_RIGHT);
}

void COpenGLView::OnUpdateRightView(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_Camera.m_enumCameraPosition==CP_RIGHT);
}

void COpenGLView::OnIsoFrontView()
{
	m_Camera.StartAnimatePosition(this,CP_ISO_FRONT);
}

void COpenGLView::OnUpdateIsoFrontView(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_Camera.m_enumCameraPosition==CP_ISO_FRONT);
}

void COpenGLView::OnIsoBackView()
{
	m_Camera.StartAnimatePosition(this,CP_ISO_BACK);
}

void COpenGLView::OnUpdateIsoBackView(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_Camera.m_enumCameraPosition==CP_ISO_BACK);
}

void COpenGLView::OnTimer(UINT nIDEvent)
{
	m_Camera.AnimatePosition(this);
	__super::OnTimer(nIDEvent);
}

void COpenGLView::OnMoveEyeHand()
{
	m_hand_action = HA_MOVE;
}

void COpenGLView::OnUpdateMoveEyeHand(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_hand_action==HA_MOVE);
}

void COpenGLView::OnRotateEyeHand()
{
	m_hand_action = HA_ROTATE;
}

void COpenGLView::OnUpdateRotateEyeHand(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_hand_action==HA_ROTATE);
}

void COpenGLView::OnZoomEyeHand()
{
	m_hand_action = HA_ZOOM;
}

void COpenGLView::OnUpdateZoomEyeHand(CCmdUI *pCmdUI)
{
	pCmdUI->SetCheck(m_hand_action==HA_ZOOM);
}

void COpenGLView::OnMoveLeftAuto()
{
	SG_VECTOR sdv;
	SG_VECTOR Pnt1;
	SG_VECTOR Pnt2;
	
	m_Camera.GetWorldCoord(100,100, 0.0, Pnt1);
	m_Camera.GetWorldCoord(110,100, 0.0, Pnt2);
	//VecSubf(Pnt1, Pnt2, sdv);
	sdv = sgSpaceMath::VectorsSub(Pnt1, Pnt2);
	
	TranslateCamera(sdv);
}

void COpenGLView::OnMoveRightAuto()
{
	SG_VECTOR sdv;
	SG_VECTOR Pnt1;
	SG_VECTOR Pnt2;

	m_Camera.GetWorldCoord(100,100, 0.0, Pnt1);
	m_Camera.GetWorldCoord(90,100, 0.0, Pnt2);
	//VecSubf(Pnt1, Pnt2, sdv);
	sdv = sgSpaceMath::VectorsSub(Pnt1, Pnt2);

	TranslateCamera(sdv);
}

void COpenGLView::OnMoveUpAuto()
{
	SG_VECTOR sdv;
	SG_VECTOR Pnt1;
	SG_VECTOR Pnt2;

	m_Camera.GetWorldCoord(100,100, 0.0, Pnt1);
	m_Camera.GetWorldCoord(100,110, 0.0, Pnt2);
	//VecSubf(Pnt1, Pnt2, sdv);
	sdv = sgSpaceMath::VectorsSub(Pnt1, Pnt2);

	TranslateCamera(sdv);
}

void COpenGLView::OnMoveDownAuto()
{
	SG_VECTOR sdv;
	SG_VECTOR Pnt1;
	SG_VECTOR Pnt2;

	m_Camera.GetWorldCoord(100,100, 0.0, Pnt1);
	m_Camera.GetWorldCoord(100,90, 0.0, Pnt2);
	//VecSubf(Pnt1, Pnt2, sdv);
	sdv = sgSpaceMath::VectorsSub(Pnt1, Pnt2);

	TranslateCamera(sdv);
}

void COpenGLView::OnRotateXAuto()
{
	sgCMatrix matr;

	SG_POINT baseP;
	memset(&baseP,0,sizeof(SG_POINT));

	SG_VECTOR dirV;
	memset(&dirV,0,sizeof(SG_VECTOR));
	dirV.x = 1.0;
	
	matr.Rotate(baseP,dirV,0.1);

	SG_POINT eyePosPnt;
	m_Camera.GetEyePos(&eyePosPnt.x,&eyePosPnt.y,&eyePosPnt.z);
	
	matr.ApplyMatrixToPoint(eyePosPnt);

	m_Camera.SetEyePos(eyePosPnt.x,eyePosPnt.y,eyePosPnt.z);
	Invalidate();
}

void COpenGLView::OnRotateYAuto()
{
	sgCMatrix matr;

	SG_POINT baseP;
	memset(&baseP,0,sizeof(SG_POINT));

	SG_VECTOR dirV;
	memset(&dirV,0,sizeof(SG_VECTOR));
	dirV.y = 1.0;

	matr.Rotate(baseP,dirV,0.1);

	SG_POINT eyePosPnt;
	m_Camera.GetEyePos(&eyePosPnt.x,&eyePosPnt.y,&eyePosPnt.z);

	matr.ApplyMatrixToPoint(eyePosPnt);

	m_Camera.SetEyePos(eyePosPnt.x,eyePosPnt.y,eyePosPnt.z);
	Invalidate();
}

void COpenGLView::OnRotateZAuto()
{
	sgCMatrix matr;

	SG_POINT baseP;
	memset(&baseP,0,sizeof(SG_POINT));

	SG_VECTOR dirV;
	memset(&dirV,0,sizeof(SG_VECTOR));
	dirV.z = 1.0;

	matr.Rotate(baseP,dirV,0.1);

	SG_POINT eyePosPnt;
	m_Camera.GetEyePos(&eyePosPnt.x,&eyePosPnt.y,&eyePosPnt.z);

	matr.ApplyMatrixToPoint(eyePosPnt);

	m_Camera.SetEyePos(eyePosPnt.x,eyePosPnt.y,eyePosPnt.z);
	Invalidate();
}

void COpenGLView::OnZoomPlusAuto()
{
	ZoomCamera(CSize(0,-20));
}

void COpenGLView::OnZoomMinusAuto()
{
	ZoomCamera(CSize(0,20));
}

void COpenGLView::OnCopyAsRastr()
{
	/*Баг - копирует вместе с меню*/
	CDC dc;
	HDC hdc = ::GetWindowDC(m_hWnd);
	dc.Attach(hdc);
	CDC memDC;
	memDC.CreateCompatibleDC(&dc);

	CBitmap bm;
	CRect r;
	GetClientRect(&r);

	bm.CreateCompatibleBitmap(&dc, r.Width(), r.Height());
	CBitmap * oldbm = memDC.SelectObject(&bm);
	memDC.BitBlt(0, 0, r.Width(), r.Height(), &dc, 0, 0, SRCCOPY);

	OpenClipboard();
	::EmptyClipboard();
	::SetClipboardData(CF_BITMAP, bm.m_hObject);
	CloseClipboard();

	memDC.SelectObject(oldbm);
	dc.Detach();
}

void COpenGLView::OnCopyAsVector()
{
	BeginWaitCursor();

	if ( OpenClipboard() )
	{
		EmptyClipboard();
		//create the metafile DC
		CMetaFileDC * cDC = new CMetaFileDC();
		cDC->CreateEnhanced(GetDC(),NULL,NULL,"meta");
		//call draw routine here that makes GDI calls int cDC

		CRect rect;
		GetClientRect(&rect);
		// Start rendering via std GDI 2D drawing functions
		sgCObject*  curObj = sgGetScene()->GetObjectsList()->GetHead();
		while (curObj) 
		{
			//if (pDoc->GetLayerVisibles(curObj->GetLayer()))
			Drawer::ProjectObjectOnMetaDC(curObj,
				cDC,
				m_Camera.m_dModelViewMatrix,
				m_Camera.m_dProjectionMatrix,
				m_Camera.m_iViewport,
				rect.Height(),Drawer::PROJECT_LINES,
				30.0,
				1.0);
			curObj = sgGetScene()->GetObjectsList()->GetNext(curObj);
		}
		//close meta CMetafileDC and get its handle
		HENHMETAFILE handle = cDC->CloseEnhanced();
		//place it on the clipboard
		SetClipboardData(CF_ENHMETAFILE,handle);
		cDC->Detach();
		CloseClipboard();
		//delete the dc
		delete cDC;
	}

	EndWaitCursor();
}

void COpenGLView::OnSaveAsRastr()
{
#ifdef NUGEN_RETAIL
	CString			Path;

	CFileDialog dlg(
		FALSE,
		NULL,								// Open File Dialog
		_T(""),							// Default extension
		OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT,	// No default filename
		_T("BMP files(*.bmp)|*.bmp||"));// Filter string

	if (dlg.DoModal() != IDOK)
		return;

	Path = dlg.GetPathName();

	if (Path.Right(4)!=".bmp")
		Path+=".bmp";

	ReadAndWriteToBmp(Path);
#else
    AfxMessageBox("This feature is not available in DEMO!");
#endif
}

void COpenGLView::OnSaveAsVector()
{
#ifdef NUGEN_RETAIL
	CString			Path;

	CFileDialog dlg(
		FALSE,
		NULL,								// Open File Dialog
		_T(""),							// Default extension
		OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT,	// No default filename
		_T("WMF files(*.wmf)|*.wmf||"));// Filter string

	if (dlg.DoModal() != IDOK)
		return;
	
	Path = dlg.GetPathName();

	if (Path.Right(4)!=".wmf")
		Path+=".wmf";

	BeginWaitCursor();

		//create the metafile DC
		CMetaFileDC * cDC = new CMetaFileDC();
		cDC->CreateEnhanced(GetDC(),Path,NULL,"meta");
		//call draw routine here that makes GDI calls int cDC

		CRect rect;
		GetClientRect(&rect);
		// Start rendering via std GDI 2D drawing functions
		sgCObject*  curObj = sgGetScene()->GetObjectsList()->GetHead();
		while (curObj) 
		{
			//if (pDoc->GetLayerVisibles(curObj->GetLayer()))
			Drawer::ProjectObjectOnMetaDC(curObj,
				cDC,
				m_Camera.m_dModelViewMatrix,
				m_Camera.m_dProjectionMatrix,
				m_Camera.m_iViewport,
				rect.Height(),Drawer::PROJECT_LINES,
				30.0,
				1.0);
			curObj = sgGetScene()->GetObjectsList()->GetNext(curObj);
		}
		//close meta CMetafileDC and get its handle
		cDC->CloseEnhanced();
		//place it on the clipboard
		//cDC->Detach();
		//delete the dc
		delete cDC;
	
	EndWaitCursor();
#else
    AfxMessageBox("This feature is not available in DEMO!");
#endif
}

void COpenGLView::OnTranslate()
{
	m_hand_action = HA_OBJ_TRANSLATE;
	static_cast<CChildFrame*>(GetParentFrame())->CreateTransformCommander();

	/*sgCObject*  curObj = sgGetScene()->GetHeadSelectedObject();
	while (curObj) 
	{
		SG_VECTOR aaa;
		aaa.x = 5.0; aaa.y=aaa.z = 0.0;
		curObj->InitTempMatrix()->Translate(&aaa);
		curObj->ApplyTempMatrix();
		curObj->DestroyTempMatrix();
		curObj = sgGetScene()->GetNextSelectedObject(curObj);
	}
	InvalidateViewPort();*/
}

void COpenGLView::OnUpdateTranslate(CCmdUI *pCmdUI)
{
	if (sgGetScene()->GetObjectsList()->GetCount()!=0)
	{
		pCmdUI->Enable(TRUE);
		pCmdUI->SetCheck(m_hand_action==HA_OBJ_TRANSLATE);
	}
	else
	{
		pCmdUI->SetCheck(FALSE);
		pCmdUI->Enable(FALSE);
	}
}

void COpenGLView::OnRotate()
{
	m_hand_action = HA_OBJ_ROTATE;
	static_cast<CChildFrame*>(GetParentFrame())->CreateTransformCommander();
}

void COpenGLView::OnUpdateRotate(CCmdUI *pCmdUI)
{
	if (sgGetScene()->GetObjectsList()->GetCount()!=0)
	{
		pCmdUI->Enable(TRUE);
		pCmdUI->SetCheck(m_hand_action==HA_OBJ_ROTATE);
	}
	else
	{
		pCmdUI->SetCheck(FALSE);
		pCmdUI->Enable(FALSE);
	}
}

void COpenGLView::OnAllSceneView()
{
	SG_POINT a1,a2;
	sgGetScene()->GetGabarits(a1,a2);
	m_Camera.FitBounds(a1.x,a1.y,a1.z,a2.x,a2.y,a2.z);
	Invalidate();
}

void COpenGLView::OnUpdateAllSceneView(CCmdUI *pCmdUI)
{
	pCmdUI->Enable(sgGetScene()->GetObjectsList()->GetCount()!=0);
}

/*
void COpenGLView::OnGroupSelecting()
{
	int selC = sgGetScene()->GetSelectedObjectsCount();
	sgCObject**  objArr = (sgCObject**)malloc(selC*sizeof(sgCObject*));
		
	sgCObject*  curObj = sgGetScene()->GetHeadSelectedObject();
	int i=0;
	while (curObj) 
	{
		ASSERT(i<selC);
		objArr[i] = curObj;
		i++;
		//sgGetScene()->DetachObject(curObj);
		curObj = sgGetScene()->GetNextSelectedObject(curObj);
	}
	ASSERT(i==selC);
	sgGetScene()->AttachObject(sgCGroup::CreateGroup(objArr,selC));
	free(objArr);
	Invalidate();
}

*/
void COpenGLView::OnCreateGroup()
{
	static_cast<CChildFrame*>(GetParentFrame())->CreateGroupCommander();
}

void COpenGLView::OnUpdateCreateGroup(CCmdUI *pCmdUI)
{
	pCmdUI->Enable(sgGetScene()->GetObjectsList()->GetCount()!=0);
}
