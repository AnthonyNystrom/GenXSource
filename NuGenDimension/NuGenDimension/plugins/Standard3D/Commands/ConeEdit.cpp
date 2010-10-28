#include "stdafx.h"

#include "ConeEdit.h"

#include "..//resource.h"

#include <math.h>

CConeEditCommand::CConeEditCommand(sgCCone* edC,IApplicationInterface* appI):
					m_editable_cone(edC)
					, m_app(appI)
					, m_was_started(false)
					, m_rad(0.0)
					, m_height(0.0)
					, m_scenar(-1)
					, m_matr(NULL)
					,m_other_params_dlg(NULL)
					, m_r_panel(NULL)
{
	ASSERT(edC);
	ASSERT(m_app);
	m_base_pnt.x = m_base_pnt.y = m_base_pnt.z = 0.0;
	m_dir.x = m_dir.y = 0.0;
	m_dir.z = 1.0;
}

CConeEditCommand::~CConeEditCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
	if(m_matr)
		delete m_matr;
}



bool    CConeEditCommand::PreTranslateMessage(MSG* pMsg)
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
			if (m_r_panel)
			{
				m_r_panel->GetWindow()->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
			}
			if (m_other_params_dlg)
			{
				m_other_params_dlg->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
				return false;
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

void  CConeEditCommand::Start()	
{
	ASSERT(m_editable_cone);

	m_base_pnt.x = m_base_pnt.y = m_base_pnt.z = 0.0;
	m_dir.x = m_dir.y = 0.0;
	m_dir.z = 1.0;

	if(m_matr)
		delete m_matr;

	m_matr = new sgCMatrix(m_editable_cone->GetWorldMatrixData());

	m_matr->Transparent();

	m_base_pnt.x = m_base_pnt.y = m_base_pnt.z = 0.0;
	m_dir.x = m_dir.y = 0.0;
	m_dir.z = 1.0;

	m_matr->ApplyMatrixToPoint(m_base_pnt);
	SG_POINT ppp;
	ppp.x = ppp.y = ppp.z = 0.0;
	m_matr->ApplyMatrixToVector(ppp,m_dir);

	m_editable_cone->GetGeometry(m_cone_geo);
	SWITCH_RESOURCE

	m_app->GetCommandPanel()->RemoveAllDialogs();

	CString lab;
	lab.LoadString(IDS_REDACTION);
	m_app->StartCommander(lab);

	switch(m_scenar) 
	{
	case 0:
		lab.LoadString(IDS_NEW_RADIUS);
		m_r_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
			GetCommandPanel()->
			AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,
			lab,false));
		m_r_panel->SetNumber(m_cone_geo.Radius1);
		lab.LoadString(IDS_ENTER_NEW_RAD);
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);
		break;
	case 1:
		lab.LoadString(IDS_NEW_RADIUS);
		m_r_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
			GetCommandPanel()->
			AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,
			lab,false));
		m_r_panel->SetNumber(m_cone_geo.Radius2);
		lab.LoadString(IDS_ENTER_NEW_RAD);
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);
		break;
		/*case :
		break;*/
	case 2:
		lab.LoadString(IDS_NEW_HEIGHT);
		m_r_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
			GetCommandPanel()->
			AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,
			lab,false));
		m_r_panel->SetNumber(m_cone_geo.Height);
		lab.LoadString(IDS_ENTER_NEW_HEIGHT);
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);
		break;
		/*case :
		break;*/
	default:
		break;
	}

	m_app->GetCommandPanel()->SetActiveRadio(0);
	m_app->GetViewPort()->SetEditableObject(m_editable_cone);
	m_was_started = true;
}

void  CConeEditCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;
	switch(m_scenar) {
	case 0:
	case 1:
		{
			ASSERT(m_r_panel);
			IViewPort::GET_SNAP_IN in_arg;
			in_arg.scrX = pX;
			in_arg.scrY = pY;
			in_arg.snapType = SNAP_SYSTEM;
			in_arg.XFix = in_arg.YFix = in_arg.ZFix = false;
			in_arg.FixPoint.x = in_arg.FixPoint.y = in_arg.FixPoint.z = 0.0;
			IViewPort::GET_SNAP_OUT out_arg;
			m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
			SG_POINT cur_pnt = out_arg.result_point;

			SG_POINT projection;
			m_rad = sgSpaceMath::ProjectPointToLineAndGetDist(m_base_pnt,
				m_dir,
				cur_pnt,
				projection);
			m_rad = m_app->ApplyPrecision(m_rad);
			ASSERT(m_r_panel);
			m_r_panel->SetNumber(m_rad);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	case 2:
		{
			ASSERT(m_r_panel);
			SG_POINT projection;
			m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,m_base_pnt,
				m_dir,
				projection);
			m_height = sgSpaceMath::PointsDistance(m_base_pnt,projection);
			m_height = m_app->ApplyPrecision(m_height);
			ASSERT(m_r_panel);
			m_r_panel->SetNumber(m_height);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	default:
		break;
	}
}

