#include "stdafx.h"

#include "Line.h"

#include "..//resource.h"

#include <math.h>

int     line_name_index = 1;

LineCommand::LineCommand(IApplicationInterface* appI):
					m_app(appI)
					, m_scenario(0)
					, m_step(0)
					, m_bitmaps(NULL)
					, m_other_line(NULL)
					, m_other_arc(NULL)
{
	ASSERT(m_app);
	memset(&m_1_panels,0,sizeof(FIRST_SC));
	memset(&m_2_panels,0,sizeof(FIRST_SC));
	memset(&m_3_panels,0,sizeof(THIRD_SC));
	memset(&m_4_panels,0,sizeof(FOURTH_SC));
	memset(&m_5_panels,0,sizeof(FIVETH_SC));
	memset(&m_6_panels,0,sizeof(SIXTH_SC));
	memset(&m_7_panels,0,sizeof(SEVENTH_SC));
}

LineCommand::~LineCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	if (m_bitmaps)
	{
		for (int i=0;i<7;i++)
			if (m_bitmaps[i].m_hObject)
				m_bitmaps[i].DeleteObject();
		delete [] m_bitmaps;
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

bool    LineCommand::PreTranslateMessage(MSG* pMsg)
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
				if (m_1_panels.m_get_first_point_panel)
					m_1_panels.m_get_first_point_panel->GetWindow()->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
				break;
			case 1:
				if (m_1_panels.m_get_second_point_panel)
					m_1_panels.m_get_second_point_panel->GetWindow()->SendMessage(pMsg->message,
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

void     LineCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
												 void* params) 
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=1);

		m_step = (unsigned int)newActiveDlg;

		if (m_step==0)
			m_app->GetCommandPanel()->EnableRadio(1,false);

		m_app->GetViewPort()->InvalidateViewPort();
		return;
	}
}

void  LineCommand::Start()	
{
	m_app->GetCommandPanel()->RemoveAllDialogs();

	SWITCH_RESOURCE

	CString lab;
	lab.LoadString(IDS_TOOLTIP_FIRST);
	m_app->StartCommander(lab);
	
	LineTwoPointsScenario();

	/*m_bitmaps = new CBitmap[7];
	m_bitmaps[0].LoadBitmap(IDB_LINE_TWO_PNTS);
	m_bitmaps[1].LoadBitmap(IDB_LINE_POINT_INC);
	m_bitmaps[2].LoadBitmap(IDB_LINE_POINT_VECTOR);
	m_bitmaps[3].LoadBitmap(IDB_LINE_LINE_EXT);
	m_bitmaps[4].LoadBitmap(IDB_LINE_LINE_PERP);
	m_bitmaps[5].LoadBitmap(IDB_LINE_ARC_EXT);
	m_bitmaps[6].LoadBitmap(IDB_LINE_ARC_PERP);*/
}

