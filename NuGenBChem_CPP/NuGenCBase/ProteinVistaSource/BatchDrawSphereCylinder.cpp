#include "StdAfx.h"
#include "ProteinVista.h"


#include "ProteinVistaView.h"
#include "PDBRenderer.h"
#include "ProteinVistaRenderer.h"
#include "Interface.h"
#include "BatchDrawSphereCylinder.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

static D3DVERTEXELEMENT9 g_VertexElemSphereShaderInstancing[] =
{
    { 0, 0,     D3DDECLTYPE_FLOAT3,     D3DDECLMETHOD_DEFAULT,  D3DDECLUSAGE_POSITION,  0 },
    { 0, 3 * 4, D3DDECLTYPE_FLOAT3,     D3DDECLMETHOD_DEFAULT,  D3DDECLUSAGE_NORMAL,    0 },
    { 0, 6 * 4, D3DDECLTYPE_FLOAT1,     D3DDECLMETHOD_DEFAULT,  D3DDECLUSAGE_TEXCOORD,  0 },
	D3DDECL_END()
};

CBatchDrawSphere::CBatchDrawSphere()
{
	m_pMeshBatchVertexBuffer = NULL;
	m_pMeshBatchIndexBuffer = NULL;

	m_arrayInstancePosition = NULL;
	m_arrayInstanceColor = NULL;
	m_arrayInstanceSelection = NULL;

	m_pVertexDeclShader = NULL;

	m_numMaxInstance = 0;
}

CBatchDrawSphere::~CBatchDrawSphere()
{
	DeleteDeviceObjects();

	SAFE_DELETE(m_arrayInstancePosition);
	SAFE_DELETE(m_arrayInstanceColor);
	SAFE_DELETE(m_arrayInstanceSelection);
}

//
//
void CBatchDrawSphere::Init ( CProteinVistaRenderer * pProteinVistaRenderer, FLOAT radius,  LONG resolution )
{
	m_pProteinVistaRenderer = pProteinVistaRenderer;

	m_numBatchInstance = NumBatchInstance;
	m_fRadius = radius;
	m_resolution = resolution;
}

void CBatchDrawSphere::SetInstanceData( CSTLArrayVector4 & arrayInstancePosition, CSTLFLOATArray & arraySelection , CSTLArrayColor & arrayInstanceColor )
{
	m_numMaxInstance = arrayInstancePosition.size();

	SAFE_DELETE(m_arrayInstancePosition);
	m_arrayInstancePosition = new D3DXVECTOR4 [m_numMaxInstance];

	SAFE_DELETE(m_arrayInstanceColor);
	m_arrayInstanceColor = new D3DXCOLOR [m_numMaxInstance];

	SAFE_DELETE(m_arrayInstanceSelection);
	m_arrayInstanceSelection = new D3DXVECTOR4 [m_numMaxInstance];
	ZeroMemory(m_arrayInstanceSelection, sizeof(D3DXVECTOR4) * m_numMaxInstance );

	for ( int i = 0 ; i < m_numMaxInstance; i++ )
	{
		m_arrayInstancePosition[i] = arrayInstancePosition[i];
		m_arrayInstanceSelection[i].x = arraySelection[i];
		m_arrayInstanceColor[i] = arrayInstanceColor[i];
	}
}

