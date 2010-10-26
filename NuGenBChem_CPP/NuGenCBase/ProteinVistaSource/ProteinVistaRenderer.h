//-----------------------------------------------------------------------------
// File: ProteinVistaRenderer.h
//-----------------------------------------------------------------------------
#pragma once

#include "pdb.h"
#include "pdbInst.h"
#include "RenderProperty.h"
#include "SelectionDisplay.h"
#include "ColorScheme.h"
#include "DxUTMisc.h"
#include "CoordinateAxis.h"
#include "LightWidget.h"
#include "Pick.h"

#include <map>

class CPDBRenderer;

typedef std::vector < CPDBRenderer * > CSTLArrayPDBRenderer;

class	CProteinSurfaceBase;
typedef std::vector <CProteinSurfaceBase *>			CSTLArrayProteinSurfaceBase;

class CProteinRibbonVertexData;
typedef std::vector <CProteinRibbonVertexData *>		CSTLArrayCProteinRibbonVertexData;

#define		MAX_LIGHTS						2
#define		MAX_RENDERING_TECHNIQUE			21
#define		MAX_SELECTION_COLOR				7

class CRenderQualityPreset;
class CFontTextureContainer;
class CClipPlane;

const 	int			m_maxSamplesSSAO = 64;
//=================================================================================
//=================================================================================

struct fvfAtomSphereVertex 
{ 
	D3DXVECTOR3	FvfVertex; 
	D3DXVECTOR3	FvfNormal; 
	enum { FVF = (D3DFVF_XYZ | D3DFVF_NORMAL ) };
};

struct fvfBondVertex 
{ 
	D3DXVECTOR3	FvfVertex; 
	D3DXVECTOR3	FvfNormal; 
	enum { FVF = ( D3DFVF_XYZ | D3DFVF_NORMAL ) };
};

class	CProteinVistaView;
class	CProteinVistaDoc;
class	CMolecule;
class	CSkyManager;

// Struct to store the current input state
struct UserInput
{
    BOOL bRotateUp;
    BOOL bRotateDown;
    BOOL bRotateLeft;
    BOOL bRotateRight;
};

//    Render Quality Constant
class CRenderQualityPreset
{
public:
	//    ball&stick and stick
	long	m_sphereResolution;		//	 + spacefill
	long	m_cylinderResolution;

	//    Ribbon
	long	m_ribbonResolution;

	int		m_modelQuality;

	//    Surface
	int		m_surfaceQuality;	//	0..10

	BOOL	m_bShowSelectionMark;

	int		m_shaderQuality;

	CRenderQualityPreset()
	{
		//    render Quality is 0..4
		//    0 is very low, 4 is very high, 2 is default.
		m_sphereResolutionConst[0] = 8;
		m_sphereResolutionConst[1] = 12;
		m_sphereResolutionConst[2] = 16;
		m_sphereResolutionConst[3] = 24;
		m_sphereResolutionConst[4] = 30;

		m_cylinderResolutionConst[0] = 5;
		m_cylinderResolutionConst[1] = 8;
		m_cylinderResolutionConst[2] = 14;
		m_cylinderResolutionConst[3] = 20;
		m_cylinderResolutionConst[4] = 26;

		m_ribbonResolutionConst[0] = 3;
		m_ribbonResolutionConst[1] = 4;
		m_ribbonResolutionConst[2] = 6;
		m_ribbonResolutionConst[3] = 8;
		m_ribbonResolutionConst[4] = 14;

		m_surfaceQualityConst[0] = 0;
		m_surfaceQualityConst[1] = 2;
		m_surfaceQualityConst[2] = 5;
		m_surfaceQualityConst[3] = 8;
		m_surfaceQualityConst[4] = 10;

		SetModelQuality(3);
		SetShaderQuality(1);
		SetShowSelectionMark(1);
	}

	int		GetSurfaceQuality() { return m_surfaceQuality; }	//	0..10
	void	SetSurfaceQuality(int quality) { m_surfaceQuality = quality; }	//	0..10

