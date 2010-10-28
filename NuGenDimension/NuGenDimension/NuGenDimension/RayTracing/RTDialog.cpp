#include "stdafx.h"
#include "RTDialog.h"
#include "RTRenderer.h"
#include "RTMaterials.h"
#include <process.h>

MyRenderer global_rend;

HFONT CRTDialog::m_hFont = NULL;
HWND  CRTDialog::m_hWndDialog = NULL;
HWND  CRTDialog::m_hWndParent = NULL;

HWND  CRTDialog::m_hWndStart = NULL;
HWND  CRTDialog::m_hWndStop = NULL;
HWND  CRTDialog::m_hWndFrame = NULL;
HDC   CRTDialog::m_FrameDC = NULL;

C3dCamera*  CRTDialog::m_camera = NULL;

HINSTANCE CRTDialog::m_hInst = NULL;

#define   DLG_WIDTH    500
#define   DLG_HEIGHT   400

#define   BTN_WIDTH    90
#define   BTN_HEIGHT   25

#define   CX_SHIFT     5
#define   CY_SHIFT     5

CRTDialog::CRTDialog(HWND hWndParent, C3dCamera*  cam)
{
	HINSTANCE hInst = GetModuleHandle(NULL);

	WNDCLASSEX wcex;

	if (!GetClassInfoEx(hInst, "RTDialog", &wcex))
	{
		wcex.cbSize = sizeof(WNDCLASSEX); 

		wcex.style			= CS_HREDRAW | CS_VREDRAW;
		wcex.lpfnWndProc	= (WNDPROC)WndProc;
		wcex.cbClsExtra		= 0;
		wcex.cbWndExtra		= 0;
		wcex.hInstance		= hInst;
		wcex.hIcon			= NULL;
		wcex.hCursor		= LoadCursor(NULL, IDC_ARROW);
		wcex.hbrBackground	= (HBRUSH)(COLOR_WINDOW);
		wcex.lpszMenuName	= NULL;
		wcex.lpszClassName	= "RTDialog";
		wcex.hIconSm		= NULL;

		if (RegisterClassEx(&wcex) == 0)
			MessageBox(NULL, "Can't create CRTDialog!", "Error", MB_OK);
	}

    m_hWndParent = hWndParent;
	m_camera = cam;
}

CRTDialog::~CRTDialog()
{
}

LRESULT CALLBACK CRTDialog::WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    LOGFONT lfont;
	RECT    clRect;
	LONG dlgH;
	LONG dlgW;


	switch (message) 
	{
		case WM_CREATE:
            // font
            memset(&lfont, 0, sizeof(lfont));
            lstrcpy(lfont.lfFaceName, _T("Arial"));
            lfont.lfHeight = 16;
            lfont.lfWeight = FW_NORMAL;//FW_BOLD;
            lfont.lfItalic = FALSE;
            lfont.lfCharSet = DEFAULT_CHARSET;
            lfont.lfOutPrecision = OUT_DEFAULT_PRECIS;
            lfont.lfClipPrecision = CLIP_DEFAULT_PRECIS;
            lfont.lfQuality = DEFAULT_QUALITY;
            lfont.lfPitchAndFamily = DEFAULT_PITCH;
	        m_hFont = CreateFontIndirect(&lfont);

	        m_hInst = GetModuleHandle(NULL);

			GetClientRect(hWnd,&clRect);
			dlgH = clRect.bottom-clRect.top;
			dlgW = clRect.right-clRect.left;

			// button OK
			m_hWndStart = CreateWindowEx(WS_EX_STATICEDGE,
				"button","Start",
				WS_VISIBLE | WS_CHILD | WS_TABSTOP, 
				dlgW - BTN_WIDTH-CX_SHIFT, CY_SHIFT, BTN_WIDTH, BTN_HEIGHT, 
				hWnd, 
				NULL, 
				m_hInst, 
				NULL); 

            // setting font
            SendMessage(m_hWndStart, WM_SETFONT, (WPARAM)m_hFont, 0);

            // button Cancel
			m_hWndStop = CreateWindowEx(WS_EX_STATICEDGE,
				"button","Stop",
				WS_VISIBLE | WS_CHILD | WS_TABSTOP, 
				dlgW - BTN_WIDTH-CX_SHIFT, CY_SHIFT+BTN_HEIGHT+CY_SHIFT, BTN_WIDTH, BTN_HEIGHT, 
				hWnd, 
				NULL, 
				m_hInst, 
				NULL); 

            // setting font
            SendMessage(m_hWndStop, WM_SETFONT, (WPARAM)m_hFont, 0);

			       
            // static Propmpt
			m_hWndFrame = CreateWindowEx(WS_EX_STATICEDGE,
				"static","",
				WS_VISIBLE | WS_CHILD, 
				//CX_SHIFT, CY_SHIFT, dlgW - 3*CX_SHIFT - BTN_WIDTH, dlgH - 2*CY_SHIFT,
				CX_SHIFT, CY_SHIFT, 2000, 2000,
				hWnd, 
				NULL, 
				m_hInst, 
				NULL); 

            // setting font
            SendMessage(m_hWndFrame, WM_SETFONT, (WPARAM)m_hFont, 0);

            SetFocus(m_hWndStart);
			break;
		case WM_DESTROY:

			OnDestroy();

			DeleteObject(m_hFont);
			EnableWindow(m_hWndParent, TRUE);
			SetForegroundWindow(m_hWndParent);
			DestroyWindow(hWnd);
			PostQuitMessage(0);

			break;
		case WM_SIZE:
			OnSize(LOWORD(lParam), HIWORD(lParam));
			break;
        case WM_COMMAND:
            switch (HIWORD(wParam))
            {
                case BN_CLICKED:
                    if ((HWND)lParam == m_hWndStart)
                        OnStart();
                    if ((HWND)lParam == m_hWndStop)
                        OnStop();
                    break;
            }
            break;

		default:
			return DefWindowProc(hWnd, message, wParam, lParam);
   }
   return 0;
}

