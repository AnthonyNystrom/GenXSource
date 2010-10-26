//-----------------------------------------------------------------------------
// File: ProteinVistaRenderer.cpp
//-----------------------------------------------------------------------------
#include "stdafx.h"
#include "ProteinVista.h"
#include "ProteinVistaView.h"

#include "PDBRenderer.h"
#include "ProteinVistaRenderer.h"
#include "SkyBox.h"
#include "RenderProperty.h"
#include "SelectionDisplay.h"
#include "Interface.h"
#include "RenderProperty.h"
#include "Utility.h"
#include "DrawText3D.h"
#include "ClipPlane.h"
#include "ProteinSurfaceMSMS.h"
#include "RibbonVertexData.h"
//#include "ProteinViewPanel.h"
#include "ResiduePane.h"
#include "PDBTreePane.h"
#include <map>

#include "SelectionListPane.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif



//	for debugging...
//	In normal case, comment next line.
//	#define FORCE_SHADER_VERSION		2

const DWORD                 SCREEN_VERTEX::FVF = D3DFVF_XYZRHW | D3DFVF_TEX4;
#pragma managed(push,off)
//-----------------------------------------------------------------------------
// Name: CProteinVistaRenderer()
// Desc: Application constructor.   Paired with ~CProteinVistaRenderer()
//       Member variables should be initialized to a known state here.  
//       The application window has not yet been created and no Direct3D device 
//       has been created, so any initialization that depends on a window or 
//       Direct3D should be deferred to a later stage. 
//-----------------------------------------------------------------------------
CProteinVistaRenderer::CProteinVistaRenderer(CProteinVistaView * pProteinVistaView)
					: m_FromVec(0,0,0), m_ToVec(0,0,0)
{
    m_dwCreationWidth           = 500;
    m_dwCreationHeight          = 375;
    m_strWindowTitle            = TEXT( "ProteinVistaRenderer" );
    m_d3dEnumeration.AppUsesDepthBuffer   = TRUE;
	m_bStartFullscreen			= false;
	m_bShowCursorWhenFullscreen	= false;

	m_pFont                     = new CD3DFont( _T("Arial"), 12, D3DFONT_BOLD );
    m_bLoadingApp               = TRUE;

    ZeroMemory( &m_UserInput, sizeof(m_UserInput) );

	m_pProteinVistaView = pProteinVistaView;

	m_pDeclAtomSphere = NULL;
	m_pDeclBond = NULL;
	m_pDeclLine = NULL;

	m_bEnablePreshader = TRUE;

	m_pEffectBasicShading = NULL;

	m_maxBondVertex = 24;

	m_fovY = m_fAspectRatio = 0.0f;

	m_nSphereSlices[0] = 24;	
	m_nSphereSlices[1] = 20;
	m_nSphereSlices[2] = 16;

	m_nSphereStacks[0] = 24;
	m_nSphereStacks[1] = 20;
	m_nSphereStacks[2] = 16;

	m_sphereRadius[0] =	1.0f;
	m_sphereRadius[1] = 0.5f;
	m_sphereRadius[2] = 0.25f;
	
	ZeroMemory(m_pMeshAtomVertexBuffer, sizeof(IDirect3DVertexBuffer9 *)*3);
	ZeroMemory(m_pMeshAtomIndexBuffer, sizeof(IDirect3DIndexBuffer9 * )*3);

	m_pD3DXMeshBond = NULL;
	
	m_pD3DXTextureRibbon = NULL;
	m_pD3DXTextureRibbonHelix = NULL;
	m_pD3DXTextureRibbonSheet = NULL;

	m_pSkyManager = new CSkyManager(this);

    m_FromVec = D3DXVECTOR3(0,0,-1);
	m_ToVec = D3DXVECTOR3(0,0,0);
	m_cameraZPos = -50.0f;

	m_bCameraDrag = FALSE;
	m_posOldX = m_posOldY = -1000;

	D3DXMatrixIdentity(&m_matCameraRotation);
	D3DXMatrixIdentity(&m_matCameraRotationTemp);

	D3DXMatrixIdentity(&m_matrixView);
	D3DXMatrixIdentity(&m_matrixProj);

	m_pLastPickObjectInst = NULL;

	//	light
	for( int i=0; i<MAX_LIGHTS; i++ )
		m_LightControl[i].SetLightDirection( D3DXVECTOR3( sinf(D3DX_PI*2*i/MAX_LIGHTS-D3DX_PI/6), 0, -cosf(D3DX_PI*2*i/MAX_LIGHTS-D3DX_PI/6) ) );
	//	m_LightControl[i].SetLightDirection( D3DXVECTOR3(0,0,-1));

	m_arraySelectionDisplay.resize(MAX_DISPLAY_SELECTION_INDEX);
	
	m_f3Light1Direction = m_f3Light2Direction = NULL;
	m_hLight1Intensity = m_hLight2Intensity = NULL;
	m_hLight1Color = m_hLight2Color = NULL;

	m_f4HadleWorld = m_f4HadleWorldView = m_f4HadleWorldViewProj = NULL;
	m_texShader = m_texShaderFinalScene = NULL;
	m_f4MaterialDiffuseColor = NULL;
	m_alphaValue = NULL;

	m_indicateDiffuseColor = NULL;

	m_bIndicate = NULL;
	m_hClipPlane0 = NULL;
	m_hClipPlane0Dir = NULL;

	m_hClipPlane1 = NULL;
	m_hClipPlane1Dir = NULL;

	m_hClipPlane2 = NULL;
	m_hClipPlane2Dir = NULL;

	m_hFogParam = NULL;
	m_hFogParamSM3 = NULL;
	m_hFogColor = NULL;

	m_hUseBackfaceColor = NULL;
	m_hBlendBackfaceColor = NULL;
	m_hBackfaceDiffuseColor = NULL;

	m_hCameraPosInvWorld = NULL;
	m_hWireframeLineWidth = NULL;

	m_hVecEye = NULL;
     
	m_pPropertyScene = new CPropertyScene;
	m_pPropertyScene->InitProperty();
 
	m_maxIndexSelectionDisplay = 0;
	m_pActivePDBRenderer = NULL;

	m_pTexFinalRenderTarget = NULL;
	m_typeMultiSample = D3DMULTISAMPLE_NONE;
	m_pSurfaceMultiSampleRenderTargetColor = NULL;
	m_pSurfaceMultiSampleDepthStencil = NULL;
 
	m_maxVertexIndex = 0;
	m_formatIndexBuffer = D3DFMT_INDEX32;
	m_byteSizeIndexBuffer = sizeof(DWORD);
	 
	m_pFontTextureContainer = new CFontTextureContainer(this);
 
	m_pClipPlane = new CClipPlane;

	m_pTexNoise = m_pTexRenderColor = m_pTexRenderNormal = m_pTexRenderDepth = m_pTexRenderSSAO = m_pTexRenderBlur = NULL;
	m_ssaoTextureSizeWidth = 800;
	m_ssaoTextureSizeHeight = 600;

	//	
	m_bCapableSSAO = TRUE;

	m_numMaxRTS = 0;
	m_versionVS = 0;
	m_versionPS = 0;
	m_shaderVersion = 0;

	m_bAnimationing = FALSE;

	m_maxVertexShaderConst = 0;

	m_surfaceGenAlgoritm = 1;		//	default msms
	m_surfaceBiounitGenAlgoritm = 0;	//	default MQ

	m_fNearClipPlane = 1.0f;
	m_fFarClipPlane = 300.0f;

	m_radius = 10.0f;
	m_center = D3DXVECTOR3(0,0,0);
	m_lightRadius = 0.0f;

	CSelectionDisplay::m_maxSelectionIndex = 0;
 
	InitIndicateColorSlot();

	//	fill ssao sample
	srand( (unsigned)time( NULL ) );

	for ( int i = 0 ; i < m_maxSamplesSSAO ; i++ )
	{
		int range_max = 10000L;
		int	range_min = -10000L;

		int randNum1 = (double)rand() / (RAND_MAX + 1) * (range_max - range_min) + range_min;
		int randNum2 = (double)rand() / (RAND_MAX + 1) * (range_max - range_min) + range_min;
		int randNum3 = (double)rand() / (RAND_MAX + 1) * (range_max - range_min) + range_min;

		D3DXVECTOR4 vec;
		vec.x = randNum1;
		vec.y = randNum2;
		vec.z = randNum3;
		vec.w = 0.0f;

		D3DXVec4Normalize(&vec, &vec);

		m_samplesSSAO[i] = vec;
	}
	
    
}
#pragma managed(pop)
//-----------------------------------------------------------------------------
// Name: ~CProteinVistaRenderer()
// Desc: Application destructor.  Paired with CProteinVistaRenderer()
//-----------------------------------------------------------------------------
CProteinVistaRenderer::~CProteinVistaRenderer()
{
	// Cleanup D3D font
	SAFE_DELETE(m_pPropertyScene);
	SAFE_DELETE(m_pFontTextureContainer);
	SAFE_DELETE(m_pClipPlane);
}

//-----------------------------------------------------------------------------
// Name: OneTimeSceneInit()
// Desc: Paired with FinalCleanup().
//       The window has been created and the IDirect3D9 interface has been
//       created, but the device has not been created yet.  Here you can
//       perform application-related initialization and cleanup that does
//       not depend on a device.
//-----------------------------------------------------------------------------
HRESULT CProteinVistaRenderer::OneTimeSceneInit()
{
    // perform one time initialization

    m_bLoadingApp = FALSE;

	// Add your specialized creation code here
    //CString strFilename = m_pProteinVistaView->GetDocument()->GetPathName();
	
	for ( int i = 0 ; i < m_arrayPDBRenderer.size(); i++ )
	{
		m_arrayPDBRenderer[i]->OneTimeSceneInit();
	}

	m_pSkyManager->OneTimeSceneInit();

    return S_OK;
}


//-----------------------------------------------------------------------------
// Name: ConfirmDevice()
// Desc: Called during device initialization, this code checks the display device
//       for some minimum set of capabilities
//-----------------------------------------------------------------------------
HRESULT CProteinVistaRenderer::ConfirmDevice( D3DCAPS9* pCaps, DWORD dwBehavior, D3DFORMAT adapterFormat , D3DFORMAT backBufferFormat)
{
    UNREFERENCED_PARAMETER( adapterFormat );
	UNREFERENCED_PARAMETER( backBufferFormat );
    UNREFERENCED_PARAMETER( dwBehavior );
    UNREFERENCED_PARAMETER( pCaps );

	//	GetTransform doesn't work on PUREDEVICE
	//	if( dwBehavior & D3DCREATE_PUREDEVICE != NULL )	return E_FAIL;

	if( pCaps->PixelShaderVersion < D3DPS_VERSION(2,0) )
		return E_FAIL;

	// If device doesn't support 1.1 vertex shaders in HW, switch to SWVP.
	if( pCaps->VertexShaderVersion < D3DVS_VERSION(2,0) )
	{
		if( (dwBehavior & D3DCREATE_SOFTWARE_VERTEXPROCESSING ) == 0 )
			return E_FAIL;
	}

	// Skip backbuffer formats that don't support alpha blending
	if ( FAILED( m_pD3D->CheckDeviceFormat( pCaps->AdapterOrdinal, pCaps->DeviceType, adapterFormat, D3DUSAGE_QUERY_POSTPIXELSHADER_BLENDING, D3DRTYPE_TEXTURE, backBufferFormat ) ) ) 
			return E_FAIL;

	return S_OK;
}

//-----------------------------------------------------------------------------
// Name: InitDeviceObjects()
// Desc: Paired with DeleteDeviceObjects()
//       The device has been created.  Resources that are not lost on
//       Reset() can be created here -- resources in D3DPOOL_MANAGED,
//       D3DPOOL_SCRATCH, or D3DPOOL_SYSTEMMEM.  Image surfaces created via
//       CreateOffScreenPlainSurface are never lost and can be created here.  Vertex
//       shaders and pixel shaders can also be created here as they are not
//       lost on Reset().
//-----------------------------------------------------------------------------
HRESULT CProteinVistaRenderer::InitDeviceObjects()
{
    // create device objects
    HRESULT hr;

	//	Check shader version
	{
		D3DCAPS9 caps9;
		HRESULT hr = m_pd3dDevice->GetDeviceCaps(&caps9);
		if ( FAILED(hr) )
		{
			//AfxMessageBox(_T("Cannot find hardware rasterization device. Please check your graphics card"));
			OutputTextMsg(_T("Cannot find hardware rasterization device. Please check your graphics card"));
			return E_FAIL;
		}

		//	shader version check
		BYTE vsVer = HIBYTE(caps9.VertexShaderVersion);
		if ( vsVer < 2 )
		{
			CString msg;
			msg.Format(_T("This program needs vertex shader 2.0 or more higher version. This system has vertex shader %d version. Program is terminated."), vsVer);
			OutputTextMsg(msg);
			return E_FAIL;
		}
		else
		{
			CString msg;
			msg.Format(_T("This system supports vertex shader %d"), vsVer);
			OutputTextMsg(msg);
		}

		m_versionVS = vsVer;

		m_maxVertexShaderConst = caps9.MaxVertexShaderConst;

		if ( caps9.MaxVertexShaderConst < 256 )
		{
			CString msg;
			msg.Format(_T("This program needs at least 255(0xff) Shader Constant Register. This system has %d constant register"), caps9.MaxVertexShaderConst );
			OutputTextMsg(msg);
			return E_FAIL;
		}
		else
		{
			CString msg;
			msg.Format(_T("This system has %d shader constant register"), caps9.MaxVertexShaderConst);
			OutputTextMsg(msg);
		}

		BYTE psVer = HIBYTE(caps9.PixelShaderVersion);
		if ( psVer < 2 )
		{
			CString msg;
			msg.Format(_T("This program needs pixel shader 2.0 or more higher version. This system has pixel shader %d version. Program is terminated."), psVer);
			OutputTextMsg(msg);
			return E_FAIL;
		}
		else
		{
			CString msg;
			msg.Format(_T("This system supports pixel shader %d"), psVer);
			OutputTextMsg(msg);
		}

		m_versionPS = psVer;

#ifndef FORCE_SHADER_VERSION
		m_shaderVersion = min(vsVer, psVer );
#else
		m_shaderVersion = FORCE_SHADER_VERSION;
#endif

		//	render targer의 갯수
		m_numMaxRTS = caps9.NumSimultaneousRTs;

		//	Check two caps.
		m_bCapableSSAO = TRUE;

		if ( m_shaderVersion < 3 )
			m_bCapableSSAO = FALSE;

		if ( m_numMaxRTS < 3 )	
			m_bCapableSSAO = FALSE;

		if ( (caps9.PrimitiveMiscCaps & D3DPMISCCAPS_MRTINDEPENDENTBITDEPTHS) == 0 )
			m_bCapableSSAO = FALSE;

		if ( (caps9.PrimitiveMiscCaps & D3DPMISCCAPS_MRTPOSTPIXELSHADERBLENDING) == 0 )
			m_bCapableSSAO = FALSE;

		if ( m_pPropertyScene->m_iAntialiasing != CPropertyScene::AA_NONE )
			m_bCapableSSAO = FALSE;

		if ( m_bCapableSSAO == TRUE  )
		{
			//	ssao의 설정을 변경할수 있게 만듬.
			//m_pPropertyScene->m_pItembUseSSAO->SetReadOnly(FALSE);
			//m_pPropertyScene->m_pItembUseSSAO->SetExpandable(TRUE);
		}
		else
		{	//	false.
			m_pPropertyScene->m_bUseSSAO = FALSE;
			//m_pPropertyScene->m_pItembUseSSAO->SetBool(m_pPropertyScene->m_bUseSSAO);
			//m_pPropertyScene->m_pItembUseSSAO->SetReadOnly(TRUE);
			//m_pPropertyScene->m_pItembUseSSAO->SetExpandable(FALSE);
		}
		m_maxVertexIndex = caps9.MaxVertexIndex;

		if ( m_maxVertexIndex <= 0xffff )
		{
			m_formatIndexBuffer = D3DFMT_INDEX16;
			m_byteSizeIndexBuffer = sizeof(WORD);

			CString msg;
			msg.Format(_T("This system has maximum %ld(%x) vertex index. It's insufficient to render large molecule."), caps9.MaxVertexIndex, caps9.MaxVertexIndex );
			OutputTextMsg(msg);
		}
		else
		{
			CString msg;
			msg.Format(_T("This system has maximum %ld(%x) vertex index"), caps9.MaxVertexIndex, caps9.MaxVertexIndex );
			OutputTextMsg(msg);
		}

		CString msg;
		msg.Format( _T("Vertex shader version of your graphics card is %d"), vsVer );	OutputTextMsg(msg);
		msg.Format( _T("Pixel shader version of your graphics card is %d"), psVer );	OutputTextMsg(msg);
		msg.Format( _T("The number of render target of your graphics card is %d"), caps9.NumSimultaneousRTs );	OutputTextMsg(msg);
		if ( m_bCapableSSAO == TRUE )
		{
			msg.Format( _T("Your graphics card can support ambient occlusion function"));	
			OutputTextMsg(msg);
		}
		else
		{
			msg.Format( _T("Your graphics card can NOT support ambient occlusion function"));	
			OutputTextMsg(msg);
		}
	}
 
	//    default texture preload.
	CPropertyRibbon propertyRibbon(NULL);	//	값을 얻기 위해 임시로 만든다.
	 
	GetTexture(propertyRibbon.m_strTextureFilenameCoil);
	GetTexture(propertyRibbon.m_strTextureFilenameHelix);
	GetTexture(propertyRibbon.m_strTextureFilenameSheet);
	GetTexture(m_pSkyManager->m_strTextureFilename);
 
	if ( m_bCapableSSAO == TRUE )
	{
		CString strNoiseTextureFilename = GetMainApp()->m_strBaseTexturePath + _T("noise.dds");
		hr = D3DXCreateTextureFromFileEx( m_pd3dDevice, strNoiseTextureFilename, D3DX_DEFAULT, D3DX_DEFAULT,
										 D3DX_DEFAULT, 0, D3DFMT_UNKNOWN, D3DPOOL_MANAGED,
										 D3DX_DEFAULT, D3DX_DEFAULT, 0,
										 NULL, NULL, &m_pTexNoise );
		if( FAILED( hr ) )
			return DXTRACE_ERR( "Cannot find noise.dds(D3DXCreateTextureFromFileEx)", hr );
	}

	//	
    // Init the font
	hr = m_pFont->InitDeviceObjects( m_pd3dDevice );
	if( FAILED( hr ) )
		return DXTRACE_ERR( "m_pFont->InitDeviceObjects", hr );

	//	IndicateColor 를 reset
	InitIndicateColorSlot();

	//
	//	create Atom mesh
	//
	MakeAtomSphereMesh();

	//	create atom bond mesh
	MakeAtomBondMesh();

	//	make shader.
	hr = CreateShader();
	if ( FAILED(hr) )
		 return E_FAIL;

	for ( long i = 0 ; i < m_arrayPDBRenderer.size(); i++ )
	{
		m_arrayPDBRenderer[i]->InitDeviceObjects();
	}
 
	CHTMLListCtrl * phtmlListCtrl = GetMainActiveView()->GetSelectList();
	for ( int i = 0 ; i < phtmlListCtrl->GetItemCount() ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *)(phtmlListCtrl->GetItemData(i));
		if ( pSelectionDisplay )
		{
			pSelectionDisplay->InitDeviceObjects();
		}
	}
	//	
	 m_pSkyManager->InitDeviceObjects();

	//	light
	V( CDXUTDirectionWidget::StaticOnCreateDevice( m_pd3dDevice ) );
	 m_CoordinateAxisDisplay.Init(this);
	 m_CoordinateAxisDisplay.InitDeviceObjects();
 
	m_pClipPlane->Init(this, NULL, 20);
	 m_pClipPlane->InitDeviceObjects();
 
	SetFog();
	SetShaderLight();
	SetGlobalClipPlane();
	SetDisplayHETATM();
	SetSelectionColor();

	m_pFontTextureContainer->InitDeviceObjects();

    return S_OK;
}

