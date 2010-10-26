//-----------------------------------------------------------------------------
// File: CoordinateAxis.cpp
//
// Desc: 
//-----------------------------------------------------------------------------

#include "StdAfx.h"
#include "ProteinVista.h"
#include "Interface.h"

#include "ProteinVistaView.h"
#include "ProteinVistaRenderer.h"
#include "CoordinateAxis.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//	CCoordinateAxisDisplay
//


CCoordinateAxisDisplay::CCoordinateAxisDisplay()
{
	// 초기화
	m_pProteinVistaRenderer	= NULL;
	m_pMesh				= NULL;
	m_pDeclAxis			= NULL;

	D3DXMatrixIdentity(&m_matWorldPDB);
	D3DXMatrixIdentity(&m_matScale);

	m_center = D3DXVECTOR3(0,0,0);
}


void CCoordinateAxisDisplay::Init(CProteinVistaRenderer * pProteinVistaRenderer)
{
	m_pProteinVistaRenderer = pProteinVistaRenderer;											
}
#pragma managed(push,off)
HRESULT CCoordinateAxisDisplay::InitDeviceObjects()
{	
	// 메쉬 생성 - 화살표 모양
	 CString meshFileName = GetMainApp()->m_strBaseResPath + "arrow.x";
	if(FAILED(D3DXLoadMeshFromX(meshFileName, D3DXMESH_MANAGED, GetD3DDevice(), NULL, NULL, NULL, NULL, &m_pMesh)))
		return E_FAIL; 

	numFaces = m_pMesh->GetNumFaces();
	numVertices = m_pMesh->GetNumVertices();

	D3DXMATRIXA16	matIden;		

	// X축 행렬 설정
	FLOAT	transDelta = 2.55f;
	D3DXMATRIXA16 rot, trans, scale;
	D3DXMatrixIdentity(&rot);
	D3DXMatrixScaling(&scale, 0.004,0.01,0.004);
	{
		D3DXMatrixTranslation(&trans, transDelta ,0,0);
		D3DXMatrixRotationZ(&rot, D3DX_PI/ 2.0f );
		m_matAxisX = scale * rot * trans ;
	}

	// Y축 행렬 설정
	{
		D3DXMatrixTranslation(&trans, 0,transDelta,0);
		D3DXMatrixRotationX(&rot, -D3DX_PI );
		m_matAxisY = scale * rot * trans ;
	}

	// Z축 행렬 설정
	{
		D3DXMatrixTranslation(&trans, 0,0,transDelta);
		D3DXMatrixRotationX(&rot, D3DX_PI/ -2.0f );
		m_matAxisZ = scale * rot * trans ;
	}

	D3DVERTEXELEMENT9	decl[MAX_FVF_DECL_SIZE];
	D3DXDeclaratorFromFVF( CVertexAxis::FVF, decl);
	GetD3DDevice()->CreateVertexDeclaration(decl, &m_pDeclAxis);

	return S_OK;
}
#pragma managed(pop)
HRESULT CCoordinateAxisDisplay::SetModelTransform(D3DXVECTOR3 & center, D3DXMATRIXA16 & matWorld)
{
	m_center = center;
	m_matWorldPDB = matWorld;
	return S_OK;
}