HRESULT CBatchDrawSphere::InitDeviceObjects()
{
	//	
	if ( m_numMaxInstance == 0 )	return S_OK;

	HRESULT hr;
	LPD3DXMESH	pAtomMesh = NULL;
	LPD3DXMESH	pSphereInstanceMesh = NULL;
	LPDIRECT3DDEVICE9 pd3dDevice = m_pProteinVistaRenderer->GetD3DDevice();

	V_RETURN(D3DXCreateSphere ( pd3dDevice, m_fRadius, m_resolution, m_resolution, &pAtomMesh , NULL ));
	V_RETURN(pAtomMesh->CloneMesh(D3DXMESH_MANAGED , g_VertexElemSphereShaderInstancing, pd3dDevice, &pSphereInstanceMesh ));
	SAFE_RELEASE(pAtomMesh);

	IDirect3DVertexBuffer9 *		pSphereInstanceVertexBuffer;
	IDirect3DIndexBuffer9 *			pSphereInstanceIndexBuffer;

	hr = pSphereInstanceMesh->GetVertexBuffer(&pSphereInstanceVertexBuffer);
	hr = pSphereInstanceMesh->GetIndexBuffer(&pSphereInstanceIndexBuffer);

	m_numVertexSphere = pSphereInstanceMesh->GetNumVertices();
	m_numFaceSphere = pSphereInstanceMesh->GetNumFaces();
	int numBytesPerVertex = pSphereInstanceMesh->GetNumBytesPerVertex();

	SAFE_RELEASE(pSphereInstanceMesh);

	D3DVERTEXBUFFER_DESC descAtomVB;
	pSphereInstanceVertexBuffer->GetDesc(&descAtomVB);
	D3DINDEXBUFFER_DESC descAtomIB;
	pSphereInstanceIndexBuffer->GetDesc(&descAtomIB);

	LONG numIndexSphere = descAtomIB.Size/sizeof(WORD);

	// First create the vertex declaration we need
	V_RETURN( pd3dDevice->CreateVertexDeclaration( g_VertexElemSphereShaderInstancing, &m_pVertexDeclShader ) );

	//	m_numBatchInstance 를 계산. index buffer (WORD)가 65536을 넘는다.
	for ( int i = min(m_numBatchInstance,m_numMaxInstance); i > 0 ; i-- )
	{
		if ( i * m_numVertexSphere < 65536L )
		{
			break;
		}
	}
	m_numBatchInstance = i;

	// Create m_numBatchInstance copies
	V_RETURN( pd3dDevice->CreateVertexBuffer( min(m_numBatchInstance,m_numMaxInstance) * descAtomVB.Size , 0, 0, D3DPOOL_MANAGED, &m_pMeshBatchVertexBuffer, 0 ) );

	// And an IB to go with it. We will be rendering
	V_RETURN( pd3dDevice->CreateIndexBuffer( min(m_numBatchInstance,m_numMaxInstance) * descAtomIB.Size, 0, descAtomIB.Format, D3DPOOL_MANAGED, &m_pMeshBatchIndexBuffer, 0 ) );

	BATCH_SPHERE_VERTEX_INSTANCE* pSphereVerts;
	hr = pSphereInstanceVertexBuffer->Lock(0, NULL, (void**)&pSphereVerts, 0 );

	BATCH_SPHERE_VERTEX_INSTANCE* pBatchVerts;
	hr = m_pMeshBatchVertexBuffer->Lock( 0, NULL, (void**)&pBatchVerts, 0 );

	WORD * pSphereIndex;
	hr = pSphereInstanceIndexBuffer->Lock(0,NULL, (void**)&pSphereIndex, 0 );

	WORD * pBatchIndex;
	hr = m_pMeshBatchIndexBuffer->Lock(0,NULL, (void**)&pBatchIndex, 0 );

	for ( int i = 0 ; i < min(m_numBatchInstance,m_numMaxInstance) ; i++ )
	{
		//	copy sphere
		CopyMemory( pBatchVerts + i * m_numVertexSphere , pSphereVerts , descAtomVB.Size );

		for ( int j = 0 ; j < m_numVertexSphere ; j++ )
		{
			(*(pBatchVerts + i * m_numVertexSphere + j)).boxInstance = i;
		}

		//	copy index buffer
		//	descAtomIB.Size
		//	CopyMemory( ((BYTE*)pBatchIndex) + i * descAtomIB.Size, pSphereIndex , descAtomIB.Size );
		for ( int j = 0 ; j < numIndexSphere ; j++ )
		{
			*(pBatchIndex+i*numIndexSphere+j) = *(pSphereIndex+j) + i*m_numVertexSphere;
		}
	}

	m_pMeshBatchVertexBuffer->Unlock();
	pSphereInstanceVertexBuffer->Unlock();

	pSphereInstanceIndexBuffer->Unlock();
	m_pMeshBatchIndexBuffer->Unlock();

	SAFE_RELEASE(pSphereInstanceVertexBuffer);
	SAFE_RELEASE(pSphereInstanceIndexBuffer);

	return S_OK; 
}

