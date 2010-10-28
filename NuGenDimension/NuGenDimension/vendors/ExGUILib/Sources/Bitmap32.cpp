#include "StdAfx.h"
#include "bitmap32.h"

#include <vector>
#include <math.h>

using std::vector;

HBITMAP LoadBitmap32 ( LPCTSTR lpszResourceName, LPLONG lpnWidth, LPLONG lpnHeight, LPWORD lpnBitCount, HINSTANCE hInst ) {
	HBITMAP hBitmap = NULL;
	if ( NULL == hInst )
		hInst = ::AfxFindResourceHandle( lpszResourceName, RT_BITMAP);
	HRSRC hRsrc = ::FindResource(hInst, lpszResourceName, RT_BITMAP);
	if ( hRsrc ){
		HGLOBAL hglb = LoadResource(hInst, hRsrc);
		if ( hglb ){
			// Читаем заголовок
			LPBITMAPINFO pbi = (LPBITMAPINFO)LockResource(hglb);
			if (pbi ) {
				if( pbi->bmiHeader.biBitCount >= 24 ) { 
					if ( lpnWidth )
						*lpnWidth = pbi->bmiHeader.biWidth;
					if ( lpnHeight )
						*lpnHeight = pbi->bmiHeader.biHeight;
					if ( lpnBitCount )
						*lpnBitCount = pbi->bmiHeader.biBitCount;
					// Читаем данные
					HDC hdc = GetDC( NULL );
	//				BYTE* pData = (BYTE*)pbi + sizeof(BITMAPINFO) + pbi->bmiHeader.biClrUsed * sizeof(COLORREF);
					//BYTE* pData = (BYTE*)pbi + sizeof(BITMAPINFOHEADER) + pbi->bmiHeader.biClrUsed * sizeof(COLORREF);
					BYTE* pData = (BYTE*)((LPSTR)pbi + (WORD)(pbi->bmiHeader.biSize));
					hBitmap = CreateDIBitmap( hdc, &pbi->bmiHeader, CBM_INIT, (void*)pData, pbi, DIB_RGB_COLORS);
					::ReleaseDC	(NULL, hdc);
				}
			}
			FreeResource( hglb );
		}	
	}
	if ( NULL == hBitmap )
		hBitmap = (HBITMAP)LoadImage( hInst, lpszResourceName, IMAGE_BITMAP, 0, 0, LR_CREATEDIBSECTION ); 
	return hBitmap;
}


BOOL GetBitmapRGBA( HBITMAP hBitmap, LPRGBA8 lpRGBA, LPWORD lpwColors, BOOL bForceAlpha = TRUE, COLORREF clrTransp = CLR_NONE ) {

	BITMAP bmp;
	if ( !::GetObject(hBitmap, sizeof(BITMAP), &bmp) ) 
		return FALSE;
	if ( lpwColors )
		bmp.bmBitsPixel = *lpwColors;

	// читаем данные в 32bpp, независимо от реальных данных
	BITMAPINFOHEADER bmiHeader;
	memset(&bmiHeader, 0, sizeof(bmiHeader));

	bmiHeader.biSize = sizeof(bmiHeader);
	bmiHeader.biWidth = bmp.bmWidth;
	bmiHeader.biHeight = bmp.bmHeight;
	bmiHeader.biPlanes = 1;
	bmiHeader.biBitCount = 32;
	bmiHeader.biCompression = BI_RGB;

	HDC hdc = GetDC(NULL);
	GetDIBits(hdc, hBitmap, 0, bmp.bmHeight, lpRGBA, (BITMAPINFO *)&bmiHeader, DIB_RGB_COLORS);
	ReleaseDC( NULL, hdc );

	// дорабатываем до альфа-картинки
	if ( 32 != bmp.bmBitsPixel && bForceAlpha ) {
		LPRGBA8 clrTransparent = ( CLR_NONE == clrTransp ) ?  &lpRGBA[0] : (LPRGBA8)&clrTransp ;
		
		for ( int x = 0; x < bmp.bmWidth; ++x )
			for ( int y = 0; y < bmp.bmHeight; ++y ) {
				LPRGBA8 rgba = &lpRGBA[ x + y * bmp.bmWidth ];
				if ( !(rgba->red == clrTransparent->red && 
					 rgba->blue == clrTransparent->blue &&
					 rgba->green == clrTransparent->green) )
				rgba->alpha = 255; // делаем прозрачным
			}
	}
	return TRUE;
}

