#include "stdafx.h"

#include "Arc.h"

#include "..//resource.h"
#include <math.h>

int     arc_name_index = 1;

ArcCommand::ArcCommand(IApplicationInterface* appI):
						m_app(appI)
						,m_get_first_point_panel(NULL)
						,m_get_second_point_panel(NULL)
						,m_get_third_point_panel(NULL)
						, m_invert(false)
						, m_exist_arc_data(false)
						, m_scenario(0)
						, m_step(0)
						, m_other_line(NULL)
						, m_other_arc(NULL)
{
	ASSERT(m_app);
}

ArcCommand::~ArcCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
}


bool    ArcCommand::PreTranslateMessage(MSG* pMsg)
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
				if (m_get_first_point_panel)
					m_get_first_point_panel->GetWindow()->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
				break;
			case 1:
				if (m_get_second_point_panel)
					m_get_second_point_panel->GetWindow()->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
				break;
			case 2:
				if (m_get_third_point_panel)
					m_get_third_point_panel->GetWindow()->SendMessage(pMsg->message,
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

void     ArcCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
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


		m_app->GetViewPort()->InvalidateViewPort();
		return;
	}
}

void  ArcCommand::Start()	
{
	SWITCH_RESOURCE

	CString lab;
	lab.LoadString(IDS_TOOLTIP_THIRD);
	m_app->StartCommander(lab);

	Arc_b_e_m();
}

