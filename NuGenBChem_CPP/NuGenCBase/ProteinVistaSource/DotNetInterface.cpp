#include "stdafx.h"
#include "ProteinVista.h"

#include "DotNetInterface.h"
#include "Interface.h"
#include "PDBRenderer.h"
#include "ProteinVistaRenderer.h"
#include "ProteinVistaView.h"
#include "SelectionDisplay.h"
#include "MatrixMath.h"
#include "Utils.h"

#ifdef _MANAGED
void ForceRenderScene()
{
	CProteinVistaRenderer * pProteinVistaRenderer = GetProteinVistaRenderer();	
	if ( pProteinVistaRenderer )
	{
		pProteinVistaRenderer->Render3DEnvironment();
	}
	PumpMessage();
}
#define ManagedColor2COLORREF(_value) RGB(_value.R, _value.G, _value.B)
void CDotNetPropertyScene::Init()
{
	m_clipping = gcnew CDotNetClipping(NULL, 0); 

	m_arrayLights = gcnew array <ILight ^> (2);

	CDotNetLight ^ light1 = gcnew CDotNetLight(0);
	light1->Init();
	m_arrayLights[0] = light1;

	CDotNetLight ^ light2 = gcnew CDotNetLight(1);
	light2->Init();
	m_arrayLights[1] = light2;
}

Color CDotNetPropertyScene::BackgroundColor::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return System::Drawing::Color::FromArgb(pProteinVistaRenderer->m_pPropertyScene->m_colorBackroundColor);
	}
	return Color::White;
}

void CDotNetPropertyScene::BackgroundColor::set(Color color)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemcolorBackroundColor->SetColor( ManagedColor2COLORREF(color) );
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemcolorBackroundColor->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemcolorBackroundColor)) ;
	}
	ForceRenderScene(); 
}

bool CDotNetPropertyScene::ShowBackgroundTexture::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bUseBackgroundTexture);
	}
	return true;
}

void CDotNetPropertyScene::ShowBackgroundTexture::set(bool show)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItembUseBackgroundTexture->SetBool(show);
		//pProteinVistaRenderer->m_pPropertyScene->m_pItembUseBackgroundTexture->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItembUseBackgroundTexture) );
	}
	ForceRenderScene(); 
}

String ^ CDotNetPropertyScene::BackgroundTextureFilename::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return gcnew String (pProteinVistaRenderer->m_pPropertyScene->m_strBackgroundTextureFilename);
	}
	return nullptr;
}

void CDotNetPropertyScene::BackgroundTextureFilename::set(String ^ show)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemstrBackgroundTexture->SetValue(show);
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemstrBackgroundTexture->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemstrBackgroundTexture) );
	}
	ForceRenderScene(); 
}

//	SSAO
bool CDotNetPropertyScene::EnableAO::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bUseSSAO);
	}

	return true;
}

void CDotNetPropertyScene::EnableAO::set(bool _value)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItembUseSSAO->SetBool(_value);
		//pProteinVistaRenderer->m_pPropertyScene->m_pItembUseSSAO->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItembUseSSAO)) ;
	}
	ForceRenderScene(); 
}

int	CDotNetPropertyScene::AOSampling::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return Convert::ToInt32(pProteinVistaRenderer->m_pPropertyScene->m_numSSAOSampling);
	}

	return 0;
}

void CDotNetPropertyScene::AOSampling::set(int _value)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemNumSSAOSampling->SetNumber(_value);
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemNumSSAOSampling->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemNumSSAOSampling)) ;
	}
	ForceRenderScene(); 
}

int	CDotNetPropertyScene::AORange::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return Convert::ToInt32(pProteinVistaRenderer->m_pPropertyScene->m_ssaoRange);
	}

	return 0;
}

void CDotNetPropertyScene::AORange::set(int _value)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemSSAORange->SetNumber(_value);
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemSSAORange->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemSSAORange)) ;
	}
	ForceRenderScene(); 
}

int	CDotNetPropertyScene::AOIntensity::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return Convert::ToInt32(pProteinVistaRenderer->m_pPropertyScene->m_ssaoIntensity);
	}

	return 0;
}

void CDotNetPropertyScene::AOIntensity::set(int _value)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemSSAOIntensity->SetNumber(_value);
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemSSAOIntensity->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemSSAOIntensity)) ;
	}
	ForceRenderScene(); 
}

IPropertyScene::IAOBlurType	CDotNetPropertyScene::AOBlurType::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return static_cast<IPropertyScene::IAOBlurType>(pProteinVistaRenderer->m_pPropertyScene->m_ssaoBlurType);
	}

	return IPropertyScene::IAOBlurType::None;
}

void CDotNetPropertyScene::AOBlurType::set(IPropertyScene::IAOBlurType _value)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemSSAOBlurType->SetEnum(Convert::ToInt32(_value));
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemSSAOBlurType->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemSSAOBlurType)) ;
	}
	ForceRenderScene(); 
}


bool CDotNetPropertyScene::AOFullSizeBuffer::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bUseFullSizeBlur);
	}

	return true;
}

void CDotNetPropertyScene::AOFullSizeBuffer::set(bool _value)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItembHalfSizeBlur->SetBool(_value);
		//pProteinVistaRenderer->m_pPropertyScene->m_pItembHalfSizeBlur->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItembHalfSizeBlur)) ;
	}
	ForceRenderScene(); 
}

bool CDotNetPropertyScene::DepthOfField::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bDepthOfField);
	}

	return true;
}

void CDotNetPropertyScene::DepthOfField::set(bool dof)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItembDepthOfField->SetBool(dof);
		//pProteinVistaRenderer->m_pPropertyScene->m_pItembDepthOfField->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItembDepthOfField)) ;
	}
	ForceRenderScene(); 
}

Color CDotNetPropertyScene::FogColor::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return System::Drawing::Color::FromArgb(pProteinVistaRenderer->m_pPropertyScene->m_fogColor);
	}
	return Color::White;
}

void CDotNetPropertyScene::FogColor::set(Color color)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemFogColor->SetColor(color.ToArgb());
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemFogColor->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemFogColor));
	}
	ForceRenderScene(); 
}

int CDotNetPropertyScene::FogStart::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return pProteinVistaRenderer->m_pPropertyScene->m_fogStart;
	}
	return 0;
}

void CDotNetPropertyScene::FogStart::set(int start)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemFogStart->SetNumber(start);
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemFogStart->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemFogStart));
	}
	ForceRenderScene(); 
}

int CDotNetPropertyScene::FogEnd::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return pProteinVistaRenderer->m_pPropertyScene->m_fogEnd;
	}
	return 1000;
}

void CDotNetPropertyScene::FogEnd::set(int end)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemFogEnd->SetNumber(end);
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemFogEnd->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemFogEnd));
	}
	ForceRenderScene(); 
}

Vector3	CDotNetPropertyScene::CameraPosition::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		Vector3 pos;
		pos.X = pProteinVistaRenderer->m_FromVec.x;
		pos.Y = pProteinVistaRenderer->m_FromVec.y;
		pos.Z = pProteinVistaRenderer->m_FromVec.z;
		return pos;
	}

	return Vector3(0,0,0);
}

void CDotNetPropertyScene::CameraPosition::set(Vector3	pos)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		D3DXVECTOR3 cameraPos(pos.X, pos.Y, pos.Z);
		pProteinVistaRenderer->SetCameraPos(cameraPos);
		ForceRenderScene(); 
	}
}

IPropertyScene::ICameraType	CDotNetPropertyScene::CameraType::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return static_cast<IPropertyScene::ICameraType>(pProteinVistaRenderer->m_pPropertyScene->m_cameraType);
	}

	return IPropertyScene::ICameraType::Perspective;
}


void CDotNetPropertyScene::CameraType::set (IPropertyScene::ICameraType cameraType)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemCameraType->SetEnum(Convert::ToInt32(cameraType));
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemCameraType->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemCameraType));
	}
	ForceRenderScene();
}

int	CDotNetPropertyScene::FOV::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return pProteinVistaRenderer->m_pPropertyScene->m_lFOV;
	}
	return 50;
}

void CDotNetPropertyScene::FOV::set(int fov)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//	이렇게 해야 border 가 걸러진다.
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemFOV->SetNumber(Convert::ToInt32(fov));
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemFOV->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemFOV));
	}
	ForceRenderScene();
}

int	CDotNetPropertyScene::SizeViewVol::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return pProteinVistaRenderer->m_pPropertyScene->m_othoCameraViewVol;
	}
	return 50;
}

void CDotNetPropertyScene::SizeViewVol::set(int sizeViewVol)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//	이렇게 해야 border 가 걸러진다.
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemOthoCameraViewVol->SetNumber(Convert::ToInt32(sizeViewVol));
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemOthoCameraViewVol->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemOthoCameraViewVol));
	}
	ForceRenderScene();
}


//
IPropertyScene::IGeometryQuality CDotNetPropertyScene::GeometryQuality::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return static_cast<IPropertyScene::IGeometryQuality>(pProteinVistaRenderer->m_pPropertyScene->m_modelQuality);
	}

	return IPropertyScene::IGeometryQuality::Low;
}

void CDotNetPropertyScene::GeometryQuality::set(IPropertyScene::IGeometryQuality value)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemEnumModelQuality->SetEnum(Convert::ToInt32(value));
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemEnumModelQuality->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemEnumModelQuality));
	}
	ForceRenderScene(); 
}

IPropertyScene::IAntiAliasing CDotNetPropertyScene::AntiAliasing::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return static_cast<IPropertyScene::IAntiAliasing>(pProteinVistaRenderer->m_pPropertyScene->m_iAntialiasing);
	}
	return IPropertyScene::IAntiAliasing::None;
}

void CDotNetPropertyScene::AntiAliasing::set(IPropertyScene::IAntiAliasing aa)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemiAntialiasing->SetEnum(Convert::ToInt32(aa));
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemiAntialiasing->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemiAntialiasing));
	}
	ForceRenderScene(); 
}

//
IPropertyScene::IShaderQuality CDotNetPropertyScene::ShaderQuality::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return static_cast<IPropertyScene::IShaderQuality>(pProteinVistaRenderer->m_pPropertyScene->m_shaderQuality);
	}

	return IPropertyScene::IShaderQuality::Low;
}

void CDotNetPropertyScene::ShaderQuality::set(IPropertyScene::IShaderQuality quality)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemEnumShaderQuality->SetEnum(Convert::ToInt32(quality));
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemEnumShaderQuality->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemEnumShaderQuality));
	}
	ForceRenderScene(); 
}

bool CDotNetPropertyScene::ShowSelectionMark::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bShowSelectionMark);
	}

	return false;
}

void CDotNetPropertyScene::ShowSelectionMark::set(bool value)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemShowSelectionMark->SetBool(Convert::ToInt32(value));
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemShowSelectionMark->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemShowSelectionMark));
	}
	ForceRenderScene(); 
}

//====================================================================================================
//====================================================================================================
bool CDotNetPropertyScene::DisplayAxis::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bDisplayAxis);
	}
	return true;
}

void CDotNetPropertyScene::DisplayAxis::set(bool display)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItembDisplayAxis->SetBool(display);
		//pProteinVistaRenderer->m_pPropertyScene->m_pItembDisplayAxis->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItembDisplayAxis));
	}
	ForceRenderScene(); 
}

int CDotNetPropertyScene::AxisSize::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return pProteinVistaRenderer->m_pPropertyScene->m_axisScale;
	}
	return 50;
}

void CDotNetPropertyScene::AxisSize::set(int size)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//pProteinVistaRenderer->m_pPropertyScene->m_pItemAxisScale->SetNumber(size);
	}
	ForceRenderScene(); 
}


double CDotNetPropertyScene::ClipPlaneNear::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return pProteinVistaRenderer->m_fNearClipPlane;
	}

	return 0.0;
}

void CDotNetPropertyScene::ClipPlaneNear::set(double nearPlane)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//	pProteinVistaRenderer->m_pPropertyScene->m_pItemfNearClipPlane->SetDouble( nearPlane );
		//	pProteinVistaRenderer->m_pPropertyScene->m_pItemfNearClipPlane->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemfNearClipPlane));
	}
	ForceRenderScene();
}

double CDotNetPropertyScene::ClipPlaneFar::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		return pProteinVistaRenderer->m_fFarClipPlane;
	}

	return 1000.0;
}

void CDotNetPropertyScene::ClipPlaneFar::set(double farPlane)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		//	pProteinVistaRenderer->m_pPropertyScene->m_pItemfFarClipPlane->SetDouble(farPlane);
		//	pProteinVistaRenderer->m_pPropertyScene->m_pItemfFarClipPlane->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemfFarClipPlane));
	}
	ForceRenderScene(); 
}

//	only camera animation.
void CDotNetPropertyScene::CameraAnimation(IAtom ^ atom, float time)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		pProteinVistaRenderer->SetCameraAnimation((dynamic_cast<CDotNetAtom ^> (atom))->GetUnManagedAtom(), time);
	}
}

void CDotNetPropertyScene::CameraAnimation(IResidue ^ residue, float time)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		pProteinVistaRenderer->SetCameraAnimation((dynamic_cast<CDotNetResidue ^> (residue))->GetUnManagedResidue(), time);
	}
}

void CDotNetPropertyScene::CameraAnimation(Vector3 pos , float time)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		D3DXVECTOR3 endPos(pos.X, pos.Y, pos.Z);
		pProteinVistaRenderer->SetCameraAnimation(endPos, time);
	}
}

void CDotNetPropertyScene::CameraAnimation(IVP ^ vp, float time)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		CDotNetSelection ^ selection = dynamic_cast<CDotNetSelection ^> (vp);
		if ( selection != nullptr )
		{
			pProteinVistaRenderer->SetCameraAnimation( selection->m_pSelectionDisplay->m_arrayAtomInst , time );
		}
	}
}


void CDotNetPropertyScene::CameraAnimation()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		pProteinVistaRenderer->SetCameraAnimation();
	}
}
CDotNetProperty::CDotNetProperty(CSelectionDisplay * pSelectionDisplay)
{
	m_pSelectionDisplay = pSelectionDisplay;
}

void CDotNetProperty::Init()
{
	m_arrayClippings = gcnew array <IClipping ^ >(2);

	CDotNetClipping ^ pClipping1 = gcnew CDotNetClipping(m_pSelectionDisplay, 1);
	pClipping1->Init();
	m_arrayClippings[0] = pClipping1;

	CDotNetClipping ^ pClipping2 = gcnew CDotNetClipping(m_pSelectionDisplay, 2);
	pClipping2->Init();
	m_arrayClippings[1] = pClipping2;

	m_annotationVP = gcnew CDotNetAnnotation(m_pSelectionDisplay, CSelectionDisplay::ANNOTATION_VP );
	m_annotationAtom = gcnew CDotNetAnnotation(m_pSelectionDisplay, CSelectionDisplay::ANNOTATION_ATOM );
	m_annotationResidue = gcnew CDotNetAnnotation(m_pSelectionDisplay, CSelectionDisplay::ANNOTATION_RESIDUE );
}

String ^ CDotNetProperty::Name::get()
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		return gcnew String(pPropertyCommon->m_strSelectionName);
	}

	return nullptr;
}

void CDotNetProperty::Name::set(String ^ name)
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		//pPropertyCommon->m_pItempSelectionName->SetValue(name);
		//pPropertyCommon->m_pItempSelectionName->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItempSelectionName) );
	}
	ForceRenderScene(); 
}

bool CDotNetProperty::DisplaySideChain::get()
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		return Convert::ToBoolean(pPropertyCommon->m_bDisplaySideChain);
	}

	return true;
}

void CDotNetProperty::DisplaySideChain::set(bool display)
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		//pPropertyCommon->m_pItempDisplaySideChain->SetBool(display);
		//pPropertyCommon->m_pItempDisplaySideChain->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItempDisplaySideChain) );
	}
	ForceRenderScene(); 
}

List<System::Drawing::Color> ^ CDotNetProperty::CustomizeColors::get()
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		int iScheme = pPropertyCommon->m_enumColorScheme;
		CArrayColorRow & listColorRow = pPropertyCommon->m_arrayColorScheme[iScheme];

		List<System::Drawing::Color> ^ listColor = gcnew List<System::Drawing::Color>(listColorRow.size());

		for ( int i = 0 ; i < listColorRow.size() ; i++ )
		{
			//	TODO:
			listColor->Add( System::Drawing::Color::FromArgb(listColorRow[i]->m_color) );
		}

		return listColor;
	}

	return nullptr;
}

