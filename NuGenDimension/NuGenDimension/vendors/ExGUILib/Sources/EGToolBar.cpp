#include "stdafx.h"
#include "EGMenu.h"
#include "EGToolBar.h"
#include "EGDockingPane.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#define CX_GRIPPER  3
#define CY_GRIPPER  3
#define CX_BORDER_GRIPPER 2
#define CY_BORDER_GRIPPER 2

#ifndef BTNS_DROPDOWN
#define BTNS_DROPDOWN   TBSTYLE_DROPDOWN    // 0x0008
#endif


// CEGToolBar

IMPLEMENT_DYNAMIC(CEGToolBar, CToolBar)
CEGToolBar::CEGToolBar()
: m_ActMenuIndex(-1),
  m_pCustomizeMenu(NULL),
  m_DoCheck(0)
{
}

CEGToolBar::~CEGToolBar()
{
}

BEGIN_MESSAGE_MAP(CEGToolBar, CToolBar)
  ON_WM_NCPAINT()
  ON_WM_ERASEBKGND()
  ON_NOTIFY_REFLECT(NM_CUSTOMDRAW, OnNMCustomdraw)
  ON_WM_CREATE()
  ON_WM_PAINT()
  ON_MESSAGE(WM_IDLEUPDATECMDUI, OnIdleUpdateCmdUI)
END_MESSAGE_MAP()



// CEGToolBar message handlers
void CEGToolBar::DrawGripper(CDC* pDC, const CRect& rect)
{
  // only draw the gripper if not floating and gripper is specified
  if ((m_dwStyle & (CBRS_GRIPPER|CBRS_FLOATING)) == CBRS_GRIPPER)
  {
    // draw the gripper in the border
    if (m_dwStyle & CBRS_ORIENT_HORZ)
    {
      switch (CEGMenu::GetMenuDrawMode())
      {
      case CEGMenu::STYLE_XP:
      case CEGMenu::STYLE_XP_NOBORDER:
        {
          COLORREF col = GetSysColor(COLOR_BTNSHADOW); //DarkenColorXP(CEGMenu::GetMenuBarColorXP());
          CPen pen(PS_SOLID,0,col);
          CPen* pOld = pDC->SelectObject(&pen);
          for (int n=rect.top+m_cyTopBorder+2*CY_BORDER_GRIPPER;n<rect.Height()-m_cyTopBorder-m_cyBottomBorder-4*CY_BORDER_GRIPPER;n+=2)
          {
            pDC->MoveTo(rect.left+CX_BORDER_GRIPPER+2,n);
            pDC->LineTo(rect.left+CX_BORDER_GRIPPER+CX_GRIPPER+2,n);
          }
          pDC->SelectObject(pOld);
        }
        break;

      case CEGMenu::STYLE_ICY:
      case CEGMenu::STYLE_ICY_NOBORDER:
        {
          COLORREF color = DarkenColor(100,CEGMenu::GetMenuColor());
          for (int n=rect.top+m_cyTopBorder+2*CY_BORDER_GRIPPER;n<rect.Height()-m_cyTopBorder-m_cyBottomBorder-4*CY_BORDER_GRIPPER;n+=4)
          {
            //pDC->FillSolidRect(rect.left+CX_BORDER_GRIPPER+2,n  +1,2,2,color);
            pDC->FillSolidRect(rect.left+CX_BORDER_GRIPPER+2+m_cxLeftBorder,n  +1,2,2,color);
          }
           // make round corners
          color = GetSysColor(COLOR_3DLIGHT);//CEGMenu::GetMenuBarColor();
          CRect temp(rect);
          temp.InflateRect(-m_cxLeftBorder,-2,-m_cxRightBorder-1,-m_cxRightBorder);
          PaintCorner(pDC,temp,color);
       }
        break;

      case CEGMenu::STYLE_XP_2003_NOBORDER:
      case CEGMenu::STYLE_XP_2003:
      case CEGMenu::STYLE_COLORFUL_NOBORDER:
      case CEGMenu::STYLE_COLORFUL:
        {
          COLORREF color = GetSysColor(COLOR_BTNSHADOW); //DarkenColor(10,GetSysColor(COLOR_ACTIVECAPTION));

          for (int n=rect.top+m_cyTopBorder+2*CY_BORDER_GRIPPER;n<rect.Height()-m_cyTopBorder-m_cyBottomBorder-4*CY_BORDER_GRIPPER;n+=4)
          {
            pDC->FillSolidRect(rect.left+CX_BORDER_GRIPPER+3+m_cxLeftBorder,n+1+1,2,2,RGB(255,255,255));
            pDC->FillSolidRect(rect.left+CX_BORDER_GRIPPER+2+m_cxLeftBorder,n  +1,2,2,color);
          }
          // make round corners
          color = CEGMenu::GetMenuBarColor2003();
          CRect temp(rect);
          temp.InflateRect(-m_cxLeftBorder,-2,-m_cxRightBorder-1,-m_cxRightBorder);
          PaintCorner(pDC,temp,color);
        }
        break;

      default:
        CToolBar::DrawGripper(pDC,rect);
        break;
      }
    }
    else
    {
      switch (CEGMenu::GetMenuDrawMode())
      {
      case CEGMenu::STYLE_XP:
      case CEGMenu::STYLE_XP_NOBORDER:
        {
          COLORREF col = GetSysColor(COLOR_BTNSHADOW); //DarkenColorXP(CEGMenu::GetMenuBarColorXP());
          CPen pen(PS_SOLID,0,col);
          CPen* pOld = pDC->SelectObject(&pen);
          for (int n=rect.top+m_cxLeftBorder+2*CX_BORDER_GRIPPER;n<rect.Width()-m_cxLeftBorder-m_cxRightBorder-2*CX_BORDER_GRIPPER;n+=2)
          {
            pDC->MoveTo(n,rect.top+CY_BORDER_GRIPPER+2);
            pDC->LineTo(n,rect.top+CY_BORDER_GRIPPER+CY_GRIPPER+2);
          }
          pDC->SelectObject(pOld);
        }
        break;

      case CEGMenu::STYLE_ICY:
      case CEGMenu::STYLE_ICY_NOBORDER:
        {
          COLORREF color = DarkenColor(100,CEGMenu::GetMenuColor());

          for (int n=rect.top+m_cxLeftBorder+2*CX_BORDER_GRIPPER;n<rect.Width()-m_cxRightBorder-2*CX_BORDER_GRIPPER;n+=4)
          {
            //pDC->FillSolidRect(n,  rect.top+CY_BORDER_GRIPPER+2,2,2,color);
            pDC->FillSolidRect(n,  rect.top+CY_BORDER_GRIPPER+2+m_cxLeftBorder,2,2,color);
          }
          // make the corners round
          color = GetSysColor(COLOR_3DLIGHT);//CEGMenu::GetMenuBarColor();
          CRect temp(rect);
          temp.InflateRect(-m_cxLeftBorder+1,-3,-m_cxRightBorder,-m_cxRightBorder-1);
          PaintCorner(pDC,temp,color);
        }
        break;

      case CEGMenu::STYLE_XP_2003_NOBORDER:
      case CEGMenu::STYLE_XP_2003:
      case CEGMenu::STYLE_COLORFUL_NOBORDER:
      case CEGMenu::STYLE_COLORFUL:
        {
          COLORREF color = GetSysColor(COLOR_BTNSHADOW); //DarkenColor(10,GetSysColor(COLOR_ACTIVECAPTION));
          for (int n=rect.top+m_cxLeftBorder+2*CX_BORDER_GRIPPER;n<rect.Width()-m_cxRightBorder-2*CX_BORDER_GRIPPER;n+=4)
          {
            pDC->FillSolidRect(n+1,rect.top+CY_BORDER_GRIPPER+3+m_cxLeftBorder,2,2,RGB(255,255,255));
            pDC->FillSolidRect(n,  rect.top+CY_BORDER_GRIPPER+2+m_cxLeftBorder,2,2,color);
          }

          // make the corners round
          color = CEGMenu::GetMenuBarColor2003();
          CRect temp(rect);
          temp.InflateRect(-m_cxLeftBorder+1,-3,-m_cxRightBorder,-m_cxRightBorder-1);
          PaintCorner(pDC,temp,color);
        }
        break;

      default:
        CToolBar::DrawGripper(pDC,rect);
        break;
      }
    }
  }
}

void CEGToolBar::OnNcPaint()
{
  EraseNonClient();
}

void CEGToolBar::EraseNonClient()
{
  // get window DC that is clipped to the non-client area
  CWindowDC dc(this);
  CRect rectClient;
  GetClientRect(rectClient);
  CRect rectWindow;
  GetWindowRect(rectWindow);
  ScreenToClient(rectWindow);

  rectClient.OffsetRect(-rectWindow.left, -rectWindow.top);
  dc.ExcludeClipRect(rectClient);

  CPoint Offset = rectWindow.TopLeft();

  // draw borders in non-client area
  rectWindow.OffsetRect(-rectWindow.left, -rectWindow.top);
  DrawBorders(&dc, rectWindow);

  // erase parts not drawn
  dc.IntersectClipRect(rectWindow);

  CPoint oldOffset;
  // correcting offset of the border
  if(!(m_dwStyle & CBRS_FLOATING))
  {
    oldOffset = dc.SetWindowOrg(Offset);
  }
  else
  {
    oldOffset = dc.GetWindowOrg();
  }
  SendMessage(WM_ERASEBKGND, (WPARAM)dc.m_hDC);

  // reset offset
  dc.SetWindowOrg(oldOffset);

  // draw gripper in non-client area
  DrawGripper(&dc, rectWindow);
}

