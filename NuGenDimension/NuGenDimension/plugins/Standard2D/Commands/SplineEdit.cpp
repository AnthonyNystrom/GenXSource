#include "stdafx.h"

#include "SplineEdit.h"

#include "..//resource.h"
#include <math.h>
#include <float.h>


SplineEditCommand::SplineEditCommand(sgCSpline* es, IApplicationInterface* appI):
						m_editable_spline(es)
						, m_clone_spline(NULL)
						, m_geo(NULL)
						, m_app(appI)
						,m_sel_point_panel(NULL)
						, m_get_point_panel(NULL)
						, m_choise_pnt(-1)
						, m_step(0)
						, m_was_started(false)
{
	ASSERT(es);
	ASSERT(m_app);
	
	SWITCH_RESOURCE
	m_bitmap = new CBitmap;
	m_bitmap->LoadBitmap(IDB_SPL_EDIT);
}

SplineEditCommand::~SplineEditCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
	if (m_bitmap)
	{
		if (m_bitmap->m_hObject)
			m_bitmap->DeleteObject();
		delete m_bitmap;
	}
	if (m_clone_spline)
		sgDeleteObject(m_clone_spline);
	/*if (m_geo)
		SG_SPLINE::DeleteSplineGeo(m_geo);*/
}

void     SplineEditCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
											   void* params) 
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=1);

		m_step = (unsigned int)newActiveDlg;
		if (newActiveDlg==0)
		{
			SWITCH_RESOURCE
			m_app->GetCommandPanel()->EnableRadio(1,false);
			m_message.LoadString(IDS_CHOISE_SPLINE_PNT);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		}
		m_app->GetViewPort()->InvalidateViewPort();
		return;
	}
	if (mes==ICommander::CM_UPDATE_COMMAND_PANEL)
	{
		ASSERT(params==NULL);
		if (m_step==0)
		{
			m_choise_pnt = m_sel_point_panel->GetCurrentPoint();
			m_app->GetViewPort()->InvalidateViewPort();
		}
	}
}

bool    SplineEditCommand::PreTranslateMessage(MSG* pMsg)
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
			switch(m_step) 
			{
			case 0:
				if (m_sel_point_panel)
				{
					m_sel_point_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
				}
				break;
			case 1:
				if (m_get_point_panel)
				{
					m_get_point_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
				}
				break;
			default:
				break;
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
				case WM_LBUTTONUP:
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

void  SplineEditCommand::Start()	
{
	ASSERT(m_editable_spline);

	SWITCH_RESOURCE
	CString lab;
	lab.LoadString(IDS_REDACTION);
	m_app->StartCommander(lab);

	if (m_clone_spline)
		sgDeleteObject(m_clone_spline);

	m_clone_spline = reinterpret_cast<sgCSpline*>(m_editable_spline->Clone());	
	m_geo = const_cast<SG_SPLINE*>(m_clone_spline->GetGeometry());
	
	NewPanels();
	m_app->GetViewPort()->SetEditableObject(m_editable_spline);
	m_was_started = true;
}

void   SplineEditCommand::NewPanels()
{
	SWITCH_RESOURCE
	
	CString lab;

	lab.LoadString(IDS_CHOISE_SPLINE_PNT);
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_sel_point_panel = 
		reinterpret_cast<ISelectPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::SELECT_POINT_DLG,
		lab,true));
	if (m_geo)
	{
		int ssz = (m_geo->IsClosed())?(m_geo->GetKnotsCount()-1):(m_geo->GetKnotsCount());
		for (int i=0;i<ssz;i++)
			m_sel_point_panel->AddPoint(m_geo->GetKnots()[i].x,
										m_geo->GetKnots()[i].y,
										m_geo->GetKnots()[i].z);
	}
	
	lab.LoadString(IDS_NEW_COORDINATES);

	m_get_point_panel = 
		reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
		lab,true));
	
	m_app->GetCommandPanel()->SetActiveRadio(0);
	m_step=0;
	m_choise_pnt =0;
}

