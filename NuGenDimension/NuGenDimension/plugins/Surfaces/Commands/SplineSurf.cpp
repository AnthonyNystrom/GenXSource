#include "stdafx.h"

#include "SplineSurf.h"


#include <math.h>
#include <float.h>
#include "..//resource.h"


int     spline_surface_index = 1;

SplineSurfaceCommand::SplineSurfaceCommand(IApplicationInterface* appI):
						m_app(appI)
{
	ASSERT(m_app);
	m_step = 0;
	m_cur_obj = NULL;

	m_get_object_panel = NULL;

	m_objs.clear();
	m_coef_on_curve.clear();

	m_need_regime = NEED_OBJECT;
	m_inQuestionRegime = false;
}

SplineSurfaceCommand::~SplineSurfaceCommand()
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


bool    SplineSurfaceCommand::PreTranslateMessage(MSG* pMsg)
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
			
			if (m_get_object_panel)
					m_get_object_panel->GetWindow()->SendMessage(pMsg->message,
								pMsg->wParam,
								pMsg->lParam);

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

void     SplineSurfaceCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
										   void* params) 
{
	/*if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=1);
		m_step = (unsigned int)newActiveDlg;
		for (unsigned int i=m_step+1;i<=1;i++)
		{
			m_app->GetCommandPanel()->EnableRadio(i,false);
			m_objs[i]=NULL;
		}
		SWITCH_RESOURCE
			
		m_message.LoadString(IDS_COON_SEL_1_CONT);
			
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
	}*/
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


void  SplineSurfaceCommand::Start()	
{
	SWITCH_RESOURCE

	m_app->GetCommandPanel()->RemoveAllDialogs();

	m_message.LoadString(IDS_TOOLTIP_FIVETH);
	
	m_app->StartCommander(m_message);

	m_message.LoadString(IDS_LS_FIR_CONT);
	m_get_object_panel = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,m_message,true));
	
	if (m_get_object_panel)
	{
		m_get_object_panel->SetMultiselectMode(true);
		m_get_object_panel->FillList(isObjAddToList);
	}
	
	m_step=0;
	m_app->GetCommandPanel()->SetActiveRadio(0);
	m_app->GetCommandPanel()->EnableRadio(0,false);

	m_message.LoadString(IDS_LS_SEL_FIR_CONT);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

}

void   SplineSurfaceCommand::NeedObject(int pX, int pY)
{
	int snapSz = m_app->GetViewPort()->GetSnapSize();

	sgCObject* ho = m_app->GetViewPort()->GetTopObject(
		m_app->GetViewPort()->GetHitsInRect(CRect(pX-snapSz, pY-snapSz,
		pX+snapSz, pY+snapSz)));
	if (ho && isObjAddToList(ho))
	{
		m_app->GetViewPort()->SetHotObject(ho);
		if (m_get_object_panel)
			m_get_object_panel->SelectObject(ho,true);
	}
	else
	{
		m_app->GetViewPort()->SetHotObject(NULL);
		if (m_get_object_panel)
			m_get_object_panel->SelectObject(NULL,true);
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

void  SplineSurfaceCommand::NeedOneOfEndPoint(int pX, int pY)
{
	SG_POINT begP,endP;
	begP = m_points_on_curve[m_points_on_curve.size()-1][0];
	endP = m_points_on_curve[m_points_on_curve.size()-1][m_points_on_curve[m_points_on_curve.size()-1].size()-1];

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
		m_inverse_curve[m_inverse_curve.size()-1] = false;
	else
		m_inverse_curve[m_inverse_curve.size()-1] = true;

	m_app->GetViewPort()->InvalidateViewPort();
}

void  SplineSurfaceCommand::NeedPointOnCurve(int pX, int pY)
{
	double dis = FLT_MAX;
	size_t pInd = 0;
	size_t sz = m_temp_buffer.size();
	SG_POINT P_proj;
	double d2;
	m_inverse_curve[m_inverse_curve.size()-1] = false;
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
			m_coef_on_curve[m_coef_on_curve.size()-1] = m_temp_buffer[i].coef;
		}
	}

	m_points_on_curve[m_points_on_curve.size()-1].clear();

	m_points_on_curve[m_points_on_curve.size()-1].reserve(COUNT_OF_POINTS_ON_CURVE_2+1);
	for (double i=0.0;i<=1.0;i+=1.0/COUNT_OF_POINTS_ON_CURVE_2)
	{
		double istCoef = m_coef_on_curve[m_coef_on_curve.size()-1]+i;
		if (istCoef>1.0)
			istCoef-=1.0;
		m_points_on_curve[m_points_on_curve.size()-1].push_back(m_objs[m_objs.size()-1]->GetPointFromCoefficient(istCoef));
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

void  SplineSurfaceCommand::NeedDirection(int pX, int pY)
{
	SG_POINT begP,endP;
	begP = m_points_on_curve[m_points_on_curve.size()-1][1];
	endP = m_points_on_curve[m_points_on_curve.size()-1][m_points_on_curve[m_points_on_curve.size()-1].size()-1];

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
		m_inverse_curve[m_inverse_curve.size()-1] = false;
	else
		m_inverse_curve[m_inverse_curve.size()-1] = true;

	m_app->GetViewPort()->InvalidateViewPort();
}


void  SplineSurfaceCommand::MouseMove(unsigned int nFlags,int pX,int pY)
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
}

void  SplineSurfaceCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE

	switch(m_need_regime) 
	{
		case NEED_OBJECT:
			m_cur_obj=m_app->GetViewPort()->GetHotObject();
			if (m_cur_obj)
			{
				m_cur_obj->Select(true);
				sgC2DObject* ooo = reinterpret_cast<sgC2DObject*>(m_cur_obj);
				m_objs.push_back(ooo);
				
					std::vector<SG_POINT>  pnts;
					pnts.reserve(COUNT_OF_POINTS_ON_CURVE_2+1);
					for (double i=0.0;i<=1.0;i+=1.0/COUNT_OF_POINTS_ON_CURVE_2)
						pnts.push_back(ooo->GetPointFromCoefficient(i));
					m_points_on_curve.push_back(pnts);

					m_inverse_curve.push_back(false);
					m_coef_on_curve.push_back(0.0);

					if (m_objs.size()==1)
					{
						m_message.LoadString(IDS_SELECT_NEXT_CONTOUR);
						m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
						return;
					}

				if (!ooo->IsClosed())
				{
					m_need_regime = NEED_ONE_OF_END_POINT;
					m_message.LoadString(IDS_SEL_CURVE_DIR);
					m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
					m_app->GetViewPort()->InvalidateViewPort();
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
						tmpS.pnt = ooo->GetPointFromCoefficient(i);
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
			if (m_inverse_curve[m_inverse_curve.size()-1])
					m_objs[m_objs.size()-1]->ChangeOrient();
				m_need_regime=NEED_OBJECT;
				m_message.LoadString(IDS_SELECT_NEXT_CONTOUR);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetViewPort()->InvalidateViewPort();
				return;
		case NEED_POINT_ON_CURVE:
			m_need_regime = NEED_DIRECTION;
			m_message.LoadString(IDS_SEL_CURVE_DIR);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			m_temp_buffer.clear();
			m_app->GetViewPort()->InvalidateViewPort();
			return;
		case NEED_DIRECTION:
			{
				if (m_inverse_curve[m_inverse_curve.size()-1])
					m_objs[m_objs.size()-1]->ChangeOrient();

				m_need_regime=NEED_OBJECT;
				m_message.LoadString(IDS_SELECT_NEXT_CONTOUR);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetViewPort()->InvalidateViewPort();
				return;
			}
			break;
	}
}


void  SplineSurfaceCommand::Draw()
{
	size_t curves_cnt = m_points_on_curve.size();
	if (curves_cnt==0)
		return;

	
		float pC[3];
		m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		size_t poF = m_points_on_curve[0].size();
		for (size_t i =0;i<poF;i++)
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_points_on_curve[0][i]);
		if (m_points_on_curve.size()==1)
			return;
	
		for (size_t i=1;i<curves_cnt;i++)
		{
			m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			for (size_t j =0;j<poF;j++)
			{
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_points_on_curve[i-1][j]);
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_points_on_curve[i][j]);
			}
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			for (size_t j =0;j<poF;j++)
			{
				SG_LINE ln;
				if (m_inverse_curve[i-1])
					ln.p1 = m_points_on_curve[i-1][poF-j-1];
				else
					ln.p1 = m_points_on_curve[i-1][j];

				if (m_inverse_curve[i])
					ln.p2 = m_points_on_curve[i][poF-j-1];
				else
					ln.p2 = m_points_on_curve[i][j];
				m_app->GetViewPort()->GetPainter()->DrawLine(ln);
			}
		}
}