void CEGToolBar::DrawBorders(CDC* pDC, CRect& rect)
{
  switch (CEGMenu::GetMenuDrawMode())
  {
  case CEGMenu::STYLE_XP:
  case CEGMenu::STYLE_XP_NOBORDER:
    break;

  case CEGMenu::STYLE_XP_2003_NOBORDER:
  case CEGMenu::STYLE_XP_2003:
  case CEGMenu::STYLE_COLORFUL_NOBORDER:
  case CEGMenu::STYLE_COLORFUL:
    break;

  case CEGMenu::STYLE_ICY:
  case CEGMenu::STYLE_ICY_NOBORDER:
   break;

  default:
    CToolBar::DrawBorders(pDC,rect);
    break;
  }
}

BOOL CEGToolBar::OnEraseBkgnd(CDC* pDC)
{
  BOOL bRet = true;//CToolBar::OnEraseBkgnd(pDC);

  CRect rect;
  if(m_dwStyle & CBRS_FLOATING)
  {
    GetClientRect(rect);
  }
  else
  {
    GetWindowRect(rect);
    ScreenToClient(rect);
  }

  switch (CEGMenu::GetMenuDrawMode())
  {
  case CEGMenu::STYLE_XP:
  case CEGMenu::STYLE_XP_NOBORDER:
    {
      COLORREF menuColor = CEGMenu::GetMenuBarColorXP();
      pDC->FillSolidRect(rect,MixedColor(menuColor,GetSysColor(COLOR_WINDOW)));
    }
    break;

  case CEGMenu::STYLE_XP_2003_NOBORDER:
  case CEGMenu::STYLE_XP_2003:
  case CEGMenu::STYLE_COLORFUL_NOBORDER:
  case CEGMenu::STYLE_COLORFUL:
    if(NumScreenColors()<256)
    {
      CRect rcToolBar = rect;
      if(m_dwStyle & CBRS_ORIENT_HORZ)
      {
        rcToolBar.left += m_cxLeftBorder;
        rcToolBar.right -= m_cxRightBorder;

        if(!(m_dwStyle & CBRS_FLOATING))
        {
          rcToolBar.bottom = rect.bottom - 2;
          rcToolBar.top = rect.top + 2;
        }
      }
      else
      {
        rcToolBar.top += m_cxLeftBorder;
        rcToolBar.bottom -= m_cxRightBorder;
        rcToolBar.left += m_cxLeftBorder;
        rcToolBar.right -= m_cxRightBorder;
      }

      PaintToolBarBackGnd(pDC);
      COLORREF menuColor = GetXpHighlightColor();
      pDC->FillSolidRect(rcToolBar,DarkenColor(30,menuColor));
    }
    else
    {
      COLORREF clrUpperColor, clrMediumColor, clrBottomColor, clrDarkLine;
      if(IsMenuThemeActive())
      {
        COLORREF menuColor = CEGMenu::GetMenuBarColor2003();

        // corrections from Andreas Sch�rer
        switch(menuColor)
        {
        case RGB(163,194,245)://blau
          {
            clrUpperColor = RGB(221,236,254);
            clrMediumColor = RGB(196, 219,249);
            clrBottomColor = RGB(129,169,226);
            clrDarkLine = RGB(59,97,156);
            break;
          }
        case RGB(215,215,229)://silber
          {
            clrUpperColor = RGB(243,244,250);
            clrMediumColor = RGB(225,226,236);
            clrBottomColor = RGB(153,151,181);
            clrDarkLine = RGB(124,124,148);
            break;
          }
        case RGB(218,218,170)://olivgr�n
          {
            clrUpperColor = RGB(244,247,222);
            clrMediumColor = RGB(209,222,172);
            clrBottomColor = RGB(183,198,145);
            clrDarkLine = RGB(96,128,88);
            break;
          }
        default:
          {
            clrUpperColor = LightenColor(140,menuColor);
            clrMediumColor = LightenColor(115,menuColor);
            clrBottomColor = DarkenColor(40,menuColor);
            clrDarkLine = DarkenColor(110,menuColor);
            break;
          }
        }
      }
      else
      {
        COLORREF menuColor, menuColor2;
        CEGMenu::GetMenuBarColor2003(menuColor, menuColor2, FALSE);
        clrUpperColor = menuColor2;
        clrBottomColor = menuColor;
        clrMediumColor = GetAlphaBlendColor(clrBottomColor, clrUpperColor,100);
        clrDarkLine = CLR_NONE;
      }
      PaintToolBarBackGnd(pDC);

      //clrDarkLine = DarkenColor(80,menuColor);

      if(m_dwStyle & CBRS_ORIENT_HORZ)
      {
        CRect rcToolBar = rect;
        rcToolBar.left += m_cxLeftBorder;
        rcToolBar.right -= m_cxRightBorder;
        rcToolBar.bottom /= 2;
        rcToolBar.top = rect.top + 2;
        DrawGradient(pDC,rcToolBar,clrUpperColor,clrMediumColor,false);

        rcToolBar.top = rcToolBar.bottom;
        if(!(m_dwStyle & CBRS_FLOATING))
        {
          rcToolBar.bottom = rect.bottom - 2;
        }
        else
        {
          rcToolBar.bottom = rect.bottom;
        }
        DrawGradient(pDC,rcToolBar,clrMediumColor,clrBottomColor,false);

        if(!(m_dwStyle & CBRS_FLOATING))
        {
          if(clrDarkLine!=CLR_NONE)
          {
            //GetClientRect(rect);
            CRect line(rect.left+m_cxLeftBorder,rect.bottom-3,rect.right-m_cxRightBorder,rect.bottom-2);
            //dark line on bottom toolbar
            pDC->FillSolidRect(line,clrDarkLine);
          }
        }
        else
        {
          CRect itemRect;
          CRect lastItemRect(0,0,0,0);
          int nCountBtn = (int)SendMessage (TB_BUTTONCOUNT, 0, 0);
          TBBUTTON tbButton;
          for(int i=0;i<nCountBtn;i++)
          {
            if (!SendMessage(TB_GETBUTTON, i, (LPARAM)&tbButton) ||
              (tbButton.fsState & TBSTATE_HIDDEN))
            { // coninue by error or hidden buttons
              continue;
            }
            GetItemRect(i,itemRect);
            if(itemRect.top==lastItemRect.top &&
               itemRect.bottom==lastItemRect.bottom)
            {
              continue;
            }
            if((tbButton.fsStyle == TBSTYLE_SEP) && (tbButton.fsState & TBSTATE_WRAP) )
            {
              CRect rcToolBar(rect.left,lastItemRect.bottom,rect.right,itemRect.bottom);
              DrawGradient(pDC,rcToolBar,clrBottomColor,clrUpperColor,false);
            }
            else
            {
              CRect rcToolBar(rect.left,itemRect.top,rect.right,(itemRect.bottom+itemRect.top)/2);
              DrawGradient(pDC,rcToolBar,clrUpperColor,clrMediumColor,false);

              rcToolBar.top = rcToolBar.bottom;
              rcToolBar.bottom = itemRect.bottom;
              DrawGradient(pDC,rcToolBar,clrMediumColor,clrBottomColor,false);
            }
            lastItemRect = itemRect;
          }
        }
      }
      else
      {
        if(!(m_dwStyle & CBRS_FLOATING))
        {
          if(clrDarkLine!=CLR_NONE)
          {
            //GetClientRect(rect);
            CRect line(rect.right-m_cxRightBorder  ,rect.top+m_cxRightBorder,
              rect.right-m_cxRightBorder+1,rect.bottom-m_cxRightBorder);

            //dunkler strich am unteren ende der toolbar
            pDC->FillSolidRect(line,clrDarkLine);
          }
        }
        CRect rcToolBar = rect;
        rcToolBar.top += m_cxLeftBorder;
        rcToolBar.bottom -= m_cxRightBorder;
        rcToolBar.left += m_cxLeftBorder-1;
        rcToolBar.right /= 2;
        DrawGradient(pDC,rcToolBar,clrUpperColor,clrMediumColor,true);
        rcToolBar.left = rcToolBar.right;
        rcToolBar.right = rect.right-m_cxRightBorder;
        DrawGradient(pDC,rcToolBar,clrMediumColor,clrBottomColor,true);
      }
    }
    break;

  case CEGMenu::STYLE_ICY:
  case CEGMenu::STYLE_ICY_NOBORDER:
    if(NumScreenColors()<256)
    {
      CRect rcToolBar = rect;
      if(m_dwStyle & CBRS_ORIENT_HORZ)
      {
        rcToolBar.left += m_cxLeftBorder;
        rcToolBar.right -= m_cxRightBorder;

        if(!(m_dwStyle & CBRS_FLOATING))
        {
          rcToolBar.bottom = rect.bottom - 2;
          rcToolBar.top = rect.top + 2;
        }
      }
      else
      {
        rcToolBar.top += m_cxLeftBorder;
        rcToolBar.bottom -= m_cxRightBorder;
        rcToolBar.left += m_cxLeftBorder;
        rcToolBar.right -= m_cxRightBorder;
      }

      PaintToolBarBackGnd(pDC);
      COLORREF menuColor = CEGMenu::GetMenuBarColor();
      pDC->FillSolidRect(rcToolBar,DarkenColor(30,menuColor));
    }
    else
    {
      COLORREF menuColor = CEGMenu::GetMenuColor();
      COLORREF clrUpperColor = LightenColor(60,menuColor);
      COLORREF clrMediumColor = menuColor;
      COLORREF clrBottomColor = DarkenColor(60,menuColor);
      COLORREF clrDarkLine = DarkenColor(150,menuColor);

      PaintToolBarBackGnd(pDC);
      if(m_dwStyle & CBRS_ORIENT_HORZ)
      {
        CRect rcToolBar = rect;
        rcToolBar.left += m_cxLeftBorder;
        rcToolBar.right -= m_cxRightBorder;
        rcToolBar.bottom /= 2;
        rcToolBar.top = rect.top + 2;
        DrawGradient(pDC,rcToolBar,clrUpperColor,clrMediumColor,false);

        rcToolBar.top = rcToolBar.bottom;
         if(!(m_dwStyle & CBRS_FLOATING))
        {
          rcToolBar.bottom = rect.bottom - 2;
        }
        else
        {
          rcToolBar.bottom = rect.bottom;
        }
        DrawGradient(pDC,rcToolBar,clrMediumColor,clrBottomColor,false);

        if(!(m_dwStyle & CBRS_FLOATING))
        {
          CRect topLine(rect.left+m_cxLeftBorder,rect.top+m_cyTopBorder+1,
                        rect.right-m_cxRightBorder,rect.top+m_cyTopBorder+2);
          pDC->FillSolidRect(topLine,LightenColor(125,menuColor));

          CRect line(rect.left+m_cxLeftBorder,rect.bottom-3,rect.right-m_cxRightBorder,rect.bottom-2);
          //dark line on bottom toolbar
          pDC->FillSolidRect(line,clrDarkLine);
        }
        else
        {
          CRect itemRect;
          CRect lastItemRect(0,0,0,0);
          int nCountBtn = (int)SendMessage (TB_BUTTONCOUNT, 0, 0);
          TBBUTTON tbButton;
          for(int i=0;i<nCountBtn;i++)
          {
            if (!SendMessage(TB_GETBUTTON, i, (LPARAM)&tbButton) ||
              (tbButton.fsState & TBSTATE_HIDDEN))
            { // coninue by error or hidden buttons
              continue;
            }
            GetItemRect(i,itemRect);
            if(itemRect.top==lastItemRect.top &&
               itemRect.bottom==lastItemRect.bottom)
            {
              continue;
            }
            if((tbButton.fsStyle == TBSTYLE_SEP) && (tbButton.fsState & TBSTATE_WRAP) )
            {
              CRect rcToolBar(rect.left,lastItemRect.bottom,rect.right,itemRect.bottom);

              DrawGradient(pDC,rcToolBar,clrBottomColor,clrUpperColor,false);
            }
            else
            {
              CRect rcToolBar(rect.left,itemRect.top,rect.right,(itemRect.bottom+itemRect.top)/2);

              DrawGradient(pDC,rcToolBar,clrUpperColor,clrMediumColor,false);

              rcToolBar.top = rcToolBar.bottom;
              rcToolBar.bottom = itemRect.bottom;
              DrawGradient(pDC,rcToolBar,clrMediumColor,clrBottomColor,false);
            }
            lastItemRect = itemRect;
          }
        }
      }
      else
      {
        if(!(m_dwStyle & CBRS_FLOATING))
        {
          CRect topLine(rect.left+m_cxLeftBorder-1,rect.top+m_cxRightBorder,
                        rect.left+m_cxLeftBorder  ,rect.bottom-m_cxRightBorder);
          pDC->FillSolidRect(topLine,LightenColor(125,menuColor));

          CRect line(rect.right-m_cxRightBorder  ,rect.top+m_cxRightBorder,
                     rect.right-m_cxRightBorder+1,rect.bottom-m_cxRightBorder);

          //dunkler strich am unteren ende der toolbar
          pDC->FillSolidRect(line,clrDarkLine);
        }
        CRect rcToolBar = rect;
        rcToolBar.top += m_cxLeftBorder;
        rcToolBar.bottom -= m_cxRightBorder;
        rcToolBar.left += m_cxLeftBorder;
        rcToolBar.right /= 2;
        DrawGradient(pDC,rcToolBar,clrUpperColor,clrMediumColor,true);
        rcToolBar.left = rcToolBar.right;
        rcToolBar.right = rect.right-m_cxRightBorder;
        DrawGradient(pDC,rcToolBar,clrMediumColor,clrBottomColor,true);
      }
    }
    break;

  default:
	pDC->FillSolidRect(rect,GetSysColor(COLOR_BTNFACE));
    break;
  }
  return bRet;
}


