//--------------------------------------------------------------------------------------
// File: DXUTMisc.cpp
//
// Shortcut macros and functions for using DX objects
//
// Copyright (c) Microsoft Corporation. All rights reserved
//--------------------------------------------------------------------------------------
#include "stdafx.h"
#include "DXUTmisc.h"


#define DXUT_GAMEPAD_TRIGGER_THRESHOLD      30
#undef min // use __min instead
#undef max // use __max instead

HRESULT DXUTCreateArrowMeshFromInternalArray( LPDIRECT3DDEVICE9 pd3dDevice, ID3DXMesh** ppMesh );

//--------------------------------------------------------------------------------------
CD3D10ArcBall::CD3D10ArcBall()
{
    Reset();
    m_vDownPt = D3DXVECTOR3(0,0,0);
    m_vCurrentPt = D3DXVECTOR3(0,0,0);
    m_Offset.x = m_Offset.y = 0;

    RECT rc;
    GetClientRect( GetForegroundWindow(), &rc );
    SetWindow( rc.right, rc.bottom );
}





//--------------------------------------------------------------------------------------
void CD3D10ArcBall::Reset()
{
    D3DXQuaternionIdentity( &m_qDown );
    D3DXQuaternionIdentity( &m_qNow );
    D3DXMatrixIdentity( &m_mRotation );
    D3DXMatrixIdentity( &m_mTranslation );
    D3DXMatrixIdentity( &m_mTranslationDelta );
    m_bDrag = FALSE;
    m_fRadiusTranslation = 1.0f;
    m_fRadius = 1.0f;
}




//--------------------------------------------------------------------------------------
D3DXVECTOR3 CD3D10ArcBall::ScreenToVector( float fScreenPtX, float fScreenPtY )
{
    // Scale to screen
    FLOAT x   = -(fScreenPtX - m_Offset.x - m_nWidth/2)  / (m_fRadius*m_nWidth/2);
    FLOAT y   =  (fScreenPtY - m_Offset.y - m_nHeight/2) / (m_fRadius*m_nHeight/2);

    FLOAT z   = 0.0f;
    FLOAT mag = x*x + y*y;

    if( mag > 1.0f )
    {
        FLOAT scale = 1.0f/sqrtf(mag);
        x *= scale;
        y *= scale;
    }
    else
        z = sqrtf( 1.0f - mag );

    // Return vector
    return D3DXVECTOR3( x, y, z );
}




//--------------------------------------------------------------------------------------
D3DXQUATERNION CD3D10ArcBall::QuatFromBallPoints(const D3DXVECTOR3 &vFrom, const D3DXVECTOR3 &vTo)
{
    D3DXVECTOR3 vPart;
    float fDot = D3DXVec3Dot(&vFrom, &vTo);
    D3DXVec3Cross(&vPart, &vFrom, &vTo);

    return D3DXQUATERNION(vPart.x, vPart.y, vPart.z, fDot);
}




//--------------------------------------------------------------------------------------
void CD3D10ArcBall::OnBegin( int nX, int nY )
{
    // Only enter the drag state if the click falls
    // inside the click rectangle.
    if( nX >= m_Offset.x &&
        nX < m_Offset.x + m_nWidth &&
        nY >= m_Offset.y &&
        nY < m_Offset.y + m_nHeight )
    {
        m_bDrag = true;
        m_qDown = m_qNow;
        m_vDownPt = ScreenToVector( (float)nX, (float)nY );
    }
}




//--------------------------------------------------------------------------------------
void CD3D10ArcBall::OnMove( int nX, int nY )
{
    if (m_bDrag) 
    { 
        m_vCurrentPt = ScreenToVector( (float)nX, (float)nY );
        m_qNow = m_qDown * QuatFromBallPoints( m_vDownPt, m_vCurrentPt );
    }
}




//--------------------------------------------------------------------------------------
void CD3D10ArcBall::OnEnd()
{
    m_bDrag = false;
}




