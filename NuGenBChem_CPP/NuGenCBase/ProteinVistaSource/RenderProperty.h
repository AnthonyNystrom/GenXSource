#pragma once
 
#include <list>

//
void ReserveWrite(CFile * fpWorkspace, int nDWORD);
LONG ReserveLoad(CFile * fpWorkspace, int nDWORD);

//
class CRenderPropertyPane;
class CSelectionDisplay;
class CPropertyCommon;

inline D3DXCOLOR COLORREF2D3DXCOLOR(COLORREF color)
{
	return D3DXCOLOR(GetRValue(color)/255.0f, GetGValue(color)/255.0f, GetBValue(color)/255.0f, 0.0f);
}

class CRenderProperty
{
public:
	CRenderProperty();
	virtual ~CRenderProperty(){}

	CWnd *				m_pParent;
	 
	virtual HRESULT		InitProperty(int modeValue =0){ return S_OK; }

	virtual	HRESULT		Load(CFile * fpWorkspace) { return S_OK; }
	virtual	HRESULT		Save(CFile * fpWorkspace) { return S_OK; }
};

class CPropertyScene: public CRenderProperty
{
public:
	CPropertyScene();

	D3DCOLOR	m_d3dcolorBackroundColor;
	COLORREF	m_colorBackroundColor;
	BOOL		m_bUseBackgroundTexture;
	CString		m_strBackgroundTextureFilename;

	int			m_modelQuality;
	int			m_shaderQuality;

	BOOL		m_bDepthOfField;
	COLORREF	m_fogColor;
	long		m_fogStart;
	long		m_fogEnd;

	double		m_fNearClipPlane;
	double		m_fFarClipPlane;

	BOOL		m_bDisplayAxis;
	long		m_axisScale;

	enum {	AA_NONE, AA_LOW, AA_MEDIUM, AA_HIGH };
	int			m_iAntialiasing;

	BOOL		m_bLight1Use;
	BOOL		m_bLight1Show;
	COLORREF	m_light1Color;
	long		m_light1Intensity;
	DOUBLE		m_fLight1Radius;

	BOOL		m_bLight2Use;
	BOOL		m_bLight2Show;
	COLORREF	m_light2Color;
	long		m_light2Intensity;
	DOUBLE		m_fLight2Radius;

	BOOL		m_bShowSelectionMark;

	BOOL		m_bClipping0;
	BOOL		m_bShowClipPlane0;
	COLORREF	m_clipPlaneColor0;
	long		m_clipPlaneTransparency0;
	BOOL		m_bClipDirection0;
	long		m_radiusClipPlane0;

	D3DXPLANE	m_clipPlaneEquation0;

	enum {	DISPLAY_HETATM_NONE, DISPLAY_HETATM_SPACEFILL, DISPLAY_HETATM_BALL };
	int			m_iDisplayHETATM;

	enum {	CAMERA_OTHO, CAMERA_PERSPECTIVE } ;
	int			m_cameraType;
	long		m_lFOV;
	long		m_othoCameraViewVol;
	
	COLORREF	m_selectionColor;

	BOOL		m_bDoubleClockToCameraAnimation;
	DOUBLE		m_fAnimationTime;

	//	ssao
	BOOL		m_bUseSSAO;
	 
	long		m_numSSAOSampling;
	 
	long		m_ssaoRange;
 
	long		m_ssaoIntensity;
	 
	enum { BLUR_NONE, BLUR_4, BLUR_16 };
	int			m_ssaoBlurType;
	 
	BOOL		m_bUseFullSizeBlur;
  
	virtual	HRESULT		Load(CFile * fpWorkspace);
	virtual	HRESULT		Save(CFile * fpWorkspace);

private: 
	long				m_dwReserve;
};

class CPropertyCommon
{
public:
	CPropertyCommon(CSelectionDisplay * pSelectionDisplay);
	virtual ~CPropertyCommon();

	CSelectionDisplay * m_pSelectionDisplay;

	int			m_enumDisplayMode;

	CString		m_strSelectionName;

	BOOL		m_bDisplaySideChain;
	BOOL		m_bDisplayHETATM;

