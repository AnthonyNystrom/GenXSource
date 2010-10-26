
#include "StdAfx.h"
#include "ProteinVista.h"


#include "ProteinVistaView.h"
#include "PDBRenderer.h"
#include "ProteinVistaRenderer.h"
#include "Interface.h"
#include "RenderBallStickSelection.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

CRenderBallStickSelection::CRenderBallStickSelection()
{
	m_iDisplaySelection = 0;

	m_pPDBRenderer = NULL;

	m_bDisplayBallStck = TRUE;

	m_bDisplaySideChain = TRUE;
	m_bDisplayHETATM = FALSE;
}

CRenderBallStickSelection::~CRenderBallStickSelection()
{
	DeleteDeviceObjects();
}

void CRenderBallStickSelection::SetModelQuality()
{
	//    notify message.
	long	sphereResolution = m_pProteinVistaRenderer->m_renderQualityPreset.m_sphereResolutionConst[m_pPropertyCommon->m_modelQuality];
	long	cylinderResolution = m_pProteinVistaRenderer->m_renderQualityPreset.m_cylinderResolutionConst[m_pPropertyCommon->m_modelQuality];

	//    model quality
	/*if ( m_typeSelection == BALLSTICK )
	{
		m_pPropertyBallStick->m_pItemSphereResolution->SetNumber(sphereResolution);
		m_pPropertyBallStick->m_pItemSphereResolution->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(m_pPropertyBallStick->m_pItemSphereResolution));

		m_pPropertyBallStick->m_pItemCylinderResolution->SetNumber(cylinderResolution);
		m_pPropertyBallStick->m_pItemCylinderResolution->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(m_pPropertyBallStick->m_pItemCylinderResolution));
	}
	else
	{
		m_pPropertyStick->m_pItemSphereResolution->SetNumber(sphereResolution);
		m_pPropertyStick->m_pItemSphereResolution->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(m_pPropertyStick->m_pItemSphereResolution));

		m_pPropertyStick->m_pItemCylinderResolution->SetNumber(cylinderResolution);
		m_pPropertyStick->m_pItemCylinderResolution->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(m_pPropertyStick->m_pItemCylinderResolution));
	}*/
}

