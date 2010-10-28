#include "stdafx.h"

#include "LinearDimCommand.h"

#include "..//resource.h"

int     linear_dim_name_index = 1;

LinearDimCommand::LinearDimCommand(IApplicationInterface* appI):
						m_app(appI)
							,m_get_first_point_panel(NULL)
							,m_get_second_point_panel(NULL)
							,m_get_last_point_panel(NULL)
							,m_props_panel(NULL)
							,m_text_align_panel(NULL)
							,m_text_params_panel(NULL)
							,m_styles_combo(NULL)
							,m_precision_combo(NULL)
							,m_step(0)
		
{
	ASSERT(m_app);
	m_dim_style.behaviour_type = SG_DBT_PARALLEL;
	m_dim_style.dimension_line = true;
	m_dim_style.first_side_line = true;
	m_dim_style.second_side_line = true;
	m_dim_style.lug_size = 0.5;

	m_dim_style.automatic_arrows = true;

	m_dim_style.out_first_arrow = false;
	m_dim_style.first_arrow_style = 1;

	m_dim_style.out_second_arrow= false;
	m_dim_style.second_arrow_style= 1;

	m_dim_style.arrows_size= 1.1;

	m_dim_style.text_align = SG_TA_CENTER;

	m_dim_style.text_style.state = 0;
	m_dim_style.text_style.height= 0.4;
	m_dim_style.text_style.proportions= 100.0;
	m_dim_style.text_style.angle= 30.0;
	m_dim_style.text_style.horiz_space_proportion= 0.0;
	m_dim_style.text_style.vert_space_proportion= 50.0; 

	m_dim_style.precision = 3;
}

LinearDimCommand::~LinearDimCommand()
{	
	m_app->GetCommandPanel()->RemoveAllDialogs();
	if (m_props_panel)
	{
		m_props_panel->DestroyWindow();
		delete m_props_panel;
		m_props_panel = NULL;
	}
	if  (m_text_params_panel)
	{
		m_text_params_panel->DestroyWindow();
		delete m_text_params_panel;
		m_text_params_panel = NULL;
	}
	m_app->GetViewPort()->InvalidateViewPort();
}


void     LinearDimCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
										  void* params) 
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=2);

		m_step = (unsigned int)newActiveDlg;

		for (unsigned int i=m_step+1;i<3;i++)
			m_app->GetCommandPanel()->EnableRadio(i,false);

		SWITCH_RESOURCE
		switch(m_step) 
		{
		case 0:
			m_message.LoadString(IDS_ENTER_F_POINT);
			break;
		case 1:
			m_message.LoadString(IDS_ENTER_S_POINT);	
			break;
		case 2:
			m_message.LoadString(IDS_ENTER_L_POINT);	
			break;
		}
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
			m_message);

		m_app->GetViewPort()->InvalidateViewPort();
		return;
	}
	else
	{
		if (mes==ICommander::CM_CHANGE_COMBO)
		{
			ASSERT(params!=NULL);
			IComboPanel*  cmbb = (reinterpret_cast<IComboPanel*>(params));
			if (cmbb==m_text_align_panel)
			{	
				switch(cmbb->GetCurString()) 
				{
				case 0:
					m_dim_style.text_align = SG_TA_CENTER;
					break;
				case 1:
					m_dim_style.text_align = SG_TA_LEFT;
					break;
				case 2:
					m_dim_style.text_align = SG_TA_RIGHT;
					break;
				default:
					ASSERT(0);
				}
			}
			else
				if (cmbb==m_styles_combo)
				{
					switch(cmbb->GetCurString()) 
					{
					case 0:
						m_dim_style.behaviour_type = SG_DBT_HORIZONTAL;
						break;
					case 1:
						m_dim_style.behaviour_type = SG_DBT_VERTICAL;
						break;
					case 2:
						m_dim_style.behaviour_type = SG_DBT_PARALLEL;
						break;
					case 3:
						m_dim_style.behaviour_type = SG_DBT_SLANT;
						break;
					case 4:
						m_dim_style.behaviour_type = SG_DBT_OPTIMAL;
						break;
					default:
						ASSERT(0);
					}
				}
				else
					if (cmbb==m_precision_combo)
					{
						m_dim_style.precision = cmbb->GetCurString()+1;
					}
					else
					{
						ASSERT(0);
						return;
					}
			m_app->GetViewPort()->InvalidateViewPort();
		}
	}
}