	BOOL		m_bIndicate;
	COLORREF	m_indicateColor;
	int			m_indicateColorSlot;		//	piw 에 저장하지 않는 변수

	int			m_enumColorScheme;
	COLORREF	m_singleColor;

	void		InitColorScheme();
	CArrayColorRow	m_arrayColorSchemeDefault[NUM_COLOR_SCHEME];
	CArrayColorRow	m_arrayColorScheme[NUM_COLOR_SCHEME];

	BOOL		m_bShowSelectionMark;
	//	COLORREF	m_selectionColor;

	int			m_modelQuality;
	int			m_shaderQuality;

	BOOL		m_bAnnotation[3];
	int			m_enumTextDisplayTechnique[3];
	CString		m_strAnnotation[3];
	int			m_enumAnnotatonType[3];
	int			m_enumAnnotationColorScheme[3];
	COLORREF	m_annotationColor[3];
	int			m_enumAnnotationPos[3];
	LOGFONT		m_logFont[3];
	LONG		m_annotationXPos[3];
	LONG		m_annotationYPos[3];
	LONG		m_annotationZPos[3];
	LONG		m_annotationTextXPos[3];
	LONG		m_annotationTextYPos[3];
	LONG		m_annotationTransparency[3];
	LONG		m_annotationDepthBias[3];

 	//	
	BOOL		m_bClipping1;
	BOOL		m_bShowClipPlane1;
	COLORREF	m_clipPlaneColor1;
	long		m_clipPlaneTransparency1;
	BOOL		m_bClipDirection1;

	D3DXPLANE	m_clipPlaneEquation1;

	//	
	BOOL		m_bClipping2;
	BOOL		m_bShowClipPlane2;
	COLORREF	m_clipPlaneColor2;
	long		m_clipPlaneTransparency2;
	BOOL		m_bClipDirection2;

	D3DXPLANE	m_clipPlaneEquation2;

	// 	BOOL		m_bDepthOfField;
	// 	COLORREF	m_fogColor;
	// 	long		m_fogStart;
	// 	long		m_fogEnd;

	BOOL		m_bClipPS;

	long		m_intensityAmbient;
	long		m_intensiryDiffuse;
	long		m_intensitySpecular;

	BOOL		m_bDisplayAxisLocalCoord;
	long		m_axisScaleLocalCoord;

	HRESULT		InitCommonProperty();

	HRESULT		Load(CFile * fpWorkspace);
	HRESULT		Save(CFile * fpWorkspace);

protected:
	long				m_dwReserve;

};

class CPropertyWireframe: public CRenderProperty, public CPropertyCommon
{
public:
	CPropertyWireframe(CSelectionDisplay * pSelectionDisplay);

	CSelectionDisplay * m_pSelectionDisplay;

	int			m_enumDisplayMethod;
	long		m_lineWidth;		//	1 to 100
	long		m_lineWidthOld;		//	1에서 1이 아닌것으로 변할때만 Initdevice를 하기위해 사용.

	virtual	HRESULT		Load(CFile * fpWorkspace);
	virtual	HRESULT		Save(CFile * fpWorkspace);
};

class CPropertyStick: public CRenderProperty, public CPropertyCommon
{
public:
	CPropertyStick(CSelectionDisplay * pSelectionDisplay);
	
	CSelectionDisplay * m_pSelectionDisplay;

	long	m_sphereResolution;
	long	m_cylinderResolution;
	DOUBLE	m_stickSize;

	virtual	HRESULT		Load(CFile * fpWorkspace);
	virtual	HRESULT		Save(CFile * fpWorkspace);

};

class CPropertyBallStick: public CRenderProperty, public CPropertyCommon
{
public:
	CPropertyBallStick(CSelectionDisplay * pSelectionDisplay);

	CSelectionDisplay * m_pSelectionDisplay;

	long	m_sphereResolution;
	long	m_cylinderResolution;
 
	DOUBLE	m_sphereRadius;
	DOUBLE	m_cylinderSize;
 
	virtual	HRESULT		Load(CFile * fpWorkspace);
	virtual	HRESULT		Save(CFile * fpWorkspace);
};

