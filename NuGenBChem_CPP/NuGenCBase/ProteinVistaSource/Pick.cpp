#include "stdafx.h"
#include "ProteinVista.h"
#include "ProteinVistaView.h"
#include "PDBRenderer.h"

#include "ProteinVistaRenderer.h"
#include "Pick.h"

#include "ProteinSurfaceMSMS.h"
#include "RenderRibbonSelection.h"
#include "Interface.h"
#include "Utility.h"
#include "RenderSurfaceSelection.h"
#include "ClipPlane.h"
 
#include "SelectionListPane.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif
#pragma managed(push,off)
void CPDBRenderer::SetPickRay(POINT pt)
{
	D3DXMATRIXA16 * pMatView = m_pProteinVistaRenderer->GetViewMatrix();
	D3DXMATRIXA16 * pMatProj = m_pProteinVistaRenderer->GetProjMatrix();
	D3DXMATRIXA16 * pMatWorld = &m_matWorld;

	HWND hWnd =  GetMainApp()->m_CanvsHandle;
	POINT ptCursor = pt;

	// Compute the vector of the pick ray in screen space
	D3DXVECTOR3 v;
	v.x =  ( ( ( 2.0f * ptCursor.x ) / m_pProteinVistaRenderer->m_d3dsdBackBuffer.Width  ) - 1 ) / pMatProj->_11;
	v.y = -( ( ( 2.0f * ptCursor.y ) / m_pProteinVistaRenderer->m_d3dsdBackBuffer.Height ) - 1 ) / pMatProj->_22;
	v.z =  1.0f;

	D3DXMATRIXA16 m = (*pMatWorld) * (*pMatView);
	D3DXMatrixInverse( &m, NULL, &m );

	// Transform the screen space pick ray into 3D space
	m_vPickRayDir.x  = v.x*m._11 + v.y*m._21 + v.z*m._31;
	m_vPickRayDir.y  = v.x*m._12 + v.y*m._22 + v.z*m._32;
	m_vPickRayDir.z  = v.x*m._13 + v.y*m._23 + v.z*m._33;
	m_vPickRayOrig.x = m._41;
	m_vPickRayOrig.y = m._42;
	m_vPickRayOrig.z = m._43;
}

void GetPickRay ( POINT & ptCursor, D3DXMATRIXA16 &matWorld, D3DXVECTOR3 & vPickRayDir, D3DXVECTOR3 & vPickRayOrig )
{
	CProteinVistaRenderer * pProteinVistaRenderer = GetProteinVistaRenderer();
	D3DXMATRIXA16 * pMatView = pProteinVistaRenderer->GetViewMatrix();
	D3DXMATRIXA16 * pMatProj = pProteinVistaRenderer->GetProjMatrix();
	D3DXMATRIXA16 * pMatWorld = &matWorld;

	// Compute the vector of the pick ray in screen space
	D3DXVECTOR3 v;
	v.x =  ( ( ( 2.0f * ptCursor.x ) / pProteinVistaRenderer->m_d3dsdBackBuffer.Width  ) - 1 ) / pMatProj->_11;
	v.y = -( ( ( 2.0f * ptCursor.y ) / pProteinVistaRenderer->m_d3dsdBackBuffer.Height ) - 1 ) / pMatProj->_22;
	v.z =  1.0f;

	D3DXMATRIXA16 m = (*pMatWorld) * (*pMatView);
	D3DXMatrixInverse( &m, NULL, &m );

	// Transform the screen space pick ray into 3D space
	vPickRayDir.x  = v.x*m._11 + v.y*m._21 + v.z*m._31;
	vPickRayDir.y  = v.x*m._12 + v.y*m._22 + v.z*m._32;
	vPickRayDir.z  = v.x*m._13 + v.y*m._23 + v.z*m._33;
	vPickRayOrig.x = m._41;
	vPickRayOrig.y = m._42;
	vPickRayOrig.z = m._43;
}

