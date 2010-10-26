#include "stdafx.h"
#include "ProteinVista.h"


#include "ProteinVistaView.h"

#include "ProteinVistaRenderer.h"

#include "SelectionDisplay.h"
#include "Interface.h"
//#include "RenderPropertyCustomItem.h"
#include "ProteinSurfaceMSMS.h"
#include "ProteinSurfaceMQ.h"

#include "RenderWireframeSelection.h"
#include "RenderSpaceFillSelection.h"
#include "RenderBallStickSelection.h"
#include "RenderSurfaceSelection.h"
#include "RenderRibbonSelection.h"
#include "PDBRenderer.h"
#include "Utility.h"
#include "RibbonVertexData.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


//	
long CSelectionDisplay::m_maxSelectionIndex = 0;

CSelectionDisplay * CSelectionDisplay::CreateSelectionDisplay(long mode)
{
	CSelectionDisplay * pSelectionDisplay = new CSelectionDisplay;

	//	순차적으로 만든다. Selection List 에 순서가 엉망이 되어 헷갈림.
	pSelectionDisplay->m_iSerial = CSelectionDisplay::m_maxSelectionIndex++;
	return pSelectionDisplay;
}

//	현재 selection 에 넣어져야 할 Rendering 을 넣는다.
HRESULT	CSelectionDisplay::InitRenderSceneSelection()
{
	for ( long i = 0 ; i < m_arrayRenderObjectSelection.size() ; i++ )
	{
		m_arrayRenderObjectSelection[i]->DeleteDeviceObjects();
		delete m_arrayRenderObjectSelection[i];
	}
	m_arrayRenderObjectSelection.clear();

	CPDBInst * pPDBInst = m_pPDBRenderer->GetPDBInst();

	long iProgress;
	if ( pPDBInst->m_arrayModelInst.size() > 1 )
		iProgress =GetMainActiveView()-> InitProgress(100);

	for ( int iModel = 0 ; iModel < pPDBInst->m_arrayModelInst.size() ; iModel ++ )
	{
		if ( pPDBInst->m_arrayModelInst.size() > 1 )
			GetMainActiveView()->SetProgress((iModel+1)*100/pPDBInst->m_arrayModelInst.size(), iProgress);

		CModelInst * pModelInst = pPDBInst->m_arrayModelInst[iModel];

		long iProgress2 = GetMainActiveView()->InitProgress(100);
		for ( int iChain = 0 ; iChain < pModelInst->m_arrayChainInst.size() ; iChain++ )
		{
			GetMainActiveView()->SetProgress((iChain+1)*100/pModelInst->m_arrayChainInst.size(), iProgress2);

			CChainInst * pChainInst = pModelInst->m_arrayChainInst[iChain];
			InitRenderSelection(pChainInst);
		}
		GetMainActiveView()->EndProgress(iProgress2);

	}
 
	if ( pPDBInst->m_arrayModelInst.size() > 1 )
		GetMainActiveView()->EndProgress(iProgress);


	return S_OK;
}

//	
//	현재 pAtom 이후의 Atom 들에 대해서 Rendering을 한다.
//
void CSelectionDisplay::InitRenderSelection(CChainInst * pChainInst)
{
	switch ( m_displayStyle )
	{
	case SPACEFILL:
		InitRenderSelectionSpaceFill(pChainInst);
		break;
	case WIREFRAME:
		InitRenderSelectionWire(pChainInst);
		break;
	case STICKS:
		InitRenderSelectionBallStick(pChainInst,TRUE );
		break;
	case BALLANDSTICK:
		InitRenderSelectionBallStick(pChainInst, FALSE );
		break;
	case RIBBON:
		InitRenderSelectionRibbon(pChainInst);
		break;
	case SURFACE:
		InitRenderSelectionSurface(pChainInst);
		break;
	case NGRealistic:
		InitRenderSelectionSurface(pChainInst);
		break;
	}
}


