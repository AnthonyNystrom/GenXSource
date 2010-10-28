// HeaderControlEx.cpp : implementation file
//

#include "stdafx.h"
#include "EGHeaderControl.h"
#include "EGMenu.h"

#include <windowsx.h>

// CHeaderControlEx

IMPLEMENT_DYNAMIC(CEGHeaderControl, CHeaderCtrl)
CEGHeaderControl::CEGHeaderControl()
{
	m_oldWndProc = NULL;
	m_nPressed = -2;
}

CEGHeaderControl::~CEGHeaderControl()
{
}


BEGIN_MESSAGE_MAP(CEGHeaderControl, CHeaderCtrl)
	ON_MESSAGE (HDM_LAYOUT, OnHDMLayout)
	ON_MESSAGE (HDM_HITTEST, OnHDMHitTest)
	ON_MESSAGE (WM_PAINT, OnWMPaintEx)
	ON_MESSAGE (WM_LBUTTONDOWN, OnLButtonDown)
	ON_MESSAGE (WM_LBUTTONUP, OnLButtonUp)
END_MESSAGE_MAP()



// CEGHeaderControl message handlers

LRESULT CEGHeaderControl::OnHDMLayout(WPARAM wParam, LPARAM lParam){
	HDLAYOUT* lphl = (HDLAYOUT*)lParam;
	
	CallWindowProc(m_oldWndProc, m_hWnd, HDM_LAYOUT, wParam, lParam);
	
	if(lphl->pwpos && lphl->pwpos->cy){
		lphl->prc->top = HeaderExTitleHeight()+5;
		lphl->pwpos->cy = HeaderExTitleHeight()+4;
	}
	
	return TRUE;
}

int CEGHeaderControl::GetLineHeight(){
	SIZE sz;
	HDC dc = ::GetDC(m_hWnd);
	HFONT hfnt = GetWindowFont(::GetParent(m_hWnd));
	HFONT ofnt = SelectFont(dc,hfnt);
	GetTextExtentPoint32( dc, _T("W") , 1, &sz );
	SelectFont(dc,ofnt);
	::ReleaseDC(m_hWnd, dc);
	return sz.cy;
}

LRESULT CEGHeaderControl::OnHDMHitTest(WPARAM wParam, LPARAM lParam){
	CallWindowProc(m_oldWndProc, m_hWnd, HDM_HITTEST, wParam, lParam);
	HDHITTESTINFO* phti = (HDHITTESTINFO*)lParam;
	if (HHT_ONHEADER == phti->flags || HHT_ONDIVOPEN == phti->flags || HHT_ONDIVIDER == phti->flags){
		TCHAR out[500];
		HDITEM hdi;
		hdi.mask = HDI_TEXT;
		hdi.pszText = out;
		hdi.cchTextMax = sizeof(out);
		if (Header_GetItem(m_hWnd, phti->iItem, &hdi)){
			int nBottom = 0;
			TCHAR* szFind = out;
			while( NULL != (szFind = _tcschr(szFind, _T('\n') ) ) ){
				nBottom++;
				szFind++;
			}
			if (phti->pt.y < nBottom * GetLineHeight())
				phti->flags = HHT_NOWHERE;
		}
	}
	return 0;

}

LRESULT CEGHeaderControl::OnWMPaintEx(WPARAM wParam, LPARAM lParam){
	SendMessage(WM_SETREDRAW, FALSE, 0);
	CallWindowProc(m_oldWndProc, m_hWnd, WM_PAINT, wParam, lParam);
	SendMessage(WM_SETREDRAW, TRUE, 0);
	HeaderEx_Title();
	return 0;
}

