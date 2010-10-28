#include "stdafx.h"
#include "Cursorer.h"

#include "..//Drawer.h"


CCursorer::CCursorer()
{
	m_cursor_structure.isRound = TRUE;
	m_cursor_structure.size = 4;
	m_cursor_structure.insideColor = 15;
	m_cursor_structure.outsideColor = 0;
    m_hCursor = NULL;
    SetCursorStructure(m_cursor_structure);
}

CCursorer::~CCursorer()
{
  if(m_hCursor)
    ::DestroyIcon(m_hCursor);
}

CURSOR_STRUCTURE*   CCursorer::GetCursorStructure() 
{
	return &m_cursor_structure;
}

bool  CCursorer::SetCursorStructure(CURSOR_STRUCTURE& cS)
{
	if (cS.size >MAX_CURSOR_SIZE+1 || 
		cS.insideColor > 239 ||
		cS.outsideColor > 239)
	{
		ASSERT(0);
		return false;
	}

	m_cursor_structure = cS;

	HBITMAP hSourceBitmap = GetCursorBitmap();
	//Now create the real one
	if(m_hCursor!=NULL)
	{
		::DestroyIcon(m_hCursor);
	}
	m_hCursor = CreateCursorFromBitmap(hSourceBitmap,RGB(0,0,0),m_cursor_structure.size,
		m_cursor_structure.size);
	return (m_hCursor!=NULL);
}


static void FillSolidRectMy( HDC hDC, int x, int y, int cx, int cy, COLORREF clr )
{
	SetBkColor( hDC, clr );
	RECT rect;
	SetRect( &rect, x, y, x + cx, y + cy );
	ExtTextOut( hDC, 0, 0, ETO_OPAQUE, &rect, NULL, 0, NULL );
}

HBITMAP CCursorer::GetCursorBitmap()
{
  COLORREF OutsideColor = RGB((int)(Drawer::GetColorByIndex(m_cursor_structure.outsideColor)[0]*255.0f),
		(int)(Drawer::GetColorByIndex(m_cursor_structure.outsideColor)[1]*255.0f),
		(int)(Drawer::GetColorByIndex(m_cursor_structure.outsideColor)[2]*255.0f));
  COLORREF InsideColor = RGB((int)(Drawer::GetColorByIndex(m_cursor_structure.insideColor)[0]*255.0f),
	  (int)(Drawer::GetColorByIndex(m_cursor_structure.insideColor)[1]*255.0f),
	  (int)(Drawer::GetColorByIndex(m_cursor_structure.insideColor)[2]*255.0f));

  if (m_cursor_structure.insideColor==0)
	  InsideColor = RGB(1,1,1);
  if (m_cursor_structure.outsideColor==0)
	  OutsideColor = RGB(1,1,1);

  HDC hMainDC = ::GetDC(NULL);
  HDC hTempDC = ::CreateCompatibleDC(hMainDC);
  HBITMAP hTempBitmap = ::CreateCompatibleBitmap(hMainDC,32,32);
  HBITMAP hOldBitmap  = (HBITMAP)::SelectObject(hTempDC,hTempBitmap);
  FillSolidRectMy(hTempDC,0,0,32,32,RGB(0,0,0));
  HPEN    hPen    = ::CreatePen(PS_SOLID,1,OutsideColor);
  HPEN    hPen1   = ::CreatePen(PS_SOLID,1,InsideColor);

  HPEN    hOldPen   = (HPEN)::SelectObject(hTempDC,hPen);
  HBRUSH  hBrush    = ::CreateSolidBrush(RGB(0,0,0));
  HBRUSH  hOldBrush = (HBRUSH)::SelectObject(hTempDC,hBrush);

  if (m_cursor_structure.isRound)
	::Ellipse(hTempDC,0,0,2*m_cursor_structure.size+1, 2*m_cursor_structure.size+1);
  else
	::Rectangle(hTempDC,0,0,2*m_cursor_structure.size+1,2*m_cursor_structure.size+1);

  ::SelectObject(hTempDC,hPen1);

  if (m_cursor_structure.isRound)
	::Ellipse(hTempDC,1,1,2*m_cursor_structure.size,2*m_cursor_structure.size);
  else
	::Rectangle(hTempDC,1,1,2*m_cursor_structure.size,2*m_cursor_structure.size);

  ::SetPixel(hTempDC,m_cursor_structure.size,m_cursor_structure.size, OutsideColor);

  ::SelectObject(hTempDC,hOldBrush);
  ::SelectObject(hTempDC,hOldPen);
  ::SelectObject(hTempDC,hOldBitmap);

  ::DeleteObject(hBrush);
  ::DeleteObject(hPen);
  ::DeleteDC(hTempDC);
  ::ReleaseDC(NULL,hMainDC);
  return hTempBitmap;

}

