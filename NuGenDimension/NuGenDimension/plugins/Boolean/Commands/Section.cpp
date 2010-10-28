#include "stdafx.h"

#include "Section.h"

#include "..//resource.h"


SectionCommand::SectionCommand(IApplicationInterface* appI):
						m_app(appI)
{
	ASSERT(m_app);
}

SectionCommand::~SectionCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
}


bool    SectionCommand::PreTranslateMessage(MSG* pMsg)
{
	try {  //#try
		if (pMsg->message==WM_KEYUP)
			return true;

		if (pMsg->message==WM_KEYDOWN || 
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
			/*if (m_get_point_panel)
			{
			m_get_point_panel->GetWindow()->SendMessage(WM_CHAR,
			pMsg->wParam,
			pMsg->lParam);
			return true;
			}*/
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


void  SectionCommand::Start()	
{

}

void  SectionCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	
}

void  SectionCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	
}


void  SectionCommand::Draw()
{
	
}


void  SectionCommand::OnEnter()
{
	
}

unsigned int  SectionCommand::GetItemsCount()
{
	return 0;
}

void         SectionCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	
}

void     SectionCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	
}

HBITMAP   SectionCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         SectionCommand::Run(unsigned int itemID)
{
	
}