void GetPickRay (D3DXMATRIXA16 &matWorld, D3DXVECTOR3 & vPickRayDir, D3DXVECTOR3 & vPickRayOrig )
{
	HWND hWnd =  GetMainApp()->m_CanvsHandle;
	POINT ptCursor;
	GetCursorPos( &ptCursor );
	ScreenToClient( hWnd, &ptCursor );

	GetPickRay ( ptCursor, matWorld, vPickRayDir, vPickRayOrig );
}
#pragma managed(pop)
//////////////////////////////////////////////////////////////////////////
void CProteinVistaRenderer::PickWireBallStickInSelectionList( CSTLArrayPickedAtomInst & arrayPickAtomInst )
{
	for ( int i = 0 ; i < MAX_DISPLAY_SELECTION_INDEX ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = m_arraySelectionDisplay[i];
		if ( pSelectionDisplay == NULL )
			continue;

		if ( pSelectionDisplay->m_bShow == TRUE )
		{
			long displayStyle = pSelectionDisplay->m_displayStyle;
			if ( displayStyle == CSelectionDisplay::SURFACE || displayStyle ==CSelectionDisplay::RIBBON )
				continue;

			//	
			for ( int iAtom = 0 ; iAtom < pSelectionDisplay->m_arrayAtomInst.size(); iAtom ++ )
			{
				CAtomInst * pAtomInst = pSelectionDisplay->m_arrayAtomInst[iAtom];
				CAtom * pAtom = pAtomInst->GetAtom();

				BOOL bShowed = PosClipPlaneClipped(pSelectionDisplay, pAtom->m_pos);
				if ( bShowed == TRUE )
				{
					FLOAT fLen = CalcLenPointToLine ( pAtom->m_pos, pSelectionDisplay->m_pPDBRenderer->m_vPickRayDir , pSelectionDisplay->m_pPDBRenderer->m_vPickRayOrig );

					BOOL bSelect = FALSE;
					if ( displayStyle == CSelectionDisplay::WIREFRAME || displayStyle == CSelectionDisplay::STICKS )
					{
						if ( fLen < m_sphereRadius[CProteinVistaRenderer::INDEX_SPHERE_BALL_DISPLAY] )	//	0.25
						{
							bSelect = TRUE;
						}
					}
					else if ( displayStyle == CSelectionDisplay::SPACEFILL )
					{
						if ( fLen < pAtom->m_fRadius )
						{
							bSelect = TRUE;
						}
					} 
					else if ( displayStyle == CSelectionDisplay::BALLANDSTICK )
					{
						if ( fLen < m_sphereRadius[CProteinVistaRenderer::INDEX_SPHERE_BALL_STICK_DISPLAY] )	//	0.5
						{
							bSelect = TRUE;
						}
					}

					if( bSelect == TRUE )
					{
						CPickAtomInst pickAtom;
						D3DXVECTOR3 vec = pSelectionDisplay->m_pPDBRenderer->m_vPickRayOrig-(pAtom->m_pos);

						pickAtom.m_pAtomInst = pAtomInst;
						pickAtom.m_len = D3DXVec3Length(&vec);

						arrayPickAtomInst.push_back(pickAtom);

						//	CString strMsg;
						//	strMsg.Format("%s-%d-(%f,%f,%f)\n", pAtom->m_atomName, pAtomInst->GetSelect(), pAtom->m_pos.x, pAtom->m_pos.y, pAtom->m_pos.z );
						//	OutputTextMsg(strMsg);
					}
				}
			}
		}
	}
}

void CPDBRenderer::Pick ( CSTLArrayPickedAtomInst & pickAtomArray )
{
 
}

