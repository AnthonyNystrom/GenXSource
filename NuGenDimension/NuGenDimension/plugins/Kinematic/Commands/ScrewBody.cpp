#include "stdafx.h"

#include "ScrewBody.h"

#include "..//resource.h"

#include <math.h>

#include <algorithm>

int     screw_name_index = 1;

ScrewCommand::ScrewCommand(IApplicationInterface* appI):
						m_app(appI)
							,m_get_main_object_panel(NULL)
							,m_get_sub_objects_panel(NULL)
							,m_get_f_p_panel(NULL)
							,m_get_s_p_panel(NULL)
							,m_get_step_panel(NULL)
						    ,m_get_len_panel(NULL)
							,m_need_third_pnt_for_plane(false)
							,m_need_fourth_pnt_for_plane(false)
							,m_bad_point_on_plane(false)
							,m_inQuestionRegime(false)
{
	ASSERT(m_app);
	m_step = 0;
	m_otv.clear();
	m_cur_pnt.x = m_cur_pnt.y = m_cur_pnt.z = 0.0;
}

ScrewCommand::~ScrewCommand()
{
	m_otv.clear();
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


bool    ScrewCommand::PreTranslateMessage(MSG* pMsg)
{
	try {  //#try
		if (m_inQuestionRegime)
			return false;

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
				if (m_get_main_object_panel)
					m_get_main_object_panel->GetWindow()->SendMessage(pMsg->message,
								pMsg->wParam,
								pMsg->lParam);
				break;
			case 1:
				if (m_get_sub_objects_panel)
					m_get_sub_objects_panel->GetWindow()->SendMessage(pMsg->message,
									pMsg->wParam,
									pMsg->lParam);
			case 2:
				if (m_get_f_p_panel)
					m_get_f_p_panel->GetWindow()->SendMessage(pMsg->message,
									pMsg->wParam,
									pMsg->lParam);
				break;
			case 3:
				if (m_get_s_p_panel)
					m_get_s_p_panel->GetWindow()->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
				break;
			case 4:
				if (m_get_step_panel)
					m_get_step_panel->GetWindow()->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
				break;
			case 5:
				if (m_get_len_panel)
					m_get_len_panel->GetWindow()->SendMessage(pMsg->message,
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

void     ScrewCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
										   void* params) 
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=5);
		m_step = (unsigned int)newActiveDlg;
		for (unsigned int i=m_step+1;i<=5;i++)
			m_app->GetCommandPanel()->EnableRadio(i,false);
		SWITCH_RESOURCE
			switch(m_step) 
			{
				case 0:
					m_message.LoadString(IDS_SEL_MAIN_CONT);
					m_get_main_object_panel->SelectObject(NULL,false);
					if (m_first_obj)
					{
						m_get_sub_objects_panel->AddObject(m_first_obj,false);
						m_first_obj->Select(false);
					}
					m_first_obj = NULL;
					m_get_sub_objects_panel->SelectObject(NULL,false);
					for (size_t i=0;i<m_otv.size();i++)
						m_otv[i]->Select(false);
					m_otv.clear();
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
					break;
				case 1:
					m_get_sub_objects_panel->SelectObject(NULL,false);
					for (size_t i=0;i<m_otv.size();i++)
						m_otv[i]->Select(false);
					m_otv.clear();
					m_message.LoadString(IDS_SEL_OTV);
					break;
				case 2:
					m_message.LoadString(IDS_SCR_ENTER_F_PNT);
					break;
				case 3:
					m_message.LoadString(IDS_SCR_ENTER_S_PNT);
					break;
				case 4:
					m_message.LoadString(IDS_SCR_ENTER_STEP);
					break;
				case 5:
					m_message.LoadString(IDS_SCR_ENTER_LEN);
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
		if (m_step>1)
			return;
		sgCObject* so = (sgCObject*)params;
		if (so!=m_app->GetViewPort()->GetHotObject())
			m_app->GetViewPort()->SetHotObject(so);

		if (m_step==0)
		{
			m_first_obj = so;
		}
		else
		{
			if (!so->IsSelect())
			{
				SWITCH_RESOURCE
				sgC2DObject* curH = reinterpret_cast<sgC2DObject*>(m_app->GetViewPort()->GetHotObject());
				HO_CORRECT_PATHS_RES testRes = GoodObjectForHole(curH);
				if (testRes!=HO_SUCCESS)
				{
					switch(testRes) 
					{
					case HO_NOT_IN_ONE_PLANE:
						m_message.LoadString(IDS_NOT_IN_ONE_PLANE);
						break;
					case HO_INTERSECTS_WITH_OUT:
						m_message.LoadString(IDS_INTERS_OUT);
						break;
					case HO_IN_NOT_INSIDE_OUT:
						m_message.LoadString(IDS_NOT_INSIDE_OUT);
						break;
					case HO_INTERSECTS_WITH_OTHER_HOLE:
						m_message.LoadString(IDS_INTERS_WITH_OTHERS);
						break;
					case HO_INSIDE_EXIST_HOLE:
						m_message.LoadString(IDS_INSIDE_EXIST_HOLE);
						break;
					case HO_CONTAIN_EXIST_HOLE:
						m_message.LoadString(IDS_CONSIST_EXIST_HOLE);
						break;
					default:
						m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
						break;
					}
					m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
					m_get_sub_objects_panel->SelectObject(so,false);
					return;
				}
			}
			so->Select(!so->IsSelect());
			if (so->IsSelect())
				m_otv.push_back(so);
			else
			{
				std::vector<sgCObject*>::iterator resultIt;
				resultIt = std::find(m_otv.begin(),m_otv.end(),so);
				if (resultIt!=m_otv.end())
					m_otv.erase(resultIt);
			}
		}
		
		m_app->GetViewPort()->InvalidateViewPort();
	}
}



static   bool isObjAddToList1(sgCObject* obj)
{
	SG_OBJECT_TYPE ot = obj->GetType();
	if (ot==SG_OT_CIRCLE || ot==SG_OT_ARC || ot==SG_OT_LINE)
			return true;
	if (ot==SG_OT_SPLINE)
	{
		sgCSpline* spl = reinterpret_cast<sgCSpline*>(obj);
		if (spl->IsPlane(NULL,NULL) && !spl->IsSelfIntersecting())
			return true;
		else
			return false;
	}
	if (ot==SG_OT_CONTOUR)
	{
		sgCContour* cntr = reinterpret_cast<sgCContour*>(obj);
		if (cntr->IsPlane(NULL,NULL) && !cntr->IsSelfIntersecting())
			return true;
		else
			return false;
	}
	return false;
}

static   bool isObjAddToList2(sgCObject* obj)
{
	SG_OBJECT_TYPE ot = obj->GetType();
	if (ot==SG_OT_CIRCLE)
		return true;
	if (ot==SG_OT_SPLINE)
	{
		sgCSpline* spl = reinterpret_cast<sgCSpline*>(obj);
		if (spl->IsPlane(NULL,NULL) && spl->IsClosed() && !spl->IsSelfIntersecting())
			return true;
		else
			return false;
	}
	if (ot==SG_OT_CONTOUR)
	{
		sgCContour* cntr = reinterpret_cast<sgCContour*>(obj);
		if (cntr->IsPlane(NULL,NULL) && cntr->IsClosed() && !cntr->IsSelfIntersecting())
			return true;
		else
			return false;
	}
	return false;
}


void  ScrewCommand::Start()	
{
	SWITCH_RESOURCE

	m_app->GetCommandPanel()->RemoveAllDialogs();

	m_message.LoadString(IDS_SCREW_NAME);
	m_app->StartCommander(m_message);

	m_message.LoadString(IDS_MAIN_CON);
	m_get_main_object_panel = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,m_message,true));
	
	m_message.LoadString(IDS_OTV);
	m_get_sub_objects_panel = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,m_message,true));

	m_message.LoadString(IDS_FIRST_POINT);
	m_get_f_p_panel = reinterpret_cast<IGetPointPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,m_message,true));

	m_message.LoadString(IDS_SECOND_POINT);
	m_get_s_p_panel = reinterpret_cast<IGetPointPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,m_message,true));

	m_message.LoadString(IDS_SCR_STEP);
	m_get_step_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
				GetCommandPanel()->
					AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,m_message,true));
	
	m_message.LoadString(IDS_SCR_LEN);
	m_get_len_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,m_message,true));


	m_get_main_object_panel->SetMultiselectMode(false);
	m_get_main_object_panel->FillList(isObjAddToList1);
	m_get_sub_objects_panel->SetMultiselectMode(true);
	m_get_sub_objects_panel->FillList(isObjAddToList2);
	
	m_step=0;
	m_app->GetCommandPanel()->SetActiveRadio(0);

	m_message.LoadString(IDS_SEL_MAIN_CONT);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

}