void  LineCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	switch(m_scenario) 
	{
	case 0:
	//case 1:
		{
			ASSERT(m_1_panels.m_get_first_point_panel);
			IViewPort::GET_SNAP_IN in_arg;
			in_arg.scrX = pX;
			in_arg.scrY = pY;
			in_arg.snapType = SNAP_SYSTEM;
			in_arg.XFix = m_1_panels.m_get_first_point_panel->IsXFixed();
			in_arg.YFix = m_1_panels.m_get_first_point_panel->IsYFixed();
			in_arg.ZFix = m_1_panels.m_get_first_point_panel->IsZFixed();
			double tmpFl[3];
			m_1_panels.m_get_first_point_panel->GetPoint(tmpFl[0],tmpFl[1],tmpFl[2]);
			if (m_scenario==0)
			{
				in_arg.FixPoint.x = tmpFl[0];
				in_arg.FixPoint.y = tmpFl[1];
				in_arg.FixPoint.z = tmpFl[2];
			}
			else
			{
				in_arg.FixPoint.x = m_first_pnt.x+tmpFl[0];
				in_arg.FixPoint.y = m_first_pnt.y+tmpFl[1];
				in_arg.FixPoint.z = m_first_pnt.z+tmpFl[2];
			}
			IViewPort::GET_SNAP_OUT out_arg;
			m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
			m_cur_pnt = out_arg.result_point;
			switch(m_step) 
			{
			case 0:
				m_1_panels.m_get_first_point_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
				break;
			case 1:
				{
					/*if (m_scenario==1)
						m_1_panels.m_get_first_point_panel->SetPoint(m_cur_pnt.x-m_first_pnt.x,
									m_cur_pnt.y-m_first_pnt.y,
									m_cur_pnt.z-m_first_pnt.z);
					else*/
						m_1_panels.m_get_second_point_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
				}
				break;
			default:
				ASSERT(0);
			}
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	/*case 2:
		{
			switch(m_step) {
			case 0:
				{
					ASSERT(m_3_panels->m_get_first_point_panel);

					IViewPort::GET_SNAP_IN in_arg;
					in_arg.scrX = pX;
					in_arg.scrY = pY;
					in_arg.snapType = SNAP_SYSTEM;
					in_arg.XFix = m_3_panels->m_get_first_point_panel->IsXFixed();
					in_arg.YFix = m_3_panels->m_get_first_point_panel->IsYFixed();
					in_arg.ZFix = m_3_panels->m_get_first_point_panel->IsZFixed();
					m_3_panels->m_get_first_point_panel->GetPoint(in_arg.FixPoint.x,
						in_arg.FixPoint.y,
						in_arg.FixPoint.z);
					IViewPort::GET_SNAP_OUT out_arg;
					m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
					m_cur_pnt = out_arg.result_point;
					m_3_panels->m_get_first_point_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
				}
				break;
			case 1:
				{
					ASSERT(m_3_panels->m_get_dir_panel);

					double tmpFl[3];
					if (m_3_panels->m_get_dir_panel->GetVector(tmpFl[0],tmpFl[1],tmpFl[2])==
										IGetVectorPanel::USER_VECTOR)
					{
						IViewPort::GET_SNAP_IN in_arg;
						in_arg.scrX = pX;
						in_arg.scrY = pY;
						in_arg.snapType = SNAP_SYSTEM;
						in_arg.XFix = m_3_panels->m_get_dir_panel->IsXFixed();
						in_arg.YFix = m_3_panels->m_get_dir_panel->IsYFixed();
						in_arg.ZFix = m_3_panels->m_get_dir_panel->IsZFixed();
						in_arg.FixPoint.x = tmpFl[0];
						in_arg.FixPoint.y = tmpFl[1];
						in_arg.FixPoint.z = tmpFl[2];
						IViewPort::GET_SNAP_OUT out_arg;
						m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
						m_cur_pnt = out_arg.result_point;
						SG_VECTOR tmpDir;
						tmpDir.x = m_cur_pnt.x - m_first_pnt.x;
						tmpDir.y = m_cur_pnt.y - m_first_pnt.y;
						tmpDir.z = m_cur_pnt.z - m_first_pnt.z;
						sgSpaceMath::NormalVector(&tmpDir);
						m_3_panels->m_get_dir_panel->SetVector(IGetVectorPanel::USER_VECTOR,
							tmpDir.x,tmpDir.y,tmpDir.z);
						m_second_pnt.x = 2.0*m_cur_pnt.x-m_first_pnt.x;
						m_second_pnt.y = 2.0*m_cur_pnt.y-m_first_pnt.y;
						m_second_pnt.z = 2.0*m_cur_pnt.z-m_first_pnt.z;
					}
					else
					{
						double grSz = (double)m_app->GetViewPort()->GetGridSize();
						m_cur_pnt.x = m_first_pnt.x+tmpFl[0]*grSz;
						m_cur_pnt.y = m_first_pnt.y+tmpFl[1]*grSz;
						m_cur_pnt.z = m_first_pnt.z+tmpFl[2]*grSz;
						m_second_pnt.x = m_cur_pnt.x +tmpFl[0]*grSz;
						m_second_pnt.y = m_cur_pnt.y +tmpFl[1]*grSz;
						m_second_pnt.z = m_cur_pnt.z +tmpFl[2]*grSz;
					}
				}
				break;
			case 2:
				{
					ASSERT(m_3_panels->m_get_length_panel);
					double sz;
					SG_POINT projection;
	
					if (!m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,m_first_pnt,m_dir,projection))
					{
						sz = 0.0;
						m_3_panels->m_get_length_panel->SetNumber(sz);
						break;
					}
					char sig = (((projection.x-m_first_pnt.x)*m_dir.x+
						(projection.y-m_first_pnt.y)*m_dir.y+
						(projection.z-m_first_pnt.z)*m_dir.z)>0)?1:-1;

					sz = sgSpaceMath::PointsDistance(&m_first_pnt,&projection)*sig;
					sz = m_commandBar->ApplyPrecision(sz);
					projection.x = m_first_pnt.x+m_dir.x*sz;
					projection.y = m_first_pnt.y+m_dir.y*sz;
					projection.z = m_first_pnt.z+m_dir.z*sz;
					m_3_panels->m_get_length_panel->SetNumber(sz);
					m_second_pnt = projection;
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
						m_dir.x = m_first_pnt.x-lineGeo->p2.x;
						m_dir.y = m_first_pnt.y-lineGeo->p2.y;
						m_dir.z = m_first_pnt.z-lineGeo->p2.z;
						sgSpaceMath::NormalVector(&m_dir);
					}
					else
					{
						m_message.LoadString(IDS_SECOND_POINT);
						gsd->SetString(m_message);
						m_first_pnt = lineGeo->p2;
						m_dir.x = m_first_pnt.x-lineGeo->p1.x;
						m_dir.y = m_first_pnt.y-lineGeo->p1.y;
						m_dir.z = m_first_pnt.z-lineGeo->p1.z;
						sgSpaceMath::NormalVector(&m_dir);
					}
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

					SG_POINT projection;
					
					IGetNumberPanel*  gnd = m_panel->GetCurrentGetNumberPanel();
					ASSERT(gnd);
					if (!m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,
						m_first_pnt,m_dir,projection))
					{
						gnd->SetNumber(0.0);
						break;
					}
					char sig = (((projection.x-m_first_pnt.x)*m_dir.x+
						(projection.y-m_first_pnt.y)*m_dir.y+
						(projection.z-m_first_pnt.z)*m_dir.z)>0)?1:-1;

					double sz = sgSpaceMath::PointsDistance(&m_first_pnt,&projection)*sig;
					sz = m_commandBar->ApplyPrecision(sz);
					projection.x = m_first_pnt.x+m_dir.x*sz;
					projection.y = m_first_pnt.y+m_dir.y*sz;
					projection.z = m_first_pnt.z+m_dir.z*sz;
					gnd->SetNumber(sz);
					m_second_pnt = projection;
				}
				break;
			default:
				ASSERT(0);
			}
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	case 4:
		{
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
						}
						break;
					case 2:
						{
							SG_VECTOR plN;

							ASSERT(m_other_line);
							const SG_LINE* lineGeo = m_other_line->GetGeometry();

							plN.x = lineGeo->p1.x - lineGeo->p2.x;
							plN.y = lineGeo->p1.y - lineGeo->p2.y;
							plN.z = lineGeo->p1.z - lineGeo->p2.z;
							sgSpaceMath::NormalVector(&plN);
							//sgSpaceMath::PlaneFromNormalAndPoint(&m_first_pnt, &plN, &plD);

							//if (!m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,plN,plD,m_cur_pnt))
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
								case IViewPort::USER_VIEW:
									m_app->GetViewPort()->GetViewPortNormal(m_dir);
									break;
								}
								SG_VECTOR lineDir = sgSpaceMath::VectorsVectorMult(&m_dir,&plN);
								SG_POINT  prP;
								sgSpaceMath::ProjectPointToLineAndGetDist(&m_first_pnt,&lineDir,
												&m_cur_pnt,&prP);
								m_cur_pnt = prP;
							}
														
							IGetPointPanel*  gpd = m_panel->GetCurrentGetPointPanel();
							ASSERT(gpd);
							gpd->SetPoint(m_cur_pnt.x, m_cur_pnt.y, m_cur_pnt.z);
						}
						break;
					default:
						ASSERT(0);
				}
				m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	case 5:
	case 6:
		{
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
									if (m_scenario==5)
										m_dir = sgSpaceMath::VectorsVectorMult(const_cast<SG_VECTOR*>(&arcGeo->normal),
																	&tmpV);
									else
										m_dir = tmpV;
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
									if (m_scenario==5)
										m_dir = sgSpaceMath::VectorsVectorMult(&tmpV,
														const_cast<SG_VECTOR*>(&arcGeo->normal));
									else
										m_dir = tmpV;
									sgSpaceMath::NormalVector(&m_dir);
								}
							}
							break;
						case 2:
							{
								SG_POINT projection;
								m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,m_first_pnt,m_dir,
									projection);
								char sig = (((projection.x-m_first_pnt.x)*m_dir.x+
									(projection.y-m_first_pnt.y)*m_dir.y+
									(projection.z-m_first_pnt.z)*m_dir.z)>0)?1:-1;

								IGetNumberPanel*  gnd = m_panel->GetCurrentGetNumberPanel();
								ASSERT(gnd);
								double sz = sgSpaceMath::PointsDistance(&m_first_pnt,&projection)*sig;
								sz = m_commandBar->ApplyPrecision(sz);
								projection.x = m_first_pnt.x+m_dir.x*sz;
								projection.y = m_first_pnt.y+m_dir.y*sz;
								projection.z = m_first_pnt.z+m_dir.z*sz;
								gnd->SetNumber(sz);
								m_second_pnt = projection;
								m_cur_pnt = projection;
							}
							break;
						default:
							ASSERT(0);
			}
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;*/
	/*case 6: OLD-NOT CHANGE
		{
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
									m_plN = sgSpaceMath::VectorsVectorMult(const_cast<SG_VECTOR*>(&arcGeo->normal),
										&tmpV);
									sgSpaceMath::NormalVector(&m_plN);
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
									m_plN = sgSpaceMath::VectorsVectorMult(&tmpV,
										const_cast<SG_VECTOR*>(&arcGeo->normal));
									sgSpaceMath::NormalVector(&m_plN);
								}
							}
							break;
						case 2:
							{
								
								//if (!m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,plN,plD,m_cur_pnt))
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
									case IViewPort::USER_VIEW:
										m_app->GetViewPort()->GetViewPortNormal(m_dir);
										break;
									}
									SG_VECTOR lineDir = sgSpaceMath::VectorsVectorMult(&m_dir,&m_plN);
									SG_POINT  prP;
									sgSpaceMath::ProjectPointToLineAndGetDist(&m_first_pnt,&lineDir,
										&m_cur_pnt,&prP);
									m_cur_pnt = prP;
								}

								IGetPointPanel*  gpd = m_panel->GetCurrentGetPointPanel();
								ASSERT(gpd);
								gpd->SetPoint(m_cur_pnt.x, m_cur_pnt.y, m_cur_pnt.z);
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

void  LineCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	switch(m_scenario) 
	{
	case 0:
	//case 1:
		{
			switch(m_step) {
			case 0:
				m_first_pnt=m_cur_pnt;
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				break;
			case 1:
				{
					if (sgSpaceMath::PointsDistance(m_first_pnt,m_cur_pnt)>0.000001)
					{
							SG_LINE lnG;
							lnG.p1 = m_first_pnt;
							lnG.p2 = m_cur_pnt;
							sgCLine* ln = sgCreateLine(lnG.p1.x,lnG.p1.y,lnG.p1.z,
								lnG.p2.x,lnG.p2.y,lnG.p2.z);
							if (!ln)
								return;
							CString nm;
							nm.LoadString(IDS_TOOLTIP_FIRST);
							CString nmInd;
							nmInd.Format("%i",line_name_index);
							nm+=nmInd;
							ln->SetName(nm.GetBuffer());
							sgGetScene()->StartUndoGroup();
							sgGetScene()->AttachObject(ln);
							m_app->ApplyAttributes(ln);
							sgGetScene()->EndUndoGroup();
							line_name_index++;
							
							m_app->GetViewPort()->InvalidateViewPort();
							m_step=0;
							//if (m_scenario==0)
								LineTwoPointsScenario();
							/*else
								LinePointDxdydzScenario();*/
					}
					else
					{
						m_message.LoadString(IDS_ERROR_ZERO_LENGTH);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
									m_message);
					}
				}
				break;
			default:
				ASSERT(0);
			}
		}
		break;
	/*case 2:
		{
			switch(m_step) 
			{
			case 0:
				m_first_pnt=m_cur_pnt;
				m_step++;
				m_panel->EnablePage(m_step,true);
				m_panel->SetActivePage(m_step);
				break;
			case 1:
				{
					IGetVectorPanel*  gvd = m_panel->GetCurrentGetVectorPanel();
					ASSERT(gvd);

					gvd->GetVector(m_dir.x, m_dir.y,m_dir.z);
					if (sgSpaceMath::NormalVector(&m_dir))
					{
						m_step++;
						m_panel->EnablePage(m_step,true);
						m_panel->SetActivePage(m_step);
					}
					else
					{
						m_message.LoadString(IDS_ERROR_ZERO_VECTOR);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
					}
				}
				break;
			case 2:
				{
					SG_LINE lnG;
					lnG.p1 = m_first_pnt;

					IGetNumberPanel*  gnd = m_panel->GetCurrentGetNumberPanel();
					ASSERT(gnd);

					double dst = gnd->GetNumber();
					if (fabs(dst)>0.000001)
					{
						lnG.p2.x = m_first_pnt.x+m_dir.x*dst;
						lnG.p2.y = m_first_pnt.y+m_dir.y*dst;
						lnG.p2.z = m_first_pnt.z+m_dir.z*dst;
						sgCLine* ln = sgCreateLine(&lnG);
						CString nm;
						nm.LoadString(IDS_TOOLTIP_FIRST);
						CString nmInd;
						nmInd.Format("%i",line_name_index);
						nm+=nmInd;
						ln->SetName(nm.GetBuffer());
						sgGetScene()->StartUndoGroup();
						sgGetScene()->AttachObject(ln);
						m_app->ApplyAttributes(ln);
						sgGetScene()->EndUndoGroup();
						line_name_index++;
						
						m_app->GetViewPort()->InvalidateViewPort();
						m_step=0;
						LinePointDirLenScenario();
					}
					else
					{
						m_message.LoadString(IDS_ERROR_ZERO_LENGTH);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
					}
				}
				break;
			default:
				ASSERT(0);
				break;
			}
		}
		break;
	case 3:
		{
			switch(m_step) 
			{
			case 0:
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
					SG_LINE lnG;
					lnG.p1 = m_first_pnt;

					IGetNumberPanel*  gnd = m_panel->GetCurrentGetNumberPanel();
					ASSERT(gnd);

					double dst = gnd->GetNumber();
					if (fabs(dst)>0.000001)
					{
						lnG.p2.x = m_first_pnt.x+m_dir.x*dst;
						lnG.p2.y = m_first_pnt.y+m_dir.y*dst;
						lnG.p2.z = m_first_pnt.z+m_dir.z*dst;
						sgCLine* ln = sgCreateLine(&lnG);
						CString nm;
						nm.LoadString(IDS_TOOLTIP_FIRST);
						CString nmInd;
						nmInd.Format("%i",line_name_index);
						nm+=nmInd;
						ln->SetName(nm.GetBuffer());
						sgGetScene()->StartUndoGroup();
						sgGetScene()->AttachObject(ln);
						m_app->ApplyAttributes(ln);
						sgGetScene()->EndUndoGroup();
						line_name_index++;
						
						m_app->GetViewPort()->InvalidateViewPort();
						m_step=0;
						LineLineExtScenario();
					}
					else
					{
						m_message.LoadString(IDS_ERROR_ZERO_LENGTH);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
					}
				}
				break;
			default:
				break;
			}
		}
		break;
	case 4:
		{
			switch(m_step) 
			{
			case 0:
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
					if (sgSpaceMath::PointsDistance(&m_first_pnt,&m_cur_pnt)>0.000001)
					{
						SG_LINE lnG;
						lnG.p1 = m_first_pnt;
						lnG.p2 = m_cur_pnt;
						sgCLine* ln = sgCreateLine(&lnG);
						CString nm;
						nm.LoadString(IDS_TOOLTIP_FIRST);
						CString nmInd;
						nmInd.Format("%i",line_name_index);
						nm+=nmInd;
						ln->SetName(nm.GetBuffer());
						sgGetScene()->StartUndoGroup();
						sgGetScene()->AttachObject(ln);
						m_app->ApplyAttributes(ln);
						sgGetScene()->EndUndoGroup();
						line_name_index++;
						
						m_app->GetViewPort()->InvalidateViewPort();
						m_step=0;
						LineLinePerpScenario();
					}
					else
					{
						m_message.LoadString(IDS_ERROR_ZERO_LENGTH);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
					}
				}
				break;
			default:
				break;
			}
		}
		break;
	case 5:
	case 6:
		{
			switch(m_step) 
			{
			case 0:
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
					SG_LINE lnG;
					lnG.p1 = m_first_pnt;

					IGetNumberPanel*  gnd = m_panel->GetCurrentGetNumberPanel();
					ASSERT(gnd);

					double dst = gnd->GetNumber();
					if (fabs(dst)>0.000001)
					{
						lnG.p2.x = m_first_pnt.x+m_dir.x*dst;
						lnG.p2.y = m_first_pnt.y+m_dir.y*dst;
						lnG.p2.z = m_first_pnt.z+m_dir.z*dst;
						sgCLine* ln = sgCreateLine(&lnG);
						CString nm;
						nm.LoadString(IDS_TOOLTIP_FIRST);
						CString nmInd;
						nmInd.Format("%i",line_name_index);
						nm+=nmInd;
						ln->SetName(nm.GetBuffer());
						sgGetScene()->StartUndoGroup();
						sgGetScene()->AttachObject(ln);
						m_app->ApplyAttributes(ln);
						sgGetScene()->EndUndoGroup();
						line_name_index++;
						
						m_app->GetViewPort()->InvalidateViewPort();
						m_step=0;
						if (m_scenario==5)
							LineArcExtScenario();
						else
							LineArcPerpScenario();
					}
					else
					{
						m_message.LoadString(IDS_ERROR_ZERO_LENGTH);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
					}
				}
				break;
			default:
				break;
			}
		}
		break;
	case 6:
		{
			switch(m_step) 
			{
			case 0:
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
					if (sgSpaceMath::PointsDistance(&m_first_pnt,&m_cur_pnt)>0.000001)
					{
						SG_LINE lnG;
						lnG.p1 = m_first_pnt;
						lnG.p2 = m_cur_pnt;
						sgCLine* ln = sgCreateLine(&lnG);
						CString nm;
						nm.LoadString(IDS_TOOLTIP_FIRST);
						CString nmInd;
						nmInd.Format("%i",line_name_index);
						nm+=nmInd;
						ln->SetName(nm.GetBuffer());
						sgGetScene()->StartUndoGroup();
						sgGetScene()->AttachObject(ln);
						m_app->ApplyAttributes(ln);
						sgGetScene()->EndUndoGroup();
						line_name_index++;
						
						m_app->GetViewPort()->InvalidateViewPort();
						m_step=0;
						LineArcPerpScenario();
					}
					else
					{
						m_message.LoadString(IDS_ERROR_ZERO_LENGTH);
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


void  LineCommand::Draw()
{
	switch(m_scenario) 
	{
		case 0:
		//case 1:
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
						SG_LINE ln;
						ln.p1 = m_first_pnt;
						ln.p2 = m_cur_pnt;
						m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
						m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
						m_app->GetViewPort()->GetPainter()->DrawLine(ln);
					}
					break;
				default:
					ASSERT(0);
				}
			}
			break;
		/*case 2:
			{
				switch(m_step) {
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
							SG_LINE ln;
							ln.p1 = m_first_pnt;
							ln.p2 = m_second_pnt;
							m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
							m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
							m_app->GetViewPort()->GetPainter()->DrawLine(ln);
						}
						break;
					case 2:
						{
							float pC[3];
							m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
							m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
							m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);
							m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
							m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
							SG_LINE ln;
							ln.p1 = m_first_pnt;
							ln.p2 = m_second_pnt;
							m_app->GetViewPort()->GetPainter()->DrawLine(ln);
						}
						break;
					default:
						ASSERT(0);
							}
			}
			break;
		case 3:
			{
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
							m_app->GetViewPort()->GetPainter()->DrawPoint(m_second_pnt);
							m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
							m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
							SG_LINE ln;
							ln.p1 = m_first_pnt;
							ln.p2 = m_second_pnt;
							m_app->GetViewPort()->GetPainter()->DrawLine(ln);
						}
						break;
					default:
						ASSERT(0);
							}
						}
			break;
		case 4:
			{
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
							m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
							m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
							m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
							SG_LINE ln;
							ln.p1 = m_first_pnt;
							ln.p2 = m_cur_pnt;
							m_app->GetViewPort()->GetPainter()->DrawLine(ln);
						}
						break;
					default:
						ASSERT(0);
							}
			}
			break;
		case 5:
		case 6:
			{
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
							m_app->GetViewPort()->GetPainter()->DrawPoint(m_second_pnt);
							m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
							m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
							SG_LINE ln;
							ln.p1 = m_first_pnt;
							ln.p2 = m_second_pnt;
							m_app->GetViewPort()->GetPainter()->DrawLine(ln);
						}
						break;
					default:
						ASSERT(0);
							}
			}
			break;
		case 6:
			{
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
							m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
							m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
							m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
							SG_LINE ln;
							ln.p1 = m_first_pnt;
							ln.p2 = m_cur_pnt;
							m_app->GetViewPort()->GetPainter()->DrawLine(ln);
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

void  LineCommand::OnEnter()
{
	SWITCH_RESOURCE
	switch(m_scenario) 
	{
	case 0:
	//case 1:
		{
			switch(m_step) {
						case 0:
							{
								ASSERT(m_1_panels.m_get_first_point_panel);

								m_1_panels.m_get_first_point_panel->GetPoint(m_first_pnt.x,
									m_first_pnt.y,
									m_first_pnt.z);
								m_step++;
								m_app->GetCommandPanel()->SetActiveRadio(m_step);

							}
							break;
						case 1:
							{
								SG_LINE lnG;
								lnG.p1 = m_first_pnt;

								ASSERT(m_1_panels.m_get_second_point_panel);


								double tmpFl[3];
								m_1_panels.m_get_second_point_panel->GetPoint(tmpFl[0],tmpFl[1],tmpFl[2]);

								if (m_scenario==0)
								{
									m_cur_pnt.x = tmpFl[0];
									m_cur_pnt.y = tmpFl[1];
									m_cur_pnt.z = tmpFl[2];
								}
								else
								{
									m_cur_pnt.x = m_first_pnt.x+tmpFl[0];
									m_cur_pnt.y = m_first_pnt.y+tmpFl[1];
									m_cur_pnt.z = m_first_pnt.z+tmpFl[2];
								}

								lnG.p2 = m_cur_pnt;

								if (sgSpaceMath::PointsDistance(m_first_pnt,m_cur_pnt)>0.000001)
								{
									sgCLine* ln = sgCreateLine(lnG.p1.x,lnG.p1.y,lnG.p1.z,
										lnG.p2.x,lnG.p2.y,lnG.p2.z);
									if (!ln)
										return;
									CString nm;
									nm.LoadString(IDS_TOOLTIP_FIRST);
									CString nmInd;
									nmInd.Format("%i",line_name_index);
									nm+=nmInd;
									ln->SetName(nm.GetBuffer());
									sgGetScene()->StartUndoGroup();
									sgGetScene()->AttachObject(ln);
									m_app->ApplyAttributes(ln);
									sgGetScene()->EndUndoGroup();
									line_name_index++;
									
									m_app->GetViewPort()->InvalidateViewPort();
									m_step=0;
									if (m_scenario==0)
										LineTwoPointsScenario();
									else
										LinePointDxdydzScenario();
								}
								else
								{
									m_message.LoadString(IDS_ERROR_ZERO_LENGTH);
									m_app->PutMessage(IApplicationInterface::MT_ERROR,
										m_message);
								}
							}
							break;
						default:
							ASSERT(0);
			}
		}
		break;
	/*case 2:
		{
			switch(m_step) 
			{
			case 0:
				{
					IGetPointPanel*  gpd = m_panel->GetCurrentGetPointPanel();
					ASSERT(gpd);

					gpd->GetPoint(m_first_pnt.x,m_first_pnt.y,m_first_pnt.z);
					
					m_step++;
					m_panel->EnablePage(m_step,true);
					m_panel->SetActivePage(m_step);
				}
				break;
			case 1:
				{
					IGetVectorPanel*  gvd = m_panel->GetCurrentGetVectorPanel();
					ASSERT(gvd);
					gvd->GetVector(m_dir.x, m_dir.y, m_dir.z);
					if (sgSpaceMath::NormalVector(&m_dir))
					{
						m_step++;
						m_panel->EnablePage(m_step,true);
						m_panel->SetActivePage(m_step);
					}
					else
					{
						m_message.LoadString(IDS_ERROR_ZERO_VECTOR);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
					}
				}
				break;
			case 2:
				{
					SG_LINE lnG;
					lnG.p1 = m_first_pnt;

					IGetNumberPanel*  gnd = m_panel->GetCurrentGetNumberPanel();
					ASSERT(gnd);

					double dst = gnd->GetNumber();
					if (fabs(dst)>0.000001)
					{
						lnG.p2.x = m_first_pnt.x+m_dir.x*dst;
						lnG.p2.y = m_first_pnt.y+m_dir.y*dst;
						lnG.p2.z = m_first_pnt.z+m_dir.z*dst;
						sgCLine* ln = sgCreateLine(&lnG);
						CString nm;
						nm.LoadString(IDS_TOOLTIP_FIRST);
						CString nmInd;
						nmInd.Format("%i",line_name_index);
						nm+=nmInd;
						ln->SetName(nm.GetBuffer());
						sgGetScene()->StartUndoGroup();
						sgGetScene()->AttachObject(ln);
						m_app->ApplyAttributes(ln);
						sgGetScene()->EndUndoGroup();
						line_name_index++;
						
						m_app->GetViewPort()->InvalidateViewPort();
						m_step=0;
						LinePointDirLenScenario();
					}
					else
					{
						m_message.LoadString(IDS_ERROR_ZERO_LENGTH);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
					}
				}
				break;
			default:
				ASSERT(0);
				break;
			}
		}
		break;
	case 3:
		{
			switch(m_step) 
			{
			case 0:
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
					SG_LINE lnG;
					lnG.p1 = m_first_pnt;

					IGetNumberPanel*  gnd = m_panel->GetCurrentGetNumberPanel();
					ASSERT(gnd);

					double dst = gnd->GetNumber();
					if (fabs(dst)>0.000001)
					{
						lnG.p2.x = m_first_pnt.x+m_dir.x*dst;
						lnG.p2.y = m_first_pnt.y+m_dir.y*dst;
						lnG.p2.z = m_first_pnt.z+m_dir.z*dst;
						sgCLine* ln = sgCreateLine(&lnG);
						CString nm;
						nm.LoadString(IDS_TOOLTIP_FIRST);
						CString nmInd;
						nmInd.Format("%i",line_name_index);
						nm+=nmInd;
						ln->SetName(nm.GetBuffer());
						sgGetScene()->StartUndoGroup();
						sgGetScene()->AttachObject(ln);
						m_app->ApplyAttributes(ln);
						sgGetScene()->EndUndoGroup();
						line_name_index++;
						
						m_app->GetViewPort()->InvalidateViewPort();
						m_step=0;
						LineLineExtScenario();
					}
					else
					{
						m_message.LoadString(IDS_ERROR_ZERO_LENGTH);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
					}
				}
				break;
			default:
				break;
			}
		}
		break;
	case 4:
		{
			switch(m_step) 
			{
			case 0:
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
					m_message.LoadString(IDS_WARNING_CHOISE_SCREEN_POINT);
					m_app->PutMessage(IApplicationInterface::MT_WARNING,
						m_message);
				}
				break;
			default:
				break;
			}
		}
		break;
	case 5:
	case 6:
		{
			switch(m_step) 
			{
			case 0:
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
					SG_LINE lnG;
					lnG.p1 = m_first_pnt;

					IGetNumberPanel*  gnd = m_panel->GetCurrentGetNumberPanel();
					ASSERT(gnd);

					double dst = gnd->GetNumber();
					if (fabs(dst)>0.000001)
					{
						lnG.p2.x = m_first_pnt.x+m_dir.x*dst;
						lnG.p2.y = m_first_pnt.y+m_dir.y*dst;
						lnG.p2.z = m_first_pnt.z+m_dir.z*dst;
						sgCLine* ln = sgCreateLine(&lnG);
						CString nm;
						nm.LoadString(IDS_TOOLTIP_FIRST);
						CString nmInd;
						nmInd.Format("%i",line_name_index);
						nm+=nmInd;
						ln->SetName(nm.GetBuffer());
						sgGetScene()->StartUndoGroup();
						sgGetScene()->AttachObject(ln);
						m_app->ApplyAttributes(ln);
						sgGetScene()->EndUndoGroup();
						line_name_index++;
						
						m_app->GetViewPort()->InvalidateViewPort();
						m_step=0;
						if (m_scenario==5)
							LineArcExtScenario();
						else
							LineArcPerpScenario();
					}
					else
					{
						m_message.LoadString(IDS_ERROR_ZERO_LENGTH);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
					}
				}
				break;
			default:
				break;
			}
		}
		break;
	case 6:
		{
			switch(m_step) 
			{
			case 0:
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

unsigned int  LineCommand::GetItemsCount()
{
	return 0;
}

void         LineCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	/*SWITCH_RESOURCE
		switch(itemID) {
	case 0:
		itSrt.LoadString(IDS_LINE_SCENAR_TWO_POINTS);
		break;
	case 1:
		itSrt.LoadString(IDS_LINE_SCENSG_POINT_DXDYDZ);
		break;
	case 2:
		itSrt.LoadString(IDS_LINE_SCENSG_POINT_DIR_LEN);
		break;
	case 3:
		itSrt.LoadString(IDS_LINE_SCENAR_OTHER_LINE_EXT);
		break;
	case 4:
		itSrt.LoadString(IDS_LINE_SCENAR_PERPEND_OTHER_LINE);
		break;
	case 5:
		itSrt.LoadString(IDS_LINE_SCENSG_ARC_EXT);
		break;
	case 6:
		itSrt.LoadString(IDS_LINE_SCENAR_PERPEND_ARC);
		break;
	default:
		ASSERT(0);
		}*/
}


