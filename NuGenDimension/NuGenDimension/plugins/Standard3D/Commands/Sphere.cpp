#include "stdafx.h"

#include "Sphere.h"

#include "..//resource.h"

#include <math.h>

int     sphere_name_index = 1;

SphereCommand::SphereCommand(IApplicationInterface* appI):
				m_app(appI)
				,m_base_point_panel(NULL)
				,m_r_panel(NULL)
				, m_step(0)
{
	ASSERT(m_app);
	m_matrix = new sgCMatrix;
}

SphereCommand::~SphereCommand()
{
	if (m_matrix)
		delete m_matrix;
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
}


bool    SphereCommand::PreTranslateMessage(MSG* pMsg)
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
			switch(m_step) {
			case 0:
				if (m_base_point_panel)
					m_base_point_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
				break;
			case 1:
				if (m_r_panel)
					m_r_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
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


void   SphereCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, void* params)
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=1);
		m_step = (unsigned int)newActiveDlg;
		if (m_step==0)
		{
			m_app->GetCommandPanel()->EnableRadio(1,false);
			SWITCH_RESOURCE
			CString aaa;
			aaa.LoadString(IDS_SPH_ENTER_CENTER);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,aaa);
		}
	}
}



void  SphereCommand::Start()	
{
	NewScenar();
	SWITCH_RESOURCE
	CString nm;
	nm.LoadString(IDS_TOOLTIP_FIRST);
	m_app->StartCommander(nm);
}

void  SphereCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	switch(m_step) 
	{
	case 0:
		{
			ASSERT(m_base_point_panel);
			IViewPort::GET_SNAP_IN in_arg;
			in_arg.scrX = pX;
			in_arg.scrY = pY;
			in_arg.snapType = SNAP_SYSTEM;
			in_arg.XFix = m_base_point_panel->IsXFixed();
			in_arg.YFix = m_base_point_panel->IsYFixed();
			in_arg.ZFix = m_base_point_panel->IsZFixed();
			double tmpFl[3];
			m_base_point_panel->GetPoint(tmpFl[0],tmpFl[1],tmpFl[2]);
			in_arg.FixPoint.x = tmpFl[0];
			in_arg.FixPoint.y = tmpFl[1];
			in_arg.FixPoint.z = tmpFl[2];
			IViewPort::GET_SNAP_OUT out_arg;
			m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
			m_cur_pnt = out_arg.result_point;
			m_base_point_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
		}
		break;
	case 1:
		{
			ASSERT(m_r_panel);
			double plD;
			SG_VECTOR PlNorm;
			m_app->GetViewPort()->GetViewPortNormal(PlNorm);
			sgSpaceMath::PlaneFromNormalAndPoint(m_first_pnt,PlNorm,plD);

			if (m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,PlNorm,plD,m_cur_pnt))
			{
				m_rad = m_app->ApplyPrecision(
					sgSpaceMath::PointsDistance(m_first_pnt,m_cur_pnt)
					);
			}
			else
				m_rad = 0.0;
			m_r_panel->SetNumber(m_rad);
		}
		break;
	default:
		ASSERT(0);
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

void  SphereCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	switch(m_step) 
	{
			case 0:
				m_first_pnt=m_cur_pnt;
				m_matrix->Identity();
				m_matrix->Translate(m_first_pnt);
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				{
					CString aaa;
					aaa.LoadString(IDS_SPH_ENTER_RAD);
					m_app->PutMessage(IApplicationInterface::MT_MESSAGE,aaa);
				}
				break;
			case 1:
				{
					double rd = sgSpaceMath::PointsDistance(m_first_pnt,m_cur_pnt);
					rd = m_app->ApplyPrecision(rd);
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
					sgCSphere* sp = sgCreateSphere(rd,24,24);
					if (!sp) return;
					sp->InitTempMatrix()->Translate(m_first_pnt);
					sp->ApplyTempMatrix();
					sp->DestroyTempMatrix();
					CString nm;
					nm.LoadString(IDS_TOOLTIP_FIRST);
					CString nmInd;
					nmInd.Format("%i",sphere_name_index);
					nm+=nmInd;
					sp->SetName(nm);
					sgGetScene()->StartUndoGroup();
					sgGetScene()->AttachObject(sp);
					m_app->ApplyAttributes(sp);
					sgGetScene()->EndUndoGroup();
					sphere_name_index++;
					m_app->GetViewPort()->InvalidateViewPort();
					NewScenar();
				}
				break;
			default:
				ASSERT(0);
				break;
	}
}



void  SphereCommand::Draw()
{
	switch(m_step) 
	{
	case 0:
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
		}
		break;
	case 1:
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(m_matrix);
			m_app->GetViewPort()->GetPainter()->DrawSphere(m_rad);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);
		}
		break;
	default:
		ASSERT(0);
	}
}

void  SphereCommand::OnEnter()
{
	SWITCH_RESOURCE
	switch(m_step) 
	{
		case 0:
			{
				ASSERT(m_base_point_panel);
				m_base_point_panel->GetPoint(m_first_pnt.x,
								m_first_pnt.y,
								m_first_pnt.z);
				m_matrix->Identity();
				m_matrix->Translate(m_first_pnt);
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				{
					CString aaa;
					aaa.LoadString(IDS_SPH_ENTER_RAD);
					m_app->PutMessage(IApplicationInterface::MT_MESSAGE,aaa);
				}
			}
			break;
		case 1:
			{
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
				sgCSphere* sp = sgCreateSphere(rd,24,24);
				if (!sp) return;
				sp->InitTempMatrix()->Translate(m_first_pnt);
				sp->ApplyTempMatrix();
				sp->DestroyTempMatrix();

				CString nm;
				nm.LoadString(IDS_TOOLTIP_FIRST);
				CString nmInd;
				nmInd.Format("%i",sphere_name_index);
				nm+=nmInd;
				sp->SetName(nm);
				
				sgGetScene()->StartUndoGroup();
				sgGetScene()->AttachObject(sp);
				m_app->ApplyAttributes(sp);
				sgGetScene()->EndUndoGroup();
				sphere_name_index++;
				m_app->GetViewPort()->InvalidateViewPort();
				NewScenar();
			}
			break;
		default:
			ASSERT(0);
			break;
	}
}

unsigned int  SphereCommand::GetItemsCount()
{
	return 0;
}

void         SphereCommand::GetItem(unsigned int, CString&)
{
	//SWITCH_RESOURCE
}

void     SphereCommand::GetItemState(unsigned int, bool&, bool&)
{
}

HBITMAP    SphereCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         SphereCommand::Run(unsigned int)
{
}

void  SphereCommand::NewScenar()
{
	SWITCH_RESOURCE
	m_app->GetCommandPanel()->RemoveAllDialogs();
	CString lab;
	lab.LoadString(IDS_CENTER);
	m_base_point_panel = reinterpret_cast<IGetPointPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,lab,true));
	
	lab.LoadString(IDS_RADIUS);
	m_r_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,lab,true));
	
	m_app->GetCommandPanel()->SetActiveRadio(0);

	CString aaa;
	aaa.LoadString(IDS_SPH_ENTER_CENTER);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,aaa);


	m_step=0;
}