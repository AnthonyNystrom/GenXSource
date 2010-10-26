#include "stdafx.h"
#include "ProteinVista.h"


#include "ProteinVistaView.h"

#include "PDBRenderer.h"
#include "ProteinVistaRenderer.h"

#include "SelectionDisplay.h"
#include "RenderSurfaceSelection.h"
#include "Interface.h"

#include "ProteinSurfaceBase.h"
#include "ProteinSurfaceMSMS.h"
#include "ProteinSurfaceMQ.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif



//
//	usage가 여러개일때, 맨뒤에 0,1,2, 인덱스 사용
//	
static D3DVERTEXELEMENT9 g_VertexElemSurface[] =
{
	{ 0, 0				, D3DDECLTYPE_FLOAT3,	D3DDECLMETHOD_DEFAULT,  D3DDECLUSAGE_POSITION,  0 },
	{ 0, 3*4			, D3DDECLTYPE_FLOAT3,	D3DDECLMETHOD_DEFAULT,  D3DDECLUSAGE_NORMAL,    0 },
	{ 0, 3*4+3*4		, D3DDECLTYPE_D3DCOLOR,	D3DDECLMETHOD_DEFAULT,  D3DDECLUSAGE_COLOR ,    0 },
	D3DDECL_END()
};

CRenderSurfaceSelection::CRenderSurfaceSelection()
{
	m_pProteinSurface = NULL; 

	m_iDisplaySelection = -1; 
	m_pIB = NULL; 
	m_pVB = NULL; 

	m_pDeclSurface = NULL;

	m_pPropertySurface = NULL;

	m_pIBSelection = NULL;
	m_sizeIndexBuffer = 0;

}

CRenderSurfaceSelection::~CRenderSurfaceSelection()
{ 
	DeleteDeviceObjects();
}

void CRenderSurfaceSelection::SetModelQuality()
{
	//    notify message.
	long	surface = m_pProteinVistaRenderer->m_renderQualityPreset.m_surfaceQualityConst[m_pPropertyCommon->m_modelQuality];

	//    model quality
	//m_pPropertySurface->m_pSurfaceQuality->SetEnum(surface);
	//m_pPropertySurface->m_pSurfaceQuality->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(m_pPropertySurface->m_pSurfaceQuality));
}


