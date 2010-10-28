#include "stdafx.h"

#include "BooleanOperation.h"

#include "..//resource.h"

int     bool_name_index = 1;

BooleanCommand::BooleanCommand(IApplicationInterface* appI,BO_TYPE bt):
				m_app(appI)
				,m_bo_type(bt)
				,m_get_first_obj_panel(NULL)
				,m_get_second_obj_panel(NULL)
				,m_step(0)
				, m_first_obj(NULL)
				, m_second_obj(NULL)
				, m_sec_obj_sel(false)
{
	ASSERT(m_app);
}

BooleanCommand::~BooleanCommand()
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


bool    BooleanCommand::PreTranslateMessage(MSG* pMsg)
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
			if (m_step==0 && m_get_first_obj_panel)
			{
				m_get_first_obj_panel->GetWindow()->SendMessage(WM_CHAR,
					pMsg->wParam,
					pMsg->lParam);
				return true;
			}
			if (m_step==1 && m_get_second_obj_panel)
			{
				m_get_second_obj_panel->GetWindow()->SendMessage(WM_CHAR,
					pMsg->wParam,
					pMsg->lParam);
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


void   BooleanCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, void* params)
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=1);
		m_step = (unsigned int)newActiveDlg;
		if (newActiveDlg==0)
		{
			m_app->GetCommandPanel()->EnableRadio(1,false);
			m_message.LoadString(IDS_SEL_F_O);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		}
		else
		{
			m_message.LoadString(IDS_SEL_S_O);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		}
		m_app->GetViewPort()->InvalidateViewPort();
		return;
	}
	if (mes==ICommander::CM_SELECT_OBJECT)
	{
		ASSERT(params!=NULL);
		sgCObject* so = (sgCObject*)params;
		if(so->GetType()!=SG_OT_3D)
		{
			ASSERT(0);
			return;
		}
		so->Select(true);
		
		if (m_step==0)
		{
			if (m_first_obj!=so)
			{
				if (m_first_obj)
				{
					m_first_obj->Select(false);
					m_get_second_obj_panel->AddObject(m_first_obj,false);
				}
				m_first_obj = reinterpret_cast<sgC3DObject*>(so);
				m_get_second_obj_panel->RemoveObject(m_first_obj);
			}
		}
		else
		{
			if (m_second_obj!=so)
			{
				if (m_second_obj)
					m_second_obj->Select(false);
				m_second_obj = reinterpret_cast<sgC3DObject*>(so);
			}
		}
		m_app->GetViewPort()->InvalidateViewPort();
	}
}

static   bool isObjAddToList(sgCObject* obj)
{
	if (obj->GetType()==SG_OT_3D)
		return true;
	return false;
}

void  BooleanCommand::Start()	
{
	SWITCH_RESOURCE
	
	m_app->GetCommandPanel()->RemoveAllDialogs();

	switch(m_bo_type) {
	case BO_INTSCT:
		m_message.LoadString(IDS_TOOLTIP_ZERO);
		break;
	case BO_UNION:
		m_message.LoadString(IDS_TOOLTIP_FIRST);
		break;
	case BO_SUB:
		m_message.LoadString(IDS_TOOLTIP_SECOND);
		break;
	case BO_IL:
		m_message.LoadString(IDS_TOOLTIP_THIRD);
		break;
	default:
		ASSERT(0);
		return;
	}
	
	m_app->StartCommander(m_message);

	CString lab;
	lab.LoadString(IDS_FIRST_OBJECT);
	m_get_first_obj_panel = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,lab,true));
	
	lab.LoadString(IDS_SECOND_OBJECT);
	m_get_second_obj_panel = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,lab,true));
	
	m_get_first_obj_panel->SetMultiselectMode(false);
	m_get_second_obj_panel->SetMultiselectMode(false);

	m_get_first_obj_panel->FillList(isObjAddToList);
	m_get_second_obj_panel->FillList(isObjAddToList);

	m_step=0;
	m_app->GetCommandPanel()->SetActiveRadio(0);

	m_message.LoadString(IDS_SEL_F_O);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
}

