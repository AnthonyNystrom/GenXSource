#include "stdafx.h"

#include "RotationBody.h"
#include <math.h>

#include "..//resource.h"

int     rotation_name_index = 1;

RotationCommand::RotationCommand(IApplicationInterface* appI):
						m_app(appI)
							,m_get_contour_panel(NULL)
							,m_get_first_point_panel(NULL)
							,m_get_second_point_panel(NULL)
							,m_get_angle_panel(NULL)
							,m_first_obj(NULL)
							,m_want_close(false)
							,m_need_third_pnt_for_plane(false)
							,m_need_fourth_pnt_for_plane(false)
							,m_bad_point_on_plane(false)
							,m_inQuestionRegime(false)
{
	ASSERT(m_app);
	m_step = 0;
	m_axe_p1.x = m_axe_p1.y = m_axe_p1.z = 0.0;
	m_axe_p2.x = m_axe_p2.y = m_axe_p2.z = 0.0;
	m_angle = 0.0;
	m_exist_draw_arc = false;
}

RotationCommand::~RotationCommand()
{
	if (!m_object_lines.empty())
		m_object_lines.clear();
	if (sgGetScene()->GetSelectedObjectsList()->GetCount()>0)
	{
		sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
		while (curObj) 
		{
			curObj->Select(false);
			curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
		}
	}
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
}


bool    RotationCommand::PreTranslateMessage(MSG* pMsg)
{
	try {  //#try
		/*if (pMsg->message==WM_KEYUP||
		pMsg->message==WM_CHAR)
		return false;*/
		if (m_inQuestionRegime)
			return false;

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
				if (m_get_contour_panel)
					m_get_contour_panel->GetWindow()->SendMessage(pMsg->message,
								pMsg->wParam,
								pMsg->lParam);
				break;
			case 1:
				if (m_get_first_point_panel)
					m_get_first_point_panel->GetWindow()->SendMessage(pMsg->message,
									pMsg->wParam,
									pMsg->lParam);
				break;
			case 2:
				if (m_get_second_point_panel)
					m_get_second_point_panel->GetWindow()->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
				break;
			case 3:
				if (m_get_angle_panel)
					m_get_angle_panel->GetWindow()->SendMessage(pMsg->message,
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

void     RotationCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
										   void* params) 
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
				m_message.LoadString(IDS_ROT_SEL_OBJ);
				m_object_lines.clear();
				if (sgGetScene()->GetSelectedObjectsList()->GetCount()>0)
				{
					sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
					while (curObj) 
					{
						curObj->Select(false);
						curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
					}
				}
				break;
			case 1:
				m_message.LoadString(IDS_ROT_SEL_F_P);
				break;
			case 2:
				m_message.LoadString(IDS_ROT_SEL_S_P);
				break;
			case 3:
				m_message.LoadString(IDS_ROT_ANG);
				break;
			default:
				ASSERT(0);
				break;
		}
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		return;
	}
	if (mes==ICommander::CM_SELECT_OBJECT)
	{
		ASSERT(params!=NULL);
		ASSERT(m_step==0);
		sgCObject* so = (sgCObject*)params;
		if (so!=m_app->GetViewPort()->GetHotObject())
			m_app->GetViewPort()->SetHotObject(so);
		
		m_app->GetViewPort()->InvalidateViewPort();
	}
}



static   bool isObjAddToList(sgCObject* obj)
{
	SG_OBJECT_TYPE ot = obj->GetType();
	if (ot==SG_OT_LINE ||
		ot==SG_OT_ARC ||
		ot==SG_OT_CIRCLE)
			return true;
	if (ot==SG_OT_SPLINE)
	{
		sgCSpline* spl = reinterpret_cast<sgCSpline*>(obj);
		if (spl->IsLinear() && !spl->IsSelfIntersecting())
			return true;
		if (spl->IsPlane(NULL,NULL) && !spl->IsSelfIntersecting())
			return true;
		else
			return false;
	}
	if (ot==SG_OT_CONTOUR)
	{
		sgCContour* cntr = reinterpret_cast<sgCContour*>(obj);
		if (cntr->IsLinear() && !cntr->IsSelfIntersecting())
			return true;
		if (cntr->IsPlane(NULL,NULL) && !cntr->IsSelfIntersecting())
			return true;
		else
			return false;
	}
	return false;
}