HRESULT CRenderSurfaceSelection::InitDeviceObjects()
{
	DeleteDeviceObjects();

	CSelectionDisplay * pSelectionDisplay = m_pProteinVistaRenderer->m_arraySelectionDisplay[m_iDisplaySelection];
	m_pPropertyCommon = pSelectionDisplay->GetPropertyCommon();
	m_pPropertySurface = pSelectionDisplay->GetPropertySurface();

	//	surface 에 참여하는 atom Inst를 구한다.
	m_pProteinSurface->GetSurfaceAtomInst(m_pChainInst, m_arrayAtomInst);

	//	vertex declation.
	//    D3DVERTEXELEMENT9	decl[MAX_FVF_DECL_SIZE];
	//    D3DXDeclaratorFromFVF( SurfaceVertexFVF, decl);
	GetD3DDevice()->CreateVertexDeclaration(g_VertexElemSurface , &m_pDeclSurface);

	CSTLLongArray indexBuffer;
	GetIndexBuffer(indexBuffer, FALSE );

	m_sizeIndexBuffer = indexBuffer.size();
	if ( m_sizeIndexBuffer == 0) return S_FALSE;
	
	m_indexBufferOrig.clear();
	//	Render() 로 옮겨짐. 실제 사용할때 alloc.
	// 	m_indexBufferOrig.resize(m_sizeIndexBuffer);
	// 	m_indexBufferSorted.resize(m_sizeIndexBuffer);
	// 	CopyMemory(&m_indexBufferOrig[0], &indexBuffer[0], indexBuffer.size()*sizeof(DWORD));

	if ( m_pProteinSurface->m_arrayVertex.size() <= 0xffff )
	{
		m_formatIndexBuffer = D3DFMT_INDEX16;
		m_byteSizeIndexBuffer  = sizeof(WORD);
	}
	else
	{
		m_formatIndexBuffer = m_pProteinVistaRenderer->m_formatIndexBuffer;
		m_byteSizeIndexBuffer  = m_pProteinVistaRenderer->m_byteSizeIndexBuffer;
	}

	GetD3DDevice()->CreateIndexBuffer(indexBuffer.size()*m_byteSizeIndexBuffer, 0, m_formatIndexBuffer , D3DPOOL_MANAGED, &(m_pIB), NULL );
	if ( m_byteSizeIndexBuffer == sizeof(DWORD) )
	{
		DWORD * ib;
		m_pIB->Lock(0,0, (VOID**) &ib, 0 );
		CopyMemory(ib, &indexBuffer[0], indexBuffer.size()*sizeof(DWORD));
		m_pIB->Unlock();
	}
	else
	{
		WORD * ib;
		m_pIB->Lock(0,0, (VOID**) &ib, 0 );
		for ( int i = 0 ; i < indexBuffer.size(); i++ )
		{
			ib[i] = (WORD)indexBuffer[i];
		}
		m_pIB->Unlock();
	}


	//	vertex buffer는 m_pProteinSurface 꺼를 그대로 쓴다.
	//	Vertex Buffer를 만들어서 copy 해 놓는다.
	HRESULT hr = GetD3DDevice()->CreateVertexBuffer(m_pProteinSurface->m_arrayVertex.size()*sizeof(CSurfaceVertex), 0, 0 , D3DPOOL_MANAGED, &(m_pVB), NULL );

	//	CProteinSurface에서 vertex buffer를 copy 한다.
	{
		CSurfaceVertex * pSurfaceVertex = NULL;
		m_pVB->Lock(0, 0, (void**) &pSurfaceVertex, 0);

		for ( long i = 0 ; i < m_pProteinSurface->m_arrayVertex.size() ; i++ )
		{
			pSurfaceVertex[i].pos = m_pProteinSurface->m_arrayVertex[i];
			pSurfaceVertex[i].normal = m_pProteinSurface->m_arrayNormal[i];
		}

		m_pVB->Unlock();
	}

	//
	//	m_arrayVertexCenter 에 vertex center값을 전부 넣어둔다.
	//
	{
		long numIndex = indexBuffer.size();

		m_arrayVertexCenter.clear();
		m_arrayVertexCenter.resize(numIndex/3);
		m_arrayVertexCenterTr.resize(numIndex/3);

		D3DXVECTOR3 vecCenter(0,0,0);
		for ( int i = 0 ; i < numIndex ; i+=3 )
		{
			D3DXVECTOR3 vec1 = m_pProteinSurface->m_arrayVertex[indexBuffer[i]];
			D3DXVECTOR3 vec2 = m_pProteinSurface->m_arrayVertex[indexBuffer[i+1]];
			D3DXVECTOR3 vec3 = m_pProteinSurface->m_arrayVertex[indexBuffer[i+2]];
			vecCenter = vec1 + vec2 + vec3;
			vecCenter /= 3;
			m_arrayVertexCenter[i/3] = vecCenter;
		}
	}

	SetVertexColor();
	UpdateAtomSelectionChanged();

	return S_OK;
}

HRESULT CRenderSurfaceSelection::DeleteDeviceObjects()
{
	m_arrayVertexCenter.clear();
	m_arrayVertexCenterTr.clear();
	m_indexBufferOrig.clear();

	SAFE_RELEASE(m_pIB); 
	SAFE_RELEASE(m_pIBSelection);

	SAFE_RELEASE(m_pVB); 
	SAFE_RELEASE(m_pDeclSurface);

	m_sizeIndexBuffer = 0;
	m_sizeIndexBufferSelection = 0;

	m_indexBufferSorted.clear();

	return S_OK;
}

#define _SWAP(t,x,y)	{ t = x; x = y; y = t; }

