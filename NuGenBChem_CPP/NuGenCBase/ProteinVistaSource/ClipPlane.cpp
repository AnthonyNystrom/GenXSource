#include "StdAfx.h"

#include "ClipPlane.h"
#include "ProteinVistaRenderer.h"
#include "Interface.h"
#include "Pick.h"
#include "ProteinVistaView.h"
#include "MatrixMath.h"
#include "PDBRenderer.h"
#include "SelectionDisplay.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif


//    
//    
CClipPlane::CClipPlane()
{
	m_numFace = 0;

	m_bDrag = FALSE;
	m_posOldX = m_posOldY = -10000;

	D3DXMatrixIdentity(&m_matWorld);
	D3DXMatrixIdentity(&m_matWorldOld);

	D3DXMatrixRotationY(&m_matWorldMouseMoveRot, D3DXToRadian(110));
	D3DXMatrixIdentity(&m_matWorldMouseMoveTrans);

	D3DXMatrixIdentity(&m_matWorldUserInput);
	D3DXMatrixIdentity(&m_matWorldUserInputOrig);

	m_pVB = NULL;
	m_pDeclClipPlane = NULL;

	m_transparency = 100;
	m_colorPlane = D3DXCOLOR(1,1,1,0);

	//m_pItemPlaneText = NULL;
}

void CClipPlane::Init (CProteinVistaRenderer * pProteinVistaRenderer, CSelectionDisplay * pSelectionDisplay, float radius) 
{ 
	CMoleculeRenderObject::Init(pProteinVistaRenderer);

	m_pSelectionDisplay = pSelectionDisplay;
	m_pPDBRenderer = NULL;
	if ( m_pSelectionDisplay )
		m_pPDBRenderer = m_pSelectionDisplay->m_pPDBRenderer;

	m_radius = radius;
	//m_pItemPlaneText = pItemPlaneText;
}

HRESULT CClipPlane::InitDeviceObjects()
{
	HRESULT hr;

	DeleteDeviceObjects();

	FLOAT radius = (m_radius*2)/3;	

	float theta;
	m_vecPos.clear();
	m_vecPos.reserve(50);

	D3DXVECTOR3 vec(0,0,0);
	m_vecPos.push_back(vec);

	for ( theta = 0.0f; theta < D3DX_PI * 2 ; theta += D3DXToRadian(8) )
	{
		vec.x = cos(theta);
		vec.y = sin(theta);
		vec.z = 0.0f;

		m_vecPos.push_back(vec);
	}

	m_vecPos.push_back(m_vecPos[1]);

	m_vecBasePos[0] = m_vecPos[1];
	m_vecBasePos[1] = m_vecPos[m_vecPos.size()/3];
	m_vecBasePos[2] = m_vecPos[m_vecPos.size()*2/3];

	m_numFace = m_vecPos.size()-2;

	//	vertex buffer.
	LPDIRECT3DDEVICE9 device = m_pProteinVistaRenderer->GetD3DDevice();
	 
	D3DVERTEXELEMENT9	decl[MAX_FVF_DECL_SIZE];
	D3DXDeclaratorFromFVF( CVertexClipPlane::FVF, decl);
	device->CreateVertexDeclaration(decl, &m_pDeclClipPlane);

	//	len : sizeof(vertexClipPlane) * ( 1+ 360/8 )
	hr = device->CreateVertexBuffer( m_vecPos.size() * sizeof(CVertexClipPlane) , 0, CVertexClipPlane::FVF, D3DPOOL_MANAGED, &m_pVB, NULL );

	CVertexClipPlane * vertexClipPlane;
	m_pVB->Lock(0,0, (VOID**) &vertexClipPlane, 0 );

	for ( long i = 0 ; i < m_vecPos.size() ; i++ )
	{
		vertexClipPlane[i].pos = m_vecPos[i]*radius;
		vertexClipPlane[i].normal = D3DXVECTOR3(0,0,1);
	}
	m_pVB->Unlock();

	//    update.
	UpdatePropertyPanePlaneEquation();

	return S_OK;
}

void CClipPlane::InitRenderParam(BOOL bShowClipPlane, long transparency, D3DXCOLOR &color)
{
	m_bShowClipPlane = bShowClipPlane;
	m_transparency = transparency;
	m_colorPlane = color;
}

