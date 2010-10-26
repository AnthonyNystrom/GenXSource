#pragma once

#include "GraphicsObject.h"

class CSelectionDisplay;
class CPropertyCommon;
class CPDBRenderer;

typedef struct vertexClipPlane{
	D3DXVECTOR3 pos;
	D3DXVECTOR3 normal;
	enum { FVF = D3DFVF_XYZ|D3DFVF_NORMAL };
} CVertexClipPlane;


class CClipPlane: public CMoleculeRenderObject
{
public:
	CClipPlane();
	~CClipPlane(){ DeleteDeviceObjects(); }

	void	Init (CProteinVistaRenderer * pProteinVistaRenderer, CSelectionDisplay * pSelectionDisplay, float radius) ;

	CSelectionDisplay * m_pSelectionDisplay;
	CPDBRenderer *	m_pPDBRenderer;

	float		m_radius;
	long		m_numFace;

	LPDIRECT3DVERTEXDECLARATION9	m_pDeclClipPlane;
	IDirect3DVertexBuffer9 *		m_pVB;

	CSTLVectorValueArray m_vecPos;

	BOOL		m_bShowClipPlane;
	long		m_transparency;
	D3DXCOLOR	m_colorPlane;
	void		InitRenderParam(BOOL bShowClipPlane, long transparency, D3DXCOLOR &color);

	void		InitRadius(float radius);

	virtual HRESULT InitDeviceObjects();
	virtual HRESULT FrameMove();
	virtual HRESULT Render();
	virtual HRESULT DeleteDeviceObjects();

	//CXTPPropertyGridItem * m_pItemPlaneText;

	D3DXVECTOR3 m_vecBasePos[3];
	D3DXPLANE * GetPlaneEquation(D3DXPLANE * pPlane);
	void		SetPlaneEquation(D3DXPLANE & pPlane);

	void UpdatePropertyPanePlaneEquation();

	BOOL Pick(FLOAT &dist);

	//	mouse message,
	BOOL			m_bDrag;
	long			m_posOldX, m_posOldY;

	D3DXMATRIXA16	m_matWorld;
	D3DXMATRIXA16	m_matWorldOld;

	D3DXMATRIXA16	m_matWorldUserInput;
	D3DXMATRIXA16	m_matWorldUserInputOrig;

	D3DXMATRIXA16	m_matWorldMouseMoveRot;
	D3DXMATRIXA16	m_matWorldMouseMoveTrans;

public:
	LRESULT HandleMessages( HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam );

public:
	HRESULT Save(CFile &file);
	HRESULT Load(CFile &file);
};

typedef std::vector< CClipPlane * > CSTLArrayClipPlane;

