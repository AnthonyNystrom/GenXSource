//-----------------------------------------------------------------------------
// File: DrawText3D.cpp
//	D3DFont.cpp 의 3차원 버전 + acceleration + shader
//
// Desc: Texture-based font class
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#include "stdafx.h"
#include "DrawText3D.h"

#include "ProteinVistaRenderer.h"
#include "Interface.h"

#pragma region CFontTexture

#define		TEXTURE_FONT_SIZE			18			//	256x256 텍스쳐 크기의 최대 폰트 사이즈		


//
CFontTexture::CFontTexture(CProteinVistaRenderer * pProteinVistaRenderer):m_pTexture(NULL),m_refCount(0) 
{
	Init(pProteinVistaRenderer);

	m_strFontName = _T("Arial"); 
	m_dwFontHeight         = TEXTURE_FONT_SIZE;			
	m_dwFontFlags          = 0;
	m_dwSpacing            = 0;

}

CFontTexture::~CFontTexture()
{
	SAFE_RELEASE(m_pTexture);
}

HRESULT CFontTexture::InitDeviceObjects()
{
	DeleteDeviceObjects();

	CreateFontTexture(m_strFontName);

	return S_OK;
}

HRESULT CFontTexture::DeleteDeviceObjects()
{
	SAFE_RELEASE(m_pTexture);

	return S_OK;
}

HRESULT CFontTexture::CreateGDIFont( HDC hDC, HFONT* pFont )
{
	// Create a font.  By specifying ANTIALIASED_QUALITY, we might get an
	// antialiased font, but this is not guaranteed.

	INT nHeight    = -MulDiv( m_dwFontHeight, (INT)(GetDeviceCaps(hDC, LOGPIXELSY) * m_fTextScale), 72 );
	DWORD dwBold   = (m_dwFontFlags & D3DFONT_BOLD)   ? FW_BOLD : FW_NORMAL;
	DWORD dwItalic = (m_dwFontFlags & D3DFONT_ITALIC) ? TRUE    : FALSE;
	*pFont         = CreateFont( nHeight, 0, 0, 0, dwBold, dwItalic,
									FALSE, FALSE, DEFAULT_CHARSET, OUT_DEFAULT_PRECIS,
									CLIP_DEFAULT_PRECIS, ANTIALIASED_QUALITY,
									VARIABLE_PITCH, m_strFontName );

	if( *pFont == NULL )
		return E_FAIL;

	return S_OK;
}

//-----------------------------------------------------------------------------
// Name: PaintAlphabet
// Desc: Paint the printable characters for the given GDI font onto the
//       provided device context. If the bMeasureOnly flag is set, no drawing 
//       will occur.
//-----------------------------------------------------------------------------
HRESULT CFontTexture::PaintAlphabet( HDC hDC, BOOL bMeasureOnly )
{
	SIZE size;
	TCHAR str[2] = _T("x"); // One-character, null-terminated string

	// Calculate the spacing between characters based on line height
	if( 0 == GetTextExtentPoint32( hDC, str, 1, &size ) )
		return E_FAIL;
	//	m_dwSpacing = (DWORD) ceil(size.cy * 0.3f);
	m_dwSpacing = 2;

	// Set the starting point for the drawing
	DWORD x = m_dwSpacing;
	DWORD y = 0;

	// For each character, draw text on the DC and advance the current position
	for( char c = 32; c < 127; c++ )
	{
		str[0] = c;
		if( 0 == GetTextExtentPoint32( hDC, str, 1, &size ) )
			return E_FAIL;

		if( (DWORD)(x + size.cx + m_dwSpacing) > m_dwTextureWidth )
		{
			x  = m_dwSpacing;
			y += size.cy + 1;
		}

		// Check to see if there's room to write the character here
		if( y + size.cy > m_dwTextureHeight )
			return D3DERR_MOREDATA;

		if( !bMeasureOnly )
		{
			// Perform the actual drawing
			if( 0 == ExtTextOut( hDC, x+0, y+0, ETO_OPAQUE, NULL, str, 1, NULL ) )
				return E_FAIL;

			m_fTexCoords[c-32][0] = ((FLOAT)(x + 0       - m_dwSpacing))/m_dwTextureWidth;
			m_fTexCoords[c-32][1] = ((FLOAT)(y + 0       + 0          ))/m_dwTextureHeight;
			m_fTexCoords[c-32][2] = ((FLOAT)(x + size.cx + m_dwSpacing))/m_dwTextureWidth;
			m_fTexCoords[c-32][3] = ((FLOAT)(y + size.cy + 0          ))/m_dwTextureHeight;
		}

		x += size.cx + (2 * m_dwSpacing);
	}

	return S_OK;
}