class CPropertySpaceFill: public CRenderProperty, public CPropertyCommon
{
public:
	CPropertySpaceFill(CSelectionDisplay * pSelectionDisplay);

	CSelectionDisplay * m_pSelectionDisplay;

	FLOAT		m_atomRadiusDefault[MAX_ATOM];
	FLOAT		m_atomRadius[MAX_ATOM];
	long				m_sphereResolution;
	virtual	HRESULT		Load(CFile * fpWorkspace);
	virtual	HRESULT		Save(CFile * fpWorkspace);
};

class CPropertyRibbon: public CRenderProperty, public CPropertyCommon
{
public:
	CPropertyRibbon(CSelectionDisplay * pSelectionDisplay);

	CSelectionDisplay * m_pSelectionDisplay;
	//    ribbon common
	long		m_curveTension;
	long		m_resolution;

	//	Helix
	//	helix shape: cylinder, round cylinder, coil
	BOOL		m_bTextureHelix;				//	texture 사용 on/off
	CString		m_strTextureFilenameHelix;
	long		m_textureCoordUHelix;
	long		m_textureCoordVHelix;
	COLORREF	m_colorHelix;
	BOOL		m_bDisplayHelix;
	CSize		m_sizeHelix;
	//    Helix 모양: 원통(Fitting), 원통(시작점끝점), coil과 같은 모양.
	int			m_fittingMethodHelix;
	//    helix 형태: 원통, 삼각, 사각형, 오각형, 육각형, 타원. 
	int			m_shapeHelix;
	//    커브의 텐션
	long		m_curveTensionHelix;
	//    
	long		m_resolutionHelix;
	BOOL		m_bShowCoilOnHelix;

	//	Sheet
	//	sheet shape: 
	BOOL		m_bTextureSheet;		//	texture 사용 on/off
	CString		m_strTextureFilenameSheet;
	long		m_textureCoordUSheet;
	long		m_textureCoordVSheet;
	COLORREF	m_colorSheet;
	BOOL		m_bDisplaySheet;
	CSize		m_sizeSheet;
	long		m_curveTensionSheet;
	long		m_resolutionSheet;
	int			m_shapeSheet;		//
	BOOL		m_bShowCoilOnSheet;

	//	Coil
	BOOL		m_bTextureCoil;			//	texture 사용 on/off
	CString		m_strTextureFilenameCoil;
	long		m_textureCoordUCoil;
	long		m_textureCoordVCoil;
	COLORREF	m_colorCoil;
	CSize		m_sizeCoil;
	BOOL		m_bDisplayCoil;			//    coil display On/Off
	long		m_curveTensionCoil;
	long		m_resolutionCoil;
	int			m_shapeCoil;
	
	virtual	HRESULT		Load(CFile * fpWorkspace);
	virtual	HRESULT		Save(CFile * fpWorkspace);
};

typedef std::vector<COLORREF> CColorsList;
class CPropertySurface: public CRenderProperty, public CPropertyCommon
{
public:
	CPropertySurface(CSelectionDisplay * pSelectionDisplay);
	virtual ~CPropertySurface();

	CSelectionDisplay * m_pSelectionDisplay;

	int			m_enumSurfaceDisplayMethod;
	long		m_transparency;			//	0 이면 불투명.

	double		m_probeSphere;
	int			m_surfaceQuality;
	BOOL		m_bAddHETATM;
	BOOL		m_surfaceGenMethod;

	BOOL		m_bDisplayCurvature;
	int			m_curvatureRingSize;

	int			m_iSurfaceBlurring;
	BOOL		m_bSurfaceDepthSort;
	
	BOOL		m_useInnerFaceColor;
	long		m_blendFactor;
	COLORREF	m_colorInnerFace;

	CArrayColorRow	m_arrayColorRowCurvature;
	CArrayColorRow	m_arrayColorRowCurvatureDefault;
	BOOL		m_bSurfaceCutPlaneCap;

	virtual	HRESULT		Load(CFile * fpWorkspace);
	virtual	HRESULT		Save(CFile * fpWorkspace);
	virtual HRESULT		InitProperty(int modeValue =0);
 
};
