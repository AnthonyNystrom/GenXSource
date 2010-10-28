#pragma once
#include "afxwin.h"

#include <vector>

using std::vector;

typedef struct {
	BYTE red;
	BYTE green;
	BYTE blue;
	BYTE alpha;
} RGBA8, *LPRGBA8;

typedef struct {
	UINT nCount;
	LPBITMAPINFO* ppbi;
} ICONSDATA, *LPICONSDATA;

typedef struct
{
	BYTE	bWidth;               // Width of the image
	BYTE	bHeight;              // Height of the image (times 2)
	BYTE	bColorCount;          // Number of colors in image (0 if >=8bpp)
	BYTE	bReserved;            // Reserved
	WORD	wPlanes;              // Color Planes
	WORD	wBitCount;            // Bits per pixel
	DWORD	dwBytesInRes;         // how many bytes in this resource?
	DWORD	dwImageOffset;        // where in the file is this image
} _ICONDIRENTRY, *_LPICONDIRENTRY;

HBITMAP CombineResources( int cy, ... );
HBITMAP CombineBitmaps( int cy, ... );
HBITMAP Combine( int cx, int cy, vector<LPBITMAP> *lstSrc );

BOOL LoadBitmap32( LPTSTR pszFile, LPBITMAP bmp );
int SaveBitmap( HBITMAP hBMP, LPTSTR pszFile );
int GetBitmapInfo( HBITMAP hBMP, LPBITMAPINFO pbi );

HBITMAP LoadBitmap32 ( LPCTSTR lpszResourceName, LPLONG lpnWidth = NULL, LPLONG lpnHeight = NULL, LPWORD lpnBitCount = NULL, HINSTANCE hInst = NULL);
BOOL LoadIcon32(  LPCTSTR lpszFilename, LPICONSDATA lpid );

class CSurface
{
	CDC * m_pLockedDC;
	CDC m_SurfaceDC;
	HBITMAP m_hBitmap;
	HBITMAP m_hOldBitmap;
	
	int m_nXOffset;
	int m_nYOffset;
	int m_nWidth;
	int m_nHeight;
	CRect m_rcBounds;
public:
	CSurface( );

	CDC * Lock( CDC * pDC, CRect* prcBounds );
	void Release();

	void GetSizes( int *pnWidth, int * pnHeight ) { *pnWidth = m_nWidth; *pnHeight = m_nHeight; }
	CRect * GetBounds() { return &m_rcBounds; }

	LPRGBA8 m_pBits;
};

class CBitmap32 :
	public CBitmap
{
	LPRGBA8 m_pBits;
	int m_nWidth;
	int m_nHeight;
public:
	CBitmap32(void);
	~CBitmap32(void);
	
	int GetWidth() { return m_nWidth; }
	int GetHeight() { return m_nHeight; }
	void SetHeight( int nHeight ) { m_nHeight = nHeight; }

	BOOL Attach( HGDIOBJ hObject, LPWORD lpwColors );
	BOOL DeleteObject();

	BOOL LoadBitmap( UINT nIDResource, HINSTANCE hInst = NULL );
	BOOL LoadBitmap( LPCTSTR lpszResourceName, HINSTANCE hInst = NULL );

	BOOL AddBitmap( HBITMAP hBitmap, LPWORD lpwColors );
	BOOL AddBitmap( UINT nIDResource, HINSTANCE hInst = NULL );
	BOOL AddBitmap( LPCTSTR lpszResourceName, HINSTANCE hInst = NULL );

	void GetSurface( CDC * pDC, const CRect *pRect, CDC *pSurfaceDC, LPRGBA8 * ppSurfaceBits, HBITMAP * phSurface );
	void Draw(CDC *pDC, CRect* prcDest, CRect* prcSrc, int nMode = 0);
	void StretchDraw(CDC *pDC, CRect* prcDest, CRect* prcSrc, int nMode = 0);
	void StretchDrawEx( CDC * pDC, const CRect* prcDest, const CRect* prcSrc, int nMode = 0 );
};
