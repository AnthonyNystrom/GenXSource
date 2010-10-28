#include "stdafx.h"

#include "SphericBand.h"

#include "..//resource.h"

#include <math.h>

 int     spheric_band_name_index=1;

SphericBandCommand::SphericBandCommand(IApplicationInterface* appI):
				m_app(appI)
				,m_base_point_panel(NULL)
				,m_r_panel(NULL)
				,m_k1_panel(NULL)
				,m_k2_panel(NULL)
				, m_step(0)
{
	ASSERT(m_app);
	m_rad = 0.0;
	m_matrix = new sgCMatrix;
}

SphericBandCommand::~SphericBandCommand()
{
	if (m_matrix)
		delete m_matrix;
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
}



bool    SphericBandCommand::PreTranslateMessage(MSG* pMsg)
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
			case 2:
				if (m_k1_panel)
					m_k1_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
				break;
			case 3:
				if (m_k2_panel)
					m_k2_panel->GetWindow()->SendMessage(pMsg->message,
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
	return false;;
}


void   SphericBandCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, void* params)
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
				m_message.LoadString(IDS_SPH_ENTER_RAD);
				break;
			case 2:
				m_message.LoadString(IDS_SP_B_ENTER_F_C);
				break;
			case 3:
				m_message.LoadString(IDS_SP_B_ENTER_S_C);
				break;
			default:
				ASSERT(0);
				break;
		}
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

	}
}

void  SphericBandCommand::Start()	
{
	NewScenar();
	SWITCH_RESOURCE
	CString nm;
	nm.LoadString(IDS_TOOLTIP_SIXTH);
	m_app->StartCommander(nm);
}

void  SphericBandCommand::MouseMove(unsigned int nFlags,int pX,int pY)
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
	case 2:
		{
			ASSERT(m_k1_panel);
			SG_VECTOR dir; dir.z = 1.0; dir.y = 0.0; dir.x = 0.0;
			if (!m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,m_first_pnt,dir,m_projection))
			{
				m_coef[0] = 0.0;
				m_k1_panel->SetNumber(m_coef[0]);
				break;
			}
			char sig = (((m_projection.x-m_first_pnt.x)*dir.x+
				(m_projection.y-m_first_pnt.y)*dir.y+
				(m_projection.z-m_first_pnt.z)*dir.z)>0)?1:-1;


			m_coef[0] = sgSpaceMath::PointsDistance(m_first_pnt,m_projection)*sig/m_rad;
			if (m_coef[0]>1.0) m_coef[0] = 1.0;
			if (m_coef[0]<-1.0) m_coef[0] = -1.0;
			m_projection.z = m_first_pnt.z+m_rad*m_coef[0];
			m_k1_panel->SetNumber(m_coef[0]);
			m_fir_projection = m_projection;
		}
		break;
	case 3:
		{
			ASSERT(m_k2_panel);

			SG_VECTOR dir; dir.z = 1.0; dir.y = 0.0; dir.x = 0.0;
			if (!m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,m_first_pnt,dir,m_projection))
			{
				m_coef[1] = 0.0;
				m_k2_panel->SetNumber(m_coef[1]);
				break;
			}
			char sig = (((m_projection.x-m_first_pnt.x)*dir.x+
				(m_projection.y-m_first_pnt.y)*dir.y+
				(m_projection.z-m_first_pnt.z)*dir.z)>0)?1:-1;


			m_coef[1] = sgSpaceMath::PointsDistance(m_first_pnt,m_projection)*sig/m_rad;
			if (m_coef[1]>1.0) m_coef[1] = 1.0;
			if (m_coef[1]<-1.0) m_coef[1] = -1.0;
			m_projection.z = m_first_pnt.z+m_rad*m_coef[1];
			m_k2_panel->SetNumber(m_coef[1]);
		}
		break;
	default:
		ASSERT(0);
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

