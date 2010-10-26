#include "stdafx.h"
#include "ProteinVista.h"

#include "ProteinVistaView.h"
#include "ProteinVistaRenderer.h"
#include "CustomColorPage.h"

#include "SelectionDisplay.h"
#include "Interface.h"
#include "RenderRibbonSelection.h"
#include "DrawText3D.h"
#include "RenderSurfaceSelection.h"
#include "CoordinateAxis.h"
#include "ClipPlane.h"
#include "PDBRenderer.h"
#include "ProteinSurfaceMSMS.h"
#include "SelectionDisplay.h"
#include "SelectionListPane.h"
#include <algorithm>

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


CSelectionDisplay::CSelectionDisplay()
{ 
	m_iSerial = 0;
	m_iDisplayStylePDB = 0; 
	m_displayStyle = 0; 
	m_bShow = TRUE; 
	m_bSelect = FALSE;
	m_center.x = m_center.y = m_center.z = 0.0f; 
	m_radius = 0.0f; 	
	m_pPDBRenderer = NULL; 
	m_pRenderProperty = NULL; 

	m_iDisplaySelectionList = -1;

	for ( int iAnno = 0 ; iAnno < 3 ; iAnno++ )
		m_pAnnotation[iAnno] = NULL;

	m_pCoordinateAxis = new CCoordinateAxisDisplay;

	m_pClipPlane1 = new CClipPlane;
	m_pClipPlane2 = new CClipPlane;

	m_arrayAtomInst.reserve(1000);

	for ( int i = 0 ; i < 3 ; i++ )
	{
		m_pAnnotation[i] = new CDrawText3D();
	}
}


CSelectionDisplay::~CSelectionDisplay() 
{ 
	DeleteDeviceObjects();
	
	for ( long i = 0 ; i < m_arrayRenderObjectSelection.size() ; i++ )
	{
		delete m_arrayRenderObjectSelection[i];
	}
	m_arrayRenderObjectSelection.clear();

	m_selectionInst.clear(); 
	m_arrayAtomInst.clear();

	if ( m_pRenderProperty ) 
	{ 
		//m_pRenderProperty->m_wndProperty.DestroyWindow(); 
		//delete m_pRenderProperty; 
	}

	for ( int iAnno = 0 ; iAnno < 3 ; iAnno ++ )
		SAFE_DELETE(m_pAnnotation[iAnno] );

	SAFE_DELETE(m_pCoordinateAxis);
	SAFE_DELETE(m_pClipPlane1);
	SAFE_DELETE(m_pClipPlane2);
}

void CSelectionDisplay::InitDisplayStyleProperty(long mode)
{
	m_displayStyle = mode;

	switch (mode)
	{
	case WIREFRAME:
		m_pRenderProperty = new CPropertyWireframe(this);
		m_pRenderProperty->InitProperty();
		break;
	case STICKS:
		m_pRenderProperty = new CPropertyStick(this);
		m_pRenderProperty->InitProperty();
		break;
	case SPACEFILL:
		m_pRenderProperty = new CPropertySpaceFill(this);
		m_pRenderProperty->InitProperty();
		break;
	case BALLANDSTICK:
		m_pRenderProperty = new CPropertyBallStick(this);
		m_pRenderProperty->InitProperty();
		break;
	case RIBBON:
		m_pRenderProperty = new CPropertyRibbon(this);
		m_pRenderProperty->InitProperty();
		break;
	case SURFACE:
		m_pRenderProperty = new CPropertySurface(this);
		m_pRenderProperty->InitProperty();
		break;
    case NGRealistic:
		//TODO
		m_pRenderProperty = new CPropertySurface(this);
		m_pRenderProperty->InitProperty();
		break;
	}
	GetMainActiveView()->RefreshProptery(this,mode);
}

void CSelectionDisplay::SetModelQuality()
{
	//	아래 방법대로 해야 Geometry 가 여러번 생성되지 않는다.
	if ( m_arrayRenderObjectSelection.size() > 0 )
		m_arrayRenderObjectSelection[0]->SetModelQuality();
}

//
//	TODO: 수정. 다른것으로 변경하였을때, common property 가 reset 이 된다.
//	
void CSelectionDisplay::ChangeDisplayMode(long mode)
{
	if ( m_displayStyle == mode )
		return;

	DeleteDeviceObjects();
	for ( long i = 0 ; i < m_arrayRenderObjectSelection.size() ; i++ )
	{
		delete m_arrayRenderObjectSelection[i];
	}
	m_arrayRenderObjectSelection.clear();

	CSelectionListPane * SelectionListPane = GetMainActiveView()->GetSelectPanel();
	 
	SAFE_DELETE(m_pRenderProperty);

	//	렌더링 설정을 한다.
	InitDisplayStyleProperty(mode);
	InitRenderSceneSelection();
	InitDeviceObjects();

	SelectionListPane->OnUpdate();
	SelectionListPane->SelectListItem(this);
}

void CSelectionDisplay::SetSelection(CSTLArraySelectionInst & selection)
{
	m_selectionInst = selection;

	for ( int i = 0 ; i < m_selectionInst.size() ; i++ )
	{
		m_selectionInst[i]->GetChildAtoms(m_arrayAtomInst);
	}
}

void CSelectionDisplay::InitChainColorScheme()
{
	CPropertyCommon	* pPropertyCommon = GetPropertyCommon();

	if ( pPropertyCommon->m_arrayColorScheme[COLOR_SCHEME_CHAIN].size() == 0 )
	{
		//    chain일 경우에 갯수에 따라서 만들어준다.
		//    다른 종류의 chain을 찾는다.
		CStringArray  arrayChainID;

		for ( int i = 0 ; i < m_arrayAtomInst.size(); i++ )
		{
			CString strText;
			strText.Format ( "%c" , m_arrayAtomInst[i]->GetAtom()->m_chainID );

			BOOL bFind = TRUE;
			for ( int j = 0 ; j < arrayChainID.GetSize(); j++ )
			{
				if ( arrayChainID[j] == strText )
				{
					bFind = FALSE;
					break;
				}
			}

			if ( bFind == TRUE )
				arrayChainID.Add(strText);
		}

		//    체인의 갯수에 따라 색을 만들어 넣어둔다.
		int numChain = arrayChainID.GetSize();
		for ( int i = 0 ; i < numChain ; i++ )
		{
			D3DXCOLOR diffuse( CColorSchemeDefault::ConvertHSIToRGB( ( numChain == 1 )? 0.0f : ( (FLOAT)i / (FLOAT)(numChain-1) * 0.83f ) , 1.0f, 0.5f ) );
			D3DXCOLOR ambient = CColorSchemeDefault::GetAmbientColorFromDiffuse(diffuse);
			D3DXCOLOR specular = CColorSchemeDefault::GetSpecularColorFromDiffuse(diffuse);
			CColorRow colorRow ( arrayChainID[i] , diffuse );

			pPropertyCommon->m_arrayColorSchemeDefault[COLOR_SCHEME_CHAIN].push_back(new CColorRow(colorRow));
			pPropertyCommon->m_arrayColorScheme[COLOR_SCHEME_CHAIN].push_back(new CColorRow(colorRow));
		}
	}
}

void CSelectionDisplay::InitCustomColorScheme()
{
	CPropertyCommon	* pPropertyCommon = GetPropertyCommon();

	//	처음 실행하는것임.
	if ( pPropertyCommon->m_arrayColorScheme[COLOR_SCHEME_CUSTOM].size() == 0 )
	{
		pPropertyCommon->m_arrayColorScheme[COLOR_SCHEME_CUSTOM].reserve(m_arrayAtomInst.size());

		for ( int i = 0 ; i < m_arrayAtomInst.size() ; i++ )
		{
			CColorRow * pColorRow = pPropertyCommon->m_arrayColorScheme[COLOR_SCHEME_CPK][m_arrayAtomInst[i]->GetAtom()->m_atomNameIndex];
			CColorRow * pColorRowNew = new CColorRow(*pColorRow);

			CString strName;
			strName.Format( _T("%s-%c-%s-%s [%d]") , m_arrayAtomInst[i]->GetAtom()->m_strPDBID, m_arrayAtomInst[i]->GetAtom()->m_chainID, 
													 m_arrayAtomInst[i]->GetAtom()->m_residueName, m_arrayAtomInst[i]->GetAtom()->m_atomName , m_arrayAtomInst[i]->GetAtom()->m_serial );
			pColorRowNew->m_name = strName;

			pPropertyCommon->m_arrayColorScheme[COLOR_SCHEME_CUSTOM].push_back( pColorRowNew );
			pPropertyCommon->m_arrayColorSchemeDefault[COLOR_SCHEME_CUSTOM].push_back( new CColorRow (*pColorRowNew) );
		}
	}
}