HRESULT CFontTexture::CreateFontTexture(CString fontName)
{
	m_strFontName = fontName;

	m_pd3dDevice = m_pProteinVistaRenderer->GetD3DDevice();
	if ( m_pd3dDevice == NULL )
		return S_OK;

	HRESULT hr = S_OK;
	HFONT hFont = NULL;
	HFONT hFontOld = NULL;
	HDC hDC = NULL;
	HBITMAP hbmBitmap = NULL;
	HGDIOBJ hbmOld = NULL;

	// Assume we will draw fonts into texture without scaling unless the
	// required texture size is found to be larger than the device max
	m_fTextScale  = 1.0f; 

	hDC = CreateCompatibleDC( NULL );
	SetMapMode( hDC, MM_TEXT );

	hr = CreateGDIFont( hDC, &hFont );
	if( FAILED(hr) )
		goto LCleanReturn;

	hFontOld = (HFONT) SelectObject( hDC, hFont );

	// Calculate the dimensions for the smallest power-of-two texture which
	// can hold all the printable characters
	m_dwTextureWidth = m_dwTextureHeight = 128;
	while( D3DERR_MOREDATA == ( hr = PaintAlphabet( hDC, true ) ) )
	{
		m_dwTextureWidth *= 2;
		m_dwTextureHeight *= 2;
	}

	if( FAILED(hr) )
		goto LCleanReturn;

	// If requested texture is too big, use a smaller texture and smaller font,
	// and scale up when rendering.
	D3DCAPS9 d3dCaps;
	m_pd3dDevice->GetDeviceCaps( &d3dCaps );

	if( m_dwTextureWidth > d3dCaps.MaxTextureWidth )
	{
		m_fTextScale = (FLOAT)d3dCaps.MaxTextureWidth / (FLOAT)m_dwTextureWidth;
		m_dwTextureWidth = m_dwTextureHeight = d3dCaps.MaxTextureWidth;

		bool bFirstRun = true; // Flag clear after first run

		do
		{
			// If we've already tried fitting the new text, the scale is still 
			// too large. Reduce and try again.
			if( !bFirstRun)
				m_fTextScale *= 0.9f;

			// The font has to be scaled to fit on the maximum texture size; our
			// current font is too big and needs to be recreated to scale.
			DeleteObject( SelectObject( hDC, hFontOld ) );

			hr = CreateGDIFont( hDC, &hFont );
			if( FAILED(hr) )
				goto LCleanReturn;

			hFontOld = (HFONT) SelectObject( hDC, hFont );

			bFirstRun = false;
		} 
		while( D3DERR_MOREDATA == ( hr = PaintAlphabet( hDC, true ) ) );
	}

	// Create a new texture for the font
	hr = m_pd3dDevice->CreateTexture( m_dwTextureWidth, m_dwTextureHeight, 1, 0, D3DFMT_A4R4G4B4, D3DPOOL_MANAGED, &m_pTexture, NULL );
	if( FAILED(hr) )
		goto LCleanReturn;

	// Prepare to create a bitmap
	DWORD*      pBitmapBits;
	BITMAPINFO bmi;
	ZeroMemory( &bmi.bmiHeader, sizeof(BITMAPINFOHEADER) );
	bmi.bmiHeader.biSize        = sizeof(BITMAPINFOHEADER);
	bmi.bmiHeader.biWidth       =  (int)m_dwTextureWidth;
	bmi.bmiHeader.biHeight      = -(int)m_dwTextureHeight;
	bmi.bmiHeader.biPlanes      = 1;
	bmi.bmiHeader.biCompression = BI_RGB;
	bmi.bmiHeader.biBitCount    = 32;

	// Create a bitmap for the font
	hbmBitmap = CreateDIBSection( hDC, &bmi, DIB_RGB_COLORS,
		(void**)&pBitmapBits, NULL, 0 );

	hbmOld = SelectObject( hDC, hbmBitmap );

	// Set text properties
	SetTextColor( hDC, RGB(255,255,255) );
	SetBkColor(   hDC, 0x00000000 );
	SetTextAlign( hDC, TA_TOP );

	// Paint the alphabet onto the selected bitmap
	hr = PaintAlphabet( hDC, false );
	if( FAILED(hr) )
		goto LCleanReturn;

	// Lock the surface and write the alpha values for the set pixels
	D3DLOCKED_RECT d3dlr;
	m_pTexture->LockRect( 0, &d3dlr, 0, 0 );
	BYTE* pDstRow;
	pDstRow = (BYTE*)d3dlr.pBits;
	WORD* pDst16;
	BYTE bAlpha; // 4-bit measure of pixel intensity
	DWORD x, y;

	for( y=0; y < m_dwTextureHeight; y++ )
	{
		pDst16 = (WORD*)pDstRow;
		for( x=0; x < m_dwTextureWidth; x++ )
		{
			bAlpha = (BYTE)((pBitmapBits[m_dwTextureWidth*y + x] & 0xff) >> 4);
			if (bAlpha > 0)
			{
				*pDst16++ = (WORD) ((bAlpha << 12) | 0x0fff);
			}
			else
			{
				*pDst16++ = 0x0000;
			}
		}
		pDstRow += d3dlr.Pitch;
	}

	hr = S_OK;

	// Done updating texture, so clean up used objects
LCleanReturn:
	if( m_pTexture )
		m_pTexture->UnlockRect(0);

	SelectObject( hDC, hbmOld );
	SelectObject( hDC, hFontOld );
	DeleteObject( hbmBitmap );
	DeleteObject( hFont );
	DeleteDC( hDC );

	return S_OK;
}

