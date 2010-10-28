#include "stdafx.h"

#include "EllipsoidEdit.h"

#include "..//resource.h"

#include <math.h>

CEllipsoidEditCommand::CEllipsoidEditCommand(sgCEllipsoid* edE, IApplicationInterface* appI):
					m_editable_ellipsoid(edE)
					, m_app(appI)
					, m_size_panel(NULL)
					, m_was_started(false)
					,m_other_params_dlg(NULL)
					, m_matr(NULL)
					, m_scenar(0)
{
	ASSERT(edE);
	ASSERT(m_app);
	m_base_pnt.x = m_base_pnt.y = m_base_pnt.z = 0.0;
	m_dir.x = m_dir.y = m_dir.z = 0.0;
}

CEllipsoidEditCommand::~CEllipsoidEditCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
	if(m_matr)
		delete m_matr;
}


void  CEllipsoidEditCommand::ReCalcDir()
{
	m_dir.x = m_dir.y = m_dir.z = 0.0;
	switch(m_scenar) 
	{
	case 0:
		m_dir.x = 1.0;
		break;
	case 1:
		m_dir.y = 1.0;
		break;
	case 2:
		m_dir.z = 1.0;
		break;
	default:
		ASSERT(0);
		break;
	}

	if (m_matr)
	{
		SG_POINT ppp;
		ppp.x = ppp.y = ppp.z = 0.0;
		m_matr->ApplyMatrixToVector(ppp,m_dir);
	}
	else
	{
		ASSERT(0);
	}
}




bool    CEllipsoidEditCommand::PreTranslateMessage(MSG* pMsg)
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
			if (m_size_panel)
			{
				m_size_panel->GetWindow()->SendMessage(pMsg->message,
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


void  CEllipsoidEditCommand::Start()	
{
	ASSERT(m_editable_ellipsoid);

	if (m_matr)
		delete m_matr;

	m_matr = new sgCMatrix(m_editable_ellipsoid->GetWorldMatrixData());

	m_matr->Transparent();

	m_matr->ApplyMatrixToPoint(m_base_pnt);

	SWITCH_RESOURCE
	
	CString lab;

	lab.LoadString(IDS_REDACTION);
	m_app->StartCommander(lab);

	m_app->GetCommandPanel()->RemoveAllDialogs();
	lab.LoadString(IDS_NEW_SIZE);
	m_size_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,lab,false));
	lab.LoadString(IDS_ENTER_NEW_SIZE);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);

	m_editable_ellipsoid->GetGeometry(m_ell_geo);
	switch(m_scenar) 
	{
	case 0:
		m_size_panel->SetNumber(m_ell_geo.Radius1);
		break;
	case 1:
		m_size_panel->SetNumber(m_ell_geo.Radius2);
		break;
	case 2:
		m_size_panel->SetNumber(m_ell_geo.Radius3);
		break;
	default:
		ASSERT(0);
		break;
	}

	ReCalcDir();

m_app->GetCommandPanel()->SetActiveRadio(0);
	m_app->GetViewPort()->SetEditableObject(m_editable_ellipsoid);
	m_was_started = true;
}

void  CEllipsoidEditCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;
	ASSERT(m_size_panel);

	if (!m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,m_base_pnt,m_dir,m_projection))
	{
		switch(m_scenar) 
		{
		case 0:
			m_ell_geo.Radius1 = 0.0;
			break;
		case 1:
			m_ell_geo.Radius2 = 0.0;
			break;
		case 2:
			m_ell_geo.Radius3 = 0.0;
			break;
		default:
			ASSERT(0);
			break;
		}
		m_size_panel->SetNumber(0.0);
		return;
	}
	char sig = (((m_projection.x-m_base_pnt.x)*m_dir.x+
		(m_projection.y-m_base_pnt.y)*m_dir.y+
		(m_projection.z-m_base_pnt.z)*m_dir.z)>0)?1:-1;

	double  sz;
	sz = sgSpaceMath::PointsDistance(m_base_pnt,m_projection)*sig;
	sz = m_app->ApplyPrecision(sz);
	m_projection.x = m_base_pnt.x+m_dir.x*sz;
	m_projection.y = m_base_pnt.y+m_dir.y*sz;
	m_projection.z = m_base_pnt.z+m_dir.z*sz;
	switch(m_scenar) 
	{
	case 0:
		m_ell_geo.Radius1 = sz;
		break;
	case 1:
		m_ell_geo.Radius2 = sz;
		break;
	case 2:
		m_ell_geo.Radius3 = sz;
		break;
	default:
		ASSERT(0);
		break;
	}
	m_size_panel->SetNumber(sz);
	m_app->GetViewPort()->InvalidateViewPort();

}

void  CEllipsoidEditCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;
	SWITCH_RESOURCE
		ASSERT(m_editable_ellipsoid);

	if (fabs(m_ell_geo.Radius1)<0.0001 ||
		fabs(m_ell_geo.Radius2)<0.0001 ||
		fabs(m_ell_geo.Radius3)<0.0001)
	{
		m_message.LoadString(IDS_ERROR_ZERO_SIZE);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,
			m_message);
		return;
	}
	if (m_ell_geo.Radius1<0) m_ell_geo.Radius1=-m_ell_geo.Radius1;
	if (m_ell_geo.Radius2<0) m_ell_geo.Radius2=-m_ell_geo.Radius2;
	if (m_ell_geo.Radius3<0) m_ell_geo.Radius3=-m_ell_geo.Radius3;
	sgCEllipsoid* elpsd = sgCreateEllipsoid(m_ell_geo.Radius1,
		m_ell_geo.Radius2,m_ell_geo.Radius3,m_ell_geo.MeridiansCount,
		m_ell_geo.ParallelsCount);
	if (!elpsd)
		return;
	elpsd->InitTempMatrix()->Multiply(*m_matr);
	elpsd->ApplyTempMatrix();
	elpsd->DestroyTempMatrix();

	sgGetScene()->StartUndoGroup();
	sgGetScene()->DetachObject(m_editable_ellipsoid);
	sgGetScene()->AttachObject(elpsd);
	sgGetScene()->EndUndoGroup();
	m_app->CopyAttributes(*elpsd,*m_editable_ellipsoid);
	m_app->GetViewPort()->InvalidateViewPort();

	m_app->StopCommander();
}