LRESULT CEGHeaderControl::OnLButtonDown(WPARAM wParam, LPARAM lParam){
	int nOldPressed = m_nPressed;
	HDHITTESTINFO hti;
	hti.pt.x = LOWORD(lParam);
	hti.pt.y = HIWORD(lParam);
	SendMessage(HDM_HITTEST, 0, (LPARAM)&hti );
	m_nPressed = (HHT_ONHEADER == ( hti.flags & HHT_ONHEADER) ) ? hti.iItem : -1 ;
	if (nOldPressed != m_nPressed){
		RECT rc;
		Header_GetItemRect(m_hWnd, m_nPressed, &rc);
		InvalidateRect(&rc, TRUE);
	}
	return CallWindowProc(m_oldWndProc, m_hWnd, WM_LBUTTONDOWN, wParam, lParam);
}

LRESULT CEGHeaderControl::OnLButtonUp(WPARAM wParam, LPARAM lParam){
	if ( m_nPressed != -1 ){
		RECT rc;
		Header_GetItemRect(m_hWnd, m_nPressed, &rc);
		m_nPressed = -1;
		InvalidateRect(&rc, TRUE);
	}		
	return CallWindowProc(m_oldWndProc, m_hWnd, WM_LBUTTONUP, wParam, lParam);
}

void CEGHeaderControl::PreSubclassWindow()
{
	m_oldWndProc = (WNDPROC) (LONG_PTR) ::GetWindowLong(m_hWnd, GWLP_WNDPROC);

	CHeaderCtrl::PreSubclassWindow();
}


/*
    А эта ф-ция написана для создания градаций
	У хедера есть наворот - когда цветов больше 256,
	рисует "выпуклый" заголовок
 */
DWORD CEGHeaderControl::OffsetColor(DWORD color,BYTE offset,BOOL dir)
{
	DWORD ncolor = color;
	if(!dir)
	{
		if(GetRValue(ncolor)>offset) ncolor-=offset;				else ncolor&=0xffffff00;
		if(GetGValue(ncolor)>offset) ncolor-=(DWORD)(offset<<8);	else ncolor&=0xffff00ff;
		if(GetBValue(ncolor)>offset) ncolor-=(DWORD)(offset<<16);	else ncolor&=0xff00ffff;
	}
	else
	{
		if((GetRValue(ncolor)+offset)<0x100) ncolor+=offset;				else ncolor|=0x000000ff;
		if((GetGValue(ncolor)+offset)<0x100) ncolor+=(DWORD)(offset<<8);	else ncolor|=0x0000ff00;
		if((GetBValue(ncolor)+offset)<0x100) ncolor+=(DWORD)(offset<<16);	else ncolor|=0x00ff0000;
	}
	return ncolor;
}



/*
	Функция расчета высоты заголовка
	Для чего просто считает количество переводов строки и
	умножает на высоту буквы "W" - стандартно для MSDN
	Ну и добавляет высоту рамки, которой будет окружен
	элемент заголовка
*/
int CEGHeaderControl::HeaderExTitleHeight()
{
	int titleheight = 0;
	TCHAR out[500];
	HDITEM hdi;
	hdi.mask = HDI_TEXT;
	hdi.pszText = out;
	hdi.cchTextMax = sizeof(out);

	int cnt = Header_GetItemCount(m_hWnd);
	for(int i=0;i<cnt;i++){
		Header_GetItem(m_hWnd, i, &hdi);
		int tmp=0; TCHAR *a=out;
		while(a) { tmp++; a = _tcschr( a + 1, _T('\n') ); }
		if(titleheight<tmp) titleheight=tmp;
	}
	return titleheight * GetLineHeight();
}

/*
	Эта ф-ция аналог DrawFrameControl, только заполняет прямоугольник
	градацией, если имеем более 256 цветов
	Спросите, почему не воспользоваться стандартными ф-циями?
	Дык это должно работать и на Вин95 (по заданию проекта, для которого
	весь этот сыр-бор мутился)
 */
