#include "StdAfx.h"
#include "ProteinVista.h"
#include "ProteinVistaView.h"
#include "PDBRenderer.h"
#include "Interface.h"
#include "ProteinVistaRenderer.h"
#include "ProteinSurfaceMSMS.h"
//#include "ProteinViewPanel.h"

#include "SelectionListPane.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


CPDBRenderer::CPDBRenderer(): m_bSelected(TRUE)
{
	m_iDisplayModel = 0;


	D3DXMatrixIdentity(&m_matWorld);
	
	D3DXMatrixIdentity(&m_matWorldMouseMoveRot);
	D3DXMatrixIdentity(&m_matWorldMouseMoveTrans);

	D3DXMatrixIdentity(&m_matWorldUserInput);
	D3DXMatrixIdentity(&m_matWorldUserInputOrig);
	D3DXMatrixIdentity(&m_matPDBCenter);

	D3DXMatrixIdentity(&m_matWorldPrevious);
	D3DXMatrixIdentity(&m_matPDBCenterInv);
	D3DXMatrixIdentity(&m_matPDBCenter);

	m_selectionCenterTransformed.x = m_selectionCenterTransformed.y = m_selectionCenterTransformed.z = 0.0f;

	m_biounitCenter = D3DXVECTOR3(0,0,0);
	m_bioUnitMinMaxBB[0] = D3DXVECTOR3(0,0,0);
	m_bioUnitMinMaxBB[1] = D3DXVECTOR3(0,0,0);
	m_biounitRadius = 0;

	m_bAttatchBioUnit = FALSE;

	m_bDrag = FALSE;
	m_posOldX = m_posOldY = -1000;

	m_vPickRayDir = D3DXVECTOR3(0,0,0);
	m_vPickRayOrig = D3DXVECTOR3(0,0,0);

	m_iBioUnit = 0;
	m_pPDBRendererParentBioUnit = NULL;
	D3DXMatrixIdentity(&m_matTransformBioUnit);

	m_bSelectionRotCenter = TRUE;	

	m_pPDBInst = NULL;

	m_selectionMinMaxBB[0] = D3DXVECTOR3(0,0,0);
	m_selectionMinMaxBB[1] = D3DXVECTOR3(0,0,0);
	m_selectionRadius = 0;
	m_selectionCenter = D3DXVECTOR3(0,0,0);

	m_bIsSelectionExist = FALSE;
	srand( (unsigned)time( NULL ) );
}

