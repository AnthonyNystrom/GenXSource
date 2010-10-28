#include "stdafx.h"

#include "RDDimCommand.h"

#include "..//resource.h"

int     rad_dim_name_index = 1;
int     dim_dim_name_index = 1;


static   bool isObjAddToList(sgCObject* obj)
{
	SG_OBJECT_TYPE ot = obj->GetType();
	if (ot==SG_OT_CIRCLE ||
		ot==SG_OT_ARC)
		return true;

	return false;
}


RDDimCommand::RDDimCommand(bool rad, IApplicationInterface* appI):
						m_app(appI)
							,m_get_object_panel(NULL)
							,m_get_point_panel(NULL)
							,m_props_panel(NULL)
							,m_text_align_panel(NULL)
							,m_text_params_panel(NULL)
							,m_precision_combo(NULL)
							,m_step(0)
							,m_rad_regime(rad)
							,m_obj(NULL)
							,m_cur_obj(NULL)
		
{
	ASSERT(m_app);

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

	m_dim_style.precision = 8;
}

RDDimCommand::~RDDimCommand()
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


void     RDDimCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
										  void* params) 
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=2);

		m_step = (unsigned int)newActiveDlg;

		for (unsigned int i=m_step+1;i<5;i++)
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
		if (mes==ICommander::CM_SELECT_OBJECT)
		{
			ASSERT(params!=NULL);
			ASSERT(m_step==0);
			sgCObject* so = (sgCObject*)params;
			if (so!=m_app->GetViewPort()->GetHotObject())
			{
				m_app->GetViewPort()->SetHotObject(so);
				m_cur_obj = so;
			}
			m_app->GetViewPort()->InvalidateViewPort();
		}
	}

}


