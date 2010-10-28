#include "stdafx.h"

#include "Contour.h"

#include "..//Dialogs//ContScenarDlg.h"
#include "..//Dialogs//ContObjDlg.h"

#include "..//resource.h"

#include <math.h>

int     contour_name_index = 1;

Contour::Contour(IApplicationInterface* appI):
				m_app(appI)
				, m_start_object(NULL)
				, m_scenar(-1)
				, m_step(0)
				, m_lines(false)
				, m_scenar_panel(NULL)
				, m_get_object_panel(NULL)
				, m_select_obj_type_panel(NULL)
				, m_get_first_point(NULL)
				, m_get_second_point(NULL)
				, m_get_third_point(NULL)
				, m_isFirstPoint(true)
				, m_isSecondPoint(false)
				, m_isLastPointOnArc(false)
				, m_exist_arc_data(false)
				, m_can_close(false)

{
	ASSERT(m_app);
}

Contour::~Contour()
{
	size_t sz =m_objects.size();
	for (size_t i=0;i<sz;i++)
		sgCObject::DeleteObject(m_objects[i]);
	m_objects.clear();

	m_app->GetCommandPanel()->RemoveAllDialogs();
	if (m_scenar_panel)
	{
		m_scenar_panel->DestroyWindow();
		delete m_scenar_panel;
		m_scenar_panel = NULL;
	}
	if (m_select_obj_type_panel)
	{
		m_select_obj_type_panel->DestroyWindow();
		delete m_select_obj_type_panel;
		m_select_obj_type_panel = NULL;
	}
	m_app->GetViewPort()->InvalidateViewPort();
}


