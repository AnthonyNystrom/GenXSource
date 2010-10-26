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
//	selection �Ȱ��� surface ����Ÿ ����.
//	CProteinSurface���� vertex buffer�� ������ ����, selection�� ���� index buffer�� �籸��.
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

	IDirect3DVertexBuffer9	* m_pVB;		//	surface ��ü�� vertex buffer�� �����ؼ� ������ ����. vertex color������ ��¿�� ����.

	IDirect3DIndexBuffer9	* m_pIB;		//	selection �Ǵ� �κп� ���� index buffer�� ���� ����
	long			m_sizeIndexBuffer;

	IDirect3DIndexBuffer9	* m_pIBSelection;	
	long			m_sizeIndexBufferSelection;

	D3DFORMAT		m_formatIndexBuffer;
	UINT			m_byteSizeIndexBuffer;

	//	depth sorting�� ����, vertex center list
	CSTLVectorValueArray	m_arrayVertexCenter;		//	center
	CSTLVectorValueArray	m_arrayVertexCenterTr;		//	transformed �Ǿ��� center
	CSTLLongArray			m_indexBufferOrig;			//	orig indexed buffer 

	//	���� ���õȰ��� index buffer�� ���Ѵ�.
	HRESULT GetIndexBuffer(CSTLLongArray & arrayIndexBuffer, BOOL bCountSelect );

	//    �߰�.
	//    ambient, specular light.
	//    curvature color
	//    cut plane cap.

	//	center�� depth �� ���� index buffer�� �����Ѵ�.
	//	custom quick sort.
	void	DepthSort(D3DXVECTOR3 array[], int l, int r);
	CSTLLongArray			m_indexBufferSorted;		//	sorted Index buffer.

	//	blurring
	void	SurfaceBlurring();		//
};

typedef std::vector < CRenderSurfaceSelection * > CSTLArrayRenderSurfaceSelection;