//--------------------------------------------------------------------------------------
// Desc:
//--------------------------------------------------------------------------------------
LRESULT CD3D10ArcBall::HandleMessages( HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam )
{
    // Current mouse position
    int iMouseX = (short)LOWORD(lParam);
    int iMouseY = (short)HIWORD(lParam);

    switch( uMsg )
    {
        case WM_LBUTTONDOWN:
        case WM_LBUTTONDBLCLK:
            SetCapture( hWnd );
            OnBegin( iMouseX, iMouseY );
            return TRUE;

        case WM_LBUTTONUP:
            ReleaseCapture();
            OnEnd();
            return TRUE;

        case WM_CAPTURECHANGED:
            if( (HWND)lParam != hWnd )
            {
                ReleaseCapture();
                OnEnd();
            }
            return TRUE;

        case WM_RBUTTONDOWN:
        case WM_RBUTTONDBLCLK:
        case WM_MBUTTONDOWN:
        case WM_MBUTTONDBLCLK:
            SetCapture( hWnd );
            // Store off the position of the cursor when the button is pressed
            m_ptLastMouse.x = iMouseX;
            m_ptLastMouse.y = iMouseY;
            return TRUE;

        case WM_RBUTTONUP:
        case WM_MBUTTONUP:
            ReleaseCapture();
            return TRUE;

        case WM_MOUSEMOVE:
            if( MK_LBUTTON&wParam )
            {
                OnMove( iMouseX, iMouseY );
            }
            else if( (MK_RBUTTON&wParam) || (MK_MBUTTON&wParam) )
            {
                // Normalize based on size of window and bounding sphere radius
                FLOAT fDeltaX = ( m_ptLastMouse.x-iMouseX ) * m_fRadiusTranslation / m_nWidth / 20;
                FLOAT fDeltaY = ( m_ptLastMouse.y-iMouseY ) * m_fRadiusTranslation / m_nHeight / 20;

                if( wParam & MK_RBUTTON )
                {
                    D3DXMatrixTranslation( &m_mTranslationDelta, -2*fDeltaX, 2*fDeltaY, 0.0f );
                    D3DXMatrixMultiply( &m_mTranslation, &m_mTranslation, &m_mTranslationDelta );
                }
                else  // wParam & MK_MBUTTON
                {
                    D3DXMatrixTranslation( &m_mTranslationDelta, 0.0f, 0.0f, 5*fDeltaY );
                    D3DXMatrixMultiply( &m_mTranslation, &m_mTranslation, &m_mTranslationDelta );
                }

                // Store mouse coordinate
                m_ptLastMouse.x = iMouseX;
                m_ptLastMouse.y = iMouseY;
            }
            return TRUE;
    }

    return FALSE;
}



//--------------------------------------------------------------------------------------
IDirect3DDevice9* CDXUTDirectionWidget::s_pd3dDevice = NULL;
ID3DXEffect*      CDXUTDirectionWidget::s_pEffect = NULL;       
ID3DXMesh*        CDXUTDirectionWidget::s_pMesh = NULL;    

#pragma managed(push,off)
//--------------------------------------------------------------------------------------
CDXUTDirectionWidget::CDXUTDirectionWidget()
{
    m_fRadius = 1.0f;
    m_vDefaultDir = D3DXVECTOR3(0,1,0);
    m_vCurrentDir = m_vDefaultDir;
    m_nRotateMask = MOUSE_LEFT_BUTTON;

    D3DXMatrixIdentity( &m_mView );
    D3DXMatrixIdentity( &m_mRot );
    D3DXMatrixIdentity( &m_mRotSnapshot );
	D3DXMatrixIdentity( &m_matWorld );
}
#pragma managed(pop)