void     LineCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
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
	case 2:
		if (m_scenario==2)
			checked = true;
		break;
	case 3:
		if (m_scenario==3)
			checked = true;
		break;
	case 4:
		if (m_scenario==4)
			checked = true;
		break;
	case 5:
		if (m_scenario==5)
			checked = true;
		break;
	case 6:
		if (m_scenario==6)
			checked = true;
		break;
	default:
		ASSERT(0);
	}*/
}

HBITMAP   LineCommand::GetItemBitmap(unsigned int itemID)
{
	/*if (itemID<7 && itemID>=0) 
	{
		return &m_bitmaps[itemID];
	}
	else
	{
		ASSERT(0);*/
		return NULL;
	//}
}

void         LineCommand::Run(unsigned int itemID)
{
	switch(itemID) {
	case 0:
		LineTwoPointsScenario();
		break;
	/*case 1:
		LinePointDxdydzScenario();
		break;
	case 2:
		LinePointDirLenScenario();
		break;
	case 3:
		LineLineExtScenario();
		break;
	case 4:
		LineLinePerpScenario();
		break;
	case 5:
		LineArcExtScenario();
		break;
	case 6:
		LineArcPerpScenario();
		break;*/
	default:
		ASSERT(0);
	}
}


void LineCommand::LineTwoPointsScenario()
{
	memset(&m_1_panels,0,sizeof(FIRST_SC));
	memset(&m_2_panels,0,sizeof(FIRST_SC));
	memset(&m_3_panels,0,sizeof(THIRD_SC));
	memset(&m_4_panels,0,sizeof(FOURTH_SC));
	memset(&m_5_panels,0,sizeof(FIVETH_SC));
	memset(&m_6_panels,0,sizeof(SIXTH_SC));
	memset(&m_7_panels,0,sizeof(SEVENTH_SC));

	SWITCH_RESOURCE

	CString lab;
	lab.LoadString(IDS_FIRST_POINT);

	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_1_panels.m_get_first_point_panel = 
		reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
			AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
					lab,true));
	
	lab.LoadString(IDS_SECOND_POINT);
	
	m_1_panels.m_get_second_point_panel = 
		reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
		lab,true));
	
	m_app->GetCommandPanel()->SetActiveRadio(0);

	m_message.LoadString(IDS_ENTER_POINT_COORDS);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

	m_scenario = 0;
	m_step=0;
}

