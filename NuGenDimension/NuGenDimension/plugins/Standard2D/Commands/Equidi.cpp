#include "stdafx.h"

#include "Equidi.h"

#include "..//resource.h"

int     equidi_name_index =1;

Equidi::Equidi(IApplicationInterface* appI):
				m_app(appI)
				, m_start_object(NULL)
				, m_get_object_panel(NULL)
				, m_get_H_panel(NULL)
				, m_step(0)
{
	ASSERT(m_app);
}

Equidi::~Equidi()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
}


bool    Equidi::PreTranslateMessage(MSG* pMsg)
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
			switch(m_step) {
			case 0:
				if (m_get_object_panel)
					m_get_object_panel->GetWindow()->SendMessage(pMsg->message,
								pMsg->wParam,
								pMsg->lParam);
				break;
			case 1:
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


static   bool isObjAddToList(sgCObject* o)
{
	if (o->GetType()==SG_OT_CIRCLE ||
		o->GetType()==SG_OT_ARC)
		return true;
	if (o->GetType()==SG_OT_CONTOUR)
	{
		sgCContour* cc = reinterpret_cast<sgCContour*>(o);
		if (!cc->IsLinear() && cc->IsPlane(NULL,NULL))
			return true;
		else
			return false;
	}
	if (o->GetType()==SG_OT_SPLINE)
	{
		sgCSpline* cc = reinterpret_cast<sgCSpline*>(o);
		if (!cc->IsLinear() && cc->IsPlane(NULL,NULL))
			return true;
		else
			return false;
	}
	return false;
}

void   Equidi::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, void* params)
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=1);
		m_step = (unsigned int)newActiveDlg;
		if (newActiveDlg==0)
		{
			m_app->GetCommandPanel()->EnableRadio(1,false);
			m_message.LoadString(IDS_QEUID_GET_OBJ);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		}
		m_app->GetViewPort()->InvalidateViewPort();
		return;
	}
	if (mes==ICommander::CM_SELECT_OBJECT)
	{
		ASSERT(params!=NULL);
		sgCObject* so = (sgCObject*)params;
		if (so!=m_start_object)
		{
			m_app->GetViewPort()->SetHotObject(so);
			m_start_object = so;
		}
		m_app->GetViewPort()->InvalidateViewPort();	
	}
}

void  Equidi::Start()	
{
	SWITCH_RESOURCE

	m_app->GetCommandPanel()->RemoveAllDialogs();

	CString lab;
	lab.LoadString(IDS_TOOLTIP_SEVETH);
	m_app->StartCommander(lab);

	lab.LoadString(IDS_OBJECT);
	m_get_object_panel = 
		reinterpret_cast<IGetObjectsPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,
		lab,true));

	lab.LoadString(IDS_EQUID_H);
	m_get_H_panel = 
		reinterpret_cast<IGetNumberPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,
		lab,true));

	lab.LoadString(IDS_QEUID_GET_OBJ);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);

	m_app->GetCommandPanel()->SetActiveRadio(0);
	m_get_object_panel->SetMultiselectMode(false);
	m_get_object_panel->FillList(isObjAddToList);

	m_step=0;
}

static  void get_end_points_of_object(sgCObject* ob, SG_POINT& bP, SG_POINT& eP)
{
	if (ob->GetType()==SG_OT_ARC)
	{
		sgCArc* arcO = reinterpret_cast<sgCArc*>(ob);
		const SG_ARC* arcG = arcO->GetGeometry();
		bP = arcG->begin;
		eP = arcG->end;
	}
	else
			if (ob->GetType()==SG_OT_CONTOUR)
			{
				sgCContour* cO = reinterpret_cast<sgCContour*>(ob);
				bP = cO->GetPointFromCoefficient(0.0);
				eP = cO->GetPointFromCoefficient(1.0);
			}
			else
			{
				ASSERT(0);
			}
}