bool    Contour::PreTranslateMessage(MSG* pMsg)
{
	try {  //#try
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
			switch(m_step) 
			{
			case 0:
				if (m_get_first_point)
					m_get_first_point->GetWindow()->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
				break;
			case 1:
				if (m_get_second_point)
					m_get_second_point->GetWindow()->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
				break;
			case 2:
				if (m_get_third_point)
					m_get_third_point->GetWindow()->SendMessage(pMsg->message,
					pMsg->wParam,
					pMsg->lParam);
				break;
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

static   bool isObjAddToList(sgCObject* o)
{
	if (o->GetType()==SG_OT_LINE ||
		o->GetType()==SG_OT_ARC)
		return true;
	if (o->GetType()==SG_OT_CONTOUR)
	{
		sgCContour* cc = reinterpret_cast<sgCContour*>(o);
		if (cc->IsClosed())
			return false;
		else
			return true;
	}
	return false;
}

void  Contour::SwitchScenario(int newScen)
{
	SWITCH_RESOURCE
	switch(newScen) {
	case 1:
		m_lines = false;
		if (m_scenar==0)
			break;
		m_scenar = 0;
		if (m_select_obj_type_panel!=NULL)
		{
			m_app->GetCommandPanel()->RemoveDialog(m_select_obj_type_panel);
			m_select_obj_type_panel->DestroyWindow();
			delete m_select_obj_type_panel;
			m_select_obj_type_panel = NULL;
		}
		if (m_get_first_point)
		{
			m_app->GetCommandPanel()->RemoveDialog(m_get_first_point);
			m_get_first_point = NULL;
		}
		if (m_get_second_point)
		{
			m_app->GetCommandPanel()->RemoveDialog(m_get_second_point);
			m_get_second_point = NULL;
		}
		if (m_get_third_point)
		{
			m_app->GetCommandPanel()->RemoveDialog(m_get_third_point);
			m_get_third_point = NULL;
		}
		if (m_get_object_panel==NULL)
		{
			CString lab;
			lab.LoadString(IDS_OBJECT);
			m_get_object_panel = 
				reinterpret_cast<IGetObjectsPanel*>(m_app->GetCommandPanel()->
				AddDialog(IBaseInterfaceOfGetDialogs::GET_OBJECTS_DLG,
				lab,false));
			m_get_object_panel->EnableControls(true);
			m_get_object_panel->SetMultiselectMode(false);
			m_get_object_panel->FillList(isObjAddToList);
			m_app->GetCommandPanel()->EnableRadio(0,true);
			m_app->GetCommandPanel()->EnableRadio(1,true);
		}
		m_message.LoadString(IDS_CHOISE_FIRST_OBJ_OF_PATH);
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		{
			size_t sz =m_objects.size();
			for (size_t i=0;i<sz;i++)
				sgCObject::DeleteObject(m_objects[i]);
		}
		break;
	case 0:
		if (m_scenar==1)
			break;
		m_scenar = 1;
		if (m_get_object_panel!=NULL)
		{
			m_app->GetCommandPanel()->RemoveDialog(m_get_object_panel);
			m_get_object_panel = NULL;
		}
		if (m_select_obj_type_panel==NULL)
		{	
			m_select_obj_type_panel = new CContObjDlg;
			m_select_obj_type_panel->Create(IDD_CONT_OBJ_TYPE,
				m_app->GetCommandPanel()->GetDialogsContainerWindow());
			m_select_obj_type_panel->SetCommander(this);
			m_message.LoadString(IDS_CONT_OBJ_TYPE);
			m_app->GetCommandPanel()->AddDialog(m_select_obj_type_panel,m_message,false);

			m_select_obj_type_panel->EnableControls(true);
			m_app->GetCommandPanel()->EnableRadio(1,true);

			m_select_obj_type_panel->OnBnClickedContObjRadio1();
		}
		m_message.LoadString(IDS_ENTER_POINT_COORDS);
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		break;
	default:
		ASSERT(0);
		break;
	}
}

void Contour::SwitchObjectType(int newObjType)
{
	SWITCH_RESOURCE
	switch(newObjType) 
	{
	case 0:
		if (m_lines)
			break;
		if (m_step==1)
		{
			if (m_get_third_point)
			{
				m_app->GetCommandPanel()->RemoveDialog(m_get_third_point);
				m_get_third_point = NULL;
			}
			m_lines = true;
			break;
		}
		m_lines = true;
		if (m_get_first_point)
		{
			m_app->GetCommandPanel()->RemoveDialog(m_get_first_point);
			m_get_first_point = NULL;
		}
		if (m_get_second_point)
		{
			m_app->GetCommandPanel()->RemoveDialog(m_get_second_point);
			m_get_second_point = NULL;
		}
		if (m_get_third_point)
		{
			m_app->GetCommandPanel()->RemoveDialog(m_get_third_point);
			m_get_third_point = NULL;
		}
		if (m_get_first_point==NULL)
		{
			m_message.LoadString(IDS_FIRST_POINT);
			m_get_first_point = 
				reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
				AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
				m_message,true));
		}
		if (m_get_second_point==NULL)
		{
			m_message.LoadString(IDS_SECOND_POINT);
			m_get_second_point = 
				reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
				AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
				m_message,true));
		}
		m_app->GetCommandPanel()->SetActiveRadio(2);
		break;
	case 1:
		if (!m_lines)
			break;
		if (m_step==1)
		{
			if (m_get_third_point==NULL)
			{
				m_message.LoadString(IDS_POINT_ON_ARC);
				m_get_third_point = 
					reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
					AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
					m_message,true));
			}
			m_lines = false;
			break;
		}
		m_lines = false;
		if (m_get_first_point)
		{
			m_app->GetCommandPanel()->RemoveDialog(m_get_first_point);
			m_get_first_point = NULL;
		}
		if (m_get_second_point)
		{
			m_app->GetCommandPanel()->RemoveDialog(m_get_second_point);
			m_get_second_point = NULL;
		}
		if (m_get_third_point)
		{
			m_app->GetCommandPanel()->RemoveDialog(m_get_third_point);
			m_get_third_point = NULL;
		}
		if (m_get_first_point==NULL)
		{
			m_message.LoadString(IDS_BEG_POINT);
			m_get_first_point = 
				reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
				AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
				m_message,true));
		}
		if (m_get_second_point==NULL)
		{
			m_message.LoadString(IDS_END_POINT);
			m_get_second_point = 
				reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
				AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
				m_message,true));
		}
		if (m_get_third_point==NULL)
		{
			m_message.LoadString(IDS_POINT_ON_ARC);
			m_get_third_point = 
				reinterpret_cast<IGetPointPanel*>(m_app->GetCommandPanel()->
				AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,
				m_message,true));
		}
		m_app->GetCommandPanel()->SetActiveRadio(2);
		break;
	default:
		ASSERT(0);
		break;
	}
}