void CDotNetProperty::CustomizeColors::set(List<System::Drawing::Color> ^ listColor)
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		int iScheme = pPropertyCommon->m_enumColorScheme;
		CArrayColorRow & arrayColorRow = pPropertyCommon->m_arrayColorScheme[iScheme];

		if ( iScheme == COLOR_SCHEME_OCCUPANCY || iScheme == COLOR_SCHEME_TEMPARATURE || iScheme == COLOR_SCHEME_PROGRESSIVE || iScheme == COLOR_SCHEME_HYDROPATHY )
		{	//	변동길이. arrayColor 대로 추가한다.
			for ( int i = 0 ; i < arrayColorRow.size() ; i++ )
			{
				SAFE_DELETE(arrayColorRow[i]);
			}

			arrayColorRow.clear();
			arrayColorRow.reserve(listColor->Count);

			for ( int i = 0 ; i < listColor->Count ; i++ )
			{
				CString strColorName;
				strColorName.Format("Color %d", i+1 );
				D3DXCOLOR diffuse = D3DCOLOR_ARGB(0, listColor[i].R, listColor[i].G, listColor[i].B);

				CColorRow * pColorRow = new CColorRow(strColorName, diffuse );
				arrayColorRow.push_back( pColorRow );
			}
		}
		else
		{
			//	고정길이 overwrite 하고, 남거나 모자르는것 무시.
			for ( int i = 0; i < listColor->Count ; i++ )
			{
				if ( i >= arrayColorRow.size() ) 
					break;

				arrayColorRow[i]->m_color = D3DCOLOR_ARGB(0, listColor[i].R, listColor[i].G, listColor[i].B);
			}
		}

		m_pSelectionDisplay->UpdateAtomPosColorChanged();
		m_pSelectionDisplay->UpdateAnnotation();

	}
}

IProperty::IColorScheme CDotNetProperty::ColorScheme::get()
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		return static_cast<IProperty::IColorScheme>(pPropertyCommon->m_enumColorScheme);
	}

	return IProperty::IColorScheme::CPK;
}


void CDotNetProperty::ColorScheme::set(IProperty::IColorScheme colorScheme)
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		//pPropertyCommon->m_pItemColorScheme->SetEnum(Convert::ToInt32(colorScheme));
		//pPropertyCommon->m_pItemColorScheme->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemColorScheme) );
	}
	ForceRenderScene(); 
}

bool CDotNetProperty::ShowIndicateSelectionMark::get()
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		return Convert::ToBoolean(pPropertyCommon->m_bIndicate);
	}
	return true;
}

void CDotNetProperty::ShowIndicateSelectionMark::set(bool value)
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		//pPropertyCommon->m_pItempItemIndicate->SetBool(value);
		//pPropertyCommon->m_pItempItemIndicate->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItempItemIndicate) );
	}
	ForceRenderScene();
}

System::Drawing::Color CDotNetProperty::IndicateSelectionMarkColor::get()
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		return System::Drawing::Color::FromArgb(pPropertyCommon->m_indicateColor);
	}

	return System::Drawing::Color::White;
}

void CDotNetProperty::IndicateSelectionMarkColor::set(System::Drawing::Color _value)
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		//pPropertyCommon->m_pItemindicateColor->SetColor(ManagedColor2COLORREF(_value));
		//pPropertyCommon->m_pItemindicateColor->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemindicateColor) );
	}
	ForceRenderScene();
}

bool CDotNetProperty::DisplayAxis::get()
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		return Convert::ToBoolean(pPropertyCommon->m_bDisplayAxisLocalCoord);
	}

	return true;
}

void CDotNetProperty::DisplayAxis::set(bool display)
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		//pPropertyCommon->m_pItempShowLocalAxis->SetBool(display);
	}

	ForceRenderScene();
}

int CDotNetProperty::AxisSize::get()
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		return pPropertyCommon->m_axisScaleLocalCoord;
	}

	return 50;
}

void CDotNetProperty::AxisSize::set(int size)
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		//pPropertyCommon->m_pItemAxisScale->SetNumber(size);
	}

	ForceRenderScene();
}


//bool CDotNetProperty::AnnotationShow::get()
//{
//	//	TODO: 수정
//	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
//	if ( pPropertyCommon )
//	{
//		return Convert::ToBoolean(pPropertyCommon->m_bAnnotation[0]);
//	}
//
//	return true;
//}
//
//void CDotNetProperty::AnnotationShow::set(bool annotation)
//{
//	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
//	if ( pPropertyCommon )
//	{
//		pPropertyCommon->m_pItemShowAnnotation[0]->SetBool(annotation);
//		pPropertyCommon->m_pItemShowAnnotation[0]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemShowAnnotation[0]) );
//	}
//	ForceRenderScene(); 
//}
//
//String ^ CDotNetProperty::AnnotationName::get()
//{
//	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
//	if ( pPropertyCommon )
//	{
//		return gcnew String(pPropertyCommon->m_strAnnotation[0]);
//	}
//
//	return nullptr;
//}
//
//
//void CDotNetProperty::AnnotationName::set(String ^ name)
//{
//	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
//	if ( pPropertyCommon )
//	{
//		pPropertyCommon->m_pItemstrAnnotation[0]->SetValue(name);
//		pPropertyCommon->m_pItemstrAnnotation[0]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemstrAnnotation[0]) );
//	}
//	ForceRenderScene(); 
//}
//
//Color CDotNetProperty::AnnotationColor::get()
//{
//	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
//	if ( pPropertyCommon )
//	{
//		return System::Drawing::Color::FromArgb(pPropertyCommon->m_annotationColor[0]);
//	}
//
//	return Color::Aqua;
//}
//
//void CDotNetProperty::AnnotationColor::set(Color color)
//{
//	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
//	if ( pPropertyCommon )
//	{
//		pPropertyCommon->m_pItemannotationColor[0]->SetColor( ManagedColor2COLORREF(color) );
//		pPropertyCommon->m_pItemannotationColor[0]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemannotationColor[0]) );
//	}
//	ForceRenderScene(); 
//}

//
//
IProperty::IGeometryQuality CDotNetProperty::GeometryQuality::get()
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		return static_cast<IProperty::IGeometryQuality>(pPropertyCommon->m_modelQuality);
	}

	return IProperty::IGeometryQuality::High;
}

void CDotNetProperty::GeometryQuality::set(IProperty::IGeometryQuality value)
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		//pPropertyCommon->m_pItemEnumModelQuality->SetEnum(Convert::ToInt32(value));
		//pPropertyCommon->m_pItemEnumModelQuality->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemEnumModelQuality) );
	}
	ForceRenderScene(); 
}

//
IProperty::IShaderQuality CDotNetProperty::ShaderQuality::get()
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		return static_cast<IProperty::IShaderQuality>(pPropertyCommon->m_shaderQuality);
	}
	return IProperty::IShaderQuality::High;
}

void CDotNetProperty::ShaderQuality::set(IProperty::IShaderQuality value)
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		//pPropertyCommon->m_pItemEnumShaderQuality->SetEnum(Convert::ToInt32(value));
		//pPropertyCommon->m_pItemEnumShaderQuality->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemEnumShaderQuality) );
	}
	ForceRenderScene(); 
}

//
bool CDotNetProperty::ShowSelectionMark::get()
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		return Convert::ToBoolean ( pPropertyCommon->m_bShowSelectionMark ) ;
	}

	return true;
}

void CDotNetProperty::ShowSelectionMark::set(bool value)
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		//pPropertyCommon->m_pItemShowSelectionMark->SetBool(Convert::ToInt32(value));
		//pPropertyCommon->m_pItemShowSelectionMark->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemShowSelectionMark) );
	}
	ForceRenderScene(); 
}

int CDotNetProperty::IntensityAmbient::get()
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		return pPropertyCommon->m_intensityAmbient;
	}

	return 0;
}

void CDotNetProperty::IntensityAmbient::set(int _value)
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		//pPropertyCommon->m_pItemIntensityAmbient->SetNumber(_value);
		//pPropertyCommon->m_pItemIntensityAmbient->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemIntensityAmbient));
	}
	ForceRenderScene(); 
}

int CDotNetProperty::IntensityDiffuse::get()
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		return pPropertyCommon->m_intensiryDiffuse;
	}

	return 0;
}

void CDotNetProperty::IntensityDiffuse::set(int _value)
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		//pPropertyCommon->m_pItemIntensityDiffuse->SetNumber(_value);
		//pPropertyCommon->m_pItemIntensityDiffuse->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemIntensityDiffuse));
	}
	ForceRenderScene(); 
}

int CDotNetProperty::IntensitySpecular::get()
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		return pPropertyCommon->m_intensitySpecular;
	}

	return 0;
}

void CDotNetProperty::IntensitySpecular::set(int _value)
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		//pPropertyCommon->m_pItemIntensitySpecular->SetNumber(_value);
		//pPropertyCommon->m_pItemIntensitySpecular->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemIntensitySpecular));
	}
	ForceRenderScene(); 
}


IPropertyWireframe::IDisplayMode CDotNetPropertyWireframe::DisplayMode::get()
{
	CPropertyWireframe * pProperty = m_pSelectionDisplay->GetPropertyWireframe();
	if ( pProperty )
	{
		return static_cast<IPropertyWireframe::IDisplayMode>(pProperty->m_enumDisplayMethod);
	}

	return IPropertyWireframe::IDisplayMode::All;
}

void CDotNetPropertyWireframe::DisplayMode::set(IPropertyWireframe::IDisplayMode _value)
{
	CPropertyWireframe * pProperty= m_pSelectionDisplay->GetPropertyWireframe();
	if ( pProperty)
	{
		//pProperty->m_penumDisplayMethod->SetEnum( Convert::ToInt32(_value) );
		//pProperty->m_penumDisplayMethod->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_penumDisplayMethod) );
	}
	ForceRenderScene();
}

int CDotNetPropertyWireframe::LineWidth::get()
{
	CPropertyWireframe * pProperty = m_pSelectionDisplay->GetPropertyWireframe();
	if ( pProperty )
	{
		return pProperty->m_lineWidth;
	}

	return 1;
}

void CDotNetPropertyWireframe::LineWidth::set(int _value)
{
	CPropertyWireframe * pProperty= m_pSelectionDisplay->GetPropertyWireframe();
	if ( pProperty)
	{
		//pProperty->m_pItemLineWidth->SetNumber(_value);
		//pProperty->m_pItemLineWidth->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemLineWidth) );
	}
	ForceRenderScene();
}

//
//
//
int CDotNetPropertyStick::SphereResolution::get()
{
	CPropertyStick *	pProperty = m_pSelectionDisplay->GetPropertyStick();
	if ( pProperty )
	{
		return pProperty->m_sphereResolution;
	}

	return 0;
}

void CDotNetPropertyStick::SphereResolution::set(int _value)
{
	CPropertyStick * pProperty= m_pSelectionDisplay->GetPropertyStick();
	if ( pProperty )
	{
		//pProperty->m_pItemSphereResolution->SetNumber ( Convert::ToInt32(_value) );
		//pProperty->m_pItemSphereResolution->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemSphereResolution) );
	}
	ForceRenderScene();
}

int CDotNetPropertyStick::CylinderResolution::get()
{
	CPropertyStick *	pProperty = m_pSelectionDisplay->GetPropertyStick();
	if ( pProperty )
	{
		return pProperty->m_cylinderResolution;
	}

	return 0;
}

void CDotNetPropertyStick::CylinderResolution::set(int _value)
{
	CPropertyStick * pProperty= m_pSelectionDisplay->GetPropertyStick();
	if ( pProperty )
	{
		//pProperty->m_pItemCylinderResolution->SetNumber ( Convert::ToInt32(_value) );
		//pProperty->m_pItemCylinderResolution->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemCylinderResolution) );
	}
	ForceRenderScene();
}

double CDotNetPropertyStick::StickSize::get()
{
	CPropertyStick *	pProperty = m_pSelectionDisplay->GetPropertyStick();
	if ( pProperty )
	{
		return pProperty->m_stickSize;
	}

	return 0.0;
}

void CDotNetPropertyStick::StickSize::set(double _value)
{
	CPropertyStick * pProperty= m_pSelectionDisplay->GetPropertyStick();
	if ( pProperty )
	{
		//pProperty->m_pItemStickSize->SetDouble ( _value );
		//pProperty->m_pItemStickSize->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemStickSize) );
	}
	ForceRenderScene();
}

//    
//    
int CDotNetPropertySpaceFill::SphereResolution::get()
{
	CPropertySpaceFill *	pProperty = m_pSelectionDisplay->GetPropertySpaceFill();
	if ( pProperty )
	{
		return pProperty->m_sphereResolution;
	}

	return 0;
}

void CDotNetPropertySpaceFill::SphereResolution::set(int _value)
{
	CPropertySpaceFill * pProperty= m_pSelectionDisplay->GetPropertySpaceFill();
	if ( pProperty )
	{
		//pProperty->m_pItemSphereResolution->SetNumber( _value );
		//pProperty->m_pItemSphereResolution->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemSphereResolution) );
	}
	ForceRenderScene();
}

//
//
int CDotNetPropertyBallnStick::SphereResolution::get()
{
	CPropertyBallStick *	pProperty = m_pSelectionDisplay->GetPropertyBallStick();
	if ( pProperty )
	{
		return pProperty->m_sphereResolution;
	}
	return 0;
}

void CDotNetPropertyBallnStick::SphereResolution::set(int _value)
{
	CPropertyBallStick *	pProperty = m_pSelectionDisplay->GetPropertyBallStick();
	if ( pProperty )
	{
		//pProperty->m_pItemSphereResolution->SetNumber( _value );
		//pProperty->m_pItemSphereResolution->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemSphereResolution) );
	}
	ForceRenderScene();
}

int CDotNetPropertyBallnStick::CylinderResolution::get()
{
	CPropertyBallStick *	pProperty = m_pSelectionDisplay->GetPropertyBallStick();
	if ( pProperty )
	{
		return pProperty->m_cylinderResolution;
	}

	return 0;
}

void CDotNetPropertyBallnStick::CylinderResolution::set(int _value)
{
	CPropertyBallStick *	pProperty = m_pSelectionDisplay->GetPropertyBallStick();
	if ( pProperty )
	{
		//pProperty->m_pItemCylinderResolution->SetNumber( _value );
		//pProperty->m_pItemCylinderResolution->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemCylinderResolution) );
	}
	ForceRenderScene();
}

double CDotNetPropertyBallnStick::SphereRadius::get()
{
	CPropertyBallStick *	pProperty = m_pSelectionDisplay->GetPropertyBallStick();
	if ( pProperty )
	{
		return pProperty->m_sphereRadius;
	}

	return 0.0;
}

void CDotNetPropertyBallnStick::SphereRadius::set(double _value)
{
	CPropertyBallStick *	pProperty = m_pSelectionDisplay->GetPropertyBallStick();
	if ( pProperty )
	{
		//pProperty->m_pItemSphereRadius->SetDouble( _value );
		//pProperty->m_pItemSphereRadius->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemSphereRadius) );
	}
	ForceRenderScene();
}

double CDotNetPropertyBallnStick::CylinderSize::get()
{
	CPropertyBallStick *	pProperty = m_pSelectionDisplay->GetPropertyBallStick();
	if ( pProperty )
	{
		return pProperty->m_cylinderSize;
	}

	return 0.0;
}

void CDotNetPropertyBallnStick::CylinderSize::set(double _value)
{
	CPropertyBallStick *	pProperty = m_pSelectionDisplay->GetPropertyBallStick();
	if ( pProperty )
	{
		//pProperty->m_pItemCylinderSize->SetDouble( _value );
		//pProperty->m_pItemCylinderSize->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pItemCylinderSize) );
	}
	ForceRenderScene();
}

//
//
//
bool CDotNetPropertyHelix::Show::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return Convert::ToBoolean(pProperty->m_bDisplayHelix);
	}

	return true;
}

void CDotNetPropertyHelix::Show::set(bool _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pDisplayHelix->SetBool ( _value );
		//pProperty->m_pDisplayHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pDisplayHelix) );
	}
	ForceRenderScene();
}

bool CDotNetPropertyHelix::ShowTexture::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return Convert::ToBoolean(pProperty->m_bTextureHelix);
	}

	return true;
}

void CDotNetPropertyHelix::ShowTexture::set(bool _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pTextureHelix->SetBool ( _value );
		//pProperty->m_pTextureHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pTextureHelix) );
	}
	ForceRenderScene();
}

String ^ CDotNetPropertyHelix::TextureFilename::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return gcnew String (pProperty->m_strTextureFilenameHelix);
	}

	return nullptr;
}

