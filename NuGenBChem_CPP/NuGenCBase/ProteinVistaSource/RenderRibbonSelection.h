#pragma once

#include "RenderObjectSelection.h"

struct CRibbonVertex
{
	D3DXVECTOR3	pos;
	//	float		w;			//	selection on/off
	D3DXVECTOR3	normal;
	FLOAT		tu, tv;
	D3DXCOLOR	DiffuseColor;
};

class CRenderCoilSelection;
class CRenderHelixSelection;
class CRenderSheetSelection;
class CRenderDNASelection;

typedef std::vector < CRenderCoilSelection * > CSTLArrayRenderCoilSelection;
typedef std::vector < CRenderHelixSelection * > CSTLArrayRenderHelixSelection;
typedef std::vector < CRenderSheetSelection * > CSTLArrayRenderSheetSelection;
typedef std::vector < CRenderDNASelection * > CSTLArrayRenderDNASelection;

//	하나의 selection 안에서는 그려질 atom이 continuous 하다.
//	helix는 하나의 selection안에서 여러개 생길수 있다.
class CRenderRibbonSelectionContainer: public CRenderObjectSelection
{
public:
	CRenderRibbonSelectionContainer();
	virtual ~CRenderRibbonSelectionContainer();

	virtual void Init(CChainInst * pChainInst, long beginResidue, long endResidue );
	virtual void	SetModelQuality();

	HRESULT		InitDeviceObjects();
	HRESULT		DeleteDeviceObjects();
	HRESULT		Render();
	HRESULT		UpdateAtomSelectionChanged();
	virtual void ResetTexture();

	CPropertyRibbon	*				m_propertyRibbon;

	CProteinRibbonVertexData *		m_pRibbonVertexData;

	long		m_numSegment;
	float		m_curveTension;

	//	void		MakeRibbonSkeletonVertex();
	//	void		MakeHelixCylinderSkeletonVertex();

	void		Picking(D3DXVECTOR3 &pickRayDir, D3DXVECTOR3 &pickRayOrig, CSTLArrayPickedResidueInst & pickResidueArray );

public:
	//	렌더링되는 범위.
	//	parent 에 있는 멤버
	//CChainInst *					m_pChainInst;

	long						m_beginResidue;
	long						m_endResidue;

	CSTLArrayResidueInst		m_arrayResidueInst;		//	Ca 에 매칭되는 CResidue

// 
// 	CSTLArrayD3DXVECTOR3		m_arrayCarbonAtom;
// 	CSTLArrayD3DXVECTOR3		m_arrayUpVec;

	//
	//	m_arrayCarbonAtom을 가지고 Ribbon의 중심을 구하여 둔다. Picking 에 사용
	//	picking 모듈 수정(pick.cpp)
	//	rendering수정해서 이것 렌더링 시에 사용.
// 	CSTLArrayD3DXVECTOR3		m_arrayRibbonCurve;
// 	CSTLArrayD3DXVECTOR3		m_arrayRibbonCurveUpVec;
// 	CSTLArrayD3DXVECTOR3		m_arrayRibbonCurveDirVec;
// 
// 	//    
// 	CSTLArrayD3DXVECTOR3		m_arrayHelixOptimalCylinder;
// 	CSTLArrayD3DXVECTOR3		m_arrayHelixOptimalCylinderDirVec;
// 	CSTLArrayD3DXVECTOR3		m_arrayHelixOptimalCylinderUpVec;
// 
// 	CSTLArrayD3DXVECTOR3		m_arrayHelixTwoPointCylinder;
// 	CSTLArrayD3DXVECTOR3		m_arrayHelixTwoPointCylinderDirVec;
// 	CSTLArrayD3DXVECTOR3		m_arrayHelixTwoPointCylinderUpVec;

private:
	CSTLArrayRenderCoilSelection	m_arrayRenderCoilSelection;		
	CSTLArrayRenderHelixSelection	m_arrayRenderHelixSelection;
	CSTLArrayRenderSheetSelection	m_arrayRenderSheetSelection;
	CSTLArrayRenderDNASelection		m_arrayRenderDNASelection;		//	Draw DNA Sugar...

};

typedef std::vector < CRenderRibbonSelectionContainer * > CSTLArrayRenderRibbonSelectionContainer;

//
//
//    coil, helix, sheet를 렌더링 하는 클래스의 parent class
class CRenderRibbonSelection: public CMoleculeRenderObject
{
public:
	CRenderRibbonSelection();
	virtual ~CRenderRibbonSelection();