void  RotationCommand::Start()	
{
	SWITCH_RESOURCE

	m_app->GetCommandPanel()->RemoveAllDialogs();

	m_message.LoadString(IDS_TOOLTIP_ZERO);
	m_app->StartCommander(m_message);

	m_message.LoadString(IDS_ROTAT_CONTOUR);
	m_get_contour_panel = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,m_message,true));
	
	m_message.LoadString(IDS_FIRST_POINT);
	m_get_first_point_panel = reinterpret_cast<IGetPointPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,m_message,true));
	
	m_message.LoadString(IDS_SECOND_POINT);
	m_get_second_point_panel = reinterpret_cast<IGetPointPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,m_message,true));
	
	m_message.LoadString(IDS_ANGLE);
	m_get_angle_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,m_message,true));
	

	m_get_contour_panel->SetMultiselectMode(false);
	m_get_contour_panel->FillList(isObjAddToList);
	
	m_step=0;
	m_app->GetCommandPanel()->SetActiveRadio(0);

	m_message.LoadString(IDS_ROT_SEL_OBJ);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

}

void  RotationCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (m_step==0)
	{
		if (!(nFlags & MK_LBUTTON))
		{
			int snapSz = m_app->GetViewPort()->GetSnapSize();

			sgCObject* ho = m_app->GetViewPort()->GetTopObject(
				m_app->GetViewPort()->GetHitsInRect(CRect(pX-snapSz, pY-snapSz,
				pX+snapSz, pY+snapSz)));
			if (ho && isObjAddToList(ho))
			{
				m_app->GetViewPort()->SetHotObject(ho);
				if (m_get_contour_panel)
					m_get_contour_panel->SelectObject(ho,true);
			}
			else
			{
				m_app->GetViewPort()->SetHotObject(NULL);
				if (m_get_contour_panel)
					m_get_contour_panel->SelectObject(NULL,true);
			}
			m_app->GetViewPort()->InvalidateViewPort();
		}
		return;
	}
	else
		if (m_step==1)
		{
			ASSERT(m_get_first_point_panel);
			if (m_need_third_pnt_for_plane)
			{
					IViewPort::GET_SNAP_IN in_arg;
					in_arg.scrX = pX;
					in_arg.scrY = pY;
					in_arg.snapType = SNAP_SYSTEM;
					in_arg.XFix = m_get_first_point_panel->IsXFixed();
					in_arg.YFix = m_get_first_point_panel->IsYFixed();
					in_arg.ZFix = m_get_first_point_panel->IsZFixed();
					double tmpFl[3];
					m_get_first_point_panel->GetPoint(tmpFl[0],tmpFl[1],tmpFl[2]);
					in_arg.FixPoint.x = tmpFl[0];
					in_arg.FixPoint.y = tmpFl[1];
					in_arg.FixPoint.z = tmpFl[2];
					IViewPort::GET_SNAP_OUT out_arg;
					m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
					m_cur_pnt = out_arg.result_point;
					m_bad_point_on_plane = false;
			}
			else
			{
				if (m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,m_plane_Normal,
													m_plane_D,m_cur_pnt))
					m_bad_point_on_plane = false;
				else
					m_bad_point_on_plane = true;
			}
			m_get_first_point_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		else
			if (m_step==2)
			{
				ASSERT(m_get_second_point_panel);
				if (m_need_fourth_pnt_for_plane)
				{
					IViewPort::GET_SNAP_IN in_arg;
					in_arg.scrX = pX;
					in_arg.scrY = pY;
					in_arg.snapType = SNAP_SYSTEM;
					in_arg.XFix = m_get_second_point_panel->IsXFixed();
					in_arg.YFix = m_get_second_point_panel->IsYFixed();
					in_arg.ZFix = m_get_second_point_panel->IsZFixed();
					double tmpFl[3];
					m_get_second_point_panel->GetPoint(tmpFl[0],tmpFl[1],tmpFl[2]);
					in_arg.FixPoint.x = tmpFl[0];
					in_arg.FixPoint.y = tmpFl[1];
					in_arg.FixPoint.z = tmpFl[2];
					IViewPort::GET_SNAP_OUT out_arg;
					m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
					m_cur_pnt = out_arg.result_point;
					m_bad_point_on_plane = false;
				}
				else
				{
					if (m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,m_plane_Normal,
						m_plane_D,m_cur_pnt))
						m_bad_point_on_plane = false;
					else
						m_bad_point_on_plane = true;
				}
				m_get_second_point_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
				m_app->GetViewPort()->InvalidateViewPort();
			}
			else
				if (m_step==3)
				{
					ASSERT(m_get_angle_panel);
					SG_VECTOR lineDir;
					lineDir.x = m_axe_p2.x - m_axe_p1.x;
					lineDir.y = m_axe_p2.y - m_axe_p1.y;
					lineDir.z = m_axe_p2.z - m_axe_p1.z;
					sgSpaceMath::NormalVector(lineDir);
					double plD;
					sgSpaceMath::PlaneFromNormalAndPoint(m_obj_gab_center,lineDir,plD);
					m_app->GetViewPort()->ProjectScreenPointOnPlane(pX,pY,lineDir,plD,m_cur_pnt);
					SG_VECTOR aVec,bVec;
					aVec.x = m_obj_gab_center.x - m_arc_center.x;
					aVec.y = m_obj_gab_center.y - m_arc_center.y;
					aVec.z = m_obj_gab_center.z - m_arc_center.z;
					sgSpaceMath::NormalVector(aVec);
					bVec.x = m_cur_pnt.x - m_arc_center.x;
					bVec.y = m_cur_pnt.y - m_arc_center.y;
					bVec.z = m_cur_pnt.z - m_arc_center.z;
					sgSpaceMath::NormalVector(bVec);
					double ang = acos(sgSpaceMath::VectorsScalarMult(aVec,bVec));
					if (sgSpaceMath::VectorsScalarMult(sgSpaceMath::VectorsVectorMult(aVec,bVec),lineDir)<0)
						ang=2.0*3.14159265-ang;
					m_exist_draw_arc = m_draw_arc.FromCenterBeginNormalAngle(m_arc_center, m_obj_gab_center, lineDir,ang);
					if (m_exist_draw_arc)
					{
						ASSERT(m_get_angle_panel);
						m_get_angle_panel->SetNumber(ang/3.14159265*180.0);
						m_app->GetViewPort()->InvalidateViewPort();
					}
				}
}