void  ScrewCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (m_step==0)
	{
		if (!(nFlags & MK_LBUTTON))
		{
			int snapSz = m_app->GetViewPort()->GetSnapSize();

			sgCObject* ho = m_app->GetViewPort()->GetTopObject(
				m_app->GetViewPort()->GetHitsInRect(CRect(pX-snapSz, pY-snapSz,
				pX+snapSz, pY+snapSz)));
			if (ho && isObjAddToList1(ho))
			{
				m_app->GetViewPort()->SetHotObject(ho);
				if (m_get_main_object_panel)
					m_get_main_object_panel->SelectObject(ho,true);
			}
			else
			{
				m_app->GetViewPort()->SetHotObject(NULL);
				if (m_get_main_object_panel)
					m_get_main_object_panel->SelectObject(NULL,true);
			}
			m_app->GetViewPort()->InvalidateViewPort();
		}
		return;
	}
	else
		if (m_step==1)
		{
			if (!(nFlags & MK_LBUTTON))
			{
				int snapSz = m_app->GetViewPort()->GetSnapSize();

				sgCObject* ho = m_app->GetViewPort()->GetTopObject(
					m_app->GetViewPort()->GetHitsInRect(CRect(pX-snapSz, pY-snapSz,
					pX+snapSz, pY+snapSz)));
				if (ho && isObjAddToList2(ho))
				{
					m_app->GetViewPort()->SetHotObject(ho);
					//if (m_get_sub_objects_panel)
					//	m_get_sub_objects_panel->SelectObject(ho,true);
				}
				else
				{
					m_app->GetViewPort()->SetHotObject(NULL);
					//if (m_get_sub_objects_panel)
					//	m_get_sub_objects_panel->SelectObject(NULL,true);
				}
				m_app->GetViewPort()->InvalidateViewPort();
			}
			return;
		}
		else
			if (m_step==2)
			{
				ASSERT(m_get_f_p_panel);
				if (m_need_third_pnt_for_plane)
				{
					IViewPort::GET_SNAP_IN in_arg;
					in_arg.scrX = pX;
					in_arg.scrY = pY;
					in_arg.snapType = SNAP_SYSTEM;
					in_arg.XFix = m_get_f_p_panel->IsXFixed();
					in_arg.YFix = m_get_f_p_panel->IsYFixed();
					in_arg.ZFix = m_get_f_p_panel->IsZFixed();
					double tmpFl[3];
					m_get_f_p_panel->GetPoint(tmpFl[0],tmpFl[1],tmpFl[2]);
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
				m_get_f_p_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
				m_app->GetViewPort()->InvalidateViewPort();
			}
			else
				if (m_step==3)
				{
					ASSERT(m_get_s_p_panel);
					if (m_need_fourth_pnt_for_plane)
					{
						IViewPort::GET_SNAP_IN in_arg;
						in_arg.scrX = pX;
						in_arg.scrY = pY;
						in_arg.snapType = SNAP_SYSTEM;
						in_arg.XFix = m_get_s_p_panel->IsXFixed();
						in_arg.YFix = m_get_s_p_panel->IsYFixed();
						in_arg.ZFix = m_get_s_p_panel->IsZFixed();
						double tmpFl[3];
						m_get_s_p_panel->GetPoint(tmpFl[0],tmpFl[1],tmpFl[2]);
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
					m_get_s_p_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
					m_app->GetViewPort()->InvalidateViewPort();
				}
				else
					if (m_step==4)
					{
						if (m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,m_obj_gab_center,
								m_axe_dir_no_Normal,m_cur_pnt))
						{
							m_good_step_of_screw = true;
							SG_VECTOR tttDir;
							tttDir.x = m_cur_pnt.x - m_obj_gab_center.x;
							tttDir.y = m_cur_pnt.y - m_obj_gab_center.y;
							tttDir.z = m_cur_pnt.z - m_obj_gab_center.z;
							if (!sgSpaceMath::NormalVector(tttDir))
							{
								m_good_step_of_screw = false;
								return;
							}
							double sigg;
							if (sgSpaceMath::VectorsScalarMult(tttDir,m_axe_dir_no_Normal)>0.0)
								sigg = 1.0;
							else
								sigg = -1.0;
							m_cur_step_of_screw = sigg*sgSpaceMath::PointsDistance(m_obj_gab_center,m_cur_pnt);
							m_get_step_panel->SetNumber(m_cur_step_of_screw);
							m_app->GetViewPort()->InvalidateViewPort();
						}
						else
							m_good_step_of_screw = false;
					}
					else
						if (m_step==5)
						{
							if (m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,m_axe_p1,
								m_axe_dir_no_Normal,m_cur_pnt))
							{
								m_good_len_of_screw = true;
								SG_VECTOR tttDir;
								tttDir.x = m_cur_pnt.x - m_project_on_axe_obj_gab_center.x;
								tttDir.y = m_cur_pnt.y - m_project_on_axe_obj_gab_center.y;
								tttDir.z = m_cur_pnt.z - m_project_on_axe_obj_gab_center.z;
								if (!sgSpaceMath::NormalVector(tttDir))
								{
									m_good_len_of_screw = false;
									return;
								}
								double sigg;
								if (sgSpaceMath::VectorsScalarMult(tttDir,m_axe_dir_no_Normal)>0.0)
									sigg = 1.0;
								else
									sigg = -1.0;
								m_cur_len_of_screw = sigg*sgSpaceMath::PointsDistance(m_project_on_axe_obj_gab_center,
									m_cur_pnt);
								m_invert_length = false;
								if ((m_cur_len_of_screw*m_cur_step_of_screw)<0.0)
								{
									m_cur_len_of_screw = m_cur_len_of_screw*-1.0;
									m_invert_length = true;
								}
								m_get_len_panel->SetNumber(m_cur_len_of_screw);
								m_app->GetViewPort()->InvalidateViewPort();
							}
							else
								m_good_len_of_screw = false;
						}
					
}

