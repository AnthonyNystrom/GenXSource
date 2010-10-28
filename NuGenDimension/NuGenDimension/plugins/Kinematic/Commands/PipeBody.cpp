#include "stdafx.h"

#include "PipeBody.h"

#include "..//resource.h"

#include <math.h>

#include <algorithm>

int     pipe_name_index = 1;

PipeCommand::PipeCommand(IApplicationInterface* appI):
						m_app(appI)
							,m_get_main_object_panel(NULL)
							,m_get_sub_objects_panel(NULL)
							,m_get_pipe_profile(NULL)
							,m_pipe_cont(NULL)
{
	ASSERT(m_app);
	m_step = 0;
	m_otv.clear();
	m_inQuestionRegime = false;
}

PipeCommand::~PipeCommand()
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


bool    PipeCommand::PreTranslateMessage(MSG* pMsg)
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
				if (m_get_pipe_profile)
					m_get_pipe_profile->GetWindow()->SendMessage(pMsg->message,
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

void     PipeCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
										   void* params) 
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=1);
		m_step = (unsigned int)newActiveDlg;
		for (unsigned int i=m_step+1;i<=2;i++)
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
					m_object_lines.clear();
					break;
				case 1:
					m_get_sub_objects_panel->SelectObject(NULL,false);
					for (size_t i=0;i<m_otv.size();i++)
						m_otv[i]->Select(false);
					m_otv.clear();
					m_message.LoadString(IDS_SEL_OTV);
					break;
				case 2:
					m_message.LoadString(IDS_SEL_PIPE_CONT);
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

static   bool isObjAddToList3(sgCObject* obj)
{
	SG_OBJECT_TYPE ot = obj->GetType();
	if (ot==SG_OT_CIRCLE || ot==SG_OT_ARC || ot==SG_OT_LINE || 
		ot==SG_OT_SPLINE || ot==SG_OT_CONTOUR)
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

void  PipeCommand::Start()	
{
	SWITCH_RESOURCE

	m_app->GetCommandPanel()->RemoveAllDialogs();

	m_message.LoadString(IDS_PIPE_NAME);
	m_app->StartCommander(m_message);

	m_message.LoadString(IDS_MAIN_CON);
	m_get_main_object_panel = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,m_message,true));
	
	m_message.LoadString(IDS_OTV);
	m_get_sub_objects_panel = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,m_message,true));

	m_message.LoadString(IDS_PIPE_CONT);
	m_get_pipe_profile = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,m_message,true));

	m_get_main_object_panel->SetMultiselectMode(false);
	m_get_main_object_panel->FillList(isObjAddToList1);
	m_get_sub_objects_panel->SetMultiselectMode(true);
	m_get_sub_objects_panel->FillList(isObjAddToList2);

	m_get_pipe_profile->SetMultiselectMode(false);
	m_get_pipe_profile->FillList(isObjAddToList3);
	
	m_step=0;
	m_app->GetCommandPanel()->SetActiveRadio(0);

	m_message.LoadString(IDS_SEL_MAIN_CONT);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

}

void  PipeCommand::MouseMove(unsigned int nFlags,int pX,int pY)
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
				int snapSz = m_app->GetViewPort()->GetSnapSize();

				sgCObject* ho = m_app->GetViewPort()->GetTopObject(
					m_app->GetViewPort()->GetHitsInRect(CRect(pX-snapSz, pY-snapSz,
					pX+snapSz, pY+snapSz)));
				if (ho && isObjAddToList3(ho))
				{
					m_app->GetViewPort()->SetHotObject(ho);
					if (m_get_pipe_profile)
						m_get_pipe_profile->SelectObject(ho,true);
				}
				else
				{
					m_app->GetViewPort()->SetHotObject(NULL);
					if (m_get_pipe_profile)
						m_get_pipe_profile->SelectObject(NULL,true);
				}
				m_app->GetViewPort()->InvalidateViewPort();
			}
			else
				ASSERT(0);
}

void   PipeCommand::FillObjectLinesList(sgCObject* obj)
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

HO_CORRECT_PATHS_RES  PipeCommand::GoodObjectForHole(const sgC2DObject* try_hole)
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

