// SkyBox를 Sphere로 구현

#include "stdafx.h"
#include "ProteinVista.h"


#include "ProteinVistaView.h"
#include "ProteinVistaRenderer.h"
#include "SkyBox.h"
#include "RenderProperty.h"
#include "Interface.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//	CSkyManager
//

CSkyManager::CSkyManager(CProteinVistaRenderer * pRenderer)
{
	m_pProteinVistaRenderer = pRenderer;

	m_pSkyBoxTexture = NULL;

	m_pSkyBox  = NULL;
}

HRESULT CSkyManager::InitDeviceObjects()
{
	HRESULT	hr;

	m_strTextureFilename = m_pProteinVistaRenderer->m_pPropertyScene->m_strBackgroundTextureFilename;
	m_pSkyBoxTexture = m_pProteinVistaRenderer->GetTexture(m_strTextureFilename);

	//	float farClip = m_pProteinVistaRenderer->m_fFarClipPlane;

	m_pSkyBox = CreateMappedSphere(GetD3DDevice(), 1.0f , 16, 16, 0 );
	
	ChangeSphereColor();

	return S_OK;
}

HRESULT CSkyManager::DeleteDeviceObjects()
{
	m_pProteinVistaRenderer->ReleaseTexture(m_strTextureFilename);
	SAFE_RELEASE(m_pSkyBox);

	return S_OK;
}

HRESULT CSkyManager::RestoreDeviceObjects()
{
	D3DVERTEXELEMENT9	decl[MAX_FVF_DECL_SIZE];
	D3DXDeclaratorFromFVF( FVF_SKYBOX_VERTEX, decl);
	GetD3DDevice()->CreateVertexDeclaration(decl, &m_pDeclSkybox);

	return S_OK;
}

HRESULT CSkyManager::FrameMove()
{
	return S_OK;
}
#pragma managed(push,off)
HRESULT CSkyManager::Render()
{
	HRESULT hr;

	if ( m_pSkyBox == NULL ) return S_OK;

	//	change shader version.
	//	alpha값이 255 로 써진다.
	//	texture 값때문.

	LPDIRECT3DDEVICE9 pDevice = GetD3DDevice();

	float farClip = m_pProteinVistaRenderer->m_fFarClipPlane;

	D3DXMATRIXA16 matTrans;
	D3DXMatrixTranslation(&matTrans, m_pProteinVistaRenderer->m_FromVec.x, m_pProteinVistaRenderer->m_FromVec.y, m_pProteinVistaRenderer->m_FromVec.z );
	D3DXMATRIXA16 matScale;
	D3DXMatrixScaling(&matScale, farClip, farClip, farClip );

	D3DXMATRIXA16	matWorld = matScale * matTrans;

	pDevice->SetTransform(D3DTS_WORLD, &matWorld);

	pDevice->SetTexture(0, m_pSkyBoxTexture);

	D3DXMATRIXA16 matWorldViewProj = matWorld * (*m_pProteinVistaRenderer->GetViewMatrix()) *(* m_pProteinVistaRenderer->GetProjMatrix() );
	m_pProteinVistaRenderer->SetShaderWorldViewProjMatrix( matWorldViewProj );

	pDevice->SetRenderState(D3DRS_ZENABLE, FALSE);
	pDevice->SetRenderState(D3DRS_ZWRITEENABLE, FALSE);

	pDevice->SetRenderState(D3DRS_CULLMODE, D3DCULL_NONE);
	pDevice->SetRenderState(D3DRS_LIGHTING, FALSE);

	pDevice->SetVertexDeclaration( m_pDeclSkybox );
	pDevice->SetFVF(FVF_SKYBOX_VERTEX);

	m_pProteinVistaRenderer->SetShaderTechnique(CProteinVistaRenderer::SkyBoxRendering);
	m_pProteinVistaRenderer->SetShaderSelectionTexture(m_pSkyBoxTexture);

	UINT cPasses;
	V( m_pProteinVistaRenderer->m_pEffectBasicShading->Begin(&cPasses, 0) );
	for (long iPass = 0; iPass < cPasses; iPass++)
	{
		V( m_pProteinVistaRenderer->m_pEffectBasicShading->BeginPass(iPass) );

		m_pSkyBox->DrawSubset(0);

		V( m_pProteinVistaRenderer->m_pEffectBasicShading->EndPass() );
	}
	V( m_pProteinVistaRenderer->m_pEffectBasicShading->End() );

	pDevice->SetRenderState(D3DRS_LIGHTING, TRUE );
	pDevice->SetRenderState(D3DRS_CULLMODE, D3DCULL_CCW);

	pDevice->SetRenderState(D3DRS_ZENABLE, TRUE);
	pDevice->SetRenderState(D3DRS_ZWRITEENABLE, TRUE);
	
	pDevice->SetTexture(0, NULL);

	return S_OK;
}
#pragma managed(pop)

