#include "stdafx.h"
#include "ProteinVista.h"
#include "ProteinVistaView.h"
#include "RenderProperty.h"
#include "SelectionDisplay.h"
#include "Utility.h"
#include "pdb.h"
#include "pdbInst.h"
#include "Interface.h"
#include "ProteinVistaRenderer.h"
#include "PIProperty.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

 
//
void ReserveWrite(CFile * fpWorkspace, int nDWORD)
{
	DWORD		reserve = 0;
	for ( int i = 0 ; i < nDWORD ; i++ )
		fpWorkspace->Write(&reserve, sizeof(DWORD));
}

LONG ReserveRead(CFile * fpWorkspace, int nDWORD)
{
	DWORD		nRead = 0;
	DWORD		reserve = 0;
	for ( int i = 0 ; i < nDWORD ; i++ )
		nRead += fpWorkspace->Read(&reserve, sizeof(DWORD));

	return nRead;
}


//    
CRenderProperty::CRenderProperty()
{
}

CPropertyScene::CPropertyScene() 
{ 
	m_colorBackroundColor = RGB(128,128,128);
	m_bUseBackgroundTexture = TRUE; 
	m_strBackgroundTextureFilename = GetMainApp()->m_strBaseTexturePath + _T("BackgroundDefault.dds");
	
	m_d3dcolorBackroundColor = 0; 

	m_modelQuality = 3;		//	0,1,2,3,4 : 0 is very low , default is high, 4 is highest.
	m_shaderQuality = 1;

	m_bDepthOfField = FALSE;
	m_fogColor = RGB(255,255,255);
	m_fogStart = 20;
	m_fogEnd = 80;

	m_fNearClipPlane = 1.0f;
	m_fFarClipPlane = 300.0f;

	m_bDisplayAxis = TRUE;
	m_axisScale = 50;

	m_iAntialiasing = AA_NONE;

	m_bLight1Use = TRUE;
	m_bLight1Show = TRUE;
	m_light1Color = RGB(255,255,255);
	m_light1Intensity = 100;
	m_fLight1Radius = 50.0;

	m_bLight2Use = TRUE;
	m_bLight2Show = TRUE;
	m_light2Color = RGB(255,255,255);
	m_light2Intensity = 100;
	m_fLight2Radius = 50.0;

	m_bShowSelectionMark = TRUE;

	m_bClipping0 = FALSE;
	m_bShowClipPlane0 = TRUE;
	m_clipPlaneColor0 = RGB(255,255,255);
	m_clipPlaneTransparency0 = 100;
	m_bClipDirection0 = TRUE;
	m_clipPlaneEquation0 = D3DXPLANE(0,0,0,0);

	m_iDisplayHETATM = DISPLAY_HETATM_NONE;

	m_cameraType = CAMERA_PERSPECTIVE ;	//	0 is otho.
	m_lFOV = 45;
	m_othoCameraViewVol = 50;

	m_selectionColor = 0x00ff00;

	m_bDoubleClockToCameraAnimation = TRUE;
	m_fAnimationTime = 1.0;

	m_bUseSSAO = TRUE;
	m_ssaoRange = 100;	//	[0..200]*0.1
	m_numSSAOSampling = 16;
	m_ssaoIntensity = 35;	//	[0..100]*0.05 -> [0..5]
	m_ssaoBlurType = 2;
	m_bUseFullSizeBlur = TRUE;

	m_dwReserve = 40;
}
//
//
HRESULT	 CPropertyScene::Save(CFile * fpWorkspace) 
{
	CString vf ;
	vf.Format("%d",m_colorBackroundColor);
	//AfxMessageBox(vf);
	fpWorkspace->Write(&m_colorBackroundColor, sizeof(COLORREF));
	fpWorkspace->Write(&m_bUseBackgroundTexture, sizeof(BOOL));

	CTextureInfo::Save(fpWorkspace, m_strBackgroundTextureFilename);
	WriteString(*fpWorkspace, m_strBackgroundTextureFilename);

	fpWorkspace->Write(&m_modelQuality, sizeof(int));
	fpWorkspace->Write(&m_shaderQuality, sizeof(int));
	fpWorkspace->Write(&m_bShowSelectionMark, sizeof(BOOL));

	fpWorkspace->Write(&m_fNearClipPlane, sizeof(double));
	fpWorkspace->Write(&m_fFarClipPlane, sizeof(double));

	//	ssao
	fpWorkspace->Write(&m_bUseSSAO, sizeof(BOOL));
	fpWorkspace->Write(&m_numSSAOSampling, sizeof(long));
	fpWorkspace->Write(&m_ssaoRange, sizeof(long));
	fpWorkspace->Write(&m_ssaoIntensity, sizeof(long));
	fpWorkspace->Write(&m_ssaoBlurType, sizeof(int));
	fpWorkspace->Write(&m_bUseFullSizeBlur, sizeof(BOOL));

	//	depth of field
	fpWorkspace->Write(&m_bDepthOfField, sizeof(BOOL));
	fpWorkspace->Write(&m_fogColor, sizeof(COLORREF));
	fpWorkspace->Write(&m_fogStart, sizeof(long));
	fpWorkspace->Write(&m_fogEnd, sizeof(long));

	fpWorkspace->Write(&m_bDisplayAxis, sizeof(BOOL));
	fpWorkspace->Write(&m_axisScale, sizeof(LONG));

	fpWorkspace->Write(&m_iAntialiasing, sizeof(int));

	fpWorkspace->Write(&m_bLight1Use, sizeof(BOOL));
	fpWorkspace->Write(&m_bLight1Show, sizeof(BOOL));
	fpWorkspace->Write(&m_light1Color, sizeof(COLORREF));
	fpWorkspace->Write(&m_light1Intensity, sizeof(long));
	fpWorkspace->Write(&m_fLight1Radius, sizeof(DOUBLE));

	fpWorkspace->Write(&m_bLight2Use, sizeof(BOOL));
	fpWorkspace->Write(&m_bLight2Show, sizeof(BOOL));
	fpWorkspace->Write(&m_light2Color, sizeof(COLORREF));
	fpWorkspace->Write(&m_light2Intensity, sizeof(long));
	fpWorkspace->Write(&m_fLight1Radius, sizeof(DOUBLE));

	fpWorkspace->Write(&m_bClipping0, sizeof(BOOL));
	fpWorkspace->Write(&m_bShowClipPlane0, sizeof(BOOL));
	fpWorkspace->Write(&m_clipPlaneColor0, sizeof(COLORREF));
	fpWorkspace->Write(&m_clipPlaneTransparency0, sizeof(long));
	fpWorkspace->Write(&m_bClipDirection0, sizeof(BOOL));

	CString strPlaneEquation0;
	strPlaneEquation0.Format ("%.3f,%.3f,%.3f,%.3f", m_clipPlaneEquation0.a, m_clipPlaneEquation0.b, m_clipPlaneEquation0.c, m_clipPlaneEquation0.d );
	WriteString(*fpWorkspace, strPlaneEquation0);

	fpWorkspace->Write(&m_cameraType, sizeof(int));
	fpWorkspace->Write(&m_lFOV, sizeof(long));
	fpWorkspace->Write(&m_othoCameraViewVol, sizeof(long));

	fpWorkspace->Write(&m_bDoubleClockToCameraAnimation, sizeof(BOOL));
	fpWorkspace->Write(&m_fAnimationTime, sizeof(DOUBLE));

	ReserveWrite(fpWorkspace, m_dwReserve);
	return S_OK;
}

//