HBITMAP CombineResources( int cy, ... ) {

	HBITMAP hBitmap = NULL;
	DWORD nResourceID;
	
	int nTotalWidth = 0;
	BOOL bFailed = FALSE;

	// Читаем ресурсы
	vector<LPBITMAP>::iterator itSrc;
	vector<LPBITMAP> lstSrc;	
	va_list list;
	va_start( list, cy );
	while( NULL != ( nResourceID = va_arg( list, DWORD) ) ){
		LPBITMAP bmpSrc = new BITMAP;

		// Читаем картинку
		HBITMAP hSrc = LoadBitmap32( MAKEINTRESOURCE( nResourceID ), &bmpSrc->bmWidth, &bmpSrc->bmHeight, &bmpSrc->bmBitsPixel );
		if ( !hSrc ) {
			delete bmpSrc;
			bFailed = TRUE;
			break;
		}
		nTotalWidth += bmpSrc->bmWidth;

		// Читаем биты
		bmpSrc->bmBits = malloc( sizeof(RGBA8) *  bmpSrc->bmWidth *  bmpSrc->bmHeight );
		if ( !GetBitmapRGBA(hSrc, (LPRGBA8) bmpSrc->bmBits, &bmpSrc->bmBitsPixel, TRUE) ) {
			free( bmpSrc->bmBits );
			delete bmpSrc;
			bFailed = TRUE;
			break;
		}
	
		lstSrc.push_back( bmpSrc );
	}
	va_end( list ); 
	
	// Собственно обработка
	if ( !bFailed )
		hBitmap = Combine( nTotalWidth, cy, &lstSrc );

	// Очистка ресурсов
	for ( itSrc = lstSrc.begin(); itSrc != lstSrc.end(); ++itSrc ) {
		free( (*itSrc)->bmBits );
		delete (*itSrc);
	}

	return hBitmap;
}

HBITMAP CombineBitmaps( int cy, ... ) {
	HBITMAP hBitmap = NULL;
	HBITMAP hSrc;
	
	int nTotalWidth = 0;
	BOOL bFailed = FALSE;

	// Читаем ресурсы
	vector<LPBITMAP>::iterator itSrc;
	vector<LPBITMAP> lstSrc;	
	va_list list;
	va_start( list, cy );
	while( NULL != ( hSrc = va_arg( list, HBITMAP ) ) ){
		LPBITMAP bmpSrc = new BITMAP;

		// Читаем картинку
		if ( !::GetObject( bmpSrc, sizeof(BITMAP), &bmpSrc ) ) {
			delete bmpSrc;
			bFailed = TRUE;
			break;
		}
		nTotalWidth += bmpSrc->bmWidth;

		// Читаем биты
		bmpSrc->bmBits = malloc( sizeof(RGBA8) *  bmpSrc->bmWidth *  bmpSrc->bmHeight );
		if ( !GetBitmapRGBA(hSrc, (LPRGBA8) bmpSrc->bmBits, &bmpSrc->bmBitsPixel, TRUE) ) {
			free( bmpSrc->bmBits );
			delete bmpSrc;
			bFailed = TRUE;
			break;
		}
	
		lstSrc.push_back( bmpSrc );
	}
	va_end( list ); 
	
	// Собственно обработка
	if ( !bFailed )
		hBitmap = Combine( nTotalWidth, cy, &lstSrc );

	// Очистка ресурсов
	for ( itSrc = lstSrc.begin(); itSrc != lstSrc.end(); ++itSrc ) {
		free( (*itSrc)->bmBits );
		delete (*itSrc);
	}

	return hBitmap;
}

HBITMAP Combine( int cx, int cy, vector<LPBITMAP> *plstSrc ) {

	HBITMAP hBitmap = NULL;

	// Создаем чистую картинку
	BITMAPINFO bmi;
	memset( &bmi, 0, sizeof(BITMAPINFO) );

	bmi.bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
	bmi.bmiHeader.biWidth = cx;
	bmi.bmiHeader.biHeight = cy;
	bmi.bmiHeader.biPlanes = 1;
	bmi.bmiHeader.biBitCount = 32;
	bmi.bmiHeader.biCompression = BI_RGB;
	bmi.bmiHeader.biSizeImage = bmi.bmiHeader.biWidth * bmi.bmiHeader.biHeight * 4;

	LPRGBA8 lpDestBits = NULL;
	hBitmap = CreateDIBSection( NULL, &bmi, DIB_RGB_COLORS, (void **)&lpDestBits, NULL, NULL ); 

	// Копируем из источников
	vector<LPBITMAP>::iterator itSrc;
	itSrc = plstSrc->begin();
	int nSrcOffset = 0, nSrcWidth = (*itSrc)->bmWidth, nSrcHeight = min( cy, (*itSrc)->bmHeight), nCurrentSrcWidth = nSrcWidth;
	LPRGBA8 lpSrcBits = (LPRGBA8)(*itSrc)->bmBits;
	for ( int x = 0; x < cx; ++x ) {

		if ( x == nCurrentSrcWidth ) { 
			// Переходим на следующий источник
			++itSrc;
			lpSrcBits = (LPRGBA8)(*itSrc)->bmBits;
			nCurrentSrcWidth += (*itSrc)->bmWidth;
			nSrcWidth = (*itSrc)->bmWidth;
			nSrcHeight = min( cy, (*itSrc)->bmHeight);
			nSrcOffset = 0;
		};

		// Копируем из источника
		for ( int y = 0; y < nSrcHeight; ++y )
			memcpy(&lpDestBits[ x + y * cx ], &lpSrcBits[ nSrcOffset + y * nSrcWidth ], sizeof(RGBA8) );

		// увеличиваем смещение источника
		++nSrcOffset;
	}
	
	return hBitmap;
}