BOOL CProteinVistaRenderer::PosClipPlaneClipped(CSelectionDisplay * pSelectionDisplay, D3DXVECTOR3 & pos)
{
	D3DXMATRIXA16 * pMatWorld = pSelectionDisplay->m_pPDBRenderer->GetWorldMatrix();

	D3DXVECTOR3 posTransformed;
	D3DXVec3TransformCoord( &posTransformed, &pos, pMatWorld );
	//
	//	D3DXVECTOR3 pos가  clip 되었는지 조사.
	//
	long bShowed = FALSE;

	float clipValue0 = 1.0f;
	float clipValue1 = 1.0f;
	float clipValue2 = 1.0f;

	if ( m_pPropertyScene->m_bClipping0 == TRUE )
	{
		D3DXPLANE & plane0 = m_pPropertyScene->m_clipPlaneEquation0;
		clipValue0 = plane0.a * posTransformed.x + plane0.b * posTransformed.y + plane0.c * posTransformed.z + plane0.d ;
		if ( m_pPropertyScene->m_bClipDirection0 == FALSE )
			clipValue0 *= -100.0f;
	}

	if ( pSelectionDisplay->GetPropertyCommon()->m_bClipping1 == TRUE )
	{
		D3DXPLANE & plane0 = pSelectionDisplay->GetPropertyCommon()->m_clipPlaneEquation1;
		clipValue1 = plane0.a * posTransformed.x + plane0.b * posTransformed.y + plane0.c * posTransformed.z + plane0.d ;
		if ( pSelectionDisplay->GetPropertyCommon()->m_bClipDirection1 == FALSE )
			clipValue1 *= -100.0f;
	}

	if ( pSelectionDisplay->GetPropertyCommon()->m_bClipping2 == TRUE )
	{
		D3DXPLANE & plane0 = pSelectionDisplay->GetPropertyCommon()->m_clipPlaneEquation2;
		clipValue2 = plane0.a * posTransformed.x + plane0.b * posTransformed.y + plane0.c * posTransformed.z + plane0.d ;
		if ( pSelectionDisplay->GetPropertyCommon()->m_bClipDirection2 == FALSE )
			clipValue2 *= -100.0f;
	}

	if ( clipValue0 * clipValue1 * clipValue2 >= 0 )
		bShowed = TRUE;

	return bShowed;
}

//////////////////////////////////////////////////////////////////////////
//	Ribbon 과 Surface 에서 Pick을 찾는다.
//////////////////////////////////////////////////////////////////////////
void CProteinVistaRenderer::PickRibbonInSelectionList(CSTLArrayPickedResidueInst & pickResidueArray)
{
	for ( int i = 0 ; i < MAX_DISPLAY_SELECTION_INDEX ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = m_arraySelectionDisplay[i];
		if ( pSelectionDisplay == NULL )
			continue;

		if ( pSelectionDisplay->m_bShow == TRUE )
		{
			if ( pSelectionDisplay->m_displayStyle == CSelectionDisplay::RIBBON )
			{
				D3DXVECTOR3 center = pSelectionDisplay->m_center;
				FLOAT radius = pSelectionDisplay->m_radius;
				FLOAT fLen = CalcLenPointToLine ( center , pSelectionDisplay->m_pPDBRenderer->m_vPickRayDir , pSelectionDisplay->m_pPDBRenderer->m_vPickRayOrig );
				if ( fLen > radius )
					continue;

				for ( int iObject = 0 ; iObject < pSelectionDisplay->m_arrayRenderObjectSelection.size() ; iObject ++ )
				{
					CRenderRibbonSelectionContainer * pRenderRibbonSelectionContainer = (CRenderRibbonSelectionContainer *)(pSelectionDisplay->m_arrayRenderObjectSelection[iObject]);

					pRenderRibbonSelectionContainer->Picking(pSelectionDisplay->m_pPDBRenderer->m_vPickRayDir,pSelectionDisplay->m_pPDBRenderer->m_vPickRayOrig, pickResidueArray );

					for ( int j = 0 ; j < pickResidueArray.size(); j++ )
					{
						BOOL bShowed = PosClipPlaneClipped(pSelectionDisplay, pickResidueArray[j].m_pos );
						if ( bShowed == FALSE )
						{
							//	clipping 되어서 없어지는것
							pickResidueArray.erase(pickResidueArray.begin()+j);
							j--;
						}
					}
				}
			}
		}
	}
}

