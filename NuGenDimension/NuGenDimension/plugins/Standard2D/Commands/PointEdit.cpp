#include "stdafx.h"

#include "PointEdit.h"

#include "..//resource.h"

CPointEditCommand::CPointEditCommand(sgCPoint* edP, IApplicationInterface* appI):
					m_editable_point(edP)
					, m_app(appI)
					, m_was_started(false)
{
	ASSERT(edP);
	ASSERT(m_app);

	SWITCH_RESOURCE
	m_bitmap = new CBitmap;
	m_bitmap->LoadBitmap(IDB_PNT_EDIT);
}

CPointEditCommand::~CPointEditCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
	if (m_bitmap)
	{
		if (m_bitmap->m_hObject)
				m_bitmap->DeleteObject();
		delete m_bitmap;
	}
}

bool    CPointEditCommand::PreTranslateMessage(MSG* pMsg)
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


void  CPointEditCommand::Start()	
{
	ASSERT(m_editable_point);

	SWITCH_RESOURCE
	m_app->GetCommandPanel()->RemoveAllDialogs();
	CString lab;
	lab.LoadString(IDS_REDACTION);
	m_app->StartCommander(lab);
	lab.LoadString(IDS_NEW_COORDINATES);
	m_get_point_panel = reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,lab,false));
	lab.LoadString(IDS_ENTER_NEW_COORDS);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);

	m_cur_pnt = *m_editable_point->GetGeometry();
	m_get_point_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
	m_app->GetCommandPanel()->SetActiveRadio(0);
	m_app->GetViewPort()->SetEditableObject(m_editable_point);
	m_was_started = true;
}

void  CPointEditCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;

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

void  CPointEditCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;

	SWITCH_RESOURCE
	ASSERT(m_editable_point);
	sgCPoint* pnt = sgCreatePoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
	
	sgGetScene()->StartUndoGroup();
	sgGetScene()->DetachObject(m_editable_point);
	sgGetScene()->AttachObject(pnt);
	sgGetScene()->EndUndoGroup();
	m_app->CopyAttributes(*pnt,*m_editable_point);
	m_app->GetViewPort()->InvalidateViewPort();
	m_app->StopCommander();
}


void  CPointEditCommand::Draw()
{
	if (!m_was_started)
		return;

	float pC[3];
	m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
}

void  CPointEditCommand::OnEnter()
{
	if (!m_was_started)
		return;

	SWITCH_RESOURCE
	ASSERT(m_get_point_panel);

	double tmpFl[3];
	m_get_point_panel->GetPoint(tmpFl[0],tmpFl[1],tmpFl[2]);
	sgCPoint* pnt = sgCreatePoint(tmpFl[0],tmpFl[1],tmpFl[2]);
	
	sgGetScene()->StartUndoGroup();
	sgGetScene()->DetachObject(m_editable_point);
	sgGetScene()->AttachObject(pnt);
	sgGetScene()->EndUndoGroup();
	m_app->CopyAttributes(*pnt,*m_editable_point);
	m_app->GetViewPort()->InvalidateViewPort();
	m_app->StopCommander();
}

unsigned int  CPointEditCommand::GetItemsCount()
{
	if (!m_was_started)
		return 1;
	else
		return 0;
}

void         CPointEditCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	if (m_was_started)
	{
		ASSERT(0);
		return;
	}

	SWITCH_RESOURCE
	switch(itemID) {
	case 0:
		itSrt.LoadString(IDS_CHANGE_COORDS);
		break;
	default:
		ASSERT(0);
		}
}

void     CPointEditCommand::GetItemState(unsigned int itemID, 
														bool& enbl, bool& checked)
{
	if (m_was_started)
	{
		ASSERT(0);
		return;
	}

	enbl = true;
	checked = false;
}

HBITMAP  CPointEditCommand::GetItemBitmap(unsigned int)
{
	/*if (!m_was_started)
		return (HBITMAP)m_bitmap;
	else*/
		return NULL;
}

void         CPointEditCommand::Run(unsigned int itemID)
{
	if (m_was_started)
	{
		ASSERT(0);
		return;
	}

	if (itemID==0)
	{
		Start();
	}
	else
	{
		ASSERT(0);
	}
	
}
