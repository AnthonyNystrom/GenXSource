#include "stdafx.h"

#include "CircleEdit.h"

#include "..//resource.h"

#include <math.h>


CircleEditCommand::CircleEditCommand(sgCCircle* eC, IApplicationInterface* appI):
						m_editable_circle(eC)
						, m_app(appI)
						,m_get_center_panel(NULL)
						,m_get_normal_panel(NULL)
						,m_get_r_panel(NULL)
						, m_scenario(0)
						, m_was_started(false)
{
	ASSERT(eC);
	ASSERT(m_app);

	SWITCH_RESOURCE
	m_bitmaps = new CBitmap[3];
	m_bitmaps[0].LoadBitmap(IDB_CIRC_EDIT_CEN);
	m_bitmaps[1].LoadBitmap(IDB_CIRC_EDIT_RAD);
	m_bitmaps[2].LoadBitmap(IDB_CIRC_EDIT_NORM);
}

CircleEditCommand::~CircleEditCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	if (m_bitmaps)
	{
		for (int i=0;i<3;i++)
			if (m_bitmaps[i].m_hObject)
				m_bitmaps[i].DeleteObject();
		delete [] m_bitmaps;
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

void  CircleEditCommand::Start()	
{
	ASSERT(m_editable_circle);

	SWITCH_RESOURCE
	CString lab;
	lab.LoadString(IDS_REDACTION);
	m_app->StartCommander(lab);

	NewScenario();
	m_app->GetViewPort()->SetEditableObject(m_editable_circle);
	m_was_started = true;
}


bool    CircleEditCommand::PreTranslateMessage(MSG* pMsg)
{
	try {  //#try
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
			if (m_get_center_panel)
			{
				m_get_center_panel->GetWindow()->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
			}
			if (m_get_normal_panel)
			{
				m_get_normal_panel->GetWindow()->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
			}
			if (m_get_r_panel)
			{
				m_get_r_panel->GetWindow()->SendMessage(pMsg->message,
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



void  CircleEditCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;

	ASSERT(m_editable_circle);
	m_tmp_circ = *m_editable_circle->GetGeometry();
	switch(m_scenario) {
	case 0:
		{
			ASSERT(m_get_center_panel);
			IViewPort::GET_SNAP_IN in_arg;
			in_arg.scrX = pX;
			in_arg.scrY = pY;
			in_arg.snapType = SNAP_SYSTEM;
			in_arg.XFix = m_get_center_panel->IsXFixed();
			in_arg.YFix = m_get_center_panel->IsYFixed();
			in_arg.ZFix = m_get_center_panel->IsZFixed();
			double tmpFl[3];
			m_get_center_panel->GetPoint(tmpFl[0],tmpFl[1],tmpFl[2]);
			in_arg.FixPoint.x = tmpFl[0];
			in_arg.FixPoint.y = tmpFl[1];
			in_arg.FixPoint.z = tmpFl[2];

			IViewPort::GET_SNAP_OUT out_arg;
			m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
			m_cur_pnt = out_arg.result_point;
			m_get_center_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
			m_tmp_circ.center = m_cur_pnt;
		}
		break;
	case 1:
		{
			double plD;
			sgSpaceMath::PlaneFromNormalAndPoint(m_tmp_circ.center,m_tmp_circ.normal,plD);

			double rd;
			if (m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,m_tmp_circ.normal,plD,m_cur_pnt))
			{
				rd = m_app->ApplyPrecision(
					sgSpaceMath::PointsDistance(m_tmp_circ.center,m_cur_pnt)
					);
			}
			else
				rd = 1.0;
			
			
			ASSERT(m_get_r_panel);

			m_get_r_panel->SetNumber(rd);
			m_tmp_circ.radius = rd;
		}
		break;
	case 2:
		{
			ASSERT(m_get_normal_panel);

			double tmpFl[3];
			if (m_get_normal_panel->GetVector(tmpFl[0],tmpFl[1],tmpFl[2])==
				IGetVectorPanel::USER_VECTOR)
			{
				IViewPort::GET_SNAP_IN in_arg;
				in_arg.scrX = pX;
				in_arg.scrY = pY;
				in_arg.snapType = SNAP_SYSTEM;
				in_arg.XFix = m_get_normal_panel->IsXFixed();
				in_arg.YFix = m_get_normal_panel->IsYFixed();
				in_arg.ZFix = m_get_normal_panel->IsZFixed();
				in_arg.FixPoint.x = tmpFl[0];
				in_arg.FixPoint.y = tmpFl[1];
				in_arg.FixPoint.z = tmpFl[2];
				IViewPort::GET_SNAP_OUT out_arg;
				m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
				m_cur_pnt = out_arg.result_point;
				m_dir.x = m_cur_pnt.x - m_tmp_circ.center.x;
				m_dir.y = m_cur_pnt.y - m_tmp_circ.center.y;
				m_dir.z = m_cur_pnt.z - m_tmp_circ.center.z;
				sgSpaceMath::NormalVector(m_dir);
				m_get_normal_panel->SetVector(IGetVectorPanel::USER_VECTOR,
					m_dir.x,m_dir.y,m_dir.z);
				m_tmp_circ.normal = m_dir;
			}
			else
			{
				m_cur_pnt.x = m_tmp_circ.center.x+tmpFl[0];
				m_cur_pnt.y = m_tmp_circ.center.y+tmpFl[1];
				m_cur_pnt.z = m_tmp_circ.center.z+tmpFl[2];
				m_tmp_circ.normal.x = tmpFl[0];
				m_tmp_circ.normal.y = tmpFl[1];
				m_tmp_circ.normal.z = tmpFl[2];
			}
		}
		break;
	default:
		ASSERT(0);
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

void  CircleEditCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;

	SWITCH_RESOURCE

	CString mes;
	if (fabs(m_tmp_circ.radius)<0.00001)
	{
		mes.LoadString(IDS_ERROR_ZERO_RADIUS);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,
			mes);
		return;
	}
	if (!sgSpaceMath::NormalVector(m_tmp_circ.normal))
	{
		mes.LoadString(IDS_ERROR_ZERO_VECTOR);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,
			mes);
		return;
	}
	sgCCircle* cr = sgCreateCircle(m_tmp_circ);
	sgGetScene()->StartUndoGroup();
	sgGetScene()->DetachObject(m_editable_circle);
	sgGetScene()->AttachObject(cr);
	sgGetScene()->EndUndoGroup();
	m_app->CopyAttributes(*cr,*m_editable_circle);
	m_app->GetViewPort()->InvalidateViewPort();
	m_app->StopCommander();
}


void  CircleEditCommand::Draw()
{
	if (!m_was_started)
		return;

	float pC[3];
	ASSERT(m_editable_circle);
	switch(m_scenario) {
	case 0:
	case 1:
		{	
			m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawCircle(m_tmp_circ);
		}
		break;
	case 2:
		{
			m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_tmp_circ.center);
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			SG_LINE ln;
			ln.p1 = m_tmp_circ.center;
			ln.p2 = m_cur_pnt;
			m_app->GetViewPort()->GetPainter()->DrawLine(ln);
			m_app->GetViewPort()->GetPainter()->DrawCircle(m_tmp_circ);
		}
		break;
	default:
		ASSERT(0);
	}
}

void  CircleEditCommand::OnEnter()
{
	if (!m_was_started)
		return;

	SWITCH_RESOURCE
	ASSERT(m_editable_circle);
	m_tmp_circ = *m_editable_circle->GetGeometry();
	CString mes;
	switch(m_scenario) {
	case 0:
		{
			ASSERT(m_get_center_panel);
			m_get_center_panel->GetPoint(m_tmp_circ.center.x,
				m_tmp_circ.center.y,
				m_tmp_circ.center.z);
		}
		break;
	case 1:
		{
			ASSERT(m_get_r_panel);
			m_tmp_circ.radius = m_get_r_panel->GetNumber();
			if (fabs(m_tmp_circ.radius)<0.00001)
			{
				mes.LoadString(IDS_ERROR_ZERO_RADIUS);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					mes);
				return;
			}
		}
		break;
	case 2:
		{
			ASSERT(m_get_normal_panel);
			m_get_normal_panel->GetVector(m_tmp_circ.normal.x,
				m_tmp_circ.normal.y,
				m_tmp_circ.normal.z);
			if (!sgSpaceMath::NormalVector(m_tmp_circ.normal))
			{
				
				mes.LoadString(IDS_ERROR_ZERO_VECTOR);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					mes);
				return;
			}
		}
		break;
	default:
		ASSERT(0);
	}
	

	sgCCircle* cr = sgCreateCircle(m_tmp_circ);
	sgGetScene()->StartUndoGroup();
	sgGetScene()->DetachObject(m_editable_circle);
	sgGetScene()->AttachObject(cr);
	sgGetScene()->EndUndoGroup();
	m_app->CopyAttributes(*cr,*m_editable_circle);
	m_app->GetViewPort()->InvalidateViewPort();
	m_app->StopCommander();
}