void CClipPlane::InitRadius(float radius)
{
	if ( m_radius == radius )
		return;

	m_radius = radius;
	FLOAT render_radius = (m_radius*2)/3;	

	CVertexClipPlane * vertexClipPlane;
	m_pVB->Lock(0,0, (VOID**) &vertexClipPlane, 0 );

	for ( long i = 0 ; i < m_vecPos.size() ; i++ )
	{
		vertexClipPlane[i].pos = m_vecPos[i]*render_radius;
		vertexClipPlane[i].normal = D3DXVECTOR3(0,0,1);
	}
	m_pVB->Unlock();
}
#pragma managed(push,off)
HRESULT CClipPlane::FrameMove()
{
	//	
	if ( m_pSelectionDisplay )
	{
		D3DXMATRIXA16 worldRot = m_pPDBRenderer->m_matWorld;
		worldRot._41 = worldRot._42 = worldRot._43 = 0.0f;

		D3DXMATRIXA16 worldTrans;
		D3DXMatrixIdentity(&worldTrans);

		D3DXVECTOR3 center = m_pSelectionDisplay->m_center;
		D3DXVec3TransformCoord(&center, &center, &(m_pPDBRenderer->m_matWorld));

		worldTrans._41 = center.x;
		worldTrans._42 = center.y;
		worldTrans._43 = center.z;

		m_matWorld =  m_matWorldUserInput * worldRot * worldTrans; 
	}
	else
	{	
		//	global clip plane.
		m_matWorld = m_matWorldUserInput;
	}

	return S_OK;
}