HRESULT		CPropertyScene::Load(CFile * fpWorkspace) 
{
	COLORREF	colorBackroundColor;
	BOOL		bUseBackgroundTexture;
	CString		strBackgroundTexture;

	int			enumModelQuality;
	int			enumShaderQuality;
	BOOL		showSelectionMark;
	
	BOOL		bDepthOfField;
	COLORREF	fogColor;
	long		fogStart;
	long		fogEnd;

	double		fNearClipPlane;
	double		fFarClipPlane;

	BOOL		bDisplayAxis;
	LONG		axisScale;
	int			iAntialiasing;

	BOOL		bLight1;
	COLORREF	light1Color;
	long		light1Intensity;

	BOOL		bLight2;
	COLORREF	light2Color;
	long		light2Intensity;

	BOOL		bClipping0;
	BOOL		bShowClipPlane0;
	COLORREF	clipPlaneColor0;
	long		clipPlaneTransparency0;
	BOOL		bClipDirection0;

	int			iDisplayHETATM;

	BOOL		bLight1Show;
	BOOL		bLight2Show;
	DOUBLE		fLight1Radius;
	DOUBLE		fLight2Radius;

	CString		strClipEquation0;

	int			cameraType;
	long		lFOV;
	long		lViewVol;

	BOOL		bCameraAnimation;
	DOUBLE		fAnimationTime;
    
	CPIPropertyScene^ mProrpetyScene = gcnew CPIPropertyScene(nullptr);

	fpWorkspace->Read(&colorBackroundColor, sizeof(COLORREF));
	CString vf ;
	vf.Format("%d",colorBackroundColor);
	mProrpetyScene->BackgroundColor =System::Drawing::Color::FromArgb(colorBackroundColor);
	 

	fpWorkspace->Read(&bUseBackgroundTexture, sizeof(BOOL));
	mProrpetyScene->ShowBackgroundTexture =Convert::ToBoolean(bUseBackgroundTexture);

	CTextureInfo::Load(fpWorkspace);
	ReadString(*fpWorkspace, strBackgroundTexture);
	CTextureInfo::ChangeTexturePathFilename(strBackgroundTexture, strBackgroundTexture);

	mProrpetyScene->BackgroundTextureFilename =CStringToMStr(strBackgroundTexture);

	fpWorkspace->Read(&enumModelQuality, sizeof(int));
	mProrpetyScene->GeometryQuality = (IPropertyScene::IGeometryQuality)enumModelQuality;

	fpWorkspace->Read(&enumShaderQuality, sizeof(int));
	mProrpetyScene->ShaderQuality =(IPropertyScene::IShaderQuality)enumShaderQuality;

	fpWorkspace->Read(&showSelectionMark, sizeof(BOOL));
	mProrpetyScene->ShowSelectionMark =Convert::ToBoolean(showSelectionMark);

	fpWorkspace->Read(&fNearClipPlane, sizeof(double));
	mProrpetyScene->ClipPlaneNear =fNearClipPlane;

	fpWorkspace->Read(&fFarClipPlane, sizeof(double));
	mProrpetyScene->ClipPlaneFar =fFarClipPlane;

	//	ssao
	BOOL		bUseSSAO;
	long		numSSAOSampling;
	long		ssaoRange;
	long		ssaoIntensity;
	int			ssaoBlur;
	BOOL		bUseFullSizeBlur;

	fpWorkspace->Read(&bUseSSAO, sizeof(BOOL));
	mProrpetyScene->EnableAO =Convert::ToBoolean(bUseSSAO);;

	fpWorkspace->Read(&numSSAOSampling, sizeof(long));
	mProrpetyScene->AOSampling =numSSAOSampling;

	fpWorkspace->Read(&ssaoRange, sizeof(long));
	mProrpetyScene->AORange =ssaoRange;

	fpWorkspace->Read(&ssaoIntensity, sizeof(long));
	mProrpetyScene->AOIntensity =ssaoIntensity;

	fpWorkspace->Read(&ssaoBlur, sizeof(int));
	mProrpetyScene->AOBlurType =(IPropertyScene::IAOBlurType)ssaoBlur;

	fpWorkspace->Read(&bUseFullSizeBlur, sizeof(BOOL));
	mProrpetyScene->AOFullSizeBuffer =Convert::ToBoolean(bUseFullSizeBlur);

	//	depth of field
	fpWorkspace->Read(&bDepthOfField, sizeof(BOOL));
	mProrpetyScene->DepthOfField =Convert::ToBoolean(bDepthOfField);

	fpWorkspace->Read(&fogColor, sizeof(COLORREF));
	mProrpetyScene->FogColor =System::Drawing::Color::FromArgb(fogColor);

	fpWorkspace->Read(&fogStart, sizeof(long));
	mProrpetyScene->FogStart =fogStart;

	fpWorkspace->Read(&fogEnd, sizeof(long));
	mProrpetyScene->FogEnd =fogEnd;

	fpWorkspace->Read(&bDisplayAxis, sizeof(BOOL));
	mProrpetyScene->DisplayAxis =Convert::ToBoolean(bDisplayAxis);

	fpWorkspace->Read(&axisScale, sizeof(LONG));
	mProrpetyScene->AxisSize= axisScale;

	fpWorkspace->Read(&iAntialiasing, sizeof(int));
	mProrpetyScene->AntiAliasing= (IPropertyScene::IAntiAliasing)iAntialiasing;

	fpWorkspace->Read(&bLight1, sizeof(BOOL));
	mProrpetyScene->Light1->Show =Convert::ToBoolean(bLight1);

	fpWorkspace->Read(&bLight1Show, sizeof(BOOL));
	mProrpetyScene->Light1->Show =Convert::ToBoolean(bLight1);

	fpWorkspace->Read(&light1Color, sizeof(COLORREF));
	mProrpetyScene->Light1->Color =System::Drawing::Color::FromArgb(light1Color);

	fpWorkspace->Read(&light1Intensity, sizeof(long));
	mProrpetyScene->Light1->Intensity =light1Intensity;

	fpWorkspace->Read(&fLight1Radius, sizeof(DOUBLE));
	//mProrpetyScene->Light1->Position =light1Intensity;

	fpWorkspace->Read(&bLight2, sizeof(BOOL));
	mProrpetyScene->Light2->Show =Convert::ToBoolean(bLight2);

	fpWorkspace->Read(&bLight2Show, sizeof(BOOL));
	mProrpetyScene->Light2->Show =Convert::ToBoolean(bLight2Show);

	fpWorkspace->Read(&light2Color, sizeof(COLORREF));
	mProrpetyScene->Light2->Color =System::Drawing::Color::FromArgb(light2Color);

	fpWorkspace->Read(&light2Intensity, sizeof(long));
	mProrpetyScene->Light2->Intensity =light2Intensity;

	fpWorkspace->Read(&fLight2Radius, sizeof(DOUBLE));

	fpWorkspace->Read(&bClipping0, sizeof(BOOL));
	mProrpetyScene->ClippingPanel->Show =Convert::ToBoolean(bClipping0);

	fpWorkspace->Read(&bShowClipPlane0, sizeof(BOOL));
	mProrpetyScene->ClippingPanel->Show =Convert::ToBoolean(bShowClipPlane0);

	fpWorkspace->Read(&clipPlaneColor0, sizeof(COLORREF));
	mProrpetyScene->ClippingPanel->Color =System::Drawing::Color::FromArgb(clipPlaneColor0); 

	fpWorkspace->Read(&clipPlaneTransparency0, sizeof(long));
	mProrpetyScene->ClippingPanel->Transparency = clipPlaneTransparency0;

	fpWorkspace->Read(&bClipDirection0, sizeof(BOOL));
	mProrpetyScene->ClippingPanel->Direction  =Convert::ToBoolean(bClipDirection0);

	ReadString(*fpWorkspace, strClipEquation0);
	mProrpetyScene->ClippingPanel->Equation =CStringToMStr(strClipEquation0);

	fpWorkspace->Read(&cameraType, sizeof(int));
	mProrpetyScene->CameraType =(IPropertyScene::ICameraType)cameraType;

	fpWorkspace->Read(&lFOV, sizeof(long));
	mProrpetyScene->FOV = lFOV;

	fpWorkspace->Read(&lViewVol, sizeof(long));
	mProrpetyScene->SizeViewVol = lViewVol;

	fpWorkspace->Read(&bCameraAnimation, sizeof(BOOL));
	mProrpetyScene->CameraAnimation();

	fpWorkspace->Read(&fAnimationTime, sizeof(DOUBLE));
	//mProrpetyScene->CameraPosition. = fAnimationTime;

	ReserveRead(fpWorkspace, m_dwReserve);
	::GetMainActiveView()->RefreshProptery(nullptr,6);
	return S_OK;
}


///////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////

CPropertyCommon::CPropertyCommon(CSelectionDisplay * pSelectionDisplay)
{ 
	m_dwReserve = 40;
	m_pSelectionDisplay = pSelectionDisplay; 

	m_enumDisplayMode = CSelectionDisplay::WIREFRAME;

	m_bDisplaySideChain = TRUE; 
	m_bDisplayHETATM = TRUE; 
	m_enumColorScheme = 0; 
	m_singleColor = 0x0000ff;

	//	m_selectionColor = 0x00ff00;
	m_bIndicate = FALSE;
	m_indicateColor = 0xffff00;
	m_indicateColorSlot = -1;

	if ( pSelectionDisplay )
	{
		m_modelQuality = pSelectionDisplay->m_pProteinVistaRenderer->m_renderQualityPreset.m_modelQuality;
		m_bShowSelectionMark = pSelectionDisplay->m_pProteinVistaRenderer->m_renderQualityPreset.m_bShowSelectionMark;
		m_shaderQuality = pSelectionDisplay->m_pProteinVistaRenderer->m_renderQualityPreset.m_shaderQuality;
	}

	m_bClipping1 = FALSE;
	m_bShowClipPlane1 = TRUE;
	m_clipPlaneColor1 = RGB(230,230,230);
	m_clipPlaneTransparency1 = 100;
	m_bClipDirection1 = TRUE;

	m_clipPlaneEquation1 = D3DXPLANE(0,0,0,0);

	m_bClipping2 = FALSE;
	m_bShowClipPlane2 = TRUE;
	m_clipPlaneColor2 = RGB(180,180,180);
	m_clipPlaneTransparency2 = 100;
	m_bClipDirection2 = TRUE;

	m_clipPlaneEquation2 = D3DXPLANE(0,0,0,0);

	//m_bDepthOfField = FALSE;
	//m_fogColor = RGB(255,255,255);
	//m_fogStart = 20;
	//m_fogEnd = 90;

	m_bClipPS = TRUE;

	m_intensityAmbient = 12;
	m_intensiryDiffuse = 50;
	m_intensitySpecular = 25;

	m_bDisplayAxisLocalCoord = TRUE;
	m_axisScaleLocalCoord = 50;

	//	
	CFont font;
	font.CreatePointFont(100, _T("Arial"));		//	point size는 *10 단위이다.

	for ( int i = 0 ; i < 3 ; i++ )
	{
		m_bAnnotation[i] = FALSE;
		m_enumTextDisplayTechnique[i] = 0;
		font.GetLogFont(&m_logFont[i]);
		m_enumAnnotationColorScheme[i] = 0;
		m_annotationColor[i] = RGB(255,255,255);
		m_annotationXPos[i] = m_annotationYPos[i] = m_annotationZPos[i] = 50;
		m_annotationTextXPos[i] = m_annotationTextYPos[i] = 50;
		m_annotationDepthBias[i] = 0;
		m_annotationTransparency[i] = 100;
		m_enumAnnotatonType[i] = 0;
	}

	InitColorScheme();
}