BOOL CEGHeaderControl::DrawFrameControlEx(HDC hdc, LPRECT lprc, UINT uType, UINT uState)
{
	UINT nMenuDrawMode = CEGMenu::GetMenuDrawMode();
	BOOL ret = FALSE;
	if(uType == DFC_BUTTON && GetDeviceCaps(hdc,BITSPIXEL)>=16 && 
		(nMenuDrawMode == CEGMenu::STYLE_XP_2003_NOBORDER  ||
		 nMenuDrawMode == CEGMenu::STYLE_XP_2003  ||
		 nMenuDrawMode == CEGMenu::STYLE_COLORFUL_NOBORDER  ||
		 nMenuDrawMode == CEGMenu::STYLE_COLORFUL ||
		 nMenuDrawMode == CEGMenu::STYLE_ICY_NOBORDER  ||
		 nMenuDrawMode == CEGMenu::STYLE_ICY
 		)
	  )
	{

		int offset = 32;
		DWORD cl[2] = { OffsetColor(GetSysColor(COLOR_BTNFACE), (BYTE) offset / 2, TRUE), OffsetColor(GetSysColor(COLOR_BTNFACE), (BYTE) offset / 2, FALSE) };
		if((uState&DFCS_PUSHED) == DFCS_PUSHED)
		{
			DWORD cll = cl[0];
			cl[0] = cl[1];
			cl[1] = cll;
		}
		int r1 = GetRValue(cl[0]);
		int g1 = GetGValue(cl[0]);
		int b1 = GetBValue(cl[0]);
		int r2 = GetRValue(cl[1]);
		int g2 = GetGValue(cl[1]);
		int b2 = GetBValue(cl[1]);
		HPEN pen = CreatePen(PS_SOLID, 1, cl[0]);
		HPEN opn = SelectPen(hdc, pen);
		for(int y=0;y<lprc->bottom-lprc->top;y++)
		{
			DWORD cll = RGB(
				r1+(r2-r1)*y/(lprc->bottom-lprc->top),
				g1+(g2-g1)*y/(lprc->bottom-lprc->top),
				b1+(b2-b1)*y/(lprc->bottom-lprc->top));
		// Видимо это старый кусок кода, т.к. я тут
		// юзал GetNearestColor для создания пера или
		// пропуска его создания, которая имеется и
		// в Вин95.
		//   Оставил так, т.к. ни где не глючило, да и
		//   ресурсы высвобождаются своевремено
			HPEN tmp = CreatePen(PS_SOLID, 1, cll);
			SelectPen(hdc, tmp);
			DeletePen(pen);
			pen = tmp;
			MoveToEx(hdc, lprc->left, lprc->top+y, NULL);
			LineTo(hdc, lprc->right, lprc->top+y);
		}
		SelectPen(hdc, opn);
		DeletePen(pen);
		if ((uState&DFCS_PUSHED) != DFCS_PUSHED)
			DrawEdge(hdc, lprc, BDR_RAISEDINNER, BF_RECT);

	} else if( uType == DFC_BUTTON && GetDeviceCaps(hdc,BITSPIXEL)>=16 && 
		(nMenuDrawMode == CEGMenu::STYLE_XP_NOBORDER  ||
		 nMenuDrawMode == CEGMenu::STYLE_XP  )
	  )
	{
		HBRUSH hbr = ::CreateSolidBrush( ::GetSysColor( COLOR_BTNFACE ) );
		::FillRect( hdc, lprc, hbr );
		::DeleteObject( hbr );
		if ((uState&DFCS_PUSHED) == DFCS_PUSHED) {
			::DrawEdge(hdc, lprc, BDR_SUNKENINNER, BF_RECT);
		} else {
			::DrawEdge(hdc, lprc, BDR_RAISEDINNER, BF_RECT);
		}

	} else { // NATIVE THEME
	// А так рисуем, если комп-гавно, а работать нуна!
	// Или, вдруг, просим рисовать не кнопку.
	// Я же отдаю все это вам, а что попросите рисовать,
	// не в курсах
		ret = DrawFrameControl(hdc, lprc, uType, uState);
	}

	return ret;
}