HCURSOR CCursorer::CreateCursorFromBitmap(HBITMAP hSourceBitmap,
                      COLORREF clrTransparent,
                      DWORD   xHotspot,
                      DWORD   yHotspot)
{
  HCURSOR hRetCursor = NULL;

  do
  {
    if(NULL == hSourceBitmap)
    {
      break;
    }

    //Create the AND and XOR masks for the bitmap
    HBITMAP hAndMask = NULL;
    HBITMAP hXorMask = NULL;
    GetMaskBitmaps(hSourceBitmap,clrTransparent,hAndMask,hXorMask);
    if(NULL == hAndMask || NULL == hXorMask)
    {
      break;
    }

    //Create the cursor using the masks and the hotspot values provided
    ICONINFO iconinfo = {0};
    iconinfo.fIcon    = FALSE;
    iconinfo.xHotspot = xHotspot;
    iconinfo.yHotspot = yHotspot;
    iconinfo.hbmMask  = hAndMask;
    iconinfo.hbmColor = hSourceBitmap;

    hRetCursor = ::CreateIconIndirect(&iconinfo);

    if(NULL != hAndMask)
      ::DeleteObject(hAndMask);
    if(NULL != hXorMask)
      ::DeleteObject(hXorMask);

  }
  while(0);

  return hRetCursor;

}

void  CCursorer::GetMaskBitmaps(HBITMAP hSourceBitmap,
                  COLORREF clrTransparent,
                  HBITMAP &hAndMaskBitmap,
                  HBITMAP &hXorMaskBitmap)
{
  HDC hDC         = ::GetDC(NULL);
  HDC hMainDC       = ::CreateCompatibleDC(hDC);
  HDC hAndMaskDC      = ::CreateCompatibleDC(hDC);
  HDC hXorMaskDC      = ::CreateCompatibleDC(hDC);

  //Get the dimensions of the source bitmap
  BITMAP bm;
  ::GetObject(hSourceBitmap,sizeof(BITMAP),&bm);


  hAndMaskBitmap  = ::CreateCompatibleBitmap(hAndMaskDC,bm.bmWidth,bm.bmHeight);
  hXorMaskBitmap  = ::CreateCompatibleBitmap(hXorMaskDC,bm.bmWidth,bm.bmHeight);

  //Select the bitmaps to DC
  HBITMAP hOldMainBitmap = (HBITMAP)::SelectObject(hMainDC,hSourceBitmap);
  HBITMAP hOldAndMaskBitmap = (HBITMAP)::SelectObject(hAndMaskDC,hAndMaskBitmap);
  HBITMAP hOldXorMaskBitmap = (HBITMAP)::SelectObject(hXorMaskDC,hXorMaskBitmap);

  //Scan each pixel of the souce bitmap and create the masks
  COLORREF MainBitPixel;
  for(int x=0;x<bm.bmWidth;++x)
  {
    for(int y=0;y<bm.bmHeight;++y)
    {
      MainBitPixel = ::GetPixel(hMainDC,x,y);
      if(MainBitPixel == clrTransparent)
      {
        ::SetPixel(hAndMaskDC,x,y,RGB(255,255,255));
        ::SetPixel(hXorMaskDC,x,y,RGB(0,0,0));
      }
      else
      {
        ::SetPixel(hAndMaskDC,x,y,RGB(0,0,0));
        ::SetPixel(hXorMaskDC,x,y,MainBitPixel);
      }

    }
  }

  ::SelectObject(hMainDC,hOldMainBitmap);
  ::SelectObject(hAndMaskDC,hOldAndMaskBitmap);
  ::SelectObject(hXorMaskDC,hOldXorMaskBitmap);

  ::DeleteDC(hXorMaskDC);
  ::DeleteDC(hAndMaskDC);
  ::DeleteDC(hMainDC);

  ::ReleaseDC(NULL,hDC);

}