// Jan-12-2005 - Mark P. Peterson - mpp@rhinosoft.com - http://www.RhinoSoft.com/
// added this function to support gloomed buttons on the toolbar, this function was taken from
// CEGMenu almost verbatim
/*
int CEGToolBar::AddGloomIcon(HICON hIcon, int nIndex )
{
  ICONINFO  iconInfo = {0};

  // get the icon information from the icon we're adding to the gloom image list
  if (! GetIconInfo(hIcon, &iconInfo))
  {
    return (-1);
  }

  // create a DC used to convert from an icon to a bitmap
  CDC myDC;
  myDC.CreateCompatibleDC(0);

  // create a bitmap from the icon
  CBitmap bmColor;
  bmColor.Attach(iconInfo.hbmColor);

  // create a bitmap mask
  CBitmap bmMask;
  bmMask.Attach(iconInfo.hbmMask);

  // select the icon into the DC as it is now
  CBitmap* pOldBitmap = myDC.SelectObject(&bmColor);

  // do the width
  for (int nX = 0; nX < m_sizeImage.cx; nX++)
  {
    // do the height
    for (int nY = 0; nY < m_sizeImage.cy; nY++)
    {
      // get the pixel as it is now
      COLORREF crPixel = myDC.GetPixel(nX, nY);

      // darken the pixel and set it
      myDC.SetPixel(nX, nY, DarkenColor(CEGMenu::GetGloomFactor(), crPixel));
    } // for
  } // for

  // reselect the previous bitmap
  myDC.SelectObject(pOldBitmap);

  // add it to the end?
  if (nIndex == -1)
  {
    return m_GloomImageList.Add(&bmColor, &bmMask);
  }

  // replace the icon, return -1 if there was an error, otherwise return the index
  return ((m_GloomImageList.Replace(nIndex, &bmColor, &bmMask)) ? nIndex: -1);
} // AddGloomIcon
*/

// Jan-12-2005 - Mark P. Peterson - mpp@rhinosoft.com - http://www.RhinoSoft.com/
// this function creates the gloom image list based on the regular image list.
// each image is read from the original image list, modified by AddGloomIcon() pixel-by-pixel
// then added to the gloom image list
/*
void CEGToolBar::BuildGloomImageList()
{
  int nCount = m_ImageList.GetImageCount();

  // if the gloom image list already exists, delete it
  if (m_GloomImageList.GetSafeHandle())
  {
    m_GloomImageList.DeleteImageList();
  }

  // create the gloom image list
  m_GloomImageList.Create(m_sizeImage.cx, m_sizeImage.cy, (ILC_COLORDDB | ILC_MASK), 0, 10);

  // do each of the images in the image list
  for (int nIndex = 0; nIndex < nCount; nIndex++)
  {
    // extract the icon (original image) from the image list
    HICON hIcon = m_ImageList.ExtractIcon(nIndex);

    // add the gloom icon to the gloom icon list
    AddGloomIcon(hIcon);

    // release the icon now that we're done with it
    DestroyIcon(hIcon);
  } // for

  // set the appropriate image list
  SendMessage(TB_SETIMAGELIST, 0, (LPARAM) ((CEGMenu::GetXpBlending()) ? m_GloomImageList.m_hImageList: m_ImageList.m_hImageList));
} // BuildGloomImageList

*/

BOOL CEGToolBar::LoadHiColor(HBITMAP hBMP, LPWORD pwColors)
{
	if ( NULL != hBMP )
		return m_ImageList.LoadBitmap( hBMP, pwColors );

	return FALSE;
}

BOOL CEGToolBar::LoadHiColor(LPCTSTR lpszResourceName, COLORREF /*transparentColor/*=CLR_DEFAULT*/)
{
	WORD wColors;
	HBITMAP hBitmap = LoadBitmap32( lpszResourceName, NULL, NULL, &wColors );
	return LoadHiColor( hBitmap, &wColors );
}

BOOL CEGToolBar::LoadToolBar(LPCTSTR lpszResourceName)
{
  BOOL bRet = CToolBar::LoadToolBar(lpszResourceName);

  LoadHiColor(lpszResourceName);

  return bRet;
}

BOOL CEGToolBar::LoadToolBar(UINT nIDResource)
{
  return LoadToolBar(MAKEINTRESOURCE(nIDResource));
}