	void SetModelQuality(int Quality)
	{
		if ( Quality < 0 || Quality > 4 )
			return;

		m_modelQuality = Quality;

		m_sphereResolution = m_sphereResolutionConst[Quality];
		m_cylinderResolution = m_cylinderResolutionConst[Quality];
		m_sphereResolution = m_sphereResolutionConst[Quality];
		m_ribbonResolution = m_ribbonResolutionConst[Quality];
		m_surfaceQuality = m_surfaceQualityConst[Quality];
	}

	void SetShaderQuality(int shaderQuality)
	{
		m_shaderQuality = shaderQuality;
	}

	void SetShowSelectionMark(BOOL bShow)
	{
		m_bShowSelectionMark = bShow;
	}
	
public:
	long	m_sphereResolutionConst[5];
	long	m_cylinderResolutionConst[5];
	long	m_ribbonResolutionConst[5];
	int		m_surfaceQualityConst[5];
};

class CTextureInfo
{
public:
	CTextureInfo() { m_pD3DXTexture = NULL; m_refCount = 0; }
	~CTextureInfo() { SAFE_RELEASE(m_pD3DXTexture); }
	CString					m_strFilename;
	LPDIRECT3DTEXTURE9		m_pD3DXTexture;
	int						m_refCount;

	static HRESULT Load(CFile * file);
	static HRESULT Save(CFile * file, CString filenameTexture);
	static void ChangeTexturePathFilename(CString strFilenameOrig, CString & strFilenameConverted);
};

typedef std::map<CString, CTextureInfo*> CMapTextureFilename;

struct SCREEN_VERTEX
{
	FLOAT pos[4];
	FLOAT tex[4][2];

	static const DWORD FVF;
};

//
//	CProteinVistaView 에서 Rendering 부분에 관련된 것만 떼어놓은것.
//
//-----------------------------------------------------------------------------
// Name: class CProteinVistaRenderer
// Desc: Application class. The base class (CD3DApplication) provides the 
//       generic functionality needed in all Direct3D samples. CProteinVistaRenderer 
//       adds functionality specific to this sample program.
//-----------------------------------------------------------------------------
class CProteinVistaRenderer : public CD3DApplication
{
public:
	friend					CProteinVistaView;

public:
	BOOL m_bShowLog;
	CProteinVistaRenderer(CProteinVistaView * pProteinVistaView);
    virtual ~CProteinVistaRenderer();
	BEGIN("PDB")
		public:
			CSTLArrayPDBRenderer	m_arrayPDBRenderer;		//	PDBRenderer Array

			CPDBRenderer * AddPDBRenderer(CPDB * pPDB);
			HRESULT		DeletePDBRenderer(int nIndex);
			HRESULT		DeletePDBRenderer(CPDBRenderer * pPDBRendererDelete);

			void		SetInitialDefaultSelection(CPDBRenderer * pPDBRenderer, int displayMethod );

			CPDBRenderer			* m_pActivePDBRenderer;		//	현재 active 되어있는 pdbRenderer 이다.
														//	NULL 이면 하나도 active 되어 있는 경우가 없다.

			void		SetActivePDBRenderer(CPDBRenderer * pPDBRenderer);
	END;
	
	BEGIN("Direct3D Framework");
		public:
			CD3DFont*               m_pFont;                // Font for drawing text

			//	투명한것을 마지막에 렌더링하기 위해 저장해 놓는곳.
			CSTLArrayRenderObject	m_transparentRenderObject;

		private:
			BOOL                    m_bLoadingApp;          // TRUE, if the app is loading

			UserInput               m_UserInput;            // Struct for storing user input 

			LRESULT MsgProc( HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam );

		public:
			CProteinVistaView * m_pProteinVistaView;

			virtual HRESULT OneTimeSceneInit();
			virtual HRESULT InitDeviceObjects();
			virtual HRESULT RestoreDeviceObjects();
			virtual HRESULT InvalidateDeviceObjects();
			virtual HRESULT DeleteDeviceObjects();
			virtual HRESULT Render();
			virtual HRESULT FrameMove();

			void SetViewVolClipPlane();
			virtual HRESULT FinalCleanup();
			virtual HRESULT ConfirmDevice( D3DCAPS9*, DWORD, D3DFORMAT , D3DFORMAT );

			HRESULT RenderText();

			void    UpdateInput( UserInput* pUserInput );

