#include "stdafx.h"
#include "ProteinVista.h"
#include "MatrixMath.h"


#include "ProteinVistaView.h"

#include "localSuperImpose.h"
#include "PDBRenderer.h"
#include "ProteinVistaRenderer.h"

#include "SelectionDisplay.h"
#include "RenderRibbonSelection.h"
#include "Interface.h"
#include "RibbonVertexData.h"
#include "Utility.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


CRenderRibbonSelectionContainer::CRenderRibbonSelectionContainer()
{
	m_propertyRibbon = NULL;
	m_beginResidue = m_endResidue = 0;

	m_pRibbonVertexData = NULL;
}

CRenderRibbonSelectionContainer::~CRenderRibbonSelectionContainer()
{
	DeleteDeviceObjects();

}

void CRenderRibbonSelectionContainer::Init(CChainInst * pChainInst, long beginResidue, long endResidue )
{
	m_pChainInst = pChainInst;
	m_beginResidue = beginResidue;
	m_endResidue = endResidue;

	//    m_endResidue에 HETATM이 들어가 있을수 있다.
	//    m_endResidue를 보정
	while(1)
	{
		if ( pChainInst->GetChain()->m_arrayResidue[m_endResidue]->m_bHETATM == TRUE )
		{
			m_endResidue --;
			if ( m_endResidue <= 0 )
			{
				break;
			}
		}
		else
			break;
	}

	CSelectionDisplay * pSelectionDisplay = m_pProteinVistaRenderer->m_arraySelectionDisplay[m_iDisplaySelection];
	m_pPropertyCommon = pSelectionDisplay->GetPropertyCommon();
	m_propertyRibbon = pSelectionDisplay->GetPropertyRibbon();
}

void CRenderRibbonSelectionContainer::SetModelQuality()
{
	//    notify message.
	long	resolution = m_pProteinVistaRenderer->m_renderQualityPreset.m_ribbonResolutionConst[m_pPropertyCommon->m_modelQuality];

	//    model quality
	//m_propertyRibbon->m_pResolution->SetNumber(resolution);
	//m_propertyRibbon->m_pResolution->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(m_propertyRibbon->m_pResolution));
}