void   ScrewCommand::FillObjectLinesList(sgCObject* obj)
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

HO_CORRECT_PATHS_RES  ScrewCommand::GoodObjectForHole(const sgC2DObject* try_hole)
{
	if (m_first_obj==NULL)
		return HO_UNKNOWN_ERROR;

	sgC2DObject::SG_2D_OBJECT_ORIENT out_or;

	SG_VECTOR out_Norm;
	double    out_D;

	sgC2DObject* out_obj = reinterpret_cast<sgC2DObject*>(m_first_obj);

	if (!out_obj->IsPlane(&out_Norm,&out_D))
		return HO_OUT_NOT_FLAT;
	
	out_or = out_obj->GetOrient(out_Norm);
	if (out_or == sgC2DObject::OO_ERROR) 
	{
		// ПЛОХОЙ ВНЕШНИЙ КОНТУР
		return HO_OUT_BAD_ORIENT;
	}

	if (!try_hole->IsClosed())
	{
		// ОТВЕРСТИЕ НЕ замкнутое
		return HO_IN_NO_CLOSE;
	}
	SG_VECTOR otv_Norm;
	double    otv_D;
	if (!try_hole->IsPlane(&otv_Norm,&otv_D))
	{
		// ОТВЕРСТИЕ НЕ плоское
		return HO_IN_NO_FLAT;
	}
	if (!sgC2DObject::IsObjectsOnOnePlane(*out_obj,*try_hole)) 
	{
		// ОТВЕРСТИЕ НЕ лежит в плоскости внешнего контура
		return HO_NOT_IN_ONE_PLANE;
	}
	if (try_hole->IsSelfIntersecting() ||
		sgC2DObject::IsObjectsIntersecting(*out_obj,*try_hole)) 
	{
		// ОТВЕРСТИЕ с самопересечением или пересекает внешний контур
		return HO_INTERSECTS_WITH_OUT;
	}
	if (!sgC2DObject::IsFirstObjectInsideSecondObject(*try_hole,*out_obj)) 
	{
		// ОТВЕРСТИЕ НЕ внутри внешнего контура
		return HO_IN_NOT_INSIDE_OUT;
	}

	size_t otvs_cnt = m_otv.size();

	for (size_t i=0; i<otvs_cnt; i++)
	{
		sgC2DObject* cur_otv = reinterpret_cast<sgC2DObject*>(m_otv[i]);
		
			if (sgC2DObject::IsObjectsIntersecting(*try_hole,*cur_otv)) 
			{
				// ОТВЕРСТИЕ с самопересечением или пересекает внешний контур
				return HO_INTERSECTS_WITH_OTHER_HOLE;
			}
			if (sgC2DObject::IsFirstObjectInsideSecondObject(*try_hole,*cur_otv)) 
			{
				return HO_INSIDE_EXIST_HOLE;
			}
			if (sgC2DObject::IsFirstObjectInsideSecondObject(*cur_otv,*try_hole)) 
			{
				return HO_CONTAIN_EXIST_HOLE;
			}

	}
	return HO_SUCCESS;
}