			LPDIRECT3DDEVICE9 GetD3DDevice(){ return m_pd3dDevice; }

			HRESULT SetViewVolume();
	END;

	BEGIN("Camera position and camera rotation")
		public:
			D3DXMATRIXA16 *		GetViewMatrix()		{ return &m_matrixView; }
			D3DXMATRIXA16 *		GetProjMatrix()		{ return &m_matrixProj; }

		private:
			D3DXMATRIXA16		m_matrixView;
			D3DXMATRIXA16		m_matrixProj;

		public:
			float			m_fovY;
			float			m_fAspectRatio;

			D3DXVECTOR3		m_FromVec;			//	camera From Vec
			D3DXVECTOR3		m_FromVecOld;
			D3DXVECTOR3		m_ToVec;			//	camera To Vec

			FLOAT			m_cameraZPos;		//	camera의 z 값.

			D3DXMATRIXA16	m_matCameraRotation;
			D3DXMATRIXA16	m_matCameraRotationTemp;

			LRESULT			HandleMessageCamera( HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam );

			//	
			D3DXVECTOR3		m_center;		//	전체 PDB의 center.
			FLOAT			m_radius;		//	전체 PDB의 radius
			FLOAT			m_lightRadius;

			FLOAT			m_fNearClipPlane;
			FLOAT			m_fFarClipPlane;

			void			AttatchBioUnit();

			void			CameraViewAll();
			float			GetCameraZPosinTwoPoint(D3DXVECTOR3 minBB, D3DXVECTOR3 maxBB);
			float			GetCameraZPosInSphere(D3DXVECTOR3 center, FLOAT radius);

		private:
			BOOL			m_bCameraDrag;
			long			m_posOldX, m_posOldY;
	END;
	BEGIN("D3D 렌더링");
		public:
			CSkyManager	*		m_pSkyManager;

		public:
			LPDIRECT3DVERTEXDECLARATION9	m_pDeclAtomSphere;
			LPDIRECT3DVERTEXDECLARATION9	m_pDeclBond;
			LPDIRECT3DVERTEXDECLARATION9	m_pDeclLine;

			long						m_nSphereSlices[3];
			long						m_nSphereStacks[3];

			long						m_nAtomVertices[3];
			long						m_nAtomFaces[3];

			//	spaceFill radius 1.0
			//	stick 모델에서 stick 사이를 메꾸는 작은 sphere
			//	ball_stick 모델에서 ball.
			enum { INDEX_SPHERE_SPACEFILL, INDEX_SPHERE_BALL_STICK_DISPLAY, INDEX_SPHERE_BALL_DISPLAY };
			//			1.0,					0.5,							0.25	
			float						m_sphereRadius[3];

			IDirect3DVertexBuffer9 *	m_pMeshAtomVertexBuffer[3];
			IDirect3DIndexBuffer9 *		m_pMeshAtomIndexBuffer[3];

			HRESULT MakeAtomSphereMesh();

			//
			//
			long					m_maxBondVertex;
			LPD3DXMESH				m_pD3DXMeshBond;

			HRESULT MakeAtomBondMesh();

			CRenderQualityPreset	m_renderQualityPreset;

			D3DFORMAT				m_formatIndexBuffer;
			UINT					m_byteSizeIndexBuffer;

			DWORD					m_maxVertexIndex;

			UINT					m_versionVS;
			UINT					m_versionPS;

			UINT					m_shaderVersion;

			UINT					m_maxVertexShaderConst;

		public:
			CMapTextureFilename		m_mapTextureFilename;
			LPDIRECT3DTEXTURE9		GetTexture(CString filename);
			void					ReleaseTexture(CString filename);

			LPDIRECT3DTEXTURE9		m_pD3DXTextureRibbon;
			LPDIRECT3DTEXTURE9		m_pD3DXTextureRibbonHelix;
			LPDIRECT3DTEXTURE9		m_pD3DXTextureRibbonSheet;
			LPDIRECT3DTEXTURE9		m_pD3DXTextureSelection;

		public:		//	font texture container...
			CFontTextureContainer *	m_pFontTextureContainer;

	END;

