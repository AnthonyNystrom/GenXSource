#include "stdafx.h"
#include "DraftTemplateLoader.h"

#include "..//NuGenDimension.h"

// Заголовончая структура файла фиблиотеки
typedef struct
{
	char  szSign[16];
	int   nVersion;
}DRAFT_TEMPLATE_HEADER;

typedef struct
{
	int           imageType;
	unsigned int  imageSize;
	BYTE*         imageBits;
}THUMBNAIL_DESCRIPTOR;

CxImage*  CDraftTemplateLoader::GetThumbnailFromFile(const char* filePath)
{
	CFile  file;
	CFileException  fe;

	if (!file.Open(filePath, CFile::modeRead, &fe))
	{
		ASSERT(0);
		return NULL;
	}

	DRAFT_TEMPLATE_HEADER main_header;
	memset(&main_header,0,sizeof(DRAFT_TEMPLATE_HEADER));

	if (file.Read(&main_header,sizeof(DRAFT_TEMPLATE_HEADER))!=sizeof(DRAFT_TEMPLATE_HEADER)) 
	{
		ASSERT(0);
		file.Close();
		return NULL;
	}

	if(strcmp(main_header.szSign,"DRAFT_TEMPL"))
	{
		ASSERT(0);
		file.Close();
		return NULL;
	}

	THUMBNAIL_DESCRIPTOR  th_descr;
	memset(&th_descr,0,sizeof(THUMBNAIL_DESCRIPTOR));

	if (file.Read(&th_descr.imageType,sizeof(int))!=sizeof(int)) 
	{
		ASSERT(0);
		file.Close();
		return NULL;
	}

	if (file.Read(&th_descr.imageSize,sizeof(unsigned int))!=sizeof(unsigned int)) 
	{
		ASSERT(0);
		file.Close();
		return NULL;
	}

		try
		{
			th_descr.imageBits = new BYTE[th_descr.imageSize];
		}
		catch (std::bad_alloc)
		{
			AfxMessageBox("bad_alloc exception in GetThumbnailFromFile function");
			ASSERT(0);
			file.Close();
			return NULL;
		}

		if (file.Read(th_descr.imageBits,th_descr.imageSize*sizeof(BYTE))!=th_descr.imageSize*sizeof(BYTE))
		{
			delete[] th_descr.imageBits;
			ASSERT(0);
			file.Close();
			return NULL;
		}

		CxImage* img = new CxImage(th_descr.imageBits, th_descr.imageSize, th_descr.imageType);

		if (!img->IsValid())
		{
			AfxMessageBox(img->GetLastError());
			delete img;
			img = NULL;
			delete[] th_descr.imageBits;
			ASSERT(0);
			return NULL;
		}
		
		delete[] th_descr.imageBits;
		file.Close();

		return img;
}


typedef struct
{
	int a_f;
	int b_f;
	int c_f;
	int d_f;
	int e_f;
	double A_f;
	double B_f;
	double C_f;
	double D_f;
	double E_f;
} TEMPLATE_RESERVE_FIELDS;

static TEMPLATE_RESERVE_FIELDS templ_reserve;