static   bool isWantClose(sgCObject* obj)
{
	SG_OBJECT_TYPE ot = obj->GetType();
	switch(ot) {
	case SG_OT_LINE:
	case SG_OT_ARC:
		return false;
		break;
	case SG_OT_CIRCLE:
		return true;
		break;
	case SG_OT_SPLINE:
		{
			sgCSpline* spl = reinterpret_cast<sgCSpline*>(obj);
			return spl->IsClosed();
		}
		break;
	case SG_OT_CONTOUR:
		{
			sgCContour* cntr = reinterpret_cast<sgCContour*>(obj);
			return cntr->IsClosed();
		}
		break;
	default:
		ASSERT(0);
	}
	
	return false;
}

static   bool isNeedThirdPoint(sgCObject* obj, SG_VECTOR& plN, double& plD)
{
	SG_OBJECT_TYPE ot = obj->GetType();
	switch(ot) 
	{
	case SG_OT_LINE:
		return true;
	case SG_OT_ARC:
		{
			sgCArc* arcc = reinterpret_cast<sgCArc*>(obj);
			sgSpaceMath::PlaneFromNormalAndPoint(arcc->GetGeometry()->center,
													arcc->GetGeometry()->normal,
													plD);
			plN = arcc->GetGeometry()->normal;
		}
		return false;
		break;
	case SG_OT_CIRCLE:
		{
			sgCCircle* ccir = reinterpret_cast<sgCCircle*>(obj);
			sgSpaceMath::PlaneFromNormalAndPoint(ccir->GetGeometry()->center,
				ccir->GetGeometry()->normal,
				plD);
			plN = ccir->GetGeometry()->normal;
		}
		return false;
		break;
	case SG_OT_SPLINE:
		{
			sgCSpline* spl = reinterpret_cast<sgCSpline*>(obj);
			if (spl->IsLinear())
				return true;
			else
				if (spl->IsPlane(&plN,&plD))
					return false;
				else
					ASSERT(0);
		}
		break;
	case SG_OT_CONTOUR:
		{
			sgCContour* cntr = reinterpret_cast<sgCContour*>(obj);
			if (cntr->IsLinear())
				return true;
			else
				if (cntr->IsPlane(&plN,&plD))
					return false;
				else
					ASSERT(0);
		}
		break;
	default:
		ASSERT(0);
	}
	return true;
}