	BEGIN("렌더링 옵션");
		public:
		BOOL		m_bEnablePreshader;
		HRESULT		CreateShader();

			void SetShaderLight1Direction(D3DXVECTOR3 &vec) { m_pEffectBasicShading->SetValue(m_f3Light1Direction, &vec, sizeof(D3DXVECTOR3) ); }
			void SetShaderLight2Direction(D3DXVECTOR3 &vec) { m_pEffectBasicShading->SetValue(m_f3Light2Direction, &vec, sizeof(D3DXVECTOR3) ); }

			void SetShaderLight1Intensity(FLOAT intensity) { m_pEffectBasicShading->SetFloat(m_hLight1Intensity, intensity); }
			void SetShaderLight2Intensity(FLOAT intensity) { m_pEffectBasicShading->SetFloat(m_hLight2Intensity, intensity); }

			void SetShaderLight1Color(D3DXCOLOR	color) { m_pEffectBasicShading->SetValue(m_hLight1Color, &color, sizeof(D3DXCOLOR)); }
			void SetShaderLight2Color(D3DXCOLOR	color) { m_pEffectBasicShading->SetValue(m_hLight2Color, &color, sizeof(D3DXCOLOR)); }

			void SetNumActiveLights(int numLight) {m_pEffectBasicShading->SetInt(m_hNumActiveLight, numLight); }

			void SetShaderWorldMatrix(D3DXMATRIXA16 & mat) { m_pEffectBasicShading->SetMatrix(m_f4HadleWorld, &mat); }
			void SetShaderWorldViewMatrix(D3DXMATRIXA16 & mat) { m_pEffectBasicShading->SetMatrix(m_f4HadleWorldView, &mat); }
			void SetShaderWorldViewProjMatrix(D3DXMATRIXA16 & mat) { m_pEffectBasicShading->SetMatrix(m_f4HadleWorldViewProj, &mat); }

			void SetShaderSelectionTexture(LPDIRECT3DTEXTURE9	texture) { m_pEffectBasicShading->SetTexture(m_texShader, texture); }
			void SetShaderFinalSceneTexture(LPDIRECT3DTEXTURE9	texture) { m_pEffectBasicShading->SetTexture(m_texShaderFinalScene, texture); }

			enum {	
					SurfaceRenderingNoAlpha, 
					SurfaceRenderingWithAlpha, 
					WireframeRendering, 
					WireframeRenderingLineWidth, 
					RibbonRendering,
					RibbonRenderingNoTexture,
					ClipPlaneRenderingNoAlpha,
					ClipPlaneRenderingWithAlpha,
					AxisRendering,
					LineWireframeRendering,
					SkyBoxRendering,
					RenderFinalSceneWithBorder,
					SphereRenderingBatch,
					CylinderRenderingBatch,
					Text3DRenderingNoAlpha,
					Text3DRenderingWithAlpha,
					SSAO_SM3,
					Blur4pixel,
					Blur16pixel,
					RenderFinalSceneOneTexture,
					RenderFinalSceneTwoTexture
			};

			void SetShaderTechnique(long technique, int shader = 0);

			void SetShaderDiffuseColor(D3DXCOLOR & color) { m_pEffectBasicShading->SetValue(m_f4MaterialDiffuseColor, &color, sizeof(D3DXCOLOR)); }

			void SetShaderVertexAlpha(float alpha) { m_pEffectBasicShading->SetFloat(m_alphaValue, alpha); }

			void SetShaderIndicateDiffuseColor(D3DXCOLOR & color) { m_pEffectBasicShading->SetValue(m_indicateDiffuseColor, &color, sizeof(D3DXCOLOR)); }

			void SetShaderIndicate(BOOL bIndicate) { m_pEffectBasicShading->SetBool(m_bIndicate, bIndicate); }

			void SetShaderClipPlane0(D3DXPLANE & plane ) { m_pEffectBasicShading->SetValue(m_hClipPlane0, &plane, sizeof(D3DXPLANE)); }
			void SetShaderClipPlane0Dir(float value) { m_pEffectBasicShading->SetFloat(m_hClipPlane0Dir, value); }