CFontTexture*      CFontTextureContainer::GetFontTexture(CString fontName)
{
	for ( int i = 0 ; i < m_arrayFontTexture.size() ; i++ )
	{
		if( m_arrayFontTexture[i]->m_strFontName == fontName )
		{
			m_arrayFontTexture[i]->m_refCount ++;
			return m_arrayFontTexture[i];
		}
	}

	CFontTexture * pFontTexture = new CFontTexture(m_pProteinVistaRenderer);

	pFontTexture->m_refCount ++;
	pFontTexture->CreateFontTexture(fontName);

	m_arrayFontTexture.push_back(pFontTexture);

	return pFontTexture;
}

long	CFontTextureContainer::ReleaseFontTexture(CString fontName)
{
	for ( int i = 0 ; i < m_arrayFontTexture.size() ; i++ )
	{
		if( m_arrayFontTexture[i]->m_strFontName == fontName )
		{
			m_arrayFontTexture[i]->m_refCount --;
			if ( m_arrayFontTexture[i]->m_refCount == 0 )
			{
				CFontTexture * pFontTexture = m_arrayFontTexture[i];

				std::vector<CFontTexture*> :: iterator Iter = m_arrayFontTexture.begin();
				m_arrayFontTexture.erase(Iter+i);

				SAFE_DELETE(pFontTexture);
				return 0;
			}

			return m_arrayFontTexture[i]->m_refCount;
		}
	}

	//	발견되지 않으면... 
	return -1;
}

#pragma endregion

#pragma region CFontTextureContainer
//
//
//
CFontTextureContainer::CFontTextureContainer(CProteinVistaRenderer * pProteinVistaRenderer)
{
	Init(pProteinVistaRenderer);
}

CFontTextureContainer::~CFontTextureContainer()
{
	for ( int i = 0 ; i < m_arrayFontTexture.size() ; i++ )
	{
		SAFE_DELETE(m_arrayFontTexture[i]);
	}
	m_arrayFontTexture.clear();
}

HRESULT CFontTextureContainer::InitDeviceObjects()
{
	for( int i = 0 ; i < m_arrayFontTexture.size(); i++ )
		m_arrayFontTexture[i]->InitDeviceObjects();

	return S_OK; 
}

HRESULT CFontTextureContainer::DeleteDeviceObjects()
{
	for( int i = 0 ; i < m_arrayFontTexture.size(); i++ )
		m_arrayFontTexture[i]->DeleteDeviceObjects();

	return S_OK;
}

#pragma endregion

//==========================================================================================================
//==========================================================================================================