//	둥근 원판 그리기.
HRESULT CClipPlane::Render()
{
	HRESULT hr;

	LPDIRECT3DDEVICE9 device = m_pProteinVistaRenderer->GetD3DDevice();

	if ( m_bShowClipPlane == TRUE )
	{
		D3DXMATRIXA16 matWorldView = m_matWorld * (*m_pProteinVistaRenderer->GetViewMatrix());
		D3DXMATRIXA16 matWorldViewProj = matWorldView * (*m_pProteinVistaRenderer->GetProjMatrix());

		//
		//
		m_pProteinVistaRenderer->SetShaderWorldMatrix( m_matWorld );
		m_pProteinVistaRenderer->SetShaderWorldViewMatrix( matWorldView );
		m_pProteinVistaRenderer->SetShaderWorldViewProjMatrix( matWorldViewProj );

		device->SetVertexDeclaration( m_pDeclClipPlane );
		device->SetStreamSource(0, m_pVB, 0, sizeof(CVertexClipPlane));

		if ( m_transparency == 100 )
		{
			m_pProteinVistaRenderer->SetShaderTechnique(CProteinVistaRenderer::ClipPlaneRenderingNoAlpha );
		}
		else
		{
			m_pProteinVistaRenderer->SetShaderTechnique(CProteinVistaRenderer::ClipPlaneRenderingWithAlpha );
		}

		m_pProteinVistaRenderer->SetShaderVertexAlpha((float)(m_transparency)/100.0f);
		m_pProteinVistaRenderer->SetShaderDiffuseColor(m_colorPlane);

		UINT cPasses;
		V( m_pProteinVistaRenderer->m_pEffectBasicShading->Begin(&cPasses, 0) );
		for (long iPass = 0; iPass < cPasses; iPass++)
		{
			V( m_pProteinVistaRenderer->m_pEffectBasicShading->BeginPass(iPass) );

			device->DrawPrimitive(D3DPT_TRIANGLEFAN, 0, m_numFace);

			V( m_pProteinVistaRenderer->m_pEffectBasicShading->EndPass() );
		}
		V( m_pProteinVistaRenderer->m_pEffectBasicShading->End() );

		if ( m_bDrag == TRUE )
		{
			m_pProteinVistaRenderer->SetShaderTechnique(CProteinVistaRenderer::ClipPlaneRenderingNoAlpha );
			m_pProteinVistaRenderer->SetShaderDiffuseColor(CColorSchemeDefault::GetSpecularColorFromDiffuse(m_colorPlane));

			UINT cPasses;
			V( m_pProteinVistaRenderer->m_pEffectBasicShading->Begin(&cPasses, 0) );
			for (long iPass = 0; iPass < cPasses; iPass++)
			{
				V( m_pProteinVistaRenderer->m_pEffectBasicShading->BeginPass(iPass) );

				device->DrawPrimitive(D3DPT_LINESTRIP, 1, m_numFace);
		
				V( m_pProteinVistaRenderer->m_pEffectBasicShading->EndPass() );
			}
			V( m_pProteinVistaRenderer->m_pEffectBasicShading->End() );
		}
	}

	return S_OK;
}
#pragma managed(pop)
HRESULT CClipPlane::DeleteDeviceObjects()
{
	SAFE_RELEASE(m_pVB);
	SAFE_RELEASE(m_pDeclClipPlane);

	return S_OK;
}
#pragma managed(push,off)
LRESULT CClipPlane::HandleMessages ( HWND hWnd, UINT msg, WPARAM wParam, LPARAM lParam )
{
	BOOL	bProcessed = FALSE;

	int iMouseX = (short)LOWORD(lParam);
	int iMouseY = (short)HIWORD(lParam);
 
	switch( msg )
	{
		case WM_LBUTTONDOWN:
		case WM_RBUTTONDOWN:
			SetCapture( hWnd );
			m_bDrag = TRUE;
			m_posOldX = iMouseX;
			m_posOldY = iMouseY;

			m_matWorldOld = m_matWorld;
			m_matWorldUserInputOrig = m_matWorldUserInput;
			D3DXMatrixIdentity(&m_matWorldMouseMoveRot);
			D3DXMatrixIdentity(&m_matWorldMouseMoveTrans);
 
			bProcessed = TRUE;
			break;
           
		case WM_LBUTTONUP:
		case WM_RBUTTONUP:
			if( m_bDrag == TRUE )
			{
				m_bDrag = FALSE;
				m_posOldX = -10000;
				m_posOldY = -10000;
				bProcessed = TRUE;
				UpdatePropertyPanePlaneEquation();
				ReleaseCapture();
			}
			break;

		case WM_MOUSEMOVE:
			if (m_bDrag)  // m_bDrag == TRUE
			{
				if(MK_LBUTTON&wParam)		//	//	rotation
				{
					D3DXVECTOR3 axisX(1,0,0);
					D3DXVECTOR3 axisY(0,1,0);

					D3DXMATRIXA16 matRotModel = m_matWorldOld;
					matRotModel._41 = matRotModel._42 = matRotModel._43 = 0.0f;

					D3DXMATRIXA16	matRotInv;
					D3DXMatrixInverse(&matRotInv, NULL, &(matRotModel*m_pProteinVistaRenderer->m_matCameraRotation) );

					D3DXVec3TransformCoord(&axisX, &axisX, &matRotInv );
					D3DXVec3TransformCoord(&axisY, &axisY, &matRotInv );

					//	m_matWorld
					D3DXMATRIXA16 matX, matY, matRotTemp;
					D3DXMatrixRotationAxis(&matX, &axisX, -(iMouseY-m_posOldY)/100.0f );
					D3DXMatrixRotationAxis(&matY, &axisY, -(iMouseX-m_posOldX)/100.0f );
					matRotTemp = matX * matY;
 
					m_matWorldMouseMoveRot = matRotTemp;
					//	
					m_matWorldUserInput = m_matWorldMouseMoveRot * m_matWorldUserInputOrig;

					bProcessed = TRUE;
					GetMainActiveView()->OnPaint();
				}
				else if ( MK_RBUTTON&wParam )		//	translation
				{
					D3DXMATRIXA16 matRotModel = m_matWorldOld;
					matRotModel._41 = matRotModel._42 = matRotModel._43 = 0.0f;

					//	translation
					D3DXMATRIXA16	matRotInv;
					D3DXMatrixInverse (&matRotInv, NULL, &(matRotModel*m_pProteinVistaRenderer->m_matCameraRotation) );

					D3DXVECTOR3 vecTrans;
					D3DXVECTOR3 vecLen((iMouseX-m_posOldX)/10.0f, - (iMouseY-m_posOldY)/10.0f, 0 );
					D3DXVec3TransformCoord(&vecTrans, &vecLen, &matRotInv);

					m_matWorldMouseMoveTrans._41 = vecTrans.x;
					m_matWorldMouseMoveTrans._42 = vecTrans.y;
					m_matWorldMouseMoveTrans._43 = vecTrans.z;
 
					m_matWorldUserInput = m_matWorldMouseMoveTrans * m_matWorldUserInputOrig;
 
					bProcessed = TRUE;
				}
				UpdatePropertyPanePlaneEquation();
				
			}
			break;
		case WM_CAPTURECHANGED:
			{
				if ( m_bDrag == TRUE )
				{
					if( (lParam != NULL) && ((HWND)lParam != hWnd) )
					{
						ReleaseCapture();

						m_bDrag = FALSE;
						m_posOldX = -10000;
						m_posOldY = -10000;
						bProcessed = TRUE;

						UpdatePropertyPanePlaneEquation();
					}
				}
			}
			break;
	}

	return bProcessed;
}
#pragma managed(pop)
//	
//    
void CClipPlane::SetPlaneEquation(D3DXPLANE & pPlane)
{
	D3DXPlaneNormalize(&pPlane, &pPlane);

	D3DXMATRIX Out;
	D3DXVECTOR3 Eye(0,0,0);
	D3DXVECTOR3 At(pPlane.a, pPlane.b, pPlane.c );
	D3DXVECTOR3 temp(10,3,5);	//	임의의 벡터.
	D3DXVECTOR3 Up;
	D3DXVec3Cross(&Up, &At, &temp);

	//    D3DXMatrixLookAtLH( &Out, &Eye, &At, &Up);
	D3DXMATRIX matTransform; 
	GetMatrixFromVec ( &Eye, &At, &Up, &matTransform );

	m_matWorldMouseMoveRot = matTransform;

	At = At* pPlane.d;

	m_matWorldMouseMoveTrans._41 = -At.x;
	m_matWorldMouseMoveTrans._42 = -At.y; 
	m_matWorldMouseMoveTrans._43 = -At.z;

	m_matWorldUserInput = m_matWorldMouseMoveRot * m_matWorldMouseMoveTrans;
}
 
 #pragma managed(push,off)