HRESULT CSelectionDisplay::InitRenderSelectionSpaceFill(CChainInst * pChainInst)
{
	HRESULT hr;

	CRenderSpaceFillSelection * pSpaceFillRenderer = new CRenderSpaceFillSelection;
	pSpaceFillRenderer->m_pChainInst = pChainInst;
	pSpaceFillRenderer->m_iDisplaySelection = m_iDisplayStylePDB;
	pSpaceFillRenderer->m_pPDBRenderer = m_pPDBRenderer;
	pSpaceFillRenderer->m_pProteinVistaRenderer = m_pProteinVistaRenderer;
	pSpaceFillRenderer->Init();

	CPropertySpaceFill * propertySpaceFill = dynamic_cast<CPropertySpaceFill*>(m_pRenderProperty);
	pSpaceFillRenderer->m_colorScheme = propertySpaceFill->m_enumColorScheme;

	pSpaceFillRenderer->InitDeviceObjects();

	m_arrayRenderObjectSelection.push_back(pSpaceFillRenderer);

	return S_OK;
}

HRESULT	CSelectionDisplay::InitRenderSelectionWire ( CChainInst * pChainInst )
{
	CRenderWireframeSelection * pWireframeRenderer = new CRenderWireframeSelection;
	pWireframeRenderer->m_pChainInst = pChainInst;
	pWireframeRenderer->m_iDisplaySelection = m_iDisplayStylePDB;
	pWireframeRenderer->m_pPDBRenderer = m_pPDBRenderer;
	pWireframeRenderer->m_pProteinVistaRenderer = m_pProteinVistaRenderer;
	pWireframeRenderer->Init();

	CPropertyWireframe * propertyWireframe = dynamic_cast<CPropertyWireframe*>(m_pRenderProperty);
	pWireframeRenderer->m_colorScheme = propertyWireframe->m_enumColorScheme;
	pWireframeRenderer->m_typeWireframe = CRenderWireframeSelection::PROTEIN; 

	pWireframeRenderer->InitDeviceObjects();

	m_arrayRenderObjectSelection.push_back(pWireframeRenderer);

	return S_OK;
}

HRESULT	CSelectionDisplay::InitRenderSelectionBallStick(CChainInst * pChainInst , BOOL bStick)
{
	HRESULT hr;

	CRenderBallStickSelection * pBallStickRenderer = new CRenderBallStickSelection;
	pBallStickRenderer->m_pChainInst = pChainInst;
	pBallStickRenderer->m_iDisplaySelection = m_iDisplayStylePDB;
	pBallStickRenderer->m_pPDBRenderer = m_pPDBRenderer;
	pBallStickRenderer->m_pProteinVistaRenderer = m_pProteinVistaRenderer;
	pBallStickRenderer->Init();
	pBallStickRenderer->m_bDisplayBallStck = !(bStick);

	CPropertyCommon * propertyBallStick = dynamic_cast<CPropertyCommon*>(m_pRenderProperty);
	pBallStickRenderer->m_colorScheme = propertyBallStick->m_enumColorScheme;

	pBallStickRenderer->InitDeviceObjects();

	m_arrayRenderObjectSelection.push_back(pBallStickRenderer);

	return S_OK;
}

