#include "stdafx.h"

#include "Torus.h"

#include "..//resource.h"

#include <math.h>

int     torus_name_index = 1;

TorusCommand::TorusCommand(IApplicationInterface* appI):
				m_app(appI)
					,m_base_point_panel(NULL)
					,m_normal_panel(NULL)
					,m_r1_panel(NULL)
					, m_r2_panel(NULL)
				, m_step(0)
{
	ASSERT(m_app);
	m_matrix = new sgCMatrix;
}

TorusCommand::~TorusCommand()
{
	if (m_matrix)
		delete m_matrix;
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
}


bool    TorusCommand::PreTranslateMessage(MSG* pMsg)
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
				m_base_point_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
				break;
			case 1:
				if (m_normal_panel)
					m_normal_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
				break;
			case 2:
				if (m_r1_panel)
					m_r1_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
				break;
			case 3:
				if (m_r2_panel)
					m_r2_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
				break;
			default:
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


void   TorusCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, void* params)
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=3);
		m_step = (unsigned int)newActiveDlg;
		for (unsigned int i=m_step+1;i<=3;i++)
			m_app->GetCommandPanel()->EnableRadio(i,false);
		SWITCH_RESOURCE
			switch(m_step) 
		{
			case 0:
				m_message.LoadString(IDS_SPH_ENTER_CENTER);
				break;
			case 1:
				m_message.LoadString(IDS_TOR_ENTER_NORMAL);
				break;
			case 2:
				m_message.LoadString(IDS_TOR_ENTER_RAD);
				break;
			case 3:
				m_message.LoadString(IDS_TOR_ENTER_TOLSH);
				break;
			default:
				ASSERT(0);
				break;
		}
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

	}
	if (mes==ICommander::CM_UPDATE_COMMAND_PANEL)
	{
		MouseMove(0,0,0);
	}
}


void  TorusCommand::Start()	
{
	NewScenar();
	SWITCH_RESOURCE
	m_message.LoadString(IDS_TOOLTIP_FOURTH);
	m_app->StartCommander(m_message);
}

void  TorusCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (m_step==0)
	{
		ASSERT(m_base_point_panel);
		IViewPort::GET_SNAP_IN in_arg;
		in_arg.scrX = pX;
		in_arg.scrY = pY;
		in_arg.snapType = SNAP_SYSTEM;
		in_arg.XFix = m_base_point_panel->IsXFixed();
		in_arg.YFix = m_base_point_panel->IsYFixed();
		in_arg.ZFix = m_base_point_panel->IsZFixed();
		m_base_point_panel->GetPoint(in_arg.FixPoint.x,
			in_arg.FixPoint.y,
			in_arg.FixPoint.z);
		IViewPort::GET_SNAP_OUT out_arg;
		m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
		m_cur_pnt = out_arg.result_point;
		m_base_point_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
	}
	if (m_step==1)
	{
		ASSERT(m_normal_panel);

		double tmpFl[3];
		if (m_normal_panel->GetVector(tmpFl[0],tmpFl[1],tmpFl[2])==
			IGetVectorPanel::USER_VECTOR)
		{
			IViewPort::GET_SNAP_IN in_arg;
			in_arg.scrX = pX;
			in_arg.scrY = pY;
			in_arg.snapType = SNAP_SYSTEM;
			in_arg.XFix = m_normal_panel->IsXFixed();
			in_arg.YFix = m_normal_panel->IsYFixed();
			in_arg.ZFix = m_normal_panel->IsZFixed();
			in_arg.FixPoint.x = tmpFl[0];
			in_arg.FixPoint.y = tmpFl[1];
			in_arg.FixPoint.z = tmpFl[2];
			IViewPort::GET_SNAP_OUT out_arg;
			m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
			m_cur_pnt = out_arg.result_point;
			m_dir.x = m_cur_pnt.x - m_first_pnt.x;
			m_dir.y = m_cur_pnt.y - m_first_pnt.y;
			m_dir.z = m_cur_pnt.z - m_first_pnt.z;
			sgSpaceMath::NormalVector(m_dir);
			m_normal_panel->SetVector(IGetVectorPanel::USER_VECTOR,
				m_dir.x,m_dir.y,m_dir.z);
		}
		else
		{
			m_dir.x = tmpFl[0];m_dir.y = tmpFl[1];m_dir.z = tmpFl[2];
		}
	}
	if (m_step==2)
	{
		double plD;
		sgSpaceMath::PlaneFromNormalAndPoint(m_first_pnt,m_dir,plD);

		if (m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,m_dir,plD,m_cur_pnt))
		{
			m_rad_1 = m_app->ApplyPrecision(
				sgSpaceMath::PointsDistance(m_first_pnt,m_cur_pnt)
				);
		}
		else
			m_rad_1 = 1.0;
	
		ASSERT(m_r1_panel);

		m_r1_panel->SetNumber(m_rad_1);
	}
	if (m_step==3)
	{
		double plD;
		sgSpaceMath::PlaneFromNormalAndPoint(m_first_pnt,m_dir,plD);

		double rd;
		if (m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,m_dir,plD,m_cur_pnt))
		{
			rd = m_app->ApplyPrecision(
				sgSpaceMath::PointsDistance(m_first_pnt,m_cur_pnt)
				);
		}
		else
			rd = 1.0;
	
		ASSERT(m_r2_panel);

		m_rad_2 = fabs(m_rad_1-rd);
		m_r2_panel->SetNumber(m_rad_2);
		
	}
	m_app->GetViewPort()->InvalidateViewPort();

}