#define		FONT_POINT_TO_3D_VERTEX		800.0f

//-----------------------------------------------------------------------------
// Custom vertex types for rendering text
//-----------------------------------------------------------------------------
struct FONT3DVERTEX { D3DXVECTOR3 p;   D3DXVECTOR3 n;   DWORD color;	FLOAT tu, tv; };
#define D3DFVF_FONT3DVERTEX (D3DFVF_XYZ|D3DFVF_NORMAL|D3DFVF_DIFFUSE|D3DFVF_TEX1)

inline FONT3DVERTEX InitFont3DVertex( const D3DXVECTOR3& p, const D3DXVECTOR3& n, D3DCOLOR color, FLOAT tu, FLOAT tv )
{
	FONT3DVERTEX v;   
	v.p = p;	v.n = n;   v.color = color;  v.tu = tu;   v.tv = tv;
	return v;
}

//-----------------------------------------------------------------------------
// Name: CDrawText3D()
// Desc: Font class constructor
//-----------------------------------------------------------------------------
CDrawText3D::CDrawText3D()
{
	m_strFontName = CString(_T(""));
	m_dwFontHeight = 0;

	D3DXMatrixIdentity(&m_matMVP);

	m_pd3dDevice           = NULL;
	m_pVB                  = NULL;
	m_pDeclText3D		   = NULL;

	m_fontTexture = NULL;
}

//-----------------------------------------------------------------------------
// Name: ~CDrawText3D()
// Desc: Font class destructor
//-----------------------------------------------------------------------------
CDrawText3D::~CDrawText3D()
{
	InvalidateDeviceObjects();
	DeleteDeviceObjects();
	ResetContents();

	//	
	m_pProteinVistaRenderer->m_pFontTextureContainer->ReleaseFontTexture(m_strFontName);
}

HRESULT CDrawText3D::SetFontInfo ( const TCHAR* strFontName, DWORD dwHeight )
{
	if ( m_strFontName == CString(strFontName) && m_dwFontHeight == dwHeight )	return S_OK;

	m_pProteinVistaRenderer->m_pFontTextureContainer->ReleaseFontTexture(m_strFontName);

	m_strFontName = CString(strFontName);
	m_dwFontHeight = dwHeight;

	HDC hDC = CreateCompatibleDC( NULL );
	m_dwFontDisplayHeight = -MulDiv( dwHeight, (INT)(GetDeviceCaps(hDC, LOGPIXELSY)), 72 );
	DeleteDC(hDC);

	m_fontTexture = m_pProteinVistaRenderer->m_pFontTextureContainer->GetFontTexture(m_strFontName);

	return S_OK;
}

HRESULT CDrawText3D::SetTextInfo ( CStringArray & arrayMsg, CSTLArrayColor & arrayColor , CSTLVectorValueArray & arrayPos , int textXPosDelta, int textYPosDelta )
{
	m_strMsg.Empty();
	m_arrayMsg.RemoveAll();
	m_arrayMsgIndexBegin.clear();
	m_arrayMsgIndexEnd.clear();
	m_totalTextLen = 0;
	m_arrayColor.clear();
	m_arrayPos.clear();

	//	
	//	text중 출력불가능한 캐릭터는 전부 지운다.	
	//	한글은 안 나온다.
	//	if( (c-32) < 0 || (c-32) >= 128-32 )		continue;
	//
	for ( int i = 0 ; i < arrayMsg.GetSize(); i++ )
	{
		TCHAR	buffText[512] = {0,};
		int		iBuffText = 0;
		CString &strText = arrayMsg[i];
		for ( int j = 0 ; j < strText.GetLength(); j++ )
		{
			TCHAR c = strText[j];
			if( (c-32) < 0 || (c-32) >= 128-32 )		continue;
			buffText[iBuffText++] = c;
		}
		buffText[iBuffText++] = _T(' ');

		m_arrayMsg.Add(CString(buffText));
	}

	for ( int i = 0 ; i < m_arrayMsg.GetSize(); i++ )
	{
		m_arrayMsgIndexBegin.push_back(m_totalTextLen);
		m_totalTextLen += m_arrayMsg[i].GetLength();
		m_arrayMsgIndexEnd.push_back(m_totalTextLen-1);
		m_strMsg += m_arrayMsg[i];
	}

	m_arrayColor = arrayColor;
	m_arrayPos = arrayPos;

	m_textXPosDelta = textXPosDelta;
	m_textYPosDelta = textYPosDelta;

	return S_OK;
}

