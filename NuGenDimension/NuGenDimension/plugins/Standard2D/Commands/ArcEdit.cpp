#include "stdafx.h"

#include "ArcEdit.h"

#include "..//resource.h"

CArcEditCommand::CArcEditCommand(sgCArc* edA, IApplicationInterface* appI):
							m_editable_arc(edA)
							, m_app(appI)
							, m_get_point_panel(NULL)
							, m_was_started(false)
							, m_invert(false)
							, m_exist_arc_data(false)
{
	ASSERT(edA);
	ASSERT(m_app);
}

CArcEditCommand::~CArcEditCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
}


bool    CArcEditCommand::PreTranslateMessage(MSG* pMsg)
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


void  CArcEditCommand::Start()	
{
	ASSERT(m_editable_arc);

	SWITCH_RESOURCE
		CString lab;
	lab.LoadString(IDS_REDACTION);
	m_app->StartCommander(lab);


	m_scenario = 0;
	NewScenario();
	
	m_app->GetViewPort()->SetEditableObject(m_editable_arc);
	m_was_started = true;
}

void  CArcEditCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;

	ASSERT(m_editable_arc);
	SG_ARC tmp_arc = *m_editable_arc->GetGeometry();
	switch(m_scenario) {
	case 0:
		{
			/*ASSERT(m_get_point_panel);
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
			m_exist_arc_data = sgCArc::CreateArcGeoFrom_c_b_e(m_cur_pnt,
										tmp_arc.begin,
										tmp_arc.end,
										m_invert,
										m_tmp_arc);
			
		}
		break;
	case 1:
		{*/
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
			m_exist_arc_data = m_tmp_arc.FromThreePoints(	tmp_arc.begin,
									tmp_arc.end,
									m_cur_pnt,
									m_invert);

		}
		break;
	/*case 2:
		{
			IGetPointPanel*  gpd = m_panel->GetCurrentGetPointPanel();
			ASSERT(gpd);
			IViewPort::GET_SNAP_IN in_arg;
			in_arg.scrX = pX;
			in_arg.scrY = pY;
			in_arg.snapType = SNAP_SYSTEM;
			in_arg.XFix = gpd->IsXFixed();in_arg.YFix = gpd->IsYFixed();in_arg.ZFix = gpd->IsZFixed();
			double tmpFl[3];
			gpd->GetPoint(tmpFl[0],tmpFl[1],tmpFl[2]);
			in_arg.FixPoint.x = tmpFl[0];
			in_arg.FixPoint.y = tmpFl[1];
			in_arg.FixPoint.z = tmpFl[2];

			IViewPort::GET_SNAP_OUT out_arg;
			m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
			m_cur_pnt = out_arg.result_point;
			gpd->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
			m_exist_arc_data = sgCArc::CreateArcGeoFrom_c_b_e(	tmp_arc.center,
											m_cur_pnt,
											tmp_arc.end,
											m_invert,
											m_tmp_arc);

		}
		break;
	case 3:
		{
			IGetPointPanel*  gpd = m_panel->GetCurrentGetPointPanel();
			ASSERT(gpd);
			IViewPort::GET_SNAP_IN in_arg;
			in_arg.scrX = pX;
			in_arg.scrY = pY;
			in_arg.snapType = SNAP_SYSTEM;
			in_arg.XFix = gpd->IsXFixed();in_arg.YFix = gpd->IsYFixed();in_arg.ZFix = gpd->IsZFixed();
			double tmpFl[3];
			gpd->GetPoint(tmpFl[0],tmpFl[1],tmpFl[2]);
			in_arg.FixPoint.x = tmpFl[0];
			in_arg.FixPoint.y = tmpFl[1];
			in_arg.FixPoint.z = tmpFl[2];

			IViewPort::GET_SNAP_OUT out_arg;
			m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
			m_cur_pnt = out_arg.result_point;
			gpd->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
			m_exist_arc_data = sgCArc::CreateArcGeoFrom_c_b_e(tmp_arc.center,
											tmp_arc.begin,
											m_cur_pnt,
											m_invert,
											m_tmp_arc);

		}
		break;*/
	default:
		ASSERT(0);
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

void  CArcEditCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;

	SWITCH_RESOURCE

	CString mes;
	if (!m_exist_arc_data)
	{
		mes.LoadString(IDS_ERROR_INFIN_RAD);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,
			mes);
		return;
	}
	
	sgCArc* ar = sgCreateArc(m_tmp_arc);
	if (!ar)
		return;
	sgGetScene()->StartUndoGroup();
	sgGetScene()->DetachObject(m_editable_arc);
	sgGetScene()->AttachObject(ar);
	sgGetScene()->EndUndoGroup();
	m_app->CopyAttributes(*ar,*m_editable_arc);
	m_app->GetViewPort()->InvalidateViewPort();
	m_app->StopCommander();
}

