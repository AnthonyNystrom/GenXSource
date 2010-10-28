#include "stdafx.h"
#include "EGMenu.h"
#include "EGDialogBar.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

#define CX_GRIPPER  3
#define CY_GRIPPER  3
#define CX_BORDER_GRIPPER 2
#define CY_BORDER_GRIPPER 2


// CMyDialogBar

IMPLEMENT_DYNAMIC(CEGDialogBar, CDialogBar)
CEGDialogBar::CEGDialogBar()
{
}

CEGDialogBar::~CEGDialogBar()
{
}

BEGIN_MESSAGE_MAP(CEGDialogBar, CDialogBar)
  ON_WM_NCPAINT()
  ON_WM_ERASEBKGND()
  ON_WM_CREATE()
  ON_WM_PAINT()
END_MESSAGE_MAP()

// CEGDialogBar message handlers
void CEGDialogBar::DrawGripper(CDC* pDC, const CRect& rect)
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
          COLORREF col = DarkenColorXP(CEGMenu::GetMenuBarColorXP());
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
          for (int n=rect.top+2*CY_BORDER_GRIPPER;n<rect.Height()-m_cyTopBorder-m_cyBottomBorder-4*CY_BORDER_GRIPPER;n+=4)
          {
            pDC->FillSolidRect(rect.left+CX_BORDER_GRIPPER+2,n  +1,2,2,color);
          }
           // make round corners
          color = GetSysColor(COLOR_3DLIGHT);//CEGMenu::GetMenuBarColor();
          CRect temp(rect);
          temp.InflateRect(0,-2,-1,-3);
          PaintCorner(pDC,temp,color);
       }
        break;

      case CEGMenu::STYLE_XP_2003_NOBORDER:
      case CEGMenu::STYLE_XP_2003:
      case CEGMenu::STYLE_COLORFUL_NOBORDER:
      case CEGMenu::STYLE_COLORFUL:
        {
          COLORREF color = DarkenColor(10,GetSysColor(COLOR_ACTIVECAPTION));

          for (int n=rect.top+m_cyTopBorder+2*CY_BORDER_GRIPPER;n<rect.Height()-m_cyTopBorder-m_cyBottomBorder-4*CY_BORDER_GRIPPER;n+=4)
          {
            pDC->FillSolidRect(rect.left+CX_BORDER_GRIPPER+3,n+1+1,2,2,RGB(255,255,255));
            pDC->FillSolidRect(rect.left+CX_BORDER_GRIPPER+2,n  +1,2,2,color);
          }
          // make round corners
          color = CEGMenu::GetMenuBarColor2003();
          CRect temp(rect);
          temp.InflateRect(0,-2,-1,-3);
          PaintCorner(pDC,temp,color);
        }
        break;

      default:
        CDialogBar::DrawGripper(pDC,rect);
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
          COLORREF col = DarkenColorXP(CEGMenu::GetMenuBarColorXP());
          CPen pen(PS_SOLID,0,col);
          CPen* pOld = pDC->SelectObject(&pen);
          for (int n=rect.top+2*CX_BORDER_GRIPPER;n<rect.Width()-2*CX_BORDER_GRIPPER;n+=2)
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
            pDC->FillSolidRect(n,  rect.top+CY_BORDER_GRIPPER+2,2,2,color);
          }
          // make the corners round
          color = GetSysColor(COLOR_3DLIGHT);//CEGMenu::GetMenuBarColor();
          CRect temp(rect);
          temp.InflateRect(-2,0,-3,-1);
          PaintCorner(pDC,temp,color);
        }
        break;

      case CEGMenu::STYLE_XP_2003_NOBORDER:
      case CEGMenu::STYLE_XP_2003:
      case CEGMenu::STYLE_COLORFUL_NOBORDER:
      case CEGMenu::STYLE_COLORFUL:
        {
          COLORREF color = DarkenColor(10,GetSysColor(COLOR_ACTIVECAPTION));
          for (int n=rect.top+2*CX_BORDER_GRIPPER;n<rect.Width()-3*CX_BORDER_GRIPPER;n+=4)
          {
            pDC->FillSolidRect(n+1,rect.top+CY_BORDER_GRIPPER+3,2,2,RGB(255,255,255));
            pDC->FillSolidRect(n,  rect.top+CY_BORDER_GRIPPER+2,2,2,color);
          }
          // make the corners round
          color = CEGMenu::GetMenuBarColor2003();
          CRect temp(rect);
          temp.InflateRect(-2,0,-3,-1);
          PaintCorner(pDC,temp,color);
        }
        break;

      default:
        CDialogBar::DrawGripper(pDC,rect);
        break;
      }
    }
  }
}