void  SplineEditCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;

	SWITCH_RESOURCE
	if (m_step==0)
	{
		ASSERT(m_sel_point_panel);
		ASSERT(m_editable_spline);
		ASSERT(m_geo);
		double    winX1,winY1,winZ1;
		double    minDist = FLT_MAX;
		m_choise_pnt = -1;
		for (int i=0;i<m_geo->GetKnotsCount();i++)
		{
			m_app->GetViewPort()->ProjectWorldPoint(m_geo->GetKnots()[i],winX1,winY1,winZ1);
			double d1;
			d1 = sqrt((winX1-pX)*(winX1-pX)+(winY1-pY)*(winY1-pY));
			if (d1<minDist)
			{
				minDist = d1;
				m_choise_pnt = i;
			}
		}
		if (m_choise_pnt>=0)
		{
			if (m_sel_point_panel)
				m_sel_point_panel->SetCurrentPoint(m_choise_pnt);
		}
	}
	else
	{
		if (m_choise_pnt<0)
		{
			ASSERT(0);
			return;
		}
			ASSERT(m_get_point_panel);
			IViewPort::GET_SNAP_IN in_arg;
			in_arg.scrX = pX;
			in_arg.scrY = pY;
			in_arg.snapType = SNAP_SYSTEM;
			in_arg.XFix = m_get_point_panel->IsXFixed();
			in_arg.YFix = m_get_point_panel->IsYFixed();
			in_arg.ZFix = m_get_point_panel->IsZFixed();
			double tmpFl[3];
			m_get_point_panel->GetPoint(tmpFl[0],tmpFl[1],tmpFl[2]);
			in_arg.FixPoint.x = tmpFl[0];
			in_arg.FixPoint.y = tmpFl[1];
			in_arg.FixPoint.z = tmpFl[2];

			IViewPort::GET_SNAP_OUT out_arg;
			m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
			m_cur_pnt = out_arg.result_point;
			m_get_point_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
			m_geo->MoveKnot(m_choise_pnt,m_cur_pnt);
	}
	m_app->GetViewPort()->InvalidateViewPort();
		
}

void  SplineEditCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;

	SWITCH_RESOURCE
		
	if (m_step==0)
	{
		if (m_choise_pnt>=0)
		{
			m_step++;
			m_app->GetCommandPanel()->SetActiveRadio(m_step);
		}
		else
		{
			ASSERT(0);
		}
	}
	else
	{
		if (m_choise_pnt<0)
		{
			ASSERT(0);
			return;
		}
			CString mes;

			if ((m_choise_pnt>0 && sgSpaceMath::PointsDistance((m_geo->GetKnots()[m_choise_pnt]),
											(m_geo->GetKnots()[m_choise_pnt-1]))<0.001) ||
				(m_choise_pnt<m_geo->GetKnotsCount()-1 && sgSpaceMath::PointsDistance((m_geo->GetKnots()[m_choise_pnt]),
											(m_geo->GetKnots()[m_choise_pnt+1]))<0.001))
			{
				m_message.LoadString(IDS_ERROR_SPL_KNOT_AS_PREV);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
				sgCSpline* spl = sgCreateSpline(*m_geo);
				if (!spl)
				{
					if (m_clone_spline)
					{
						sgDeleteObject(m_clone_spline);
						m_clone_spline = NULL;
					}
					//m_geo = SG_SPLINE::CreateSplineGeo();
					m_message.LoadString(IDS_CREARE_SPL_ERROR);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					m_app->GetViewPort()->InvalidateViewPort();
					m_app->StopCommander();
					return;
				}
				sgGetScene()->StartUndoGroup();
				sgGetScene()->DetachObject(m_editable_spline);
				sgGetScene()->AttachObject(spl);
				sgGetScene()->EndUndoGroup();
				m_app->CopyAttributes(*spl,*m_editable_spline);
				m_app->GetViewPort()->InvalidateViewPort();
				m_app->StopCommander();
	}
			
}