void CEGToolBar::PaintOrangeState(CDC *pDC, CRect rc, bool bHot)
{
  CPen penBlue;
  //dunkelblauer rahmen
  penBlue.CreatePen(PS_INSIDEFRAME,1, RGB(4, 2, 132));
  CPen* pOldPen = pDC->SelectObject(&penBlue);

  pDC->Rectangle(rc);
  rc.DeflateRect(1,1);

  if(bHot)
  { //heller oranger rahmen
    DrawGradient(pDC,rc,RGB(250,239,219),RGB(255,212,151),false);
  }
  else
  { //dunkler oranger rahmen
    DrawGradient(pDC,rc,RGB(242,151,107),RGB(249,202,163),false);
  }
  pDC->SelectObject(pOldPen);
}
HBRUSH CreatePressedPatternBrush()
{
	HDC hdc = GetDC( 0 );
	HDC hMemDC = CreateCompatibleDC( hdc );
	HBITMAP hBmp = CreateCompatibleBitmap(hdc, 4, 4), hbmpOld;
	HBRUSH hPatBr = 0;
	if( hMemDC && hBmp ) {
		hbmpOld = (HBITMAP)SelectObject(hMemDC, hBmp);
		COLORREF clrHi = GetSysColor(COLOR_3DHILIGHT);
		HBRUSH hbrOld = (HBRUSH)SelectObject(hMemDC, GetSysColorBrush(COLOR_BTNFACE));
		PatBlt(hMemDC, 0, 0, 4, 4, PATCOPY);
		SelectObject(hMemDC, hbrOld);
		SetPixel(hMemDC,0,0,clrHi); SetPixel(hMemDC,2,0,clrHi); 
		SetPixel(hMemDC,1,1,clrHi); SetPixel(hMemDC,3,1,clrHi);
		SetPixel(hMemDC,0,2,clrHi); SetPixel(hMemDC,2,2,clrHi);
		SetPixel(hMemDC,1,3,clrHi); SetPixel(hMemDC,3,3,clrHi);
		SelectObject(hMemDC, hbrOld);
		SelectObject(hMemDC, hbmpOld);
		DeleteDC(hMemDC);
		hPatBr = CreatePatternBrush(hBmp);
		DeleteObject(hBmp);
	}
	ReleaseDC( 0, hdc );
	return hPatBr;
}

// here we draw only the button background with border
BOOL CEGToolBar::PaintHotButton(LPNMTBCUSTOMDRAW lpNMTBCustomDraw)
{
  LPNMCUSTOMDRAW lpNMCustomDraw = &lpNMTBCustomDraw->nmcd;
  CDC* pDC = CDC::FromHandle(lpNMCustomDraw->hdc);

  int nIndex = CommandToIndex((UINT)lpNMCustomDraw->dwItemSpec);
  UINT nID = 0;
  UINT nStyle = 0;
  int iImage = 0;
  GetButtonInfo(nIndex,nID,nStyle,iImage);

   int cx=0, cy=0;
  CImageList* pImgList = (lpNMCustomDraw->uItemState&CDIS_DISABLED)?&m_ImageListDisabled:&m_ImageList;
  ::ImageList_GetIconSize(pImgList->GetSafeHandle(),&cx,&cy);

  extern BOOL bHighContrast; // defined in newmenu.cpp

  UINT nDrawMode = CEGMenu::GetMenuDrawMode();
  if( !IsMenuThemeActive() &&
     ((nDrawMode==CEGMenu::STYLE_XP_2003_NOBORDER)||
      (nDrawMode==CEGMenu::STYLE_XP_2003)) )
  {
    nDrawMode = CEGMenu::STYLE_XP;
  }
  switch (nDrawMode)
  {
  case CEGMenu::STYLE_XP:
  case CEGMenu::STYLE_XP_NOBORDER:
    {
      if(lpNMCustomDraw->uItemState&CDIS_INDETERMINATE)
      {
        COLORREF colorBitmap = MixedColor(CEGMenu::GetMenuBarColor(),GetSysColor(COLOR_WINDOW));
        pDC->FillSolidRect(&(lpNMCustomDraw->rc),colorBitmap);

        //COLORREF colorBorder = GetSysColor(COLOR_HIGHLIGHT);
        COLORREF colorBorder = bHighContrast?GetSysColor(COLOR_BTNTEXT):DarkenColor(128,CEGMenu::GetMenuBarColor());
        pDC->Draw3dRect(&(lpNMCustomDraw->rc),colorBorder,colorBorder);
      }
      else
      if (lpNMCustomDraw->uItemState&CDIS_CHECKED ||
          lpNMCustomDraw->uItemState&CDIS_HOT)
      {
        COLORREF colorSel = GetXpHighlightColor();
        COLORREF colorBorder = GetSysColor(COLOR_HIGHLIGHT);
        pDC->FillSolidRect(&(lpNMCustomDraw->rc),colorSel);
        pDC->Draw3dRect(&(lpNMCustomDraw->rc),colorBorder,colorBorder);
        // we are a menu button
        if(BTNS_DROPDOWN&nStyle )
        {
          CRect rect(lpNMCustomDraw->rc);
          rect.left = lpNMCustomDraw->rc.right-13;
          pDC->Draw3dRect(rect,colorBorder,colorBorder);
        }
      }
    }
    break;

    case CEGMenu::STYLE_ICY:
    case CEGMenu::STYLE_ICY_NOBORDER:
      {
        COLORREF colorMid = GetSysColor(COLOR_HIGHLIGHT);
        COLORREF colorSel = LightenColor(60,colorMid);
        COLORREF colorBorder = DarkenColor(60,colorMid);
        CRect rect(lpNMCustomDraw->rc);
        BOOL bDrawBorder = FALSE;
        if(lpNMCustomDraw->uItemState&CDIS_INDETERMINATE)
        {
          if(NumScreenColors()<256)
          {
            pDC->FillSolidRect(rect,colorSel);
          }
          else
          {
            COLORREF menuColor = CEGMenu::GetMenuColor();
            COLORREF clrUpperColor = LightenColor(60,menuColor);
            COLORREF clrMediumColor = menuColor;
            COLORREF clrBottomColor = DarkenColor(60,menuColor);

            rect.bottom = (rect.bottom+rect.top)/2;
            DrawGradient(pDC,rect,clrUpperColor,clrMediumColor,false);
            rect.top = rect.bottom;
            rect.bottom = lpNMCustomDraw->rc.bottom;
            DrawGradient(pDC,rect,clrMediumColor,clrBottomColor,false);
          }
          pDC->Draw3dRect(&(lpNMCustomDraw->rc),colorBorder,colorBorder);
        }
        else if(lpNMCustomDraw->uItemState == CDIS_HOT)
        {
          DrawGradient(pDC,rect,colorSel,colorMid,false);
          pDC->Draw3dRect(&(lpNMCustomDraw->rc),colorBorder,colorBorder);
          bDrawBorder = TRUE;
        }
        else if (lpNMCustomDraw->uItemState == (CDIS_HOT | CDIS_SELECTED))
        {
          DrawGradient(pDC,rect,colorMid,colorBorder,false);
          pDC->Draw3dRect(&(lpNMCustomDraw->rc),colorBorder,colorBorder);
          bDrawBorder = TRUE;
        }
        else if (lpNMCustomDraw->uItemState&CDIS_CHECKED)
        {
          pDC->FillSolidRect(&(lpNMCustomDraw->rc),colorSel);
          pDC->Draw3dRect(&(lpNMCustomDraw->rc),colorBorder,colorBorder);
          bDrawBorder = TRUE;
        }
        // we are a menu button
        if(bDrawBorder && BTNS_DROPDOWN&nStyle)
        {
          CRect rect(lpNMCustomDraw->rc);
          rect.left = lpNMCustomDraw->rc.right-13;//lpNMCustomDraw->rc.left + 3 + 4 + cx;
          pDC->Draw3dRect(rect,colorBorder,colorBorder);
        }
      }
      break;

    case CEGMenu::STYLE_XP_2003_NOBORDER:
    case CEGMenu::STYLE_XP_2003:
    case CEGMenu::STYLE_COLORFUL_NOBORDER:
    case CEGMenu::STYLE_COLORFUL:
    {
      BOOL bDrawBorder = FALSE;
      if(lpNMCustomDraw->uItemState&CDIS_INDETERMINATE)
      {
        COLORREF colorMenu = CEGMenu::GetMenuBarColor2003();
        CRect rect(lpNMCustomDraw->rc);
        if(NumScreenColors() <= 256)
        {
          pDC->FillSolidRect(rect,colorMenu);
        }
        else
        {
          COLORREF colorBitmap = colorMenu;
          colorMenu = MixedColor(DarkenColor(10,GetSysColor(COLOR_WINDOW)),CEGMenu::GetMenuColor());

          DrawGradient(pDC,rect,colorMenu,colorBitmap,FALSE,TRUE);
        }

        //COLORREF colorBorder = GetSysColor(COLOR_HIGHLIGHT);
        COLORREF colorBorder = bHighContrast?GetSysColor(COLOR_BTNTEXT):DarkenColor(128,CEGMenu::GetMenuBarColor());
        pDC->Draw3dRect(rect,colorBorder,colorBorder);
      }
      else if(lpNMCustomDraw->uItemState == CDIS_HOT)
      {
        PaintOrangeState(pDC,lpNMCustomDraw->rc, true);
        bDrawBorder = TRUE;
      }
      else if (lpNMCustomDraw->uItemState == (CDIS_HOT | CDIS_SELECTED))
      {
        PaintOrangeState(pDC,lpNMCustomDraw->rc, false);
        bDrawBorder = TRUE;
      }
      else if (lpNMCustomDraw->uItemState&CDIS_CHECKED)
      {
        COLORREF colorSel = RGB(255,238,194);
        COLORREF colorBorder = RGB(4, 2, 132);//GetSysColor(COLOR_HIGHLIGHT);
        pDC->FillSolidRect(&(lpNMCustomDraw->rc),colorSel);
        pDC->Draw3dRect(&(lpNMCustomDraw->rc),colorBorder,colorBorder);
        bDrawBorder = TRUE;
      }
      // we are a menu button
      if(bDrawBorder && BTNS_DROPDOWN&nStyle)
      {
        COLORREF colorBorder = RGB(4, 2, 132);//GetSysColor(COLOR_HIGHLIGHT);
        CRect rect(lpNMCustomDraw->rc);
        rect.left = lpNMCustomDraw->rc.right-13;//lpNMCustomDraw->rc.left + 3 + 4 + cx;
        pDC->Draw3dRect(rect,colorBorder,colorBorder);
      }
    }
    break;

  default:
    {
      BOOL bDrawBorder = FALSE;
      if(lpNMCustomDraw->uItemState == CDIS_HOT )
      {
        pDC->Draw3dRect(&(lpNMCustomDraw->rc),GetSysColor(COLOR_HIGHLIGHTTEXT),GetSysColor(COLOR_BTNSHADOW));
        bDrawBorder = TRUE;
      }
      else if (lpNMCustomDraw->uItemState == (CDIS_HOT | CDIS_SELECTED)||
         lpNMCustomDraw->uItemState&CDIS_INDETERMINATE)
      {
        pDC->Draw3dRect(&(lpNMCustomDraw->rc),GetSysColor(COLOR_BTNSHADOW),GetSysColor(COLOR_HIGHLIGHTTEXT));
        bDrawBorder = TRUE;
      }
      else if (lpNMCustomDraw->uItemState&CDIS_CHECKED)
      {
        pDC->Draw3dRect(&(lpNMCustomDraw->rc),GetSysColor(COLOR_BTNSHADOW),GetSysColor(COLOR_HIGHLIGHTTEXT));
				if ( !(lpNMCustomDraw->uItemState & CDIS_HOT || lpNMCustomDraw->uItemState & CDIS_SELECTED ) ) {
					CRect rc = lpNMCustomDraw->rc;
					InflateRect( &rc, -2, -2 );
					CBrush br;
					br.Attach( CreatePressedPatternBrush() );
					pDC->FillRect( &rc, &br );
				}
        bDrawBorder = TRUE;
      }
      // we are a menu button
      if(bDrawBorder && BTNS_DROPDOWN&nStyle && !(lpNMCustomDraw->uItemState&CDIS_INDETERMINATE) )
      {
        CRect rect(lpNMCustomDraw->rc);
        rect.left = lpNMCustomDraw->rc.right-13;//lpNMCustomDraw->rc.left + 3 + 4 + cx;
        if(lpNMCustomDraw->uItemState == CDIS_HOT)
        {
          pDC->Draw3dRect(rect,GetSysColor(COLOR_HIGHLIGHTTEXT),GetSysColor(COLOR_BTNSHADOW));
        }
        else
        {
          pDC->Draw3dRect(rect,GetSysColor(COLOR_BTNSHADOW),GetSysColor(COLOR_HIGHLIGHTTEXT));
        }
      }
    }
    break;
  }
  if(BTNS_DROPDOWN&nStyle)
  {
    COLORREF colorDropDown;
    if(lpNMCustomDraw->uItemState&CDIS_DISABLED)
    {
      colorDropDown = GetSysColor(COLOR_GRAYTEXT);
    }
    else
    {
      colorDropDown = GetSysColor(COLOR_WINDOWTEXT);
    }
    CRect rect(lpNMCustomDraw->rc);
    rect.left = lpNMCustomDraw->rc.right-13;//lpNMCustomDraw->rc.left + 3 + 4 + cx;

    CPen pen(PS_SOLID,0,colorDropDown);
    CPen* pOldPen = pDC->SelectObject(&pen);

    CPoint cetner = rect.CenterPoint();
    pDC->MoveTo(cetner+CPoint(-2,-1));pDC->LineTo(cetner+CPoint(+3,-1));
    pDC->MoveTo(cetner+CPoint(-1,-0));pDC->LineTo(cetner+CPoint(+2,-0));
    pDC->MoveTo(cetner+CPoint(-0,+1));pDC->LineTo(cetner+CPoint(+1,+1));

    pDC->SelectObject(pOldPen);
  }

  return TRUE;
}