//    현재의 color scheme을 가지고, atom 에 해당되는 컬러를 구한다.
CColorRow * CSelectionDisplay::GetAtomColor(CAtom* pAtom)
{
	if ( pAtom == NULL )
	{
		static CColorRow colorRowTemp;
		return &colorRowTemp;
	}

	int colorScheme = GetPropertyCommon()->m_enumColorScheme;

	switch (colorScheme)
	{
		case COLOR_SCHEME_CPK:
			{
				return GetPropertyCommon()->m_arrayColorScheme[COLOR_SCHEME_CPK][pAtom->m_atomNameIndex];
			}
			break;
		case COLOR_SCHEME_AMINO_ACID:
			{
				return GetPropertyCommon()->m_arrayColorScheme[COLOR_SCHEME_AMINO_ACID][pAtom->m_residueNameIndex];
			}
			break;
		case COLOR_SCHEME_CHAIN:
			{
				InitChainColorScheme();

				CArrayColorRow & arrayColorRow = GetPropertyCommon()->m_arrayColorScheme[COLOR_SCHEME_CHAIN];
				for ( int i = 0 ; i < arrayColorRow.size(); i++ )
				{
					if ( CString(arrayColorRow[i]->m_name) == CString(pAtom->m_chainID) )
					{
						return (arrayColorRow[i]);
					}
				}
			}
			break;
		case COLOR_SCHEME_OCCUPANCY:
			{
				float occupancy = pAtom->m_occupancy;

				D3DXCOLOR diffuse = FindGradientColor( GetPropertyCommon()->m_arrayColorScheme[COLOR_SCHEME_OCCUPANCY], 
														(int)((occupancy-m_rangeOccupancy.x)*1000) , 
														(int)((m_rangeOccupancy.y-m_rangeOccupancy.x)*1000+1 ) );

				D3DXCOLOR ambient = CColorSchemeDefault::GetAmbientColorFromDiffuse(diffuse);
				D3DXCOLOR specular = CColorSchemeDefault::GetSpecularColorFromDiffuse(diffuse);

				static CColorRow colorRow;
				colorRow = CColorRow ( CString("") , diffuse );
				return &colorRow;
			}
			break;
		case COLOR_SCHEME_TEMPARATURE:
			{
				float temparature = pAtom->m_temperature;

				D3DXCOLOR diffuse = FindGradientColor( GetPropertyCommon()->m_arrayColorScheme[COLOR_SCHEME_TEMPARATURE], 
														(int)((temparature-m_rangeTemperature.x)*1000) , 
														(int)((m_rangeTemperature.y-m_rangeTemperature.x)*1000+1 ) );

				D3DXCOLOR ambient = CColorSchemeDefault::GetAmbientColorFromDiffuse(diffuse);
				D3DXCOLOR specular = CColorSchemeDefault::GetSpecularColorFromDiffuse(diffuse);

				static CColorRow colorRow;
				colorRow = CColorRow ( CString("") , diffuse );

				return &colorRow;
			}
			break;
		case COLOR_SCHEME_HYDROPATHY:
			{
				float hydrophathy = pAtom->m_hydropathy;

				D3DXCOLOR diffuse = FindGradientColor( GetPropertyCommon()->m_arrayColorScheme[COLOR_SCHEME_HYDROPATHY], 
														(int)((hydrophathy-m_rangeHydropathy.x)*1000) , 
														(int)((m_rangeHydropathy.y-m_rangeHydropathy.x)*1000+1) );

				D3DXCOLOR ambient = CColorSchemeDefault::GetAmbientColorFromDiffuse(diffuse);
				D3DXCOLOR specular = CColorSchemeDefault::GetSpecularColorFromDiffuse(diffuse);

				static CColorRow colorRow;
				colorRow = CColorRow ( CString("") , diffuse );

				return &colorRow;
			}
			break;
		case COLOR_SCHEME_PROGRESSIVE:
			{
				//    일반적으로 다음것이 연속해서 온다. so, 주변것을 먼저 검색한다.
				static int iFind = 0;
				iFind = max(iFind-10, 0);

				int		arrayLen = m_arrayAtomInst.size();
				for ( int i = 0 ; i < arrayLen ; i++ )
				{
					if ( m_arrayAtomInst[iFind%arrayLen]->GetAtom() == pAtom ) break;
					iFind = (iFind+1)%arrayLen;
				}

				D3DXCOLOR diffuse = FindGradientColor( GetPropertyCommon()->m_arrayColorScheme[COLOR_SCHEME_PROGRESSIVE], 
														iFind , m_arrayAtomInst.size() );
					
				D3DXCOLOR ambient = CColorSchemeDefault::GetAmbientColorFromDiffuse(diffuse);
				D3DXCOLOR specular = CColorSchemeDefault::GetSpecularColorFromDiffuse(diffuse);

				static CColorRow colorRow;
				colorRow = CColorRow ( CString("") , diffuse );

				return &colorRow;
			}
			break;
		case COLOR_SCHEME_SINGLECOLOR:
			{
				return (GetPropertyCommon()->m_arrayColorScheme[COLOR_SCHEME_SINGLECOLOR][0]);
			}
			break;
		case COLOR_SCHEME_CUSTOM:
			{
				InitCustomColorScheme();

				CPropertyCommon	* pPropertyCommon = GetPropertyCommon();

				static int iFind = 0;
				iFind = max(iFind-10, 0);

				int		arrayLen = m_arrayAtomInst.size();
				for ( int i = 0 ; i < arrayLen ; i++ )
				{
					if ( m_arrayAtomInst[iFind%arrayLen]->GetAtom() == pAtom ) break;
					iFind = (iFind+1)%arrayLen;
				}

				return pPropertyCommon->m_arrayColorScheme[COLOR_SCHEME_CUSTOM][iFind];
			}
			break;
	};

	static CColorRow colorRowTemp;
	return &colorRowTemp;
}

