#include "stdafx.h"

#include "Coons3Command.h"

#include "..//resource.h"

Coons3Command::Coons3Command(IApplicationInterface* appI):
						m_app(appI)
{
	ASSERT(m_app);
	m_step = 0;
	m_cur_obj = NULL;

	m_get_object_panels[0] = 
		m_get_object_panels[1] = 
		m_get_object_panels[2] = NULL;

	m_objs[0] = 
		m_objs[1] = 
		m_objs[2] =  NULL;
}

Coons3Command::~Coons3Command()
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


bool    Coons3Command::PreTranslateMessage(MSG* pMsg)
{
	/*if (pMsg->message==WM_KEYUP||
	pMsg->message==WM_CHAR)
	return false;*/

	try {  //#try
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
			if(m_step>=0 && m_step<3) 
			{
				if (m_get_object_panels[m_step])
					m_get_object_panels[m_step]->GetWindow()->SendMessage(pMsg->message,
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

void     Coons3Command::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
										   void* params) 
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=1);
		m_step = (unsigned int)newActiveDlg;
		for (unsigned int i=m_step+1;i<=1;i++)
		{
			m_app->GetCommandPanel()->EnableRadio(i,false);
			m_objs[i]=NULL;
		}
		SWITCH_RESOURCE
			switch(m_step) 
			{
				case 0:
					m_message.LoadString(IDS_COON_SEL_1_CONT);
					break;
				case 1:
					m_message.LoadString(IDS_COON_SEL_2_CONT);
					break;
				case 2:
					m_message.LoadString(IDS_COON_SEL_3_CONT);
					break;
				default:
					ASSERT(0);
					break;
			}
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		return;
	}
	if (mes==ICommander::CM_SELECT_OBJECT)
	{
		ASSERT(params!=NULL);
		ASSERT(m_step<=2);
		sgCObject* so = (sgCObject*)params;
		if (so!=m_app->GetViewPort()->GetHotObject())
		{
			m_app->GetViewPort()->SetHotObject(so);
			m_cur_obj = so;
		}
		m_app->GetViewPort()->InvalidateViewPort();
	}
}



static   bool isObjAddToList(sgCObject* obj)
{
	SG_OBJECT_TYPE ot = obj->GetType();
	if (ot==SG_OT_LINE ||
		ot==SG_OT_ARC)
			return true;
	if (ot==SG_OT_SPLINE)
	{
		sgCSpline* spl = reinterpret_cast<sgCSpline*>(obj);
		if (!spl->IsClosed() && !spl->IsSelfIntersecting())
			return true;
		else
			return false;
	}
	if (ot==SG_OT_CONTOUR)
	{
		sgCContour* cntr = reinterpret_cast<sgCContour*>(obj);
		if (!cntr->IsClosed() && !cntr->IsSelfIntersecting())
			return true;
		else
			return false;
	}
	return false;
}


void  Coons3Command::Start()	
{
	SWITCH_RESOURCE

	m_app->GetCommandPanel()->RemoveAllDialogs();

	m_message.LoadString(IDS_TOOLTIP_SECOND);
	m_app->StartCommander(m_message);

	m_message.LoadString(IDS_CONT);
	m_get_object_panels[0] = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,m_message,true));
	
	m_get_object_panels[1] = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,m_message,true));

	m_get_object_panels[2] = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,m_message,true));

	for (int i=0;i<3;i++)
	{
		if (m_get_object_panels[i])
		{
			m_get_object_panels[i]->SetMultiselectMode(false);
			m_get_object_panels[i]->FillList(isObjAddToList);
		}
	}
	
	m_step=0;
	m_app->GetCommandPanel()->SetActiveRadio(0);

	m_message.LoadString(IDS_COON_SEL_1_CONT);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

}

void  Coons3Command::MouseMove(unsigned int nFlags,int pX,int pY)
{
	//if (m_step==0)
	{
		if (!(nFlags & MK_LBUTTON))
		{
			int snapSz = m_app->GetViewPort()->GetSnapSize();

			sgCObject* ho = m_app->GetViewPort()->GetTopObject(
				m_app->GetViewPort()->GetHitsInRect(CRect(pX-snapSz, pY-snapSz,
				pX+snapSz, pY+snapSz)));
			if (ho && isObjAddToList(ho))
			{
				m_app->GetViewPort()->SetHotObject(ho);
				if (m_get_object_panels[m_step])
					m_get_object_panels[m_step]->SelectObject(ho,true);
			}
			else
			{
				m_app->GetViewPort()->SetHotObject(NULL);
				if (m_get_object_panels[m_step])
					m_get_object_panels[m_step]->SelectObject(NULL,true);
			}
			m_app->GetViewPort()->InvalidateViewPort();
		}
		return;
	}
}

