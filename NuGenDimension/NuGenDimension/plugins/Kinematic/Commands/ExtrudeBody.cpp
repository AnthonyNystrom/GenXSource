#include "stdafx.h"

#include "ExtrudeBody.h"

#include "..//resource.h"

#include <math.h>

#include <algorithm>

int     extrude_name_index = 1;

ExtrudeCommand::ExtrudeCommand(IApplicationInterface* appI):
						m_app(appI)
							,m_get_main_object_panel(NULL)
							,m_get_sub_objects_panel(NULL)
							,m_get_H_panel(NULL)
{
	ASSERT(m_app);
	m_step = 0;
	m_otv.clear();
	m_inQuestionRegime = false;
}

ExtrudeCommand::~ExtrudeCommand()
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


bool    ExtrudeCommand::PreTranslateMessage(MSG* pMsg)
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
				if (m_get_H_panel)
					m_get_H_panel->GetWindow()->SendMessage(pMsg->message,
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

void     ExtrudeCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
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
					m_message.LoadString(IDS_SEL_H);
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


void  ExtrudeCommand::Start()	
{
	SWITCH_RESOURCE

	m_app->GetCommandPanel()->RemoveAllDialogs();

	m_message.LoadString(IDS_TOOLTIP_ZERO);
	m_app->StartCommander(m_message);

	m_message.LoadString(IDS_MAIN_CON);
	m_get_main_object_panel = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,m_message,true));
	
	m_message.LoadString(IDS_OTV);
	m_get_sub_objects_panel = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,m_message,true));

	m_message.LoadString(IDS_EXTR_H);
	m_get_H_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
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

void  ExtrudeCommand::MouseMove(unsigned int nFlags,int pX,int pY)
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
				m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,
					m_gab_center,m_plane_Normal,m_cur_pnt);
				ASSERT(m_get_H_panel);
				SG_VECTOR ddd;
				ddd.x = m_cur_pnt.x-m_gab_center.x;
				ddd.y = m_cur_pnt.y-m_gab_center.y;
				ddd.z = m_cur_pnt.z-m_gab_center.z;
				double hh = sqrt(ddd.x*ddd.x+ddd.y*ddd.y+ddd.z*ddd.z);
				if (sgSpaceMath::VectorsScalarMult(m_plane_Normal,ddd)<0)
					hh*=-1.0;
				m_get_H_panel->SetNumber(hh);
				m_app->GetViewPort()->InvalidateViewPort();
			}
			else
				ASSERT(0);
}

void   ExtrudeCommand::FillObjectLinesList(sgCObject* obj)
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

HO_CORRECT_PATHS_RES  ExtrudeCommand::GoodObjectForHole(const sgC2DObject* try_hole)
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

void  ExtrudeCommand::LeftClick(unsigned int nFlags,int pX,int pY)
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
			if (ttt->GetType()!=SG_OT_LINE)
			{
				ttt->IsPlane(&m_plane_Normal,NULL);
				sgSpaceMath::NormalVector(m_plane_Normal);
			}
			else
			{
				m_plane_Normal.x = 0.0;
				m_plane_Normal.y = 0.0;
				m_plane_Normal.z = 1.0;
			}
			SG_POINT oMin,oMax;
			m_first_obj->GetGabarits(oMin,oMax);
			m_gab_center.x = (oMin.x+oMax.x)/2.0;
			m_gab_center.y = (oMin.y+oMax.y)/2.0;
			m_gab_center.z = (oMin.z+oMax.z)/2.0;

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
				m_message.LoadString(IDS_SEL_H);
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
			sgCObject* ooo = NULL;

			SG_VECTOR ddd;
			ddd.x = m_cur_pnt.x-m_gab_center.x;
			ddd.y = m_cur_pnt.y-m_gab_center.y;
			ddd.z = m_cur_pnt.z-m_gab_center.z;

			double hh = sqrt(ddd.x*ddd.x+ddd.y*ddd.y+ddd.z*ddd.z);
			if (fabs(hh)<0.00001)
			{
				m_message.LoadString(IDS_ZERO_H);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}

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
				ooo = sgKinematic::Extrude((const sgC2DObject&)(*ttt),
							(const sgC2DObject**)(&m_otv[0]),m_otv.size(),ddd,cl);
			else
				ooo = sgKinematic::Extrude((const sgC2DObject&)(*ttt),NULL,0,ddd,cl);

			if (!ooo)
			{
				m_message.LoadString(IDS_ERR_CANT_CREATE);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}

			CString nm;
			nm.LoadString(IDS_EXTR_NAME);
			CString nmInd;
			nmInd.Format("%i",extrude_name_index);
			nm+=nmInd;
			ooo->SetName(nm);

			ooo->SetUserGeometry("{2D9BCE0E-4547-4432-8A5C-A4FD115E82F7}",0,NULL);


			sgGetScene()->StartUndoGroup();
			sgGetScene()->AttachObject(ooo);
			m_app->ApplyAttributes(ooo);
			sgGetScene()->EndUndoGroup();
			extrude_name_index++;

			m_otv.clear();
			if (!m_object_lines.empty())
				m_object_lines.clear();

			m_step=0;
			m_app->GetCommandPanel()->SetActiveRadio(0);

			m_message.LoadString(IDS_SEL_MAIN_CONT);
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