//////////////////////////////////////////////////////////////////////////
//	각각의 선택범위에 따라서 각각 전부 CRibbonRenderer를 만든다.
//	범위에 따라 RibbonRender의 display속성이 다를수 있다.
//	
HRESULT	CSelectionDisplay::InitRenderSelectionRibbon(CChainInst * pChainInst)
{
	CPropertyRibbon * propertyRibbon = GetPropertyRibbon();

	long	numSegment = propertyRibbon->m_resolution*2;		//	반드시 even
	float	curveTension = (FLOAT)(propertyRibbon->m_curveTension)/(FLOAT)100.0f;

	CProteinRibbonVertexData * pProteinRibbonVertexData = NULL;

	for ( int i = 0 ; i < m_pProteinVistaRenderer->m_arrayProteinRibbonVertexData.size() ; i++ )
	{
		//
		//	생성 옵션을 전부 비교해야 한다.
		//	
		if ( pChainInst->GetChain() == m_pProteinVistaRenderer->m_arrayProteinRibbonVertexData[i]->m_pChain &&
			numSegment == m_pProteinVistaRenderer->m_arrayProteinRibbonVertexData[i]->m_numSegment && 
			abs(curveTension - m_pProteinVistaRenderer->m_arrayProteinRibbonVertexData[i]->m_curveTension) < 0.01f ) 
		{
			pProteinRibbonVertexData = m_pProteinVistaRenderer->m_arrayProteinRibbonVertexData[i];
			break;
		}
	}

	if ( pProteinRibbonVertexData == NULL )
	{
		HRESULT hr;
		//	새롭게 만든다.
		pProteinRibbonVertexData = new CProteinRibbonVertexData();
		pProteinRibbonVertexData->Init(pChainInst->GetChain(), numSegment, curveTension);
		hr = pProteinRibbonVertexData->CreateRibbonVertexData();
		if ( SUCCEEDED(hr))
			m_pProteinVistaRenderer->m_arrayProteinRibbonVertexData.push_back(pProteinRibbonVertexData);
		else
		{
			SAFE_DELETE(pProteinRibbonVertexData);
			return E_FAIL;
		}
	}

	long	indexSelectionBegin = 0;
	long	indexSelectionEnd = 0;
	long	prevFlag = 0;

	for ( int iResidue = 0 ; iResidue <= pChainInst->m_arrayResidueInst.size() ; iResidue++ )
	{
		long currFlag;
		if ( iResidue == pChainInst->m_arrayResidueInst.size() )
			currFlag  = 0;
		else
			currFlag = pChainInst->m_arrayResidueInst[iResidue]->GetDisplayStyle(m_iDisplayStylePDB);

		if ( prevFlag == 0 && currFlag == 1 )
		{
			indexSelectionBegin = iResidue;
		}
		if ( prevFlag == 1 && currFlag == 0 )
		{
			indexSelectionEnd = iResidue-1;

			CRenderRibbonSelectionContainer * pRenderRibbonSelectionContainer;
			pRenderRibbonSelectionContainer = new CRenderRibbonSelectionContainer();
			pRenderRibbonSelectionContainer->m_pChainInst = pChainInst;
			pRenderRibbonSelectionContainer->m_iDisplaySelection = m_iDisplayStylePDB;
			pRenderRibbonSelectionContainer->m_pPDBRenderer = m_pPDBRenderer;
			pRenderRibbonSelectionContainer->m_pProteinVistaRenderer = m_pProteinVistaRenderer;		
			pRenderRibbonSelectionContainer->m_pRibbonVertexData = pProteinRibbonVertexData;
			pRenderRibbonSelectionContainer->Init( pChainInst , indexSelectionBegin, indexSelectionEnd );
			pRenderRibbonSelectionContainer->InitDeviceObjects();
			m_arrayRenderObjectSelection.push_back(pRenderRibbonSelectionContainer);
		}

		prevFlag = currFlag;
	}

	CleanUnusedRibbonModelMemory();

	return S_OK;
}