/*
//	called Init()
//	direction, up, right vector를 구한다.
void CRenderRibbonSelectionContainer::MakeRibbonSkeletonVertex()
{
	D3DXVECTOR3	vecUpOld(1,0,0);

	if ( m_pChainInst->GetChain()->m_bDNA == TRUE )
	{
		//    chain 전체에 대해서 구해둔다. 부분만 selection을 하였을때 오류를 방지.
		for ( int i = 0 ; i < m_pChainInst->m_arrayResidueInst.size() ; i++ )
		{
			CResidueInst * pResidueInst = m_pChainInst->m_arrayResidueInst[i];

			if ( pResidueInst->GetResidue()->m_bHETATM == TRUE )
			{
				continue;
			}

			//	C1'
			//	C5'
			CAtomInst * pAtomC1 = NULL;
			CAtomInst * pAtomC5 = NULL;
			for ( int j = 0 ; j < pResidueInst->m_arrayAtomInst.size() ; j++ )
			{
				CString strAtomName = pResidueInst->m_arrayAtomInst[j]->GetAtom()->m_atomName;
				if ( strAtomName == _T(" C1'") || strAtomName == _T(" C1*") )
					pAtomC1 = pResidueInst->m_arrayAtomInst[j];
				if ( strAtomName == _T(" C5'") || strAtomName == _T(" C5*") )
					pAtomC5 = pResidueInst->m_arrayAtomInst[j];

				if ( pAtomC1 && pAtomC5 )
					break;
			}

			if ( pAtomC1 == NULL )
			{
				pAtomC1 = pResidueInst->m_arrayAtomInst[0];
			}

			if ( pAtomC5 == NULL )
			{
				pAtomC5 = pResidueInst->m_arrayAtomInst[0];
			}

			m_arrayCarbonAtom.push_back(pAtomC5->GetAtom()->m_pos);
			m_arrayResidueInst.push_back(pResidueInst);

			D3DXVECTOR3	vecUp(0,0,0);
			//	리본의 upvector 구하기.
			vecUp = pAtomC1->GetAtom()->m_pos - pAtomC5->GetAtom()->m_pos;
			D3DXVec3Normalize( &vecUp, &vecUp );

			m_arrayUpVec.push_back(vecUp);
		}
	}
	else
	{
		//    chain 전체에 대해서 구해둔다. 부분만 selection을 하였을때 오류를 방지.
		for ( int i = 0 ; i < m_pChainInst->m_arrayResidueInst.size() ; i++ )
		{
			CResidueInst * pResidueInst = m_pChainInst->m_arrayResidueInst[i];

			if ( pResidueInst->GetResidue()->m_bHETATM == TRUE )
			{
				continue;
			}

			if ( pResidueInst->GetResidue()->m_bExistMainChain == FALSE )
			{
				//    비는것이 있을 경우에 좌우값으로 보정을 한다.
				if ( i == 0 )
				{
					pResidueInst = m_pChainInst->m_arrayResidueInst[i+1];
				}
				else 
				{	//	 끝에 빈것
					pResidueInst = m_pChainInst->m_arrayResidueInst[i-1];
				}
			}

			D3DXVECTOR3 vec1, vec2, vec3, vec4(0,0,0);
			vec1 = pResidueInst->GetResidue()->GetNAtom()->m_pos;
			vec2 = pResidueInst->GetResidue()->GetCAAtom()->m_pos;
			//    vec3 = pResidueInst->GetOAtom()->m_pos;
			vec4 = ( pResidueInst->GetResidue()->GetCBAtom() ) ? pResidueInst->GetResidue()->GetCBAtom()->m_pos : vec2 ;

			m_arrayCarbonAtom.push_back(pResidueInst->GetResidue()->GetCAAtom()->m_pos);
			m_arrayResidueInst.push_back(pResidueInst);

			D3DXVECTOR3	vecUp(0,0,0);
			//	리본의 upvector 구하기. (vec Ca to R)
			vecUp = vec4-vec2;
			D3DXVec3Normalize( &vecUp, &vecUp );

			m_arrayUpVec.push_back(vecUp);
		}

		//    CB가 없는 경우 up-vector를 interpolation.
		for ( int i = 0 ; i < m_arrayUpVec.size() ; i++ )
		{
			if ( m_arrayUpVec[i] == D3DXVECTOR3(0,0,0) )
			{
				D3DXVECTOR3 vec1(0,0,0), vec2(0,0,0);
				if ( i > 0 )
					vec1 = m_arrayUpVec[i-1];
				if ( i < m_arrayUpVec.size()-1 )
					vec2 = m_arrayUpVec[i+1];

				D3DXVECTOR3 aveVec = (vec1+vec2)/2;
				D3DXVec3Normalize(&aveVec, &aveVec);
				m_arrayUpVec[i] = aveVec;
			}
		}
	}

	//    
	//    m_arrayRibbonCurve
	//    
	//    
	//    curve를 구한다.
	if ( m_arrayCarbonAtom.size() >= 2 )
	{
		m_arrayRibbonCurve.clear();
		m_arrayRibbonCurve.reserve( m_arrayCarbonAtom.size() * m_numSegment );
		GetCardianlCurvePoint( m_arrayCarbonAtom, m_arrayRibbonCurve, m_numSegment , m_curveTension );
	}

	//    curve 의 각각의 point의 up-vector와 direction-vector를 구한다.
	//    Up-vector는 각각의 ctrl-point에서 interpolation
	//    dir-vector는 주변값으로 구한다.
	if ( m_arrayRibbonCurve.size() > 0 )
	{
		m_arrayRibbonCurveUpVec.reserve(m_arrayRibbonCurve.size());
		m_arrayRibbonCurveDirVec.reserve(m_arrayRibbonCurve.size());

		for ( int i = 0 ; i < m_arrayRibbonCurve.size() ; i++ )
		{
			D3DXVECTOR3 vec1(0,0,0), vec2(0,0,0), vec3(0,0,0);

			vec1 = ( i != 0 )? m_arrayRibbonCurve[i-1]: m_arrayRibbonCurve[i];
			vec2 = m_arrayRibbonCurve[i];
			vec3 = ( i != m_arrayRibbonCurve.size()-1 )? m_arrayRibbonCurve[i+1]: m_arrayRibbonCurve[i];

			D3DXVECTOR3 dirVec = ((vec2-vec1) + (vec3-vec2))/2;
			D3DXVec3Normalize(&dirVec, &dirVec);
			m_arrayRibbonCurveDirVec.push_back(dirVec);
		}

		D3DXVECTOR3	vecUpOld(0,0,0);
		for ( int i = 0 ; i < m_arrayCarbonAtom.size() ; i++ )
		{
			D3DXVECTOR3 result;
			D3DXVec3Cross(&result, &m_arrayRibbonCurveDirVec[i*m_numSegment], &m_arrayUpVec[i] );
			D3DXVec3Cross(&result, &result, &m_arrayRibbonCurveDirVec[i*m_numSegment] );
			D3DXVec3Normalize(&result, &result);

			if ( D3DXVec3Dot(&result, &vecUpOld) < 0.0f )
				result = -result;

			m_arrayUpVec[i] = result;

			vecUpOld = result;
		}

		for ( int i = 0 ; i < ((int)m_arrayCarbonAtom.size())-1 ; i++ )
		{
			D3DXVECTOR3 upVec1 = m_arrayUpVec[i];
			D3DXVECTOR3 upVec2 = m_arrayUpVec[i+1];

			for ( int j = 0 ; j < m_numSegment ; j++ )
			{
				D3DXVECTOR3 vecUp = upVec2 * (FLOAT)j/(FLOAT)m_numSegment + upVec1 * (FLOAT)(m_numSegment-j)/(FLOAT)m_numSegment;
				D3DXVec3Normalize(&vecUp, &vecUp);
				m_arrayRibbonCurveUpVec.push_back(vecUp);
			}
		}

		m_arrayRibbonCurveUpVec.push_back( m_arrayUpVec[ m_arrayUpVec.size()-1 ] );
	}
}

//    helix cylinder 에 대한 skeleton vertex이다.
void CRenderRibbonSelectionContainer::MakeHelixCylinderSkeletonVertex()
{
	if ( m_arrayCarbonAtom.size() <= 0 )
		return;

	if ( m_arrayHelixOptimalCylinder.size() == 0 )
	{
		m_arrayHelixOptimalCylinder.resize(m_arrayRibbonCurve.size());
		m_arrayHelixOptimalCylinderDirVec.resize(m_arrayRibbonCurveDirVec.size());
		m_arrayHelixOptimalCylinderUpVec.resize(m_arrayRibbonCurveUpVec.size());

		m_arrayHelixTwoPointCylinder.resize(m_arrayRibbonCurve.size());
		m_arrayHelixTwoPointCylinderDirVec.resize(m_arrayRibbonCurveDirVec.size());
		m_arrayHelixTwoPointCylinderUpVec.resize(m_arrayRibbonCurveUpVec.size());
	}

	//	create coil, helix, sheet selection class
	//	
	long beginIndex = 0;
	long oldTypeSS = m_arrayResidueInst[0]->GetResidue()->GetSS();
	
	for ( int iAtom = 0 ; iAtom <= m_arrayCarbonAtom.size() ; iAtom++ )
	{
		long typeSS;
		if ( iAtom >= m_arrayCarbonAtom.size() )
			typeSS = -1;	//	마지막은 -1 로 항상 추가된다.
		else
			typeSS = m_arrayResidueInst[iAtom]->GetResidue()->GetSS();

		if ( typeSS != oldTypeSS )
		{
			if ( oldTypeSS == SS_HELIX )
			{
				long beginCarbonAtom = beginIndex;
				long endCarbonAtom = iAtom-1;

				//    선택된 부분에 helix/sheet 가 있는지 조사.
				if ( !((m_endResidue+2 < beginCarbonAtom) || (m_beginResidue-2 > endCarbonAtom) ) )
				{
					//    최적 fitting.
					CSTLArrayD3DXVECTOR3	arrayCarbonAtom;
					arrayCarbonAtom.reserve(endCarbonAtom-beginCarbonAtom+3);

					for ( int i = beginCarbonAtom; i <= endCarbonAtom ; i++ )
					{
						arrayCarbonAtom.push_back(m_arrayCarbonAtom[i]);
					}

					CSTLVectorValueArray arrayPosCylinder1;		//	직선에 fitting 하기 위한 임시 pos array.
					CSTLVectorValueArray arrayPosCylinder2;		//	직선에 fitting 하기 위한 임시 pos array.
					arrayPosCylinder1.reserve(endCarbonAtom-beginCarbonAtom+1);
					arrayPosCylinder2.reserve(endCarbonAtom-beginCarbonAtom+1);

					FLOAT len = D3DXVec3Length( &(m_arrayCarbonAtom[beginCarbonAtom]-m_arrayCarbonAtom[endCarbonAtom] ) );

					for ( int i = 0 ; i <= endCarbonAtom-beginCarbonAtom ; i++ )
					{
						D3DXVECTOR3 v(0,0,0);
						v.z = ((FLOAT)i/(endCarbonAtom-beginCarbonAtom))*len;
						arrayPosCylinder1.push_back(v);
					}

					D3DXMATRIX transform;
					//	직선에 최적 fit
					FLOAT minRmsd = CalcMatchResidueRmsd(arrayCarbonAtom , arrayPosCylinder1, transform );
					D3DXMatrixInverse(&transform, NULL, &transform);
					for ( int i = 0 ; i < arrayPosCylinder1.size() ; i++ )
					{
						D3DXVECTOR3 result;
						D3DXVec3TransformCoord(&result, &arrayPosCylinder1[i], &transform);
						arrayPosCylinder2.push_back(result);
					}

					CSTLVectorValueArray arrayCylinderNewPos;
					arrayCylinderNewPos.reserve(arrayPosCylinder2.size() * m_numSegment + 1 + m_numSegment );
					GetInterLinePoint ( arrayPosCylinder2, arrayCylinderNewPos, m_numSegment );

					//    find direction vector.
					D3DXVECTOR3 dirVec;
					dirVec = arrayPosCylinder2[arrayPosCylinder2.size()-1] - arrayPosCylinder2[0];
					D3DXVec3Normalize(&dirVec, &dirVec);

					//    find UpVec
					D3DXVECTOR3 upVec;
					D3DXVec3Cross(&upVec, &dirVec, &D3DXVECTOR3(0,0,10));

					long	numHalfSegment = m_numSegment/2;

					//    cylinder에 양쪽으로 늘어난것을 추가한다.
					//    arrayCylinderNewPos
					//    
					FLOAT lenOneSegment = D3DXVec3Length(&(arrayCylinderNewPos[0]-arrayCylinderNewPos[1]));	//	하나 간격의 길이.//    
					D3DXVECTOR3 beginVertexPos = arrayCylinderNewPos[0];
					D3DXVECTOR3 endVertexPos = arrayCylinderNewPos[arrayCylinderNewPos.size()-1];
					for ( int i = 0 ; i < numHalfSegment ; i++ )
					{
						if ( beginCarbonAtom != 0 )
						{	//	처음꺼면 추가하지 않는다.
							D3DXVECTOR3 vecNewPosLeft = beginVertexPos + (-dirVec)*( lenOneSegment*(i+1) );
							arrayCylinderNewPos.insert(arrayCylinderNewPos.begin(), 1 ,   vecNewPosLeft);
						}

						if ( endCarbonAtom != m_arrayCarbonAtom.size()-1 )	
						{	//	마지막꺼면 추가하지 않는다.
							D3DXVECTOR3 vecNewPosRight = endVertexPos + (dirVec)*( lenOneSegment*(i+1) );
							arrayCylinderNewPos.push_back(vecNewPosRight);
						}
					}

					//    
					if ( beginCarbonAtom == 0 )		
						numHalfSegment = 0;

					for ( int i = 0 ; i < arrayCylinderNewPos.size() ; i++ )
					{
						m_arrayHelixOptimalCylinder[beginCarbonAtom*m_numSegment + i - numHalfSegment] = arrayCylinderNewPos[i];
						m_arrayHelixOptimalCylinderDirVec[beginCarbonAtom*m_numSegment + i - numHalfSegment] = dirVec;
						m_arrayHelixOptimalCylinderUpVec[beginCarbonAtom*m_numSegment + i - numHalfSegment] = upVec;
					}

					arrayPosCylinder2.clear();
					arrayPosCylinder2.reserve(arrayPosCylinder1.size());

					//===============================================================
					//    시작점 끝점 fitting.
					//
					len = D3DXVec3Length( &(m_arrayCarbonAtom[beginCarbonAtom]-m_arrayCarbonAtom[endCarbonAtom] ) );

					CSTLVectorValueArray arrayPos1;
					arrayPos1.push_back ( m_arrayCarbonAtom[beginCarbonAtom] );
					arrayPos1.push_back ( m_arrayCarbonAtom[endCarbonAtom] );
					CSTLVectorValueArray arrayPos2;
					arrayPos2.push_back ( D3DXVECTOR3(0,0,0) );
					arrayPos2.push_back ( D3DXVECTOR3(0,0,len) );

					minRmsd = CalcMatchResidueRmsd(arrayPos1 , arrayPos2, transform );
					D3DXMatrixInverse(&transform, NULL, &transform);
					for ( int i = 0 ; i < arrayPosCylinder1.size() ; i++ )
					{
						D3DXVECTOR3 result;
						D3DXVec3TransformCoord(&result, &arrayPosCylinder1[i], &transform);
						arrayPosCylinder2.push_back(result);
					}

					arrayCylinderNewPos.clear();
					arrayCylinderNewPos.reserve(arrayPosCylinder2.size() * m_numSegment + 1);
					GetInterLinePoint( arrayPosCylinder2, arrayCylinderNewPos, m_numSegment );

					dirVec = arrayPosCylinder2[arrayPosCylinder2.size()-1] - arrayPosCylinder2[0];
					D3DXVec3Normalize(&dirVec, &dirVec);

					//    find UpVec
					D3DXVec3Cross(&upVec, &dirVec, &D3DXVECTOR3(0,0,10));

					numHalfSegment = m_numSegment/2;

					//    cylinder에 양쪽으로 늘어난것을 추가한다.
					//    arrayCylinderNewPos
					//    
					lenOneSegment = D3DXVec3Length(&(arrayCylinderNewPos[0]-arrayCylinderNewPos[1]));	//	하나 간격의 길이.//    
					beginVertexPos = arrayCylinderNewPos[0];
					endVertexPos = arrayCylinderNewPos[arrayCylinderNewPos.size()-1];
					for ( int i = 0 ; i < numHalfSegment ; i++ )
					{
						if ( beginCarbonAtom != 0 )
						{	//	처음꺼면 추가하지 않는다.
							D3DXVECTOR3 vecNewPosLeft = beginVertexPos + (-dirVec)*( lenOneSegment*(i+1) );
							arrayCylinderNewPos.insert(arrayCylinderNewPos.begin(), 1 ,   vecNewPosLeft);
						}

						if ( endCarbonAtom != m_arrayCarbonAtom.size()-1 )
						{	//	마지막꺼면 추가하지 않는다.
							D3DXVECTOR3 vecNewPosRight = endVertexPos + (dirVec)*( lenOneSegment*(i+1) );
							arrayCylinderNewPos.push_back(vecNewPosRight);
						}
					}

					//  
					if ( beginCarbonAtom == 0 )		
						numHalfSegment = 0;

					for ( int i = 0 ; i < arrayCylinderNewPos.size() ; i++ )
					{
						m_arrayHelixTwoPointCylinder[beginCarbonAtom*m_numSegment + i - numHalfSegment] = arrayCylinderNewPos[i];
						m_arrayHelixTwoPointCylinderDirVec[beginCarbonAtom*m_numSegment + i - numHalfSegment] = dirVec;
						m_arrayHelixTwoPointCylinderUpVec[beginCarbonAtom*m_numSegment + i - numHalfSegment] = upVec;
					}
				}
			}

			beginIndex = iAtom;
			oldTypeSS = typeSS;
		}
	}
}
*/