void CRenderSurfaceSelection::DepthSort(D3DXVECTOR3 array[], int l, int r)
{ 
	int i = l-1, j = r; 
	D3DXVECTOR3 v = array[r];

	if (r <= l) return;

	for (;;)
	{
		while (array[++i].z > v.z);
		while (v.z > array[--j].z) 
			if (j == l) break;

		if (i >= j) break;

		{
			D3DXVECTOR3 temp;
			_SWAP(temp, array[i], array[j]);

			DWORD iTemp;
			_SWAP(iTemp, m_indexBufferSorted[i*3], m_indexBufferSorted[j*3]);
			_SWAP(iTemp, m_indexBufferSorted[i*3+1], m_indexBufferSorted[j*3+1]);
			_SWAP(iTemp, m_indexBufferSorted[i*3+2], m_indexBufferSorted[j*3+2]);
		}
	}
	{
		D3DXVECTOR3 temp;
		_SWAP(temp, array[i], array[r]);

		DWORD iTemp;
		_SWAP(iTemp, m_indexBufferSorted[i*3], m_indexBufferSorted[r*3]);
		_SWAP(iTemp, m_indexBufferSorted[i*3+1], m_indexBufferSorted[r*3+1]);
		_SWAP(iTemp, m_indexBufferSorted[i*3+2], m_indexBufferSorted[r*3+2]);
	}

	DepthSort(array, l, i-1);
	DepthSort(array, i+1, r);
}
#pragma managed(push,off)
HRESULT CRenderSurfaceSelection::Render()
{
	HRESULT hr;

	if ( m_pVB == NULL )	//	
		return E_FAIL;

	CSelectionDisplay * pSelectionDisplay = m_pProteinVistaRenderer->m_arraySelectionDisplay[m_iDisplaySelection];
	if ( pSelectionDisplay == NULL )
		return E_FAIL;

	int  DisplayMethod = m_pPropertySurface->m_enumSurfaceDisplayMethod;
	long nTransparency = m_pPropertySurface->m_transparency;		//	0 이면 불투명, 1이면 완전 투명.

	int currentRenderMode = D3DFILL_SOLID;
	int currentSelectionMode = D3DFILL_WIREFRAME;
	if ( DisplayMethod == 1 )
	{
		currentRenderMode = D3DFILL_WIREFRAME;
		currentSelectionMode = D3DFILL_WIREFRAME;
	}
	else
	if ( DisplayMethod == 2 )
	{
		currentRenderMode = D3DFILL_POINT;
		currentSelectionMode = D3DFILL_POINT;
	}

	GetD3DDevice()->SetRenderState(D3DRS_FILLMODE, currentRenderMode );

	//
	//	transparent depth sorting.
	//	
	if ( DisplayMethod == 0 && nTransparency < 100 )
	{
		if ( m_pPropertySurface->m_bSurfaceDepthSort == TRUE )
		{
			//
			//	m_indexBufferSorted 를 설정한다.
			//
			if ( m_indexBufferOrig.size() == 0 )
			{
		 		m_indexBufferOrig.resize(m_sizeIndexBuffer);
		 		m_indexBufferSorted.resize(m_sizeIndexBuffer);

				CSTLLongArray indexBuffer;
				GetIndexBuffer(indexBuffer, FALSE );

		 		CopyMemory(&m_indexBufferOrig[0], &indexBuffer[0], indexBuffer.size()*sizeof(DWORD));
			}

			//
			//
			D3DXMATRIXA16 &matModel = m_pPDBRenderer->m_matWorld;
			D3DXMATRIXA16 & matView = (*m_pProteinVistaRenderer->GetViewMatrix());

			D3DXMATRIXA16 matWorldView = matModel * (*m_pProteinVistaRenderer->GetViewMatrix());

			//	view space로 변환.
			D3DXVec3TransformCoordArray(&m_arrayVertexCenterTr[0], sizeof(D3DXVECTOR3), &m_arrayVertexCenter[0], sizeof(D3DXVECTOR3), &matWorldView , m_arrayVertexCenter.size() );
			
			CopyMemory(&m_indexBufferSorted[0], &m_indexBufferOrig[0], sizeof(long)*m_sizeIndexBuffer);

			//	sorting할때 index buffer를 바꾸어, m_indexBufferSorted 를 설정한다.
			DepthSort(&m_arrayVertexCenterTr[0], 0, m_arrayVertexCenterTr.size()-1);

			if ( m_byteSizeIndexBuffer == sizeof(DWORD) )
			{
				DWORD * ibTemp = NULL;
				m_pIB->Lock(0,0, (VOID**) &ibTemp, 0 );
				CopyMemory(ibTemp, &m_indexBufferSorted[0] , sizeof(long)*m_sizeIndexBuffer );
				m_pIB->Unlock();
			}
			else
			{
				WORD * ibTemp = NULL;
				m_pIB->Lock(0,0, (VOID**) &ibTemp, 0 );
				for ( int i = 0 ; i < m_sizeIndexBuffer ; i++ )
					ibTemp[i] = (WORD)m_indexBufferSorted[i];
				m_pIB->Unlock();
			}
		}
	}

	//
	//	Shader rendering.
	//	
	if ( nTransparency == 100 )
	{
		m_pProteinVistaRenderer->SetShaderTechnique(CProteinVistaRenderer::SurfaceRenderingNoAlpha, m_pPropertyCommon->m_shaderQuality);
	}
	else
	{
		m_pProteinVistaRenderer->SetShaderTechnique(CProteinVistaRenderer::SurfaceRenderingWithAlpha, m_pPropertyCommon->m_shaderQuality);
	}

	m_pProteinVistaRenderer->SetShaderWorldMatrix( m_pPDBRenderer->m_matWorld );

	D3DXMATRIXA16 matWorldView = m_pPDBRenderer->m_matWorld * (*m_pProteinVistaRenderer->GetViewMatrix());
	m_pProteinVistaRenderer->SetShaderWorldViewMatrix( matWorldView );
	D3DXMATRIXA16 matWorldViewProj = matWorldView * (*m_pProteinVistaRenderer->GetProjMatrix());
	m_pProteinVistaRenderer->SetShaderWorldViewProjMatrix( matWorldViewProj );

	m_pProteinVistaRenderer->SetShaderVertexAlpha((float)nTransparency/100.0f);

	m_pProteinVistaRenderer->SetShaderIndicate(FALSE);

	//	m_useInnerFaceColor는 BeginPass 안에 들어가있다.
	m_pProteinVistaRenderer->SetShaderBlendBackface(( m_pPropertySurface->m_useInnerFaceColor == TRUE )? m_pPropertySurface->m_blendFactor: 0);

	COLORREF innerDiffuseColor = m_pPropertySurface->m_colorInnerFace;
	m_pProteinVistaRenderer->SetShaderBackfaceDiffuseColor(COLORREF2D3DXCOLOR(innerDiffuseColor));

	D3DXVECTOR4	EyePos(m_pProteinVistaRenderer->m_FromVec);
	m_pProteinVistaRenderer->SetShaderEyePos(EyePos);

	m_pProteinVistaRenderer->SetShaderIntensityAmbient((m_pPropertyCommon->m_intensityAmbient*2)/100.0f);
	m_pProteinVistaRenderer->SetShaderIntensityDiffuse((m_pPropertyCommon->m_intensiryDiffuse*2)/100.0f);
	m_pProteinVistaRenderer->SetShaderIntensitySpecular((m_pPropertyCommon->m_intensitySpecular*2)/100.0f);


	//	m_pProteinSurface->m_pVB
	GetD3DDevice()->SetStreamSource ( 0, m_pVB , 0, sizeof(CSurfaceVertex) );
	GetD3DDevice()->SetVertexDeclaration(m_pDeclSurface);
	GetD3DDevice()->SetIndices(m_pIB);

	GetD3DDevice()->SetRenderState(D3DRS_CULLMODE,   D3DCULL_NONE);

	UINT cPasses;
	V( m_pProteinVistaRenderer->m_pEffectBasicShading->Begin(&cPasses, 0) );
	for (long iPass = 0; iPass < cPasses; iPass++)
	{
		V( m_pProteinVistaRenderer->m_pEffectBasicShading->BeginPass(iPass) );

		//	앞쪽면 렌더링.
		m_pProteinVistaRenderer->SetShaderUseBackfaceColor(FALSE);
		GetD3DDevice()->SetRenderState(D3DRS_CULLMODE , D3DCULL_NONE);

		m_pProteinVistaRenderer->m_pEffectBasicShading->CommitChanges();
		GetD3DDevice()->DrawIndexedPrimitive( D3DPT_TRIANGLELIST, 0,0, m_pProteinSurface->m_arrayVertex.size() , 0, m_sizeIndexBuffer/3 );

		//	항상 뒷쪽면도 렌더링.
		//	if ( m_pPropertySurface->m_useInnerFaceColor == TRUE )
		{
			//	뒷쪽면 렌더링.
			m_pProteinVistaRenderer->SetShaderUseBackfaceColor(TRUE);
			GetD3DDevice()->SetRenderState(D3DRS_CULLMODE , D3DCULL_CW);

			m_pProteinVistaRenderer->m_pEffectBasicShading->CommitChanges();
			GetD3DDevice()->DrawIndexedPrimitive( D3DPT_TRIANGLELIST, 0,0, m_pProteinSurface->m_arrayVertex.size() , 0, m_sizeIndexBuffer/3 );
		}

		m_pProteinVistaRenderer->SetShaderUseBackfaceColor(FALSE);
		GetD3DDevice()->SetRenderState(D3DRS_CULLMODE , D3DCULL_NONE);

		//	depth bias가 있으면, SSAO 에서 depth로 인식하여, line 주변이 어둡게 된다.
		//	FLOAT depthBias = -0.00001f;		
		//	m_pProteinVistaRenderer->GetD3DDevice()->SetRenderState(D3DRS_DEPTHBIAS, *(DWORD*)&(depthBias) );

		//	indicate 가 있을 경우에, wireframe으로 한번 더 렌더링한다.
		if ( m_pPropertyCommon->m_bIndicate == TRUE )
		{
			m_pProteinVistaRenderer->SetShaderIndicate(m_pPropertyCommon->m_bIndicate);
			m_pProteinVistaRenderer->SetShaderIndicateDiffuseColor(COLORREF2D3DXCOLOR(m_pPropertyCommon->m_indicateColor));
			GetD3DDevice()->SetRenderState(D3DRS_FILLMODE, currentSelectionMode );

			m_pProteinVistaRenderer->m_pEffectBasicShading->CommitChanges();
			GetD3DDevice()->DrawIndexedPrimitive( D3DPT_TRIANGLELIST, 0,0, m_pProteinSurface->m_arrayVertex.size() , 0, m_sizeIndexBuffer/3 );

			GetD3DDevice()->SetRenderState(D3DRS_FILLMODE, currentRenderMode );
			m_pProteinVistaRenderer->SetShaderIndicate(FALSE);
		}

		//    selection이 있을 경우에 한번 더 렌더링한다.
		if ( m_pPropertyCommon->m_bShowSelectionMark == TRUE )
		{
			if ( m_sizeIndexBufferSelection != 0 )
			{
				m_pProteinVistaRenderer->SetShaderIndicate(TRUE);
				m_pProteinVistaRenderer->SetShaderIndicateDiffuseColor(COLORREF2D3DXCOLOR(m_pProteinVistaRenderer->m_pPropertyScene->m_selectionColor));

				GetD3DDevice()->SetRenderState(D3DRS_FILLMODE, currentSelectionMode );
				GetD3DDevice()->SetIndices(m_pIBSelection);

				m_pProteinVistaRenderer->m_pEffectBasicShading->CommitChanges();
				GetD3DDevice()->DrawIndexedPrimitive( D3DPT_TRIANGLELIST, 0,0, m_pProteinSurface->m_arrayVertex.size() , 0, m_sizeIndexBufferSelection/3 );

				GetD3DDevice()->SetRenderState(D3DRS_FILLMODE, currentRenderMode );
			}
		}

		//	depthBias = 0.0f;	
		//	m_pProteinVistaRenderer->GetD3DDevice()->SetRenderState(D3DRS_DEPTHBIAS, *(DWORD*)&(depthBias) );

		m_pProteinVistaRenderer->m_pEffectBasicShading->CommitChanges();

		V( m_pProteinVistaRenderer->m_pEffectBasicShading->EndPass() );
	}
	V( m_pProteinVistaRenderer->m_pEffectBasicShading->End() );
	
	//	
	if ( DisplayMethod != 0 )
	{
		GetD3DDevice()->SetRenderState(D3DRS_FILLMODE, D3DFILL_SOLID );
	}

	return S_OK;
}