//	
//	각 체인에 대해서 1개씩 surface를 만들고. index buffer 만 각 selection 에 대해서 만든다.
//	
HRESULT	CSelectionDisplay::InitRenderSelectionSurface( CChainInst * pChainInst )
{
	long	modelID = pChainInst->GetChain()->m_iModel;
	long	chainID = pChainInst->GetChain()->m_chainID;
	CPDB * pPDB = pChainInst->m_pPDBInst->GetPDB();

	CPropertySurface * propertySurface = GetPropertySurface();

	double	probeSphere = propertySurface->m_probeSphere;
	long	quality = propertySurface->m_surfaceQuality;
	BOOL	bHETATM = propertySurface->m_bAddHETATM;
	long	genMethod = propertySurface->m_surfaceGenMethod;

	CProteinSurfaceBase * pProteinSurfaceBase = NULL;

	//
	//	pChain 에 대해서 surface를 이미 구하였는지 조사.
	//
	for ( int i = 0 ; i < m_pProteinVistaRenderer->m_arrayProteinSurface.size() ; i++ )
	{
		//
		//	생성 옵션을 전부 비교해야 한다.
		//	
		if ( pChainInst->GetChain() == m_pProteinVistaRenderer->m_arrayProteinSurface[i]->m_pChain &&
			quality ==		m_pProteinVistaRenderer->m_arrayProteinSurface[i]->m_surfaceQuality && 
			probeSphere == m_pProteinVistaRenderer->m_arrayProteinSurface[i]->m_probeSphere && 
			bHETATM ==		m_pProteinVistaRenderer->m_arrayProteinSurface[i]->m_bAddHETATM &&
			genMethod ==		m_pProteinVistaRenderer->m_arrayProteinSurface[i]->m_surfaceGenMethod )
		{
			pProteinSurfaceBase = m_pProteinVistaRenderer->m_arrayProteinSurface[i];
			break;
		}
	}

	if ( pProteinSurfaceBase == NULL )
	{
		if ( propertySurface->m_surfaceGenMethod == 0 )			//	 MQ	
		{
			HRESULT hr;
			//	새롭게 만든다.
			pProteinSurfaceBase = new CProteinSurfaceMQ();
			pProteinSurfaceBase->Init( pPDB , pChainInst->GetChain() , probeSphere, quality, bHETATM );
			hr = pProteinSurfaceBase->CreateSurface();
			if ( SUCCEEDED(hr) )
				m_pProteinVistaRenderer->m_arrayProteinSurface.push_back(pProteinSurfaceBase);
			else
			{
				SAFE_DELETE(pProteinSurfaceBase);
				return E_FAIL;
			}
		}
		else
		{
			HRESULT hr;
			//	새롭게 만든다.
			pProteinSurfaceBase = new CProteinSurfaceMSMS();
			pProteinSurfaceBase->Init( pPDB , pChainInst->GetChain() , probeSphere, quality, bHETATM );
			hr = pProteinSurfaceBase->CreateSurface();
			if ( SUCCEEDED(hr))
				m_pProteinVistaRenderer->m_arrayProteinSurface.push_back(pProteinSurfaceBase);
			else
			{
				SAFE_DELETE(pProteinSurfaceBase);
				return E_FAIL;
			}
		}
	}

	CRenderSurfaceSelection * pRenderSurfaceSelection = new CRenderSurfaceSelection();
	pRenderSurfaceSelection->m_pChainInst = pChainInst;
	pRenderSurfaceSelection->m_iDisplaySelection = m_iDisplayStylePDB;
	pRenderSurfaceSelection->m_pProteinSurface = pProteinSurfaceBase;
	pRenderSurfaceSelection->m_pPDBRenderer = m_pPDBRenderer;
	pRenderSurfaceSelection->m_pProteinVistaRenderer = m_pProteinVistaRenderer;

	pRenderSurfaceSelection->Init();

	//	rendering할 범위를 찾아 index 버퍼를 만든다.
	pRenderSurfaceSelection->InitDeviceObjects();

	//	컨테이너에 넣는다.
	m_arrayRenderObjectSelection.push_back(pRenderSurfaceSelection);

	//
	//	사용되지 않는 서피스를 지운다.
	//	
	CleanUnusedSurfaceModelMemory();

	return S_OK;
}