//--------------------------------------------------------------------------------------
HRESULT CDXUTDirectionWidget::StaticOnCreateDevice( IDirect3DDevice9* pd3dDevice )
{
    HRESULT hr;

    s_pd3dDevice = pd3dDevice;

    const char* g_strBuffer = 
    "float4 g_MaterialDiffuseColor;      // Material's diffuse color\r\n"
    "float3 g_LightDir;                  // Light's direction in world space\r\n"
    "float4x4 g_mWorld;                  // World matrix for object\r\n"
    "float4x4 g_mWorldViewProjection;    // World * View * Projection matrix\r\n"
    "\r\n"
    "struct VS_OUTPUT\r\n"
    "{\r\n"
    "    float4 Position   : POSITION;   // vertex position\r\n"
    "    float4 Diffuse    : COLOR0;     // vertex diffuse color\r\n"
    "};\r\n"
    "\r\n"
    "VS_OUTPUT RenderWith1LightNoTextureVS( float4 vPos : POSITION,\r\n"
    "                                       float3 vNormal : NORMAL )\r\n"
    "{\r\n"
    "    VS_OUTPUT Output;\r\n"
    "\r\n"
    "    // Transform the position from object space to homogeneous projection space\r\n"
    "    Output.Position = mul(vPos, g_mWorldViewProjection);\r\n"
    "\r\n"
    "    // Transform the normal from object space to world space\r\n"
    "    float3 vNormalWorldSpace;\r\n"
    "    vNormalWorldSpace = normalize(mul(vNormal, (float3x3)g_mWorld)); // normal (world space)\r\n"
    "\r\n"
    "    // Compute simple directional lighting equation\r\n"
    "    Output.Diffuse.rgb = g_MaterialDiffuseColor * max(0,dot(vNormalWorldSpace, g_LightDir));\r\n"
    "    Output.Diffuse.a = 0.0f;\r\n"
    "\r\n"
    "    return Output;\r\n"
    "}\r\n"
    "\r\n"
    "float4 RenderWith1LightNoTexturePS( float4 Diffuse : COLOR0 ) : COLOR0\r\n"
    "{\r\n"
    "    return Diffuse;\r\n"
    "}\r\n"
    "\r\n"
    "technique RenderWith1LightNoTexture\r\n"
    "{\r\n"
    "    pass P0\r\n"
    "    {\r\n"
    "        VertexShader = compile vs_1_1 RenderWith1LightNoTextureVS();\r\n"
    "        PixelShader  = compile ps_2_0 RenderWith1LightNoTexturePS();\r\n"
    "    }\r\n"
    "}\r\n"
    "";

    UINT dwBufferSize = (UINT)strlen(g_strBuffer) + 1; 

    V_RETURN( D3DXCreateEffect( s_pd3dDevice, g_strBuffer, dwBufferSize, NULL, NULL, D3DXFX_NOT_CLONEABLE, NULL, &s_pEffect, NULL ) );

    // Load the mesh with D3DX and get back a ID3DXMesh*.  For this
    // sample we'll ignore the X file's embedded materials since we know 
    // exactly the model we're loading.  See the mesh samples such as
    // "OptimizedMesh" for a more generic mesh loading example.
    V_RETURN( DXUTCreateArrowMeshFromInternalArray( s_pd3dDevice, &s_pMesh ) );

    // Optimize the mesh for this graphics card's vertex cache 
    // so when rendering the mesh's triangle list the vertices will 
    // cache hit more often so it won't have to re-execute the vertex shader 
    // on those vertices so it will improve perf.     
    DWORD* rgdwAdjacency = new DWORD[s_pMesh->GetNumFaces() * 3];
    if( rgdwAdjacency == NULL )
        return E_OUTOFMEMORY;
    V( s_pMesh->GenerateAdjacency(1e-6f,rgdwAdjacency) );
    V( s_pMesh->OptimizeInplace(D3DXMESHOPT_VERTEXCACHE, rgdwAdjacency, NULL, NULL, NULL) );
    delete []rgdwAdjacency;

    return S_OK;
}


//--------------------------------------------------------------------------------------
HRESULT CDXUTDirectionWidget::OnResetDevice( long width, long height )
{
    m_ArcBall.SetWindow( width, height );
    return S_OK;
}


//--------------------------------------------------------------------------------------
void CDXUTDirectionWidget::StaticOnLostDevice()
{
    if( s_pEffect )
        s_pEffect->OnLostDevice();
}


//--------------------------------------------------------------------------------------
void CDXUTDirectionWidget::StaticOnDestroyDevice()
{
    SAFE_RELEASE(s_pEffect);
    SAFE_RELEASE(s_pMesh);
}    