//
//
//
#define CallVectorAllElement(_vec_,_func_) 	\
	for ( unsigned __i = 0 ; __i < _vec_ .size() ; __i++ ) _vec_ [__i]->_func_();									

HRESULT		CRenderRibbonSelectionContainer::InitDeviceObjects()
{
	DeleteDeviceObjects();

	if ( m_beginResidue > m_endResidue )
	{
		return S_OK;
	}

	if ( m_pChainInst->m_arrayResidueInst.size() <= 1 )
	{
		return S_OK;
	}

	m_arrayResidueInst.reserve(m_pChainInst->m_arrayResidueInst.size());
	//m_arrayCarbonAtom.reserve(m_pChainInst->m_arrayResidueInst.size());
	//	m_arrayUpVec.reserve(m_pChainInst->m_arrayResidueInst.size());

	//    하나의 ribbon selection 안에서 일정.
	m_numSegment = m_propertyRibbon->m_resolution*2;		//	반드시 even
	m_curveTension = (FLOAT)(m_propertyRibbon->m_curveTension)/(FLOAT)100.0f;

	m_pRibbonVertexData->GetRibbonResidueInst ( m_pChainInst, m_arrayResidueInst );

	//    기본 코일의 뼈때를 만든다.
	//	MakeRibbonSkeletonVertex();
	//    cylinder 헬릭스 모양의 뼈대를 만든다.
	//	MakeHelixCylinderSkeletonVertex();

	if ( m_arrayResidueInst.size() <= m_beginResidue )
	{
		return S_OK;
	}

	//	create coil, helix, sheet selection class
	long oldTypeSS;
	long beginIndex = m_beginResidue;

	if ( m_pRibbonVertexData->m_arrayCarbonAtom.size() > 0 )
	{
		oldTypeSS = m_arrayResidueInst[m_beginResidue]->GetResidue()->GetSS();

		for ( int i = m_beginResidue ; i <= m_endResidue+1 ; i++ )
		{
			long typeSS;
			if ( i > m_endResidue )
				typeSS = -1;	//	마지막은 -1 로 항상 추가된다.
			else
				typeSS = m_arrayResidueInst[i]->GetResidue()->GetSS();

			if ( typeSS != oldTypeSS )
			{
				CRenderRibbonSelection * pRenderRibbonSelection = NULL;
				CRenderRibbonSelection * pRenderRibbonSelectionCoilInHelix = NULL;
				CRenderRibbonSelection * pRenderRibbonSelectionCoilInSheet = NULL;

				if ( oldTypeSS == SS_HELIX )
				{
					pRenderRibbonSelection = new CRenderHelixSelection();
					pRenderRibbonSelectionCoilInHelix = new CRenderCoilSelection();
				}
				else if ( oldTypeSS == SS_SHEET )
				{
					pRenderRibbonSelection = new CRenderSheetSelection();
					pRenderRibbonSelectionCoilInSheet = new CRenderCoilSelection();
				}
				else if ( oldTypeSS == SS_NONE )
				{
					pRenderRibbonSelection = new CRenderCoilSelection();
				}

				pRenderRibbonSelection->m_iBeginCarbonAtom =  beginIndex;
				pRenderRibbonSelection->m_iEndCarbonAtom = i-1;
				pRenderRibbonSelection->Init(this);

				if ( pRenderRibbonSelectionCoilInHelix )
				{
					pRenderRibbonSelectionCoilInHelix->m_iBeginCarbonAtom =  beginIndex;
					pRenderRibbonSelectionCoilInHelix->m_iEndCarbonAtom = i-1;
					pRenderRibbonSelectionCoilInHelix->Init(this);
					pRenderRibbonSelectionCoilInHelix->m_bCoilInHelix = TRUE;
				}

				if ( pRenderRibbonSelectionCoilInSheet )
				{
					pRenderRibbonSelectionCoilInSheet->m_iBeginCarbonAtom =  beginIndex;
					pRenderRibbonSelectionCoilInSheet->m_iEndCarbonAtom = i-1;
					pRenderRibbonSelectionCoilInSheet->Init(this);
					pRenderRibbonSelectionCoilInSheet->m_bCoilInSheet = TRUE;
				}

				if ( oldTypeSS == SS_HELIX )
				{
					m_arrayRenderHelixSelection.push_back((CRenderHelixSelection*)pRenderRibbonSelection);
					m_arrayRenderCoilSelection.push_back((CRenderCoilSelection*)pRenderRibbonSelectionCoilInHelix);
				}
				else if ( oldTypeSS == SS_SHEET )
				{
					m_arrayRenderSheetSelection.push_back((CRenderSheetSelection*)pRenderRibbonSelection);
					m_arrayRenderCoilSelection.push_back((CRenderCoilSelection*)pRenderRibbonSelectionCoilInSheet);
				}
				else if ( oldTypeSS == SS_NONE )
				{
					m_arrayRenderCoilSelection.push_back((CRenderCoilSelection*)pRenderRibbonSelection);
				}

				beginIndex = i;
				oldTypeSS = typeSS;
			}

			//	add DNA sugar.
			//	m_arrayRenderDNASelection
			//if ( m_pChainInst->m_bDNA == TRUE && i <= m_endResidue )
			//{
			//	CAtom * pC5 = m_arrayResidueInst[i]->GetC5Atom();
			//	if ( pC5 )
			//	{
			//		CRenderDNASelection * pRenderDNASelection = new CRenderDNASelection;
			//		pRenderDNASelection->Init(this);
			//		pRenderDNASelection->m_posBegin = pC5->m_pos;
			//		//	pRenderDNASelection->m_posEnd = 
			//		m_arrayRenderDNASelection.push_back(pRenderDNASelection);
			//	}
			//}
		}
	}

	CallVectorAllElement( m_arrayRenderCoilSelection, InitDeviceObjects ); 
	CallVectorAllElement( m_arrayRenderHelixSelection, InitDeviceObjects ); 
	CallVectorAllElement( m_arrayRenderSheetSelection, InitDeviceObjects ); 

	UpdateAtomSelectionChanged();

	return S_OK;
}