void CPDBRenderer::Init(CProteinVistaRenderer * pProteinVistaRenderer, CPDB * pPDB ) 
{
	m_pProteinVistaRenderer = pProteinVistaRenderer ; 
	//	m_pPDB = pPDB; 
	pPDB->m_pPDBRenderer = this; 

	//	
	//	여기에서, CPDBInst를 만든다.
	//
	CPDBInst * pPDBInst = new CPDBInst(pPDB);

	pPDBInst->m_pPDBRenderer = this;

	pPDBInst->m_arrayModelInst.reserve(pPDB->m_arrayModel.size());
	pPDBInst->m_arrayarrayChainInst.resize(pPDB->m_arrayarrayChain.size());
	for ( int i = 0 ; i < pPDB->m_arrayModel.size(); i++ )
	{
		CModel * pModel = pPDB->m_arrayModel[i];
		CModelInst * pModelInst = new CModelInst(pModel);
		pModelInst->m_pPDBInst = pPDBInst;
		pModelInst->m_pParent = pPDBInst;

		pPDBInst->m_arrayModelInst.push_back(pModelInst);

		pModelInst->m_arrayChainInst.reserve(pModel->m_arrayChain.size());
		for ( int j = 0 ; j < pModel->m_arrayChain.size() ; j++ )
		{
			CChain * pChain = pModel->m_arrayChain[j];
			CChainInst * pChainInst = new CChainInst(pChain);
			pChainInst->m_pPDBInst = pPDBInst;
			pChainInst->m_pModelInst = pModelInst;
			pChainInst->m_pParent = pModelInst;

			pModelInst->m_arrayChainInst.push_back(pChainInst);
			pPDBInst->m_arrayarrayChainInst[i].push_back(pChainInst);

			pChainInst->m_arrayResidueInst.reserve(pChain->m_arrayResidue.size());
			for ( int k = 0 ; k < pChain->m_arrayResidue.size() ; k ++ )
			{
				CResidue * pResidue = pChain->m_arrayResidue[k];
				CResidueInst * pResidueInst = new CResidueInst(pResidue);
				pResidueInst->m_pChainInst = pChainInst;
				pResidueInst->m_pParent = pChainInst;

				pChainInst->m_arrayResidueInst.push_back(pResidueInst);
				
				pResidueInst->m_arrayAtomInst.reserve(pResidue->m_arrayAtom.size());
				for ( int l = 0 ; l < pResidue->m_arrayAtom.size(); l++ )
				{
					CAtom * pAtom = pResidue->m_arrayAtom[l];
					CAtomInst * pAtomInst = new CAtomInst(pAtom);
					pAtomInst->m_pResidueInst = pResidueInst;
					pAtomInst->m_pChainInst = pChainInst;
					pAtomInst->m_pPDBInst = pPDBInst;
					pAtomInst->m_pParent = pResidueInst;

					//	
					LARGE_INTEGER li;
					li.LowPart = pAtom->m_serial; 
					li.HighPart = pAtom->m_iModel;

					pPDBInst->m_hashMapAtomInst.insert( CMapAtomInstPair( li.QuadPart , pAtomInst )) ;

					pResidueInst->m_arrayAtomInst.push_back(pAtomInst);

					pChainInst->m_arrayAtomInst.push_back(pAtomInst);

					if ( pAtomInst->GetAtom()->m_bHETATM == TRUE )
						pChainInst->m_arrayHETATMInst.push_back(pAtomInst);
				}
			}
		}
	}

	m_pPDBInst = pPDBInst;
}

CPDBRenderer::~CPDBRenderer()
{
	SAFE_DELETE(m_pPDBInst);

}

HRESULT CPDBRenderer::OneTimeSceneInit()            
{ 
	return S_OK; 
}

HRESULT CPDBRenderer::InitDeviceObjects()           
{ 

	return S_OK; 
}

HRESULT CPDBRenderer::RestoreDeviceObjects()        
{
	return S_OK; 
}
#pragma managed(push,off)
HRESULT CPDBRenderer::FrameMove()
{
	if ( m_bAttatchBioUnit == TRUE && m_pPDBRendererParentBioUnit != NULL )
	{	//	child 라면,
		CPDBRenderer * pPDBRenderer = m_pPDBRendererParentBioUnit;
		D3DXMATRIXA16	matWorldParent = pPDBRenderer->m_matWorldPrevious;

		m_matWorldPrevious	= m_matTransformBioUnit * matWorldParent ;	
		m_matPDBCenter		= pPDBRenderer->m_matPDBCenter;
		m_matWorldUserInput = pPDBRenderer->m_matWorldUserInput;
		m_matPDBCenterInv	= pPDBRenderer->m_matPDBCenterInv;
	}

	// user input(mouse_move)가 없으면 ( m_matPDBCenterInv * m_matWorldUserInput * m_matPDBCenter ) = I 이다.
	m_matWorld  =  m_matWorldPrevious * ( m_matPDBCenterInv * m_matWorldUserInput * m_matPDBCenter ) ;
	GetD3DDevice()->SetTransform( D3DTS_WORLD, &m_matWorld );

	D3DXVec3TransformCoord(&m_selectionCenterTransformed, &m_selectionCenter, &m_matWorld );

	return S_OK; 
}
 #pragma managed(pop)
HRESULT CPDBRenderer::Render()                      
{
	return S_OK; 
}

HRESULT CPDBRenderer::InvalidateDeviceObjects()     
{
	return S_OK; 
}