CPropertyCommon::~CPropertyCommon() 
{ 
	for ( int i = 0 ; i < NUM_COLOR_SCHEME ; i++ ) 
	{
		for (int j = 0 ; j < m_arrayColorScheme[i].size() ; j++ )
			SAFE_DELETE(m_arrayColorScheme[i][j]);
		m_arrayColorScheme[i].clear(); 
	}

	for ( int i = 0 ; i < NUM_COLOR_SCHEME ; i++ ) 
	{
		for (int j = 0 ; j < m_arrayColorSchemeDefault[i].size() ; j++ )
			SAFE_DELETE(m_arrayColorSchemeDefault[i][j]);
		m_arrayColorSchemeDefault[i].clear();
	}
}

void CPropertyCommon::InitColorScheme()
{
	//    
	//	CProteinVistaRenderer 에 있는 default color scheme을 복사한다.
	//	
	if ( m_pSelectionDisplay )
	{
		CProteinVistaRenderer * pProteinVistaRenderer = m_pSelectionDisplay->m_pProteinVistaRenderer;
		for ( int i= 0 ; i < NUM_COLOR_SCHEME ; i++ )
		{
			long numRow = pProteinVistaRenderer->m_colorSchemeDefault.m_arrayColorRowDefault[i].size();
			if ( numRow > 0 )
			{
				m_arrayColorScheme[i].reserve(numRow);
				m_arrayColorSchemeDefault[i].reserve(numRow);

				pProteinVistaRenderer->m_colorSchemeDefault.CopyArrayColorRow(m_arrayColorScheme[i], pProteinVistaRenderer->m_colorSchemeDefault.m_arrayColorRowDefault[i] );
				pProteinVistaRenderer->m_colorSchemeDefault.CopyArrayColorRow(m_arrayColorSchemeDefault[i], pProteinVistaRenderer->m_colorSchemeDefault.m_arrayColorRowDefault[i] );
			}
		}
	}
}

HRESULT	CPropertyCommon::Save(CFile * fpWorkspace)
{
	fpWorkspace->Write(&m_enumDisplayMode, sizeof(int));

	WriteString(*fpWorkspace, m_strSelectionName);

	fpWorkspace->Write( &m_bDisplaySideChain, sizeof(BOOL));
	fpWorkspace->Write( &m_enumColorScheme, sizeof(int));
	//    color scheme customize.
	
	for ( int i = 0 ; i < NUM_COLOR_SCHEME ; i++ )
	{
		CArrayColorRow & arrayColorRow = m_arrayColorScheme[i];

		int lenColors = arrayColorRow.size();
		fpWorkspace->Write( &lenColors, sizeof(int));

		for ( int j = 0; j < arrayColorRow.size(); j++ )
		{
			CColorRow * colorRow = arrayColorRow[j];
			
			CString colorName(colorRow->m_name);
			WriteString(*fpWorkspace, colorName );
			DWORD color0 = (DWORD)(colorRow->m_color);		fpWorkspace->Write( &color0, sizeof(DWORD) );
		}
	}

	//    
	fpWorkspace->Write( &m_modelQuality, sizeof(int));
	fpWorkspace->Write( &m_shaderQuality, sizeof(int));
	fpWorkspace->Write( &m_bShowSelectionMark, sizeof(BOOL));

	//	fpWorkspace->Write( &m_selectionColor, sizeof(COLORREF));
	fpWorkspace->Write( &m_bIndicate, sizeof(BOOL));
	fpWorkspace->Write( &m_indicateColor, sizeof(COLORREF));

	for ( int i = 0 ; i < 3 ; i++ )
	{
		fpWorkspace->Write( &m_bAnnotation[i], sizeof(BOOL) );
		fpWorkspace->Write( &m_enumAnnotatonType[i], sizeof(int) );
		WriteString ( *fpWorkspace, m_strAnnotation[i] );
		fpWorkspace->Write( &m_enumTextDisplayTechnique[i], sizeof(int) );
		fpWorkspace->Write( &m_logFont[i], sizeof(LOGFONT) );
		fpWorkspace->Write( &m_annotationColor[i], sizeof(COLORREF) );
		fpWorkspace->Write( &m_enumAnnotationColorScheme[i], sizeof(int) );
		fpWorkspace->Write( &m_annotationXPos[i], sizeof(LONG) );
		fpWorkspace->Write( &m_annotationYPos[i], sizeof(LONG) );
		fpWorkspace->Write( &m_annotationZPos[i], sizeof(LONG) );
		fpWorkspace->Write( &m_annotationTextXPos[i], sizeof(LONG) );
		fpWorkspace->Write( &m_annotationTextYPos[i], sizeof(LONG) );
		fpWorkspace->Write( &m_annotationDepthBias[i], sizeof(LONG) );
		fpWorkspace->Write( &m_annotationTransparency[i], sizeof(LONG) );
	}

	fpWorkspace->Write( &m_bClipping1, sizeof(BOOL));
	fpWorkspace->Write( &m_bShowClipPlane1, sizeof(BOOL));
	fpWorkspace->Write( &m_clipPlaneColor1, sizeof(COLORREF));
	fpWorkspace->Write( &m_clipPlaneTransparency1, sizeof(long));
	fpWorkspace->Write( &m_bClipDirection1, sizeof(BOOL));
	CString strPlaneEquation1;
	strPlaneEquation1.Format ("%.3f,%.3f,%.3f,%.3f", m_clipPlaneEquation1.a, m_clipPlaneEquation1.b, m_clipPlaneEquation1.c, m_clipPlaneEquation1.d );
	WriteString(*fpWorkspace, strPlaneEquation1);

	fpWorkspace->Write( &m_bClipping2, sizeof(BOOL));
	fpWorkspace->Write( &m_bShowClipPlane2, sizeof(BOOL));
	fpWorkspace->Write( &m_clipPlaneColor2, sizeof(COLORREF));
	fpWorkspace->Write( &m_clipPlaneTransparency2, sizeof(long));
	fpWorkspace->Write( &m_bClipDirection2, sizeof(BOOL));
	CString strPlaneEquation2;
	strPlaneEquation2.Format ("%.3f,%.3f,%.3f,%.3f", m_clipPlaneEquation2.a, m_clipPlaneEquation2.b, m_clipPlaneEquation2.c, m_clipPlaneEquation2.d );
	WriteString(*fpWorkspace, strPlaneEquation2);

	// 	fpWorkspace->Write(&m_bDepthOfField, sizeof(BOOL));
	// 	fpWorkspace->Write(&m_fogColor, sizeof(COLORREF));
	// 	fpWorkspace->Write(&m_fogStart, sizeof(long));
	// 	fpWorkspace->Write(&m_fogEnd, sizeof(long));

	fpWorkspace->Write(&m_bClipPS, sizeof(BOOL));
	fpWorkspace->Write(&m_intensityAmbient, sizeof(int));
	fpWorkspace->Write(&m_intensiryDiffuse, sizeof(int));
	fpWorkspace->Write(&m_intensitySpecular, sizeof(int));

	fpWorkspace->Write(&m_bDisplayAxisLocalCoord, sizeof(BOOL));
	fpWorkspace->Write(&m_axisScaleLocalCoord, sizeof(long));

	//
	ReserveWrite(fpWorkspace, m_dwReserve);

	return S_OK;
}