void CEGDialogBar::OnPaint()
{
  CDialogBar::OnPaint();
}

void CEGDialogBar::DoPaint(CDC* pDC)
{
  // we do not call basis class, because needed function are not virtual declared
	ASSERT_VALID(this);
	ASSERT_VALID(pDC);

	// paint inside client area
	CRect rect;
	GetClientRect(rect);
	DrawBorders(pDC, rect);
	DrawGripper(pDC, rect);
}


void CEGDialogBar::OnNcPaint()
{
  EraseNonClient();
}

void CEGDialogBar::EraseNonClient()
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

void CEGDialogBar::DrawBorders(CDC* pDC, CRect& rect)
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
    CDialogBar::DrawBorders(pDC,rect);
    break;
  }
}

BOOL CEGDialogBar::OnEraseBkgnd(CDC* pDC)
{
  BOOL bRet = true;

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
        if(!(m_dwStyle & CBRS_FLOATING))
        {
          rcToolBar.bottom = rect.bottom - 2;
          rcToolBar.top = rect.top + 2;
        }
      }
      else
      {
        rcToolBar.left += 2;
        rcToolBar.right -= 3;
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
      if(m_dwStyle & CBRS_ORIENT_HORZ)
      {
        CRect rcToolBar = rect;
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
            CRect line(rect.left+2,rect.bottom-3,rect.right-2,rect.bottom-2);
            //dark line on bottom toolbar
            pDC->FillSolidRect(line,clrDarkLine);
          }
        }
      }
      else
      {
        if(!(m_dwStyle & CBRS_FLOATING))
        {
          if(clrDarkLine!=CLR_NONE)
          {
            CRect line(rect.right-3,rect.top,rect.right-2,rect.bottom);
            //dunkler strich am unteren ende der toolbar
            pDC->FillSolidRect(line,clrDarkLine);
          }
        }
        CRect rcToolBar = rect;
        rcToolBar.left += 2;
        rcToolBar.right /= 2;
        DrawGradient(pDC,rcToolBar,clrUpperColor,clrMediumColor,true);
        rcToolBar.left = rcToolBar.right;
        rcToolBar.right = rect.right-3;
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
        if(!(m_dwStyle & CBRS_FLOATING))
        {
          rcToolBar.bottom = rect.bottom - 2;
          rcToolBar.top = rect.top + 2;
        }
      }
      else
      {
        rcToolBar.left += 2;
        rcToolBar.right -= -3;
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
          CRect line(rect.left+2,rect.bottom-3,rect.right-2,rect.bottom-2);
          //dark line on bottom toolbar
          pDC->FillSolidRect(line,clrDarkLine);
        }
      }
      else
      {
        if(!(m_dwStyle & CBRS_FLOATING))
        {
           if(clrDarkLine!=CLR_NONE)
          {
            CRect line(rect.right-3,rect.top,rect.right-2,rect.bottom);
            //dunkler strich am unteren ende der toolbar
            pDC->FillSolidRect(line,clrDarkLine);
          }
        }
        CRect rcToolBar = rect;
        rcToolBar.left += 2;
        rcToolBar.right /= 2;
        DrawGradient(pDC,rcToolBar,clrUpperColor,clrMediumColor,true);
        rcToolBar.left = rcToolBar.right;
        rcToolBar.right = rect.right-3;
        DrawGradient(pDC,rcToolBar,clrMediumColor,clrBottomColor,true);
      }
    }
    break;

  default:
    {
      pDC->FillSolidRect(rect,GetSysColor(COLOR_3DLIGHT));
    }
    break;
  }
  return bRet;
}

void CEGDialogBar::PaintToolBarBackGnd(CDC* pDC)
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

  HWND hParent = ::GetParent(m_hWnd);
  // if there is a parent to the parent, then get it
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

    CBrush*pBrush = GetMenuBarBrush();
    if(pBrush)
    {
      // need for win95/98/me
      VERIFY(pBrush->UnrealizeObject());
      CPoint oldOrg = pDC->SetBrushOrg(windowRect.left,0);

      pDC->FillRect(clientRect,pBrush);
      pDC->SetBrushOrg(oldOrg);
    }
    else
    {
      pDC->FillSolidRect(clientRect,CEGMenu::GetMenuBarColor());
    }
  }
}

void CEGDialogBar::PaintCorner(CDC *pDC, LPCRECT pRect, COLORREF color)
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