#pragma once

#include "RenderObjectSelection.h"

class CProteinSurfaceMSMS;
class CProteinSurfaceMQ;
class CProteinSurfaceBase;

//===================================================================================================
//	Direct3D
struct CSurfaceVertex
{
	D3DXVECTOR3	pos;
	D3DXVECTOR3	normal;
	D3DCOLOR	diffuse;
};

//    const DWORD SurfaceVertexFVF = ( D3DFVF_XYZ | D3DFVF_NORMAL | D3DFVF_DIFFUSE );

//	
//	selection 된것의 surface 데이타 정보.
//	CProteinSurface에서 vertex buffer를 복사해 오고, selection에 따른 index buffer를 재구성.
//	
class CRenderSurfaceSelection: public CRenderObjectSelection
{
public:
	CRenderSurfaceSelection();
	~CRenderSurfaceSelection();

	CProteinSurfaceBase*	m_pProteinSurface;

	CSTLArrayAtomInst		m_arrayAtomInst;

	CPropertySurface	* m_pPropertySurface;

	void		SetModelQuality();

	HRESULT		InitDeviceObjects();
	HRESULT		DeleteDeviceObjects();
	HRESULT		Render();

	HRESULT		SetVertexColor();
	void		SurfaceCurvatureColoring();

	HRESULT		UpdateAtomSelectionChanged();

	LPDIRECT3DVERTEXDECLARATION9	m_pDeclSurface;

	IDirect3DVertexBuffer9	* m_pVB;		//	surface 전체의 vertex buffer를 복사해서 가지고 있음. vertex color때문에 어쩔수 없음.

	IDirect3DIndexBuffer9	* m_pIB;		//	selection 되는 부분에 대한 index buffer를 새로 만듬
	long			m_sizeIndexBuffer;

	IDirect3DIndexBuffer9	* m_pIBSelection;	
	long			m_sizeIndexBufferSelection;

	D3DFORMAT		m_formatIndexBuffer;
	UINT			m_byteSizeIndexBuffer;

	//	depth sorting을 위한, vertex center list
	CSTLVectorValueArray	m_arrayVertexCenter;		//	center
	CSTLVectorValueArray	m_arrayVertexCenterTr;		//	transformed 되어진 center
	CSTLLongArray			m_indexBufferOrig;			//	orig indexed buffer 

	//	현재 선택된것의 index buffer를 구한다.
	HRESULT GetIndexBuffer(CSTLLongArray & arrayIndexBuffer, BOOL bCountSelect );

	//    추가.
	//    ambient, specular light.
	//    curvature color
	//    cut plane cap.

	//	center의 depth 에 따라서 index buffer를 소팅한다.
	//	custom quick sort.
	void	DepthSort(D3DXVECTOR3 array[], int l, int r);
	CSTLLongArray			m_indexBufferSorted;		//	sorted Index buffer.

	//	blurring
	void	SurfaceBlurring();		//
};

typedef std::vector < CRenderSurfaceSelection * > CSTLArrayRenderSurfaceSelection;


