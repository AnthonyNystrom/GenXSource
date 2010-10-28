#include "stdafx.h"

#include "Equidi2.h"
#include <math.h>

#include "..//resource.h"

Equidi2::Equidi2(IApplicationInterface* appI):
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

Equidi2::~Equidi2()
{
	if (m_panel)
	{
		m_commandBar->DeleteCommandPanel();
		DestroyCommandPanel();
		m_panel = NULL;
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

void  Equidi2::Start()	
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

void  Equidi2::MouseMove(unsigned int nFlags,int pX,int pY)
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

void  Equidi2::LeftClick(unsigned int nFlags,int pX,int pY)
{
	   	
}

void  Equidi2::Draw()
{
	
}

void  Equidi2::OnEnter()
{
	SWITCH_RESOURCE

	if (m_start_object==NULL)
	{
		m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,
			m_message);
	}
	
}

unsigned int  Equidi2::GetItemsCount()
{
	return 3;
}

void         Equidi2::GetItem(unsigned int itemID, CString& itSrt)
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

void     Equidi2::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
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

HBITMAP   Equidi2::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         Equidi2::Run(unsigned int itemID)
{
	switch(itemID) 
	{
	case 0:
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