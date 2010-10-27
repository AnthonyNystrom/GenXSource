#ifndef __FRAMEWORK_GRAPHICS32_H__
#define __FRAMEWORK_GRAPHICS32_H__

#include "Graphics.h"
#include "Config.h"
#ifndef GRAPHICS32_REDUCE

// Class ������������ ��� ��������� ����������� ����������
class GraphicsSystem32: public GraphicsSystem
{
public:

	typedef uint32 PixelType;
	//! @brief �����������
	//! @param[in] app - ��������� �� App, ������ ������� native �������
	//! �������� ������� ���������� ������ � ������� @ref Application::GetDisplayArray()
	GraphicsSystem32(Application * app);
	//! @brief ����������. 
	~GraphicsSystem32();

	virtual uint32 * Convert565ToNative(uint16 * pData, uint32 numOfColors);


	virtual void SetPaletteColor(uint16 * pPalette, uint8 index, uint8 r, uint8 g, uint8 b);
	//! @brief ��������� �������� �����.
	//! @param[in] r,g,b - �������� �����
	virtual void	SetColor(uint8 r, uint8 g, uint8 b);

	//! @brief ������� �������� ������ ������������� ������.
	virtual void	Clear();

	//! @brief ��������� ����� ������� ������
	//! �� ������� ������ ����� 1
	virtual void	DrawLine(int32 x1,int32 y1, int32 x2, int32 y2, int8 width=1);

	//! @brief ��������� ��������.
	virtual void	DrawRect(int32 x, int32 y, int32 width, int32 height);

	//! @brief ������� �������� ������� ������
	virtual void	FillRect(int32 x, int32 y, int32 width, int32 height);

	void FillGradientV( const Rect& rect, uint8 nr, uint8 ng, uint8 nb );

	//! @brief ��������� �������.
	//! @param[in] x1,y1 - ���������� ������ �������� ���� ��������������
	//! @param[in] x2,y2 - ���������� ������� ������� ���� ��������������
	virtual void	DrawEllipse(int32 x1, int32 y1, int32 x2, int32 y2);

	//! @brief ������� �������.
	//! @param[in] x1,y1 - ���������� ������ �������� ���� ��������������
	//! @param[in] x2,y2 - ���������� ������� ������� ���� ��������������
	virtual void	FillEllipse(int32 x1, int32 y1, int32 x2, int32 y2);

	//! @brief ��������� �������������� �����
	//! @param[in] x1,y1 - ���������� ����� ����� �����
	//! @param[in] x2 - ���������� x ������ �����
	virtual void	DrawHLine(int32 x1, int32 y1, int32 x2);

	//! @brief ��������� ������������ �����
	//! @param[in] x1,y1 - ���������� ����� ����� �����
	//! @param[in] y2 - ���������� y ������ �����
	virtual void	DrawVLine(int32 x1, int32 y1, int32 y2);

	//! @brief ��������� �����
	//! @param[in] x1,y1 - ���������� �����
	virtual void	DrawPixel(int32 x, int32 y);

	//! @brief �������� ��������� ������
	//! @param[in] width,height - ������� ������
	//! @return ��������� �� ��������� �����
	virtual Surface		* CreateNativeSurface(uint16 width, uint16 height);

	//! @brief ����������� ��������� ������.
	//! ���� surface == NULL, ������ �� ����������
	//! @param[in] surface - ��������� �� ������������ �����, ��� �������� ���������� ���������� NULL
	virtual void		ReleaseNativeSurface(Surface * surface);

