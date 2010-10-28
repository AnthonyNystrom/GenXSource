#include "stdafx.h"

#include "LineEdit.h"

#include "..//resource.h"
#include <math.h>


LineEditCommand::LineEditCommand(sgCLine* el, IApplicationInterface* appI):
						m_editable_line(el)
						, m_app(appI)
						,m_sel_point_panel(NULL)
						, m_get_point_panel(NULL)
						, m_choise_pnt(0)
						, m_step(0)
						, m_was_started(false)
{
	ASSERT(el);
	ASSERT(m_app);

	SWITCH_RESOURCE
	m_bitmap = new CBitmap;
	m_bitmap->LoadBitmap(IDB_LINE_EDIT);
}

LineEditCommand::~LineEditCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	if (m_bitmap)
	{
		if (m_bitmap->m_hObject)
			m_bitmap->DeleteObject();
		delete m_bitmap;
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

void     LineEditCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
											   void* params) 
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=1);
		
		m_step = (unsigned int)newActiveDlg;
		if (newActiveDlg==0)
		{
			SWITCH_RESOURCE
			m_app->GetCommandPanel()->EnableRadio(1,false);
			m_message.LoadString(IDS_CHOISE_LINE_PNT);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		}
		m_app->GetViewPort()->InvalidateViewPort();
		return;
	}
	if (mes==ICommander::CM_UPDATE_COMMAND_PANEL)
	{
		ASSERT(params==NULL);
		if (m_step==0)
		{
			m_choise_pnt = m_sel_point_panel->GetCurrentPoint();
			m_app->GetViewPort()->InvalidateViewPort();
		}
	}
}



bool    LineEditCommand::PreTranslateMessage(MSG* pMsg)
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
			switch(m_step) 
			{
			case 0:
				if (m_sel_point_panel)
				{
					m_sel_point_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
				}
				break;
			case 1:
				if (m_get_point_panel)
				{
					m_get_point_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
				}
				break;
			default:
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
				case WM_LBUTTONUP:
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


void  LineEditCommand::Start()	
{
	ASSERT(m_editable_line);

	SWITCH_RESOURCE
	CString lab;
	lab.LoadString(IDS_REDACTION);
	m_app->StartCommander(lab);

	NewPanels();

	m_app->GetViewPort()->SetEditableObject(m_editable_line);
	m_was_started = true;
}

void   LineEditCommand::NewPanels()
{
	SWITCH_RESOURCE
	
	CString lab;

	lab.LoadString(IDS_TOOLTIP_ZERO);
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_sel_point_panel = 
		reinterpret_cast<ISelectPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::SELECT_POINT_DLG,
		lab,true));

	const SG_LINE* sgL = m_editable_line->GetGeometry();
	m_sel_point_panel->AddPoint(sgL->p1.x,sgL->p1.y,sgL->p1.z);
	m_sel_point_panel->AddPoint(sgL->p2.x,sgL->p2.y,sgL->p2.z);


	lab.LoadString(IDS_NEW_COORDINATES);

	m_get_point_panel = 
		reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
		lab,true));
	
	m_app->GetCommandPanel()->SetActiveRadio(0);

	lab.LoadString(IDS_CHOISE_LINE_PNT);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);

	m_step=0;
	m_choise_pnt =0;
}

void  LineEditCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;

	SWITCH_RESOURCE
	if (m_step==0)
	{
		ASSERT(m_sel_point_panel);
		ASSERT(m_editable_line);
		const SG_LINE* lineGeo = m_editable_line->GetGeometry();
		double    winX1,winY1,winZ1;
		double    winX2,winY2,winZ2;
		m_app->GetViewPort()->ProjectWorldPoint(lineGeo->p1,winX1,winY1,winZ1);
		m_app->GetViewPort()->ProjectWorldPoint(lineGeo->p2,winX2,winY2,winZ2);
		double d1;
		d1 = sqrt((winX1-pX)*(winX1-pX)+(winY1-pY)*(winY1-pY));
		double d2;
		d2 = sqrt((winX2-pX)*(winX2-pX)+(winY2-pY)*(winY2-pY));
		if (d1<d2)
		{
			if (m_sel_point_panel)
				m_sel_point_panel->SetCurrentPoint(0);
			m_choise_pnt = 0;
		}
		else
		{
			if (m_sel_point_panel)
				m_sel_point_panel->SetCurrentPoint(1);
			m_choise_pnt = 1;
		}
	}
	else
	{
			ASSERT(m_get_point_panel);
			IViewPort::GET_SNAP_IN in_arg;
			in_arg.scrX = pX;
			in_arg.scrY = pY;
			in_arg.snapType = SNAP_SYSTEM;
			in_arg.XFix = m_get_point_panel->IsXFixed();
			in_arg.YFix = m_get_point_panel->IsYFixed();
			in_arg.ZFix = m_get_point_panel->IsZFixed();
			double tmpFl[3];
			m_get_point_panel->GetPoint(tmpFl[0],tmpFl[1],tmpFl[2]);
			in_arg.FixPoint.x = tmpFl[0];
			in_arg.FixPoint.y = tmpFl[1];
			in_arg.FixPoint.z = tmpFl[2];

			IViewPort::GET_SNAP_OUT out_arg;
			m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
			m_cur_pnt = out_arg.result_point;
			m_get_point_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
	}
	m_app->GetViewPort()->InvalidateViewPort();
		
}

