#include "stdafx.h"
#include "ProteinVista.h"

#include "ProteinVistaRenderer.h"
#include "pdb.h"
#include "pdbInst.h"
#include "RibbonVertexData.h"
#include "localSuperImpose.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


CProteinRibbonVertexData::CProteinRibbonVertexData()
: m_bUsed(FALSE) 
{

}

CProteinRibbonVertexData::~CProteinRibbonVertexData()
{

}


HRESULT CProteinRibbonVertexData::CreateRibbonVertexData()
{
	CreateCoilSkeletonVertex();
	CreateHelixSkeletonVertex();

	return S_OK;
}

//
//	
HRESULT CProteinRibbonVertexData::GetRibbonResidueInst ( CChainInst * pChainInst, CSTLArrayResidueInst & arrayResidueInst )
{
	arrayResidueInst.clear();
	arrayResidueInst.reserve(pChainInst->m_arrayResidueInst.size());

	if ( pChainInst->GetChain()->m_bDNA == TRUE )
	{
		//    chain 전체에 대해서 구해둔다. 부분만 selection을 하였을때 오류를 방지.
		for ( int i = 0 ; i < pChainInst->m_arrayResidueInst.size() ; i++ )
		{
			CResidueInst * pResidueInst = pChainInst->m_arrayResidueInst[i];

			if ( pResidueInst->GetResidue()->m_bHETATM == TRUE )
			{
				continue;
			}

			arrayResidueInst.push_back(pResidueInst);
		}
	}
	else
	{
		//    chain 전체에 대해서 구해둔다. 부분만 selection을 하였을때 오류를 방지.
		for ( int i = 0 ; i < pChainInst->m_arrayResidueInst.size() ; i++ )
		{
			CResidueInst * pResidueInst = pChainInst->m_arrayResidueInst[i];

			if ( pResidueInst->GetResidue()->m_bHETATM == TRUE )
			{
				continue;
			}

			if ( pResidueInst->GetResidue()->m_bExistMainChain == FALSE )
			{
				//    비는것이 있을 경우에 좌우값으로 보정을 한다.
				if ( i == 0 )
				{
					pResidueInst = pChainInst->m_arrayResidueInst[i+1];
				}
				else 
				{	//	 끝에 빈것
					pResidueInst = pChainInst->m_arrayResidueInst[i-1];
				}
			}

			arrayResidueInst.push_back(pResidueInst);
		}
	}

	return S_OK;
}