bool  get_point_on_path_after_this(sgCContour* contr, const SG_POINT ppp, SG_POINT& resP)
{
	sgCObject* fo = contr->GetChildrenList()->GetHead();
	SG_OBJECT_TYPE foT = fo->GetType();
	if (foT==SG_OT_LINE)
	{
		sgCLine* ln = reinterpret_cast<sgCLine*>(fo);
		const SG_LINE* lnG = ln->GetGeometry();
		if (sgSpaceMath::PointsDistance(lnG->p1,ppp)<0.00001)
		{
			resP = lnG->p2;
			return true;
		}
		if (sgSpaceMath::PointsDistance(lnG->p2,ppp)<0.00001)
		{
			resP = lnG->p1;
			return true;
		}
	}
	else
		if (foT==SG_OT_ARC)
		{
			sgCArc* ar = reinterpret_cast<sgCArc*>(fo);
			const int pntsCnt = ar->GetPointsCount();
			const SG_POINT* arcPnts = ar->GetPoints();
			if (sgSpaceMath::PointsDistance(*arcPnts,ppp)<0.00001)
			{
				resP = arcPnts[1];
				return true;
			}
			if (sgSpaceMath::PointsDistance(*(arcPnts+pntsCnt-1),ppp)<0.00001)
			{
				resP = arcPnts[pntsCnt-2];
				return true;
			}
		}
		else
			if (foT==SG_OT_CONTOUR)
			{
				sgCContour* co = reinterpret_cast<sgCContour*>(fo);
				if (get_point_on_path_after_this(co, ppp, resP))
					return true;
			}


	sgCObject* so = contr->GetChildrenList()->GetTail();
	SG_OBJECT_TYPE soT = so->GetType();
			if (soT==SG_OT_LINE)
			{
				sgCLine* ln = reinterpret_cast<sgCLine*>(so);
				const SG_LINE* lnG = ln->GetGeometry();
				if (sgSpaceMath::PointsDistance(lnG->p1,ppp)<0.00001)
				{
					resP = lnG->p2;
					return true;
				}
				if (sgSpaceMath::PointsDistance(lnG->p2,ppp)<0.00001)
				{
					resP = lnG->p1;
					return true;
				}
			}
			else
				if (soT==SG_OT_ARC)
				{
					sgCArc* ar = reinterpret_cast<sgCArc*>(so);
					const int pntsCnt = ar->GetPointsCount();
					const SG_POINT* arcPnts = ar->GetPoints();
					if (sgSpaceMath::PointsDistance(*arcPnts,ppp)<0.00001)
					{
						resP = arcPnts[1];
						return true;
					}
					if (sgSpaceMath::PointsDistance(*(arcPnts+pntsCnt-1),ppp)<0.00001)
					{
						resP = arcPnts[pntsCnt-2];
						return true;
					}
				}
				else
					if (soT==SG_OT_CONTOUR)
					{
						sgCContour* co = reinterpret_cast<sgCContour*>(so);
						if (get_point_on_path_after_this(co, ppp, resP))
							return true;
					}
	return false;
}

