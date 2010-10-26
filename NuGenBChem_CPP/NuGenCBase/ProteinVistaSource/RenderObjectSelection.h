#pragma once

class CPropertyCommon;
class CPropertyWireframe;
class CPropertyStick;
class CPropertySpaceFill;
class CPropertyBallStick;
class CPropertyRibbon;
class CPropertySurface;

class CRenderObjectSelection : public CMoleculeRenderObject
{
public:
	CRenderObjectSelection() {m_pChainInst = NULL; m_iDisplaySelection = m_colorScheme = 0;	m_pPDBRenderer = NULL;	m_pProteinVistaRenderer = NULL;	m_pPropertyCommon = NULL; }
	virtual ~CRenderObjectSelection() {}

	CChainInst *			m_pChainInst;			//	render Object�� chainInst.

	long				m_iDisplaySelection;		//	���� VP �� ����Ʈ������ index��ġ.

	//	
	long				m_colorScheme;

	CPDBRenderer *		m_pPDBRenderer;

	CPropertyCommon *	m_pPropertyCommon;

	//	Init is not virtual
	void	Init() { }

	virtual void	SetModelQuality(){}
	virtual void	SetShaderQuality(){}
	virtual void	SetShowSelectionMark(){}

	virtual HRESULT InitDeviceObjects() { return S_OK; }
	virtual HRESULT DeleteDeviceObjects() { return S_OK; }
	virtual HRESULT Render() { return S_OK; }
	virtual HRESULT UpdateAtomSelectionChanged(){DeleteDeviceObjects();InitDeviceObjects(); return S_OK; }
	virtual HRESULT UpdateAtomPosColorChanged(){DeleteDeviceObjects();InitDeviceObjects(); return S_OK; }
	virtual void	ResetTexture() {}
};

typedef std::vector < CRenderObjectSelection * > CSTLArrayRenderObjectSelection;


