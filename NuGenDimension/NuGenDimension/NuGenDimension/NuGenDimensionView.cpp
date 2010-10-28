// NuGenDimensionView.cpp : implementation of the CNuGenDimensionView class
//

#include "stdafx.h"
#include "NuGenDimension.h"

#include "NuGenDimensionDoc.h"
#include "NuGenDimensionView.h"
#include "ChildFrm.h"

#include "Drawer.h" 

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

CNuGenDimensionView*       global_opengl_view = NULL;

static GLfloat g_fMouseZMult	  = 0.01f;	// Mouse 'Z'-Axis multiplier

// CNuGenDimensionView

IMPLEMENT_DYNCREATE(CNuGenDimensionView, COpenGLView)

BEGIN_MESSAGE_MAP(CNuGenDimensionView, COpenGLView)
	ON_COMMAND(ID_FILE_PRINT, CView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, CView::OnFilePrint)
	//ON_COMMAND(ID_FILE_PRINT_PREVIEW, CView::OnFilePrintPreview)
	ON_WM_LBUTTONDOWN()
	ON_WM_LBUTTONUP()
	ON_WM_MOUSEMOVE()
	ON_WM_SETCURSOR()	
	ON_WM_RBUTTONDOWN()
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, OnFilePrintPreview)
END_MESSAGE_MAP()

// CNuGenDimensionView construction/destruction

CNuGenDimensionView::CNuGenDimensionView()
{
	global_opengl_view = this;
	m_isPrinting = false;
}

CNuGenDimensionView::~CNuGenDimensionView()
{
	global_opengl_view = NULL;
}

BOOL CNuGenDimensionView::PreCreateWindow(CREATESTRUCT& cs)
{
	cs.style |= WS_CLIPSIBLINGS | WS_CLIPCHILDREN|CS_OWNDC;
	return COpenGLView::PreCreateWindow(cs);
}

void CNuGenDimensionView::OnInitialUpdate()
{
	COpenGLView::OnInitialUpdate();

	static_cast<CChildFrame*>(GetParentFrame())->SetView(this);

	m_Camera.ReInit();

	if (sgGetScene()->GetObjectsList()->GetCount()>0)
	{
		SG_POINT a1,a2;
		sgGetScene()->GetGabarits(a1,a2);
		m_Camera.FitBounds(a1.x,a1.y,a1.z,a2.x,a2.y,a2.z);	
	}
	
	Invalidate();
}

#include "Tools//TranslateCommand.h"
#include "Tools//RotateCommand.h"
void  CNuGenDimensionView::DrawScene(GLenum mode, bool selSubObj)
{
	sgCObject*  curObj = sgGetScene()->GetObjectsList()->GetHead();
	while (curObj) 
	{
		if (/*sgGetScene()->GetLayerVisible(curObj->GetAttribute(SG_OA_LAYER))*/true)
		{
			Drawer::DrawObject(mode,curObj,selSubObj);
			
			if ((curObj->GetAttribute(SG_OA_DRAW_STATE) & SG_DS_GABARITE) &&
				curObj!=Drawer::CurrentEditableObject)
			{
				SG_POINT a1,a2;
				curObj->GetGabarits(a1,a2);
				Drawer::DrawGabariteBox(a1,a2,Drawer::HotObjectColor);
			}

			/*if (curObj->GetType()==SG_OT_3D)
			{
				sgC3DObject* ooo = reinterpret_cast<sgC3DObject*>(curObj);
				for (unsigned int i=0;i<ooo->GetBRep()->GetPiecesCount();i++)
				{
					SG_POINT s1,s2;
					ooo->GetBRep()->GetPiece(i)->GetLocalGabarits(s1,s2);
					Drawer::DrawGabariteBox(s1,s2,Drawer::HotObjectColor);
				}
			}*/
		}
		curObj = sgGetScene()->GetObjectsList()->GetNext(curObj);
	}
	/*SG_POINT s1,s2;
	sgGetScene()->GetGabarits(s1,s2);
	Drawer::DrawGabariteBox(s1,s2,Drawer::HotObjectColor);*/
}