void  ArcCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	switch(m_scenario) 
	{
	case 0:
		{
			IGetPointPanel* this_step = NULL;
			switch(m_step) 
			{
			case 0:
				this_step = m_get_first_point_panel;
   				break;
			case 1:
				this_step = m_get_second_point_panel;
				break;
			case 2:
				this_step = m_get_third_point_panel;
				break;
			default:
				ASSERT(0);
				break;
			}
			IViewPort::GET_SNAP_IN in_arg;
			in_arg.scrX = pX;
			in_arg.scrY = pY;
			in_arg.snapType = SNAP_SYSTEM;
			in_arg.XFix = this_step->IsXFixed();
			in_arg.YFix = this_step->IsYFixed();
			in_arg.ZFix = this_step->IsZFixed();
			this_step->GetPoint(in_arg.FixPoint.x,in_arg.FixPoint.y,in_arg.FixPoint.z);
			IViewPort::GET_SNAP_OUT out_arg;
			m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
			m_cur_pnt = out_arg.result_point;
			this_step->SetPoint((float)(m_cur_pnt.x),(float)(m_cur_pnt.y),(float)(m_cur_pnt.z));
	
			if (m_step==2)
			{
				m_exist_arc_data = m_arc_geo_data.FromThreePoints(m_first_pnt,
										m_second_pnt,
										m_cur_pnt,
										m_invert);
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
				m_exist_arc_data = sgCArc::CreateArcGeoFrom_c_b_e(m_first_pnt,
										m_second_pnt,
										m_cur_pnt,
										m_invert,
										m_arc_geo_data);
			}
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	case 2:
		{
			SWITCH_RESOURCE
			switch(m_step) {
						case 0:
							{	
								IGetStringPanel*  gsd = m_panel->GetCurrentGetStringPanel();
								ASSERT(gsd);
								const int sz = m_app->GetViewPort()->GetSnapSize();
								sgCObject* hO = m_app->GetViewPort()->GetTopObjectByType(
									m_app->GetViewPort()->GetHitsInRect(CRect(pX-sz, pY-sz,
									pX+sz, pY+sz),true),sgCObject::ARC_OBJ);
								m_app->GetViewPort()->SetHotObject(hO);
								if (!hO)
								{
									gsd->SetString("     ");
									m_other_arc = NULL;
								}
								else
								{
									if ((hO->GetType()==sgCObject::ARC_OBJ))
									{
										m_other_arc = reinterpret_cast<sgCArc*>(hO);
										m_message.LoadString(IDS_STBAR_THIRD);
										m_message+=":  ";
										m_message+=m_other_arc->GetName();
										gsd->SetString(m_message);
									}
									else
									{
										ASSERT(0);
										gsd->SetString("ERROR");
										m_other_arc = NULL;
									}
								}
							}
							break;
						case 1:
							{	
								IGetStringPanel*  gsd = m_panel->GetCurrentGetStringPanel();
								ASSERT(gsd);
								ASSERT(m_other_arc);
								const SG_ARC* arcGeo = m_other_arc->GetGeometry();
								double    winX1,winY1,winZ1;
								double    winX2,winY2,winZ2;
								m_app->GetViewPort()->ProjectWorldPoint(arcGeo->begin,winX1,winY1,winZ1);
								m_app->GetViewPort()->ProjectWorldPoint(arcGeo->end,winX2,winY2,winZ2);
								double d1;
								d1 = sqrt((winX1-pX)*(winX1-pX)+(winY1-pY)*(winY1-pY));
								double d2;
								d2 = sqrt((winX2-pX)*(winX2-pX)+(winY2-pY)*(winY2-pY));
								if (d1<d2)
								{
									m_message.LoadString(IDS_FIRST_POINT);
									gsd->SetString(m_message);
									m_first_pnt = arcGeo->begin;
									m_dir.x = m_first_pnt.x - arcGeo->center.x;
									m_dir.y = m_first_pnt.y - arcGeo->center.y;
									m_dir.z = m_first_pnt.z - arcGeo->center.z;
									
									sgSpaceMath::NormalVector(&m_dir);
								}
								else
								{
									m_message.LoadString(IDS_SECOND_POINT);
									gsd->SetString(m_message);
									m_first_pnt = arcGeo->end;
									m_dir.x = m_first_pnt.x - arcGeo->center.x;
									m_dir.y = m_first_pnt.y - arcGeo->center.y;
									m_dir.z = m_first_pnt.z - arcGeo->center.z;
									
									sgSpaceMath::NormalVector(&m_dir);
								}
							}
							break;
						case 2:
							{
								double plD;
								SG_VECTOR nrml = m_other_arc->GetGeometry()->normal;
								sgSpaceMath::PlaneFromNormalAndPoint(&m_first_pnt,
									&nrml,&plD);
								m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,
									nrml, plD, m_projection);
								m_cur_pnt = m_projection;
								sgSpaceMath::ProjectPointToLineAndGetDist(&m_first_pnt,
									&m_dir,
									&m_cur_pnt,
									&m_projection);
								char sig = (((m_projection.x-m_first_pnt.x)*m_dir.x+
									(m_projection.y-m_first_pnt.y)*m_dir.y+
									(m_projection.z-m_first_pnt.z)*m_dir.z)>0)?1:-1;

								IGetNumberPanel*  gnd = m_panel->GetCurrentGetNumberPanel();
								ASSERT(gnd);
								m_dist = sgSpaceMath::PointsDistance(&m_first_pnt,&m_projection);
								gnd->SetNumber(m_dist*sig);
								m_cur_pnt = m_projection;
							}
							break;
						case 3:
							{
								IGetPointPanel*  gpd = m_panel->GetCurrentGetPointPanel();
								ASSERT(gpd);
							
								double plD;
								SG_VECTOR nrml = m_other_arc->GetGeometry()->normal;
								sgSpaceMath::PlaneFromNormalAndPoint(&m_first_pnt,
									&nrml,&plD);
								m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,
									nrml, plD, m_projection);
								m_cur_pnt = m_projection;
								gpd->SetPoint((float)(m_cur_pnt.x),(float)(m_cur_pnt.y),(float)(m_cur_pnt.z));
								m_exist_arc_data = sgCArc::CreateArcGeoFrom_c_b_e(m_second_pnt,
												m_first_pnt,
												m_cur_pnt,
												m_invert,
												m_arc_geo_data);
							}
							break;
						default:
							ASSERT(0);
								}
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	case 3:
		{
			SWITCH_RESOURCE
				switch(m_step) {
						case 0:
							{	
								IGetStringPanel*  gsd = m_panel->GetCurrentGetStringPanel();
								ASSERT(gsd);
								const int sz = m_app->GetViewPort()->GetSnapSize();
								sgCObject* hO = m_app->GetViewPort()->GetTopObjectByType(
									m_app->GetViewPort()->GetHitsInRect(CRect(pX-sz, pY-sz,
									pX+sz, pY+sz),true),sgCObject::ARC_OBJ);
								m_app->GetViewPort()->SetHotObject(hO);
								if (!hO)
								{
									gsd->SetString("     ");
									m_other_arc = NULL;
								}
								else
								{
									if ((hO->GetType()==sgCObject::ARC_OBJ))
									{
										m_other_arc = reinterpret_cast<sgCArc*>(hO);
										m_message.LoadString(IDS_STBAR_THIRD);
										m_message+=":  ";
										m_message+=m_other_arc->GetName();
										gsd->SetString(m_message);
									}
									else
									{
										ASSERT(0);
										gsd->SetString("ERROR");
										m_other_arc = NULL;
									}
								}
							}
							break;
						case 1:
							{	
								IGetStringPanel*  gsd = m_panel->GetCurrentGetStringPanel();
								ASSERT(gsd);
								ASSERT(m_other_arc);
								const SG_ARC* arcGeo = m_other_arc->GetGeometry();
								double    winX1,winY1,winZ1;
								double    winX2,winY2,winZ2;
								m_app->GetViewPort()->ProjectWorldPoint(arcGeo->begin,winX1,winY1,winZ1);
								m_app->GetViewPort()->ProjectWorldPoint(arcGeo->end,winX2,winY2,winZ2);
								double d1;
								d1 = sqrt((winX1-pX)*(winX1-pX)+(winY1-pY)*(winY1-pY));
								double d2;
								d2 = sqrt((winX2-pX)*(winX2-pX)+(winY2-pY)*(winY2-pY));
								if (d1<d2)
								{
									m_message.LoadString(IDS_FIRST_POINT);
									gsd->SetString(m_message);
									m_first_pnt = arcGeo->begin;
									SG_VECTOR tmpV;
									tmpV.x = m_first_pnt.x - arcGeo->center.x;
									tmpV.y = m_first_pnt.y - arcGeo->center.y;
									tmpV.z = m_first_pnt.z - arcGeo->center.z;
									m_dir = sgSpaceMath::VectorsVectorMult(const_cast<SG_VECTOR*>(&arcGeo->normal),
												&tmpV);
									sgSpaceMath::NormalVector(&m_dir);
								}
								else
								{
									m_message.LoadString(IDS_SECOND_POINT);
									gsd->SetString(m_message);
									m_first_pnt = arcGeo->end;
									SG_VECTOR tmpV;
									tmpV.x = m_first_pnt.x - arcGeo->center.x;
									tmpV.y = m_first_pnt.y - arcGeo->center.y;
									tmpV.z = m_first_pnt.z - arcGeo->center.z;
									m_dir = sgSpaceMath::VectorsVectorMult(&tmpV,
										const_cast<SG_VECTOR*>(&arcGeo->normal));
									sgSpaceMath::NormalVector(&m_dir);
								}
							}
							break;
						case 2:
							{
								double plD;
								SG_VECTOR nrml = m_other_arc->GetGeometry()->normal;
								sgSpaceMath::PlaneFromNormalAndPoint(&m_first_pnt,
									&nrml,&plD);
								m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,
									nrml, plD, m_projection);
								m_cur_pnt = m_projection;
								sgSpaceMath::ProjectPointToLineAndGetDist(&m_first_pnt,
									&m_dir,
									&m_cur_pnt,
									&m_projection);
								char sig = (((m_projection.x-m_first_pnt.x)*m_dir.x+
									(m_projection.y-m_first_pnt.y)*m_dir.y+
									(m_projection.z-m_first_pnt.z)*m_dir.z)>0)?1:-1;

								IGetNumberPanel*  gnd = m_panel->GetCurrentGetNumberPanel();
								ASSERT(gnd);
								m_dist = sgSpaceMath::PointsDistance(&m_first_pnt,&m_projection);
								gnd->SetNumber(m_dist*sig);
								m_cur_pnt = m_projection;
							}
							break;
						case 3:
							{
								IGetPointPanel*  gpd = m_panel->GetCurrentGetPointPanel();
								ASSERT(gpd);
								double plD;
								SG_VECTOR nrml = m_other_arc->GetGeometry()->normal;
								sgSpaceMath::PlaneFromNormalAndPoint(&m_first_pnt,
									&nrml,&plD);
								m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,
									nrml, plD, m_projection);
								m_cur_pnt = m_projection;
								gpd->SetPoint((float)(m_cur_pnt.x),(float)(m_cur_pnt.y),(float)(m_cur_pnt.z));
								
								m_exist_arc_data = sgCArc::CreateArcGeoFrom_c_b_e(m_second_pnt,
									m_first_pnt,
									m_cur_pnt,
									m_invert,
									m_arc_geo_data);
							}
							break;
						default:
							ASSERT(0);
					}
				m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	case 4:
	case 5:
		{
			SWITCH_RESOURCE
				switch(m_step) {
					case 0:
						{	
							IGetStringPanel*  gsd = m_panel->GetCurrentGetStringPanel();
							ASSERT(gsd);
							const int sz = m_app->GetViewPort()->GetSnapSize();
							sgCObject* hO = m_app->GetViewPort()->GetTopObjectByType(
								m_app->GetViewPort()->GetHitsInRect(CRect(pX-sz, pY-sz,
								pX+sz, pY+sz),true),sgCObject::LINE_OBJ);
							m_app->GetViewPort()->SetHotObject(hO);
							if (!hO)
							{
								gsd->SetString("     ");
								m_other_line = NULL;
							}
							else
							{
								if ((hO->GetType()==sgCObject::LINE_OBJ))
								{
									m_other_line = reinterpret_cast<sgCLine*>(hO);
									m_message.LoadString(IDS_STBAR_FIRST);
									m_message+=":  ";
									m_message+=m_other_line->GetName();
									gsd->SetString(m_message);
								}
								else
								{
									ASSERT(0);
									gsd->SetString("ERROR");
									m_other_line = NULL;
								}
							}
						}
						break;
					case 1:
						{	
							IGetStringPanel*  gsd = m_panel->GetCurrentGetStringPanel();
							ASSERT(gsd);
							ASSERT(m_other_line);
							const SG_LINE* lineGeo = m_other_line->GetGeometry();
							double    winX1,winY1,winZ1;
							double    winX2,winY2,winZ2;
							m_app->GetViewPort()->ProjectWorldPoint(lineGeo->p1,winX1,winY1,winZ1);
							m_app->GetViewPort()->ProjectWorldPoint(lineGeo->p2,winX2,winY2,winZ2);
							double d1;
							d1 = sqrt((winX1-pX)*(winX1-pX)+(winY1-pY)*(winY1-pY));
							double d2;
							d2 = sqrt((winX2-pX)*(winX2-pX)+(winY2-pY)*(winY2-pY));
							if (d1<d2)
							{
								m_message.LoadString(IDS_FIRST_POINT);
								gsd->SetString(m_message);
								m_first_pnt = lineGeo->p1;
							}
							else
							{
								m_message.LoadString(IDS_SECOND_POINT);
								gsd->SetString(m_message);
								m_first_pnt = lineGeo->p2;
							}
							
							SG_VECTOR plN;
							plN.x = lineGeo->p1.x - lineGeo->p2.x;
							plN.y = lineGeo->p1.y - lineGeo->p2.y;
							plN.z = lineGeo->p1.z - lineGeo->p2.z;
							sgSpaceMath::NormalVector(&plN);
							m_dir = plN;
								switch(m_app->GetViewPort()->GetViewPortViewType()) 
								{
								case IViewPort::TOP_VIEW:
								case IViewPort::BOTTOM_VIEW:
									m_v_dir.x = m_v_dir.y = 0.0;
									m_v_dir.z = 1.0;
									break;
								case IViewPort::LEFT_VIEW:
								case IViewPort::RIGHT_VIEW:
									m_v_dir.x = m_v_dir.z = 0.0;
									m_v_dir.y = 1.0;
									break;
								case IViewPort::FRONT_VIEW:
								case IViewPort::BACK_VIEW:
									m_v_dir.y = m_v_dir.z = 0.0;
									m_v_dir.x = 1.0;
									break;
								case IViewPort::USER_VIEW:
									m_app->GetViewPort()->GetViewPortNormal(m_v_dir);
									break;
								}
								if (m_scenario==4)
									m_dir = sgSpaceMath::VectorsVectorMult(&m_v_dir,&plN);
						}
						break;
					case 2:
						{
							IViewPort::GET_SNAP_IN in_arg;
							in_arg.scrX = pX;
							in_arg.scrY = pY;
							in_arg.snapType = SNAP_NO;
							in_arg.XFix = in_arg.YFix = in_arg.ZFix = false;
							in_arg.FixPoint.x = in_arg.FixPoint.y = in_arg.FixPoint.z = 0.0;
							IViewPort::GET_SNAP_OUT out_arg;
							m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
							m_cur_pnt = out_arg.result_point;

							sgSpaceMath::ProjectPointToLineAndGetDist(&m_first_pnt,
								&m_dir,
								&m_cur_pnt,
								&m_projection);
							char sig = (((m_projection.x-m_first_pnt.x)*m_dir.x+
								(m_projection.y-m_first_pnt.y)*m_dir.y+
								(m_projection.z-m_first_pnt.z)*m_dir.z)>0)?1:-1;

							IGetNumberPanel*  gnd = m_panel->GetCurrentGetNumberPanel();
							ASSERT(gnd);
							m_dist = sgSpaceMath::PointsDistance(&m_first_pnt,&m_projection);
							gnd->SetNumber(m_dist*sig);
							m_cur_pnt = m_projection;
						}
						break;
					case 3:
						{
							IGetPointPanel*  gpd = m_panel->GetCurrentGetPointPanel();
							ASSERT(gpd);
							double plD;
							sgSpaceMath::PlaneFromNormalAndPoint(&m_first_pnt,
								&m_v_dir,&plD);
							m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,
								m_v_dir, plD, m_projection);
							m_cur_pnt = m_projection;
							gpd->SetPoint((float)(m_cur_pnt.x),(float)(m_cur_pnt.y),(float)(m_cur_pnt.z));
							m_exist_arc_data = sgCArc::CreateArcGeoFrom_c_b_e(m_second_pnt,
								m_first_pnt,
								m_cur_pnt,
								m_invert,
								m_arc_geo_data);
						}
						break;
					default:
						ASSERT(0);
								}
								m_app->GetViewPort()->InvalidateViewPort();
		}
		break;*/
	default:
		ASSERT(0);
		break;
	}
}