HRESULT CProteinVistaRenderer::MakeAtomSphereMesh()
{
	HRESULT hr;

	for ( int i = 0 ; i < 3 ; i++ )
	{
		SAFE_RELEASE(m_pMeshAtomVertexBuffer[i]);
		SAFE_RELEASE(m_pMeshAtomIndexBuffer[i]);
	}

	SAFE_RELEASE(m_pDeclAtomSphere);

	//	vertex declation...
	D3DVERTEXELEMENT9	decl[MAX_FVF_DECL_SIZE];
	D3DXDeclaratorFromFVF( fvfAtomSphereVertex::FVF, decl);
	m_pd3dDevice->CreateVertexDeclaration(decl, &m_pDeclAtomSphere);

	for ( int i = 0 ; i < 3 ; i++ )
	{
		LPD3DXMESH	pAtomMesh = NULL;
		hr = D3DXCreateSphere(m_pd3dDevice, m_sphereRadius[i], m_nSphereSlices[i], m_nSphereStacks[i], &pAtomMesh , NULL);
		m_nAtomVertices[i] = pAtomMesh->GetNumVertices();
		m_nAtomFaces[i] = pAtomMesh->GetNumFaces();

		hr = pAtomMesh->GetVertexBuffer(&m_pMeshAtomVertexBuffer[i]);
		hr = pAtomMesh->GetIndexBuffer(&m_pMeshAtomIndexBuffer[i]);

		SAFE_RELEASE(pAtomMesh);
	}

	return S_OK;
}

HRESULT CProteinVistaRenderer::MakeAtomBondMesh()
{
	HRESULT hr;

	SAFE_RELEASE(m_pD3DXMeshBond);
	SAFE_RELEASE(m_pDeclBond);

	//	D3DXCreateCylinder를 사용하지 않는다. 뚜껑은 그릴필요 없는데. cylinder 는 뚜껑을 그린다.(많이 느려진다)

	//	bond 만들기.
	long	nFace = m_maxBondVertex;	//	vertex == face
	long	nVertex = nFace;
	ID3DXMesh *  pD3DXMeshBond;
	D3DXCreateMeshFVF(nFace, nVertex , D3DXMESH_MANAGED, fvfBondVertex::FVF, m_pd3dDevice, &pD3DXMeshBond);

	fvfBondVertex	* pvertex;
	pD3DXMeshBond->LockVertexBuffer(0, (LPVOID*) &pvertex);
	for ( long i = 0; i < nVertex/2 ; i++ )
	{
		FLOAT theta = (2*D3DX_PI*i*2)/(nFace-1);

		pvertex[2*i+0].FvfVertex = D3DXVECTOR3( sinf(theta)*0.25f, cosf(theta)*0.25f, 0.0f );
		pvertex[2*i+1].FvfVertex = D3DXVECTOR3( sinf(theta)*0.25f, cosf(theta)*0.25f, 1.0f );

	}

	pD3DXMeshBond->UnlockVertexBuffer();

	WORD* pIndex;
	pD3DXMeshBond->LockIndexBuffer(0, (LPVOID*) &pIndex);
	for ( i =0 ; i < nFace ; i++ )
	{
		if ( i%2 == 0 )
		{
			*(pIndex+i*3)= i%nFace;
			*(pIndex+i*3+1)= (i+1)%nFace;
			*(pIndex+i*3+2)= (i+1+2)%nFace;
		}
		else
		{
			*(pIndex+i*3)= (i-1)%nFace;
			*(pIndex+i*3+1)= (i+3-1)%nFace;
			*(pIndex+i*3+2)= (i+2-1)%nFace;
		}
	}
	pD3DXMeshBond->UnlockIndexBuffer();
	D3DXComputeNormals(pD3DXMeshBond, NULL );

	m_pD3DXMeshBond = pD3DXMeshBond;

	return S_OK;
}

HRESULT CProteinVistaRenderer::CreateShader()
{
	HRESULT hr;

	// Define DEBUG_VS and/or DEBUG_PS to debug vertex and/or pixel shaders with the 
	// shader debugger. Debugging vertex shaders requires either REF or software vertex 
	// processing, and debugging pixel shaders requires REF.  The 
	// D3DXSHADER_FORCE_*_SOFTWARE_NOOPT flag improves the debug experience in the 
	// shader debugger.  It enables source level debugging, prevents instruction 
	// reordering, prevents dead code elimination, and forces the compiler to compile 
	// against the next higher available software target, which ensures that the 
	// unoptimized shaders do not exceed the shader model limitations.  Setting these 
	// flags will cause slower rendering since the shaders will be unoptimized and 
	// forced into software.  See the DirectX documentation for more information about 
	// using the shader debugger.
	DWORD dwShaderFlags = D3DXFX_NOT_CLONEABLE;

#if defined( DEBUG ) || defined( _DEBUG )
	// Set the D3DXSHADER_DEBUG flag to embed debug information in the shaders.
	// Setting this flag improves the shader debugging experience, but still allows 
	// the shaders to be optimized and to run exactly the way they will run in 
	// the release configuration of this program.
	dwShaderFlags |= D3DXSHADER_DEBUG;
#endif

#ifdef DEBUG_VS
	dwShaderFlags |= D3DXSHADER_FORCE_VS_SOFTWARE_NOOPT;
#endif
#ifdef DEBUG_PS
	dwShaderFlags |= D3DXSHADER_FORCE_PS_SOFTWARE_NOOPT;
#endif

	// Preshaders are parts of the shader that the effect system pulls out of the 
	// shader and runs on the host CPU. They should be used if you are GPU limited. 
	// The D3DXSHADER_NO_PRESHADER flag disables preshaders.
	if( !m_bEnablePreshader )
		dwShaderFlags |= D3DXSHADER_NO_PRESHADER;

	if ( m_shaderVersion < 3 )
	{
		CString shaderPath = GetMainApp()->m_strBaseResPath + _T("Basiclllumination.fxo");
		 
		// If this fails, there should be debug output as to 
		// why the .fx file failed to compile
		V( D3DXCreateEffectFromFile( m_pd3dDevice, shaderPath , NULL, NULL, dwShaderFlags, NULL, &m_pEffectBasicShading, NULL ) );
		if ( hr != S_OK )
		{
			OutputTextMsg(_T("Cannot find Basiclllumination.fxo. Application terminated."));
			return E_FAIL;
		}
	}
	else
	{
		CString shaderPath = GetMainApp()->m_strBaseResPath + _T("BasicIlluminationShader3.fxo");

		// If this fails, there should be debug output as to 
		// why the .fx file failed to compile
		V( D3DXCreateEffectFromFile( m_pd3dDevice, shaderPath , NULL, NULL, dwShaderFlags, NULL, &m_pEffectBasicShading, NULL ) );
		if ( hr != S_OK )
		{
			OutputTextMsg(_T("Cannot find BasicllluminationShader3.fxo. Application terminated."));
			return E_FAIL;
		}
	}

	//
	m_f3Light1Direction = m_pEffectBasicShading->GetParameterByName(NULL, "g_Light1Dir");
	m_f3Light2Direction = m_pEffectBasicShading->GetParameterByName(NULL, "g_Light2Dir");

	m_hLight1Intensity = m_pEffectBasicShading->GetParameterByName(NULL, "g_Light1Intensity");
	m_hLight2Intensity = m_pEffectBasicShading->GetParameterByName(NULL, "g_Light2Intensity");

	m_hLight1Color = m_pEffectBasicShading->GetParameterByName(NULL, "g_Light1Color");
	m_hLight2Color = m_pEffectBasicShading->GetParameterByName(NULL, "g_Light2Color");

	m_f4HadleWorld = m_pEffectBasicShading->GetParameterByName(NULL, "g_mWorld");
	m_f4HadleWorldView = m_pEffectBasicShading->GetParameterByName(NULL, "g_mWorldView");
	m_f4HadleWorldViewProj = m_pEffectBasicShading->GetParameterByName(NULL, "g_mWorldViewProjection");

	m_texShader = m_pEffectBasicShading->GetParameterByName(NULL, "g_Texture");
	m_texShaderFinalScene = m_pEffectBasicShading->GetParameterByName(NULL, "g_MeshBackground");

	m_f4MaterialDiffuseColor = m_pEffectBasicShading->GetParameterByName(NULL, "g_MaterialDiffuseColor");

	m_alphaValue = m_pEffectBasicShading->GetParameterByName(NULL, "g_alpha");

	m_indicateDiffuseColor = m_pEffectBasicShading->GetParameterByName(NULL, "g_indicateDiffuseColor");

	m_bIndicate = m_pEffectBasicShading->GetParameterByName(NULL, "g_bIndicate");

	m_hClipPlane0 = m_pEffectBasicShading->GetParameterByName(NULL, "g_clipPlane0");
	m_hClipPlane0Dir = m_pEffectBasicShading->GetParameterByName(NULL, "g_clipPlane0Dir");

	m_hClipPlane1 = m_pEffectBasicShading->GetParameterByName(NULL, "g_clipPlane1");
	m_hClipPlane1Dir = m_pEffectBasicShading->GetParameterByName(NULL, "g_clipPlane1Dir");	

	m_hClipPlane2 = m_pEffectBasicShading->GetParameterByName(NULL, "g_clipPlane2");
	m_hClipPlane2Dir = m_pEffectBasicShading->GetParameterByName(NULL, "g_clipPlane2Dir");

	m_hFogParam = m_pEffectBasicShading->GetParameterByName(NULL, "g_vFog");
	m_hFogParamSM3 = m_pEffectBasicShading->GetParameterByName(NULL, "g_fogParam");
	m_hFogColor = m_pEffectBasicShading->GetParameterByName(NULL, "g_fogColor");

	m_hCameraPosInvWorld = m_pEffectBasicShading->GetParameterByName(NULL, "g_cameraPosInvWorld");
	m_hWireframeLineWidth = m_pEffectBasicShading->GetParameterByName(NULL, "g_wireframeLineWidth");

	m_hUseBackfaceColor = m_pEffectBasicShading->GetParameterByName(NULL, "g_bBackfaceColor");
	m_hBlendBackfaceColor = m_pEffectBasicShading->GetParameterByName(NULL, "g_bBlendBackfaceColor");
	m_hBackfaceDiffuseColor = m_pEffectBasicShading->GetParameterByName(NULL, "g_backfaceDiffuseColor");

	m_hVecEye = m_pEffectBasicShading->GetParameterByName(NULL, "g_vecEye");

	m_hIntensityAmbient = m_pEffectBasicShading->GetParameterByName(NULL, "g_intensityAmbient");
	m_hIntensityDiffuse = m_pEffectBasicShading->GetParameterByName(NULL, "g_intensityDiffuse");
	m_hIntensitySpecular = m_pEffectBasicShading->GetParameterByName(NULL, "g_intensitySpecular");

	m_hBatchInstancePosition = m_pEffectBasicShading->GetParameterByName(NULL, "g_vBatchInstancePosition");
	m_hBatchInstanceColor = m_pEffectBasicShading->GetParameterByName(NULL, "g_vBatchInstanceColor");
	m_hBatchInstanceSelectionRotationXYScale = m_pEffectBasicShading->GetParameterByName(NULL, "g_vBatchInstanceSelectionRotationXYScale");

	m_hFinalSceneImageDelta = m_pEffectBasicShading->GetParameterByName(NULL, "g_finalImageDelta");

	m_hColorIndicate = m_pEffectBasicShading->GetParameterByName(NULL, "g_colorIndicate");
	m_hNumActiveLight = m_pEffectBasicShading->GetParameterByName(NULL, "g_numActiveLight");


	//	SSAO
		m_hSamplesSSAO = m_pEffectBasicShading->GetParameterByName(NULL, "g_vBatchInstanceColor");
		m_hNumSampleSSAO = m_pEffectBasicShading->GetParameterByName(NULL, "g_numSampleSSAO");

		m_hCamFrustumTopLeft = m_pEffectBasicShading->GetParameterByName(NULL, "g_camFrustumTopLeft");
		m_hMatProj = m_pEffectBasicShading->GetParameterByName(NULL, "g_matProj");

		m_hTextureDepth = m_pEffectBasicShading->GetParameterByName(NULL, "textureDepth");
		m_hTextureNormal = m_pEffectBasicShading->GetParameterByName(NULL, "textureNormal");
		m_hTextureNoise = m_pEffectBasicShading->GetParameterByName(NULL, "textureNoise");
		m_hTextureColor = m_pEffectBasicShading->GetParameterByName(NULL, "textureColor");

		m_hSSAOParam = m_pEffectBasicShading->GetParameterByName(NULL, "g_ssaoParam");
		m_hTextureSSAO = m_pEffectBasicShading->GetParameterByName(NULL, "textureSSAO");
		m_hBlurTextureSize = m_pEffectBasicShading->GetParameterByName(NULL, "g_blurTextureSize");

		m_hFinalTexture1 = m_pEffectBasicShading->GetParameterByName(NULL, "textureColor1");
		m_hFinalTexture2 = m_pEffectBasicShading->GetParameterByName(NULL, "textureColor2");

	static CString strIllModel[2][MAX_RENDERING_TECHNIQUE] = {	
									{	"SurfaceRenderingNoAlphaGouraud"	, 
										"SurfaceRenderingWithAlphaGouraud"	, 
										"WireframeRenderingPhong"			,
										"WireframeRenderingPhongLineWidth"	,
										"RibbonRenderingGouraud"			,	
										"RibbonRenderingNoTextureGouraud"	,	
										"ClipPlaneRenderingNoAlpha"			,
										"ClipPlaneRenderingWithAlpha"		,
										"AxisRendering"						,
										"LineWireframeRendering"			,
										"SkyBoxRendering"					,
										"RenderFinalSceneBorder"			,
										"SphereRenderingBatchGouraud"		,
										"CylinderRenderingBatchGouraud"		,
										"Text3DRenderingNoAlpha"			,
										"Text3DRenderingWithAlpha"			,		
										"SSAO_SM3"							,
										"Blur4pixel"						,
										"Blur16pixel"						,
										"RenderFinalSceneOneTexture"		,
										"RenderFinalSceneTwoTexture"

									},

									{	"SurfaceRenderingNoAlphaPhong",		
										"SurfaceRenderingWithAlphaPhong",	
										"WireframeRenderingPhong"		,	
										"WireframeRenderingPhongLineWidth"	,
										"RibbonRenderingPhong"			,	
										"RibbonRenderingNoTexturePhong"	,	
										"ClipPlaneRenderingNoAlpha"		,	
										"ClipPlaneRenderingWithAlpha"	,	
										"AxisRendering"					,	
										"LineWireframeRendering"		,
										"SkyBoxRendering"				,
										"RenderFinalSceneBorder"		,
										"SphereRenderingBatchPhong"		,
										"CylinderRenderingBatchPhong"	,
										"Text3DRenderingNoAlpha"		,
										"Text3DRenderingWithAlpha"		,
										"SSAO_SM3"						,		
										"Blur4pixel"					,
										"Blur16pixel"					,
										"RenderFinalSceneOneTexture"	,
										"RenderFinalSceneTwoTexture"
									}
								};


	for ( int i = 0 ; i < 2 ; i++ )
	{
		for ( int j = 0 ; j < MAX_RENDERING_TECHNIQUE ; j++ )
		{
			m_hIlluminationModel[i][j] = m_pEffectBasicShading->GetTechniqueByName(strIllModel[i][j]);
		}
	}

	return S_OK;
}

