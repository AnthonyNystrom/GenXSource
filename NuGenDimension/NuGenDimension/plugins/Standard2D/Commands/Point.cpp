#include "stdafx.h"

#include "Point.h"

#include "..//resource.h"

int     point_name_index = 1;

PointCommand::PointCommand(IApplicationInterface* appI):
				m_app(appI)
				, m_get_point_panel(NULL)
{
	ASSERT(m_app);
}

PointCommand::~PointCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
}

bool    PointCommand::PreTranslateMessage(MSG* pMsg)
{
	try {
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
			if (m_get_point_panel)
			{
				m_get_point_panel->GetWindow()->SendMessage(pMsg->message,
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

void  PointCommand::Start()	
{
	SWITCH_RESOURCE
	CString nm;
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_message.LoadString(IDS_TOOLTIP_ZERO);
	m_app->StartCommander(m_message);
	m_get_point_panel = reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,m_message,false));
	m_app->GetCommandPanel()->SetActiveRadio(0);
	m_message.LoadString(IDS_ENTER_POINT_COORDS);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

}

void  PointCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	ASSERT(m_get_point_panel);

	IViewPort::GET_SNAP_IN in_arg;
	in_arg.scrX = pX;
	in_arg.scrY = pY;
	in_arg.snapType = SNAP_SYSTEM;
	in_arg.XFix = m_get_point_panel->IsXFixed();
	in_arg.YFix = m_get_point_panel->IsYFixed();
	in_arg.ZFix = m_get_point_panel->IsZFixed();
	m_get_point_panel->GetPoint(in_arg.FixPoint.x,in_arg.FixPoint.y,in_arg.FixPoint.z);
	IViewPort::GET_SNAP_OUT out_arg;
	m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
	m_cur_pnt = out_arg.result_point;
	m_get_point_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
	m_app->GetViewPort()->InvalidateViewPort();
}

void  PointCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	sgCPoint* pnt = sgCreatePoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
	CString nm;
	nm.LoadString(IDS_TOOLTIP_ZERO);
	CString nmInd;
	nmInd.Format("%i",point_name_index);
	nm+=nmInd;
	pnt->SetName(nm.GetBuffer());
	sgGetScene()->StartUndoGroup();
	sgGetScene()->AttachObject(pnt);
	m_app->ApplyAttributes(pnt);
	sgGetScene()->EndUndoGroup();
	point_name_index++;
	
	m_app->GetViewPort()->InvalidateViewPort();
}


void  PointCommand::Draw()
{
	float pC[3];
	m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
}

void  PointCommand::OnEnter()
{
	SWITCH_RESOURCE
	ASSERT(m_get_point_panel);

	double tmpFl[3];
	m_get_point_panel->GetPoint(tmpFl[0],tmpFl[1],tmpFl[2]);
	sgCPoint* pnt = sgCreatePoint(tmpFl[0],tmpFl[1],tmpFl[2]);
	CString nm;
	nm.LoadString(IDS_TOOLTIP_ZERO);
	CString nmInd;
	nmInd.Format("%i",point_name_index);
	nm+=nmInd;
	pnt->SetName(nm.GetBuffer());
	sgGetScene()->StartUndoGroup();
	sgGetScene()->AttachObject(pnt);
	m_app->ApplyAttributes(pnt);
	sgGetScene()->EndUndoGroup();
	point_name_index++;
	m_app->GetViewPort()->InvalidateViewPort();
}

unsigned int  PointCommand::GetItemsCount()
{
	return 0;
}

void         PointCommand::GetItem(unsigned int, CString&)
{
}

void     PointCommand::GetItemState(unsigned int, bool&, bool&)
{
}

HBITMAP   PointCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         PointCommand::Run(unsigned int)
{
}