void CDotNetPropertyHelix::TextureFilename::set(String ^ _value)
{	
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pTextureFilenameHelix->SetValue( _value );
		//pProperty->m_pTextureFilenameHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pTextureFilenameHelix) );
	}
	ForceRenderScene();
}

int CDotNetPropertyHelix::TextureCoordU::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return pProperty->m_textureCoordUHelix;
	}

	return 0;
}

void CDotNetPropertyHelix::TextureCoordU::set(int _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pTextureCoordUHelix->SetNumber ( _value );
		//pProperty->m_pTextureCoordUHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pTextureCoordUHelix) );
	}
	ForceRenderScene();
}

int CDotNetPropertyHelix::TextureCoordV::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return pProperty->m_textureCoordVHelix;
	}

	return 0;
}

void CDotNetPropertyHelix::TextureCoordV::set(int _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pTextureCoordVHelix->SetNumber ( _value );
		//pProperty->m_pTextureCoordVHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pTextureCoordVHelix) );
	}
	ForceRenderScene();
}

System::Drawing::Color CDotNetPropertyHelix::Color::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return System::Drawing::Color::FromArgb(pProperty->m_colorHelix);
	}

	return System::Drawing::Color::White;
}

void CDotNetPropertyHelix::Color::set(System::Drawing::Color _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{

		//pProperty->m_pcolorHelix->SetColor ( ManagedColor2COLORREF(_value) );
		//pProperty->m_pcolorHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pcolorHelix) );
	}
	ForceRenderScene(); 
}

System::Drawing::Size CDotNetPropertyHelix::Size::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return System::Drawing::Size(pProperty->m_sizeHelix.cx, pProperty->m_sizeHelix.cy);
	}

	return System::Drawing::Size(0,0);
}

void CDotNetPropertyHelix::Size::set(System::Drawing::Size _value )
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_psizeHelix->SetSize ( CSize(_value.Width, _value.Height) );
		//pProperty->m_psizeHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_psizeHelix) );
	}
	ForceRenderScene(); 
}

IPropertyHelix::IFitting CDotNetPropertyHelix::Fitting::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return static_cast<IPropertyHelix::IFitting>(pProperty->m_fittingMethodHelix);
	}

	return IPropertyHelix::IFitting::Optimal;
}

void CDotNetPropertyHelix::Fitting::set(IPropertyHelix::IFitting _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_penumFittingMethodHelix->SetEnum ( Convert::ToUInt32(_value) );
		//pProperty->m_penumFittingMethodHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_penumFittingMethodHelix) );
	}
	ForceRenderScene(); 
}

IPropertyHelix::IShape CDotNetPropertyHelix::Shape::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return static_cast<IPropertyHelix::IShape>(pProperty->m_shapeHelix);
	}

	return IPropertyHelix::IShape::_30Poly;
}

void CDotNetPropertyHelix::Shape::set(IPropertyHelix::IShape _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_penumShapeHelix->SetEnum ( Convert::ToInt32(_value) );
		//pProperty->m_penumShapeHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_penumShapeHelix) );
	}
	ForceRenderScene(); 
}

bool CDotNetPropertyHelix::ShowCoilOnHelix::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return Convert::ToBoolean(pProperty->m_bShowCoilOnHelix);
	}

	return true;
}

void CDotNetPropertyHelix::ShowCoilOnHelix::set(bool _value )
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pShowCoilOnHelix->SetBool ( _value );
		//pProperty->m_pShowCoilOnHelix->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pShowCoilOnHelix) );
	}
	ForceRenderScene(); 
}

//
//
//
bool CDotNetPropertySheet::Show::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return Convert::ToBoolean(pProperty->m_bDisplaySheet);
	}

	return true;
}

void CDotNetPropertySheet::Show::set(bool _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pDisplaySheet->SetBool ( _value );
		//pProperty->m_pDisplaySheet->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pDisplaySheet) );
	}
	ForceRenderScene(); 
}

bool CDotNetPropertySheet::ShowTexture::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return Convert::ToBoolean(pProperty->m_bTextureSheet);
	}

	return true;
}

void CDotNetPropertySheet::ShowTexture::set(bool _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pTextureSheet->SetBool ( _value );
		//pProperty->m_pTextureSheet->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pTextureSheet) );
	}
	ForceRenderScene(); 
}

String ^ CDotNetPropertySheet::TextureFilename::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return gcnew String (pProperty->m_strTextureFilenameSheet);
	}

	return nullptr;
}

void CDotNetPropertySheet::TextureFilename::set(String ^ _value)
{	
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pTextureFilenameSheet->SetValue( _value );
		//pProperty->m_pTextureFilenameSheet->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pTextureFilenameSheet) );
	}
	ForceRenderScene(); 
}

int CDotNetPropertySheet::TextureCoordU::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return pProperty->m_textureCoordUSheet;
	}

	return 0;
}

void CDotNetPropertySheet::TextureCoordU::set(int _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pTextureCoordUSheet->SetNumber ( _value );
		//pProperty->m_pTextureCoordUSheet->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pTextureCoordUSheet) );
	}
	ForceRenderScene(); 
}

int CDotNetPropertySheet::TextureCoordV::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return pProperty->m_textureCoordVSheet;
	}

	return 0;
}

void CDotNetPropertySheet::TextureCoordV::set(int _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pTextureCoordVSheet->SetNumber ( _value );
		//pProperty->m_pTextureCoordVSheet->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pTextureCoordVSheet) );
	}
	ForceRenderScene(); 
}

System::Drawing::Color CDotNetPropertySheet::Color::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return System::Drawing::Color::FromArgb(pProperty->m_colorSheet);
	}

	return System::Drawing::Color::White;
}

void CDotNetPropertySheet::Color::set(System::Drawing::Color _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pcolorSheet->SetColor ( ManagedColor2COLORREF(_value) );
		//pProperty->m_pcolorSheet->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pcolorSheet) );
	}
	ForceRenderScene(); 
}

System::Drawing::Size CDotNetPropertySheet::Size::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return System::Drawing::Size(pProperty->m_sizeSheet.cx, pProperty->m_sizeSheet.cy);
	}

	return System::Drawing::Size(0,0);
}

void CDotNetPropertySheet::Size::set(System::Drawing::Size _value )
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_psizeSheet->SetSize ( CSize(_value.Width, _value.Height) );
		//pProperty->m_psizeSheet->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_psizeSheet) );
	}
	ForceRenderScene(); 
}

IPropertySheet::IShape CDotNetPropertySheet::Shape::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return static_cast<IPropertySheet::IShape>(pProperty->m_shapeSheet);
	}

	return IPropertySheet::IShape::_30Poly;
}

void CDotNetPropertySheet::Shape::set(IPropertySheet::IShape _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_penumShapeSheet->SetEnum ( Convert::ToInt32(_value) );
		//pProperty->m_penumShapeSheet->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_penumShapeSheet) );
	}
	ForceRenderScene(); 
}

bool CDotNetPropertySheet::ShowCoilOnSheet::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return Convert::ToBoolean(pProperty->m_bShowCoilOnSheet);
	}

	return true;
}

void CDotNetPropertySheet::ShowCoilOnSheet::set(bool _value )
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pShowCoilOnSheet->SetBool ( _value );
		//pProperty->m_pShowCoilOnSheet->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pShowCoilOnSheet) );
	}
	ForceRenderScene(); 
}

//
//
bool CDotNetPropertyCoil::Show::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return Convert::ToBoolean(pProperty->m_bDisplayCoil);
	}

	return true;
}

void CDotNetPropertyCoil::Show::set(bool _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pDisplayCoil->SetBool ( _value );
		//pProperty->m_pDisplayCoil->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pDisplayCoil) );
	}
	ForceRenderScene(); 
}

bool CDotNetPropertyCoil::ShowTexture::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return Convert::ToBoolean(pProperty->m_bTextureCoil);
	}

	return true;
}

void CDotNetPropertyCoil::ShowTexture::set(bool _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pTextureCoil->SetBool ( _value );
		//pProperty->m_pTextureCoil->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pTextureCoil) );
	}
	ForceRenderScene(); 
}

String ^ CDotNetPropertyCoil::TextureFilename::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return gcnew String (pProperty->m_strTextureFilenameCoil);
	}

	return nullptr;
}

void CDotNetPropertyCoil::TextureFilename::set(String ^ _value)
{	
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pTextureFilenameCoil->SetValue( _value );
		//pProperty->m_pTextureFilenameCoil->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pTextureFilenameCoil) );
	}
	ForceRenderScene(); 
}

int CDotNetPropertyCoil::TextureCoordU::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return pProperty->m_textureCoordUCoil;
	}

	return 0;
}

void CDotNetPropertyCoil::TextureCoordU::set(int _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pTextureCoordUCoil->SetNumber ( _value );
		//pProperty->m_pTextureCoordUCoil->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pTextureCoordUCoil) );
	}
	ForceRenderScene(); 
}

int CDotNetPropertyCoil::TextureCoordV::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return pProperty->m_textureCoordVCoil;
	}

	return 0;
}

void CDotNetPropertyCoil::TextureCoordV::set(int _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pTextureCoordVCoil->SetNumber ( _value );
		//pProperty->m_pTextureCoordVCoil->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pTextureCoordVCoil) );
	}
	ForceRenderScene(); 
}

System::Drawing::Color CDotNetPropertyCoil::Color::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return System::Drawing::Color::FromArgb(pProperty->m_colorCoil);
	}

	return System::Drawing::Color::White;
}

void CDotNetPropertyCoil::Color::set(System::Drawing::Color _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pcolorCoil->SetColor ( ManagedColor2COLORREF(_value) );
		//pProperty->m_pcolorCoil->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pcolorCoil) );
	}
	ForceRenderScene(); 
}

System::Drawing::Size CDotNetPropertyCoil::Size::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return System::Drawing::Size(pProperty->m_sizeCoil.cx, pProperty->m_sizeCoil.cy);
	}

	return System::Drawing::Size(0,0);
}

void CDotNetPropertyCoil::Size::set(System::Drawing::Size _value )
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_psizeCoil->SetSize ( CSize(_value.Width, _value.Height) );
		//pProperty->m_psizeCoil->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_psizeCoil) );
	}
	ForceRenderScene(); 
}

IPropertyCoil::IShape CDotNetPropertyCoil::Shape::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return static_cast<IPropertyCoil::IShape>(pProperty->m_shapeCoil);
	}

	return IPropertyCoil::IShape::_30Poly;
}

void CDotNetPropertyCoil::Shape::set(IPropertyCoil::IShape _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_penumShapeCoil->SetEnum ( Convert::ToInt32(_value) );
		//pProperty->m_penumShapeCoil->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_penumShapeCoil) );
	}
	ForceRenderScene(); 
}

//
//

void CDotNetPropertyRibbon::Init()
{
	m_propertyHelix = gcnew CDotNetPropertyHelix(m_pSelectionDisplay);
	m_propertyHelix->Init();

	m_propertySheet = gcnew CDotNetPropertySheet(m_pSelectionDisplay);
	m_propertySheet->Init();

	m_propertyCoil = gcnew CDotNetPropertyCoil(m_pSelectionDisplay);
	m_propertyCoil->Init();
}

int CDotNetPropertyRibbon::CurveTension::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return pProperty->m_curveTension;
	}

	return 0;
}
void CDotNetPropertyRibbon::CurveTension::set(int _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pCurveTension->SetNumber(_value);
		//pProperty->m_pCurveTension->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pCurveTension) );
	}
	ForceRenderScene(); 
}

int CDotNetPropertyRibbon::CurveResolution::get()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		return pProperty->m_resolution;
	}

	return 0;
}
void CDotNetPropertyRibbon::CurveResolution::set(int _value)
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pResolution->SetNumber(_value);
		//pProperty->m_pResolution->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pResolution) );
	}
	ForceRenderScene(); 
}

void CDotNetPropertyRibbon::SelectSugarInDNA()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pDNASelectSugar->OnClick();
	}
	ForceRenderScene(); 

}

void CDotNetPropertyRibbon::SelectBackBoneInDNA()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pDNASelectBase->OnClick();
	}
	ForceRenderScene(); 

}

void CDotNetPropertyRibbon::SelectInnerAtomsInDNA()
{
	CPropertyRibbon *	pProperty = m_pSelectionDisplay->GetPropertyRibbon();
	if ( pProperty )
	{
		//pProperty->m_pDNASelectInnerAtom->OnClick();
	}
	ForceRenderScene(); 
}

//
IPropertySurface::IDisplayMethod CDotNetPropertySurface::DisplayMethod::get()
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		return static_cast<IPropertySurface::IDisplayMethod>(pProperty->m_enumSurfaceDisplayMethod);
	}

	return IPropertySurface::IDisplayMethod::Solid;
}

void CDotNetPropertySurface::DisplayMethod::set(IPropertySurface::IDisplayMethod _value)
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		//pProperty->m_pSurfaceDisplayMethod->SetEnum(Convert::ToInt32(_value));
		//pProperty->m_pSurfaceDisplayMethod->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pSurfaceDisplayMethod) );
	}
	ForceRenderScene(); 
}

int CDotNetPropertySurface::Transparency::get()
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		return pProperty->m_transparency;
	}

	return 0;
}

void CDotNetPropertySurface::Transparency::set(int _value)
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		//pProperty->m_ptransparency->SetNumber(_value);
		//pProperty->m_ptransparency->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_ptransparency) );
	}
	ForceRenderScene(); 
}

double CDotNetPropertySurface::ProbeSphereRadius::get()
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		return pProperty->m_probeSphere;
	}

	return 0.0;
}

void CDotNetPropertySurface::ProbeSphereRadius::set(double _value)
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		//pProperty->m_pProbeSphere->SetDouble(_value);
		//pProperty->m_pProbeSphere->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pProbeSphere) );
	}
	ForceRenderScene(); 
}

IPropertySurface::IAlgorithm CDotNetPropertySurface::Algorithm::get()
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		return static_cast<IPropertySurface::IAlgorithm> (pProperty->m_surfaceGenMethod);
	}
	return IPropertySurface::IAlgorithm::MQ;
}

void CDotNetPropertySurface::Algorithm::set(IPropertySurface::IAlgorithm algorithm)
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		//pProperty->m_pSurfaceGenMethod->SetEnum(Convert::ToInt32(algorithm));
		//pProperty->m_pSurfaceGenMethod->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pSurfaceGenMethod) );
	}
	ForceRenderScene(); 
}

//
IPropertySurface::IQuality CDotNetPropertySurface::GeometryQuality::get()
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		return static_cast<IPropertySurface::IQuality>(pProperty->m_surfaceQuality);
	}

	return IPropertySurface::IQuality::_5;
}

void CDotNetPropertySurface::GeometryQuality::set(IPropertySurface::IQuality _value)
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		//pProperty->m_pSurfaceQuality->SetEnum(Convert::ToInt32(_value));
		//pProperty->m_pSurfaceQuality->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pSurfaceQuality) );
	}
	ForceRenderScene(); 
}

bool CDotNetPropertySurface::AddHETATM::get()
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		return Convert::ToBoolean(pProperty->m_bAddHETATM);
	}
	return true;

}

void CDotNetPropertySurface::AddHETATM::set(bool _value)
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		//pProperty->m_pAddHETATM->SetBool(Convert::ToInt32(_value));
		//pProperty->m_pAddHETATM->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pAddHETATM) );
	}
	ForceRenderScene();
}

bool CDotNetPropertySurface::ShowCurvature::get()
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		return Convert::ToBoolean(pProperty->m_bDisplayCurvature);
	}
	return true;

}

void CDotNetPropertySurface::ShowCurvature::set(bool _value)
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		//pProperty->m_pDisplayCurvate->SetBool(Convert::ToInt32(_value));
		//pProperty->m_pDisplayCurvate->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pDisplayCurvate) );
	}
	ForceRenderScene();
}

IPropertySurface::ICurvatureRingSize CDotNetPropertySurface::CurvatureRingSize::get()
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		return static_cast<IPropertySurface::ICurvatureRingSize>(pProperty->m_curvatureRingSize);
	}

	return IPropertySurface::ICurvatureRingSize::_1;
}

void CDotNetPropertySurface::CurvatureRingSize::set(IPropertySurface::ICurvatureRingSize _value)
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		//pProperty->m_pCurvatureRingSize->SetEnum(Convert::ToInt32(_value));
		//pProperty->m_pCurvatureRingSize->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pCurvatureRingSize) );
	}
	ForceRenderScene(); 
}

IPropertySurface::IColorSmoothing CDotNetPropertySurface::ColorSmoothing::get()
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		return static_cast<IPropertySurface::IColorSmoothing>(pProperty->m_iSurfaceBlurring);
	}

	return	IPropertySurface::IColorSmoothing::_1;
}

