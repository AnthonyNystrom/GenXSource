#include "stdafx.h"
#include "EGImageList.h"

CEGmageList::CEGmageList(void)
{

}

CEGmageList::~CEGmageList(void)
{

}

int CEGmageList::LoadBitmap( LPCTSTR lpszResourceName, HINSTANCE hInst ){
	
	if(GetSafeHandle())
		DeleteImageList();

	m_bmp.LoadBitmap( lpszResourceName, hInst );

	Create(m_bmp.GetHeight(), m_bmp.GetHeight(), ILC_COLOR32 | ILC_MASK, (int)m_bmp.GetWidth()/m_bmp.GetHeight(), 1);
	
	CImageList::Add( (CBitmap*)&m_bmp, (CBitmap*) NULL );
	
	return TRUE;
}


int CEGmageList::LoadBitmap( HBITMAP hBitmap, LPWORD pwColors ){
	
	if(GetSafeHandle())
		DeleteImageList();

	m_bmp.Attach( hBitmap, pwColors );

	Create(m_bmp.GetHeight(), m_bmp.GetHeight(), ILC_COLOR32 | ILC_MASK, (int)m_bmp.GetWidth()/m_bmp.GetHeight(), 1);
	
	CImageList::Add( (CBitmap*)&m_bmp, (CBitmap*) NULL );

	return TRUE;
}

/*
int CEGmageList::Add(HICON hIcon) {

	return CImageList::Add( hIcon );
}
*/

BOOL CEGmageList::DrawMono( CDC* pDC, int nImage, POINT pt ){

	CRect rcDest( pt, CSize( m_bmp.GetHeight(), m_bmp.GetHeight() ) );
	CRect rcSrc( CPoint( m_bmp.GetHeight()*nImage, 0), CSize( m_bmp.GetHeight(), m_bmp.GetHeight() ) );
	m_bmp.Draw(pDC, &rcDest, &rcSrc, 2 );

	return TRUE;	
}

BOOL CEGmageList::Draw( CDC* pDC, int nImage, POINT pt, UINT /*nStyle*/, BOOL bDisabled ){

	CRect rcDest( pt, CSize( m_bmp.GetHeight(), m_bmp.GetHeight() ) );
	CRect rcSrc( CPoint( m_bmp.GetHeight()*nImage, 0), CSize( m_bmp.GetHeight(), m_bmp.GetHeight() ) );
	m_bmp.Draw(pDC, &rcDest, &rcSrc, bDisabled ? 1 : 0 );

	return TRUE;	
}