BOOL GetBitmapInfo( HBITMAP hBMP, LPBITMAPINFO pbi ) {
	BITMAP bmp;
	if( !GetObject( hBMP, sizeof(BITMAP), (LPVOID)&bmp ) )
		return FALSE;

	pbi->bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
	pbi->bmiHeader.biWidth = bmp.bmWidth;
	pbi->bmiHeader.biHeight = bmp.bmHeight;
	pbi->bmiHeader.biPlanes = 1;
	pbi->bmiHeader.biBitCount = 32; //bmp.bmBitsPixel;
	pbi->bmiHeader.biCompression = BI_RGB;
	pbi->bmiHeader.biSizeImage = pbi->bmiHeader.biWidth * pbi->bmiHeader.biHeight * 4;

	return TRUE;
}

BOOL LoadBitmap32( LPTSTR pszFile, LPBITMAP bmp ) {
	
	BITMAPFILEHEADER hdr;   
	BITMAPINFOHEADER hdrBmp;   

	DWORD dwReaded;
	HANDLE hFile = CreateFile( pszFile,  GENERIC_READ, (DWORD) 0, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL ); 
    if ( INVALID_HANDLE_VALUE == hFile ) 
        return FALSE;

	// reading a file header
	if (!ReadFile( hFile, (LPVOID) &hdr, sizeof(BITMAPFILEHEADER), &dwReaded,  NULL) || sizeof(BITMAPFILEHEADER) != dwReaded ) {
		CloseHandle( hFile );
		return FALSE;
	}

	// reading a bitmap header
	if (!ReadFile( hFile, (LPVOID) &hdrBmp, sizeof(BITMAPINFOHEADER), &dwReaded,  NULL) || sizeof(BITMAPINFOHEADER) != dwReaded ) {
		CloseHandle( hFile );
		return FALSE;
	}

	// reading a bitmap data
	int nSize = sizeof(RGBA8) *  hdrBmp.biWidth *  hdrBmp.biHeight;
	bmp->bmBits = malloc( nSize );
	if (!ReadFile( hFile, bmp->bmBits, nSize, &dwReaded,  NULL) || nSize != dwReaded ) {
		free( bmp->bmBits );
		CloseHandle( hFile );
		return FALSE;
	}

	bmp->bmType = hdr.bfType;
	bmp->bmBitsPixel = hdrBmp.biBitCount;
	bmp->bmHeight = hdrBmp.biHeight;
	bmp->bmPlanes = hdrBmp.biPlanes;
	bmp->bmWidth = hdrBmp.biWidth;

	CloseHandle( hFile );

	return TRUE;
}