bool  Equidi::CheckStartObject()
{
	SG_OBJECT_TYPE oT = m_start_object->GetType();
	if (oT==SG_OT_ARC)
	{
		sgCArc* ar = reinterpret_cast<sgCArc*>(m_start_object);
		/*const SG_ARC* arcG = ar->GetGeometry();
		m_end_points[0] = arcG->begin;
		m_end_points[1] = arcG->end;
		m_end_vectors[0].x = arcG->center.x-arcG->begin.x;
		m_end_vectors[0].y = arcG->center.y-arcG->begin.y;
		m_end_vectors[0].z = arcG->center.z-arcG->begin.z;
		sgSpaceMath::NormalVector(&m_end_vectors[0]);
		m_end_vectors[1].x = arcG->center.x-arcG->end.x;
		m_end_vectors[1].y = arcG->center.y-arcG->end.y;
		m_end_vectors[1].z = arcG->center.z-arcG->end.z;
		sgSpaceMath::NormalVector(&m_end_vectors[1]);
		return true;*/

		const SG_POINT*  pnts = ar->GetPoints();
		m_end_points[0] = pnts[0];
		m_end_points[1] = pnts[ar->GetPointsCount()-1];
		SG_VECTOR plN = ar->GetGeometry()->normal;
		plN.x = -plN.x; plN.y = -plN.y; plN.z = -plN.z;
		SG_VECTOR tmpV;
		tmpV.x = pnts[1].x - pnts[0].x;
		tmpV.y = pnts[1].y - pnts[0].y;
		tmpV.z = pnts[1].z - pnts[0].z;
		m_end_vectors[0] = sgSpaceMath::VectorsVectorMult(plN,tmpV);
		sgSpaceMath::NormalVector(m_end_vectors[0]);
		tmpV.x = pnts[ar->GetPointsCount()-1].x - pnts[ar->GetPointsCount()-2].x;
		tmpV.y = pnts[ar->GetPointsCount()-1].y - pnts[ar->GetPointsCount()-2].y;
		tmpV.z = pnts[ar->GetPointsCount()-1].z - pnts[ar->GetPointsCount()-2].z;
		m_end_vectors[1] = sgSpaceMath::VectorsVectorMult(plN,tmpV);
		sgSpaceMath::NormalVector(m_end_vectors[1]);
		return true;
	}
	if (oT==SG_OT_CIRCLE)
	{
		sgCCircle* cr = reinterpret_cast<sgCCircle*>(m_start_object);
		const SG_CIRCLE* crG = cr->GetGeometry();
		const SG_POINT*  pnts = cr->GetPoints();
		m_end_points[0] = pnts[0];
		m_end_points[1] = m_end_points[0];
		m_end_vectors[0].x = crG->center.x-pnts[0].x;
		m_end_vectors[0].y = crG->center.y-pnts[0].y;
		m_end_vectors[0].z = crG->center.z-pnts[0].z;
		sgSpaceMath::NormalVector(m_end_vectors[0]);
		m_end_vectors[1] = m_end_vectors[0];
		return true;
	}
	if (oT==SG_OT_SPLINE)
	{
		sgCSpline* spl = reinterpret_cast<sgCSpline*>(m_start_object);
		const SG_SPLINE* spG = spl->GetGeometry();
		const SG_POINT*  pnts = spG->GetPoints();
		m_end_points[0] = pnts[0];
		if (!spl->IsClosed())
			m_end_points[1] = pnts[spG->GetPointsCount()-1];
		else
			m_end_points[1] = m_end_points[0];
		SG_VECTOR plN;
		double    plD;
		if (!spl->IsPlane(&plN,&plD))
			return false;
		SG_VECTOR tmpV;
		tmpV.x = pnts[1].x - pnts[0].x;
		tmpV.y = pnts[1].y - pnts[0].y;
		tmpV.z = pnts[1].z - pnts[0].z;
		m_end_vectors[0] = sgSpaceMath::VectorsVectorMult(plN,tmpV);
		sgSpaceMath::NormalVector(m_end_vectors[0]);
		if (!spl->IsClosed())
		{
			tmpV.x = pnts[spG->GetPointsCount()-1].x - pnts[spG->GetPointsCount()-2].x;
			tmpV.y = pnts[spG->GetPointsCount()-1].y - pnts[spG->GetPointsCount()-2].y;
			tmpV.z = pnts[spG->GetPointsCount()-1].z - pnts[spG->GetPointsCount()-2].z;
			m_end_vectors[1] = sgSpaceMath::VectorsVectorMult(plN,tmpV);
			sgSpaceMath::NormalVector(m_end_vectors[1]);
		}
		else
			m_end_vectors[1] = m_end_vectors[0];
		return true;
	}
	if (oT==SG_OT_CONTOUR)
	{
		sgCContour* cnt = reinterpret_cast<sgCContour*>(m_start_object);
		//cnt->GetEndPoints(m_end_points[0],m_end_points[1]);
		m_end_points[0] = cnt->GetPointFromCoefficient(0.0);
		m_end_points[1] = cnt->GetPointFromCoefficient(1.0);
		SG_POINT secP;
		SG_VECTOR plN;
		double    plD;
		if (!cnt->IsPlane(&plN,&plD))
			return false;
		if (!get_point_on_path_after_this(cnt, m_end_points[0], secP))
		{
			ASSERT(0);
			return false;
		}
		SG_VECTOR tmpV;
		tmpV.x = secP.x - m_end_points[0].x;
		tmpV.y = secP.y - m_end_points[0].y;
		tmpV.z = secP.z - m_end_points[0].z;
		m_end_vectors[0] = sgSpaceMath::VectorsVectorMult(plN,tmpV);
		sgSpaceMath::NormalVector(m_end_vectors[0]);

		if (!cnt->IsClosed())
		{
				if (!get_point_on_path_after_this(cnt, m_end_points[1], secP))
				{
					ASSERT(0);
					return false;
				}
				tmpV.x = m_end_points[1].x - secP.x;
				tmpV.y = m_end_points[1].y - secP.y;
				tmpV.z = m_end_points[1].z - secP.z;
				m_end_vectors[1] = sgSpaceMath::VectorsVectorMult(plN,tmpV);
				sgSpaceMath::NormalVector(m_end_vectors[1]);
		}
		else
		{
			m_end_points[1] = m_end_points[0];
			m_end_vectors[1] = m_end_vectors[0];
		}
		return true;
	}
	return false;
}