void CProteinVistaRenderer::PickSurfaceInSelectionList(CSTLArrayPickedAtomInst & pickAtomArray)
{
	for ( int i = 0 ; i < MAX_DISPLAY_SELECTION_INDEX ; i++ )
	{
		CSelectionDisplay * pSelectionDisplay = m_arraySelectionDisplay[i];
		if ( pSelectionDisplay == NULL )
			continue;

		if ( pSelectionDisplay->m_bShow == TRUE )
		{
			if ( pSelectionDisplay->m_displayStyle == CSelectionDisplay::SURFACE )
			{
				D3DXVECTOR3 center = pSelectionDisplay->m_center;
				FLOAT radius = pSelectionDisplay->m_radius;
				FLOAT fLen = CalcLenPointToLine ( center , pSelectionDisplay->m_pPDBRenderer->m_vPickRayDir , pSelectionDisplay->m_pPDBRenderer->m_vPickRayOrig );
				if ( fLen > radius )
					continue;

				for ( int iObject = 0 ; iObject < pSelectionDisplay->m_arrayRenderObjectSelection.size() ; iObject ++ )
				{
					CRenderSurfaceSelection * pRenderSurfaceSelection = (CRenderSurfaceSelection *)(pSelectionDisplay->m_arrayRenderObjectSelection[iObject]);
					//	NULL 인 경우가 있다. chain에 HETATM 만 있는데, 이 chain을 surface로 만든경우
					if ( pRenderSurfaceSelection->m_pVB == NULL ) continue;

					CSurfaceVertex * pSurfaceVertex = NULL;
					pRenderSurfaceSelection->m_pVB->Lock(0, 0, (void**) &pSurfaceVertex, 0);

					VOID * pIB = NULL;
					pRenderSurfaceSelection->m_pIB->Lock(0,0, (VOID**) &pIB, 0 );
					
					for ( int iIB = 0 ; iIB < pRenderSurfaceSelection->m_sizeIndexBuffer ; iIB += 3 )
					{
						D3DXVECTOR3 pos1;
						D3DXVECTOR3 pos2;
						D3DXVECTOR3 pos3;

						if ( pRenderSurfaceSelection->m_byteSizeIndexBuffer == sizeof(DWORD) )
						{
							DWORD * pIBTemp = (DWORD*)pIB;
							pos1 = pSurfaceVertex[pIBTemp[iIB]].pos;
							pos2 = pSurfaceVertex[pIBTemp[iIB+1]].pos;
							pos3 = pSurfaceVertex[pIBTemp[iIB+2]].pos;
						}
						else
						{
							WORD * pIBTemp = (WORD*)pIB;
							pos1 = pSurfaceVertex[pIBTemp[iIB]].pos;
							pos2 = pSurfaceVertex[pIBTemp[iIB+1]].pos;
							pos3 = pSurfaceVertex[pIBTemp[iIB+2]].pos;
						}

						BOOL bShowed = PosClipPlaneClipped(pSelectionDisplay, pos1);
						if ( bShowed == TRUE )
						{
							FLOAT fDist,u,v;
							BOOL bIntersect = IntersectTriangle( pSelectionDisplay->m_pPDBRenderer->m_vPickRayOrig, pSelectionDisplay->m_pPDBRenderer->m_vPickRayDir, pos1, pos2, pos3, &fDist, &u, &v );
							if ( bIntersect == TRUE )
							{
								long iAtom = 0;
								if ( pRenderSurfaceSelection->m_byteSizeIndexBuffer == sizeof(DWORD) )
								{
									DWORD * pIBTemp = (DWORD*)pIB;
									iAtom = pRenderSurfaceSelection->m_pProteinSurface->m_arrayIndexAtom[pIBTemp[iIB]];
								}
								else
								{
									WORD * pIBTemp = (WORD*)pIB;
									iAtom = pRenderSurfaceSelection->m_pProteinSurface->m_arrayIndexAtom[pIBTemp[iIB]];
								}

								CAtomInst* pAtomInst= pRenderSurfaceSelection->m_arrayAtomInst[iAtom];

								CPickAtomInst pickAtom;
								//	D3DXVECTOR3 vec = (pSelectionDisplay->m_pPDBRenderer->m_vPickRayOrig)-(pAtom->m_pos);

								//	TODO: 수정
								pickAtom.m_pAtomInst = pAtomInst;
								pickAtom.m_len = fDist;

								pickAtomArray.push_back(pickAtom);

								//	selection text msg
								//	CString strMsg;
								//	strMsg.Format("%s-%d-(%f,%f,%f)\n", pAtomInst->GetAtom()->m_atomName, pAtomInst->GetSelect(), pAtomInst->GetAtom()->m_pos.x, pAtomInst->GetAtom()->m_pos.y, pAtomInst->GetAtom()->m_pos.z );
								//	OutputTextMsg(strMsg);
							}
						}
					}

					pRenderSurfaceSelection->m_pIB->Unlock();
					pRenderSurfaceSelection->m_pVB->Unlock();
				}
			}
		}
	}
}