void  TorusCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	if (m_step==0)
	{
		ASSERT(m_base_point_panel);
		IViewPort::GET_SNAP_IN in_arg;
		in_arg.scrX = pX;
		in_arg.scrY = pY;
		in_arg.snapType = SNAP_SYSTEM;
		in_arg.XFix = m_base_point_panel->IsXFixed();
		in_arg.YFix = m_base_point_panel->IsYFixed();
		in_arg.ZFix = m_base_point_panel->IsZFixed();
		m_base_point_panel->GetPoint(in_arg.FixPoint.x,in_arg.FixPoint.y,in_arg.FixPoint.z);
		IViewPort::GET_SNAP_OUT out_arg;
		m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
		m_first_pnt = out_arg.result_point;
		if (out_arg.isOnWorkPlane)
		{
			m_dir = out_arg.snapWorkPlaneNormal;
			m_step+=2;
			m_app->GetCommandPanel()->EnableRadio(m_step-1,true);
			m_message.LoadString(IDS_TOR_ENTER_RAD);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		}
		else
			if (m_app->GetViewPort()->GetViewPortViewType()!=IViewPort::USER_VIEW)
			{
				switch(m_app->GetViewPort()->GetViewPortViewType()) 
				{
				case IViewPort::TOP_VIEW:
				case IViewPort::BOTTOM_VIEW:
					m_dir.x = m_dir.y = 0.0;
					m_dir.z = 1.0;
					break;
				case IViewPort::LEFT_VIEW:
				case IViewPort::RIGHT_VIEW:
					m_dir.x = m_dir.z = 0.0;
					m_dir.y = 1.0;
					break;
				case IViewPort::FRONT_VIEW:
				case IViewPort::BACK_VIEW:
					m_dir.y = m_dir.z = 0.0;
					m_dir.x = 1.0;
					break;
				default:
					ASSERT(0);
					break;
				}
				m_step+=2;
				m_app->GetCommandPanel()->EnableRadio(m_step-1,true);
				m_message.LoadString(IDS_TOR_ENTER_RAD);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			}
			else
			{
				m_step++;
				m_message.LoadString(IDS_TOR_ENTER_NORMAL);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			}
			m_app->GetCommandPanel()->SetActiveRadio(m_step);
	}
	else
		if (m_step==1)
		{
			ASSERT(m_normal_panel);

			m_normal_panel->GetVector(m_dir.x, m_dir.y, m_dir.z);
			if (sgSpaceMath::NormalVector(m_dir))
			{
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_TOR_ENTER_RAD);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			}
			else
			{
				m_message.LoadString(IDS_ERROR_ZERO_VECTOR);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
			}
		}
		else
			if (m_step==2)
			{
				if (fabs(m_rad_1)<0.00001)
				{
					m_message.LoadString(IDS_ERROR_ZERO_RAD);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				else
					if (m_rad_1<-0.0001)
					{
						m_message.LoadString(IDS_ERROR_RAD_MUST_BE_POSIT);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
						return;
					}
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_TOR_ENTER_TOLSH);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			}
			else
				if (m_step==3)
				{
					if (fabs(m_rad_2)<0.00001)
					{
						m_message.LoadString(IDS_ERROR_ZERO_TOLZCHINA);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
						return;
					}
					else
						if (m_rad_2<-0.0001)
						{
							m_message.LoadString(IDS_ERROR_TOLS_MUST_BE_POSIT);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,
								m_message);
							return;
						}
					if (m_rad_2>m_rad_1)
					{
						m_message.LoadString(IDS_ERROR_TOL_MUST_BE_LETTER_RAD);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
						return;
					}
					sgCTorus* tor = sgCreateTorus(fabs(m_rad_1),fabs(m_rad_2),24,24);
					if (!tor)
						return;
					tor->InitTempMatrix()->VectorToZAxe(m_dir);
					tor->GetTempMatrix()->Inverse();
					tor->GetTempMatrix()->Translate(m_first_pnt);
					tor->ApplyTempMatrix();
					tor->DestroyTempMatrix();

					CString nm;
					nm.LoadString(IDS_TOOLTIP_FOURTH);
					CString nmInd;
					nmInd.Format("%i",torus_name_index);
					nm+=nmInd;
					tor->SetName(nm);
					
					sgGetScene()->StartUndoGroup();
					sgGetScene()->AttachObject(tor);
					m_app->ApplyAttributes(tor);
					sgGetScene()->EndUndoGroup();

					torus_name_index++;
					m_app->GetViewPort()->InvalidateViewPort();
					NewScenar();
				}
}