HRESULT CPDBRenderer::DeleteDeviceObjects()         
{
	return S_OK; 
}

HRESULT CPDBRenderer::FinalCleanup()                
{
  
	return S_OK; 
}
#pragma managed(push,off)
LRESULT CPDBRenderer::HandleMessages( HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam )
{
	BOOL bProcessed = FALSE;

	if ( m_bAttatchBioUnit == TRUE && m_pPDBRendererParentBioUnit != NULL )
	{
		bProcessed = m_pPDBRendererParentBioUnit->HandleMessages(hWnd, msg, wParam, lParam);
		return bProcessed;
	}

	int iMouseX = (short)LOWORD(lParam);
	int iMouseY = (short)HIWORD(lParam);

	switch( msg )
	{
		case WM_LBUTTONDOWN:
		case WM_RBUTTONDOWN:
			 
			SetCapture( hWnd );
			m_bDrag = TRUE;
			m_posOldX = iMouseX;
			m_posOldY = iMouseY;

			m_matWorldUserInputOrig = m_matWorldUserInput;
			D3DXMatrixIdentity(&m_matWorldMouseMoveRot);
			D3DXMatrixIdentity(&m_matWorldMouseMoveTrans);

			bProcessed = TRUE;
			break;

		case WM_LBUTTONUP:
		case WM_RBUTTONUP:
			if( m_bDrag == TRUE )
			{
				 
				m_bDrag = FALSE;
				m_posOldX = -10000;
				m_posOldY = -10000;
				bProcessed = TRUE;

				UpdatePDBRendererCenter();

				ReleaseCapture();
			}
			break;

		case WM_MOUSEMOVE:
			if ( m_bDrag == TRUE )
			{
			 
				if( MK_LBUTTON&wParam )
				{
					D3DXVECTOR3 axisX(1,0,0);
					D3DXVECTOR3 axisY(0,1,0);

					D3DXMATRIXA16 matRotModel = m_matWorldUserInputOrig;
					matRotModel._41 = matRotModel._42 = matRotModel._43 = 0.0f;

					D3DXMATRIXA16	matRotInv;
					D3DXMatrixInverse(&matRotInv, NULL, &(matRotModel*m_pProteinVistaRenderer->m_matCameraRotation) );

					D3DXVec3TransformCoord(&axisX, &axisX, &matRotInv );
					D3DXVec3TransformCoord(&axisY, &axisY, &matRotInv );

					//	m_matWorld
					D3DXMATRIXA16 matX, matY, matRotTemp;
					D3DXMatrixRotationAxis(&matX, &axisX, -(iMouseY-m_posOldY)/100.0f );
					D3DXMatrixRotationAxis(&matY, &axisY, -(iMouseX-m_posOldX)/100.0f );
					matRotTemp = matX * matY;

					m_matWorldMouseMoveRot = matRotTemp;
					//	
					m_matWorldUserInput = m_matWorldMouseMoveRot * m_matWorldUserInputOrig;

					bProcessed = TRUE;
					GetMainActiveView()->OnPaint();
				}
				else if( MK_RBUTTON&wParam ) 
				{
					D3DXMATRIXA16 matRotModel = m_matWorldUserInputOrig;
					matRotModel._41 = matRotModel._42 = matRotModel._43 = 0.0f;

					//	translation
					D3DXMATRIXA16	matRotInv;
					D3DXMatrixInverse (&matRotInv, NULL, &(matRotModel*m_pProteinVistaRenderer->m_matCameraRotation) );

					D3DXVECTOR3 vecTrans;
					if ( (GetAsyncKeyState(VK_LSHIFT)>>15) == 0 )
					{
						D3DXVECTOR3 vecLen((iMouseX-m_posOldX)/10.0f, - (iMouseY-m_posOldY)/10.0f, 0 );
						D3DXVec3TransformCoord(&vecTrans, &vecLen, &matRotInv);
					}
					else
					{	
						//	
						float moveDelta = 3.0f;
						D3DXVECTOR3 vecLen(0,0, (iMouseY-m_posOldY)/moveDelta);
						D3DXVec3TransformCoord(&vecTrans, &vecLen, &matRotInv);
					}

					m_matWorldMouseMoveTrans._41 = vecTrans.x;
					m_matWorldMouseMoveTrans._42 = vecTrans.y;
					m_matWorldMouseMoveTrans._43 = vecTrans.z;

					//	
					m_matWorldUserInput = m_matWorldMouseMoveTrans * m_matWorldUserInputOrig;

					bProcessed = TRUE;
					GetMainActiveView()->OnPaint();
				}
				FrameMove();
				
			}
			break;

		case WM_CAPTURECHANGED:
			{
				if ( m_bDrag == TRUE )
				{
					if( (lParam != NULL) && ((HWND)lParam != hWnd) )
					{
						ReleaseCapture();

						m_bDrag = FALSE;
						m_posOldX = -10000;
						m_posOldY = -10000;
						bProcessed = TRUE;

						UpdatePDBRendererCenter();
					}
				}
			}
			break;
	}

	return bProcessed;
}
#pragma managed(pop)
//
//	
void CPDBRenderer::SetBioUnitTransform(CPDBRenderer * pPDBRendererParent, long index, D3DXMATRIX &matTransform)
{
	m_iBioUnit = index;
	m_matTransformBioUnit = matTransform;
	m_pPDBRendererParentBioUnit = pPDBRendererParent;
}
//
//	모델에서 center 의 좌표를 지정한다.
//	center로 모델을 옮기는 것(CenterPDBRenderer()) 이 아니다. 
//
void CPDBRenderer::SetTransformCenter()
{
	//
	//	새로운 점이 예전 transform으로 변환하였을때 어디로 가는지 확인.
	//
	m_matWorldPrevious = m_matWorld;

	D3DXVec3TransformCoord(&m_selectionCenterTransformed, &m_selectionCenter, &m_matWorld );

	D3DXMatrixTranslation(&m_matPDBCenter, m_selectionCenterTransformed.x, m_selectionCenterTransformed.y, m_selectionCenterTransformed.z );
	D3DXMatrixTranslation(&m_matPDBCenterInv, -m_selectionCenterTransformed.x, -m_selectionCenterTransformed.y, -m_selectionCenterTransformed.z );

	D3DXMatrixIdentity(&m_matWorldUserInput);
}