HRESULT		CRenderRibbonSelectionContainer::DeleteDeviceObjects()
{
	CallVectorAllElement( m_arrayRenderCoilSelection, ResetTexture ); 
	CallVectorAllElement( m_arrayRenderHelixSelection, ResetTexture ); 
	CallVectorAllElement( m_arrayRenderSheetSelection, ResetTexture ); 

	CallVectorAllElement( m_arrayRenderCoilSelection, DeleteDeviceObjects ); 
	CallVectorAllElement( m_arrayRenderHelixSelection, DeleteDeviceObjects ); 
	CallVectorAllElement( m_arrayRenderSheetSelection, DeleteDeviceObjects ); 

	for ( int i = 0 ; i < m_arrayRenderCoilSelection.size() ; i++ )
		delete m_arrayRenderCoilSelection[i];
	m_arrayRenderCoilSelection.clear();
	for ( int i = 0 ; i < m_arrayRenderHelixSelection.size() ; i++ )
		delete m_arrayRenderHelixSelection[i];
	m_arrayRenderHelixSelection.clear();
	for ( int i = 0 ; i < m_arrayRenderSheetSelection.size() ; i++ )
		delete m_arrayRenderSheetSelection[i];
	m_arrayRenderSheetSelection.clear();

	//	m_arrayCarbonAtom.clear();
	//	m_arrayUpVec.clear();

	//m_arrayRibbonCurve.clear();
	//m_arrayRibbonCurveUpVec.clear();
	//m_arrayRibbonCurveDirVec.clear();

	////    
	//m_arrayHelixOptimalCylinder.clear();
	//m_arrayHelixOptimalCylinderDirVec.clear();
	//m_arrayHelixOptimalCylinderUpVec.clear();

	////
	//m_arrayHelixTwoPointCylinder.clear();
	//m_arrayHelixTwoPointCylinderDirVec.clear();
	//m_arrayHelixTwoPointCylinderUpVec.clear();

	m_arrayResidueInst.clear();

	return S_OK;
}

HRESULT		CRenderRibbonSelectionContainer::Render()
{
	CallVectorAllElement( m_arrayRenderCoilSelection, Render ); 
	CallVectorAllElement( m_arrayRenderHelixSelection, Render ); 
	CallVectorAllElement( m_arrayRenderSheetSelection, Render ); 

	return S_OK;
}

HRESULT		CRenderRibbonSelectionContainer::UpdateAtomSelectionChanged()
{
	CallVectorAllElement( m_arrayRenderCoilSelection, UpdateAtomSelectionChanged ); 
	CallVectorAllElement( m_arrayRenderHelixSelection, UpdateAtomSelectionChanged ); 
	CallVectorAllElement( m_arrayRenderSheetSelection, UpdateAtomSelectionChanged ); 

	return S_OK;
}

void		CRenderRibbonSelectionContainer::Picking(D3DXVECTOR3 &pickRayDir, D3DXVECTOR3 &pickRayOrig, CSTLArrayPickedResidueInst & pickResidueArray )
{
	for ( int j = 0 ; j < m_arrayRenderCoilSelection.size(); j++ )
	{
		CRenderCoilSelection * coilSelection = m_arrayRenderCoilSelection[j];
		coilSelection->Picking( pickRayDir, pickRayOrig, pickResidueArray );
	}

	for ( int j = 0 ; j < m_arrayRenderSheetSelection.size() ; j++ )
	{
		CRenderSheetSelection * sheetSelection = m_arrayRenderSheetSelection[j];
		sheetSelection->Picking( pickRayDir, pickRayOrig, pickResidueArray );
	}

	for ( int j = 0 ; j < m_arrayRenderHelixSelection.size(); j++ )
	{
		CRenderHelixSelection * helixSelection = m_arrayRenderHelixSelection[j];
		helixSelection->Picking( pickRayDir, pickRayOrig, pickResidueArray );
	}
}

void CRenderRibbonSelectionContainer::ResetTexture()
{
	CallVectorAllElement( m_arrayRenderCoilSelection, ResetTexture); 
	CallVectorAllElement( m_arrayRenderHelixSelection, ResetTexture); 
	CallVectorAllElement( m_arrayRenderSheetSelection, ResetTexture); 
}

//=========================================================================================================
//=========================================================================================================

static D3DVERTEXELEMENT9 g_VertexElemRibbon[] = 
{
	{ 0, 0,     D3DDECLTYPE_FLOAT3,     D3DDECLMETHOD_DEFAULT,  D3DDECLUSAGE_POSITION,  0 },
	{ 0, 3 * 4, D3DDECLTYPE_FLOAT3,     D3DDECLMETHOD_DEFAULT,  D3DDECLUSAGE_NORMAL,    0 },
	{ 0, 6 * 4, D3DDECLTYPE_FLOAT2,     D3DDECLMETHOD_DEFAULT,  D3DDECLUSAGE_TEXCOORD,  0 },

	{ 0, 8 * 4, D3DDECLTYPE_FLOAT4,     D3DDECLMETHOD_DEFAULT,  D3DDECLUSAGE_COLOR ,    0 },
	D3DDECL_END()
};

CRenderRibbonSelection::CRenderRibbonSelection()
{
	m_sizeWidth = m_sizeHeight = 0.2f;

	m_pVB = NULL;
	m_pVBCap = NULL;
	m_pIB = NULL;

	m_pRenderRibbonSelectionContainer = NULL;

	m_pPDBRenderer = NULL;

	m_pVertexDeclRibbon = NULL;

	m_iBeginCarbonAtom = 0;
	m_iEndCarbonAtom = 0;

	m_bCoilInHelix = FALSE;
	m_bCoilInSheet = FALSE;

	m_pD3DXTextureRibbon = NULL;
}

CRenderRibbonSelection::~CRenderRibbonSelection()
{
	DeleteDeviceObjects();
}

void CRenderRibbonSelection::Init( CRenderRibbonSelectionContainer * pRenderRibbonSelectionContainer )
{
	m_pRenderRibbonSelectionContainer = pRenderRibbonSelectionContainer;
	m_pProteinVistaRenderer = m_pRenderRibbonSelectionContainer->m_pProteinVistaRenderer;

	m_pPDBRenderer = m_pRenderRibbonSelectionContainer->m_pPDBRenderer;
}

HRESULT		CRenderRibbonSelection::InitDeviceObjects()
{
	HRESULT hr;
	DeleteDeviceObjects();

	GetModelPropertyValue();

	GetD3DDevice()->CreateVertexDeclaration( g_VertexElemRibbon , &m_pVertexDeclRibbon );
	MakeVertexBuffer();
	MakeIndexBuffer();

	UpdateAtomSelectionChanged();
	UpdateRibbonColor();

	return S_OK;
}

void CRenderRibbonSelection::Picking(D3DXVECTOR3 &pickRayDir, D3DXVECTOR3 &pickRayOrig, CSTLArrayPickedResidueInst & pickResidueArray )
{
	CSTLArrayD3DXVECTOR3 * ribbonCurve = NULL;
	CSTLArrayD3DXVECTOR3 * ribbonCurveDir = NULL;
	CSTLArrayD3DXVECTOR3 * ribbonCurveUp = NULL;

	//	helix & sheet shape. default.
	ribbonCurve = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayRibbonCurve);
	ribbonCurveDir = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayRibbonCurveDirVec);
	ribbonCurveUp = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayRibbonCurveUpVec);

	if ( GetSSType() == SS_HELIX )
	{
		if ( m_fittingMethodRibbon == 0 )
		{	//	optimal cylinder fit
			ribbonCurve = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayHelixOptimalCylinder);
			ribbonCurveDir = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayHelixOptimalCylinderDirVec);
			ribbonCurveUp = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayHelixOptimalCylinderUpVec);
		}
		else if ( m_fittingMethodRibbon == 1 )
		{	//	 begin, end fit
			ribbonCurve = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayHelixTwoPointCylinder);
			ribbonCurveDir = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayHelixTwoPointCylinderDirVec);
			ribbonCurveUp = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayHelixTwoPointCylinderUpVec);
		}
	}

	long	halfNum = m_numSegment/2;
	for ( int i = m_iBeginCarbonAtom*m_numSegment-halfNum ; i <= (m_iEndCarbonAtom*m_numSegment)+halfNum ; i++ )
	{
		if ( i < 0 )	continue;
		if ( i >= m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayRibbonCurve.size() ) continue;

		//    0.6 은 pos 가 겹치기 때문에 조정되는 값.
		float selectionDelta = max(m_sizeWidth, m_sizeHeight)*0.6;

		D3DXVECTOR3 & pos = (*ribbonCurve)[i];
		FLOAT fLen = CalcLenPointToLine ( pos, pickRayDir, pickRayOrig );
		if ( fLen < selectionDelta )
		{
			CPickResidueInst	pickResidue;

			pickResidue.m_pos = pos;

			int iResidue = (i+halfNum) / m_numSegment;
			pickResidue.m_pResidueInst = m_pRenderRibbonSelectionContainer->m_arrayResidueInst[iResidue];

			D3DXVECTOR3 vecLen = pickRayOrig-pos;
			pickResidue.m_len = D3DXVec3Length(&vecLen);

			pickResidueArray.push_back(pickResidue);
		}
	}
}


