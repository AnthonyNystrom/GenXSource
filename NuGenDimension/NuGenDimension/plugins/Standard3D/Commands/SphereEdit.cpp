#include "stdafx.h"

#include "SphereEdit.h"

#include "..//resource.h"

#include <math.h>

CSphereEditCommand::CSphereEditCommand(sgCSphere* edS, IApplicationInterface* appI):
					m_editable_sphere(edS)
					, m_app(appI)
					, m_r_panel(NULL)
					, m_was_started(false)
					,m_other_params_dlg(NULL)
					, m_matr(NULL)
{
	ASSERT(edS);
	ASSERT(m_app);
	m_center.x = m_center.y = m_center.z = 0.0;
}

CSphereEditCommand::~CSphereEditCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	if(m_matr)
		delete m_matr;
	m_app->GetViewPort()->InvalidateViewPort();
}

bool    CSphereEditCommand::PreTranslateMessage(MSG* pMsg)
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


void  CSphereEditCommand::Start()	
{
	ASSERT(m_editable_sphere);

	m_matr = new sgCMatrix(m_editable_sphere->GetWorldMatrixData());

	m_matr->Transparent();

	SWITCH_RESOURCE
	
	m_app->GetCommandPanel()->RemoveAllDialogs();
	CString lab;
	lab.LoadString(IDS_NEW_RADIUS);
	m_r_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,lab,false));
	
	lab.LoadString(IDS_REDACTION);
	m_app->StartCommander(lab);

	lab.LoadString(IDS_ENTER_NEW_RAD);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);

	m_editable_sphere->GetGeometry(m_sph_geo);
	m_r_panel->SetNumber(m_sph_geo.Radius);

	sgCMatrix spM(m_editable_sphere->GetWorldMatrixData());
	spM.Transparent();
	spM.ApplyMatrixToPoint(m_center);
	m_app->GetCommandPanel()->SetActiveRadio(0);
	m_app->GetViewPort()->SetEditableObject(m_editable_sphere);
	m_was_started = true;
}

void  CSphereEditCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;
	SWITCH_RESOURCE

	ASSERT(m_r_panel);
	double plD;
	SG_VECTOR PlNorm;
	m_app->GetViewPort()->GetViewPortNormal(PlNorm);
	sgSpaceMath::PlaneFromNormalAndPoint(m_center,PlNorm,plD);

	if (m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,PlNorm,plD,m_cur_pnt))
	{
		m_rad = m_app->ApplyPrecision(
			sgSpaceMath::PointsDistance(m_center,m_cur_pnt)
			);
	}
	else
		m_rad = 0.0;
	m_r_panel->SetNumber(m_rad);
	m_app->GetViewPort()->InvalidateViewPort();
}

void  CSphereEditCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;
	SWITCH_RESOURCE
	ASSERT(m_editable_sphere);

	CString m_message;
	double rd = m_app->ApplyPrecision(m_rad);

	if (fabs(rd)<0.0001)
	{
		m_message.LoadString(IDS_ERROR_ZERO_RAD);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,
			m_message);
		return;
	}
	else
		if (rd<-0.0001)
		{
			m_message.LoadString(IDS_ERROR_RAD_MUST_BE_POSIT);
			m_app->PutMessage(IApplicationInterface::MT_ERROR,
				m_message);
			return;
		}
	sgCSphere* sp = sgCreateSphere(rd,m_sph_geo.MeridiansCount,m_sph_geo.ParallelsCount);
	if (!sp)
		return;
	sp->InitTempMatrix()->Multiply(*m_matr);
	sp->ApplyTempMatrix();
	sp->DestroyTempMatrix();
	sgGetScene()->StartUndoGroup();
	sgGetScene()->DetachObject(m_editable_sphere);
	sgGetScene()->AttachObject(sp);
	sgGetScene()->EndUndoGroup();
	m_app->CopyAttributes(*sp,*m_editable_sphere);
	m_app->GetViewPort()->InvalidateViewPort();

	m_app->StopCommander();
}