static   bool isNeedThirdPoint111(sgCObject* obj, SG_VECTOR& plN, double& plD)
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


static bool setPlane111(sgCObject* obj, const SG_POINT& cur_pnt,SG_VECTOR& plN, double& plD)
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

void  ScrewCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	if (m_step==0)
	{
		m_first_obj=m_app->GetViewPort()->GetHotObject();
		if (m_first_obj)
		{
			m_first_obj->Select(true);
			m_need_third_pnt_for_plane = isNeedThirdPoint111(m_first_obj, m_plane_Normal, m_plane_D);
			sgC2DObject* ttt = reinterpret_cast<sgC2DObject*>(m_first_obj);
		
			SG_POINT oMin,oMax;
			m_first_obj->GetGabarits(oMin,oMax);
			m_obj_gab_center.x = (oMin.x+oMax.x)/2.0;
			m_obj_gab_center.y = (oMin.y+oMax.y)/2.0;
			m_obj_gab_center.z = (oMin.z+oMax.z)/2.0;

			m_get_main_object_panel->SelectObject(m_first_obj,true);
			if (ttt->IsClosed())
			{	
				m_get_sub_objects_panel->RemoveObject(m_first_obj);
				m_step++;
				m_message.LoadString(IDS_OTV);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_app->GetViewPort()->InvalidateViewPort();
				m_otv.clear();
			}
			else
			{
				m_step+=2;
				m_message.LoadString(IDS_ROT_SEL_F_P);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_app->GetViewPort()->InvalidateViewPort();
			}

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
			if (m_app->GetViewPort()->GetHotObject())
			{
				if (m_app->GetViewPort()->GetHotObject()->IsSelect())
				{
					m_message.LoadString(IDS_CONT_ALREADY_SEL);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
					return;
				}
				sgC2DObject* curH = reinterpret_cast<sgC2DObject*>(m_app->GetViewPort()->GetHotObject());
				HO_CORRECT_PATHS_RES testRes = GoodObjectForHole(curH);
				if (testRes!=HO_SUCCESS)
				{
					switch(testRes) 
					{
					case HO_NOT_IN_ONE_PLANE:
						m_message.LoadString(IDS_NOT_IN_ONE_PLANE);
						break;
					case HO_INTERSECTS_WITH_OUT:
						m_message.LoadString(IDS_INTERS_OUT);
						break;
					case HO_IN_NOT_INSIDE_OUT:
						m_message.LoadString(IDS_NOT_INSIDE_OUT);
						break;
					case HO_INTERSECTS_WITH_OTHER_HOLE:
						m_message.LoadString(IDS_INTERS_WITH_OTHERS);
						break;
					case HO_INSIDE_EXIST_HOLE:
						m_message.LoadString(IDS_INSIDE_EXIST_HOLE);
						break;
					case HO_CONTAIN_EXIST_HOLE:
						m_message.LoadString(IDS_CONSIST_EXIST_HOLE);
						break;
					default:
						m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
						break;
					}
					m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
					return;
				}
				else
				{
					m_app->GetViewPort()->GetHotObject()->Select(true);
					m_get_sub_objects_panel->SelectObject(m_app->GetViewPort()->GetHotObject(),true);
					m_otv.push_back(m_app->GetViewPort()->GetHotObject());
					m_app->GetViewPort()->InvalidateViewPort();
				}
			}
			else
			{
				m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}
		}
		else
		if (m_step==2)
		{
			if (!m_need_third_pnt_for_plane && m_bad_point_on_plane)
			{
				m_message.LoadString(IDS_ROT_ERROR_POINTS_MUST_BE_ON_PLANE);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}
			m_axe_p1 = m_cur_pnt;
			if (m_need_third_pnt_for_plane)
				m_need_fourth_pnt_for_plane = !setPlane111(m_first_obj,m_cur_pnt,m_plane_Normal, m_plane_D);
			m_step++;
			m_app->GetCommandPanel()->SetActiveRadio(m_step);
			m_message.LoadString(IDS_SCR_ENTER_S_PNT);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		}
		else
			if (m_step==3)
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
					if (!setPlane111(m_first_obj,m_cur_pnt,m_plane_Normal, m_plane_D))
					{
						m_message.LoadString(IDS_ROT_ERROR_AXE_ONE_ONE_LINE_WITH_OBJ);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
						return;
					}
					
					m_step++;
					m_app->GetCommandPanel()->SetActiveRadio(m_step);
					m_message.LoadString(IDS_SCR_ENTER_STEP);
					m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

					m_axe_dir_no_Normal.x = m_axe_p2.x - m_axe_p1.x;
					m_axe_dir_no_Normal.y = m_axe_p2.y - m_axe_p1.y;
					m_axe_dir_no_Normal.z = m_axe_p2.z - m_axe_p1.z;

					sgSpaceMath::ProjectPointToLineAndGetDist(m_axe_p1,m_axe_dir_no_Normal,
						m_obj_gab_center,m_project_on_axe_obj_gab_center);


					/*SG_VECTOR lineDir;
					lineDir.x = m_axe_p2.x - m_axe_p1.x;
					lineDir.y = m_axe_p2.y - m_axe_p1.y;
					lineDir.z = m_axe_p2.z - m_axe_p1.z;
					sgSpaceMath::ProjectPointToLineAndGetDist(m_axe_p1,lineDir,m_obj_gab_center,m_arc_center);*/
			}
			else
				if (m_step==4)
				{
					if (m_good_step_of_screw)
					{
						m_step++;
						m_app->GetCommandPanel()->SetActiveRadio(m_step);
						m_message.LoadString(IDS_SCR_ENTER_LEN);
						m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
					}
					else
						return;
				}
				else
				if (m_step==5)
				{
					sgCObject* ooo = NULL;

					sgC2DObject* ttt = reinterpret_cast<sgC2DObject*>(m_first_obj);

					bool  cl = false;

					if (ttt && ttt->IsClosed())
					{
						m_message.LoadString(IDS_LOCK_OBJ);
						CString qe;
						qe.LoadString(IDS_QUESTION);
						m_inQuestionRegime = true;
						if (MessageBox(m_app->GetViewPort()->GetWindow()->m_hWnd,
							m_message,qe,MB_YESNO|MB_ICONQUESTION)==IDYES)
							cl = true;
						else
							cl = false;
						m_inQuestionRegime = false;
					}

					if (m_otv.size()>0)
						ooo = sgKinematic::Spiral((const sgC2DObject&)(*ttt),
						(const sgC2DObject**)(&m_otv[0]),m_otv.size(),
									m_axe_p1,m_axe_p2,m_cur_step_of_screw,m_cur_len_of_screw,24,cl);
					else
						ooo = sgKinematic::Spiral((const sgC2DObject&)(*ttt),NULL,0,
										m_axe_p1,m_axe_p2,m_cur_step_of_screw,m_cur_len_of_screw,24,cl);

					if (!ooo)
					{
						m_message.LoadString(IDS_ERR_CANT_CREATE);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
						return;
					}
					CString nm;
					nm.LoadString(IDS_SCREW_NAME);
					CString nmInd;
					nmInd.Format("%i",screw_name_index);
					nm+=nmInd;
					ooo->SetName(nm);

					ooo->SetUserGeometry("{260E8383-41CD-4552-9034-F6C830495EBD}",0,NULL);

					sgGetScene()->StartUndoGroup();
					sgGetScene()->AttachObject(ooo);
					m_app->ApplyAttributes(ooo);
					sgGetScene()->EndUndoGroup();
					screw_name_index++;

					m_otv.clear();
				
					m_step=0;
					m_app->GetCommandPanel()->SetActiveRadio(0);
					m_message.LoadString(IDS_SEL_MAIN_CONT);
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


void  ScrewCommand::Draw()
{
	switch(m_step) 
	{
	case 2:
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
		}
		break;
	case 3:
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
	case 4:
		if(!m_good_step_of_screw)
			return;
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			
			SG_ARC ff;
			SG_POINT arcCen;
			arcCen.x = m_project_on_axe_obj_gab_center.x + (m_cur_pnt.x-m_obj_gab_center.x)/4;
			arcCen.y = m_project_on_axe_obj_gab_center.y + (m_cur_pnt.y-m_obj_gab_center.y)/4;
			arcCen.z = m_project_on_axe_obj_gab_center.z + (m_cur_pnt.z-m_obj_gab_center.z)/4;
			double arcRad = sgSpaceMath::PointsDistance(arcCen,m_obj_gab_center);
			SG_VECTOR tmpVec1;
			tmpVec1.x = arcCen.x-m_obj_gab_center.x;
			tmpVec1.y = arcCen.y-m_obj_gab_center.y;
			tmpVec1.z = arcCen.z-m_obj_gab_center.z;
			SG_POINT arcSecP;
			arcSecP.x = m_obj_gab_center.x + 2*tmpVec1.x;
			arcSecP.y = m_obj_gab_center.y + 2*tmpVec1.y;
			arcSecP.z = m_obj_gab_center.z + 2*tmpVec1.z;
			SG_VECTOR tmpVec2;
			tmpVec2 = sgSpaceMath::VectorsVectorMult(tmpVec1,m_axe_dir_no_Normal);
			sgSpaceMath::NormalVector(tmpVec2);
			arcCen.x = arcCen.x+tmpVec2.x*arcRad;
			arcCen.y = arcCen.y+tmpVec2.y*arcRad;
			arcCen.z = arcCen.z+tmpVec2.z*arcRad;
			if (ff.FromThreePoints(m_obj_gab_center,arcSecP,arcCen,false))
				m_app->GetViewPort()->GetPainter()->DrawArc(ff);

			arcCen.x = arcCen.x + (m_cur_pnt.x-m_obj_gab_center.x)/2-2*arcRad*tmpVec2.x;
			arcCen.y = arcCen.y + (m_cur_pnt.y-m_obj_gab_center.y)/2-2*arcRad*tmpVec2.y;
			arcCen.z = arcCen.z + (m_cur_pnt.z-m_obj_gab_center.z)/2-2*arcRad*tmpVec2.z;
			if (ff.FromThreePoints(arcSecP,m_cur_pnt,arcCen,false))
				m_app->GetViewPort()->GetPainter()->DrawArc(ff);

			sgCMatrix mmm;
			SG_VECTOR tttDir;
			tttDir.x = m_cur_pnt.x - m_obj_gab_center.x;
			tttDir.y = m_cur_pnt.y - m_obj_gab_center.y;
			tttDir.z = m_cur_pnt.z - m_obj_gab_center.z;
			mmm.Translate(tttDir);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(&mmm);
			m_app->GetViewPort()->GetPainter()->DrawObject(m_first_obj);
			for (size_t i=0;i<m_otv.size();i++)
				m_app->GetViewPort()->GetPainter()->DrawObject(m_otv[i]);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);
		}
		break;
	case 5:
		if(!m_good_len_of_screw)
			return;
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);

			
			sgCMatrix mmm;
			SG_VECTOR tttDir;
			tttDir.x = m_cur_pnt.x - m_project_on_axe_obj_gab_center.x;
			tttDir.y = m_cur_pnt.y - m_project_on_axe_obj_gab_center.y;
			tttDir.z = m_cur_pnt.z - m_project_on_axe_obj_gab_center.z;
			if (m_invert_length)
			{
				tttDir.x = -tttDir.x;tttDir.y = -tttDir.y;tttDir.z = -tttDir.z;
			}
			SG_LINE ln;
			ln.p1 = m_project_on_axe_obj_gab_center;
			ln.p2.x = m_project_on_axe_obj_gab_center.x+tttDir.x;
			ln.p2.y = m_project_on_axe_obj_gab_center.y+tttDir.y;
			ln.p2.z = m_project_on_axe_obj_gab_center.z+tttDir.z;

			m_app->GetViewPort()->GetPainter()->DrawLine(ln);

			mmm.Translate(tttDir);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(&mmm);
			m_app->GetViewPort()->GetPainter()->DrawObject(m_first_obj);
			for (size_t i=0;i<m_otv.size();i++)
				m_app->GetViewPort()->GetPainter()->DrawObject(m_otv[i]);
			m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);
		}
		break;
	}
}