void  TorusCommand::Draw()
{
	if (m_step==0)
	{
		float pC[3];
		m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
	}
	if (m_step==1)
	{
		ASSERT(m_normal_panel);

		double tmpFl[3];
		if (m_normal_panel->GetVector(tmpFl[0],tmpFl[1],tmpFl[2])==
			IGetVectorPanel::USER_VECTOR)
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
			SG_LINE ln;
			ln.p1 = m_first_pnt;
			ln.p2 = m_cur_pnt;
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawLine(ln);
			SG_CIRCLE crc;
			crc.center = m_first_pnt;
			crc.radius = m_app->GetViewPort()->GetGridSize();
			crc.normal = m_dir;
			m_app->GetViewPort()->GetPainter()->DrawCircle(crc);
		}
		else
		{
			SG_LINE ln;
			ln.p1 = m_first_pnt;
			ln.p2.x = m_first_pnt.x+m_dir.x*m_app->GetViewPort()->GetGridSize();
			ln.p2.y = m_first_pnt.y+m_dir.y*m_app->GetViewPort()->GetGridSize();
			ln.p2.z = m_first_pnt.z+m_dir.z*m_app->GetViewPort()->GetGridSize();
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);
			m_app->GetViewPort()->GetPainter()->DrawPoint(ln.p2);
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawLine(ln);
			SG_CIRCLE crc;
			crc.center = m_first_pnt;
			crc.radius = m_app->GetViewPort()->GetGridSize();
			crc.normal = m_dir;
			m_app->GetViewPort()->GetPainter()->DrawCircle(crc);
		}
	}
	if (m_step==2)
	{
		float pC[3];
		m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);

		SG_CIRCLE tmpCir;
		tmpCir.center = m_first_pnt;
		tmpCir.radius = m_rad_1;
		tmpCir.normal = m_dir;
		m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->DrawCircle(tmpCir);
	}
	if (m_step==3)
	{
		float pC[3];
		m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);

		SG_CIRCLE tmpCir;
		tmpCir.center = m_first_pnt;
		tmpCir.radius = fabs(m_rad_1-m_rad_2);
		tmpCir.normal = m_dir;
		m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->DrawCircle(tmpCir);
		tmpCir.radius = m_rad_1+m_rad_2;
		m_app->GetViewPort()->GetPainter()->DrawCircle(tmpCir);
		tmpCir.radius = m_rad_1;
		m_app->GetViewPort()->GetPainter()->DrawCircle(tmpCir);
	}
}