			void SetShaderClipPlane1(D3DXPLANE & plane ) { m_pEffectBasicShading->SetValue(m_hClipPlane1, &plane, sizeof(D3DXPLANE)); }
			void SetShaderClipPlane1Dir(float value) { m_pEffectBasicShading->SetFloat(m_hClipPlane1Dir, value); }

			void SetShaderClipPlane2(D3DXPLANE & plane ) { m_pEffectBasicShading->SetValue(m_hClipPlane2, &plane, sizeof(D3DXPLANE)); }
			void SetShaderClipPlane2Dir(float value) { m_pEffectBasicShading->SetFloat(m_hClipPlane2Dir, value); }

			void SetShaderFogParam(FLOAT * vFog) { m_pEffectBasicShading->SetValue(m_hFogParam, vFog, sizeof(FLOAT)*2 ); }

			void SetShaderCameraPosInvWorld(D3DXVECTOR3 &vecPos) { m_pEffectBasicShading->SetValue(m_hCameraPosInvWorld, &vecPos, sizeof(D3DXVECTOR3) ); }
			void SetShaderWireframeLineWidth(FLOAT value) { m_pEffectBasicShading->SetFloat(m_hWireframeLineWidth, value ); }

			void SetShaderUseBackfaceColor(BOOL value) { m_pEffectBasicShading->SetBool(m_hUseBackfaceColor, value); }
			void SetShaderBlendBackface(int value) { m_pEffectBasicShading->SetFloat(m_hBlendBackfaceColor, value/100.0f); }
			void SetShaderBackfaceDiffuseColor(D3DXCOLOR &	color) { m_pEffectBasicShading->SetValue(m_hBackfaceDiffuseColor, &color, sizeof(D3DXCOLOR) ); }

			void SetShaderEyePos(D3DXVECTOR4 &	pos) { m_pEffectBasicShading->SetValue(m_hVecEye, &pos, sizeof(D3DXVECTOR4) ); }

			void SetShaderIntensityAmbient(FLOAT value) { m_pEffectBasicShading->SetFloat(m_hIntensityAmbient, value); }
			void SetShaderIntensityDiffuse(FLOAT value) { m_pEffectBasicShading->SetFloat(m_hIntensityDiffuse, value); }
			void SetShaderIntensitySpecular(FLOAT value) { m_pEffectBasicShading->SetFloat(m_hIntensitySpecular, value); }

			void SetShaderImageDelta(FLOAT * delta) { m_pEffectBasicShading->SetFloatArray(m_hFinalSceneImageDelta, delta , 2 ); }

			void SetBatchInstancePosition (D3DXVECTOR4 * batchPos, LONG count) { m_pEffectBasicShading->SetVectorArray (m_hBatchInstancePosition, batchPos, count); }
			void SetBatchInstanceColor (D3DXCOLOR * batchColor, LONG count) { m_pEffectBasicShading->SetVectorArray (m_hBatchInstanceColor, (D3DXVECTOR4 *)batchColor, count); }
			void SetBatchInstanceSelectionRotationXYScale(D3DXVECTOR4 * batchInstance, LONG count ) { m_pEffectBasicShading->SetVectorArray ( m_hBatchInstanceSelectionRotationXYScale, batchInstance, count); }

			void SetBatchSamplesSSAO (D3DXVECTOR4 * samples, LONG count) { m_pEffectBasicShading->SetVectorArray (m_hBatchInstanceColor, samples, count); }

			float	g_intensiryAmbient;			//	ambient Intensity
			float	g_intensiryDiffuse;			//	diffuse Intensity
			float	g_intensirySpecular;		//	specular Intensity

			ID3DXEffect * m_pEffectBasicShading;       // D3DX effect interface

		private:

			D3DXHANDLE	m_f3Light1Direction;
			D3DXHANDLE	m_f3Light2Direction;

			D3DXHANDLE	m_hLight1Intensity;
			D3DXHANDLE	m_hLight2Intensity;

			D3DXHANDLE	m_hLight1Color;
			D3DXHANDLE	m_hLight2Color;

			D3DXHANDLE	m_f4HadleWorld;
			D3DXHANDLE	m_f4HadleWorldView;
			D3DXHANDLE	m_f4HadleWorldViewProj;