// here we draw the button image
void CEGToolBar::PaintTBButton(LPNMTBCUSTOMDRAW lpNMTBCustomDraw)
{
  LPNMCUSTOMDRAW pInfo = &lpNMTBCustomDraw->nmcd;
  CDC *pDC = CDC::FromHandle(pInfo->hdc);

  int nIndex = CommandToIndex((UINT)pInfo->dwItemSpec);
  UINT nID = 0;
  UINT nStyle = 0;
  int iImage = 0;
  GetButtonInfo(nIndex,nID,nStyle,iImage);

  int cx=0, cy=0;
  // Jan-12-2005 - Mark P. Peterson - mpp@rhinosoft.com - http://www.RhinoSoft.com/
  // added the last ternary checking whether or not the image is hot or gloom is not being used, if hot or not glooming, then
  // the original image list is used, otherwise the gloom image list is used

  CEGmageList* pImgList;
//  if ( m_ImageList.IsAlpha() ) {
	pImgList = &m_ImageList;
 // }else{
//	pImgList = ((pInfo->uItemState&CDIS_DISABLED) ? &m_ImageListDisabled:
//              (((pInfo->uItemState & CDIS_HOT) || (! CEGMenu::GetXpBlending())) ? &m_ImageList: &m_GloomImageList));
//  }
  ::ImageList_GetIconSize(pImgList->GetSafeHandle(),&cx,&cy);

  // always center image
  CPoint ptImage((pInfo->rc.left + pInfo->rc.right - cx) / 2 ,
                 (pInfo->rc.top + pInfo->rc.bottom - cy) / 2);

  if(BTNS_DROPDOWN&nStyle)
  {
    ptImage.x -= 7;
  }
  CString sText = GetButtonText(nIndex);
  if (!sText.IsEmpty())
  {
    // correcting of image
    DWORD dwStyle = GetStyle();
    if(dwStyle&TBSTYLE_LIST)
    {// text on the right side
      ptImage.x = pInfo->rc.left+3;
    }
    else
    {// text on the left side
      ptImage.y = pInfo->rc.top+4;
    }

    CFont* pOldFont = (CFont*)pDC->SelectStockObject(ANSI_VAR_FONT);
    int iOldMode = pDC->SetBkMode(TRANSPARENT);
    CRect rect(lpNMTBCustomDraw->rcText);
    if (pInfo->uItemState&CDIS_DISABLED)
    {
      pDC->SetTextColor(GetSysColor(COLOR_GRAYTEXT));
    }
    // Jan-19-2005 - Mark P. Peterson - mpp@rhinosoft.com - http://www.RhinoSoft.com/
    // added this conditional, because in icy style the menu text becomes hidden, since we're using the highlight color we really need to use the highlight text color too
    else if ((pInfo->uItemState&CDIS_HOT) && ((CEGMenu::GetMenuDrawMode() == CEGMenu::STYLE_ICY) || (CEGMenu::GetMenuDrawMode() == CEGMenu::STYLE_ICY_NOBORDER)))
    {
      pDC->SetTextColor(GetSysColor(COLOR_HIGHLIGHTTEXT));
    }
  else
    {
      pDC->SetTextColor(GetSysColor(COLOR_WINDOWTEXT));
    }
    pDC->DrawText(sText, rect, DT_CENTER);
    pDC->SetBkMode(iOldMode);
    pDC->SelectObject(pOldFont);
  }

  switch (CEGMenu::GetMenuDrawMode())
  {
  case CEGMenu::STYLE_XP:
  case CEGMenu::STYLE_XP_NOBORDER:
    if(pInfo->uItemState&CDIS_HOT && !(pInfo->uItemState&CDIS_SELECTED))
    {
      ptImage.x += 1; ptImage.y += 1;
	  
	  pImgList->DrawMono(pDC, iImage, ptImage );
      
	  ptImage.x -= 2; ptImage.y -= 2;
    }
    break;

  case CEGMenu::STYLE_XP_2003_NOBORDER:
  case CEGMenu::STYLE_XP_2003:
  case CEGMenu::STYLE_COLORFUL_NOBORDER:
  case CEGMenu::STYLE_COLORFUL:
    break;

  default:
    break;
  }

  if(pImgList->GetSafeHandle())
  {
	  pImgList->Draw(pDC, iImage, ptImage, ILD_TRANSPARENT, (pInfo->uItemState&CDIS_DISABLED) );
  }
}