void  ArcCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	switch(m_scenario) 
	{
	case 0:
		{
			if (m_step==0)
			{
				m_first_pnt=m_cur_pnt;
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
			}
			else
				if (m_step==1)
				{
					if (sgSpaceMath::PointsDistance(m_first_pnt,m_cur_pnt)<0.000001)
					{
						m_message.LoadString(IDS_ARC_ERR_SEC_P_AS_FIR);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
						return;
					}
					m_second_pnt=m_cur_pnt;
					m_step++;
					m_app->GetCommandPanel()->SetActiveRadio(m_step);

				}
				else
					if (m_step==2 && m_exist_arc_data)
					{
						sgCArc* ar = sgCreateArc(m_arc_geo_data);
						if (!ar)
							return;
						CString nm;
						nm.LoadString(IDS_TOOLTIP_THIRD);
						CString nmInd;
						nmInd.Format("%i",arc_name_index);
						nm+=nmInd;
						ar->SetName(nm.GetBuffer());
						sgGetScene()->StartUndoGroup();
						sgGetScene()->AttachObject(ar);
						m_app->ApplyAttributes(ar);
						sgGetScene()->EndUndoGroup();
						arc_name_index++;
						
						m_app->GetViewPort()->InvalidateViewPort();
						m_step=0;
						m_exist_arc_data = false;
						Arc_b_e_m();
					}
					else
						if (!m_exist_arc_data)
						{
							m_message.LoadString(IDS_ERROR_INFIN_RAD);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,
								m_message);
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
					if (m_step==2 && m_exist_arc_data)
					{
						sgCArc* ar = sgCreateArc(&m_arc_geo_data);
						if (!ar)
						return;
						CString nm;
						nm.LoadString(IDS_TOOLTIP_THIRD);
						CString nmInd;
						nmInd.Format("%i",arc_name_index);
						nm+=nmInd;
						ar->SetName(nm.GetBuffer());
						sgGetScene()->StartUndoGroup();
						sgGetScene()->AttachObject(ar);
						m_app->ApplyAttributes(ar);
						sgGetScene()->EndUndoGroup();
						arc_name_index++;
						
						m_app->GetViewPort()->InvalidateViewPort();
						m_step=0;
						m_exist_arc_data = false;
						Arc_c_b_e();
					}
					else
						if (!m_exist_arc_data)
						{
							m_message.LoadString(IDS_ERROR_INFIN_RAD);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,
								m_message);
						}
		}
		break;
	case 2:
	case 3:
	case 4:
	case 5:
		{
			switch(m_step) 
			{
			case 0:
				if (m_scenario==2 || m_scenario==3)
				{
					if (m_other_arc==NULL)
					{
						m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
					}
					else
					{
						m_message="  ";
						m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
							m_message);
						m_step++;
						m_panel->EnablePage(m_step,true);
						m_panel->SetActivePage(m_step);
					}
				}
				if (m_scenario==4 || m_scenario==5)
				{
					if (m_other_line==NULL)
					{
						m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
					}
					else
					{
						m_message="  ";
						m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
							m_message);
						m_step++;
						m_panel->EnablePage(m_step,true);
						m_panel->SetActivePage(m_step);
					}
				}
				break;
			case 1:
				{
					m_step++;
					m_panel->EnablePage(m_step,true);
					m_panel->SetActivePage(m_step);
				}
				break;
			case 2:
				{
					m_second_pnt=m_cur_pnt;
					m_step++;
					m_panel->EnablePage(m_step,true);
					m_panel->SetActivePage(m_step);
				}
				break;
			case 3:
				{
					if (m_exist_arc_data)
					{
						sgCArc* ar = sgCreateArc(&m_arc_geo_data);
						if (!ar)
						return;
						CString nm;
						nm.LoadString(IDS_TOOLTIP_THIRD);
						CString nmInd;
						nmInd.Format("%i",arc_name_index);
						nm+=nmInd;
						ar->SetName(nm.GetBuffer());
						sgGetScene()->StartUndoGroup();
						sgGetScene()->AttachObject(ar);
						sgGetScene()->EndUndoGroup();
						arc_name_index++;
						
						m_app->GetViewPort()->InvalidateViewPort();
						m_step=0;
						m_exist_arc_data = false;
						if (m_scenario==2)
							ArcArcExtScenario();
						else
							if (m_scenario==3)
								ArcArcPerpScenario();
							else
								if (m_scenario==4)
									ArcLineExtScenario();
								else
									if (m_scenario==5)
										ArcLinePerpScenario();
					}
					else
						{
							m_message.LoadString(IDS_ERROR_INFIN_RAD);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,
								m_message);
						}
				}
				break;
			default:
				break;
			}
		}
		break;*/
	default:
		ASSERT(0);
		break;
	}
}