void CNuGenDimensionView::DrawFromCommander()
{
	CChildFrame* pFrame = static_cast<CChildFrame*>(GetParentFrame());
	if (pFrame->m_commander)
		pFrame->m_commander->Draw();
}

void CNuGenDimensionView::OnDraw(CDC* pDC)
{
	if (pDC->IsPrinting()) 
	{
		CRect rcDIB;
		GetClientRect(&rcDIB);

		rcDIB.right = rcDIB.Width();
		rcDIB.bottom = rcDIB.Height();


		// get size of printer page (in pixels)
		int cxPage = pDC->GetDeviceCaps(HORZRES);
		int cyPage = pDC->GetDeviceCaps(VERTRES);
		// get printer pixels per inch
		int cxInch = pDC->GetDeviceCaps(LOGPIXELSX);
		int cyInch = pDC->GetDeviceCaps(LOGPIXELSY);

		CRect rcDest;
		rcDest.top = rcDest.left = 0;
		rcDest.bottom = (int)(((double)rcDIB.Height() * cxPage * cyInch)
			/ ((double)rcDIB.Width() * cxInch));
		rcDest.right = cxPage;

		m_CapturedImage.OnDraw(pDC->m_hDC, &rcDest, &rcDIB);

	}
}


BOOL CNuGenDimensionView::OnPreparePrinting(CPrintInfo* pInfo)
{
	if(!pInfo->m_bPreview)
	{
		CRect rcDIB;
		GetClientRect(&rcDIB);
		CDC* sss = CDC::FromHandle(m_hDC);
		OnDraw(sss);
		m_CapturedImage.Capture(sss, rcDIB);
	}
	return DoPreparePrinting(pInfo);
}


void CNuGenDimensionView::OnFilePrintPreview()
{
	AfxGetMainWnd()->SendMessage(WM_CANCELMODE, 0, 0);
	CRect rcDIB;
	GetClientRect(&rcDIB);
	CDC* sss = CDC::FromHandle(m_hDC);
	OnDraw(sss);
	m_CapturedImage.Capture(sss, rcDIB);

	CView::OnFilePrintPreview();
}


void CNuGenDimensionView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	m_isPrinting = true;
	CChildFrame* pFrame = static_cast<CChildFrame*>(GetParentFrame());
	pFrame->FreeCommander();
}

void CNuGenDimensionView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	m_CapturedImage.Release();
	m_isPrinting = false;
}



// CNuGenDimensionView diagnostics

#ifdef _DEBUG
void CNuGenDimensionView::AssertValid() const
{
	COpenGLView::AssertValid();
}

void CNuGenDimensionView::Dump(CDumpContext& dc) const
{
	COpenGLView::Dump(dc);
}

CNuGenDimensionDoc* CNuGenDimensionView::GetDocument() const // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CNuGenDimensionDoc)));
	return (CNuGenDimensionDoc*)m_pDocument;
}
#endif //_DEBUG

void CNuGenDimensionView::OnLButtonDown(UINT nFlags, CPoint point)
{
	CChildFrame* pFrame = static_cast<CChildFrame*>(GetParentFrame());

	// Save the mouse left button down screen position
	m_ScreenLeftButtonDownPoint = point;

	SetCapture();

	COpenGLView::OnLButtonDown(nFlags, point);
}

void CNuGenDimensionView::OnLButtonUp(UINT nFlags, CPoint point)
{
	
	m_ScreenLeftButtonDownPoint.x = 0;
	m_ScreenLeftButtonDownPoint.y = 0;

	if (GetCapture()==this)
		ReleaseCapture();

	COpenGLView::OnLButtonUp(nFlags, point);
}

