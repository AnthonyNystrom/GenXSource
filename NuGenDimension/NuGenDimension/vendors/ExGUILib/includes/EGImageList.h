#pragma once

#include "Bitmap32.h"

class CEGmageList :
	public CImageList
{
	CBitmap32 m_bmp;
public:
	CEGmageList(void);
	~CEGmageList(void);

	int LoadBitmap( LPCTSTR lpszResourceName, HINSTANCE hInst = NULL );
	int LoadBitmap( HBITMAP hBitmap, LPWORD pwColors );

//	int Add(HICON hIcon);

	BOOL DrawMono( CDC* pDC, int nImage, POINT pt );
	BOOL Draw( CDC* pDC, int nImage, POINT pt, UINT nStyle, BOOL bDisabled = TRUE);
};