static  void  StartRender(void* arg)
{
	rtRayTracer::rtStart((RT_VIEW_PORT*)arg);
}

void CRTDialog::OnStart()
{
	if (!m_hWndFrame || !m_camera)
		return;

	OnStop();

	RECT frame_rect;
	GetClientRect(m_hWndFrame,&frame_rect);

	if (m_FrameDC==NULL)
	{
		m_FrameDC = GetDC(m_hWndFrame);
	}

	global_rend.InitRender(m_FrameDC,frame_rect.right-frame_rect.left, 
									frame_rect.bottom-frame_rect.top,
									0, 0);

	SG_VECTOR eye_loc;
	SG_VECTOR eye_look_at;
	SG_VECTOR eye_y;
	SG_VECTOR eye_x;

	m_camera->GetVectorsForRayTracingCamera(eye_loc,eye_look_at,eye_y,eye_x);
	
	global_rend.FillScene();

	rtRayTracer::rtSetScene(&global_rend);
	rtRayTracer::rtSetLightSourcesContainer(&global_rend);
	rtRayTracer::rtSetAntialiasFlag(true);

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
	//rtRayTracer::sgStart(&global_rend);
}

void CRTDialog::OnStop()
{
	rtRayTracer::rtStop();
}

void CRTDialog::OnDestroy()
{
	rtRayTracer::rtStop();

	global_rend.DeInitRender();
	if (m_FrameDC)
	{
		ReleaseDC(m_hWndFrame, m_FrameDC);
		m_FrameDC = NULL;
	}
}

void CRTDialog::OnSize(int cx, int cy)
{
	if (m_hWndStart)
	{
		MoveWindow(m_hWndStart,cx - BTN_WIDTH-CX_SHIFT, CY_SHIFT, BTN_WIDTH, BTN_HEIGHT, FALSE);
	}
	if (m_hWndStop)
	{
		MoveWindow(m_hWndStop, cx - BTN_WIDTH-CX_SHIFT, CY_SHIFT+BTN_HEIGHT+CY_SHIFT, BTN_WIDTH, BTN_HEIGHT, FALSE);
	}
	if (m_hWndFrame)
	{
		//MoveWindow(m_hWndFrame,CX_SHIFT, CY_SHIFT, cx - 3*CX_SHIFT - BTN_WIDTH, cy - 2*CY_SHIFT, FALSE);
	}
}

BOOL CRTDialog::DoModal()
{
	RECT r;
	GetWindowRect(GetDesktopWindow(), &r);

	m_hWndDialog = CreateWindowEx(WS_EX_TOOLWINDOW, 
                "RTDialog",
                "sgCore Ray Tracing",
                WS_POPUPWINDOW | WS_CAPTION | WS_SIZEBOX, 
                (r.right - DLG_WIDTH) / 2, (r.bottom - DLG_HEIGHT) / 2,
                DLG_WIDTH, DLG_HEIGHT,
                m_hWndParent,
                NULL,
                m_hInst,
                NULL);
    if(m_hWndDialog == NULL)
        return FALSE;

    SetForegroundWindow(m_hWndDialog);

	EnableWindow(m_hWndParent, FALSE);

    ShowWindow(m_hWndDialog, SW_SHOW); 
    UpdateWindow(m_hWndDialog);

    BOOL ret = 0;

	MSG msg;
    while (GetMessage(&msg, NULL, 0, 0)) 
    {       
		if (msg.message == WM_KEYDOWN) 
		{
			if (msg.wParam == VK_ESCAPE)
            {
				SendMessage(m_hWndDialog, WM_DESTROY, 0, 0);
                ret = 0;
            }         
		}
        TranslateMessage(&msg);
		DispatchMessage(&msg);      
    }

	return ret;
}