void CSelectionDisplay::CleanUnusedRibbonModelMemory()
{
	static int cleanupCount = 0;
	//
	//	사용되지 않는 surface를 free 시킨다.
	//
	//	0. 20번중에 한번 cleanup.
	//	1. 전체를 m_bUsed = FALSE 시킨다.
	//	2. 사용되는것만 m_bUsed = TRUE.
	//	3. m_bUsed 인것을 delete.
	//	
	if ( (++cleanupCount) % 40 == 0 )
	{
		int i,j,k;
		long iProgress = GetMainActiveView()->InitProgress(100);

		OutputTextMsg(_T("Garbage Collecting..."));

		//	 1.
		for ( i = 0 ; i < m_pProteinVistaRenderer->m_arrayPDBRenderer.size() ; i++ )
		{
			CSTLArrayCProteinRibbonVertexData &  arrayProteinRibbonVertexData = m_pProteinVistaRenderer->m_arrayProteinRibbonVertexData;
			for ( j = 0 ; j < arrayProteinRibbonVertexData.size() ; j++ )
			{
				arrayProteinRibbonVertexData[j]->m_bUsed = FALSE;
			}
		}

		//	2.
		for ( i = 0 ; i < m_pProteinVistaRenderer->m_arraySelectionDisplay.size(); i++ )
		{
			CSelectionDisplay * pSelectionDisplay = m_pProteinVistaRenderer->m_arraySelectionDisplay[i];
			if ( pSelectionDisplay == NULL ) continue;
			if ( pSelectionDisplay->m_displayStyle != RIBBON ) continue;

			for ( j = 0 ; j < pSelectionDisplay->m_arrayRenderObjectSelection.size() ; j++ )
			{
				CPropertyRibbon * propertyRibbon = pSelectionDisplay->GetPropertyRibbon();
				if ( propertyRibbon == NULL ) continue;

				long	numSegment = propertyRibbon->m_resolution*2;		//	반드시 even
				float	curveTension = (FLOAT)(propertyRibbon->m_curveTension)/(FLOAT)100.0f;

				CChainInst * pChainInst = pSelectionDisplay->m_arrayRenderObjectSelection[j]->m_pChainInst;

				CSTLArrayCProteinRibbonVertexData &  arrayProteinRibbonVertexData = m_pProteinVistaRenderer->m_arrayProteinRibbonVertexData;
				for ( k = 0 ; k < arrayProteinRibbonVertexData.size() ; k++ )
				{
					if ( pChainInst->GetChain() == arrayProteinRibbonVertexData[k]->m_pChain &&
						 numSegment == arrayProteinRibbonVertexData[k]->m_numSegment && 
						 abs( curveTension - arrayProteinRibbonVertexData[k]->m_curveTension ) < 0.01f )
					{
						arrayProteinRibbonVertexData[k]->m_bUsed = TRUE;
						break;
					}
				}
			}
		}

		//	3.
		for ( i = 0 ; i < m_pProteinVistaRenderer->m_arrayPDBRenderer.size() ; i++ )
		{
			GetMainActiveView()->SetProgress((long)(i*100/m_pProteinVistaRenderer->m_arrayPDBRenderer.size()),100);

			CSTLArrayCProteinRibbonVertexData &  arrayProteinRibbonVertexData = m_pProteinVistaRenderer->m_arrayProteinRibbonVertexData;
			for ( int j = 0 ; j < arrayProteinRibbonVertexData.size() ; j++ )
			{
				if ( arrayProteinRibbonVertexData[j]->m_bUsed == FALSE )
				{
					//	delete.
					SAFE_DELETE(arrayProteinRibbonVertexData[j]);
					arrayProteinRibbonVertexData.erase(arrayProteinRibbonVertexData.begin()+j );
					j = -1;
					continue;
				}
			}
		}

		GetMainActiveView()->EndProgress(iProgress);
		OutputTextMsg(_T("Finish Garbage Collecting."));
	}
}