//
//	find center, minMaxBB, radius
//	
void CPDBRenderer::GetCenterRadiusBB()
{
	//	object center를 설정하는것.
	if ( m_bAttatchBioUnit == TRUE )
	{	//
		if ( m_pPDBRendererParentBioUnit == NULL )
		{	//	parent 라면, 전체 biounit minmax를 사용
			m_selectionCenter = m_biounitCenter;
			m_selectionMinMaxBB[0] = m_bioUnitMinMaxBB[0];
			m_selectionMinMaxBB[1] = m_bioUnitMinMaxBB[1];
			m_selectionRadius = m_biounitRadius;
		}
		else
		{	//	child 라면, parent의 minmax를 사용
			m_selectionCenter = m_pPDBRendererParentBioUnit->m_biounitCenter;
			m_selectionMinMaxBB[0] = m_pPDBRendererParentBioUnit->m_bioUnitMinMaxBB[0];
			m_selectionMinMaxBB[1] = m_pPDBRendererParentBioUnit->m_bioUnitMinMaxBB[1];
			m_selectionRadius = m_pPDBRendererParentBioUnit->m_biounitRadius;
		}
	}
	else if ( m_bSelectionRotCenter == TRUE )
	{
		CSelectionDisplay * pSelectionDisplayCurrent = NULL;
		CHTMLListCtrl * phtmlListCtrl = GetMainActiveView()->GetSelectList();
		for ( int i = 0 ; i < phtmlListCtrl->GetItemCount() ; i++ )
		{
			CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *)(phtmlListCtrl->GetItemData(i));
			if ( pSelectionDisplay )
			{
				if ( pSelectionDisplay->m_bSelect == TRUE )
				{
					pSelectionDisplayCurrent = pSelectionDisplay;
					break;
				}
			}
		}

		if ( pSelectionDisplayCurrent != NULL )
		{
			m_selectionCenter = pSelectionDisplayCurrent->m_center;
			m_selectionMinMaxBB[0] = pSelectionDisplayCurrent->m_minMaxBB[0];
			m_selectionMinMaxBB[1] = pSelectionDisplayCurrent->m_minMaxBB[1];
			m_selectionRadius = pSelectionDisplayCurrent->m_radius;
		}
		else
		{
			m_selectionCenter = D3DXVECTOR3(0,0,0);

			CSTLArraySelectionInst selection;
			GetSelectedObject(selection);

			CSTLArrayAtomInst	arrayAtomInst;
			arrayAtomInst.reserve(4000);
			for ( int i = 0 ; i < selection.size() ; i++ )
			{
				selection[i]->GetChildAtoms(arrayAtomInst);
			}

			if ( arrayAtomInst.size() != 0 )
			{
				D3DXVECTOR3 minBB(1e20, 1e20, 1e20);
				D3DXVECTOR3 maxBB(-1e20, -1e20, -1e20);

				for ( int iAtom = 0 ; iAtom < arrayAtomInst.size() ; iAtom ++ )
				{
					D3DXVECTOR3 pos = arrayAtomInst[iAtom]->GetAtom()->m_pos;
					if (maxBB.x < pos.x) maxBB.x = pos.x;
					if (maxBB.y < pos.y) maxBB.y = pos.y;
					if (maxBB.z < pos.z) maxBB.z = pos.z;
					if (minBB.x > pos.x) minBB.x = pos.x;
					if (minBB.y > pos.y) minBB.y = pos.y;
					if (minBB.z > pos.z) minBB.z = pos.z;
				}
				m_selectionCenter = (minBB + maxBB)/2.0f;
				m_selectionMinMaxBB[0] = minBB;
				m_selectionMinMaxBB[1] = maxBB;
				m_selectionRadius = D3DXVec3Length(&D3DXVECTOR3(m_selectionCenter-minBB))+ 4.0f;
			}
			else
			{
				m_selectionCenter = GetPDBInst()->GetPDB()->m_pdbCenter;
				m_selectionMinMaxBB[0] = GetPDBInst()->GetPDB()->m_pdbMinMaxBB[0];
				m_selectionMinMaxBB[1] = GetPDBInst()->GetPDB()->m_pdbMinMaxBB[1];
				m_selectionRadius = GetPDBInst()->GetPDB()->m_pdbRadius;
			}
		}
	}
	else
	{
		m_selectionCenter = GetPDBInst()->GetPDB()->m_pdbCenter;
		m_selectionMinMaxBB[0] = GetPDBInst()->GetPDB()->m_pdbMinMaxBB[0];
		m_selectionMinMaxBB[1] = GetPDBInst()->GetPDB()->m_pdbMinMaxBB[1];
		m_selectionRadius = GetPDBInst()->GetPDB()->m_pdbRadius;
	}
}

void CPDBRenderer::UpdatePDBRendererCenter() 
{ 
	GetCenterRadiusBB();
	SetTransformCenter(); 
}

//	center로 옮겨놓는다.
void CPDBRenderer::CenterPDBRenderer()
{
	//
	D3DXMatrixTranslation(&m_matWorldUserInput, -m_selectionCenterTransformed.x, -m_selectionCenterTransformed.y, -m_selectionCenterTransformed.z );

	//	맨처음 load 헀을때, framemove 없이 CenterPDBRenderer 가 불리웠을때를 방지. 한번 transform 해준다.
	m_matWorld  =  m_matWorldPrevious * ( m_matPDBCenterInv * m_matWorldUserInput * m_matPDBCenter ) ;

	UpdatePDBRendererCenter();

	g_bRequestRender = TRUE;
}