void CDotNetPropertySurface::ColorSmoothing::set(IPropertySurface::IColorSmoothing _value )
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		//pProperty->m_pSurfaceBlurring->SetEnum(Convert::ToInt32(_value));
		//pProperty->m_pSurfaceBlurring->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pSurfaceBlurring) );
	}
	ForceRenderScene(); 
}

bool CDotNetPropertySurface::DepthSort::get()
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		return Convert::ToBoolean(pProperty->m_bSurfaceDepthSort);
	}

	return true;
}

void CDotNetPropertySurface::DepthSort::set(bool _value)
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		//pProperty->m_pSurfaceDepthSort->SetBool(Convert::ToInt32(_value));
		//pProperty->m_pSurfaceDepthSort->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pSurfaceDepthSort) );
	}
	ForceRenderScene(); 
}

bool CDotNetPropertySurface::UseInnerFaceColor::get()
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		return Convert::ToBoolean(pProperty->m_useInnerFaceColor);
	}
	return true;
}

void CDotNetPropertySurface::UseInnerFaceColor::set(bool _value)
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		//pProperty->m_pUseInnerFaceColor->SetBool(Convert::ToInt32(_value));
		//pProperty->m_pUseInnerFaceColor->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pUseInnerFaceColor) );
	}
	ForceRenderScene(); 
}

int CDotNetPropertySurface::InnerFaceColorBlend::get()
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		return pProperty->m_blendFactor;
	}

	return 0;
}

void CDotNetPropertySurface::InnerFaceColorBlend::set(int _value)
{

	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		//pProperty->m_pBlendInnerFace->SetNumber(_value);
		//pProperty->m_pBlendInnerFace->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pBlendInnerFace) );
	}
	ForceRenderScene(); 
}

System::Drawing::Color CDotNetPropertySurface::InnerFaceColor::get()
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		return Color::FromArgb(pProperty->m_colorInnerFace);
	}

	return System::Drawing::Color::White;
}

void CDotNetPropertySurface::InnerFaceColor::set(System::Drawing::Color _value)
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		//pProperty->m_pColorInnerFace->SetColor( ManagedColor2COLORREF(_value) );
		//pProperty->m_pColorInnerFace->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProperty->m_pColorInnerFace) );
	}
	ForceRenderScene(); 
}


void CDotNetPropertySurface::SelectSurfaceAtoms()
{
	CPropertySurface *	pProperty = m_pSelectionDisplay->GetPropertySurface();
	if ( pProperty )
	{
		//pProperty->m_pSelectSurfaceAtom->OnClick();		//	SetBool(TRUE);
	}
	ForceRenderScene(); 
}
//////////////////////////////////////////////////////////////////////////
//	정상적인 atom 일 경우에..
CDotNetAtom::CDotNetAtom( CAtomInst * pAtomInst , CDotNetResidue ^ pResidue ) 
{
	m_pAtomInst = pAtomInst; 
	m_pResidue = pResidue;

	if ( m_pResidue != nullptr )
	{
		m_pChain = safe_cast<CDotNetChain ^>(pResidue->ParentChain);
		m_pPDB = safe_cast<CDotNetPDB ^>(pResidue->ParentPDB);
	}
	else
	{
		m_pChain = nullptr;
		m_pPDB = nullptr;
	}
}

//	HETATOM 일 경우에 Residue에 속하지 않는다.
CDotNetAtom::CDotNetAtom( CAtomInst * pAtomInst , CDotNetChain ^ pChain ) 
{ 
	m_pAtomInst = pAtomInst; 
	m_pResidue = nullptr;
	m_pChain = pChain;
	m_pPDB = safe_cast<CDotNetPDB ^>(pChain->ParentPDB);
}

void CDotNetAtom::Init()
{

}

String ^ CDotNetAtom::Name::get()
{ 
	CString strAtomName = m_pAtomInst->GetAtom()->m_atomName;
	strAtomName.TrimLeft(" \t");
	strAtomName.TrimRight(" \t");
	return gcnew String(strAtomName); 
}

Vector3	CDotNetAtom::PositionTransformed::get()
{ 
	D3DXMATRIXA16 * pMatWorld = m_pAtomInst->m_pPDBInst->m_pPDBRenderer->GetWorldMatrix();

	D3DXVECTOR3 posTransformed;
	D3DXVec3TransformCoord( &posTransformed, &(m_pAtomInst->GetAtom()->m_pos), pMatWorld );

	return Vector3(posTransformed.x, posTransformed.y, posTransformed.z );
}

void	CDotNetAtom::SetSelect(bool select, bool bNeedUpdate)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		m_pAtomInst->SetSelectChild(select); 
		if ( bNeedUpdate == true )
			pProteinVistaRenderer->SelectedAtomApply();
	}
}

bool CDotNetAtom::Select::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer ){ pProteinVistaRenderer->SelectedAtomApply(); }

	return Convert::ToBoolean(m_pAtomInst->m_bSelect);
}

void CDotNetAtom::Select::set(bool bValue)
{
	SetSelect(bValue, true);
}

//
//
CDotNetResidue::CDotNetResidue(CResidueInst * pResidue, CDotNetChain ^ pChain) 
{
	m_pResidueInst = pResidue; 
	m_pChain = pChain;
	m_pPDB = safe_cast<CDotNetPDB ^>(pChain->ParentPDB);
}

void CDotNetResidue::Init()
{
	m_arrayAtomSpecial = gcnew array<CDotNetAtom ^> (6);
	m_arrayAtoms = gcnew List <IAtom ^>(m_pResidueInst->m_arrayAtomInst.size());

	for ( int i = 0 ; i < m_pResidueInst->m_arrayAtomInst.size() ; i++ )
	{
		CAtomInst * pAtomInst = m_pResidueInst->m_arrayAtomInst[i];
		CDotNetAtom ^ pIAtom = gcnew CDotNetAtom(pAtomInst, this);
		m_arrayAtoms->Add(pIAtom);
		pIAtom->Init();

		if ( pAtomInst->GetAtom()->m_typeAtom == MAINCHAIN_N )
			m_arrayAtomSpecial[MAINCHAIN_N] = pIAtom;
		else if ( pAtomInst->GetAtom()->m_typeAtom == MAINCHAIN_CA )
			m_arrayAtomSpecial[MAINCHAIN_CA] = pIAtom;
		else if ( pAtomInst->GetAtom()->m_typeAtom == RESIDUE_CB )
			m_arrayAtomSpecial[RESIDUE_CB] = pIAtom;
		else if ( pAtomInst->GetAtom()->m_typeAtom == MAINCHAIN_C )
			m_arrayAtomSpecial[MAINCHAIN_C] = pIAtom;
		else if ( pAtomInst->GetAtom()->m_typeAtom == MAINCHAIN_O )
			m_arrayAtomSpecial[MAINCHAIN_O] = pIAtom;
	}
}

void CDotNetResidue::SetSelect(bool select, bool bNeedUpdate) 
{ 
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		m_pResidueInst->SetSelectChild(select); 
		if (bNeedUpdate == true)
			pProteinVistaRenderer->SelectedAtomApply();
	}
}

bool CDotNetResidue::Select::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer ){ pProteinVistaRenderer->SelectedAtomApply(); }

	return Convert::ToBoolean(m_pResidueInst->m_bSelect);
}

void CDotNetResidue::Select::set(bool bValue)
{
	SetSelect(bValue, true);
}

//
//
CDotNetChain::CDotNetChain( CChainInst * pChain , CDotNetModel ^ pModel ) 
{
	m_pChainInst = pChain; 
	m_pModel = pModel;
	m_pPDB = safe_cast<CDotNetPDB ^>(pModel->ParentPDB);
}

void CDotNetChain::Init()
{
	m_arrayResidues = gcnew List<IResidue ^>(m_pChainInst->m_arrayResidueInst.size());
	for ( int i = 0 ; i < m_pChainInst->m_arrayResidueInst.size() ; i++ )
	{
		CResidueInst * pResidueInst = m_pChainInst->m_arrayResidueInst[i];
		CDotNetResidue ^ dotnetResidue = gcnew CDotNetResidue(pResidueInst, this);
		m_arrayResidues->Add(dotnetResidue);
		dotnetResidue->Init();
	}

	//	arrayAtom을 설정
	m_arrayAtoms = gcnew List<IAtom ^>(m_arrayResidues->Count * 15);
	for each ( CDotNetResidue ^ residue in m_arrayResidues )
	{
		for each ( CDotNetAtom ^ atom in residue->Atoms )
		{
			m_arrayAtoms->Add(atom);
		}
	}

	//	HetAtom을 설정
	m_arrayHETAtoms = gcnew List<IAtom ^>(m_pChainInst->m_arrayHETATMInst.size());
	for ( int i = 0 ; i < m_pChainInst->m_arrayHETATMInst.size() ; i++ )
	{
		CDotNetAtom ^ dotnetAtom = gcnew CDotNetAtom(m_pChainInst->m_arrayHETATMInst[i] , this);
		m_arrayHETAtoms->Add(dotnetAtom);
		dotnetAtom->Init();
	}
}

void CDotNetChain::SetSelect(bool select, bool bNeedUpdate)
{ 
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		m_pChainInst->SetSelectChild(select); 
		if (bNeedUpdate == true)
			pProteinVistaRenderer->SelectedAtomApply();
	}
}

bool CDotNetChain::Select::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer ){ pProteinVistaRenderer->SelectedAtomApply(); }

	return Convert::ToBoolean(m_pChainInst->m_bSelect);
}

void CDotNetChain::Select::set(bool bValue)
{
	SetSelect(bValue, true);
}

List<IResidue ^ > ^ CDotNetChain::GetResidues(String ^ residueName) 
{
	List<IResidue ^ > ^ residueList = gcnew List<IResidue ^ >;

	for each ( IResidue ^ residue in m_arrayResidues )
	{
		CDotNetResidue ^ dotnetResidue = safe_cast<CDotNetResidue ^>(residue);
		if ( dotnetResidue->Name == residueName )
		{
			residueList->Add(dotnetResidue);
		}
	}

	return residueList;
}

List<IAtom ^ > ^ CDotNetChain::GetAtoms(String ^ atomName) 
{ 
	List<IAtom ^ > ^ atomList = gcnew List<IAtom ^ >;

	for each ( IAtom ^ atom in m_arrayAtoms )
	{
		CDotNetAtom ^ dotnetAtom = safe_cast<CDotNetAtom ^>(atom);
		if ( dotnetAtom->Name == atomName )
		{
			atomList->Add(dotnetAtom);
		}
	}

	return atomList;
}

List<IResidue ^ > ^ CDotNetChain::GetSSResidues(IResidue::ISSType type)
{
	List<IResidue ^ > ^ residueList = gcnew List<IResidue ^ >;

	for each ( IResidue ^ residue in m_arrayResidues )
	{
		CDotNetResidue ^ dotnetResidue = safe_cast<CDotNetResidue ^>(residue);
		if ( (IResidue::ISSType)(dotnetResidue->SSType) == type )
		{
			residueList->Add(dotnetResidue);
		}
	}

	return residueList;
}

//
//
CDotNetModel::CDotNetModel(CModelInst * pModel , CDotNetPDB ^ pdb)
{
	m_pModelInst = pModel; 
	m_pPDB = pdb;
}

void CDotNetModel::Init()
{
	m_arrayListChains = gcnew List<IChain ^>(m_pModelInst->m_arrayChainInst.size());
	for ( int i = 0 ; i < m_pModelInst->m_arrayChainInst.size() ; i++ )
	{
		//    아래코드에서 dialog 나오는 에러 나옴. 
		CDotNetChain ^ pDotNetChain = gcnew CDotNetChain(m_pModelInst->m_arrayChainInst[i], this);
		m_arrayListChains->Add(pDotNetChain);
		pDotNetChain->Init();
	}
}

void CDotNetModel::SetSelect(bool select, bool bNeedUpdate)
{ 
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		m_pModelInst->SetSelectChild(select); 
		if (bNeedUpdate == true)
			pProteinVistaRenderer->SelectedAtomApply();
	}
}

bool CDotNetModel::Select::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer ){ pProteinVistaRenderer->SelectedAtomApply(); }

	return Convert::ToBoolean(m_pModelInst->m_bSelect);
}

void CDotNetModel::Select::set(bool bValue)
{
	SetSelect(bValue, true);
}

IChain ^ CDotNetModel::GetChain(String ^ chainID) 
{ 
	for each ( IChain ^ chain in m_arrayListChains )
	{
		CDotNetChain ^ dotnetChain = safe_cast<CDotNetChain ^>(chain);
		if ( dotnetChain->ID == chainID )
			return dotnetChain;
	}

	return nullptr;
}

//
//
CDotNetPDB::CDotNetPDB(CPDBInst * pPDB, CDotNetProteinInsight ^ proteinVista )
{
	m_pDotNetProteinVista = proteinVista;
	m_pPDBInst = pPDB;
}

void CDotNetPDB::Init()
{
	m_arrayListModels = gcnew List <IModel ^>(m_pPDBInst->m_arrayModelInst.size());
	for ( unsigned i = 0 ; i < m_pPDBInst->m_arrayModelInst.size() ; i++ )
	{
		CModelInst * pModel = m_pPDBInst->m_arrayModelInst[i];
		CDotNetModel ^ dotnetModel = gcnew CDotNetModel(pModel, this);
		m_arrayListModels->Add(dotnetModel);
		dotnetModel->Init();
	}

	//	모델이 1개일때는 chain을 넣고, model 이 여러개일때에는 첫번째 model의 chain을 넣는다.
	CDotNetModel ^ pDotNetModel = safe_cast<CDotNetModel ^ >(m_arrayListModels[0]);
	m_arrayListChains = gcnew List<IChain ^>(pDotNetModel->Chains->Count);
	for each ( CDotNetChain ^ chain in pDotNetModel->Chains )
	{
		m_arrayListChains->Add(chain);
	}
}


List <IChain ^>  ^ CDotNetPDB::GetChains(String ^ chainID) 
{
	List<IChain ^ > ^ chainList = gcnew List<IChain ^ >(m_arrayListModels->Count);

	for each ( IModel ^ model in m_arrayListModels )
	{
		CDotNetChain ^ chain = safe_cast<CDotNetChain ^>(model->GetChain(chainID));
		if ( chain != nullptr )
		{
			chainList->Add(chain);
		}
	}

	return chainList;
}

IModel ^	CDotNetPDB::GetModel(int modelNum) 
{
	for each ( IModel ^ model in m_arrayListModels )
	{
		CDotNetModel ^ dotnetModel = safe_cast<CDotNetModel ^>(model);
		if ( dotnetModel->Num == modelNum )
			return dotnetModel;
	}
	return nullptr; 
}

bool CDotNetPDB::Active::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		if ( m_pPDBInst == pProteinVistaRenderer->m_pActivePDBRenderer->GetPDBInst() )
			return true;
		else
			return false;
	}
	
	return true;
}

void CDotNetPDB::Active::set(bool value)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		if ( value == true )
		{	//	true 일때만 의미가 있다.
			if ( m_pPDBInst != pProteinVistaRenderer->m_pActivePDBRenderer->GetPDBInst() )
			{	//	현재 선택되어 있는것이 this 가 아니다.
				pProteinVistaRenderer->SetActivePDBRenderer(m_pPDBInst->m_pPDBRenderer);
			}
		}
	}
}

void CDotNetPDB::ShowBioUnit()
{
	Active = true;
    GetMainActiveView()->OnDisplayBioUnit();

	//GetMainActiveView()->SendMessage(WM_COMMAND, ID_DISPLAY_BIOUNIT ); 
	/*CView * pView = ;
	if ( pView )
		((CProteinVistaView*)pView)->SendMessage(WM_COMMAND, ID_DISPLAY_BIOUNIT );*/
}

bool	CDotNetPDB::IsBioUnit::get()
{
	if ( m_pPDBInst->GetPDB()->m_bioUnitMatrix.size() > 0 )
		return true;
	return false;
}

bool	CDotNetPDB::IsBioUnitChild::get()
{
	if ( m_pPDBInst->m_pPDBRenderer->m_pPDBRendererParentBioUnit != NULL )
		return true;

	return false;
}

bool	CDotNetPDB::IsBioUnitParent::get()
{
	if ( m_pPDBInst->GetPDB()->m_bioUnitMatrix.size() > 0 )
	{
		if ( m_pPDBInst->m_pPDBRenderer->m_pPDBRendererParentBioUnit == NULL )
		{
			return true;
		}
	}

	return false;
}