//-----------------------------------------------------------------------------
// Name: RestoreDeviceObjects()
// Desc: Paired with InvalidateDeviceObjects()
//       The device exists, but may have just been Reset().  Resources in
//       D3DPOOL_DEFAULT and any other device state that persists during
//       rendering should be set here.  Render states, matrices, textures,
//       etc., that don't change during rendering can be set once here to
//       avoid redundant state setting during Render () or FrameMove().
//-----------------------------------------------------------------------------
HRESULT CProteinVistaRenderer::RestoreDeviceObjects()
{
	HRESULT hr;

    // setup render states
    m_pd3dDevice->SetRenderState( D3DRS_LIGHTING, TRUE );

    // Setup a material
    D3DMATERIAL9 mtrl;
	ZeroMemory(&mtrl, sizeof(D3DMATERIAL9) );
    mtrl.Diffuse.r = mtrl.Ambient.r = 1.0f;
    mtrl.Diffuse.g = mtrl.Ambient.g = 1.0f;
    mtrl.Diffuse.b = mtrl.Ambient.b = 1.0f;
    mtrl.Diffuse.a = mtrl.Ambient.a = 1.0f;
    m_pd3dDevice->SetMaterial( &mtrl );

    // Set up the textures
    m_pd3dDevice->SetSamplerState( 0, D3DSAMP_MINFILTER, D3DTEXF_LINEAR );
    m_pd3dDevice->SetSamplerState( 0, D3DSAMP_MAGFILTER, D3DTEXF_LINEAR );

    // Set miscellaneous render states
    m_pd3dDevice->SetRenderState( D3DRS_ZENABLE,        TRUE );
    m_pd3dDevice->SetRenderState( D3DRS_AMBIENT,        0x00404040 );

	if ( m_pPropertyScene->m_iAntialiasing != CPropertyScene::AA_NONE )
	{
		HRESULT hr;

		hr = m_pd3dDevice->CreateRenderTarget( m_d3dsdBackBuffer.Width, m_d3dsdBackBuffer.Height, D3DFMT_A8R8G8B8	/*m_d3dSettings.BackBufferFormat()*/ , 
												m_typeMultiSample, 0,
												FALSE, &m_pSurfaceMultiSampleRenderTargetColor, NULL );

		if( FAILED( hr ) )
			m_pPropertyScene->m_iAntialiasing = CPropertyScene::AA_NONE;
		else
		{
			hr = m_pd3dDevice->CreateDepthStencilSurface( m_d3dsdBackBuffer.Width, m_d3dsdBackBuffer.Height, 
															m_d3dSettings.DepthStencilBufferFormat() ,
															m_typeMultiSample, 0,
															TRUE, &m_pSurfaceMultiSampleDepthStencil, NULL );
			if( FAILED( hr ) )
			{
				m_pPropertyScene->m_iAntialiasing = CPropertyScene::AA_NONE;
				SAFE_RELEASE( m_pSurfaceMultiSampleRenderTargetColor );
			}
		}
	}

	hr = m_pd3dDevice->CreateTexture( m_d3dsdBackBuffer.Width, m_d3dsdBackBuffer.Height, 1, D3DUSAGE_RENDERTARGET, D3DFMT_A8R8G8B8, D3DPOOL_DEFAULT, &m_pTexRenderColor, NULL );
	if( FAILED( hr ) )
		return hr;

	//
	//
	if ( m_pPropertyScene->m_bUseSSAO == TRUE )
	{
		//	TODO: m_d3dSettings.BackBufferFormat() --> 를 D3DFMT_A8R8G8B8로 바꾸는것을 check 해 봐야 한다.
		hr = m_pd3dDevice->CreateTexture( m_d3dsdBackBuffer.Width, m_d3dsdBackBuffer.Height, 1 , D3DUSAGE_RENDERTARGET, D3DFMT_A8R8G8B8 /*m_d3dSettings.BackBufferFormat()*/ , D3DPOOL_DEFAULT, &m_pTexFinalRenderTarget, NULL );
		if( FAILED( hr ) )
			return hr;

		hr = m_pd3dDevice->CreateTexture( m_d3dsdBackBuffer.Width, m_d3dsdBackBuffer.Height, 1, D3DUSAGE_RENDERTARGET, D3DFMT_R32F, D3DPOOL_DEFAULT, &m_pTexRenderDepth, NULL );
		if( FAILED( hr ) )
			return hr;

		hr = m_pd3dDevice->CreateTexture( m_d3dsdBackBuffer.Width, m_d3dsdBackBuffer.Height, 1, D3DUSAGE_RENDERTARGET, D3DFMT_A8R8G8B8, D3DPOOL_DEFAULT, &m_pTexRenderNormal, NULL );
		if( FAILED( hr ) )
			return hr;

		m_ssaoTextureSizeWidth = m_d3dsdBackBuffer.Width;
		m_ssaoTextureSizeHeight = m_d3dsdBackBuffer.Height;

		if ( m_pPropertyScene->m_bUseFullSizeBlur == FALSE )
		{
			m_ssaoTextureSizeWidth = (m_ssaoTextureSizeWidth+1)/2;
			m_ssaoTextureSizeHeight = (m_ssaoTextureSizeHeight+1)/2;
		}

		hr = m_pd3dDevice->CreateTexture( m_ssaoTextureSizeWidth, m_ssaoTextureSizeHeight ,1, D3DUSAGE_RENDERTARGET, D3DFMT_A8R8G8B8, D3DPOOL_DEFAULT, &m_pTexRenderSSAO, NULL );
		if( FAILED( hr ) )
			return hr;

		hr = m_pd3dDevice->CreateTexture( m_ssaoTextureSizeWidth, m_ssaoTextureSizeHeight ,1, D3DUSAGE_RENDERTARGET, D3DFMT_A8R8G8B8, D3DPOOL_DEFAULT, &m_pTexRenderBlur, NULL );
		if( FAILED( hr ) )
			return hr;
	}

	// Set the world matrix
    D3DXMATRIX matIdentity;
    D3DXMatrixIdentity( &matIdentity );
    m_pd3dDevice->SetTransform( D3DTS_WORLD,  &matIdentity );

    // Setup the projection matrix
	SetViewVolume();

    // Restore the font
    m_pFont->RestoreDeviceObjects();

	if (m_pEffectBasicShading)	m_pEffectBasicShading->OnResetDevice();

	long iProgress =GetMainActiveView()->InitProgress(100);
	for ( int i = 0 ; i < m_arrayPDBRenderer.size(); i++ )
	{
		GetMainActiveView()->SetProgress( (i+1)*100/m_arrayPDBRenderer.size() , iProgress);
		m_arrayPDBRenderer[i]->RestoreDeviceObjects();
	}
	GetMainActiveView()->EndProgress(iProgress);

	iProgress = GetMainActiveView()->InitProgress(100);

	CHTMLListCtrl * phtmlListCtrl = GetMainActiveView()->GetSelectList();
	for ( int i = 0 ; i < phtmlListCtrl->GetItemCount() ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *)(phtmlListCtrl->GetItemData(i));

		GetMainActiveView()->SetProgress( (i+1)*100/phtmlListCtrl->GetItemCount() , iProgress);
		if ( pSelectionDisplay )
		{
			pSelectionDisplay->RestoreDeviceObjects();
		}
	} 

	GetMainActiveView()->EndProgress(iProgress);

	m_pSkyManager->RestoreDeviceObjects();

	RECT rect;
	GetClientRect(m_hWnd, &rect);

	for( int i=0; i<MAX_LIGHTS; i++ )
		m_LightControl[i].OnResetDevice( rect.right, rect.bottom );

	m_LightControl[0].SetRadius(m_pPropertyScene->m_fLight1Radius);
	m_LightControl[1].SetRadius(m_pPropertyScene->m_fLight2Radius);

	//	여기서 SetFog를 해준다..
	//	D3D 버그라고 생각됨.
	//	D3DRS_FOGCOLOR 가 device 초기에 설정하면 적용이 안됨.
	SetFog();
	
	//	Final Scene Render Init
	{
		// Ensure that we're directly mapping texels to pixels by offset by 0.5
		// For more info see the doc page titled "Directly Mapping Texels to Pixels"
		FLOAT fWidth5 = ( FLOAT )m_d3dsdBackBuffer.Width - 0.5f;
		FLOAT fHeight5 = ( FLOAT )m_d3dsdBackBuffer.Height - 0.5f;

		// Draw the quad
		FLOAT du = 1.0f/(m_d3dsdBackBuffer.Width);
		FLOAT dv = 1.0f/(m_d3dsdBackBuffer.Height);

		FLOAT shaderConst[2] = {du, dv};
		SetShaderImageDelta(shaderConst);

		SCREEN_VERTEX svQuad[4] = {
			//   x    y   z    rhw			tu       tv
			{ -0.5f,   -0.5f , 0.5f, 1.0f
			, 0.0f-du, 0.0f-dv
			, 0.0f+du, 0.0f+dv 
			, 0.0f-du, 0.0f+dv 
			, 0.0f+du, 0.0f-dv},
			{    fWidth5,   -0.5f , 0.5f, 1.0f
			, 1.0f-du, 0.0f-dv
			, 1.0f+du, 0.0f+dv
			, 1.0f-du, 0.0f+dv
			, 1.0f+du, 0.0f-dv, },
			{    fWidth5,   fHeight5, 0.5f, 1.0f
			, 1.0f-du, 1.0f-dv
			, 1.0f+du, 1.0f+dv
			, 1.0f-du, 1.0f+dv
			, 1.0f+du, 1.0f-dv, },
			{ -0.5f,   fHeight5, 0.5f, 1.0f
			, 0.0f-du, 1.0f-dv
			, 0.0f+du, 1.0f+dv
			, 0.0f-du, 1.0f+dv
			, 0.0f+du, 1.0f-dv, },
		};

		m_svQuad[0] = svQuad[0];
		m_svQuad[1] = svQuad[1];
		m_svQuad[2] = svQuad[2];
		m_svQuad[3] = svQuad[3];
	}

    return S_OK;
}

#pragma managed(push,off)
//-----------------------------------------------------------------------------
// Name: FrameMove()
// Desc: Called once per frame, the call is the entry point for animating
//       the scene.
//-----------------------------------------------------------------------------
HRESULT CProteinVistaRenderer::FrameMove()
{
	//	m_ToVec는 원점으로 고정. 카메라는 원점을 중심으로 회전
	D3DXMATRIXA16 matRotInv;
	D3DXMatrixInverse(&matRotInv, NULL, &m_matCameraRotation);

	D3DXVECTOR3 vecFrom(0,0, m_cameraZPos);
	D3DXVec3TransformCoord ( &m_FromVec, &vecFrom, &matRotInv );

    D3DXVECTOR3 vUp( 0, 1, 0 );
	D3DXVec3TransformCoord ( &vUp, &vUp, &matRotInv );

	D3DXMatrixLookAtLH( &m_matrixView, &m_FromVec, &m_ToVec, &vUp );
    m_pd3dDevice->SetTransform( D3DTS_VIEW, &m_matrixView );
	
	//	
	m_pSkyManager->FrameMove();

// 
 	for ( int i = 0 ; i < m_arrayPDBRenderer.size(); i++ )
 	{
 		m_arrayPDBRenderer[i]->FrameMove();
 	}

	//	global clip plane.
	m_pClipPlane->FrameMove();

	//
	CHTMLListCtrl * phtmlListCtrl = GetMainActiveView()->GetSelectList();
	for ( int i = 0 ; i < phtmlListCtrl->GetItemCount() ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *)(phtmlListCtrl->GetItemData(i));

		if ( pSelectionDisplay )
		{
			pSelectionDisplay->FrameMove();
		}
	}

	SetViewVolClipPlane();

    return S_OK;
}
#pragma managed(pop)
//-----------------------------------------------------------------------------
// Name: UpdateInput()
// Desc: Update the user input.  Called once per frame 
//-----------------------------------------------------------------------------
void CProteinVistaRenderer::UpdateInput( UserInput* pUserInput )
{
    pUserInput->bRotateUp    = ( m_bActive && (GetAsyncKeyState( VK_UP )    & 0x8000) == 0x8000 );
    pUserInput->bRotateDown  = ( m_bActive && (GetAsyncKeyState( VK_DOWN )  & 0x8000) == 0x8000 );
    pUserInput->bRotateLeft  = ( m_bActive && (GetAsyncKeyState( VK_LEFT )  & 0x8000) == 0x8000 );
    pUserInput->bRotateRight = ( m_bActive && (GetAsyncKeyState( VK_RIGHT ) & 0x8000) == 0x8000 );
}

