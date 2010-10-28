#include "stdafx.h"

#include "Circle.h"

#include "..//resource.h"


#include <math.h>

int     circle_name_index = 1;

CircleCommand::CircleCommand(IApplicationInterface* appI):
						m_app(appI)
						,m_get_center_panel(NULL)
						,m_get_normal_panel(NULL)
						,m_get_r_panel(NULL)
						,m_exist_circ_data(false)
						, m_scenario(0)
						, m_step(0)
						, m_bitmaps(NULL)
{
	ASSERT(m_app);
}

CircleCommand::~CircleCommand()
{
    m_app->GetCommandPanel()->RemoveAllDialogs();
	if (m_bitmaps)
	{
		for (int i=0;i<4;i++)
			if (m_bitmaps[i].m_hObject)
				m_bitmaps[i].DeleteObject();
		delete [] m_bitmaps;
	}
	m_app->GetViewPort()->InvalidateViewPort();
}


bool    CircleCommand::PreTranslateMessage(MSG* pMsg)
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
			switch(m_step) 
			{
			case 0:
				if (m_get_center_panel)
					m_get_center_panel->GetWindow()->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
				break;
			case 1:
				if (m_get_normal_panel)
					m_get_normal_panel->GetWindow()->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
				break;
			case 2:
				if (m_get_r_panel)
					m_get_r_panel->GetWindow()->SendMessage(pMsg->message,
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


void     CircleCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
										  void* params) 
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=2);

		m_step = (unsigned int)newActiveDlg;

		for (unsigned int i=m_step+1;i<3;i++)
			m_app->GetCommandPanel()->EnableRadio(i,false);

		switch(m_step) {
		case 0:
			m_message.LoadString(IDS_CIR_ENTER_CENTER);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
				m_message);
			break;
		case 1:
			m_message.LoadString(IDS_CIR_ENTER_NORMAL);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
				m_message);
			break;
		case 2:
			m_message.LoadString(IDS_CIR_ENTER_RAD);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
				m_message);
			break;
		default:
			ASSERT(0);
			break;
		}

		m_app->GetViewPort()->InvalidateViewPort();
		return;
	}
}


void  CircleCommand::Start()	
{
	SWITCH_RESOURCE

		CString lab;
	lab.LoadString(IDS_TOOLTIP_SECOND);
	m_app->StartCommander(lab);

	CircleRadCenNormScenario();

	/*m_bitmaps = new CBitmap[4];
	m_bitmaps[0].LoadBitmap(IDB_CIRC_CEN_RAD);
	m_bitmaps[1].LoadBitmap(IDB_CIRC_CEN_RAD_CH);
	m_bitmaps[2].LoadBitmap(IDB_CIRC_3P);
	m_bitmaps[3].LoadBitmap(IDB_CIRC_3P_CH);*/
}