bool CDotNetPDB::AttatchBioUnit::get()
{
	Active = true;
	return Convert::ToBoolean(m_pPDBInst->m_pPDBRenderer->m_bAttatchBioUnit);
}

void CDotNetPDB::AttatchBioUnit::set(bool value)
{
	Active = true;

	if ( value != Convert::ToBoolean(m_pPDBInst->m_pPDBRenderer->m_bAttatchBioUnit) )
	{	//	같지 않으면 바꾼다.
		//CView * pView = GetMainActiveView();
		//if ( pView )
		//	((CProteinVistaView*)pView)->SendMessage(WM_COMMAND, ID_ATTATCH_BIOUNIT);
		 GetMainActiveView()->OnAttatchBiounit();
		// GetMainActiveView()->SendMessage(WM_COMMAND, ID_ATTATCH_BIOUNIT ); 
	}
}

void CDotNetPDB::MoveCenter() 
{
	Active = true;

	/*CView * pView = GetMainActiveView();
	if ( pView )
	((CProteinVistaView*)pView)->SendMessage(WM_COMMAND, ID_CENTER_MOLECULE );*/
     //GetMainActiveView()->SendMessage(WM_COMMAND, ID_CENTER_MOLECULE );
	 GetMainActiveView()->OnCenterMolecule();
	ForceRenderScene();
}

void CDotNetPDB::RotationAxis(Vector3 axis, float angle)
{
	Active = true;

	D3DXMATRIX matRot;
	D3DXVECTOR3 vecAxis(axis.X, axis.Y, axis.Z);
	D3DXMatrixRotationAxis(&matRot, &vecAxis, angle);

	m_pPDBInst->m_pPDBRenderer->m_matWorldUserInput = matRot;

	//	ForceRenderScene();
	m_pPDBInst->m_pPDBRenderer->UpdatePDBRendererCenter();
}

void CDotNetPDB::RotationX(float angle)
{
	Active = true;

	D3DXMATRIX matRot;
	D3DXMatrixRotationX(&matRot, angle);

	m_pPDBInst->m_pPDBRenderer->m_matWorldUserInput = matRot;

	//	ForceRenderScene();
	m_pPDBInst->m_pPDBRenderer->UpdatePDBRendererCenter();
}

void CDotNetPDB::RotationY(float angle)
{
	Active = true;

	D3DXMATRIX matRot;
	D3DXMatrixRotationY(&matRot, angle);

	m_pPDBInst->m_pPDBRenderer->m_matWorldUserInput = matRot;

	//	ForceRenderScene();
	m_pPDBInst->m_pPDBRenderer->UpdatePDBRendererCenter();
}

void CDotNetPDB::RotationZ(float angle)
{
	Active = true;

	D3DXMATRIX matRot;
	D3DXMatrixRotationZ(&matRot, angle);

	m_pPDBInst->m_pPDBRenderer->m_matWorldUserInput = matRot;

	//	ForceRenderScene();
	m_pPDBInst->m_pPDBRenderer->UpdatePDBRendererCenter();
}

void CDotNetPDB::Move(float x, float y, float z)
{
	Active = true;

	D3DXMATRIX matTrans;
	D3DXMatrixTranslation(&matTrans, x, y, z);

	m_pPDBInst->m_pPDBRenderer->m_matWorldUserInput = matTrans;

	ForceRenderScene();
	m_pPDBInst->m_pPDBRenderer->UpdatePDBRendererCenter();
}

Matrix CDotNetPDB::TransformLocal::get() 
{ 
	Active = true;

	Microsoft::DirectX::Matrix mat;
	mat.M11 = m_pPDBInst->m_pPDBRenderer->m_matWorld._11;
	mat.M12 = m_pPDBInst->m_pPDBRenderer->m_matWorld._12;
	mat.M13 = m_pPDBInst->m_pPDBRenderer->m_matWorld._13;
	mat.M14 = m_pPDBInst->m_pPDBRenderer->m_matWorld._14;
	mat.M21 = m_pPDBInst->m_pPDBRenderer->m_matWorld._21;
	mat.M22 = m_pPDBInst->m_pPDBRenderer->m_matWorld._22;
	mat.M23 = m_pPDBInst->m_pPDBRenderer->m_matWorld._23;
	mat.M24 = m_pPDBInst->m_pPDBRenderer->m_matWorld._24;
	mat.M31 = m_pPDBInst->m_pPDBRenderer->m_matWorld._31;
	mat.M32 = m_pPDBInst->m_pPDBRenderer->m_matWorld._32;
	mat.M33 = m_pPDBInst->m_pPDBRenderer->m_matWorld._33;
	mat.M34 = m_pPDBInst->m_pPDBRenderer->m_matWorld._34;
	mat.M41 = m_pPDBInst->m_pPDBRenderer->m_matWorld._41;
	mat.M42 = m_pPDBInst->m_pPDBRenderer->m_matWorld._42;
	mat.M43 = m_pPDBInst->m_pPDBRenderer->m_matWorld._43;
	mat.M44 = m_pPDBInst->m_pPDBRenderer->m_matWorld._44;

	return mat;
}

void CDotNetPDB::TransformLocal::set(Matrix mat)
{
	Active = true;

	D3DXMATRIX	matTransform;
	matTransform._11 = mat.M11;  
	matTransform._12 = mat.M12; 
	matTransform._13 = mat.M13;  
	matTransform._14 = mat.M14;  
	matTransform._21 = mat.M21;  
	matTransform._22 = mat.M22;  
	matTransform._23 = mat.M23;  
	matTransform._24 = mat.M24;  
	matTransform._31 = mat.M31;  
	matTransform._32 = mat.M32;  
	matTransform._33 = mat.M33;  
	matTransform._34 = mat.M34;  
	matTransform._41 = mat.M41;  
	matTransform._42 = mat.M42;  
	matTransform._43 = mat.M43;  
	matTransform._44 = mat.M44;  

	m_pPDBInst->m_pPDBRenderer->m_matWorldPrevious = matTransform;
	ForceRenderScene();
	m_pPDBInst->m_pPDBRenderer->UpdatePDBRendererCenter();
}

//
//
//
void CDotNetPDB::SetSelect(bool select, bool bNeedUpdate)
{ 
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		m_pPDBInst->SetSelectChild(select); 
		if (bNeedUpdate == true)
			pProteinVistaRenderer->SelectedAtomApply();
	}
}

bool CDotNetPDB::Select::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer ){ pProteinVistaRenderer->SelectedAtomApply(); }

	return Convert::ToBoolean(m_pPDBInst->m_bSelect);
}

void CDotNetPDB::Select::set(bool bValue)
{
	SetSelect(bValue, true);
}

//==================================================================================================
//==================================================================================================
//==================================================================================================
//==================================================================================================
CDotNetClipping::CDotNetClipping( CSelectionDisplay * pSelectionDisplay , int clipIndex )
{
	m_pSelectionDisplay = pSelectionDisplay;
	m_clipIndex = clipIndex;
}

void CDotNetClipping::Init()
{

}

bool CDotNetClipping::Enable::get()
{
	if ( m_pSelectionDisplay != NULL )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			if ( m_clipIndex == 1 )
				return Convert::ToBoolean(pPropertyCommon->m_bClipping1);
			else
				return Convert::ToBoolean(pPropertyCommon->m_bClipping2);
		}
	}
	else
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
			return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bClipping0);
	}

	return true;
}

void CDotNetClipping::Enable::set(bool clip)
{
	if ( m_pSelectionDisplay != NULL )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			if ( m_clipIndex == 1 )
			{
				//pPropertyCommon->m_pItempItemClipping1->SetBool(clip);
				//pPropertyCommon->m_pItempItemClipping1->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItempItemClipping1));
			}
			else
			{
				//pPropertyCommon->m_pItempItemClipping2->SetBool(clip);
				//pPropertyCommon->m_pItempItemClipping2->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItempItemClipping2));
			}
		}
	}
	else
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
		{
			//pProteinVistaRenderer->m_pPropertyScene->m_pItembClipping0->SetBool(clip);
			//pProteinVistaRenderer->m_pPropertyScene->m_pItembClipping0->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItembClipping0));
		}

	}

	ForceRenderScene(); 
}

bool CDotNetClipping::Show::get()
{
	if ( m_pSelectionDisplay != NULL )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			if ( m_clipIndex == 1 )
				return Convert::ToBoolean(pPropertyCommon->m_bShowClipPlane1);
			else
				return Convert::ToBoolean(pPropertyCommon->m_bShowClipPlane2);
		}
	}
	else
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
			return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bShowClipPlane0);
	}

	return true;
}

void CDotNetClipping::Show::set(bool show)
{
	if ( m_pSelectionDisplay != NULL )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			if ( m_clipIndex == 1 )
			{
				//pPropertyCommon->m_pItempItemShowClipPlane1->SetBool(show);
				//pPropertyCommon->m_pItempItemShowClipPlane1->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItempItemShowClipPlane1)) ;
			}
			else
			{
				//pPropertyCommon->m_pItempItemShowClipPlane2->SetBool(show);
				//pPropertyCommon->m_pItempItemShowClipPlane2->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItempItemShowClipPlane2)) ;
			}
		}
	}
	else
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
		{
			//pProteinVistaRenderer->m_pPropertyScene->m_pItembShowClipPlane0->SetBool(show);
			//pProteinVistaRenderer->m_pPropertyScene->m_pItembShowClipPlane0->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItembShowClipPlane0));
		}
	}

	ForceRenderScene(); 
}

Color CDotNetClipping::Color::get()
{
	if ( m_pSelectionDisplay != NULL )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			if ( m_clipIndex == 1 )
				return System::Drawing::Color::FromArgb(pPropertyCommon->m_clipPlaneColor1);
			else
				return System::Drawing::Color::FromArgb(pPropertyCommon->m_clipPlaneColor2);
		}
	}
	else
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
			return System::Drawing::Color::FromArgb(pProteinVistaRenderer->m_pPropertyScene->m_clipPlaneColor0);
	}

	return System::Drawing::Color::Aqua;
}

void CDotNetClipping::Color::set(System::Drawing::Color color)
{
	if ( m_pSelectionDisplay != NULL )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			if ( m_clipIndex == 1 )
			{
				//pPropertyCommon->m_pItemclipPlaneColor1->SetColor( ManagedColor2COLORREF(color) );
				//pPropertyCommon->m_pItemclipPlaneColor1->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemclipPlaneColor1));
			}
			else
			{
				//pPropertyCommon->m_pItemclipPlaneColor2->SetColor( ManagedColor2COLORREF(color) );
				//pPropertyCommon->m_pItemclipPlaneColor2->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemclipPlaneColor2));
			}
		}
	}
	else
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
		{
			//pProteinVistaRenderer->m_pPropertyScene->m_pItemclipPlaneColor0->SetColor( ManagedColor2COLORREF(color) );
			//pProteinVistaRenderer->m_pPropertyScene->m_pItemclipPlaneColor0->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemclipPlaneColor0));
		}
	}

	ForceRenderScene();
}

int CDotNetClipping::Transparency::get()
{
	if ( m_pSelectionDisplay != NULL )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			if ( m_clipIndex == 1 )
				return pPropertyCommon->m_clipPlaneTransparency1;
			else
				return pPropertyCommon->m_clipPlaneTransparency2;
		}
	}
	else
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
			return pProteinVistaRenderer->m_pPropertyScene->m_clipPlaneTransparency0;
	}

	return 0;
}

void CDotNetClipping::Transparency::set(int transparency)
{
	if ( m_pSelectionDisplay != NULL )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			if ( m_clipIndex == 1 )
			{
				//pPropertyCommon->m_pItemclipPlaneTransparency1->SetNumber(transparency);
				//pPropertyCommon->m_pItemclipPlaneTransparency1->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemclipPlaneTransparency1));
			}
			else
			{
				//pPropertyCommon->m_pItemclipPlaneTransparency2->SetNumber(transparency);
				//pPropertyCommon->m_pItemclipPlaneTransparency2->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemclipPlaneTransparency2));
			}
		}
	}
	else
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
		{
			//pProteinVistaRenderer->m_pPropertyScene->m_pItemclipPlaneTransparency0->SetNumber(transparency);
			//pProteinVistaRenderer->m_pPropertyScene->m_pItemclipPlaneTransparency0->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemclipPlaneTransparency0));
		}
	}

	ForceRenderScene(); 
}

bool CDotNetClipping::Direction::get()
{
	if ( m_pSelectionDisplay != NULL )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			if ( m_clipIndex == 1 )
				return Convert::ToBoolean(pPropertyCommon->m_bClipDirection1);
			else
				return Convert::ToBoolean(pPropertyCommon->m_bClipDirection2);
		}
	}
	else
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
			return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bClipDirection0);
	}
	
	return true;
}

void CDotNetClipping::Direction::set(bool direction)
{
	if ( m_pSelectionDisplay != NULL )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			if ( m_clipIndex == 1 )
			{
				//pPropertyCommon->m_pItempItemClipDirection1->SetBool(direction);
				//pPropertyCommon->m_pItempItemClipDirection1->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItempItemClipDirection1));
			}
			else
			{
				//pPropertyCommon->m_pItempItemClipDirection2->SetBool(direction);
				//pPropertyCommon->m_pItempItemClipDirection2->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItempItemClipDirection2));
			}
		}
	}
	else
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
		{
			//pProteinVistaRenderer->m_pPropertyScene->m_pItembClipDirection0->SetBool(direction);
			//pProteinVistaRenderer->m_pPropertyScene->m_pItembClipDirection0->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItembClipDirection0));
		}
	}

	ForceRenderScene(); 
}

String ^ CDotNetClipping::Equation::get()
{
	CString strEqu;

	if ( m_pSelectionDisplay != NULL )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			/*if ( m_clipIndex == 1 )
				strEqu = pPropertyCommon->m_pItemClipPlaneEquation1->GetValue();
			else
				strEqu = pPropertyCommon->m_pItemClipPlaneEquation2->GetValue();*/
		}
	}
	else
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		//if ( pProteinVistaRenderer )
			//strEqu = pProteinVistaRenderer->m_pPropertyScene->m_pItemClipPlaneEquation0->GetValue();
	}

	return gcnew String(strEqu);
}

void CDotNetClipping::Equation::set(String ^ equ)
{
	CString strEqu;
	strEqu =MStrToCString(equ);

	if ( m_pSelectionDisplay != NULL )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			if ( m_clipIndex == 1 )
			{
				//pPropertyCommon->m_pItemClipPlaneEquation1->SetValue(strEqu);
				//pPropertyCommon->m_pItemClipPlaneEquation1->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemClipPlaneEquation1) );
			}
			else
			{
				//pPropertyCommon->m_pItemClipPlaneEquation2->SetValue(strEqu);
				//pPropertyCommon->m_pItemClipPlaneEquation2->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemClipPlaneEquation2) );
			}
		}
	}
	else
	{
		CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
		if ( pProteinVistaRenderer )
		{
			//pProteinVistaRenderer->m_pPropertyScene->m_pItemClipPlaneEquation0->SetValue(strEqu);
			//pProteinVistaRenderer->m_pPropertyScene->m_pItemClipPlaneEquation0->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItemClipPlaneEquation0) );
		}
	}
	ForceRenderScene(); 
}

//==================================================================================================
bool CDotNetAnnotation::Show::get()
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			return Convert::ToBoolean( pPropertyCommon->m_bAnnotation[m_iAnnotation] );
		}
	}
	return false;
}

void CDotNetAnnotation::Show::set(bool _value)
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			//pPropertyCommon->m_pItemShowAnnotation[m_iAnnotation]->SetBool(_value);
			//pPropertyCommon->m_pItemShowAnnotation[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemShowAnnotation[m_iAnnotation]) );
		}
	}
	ForceRenderScene();
}

String ^ CDotNetAnnotation::Text::get()
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			return gcnew String(pPropertyCommon->m_strAnnotation[m_iAnnotation]);
		}
	}

	return nullptr;
}

void CDotNetAnnotation::Text::set(String ^ _value)
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			//pPropertyCommon->m_pItemstrAnnotation[m_iAnnotation]->SetValue(_value);
			//pPropertyCommon->m_pItemstrAnnotation[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemstrAnnotation[m_iAnnotation]) );
		}
	}
	ForceRenderScene();
}

String ^ CDotNetAnnotation::FontName::get()
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			return gcnew String(pPropertyCommon->m_logFont[m_iAnnotation].lfFaceName);
		}
	}

	return nullptr;
} 