#pragma managed(push,off)
//-----------------------------------------------------------------------------
// Name: Render ()
// Desc: Called once per frame, the call is the entry point for 3d
//       rendering. This function sets up render states, clears the
//       viewport, and renders the scene.
//-----------------------------------------------------------------------------
HRESULT CProteinVistaRenderer::Render()
{ 
	HRESULT hr;
 
	ApplyColorIndicateTable();

	//	Backface Rendering.
	LPDIRECT3DSURFACE9 pSurfRenderTargetOrig = NULL;
	PDIRECT3DSURFACE9 pSurfDepthStencilOrig = NULL;
	m_pd3dDevice->GetRenderTarget( 0, &pSurfRenderTargetOrig );
	m_pd3dDevice->GetDepthStencilSurface( &pSurfDepthStencilOrig );

	LPDIRECT3DSURFACE9 pSurfFinalRenderTarget = NULL;

	//	Add SSAO
	LPDIRECT3DSURFACE9 pSurfRenderTargetColor = NULL;
	LPDIRECT3DSURFACE9 pSurfRenderTargetNormal = NULL;
	LPDIRECT3DSURFACE9 pSurfRenderTargetDepth = NULL;
	
	LPDIRECT3DSURFACE9 pSurfRenderTargetSSAO = NULL;
	LPDIRECT3DSURFACE9 pSurfRenderTargetBlur= NULL;

	m_pTexRenderColor->GetSurfaceLevel( 0, &pSurfRenderTargetColor );

	if ( m_pPropertyScene->m_bUseSSAO == TRUE )
	{
		m_pTexFinalRenderTarget->GetSurfaceLevel( 0, &pSurfFinalRenderTarget );

		m_pTexRenderNormal->GetSurfaceLevel( 0, &pSurfRenderTargetNormal );
		m_pTexRenderDepth->GetSurfaceLevel( 0, &pSurfRenderTargetDepth );
		m_pTexRenderSSAO->GetSurfaceLevel( 0, &pSurfRenderTargetSSAO );
		m_pTexRenderBlur->GetSurfaceLevel( 0, &pSurfRenderTargetBlur);
	}

	// 
	if( m_pPropertyScene->m_iAntialiasing != CPropertyScene::AA_NONE )
	{	//	Multisample target.
		m_pd3dDevice->SetRenderTarget( 0, m_pSurfaceMultiSampleRenderTargetColor );
		m_pd3dDevice->SetDepthStencilSurface( m_pSurfaceMultiSampleDepthStencil );
	}
	else
	{
		//	1x target.
		m_pd3dDevice->SetRenderTarget( 0, pSurfRenderTargetColor );
		if ( m_pPropertyScene->m_bUseSSAO == TRUE )
		{
			m_pd3dDevice->SetRenderTarget( 1, pSurfRenderTargetNormal );
			m_pd3dDevice->SetRenderTarget( 2, pSurfRenderTargetDepth );
		}
	}
	
    // Clear the viewport
    m_pd3dDevice->Clear( 0L, NULL, D3DCLEAR_TARGET|D3DCLEAR_ZBUFFER, m_pPropertyScene->m_d3dcolorBackroundColor, 1.0f, 0L );
	if ( m_pPropertyScene->m_bUseSSAO == TRUE )
	{
		m_pd3dDevice->ColorFill( pSurfRenderTargetDepth, NULL, D3DXCOLOR(1,1,1,1) );
		m_pd3dDevice->ColorFill( pSurfRenderTargetNormal, NULL, D3DXCOLOR(0,0,0,0) );
	}

	//	
	if ( m_pPropertyScene->m_bUseSSAO == TRUE )
	{
		float farClipPlane = m_fFarClipPlane;
		m_pEffectBasicShading->SetFloat("g_farClipPlane", farClipPlane );
	}

	//	ViewVol이 Fog 보다 앞에 있어야 함.
	SetViewVolume();
	//	
	SetFog();

	if ( m_shaderVersion >= 3 )
	{
		//	SM3 에서 Light의 갯수를 설정
		int maxLight = m_pPropertyScene->m_bLight1Use + m_pPropertyScene->m_bLight2Use;
		SetNumActiveLights( maxLight );
	}
	
    // Begin the scene
    if( SUCCEEDED( m_pd3dDevice->BeginScene() ) )
    {
        // render world
		 
		m_pd3dDevice->SetRenderState( D3DRS_ZENABLE, TRUE );

		SetFogEnable(FALSE);
		{
			//	skybox 는 항상 맨앞에.
 			if ( m_pPropertyScene->m_bUseBackgroundTexture == TRUE )
 				m_pSkyManager->Render();

			//	render light direction
			// Render the light arrow so the user can visually see the light dir
			if ( m_pPropertyScene->m_bLight1Use == TRUE )
			{
				D3DXVECTOR3 vLightDir;
				D3DXCOLOR arrowColor = COLORREF2D3DXCOLOR(m_pPropertyScene->m_light1Color);
				if( m_pPropertyScene->m_bLight1Show == TRUE )
				{
					m_LightControl[0].OnRender( arrowColor, &m_matrixView, &m_matrixProj , &m_FromVec );
				}
				vLightDir = m_LightControl[0].GetLightDirection();
				SetShaderLight1Direction(vLightDir);
			}

			if ( m_pPropertyScene->m_bLight2Use == TRUE )
			{
				D3DXVECTOR3 vLightDir;
				D3DXCOLOR arrowColor = COLORREF2D3DXCOLOR(m_pPropertyScene->m_light2Color);
				if( m_pPropertyScene->m_bLight2Show == TRUE )
					m_LightControl[1].OnRender( arrowColor, &m_matrixView, &m_matrixProj , &m_FromVec );
				vLightDir = m_LightControl[1].GetLightDirection();
				SetShaderLight2Direction(vLightDir);
			}

			if ( m_pPropertyScene->m_bDisplayAxis == TRUE )
			{
				FLOAT scale = m_pPropertyScene->m_axisScale/50.0f;
				if ( scale > 1.0f )		scale = scale*scale*scale;

				D3DXMATRIXA16 matScale;
				D3DXMatrixScaling(&matScale, scale, scale, scale);

				m_CoordinateAxisDisplay.SetModelScale(matScale);
				m_CoordinateAxisDisplay.Render();
			}

		}
		SetFogEnable(TRUE);

		SetGlobalClipPlane();

		//	PDB Renderer를 Render 한다.
		//	HETATM 에 대해서 Rendering.
		for ( int i = 0 ; i < m_arrayPDBRenderer.size(); i++ )
		{
			m_arrayPDBRenderer[i]->Render();
		}

		//	render molecule.
		//	surface는 투명도가 있기 때문에 나중에 그린다.
		m_transparentRenderObject.clear();

		BOOL bCullMode = D3DCULL_CCW;

		CSTLArrayClipPlane	clipPlaneTransparency;

		CHTMLListCtrl * phtmlListCtrl = GetMainActiveView()->GetSelectList();
		for ( int i = 0 ; i < phtmlListCtrl->GetItemCount() ; i++ )
		{
			CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *)(phtmlListCtrl->GetItemData(i));
			if ( pSelectionDisplay )
			{
				if ( pSelectionDisplay->m_bShow == TRUE )
				{
					//	surface 이면서, 투명값이 100 이 아니면, 투명한것이므로 나중에 그린다.
					if ( pSelectionDisplay->m_displayStyle == CSelectionDisplay::SURFACE && pSelectionDisplay->GetPropertySurface()->m_transparency != 100 )
					{
						m_transparentRenderObject.push_back(pSelectionDisplay);
					}
					else
					{
						CPropertyCommon* pPropertyCommon = pSelectionDisplay->GetPropertyCommon();
						if ( pPropertyCommon->m_bClipping1 == TRUE || pPropertyCommon->m_bClipping2 == TRUE )
							m_pd3dDevice->SetRenderState( D3DRS_CULLMODE , D3DCULL_NONE );

						pSelectionDisplay->Render();

						m_pd3dDevice->SetRenderState( D3DRS_CULLMODE , bCullMode );
					}
				}
			}
		}

		SetFogEnable(FALSE); 
		{
			// If using floating point multi sampling, StretchRect to the rendertarget
			if ( m_pPropertyScene->m_iAntialiasing != CPropertyScene::AA_NONE )
			{
				m_pd3dDevice->StretchRect( m_pSurfaceMultiSampleRenderTargetColor, NULL, pSurfRenderTargetColor, NULL, D3DTEXF_NONE );
			}

			if( m_pPropertyScene->m_iAntialiasing != CPropertyScene::AA_NONE )
			{
				m_pd3dDevice->SetRenderTarget( 0, pSurfRenderTargetOrig );
			}
			else
			{
				//	restore original back buffer.
				if ( m_pPropertyScene->m_bUseSSAO == TRUE )
					m_pd3dDevice->SetRenderTarget( 0, pSurfFinalRenderTarget );
				else
					m_pd3dDevice->SetRenderTarget( 0, pSurfRenderTargetOrig );
			}

			SetShaderTechnique(RenderFinalSceneWithBorder);
			SetShaderFinalSceneTexture(m_pTexRenderColor);

			UINT iPass, cPasses;
			V( m_pEffectBasicShading->Begin( &cPasses, 0 ) );
			for( iPass = 0; iPass < cPasses; iPass++ )
			{
				V( m_pEffectBasicShading->BeginPass( iPass ) );
				DrawFullScreenQuad();
				V( m_pEffectBasicShading->EndPass() );
			}
			V( m_pEffectBasicShading->End() );

			m_pd3dDevice->SetTexture( 0, NULL );
			SetShaderFinalSceneTexture(NULL);
		}

		SetFogEnable(TRUE);

		if ( m_pPropertyScene->m_bClipping0 == TRUE )
		{
			//	global clip plane 이 있으면 cull mode 무조건 NONE 이다.
			m_pd3dDevice->SetRenderState( D3DRS_CULLMODE , D3DCULL_NONE );

			if ( m_pPropertyScene->m_clipPlaneTransparency0 != 100 )
				m_transparentRenderObject.push_back(m_pClipPlane);
			else
				m_pClipPlane->Render();
		}

		//
		//
		m_pd3dDevice->SetRenderState( D3DRS_CULLMODE , D3DCULL_NONE );

		if ( m_pPropertyScene->m_bUseSSAO == TRUE )
		{
			if( m_pPropertyScene->m_iAntialiasing != CPropertyScene::AA_NONE )
			{
				m_pd3dDevice->StretchRect( m_pSurfaceMultiSampleRenderTargetColor, NULL, pSurfFinalRenderTarget, NULL, D3DTEXF_NONE );
				//D3DXSaveTextureToFile("k:\\pSurfFinalRenderTarget.png",D3DXIFF_PNG,m_pTexFinalRenderTarget, NULL );
			}

			m_pd3dDevice->SetRenderTarget( 1, NULL );
			m_pd3dDevice->SetRenderTarget( 2, NULL );

			//========================================
			//	SSAO
			//========================================
			if( m_pPropertyScene->m_iAntialiasing != CPropertyScene::AA_NONE )
				m_pd3dDevice->SetRenderTarget( 0, m_pSurfaceMultiSampleRenderTargetColor );
			else
				m_pd3dDevice->SetRenderTarget( 0, pSurfRenderTargetSSAO );

			SetShaderTechnique(SSAO_SM3);

			//	ssam sampling.
			SetBatchSamplesSSAO ( m_samplesSSAO, m_pPropertyScene->m_numSSAOSampling );

			m_pEffectBasicShading->SetTexture( m_hTextureNormal, m_pTexRenderNormal );
			m_pEffectBasicShading->SetTexture( m_hTextureDepth,  m_pTexRenderDepth );
			m_pEffectBasicShading->SetTexture( m_hTextureColor,  m_pTexRenderColor );

			//	random noise texture.
			m_pEffectBasicShading->SetTexture( m_hTextureNoise,  m_pTexNoise );

			float fCamFOVY = m_fovY;
			//	float fCamFOVX = fCamFOVY * m_fAspectRatio;		//	틀린코드. fov*aspect 하면 안된다.

			float farClipPlane = m_fFarClipPlane;
			float fCamFrustumTopLeft[2];
			fCamFrustumTopLeft[1] = tan( fCamFOVY /2 ) * farClipPlane ;
			fCamFrustumTopLeft[0] = fCamFrustumTopLeft[1]*m_fAspectRatio;

			m_pEffectBasicShading->SetValue( m_hCamFrustumTopLeft, fCamFrustumTopLeft, sizeof(FLOAT)*2 );

			//
			m_pEffectBasicShading->SetMatrix( m_hMatProj, GetProjMatrix() );

			FLOAT	ssaoParam[3];
			ssaoParam[0] = m_pPropertyScene->m_ssaoRange*0.1f;
			ssaoParam[1] = m_pPropertyScene->m_ssaoIntensity*0.05f;
			ssaoParam[2] = m_pPropertyScene->m_numSSAOSampling;

			m_pEffectBasicShading->SetValue(m_hSSAOParam, ssaoParam, sizeof(FLOAT)*3 );

			m_pd3dDevice->SetRenderState( D3DRS_ZENABLE, FALSE );
			m_pd3dDevice->SetRenderState( D3DRS_LIGHTING, FALSE );

			FLOAT du = 1.0f/(m_d3dsdBackBuffer.Width);
			FLOAT dv = 1.0f/(m_d3dsdBackBuffer.Height);

			typedef struct {FLOAT p[3]; FLOAT t[2];} TVERTEX;

			static TVERTEX Vertex1[4] = {
				//   x    y     z    u0 v0
				{-1.0f, +1.0f, 0.1f,  0, 0,},
				{+1.0f, +1.0f, 0.1f,  1, 0,},
				{+1.0f, -1.0f, 0.1f,  1, 1,},
				{-1.0f, -1.0f, 0.1f,  0, 1,},
			};

			m_pd3dDevice->SetFVF( D3DFVF_XYZ | D3DFVF_TEX1 );

			//
			//
			UINT uiPass, uiNumPasses;
			m_pEffectBasicShading->Begin( &uiNumPasses, 0 );

			for( uiPass = 0; uiPass < uiNumPasses; uiPass++ )
			{
				m_pEffectBasicShading->BeginPass( uiPass );

				m_pd3dDevice->DrawPrimitiveUP( D3DPT_TRIANGLEFAN, 2, Vertex1, sizeof( TVERTEX ) );

				m_pEffectBasicShading->EndPass();
			}

			m_pEffectBasicShading->End();

			if( m_pPropertyScene->m_iAntialiasing != CPropertyScene::AA_NONE )
			{
				m_pd3dDevice->StretchRect( m_pSurfaceMultiSampleRenderTargetColor, NULL, pSurfRenderTargetSSAO , NULL, D3DTEXF_NONE );
			}

			//D3DXSaveTextureToFile("k:\\m_pTexNoise.png" , D3DXIFF_PNG, m_pTexNoise, NULL );
			//D3DXSaveTextureToFile("k:\\m_pTexRenderNormal.png" , D3DXIFF_PNG, m_pTexRenderNormal, NULL );
			//D3DXSaveTextureToFile("k:\\m_pTexRenderSSAO.png" , D3DXIFF_PNG, m_pTexRenderSSAO, NULL );
			//D3DXSaveTextureToFile("k:\\m_pTexRenderColor.png" , D3DXIFF_PNG, m_pTexRenderColor, NULL );
			//========================================
			//	Blur
			//========================================
			if ( m_pPropertyScene->m_ssaoBlurType != CPropertyScene::BLUR_NONE )
			{
				FLOAT blurTextureSize[2];
				blurTextureSize[0] = m_ssaoTextureSizeWidth;
				blurTextureSize[1] = m_ssaoTextureSizeHeight;

				m_pEffectBasicShading->SetValue( m_hBlurTextureSize, blurTextureSize, sizeof(FLOAT)*2 );

				if( m_pPropertyScene->m_iAntialiasing != CPropertyScene::AA_NONE )
					m_pd3dDevice->SetRenderTarget( 0, m_pSurfaceMultiSampleRenderTargetColor );
				else
					m_pd3dDevice->SetRenderTarget( 0, pSurfRenderTargetBlur);

				if ( m_pPropertyScene->m_ssaoBlurType == CPropertyScene::BLUR_4 )
					SetShaderTechnique( Blur4pixel );
				else
					SetShaderTechnique( Blur16pixel );

				V(m_pEffectBasicShading->SetTexture( m_hTextureSSAO ,  m_pTexRenderSSAO ));

				typedef struct {FLOAT p[3]; FLOAT t[2];} TVERTEX;
				static TVERTEX Vertex1[4] = {
					//   x    y     z    u0 v0
					{-1.0f, +1.0f, 0.1f,  0, 0,},
					{+1.0f, +1.0f, 0.1f,  1, 0,},
					{+1.0f, -1.0f, 0.1f,  1, 1,},
					{-1.0f, -1.0f, 0.1f,  0, 1,},
				};
				m_pd3dDevice->SetFVF( D3DFVF_XYZ | D3DFVF_TEX1 );

				UINT uiPass, uiNumPasses;
				m_pEffectBasicShading->Begin( &uiNumPasses, 0 );
				for( uiPass = 0; uiPass < uiNumPasses; uiPass++ )
				{
					m_pEffectBasicShading->BeginPass( uiPass );

					m_pd3dDevice->DrawPrimitiveUP( D3DPT_TRIANGLEFAN, 2, Vertex1, sizeof( TVERTEX ) );

					m_pEffectBasicShading->EndPass();
				}
				m_pEffectBasicShading->End();

				if( m_pPropertyScene->m_iAntialiasing != CPropertyScene::AA_NONE )
				{
					m_pd3dDevice->StretchRect( m_pSurfaceMultiSampleRenderTargetColor, NULL, pSurfRenderTargetBlur , NULL, D3DTEXF_NONE );
				}
			}
 

			static int count = 0;

			m_pd3dDevice->SetRenderTarget( 0, pSurfRenderTargetOrig );

			int istep = 0;
			//	istep = (count ++ / 100) %3;
			if ( istep == 0 )
			{	
				m_pEffectBasicShading->SetTexture( m_hFinalTexture1 , m_pTexFinalRenderTarget );
				if ( m_pPropertyScene->m_ssaoBlurType == CPropertyScene::BLUR_NONE )
					m_pEffectBasicShading->SetTexture( m_hFinalTexture2 , m_pTexRenderSSAO);
				else
					m_pEffectBasicShading->SetTexture( m_hFinalTexture2 , m_pTexRenderBlur);

				SetShaderTechnique( RenderFinalSceneTwoTexture );
			}
			else if ( istep == 1 )
			{
				m_pEffectBasicShading->SetTexture( m_hFinalTexture1, m_pTexRenderBlur /*m_pTexRenderColor */);
				SetShaderTechnique( RenderFinalSceneOneTexture );
			}
			else
			{
				m_pEffectBasicShading->SetTexture( m_hFinalTexture1, m_pTexRenderNormal);
				SetShaderTechnique( RenderFinalSceneOneTexture );
			}

			m_pEffectBasicShading->Begin( &uiNumPasses, 0 );
			for( uiPass = 0; uiPass < uiNumPasses; uiPass++ )
			{
				m_pEffectBasicShading->BeginPass( uiPass );
				DrawFullScreenQuad();
				m_pEffectBasicShading->EndPass();
			}
			m_pEffectBasicShading->End();

			m_pd3dDevice->SetTexture( 0, NULL );
			m_pd3dDevice->SetTexture( 1, NULL );
			m_pd3dDevice->SetTexture( 2, NULL );

		}	//	end SSAO

		//	투명한것 그린다.
		//	투명한것은 SSAO를 적용하지 않는다. 대부분 투명한것들은 아주 투명하게 사용된다.
		if ( m_transparentRenderObject.size() > 0 )
		{
			m_pd3dDevice->SetRenderState( D3DRS_CULLMODE , D3DCULL_NONE );
			m_pd3dDevice->SetRenderState( D3DRS_ALPHABLENDENABLE, TRUE );

			for ( int i = 0 ; i < m_transparentRenderObject.size() ; i++ )
			{
				m_transparentRenderObject[i]->Render();
			}

			m_pd3dDevice->SetRenderState( D3DRS_CULLMODE , bCullMode );
			m_pd3dDevice->SetRenderState( D3DRS_ALPHABLENDENABLE, FALSE);
		}

		//	annotation을 SSAO 뒤로 뺀다. 이렇게 하지 않으면 글자가 지저분해짐.
		//	annotation 글자를 쓴다. 
		for ( int i = 0 ; i < phtmlListCtrl->GetItemCount() ; i++ )
		{
			CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *)(phtmlListCtrl->GetItemData(i));
 
			if ( pSelectionDisplay )
			{
				if ( pSelectionDisplay->m_bShow == TRUE )
				{
					pSelectionDisplay->RenderAnnotation();
				}
			}
		}
		if(GetMainActiveView()->m_bShowLog ==TRUE)
		{
			RenderText();
		}

		 
		////
		////	Text, HUD, 기타 UI는 final pass 뒤에.
		//// Render stats and help text  
		// if ( g_bDisplayFPS == TRUE )
		//	 RenderText();

		// End the scene.
        m_pd3dDevice->EndScene();
    }

    SAFE_RELEASE( pSurfFinalRenderTarget );
    SAFE_RELEASE( pSurfRenderTargetOrig );
    SAFE_RELEASE( pSurfDepthStencilOrig );

	SAFE_RELEASE( pSurfRenderTargetColor );
	SAFE_RELEASE( pSurfRenderTargetNormal );
	SAFE_RELEASE( pSurfRenderTargetDepth );
	SAFE_RELEASE( pSurfRenderTargetSSAO );
	SAFE_RELEASE( pSurfRenderTargetBlur );

    return S_OK;
}
#pragma managed(pop)
//-----------------------------------------------------------------------------
// Name: RenderText()
// Desc: Renders stats and help text to the scene.
//-----------------------------------------------------------------------------
HRESULT CProteinVistaRenderer::RenderText()
{
    D3DCOLOR fontColor        = D3DCOLOR_ARGB(255,255,255,255);
    TCHAR szMsg[MAX_PATH] = TEXT("");

    // Output display stats
    FLOAT fNextLine = 40.0f; 
  
	lstrcpy( szMsg, m_strDeviceStats );
	fNextLine -= 20.0f;
	m_pFont->DrawText( 2, fNextLine, fontColor, szMsg );

	lstrcpy( szMsg, m_strFrameStats );
	fNextLine -= 20.0f;
	m_pFont->DrawText( 2, fNextLine, fontColor, szMsg );

	
    return S_OK;
}
#pragma managed(push,off)
LRESULT CProteinVistaRenderer::HandleMessageCamera( HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam )
{
	BOOL	bProcessed = FALSE;
	int iMouseX = (short)LOWORD(lParam);
	int iMouseY = (short)HIWORD(lParam);

	switch( msg )
	{
		case WM_LBUTTONDOWN:
			{
			   {
					bProcessed = TRUE;

					SetCapture( hWnd );
					m_bCameraDrag = TRUE;
					m_posOldX = iMouseX;
					m_posOldY = iMouseY;
					m_matCameraRotationTemp = m_matCameraRotation;
					g_bRequestRender = TRUE;
				}
			}
			break;

		case WM_LBUTTONUP:
			if( m_bCameraDrag == TRUE )
			{
				bProcessed = TRUE;
				m_bCameraDrag = FALSE;
				m_posOldX = -10000;
				m_posOldY = -10000;
				ReleaseCapture();
			}
			break;

		case WM_CAPTURECHANGED:
			{
				if ( m_bCameraDrag == TRUE )
				{
					if( (HWND)lParam != hWnd )
					{
						bProcessed = TRUE;
						m_bCameraDrag = FALSE;
						m_posOldX = -10000;
						m_posOldY = -10000;
						ReleaseCapture();
					}
				}
			}
			break;

		case WM_MOUSEMOVE:
			if ( m_bCameraDrag == TRUE )
			{
				bProcessed = TRUE;

				D3DXMATRIXA16 matRotTemp;

				//	도리도리, 끄덕끄덕, 갸우뚱
				D3DXMatrixRotationYawPitchRoll ( &matRotTemp, -(iMouseX-m_posOldX)/100.0f , -(iMouseY-m_posOldY)/100.0f, 0 );
				m_matCameraRotation = m_matCameraRotationTemp * matRotTemp;

				g_bRequestRender = TRUE;
                GetMainActiveView()->OnPaint();
			}
			break;

		case WM_MOUSEWHEEL:
			{
				bProcessed = TRUE;
				//	zDelta is multiple of WHEEL_DELTA.
				signed short zDelta = HIWORD(wParam);

				float delta;
				BOOL	keyCtrl = GetAsyncKeyState(VK_CONTROL)>>15;
				BOOL	keyShift = GetAsyncKeyState(VK_SHIFT)>>15;

				if ( keyCtrl != 0 && keyShift != 0 )
				{
					delta = 30.0f;
				}
				else
				if ( keyCtrl != 0 )
				{
					delta = 12.0f;
				}
				else if ( keyShift != 0 )
				{
					delta = 0.3f;
				}
				else
				{
					delta = 3.0f;
				}

				if ( zDelta > 0 )
				{
					m_cameraZPos += delta;
				}
				else
				{
					m_cameraZPos -= delta;
				}
				m_cameraZPos = min(m_cameraZPos, -3.0f);
				g_bRequestRender = TRUE;

				GetMainActiveView()->OnPaint();
			}
			break;
	}

	return bProcessed;
}
#pragma managed(pop)