#pragma managed(pop)
//	color scheme 에 따른 vertex color를 설정한다.
HRESULT CRenderSurfaceSelection::SetVertexColor()
{
	long colorScheme = m_pPropertyCommon->m_enumColorScheme;

	CSurfaceVertex * pSurfaceVertex = NULL;
	m_pVB->Lock(0, 0, (void**) &pSurfaceVertex, 0);

	for ( long i = 0 ; i < m_pProteinSurface->m_arrayVertex.size() ; i++ )
	{
		long colorIndex = m_pProteinSurface->m_arrayIndexAtom[i];
		CAtom * pAtom = m_pProteinSurface->m_arrayAtom[colorIndex];

		//    아래코드를 넣으면 surface selection 조각의 border 색이 옆 색과 blending이 되어서
		//    검게 나온다.
		//    if ( pAtomInst->GetDisplayStyle(m_iDisplaySelection) == FALSE )	continue;

		CColorRow * pColorRow = m_pPropertyCommon->m_pSelectionDisplay->GetAtomColor(pAtom);
		D3DXCOLOR diffuse = pColorRow->m_color;
		pSurfaceVertex[i].diffuse = diffuse;
	}

	m_pVB->Unlock();

	//    surface color 를 curvature로 설정할 경우
	if ( m_pPropertySurface->m_bDisplayCurvature == TRUE )
	{
		SurfaceCurvatureColoring();
	}

	//	blurring factor를 넣는다.
	if ( m_pPropertySurface->m_iSurfaceBlurring > 0 )
			SurfaceBlurring();

	return S_OK;
}