static bool setPlane(sgCObject* obj, const SG_POINT& cur_pnt,SG_VECTOR& plN, double& plD)
{
	SG_OBJECT_TYPE ot = obj->GetType();
	switch(ot) 
	{
	case SG_OT_LINE:
		{
			sgCLine* ln = reinterpret_cast<sgCLine*>(obj);
			return sgSpaceMath::PlaneFromPoints(ln->GetGeometry()->p1,
									ln->GetGeometry()->p2,
									cur_pnt,
									plN,plD);
		}
		break;
	case SG_OT_ARC:
	case SG_OT_CIRCLE:
		ASSERT(0);
		return true;
		break;
	case SG_OT_SPLINE:
		{
			sgCSpline* spl = reinterpret_cast<sgCSpline*>(obj);
			if (!spl->IsLinear())
			{
				ASSERT(0);
				return true;
			}
			else
			{
				const SG_POINT* splPnts = spl->GetGeometry()->GetPoints();
				return sgSpaceMath::PlaneFromPoints(splPnts[0],splPnts[1],
					cur_pnt,
					plN,plD);
			}
		}
		break;
	case SG_OT_CONTOUR:
		{
			sgCContour* cntr = reinterpret_cast<sgCContour*>(obj);
			if (!cntr->IsLinear())
			{
				ASSERT(0);
				return true;
			}
			else
			{
				SG_POINT endPnts[2];
				//cntr->GetEndPoints(endPnts[0],endPnts[1]);
				endPnts[0] = cntr->GetPointFromCoefficient(0.0);
				endPnts[1] = cntr->GetPointFromCoefficient(1.0);
				return sgSpaceMath::PlaneFromPoints(endPnts[0],endPnts[1],
					cur_pnt,
					plN,plD);
			}
		}
		break;
	default:
		ASSERT(0);
	}
	return false;
}

void   RotationCommand::FillObjectLinesList(sgCObject* obj)
{
	if (!obj)
	{
		ASSERT(0);
		return;
	}
	SG_OBJECT_TYPE ot = obj->GetType();
	SG_LINE  tmpLn;
	switch(ot) 
	{
	case SG_OT_LINE:
		{
			sgCLine* ln = reinterpret_cast<sgCLine*>(obj);
			memcpy(&tmpLn,ln->GetGeometry(),sizeof(SG_LINE));
			m_object_lines.push_back(tmpLn);
		}
		break;
	case SG_OT_ARC:
		{
			sgCArc* arcc = reinterpret_cast<sgCArc*>(obj);
			const int pc = arcc->GetPointsCount();
			const SG_POINT* pnts = arcc->GetPoints();
			if (pc>2)
			{
				for (int i=0;i<pc-1;i++)
				{
					tmpLn.p1 = pnts[i];
					tmpLn.p2 = pnts[i+1];
					m_object_lines.push_back(tmpLn);
				}
			}
		}
		break;
	case SG_OT_CIRCLE:
		{
			sgCCircle* ccir = reinterpret_cast<sgCCircle*>(obj);
			const int pc = ccir->GetPointsCount();
			const SG_POINT* pnts = ccir->GetPoints();
			if (pc>2)
			{
				for (int i=0;i<pc-1;i++)
				{
					tmpLn.p1 = pnts[i];
					tmpLn.p2 = pnts[i+1];
					m_object_lines.push_back(tmpLn);
				}
			}
		}
		break;
	case SG_OT_SPLINE:
		{
			sgCSpline* spl = reinterpret_cast<sgCSpline*>(obj);
			const int pc = spl->GetGeometry()->GetPointsCount();
			const SG_POINT* pnts = spl->GetGeometry()->GetPoints();
			if (pc>2)
			{
				for (int i=0;i<pc-1;i++)
				{
					tmpLn.p1 = pnts[i];
					tmpLn.p2 = pnts[i+1];
					m_object_lines.push_back(tmpLn);
				}
			}
		}
		break;
	case SG_OT_CONTOUR:
		{
			sgCContour* cntr = reinterpret_cast<sgCContour*>(obj);
			sgCObject*  curObj = cntr->GetChildrenList()->GetHead();
			while (curObj) 
			{
				FillObjectLinesList(curObj);
				curObj = cntr->GetChildrenList()->GetNext(curObj);
			}
		}
		break;
	default:
		ASSERT(0);
	}
}