void  Contour::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, void* params)
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=1);
		m_step = (unsigned int)newActiveDlg;
		/*if (newActiveDlg==0)
		{
			m_app->GetCommandPanel()->EnableRadio(1,false);
			m_message.LoadString(IDS_CHOISE_TRANS_OBJ);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);
		}*/
		//m_app->GetViewPort()->InvalidateViewPort();
		return;
	}
	if (mes==ICommander::CM_SELECT_OBJECT)
	{
		ASSERT(params!=NULL);
		ASSERT(m_scenar==0);
		sgCObject* so = (sgCObject*)params;
		if (so!=m_start_object)
		{
			m_app->GetViewPort()->SetHotObject(so);
			m_start_object = so;
		}
		m_app->GetViewPort()->InvalidateViewPort();
	}
}


void  Contour::Start()	
{
	SWITCH_RESOURCE
	m_app->GetCommandPanel()->RemoveAllDialogs();

	CString lab;
	lab.LoadString(IDS_TOOLTIP_FIVETH);
	m_app->StartCommander(lab);

	m_scenar_panel = new CContScenarDlg;
	m_scenar_panel->Create(IDD_CONT_SCENARS_DLG,
		m_app->GetCommandPanel()->GetDialogsContainerWindow());
	m_scenar_panel->SetCommander(this);

	m_message.LoadString(IDS_CONT_SCENAR);
	m_app->GetCommandPanel()->AddDialog(m_scenar_panel,m_message,false);

	m_app->GetCommandPanel()->EnableRadio(0,true);

}

static  void get_end_points_of_object(sgCObject* ob, SG_POINT& bP, SG_POINT& eP)
{
	if (ob->GetType()==SG_OT_ARC)
	{
		sgCArc* arcO = reinterpret_cast<sgCArc*>(ob);
		const SG_ARC* arcG = arcO->GetGeometry();
		bP = arcG->begin;
		eP = arcG->end;
	}
	else
		if (ob->GetType()==SG_OT_LINE)
		{
			sgCLine* lineO = reinterpret_cast<sgCLine*>(ob);
			const SG_LINE* lineG = lineO->GetGeometry();
			bP = lineG->p1;
			eP = lineG->p2;
		}
		else
			if (ob->GetType()==SG_OT_CONTOUR)
			{
				sgCContour* cO = reinterpret_cast<sgCContour*>(ob);
				//cO->GetEndPoints(bP,eP);
				bP = cO->GetPointFromCoefficient(0.0);
				eP = cO->GetPointFromCoefficient(1.0);
			}
			else
			{
				ASSERT(0);
			}
}