void  ScrewCommand::OnEnter()
{
	SWITCH_RESOURCE
		if (m_step==0)
		{
			m_first_obj=m_app->GetViewPort()->GetHotObject();
			if (m_first_obj)
			{
				/*m_object_lines.clear();
				FillObjectLinesList(m_first_obj);*/
				m_first_obj->Select(true);
				m_need_third_pnt_for_plane = isNeedThirdPoint111(m_first_obj, m_plane_Normal, m_plane_D);

				sgC2DObject* ttt = reinterpret_cast<sgC2DObject*>(m_first_obj);
				
				SG_POINT oMin,oMax;
				m_first_obj->GetGabarits(oMin,oMax);
				m_obj_gab_center.x = (oMin.x+oMax.x)/2.0;
				m_obj_gab_center.y = (oMin.y+oMax.y)/2.0;
				m_obj_gab_center.z = (oMin.z+oMax.z)/2.0;

				m_get_main_object_panel->SelectObject(m_first_obj,true);

				if (ttt->IsClosed())
				{
					m_get_sub_objects_panel->RemoveObject(m_first_obj);
					m_step++;
					m_message.LoadString(IDS_SEL_OTV);
					m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
					m_app->GetCommandPanel()->SetActiveRadio(m_step);
					m_app->GetViewPort()->InvalidateViewPort();
					m_otv.clear();
				}
				else
				{
					m_step+=2;
					m_message.LoadString(IDS_SCR_ENTER_F_PNT);
					m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
					m_app->GetCommandPanel()->SetActiveRadio(m_step);
					m_app->GetViewPort()->InvalidateViewPort();
				}
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
				m_step++;
				m_message.LoadString(IDS_SCR_ENTER_F_PNT);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_app->GetViewPort()->InvalidateViewPort();
			}
			else
				if (m_step==2)
				{
					m_get_f_p_panel->GetPoint(m_axe_p1.x,m_axe_p1.y,m_axe_p1.z);
					m_step++;
					m_app->GetCommandPanel()->SetActiveRadio(m_step);
					m_message.LoadString(IDS_SCR_ENTER_S_PNT);
					m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
					m_app->GetViewPort()->InvalidateViewPort();
				}
				else
					if (m_step==3)
					{
						m_get_s_p_panel->GetPoint(m_axe_p2.x,m_axe_p2.y,m_axe_p2.z);

						m_axe_dir_no_Normal.x = m_axe_p2.x - m_axe_p1.x;
						m_axe_dir_no_Normal.y = m_axe_p2.y - m_axe_p1.y;
						m_axe_dir_no_Normal.z = m_axe_p2.z - m_axe_p1.z;

						sgSpaceMath::ProjectPointToLineAndGetDist(m_axe_p1,m_axe_dir_no_Normal,
							m_obj_gab_center,m_project_on_axe_obj_gab_center);


						m_step++;
						m_app->GetCommandPanel()->SetActiveRadio(m_step);
						m_message.LoadString(IDS_SCR_ENTER_STEP);
						m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

						/*SG_VECTOR lineDir;
						lineDir.x = m_axe_p2.x - m_axe_p1.x;
						lineDir.y = m_axe_p2.y - m_axe_p1.y;
						lineDir.z = m_axe_p2.z - m_axe_p1.z;
						sgSpaceMath::ProjectPointToLineAndGetDist(m_axe_p1,lineDir,m_obj_gab_center,m_arc_center);
*/
					}
					else
					if (m_step==4)
					{
						m_cur_step_of_screw = m_get_step_panel->GetNumber();
						if (fabs(m_cur_step_of_screw)>0.0001)
							m_good_step_of_screw = true;
						else
						{
							m_message.LoadString(IDS_SCR_ERR_ZERO_STEP);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
							m_good_step_of_screw = false;
							return;
						}
						if (m_good_step_of_screw)
						{
							m_step++;
							m_app->GetCommandPanel()->SetActiveRadio(m_step);
							m_message.LoadString(IDS_SCR_ENTER_LEN);
							m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
						}
					}
					else
						if (m_step==5)
						{
							m_cur_len_of_screw = m_get_len_panel->GetNumber();
							if (fabs(m_cur_len_of_screw)>0.0001)
								m_good_len_of_screw = true;
							else
							{
								m_message.LoadString(IDS_SCR_ERR_ZERO_LEN);
								m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
								m_good_len_of_screw = false;
								return;
							}

							if (m_cur_len_of_screw*m_cur_step_of_screw<0.0)
								m_cur_len_of_screw = -1.0*m_cur_len_of_screw;
							
							sgCObject* ooo = NULL;

							sgC2DObject* ttt = reinterpret_cast<sgC2DObject*>(m_first_obj);

							bool  cl = false;

							if (ttt && ttt->IsClosed())
							{
								m_message.LoadString(IDS_LOCK_OBJ);
								CString qe;
								qe.LoadString(IDS_QUESTION);
								m_inQuestionRegime = true;
								if (MessageBox(m_app->GetViewPort()->GetWindow()->m_hWnd,
									m_message,qe,MB_YESNO|MB_ICONQUESTION)==IDYES)
									cl = true;
								else
									cl = false;
								m_inQuestionRegime = false;
							}

							if (m_otv.size()>0)
								ooo = sgKinematic::Spiral((const sgC2DObject&)(*ttt),
								(const sgC2DObject**)(&m_otv[0]),m_otv.size(),
								m_axe_p1,m_axe_p2,m_cur_step_of_screw,m_cur_len_of_screw,24,cl);
							else
								ooo = sgKinematic::Spiral((const sgC2DObject&)(*ttt),NULL,0,
								m_axe_p1,m_axe_p2,m_cur_step_of_screw,m_cur_len_of_screw,24,cl);

							if (!ooo)
							{
								m_message.LoadString(IDS_ERR_CANT_CREATE);
								m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
								return;
							}
							CString nm;
							nm.LoadString(IDS_SCREW_NAME);
							CString nmInd;
							nmInd.Format("%i",screw_name_index);
							nm+=nmInd;
							ooo->SetName(nm);

							ooo->SetUserGeometry("{260E8383-41CD-4552-9034-F6C830495EBD}",0,NULL);

							sgGetScene()->StartUndoGroup();
							sgGetScene()->AttachObject(ooo);
							m_app->ApplyAttributes(ooo);
							sgGetScene()->EndUndoGroup();
							screw_name_index++;

							m_otv.clear();

							m_step=0;
							m_app->GetCommandPanel()->SetActiveRadio(0);
							m_message.LoadString(IDS_SEL_MAIN_CONT);
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

unsigned int  ScrewCommand::GetItemsCount()
{
	return 0;
}

void         ScrewCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	
}

void     ScrewCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	
}

HBITMAP   ScrewCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         ScrewCommand::Run(unsigned int itemID)
{
	
}