void  CircleCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	switch(m_scenario) 
	{
	case 0:
		{
			if (m_step==0)
			{
				ASSERT(m_get_center_panel);
				IViewPort::GET_SNAP_IN in_arg;
				in_arg.scrX = pX;
				in_arg.scrY = pY;
				in_arg.snapType = SNAP_SYSTEM;
				in_arg.XFix = m_get_center_panel->IsXFixed();
				in_arg.YFix = m_get_center_panel->IsYFixed();
				in_arg.ZFix = m_get_center_panel->IsZFixed();
				m_get_center_panel->GetPoint(in_arg.FixPoint.x,
					in_arg.FixPoint.y,
					in_arg.FixPoint.z);
				IViewPort::GET_SNAP_OUT out_arg;
				m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
				m_cur_pnt = out_arg.result_point;
				m_get_center_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
			}
			if (m_step==1)
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
					m_dir.x = m_cur_pnt.x - m_first_pnt.x;
					m_dir.y = m_cur_pnt.y - m_first_pnt.y;
					m_dir.z = m_cur_pnt.z - m_first_pnt.z;
					sgSpaceMath::NormalVector(m_dir);
					m_get_normal_panel->SetVector(IGetVectorPanel::USER_VECTOR,
						m_dir.x,m_dir.y,m_dir.z);
				}
				else
				{
					SG_LINE ln;
					ln.p1 = m_first_pnt;
					m_cur_pnt.x = m_first_pnt.x+tmpFl[0];
					m_cur_pnt.y = m_first_pnt.y+tmpFl[1];
					m_cur_pnt.z = m_first_pnt.z+tmpFl[2];
					m_dir.x = tmpFl[0];m_dir.y = tmpFl[1];m_dir.z = tmpFl[2];
				}
			}
			if (m_step==2)
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
		
				ASSERT(m_get_r_panel);
				
				m_get_r_panel->SetNumber(rd);
				m_circle_geo_data.center = m_first_pnt;
				m_circle_geo_data.radius = rd;
				m_circle_geo_data.normal = m_dir;
				m_exist_circ_data = true;
			}
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	/*case 1:
		{
			IGetPointPanel*  gpd = m_panel->GetCurrentGetPointPanel();
			ASSERT(gpd);
			IViewPort::GET_SNAP_IN in_arg;
			in_arg.scrX = pX;
			in_arg.scrY = pY;
			in_arg.snapType = SNAP_SYSTEM;
			in_arg.XFix = gpd->IsXFixed();in_arg.YFix = gpd->IsYFixed();in_arg.ZFix = gpd->IsZFixed();
			gpd->GetPoint(in_arg.FixPoint.x,in_arg.FixPoint.y,in_arg.FixPoint.z);
			IViewPort::GET_SNAP_OUT out_arg;
			m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
			m_cur_pnt = out_arg.result_point;
			gpd->SetPoint((float)(m_cur_pnt.x),(float)(m_cur_pnt.y),(float)(m_cur_pnt.z));
			
			if (m_step==2)
			{
				m_exist_circ_data = SG_CIRCLE::FromThreePoints(m_first_pnt,
																		m_second_pnt,
																		m_cur_pnt,
																		m_circle_geo_data);
			}	
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;*/
	default:
		ASSERT(0);
		break;
	}
}