void CSelectionDisplay::FindCenterRadius()
{
	CSTLArrayAtomInst & atoms = m_arrayAtomInst;

	if ( atoms.size() == 0 )
		return;

	//
	D3DXVECTOR3 centerSum(0,0,0);
	D3DXVECTOR3 minBB(1e20, 1e20, 1e20);
	D3DXVECTOR3 maxBB(-1e20, -1e20, -1e20);

	m_rangeOccupancy.x = 10000.0f;		m_rangeOccupancy.y = -10000.0f;
	m_rangeTemperature.x = 10000.0f;	m_rangeTemperature.y = -10000.0f;
	m_rangeHydropathy.x = 10000.0f;		m_rangeHydropathy.y = -10000.0f;

	for ( int i = 0 ; i < atoms.size() ; i++ )
	{
		CAtom * pAtom = atoms[i]->GetAtom();
		centerSum += pAtom->m_pos;

		if (maxBB.x < pAtom->m_pos.x) maxBB.x = pAtom->m_pos.x;
		if (maxBB.y < pAtom->m_pos.y) maxBB.y = pAtom->m_pos.y;
		if (maxBB.z < pAtom->m_pos.z) maxBB.z = pAtom->m_pos.z;
		if (minBB.x > pAtom->m_pos.x) minBB.x = pAtom->m_pos.x;
		if (minBB.y > pAtom->m_pos.y) minBB.y = pAtom->m_pos.y;
		if (minBB.z > pAtom->m_pos.z) minBB.z = pAtom->m_pos.z;

		if ( pAtom->m_occupancy < m_rangeOccupancy.x )		m_rangeOccupancy.x = pAtom->m_occupancy;
		if ( pAtom->m_occupancy > m_rangeOccupancy.y )		m_rangeOccupancy.y = pAtom->m_occupancy;
		if ( pAtom->m_temperature < m_rangeTemperature.x )	m_rangeTemperature.x = pAtom->m_temperature;
		if ( pAtom->m_temperature > m_rangeTemperature.y )	m_rangeTemperature.y = pAtom->m_temperature;
		if ( pAtom->m_hydropathy < m_rangeHydropathy.x )	m_rangeHydropathy.x = pAtom->m_hydropathy;
		if ( pAtom->m_hydropathy > m_rangeHydropathy.y )	m_rangeHydropathy.y = pAtom->m_hydropathy;
	}

	//	m_center는 무게중심이 아니라. bb 의 중심이다.
	//	m_center = centerSum/atoms.size();
	m_center = (minBB + maxBB)/2.0f;
	m_radius = D3DXVec3Length( &D3DXVECTOR3(m_center-minBB) ) + 4.0f;

	// 	m_BBPos[0].x = minBB.x;	m_BBPos[0].y = minBB.y;	m_BBPos[0].z = minBB.z;
	// 	m_BBPos[1].x = maxBB.x;	m_BBPos[1].y = minBB.y;	m_BBPos[1].z = minBB.z;
	// 	m_BBPos[2].x = minBB.x;	m_BBPos[2].y = maxBB.y;	m_BBPos[2].z = minBB.z;
	// 	m_BBPos[3].x = maxBB.x;	m_BBPos[3].y = maxBB.y;	m_BBPos[3].z = minBB.z;
	// 	m_BBPos[4].x = minBB.x;	m_BBPos[4].y = minBB.y;	m_BBPos[4].z = maxBB.z;
	// 	m_BBPos[5].x = maxBB.x;	m_BBPos[5].y = minBB.y;	m_BBPos[5].z = maxBB.z;
	// 	m_BBPos[6].x = minBB.x;	m_BBPos[6].y = maxBB.y;	m_BBPos[6].z = maxBB.z;
	// 	m_BBPos[7].x = maxBB.x;	m_BBPos[7].y = maxBB.y;	m_BBPos[7].z = maxBB.z;

	m_minMaxBB[0] = minBB;
	m_minMaxBB[1] = maxBB;
}

HRESULT CSelectionDisplay::InitDeviceObjects()
{
	DeleteDeviceObjects();

	CPropertyCommon		* pCommon = GetPropertyCommon();

	m_pClipPlane1->Init(m_pProteinVistaRenderer, this, m_radius);
	m_pClipPlane2->Init(m_pProteinVistaRenderer, this, m_radius);

	m_pClipPlane1->InitDeviceObjects();
	m_pClipPlane2->InitDeviceObjects();

	//	InitDeviceObjects 에 indicateSelect 코드를 넣어야 한다.
	if ( GetPropertyCommon()->m_bIndicate == TRUE )
	{	
		//	이미 할당되어 있지 않다면,
		int slot = GetPropertyCommon()->m_indicateColorSlot;
		if ( slot == -1 )
		{
			//	indicate가 설정되면 indicate용 빈 slot을 하나 받는다.
			slot = m_pProteinVistaRenderer->GetIndicateColorSlot();
			if ( slot > 0 )
			{	//	
				GetPropertyCommon()->m_indicateColorSlot = slot;
			}
			else
			{
				GetPropertyCommon()->m_indicateColorSlot = -1;
				GetPropertyCommon()->m_bIndicate = FALSE;
			}
		}

		if ( slot != -1 )
		{
			m_pProteinVistaRenderer->SetIndicateColorSlot(slot, COLORREF2D3DXCOLOR(GetPropertyCommon()->m_indicateColor) );
		}
	}

	long iProgress =GetMainActiveView()->InitProgress(100);
	for ( long i = 0 ; i < m_arrayRenderObjectSelection.size() ; i++ )
	{
		GetMainActiveView()->SetProgress((i+1)*100/m_arrayRenderObjectSelection.size(), iProgress);

		m_arrayRenderObjectSelection[i]->InitDeviceObjects();
	}
	GetMainActiveView()->EndProgress(iProgress);

	if ( m_pCoordinateAxis )
	{
		m_pCoordinateAxis->Init(m_pProteinVistaRenderer);
		m_pCoordinateAxis->InitDeviceObjects ();
	}

	for ( int iAnno = 0 ; iAnno < 3 ; iAnno++ )
	{
		if ( m_pAnnotation[iAnno] )	
		{
			m_pAnnotation[iAnno]->Init(m_pProteinVistaRenderer);
			SetAnnotationInfo(iAnno);
			m_pAnnotation[iAnno]->InitDeviceObjects();
		}
	}

	return S_OK;
}

HRESULT CSelectionDisplay::RestoreDeviceObjects()
{
	if ( m_pCoordinateAxis )
		m_pCoordinateAxis->RestoreDeviceObjects();

	for ( int iAnno = 0 ; iAnno < 3 ; iAnno++ )
		if ( m_pAnnotation[iAnno] )
			m_pAnnotation[iAnno]->RestoreDeviceObjects();

	return S_OK;
}

HRESULT CSelectionDisplay::InvalidateDeviceObjects()
{
	if ( m_pCoordinateAxis )
		m_pCoordinateAxis->InvalidateDeviceObjects();

	for ( int iAnno = 0 ; iAnno < 3 ; iAnno++ )
		if ( m_pAnnotation[iAnno] )
			m_pAnnotation[iAnno]->InvalidateDeviceObjects();

	return S_OK;
}

HRESULT CSelectionDisplay::DeleteDeviceObjects()
{
	//	m_indicateColorSlot 을 해지.
 
	m_pProteinVistaRenderer->DeleteIndicateColorSlot(GetPropertyCommon()->m_indicateColorSlot);
	GetPropertyCommon()->m_indicateColorSlot = -1;

	m_pClipPlane1->DeleteDeviceObjects();
	m_pClipPlane2->DeleteDeviceObjects();

	for ( long i = 0 ; i < m_arrayRenderObjectSelection.size() ; i++ )
	{
		m_arrayRenderObjectSelection[i]->DeleteDeviceObjects();
	}

	if ( m_pCoordinateAxis )
		m_pCoordinateAxis->DeleteDeviceObjects();

	for ( int iAnno = 0 ; iAnno < 3 ; iAnno++ )
		if ( m_pAnnotation[iAnno] )	
			m_pAnnotation[iAnno]->DeleteDeviceObjects();

	return S_OK;
}