void  Contour::CreateContour(sgCObject* stObj)
{
	std::vector<sgCObject*>   ArcAndLines;
	std::vector<sgCObject*>   goodOnPath;

	goodOnPath.push_back(stObj);

	SG_POINT  zeroBegP;
	SG_POINT  zeroEndP;

	get_end_points_of_object(stObj, zeroBegP, zeroEndP);

	sgCObject*  curObj = sgGetScene()->GetObjectsList()->GetHead();
	while (curObj) 
	{
		if (curObj->GetType()==SG_OT_ARC ||
			curObj->GetType()==SG_OT_LINE )
		{
				if (curObj!=stObj)
					ArcAndLines.push_back(curObj);
		}
		else
		{
			if (curObj->GetType()==SG_OT_CONTOUR)
			{
				sgCContour* ccont = reinterpret_cast<sgCContour*>(curObj);
				if (!ccont->IsClosed())
				{
					if (curObj!=stObj)
						ArcAndLines.push_back(curObj);
				}
			}
		}
		curObj = sgGetScene()->GetObjectsList()->GetNext(curObj);
	}

#define MIN_DIST   0.0000001

	for (size_t i=0; i<ArcAndLines.size(); i++)
	{
		if (sgSpaceMath::PointsDistance(zeroEndP,zeroBegP)<MIN_DIST)
			break;
		if (ArcAndLines[i]==NULL)
			continue;

		SG_POINT begP;
		SG_POINT endP;

		get_end_points_of_object(ArcAndLines[i], begP, endP);

		bool  yes = false;

		if (sgSpaceMath::PointsDistance(begP,zeroBegP)<MIN_DIST)
		{
			zeroBegP = endP;
			yes = true;
			goto metlab;
		}
		if (sgSpaceMath::PointsDistance(endP,zeroBegP)<MIN_DIST)
		{
			zeroBegP = begP;
			yes = true;
			goto metlab;
		}

		if (sgSpaceMath::PointsDistance(begP,zeroEndP)<MIN_DIST)
		{
			zeroEndP = endP;
			yes = true;
			goto metlab;
		}
		if (sgSpaceMath::PointsDistance(endP,zeroEndP)<MIN_DIST)
		{
			zeroEndP = begP;
			yes = true;
			goto metlab;
		}
metlab:
		if (yes)
		{
			goodOnPath.push_back(ArcAndLines[i]);
			//ArcAndLines.erase(&ArcAndLines[i]);
			ArcAndLines[i] = NULL;
			i = -1;
			continue;
		}
		yes = false;
	}

	if (goodOnPath.size()>1)
	{
		sgGetScene()->StartUndoGroup();
		sgCContour* cont = sgCContour::CreateContour(&goodOnPath[0],goodOnPath.size());
		if (!cont)
			return;
		SWITCH_RESOURCE
		m_message.LoadString(IDS_TOOLTIP_FIVETH);
		CString nmInd;
		nmInd.Format("%i",contour_name_index);
		m_message+=nmInd;
		cont->SetName(m_message);
		contour_name_index++;
		sgGetScene()->AttachObject(cont);
		sgGetScene()->EndUndoGroup();
	}
	else
	{
		if (goodOnPath.size()==0)
		{
			m_message.LoadString(IDS_ERR_PATH_NO_OBJ);
			m_app->PutMessage(IApplicationInterface::MT_ERROR,
				m_message);
			return;
		}
		if (goodOnPath.size()==1)
		{
			m_message.LoadString(IDS_ERR_PATH_ONE_OBJ);
			m_app->PutMessage(IApplicationInterface::MT_ERROR,
				m_message);
			return;
		}
	}

}

void  Contour::CreateContourFromObjects()
{
	size_t sz = m_objects.size();
	if (sz==0)
	{
		m_message.LoadString(IDS_ERR_PATH_NO_OBJ);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,
			m_message);
		return;
	}
	if (sz==1)
	{
		m_message.LoadString(IDS_ERR_PATH_ONE_OBJ);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,
			m_message);
		return;
	}
	if (sz>1)
	{
		sgGetScene()->StartUndoGroup();
		sgCContour* cont = sgCContour::CreateContour(&m_objects[0],sz);
		if (!cont)
			return;
		SWITCH_RESOURCE
		m_message.LoadString(IDS_TOOLTIP_FIVETH);
		CString nmInd;
		nmInd.Format("%i",contour_name_index);
		m_message+=nmInd;
		cont->SetName(m_message);
		contour_name_index++;
		sgGetScene()->AttachObject(cont);
		sgGetScene()->EndUndoGroup();
		m_objects.clear();
		m_isFirstPoint = true;
		m_isSecondPoint = false;
		m_isLastPointOnArc = false;
		m_exist_arc_data = false;
		m_can_close =false;
		m_message.LoadString(IDS_FIRST_POINT);
		m_app->GetCommandPanel()->RenameRadio(2,m_message);
		m_app->GetCommandPanel()->SetActiveRadio(2);
		m_app->GetCommandPanel()->EnableRadio(3,false);
		m_step = 0;
	}
}

