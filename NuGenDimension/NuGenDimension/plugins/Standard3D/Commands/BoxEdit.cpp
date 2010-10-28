#include "stdafx.h"

#include "BoxEdit.h"

#include "..//resource.h"


#include <math.h>


CBoxEditCommand::CBoxEditCommand(sgCBox* edB, IApplicationInterface* appI):
					m_editable_box(edB)
					, m_app(appI)
					, m_size_panel(NULL)
					, m_was_started(false)
					, m_matr(NULL)
					, m_scenar(0)
{
	ASSERT(edB);
	ASSERT(m_app);
	m_base_pnt.x = m_base_pnt.y = m_base_pnt.z = 0.0;
	m_dir.x = m_dir.y = m_dir.z = 0.0;
}

CBoxEditCommand::~CBoxEditCommand()
{
	m_app->GetCommandPanel()->RemoveAllDialogs();
	m_app->GetViewPort()->InvalidateViewPort();
	if(m_matr)
		delete m_matr;
}

void  CBoxEditCommand::ReCalcDir()
{
	m_dir.x = m_dir.y = m_dir.z = 0.0;
	switch(m_scenar) 
	{
	case 1:
		m_dir.x = 1.0;
		break;
	case 2:
		m_dir.y = 1.0;
		break;
	case 3:
		m_dir.z = 1.0;
		break;
	default:
		ASSERT(0);
		break;
	}

	if (m_matr)
	{
		SG_POINT ppp;
		ppp.x = ppp.y = ppp.z = 0.0;
		m_matr->ApplyMatrixToVector(ppp,m_dir);
	}
	else
	{
		ASSERT(0);
	}
}