	//! @brief ����������� �������
	//! ���� src == dest, ����������� �� ����������
	//! @param[in] src - ������ ��������
	//! @param[in] dest - ���� ��������
	//! @param[in] xDst,yDst - ���������� ��� ����������� � dest
	//! @param[in] xSrc,ySrc - ���������� ������ ��������
	//! @param[in] width,height - ������ src, ������� ����� ����������
	virtual void		CopySurface(Surface * src, Surface * dest, int32 xDst, int32 yDst, int32 xSrc, int32 ySrc, int32 copyWidth, int32 copyHeight);

#ifdef USE_ROT_BLT_NATIVE
	void	BitBltRotateNative(void * pData, 
		void * pPal, 
		int32 dstX, 
		int32 dstY,
		int32 xVector,
		int32 yVector,
		int32 width,
		int32 height,
		bool transparent,
		uint32 transparentColor,
		Fixed angle,
		Fixed scale);
#endif

#ifdef USE_ROT_BLT_4444
	void	BitBltRotate4444(void * pData, 
		void * pPal, 
		int32 dstX, 
		int32 dstY,
		int32 xVector,
		int32 yVector,
		int32 width,
		int32 height,
		bool transparent,
		uint32 transparentColor,
		Fixed angle,
		Fixed scale);
#endif

#ifdef USE_ROT_BLT_NATIVE_PAL8
	void	BitBltRotatePal8(void * pData, 
		void * pPal, 
		int32 dstX, 
		int32 dstY,
		int32 xVector,
		int32 yVector,
		int32 width,
		int32 height,
		bool transparent,
		uint32 transparentColor,
		Fixed angle,
		Fixed scale);
#endif

	void	BitBltRotateAssert(void * pData, 
		void * pPal, 
		int32 dstX, 
		int32 dstY,
		int32 xVector,
		int32 yVector,
		int32 width,
		int32 height,
		bool transparent,
		uint32 transparentColor,
		Fixed angle,
		Fixed scale);

#ifdef USE_BLT_NATIVE
	void	BitBltNative(void * pData, 
								void * pPal, 
								int32 dstX, 
								int32 dstY,
								int32 width,
								int32 height,
								bool transparent,
								uint32 transparentColor,
								uint8 nManip);
#endif

#ifdef USE_BLT_NATIVE_PAL8
	void	BitBltPal8(void * pData, 
								void * pPal, 
								int32 dstX, 
								int32 dstY,
								int32 width,
								int32 height,
								bool transparent, 
								uint32 transparentColor,
								uint8 nManip);
#endif

#ifdef USE_BLT_NATIVE_PAL4A4
	void	BitBltPal4A4(void * pData, 
									void * pPal, 
									int32 dstX, 
									int32 dstY,
									int32 width,
									int32 height,
									bool transparent, 
									uint32 transparentColor,
									uint8 nManip);
#endif

#ifdef USE_BLT_NATIVE_PAL4
	void	BitBltPal4(void * pData, 
								void * pPal, 
								int32 dstX, 
								int32 dstY,
								int32 width,
								int32 height,
								bool transparent, 
								uint32 transparentColor,
								uint8 nManip);
#endif

#ifdef USE_BLT_NATIVE_PAL2
	void	BitBltPal2(void * pData, 
								void * pPal, 
								int32 dstX, 
								int32 dstY,
								int32 width,
								int32 height,
								bool transparent, 
								uint32 transparentColor,
								uint8 nManip);
#endif

#ifdef USE_BLT_4444
	void	BitBlt4444(void * pData, 
								void * pPal, 
								int32 dstX, 
								int32 dstY,
								int32 width,
								int32 height,
								bool transparent,
								uint32 transparentColor,
								uint8 nManip);
#endif

#ifdef USE_BLT_4444_PAL8
	void	BitBlt4444Pal8(void * pData, 
									void * pPal, 
									int32 dstX, 
									int32 dstY,
									int32 width,
									int32 height,
									bool transparent,
									uint32 transparentColor,
									uint8 nManip);
#endif

private:

	virtual void FindShifts(uint32 color,uint32* pShiftLeft,uint32* pShiftRight,uint32* pShiftAlign,uint32* pShiftBack);

	PixelType currentColor;
	PixelType tempColor;

};
#endif
#endif //__FRAMEWORK_GRAPHICS_H__