			D3DXHANDLE	m_texShader;					
			D3DXHANDLE	m_texShaderFinalScene;					

			D3DXHANDLE	m_f4MaterialDiffuseColor;      
			D3DXHANDLE	m_alphaValue;

			D3DXHANDLE	m_indicateDiffuseColor;

			D3DXHANDLE	m_bIndicate;

			D3DXHANDLE	m_hClipPlane0;
			D3DXHANDLE	m_hClipPlane0Dir;

			D3DXHANDLE	m_hClipPlane1;
			D3DXHANDLE	m_hClipPlane1Dir;

			D3DXHANDLE	m_hClipPlane2;
			D3DXHANDLE	m_hClipPlane2Dir;

			D3DXHANDLE	m_hFogParam;
			D3DXHANDLE	m_hFogParamSM3;
			D3DXHANDLE	m_hFogColor;

			D3DXHANDLE	m_hCameraPosInvWorld;
			D3DXHANDLE	m_hWireframeLineWidth;

			D3DXHANDLE	m_hUseBackfaceColor;
			D3DXHANDLE	m_hBlendBackfaceColor;
			D3DXHANDLE	m_hBackfaceDiffuseColor;

			D3DXHANDLE	m_hVecEye;

			D3DXHANDLE	m_hIntensityAmbient;
			D3DXHANDLE	m_hIntensityDiffuse;
			D3DXHANDLE	m_hIntensitySpecular;

			D3DXHANDLE	m_hFinalSceneImageDelta;

			D3DXHANDLE	m_hBatchInstancePosition;
			D3DXHANDLE	m_hBatchInstanceColor;
			D3DXHANDLE	m_hBatchInstanceSelectionRotationXYScale;

			D3DXHANDLE	m_hIlluminationModel[2][MAX_RENDERING_TECHNIQUE];

			D3DXHANDLE	m_hColorIndicate;

			//	SSAO
			D3DXHANDLE	m_hSamplesSSAO;
			D3DXHANDLE	m_hNumSampleSSAO;

			D3DXHANDLE	m_hCamFrustumTopLeft;
			D3DXHANDLE	m_hMatProj;

			D3DXHANDLE	m_hTextureDepth;
			D3DXHANDLE	m_hTextureNormal;
			D3DXHANDLE	m_hTextureNoise;
			D3DXHANDLE	m_hTextureColor;

			D3DXHANDLE	m_hSSAOParam;

			D3DXHANDLE	m_hTextureSSAO;
			D3DXHANDLE	m_hBlurTextureSize;

			D3DXHANDLE	m_hFinalTexture1;
			D3DXHANDLE	m_hFinalTexture2;

			D3DXHANDLE  m_hNumActiveLight;
	END;

	BEGIN("CoordinateAxisDisplay");
		public:
			CCoordinateAxisDisplay	m_CoordinateAxisDisplay;
	END;

	BEGIN("화면 스크린 샷 크게 만들기");
		HRESULT RenderLargeScreenShot( LPCSTR fileName, int numTiles );
		HRESULT SaveScreenImage ( LPCSTR fileName, long imageWidth = -1, long imageHeight = -1, D3DXIMAGE_FILEFORMAT format = D3DXIFF_PNG );	//	-1 이면 현재 화면 크기 그대로 무비를 만든다.
	END;

	BEGIN("카메라 이동 애니메이션");
		public:
			BOOL		m_bAnimationing;

			//	화면 animation은 time으로만 적용
			HRESULT		SetCameraAnimation(CAtomInst * pAtom , FLOAT lastingTime );
			HRESULT		SetCameraAnimation(CResidueInst * pResidue, FLOAT lastingTime );
			HRESULT		SetCameraAnimation(CSTLArrayAtomInst &arrayAtom, FLOAT lastingTime );
			HRESULT		SetCameraAnimation(D3DXVECTOR3 &endPos, FLOAT lastingTime );
			HRESULT		SetCameraAnimation();	//	현재 선택된것의 평균으로 이동

			void		SetCameraPos(D3DXVECTOR3 &pos);
			HRESULT		FindAnimationTargetPos( _IN_ CSTLArrayAtomInst &arrayAtom, _OUT_ D3DXVECTOR3 &pos);