//--------------------------------------------------------------------------------------
LRESULT CDXUTDirectionWidget::HandleMessages( HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam )
{
    switch( uMsg )
    {
        case WM_LBUTTONDOWN:
        case WM_MBUTTONDOWN:
        case WM_RBUTTONDOWN:
        {
			SHORT keyState = GetAsyncKeyState(VK_LSHIFT)>>15;
            if( ((m_nRotateMask & MOUSE_LEFT_BUTTON) != 0 && uMsg == WM_LBUTTONDOWN) ||
                ((m_nRotateMask & MOUSE_MIDDLE_BUTTON) != 0 && uMsg == WM_MBUTTONDOWN) ||
                ((m_nRotateMask & MOUSE_RIGHT_BUTTON) != 0 && uMsg == WM_RBUTTONDOWN) )
            {
				if ( keyState )
				{
					int iMouseX = (int)(short)LOWORD(lParam);
					int iMouseY = (int)(short)HIWORD(lParam);
					m_ArcBall.OnBegin( iMouseX, iMouseY );
					SetCapture(hWnd);
					return TRUE;
				}
            }
        }
		break;

        case WM_MOUSEMOVE:
        {
            if( m_ArcBall.IsBeingDragged() )
            {
                int iMouseX = (int)(short)LOWORD(lParam);
                int iMouseY = (int)(short)HIWORD(lParam);
                m_ArcBall.OnMove( iMouseX, iMouseY );
                UpdateLightDir();
				return TRUE;
            }
        }

		break;
        case WM_LBUTTONUP:
        case WM_MBUTTONUP:
        case WM_RBUTTONUP:
        {
            if( ((m_nRotateMask & MOUSE_LEFT_BUTTON) != 0 && uMsg == WM_LBUTTONUP) ||
                ((m_nRotateMask & MOUSE_MIDDLE_BUTTON) != 0 && uMsg == WM_MBUTTONUP) ||
                ((m_nRotateMask & MOUSE_RIGHT_BUTTON) != 0 && uMsg == WM_RBUTTONUP) )
            {
				m_ArcBall.OnEnd();
				ReleaseCapture();
				UpdateLightDir();
				return TRUE;
            }
        }

		break;

        case WM_CAPTURECHANGED:
        {
            if( (HWND)lParam != hWnd )
            {
                if( (m_nRotateMask & MOUSE_LEFT_BUTTON) ||
                    (m_nRotateMask & MOUSE_MIDDLE_BUTTON) ||
                    (m_nRotateMask & MOUSE_RIGHT_BUTTON) )
                {
                    m_ArcBall.OnEnd();
                    ReleaseCapture();
					return TRUE;
                }
            }
        }

		break;

    }

    return 0;
}

#pragma managed(push,off)
//--------------------------------------------------------------------------------------
HRESULT CDXUTDirectionWidget::OnRender( D3DXCOLOR color, const D3DXMATRIX* pmView, 
                                        const D3DXMATRIX* pmProj, const D3DXVECTOR3* pEyePt )
{
    m_mView = *pmView;

    // Render the light spheres so the user can visually see the light dir
    UINT iPass, cPasses;
    D3DXMATRIX mRotate;
    D3DXMATRIX mScale;
    D3DXMATRIX mTrans;
    D3DXMATRIXA16 mWorldViewProj;
    HRESULT hr;

    V( s_pEffect->SetTechnique( "RenderWith1LightNoTexture" ) );
    V( s_pEffect->SetVector( "g_MaterialDiffuseColor", (D3DXVECTOR4*)&color ) );

    D3DXVECTOR3 vEyePt;
    D3DXVec3Normalize( &vEyePt, pEyePt );
    V( s_pEffect->SetValue( "g_LightDir", &vEyePt, sizeof(D3DXVECTOR3) ) );

    // Rotate arrow model to point towards origin
    D3DXMATRIX mRotateA, mRotateB;
    D3DXVECTOR3 vAt = D3DXVECTOR3(0,0,0);
    D3DXVECTOR3 vUp = D3DXVECTOR3(0,1,0);
    D3DXMatrixRotationX( &mRotateB, D3DX_PI );
    D3DXMatrixLookAtLH( &mRotateA, &m_vCurrentDir, &vAt, &vUp );
    D3DXMatrixInverse( &mRotateA, NULL, &mRotateA );
    mRotate = mRotateB * mRotateA;

    D3DXVECTOR3 vL = m_vCurrentDir * m_fRadius * 1.0f;
    D3DXMatrixTranslation( &mTrans, vL.x, vL.y, vL.z );
    D3DXMatrixScaling( &mScale, m_fRadius*0.2f, m_fRadius*0.2f, m_fRadius*0.2f );

    D3DXMATRIX mWorld = mRotate * mScale * mTrans;
	m_matWorld = mWorld;
    mWorldViewProj = mWorld * (m_mView) * (*pmProj);

    V( s_pEffect->SetMatrix( "g_mWorldViewProjection", &mWorldViewProj ) );
    V( s_pEffect->SetMatrix( "g_mWorld", &mWorld ) );

    for( int iSubset=0; iSubset<2; iSubset++ )
    {
        V( s_pEffect->Begin(&cPasses, 0) );
        for (iPass = 0; iPass < cPasses; iPass++)
        {
            V( s_pEffect->BeginPass(iPass) );

			if ( m_ArcBall.IsBeingDragged() == TRUE )
				CDXUTDirectionWidget::s_pd3dDevice->SetRenderState(D3DRS_SHADEMODE, D3DSHADE_FLAT );
			
			V( s_pMesh->DrawSubset(iSubset) );

			if ( m_ArcBall.IsBeingDragged() == TRUE )	
				CDXUTDirectionWidget::s_pd3dDevice->SetRenderState(D3DRS_SHADEMODE, D3DSHADE_GOURAUD );
			
            V( s_pEffect->EndPass() );
        }
        V( s_pEffect->End() );
    }

    return S_OK;
}
#pragma managed(pop)