void  CArcEditCommand::Draw()
{
	if (!m_was_started)
		return;

	ASSERT(m_editable_arc);
	SG_ARC tmp_arc = *m_editable_arc->GetGeometry();
	switch(m_scenario) {
	case 0:
	/*case 1:
	case 2:
	case 3:*/
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);

			if (m_exist_arc_data)
			{
				m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->DrawArc(m_tmp_arc);
			}

		}
		break;
	default:
		ASSERT(0);
	}
}

void  CArcEditCommand::OnEnter()
{
	if (!m_was_started)
		return;

	SWITCH_RESOURCE
	ASSERT(m_editable_arc);
	m_tmp_arc = *m_editable_arc->GetGeometry();

	ASSERT(m_get_point_panel);
	m_get_point_panel->GetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
SG_ARC tmp_arc = *m_editable_arc->GetGeometry();
	switch(m_scenario) {
	case 0:
		{
			/*if (sgCArc::CreateArcGeoFrom_c_b_e(m_cur_pnt,
									tmp_arc.begin,
									tmp_arc.end,
									m_invert,
									m_tmp_arc))
				m_exist_arc_data = true;
			else
				m_exist_arc_data = false;
		}
		break;
	case 1:
		{*/
			if (m_tmp_arc.FromThreePoints(	tmp_arc.begin,
										tmp_arc.end,
										m_cur_pnt,
										m_invert))
				m_exist_arc_data = true;
			else
				m_exist_arc_data = false;
		}
		break;
	/*case 2:
		{
			if (sgCArc::CreateArcGeoFrom_c_b_e(	tmp_arc.center,
												m_cur_pnt,
												tmp_arc.end,
												m_invert,
												m_tmp_arc))
				m_exist_arc_data = true;
			else
				m_exist_arc_data = false;
		}
		break;
	case 3:
		{
			if (sgCArc::CreateArcGeoFrom_c_b_e(	tmp_arc.center,
											tmp_arc.begin,
											m_cur_pnt,
											m_invert,
											m_tmp_arc))
				m_exist_arc_data = true;
			else
				m_exist_arc_data = false;
		}
		break;*/
	default:
		ASSERT(0);
	}

	
	CString mes;
	if (!m_exist_arc_data)
	{
		mes.LoadString(IDS_ERROR_INFIN_RAD);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,
			mes);
		return;
	}

	sgCArc* ar = sgCreateArc(m_tmp_arc);
	if (!ar)
		return;
	
	sgGetScene()->StartUndoGroup();
	sgGetScene()->DetachObject(m_editable_arc);
	sgGetScene()->AttachObject(ar);
	sgGetScene()->EndUndoGroup();
	m_app->CopyAttributes(*ar,*m_editable_arc);

	m_app->GetViewPort()->InvalidateViewPort();
	m_app->StopCommander();
}

unsigned int  CArcEditCommand::GetItemsCount()
{
	/*if (!m_was_started)
		return 4;
	else
		return 5;*/
	return 1;
}

