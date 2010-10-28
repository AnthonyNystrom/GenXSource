#include "stdafx.h"

#include "Ellipsoid.h"

#include "..//resource.h"


#include <math.h>

int     ellipsoid_name_index=1;

EllipsoidCommand::EllipsoidCommand(IApplicationInterface* appI):
				m_app(appI)
					,m_base_point_panel(NULL)
					,m_x_size_panel(NULL)
					,m_y_size_panel(NULL)
					, m_z_size_panel(NULL)
				, m_step(0)
				, size1(0.0)
				, size2(0.0)
				, size3(0.0)
{
	ASSERT(m_app);
}

EllipsoidCommand::~EllipsoidCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
}


bool    EllipsoidCommand::PreTranslateMessage(MSG* pMsg)
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
			switch(m_step) {
			case 0:
				if (m_base_point_panel)
					m_base_point_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
				break;
			case 1:
				if (m_x_size_panel)
					m_x_size_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
				break;
			case 2:
				if (m_y_size_panel)
					m_y_size_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
				break;
			case 3:
				if (m_z_size_panel)
					m_z_size_panel->GetWindow()->SendMessage(pMsg->message,
						pMsg->wParam,
						pMsg->lParam);
				break;
			default:
				return false;
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


void   EllipsoidCommand::SendCommanderMessage(ICommander::COMMANDER_MESSAGE mes, void* params)
{
	if (mes==ICommander::CM_SWITCH_ROLLUP_DIALOG)
	{
		ASSERT(params!=NULL);
		int   newActiveDlg = *(reinterpret_cast<int*>(params));

		ASSERT(newActiveDlg<=3);
		m_step = (unsigned int)newActiveDlg;
		for (unsigned int i=m_step+1;i<=3;i++)
			m_app->GetCommandPanel()->EnableRadio(i,false);
		SWITCH_RESOURCE
			switch(m_step) 
		{
			case 0:
				m_message.LoadString(IDS_BOX_ENTER_BASE_PNT);
				break;
			case 1:
				m_message.LoadString(IDS_BOX_X_SIZE);
				break;
			case 2:
				m_message.LoadString(IDS_BOX_Y_SIZE);
				break;
			case 3:
				m_message.LoadString(IDS_BOX_Z_SIZE);
				break;
			default:
				ASSERT(0);
				break;
		}
		m_app->PutMessage(IApplicationInterface::MT_MESSAGE,m_message);

	}
}


void  EllipsoidCommand::Start()	
{
	NewScenar();
	SWITCH_RESOURCE
	m_message.LoadString(IDS_TOOLTIP_FIVETH);
	m_app->StartCommander(m_message);
}

void  EllipsoidCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
	switch(m_step) 
	{
		case 0:
			{
				ASSERT(m_base_point_panel);
				IViewPort::GET_SNAP_IN in_arg;
				in_arg.scrX = pX;
				in_arg.scrY = pY;
				in_arg.snapType = SNAP_SYSTEM;
				in_arg.XFix = m_base_point_panel->IsXFixed();
				in_arg.YFix = m_base_point_panel->IsYFixed();
				in_arg.ZFix = m_base_point_panel->IsZFixed();
				double tmpFl[3];
				m_base_point_panel->GetPoint(tmpFl[0],tmpFl[1],tmpFl[2]);
				in_arg.FixPoint.x = tmpFl[0];
				in_arg.FixPoint.y = tmpFl[1];
				in_arg.FixPoint.z = tmpFl[2];
				IViewPort::GET_SNAP_OUT out_arg;
				m_app->GetViewPort()->GetWorldPointAfterSnap(in_arg,out_arg);
				m_cur_pnt = out_arg.result_point;
				m_base_point_panel->SetPoint(m_cur_pnt.x,m_cur_pnt.y,m_cur_pnt.z);
			}
			break;
		case 1:
			{
				ASSERT(m_x_size_panel);

				SG_VECTOR dir; dir.x = 1.0; dir.y = 0.0; dir.z = 0.0;
				
				if (!m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,m_first_pnt,dir,m_projection))
				{
					size1 = 0.0;
					m_x_size_panel->SetNumber(size1);
					break;
				}
				size1 = sgSpaceMath::PointsDistance(m_first_pnt,m_projection);
				size1 = m_app->ApplyPrecision(size1);
				m_projection.x = m_first_pnt.x+dir.x*size1;
				m_projection.y = m_first_pnt.y+dir.y*size1;
				m_projection.z = m_first_pnt.z+dir.z*size1;
				m_x_size_panel->SetNumber(size1);
			}
			break;
		case 2:
			{
				ASSERT(m_y_size_panel);

				SG_VECTOR dir; dir.y = 1.0; dir.x = 0.0; dir.z = 0.0;
				
				if (!m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,m_first_pnt,dir,m_projection))
				{
					size2 = 0.0;
					m_y_size_panel->SetNumber(size2);
					break;
				}
				
				size2 = sgSpaceMath::PointsDistance(m_first_pnt,m_projection);
				size2 = m_app->ApplyPrecision(size2);
				m_projection.x = m_first_pnt.x+dir.x*size2;
				m_projection.y = m_first_pnt.y+dir.y*size2;
				m_projection.z = m_first_pnt.z+dir.z*size2;
				m_y_size_panel->SetNumber(size2);
			}
			break;
		case 3:
			{
				
				ASSERT(m_z_size_panel);

				SG_VECTOR dir; dir.z = 1.0; dir.y = 0.0; dir.x = 0.0;
				
				if (!m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,m_first_pnt,dir,m_projection))
				{
					size3 = 0.0;
					m_z_size_panel->SetNumber(size3);
					break;
				}
				
				size3 = sgSpaceMath::PointsDistance(m_first_pnt,m_projection);
				size3 = m_app->ApplyPrecision(size3);
				m_projection.x = m_first_pnt.x+dir.x*size3;
				m_projection.y = m_first_pnt.y+dir.y*size3;
				m_projection.z = m_first_pnt.z+dir.z*size3;
				m_z_size_panel->SetNumber(size3);
			}
			break;
		default:
			ASSERT(0);
	}
	m_app->GetViewPort()->InvalidateViewPort();
}

