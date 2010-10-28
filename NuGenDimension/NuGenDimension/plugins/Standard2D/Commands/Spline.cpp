#include "stdafx.h"

#include "Spline.h"

#include "..//resource.h"
#include <math.h>

int     spline_name_index = 1;

SplineCommand::SplineCommand(IApplicationInterface* appI):
						m_app(appI)
						, m_get_point_panel(NULL)
						, m_existing_points(NULL)
{
	ASSERT(m_app);
	m_ex_temp_pnt = false;
	m_geo = NULL;
}

SplineCommand::~SplineCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	if (m_existing_points)
	{
		m_existing_points->DestroyWindow();
		delete m_existing_points;
		m_existing_points = NULL;
	}
	m_app->GetViewPort()->InvalidateViewPort();
	if (m_geo)
		SG_SPLINE::Delete(m_geo);
}

bool    SplineCommand::PreTranslateMessage(MSG* pMsg)
{
	try {
		/*if (pMsg->message==WM_KEYUP||
		pMsg->message==WM_CHAR)
		return false;*/

		if (pMsg->message==WM_KEYUP||pMsg->message==WM_KEYDOWN || 
			pMsg->message==WM_CHAR)
		{
			if (pMsg->wParam==VK_RETURN)
			{
				OnEnter();
				return true;
			}
			if (pMsg->wParam==VK_ESCAPE)
			{
				m_app->StopCommander();
				return true;
			}
			if (m_get_point_panel)
			{
				m_get_point_panel->GetWindow()->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
			}
			if (pMsg->message==WM_KEYDOWN)
				return false;
			else 
				return true;
		}
		else
		{
			if (pMsg->hwnd == m_app->GetViewPort()->GetWindow()->m_hWnd)
			{
				switch(pMsg->message) 
				{
				case WM_MOUSEMOVE:
					MouseMove(pMsg->wParam,GET_X_LPARAM(pMsg->lParam),GET_Y_LPARAM(pMsg->lParam));
					return true;
				case WM_LBUTTONDOWN:
					LeftClick(pMsg->wParam,GET_X_LPARAM(pMsg->lParam),GET_Y_LPARAM(pMsg->lParam));
					return true;
				default:
					return false;
				}	
			}
		}
	}
	catch(...){
	}
	return false;
}

void  SplineCommand::Start()	
{
	SWITCH_RESOURCE
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_message.LoadString(IDS_TOOLTIP_FOURTH);
	m_app->StartCommander(m_message);
	m_existing_points = new CSplinePointsDlg;
	m_existing_points->Create(IDD_SPLINE_POINTS_DLG,
		m_app->GetCommandPanel()->GetDialogsContainerWindow());

	m_message.LoadString(IDS_SPLINE_EXISTING_KNOTS);
	m_app->GetCommandPanel()->AddDialog(m_existing_points,m_message,false);
	
	m_message.LoadString(IDS_SPL_NEW_KNOT);
	m_get_point_panel = reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,m_message,false));
	m_geo = SG_SPLINE::Create();
	m_get_point_panel->EnableControls(true);
	m_app->GetCommandPanel()->EnableRadio(0,true);
	m_app->GetCommandPanel()->EnableRadio(1,true);
	m_message.LoadString(IDS_SPL_ENTER_NEW_KNOT);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
}

