//-----------------------------------------------------------------------------
// File: CoordinateAxis.h
//
// Desc: hgshin
//-----------------------------------------------------------------------------

#pragma once

#include "GraphicsObject.h"

class CProteinVistaRenderer;

//-----------------------------------------------------------------------------
// Name: class CCoordinateAxisDisplay
//
// Desc: 
//
//-----------------------------------------------------------------------------

struct CVertexAxis
{
	D3DXVECTOR3	pos;
	D3DXVECTOR3	normal;
	enum { FVF = D3DFVF_XYZ|D3DFVF_NORMAL };
};

class CCoordinateAxisDisplay : public CMoleculeRenderObject
{
public:
	
	ID3DXMesh*						m_pMesh;
	D3DMATERIAL9*					m_pMeshMaterials;

	DWORD	numFaces;
	DWORD	numVertices;

	D3DXVECTOR3						m_center;
	D3DXMATRIXA16					m_matWorldPDB;		//	Local coordinate.

	D3DXMATRIXA16					m_matAxisX;
	D3DXMATRIXA16					m_matAxisY;
	D3DXMATRIXA16					m_matAxisZ;

	D3DXMATRIXA16					m_matScale;

	LPDIRECT3DVERTEXDECLARATION9	m_pDeclAxis;

	void			Init(CProteinVistaRenderer * pProteinVistaRenderer);
	HRESULT			SetModelTransform(D3DXVECTOR3& center, D3DXMATRIXA16 & matWorld);
	HRESULT			SetModelScale(D3DXMATRIXA16 & matScale);

    virtual HRESULT InitDeviceObjects();
	virtual HRESULT DeleteDeviceObjects();
	virtual HRESULT Render();

	CCoordinateAxisDisplay();
	~CCoordinateAxisDisplay() {}
};

