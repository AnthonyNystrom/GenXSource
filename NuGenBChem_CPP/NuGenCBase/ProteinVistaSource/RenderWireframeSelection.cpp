
#include "StdAfx.h"
#include "ProteinVista.h"


#include "ProteinVistaView.h"
#include "PDBRenderer.h"
#include "ProteinVistaRenderer.h"
#include "Interface.h"
#include "RenderWireframeSelection.h"
#include "Pick.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


//	
CRenderWireframeSelection::CRenderWireframeSelection()
{
	m_iDisplaySelection = 0;

	m_typeWireframe = PROTEIN;

	m_pMeshWireframeVertexBuffer = NULL;
	m_pDeclWireframe = NULL;

	m_pPDBRenderer = NULL;
	m_maxNumLine = 1000;

	m_pPropertyCommon = NULL;

	m_weightLine = 0.1f;
	m_pPropertyWireframe = NULL;

	m_numWireframeVertex = 0;
}

CRenderWireframeSelection::~CRenderWireframeSelection()
{
	DeleteDeviceObjects();
}

//	
//	
//	
HRESULT CRenderWireframeSelection::InitDeviceObjects()
{
	DeleteDeviceObjects();

	m_arrayWireframeVertex.reserve(m_maxNumLine);
	m_maxNumLine = 0;

	D3DVERTEXELEMENT9	decl[MAX_FVF_DECL_SIZE];
	D3DXDeclaratorFromFVF( CWireframeVertex::FVF, decl);
	GetD3DDevice()->CreateVertexDeclaration(decl, &m_pDeclWireframe);

	CSelectionDisplay * pSelectionDisplay = m_pProteinVistaRenderer->m_arraySelectionDisplay[m_iDisplaySelection];
	m_pPropertyCommon = pSelectionDisplay->GetPropertyCommon();

	m_pPropertyWireframe = pSelectionDisplay->GetPropertyWireframe();
	BOOL bDisplayMethod = m_pPropertyWireframe->m_enumDisplayMethod;		//	0,1,2	all, main chain, only ca

	//	D3DXCOLOR	selectionColor = COLORREF2D3DXCOLOR(m_pProteinVistaRenderer->m_pPropertyScene->m_selectionColor);
	CChainInst * pChainInst = m_pChainInst;

	if ( bDisplayMethod != 2 )
	{
		for ( int iAtom = 0 ; iAtom < pChainInst->m_arrayAtomInst.size() ; iAtom++ )
		{
			CAtomInst * pAtom1 = pChainInst->m_arrayAtomInst[iAtom];
			if ( pAtom1->GetDisplayStyle(m_iDisplaySelection) == FALSE )
				continue;

			//	main chain 이면서 
			if ( bDisplayMethod == 1 && pAtom1->GetAtom()->m_bSideChain == TRUE )
				continue;

			for ( int i = 0 ; i < pAtom1->GetAtom()->m_arrayBondIndex.size() ; i++ )
			{
				DWORD bondIndex = pAtom1->GetAtom()->m_arrayBondIndex[i];
				DWORD iBond = pChainInst->GetChain()->m_arrayBond[bondIndex];
				CAtomInst * pAtom2 = pChainInst->m_arrayAtomInst[HIWORD(iBond)];

				if ( pAtom2->GetDisplayStyle(m_iDisplaySelection) == TRUE )
				{
					//	pAtom1, pAtom2 에 대해서 line 이 2개, vertex 가 4개 생긴다.

					CColorRow * pColorRow1 = m_pPropertyCommon->m_pSelectionDisplay->GetAtomColor(pAtom1->GetAtom());
					CColorRow * pColorRow2 = m_pPropertyCommon->m_pSelectionDisplay->GetAtomColor(pAtom2->GetAtom());

					D3DCOLOR	atom1Color = pColorRow1->m_color;
					D3DCOLOR	atom2Color = pColorRow2->m_color;

					D3DXVECTOR3 mid = (pAtom1->GetAtom()->m_pos + pAtom2->GetAtom()->m_pos)/2;
					CWireframeVertex vertex1;
					vertex1.m_pos = pAtom1->GetAtom()->m_pos;
					vertex1.m_color = atom1Color;
					vertex1.m_bSelect = ( m_pPropertyCommon->m_indicateColorSlot != -1 )?(m_pPropertyCommon->m_indicateColorSlot * 0.1f):(float)(pAtom1->GetSelect())*0.1f;

					CWireframeVertex vertex2;
					vertex2.m_pos = mid;
					vertex2.m_color = atom1Color;
					vertex2.m_bSelect = ( m_pPropertyCommon->m_indicateColorSlot != -1 )?(m_pPropertyCommon->m_indicateColorSlot * 0.1f):(float)(pAtom1->GetSelect()) * 0.1f;

					CWireframeVertex vertex3;
					vertex3.m_pos = mid;
					vertex3.m_color = atom2Color;
					vertex3.m_bSelect = ( m_pPropertyCommon->m_indicateColorSlot != -1 )?(m_pPropertyCommon->m_indicateColorSlot * 0.1f):(float)(pAtom2->GetSelect()) * 0.1f;

					CWireframeVertex vertex4;
					vertex4.m_pos = pAtom2->GetAtom()->m_pos;
					vertex4.m_color = atom2Color;
					vertex4.m_bSelect = ( m_pPropertyCommon->m_indicateColorSlot != -1 )?(m_pPropertyCommon->m_indicateColorSlot * 0.1f):(float)(pAtom2->GetSelect()) * 0.1f;

					if ( m_pPropertyCommon->m_bShowSelectionMark == FALSE )
					{
						vertex1.m_bSelect = 0;
						vertex2.m_bSelect = 0;
						vertex3.m_bSelect = 0;
						vertex4.m_bSelect = 0;
					}

					m_arrayWireframeVertex.push_back(vertex1);
					m_arrayWireframeVertex.push_back(vertex2);
					m_arrayWireframeVertex.push_back(vertex3);
					m_arrayWireframeVertex.push_back(vertex4);

					m_maxNumLine += 4;
				}
			}
		}
	}
	else
	{
		CAtomInst * pAtom1 = NULL;
		for ( int iAtom = 0 ; iAtom < pChainInst->m_arrayAtomInst.size() ; iAtom++ )
		{
			CAtomInst * pAtom2 = pChainInst->m_arrayAtomInst[iAtom];
			if ( pAtom2->GetDisplayStyle(m_iDisplaySelection) == FALSE )	//	main chain 일때에는 처음 나오는것 하나만 사용.
				break;

			if ( pAtom2->GetAtom()->m_typeAtom != MAINCHAIN_CA )
				continue;

			if ( pAtom1 == NULL )
			{
				pAtom1 = pAtom2;
				continue;
			}

			//	pAtom1, pAtom2 에 대해서 line 이 2개, vertex 가 4개 생긴다.
			CColorRow * pColorRow1 = m_pPropertyCommon->m_pSelectionDisplay->GetAtomColor(pAtom1->GetAtom());
			CColorRow * pColorRow2 = m_pPropertyCommon->m_pSelectionDisplay->GetAtomColor(pAtom2->GetAtom());

			D3DCOLOR	atom1Color = pColorRow1->m_color;
			D3DCOLOR	atom2Color = pColorRow2->m_color;

			D3DXVECTOR3 mid = (pAtom1->GetAtom()->m_pos + pAtom2->GetAtom()->m_pos)/2;
			CWireframeVertex vertex1;
			vertex1.m_pos = pAtom1->GetAtom()->m_pos;
			vertex1.m_color = atom1Color;
			vertex1.m_bSelect = ( m_pPropertyCommon->m_indicateColorSlot != -1 )?(m_pPropertyCommon->m_indicateColorSlot * 0.1f):(float)(pAtom1->GetSelect()) * 0.1f;

			CWireframeVertex vertex2;
			vertex2.m_pos = mid;
			vertex2.m_color = atom1Color;
			vertex2.m_bSelect = ( m_pPropertyCommon->m_indicateColorSlot != -1 )?(m_pPropertyCommon->m_indicateColorSlot * 0.1f):(float)(pAtom1->GetSelect()) * 0.1f;

			CWireframeVertex vertex3;
			vertex3.m_pos = mid;
			vertex3.m_color = atom2Color;
			vertex3.m_bSelect = ( m_pPropertyCommon->m_indicateColorSlot != -1 )?(m_pPropertyCommon->m_indicateColorSlot * 0.1f):(float)(pAtom2->GetSelect()) * 0.1f;

			CWireframeVertex vertex4;
			vertex4.m_pos = pAtom2->GetAtom()->m_pos;
			vertex4.m_color = atom2Color;
			vertex4.m_bSelect = ( m_pPropertyCommon->m_indicateColorSlot != -1 )?(m_pPropertyCommon->m_indicateColorSlot * 0.1f):(float)(pAtom2->GetSelect()) * 0.1f;

			if ( m_pPropertyCommon->m_bShowSelectionMark == FALSE )
			{
				vertex1.m_bSelect = 0;
				vertex2.m_bSelect = 0;
				vertex3.m_bSelect = 0;
				vertex4.m_bSelect = 0;
			}

			m_arrayWireframeVertex.push_back(vertex1);
			m_arrayWireframeVertex.push_back(vertex2);
			m_arrayWireframeVertex.push_back(vertex3);
			m_arrayWireframeVertex.push_back(vertex4);

			pAtom1 = pAtom2;
		}
	}

	if ( m_arrayWireframeVertex.size() == 0 )
		return E_FAIL;

	if ( m_pPropertyWireframe->m_lineWidth == 1 )
	{	//	wireframe
		GetD3DDevice()->CreateVertexBuffer(sizeof(CWireframeVertex)*m_arrayWireframeVertex.size() , 0, 0, D3DPOOL_MANAGED, &m_pMeshWireframeVertexBuffer, NULL);

		CWireframeVertex *vertex;
		m_pMeshWireframeVertexBuffer->Lock(0,0, (void**)&vertex, 0 );
		CopyMemory(vertex, &m_arrayWireframeVertex[0], sizeof(CWireframeVertex)*m_arrayWireframeVertex.size() );
		m_pMeshWireframeVertexBuffer->Unlock();

		m_numWireframeVertex = m_arrayWireframeVertex.size();
	}
	else
	{
		//	billboard

		m_arrayWireframeVertexWeight.reserve(m_arrayWireframeVertex.size()* 3);

		for ( int i = 0 ; i < m_arrayWireframeVertex.size() ; i += 2 )
		{
			D3DXVECTOR3 pos0 = m_arrayWireframeVertex[i].m_pos;
			D3DXVECTOR3 pos1 = m_arrayWireframeVertex[i+1].m_pos;

			//
			//
			CWireframeVertex vertex;	
			vertex.m_pos = pos0;
			vertex.m_posEnd = pos1;

			vertex.m_bSelect =  m_arrayWireframeVertex[i+1].m_bSelect;	vertex.m_index = 1.0f; 	vertex.m_color = m_arrayWireframeVertex[i+1].m_color;	m_arrayWireframeVertexWeight.push_back(vertex);
			vertex.m_bSelect =  m_arrayWireframeVertex[i+1].m_bSelect;	vertex.m_index = 2.0f; 	vertex.m_color = m_arrayWireframeVertex[i+1].m_color;	m_arrayWireframeVertexWeight.push_back(vertex);
			vertex.m_bSelect =  m_arrayWireframeVertex[i].m_bSelect;	vertex.m_index = 3.0f; 	vertex.m_color = m_arrayWireframeVertex[i].m_color;		m_arrayWireframeVertexWeight.push_back(vertex);
			vertex.m_bSelect =  m_arrayWireframeVertex[i].m_bSelect;	vertex.m_index = 3.0f; 	vertex.m_color = m_arrayWireframeVertex[i].m_color;		m_arrayWireframeVertexWeight.push_back(vertex);
			vertex.m_bSelect =  m_arrayWireframeVertex[i+1].m_bSelect;	vertex.m_index = 2.0f; 	vertex.m_color = m_arrayWireframeVertex[i+1].m_color;	m_arrayWireframeVertexWeight.push_back(vertex);
			vertex.m_bSelect =  m_arrayWireframeVertex[i].m_bSelect;	vertex.m_index = 4.0f; 	vertex.m_color = m_arrayWireframeVertex[i].m_color;		m_arrayWireframeVertexWeight.push_back(vertex);
		}

		//	
		HRESULT hr;
		hr = GetD3DDevice()->CreateVertexBuffer(sizeof(CWireframeVertex)*m_arrayWireframeVertexWeight.size() , 0, 0, D3DPOOL_MANAGED, &m_pMeshWireframeVertexBuffer, NULL);

		CWireframeVertex *vertex;
		m_pMeshWireframeVertexBuffer->Lock(0,0, (void**)&vertex, 0 );
		CopyMemory(vertex, &m_arrayWireframeVertexWeight[0], sizeof(CWireframeVertex)*m_arrayWireframeVertexWeight.size() );
		m_pMeshWireframeVertexBuffer->Unlock();

		m_numWireframeVertex = m_arrayWireframeVertexWeight.size();
	}

	m_arrayWireframeVertex.clear();
	m_arrayWireframeVertexWeight.clear();


	return S_OK;
}