void CDotNetAnnotation::FontName::set(String ^ _value)
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			LOGFONT lf;
			//pPropertyCommon->m_pItemAnnotationFont[m_iAnnotation]->GetFont(&lf);

			CString strFontFaceName; 
			strFontFaceName =MStrToCString(_value);

			_tcscpy( lf.lfFaceName, strFontFaceName);
			//pPropertyCommon->m_pItemAnnotationFont[m_iAnnotation]->SetFont(lf);
			//pPropertyCommon->m_pItemAnnotationFont[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemAnnotationFont[m_iAnnotation]) );
		}
	}
	ForceRenderScene();
}

int CDotNetAnnotation::FontHeight::get()
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			HDC hDC = CreateCompatibleDC( NULL );
			int lFontSize = -((pPropertyCommon->m_logFont[m_iAnnotation].lfHeight * 72) / GetDeviceCaps(hDC, LOGPIXELSY));
			DeleteDC(hDC);

			return lFontSize;
		}
	}

	return 0;
} 

void CDotNetAnnotation::FontHeight::set(int _value)
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			HDC hDC = CreateCompatibleDC( NULL );
			int heightFont = -MulDiv( _value, (INT)(GetDeviceCaps(hDC, LOGPIXELSY)), 72 );
			DeleteDC(hDC);

			LOGFONT lf;
			//pPropertyCommon->m_pItemAnnotationFont[m_iAnnotation]->GetFont(&lf);
			//lf.lfHeight = heightFont;
			//pPropertyCommon->m_pItemAnnotationFont[m_iAnnotation]->SetFont(lf);
			//pPropertyCommon->m_pItemAnnotationFont[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemAnnotationFont[m_iAnnotation]) );
		}
	}
	ForceRenderScene();
}

IAnnotation::IDisplayMethod CDotNetAnnotation::DisplayMethod::get()
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			return static_cast<IAnnotation::IDisplayMethod>(pPropertyCommon->m_enumTextDisplayTechnique[m_iAnnotation]);
		}
	}

	return IAnnotation::IDisplayMethod::EnableZBuffer;
}

void CDotNetAnnotation::DisplayMethod::set(IAnnotation::IDisplayMethod _value)
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			//pPropertyCommon->m_pItemenumAnnotationTechnique[m_iAnnotation]->SetEnum(Convert::ToUInt32(_value));
			//pPropertyCommon->m_pItemenumAnnotationTechnique[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemenumAnnotationTechnique[m_iAnnotation]) );
		}
	}
	ForceRenderScene();
}

IAnnotation::ITextType CDotNetAnnotation::TextType::get()
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			return static_cast<IAnnotation::ITextType>(pPropertyCommon->m_enumAnnotatonType[m_iAnnotation]);
		}
	}

	return IAnnotation::ITextType::ThreeLetter;
} 

void CDotNetAnnotation::TextType::set(IAnnotation::ITextType _value)
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			//pPropertyCommon->m_pItemenumAnnotationType[m_iAnnotation]->SetEnum(Convert::ToUInt32(_value));
			//pPropertyCommon->m_pItemenumAnnotationType[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemenumAnnotationType[m_iAnnotation]) );
		}
	}
	ForceRenderScene();
}

IAnnotation::IColorScheme CDotNetAnnotation::ColorScheme::get()
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			return static_cast<IAnnotation::IColorScheme>(pPropertyCommon->m_enumAnnotationColorScheme[m_iAnnotation]);
		}
	}

	return IAnnotation::IColorScheme::FollowAtom;
} 

void CDotNetAnnotation::ColorScheme::set(IAnnotation::IColorScheme _value)
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			//pPropertyCommon->m_pItemAnnotationColorScheme[m_iAnnotation]->SetEnum(Convert::ToInt32(_value));
			//pPropertyCommon->m_pItemAnnotationColorScheme[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemAnnotationColorScheme[m_iAnnotation]) );
		}
	}
	ForceRenderScene();
}

System::Drawing::Color CDotNetAnnotation::Color::get()
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			return System::Drawing::Color::FromArgb(pPropertyCommon->m_annotationColor[m_iAnnotation]);
		}
	}

	return System::Drawing::Color::White;
}  

void CDotNetAnnotation::Color::set(System::Drawing::Color _value)
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			//pPropertyCommon->m_pItemannotationColor[m_iAnnotation]->SetColor(ManagedColor2COLORREF(_value));
			//pPropertyCommon->m_pItemannotationColor[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemannotationColor[m_iAnnotation]) );
		}
	}
	ForceRenderScene();
}

int CDotNetAnnotation::RelativeXPos::get()
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			return pPropertyCommon->m_annotationXPos[m_iAnnotation];
		}
	}

	return 0;
} 

void CDotNetAnnotation::RelativeXPos::set(int _value)
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			//pPropertyCommon->m_pItemAnnotationXPos[m_iAnnotation]->SetNumber(_value);
			//pPropertyCommon->m_pItemAnnotationXPos[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemAnnotationXPos[m_iAnnotation]) );
		}
	}
	ForceRenderScene();
}

int CDotNetAnnotation::RelativeYPos::get()
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			return pPropertyCommon->m_annotationYPos[m_iAnnotation];
		}
	}

	return 0;
} 

void CDotNetAnnotation::RelativeYPos::set(int _value)
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			//pPropertyCommon->m_pItemAnnotationYPos[m_iAnnotation]->SetNumber(_value);
			//pPropertyCommon->m_pItemAnnotationYPos[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemAnnotationYPos[m_iAnnotation]) );
		}
	}
	ForceRenderScene();
}

int CDotNetAnnotation::RelativeZPos::get()
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			return pPropertyCommon->m_annotationZPos[m_iAnnotation];		
		}
	}

	return 0;
} 

void CDotNetAnnotation::RelativeZPos::set(int _value)
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			//pPropertyCommon->m_pItemAnnotationZPos[m_iAnnotation]->SetNumber(_value);
			//pPropertyCommon->m_pItemAnnotationZPos[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemAnnotationZPos[m_iAnnotation]) );
		}
	}
	ForceRenderScene();
}

int CDotNetAnnotation::TextXPos::get()
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			return pPropertyCommon->m_annotationTextXPos[m_iAnnotation];				
		}
	}

	return 0;
} 

void CDotNetAnnotation::TextXPos::set(int _value)
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			//pPropertyCommon->m_pItemAnnotationTextXPos[m_iAnnotation]->SetNumber(_value);
			//pPropertyCommon->m_pItemAnnotationTextXPos[m_iAnnotation]->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemAnnotationTextXPos[m_iAnnotation]) );
		}
	}
	ForceRenderScene();
}

int CDotNetAnnotation::TextYPos::get()
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			return pPropertyCommon->m_annotationTextYPos[m_iAnnotation];						
		}
	}

	return 0;
} 

void CDotNetAnnotation::TextYPos::set(int _value)
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			//pPropertyCommon->m_pItemAnnotationTextYPos[m_iAnnotation]->SetNumber(_value);
			//pPropertyCommon->m_pItemAnnotationTextYPos[m_iAnnotation]->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItemAnnotationTextYPos[m_iAnnotation]) );
		}
	}
	ForceRenderScene();
}

int CDotNetAnnotation::DepthBias::get()
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			return pPropertyCommon->m_annotationDepthBias[m_iAnnotation];						
		}
	}

	return 0;
} 

void CDotNetAnnotation::DepthBias::set(int _value)
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			//pPropertyCommon->m_pItemAnnotationDepthBias[m_iAnnotation]->SetNumber(_value);
		}
	}
	ForceRenderScene();
}

int CDotNetAnnotation::Transparency::get()
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			return pPropertyCommon->m_annotationTransparency[m_iAnnotation];						
		}
	}

	return 0;
} 

void CDotNetAnnotation::Transparency::set(int _value)
{
	if ( m_pSelectionDisplay )
	{
		CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
		if ( pPropertyCommon )
		{
			//pPropertyCommon->m_pItemAnnotationTransparency[m_iAnnotation]->SetNumber(_value);
		}
	}
	ForceRenderScene();
}

//==================================================================================================
//==================================================================================================
CDotNetSelection::CDotNetSelection(CSelectionDisplay * pSelectionDisplay, CDotNetProteinInsight ^ proteinVista)
{
	m_pDotNetProteinVista = proteinVista;
	m_pSelectionDisplay = pSelectionDisplay;
	m_bSelectionUsed = false;
}

void CDotNetSelection::Init()
{
	m_property = gcnew CDotNetProperty(m_pSelectionDisplay);		
	m_property->Init();
	m_propertyWireframe = gcnew CDotNetPropertyWireframe(m_pSelectionDisplay);	
	m_propertyWireframe->Init();
	m_propertyStick = gcnew CDotNetPropertyStick(m_pSelectionDisplay);
	m_propertyStick->Init();
	m_propertySpaceFill = gcnew CDotNetPropertySpaceFill(m_pSelectionDisplay);
	m_propertySpaceFill->Init();
	m_propertyBallnStick = gcnew CDotNetPropertyBallnStick(m_pSelectionDisplay);
	m_propertyBallnStick->Init();
	m_propertyRibbon = gcnew CDotNetPropertyRibbon(m_pSelectionDisplay);
	m_propertyRibbon->Init();
	m_propertySurface = gcnew CDotNetPropertySurface(m_pSelectionDisplay);
	m_propertySurface->Init();

	//
	//    selection을 구성하는 atom 에 대해서 list를 만든다.
	//    
	m_arrayAtoms = gcnew List<IAtom ^>(m_pSelectionDisplay->m_arrayAtomInst.size());
	for ( int i = 0 ; i < m_pSelectionDisplay->m_arrayAtomInst.size(); i++ )
	{
		CAtomInst * pAtomInst = m_pSelectionDisplay->m_arrayAtomInst[i];

		//
		//    pAtom 에 대해서 CDotNetAtom 으로의 pointer를 찾아야 한다.
		//    나중에 수정
		//    
		CDotNetAtom ^ pIAtom = gcnew CDotNetAtom ( pAtomInst, (CDotNetResidue ^)nullptr );
		m_arrayAtoms->Add(pIAtom);
		pIAtom->Init();
	}
}

IProteinInsight::IDisplayStyle CDotNetSelection::DisplayStyle::get()
{
	return static_cast<IProteinInsight::IDisplayStyle>(m_pSelectionDisplay->m_displayStyle);
}

void CDotNetSelection::DisplayStyle::set(IProteinInsight::IDisplayStyle displayStyle)
{
	CPropertyCommon	* propertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	
	if ( propertyCommon )
	{
		//propertyCommon->m_pItemDisplayMode->SetEnum(Convert::ToInt32(displayStyle));
		//propertyCommon->m_pItemDisplayMode->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(propertyCommon->m_pItemDisplayMode) );
	}
}

String ^ CDotNetSelection::PDBID::get()
{
	return gcnew String(m_pSelectionDisplay->m_pPDBRenderer->GetPDBInst()->GetPDB()->m_strPDBID);
}

String ^ CDotNetSelection::Filename::get()
{
	return gcnew String(m_pSelectionDisplay->m_pPDBRenderer->GetPDBInst()->GetPDB()->m_strFilename);
}


String ^ CDotNetSelection::Name::get()
{
	return gcnew String(m_pSelectionDisplay->GetPropertyCommon()->m_strSelectionName);
}

void CDotNetSelection::Name::set(String ^ name)
{
	CPropertyCommon * pPropertyCommon = m_pSelectionDisplay->GetPropertyCommon();
	if ( pPropertyCommon )
	{
		CString strName;
		strName =MStrToCString(name);
		pPropertyCommon->m_strSelectionName = strName;
		//pPropertyCommon->m_pItempSelectionName->GetGrid()->SendNotifyMessage ( XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pPropertyCommon->m_pItempSelectionName) );
	}
}

bool CDotNetSelection::Show::get()
{
	return ( Convert::ToBoolean(m_pSelectionDisplay->m_bShow) );
}

void CDotNetSelection::Show::set(bool show)
{
	if ( m_pSelectionDisplay->m_bShow != Convert::ToInt32(show) )
	{
		/*if ( AfxGetMainWnd() )
		{
			CSelectionListPane * pSelectionListPane = ((CMainFrame*)AfxGetMainWnd())->m_pSelectionListPane;
			if ( pSelectionListPane )
			{
				long iList = pSelectionListPane->GetListCtrlIndex(m_pSelectionDisplay);
				if ( iList != -1 )
				{
					pSelectionListPane->m_htmlListCtrl->SetItemCheck( iList, Convert::ToInt32(show) );
					pSelectionListPane->m_htmlListCtrl->SendCheckStateChangedNotification(iList);
				}
			}
		}*/
	}
	ForceRenderScene(); 
}

bool CDotNetSelection::Select::get()
{
	return Convert::ToBoolean(m_pSelectionDisplay->m_bSelect);
}

void CDotNetSelection::Select::set(bool select)
{
	if ( select == true )
	{
		if ( select != Convert::ToBoolean(m_pSelectionDisplay->m_bSelect) )
		{	//	현재꺼가 선택되어 있지 않다. -> 선택되게 바꾼다.
			/*if ( AfxGetMainWnd() )
			{
				CSelectionListPane * pSelectionListPane = ((CMainFrame*)AfxGetMainWnd())->m_pSelectionListPane;
				if ( pSelectionListPane )
				{
					pSelectionListPane->Deselect();
					pSelectionListPane->SelectListItem(m_pSelectionDisplay);
				}
			}*/
		}
	}
	else
	{
		if ( select != Convert::ToBoolean(m_pSelectionDisplay->m_bSelect) )
		{	//	현재꺼가 선택되어 있다.--> deselect 로 바꾼다.
			/*if ( AfxGetMainWnd() )
			{
				CSelectionListPane * pSelectionListPane = ((CMainFrame*)AfxGetMainWnd())->m_pSelectionListPane;
				if ( pSelectionListPane )
				{
					pSelectionListPane->Deselect();
				}
			}*/
		}
	}
}

void CDotNetSelection::MoveCenter()
{
	//    현재 m_pSelectionDisplay 를 선택.
	Select = true;
	GetMainActiveView()->OnCenterMolecule();
	//GetMainActiveView()->SendMessage(WM_COMMAND, ID_CENTER_MOLECULE );
	 
	ForceRenderScene();
}

void CDotNetSelection::RotationAxis(Vector3 axis, float angle)
{
	Select = true;

	D3DXMATRIX matRot;
	D3DXVECTOR3 vecAxis(axis.X, axis.Y, axis.Z);
	D3DXMatrixRotationAxis(&matRot, &vecAxis, angle);
 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorldUserInput = matRot;

	ForceRenderScene();
	m_pSelectionDisplay->m_pPDBRenderer->UpdatePDBRendererCenter();
}

void CDotNetSelection::RotationX(float angle)
{
	Select = true;

	D3DXMATRIX matRot;
	D3DXMatrixRotationX(&matRot, angle);

	m_pSelectionDisplay->m_pPDBRenderer->m_matWorldUserInput = matRot;

	ForceRenderScene();
	m_pSelectionDisplay->m_pPDBRenderer->UpdatePDBRendererCenter();
}

void CDotNetSelection::RotationY(float angle)
{
	Select = true;

	D3DXMATRIX matRot;
	D3DXMatrixRotationY(&matRot, angle);
 	m_pSelectionDisplay->m_pPDBRenderer->m_matWorldUserInput = matRot;
	
	ForceRenderScene();
	m_pSelectionDisplay->m_pPDBRenderer->UpdatePDBRendererCenter();
 
}

void CDotNetSelection::RotationZ(float angle)
{
	Select = true;

	D3DXMATRIX matRot;
	D3DXMatrixRotationZ(&matRot, angle);
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorldUserInput = matRot;

	ForceRenderScene();
	m_pSelectionDisplay->m_pPDBRenderer->UpdatePDBRendererCenter();
}

void CDotNetSelection::Move(float x, float y, float z)
{
	Select = true;

	D3DXMATRIX matTrans;
	D3DXMatrixTranslation(&matTrans, x, y, z);
 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorldUserInput = matTrans;

	ForceRenderScene();
	m_pSelectionDisplay->m_pPDBRenderer->UpdatePDBRendererCenter();
}

void CDotNetSelection::ViewAll(float time)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		pProteinVistaRenderer->SetCameraAnimation( m_pSelectionDisplay->m_arrayAtomInst , time );
	}
}