HRESULT		CRenderRibbonSelection::MakeVertexBuffer()
{
	//	기본 도형을 만든다.
	long	index = 0;

	//	타원
	D3DXVECTOR3 * vec = NULL;
	vec = new D3DXVECTOR3 [m_numRibbonSlices];
	for ( long iangle = 0 ; iangle < m_numRibbonSlices ; iangle ++ )
	{
		float theta = (FLOAT)(iangle) * (360/m_numRibbonSlices) + 45;
		vec[index].y = (FLOAT)(m_sizeHeight* sin(D3DXToRadian(theta)));
		vec[index].z = 0.0f;
		vec[index].x = (FLOAT)(m_sizeWidth* cos(D3DXToRadian(theta)));
		index ++;
	}

	if ( m_numRibbonSlices == 3 || m_numRibbonSlices == 4 )
	{
		CSTLArrayD3DXVECTOR3 arrayCtrlPoint;
		arrayCtrlPoint.reserve(m_numRibbonSlices+1);
		for ( int i = 0 ; i < m_numRibbonSlices ; i++ )
			arrayCtrlPoint.push_back(vec[i]);
		arrayCtrlPoint.push_back(vec[0]);	

		CSTLArrayD3DXVECTOR3 m_arrayVertex;
		GetInterLinePoint( arrayCtrlPoint, m_arrayVertex , 6 );

		delete [] vec;

		m_numRibbonSlices = m_arrayVertex.size()-1;
		vec = new D3DXVECTOR3 [m_numRibbonSlices];

		for ( int i = 0 ; i < m_numRibbonSlices ; i++ )
		{
			vec[i] = m_arrayVertex[i];
		}
	}

	CSTLArrayD3DXVECTOR3 * ribbonCurve = NULL;
	CSTLArrayD3DXVECTOR3 * ribbonCurveDir = NULL;
	CSTLArrayD3DXVECTOR3 * ribbonCurveUp = NULL;

	//	helix & sheet shape. default.
	ribbonCurve = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayRibbonCurve);
	ribbonCurveDir = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayRibbonCurveDirVec);
	ribbonCurveUp = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayRibbonCurveUpVec);
	
	if ( GetSSType() == SS_HELIX )
	{
		if ( m_fittingMethodRibbon == 0 )
		{	//	optimal cylinder fit
			ribbonCurve = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayHelixOptimalCylinder);
			ribbonCurveDir = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayHelixOptimalCylinderDirVec);
			ribbonCurveUp = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayHelixOptimalCylinderUpVec);
		}
		else if ( m_fittingMethodRibbon == 1 )
		{	//	 begin, end fit
			ribbonCurve = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayHelixTwoPointCylinder);
			ribbonCurveDir = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayHelixTwoPointCylinderDirVec);
			ribbonCurveUp = &(m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayHelixTwoPointCylinderUpVec);
		}
	}

	//
	CSTLArrayD3DXVECTOR3 arrayVertexPos[30];
	for ( int i = 0 ; i < m_numRibbonSlices ; i++ )
	{
		arrayVertexPos[i].reserve( (m_iEndCarbonAtom-m_iBeginCarbonAtom+1)* m_numSegment + 1 );
	}

	long	halfNum = m_numSegment/2;

	D3DXVECTOR3 vecCapCenter1(-10000,-10000,-10000);
	D3DXVECTOR3 vecCapCenter2(0,0,0);

	for ( int i = m_iBeginCarbonAtom*m_numSegment-halfNum ; i <= (m_iEndCarbonAtom*m_numSegment)+halfNum ; i++ )
	{
		if ( i < 0 )	continue;
		if ( i >= m_pRenderRibbonSelectionContainer->m_pRibbonVertexData->m_arrayRibbonCurve.size() ) continue;

		if ( vecCapCenter1 == D3DXVECTOR3(-10000,-10000,-10000) )
		{
			//    처음 pos 가 담긴다.
			vecCapCenter1 = (*ribbonCurve)[i];
		}

		//    마지막 pos 이 담긴다.
		vecCapCenter2 = (*ribbonCurve)[i];

		D3DXMATRIX	matRot;
		D3DXVECTOR3 result;
		D3DXVECTOR3 upVec = (*ribbonCurveUp)[i];
		D3DXVECTOR3 dirVec = (*ribbonCurveDir)[i];	//	m_pRenderRibbonSelectionContainer->m_arrayRibbonCurveDirVec[i];

		//	direction vector와 up vector를 가지고 matRot 를 만든다.
		//  GetRotMatrixFromVec(&upVec, &dirVec, &matRot);
		D3DXVECTOR3 vecAt(0,0,0);
		GetMatrixFromVec(&dirVec, &vecAt, &upVec, &matRot);
		matRot._41 = matRot._42 = matRot._43 = 0.0f;

		for ( int iSlice = 0 ; iSlice < m_numRibbonSlices ; iSlice ++ )
		{
			D3DXVec3TransformCoord(&result, &vec[iSlice], &matRot);
			result += (*ribbonCurve)[i];			//	m_pRenderRibbonSelectionContainer->m_arrayRibbonCurve[i];

			arrayVertexPos[iSlice].push_back(result);
		}
	}

	delete [] vec;

	//
	//
	m_numRibbonLen = arrayVertexPos[0].size();
	m_totalVertex = m_numRibbonSlices * m_numRibbonLen;
	//	make vertex buffer.
	HRESULT hr = GetD3DDevice()->CreateVertexBuffer (	m_totalVertex*sizeof(CRibbonVertex) , 0, 0, D3DPOOL_MANAGED, &m_pVB , NULL );
	if ( FAILED(hr) )
	{
		OutputTextMsg ("Fail to make a vertex buffer");
		return hr;
	}

	CRibbonVertex *	pVertex;
	hr = m_pVB->Lock(0,0, (VOID**)&pVertex, 0 );

	for ( i = 0 ; i < m_totalVertex ; i++ )
	{
		pVertex[i].pos = arrayVertexPos[i%m_numRibbonSlices].at(i/m_numRibbonSlices);
	}

	//	calc normal
	for ( i = 0 ; i < m_totalVertex ; i++ )
	{
		D3DXVECTOR3 v0 = pVertex[i].pos;

		D3DXVECTOR3 v1 = pVertex[(i/m_numRibbonSlices)*m_numRibbonSlices + (i+1)%m_numRibbonSlices].pos;
		D3DXVECTOR3 v2 = pVertex[(i/m_numRibbonSlices)*m_numRibbonSlices + (i+m_numRibbonSlices-1)%m_numRibbonSlices].pos;
		D3DXVECTOR3 v3 = pVertex[(i+m_numRibbonSlices)%m_totalVertex].pos;
		D3DXVECTOR3 v4 = pVertex[(i+m_totalVertex-m_numRibbonSlices)%m_totalVertex].pos;

		D3DXVECTOR3 nv1 = (v1-v0);
		D3DXVECTOR3 nv2 = (v2-v0);
		D3DXVECTOR3 nv3 = (v3-v0);
		D3DXVECTOR3 nv4 = (v4-v0);

		D3DXVec3Normalize(&nv1, &nv1 );
		D3DXVec3Normalize(&nv2, &nv2 );
		D3DXVec3Normalize(&nv3, &nv3 );
		D3DXVec3Normalize(&nv4, &nv4 );

		D3DXVECTOR3 normal;
		D3DXVECTOR3 outNormal1,outNormal2,outNormal3,outNormal4 ;
		//    맨처음과 끝은 vector 가 1개씩 모자른다.
		if ( i < m_numRibbonSlices )
		{	//	처음
			D3DXVec3Cross( &outNormal1, &nv3, &nv1 );
			D3DXVec3Cross( &outNormal4, &nv2, &nv3 );

			normal = (outNormal1 + outNormal4)/2;
			D3DXVec3Normalize(&normal, &normal);
		}
		else if ( i >= m_totalVertex-m_numRibbonSlices )
		{
			D3DXVec3Cross( &outNormal2, &nv1, &nv4 );
			D3DXVec3Cross( &outNormal3, &nv4, &nv2 );

			normal = (outNormal2 + outNormal3 )/2;
			D3DXVec3Normalize(&normal, &normal);
		}
		else
		{
			D3DXVec3Cross( &outNormal1, &nv3, &nv1 );
			D3DXVec3Cross( &outNormal2, &nv1, &nv4 );
			D3DXVec3Cross( &outNormal3, &nv4, &nv2 );
			D3DXVec3Cross( &outNormal4, &nv2, &nv3 );

			normal = (outNormal1 + outNormal2 + outNormal3 + outNormal4)/4;
			D3DXVec3Normalize(&normal, &normal);
		}

		pVertex[i].normal = normal;
	}

	//    texcoordU는 항상 짝수
	FLOAT texcoordU = m_textureCoordURibbon*2;
	//	FLOAT texcoordV = m_textureCoordVRibbon/40.0f;

	for ( i = 0 ; i < m_totalVertex ; i++ )
	{
		FLOAT tu = (FLOAT)(i%m_numRibbonSlices)/(FLOAT)m_numRibbonSlices * texcoordU;	

		//    예로 U 가 4일때,
		//    0,1,2,3,0 으로 생성되는 tu를 0,1,2,1,0 으로 바꿔준다.
		if( tu > texcoordU/2 )
			pVertex[i].tu = texcoordU/2-(tu-texcoordU/2);
		else
			pVertex[i].tu = tu;

		//	ORIG.
		//	pVertex[i].tv = (m_totalVertex/m_numRibbonSlices * texcoordV)-(float)( i/m_numRibbonSlices * texcoordV );

		//	pVertex[i].tv  = (i/m_numRibbonSlices) / (float)m_numSegment * (FLOAT)m_textureCoordVRibbon;

		pVertex[i].tv  = (i/m_numRibbonSlices) / (float)m_numSegment * ((FLOAT)m_textureCoordVRibbon/8.0f);
	}

	m_pVB->Unlock();

	//
	//    SS의 cap을 만든다
	//
	hr = GetD3DDevice()->CreateVertexBuffer (	((m_numRibbonSlices+2)*2)*sizeof(CRibbonVertex) , 0, 0, D3DPOOL_MANAGED, &m_pVBCap , NULL );
	if ( FAILED(hr) )
	{
		OutputTextMsg("Fail to make a vertex buffer");
		return hr;
	}

	//    normal
	D3DXVECTOR3 v1 = arrayVertexPos[0].at(0);
	D3DXVECTOR3 v2 = arrayVertexPos[m_numRibbonSlices/3].at(0);
	D3DXVECTOR3 v3 = arrayVertexPos[(m_numRibbonSlices*2)/3].at(0);
	D3DXVECTOR3 normal1;
	D3DXVec3Cross ( &normal1, &(v2-v1), &(v3-v1) );
	D3DXVec3Normalize(&normal1, &normal1 );

	//    normal
	D3DXVECTOR3 v4 = arrayVertexPos[0].at(m_numRibbonLen-1);
	D3DXVECTOR3 v5 = arrayVertexPos[m_numRibbonSlices/3].at(m_numRibbonLen-1);
	D3DXVECTOR3 v6 = arrayVertexPos[(m_numRibbonSlices*2)/3].at(m_numRibbonLen-1);
	D3DXVECTOR3 normal2;
	D3DXVec3Cross ( &normal2, &(v6-v4), &(v5-v4) );
	D3DXVec3Normalize(&normal2, &normal2);

	hr = m_pVBCap->Lock(0,0, (VOID**)&pVertex, 0 );

	pVertex[0].pos = vecCapCenter1;
	pVertex[2+m_numRibbonSlices].pos = vecCapCenter2;

	//    normal
	pVertex[0].normal = normal1;
	pVertex[2+m_numRibbonSlices].normal = normal2;

	//    uv
	pVertex[0].tu= 0.0f;
	pVertex[2+m_numRibbonSlices].tv = 0.0f;

	pVertex[0].DiffuseColor = D3DXCOLOR(1,1,1,0);
	pVertex[2+m_numRibbonSlices].DiffuseColor = D3DXCOLOR(1,1,1,0);

	for ( i = 0 ; i < m_numRibbonSlices ; i++ )
	{
		//    pos
		pVertex[1+i].pos = arrayVertexPos[i].at(0);
		pVertex[(m_numRibbonSlices+2)*2-1-i].pos = arrayVertexPos[i].at(m_numRibbonLen-1);
		
		//    normal
		D3DXVECTOR3 vecTemp = pVertex[1+i].pos - vecCapCenter1;
		D3DXVec3Normalize(&vecTemp, &vecTemp);
		vecTemp = (normal1+vecTemp/2);
		pVertex[1+i].normal = *(D3DXVec3Normalize(&vecTemp, &vecTemp));

		vecTemp = pVertex[(m_numRibbonSlices+2)*2-1-i].pos - vecCapCenter2;
		D3DXVec3Normalize(&vecTemp, &vecTemp);
		vecTemp = (normal2+vecTemp/2);
		pVertex[(m_numRibbonSlices+2)*2-1-i].normal = *(D3DXVec3Normalize(&vecTemp, &vecTemp));

		//    uv
		pVertex[1+i].tu= 0.0f;
		pVertex[3+i+m_numRibbonSlices].tv = 0.0f;

		pVertex[1+i].DiffuseColor = D3DXCOLOR(1,1,1,0);
		pVertex[3+i+m_numRibbonSlices].DiffuseColor = D3DXCOLOR(1,1,1,0);
	}

	pVertex[m_numRibbonSlices+1].pos = pVertex[1].pos;
	pVertex[3+m_numRibbonSlices].pos = pVertex[(m_numRibbonSlices+2)*2-1].pos;

	//    normal
	D3DXVECTOR3 vecTemp = pVertex[1].pos - vecCapCenter1;
	D3DXVec3Normalize(&vecTemp, &vecTemp);
	vecTemp = (normal1+vecTemp/2);
	pVertex[m_numRibbonSlices+1].normal = *(D3DXVec3Normalize(&vecTemp, &vecTemp));

	vecTemp = pVertex[(m_numRibbonSlices+2)*2-1].pos - vecCapCenter2;
	D3DXVec3Normalize(&vecTemp, &vecTemp);
	vecTemp = (normal2+vecTemp/2);
	pVertex[3+m_numRibbonSlices].normal = *(D3DXVec3Normalize(&vecTemp, &vecTemp));

	//    uv
	pVertex[m_numRibbonSlices+1].tu= 0.0f;
	pVertex[3+m_numRibbonSlices].tv = 0.0f;

	pVertex[m_numRibbonSlices+1].DiffuseColor = D3DXCOLOR(1,1,1,0);
	pVertex[3+m_numRibbonSlices].DiffuseColor = D3DXCOLOR(1,1,1,0);

	m_pVBCap->Unlock();


	return hr;
}