void  ArcCommand::Draw()
{
	switch(m_scenario) 
	{
	case 0:
	//case 1:
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
				m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->DrawLine(ln);
			}
			if (m_step==2)
			{
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_second_pnt);
				if (m_exist_arc_data)
				{
					m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
					m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
					m_app->GetViewPort()->GetPainter()->DrawArc(m_arc_geo_data);
				}
			}
		}
		break;
	/*case 2:
	case 3:
	case 4:
	case 5:
		{
			SWITCH_RESOURCE
				switch(m_step) {
					case 0:
						break;
					case 1:
						{	
							float pC[3];
							m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
							m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
							m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);
						}
						break;
					case 2:
						{
							float pC[3];
							m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
							m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
							m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);
							m_app->GetViewPort()->GetPainter()->DrawPoint(m_projection);
							m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
							m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
							SG_LINE ln;
							ln.p1 = m_first_pnt;
							ln.p2 = m_projection;
							m_app->GetViewPort()->GetPainter()->DrawLine(ln);
							if (m_other_arc)
							{
							SG_CIRCLE cr;
							cr.center = m_projection;
							cr.normal = m_other_arc->GetGeometry()->normal;
							cr.radius = m_dist;
							m_app->GetViewPort()->GetPainter()->DrawCircle(cr);
							}
						}
						break;
					case 3:
						{
						
							float pC[3];
							m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
							m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
							m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
							m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);
							m_app->GetViewPort()->GetPainter()->DrawPoint(m_second_pnt);
							if (m_exist_arc_data)
							{
								m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
								m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
								m_app->GetViewPort()->GetPainter()->DrawArc(m_arc_geo_data);
							}
						}
						break;
					default:
						ASSERT(0);
				}
		}
		break;*/
	default:
		ASSERT(0);
		break;
	}
}