void  SplineEditCommand::Draw()
{

	if (!m_was_started)
		return;

	if (!m_geo)
		return;

	float pC[3];
	if (m_choise_pnt>=0)
	{
		m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->DrawPoint(m_geo->GetKnots()[m_choise_pnt]);
	}

	m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->DrawSplineFrame(m_geo);
	m_app->GetViewPort()->GetPainter()->SetCurColor(1.0,0.0,0.0);
	m_app->GetViewPort()->GetPainter()->DrawSpline(m_geo);
}


void  SplineEditCommand::OnEnter()
{
	if (!m_was_started)
		return;
	SWITCH_RESOURCE

	if (m_step==0)
	{
		if (m_choise_pnt>=0)
		{
			m_step++;
			m_app->GetCommandPanel()->SetActiveRadio(m_step);
		}
		else
		{
			ASSERT(0);
		}
	}
	else
	{
				ASSERT(m_get_point_panel);

				m_get_point_panel->GetPoint(m_cur_pnt.x,m_cur_pnt.y,	m_cur_pnt.z);

				if (m_choise_pnt<0)
				{
					ASSERT(0);
					return;
				}

				m_geo->MoveKnot(m_choise_pnt,m_cur_pnt);
				CString mes;

				if ((m_choise_pnt>0 && sgSpaceMath::PointsDistance((m_geo->GetKnots()[m_choise_pnt]),
					(m_geo->GetKnots()[m_choise_pnt-1]))<0.001) ||
					(m_choise_pnt<m_geo->GetKnotsCount()-1 && sgSpaceMath::PointsDistance((m_geo->GetKnots()[m_choise_pnt]),
					(m_geo->GetKnots()[m_choise_pnt+1]))<0.001))
				{
					m_message.LoadString(IDS_ERROR_SPL_KNOT_AS_PREV);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}

				sgCSpline* spl = sgCreateSpline(*m_geo);
				if (!spl)
				{
					if (m_clone_spline)
					{
						sgDeleteObject(m_clone_spline);
						m_clone_spline = NULL;
					}
					//m_geo = SG_SPLINE::CreateSplineGeo();
					m_message.LoadString(IDS_CREARE_SPL_ERROR);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					m_app->GetViewPort()->InvalidateViewPort();
					m_app->StopCommander();
					return;
				}
				sgGetScene()->StartUndoGroup();
				sgGetScene()->DetachObject(m_editable_spline);
				sgGetScene()->AttachObject(spl);
				sgGetScene()->EndUndoGroup();
				m_app->CopyAttributes(*spl,*m_editable_spline);
				m_app->GetViewPort()->InvalidateViewPort();
				m_app->StopCommander();
	}
}


unsigned int  SplineEditCommand::GetItemsCount()
{
	if (!m_was_started)
		return 1;
	else
		return 0;
}

void         SplineEditCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	if (m_was_started)
	{
		ASSERT(0);
		return;
	}

	SWITCH_RESOURCE
	if(itemID==0) 
	{
		itSrt.LoadString(IDS_MOVE_SPLINE_KNOT);
	}
	else
		ASSERT(0);
}


void     SplineEditCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	if (m_was_started)
	{
		ASSERT(0);
		return;
	}

	enbl = true;
	checked = false;

	
}

HBITMAP   SplineEditCommand::GetItemBitmap(unsigned int)
{
	/*if (!m_was_started)
		return (HBITMAP)m_bitmap;
	else*/
		return NULL;
}

void         SplineEditCommand::Run(unsigned int itemID)
{
	if (!m_was_started)
	{
		if (itemID==0 || itemID==1)
		{
			//m_scenario = itemID;
			Start();
		}
		else
		{
			ASSERT(0);
		}
	}
	/*else
		m_scenario = itemID;*/
}