HRESULT CBatchDrawSphere::DeleteDeviceObjects()
{
	SAFE_RELEASE(m_pVertexDeclShader);

	SAFE_RELEASE(m_pMeshBatchVertexBuffer);
	SAFE_RELEASE(m_pMeshBatchIndexBuffer);

	return S_OK; 
}

HRESULT CBatchDrawSphere::Render()
{
	HRESULT hr;
	UINT iPass, cPasses;

	if ( m_pProteinVistaRenderer == NULL )
		return E_FAIL;

	if ( m_numMaxInstance == 0 )	return S_OK;

	LPDIRECT3DDEVICE9 pd3dDevice = m_pProteinVistaRenderer->GetD3DDevice();

	V( pd3dDevice->SetVertexDeclaration( m_pVertexDeclShader ) );

	// Stream zero is our model
	V( pd3dDevice->SetStreamSource( 0, m_pMeshBatchVertexBuffer, 0, sizeof( BATCH_SPHERE_VERTEX_INSTANCE ) ) );
	V( pd3dDevice->SetIndices( m_pMeshBatchIndexBuffer ) );

	ID3DXEffect * pEffect = m_pProteinVistaRenderer->m_pEffectBasicShading;

	V( pEffect->Begin( &cPasses, 0 ) );
	for( iPass = 0; iPass < cPasses; iPass++ )
	{
		V( pEffect->BeginPass( iPass ) );

		int nRemainingMeshes = m_numMaxInstance;
		while( nRemainingMeshes > 0 )
		{
			// determine how many instances are in this batch (up to g_nNumBatchInstance)           
			int nRenderMeshes = min( nRemainingMeshes, m_numBatchInstance );

			// set the box Instancing array
			m_pProteinVistaRenderer->SetBatchInstancePosition( m_arrayInstancePosition + m_numMaxInstance - nRemainingMeshes, nRenderMeshes);
			m_pProteinVistaRenderer->SetBatchInstanceSelectionRotationXYScale( m_arrayInstanceSelection + m_numMaxInstance - nRemainingMeshes, nRenderMeshes);
			m_pProteinVistaRenderer->SetBatchInstanceColor( m_arrayInstanceColor + m_numMaxInstance - nRemainingMeshes, nRenderMeshes);

			// The effect interface queues up the changes and performs them 
			// with the CommitChanges call. You do not need to call CommitChanges if 
			// you are not setting any parameters between the BeginPass and EndPass.
			V( pEffect->CommitChanges() );

			V( pd3dDevice->DrawIndexedPrimitive( D3DPT_TRIANGLELIST, 0, 0, nRenderMeshes * m_numVertexSphere, 0, nRenderMeshes * m_numFaceSphere ) );

			// subtract the rendered boxes from the remaining boxes
			nRemainingMeshes -= nRenderMeshes;
		}

		V( pEffect->EndPass() );
	}
	V( pEffect->End() );

	return S_OK; 
}

//==============================================================================================================================================

static D3DVERTEXELEMENT9 g_VertexElemCylinderShaderInstancing[] =
{
	{ 0, 0,     D3DDECLTYPE_FLOAT3,     D3DDECLMETHOD_DEFAULT,  D3DDECLUSAGE_POSITION,  0 },
	{ 0, 3 * 4, D3DDECLTYPE_FLOAT3,     D3DDECLMETHOD_DEFAULT,  D3DDECLUSAGE_NORMAL,    0 },
	{ 0, 6 * 4, D3DDECLTYPE_FLOAT1,     D3DDECLMETHOD_DEFAULT,  D3DDECLUSAGE_TEXCOORD,  0 },
	D3DDECL_END()
};

CBatchDrawCylinder::CBatchDrawCylinder()
{
	m_pMeshBatchVertexBuffer = NULL;
	m_pMeshBatchIndexBuffer = NULL;

	m_arrayInstancePosition = NULL;
	m_arrayInstanceSelectionRotationXYScale = NULL;
	m_arrayInstanceColor = NULL;

	m_pVertexDeclShader = NULL;

	m_numMaxInstance = 0;
}