HRESULT	CPropertyCommon::Load(CFile * fpWorkspace)
{
	CString		strSelectionName;

	BOOL		bDisplaySideChain;
	BOOL		bDisplayHETATM;
	int			enumColorScheme;
	COLORREF	singleColor;
	COLORREF	selectionColor;
	BOOL		bIndicate;
	COLORREF	indicateColor;
	
	BOOL		bClipping1;
	BOOL		bShowClipPlane1;
	COLORREF	clipPlaneColor1;
	long		clipPlaneTransparency1;
	BOOL		bClipDirection1;
	BOOL		bClipping2;
	BOOL		bShowClipPlane2;
	COLORREF	clipPlaneColor2;
	long		clipPlaneTransparency2;
	BOOL		bClipDirection2;

	BOOL		bDepthOfField;
	COLORREF	fogColor;
	long		fogStart;
	long		fogEnd;

	CPICommonProperty^ mCommProperty = gcnew CPICommonProperty(this->m_pSelectionDisplay);

	int		enumDisplayMode;
	fpWorkspace->Read(&enumDisplayMode, sizeof(int));
	mCommProperty->VisualizationMode =(VisualMode)enumDisplayMode;

	ReadString(*fpWorkspace, strSelectionName);
	mCommProperty->VPName =CStringToMStr(strSelectionName);

	fpWorkspace->Read( &bDisplaySideChain, sizeof(BOOL));
	mCommProperty->DisplaySideChain =Convert::ToBoolean(bDisplaySideChain);

	fpWorkspace->Read( &enumColorScheme, sizeof(int));
	mCommProperty->ColorScheme =(IProperty::IColorScheme )enumColorScheme;

	for ( int i = 0 ; i < NUM_COLOR_SCHEME ; i++ )
	{
		CArrayColorRow & arrayColorRow = m_arrayColorScheme[i];

		int lenColors;
		fpWorkspace->Read( &lenColors, sizeof(int) );

		arrayColorRow.resize(lenColors);

		for ( int j = 0; j < arrayColorRow.size(); j++ )
		{
			if ( arrayColorRow[j] == NULL )
				arrayColorRow[j] = new CColorRow;

			CColorRow * colorRow = arrayColorRow[j];

			CString colorName;
			ReadString(*fpWorkspace, colorName);
			colorRow->m_name = colorName;
			DWORD color0;
			fpWorkspace->Read( &color0, sizeof(DWORD) );
			colorRow->m_color = D3DXCOLOR(color0);
		}
	}
	if(m_pSelectionDisplay)
	{
	    m_pSelectionDisplay->SetPropertyChanged(PROPERTY_COMMON_COLOR_SCHEME_CUSTOMIZE); 
	}

	int modelQuality;
	fpWorkspace->Read ( &modelQuality, sizeof(int));
	mCommProperty->GeometryQuality =(IProperty::IGeometryQuality )modelQuality;

	int shaderQuality;
	fpWorkspace->Read( &shaderQuality, sizeof(int));
	mCommProperty->ShaderQuality =(IProperty::IShaderQuality )shaderQuality;

	BOOL bShowSelectionMark;
	fpWorkspace->Read( &bShowSelectionMark, sizeof(BOOL));
	mCommProperty->ShowSelectionMark =Convert::ToBoolean(bShowSelectionMark);

	//	fpWorkspace->Read( &selectionColor, sizeof(COLORREF));
	fpWorkspace->Read( &bIndicate, sizeof(BOOL));
	mCommProperty->ShowIndicateSelectionMark =Convert::ToBoolean(bIndicate);

	fpWorkspace->Read( &indicateColor, sizeof(COLORREF));
	mCommProperty->IndicateSelectionMarkColor = System::Drawing::Color::FromArgb(indicateColor);

	BOOL		bAnnotation[3];
	int			enumTextDisplayTechnique[3];
	CString		strAnnotation[3];
	int			enumAnnotatonType[3];
	int			enumAnnotationColorScheme[3];
	COLORREF	annotationColor[3];
	int			enumAnnotationPos[3];
	long		transparency[3];
	LOGFONT		logFont[3];
	LONG		annotationXPos[3];
	LONG		annotationYPos[3];
	LONG		annotationZPos[3];
	LONG		annotationTextXPos[3];
	LONG		annotationTextYPos[3];
	LONG		annotationDepthBias[3];
	LONG		annotationTransparency[3];

	for ( int i = 0 ; i < 3 ; i++ )
	{
		fpWorkspace->Read( &bAnnotation[i], sizeof(BOOL) );
		fpWorkspace->Read( &enumAnnotatonType[i], sizeof(int) );
		ReadString ( *fpWorkspace, strAnnotation[i] );
		fpWorkspace->Read( &enumTextDisplayTechnique[i], sizeof(int) );
		fpWorkspace->Read( &logFont[i], sizeof(LOGFONT) );
		fpWorkspace->Read( &annotationColor[i], sizeof(COLORREF) );
		fpWorkspace->Read( &enumAnnotationColorScheme[i], sizeof(int) );
		fpWorkspace->Read( &annotationXPos[i], sizeof(LONG) );
		fpWorkspace->Read( &annotationYPos[i], sizeof(LONG) );
		fpWorkspace->Read( &annotationZPos[i], sizeof(LONG) );
		fpWorkspace->Read( &annotationTextXPos[i], sizeof(LONG) );
		fpWorkspace->Read( &annotationTextYPos[i], sizeof(LONG) );
		fpWorkspace->Read( &annotationDepthBias[i], sizeof(LONG) );
		fpWorkspace->Read( &annotationTransparency[i], sizeof(LONG) );
		if(i==0)
		{
			mCommProperty->DisplayVPAnnotation->Show =Convert::ToBoolean(bAnnotation[i]);
			mCommProperty->DisplayVPAnnotation->TextType =(IAnnotation::ITextType)enumAnnotatonType[i];
			mCommProperty->DisplayVPAnnotation->DisplayMethod =(IAnnotation::IDisplayMethod	)enumTextDisplayTechnique[i];
			mCommProperty->DisplayVPAnnotation->FontHeight =logFont[i].lfHeight;
			mCommProperty->DisplayVPAnnotation->Color =System::Drawing::Color::FromArgb(annotationColor[i]);;
			mCommProperty->DisplayVPAnnotation->ColorScheme =(IAnnotation::IColorScheme)enumAnnotationColorScheme[i];
			mCommProperty->DisplayVPAnnotation->RelativeXPos =annotationXPos[i];
			mCommProperty->DisplayVPAnnotation->RelativeYPos =annotationYPos[i];
			mCommProperty->DisplayVPAnnotation->RelativeZPos =annotationZPos[i];
			mCommProperty->DisplayVPAnnotation->TextXPos =annotationTextXPos[i];
			mCommProperty->DisplayVPAnnotation->TextYPos =annotationTextYPos[i];
			mCommProperty->DisplayVPAnnotation->DepthBias =annotationDepthBias[i];
			mCommProperty->DisplayVPAnnotation->Transparency =annotationTransparency[i];
		}else if(i==1)
		{
			mCommProperty->DisplayAnnotationAtom->Show =Convert::ToBoolean(bAnnotation[i]);
			mCommProperty->DisplayAnnotationAtom->TextType =(IAnnotation::ITextType)enumAnnotatonType[i];
			mCommProperty->DisplayAnnotationAtom->DisplayMethod =(IAnnotation::IDisplayMethod	)enumTextDisplayTechnique[i];
			mCommProperty->DisplayAnnotationAtom->FontHeight =logFont[i].lfHeight;
			mCommProperty->DisplayAnnotationAtom->Color =System::Drawing::Color::FromArgb(annotationColor[i]);;
			mCommProperty->DisplayAnnotationAtom->ColorScheme =(IAnnotation::IColorScheme)enumAnnotationColorScheme[i];
			mCommProperty->DisplayAnnotationAtom->RelativeXPos =annotationXPos[i];
			mCommProperty->DisplayAnnotationAtom->RelativeYPos =annotationYPos[i];
			mCommProperty->DisplayAnnotationAtom->RelativeZPos =annotationZPos[i];
			mCommProperty->DisplayAnnotationAtom->TextXPos =annotationTextXPos[i];
			mCommProperty->DisplayAnnotationAtom->TextYPos =annotationTextYPos[i];
			mCommProperty->DisplayAnnotationAtom->DepthBias =annotationDepthBias[i];
			mCommProperty->DisplayAnnotationAtom->Transparency =annotationTransparency[i];
		}
		else
		{
			mCommProperty->DisplayResidueName->Show =Convert::ToBoolean(bAnnotation[i]);
			mCommProperty->DisplayResidueName->TextType =(IAnnotation::ITextType)enumAnnotatonType[i];
			mCommProperty->DisplayResidueName->DisplayMethod =(IAnnotation::IDisplayMethod	)enumTextDisplayTechnique[i];
			mCommProperty->DisplayResidueName->FontHeight =logFont[i].lfHeight;
			mCommProperty->DisplayResidueName->Color =System::Drawing::Color::FromArgb(annotationColor[i]);;
			mCommProperty->DisplayResidueName->ColorScheme =(IAnnotation::IColorScheme)enumAnnotationColorScheme[i];
			mCommProperty->DisplayResidueName->RelativeXPos =annotationXPos[i];
			mCommProperty->DisplayResidueName->RelativeYPos =annotationYPos[i];
			mCommProperty->DisplayResidueName->RelativeZPos =annotationZPos[i];
			mCommProperty->DisplayResidueName->TextXPos =annotationTextXPos[i];
			mCommProperty->DisplayResidueName->TextYPos =annotationTextYPos[i];
			mCommProperty->DisplayResidueName->DepthBias =annotationDepthBias[i];
			mCommProperty->DisplayResidueName->Transparency =annotationTransparency[i];
		}
	}

	fpWorkspace->Read( &bClipping1, sizeof(BOOL));
	mCommProperty->Clipping0->Show= Convert::ToBoolean(bClipping1);

	fpWorkspace->Read( &bShowClipPlane1, sizeof(BOOL));
	mCommProperty->Clipping0->Show= Convert::ToBoolean(bShowClipPlane1);

	fpWorkspace->Read( &clipPlaneColor1, sizeof(COLORREF));
	mCommProperty->Clipping0->Color= System::Drawing::Color::FromArgb(clipPlaneColor1);

	fpWorkspace->Read( &clipPlaneTransparency1, sizeof(long));
	mCommProperty->Clipping0->Transparency=clipPlaneTransparency1;

	fpWorkspace->Read( &bClipDirection1, sizeof(BOOL));
	mCommProperty->Clipping0->Direction= Convert::ToBoolean(bClipDirection1);

	CString strPlaneEquation1;
	ReadString(*fpWorkspace, strPlaneEquation1);
	mCommProperty->Clipping0->Equation= CStringToMStr(strPlaneEquation1);

	fpWorkspace->Read( &bClipping2, sizeof(BOOL));
	mCommProperty->Clipping1->Show= Convert::ToBoolean(bClipping2);

	fpWorkspace->Read( &bShowClipPlane2, sizeof(BOOL));
	mCommProperty->Clipping1->Show= Convert::ToBoolean(bShowClipPlane2);

	fpWorkspace->Read( &clipPlaneColor2, sizeof(COLORREF));
	mCommProperty->Clipping1->Color=System::Drawing::Color::FromArgb(clipPlaneColor2);

	fpWorkspace->Read( &clipPlaneTransparency2, sizeof(long));
	mCommProperty->Clipping1->Transparency= clipPlaneTransparency2;

	fpWorkspace->Read( &bClipDirection2, sizeof(BOOL));
	mCommProperty->Clipping1->Direction= Convert::ToBoolean(bClipDirection2);

	CString strPlaneEquation2;
	ReadString(*fpWorkspace, strPlaneEquation2);
	mCommProperty->Clipping1->Equation= CStringToMStr(strPlaneEquation2);

	// 	fpWorkspace->Read(&bDepthOfField, sizeof(BOOL));
	// 	fpWorkspace->Read(&fogColor, sizeof(COLORREF));
	// 	fpWorkspace->Read(&fogStart, sizeof(long));
	// 	fpWorkspace->Read(&fogEnd, sizeof(long));

	//    pItem = pCategoryRendering->AddChildItem (m_pItemClipPS = new CXTPPropertyGridItemBool(_T("Clipping by PS"), m_bClipPS, &m_bClipPS ));
	BOOL	bClipPS;
	fpWorkspace->Read(&bClipPS, sizeof(BOOL));
	
	//    pItem = pCategoryRendering->AddChildItem(m_pItemIntensityAmbient = new CCustomItemSlider(_T("Ambient Intensity"), m_intensityAmbient, &m_intensityAmbient ));
	int		intensityAmbient;
	fpWorkspace->Read(&intensityAmbient, sizeof(int));
	mCommProperty->IntensityAmbient = intensityAmbient;
	//    pItem = pCategoryRendering->AddChildItem(m_pItemIntensityDiffuse = new CCustomItemSlider(_T("Diffuse Intensity"), m_intensiryDiffuse, &m_intensiryDiffuse ));
	int		intensiryDiffuse;
	fpWorkspace->Read(&intensiryDiffuse, sizeof(int));
	mCommProperty->DiffuseIntensity = intensiryDiffuse;
	//    pItem = pCategoryRendering->AddChildItem(m_pItemIntensitySpecular = new CCustomItemSlider(_T("Specular Intensity"), m_intensitySpecular, &m_intensitySpecular ));
	int		intensitySpecular;
	fpWorkspace->Read(&intensitySpecular, sizeof(int));
	mCommProperty->SpecularIntensity = intensitySpecular;

	BOOL		bDisplayAxisLocalCoord;
	long		axisScale;

	fpWorkspace->Read(&bDisplayAxisLocalCoord, sizeof(BOOL));
	mCommProperty->DisplayAxis = Convert::ToBoolean(bDisplayAxisLocalCoord);

	fpWorkspace->Read(&axisScale, sizeof(LONG));
	ReserveRead(fpWorkspace, m_dwReserve);
	mCommProperty->AxisSize = axisScale;

	return S_OK;
}