HRESULT		CRenderRibbonSelection::MakeIndexBuffer()
{
	//	index buffer
	//    원통의 triangle strip index 갯수
	HRESULT hr;

	DWORD numIndex = (m_numRibbonSlices+1)*2 * (m_numRibbonLen-1);

	if ( numIndex <= 0xffff )
	{
		m_formatIndexBuffer = D3DFMT_INDEX16;
		m_byteSizeIndexBuffer  = sizeof(WORD);
	}
	else
	{
		m_formatIndexBuffer = m_pProteinVistaRenderer->m_formatIndexBuffer;
		m_byteSizeIndexBuffer  = m_pProteinVistaRenderer->m_byteSizeIndexBuffer;
	}

	hr = GetD3DDevice()->CreateIndexBuffer( numIndex*m_byteSizeIndexBuffer , 0, m_formatIndexBuffer, D3DPOOL_MANAGED, &m_pIB, 0 );
	if ( FAILED(hr) )
	{
		OutputTextMsg("Failed to make Index buffer. 32 bit Index buffer does not supported");
		return hr;
	}

	if ( m_byteSizeIndexBuffer == sizeof(DWORD) )
	{
		DWORD * pIndices;
		hr = m_pIB->Lock(0,0, (VOID**) &pIndices, 0);

		//	TriangleStrip.
		DWORD iIB = 0;
		for ( int iLong = 0 ; iLong < m_numRibbonLen-1 ; iLong ++ )
		{
			for ( int iEllipse = 0 ; iEllipse < m_numRibbonSlices+1 ; iEllipse ++ )
			{
				pIndices[iIB++] = (iLong * m_numRibbonSlices) + (iEllipse%m_numRibbonSlices);
				pIndices[iIB++] = ((iLong+1) * m_numRibbonSlices) + (iEllipse%m_numRibbonSlices);
			}
		}

		m_pIB->Unlock();
	}
	else
	{
		WORD * pIndices;
		hr = m_pIB->Lock(0,0, (VOID**) &pIndices, 0);

		//	TriangleStrip.
		DWORD iIB = 0;
		for ( int iLong = 0 ; iLong < m_numRibbonLen-1 ; iLong ++ )
		{
			for ( int iEllipse = 0 ; iEllipse < m_numRibbonSlices+1 ; iEllipse ++ )
			{
				pIndices[iIB++] = (WORD)((iLong * m_numRibbonSlices) + (iEllipse%m_numRibbonSlices));
				pIndices[iIB++] = (WORD)(((iLong+1) * m_numRibbonSlices) + (iEllipse%m_numRibbonSlices));
			}
		}

		m_pIB->Unlock();
	}

	return S_OK;
}