bool  Coons3Command::CheckTwoObjects()
{
	m_objs[2] = m_objs[3] = NULL;
	if (m_objs[0]==NULL || m_objs[1]==NULL)
		return false;

	SG_POINT begFir, endFir, begSec, endSec;
	begFir = m_objs[0]->GetPointFromCoefficient(0.0);
	endFir = m_objs[0]->GetPointFromCoefficient(1.0);
	begSec = m_objs[1]->GetPointFromCoefficient(0.0);
	endSec = m_objs[1]->GetPointFromCoefficient(1.0);

	if (sgSpaceMath::PointsDistance(begFir,begSec)<0.00001)
	{
		return true;
	}
	else
		if (sgSpaceMath::PointsDistance(begFir,endSec)<0.00001)
		{
			return true;
		}
		else
			if (sgSpaceMath::PointsDistance(endFir,begSec)<0.00001)
			{
				return true;
			}
			else
				if (sgSpaceMath::PointsDistance(endFir,endSec)<0.00001)
				{
					return true;
				}
				else
				{
					return false;
				}
}

bool  Coons3Command::CheckThreeObjects()
{
	m_objs[3] = NULL;
	if (m_objs[0]==NULL || m_objs[1]==NULL || m_objs[2]==NULL)
		return false;

	typedef struct  
	{
		SG_POINT  begin_of_curve;
		SG_POINT  end_of_curve;
	} BEGIN_AND_END;

	BEGIN_AND_END  begins_and_ends[4];

	int i;
	for (i=0;i<4;i++)
	{
		if (!m_objs[i]) continue;	
		begins_and_ends[i].begin_of_curve = m_objs[i]->GetPointFromCoefficient(0.0);
		begins_and_ends[i].end_of_curve = m_objs[i]->GetPointFromCoefficient(1.0);
	}

	SG_POINT  curStart,curFinish;

	curStart = begins_and_ends[0].begin_of_curve;
	curFinish = begins_and_ends[0].end_of_curve;

	int sec_curve_index=-1;
	for (i=1;i<4;i++)
	{
		if (!m_objs[i]) continue;
		if (sgSpaceMath::PointsDistance(curFinish,begins_and_ends[i].begin_of_curve)<0.00001)
		{
			curFinish = begins_and_ends[i].end_of_curve;
			sec_curve_index = i;
			goto searchThird;
		}
		if (sgSpaceMath::PointsDistance(curFinish,begins_and_ends[i].end_of_curve)<0.00001)
		{
			curFinish = begins_and_ends[i].begin_of_curve;
			sec_curve_index = i;
			goto searchThird;
		}
	}

	return false;

searchThird:

	int third_curve_index=-1;

	for (i=1;i<4;i++)
	{
		if (!m_objs[i] || i==sec_curve_index) continue;
		if (sgSpaceMath::PointsDistance(curFinish,begins_and_ends[i].begin_of_curve)<0.00001)
		{
			curFinish = begins_and_ends[i].end_of_curve;
			third_curve_index = i;
			goto searchFourth;
		}
		if (sgSpaceMath::PointsDistance(curFinish,begins_and_ends[i].end_of_curve)<0.00001)
		{
			curFinish = begins_and_ends[i].begin_of_curve;
			third_curve_index = i;
			goto searchFourth;
		}
	}

	return false;

searchFourth:

	if (m_objs[3]==NULL)
	{
		if (sgSpaceMath::PointsDistance(curFinish,begins_and_ends[0].begin_of_curve)>0.00001)
			return false;
		return true;
	}

	for (i=1;i<4;i++)
	{
		if (!m_objs[i] || i==sec_curve_index || i==third_curve_index) continue;
		if (sgSpaceMath::PointsDistance(curFinish,begins_and_ends[i].begin_of_curve)<0.00001)
		{
			curFinish = begins_and_ends[i].end_of_curve;
			if (sgSpaceMath::PointsDistance(curFinish,begins_and_ends[0].begin_of_curve)>0.00001)
				return false;
			return true;
		}
		if (sgSpaceMath::PointsDistance(curFinish,begins_and_ends[i].end_of_curve)<0.00001)
		{
			curFinish = begins_and_ends[i].begin_of_curve;
			if (sgSpaceMath::PointsDistance(curFinish,begins_and_ends[0].begin_of_curve)>0.00001)
				return false;
			return true;
		}
	}
	return false;
}