HRESULT CRenderBallStickSelection::InitDeviceObjects()
{
	HRESULT hr;
	DeleteDeviceObjects();

	CSelectionDisplay * pSelectionDisplay = m_pProteinVistaRenderer->m_arraySelectionDisplay[m_iDisplaySelection];
	m_pPropertyCommon = pSelectionDisplay->GetPropertyCommon();
	m_pPropertyStick = pSelectionDisplay->GetPropertyStick();
	m_pPropertyBallStick = pSelectionDisplay->GetPropertyBallStick();

	if ( m_pPropertyStick == NULL )
	{
		m_typeSelection = BALLSTICK;

		m_sphereResolution = m_pPropertyBallStick->m_sphereResolution;
		m_cylinderResolution = m_pPropertyBallStick->m_cylinderResolution*2;
		m_sphereRadius = m_pPropertyBallStick->m_sphereRadius;
		m_cylinderSize = m_pPropertyBallStick->m_cylinderSize;
	}
	else
	{
		m_typeSelection = STICK;

		m_sphereResolution = m_pPropertyStick->m_sphereResolution;
		m_cylinderResolution = m_pPropertyStick->m_cylinderResolution*2;
		m_sphereRadius = m_pPropertyStick->m_stickSize;
		m_cylinderSize = m_pPropertyStick->m_stickSize;
	}

	m_bDisplaySideChain = m_pPropertyCommon->m_bDisplaySideChain;
	m_bDisplayHETATM = m_pPropertyCommon->m_bDisplayHETATM;

	//	
	CSTLArrayVector4	arraySpherePosition[MAX_ATOM];
	CSTLFLOATArray		arraySphereSelection[MAX_ATOM];
	CSTLArrayColor		arraySphereColor[MAX_ATOM];

	CSTLArrayVector4	arrayBondPosition[MAX_ATOM];
	CSTLFLOATArray		arrayBondSelection[MAX_ATOM];
	CSTLArrayColor		arrayBondColor[MAX_ATOM];

	CSTLArrayVector2	arrayBondRotation[MAX_ATOM];
	CSTLFLOATArray		arrayBondScale[MAX_ATOM];

	for ( int i = 0 ; i < MAX_ATOM; i++ )
	{
		arraySpherePosition[i].reserve(1000);
		arraySphereSelection[i].reserve(1000);
		arraySphereColor[i].reserve(1000);

		arrayBondPosition[i].reserve(1000);
		arrayBondSelection[i].reserve(1000);
		arrayBondColor[i].reserve(1000);
		arrayBondRotation[i].reserve(1000);
		arrayBondScale[i].reserve(1000);
	}

	//
	MakeSphereInstanceData(arraySpherePosition, arraySphereSelection, arraySphereColor);
	MakeBondInstanceData(arrayBondPosition, arrayBondSelection, arrayBondRotation , arrayBondScale , arrayBondColor );

	for ( int i = 0 ; i < MAX_ATOM; i++ )
	{
		//	한개라도 atom 이 있다면,
		if ( arraySpherePosition[i].size() > 0 )
		{
			FLOAT	sphereRadius = m_sphereRadius;
			long	sphereResolution = m_sphereResolution;

			m_batchDrawAtom[i].Init( m_pProteinVistaRenderer, sphereRadius , sphereResolution);
			m_batchDrawAtom[i].SetInstanceData(arraySpherePosition[i], arraySphereSelection[i], arraySphereColor[i]);
		}

		if ( arrayBondPosition[i].size() > 0 )
		{
			FLOAT	sphereRadius = m_cylinderSize;
			long	sphereResolution = m_cylinderResolution;

			m_batchDrawBond[i].Init( m_pProteinVistaRenderer, sphereRadius , sphereResolution);
			m_batchDrawBond[i].SetInstanceData(arrayBondPosition[i], arrayBondSelection[i], arrayBondRotation[i] , arrayBondScale[i], arrayBondColor[i] );
		}
	}

	//	
	for ( int i = 0 ; i < MAX_ATOM; i++ )
	{
		m_batchDrawAtom[i].InitDeviceObjects();
		m_batchDrawBond[i].InitDeviceObjects();
	}

	return S_OK;
}