void CEGToolBar::OnNMCustomdraw(NMHDR *pNMHDR, LRESULT *pResult)
{
  LPNMTBCUSTOMDRAW lpNMCustomDraw = (LPNMTBCUSTOMDRAW) pNMHDR;
  *pResult = CDRF_DODEFAULT;

  switch(lpNMCustomDraw->nmcd.dwDrawStage)
  {
  case CDDS_PREPAINT:
    *pResult = CDRF_NOTIFYITEMDRAW|CDRF_NOTIFYPOSTPAINT|CDRF_SKIPDEFAULT;
    break;

  case CDDS_POSTPAINT:
    *pResult = CDRF_NOTIFYSUBITEMDRAW|CDRF_SKIPDEFAULT;
    // Erase Seperators
    break;

  case CDDS_ITEMPOSTPAINT:
    *pResult = CDRF_NOTIFYSUBITEMDRAW|CDRF_SKIPDEFAULT;
    // Erase Seperators
    break;

  case CDDS_ITEMPREPAINT:
    if(PaintHotButton(lpNMCustomDraw))
    {
      *pResult = CDRF_DODEFAULT|TBCDRF_NOEDGES;//CDRF_SKIPDEFAULT;
    }
    PaintTBButton(lpNMCustomDraw);
    *pResult = CDRF_SKIPDEFAULT;
    break;

  default:
    break;
  }
}

int CEGToolBar::OnCreate(LPCREATESTRUCT lpCreateStruct)
{
  // For correct handling of toolbar highliting the TBSTYLE_FLAT style has to be set
  ASSERT(lpCreateStruct->style&TBSTYLE_FLAT);

  if (CToolBar::OnCreate(lpCreateStruct) == -1)
  {
    return -1;
  }
  //typedef HRESULT (WINAPI* FktSetWindowTheme)(HWND hwnd, LPCWSTR pszSubAppName, LPCWSTR pszSubIdList);
  //extern FktSetWindowTheme pSetWindowTheme;
  //if(pSetWindowTheme)
  //{ // try do disable thems, but it does not work!!!
  //  pSetWindowTheme(m_hWnd,L" ",L" ");
  //}

  switch (CEGMenu::GetMenuDrawMode())
  {
  case CEGMenu::STYLE_XP:
  case CEGMenu::STYLE_XP_NOBORDER:
    {
      COLORSCHEME scheme;
      scheme.dwSize = sizeof(COLORSCHEME);
      scheme.clrBtnHighlight = CLR_DEFAULT;//DarkenColorXP(CEGMenu::GetMenuBarColor());;
      scheme.clrBtnShadow= DarkenColorXP(CEGMenu::GetMenuBarColorXP());

      SendMessage(TB_SETCOLORSCHEME,0,(LPARAM)&scheme);
    }
    break;

  case CEGMenu::STYLE_XP_2003_NOBORDER:
  case CEGMenu::STYLE_XP_2003:
  case CEGMenu::STYLE_COLORFUL_NOBORDER:
  case CEGMenu::STYLE_COLORFUL:
    {
      SetBorders(3,1,3,1);

      COLORSCHEME scheme;
      scheme.dwSize = sizeof(COLORSCHEME);
      scheme.clrBtnHighlight = CLR_DEFAULT;//DarkenColorXP(CEGMenu::GetMenuBarColor());;
      scheme.clrBtnShadow= DarkenColorXP(CEGMenu::GetMenuBarColor2003());

      SendMessage(TB_SETCOLORSCHEME,0,(LPARAM)&scheme);
    }
    break;

  default:
    break;
  }
  return 0;
}

LRESULT CEGToolBar::DefWindowProc(UINT nMsg, WPARAM wParam, LPARAM lParam)
{
  LRESULT result = CToolBar::DefWindowProc ( nMsg, wParam, lParam);
  if(m_DoCheck && nMsg == TB_GETBUTTON && result)
  {
    TBBUTTON* pButton = (TBBUTTON*)lParam;
    if(pButton && (pButton->fsStyle&TBSTYLE_SEP) && pButton->idCommand!=0 )
    {
      if(m_DoCheck==1)
      {
        pButton->fsState |= TBSTATE_HIDDEN;
      }
      else
      {
        pButton->fsState &= ~TBSTATE_HIDDEN;
      }
    }
  }
  return result;
}


CSize CEGToolBar::CalcDynamicLayout (int nLength, DWORD dwMode)
{
  bool bHideControls = (dwMode & LM_VERTDOCK) == LM_VERTDOCK;
  m_DoCheck = bHideControls ? 1 : 2;
  CSize size = CToolBar::CalcDynamicLayout (nLength, dwMode);
  m_DoCheck = 0;

  if ( dwMode & LM_COMMIT )
  {
    int nCountBtn = (int)SendMessage(TB_BUTTONCOUNT, 0, 0);
    TBBUTTON tbButton;
    for(int i=0;i<nCountBtn;i++)
    {
      if ( !SendMessage(TB_GETBUTTON, i, (LPARAM)&tbButton) ||
        !((tbButton.fsStyle&TBSTYLE_SEP) && tbButton.idCommand!=0) )
      { // coninue by error or not controls
        continue;
      }
      CWnd* pWnd = GetDlgItem (tbButton.idCommand);
      if ( !pWnd->GetSafeHwnd() )
      {
        continue;
      }
      if ( bHideControls )
      {
        GetToolBarCtrl().HideButton (tbButton.idCommand, true);
        pWnd->ShowWindow (SW_HIDE);
      }
      else
      {
        GetToolBarCtrl().HideButton (tbButton.idCommand, false);
        // Update control position
        CRect rectTBItem;
        if(SendMessage(TB_GETITEMRECT, i, (LPARAM)&rectTBItem))
        {
          rectTBItem.InflateRect(-1,-1);
          CRect rectControl;
          pWnd->GetWindowRect(rectControl);
          UINT nFlags = SWP_NOACTIVATE|SWP_NOZORDER|SWP_SHOWWINDOW;
          if ( rectControl.Width() == rectTBItem.Width() )
          {
            nFlags |= SWP_NOSIZE;
          }
          // center the control in the heigt
          int nOffsety = (rectTBItem.Height()-rectControl.Height())/2;
          rectTBItem.OffsetRect(0,nOffsety);

          pWnd->SetWindowPos (NULL, rectTBItem.left, rectTBItem.top, rectTBItem.Width(), rectTBItem.Height(),nFlags);
        }
      }
    }
  }
  return size;
}

void CEGToolBar::OnBarStyleChange(DWORD dwOldStyle, DWORD dwNewStyle)
{
  switch (CEGMenu::GetMenuDrawMode())
  {
  case CEGMenu::STYLE_XP_2003_NOBORDER:
  case CEGMenu::STYLE_XP_2003:
  case CEGMenu::STYLE_COLORFUL_NOBORDER:
  case CEGMenu::STYLE_COLORFUL:

  case CEGMenu::STYLE_ICY:
  case CEGMenu::STYLE_ICY_NOBORDER:
    if(dwNewStyle& CBRS_FLOATING)
    {
      SetBorders(0,0,0,0);
    }
    else
    {
      SetBorders(3,1,3,1);
    }
    break;

  case CEGMenu::STYLE_XP:
  case CEGMenu::STYLE_XP_NOBORDER:
  default:
    break;
  }
  CToolBar::OnBarStyleChange(dwOldStyle, dwNewStyle);
}