void  CSphereEditCommand::Draw()
{
	if (!m_was_started)
		return;
	
	float pC[3];
	m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
	m_app->GetViewPort()->GetPainter()->DrawPoint(m_center);
	
	m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
	
	m_app->GetViewPort()->GetPainter()->SetTransformMatrix(m_matr);
	m_app->GetViewPort()->GetPainter()->DrawSphere(m_rad);
	m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);

}
void  CSphereEditCommand::OnEnter()
{
	if (!m_was_started)
		return;

	SWITCH_RESOURCE
		ASSERT(m_editable_sphere);

	CString m_message;
	double rd = m_r_panel->GetNumber();
	if (fabs(rd)<0.0001)
	{
		m_message.LoadString(IDS_ERROR_ZERO_RAD);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,
			m_message);
		return;
	}
	else
		if (rd<-0.0001)
		{
			m_message.LoadString(IDS_ERROR_RAD_MUST_BE_POSIT);
			m_app->PutMessage(IApplicationInterface::MT_ERROR,
				m_message);
			return;
		}
	sgCSphere* sp = sgCreateSphere(rd,m_sph_geo.MeridiansCount,m_sph_geo.ParallelsCount);
	if (!sp)
		return;
	sp->InitTempMatrix()->Multiply(*m_matr);
	sp->ApplyTempMatrix();
	sp->DestroyTempMatrix();
	sgGetScene()->StartUndoGroup();
	sgGetScene()->DetachObject(m_editable_sphere);
	sgGetScene()->AttachObject(sp);
	sgGetScene()->EndUndoGroup();
	m_app->CopyAttributes(*sp,*m_editable_sphere);
	m_app->GetViewPort()->InvalidateViewPort();

	m_app->StopCommander();
}

unsigned int  CSphereEditCommand::GetItemsCount()
{
	return 2;
}

void         CSphereEditCommand::GetItem(unsigned int itemID, CString& itSrt)
{
		SWITCH_RESOURCE
		switch(itemID) {
	case 0:
		itSrt.LoadString(IDS_CHANGE_SPH_RAD);
		break;
	case 1:
		itSrt.LoadString(IDS_OTHER_PARAMS);
		break;
	/*case 2:
		itSrt.LoadString(IDS_CHANGE_NORM);
		break;*/
	default:
		ASSERT(0);
		}
}

void     CSphereEditCommand::GetItemState(unsigned int itemID, 
														bool& enbl, bool& checked)
{
	enbl = true;
	if (!m_was_started)
	{
		checked = false;
	}
	else
	{
		checked = true;
		/*if (itemID==m_scenario)
			checked = true;*/
	}
}

HBITMAP     CSphereEditCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         CSphereEditCommand::Run(unsigned int itemID)
{
	if (itemID==1)
	{
		SWITCH_RESOURCE
		CSphereParamsDlg dlg;
		m_other_params_dlg = &dlg;
		ASSERT(m_editable_sphere);
		m_editable_sphere->GetGeometry(m_sph_geo);
		dlg.SetParams(m_sph_geo.MeridiansCount, m_sph_geo.ParallelsCount);
		if (dlg.DoModal()==IDOK)
		{
			m_other_params_dlg = NULL;
			int mC,pC;
			dlg.GetParams(mC,pC);

			sgCMatrix spM(m_editable_sphere->GetWorldMatrixData());
			spM.Transparent();
			spM.ApplyMatrixToPoint(m_center);

			sgCSphere* sp = sgCreateSphere(m_sph_geo.Radius,mC,pC);
			if (!sp)
				return;
			sp->InitTempMatrix()->Multiply(spM);
			sp->ApplyTempMatrix();
			sp->DestroyTempMatrix();
			sgGetScene()->StartUndoGroup();
			sgGetScene()->DetachObject(m_editable_sphere);
			sgGetScene()->AttachObject(sp);
			sgGetScene()->EndUndoGroup();
			m_app->CopyAttributes(*sp,*m_editable_sphere);
			m_app->GetViewPort()->InvalidateViewPort();

		}
		m_app->StopCommander();
			return;
	}

	if (!m_was_started)
	{
		/*if (itemID==0 || itemID==1 || itemID==2)
		{
			m_scenario = itemID;*/
			Start();
		/*}
		else
		{
			ASSERT(0);
		}*/
	}
	else
	{
	/*	m_scenario = itemID;
		NewScenario();*/
	}
}