HRESULT	CSelectionDisplay::FrameMove()
{
	if ( m_pClipPlane1 ) m_pClipPlane1->FrameMove();
	if ( m_pClipPlane2 ) m_pClipPlane2->FrameMove();

	return S_OK;
}
#pragma managed(push,off)
HRESULT CSelectionDisplay::Render()
{ 
	//	shader 에 클리핑 정보 넘기기.
	SetClipPlaneEquationToShader();

	for ( int i = 0 ; i < m_arrayRenderObjectSelection.size() ; i++ )
	{
		m_arrayRenderObjectSelection[i]->Render();
	}

	//	clip plane을 그린다.
	//	이것도 투명한것을 뒤로 미룬다.
	CPropertyCommon * property = GetPropertyCommon();
	if ( property != NULL )
	{
		//	
		m_pProteinVistaRenderer->m_pd3dDevice->SetRenderState( D3DRS_CULLMODE , D3DCULL_NONE );

		if ( property->m_bClipping1 == TRUE )
		{
			m_pClipPlane1->InitRenderParam(property->m_bShowClipPlane1, property->m_clipPlaneTransparency1, COLORREF2D3DXCOLOR(property->m_clipPlaneColor1) );

			if ( property->m_clipPlaneTransparency1 != 100 )
				m_pProteinVistaRenderer->m_transparentRenderObject.push_back(m_pClipPlane1);
			else
				m_pClipPlane1->Render();
		}

		if ( property->m_bClipping2 == TRUE )
		{
			m_pClipPlane2->InitRenderParam(property->m_bShowClipPlane2, property->m_clipPlaneTransparency2, COLORREF2D3DXCOLOR(property->m_clipPlaneColor2) );

			if ( property->m_clipPlaneTransparency2 != 100 )
				m_pProteinVistaRenderer->m_transparentRenderObject.push_back(m_pClipPlane2);
			else
				m_pClipPlane2->Render();
		}
	}

	if ( m_pCoordinateAxis && m_bSelect == TRUE )
	{
		if ( property->m_bDisplayAxisLocalCoord == TRUE )		
		{
			m_pCoordinateAxis->SetModelTransform(m_pPDBRenderer->m_selectionCenterTransformed, m_pPDBRenderer->m_matWorld);

			FLOAT scale = property->m_axisScaleLocalCoord/50.0f;
			if ( scale > 1.0f )		scale = scale*scale*scale;

			D3DXMATRIXA16 matScale;
			D3DXMatrixScaling(&matScale, scale, scale, scale);
			m_pCoordinateAxis->SetModelScale(matScale);

			m_pCoordinateAxis->Render();
		}
	}

	//	annotation Display
	//	alpha값이 있어서 뒤로 뺀다.
	//	RenderAnnotation();

	return S_OK;
}
#pragma managed(pop)
//	
HRESULT CSelectionDisplay::UpdateAtomSelectionChanged()
{ 
	for ( int i = 0 ; i < m_arrayRenderObjectSelection.size() ; i++ )
	{
		dynamic_cast<CRenderObjectSelection*>(m_arrayRenderObjectSelection[i])->UpdateAtomSelectionChanged();
	}
	GetMainActiveView()->OnPaint();
	return S_OK;
}

HRESULT CSelectionDisplay::UpdateAtomPosColorChanged()
{
	for ( int i = 0 ; i < m_arrayRenderObjectSelection.size() ; i++ )
	{
		dynamic_cast<CRenderObjectSelection*>(m_arrayRenderObjectSelection[i])->UpdateAtomPosColorChanged();
	}

	return S_OK;
}

HRESULT CSelectionDisplay::UpdateSurfaceCurvatureChanged()
{

	return S_OK;
}

//	shader 에 클리핑 정보 넘기기.
void CSelectionDisplay::SetClipPlaneEquationToShader()
{
	CPropertyCommon * property = GetPropertyCommon();
	if ( property != NULL )
	{
		if ( property->m_bClipping1 == TRUE )
		{
			float dir1;
			if ( property->m_bClipDirection1 == TRUE )
				dir1 = 100.0f;
			else
				dir1 = -100.0f;
			m_pProteinVistaRenderer->SetShaderClipPlane1Dir(dir1);
			m_pClipPlane1->GetPlaneEquation(&(property->m_clipPlaneEquation1));
			m_pProteinVistaRenderer->SetShaderClipPlane1(property->m_clipPlaneEquation1);
		}
		else
		{
			m_pProteinVistaRenderer->SetShaderClipPlane1Dir(1.0f);

			D3DXPLANE plane(0,0,0,1);
			m_pProteinVistaRenderer->SetShaderClipPlane1(plane);
		}

		if ( property->m_bClipping2 == TRUE )
		{
			float dir2;
			if ( property->m_bClipDirection2 == TRUE )
				dir2 = 100.0f;
			else
				dir2 = -100.0f;
			m_pProteinVistaRenderer->SetShaderClipPlane2Dir(dir2);
			m_pClipPlane2->GetPlaneEquation(&(property->m_clipPlaneEquation2));
			m_pProteinVistaRenderer->SetShaderClipPlane2(property->m_clipPlaneEquation2);
		}
		else
		{
			m_pProteinVistaRenderer->SetShaderClipPlane2Dir(1.0f);

			D3DXPLANE plane(0,0,0,1);
			m_pProteinVistaRenderer->SetShaderClipPlane2(plane);
		}

		BOOL bClipExist = (property->m_bClipping2)|(property->m_bClipping1)|(m_pProteinVistaRenderer->m_pPropertyScene->m_bClipping0);
	}
}

void CSelectionDisplay::OnColorSelectionDialogBox(CArrayColorRow & colorRow, CArrayColorRow & colorRowDefault, BOOL bGradient)
{
	CPropertyCommon	*pCommonProperty = GetPropertyCommon();

	/*CColorSelectionDialog colorSelectionDialog;

	colorSelectionDialog.m_bGradientColor = bGradient;
	m_pProteinVistaRenderer->m_colorSchemeDefault.CopyArrayColorRow(colorSelectionDialog.m_arrayColorRow, colorRow);
	m_pProteinVistaRenderer->m_colorSchemeDefault.CopyArrayColorRow(colorSelectionDialog.m_arrayColorRowDefault, colorRowDefault);

	int nRet = colorSelectionDialog.DoModal();
	DWORD errCode = GetLastError();
	if ( nRet == IDOK )
	{
		for ( int i = 0 ; i < colorRow.size(); i++ )
			SAFE_DELETE(colorRow[i]);
		colorRow.clear();
		m_pProteinVistaRenderer->m_colorSchemeDefault.CopyArrayColorRow(colorRow, colorSelectionDialog.m_arrayColorRow);
	}*/
}

void CSelectionDisplay::OnColorSelectionDialogBox(int iScheme, BOOL bGradient)
{
	CPropertyCommon	*pCommonProperty = GetPropertyCommon();

	CArrayColorRow & colorRow = pCommonProperty->m_arrayColorScheme[iScheme];
	CArrayColorRow & colorRowDefault = pCommonProperty->m_arrayColorSchemeDefault[iScheme];

	OnColorSelectionDialogBox(colorRow, colorRowDefault, bGradient);
}