HRESULT CRenderBallStickSelection::DeleteDeviceObjects()
{
	for ( int i = 0 ; i < MAX_ATOM; i++ )
	{
		m_batchDrawAtom[i].DeleteDeviceObjects();
		m_batchDrawBond[i].DeleteDeviceObjects();
	}

	return S_OK;
}
#pragma managed(push,off)
HRESULT CRenderBallStickSelection::Render()
{
	HRESULT hr;

	//
	//	Ball
	//
	{
		m_pProteinVistaRenderer->SetShaderTechnique(CProteinVistaRenderer::SphereRenderingBatch, m_pPropertyCommon->m_shaderQuality);

		m_pProteinVistaRenderer->SetShaderIndicate(m_pPropertyCommon->m_bIndicate);
		m_pProteinVistaRenderer->SetShaderIndicateDiffuseColor(COLORREF2D3DXCOLOR(m_pPropertyCommon->m_indicateColor));

		D3DXVECTOR4	EyePos(m_pProteinVistaRenderer->m_FromVec);
		m_pProteinVistaRenderer->SetShaderEyePos(EyePos);

		m_pProteinVistaRenderer->SetShaderIntensityAmbient((m_pPropertyCommon->m_intensityAmbient*2)/100.0f);
		m_pProteinVistaRenderer->SetShaderIntensityDiffuse((m_pPropertyCommon->m_intensiryDiffuse*2)/100.0f);
		m_pProteinVistaRenderer->SetShaderIntensitySpecular((m_pPropertyCommon->m_intensitySpecular*2)/100.0f);

		long iDisplay = 0;
		if ( m_bDisplayBallStck == TRUE )
			iDisplay = CProteinVistaRenderer::INDEX_SPHERE_BALL_STICK_DISPLAY;
		else
			iDisplay = CProteinVistaRenderer::INDEX_SPHERE_BALL_DISPLAY;

		m_pProteinVistaRenderer->SetShaderWorldMatrix( m_pPDBRenderer->m_matWorld );

		D3DXMATRIXA16 matWorldView = m_pPDBRenderer->m_matWorld * (*m_pProteinVistaRenderer->GetViewMatrix());
		m_pProteinVistaRenderer->SetShaderWorldViewMatrix( matWorldView );
		D3DXMATRIXA16 matWorldViewProj = matWorldView * (*m_pProteinVistaRenderer->GetProjMatrix());
		m_pProteinVistaRenderer->SetShaderWorldViewProjMatrix( matWorldViewProj );

		for ( int i = 0 ; i < MAX_ATOM; i++ )
		{
			m_batchDrawAtom[i].Render();
		}
	}

	//
	//	Stick
	{
		m_pProteinVistaRenderer->SetShaderTechnique(CProteinVistaRenderer::CylinderRenderingBatch, m_pPropertyCommon->m_shaderQuality);

		m_pProteinVistaRenderer->SetShaderWorldMatrix( m_pPDBRenderer->m_matWorld );

		D3DXMATRIXA16 matWorldView = m_pPDBRenderer->m_matWorld * (*m_pProteinVistaRenderer->GetViewMatrix());
		m_pProteinVistaRenderer->SetShaderWorldViewMatrix( matWorldView );
		D3DXMATRIXA16 matWorldViewProj = matWorldView * (*m_pProteinVistaRenderer->GetProjMatrix());
		m_pProteinVistaRenderer->SetShaderWorldViewProjMatrix( matWorldViewProj );

		for ( int i = 0 ; i < MAX_ATOM; i++ )
		{
			m_batchDrawBond[i].Render();
		}
	}

	return S_OK;
}
#pragma managed(pop)
//
//
#pragma managed(push,off)
HRESULT	CRenderBallStickSelection::MakeSphereInstanceData(CSTLArrayVector4 arraySpherePosition[], CSTLFLOATArray arraySphereSelection[], CSTLArrayColor arraySphereColor[] )
{
	HRESULT hr;
	UINT iPass, cPasses;

	D3DXMATRIXA16	matTrans;
	D3DXMATRIXA16	matModel;

	D3DCOLOR	colorOld = 0;

	CChainInst * pChainInst = m_pChainInst;

	for ( int iAtom = 0; iAtom < pChainInst->m_arrayAtomInst.size() ; iAtom++ )
	{
		CAtomInst * pAtomInst = pChainInst->m_arrayAtomInst[iAtom];

		//	중간에 떨어진 같은 index 는 두개로 분리되어 렌더링된다.
		if ( pAtomInst->GetDisplayStyle(m_iDisplaySelection) == FALSE )
			continue;

		//	find ATOM...
		if ( (pAtomInst->GetAtom()->m_bSideChain == FALSE) || (pAtomInst->GetAtom()->m_bSideChain == TRUE && m_bDisplaySideChain == TRUE) )
		{
			D3DXVECTOR4 vecPos;
			vecPos.x = pAtomInst->GetAtom()->m_pos.x;
			vecPos.y = pAtomInst->GetAtom()->m_pos.y;
			vecPos.z = pAtomInst->GetAtom()->m_pos.z;
			vecPos.w = 0.0f;
			arraySpherePosition[pAtomInst->GetAtom()->m_atomNameIndex].push_back(vecPos);

			FLOAT bSelection = ( m_pPropertyCommon->m_indicateColorSlot != -1 )?(m_pPropertyCommon->m_indicateColorSlot * 0.1f):((pAtomInst->GetSelect() == TRUE )? 0.1f*(float)(m_pPropertyCommon->m_bShowSelectionMark): 0.0f);
			arraySphereSelection[pAtomInst->GetAtom()->m_atomNameIndex].push_back(bSelection);

			CColorRow * pColorRow = m_pPropertyCommon->m_pSelectionDisplay->GetAtomColor(pAtomInst->GetAtom());
			arraySphereColor[pAtomInst->GetAtom()->m_atomNameIndex].push_back(pColorRow->m_color);
		}
	}

	return S_OK;
}