/*
	Эта функция рассчитывает положение текста в рамке
	точно в центре, ну и, конечно же, именно там и
	рисует.
	Вопрос: Почему не воспользоваться флажком DT_VCENTER?
	Ответ:  Дык, он не пашет с DT_WORDBREAK!
 */
BOOL CEGHeaderControl::HeaderExVCenter(HDC dc, TCHAR* out, RECT* r, BOOL bPressed)
{
	RECT rr,r1;
	CopyRect(&r1,r);
	CopyRect(&rr,r);
	if(((r1.right-1)>r1.left)&&((r1.bottom-7)>r1.top))
	{
		DrawFrameControlEx(dc,&r1,DFC_BUTTON ,DFCS_BUTTONPUSH | (bPressed ? DFCS_PUSHED : 0) );
		DrawText(dc,out,-1,&r1,DT_WORDBREAK|DT_CENTER|DT_CALCRECT);
		int dist = ((rr.bottom-rr.top)-(r1.bottom-r1.top))/2;
		r1.top+=dist; r1.bottom+=dist; r1.right = rr.right;
		if(dist<0) CopyRect(&r1,&rr);
		DrawText(dc,out,-1,&r1,DT_WORDBREAK|DT_CENTER);
	}
	return TRUE;
}

/*
	Эта функция моя гордость или наоборот.
	Комментировать выбор странных входных переменных, типа
		RECT **r, char** out
	не буду, типа нужно вникнуть в ее алгоритм, а это не
	тривиально. Да и проблемы у мя с выражениями :)
	Скажу одно - функция рисует все шапочки за один проход.
	Судите сами о ее сложности по этому высказыванию...
 */
void CEGHeaderControl::HeaderEx_SetTit(HDC dc, RECT **r, TCHAR** out, LPHEADEREXSHOWDATA* l, int pos, int maxstr)
{
	RECT r1,r2;
	TCHAR tmp[1000];
	while(*l)
	{
		SetRect(&r1,(*r)->right,(*r)->top,(*r)->right+(*l)->width,(*r)->bottom);
		TCHAR *a=(*out);
		for(int i=0;i<=pos;i++)
		{
			a=_tcschr(a+1,'\n');
			if(!a) { a=(*out) + lstrlen(*out); break; }
		}
		int alen = ( int ) ( a - ( *out ) );
		if(*a=='\n' && !_tcsncmp((*out),(*l)->szName,alen))
		{
			int rl=(*r)->left;
			TCHAR* b=a=(*out);
			for(int i=0;i<pos;i++)
			{
				a=_tcschr(a+1,'\n');
				if(!a) { a=(*out)+lstrlen(*out); break; }
			}
			int aa = ( int ) ( _tcschr( a + 1, '\n' ) - a ) ;
			while(!_tcsncmp(b,(*out),alen)) HeaderEx_SetTit(dc, r, out, l, pos+1, maxstr);
			SetRect(&r2,rl,(*r)->top+((*r)->bottom-(*r)->top)*pos/maxstr,(*r)->left,(*r)->top+((*r)->bottom-(*r)->top)*(pos+1)/maxstr);
			lstrcpyn(tmp,a+(*a=='\n' ? 1 : 0),aa+(*a=='\n' ? 0 : 1));
			HeaderExVCenter(dc,tmp,&r2, FALSE); //nIndex == nPressed);
			if(pos) return;
			continue;
		}
		a=(*out);
		for(int i=0;i<pos;i++)
		{
			a= _tcschr(a+1, '\n');
			if(!a) { a=(*out)+lstrlen(*out); break; }
			a++;
		}
		SetRect(&r2,(*r)->left,(*r)->top+((*r)->bottom-(*r)->top)*pos/maxstr,(*r)->right,(*r)->bottom);
		HeaderExVCenter(dc,a,&r2, (*l)->nIndex == m_nPressed+1);
		CopyRect(*r,&r1);
		if( _tcsncmp((*out),(*l)->szName,a-(*out))) break;
		if(*l) { (*out)=(*l)->szName; (*l)=(*l)->next; }
	}
	if(*l) { (*out)=(*l)->szName; (*l)=(*l)->next; }
	SetRect(&r1,(*r)->right,(*r)->top,2000,(*r)->bottom);
	//HeaderExVCenter(dc,"",&r1,FALSE);
	FillRect(dc, &r1, GetStockBrush(WHITE_BRUSH) );
}


