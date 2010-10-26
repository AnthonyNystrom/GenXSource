//-----------------------------------------------------------------------------
// File: D3DFont.h
//
// Desc: Texture-based font class
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#pragma once

// Font creation flags
#define D3DFONT_BOLD        0x0001
#define D3DFONT_ITALIC      0x0002
#define D3DFONT_ZENABLE     0x0004

// Font rendering flags
#define D3DFONT_CENTERED_X  0x0001
#define D3DFONT_CENTERED_Y  0x0002
#define D3DFONT_TWOSIDED    0x0004
#define D3DFONT_FILTERED    0x0008

#include "GraphicsObject.h"

class CProteinVistaRenderer;

class CFontTexture:public CMoleculeRenderObject
{
public:
	int						m_refCount;

public:
	CString					m_strFontName;
	LPDIRECT3DTEXTURE9      m_pTexture;
	FLOAT					m_fTexCoords[128-32][4];

	CFontTexture(CProteinVistaRenderer * pProteinVistaRenderer);
	~CFontTexture();

	HRESULT CreateFontTexture( CString fontName );

    virtual HRESULT InitDeviceObjects();
    virtual HRESULT DeleteDeviceObjects();

	//
	//
	//
	LPDIRECT3DDEVICE9	m_pd3dDevice;

	DWORD	m_dwFontDisplayHeight;
	DWORD	m_dwFontHeight;
	DWORD   m_dwFontFlags;

	DWORD   m_dwTextureWidth;                 // Texture dimensions
	DWORD   m_dwTextureHeight;
	FLOAT   m_fTextScale;
	
	DWORD   m_dwSpacing;                  // Character pixel spacing per side
	HRESULT   CreateGDIFont( HDC hDC, HFONT* pFont );
	HRESULT   PaintAlphabet( HDC hDC, BOOL bMeasureOnly=FALSE );
};

typedef std::vector<CFontTexture*> CSTLArrayFontTexture;

class CFontTextureContainer:public CMoleculeRenderObject
{
public:
	CFontTextureContainer(CProteinVistaRenderer * pProteinVistaRenderer);
	~CFontTextureContainer();

    virtual HRESULT InitDeviceObjects();
    virtual HRESULT DeleteDeviceObjects();

	//
	//
	CSTLArrayFontTexture	m_arrayFontTexture;

	CFontTexture*       GetFontTexture(CString fontName);		//	fontHeight´Â 
	long				ReleaseFontTexture(CString faceName);
};

//-----------------------------------------------------------------------------
// Name: class CDrawText3D
// Desc: Texture-based font class for doing text in a 3D scene.
//-----------------------------------------------------------------------------
class CDrawText3D: public CMoleculeRenderObject
{
	CString		m_strFontName;
	DWORD		m_dwFontHeight;

	DWORD		m_dwFontDisplayHeight;

	CFontTexture *		m_fontTexture;
	
	LPDIRECT3DDEVICE9       m_pd3dDevice; // A D3DDevice used for rendering
	LPDIRECT3DVERTEXBUFFER9 m_pVB;        // VertexBuffer for rendering text
	LPDIRECT3DVERTEXDECLARATION9	m_pDeclText3D;

	D3DXMATRIXA16	* m_pMatWorld;
	D3DXMATRIXA16	m_matMVP;

	CString					m_strMsg; 
	CStringArray			m_arrayMsg; 
	CSTLIntArray			m_arrayMsgIndexBegin;
	CSTLIntArray			m_arrayMsgIndexEnd;
	int						m_totalTextLen;
	CSTLArrayColor			m_arrayColor;
	CSTLVectorValueArray	m_arrayPos;
	
	int				 m_textXPosDelta; 
	int				 m_textYPosDelta;

public:
	void	ResetContents();

	HRESULT SetModelMatrix ( D3DXMATRIXA16 * matWorld );
	HRESULT SetFontInfo ( const TCHAR* strFontName, DWORD dwHeight );
	HRESULT SetTextInfo ( CStringArray & arrayMsg, CSTLArrayColor & arrayColor , CSTLVectorValueArray & arrayPos , int textXPosDelta, int textYPosDelta );
	HRESULT Render3DTextBatch ();

	HRESULT Render3DText ( D3DXVECTOR3 & vecPos, D3DXCOLOR color, const TCHAR* strText, D3DXMATRIXA16 * matWorld, D3DXMATRIXA16 * matView, D3DXMATRIXA16 * matProj , long TextXPos = 50, long TextYPos = 50);

	// Function to get extent of text
	HRESULT GetTextExtent( const TCHAR* strText, SIZE* pSize );

	// Initializing and destroying device-dependent objects
	HRESULT InitDeviceObjects();
	HRESULT RestoreDeviceObjects();
	HRESULT Render();
	HRESULT InvalidateDeviceObjects();
	HRESULT DeleteDeviceObjects();

	// Constructor / destructor
	CDrawText3D();
	~CDrawText3D();
};


