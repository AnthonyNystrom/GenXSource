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

//	�ϳ��� selection �ȿ����� �׷��� atom�� continuous �ϴ�.
//	helix�� �ϳ��� selection�ȿ��� ������ ����� �ִ�.
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
	//	�������Ǵ� ����.
	//	parent �� �ִ� ���
	//CChainInst *					m_pChainInst;

	long						m_beginResidue;
	long						m_endResidue;

	CSTLArrayResidueInst		m_arrayResidueInst;		//	Ca �� ��Ī�Ǵ� CResidue

// 
// 	CSTLArrayD3DXVECTOR3		m_arrayCarbonAtom;
// 	CSTLArrayD3DXVECTOR3		m_arrayUpVec;

	//
	//	m_arrayCarbonAtom�� ������ Ribbon�� �߽��� ���Ͽ� �д�. Picking �� ���
	//	picking ��� ����(pick.cpp)
	//	rendering�����ؼ� �̰� ������ �ÿ� ���.
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
//    coil, helix, sheet�� ������ �ϴ� Ŭ������ parent class
class CRenderRibbonSelection: public CMoleculeRenderObject
{
public:
	CRenderRibbonSelection();
	virtual ~CRenderRibbonSelection();

	CRenderRibbonSelectionContainer * m_pRenderRibbonSelectionContainer;

	//	���Ǹ� ���� �����س��� �Լ�
	CPDBRenderer *		m_pPDBRenderer;

	//	device�� �߰��� �ٲ�� �ִ�. Multi-sample������ device �� �ٲ�� �ִ�.
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

	long				m_numRibbonLen;		//	���� selection ���� ����(atom ���� * m_numSegment)
	long				m_totalVertex;		//	���� selection ���� vertex ����(atom ���� * m_numSegment * slices)

	//    for picking. called in Pick.cpp
	void	Picking(D3DXVECTOR3 &pickRayDir, D3DXVECTOR3 &pickRayOrig, CSTLArrayPickedResidueInst & pickResidueArray );

	D3DFORMAT		m_formatIndexBuffer;
	UINT			m_byteSizeIndexBuffer;

public:
	LPDIRECT3DVERTEXDECLARATION9	m_pVertexDeclRibbon;

	IDirect3DVertexBuffer9	* m_pVB;		//	SS �� vertex buffer.
	IDirect3DIndexBuffer9	* m_pIB;

	IDirect3DVertexBuffer9	* m_pVBCap;		//	cap�� ���� vertex buffer
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

//	selection �ȿ� ���ԵǴ� helix cylinder ����
//	Helix�� ���� ������ ���´�.
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

//	selection �ȿ� ���ԵǴ� Sheet ����
//	Sheet�� ���� ������ ���´�.
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