//	called Init()
//	direction, up, right vector를 구한다.
HRESULT CProteinRibbonVertexData::CreateCoilSkeletonVertex()
{
	D3DXVECTOR3	vecUpOld(1,0,0);

	if ( m_pChain->m_bDNA == TRUE )
	{
		//    chain 전체에 대해서 구해둔다. 부분만 selection을 하였을때 오류를 방지.
		for ( int i = 0 ; i < m_pChain->m_arrayResidue.size() ; i++ )
		{
			CResidue * pResidue = m_pChain->m_arrayResidue[i];

			if ( pResidue->m_bHETATM == TRUE )
			{
				continue;
			}

			//	C1'
			//	C5'
			CAtom * pAtomC1 = NULL;
			CAtom * pAtomC5 = NULL;
			for ( int j = 0 ; j < pResidue->m_arrayAtom.size() ; j++ )
			{
				CString strAtomName = pResidue->m_arrayAtom[j]->m_atomName;
				if ( strAtomName == _T(" C1'") || strAtomName == _T(" C1*") )
					pAtomC1 = pResidue->m_arrayAtom[j];
				if ( strAtomName == _T(" C5'") || strAtomName == _T(" C5*") )
					pAtomC5 = pResidue->m_arrayAtom[j];

				if ( pAtomC1 && pAtomC5 )
					break;
			}

			if ( pAtomC1 == NULL )
			{
				pAtomC1 = pResidue->m_arrayAtom[0];
			}

			if ( pAtomC5 == NULL )
			{
				pAtomC5 = pResidue->m_arrayAtom[0];
			}

			m_arrayCarbonAtom.push_back(pAtomC5->m_pos);
			m_arrayResidue.push_back(pResidue);

			D3DXVECTOR3	vecUp(0,0,0);
			//	리본의 upvector 구하기.
			vecUp = pAtomC1->m_pos - pAtomC5->m_pos;
			D3DXVec3Normalize( &vecUp, &vecUp );

			m_arrayUpVec.push_back(vecUp);
		}
	}
	else
	{
		//	
		//	chain 이 전체다 m_bExistMainChain = FALSE 인 경우가 있다.
		//	1ffk,  1xi4
		//
		long	countFalse = 0;
		for ( int i = 0 ; i < m_pChain->m_arrayResidue.size() ; i++ )
		{
			CResidue * pResidue = m_pChain->m_arrayResidue[i];
			if ( pResidue->m_bExistMainChain == FALSE )		countFalse ++;
		}

		//	false 인것이 5개 이라하면, 
		if ( countFalse <= max(5 , m_pChain->m_arrayResidue.size()/10 ) )
		{
			//    chain 전체에 대해서 구해둔다. 부분만 selection을 하였을때 오류를 방지.
			for ( int i = 0 ; i < m_pChain->m_arrayResidue.size() ; i++ )
			{
				CResidue * pResidue = m_pChain->m_arrayResidue[i];

				if ( pResidue->m_bHETATM == TRUE )
				{
					continue;
				}

				if ( pResidue->m_bExistMainChain == FALSE )
				{
					//    비는것이 있을 경우에 좌우값으로 보정을 한다.
					if ( i == 0 )
					{
						pResidue = m_pChain->m_arrayResidue[i+1];
					}
					else 
					{	//	 끝에 빈것
						pResidue = m_pChain->m_arrayResidue[i-1];
					}
				}

				D3DXVECTOR3 vec1, vec2, vec3, vec4(0,0,0);
				vec2 = ( pResidue->GetCAAtom() )? pResidue->GetCAAtom()->m_pos : D3DXVECTOR3(0,0,0) ;
				vec1 = ( pResidue->GetNAtom() )? pResidue->GetNAtom()->m_pos : vec2 ;
				//    vec3 = pResidue->GetOAtom()->m_pos;
				vec4 = ( pResidue->GetCBAtom() ) ? pResidue->GetCBAtom()->m_pos : vec2 ;

				m_arrayCarbonAtom.push_back(pResidue->GetCAAtom()->m_pos);
				m_arrayResidue.push_back(pResidue);

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

	return S_OK;
}

//    helix cylinder 에 대한 skeleton vertex이다.
HRESULT CProteinRibbonVertexData::CreateHelixSkeletonVertex()
{
	if ( m_arrayCarbonAtom.size() <= 0 )
		return S_OK;

	//	create coil, helix, sheet selection class
	//	
	long beginIndex = 0;
	long oldTypeSS = m_arrayResidue[0]->GetSS();

	for ( int iAtom = 0 ; iAtom <= m_arrayCarbonAtom.size() ; iAtom++ )
	{
		long typeSS;
		if ( iAtom >= m_arrayCarbonAtom.size() )
			typeSS = -1;	//	마지막은 -1 로 항상 추가된다.
		else
			typeSS = m_arrayResidue[iAtom]->GetSS();

		if ( typeSS != oldTypeSS )
		{
			if ( oldTypeSS == SS_HELIX )
			{
				long beginCarbonAtom = beginIndex;
				long endCarbonAtom = iAtom-1;

				//    선택된 부분에 helix/sheet 가 있는지 조사.
				//	Preprocessing.
				//	if ( !((m_endResidue+2 < beginCarbonAtom) || (m_beginResidue-2 > endCarbonAtom) ) )
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

					if ( m_arrayHelixOptimalCylinder.size() == 0 )
					{
						m_arrayHelixOptimalCylinder.resize(m_arrayRibbonCurve.size());
						std::fill(m_arrayHelixOptimalCylinder.begin(), m_arrayHelixOptimalCylinder.end(), D3DXVECTOR3(0,0,0));

						m_arrayHelixOptimalCylinderDirVec.resize(m_arrayRibbonCurveDirVec.size());
						std::fill(m_arrayHelixOptimalCylinderDirVec.begin(), m_arrayHelixOptimalCylinderDirVec.end(), D3DXVECTOR3(0,0,0));

						m_arrayHelixOptimalCylinderUpVec.resize(m_arrayRibbonCurveUpVec.size());
						std::fill(m_arrayHelixOptimalCylinderUpVec.begin(), m_arrayHelixOptimalCylinderUpVec.end(), D3DXVECTOR3(0,0,0));
					}

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

					if ( m_arrayHelixTwoPointCylinder.size() == 0 )
					{
						m_arrayHelixTwoPointCylinder.resize(m_arrayRibbonCurve.size());
						std::fill(m_arrayHelixTwoPointCylinder.begin(), m_arrayHelixTwoPointCylinder.end(), D3DXVECTOR3(0,0,0));

						m_arrayHelixTwoPointCylinderDirVec.resize(m_arrayRibbonCurveDirVec.size());
						std::fill(m_arrayHelixTwoPointCylinderDirVec.begin(), m_arrayHelixTwoPointCylinderDirVec.end(), D3DXVECTOR3(0,0,0));

						m_arrayHelixTwoPointCylinderUpVec.resize(m_arrayRibbonCurveUpVec.size());
						std::fill(m_arrayHelixTwoPointCylinderUpVec.begin(), m_arrayHelixTwoPointCylinderUpVec.end(), D3DXVECTOR3(0,0,0));
					}

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

	return S_OK;
}


//////////////////////////////////////////////////////////////////////////
//	cardinal curve utility function
//////////////////////////////////////////////////////////////////////////

HRESULT GetCardianlCurvePoint( _IN_ CSTLArrayD3DXVECTOR3 & arrayCtrlPoint, _OUT_ CSTLArrayD3DXVECTOR3 & m_arrayVertex , _IN_ long seg , _IN_ float tension )
{
	//	float	tension = tension;
	long	numSegs = seg;		//	ctrl 포인트 간에 세그먼트의 수.

	if ( numSegs == 0 )
	{	//	마지막꺼만 복사.
		return S_OK;
	}

	//
	//	
	CSTLArrayD3DXVECTOR3 arrayCtrlPointTangent;
	arrayCtrlPointTangent.resize(arrayCtrlPoint.size());

	//	calc tangent vector
	{
		long	numCtrlPts = arrayCtrlPoint.size();

		// CALCULATE TENSION FACTOR
		float T = (float)tension;  

		// FIRST HANDLE ENDPTS (MUST HANDLE "CLOSED" AND "OPEN" CASES)
		int last = numCtrlPts-1;                             // INDEX OF LAST POINT

		arrayCtrlPointTangent[0] = (arrayCtrlPoint[1]-arrayCtrlPoint[0]) * T;   // FIRST ENDPT
		arrayCtrlPointTangent[last] = (arrayCtrlPoint[last]-arrayCtrlPoint[last-1]) * T; // SECOND ENDPT

		// HANDLE THE REST OF THE POINTS (SAME REGARDLESS WHETHER OPEN OR CLOSED)
		for (long i=1; i < numCtrlPts-1; i++)
			arrayCtrlPointTangent[i] = (arrayCtrlPoint[i+1]-arrayCtrlPoint[i-1]) * T;
	}

	{	//	calc Hermite Function
		double du = 1 / (double)numSegs;

		// ALLOCATE THE BLENDING FUNCTION LOOKUP TABLES
		numSegs--;		// DECREMENT BY 1 - NOT INCLUDING 1ST AND LAST PT

		double *H0, *H1, *H2, *H3;
		H0 = new double[numSegs];
		H1 = new double[numSegs];
		H2 = new double[numSegs];
		H3 = new double[numSegs];

		// INITIALIZE THE LUT's WITH THE HERMITE BLENDING FUNCTIONS EVALUATED AT U
		double u=0, u2, u3;
		for ( long i=0; i < numSegs; i++)
		{
			u+=du; 
			u2=u*u; 
			u3=u2*u;

			H0[i] = 2*u3 - 3*u2 + 1;
			H1[i] = -2*u3 + 3*u2;
			H2[i] = u3 - 2*u2 + u;
			H3[i] =	u3 - u2;
		}

		//	get hermite curve position
		D3DXVECTOR3 currBasePt;
		D3DXVECTOR3 nextBasePt;

		for ( i = 0 ; i < arrayCtrlPoint.size()-1 ; i++ )
		{
			currBasePt = arrayCtrlPoint[i];
			nextBasePt = arrayCtrlPoint[i+1];
			D3DXVECTOR3 currBaseTangent = arrayCtrlPointTangent[i];
			D3DXVECTOR3 nextBaseTangent = arrayCtrlPointTangent[i+1];

			m_arrayVertex.push_back(currBasePt);

			for ( int j = 0 ; j < numSegs ; j++ )
			{
				D3DXVECTOR3 nextPt = currBasePt*H0[j] + nextBasePt*H1[j] + currBaseTangent*H2[j] + nextBaseTangent*H3[j];
				m_arrayVertex.push_back(nextPt);
			}
		}

		m_arrayVertex.push_back(nextBasePt);

		delete [] H0;
		delete [] H1;
		delete [] H2;
		delete [] H3;
	}

	return S_OK;
}


HRESULT GetInterLinePoint( _IN_ CSTLArrayD3DXVECTOR3 & arrayCtrlPoint, _OUT_ CSTLArrayD3DXVECTOR3 & m_arrayVertex , _IN_ long seg )
{
	m_arrayVertex.reserve(arrayCtrlPoint.size() * seg + 1);

	for ( int i = 0 ; i < arrayCtrlPoint.size()-1 ; i++ )
	{
		D3DXVECTOR3 vecSeg = arrayCtrlPoint[i+1] - arrayCtrlPoint[i];
		for ( int j = 0 ; j < seg ; j++ )
		{
			m_arrayVertex.push_back( arrayCtrlPoint[i] + vecSeg * ((FLOAT)j/(FLOAT)seg) );
		}
	}
	m_arrayVertex.push_back( arrayCtrlPoint[arrayCtrlPoint.size()-1] );
	return S_OK;
}