void CEGToolBar::OnPaint()
{
  CPaintDC dc(this);
  CWnd* pParent = GetParent();

  CWnd* pFrameWnd = GetParentFrame();
  if (pFrameWnd == NULL)
  {
    pFrameWnd = m_pDockSite;
  }
  if(pFrameWnd == NULL)
  {
    pFrameWnd = AfxGetMainWnd();
  }
  while(pFrameWnd && pFrameWnd->GetParentFrame())
  {
    pFrameWnd = pFrameWnd->GetParentFrame();
  }
  BOOL bFocus = pFrameWnd?pFrameWnd->IsChild(GetFocus()):false;

  //CFrameWnd* pFrame = GetDockingFrame();
  //while(pFrame && pFrame->GetParentFrame())
  //{
  //  pFrame = pFrame->GetParentFrame();
  //}
  //BOOL bFocus = AfxGetMainWnd()->IsChild(GetFocus());

  NMTBCUSTOMDRAW myDraw = {0};
  myDraw.nmcd.hdr.code = NM_CUSTOMDRAW;
  myDraw.nmcd.hdr.hwndFrom = m_hWnd;
  myDraw.nmcd.hdr.idFrom = GetDlgCtrlID();
  CRect client;
  GetClientRect(client);
  myDraw.nmcd.rc = client;
  myDraw.nmcd.hdc = dc.m_hDC;

  COLORSCHEME colorscheme = {0};
  colorscheme.dwSize = sizeof(colorscheme);
  if(SendMessage(TB_GETCOLORSCHEME,0,(LPARAM)&colorscheme))
  {
    myDraw.clrBtnHighlight = colorscheme.clrBtnHighlight;
    myDraw.clrBtnFace = colorscheme.clrBtnShadow;
  }

  myDraw.nmcd.dwDrawStage = CDDS_PREPAINT;
  LRESULT result = pParent->SendMessage(WM_NOTIFY,myDraw.nmcd.hdr.idFrom,(LPARAM)&myDraw);

  myDraw.nmcd.dwDrawStage = CDDS_PREERASE;
  result = pParent->SendMessage(WM_NOTIFY,myDraw.nmcd.hdr.idFrom,(LPARAM)&myDraw);

  myDraw.nmcd.dwDrawStage = CDDS_POSTERASE;
  result = pParent->SendMessage(WM_NOTIFY,myDraw.nmcd.hdr.idFrom,(LPARAM)&myDraw);

  int nCountBtn = (int)SendMessage(TB_BUTTONCOUNT, 0, 0);
  //int nHotItem = (int)SendMessage(TB_GETHOTITEM,0,0);

  COLORREF clrUpperColor = GetSysColor(COLOR_3DLIGHT);
  COLORREF clrBottomColor = GetSysColor(COLOR_3DDKSHADOW);
  switch (CEGMenu::GetMenuDrawMode())
  {
  case CEGMenu::STYLE_XP:
  case CEGMenu::STYLE_XP_NOBORDER:
    {
      COLORREF menuColor = CEGMenu::GetMenuBarColorXP();
      clrUpperColor = clrBottomColor = DarkenColor(40,menuColor);
    }
    break;

  case CEGMenu::STYLE_XP_2003_NOBORDER:
  case CEGMenu::STYLE_XP_2003:
  case CEGMenu::STYLE_COLORFUL_NOBORDER:
  case CEGMenu::STYLE_COLORFUL:
  default :
    {
      COLORREF menuColor = CEGMenu::GetMenuBarColor2003();
      clrUpperColor = LightenColor(140,menuColor);
      clrBottomColor = DarkenColor(40,menuColor);
    }
    break;

  case CEGMenu::STYLE_ICY:
  case CEGMenu::STYLE_ICY_NOBORDER:
    clrBottomColor = clrUpperColor = LightenColor(150,clrBottomColor);
    break;
  }

  TBBUTTON tbButton;
  CPoint pt;
  GetCursorPos(&pt);
  if(m_hWnd==::WindowFromPoint(pt))
  {
    ScreenToClient(&pt);
  }
  else
  {
    pt = CPoint(-1,-1);
  }
  DWORD dwStyle = GetStyle();//TBSTYLE_LIST

  for(int i=0;i<nCountBtn;i++)
  {
    if (!SendMessage(TB_GETBUTTON, i, (LPARAM)&tbButton) ||
        (tbButton.fsState & TBSTATE_HIDDEN))
    { // coninue by error or hidden buttons
      continue;
    }

    GetItemRect(i,&myDraw.nmcd.rc);
    if (tbButton.fsStyle == TBSTYLE_SEP)
    {
      CRect rect(myDraw.nmcd.rc);
      if (!(tbButton.fsState & TBSTATE_WRAP) || ! IsFloating())
      {
        if(tbButton.idCommand && GetDlgItem(tbButton.idCommand))
        {
          // do not draw seperators under controls
          continue;
        }
        if (m_dwStyle & CBRS_ORIENT_HORZ)
        {
          rect.left = ((rect.left + rect.right)/2)-1;
          if(clrBottomColor==clrUpperColor)
          {
            rect.right = rect.left + 1;
          }
          else
          {
            rect.right = rect.left + 2;
          }
          rect.top += 3;
          rect.bottom -= 3;
          dc.Draw3dRect(rect,clrBottomColor,clrUpperColor);
        }
        else
        {
          CRect lastRect(0,0,0,0);
          if(i>0)
          {
            GetItemRect(i-1,lastRect);
          }

          rect.top = (rect.bottom + lastRect.bottom)/2;
          if(clrBottomColor==clrUpperColor)
          {
            rect.bottom = rect.top + 1;
          }
          else
          {
            rect.bottom = rect.top + 2;
          }
          rect.right = lastRect.right - 3;
          rect.left += 3;

          dc.Draw3dRect(rect,clrBottomColor,clrUpperColor);
        }
      }
      else if(tbButton.fsState & TBSTATE_WRAP)
      {
        CRect lastRect(0,0,0,0);
        if(i>0)
        {
          GetItemRect(i-1,lastRect);
        }

        rect.top = (rect.bottom + lastRect.bottom)/2-1;
        if(clrBottomColor==clrUpperColor)
        {
          rect.bottom = rect.top + 1;
        }
        else
        {
          rect.bottom = rect.top + 2;
        }
        rect.right = client.right-2;
        rect.left += 2+2;

        dc.Draw3dRect(rect,clrBottomColor,clrUpperColor);
      }
    }
    else
    {
      if(tbButton.idCommand)
      {
        myDraw.nmcd.uItemState = 0;
        myDraw.nmcd.uItemState |= (tbButton.fsState&TBSTATE_CHECKED)?CDIS_CHECKED:0;
        myDraw.nmcd.uItemState |= (tbButton.fsState&TBSTATE_ENABLED)?0:CDIS_DISABLED;
        myDraw.nmcd.uItemState |= (tbButton.fsState&TBSTATE_INDETERMINATE)?CDIS_INDETERMINATE:0;

        if(bFocus && PtInRect (&myDraw.nmcd.rc,pt))
        {
          if(tbButton.fsState&TBSTATE_PRESSED)
          {
            myDraw.nmcd.uItemState |= CDIS_HOT|CDIS_SELECTED;
          }
          else if (tbButton.fsState&TBSTATE_ENABLED)
          {
            myDraw.nmcd.uItemState |= CDIS_HOT;
          }
        }
        else if(tbButton.fsState&TBSTATE_PRESSED)
        {
          myDraw.nmcd.uItemState |= CDIS_HOT;
        }
        myDraw.rcText = CRect(0,0,0,0);
        if(tbButton.iString!=(-1))
        {
          int nSize = (int)SendMessage(TB_GETBUTTONTEXT,tbButton.idCommand,0);
          if(nSize!=(-1))
          {
            PTCHAR nBuffer = (PTCHAR)_alloca((nSize+2)*sizeof(TCHAR));
            *nBuffer = 0;
            SendMessage(TB_GETBUTTONTEXT,tbButton.idCommand,(LPARAM)nBuffer);
            SIZE size = {0,0};
            VERIFY(::GetTextExtentPoint32(dc.m_hDC,nBuffer,nSize,&size));
            if(dwStyle&TBSTYLE_LIST)
            {// text will be shown right
              int nBorderY = (myDraw.nmcd.rc.bottom-myDraw.nmcd.rc.top-size.cy)/2;
              myDraw.rcText.top = myDraw.nmcd.rc.top+nBorderY+2;
              myDraw.rcText.bottom = myDraw.rcText.top+size.cy;

              if(tbButton.fsStyle&BTNS_DROPDOWN)
              {
                myDraw.rcText.right = myDraw.nmcd.rc.right-13 - 4;
                myDraw.rcText.left  = myDraw.nmcd.rc.left + m_sizeImage.cx + 4;
              }
              else
              {
                myDraw.rcText.right = myDraw.nmcd.rc.right - 2;
                myDraw.rcText.left  = myDraw.nmcd.rc.left  + m_sizeImage.cx + 4;
              }
            }
            else
            {// text will be shown below
              int nBorderX = (myDraw.nmcd.rc.right-myDraw.nmcd.rc.left-size.cx)/2;
              if(tbButton.fsStyle&BTNS_DROPDOWN)
              {
                nBorderX -= 7;
              }
              myDraw.rcText.left = myDraw.nmcd.rc.left+nBorderX;
              myDraw.rcText.right = myDraw.rcText.left+size.cx;

              myDraw.rcText.bottom = myDraw.nmcd.rc.bottom-1;
              myDraw.rcText.top = myDraw.nmcd.rc.bottom-size.cy;
            }
          }
        }
        myDraw.nmcd.dwItemSpec  = tbButton.idCommand;
        myDraw.nmcd.lItemlParam = tbButton.dwData;

        myDraw.nmcd.dwDrawStage = CDDS_PREPAINT|CDDS_ITEM;
        result = pParent->SendMessage(WM_NOTIFY,myDraw.nmcd.hdr.idFrom,(LPARAM)&myDraw);

        myDraw.nmcd.dwDrawStage = CDDS_POSTPAINT|CDDS_ITEM;
        result = pParent->SendMessage(WM_NOTIFY,myDraw.nmcd.hdr.idFrom,(LPARAM)&myDraw);
      }
    }
  }

  myDraw.nmcd.dwDrawStage = CDDS_POSTPAINT;
  result = pParent->SendMessage(WM_NOTIFY,myDraw.nmcd.hdr.idFrom,(LPARAM)&myDraw);
}

void CEGToolBar::PaintCorner(CDC *pDC, LPCRECT pRect, COLORREF color)
{
  pDC->SetPixel(pRect->left+1,pRect->top  ,color);
  pDC->SetPixel(pRect->left+0,pRect->top  ,color);
  pDC->SetPixel(pRect->left+0,pRect->top+1,color);

  pDC->SetPixel(pRect->left+0,pRect->bottom  ,color);
  pDC->SetPixel(pRect->left+0,pRect->bottom-1,color);
  pDC->SetPixel(pRect->left+1,pRect->bottom  ,color);

  pDC->SetPixel(pRect->right-1,pRect->top  ,color);
  pDC->SetPixel(pRect->right  ,pRect->top  ,color);
  pDC->SetPixel(pRect->right  ,pRect->top+1,color);

  pDC->SetPixel(pRect->right-1,pRect->bottom  ,color);
  pDC->SetPixel(pRect->right  ,pRect->bottom  ,color);
  pDC->SetPixel(pRect->right  ,pRect->bottom-1,color);
}