HRESULT	 CRenderSurfaceSelection::UpdateAtomSelectionChanged()
{
	CChainInst * pChainInst = m_pChainInst;

	CSTLLongArray indexBuffer;
	GetIndexBuffer(indexBuffer, TRUE);

	m_sizeIndexBufferSelection = indexBuffer.size();
	if ( m_sizeIndexBufferSelection == 0) return S_FALSE;

	//	기존것을 지움.
	SAFE_RELEASE(m_pIBSelection);

	if ( m_pProteinSurface->m_arrayVertex.size() <= 0xffff )
	{
		m_formatIndexBuffer = D3DFMT_INDEX16;
		m_byteSizeIndexBuffer  = sizeof(WORD);
	}
	else
	{
		m_formatIndexBuffer = m_pProteinVistaRenderer->m_formatIndexBuffer;
		m_byteSizeIndexBuffer  = m_pProteinVistaRenderer->m_byteSizeIndexBuffer;
	}

	GetD3DDevice()->CreateIndexBuffer(m_sizeIndexBufferSelection*m_byteSizeIndexBuffer, 0, m_formatIndexBuffer, D3DPOOL_MANAGED, &(m_pIBSelection), NULL );

	if ( m_byteSizeIndexBuffer == sizeof(DWORD) )
	{
		DWORD * ib;
		m_pIBSelection->Lock(0,0, (VOID**) &ib, 0 );
		CopyMemory(ib, &indexBuffer[0], m_sizeIndexBufferSelection*sizeof(DWORD));
		m_pIBSelection->Unlock();
	}
	else
	{
		WORD * ib;
		m_pIBSelection->Lock(0,0, (VOID**) &ib, 0 );
		for ( int i = 0 ; i < m_sizeIndexBufferSelection; i++ )
			ib[i] = (WORD)indexBuffer[i];
		m_pIBSelection->Unlock();
	}

	return S_OK;
}