//--------------------------------------------------------------------------------------
HRESULT CDXUTDirectionWidget::UpdateLightDir()
{
    D3DXMATRIX mInvView;
    D3DXMatrixInverse(&mInvView, NULL, &m_mView);
    mInvView._41 = mInvView._42 = mInvView._43 = 0;

    D3DXMATRIX mLastRotInv;
    D3DXMatrixInverse(&mLastRotInv, NULL, &m_mRotSnapshot);

    D3DXMATRIX mRot = *m_ArcBall.GetRotationMatrix();
    m_mRotSnapshot = mRot;

    // Accumulate the delta of the arcball's rotation in view space.
    // Note that per-frame delta rotations could be problematic over long periods of time.
    m_mRot *= m_mView * mLastRotInv * mRot * mInvView;

    // Since we're accumulating delta rotations, we need to orthonormalize 
    // the matrix to prevent eventual matrix skew
    D3DXVECTOR3* pXBasis = (D3DXVECTOR3*) &m_mRot._11;
    D3DXVECTOR3* pYBasis = (D3DXVECTOR3*) &m_mRot._21;
    D3DXVECTOR3* pZBasis = (D3DXVECTOR3*) &m_mRot._31;
    D3DXVec3Normalize( pXBasis, pXBasis );
    D3DXVec3Cross( pYBasis, pZBasis, pXBasis );
    D3DXVec3Normalize( pYBasis, pYBasis );
    D3DXVec3Cross( pZBasis, pXBasis, pYBasis );

    // Transform the default direction vector by the light's rotation matrix
    D3DXVec3TransformNormal( &m_vCurrentDir, &m_vDefaultDir, &m_mRot );

    return S_OK;
}

