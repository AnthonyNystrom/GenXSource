
#include "StdAfx.h"
#include "ProteinVista.h"


#include "ProteinVistaView.h"
#include "PDBRenderer.h"
#include "ProteinVistaRenderer.h"
#include "Interface.h"
#include "RenderSpaceFillSelection.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


CRenderSpaceFillSelection::CRenderSpaceFillSelection()
{
	m_iDisplaySelection = 0;

	m_pPDBRenderer = NULL;
	m_bDisplaySideChain = TRUE;
}

CRenderSpaceFillSelection::~CRenderSpaceFillSelection()
{
	DeleteDeviceObjects();
}

void CRenderSpaceFillSelection::SetModelQuality()
{
	//    notify message.
	long	sphereResolution = m_pProteinVistaRenderer->m_renderQualityPreset.m_sphereResolutionConst[m_pPropertyCommon->m_modelQuality];

	//    model quality
	//m_pPropertySpacFill->m_pItemSphereResolution->SetNumber(sphereResolution);
	//m_pPropertySpacFill->m_pItemSphereResolution->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(m_pPropertySpacFill->m_pItemSphereResolution));
}
#pragma managed(push,off)
//	New function
HRESULT CRenderSpaceFillSelection::InitDeviceObjects()
{
	HRESULT hr;
	DeleteDeviceObjects();

	CSelectionDisplay * pSelectionDisplay = m_pProteinVistaRenderer->m_arraySelectionDisplay[m_iDisplaySelection];
	m_pPropertyCommon = pSelectionDisplay->GetPropertyCommon();
	m_pPropertySpacFill = pSelectionDisplay->GetPropertySpaceFill();

	long colorScheme = m_pPropertyCommon->m_enumColorScheme;

	m_bDisplaySideChain = m_pPropertyCommon->m_bDisplaySideChain;

	D3DXMATRIXA16	matTrans;
	D3DXMATRIXA16	matModel;
	D3DCOLOR	colorOld = 0;

	CChainInst * pChainInst = m_pChainInst;

	//	position. selection. color.
	CSTLArrayVector4	arrayPosition[MAX_ATOM];
	CSTLFLOATArray		arraySelection[MAX_ATOM];
	CSTLArrayColor		arrayColor[MAX_ATOM];

	for ( int i = 0 ; i < MAX_ATOM; i++ )
	{
		arrayPosition[i].reserve(1000);
		arraySelection[i].reserve(1000);
		arrayColor[i].reserve(1000);
	}

	for ( int iAtom = 0; iAtom < pChainInst->m_arrayAtomInst.size() ; iAtom++ )
	{
		CAtomInst * pAtomInst = pChainInst->m_arrayAtomInst[iAtom];

		//	중간에 떨어진 같은 index 도 하나로 렌더링.
		if ( pAtomInst->GetDisplayStyle(m_iDisplaySelection) == FALSE )
			continue;

		if ( m_bDisplaySideChain == FALSE && pAtomInst->GetAtom()->m_bSideChain == TRUE )
			continue;

		//	pAtomInst 이 실제 그려질 atom
		D3DXVECTOR4 vecPos;
		vecPos.x = pAtomInst->GetAtom()->m_pos.x;
		vecPos.y = pAtomInst->GetAtom()->m_pos.y;
		vecPos.z = pAtomInst->GetAtom()->m_pos.z;
		vecPos.w = 0.0f;
		arrayPosition[pAtomInst->GetAtom()->m_atomNameIndex].push_back(vecPos);

		FLOAT bSelection = ( m_pPropertyCommon->m_indicateColorSlot != -1 )?(m_pPropertyCommon->m_indicateColorSlot * 0.1f):((pAtomInst->GetSelect() == TRUE )? 0.1f*(float)(m_pPropertyCommon->m_bShowSelectionMark): 0.0f);
		arraySelection[pAtomInst->GetAtom()->m_atomNameIndex].push_back(bSelection);

		CColorRow * pColorRow = m_pPropertyCommon->m_pSelectionDisplay->GetAtomColor(pAtomInst->GetAtom());
		arrayColor[pAtomInst->GetAtom()->m_atomNameIndex].push_back(pColorRow->m_color);
	}

	for ( int i = 0 ; i < MAX_ATOM; i++ )
	{
		//	한개라도 atom 이 있다면,
		if ( arrayPosition[i].size() > 0 )
		{
			FLOAT	sphereRadius = m_pPropertySpacFill->m_atomRadius[i];
			long	sphereResolution = m_pPropertySpacFill->m_sphereResolution;

			m_batchDrawAtom[i].Init( m_pProteinVistaRenderer, sphereRadius , sphereResolution );
			m_batchDrawAtom[i].SetInstanceData(arrayPosition[i], arraySelection[i], arrayColor[i]);
		}
	}

	for ( int i = 0 ; i < MAX_ATOM; i++ )
	{
		m_batchDrawAtom[i].InitDeviceObjects();
	}

	return S_OK;
}

#pragma managed(pop)
HRESULT CRenderSpaceFillSelection::DeleteDeviceObjects()
{
	for ( int i = 0 ; i < MAX_ATOM; i++ )
	{
		m_batchDrawAtom[i].DeleteDeviceObjects();
	}

	return S_OK;
}
#pragma managed(push,off)
HRESULT CRenderSpaceFillSelection::Render()
{
	HRESULT hr;

	m_pProteinVistaRenderer->SetShaderTechnique(CProteinVistaRenderer::SphereRenderingBatch, m_pPropertyCommon->m_shaderQuality);

	m_pProteinVistaRenderer->SetShaderIndicate(m_pPropertyCommon->m_bIndicate);
	m_pProteinVistaRenderer->SetShaderIndicateDiffuseColor(COLORREF2D3DXCOLOR(m_pPropertyCommon->m_indicateColor));

	m_pProteinVistaRenderer->SetShaderWorldMatrix( m_pPDBRenderer->m_matWorld );

	D3DXMATRIXA16 matWorldView = m_pPDBRenderer->m_matWorld * (*m_pProteinVistaRenderer->GetViewMatrix());
	m_pProteinVistaRenderer->SetShaderWorldViewMatrix( matWorldView );
	D3DXMATRIXA16 matWorldViewProj = matWorldView * (*m_pProteinVistaRenderer->GetProjMatrix());
	m_pProteinVistaRenderer->SetShaderWorldViewProjMatrix( matWorldViewProj );

	D3DXVECTOR4	EyePos(m_pProteinVistaRenderer->m_FromVec);
	m_pProteinVistaRenderer->SetShaderEyePos(EyePos);

	m_pProteinVistaRenderer->SetShaderIntensityAmbient((m_pPropertyCommon->m_intensityAmbient*2)/100.0f);
	m_pProteinVistaRenderer->SetShaderIntensityDiffuse((m_pPropertyCommon->m_intensiryDiffuse*2)/100.0f);
	m_pProteinVistaRenderer->SetShaderIntensitySpecular((m_pPropertyCommon->m_intensitySpecular*2)/100.0f);

	for ( int i = 0 ; i < MAX_ATOM; i++ )
	{
		m_batchDrawAtom[i].Render();
	}

	return S_OK;
}
#pragma managed(pop)