//-----------------------------------------------------------------------------
// Name: MsgProc()
// Desc: Overrrides the main WndProc, so the sample can do custom message
//       handling (e.g. processing mouse, keyboard, or menu commands).
//-----------------------------------------------------------------------------
LRESULT CProteinVistaRenderer::MsgProc( HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam )
{
	static	POINT	oldPos= {-10000,-10000};
	static	BOOL	bAtomSelectMsg = FALSE;
 
	BOOL	bProcessed = FALSE;
	switch ( msg )
	{
		case WM_LBUTTONDOWN:
		case WM_RBUTTONDOWN:
			{
				oldPos.x = GET_X_LPARAM(lParam);
				oldPos.y = GET_Y_LPARAM(lParam);

				TRACE("WM_LBUTTONDOWN:%d,%d\n", oldPos.x, oldPos.y );

				BOOL	bProcessed = FALSE;
				if ( (GetAsyncKeyState( VK_LSHIFT ) & 0x8000) == 0x8000 )
				{	 
					long iLight = 0;
					FLOAT distLight = 10000.0f;
					FLOAT distClipPlane = 10000.0f;

					BOOL bSelectLight = GetSelectLightSource(iLight, distLight);
					CClipPlane * clipPlane = GetSelectClipPlane(distClipPlane);

					if ( distLight < distClipPlane )
					{
						if ( bSelectLight == TRUE )
						{
							m_LightControl[iLight].HandleMessages( hWnd, msg, wParam, lParam );
							bProcessed |= TRUE;
						}
					}
					else
					{
						if ( clipPlane != NULL )
						{
							clipPlane->HandleMessages(hWnd, msg, wParam, lParam );
							bProcessed |= TRUE;
						}
					}
				}

				if ( (GetAsyncKeyState( VK_LCONTROL )& 0x8000) == 0x8000 )
				{	//	atom 선택 없이 무조건 camera rot.trans.
					HandleMessageCamera(hWnd, msg, wParam, lParam );
				}

				//	이경우 buttonUp에서 selectAtom이 불린다.
				if ( bProcessed == FALSE )
					bAtomSelectMsg = TRUE;

				if ( bProcessed == FALSE )
				{
					//	일반적인 selection의 rot, trans
					if ( m_pActivePDBRenderer )
						bProcessed |= m_pActivePDBRenderer->HandleMessages( m_hWnd, msg, wParam, lParam );
				}

				if ( bProcessed == TRUE )
					g_bRequestRender = TRUE;
			}
			break;

		case WM_LBUTTONDBLCLK:
			 {
					FLOAT animationTime = 0.0f;
					if ( m_pPropertyScene->m_bDoubleClockToCameraAnimation == TRUE )
						animationTime = m_pPropertyScene->m_fAnimationTime;
					else
						animationTime = 0.0f;

					POINT pt;
					pt.x = GET_X_LPARAM(lParam);
					pt.y = GET_Y_LPARAM(lParam);

					oldPos.x = pt.x;
					oldPos.y = pt.y;

					TRACE("WM_LBUTTONDBLCLK:%d,%d\n", pt.x, pt.y );

					SelectAtom(pt);

					if (m_pLastPickObjectInst)
					{	
						D3DXVECTOR3 center;
						if ( dynamic_cast<CResidueInst*>(m_pLastPickObjectInst) != NULL )
						{
							CResidueInst * pResidueInst = dynamic_cast<CResidueInst*>(m_pLastPickObjectInst);
							SetCameraAnimation( pResidueInst, (FLOAT)(animationTime) );
						}
						else
						if ( dynamic_cast<CAtomInst*>(m_pLastPickObjectInst) != NULL )
						{
							CAtomInst * pAtom = dynamic_cast<CAtomInst *>(m_pLastPickObjectInst);
							SetCameraAnimation( pAtom, (FLOAT)(animationTime) );
						}
 
						PumpMessage();
					}
				}
               
			break;

		case WM_LBUTTONUP:
		case WM_RBUTTONUP:
		case WM_MOUSEMOVE:
		case WM_MOUSEWHEEL:
		case WM_CAPTURECHANGED:
			{
				if ( msg == WM_LBUTTONUP )
				{
					if ( bAtomSelectMsg == TRUE )
					{
						POINT currentPos;
						currentPos.x = GET_X_LPARAM(lParam);
						currentPos.y = GET_Y_LPARAM(lParam);

						TRACE("WM_LBUTTONUP:%d,%d\n", currentPos.x, currentPos.y );

						if ( abs(currentPos.x-oldPos.x) <= 1 && abs(currentPos.y-oldPos.y) <= 1 )
						{
							if ( SelectAtom(oldPos) == FALSE )
							{
								DeSelectAllAtoms();
							}
						}
					}

					bAtomSelectMsg = FALSE;
				}
				//	여기 수정.
				BOOL bProcessed = FALSE;
				bProcessed |= HandleMessageCamera(hWnd, msg, wParam, lParam );
				bProcessed |= m_LightControl[0].HandleMessages( hWnd, msg, wParam, lParam );
				bProcessed |= m_LightControl[1].HandleMessages( hWnd, msg, wParam, lParam );
				bProcessed |= m_pClipPlane->HandleMessages( hWnd, msg, wParam, lParam );
				 
				CSelectionDisplay * selectionDisplay = GetCurrentSelectionDisplay();
				if ( selectionDisplay )
				{
					if ( selectionDisplay->GetPropertyCommon()->m_bClipping1 == TRUE )
						bProcessed |= selectionDisplay->m_pClipPlane1->HandleMessages(m_hWnd, msg, wParam, lParam );

					if ( selectionDisplay->GetPropertyCommon()->m_bClipping2 == TRUE )
						bProcessed |= selectionDisplay->m_pClipPlane2->HandleMessages(m_hWnd, msg, wParam, lParam );
				}
				if ( m_pActivePDBRenderer )
					if ( bProcessed == FALSE )	bProcessed |= m_pActivePDBRenderer->HandleMessages( m_hWnd, msg, wParam, lParam );

				if ( bProcessed == TRUE )
					g_bRequestRender = TRUE; 

			}
			break;

		case WM_PAINT:
			if( m_bLoadingApp )
			{
				HDC hDC = GetDC( hWnd );
				TCHAR strMsg[MAX_PATH];
				wsprintf( strMsg, TEXT("Loading... Please wait") );
				RECT rct;
				GetClientRect( hWnd, &rct );
				DrawText( hDC, strMsg, -1, &rct, DT_CENTER|DT_VCENTER|DT_SINGLELINE );
				ReleaseDC( hWnd, hDC );

				bProcessed = TRUE;
			}
			break;
	}
    return CD3DApplication::MsgProc( hWnd, msg, wParam, lParam );
}


//-----------------------------------------------------------------------------
// Name: InvalidateDeviceObjects()
// Desc: Invalidates device objects.  Paired with RestoreDeviceObjects()
//-----------------------------------------------------------------------------
HRESULT CProteinVistaRenderer::InvalidateDeviceObjects()
{
    // Cleanup any objects created in RestoreDeviceObjects()
	SAFE_RELEASE(m_pTexFinalRenderTarget);
	SAFE_RELEASE(m_pSurfaceMultiSampleRenderTargetColor);       // Multi-Sample float render target
	SAFE_RELEASE(m_pSurfaceMultiSampleDepthStencil);       // Depth Stencil surface for the float RT

	SAFE_RELEASE(m_pTexRenderColor);
	SAFE_RELEASE(m_pTexRenderNormal);
	SAFE_RELEASE(m_pTexRenderDepth);
	SAFE_RELEASE(m_pTexRenderSSAO);
	SAFE_RELEASE(m_pTexRenderBlur);

    m_pFont->InvalidateDeviceObjects();
	if (m_pEffectBasicShading)	m_pEffectBasicShading->OnLostDevice();

	m_pSkyManager->InvalidateDeviceObjects();
	for ( int i = 0 ; i < m_arrayPDBRenderer.size(); i++ )
	{
		m_arrayPDBRenderer[i]->InvalidateDeviceObjects();
	}

	CHTMLListCtrl * phtmlListCtrl = GetMainActiveView()->GetSelectList();
	for ( int i = 0 ; i < phtmlListCtrl->GetItemCount() ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *)(phtmlListCtrl->GetItemData(i));

		if ( pSelectionDisplay )
		{
			pSelectionDisplay->InvalidateDeviceObjects();
		}
	}

    CDXUTDirectionWidget::StaticOnLostDevice();

    return S_OK;
}

//-----------------------------------------------------------------------------
// Name: DeleteDeviceObjects()
// Desc: Paired with InitDeviceObjects()
//       Called when the app is exiting, or the device is being changed,
//       this function deletes any device dependent objects.  
//-----------------------------------------------------------------------------
HRESULT CProteinVistaRenderer::DeleteDeviceObjects()
{
    // Cleanup any objects created in InitDeviceObjects()
    m_pFont->DeleteDeviceObjects();

	for ( int i = 0 ; i < MAX_DISPLAY_SELECTION_INDEX ; i++ )
	{
		if ( m_arraySelectionDisplay[i] )
			m_arraySelectionDisplay[i]->DeleteDeviceObjects();
	}

	for ( int i = 0 ; i < 3 ; i++ )
	{
		SAFE_RELEASE(m_pMeshAtomVertexBuffer[i]);
		SAFE_RELEASE(m_pMeshAtomIndexBuffer[i]);
	}

	SAFE_RELEASE(m_pTexNoise);  

	SAFE_RELEASE(m_pD3DXMeshBond);

	SAFE_RELEASE(m_pDeclAtomSphere);
	SAFE_RELEASE(m_pDeclBond);
	SAFE_RELEASE(m_pDeclLine);

	SAFE_RELEASE(m_pD3DXTextureRibbon);
	SAFE_RELEASE(m_pD3DXTextureRibbonHelix);
	SAFE_RELEASE(m_pD3DXTextureRibbonSheet);

	SAFE_RELEASE(m_pEffectBasicShading);

	m_pSkyManager->DeleteDeviceObjects();

	for ( long i = 0 ; i < m_arrayPDBRenderer.size(); i++ )
	{
		m_arrayPDBRenderer[i]->DeleteDeviceObjects();
	}

	CHTMLListCtrl * phtmlListCtrl = GetMainActiveView()->GetSelectList();
	for ( int i = 0 ; i < phtmlListCtrl->GetItemCount() ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *)(phtmlListCtrl->GetItemData(i));

		if ( pSelectionDisplay )
		{
			pSelectionDisplay->DeleteDeviceObjects();
		}
	}

	CDXUTDirectionWidget::StaticOnDestroyDevice();

	m_CoordinateAxisDisplay.DeleteDeviceObjects();
	m_pClipPlane->DeleteDeviceObjects();

	CPropertyRibbon propertyRibbon(NULL);	//	값을 얻기 위해 임시로 만든다.
	ReleaseTexture(propertyRibbon.m_strTextureFilenameCoil);
	ReleaseTexture(propertyRibbon.m_strTextureFilenameHelix);
	ReleaseTexture(propertyRibbon.m_strTextureFilenameSheet);
	ReleaseTexture(m_pSkyManager->m_strTextureFilename);

	m_pFontTextureContainer->DeleteDeviceObjects();

    return S_OK;
}

//-----------------------------------------------------------------------------
// Name: FinalCleanup()
// Desc: Paired with OneTimeSceneInit()
//       Called before the app exits, this function gives the app the chance
//       to cleanup after itself.
//-----------------------------------------------------------------------------
HRESULT CProteinVistaRenderer::FinalCleanup()
{
    // Perform any final cleanup needed
	CPropertyRibbon propertyRibbon(NULL);
	ReleaseTexture(propertyRibbon.m_strTextureFilenameCoil);
	ReleaseTexture(propertyRibbon.m_strTextureFilenameHelix);
	ReleaseTexture(propertyRibbon.m_strTextureFilenameSheet);
	if ( m_pSkyManager )
		ReleaseTexture(m_pSkyManager->m_strTextureFilename);

    if ( m_pSkyManager )
		m_pSkyManager->FinalCleanup();
	for ( int i = 0 ; i < m_arrayPDBRenderer.size(); i++ )
	{
		m_arrayPDBRenderer[i]->FinalCleanup();
	}

	//
	//	여기 안에서 VB, IB 를 delete 하는것이 있다.
	//	
	//
	for ( int i = 0 ; i < MAX_DISPLAY_SELECTION_INDEX ; i++ )
	{
		SAFE_DELETE(m_arraySelectionDisplay[i]);
	}

	m_arraySelectionDisplay.clear();
	m_arraySelectionDisplay.resize(MAX_DISPLAY_SELECTION_INDEX);

	SAFE_DELETE( m_pFont );
	SAFE_DELETE(m_pSkyManager);

	for ( int i = 0 ; i < m_arrayPDBRenderer.size(); i++ )
	{
		delete m_arrayPDBRenderer[i];
	}

	m_arrayPDBRenderer.clear();

	m_pLastPickObjectInst = NULL;

	for ( long i = 0 ; i < m_arrayProteinSurface.size() ; i++ )
	{
		SAFE_DELETE(m_arrayProteinSurface[i]);
	}
	m_arrayProteinSurface.clear();

	for ( long i = 0 ; i < m_arrayProteinRibbonVertexData.size() ; i++ )
	{
		SAFE_DELETE(m_arrayProteinRibbonVertexData[i]);
	}
	m_arrayProteinRibbonVertexData.clear();

    return S_OK;
}

HRESULT CProteinVistaRenderer::SetViewVolume()
{
	FLOAT nearPlane;
	FLOAT farPlane;

	nearPlane = m_fNearClipPlane;
	farPlane = m_fFarClipPlane;

	m_fAspectRatio = (FLOAT)m_d3dsdBackBuffer.Width / (FLOAT)m_d3dsdBackBuffer.Height;

    // For the projection matrix, we set up a perspective transform (which
    // transforms geometry from 3D view space to 2D viewport space, with
    // a perspective divide making objects smaller in the distance). To build
    // a perpsective transform, we need the field of view (1/4 pi is common),
    // the aspect ratio, and the near and far clipping planes (which define at
    // what distances geometry should be no longer be rendered).
	if ( m_pPropertyScene->m_cameraType == m_pPropertyScene->CAMERA_OTHO )
	{
		//	TODO: check fovY in othorgonal projection 
		m_fovY = 1.0f;
		//	ratio.	1..100. 50 is ratio 1
		float ratio = m_pPropertyScene->m_othoCameraViewVol / 600.0f;
		D3DXMatrixOrthoLH ( &m_matrixProj ,	m_d3dsdBackBuffer.Width * ratio , 
											m_d3dsdBackBuffer.Height* ratio , 
											nearPlane, farPlane );
	}
	else
	{
		m_fovY = (m_pPropertyScene->m_lFOV/180.0f) * D3DX_PI;
		D3DXMatrixPerspectiveFovLH( &m_matrixProj , m_fovY , m_fAspectRatio, nearPlane, farPlane );
	}

    return m_pd3dDevice->SetTransform( D3DTS_PROJECTION, &m_matrixProj  );
}

//==================================================================================
 

//==================================================================================
//==================================================================================
//
//	display screen
//	
HRESULT	CProteinVistaRenderer::SetCameraAnimation(CAtomInst * pAtom , FLOAT lastingTime )
{
	CSTLArrayAtomInst arrayAtom;
	arrayAtom.push_back(pAtom);

	return SetCameraAnimation(arrayAtom, lastingTime );
}

