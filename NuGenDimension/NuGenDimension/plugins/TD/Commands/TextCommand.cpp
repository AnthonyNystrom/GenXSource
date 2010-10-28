#include "stdafx.h"

#include "TextCommand.h"

#include "..//resource.h"

#include <math.h>

int     text_name_index = 1;

TextCommand::TextCommand(IApplicationInterface* appI):
						m_app(appI)
							,m_get_point_panel(NULL)
							,m_text_text_panel(NULL)
							,m_orient_combo_panel(NULL)
							,m_text_panel(NULL)
		
{
	ASSERT(m_app);
	m_params_dlg = NULL;
	m_text_style.state = 0;
	m_text_style.height= 0.5;
	m_text_style.proportions= 100.0;
	m_text_style.angle= 0.0;
	m_text_style.horiz_space_proportion= 0.0;
	m_text_style.vert_space_proportion= 50.0;
}

TextCommand::~TextCommand()
{	
	m_app->GetCommandPanel()->RemoveAllDialogs();
	if (m_text_text_panel)
	{
		m_text_text_panel->DestroyWindow();
		delete m_text_text_panel;
		m_text_text_panel =NULL;
	}
	if (m_text_panel)
	{
		m_text_panel->DestroyWindow();
		delete m_text_panel;
		m_text_panel = NULL;
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

/*

void CTextParamsDlg::OnBnClickedTextOrientRadio1()
{
m_cur_text_style->state &= ~SG_TEXT_VERTICAL;
}

void CTextParamsDlg::OnBnClickedTextOrientRadio2()
{
m_cur_text_style->state |= SG_TEXT_VERTICAL;
}


*/

void     TextCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
												void* params) 
{
	
		if (mes==ICommander::CM_CHANGE_COMBO)
		{
			ASSERT(params!=NULL);
			IComboPanel*  cmbb = (reinterpret_cast<IComboPanel*>(params));
			if (cmbb!=m_orient_combo_panel)
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
bool    TextCommand::PreTranslateMessage(MSG* pMsg)
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

			if (m_text_text_panel && m_text_text_panel->IsInFocus())
					m_text_text_panel->GetWindow()->SendMessage(pMsg->message,
												pMsg->wParam,
												pMsg->lParam);
			else
				if (m_text_panel && m_text_panel->IsInFocus())
					m_text_panel->GetWindow()->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
			else
			{
				if (pMsg->wParam==VK_RETURN)
				{
					OnEnter();
					return true;
				}
				if (m_get_point_panel)
				{
						m_get_point_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
			
					if (pMsg->message==WM_KEYDOWN)
						return false;
					else 
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

void  TextCommand::Start()	
{
	SWITCH_RESOURCE

	CString lab;

	lab.LoadString(IDS_TOOLTIP_ZERO);
	m_app->StartCommander(lab);

	lab.LoadString(IDS_TEXT_POINT);
	m_get_point_panel = reinterpret_cast<IGetPointPanel*>(m_app->
	GetCommandPanel()->
	AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,lab.GetBuffer(),false));
	lab.ReleaseBuffer();

	m_text_text_panel = new CTextTextDlg(m_app,m_app->GetCommandPanel()->GetDialogsContainerWindow());
	m_text_text_panel->Create(IDD_TEXT_TEXT_DLG,m_app->GetCommandPanel()->GetDialogsContainerWindow());
	lab.LoadString(IDS_TOOLTIP_ZERO);
	m_app->GetCommandPanel()->AddDialog(m_text_text_panel,lab,false);

	lab.LoadString(IDS_TEXT_ORIENT);
	m_orient_combo_panel = 
		reinterpret_cast<IComboPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::COMBO_DLG,
		lab,false));

	m_message.LoadString(IDS_TEXT_ORINT_H);
	m_orient_combo_panel->AddString(m_message);
	m_message.LoadString(IDS_TEXT_ORINT_V);
	m_orient_combo_panel->AddString(m_message);
	
	m_orient_combo_panel->SetCurString(0);



	m_text_panel = new CTextParamsDlg(&m_text_style,m_app,m_app->GetCommandPanel()->GetDialogsContainerWindow());
	m_text_panel->Create(IDD_TEXT_PARAMS_DLG,m_app->GetCommandPanel()->GetDialogsContainerWindow());
	//m_text_panel->SetCommander(this);
	
	lab.LoadString(IDS_TEXT_PARAMS);
	m_app->GetCommandPanel()->AddDialog(m_text_panel,lab,false);

	m_app->GetCommandPanel()->SetActiveRadio(0);
	m_app->GetCommandPanel()->EnableRadio(1,true);
	m_app->GetCommandPanel()->EnableRadio(2,true);
	m_app->GetCommandPanel()->EnableRadio(3,true);

	m_text_text_panel->EnableControls(true);
	m_orient_combo_panel->EnableControls(true);
	m_text_panel->EnableControls(true);

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

void  TextCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	//if ((nFlags & MK_LBUTTON))
	{
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
		m_text_rot_matr.Identity();
		SG_VECTOR   vpN;
		m_app->GetViewPort()->GetViewPortNormal(vpN);
		if (out_arg.isOnWorkPlane && 
			m_app->GetViewPort()->GetViewPortViewType()==IViewPort::USER_VIEW)
		{
			SG_VECTOR tttV;
			tttV = out_arg.snapWorkPlaneNormal;
			if (sgSpaceMath::VectorsScalarMult(vpN,tttV)>0.0)
			{
				tttV.x = -tttV.x;tttV.y = -tttV.y;tttV.z = -tttV.z;
			}
			SG_VECTOR zP = {0.0, 0.0, 0.0};
			m_text_rot_matr.Rotate(zP,tttV,3.14159265358979323846);
			m_text_rot_matr.VectorToZAxe(tttV);
			m_text_rot_matr.Inverse();
		}
		else
		{
			vpN.x = -vpN.x;vpN.y = -vpN.y;vpN.z = -vpN.z;
			m_text_rot_matr.VectorToZAxe(vpN);
			m_text_rot_matr.Inverse();
		}
		m_app->GetViewPort()->InvalidateViewPort();
	}
}

void  TextCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	CString  ttt;
	if (m_text_text_panel)
		m_text_text_panel->GetText(ttt);
	else
		return;
	sgCText* ttO = sgCText::Create(sgFontManager::GetFont(sgFontManager::GetCurrentFont()),
		m_text_style,ttt);
	ttO->InitTempMatrix()->SetMatrix(&m_text_rot_matr);
	ttO->GetTempMatrix()->Translate(m_cur_pnt);
	ttO->ApplyTempMatrix();
	ttO->DestroyTempMatrix();
	if (!ttO)
	{
		ASSERT(0);
		return;
	}
	SWITCH_RESOURCE
	CString nm;
	nm.LoadString(IDS_TOOLTIP_ZERO);
	CString nmInd;
	nmInd.Format("%i",text_name_index);
	nm+=nmInd;
	ttO->SetName(nm.GetBuffer());
	sgGetScene()->StartUndoGroup();
	sgGetScene()->AttachObject(ttO);
	sgGetScene()->EndUndoGroup();
	text_name_index++;

	m_app->GetViewPort()->InvalidateViewPort();
	m_app->StopCommander();
}


void  TextCommand::Draw()
{
	CString  ttt;
	if (m_text_text_panel)
	{
		m_text_text_panel->GetText(ttt);
		float pC[3];
		m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
		m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		st_painter = m_app->GetViewPort()->GetPainter();
		sgCMatrix* mmm = new sgCMatrix(m_text_rot_matr);
		mmm->Translate(m_cur_pnt);
		sgCText::Draw(sgFontManager::GetFont(sgFontManager::GetCurrentFont()),m_text_style,mmm,
			ttt,draw_for_draw);
		delete mmm;
	}
}


void  TextCommand::OnEnter()
{
	CString  ttt;
	if (m_text_text_panel)
		m_text_text_panel->GetText(ttt);
	else
		return;
	sgCText* ttO = sgCText::Create(sgFontManager::GetFont(sgFontManager::GetCurrentFont()),
		m_text_style,ttt);
	if (!ttO)
	{
		ASSERT(0);
		return;
	}
	ttO->InitTempMatrix()->SetMatrix(&m_text_rot_matr);
	ttO->GetTempMatrix()->Translate(m_cur_pnt);
	ttO->ApplyTempMatrix();
	ttO->DestroyTempMatrix();
	SWITCH_RESOURCE
	CString nm;
	nm.LoadString(IDS_TOOLTIP_ZERO);
	CString nmInd;
	nmInd.Format("%i",text_name_index);
	nm+=nmInd;
	ttO->SetName(nm.GetBuffer());
	sgGetScene()->StartUndoGroup();
	sgGetScene()->AttachObject(ttO);
	sgGetScene()->EndUndoGroup();
	text_name_index++;

	m_app->GetViewPort()->InvalidateViewPort();
	m_app->StopCommander();
}

unsigned int  TextCommand::GetItemsCount()
{
	return 0;
}

void         TextCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	
}

void     TextCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	
}

HBITMAP   TextCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         TextCommand::Run(unsigned int itemID)
{
	
}