static const DWORD g_DXUTArrowMeshSrcData[] =
{
	0x20666f78, 0x33303330, 0x70697a62, 0x32333030, 0x000030d7, 0x087930c7, 0x59ed4b43, 0xd51c6c5d, 
	0x71dbbe15, 0xacbbc1d6, 0xe125d493, 0xc1024e27, 0x7133f9c1, 0xec1098ec, 0x1b1daef1, 0xc6d24eb7, 
	0xc713fa10, 0x64866f59, 0xd9aecd95, 0x694304dd, 0x4485686b, 0xad2fb6a5, 0x78df44a8, 0x6cca8828, 
	0x20a2895a, 0x2aaf60ef, 0x905215b5, 0x4fa9515a, 0xa45b4d45, 0xa1e09548, 0x9e94d282, 0xcf5df333, 
	0x8a3acdf1, 0xec0c5368, 0xbefec6ac, 0xf77ee733, 0xdef73bdc, 0x9d667739, 0x4a65a90a, 0xf27c81a9, 
	0x7040d394, 0xba2785e0, 0x7e7e38ff, 0xd5bb0be5, 0xba1f17fa, 0x56a633b0, 0x1546bf79, 0x1d7cec51, 
	0x164bef39, 0x8ec9e9f3, 0xb02e0ec9, 0x1fe77ed7, 0xbf399e4f, 0x550707f2, 0x1fceceef, 0x36a1db3e, 
	0xdc985b93, 0x316e794e, 0xc415a965, 0x2413551a, 0xf77d749a, 0xc2d993e5, 0x29fcb983, 0xe2d5905f, 
	0x61519a86, 0x7a942488, 0x36fb0b90, 0x1df5c9ad, 0x5cc55272, 0x188e559e, 0x4288e08d, 0x1aee41e4, 
	0x47c0123e, 0xae72c8b5, 0x189c29ca, 0xbebca648, 0x5a643318, 0xf91567c9, 0x90e1d964, 0x933a2f93, 
	0xe963d943, 0x52685394, 0xaeaf8a1d, 0x8bcce8e4, 0x058b45b1, 0xaa87527d, 0x2afedab9, 0x642f72d5, 
	0x4daeacb9, 0xfc8fd936, 0x4cc6ae0c, 0x15a5ae77, 0x2018bc05, 0x8feb9b20, 0x2bdd0e02, 0x6172b81f, 
	0x50440c73, 0xbadd3f70, 0x31c0a664, 0xadf719f7, 0xf859fa15, 0x3d721e5c, 0xc62ff55f, 0x701d670c, 
	0x40b14754, 0x11d86164, 0x903fbd84, 0xbd058b32, 0x97d2ddcc, 0x07f2e4cb, 0x0e711307, 0x1ffd4ffb, 
	0x09963e74, 0x6121e6c2, 0x39de5391, 0xee0f8fe6, 0xc779e395, 0x6f94fac9, 0xb4e678d3, 0x664c5c13, 
	0xe5244e26, 0x2cfad200, 0xcdf678ff, 0x7e4e75df, 0x20317918, 0x9f274593, 0x6f58664e, 0x8c32319f, 
	0x7b94f916, 0x550533c3, 0x761ee44c, 0x300c2e88, 0x5627aea4, 0xa7527098, 0x4a3cc933, 0x4e717354, 
	0xf4850ba5, 0x8e6a2d19, 0x1fb81586, 0x74e1d03a, 0x26c1c8a1, 0x06890caa, 0xb1da35be, 0x9a65dc60, 
	0x3064ce14, 0x859df497, 0xd5c55639, 0xfb0b9c1f, 0xd25ef2e9, 0x46f1ae81, 0xfebf4f1f, 0x749913d3, 
	0xe74c592a, 0x1c8998b2, 0x6ffdd0f0, 0x3f9bfcbc, 0xa9e868fc, 0x3fcf23f3, 0xe7f1da57, 0x3f3c31f9, 
	0xcf14f5c9, 0x8fa7c81e, 0x8cc9b3a7, 0x917c919c, 0x983a7507, 0x3fb4b2f5, 0xeb6ac50a, 0xca48c6f7, 
	0x07633ac9, 0xb3ea3973, 0xf58d2d2b, 0x352b3f5e, 0x8ecbab65, 0x0119b7ab, 0xf3e78c30, 0xa6af87b9, 
	0xfddef7ee, 0xe5551ece, 0xafc27690, 0x65c7aaa8, 0xfbe37375, 0x6f50c91e, 0x773bf939, 0x9bc6f87c, 
	0xa35729f6, 0xaef0ae99, 0x256f73b6, 0x00957ff8, 0x7d46b8f8, 0x7e7fc53e, 0x5f343232, 0x75de5ead, 
	0xc1ef61ad, 0xf59d3773, 0x3950da6a, 0x35c4aa5e, 0x8a503e1e, 0x3adebfd7, 0xb2c778cb, 0x7f9dfc62, 
	0x97c3d8ff, 0x8ef74db6, 0x61ecbaff, 0xd5fa7db5, 0xb2fb7024, 0xd9f6ace2, 0xab34a077, 0xece692cb, 
	0x9d93d74c, 0xfe5d9cd0, 0x4757ab7a, 0xe1efef9b, 0xbf4db7f7, 0x68719a4a, 0x498126f6, 0x7c36fe33, 
	0x461e3348, 0x68ce20d9, 0x5a338825, 0x568ce209, 0x95a33882, 0x2568ce20, 0x095a3388, 0x82568ce2, 
	0xc44d3d38, 0x7104ad19, 0x9c412b46, 0xf4e2269e, 0xa7a71134, 0x67c93889, 0x6bf4f0fe, 0xec3ffb7d, 
	0x937ab0db, 0x4e8bbc62, 0x19fabd0b, 0x5b82433a, 0x075e69b0, 0x6132f9ac, 0x9b0c23e3, 0x69b0f5e6, 
	0xcd361cbe, 0xf34d83af, 0x8f34d806, 0x1479a6c0, 0x87afcd36, 0x6c18f34d, 0x6360879a, 0x9fa74cde, 
	0x1d7c06ca, 0x4af0166c, 0xcd87af83, 0xf0655e02, 0xc059b00d, 0xadf8359f, 0x4dc059b0, 0x360dbf06, 
	0x0673f80b, 0x059b08df, 0x6fc1837c, 0x5e02cd87, 0xd816f06d, 0x0635e02c, 0x059b04df, 0xefc1a37c, 
	0xbe02cd80, 0xb0ade0c9, 0x1b37c059, 0x166c337c, 0xdf062df0, 0xdc059b02, 0x7166f068, 0xb9d2d6bf, 
	0x826dce95, 0x382ede33, 0x19c136e3, 0xe33829df, 0x6e33821d, 0x4ef19c17, 0x087719c1, 0x7053b8ce, 
	0x6704bbc6, 0x977825dc, 0x816b5f98, 0x1bbe0b77, 0x9592aa6d, 0x5990fe2f, 0xd25cbcf0, 0xb7df7d78, 
	0x9f46a7ec, 0xe17666e9, 0x7dced644, 0xf1995cfb, 0xf6eda1e0, 0x78f667b3, 0x2607facf, 0xf479ee9f, 
	0xd5e045bb, 0x6fb686e5, 0xddffb878, 0x7a707746, 0x7f4fc3c4, 0xf4cdd47f, 0xa6930f2a, 0xd0339a4b, 
	0x07b34974, 0xda692e9a, 0x26932cd0, 0xd21eeaf0, 0x9369a1b4, 0x9b4d0da6, 0x60448d34, 0x33cc45ca, 
	0x6515c7c2, 0x8ae9a15d, 0x83115cba, 0x8b2b622b, 0x5b45bb98, 0x5b46d16c, 0x7d16ddb4, 0xb2be8b62, 
	0x98f61f45, 0x5c652f31, 0x1ed4c87b, 0xcc87b532, 0x44c87852, 0x87eedd16, 0x084ce7bd, 0x87e93f65, 
	0xdea666f9, 0xb3feed9b, 0x2da69e4b, 0x5f5e2238, 0x448df95e, 0xe783dd58, 0x2758b377, 0x87573af4, 
	0xb58c2c35, 0xa4d6396a, 0x513dac06, 0x8c3131ac, 0x135815cd, 0x376e6156, 0x80de8e61, 0x7306b6b9, 
	0xfd8c26e6, 0xffef705b, 0xf04675df, 0x0f57b05b, 0xb2a52f7b, 0x5179b5ce, 0xced6ede0, 0xe45895bd, 
	0x7c35c54d, 0xeeaea051, 0xd7b8c145, 0x85a12ddf, 0x274a972a, 0x9ec71a13, 0xa10a254f, 0x1c69b7a2, 
	0xb89c3b5d, 0xe1e8ba54, 0x24b5cfb3, 0xe3274e7e, 0x08ed917f, 0x413c635d, 0xf5065cbe, 0x4eb5a4bf, 
	0xdab38a87, 0xe2e941fe, 0x49fc28ec, 0x742cee63, 0x7dee6348, 0x5cef93e8, 0xc692f516, 0xf57ecfdc, 
	0x7db497d3, 0xcf10b492, 0xd17b3a9c, 0xf9d225c3, 0xff8c9d05, 0xc5acfa49, 0x36f924f8, 0xb7bc1952, 
	0xc62d62e9, 0xf4e3f75f, 0xc5ece870, 0x93f861f8, 0x9d04a246, 0xbdf18a31, 0x1a4faf92, 0x57a81389, 
	0x6fe7db1f, 0x862f0d6d, 0x36a9636b, 0x1c1bfe96, 0xead38f56, 0x9c13f474, 0x1c8a37a3, 0xba39195d, 
	0x65727232, 0xc8cae0e4, 0x639195b9, 0x56a7232b, 0x8cad0e46, 0x3919599c, 0x740dceb2, 0xe46edc4e, 
	0xd45f3ac0, 0x234eef39, 0xa3f9d607, 0x8c3b79ce, 0x93e7581c, 0x44ede73a, 0x2ef9206e, 0x66e02cd8, 
	0x66c17783, 0xf833af01, 0xc059b05d, 0x1bbe0deb, 0x83780b36, 0xcd86efc1, 0xe0d5be02, 0x80b3607b, 
	0x3df8336f, 0x1bc059b0, 0x9b0dde0d, 0xc1bb7c05, 0x80b3607b, 0xc36e0c5b, 0x326f0166, 0x166c2f78, 
	0x8f061df0, 0xb70166c3, 0x9b013c1a, 0xe0d9bc05, 0xc059b07d, 0x0fde0c5b, 0xd5bc059b, 0xb3617be0, 
	0x7c19b780, 0x0679ec40, 0x7c91c7b0, 0x0681ec50, 0xfc91e7b1, 0xba27b15e, 0xc9207b17, 0x691ec487, 
	0xf25b6308, 0x79de3176, 0x3a79f4bf, 0xe33820bf, 0xef19c161, 0x7ef19c13, 0x101f19c1, 0x0517f19c, 
	0xc125fc67, 0x9c141f19, 0x8ce088f1, 0xf19c13ef, 0x3e3382fd, 0x07e33824, 0x65fc6704, 0x457f19c1, 
	0x155fc670, 0x04d7f19c, 0x70587c67, 0x3382a3c6, 0xe338223e, 0x1f19c131, 0x52719c15, 0x7cfa24f0, 
	0xd59117d1, 0xfda5d892, 0xe02969e2, 0xb71fbffd, 0xdfebfe5f, 0xe5fede67, 0xbb807a5f, 0x46c64719, 
	0x5f9b96ec, 0xf1dc61ec, 0xdafda744, 0xcc66c440, 0xae3399ff, 0xc7eceba3, 0x7971cb98, 0xc98d19e0, 
	0xa87daca7, 0xd2cbabac, 0xd26b577f, 0x750eb775, 0xc7573951, 0x7c233c00, 0x2baca2ca, 0x97515d34, 
	0x4570622b, 0xf16e1e6c, 0x5b457663, 0x5746d15c, 0x25d75db7, 0xa58f5337, 0xcb78798e, 0xe27adff0, 
	0x6fb61ccf, 0x187a67cc, 0x5eedfe1f, 0x0ba97bfb, 0x9be57e1e, 0x193e6c1b, 0x31af8db3, 0xc32e5f66, 
	0x5ec16b58, 0x2ff276d6, 0x70d27f6b, 0xdcd90eb5, 0x5f27d093, 0x1a4bd757, 0x84a72f7c, 0x59fdacbd, 
	0x46bceb4b, 0x4577d0a7, 0xc788aebd, 0x22ba7115, 0xcbb4577e, 0x093dedd5, 0x8dfd8e7d, 0x3f7afc87, 
	0x3ffe2f4e, 0x624b5c0a, 0x2d6c496b, 0xb125ad89, 0xd3f624b5, 0xe95ec49e, 0xd638c743, 0x829fa3a9, 
	0x0fb51d0f, 0x9887fe31, 0xb4bfd863, 0x621f847f, 0xdec7c59a, 0xecc3e20d, 0x7511f146, 0xa9e0f893, 
	0x156e5c3f, 0xc4db201f, 0x7b8fc5c7, 0xf7e2221b, 0x0000001f
};

static const UINT g_DXUTArrowMeshSrcDataSizeInBytes = 2193;

//-----------------------------------------------------------------------------
HRESULT DXUTCreateArrowMeshFromInternalArray( LPDIRECT3DDEVICE9 pd3dDevice, ID3DXMesh** ppMesh )
{
	return D3DXLoadMeshFromXInMemory( g_DXUTArrowMeshSrcData, g_DXUTArrowMeshSrcDataSizeInBytes, 
		D3DXMESH_MANAGED, pd3dDevice, NULL, NULL, NULL, NULL, ppMesh );
}