void	CRenderSurfaceSelection::SurfaceBlurring()
{
	long level = m_pPropertySurface->m_iSurfaceBlurring;

	if ( level == 0 )	return;			//	dont' apply blurring.

	D3DXCOLOR	* pSurfaceColorOrig = new D3DXCOLOR[m_pProteinSurface->m_arrayVertex.size()];

	if ( m_pProteinSurface->m_ArrayArrayAdjacentVertex.size() == 0 )
		m_pProteinSurface->CreateAdjacentVertex();


	CSurfaceVertex * pSurfaceVertex = NULL;
	m_pVB->Lock(0, 0, (void**) &pSurfaceVertex, 0);

	//	원본을 copy 해둠
	for ( long i = 0 ; i < m_pProteinSurface->m_arrayVertex.size() ; i++ )
	{
		pSurfaceColorOrig[i] = pSurfaceVertex[i].diffuse;
	}

	//								0,    1,    2,    3,   4,     5
	FLOAT	blurringFactor[] = { 1.0f, 1.5f, 2.0f, 3.0f, 5.0f , 8.0f , 12.0f };
	FLOAT	iDivide = blurringFactor[level];

	FLOAT	adjacentPart = 1.0f - (1.0f/iDivide);
	//	
	//	use m_pProteinSurface->m_ArrayArrayAdjacentVertex
	//
	long iProgress =GetMainActiveView()->InitProgress(100);
	for ( long i = 0 ; i < m_pProteinSurface->m_arrayVertex.size() ; i++ )
	{
		if ( i%(m_pProteinSurface->m_arrayVertex.size()/100) == 0 )
			GetMainActiveView()->SetProgress(i*100/m_pProteinSurface->m_arrayVertex.size(), iProgress);

		CSTLLONGArray & intArray = m_pProteinSurface->m_ArrayArrayAdjacentVertex[i];

		FLOAT nSizeConvolution = intArray.size();

		D3DXCOLOR colorBlur(0,0,0,0);
		colorBlur.r += pSurfaceColorOrig[i].r * (1.0f/iDivide);
		colorBlur.g += pSurfaceColorOrig[i].g * (1.0f/iDivide);
		colorBlur.b += pSurfaceColorOrig[i].b * (1.0f/iDivide);
		
		CSTLLONGArray::iterator iterator;
		for ( iterator = intArray.begin() ; iterator != intArray.end() ; iterator++ )
		{
			long indexAdjacent = *iterator;
			colorBlur.r += pSurfaceColorOrig[indexAdjacent].r * (adjacentPart/nSizeConvolution);
			colorBlur.g += pSurfaceColorOrig[indexAdjacent].g * (adjacentPart/nSizeConvolution);
			colorBlur.b += pSurfaceColorOrig[indexAdjacent].b * (adjacentPart/nSizeConvolution);
		}

		pSurfaceVertex[i].diffuse = colorBlur;
	}
	GetMainActiveView()->EndProgress(iProgress);

	m_pVB->Unlock();

	delete [] pSurfaceColorOrig;
}