//=================================================================================================
//=================================================================================================

CPropertyWireframe::CPropertyWireframe(CSelectionDisplay * pSelectionDisplay)
					: CPropertyCommon(pSelectionDisplay) 
{ 
	m_pSelectionDisplay = pSelectionDisplay; 
	m_pParent = NULL; 
	m_enumDisplayMethod = 0; 
	m_lineWidth = 1;
	m_lineWidthOld = m_lineWidth;
	m_enumDisplayMode = CSelectionDisplay::WIREFRAME;
}

HRESULT	CPropertyWireframe::Save(CFile * fpWorkspace)
{
	fpWorkspace->Write(&m_enumDisplayMethod, sizeof(int));
	fpWorkspace->Write(&m_lineWidth, sizeof(long));

	ReserveWrite(fpWorkspace, m_dwReserve);
	return S_OK;
}

HRESULT	CPropertyWireframe::Load(CFile * fpWorkspace)
{
	CPIPropertyWireframe^ mWireframObj = gcnew CPIPropertyWireframe(this->m_pSelectionDisplay);
	int enumDisplayMethod;
	fpWorkspace->Read( &enumDisplayMethod, sizeof(int));
	mWireframObj->DisplayMode =( IPropertyWireframe::IDisplayMode)enumDisplayMethod;

	long lineWidth;
	fpWorkspace->Read( &lineWidth, sizeof(long));
	ReserveRead(fpWorkspace, m_dwReserve);
	mWireframObj->LineWidth = lineWidth;

	::GetMainActiveView()-> RefreshProptery(this->m_pSelectionDisplay,CSelectionDisplay::WIREFRAME);
	return S_OK;
}

CPropertyBallStick::CPropertyBallStick(CSelectionDisplay * pSelectionDisplay)
	: CPropertyCommon(pSelectionDisplay) 
{ 
	m_pSelectionDisplay = pSelectionDisplay; m_pParent = NULL;  

	m_sphereResolution = pSelectionDisplay->m_pProteinVistaRenderer->m_renderQualityPreset.m_sphereResolution;
	m_cylinderResolution = pSelectionDisplay->m_pProteinVistaRenderer->m_renderQualityPreset.m_cylinderResolution;

	m_sphereRadius = 0.5f;
	m_cylinderSize = 0.2f;

	m_enumDisplayMode = CSelectionDisplay::BALLANDSTICK;
}


HRESULT CPropertyBallStick::Load(CFile * fpWorkspace)
{
	CPIPropertyBallnStick^ mObj = gcnew CPIPropertyBallnStick(this->m_pSelectionDisplay);

	int sphereResolution;
	fpWorkspace->Read(&sphereResolution, sizeof(int));
	mObj->SphereResolution =sphereResolution;
	 
	int cylinderResolution;
	fpWorkspace->Read(&cylinderResolution, sizeof(int));
	mObj->CylinderResolution =cylinderResolution;
	 
	double sphereRadius;
	fpWorkspace->Read(&sphereRadius, sizeof(double));
	mObj->SphereRadius =sphereRadius;
	 
	double cylinderSize;
	fpWorkspace->Read(&cylinderSize, sizeof(double));
	mObj->CylinderSize = cylinderSize;
	 
	ReserveRead(fpWorkspace, m_dwReserve);
	::GetMainActiveView()-> RefreshProptery(this->m_pSelectionDisplay,CSelectionDisplay::BALLANDSTICK);
	return S_OK;
}

HRESULT CPropertyBallStick::Save(CFile * fpWorkspace)
{
	fpWorkspace->Write(&m_sphereResolution, sizeof(int));
	fpWorkspace->Write(&m_cylinderResolution, sizeof(int));
	fpWorkspace->Write(&m_sphereRadius, sizeof(double));
	fpWorkspace->Write(&m_cylinderSize, sizeof(double));

	ReserveWrite(fpWorkspace, m_dwReserve);
	return S_OK;
}

CPropertyStick::CPropertyStick(CSelectionDisplay * pSelectionDisplay)
: CPropertyCommon(pSelectionDisplay) 
{ 
	m_pSelectionDisplay = pSelectionDisplay; 
	m_pParent = NULL; 

	m_sphereResolution = pSelectionDisplay->m_pProteinVistaRenderer->m_renderQualityPreset.m_sphereResolution;
	m_cylinderResolution = pSelectionDisplay->m_pProteinVistaRenderer->m_renderQualityPreset.m_cylinderResolution;

	m_stickSize = 0.3f;
 
	m_enumDisplayMode = CSelectionDisplay::STICKS;
}


HRESULT CPropertyStick::Load(CFile * fpWorkspace)
{
	CPIPropertyStick^ mObj = gcnew CPIPropertyStick(this->m_pSelectionDisplay);

	int sphereResolution;
	fpWorkspace->Read(&sphereResolution, sizeof(int));
	mObj->SphereResolution = sphereResolution;
 
	int cylinderResolution;
	fpWorkspace->Read(&cylinderResolution, sizeof(int));
	mObj->CylinderResolution = cylinderResolution;
	 
	double stickSize;
	fpWorkspace->Read(&stickSize, sizeof(double)); 
	mObj->StickSize = stickSize;
	 
	ReserveRead(fpWorkspace, m_dwReserve);

	::GetMainActiveView()-> RefreshProptery(this->m_pSelectionDisplay,CSelectionDisplay::STICKS);
	return S_OK;
}