void  CircleCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	switch(m_scenario) 
	{
	case 0:
		{
			if (m_step==0)
			{
				ASSERT(m_get_center_panel);
				IViewPort::GET_SNAP_IN in_arg;
				in_arg.scrX = pX;
				in_arg.scrY = pY;
				in_arg.snapType = SNAP_SYSTEM;
				in_arg.XFix = m_get_center_panel->IsXFixed();
				in_arg.YFix = m_get_center_panel->IsYFixed();
				in_arg.ZFix = m_get_center_panel->IsZFixed();
				m_get_center_panel->GetPoint(in_arg.FixPoint.x,in_arg.FixPoint.y,in_arg.FixPoint.z);
				IViewPort::GET_SNAP_OUT out_arg;
				m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
				m_first_pnt = out_arg.result_point;
				if (out_arg.isOnWorkPlane)
				{
					m_dir = out_arg.snapWorkPlaneNormal;
					m_step+=2;
					m_app->GetCommandPanel()->SetActiveRadio(m_step-1);
					m_message.LoadString(IDS_CIR_ENTER_RAD);
					m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
						m_message);
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
						m_app->GetCommandPanel()->SetActiveRadio(m_step-1);
						m_message.LoadString(IDS_CIR_ENTER_RAD);
						m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
							m_message);
					}
					else
					{
						m_step++;
						m_message.LoadString(IDS_CIR_ENTER_NORMAL);
						m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
							m_message);
					}
					m_app->GetCommandPanel()->EnableRadio(m_step,true);
					m_app->GetCommandPanel()->SetActiveRadio(m_step);
			}
			else
				if (m_step==1)
				{
					ASSERT(m_get_normal_panel);

					m_get_normal_panel->GetVector(m_dir.x, m_dir.y, m_dir.z);
					if (sgSpaceMath::NormalVector(m_dir))
					{
						m_step++;
						m_app->GetCommandPanel()->SetActiveRadio(m_step);
						m_message.LoadString(IDS_CIR_ENTER_RAD);
						m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
							m_message);
					}
					else
					{
						m_message.LoadString(IDS_ERROR_ZERO_VECTOR);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
					}
				}
				else
					if (m_step==2 && m_exist_circ_data)
					{
						if (fabs(m_circle_geo_data.radius)>0.00001)
						{
							sgCCircle* cr = sgCreateCircle(m_circle_geo_data);
							if (!cr)
								return;
							CString nm;
							nm.LoadString(IDS_TOOLTIP_SECOND);
							CString nmInd;
							nmInd.Format("%i",circle_name_index);
							nm+=nmInd;
							cr->SetName(nm.GetBuffer());
							sgGetScene()->StartUndoGroup();
							sgGetScene()->AttachObject(cr);
							m_app->ApplyAttributes(cr);
							sgGetScene()->EndUndoGroup();
							circle_name_index++;
							
							m_app->GetViewPort()->InvalidateViewPort();
							m_step=0;
							m_exist_circ_data = false;
							CircleRadCenNormScenario();
						}
						else
						{
							m_message.LoadString(IDS_ERROR_ZERO_RADIUS);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,
								m_message);
						}
					}
		}
		break;
	/*case 1:
		{
			if (m_step==0)
			{
				m_first_pnt=m_cur_pnt;
				m_step++;
				m_panel->EnablePage(m_step,true);
				m_panel->SetActivePage(m_step);
			}
			else
				if (m_step==1)
				{
					m_second_pnt=m_cur_pnt;
					m_step++;
					m_panel->EnablePage(m_step,true);
					m_panel->SetActivePage(m_step);
				}
				else
					if (m_step==2 && m_exist_circ_data)
					{
						sgCCircle* cr = sgCreateCircle(&m_circle_geo_data);
						CString nm;
						nm.LoadString(IDS_TOOLTIP_SECOND);
						CString nmInd;
						nmInd.Format("%i",circle_name_index);
						nm+=nmInd;
						cr->SetName(nm.GetBuffer());
						sgGetScene()->StartUndoGroup();
						sgGetScene()->AttachObject(cr);
						m_app->ApplyAttributes(cr);
						sgGetScene()->EndUndoGroup();
						circle_name_index++;
						
						m_app->GetViewPort()->InvalidateViewPort();
						m_step=0;
						m_exist_circ_data = false;
						CircleThreePointsScenario();
					}
					else
						if (!m_exist_circ_data)
						{
							m_message.LoadString(IDS_ERROR_INFIN_RAD);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,
								m_message);
						}
		}
		break;*/
	default:
		ASSERT(0);
		break;
	}
}

void  CircleCommand::Draw()
{
	SWITCH_RESOURCE
		switch(m_scenario) 
	{
		case 0:
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
					float pC[3];
					m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
					m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
					m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
					m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);
					SG_LINE ln;
					ln.p1 = m_first_pnt;
					ln.p2 = m_cur_pnt;
					m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
					m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
					m_app->GetViewPort()->GetPainter()->DrawLine(ln);
					SG_CIRCLE crc;
					crc.center = m_first_pnt;
					crc.radius = (double)m_app->GetViewPort()->GetGridSize();
					crc.normal = m_dir;
					m_app->GetViewPort()->GetPainter()->DrawCircle(crc);
				}
				if (m_step==2)
				{
					float pC[3];
					m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
					m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
					m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);
					if (m_exist_circ_data)
					{
						m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
						m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
						m_app->GetViewPort()->GetPainter()->DrawCircle(m_circle_geo_data);
					}
				}
			}
			break;
	/*	case 1:
			{
				float pC[3];
				m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);

				if (m_step==1)
				{
					m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);
					SG_LINE ln;
					ln.p1 = m_first_pnt;
					ln.p2 = m_cur_pnt;
					m_app->GetViewPort()->GetPainter()->DrawLine(ln);
				}
				if (m_step==2)
				{
					m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);
					m_app->GetViewPort()->GetPainter()->DrawPoint(m_second_pnt);
					if (m_exist_circ_data)
					{
						m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
						m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
						m_app->GetViewPort()->GetPainter()->DrawCircle(m_circle_geo_data);
					}
				}
			}
			break;*/
		default:
			ASSERT(0);
			break;
	}
}

