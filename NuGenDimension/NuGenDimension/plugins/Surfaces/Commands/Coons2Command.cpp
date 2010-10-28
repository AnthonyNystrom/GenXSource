#include "stdafx.h"

#include "Coons2Command.h"

#include "..//resource.h"

int     coons_name_index = 1;

Coons2Command::Coons2Command(IApplicationInterface* appI):
						m_app(appI)
{
	ASSERT(m_app);
	m_step = 0;
	m_cur_obj = NULL;

	m_get_object_panels[0] = 
		m_get_object_panels[1] = NULL;

	m_objs[0] = m_objs[1] = m_objs[2] = m_objs[3] = NULL;
}

Coons2Command::~Coons2Command()
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


bool    Coons2Command::PreTranslateMessage(MSG* pMsg)
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
			if(m_step>=0 && m_step<2) 
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

void     Coons2Command::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, 
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
		ASSERT(m_step==0 || m_step==1);
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


void  Coons2Command::Start()	
{
	SWITCH_RESOURCE

	m_app->GetCommandPanel()->RemoveAllDialogs();

	m_message.LoadString(IDS_COONS_NAME);
	
	m_app->StartCommander(m_message);

	m_message.LoadString(IDS_CONT);
	m_get_object_panels[0] = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,m_message,true));
	
	m_get_object_panels[1] = reinterpret_cast<IGetObjectsPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,m_message,true));

	for (int i=0;i<2;i++)
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

void  Coons2Command::MouseMove(unsigned int nFlags,int pX,int pY)
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

bool Coons2Command::SetFourthAndFivethByFirstAndSecond()
{
	m_objs[2] = m_objs[3] = NULL;
	if (m_objs[0]==NULL || m_objs[1]==NULL)
		return false;

	SG_POINT begFir, endFir, begSec, endSec;
	begFir = m_objs[0]->GetPointFromCoefficient(0.0);
	endFir = m_objs[0]->GetPointFromCoefficient(1.0);
	begSec = m_objs[1]->GetPointFromCoefficient(0.0);
	endSec = m_objs[1]->GetPointFromCoefficient(1.0);

	SG_VECTOR transVec;
	if (sgSpaceMath::PointsDistance(begFir,begSec)<0.00001)
	{
		m_objs[2] = (sgC2DObject*)m_objs[1]->Clone();
		transVec.x = endFir.x - begFir.x;
		transVec.y = endFir.y - begFir.y;
		transVec.z = endFir.z - begFir.z;
		m_objs[2]->InitTempMatrix()->Translate(transVec);
		m_objs[2]->ApplyTempMatrix();
		m_objs[2]->DestroyTempMatrix();

		m_objs[3] = (sgC2DObject*)m_objs[0]->Clone();
		transVec.x = endSec.x - begSec.x;
		transVec.y = endSec.y - begSec.y;
		transVec.z = endSec.z - begSec.z;
		m_objs[3]->InitTempMatrix()->Translate(transVec);
		m_objs[3]->ApplyTempMatrix();
		m_objs[3]->DestroyTempMatrix();
		return true;
	}
	else
		if (sgSpaceMath::PointsDistance(begFir,endSec)<0.00001)
		{
			m_objs[2] = (sgC2DObject*)m_objs[1]->Clone();
			transVec.x = endFir.x - begFir.x;
			transVec.y = endFir.y - begFir.y;
			transVec.z = endFir.z - begFir.z;
			m_objs[2]->InitTempMatrix()->Translate(transVec);
			m_objs[2]->ApplyTempMatrix();
			m_objs[2]->DestroyTempMatrix();

			m_objs[3] = (sgC2DObject*)m_objs[0]->Clone();
			transVec.x = begSec.x - endSec.x;
			transVec.y = begSec.y - endSec.y;
			transVec.z = begSec.z - endSec.z;
			m_objs[3]->InitTempMatrix()->Translate(transVec);
			m_objs[3]->ApplyTempMatrix();
			m_objs[3]->DestroyTempMatrix();
			return true;
		}
		else
			if (sgSpaceMath::PointsDistance(endFir,begSec)<0.00001)
			{
				m_objs[2] = (sgC2DObject*)m_objs[1]->Clone();
				transVec.x = begFir.x - endFir.x;
				transVec.y = begFir.y - endFir.y;
				transVec.z = begFir.z - endFir.z;
				m_objs[2]->InitTempMatrix()->Translate(transVec);
				m_objs[2]->ApplyTempMatrix();
				m_objs[2]->DestroyTempMatrix();

				m_objs[3] = (sgC2DObject*)m_objs[0]->Clone();
				transVec.x = endSec.x - begSec.x;
				transVec.y = endSec.y - begSec.y;
				transVec.z = endSec.z - begSec.z;
				m_objs[3]->InitTempMatrix()->Translate(transVec);
				m_objs[3]->ApplyTempMatrix();
				m_objs[3]->DestroyTempMatrix();
				return true;
			}
			else
				if (sgSpaceMath::PointsDistance(endFir,endSec)<0.00001)
				{
					m_objs[2] = (sgC2DObject*)m_objs[1]->Clone();
					transVec.x = begFir.x-endFir.x;
					transVec.y = begFir.y-endFir.y;
					transVec.z = begFir.z-endFir.z;
					m_objs[2]->InitTempMatrix()->Translate(transVec);
					m_objs[2]->ApplyTempMatrix();
					m_objs[2]->DestroyTempMatrix();

					m_objs[3] = (sgC2DObject*)m_objs[0]->Clone();
					transVec.x = begSec.x - endSec.x;
					transVec.y = begSec.y - endSec.y;
					transVec.z = begSec.z - endSec.z;
					m_objs[3]->InitTempMatrix()->Translate(transVec);
					m_objs[3]->ApplyTempMatrix();
					m_objs[3]->DestroyTempMatrix();
					return true;
				}
				else
				{
					return false;
				}

}