int SaveBitmap( HBITMAP hBMP, LPTSTR pszFile ) { 
    
	HANDLE hf;                 
    BITMAPFILEHEADER hdr;      
    PBITMAPINFOHEADER pbih;    
	LPBYTE lpBits;
    DWORD dwTotal;
    DWORD cb;
    BYTE *hp;
    DWORD dwTmp; 

	BITMAPINFO bi;
	memset( &bi, 0, sizeof(BITMAPINFO) );
	if ( !GetBitmapInfo( hBMP, &bi ) )
		return 1;

    pbih = (PBITMAPINFOHEADER) &bi; 
    lpBits = (LPBYTE) GlobalAlloc(GMEM_FIXED, pbih->biSizeImage);

    if (!lpBits) 
         return -1;

    // Retrieve the color table (RGBQUAD array) and the bits 
    // (array of palette indices) from the DIB. 
 	HDC hdc = GetDC(NULL);
	BOOL bResult = GetDIBits(hdc, hBMP, 0, (WORD) pbih->biHeight, lpBits, &bi,  DIB_RGB_COLORS);
	ReleaseDC(NULL, hdc);
	if ( !bResult )
        return -2;

    // Create the .BMP file. 
    hf = CreateFile(pszFile,  GENERIC_READ | GENERIC_WRITE, (DWORD) 0, NULL, CREATE_ALWAYS, 
                   FILE_ATTRIBUTE_NORMAL, (HANDLE) NULL ); 
    if (hf == INVALID_HANDLE_VALUE) 
        return -3;

    hdr.bfType = 0x4d42;        // 0x42 = "B" 0x4d = "M" 
    // Compute the size of the entire file. 
    hdr.bfSize = (DWORD) (sizeof(BITMAPFILEHEADER) + 
                 pbih->biSize + pbih->biClrUsed 
                 * sizeof(RGBQUAD) + pbih->biSizeImage); 
    hdr.bfReserved1 = 0; 
    hdr.bfReserved2 = 0; 

    // Compute the offset to the array of color indices. 
    hdr.bfOffBits = (DWORD) sizeof(BITMAPFILEHEADER) + 
                    pbih->biSize + pbih->biClrUsed 
                    * sizeof (RGBQUAD); 

    // Copy the BITMAPFILEHEADER into the .BMP file. 
    if (!WriteFile(hf, (LPVOID) &hdr, sizeof(BITMAPFILEHEADER), 
        (LPDWORD) &dwTmp,  NULL)) 
       return -4;

    // Copy the BITMAPINFOHEADER and RGBQUAD array into the file. 
    if (!WriteFile(hf, (LPVOID) pbih, sizeof(BITMAPINFOHEADER) 
                  + pbih->biClrUsed * sizeof (RGBQUAD), 
                  (LPDWORD) &dwTmp, ( NULL)) )
        return -5;

    // Copy the array of color indices into the .BMP file. 
    dwTotal = cb = pbih->biSizeImage; 
    hp = lpBits; 
    if (!WriteFile(hf, (LPSTR) hp, (int) cb, (LPDWORD) &dwTmp,NULL)) 
        return -6;

    // Close the .BMP file. 
     if (!CloseHandle(hf)) 
        return -7;

    // Free memory. 
    GlobalFree((HGLOBAL)lpBits);
	
	return 0;
}


BOOL LoadIcon32(  LPCTSTR lpszFilename, LPICONSDATA lpid ) {

	HANDLE hFile = CreateFile( lpszFilename, GENERIC_READ, 0, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL );
    if( INVALID_HANDLE_VALUE == hFile )
        return FALSE;
	
	// read header
	DWORD dwBytesRead;
	WORD wHeader[3];
	if( ! ReadFile( hFile, &wHeader, 6, &dwBytesRead, NULL ) || dwBytesRead != 6) {
		CloseHandle( hFile );
        return FALSE;
	}
	// check format
	if( wHeader[0]!= 0 || wHeader[1] != 1 ) {
		CloseHandle( hFile );
		return FALSE;
	}

	lpid->nCount = wHeader[2]; // image counts
	
	_LPICONDIRENTRY lpIDE = new _ICONDIRENTRY[ lpid->nCount ];
	DWORD dwRead = lpid->nCount * sizeof( _ICONDIRENTRY ), dwReaded;
  if( ! ReadFile( hFile, lpIDE, dwRead , &dwReaded, NULL ) || dwReaded != dwRead ) {
		CloseHandle( hFile );
		delete[] lpIDE;
		return FALSE;
  }
	
	lpid->ppbi = new LPBITMAPINFO[ lpid->nCount ];
  // Loop through and read in each image
  for( UINT i = 0; i < lpid->nCount; i++ ) {
		
		// allocate space
		LPBITMAPINFO lpbi = (LPBITMAPINFO) new BYTE [ sizeof(BITMAPINFO) + lpIDE[i].dwBytesInRes ]; 
		lpid->ppbi[i] = lpbi;
		
    if( SetFilePointer( hFile, lpIDE[i].dwImageOffset, NULL, FILE_BEGIN ) == 0xFFFFFFFF ) {
			CloseHandle( hFile );
			delete[] lpIDE;
			delete lpbi;
			int n = i;
			while(n--)
				delete lpid->ppbi[n];
			delete[] lpid->ppbi;
			return FALSE;
		}
		int nCount = lpIDE[i].dwBytesInRes; //40 + lpIDE[i].bWidth * lpIDE[i].bHeight * 4; // Сама картинка
		// Read main bitmap
    if( !ReadFile( hFile, (LPVOID)lpbi, nCount, &dwReaded, NULL ) || nCount!=  dwReaded )
    {
			CloseHandle( hFile );
			delete[] lpIDE;
			int n = i;
			while(n--)
				delete lpid->ppbi[n];
			delete[] lpid->ppbi;
            return FALSE;
    }
		lpbi->bmiHeader.biHeight /= 2;
		lpbi->bmiHeader.biSizeImage = lpbi->bmiHeader.biWidth * lpbi->bmiHeader.biHeight * 4;

	}
	delete[] lpIDE;
    CloseHandle( hFile );
	return TRUE;
}