void LineCommand::LinePointDxdydzScenario()
{
	memset(&m_1_panels,0,sizeof(FIRST_SC));
	memset(&m_2_panels,0,sizeof(FIRST_SC));
	memset(&m_3_panels,0,sizeof(THIRD_SC));
	memset(&m_4_panels,0,sizeof(FOURTH_SC));
	memset(&m_5_panels,0,sizeof(FIVETH_SC));
	memset(&m_6_panels,0,sizeof(SIXTH_SC));
	memset(&m_7_panels,0,sizeof(SEVENTH_SC));

	SWITCH_RESOURCE
	
	CString lab;
	lab.LoadString(IDS_FIRST_POINT);

	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_2_panels.m_get_first_point_panel = 
		reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
		lab,true));
	
	lab.LoadString(IDS_INCREASE);
	m_2_panels.m_get_second_point_panel = 
		reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
		lab,true));
	
	m_scenario = 1;
	m_step=0;

}

void LineCommand::LinePointDirLenScenario()
{
	memset(&m_1_panels,0,sizeof(FIRST_SC));
	memset(&m_2_panels,0,sizeof(FIRST_SC));
	memset(&m_3_panels,0,sizeof(THIRD_SC));
	memset(&m_4_panels,0,sizeof(FOURTH_SC));
	memset(&m_5_panels,0,sizeof(FIVETH_SC));
	memset(&m_6_panels,0,sizeof(SIXTH_SC));
	memset(&m_7_panels,0,sizeof(SEVENTH_SC));

	SWITCH_RESOURCE
	
	CString lab;
	lab.LoadString(IDS_FIRST_POINT);
	
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_3_panels.m_get_first_point_panel = 
		reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
		lab,true));

	lab.LoadString(IDS_DIR);
	
	m_3_panels.m_get_dir_panel = 
		reinterpret_cast<IGetVectorPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_VECTOR_DLG,
		lab,true));
	
	lab.LoadString(IDS_LENGTH);
	
	m_3_panels.m_get_length_panel = 
		reinterpret_cast<IGetNumberPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,
		lab,true));
	
	m_scenario = 2;
	m_step=0;
}