HRESULT CSkyManager::InvalidateDeviceObjects()
{
	SAFE_RELEASE(m_pDeclSkybox);

	return S_OK; 
}

HRESULT CSkyManager::FinalCleanup()
{
	return S_OK;
}

void CSkyManager::ChangeSphereColor()
{
	if ( m_pProteinVistaRenderer )
	{
		COLORREF color1 = m_pProteinVistaRenderer->m_pPropertyScene->m_colorBackroundColor;
		D3DCOLOR d3dBackColor = m_pProteinVistaRenderer->m_pPropertyScene->m_d3dcolorBackroundColor = D3DCOLOR_ARGB(0, GetRValue(color1), GetGValue(color1), GetBValue(color1));

		LPDIRECT3DDEVICE9 pDev = GetD3DDevice();

		CSkyboxVertex * pVerts;
		if (SUCCEEDED(m_pSkyBox->LockVertexBuffer(0,(void**) &pVerts))) 
		{
			int numVerts=m_pSkyBox->GetNumVertices();

			for (int i=0;i<numVerts;i++) 
			{
				pVerts->col=d3dBackColor;
				pVerts++;
			}

			m_pSkyBox->UnlockVertexBuffer();
		}
	}
}

LPD3DXMESH CSkyManager::CreateMappedSphere(LPDIRECT3DDEVICE9 pDev, float fRad, UINT slices, UINT stacks, DWORD col)
{
	// create the sphere
	LPD3DXMESH mesh;
	if (FAILED(D3DXCreateSphere(pDev, fRad, slices, stacks, &mesh, NULL)))
		return NULL;

	// create a copy of the mesh with texture coordinates,
	// since the D3DX function doesn't include them
	LPD3DXMESH texMesh;
	if (FAILED(mesh->CloneMeshFVF(D3DXMESH_MANAGED, FVF_SKYBOX_VERTEX, pDev, &texMesh)))
		// failed, return un-textured mesh
		return mesh;

	// finished with the original mesh, release it
	mesh->Release();

	// lock the vertex buffer
	CSkyboxVertex	* pVerts;
	if (SUCCEEDED(texMesh->LockVertexBuffer(0,(void**) &pVerts))) 
	{
		// get vertex count
		int numVerts=texMesh->GetNumVertices();

		// loop through the vertices
		for (int i=0;i<numVerts;i++) 
		{

			// calculate normal
			D3DXVECTOR3 norm;
			D3DXVec3Normalize(&norm,(D3DXVECTOR3 *) pVerts);

			// calculate texture coordinates
			pVerts->tu=asinf(norm.x)/D3DX_PI+0.5f;
			pVerts->tv=asinf(norm.y)/D3DX_PI+0.5f;

			pVerts->col=col;

			// go to next vertex
			pVerts++;
		}

		// unlock the vertex buffer
		texMesh->UnlockVertexBuffer();
	}

	// return pointer to caller
	return texMesh;
}