void CDrawText3D::ResetContents()
{
	D3DXMatrixIdentity(&m_matMVP);

	m_strMsg.Empty();
	m_arrayMsg.RemoveAll();
	m_arrayMsgIndexBegin.clear();
	m_arrayMsgIndexEnd.clear();
	m_totalTextLen = 0;
	m_arrayColor.clear();
	m_arrayPos.clear();

	m_totalTextLen = 0;
}

//-----------------------------------------------------------------------------
// Name: InitDeviceObjects()
// Desc: Initializes device-dependent objects, including the vertex buffer used
//       for rendering text and the texture map which stores the font image.
//-----------------------------------------------------------------------------
HRESULT CDrawText3D::InitDeviceObjects()
{
	HRESULT hr = S_OK;

	//	
	DeleteDeviceObjects();

	//	텍스트가 하나도 없으면 만들지 않는다.
	if ( m_arrayMsg.GetSize() == 0 )
		return S_OK;

	m_pd3dDevice = GetD3DDevice();

	//
	//
	// Create vertex buffer for the letters
	//
	D3DVERTEXELEMENT9	decl[MAX_FVF_DECL_SIZE];
	D3DXDeclaratorFromFVF(D3DFVF_FONT3DVERTEX, decl);
	m_pd3dDevice->CreateVertexDeclaration(decl, &m_pDeclText3D);

	int vertexSize = sizeof(FONT3DVERTEX );
	if( FAILED( hr = m_pd3dDevice->CreateVertexBuffer( m_totalTextLen * 6 * vertexSize, 0 , 0, D3DPOOL_MANAGED, &m_pVB, NULL ) ) )
	{
		return hr;
	}

	// Fill vertex buffer
	FONT3DVERTEX* pVertices;
	DWORD         dwNumTriangles = 0L;
	V_RETURN(m_pVB->Lock( 0, 0, (void**)&pVertices, 0 ));

	for ( int istr = 0 ; istr < m_arrayMsg.GetSize(); istr++ )
	{
		//
		//	vertex pos
		//
		// Position for each text element
		FLOAT xPos = 0.0f;
		FLOAT yPos = 0.0f;

		// Adjust for character spacing
		//	xPos -= m_dwSpacing / 10.0f;
		FLOAT fStartX = xPos;
		TCHAR c;

		//	text string 의 width 와 height를 구한다.
		FLOAT	textWidth = 0;
		FLOAT	textHeight = 0;

		const TCHAR * strTextWidth = m_arrayMsg[istr];
		while( (c = *strTextWidth++) != 0 )
		{
			if( (c-32) < 0 || (c-32) >= 128-32 )
				continue;

			FLOAT tx1 = m_fontTexture->m_fTexCoords[c-32][0];
			FLOAT ty1 = m_fontTexture->m_fTexCoords[c-32][1];
			FLOAT tx2 = m_fontTexture->m_fTexCoords[c-32][2];
			FLOAT ty2 = m_fontTexture->m_fTexCoords[c-32][3];

			FLOAT textScale = m_dwFontDisplayHeight/FONT_POINT_TO_3D_VERTEX;

			FLOAT w = (tx2-tx1) * m_fontTexture->m_dwTextureWidth  * textScale;		
			textHeight = (ty2-ty1) * m_fontTexture->m_dwTextureHeight * textScale;		
			FLOAT w2 = w-( 2* m_fontTexture->m_dwSpacing * textScale );					

			textWidth += w2;
		}

		//	TextXPos, TextYPos
		//
		// Center the text block at the origin (not the viewport)
		xPos += ((m_textXPosDelta/100.0f)-1.0f) * (textWidth);
		yPos += ((m_textYPosDelta/100.0f)-1.0f) * (textHeight);

		//
		//
		const TCHAR * strText = m_arrayMsg[istr];
		D3DXVECTOR3 vecPos(m_arrayPos[istr]);
		D3DXCOLOR	color(m_arrayColor[istr]);

		while( (c = *strText++) != 0 )
		{
			if( (c-32) < 0 || (c-32) >= 128-32 )
				continue;

			FLOAT tx1 = m_fontTexture->m_fTexCoords[c-32][0];
			FLOAT ty1 = m_fontTexture->m_fTexCoords[c-32][1];
			FLOAT tx2 = m_fontTexture->m_fTexCoords[c-32][2];
			FLOAT ty2 = m_fontTexture->m_fTexCoords[c-32][3];

			FLOAT textScale = m_dwFontDisplayHeight/FONT_POINT_TO_3D_VERTEX;

			FLOAT w = (tx2-tx1) * m_fontTexture->m_dwTextureWidth  * textScale;		//		/ ( 10.0f * m_fTextScale );
			FLOAT h = (ty2-ty1) * m_fontTexture->m_dwTextureHeight * textScale;		//		/ ( 10.0f * m_fTextScale );
			FLOAT w2 = w-( 2 * m_fontTexture->m_dwSpacing * textScale );					//		/ ( 10.0f * m_fTextScale );

			//if( c != _T(' ') )
			{
				float tx1delta = (float)m_fontTexture->m_dwSpacing/(float)m_fontTexture->m_dwTextureWidth;
				float tx2delta = (float)m_fontTexture->m_dwSpacing/(float)m_fontTexture->m_dwTextureWidth;

				*pVertices++ = InitFont3DVertex( D3DXVECTOR3(vecPos.x+xPos+0,vecPos.y+yPos+0,vecPos.z), D3DXVECTOR3(0,0,-1), color, tx1+tx1delta, ty2 );
				*pVertices++ = InitFont3DVertex( D3DXVECTOR3(vecPos.x+xPos+0,vecPos.y+yPos+h,vecPos.z), D3DXVECTOR3(0,0,-1), color, tx1+tx1delta, ty1 );
				*pVertices++ = InitFont3DVertex( D3DXVECTOR3(vecPos.x+xPos+w2,vecPos.y+yPos+0,vecPos.z), D3DXVECTOR3(0,0,-1), color, tx2-tx2delta, ty2 );
				*pVertices++ = InitFont3DVertex( D3DXVECTOR3(vecPos.x+xPos+w2,vecPos.y+yPos+h,vecPos.z), D3DXVECTOR3(0,0,-1), color, tx2-tx2delta, ty1 );
				*pVertices++ = InitFont3DVertex( D3DXVECTOR3(vecPos.x+xPos+w2,vecPos.y+yPos+0,vecPos.z), D3DXVECTOR3(0,0,-1), color, tx2-tx2delta, ty2 );
				*pVertices++ = InitFont3DVertex( D3DXVECTOR3(vecPos.x+xPos+0,vecPos.y+yPos+h,vecPos.z), D3DXVECTOR3(0,0,-1), color, tx1+tx1delta, ty1 );
				dwNumTriangles += 2;
			}

			xPos += w2;
		}
	}

	// Unlock and render the vertex buffer
	m_pVB->Unlock();

	return hr;
}

