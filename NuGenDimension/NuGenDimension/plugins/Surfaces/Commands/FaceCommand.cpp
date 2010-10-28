#include "stdafx.h"

#include "FaceCommand.h"

#include "..//resource.h"

#include <algorithm>

int     face_name_index = 1;

FaceCommand::FaceCommand(IApplicationInterface* appI):
						m_app(appI)
							,m_get_main_object_panel(NULL)
							,m_get_sub_objects_panel(NULL)
{
	ASSERT(m_app);
	m_step = 0;
	m_otv.clear();
}

FaceCommand::~FaceCommand()
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


bool    FaceCommand::PreTranslateMessage(MSG* pMsg)
{
	try { //#try
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

void     FaceCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
										   void* params) 
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=1);
		m_step = (unsigned int)newActiveDlg;
		for (unsigned int i=m_step+1;i<=1;i++)
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



static   bool isObjAddToList(sgCObject* obj)
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


void  FaceCommand::Start()	
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

	m_get_main_object_panel->SetMultiselectMode(false);
	m_get_main_object_panel->FillList(isObjAddToList);
	m_get_sub_objects_panel->SetMultiselectMode(true);
	m_get_sub_objects_panel->FillList(isObjAddToList);
	
	m_step=0;
	m_app->GetCommandPanel()->SetActiveRadio(0);

	m_message.LoadString(IDS_SEL_MAIN_CONT);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

}

void  FaceCommand::MouseMove(unsigned int nFlags,int pX,int pY)
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
				if (ho && isObjAddToList(ho))
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
			ASSERT(0);
}

void   FaceCommand::FillObjectLinesList(sgCObject* obj)
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

bool   FaceCommand::IsIntersect(SG_LINE* ln)
{
	ASSERT(ln);
	std::list<SG_LINE>::iterator Iter;
	SG_POINT tP;
	for ( Iter = m_object_lines.begin( ); Iter != m_object_lines.end( ); Iter++ )
		if (sgSpaceMath::IsSegmentsIntersecting((*Iter),false,*ln,true,tP))
			return true;
	return false;
}

HO_CORRECT_PATHS_RES  FaceCommand::GoodObjectForHole(const sgC2DObject* try_hole)
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

void  FaceCommand::LeftClick(unsigned int nFlags,int pX,int pY)
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
			m_get_main_object_panel->SelectObject(m_first_obj,true);
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
}


void  FaceCommand::Draw()
{
}


void  FaceCommand::OnEnter()
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
				m_get_main_object_panel->SelectObject(m_first_obj,true);
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
				m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}
		}
		else
			if (m_step==1)
			{
				sgCObject* ooo = NULL;
				if (m_otv.size()>0)
					ooo = sgSurfaces::Face((const sgC2DObject&)(*m_first_obj),
										(const sgC2DObject**)(&m_otv[0]),m_otv.size());
				else
					ooo = sgSurfaces::Face((const sgC2DObject&)(*m_first_obj),NULL,0);

				if (!ooo)
				{
					m_message.LoadString(IDS_ERR_CANT_CREATE);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
					return;
				}

				CString nm;
				nm.LoadString(IDS_FACE_NAME);
				CString nmInd;
				nmInd.Format("%i",face_name_index);
				nm+=nmInd;
				ooo->SetName(nm);

				ooo->SetUserGeometry("{F3CB6AB8-ED48-4e10-BE2F-7FF8B1BBE8A3}",0,NULL);

				sgGetScene()->StartUndoGroup();
				sgGetScene()->AttachObject(ooo);
				m_app->ApplyAttributes(ooo);
				sgGetScene()->EndUndoGroup();
				face_name_index++;

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

unsigned int  FaceCommand::GetItemsCount()
{
	return 0;
}

void         FaceCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	
}

void     FaceCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	
}

HBITMAP   FaceCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         FaceCommand::Run(unsigned int itemID)
{
	
}