void  Contour::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (m_scenar==0)
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
		return;
	}
	if (m_scenar==1)
	{
		IViewPort::GET_SNAP_IN in_arg;
		in_arg.scrX = pX;
		in_arg.scrY = pY;
		in_arg.snapType = SNAP_SYSTEM;
		switch(m_step) 
		{
		case 0:
			if (m_get_first_point)
			{
				in_arg.XFix = m_get_first_point->IsXFixed();
				in_arg.YFix = m_get_first_point->IsYFixed();
				in_arg.ZFix = m_get_first_point->IsZFixed();
				m_get_first_point->GetPoint(in_arg.FixPoint.x,in_arg.FixPoint.y,in_arg.FixPoint.z);
				IViewPort::GET_SNAP_OUT out_arg;
				m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
				m_cur_point = out_arg.result_point;
				m_get_first_point->SetPoint((float)(m_cur_point.x),(float)(m_cur_point.y),(float)(m_cur_point.z));
			}
			break;
		case 1:
			if (m_get_second_point)
			{
				in_arg.XFix = m_get_second_point->IsXFixed();
				in_arg.YFix = m_get_second_point->IsYFixed();
				in_arg.ZFix = m_get_second_point->IsZFixed();
				m_get_second_point->GetPoint(in_arg.FixPoint.x,in_arg.FixPoint.y,in_arg.FixPoint.z);
				IViewPort::GET_SNAP_OUT out_arg;
				m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
				m_cur_point = out_arg.result_point;
				m_get_second_point->SetPoint((float)(m_cur_point.x),(float)(m_cur_point.y),(float)(m_cur_point.z));
			}
			break;
		case 2:
			if (m_lines)
			{
				ASSERT(0);
				return;
			}
			if (m_get_third_point)
			{
				in_arg.XFix = m_get_third_point->IsXFixed();
				in_arg.YFix = m_get_third_point->IsYFixed();
				in_arg.ZFix = m_get_third_point->IsZFixed();
				m_get_third_point->GetPoint(in_arg.FixPoint.x,in_arg.FixPoint.y,in_arg.FixPoint.z);
				IViewPort::GET_SNAP_OUT out_arg;
				m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
				m_cur_point = out_arg.result_point;
				m_get_third_point->SetPoint((float)(m_cur_point.x),(float)(m_cur_point.y),(float)(m_cur_point.z));
			}
			if (!m_lines)
			{
				m_exist_arc_data = m_arc_geo.FromThreePoints(m_tmp_first_point,m_tmp_second_point,
					m_cur_point,false);
			}
			break;
		default:
			ASSERT(0);
			return;
		}
		m_app->GetViewPort()->InvalidateViewPort();
	}
}

