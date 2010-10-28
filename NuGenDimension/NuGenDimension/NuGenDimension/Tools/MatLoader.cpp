#include "stdafx.h"
#include "MatLoader.h"
#include "..//raytracing//RTMaterials.h"


CMatLoader::CMatLoader()
	:m_mat_main_header(NULL)
{
}


CMatLoader::~CMatLoader()
{
	if (m_mat_main_header)
	{
		DetachMatLibrary();
	}
}

bool	CMatLoader::AttachMatLibrary(LPCTSTR path)
{
	if (IsAttached())
		DetachMatLibrary();


	CFile  file;
	CFileException	fe;

	if (!file.Open(path, CFile::modeRead, &fe))	
	{
		ASSERT(0);
		return false;
	}

	if (m_mat_main_header)
		ASSERT(0);

	m_mat_main_header = (MAT_MAIN*)malloc(sizeof(MAT_MAIN));
	memset(m_mat_main_header,0,sizeof(MAT_MAIN));

	if (file.Read(m_mat_main_header,sizeof(MAT_MAIN))!=sizeof(MAT_MAIN)) return FALSE;

	if(strcmp(m_mat_main_header->szSign,"AR_MAT_V1.0"))
	{
		ASSERT(0);
		file.Close();
		free(m_mat_main_header);
		m_mat_main_header = NULL;
		return false;
	}

	m_materials.reserve(m_mat_main_header->nTotalMat);

	int i;

	m_materials.reserve(m_mat_main_header->nTotalMat);
	MAT_ITEM tmpItem;
	for (i=0;i<m_mat_main_header->nTotalMat;i++)
	{
		if (file.Read(&tmpItem,sizeof(MAT_ITEM))!=sizeof(MAT_ITEM)) 
		{
			m_materials.clear();
			free(m_mat_main_header);
			m_mat_main_header = NULL;
			return false;
		}
		m_materials.push_back(tmpItem);
	}

	m_textures.reserve(m_mat_main_header->nTotalTex);

	for (i=0;i<m_mat_main_header->nTotalTex;i++)
	{
		TEXTURE_ITEM  tmp_t_item;
		memset(&tmp_t_item,0,sizeof(TEXTURE_ITEM));

		if (file.Read(&tmp_t_item.imageType,sizeof(int))!=sizeof(int)) 
		{
			ASSERT(0);
			return false;
		}
		if (file.Read(&tmp_t_item.imageSize,sizeof(unsigned int))!=sizeof(unsigned int)) 
		{
			ASSERT(0);
			return false;
		}

		try
		{
			tmp_t_item.imageBits = new BYTE[tmp_t_item.imageSize];
		}
		catch (std::bad_alloc) 
		{
			AfxMessageBox("bad_alloc exception in OnOpenDocument function");
			ASSERT(0);
			return false;
		}

		if (file.Read(tmp_t_item.imageBits,tmp_t_item.imageSize*sizeof(BYTE))!=tmp_t_item.imageSize*sizeof(BYTE))
		{
			delete[] tmp_t_item.imageBits;
			return false;
		}

		tmp_t_item.image = new CxImage(tmp_t_item.imageBits, tmp_t_item.imageSize, tmp_t_item.imageType);

		if (!tmp_t_item.image->IsValid())
		{
			AfxMessageBox(tmp_t_item.image->GetLastError());
			delete tmp_t_item.image;
			tmp_t_item.image = NULL;
			delete[] tmp_t_item.imageBits;
			ASSERT(0);
			return false;
		}
		CalcOpenGLTexture(&tmp_t_item);
		m_textures.push_back(tmp_t_item);
	}

	// LOAD MATERIAL FOR RAY TRACING LIBRARY
	// load only texture linked to the material
	for (i=0; i<m_mat_main_header->nTotalMat; i++)
	{	MAT_ITEM *pM=GetMaterialByIndex(i);
		int iIdx = pM->nIdxTexture-1;
		if (iIdx>=0)
			mat_library.AddMaterial(
			m_textures[iIdx].opengl_texture->pPicBits,
			m_textures[iIdx].opengl_texture->Width,
			m_textures[iIdx].opengl_texture->Height);
		else if(pM->m_iSolidMaterial)
			CreateMaterials(pM->m_iSolidMaterial);
	}


	file.Close();

	return true;
}