void  CConeEditCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;
	SWITCH_RESOURCE
		ASSERT(m_editable_cone);

	switch(m_scenar) {
	case 0:
	case 1:
		{
			ASSERT(m_r_panel);
			bool condis = false;
			if (m_scenar==0)
				condis = (fabs(m_rad)<0.0001 && fabs(m_cone_geo.Radius2)<0.0001);
			else
				condis = (fabs(m_rad)<0.0001 && fabs(m_cone_geo.Radius1)<0.0001);
			if (condis)
			{
				m_message.LoadString(IDS_ERROR_ZERO_BOTH_RADS);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			sgCCone* cone = NULL;
			if (m_scenar==0)
				cone = sgCreateCone(fabs(m_rad),m_cone_geo.Radius2,fabs(m_cone_geo.Height),
								m_cone_geo.MeridiansCount);
			else
				cone = sgCreateCone(m_cone_geo.Radius1,fabs(m_rad),fabs(m_cone_geo.Height),
								m_cone_geo.MeridiansCount);
			if (!cone)
				return;
			cone->InitTempMatrix()->Multiply(*m_matr);
			cone->ApplyTempMatrix();
			cone->DestroyTempMatrix();

			sgGetScene()->StartUndoGroup();
			sgGetScene()->DetachObject(m_editable_cone);
			sgGetScene()->AttachObject(cone);
			sgGetScene()->EndUndoGroup();
			m_app->CopyAttributes(*cone,*m_editable_cone);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	case 2:
		{
			ASSERT(m_r_panel);
			if (fabs(m_height)<0.0001)
			{
				m_message.LoadString(IDS_ERROR_ZERO_LENGTH);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			else
				if (m_height<-0.0001)
				{
					m_message.LoadString(IDS_ERROR_HEIGHT_MUST_BE_POSIT);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
			sgCCone* cone = NULL;
			cone = sgCreateCone(m_cone_geo.Radius1,m_cone_geo.Radius2,
				fabs(m_height),
				m_cone_geo.MeridiansCount);
			if (!cone)
				return;
			
			cone->InitTempMatrix()->Multiply(*m_matr);
			cone->ApplyTempMatrix();
			cone->DestroyTempMatrix();

			sgGetScene()->StartUndoGroup();
			sgGetScene()->DetachObject(m_editable_cone);
			sgGetScene()->AttachObject(cone);
			sgGetScene()->EndUndoGroup();
			m_app->CopyAttributes(*cone,*m_editable_cone);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	default:
		break;
	}

	m_app->StopCommander();
}


void  CConeEditCommand::Draw()
{
	if (!m_was_started)
		return;

	switch(m_scenar) {
	case 0:
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(m_matr);
			m_app->GetViewPort()->GetPainter()->DrawCone(m_rad,m_cone_geo.Radius2,
				m_cone_geo.Height);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);
		}
		break;
	case 1:
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(m_matr);
			m_app->GetViewPort()->GetPainter()->DrawCone(m_cone_geo.Radius1,m_rad,
				m_cone_geo.Height);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);
		}
		break;
	case 2:
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(m_matr);
			m_app->GetViewPort()->GetPainter()->DrawCone(m_cone_geo.Radius1,m_cone_geo.Radius2,
				m_height);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);
		}
		break;
	default:
		break;
	}
}