void CSelectionDisplay::CleanUnusedSurfaceModelMemory()
{
	static int cleanupCount = 0;
	//
	//	사용되지 않는 surface를 free 시킨다.
	//
	//	0. 20번중에 한번 cleanup.
	//	1. 전체를 m_bUsed = FALSE 시킨다.
	//	2. 사용되는것만 m_bUsed = TRUE.
	//	3. m_bUsed 인것을 delete.
	//	
	if ( (++cleanupCount) % 40 == 0 )
	{
		int i,j,k;
		long iProgress = GetMainActiveView()->InitProgress(100);

		OutputTextMsg(_T("Garbage Collecting..."));

		//	 1.
		for ( i = 0 ; i < m_pProteinVistaRenderer->m_arrayPDBRenderer.size() ; i++ )
		{
			CSTLArrayProteinSurfaceBase &  arrayProteinSurface= m_pProteinVistaRenderer->m_arrayProteinSurface;
			for ( j = 0 ; j < arrayProteinSurface.size() ; j++ )
			{
				arrayProteinSurface[j]->m_bUsed = FALSE;
			}
		}

		//	2.
		for ( i = 0 ; i < m_pProteinVistaRenderer->m_arraySelectionDisplay.size(); i++ )
		{
			CSelectionDisplay * pSelectionDisplay = m_pProteinVistaRenderer->m_arraySelectionDisplay[i];
			if ( pSelectionDisplay == NULL ) continue;
			if ( pSelectionDisplay->m_displayStyle != SURFACE ) continue;

			for ( j = 0 ; j < pSelectionDisplay->m_arrayRenderObjectSelection.size() ; j++ )
			{
				CPropertySurface * propertySurface = pSelectionDisplay->GetPropertySurface();
				if ( propertySurface == NULL ) continue;

				CChainInst * pChainInst = pSelectionDisplay->m_arrayRenderObjectSelection[j]->m_pChainInst;
				long	modelID = pChainInst->GetChain()->m_iModel;
				long	chainID = pChainInst->GetChain()->m_chainID;
				long	quality = propertySurface->m_surfaceQuality;
				double	probeSphere = propertySurface->m_probeSphere;
				BOOL bHETATM = propertySurface->m_bAddHETATM;
				long	genMethod = propertySurface->m_surfaceGenMethod;

				CSTLArrayProteinSurfaceBase &  arrayProteinSurface = m_pProteinVistaRenderer->m_arrayProteinSurface;
				for ( k = 0 ; k < arrayProteinSurface.size() ; k++ )
				{
					if (	pChainInst->GetChain() == arrayProteinSurface[k]->m_pChain &&
							quality == arrayProteinSurface[k]->m_surfaceQuality && 
							probeSphere == arrayProteinSurface[k]->m_probeSphere && 
							bHETATM == arrayProteinSurface[k]->m_bAddHETATM &&
							genMethod == arrayProteinSurface[k]->m_surfaceGenMethod )
					{
						arrayProteinSurface[k]->m_bUsed = TRUE;
						break;
					}
				}
			}
		}

		//	3.
		for ( i = 0 ; i < m_pProteinVistaRenderer->m_arrayPDBRenderer.size() ; i++ )
		{
			GetMainActiveView()->SetProgress(i*100/m_pProteinVistaRenderer->m_arrayPDBRenderer.size(),100);

			CSTLArrayProteinSurfaceBase &  arrayProteinSurface = m_pProteinVistaRenderer->m_arrayProteinSurface;
			for ( int j = 0 ; j < arrayProteinSurface.size() ; j++ )
			{
				if ( arrayProteinSurface[j]->m_bUsed == FALSE )
				{
					//	delete.
					SAFE_DELETE(arrayProteinSurface[j]);
					arrayProteinSurface.erase(arrayProteinSurface.begin()+j );
					j = -1;
					continue;
				}
			}
		}

		GetMainActiveView()->EndProgress(iProgress);
		OutputTextMsg(_T("Finish Garbage Collecting."));
	}
}

void CSelectionDisplay::SelectSurfaceAtom()
{
	for ( int i = 0 ; i < m_arrayRenderObjectSelection.size() ; i++ )
	{
		CRenderSurfaceSelection * pRenderSurfaceSelection = dynamic_cast<CRenderSurfaceSelection *>(m_arrayRenderObjectSelection[i]);
		if ( pRenderSurfaceSelection )
		{
			CSTLIntArray & arrayIndexAtom = pRenderSurfaceSelection->m_pProteinSurface->m_arrayIndexAtom;
			for ( int j = 0 ; j < arrayIndexAtom.size(); j++ )
			{
				//	TODO: 수정
				CAtomInst * pAtom = pRenderSurfaceSelection->m_arrayAtomInst[arrayIndexAtom[j]];
				pAtom->SetSelect(TRUE);
			}
		}
	}
}