void  TorusCommand::OnEnter()
{
	SWITCH_RESOURCE
	if (m_step==0)
	{
		ASSERT(m_base_point_panel);

		m_base_point_panel->GetPoint(m_first_pnt.x,m_first_pnt.y,m_first_pnt.z);

		if (m_app->GetViewPort()->GetViewPortViewType()!=IViewPort::USER_VIEW)
		{
			switch(m_app->GetViewPort()->GetViewPortViewType()) 
			{
			case IViewPort::TOP_VIEW:
			case IViewPort::BOTTOM_VIEW:
				m_dir.x = m_dir.y = 0.0;
				m_dir.z = 1.0;
				break;
			case IViewPort::LEFT_VIEW:
			case IViewPort::RIGHT_VIEW:
				m_dir.x = m_dir.z = 0.0;
				m_dir.y = 1.0;
				break;
			case IViewPort::FRONT_VIEW:
			case IViewPort::BACK_VIEW:
				m_dir.y = m_dir.z = 0.0;
				m_dir.x = 1.0;
				break;
			default:
				ASSERT(0);
				break;
			}
			m_step+=2;
			m_app->GetCommandPanel()->EnableRadio(m_step-1,true);
			m_message.LoadString(IDS_TOR_ENTER_RAD);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		}
		else
		{
			m_step++;
			m_message.LoadString(IDS_TOR_ENTER_NORMAL);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		}
		m_app->GetCommandPanel()->SetActiveRadio(m_step);
	}
	else
		if (m_step==1)
		{
			ASSERT(m_normal_panel);

			m_normal_panel->GetVector(m_dir.x, m_dir.y, m_dir.z);

			if (sgSpaceMath::NormalVector(m_dir))
			{
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_TOR_ENTER_RAD);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			}
			else
			{
				m_message.LoadString(IDS_ERROR_ZERO_VECTOR);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
			}
		}
		else
			if (m_step==2)
			{
				ASSERT(m_r1_panel);
				double dbl = m_r1_panel->GetNumber();
				m_rad_1 = fabs(dbl);
				if (m_rad_1<0.00001)
				{
					m_message.LoadString(IDS_ERROR_ZERO_RAD);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				else
					if (dbl<-0.0001)
					{
						m_message.LoadString(IDS_ERROR_RAD_MUST_BE_POSIT);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
						return;
					}
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_TOR_ENTER_TOLSH);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			}
			else
				if (m_step==3)
				{
					ASSERT(m_r2_panel);
					double dbl = m_r2_panel->GetNumber();
					m_rad_2 = fabs(dbl);
					if (fabs(m_rad_2)<0.00001)
					{
						m_message.LoadString(IDS_ERROR_ZERO_TOLZCHINA);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
						return;
					}
					else
						if (dbl<-0.0001)
						{
							m_message.LoadString(IDS_ERROR_TOLS_MUST_BE_POSIT);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,
								m_message);
							return;
						}
					if (m_rad_2>m_rad_1)
						{
							m_message.LoadString(IDS_ERROR_TOL_MUST_BE_LETTER_RAD);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,
								m_message);
							return;
						}
					sgCTorus* tor = sgCreateTorus(fabs(m_rad_1),fabs(m_rad_2),24,24);
					if (!tor)
						return;
					tor->InitTempMatrix()->VectorToZAxe(m_dir);
					tor->GetTempMatrix()->Inverse();
					tor->GetTempMatrix()->Translate(m_first_pnt);
					tor->ApplyTempMatrix();
					tor->DestroyTempMatrix();

					CString nm;
					nm.LoadString(IDS_TOOLTIP_FOURTH);
					CString nmInd;
					nmInd.Format("%i",torus_name_index);
					nm+=nmInd;
					tor->SetName(nm);

					
					sgGetScene()->StartUndoGroup();
					sgGetScene()->AttachObject(tor);
					m_app->ApplyAttributes(tor);
					sgGetScene()->EndUndoGroup();
					torus_name_index++;
					m_app->GetViewPort()->InvalidateViewPort();
					NewScenar();
				}
}

unsigned int  TorusCommand::GetItemsCount()
{
	return 0;
}

void         TorusCommand::GetItem(unsigned int, CString&)
{
	SWITCH_RESOURCE
}

void     TorusCommand::GetItemState(unsigned int, bool&, bool&)
{
}

HBITMAP     TorusCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         TorusCommand::Run(unsigned int)
{
}


void         TorusCommand::NewScenar()
{
	SWITCH_RESOURCE
	m_app->GetCommandPanel()->RemoveAllDialogs();
	CString lab;
	lab.LoadString(IDS_CENTER);
	m_base_point_panel = reinterpret_cast<IGetPointPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,lab,true));
	
	lab.LoadString(IDS_NORMAL);
	m_normal_panel = reinterpret_cast<IGetVectorPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_VECTOR_DLG,lab,true));

	lab.LoadString(IDS_RADIUS);
	m_r1_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,lab,true));

	lab.LoadString(IDS_TOR_TOLZCHINA);
	m_r2_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,lab,true));
	
	lab.LoadString(IDS_SPH_ENTER_CENTER);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);

	m_app->GetCommandPanel()->SetActiveRadio(0);

	m_step=0;
}