//	property event handler.
void CSelectionDisplay::SetPropertyChanged(long id,CString pValue)
{
	switch( id )
	{
		case PROPERTY_COMMON_SHOW:
			{
				//    show/hide.
				long iListCtrl =GetMainActiveView()->GetSelectPanel()->GetListCtrlIndex(this);
				if ( iListCtrl != -1 )
				{
					GetMainActiveView()->GetSelectPanel()->m_htmlListCtrl->SetItemCheck(iListCtrl, m_bShow);
				}
			}
			break;

		case PROPERTY_COMMON_DISPLAY_MODE:
			{
				//    여기서 바로 ChangeDisplayMode를 하면 안된다.
				//    Focus 가 있는 상태에서 propertyWindow가 바뀌어 에러가 난다.
				CPropertyCommon	*pCommonProperty = GetPropertyCommon();
				long mode = pCommonProperty->m_enumDisplayMode;
				 
				GetMainActiveView()->OnChangeDisplayMode((WPARAM)mode, (LPARAM)this);
				//this->ChangeDisplayMode(mode);
			}
			break;

		case PROPERTY_COMMON_SELECTION_NAME:
			{
				//    update selection list panel.
				GetMainActiveView()->GetSelectPanel()->OnUpdate();
				GetMainActiveView()->GetSelectPanel()->SelectListItem(m_iDisplaySelectionList);
			}
			break;

		case PROPERTY_COMMON_DISPLAY_HETATM:
			//	AfxMessageBox("Scene Rendering 에 Display HETATM 옵션이 있습니다. Selection 에서의 개별 HETATM display 는 아직 지원하지 않습니다");
			break;

		case PROPERTY_COMMON_DISPLAY_SIDECHAIN:
			{
				//	stick, ball stick, spaceFill 일때만 적용.
				if ( m_displayStyle == STICKS || m_displayStyle == SPACEFILL || m_displayStyle == BALLANDSTICK )
				{
					UpdateAtomPosColorChanged();
				}
			}
			break;

		case PROPERTY_COMMON_COLOR_SCHEME:
			{
				//	atom color를 설정.
				UpdateAtomPosColorChanged();

				//	color scheme이 바뀌면, annotation의 text color도 바뀌어야 한다.
				UpdateAnnotation();
			}
			break;

		case PROPERTY_COMMON_COLOR_SCHEME_CUSTOMIZE:
			{
				CPropertyCommon	*pCommonProperty = GetPropertyCommon();
				long colorScheme = pCommonProperty->m_enumColorScheme;

				//	define은 colorScheme.h 에 들어있음.
				switch(colorScheme)
				{
					case COLOR_SCHEME_CPK:
					case COLOR_SCHEME_AMINO_ACID:
					case COLOR_SCHEME_SINGLECOLOR:
					case COLOR_SCHEME_CHAIN:
						OnColorSelectionDialogBox(colorScheme);
						break;

					case COLOR_SCHEME_CUSTOM:
						{
							CPropertyCommon	* pPropertyCommon = GetPropertyCommon();

							//	처음 실행하는것임.
							if ( pPropertyCommon->m_arrayColorScheme[COLOR_SCHEME_CUSTOM].size() == 0 )
							{
								pPropertyCommon->m_arrayColorScheme[COLOR_SCHEME_CUSTOM].reserve(m_arrayAtomInst.size());

								for ( int i = 0 ; i < m_arrayAtomInst.size() ; i++ )
								{
									CColorRow * pColorRow = pPropertyCommon->m_arrayColorScheme[COLOR_SCHEME_CPK][m_arrayAtomInst[i]->GetAtom()->m_atomNameIndex];
									pPropertyCommon->m_arrayColorScheme[COLOR_SCHEME_CUSTOM].push_back( new CColorRow(*pColorRow) );
								}
							}

							OnColorSelectionDialogBox(colorScheme);
						}
						break;

					case COLOR_SCHEME_OCCUPANCY:
					case COLOR_SCHEME_TEMPARATURE:
					case COLOR_SCHEME_PROGRESSIVE:
					case COLOR_SCHEME_HYDROPATHY:
						OnColorSelectionDialogBox(colorScheme, TRUE);
						break;
				}

				UpdateAtomPosColorChanged();

				//	color scheme이 바뀌면, annotation의 text color도 바뀌어야 한다.
				UpdateAnnotation();
			}
			break;

		case PROPERTY_COMMON_MODEL_QUALITY:
			SetModelQuality();
			break;

		case PROPERTY_COMMON_SELECTION_SHOW:
		case PROPERTY_COMMON_SINGLE_COLOR:
			{
				UpdateAtomPosColorChanged();
			}
			break;

		case PROPERTY_COMMON_INDICATE_SELECTION:
			{	
				if ( GetPropertyCommon()->m_bIndicate == TRUE )
				{	
					int slot = GetPropertyCommon()->m_indicateColorSlot;

					//	이미 할당되어 있지 않다면,
					if ( slot == -1 )
					{
						//	indicate가 설정되면 indicate용 빈 slot을 하나 받는다.
						slot = m_pProteinVistaRenderer->GetIndicateColorSlot();
						if ( slot > 0 )
						{	//	
							GetPropertyCommon()->m_indicateColorSlot = slot;
						}
						else
						{
							GetPropertyCommon()->m_indicateColorSlot = -1;
							GetPropertyCommon()->m_bIndicate = FALSE;

							/*CXTPPropertyGridItemBool * itemBool = dynamic_cast<CXTPPropertyGridItemBool *>(pItem);
							if ( itemBool )
							{
								itemBool->SetBool(FALSE);
							}*/
						}
					}

					if ( slot != -1 )
						m_pProteinVistaRenderer->SetIndicateColorSlot(slot, COLORREF2D3DXCOLOR(GetPropertyCommon()->m_indicateColor) );
				}
				else
				{
					m_pProteinVistaRenderer->DeleteIndicateColorSlot(GetPropertyCommon()->m_indicateColorSlot);
					GetPropertyCommon()->m_indicateColorSlot = -1;
				}

				UpdateAtomSelectionChanged();
			}
			break;

		case PROPERTY_COMMON_INDICATE_SELECTION_COLOR:
			{
				int slot = GetPropertyCommon()->m_indicateColorSlot;
				if ( slot != -1 )
					m_pProteinVistaRenderer->SetIndicateColorSlot(slot, COLORREF2D3DXCOLOR(GetPropertyCommon()->m_indicateColor) );

				UpdateAtomSelectionChanged();
			}
			break;

		case PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_SHOW:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_FONT:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_DESCRIPTION:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_COLOR_SCHEME:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_COLOR:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_POS_X:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_POS_Y:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_POS_Z:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_TEXT_POS_X:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_TEXT_POS_Y:
			{
				if ( m_pAnnotation[ANNOTATION_VP] )	
				{
					if ( id == PROPERTY_COMMON_ANNOTATION_DISPLAY_VP_FONT )
					{
						//CXTPPropertyGridItemFont * pItemFont = dynamic_cast<CXTPPropertyGridItemFont *>(pItem);
						//if ( pItemFont )
						//	pItemFont->GetFont(&(GetPropertyCommon()->m_logFont[ANNOTATION_VP]));

						//if ( CString(GetPropertyCommon()->m_logFont[ANNOTATION_VP].lfFaceName) == _T("") )
						//{	//	font face 가 "" 이 되는 경우가 있다.
						//	//	keboard 입력.
						//	CString	fontFace(_T("Arial"));
						//	_tcscpy( GetPropertyCommon()->m_logFont[ANNOTATION_VP].lfFaceName, fontFace );
						//	pItemFont->SetValue(fontFace);
						//}
					}

					m_pAnnotation[ANNOTATION_VP]->DeleteDeviceObjects();
					m_pAnnotation[ANNOTATION_VP]->Init(m_pProteinVistaRenderer);
					SetAnnotationInfo(ANNOTATION_VP);
					m_pAnnotation[ANNOTATION_VP]->InitDeviceObjects();
				}
			}
			break;

		case PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_SHOW:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_FONT:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_DESCRIPTION:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_COLOR_SCHEME:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_TYPE:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_COLOR:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_POS_X:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_POS_Y:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_POS_Z:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_TEXT_POS_X:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_TEXT_POS_Y:
			{
				if ( m_pAnnotation[ANNOTATION_ATOM] )	
				{
					if ( id == PROPERTY_COMMON_ANNOTATION_DISPLAY_ATOM_FONT )
					{
						//CXTPPropertyGridItemFont * pItemFont = dynamic_cast<CXTPPropertyGridItemFont *>(pItem);
						//if ( pItemFont )
						//	pItemFont->GetFont(&(GetPropertyCommon()->m_logFont[ANNOTATION_ATOM]));

						//if ( CString(GetPropertyCommon()->m_logFont[ANNOTATION_ATOM].lfFaceName) == _T("") )
						//{	//	font face 가 "" 이 되는 경우가 있다.
						//	//	keboard 입력.
						//	CString	fontFace(_T("Arial"));
						//	_tcscpy( GetPropertyCommon()->m_logFont[ANNOTATION_ATOM].lfFaceName, fontFace );
						//	pItemFont->SetValue(fontFace);
						//}
					}

					m_pAnnotation[ANNOTATION_ATOM]->DeleteDeviceObjects();
					m_pAnnotation[ANNOTATION_ATOM]->Init(m_pProteinVistaRenderer);
					SetAnnotationInfo(ANNOTATION_ATOM);
					m_pAnnotation[ANNOTATION_ATOM]->InitDeviceObjects();
				}
			}
			break;

		case PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_SHOW:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_FONT:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_COLOR_SCHEME:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_TYPE:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_COLOR:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_POS_X:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_POS_Y:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_POS_Z:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_TEXT_POS_X:
		case PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_TEXT_POS_Y:
			{
				if ( m_pAnnotation[ANNOTATION_ATOM] )	
				{
					if ( id == PROPERTY_COMMON_ANNOTATION_DISPLAY_RESIDUE_FONT )
					{
						//CXTPPropertyGridItemFont * pItemFont = dynamic_cast<CXTPPropertyGridItemFont *>(pItem);
						//if ( pItemFont )
						//	pItemFont->GetFont(&(GetPropertyCommon()->m_logFont[ANNOTATION_RESIDUE]));

						//if ( CString(GetPropertyCommon()->m_logFont[ANNOTATION_RESIDUE].lfFaceName) == _T("") )
						//{	//	font face 가 "" 이 되는 경우가 있다.
						//	//	keboard 입력.
						//	CString	fontFace(_T("Arial"));
						//	_tcscpy( GetPropertyCommon()->m_logFont[ANNOTATION_RESIDUE].lfFaceName, fontFace );
						//	pItemFont->SetValue(fontFace);
						//}
					}
					m_pAnnotation[ANNOTATION_RESIDUE]->DeleteDeviceObjects();
					m_pAnnotation[ANNOTATION_RESIDUE]->Init(m_pProteinVistaRenderer);
					SetAnnotationInfo(ANNOTATION_RESIDUE);
					m_pAnnotation[ANNOTATION_RESIDUE]->InitDeviceObjects();
				}
			}
			break;

		case PROPERTY_COMMON_SELECTION_COLOR:
			{
				UpdateAtomSelectionChanged();
			}
			break;

		case PROPERTY_WIREFRAME_DISPLAY_METHOD:
			{
				UpdateAtomSelectionChanged();
			}
			break;

		case PROPERTY_WIREFRAME_LINE_WIDTH:
			{
		 
				if ( ( GetPropertyWireframe()->m_lineWidthOld == 1  ) ||
					 ( GetPropertyWireframe()->m_lineWidthOld != 1  ) )
				{
					UpdateAtomSelectionChanged();	
				}
				/*CCustomItemSlider * pSlider = dynamic_cast<CCustomItemSlider * >(pItem);
				if ( pSlider != NULL )
				{
					long numWidth = pSlider->GetNumber();
					if ( ( GetPropertyWireframe()->m_lineWidthOld == 1 && numWidth != 1 ) ||
						 ( GetPropertyWireframe()->m_lineWidthOld != 1 && numWidth == 1 ) )
					{
						UpdateAtomSelectionChanged();	
					}
					GetPropertyWireframe()->m_lineWidthOld  = numWidth;
				}*/
			}
			break;

		case PROPERTY_BALL_STICK_SPHERE_RESOLUTION:
		case PROPERTY_BALL_STICK_CYLINDER_RESOLUTION:
		case PROPERTY_BALL_STICK_SPHERE_RADIUS:
		case PROPERTY_BALL_STICK_CYLINDER_RADIUS:
			{
				UpdateAtomSelectionChanged();
			}
			break;

		case PROPERTY_STICK_SPHERE_RESOLUTION:
		case PROPERTY_STICK_CYLINDER_RESOLUTION:
		case PROPERTY_STICK_STICK_SIZE:
			{
				UpdateAtomSelectionChanged();
			}
			break;

		case PROPERTY_SPACEFILL_SPHERE_RESOLUTION:
			{
				UpdateAtomSelectionChanged();
			}
			break;

		case PROPERTY_SURFACE_TRANSPARENCY:
			{
				//	automatically
			}
			break;

		case PROPERTY_SURFACE_QUALITY:
		case PROPERTY_SURFACE_TRANSPARENCY_SURFACE_PROBE_SPHERE:
		case PROPERTY_SURFACE_INCLUDE_HETATM:
		case PROPERTY_SURFACE_GEN_METHOD:
			{
				CPropertySurface *pSurfaceProperty = GetPropertySurface();
				if ( id == PROPERTY_SURFACE_TRANSPARENCY_SURFACE_PROBE_SPHERE )
				{
					double probeSphere  = pSurfaceProperty->m_probeSphere;
					if ( probeSphere < 0.5 ) probeSphere = 0.5;
					if ( probeSphere > 10 ) probeSphere = 10.0;

					//dynamic_cast<CXTPPropertyGridItemDouble*>(pItem)->SetDouble(probeSphere);
				}

				DeleteDeviceObjects();
				InitRenderSceneSelection();
				InitDeviceObjects();
				g_bRequestRender = TRUE;
			}
			break;

		case PROPERTY_SURFACE_SELECT_ATOM_SURFACE:
			{
				m_pProteinVistaRenderer->SelectAll(FALSE);

				SelectSurfaceAtom();

				m_pProteinVistaRenderer->UpdateSelectionInfoPane();
				//	렌더러에 반영
				m_pProteinVistaRenderer->UpdateAtomSelectionChanged();
				g_bRequestRender = TRUE;
			}
			break;

		case PROPERTY_SURFACE_BLURRING:
			{
				//	새롭게 만든다.
				UpdateAtomPosColorChanged();
			}
			break;

			//    
		case PROPERTY_SURFACE_SET_CURVATURE_COLOR:
			{
				CPropertySurface *pSurfaceProperty = GetPropertySurface();
				OnColorSelectionDialogBox(pSurfaceProperty->m_arrayColorRowCurvature, pSurfaceProperty->m_arrayColorRowCurvatureDefault, TRUE );
				UpdateAtomPosColorChanged();
			}
			break;

		case PROPERTY_SURFACE_SHOW_CURVATURE:
			{
				UpdateAtomPosColorChanged();
			}
			break;
		case PROPERTY_SURFACE_CURVATURE_RINGSIZE:
			{
				UpdateAtomPosColorChanged();
			}
			break;

		//case PROPERTY_COMMON_DEPTH_OF_FIELD:
		//case PROPERTY_COMMON_FOG_COLOR:
		//case PROPERTY_COMMON_FOG_START:
		//case PROPERTY_COMMON_FOG_END:
		//	{
		//		SetFog();
		//	}
		//	break;

		case PROPERTY_RIBBON_CURVE_TENSION:
		case PROPERTY_RIBBON_RESOLUTION:
			{
				DeleteDeviceObjects();
				InitRenderSceneSelection();
				InitDeviceObjects();
			}
			break;

		case PROPERTY_RIBBON_COIL_SIZE:
		case PROPERTY_RIBBON_HELIX_SIZE:
		case PROPERTY_RIBBON_SHEET_SIZE:
		case PROPERTY_RIBBON_HELIX_FIT_METHOD:
		case PROPERTY_RIBBON_HELIX_SHAPE:
		case PROPERTY_RIBBON_SHEET_SHAPE:
		case PROPERTY_RIBBON_COIL_SHAPE:
		case PROPERTY_RIBBON_HELIX_CURVE_TENSION:
		case PROPERTY_RIBBON_SHEET_CURVE_TENSION:
		case PROPERTY_RIBBON_COIL_CURVE_TENSION:
		case PROPERTY_RIBBON_HELIX_RESOLUTION:
		case PROPERTY_RIBBON_SHEET_RESOLUTION:
		case PROPERTY_RIBBON_COIL_RESOLUTION:
		case PROPERTY_RIBBON_COIL_SHOW_ON_SHEET:
		case PROPERTY_RIBBON_COIL_SHOW_ON_HELIX:
		case PROPERTY_RIBBON_HELIX_TEXTURE_COORD_U:
		case PROPERTY_RIBBON_HELIX_TEXTURE_COORD_V:
		case PROPERTY_RIBBON_SHEET_TEXTURE_COORD_U:
		case PROPERTY_RIBBON_SHEET_TEXTURE_COORD_V:
		case PROPERTY_RIBBON_COIL_TEXTURE_COORD_U:
		case PROPERTY_RIBBON_COIL_TEXTURE_COORD_V:
		case PROPERTY_RIBBON_SHEET_COLOR:
		case PROPERTY_RIBBON_HELIX_COLOR:
		case PROPERTY_RIBBON_COIL_COLOR:
			{
				//	새롭게 만든다.
				UpdateAtomPosColorChanged();
			}
			break;
			
		case PROPERTY_RIBBON_PRESET_DNA_DISPLAY:
			{
				 
			}
			break;

		case PROPERTY_RIBBON_DNA_SELECT_INNER_ATOM:
		case PROPERTY_RIBBON_DNA_SELECT_SUGAR:
		case PROPERTY_RIBBON_DNA_SELECT_BASE:
			{
				if ( m_arrayAtomInst.size() == 0 )	break;
				if ( m_arrayAtomInst[0]->m_pChainInst == NULL ) break;
				if ( m_arrayAtomInst[0]->m_pChainInst->GetChain()->m_bDNA == FALSE ) break;

				m_pProteinVistaRenderer->SelectAll(FALSE);

				for ( int i = 0 ; i < m_arrayAtomInst.size() ; i++ )
				{
					CAtomInst * pAtomInst = m_arrayAtomInst[i];
					if ( pAtomInst->GetAtom()->m_bHETATM == TRUE ) continue;
					if ( pAtomInst->m_pResidueInst->GetResidue()->m_bDNA != TRUE ) continue;

					CString residueName = pAtomInst->GetAtom()->m_residueName;
					residueName.Trim(_T(" "));
					TCHAR cDNA = residueName[0];
					if ( cDNA == _T('A') || cDNA == _T('G') || cDNA == _T('T') || cDNA == _T('C') || cDNA == _T('U') ||
						residueName == _T("DA") || residueName == _T("DG") || residueName == _T("DT") || residueName == _T("DC") || residueName == _T("DU") )
					{
						// P, OP1, OP2, O5'
						if ( id == PROPERTY_RIBBON_DNA_SELECT_INNER_ATOM )
						{
							if (	pAtomInst->GetAtom()->m_atomName != _T(" P  ") && 
									pAtomInst->GetAtom()->m_atomName != _T(" OP1") && pAtomInst->GetAtom()->m_atomName != _T(" OP2") && 
									pAtomInst->GetAtom()->m_atomName != _T(" O1P") && pAtomInst->GetAtom()->m_atomName != _T(" O2P") && 
									pAtomInst->GetAtom()->m_atomName != _T(" O5'") && pAtomInst->GetAtom()->m_atomName != _T(" O5*") )
							{
								m_arrayAtomInst[i]->SetSelect(TRUE);
							}
						}
						else
						{
							if (	pAtomInst->GetAtom()->m_atomName[3] == _T('\'') || pAtomInst->GetAtom()->m_atomName[3] == _T('*') || 
									pAtomInst->GetAtom()->m_atomName == _T(" P  ") || pAtomInst->GetAtom()->m_atomName == _T(" OP1") || pAtomInst->GetAtom()->m_atomName == _T(" OP2") )
							{
								//	sugar
								if ( id == PROPERTY_RIBBON_DNA_SELECT_SUGAR )
									m_arrayAtomInst[i]->SetSelect(TRUE);
							}
							else
							{
								//	base
								if ( id == PROPERTY_RIBBON_DNA_SELECT_BASE )
									m_arrayAtomInst[i]->SetSelect(TRUE);
							}
						}
					}
				}

				m_pProteinVistaRenderer->UpdateSelectionInfoPane();

				//	렌더러에 반영
				m_pProteinVistaRenderer->UpdateAtomSelectionChanged();

				g_bRequestRender = TRUE;
			}
			break;

		case PROPERTY_RIBBON_HELIX_TEXTURE_FILENAME:
		case PROPERTY_RIBBON_Sheet_TEXTURE_FILENAME:
		case PROPERTY_RIBBON_COIL_TEXTURE_FILENAME:
			{	
				for ( int i = 0 ; i < m_arrayRenderObjectSelection.size() ; i++ )
				{
					dynamic_cast<CRenderObjectSelection*>(m_arrayRenderObjectSelection[i])->ResetTexture();
				}
			}
			break;

		case PROPERTY_COMMON_PLANE1_EQUATION:
		case PROPERTY_COMMON_PLANE2_EQUATION:
			{
				CPropertyCommon * pCommonProperty = GetPropertyCommon();
				if ( pCommonProperty )
				{
					CString strPlaneEqu = pValue;

					CString resToken;
					int curPos = 0;

					CSTLFLOATArray	PlaneEqu;

					resToken= strPlaneEqu.Tokenize(_T(","),curPos);
					while (resToken != _T(""))
					{
						PlaneEqu.push_back(atof(resToken));
						resToken = strPlaneEqu.Tokenize(_T(","), curPos);
					};   

					//    m_clipPlaneEquation1
					if ( PlaneEqu.size() == 4 )
					{
						if ( id == PROPERTY_COMMON_PLANE1_EQUATION )
						{
							pCommonProperty->m_clipPlaneEquation1 = D3DXPLANE(PlaneEqu[0],PlaneEqu[1],PlaneEqu[2],PlaneEqu[3] );
							pCommonProperty->m_pSelectionDisplay->m_pClipPlane1->SetPlaneEquation(pCommonProperty->m_clipPlaneEquation1);
						}
						else
						{
							pCommonProperty->m_clipPlaneEquation2 = D3DXPLANE(PlaneEqu[0],PlaneEqu[1],PlaneEqu[2],PlaneEqu[3] );
							pCommonProperty->m_pSelectionDisplay->m_pClipPlane2->SetPlaneEquation(pCommonProperty->m_clipPlaneEquation2);
						}
						SetClipPlaneEquationToShader();
					}
				}
			}
		default:
			{
				if ( id >= PROPERTY_SPACEFILL_ATOM_SIZE_0 && id <= PROPERTY_SPACEFILL_ATOM_SIZE_20 )
				{	//	atom size change.
					FLOAT oldValue = GetPropertySpaceFill()->m_atomRadius[id-PROPERTY_SPACEFILL_ATOM_SIZE_0];
					CString value = pValue;
					FLOAT newValue = atof(value);
					//  값이 허용되는 범위인가를 확인.  
					if ( newValue == 0.0f || newValue < 0 )
					{	//	잘못된것.
						CString strText;	
						strText.Format("%.2f" , oldValue);
						//pItem->SetValue(strText);
					}
					else
					{	//	허용되는값.
						GetPropertySpaceFill()->m_atomRadius[id-PROPERTY_SPACEFILL_ATOM_SIZE_0] = newValue;
  
						//    refresh screen
						UpdateAtomPosColorChanged();
					}
				}
			}
			break;
	}  
	g_bRequestRender = TRUE;
}