HRESULT CPropertyStick::Save(CFile * fpWorkspace)
{
	fpWorkspace->Write(&m_sphereResolution, sizeof(int));
	fpWorkspace->Write(&m_cylinderResolution, sizeof(int));
	fpWorkspace->Write(&m_stickSize, sizeof(double));

	ReserveWrite(fpWorkspace, m_dwReserve);
	return S_OK;
}


CPropertySpaceFill::CPropertySpaceFill(CSelectionDisplay * pSelectionDisplay)
: CPropertyCommon(pSelectionDisplay) 
{ 
	m_pSelectionDisplay = pSelectionDisplay; 
	m_pParent = NULL; 

	static FLOAT atomRadiusDefault[] = { 
		ATOM_RADIUS_C, ATOM_RADIUS_N, ATOM_RADIUS_O, ATOM_RADIUS_S, ATOM_RADIUS_H,
		ATOM_RADIUS_P, ATOM_RADIUS_CL, ATOM_RADIUS_ZN, ATOM_RADIUS_NA, ATOM_RADIUS_FE,
		ATOM_RADIUS_MG, ATOM_RADIUS_K, ATOM_RADIUS_CA, ATOM_RADIUS_I, ATOM_RADIUS_F,
		ATOM_RADIUS_B, ATOM_RADIUS_UNKNOWN };

		for ( int i = 0 ; i < MAX_ATOM ; i++ )
		{
			m_atomRadiusDefault[i] = atomRadiusDefault[i];
			m_atomRadius[i] = atomRadiusDefault[i];
		}

		m_sphereResolution = pSelectionDisplay->m_pProteinVistaRenderer->m_renderQualityPreset.m_sphereResolution;
		m_enumDisplayMode = CSelectionDisplay::SPACEFILL;
}
 
HRESULT CPropertySpaceFill::Load(CFile * fpWorkspace)
{
	CPIPropertySpaceFill^ mObj = gcnew CPIPropertySpaceFill(this->m_pSelectionDisplay);

	int sphereResolution;
	fpWorkspace->Read(&sphereResolution, sizeof(int));
	mObj->SphereResolution = sphereResolution;
 
	for ( int i = 0 ; i < MAX_ATOM ; i++ )
	{
		CString strText;
		ReadString(*fpWorkspace, strText);
		 
	}
	ReserveRead(fpWorkspace, m_dwReserve);
	::GetMainActiveView()-> RefreshProptery(this->m_pSelectionDisplay,CSelectionDisplay::SPACEFILL);
	return S_OK;
}

HRESULT CPropertySpaceFill::Save(CFile * fpWorkspace)
{
	fpWorkspace->Write(&m_sphereResolution, sizeof(int));

	//  
	for ( int i = 0 ; i < MAX_ATOM ; i++ )
	{
		CString strText;
		strText.Format("%.2f" , m_atomRadius[i]);
		WriteString(*fpWorkspace, strText);
	}

	ReserveWrite(fpWorkspace, m_dwReserve);

	return S_OK;
}

CPropertyRibbon::CPropertyRibbon(CSelectionDisplay * pSelectionDisplay):CPropertyCommon(pSelectionDisplay)
{
	m_pSelectionDisplay = pSelectionDisplay; 
	m_pParent = NULL; 

	m_curveTension = 80;
	if ( pSelectionDisplay )
		m_resolution = pSelectionDisplay->m_pProteinVistaRenderer->m_renderQualityPreset.m_ribbonResolution;

	m_strTextureFilenameCoil = GetMainApp()->m_strBaseTexturePath + _T("RibbonCoilDefault.dds");
	m_strTextureFilenameHelix = GetMainApp()->m_strBaseTexturePath + _T("RibbonHelixDefault.dds");
	m_strTextureFilenameSheet = GetMainApp()->m_strBaseTexturePath + _T("RibbonSheetDefault.dds");
 

	m_bTextureCoil = m_bTextureHelix = m_bTextureSheet = TRUE;	
	m_colorCoil = m_colorHelix = m_colorSheet = 0x00ffffff;	
	m_bDisplayCoil = m_bDisplayHelix = m_bDisplaySheet = TRUE; 
	m_sizeCoil.cx = m_sizeCoil.cy = 0.4*100;	
	m_sizeHelix.cx = (long)(1.2*100);
	m_sizeHelix.cy = (long)(1.2*100);
	m_sizeSheet.cx= (long)(1.4*100);
	m_sizeSheet.cy = (long)(0.45*100);

	m_fittingMethodHelix = 0;

	m_shapeHelix = 30;
	m_shapeCoil = 30;
	m_shapeSheet = 4;

	m_curveTensionHelix = 80;
	m_resolutionHelix = 20;

	m_curveTensionSheet = 80;
	m_resolutionSheet = 20;

	m_curveTensionCoil = 80;
	m_resolutionCoil = 20;

	m_bShowCoilOnHelix = TRUE;
	m_bShowCoilOnSheet = FALSE;

	m_textureCoordUHelix = 4;
	m_textureCoordVHelix = 8;
	
	m_textureCoordUSheet = 2;
	m_textureCoordVSheet = 8;
	
	m_textureCoordUCoil	= 1;
	m_textureCoordVCoil = 8;

	m_enumDisplayMode = CSelectionDisplay::RIBBON;
}
 