Microsoft::DirectX::Matrix CDotNetSelection::TransformLocal::get()
{
	Select = true;

	Microsoft::DirectX::Matrix mat;
	mat.M11 = m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._11;
	mat.M12 = m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._12;
	mat.M13 = m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._13;
	mat.M14 = m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._14;
	mat.M21 = m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._21;
	mat.M22 = m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._22;
	mat.M23 = m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._23;
	mat.M24 = m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._24;
	mat.M31 = m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._31;
	mat.M32 = m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._32;
	mat.M33 = m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._33;
	mat.M34 = m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._34;
	mat.M41 = m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._41;
	mat.M42 = m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._42;
	mat.M43 = m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._43;
	mat.M44 = m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._44;

	return mat;
}

void CDotNetSelection::TransformLocal::set(Microsoft::DirectX::Matrix mat)
{
	Select = true;

	m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._11 = mat.M11 ; 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._12 = mat.M12 ; 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._13 = mat.M13 ; 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._14 = mat.M14 ; 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._21 = mat.M21 ; 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._22 = mat.M22 ; 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._23 = mat.M23 ; 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._24 = mat.M24 ; 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._31 = mat.M31 ; 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._32 = mat.M32 ; 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._33 = mat.M33 ; 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._34 = mat.M34 ; 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._41 = mat.M41 ; 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._42 = mat.M42 ; 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._43 = mat.M43 ; 
	m_pSelectionDisplay->m_pPDBRenderer->m_matWorld._44 = mat.M44 ; 
}

IPDB ^ CDotNetSelection::PDB::get()
{
	CPDBInst * pPDBInst = m_pSelectionDisplay->m_pPDBRenderer->GetPDBInst();

	for each ( CDotNetPDB ^ pPDB in m_pDotNetProteinVista->PDBs )
	{
		if ( pPDB->GetPDB() == pPDBInst )
		{
			return pPDB;
		}
	}

	return nullptr;
}

//==================================================================================================
//==================================================================================================
//
bool CDotNetLight::Enable::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		if ( m_indexLight == 0 )
			return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bLight1Use);
		else
			return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bLight2Use);
	}
	return true;
}

void CDotNetLight::Enable::set(bool light)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		if ( m_indexLight == 0 )
		{
			pProteinVistaRenderer->m_pPropertyScene->m_bLight1Use = light?TRUE:FALSE;
			 GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_LIGHT1_ENABLE,"",NULL);
		}
		else
		{
			pProteinVistaRenderer->m_pPropertyScene->m_bLight2Use = light?TRUE:FALSE;
		    GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_LIGHT1_ENABLE,"",NULL);
 		}
	}
	ForceRenderScene(); 
}

bool CDotNetLight::Show::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		if ( m_indexLight == 0 )
			return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bLight1Show);
		else
			return Convert::ToBoolean(pProteinVistaRenderer->m_pPropertyScene->m_bLight2Show);
	}

	return true;
}

void CDotNetLight::Show::set(bool light)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		if ( m_indexLight == 0 )
		{
			pProteinVistaRenderer->m_pPropertyScene->m_bLight1Show = light?TRUE:FALSE;
			//pProteinVistaRenderer->m_pPropertyScene->m_pItembLight1Show->SetBool(light);
			//pProteinVistaRenderer->m_pPropertyScene->m_pItembLight1Show->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItembLight1Show) );
			GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_LIGHT1_ENABLE,"",NULL);
		}
		else
		{
			pProteinVistaRenderer->m_pPropertyScene->m_bLight2Show = light?TRUE:FALSE;
			//pProteinVistaRenderer->m_pPropertyScene->m_pItembLight2Show->SetBool(light);
			//pProteinVistaRenderer->m_pPropertyScene->m_pItembLight2Show->GetGrid()->SendNotifyMessage(XTP_PGN_ITEMVALUE_CHANGED, (LPARAM)(pProteinVistaRenderer->m_pPropertyScene->m_pItembLight2Show) );
			GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_LIGHT2_ENABLE,"",NULL);
		}
	}
	ForceRenderScene(); 
}

System::Drawing::Color CDotNetLight::Color::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		if ( m_indexLight == 0 )
			return System::Drawing::Color::FromArgb(pProteinVistaRenderer->m_pPropertyScene->m_light1Color);
		else
			return System::Drawing::Color::FromArgb(pProteinVistaRenderer->m_pPropertyScene->m_light2Color);
	}

	return System::Drawing::Color::White;
}

void CDotNetLight::Color::set(System::Drawing::Color color)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		if ( m_indexLight == 0 )
		{
			pProteinVistaRenderer->m_pPropertyScene->m_light1Color =ManagedColor2COLORREF(color);
			GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_LIGHT1_COLOR,"",NULL);
		}
		else
		{
		   pProteinVistaRenderer->m_pPropertyScene->m_light2Color =ManagedColor2COLORREF(color);
		   GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_LIGHT1_COLOR,"",NULL);
		}
	}
	ForceRenderScene(); 
}

int CDotNetLight::Intensity::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		if ( m_indexLight == 0)
			return pProteinVistaRenderer->m_pPropertyScene->m_light1Intensity;
		else
			return pProteinVistaRenderer->m_pPropertyScene->m_light2Intensity;
	}

	return 1;
}

void CDotNetLight::Intensity::set(int intensity)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		if ( m_indexLight == 0 )
		{
		   pProteinVistaRenderer->m_pPropertyScene->m_light1Intensity =intensity;
		   GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_LIGHT2_COLOR,"",NULL);
		}
		else
		{
		   pProteinVistaRenderer->m_pPropertyScene->m_light2Intensity =intensity;
		   GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_LIGHT2_COLOR,"",NULL);
		}
	}
	ForceRenderScene(); 
}

Vector3 CDotNetLight::Position::get()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		D3DXMATRIX		matWorldLight = pProteinVistaRenderer->m_LightControl[m_indexLight].m_matWorld;
		D3DXVECTOR3		vecResult;
		D3DXVECTOR3		vecOrigin(0,0,1);
		D3DXVec3TransformCoord(&vecResult, &vecOrigin , &matWorldLight);

		Vector3 result;
		result.X = vecResult.x;
		result.Y = vecResult.y;
		result.Z = vecResult.z;
		return result;
	}

	return Vector3(0,0,0);
}

void CDotNetLight::Position::set(Vector3 pos )
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		float len = pos.Length();
		D3DXVECTOR3 posLight(pos.X, pos.Y, pos.Z);
		D3DXVECTOR3 posLightNorm;
		D3DXVec3Normalize(&posLightNorm, &posLight);
		
		pProteinVistaRenderer->m_LightControl[m_indexLight].SetLightDirection(posLightNorm);
		pProteinVistaRenderer->m_LightControl[m_indexLight].SetRadius(len);
				
		GetMainActiveView()->ChanegPropertyValue(PROPERTY_SCENE_LIGHT1_RADIUS,"",NULL);
		ForceRenderScene(); 
	}
}




CDotNetMovie::CDotNetMovie()
{
	m_movieFilename = gcnew String("Temp.wmv");
	m_width = -1;
	m_height = -1;
	m_fps = 10;
}

void CDotNetMovie::BeginMovie(String ^ filename, int width, int height, int fps )
{
	CancelMovie();
	
	m_movieFilename = gcnew String(filename);
	m_width = width;
	m_height = height;
	m_fps = fps;
}

void CDotNetMovie::Capture(int frame)
{
	static int serial = 0;
	CString tempDir = GetMainApp()->m_strBaseTempPath;
	TCHAR	tempPath[MAX_PATH];
	::GetTempFileName(tempDir , "PV", GetTickCount(), tempPath);
	_tcscat(tempPath, _T(".png") );

	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		HRESULT hr;
		hr = pProteinVistaRenderer->SaveScreenImage ( tempPath, m_width, m_height , D3DXIFF_PNG);
		if ( SUCCEEDED(hr) )
		{
			m_listImageFilename.Add(gcnew String(tempPath));
			m_listImageFrame.Add(frame);
		}
	}
}

//	
void CDotNetMovie::CaptureCameraAnimation(IAtom ^ atom, int frame)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		CSTLArrayAtomInst arrayAtom;
		arrayAtom.push_back( dynamic_cast<CDotNetAtom ^>(atom)->GetUnManagedAtom() );
		D3DXVECTOR3 posTarget;

		pProteinVistaRenderer->FindAnimationTargetPos( arrayAtom, posTarget );

		Vector3 vecPosTarget(posTarget.x, posTarget.y, posTarget.z );
		CaptureCameraAnimation( vecPosTarget, frame );
	}
}

void CDotNetMovie::CaptureCameraAnimation(IResidue ^ residue, int frame)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		D3DXVECTOR3 posTarget;
		pProteinVistaRenderer->FindAnimationTargetPos( dynamic_cast<CDotNetResidue ^>(residue)->GetUnManagedResidue()->m_arrayAtomInst , posTarget );

		Vector3 vecPosTarget(posTarget.x, posTarget.y, posTarget.z );
		CaptureCameraAnimation( vecPosTarget, frame );
	}
}

void CDotNetMovie::CaptureCameraAnimation(IVP ^ vp, int frame)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		D3DXVECTOR3 posTarget;
		pProteinVistaRenderer->FindAnimationTargetPos( dynamic_cast<CDotNetSelection ^>(vp)->m_pSelectionDisplay->m_arrayAtomInst , posTarget );

		Vector3 vecPosTarget(posTarget.x, posTarget.y, posTarget.z );
		CaptureCameraAnimation( vecPosTarget, frame );
	}
}

void CDotNetMovie::CaptureCameraAnimation(Vector3 pos , int frame)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		D3DXVECTOR3 endPos(pos.X, pos.Y, pos.Z);
		pProteinVistaRenderer->GenerateCameraAnimationPos(endPos, frame);
		for ( int i = 0 ; i <= frame ; i++ )
		{
			D3DXVECTOR3 cameraPos = pProteinVistaRenderer->GetCameraAnimationPos(i);
			pProteinVistaRenderer->SetCameraPos(cameraPos);
			ForceRenderScene(); 

			Capture(1);
		}
	}
}

//
void CDotNetMovie::Caption(String ^ strCaption, int frame, IMovie::ICaptionPosition pos, String ^ fontFamily, int fontHeight )
{

}

void CDotNetMovie::EndMovie()
{
	HRESULT hr;
	CString strMovieFilename; 
	strMovieFilename = MStrToCString(m_movieFilename);

	CStringArray strArrayFilename;
	for each ( String ^ str in m_listImageFilename )
	{
		CString imageFilename;
		imageFilename =MStrToCString(str);
		strArrayFilename.Add(imageFilename);
	}

	CSTLIntArray arrayFrame;
	for each ( int frame in m_listImageFrame )
	{
		arrayFrame.push_back(frame);
	}

	hr = GetMainApp() ->MakeMovieWithImages(strMovieFilename, strArrayFilename, arrayFrame, m_width, m_height, m_fps );
	if ( FAILED(hr) )
	{

	}

	CancelMovie();
}

void CDotNetMovie::CancelMovie()
{
	//    file을 모두 지운다.
	for each ( String ^ filename in m_listImageFilename )
	{
		CString strFilename;
		strFilename =MStrToCString(filename);
		::DeleteFile( strFilename );
	}
	
	m_listImageFilename.Clear();
	m_listImageFrame.Clear();
}

CDotNetUtility::CDotNetUtility()
{
}

System::Drawing::Color	CDotNetUtility::GetGradientColor (int iStep, int nTotalStep)
{

	D3DXCOLOR color = FindGradientColor ( iStep , nTotalStep );
	return System::Drawing::Color::FromArgb(color.r*255.0f, color.g*255.0f, color.b*255.0f );
}

System::Drawing::Color	CDotNetUtility::GetGradientColor (System::Drawing::Color color1, System::Drawing::Color	color2, int iStep, int nTotalStep)
{
	COLORREF _color1 = RGB(color1.R, color1.G, color1.B );
	COLORREF _color2 = RGB(color2.R, color2.G, color2.B );

	D3DXCOLOR color = FindGradientColor ( COLORREF2D3DXCOLOR(_color1), COLORREF2D3DXCOLOR(_color2), iStep , nTotalStep );

	return System::Drawing::Color::FromArgb(color.r*255.0f, color.g*255.0f, color.b*255.0f );
}

void CDotNetUtility::OutputMsg(String ^ msg)
{
	CString strMsg;
	strMsg =MStrToCString(msg);
	GetMainActiveView()->OutPutLog(strMsg);
}

void CDotNetUtility::OutputMsgInStatusBar(String ^ msg)
{
	CString strMsg;
	strMsg =MStrToCString(msg);
	GetMainActiveView()->SetStatusBarText(strMsg);
}

void CDotNetUtility::SetProgressInStatusBar(int progress)
{
	//CMainFrame *pFrame = (CMainFrame*)AfxGetApp()->m_pMainWnd;
	//pFrame->m_wndProgCtrl.SetRange(0,100);        //    범위를 0에서 100 으로 설정
	//pFrame->m_wndProgCtrl.SetPos ( max(min(progress,100),0) );

	//CString strMsg;
	//strMsg.Format("%d%%", progress);
	//pFrame->m_wndStatusBar.SetPaneText(STATUS_PANE_PROGRESS_TEXT, strMsg );
	//pFrame->m_wndStatusBar.OnPaint();	//	사실 틀린방법. 이 방법 이외에 다른 방법이 있는지 확인.
}

Microsoft::DirectX::Direct3D::Device ^ CDotNetUtility::GetDirect3DDevice9()
{
	//    native C++ 의 Direct3D Device 포인터를 얻어서 managed Direct3D Device 를 생성한다.
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		Microsoft::DirectX::Direct3D::Device ^ device = 
			gcnew Microsoft::DirectX::Direct3D::Device((System::IntPtr) pProteinVistaRenderer->GetD3DDevice() ) ;
		return device;
	}

	return nullptr;
}

//==================================================================================================
//
CDotNetProteinInsight::CDotNetProteinInsight()
{
	if ( m_pMovie == nullptr )
	{
		m_pMovie = gcnew CDotNetMovie();
		m_pMovie->Init();
	}

	if ( m_pUtilty == nullptr )
	{
		m_pUtilty = gcnew CDotNetUtility();
		m_pUtilty->Init();
	}

}

CDotNetProteinInsight::!CDotNetProteinInsight()
{
	WorkspaceDestroy();
	m_pMovie = nullptr;
	m_pUtilty = nullptr;

	GC::Collect();
}

CDotNetProteinInsight::~CDotNetProteinInsight()
{
	this->!CDotNetProteinInsight();
}

void CDotNetProteinInsight::WorkspaceInit()
{
	WorkspaceDestroy();

	if ( m_arrayVP == nullptr )
		m_arrayVP = gcnew List <IVP ^>;

	if ( m_pPropertyScene == nullptr )
	{
		m_pPropertyScene = gcnew CDotNetPropertyScene();
		m_pPropertyScene->Init();
	}

	if ( m_arrayPDB == nullptr )
		m_arrayPDB = gcnew List <IPDB ^>();

	Init();
}

void CDotNetProteinInsight::WorkspaceDestroy()
{
	m_arrayVP = nullptr;
	m_pPropertyScene = nullptr;
	m_arrayPDB = nullptr;

	GC::Collect();
}

//	
//	Open 일때도 불리고, Add 일때에도 불린다.
//	
void CDotNetProteinInsight::Init()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer == NULL )
		return;

	//	m_arrayPDB에 값이 들어있는 경우에 m_bUsed 를 false로 만든다.
	for each ( CDotNetPDB ^ pPDB in m_arrayPDB )
		pPDB->m_bUsed = false;

	for ( int i = 0 ; i < pProteinVistaRenderer->m_arrayPDBRenderer.size() ; i++ )
	{
		m_pUtilty->OutputMsgInStatusBar("Preparing to execute plug-in");
		m_pUtilty->SetProgressInStatusBar((i+1)*100/pProteinVistaRenderer->m_arrayPDBRenderer.size());
		MessagePump();

		CPDBRenderer * pPDBRenderer = pProteinVistaRenderer->m_arrayPDBRenderer[i];

		bool bFind = false;
		for each ( CDotNetPDB ^ pPDB in m_arrayPDB )
		{
			if ( pPDB->GetPDB() == pPDBRenderer->GetPDBInst() )
			{
				pPDB->m_bUsed = true;
				bFind = true;
				break;
			}
		}

		if ( bFind == false )
		{
			CDotNetPDB ^ dotnetPDB = gcnew CDotNetPDB(pPDBRenderer->GetPDBInst(), this);
			dotnetPDB->m_bUsed = true;
			m_arrayPDB->Add( dotnetPDB );
			dotnetPDB->Init();
		}
	}

	//    사용되지 않는것 지운다.
	bool FindPDBUsed(IPDB ^ pdb);
	m_arrayPDB->RemoveAll(gcnew Predicate <IPDB ^>(FindPDBUsed));

	m_pUtilty->OutputMsgInStatusBar("");
	m_pUtilty->SetProgressInStatusBar(0);

}