void  Contour::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	if (m_scenar==0)
	{
		m_start_object = m_app->GetViewPort()->GetHotObject();

		if (m_start_object==NULL)
		{
			m_message.LoadString(IDS_ERR_PATH_NO_OBJ);
			m_app->PutMessage(IApplicationInterface::MT_ERROR,
				m_message);
			return;
		}

		CreateContour(m_start_object);

		m_get_object_panel->RemoveAllObjects();
		m_get_object_panel->FillList(isObjAddToList);

		return;
	}
	if (m_scenar==1)
	{
		if (m_isFirstPoint)
		{
			m_tmp_first_point = m_cur_point;
			m_isFirstPoint = false;
			m_isSecondPoint = true;
			if (m_objects.size()==0)
				m_first_point = m_cur_point;
			m_step=1;
			m_app->GetCommandPanel()->SetActiveRadio(m_step+2);
			m_app->GetCommandPanel()->EnableRadio(m_step+1,false);
			m_message.LoadString(IDS_PREV);
			m_app->GetCommandPanel()->RenameRadio(m_step+1,m_message);
		}
		else
			if (m_isSecondPoint)
			{
				if (sgSpaceMath::PointsDistance(m_tmp_first_point,m_cur_point)<0.00001)
				{
					m_message.LoadString(IDS_ERR_PNT_AS_PREV);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				if (m_lines)
				{
					if (m_get_first_point)
						m_get_first_point->SetPoint(m_cur_point.x,
													m_cur_point.y,
													m_cur_point.z);
					sgCLine* ll = sgCreateLine(m_tmp_first_point.x,
						m_tmp_first_point.y,
						m_tmp_first_point.z,
						m_cur_point.x,
						m_cur_point.y,
						m_cur_point.z);
					m_objects.push_back(ll);
					m_tmp_first_point = m_cur_point;
					m_isFirstPoint = false;
					m_isSecondPoint = true;
				}
				else
				{
					m_tmp_second_point = m_cur_point;
					m_isFirstPoint = false;
					m_isSecondPoint = false;
					m_isLastPointOnArc = true;
					m_step=2;
					m_app->GetCommandPanel()->SetActiveRadio(m_step+2);
					m_app->GetCommandPanel()->EnableRadio(m_step+1,false);
					if (m_select_obj_type_panel)
						m_select_obj_type_panel->EnableLineType(false);
				}

				int snSz = m_app->GetViewPort()->GetSnapSize();
				double  coords[3];
				m_app->GetViewPort()->ProjectWorldPoint(m_first_point,coords[0],coords[1],coords[2]);
				if (sqrt((coords[0]-pX)*(coords[0]-pX)+
					(coords[1]-pY)*(coords[1]-pY))<=snSz ||
					sgSpaceMath::PointsDistance(m_first_point,m_cur_point)<0.00001)
					m_can_close = true;
				if (m_lines && m_can_close)
				{
					m_message.LoadString(IDS_CLOSE_CONTOUR);
					if (AfxMessageBox(m_message,MB_YESNO)==IDYES)
					{
						sgCObject* lastO = m_objects[m_objects.size()-1];
						ASSERT(lastO->GetType()==SG_OT_LINE);
						sgCLine* lastL = reinterpret_cast<sgCLine*>(lastO);
						m_tmp_first_point = lastL->GetGeometry()->p1;
						sgCObject::DeleteObject(lastO);
						m_objects.pop_back();
						sgCLine* ll = sgCreateLine(m_tmp_first_point.x,
							m_tmp_first_point.y,
							m_tmp_first_point.z,
							m_first_point.x,
							m_first_point.y,
							m_first_point.z);
						m_objects.push_back(ll);
						CreateContourFromObjects();
					}
					else
						m_can_close = false;
				}
			}
			else
				if (m_isLastPointOnArc)
				{
					if (sgSpaceMath::IsPointsOnOneLine(m_tmp_first_point,m_tmp_second_point,m_cur_point))
					{
						m_message.LoadString(IDS_ARC_ERR_ON_LINE);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
						return;
					}
					ASSERT(!m_lines);
					ASSERT(m_exist_arc_data);
					sgCArc* aa = sgCreateArc(m_arc_geo);
					m_objects.push_back(aa);
					m_tmp_first_point = m_tmp_second_point;
					m_isFirstPoint = false;
					m_isSecondPoint = true;
					m_isLastPointOnArc = false;
					m_exist_arc_data = false;
					m_step =1;
					m_app->GetCommandPanel()->SetActiveRadio(m_step+2);
					m_app->GetCommandPanel()->EnableRadio(m_step+3,false);
					if (m_get_first_point)
						m_get_first_point->SetPoint(m_tmp_first_point.x,
													m_tmp_first_point.y,
													m_tmp_first_point.z);
					if (m_select_obj_type_panel)
						m_select_obj_type_panel->EnableLineType(true);
					if (m_can_close)
					{
						m_message.LoadString(IDS_CLOSE_CONTOUR);
						if (AfxMessageBox(m_message,MB_YESNO)==IDYES)
						{
							sgCObject* lastO = m_objects[m_objects.size()-1];
							ASSERT(lastO->GetType()==SG_OT_ARC);
							sgCArc* lastA = reinterpret_cast<sgCArc*>(lastO);
							m_tmp_first_point = lastA->GetGeometry()->begin;
							sgCObject::DeleteObject(lastO);
							m_objects.pop_back();
							m_arc_geo.FromThreePoints(m_tmp_first_point,m_first_point,
								m_cur_point,false);
							sgCArc* aa = sgCreateArc(m_arc_geo);
							m_objects.push_back(aa);
							CreateContourFromObjects();
						}
						else
							m_can_close = false;
					}
				}
				else
				{
					ASSERT(0);
				}
	}

    	
}

void  Contour::Draw()
{
	if (m_scenar==1)
	{
		float pC[3];
		m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_point);
		m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
		m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
		size_t sz = m_objects.size();
		for(size_t i=0;i<sz;i++)
		{
			if (m_objects[i]->GetType()==SG_OT_LINE)
			{
				sgCLine* tmpl = reinterpret_cast<sgCLine*>(m_objects[i]);
				m_app->GetViewPort()->GetPainter()->DrawLine(*tmpl->GetGeometry());
				continue;
			}
			if (m_objects[i]->GetType()==SG_OT_ARC)
			{
				sgCArc* tmpa = reinterpret_cast<sgCArc*>(m_objects[i]);
				m_app->GetViewPort()->GetPainter()->DrawArc(*tmpa->GetGeometry());
				continue;
			}
		}
		if (m_exist_arc_data)
		{
			m_app->GetViewPort()->GetPainter()->DrawArc(m_arc_geo);
		}
		if (m_isSecondPoint)
		{
			SG_LINE lll;
			lll.p1 = m_tmp_first_point;
			lll.p2 = m_cur_point;
			m_app->GetViewPort()->GetPainter()->DrawLine(lll);
		}
	}
}