HRESULT	CProteinVistaRenderer::SetCameraAnimation(CResidueInst * pResidue, FLOAT lastingTime )
{
	return SetCameraAnimation(pResidue->m_arrayAtomInst , lastingTime );
}

HRESULT	CProteinVistaRenderer::SetCameraAnimation(CSTLArrayAtomInst &arrayAtom , FLOAT lastingTime )
{
	D3DXVECTOR3 posTarget;
	FindAnimationTargetPos( arrayAtom , posTarget );

	SetCameraAnimation( posTarget, lastingTime );

	return S_OK;
}

HRESULT	CProteinVistaRenderer::FindAnimationTargetPos( _IN_ CSTLArrayAtomInst &arrayAtomInst, _OUT_ D3DXVECTOR3 &posTarget )
{
	D3DXVECTOR3 minBB(1e20, 1e20, 1e20);
	D3DXVECTOR3 maxBB(-1e20, -1e20, -1e20);

	for ( int i =0 ; i < arrayAtomInst.size(); i++ )
	{
		CAtomInst * pAtomInst = arrayAtomInst[i];
		D3DXMATRIXA16 * pMatWorld = pAtomInst->m_pPDBInst->m_pPDBRenderer->GetWorldMatrix();

		D3DXVECTOR3 pos;
		D3DXVec3TransformCoord( &pos, &(pAtomInst->GetAtom()->m_pos), pMatWorld );

		if (maxBB.x < pos.x) maxBB.x = pos.x;
		if (maxBB.y < pos.y) maxBB.y = pos.y;
		if (maxBB.z < pos.z) maxBB.z = pos.z;
		if (minBB.x > pos.x) minBB.x = pos.x;
		if (minBB.y > pos.y) minBB.y = pos.y;
		if (minBB.z > pos.z) minBB.z = pos.z;
	}

	D3DXVECTOR3		center = (minBB + maxBB)/2.0f;
	if ( center.x < 0.0001f && center.y < 0.0001f && center.z < 0.0001f )
	{
		center.x = 0.0f;
		center.y = 0.0f;
		center.z = -0.001f;
	}

	FLOAT radius = D3DXVec3Length( &D3DXVECTOR3(minBB-center) );
	radius = max( 3.0f, radius );

	//	
	//	float cameraDelta = GetCameraZPosInSphere(center, radius);

	D3DXVECTOR3	centerNorm;
	D3DXVec3Normalize(&centerNorm, &center);

	posTarget = center + centerNorm * radius * 3;
 
	return S_OK;
}

HRESULT	CProteinVistaRenderer::SetCameraAnimation(D3DXVECTOR3 &endPos, FLOAT lastingTime )
{
	if ( m_bAnimationing == TRUE )
		return E_FAIL;

	m_bAnimationing = TRUE;

	//	time으로 줬을때, fps 와 곱해서 총 frame 수를 만들어낸다.
	long defaultAnimationFPS = 33;
	long cameraAnimationFrame = (lastingTime * defaultAnimationFPS);

	GenerateCameraAnimationPos(endPos, cameraAnimationFrame);

	BOOL	bCameraAnimation = TRUE;
	long	iCameraAnimationCurrent = 0;

	FLOAT fAppTimeOld = 0.0f;
	while(1)
	{
		if ( bCameraAnimation == FALSE )
			break;

		FLOAT fAppTime = DXUtil_Timer( TIMER_GETAPPTIME );

		if ( fAppTime - fAppTimeOld > 0.033f )
		{
			D3DXVECTOR3 animationPos = m_animationPosArray[iCameraAnimationCurrent];
			SetCameraPos(animationPos);

			iCameraAnimationCurrent++;

			fAppTimeOld = fAppTime;

			Render3DEnvironment();

			if ( iCameraAnimationCurrent >= m_animationPosArray.size() )
			{
				bCameraAnimation = FALSE;
				m_animationPosArray.clear();
			}
		}
		
		PumpMessage();
	}

	Render3DEnvironment();

	m_bAnimationing = FALSE;

	return S_OK;
}

HRESULT CProteinVistaRenderer::SetCameraAnimation()
{
	CSTLArrayAtomInst atomSelected;
	atomSelected.reserve(2000);

	for(int nPDB = 0 ; nPDB < m_arrayPDBRenderer.size(); nPDB++)
	{
		CPDBInst * pPDBInst = m_arrayPDBRenderer[nPDB]->GetPDBInst();

		//	선택된것을 얻은 다음에 
		CSTLArraySelectionInst selection;
		selection.reserve(100);
		pPDBInst->GetSelectNodeChild(selection);
		for ( int j = 0 ; j < selection.size() ; j++ )
		{
			selection[j]->GetChildAtoms(atomSelected);
		}
	}

	if ( atomSelected.size() == 0 )
	{
		OutputTextMsg (_T("There is no checked items"));
	}
	else
	{
		FLOAT animationTime = 0.0f;
		if ( m_pPropertyScene->m_bDoubleClockToCameraAnimation == TRUE )
			animationTime = m_pPropertyScene->m_fAnimationTime;
		else
			animationTime = 0.0f;

		SetCameraAnimation(atomSelected , animationTime);
	}

	return S_OK;
}

//===============================================================================================
//===============================================================================================

HRESULT CProteinVistaRenderer::GenerateCameraAnimationPos(D3DXVECTOR3 &endPos, long frame)
{
	CSTLArrayD3DXVECTOR3 arrayPos;

	if ( frame > 0 )
	{
		HRESULT GetCardianlCurvePoint( _IN_ CSTLArrayD3DXVECTOR3 & arrayCtrlPoint, _OUT_ CSTLArrayD3DXVECTOR3 & m_arrayVertex , _IN_ long seg , _IN_ float tension );
		CSTLArrayD3DXVECTOR3 arrayCtrlPoint;

		arrayCtrlPoint.push_back(D3DXVECTOR3(0,0,0));
		arrayCtrlPoint.push_back(m_FromVec);
		arrayCtrlPoint.push_back(endPos);
		arrayCtrlPoint.push_back(D3DXVECTOR3(0,0,0));

		GetCardianlCurvePoint( arrayCtrlPoint, arrayPos, frame , 2.0 );
	}
	else
	{
		arrayPos.push_back(endPos);
	}

	m_animationPosArray.clear();
	m_animationPosArray.reserve( frame );

	for ( int i = 0 ; i <= frame ; i++ )
	{
		m_animationPosArray.push_back(arrayPos[frame+i]);
	}
	return TRUE;
}

void CProteinVistaRenderer::SetCameraPos(D3DXVECTOR3 &pos)
{
	D3DXMATRIX MatTransform;
	D3DXVECTOR3 Eye(0,0,0);
	D3DXVECTOR3 At(-pos.x, -pos.y, -pos.z);
	D3DXVECTOR3 Up(0,1,0);

	D3DXMatrixLookAtLH( &MatTransform, &Eye, &At, &Up );

	m_matCameraRotation = MatTransform;
	m_cameraZPos = -D3DXVec3Length(&At);
	D3DXMatrixIdentity(&m_matCameraRotationTemp);

}

//==================================================================================
//==================================================================================
CPDBRenderer * CProteinVistaRenderer::AddPDBRenderer(CPDB * pPDB)
{
	CPDBRenderer * pPDBRenderer = new CPDBRenderer();
	pPDBRenderer->Init(this, pPDB);

	m_arrayPDBRenderer.push_back(pPDBRenderer);

	//	초기화.
	pPDBRenderer->OneTimeSceneInit();
	pPDBRenderer->InitDeviceObjects();
	pPDBRenderer->RestoreDeviceObjects();

	return pPDBRenderer;
}

HRESULT	CProteinVistaRenderer::DeletePDBRenderer(int nIndex)
{
	if ( m_arrayPDBRenderer.size() >= 2 )
	{
		CPDBRenderer * pPDBRenderer = m_arrayPDBRenderer[nIndex];
		m_arrayPDBRenderer.erase( m_arrayPDBRenderer.begin() + nIndex );

		pPDBRenderer->InvalidateDeviceObjects();
		pPDBRenderer->DeleteDeviceObjects();
		delete pPDBRenderer;
	}
	
	return S_OK;
}

HRESULT	CProteinVistaRenderer::DeletePDBRenderer(CPDBRenderer * pPDBRendererDelete)
{
	for ( int i = 0 ; i < m_arrayPDBRenderer.size() ; i++ )
	{
		CPDBRenderer * pPDBRenderer = m_arrayPDBRenderer[i];
		if ( pPDBRenderer == pPDBRendererDelete )
		{
			m_arrayPDBRenderer.erase (m_arrayPDBRenderer.begin() + i );

			pPDBRenderer->InvalidateDeviceObjects();
			pPDBRenderer->DeleteDeviceObjects();
			pPDBRenderer->FinalCleanup();
			delete pPDBRenderer;

			break;
		}
	}

	return S_OK;
}

//==================================================================================
//	AtomSelectionList 가 Add 되거나 Remove 되었을때 callback으로 불린다.
void CProteinVistaRenderer::SetAtomSelectionAddRemoved()
{
	//SetSelectionMaxRadius();
	//m_pClipPlane->InitRadius(m_radius);
}
#pragma managed(push,off)
float CProteinVistaRenderer::GetCameraZPosInSphere(D3DXVECTOR3 center, FLOAT radius)
{
	//	center가 0 이면 조정
	//	if ( center == D3DXVECTOR3(0,0,0) )	center = D3DXVECTOR3(0,0.0001f,0);
	if ( D3DXVec3Length(&center) < 0.0001f ) center = D3DXVECTOR3(0,0.0001f,0);
	
	//	center랑 camera랑 벡터가 같으면 조정
	FLOAT cosTheta = D3DXVec3Dot(&center, &m_FromVec)/(D3DXVec3Length(&center)*D3DXVec3Length(&m_FromVec));;
	if (  cosTheta >= 0.9999f || cosTheta <= -0.9999f )
	{
		D3DXVECTOR3	vecFronNorm;
		D3DXVec3Normalize(&vecFronNorm,&m_FromVec);
		vecFronNorm.y += 1.0f;
		center += vecFronNorm*0.0001f;
	}

	D3DXMATRIXA16 matRotInv;
	D3DXMatrixInverse(&matRotInv, NULL, &m_matCameraRotation);

	D3DXVECTOR3 vUp( 0, 1, 0 );
	D3DXVec3TransformCoord ( &vUp, &vUp, &matRotInv );

	//	view vector를 구해서..
	D3DXMATRIXA16	rotCamera;
	D3DXMatrixLookAtLH(&rotCamera, &D3DXVECTOR3(0,0,0), &m_FromVec, &vUp );

	D3DXVECTOR3		centerTransformed;
	D3DXVec3TransformCoord(&centerTransformed, &center, &rotCamera);

	//	xy 바꾸기.
	D3DXVECTOR3		centerTransformed2 = centerTransformed;
	centerTransformed2.x = -centerTransformed2.x;
	centerTransformed2.y = -centerTransformed2.y;

	D3DXVECTOR3 &center1 = centerTransformed;
	D3DXVECTOR3 &center2 = centerTransformed2;

	D3DXVECTOR3	vec12Norm = (center1-center2);
	D3DXVec3Normalize(&vec12Norm, &vec12Norm);
	D3DXVECTOR3	centerBB1 = center1 + vec12Norm * radius;
	D3DXVECTOR3	centerBB2 = center2 - vec12Norm * radius;

	//	z 값은 항상 음수.
	return -GetCameraZPosinTwoPoint(centerBB1, centerBB2);

 

}
 #pragma managed(pop)
//	
float CProteinVistaRenderer::GetCameraZPosinTwoPoint(D3DXVECTOR3 minBB, D3DXVECTOR3 maxBB)
{
	float fCamFovX = m_fovY * m_fAspectRatio;
 

	return  max(minBB.z, maxBB.z)+  
				max( max( abs(minBB.x)/tan(fCamFovX/2), abs(maxBB.x)/tan(fCamFovX/2) ), 
					 max( abs(minBB.y)/tan(m_fovY/2), abs(maxBB.y)/tan(m_fovY/2) ) ) ;

}

void CProteinVistaRenderer::CameraViewAll()
{
	FLOAT	farZPos = 0.0f;
	BOOL	bViewAll = FALSE;
	CHTMLListCtrl * phtmlListCtrl = GetMainActiveView()->GetSelectList();
	for ( int i = 0 ; i < phtmlListCtrl->GetItemCount() ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *)(phtmlListCtrl->GetItemData(i));

		if ( pSelectionDisplay )
		{
			if ( pSelectionDisplay->m_bSelect == TRUE )
			{
				D3DXVECTOR3 center = pSelectionDisplay->m_pPDBRenderer->m_selectionCenter;
				D3DXVec3TransformCoord(&center, &center, &(pSelectionDisplay->m_pPDBRenderer->m_matWorld));
				m_cameraZPos = GetCameraZPosInSphere(center, pSelectionDisplay->m_pPDBRenderer->m_selectionRadius );

				m_cameraZPos = min(m_cameraZPos, -3.0f);
				bViewAll = TRUE;
				break;
			}
		}
	}

	if ( bViewAll == FALSE )
	{	//	selection에서 viewall 이 되지 않았을 경우,
		//	모든 PDB 에 대해서 viewAll을 한다.
		FLOAT cameraZPos = 1e20;
		for ( int i = 0 ; i < m_arrayPDBRenderer.size(); i++ )
		{
			CPDBRenderer * pPDBRenderer = m_arrayPDBRenderer[i];
			D3DXVECTOR3 center = pPDBRenderer->GetPDBInst()->GetPDB()->m_pdbCenter;
			FLOAT radius = pPDBRenderer->GetPDBInst()->GetPDB()->m_pdbRadius;

			D3DXVec3TransformCoord(&center, &center, &(pPDBRenderer->m_matWorld));

			FLOAT zPos = GetCameraZPosInSphere(center, radius );
			if ( zPos < cameraZPos )
				cameraZPos = zPos;
		}
		m_cameraZPos = min(cameraZPos, -3.0f);
	}

	//
	//	light positon을 위한 m_radius, m_center 찾기.
	//
	D3DXVECTOR3 minBB(1e20, 1e20, 1e20);
	D3DXVECTOR3 maxBB(-1e20, -1e20, -1e20);

	for ( int i = 0 ; i < m_arrayPDBRenderer.size(); i++ )
	{
		CPDBRenderer * pPDBRenderer = m_arrayPDBRenderer[i];

		for ( int iBB = 0 ; iBB < 2 ; iBB ++ )
		{
			D3DXVECTOR3 pos = pPDBRenderer->GetPDBInst()->GetPDB()->m_pdbMinMaxBB[iBB];
			D3DXVec3TransformCoord(&pos, &pos, &(pPDBRenderer->m_matWorld));
			if (maxBB.x < pos.x) maxBB.x = pos.x;
			if (maxBB.y < pos.y) maxBB.y = pos.y;
			if (maxBB.z < pos.z) maxBB.z = pos.z;
			if (minBB.x > pos.x) minBB.x = pos.x;
			if (minBB.y > pos.y) minBB.y = pos.y;
			if (minBB.z > pos.z) minBB.z = pos.z;
		}
	}

	m_center = (minBB + maxBB)/2.0f;
	m_radius = D3DXVec3Length( &D3DXVECTOR3(minBB-m_center) );

	m_lightRadius = D3DXVec3Length(&m_center) + m_radius * 0.6f;

	//	Setting light radius
	//m_pPropertyScene->m_pItemLight1Radius->SetDouble(m_lightRadius);
	//m_pPropertyScene->m_pItemLight1Radius->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(m_pPropertyScene->m_pItemLight1Radius)) ;

	//m_pPropertyScene->m_pItemLight2Radius->SetDouble(m_lightRadius);
	//m_pPropertyScene->m_pItemLight2Radius->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(m_pPropertyScene->m_pItemLight2Radius)) ;

	g_bRequestRender = TRUE;
}

void	CProteinVistaRenderer::SelectAll(BOOL bSelect)
{
	for ( int i = 0 ; i < m_arrayPDBRenderer.size() ; i++ )
	{
		CPDBInst * pPDBInst = m_arrayPDBRenderer[i]->GetPDBInst();
		pPDBInst->SetSelectChild(bSelect);
	}
}

void	CProteinVistaRenderer::SelectChildren(CProteinObjectInst * pProteinObject, BOOL bSelect)
{
	SelectChildrenRecursive(pProteinObject, bSelect);
}

void	CProteinVistaRenderer::SelectChildrenRecursive(CProteinObjectInst * pProteinObject, BOOL bSelect)
{
	if ( pProteinObject->GetSelect() == bSelect )
		return;
	
	pProteinObject->SetSelectChild(bSelect);
}

void	CProteinVistaRenderer::GetSelectedObject(CSTLArraySelectionInst &selection)
{
	selection.reserve(2000);

	for ( int i = 0 ; i < m_arrayPDBRenderer.size() ; i++ )
	{
		m_arrayPDBRenderer[i]->GetSelectedObject(selection);
	}
}

//	항상 앞에부터 index를 준다.
long CProteinVistaRenderer::GetNewIndexSelectionDisplay()
{
	for ( int i = 0 ; i < MAX_DISPLAY_SELECTION_INDEX ; i++ )
	{
		if ( m_arraySelectionDisplay[i] == NULL )
			return i;
	}

	return -1;
}

long CProteinVistaRenderer::GetMaxIndexSelectionDisplay()
{
	for ( int i = MAX_DISPLAY_SELECTION_INDEX-1 ; i >= 0 ; i-- )
	{
		if ( m_arraySelectionDisplay[i] != NULL )
			return i;
	}

	return -1;
}

CSelectionDisplay * CProteinVistaRenderer::GetCurrentSelectionDisplay()
{
	int select = GetMainActiveView()->GetSelectList()->GetSelectedItem();
	if ( select == -1 )
		return NULL;
    CHTMLListCtrl * phtmlListCtrl = GetMainActiveView()->GetSelectList();
	CSelectionDisplay * selectionDisplay = (CSelectionDisplay *)(phtmlListCtrl->GetItemData(select));
	return selectionDisplay;
}