void  SplineSurfaceCommand::OnEnter()
{
	/*SWITCH_RESOURCE
	m_cur_obj=m_app->GetViewPort()->GetHotObject();*/
}

unsigned int  SplineSurfaceCommand::GetItemsCount()
{
	return 1;
}

void         SplineSurfaceCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	SWITCH_RESOURCE
		if (itemID==0) 
		{
			itSrt.LoadString(IDS_END_COM);
		}
		else
		{
			ASSERT(0);
		}
}

void     SplineSurfaceCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	if (m_objs.size()>=3)
		enbl = true;	
	else
		enbl = false;
	checked = false;
}

HBITMAP   SplineSurfaceCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         SplineSurfaceCommand::Run(unsigned int itemID)
{
	SWITCH_RESOURCE
		if (itemID==0)
		{
			bool  cl = false;

			if (m_objs[0]->IsPlane(NULL,NULL) && m_objs[0]->IsClosed() &&
				m_objs[m_objs.size()-1]->IsPlane(NULL,NULL) && m_objs[m_objs.size()-1]->IsClosed())
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

			sgCObject* aaa = sgSurfaces::SplineSurfaceFromSections((const sgC2DObject**)(&m_objs[0]),
				(const double*)(&m_coef_on_curve[0]),m_objs.size(),cl);

			if (!aaa)
			{
				m_message.LoadString(IDS_ERR_CANT_CREATE);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}

			CString nm;
			nm.LoadString(IDS_SPLINE_NAME);
			CString nmInd;
			nmInd.Format("%i",spline_surface_index);
			nm+=nmInd;
			aaa->SetName(nm);

			aaa->SetUserGeometry("{8809183F-18CC-49ee-8B5E-9570D3AF3A6A}",0,NULL);

			sgGetScene()->StartUndoGroup();
			sgGetScene()->AttachObject(aaa);
			m_app->ApplyAttributes(aaa);
			sgGetScene()->EndUndoGroup();
			spline_surface_index++;

			if (sgGetScene()->GetSelectedObjectsList()->GetCount()>0)
			{
				sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
				while (curObj) 
				{
					curObj->Select(false);
					curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
				}
			}

				for (size_t i=0;i<m_points_on_curve.size();i++)
				  m_points_on_curve[i].clear();
				m_points_on_curve.clear();

				m_temp_buffer.clear();

				m_inverse_curve.clear();
				m_coef_on_curve.clear();

				m_step=0;
				m_app->GetCommandPanel()->SetActiveRadio(0);
				m_app->GetCommandPanel()->EnableRadio(0,false);

				m_message.LoadString(IDS_LS_SEL_FIR_CONT);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

		}
		else
		{
			ASSERT(0);
		}
}
