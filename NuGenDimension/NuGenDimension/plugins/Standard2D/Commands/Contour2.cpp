#include "stdafx.h"

#include "Contour2.h"
#include <math.h>

#include "..//resource.h"

Contour2::Contour2(IApplicationInterface* appI):
				m_app(appI)
				, m_start_object(NULL)
				, m_panel(NULL)
				, m_line_regime(true)
				, m_isFirstPoint(true)
				, m_isSecondPoint(false)
				, m_isLastPointOnArc(false)
				, m_exist_arc_data(false)
				, m_can_close(false)
{
	ASSERT(m_app);
}

Contour2::~Contour2()
{
	if (m_panel)
	{
		m_commandBar->DeleteCommandPanel();
		DestroyCommandPanel();
		m_panel = NULL;
	}
	size_t sz =m_objects.size();
	for (size_t i=0;i<sz;i++)
		sgCObject::DeleteObject(m_objects[i]);
	m_objects.clear();
	m_app->GetViewPort()->InvalidateViewPort();
}

void  Contour2::Start()	
{
	SWITCH_RESOURCE
	m_panel = CreateCommandPanel(m_commandBar->GetCommandBarWindow());
	m_panel->SetKeeper(this);
	m_commandBar->SetCommandPanel(m_panel->GetPanelWindow());
	m_panel->RemoveAllPages();
	CString lab;
	lab.LoadString(IDS_TOOLTIP_ZERO);
	m_panel->AddPage(lab,IBasePanel::GET_POINT_DLG);
	m_panel->EnablePage(0,true);
}

void  Contour2::CreateContour()
{
	size_t sz = m_objects.size();
	if (sz==0)
		return;
	if (sz>1)
	{
		sgGetScene()->StartUndoGroup();
		sgGetScene()->AttachObject(sgCContour::CreateContour(&m_objects[0],sz));
		sgGetScene()->EndUndoGroup();
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,"GOOOOOOD");
		m_objects.clear();
		m_isFirstPoint = true;
		m_isSecondPoint = false;
		m_isLastPointOnArc = false;
		m_exist_arc_data = false;
		m_can_close =false;
	}
	else
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,"Only One Object");

}