//
//
void CProteinVistaRenderer::SetShaderTechnique(long technique, int shader)
{
	//    0.1.2.3.4
	//    long shader = ( m_pPropertyScene->m_enumModelQuality <= 2)? 1: 0 ;
	m_pEffectBasicShading->SetTechnique( m_hIlluminationModel[shader][technique] );
}

//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
void	CProteinVistaRenderer::DeSelectAllAtoms()
{
	//	CPDB 안에 있는 m_bSelected 를 설정.
	SelectAll(FALSE);

	//	pdb 에서 select 된것을 Pane 에 반영
	UpdateSelectionInfoPane();

	//	selection List pane을 deselect.
	GetMainActiveView()->GetSelectPanel()-> DeselectPaneItem();

	//	렌더러에 반영
	UpdateAtomSelectionChanged();

	g_bRequestRender = TRUE;
}

void	CProteinVistaRenderer::SelectedAtomApply()
{
	//	Hierarchy 조정
	//((CMainFrame*)AfxGetMainWnd())->m_pPDBTreePane->OnFullSyncPDB();

	//	pdb 에서 select 된것을 렌더러에 반영
	UpdateAtomSelectionChanged();

	//	pdb 에서 select 된것을 Tree control 과 residue grid Pane 에 반영
	UpdateSelectionInfoPane();

	g_bRequestRender = TRUE;
}

//	선택된 것이 변경되었을때, Instancing Rendering data의 색깔을 바꾸어준다.
void	CProteinVistaRenderer::UpdateAtomSelectionChanged()
{
	//	여기에 InitProgress 를 사용하면 안된다.
	//	re-enterent 가 된다.
	for ( int i = 0 ; i < m_arraySelectionDisplay.size() ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = m_arraySelectionDisplay[i];
		if ( pSelectionDisplay == NULL )
			continue;

		pSelectionDisplay->UpdateAtomSelectionChanged();
	}

}

//
//	Residue tree 와 residue list 를 반영.
//
void CProteinVistaRenderer::UpdateSelectionInfoPane()
{
	GetMainActiveView()->GetCPDBTreePane()->UpdateTreeCtrlFromPDBSelection();
	GetMainActiveView()->GetResiduePanel()->UpdateResidueFromPDBSelection();
}


//////////////////////////////////////////////////////////////////////////

HRESULT CProteinVistaRenderer::TransformAToB(CSelectionDisplay* proteinA, CSelectionDisplay* proteinB, D3DXMATRIXA16 &matTransform)
{
	if ( proteinA->m_pPDBRenderer == proteinB->m_pPDBRenderer )
		return E_INVALIDARG;

	TransformAToB ( proteinA->m_pPDBRenderer, proteinB->m_pPDBRenderer, matTransform , NULL /*proteinA->m_center*/ );
	return S_OK;
}
#pragma managed(push,off)
//	
//	pdb1 을 pdb2 에 맞춘다.
//  center로 pPDBRenderer1 을 이동 -> matTransform 적용-> pPDBRenderer2 로 이동.
//	
HRESULT CProteinVistaRenderer::TransformAToB(CPDBRenderer * pPDBRenderer1, CPDBRenderer * pPDBRenderer2, D3DXMATRIXA16 &matTransform, D3DXVECTOR3 *pCenter )
{
	if ( pPDBRenderer1 == pPDBRenderer2 )
		return E_INVALIDARG;

	D3DXVECTOR3 center;
	if ( pCenter == NULL )
		center.x = center.y = center.z = 0.0f;
	else
		center = *pCenter;

	D3DXMATRIXA16 matCenter;
	D3DXMatrixIdentity(&matCenter);
	matCenter._41 = -center.x;
	matCenter._42 = -center.y;
	matCenter._43 = -center.z;

	D3DXMATRIXA16 matCenter2;
	D3DXMatrixIdentity(&matCenter2);
	matCenter2._41 = center.x;
	matCenter2._42 = center.y;
	matCenter2._43 = center.z;

	//	pdb1을 center로 -> center를 적용하여 matTransform 적용 ->pPDBRenderer2->m_matWorld 로.
	//	pdb1의 matOld는 Identity, 
	D3DXMATRIXA16 matTransformResult;
	matTransformResult = matCenter * matTransform * matCenter2 * pPDBRenderer2->m_matWorld;

	pPDBRenderer1->m_matWorldPrevious = matTransformResult;

	//	center 는 그대로.
	//	
	//	m_matWorldRotCenter 는 변하지 않는다.
	//	

	return S_OK;
}
#pragma managed(pop)
//
//	맨처음 load 되었을 경우에 전체를 선택하여 selection list 에 넣고
//	중심으로 이동한다.
//
long	CProteinVistaRenderer::AddInitialSelection(CPDBRenderer * pPDBRenderer, long mode)
{
	//	전체를 deselect 한다음.
	DeSelectAllAtoms();

	//	현재꺼만 select 한다.
	SelectChildren(pPDBRenderer->GetPDBInst(), TRUE);

	long indexSelection = -1;
	CSelectionDisplay * pSelectionDisplay = AddCurrentSelection(mode, pPDBRenderer);
	if ( pSelectionDisplay )
		indexSelection = pSelectionDisplay->m_iDisplayStylePDB;

	return indexSelection;
}

CSelectionDisplay * CProteinVistaRenderer::AddCurrentSelection(long mode, CPDBRenderer * pPDBRendererAddSelection )
{
	CSelectionDisplay * pSelectionDisplay = NULL;

	CSTLArrayPDBRenderer	arrayPDBRendererTemp;		//	PDBRenderer Array
	if ( pPDBRendererAddSelection != NULL )
	{	//	only one
		arrayPDBRendererTemp.push_back(pPDBRendererAddSelection);
	}
	else
	{	//	pointer copy.
		arrayPDBRendererTemp = m_arrayPDBRenderer;
	}
	for ( int i = 0 ; i < arrayPDBRendererTemp.size() ; i++ )
	{
		CPDBRenderer * pPDBRenderer = arrayPDBRendererTemp[i];
		CSTLArraySelectionInst selection;
		pPDBRenderer->GetSelectedObject(selection);
		if ( selection.size() > 0 )
		{	//	선택된것이 존재하면...
			long indexSelection = GetNewIndexSelectionDisplay();
			if ( indexSelection != -1 )
			{
				pSelectionDisplay = CSelectionDisplay::CreateSelectionDisplay(mode);
 
				pSelectionDisplay->m_iDisplayStylePDB = indexSelection;
				pSelectionDisplay->m_pProteinVistaRenderer = this;
				pSelectionDisplay->m_pPDBRenderer = pPDBRenderer;

				pSelectionDisplay->InitDisplayStyleProperty(mode);
	 
				pSelectionDisplay->SetSelection(selection);
				pSelectionDisplay->FindCenterRadius();

				//	CPDB 에 설정을 한다.
				for ( int i = 0 ; i < selection.size(); i++ )
				{
					selection[i]->SetDisplayStyleChild(indexSelection, TRUE);
				}
 
				//	컨테이너에 넣는다.
				m_arraySelectionDisplay[indexSelection] = pSelectionDisplay;
				m_maxIndexSelectionDisplay = GetMaxIndexSelectionDisplay();

				//	렌더링 설정을 한다.
				pSelectionDisplay->InitRenderSceneSelection();	
		 
				pSelectionDisplay->InitDeviceObjects(); 
				 
				pSelectionDisplay->RestoreDeviceObjects();

				GetMainActiveView()->UpdateAllPanes();
			}
			else
			{
				CString strMsg;
				strMsg.Format("The number of maximum selection list is %d", MAX_DISPLAY_SELECTION_INDEX);
				OutputTextMsg(strMsg);
			}
		}
	}
	//	
	if ( pSelectionDisplay )
		SetAtomSelectionAddRemoved();
	return pSelectionDisplay;
}

void CProteinVistaRenderer::SetFog()
{
	if ( m_pd3dDevice == NULL )		return;

	//	fog Enable/Disable 은 여기서 조정이 가능.
	if ( m_shaderVersion >= 3 )
	{
		m_pd3dDevice->SetRenderState(D3DRS_FOGENABLE, FALSE);

		//	fog color
		//m_pEffectBasicShading->SetValue(m_hFogColor, &(COLORREF2D3DXCOLOR(m_pPropertyScene->m_fogColor)), sizeof(D3DXCOLOR)); 

		FLOAT fogStart = m_fNearClipPlane + (m_fFarClipPlane-m_fNearClipPlane)*m_pPropertyScene->m_fogStart/100.0f;
		FLOAT fogEnd = m_fNearClipPlane + (m_fFarClipPlane-m_fNearClipPlane)*m_pPropertyScene->m_fogEnd/100.0f;

		FLOAT	fogParam[3];
		fogParam[0] = (FLOAT)m_pPropertyScene->m_bDepthOfField;
		fogParam[1] = fogEnd/(fogEnd-fogStart);
		fogParam[2] = 1/(fogEnd-fogStart);

		//m_pEffectBasicShading->SetValue(m_hFogParamSM3, fogParam, sizeof(FLOAT)*3 );
	}
	else
	{
		m_pd3dDevice->SetRenderState(D3DRS_FOGENABLE, m_pPropertyScene->m_bDepthOfField);

		if ( m_pPropertyScene->m_bDepthOfField == TRUE )
		{
			m_pd3dDevice->SetRenderState(D3DRS_FOGCOLOR, COLORREF2D3DXCOLOR(m_pPropertyScene->m_fogColor) );

			FLOAT fogStart = m_fNearClipPlane + (m_fFarClipPlane-m_fNearClipPlane)*m_pPropertyScene->m_fogStart/100.0f;
			FLOAT fogEnd = m_fNearClipPlane + (m_fFarClipPlane-m_fNearClipPlane)*m_pPropertyScene->m_fogEnd/100.0f;

			//	fog value.	
			FLOAT vFog[2];
			vFog[0] = fogEnd/(fogEnd-fogStart);
			vFog[1] = 1/(fogEnd-fogStart);

			SetShaderFogParam(vFog);
		}
	}
}

void CProteinVistaRenderer::SetFogEnable(BOOL bForceEnable)
{
	if ( bForceEnable == FALSE )			//	강제적으로 fog를 끈다. fog 가 적용안되는 mesh를 그린다.
		m_pd3dDevice->SetRenderState(D3DRS_FOGENABLE, FALSE );
	else
		m_pd3dDevice->SetRenderState(D3DRS_FOGENABLE, m_pPropertyScene->m_bDepthOfField);
}

void CProteinVistaRenderer::SetModelQuality()
{
	m_renderQualityPreset.SetModelQuality(m_pPropertyScene->m_modelQuality);

	//    모든 selection에 대해 quality 변수를 바꿈.
	for ( int i = 0 ; i <= m_maxIndexSelectionDisplay; i++ )
	{
		if ( m_arraySelectionDisplay[i] )
		{
			//    notify event.
			/*CXTPPropertyGridItemEnum * item = m_arraySelectionDisplay[i]->GetPropertyCommon()->m_pItemEnumModelQuality;
			item->SetEnum(m_pPropertyScene->m_modelQuality);
			item->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(item));*/
			m_arraySelectionDisplay[i]->SetPropertyChanged(PROPERTY_COMMON_MODEL_QUALITY);
		}
	}
}

void CProteinVistaRenderer::SetShaderQuality()
{
	m_renderQualityPreset.SetShaderQuality(m_pPropertyScene->m_shaderQuality);

	//    모든 selection에 대해 quality 변수를 바꿈.
	for ( int i = 0 ; i <= m_maxIndexSelectionDisplay; i++ )
	{
		if ( m_arraySelectionDisplay[i] )
		{
			/*CXTPPropertyGridItemEnum * item = m_arraySelectionDisplay[i]->GetPropertyCommon()->m_pItemEnumShaderQuality;
			item->SetEnum(m_pPropertyScene->m_shaderQuality);
			item->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(item));*/
			//m_arraySelectionDisplay[i]->SetPropertyChanged(PROPERTY_COMMON_MODEL_QUALITY);
		}
	}
}

void CProteinVistaRenderer::SetShowSelectionMark()
{
	m_renderQualityPreset.SetShowSelectionMark(m_pPropertyScene->m_bShowSelectionMark);

	//    모든 selection에 대해 quality 변수를 바꿈.
	for ( int i = 0 ; i <= m_maxIndexSelectionDisplay; i++ )
	{
		if ( m_arraySelectionDisplay[i] )
		{
			/*CXTPPropertyGridItemBool * item = m_arraySelectionDisplay[i]->GetPropertyCommon()->m_pItemShowSelectionMark;
			item->SetBool(m_pPropertyScene->m_bShowSelectionMark);
			item->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(item));*/
			//m_arraySelectionDisplay[i]->SetPropertyChanged(PROPERTY_COMMON_MODEL_QUALITY);
		}
	}
}

//	
void CProteinVistaRenderer::SetSelectionColor()
{
	//	index 1 이 selection color 이다.
	m_finalRenderSelectionColor[1] = COLORREF2D3DXCOLOR(m_pPropertyScene->m_selectionColor);
	
	ApplyColorIndicateTable();
}

void CProteinVistaRenderer::ApplyColorIndicateTable() 
{
	m_pEffectBasicShading->SetVectorArray(m_hColorIndicate, (D3DXVECTOR4*) m_finalRenderSelectionColor, MAX_SELECTION_COLOR );
}



void CProteinVistaRenderer::SetAntialiasing()
{
	//
	CD3DSettings      m_d3dSettingsOld = m_d3dSettings;
	if ( m_pPropertyScene->m_iAntialiasing != CPropertyScene::AA_NONE )
	{	//	수정
		//	m_d3dSettings.SetMultisampleType(D3DMULTISAMPLE_16_SAMPLES);

		D3DFORMAT fmt = m_d3dSettings.DepthStencilBufferFormat();

		long	maxSampleType = 0;
		D3DMULTISAMPLE_TYPE	sampleType[30];
		// Build multisample list
		D3DDeviceCombo* pDeviceCombo = m_d3dSettings.PDeviceCombo();
		for( UINT ims = 0; ims < pDeviceCombo->pMultiSampleTypeList->Count(); ims++ )
		{
			D3DMULTISAMPLE_TYPE msType = *(D3DMULTISAMPLE_TYPE*)pDeviceCombo->pMultiSampleTypeList->GetPtr(ims);

			// check for DS/MS conflicts
			BOOL bConflictFound = FALSE;
			for( UINT iConf = 0; iConf < pDeviceCombo->pDSMSConflictList->Count(); iConf++ )
			{
				D3DDSMSConflict* pDSMSConf = (D3DDSMSConflict*)pDeviceCombo->pDSMSConflictList->GetPtr(iConf);
				if( pDSMSConf->DSFormat == fmt && pDSMSConf->MSType == msType )
				{
					bConflictFound = TRUE;
					break;
				}
			}
			if( !bConflictFound )
			{
				sampleType[maxSampleType] = msType;
				maxSampleType ++;
			}
		}

		int iSampleType = min(max(m_pPropertyScene->m_iAntialiasing+1, 0), maxSampleType-1);
		m_typeMultiSample = sampleType[iSampleType];
		m_d3dSettings.SetMultisampleType(m_typeMultiSample);
	}
	else
	{
		//	NO FSAA.
		m_d3dSettings.SetMultisampleType(D3DMULTISAMPLE_NONE);
	}

	//	아래 코드는 UserSelectNewDevice();
	//	에서 가져온것.
	HRESULT hr = S_OK;

	// Release all scene objects that will be re-created for the new device
	Cleanup3DEnvironment();

	// Inform the display class of the change. It will internally
	// re-create valid surfaces, a d3ddevice, etc.
	if( FAILED( hr = Initialize3DEnvironment() ) )
	{
		//	restore original status.
		m_d3dSettings = m_d3dSettingsOld;

		Cleanup3DEnvironment();
		Initialize3DEnvironment();
	}

	m_pd3dDevice->SetRenderState(D3DRS_MULTISAMPLEANTIALIAS, (m_pPropertyScene->m_iAntialiasing == CPropertyScene::AA_NONE)? FALSE: TRUE );
}

void CProteinVistaRenderer::SetShaderLight()
{
	if ( m_pPropertyScene->m_bLight1Use == TRUE )
	{
		SetShaderLight1Intensity(m_pPropertyScene->m_light1Intensity/100.0f);
	}
	else
		SetShaderLight1Intensity(0.0f);		//	shader에서 if 문을 없애기 위해 m_bLight1Use 가 FALSE 일때, Intensity를 0 으로 설정.
	SetShaderLight1Color(COLORREF2D3DXCOLOR(m_pPropertyScene->m_light1Color));

	if ( m_pPropertyScene->m_bLight2Use == TRUE )
	{
		SetShaderLight2Intensity(m_pPropertyScene->m_light2Intensity/100.0f);
	}
	else
		SetShaderLight2Intensity(0.0f);
	SetShaderLight2Color(COLORREF2D3DXCOLOR(m_pPropertyScene->m_light2Color));
}

void CProteinVistaRenderer::SetGlobalClipPlane()
{
	if ( m_pPropertyScene->m_bClipping0 == TRUE )
	{
		float dir0;
		if ( m_pPropertyScene->m_bClipDirection0 == TRUE )
			dir0 = 100.0f;
		else
			dir0 = -100.0f;
		SetShaderClipPlane0Dir(dir0);

		m_pClipPlane->GetPlaneEquation(&(m_pPropertyScene->m_clipPlaneEquation0));
		SetShaderClipPlane0(m_pPropertyScene->m_clipPlaneEquation0);

		m_pClipPlane->InitRadius(m_radius);
		m_pClipPlane->InitRenderParam(	m_pPropertyScene->m_bShowClipPlane0, 
										m_pPropertyScene->m_clipPlaneTransparency0, 
										COLORREF2D3DXCOLOR(m_pPropertyScene->m_clipPlaneColor0) );
	}
	else
	{
		D3DXPLANE plane(0,0,0,1);
		SetShaderClipPlane0(plane);

		SetShaderClipPlane0Dir(1.0f);
	}
}