bool    RDDimCommand::PreTranslateMessage(MSG* pMsg)
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
							if (m_get_object_panel)
								m_get_object_panel->GetWindow()->SendMessage(pMsg->message,
								pMsg->wParam,
								pMsg->lParam);
							break;
						case 1:
							if (m_get_point_panel)
								m_get_point_panel->GetWindow()->SendMessage(pMsg->message,
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

void  RDDimCommand::Start()	
{
	SWITCH_RESOURCE

	CString lab;
	lab.LoadString(IDS_TOOLTIP_FIRST);
	m_app->StartCommander(lab);

	NewScenar();

}

void  RDDimCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (m_step==0)
	{
		if (!(nFlags & MK_LBUTTON))
		{
			int snapSz = m_app->GetViewPort()->GetSnapSize();

			sgCObject* ho = m_app->GetViewPort()->GetTopObject(
				m_app->GetViewPort()->GetHitsInRect(CRect(pX-snapSz, pY-snapSz,
				pX+snapSz, pY+snapSz),true));
			if (ho && isObjAddToList(ho))
			{
				m_app->GetViewPort()->SetHotObject(ho);
				if (m_get_object_panel)
					m_get_object_panel->SelectObject(ho,true);
			}
			else
			{
				m_app->GetViewPort()->SetHotObject(NULL);
				if (m_get_object_panel)
					m_get_object_panel->SelectObject(NULL,true);
			}
			m_app->GetViewPort()->InvalidateViewPort();
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
		m_get_point_panel->GetPoint(in_arg.FixPoint.x,
			in_arg.FixPoint.y,
			in_arg.FixPoint.z);
		IViewPort::GET_SNAP_OUT out_arg;
		m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
		m_cur_pnt = out_arg.result_point;
		m_get_point_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
		m_app->GetViewPort()->InvalidateViewPort();
	}
}

void  RDDimCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	if (m_step==0)
	{
		m_cur_obj=m_app->GetViewPort()->GetHotObject();
		if (m_cur_obj)
		{
			if (m_app->GetViewPort()->GetHotObject()->IsSelect())
			{
				//m_message.LoadString(IDS_CONT_ALREADY_SEL);
				//m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}
			m_cur_obj->Select(true);
			m_obj = reinterpret_cast<sgC2DObject*>(m_cur_obj);
			if (m_obj->GetType()==SG_OT_CIRCLE)
			{
				sgCCircle* ccc =reinterpret_cast<sgCCircle*>(m_cur_obj);
				const SG_CIRCLE*  CG = ccc->GetGeometry();
				m_first_pnt = CG->center;
				m_second_pnt = ccc->GetPoints()[0];
			}
			else
			{
				if (m_obj->GetType()==SG_OT_ARC)
				{
					sgCArc* aaa =reinterpret_cast<sgCArc*>(m_cur_obj);
					const SG_ARC*  AG = aaa->GetGeometry();
					m_first_pnt = AG->center;
					m_second_pnt = aaa->GetPoints()[0];
				}
				else
				{
					ASSERT(0);
					return;
				}
			}
			m_step++;
			m_app->GetCommandPanel()->SetActiveRadio(m_step);
			m_message.LoadString(IDS_ENTER_S_POINT);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
				m_message);
			m_app->GetCommandPanel()->EnableRadio(3,true);
			m_app->GetCommandPanel()->EnableRadio(4,true);
			m_app->GetCommandPanel()->EnableRadio(5,true);
			m_app->GetCommandPanel()->EnableRadio(6,true);
			m_text_align_panel->EnableControls(true);
			m_text_params_panel->EnableControls(true);
			m_precision_combo->EnableControls(true);
		}
		else
			return;
	}
	else
			if (m_step==1)
					{
						SG_POINT ppp[3];
						ppp[0] = m_first_pnt;
						ppp[1] = m_second_pnt;
						ppp[2] = m_cur_pnt;
						
						sgCDimensions* dm = sgCDimensions::Create((m_rad_regime)?SG_DT_RAD:SG_DT_DIAM,ppp,
							sgFontManager::GetFont(sgFontManager::GetCurrentFont()),m_dim_style,
							NULL);
						if (!dm)
							return;
						CString nm;
						nm.LoadString(IDS_TOOLTIP_THIRD);
						CString nmInd;
						nmInd.Format("%i",rad_dim_name_index);
						nm+=nmInd;
						dm->SetName(nm.GetBuffer());
						sgGetScene()->StartUndoGroup();
						sgGetScene()->AttachObject(dm);
						sgGetScene()->EndUndoGroup();
						rad_dim_name_index++;

						m_app->GetViewPort()->InvalidateViewPort();
						m_step=0;

						if (sgGetScene()->GetSelectedObjectsList()->GetCount()>0)
						{
							sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
							while (curObj) 
							{
								curObj->Select(false);
								curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
							}
						}

						NewScenar();
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

void  RDDimCommand::Draw()
{
	float pC[3];
	if(m_step==1) 
	{
		m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		st_painter = m_app->GetViewPort()->GetPainter();
		SG_POINT ppp[3];
		ppp[0] = m_first_pnt;
		ppp[1] = m_second_pnt;
		ppp[2] = m_cur_pnt;
		sgCDimensions::Draw((m_rad_regime)?SG_DT_RAD:SG_DT_DIAM,ppp,
			sgFontManager::GetFont(sgFontManager::GetCurrentFont()),m_dim_style,
			NULL/*"sfsdf"*/,draw_for_draw);
	}

}


void  RDDimCommand::OnEnter()
{
	SWITCH_RESOURCE
}

unsigned int  RDDimCommand::GetItemsCount()
{
	return 0;
}

void         RDDimCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	
}

void     RDDimCommand::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	
}

HBITMAP   RDDimCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         RDDimCommand::Run(unsigned int itemID)
{
	
}

void     RDDimCommand::NewScenar()
{
	SWITCH_RESOURCE
		
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

	CString lab;
	lab.LoadString(IDS_OBJECT);
	m_get_object_panel = 
		reinterpret_cast<IGetObjectsPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,
		lab,true));

	m_get_object_panel->SetMultiselectMode(false);
	m_get_object_panel->FillList(isObjAddToList);
	
	lab.LoadString(IDS_L_POINT);
	m_get_point_panel = 
		reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
		lab,true));

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
	m_text_align_panel->EnableControls(true);
	m_text_params_panel->EnableControls(true);
	m_precision_combo->EnableControls(true);


	m_message.LoadString(IDS_ENTER_F_POINT);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

	m_step=0;
}
