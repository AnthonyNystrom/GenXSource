#include <stdafx.h>

#include "FontPreviewDlg.h"
#include "..//Resource.h"

static  sgCFont*   current_font = NULL;
static  char comment[100];

static short  hText = 0;

SG_POINT  gab[2];
RECT      draw_rect;
HDC       draw_DC = NULL;

BOOL NEAR CALLBACK HandleNotify(HWND , LPOFNOTIFY);

static void RefillFontPic(HWND hDlg);
static void FPDrawItem(HWND hDlg, LPDRAWITEMSTRUCT lpdis);
static void setlitext(WORD i, char *buf);

DWORD GetTextExtent(HDC hDC, LPCSTR s, int len)
{
	SIZE dim;
	DWORD dw;
	GetTextExtentPoint32(hDC, s, len, &dim);
	dw = ((dim.cy << 16) & 0xFFFF0000)| dim.cx;
	return dw;
}

//Hook function for the Comm Dlg
LRESULT CALLBACK ComDlgPreviewProc(HWND hDlg, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
//int		              wtext, wlist, hlist, hctl, zbut, xnc, ync;
LPDRAWITEMSTRUCT		lpdis;
LPMEASUREITEMSTRUCT lpmis;
HDC                 hDC;

	switch (uMsg)
	{
	case WM_INITDIALOG:
		// Save off the long pointer to the OPENFILENAME structure.
		SetWindowLong(hDlg, DWL_USER, lParam);
		break;
	case WM_CTLCOLORLISTBOX:
		return (LRESULT)GetStockObject(GRAY_BRUSH);
	case WM_DESTROY:
		{
			LPOPENFILENAME lpOFN = (LPOPENFILENAME)GetWindowLong(hDlg, DWL_USER);
		}
		break;
	case WM_COMMAND:
		break;
	case WM_NOTIFY:
		//Handles Notify messages
		HandleNotify(hDlg, (LPOFNOTIFY)lParam);
	case WM_DRAWITEM:
		lpdis = (LPDRAWITEMSTRUCT)lParam;
		hText = HIWORD(GetTextExtent(lpdis->hDC, "9",1));
		//if(lpdis->itemAction & ODA_DRAWENTIRE){
			FPDrawItem(hDlg, lpdis);
			return TRUE;
		/*}
		else if(lpdis->itemAction & ODA_SELECT){
			FPSelectionItem(hDlg, lpdis);
			return TRUE;
		}*/
		return TRUE;
	case WM_MEASUREITEM:
		hDC = GetDC(hDlg);
		// Размер элемента списка
		lpmis = (LPMEASUREITEMSTRUCT)lParam;
		lpmis->itemHeight = /*HIWORD(GetTextExtent(hDC, "9",1))+ */
			LOWORD(GetTextExtent(hDC, "9",1))*4 + 13;
		lpmis->itemWidth  = LOWORD(GetTextExtent(hDC, "9",1))*4 + 13;
		ReleaseDC(hDlg, hDC);
	default:
		return FALSE;
	}
	return TRUE;
}