HRESULT	CPropertyRibbon::Load(CFile * fpWorkspace)
{
	CPIPropertyRibbon^ mObj = gcnew CPIPropertyRibbon(this->m_pSelectionDisplay);
	//pItem = pCategoryRendering->AddChildItem(m_pCurveTension= new CCustomItemSlider(_T("Curve Tension"), m_curveTension, &m_curveTension));
	int curveTension;
	fpWorkspace->Read(&curveTension, sizeof(int));
	mObj->CurveTension = curveTension;
	 
	//pItem = pCategoryRendering->AddChildItem(m_pResolution= new CCustomItemSlider(_T("Curve Resolution"), m_resolution, &m_resolution));
	int resolution;
	fpWorkspace->Read(&resolution, sizeof(int));
	mObj->CurveResolution = resolution;
	 
	BOOL bTexture;
	fpWorkspace->Read(&bTexture, sizeof(int));
	mObj->ShowHelix =Convert::ToBoolean(bTexture);
	 
	fpWorkspace->Read(&bTexture, sizeof(int));
	mObj->ShowSheet =Convert::ToBoolean(bTexture);
	 
	fpWorkspace->Read(&bTexture, sizeof(int));
	mObj->ShowCoil =Convert::ToBoolean(bTexture);
	 	//helix
	//pItem = pCategoryRendering->AddChildItem(m_pTextureFilenameHelix = new CCustomItemFileBox(_T("Texture Filename"), m_strTextureFilenameHelix , &m_strTextureFilenameHelix));
	CString strTextureFilenameHelix;
	ReadString(*fpWorkspace, strTextureFilenameHelix);
	CTextureInfo::ChangeTexturePathFilename(strTextureFilenameHelix, strTextureFilenameHelix);
	mObj->HelixTexture->Filename =CStringToMStr(strTextureFilenameHelix);
	 
	//pItem = pCategoryRendering->AddChildItem(m_pTextureCoordUHelix = new CCustomItemSlider(_T("Texture Coord U"), m_textureCoordUHelix, &m_textureCoordUHelix));
	int textureCoordUHelix;
	fpWorkspace->Read(&textureCoordUHelix, sizeof(int));
	mObj->HelixTexture->CoordU =textureCoordUHelix;
	 
	//pItem = pCategoryRendering->AddChildItem(m_pTextureCoordVHelix = new CCustomItemSlider(_T("Texture Coord V"), m_textureCoordVHelix, &m_textureCoordVHelix ));
	int textureCoordVHelix;
	fpWorkspace->Read(&textureCoordVHelix, sizeof(int));
	mObj->HelixTexture->CoordV = textureCoordVHelix;
	 
	//pItem = pCategoryRendering->AddChildItem(m_pcolorHelix = new CCustomItemColor(_T("Helix Color"), m_colorHelix, &m_colorHelix));		//	일종의 material color.
	COLORREF colorHelix;
	fpWorkspace->Read(&colorHelix, sizeof(COLORREF));
	mObj->HelixColor = System::Drawing::Color::FromArgb(colorHelix);
	 
	//pItem = pCategoryRendering->AddChildItem (m_penumFittingMethodHelix = new CXTPPropertyGridItemEnum(_T("Helix Fitting"), m_fittingMethodHelix, &m_fittingMethodHelix) );
	int fittingMethodHelix;
	fpWorkspace->Read(&fittingMethodHelix, sizeof(int));
	mObj->Fitting =(IPropertyHelix::IFitting)fittingMethodHelix;
	 
	//pItem = pCategoryRendering->AddChildItem (m_penumShapeHelix = new CXTPPropertyGridItemEnum(_T("Helix Shape"), m_shapeHelix, &m_shapeHelix ) );
	int shapeHelix;
	fpWorkspace->Read(&shapeHelix, sizeof(int));
	mObj->HelixShape =(IPropertyHelix::IShape)shapeHelix;
	 
	//pItem = pCategoryRendering->AddChildItem(m_pShowCoilOnHelix = new CXTPPropertyGridItemBool(_T("Show Coil On Helix"), m_bShowCoilOnHelix, &m_bShowCoilOnHelix ));
	BOOL bShowCoilOnHelix;
	fpWorkspace->Read(&bShowCoilOnHelix, sizeof(BOOL));
	mObj->ShowCoilOnHelix = Convert::ToBoolean(bShowCoilOnHelix);
	 
	////	sheet
	//pItem = pCategoryRendering->AddChildItem(m_pTextureFilenameSheet = new CCustomItemFileBox(_T("Texture Filename"), m_strTextureFilenameSheet , &m_strTextureFilenameSheet));
	CString strTextureFilenameSheet;
	ReadString(*fpWorkspace, strTextureFilenameSheet);
	CTextureInfo::ChangeTexturePathFilename(strTextureFilenameSheet, strTextureFilenameSheet);
	mObj->SheetTexture->Filename =CStringToMStr(strTextureFilenameSheet);
	 
	//pItem = pCategoryRendering->AddChildItem(m_pTextureCoordUSheet = new CCustomItemSlider(_T("Texture Coord U"), m_textureCoordUSheet, &m_textureCoordUSheet));
	int textureCoordUSheet;
	fpWorkspace->Read(&textureCoordUSheet, sizeof(int));
	mObj->SheetTexture->CoordU = textureCoordUSheet;
	 
	//pItem = pCategoryRendering->AddChildItem(m_pTextureCoordVSheet = new CCustomItemSlider(_T("Texture Coord V"), m_textureCoordVSheet, &m_textureCoordVSheet ));
	int textureCoordVSheet;
	fpWorkspace->Read(&textureCoordVSheet, sizeof(int));
	mObj->SheetTexture->CoordV = textureCoordVSheet;
	 
	//pItem = pCategoryRendering->AddChildItem(m_pcolorSheet = new CCustomItemColor(_T("Sheet Color"), m_colorSheet, &m_colorSheet));
	COLORREF colorSheet;
	fpWorkspace->Read(&colorSheet, sizeof(COLORREF));
	mObj->SheetColor = System::Drawing::Color::FromArgb(colorSheet);
	 
	//pItem = pCategoryRendering->AddChildItem (m_penumShapeSheet = new CXTPPropertyGridItemEnum(_T("Sheet Shape"), m_shapeSheet, &m_shapeSheet ) );
	int shapeSheet;
	fpWorkspace->Read(&shapeSheet, sizeof(int));
	mObj->SheetShape =(IPropertySheet::IShape)shapeSheet;
	 
	//pItem = pCategoryRendering->AddChildItem(m_pShowCoilOnSheet = new CXTPPropertyGridItemBool(_T("Show Coil On Sheet"), m_bShowCoilOnSheet, &m_bShowCoilOnSheet ));
	BOOL bShowCoilOnSheet;
	fpWorkspace->Read(&bShowCoilOnSheet, sizeof(BOOL));
	mObj->ShowCoilOnSheet = Convert::ToBoolean(bShowCoilOnSheet);
	 
	////	coil
	//pItem = pCategoryRendering->AddChildItem(m_pTextureFilenameCoil = new CCustomItemFileBox(_T("Texture Filename"), m_strTextureFilenameCoil , &m_strTextureFilenameCoil));
	CString strTextureFilenameCoil;
	ReadString(*fpWorkspace, strTextureFilenameCoil);
	CTextureInfo::ChangeTexturePathFilename(strTextureFilenameCoil, strTextureFilenameCoil);
	mObj->CoilTexture->Filename =CStringToMStr(strTextureFilenameCoil);
	 
	//pItem = pCategoryRendering->AddChildItem(m_pTextureCoordUCoil = new CCustomItemSlider(_T("Texture Coord U"), m_textureCoordUCoil, &m_textureCoordUCoil));
	int textureCoordUCoil;
	fpWorkspace->Read(&textureCoordUCoil, sizeof(int));
	mObj->CoilTexture->CoordU = textureCoordUCoil;
	 
	//pItem = pCategoryRendering->AddChildItem(m_pTextureCoordVCoil = new CCustomItemSlider(_T("Texture Coord V"), m_textureCoordVCoil, &m_textureCoordVCoil ));
	int textureCoordVCoil;
	fpWorkspace->Read(&textureCoordVCoil, sizeof(int));
	mObj->CoilTexture->CoordV = textureCoordVCoil;
	 
	//pItem = pCategoryRendering->AddChildItem(m_pcolorCoil = new CCustomItemColor(_T("Coil Color"), m_colorCoil, &m_colorCoil));
	COLORREF colorCoil;
	fpWorkspace->Read(&colorCoil, sizeof(COLORREF));
	mObj->CoilColor = System::Drawing::Color::FromArgb(colorCoil);
	 
	//pItem = pCategoryRendering->AddChildItem (m_penumShapeCoil = new CXTPPropertyGridItemEnum(_T("Coil Shape"), m_shapeCoil, &m_shapeCoil ) );
	int shapeCoil;
	fpWorkspace->Read(&shapeCoil, sizeof(int));
	mObj->CoilShape =(IPropertyCoil::IShape)shapeCoil;
	 
	//
	SIZE sizeHelix;
	fpWorkspace->Read(&sizeHelix, sizeof(SIZE));
	mObj->HelixSize = System::Drawing::Size(sizeHelix.cx,sizeHelix.cy);
	 
	SIZE sizeSheet;
	fpWorkspace->Read(&sizeSheet, sizeof(SIZE));
	mObj->SheetSize = System::Drawing::Size(sizeSheet.cx,sizeSheet.cy);
	 
	SIZE sizeCoil;
	fpWorkspace->Read(&sizeCoil, sizeof(SIZE));
	mObj->CoilSize = System::Drawing::Size(sizeCoil.cx,sizeCoil.cy);
	  
	BOOL	display;
	fpWorkspace->Read(&display, sizeof(int));
	mObj->HelixTexture->ShowTexture = Convert::ToBoolean(display);
	 
	fpWorkspace->Read(&display, sizeof(int));
	mObj->SheetTexture->ShowTexture = Convert::ToBoolean(display);
	 
	fpWorkspace->Read(&display, sizeof(int));
	mObj->CoilTexture->ShowTexture = Convert::ToBoolean(display);
	 
	ReserveRead(fpWorkspace, m_dwReserve);

	::GetMainActiveView()-> RefreshProptery(this->m_pSelectionDisplay,CSelectionDisplay::RIBBON);
	return S_OK;
}

HRESULT	CPropertyRibbon::Save(CFile * fpWorkspace)
{
	fpWorkspace->Write(&m_curveTension, sizeof(int));
	fpWorkspace->Write(&m_resolution, sizeof(int));

	fpWorkspace->Write(&m_bTextureHelix , sizeof(int));
	fpWorkspace->Write(&m_bTextureSheet, sizeof(int));
	fpWorkspace->Write(&m_bTextureCoil, sizeof(int));

	WriteString(*fpWorkspace, m_strTextureFilenameHelix);
	fpWorkspace->Write(&m_textureCoordUHelix, sizeof(int));
	fpWorkspace->Write(&m_textureCoordVHelix, sizeof(int));
	fpWorkspace->Write(&m_colorHelix, sizeof(COLORREF));
	fpWorkspace->Write(&m_fittingMethodHelix, sizeof(int));
	fpWorkspace->Write(&m_shapeHelix, sizeof(int));
	fpWorkspace->Write(&m_bShowCoilOnHelix, sizeof(BOOL));
	WriteString(*fpWorkspace, m_strTextureFilenameSheet);
	fpWorkspace->Write(&m_textureCoordUSheet, sizeof(int));
	fpWorkspace->Write(&m_textureCoordVSheet, sizeof(int));
	fpWorkspace->Write(&m_colorSheet, sizeof(COLORREF));
	fpWorkspace->Write(&m_shapeSheet, sizeof(int));
	fpWorkspace->Write(&m_bShowCoilOnSheet, sizeof(BOOL));
	WriteString(*fpWorkspace, m_strTextureFilenameCoil);
	fpWorkspace->Write(&m_textureCoordUCoil, sizeof(int));
	fpWorkspace->Write(&m_textureCoordVCoil, sizeof(int));
	fpWorkspace->Write(&m_colorCoil, sizeof(COLORREF));
	fpWorkspace->Write(&m_shapeCoil, sizeof(int));

	fpWorkspace->Write(&m_sizeHelix, sizeof(SIZE));
	fpWorkspace->Write(&m_sizeSheet, sizeof(SIZE));
	fpWorkspace->Write(&m_sizeCoil, sizeof(SIZE));

	fpWorkspace->Write(&m_bDisplayHelix, sizeof(int));
	fpWorkspace->Write(&m_bDisplaySheet, sizeof(int));
	fpWorkspace->Write(&m_bDisplayCoil, sizeof(int));

	ReserveWrite(fpWorkspace, m_dwReserve);

	return S_OK;
}