HRESULT CRenderRibbonSelection::DeleteDeviceObjects()
{
	SAFE_RELEASE(m_pVertexDeclRibbon);

	SAFE_RELEASE(m_pVB);
	SAFE_RELEASE(m_pVBCap);
	SAFE_RELEASE(m_pIB);

	return S_OK;
}
#pragma managed(push,off)
HRESULT	CRenderRibbonSelection::Render()
{
	if ( m_numRibbonLen == 0 )
		return E_ABORT;

	if ( m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_bShowCoilOnHelix == FALSE && m_bCoilInHelix == TRUE )
		return S_OK;

	if ( m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_bShowCoilOnSheet == FALSE && m_bCoilInSheet == TRUE )
		return S_OK;

	if ( m_pVB == NULL )
		return S_OK;

	if ( m_pVBCap == NULL )
		return S_OK;

	HRESULT hr = S_OK;
	 
	GetRenderPropertyValue();

	V( GetD3DDevice()->SetVertexDeclaration( m_pVertexDeclRibbon ));

	CPropertyCommon * propertyCommon = m_pRenderRibbonSelectionContainer->m_pPropertyCommon;
	//	m_pProteinVistaRenderer->SetShaderSelectionDiffuseColor(COLORREF2D3DXCOLOR(m_pProteinVistaRenderer->m_pPropertyScene->m_selectionColor));

	//	m_pProteinVistaRenderer->SetShaderIndicate(propertyCommon->m_bIndicate);
	//	m_pProteinVistaRenderer->SetShaderIndicateDiffuseColor(COLORREF2D3DXCOLOR(propertyCommon->m_indicateColor));

	D3DXVECTOR4	EyePos(m_pProteinVistaRenderer->m_FromVec);
	m_pProteinVistaRenderer->SetShaderEyePos(EyePos);

	m_pProteinVistaRenderer->SetShaderIntensityAmbient((propertyCommon->m_intensityAmbient*2)/100.0f);
	m_pProteinVistaRenderer->SetShaderIntensityDiffuse((propertyCommon->m_intensiryDiffuse*2)/100.0f);
	m_pProteinVistaRenderer->SetShaderIntensitySpecular((propertyCommon->m_intensitySpecular*2)/100.0f);


	m_pProteinVistaRenderer->SetShaderWorldMatrix( m_pPDBRenderer->m_matWorld );

	D3DXMATRIXA16 matWorldView = m_pPDBRenderer->m_matWorld * (*m_pProteinVistaRenderer->GetViewMatrix());
	m_pProteinVistaRenderer->SetShaderWorldViewMatrix( matWorldView );
	D3DXMATRIXA16 matWorldViewProj = matWorldView * (*m_pProteinVistaRenderer->GetProjMatrix());
	m_pProteinVistaRenderer->SetShaderWorldViewProjMatrix( matWorldViewProj );

	//	Helix
	GetD3DDevice()->SetIndices(m_pIB);

	UINT cPasses;

	//    GetD3DDevice()->SetRenderState( D3DRS_FILLMODE , D3DFILL_WIREFRAME );

	if ( m_bDisplayRibbon == TRUE )
	{
		if ( m_bDisplayTexture == TRUE )
		{
			m_pProteinVistaRenderer->SetShaderTechnique(CProteinVistaRenderer::RibbonRendering, propertyCommon->m_shaderQuality);
			m_pProteinVistaRenderer->SetShaderSelectionTexture(m_pD3DXTextureRibbon);
		}
		else
		{
			m_pProteinVistaRenderer->SetShaderTechnique(CProteinVistaRenderer::RibbonRenderingNoTexture, propertyCommon->m_shaderQuality);
			m_pProteinVistaRenderer->SetShaderSelectionTexture(NULL);
		}

		GetD3DDevice()->SetStreamSource ( 0, m_pVB, 0, sizeof(CRibbonVertex) );
		m_pProteinVistaRenderer->SetShaderDiffuseColor(COLORREF2D3DXCOLOR(m_colorRibbon));

		V( m_pProteinVistaRenderer->m_pEffectBasicShading->Begin(&cPasses, 0) );
		for (long iPass = 0; iPass < cPasses; iPass++)
		{
			V( m_pProteinVistaRenderer->m_pEffectBasicShading->BeginPass(iPass) );

			//    원통
			hr = GetD3DDevice()->DrawIndexedPrimitive ( D3DPT_TRIANGLESTRIP, 0,0, 
				m_numRibbonSlices*m_numRibbonLen, 0,	
				(m_numRibbonSlices+1)*2 * (m_numRibbonLen-1) - 2 );

			V( m_pProteinVistaRenderer->m_pEffectBasicShading->EndPass() );
		}

		GetD3DDevice()->SetStreamSource ( 0, m_pVBCap, 0, sizeof(CRibbonVertex) );
		for (long iPass = 0; iPass < cPasses; iPass++)
		{
			V( m_pProteinVistaRenderer->m_pEffectBasicShading->BeginPass(iPass) );

			//    앞뚜껑
			hr = GetD3DDevice()->DrawPrimitive ( D3DPT_TRIANGLEFAN , 0, m_numRibbonSlices );
			//    뒷뚜껑.
			hr = GetD3DDevice()->DrawPrimitive ( D3DPT_TRIANGLEFAN , m_numRibbonSlices+2 , m_numRibbonSlices );

			V( m_pProteinVistaRenderer->m_pEffectBasicShading->EndPass() );
		}

		V( m_pProteinVistaRenderer->m_pEffectBasicShading->End() );
	}

	return S_OK;
}
#pragma managed(pop)

D3DXCOLOR MultiplyD3DXCOLOR(D3DXCOLOR &c1, D3DXCOLOR &c2)
{
	return D3DXCOLOR(c1.r * c2.r,	c1.g * c2.g,	c1.b * c2.b,	c1.a * c2.a );
}

HRESULT	CRenderRibbonSelection::UpdateRibbonColor()
{
	HRESULT hr;
	if ( m_pVB == NULL )
		return E_FAIL;

	CRibbonVertex *	pVertex;
	CRibbonVertex *	pVertexCap;

	m_pVB->Lock(0,0, (VOID**)&pVertex, 0 );
	m_pVBCap->Lock(0,0, (VOID**)&pVertexCap, 0 );

	long colorScheme = m_pRenderRibbonSelectionContainer->m_pPropertyCommon->m_enumColorScheme;

	//    첫번째와 마지막은 반씩만 색칠된다.
	long	iVertex = 0;

	D3DXCOLOR	DiffuseColor;
	D3DXCOLOR	DiffuseSSColor = COLORREF2D3DXCOLOR(m_colorRibbon);

	for ( int i = m_iBeginCarbonAtom ; i <= m_iEndCarbonAtom ; i++ )
	{
		//	하나의 band 구간의 vertex의 갯수 = m_numRibbonSlices * m_numSegment
		long numVertexSeg = m_numRibbonSlices * (m_numSegment);

		CResidueInst * pResidueInst = m_pRenderRibbonSelectionContainer->m_arrayResidueInst[i];
		
		if ( colorScheme == 0 )
		{
			//	color scheme 이 0 이라면, (CPK 일때), 색깔을 흰색으로 놓는다.
			DiffuseColor = DiffuseSSColor;
		}
		else
		{
			CAtom * pAtomColor = NULL;
			if ( pResidueInst->GetResidue()->m_bDNA == TRUE )
			{			
				pAtomColor = pResidueInst->GetResidue()->GetC5Atom();
			}
			else
				pAtomColor = pResidueInst->GetResidue()->GetCAAtom();
				
			if ( pAtomColor )
			{
				CColorRow * pColorRow = m_pRenderRibbonSelectionContainer->m_pPropertyCommon->m_pSelectionDisplay->GetAtomColor(pAtomColor);
				DiffuseColor = MultiplyD3DXCOLOR( pColorRow->m_color, DiffuseSSColor);
			}
			else
				DiffuseColor = D3DXCOLOR(128,128,128,0);
		}

		for ( int j = ( i != 0 )? (-numVertexSeg/2): 0 ; j < numVertexSeg/2 ; j++ )
		{
			if ( iVertex >= m_totalVertex )	break;
			pVertex[iVertex].DiffuseColor = DiffuseColor;
			iVertex++;
		}

		if ( i == m_iBeginCarbonAtom )
		{
			for ( int iCap = 0 ; iCap < m_numRibbonSlices+2 ; iCap++ )
			{
				pVertexCap[iCap].DiffuseColor= DiffuseColor;
			}
		}
		if ( i == m_iEndCarbonAtom )
		{
			for ( int iCap = 0 ; iCap < m_numRibbonSlices+2 ; iCap++ )
			{
				pVertexCap[m_numRibbonSlices+2+iCap].DiffuseColor= DiffuseColor;
			}
		}
	}

	//    마지막 m_numRibbonSlices 만큼 더 한다.
	for ( int j = 0 ; j < m_numRibbonSlices ; j++ )
	{
		if ( iVertex >= m_totalVertex )	break;
		pVertex[iVertex].DiffuseColor= DiffuseColor;
		iVertex++;
	}

	m_pVB->Unlock();
	m_pVBCap->Unlock();

	return S_OK;
}