//Call back to handle notify messages
BOOL NEAR CALLBACK HandleNotify(HWND hDlg, LPOFNOTIFY pofn)
{
	switch (pofn->hdr.code)
	{
		// The selection has changed. 
	case CDN_SELCHANGE:
		{
			char szFile[MAX_PATH]  = "\0";
			// Get the path of the selected file.
			if (CommDlg_OpenSave_GetFilePath(GetParent(hDlg),
				szFile, sizeof(szFile)) <= sizeof(szFile))
			{
				CString tmpStr(szFile);
				//if it is a dirctory than we don't care
				if(GetFileAttributes(szFile) !=  FILE_ATTRIBUTE_DIRECTORY &&
					GetFileAttributes(szFile) != DWORD(-1) &&
					tmpStr.MakeUpper().Right(4)==".SHX")
				{

					//Should we load the Pic
				/*	if(SendDlgItemMessage(hDlg,IDC_CHECK1,BM_GETCHECK,0,0) == BST_UNCHECKED)
						//oh yes..go on..
						ShowImagePreview(hDlg,szFile);*/
					if (current_font)
						sgCFont::UnloadFont(current_font);
					current_font = sgCFont::LoadFont(szFile,comment,99);
					if (!current_font)
					{
						SetDlgItemText(hDlg,IDC_FONT_COMMENT_STRING,
							"---------------------");
						SendDlgItemMessage(hDlg, IDC_FONT_PREVIEW_LIST, LB_RESETCONTENT, 0, 0);
					}
					else
					{
						SetDlgItemText(hDlg,IDC_FONT_COMMENT_STRING,
							comment);
						RefillFontPic(hDlg);
					}
				}
				else
				{
					SetDlgItemText(hDlg,IDC_FONT_COMMENT_STRING,
						"---------------------");
					SendDlgItemMessage(hDlg, IDC_FONT_PREVIEW_LIST, LB_RESETCONTENT, 0, 0);
					if (current_font)
						sgCFont::UnloadFont(current_font);
					current_font=NULL;
				}
			}
		}
		break;
	case CDN_FILEOK:
		//return FALSE to close the Comm Dlg
		return FALSE;
		break;
	}
	return(TRUE);
}
//Shows the Comm Dialog
BOOL ShowOpenFontDialogPreview(HWND hWnd,TCHAR * szFile)
{

	OPENFILENAME OpenFileName;
	// Fill in the OPENFILENAME structure to support a template and hook.
	OpenFileName.lStructSize       = sizeof(OPENFILENAME);
	OpenFileName.hwndOwner         = hWnd;
	OpenFileName.hInstance         = AfxGetInstanceHandle();
	OpenFileName.lpstrFilter       = "SHX files (*.SHX)\0*.shx\0";
	OpenFileName.lpstrCustomFilter = NULL;
	OpenFileName.nMaxCustFilter    = 0;
	OpenFileName.nFilterIndex      = 0;
	OpenFileName.lpstrFile         = szFile;
	OpenFileName.nMaxFile          = MAX_PATH;
	OpenFileName.lpstrFileTitle    = NULL;
	OpenFileName.nMaxFileTitle     = 0;
	OpenFileName.lpstrInitialDir   = NULL;
	OpenFileName.lpstrTitle        = "Open a font file";
	OpenFileName.nFileOffset       = 0;
	OpenFileName.nFileExtension    = 0;
	OpenFileName.lpstrDefExt       = NULL;
	OpenFileName.lCustData         = NULL;
	OpenFileName.lpfnHook 		   = (LPOFNHOOKPROC)ComDlgPreviewProc;
	OpenFileName.lpTemplateName    = MAKEINTRESOURCE(IDD_FONT_PREVIEW_DLG);
	OpenFileName.Flags             = OFN_EXPLORER | OFN_ENABLEHOOK | OFN_ENABLETEMPLATE|OFN_HIDEREADONLY;

	BOOL res = GetOpenFileName(&OpenFileName);
	if (current_font)
		sgCFont::UnloadFont(current_font);
	current_font = NULL;
	return res;
}

static void RefillFontPic(HWND hDlg)
{
	if (!current_font)
		return;

	WORD	i, n;
	WORD* tab;
	
	SendDlgItemMessage(hDlg, IDC_FONT_PREVIEW_LIST, LB_RESETCONTENT, 0, 0);

	n = current_font->GetFontData()->table_size;
	tab = (WORD*)current_font->GetFontData()->symbols_table;
	if(*tab == 0){tab +=2; n -= 1;}
	for(i = 0; i < n; i++)
		SendDlgItemMessage(hDlg, IDC_FONT_PREVIEW_LIST, LB_ADDSTRING, 0,
		(LPARAM)*(tab + 2*i));
}


void draw_for_gabar_calc(SG_POINT* pb,SG_POINT* pe)
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

void draw_for_draw(SG_POINT* pb,SG_POINT* pe)
{
	double scX = (draw_rect.right-draw_rect.left)/(gab[1].x-gab[0].x);
	double scY = (draw_rect.bottom-draw_rect.top)/(gab[1].y-gab[0].y);
	MoveToEx(draw_DC, (int)(draw_rect.left+(pb->x-gab[0].x)*scX), 
		(int)(draw_rect.bottom-(pb->y-gab[0].y)*scY),NULL);
	LineTo(draw_DC, (int)(draw_rect.left+(pe->x-gab[0].x)*scX), 
		(int)(draw_rect.bottom-(pe->y-gab[0].y)*scY));
}

//#pragma argsused
static void FPDrawItem(HWND hDlg, LPDRAWITEMSTRUCT lpdis)
{
	if (!current_font)
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

	/*FillRect (hDC, (LPRECT)&draw_rect,
		(lpdis->itemState & ODS_SELECTED) ? (HBRUSH)GetStockObject(DKGRAY_BRUSH):
								(HBRUSH)GetStockObject(GRAY_BRUSH));
	SetBkMode(hDC, TRANSPARENT);
	SetTextColor(hDC, (lpdis->itemState & ODS_SELECTED) ? RGB(255,255,255) : RGB(0,0,255));
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
	sgCText::Draw(current_font,stl,NULL,buf,draw_for_gabar_calc);
	if(gab[0].x == 1.e35f) return; // Пустой текст

	unsigned char nUp = current_font->GetFontData()->posit_size;
	if(nUp)
	{
		dx = stl.height/nUp;
		p.y = -dx*current_font->GetFontData()->negat_size;
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
	sgCText::Draw(current_font,stl,NULL,buf,draw_for_draw);
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