void  Contour::OnEnter()
{
	SWITCH_RESOURCE
		if (m_scenar==0)
		{
			m_start_object = m_app->GetViewPort()->GetHotObject();

			if (m_start_object==NULL)
			{
				m_message.LoadString(IDS_ERR_PATH_NO_OBJ);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}

			CreateContour(m_start_object);

			m_get_object_panel->RemoveAllObjects();
			m_get_object_panel->FillList(isObjAddToList);

			return;
		}
		if (m_scenar==1)
		{
			switch(m_step) 
			{
			case 0:
				if (m_get_first_point)
					m_get_first_point->GetPoint(m_cur_point.x,
												m_cur_point.y,
												m_cur_point.z);
				break;
			case 1:
				if (m_get_second_point)
					m_get_second_point->GetPoint(m_cur_point.x,
												m_cur_point.y,
												m_cur_point.z);
				break;
			case 2:
				if (m_get_third_point)
					m_get_third_point->GetPoint(m_cur_point.x,
												m_cur_point.y,
												m_cur_point.z);
				break;
			}
			
			if (m_isFirstPoint)
			{
				m_tmp_first_point = m_cur_point;
				m_isFirstPoint = false;
				m_isSecondPoint = true;
				if (m_objects.size()==0)
					m_first_point = m_cur_point;
				m_step=1;
				m_app->GetCommandPanel()->SetActiveRadio(m_step+2);
				m_app->GetCommandPanel()->EnableRadio(m_step+1,false);
				m_message.LoadString(IDS_PREV);
				m_app->GetCommandPanel()->RenameRadio(m_step+1,m_message);
			}
			else
				if (m_isSecondPoint)
				{
					if (sgSpaceMath::PointsDistance(m_tmp_first_point,m_cur_point)<0.00001)
					{
						m_message.LoadString(IDS_ERR_PNT_AS_PREV);
						m_app->PutMessage(IApplicationInterface::MT_ERROR,
							m_message);
						goto unfix;
					}
					if (m_lines)
					{
						if (m_get_first_point)
							m_get_first_point->SetPoint(m_cur_point.x,
							m_cur_point.y,
							m_cur_point.z);
						sgCLine* ll = sgCreateLine(m_tmp_first_point.x,
							m_tmp_first_point.y,
							m_tmp_first_point.z,
							m_cur_point.x,
							m_cur_point.y,
							m_cur_point.z);
						m_objects.push_back(ll);
						m_tmp_first_point = m_cur_point;
						m_isFirstPoint = false;
						m_isSecondPoint = true;
					}
					else
					{
						m_tmp_second_point = m_cur_point;
						m_isFirstPoint = false;
						m_isSecondPoint = false;
						m_isLastPointOnArc = true;
						m_step=2;
						m_app->GetCommandPanel()->SetActiveRadio(m_step+2);
						m_app->GetCommandPanel()->EnableRadio(m_step+1,false);
						if (m_select_obj_type_panel)
							m_select_obj_type_panel->EnableLineType(false);
					}

					if (sgSpaceMath::PointsDistance(m_first_point,m_cur_point)<0.00001)
						m_can_close = true;
					if (m_lines && m_can_close)
					{
						m_message.LoadString(IDS_CLOSE_CONTOUR);
						if (AfxMessageBox(m_message,MB_YESNO)==IDYES)
						{
							sgCObject* lastO = m_objects[m_objects.size()-1];
							ASSERT(lastO->GetType()==SG_OT_LINE);
							sgCLine* lastL = reinterpret_cast<sgCLine*>(lastO);
							m_tmp_first_point = lastL->GetGeometry()->p1;
							sgCObject::DeleteObject(lastO);
							m_objects.pop_back();
							sgCLine* ll = sgCreateLine(m_tmp_first_point.x,
								m_tmp_first_point.y,
								m_tmp_first_point.z,
								m_first_point.x,
								m_first_point.y,
								m_first_point.z);
							m_objects.push_back(ll);
							CreateContourFromObjects();
						}
						else
							m_can_close = false;
					}
				}
				else
					if (m_isLastPointOnArc)
					{
						if (sgSpaceMath::IsPointsOnOneLine(m_tmp_first_point,m_tmp_second_point,m_cur_point))
						{
							m_message.LoadString(IDS_ARC_ERR_ON_LINE);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,
								m_message);
							goto unfix;
						}
						ASSERT(!m_lines);
						if (m_arc_geo.FromThreePoints(m_tmp_first_point,
														m_tmp_second_point,
														m_cur_point,
														false))
						{
										sgCArc* aa = sgCreateArc(m_arc_geo);
										m_objects.push_back(aa);
										m_tmp_first_point = m_tmp_second_point;
										m_isFirstPoint = false;
										m_isSecondPoint = true;
										m_isLastPointOnArc = false;
										m_exist_arc_data = false;
										m_step =1;
										m_app->GetCommandPanel()->SetActiveRadio(m_step+2);
										m_app->GetCommandPanel()->EnableRadio(m_step+3,false);
										if (m_get_first_point)
											m_get_first_point->SetPoint(m_tmp_first_point.x,
											m_tmp_first_point.y,
											m_tmp_first_point.z);
										if (m_select_obj_type_panel)
											m_select_obj_type_panel->EnableLineType(true);
										if (m_can_close)
										{
											m_message.LoadString(IDS_CLOSE_CONTOUR);
											if (AfxMessageBox(m_message,MB_YESNO)==IDYES)
											{
												sgCObject* lastO = m_objects[m_objects.size()-1];
												ASSERT(lastO->GetType()==SG_OT_ARC);
												sgCArc* lastA = reinterpret_cast<sgCArc*>(lastO);
												m_tmp_first_point = lastA->GetGeometry()->begin;
												sgCObject::DeleteObject(lastO);
												m_objects.pop_back();
												m_arc_geo.FromThreePoints(m_tmp_first_point,m_first_point,
													m_cur_point,false);
												sgCArc* aa = sgCreateArc(m_arc_geo);
												m_objects.push_back(aa);
												CreateContourFromObjects();
											}
											else
												m_can_close = false;
										}
						}
						else
						{
							m_message.LoadString(IDS_ARC_ERR_ON_LINE);
							m_app->PutMessage(IApplicationInterface::MT_ERROR,
								m_message);
							goto unfix;
						}
					}
					else
					{
						ASSERT(0);
					}
		}
