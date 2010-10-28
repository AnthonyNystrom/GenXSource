#include "stdafx.h"
#include "..//NuGenDimension.h"
#include "GroupCommand.h"

int   group_name_index =1;

GroupCommand::GroupCommand(IApplicationInterface* appI):
m_app(appI)
,m_get_objects_panel(NULL)
{
	ASSERT(m_app);
}

GroupCommand::~GroupCommand()
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


bool    GroupCommand::PreTranslateMessage(MSG* pMsg)
{
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
	return false;
}


void   GroupCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, void* params)
{
	if (mes==ICommander::CM_SELECT_OBJECT)
	{
		ASSERT(params!=NULL);
		sgCObject* so = (sgCObject*)params;
		if (so->IsSelect())
		{
			so->Select(false);
		}
		else
		{
			so->Select(true);
		}
		m_app->GetViewPort()->InvalidateViewPort();
	}
}

static   bool isObjAddToList(sgCObject*)
{
	return true;
}

void  GroupCommand::Start()	
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	
	APP_SWITCH_RESOURCE

	m_message.LoadString(IDS_GROUP_COMMAND);
	m_app->StartCommander(m_message);

	m_message.LoadString(IDS_SELECT_OBJS_FOR_GROUP);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
		m_message);

	CString lab;
	lab.LoadString(IDS_OBJCTS);
	m_get_objects_panel = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,lab,true));
	
	m_get_objects_panel->FillList(isObjAddToList);

	m_app->GetCommandPanel()->SetActiveRadio(0);
}

void  GroupCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (!(nFlags & MK_LBUTTON))
		{
			int snapSz = m_app->GetViewPort()->GetSnapSize();

			m_app->GetViewPort()->SetHotObject(m_app->GetViewPort()->GetTopObject(
				m_app->GetViewPort()->GetHitsInRect(CRect(pX-snapSz, pY-snapSz,
				pX+snapSz, pY+snapSz))));
			m_app->GetViewPort()->InvalidateViewPort();
		}
	
}

void  GroupCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	if (m_app->GetViewPort()->GetHotObject())
		{
			if (m_app->GetViewPort()->GetHotObject()->IsSelect())
			{
				m_app->GetViewPort()->GetHotObject()->Select(false);
			}
			else
			{
				m_app->GetViewPort()->GetHotObject()->Select(true);
			}
			m_get_objects_panel->SelectObject(m_app->GetViewPort()->GetHotObject(),
				m_app->GetViewPort()->GetHotObject()->IsSelect());
			m_app->GetViewPort()->InvalidateViewPort();
		}
}

void  GroupCommand::OnEnter()
{
	APP_SWITCH_RESOURCE
	int selC = sgGetScene()->GetSelectedObjectsList()->GetCount();
		if (selC==0)
		{
			m_message.LoadString(IDS_NO_SELECTED_OBJECTS);
			m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
			return;
		}

		
		sgCObject**  objArr = (sgCObject**)malloc(selC*sizeof(sgCObject*));

		sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
		int i=0;
		while (curObj) 
		{
			ASSERT(i<selC);
			objArr[i] = curObj;
			i++;
			//sgGetScene()->DetachObject(curObj);
			curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
		}
		ASSERT(i==selC);
		sgGetScene()->StartUndoGroup();
		sgCGroup* grO = sgCGroup::CreateGroup(objArr,selC);
		if (!grO)
		{
			ASSERT(0);
			return;
		}
		CString nm;
		nm.LoadString(IDS_GROUP_COMMAND);
		CString nmInd;
		nmInd.Format("%i",group_name_index);
		nm+=nmInd;
		grO->SetName(nm);

		group_name_index++;

		sgGetScene()->AttachObject(grO);
		free(objArr);

		m_get_objects_panel->RemoveAllObjects();
		m_get_objects_panel->FillList(isObjAddToList);
		sgGetScene()->EndUndoGroup();

		m_app->GetViewPort()->InvalidateViewPort();
}

unsigned int  GroupCommand::GetItemsCount()
{
	return 0;
}

void         GroupCommand::GetItem(unsigned int, CString&)
{
}

void     GroupCommand::GetItemState(unsigned int, bool&, bool&)
{
}

HBITMAP   GroupCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         GroupCommand::Run(unsigned int)
{
}

void GroupCommand::Draw()
{
	
}