void  EllipsoidCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	SWITCH_RESOURCE
		switch(m_step) 
	{
		case 0:
			m_first_pnt=m_cur_pnt;
			m_step++;
			m_app->GetCommandPanel()->SetActiveRadio(m_step);
			m_message.LoadString(IDS_BOX_X_SIZE);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
				m_message);
			break;
		case 1:
			if (fabs(size1)<0.0001)
			{
				m_message.LoadString(IDS_ERROR_ZERO_SIZE);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			m_step++;
			m_app->GetCommandPanel()->SetActiveRadio(m_step);
			m_message.LoadString(IDS_BOX_Y_SIZE);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
				m_message);
			break;
		case 2:
			if (fabs(size2)<0.0001)
			{
				m_message.LoadString(IDS_ERROR_ZERO_SIZE);
				m_app->PutMessage(IApplicationInterface::MT_ERROR,
					m_message);
				return;
			}
			m_step++;
			m_app->GetCommandPanel()->SetActiveRadio(m_step);
			m_message.LoadString(IDS_BOX_Z_SIZE);
			m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
				m_message);
			break;
		case 3:
			{
				if (fabs(size3)<0.0001)
				{
					m_message.LoadString(IDS_ERROR_ZERO_SIZE);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				if (size1<0) size1=-size1;
				if (size2<0) size2=-size2;
				if (size3<0) size3=-size3;
				sgCEllipsoid* elpsd = sgCreateEllipsoid(size1,size2,size3,24,24);
				if (!elpsd)
					return;
				sgCMatrix* mtrx = elpsd->InitTempMatrix();
				mtrx->Translate(m_first_pnt);
				elpsd->ApplyTempMatrix();
				elpsd->DestroyTempMatrix();
				CString nm;
				nm.LoadString(IDS_TOOLTIP_FIVETH);
				CString nmInd;
				nmInd.Format("%i",ellipsoid_name_index);
				nm+=nmInd;
				elpsd->SetName(nm);
				sgGetScene()->StartUndoGroup();
				sgGetScene()->AttachObject(elpsd);
				m_app->ApplyAttributes(elpsd);
				sgGetScene()->EndUndoGroup();
				ellipsoid_name_index++;
				m_app->GetViewPort()->InvalidateViewPort();
				NewScenar();
			}
			break;
		default:
			ASSERT(0);
			break;
	}
}


