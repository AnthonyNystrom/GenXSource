#pragma once 

#include "RenderObjectSelection.h"

class CWireframeVertex 
{
public:
	CWireframeVertex () { m_pos = D3DXVECTOR3(0,0,0); m_color = 0; m_posEnd = D3DXVECTOR3(0,0,0); m_index = 0; m_bSelect = 0; }
	D3DXVECTOR3		m_pos; 
	DWORD			m_color;  
	D3DXVECTOR3		m_posEnd; 
	FLOAT			m_index;  
	FLOAT			m_bSelect;  

	enum { FVF = ( D3DFVF_XYZ | D3DFVF_DIFFUSE | D3DFVF_TEX2 | D3DFVF_TEXCOORDSIZE3(0) | D3DFVF_TEXCOORDSIZE2(1) ) };
};

typedef std::vector <CWireframeVertex> CArrayWirefeameVertex;

//
//	
class CRenderWireframeSelection: public CRenderObjectSelection
{
public:
	CRenderWireframeSelection();
	~CRenderWireframeSelection(); 

	CPropertyWireframe	* m_pPropertyWireframe;

	enum { PROTEIN = 0, ONLYCA };
	long			m_typeWireframe;

	long			m_maxNumLine;

	LPDIRECT3DVERTEXDECLARATION9	m_pDeclWireframe;
	IDirect3DVertexBuffer9 *	m_pMeshWireframeVertexBuffer;

	FLOAT			m_weightLine;

	//	vertex 를 저장하는곳.
	CArrayWirefeameVertex		m_arrayWireframeVertex;
	CArrayWirefeameVertex		m_arrayWireframeVertexWeight;

	long			m_numWireframeVertex;

	HRESULT InitDeviceObjects();
	HRESULT DeleteDeviceObjects();
	HRESULT Render();

	HRESULT UpdateAtomSelectionChanged();
};

typedef std::vector < CRenderWireframeSelection * > CSTLArrayRenderWireframeSelection;