CBitmap32::CBitmap32(void)
{
	m_pBits = NULL;
}

CBitmap32::~CBitmap32(void)
{
	if(m_pBits)
		free( m_pBits );
}

BOOL CBitmap32::DeleteObject(){
	if(m_pBits)
		free( m_pBits );
	return CBitmap::DeleteObject();
}

BOOL CBitmap32::LoadBitmap( UINT nIDResource, HINSTANCE hInst ){
	return LoadBitmap( MAKEINTRESOURCE(nIDResource), hInst );
}


BOOL CBitmap32::LoadBitmap( LPCTSTR lpszResourceName, HINSTANCE hInst ){

	if ( GetSafeHandle() )
		DeleteObject();

	WORD wColors = 0;
	HBITMAP hBmp = LoadBitmap32( lpszResourceName, NULL, NULL, &wColors, hInst );
	if ( NULL == hBmp )
		return FALSE;
	
	return Attach( hBmp, &wColors );
}

BOOL CBitmap32::Attach( HGDIOBJ hObject, LPWORD lpwColors  ) {

	if ( GetSafeHandle() )
		DeleteObject();

	BITMAP bmp;
	if ( !::GetObject( (HBITMAP)hObject, sizeof(BITMAP), &bmp) ) 
		return FALSE;

	m_nWidth = bmp.bmWidth;
	m_nHeight = bmp.bmHeight;
	m_pBits = (LPRGBA8) malloc( sizeof(RGBA8) *  m_nWidth *  m_nHeight );

	if ( !GetBitmapRGBA( (HBITMAP)hObject, m_pBits, lpwColors, TRUE ) )
		return FALSE;
	
	// Устанавливаем
	return CBitmap::Attach( hObject );
}

BOOL CBitmap32::AddBitmap( HBITMAP hBitmap, LPWORD lpwColors  ) {
	
	int cy = m_nHeight;
	
	HBITMAP hOldBitmap = (HBITMAP)Detach();
	
	if ( hOldBitmap ) {
		WORD wColors = 32;
		return Attach( CombineBitmaps( cy, hOldBitmap, hBitmap, NULL ), &wColors ); 
	}
	
	return Attach( hBitmap, lpwColors );
}

BOOL CBitmap32::AddBitmap( UINT nIDResource, HINSTANCE hInst  ) {
	return AddBitmap( MAKEINTRESOURCE( nIDResource ), hInst );
}

BOOL CBitmap32::AddBitmap( LPCTSTR lpszResourceName, HINSTANCE hInst  ) {

	WORD wColors;
	HBITMAP hBitmap = LoadBitmap32( lpszResourceName, NULL, NULL, &wColors, hInst );
	if ( hBitmap )
		return AddBitmap( hBitmap, &wColors );
	return FALSE;
}