void  LineEditCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;

	SWITCH_RESOURCE

	if (m_step==0)
	{
		m_step++;
		m_app->GetCommandPanel()->SetActiveRadio(m_step);
		m_message.LoadString(IDS_ENTER_NEW_COORDS);
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
			m_message);

	}
	else
	{
			SG_LINE lnG;
			switch(m_choise_pnt) 
			{
			case 0:
				lnG.p1 = m_cur_pnt;
				lnG.p2 = m_editable_line->GetGeometry()->p2;
				break;
			case 1:
				lnG.p1 = m_editable_line->GetGeometry()->p1;
				lnG.p2 = m_cur_pnt;
				break;
			default:
				ASSERT(0);
			}
			CString mes;
			if (sgSpaceMath::PointsDistance(lnG.p1,lnG.p2)>0.000001)
			{

				sgCLine* ln = sgCreateLine(lnG.p1.x,lnG.p1.y,lnG.p1.z,
											lnG.p2.x,lnG.p2.y,lnG.p2.z);
				if (!ln)
					return;
				sgGetScene()->StartUndoGroup();
				sgGetScene()->DetachObject(m_editable_line);
				sgGetScene()->AttachObject(ln);
				sgGetScene()->EndUndoGroup();
				m_app->CopyAttributes(*ln,*m_editable_line);
				m_app->GetViewPort()->InvalidateViewPort();
				m_app->StopCommander();
			}
			else
			{
				mes.LoadString(IDS_ERROR_ZERO_LENGTH);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					mes);
			}
	}
			
}

void  LineEditCommand::Draw()
{
	if (!m_was_started)
		return;

		if (m_step==0)
		{
			ASSERT(m_editable_line);
			const SG_LINE* lineGeo = m_editable_line->GetGeometry();
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			if (m_choise_pnt == 0)
			{
				m_app->GetViewPort()->GetPainter()->DrawPoint(lineGeo->p1);
			}
			else
			{
				m_app->GetViewPort()->GetPainter()->DrawPoint(lineGeo->p2);
			}
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawLine(*lineGeo);
		}
		else
		{
			ASSERT(m_editable_line);
			const SG_LINE* lineGeo = m_editable_line->GetGeometry();
			float pC[3];
			m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
			m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
			m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
			SG_LINE ln;
			if (m_choise_pnt == 0)
			{
				ln.p1 = m_cur_pnt;
				ln.p2 = lineGeo->p2;
			}
			else
			{
				ln.p2 = m_cur_pnt;
				ln.p1 = lineGeo->p1;
			}
			m_app->GetViewPort()->GetPainter()->DrawLine(ln);
		}
}

void  LineEditCommand::OnEnter()
{
	if (!m_was_started)
		return;

	SWITCH_RESOURCE

	if (m_step==0)
	{
		m_step++;
		m_app->GetCommandPanel()->SetActiveRadio(m_step);
		m_message.LoadString(IDS_ENTER_NEW_COORDS);
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
			m_message);
	}
	else
	{
				ASSERT(m_get_point_panel);

				m_get_point_panel->GetPoint(m_cur_pnt.x,m_cur_pnt.y,	m_cur_pnt.z);

				SG_LINE lnG;
				switch(m_choise_pnt) 
				{
				case 0:
					lnG.p1 = m_cur_pnt;
					lnG.p2 = m_editable_line->GetGeometry()->p2;
					break;
				case 1:
					lnG.p1 = m_editable_line->GetGeometry()->p1;
					lnG.p2 = m_cur_pnt;
					break;
				default:
					ASSERT(0);
				}

				CString mes;
				if (sgSpaceMath::PointsDistance(lnG.p1,lnG.p2)>0.000001)
				{
					sgCLine* ln = sgCreateLine(lnG.p1.x,lnG.p1.y,lnG.p1.z,
						lnG.p2.x,lnG.p2.y,lnG.p2.z);
					if (!ln)
						return;
					sgGetScene()->StartUndoGroup();
					sgGetScene()->DetachObject(m_editable_line);
					sgGetScene()->AttachObject(ln);
					sgGetScene()->EndUndoGroup();

					m_app->CopyAttributes(*ln,*m_editable_line);
					m_app->GetViewPort()->InvalidateViewPort();
					m_app->StopCommander();
				}
				else
				{
					mes.LoadString(IDS_ERROR_ZERO_LENGTH);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						mes);
				}
	}
}


unsigned int  LineEditCommand::GetItemsCount()
{
	if (!m_was_started)
		return 1;
	else
		return 0;
}

void         LineEditCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	if (m_was_started)
	{
		ASSERT(0);
		return;
	}

	SWITCH_RESOURCE
	if(itemID==0) 
	{
		itSrt.LoadString(IDS_CHANGE_COORDS);
	}
	else
		ASSERT(0);
}


void     LineEditCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	if (m_was_started)
	{
		ASSERT(0);
		return;
	}

	enbl = true;
	checked = false;
	
}

HBITMAP   LineEditCommand::GetItemBitmap(unsigned int)
{
	/*if (!m_was_started)
		return (HBITMAP)m_bitmap;
	else*/
		return NULL;
}

void         LineEditCommand::Run(unsigned int itemID)
{
	if (!m_was_started)
	{
		if (itemID==0 || itemID==1)
		{
			//m_scenario = itemID;
			Start();
		}
		else
		{
			ASSERT(0);
		}
	}
	/*else
		m_scenario = itemID;*/
}