void  ArcCommand::OnEnter()
{
	SWITCH_RESOURCE
	switch(m_scenario) 
	{
	case 0:
		{
			if (m_step==0)
			{
				ASSERT(m_get_first_point_panel);

				m_get_first_point_panel->GetPoint(m_first_pnt.x,m_first_pnt.y,m_first_pnt.z);
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);

			}
			else
				if (m_step==1)
				{
					ASSERT(m_get_second_point_panel);

					m_get_second_point_panel->GetPoint(m_second_pnt.x,m_second_pnt.y,m_second_pnt.z);

					if (sgSpaceMath::PointsDistance(m_first_pnt,m_second_pnt)<0.000001)
					{
						m_message.LoadString(IDS_ARC_ERR_SEC_P_AS_FIR);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
						return;
					}
					m_step++;
					m_app->GetCommandPanel()->SetActiveRadio(m_step);

				}
				else
					if (m_step==2)
					{
						ASSERT(m_get_third_point_panel);

						SG_POINT tmpPn;
						m_get_third_point_panel->GetPoint(tmpPn.x,tmpPn.y,tmpPn.z);
						if (m_arc_geo_data.FromThreePoints(m_first_pnt,
													m_second_pnt,
													tmpPn,
													m_invert))
						{
							sgCArc* ar = sgCreateArc(m_arc_geo_data);
							if (!ar)
								return;
							CString nm;
							nm.LoadString(IDS_TOOLTIP_THIRD);
							CString nmInd;
							nmInd.Format("%i",arc_name_index);
							nm+=nmInd;
							ar->SetName(nm.GetBuffer());
							sgGetScene()->StartUndoGroup();
							sgGetScene()->AttachObject(ar);
							m_app->ApplyAttributes(ar);
							sgGetScene()->EndUndoGroup();
							arc_name_index++;
							
							m_app->GetViewPort()->InvalidateViewPort();
							m_step=0;
							m_exist_arc_data = false;
							Arc_b_e_m();
						}
						else
						{
							m_message.LoadString(IDS_ERROR_INFIN_RAD);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,
								m_message);
							m_exist_arc_data = false;
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
						if (sgCArc::CreateArcGeoFrom_c_b_e(m_first_pnt,
												m_second_pnt,
												tmpPn,
												m_invert,
												m_arc_geo_data))
						{
							sgCArc* ar = sgCreateArc(&m_arc_geo_data);
							if (!ar)
							return;
							CString nm;
							nm.LoadString(IDS_TOOLTIP_THIRD);
							CString nmInd;
							nmInd.Format("%i",arc_name_index);
							nm+=nmInd;
							ar->SetName(nm.GetBuffer());
							sgGetScene()->StartUndoGroup();
							sgGetScene()->AttachObject(ar);
							m_app->ApplyAttributes(ar);
							sgGetScene()->EndUndoGroup();
							arc_name_index++;
							
							m_app->GetViewPort()->InvalidateViewPort();
							m_step=0;
							m_exist_arc_data = false;
							Arc_c_b_e();
						}
						else
						{
							m_message.LoadString(IDS_ERROR_INFIN_RAD);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,
								m_message);
							m_exist_arc_data = false;
						}
					}
		}
		break;
	case 2:
	case 3:
	case 4:
	case 5:
		{
			switch(m_step) 
			{
			case 0:
				if (m_scenario==2 || m_scenario==3)
				{
					if (m_other_arc==NULL)
					{
						m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
					}
					else
					{
						m_message="  ";
						m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
							m_message);
						m_step++;
						m_panel->EnablePage(m_step,true);
						m_panel->SetActivePage(m_step);
					}
				}
				if (m_scenario==4 || m_scenario==5)
				{
					if (m_other_line==NULL)
					{
						m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
					}
					else
					{
						m_message="  ";
						m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
							m_message);
						m_step++;
						m_panel->EnablePage(m_step,true);
						m_panel->SetActivePage(m_step);
					}
				}
				break;
			case 1:
				{
					m_step++;
					m_panel->EnablePage(m_step,true);
					m_panel->SetActivePage(m_step);
				}
				break;
			case 2:
				{
					IGetNumberPanel*  gnd = m_panel->GetCurrentGetNumberPanel();
					ASSERT(gnd);

					double dst = gnd->GetNumber();

					m_second_pnt.x = m_first_pnt.x+dst*m_dir.x;
					m_second_pnt.y = m_first_pnt.y+dst*m_dir.y;
					m_second_pnt.z = m_first_pnt.z+dst*m_dir.z;

					m_step++;
					m_panel->EnablePage(m_step,true);
					m_panel->SetActivePage(m_step);
				}
				break;
			case 3:
				{
					m_message.LoadString(IDS_WARNING_CHOISE_SCREEN_POINT);
					m_app->PutMessage(IApplicationInterface::MT_WARNING,
						m_message);
				}
				break;
			default:
				break;
			}
		}
		break;*/
	default:
		ASSERT(0);
		break;
	}
}