bool  CDraftTemplateLoader::SaveDraftTemplateInFile(const char* thumbnailFile,CDiagramEntityContainer* obj_container,
														const char* targetFile)
{
	CFile f;

	if( !f.Open( targetFile, CFile::modeCreate | CFile::modeWrite ) ) 
	{
		return false;
	}
	

	CArchive ar( &f, CArchive::store);

	DRAFT_TEMPLATE_HEADER main_header;
	memset(&main_header,0,sizeof(DRAFT_TEMPLATE_HEADER));

	//strcpy(main_header.szSign,_T("DRAFT_TEMPL"));  #OBSOLETE
	strcpy_s(main_header.szSign, sizeof (main_header.szSign),"DRAFT_TEMPL");

	main_header.nVersion	= 1;

	ar.Write(&main_header,sizeof(DRAFT_TEMPLATE_HEADER));

		// try to write image bits
	{
		THUMBNAIL_DESCRIPTOR  th_descr;
		memset(&th_descr,0,sizeof(THUMBNAIL_DESCRIPTOR));

		CFile			Picfile;
		CFileException	fe;

		if (!Picfile.Open(thumbnailFile, CFile::modeRead, &fe))
		{
			ASSERT(0);
			ar.Close();
			return false;
		}

		th_descr.imageSize = (unsigned int)Picfile.GetLength();
		th_descr.imageType = CXIMAGE_FORMAT_JPG;

		try
		{
			th_descr.imageBits = new BYTE[th_descr.imageSize];
		}
		catch (std::bad_alloc) 
		{
			AfxMessageBox("bad_alloc exception in AddNewTexture function");
			ASSERT(0);
			ar.Close();
			return false;
		}

		if (Picfile.Read(th_descr.imageBits,th_descr.imageSize*sizeof(BYTE))!=th_descr.imageSize*sizeof(BYTE))
		{
			delete[] th_descr.imageBits;
			ASSERT(0);
			ar.Close();
			return false;
		}

		ar.Write(&th_descr.imageType,sizeof(int));
		ar.Write(&th_descr.imageSize,sizeof(unsigned int));
		ar.Write(th_descr.imageBits,th_descr.imageSize*sizeof(BYTE));
		delete[] th_descr.imageBits;

		Picfile.Close();
	}

	memset(&templ_reserve,0,sizeof(TEMPLATE_RESERVE_FIELDS));
	ar.Write(&templ_reserve,sizeof(TEMPLATE_RESERVE_FIELDS));

	int tmpPrM = obj_container->GetPrinterMode();
	ar.Write(&tmpPrM,sizeof(int));// save page orient
	const CSize*  pS = obj_container->GetPageSizes();
	ASSERT(pS->cx>0);
	ASSERT(pS->cy>0);
	ar.Write(&(pS->cx),sizeof(INT));
	ar.Write(&(pS->cy),sizeof(INT));
	
	// objects
	{
		int objCnt = obj_container->GetSize();
		for (int j=0;j<objCnt;j++)
		{
			CDiagramEntity* de = obj_container->GetAt(j);
			DIAGRAM_OBJECT_TYPE dot = de->GetEntityType();
			switch(dot) 
			{
			case DIAGRAM_LINE:
			case DIAGRAM_RECT:
			case DIAGRAM_LABEL:
			case DIAGRAM_PICTURE:
				ar.Write(&dot,sizeof(int)); // save object type
				de->Serialize(ar);
				break;
			default:
				ASSERT(0);
				break;
			}
		}
	}


	ar.Close();
	return true;
	
}


#include "..//ReportEditor/ReportEntityLine.h"
#include "..//ReportEditor/ReportEntityBox.h"
#include "..//ReportEditor/ReportEntityLabel.h"
#include "..//ReportEditor/ReportEntityPicture.h"

