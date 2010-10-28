// PreviewTextWnd.cpp : implementation file
//

#include "stdafx.h"
#include "PreviewTextWnd.h"
#include <math.h>


// CPreviewTextWnd

IMPLEMENT_DYNAMIC(CPreviewTextWnd, CWnd)
CPreviewTextWnd::CPreviewTextWnd()
{
	m_cur_style_pntr = NULL;
	m_cur_fnt = -1;
}

CPreviewTextWnd::~CPreviewTextWnd()
{
}


BEGIN_MESSAGE_MAP(CPreviewTextWnd, CWnd)
	ON_WM_PAINT()
END_MESSAGE_MAP()

// CPreviewTextWnd message handlers
static SG_POINT  gab[2];
static RECT      draw_rect;
static HDC       draw_DC = NULL;
static double    sc = 0.0;
static double locZx = 0.0;
static double locZy = 0.0;


static void draw_for_gabar_calc(SG_POINT* pb,SG_POINT* pe)
{
	if (pb->x < gab[0].x) gab[0].x = pb->x;
	if (pb->y < gab[0].y) gab[0].y = pb->y;
	if (pb->z < gab[0].z) gab[0].z = pb->z;
	if (pb->x > gab[1].x) gab[1].x = pb->x;
	if (pb->y > gab[1].y) gab[1].y = pb->y;
	if (pb->z > gab[1].z) gab[1].z = pb->z;

	if (pe->x < gab[0].x) gab[0].x = pe->x;
	if (pe->y < gab[0].y) gab[0].y = pe->y;
	if (pe->z < gab[0].z) gab[0].z = pe->z;
	if (pe->x > gab[1].x) gab[1].x = pe->x;
	if (pe->y > gab[1].y) gab[1].y = pe->y;
	if (pe->z > gab[1].z) gab[1].z = pe->z;
}

static void draw_for_draw(SG_POINT* pb,SG_POINT* pe)
{
	MoveToEx(draw_DC, (int)(locZx+(pb->x-gab[0].x)*sc), 
		(int)(locZy-(pb->y-gab[0].y)*sc),NULL);
	LineTo(draw_DC, (int)(locZx+(pe->x-gab[0].x)*sc), 
		(int)(locZy-(pe->y-gab[0].y)*sc));
}


void CPreviewTextWnd::SetText(const char* txt)
{
	m_text = CString(txt);
	if (m_text.IsEmpty())
		return;

	SG_POINT    p = {0, 0, 0};

	gab[0].x = gab[0].y = gab[0].z =  1.e35f;
	gab[1].x = gab[1].y = gab[1].z = -1.e35f;

	sgCText::Draw(sgFontManager::GetFont(m_cur_fnt),*m_cur_style_pntr,
		NULL,m_text,draw_for_gabar_calc);

	if(gab[0].x == 1.e35f)
	{
		//ASSERT(0);
		return; // Пустой текст
	}

	unsigned char nUp = sgFontManager::GetFont(m_cur_fnt)->GetFontData()->posit_size;
	if(nUp)
	{
		double      dx = m_cur_style_pntr->height/nUp;
		p.y = -dx*sgFontManager::GetFont(m_cur_fnt)->GetFontData()->negat_size;
		draw_for_gabar_calc(&p,&p);
		p.x = p.y = dx*nUp;
		draw_for_gabar_calc(&p,&p);
	}

	if (fabs(gab[0].x-gab[1].x)<0.001 ||
		fabs(gab[0].y-gab[1].y)<0.001) 
	{
			ASSERT(0);
			return; 
	}
	Invalidate();
}

void CPreviewTextWnd::OnPaint()
{
	CPaintDC dc(this); // device context for painting
	CRect clR;
	GetClientRect(clR);
	dc.FillSolidRect(0,0,clR.Width(),clR.Height(),::GetSysColor(COLOR_WINDOW));

	draw_DC = dc.m_hDC;
	draw_rect = clR;
	InflateRect(&draw_rect,-5,-5);
	sc = 0.0;
	
	double frame_width = /*min(*/draw_rect.right-draw_rect.left/*, gab[1].x-gab[0].x)*/;
	double frame_height = /*min(*/draw_rect.bottom-draw_rect.top/*, gab[1].y-gab[0].y)*/;
	if ((gab[1].x-gab[0].x)>0.001 &&
		(gab[1].y-gab[0].y)>0.001)
	{
		double scale_w = frame_width / (gab[1].x-gab[0].x);
		double scale_h = frame_height / (gab[1].y-gab[0].y);
		sc = min(scale_w, scale_h);

		locZx = (draw_rect.right-draw_rect.left)/2-(gab[1].x-gab[0].x)*sc/2;
		locZy = (draw_rect.bottom-draw_rect.top)/2+(gab[1].y-gab[0].y)*sc/2;

		if (sc>0.0001)
			sgCText::Draw(sgFontManager::GetFont(m_cur_fnt),*m_cur_style_pntr,
			NULL,m_text,draw_for_draw);
	}
}