unfix:
		if (m_get_first_point)
		{
			m_get_first_point->XFix(false);
			m_get_first_point->YFix(false);
			m_get_first_point->ZFix(false);
		}
		if (m_get_second_point)
		{
			m_get_second_point->XFix(false);
			m_get_second_point->YFix(false);
			m_get_second_point->ZFix(false);
		}
		if (m_get_third_point)
		{
			m_get_third_point->XFix(false);
			m_get_third_point->YFix(false);
			m_get_third_point->ZFix(false);
		}
		m_app->GetViewPort()->InvalidateViewPort();
}

unsigned int  Contour::GetItemsCount()
{
	return 1;
}

void         Contour::GetItem(unsigned int itemID, CString& itSrt)
{
	SWITCH_RESOURCE
		if (itemID==0) 
		{
			itSrt.LoadString(IDS_END_OPER);
		}
		else
		{
			ASSERT(0);
		}
}

void     Contour::GetItemState(unsigned int itemID, bool& enbl, bool& checked)
{
	enbl = true;
	checked = false;
}

HBITMAP   Contour::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         Contour::Run(unsigned int itemID)
{
	switch(itemID) 
	{
	case 0:
		if (m_objects.size()==0)
		{
			m_message.LoadString(IDS_ERR_PATH_NO_OBJ);
			m_app->PutMessage(IApplicationInterface::MT_ERROR,
				m_message);
			return;
		}
		if (m_objects.size()==1)
		{
			m_message.LoadString(IDS_ERR_PATH_ONE_OBJ);
			m_app->PutMessage(IApplicationInterface::MT_ERROR,
				m_message);
			return;
		}
		CreateContourFromObjects();
		break;
	default:
		ASSERT(0);
	}
}