HRESULT	CRenderBallStickSelection::MakeBondInstanceData(CSTLArrayVector4 arrayBondPosition[], CSTLFLOATArray arrayBondSelection[], CSTLArrayVector2 arrayBondRotation[] , CSTLFLOATArray arrayBondScale [] , CSTLArrayColor arrayBondColor[] )
{
	HRESULT hr;

	D3DXMATRIXA16	matTrans;
	D3DXMATRIXA16	matModel;

	D3DCOLOR	colorOld = 0;

	CChainInst * pChainInst = m_pChainInst;

	//	
	for ( int iAtom = 0 ; iAtom < pChainInst->m_arrayAtomInst.size() ; iAtom++ )
	{
		CAtomInst * pAtom1 = pChainInst->m_arrayAtomInst[iAtom];

		if ( !((pAtom1->GetAtom()->m_bSideChain == FALSE) || (pAtom1->GetAtom()->m_bSideChain == TRUE && m_bDisplaySideChain == TRUE) ) )
			continue;

		if ( pAtom1->GetDisplayStyle(m_iDisplaySelection) == FALSE )
			continue;

		//	한 atom 에 본드가 여러개일수 있다.
		for ( int i = 0 ; i < pAtom1->GetAtom()->m_arrayBondIndex.size() ; i++ )
		{
			DWORD bondIndex = pAtom1->GetAtom()->m_arrayBondIndex[i];
			DWORD iBond = pChainInst->GetChain()->m_arrayBond[bondIndex];
			CAtomInst * pAtom2 = pChainInst->m_arrayAtomInst[HIWORD(iBond)];

			if ( !( (pAtom2->GetAtom()->m_bSideChain == FALSE) || (pAtom2->GetAtom()->m_bSideChain == TRUE && m_bDisplaySideChain == TRUE) ) )
				continue;

			//	pAtom1, pAtom2
			if ( pAtom2->GetDisplayStyle(m_iDisplaySelection) == TRUE )
			{
				{
					arrayBondPosition[pAtom1->GetAtom()->m_atomNameIndex].push_back(pChainInst->GetChain()->m_arrayBondPos1[bondIndex]);

					FLOAT bSelection = ( m_pPropertyCommon->m_indicateColorSlot != -1 )?(m_pPropertyCommon->m_indicateColorSlot * 0.1f):((pAtom1->GetSelect() == TRUE )? 0.1f*(float)(m_pPropertyCommon->m_bShowSelectionMark): 0.0f);
					arrayBondSelection[pAtom1->GetAtom()->m_atomNameIndex].push_back(bSelection);

					CColorRow * pColorRow = m_pPropertyCommon->m_pSelectionDisplay->GetAtomColor(pAtom1->GetAtom());
					arrayBondColor[pAtom1->GetAtom()->m_atomNameIndex].push_back(pColorRow->m_color);

					arrayBondRotation[pAtom1->GetAtom()->m_atomNameIndex].push_back(pChainInst->GetChain()->m_arrayBondRotation1[bondIndex]);
					arrayBondScale[pAtom1->GetAtom()->m_atomNameIndex].push_back(pChainInst->GetChain()->m_arrayBondScale[bondIndex]);
				}

				{
					arrayBondPosition[pAtom2->GetAtom()->m_atomNameIndex].push_back(pChainInst->GetChain()->m_arrayBondPos2[bondIndex]);

					FLOAT bSelection = ( m_pPropertyCommon->m_indicateColorSlot != -1 )?(m_pPropertyCommon->m_indicateColorSlot * 0.1f):((pAtom2->GetSelect() == TRUE )? 0.1f*(float)(m_pPropertyCommon->m_bShowSelectionMark): 0.0f);
					arrayBondSelection[pAtom2->GetAtom()->m_atomNameIndex].push_back(bSelection);

					CColorRow * pColorRow = m_pPropertyCommon->m_pSelectionDisplay->GetAtomColor(pAtom2->GetAtom());
					arrayBondColor[pAtom2->GetAtom()->m_atomNameIndex].push_back(pColorRow->m_color);

					arrayBondRotation[pAtom2->GetAtom()->m_atomNameIndex].push_back(pChainInst->GetChain()->m_arrayBondRotation2[bondIndex]);
					arrayBondScale[pAtom2->GetAtom()->m_atomNameIndex].push_back(pChainInst->GetChain()->m_arrayBondScale[bondIndex]);
				}
			}
		}
	}

	return S_OK;
}
#pragma managed(pop)