void  CircleCommand::OnEnter()
{
	
	SWITCH_RESOURCE
	switch(m_scenario) 
	{
	case 0:
		{
			if (m_step==0)
			{
				ASSERT(m_get_center_panel);

				m_get_center_panel->GetPoint(m_first_pnt.x,m_first_pnt.y,m_first_pnt.z);
				
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
					m_message.LoadString(IDS_CIR_ENTER_RAD);
					m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
						m_message);
				}
				else
				{
					m_step++;
					m_message.LoadString(IDS_CIR_ENTER_NORMAL);
					m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
						m_message);
				}
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
			}
			else
				if (m_step==1)
				{
					ASSERT(m_get_normal_panel);

					m_get_normal_panel->GetVector(m_dir.x, m_dir.y, m_dir.z);
				
					if (sgSpaceMath::NormalVector(m_dir))
					{
						m_step++;
						m_app->GetCommandPanel()->SetActiveRadio(m_step);
						m_message.LoadString(IDS_CIR_ENTER_RAD);
						m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
							m_message);
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
						ASSERT(m_get_r_panel);
						double dbl = m_get_r_panel->GetNumber();
						if (fabs(dbl)>0.00001)
						{
							m_circle_geo_data.center = m_first_pnt;
							m_circle_geo_data.radius = m_get_r_panel->GetNumber();
							m_circle_geo_data.normal = m_dir;

							sgCCircle* cr = sgCreateCircle(m_circle_geo_data);
							if (!cr)
								return;
							CString nm;
							nm.LoadString(IDS_TOOLTIP_SECOND);
							CString nmInd;
							nmInd.Format("%i",circle_name_index);
							nm+=nmInd;
							cr->SetName(nm.GetBuffer());
							sgGetScene()->StartUndoGroup();
							sgGetScene()->AttachObject(cr);
							m_app->ApplyAttributes(cr);
							sgGetScene()->EndUndoGroup();
							circle_name_index++;
							
							m_app->GetViewPort()->InvalidateViewPort();
							m_step=0;
							m_exist_circ_data = false;
							CircleRadCenNormScenario();
						}
						else
						{
							m_message.LoadString(IDS_ERROR_ZERO_RADIUS);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,
								m_message);
						}
					}
		}
		break;
	/*case 1:
		{
			if (m_step==0)
			{
				IGetPointPanel*  gpd = m_panel->GetCurrentGetPointPanel();
				ASSERT(gpd);

				gpd->GetPoint(m_first_pnt.x,m_first_pnt.y,m_first_pnt.z);
				m_step++;
				m_panel->EnablePage(m_step,true);
				m_panel->SetActivePage(m_step);
			}
			else
				if (m_step==1)
				{
					IGetPointPanel*  gpd = m_panel->GetCurrentGetPointPanel();
					ASSERT(gpd);

					gpd->GetPoint(m_second_pnt.x,m_second_pnt.y,m_second_pnt.z);
		
					m_step++;
					m_panel->EnablePage(m_step,true);
					m_panel->SetActivePage(m_step);
				}
				else
					if (m_step==2)
					{
						IGetPointPanel*  gpd = m_panel->GetCurrentGetPointPanel();
						ASSERT(gpd);

						SG_POINT tmpPn;
						gpd->GetPoint(tmpPn.x,tmpPn.y,tmpPn.z);
						if (SG_CIRCLE::FromThreePoints(m_first_pnt,
																		m_second_pnt,
																		tmpPn,
																		m_circle_geo_data))
						{
							sgCCircle* cr = sgCreateCircle(&m_circle_geo_data);
							CString nm;
							nm.LoadString(IDS_TOOLTIP_SECOND);
							CString nmInd;
							nmInd.Format("%i",circle_name_index);
							nm+=nmInd;
							cr->SetName(nm.GetBuffer());
							sgGetScene()->StartUndoGroup();
							sgGetScene()->AttachObject(cr);
							m_app->ApplyAttributes(cr);
							sgGetScene()->EndUndoGroup();
							circle_name_index++;
							
							m_app->GetViewPort()->InvalidateViewPort();
							m_step=0;
							m_exist_circ_data = false;
							CircleThreePointsScenario();
						}
						else
						{
							m_message.LoadString(IDS_ERROR_INFIN_RAD);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,
								m_message);
							m_exist_circ_data = false;
						}
					}
		}
		break;*/
	default:
		ASSERT(0);
		break;
	}
}

