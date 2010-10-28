// DlgRender.cpp : file di implementazione
//

#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "DlgRender.h"
#include "..//RayTracing//RTRenderer.h"
#include "..//RayTracing//RTMaterials.h"
#include "..//Drawer.h"

#include <process.h>

MyRenderer global_rend;

static  void  StartRender(void* arg)
{
	rtRayTracer::rtStart((RT_VIEW_PORT*)arg);
}



// finestra di dialogo CDlgRender

IMPLEMENT_DYNAMIC(CDlgRender, CDialog)

CDlgRender::CDlgRender(C3dCamera*  cam, CWnd* pParent /*=NULL*/)
	: CDialog(CDlgRender::IDD, pParent)
	, m_bAntiAlias(FALSE)
	, m_iSize(800)
	, m_iColor(0)
{
	m_FrameDC=NULL;
	m_camera = cam;
}

CDlgRender::~CDlgRender()
{
}

void CDlgRender::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	DDX_Check(pDX, IDC_ANTIALIAS, m_bAntiAlias);
	DDX_Control(pDX, IDC_RENDRESIZE, m_Preview);
	DDX_Text(pDX, IDC_SIZEIMG, m_iSize);
	DDV_MinMaxInt(pDX, m_iSize, 100, 10000);
	DDX_Control(pDX, IDC_COMBO1, m_cmbColorBackground);
	DDX_CBIndex(pDX, IDC_COMBO1, m_iColor);
}


BEGIN_MESSAGE_MAP(CDlgRender, CDialog)
	ON_WM_DESTROY()
	ON_WM_SIZE()
	ON_BN_CLICKED(IDC_START_RENDER, &CDlgRender::OnBnClickedStartRender)
	ON_BN_CLICKED(IDC_STOP_RENDER, &CDlgRender::OnBnClickedStopRender)
END_MESSAGE_MAP()

void CDlgRender::OnSize(UINT nType,int cx,int cy)
{
	CWnd *pFrame = GetDlgItem(IDC_RENDERBMP);
	if (pFrame)
		pFrame->SetWindowPos(&CWnd::wndTop, 0,0, cx - 3, cy, SWP_NOMOVE);

	CDialog::OnSize(nType,cx,cy);
}

void CDlgRender::OnDestroy()
{
	CDialog::OnDestroy();

	rtRayTracer::rtStop();
	
	if (!m_csPathBmp.IsEmpty())
		SaveBitmap(global_rend.m_bitmap, m_csPathBmp.GetBuffer());

	global_rend.DeInitRender();
	if (m_FrameDC)
	{
		m_Preview.ReleaseDC(m_FrameDC);
		m_FrameDC = NULL;
	}
}
void CDlgRender::OnBnClickedStopRender()
{	rtRayTracer::rtStop();
}
void CDlgRender::OnBnClickedStartRender()
{
	UpdateData();

	CFileDialog dlg(
	FALSE,
	NULL,               // Open File Dialog
	_T(""),             // Default extension
	OFN_HIDEREADONLY | OFN_OVERWRITEPROMPT, // No default filename
	_T("Bitmap BMP (*.bmp)|*.bmp||"));// Filter string

	if (dlg.DoModal() != IDOK)
		return;
	m_csPathBmp = dlg.GetPathName();
	if (m_csPathBmp.Right(4)!=".bmp")
		m_csPathBmp+=".bmp";

	if (!m_camera)
		return;

	OnBnClickedStopRender();

	RECT frame_rect;
	m_Preview.GetClientRect(&frame_rect);

	if (m_FrameDC==NULL)
		m_FrameDC = m_Preview.GetDC();

/*	global_rend.InitRender(m_FrameDC->m_hDC,frame_rect.right-frame_rect.left, 
									frame_rect.bottom-frame_rect.top,0, 0);*/
	//global_rend.InitRender(m_FrameDC->m_hDC,m_iSize, m_iSize,0,0);
	global_rend.InitRender(m_Preview.GetDC()->m_hDC ,m_iSize, m_iSize,0,0,frame_rect.right-frame_rect.left, 
									frame_rect.bottom-frame_rect.top);									
	SG_VECTOR eye_loc;
	SG_VECTOR eye_look_at;
	SG_VECTOR eye_y;
	SG_VECTOR eye_x;

	m_camera->GetVectorsForRayTracingCamera(eye_loc,eye_look_at,eye_y,eye_x);

	global_rend.FillScene();

	rtRayTracer::rtSetScene(&global_rend);
	rtRayTracer::rtSetLightSourcesContainer(&global_rend);
	rtRayTracer::rtSetAntialiasFlag(m_bAntiAlias);
	if (m_iColor>=0)
	{	rtTexture::RT_COLOR colBack;
		colBack.m_red   = Drawer::GetColorByIndex(m_iColor)[0];
		colBack.m_green = Drawer::GetColorByIndex(m_iColor)[1];
		colBack.m_blue  = Drawer::GetColorByIndex(m_iColor)[2];
		rtRayTracer::rtSetBackgroundColor(colBack);
	}
	if (global_rend.GetLightConfig()==LC_SECOND)
	{
		rtTexture::RT_COLOR col = {0.0, 0.0, 0.0, 0.0};
		rtRayTracer::rtSetAtmosphereParams(40, 0.4, col);
		rtTexture::RT_COLOR col2 = {1.0f, 0.96f, 0.93f, 0.0f};//{0.0, 0.0, 0.0, 0.0};
		rtRayTracer::rtSetBackgroundColor(col2);
	}
	rtRayTracer::rtSetCamera(rtRayTracer::CT_PERSPECTIVE,SG_TO_RT(eye_loc),
		SG_TO_RT(eye_look_at),
		SG_TO_RT(eye_y),
		SG_TO_RT(eye_x),NULL);

	_beginthread(StartRender,1024,(void*)&global_rend);

}

BOOL CDlgRender::OnInitDialog()
{
	CDialog::OnInitDialog();
	m_cmbColorBackground.InitializeDefaultColors();

	return FALSE;
}