bool    LinearDimCommand::PreTranslateMessage(MSG* pMsg)
{
	try { //#try
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
				if (m_props_panel && m_props_panel->IsInFocus())
					m_props_panel->GetWindow()->SendMessage(pMsg->message,
									pMsg->wParam,
									pMsg->lParam);
				else
					if (m_text_params_panel && m_text_params_panel->IsInFocus())
						m_text_params_panel->GetWindow()->SendMessage(pMsg->message,
										pMsg->wParam,
										pMsg->lParam);
					else
					{
						switch(m_step) 
						{
						case 0:
							if (m_get_first_point_panel)
								m_get_first_point_panel->GetWindow()->SendMessage(pMsg->message,
								pMsg->wParam,
								pMsg->lParam);
							break;
						case 1:
							if (m_get_second_point_panel)
								m_get_second_point_panel->GetWindow()->SendMessage(pMsg->message,
								pMsg->wParam,
								pMsg->lParam);
							break;
						case 2:
							if (m_get_last_point_panel)
								m_get_last_point_panel->GetWindow()->SendMessage(pMsg->message,
								pMsg->wParam,
								pMsg->lParam);
							break;
						}
						if (pMsg->message==WM_KEYDOWN)
							return false;
						else 
							return true;
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

void  LinearDimCommand::Start()	
{
	SWITCH_RESOURCE

	CString lab;
	lab.LoadString(IDS_TOOLTIP_FIRST);
	m_app->StartCommander(lab);

	NewScenar();

}

void  LinearDimCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	IGetPointPanel* this_step = NULL;
	switch(m_step) 
	{
	case 0:
		this_step = m_get_first_point_panel;
		break;
	case 1:
		this_step = m_get_second_point_panel;
		break;
	case 2:
		this_step = m_get_last_point_panel;
		break;
	default:
		ASSERT(0);
		break;
	}
	IViewPort::GET_SNAP_IN in_arg;
	in_arg.scrX = pX;
	in_arg.scrY = pY;
	in_arg.snapType = SNAP_SYSTEM;
	in_arg.XFix = this_step->IsXFixed();
	in_arg.YFix = this_step->IsYFixed();
	in_arg.ZFix = this_step->IsZFixed();
	this_step->GetPoint(in_arg.FixPoint.x,in_arg.FixPoint.y,in_arg.FixPoint.z);
	IViewPort::GET_SNAP_OUT out_arg;
	m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
	m_cur_pnt = out_arg.result_point;
	this_step->SetPoint((float)(m_cur_pnt.x),(float)(m_cur_pnt.y),(float)(m_cur_pnt.z));

	if (m_step==2)
	{
		/*m_exist_arc_data = sgCArc::CreateArcGeoFrom_b_e_m(m_first_pnt,
			m_second_pnt,
			m_cur_pnt,
			m_invert,
			m_arc_geo_data);*/
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

void  LinearDimCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	if (m_step==0)
	{
		m_first_pnt=m_cur_pnt;
		m_step++;
		m_app->GetCommandPanel()->SetActiveRadio(m_step);
		m_message.LoadString(IDS_ENTER_S_POINT);
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
			m_message);
		m_app->GetCommandPanel()->EnableRadio(3,true);
		m_app->GetCommandPanel()->EnableRadio(4,true);
		m_app->GetCommandPanel()->EnableRadio(5,true);
		m_app->GetCommandPanel()->EnableRadio(6,true);
		m_app->GetCommandPanel()->EnableRadio(7,true);
		m_text_align_panel->EnableControls(true);
		m_text_params_panel->EnableControls(true);
		m_styles_combo->EnableControls(true);
		m_precision_combo->EnableControls(true);

	}
	else
		if (m_step==1)
		{
			if (sgSpaceMath::PointsDistance(m_first_pnt,m_cur_pnt)<0.000001)
			{
				m_message.LoadString(IDS_ERROR_POINT_IS_EQ);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			m_second_pnt=m_cur_pnt;
			m_step++;
			m_app->GetCommandPanel()->SetActiveRadio(m_step);
			m_message.LoadString(IDS_ENTER_L_POINT);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
				m_message);
			m_app->GetCommandPanel()->EnableRadio(3,true);
			m_app->GetCommandPanel()->EnableRadio(4,true);
			m_app->GetCommandPanel()->EnableRadio(5,true);
			m_app->GetCommandPanel()->EnableRadio(6,true);
			m_app->GetCommandPanel()->EnableRadio(7,true);
			m_text_align_panel->EnableControls(true);
			m_text_params_panel->EnableControls(true);
			m_styles_combo->EnableControls(true);
			m_precision_combo->EnableControls(true);

		}
		else
			if (m_step==2)
			{
				m_text = "asdas";
				SG_POINT ppp[3];
				ppp[0] = m_first_pnt;
				ppp[1] = m_second_pnt;
				ppp[2] = m_cur_pnt;
				sgCDimensions* dm = sgCDimensions::Create(SG_DT_LINEAR,ppp,
					sgFontManager::GetFont(sgFontManager::GetCurrentFont()),m_dim_style,
					NULL);
				if (!dm)
					return;
				CString nm;
				nm.LoadString(IDS_TOOLTIP_FIRST);
				CString nmInd;
				nmInd.Format("%i",linear_dim_name_index);
				nm+=nmInd;
				dm->SetName(nm);
				sgGetScene()->StartUndoGroup();
				sgGetScene()->AttachObject(dm);
				sgGetScene()->EndUndoGroup();
				linear_dim_name_index++;

				m_app->GetViewPort()->InvalidateViewPort();
				m_step=0;

				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_ENTER_F_POINT);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
					m_message);
				m_app->GetCommandPanel()->EnableRadio(1,false);
				m_app->GetCommandPanel()->EnableRadio(2,false);
				m_app->GetCommandPanel()->EnableRadio(3,true);
				m_app->GetCommandPanel()->EnableRadio(4,true);
				m_app->GetCommandPanel()->EnableRadio(5,true);
				m_app->GetCommandPanel()->EnableRadio(6,true);
				m_app->GetCommandPanel()->EnableRadio(7,true);
				m_text_align_panel->EnableControls(true);
				m_text_params_panel->EnableControls(true);
				m_styles_combo->EnableControls(true);
				m_precision_combo->EnableControls(true);
			}
			else
				ASSERT(0);
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

void  LinearDimCommand::Draw()
{
	float pC[3];
	m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
	switch(m_step) 
	{
	case 1:
		m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		SG_LINE ln;
		ln.p1 = m_first_pnt;
		ln.p2 = m_cur_pnt;
		m_app->GetViewPort()->GetPainter()->DrawLine(ln);
		break;
	case 2:
		m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		st_painter = m_app->GetViewPort()->GetPainter();
		m_text = "a<>sghjk";
		SG_VECTOR plN;
		double    plD;
		sgSpaceMath::PlaneFromPoints(m_first_pnt,m_second_pnt,m_cur_pnt,plN,plD);
		SG_VECTOR viewN;
		m_app->GetViewPort()->GetViewPortNormal(viewN);
		if (sgSpaceMath::VectorsScalarMult(plN,viewN)>0)
		{
			SG_POINT ttt = m_second_pnt;
			m_second_pnt = m_first_pnt;
			m_first_pnt = ttt;
		}
		SG_POINT ppp[3];
		ppp[0] = m_first_pnt;
		ppp[1] = m_second_pnt;
		ppp[2] = m_cur_pnt;
		sgCDimensions::Draw(SG_DT_LINEAR,ppp,
			sgFontManager::GetFont(sgFontManager::GetCurrentFont()),m_dim_style,
			/*m_text*/NULL,draw_for_draw);
		break;
	}

}


void  LinearDimCommand::OnEnter()
{
	SWITCH_RESOURCE
	if (m_step==0)
	{
		ASSERT(m_get_first_point_panel);

		m_get_first_point_panel->GetPoint(m_first_pnt.x,m_first_pnt.y,m_first_pnt.z);

		m_step++;
		m_app->GetCommandPanel()->SetActiveRadio(m_step);
		m_message.LoadString(IDS_ENTER_S_POINT);
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
			m_message);
		m_app->GetCommandPanel()->EnableRadio(3,true);
		m_app->GetCommandPanel()->EnableRadio(4,true);
		m_app->GetCommandPanel()->EnableRadio(5,true);
		m_app->GetCommandPanel()->EnableRadio(6,true);
		m_app->GetCommandPanel()->EnableRadio(7,true);
		m_text_align_panel->EnableControls(true);
		m_text_params_panel->EnableControls(true);
		m_styles_combo->EnableControls(true);
		m_precision_combo->EnableControls(true);

	}
	else
		if (m_step==1)
		{
			ASSERT(m_get_second_point_panel);

			m_get_second_point_panel->GetPoint(m_second_pnt.x,m_second_pnt.y,m_second_pnt.z);

			if (sgSpaceMath::PointsDistance(m_first_pnt,m_second_pnt)<0.000001)
			{
				m_message.LoadString(IDS_ERROR_POINT_IS_EQ);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			m_step++;
			m_app->GetCommandPanel()->SetActiveRadio(m_step);
			m_message.LoadString(IDS_ENTER_L_POINT);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
				m_message);
			m_app->GetCommandPanel()->EnableRadio(3,true);
			m_app->GetCommandPanel()->EnableRadio(4,true);
			m_app->GetCommandPanel()->EnableRadio(5,true);
			m_app->GetCommandPanel()->EnableRadio(6,true);
			m_app->GetCommandPanel()->EnableRadio(7,true);
			m_text_align_panel->EnableControls(true);
			m_text_params_panel->EnableControls(true);
			m_styles_combo->EnableControls(true);
			m_precision_combo->EnableControls(true);

		}
		else
			if (m_step==2/* && m_exist_arc_data*/)
			{
				/*sgCArc* ar = sgCreateArc(&m_arc_geo_data);
				if (!ar)
				return;
				CString nm;
				nm.LoadString(IDS_TOOLTIP_FIRST);
				CString nmInd;
				nmInd.Format("%i",linear_dim_name_index);
				nm+=nmInd;
				ar->SetName(nm.GetBuffer());
				sgGetScene()->StartUndoGroup();
				sgGetScene()->AttachObject(ar);
				sgGetScene()->EndUndoGroup();
				arc_name_index++;

				m_app->GetViewPort()->InvalidateViewPort();
				m_step=0;
				m_exist_arc_data = false;
				Arc_b_e_m();*/
				NewScenar();
			}
			else
				ASSERT(0);
}