unsigned int  ArcCommand::GetItemsCount()
{
	return 0;
}

void         ArcCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	/*SWITCH_RESOURCE
		switch(itemID) {
	case 0:
		itSrt.LoadString(IDS_INVERT);
		break;
	case 1:
		itSrt.LoadString(IDS_ARC_SCENAR_B_E_M);
		break;
	case 2:
		itSrt.LoadString(IDS_ARC_SCENAR_C_B_E);
		break;
	case 3:
		itSrt.LoadString(IDS_ARC_SCENAR_OTHER_ARC_EXT);
		break;
	case 4:
		itSrt.LoadString(IDS_ARC_SCENAR_PERPEND_OTHER_ARC);
		break;
	case 5:
		itSrt.LoadString(IDS_ARC_SCENSG_LINE_EXT);
		break;
	case 6:
		itSrt.LoadString(IDS_ARC_SCENAR_PERPEND_LINE);
		break;
	default:
		ASSERT(0);
		}*/
}

void     ArcCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	/*enbl = true;
	checked = false;
	switch(itemID) {
	case 0:
		checked = m_invert;
		break;
	case 1:
		if (m_scenario==0)
			checked = true;
		break;
	case 2:
		if (m_scenario==1)
			checked = true;
		break;
	case 3:
		if (m_scenario==2)
			checked = true;
		break;
	case 4:
		if (m_scenario==3)
			checked = true;
		break;
	case 5:
		if (m_scenario==4)
			checked = true;
		break;
	case 6:
		if (m_scenario==5)
			checked = true;
		break;
	default:
		ASSERT(0);
	}*/
}

