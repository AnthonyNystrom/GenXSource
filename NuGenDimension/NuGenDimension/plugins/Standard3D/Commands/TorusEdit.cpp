#include "stdafx.h"

#include "TorusEdit.h"

#include "..//resource.h"

#include <math.h>
CTorusEditCommand::CTorusEditCommand(sgCTorus* edT, IApplicationInterface* appI):
					m_editable_torus(edT)
					, m_app(appI)
					, m_was_started(false)
					,m_scenar(-1)
					,m_matr(NULL)
					,m_other_params_dlg(NULL)
					,m_plD(0.0)
					,m_r_panel(NULL)
					,m_rad_1(0.0)
					,m_rad_2(0.0)
{
	ASSERT(edT);
	ASSERT(m_app);
	m_base_pnt.x = m_base_pnt.y = m_base_pnt.z = 0.0;
	m_dir.x = m_dir.y = 0.0;
	m_dir.z = 1.0;
}

CTorusEditCommand::~CTorusEditCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
	if(m_matr)
		delete m_matr;
}


bool    CTorusEditCommand::PreTranslateMessage(MSG* pMsg)
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


void  CTorusEditCommand::Start()	
{
	ASSERT(m_editable_torus);

	m_base_pnt.x = m_base_pnt.y = m_base_pnt.z = 0.0;
	m_dir.x = m_dir.y = 0.0;
	m_dir.z = 1.0;

	if(m_matr)
		delete m_matr;

	m_matr = new sgCMatrix(m_editable_torus->GetWorldMatrixData());

	m_matr->Transparent();

	m_base_pnt.x = m_base_pnt.y = m_base_pnt.z = 0.0;
	m_dir.x = m_dir.y = 0.0;
	m_dir.z = 1.0;

	m_matr->ApplyMatrixToPoint(m_base_pnt);
	SG_POINT ppp;
	ppp.x = ppp.y = ppp.z = 0.0;
	m_matr->ApplyMatrixToVector(ppp,m_dir);

	m_editable_torus->GetGeometry(m_tor_geo);

	sgSpaceMath::PlaneFromNormalAndPoint(m_base_pnt,m_dir,m_plD);

	m_app->GetCommandPanel()->RemoveAllDialogs();

	CString lab;
	lab.LoadString(IDS_REDACTION);
	m_app->StartCommander(lab);

	SWITCH_RESOURCE

	switch(m_scenar) 
	{
	case 0:
		lab.LoadString(IDS_NEW_RADIUS);
		m_r_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
			GetCommandPanel()->
			AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,lab,false));
		m_r_panel->SetNumber(m_tor_geo.Radius1);
		lab.LoadString(IDS_ENTER_NEW_RAD);
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);
		break;
	case 1:
		lab.LoadString(IDS_TOR_NEW_TOLSCH);
		m_r_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
			GetCommandPanel()->
			AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,lab,false));
		m_r_panel->SetNumber(m_tor_geo.Radius2);
		lab.LoadString(IDS_TOR_NEW_TOLSCH);
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);
		break;
	default:
		break;
	}

m_app->GetCommandPanel()->SetActiveRadio(0);
	m_app->GetViewPort()->SetEditableObject(m_editable_torus);
	m_was_started = true;
}

void  CTorusEditCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;
	switch(m_scenar) {
	case 0:
		{
			m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,m_dir,m_plD,m_cur_pnt);
			m_tor_geo.Radius1 = sgSpaceMath::PointsDistance(m_base_pnt,m_cur_pnt);
			m_tor_geo.Radius1 = m_app->ApplyPrecision(m_tor_geo.Radius1);
			ASSERT(m_r_panel);
			m_r_panel->SetNumber(m_tor_geo.Radius1);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	case 1:
		{
			m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,m_dir,m_plD,m_cur_pnt);
			m_tor_geo.Radius2 = sgSpaceMath::PointsDistance(m_base_pnt,m_cur_pnt);
			m_tor_geo.Radius2 = fabs(m_tor_geo.Radius2-m_tor_geo.Radius1);
			m_tor_geo.Radius2 = m_app->ApplyPrecision(m_tor_geo.Radius2);
			ASSERT(m_r_panel);
			m_r_panel->SetNumber(m_tor_geo.Radius2);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	default:
		break;
	}
}