bool   RotationCommand::IsIntersect(SG_LINE* ln)
{
	ASSERT(ln);
	std::list<SG_LINE>::iterator Iter;
	SG_POINT tP;
	for ( Iter = m_object_lines.begin( ); Iter != m_object_lines.end( ); Iter++ )
		if (sgSpaceMath::IsSegmentsIntersecting((*Iter),false,*ln,true,tP))
			return true;
	return false;
}

void  RotationCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	if (m_step==0)
	{
		m_first_obj=m_app->GetViewPort()->GetHotObject();
		if (m_first_obj)
		{
			m_object_lines.clear();
			FillObjectLinesList(m_first_obj);
			m_want_close = isWantClose(m_first_obj);
			m_need_third_pnt_for_plane = isNeedThirdPoint(m_first_obj, m_plane_Normal, m_plane_D);
			m_first_obj->Select(true);
			m_step++;
			m_message.LoadString(IDS_ROT_SEL_F_P);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			m_app->GetCommandPanel()->SetActiveRadio(m_step);
			m_app->GetViewPort()->InvalidateViewPort();

			SG_POINT oMin,oMax;
			m_first_obj->GetGabarits(oMin,oMax);
			m_obj_gab_center.x = (oMin.x+oMax.x)/2.0;
			m_obj_gab_center.y = (oMin.y+oMax.y)/2.0;
			m_obj_gab_center.z = (oMin.z+oMax.z)/2.0;
		}
		else
		{
			m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
			m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
			return;
		}
	}
	else
		if (m_step==1)
		{
			if (!m_need_third_pnt_for_plane && m_bad_point_on_plane)
			{
				m_message.LoadString(IDS_ROT_ERROR_POINTS_MUST_BE_ON_PLANE);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}
			m_axe_p1 = m_cur_pnt;
			if (m_need_third_pnt_for_plane)
				m_need_fourth_pnt_for_plane = !setPlane(m_first_obj,m_cur_pnt,m_plane_Normal, m_plane_D);
			m_step++;
			m_app->GetCommandPanel()->SetActiveRadio(m_step);
			m_message.LoadString(IDS_ROT_SEL_S_P);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		}
		else
			if (m_step==2)
			{
				if (sgSpaceMath::PointsDistance(m_axe_p1,m_cur_pnt)<0.000001)
				{
					m_message.LoadString(IDS_ROT_ERROR_AXE_POINTS_ARE_QUIV);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				if (!m_need_fourth_pnt_for_plane && m_bad_point_on_plane)
				{
					m_message.LoadString(IDS_ROT_ERROR_POINTS_MUST_BE_ON_PLANE);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
					return;
				}
				m_axe_p2 = m_cur_pnt;
				if (m_need_fourth_pnt_for_plane)
					if (!setPlane(m_first_obj,m_cur_pnt,m_plane_Normal, m_plane_D))
					{
						m_message.LoadString(IDS_ROT_ERROR_AXE_ONE_ONE_LINE_WITH_OBJ);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
						return;
					}
				SG_LINE lll;
				lll.p1 = m_axe_p1;
				lll.p2 = m_axe_p2;
				if (IsIntersect(&lll))
				{
					m_message.LoadString(IDS_ROT_ERROR_OBJ_AND_AXE_INTERS);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
					return;
				}
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_ROT_ANG);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

				SG_VECTOR lineDir;
				lineDir.x = m_axe_p2.x - m_axe_p1.x;
				lineDir.y = m_axe_p2.y - m_axe_p1.y;
				lineDir.z = m_axe_p2.z - m_axe_p1.z;
				sgSpaceMath::ProjectPointToLineAndGetDist(m_axe_p1,lineDir,m_obj_gab_center,m_arc_center);

			}
			else
				if (m_step==3)
				{
					if (m_exist_draw_arc)
					{
						m_angle = m_draw_arc.angle/3.14159265*180.0;
						if (m_angle>359.99)
							m_want_close = false;
						if (m_want_close)
						{
							m_message.LoadString(IDS_LOCK_OBJ);
							CString qe;
							qe.LoadString(IDS_QUESTION);
							m_inQuestionRegime = true;
							if (MessageBox(m_app->GetViewPort()->GetWindow()->m_hWnd,
								m_message,qe,MB_YESNO|MB_ICONQUESTION)==IDYES)
								m_want_close = true;
							else
								m_want_close = false;
							m_inQuestionRegime = false;
						}

						sgCObject* rO = sgKinematic::Rotation(*(reinterpret_cast<sgC2DObject*>(m_first_obj)),
							m_axe_p1,
							m_axe_p2,
							m_angle,
							m_want_close);
						if (!rO)
						{
							m_message.LoadString(IDS_ERR_CANT_CREATE);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
							return;
						}

						CString nm;
						nm.LoadString(IDS_ROT_NAME);
						CString nmInd;
						nmInd.Format("%i",rotation_name_index);
						nm+=nmInd;
						rO->SetName(nm);

						rO->SetUserGeometry("{D5E003ED-B53E-40b8-9ABB-2E7B6E72D7DC}",0,NULL);

						sgGetScene()->StartUndoGroup();
						sgGetScene()->AttachObject(rO);
						m_app->ApplyAttributes(rO);
						sgGetScene()->EndUndoGroup();
						rotation_name_index++;

						m_step=0;
						m_app->GetCommandPanel()->SetActiveRadio(0);
						m_message.LoadString(IDS_ROT_SEL_OBJ);
						m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

						if (!m_object_lines.empty())
							m_object_lines.clear();
						if (sgGetScene()->GetSelectedObjectsList()->GetCount()>0)
						{
							sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
							while (curObj) 
							{
								curObj->Select(false);
								curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
							}
						}

						m_app->GetViewPort()->InvalidateViewPort();
					}
				}
}