			HRESULT		GenerateCameraAnimationPos(D3DXVECTOR3 &endPos, long frame);
			D3DXVECTOR3	GetCameraAnimationPos(int index) { if ( index < m_animationPosArray.size() ) return m_animationPosArray[index]; else return D3DXVECTOR3(0,0,0); }

		private:
			//	구해진 camera animation pos array
			CSTLArrayD3DXVECTOR3	m_animationPosArray;
	END;
	
	BEGIN("Directional Light Widget")
		public:
			CLightWidget m_LightControl[MAX_LIGHTS];
	END;

	BEGIN("Picking")
		private:
			void	PickWireBallStickInSelectionList(CSTLArrayPickedAtomInst & );
			void	PickSurfaceInSelectionList(CSTLArrayPickedAtomInst & );
			void	PickRibbonInSelectionList(CSTLArrayPickedResidueInst & );

		public:
			BOOL	PosClipPlaneClipped(CSelectionDisplay * pSelectionDisplay, D3DXVECTOR3 & pos);
			BOOL	GetSelectLightSource(long &iLight, FLOAT &dist);
			CClipPlane * GetSelectClipPlane(FLOAT &dist);

			void	UpdateSelectionInfoPane();

			//	CProteinObject *	m_pLastPickObject;		//	마지막 picking object 를 저장, tree, residue pane 에 선택부분 표시
			CProteinObjectInst *	m_pLastPickObjectInst;		//	마지막 picking object 를 저장, tree, residue pane 에 선택부분 표시
			
	END;

	BEGIN("Annotaion");
		public:
			BOOL					m_bDisplayAnnotation;		// Annotation Display

			CPickAtomInst 			m_SelectAtom;				// 현재 선택한 Atom
	END;

	BEGIN("Rendering 설정");
		public:
			//	렌더링 property.
			CPropertyScene *		m_pPropertyScene;

			void	SetModelQuality();
			void	SetShaderQuality();
			void	SetShowSelectionMark();
			void	SetSelectionColor();

			void	SetFogEnable(BOOL bForceEnable = TRUE);
			void	SetFog();
			void	SetAntialiasing();
			void	SetShaderLight();
			void	SetGlobalClipPlane();
	
			void	SetDisplayHETATM();
	END;

	BEGIN("Global Clip Plane");
		public:
			CClipPlane *		m_pClipPlane;
	END;

	BEGIN("Surface Generation");
		long	m_surfaceGenAlgoritm;			//	0 is MQ, 1 is MSMS
		long	m_surfaceBiounitGenAlgoritm;
	END;

	BEGIN("MultiSelect");
		public:
			BOOL			SelectAtom(POINT pt);
	END;

	BEGIN("PDB 선택");
		public:
			void	DeSelectAllAtoms();			//	CPDB, 화면, pane 전부 deselect 한다. m_bSelect = TRUE 하기 전에 전부 deselect 하기 위해 불러준다.
			void	SelectedAtomApply();			//	m_bSelect = TRUE 하고 나서 화면에 전부 update 하기위해 불러준다.

			void	SelectAll(BOOL bSelect);	//	CPDB 에 있는 m_bSelect flag를 모두 select/deselect 한다.
			void	SelectChildren(CProteinObjectInst * pObject, BOOL bSelect);
			void	SelectChildrenRecursive(CProteinObjectInst * pObject, BOOL bSelect);		//	pObject로 넘어온 pdb, chain, atom 의 하위노드를 전부 select 한다.

			void	GetSelectedObject(CSTLArraySelectionInst &selection);

			void	SelectSpecificAtoms( int conditionID );
	END;

	BEGIN("표면 선택 표시");
		public:
			void	UpdateAtomSelectionChanged();	//	선택된 것이 변경되었을때, Instancing Rendering data의 색깔을 바꾸어준다.
			void	SetAtomSelectionAddRemoved();		//	AtomSelectionList 가 Add 되거나 Remove 되었을때 callback으로 불린다.
	END;

	BEGIN("화면선택 개별 렌더링");
		public:
			CSTLArraySelectionDisplay		m_arraySelectionDisplay;		// reserve(320)이 되어있고 index에 값에 들어있다.
			long							m_maxIndexSelectionDisplay;