//	한점과 직선간의 길이를 구한다.
//	ref: 좋은게임을 만드는 핵심원리 p147
FLOAT CalcLenPointToLine(D3DXVECTOR3 &p, D3DXVECTOR3 &d , D3DXVECTOR3 &p1 )
{
	//	투영
	D3DXVECTOR3 temp = p-p1;
	FLOAT t = D3DXVec3Dot( &temp , &d ) / D3DXVec3Dot(&d,&d) ;

	//	최단거리의 점 위치 계산.
	D3DXVECTOR3 q = p1 + t*d;

	//	실제 거리 계산
	D3DXVECTOR3 vecResult = p-q;
	FLOAT l = D3DXVec3Length(&vecResult);

	return l;
}

//--------------------------------------------------------------------------------------
// Given a ray origin (orig) and direction (dir), and three vertices of a triangle, this
// function returns TRUE and the interpolated texture coordinates if the ray intersects 
// the triangle
//--------------------------------------------------------------------------------------
BOOL IntersectTriangle( const D3DXVECTOR3& orig, const D3DXVECTOR3& dir, D3DXVECTOR3& v0, D3DXVECTOR3& v1, D3DXVECTOR3& v2, FLOAT* t, FLOAT* u, FLOAT* v )
{
	// Find vectors for two edges sharing vert0
	D3DXVECTOR3 edge1 = v1 - v0;
	D3DXVECTOR3 edge2 = v2 - v0;

	// Begin calculating determinant - also used to calculate U parameter
	D3DXVECTOR3 pvec;
	D3DXVec3Cross( &pvec, &dir, &edge2 );

	// If determinant is near zero, ray lies in plane of triangle
	FLOAT det = D3DXVec3Dot( &edge1, &pvec );

	D3DXVECTOR3 tvec;
	if( det > 0 )
	{
		tvec = orig - v0;
	}
	else
	{
		tvec = v0 - orig;
		det = -det;
	}

	if( det < 0.0001f )
		return FALSE;

	// Calculate U parameter and test bounds
	*u = D3DXVec3Dot( &tvec, &pvec );
	if( *u < 0.0f || *u > det )
		return FALSE;

	// Prepare to test V parameter
	D3DXVECTOR3 qvec;
	D3DXVec3Cross( &qvec, &tvec, &edge1 );

	// Calculate V parameter and test bounds
	*v = D3DXVec3Dot( &dir, &qvec );
	if( *v < 0.0f || *u + *v > det )
		return FALSE;

	// Calculate t, scale parameters, ray intersects triangle
	*t = D3DXVec3Dot( &edge2, &qvec );
	FLOAT fInvDet = 1.0f / det;
	*t *= fInvDet;
	*u *= fInvDet;
	*v *= fInvDet;

	return TRUE;
}