HRESULT CRenderWireframeSelection::DeleteDeviceObjects()
{
	SAFE_RELEASE(m_pMeshWireframeVertexBuffer);
	SAFE_RELEASE(m_pDeclWireframe);

	return S_OK;
}
#pragma managed(push,off)
HRESULT CRenderWireframeSelection::Render()
{
	if ( m_pMeshWireframeVertexBuffer == NULL )
		return S_OK;

	HRESULT hr;

	GetD3DDevice()->SetRenderState(D3DRS_CULLMODE , D3DCULL_NONE);
	GetD3DDevice()->SetVertexDeclaration( m_pDeclWireframe );
	
	m_pProteinVistaRenderer->SetShaderIndicate(m_pPropertyCommon->m_bIndicate);

	//	D3DXCOLOR	selectionColor = COLORREF2D3DXCOLOR(m_pProteinVistaRenderer->m_pPropertyScene->m_selectionColor);
	//	m_pProteinVistaRenderer->SetShaderSelectionDiffuseColor(selectionColor);
	
	D3DXMATRIXA16 &	matModel = m_pPDBRenderer->m_matWorld;
	m_pProteinVistaRenderer->SetShaderWorldMatrix( matModel );

	D3DXMATRIXA16 matWorldView = matModel * (*m_pProteinVistaRenderer->GetViewMatrix());
	D3DXMATRIXA16 matWorldViewProj = matWorldView * (*m_pProteinVistaRenderer->GetProjMatrix());
	m_pProteinVistaRenderer->SetShaderWorldViewProjMatrix( matWorldViewProj );

	if ( m_pPropertyWireframe->m_lineWidth == 1 )
	{
		m_pProteinVistaRenderer->SetShaderTechnique(CProteinVistaRenderer::WireframeRendering, m_pPropertyCommon->m_shaderQuality);
		GetD3DDevice()->SetStreamSource( 0, m_pMeshWireframeVertexBuffer, 0, sizeof(CWireframeVertex) );
	}
	else
	{
		m_pProteinVistaRenderer->SetShaderTechnique(CProteinVistaRenderer::WireframeRenderingLineWidth, m_pPropertyCommon->m_shaderQuality);
		GetD3DDevice()->SetStreamSource( 0, m_pMeshWireframeVertexBuffer, 0, sizeof(CWireframeVertex) );

		D3DXVECTOR3 cameraPos = m_pProteinVistaRenderer->m_FromVec;
		D3DXMATRIXA16 matWorldInverse = matModel;
		D3DXMatrixTranspose(&matWorldInverse, &matWorldInverse);
		D3DXVec3TransformCoord(&cameraPos, &cameraPos, &matWorldInverse);

		m_pProteinVistaRenderer->SetShaderCameraPosInvWorld(cameraPos);

		//	line width는 거리에 따라 변하지 않는다.
		//
		if ( m_pProteinVistaRenderer->m_pPropertyScene->m_cameraType == m_pProteinVistaRenderer->m_pPropertyScene->CAMERA_OTHO )
		{
			m_pProteinVistaRenderer->SetShaderWireframeLineWidth ( (m_pPropertyWireframe->m_lineWidth)/900.0f );
		}
		else
		{
			D3DXVECTOR3 vec3Len = ( m_pPDBRenderer->m_selectionCenterTransformed ) -  m_pProteinVistaRenderer->m_FromVec;

			//	FOV를 포함해야 fov 에 따라서 두께가 안 변한다.
			FLOAT len = D3DXVec3Length(&vec3Len) * tan((m_pProteinVistaRenderer->m_pPropertyScene->m_lFOV/180.0f) * D3DX_PI /2 );

			m_pProteinVistaRenderer->SetShaderWireframeLineWidth( len/300.0f * m_pPropertyWireframe->m_lineWidth / 80.0f );
		}
	}

	//	
	UINT cPasses;
	V( m_pProteinVistaRenderer->m_pEffectBasicShading->Begin(&cPasses, 0) );
	for (long iPass = 0; iPass < cPasses; iPass++)
	{
		V( m_pProteinVistaRenderer->m_pEffectBasicShading->BeginPass(iPass) );

		if ( m_pPropertyWireframe->m_lineWidth == 1 )
			GetD3DDevice()->DrawPrimitive(D3DPT_LINELIST , 0, m_numWireframeVertex/2);
		else
			GetD3DDevice()->DrawPrimitive(D3DPT_TRIANGLELIST , 0, m_numWireframeVertex/3);

		V( m_pProteinVistaRenderer->m_pEffectBasicShading->EndPass() );
	}
	V( m_pProteinVistaRenderer->m_pEffectBasicShading->End() );

	GetD3DDevice()->SetRenderState(D3DRS_CULLMODE , D3DCULL_CCW);

	return S_OK;
}
#pragma managed(pop)
HRESULT CRenderWireframeSelection::UpdateAtomSelectionChanged()
{
	DeleteDeviceObjects();
	InitDeviceObjects();

	return S_OK;
}