void CMatLoader::CalcOpenGLTexture(TEXTURE_ITEM* text_item)
{
	CBitmap*		pBitmap;
	BITMAP			BmpInfo;
	
	ASSERT(text_item->image);

	text_item->opengl_texture = (OPENGL_TEXTURE*)malloc(sizeof(OPENGL_TEXTURE));
	memset(text_item->opengl_texture,0,sizeof(OPENGL_TEXTURE));

	pBitmap = CBitmap::FromHandle(text_item->image->MakeBitmap());
	pBitmap->GetBitmap( &BmpInfo );

	text_item->opengl_texture->Width   = BmpInfo.bmWidth;
	text_item->opengl_texture->Height  = BmpInfo.bmHeight;


	const int BITMAPINFOHEADER_SIZE = sizeof(BITMAPINFOHEADER) ;
	BYTE* abBitmapInfo[BITMAPINFOHEADER_SIZE] ;
	BITMAPINFOHEADER* pBMIH = (BITMAPINFOHEADER*)abBitmapInfo;
	memset(pBMIH, 0, BITMAPINFOHEADER_SIZE);

	// fill in the header info 
	pBMIH->biSize 			= sizeof(BITMAPINFOHEADER);
	pBMIH->biWidth 			= text_item->opengl_texture->Width;
	pBMIH->biHeight 		= text_item->opengl_texture->Height;
	pBMIH->biPlanes 		= 1;
	pBMIH->biBitCount 		= 24 ; //32?? Use 32 bpp to avoid boundary alignment issues.
	pBMIH->biCompression  	= BI_RGB ; 

	//
	// Create the new 32 bpp DIB section.
	//
	CDC dc;
	dc.CreateCompatibleDC(NULL);
	BYTE* pBits ;
	HBITMAP hbmBuffer = CreateDIBSection(dc.GetSafeHdc(),
												(BITMAPINFO*)pBMIH,
												DIB_RGB_COLORS,
												(VOID **) &pBits,
												NULL,
												0);
	// Select DIB into DC.
	HBITMAP hbmOld = (HBITMAP)::SelectObject(dc.GetSafeHdc(), hbmBuffer);

	int		iImageSize = text_item->opengl_texture->Width*
				text_item->opengl_texture->Height * sizeof(CGLRGBTRIPLE) ;
	text_item->opengl_texture->pPicBits = malloc(iImageSize);


	RGBQUAD* pSrc = (RGBQUAD*)pBits ;
	CGLRGBTRIPLE* pDest = (CGLRGBTRIPLE*) text_item->opengl_texture->pPicBits ;

	int i,j;
	RGBQUAD rgb;
	for (i = 0 ; i < text_item->opengl_texture->Height; i++)
	{
		for(j = 0 ; j < text_item->opengl_texture->Width; j++)
		{
			rgb = text_item->image->GetPixelColor(j, i);
			pDest->rgbRed	= rgb.rgbRed;	//R;
			pDest->rgbGreen = rgb.rgbGreen;	//G;
			pDest->rgbBlue	= rgb.rgbBlue;	//B;
			pDest++ ;
		}
	}

	if (hbmOld)
		(HBITMAP)::SelectObject(dc.GetSafeHdc(), hbmOld);
	DeleteObject(hbmBuffer) ;

	//delete pBitmap;

	glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
	glTexImage2D(GL_TEXTURE_2D, 0, 3, 
		text_item->opengl_texture->Width, 
		text_item->opengl_texture->Height,			// <<---
		0, GL_RGB, GL_UNSIGNED_BYTE, 
		text_item->opengl_texture->pPicBits);					//      |
	//		|
	
	//TexMapScalePow2();												//		|
	// масштабируем
	{
		GLint glMaxTexDim ;
		double xPow2, yPow2;
		int ixPow2, iyPow2;
		int xSize2, ySize2;
		int	res;

		::glGetIntegerv(GL_MAX_TEXTURE_SIZE, &glMaxTexDim);
		glMaxTexDim = min(128, glMaxTexDim);

		if (text_item->opengl_texture->Width <= glMaxTexDim)
			xPow2 = log((double)text_item->opengl_texture->Width) / log(2.0);
		else
			xPow2 = log((double)glMaxTexDim) / log(2.0);

		if (text_item->opengl_texture->Height <= glMaxTexDim)
			yPow2 = log((double)text_item->opengl_texture->Height) / log(2.0);
		else
			yPow2 = log((double)glMaxTexDim) / log(2.0);

		ixPow2 = (int)xPow2;
		iyPow2 = (int)yPow2;

		if (xPow2 != (double)ixPow2)
			ixPow2++;
		if (yPow2 != (double)iyPow2)
			iyPow2++;

		xSize2 = 1 << ixPow2;
		ySize2 = 1 << iyPow2;

		BYTE *pData = (BYTE*)malloc(xSize2 * ySize2 * 3 * sizeof(BYTE));
		if (!pData)
		{
			ASSERT(0);
			return;
		}

		//pGL->MakeCurrent() ;
		res = gluScaleImage(GL_RGB, text_item->opengl_texture->Width, 
			text_item->opengl_texture->Height,
			GL_UNSIGNED_BYTE, text_item->opengl_texture->pPicBits,
			xSize2, ySize2, GL_UNSIGNED_BYTE,
			pData);

		//pGL->OutputGlError("CGLImage::TexMapScalePow2") ;

		free(text_item->opengl_texture->pPicBits);
		text_item->opengl_texture->pPicBits = pData; 
		text_item->opengl_texture->Width = xSize2 ;
		text_item->opengl_texture->Height = ySize2 ;
	}
}

bool  CMatLoader::DetachMatLibrary()
{
	mat_library.Delete();

	if (m_mat_main_header)
	{
		size_t texSz = m_textures.size();
		ASSERT(m_materials.size()==m_mat_main_header->nTotalMat);
		ASSERT(texSz==m_mat_main_header->nTotalTex);

		for (size_t i=0;i<texSz;i++)
		{
			delete m_textures[i].image;
			delete[] m_textures[i].imageBits;

			//  этого нет в файле и в materialEditor. 
			//  Надо для отрисовки
			free(m_textures[i].opengl_texture->pPicBits);
			free(m_textures[i].opengl_texture);
		}
		m_textures.clear();
		m_materials.clear();
		free(m_mat_main_header);
		m_mat_main_header = NULL;
		return true;
	}
	return false;
}

MAT_ITEM*  CMatLoader::GetMaterialByIndex(int ind)
{
	int mtSz = (int)m_materials.size();

	if (ind<0 || ind>=mtSz)
	{
		ASSERT(0);
		return NULL;
	}

	return &m_materials[ind];
}

TEXTURE_ITEM* CMatLoader::GetTextureByIndex(int ind)
{
	int tSz = (int)m_textures.size();

	if (ind<0 || ind>=tSz)
	{
		ASSERT(0);
		return NULL;
	}

	return &m_textures[ind];
}