void  CConeEditCommand::OnEnter()
{
	if (!m_was_started)
		return;
	SWITCH_RESOURCE
		ASSERT(m_editable_cone);

	switch(m_scenar) {
	case 0:
	case 1:
		{
			ASSERT(m_r_panel);
			m_rad = m_r_panel->GetNumber();
			bool condis = false;
			if (m_scenar==0)
				condis = (fabs(m_rad)<0.0001 && fabs(m_cone_geo.Radius2)<0.0001);
			else
				condis = (fabs(m_rad)<0.0001 && fabs(m_cone_geo.Radius1)<0.0001);
			if (condis)
			{
				m_message.LoadString(IDS_ERROR_ZERO_BOTH_RADS);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			sgCCone* cone = NULL;
			if (m_scenar==0)
				cone = sgCreateCone(fabs(m_rad),m_cone_geo.Radius2,fabs(m_cone_geo.Height),
				m_cone_geo.MeridiansCount);
			else
				cone = sgCreateCone(m_cone_geo.Radius1,fabs(m_rad),fabs(m_cone_geo.Height),
				m_cone_geo.MeridiansCount);

			if (!cone)
				return;

			cone->InitTempMatrix()->Multiply(*m_matr);
			cone->ApplyTempMatrix();
			cone->DestroyTempMatrix();

			sgGetScene()->StartUndoGroup();
			sgGetScene()->DetachObject(m_editable_cone);
			sgGetScene()->AttachObject(cone);
			sgGetScene()->EndUndoGroup();
			m_app->CopyAttributes(*cone,*m_editable_cone);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	case 2:
		{
			ASSERT(m_r_panel);
			m_height = m_r_panel->GetNumber();
			if (fabs(m_height)<0.0001)
			{
				m_message.LoadString(IDS_ERROR_ZERO_LENGTH);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			else
				if (m_height<-0.0001)
				{
					m_message.LoadString(IDS_ERROR_HEIGHT_MUST_BE_POSIT);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
			sgCCone* cone = NULL;
			cone = sgCreateCone(m_cone_geo.Radius1,m_cone_geo.Radius2,
				fabs(m_height),
				m_cone_geo.MeridiansCount);
			if (!cone)
				return;

			cone->InitTempMatrix()->Multiply(*m_matr);
			cone->ApplyTempMatrix();
			cone->DestroyTempMatrix();

			sgGetScene()->StartUndoGroup();
			sgGetScene()->DetachObject(m_editable_cone);
			sgGetScene()->AttachObject(cone);
			sgGetScene()->EndUndoGroup();
			m_app->CopyAttributes(*cone,*m_editable_cone);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	default:
		break;
	}

	m_app->StopCommander();
}

unsigned int  CConeEditCommand::GetItemsCount()
{
return 4;
}

void         CConeEditCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	SWITCH_RESOURCE
		switch(itemID) {
		case 0:
			itSrt.LoadString(IDS_CHANGE_FIRST_RAD);
			break;
		case 1:
			itSrt.LoadString(IDS_CHANGE_SECOND_RAD);
			break;
		case 2:
			itSrt.LoadString(IDS_CHANGE_HEIGHT);
			break;
	case 3:
		itSrt.LoadString(IDS_OTHER_PARAMS);
		break;
	default:
		ASSERT(0);
		}
}

void     CConeEditCommand::GetItemState(unsigned int itemID, 
														bool& enbl, bool& checked)
{
	enbl = true;
	if (!m_was_started)
	{
		checked = false;
	}
	else
	{
		if (m_scenar!=itemID)
			checked = false;
		else
			checked = true;
	}
}

HBITMAP     CConeEditCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         CConeEditCommand::Run(unsigned int itemID)
{
	if (itemID==3)
	{
		SWITCH_RESOURCE
		CMeridiansDlg dlg;
		m_other_params_dlg = &dlg;
		ASSERT(m_editable_cone);
		dlg.SetDlgType(CMeridiansDlg::CONE_PARAMS);
		m_editable_cone->GetGeometry(m_cone_geo);
		dlg.SetParams(m_cone_geo.MeridiansCount);
		if (dlg.DoModal()==IDOK)
		{
			m_other_params_dlg = NULL;
			int mC;
			dlg.GetParams(mC);

			sgCMatrix spM(m_editable_cone->GetWorldMatrixData());
			spM.Transparent();

			sgCCone* cone = 
				 sgCreateCone(m_cone_geo.Radius1,
				 m_cone_geo.Radius2,m_cone_geo.Height,mC);
			cone->InitTempMatrix()->Multiply(spM);
			cone->ApplyTempMatrix();
			cone->DestroyTempMatrix();
			sgGetScene()->StartUndoGroup();
			sgGetScene()->DetachObject(m_editable_cone);
			sgGetScene()->AttachObject(cone);
			sgGetScene()->EndUndoGroup();
			m_app->CopyAttributes(*cone,*m_editable_cone);
			m_app->GetViewPort()->InvalidateViewPort();

		}
		m_app->StopCommander();
		return;
	}
	else
	{
		m_scenar = itemID;
		Start();
	}
}