//-----------------------------------------------------------------------------
// Name: RestoreDeviceObjects()
// Desc:
//-----------------------------------------------------------------------------
HRESULT CDrawText3D::RestoreDeviceObjects()
{
	HRESULT hr;

	

	return S_OK;
}

//-----------------------------------------------------------------------------
// Name: InvalidateDeviceObjects()
// Desc: Destroys all device-dependent objects
//-----------------------------------------------------------------------------
HRESULT CDrawText3D::InvalidateDeviceObjects()
{


	return S_OK;
}

//-----------------------------------------------------------------------------
// Name: DeleteDeviceObjects()
// Desc: Destroys all device-dependent objects
//-----------------------------------------------------------------------------
HRESULT CDrawText3D::DeleteDeviceObjects()
{
	SAFE_RELEASE( m_pVB );
	SAFE_RELEASE( m_pDeclText3D );

	return S_OK;
}

//-----------------------------------------------------------------------------
// Name: Render3DText()
// Desc: Renders 3D text
//	Obsolete function
//-----------------------------------------------------------------------------
HRESULT CDrawText3D::Render3DText ( D3DXVECTOR3 & vecPos, D3DXCOLOR color, const TCHAR* strText, D3DXMATRIXA16 * worldMatrix , D3DXMATRIXA16 * viewMatrix, D3DXMATRIXA16 * projMatrix , long TextXPos, long TextYPos )
{
	HRESULT hr;
	return S_OK;
}