//===================================================================================================
//===================================================================================================
//	(0) clip plane, Light source 가 pick 되었는지 찾아냄.
//	(1) pdb enumeration 을 해서 pdb 에서 pick 된 atom을 골라냄
//	(1.1) SELECT 되었을 경우에 SELECTION LIST를 참고로 해서, atom size 에 따라 selection을 선택
//		wireframe : center 에서 delta 이하일경우에
//		ball & stick : center 에서 radius 이하일경우에
//		spacefill : center 에서 radius 이하일경우에
//	(2) Selection List를 enumeration
//	(2.1) Ribbon과 Surface 일때만 처리
//		Ribbon : 선택된 residue 
//		Surface : 선택된 atom
//

//	
BOOL CProteinVistaRenderer::GetSelectLightSource(long &selectLight, FLOAT &dist)
{
	BOOL bHitLight = FALSE;
	dist = 1e20;
	selectLight = -1;

	for ( int iLight = 0 ; iLight < MAX_LIGHTS; iLight ++ )
	{
		//	light source 가 2개이다.
		if ( (iLight == 0 && m_pPropertyScene->m_bLight1Use == TRUE) || (iLight == 1 && m_pPropertyScene->m_bLight2Use == TRUE) )
		{
			D3DXMATRIXA16 & matWorld = m_LightControl[iLight].m_matWorld;

			D3DXVECTOR3 vPickRayDir;
			D3DXVECTOR3 vPickRayOrig;
			GetPickRay ( matWorld , vPickRayDir, vPickRayOrig );

			BOOL bHit;
			DWORD dwFace;
			FLOAT fBary1, fBary2, fDist;
			D3DXIntersect( m_LightControl[iLight].s_pMesh, &vPickRayOrig, &vPickRayDir, &bHit, &dwFace, &fBary1, &fBary2, &fDist, NULL, NULL);
			if( bHit )
			{
				if ( fDist < dist )
				{
					dist = fDist;
					selectLight = iLight;
				}
				bHitLight = TRUE;
			}
		}
	}

	return bHitLight;
}

CClipPlane * CProteinVistaRenderer::GetSelectClipPlane(FLOAT &distance)
{
	CClipPlane * pClipPlane = NULL;
	FLOAT	distSelect = 1e20;
	FLOAT	dist;

	//	for ( int i = 0 ; i < MAX_DISPLAY_SELECTION_INDEX ; i++ )
	{
		//	CSelectionDisplay * pSelectionDisplay = m_arraySelectionDisplay[i];
		CSelectionDisplay * pSelectionDisplay = GetCurrentSelectionDisplay();
		if ( pSelectionDisplay != NULL && pSelectionDisplay->m_bShow == TRUE )
		{
			if ( pSelectionDisplay->GetPropertyCommon()->m_bClipping1 == TRUE && pSelectionDisplay->GetPropertyCommon()->m_bShowClipPlane1 == TRUE )
			{
				CClipPlane & clipPlane = *(pSelectionDisplay->m_pClipPlane1);
				if ( clipPlane.Pick(dist) == TRUE )
				{
					if ( dist < distSelect )
					{
						distSelect = dist;
						pClipPlane = &clipPlane;
					}
				}
			}

			if ( pSelectionDisplay->GetPropertyCommon()->m_bClipping2 == TRUE && pSelectionDisplay->GetPropertyCommon()->m_bShowClipPlane2 == TRUE )
			{
				CClipPlane & clipPlane = *(pSelectionDisplay->m_pClipPlane2);
				if ( clipPlane.Pick(dist) == TRUE )
				{
					if ( dist < distSelect )
					{
						distSelect = dist;
						pClipPlane = &clipPlane;
					}
				}
			}
		}
	}

	//	global clip plane
	if ( m_pPropertyScene->m_bClipping0 == TRUE && m_pPropertyScene->m_bShowClipPlane0 == TRUE  )
	{
		CClipPlane & clipPlane = *(m_pClipPlane);
		if ( clipPlane.Pick(dist) == TRUE )
		{
			if ( dist < distSelect )
			{
				distSelect = dist;
				pClipPlane = &clipPlane;
			}
		}
	}

	distance = distSelect;

	return pClipPlane;
}


