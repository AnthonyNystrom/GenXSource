#pragma once

#include "RenderObjectSelection.h"

class CDrawText3D;
class CRenderProperty;
class CPropertyCommon;
class CPropertyWireframe;
class CPropertyStick;
class CPropertySpaceFill;
class CPropertyBallStick;
class CPropertyRibbon;
class CPropertySurface;
class CRenderSurfaceSelection;
class CCoordinateAxisDisplay;
class CClipPlane;

//	
//	Ư���κи� ���õǾ Ư���� display�� �ϴ� ����Ʈ�� �����Ѵ�.
//
//	��� �߰���, CSelectionListPane�� Duplicate�� ����.
//
class CSelectionDisplay : public CMoleculeRenderObject
{
public:
	CSelectionDisplay();
	virtual ~CSelectionDisplay();

	static long	m_maxSelectionIndex;

	long		m_iSerial;								//	selection display�� ������� ��������� index. m_arraySelectionDisplay�� ������� �ϸ� listCtrl�� �߰��� ������ �ȴ�.

	BOOL		m_bSelect;

	//	CSTLArraySelectionDisplay �迭�� �ε�����
	long					m_iDisplayStylePDB;			//	PDB �ȿ� �ִ� displayStyle�� index�� ����.
	CPDBRenderer *			m_pPDBRenderer;				//	���� selection �Ȱ��� ��� PDBRender �� ����ֳ�.
														//	selection 1������ �ݵ�� 1���� CPDBRenderer�� �������
	BOOL					m_bShow;

	long					m_iDisplaySelectionList;	//	selection display pane �� ���°�� display ������ ��Ÿ����.
  
	void					ChangeDisplayMode(long mode);
	void					SetModelQuality();

	//
	//
	void					InitDisplayStyleProperty(long mode);
	long					m_displayStyle;			
	CRenderProperty	*		m_pRenderProperty;				//	display style ����.

	CPropertyCommon		* GetPropertyCommon() {try{ return dynamic_cast<CPropertyCommon *>(m_pRenderProperty); }
	catch (CException* e)
	{
		return NULL;
	}}
	CPropertyWireframe	* GetPropertyWireframe() { try{ return dynamic_cast<CPropertyWireframe *>(m_pRenderProperty);  }
	catch (CException* e)
	{
		return NULL;
	}}
	CPropertyStick		* GetPropertyStick() { try{ return dynamic_cast<CPropertyStick *>(m_pRenderProperty); }
	catch (CException* e)
	{
		return NULL;
	}}
	CPropertySpaceFill	* GetPropertySpaceFill() {
		try{ return dynamic_cast<CPropertySpaceFill *>(m_pRenderProperty); }
		catch (CException* e)
		{
			return NULL;
		}
	}
	CPropertyBallStick	* GetPropertyBallStick() { try{
		return dynamic_cast<CPropertyBallStick *>(m_pRenderProperty);
	}
	catch (CException* e)
	{
		return NULL;
	}
	}
	CPropertyRibbon		* GetPropertyRibbon() {
		try
		{
			return dynamic_cast<CPropertyRibbon *>(m_pRenderProperty);
		}
		catch (CException* e)
		{
			return NULL;
		}

	}
	CPropertySurface	* GetPropertySurface() { 
		try
		{
			return dynamic_cast<CPropertySurface *>(m_pRenderProperty);}
		catch (CException* e)
		{
			return NULL;
		} 
	}
	
	//
	//	���õ� selection object�� �����̳�
	//
	CSTLArraySelectionInst		m_selectionInst;
	CSTLArrayAtomInst			m_arrayAtomInst;
	void SetSelection(CSTLArraySelectionInst & selectionInst);

	//	0 is VP
	//	1 is Atom
	//	2 is Residue
	enum { ANNOTATION_VP, ANNOTATION_ATOM, ANNOTATION_RESIDUE };
	CDrawText3D	*			m_pAnnotation[3];                // Font for drawing text
	void					CreateAnnotation(int iAnno);
	void					DeleteAnnotation( long index );
	void					SetAnnotationInfo(int iAnno);
	void					UpdateAnnotation();

	//
	//
	//
	D3DXVECTOR2				m_rangeOccupancy;
	D3DXVECTOR2				m_rangeTemperature;
	D3DXVECTOR2				m_rangeHydropathy;

	D3DXVECTOR3				m_center;			//	bb�� center. Transform ���� ���� ��ǥ.
	FLOAT					m_radius;			//	bb�� radius
	D3DXVECTOR3				m_minMaxBB[2];		//	minmaxBV
	//	D3DXVECTOR3				m_BBPos[8];		//	annotation�� ��ġ��Ű�� ���Ѱ�
	void					FindCenterRadius();

	//	
	void					InitChainColorScheme();
	void					InitCustomColorScheme();

	//    ������ color scheme�� ������, atom �� �ش�Ǵ� �÷��� ���Ѵ�.
	CColorRow * GetAtomColor(CAtom * );

	//	Selection Render Object�� container.
	CSTLArrayRenderObjectSelection	m_arrayRenderObjectSelection;

	CCoordinateAxisDisplay	* m_pCoordinateAxis;

	//	
	CClipPlane *		m_pClipPlane1;
	CClipPlane *		m_pClipPlane2;
	void	SetClipPlaneEquationToShader();

	HRESULT InitDeviceObjects();
	HRESULT	FrameMove();
	HRESULT Render();
	HRESULT DeleteDeviceObjects();
	HRESULT RestoreDeviceObjects();
	HRESULT InvalidateDeviceObjects();

	HRESULT UpdateAtomPosColorChanged();			//	atom color ��ü�� �ٲٴ°�.
	HRESULT UpdateAtomSelectionChanged();			//	selection color �� ����� ����
	HRESULT UpdateSurfaceCurvatureChanged();

	HRESULT RenderAnnotation();

	//
	//	property changed
	//
	void	SetPropertyChanged(long mode,CString pValue="");

	//    color selection dialog
	void	OnColorSelectionDialogBox(int iScheme, BOOL bGradient = FALSE );
	void	OnColorSelectionDialogBox(CArrayColorRow & colorRow, CArrayColorRow & colorRowDefault, BOOL bGradient);

	HRESULT	InitRenderSceneSelection();
	void	InitRenderSelection(CChainInst * pChainInst);

	//enum	{ WIREFRAME, STICKS, SPACEFILL, BALLANDSTICK, RIBBON, SURFACE,SCENCE,NGRealistic };
	enum	{ WIREFRAME, STICKS, SPACEFILL, BALLANDSTICK, RIBBON, SURFACE,NGRealistic };
	HRESULT InitRenderSelectionSpaceFill(CChainInst * pChainInst);
	HRESULT	InitRenderSelectionWire (CChainInst * pChainInst);
	HRESULT	InitRenderSelectionSurface(CChainInst * pChainInst);

	void	CleanUnusedSurfaceModelMemory();
	void	CleanUnusedRibbonModelMemory();

	HRESULT	InitRenderSelectionRibbon(CChainInst * pChainInst);
	HRESULT	InitRenderSelectionBallStick(CChainInst * pChainInst, BOOL bStick);

	void	SelectSurfaceAtom();

	//	
	static CSelectionDisplay* CreateSelectionDisplay(long mode);

private:
};

typedef std::vector < CSelectionDisplay * >		CSTLArraySelectionDisplay;

