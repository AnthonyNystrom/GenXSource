//--------------------------------------------------------------------------------------
// File: DXUTMisc.h
//
// Helper functions for Direct3D programming.
//
// Copyright (c) Microsoft Corporation. All rights reserved
//--------------------------------------------------------------------------------------
#pragma once

#ifndef DXUT_MISC_H
#define DXUT_MISC_H
 
//--------------------------------------------------------------------------------------
class CD3D10ArcBall
{
public:
    CD3D10ArcBall();

    // Functions to change behavior
    void Reset(); 
    void SetTranslationRadius( FLOAT fRadiusTranslation ) { m_fRadiusTranslation = fRadiusTranslation; }
    void SetWindow( INT nWidth, INT nHeight, FLOAT fRadius = 0.9f ) { m_nWidth = nWidth; m_nHeight = nHeight; m_fRadius = fRadius; m_vCenter = D3DXVECTOR2(m_nWidth/2.0f,m_nHeight/2.0f); }
    void SetOffset( INT nX, INT nY ) { m_Offset.x = nX; m_Offset.y = nY; }

    // Call these from client and use GetRotationMatrix() to read new rotation matrix
    void OnBegin( int nX, int nY );  // start the rotation (pass current mouse position)
    void OnMove( int nX, int nY );   // continue the rotation (pass current mouse position)
    void OnEnd();                    // end the rotation 

    // Or call this to automatically handle left, middle, right buttons
    LRESULT     HandleMessages( HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam );

    // Functions to get/set state
    const D3DXMATRIX* GetRotationMatrix()                   { return D3DXMatrixRotationQuaternion(&m_mRotation, &m_qNow); };
    const D3DXMATRIX* GetTranslationMatrix() const          { return &m_mTranslation; }
    const D3DXMATRIX* GetTranslationDeltaMatrix() const     { return &m_mTranslationDelta; }
    bool        IsBeingDragged() const                      { return m_bDrag; }
    D3DXQUATERNION GetQuatNow() const                       { return m_qNow; }
    void        SetQuatNow( D3DXQUATERNION q ) { m_qNow = q; }

    static D3DXQUATERNION QuatFromBallPoints( const D3DXVECTOR3 &vFrom, const D3DXVECTOR3 &vTo );


public:
    D3DXMATRIXA16  m_mRotation;         // Matrix for arc ball's orientation
    D3DXMATRIXA16  m_mTranslation;      // Matrix for arc ball's position
    D3DXMATRIXA16  m_mTranslationDelta; // Matrix for arc ball's position

    POINT          m_Offset;   // window offset, or upper-left corner of window
    INT            m_nWidth;   // arc ball's window width
    INT            m_nHeight;  // arc ball's window height
    D3DXVECTOR2    m_vCenter;  // center of arc ball 
    FLOAT          m_fRadius;  // arc ball's radius in screen coords
    FLOAT          m_fRadiusTranslation; // arc ball's radius for translating the target

    D3DXQUATERNION m_qDown;             // Quaternion before button down
    D3DXQUATERNION m_qNow;              // Composite quaternion for current drag
    bool           m_bDrag;             // Whether user is dragging arc ball

    POINT          m_ptLastMouse;      // position of last mouse point
    D3DXVECTOR3    m_vDownPt;           // starting point of rotation arc
    D3DXVECTOR3    m_vCurrentPt;        // current point of rotation arc

    D3DXVECTOR3    ScreenToVector( float fScreenPtX, float fScreenPtY );
};


//--------------------------------------------------------------------------------------
// used by CCamera to map WM_KEYDOWN keys
//--------------------------------------------------------------------------------------

#define KEY_WAS_DOWN_MASK 0x80
#define KEY_IS_DOWN_MASK  0x01

#define MOUSE_LEFT_BUTTON   0x01
#define MOUSE_MIDDLE_BUTTON 0x02
#define MOUSE_RIGHT_BUTTON  0x04
#define MOUSE_WHEEL         0x08


//--------------------------------------------------------------------------------------
// Manages the mesh, direction, mouse events of a directional arrow that 
// rotates around a radius controlled by an arcball 
//--------------------------------------------------------------------------------------
class CDXUTDirectionWidget
{
public:
    CDXUTDirectionWidget();

    static HRESULT StaticOnCreateDevice( IDirect3DDevice9* pd3dDevice );
    HRESULT OnResetDevice( long width, long height );
    HRESULT OnRender( D3DXCOLOR color, const D3DXMATRIX* pmView, const D3DXMATRIX* pmProj, const D3DXVECTOR3* pEyePt );
    LRESULT HandleMessages( HWND hWnd, UINT uMsg, WPARAM wParam, LPARAM lParam );
    static void StaticOnLostDevice();
    static void StaticOnDestroyDevice();

    D3DXVECTOR3 GetLightDirection()         { return m_vCurrentDir; };
    void        SetLightDirection( D3DXVECTOR3 vDir ) { m_vDefaultDir = m_vCurrentDir = vDir; };
    void        SetButtonMask( int nRotate = MOUSE_LEFT_BUTTON ) { m_nRotateMask = nRotate; }

    float GetRadius()                 { return m_fRadius; };
    void  SetRadius( float fRadius )  { m_fRadius = fRadius; };

    bool  IsBeingDragged() { return m_ArcBall.IsBeingDragged(); };

	D3DXMATRIXA16	m_matWorld;
	static ID3DXMesh*   s_pMesh;    

protected:
    HRESULT UpdateLightDir();

    D3DXMATRIXA16  m_mRot;
    D3DXMATRIXA16  m_mRotSnapshot;
    static IDirect3DDevice9* s_pd3dDevice;
    static ID3DXEffect* s_pEffect;       
    

    float          m_fRadius;
    int            m_nRotateMask;
    CD3D10ArcBall    m_ArcBall;
    D3DXVECTOR3    m_vDefaultDir;
    D3DXVECTOR3    m_vCurrentDir;
    D3DXMATRIX     m_mView;
};

#endif
 