BOOL CProteinVistaRenderer::SelectAtom(POINT pt)
{
	// Picking... 
	CSTLArrayPickedAtomInst arrayPickAtomInst;
	arrayPickAtomInst.reserve(100);

	for ( int i = 0 ; i < m_arrayPDBRenderer.size(); i++ )
	{
		CPDBRenderer * pPDBRenderer = m_arrayPDBRenderer[i];
		pPDBRenderer->SetPickRay(pt);
	}
	//	(1)
	PickWireBallStickInSelectionList(arrayPickAtomInst);

	//
	//	(2)
	//	SelectionList를 가지고 Pick을 조사함.
	//	
	PickSurfaceInSelectionList(arrayPickAtomInst);

	//	Ribbon에서의 Selection을 검색
	CSTLArrayPickedResidueInst	arrayPickResidueInst;
	PickRibbonInSelectionList(arrayPickResidueInst);

	//
	//	short len의 1개의 residue or atom을 선택
	//
	CResidueInst * pSelectResidueInst = NULL;
	CAtomInst * pSelectAtomInst = NULL;

	FLOAT	minLenAtom = 1e20;
	long	minIndexAtom = -1;

	for ( i = 0 ; i < arrayPickAtomInst.size() ; i++ )
	{
		if ( arrayPickAtomInst[i].m_len < minLenAtom )
		{
			minLenAtom = arrayPickAtomInst[i].m_len;
			minIndexAtom = i;
			pSelectAtomInst = arrayPickAtomInst[i].m_pAtomInst;
		}
	}

	FLOAT	minLenResidue = 1e20;
	long	minIndexResidue = -1;

	for ( i = 0 ; i < arrayPickResidueInst.size() ; i++ )
	{
		if ( arrayPickResidueInst[i].m_len < minLenResidue )
		{
			minLenResidue = arrayPickResidueInst[i].m_len;
			minIndexResidue = i;
			pSelectResidueInst = arrayPickResidueInst[i].m_pResidueInst;
		}
	}
	
	//	residue == atom 인가를 체크
	if ( minIndexAtom != -1 && minIndexResidue != -1 )
	{
		if ( arrayPickAtomInst[minIndexAtom].m_pAtomInst->m_pResidueInst == arrayPickResidueInst[minIndexResidue].m_pResidueInst )
		{
			//	같은 residue 이다.
			pSelectResidueInst = arrayPickResidueInst[minIndexResidue].m_pResidueInst;
			pSelectAtomInst = NULL;
		}
		else
		{
			//	다를때에는 가까운 것 선택.
			if ( arrayPickAtomInst[minIndexAtom].m_len < arrayPickResidueInst[minIndexResidue].m_len )
			{
				pSelectAtomInst = arrayPickAtomInst[minIndexAtom].m_pAtomInst;
				pSelectResidueInst = NULL;
			}
			else
			{
				pSelectResidueInst = arrayPickResidueInst[minIndexResidue].m_pResidueInst;
				pSelectAtomInst = NULL;
			}
		}
	}

	// Picking End... pSelectAtomInst or pSelectResidueInst.
	m_pLastPickObjectInst = NULL;
	BOOL bSelectionChanged = FALSE;
 
	//
	//
	if ( pSelectResidueInst != NULL )	//	residue 가 선택됨.
	{
		CResidueInst * pResidueInst;
		if( (GetAsyncKeyState( VK_LCONTROL )& 0x8000) == 0x8000 )
		{	//	residue 단위의 toggle
			pResidueInst = arrayPickResidueInst[minIndexResidue].m_pResidueInst;
			pResidueInst->SetSelectChild(!(pResidueInst->GetSelect()));
			m_pLastPickObjectInst = pResidueInst;
		}
		else
		{	//	shit를 누르고 선택하면 레지듀 단위로 추가.
			//	모두 deselect.
			SelectAll(FALSE);

			pResidueInst = arrayPickResidueInst[minIndexResidue].m_pResidueInst;
			pResidueInst->SetSelectChild(TRUE);
			m_pLastPickObjectInst = pResidueInst;
		}

		SetActivePDBRenderer(pResidueInst->m_pChainInst->m_pPDBInst->m_pPDBRenderer);
		pResidueInst->m_pChainInst->m_pPDBInst->m_pPDBRenderer->UpdatePDBRendererCenter();

		bSelectionChanged = TRUE;
	}

	if ( pSelectAtomInst != NULL )
	{
		if( ((GetAsyncKeyState( VK_LSHIFT )& 0x8000) == 0x8000) && ((GetAsyncKeyState( VK_LCONTROL )& 0x8000) == 0x8000) )
		{	//	residue 단위의 toggle
			CResidueInst * pResidueInst = arrayPickAtomInst[minIndexAtom].m_pAtomInst->m_pResidueInst;
			m_pLastPickObjectInst = pResidueInst;
			pResidueInst->SetSelectChild(!(pResidueInst->GetSelect()));

			SetActivePDBRenderer(pResidueInst->m_pChainInst->m_pPDBInst->m_pPDBRenderer);
			pResidueInst->m_pChainInst->m_pPDBInst->m_pPDBRenderer->UpdatePDBRendererCenter();
		}
		else
		if( (GetAsyncKeyState( VK_LCONTROL )& 0x8000) == 0x8000 )
		{	//	atom 단위의 toggle.
			arrayPickAtomInst[minIndexAtom].m_pAtomInst->SetSelect(!(arrayPickAtomInst[minIndexAtom].m_pAtomInst->GetSelect()));
			m_pLastPickObjectInst = arrayPickAtomInst[minIndexAtom].m_pAtomInst;

			SetActivePDBRenderer(arrayPickAtomInst[minIndexAtom].m_pAtomInst->m_pPDBInst->m_pPDBRenderer);
			arrayPickAtomInst[minIndexAtom].m_pAtomInst->m_pPDBInst->m_pPDBRenderer->UpdatePDBRendererCenter();
		}
		else
		if( (GetAsyncKeyState( VK_LSHIFT )& 0x8000) == 0x8000 )
		{	//	shit를 누르고 선택하면 레지듀 단위로 추가.
			//	모두 deselect.
			SelectAll(FALSE);

			CResidueInst * pResidueInst = arrayPickAtomInst[minIndexAtom].m_pAtomInst->m_pResidueInst;
			pResidueInst->SetSelectChild(TRUE);
			m_pLastPickObjectInst = pResidueInst;

			SetActivePDBRenderer(pResidueInst->m_pChainInst->m_pPDBInst->m_pPDBRenderer);
			pResidueInst->m_pChainInst->m_pPDBInst->m_pPDBRenderer->UpdatePDBRendererCenter();
		}
		else
		{	//	마우스로 1개만 선택.
			//	모두 deselect.
			SelectAll(FALSE);

			arrayPickAtomInst[minIndexAtom].m_pAtomInst->SetSelect(TRUE);
			m_pLastPickObjectInst = arrayPickAtomInst[minIndexAtom].m_pAtomInst;

			SetActivePDBRenderer(arrayPickAtomInst[minIndexAtom].m_pAtomInst->m_pPDBInst->m_pPDBRenderer);
			arrayPickAtomInst[minIndexAtom].m_pAtomInst->m_pPDBInst->m_pPDBRenderer->UpdatePDBRendererCenter();
		}

		m_SelectAtom = arrayPickAtomInst[minIndexAtom];
		bSelectionChanged = TRUE;
	}

	if ( bSelectionChanged == TRUE )
	{
		//	pdb 에서 select 된것을 Pane 에 반영
		UpdateSelectionInfoPane();

		GetMainActiveView()->GetSelectPanel()->DeselectPaneItem();
		 
		//	렌더러에 반영
		UpdateAtomSelectionChanged();

		g_bRequestRender = TRUE;
	}
	
	return bSelectionChanged;
}