CPropertySurface::CPropertySurface(CSelectionDisplay * pSelectionDisplay)
	: CPropertyCommon(pSelectionDisplay) 
{ 
	m_pSelectionDisplay = pSelectionDisplay; 
	m_pParent = NULL; 

	m_enumSurfaceDisplayMethod = 0; 
	m_transparency = 100; 
	m_bDisplayCurvature = FALSE; 
	m_curvatureRingSize = 0; 
	m_probeSphere = 1.5; 
	m_surfaceQuality = pSelectionDisplay->m_pProteinVistaRenderer->m_renderQualityPreset.m_surfaceQuality;
	m_bSurfaceDepthSort = TRUE; 
	m_iSurfaceBlurring = 0; 
	m_useInnerFaceColor = FALSE; 
	m_colorInnerFace = RGB(255,255,0); 
	m_blendFactor = 100;
	m_bSurfaceCutPlaneCap = FALSE;
	m_bAddHETATM = TRUE;
	m_surfaceGenMethod = pSelectionDisplay->m_pProteinVistaRenderer->m_surfaceGenAlgoritm;
	m_enumDisplayMode = CSelectionDisplay::SURFACE;

	//
	//    m_arrayColorRowCurvature; 에 color를 설정한다.
	//    
	D3DXCOLOR diffuse1(CColorSchemeDefault::ConvertHSIToRGB(0.0f, 1.0f, 0.5f));
	D3DXCOLOR ambient1 = CColorSchemeDefault::GetAmbientColorFromDiffuse(diffuse1);
	D3DXCOLOR specular1 = CColorSchemeDefault::GetSpecularColorFromDiffuse(diffuse1);
	CColorRow colorRow1( CString("Color 1"), diffuse1 );

	D3DXCOLOR diffuse2(CColorSchemeDefault::ConvertHSIToRGB(212/255.0f, 255/255.0f, 127/255.0f));
	D3DXCOLOR ambient2 = CColorSchemeDefault::GetAmbientColorFromDiffuse(diffuse2);
	D3DXCOLOR specular2 = CColorSchemeDefault::GetSpecularColorFromDiffuse(diffuse2);
	CColorRow colorRow2( CString("Color 2"), diffuse2 );

	m_arrayColorRowCurvature.push_back(new CColorRow(colorRow1));
	m_arrayColorRowCurvature.push_back(new CColorRow(colorRow2));

	m_arrayColorRowCurvatureDefault.push_back(new CColorRow(colorRow1));
	m_arrayColorRowCurvatureDefault.push_back(new CColorRow(colorRow2));
}

CPropertySurface::~CPropertySurface() 
{
	for ( int i = 0 ; i < m_arrayColorRowCurvature.size(); i++ )
		delete m_arrayColorRowCurvature[i];
	m_arrayColorRowCurvature.clear();

	for ( int i = 0 ; i < m_arrayColorRowCurvatureDefault.size(); i++ )
		delete m_arrayColorRowCurvatureDefault[i];
	m_arrayColorRowCurvatureDefault.clear();
}
HRESULT CPropertySurface::InitProperty(int modeValue)
{
    D3DCOLOR color1, color2, color3;
	if(modeValue ==0)
	{
		color1 = RGB(255,0,0); // red
		color2 = RGB(255,255,255); // white
		color3 = RGB(0,0,255); // blue

		return S_OK;
	}
	color1 = RGB(0,0,0); // red
	color2 = RGB(255,255,255); // white
	color3 = RGB(255,255,255); // blue

	return S_OK;
}

 
HRESULT	CPropertySurface::Load(CFile * fpWorkspace)
{
	CPIPropertySurface^ mObj = gcnew CPIPropertySurface(this->m_pSelectionDisplay);
	int			enumSurfaceDisplayMethod;
	long		transparency;			//	0 이면 불투명.
	BOOL		bDisplayCurvate;
	long		curvatureRingSize;
	long		colorListSize;
	D3DCOLOR	color;

	double		probeSphere;
	int			surfaceQuality;
	BOOL		bAddHETATM;
	BOOL		surfaceGenMethod;
	
	int			iSurfaceBlurring;
	BOOL		bSurfaceDepthSort;
	int			useInnerFaceColor;
	int			blendFactor;
	COLORREF	colorInnerFace;

	fpWorkspace->Read(&enumSurfaceDisplayMethod, sizeof(int));
	mObj->DisplayMethod =(IPropertySurface::IDisplayMethod)enumSurfaceDisplayMethod;

	fpWorkspace->Read(&transparency, sizeof(long));
	mObj->Transparency =transparency;

	fpWorkspace->Read(&bDisplayCurvate, sizeof(BOOL));
	//mObj->disp =transparency;

	fpWorkspace->Read(&curvatureRingSize, sizeof(long));
	mObj->CurvatureRingSize =(IPropertySurface::ICurvatureRingSize)curvatureRingSize;

	fpWorkspace->Read(&probeSphere, sizeof(DOUBLE));
	mObj->ProbeSphereRadius =probeSphere;

	fpWorkspace->Read(&surfaceQuality, sizeof(int));
	mObj->GeometryQuality =(IProperty::IGeometryQuality)surfaceQuality;

	fpWorkspace->Read(&bAddHETATM, sizeof(BOOL));
	mObj->AddHETATM =Convert::ToBoolean(bAddHETATM);

	fpWorkspace->Read(&surfaceGenMethod, sizeof(BOOL));
	mObj->Algorithm =(IPropertySurface::IAlgorithm)surfaceGenMethod;

	fpWorkspace->Read(&iSurfaceBlurring, sizeof(int));
	mObj->ColorSmoothing =(IPropertySurface::IColorSmoothing)iSurfaceBlurring;

	fpWorkspace->Read(&bSurfaceDepthSort, sizeof(BOOL));
	mObj->DepthSort =Convert::ToBoolean(bSurfaceDepthSort);

	fpWorkspace->Read(&useInnerFaceColor, sizeof(int));
	mObj->UseInnerFaceColor =Convert::ToBoolean(useInnerFaceColor);

	fpWorkspace->Read(&blendFactor, sizeof(int));
	mObj->InnerFaceColorBlend = blendFactor;

	fpWorkspace->Read(&colorInnerFace, sizeof(COLORREF));
	mObj->InnerFaceColor =System::Drawing::Color::FromArgb(colorInnerFace); 

	//    
	CArrayColorRow & arrayColorRow = m_arrayColorRowCurvature;
	int lenColors;
	fpWorkspace->Read( &lenColors, sizeof(int) );
	arrayColorRow.resize(lenColors);

	for ( int j = 0; j < arrayColorRow.size(); j++ )
	{
		if ( arrayColorRow[j] == NULL )
			arrayColorRow[j] = new CColorRow;

		CColorRow * colorRow = arrayColorRow[j];

		CString colorName;
		ReadString(*fpWorkspace, colorName);
		colorRow->m_name = colorName;
		DWORD color0;
		fpWorkspace->Read( &color0, sizeof(DWORD) );
		colorRow->m_color = D3DXCOLOR(color0);
	}
	ReserveRead(fpWorkspace, m_dwReserve);
	::GetMainActiveView()-> RefreshProptery(this->m_pSelectionDisplay,CSelectionDisplay::SURFACE);
	return S_OK;
}

HRESULT	CPropertySurface::Save(CFile * fpWorkspace)
{
	long colorListSize;
	
	fpWorkspace->Write(&m_enumSurfaceDisplayMethod, sizeof(int));
	fpWorkspace->Write(&m_transparency, sizeof(long));
	fpWorkspace->Write(&m_bDisplayCurvature, sizeof(BOOL));
	fpWorkspace->Write(&m_curvatureRingSize, sizeof(long));

	fpWorkspace->Write(&m_probeSphere, sizeof(DOUBLE));
	fpWorkspace->Write(&m_surfaceQuality, sizeof(int));
	fpWorkspace->Write(&m_bAddHETATM , sizeof(BOOL));
	fpWorkspace->Write(&m_surfaceGenMethod, sizeof(BOOL));

	fpWorkspace->Write(&m_iSurfaceBlurring, sizeof(int));
	fpWorkspace->Write(&m_bSurfaceDepthSort, sizeof(BOOL));

	fpWorkspace->Write(&m_useInnerFaceColor, sizeof(int));
	fpWorkspace->Write(&m_blendFactor, sizeof(int));
	fpWorkspace->Write(&m_colorInnerFace, sizeof(COLORREF));
    
	CArrayColorRow & arrayColorRow = m_arrayColorRowCurvature;

	int lenColors = arrayColorRow.size();
	fpWorkspace->Write( &lenColors, sizeof(int));

	for ( int j = 0; j < arrayColorRow.size(); j++ )
	{
		CColorRow * colorRow = arrayColorRow[j];

		CString colorName(colorRow->m_name);
		WriteString(*fpWorkspace, colorName );
		DWORD color0 = (DWORD)(colorRow->m_color);		fpWorkspace->Write( &color0, sizeof(DWORD) );
	}

	ReserveWrite(fpWorkspace, m_dwReserve);

	return S_OK;
}

 