void    BooleanCommand::BuildBoolean()
{
	ASSERT(m_first_obj);
	ASSERT(m_second_obj);

	sgCGroup*  resGr = NULL;
	
	switch(m_bo_type) {
	case BO_INTSCT:
		resGr = sgBoolean::Intersection(*m_first_obj,*m_second_obj);
		break;
	case BO_UNION:
		resGr = sgBoolean::Union(*m_first_obj,*m_second_obj);
		break;
	case BO_SUB:
		resGr = sgBoolean::Sub(*m_first_obj,*m_second_obj);
		break;
	case BO_IL:
		resGr = sgBoolean::IntersectionContour(*m_first_obj,*m_second_obj);
		break;
	default:
		return;
	}
	if (!resGr)
	{
		m_message.LoadString(IDS_ERR_CANT_CREATE);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
		return;
	}

	const int ChCnt = resGr->GetChildrenList()->GetCount();

	sgCObject**  allChilds = (sgCObject**)malloc(ChCnt*sizeof(sgCObject*));
	if (!resGr->BreakGroup(allChilds))
	{
		ASSERT(0);
	}
	const int sz = resGr->GetChildrenList()->GetCount();
	ASSERT(sz==0);
	sgCObject::DeleteObject(resGr);

	SWITCH_RESOURCE
	CString nm;
	nm.LoadString(IDS_BOOL_NAME);
	CString nmInd;
	CString curName;
	
	sgGetScene()->StartUndoGroup();
	for (int i=0;i<ChCnt;i++)
	{
		sgGetScene()->AttachObject(allChilds[i]);
		nmInd.Format("%i",bool_name_index);
		curName = nm+nmInd;
		allChilds[i]->SetName(curName);
		if (allChilds[i]->GetType()==SG_OT_3D)
		{
			sgC3DObject* cccO = reinterpret_cast<sgC3DObject*>(allChilds[i]);
			const SG_MATERIAL* mmm = m_first_obj->GetMaterial();
			if (mmm)
				cccO->SetMaterial(*mmm);
		}
		bool_name_index++;
		m_app->ApplyAttributes(allChilds[i]);
	}

	free(allChilds);
	sgGetScene()->DetachObject(m_first_obj);
	sgGetScene()->DetachObject(m_second_obj);
	sgGetScene()->EndUndoGroup();

	if (sgGetScene()->GetSelectedObjectsList()->GetCount()>0)
	{
		sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
		while (curObj) 
		{
			curObj->Select(false);
			curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
		}
	}

}

void  BooleanCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	//if (m_step==0)
	{
		if (!(nFlags & MK_LBUTTON))
		{
			int snapSz = m_app->GetViewPort()->GetSnapSize();

			m_app->GetViewPort()->SetHotObject(m_app->GetViewPort()->GetTopObjectByType(
				m_app->GetViewPort()->GetHitsInRect(CRect(pX-snapSz, pY-snapSz,
				pX+snapSz, pY+snapSz)),SG_OT_3D));
			m_app->GetViewPort()->InvalidateViewPort();
		}
	}
	/*else
	{
	}*/
}

void  BooleanCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	if (m_step==0)
	{
		if (m_app->GetViewPort()->GetHotObject())
		{
			m_app->GetViewPort()->GetHotObject()->Select(true);
			
			if (m_first_obj!=m_app->GetViewPort()->GetHotObject())
			{
				if (m_first_obj)
				{
					m_first_obj->Select(false);
					m_get_second_obj_panel->AddObject(m_first_obj,false);
				}
				m_first_obj = reinterpret_cast<sgC3DObject*>(m_app->
					GetViewPort()->GetHotObject());
				m_get_second_obj_panel->RemoveObject(m_first_obj);
			}

			m_step++;
			m_app->GetCommandPanel()->SetActiveRadio(1);
			m_app->GetViewPort()->InvalidateViewPort();
		}
	}
	else
	{
		sgCObject* co = m_app->GetViewPort()->GetHotObject();
		if (co && co!=m_first_obj)
		{
			co->Select(true);

			if (m_second_obj!=co)
			{
				if (m_second_obj)
					m_second_obj->Select(false);
				
				m_second_obj = reinterpret_cast<sgC3DObject*>(co);
				BuildBoolean();
				m_step=0;
				m_app->GetCommandPanel()->SetActiveRadio(0);

				m_message.LoadString(IDS_SEL_F_O);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
				m_app->GetViewPort()->InvalidateViewPort();
			}
		}
	}
}


void  BooleanCommand::Draw()
{
}

void  BooleanCommand::OnEnter()
{
	if (m_step==0)
	{
		if (!m_first_obj)
			return;
		m_step++;
		m_app->GetCommandPanel()->SetActiveRadio(1);
		m_app->GetViewPort()->InvalidateViewPort();
	}
	else
	{
		if (!m_first_obj || !m_second_obj)
			return;

		BuildBoolean();
		m_step=0;
		m_app->GetCommandPanel()->SetActiveRadio(0);

		m_message.LoadString(IDS_SEL_F_O);
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		m_app->GetViewPort()->InvalidateViewPort();
	}
}

unsigned int  BooleanCommand::GetItemsCount()
{
	return 0;
}

void         BooleanCommand::GetItem(unsigned int, CString&)
{
}

void     BooleanCommand::GetItemState(unsigned int, bool&, bool&)
{
}

HBITMAP   BooleanCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         BooleanCommand::Run(unsigned int)
{
}