void  CEllipsoidEditCommand::Draw()
{
	if (!m_was_started)
		return;

	float pC[3];
	m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->DrawPoint(m_projection);

	m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);

	m_app->GetViewPort()->GetPainter()->SetTransformMatrix(m_matr);
	m_app->GetViewPort()->GetPainter()->DrawEllipsoid(m_ell_geo.Radius1,
		m_ell_geo.Radius2,
		m_ell_geo.Radius3);
	m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);
}


void  CEllipsoidEditCommand::OnEnter()
{
	if (!m_was_started)
		return;
	SWITCH_RESOURCE
		ASSERT(m_editable_ellipsoid);

	double sz = m_size_panel->GetNumber();

	switch(m_scenar) 
	{
	case 0:
		m_ell_geo.Radius1 = sz;
		break;
	case 1:
		m_ell_geo.Radius2 = sz;
		break;
	case 2:
		m_ell_geo.Radius3 = sz;
		break;
	default:
		ASSERT(0);
		break;
	}

	if (fabs(m_ell_geo.Radius1)<0.0001 ||
		fabs(m_ell_geo.Radius2)<0.0001 ||
		fabs(m_ell_geo.Radius3)<0.0001)
	{
		m_message.LoadString(IDS_ERROR_ZERO_SIZE);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,
			m_message);
		return;
	}
	if (m_ell_geo.Radius1<0) m_ell_geo.Radius1=-m_ell_geo.Radius1;
	if (m_ell_geo.Radius2<0) m_ell_geo.Radius2=-m_ell_geo.Radius2;
	if (m_ell_geo.Radius3<0) m_ell_geo.Radius3=-m_ell_geo.Radius3;
	sgCEllipsoid* elpsd = sgCreateEllipsoid(m_ell_geo.Radius1,
		m_ell_geo.Radius2,m_ell_geo.Radius3,m_ell_geo.MeridiansCount,
		m_ell_geo.ParallelsCount);
	if (!elpsd)
		return;
	elpsd->InitTempMatrix()->Multiply(*m_matr);
	elpsd->ApplyTempMatrix();
	elpsd->DestroyTempMatrix();

	sgGetScene()->StartUndoGroup();
	sgGetScene()->DetachObject(m_editable_ellipsoid);
	sgGetScene()->AttachObject(elpsd);
	sgGetScene()->EndUndoGroup();
	m_app->CopyAttributes(*elpsd,*m_editable_ellipsoid);
	m_app->GetViewPort()->InvalidateViewPort();

	m_app->StopCommander();
}

unsigned int  CEllipsoidEditCommand::GetItemsCount()
{
return 4;
}

void         CEllipsoidEditCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	SWITCH_RESOURCE
	switch(itemID) {
	case 0:
		itSrt.LoadString(IDS_CHANGE_X_SIZE);
		break;
	case 1:
		itSrt.LoadString(IDS_CHANGE_Y_SIZE);
		break;
	case 2:
		itSrt.LoadString(IDS_CHANGE_Z_SIZE);
		break;
	case 3:
		itSrt.LoadString(IDS_OTHER_PARAMS);
		break;
	default:
		ASSERT(0);
		}
}

void     CEllipsoidEditCommand::GetItemState(unsigned int itemID, 
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

HBITMAP     CEllipsoidEditCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         CEllipsoidEditCommand::Run(unsigned int itemID)
{
	if (itemID==3)
	{
		SWITCH_RESOURCE
		CSphereParamsDlg dlg;
		m_other_params_dlg = &dlg;
		ASSERT(m_editable_ellipsoid);
		dlg.SetDlgType(CSphereParamsDlg::ELLIPSOID_PARAMS);
		m_editable_ellipsoid->GetGeometry(m_ell_geo);
		dlg.SetParams(m_ell_geo.MeridiansCount, m_ell_geo.ParallelsCount);
		if (dlg.DoModal()==IDOK)
		{
			m_other_params_dlg = NULL;
			int mC,pC;
			dlg.GetParams(mC,pC);

			sgCMatrix spM(m_editable_ellipsoid->GetWorldMatrixData());
			spM.Transparent();
			
			sgCEllipsoid* el = 
				sgCreateEllipsoid(m_ell_geo.Radius1,m_ell_geo.Radius2,
									m_ell_geo.Radius3,mC,pC);
			el->InitTempMatrix()->Multiply(spM);
			el->ApplyTempMatrix();
			el->DestroyTempMatrix();
			sgGetScene()->StartUndoGroup();
			sgGetScene()->DetachObject(m_editable_ellipsoid);
			sgGetScene()->AttachObject(el);
			sgGetScene()->EndUndoGroup();
			m_app->CopyAttributes(*el,*m_editable_ellipsoid);
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