/*
	А эта ф-ция готовит данные для крутик-функции
	Ни чо замечательного в ней нет :о(
 */
void CEGHeaderControl::HeaderEx_Title()
{
	int titleheight = 0;
	RECT r, rw, rc, *r1=&r; TCHAR *out= _T("•");
	GetWindowRect(&r);
	GetWindowRect(&rw);
	GetClientRect(&rc);
	titleheight = rw.bottom-rw.top;
	HDC dcc = ::GetDC(m_hWnd);
	HDC dc = ::CreateCompatibleDC(dcc);
	HBITMAP bmp = ::CreateCompatibleBitmap(dcc,r.right-r.left,titleheight);
	HBITMAP oldbmp = SelectBitmap(dc, bmp);
//	HDC dc = GetDC(m_hWnd);
	SetBkMode(dc,TRANSPARENT);
	HFONT oldfont = SelectFont(dc,GetWindowFont(::GetParent(m_hWnd)));
	int maxstr=0;
	int cnt = Header_GetItemCount(m_hWnd);
	if(cnt)
	{
		int* ar = new int[cnt];
		Header_GetOrderArray(m_hWnd, cnt, ar);
		LPHEADEREXSHOWDATA ldata = new HEADEREXSHOWDATA[cnt+1];
		for(int i=0;i<cnt;i++)
		{
			memset(&ldata[i],0,sizeof(ldata[i]));
			HDITEM hdi;
			hdi.mask = HDI_WIDTH|HDI_TEXT;
			hdi.cxy = 0;
			hdi.pszText = ldata[i].szName;
			hdi.cchTextMax = sizeof(ldata[i].szName);
			Header_GetItem(m_hWnd, ar[i], &hdi);
			ldata[i].width = hdi.cxy;
			ldata[i].next = &ldata[i+1];
			ldata[i].nIndex = ar[i];//(cnt-1) == ar[i] ? 0 : ar[i] + 1;
		}
		memset(&ldata[cnt],0,sizeof(ldata[cnt]));
		lstrcpy(ldata[cnt].szName,_T("          "));
		ldata[cnt].nIndex = cnt;
		for(LPHEADEREXSHOWDATA l = ldata;l!=NULL;l=l->next)
		{
			int tmp=0; TCHAR *a=l->szName;
			while(a) { tmp++; a=_tcschr(a+1,'\n'); }
			if(maxstr<tmp) maxstr=tmp;
		}

		SetRect(&r,0,0,0,titleheight);
		LPHEADEREXSHOWDATA l = ldata;
		HeaderEx_SetTit(dc, &r1, &out, &l, 0, maxstr);

		delete [] ldata;
		delete [] ar;
	}
	else
	{
		SetRect(&r,rc.left,rc.top,rc.right-rc.left,rc.bottom-rc.top);
		DrawFrameControlEx(dc,&r,DFC_BUTTON,DFCS_BUTTONPUSH);
	}
	SelectObject(dc,oldfont);
//	ReleaseDC(m_hWnd, dc);
	SetRect(&r,rc.left,rc.top,rc.right-rc.left,rc.bottom-rc.top);
	BitBlt(dcc,r.left,r.top,r.right,r.bottom, dc,0,0,SRCCOPY);
	SelectBitmap(dc, oldbmp);
	DeleteBitmap(bmp);
	DeleteDC(dc);
	::ReleaseDC(m_hWnd, dcc);
}
