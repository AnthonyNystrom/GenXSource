// SpecSymbolsListBox.cpp : implementation file
//

#include "stdafx.h"
#include "SpecSymbolsListBox.h"

#include <math.h>

static unsigned int loc_cur_fnt = -1;

// CSpecSymbolsListBox

IMPLEMENT_DYNAMIC(CSpecSymbolsListBox, CListBox)
CSpecSymbolsListBox::CSpecSymbolsListBox()
{
	m_cur_fnt = -1;
}

CSpecSymbolsListBox::~CSpecSymbolsListBox()
{
}


BEGIN_MESSAGE_MAP(CSpecSymbolsListBox, CListBox)
END_MESSAGE_MAP()

void    CSpecSymbolsListBox::SetCurFont(unsigned int cf)
{
	m_cur_fnt = cf;
	loc_cur_fnt = cf;
}


static DWORD GetTextExtent(HDC hDC, LPCSTR s, int len)
{
	SIZE dim;
	DWORD dw;
	GetTextExtentPoint32(hDC, s, len, &dim);
	dw = ((dim.cy << 16) & 0xFFFF0000)| dim.cx;
	return dw;
}

void CSpecSymbolsListBox::MeasureItem(LPMEASUREITEMSTRUCT lpMeasureItemStruct)
{
	/*CDC* hDC = GetDC();
	// Размер элемента списка
	lpMeasureItemStruct->itemHeight = HIWORD(GetTextExtent(hDC->m_hDC,"9",1))+ 
		LOWORD(GetTextExtent(hDC->m_hDC, "9",1))*4 + 3;
	lpMeasureItemStruct->itemWidth  = LOWORD(GetTextExtent(hDC->m_hDC, "9",1))*4 + 3;
	//SetColumnWidth(LOWORD(GetTextExtent(hDC->m_hDC, "9",1))*4 + 3);
	ReleaseDC(hDC);*/
}


static short  hText = 0;

static SG_POINT  gab[2];
static RECT      draw_rect;
static HDC       draw_DC = NULL;



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
	double scX = (draw_rect.right-draw_rect.left)/(gab[1].x-gab[0].x);
	double scY = (draw_rect.bottom-draw_rect.top)/(gab[1].y-gab[0].y);
	MoveToEx(draw_DC, (int)(draw_rect.left+(pb->x-gab[0].x)*scX), 
		(int)(draw_rect.bottom-(pb->y-gab[0].y)*scY),NULL);
	LineTo(draw_DC, (int)(draw_rect.left+(pe->x-gab[0].x)*scX), 
		(int)(draw_rect.bottom-(pe->y-gab[0].y)*scY));
}

static void setlitext(WORD i, char *buf);

//#pragma argsused
static void FPDrawItem(HWND hDlg, LPDRAWITEMSTRUCT lpdis)
{
	if (!sgFontManager::GetFont(loc_cur_fnt))
		return;

	RECT		rc;
	char        buf[10];
	WORD        i;
	//	D_POINT     gab[2], p = ;
	//	F_WINDOW    w;
	SG_POINT    p = {0, 0, 0};
	double      dx;
	HDC         hDC;


	hDC = lpdis->hDC;
	CopyRect ((LPRECT)&rc, (LPRECT)&lpdis->rcItem);
	/*hOldPen = SelectObject(hDC,
	(lpdis->itemState & ODS_SELECTED) ? hBLUEPen:hBTNSHADOWPen);*/
	MoveToEx(hDC, rc.left, rc.top, NULL);
	LineTo(hDC, rc.left, rc.bottom);
	LineTo(hDC, rc.right, rc.bottom);
	LineTo(hDC, rc.right, rc.top);
	LineTo(hDC, rc.left, rc.top);
	//SelectObject(hDC,hOldPen);

	rc.left +=  1;
	rc.right -= 1;
	rc.top += 1;
	rc.bottom -= 1;

	FillRect (hDC, (LPRECT)&rc, (HBRUSH)GetStockObject(WHITE_BRUSH));

	draw_rect	= rc;
	draw_rect.top	 = rc.bottom - hText;

	i = (WORD)lpdis->itemData;
	setlitext(i, buf);

	/*FillRect (hDC, (LPRECT)&draw_rect,(HBRUSH)GetStockObject(GRAY_BRUSH));
	SetBkMode(hDC, TRANSPARENT);
	SetTextColor(hDC, RGB(0,0,255));
	DrawText(hDC, &buf[2], lstrlen(&buf[2]),&draw_rect, DT_SINGLELINE|DT_CENTER);*/
	draw_rect = rc;
	draw_rect.left+=1;
	draw_rect.top+=1;
	draw_rect.right-=1;
	draw_rect.bottom-=1;
	draw_rect.bottom = rc.bottom /*- hText*/;

	gab[0].x = gab[0].y = gab[0].z =  1.e35f;
	gab[1].x = gab[1].y = gab[1].z = -1.e35f;

	SG_TEXT_STYLE stl = {0, 5.0, 100.0, 0.0, 0.0, 50.0};
	sgCText::Draw(sgFontManager::GetFont(loc_cur_fnt),stl,NULL,buf,draw_for_gabar_calc);
	if(gab[0].x == 1.e35f) return; // Пустой текст

	unsigned char nUp = sgFontManager::GetFont(loc_cur_fnt)->GetFontData()->posit_size;
	if(nUp)
	{
		dx = stl.height/nUp;
		p.y = -dx*sgFontManager::GetFont(loc_cur_fnt)->GetFontData()->negat_size;
		draw_for_gabar_calc(&p,&p);
		p.x = p.y = dx*nUp;
		draw_for_gabar_calc(&p,&p);
	}

	draw_rect.left +=1;
	draw_rect.top +=1;
	draw_rect.right -=1;
	draw_rect.bottom -=1;

	if (fabs(gab[0].x-gab[1].x)<0.001) return;
	if (fabs(gab[0].y-gab[1].y)<0.001) return;
	/*
	f_init_window(&w);
	f_screen_limit(&w, rc1.left+1, rc1.top+1, rc1.right-1, rc1.bottom-1);

	f_mat_limit(&w, gab[0].x, gab[0].y, gab[1].x, gab[1].y);
	fui->hDC = lpdis->hDC;
	draw_text(fui->font, fui->style, (UCHAR*)buf, draw_text_by_DC, &w);*/

	draw_DC = hDC;
	sgCText::Draw(sgFontManager::GetFont(loc_cur_fnt),stl,NULL,buf,draw_for_draw);
}


static void setlitext(WORD i, char *buf)
{
	buf[0] = buf[1] = '%';
	buf[2] = buf[3] = buf[4] = buf[5] = 0;
	if(i < 100) buf[2] = '0';
	if(i < 10) buf[3] = '0';
	//itoa(i, &buf[lstrlen(buf)], 10);#OBSOLETE RISK
	_itoa_s(i, &buf[lstrlen(buf)],1024, 10);
}




void CSpecSymbolsListBox::DrawItem(LPDRAWITEMSTRUCT lpDrawItemStruct)
{
	hText = HIWORD(GetTextExtent(lpDrawItemStruct->hDC, "9",1));
	FPDrawItem(this->m_hWnd, lpDrawItemStruct);
}