void CBitmap32::Draw(CDC *pDC, CRect* prcDest, CRect* prcSrc, int nMode ) {
	
	if( !m_pBits ) return;

	int nWidth = min( m_nWidth, prcSrc->Width() );
	int nHeight = min( m_nHeight, prcSrc->Height() );
//	int nWidth = min( m_nWidth, prcDest->Width() );
//	int nHeight = min( m_nHeight, prcDest->Height() );

	// Создаем временную поверхность - копию
	BITMAPINFO bmi;
	memset( &bmi, 0, sizeof(BITMAPINFO) );

	bmi.bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
	bmi.bmiHeader.biWidth = nWidth;
	bmi.bmiHeader.biHeight = nHeight;
	bmi.bmiHeader.biPlanes = 1;
	bmi.bmiHeader.biBitCount = 32;
	bmi.bmiHeader.biCompression = BI_RGB;
	bmi.bmiHeader.biSizeImage = nWidth*nHeight*4;

	LPRGBA8 pBackBits;
	HBITMAP hBack = CreateDIBSection( NULL, &bmi, DIB_RGB_COLORS, (void **)&pBackBits, NULL, NULL ); 
	CDC MemDC;
	MemDC.CreateCompatibleDC(pDC);
	MemDC.SelectObject(hBack);

	// Пока копируем порцию под картинку
	MemDC.BitBlt(0, 0, nWidth, nHeight, pDC, prcDest->left, prcDest->top, SRCCOPY);
	//MemDC.StretchBlt(0, 0, nWidth, nHeight, pDC, prcDest->left, prcDest->top, prcDest->right - prcDest->left, prcDest->bottom - prcDest->top, SRCCOPY);

	//MemDC.FillSolidRect( prcDest,  GetSysColor( COLOR_BTNFACE ) );

	
	// Переносим изображение
	RGBA8 *pBit, *pBackBit;
	int nCount = prcSrc->Width(), nStart = prcSrc->left, nStartY = m_nHeight-prcSrc->top-nHeight;
	if (nStart < m_nWidth ){
		for (int x = 0; x < nCount; ++x)
			for (int y = 0; y < nHeight; ++y){
//				pBit = &m_pBits[nStart+x+y*m_nWidth];
					pBit = &m_pBits[ nStart + x +( nStartY + y ) * m_nWidth];
					pBackBit = &pBackBits[x+y*nWidth];
				
				switch( nMode )  {
					case 0: // Normal image
						pBackBit->red =  min(255, ( pBackBit->red * (255-pBit->alpha)) /255 + (pBit->red * pBit->alpha) /255 );
						pBackBit->green =  min(255, ( pBackBit->green * (255-pBit->alpha)) /255 + (pBit->green * pBit->alpha) /255 );
						pBackBit->blue =  min(255, ( pBackBit->blue * (255-pBit->alpha)) /255 + (pBit->blue * pBit->alpha) /255 );
						break;
					case 1: // Disabled image
						{
							BYTE PixelApha = static_cast<BYTE>(pBit->alpha * 0.8);

							BYTE bGrayScale = ( ( pBit->red + pBit->green + pBit->blue ) * PixelApha ) / ( 3 * 255 );

							pBackBit->red =  min(255, pBackBit->red * (255 - PixelApha ) /255 +  bGrayScale );
							pBackBit->green =  min(255, pBackBit->green * (255 - PixelApha ) /255 + bGrayScale );
							pBackBit->blue =  min(255, pBackBit->blue * (255 - PixelApha ) /255 + bGrayScale );
						}
						break;
					case 2: // Shadowed image
						{
							BYTE PixelApha = static_cast<BYTE>( pBit->alpha * 0.8 );
							BYTE bGrayScale = static_cast<BYTE>( PixelApha * 0.5 ); // 128 * PixelApha / 255;

							pBackBit->red =  min(255, pBackBit->red * (255 - PixelApha ) /255 +  bGrayScale );
							pBackBit->green =  min(255, pBackBit->green * (255 - PixelApha ) /255 + bGrayScale );
							pBackBit->blue =  min(255, pBackBit->blue * (255 - PixelApha ) /255 + bGrayScale );
						}
						break;
				}

				pBackBit->alpha = 255;
			}
	}

	// Переносим обработанное изображение (только часть под иконку)
	pDC->BitBlt( prcDest->left, prcDest->top, nWidth, nHeight, &MemDC, 0, 0, SRCCOPY);
	//pDC->StretchBlt( prcDest->left, prcDest->top, prcDest->right - prcDest->left, prcDest->bottom - prcDest->top, &MemDC, 0, 0, nWidth, nHeight, SRCCOPY);

	::DeleteObject( hBack );
}