bool    CBoxEditCommand::PreTranslateMessage(MSG* pMsg)
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
			if (m_size_panel)
			{
				m_size_panel->GetWindow()->SendMessage(pMsg->message,
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

void  CBoxEditCommand::Start()	
{
	ASSERT(m_editable_box);

	m_matr = new sgCMatrix(m_editable_box->GetWorldMatrixData());

	m_matr->Transparent();

	m_matr->ApplyMatrixToPoint(m_base_pnt);

	SWITCH_RESOURCE
	
	m_app->GetCommandPanel()->RemoveAllDialogs();
	CString lab;
	lab.LoadString(IDS_REDACTION);
	m_app->StartCommander(lab);

	lab.LoadString(IDS_NEW_SIZE);
	m_size_panel = reinterpret_cast<IGetNumberPanel*>(m_app->
		GetCommandPanel()->
		AddDialog(IBaseInterfaceOfGetDialogs::GET_NUMBER_DLG,
			lab,false));

	lab.LoadString(IDS_ENTER_NEW_SIZE);
	m_app->PutMessage(IApplicationInterface::MT_MESSAGE,lab);

	m_editable_box->GetGeometry(m_box_geo);
	switch(m_scenar) 
	{
	case 1:
		m_size_panel->SetNumber(m_box_geo.SizeX);
		break;
	case 2:
		m_size_panel->SetNumber(m_box_geo.SizeY);
		break;
	case 3:
		m_size_panel->SetNumber(m_box_geo.SizeZ);
		break;
	default:
		ASSERT(0);
		break;
	}

	ReCalcDir();

m_app->GetCommandPanel()->SetActiveRadio(0);
	m_app->GetViewPort()->SetEditableObject(m_editable_box);
	m_was_started = true;
}

void  CBoxEditCommand::MouseMove(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;
	ASSERT(m_size_panel);

	if (!m_app->GetViewPort()->ProjectScreenPointOnLine(pX,pY,m_base_pnt,m_dir,m_projection))
	{
		switch(m_scenar) 
		{
		case 1:
			m_box_geo.SizeX = 0.0;
			break;
		case 2:
			m_box_geo.SizeY = 0.0;
			break;
		case 3:
			m_box_geo.SizeZ = 0.0;
			break;
		default:
			ASSERT(0);
			break;
		}
		m_size_panel->SetNumber(0.0);
		return;
	}
	char sig = (((m_projection.x-m_base_pnt.x)*m_dir.x+
		(m_projection.y-m_base_pnt.y)*m_dir.y+
		(m_projection.z-m_base_pnt.z)*m_dir.z)>0)?1:-1;

	double  sz;
	sz = sgSpaceMath::PointsDistance(m_base_pnt,m_projection)*sig;
	sz = m_app->ApplyPrecision(sz);
	m_projection.x = m_base_pnt.x+m_dir.x*sz;
	m_projection.y = m_base_pnt.y+m_dir.y*sz;
	m_projection.z = m_base_pnt.z+m_dir.z*sz;
	switch(m_scenar) 
	{
	case 1:
		m_box_geo.SizeX = sz;
		break;
	case 2:
		m_box_geo.SizeY = sz;
		break;
	case 3:
		m_box_geo.SizeZ = sz;
		break;
	default:
		ASSERT(0);
		break;
	}
	m_size_panel->SetNumber(sz);
	m_app->GetViewPort()->InvalidateViewPort();
}

void  CBoxEditCommand::LeftClick(unsigned int nFlags,int pX,int pY)
{
	if (!m_was_started)
		return;
SWITCH_RESOURCE
	ASSERT(m_editable_box);

		if (fabs(m_box_geo.SizeX)<0.0001 ||
			fabs(m_box_geo.SizeY)<0.0001 ||
			fabs(m_box_geo.SizeZ)<0.0001)
		{
			m_message.LoadString(IDS_ERROR_ZERO_SIZE);
			m_app->PutMessage(IApplicationInterface::MT_ERROR,
				m_message);
			return;
		}
		SG_VECTOR vvv;memset(&vvv,0,sizeof(SG_VECTOR));
		if (m_box_geo.SizeX<0) { vvv.x=m_box_geo.SizeX; m_box_geo.SizeX=-m_box_geo.SizeX;}
		if (m_box_geo.SizeY<0) { vvv.y=m_box_geo.SizeY; m_box_geo.SizeY=-m_box_geo.SizeY;}
		if (m_box_geo.SizeZ<0) { vvv.z=m_box_geo.SizeZ; m_box_geo.SizeZ=-m_box_geo.SizeZ;}
		sgCBox* bx = sgCreateBox(m_box_geo.SizeX,m_box_geo.SizeY,m_box_geo.SizeZ);
		if (!bx)
			return;
		sgCMatrix* mtrx = bx->InitTempMatrix();
		mtrx->Multiply(*m_matr);
		mtrx->Translate(vvv);
		bx->ApplyTempMatrix();
		bx->DestroyTempMatrix();

		sgGetScene()->StartUndoGroup();
		sgGetScene()->DetachObject(m_editable_box);
		sgGetScene()->AttachObject(bx);
		sgGetScene()->EndUndoGroup();
		m_app->CopyAttributes(*bx,*m_editable_box);
		
		m_app->GetViewPort()->InvalidateViewPort();

	m_app->StopCommander();
}


void  CBoxEditCommand::Draw()
{
	if (!m_was_started)
		return;

	float pC[3];
	m_app->GetViewPort()->GetPainter()->GetUserColorPoints(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->DrawPoint(m_projection);
	
	m_app->GetViewPort()->GetPainter()->GetUserColorLines(pC[0],pC[1],pC[2]);
	m_app->GetViewPort()->GetPainter()->SetCurColor(pC[0],pC[1],pC[2]);

	m_app->GetViewPort()->GetPainter()->SetTransformMatrix(m_matr);
	m_app->GetViewPort()->GetPainter()->DrawBox(m_box_geo.SizeX,
					m_box_geo.SizeY,
					m_box_geo.SizeZ);
	m_app->GetViewPort()->GetPainter()->SetTransformMatrix(NULL);

}

void  CBoxEditCommand::OnEnter()
{
	if (!m_was_started)
		return;
	SWITCH_RESOURCE
		ASSERT(m_editable_box);

	double sz = m_size_panel->GetNumber();

	switch(m_scenar) 
	{
	case 1:
		m_box_geo.SizeX = sz;
		break;
	case 2:
		m_box_geo.SizeY = sz;
		break;
	case 3:
		m_box_geo.SizeZ = sz;
		break;
	default:
		ASSERT(0);
		break;
	}

	if (fabs(m_box_geo.SizeX)<0.0001 ||
		fabs(m_box_geo.SizeY)<0.0001 ||
		fabs(m_box_geo.SizeZ)<0.0001)
	{
		m_message.LoadString(IDS_ERROR_ZERO_SIZE);
		m_app->PutMessage(IApplicationInterface::MT_ERROR,
			m_message);
		return;
	}
	SG_VECTOR vvv;memset(&vvv,0,sizeof(SG_VECTOR));
	if (m_box_geo.SizeX<0) { vvv.x=m_box_geo.SizeX; m_box_geo.SizeX=-m_box_geo.SizeX;}
	if (m_box_geo.SizeY<0) { vvv.y=m_box_geo.SizeY; m_box_geo.SizeY=-m_box_geo.SizeY;}
	if (m_box_geo.SizeZ<0) { vvv.z=m_box_geo.SizeZ; m_box_geo.SizeZ=-m_box_geo.SizeZ;}
	sgCBox* bx = sgCreateBox(m_box_geo.SizeX,m_box_geo.SizeY,m_box_geo.SizeZ);
	if (!bx)
		return;
	sgCMatrix* mtrx = bx->InitTempMatrix();
	mtrx->Multiply(*m_matr);
	mtrx->Translate(vvv);
	bx->ApplyTempMatrix();
	bx->DestroyTempMatrix();

	sgGetScene()->StartUndoGroup();
	sgGetScene()->DetachObject(m_editable_box);
	sgGetScene()->AttachObject(bx);
	sgGetScene()->EndUndoGroup();
	m_app->CopyAttributes(*bx,*m_editable_box);
	
	m_app->GetViewPort()->InvalidateViewPort();

	m_app->StopCommander();
}

unsigned int  CBoxEditCommand::GetItemsCount()
{
return 3;
}

void         CBoxEditCommand::GetItem(unsigned int itemID, CString& itSrt)
{
	SWITCH_RESOURCE
		switch(itemID) {
	case 0:
		itSrt.LoadString(IDS_CHANGE_X_SIZE);
		break;
	case 1:
		itSrt.LoadString(IDS_CHANGE_Y_SIZE);
		break;
	case 2:
		itSrt.LoadString(IDS_CHANGE_Z_SIZE);
		break;
	default:
		ASSERT(0);
		}
}

void     CBoxEditCommand::GetItemState(unsigned int itemID, 
														bool& enbl, bool& checked)
{
	enbl = true;
	if (!m_was_started)
	{
		checked = false;
	}
	else
	{
		if (itemID==m_scenar-1)
			checked = true;
		else
			checked = false;
	}
}

HBITMAP     CBoxEditCommand::GetItemBitmap(unsigned int)
{
	return NULL;
}

void         CBoxEditCommand::Run(unsigned int itemID)
{
	if (!m_was_started)
	{
		m_scenar = itemID+1;
		Start();
	}
	else
	{
		m_scenar = itemID+1;
		ReCalcDir();
		m_editable_box->GetGeometry(m_box_geo);
	}
}