void  PipeCommand::LeftClick(unsigned int nFlags,int pX,int pY)
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
			sgC2DObject* ttt = reinterpret_cast<sgC2DObject*>(m_first_obj);
			
			m_zero_point_on_cont = ttt->GetPointFromCoefficient(0.0);

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
				m_message.LoadString(IDS_SEL_PIPE_CONT);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_app->GetViewPort()->InvalidateViewPort();
				return;
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
		if (m_step==2)
		{
			if (!m_app->GetViewPort()->GetHotObject())
			{
				m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}

			sgC2DObject* fo = reinterpret_cast<sgC2DObject*>(m_first_obj);

			sgC2DObject* ttt = reinterpret_cast<sgC2DObject*>(m_app->GetViewPort()->GetHotObject());

			sgCObject* ooo=NULL;

			SG_POINT  poc = fo->GetPointFromCoefficient(0.0);

			bool  cl = false;

			if (fo && fo->IsClosed())
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
				ooo = sgKinematic::Pipe((const sgC2DObject&)(*fo),
						(const sgC2DObject**)(&m_otv[0]),m_otv.size(),
						(const sgC2DObject&)(*ttt),poc,0.0,cl);
			else
				ooo = sgKinematic::Pipe((const sgC2DObject&)(*fo),NULL,0,
							(const sgC2DObject&)(*ttt),poc,0.0,cl);


			if (!ooo)
			{
				m_message.LoadString(IDS_ERR_CANT_CREATE);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}

			CString nm;
			nm.LoadString(IDS_PIPE_NAME);
			CString nmInd;
			nmInd.Format("%i",pipe_name_index);
			nm+=nmInd;
			ooo->SetName(nm);

			ooo->SetUserGeometry("{C25A14A9-2D10-436c-A581-F3F19AD05EE8}",0,NULL);

			sgGetScene()->StartUndoGroup();
			sgGetScene()->AttachObject(ooo);
			m_app->ApplyAttributes(ooo);
			sgGetScene()->EndUndoGroup();
			pipe_name_index++;

			m_otv.clear();
			if (!m_object_lines.empty())
				m_object_lines.clear();

			m_step=0;
			m_app->GetCommandPanel()->SetActiveRadio(0);

			m_message.LoadString(IDS_MAIN_CON);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

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


void  PipeCommand::Draw()
{
}


void  PipeCommand::OnEnter()
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
				sgC2DObject* ttt = reinterpret_cast<sgC2DObject*>(m_first_obj);
				
				m_zero_point_on_cont = ttt->GetPointFromCoefficient(0.0);
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
					m_message.LoadString(IDS_SEL_PIPE_CONT);
					m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
					m_app->GetCommandPanel()->SetActiveRadio(m_step);
					m_app->GetViewPort()->InvalidateViewPort();
					return;
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
				m_message.LoadString(IDS_SEL_PIPE_CONT);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_app->GetViewPort()->InvalidateViewPort();
			}
			else
				if (m_step==2)
				{
					if (!m_app->GetViewPort()->GetHotObject())
					{
						m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
						return;
					}

					sgC2DObject* fo = reinterpret_cast<sgC2DObject*>(m_first_obj);

					sgC2DObject* ttt = reinterpret_cast<sgC2DObject*>(m_app->GetViewPort()->GetHotObject());

					sgCObject* ooo=NULL;

					SG_POINT  poc = fo->GetPointFromCoefficient(0.0);

					bool  cl = false;

					if (fo && fo->IsClosed())
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
						ooo = sgKinematic::Pipe((const sgC2DObject&)(*fo),
						(const sgC2DObject**)(&m_otv[0]),m_otv.size(),
						(const sgC2DObject&)(*ttt),poc,0.0,cl);
					else
						ooo = sgKinematic::Pipe((const sgC2DObject&)(*fo),NULL,0,
						(const sgC2DObject&)(*ttt),poc,0.0,cl);


					if (!ooo)
					{
						m_message.LoadString(IDS_ERR_CANT_CREATE);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
						return;
					}

					CString nm;
					nm.LoadString(IDS_PIPE_NAME);
					CString nmInd;
					nmInd.Format("%i",pipe_name_index);
					nm+=nmInd;
					ooo->SetName(nm);

					ooo->SetUserGeometry("{C25A14A9-2D10-436c-A581-F3F19AD05EE8}",0,NULL);

					sgGetScene()->StartUndoGroup();
					sgGetScene()->AttachObject(ooo);
					m_app->ApplyAttributes(ooo);
					sgGetScene()->EndUndoGroup();
					pipe_name_index++;

					m_otv.clear();
					if (!m_object_lines.empty())
						m_object_lines.clear();

					m_step=0;
					m_app->GetCommandPanel()->SetActiveRadio(0);

					m_message.LoadString(IDS_MAIN_CON);
					m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

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

unsigned int  PipeCommand::GetItemsCount()
{
	return 0;
}

void         PipeCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	
}

void     PipeCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	
}

HBITMAP   PipeCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         PipeCommand::Run(unsigned int itemID)
{
	
}
