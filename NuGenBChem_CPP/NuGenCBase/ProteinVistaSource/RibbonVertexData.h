#pragma once

//	
HRESULT GetCardianlCurvePoint( _IN_ CSTLArrayD3DXVECTOR3 & arrayCtrlPoint, _OUT_ CSTLArrayD3DXVECTOR3 & m_arrayVertex , _IN_ long seg = 16 , _IN_ float tension = 0.1 );
HRESULT GetInterLinePoint( _IN_ CSTLArrayD3DXVECTOR3 & arrayCtrlPoint, _OUT_ CSTLArrayD3DXVECTOR3 & m_arrayVertex , _IN_ long seg );

class CProteinRibbonVertexData
{
public:
	BOOL				m_bUsed;

	CChain *			m_pChain;

	long				m_numSegment;
	float				m_curveTension;

	CProteinRibbonVertexData();
	~CProteinRibbonVertexData();

	void Init(CChain * pChain, long numSegment, float curveTension) { m_pChain = pChain; m_numSegment = numSegment; m_curveTension = curveTension; }

	CSTLArrayD3DXVECTOR3		m_arrayCarbonAtom;
	CSTLArrayD3DXVECTOR3		m_arrayUpVec;

	CSTLArrayResidue			m_arrayResidue;		//	Ca 에 매칭되는 CResidue

	CSTLArrayD3DXVECTOR3		m_arrayRibbonCurve;
	CSTLArrayD3DXVECTOR3		m_arrayRibbonCurveUpVec;
	CSTLArrayD3DXVECTOR3		m_arrayRibbonCurveDirVec;

	//    
	CSTLArrayD3DXVECTOR3		m_arrayHelixOptimalCylinder;
	CSTLArrayD3DXVECTOR3		m_arrayHelixOptimalCylinderDirVec;
	CSTLArrayD3DXVECTOR3		m_arrayHelixOptimalCylinderUpVec;

	CSTLArrayD3DXVECTOR3		m_arrayHelixTwoPointCylinder;
	CSTLArrayD3DXVECTOR3		m_arrayHelixTwoPointCylinderDirVec;
	CSTLArrayD3DXVECTOR3		m_arrayHelixTwoPointCylinderUpVec;

	//	shared inst 에 사용되는 residueInst를 얻어온다.
	HRESULT GetRibbonResidueInst ( CChainInst * pChainInst, CSTLArrayResidueInst & arrayResidueInst );

	HRESULT CreateRibbonVertexData();

	HRESULT CreateCoilSkeletonVertex();
	HRESULT CreateHelixSkeletonVertex();
};