CBatchDrawCylinder::~CBatchDrawCylinder()
{
	DeleteDeviceObjects();

	SAFE_DELETE_ARRAY(m_arrayInstancePosition);
	SAFE_DELETE_ARRAY(m_arrayInstanceColor);
	SAFE_DELETE_ARRAY(m_arrayInstanceSelectionRotationXYScale);
}

//
//
void CBatchDrawCylinder::Init ( CProteinVistaRenderer * pProteinVistaRenderer, FLOAT radius,  LONG resolution )
{
	m_pProteinVistaRenderer = pProteinVistaRenderer;
	//	check size.
	m_numBatchInstance = NumBatchInstance;
	m_fRadius = radius;
	m_resolution = resolution;
}

void CBatchDrawCylinder::SetInstanceData( CSTLArrayVector4 & arrayInstancePosition, CSTLFLOATArray & arrayInstanceSelection , CSTLArrayVector2 & arrayInstanceRotation , CSTLFLOATArray & arrayInstanceScale , CSTLArrayColor & arrayInstanceColor )
{
	m_numMaxInstance = arrayInstancePosition.size();

	SAFE_DELETE_ARRAY(m_arrayInstancePosition);
	m_arrayInstancePosition = new D3DXVECTOR4 [m_numMaxInstance];

	SAFE_DELETE_ARRAY(m_arrayInstanceColor);
	m_arrayInstanceColor = new D3DXCOLOR [m_numMaxInstance];

	SAFE_DELETE_ARRAY(m_arrayInstanceSelectionRotationXYScale);
	m_arrayInstanceSelectionRotationXYScale = new D3DXVECTOR4 [m_numMaxInstance];

	for ( int i = 0 ; i < m_numMaxInstance; i++ )
	{
		m_arrayInstancePosition[i] = arrayInstancePosition[i];
		m_arrayInstanceSelectionRotationXYScale[i].x = arrayInstanceSelection[i];
		m_arrayInstanceSelectionRotationXYScale[i].y = arrayInstanceRotation[i].x;
		m_arrayInstanceSelectionRotationXYScale[i].z = arrayInstanceRotation[i].y;
		m_arrayInstanceSelectionRotationXYScale[i].w = arrayInstanceScale[i];
		m_arrayInstanceColor[i] = arrayInstanceColor[i];
	}
}