HBITMAP   ArcCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         ArcCommand::Run(unsigned int itemID)
{
	/*switch(itemID) {
	case 0:
		m_invert=!m_invert;
		break;
	case 1:
		Arc_b_e_m();
		break;
	case 2:
		Arc_c_b_e();
		break;
	case 3:
		ArcArcExtScenario();
		break;
	case 4:
		ArcArcPerpScenario();
		break;
	case 5:
		ArcLineExtScenario();
		break;
	case 6:
		ArcLinePerpScenario();
		break;
	default:
		ASSERT(0);
	}*/
}

void ArcCommand::Arc_b_e_m()
{
	SWITCH_RESOURCE
	m_app->GetCommandPanel()->RemoveAllDialogs();
	CString lab;
	lab.LoadString(IDS_BEG_POINT);
	m_get_first_point_panel = 
		reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
		lab,true));
	
	lab.LoadString(IDS_END_POINT);
	m_get_second_point_panel = 
		reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
		lab,true));

	lab.LoadString(IDS_POINT_ON_ARC);
	m_get_third_point_panel = 
		reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
		lab,true));
	
	m_app->GetCommandPanel()->SetActiveRadio(0);

	m_message.LoadString(IDS_ENTER_POINT_COORDS);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

	m_scenario = 0;
	m_step=0;
}