void CBitmap32::StretchDraw(CDC *pDC, CRect* prcDest, CRect* prcSrc, int nMode ) {
	
	if( !m_pBits ) return;

	int nWidth = min( m_nWidth, prcSrc->Width() );
	int nHeight = min( m_nHeight, prcSrc->Height() );
//	int nWidth = min( m_nWidth, prcDest->Width() );
//	int nHeight = min( m_nHeight, prcDest->Height() );

	// Создаем временную поверхность - копию
	BITMAPINFO bmi;
	memset( &bmi, 0, sizeof(BITMAPINFO) );

	bmi.bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
	bmi.bmiHeader.biWidth = nWidth;
	bmi.bmiHeader.biHeight = nHeight;
	bmi.bmiHeader.biPlanes = 1;
	bmi.bmiHeader.biBitCount = 32;
	bmi.bmiHeader.biCompression = BI_RGB;
	bmi.bmiHeader.biSizeImage = nWidth*nHeight*4;

	LPRGBA8 pBackBits;
	HBITMAP hBack = CreateDIBSection( NULL, &bmi, DIB_RGB_COLORS, (void **)&pBackBits, NULL, NULL ); 
	CDC MemDC;
	MemDC.CreateCompatibleDC(pDC);
	MemDC.SelectObject(hBack);

	// Пока копируем порцию под картинку
	//MemDC.BitBlt(0, 0, nWidth, nHeight, pDC, prcDest->left, prcDest->top, SRCCOPY);
	MemDC.StretchBlt(0, 0, nWidth, nHeight, pDC, prcDest->left, prcDest->top, prcDest->right - prcDest->left, prcDest->bottom - prcDest->top, SRCCOPY);

	//MemDC.FillSolidRect( prcDest,  GetSysColor( COLOR_BTNFACE ) );

	
	// Переносим изображение
	RGBA8 *pBit, *pBackBit;
	int nCount = prcSrc->Width(), nStart = prcSrc->left, nStartY = m_nHeight-prcSrc->top-nHeight;
	if (nStart < m_nWidth ){
		for (int x = 0; x < nCount; ++x)
			for (int y = 0; y < nHeight; ++y){
//				pBit = &m_pBits[nStart+x+y*m_nWidth];
					pBit = &m_pBits[ nStart + x +( nStartY + y ) * m_nWidth];
					pBackBit = &pBackBits[x+y*nWidth];
				
				switch( nMode )  {
					case 0: // Normal image
						pBackBit->red =  min(255, ( pBackBit->red * (255-pBit->alpha)) /255 + (pBit->red * pBit->alpha) /255 );
						pBackBit->green =  min(255, ( pBackBit->green * (255-pBit->alpha)) /255 + (pBit->green * pBit->alpha) /255 );
						pBackBit->blue =  min(255, ( pBackBit->blue * (255-pBit->alpha)) /255 + (pBit->blue * pBit->alpha) /255 );
						break;
					case 1: // Disabled image
						{
							BYTE PixelApha = static_cast<BYTE>(pBit->alpha * 0.8);

							BYTE bGrayScale = ( ( pBit->red + pBit->green + pBit->blue ) * PixelApha ) / ( 3 * 255 );

							pBackBit->red =  min(255, pBackBit->red * (255 - PixelApha ) /255 +  bGrayScale );
							pBackBit->green =  min(255, pBackBit->green * (255 - PixelApha ) /255 + bGrayScale );
							pBackBit->blue =  min(255, pBackBit->blue * (255 - PixelApha ) /255 + bGrayScale );
						}
						break;
					case 2: // Shadowed image
						{
							BYTE PixelApha = static_cast<BYTE>( pBit->alpha * 0.8 );
							BYTE bGrayScale = static_cast<BYTE>( PixelApha * 0.5 ); // 128 * PixelApha / 255;

							pBackBit->red =  min(255, pBackBit->red * (255 - PixelApha ) /255 +  bGrayScale );
							pBackBit->green =  min(255, pBackBit->green * (255 - PixelApha ) /255 + bGrayScale );
							pBackBit->blue =  min(255, pBackBit->blue * (255 - PixelApha ) /255 + bGrayScale );
						}
						break;
				}

				pBackBit->alpha = 255;
			}
	}

	// Переносим обработанное изображение (только часть под иконку)
	//pDC->BitBlt( prcDest->left, prcDest->top, nWidth, nHeight, &MemDC, 0, 0, SRCCOPY);
	pDC->StretchBlt( prcDest->left, prcDest->top, prcDest->right - prcDest->left, prcDest->bottom - prcDest->top, &MemDC, 0, 0, nWidth, nHeight, SRCCOPY);

	::DeleteObject( hBack );
}

