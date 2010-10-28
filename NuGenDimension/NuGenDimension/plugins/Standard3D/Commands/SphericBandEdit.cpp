#include "stdafx.h"

#include "SphericBandEdit.h"

#include "..//resource.h"

#include <math.h>
CSphericBandEditCommand::CSphericBandEditCommand(sgCSphericBand* edSb, IApplicationInterface* appI):
					m_editable_sb(edSb)
					, m_app(appI)
					, m_was_started(false)
					, m_matr(NULL)
					,m_other_params_dlg(NULL)
					, m_rad(0.0)
					, m_cur_coef(0.0)
					, m_number_panel(NULL)
					, m_scenar(-1)
{
	ASSERT(edSb);
	ASSERT(m_app);
	m_base_pnt.x = m_base_pnt.y = m_base_pnt.z = 0.0;
	m_dir.x = m_dir.y = 0.0;
	m_dir.z = 1.0;
}

CSphericBandEditCommand::~CSphericBandEditCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
	if(m_matr)
		delete m_matr;
}

bool    CSphericBandEditCommand::PreTranslateMessage(MSG* pMsg)
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
			if (m_number_panel)
			{
				m_number_panel->GetWindow()->SendMessage(pMsg->message,
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


void  CSphericBandEditCommand::Start()	
{
	ASSERT(m_editable_sb);

	m_base_pnt.x = m_base_pnt.y = m_base_pnt.z = 0.0;
	m_dir.x = m_dir.y = 0.0;
	m_dir.z = 1.0;

	if(m_matr)
		delete m_matr;

	m_matr = new sgCMatrix(m_editable_sb->GetWorldMatrixData());

	m_matr->Transparent();

	m_base_pnt.x = m_base_pnt.y = m_base_pnt.z = 0.0;
	m_dir.x = m_dir.y = 0.0;
	m_dir.z = 1.0;

	m_matr->ApplyMatrixToPoint(m_base_pnt);
	SG_POINT ppp;
	ppp.x = ppp.y = ppp.z = 0.0;
	m_matr->ApplyMatrixToVector(ppp,m_dir);

	m_editable_sb->GetGeometry(m_sp_b_geo);
	SWITCH_RESOURCE

	m_app->GetCommandPanel()->RemoveAllDialogs();

	CString lab;

	lab.LoadString(IDS_REDACTION);
	m_app->StartCommander(lab);

	
	switch(m_scenar) 
	{
	case 0:
		lab.LoadString(IDS_NEW_RADIUS);
		m_number_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
			GetCommandPanel()->
			AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,lab,false));
		m_number_panel->SetNumber(m_sp_b_geo.Radius);
		lab.LoadString(IDS_ENTER_NEW_RAD);
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);
		break;
	case 1:
		lab.LoadString(IDS_CHANGE_F_COEF);
		m_number_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
			GetCommandPanel()->
			AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,lab,false));
		m_number_panel->SetNumber(m_sp_b_geo.BeginCoef);
		lab.LoadString(IDS_SP_B_ENTER_NEW_COEF);
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);
		break;
	case 2:
		lab.LoadString(IDS_CHANGE_S_COEF);
		m_number_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
			GetCommandPanel()->
			AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,lab,false));
		m_number_panel->SetNumber(m_sp_b_geo.EndCoef);
		lab.LoadString(IDS_SP_B_ENTER_NEW_COEF);
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);
		break;
	default:
		break;
	}

m_app->GetCommandPanel()->SetActiveRadio(0);
	m_app->GetViewPort()->SetEditableObject(m_editable_sb);
	m_was_started = true;
}

void  CSphericBandEditCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;
	switch(m_scenar) {
	case 0:
		{
			double plD;
			SG_VECTOR PlNorm;
			m_app->GetViewPort()->GetViewPortNormal(PlNorm);
			sgSpaceMath::PlaneFromNormalAndPoint(m_base_pnt,PlNorm,plD);

			if (m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,PlNorm,plD,m_cur_pnt))
			{
				m_rad = m_app->ApplyPrecision(
					sgSpaceMath::PointsDistance(m_base_pnt,m_cur_pnt)
					);
			}
			else
				m_rad = 0.0;
			m_rad = m_app->ApplyPrecision(m_rad);
			ASSERT(m_number_panel);
			m_number_panel->SetNumber(m_rad);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	case 1:
	case 2:
		{
			ASSERT(m_number_panel);

			SG_POINT proj;
			if (!m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,m_base_pnt,m_dir,proj))
			{
				m_cur_coef = 0.0;
				m_number_panel->SetNumber(m_cur_coef);
				break;
			}
			char sig = (((proj.x-m_base_pnt.x)*m_dir.x+
				(proj.y-m_base_pnt.y)*m_dir.y+
				(proj.z-m_base_pnt.z)*m_dir.z)>0)?1:-1;


			m_cur_coef = sgSpaceMath::PointsDistance(m_base_pnt,proj)*sig/m_sp_b_geo.Radius;
			if (m_cur_coef>1.0) m_cur_coef = 1.0;
			if (m_cur_coef<-1.0) m_cur_coef = -1.0;
			m_number_panel->SetNumber(m_cur_coef);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	default:
		break;
	}

}