//
//    SurfaceBlurring과 같이 surface curvature color를 설정한다.
//    
void	CRenderSurfaceSelection::SurfaceCurvatureColoring()
{
	int		ringSize = m_pPropertySurface->m_curvatureRingSize;

	m_pProteinSurface->CalculateCurvature(ringSize);

	CSurfaceVertex * pSurfaceVertex = NULL;
	m_pVB->Lock(0, 0, (void**) &pSurfaceVertex, 0);

	CSTLFLOATArray	& arrayVertexCurvature = m_pProteinSurface->m_arrayVertexCurvature[ringSize];
	FLOAT min = m_pProteinSurface->m_arrayVertexCurvatureMin[ringSize];
	FLOAT max = m_pProteinSurface->m_arrayVertexCurvatureMax[ringSize];

	for ( int i = 0 ; i < arrayVertexCurvature.size() ; i++ )
	{
		D3DXCOLOR diffuse = FindGradientColor( m_pPropertySurface->m_arrayColorRowCurvature,
			(arrayVertexCurvature[i]*1000-min*1000), (max*1000-min*1000) +1 );

		//    find 3 colors.
		//    assign these colors to vertex.
		pSurfaceVertex[i].diffuse = diffuse;
	}

	m_pVB->Unlock();
}

//
//	현재 선택된것의 index buffer를 구한다.
//
HRESULT CRenderSurfaceSelection::GetIndexBuffer(CSTLLongArray & arrayIndexBuffer, BOOL bCountSelect )
{
	long	nItem = 0;
	//	할당할 갯수가 몇개인가를 센다.
	for ( int iAtom = 0 ; iAtom < m_arrayAtomInst.size() ; iAtom++ )
	{
		CAtomInst * pAtomInst = m_arrayAtomInst[iAtom];
		if ( pAtomInst->GetDisplayStyle(m_iDisplaySelection) == FALSE )
			continue;

		if ( bCountSelect == TRUE )
			if ( pAtomInst->m_bSelect == FALSE )
				continue;

		CSTLLONGArray & stlLongArray = m_pProteinSurface->m_ArrayArrayFaceIndex[iAtom];

		nItem += stlLongArray.size();
	}

	//	Index buffer를 만듬
	arrayIndexBuffer.clear();
	arrayIndexBuffer.reserve(nItem);

	//	렌더링할 범위를 찾아 index buffer를 만든다.
	for ( int iAtom = 0 ; iAtom < m_arrayAtomInst.size() ; iAtom++ )
	{
		CAtomInst * pAtomInst = m_arrayAtomInst[iAtom];

		if ( pAtomInst->GetDisplayStyle(m_iDisplaySelection) == FALSE )
			continue;

		if ( bCountSelect == TRUE )
			if ( pAtomInst->m_bSelect == FALSE )
				continue;

		CSTLLONGArray & stlLongArray = m_pProteinSurface->m_ArrayArrayFaceIndex[iAtom];

		for ( int iIndex = 0 ; iIndex < stlLongArray.size(); iIndex ++ )
		{
			arrayIndexBuffer.push_back(stlLongArray[iIndex] );
		}
	}

	return S_OK;
}