CDiagramEntityContainer*  CDraftTemplateLoader::LoadDraftTemplate(const char* filePath)
{
	CFile f;

	if( !f.Open( filePath, CFile::modeRead ) ) 
	{
		return NULL;
	}


	CArchive ar( &f, CArchive::load);

	DRAFT_TEMPLATE_HEADER main_header;
	memset(&main_header,0,sizeof(DRAFT_TEMPLATE_HEADER));

	if (ar.Read(&main_header,sizeof(DRAFT_TEMPLATE_HEADER))!=sizeof(DRAFT_TEMPLATE_HEADER)) 
	{
		ASSERT(0);
		ar.Close();
		return NULL;
	}

	if(strcmp(main_header.szSign,"DRAFT_TEMPL"))
	{
		ASSERT(0);
		ar.Close();
		return NULL;
	}

	THUMBNAIL_DESCRIPTOR  th_descr;
	memset(&th_descr,0,sizeof(THUMBNAIL_DESCRIPTOR));

	if (ar.Read(&th_descr.imageType,sizeof(int))!=sizeof(int)) 
	{
		ASSERT(0);
		ar.Close();
		return NULL;
	}

	if (ar.Read(&th_descr.imageSize,sizeof(unsigned int))!=sizeof(unsigned int)) 
	{
		ASSERT(0);
		ar.Close();
		return NULL;
	}

	try
	{
		th_descr.imageBits = new BYTE[th_descr.imageSize];
	}
	catch (std::bad_alloc)
	{
		AfxMessageBox("bad_alloc exception in GetThumbnailFromFile function");
		ASSERT(0);
		ar.Close();
		return NULL;
	}

	if (ar.Read(th_descr.imageBits,th_descr.imageSize*sizeof(BYTE))!=th_descr.imageSize*sizeof(BYTE))
	{
		delete[] th_descr.imageBits;
		ASSERT(0);
		ar.Close();
		return NULL;
	}

	delete[] th_descr.imageBits;

	ar.Read(&templ_reserve,sizeof(TEMPLATE_RESERVE_FIELDS));

	int tmpPrM;
	ar.Read(&tmpPrM,sizeof(int));// save page orient
	CSize nSz;
	ar.Read(&(nSz.cx),sizeof(INT));
	ar.Read(&(nSz.cy),sizeof(INT));
	ASSERT(nSz.cx>0);
	ASSERT(nSz.cy>0);

	CDiagramEntityContainer* newCont = new CDiagramEntityContainer(tmpPrM,&(nSz),&( theApp.m_clip ));
	
	int    obj_t = 0;
	while (ar.Read(&obj_t,sizeof(int))) 
	{
		switch(obj_t) 
		{
		case DIAGRAM_LINE:
			{
				CReportEntityLine* nL = new CReportEntityLine;
				nL->Serialize(ar);
				newCont->Add(nL);
			}
			break;
		case DIAGRAM_RECT:
			{
				CReportEntityBox* bx = new CReportEntityBox;
				bx->Serialize(ar);
				newCont->Add(bx);
			}
			break;
		case DIAGRAM_LABEL:
			{
				CReportEntityLabel* lb = new CReportEntityLabel;
				lb->Serialize(ar);
				newCont->Add(lb);
			}
			break;
		case DIAGRAM_PICTURE:
			{
				CReportEntityPicture* pic = new CReportEntityPicture;
				pic->Serialize(ar);
				newCont->Add(pic);
			}
			break;
		default:
			ASSERT(0);
			ar.Close();
			return NULL;
		}
	}

	ar.Close();
	
	return newCont;

}

/*
bool  CMatLoader::AttachMatLibrary(LPCTSTR path)
{
  if (IsAttached())
    DetachMatLibrary();


  CFile  file;
  CFileException  fe;

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

  file.Close();

  return true;
}

void CMatLoader::CalcOpenGLTexture(TEXTURE_ITEM* text_item)
{
  CBitmap*    pBitmap;
  BITMAP      BmpInfo;

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
  pBMIH->biSize       = sizeof(BITMAPINFOHEADER);
  pBMIH->biWidth      = text_item->opengl_texture->Width;
  pBMIH->biHeight     = text_item->opengl_texture->Height;
  pBMIH->biPlanes     = 1;
  pBMIH->biBitCount     = 24 ; //32?? Use 32 bpp to avoid boundary alignment issues.
  pBMIH->biCompression    = BI_RGB ;

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

  int   iImageSize = text_item->opengl_texture->Width*
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
      pDest->rgbRed = rgb.rgbRed; //R;
      pDest->rgbGreen = rgb.rgbGreen; //G;
      pDest->rgbBlue  = rgb.rgbBlue;  //B;
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
    text_item->opengl_texture->Height,      // <<---
    0, GL_RGB, GL_UNSIGNED_BYTE,
    text_item->opengl_texture->pPicBits);         //      |
  //    |

  //TexMapScalePow2();                        //    |
  // масштабируем
  {
    GLint glMaxTexDim ;
    double xPow2, yPow2;
    int ixPow2, iyPow2;
    int xSize2, ySize2;
    int res;

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
}*/