void  RotationCommand::Draw()
{
	switch(m_step) 
	{
	case 0:
		break;
	case 1:
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
		}
		break;
	case 2:
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_axe_p1);
			SG_LINE ln;
			ln.p1 = m_axe_p1;
			ln.p2 = m_cur_pnt;
			ln.p1.x+=m_axe_p1.x-m_cur_pnt.x;
			ln.p1.y+=m_axe_p1.y-m_cur_pnt.y;
			ln.p1.z+=m_axe_p1.z-m_cur_pnt.z;
			ln.p2.x-=m_axe_p1.x-m_cur_pnt.x;
			ln.p2.y-=m_axe_p1.y-m_cur_pnt.y;
			ln.p2.z-=m_axe_p1.z-m_cur_pnt.z;
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawLine(ln);
		}
		break;
	case 3:
		{
			if (m_exist_draw_arc)
			{
				float pC[3];
				m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->DrawArc(m_draw_arc);

				sgCMatrix mmm;
				SG_VECTOR dii;
				dii.x = m_axe_p2.x-m_axe_p1.x;
				dii.y = m_axe_p2.y-m_axe_p1.y;
				dii.z = m_axe_p2.z-m_axe_p1.z;
				mmm.Rotate(m_axe_p1,dii,m_draw_arc.angle);
				m_app->GetViewPort()->GetPainter()->SetTransformMatrix(&mmm);
				m_app->GetViewPort()->GetPainter()->DrawObject(m_first_obj);
				m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);
			}
		}
		break;
	/*default:
		ASSERT(0);*/
	}
}


