#ifndef __FRAMEWORK_GRAPHICS32_H__
#define __FRAMEWORK_GRAPHICS32_H__

#include "Graphics.h"
#include "Config.h"
#ifndef GRAPHICS32_REDUCE

// Class предназначен для рисования графических примитивов
class GraphicsSystem32: public GraphicsSystem
{
public:

	typedef uint32 PixelType;
	//! @brief Конструктор
	//! @param[in] app - указатель на App, откуда берутся native дисплеи
	//! Активным буфером становится первый в массиве @ref Application::GetDisplayArray()
	GraphicsSystem32(Application * app);
	//! @brief Деструктор. 
	~GraphicsSystem32();

	virtual uint32 * Convert565ToNative(uint16 * pData, uint32 numOfColors);


	virtual void SetPaletteColor(uint16 * pPalette, uint8 index, uint8 r, uint8 g, uint8 b);
	//! @brief Установка текущего цвета.
	//! @param[in] r,g,b - значение цвета
	virtual void	SetColor(uint8 r, uint8 g, uint8 b);

	//! @brief Заливка текущего экрана установленным цветом.
	virtual void	Clear();

	//! @brief Рисование линии текущим цветом
	//! По дефолту ширина линии 1
	virtual void	DrawLine(int32 x1,int32 y1, int32 x2, int32 y2, int8 width=1);

	//! @brief Рисование квадрата.
	virtual void	DrawRect(int32 x, int32 y, int32 width, int32 height);

	//! @brief Заливка квадрата текущим цветом
	virtual void	FillRect(int32 x, int32 y, int32 width, int32 height);

	void FillGradientV( const Rect& rect, uint8 nr, uint8 ng, uint8 nb );

	//! @brief Рисование эллипса.
	//! @param[in] x1,y1 - координаты левого верхнего угла прямоугольника
	//! @param[in] x2,y2 - координаты правого нижнего угла прямоугольника
	virtual void	DrawEllipse(int32 x1, int32 y1, int32 x2, int32 y2);

	//! @brief Заливка эллипса.
	//! @param[in] x1,y1 - координаты левого верхнего угла прямоугольника
	//! @param[in] x2,y2 - координаты правого нижнего угла прямоугольника
	virtual void	FillEllipse(int32 x1, int32 y1, int32 x2, int32 y2);

	//! @brief Рисование горизонтальной линии
	//! @param[in] x1,y1 - координаты одной точки линии
	//! @param[in] x2 - координата x второй точки
	virtual void	DrawHLine(int32 x1, int32 y1, int32 x2);

	//! @brief Рисование вертикальной линии
	//! @param[in] x1,y1 - координаты одной точки линии
	//! @param[in] y2 - координата y второй точки
	virtual void	DrawVLine(int32 x1, int32 y1, int32 y2);

	//! @brief Рисование точки
	//! @param[in] x1,y1 - координаты точки
	virtual void	DrawPixel(int32 x, int32 y);

	//! @brief Создание экранного буфера
	//! @param[in] width,height - размеры буфера
	//! @return указатель на созданный буфер
	virtual Surface		* CreateNativeSurface(uint16 width, uint16 height);

	//! @brief Уничтожение экранного буфера.
	//! Если surface == NULL, ничего не происходит
	//! @param[in] surface - указатель на уничтожаемый буфер, при успешном выполнении становится NULL
	virtual void		ReleaseNativeSurface(Surface * surface);

	//! @brief Копирование буферов
	//! Если src == dest, копирование не происходит
	//! @param[in] src - откуда копируем
	//! @param[in] dest - куда копируем
	//! @param[in] xDst,yDst - координаты для копирования в dest
	//! @param[in] xSrc,ySrc - координаты откуда копируем
	//! @param[in] width,height - размер src, который будет скопирован
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