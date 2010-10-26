#pragma once 

#include "RenderObjectSelection.h"
#include "BatchDrawSphereCylinder.h"

//
//	
class CRenderSpaceFillSelection: public CRenderObjectSelection
{
public:
	CRenderSpaceFillSelection();
	~CRenderSpaceFillSelection(); 

	CBatchDrawSphere	m_batchDrawAtom[MAX_ATOM];

	BOOL				m_bDisplaySideChain;

	CPropertySpaceFill	*	m_pPropertySpacFill;

	void	Init() {}
	virtual void	SetModelQuality();

	HRESULT InitDeviceObjects();
	HRESULT DeleteDeviceObjects();
	HRESULT Render();
};

typedef std::vector < CRenderSpaceFillSelection * > CSTLArrayRenderSpaceFillSelection;