void ArcCommand::Arc_c_b_e()
{
	/*SWITCH_RESOURCE
	ASSERT(m_panel);
	m_panel->RemoveAllPages();
	CString lab;
	lab.LoadString(IDS_CENTER);
	m_panel->AddPage(lab,IBasePanel::GET_POINT_DLG);
	m_panel->EnablePage(0,true);
	lab.LoadString(IDS_BEG_POINT);
	m_panel->AddPage(lab,IBasePanel::GET_POINT_DLG);
	m_panel->EnablePage(1,false);
	lab.LoadString(IDS_END_POINT);
	m_panel->AddPage(lab,IBasePanel::GET_POINT_DLG);
	m_panel->EnablePage(2,false);

	//m_panel->SetActivePage(1);
	m_panel->SetActivePage(0);

	m_scenario = 1;
	m_step=0;*/
}


void ArcCommand::ArcArcExtScenario()
{
	/*SWITCH_RESOURCE
	ASSERT(m_panel);
	m_panel->RemoveAllPages();
	CString lab;
	lab.LoadString(IDS_CHOISE_ARC);
	m_panel->AddPage(lab,IBasePanel::GET_STRING_DLG);
	m_panel->EnablePage(0,true);
	lab.LoadString(IDS_CHOISE_ARC_PNT);
	m_panel->AddPage(lab,IBasePanel::GET_STRING_DLG);
	m_panel->EnablePage(1,false);
	lab.LoadString(IDS_RADIUS);
	m_panel->AddPage(lab,IBasePanel::GET_NUMBER_DLG);
	m_panel->EnablePage(2,false);

	lab.LoadString(IDS_END_POINT);
	m_panel->AddPage(lab,IBasePanel::GET_POINT_DLG);
	m_panel->EnablePage(3,false);

	m_panel->SetActivePage(0);

	m_scenario = 2;
	m_step=0;*/
}

void ArcCommand::ArcArcPerpScenario()
{
/*
	SWITCH_RESOURCE
		ASSERT(m_panel);
	m_panel->RemoveAllPages();
	CString lab;
	lab.LoadString(IDS_CHOISE_ARC);
	m_panel->AddPage(lab,IBasePanel::GET_STRING_DLG);
	m_panel->EnablePage(0,true);
	lab.LoadString(IDS_CHOISE_ARC_PNT);
	m_panel->AddPage(lab,IBasePanel::GET_STRING_DLG);
	m_panel->EnablePage(1,false);
	lab.LoadString(IDS_RADIUS);
	m_panel->AddPage(lab,IBasePanel::GET_NUMBER_DLG);
	m_panel->EnablePage(2,false);

	lab.LoadString(IDS_END_POINT);
	m_panel->AddPage(lab,IBasePanel::GET_POINT_DLG);
	m_panel->EnablePage(3,false);

	m_panel->SetActivePage(0);

	m_scenario = 3;
	m_step=0;*/
}

void ArcCommand::ArcLineExtScenario()
{
/*
	SWITCH_RESOURCE
		ASSERT(m_panel);
	m_panel->RemoveAllPages();
	CString lab;
	lab.LoadString(IDS_CHOISE_LINE);
	m_panel->AddPage(lab,IBasePanel::GET_STRING_DLG);
	m_panel->EnablePage(0,true);
	lab.LoadString(IDS_CHOISE_LINE_PNT);
	m_panel->AddPage(lab,IBasePanel::GET_STRING_DLG);
	m_panel->EnablePage(1,false);
	lab.LoadString(IDS_RADIUS);
	m_panel->AddPage(lab,IBasePanel::GET_NUMBER_DLG);
	m_panel->EnablePage(2,false);

	lab.LoadString(IDS_END_POINT);
	m_panel->AddPage(lab,IBasePanel::GET_POINT_DLG);
	m_panel->EnablePage(3,false);

	m_panel->SetActivePage(0);

	m_scenario = 4;
	m_step=0;*/
}

void ArcCommand::ArcLinePerpScenario()
{
	/*SWITCH_RESOURCE
		ASSERT(m_panel);
	m_panel->RemoveAllPages();
	CString lab;
	lab.LoadString(IDS_CHOISE_LINE);
	m_panel->AddPage(lab,IBasePanel::GET_STRING_DLG);
	m_panel->EnablePage(0,true);
	lab.LoadString(IDS_CHOISE_LINE_PNT);
	m_panel->AddPage(lab,IBasePanel::GET_STRING_DLG);
	m_panel->EnablePage(1,false);
	lab.LoadString(IDS_RADIUS);
	m_panel->AddPage(lab,IBasePanel::GET_NUMBER_DLG);
	m_panel->EnablePage(2,false);

	lab.LoadString(IDS_END_POINT);
	m_panel->AddPage(lab,IBasePanel::GET_POINT_DLG);
	m_panel->EnablePage(3,false);

	m_panel->SetActivePage(0);

	m_scenario = 5;
	m_step=0;*/
}