void  EllipsoidCommand::Draw()
{
	switch(m_step) 
	{
		case 0:
			{
				float pC[3];
				m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
			}
			break;
		case 1:
			{
				float pC[3];
				m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_projection);
				SG_LINE ln;
				ln.p1 = m_first_pnt;
				ln.p1.x +=size1;
				ln.p2 = m_first_pnt;
				ln.p2.x -=size1;
				m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->DrawLine(ln);
			}
			break;
		case 2:
			{
				float pC[3];
				m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_projection);
				m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
				SG_LINE ln;
				ln.p1 = m_first_pnt;
				ln.p1.x +=size1;
				ln.p2 = m_first_pnt;
				ln.p2.x -=size1;				
				m_app->GetViewPort()->GetPainter()->DrawLine(ln);
				ln.p1 = m_first_pnt;
				ln.p1.y +=size2;
				ln.p2 = m_first_pnt;
				ln.p2.y -=size2;				
				m_app->GetViewPort()->GetPainter()->DrawLine(ln);
			}
			break;
		case 3:
			{
				float pC[3];
				m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_cur_pnt);
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_first_pnt);
				m_app->GetViewPort()->GetPainter()->DrawPoint(m_projection);
				m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
				m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
				SG_LINE ln;
				ln.p1 = m_first_pnt;
				ln.p1.x +=size1;
				ln.p2 = m_first_pnt;
				ln.p2.x -=size1;				
				m_app->GetViewPort()->GetPainter()->DrawLine(ln);
				ln.p1 = m_first_pnt;
				ln.p1.y +=size2;
				ln.p2 = m_first_pnt;
				ln.p2.y -=size2;				
				m_app->GetViewPort()->GetPainter()->DrawLine(ln);
				ln.p1 = m_first_pnt;
				ln.p1.z +=size3;
				ln.p2 = m_first_pnt;
				ln.p2.z -=size3;				
				m_app->GetViewPort()->GetPainter()->DrawLine(ln);
			}
			break;
		default:
			ASSERT(0);
	}
}


void  EllipsoidCommand::OnEnter()
{
	SWITCH_RESOURCE
		switch(m_step) 
	{
		case 0:
			{
				ASSERT(m_base_point_panel);
				m_base_point_panel->GetPoint(m_first_pnt.x,
					m_first_pnt.y,
					m_first_pnt.z);
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_BOX_X_SIZE);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
					m_message);
			}
			break;
		case 1:
			{
				size1 = m_x_size_panel->GetNumber();
				if (fabs(size1)<0.0001)
				{
					m_message.LoadString(IDS_ERROR_ZERO_SIZE);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_BOX_Y_SIZE);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
					m_message);
			}
			break;
		case 2:
			{
				size2 = m_y_size_panel->GetNumber();
				if (fabs(size2)<0.0001)
				{
					m_message.LoadString(IDS_ERROR_ZERO_SIZE);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				m_step++;
				m_app->GetCommandPanel()->SetActiveRadio(m_step);
				m_message.LoadString(IDS_BOX_Z_SIZE);
				m_app->PutMessage(IApplicationInterface::MT_MESSAGE,
					m_message);
			}
			break;
		case 3:
			{
				size3 = m_z_size_panel->GetNumber();
				if (fabs(size3)<0.0001)
				{
					m_message.LoadString(IDS_ERROR_ZERO_SIZE);
					m_app->PutMessage(IApplicationInterface::MT_ERROR,
						m_message);
					return;
				}
				if (size1<0) size1=-size1;
				if (size2<0) size2=-size2;
				if (size3<0) size3=-size3;
				sgCEllipsoid* elpsd = sgCreateEllipsoid(size1,size2,size3,24,24);
				if (!elpsd)
					return;
				sgCMatrix* mtrx = elpsd->InitTempMatrix();
				mtrx->Translate(m_first_pnt);
				elpsd->ApplyTempMatrix();
				elpsd->DestroyTempMatrix();
				CString nm;
				nm.LoadString(IDS_TOOLTIP_FIVETH);
				CString nmInd;
				nmInd.Format("%i",ellipsoid_name_index);
				nm+=nmInd;
				elpsd->SetName(nm);
				sgGetScene()->StartUndoGroup();
				sgGetScene()->AttachObject(elpsd);
				m_app->ApplyAttributes(elpsd);
				sgGetScene()->EndUndoGroup();
				ellipsoid_name_index++;
				m_app->GetViewPort()->InvalidateViewPort();
				NewScenar();
			}
			break;
		default:
			ASSERT(0);
			break;
	}
}

unsigned int  EllipsoidCommand::GetItemsCount()
{
	return 0;
}

void         EllipsoidCommand::GetItem(unsigned int, CString&)
{
	SWITCH_RESOURCE
}

void     EllipsoidCommand::GetItemState(unsigned int, bool&, bool&)
{
}

HBITMAP     EllipsoidCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         EllipsoidCommand::Run(unsigned int)
{
}

void  EllipsoidCommand::NewScenar()
{
	SWITCH_RESOURCE

	m_app->GetCommandPanel()->RemoveAllDialogs();
	CString lab;
	lab.LoadString(IDS_BASE_POINT);
	m_base_point_panel = reinterpret_cast<IGetPointPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_POINT_DLG,lab,true));
	
	lab.LoadString(IDS_X_SIZE);
	m_x_size_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,lab,true));
	

	lab.LoadString(IDS_Y_SIZE);
	m_y_size_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,lab,true));
	
	lab.LoadString(IDS_Z_SIZE);
	m_z_size_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,lab,true));
	
	m_app->GetCommandPanel()->SetActiveRadio(0);;

	lab.LoadString(IDS_BOX_ENTER_BASE_PNT);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);

	m_step=0;
}