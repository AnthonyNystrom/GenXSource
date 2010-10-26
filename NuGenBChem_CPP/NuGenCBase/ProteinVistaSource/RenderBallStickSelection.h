#pragma once 

#include "RenderObjectSelection.h"
#include "BatchDrawSphereCylinder.h"

//
//	
class CRenderBallStickSelection: public CRenderObjectSelection
{
public:
	CRenderBallStickSelection();
	~CRenderBallStickSelection(); 

	CPropertyStick	*		m_pPropertyStick;
	CPropertyBallStick	*	m_pPropertyBallStick;

	//	
	CBatchDrawSphere	m_batchDrawAtom[MAX_ATOM];
	CBatchDrawCylinder	m_batchDrawBond[MAX_ATOM];

	virtual void	SetModelQuality();

	enum { STICK, BALLSTICK };
	int		m_typeSelection;	//	STICK or BALLSTICK

	long	m_sphereResolution;
	long	m_cylinderResolution;

	DOUBLE	m_sphereRadius;
	DOUBLE	m_cylinderSize;

	BOOL				m_bDisplayBallStck;

	//	Selection 에서 조정
	BOOL				m_bDisplaySideChain;
	BOOL				m_bDisplayHETATM;

	//	
	HRESULT InitDeviceObjects();
	HRESULT DeleteDeviceObjects();
	HRESULT Render();

	//	
	HRESULT	MakeSphereInstanceData(CSTLArrayVector4 arraySpherePosition[], CSTLFLOATArray arraySphereSelection[], CSTLArrayColor arraySphereColor[] );
	HRESULT	MakeBondInstanceData(CSTLArrayVector4 arrayBondPosition[], CSTLFLOATArray arrayBondSelection[], CSTLArrayVector2 arrayBondRotation[] , CSTLFLOATArray arrayBondScale [] , CSTLArrayColor arrayBondColor[] );
};

typedef std::vector < CRenderBallStickSelection * > CSTLArrayRenderBallStickSelection;