unsigned int  CircleEditCommand::GetItemsCount()
{
	return 3;
}

void         CircleEditCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	SWITCH_RESOURCE
		switch(itemID) {
	case 0:
		itSrt.LoadString(IDS_CHANGE_CENTER_COORDS);
		break;
	case 1:
		itSrt.LoadString(IDS_CHANGE_RAD);
		break;
	case 2:
		itSrt.LoadString(IDS_CHANGE_NORM);
		break;
	default:
		ASSERT(0);
		}
}


void     CircleEditCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	enbl = true;
	if (!m_was_started)
	{
		checked = false;
	}
	else
	{
		checked = false;
		if (itemID==m_scenario)
			checked = true;
	}



}

HBITMAP   CircleEditCommand::GetItemBitmap(unsigned int itemID)
{
	return NULL;//m_bitmaps[itemID];
}

void         CircleEditCommand::Run(unsigned int itemID)
{
	if (!m_was_started)
	{
		if (itemID==0 || itemID==1 || itemID==2)
		{
			m_scenario = itemID;
			Start();
		}
		else
		{
			ASSERT(0);
		}
	}
	else
	{
		m_scenario = itemID;
		NewScenario();
	}
}

void   CircleEditCommand::NewScenario()
{
	ASSERT(m_editable_circle);

	m_app->GetCommandPanel()->RemoveAllDialogs();

	m_get_center_panel=NULL;
	m_get_normal_panel=NULL;
	m_get_r_panel = NULL;

	CString lab;
	SWITCH_RESOURCE
	switch(m_scenario) 
	{
	case 0:
		{
			lab.LoadString(IDS_NEW_COORDINATES);
			m_get_center_panel = 
				reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
				AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
				lab,true));
			m_cur_pnt = m_editable_circle->GetGeometry()->center;
			m_get_center_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
			lab.LoadString(IDS_CIR_NEW_CENTER);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);
		}
		break;
	case 1:
		{
			lab.LoadString(IDS_NEW_RAD);
			
			m_get_r_panel =
				reinterpret_cast<IGetNumberPanel*>(m_app->GetCommandPanel()->
				AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,
				lab,true));
			m_get_r_panel->SetNumber(m_editable_circle->GetGeometry()->radius);
			lab.LoadString(IDS_CIR_NEW_RAD);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);
		}
		break;
	case 2:
		{
			lab.LoadString(IDS_NEW_NORMAL);
			
			m_dir = m_editable_circle->GetGeometry()->normal;
			m_get_normal_panel = 
				reinterpret_cast<IGetVectorPanel*>(m_app->GetCommandPanel()->
				AddDialog(IBaseInterfaceOfGetDialogs::GET_VECTOR_DLG,
				lab,true));
			m_get_normal_panel->SetVector(IGetVectorPanel::USER_VECTOR,
				m_dir.x, m_dir.y, m_dir.z);
			lab.LoadString(IDS_CIR_NEW_NORM);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);
		}
		break;
	default:
		ASSERT(0);
	}
	m_app->GetCommandPanel()->SetActiveRadio(0);
	
}