void CNuGenDimensionView::OnMouseMove(UINT nFlags, CPoint point)
{
	CChildFrame* pFrame = static_cast<CChildFrame*>(GetParentFrame());
	SetFocus();
	if (!pFrame->m_commander)
	{
				switch (m_hand_action )
				{
				case HA_ROTATE:
					if (nFlags & MK_LBUTTON)
					{
						SG_VECTOR downPnt;
						SG_VECTOR curPnt;
						// Convert the mouse left button down position to world
						m_Camera.GetWorldCoord(m_ScreenLeftButtonDownPoint.x, 
							m_ScreenLeftButtonDownPoint.y, 
							0.0, 
							downPnt);
						// Convert the mouse point into world coordinates
						m_Camera.GetWorldCoord(point.x,	point.y, 0.0,curPnt);
						//VecSubf(curPnt, downPnt, curPnt);
						curPnt = sgSpaceMath::VectorsSub(curPnt, downPnt);
						CSize deltaPos;
						deltaPos = m_ScreenLeftButtonDownPoint - point;
						m_ScreenLeftButtonDownPoint = point;
						RotateCamera(deltaPos);
						Invalidate(FALSE);
					}
					break;
				case HA_MOVE:
					if (nFlags & MK_LBUTTON)
					{
						SG_VECTOR downPnt;
						SG_VECTOR curPnt;
						// Convert the mouse left button down position to world
						m_Camera.GetWorldCoord(m_ScreenLeftButtonDownPoint.x, 
							m_ScreenLeftButtonDownPoint.y, 
							0.0, 
							downPnt);
						// Convert the mouse point into world coordinates
						m_Camera.GetWorldCoord(point.x,	point.y, 0.0,curPnt);
						//VecSubf(curPnt, downPnt, curPnt);
						curPnt = sgSpaceMath::VectorsSub(curPnt, downPnt);
						m_ScreenLeftButtonDownPoint = point;
						TranslateCamera(curPnt);
						Invalidate(FALSE);
					}
					break;
				case HA_ZOOM:
					if (nFlags & MK_LBUTTON)
					{
						SG_VECTOR downPnt;
						SG_VECTOR curPnt;
						// Convert the mouse left button down position to world
						m_Camera.GetWorldCoord(m_ScreenLeftButtonDownPoint.x, 
							m_ScreenLeftButtonDownPoint.y, 
							0.0, 
							downPnt);
						// Convert the mouse point into world coordinates
						m_Camera.GetWorldCoord(point.x,	point.y, 0.0,curPnt);
						//VecSubf(curPnt, downPnt, curPnt);
						curPnt = sgSpaceMath::VectorsSub(curPnt, downPnt);
						CSize deltaPos;
						deltaPos = m_ScreenLeftButtonDownPoint - point;
						m_ScreenLeftButtonDownPoint = point;
						ZoomCamera(deltaPos);
						Invalidate(FALSE);
					}
					break;
				default:
					break;
				}
	}

	if (!pFrame->m_commander)
		/*pFrame->m_commander->MouseMove(nFlags,point.x,point.y);
	else*/
		if (!(nFlags & MK_LBUTTON))
		{
			int snapSz = pFrame->GetSnapSize();
			const sgCObject* oldHotObj = Drawer::CurrentHotObject;

				Drawer::CurrentHotObject = GetTopObject(GetHitsInRect(CRect(point.x-snapSz, point.y-snapSz,
							point.x+snapSz, point.y+snapSz),true));
				Drawer::TopParentOfHotObject = GetObjectTopParent(Drawer::CurrentHotObject);
		  if (Drawer::TopParentOfHotObject)
				Drawer::CurrentHotObject = Drawer::TopParentOfHotObject;
				/**************************************/
				if (Drawer::CurrentHotObject &&
					(Drawer::CurrentHotObject->GetType()==SG_OT_LINE ||
					 Drawer::CurrentHotObject->GetType()==SG_OT_CIRCLE ||
					 Drawer::CurrentHotObject->GetType()==SG_OT_SPLINE ||
					 Drawer::CurrentHotObject->GetType()==SG_OT_ARC ||
					 Drawer::CurrentHotObject->GetType()==SG_OT_CONTOUR)
					)
				{
					sgC2DObject* spl = reinterpret_cast<sgC2DObject*>(Drawer::CurrentHotObject);
					CString messs = "2DObject ";
					if (spl->IsClosed())
						messs += " Closed; ";
					else
						messs += " No closed; ";
					if (spl->IsLinear())
					{
						messs += " Linear;";
					}
					else
					{
						messs += " No linear - ";
						SG_VECTOR plN;
						double    plD;
						if (spl->IsPlane(&plN,&plD))
						{
							CString aaaa;
							aaaa.Format("Normal: X=%f,Y=%f,Z=%f D: %f",plN.x,plN.y,plN.z,plD);
							messs += " Plane; ";
							messs+=aaaa;
						}
						else
							messs += " No Plane; ";
					}
					
					if (spl->IsSelfIntersecting())
						messs += "Self intersecing; ";
					else
						messs += "No Self intersecing; ";

					pFrame->PutMessage(IApplicationInterface::MT_MESSAGE,
						messs);

				}
				else
					if (Drawer::CurrentHotObject &&	(Drawer::CurrentHotObject->GetType()==SG_OT_3D))
					{
						sgC3DObject* ddd = reinterpret_cast<sgC3DObject*>(Drawer::CurrentHotObject);
						CString messs;
						messs.Format("Volume = %f",ddd->GetVolume());
						messs+="  Square = ";
						CString aaaa;
						aaaa.Format("%f",ddd->GetSquare());
						messs+=aaaa;
						pFrame->PutMessage(IApplicationInterface::MT_MESSAGE,
							messs);
					}
					else
                        pFrame->PutMessage(IApplicationInterface::MT_MESSAGE,
								"                        ");
				/**************************************/
			
				if (oldHotObj!=Drawer::CurrentHotObject)
					Invalidate();
		}
	
	COpenGLView::OnMouseMove(nFlags, point);
}