void  Coons2Command::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	m_cur_obj=m_app->GetViewPort()->GetHotObject();
		if (m_cur_obj)
		{
			if (m_app->GetViewPort()->GetHotObject()->IsSelect())
			{
				m_message.LoadString(IDS_CONT_ALREADY_SEL);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				return;
			}
			m_cur_obj->Select(true);
			m_objs[m_step++] = reinterpret_cast<sgC2DObject*>(m_cur_obj);
			if (m_step==1)
			{
				m_message.LoadString(IDS_COON_SEL_2_CONT);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
			}
			if (m_step==2)
			{
				if (SetFourthAndFivethByFirstAndSecond())
				{
					sgCObject* aaa = sgSurfaces::Coons((*m_objs[0]),
						(*m_objs[1]),
						(*m_objs[2]),
						(m_objs[3]),36,36,6,6);
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
					sgDeleteObject(m_objs[2]);
					sgDeleteObject(m_objs[3]);
					m_step=0;
				}
				else
				{
					m_message.LoadString(IDS_COONS_ER_MUST_PRIMICAT);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
					m_objs[1]->Select(false);
					m_step = 1;
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


void  Coons2Command::Draw()
{
}


void  Coons2Command::OnEnter()
{
	SWITCH_RESOURCE
	m_cur_obj=m_app->GetViewPort()->GetHotObject();
	if (m_cur_obj)
	{
		if (m_app->GetViewPort()->GetHotObject()->IsSelect())
		{
			m_message.LoadString(IDS_CONT_ALREADY_SEL);
			m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
			return;
		}
		m_cur_obj->Select(true);
		m_objs[m_step++] = reinterpret_cast<sgC2DObject*>(m_cur_obj);
		if (m_step==1)
		{
			m_message.LoadString(IDS_COON_SEL_2_CONT);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		}
		if (m_step==2)
		{
			if (SetFourthAndFivethByFirstAndSecond())
			{
				sgCObject* aaa = sgSurfaces::Coons((*m_objs[0]),
					(*m_objs[1]),
					(*m_objs[2]),
					(m_objs[3]),36,36,6,6);
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
				sgDeleteObject(m_objs[2]);
				sgDeleteObject(m_objs[3]);
				m_step=0;
			}
			else
			{
				m_message.LoadString(IDS_COONS_ER_MUST_PRIMICAT);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,m_message);
				m_objs[1]->Select(false);
				m_step = 1;
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

unsigned int  Coons2Command::GetItemsCount()
{
	return 0;
}

void         Coons2Command::GetItem(unsigned int itemID, CString& itSrt)
{
	
}

void     Coons2Command::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	
}

HBITMAP   Coons2Command::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         Coons2Command::Run(unsigned int itemID)
{
	
}