/*
void CBitmap32::StretchDrawEx( CDC * pDC, const CRect* prcDest, const CRect* prcSrc, int nMode )
{
	  // pRectFrom должен полностью лежать внутри картинки 
//    OI_ASSERT( !pRectFrom || ( pRectFrom->left >= 0 || pRectFrom->top >= 0 || pRectFrom->right < getW() || pRectFrom->bottom < getH() ) );

    int wDest = prcDest->Width();
		int hDest = prcDest->Height();

		int wSrc = prcSrc->Width();
		int hSrc = prcSrc->Height();

    RECT rFrom = *prcSrc;
    RECT rTo = *prcDest;

    const int& srcPxW = wDest;
    const int& srcPxH = hDest;
    const int& destPxW = wSrc;
    const int& destPxH = hSrc;

    //////////////////////////////////////////////////////////////////////////
		// Создаем временную поверхность

		CDC SurfaceDC;
		LPRGBA8 pSurfaceBits;
		HBITMAP hSurface;
		GetSurface( pDC, prcSrc, &SurfaceDC, &pSurfaceBits, &hSurface );

    //////////////////////////////////////////////////////////////////////////

    // (+) Делаем зарубки по границам пикселов

    int* marksX = new int [wDest + wSrc];
    int* marksY = new int [hDest + hSrc];

    // ... по оси Х
    {
        int to = wSrc*wDest;
        int i = destPxW, j = srcPxW;
        int idx = 0, idxDest = 0;
        int prevValue = 0;

        for( ; i <= to; i += destPxW, idx++, idxDest++ )
        {
            for( ; j < i; j += srcPxW )
            {
                if( j == prevValue ) continue;
                marksX[idx++] = j - prevValue;
                prevValue = j;
            }

            marksX[idx] = i - prevValue;
            prevValue = i;
        }
    }

	// ... по оси Y
	{
        int to = hSrc*hDest;
        int i = destPxH, j = srcPxH;
        int idx = 0, idxDest = 0;
        int prevValue = 0;

        for( ; i <= to; i += destPxH, idx++, idxDest++ )
        {
            for( ; j < i; j += srcPxH )
            {
	            if( j == prevValue ) continue;
	            marksY[idx++] = j - prevValue;
	            prevValue = j;
            }

            marksY[idx] = i - prevValue;
            prevValue = i;
        }
    }

    // (-) Делаем зарубки

    // Перебираем все нужные точки изображения-приемника

    int idxX = 0;
    int idxY = 0;
    int idxXBase = 0;
    int idxYBase = 0;

    int destSquare = destPxH*destPxW;

    int avR = 0, avG = 0, avB = 0, avA = 0;

    int offsetTo = rTo.top * wDest + rTo.left;
    int strideYTo = wDest - (rTo.right - rTo.left);

    int start_sX = 0;
    int start_sY = 0;

	for( int sY = start_sY, y = 0; y < wSrc; sY += destPxH, y++, offsetTo += strideYTo )
	{
        for( int sX = start_sX, x = rTo.left; x < rTo.right; sX += destPxW, x++, offsetTo++ )
        {
            avR = 0, avG = 0, avB = 0, avA = 0;

            idxY = idxYBase;

            int fromX;
            int fromY;

            int currH = 0;
            do
            {
                idxX = idxXBase;

                fromY = (sY + currH) / srcPxH + rFrom.top;

                int currW = 0;					
                do
                {
                    fromX = (sX + currW) / srcPxW + rFrom.left;

                    int sqare = marksX[idxX] * marksY[idxY];

                    const Color& color = pixel( fromX, fromY );

                    avR += sqare * color.R();
                    avG += sqare * color.G();
                    avB += sqare * color.B();
                    avA += sqare * color.A();

                    currW += marksX[idxX++];
	            }
	            while( currW < destPxW );

	            currH += marksY[idxY++];
            }
            while(currH < destPxH);

            idxXBase = idxX;

            avR /= destSquare;
            avG /= destSquare;
            avB /= destSquare;
            avA /= destSquare;

            OI_ASSERT( avR < 256 );
            OI_ASSERT( avG < 256 );
            OI_ASSERT( avB < 256 );
            OI_ASSERT( avA < 256 );

            Color& out = pixel( offsetTo );

            out.R() = avR;
            out.G() = avG;
            out.B() = avB;
            out.A() = avA;
        }

        idxYBase = idxY;
        idxXBase = 0;
    }

    delete [] marksX;
    delete [] marksY;
}

*/

CSurface::CSurface( ) {
	m_pLockedDC = NULL;
	m_hBitmap = NULL;
}

CDC * CSurface::Lock( CDC * pDC, CRect *prcBounds ) {

	m_pLockedDC = pDC;
	m_nXOffset = prcBounds->left;
	m_nYOffset = prcBounds->top;

	BITMAPINFO bmi;
	memset( &bmi, 0, sizeof(BITMAPINFO) );

	m_nWidth = prcBounds->Width();
	m_nHeight = prcBounds->Height();

	bmi.bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
	bmi.bmiHeader.biWidth = m_nWidth;
	bmi.bmiHeader.biHeight = m_nHeight;
	bmi.bmiHeader.biPlanes = 1;
	bmi.bmiHeader.biBitCount = 32;
	bmi.bmiHeader.biCompression = BI_RGB;
	bmi.bmiHeader.biSizeImage = m_nWidth * m_nHeight * 4;

	m_hBitmap = CreateDIBSection( NULL, &bmi, DIB_RGB_COLORS, (void **)&m_pBits, NULL, NULL ); 
	
	m_SurfaceDC.CreateCompatibleDC( pDC );
	m_hOldBitmap = (HBITMAP)m_SurfaceDC.SelectObject( m_hBitmap );
	m_SurfaceDC.BitBlt(0, 0, m_nWidth, m_nHeight, pDC, prcBounds->left, prcBounds->top, SRCCOPY);

	m_rcBounds.SetRect( 0, 0, m_nWidth, m_nHeight );

	return &m_SurfaceDC;
}

void CSurface::Release() {
	ASSERT( m_pLockedDC != NULL );

	m_pLockedDC->BitBlt( m_nXOffset, m_nYOffset, m_nWidth, m_nHeight, &m_SurfaceDC, 0, 0, SRCCOPY);
	m_SurfaceDC.SelectObject( m_hOldBitmap );
	::DeleteObject( m_hBitmap );
	m_hBitmap = NULL;
	m_SurfaceDC.DeleteDC();
}