//
//	Annotation
//
#pragma region	Annotation

void CSelectionDisplay::SetAnnotationInfo(int iAnno)
{
	CPropertyCommon * property = GetPropertyCommon();
	if ( property == NULL )
		return;

	if ( m_pAnnotation[iAnno] == NULL )
		return;

	if ( property->m_bAnnotation[iAnno] == FALSE )
		return;

	//	SetFontInfo
	m_pAnnotation[iAnno]->SetFontInfo (property->m_logFont[iAnno].lfFaceName, property->m_logFont[iAnno].lfHeight);

	//	SetTextInfo.
	CStringArray arrayMsg;
	CSTLArrayColor arrayColor;
	CSTLVectorValueArray arrayPos;

	if ( iAnno == ANNOTATION_VP )
	{
		arrayMsg.Add(property->m_strAnnotation[iAnno]);
		arrayColor.push_back(COLORREF2D3DXCOLOR(property->m_annotationColor[iAnno]));

		FLOAT xPosRel = property->m_annotationXPos[iAnno]/100.0f;
		FLOAT yPosRel = property->m_annotationYPos[iAnno]/100.0f;
		FLOAT zPosRel = property->m_annotationZPos[iAnno]/100.0f;

		D3DXVECTOR3 pos;
		pos.x = m_minMaxBB[0].x + (m_minMaxBB[1].x-m_minMaxBB[0].x) * xPosRel;
		pos.y = m_minMaxBB[0].y + (m_minMaxBB[1].y-m_minMaxBB[0].y) * yPosRel;
		pos.z = m_minMaxBB[0].z + (m_minMaxBB[1].z-m_minMaxBB[0].z) * zPosRel;

		arrayPos.push_back(pos);
	}
	else 
	if ( iAnno == ANNOTATION_ATOM )
	{
		for ( int i = 0 ; i < m_arrayAtomInst.size() ; i++ )
		{
			CAtomInst *pAtomInst = m_arrayAtomInst[i];
			CString strName = pAtomInst->GetAtom()->m_atomName;
			if ( property->m_enumAnnotatonType[ANNOTATION_ATOM] == 1 )	//	1 char
			{
				strName = CString(pAtomInst->GetAtom()->m_atomName[1]);
			}

			strName.TrimLeft();
			strName.TrimRight();
			arrayMsg.Add(strName);

			//
			//	color setting.
			//
			if ( property->m_enumAnnotationColorScheme[iAnno]  == 0 )
			{
				CColorRow * pColorRow = GetAtomColor(pAtomInst->GetAtom());
				D3DXCOLOR colorDiffuse = pColorRow->m_color;
				arrayColor.push_back(colorDiffuse );
			}
			else
			{
				arrayColor.push_back(COLORREF2D3DXCOLOR(property->m_annotationColor[iAnno]));
			}

			//	pos setting.
			D3DXVECTOR3 pos = pAtomInst->GetAtom()->m_pos;

			FLOAT xPosRel = property->m_annotationXPos[iAnno]/10.0f - 5.0f;
			FLOAT yPosRel = property->m_annotationYPos[iAnno]/10.0f - 5.0f;
			FLOAT zPosRel = property->m_annotationZPos[iAnno]/10.0f - 5.0f;

			pos.x += xPosRel;
			pos.y += yPosRel;
			pos.z += zPosRel;

			arrayPos.push_back(pos);
		}
	}
	else 
	if ( iAnno == ANNOTATION_RESIDUE )
	{
		//	선택된 atom 이 전체 residue 일수도 있고, residue 의 일부만일수도 있다.
		//	하나라도 선택된 atom 이 있으면 residue 이름을 표시

		CResidueInst * residueOld = NULL;
		CSTLArrayResidueInst	arrayResidueInst;
		for ( int i = 0 ; i < m_arrayAtomInst.size() ; i++ )
		{
			CAtomInst * pAtomInst = m_arrayAtomInst[i];
			if ( pAtomInst->m_pResidueInst != residueOld )
			{
				arrayResidueInst.push_back(pAtomInst->m_pResidueInst);
				residueOld = pAtomInst->m_pResidueInst;
			}
		}

		for ( int i = 0 ; i < arrayResidueInst.size() ; i++ )
		{
			CResidueInst * pResidueInst = arrayResidueInst[i];

			CString strName;
			if ( property->m_enumAnnotatonType[ANNOTATION_RESIDUE] == 0 )	//	full
				strName = pResidueInst->GetResidue()->GetResidueName();
			else 
				strName = pResidueInst->GetResidue()->m_residueNameOneChar;

			strName.TrimLeft();	strName.TrimRight();
			arrayMsg.Add(strName);

			//
			//	color setting.
			//
			if ( property->m_enumAnnotationColorScheme[iAnno]  == 0 )
			{
				CColorRow * pColorRow = GetAtomColor(pResidueInst->GetResidue()->GetCAAtom());
				D3DXCOLOR colorDiffuse = pColorRow->m_color;
				arrayColor.push_back(colorDiffuse );
			}
			else
			{
				arrayColor.push_back(COLORREF2D3DXCOLOR(property->m_annotationColor[iAnno]));
			}

			//	pos setting.
			D3DXVECTOR3 pos = pResidueInst->GetResidue()->m_center;

			FLOAT xPosRel = property->m_annotationXPos[iAnno]/10.0f - 5.0f;
			FLOAT yPosRel = property->m_annotationYPos[iAnno]/10.0f - 5.0f;
			FLOAT zPosRel = property->m_annotationZPos[iAnno]/10.0f - 5.0f;

			pos.x += xPosRel;
			pos.y += yPosRel;
			pos.z += zPosRel;

			arrayPos.push_back(pos);
		}
	}

	m_pAnnotation[iAnno]->SetTextInfo (arrayMsg, arrayColor, arrayPos, property->m_annotationTextXPos[iAnno], property->m_annotationTextYPos[iAnno] );
}
#pragma managed(push,off)
HRESULT CSelectionDisplay::RenderAnnotation()
{
	CPropertyCommon * property = GetPropertyCommon();

	for ( int iAnno = 0 ; iAnno < 3 ; iAnno++ )
	{
		if ( property->m_bAnnotation[iAnno] == TRUE && m_pAnnotation[iAnno] )
		{
			if ( property->m_enumTextDisplayTechnique[iAnno] == 1 )	//	 2D
			{
				m_pProteinVistaRenderer->m_pd3dDevice->SetRenderState( D3DRS_ZENABLE , FALSE );
			}

			//	m_annotationDepthBias 
			FLOAT depthBias = property->m_annotationDepthBias[iAnno];
			if ( depthBias != 0.0f )
			{
				D3DXMATRIXA16 matWorldView;
				D3DXMatrixMultiply( &matWorldView, &(m_pPDBRenderer->m_matWorld) , m_pProteinVistaRenderer->GetViewMatrix() );

				D3DXMATRIXA16 matWorldViewProj;
				D3DXMatrixMultiply( &matWorldViewProj, &matWorldView , m_pProteinVistaRenderer->GetProjMatrix() );

				D3DXVECTOR4 vecPosOrigView;
				D3DXVECTOR4 vecPosOrigProj;
				D3DXVec3Transform ( &vecPosOrigView, &m_center , &matWorldView);
				D3DXVec3Transform ( &vecPosOrigProj, &m_center , &matWorldViewProj );

				D3DXVECTOR4 vecPosBiasView;
				D3DXVec3Transform ( &vecPosBiasView, &m_center , &matWorldView);
				vecPosBiasView.z -= (depthBias/10.0f);		//	100/10 = 10
				if ( vecPosBiasView.z < 0 )	vecPosBiasView.z = 0.001f;

				D3DXVECTOR4 vecPosBiasProj;
				D3DXVec4Transform ( &vecPosBiasProj, &vecPosBiasView , m_pProteinVistaRenderer->GetProjMatrix() );

				float delta = vecPosBiasProj.z/vecPosBiasProj.w - vecPosOrigProj.z/vecPosOrigProj.w;

				//	delta is negative.
				FLOAT depthBiasValue = delta;
				m_pProteinVistaRenderer->GetD3DDevice()->SetRenderState(D3DRS_DEPTHBIAS, *(DWORD*)&(depthBiasValue) );
			}

			m_pProteinVistaRenderer->SetShaderVertexAlpha(property->m_annotationTransparency[iAnno]/100.0f);
			m_pAnnotation[iAnno]->SetModelMatrix ( &(m_pPDBRenderer->m_matWorld));
			m_pAnnotation[iAnno]->Render();

			if ( depthBias != 0.0f )
			{
				FLOAT depthBiasValue = 0.0f;
				m_pProteinVistaRenderer->GetD3DDevice()->SetRenderState(D3DRS_DEPTHBIAS, *(DWORD*)&(depthBiasValue) );
			}

			if ( property->m_enumTextDisplayTechnique[iAnno] == 1 )	//	 2D
			{
				m_pProteinVistaRenderer->m_pd3dDevice->SetRenderState( D3DRS_ZENABLE , TRUE );
			}

		}
	}
	return S_OK;
}
#pragma managed(pop)
void CSelectionDisplay::UpdateAnnotation()
{
	//	color scheme이 바뀌면, annotation의 text color도 바뀌어야 한다.
	for ( int i = 0 ; i < 3 ; i++ )
	{
		m_pAnnotation[i]->DeleteDeviceObjects();
		m_pAnnotation[i]->Init(m_pProteinVistaRenderer);
		SetAnnotationInfo(i);
		m_pAnnotation[i]->InitDeviceObjects();
	}
}

#pragma endregion