HRESULT CBatchDrawCylinder::InitDeviceObjects()
{
	//	
	if ( m_numMaxInstance == 0 )	return S_OK;

	HRESULT hr;
	LPD3DXMESH	pAtomMesh = NULL;
	LPD3DXMESH	pCylinderInstanceMesh = NULL;
	LPDIRECT3DDEVICE9 pd3dDevice = m_pProteinVistaRenderer->GetD3DDevice();

	//	cylinder로 된 bond 만들기.
	//
	long	nFace = m_resolution;	//	vertex == face
	long	nVertex = nFace;
	ID3DXMesh *  pCylinderMesh;
	V_RETURN(D3DXCreateMesh(nFace, nVertex , D3DXMESH_MANAGED, g_VertexElemCylinderShaderInstancing, pd3dDevice , &pCylinderMesh));

	BATCH_CYLINDER_VERTEX_INSTANCE	* pvertex;
	pCylinderMesh->LockVertexBuffer(0, (LPVOID*) &pvertex);
	for ( long i = 0; i < nVertex/2 ; i++ )
	{
		FLOAT theta = (2*D3DX_PI*i*2)/(nFace-1);

		pvertex[2*i+0].pos = D3DXVECTOR3( sinf(theta)*m_fRadius, cosf(theta)*m_fRadius, 0.0f );
		pvertex[2*i+1].pos = D3DXVECTOR3( sinf(theta)*m_fRadius, cosf(theta)*m_fRadius, 1.0f );
	}
	pCylinderMesh->UnlockVertexBuffer();

	WORD* pIndex;
	pCylinderMesh->LockIndexBuffer(0, (LPVOID*) &pIndex);
	for ( i =0 ; i < nFace ; i++ )
	{
		if ( i%2 == 0 )
		{
			*(pIndex+i*3)= i%nFace;
			*(pIndex+i*3+1)= (i+1)%nFace;
			*(pIndex+i*3+2)= (i+1+2)%nFace;
		}
		else
		{
			*(pIndex+i*3)= (i-1)%nFace;
			*(pIndex+i*3+1)= (i+3-1)%nFace;
			*(pIndex+i*3+2)= (i+2-1)%nFace;
		}
	}
	pCylinderMesh->UnlockIndexBuffer();
	D3DXComputeNormals(pCylinderMesh, NULL );

	m_numVertexCylinder = pCylinderMesh->GetNumVertices();
	m_numFaceCylinder = pCylinderMesh->GetNumFaces();

	IDirect3DVertexBuffer9 *		pCylinderInstanceVertexBuffer;
	IDirect3DIndexBuffer9 *			pCylinderInstanceIndexBuffer;

	hr = pCylinderMesh->GetVertexBuffer(&pCylinderInstanceVertexBuffer);
	hr = pCylinderMesh->GetIndexBuffer(&pCylinderInstanceIndexBuffer);

	m_numVertexCylinder = pCylinderMesh->GetNumVertices();
	m_numFaceCylinder = pCylinderMesh->GetNumFaces();
	int numBytesPerVertex = pCylinderMesh->GetNumBytesPerVertex();

	SAFE_RELEASE(pCylinderMesh);

	//	
	//
	//	
	D3DVERTEXBUFFER_DESC descAtomVB;
	pCylinderInstanceVertexBuffer->GetDesc(&descAtomVB);
	D3DINDEXBUFFER_DESC descAtomIB;
	pCylinderInstanceIndexBuffer->GetDesc(&descAtomIB);

	LONG numIndexCylinder = descAtomIB.Size/sizeof(WORD);

	// First create the vertex declaration we need
	V_RETURN( pd3dDevice->CreateVertexDeclaration( g_VertexElemCylinderShaderInstancing, &m_pVertexDeclShader ) );

	//	m_numBatchInstance 를 계산. index buffer (WORD)가 65536을 넘는다.
	for ( int i = min(m_numBatchInstance,m_numMaxInstance); i > 0 ; i-- )
	{
		if ( i * m_numVertexCylinder < 65536L )
		{
			break;
		}
	}
	m_numBatchInstance = i;

	// Create m_numBatchInstance copies
	V_RETURN( pd3dDevice->CreateVertexBuffer( min(m_numBatchInstance,m_numMaxInstance) * descAtomVB.Size , 0, 0, D3DPOOL_MANAGED, &m_pMeshBatchVertexBuffer, 0 ) );

	// And an IB to go with it. We will be rendering
	V_RETURN( pd3dDevice->CreateIndexBuffer( min(m_numBatchInstance,m_numMaxInstance) * descAtomIB.Size, 0, descAtomIB.Format, D3DPOOL_MANAGED, &m_pMeshBatchIndexBuffer, 0 ) );

	BATCH_SPHERE_VERTEX_INSTANCE* pCylinderVerts;
	hr = pCylinderInstanceVertexBuffer->Lock(0, NULL, (void**)&pCylinderVerts, 0 );

	BATCH_SPHERE_VERTEX_INSTANCE* pBatchVerts;
	hr = m_pMeshBatchVertexBuffer->Lock( 0, NULL, (void**)&pBatchVerts, 0 );

	WORD * pCylinderIndex;
	hr = pCylinderInstanceIndexBuffer->Lock(0,NULL, (void**)&pCylinderIndex, 0 );

	WORD * pBatchIndex;
	hr = m_pMeshBatchIndexBuffer->Lock(0,NULL, (void**)&pBatchIndex, 0 );

	for ( int i = 0 ; i < min(m_numBatchInstance,m_numMaxInstance) ; i++ )
	{
		//	copy Cylinder
		CopyMemory( pBatchVerts + i * m_numVertexCylinder , pCylinderVerts , descAtomVB.Size );

		for ( int j = 0 ; j < m_numVertexCylinder ; j++ )
		{
			(*(pBatchVerts + i * m_numVertexCylinder + j)).boxInstance = i;
		}

		//	copy index buffer
		//	descAtomIB.Size
		//	CopyMemory( ((BYTE*)pBatchIndex) + i * descAtomIB.Size, pCylinderIndex , descAtomIB.Size );
		for ( int j = 0 ; j < numIndexCylinder ; j++ )
		{
			*(pBatchIndex+i*numIndexCylinder+j) = *(pCylinderIndex+j) + i*m_numVertexCylinder;
		}
	}

	m_pMeshBatchVertexBuffer->Unlock();
	pCylinderInstanceVertexBuffer->Unlock();

	pCylinderInstanceIndexBuffer->Unlock();
	m_pMeshBatchIndexBuffer->Unlock();

	SAFE_RELEASE(pCylinderInstanceVertexBuffer);
	SAFE_RELEASE(pCylinderInstanceIndexBuffer);

	return S_OK; 
}

