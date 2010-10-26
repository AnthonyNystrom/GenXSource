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
//	특정부분만 선택되어서 특정한 display를 하는 리스트를 관리한다.
//
//	멤버 추가시, CSelectionListPane의 Duplicate를 수정.
//
class CSelectionDisplay : public CMoleculeRenderObject
{
public:
	CSelectionDisplay();
	virtual ~CSelectionDisplay();

	static long	m_maxSelectionIndex;

	long		m_iSerial;								//	selection display의 만들어진 순서대로의 index. m_arraySelectionDisplay의 순서대로 하면 listCtrl의 중간에 삽입이 된다.

	BOOL		m_bSelect;

	//	CSTLArraySelectionDisplay 배열의 인덱스임
	long					m_iDisplayStylePDB;			//	PDB 안에 있는 displayStyle의 index를 관리.
	CPDBRenderer *			m_pPDBRenderer;				//	현재 selection 된것이 어느 PDBRender 에 들어있나.
														//	selection 1개에는 반드시 1개의 CPDBRenderer가 들어있음
	BOOL					m_bShow;

	long					m_iDisplaySelectionList;	//	selection display pane 에 몇번째로 display 된지를 나타낸다.
  
	void					ChangeDisplayMode(long mode);
	void					SetModelQuality();

	//
	//
	void					InitDisplayStyleProperty(long mode);
	long					m_displayStyle;			
	CRenderProperty	*		m_pRenderProperty;				//	display style 설정.

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
	//	선택된 selection object의 컨테이너
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

	D3DXVECTOR3				m_center;			//	bb의 center. Transform 되지 않은 좌표.
	FLOAT					m_radius;			//	bb의 radius
	D3DXVECTOR3				m_minMaxBB[2];		//	minmaxBV
	//	D3DXVECTOR3				m_BBPos[8];		//	annotation을 위치시키기 위한것
	void					FindCenterRadius();

	//	
	void					InitChainColorScheme();
	void					InitCustomColorScheme();

	//    현재의 color scheme을 가지고, atom 에 해당되는 컬러를 구한다.
	CColorRow * GetAtomColor(CAtom * );

	//	Selection Render Object의 container.
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

	HRESULT UpdateAtomPosColorChanged();			//	atom color 전체를 바꾸는것.
	HRESULT UpdateAtomSelectionChanged();			//	selection color 만 고려해 설정
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