BOOL CNuGenDimensionView::OnSetCursor(CWnd* pWnd, UINT nHitTest, UINT message)
{
	CMainFrame* mFr = static_cast<CMainFrame*>(theApp.m_pMainWnd);
	if (!mFr)
		return COpenGLView::OnSetCursor(pWnd, nHitTest, message);

	HCURSOR   curs = mFr->GetCursorer()->GetCursor();
	if (!curs)
		return COpenGLView::OnSetCursor(pWnd, nHitTest, message);

	::SetCursor(curs);
	return TRUE;
}

static WORD cm_icons[] = { IDB_PROJECTION_TOOLBAR_TC, 
16,16,
ID_ALL_SCENE_VIEW,
ID_PARALL_PROJ,
ID_PERSPECT_PROJ,
ID_FRONT_VIEW,
ID_BACK_VIEW,
ID_TOP_VIEW,
ID_BOTTOM_VIEW,
ID_LEFT_VIEW,
ID_RIGHT_VIEW,
ID_ISO_FRONT_VIEW,
ID_ISO_BACK_VIEW,
NULL
};

void CNuGenDimensionView::OnRButtonDown(UINT nFlags, CPoint point)
{
	CChildFrame* pFrame = static_cast<CChildFrame*>(GetParentFrame());

	if (!pFrame->m_commander && !Drawer::CurrentHotObject)
	{
		
		CEGMenu menu;
		menu.CreatePopupMenu();
		int nItem=0;
		UINT  chs=0;
		chs = (sgGetScene()->GetObjectsList()->GetCount()!=0)?MF_ENABLED:MF_GRAYED;
		menu.InsertMenu(nItem++, MF_BYPOSITION|chs, ID_ALL_SCENE_VIEW, GetLeftHalfOfString(ID_ALL_SCENE_VIEW));
		menu.InsertMenu(nItem++, MF_BYPOSITION|MF_SEPARATOR);
		chs = (!m_Camera.m_bPerspective)?MF_CHECKED:0;
		menu.InsertMenu(nItem++, MF_BYPOSITION|chs , ID_PARALL_PROJ, GetLeftHalfOfString(ID_PARALL_PROJ));
		chs = (m_Camera.m_bPerspective)?MF_CHECKED:0;
		menu.InsertMenu(nItem++, MF_BYPOSITION|chs, ID_PERSPECT_PROJ, GetLeftHalfOfString(ID_PERSPECT_PROJ));
		menu.InsertMenu(nItem++, MF_BYPOSITION|MF_SEPARATOR);
		chs = (m_Camera.m_enumCameraPosition==CP_FRONT)?MF_CHECKED:0;
		menu.InsertMenu(nItem++, MF_BYPOSITION|chs, ID_FRONT_VIEW, GetLeftHalfOfString(ID_FRONT_VIEW));
		chs = (m_Camera.m_enumCameraPosition==CP_BACK)?MF_CHECKED:0;
		menu.InsertMenu(nItem++, MF_BYPOSITION|chs,  ID_BACK_VIEW, GetLeftHalfOfString(ID_BACK_VIEW));
		chs = (m_Camera.m_enumCameraPosition==CP_TOP)?MF_CHECKED:0;
		menu.InsertMenu(nItem++, MF_BYPOSITION|chs , ID_TOP_VIEW, GetLeftHalfOfString(ID_TOP_VIEW));
		chs = (m_Camera.m_enumCameraPosition==CP_BOTTOM)?MF_CHECKED:0;
		menu.InsertMenu(nItem++, MF_BYPOSITION|chs , ID_BOTTOM_VIEW, GetLeftHalfOfString(ID_BOTTOM_VIEW));
		chs = (m_Camera.m_enumCameraPosition==CP_LEFT)?MF_CHECKED:0;
		menu.InsertMenu(nItem++, MF_BYPOSITION|chs,  ID_LEFT_VIEW, GetLeftHalfOfString(ID_LEFT_VIEW));
		chs = (m_Camera.m_enumCameraPosition==CP_RIGHT)?MF_CHECKED:0;
		menu.InsertMenu(nItem++, MF_BYPOSITION|chs , ID_RIGHT_VIEW, GetLeftHalfOfString(ID_RIGHT_VIEW));
		menu.InsertMenu(nItem++, MF_BYPOSITION|MF_SEPARATOR);
		chs = (m_Camera.m_enumCameraPosition==CP_ISO_FRONT)?MF_CHECKED:0;
		menu.InsertMenu(nItem++, MF_BYPOSITION|chs,  ID_ISO_FRONT_VIEW, GetLeftHalfOfString(ID_ISO_FRONT_VIEW));
		chs = (m_Camera.m_enumCameraPosition==CP_ISO_BACK)?MF_CHECKED:0;
		menu.InsertMenu(nItem++, MF_BYPOSITION|chs , ID_ISO_BACK_VIEW, GetLeftHalfOfString(ID_ISO_BACK_VIEW));
		//menu.InsertMenu(nItem++, MF_BYPOSITION|MF_SEPARATOR);

		menu.LoadToolBar( cm_icons, RGB(0,0,0) ); 

		//menu.SetDefaultItem(ID_EDIT_PASTE);

		CRect clR;
		GetWindowRect(&clR);

		menu.TrackPopupMenu( TPM_LEFTALIGN | TPM_LEFTBUTTON | 
			TPM_RIGHTBUTTON|0x0000, point.x+clR.left,point.y+clR.top, this);

	}
	else
	{
		if (pFrame->m_commander)
		{
			CRect clR;
			GetWindowRect(&clR);
			
			pFrame->CommanderContextMenu(point.x+clR.left,point.y+clR.top);

		}
		else
			if (Drawer::CurrentHotObject)
			{
				CRect clR;
				GetWindowRect(&clR);

				pFrame->EditCommanderContextMenu(point.x+clR.left,point.y+clR.top);
			}

	}

	COpenGLView::OnRButtonDown(nFlags, point);
}