void CEGToolBar::PaintToolBarBackGnd(CDC* pDC)
{
  if((m_dwStyle & CBRS_FLOATING))
  {
    return;
  }

  switch (CEGMenu::GetMenuDrawMode())
  {
  case CEGMenu::STYLE_XP:
  case CEGMenu::STYLE_XP_NOBORDER:

  case CEGMenu::STYLE_XP_2003_NOBORDER:
  case CEGMenu::STYLE_XP_2003:
  case CEGMenu::STYLE_COLORFUL_NOBORDER:
  case CEGMenu::STYLE_COLORFUL:

  case CEGMenu::STYLE_ICY:
  case CEGMenu::STYLE_ICY_NOBORDER:
    break;

  default:
    return;
  }

  //// To eliminate small border between menu and client rect
  //MENUINFO menuInfo = {0};
  //menuInfo.cbSize = sizeof(menuInfo);
  //menuInfo.fMask = MIM_BACKGROUND;
  HWND hParent = ::GetParent(m_hWnd);

  // if there is a parent to the parent, then get it
  // Jan-20-2005 - Mark P. Peterson - mpp@rhinosoft.com - http://www.RhinoSoft.com/
  // added the check to see if the parent has a parent before resetting the parent handle.
  if (::GetParent(hParent))
  {
    hParent = ::GetParent(hParent);
  }

  { // Block for local variable cleanup
    CRect clientRect;
    GetWindowRect(clientRect);
    CRect windowRect;
    ::GetWindowRect(hParent,windowRect);
    ScreenToClient(windowRect);
    ScreenToClient(clientRect);

    //CBrush *pBrush = CBrush::FromHandle( menuInfo.hbrBack );
    CBrush *pBrush = GetMenuBarBrush();
    if(pBrush)
    {
      // need for win95/98/me
      VERIFY(pBrush->UnrealizeObject());
      CPoint oldOrg = pDC->SetBrushOrg(windowRect.left,0);

      pDC->FillRect(clientRect,pBrush);
      //pDC->FillSolidRect(clientRect,RGB(255,0,0));
      pDC->SetBrushOrg(oldOrg);
    }
    else
    {
      pDC->FillSolidRect(clientRect,CEGMenu::GetMenuBarColor());
    }
  }
}

bool CEGToolBar::SetMenuButtonID(UINT nCommandID)
{
  int nTBIndex = CommandToIndex (nCommandID);
  if(nTBIndex==-1)
  {
    return false;
  }
  return SetMenuButton(nTBIndex);
}

bool CEGToolBar::SetMenuButton (int nIndex)
{
  SendMessage(TB_SETEXTENDEDSTYLE, 0, (LPARAM)TBSTYLE_EX_DRAWDDARROWS);

  TBBUTTON button;
  if(SendMessage(TB_GETBUTTON,nIndex,(LPARAM)&button))
  {
    SetButtonStyle(nIndex,button.fsStyle | TBBS_DROPDOWN);
    return true;
  }
  return false;
}

bool CEGToolBar::InsertControl (int nIndex, CWnd* pControl, DWORD_PTR dwData)
{
  HWND hWnd = pControl->GetSafeHwnd();
  if(!hWnd)
  {
    return false;
  }
  // Set the same font as in toolbar
  pControl->SetFont(GetFont());

  int nCount = (int)SendMessage(TB_BUTTONCOUNT, 0, 0);
  CRect rect;
  pControl->GetWindowRect(&rect);

  TBBUTTON tbbutton = {0};
  tbbutton.iBitmap = rect.Width();
  tbbutton.idCommand = pControl->GetDlgCtrlID();
  tbbutton.fsStyle = TBSTYLE_SEP;
  tbbutton.iString = -1;
  tbbutton.dwData = dwData;

  if ( nIndex < 0 || nIndex > nCount )
  {
    nIndex = nCount;
  }
  if (SendMessage(TB_INSERTBUTTON, nIndex, (LPARAM)&tbbutton) )
  {
    m_bDelayedButtonLayout = true;
    GetItemRect(nIndex,rect);
    //pControl->SetWindowPos(NULL,rect.top,rect.left,0,0,SWP_NOZORDER|SWP_NOSIZE|SWP_FRAMECHANGED);
    return true;
  }
  return false;
}

CEGToolBar* CEGToolBar::g_pNewToolBar = NULL;
HHOOK CEGToolBar::g_hMsgHook = NULL;

LRESULT CALLBACK CEGToolBar::MenuInputFilter(int code, WPARAM wParam, LPARAM lParam)
{
  return (code==MSGF_MENU && g_pNewToolBar && g_pNewToolBar->OnMenuInput( *((MSG*)lParam) )) ? TRUE : CallNextHookEx(g_hMsgHook, code, wParam, lParam);
}

BOOL CEGToolBar::OnMenuInput(MSG msg)
{
  ASSERT_VALID(this);
  if(msg.message==WM_MOUSEMOVE)
  {
    CPoint pt = msg.lParam;
    if(m_hWnd==::WindowFromPoint(pt))
    {
      ScreenToClient(&pt);
      TOOLINFO toolInfo = {0};
      INT_PTR nID = OnToolHitTest(pt,&toolInfo);
      if(nID)
      {
        int nTBIndex = (int)CommandToIndex ((UINT)nID);
        if(nTBIndex!=-1 && m_ActMenuIndex!=nTBIndex)
        {
          GetParentFrame()->PostMessage(WM_CANCELMODE,0,0);
        }
      }
    }
  }
  return FALSE; // pass along...
}

void CEGToolBar::TrackPopupMenu (UINT nID, CMenu* pMenu)
{
  int nTBIndex = CommandToIndex (nID);
  if(pMenu && nTBIndex!=-1)
  {
    CRect rcTBItem;
    GetItemRect (nTBIndex, rcTBItem);
    ClientToScreen (rcTBItem);

    SetButtonStyle (nTBIndex, GetButtonStyle (nTBIndex)|TBBS_INDETERMINATE);
    UpdateWindow();

    // Need to set the rectangle of the button!
    CEGMenu* pNewMenu = DYNAMIC_DOWNCAST(CEGMenu,pMenu);
    if(pNewMenu)
    {
      CEGMenu* pParentMenu = DYNAMIC_DOWNCAST(CEGMenu,CMenu::FromHandlePermanent(pNewMenu->GetParent()));
      if(pParentMenu)
      {
        pParentMenu->SetLastMenuRect(rcTBItem);
      }
    }

    // handle pending WM_PAINT messages
    MSG msg;
    while (::PeekMessage(&msg, NULL, WM_PAINT, WM_PAINT, PM_NOREMOVE))
    {
      if (!GetMessage(&msg, NULL, WM_PAINT, WM_PAINT))
        return;
      DispatchMessage(&msg);
    }

    ASSERT(g_pNewToolBar==NULL);
    ASSERT(g_hMsgHook==NULL);

    m_ActMenuIndex = nTBIndex;
    g_pNewToolBar = this;
    g_hMsgHook = ::SetWindowsHookEx(WH_MSGFILTER, MenuInputFilter, NULL, AfxGetApp()->m_nThreadID);

    TrackPopupMenuSpecial(pMenu,0,rcTBItem,GetParentFrame(),m_dwStyle & CBRS_ORIENT_VERT);

    // uninstall hook
    ::UnhookWindowsHookEx(g_hMsgHook);
    g_pNewToolBar = NULL;
    g_hMsgHook = NULL;
    m_ActMenuIndex = -1;

    SetButtonStyle (nTBIndex, GetButtonStyle (nTBIndex)&~TBBS_INDETERMINATE);
  }
}

LRESULT CEGToolBar::OnIdleUpdateCmdUI(WPARAM wParam, LPARAM lParam)
{
  // handle delay hide/show
  BOOL bVis = GetStyle() & WS_VISIBLE;
  UINT swpFlags = 0;
  if ((m_nStateFlags & delayHide) && bVis)
    swpFlags = SWP_HIDEWINDOW;
  else if ((m_nStateFlags & delayShow) && !bVis)
    swpFlags = SWP_SHOWWINDOW;
  m_nStateFlags &= ~(delayShow|delayHide);
  if (swpFlags != 0)
  {
    SetWindowPos(NULL, 0, 0, 0, 0, swpFlags|
      SWP_NOMOVE|SWP_NOSIZE|SWP_NOZORDER|SWP_NOACTIVATE);
  }

  // the style must be visible and if it is docked
  // the dockbar style must also be visible
  if ((GetStyle() & WS_VISIBLE) &&
    (m_pDockBar == NULL || (m_pDockBar->GetStyle() & WS_VISIBLE)))
  {
    CFrameWnd* pTarget = (CFrameWnd*)GetOwner();

    CEGDialog* pDlg = DYNAMIC_DOWNCAST(CEGDialog,pTarget);
    CEGDockingPane* pPane = DYNAMIC_DOWNCAST(CEGDockingPane,pTarget);
		if(pDlg || pPane ) {
      OnUpdateCmdUI(pTarget, (BOOL)wParam);
    } else {
      if (pTarget == NULL || !pTarget->IsFrameWnd())
        pTarget = GetParentFrame();
      if (pTarget != NULL)
        OnUpdateCmdUI(pTarget, (BOOL)wParam);
    }
  }
  return 0L;
}