void  Equidi::MouseMove(unsigned int nFlags,int pX,int pY)
{
	switch(m_step) {
	case 0:
		if (!(nFlags & MK_LBUTTON))
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
		break;
	case 1:
		{
			ASSERT(m_get_H_panel);

			if (!m_app->GetViewPort()->
					ProjectScreenPointOnLine(pX,pY,m_end_points[0],m_end_vectors[0],
						m_projection))
			{
				m_otstup = 0.0;
				m_get_H_panel->SetNumber(m_otstup);
				break;
			}
			char sig = (((m_projection.x-m_end_points[0].x)*m_end_vectors[0].x+
				(m_projection.y-m_end_points[0].y)*m_end_vectors[0].y+
				(m_projection.z-m_end_points[0].z)*m_end_vectors[0].z)>0)?1:-1;

			m_otstup = sgSpaceMath::PointsDistance(m_end_points[0],m_projection)*sig;
			m_otstup = m_app->ApplyPrecision(m_otstup);
			m_projection.x = m_end_points[0].x+m_end_vectors[0].x*m_otstup;
			m_projection.y = m_end_points[0].y+m_end_vectors[0].y*m_otstup;
			m_projection.z = m_end_points[0].z+m_end_vectors[0].z*m_otstup;
			m_get_H_panel->SetNumber(m_otstup);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	default:
		ASSERT(0);
		return;
	}
}

void  Equidi::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	switch(m_step) 
	{
	case 0:
			m_start_object = m_app->GetViewPort()->GetHotObject();

			if (m_start_object==NULL)
			{
				m_message.LoadString(IDS_EQ_ERR_NO_OBJ);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}

			
			m_app->GetViewPort()->InvalidateViewPort();

			if (CheckStartObject())
			{
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_EQUID_GET_H);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			}
			break;
	case 1:
		if (m_start_object->GetType()==SG_OT_ARC)
		{
			sgCArc* ar = reinterpret_cast<sgCArc*>(m_start_object);
			sgCObject* nO = ar->GetEquidistantContour(m_otstup,m_otstup,false);
			if (!nO)
			{
				m_message.LoadString(IDS_EQ_ERR);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}
			CString nm;
			nm.LoadString(IDS_TOOLTIP_SEVETH);
			CString nmInd;
			nmInd.Format("%i",equidi_name_index);
			nm+=nmInd;
			nO->SetName(nm);
			equidi_name_index++;
			sgGetScene()->StartUndoGroup();
			sgGetScene()->AttachObject(nO);
			m_app->ApplyAttributes(nO);
			sgGetScene()->EndUndoGroup();
			m_step=0;
			m_app->GetCommandPanel()->SetActiveRadio(m_step);
			m_app->GetCommandPanel()->EnableRadio(1,false);
			if (m_get_object_panel)
				m_get_object_panel->AddObject(nO,false);
			m_message.LoadString(IDS_QEUID_GET_OBJ);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		if (m_start_object->GetType()==SG_OT_CIRCLE)
		{
			sgCCircle* cr = reinterpret_cast<sgCCircle*>(m_start_object);
			sgCObject* nO = cr->GetEquidistantContour(m_otstup,m_otstup,false);
			if (!nO)
			{
				m_message.LoadString(IDS_EQ_ERR);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}
			CString nm;
			nm.LoadString(IDS_TOOLTIP_SEVETH);
			CString nmInd;
			nmInd.Format("%i",equidi_name_index);
			nm+=nmInd;
			nO->SetName(nm);
			equidi_name_index++;
			sgGetScene()->StartUndoGroup();
			sgGetScene()->AttachObject(nO);
			m_app->ApplyAttributes(nO);
			sgGetScene()->EndUndoGroup();
			m_step=0;
			m_app->GetCommandPanel()->SetActiveRadio(m_step);
			m_app->GetCommandPanel()->EnableRadio(1,false);
			if (m_get_object_panel)
				m_get_object_panel->AddObject(nO,false);
			m_message.LoadString(IDS_QEUID_GET_OBJ);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		if (m_start_object->GetType()==SG_OT_SPLINE)
		{
			sgCSpline* spl = reinterpret_cast<sgCSpline*>(m_start_object);
			sgCObject* nO = spl->GetEquidistantContour(m_otstup,m_otstup,false);
			if (!nO)
			{
				m_message.LoadString(IDS_EQ_ERR);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}
			CString nm;
			nm.LoadString(IDS_TOOLTIP_SEVETH);
			CString nmInd;
			nmInd.Format("%i",equidi_name_index);
			nm+=nmInd;
			nO->SetName(nm);
			equidi_name_index++;
			sgGetScene()->StartUndoGroup();
			sgGetScene()->AttachObject(nO);
			m_app->ApplyAttributes(nO);
			sgGetScene()->EndUndoGroup();
			m_step=0;
			m_app->GetCommandPanel()->SetActiveRadio(m_step);
			m_app->GetCommandPanel()->EnableRadio(1,false);
			if (m_get_object_panel)
				m_get_object_panel->AddObject(nO,false);
			m_message.LoadString(IDS_QEUID_GET_OBJ);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		if (m_start_object->GetType()==SG_OT_CONTOUR)
		{
			sgCContour* cn = reinterpret_cast<sgCContour*>(m_start_object);
			sgCObject* nO = cn->GetEquidistantContour(m_otstup,m_otstup,false);
			if (!nO)
			{
				m_message.LoadString(IDS_EQ_ERR);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}
			CString nm;
			nm.LoadString(IDS_TOOLTIP_SEVETH);
			CString nmInd;
			nmInd.Format("%i",equidi_name_index);
			nm+=nmInd;
			nO->SetName(nm);
			equidi_name_index++;
			sgGetScene()->StartUndoGroup();
			sgGetScene()->AttachObject(nO);
			m_app->ApplyAttributes(nO);
			sgGetScene()->EndUndoGroup();
			m_step=0;
			m_app->GetCommandPanel()->SetActiveRadio(m_step);
			m_app->GetCommandPanel()->EnableRadio(1,false);
			if (m_get_object_panel)
				m_get_object_panel->AddObject(nO,false);
			m_message.LoadString(IDS_QEUID_GET_OBJ);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			m_app->GetViewPort()->InvalidateViewPort();
		}
		break;
	default:
		break;
	}
}

void  Equidi::Draw()
{
	if (m_step==1)
	{
		float pC[3];
		m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->DrawPoint(m_end_points[0]);
		m_app->GetViewPort()->GetPainter()->DrawPoint(m_end_points[1]);
		m_app->GetViewPort()->GetPainter()->DrawPoint(m_projection);
		SG_LINE ln;
		ln.p1 = m_end_points[0];
		ln.p2 = m_projection;
		m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->DrawLine(ln);
		ln.p1 = m_end_points[1];
		ln.p2.x = m_end_points[1].x+m_end_vectors[1].x*m_otstup;
		ln.p2.y = m_end_points[1].y+m_end_vectors[1].y*m_otstup;
		ln.p2.z = m_end_points[1].z+m_end_vectors[1].z*m_otstup;
		m_app->GetViewPort()->GetPainter()->DrawLine(ln);
	}
}

void  Equidi::OnEnter()
{
	SWITCH_RESOURCE
	switch(m_step) 
	{
		case 0:
			m_start_object = m_app->GetViewPort()->GetHotObject();

			if (m_start_object==NULL)
			{
				m_message.LoadString(IDS_EQ_ERR_NO_OBJ);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}


			m_app->GetViewPort()->InvalidateViewPort();

			if (CheckStartObject())
			{
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_EQUID_GET_H);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

			}
			break;
		case 1:
			{
			ASSERT(m_get_H_panel);

			double m_otstup = m_get_H_panel->GetNumber();
			if (m_start_object->GetType()==SG_OT_ARC)
			{
				sgCArc* ar = reinterpret_cast<sgCArc*>(m_start_object);
				sgCObject* nO = ar->GetEquidistantContour(m_otstup,m_otstup,false);
				if (!nO)
				{
					m_message.LoadString(IDS_EQ_ERR);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
					return;
				}
				CString nm;
				nm.LoadString(IDS_TOOLTIP_SEVETH);
				CString nmInd;
				nmInd.Format("%i",equidi_name_index);
				nm+=nmInd;
				nO->SetName(nm);
				equidi_name_index++;
				sgGetScene()->StartUndoGroup();
				sgGetScene()->AttachObject(nO);
				m_app->ApplyAttributes(nO);
				sgGetScene()->EndUndoGroup();
				m_step=0;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_app->GetCommandPanel()->EnableRadio(1,false);
				if (m_get_object_panel)
					m_get_object_panel->AddObject(nO,false);
				m_message.LoadString(IDS_QEUID_GET_OBJ);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetViewPort()->InvalidateViewPort();
			}
			if (m_start_object->GetType()==SG_OT_CIRCLE)
			{
				sgCCircle* cr = reinterpret_cast<sgCCircle*>(m_start_object);
				sgCObject* nO = cr->GetEquidistantContour(m_otstup,m_otstup,false);
				if (!nO)
				{
					m_message.LoadString(IDS_EQ_ERR);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
					return;
				}
				CString nm;
				nm.LoadString(IDS_TOOLTIP_SEVETH);
				CString nmInd;
				nmInd.Format("%i",equidi_name_index);
				nm+=nmInd;
				nO->SetName(nm);
				equidi_name_index++;
				sgGetScene()->StartUndoGroup();
				sgGetScene()->AttachObject(nO);
				m_app->ApplyAttributes(nO);
				sgGetScene()->EndUndoGroup();
				m_step=0;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_app->GetCommandPanel()->EnableRadio(1,false);
				if (m_get_object_panel)
					m_get_object_panel->AddObject(nO,false);
				m_message.LoadString(IDS_QEUID_GET_OBJ);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetViewPort()->InvalidateViewPort();
			}
			if (m_start_object->GetType()==SG_OT_SPLINE)
			{
				sgCSpline* spl = reinterpret_cast<sgCSpline*>(m_start_object);
				sgCObject* nO = spl->GetEquidistantContour(m_otstup,m_otstup,false);
				if (!nO)
				{
					m_message.LoadString(IDS_EQ_ERR);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
					return;
				}
				CString nm;
				nm.LoadString(IDS_TOOLTIP_SEVETH);
				CString nmInd;
				nmInd.Format("%i",equidi_name_index);
				nm+=nmInd;
				nO->SetName(nm);
				equidi_name_index++;
				sgGetScene()->StartUndoGroup();
				sgGetScene()->AttachObject(nO);
				m_app->ApplyAttributes(nO);
				sgGetScene()->EndUndoGroup();
				m_step=0;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_app->GetCommandPanel()->EnableRadio(1,false);
				if (m_get_object_panel)
					m_get_object_panel->AddObject(nO,false);
				m_message.LoadString(IDS_QEUID_GET_OBJ);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetViewPort()->InvalidateViewPort();
			}
			if (m_start_object->GetType()==SG_OT_CONTOUR)
			{
				sgCContour* cn = reinterpret_cast<sgCContour*>(m_start_object);
				sgCObject* nO = cn->GetEquidistantContour(m_otstup,m_otstup,false);
				if (!nO)
				{
					m_message.LoadString(IDS_EQ_ERR);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
					return;
				}
				CString nm;
				nm.LoadString(IDS_TOOLTIP_SEVETH);
				CString nmInd;
				nmInd.Format("%i",equidi_name_index);
				nm+=nmInd;
				nO->SetName(nm);
				equidi_name_index++;
				sgGetScene()->StartUndoGroup();
				sgGetScene()->AttachObject(nO);
				m_app->ApplyAttributes(nO);
				sgGetScene()->EndUndoGroup();
				m_step=0;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_app->GetCommandPanel()->EnableRadio(1,false);
				if (m_get_object_panel)
					m_get_object_panel->AddObject(nO,false);
				m_message.LoadString(IDS_QEUID_GET_OBJ);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetViewPort()->InvalidateViewPort();
			}
			}
			break;
		default:
			break;
	}
}

unsigned int  Equidi::GetItemsCount()
{
	return 0;
}

void         Equidi::GetItem(unsigned int, CString&)
{
}

void     Equidi::GetItemState(unsigned int, bool&, bool&)
{
}

HBITMAP   Equidi::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         Equidi::Run(unsigned int)
{
}