HRESULT	CRenderRibbonSelection::UpdateAtomSelectionChanged()
{
	HRESULT hr;
	if ( m_pVB == NULL )
		return E_FAIL;

	GetRenderPropertyValue();

	CRibbonVertex *	pVertex;
	m_pVB->Lock(0,0, (VOID**)&pVertex, 0 );

	CPropertyCommon * propertyCommon = m_pRenderRibbonSelectionContainer->m_pPropertyCommon;

	long	iVertex = 0;
	float	bSelect;
	for ( int i = m_iBeginCarbonAtom ; i <= m_iEndCarbonAtom ; i++ )
	{
		//	하나의 band 구간의 vertex의 갯수 
		long numVertexSeg = m_numRibbonSlices * (m_numSegment);
		CResidueInst * pResidueInst = m_pRenderRibbonSelectionContainer->m_arrayResidueInst[i];
		
		bSelect = ( propertyCommon->m_indicateColorSlot != -1 )?(propertyCommon->m_indicateColorSlot * 0.1f):((pResidueInst->m_bSelect == TRUE )? 0.1f*(float)(propertyCommon->m_bShowSelectionMark): 0.0f);
		
		for ( int j = ( i != 0 )? (-numVertexSeg/2): 0 ; j < numVertexSeg/2 ; j++ )
		{
			if ( iVertex >= m_totalVertex )
				break;

			pVertex[ iVertex++ ].DiffuseColor.a = bSelect;
		}
	}

	for ( int j = 0 ; j < m_numRibbonSlices ; j++ )
	{
		if ( iVertex >= m_totalVertex )	break;
		pVertex[iVertex++].DiffuseColor.a = bSelect;
		if ( propertyCommon->m_bShowSelectionMark == FALSE )
			pVertex[iVertex++].DiffuseColor.a = 0.0f;
	}

	m_pVB->Unlock();

	//
	//    cap selection mark.
	//
	CRibbonVertex *	pVertexCap;
	m_pVBCap->Lock(0,0, (VOID**)&pVertexCap, 0 );

	CResidueInst * pResidueInst = m_pRenderRibbonSelectionContainer->m_arrayResidueInst[m_iBeginCarbonAtom];
	bSelect = ( propertyCommon->m_indicateColorSlot != -1 )?(propertyCommon->m_indicateColorSlot * 0.1f):((pResidueInst->m_bSelect == TRUE )? 0.1f*(float)(propertyCommon->m_bShowSelectionMark): 0.0f);

	for ( int iCap = 0 ; iCap < m_numRibbonSlices+2 ; iCap++ )
	{
		pVertexCap[iCap].DiffuseColor.a = bSelect;
	}

	pResidueInst = m_pRenderRibbonSelectionContainer->m_arrayResidueInst[m_iEndCarbonAtom];
	bSelect = ( propertyCommon->m_indicateColorSlot != -1 )?(propertyCommon->m_indicateColorSlot * 0.1f):((pResidueInst->m_bSelect == TRUE )? 0.1f*(float)(propertyCommon->m_bShowSelectionMark): 0.0f);

	for ( int iCap = 0 ; iCap < m_numRibbonSlices+2 ; iCap++ )
	{
		pVertexCap[m_numRibbonSlices+2+iCap].DiffuseColor.a = bSelect;
	}

	m_pVBCap->Unlock();

	return S_OK;
}

void CRenderRibbonSelection::ResetTexture()
{
	m_pProteinVistaRenderer->ReleaseTexture(m_strTextureFilename);
	m_pD3DXTextureRibbon = NULL;
}

//========================================================================================================
//========================================================================================================

CRenderCoilSelection::CRenderCoilSelection()
{
	//	minimum is 4
	m_numRibbonSlices = 30;		//	360%m_numRibbonSlices == 0 이어야 한다. 30 보다 크면 안된다.
	//	m_numRibbonSlices = 4;
	//	18, 20, 24, 30
	//	기본값: 18

	m_numSegment =	8;			//	control 포인트간에 보간할때 세그먼트 수. 
	//	최소값은 1(하나도 나누지 않는다.)
	//	기본값. 8

	m_totalVertex = 0;

	m_numRibbonLen = 0;
}

CRenderCoilSelection::~CRenderCoilSelection()
{ 
	DeleteDeviceObjects();
}

void CRenderCoilSelection::GetModelPropertyValue()
{
	m_numSegment = m_pRenderRibbonSelectionContainer->m_numSegment;
	m_numRibbonSlices = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_shapeCoil;

	m_sizeWidth = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_sizeCoil.cx/100.0f;
	m_sizeHeight = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_sizeCoil.cy/100.0f;

	m_fittingMethodRibbon = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_fittingMethodHelix;	//	useless.
	m_curveTensionRibbon = m_pRenderRibbonSelectionContainer->m_curveTension;
	m_textureCoordURibbon = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_textureCoordUCoil;
	m_textureCoordVRibbon = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_textureCoordVCoil;
}

//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
void CRenderCoilSelection::GetRenderPropertyValue() 
{
	m_bDisplayRibbon = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_bDisplayCoil;
	m_bDisplayTexture = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_bTextureCoil;

	if ( m_pD3DXTextureRibbon == NULL )
	{
		m_strTextureFilename = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_strTextureFilenameCoil;
		m_pD3DXTextureRibbon = m_pProteinVistaRenderer->GetTexture(m_strTextureFilename);
	}

	m_colorRibbon = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_colorCoil;
} 

CRenderDNASelection::CRenderDNASelection()
{

}

CRenderDNASelection::~CRenderDNASelection()
{
	DeleteDeviceObjects();
}

void		CRenderDNASelection::GetModelPropertyValue()
{

}

void		CRenderDNASelection::GetRenderPropertyValue()
{

}

//========================================================================================================
//========================================================================================================

CRenderHelixSelection::CRenderHelixSelection()
{
	//	minimum is 4
	m_numRibbonSlices = 30;		//	360%m_numRibbonSlices == 0 이어야 한다. 30 보다 크면 안된다.
	//	18, 20, 24, 30
	//	기본값: 18

	m_numSegment =	8;			//	control 포인트간에 보간할때 세그먼트 수. 실린더 일때도 색깔 때문에 중간을 보간할 필요가 있다.
	//	최소값은 1(하나도 나누지 않는다.)
	//	기본값. 8

	m_totalVertex = 0;
	m_numRibbonLen = 0;
}

CRenderHelixSelection::~CRenderHelixSelection()
{
	DeleteDeviceObjects();
}

void CRenderHelixSelection::GetModelPropertyValue()
{
	m_numSegment = m_pRenderRibbonSelectionContainer->m_numSegment;
	m_numRibbonSlices = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_shapeHelix;

	m_sizeWidth = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_sizeHelix.cx/100.0f;
	m_sizeHeight = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_sizeHelix.cy/100.0f;

	m_fittingMethodRibbon = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_fittingMethodHelix;
	m_curveTensionRibbon = m_pRenderRibbonSelectionContainer->m_curveTension;
	m_textureCoordURibbon = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_textureCoordUHelix;
	m_textureCoordVRibbon = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_textureCoordVHelix;

}


void CRenderHelixSelection::GetRenderPropertyValue() 
{
	m_bDisplayRibbon = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_bDisplayHelix;
	m_bDisplayTexture = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_bTextureHelix;

	if ( m_pD3DXTextureRibbon == NULL )
	{
		m_strTextureFilename = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_strTextureFilenameHelix;
		m_pD3DXTextureRibbon = m_pProteinVistaRenderer->GetTexture(m_strTextureFilename);
	}

	m_colorRibbon = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_colorHelix;
} 

//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
///
///
///
CRenderSheetSelection::CRenderSheetSelection()
{
	//	minimum is 4
	m_numRibbonSlices = 30;		//	360%m_numRibbonSlices == 0 이어야 한다. 30 보다 크면 안된다.
	//	기본값: 18

	m_numSegment =	8;			//	control 포인트간에 보간할때 세그먼트 수. 실린더 일때도 색깔 때문에 중간을 보간할 필요가 있다.
	//	최소값은 1(하나도 나누지 않는다.)
	//	기본값. 8

	m_totalVertex = 0;
	m_numRibbonLen = 0;
}

CRenderSheetSelection::~CRenderSheetSelection()
{
	DeleteDeviceObjects();
}


void CRenderSheetSelection::GetModelPropertyValue()
{
	m_numSegment = m_pRenderRibbonSelectionContainer->m_numSegment;
	m_numRibbonSlices = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_shapeSheet;

	m_sizeWidth = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_sizeSheet.cx/100.0f;
	m_sizeHeight = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_sizeSheet.cy/100.0f;

	m_fittingMethodRibbon = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_fittingMethodHelix;	//	useless.
	m_curveTensionRibbon = m_pRenderRibbonSelectionContainer->m_curveTension;
	m_textureCoordURibbon = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_textureCoordUSheet;
	m_textureCoordVRibbon = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_textureCoordVSheet;

}

void CRenderSheetSelection::GetRenderPropertyValue() 
{
	m_bDisplayRibbon = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_bDisplaySheet;
	m_bDisplayTexture = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_bTextureSheet;

	if ( m_pD3DXTextureRibbon == NULL )
	{
		m_strTextureFilename = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_strTextureFilenameSheet;
		m_pD3DXTextureRibbon = m_pProteinVistaRenderer->GetTexture(m_strTextureFilename);
	}

	m_colorRibbon = m_pRenderRibbonSelectionContainer->m_propertyRibbon->m_colorSheet;
} 

