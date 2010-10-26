#pragma once

#include "RenderObjectSelection.h"

//	this value is same as NumBatchInstance in fx file.
#define NumBatchInstance 75

struct BATCH_SPHERE_VERTEX_INSTANCE
{
	D3DXVECTOR3 pos;    // Position of the vertex
	D3DXVECTOR3 norm;   // Normal at this vertex
	float boxInstance;  // Box instance index
};

//
//	ShaderInstancing을 가지고 batchDraw를 한다.
//	
//	sphere는 numBatch의 수만큼 만든다.
//	instance는 numDraw 수만큼 만든다.
//	
//	selection 마다, resolution이 다를수 있어, CBatchDrawSphere 에서 sphere를 만든다.
//	
class CBatchDrawSphere:public CMoleculeRenderObject
{

public:
	IDirect3DVertexDeclaration9*    m_pVertexDeclShader;

	IDirect3DVertexBuffer9 *		m_pMeshBatchVertexBuffer;
	IDirect3DIndexBuffer9 *			m_pMeshBatchIndexBuffer;

	CProteinVistaRenderer *		m_pProteinVistaRenderer;
	LONG						m_numMaxInstance;
	LONG						m_numBatchInstance;
	LONG						m_resolution;

	D3DXVECTOR4 *				m_arrayInstancePosition;
	D3DXVECTOR4 *				m_arrayInstanceSelection;
	D3DXCOLOR *					m_arrayInstanceColor;

	FLOAT						m_fRadius;

	LONG						m_numVertexSphere;
	LONG						m_numFaceSphere;

	//	
	CBatchDrawSphere();
	~CBatchDrawSphere();

	//	
	//
	void	Init ( CProteinVistaRenderer * pProteinVistaRenderer, FLOAT radius,  LONG resolution);
	void	SetInstanceData(CSTLArrayVector4 & arrayInstancePosition, CSTLFLOATArray & arraySelection, CSTLArrayColor & arrayInstanceColor );

	virtual HRESULT InitDeviceObjects();
	virtual HRESULT Render();
	virtual HRESULT DeleteDeviceObjects();
};

typedef std::vector < CBatchDrawSphere * > CArrayBatchDrawSphere ;

//	
//
typedef struct BATCH_SPHERE_VERTEX_INSTANCE	BATCH_CYLINDER_VERTEX_INSTANCE;
//
//	
class CBatchDrawCylinder:public CMoleculeRenderObject
{

public:
	IDirect3DVertexDeclaration9*    m_pVertexDeclShader;

	IDirect3DVertexBuffer9 *		m_pMeshBatchVertexBuffer;
	IDirect3DIndexBuffer9 *			m_pMeshBatchIndexBuffer;

	CProteinVistaRenderer *		m_pProteinVistaRenderer;
	LONG						m_numMaxInstance;
	LONG						m_numBatchInstance;
	LONG						m_resolution;

	D3DXVECTOR4 *				m_arrayInstancePosition;
	D3DXCOLOR *					m_arrayInstanceColor;
	D3DXVECTOR4 *				m_arrayInstanceSelectionRotationXYScale;

	FLOAT						m_fRadius;

	LONG						m_numVertexCylinder;
	LONG						m_numFaceCylinder;

	//	
	CBatchDrawCylinder();
	~CBatchDrawCylinder();

	//	
	//
	void	Init ( CProteinVistaRenderer * pProteinVistaRenderer, FLOAT radius,  LONG resolution);
	void	SetInstanceData( CSTLArrayVector4 & arrayInstancePosition, CSTLFLOATArray & arraySelection , CSTLArrayVector2 & arrayInstanceRotation , CSTLFLOATArray & arrayInstanceScale , CSTLArrayColor & arrayInstanceColor );

	virtual HRESULT InitDeviceObjects();
	virtual HRESULT Render();
	virtual HRESULT DeleteDeviceObjects();
};

typedef std::vector < CBatchDrawCylinder * > CArrayBatchDrawCylinder ;