void         CArcEditCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	SWITCH_RESOURCE
		/*if (!m_was_started)
		{*/
			switch(itemID) {
				case 0:
				/*	itSrt.LoadString(IDS_CHANGE_CENTER_COORDS);
					break;
				case 1:*/
					itSrt.LoadString(IDS_CHANGE_ARC_POINT_COORD);
					break;
				/*case 2:
					itSrt.LoadString(IDS_CHANGE_COOR_1);
					break;
				case 3:
					itSrt.LoadString(IDS_CHANGE_COOR_2);
					break;*/
				default:
					ASSERT(0);
						}
		/*}
		else
		{

			switch(itemID)
			{
				case 0:
					itSrt.LoadString(IDS_INVERT);
					break;
				case 1:
					itSrt.LoadString(IDS_CHANGE_CENTER_COORDS);
					break;
				case 2:
					itSrt.LoadString(IDS_CHANGE_ARC_POINT_COORD);
					break;
				case 3:
					itSrt.LoadString(IDS_CHANGE_COOR_1);
					break;
				case 4:
					itSrt.LoadString(IDS_CHANGE_COOR_2);
					break;
				default:
					ASSERT(0);
			}
		}*/
}

void     CArcEditCommand::GetItemState(unsigned int itemID, 
													bool& enbl, bool& checked)
{
	enbl = true;
	if (!m_was_started)
	{
		checked = false;
	}
	else
	{
		/*checked = false;
		if (itemID==0 && m_invert)
			checked=true;
		else
			if (itemID==(m_scenario+1))*/
				checked = true;
	}

}

HBITMAP   CArcEditCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         CArcEditCommand::Run(unsigned int itemID)
{
	if (!m_was_started)
	{
		if (itemID==0 /*|| itemID==1 || itemID==2 || itemID==3*/)
		{
			m_scenario = itemID;
			Start();
		}
		else
		{
			ASSERT(0);
		}
	}
	/*else
	{
		if (itemID==0)
			m_invert=!m_invert;
		else
		{
			m_scenario = itemID-1;
			NewScenario();
		}
	}*/
}


void   CArcEditCommand::NewScenario()
{
	ASSERT(m_editable_arc);

	m_app->GetCommandPanel()->RemoveAllDialogs();
	CString lab;
SWITCH_RESOURCE
	switch(m_scenario) 
	{
	case 0:
		{
			/*lab.LoadString(IDS_NEW_COORDINATES);
			m_panel->AddPage(lab,IBasePanel::GET_POINT_DLG);
			m_panel->EnablePage(0,true);
			m_panel->SetActivePage(0);
			m_cur_pnt = m_editable_arc->GetGeometry()->center;
			IGetPointPanel* gpp = m_panel->GetCurrentGetPointPanel();
			gpp->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
		}
		break;
	case 1:
		{*/
			lab.LoadString(IDS_NEW_COORDINATES);
			m_get_point_panel = 
				reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
				AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
				lab,false));
			lab.LoadString(IDS_ARC_ENTER_NWE_POINT);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);

		}
		break;
	/*case 2:
		{
			lab.LoadString(IDS_NEW_COORDINATES);
			m_panel->AddPage(lab,IBasePanel::GET_POINT_DLG);
			m_panel->EnablePage(0,true);
			m_panel->SetActivePage(0);
			m_cur_pnt = m_editable_arc->GetGeometry()->begin;
			IGetPointPanel* gpp = m_panel->GetCurrentGetPointPanel();
			gpp->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
		}
		break;
	case 3:
		{
			lab.LoadString(IDS_NEW_COORDINATES);
			m_panel->AddPage(lab,IBasePanel::GET_POINT_DLG);
			m_panel->EnablePage(0,true);
			m_panel->SetActivePage(0);
			m_cur_pnt = m_editable_arc->GetGeometry()->end;
			IGetPointPanel* gpp = m_panel->GetCurrentGetPointPanel();
			gpp->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
		}
		break;*/
	default:
		ASSERT(0);
	}

	m_app->GetCommandPanel()->SetActiveRadio(0);

	m_exist_arc_data = false;

}