HRESULT CDrawText3D::SetModelMatrix ( D3DXMATRIXA16 * matWorld)
{
	m_pMatWorld = matWorld;

	return S_OK;
}

HRESULT CDrawText3D::Render()
{
	return Render3DTextBatch();
}
#pragma managed(push,off)
HRESULT CDrawText3D::Render3DTextBatch()
{
	HRESULT hr;

	if( m_pd3dDevice == NULL )
		return E_FAIL;

	if ( m_pVB == NULL )
		return E_FAIL;

	// Turn off culling for two-sided text
	m_pd3dDevice->SetRenderState( D3DRS_CULLMODE, D3DCULL_NONE );

	//	new drawText version
	m_pd3dDevice->SetVertexDeclaration( m_pDeclText3D );

	m_pd3dDevice->SetStreamSource( 0, m_pVB, 0, sizeof(FONT3DVERTEX) );

	m_pProteinVistaRenderer->SetShaderTechnique(CProteinVistaRenderer::Text3DRenderingWithAlpha);
	m_pProteinVistaRenderer->SetShaderSelectionTexture(m_fontTexture->m_pTexture);

	D3DXMATRIXA16 * worldMatrix = m_pMatWorld;
	D3DXMATRIXA16 * viewMatrix = m_pProteinVistaRenderer->GetViewMatrix();
	D3DXMATRIXA16 * projMatrix = m_pProteinVistaRenderer->GetProjMatrix();

	m_matMVP = (*m_pMatWorld) * (*viewMatrix) * (*projMatrix);

	D3DXMATRIXA16 worldMatrixInv = *worldMatrix;
	worldMatrixInv._41 = 0.0f;
	worldMatrixInv._42 = 0.0f;
	worldMatrixInv._43 = 0.0f;
	D3DXMatrixTranspose(&worldMatrixInv, &worldMatrixInv);

	D3DXMATRIXA16 viewMatrixInv = *viewMatrix;
	viewMatrixInv._41 = 0.0f;
	viewMatrixInv._42 = 0.0f;
	viewMatrixInv._43 = 0.0f;
	D3DXMatrixTranspose(&viewMatrixInv, &viewMatrixInv);

	D3DXMATRIXA16 preMatMVP = (*worldMatrix) * (*viewMatrix) * (*projMatrix);

	for ( int i = 0 ; i < m_arrayMsg.GetSize(); i++ )
	{
		int iBegin = m_arrayMsgIndexBegin[i];
		int strLen = m_arrayMsg[i].GetLength();

		D3DXVECTOR3 vecPos = m_arrayPos[i];
		
		D3DXMATRIXA16 worldMatrixCenterInv;
		D3DXMatrixTranslation(&worldMatrixCenterInv, -vecPos.x, -vecPos.y, -vecPos.z );

		D3DXMATRIXA16 worldMatrixCenter;
		D3DXMatrixTranslation(&worldMatrixCenter, vecPos.x, vecPos.y, vecPos.z );

		D3DXMATRIXA16 worldMatrixTrans;
		D3DXMatrixTranslation(&worldMatrixTrans, 2.0f, 2.0f, 0.0f );

		D3DXMATRIXA16 matMVP = ( worldMatrixCenterInv * viewMatrixInv * worldMatrixInv * worldMatrixCenter ) * preMatMVP;
		m_pProteinVistaRenderer->SetShaderWorldViewProjMatrix( matMVP );

		UINT cPasses;
		V( m_pProteinVistaRenderer->m_pEffectBasicShading->Begin(&cPasses, 0) );
		for (long iPass = 0; iPass < cPasses; iPass++)
		{
			V( m_pProteinVistaRenderer->m_pEffectBasicShading->BeginPass(iPass) );

			m_pd3dDevice->DrawPrimitive( D3DPT_TRIANGLELIST, iBegin * 2 * 3, (strLen-1)*2 );

			V( m_pProteinVistaRenderer->m_pEffectBasicShading->EndPass() );
		}
		V( m_pProteinVistaRenderer->m_pEffectBasicShading->End() );
	}

	//	
	m_pd3dDevice->SetRenderState( D3DRS_CULLMODE, D3DCULL_CCW );

	return S_OK;
}
#pragma managed(pop)