HRESULT CBatchDrawCylinder::DeleteDeviceObjects()
{
	SAFE_RELEASE(m_pVertexDeclShader);

	SAFE_RELEASE(m_pMeshBatchVertexBuffer);
	SAFE_RELEASE(m_pMeshBatchIndexBuffer);

	return S_OK; 
}

HRESULT CBatchDrawCylinder::Render()
{
	HRESULT hr;
	UINT iPass, cPasses;

	if ( m_pProteinVistaRenderer == NULL )
		return E_FAIL;

	if ( m_numMaxInstance == 0 )	return S_OK;

	LPDIRECT3DDEVICE9 pd3dDevice = m_pProteinVistaRenderer->GetD3DDevice();

	V( pd3dDevice->SetVertexDeclaration( m_pVertexDeclShader ) );

	// Stream zero is our model
	V( pd3dDevice->SetStreamSource( 0, m_pMeshBatchVertexBuffer, 0, sizeof( BATCH_SPHERE_VERTEX_INSTANCE ) ) );
	V( pd3dDevice->SetIndices( m_pMeshBatchIndexBuffer ) );

	ID3DXEffect * pEffect = m_pProteinVistaRenderer->m_pEffectBasicShading;

	V( pEffect->Begin( &cPasses, 0 ) );
	for( iPass = 0; iPass < cPasses; iPass++ )
	{
		V( pEffect->BeginPass( iPass ) );

		int nRemainingMeshes = m_numMaxInstance;
		while( nRemainingMeshes > 0 )
		{
			// determine how many instances are in this batch (up to g_nNumBatchInstance)           
			int nRenderMeshes = min( nRemainingMeshes, m_numBatchInstance );

			// set the box Instancing array
			m_pProteinVistaRenderer->SetBatchInstancePosition( m_arrayInstancePosition + m_numMaxInstance - nRemainingMeshes, nRenderMeshes);
			m_pProteinVistaRenderer->SetBatchInstanceSelectionRotationXYScale(m_arrayInstanceSelectionRotationXYScale + m_numMaxInstance - nRemainingMeshes, nRenderMeshes);
			m_pProteinVistaRenderer->SetBatchInstanceColor( m_arrayInstanceColor + m_numMaxInstance - nRemainingMeshes, nRenderMeshes);

			// The effect interface queues up the changes and performs them 
			// with the CommitChanges call. You do not need to call CommitChanges if 
			// you are not setting any parameters between the BeginPass and EndPass.
			V( pEffect->CommitChanges() );

			V( pd3dDevice->DrawIndexedPrimitive( D3DPT_TRIANGLELIST, 0, 0, nRenderMeshes * m_numVertexCylinder, 0, nRenderMeshes * m_numFaceCylinder ) );

			// subtract the rendered boxes from the remaining boxes
			nRemainingMeshes -= nRenderMeshes;
		}

		V( pEffect->EndPass() );
	}
	V( pEffect->End() );

	return S_OK; 
}




