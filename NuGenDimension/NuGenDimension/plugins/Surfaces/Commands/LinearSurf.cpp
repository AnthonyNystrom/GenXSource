#include "stdafx.h"

#include "LinearSurf.h"

#include <math.h>
#include <float.h>
#include "..//resource.h"

int     linear_surface_index = 1;

LinearSurfaceCommand::LinearSurfaceCommand(IApplicationInterface* appI):
						m_app(appI)
{
	ASSERT(m_app);
	m_step = 0;

	m_cur_obj = NULL;

	m_get_object_panels[0] = m_get_object_panels[1] = NULL;

	m_objs[0] = m_objs[1] = NULL;

	m_coef_on_second = 0.0;

	m_need_regime = NEED_OBJECT;

	m_inQuestionRegime = false;
}

LinearSurfaceCommand::~LinearSurfaceCommand()
{
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


bool    LinearSurfaceCommand::PreTranslateMessage(MSG* pMsg)
{
	try { //#try
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
			if(m_step>=0 && m_step<2) 
			{
				if (m_get_object_panels[m_step])
					m_get_object_panels[m_step]->GetWindow()->SendMessage(pMsg->message,
								pMsg->wParam,
								pMsg->lParam);

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

void     LinearSurfaceCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
										   void* params) 
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=1);
		m_step = (unsigned int)newActiveDlg;

		m_cur_obj = NULL;
		for (unsigned int i=m_step+1;i<=1;i++)
		{
			m_app->GetCommandPanel()->EnableRadio(i,false);
			m_objs[i]=NULL;
		}
		SWITCH_RESOURCE
			switch(m_step) 
			{
				case 0:
					m_message.LoadString(IDS_COON_SEL_1_CONT);
					break;
				case 1:
					m_message.LoadString(IDS_COON_SEL_2_CONT);
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
		ASSERT(m_step==0 || m_step==1);
		sgCObject* so = (sgCObject*)params;
		if (so!=m_app->GetViewPort()->GetHotObject())
		{
			m_app->GetViewPort()->SetHotObject(so);
			m_cur_obj = so;
		}
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
		if (!spl->IsSelfIntersecting())
			return true;
		else
			return false;
	}
	if (ot==SG_OT_CONTOUR)
	{
		sgCContour* cntr = reinterpret_cast<sgCContour*>(obj);
		if (!cntr->IsSelfIntersecting())
			return true;
		else
			return false;
	}
	return false;
}


void  LinearSurfaceCommand::Start()	
{
	SWITCH_RESOURCE

	m_app->GetCommandPanel()->RemoveAllDialogs();

	m_message.LoadString(IDS_TOOLTIP_FOURTH);
	
	m_app->StartCommander(m_message);

	m_message.LoadString(IDS_LS_FIR_CONT);
	m_get_object_panels[0] = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,m_message,true));
	
	m_message.LoadString(IDS_LS_SEC_CONT);
	m_get_object_panels[1] = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,m_message,true));

	for (int i=0;i<2;i++)
	{
		if (m_get_object_panels[i])
		{
			m_get_object_panels[i]->SetMultiselectMode(false);
			m_get_object_panels[i]->FillList(isObjAddToList);
		}
	}
	
	m_step=0;

	m_cur_obj = NULL;
	m_app->GetCommandPanel()->SetActiveRadio(0);

	m_need_regime = NEED_OBJECT;

	m_message.LoadString(IDS_LS_SEL_FIR_CONT);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

}