void  CSphericBandEditCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;
SWITCH_RESOURCE
	ASSERT(m_editable_sb);

switch(m_scenar) {
	case 0:
		{
			if (fabs(m_rad)<0.0001)
			{
				m_message.LoadString(IDS_ERROR_ZERO_RAD);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			sgCSphericBand* spb = NULL;
			spb = sgCreateSphericBand(m_rad,m_sp_b_geo.BeginCoef,m_sp_b_geo.EndCoef,
				m_sp_b_geo.MeridiansCount);
			if (!spb)
				return;
			spb->InitTempMatrix()->Multiply(*m_matr);
			spb->ApplyTempMatrix();
			spb->DestroyTempMatrix();

			sgGetScene()->StartUndoGroup();
			sgGetScene()->DetachObject(m_editable_sb);
			sgGetScene()->AttachObject(spb);
			sgGetScene()->EndUndoGroup();
			m_app->CopyAttributes(*spb,*m_editable_sb);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	case 1:
	case 2:
		{
			bool condis = false;
			if (m_scenar==1)
				condis = (fabs(m_cur_coef-m_sp_b_geo.EndCoef)<0.0001);
			else
				condis = (fabs(m_cur_coef-m_sp_b_geo.BeginCoef)<0.0001);
			if (condis)
			{
				m_message.LoadString(IDS_COEF_IS_EQ);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			if (m_scenar==1)
			{
				if (m_cur_coef>=m_sp_b_geo.EndCoef)
				{
					double ttt = m_cur_coef;
					m_cur_coef = m_sp_b_geo.EndCoef;
					m_sp_b_geo.EndCoef = ttt;
				}
			}
			else
			{
				if (m_cur_coef<=m_sp_b_geo.BeginCoef)
				{
					double ttt = m_cur_coef;
					m_cur_coef = m_sp_b_geo.BeginCoef;
					m_sp_b_geo.BeginCoef = ttt;
				}
			}
			sgCSphericBand* spb = NULL;
			if (m_scenar==1)
				spb = sgCreateSphericBand(m_sp_b_geo.Radius,m_cur_coef,m_sp_b_geo.EndCoef,
					m_sp_b_geo.MeridiansCount);
			else
				spb = sgCreateSphericBand(m_sp_b_geo.Radius,m_sp_b_geo.BeginCoef,m_cur_coef,
				m_sp_b_geo.MeridiansCount);

			if (!spb)
				return;

			spb->InitTempMatrix()->Multiply(*m_matr);
			spb->ApplyTempMatrix();
			spb->DestroyTempMatrix();

			sgGetScene()->StartUndoGroup();
			sgGetScene()->DetachObject(m_editable_sb);
			sgGetScene()->AttachObject(spb);
			sgGetScene()->EndUndoGroup();
			m_app->CopyAttributes(*spb,*m_editable_sb);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	default:
		break;
}

	m_app->StopCommander();
}

void  CSphericBandEditCommand::Draw()
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
			m_app->GetViewPort()->GetPainter()->DrawSphericBand(m_rad,m_sp_b_geo.BeginCoef,
				m_sp_b_geo.EndCoef);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);
		}
		break;
	case 1:
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(m_matr);
			m_app->GetViewPort()->GetPainter()->DrawSphericBand(m_sp_b_geo.Radius,m_cur_coef,
				m_sp_b_geo.EndCoef);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);
		}
		break;
	case 2:
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(m_matr);
			m_app->GetViewPort()->GetPainter()->DrawSphericBand(m_sp_b_geo.Radius,m_sp_b_geo.BeginCoef,
				m_cur_coef);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);
		}
		break;
	default:
		break;
	}
}
void  CSphericBandEditCommand::OnEnter()
{
	if (!m_was_started)
		return;
	SWITCH_RESOURCE
		ASSERT(m_editable_sb);

	switch(m_scenar) {
	case 0:
		{
			m_rad = m_number_panel->GetNumber();
			if (fabs(m_rad)<0.0001)
			{
				m_message.LoadString(IDS_ERROR_ZERO_RAD);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			else
				if (m_rad<-0.0001)
				{
					m_message.LoadString(IDS_ERROR_RAD_MUST_BE_POSIT);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
			sgCSphericBand* spb = NULL;
			spb = sgCreateSphericBand(m_rad,m_sp_b_geo.BeginCoef,m_sp_b_geo.EndCoef,
				m_sp_b_geo.MeridiansCount);
			if (!spb)
				return;
			spb->InitTempMatrix()->Multiply(*m_matr);
			spb->ApplyTempMatrix();
			spb->DestroyTempMatrix();

			sgGetScene()->StartUndoGroup();
			sgGetScene()->DetachObject(m_editable_sb);
			sgGetScene()->AttachObject(spb);
			sgGetScene()->EndUndoGroup();
			m_app->CopyAttributes(*spb,*m_editable_sb);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	case 1:
	case 2:
		{
			bool condis = false;
			m_cur_coef = m_number_panel->GetNumber();
			if (fabs(m_cur_coef)>1.000)
			{
				m_message.LoadString(IDS_ERROR_COEF_RANGE);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			if (m_scenar==1)
				condis = (fabs(m_cur_coef-m_sp_b_geo.EndCoef)<0.0001);
			else
				condis = (fabs(m_cur_coef-m_sp_b_geo.BeginCoef)<0.0001);
			if (condis)
			{
				m_message.LoadString(IDS_COEF_IS_EQ);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			if (m_scenar==1)
			{
				if (m_cur_coef>=m_sp_b_geo.EndCoef)
				{
					double ttt = m_cur_coef;
					m_cur_coef = m_sp_b_geo.EndCoef;
					m_sp_b_geo.EndCoef = ttt;
				}
			}
			else
			{
				if (m_cur_coef<=m_sp_b_geo.BeginCoef)
				{
					double ttt = m_cur_coef;
					m_cur_coef = m_sp_b_geo.BeginCoef;
					m_sp_b_geo.BeginCoef = ttt;
				}
			}
			sgCSphericBand* spb = NULL;
			if (m_scenar==1)
				spb = sgCreateSphericBand(m_sp_b_geo.Radius,m_cur_coef,m_sp_b_geo.EndCoef,
				m_sp_b_geo.MeridiansCount);
			else
				spb = sgCreateSphericBand(m_sp_b_geo.Radius,m_sp_b_geo.BeginCoef,m_cur_coef,
				m_sp_b_geo.MeridiansCount);

			if (!spb)
				return;

			spb->InitTempMatrix()->Multiply(*m_matr);
			spb->ApplyTempMatrix();
			spb->DestroyTempMatrix();

			sgGetScene()->StartUndoGroup();
			sgGetScene()->DetachObject(m_editable_sb);
			sgGetScene()->AttachObject(spb);
			sgGetScene()->EndUndoGroup();
			m_app->CopyAttributes(*spb,*m_editable_sb);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	default:
		break;
	}

	m_app->StopCommander();
}

unsigned int  CSphericBandEditCommand::GetItemsCount()
{
return 4;
}

void         CSphericBandEditCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	SWITCH_RESOURCE
		switch(itemID) {
	case 0:
		itSrt.LoadString(IDS_CHANGE_SPH_RAD);
		break;
	case 1:
		itSrt.LoadString(IDS_CHANGE_F_COEF);
		break;
	case 2:
		itSrt.LoadString(IDS_CHANGE_S_COEF);
		break;
	case 3:
		itSrt.LoadString(IDS_OTHER_PARAMS);
		break;
	default:
		ASSERT(0);
		}
}

void     CSphericBandEditCommand::GetItemState(unsigned int itemID, 
														bool& enbl, bool& checked)
{
	enbl = true;
	if (!m_was_started)
	{
		checked = false;
	}
	else
	{
		if (itemID==m_scenar-1)
			checked = true;
		else
			checked = false;
	}
}

HBITMAP     CSphericBandEditCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         CSphericBandEditCommand::Run(unsigned int itemID)
{
	try { //#try
		if (itemID==3)
		{
			SWITCH_RESOURCE
			CMeridiansDlg dlg;
			m_other_params_dlg = &dlg;
			ASSERT(m_editable_sb);
			dlg.SetDlgType(CMeridiansDlg::SPH_BAND_PARAMS);
			m_editable_sb->GetGeometry(m_sp_b_geo);
			dlg.SetParams(m_sp_b_geo.MeridiansCount);
			if (dlg.DoModal()==IDOK)
			{
				m_other_params_dlg = NULL;
				int mC;
				dlg.GetParams(mC);

				sgCMatrix spM(m_editable_sb->GetWorldMatrixData());
				spM.Transparent();

				sgCSphericBand* spb = 
					sgCreateSphericBand(m_sp_b_geo.Radius,
					m_sp_b_geo.BeginCoef,m_sp_b_geo.EndCoef,mC);
				spb->InitTempMatrix()->Multiply(spM);
				spb->ApplyTempMatrix();
				spb->DestroyTempMatrix();
				sgGetScene()->StartUndoGroup();
				sgGetScene()->DetachObject(m_editable_sb);
				sgGetScene()->AttachObject(spb);
				sgGetScene()->EndUndoGroup();
				m_app->CopyAttributes(*spb,*m_editable_sb);
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
	catch(...)
	{
	}
}