//
D3DXPLANE * CClipPlane::GetPlaneEquation(D3DXPLANE * pPlane)
{
	D3DXMATRIXA16 matModel = m_matWorld;	

	//	4점을 가지고 equation을 만든다.
	D3DXVECTOR3 vecTransformed[3];
	for ( int i = 0 ; i < 3 ; i++ )
	{
		D3DXVec3TransformCoord(&vecTransformed[i], &m_vecBasePos[i], &matModel);
	}
	D3DXPlaneFromPoints(pPlane, &vecTransformed[0], &vecTransformed[1], &vecTransformed[2] );

	return pPlane;
}

#pragma managed(pop)
void CClipPlane::UpdatePropertyPanePlaneEquation()
{
	D3DXPLANE planeEq;
	GetPlaneEquation(&planeEq);
  
	// if ( m_pItemPlaneText )
	//{
	//	

	//	//CString strEqu;
	//	//strEqu.Format ("%.3f,%.3f,%.3f,%.3f", planeEq.a, planeEq.b, planeEq.c, planeEq.d );

	//	//m_pItemPlaneText->SetValue(strEqu);
	//} 
}

BOOL CClipPlane::Pick(FLOAT &dist)
{
	D3DXVECTOR3 vPickRayDir;
	D3DXVECTOR3 vPickRayOrig;
	::GetPickRay ( m_matWorld , vPickRayDir, vPickRayOrig );

	FLOAT radius = (m_radius*2)/3;	
	for ( float theta = 0.0f; theta < D3DX_PI * 2 ; theta += D3DXToRadian(8) )
	{
		D3DXVECTOR3 pos1 = D3DXVECTOR3 (radius * cos(theta), radius * sin(theta), 0 );
		D3DXVECTOR3 pos2 = D3DXVECTOR3 (radius * cos(theta+D3DXToRadian(8)), radius * sin(theta+D3DXToRadian(8)), 0 );
		D3DXVECTOR3 pos3 = D3DXVECTOR3 (0,0,0);
		
		FLOAT fDist, u, v;
		BOOL bHit = ::IntersectTriangle( vPickRayOrig, vPickRayDir, pos1, pos2, pos3, &fDist, &u, &v );
		if ( bHit == TRUE )
		{
			dist = fDist;
			return TRUE;
		}
	}

	return FALSE;
}

HRESULT CClipPlane::Save(CFile &file)
{
	file.Write( &m_radius,sizeof(float));
	file.Write( &m_transparency, sizeof(long));
	file.Write( &m_colorPlane, sizeof(D3DXCOLOR));

	file.Write( &m_matWorld, sizeof(D3DXMATRIXA16));
	file.Write( &m_matWorldUserInput, sizeof(D3DXMATRIXA16));
	file.Write( &m_matWorldMouseMoveRot, sizeof(D3DXMATRIXA16));
	file.Write( &m_matWorldMouseMoveTrans, sizeof(D3DXMATRIXA16));

	return S_OK;
}

HRESULT CClipPlane::Load(CFile &file)
{
	file.Read( &m_radius,sizeof(float));
	file.Read( &m_transparency, sizeof(long));
	file.Read( &m_colorPlane, sizeof(D3DXCOLOR));

	file.Read( &m_matWorld, sizeof(D3DXMATRIXA16));
	file.Read( &m_matWorldUserInput, sizeof(D3DXMATRIXA16));
	file.Read( &m_matWorldMouseMoveRot, sizeof(D3DXMATRIXA16));
	file.Read( &m_matWorldMouseMoveTrans, sizeof(D3DXMATRIXA16));
	
	return S_OK;
}