void  CTorusEditCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;
SWITCH_RESOURCE
	ASSERT(m_editable_torus);

switch(m_scenar) {
	case 0:
		{
			if (fabs(m_tor_geo.Radius1)<0.00001)
			{
				m_message.LoadString(IDS_ERROR_ZERO_RAD);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			else
				if (m_tor_geo.Radius1<-0.0001)
				{
					m_message.LoadString(IDS_ERROR_RAD_MUST_BE_POSIT);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
			if (m_tor_geo.Radius2>m_tor_geo.Radius1)
				{
					m_message.LoadString(IDS_ERROR_TOL_MUST_BE_LETTER_RAD);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
			sgCTorus* tr = sgCreateTorus(fabs(m_tor_geo.Radius1),
				m_tor_geo.Radius2,m_tor_geo.MeridiansCount1,
				m_tor_geo.MeridiansCount2);
			if (!tr)
				return;
			
			tr->InitTempMatrix()->Multiply(*m_matr);
			tr->ApplyTempMatrix();
			tr->DestroyTempMatrix();

			sgGetScene()->StartUndoGroup();
			sgGetScene()->DetachObject(m_editable_torus);
			sgGetScene()->AttachObject(tr);
			sgGetScene()->EndUndoGroup();
			m_app->CopyAttributes(*tr,*m_editable_torus);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	case 1:
		{
			if (fabs(m_tor_geo.Radius2)<0.00001)
			{
				m_message.LoadString(IDS_ERROR_ZERO_RAD);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			else
				if (m_tor_geo.Radius2<-0.0001)
				{
					m_message.LoadString(IDS_ERROR_TOLS_MUST_BE_POSIT);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				if (m_tor_geo.Radius2>m_tor_geo.Radius1)
				{
					m_message.LoadString(IDS_ERROR_TOL_MUST_BE_LETTER_RAD);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
			sgCTorus* tr = sgCreateTorus(m_tor_geo.Radius1,
				fabs(m_tor_geo.Radius2),m_tor_geo.MeridiansCount1,
				m_tor_geo.MeridiansCount2);
			if (!tr)
				return;

			tr->InitTempMatrix()->Multiply(*m_matr);
			tr->ApplyTempMatrix();
			tr->DestroyTempMatrix();

			sgGetScene()->StartUndoGroup();
			sgGetScene()->DetachObject(m_editable_torus);
			sgGetScene()->AttachObject(tr);
			sgGetScene()->EndUndoGroup();
			m_app->CopyAttributes(*tr,*m_editable_torus);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	default:
		break;
}

	m_app->StopCommander();
}


void  CTorusEditCommand::Draw()
{
	if (!m_was_started)
		return;
	if (m_scenar==0 || m_scenar==1)
	{
		float pC[3];
		m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetTransformMatrix(m_matr);
		m_app->GetViewPort()->GetPainter()->DrawTorus(m_tor_geo.Radius1,
			m_tor_geo.Radius2);
		m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);
	}
}

void  CTorusEditCommand::OnEnter()
{
	if (!m_was_started)
		return;

	SWITCH_RESOURCE
	ASSERT(m_editable_torus);

	switch(m_scenar) {
	case 0:
		{
			m_tor_geo.Radius1 = m_r_panel->GetNumber();
			if (fabs(m_tor_geo.Radius1)<0.00001)
			{
				m_message.LoadString(IDS_ERROR_ZERO_RAD);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			else
				if (m_tor_geo.Radius1<-0.0001)
				{
					m_message.LoadString(IDS_ERROR_RAD_MUST_BE_POSIT);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				if (m_tor_geo.Radius2>m_tor_geo.Radius1)
				{
					m_message.LoadString(IDS_ERROR_TOL_MUST_BE_LETTER_RAD);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				sgCTorus* tr = sgCreateTorus(fabs(m_tor_geo.Radius1),
					m_tor_geo.Radius2,m_tor_geo.MeridiansCount1,
					m_tor_geo.MeridiansCount2);
				if (!tr)
					return;

				tr->InitTempMatrix()->Multiply(*m_matr);
				tr->ApplyTempMatrix();
				tr->DestroyTempMatrix();

				sgGetScene()->StartUndoGroup();
				sgGetScene()->DetachObject(m_editable_torus);
				sgGetScene()->AttachObject(tr);
				sgGetScene()->EndUndoGroup();
				m_app->CopyAttributes(*tr,*m_editable_torus);
				m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	case 1:
		{
			m_tor_geo.Radius2 = m_r_panel->GetNumber();
			if (fabs(m_tor_geo.Radius2)<0.00001)
			{
				m_message.LoadString(IDS_ERROR_ZERO_TOLZCHINA);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			else
				if (m_tor_geo.Radius2<-0.0001)
				{
					m_message.LoadString(IDS_ERROR_TOLS_MUST_BE_POSIT);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				if (m_tor_geo.Radius2>m_tor_geo.Radius1)
				{
					m_message.LoadString(IDS_ERROR_TOL_MUST_BE_LETTER_RAD);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				sgCTorus* tr = sgCreateTorus(m_tor_geo.Radius1,
					fabs(m_tor_geo.Radius2),m_tor_geo.MeridiansCount1,
					m_tor_geo.MeridiansCount2);
				if (!tr)
					return;

				tr->InitTempMatrix()->Multiply(*m_matr);
				tr->ApplyTempMatrix();
				tr->DestroyTempMatrix();

				sgGetScene()->StartUndoGroup();
				sgGetScene()->DetachObject(m_editable_torus);
				sgGetScene()->AttachObject(tr);
				sgGetScene()->EndUndoGroup();
				m_app->CopyAttributes(*tr,*m_editable_torus);
				m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	default:
		break;
	}

	m_app->StopCommander();
}

unsigned int  CTorusEditCommand::GetItemsCount()
{
return 3;
}

void         CTorusEditCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	SWITCH_RESOURCE
		switch(itemID) {
	case 0:
		itSrt.LoadString(IDS_CHANGE_SPH_RAD);
		break;
	case 1:
		itSrt.LoadString(IDS_CHANGE_TOLSH);
		break;
	case 2:
		itSrt.LoadString(IDS_OTHER_PARAMS);
		break;
	default:
		ASSERT(0);
		}
}

void     CTorusEditCommand::GetItemState(unsigned int itemID, 
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

HBITMAP     CTorusEditCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         CTorusEditCommand::Run(unsigned int itemID)
{
	if (itemID==2)
	{
		SWITCH_RESOURCE
		CTorusParamsDlg dlg;
		m_other_params_dlg = &dlg;
		ASSERT(m_editable_torus);
		m_editable_torus->GetGeometry(m_tor_geo);
		dlg.SetParams(m_tor_geo.MeridiansCount1, 
			m_tor_geo.MeridiansCount2);
		if (dlg.DoModal()==IDOK)
		{
			m_other_params_dlg = NULL;
			int mC1,mC2;
			dlg.GetParams(mC1,mC2);

			sgCMatrix spM(m_editable_torus->GetWorldMatrixData());
			spM.Transparent();
			
			sgCTorus* tr = sgCreateTorus(m_tor_geo.Radius1,
				m_tor_geo.Radius2,mC1,mC2);
			if (!tr)
				return;
			tr->InitTempMatrix()->Multiply(spM);
			tr->ApplyTempMatrix();
			tr->DestroyTempMatrix();
			sgGetScene()->StartUndoGroup();
			sgGetScene()->DetachObject(m_editable_torus);
			sgGetScene()->AttachObject(tr);
			sgGetScene()->EndUndoGroup();
			m_app->CopyAttributes(*tr,*m_editable_torus);
			CString m_message;
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