unsigned int  CircleCommand::GetItemsCount()
{
	return 0;
}

void         CircleCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	/*SWITCH_RESOURCE
	switch(itemID) {
	case 0:
		itSrt.LoadString(IDS_CIRC_SCENAR_CEN_RAD_NORM);
		break;
	case 1:
		itSrt.LoadString(IDS_CIRC_SCENAR_THREE_POINTS);
		break;
	default:
		ASSERT(0);
	}*/
}

void     CircleCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	/*enbl = true;
	checked = false;
	switch(itemID) {
	case 0:
		if (m_scenario==0)
			checked = true;
		break;
	case 1:
		if (m_scenario==1)
			checked = true;
		break;
	default:
		ASSERT(0);
	}*/
}

HBITMAP   CircleCommand::GetItemBitmap(unsigned int itemId)
{
	/*if (itemId==0) 
	{
		return &m_bitmaps[0];
	}
	else
		if (itemId==1)
			return &m_bitmaps[2];
		else
		{
			ASSERT(0);*/
			return NULL;
		//}
}

void         CircleCommand::Run(unsigned int itemID)
{
	/*switch(itemID) {
	case 0:
		CircleRadCenNormScenario();
		break;
	case 1:
		CircleThreePointsScenario();
		break;
	default:
		ASSERT(0);
	}*/
}

void CircleCommand::CircleRadCenNormScenario()
{
	SWITCH_RESOURCE
		
	m_app->GetCommandPanel()->RemoveAllDialogs();
	
	CString lab;
	lab.LoadString(IDS_CENTER);
	m_get_center_panel = 
		reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
		lab,true));
	
	lab.LoadString(IDS_NORMAL);
	m_get_normal_panel = 
		reinterpret_cast<IGetVectorPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_VECTOR_DLG,
		lab,true));


	lab.LoadString(IDS_RADIUS);
	m_get_r_panel =
		reinterpret_cast<IGetNumberPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,
		lab,true));
	
	m_app->GetCommandPanel()->SetActiveRadio(0);

	lab.LoadString(IDS_CIR_ENTER_CENTER);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
		lab);

	m_scenario = 0;
	m_step=0;
}

void CircleCommand::CircleThreePointsScenario()
{
	/*SWITCH_RESOURCE
	ASSERT(m_panel);
	m_panel->RemoveAllPages();
	CString lab;
	lab.LoadString(IDS_FIRST_POINT);
	m_panel->AddPage(lab,IBasePanel::GET_POINT_DLG);
	m_panel->EnablePage(0,true);
	lab.LoadString(IDS_SECOND_POINT);
	m_panel->AddPage(lab,IBasePanel::GET_POINT_DLG);
	m_panel->EnablePage(1,false);
	lab.LoadString(IDS_THIRD_POINT);
	m_panel->AddPage(lab,IBasePanel::GET_POINT_DLG);
	m_panel->EnablePage(2,false);

	//m_panel->SetActivePage(1);
	m_panel->SetActivePage(0);

	m_scenario =1;
	m_step=0;*/
}