void   LinearSurfaceCommand::NeedObject(int pX, int pY)
{
	int snapSz = m_app->GetViewPort()->GetSnapSize();

	sgCObject* ho = m_app->GetViewPort()->GetTopObject(
		m_app->GetViewPort()->GetHitsInRect(CRect(pX-snapSz, pY-snapSz,
		pX+snapSz, pY+snapSz)));
	if (ho && isObjAddToList(ho))
	{
		m_app->GetViewPort()->SetHotObject(ho);
		if (m_get_object_panels[m_step])
			m_get_object_panels[m_step]->SelectObject(ho,true);
	}
	else
	{
		m_app->GetViewPort()->SetHotObject(NULL);
		if (m_get_object_panels[m_step])
			m_get_object_panels[m_step]->SelectObject(NULL,true);
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

void  LinearSurfaceCommand::NeedOneOfEndPoint(int pX, int pY)
{
	SG_POINT begP,endP;
	begP = m_points_on_Second[0];
	endP = m_points_on_Second[m_points_on_Second.size()-1];

	SG_POINT begP_proj,endP_proj;
	m_app->GetViewPort()->ProjectWorldPoint(begP,begP_proj.x,begP_proj.y,begP_proj.z);
	m_app->GetViewPort()->ProjectWorldPoint(endP,endP_proj.x,endP_proj.y,endP_proj.z);

	double d1;
	d1 = sqrt((begP_proj.x-pX)*(begP_proj.x-pX)+
		(begP_proj.y-pY)*(begP_proj.y-pY));
	double d2;
	d2 = sqrt((endP_proj.x-pX)*(endP_proj.x-pX)+
		(endP_proj.y-pY)*(endP_proj.y-pY));
	
	if (d1<d2)
		m_inverse_second = false;
	else
		m_inverse_second = true;

	m_app->GetViewPort()->InvalidateViewPort();
}

void  LinearSurfaceCommand::NeedPointOnCurve(int pX, int pY)
{
	double dis = FLT_MAX;
	size_t pInd = 0;
	size_t sz = m_temp_buffer.size();
	SG_POINT P_proj;
	double d2;
	m_inverse_second = false;
	for (size_t i =0;i<sz;i++)
	{
		m_app->GetViewPort()->ProjectWorldPoint(m_temp_buffer[i].pnt,
			P_proj.x,P_proj.y,P_proj.z);
		d2 = sqrt((P_proj.x-pX)*(P_proj.x-pX)+
			(P_proj.y-pY)*(P_proj.y-pY));
		if (d2<dis)
		{
			dis=d2;
			pInd = i;
			m_coef_on_second = m_temp_buffer[i].coef;
		}
	}
	
	m_points_on_Second.clear();

	m_points_on_Second.reserve(COUNT_OF_POINTS_ON_CURVE_2+1);
	for (double i=0.0;i<=1.0;i+=1.0/COUNT_OF_POINTS_ON_CURVE_2)
	{
		double istCoef = m_coef_on_second+i;
		if (istCoef>1.0)
			istCoef-=1.0;
		m_points_on_Second.push_back(m_objs[m_step]->GetPointFromCoefficient(istCoef));
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

void  LinearSurfaceCommand::NeedDirection(int pX, int pY)
{
	SG_POINT begP,endP;
	begP = m_points_on_Second[1];
	endP = m_points_on_Second[m_points_on_Second.size()-2];

	SG_POINT begP_proj,endP_proj;
	m_app->GetViewPort()->ProjectWorldPoint(begP,begP_proj.x,begP_proj.y,begP_proj.z);
	m_app->GetViewPort()->ProjectWorldPoint(endP,endP_proj.x,endP_proj.y,endP_proj.z);

	double d1;
	d1 = sqrt((begP_proj.x-pX)*(begP_proj.x-pX)+
		(begP_proj.y-pY)*(begP_proj.y-pY));
	double d2;
	d2 = sqrt((endP_proj.x-pX)*(endP_proj.x-pX)+
		(endP_proj.y-pY)*(endP_proj.y-pY));

	if (d1<d2)
		m_inverse_second = false;
	else
		m_inverse_second = true;


	m_app->GetViewPort()->InvalidateViewPort();
}

void  LinearSurfaceCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	switch(m_need_regime) 
	{
	case NEED_OBJECT:
		NeedObject(pX,pY);
		break;
	case NEED_ONE_OF_END_POINT:
		NeedOneOfEndPoint(pX,pY);
		break;
	case NEED_POINT_ON_CURVE:
		NeedPointOnCurve(pX,pY);
		break;
	case NEED_DIRECTION:
		NeedDirection(pX,pY);
		break;
	}

/*
	if (m_sub_step==0)
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
				if (m_get_object_panels[m_step])
					m_get_object_panels[m_step]->SelectObject(ho,true);
			}
			else
			{
				m_app->GetViewPort()->SetHotObject(NULL);
				if (m_get_object_panels[m_step])
					m_get_object_panels[m_step]->SelectObject(NULL,true);
			}
			m_app->GetViewPort()->InvalidateViewPort();
		}
		return;
	}
	else
		if (m_sub_step==1)
		{
			if (m_cur_sel_object)
			{
				if (!m_cur_sel_object->IsClosed())
				{
					SG_POINT begP,endP;
					begP = m_cur_sel_object->GetPointFromCoefficient(0.0);
					endP = m_cur_sel_object->GetPointFromCoefficient(1.0);

					SG_POINT begP_proj,endP_proj;
					m_app->GetViewPort()->ProjectWorldPoint(begP,begP_proj.x,begP_proj.y,begP_proj.z);
					m_app->GetViewPort()->ProjectWorldPoint(endP,endP_proj.x,endP_proj.y,endP_proj.z);
					
					double d1;
					d1 = sqrt((begP_proj.x-pX)*(begP_proj.x-pX)+
						(begP_proj.y-pY)*(begP_proj.y-pY));
					double d2;
					d2 = sqrt((endP_proj.x-pX)*(endP_proj.x-pX)+
						(endP_proj.y-pY)*(endP_proj.y-pY));
					if (m_step==0)
					{
						m_points_on_First.clear();
						if (d1<d2)
							m_points_on_First.push_back(begP);
						else
							m_points_on_First.push_back(endP);
					}
					else
					{
						m_points_on_Second.clear();
						if (d1<d2)
						{
							m_points_on_Second.reserve(COUNT_OF_POINTS_ON_CURVE_2+1);
							for (double i=0.0;i<=1.0;i+=1.0/COUNT_OF_POINTS_ON_CURVE_2)
								m_points_on_Second.push_back(m_cur_sel_object->GetPointFromCoefficient(i));
							m_inverse_second = false;
						}
						else
						{
							m_points_on_Second.reserve(COUNT_OF_POINTS_ON_CURVE_2+1);
							for (double i=1.0;i>=0.0;i-=1.0/COUNT_OF_POINTS_ON_CURVE_2)
								m_points_on_Second.push_back(m_cur_sel_object->GetPointFromCoefficient(i));

							m_inverse_second = true;
						}
					}
				}
				else
				{
					double dis = FLT_MAX;
					size_t pInd = 0;
					size_t sz = m_temp_buffer.size();
					SG_POINT P_proj;
					double d2;
					for (size_t i =0;i<sz;i++)
					{
						m_app->GetViewPort()->ProjectWorldPoint(m_temp_buffer[i].pnt,
								P_proj.x,P_proj.y,P_proj.z);
						d2 = sqrt((P_proj.x-pX)*(P_proj.x-pX)+
							(P_proj.y-pY)*(P_proj.y-pY));
						if (d2<dis)
						{
							dis=d2;
							pInd = i;
						}
					}
					if (m_step==0)
					{
						m_points_on_First.clear();
						m_points_on_First.push_back(m_temp_buffer[pInd].pnt);
					}
					else
					{
						m_points_on_Second.clear();
						m_points_on_Second.push_back(m_temp_buffer[pInd].pnt);
					}

				}
				m_app->GetViewPort()->InvalidateViewPort();
			}
			else
				ASSERT(0);
		}*/
}

void  LinearSurfaceCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE

	switch(m_need_regime) 
	{
		case NEED_OBJECT:
			m_cur_obj=m_app->GetViewPort()->GetHotObject();
			if (m_cur_obj)
			{
				m_cur_obj->Select(true);
				m_objs[m_step] = reinterpret_cast<sgC2DObject*>(m_cur_obj);
				if (m_step==0)
				{
					m_points_on_First.clear();
					m_points_on_Second.clear();
					m_points_on_First.reserve(COUNT_OF_POINTS_ON_CURVE_2+1);
					for (double i=0.0;i<=1.0;i+=1.0/COUNT_OF_POINTS_ON_CURVE_2)
						m_points_on_First.push_back(m_objs[m_step]->GetPointFromCoefficient(i));
					m_step++;
					m_app->GetCommandPanel()->SetActiveRadio(m_step);
					m_message.LoadString(IDS_LS_SEL_SEC_CONT);
					m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
					m_app->GetViewPort()->InvalidateViewPort();
					return;
				}

				m_points_on_Second.clear();

				if (!m_objs[m_step]->IsClosed())
				{
					m_need_regime = NEED_ONE_OF_END_POINT;

					m_points_on_Second.reserve(COUNT_OF_POINTS_ON_CURVE_2+1);
					for (double i=0.0;i<=1.0;i+=1.0/COUNT_OF_POINTS_ON_CURVE_2)
						m_points_on_Second.push_back(m_objs[m_step]->GetPointFromCoefficient(i));
					m_message.LoadString(IDS_SEL_CURVE_DIR);
					m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
					return;
				}
				else
				{
					m_temp_buffer.clear();
					m_temp_buffer.reserve(COUNT_OF_POINTS_ON_CURVE_1+1);
					for (double i=0.0;i<=1.0;i+=1.0/COUNT_OF_POINTS_ON_CURVE_1)
					{
						POINT_AND_HER_COEF tmpS;
						tmpS.coef = i;
						tmpS.pnt = m_objs[m_step]->GetPointFromCoefficient(i);
						m_temp_buffer.push_back(tmpS);
					}
					m_need_regime = NEED_POINT_ON_CURVE;
					m_message.LoadString(IDS_SEL_HREBET_POINT);
					m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
					return;
				}
			}
			else
			{
				m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}
			break;
		case NEED_ONE_OF_END_POINT:
			{
				if (m_inverse_second)
					m_objs[1]->ChangeOrient();
				sgCObject* aaa = sgSurfaces::LinearSurfaceFromSections((const sgC2DObject&)(*m_objs[0]),
					(const sgC2DObject&)(*m_objs[1]),0.0,false);
				if (!aaa)
				{
					m_message.LoadString(IDS_ERR_CANT_CREATE);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
					return;
				}

				CString nm;
				nm.LoadString(IDS_LINEAR_NAME);
				CString nmInd;
				nmInd.Format("%i",linear_surface_index);
				nm+=nmInd;
				aaa->SetName(nm);

				aaa->SetUserGeometry("{3C633C06-8A86-468f-B54F-EBE54D763665}",0,NULL);

				sgGetScene()->StartUndoGroup();
				sgGetScene()->AttachObject(aaa);
				m_app->ApplyAttributes(aaa);
				sgGetScene()->EndUndoGroup();
				linear_surface_index++;

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
				m_step=0;
				m_need_regime=NEED_OBJECT;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_LS_SEL_FIR_CONT);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetViewPort()->InvalidateViewPort();
				return;
			}
		case NEED_POINT_ON_CURVE:
			m_need_regime = NEED_DIRECTION;
			m_message.LoadString(IDS_SEL_CURVE_DIR);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			m_temp_buffer.clear();
			return;
		case NEED_DIRECTION:
			{
				if (m_inverse_second)
					m_objs[1]->ChangeOrient();

				bool  cl = false;

				if (m_objs[0]->IsPlane(NULL,NULL) && m_objs[0]->IsClosed() &&
					m_objs[1]->IsPlane(NULL,NULL) && m_objs[1]->IsClosed())
				{
					m_message.LoadString(IDS_IS_CLOSE);
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

				sgCObject* aaa = sgSurfaces::LinearSurfaceFromSections((const sgC2DObject&)(*m_objs[1]),
					(const sgC2DObject&)(*m_objs[0]),m_coef_on_second,cl);
				
				if (!aaa)
				{
					m_message.LoadString(IDS_ERR_CANT_CREATE);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
					return;
				}

				CString nm;
				nm.LoadString(IDS_LINEAR_NAME);
				CString nmInd;
				nmInd.Format("%i",linear_surface_index);
				nm+=nmInd;
				aaa->SetName(nm);

				aaa->SetUserGeometry("{3C633C06-8A86-468f-B54F-EBE54D763665}",0,NULL);

				sgGetScene()->StartUndoGroup();
				sgGetScene()->AttachObject(aaa);
				m_app->ApplyAttributes(aaa);
				sgGetScene()->EndUndoGroup();
				linear_surface_index++;

				if (sgGetScene()->GetSelectedObjectsList()->GetCount()>0)
				{
					sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
					while (curObj) 
					{
						curObj->Select(false);
						curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
					}
				}

				m_step=0;
				m_need_regime=NEED_OBJECT;
				m_message.LoadString(IDS_LS_SEL_FIR_CONT);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_app->GetViewPort()->InvalidateViewPort();
				return;
			}
			break;
	}

	/*if (m_sub_step==0)
	{
		m_cur_obj=m_app->GetViewPort()->GetHotObject();
		if (m_cur_obj)
		{
			m_cur_obj->Select(true);
			m_objs[m_step] = reinterpret_cast<sgC2DObject*>(m_cur_obj);
			if (m_cur_sel_object->IsClosed())
			{
				m_sub_step++;
				m_temp_buffer.clear();
				m_temp_buffer.reserve(COUNT_OF_POINTS_ON_CURVE_1+1);
				for (double i=0.0;i<=1.0;i+=1.0/COUNT_OF_POINTS_ON_CURVE_1)
				{
					POINT_AND_HER_COEF tmpS;
					tmpS.coef = i;
					tmpS.pnt = m_cur_sel_object->GetPointFromCoefficient(i);
					m_temp_buffer.push_back(tmpS);
				}
			}
			else
			{
				if (m_step==0)
				{
					m_sub_step = 0;
					m_cur_sel_object = NULL;
					m_points_on_First.clear();
					m_points_on_First.reserve(COUNT_OF_POINTS_ON_CURVE_2+1);
					for (double i=0.0;i<=1.0;i+=1.0/COUNT_OF_POINTS_ON_CURVE_2)
						m_points_on_First.push_back(m_objs[m_step]->GetPointFromCoefficient(i));
					m_step++;
					m_app->GetCommandPanel()->SetActiveRadio(m_step);
				}
				else
				{
					m_sub_step=1;
				}
			}
			m_app->GetViewPort()->InvalidateViewPort();
		}
		else
		{
			m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
			m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
			return;
		}
	}
	else
		if (m_sub_step==1)
		{
			if (m_step==1 && !m_objs[0]->IsClosed() && !m_objs[1]->IsClosed())
			{
				if (m_inverse_second)
					m_objs[1]->ChangeOrient();
				sgCObject* aaa = sgSurfaces::LinearSurfaceFromSections((const sgC2DObject&)(*m_objs[0]),
									(const sgC2DObject&)(*m_objs[1]),0.0,0.0);
				if (aaa)
					sgGetScene()->AttachObject(aaa);
				m_app->GetViewPort()->InvalidateViewPort();
			}
			if (m_step==1 && m_objs[1]->IsClosed())
			{

			}
		}*/
}


void  LinearSurfaceCommand::Draw()
{
	size_t poF = m_points_on_First.size();
	size_t poS = m_points_on_Second.size();
	if (poF!=0 && poS!=0)
	{
		float pC[3];
		m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		for (size_t i =0;i<poF;i++)
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_points_on_First[i]);
		for (size_t i =0;i<poS;i++)
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_points_on_Second[i]);
		m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		ASSERT(poF==poS);
		for (size_t i =0;i<poF;i++)
		{
			SG_LINE ln;
			ln.p1 = m_points_on_First[i];
			if (m_inverse_second)
				ln.p2 = m_points_on_Second[poF-i-1];
			else
				ln.p2 = m_points_on_Second[i];
			m_app->GetViewPort()->GetPainter()->DrawLine(ln);
		}
	}
	else
		if (poF!=0 && poS==0)
		{
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			for (size_t i =0;i<poF;i++)
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_points_on_First[i]);
		}
}


void  LinearSurfaceCommand::OnEnter()
{
	//SWITCH_RESOURCE
}

unsigned int  LinearSurfaceCommand::GetItemsCount()
{
	return 0;
}

void         LinearSurfaceCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	
}

void     LinearSurfaceCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	
}

HBITMAP   LinearSurfaceCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         LinearSurfaceCommand::Run(unsigned int itemID)
{
	
}