void  RotationCommand::OnEnter()
{
	SWITCH_RESOURCE
		if (m_step==0)
		{
			m_first_obj=m_app->GetViewPort()->GetHotObject();
			if (m_first_obj)
			{
				m_want_close = isWantClose(m_first_obj);
				m_need_third_pnt_for_plane = isNeedThirdPoint(m_first_obj, m_plane_Normal, m_plane_D);
				m_first_obj->Select(true);

				//sgGetScene()->AttachObject(sgKinematic::Rotation(m_first_obj,NULL,NULL,270.0,false));		
				m_step++;
				m_message.LoadString(IDS_ROT_SEL_F_P);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_app->GetViewPort()->InvalidateViewPort();

				SG_POINT oMin,oMax;
				m_first_obj->GetGabarits(oMin,oMax);
				m_obj_gab_center.x = (oMin.x+oMax.x)/2.0;
				m_obj_gab_center.y = (oMin.y+oMax.y)/2.0;
				m_obj_gab_center.z = (oMin.z+oMax.z)/2.0;
			}
			else
			{
				m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}
		}
		else
			if (m_step==1)
			{
				m_get_first_point_panel->GetPoint(m_axe_p1.x,m_axe_p1.y,m_axe_p1.z);
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_ROT_SEL_S_P);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetViewPort()->InvalidateViewPort();
			}
			else
				if (m_step==2)
				{
					m_get_second_point_panel->GetPoint(m_axe_p2.x,m_axe_p2.y,m_axe_p2.z);
					m_step++;
					m_app->GetCommandPanel()->SetActiveRadio(m_step);
					m_message.LoadString(IDS_ROT_ANG);
					m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
					m_app->GetViewPort()->InvalidateViewPort();

					SG_VECTOR lineDir;
					lineDir.x = m_axe_p2.x - m_axe_p1.x;
					lineDir.y = m_axe_p2.y - m_axe_p1.y;
					lineDir.z = m_axe_p2.z - m_axe_p1.z;
					sgSpaceMath::ProjectPointToLineAndGetDist(m_axe_p1,lineDir,m_obj_gab_center,m_arc_center);

				}
				else
					if (m_step==3)
					{
						m_angle = m_get_angle_panel->GetNumber();
						if (m_angle>359.99)
							m_want_close = false;
						if (m_want_close)
						{
							m_message.LoadString(IDS_LOCK_OBJ);
							CString qe;
							qe.LoadString(IDS_QUESTION);
							m_inQuestionRegime = true;
							if (MessageBox(m_app->GetViewPort()->GetWindow()->m_hWnd,
											m_message,qe,MB_YESNO|MB_ICONQUESTION)==IDYES)
								m_want_close = true;
							else
								m_want_close = false;
							m_inQuestionRegime = false;
						}
						
						sgCObject* rO = sgKinematic::Rotation(*(reinterpret_cast<sgC2DObject*>(m_first_obj)),
											m_axe_p1,
											m_axe_p2,
											m_angle,
											m_want_close);
						if (!rO)
						{
							m_message.LoadString(IDS_ERR_CANT_CREATE);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
							return;
						}

						CString nm;
						nm.LoadString(IDS_ROT_NAME);
						CString nmInd;
						nmInd.Format("%i",rotation_name_index);
						nm+=nmInd;
						rO->SetName(nm);

						rO->SetUserGeometry("{D5E003ED-B53E-40b8-9ABB-2E7B6E72D7DC}",0,NULL);

						sgGetScene()->StartUndoGroup();
						sgGetScene()->AttachObject(rO);
						m_app->ApplyAttributes(rO);
						sgGetScene()->EndUndoGroup();
						rotation_name_index++;

						m_step=0;
						m_app->GetCommandPanel()->SetActiveRadio(0);
						m_message.LoadString(IDS_ROT_SEL_OBJ);
						m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

						if (!m_object_lines.empty())
							m_object_lines.clear();
						if (sgGetScene()->GetSelectedObjectsList()->GetCount()>0)
						{
							sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
							while (curObj) 
							{
								curObj->Select(false);
								curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
							}
						}

						m_app->GetViewPort()->InvalidateViewPort();
					}
}

unsigned int  RotationCommand::GetItemsCount()
{
	return 0;
}

void         RotationCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	
}

void     RotationCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	
}

HBITMAP   RotationCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         RotationCommand::Run(unsigned int itemID)
{
	
}