void  SphericBandCommand::LeftClick(unsigned int nFlags,int pX,int pY)
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
			m_message.LoadString(IDS_SPH_ENTER_RAD);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
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
				m_rad = rd;
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_SP_B_ENTER_F_C);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			}
			break;
		case 2:
			{
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_SP_B_ENTER_S_C);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			}
			break;
		case 3:
			{
				if (fabs(m_coef[0]-m_coef[1])<0.0001)
				{
					m_message.LoadString(IDS_COEF_IS_EQ);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				if (m_coef[0]>=m_coef[1])
				{
					double ttt = m_coef[0];
					m_coef[0] = m_coef[1];
					m_coef[1] = ttt;
				}
				sgCSphericBand* spb = sgCreateSphericBand(m_rad,m_coef[0],m_coef[1],24);
				if (!spb)
					return;
				spb->InitTempMatrix()->Translate(m_first_pnt);
				spb->ApplyTempMatrix();
				spb->DestroyTempMatrix();
				CString nm;
				nm.LoadString(IDS_TOOLTIP_SIXTH);
				CString nmInd;
				nmInd.Format("%i",spheric_band_name_index);
				nm+=nmInd;
				spb->SetName(nm);
				sgGetScene()->StartUndoGroup();
				sgGetScene()->AttachObject(spb);
				m_app->ApplyAttributes(spb);
				sgGetScene()->EndUndoGroup();
				spheric_band_name_index++;
				m_app->GetViewPort()->InvalidateViewPort();
				NewScenar();
			}
			break;
		default:
			ASSERT(0);
			break;
	}
}

void  SphericBandCommand::Draw()
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
	case 2:
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_fir_projection);	
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			SG_LINE ln;
			ln.p1 = m_first_pnt;
			ln.p2 = m_fir_projection;
			m_app->GetViewPort()->GetPainter()->DrawLine(ln);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(m_matrix);
			m_app->GetViewPort()->GetPainter()->DrawSphericBand(m_rad,m_coef[0],m_coef[0]);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);
		}
		break;
	case 3:
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_fir_projection);	
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_projection);	
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			SG_LINE ln;
			ln.p1 = m_fir_projection;
			ln.p2 = m_projection;
			m_app->GetViewPort()->GetPainter()->DrawLine(ln);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(m_matrix);
			m_app->GetViewPort()->GetPainter()->DrawSphericBand(m_rad,m_coef[0],m_coef[1]);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);
		}
		break;
	default:
		ASSERT(0);
	}
}

void  SphericBandCommand::OnEnter()
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
				m_message.LoadString(IDS_SPH_ENTER_RAD);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
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
				m_rad = rd;
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_SP_B_ENTER_F_C);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			}
			break;
		case 2:
			{
				double rd = m_k1_panel->GetNumber();
				if (fabs(rd)>1.000)
				{
					m_message.LoadString(IDS_ERROR_COEF_RANGE);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				m_coef[0] = rd;
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_SP_B_ENTER_S_C);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			}
			break;
		case 3:
			{
				double rd = m_k2_panel->GetNumber();
				if (fabs(rd)>1.000)
				{
					m_message.LoadString(IDS_ERROR_COEF_RANGE);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				m_coef[1] = rd;

				if (fabs(m_coef[0]-m_coef[1])<0.0001)
				{
					m_message.LoadString(IDS_COEF_IS_EQ);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				if (m_coef[0]>=m_coef[1])
				{
					double ttt = m_coef[0];
					m_coef[0] = m_coef[1];
					m_coef[1] = ttt;
				}
				sgCSphericBand* spb = sgCreateSphericBand(m_rad,m_coef[0],m_coef[1],24);
				if (!spb)
					return;
				spb->InitTempMatrix()->Translate(m_first_pnt);
				spb->ApplyTempMatrix();
				spb->DestroyTempMatrix();
				CString nm;
				nm.LoadString(IDS_TOOLTIP_SIXTH);
				CString nmInd;
				nmInd.Format("%i",spheric_band_name_index);
				nm+=nmInd;
				spb->SetName(nm);
				sgGetScene()->StartUndoGroup();
				sgGetScene()->AttachObject(spb);
				m_app->ApplyAttributes(spb);
				sgGetScene()->EndUndoGroup();
				spheric_band_name_index++;
				m_app->GetViewPort()->InvalidateViewPort();
				NewScenar();
				
			}
			break;
		default:
			ASSERT(0);
			break;
	}
}

unsigned int  SphericBandCommand::GetItemsCount()
{
	return 0;
}

void         SphericBandCommand::GetItem(unsigned int, CString&)
{
	//SWITCH_RESOURCE
}

void     SphericBandCommand::GetItemState(unsigned int, bool&, bool&)
{
}

HBITMAP     SphericBandCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         SphericBandCommand::Run(unsigned int)
{
}


void  SphericBandCommand::NewScenar()
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
	

	lab.LoadString(IDS_FIRST_COEF);
	m_k1_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,lab,true));
	
	lab.LoadString(IDS_SECOND_COEF);
	m_k2_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,lab,true));
	
	m_app->GetCommandPanel()->SetActiveRadio(0);

	m_step=0;

	m_message.LoadString(IDS_SPH_ENTER_CENTER);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

	memset(m_coef,0,sizeof(double)*2);
}