void  Contour2::MouseMove(unsigned int nFlags,int pX,int pY)
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
	m_cur_point = out_arg.result_point;
	gpd->SetPoint(m_cur_point.x,m_cur_point.y,m_cur_point.z);
	if (!m_line_regime && m_isLastPointOnArc)
	{
		m_exist_arc_data = sgCArc::CreateArcGeoFrom_b_e_m(m_tmp_first_point,m_tmp_second_point,
							m_cur_point,false,m_arc_geo);
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

void  Contour2::LeftClick(unsigned int nFlags,int pX,int pY)
{
	   	if (m_isFirstPoint)
		{
			m_tmp_first_point = m_cur_point;
			m_isFirstPoint = false;
			m_isSecondPoint = true;
			if (m_objects.size()==0)
				m_first_point = m_cur_point;
		}
		else
			if (m_isSecondPoint)
			{
				if (m_line_regime)
				{
					sgCLine* ll = sgCreateLine(m_tmp_first_point.x,
												m_tmp_first_point.y,
												m_tmp_first_point.z,
												m_cur_point.x,
												m_cur_point.y,
												m_cur_point.z);
					m_objects.push_back(ll);
					m_tmp_first_point = m_cur_point;
					m_isFirstPoint = false;
					m_isSecondPoint = true;
				}
				else
				{
					m_tmp_second_point = m_cur_point;
					m_isFirstPoint = false;
					m_isSecondPoint = false;
					m_isLastPointOnArc = true;
				}

				int snSz = m_app->GetViewPort()->GetSnapSize();
				double  coords[3];
				m_app->GetViewPort()->ProjectWorldPoint(m_first_point,coords[0],coords[1],coords[2]);
				if (sqrt((coords[0]-pX)*(coords[0]-pX)+
					(coords[1]-pY)*(coords[1]-pY))<=snSz)
						m_can_close = true;
				if (m_line_regime && m_can_close)
				{
					m_message.LoadString(IDS_CLOSE_CONTOUR);
					if (AfxMessageBox(m_message,MB_YESNO)==IDYES)
					{
						sgCObject* lastO = m_objects[m_objects.size()-1];
						ASSERT(lastO->GetType()==sgCObject::LINE_OBJ);
						sgCLine* lastL = reinterpret_cast<sgCLine*>(lastO);
						m_tmp_first_point = lastL->GetGeometry()->p1;
						sgCObject::DeleteObject(lastO);
						m_objects.pop_back();
						sgCLine* ll = sgCreateLine(m_tmp_first_point.x,
							m_tmp_first_point.y,
							m_tmp_first_point.z,
							m_first_point.x,
							m_first_point.y,
							m_first_point.z);
						m_objects.push_back(ll);
						CreateContour();
					}
					else
						m_can_close = false;
				}
			}
			else
				if (m_isLastPointOnArc)
				{
					ASSERT(!m_line_regime);
					ASSERT(m_exist_arc_data);
					sgCArc* aa = sgCreateArc(&m_arc_geo);
					m_objects.push_back(aa);
					m_tmp_first_point = m_tmp_second_point;
					m_isFirstPoint = false;
					m_isSecondPoint = true;
					m_isLastPointOnArc = false;
					m_exist_arc_data = false;
					if (m_can_close)
					{
						m_message.LoadString(IDS_CLOSE_CONTOUR);
						if (AfxMessageBox(m_message,MB_YESNO)==IDYES)
						{
							sgCObject* lastO = m_objects[m_objects.size()-1];
							ASSERT(lastO->GetType()==sgCObject::ARC_OBJ);
							sgCArc* lastA = reinterpret_cast<sgCArc*>(lastO);
							m_tmp_first_point = lastA->GetGeometry()->begin;
							sgCObject::DeleteObject(lastO);
							m_objects.pop_back();
							sgCArc::CreateArcGeoFrom_b_e_m(m_tmp_first_point,m_first_point,
								m_cur_point,false,m_arc_geo);
							sgCArc* aa = sgCreateArc(&m_arc_geo);
							m_objects.push_back(aa);
							CreateContour();
						}
						else
							m_can_close = false;
					}
				}
				else
				{
					ASSERT(0);
				}
}

void  Contour2::Draw()
{
	float pC[3];
	m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_point);
	m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
	size_t sz = m_objects.size();
	for(size_t i=0;i<sz;i++)
	{
		if (m_objects[i]->GetType()==sgCObject::LINE_OBJ)
		{
			sgCLine* tmpl = reinterpret_cast<sgCLine*>(m_objects[i]);
			m_app->GetViewPort()->GetPainter()->DrawLine(*tmpl->GetGeometry());
			continue;
		}
		if (m_objects[i]->GetType()==sgCObject::ARC_OBJ)
		{
			sgCArc* tmpa = reinterpret_cast<sgCArc*>(m_objects[i]);
			m_app->GetViewPort()->GetPainter()->DrawArc(*tmpa->GetGeometry());
			continue;
		}
	}
	if (m_exist_arc_data)
	{
		m_app->GetViewPort()->GetPainter()->DrawArc(m_arc_geo);
	}
	if (m_isSecondPoint)
	{
		SG_LINE lll;
		lll.p1 = m_tmp_first_point;
		lll.p2 = m_cur_point;
		m_app->GetViewPort()->GetPainter()->DrawLine(lll);
	}
}

void  Contour2::OnEnter()
{
	SWITCH_RESOURCE

	if (m_start_object==NULL)
	{
		m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,
			m_message);
	}
	
}

unsigned int  Contour2::GetItemsCount()
{
	return 3;
}

void         Contour2::GetItem(unsigned int itemID, CString& itSrt)
{
	SWITCH_RESOURCE
		if (itemID==0) 
		{
			itSrt.LoadString(IDS_END_OPER);
		}
		else
			if (itemID==1) 
			{
				itSrt.LoadString(IDS_TOOLTIP_FIRST);
			}
			else
				if (itemID==2) 
				{
					itSrt.LoadString(IDS_TOOLTIP_THIRD);
				}
				else
				{
					ASSERT(0);
				}
}

void     Contour2::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	enbl = true;
	checked = false;
	switch(itemID) 
	{
	case 0:
		checked = false;
		break;
	case 1:
		if (m_line_regime)
			checked = true;
		else
			if (m_isLastPointOnArc)
				enbl=false;
		break;
	case 2:
		if (!m_line_regime)
			checked = true;
		break;
	default:
		ASSERT(0);
	}
}

HBITMAP   Contour2::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         Contour2::Run(unsigned int itemID)
{
	switch(itemID) 
	{
	case 0:
		CreateContour();
		break;
	case 1:
		m_line_regime = true;
		break;
	case 2:
		m_line_regime = false;
		break;
	default:
		ASSERT(0);
	}
}