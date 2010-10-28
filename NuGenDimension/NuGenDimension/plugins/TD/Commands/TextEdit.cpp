#include "stdafx.h"

#include "TextEdit.h"

#include "..//resource.h"

TextEditCommand::TextEditCommand(sgCText* edT, IApplicationInterface* appI):
						m_app(appI)
							,m_editable_text(edT)
		
{
	ASSERT(m_app);
	ASSERT(edT);
	m_text_panel = NULL;
	m_orient_panel = NULL;
	m_text_text_panel = NULL;
	m_text_style = edT->GetStyle();
	m_text = edT->GetText();
	edT->GetWorldMatrix(m_matrix);
	m_fnt = edT->GetFont();
	m_scenario = -1;
}

TextEditCommand::~TextEditCommand()
{	
	m_app->GetCommandPanel()->RemoveAllDialogs();
	if (m_text_panel)
	{
		m_text_panel->DestroyWindow();
		delete m_text_panel;
		m_text_panel = NULL;
	}
	if (m_text_text_panel)
	{
		m_text_text_panel->DestroyWindow();
		delete m_text_text_panel;
		m_text_text_panel = NULL;
	}
	m_app->GetViewPort()->InvalidateViewPort();
}


void     TextEditCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
										   void* params) 
{

	if (mes==ICommander::CM_CHANGE_COMBO)
	{
		ASSERT(params!=NULL);
		IComboPanel*  cmbb = (reinterpret_cast<IComboPanel*>(params));
		if (cmbb!=m_orient_panel)
		{
			ASSERT(0);
			return;
		}
		switch(cmbb->GetCurString()) 
		{
		case 0:
			m_text_style.state &= ~SG_TEXT_VERTICAL;
			break;
		case 1:
			m_text_style.state |= SG_TEXT_VERTICAL;
			break;
		default:
			ASSERT(0);
		}
		m_app->GetViewPort()->InvalidateViewPort();
	}
}

bool    TextEditCommand::PreTranslateMessage(MSG* pMsg)
{
	try { //#try

		/*if (pMsg->message==WM_KEYUP||
		pMsg->message==WM_CHAR)
		return false;*/

		if (pMsg->message==WM_KEYUP||pMsg->message==WM_KEYDOWN || 
			pMsg->message==WM_CHAR)
		{
			if (pMsg->wParam==VK_ESCAPE)
			{
				m_app->StopCommander();
				return true;
			}

			if (m_text_text_panel && m_text_text_panel->IsEditorInFocus())
						m_text_text_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
			else
			{
				if (pMsg->wParam==VK_RETURN)
				{
					OnEnter();
					return true;
				}
			}
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

void  TextEditCommand::Start()	
{
		
}
void  TextEditCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	
}

void  TextEditCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	OnEnter();
}


static IPainter*  st_painter = NULL;

static void draw_for_draw(SG_POINT* pb,SG_POINT* pe)
{
	if (st_painter)
	{
		SG_LINE ln;
		ln.p1 = *pb;
		ln.p2 = *pe;
		st_painter->DrawLine(ln);
	}
}

void  TextEditCommand::Draw()
{
	if (m_scenario==0 && m_text_text_panel)
	{
		float pC[3];
		m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		st_painter = m_app->GetViewPort()->GetPainter();
		CString aaa;
		m_text_text_panel->GetText(aaa);
		sgCText::Draw(sgFontManager::GetFont(m_fnt),m_text_style,&m_matrix,
			aaa,draw_for_draw);
	}
	if (m_scenario==1 && m_text_panel)
	{
		float pC[3];
		m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		st_painter = m_app->GetViewPort()->GetPainter();
		sgCText::Draw(sgFontManager::GetFont(m_fnt),m_text_style,&m_matrix,
			m_text,draw_for_draw);
	}
}


void  TextEditCommand::OnEnter()
{
	if (m_scenario==-1)
		return;

	if (m_text_text_panel)
		m_text_text_panel->GetText(m_text);

	sgCText* ttO = sgCText::Create(sgFontManager::GetFont(m_fnt),
										m_text_style,m_text);

	if (!ttO)
	{
		ASSERT(0);
		return;
	}

	ttO->InitTempMatrix()->SetMatrix(&m_matrix);
	ttO->ApplyTempMatrix();
	ttO->DestroyTempMatrix();

	sgGetScene()->StartUndoGroup();
	sgGetScene()->DetachObject(m_editable_text);
	sgGetScene()->AttachObject(ttO);
	sgGetScene()->EndUndoGroup();
	m_app->CopyAttributes(*ttO,*m_editable_text);
	m_app->GetViewPort()->InvalidateViewPort();
	m_app->StopCommander();
}

unsigned int  TextEditCommand::GetItemsCount()
{
	return 2;
}

void         TextEditCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	SWITCH_RESOURCE
		switch(itemID) {
	case 0:
		itSrt.LoadString(IDS_REDACTION);
		break;
	case 1:
		itSrt.LoadString(IDS_EDIT_TEXT_STYLE);
		break;
	default:
		ASSERT(0);
		}
	
}