void LineCommand::LineLineExtScenario()
{
	memset(&m_1_panels,0,sizeof(FIRST_SC));
	memset(&m_2_panels,0,sizeof(FIRST_SC));
	memset(&m_3_panels,0,sizeof(THIRD_SC));
	memset(&m_4_panels,0,sizeof(FOURTH_SC));
	memset(&m_5_panels,0,sizeof(FIVETH_SC));
	memset(&m_6_panels,0,sizeof(SIXTH_SC));
	memset(&m_7_panels,0,sizeof(SEVENTH_SC));

	SWITCH_RESOURCE
	
	CString lab;
	lab.LoadString(IDS_CHOISE_LINE);

	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_4_panels.m_get_obj_panel  = 
		reinterpret_cast<IGetObjectsPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,
		lab,true));
	
	
	lab.LoadString(IDS_CHOISE_LINE_PNT);
	m_4_panels.m_sel_point_panel  = 
		reinterpret_cast<ISelectPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::SELECT_POINT_DLG,
		lab,true));
	
	lab.LoadString(IDS_LENGTH);
	m_4_panels.m_get_length_panel  = 
		reinterpret_cast<IGetNumberPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,
		lab,true));
	
	m_scenario = 3;
	m_step=0;
}

void LineCommand::LineLinePerpScenario()
{
	memset(&m_1_panels,0,sizeof(FIRST_SC));
	memset(&m_2_panels,0,sizeof(FIRST_SC));
	memset(&m_3_panels,0,sizeof(THIRD_SC));
	memset(&m_4_panels,0,sizeof(FOURTH_SC));
	memset(&m_5_panels,0,sizeof(FIVETH_SC));
	memset(&m_6_panels,0,sizeof(SIXTH_SC));
	memset(&m_7_panels,0,sizeof(SEVENTH_SC));

	SWITCH_RESOURCE
	
	CString lab;
	lab.LoadString(IDS_CHOISE_LINE);

	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_5_panels.m_get_obj_panel  = 
		reinterpret_cast<IGetObjectsPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,
		lab,true));
	
	lab.LoadString(IDS_CHOISE_LINE_PNT);
	m_5_panels.m_sel_point_panel  = 
		reinterpret_cast<ISelectPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::SELECT_POINT_DLG,
		lab,true));
	
	lab.LoadString(IDS_SECOND_POINT);
	m_5_panels.m_get_point_panel = 
		reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
		lab,true));
	
	m_scenario = 4;
	m_step=0;
}