HRESULT CCoordinateAxisDisplay::SetModelScale(D3DXMATRIXA16 & matScale)
{
	m_matScale = matScale;
	return S_OK;
}
#pragma managed(push,off)
HRESULT CCoordinateAxisDisplay::Render()
{	
	HRESULT hr;

	if ( m_pMesh == NULL )
		return E_FAIL;

	D3DXMATRIXA16 * pMatView = m_pProteinVistaRenderer->GetViewMatrix();
	D3DXMATRIXA16 * pMatProj = m_pProteinVistaRenderer->GetProjMatrix();

	GetD3DDevice()->SetVertexDeclaration( m_pDeclAxis );
	m_pProteinVistaRenderer->SetShaderTechnique(CProteinVistaRenderer::AxisRendering);

	LPDIRECT3DVERTEXBUFFER9 vertexBuffer;
	m_pMesh->GetVertexBuffer(&vertexBuffer);
	GetD3DDevice()->SetStreamSource( 0, vertexBuffer, 0, sizeof(CVertexAxis) );
	vertexBuffer->Release();

	LPDIRECT3DINDEXBUFFER9 indexBuffer;
	m_pMesh->GetIndexBuffer(&indexBuffer);
	GetD3DDevice()->SetIndices(indexBuffer);
	indexBuffer->Release();

	//
	//	X-Axis
	//
	m_pProteinVistaRenderer->SetShaderDiffuseColor(D3DXCOLOR(1,0,0,0));
	D3DXMATRIXA16 matLocal = m_matAxisX;

	D3DXMATRIXA16 worldRot = m_matWorldPDB;
	worldRot._41 = worldRot._42 = worldRot._43 = 0.0f;

	D3DXMATRIXA16 worldTrans;
	D3DXMatrixIdentity(&worldTrans);
	worldTrans._41 = m_center.x;
	worldTrans._42 = m_center.y;
	worldTrans._43 = m_center.z;

	D3DXMATRIXA16 matWorldSRT = m_matScale * worldRot * worldTrans;

	D3DXMATRIXA16 matWorldTransform = matLocal * matWorldSRT ;
	D3DXMATRIXA16 matWorldView = matWorldTransform * (*pMatView);
	D3DXMATRIXA16 matWorldViewProj = matWorldView * (*pMatProj);
	m_pProteinVistaRenderer->SetShaderWorldMatrix(matWorldTransform);
	m_pProteinVistaRenderer->SetShaderWorldViewMatrix(matWorldView);
	m_pProteinVistaRenderer->SetShaderWorldViewProjMatrix(matWorldViewProj);

	UINT cPasses;
	V( m_pProteinVistaRenderer->m_pEffectBasicShading->Begin(&cPasses, 0) );
	for (long iPass = 0; iPass < cPasses; iPass++)
	{
		V( m_pProteinVistaRenderer->m_pEffectBasicShading->BeginPass(iPass) );

		GetD3DDevice()->DrawIndexedPrimitive(D3DPT_TRIANGLELIST, 0, 0, numVertices, 0, numFaces);

		V( m_pProteinVistaRenderer->m_pEffectBasicShading->EndPass() );
	}
	V( m_pProteinVistaRenderer->m_pEffectBasicShading->End() );

	//
	//	Y-Axis
	//
	m_pProteinVistaRenderer->SetShaderDiffuseColor(D3DXCOLOR(0,1,0,0));
	matLocal = m_matAxisY;

	matWorldTransform = matLocal * matWorldSRT ;
	matWorldView = matWorldTransform * (*pMatView);
	matWorldViewProj = matWorldView * (*pMatProj);
	m_pProteinVistaRenderer->SetShaderWorldMatrix(matWorldTransform);
	m_pProteinVistaRenderer->SetShaderWorldViewMatrix(matWorldView);
	m_pProteinVistaRenderer->SetShaderWorldViewProjMatrix(matWorldViewProj);

	V( m_pProteinVistaRenderer->m_pEffectBasicShading->Begin(&cPasses, 0) );
	for (long iPass = 0; iPass < cPasses; iPass++)
	{
		V( m_pProteinVistaRenderer->m_pEffectBasicShading->BeginPass(iPass) );

		GetD3DDevice()->DrawIndexedPrimitive(D3DPT_TRIANGLELIST, 0, 0, numVertices, 0, numFaces);

		V( m_pProteinVistaRenderer->m_pEffectBasicShading->EndPass() );
	}
	V( m_pProteinVistaRenderer->m_pEffectBasicShading->End() );

	//
	//	Z-Axis
	//
	m_pProteinVistaRenderer->SetShaderDiffuseColor(D3DXCOLOR(0,0,1,0));
	matLocal = m_matAxisZ;

	matWorldTransform = matLocal * matWorldSRT ;
	matWorldView = matWorldTransform * (*pMatView);
	matWorldViewProj = matWorldView * (*pMatProj);
	m_pProteinVistaRenderer->SetShaderWorldMatrix(matWorldTransform);
	m_pProteinVistaRenderer->SetShaderWorldViewMatrix(matWorldView);
	m_pProteinVistaRenderer->SetShaderWorldViewProjMatrix(matWorldViewProj);

	V( m_pProteinVistaRenderer->m_pEffectBasicShading->Begin(&cPasses, 0) );
	for (long iPass = 0; iPass < cPasses; iPass++)
	{
		V( m_pProteinVistaRenderer->m_pEffectBasicShading->BeginPass(iPass) );

		GetD3DDevice()->DrawIndexedPrimitive(D3DPT_TRIANGLELIST, 0, 0, numVertices, 0, numFaces);

		V( m_pProteinVistaRenderer->m_pEffectBasicShading->EndPass() );
	}

	V( m_pProteinVistaRenderer->m_pEffectBasicShading->End() );

	return S_OK;
}
#pragma managed(pop)
HRESULT CCoordinateAxisDisplay::DeleteDeviceObjects()
{
	SAFE_RELEASE(m_pMesh);	
	SAFE_RELEASE(m_pDeclAxis);

	return S_OK;
}