	CRenderRibbonSelectionContainer * m_pRenderRibbonSelectionContainer;

	//	편의를 위해 복사해놓는 함수
	CPDBRenderer *		m_pPDBRenderer;

	//	device는 중간에 바뀔수 있다. Multi-sample등으로 device 가 바뀔수 있다.
	//	LPDIRECT3DDEVICE9	m_pD3DDevice;

	virtual void Init( CRenderRibbonSelectionContainer * pRenderRibbonSelectionContainer );

	virtual HRESULT		InitDeviceObjects();

	virtual HRESULT		MakeVertexBuffer();
	virtual	HRESULT		MakeIndexBuffer();
	virtual HRESULT		DeleteDeviceObjects();

	virtual HRESULT		Render();

	virtual HRESULT		UpdateAtomSelectionChanged();
	virtual HRESULT		UpdateRibbonColor();

	virtual int			GetSSType() { return -1; }

	long				m_numRibbonSlices;
	long				m_numSegment;
	float				m_sizeWidth;
	float				m_sizeHeight;
	long				m_fittingMethodRibbon;
	long				m_curveTensionRibbon;
	long				m_textureCoordURibbon;
	long				m_textureCoordVRibbon;
	virtual void		GetModelPropertyValue() {}
	virtual void		ResetTexture();

	BOOL				m_bDisplayRibbon;
	BOOL				m_bDisplayTexture;

	CString				m_strTextureFilename;
	LPDIRECT3DTEXTURE9	m_pD3DXTextureRibbon;

	COLORREF			m_colorRibbon;
	virtual	void		GetRenderPropertyValue() {} 

	BOOL				m_bCoilInHelix;
	BOOL				m_bCoilInSheet;

	//
	long				m_iBeginCarbonAtom;
	long				m_iEndCarbonAtom;

	long				m_numRibbonLen;		//	리본 selection 안의 길이(atom 개수 * m_numSegment)
	long				m_totalVertex;		//	리본 selection 안의 vertex 갯수(atom 개수 * m_numSegment * slices)

	//    for picking. called in Pick.cpp
	void	Picking(D3DXVECTOR3 &pickRayDir, D3DXVECTOR3 &pickRayOrig, CSTLArrayPickedResidueInst & pickResidueArray );

	D3DFORMAT		m_formatIndexBuffer;
	UINT			m_byteSizeIndexBuffer;

public:
	LPDIRECT3DVERTEXDECLARATION9	m_pVertexDeclRibbon;

	IDirect3DVertexBuffer9	* m_pVB;		//	SS 의 vertex buffer.
	IDirect3DIndexBuffer9	* m_pIB;

	IDirect3DVertexBuffer9	* m_pVBCap;		//	cap을 위한 vertex buffer
};

//	
class CRenderCoilSelection: public CRenderRibbonSelection
{
public:
	CRenderCoilSelection();
	virtual ~CRenderCoilSelection();

	virtual void		GetModelPropertyValue();
	virtual	void		GetRenderPropertyValue();
	
	virtual int			GetSSType() { return SS_NONE; }
public:
};

class CRenderDNASelection: public CRenderRibbonSelection
{
public:
	CRenderDNASelection();
	virtual ~CRenderDNASelection();

	D3DXVECTOR3			m_posBegin;
	D3DXVECTOR3			m_posEnd;

	virtual void		GetModelPropertyValue();
	virtual	void		GetRenderPropertyValue();

	virtual int			GetSSType() { return SS_NONE; }
public:
};

//	selection 안에 포함되는 helix cylinder 구조
//	Helix의 끝은 무조건 막는다.
class CRenderHelixSelection: public CRenderRibbonSelection
{
public:
	CRenderHelixSelection();
	virtual ~CRenderHelixSelection();

	virtual void		GetModelPropertyValue();
	virtual	void		GetRenderPropertyValue();

	virtual int			GetSSType() { return SS_HELIX; }
private:

};

//	selection 안에 포함되는 Sheet 구조
//	Sheet의 끝은 무조건 막는다.
class CRenderSheetSelection: public CRenderRibbonSelection
{
public:
	CRenderSheetSelection();
	virtual ~CRenderSheetSelection();

	virtual void		GetModelPropertyValue();
	virtual	void		GetRenderPropertyValue();

	virtual int			GetSSType() { return SS_SHEET; }
private:
};