void     TextEditCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	enbl = true;
	if (m_scenario==0 &&  itemID==0)
		checked = true;
	else
		if (m_scenario==1 &&  itemID==1)
			checked = true;
		else
			checked = false;
}

HBITMAP   TextEditCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         TextEditCommand::Run(unsigned int itemID)
{
	if (m_scenario!=-1)
	{
		m_app->GetCommandPanel()->RemoveAllDialogs();
		if (m_text_panel)
		{
			m_text_panel->DestroyWindow();
			delete m_text_panel;
			m_text_panel = NULL;
		}
		if (m_text_text_panel)
		{
			m_text_text_panel->DestroyWindow();
			delete m_text_text_panel;
			m_text_text_panel = NULL;
		}
	}

	switch(itemID) {
	case 0:
		{
			SWITCH_RESOURCE

			CString lab;
			lab.LoadString(IDS_TOOLTIP_ZERO);
			m_app->StartCommander(lab);

			m_text_text_panel = new CTextTextDlg(m_app,
				m_app->GetCommandPanel()->GetDialogsContainerWindow());
			m_text_text_panel->Create(IDD_TEXT_TEXT_DLG,m_app->GetCommandPanel()->GetDialogsContainerWindow());
			
			m_app->GetCommandPanel()->AddDialog(m_text_text_panel,lab,false);
			m_app->GetCommandPanel()->EnableRadio(0,true);
			m_app->GetViewPort()->SetEditableObject(m_editable_text);

			m_text_text_panel->EnableControls(true);

			m_text_text_panel->SetText(m_text);

			m_scenario = 0;

			m_message.LoadString(IDS_ENTER_NEW_TEXT);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
				m_message);
		}
		break;
	case 1:
		{
			SWITCH_RESOURCE

			CString lab;
			lab.LoadString(IDS_TOOLTIP_ZERO);
			m_app->StartCommander(lab);

			lab.LoadString(IDS_TEXT_ORIENT);
			m_orient_panel = 
				reinterpret_cast<IComboPanel*>(m_app->GetCommandPanel()->
				AddDialog(IBaseInterfaceOfGetDialogs::COMBO_DLG,
				lab,false));

			m_message.LoadString(IDS_TEXT_ORINT_H);
			m_orient_panel->AddString(m_message);
			m_message.LoadString(IDS_TEXT_ORINT_V);
			m_orient_panel->AddString(m_message);

			if (m_text_style.state==0)
				m_orient_panel->SetCurString(0);
			else
				m_orient_panel->SetCurString(1);

			m_text_panel = new CTextParamsDlg(&m_text_style,m_app,
				m_app->GetCommandPanel()->GetDialogsContainerWindow());
			m_text_panel->Create(IDD_TEXT_PARAMS_DLG,m_app->GetCommandPanel()->GetDialogsContainerWindow());
			//	m_text_panel->SetText(m_editable_text->GetText());
			//m_text_panel->SetCommander(this);
			lab.LoadString(IDS_TEXT_PARAMS);
			m_app->GetCommandPanel()->AddDialog(m_text_panel,lab,false);
			m_app->GetCommandPanel()->EnableRadio(0,true);
			m_app->GetCommandPanel()->EnableRadio(1,true);
			m_app->GetViewPort()->SetEditableObject(m_editable_text);

			m_orient_panel->EnableControls(true);
			m_text_panel->EnableControls(true);

			m_scenario = 1;

			m_message.LoadString(IDS_ENTER_NEW_STYLE);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
				m_message);

		}
		break;
	default:
		ASSERT(0);
		return;
	}
}