void CProteinVistaRenderer::SetDisplayHETATM()
{
	 
}

void CProteinVistaRenderer::SetInitialDefaultSelection(CPDBRenderer * pPDBRenderer, int displayMethod)
{
	long indexSelection;
	indexSelection = AddInitialSelection( pPDBRenderer, displayMethod );

	if ( indexSelection != -1 )
	{
		GetMainActiveView()->GetSelectPanel()->SelectListItem(indexSelection);
	}
	pPDBRenderer->CenterPDBRenderer();
	CameraViewAll();
}

void CProteinVistaRenderer::AttatchBioUnit()
{
	m_pActivePDBRenderer->m_bAttatchBioUnit = !(m_pActivePDBRenderer->m_bAttatchBioUnit);

	if ( m_pActivePDBRenderer->m_bAttatchBioUnit == TRUE )
	{
		//	BioUnit 이 On 되면, Center를 새로 만든다.
		if ( m_pActivePDBRenderer->m_pPDBRendererParentBioUnit == NULL )
		{
			//	parent 일때.
			CPDBRenderer * pPDBRendererParent = m_pActivePDBRenderer;

			D3DXVECTOR3 minBB(1e20, 1e20, 1e20);
			D3DXVECTOR3 maxBB(-1e20, -1e20, -1e20);
			for ( int iChild = 0 ; iChild < pPDBRendererParent->m_arrayPDBRendererChildBioUnit.size() ; iChild ++ )
			{
				CPDBRenderer * pPDBRendererChild = pPDBRendererParent->m_arrayPDBRendererChildBioUnit[iChild];

				D3DXVECTOR3 pos1 = pPDBRendererChild->GetPDBInst()->GetPDB()->m_pdbMinMaxBB[1];
				D3DXVECTOR3 pos2 = pPDBRendererChild->GetPDBInst()->GetPDB()->m_pdbMinMaxBB[0];

				D3DXVec3TransformCoord( &pos1, &pos1, &(pPDBRendererChild->m_matTransformBioUnit) );
				D3DXVec3TransformCoord( &pos2, &pos2, &(pPDBRendererChild->m_matTransformBioUnit) );

				if (maxBB.x < pos1.x) maxBB.x = pos1.x;
				if (maxBB.y < pos1.y) maxBB.y = pos1.y;
				if (maxBB.z < pos1.z) maxBB.z = pos1.z;
				if (minBB.x > pos1.x) minBB.x = pos1.x;
				if (minBB.y > pos1.y) minBB.y = pos1.y;
				if (minBB.z > pos1.z) minBB.z = pos1.z;

				if (maxBB.x < pos2.x) maxBB.x = pos2.x;
				if (maxBB.y < pos2.y) maxBB.y = pos2.y;
				if (maxBB.z < pos2.z) maxBB.z = pos2.z;
				if (minBB.x > pos2.x) minBB.x = pos2.x;
				if (minBB.y > pos2.y) minBB.y = pos2.y;
				if (minBB.z > pos2.z) minBB.z = pos2.z;

				pPDBRendererChild->m_bAttatchBioUnit = m_pActivePDBRenderer->m_bAttatchBioUnit; 
			}

			pPDBRendererParent->m_biounitCenter = (minBB+maxBB)/2;
			pPDBRendererParent->m_bioUnitMinMaxBB[0] = minBB;
			pPDBRendererParent->m_bioUnitMinMaxBB[1] = maxBB;
			pPDBRendererParent->m_biounitRadius = D3DXVec3Length(&D3DXVECTOR3(pPDBRendererParent->m_biounitCenter-minBB)) + 4.0f;

			pPDBRendererParent->UpdatePDBRendererCenter();
		}
		else
		{
			//	child일때. 
			m_pActivePDBRenderer->UpdatePDBRendererCenter();
		}
	}
	else
	{
		//	false 가 되면,
		//	BioUnit 이 On 되면, Center를 새로 만든다.
		if ( m_pActivePDBRenderer->m_pPDBRendererParentBioUnit == NULL )
		{
			//	parent일때,
			CPDBRenderer * pPDBRendererParent = m_pActivePDBRenderer;
			for ( int iChild = 0 ; iChild < pPDBRendererParent->m_arrayPDBRendererChildBioUnit.size() ; iChild ++ )
			{
				CPDBRenderer * pPDBRendererChild = pPDBRendererParent->m_arrayPDBRendererChildBioUnit[iChild];
				pPDBRendererChild->m_bAttatchBioUnit = m_pActivePDBRenderer->m_bAttatchBioUnit; 
			}

			pPDBRendererParent->m_biounitCenter = D3DXVECTOR3(0,0,0);
			pPDBRendererParent->UpdatePDBRendererCenter();
		}
		else
		{
			m_pActivePDBRenderer->UpdatePDBRendererCenter();
		}
	}

	g_bRequestRender = TRUE;
}

//	active 되어질 pPDBRenderer 를 설정.
void CProteinVistaRenderer::SetActivePDBRenderer(CPDBRenderer * pPDBRenderer)
{
	long iFind = -1;
	for ( int i = 0 ; i < m_arrayPDBRenderer.size() ; i++ )
	{
		if ( m_arrayPDBRenderer[i] == pPDBRenderer )
		{
			iFind = i;
			break;
		}
	}

	if ( iFind != -1 )
	{
		 GetMainActiveView()->SetCombIndex(iFind);
		//	set comboBox;
		/*CXTPControlComboBox* comboBox = ((CMainFrame *)AfxGetMainWnd())->m_pComboBoxActivePDB;
		comboBox->SetCurSel(iFind);
		*/
		m_pActivePDBRenderer = pPDBRenderer;
	}
}

LPDIRECT3DTEXTURE9	CProteinVistaRenderer::GetTexture(CString filename)
{
	LPDIRECT3DTEXTURE9 retTexture = NULL;

	CMapTextureFilename::const_iterator iteratorFind = m_mapTextureFilename.find(filename);
	
	if ( iteratorFind == m_mapTextureFilename.end() )
	{	//	찾을수 없다.
		HRESULT hr = D3DXCreateTextureFromFile( m_pd3dDevice, filename, &retTexture );
		if ( FAILED(hr) )
			return NULL;

		CTextureInfo * pTextureInfo = new CTextureInfo;
		pTextureInfo->m_refCount ++;
		pTextureInfo->m_pD3DXTexture = retTexture;
		pTextureInfo->m_strFilename = filename;

		m_mapTextureFilename.insert ( std::pair<CString, CTextureInfo *>(filename, pTextureInfo) );
	}
	else
	{	//	있다.
		iteratorFind->second->m_refCount ++;
		retTexture = iteratorFind->second->m_pD3DXTexture;
	}

	return retTexture;
}

void CProteinVistaRenderer::ReleaseTexture(CString filename)
{
	CMapTextureFilename::const_iterator iteratorFind = m_mapTextureFilename.find(filename);
	if ( iteratorFind != m_mapTextureFilename.end() )
	{	//	있다.
		iteratorFind->second->m_refCount --;
		if ( iteratorFind->second->m_refCount == 0 )
		{
			//    삭제한다.
			SAFE_RELEASE(iteratorFind->second->m_pD3DXTexture);
			delete iteratorFind->second;
			m_mapTextureFilename.erase(iteratorFind);
		}
	}
}

HRESULT CTextureInfo::Load(CFile * fileRead)
{	//		piw 가져오기. piw 저장된 것을 texture디렉토리 안의 파일로 만든다.
	//    파일이 이미 존재하면, 저장하지 않는다.

	CString strFilename;
	ReadString(*fileRead, strFilename);

	LONG len;
	fileRead->Read(&len, sizeof(LONG));

	BYTE * buff = new BYTE[len];
	fileRead->Read(buff, len);

	//    write.
	CString filenameWrite;
	ChangeTexturePathFilename(strFilename, filenameWrite);

	DWORD fileAttr = GetFileAttributes(filenameWrite);
	if (0xFFFFFFFF == fileAttr)
	{
		CFile	fileWrite(filenameWrite, CFile::modeWrite|CFile::modeCreate);
		fileWrite.Write(buff, len);
		fileWrite.Close();
	}

	delete [] buff;

	return S_OK;
}

HRESULT CTextureInfo::Save(CFile * fileWrite, CString filenameTexture)
{	//	texture file을 piw 안에 저장한다.
	//    name, len, data 로 write.
	
	DWORD fileAttr = GetFileAttributes(filenameTexture);
	if ( 0xFFFFFFFF != fileAttr)	//	file exist
	{
		WriteString(*fileWrite, filenameTexture);

		CFile file(filenameTexture, CFile::modeRead);
		LONG len = file.GetLength();
		BYTE * buff = new BYTE[len];

		file.Read(buff, len);

		fileWrite->Write(&len, sizeof(LONG));
		fileWrite->Write(buff, len);

		delete [] buff;
	}

	return S_OK;
}

void CTextureInfo::ChangeTexturePathFilename(CString strFilenameOrig, CString & strFilenameConverted)
{
	TCHAR filename[MAX_PATH];
	TCHAR ext[MAX_PATH];
	_tsplitpath(strFilenameOrig, NULL, NULL, filename, ext );

	//    write.
	strFilenameConverted.Format("%s%s%s" , GetMainApp()->m_strBaseTexturePath, filename, ext);
}


//
//
//
void CProteinVistaRenderer::DrawFullScreenQuad( float fLeftU, float fTopV, float fRightU, float fBottomV )
{
    m_pd3dDevice->SetRenderState( D3DRS_ZENABLE, FALSE );
    m_pd3dDevice->SetFVF( SCREEN_VERTEX::FVF );
    m_pd3dDevice->DrawPrimitiveUP( D3DPT_TRIANGLEFAN, 2, m_svQuad, sizeof( SCREEN_VERTEX ) );

    m_pd3dDevice->SetRenderState( D3DRS_ZENABLE, TRUE );
	m_pd3dDevice->SetFVF(NULL);
}

//
//
long	CProteinVistaRenderer::GetIndicateColorSlot()
{
	for ( int i = 2 ; i < MAX_SELECTION_COLOR ; i++ )
	{
		if ( m_bSelectionColorExist[i] == FALSE )
			break;
	}

	if ( i < MAX_SELECTION_COLOR )
	{	//	space 가 있음
		m_finalRenderSelectionColor[i] = D3DXCOLOR(0,0,0,0);
		m_bSelectionColorExist[i] = TRUE;
		return i;
	}

	return -1;
}

BOOL 	CProteinVistaRenderer::SetIndicateColorSlot(int index, D3DXCOLOR color)
{
	if ( index < 0 || index >= MAX_SELECTION_COLOR )
		return FALSE;

	if ( m_bSelectionColorExist[index] == FALSE )
		return FALSE;

	m_finalRenderSelectionColor[index] = color;
	m_bSelectionColorExist[index] = TRUE;

	return TRUE;
}

BOOL	CProteinVistaRenderer::DeleteIndicateColorSlot(long index)
{
	if ( index < 0 || index >= MAX_SELECTION_COLOR )
		return FALSE;

	m_finalRenderSelectionColor[index] = D3DXCOLOR(0,0,0,0);
	m_bSelectionColorExist[index] = FALSE;

	return TRUE;
}

void	CProteinVistaRenderer::InitIndicateColorSlot()
{
	ZeroMemory(m_finalRenderSelectionColor, sizeof(D3DXCOLOR) * MAX_SELECTION_COLOR );
	ZeroMemory(m_bSelectionColorExist, sizeof(BOOL) * MAX_SELECTION_COLOR );
}

void CProteinVistaRenderer::SelectSpecificAtoms( int conditionID )
{
	CSTLArrayAtomInst atomSelected;
	atomSelected.reserve(2000);

	for(int nPDB = 0 ; nPDB < m_arrayPDBRenderer.size(); nPDB++)
	{
		CPDBInst * pPDBInst = m_arrayPDBRenderer[nPDB]->GetPDBInst();

		//	선택된것을 얻은 다음에 
		CSTLArraySelectionInst selection;
		selection.reserve(100);
		pPDBInst->GetSelectNodeChild(selection);
		for ( int j = 0 ; j < selection.size() ; j++ )
		{
			selection[j]->GetChildAtoms(atomSelected);
		}

		//	전체를 deselect 한다.
		pPDBInst->SetSelectChild(FALSE);
	}

	for ( int i = 0 ; i < atomSelected.size() ; i++ )
	{
		CAtomInst * pAtom = atomSelected[i];

		switch (conditionID)
		{
		case ID_ATOM_SELECTC:
			if ( pAtom->GetAtom()->m_atomName[1] == _T('C') )
				pAtom->SetSelect(TRUE);
			break;

		case ID_ATOM_SELECTN:
			if ( pAtom->GetAtom()->m_atomName[1] == _T('N') )
				pAtom->SetSelect(TRUE);
			break;

		case ID_ATOM_SELECTO:
			if ( pAtom->GetAtom()->m_atomName[1] == _T('O') )
				pAtom->SetSelect(TRUE);
			break;

		case ID_ATOM_SELECTNA:
			if ( pAtom->GetAtom()->m_atomName.Left(2) == _T("NA") )
				pAtom->SetSelect(TRUE);
			break;

		case ID_ATOM_SELECTMG:
			if ( pAtom->GetAtom()->m_atomName.Left(2) == _T("MG") )
				pAtom->SetSelect(TRUE);
			break;

		case ID_ATOM_SELECTP:
			if ( pAtom->GetAtom()->m_atomName[1] == _T('P') )
				pAtom->SetSelect(TRUE);
			break;

		case ID_ATOM_SELECTS:
			if ( pAtom->GetAtom()->m_atomName[1] == _T('S') )
				pAtom->SetSelect(TRUE);
			break;

		case ID_SELECT_HETATM_WITH_HOH:
			if ( pAtom->GetAtom()->m_bHETATM == TRUE )
				pAtom->SetSelect(TRUE);
			break;

		case ID_SELECT_HETATM_WITHOUT_HOH:
			if ( pAtom->GetAtom()->m_bHETATM == TRUE && pAtom->GetAtom()->m_residueName != _T("HOH") )
				pAtom->SetSelect(TRUE);
			break;

		case ID_SELECT_CA:
			if ( pAtom->GetAtom()->m_typeAtom == MAINCHAIN_CA )
				pAtom->SetSelect(TRUE);
			break;

		case ID_SELECT_BACKBONE:
			if ( pAtom->GetAtom()->m_bSideChain == FALSE)
				pAtom->SetSelect(TRUE);
			break;

		case ID_SELECT_SIDECHAIN:
			if ( pAtom->GetAtom()->m_bSideChain == TRUE )
				pAtom->SetSelect(TRUE);
			break;

		case ID_SELECT_HYDROPHILIC:
			if ( pAtom->GetAtom()->m_hydropathy <= 0 )
			{
				pAtom->m_pResidueInst->SetSelect(TRUE);
				pAtom->SetSelect(TRUE);
			}
			break;

		case ID_SELECT_HYDROPHOBIC:
			if ( pAtom->GetAtom()->m_hydropathy > 0 )
			{
				pAtom->m_pResidueInst->SetSelect(TRUE);
				pAtom->SetSelect(TRUE);
			}
			break;

		case ID_SELECT_HELIX:
			if ( pAtom->GetAtom()->m_secondaryStructure  == SS_HELIX )
			{
				pAtom->m_pResidueInst->SetSelect(TRUE);
				pAtom->SetSelect(TRUE);
			}
			break;

		case ID_SELECT_SHEET:
			if ( pAtom->GetAtom()->m_secondaryStructure  == SS_SHEET )
			{
				pAtom->m_pResidueInst->SetSelect(TRUE);
				pAtom->SetSelect(TRUE);
			}
			break;
		}
	}

	//	sync
	SelectedAtomApply();
}

void CProteinVistaRenderer::SetViewVolClipPlane()
{
	m_fNearClipPlane = 1e20;
	m_fFarClipPlane = 1.0f;

	FLOAT lenCameraToCenter = D3DXVec3Length(&m_FromVec);
 
	CHTMLListCtrl * phtmlListCtrl = GetMainActiveView()->GetSelectList();
	for ( int i = 0 ; i < phtmlListCtrl->GetItemCount() ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = (CSelectionDisplay *)(phtmlListCtrl->GetItemData(i));
		if ( pSelectionDisplay )
		{
			pSelectionDisplay->FrameMove();

			//	near far clip plane.
			D3DXVECTOR3	selectionCenterTr;
			D3DXVec3TransformCoord(&selectionCenterTr, &(pSelectionDisplay->m_center), &(pSelectionDisplay->m_pPDBRenderer->m_matWorld) );
			D3DXVec3TransformCoord(&selectionCenterTr, &selectionCenterTr, &m_matrixView );

			FLOAT nearPos = selectionCenterTr.z - pSelectionDisplay->m_radius;
			nearPos -= 100.0f;
			nearPos = min( nearPos, lenCameraToCenter-40.0f );
			nearPos = max( 3.0f , nearPos );

			FLOAT farPos = selectionCenterTr.z + pSelectionDisplay->m_radius;
			farPos = max( farPos, lenCameraToCenter+40.0f );		//	카메라가 최소한 center 까지는 보여야 한다.
			farPos += 100.0f;

			if ( nearPos < m_fNearClipPlane )
				m_fNearClipPlane = nearPos;

			if ( farPos > m_fFarClipPlane )
				m_fFarClipPlane = farPos;
		}
	}
}



