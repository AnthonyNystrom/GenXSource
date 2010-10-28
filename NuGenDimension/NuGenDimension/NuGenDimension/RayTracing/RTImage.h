#ifndef __RT_IMAGE__
#define __RT_IMAGE__

#include <math.h>

typedef struct _CGLRGBTRIPLE { 
	BYTE rgbRed;
	BYTE rgbGreen;
	BYTE rgbBlue;
} CGLRGBTRIPLE;


class CImageTexture : public rtTexture::rtCImageTexture
{
private:
  SIZE      m_size;
  double    m_scale;
  double    m_shift;
  double    m_angle;
  void*     m_picture_bits;
public:
  CImageTexture()
  {
    m_size.cx = m_size.cy = 0;
    m_scale = 1.0;
    m_shift = 0.0;
    m_angle = 0.0;
	m_picture_bits = NULL;
  }
  ~CImageTexture()
  {
	  if (m_picture_bits)
		  free(m_picture_bits);
  }
  double  GetScale()   {return m_scale;};
  double  GetShift()   {return m_shift;};
  double  GetAngle()   {return m_angle;};

  SIZE    GetSizes()   {return m_size;};
  const void*   GetPictureBits() const    {	  return m_picture_bits;  };
  void SetPictureBits(void *picture_bits, int ixSize, int iySize) 
  {	  
   if (m_picture_bits)
  	  free(m_picture_bits);
	m_picture_bits=picture_bits;  
	m_size.cx = ixSize ;
	m_size.cy = iySize ;
  };
 
  void    Create(int resourceID, double scale, double shift, double angle)
  {
	  if (m_picture_bits)
		  free(m_picture_bits);

	  m_picture_bits=NULL;
	  m_size.cx = m_size.cy = 0;

	  HBITMAP   tmp_bitmap=NULL;
	  HBITMAP   tmp_oldBitmap=NULL;
	  HDC       tmp_hDC=NULL;

	  tmp_hDC  = CreateCompatibleDC(NULL);

      tmp_bitmap =  (HBITMAP)LoadImage(GetModuleHandle(NULL),MAKEINTRESOURCE(resourceID),
            IMAGE_BITMAP, 0, 0, LR_CREATEDIBSECTION);

	if (!tmp_bitmap)
	{
		DeleteDC(tmp_hDC);
		return;
	}

    BITMAP BM;
    ::GetObject(tmp_bitmap, sizeof (BM), &BM);
    m_size.cx = BM.bmWidth;
    m_size.cy = BM.bmHeight;

	tmp_oldBitmap = (HBITMAP)SelectObject(tmp_hDC,tmp_bitmap);

	int		iImageSize = m_size.cx*m_size.cy*sizeof(CGLRGBTRIPLE);
	m_picture_bits = malloc(iImageSize);

	CGLRGBTRIPLE* pDest = (CGLRGBTRIPLE*)m_picture_bits;

	int i,j;
	for (i =  m_size.cy-1 ; i >=0; i--)
	{
		for(j = 0 ; j < m_size.cx; j++)
		{
			COLORREF  imCol = GetPixel(tmp_hDC, j, i);
			pDest->rgbRed	= GetRValue(imCol);	//R;
			pDest->rgbGreen = GetGValue(imCol);	//G;
			pDest->rgbBlue	= GetBValue(imCol);	//B;
			pDest++ ;
		}
	}

	SelectObject(tmp_hDC,tmp_oldBitmap);
	DeleteObject(tmp_bitmap);
	DeleteDC(tmp_hDC);

	glPixelStorei(GL_UNPACK_ALIGNMENT, 1);
	glTexImage2D(GL_TEXTURE_2D, 0, 3, 
		m_size.cx,m_size.cy,0, GL_RGB, GL_UNSIGNED_BYTE, m_picture_bits);	

	{
		GLint glMaxTexDim ;
		double xPow2, yPow2;
		int ixPow2, iyPow2;
		int xSize2, ySize2;
		int	res;

		::glGetIntegerv(GL_MAX_TEXTURE_SIZE, &glMaxTexDim);
		glMaxTexDim = min(128, glMaxTexDim);

		if (m_size.cx <= glMaxTexDim)
			xPow2 = log((double)m_size.cx) / log(2.0);
		else
			xPow2 = log((double)glMaxTexDim) / log(2.0);

		if (m_size.cy <= glMaxTexDim)
			yPow2 = log((double)m_size.cy) / log(2.0);
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
			return;
		}

		res = gluScaleImage(GL_RGB, m_size.cx, 
			m_size.cy,GL_UNSIGNED_BYTE, m_picture_bits,	
			xSize2, ySize2, GL_UNSIGNED_BYTE, pData);

		free(m_picture_bits);
		m_picture_bits = pData; 
		m_size.cx = xSize2 ;
		m_size.cy = ySize2 ;
	}
  }
  virtual  void     GetColor(double uCoord, double vCoord, rtTexture::RT_COLOR& col) const
  {
    double imW = (double)(m_size.cx);
    double imH = (double)(m_size.cy);

    SG_VECTOR scVec = {1.0/m_scale, 1.0/m_scale, 1.0};
    SG_VECTOR trVec = {m_shift, m_shift, 0.0};
    SG_VECTOR rotVec = {0.0, 0.0, m_angle};

    double x,y;
    x = uCoord*cos(rotVec.z/180.0*3.14159265)-vCoord*sin(rotVec.z/180.0*3.14159265)+trVec.x;
    y = uCoord*sin(rotVec.z/180.0*3.14159265)+vCoord*cos(rotVec.z/180.0*3.14159265)+trVec.y;

    double imP_x = (x)*scVec.x*imW;
    double imP_y = (y)*scVec.y*imH;

    col.m_red = 0.0f;
    col.m_green = 0.0f;
    col.m_blue = 0.0f;
    col.m_alpha = 0.0f;

    if (imP_x>imW)
    {
      while (imP_x>imW)
        imP_x-=imW;
    }
    if (imP_y>imH)
    {
      while (imP_y>imH)
        imP_y-=imH;
    }
    if (imP_x<0.0)
    {
      while (imP_x<0.0)
        imP_x+=imW;
    }
    if (imP_y<0.0)
    {
      while (imP_y<0.0)
        imP_y+=imH;

    }

	CGLRGBTRIPLE* pDest = (CGLRGBTRIPLE*)m_picture_bits;

	col.m_red = ((float)(pDest[m_size.cx*((int)imP_y)+(int)imP_x].rgbRed ))/255.0f;
	col.m_green = ((float)(pDest[m_size.cx*((int)imP_y)+(int)imP_x].rgbGreen))/255.0f;
	col.m_blue = ((float)(pDest[m_size.cx*((int)imP_y)+(int)imP_x].rgbBlue))/255.0f;
	col.m_alpha = 0.0f;
  }
};

class CImageMaterial : public rtCMaterial
{

public:
  CImageTexture  m_texture;

  CImageMaterial(int resourceID, double scale=1.0, double shift=0.0, double angle=0.0)
  {
	  m_texture.Create(resourceID, scale, shift, angle);
  }
  
  CImageMaterial(){}

  virtual   const    rtTexture::rtCTexture*   GetTexture() const
  {
    return &m_texture;
  }
};

#endif