void LineCommand::LineArcExtScenario()
{
	memset(&m_1_panels,0,sizeof(FIRST_SC));
	memset(&m_2_panels,0,sizeof(FIRST_SC));
	memset(&m_3_panels,0,sizeof(THIRD_SC));
	memset(&m_4_panels,0,sizeof(FOURTH_SC));
	memset(&m_5_panels,0,sizeof(FIVETH_SC));
	memset(&m_6_panels,0,sizeof(SIXTH_SC));
	memset(&m_7_panels,0,sizeof(SEVENTH_SC));

	SWITCH_RESOURCE
	
	CString lab;
	lab.LoadString(IDS_CHOISE_ARC);
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_6_panels.m_get_obj_panel  = 
		reinterpret_cast<IGetObjectsPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,
		lab,true));
	
	lab.LoadString(IDS_CHOISE_ARC_PNT);
	m_6_panels.m_sel_point_panel  = 
		reinterpret_cast<ISelectPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::SELECT_POINT_DLG,
		lab,true));
	
	lab.LoadString(IDS_LENGTH);
	m_6_panels.m_get_length_panel  = 
		reinterpret_cast<IGetNumberPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,
		lab,true));
	
	m_scenario = 5;
	m_step=0;
}

void LineCommand::LineArcPerpScenario()
{
	memset(&m_1_panels,0,sizeof(FIRST_SC));
	memset(&m_2_panels,0,sizeof(FIRST_SC));
	memset(&m_3_panels,0,sizeof(THIRD_SC));
	memset(&m_4_panels,0,sizeof(FOURTH_SC));
	memset(&m_5_panels,0,sizeof(FIVETH_SC));
	memset(&m_6_panels,0,sizeof(SIXTH_SC));
	memset(&m_7_panels,0,sizeof(SEVENTH_SC));

	SWITCH_RESOURCE
	
	CString lab;
	lab.LoadString(IDS_CHOISE_ARC);
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_7_panels.m_get_obj_panel  = 
		reinterpret_cast<IGetObjectsPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,
		lab,true));
	
	lab.LoadString(IDS_CHOISE_ARC_PNT);
	m_7_panels.m_sel_point_panel  = 
		reinterpret_cast<ISelectPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::SELECT_POINT_DLG,
		lab,true));
	
	lab.LoadString(IDS_LENGTH);
	m_7_panels.m_get_length_panel  = 
		reinterpret_cast<IGetNumberPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,
		lab,true));
	
	m_scenario = 6;
	m_step=0;
}