void  SplineCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	ASSERT(m_get_point_panel);

	IViewPort::GET_SNAP_IN in_arg;
	in_arg.scrX = pX;
	in_arg.scrY = pY;
	in_arg.snapType = SNAP_SYSTEM;
	in_arg.XFix = m_get_point_panel->IsXFixed();
	in_arg.YFix = m_get_point_panel->IsYFixed();
	in_arg.ZFix = m_get_point_panel->IsZFixed();
	m_get_point_panel->GetPoint(in_arg.FixPoint.x,in_arg.FixPoint.y,in_arg.FixPoint.z);
	IViewPort::GET_SNAP_OUT out_arg;
	m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
	m_cur_pnt = out_arg.result_point;
	m_get_point_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);

	const unsigned int knC = m_geo->GetKnotsCount();
	if (knC!=0)
	{
		if (!m_geo->IsClosed())
			m_geo->MoveKnot(knC-1,m_cur_pnt);
		if (knC>3)
		{
			int snSz = m_app->GetViewPort()->GetSnapSize();
			double  coords[3];
			m_app->GetViewPort()->ProjectWorldPoint(m_first_point,coords[0],coords[1],coords[2]);
			if (sqrt((coords[0]-pX)*(coords[0]-pX)+
				(coords[1]-pY)*(coords[1]-pY))<=snSz ||
				sgSpaceMath::PointsDistance(m_first_point,m_cur_pnt)<0.0001)
			{
				if (!m_geo->IsClosed())
				{
					m_geo->DeleteKnot(m_geo->GetKnotsCount()-1);
					if (!m_geo->Close())
					{
						SWITCH_RESOURCE
						m_message.LoadString(IDS_CANNT_CLOSE_SPLINE);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
						//m_geo->AddKnot(m_temp_pnt, m_geo->GetKnotsCount());
						m_geo->AddKnot(m_cur_pnt, m_geo->GetKnotsCount());
					}
					else
					{
						m_temp_pnt = m_geo->GetKnots()[m_geo->GetKnotsCount()-3];
						m_ex_temp_pnt = true;
						m_geo->DeleteKnot(m_geo->GetKnotsCount()-3);
					}
				}
			}
			else
			{
				if (m_geo->IsClosed())
				{
					//
					m_geo->UnClose(m_geo->GetKnotsCount());
					if (m_ex_temp_pnt)
					{
						//m_geo->DeleteKnot(m_geo->GetKnotsCount());
						m_geo->MoveKnot(m_geo->GetKnotsCount()-1,m_temp_pnt);
						m_geo->AddKnot(m_cur_pnt, m_geo->GetKnotsCount());
						m_ex_temp_pnt = false;
					}
				}
			}
		}
		
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

void  SplineCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	const unsigned int knC = m_geo->GetKnotsCount();
	if (knC==0)
	{
		m_geo->AddKnot(m_cur_pnt,0);
		m_first_point = m_cur_pnt;
		m_last_point  = m_cur_pnt;
		if (m_existing_points)
			m_existing_points->AddPoint(m_first_point);
		m_geo->AddKnot(m_cur_pnt,1);
	}
	else
	{
		if (m_geo->IsClosed())
		{
			sgCSpline* spl = sgCreateSpline(*m_geo);
			if (!spl)
			{
				SG_SPLINE::Delete(m_geo);
				m_geo = SG_SPLINE::Create();
				m_message.LoadString(IDS_CREARE_SPL_ERROR);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			CString nm;
			nm.LoadString(IDS_TOOLTIP_FOURTH);
			CString nmInd;
			nmInd.Format("%i",spline_name_index);
			nm+=nmInd;
			spl->SetName(nm.GetBuffer());
			sgGetScene()->StartUndoGroup();
			sgGetScene()->AttachObject(spl);
			m_app->ApplyAttributes(spl);
			sgGetScene()->EndUndoGroup();
			spline_name_index++;
			SG_SPLINE::Delete(m_geo);
			m_geo = SG_SPLINE::Create();
			if (m_existing_points)
				m_existing_points->RemoveAllPoints();
		}
		else
		{
			if (sgSpaceMath::PointsDistance(m_cur_pnt,m_last_point)<0.001)
			{
				m_message.LoadString(IDS_ERROR_SPL_KNOT_AS_PREV);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
            m_geo->AddKnot(m_cur_pnt,knC);
			m_last_point = m_cur_pnt;
			if (m_existing_points)
				m_existing_points->AddPoint(m_cur_pnt);
		}
	}
	m_app->GetViewPort()->InvalidateViewPort();
}


void  SplineCommand::Draw()
{
	if (!m_geo)
		return;

	const unsigned int knC = m_geo->GetKnotsCount();
	float pC[3];
	m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
	if (knC!=0)
	{
		m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->DrawSplineFrame(m_geo);
		m_app->GetViewPort()->GetPainter()->SetCurColor(1.0,0.0,0.0);
		m_app->GetViewPort()->GetPainter()->DrawSpline(m_geo);
	}
}


void  SplineCommand::OnEnter()
{
	ASSERT(m_get_point_panel);

	SWITCH_RESOURCE
	m_get_point_panel->GetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);

	const unsigned int knC = m_geo->GetKnotsCount();
	if (knC==0)
	{
		m_geo->AddKnot(m_cur_pnt,0);
		m_first_point = m_cur_pnt;
		m_last_point = m_cur_pnt;
		if (m_existing_points)
			m_existing_points->AddPoint(m_first_point);
		m_geo->AddKnot(m_cur_pnt,1);
	}
	else
	{
		if (sgSpaceMath::PointsDistance(m_cur_pnt,m_last_point)<0.001)
		{
			m_message.LoadString(IDS_ERROR_SPL_KNOT_AS_PREV);
			m_app->PutMessage(IApplicationInterface::MT_ERROR,
				m_message);
			return;
		}
		m_geo->AddKnot(m_cur_pnt,knC);
		m_last_point = m_cur_pnt;
		if (m_existing_points)
			m_existing_points->AddPoint(m_cur_pnt);
		if (knC>3)
		{
			int snSz = m_app->GetViewPort()->GetSnapSize();
			
			if (sgSpaceMath::PointsDistance(m_first_point,m_cur_pnt)<0.0001)
			{
				CString sss;
				sss.LoadString(IDS_CLOSE_SPLINE);
				if (AfxMessageBox(sss,MB_YESNO)==IDYES)
				{
					if (!m_geo->IsClosed())
					{
						m_geo->DeleteKnot(m_geo->GetKnotsCount()-1);
						m_geo->DeleteKnot(m_geo->GetKnotsCount()-1);
						if (!m_geo->Close())
						{
							m_message.LoadString(IDS_CANNT_CLOSE_SPLINE);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,
								m_message);
							m_geo->AddKnot(m_cur_pnt, m_geo->GetKnotsCount());
						}
						else
						{
							m_geo->DeleteKnot(m_geo->GetKnotsCount()-3);
						}
					}
					sgCSpline* spl = sgCreateSpline(*m_geo);
					if (!spl)
					{
						SG_SPLINE::Delete(m_geo);
						m_geo = SG_SPLINE::Create();
						m_message.LoadString(IDS_CREARE_SPL_ERROR);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
						return;
					}
					CString nm;
					nm.LoadString(IDS_TOOLTIP_FOURTH);
					CString nmInd;
					nmInd.Format("%i",spline_name_index);
					nm+=nmInd;
					spl->SetName(nm);
					sgGetScene()->StartUndoGroup();
					sgGetScene()->AttachObject(spl);
					m_app->ApplyAttributes(spl);
					sgGetScene()->EndUndoGroup();
					spline_name_index++;
					m_app->GetViewPort()->InvalidateViewPort();
					SG_SPLINE::Delete(m_geo);
					m_geo = SG_SPLINE::Create();
					if (m_existing_points)
						m_existing_points->RemoveAllPoints();
				}
			}
		}
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

unsigned int  SplineCommand::GetItemsCount()
{
	return 1;
}

void         SplineCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	SWITCH_RESOURCE
	if (itemID==0) 
	{
		itSrt.LoadString(IDS_END_OPER);
	}
	else
	{
		ASSERT(0);
	}
}

void     SplineCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	enbl = true;
	checked = false;
}