void  ExtrudeCommand::Draw()
{
	if (m_step==2)
	{
		float pC[3];
		m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		SG_LINE ln;
		ln.p1 = m_gab_center;
		ln.p2 = m_cur_pnt;
		m_app->GetViewPort()->GetPainter()->DrawLine(ln);

		sgCMatrix mmm;
		SG_VECTOR dii;
		dii.x = m_cur_pnt.x-m_gab_center.x;
		dii.y = m_cur_pnt.y-m_gab_center.y;
		dii.z = m_cur_pnt.z-m_gab_center.z;
		mmm.Translate(dii);
		m_app->GetViewPort()->GetPainter()->SetTransformMatrix(&mmm);
		m_app->GetViewPort()->GetPainter()->DrawObject(m_first_obj);
		for (size_t i=0;i<m_otv.size();i++)
			m_app->GetViewPort()->GetPainter()->DrawObject(m_otv[i]);
		m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);
	}
}


void  ExtrudeCommand::OnEnter()
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
				if (ttt->GetType()!=SG_OT_LINE)
				{
					ttt->IsPlane(&m_plane_Normal,NULL);
					sgSpaceMath::NormalVector(m_plane_Normal);
				}
				else
				{
					m_plane_Normal.x = 0.0;
					m_plane_Normal.y = 0.0;
					m_plane_Normal.z = 1.0;
				}
				SG_POINT oMin,oMax;
				m_first_obj->GetGabarits(oMin,oMax);
				m_gab_center.x = (oMin.x+oMax.x)/2.0;
				m_gab_center.y = (oMin.y+oMax.y)/2.0;
				m_gab_center.z = (oMin.z+oMax.z)/2.0;

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
					m_message.LoadString(IDS_SEL_H);
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
				m_message.LoadString(IDS_SEL_H);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_app->GetViewPort()->InvalidateViewPort();
			}
			else
				if (m_step==2)
				{
					sgCObject* ooo = NULL;
					
					SG_VECTOR ddd;
					double dLen = m_get_H_panel->GetNumber();

					if (fabs(dLen)<0.00001)
					{
						m_message.LoadString(IDS_ZERO_H);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
						return;
					}

					ddd.x = m_plane_Normal.x*dLen;
					ddd.y = m_plane_Normal.y*dLen;
					ddd.z = m_plane_Normal.z*dLen;

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
						ooo = sgKinematic::Extrude((const sgC2DObject&)(*ttt),
									(const sgC2DObject**)(&m_otv[0]),m_otv.size(),ddd,cl);
					else
						ooo = sgKinematic::Extrude((const sgC2DObject&)(*ttt),NULL,0,ddd,cl);


					if (!ooo)
					{
						m_message.LoadString(IDS_ERR_CANT_CREATE);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
						return;
					}

					CString nm;
					nm.LoadString(IDS_EXTR_NAME);
					CString nmInd;
					nmInd.Format("%i",extrude_name_index);
					nm+=nmInd;
					ooo->SetName(nm);

					ooo->SetUserGeometry("{2D9BCE0E-4547-4432-8A5C-A4FD115E82F7}",0,NULL);

					sgGetScene()->StartUndoGroup();
					sgGetScene()->AttachObject(ooo);
					m_app->ApplyAttributes(ooo);
					sgGetScene()->EndUndoGroup();
					extrude_name_index++;

					m_otv.clear();
					if (!m_object_lines.empty())
						m_object_lines.clear();

					m_step=0;
					m_app->GetCommandPanel()->SetActiveRadio(0);

					m_message.LoadString(IDS_SEL_MAIN_CONT);
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

unsigned int  ExtrudeCommand::GetItemsCount()
{
	return 0;
}

void         ExtrudeCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	
}

void     ExtrudeCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	
}

HBITMAP   ExtrudeCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         ExtrudeCommand::Run(unsigned int itemID)
{
	
}