void  Coons3Command::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	m_cur_obj=m_app->GetViewPort()->GetHotObject();
	if (m_cur_obj->IsSelect())
	{
		m_message.LoadString(IDS_CONT_ALREADY_SEL);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
		return;
	}
	m_cur_obj->Select(true);
	if (m_cur_obj)
	{
		m_objs[m_step++] = reinterpret_cast<sgC2DObject*>(m_cur_obj);
		if (m_step==1)
		{
			m_message.LoadString(IDS_COON_SEL_2_CONT);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		}
		if (m_step==2)
		{
			if (!CheckTwoObjects())
			{
				m_message.LoadString(IDS_COONS_ER_MUST_PRIMICAT);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				m_objs[1]->Select(false);
				m_step = 1;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_app->GetViewPort()->InvalidateViewPort();
				return;
			}
			m_message.LoadString(IDS_COON_SEL_3_CONT);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		}
		if (m_step==3)
		{
			if (CheckThreeObjects())
			{
				sgCObject* aaa = sgSurfaces::Coons((*m_objs[0]),
					(*m_objs[1]),
					(*m_objs[2]),NULL,
					36,36,6,6);
				if (!aaa)
				{
					m_message.LoadString(IDS_ERR_CANT_CREATE);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
					return;
				}

				CString nm;
				nm.LoadString(IDS_COONS_NAME);
				CString nmInd;
				nmInd.Format("%i",coons_name_index);
				nm+=nmInd;
				aaa->SetName(nm);

				aaa->SetUserGeometry("{F7325996-1640-4cf8-BA6A-F18415B512CB}",0,NULL);

				sgGetScene()->StartUndoGroup();
				sgGetScene()->AttachObject(aaa);
				m_app->ApplyAttributes(aaa);
				sgGetScene()->EndUndoGroup();
				coons_name_index++;

				m_message.LoadString(IDS_COON_SEL_1_CONT);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

				if (sgGetScene()->GetSelectedObjectsList()->GetCount()>0)
				{
					sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
					while (curObj) 
					{
						curObj->Select(false);
						curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
					}
				}
				
				m_step=0;
			}
			else
			{
				m_message.LoadString(IDS_COONS_ER_MUST_CLOSE_AREA);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				m_objs[2]->Select(false);
				m_step = 2;
			}
		}
		m_app->GetCommandPanel()->SetActiveRadio(m_step);
		m_app->GetViewPort()->InvalidateViewPort();
	}
	else
	{
		m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
		return;
	}
}


void  Coons3Command::Draw()
{
}


void  Coons3Command::OnEnter()
{
	SWITCH_RESOURCE
		if (m_cur_obj->IsSelect())
		{
			m_message.LoadString(IDS_CONT_ALREADY_SEL);
			m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
			return;
		}
	m_cur_obj=m_app->GetViewPort()->GetHotObject();
	m_cur_obj->Select(true);
	if (m_cur_obj)
	{
		m_objs[m_step++] = reinterpret_cast<sgC2DObject*>(m_cur_obj);
		if (m_step==1)
		{
			m_message.LoadString(IDS_COON_SEL_2_CONT);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		}
		if (m_step==2)
		{
			if (!CheckTwoObjects())
			{
				m_message.LoadString(IDS_COONS_ER_MUST_PRIMICAT);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				m_objs[1]->Select(false);
				m_step = 1;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_app->GetViewPort()->InvalidateViewPort();
				return;
			}
			m_message.LoadString(IDS_COON_SEL_3_CONT);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		}
		if (m_step==3)
		{
			if (CheckThreeObjects())
			{
				sgCObject* aaa = sgSurfaces::Coons((*m_objs[0]),
					(*m_objs[1]),
					(*m_objs[2]),NULL,
					36,36,6,6);
				if (!aaa)
				{
					m_message.LoadString(IDS_ERR_CANT_CREATE);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
					return;
				}

				CString nm;
				nm.LoadString(IDS_COONS_NAME);
				CString nmInd;
				nmInd.Format("%i",coons_name_index);
				nm+=nmInd;
				aaa->SetName(nm);

				aaa->SetUserGeometry("{F7325996-1640-4cf8-BA6A-F18415B512CB}",0,NULL);

				sgGetScene()->StartUndoGroup();
				sgGetScene()->AttachObject(aaa);
				m_app->ApplyAttributes(aaa);
				sgGetScene()->EndUndoGroup();
				coons_name_index++;

				m_message.LoadString(IDS_COON_SEL_1_CONT);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

				if (sgGetScene()->GetSelectedObjectsList()->GetCount()>0)
				{
					sgCObject*  curObj = sgGetScene()->GetSelectedObjectsList()->GetHead();
					while (curObj) 
					{
						curObj->Select(false);
						curObj = sgGetScene()->GetSelectedObjectsList()->GetNext(curObj);
					}
				}

				m_step=0;
			}
			else
			{
				m_message.LoadString(IDS_COONS_ER_MUST_CLOSE_AREA);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				m_objs[2]->Select(false);
				m_step = 2;
			}
		}
		m_app->GetCommandPanel()->SetActiveRadio(m_step);
		m_app->GetViewPort()->InvalidateViewPort();
	}
	else
	{
		m_message.LoadString(IDS_ERROR_OBJ_NOT_SEL);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
		return;
	}
}

unsigned int  Coons3Command::GetItemsCount()
{
	return 0;
}

void         Coons3Command::GetItem(unsigned int itemID, CString& itSrt)
{
	
}

void     Coons3Command::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	
}

HBITMAP   Coons3Command::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         Coons3Command::Run(unsigned int itemID)
{
	
}