unsigned int  LinearDimCommand::GetItemsCount()
{
	return 0;
}

void         LinearDimCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	
}

void     LinearDimCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	
}

HBITMAP   LinearDimCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         LinearDimCommand::Run(unsigned int itemID)
{
	
}

void     LinearDimCommand::NewScenar()
{
	SWITCH_RESOURCE
		
	m_app->GetCommandPanel()->RemoveAllDialogs();
	CString lab;
	lab.LoadString(IDS_F_POINT);
	m_get_first_point_panel = 
		reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
		lab,true));

	lab.LoadString(IDS_S_POINT);
	m_get_second_point_panel = 
		reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
		lab,true));


	lab.LoadString(IDS_L_POINT);
	m_get_last_point_panel = 
		reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
		lab,true));

	lab.LoadString(IDS_LIN_STYLE);
	
	m_styles_combo = 
		reinterpret_cast<IComboPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::COMBO_DLG,
		lab,false));

	m_message.LoadString(IDS_TEXT_ORINT_H);
	m_styles_combo->AddString(m_message);
	m_message.LoadString(IDS_TEXT_ORINT_V);
	m_styles_combo->AddString(m_message);
	m_message.LoadString(IDS_DIM_ST_PARALL);
	m_styles_combo->AddString(m_message);
	m_message.LoadString(IDS_DIM_ST_POINTS);
	m_styles_combo->AddString(m_message);
	m_message.LoadString(IDS_DIM_ST_OPT);
	m_styles_combo->AddString(m_message);

	m_styles_combo->SetCurString(2);
	

	lab.LoadString(IDS_PRECIS);

	m_precision_combo = 
		reinterpret_cast<IComboPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::COMBO_DLG,
		lab,false));

	m_precision_combo->AddString("0.1");
	m_precision_combo->AddString("0.01");
	m_precision_combo->AddString("0.001");
	m_precision_combo->AddString("0.0001");
	m_precision_combo->AddString("0.00001");
	m_precision_combo->SetCurString(2);

	m_props_panel = new CLinearDimDlg(m_app->GetCommandPanel(), &m_dim_style, m_app,false,
		m_app->GetCommandPanel()->GetDialogsContainerWindow());
	m_props_panel->Create(IDD_LINEAR_DIM_DLG,m_app->GetCommandPanel()->GetDialogsContainerWindow());
	//m_text_panel->SetCommander(this);

	lab.LoadString(IDS_DIM_PARAM);
	m_app->GetCommandPanel()->AddDialog(m_props_panel,lab,false);

	lab.LoadString(IDS_TEXT_ALIGN);
	m_text_align_panel = 
		reinterpret_cast<IComboPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::COMBO_DLG,
		lab,false));

	m_message.LoadString(IDS_TA_CENTER);
	m_text_align_panel->AddString(m_message);
	m_message.LoadString(IDS_TA_LEFT);
	m_text_align_panel->AddString(m_message);
	m_message.LoadString(IDS_TA_RIGHT);
	m_text_align_panel->AddString(m_message);
	m_text_align_panel->SetCurString(0);

	m_text_params_panel = new CTextParamsDlg(&m_dim_style.text_style,m_app,m_app->GetCommandPanel()->GetDialogsContainerWindow());
	m_text_params_panel->Create(IDD_TEXT_PARAMS_DLG,m_app->GetCommandPanel()->GetDialogsContainerWindow());
	
	lab.LoadString(IDS_TEXT_PARAMS);
	m_app->GetCommandPanel()->AddDialog(m_text_params_panel,lab,false);


	m_app->GetCommandPanel()->SetActiveRadio(0);
	m_app->GetCommandPanel()->EnableRadio(3,true);
	m_app->GetCommandPanel()->EnableRadio(4,true);
	m_app->GetCommandPanel()->EnableRadio(5,true);
	m_app->GetCommandPanel()->EnableRadio(6,true);
	m_app->GetCommandPanel()->EnableRadio(7,true);
	m_text_align_panel->EnableControls(true);
	m_text_params_panel->EnableControls(true);
	m_styles_combo->EnableControls(true);
	m_precision_combo->EnableControls(true);

	
	m_message.LoadString(IDS_ENTER_F_POINT);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

	m_step=0;
}