			CSelectionDisplay * GetCurrentSelectionDisplay();

			long	GetNewIndexSelectionDisplay();		//	새로운 index를 하나 꺼내온다.
			long	GetMaxIndexSelectionDisplay();		//	

			//	mode로 현재의 selection 된것을 selectionlist에 넣는다.
			CSelectionDisplay * AddCurrentSelection(long mode, CPDBRenderer * pPDBRendererAddSelection = NULL );

			//	selection 된것을 selectionList 에 넣는다.
			long		AddInitialSelection(CPDBRenderer * pPDBRenderer, long mode);

			//
			//	proteinA 를 proteinB 로 mat 만큼 transform 시킨다.
			//	ProteinB 는 움직이지 않고 고정. proteinA 를 움직이는것임.
			//	mat 는 PDB 안에 들어있는 좌표로 계산된 값이다.
			//	
			HRESULT TransformAToB(CSelectionDisplay* proteinA, CSelectionDisplay* proteinB, D3DXMATRIXA16 &mat);
			HRESULT TransformAToB(CPDBRenderer * pPDBRenderer1, CPDBRenderer * pPDBRenderer2, D3DXMATRIXA16 &matTransform, D3DXVECTOR3 *pCenter = NULL);
	END;

	BEGIN("화면 선택된것 Que에 넣기");
		public:
			//	CSelectionQue	m_selectionQue;
	END;

	BEGIN("컬러스킴");
		public:
			CColorSchemeDefault	m_colorSchemeDefault;
	END;

	BEGIN("Surface sharing");
		CSTLArrayProteinSurfaceBase		m_arrayProteinSurface;
	END;

	BEGIN("Ribbon Vertex sharing");
		CSTLArrayCProteinRibbonVertexData		m_arrayProteinRibbonVertexData;
	END;

	BEGIN("Final Render Selection color");
		//	0 is no selection
		//	1 is default selection
		//	2.. MAX_SELECTION_COLOR-1 : custom selection
		D3DXCOLOR	m_finalRenderSelectionColor[MAX_SELECTION_COLOR];
		BOOL		m_bSelectionColorExist[MAX_SELECTION_COLOR];

		void		ApplyColorIndicateTable();

		//	return number: selection color index;
		//	return -1 : full.
		long	GetIndicateColorSlot();
		BOOL 	SetIndicateColorSlot(int index, D3DXCOLOR color);
		BOOL	DeleteIndicateColorSlot(long index);
		void	InitIndicateColorSlot();

	END;

	BEGIN("Final Render");
		SCREEN_VERTEX m_svQuad[4];

		//	Backface rendering.
		PDIRECT3DSURFACE9           m_pSurfaceMultiSampleFinalRenderTarget;

		LPDIRECT3DTEXTURE9			m_pTexFinalRenderTarget;
		PDIRECT3DSURFACE9           m_pSurfaceMultiSampleRenderTargetColor;       // Multi-Sample float render target
		PDIRECT3DSURFACE9           m_pSurfaceMultiSampleDepthStencil;       // Depth Stencil surface for the float RT

		D3DMULTISAMPLE_TYPE			m_typeMultiSample;
		void					DrawFullScreenQuad( float fLeftU=0.0f, float fTopV=0.0f, float fRightU=1.0f, float fBottomV=1.0f );
	END;

	BEGIN("SSAO");
		BOOL		m_bCapableSSAO;
		BOOL		m_bSSAOBlur;

		int			m_numMaxRTS;

		int			m_ssaoTextureSizeWidth;
		int			m_ssaoTextureSizeHeight;

		D3DXVECTOR4	m_samplesSSAO[m_maxSamplesSSAO];

		LPDIRECT3DTEXTURE9          m_pTexNoise;  
		LPDIRECT3DTEXTURE9          m_pTexRenderColor;
		LPDIRECT3DTEXTURE9          m_pTexRenderNormal;
		LPDIRECT3DTEXTURE9          m_pTexRenderDepth;
		LPDIRECT3DTEXTURE9          m_pTexRenderSSAO;
		LPDIRECT3DTEXTURE9          m_pTexRenderBlur;
	END;
};