//    predicate of RemoveAll
bool FindPDBUsed(IPDB ^ pdb)
{
	return !(((CDotNetPDB^)pdb)->m_bUsed);
};

bool CDotNetProteinInsight::Open(String ^filename)
{
	CString strFilename;
	strFilename =MStrToCString(filename);

	CString strNewFilename;
	GetMainApp()->CheckExistPDBFile(strFilename, strNewFilename);

	if ( strNewFilename.IsEmpty() == TRUE )
		return false;

	WorkspaceDestroy();

	GetMainApp()->OpenDocumentFile(strNewFilename);

	WorkspaceInit();
	ForceRenderScene();
	MessagePump();

	return true; 
}

bool CDotNetProteinInsight::AddPDB(String ^filename)
{
	CString strFilename;
	strFilename =MStrToCString(filename);

	CString strNewFilename;
	GetMainApp()->CheckExistPDBFile(strFilename, strNewFilename);
	if ( strNewFilename.IsEmpty() == TRUE )
		return false;

	GetMainApp()->GetActiveProteinVistaView()->AddPDB(strNewFilename);

	Init();

	ForceRenderScene();
	MessagePump();

	return true; 
}

void CDotNetProteinInsight::SaveImage(String ^ filename, int width, int height, IProteinInsight::IImageFormat format)
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		HRESULT hr;
		CString strFilename;
		strFilename =MStrToCString(filename);
		D3DXIMAGE_FILEFORMAT D3DFormat[] = { D3DXIFF_PNG, D3DXIFF_BMP, D3DXIFF_JPG, D3DXIFF_DIB };
		hr = pProteinVistaRenderer->SaveScreenImage ( strFilename, width, height , D3DFormat[Convert::ToInt32(format)]);
	}
}
 
void CDotNetProteinInsight::UpdateSelections()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		for each ( CDotNetSelection ^ selection in m_arrayVP ) { selection->m_bSelectionUsed = FALSE; }

		//  순서대로 ordering을 위해서 loop를 두번 사용.
		//	listCtrl의 순서.
		for ( int iSerial = 0 ; iSerial < CSelectionDisplay::m_maxSelectionIndex ; iSerial ++ )
		{
			//	m_arraySelectionDisplay의 순서.
			for ( int i = 0 ; i < MAX_DISPLAY_SELECTION_INDEX ; i++ )
			{
				CSelectionDisplay * pSelectionDisplay = pProteinVistaRenderer->m_arraySelectionDisplay[i];
				if ( pSelectionDisplay != NULL )
				{
					if ( pSelectionDisplay->m_iSerial == iSerial )
					{
						bool bFind = false;
						for each ( CDotNetSelection ^ selection in m_arrayVP ) 
						{
							if ( selection->m_pSelectionDisplay == pSelectionDisplay )
							{	//	
								selection->m_bSelectionUsed = true;
								bFind = true;
								break;
							}
						}

						if ( bFind == false )
						{	//	새로운것.
							CDotNetSelection ^ donetSelection = gcnew CDotNetSelection(pSelectionDisplay, this);
							m_arrayVP->Add(donetSelection);
							donetSelection->m_bSelectionUsed = true;
							donetSelection->Init();
						}

						break;
					}
				}
			}
		}
   
		bool FindSelectionUsed( IVP ^ selection);
		m_arrayVP->RemoveAll( gcnew Predicate <IVP ^ >(FindSelectionUsed) );
	}

	ForceRenderScene();
	MessagePump();
}

//    predicate of RemoveAll
bool FindSelectionUsed(IVP ^ selection)
{
	return !(((CDotNetSelection^)selection)->m_bSelectionUsed);
};

bool CDotNetProteinInsight::SaveWorkspace(String ^ filename)
{
	GetMainApp()->OnSaveWorkspace();
	return true;
}

bool CDotNetProteinInsight::ClosePDB (IPDB ^ pdb)
{
	GetMainActiveView()->ClosePDBnChild( ((CDotNetPDB ^)pdb)->GetPDB()->m_pPDBRenderer );

	Init();

	ForceRenderScene();
	MessagePump();

	return true; 
}

bool CDotNetProteinInsight::CloseWorkspace()
{
	/*CView * pView = GetMainActiveView();
	if ( pView )
	{
		CFrameWnd* pFrame = (CFrameWnd*)(pView->GetParent());
		if ( pFrame )
		{
			::SendMessage(pFrame->GetSafeHwnd(), WM_CLOSE, 0, 0);
		}
	}*/

	return true; 
}

String ^ CDotNetProteinInsight::ScriptDir::get()
{
	/*CMainFrame * pMainFrame = ((CMainFrame*)AfxGetMainWnd());
	if ( pMainFrame )
	{
		if ( pMainFrame->m_pScriptInputPane->m_strFilename.IsEmpty() != TRUE )
		{
			TCHAR drive[MAX_PATH];
			TCHAR dir[MAX_PATH];
			_tsplitpath(pMainFrame->m_pScriptInputPane->m_strFilename, drive, dir, NULL, NULL);

			CString strDir;
			strDir.Format("%s%s" , drive, dir);

			return gcnew String(strDir);
		}
	}*/
	return nullptr;	
}

String ^ CDotNetProteinInsight::ScriptPath::get()
{
	/*CMainFrame * pMainFrame = ((CMainFrame*)AfxGetMainWnd());
	if ( pMainFrame )
	{
	if ( pMainFrame->m_pScriptInputPane->m_strFilename.IsEmpty() != TRUE )
	{
	return gcnew String(pMainFrame->m_pScriptInputPane->m_strFilename);
	}
	}*/

	return nullptr;	
}

String ^ CDotNetProteinInsight::PlugInDir::get()
{
	return gcnew String( GetMainApp() ->m_strBasePlugInPath );
}

String ^ CDotNetProteinInsight::PlugInPath::get()
{
	return gcnew String( GetMainApp() ->m_strCurrentPluginFile );
}

String ^ CDotNetProteinInsight::ProteinInsightDir::get()
{
	return gcnew String( GetMainApp() ->m_strBaseExePath );
	 
}

String ^ CDotNetProteinInsight::PDBDir::get()
{
	return gcnew String( GetMainApp() ->m_strBaseDownloadPDB );
 
}

String ^ CDotNetProteinInsight::WorkspaceDir::get()
{
	return gcnew String( GetMainApp() ->m_strBaseWorkspacePath );
}

String ^ CDotNetProteinInsight::MovieDir::get()
{
	return gcnew String( GetMainApp() ->m_strBaseScriptMovie );
}

void CDotNetProteinInsight::UpdateSelect()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer ){ pProteinVistaRenderer->SelectedAtomApply(); }
	ForceRenderScene();
	MessagePump();
}

List<IVP ^> ^ CDotNetProteinInsight::VPs::get()
{ 
	UpdateSelections();
	return m_arrayVP;
}

IVP ^ CDotNetProteinInsight::AddVP(IProteinInsight::IDisplayStyle displayStyle)
{
	int countOld = m_arrayVP->Count;
	CProteinVistaView* pView = GetMainActiveView();
	if ( pView )
	{
		long idCommand[] = { ID_BUTTON_WIREFRAME, ID_BUTTON_STICK, ID_BUTTON_BALL, ID_BUTTON_BALL_STICK, 
			                 ID_BUTTON_RIBBON, ID_BUTTON_DOTSURFACE };
        int idValue = Convert::ToInt32(displayStyle);
		if(idValue == 0)
		{
			pView->OnButtonWireframe();
		}
		else if(idValue == 1)
		{
			pView->OnButtonStick();
		}
		else if(idValue == 2)
		{
			pView->OnButtonBall();
		}
		else if(idValue == 3)
		{
			pView->OnButtonBallStick();
		}
		else if(idValue == 5)
		{
			pView->OnButtonRibbon();
		}else if(idValue == 6)
		{
			pView->OnButtonDotsurface ();
		}
		//pView->SendMessage(WM_COMMAND, idCommand[Convert::ToInt32(displayStyle)]);
	}

	//    update.
	UpdateSelections();

	if ( countOld != m_arrayVP->Count )
		return m_arrayVP[m_arrayVP->Count-1];		//    return last one.
	else
		return nullptr;
}

IVP ^ CDotNetProteinInsight::AddVP(IPDB ^ pdb, IProteinInsight::IDisplayStyle displayStyle)
{
	SetSelect(false, false);
	pdb->SetSelect(true, true);
	return AddVP(displayStyle);
}

IVP ^ CDotNetProteinInsight::AddVP(List<IPDB ^> ^ pdbs, IProteinInsight::IDisplayStyle displayStyle)
{
	SetSelect(false, false);
	for each( IPDB ^ pdb in pdbs )
		pdb->SetSelect(true, true);
	return AddVP(displayStyle);
}

IVP ^ CDotNetProteinInsight::AddVP(IModel ^ model, IProteinInsight::IDisplayStyle displayStyle)
{
	SetSelect(false, false);
	model->SetSelect(true, true);
	return AddVP(displayStyle);
}

IVP ^ CDotNetProteinInsight::AddVP(List<IModel ^> ^  models, IProteinInsight::IDisplayStyle displayStyle)
{
	SetSelect(false, false);
	for each( IModel ^ model in models )
		model->SetSelect(true, true);
	return AddVP(displayStyle);
}

IVP ^ CDotNetProteinInsight::AddVP(IChain ^ chain, IProteinInsight::IDisplayStyle displayStyle)
{
	SetSelect(false, false);
	chain->SetSelect(true, true);
	return AddVP(displayStyle);
}

IVP ^ CDotNetProteinInsight::AddVP(List<IChain ^> ^ chains, IProteinInsight::IDisplayStyle displayStyle)
{
	SetSelect(false, false);
	for each( IChain ^ chain in chains )
		chain->SetSelect(true, true);
	return AddVP(displayStyle);
}

IVP ^ CDotNetProteinInsight::AddVP(IResidue ^ residue, IProteinInsight::IDisplayStyle displayStyle)
{
	SetSelect(false, false);
	residue->SetSelect(true, true);
	return AddVP(displayStyle);
}

IVP ^ CDotNetProteinInsight::AddVP(List<IResidue ^> ^ residues, IProteinInsight::IDisplayStyle displayStyle)
{
	SetSelect(false, false);
	for each( IResidue ^ residue in residues )
		residue->SetSelect(true, true);
	return AddVP(displayStyle);
}

IVP ^ CDotNetProteinInsight::AddVP(IAtom ^ atom, IProteinInsight::IDisplayStyle displayStyle)
{
	SetSelect(false, false);
	atom->SetSelect(true, true);
	return AddVP(displayStyle);
}

IVP ^ CDotNetProteinInsight::AddVP(List<IAtom ^> ^ atoms, IProteinInsight::IDisplayStyle displayStyle)
{
	SetSelect(false, false);
	for each ( IAtom ^ atom in atoms )
		atom->SetSelect(true, true);
	return AddVP(displayStyle);
}

void CDotNetProteinInsight::DeleteVP(IVP ^ selection)
{
	CDotNetSelection ^ dotnetSelection = dynamic_cast<CDotNetSelection ^> (selection);
	if ( dotnetSelection == nullptr )
		return;

	//if ( AfxGetMainWnd() )
	//{
	//	CSelectionListPane * pSelectionListPane = ((CMainFrame*)AfxGetMainWnd())->m_pSelectionListPane;
	//	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	//	if ( pSelectionListPane && pProteinVistaRenderer )
	//	{
	//		//    select
	//		long iList = pSelectionListPane->GetListCtrlIndex(dotnetSelection->m_pSelectionDisplay);
	//		if ( iList != -1 )
	//		{
	//			//    select
	//			pSelectionListPane->m_htmlListCtrl->SetSelectedItem(iList);

	//			//    delete
	//			SendMessage( pSelectionListPane->GetSafeHwnd(), WM_COMMAND, ID_TOOLBAR_DELETE, 0 );
	//		}
	//	}
	//}
	UpdateSelections();
}

void CDotNetProteinInsight::VPUnionVP(IVP ^ vp1, IVP ^ vp2)
{
	CDotNetSelection ^ dotnetSelection1 = dynamic_cast<CDotNetSelection ^> (vp1);
	if ( dotnetSelection1 == nullptr )
		return;

	CDotNetSelection ^ dotnetSelection2 = dynamic_cast<CDotNetSelection ^> (vp2);
	if ( dotnetSelection2 == nullptr )
		return;

	/*if ( AfxGetMainWnd() )
	{
		CSelectionListPane * pSelectionListPane = ((CMainFrame*)AfxGetMainWnd())->m_pSelectionListPane;
		if ( pSelectionListPane )
		{
			pSelectionListPane->SelectionOperation(dotnetSelection1->m_pSelectionDisplay, dotnetSelection2->m_pSelectionDisplay, CSelectionListPane::UNION);
		}
	}*/
}

void CDotNetProteinInsight::VPIntersectVP(IVP ^ vp1, IVP ^ vp2)
{
	CDotNetSelection ^ dotnetSelection1 = dynamic_cast<CDotNetSelection ^> (vp1);
	if ( dotnetSelection1 == nullptr )
		return;

	CDotNetSelection ^ dotnetSelection2 = dynamic_cast<CDotNetSelection ^> (vp2);
	if ( dotnetSelection2 == nullptr )
		return;

	/*if ( AfxGetMainWnd() )
	{
		CSelectionListPane * pSelectionListPane = ((CMainFrame*)AfxGetMainWnd())->m_pSelectionListPane;
		if ( pSelectionListPane )
		{
			pSelectionListPane->SelectionOperation(dotnetSelection1->m_pSelectionDisplay, dotnetSelection2->m_pSelectionDisplay, CSelectionListPane::INTERSECT );
		}
	}*/
}

void CDotNetProteinInsight::VPSubtrctVP(IVP ^ vp1, IVP ^ vp2)
{
	CDotNetSelection ^ dotnetSelection1 = dynamic_cast<CDotNetSelection ^> (vp1);
	if ( dotnetSelection1 == nullptr )
		return;

	CDotNetSelection ^ dotnetSelection2 = dynamic_cast<CDotNetSelection ^> (vp2);
	if ( dotnetSelection2 == nullptr )
		return;

	/*if ( AfxGetMainWnd() )
	{
		CSelectionListPane * pSelectionListPane = ((CMainFrame*)AfxGetMainWnd())->m_pSelectionListPane;
		if ( pSelectionListPane )
		{
			pSelectionListPane->SelectionOperation(dotnetSelection1->m_pSelectionDisplay, dotnetSelection2->m_pSelectionDisplay, CSelectionListPane::SUBTRACT );
		}
	}*/
}

void CDotNetProteinInsight::SetSelect(bool select, bool bNeedUpdate)
{
	for each( CDotNetPDB ^ pdb in m_arrayPDB)
	{
		pdb->SetSelect(select, false);
	}
	UpdateSelect();		//	마지막에 1번만 update select를 한다.
}

void CDotNetProteinInsight::SetSelect ( IProteinInsight::ISelectType type )
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer )
	{
		LONG idSelection[] = {
								ID_ATOM_SELECTC,
								ID_ATOM_SELECTN,
								ID_ATOM_SELECTO,
								ID_ATOM_SELECTNA,
								ID_ATOM_SELECTMG,
								ID_ATOM_SELECTP,
								ID_ATOM_SELECTS,
								ID_SELECT_HETATM_WITH_HOH,
								ID_SELECT_HETATM_WITHOUT_HOH,
								ID_SELECT_CA,
								ID_SELECT_BACKBONE,
								ID_SELECT_SIDECHAIN,
								ID_SELECT_HYDROPHILIC,
								ID_SELECT_HYDROPHOBIC,
								ID_SELECT_HELIX,
								ID_SELECT_SHEET 
		};

		pProteinVistaRenderer->SelectSpecificAtoms( idSelection[Convert::ToInt32(type)] );
	}
}

//    몇초동안 쉰다.
void CDotNetProteinInsight::Idle(double millisecond)
{
	DWORD dwOldTime = GetTickCount();

	ForceRenderScene();

	while(1)
	{
		::PumpMessage();
		DWORD dwTimeElapsed = GetTickCount() - dwOldTime;
		if ( dwTimeElapsed > millisecond )
			break;
	}
}

void CDotNetProteinInsight::ViewAll()
{
	CProteinVistaRenderer * pProteinVistaRenderer = ::GetProteinVistaRenderer();
	if ( pProteinVistaRenderer ){ pProteinVistaRenderer->CameraViewAll(); }
}


#endif