HBITMAP   SplineCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         SplineCommand::Run(unsigned int itemID)
{
	SWITCH_RESOURCE
	if (itemID==0)
	{
		const unsigned int knC = m_geo->GetKnotsCount();
		if (knC<4)
		{
			m_message.LoadString(IDS_WANT_MORE_POINTS);
			m_app->PutMessage(IApplicationInterface::MT_ERROR,
				m_message);
			return;
		}
		m_geo->DeleteKnot(knC-1);
		sgCSpline* spl = sgCreateSpline(*m_geo);
		if (!spl)
		{
			SG_SPLINE::Delete(m_geo);
			m_geo = SG_SPLINE::Create();
			m_message.LoadString(IDS_CREARE_SPL_ERROR);
			m_app->PutMessage(IApplicationInterface::MT_ERROR,
				m_message);
			return;
		}
		CString nm;
		nm.LoadString(IDS_TOOLTIP_FOURTH);
		CString nmInd;
		nmInd.Format("%i",spline_name_index);
		nm+=nmInd;
		spl->SetName(nm);
		sgGetScene()->StartUndoGroup();
		sgGetScene()->AttachObject(spl);
		m_app->ApplyAttributes(spl);
		sgGetScene()->EndUndoGroup();
		spline_name_index++;
		m_app->GetViewPort()->InvalidateViewPort();
		SG_SPLINE::Delete(m_geo);
		m_geo = SG_SPLINE::Create();
		if (m_existing_points)
			m_existing_points->RemoveAllPoints();
	}
	else
	{
		ASSERT(0);
	}
}
