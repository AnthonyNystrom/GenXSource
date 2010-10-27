/*! 
@file Graphics.h
@brief Class GraphicsSystem
*/

#ifndef __FRAMEWORK_GRAPHICS_H__
#define __FRAMEWORK_GRAPHICS_H__

#define MAX_ALPHA 16

#define GET_R(color)	(uint8)(((color << rShiftLeft) >> rShiftRight) << rShiftAlign)
#define GET_G(color)	(uint8)(((color << gShiftLeft) >> gShiftRight) << gShiftAlign)
#define GET_B(color)	(uint8)(((color << bShiftLeft) >> bShiftRight) << bShiftAlign)
#define NATIVE_RGB(r,g,b)	((((r) >> rShiftAlign) << rShiftBack)|(((g) >> gShiftAlign) << gShiftBack)|(((b) >> bShiftAlign) << bShiftBack))

#define PIXEL_APPLY_ALPHA(dstColor, srcColor, cfAlpha) \
		(PixelType)(NATIVE_RGB(((GET_R(srcColor) * cfAlpha) >> 4) + ((GET_R(dstColor) * (MAX_ALPHA - cfAlpha)) >> 4), \
							  ((GET_G(srcColor) * cfAlpha) >> 4) + ((GET_G(dstColor) * (MAX_ALPHA - cfAlpha)) >> 4), \
							  ((GET_B(srcColor) * cfAlpha) >> 4) + ((GET_B(dstColor) * (MAX_ALPHA - cfAlpha)) >> 4)))

#define PIXEL_APPLY_ALPHA_DST(dstColor, srcR, srcG, srcB, cfAlpha) \
		(PixelType)(NATIVE_RGB(srcR + ((GET_R(dstColor) * (MAX_ALPHA - cfAlpha)) >> 4), \
							  srcG + ((GET_G(dstColor) * (MAX_ALPHA - cfAlpha)) >> 4), \
							  srcB + ((GET_B(dstColor) * (MAX_ALPHA - cfAlpha)) >> 4)))

#define PIXEL_APPLY_ALPHA_SRCRGB(dstColor, srcR, srcG, srcB, cfAlpha) \
		(PixelType)(NATIVE_RGB(((srcR * cfAlpha) >> 4) + ((GET_R(dstColor) * (MAX_ALPHA - cfAlpha)) >> 4), \
							  ((srcG * cfAlpha) >> 4) + ((GET_G(dstColor) * (MAX_ALPHA - cfAlpha)) >> 4), \
							  ((srcB * cfAlpha) >> 4) + ((GET_B(dstColor) * (MAX_ALPHA - cfAlpha)) >> 4)))

#include "Application.h"
#include "FixedMath.h"

//! Class is used for drawing graphics primitives and raster objects
class GraphicsSystem
{
	typedef void (GraphicsSystem::*BltFunc)(void * pData, 
											void * pPal, 
											int32 dstX, 
											int32 dstY,
											int32 width,
											int32 height,
											bool transparent,
											uint32 transparentColor,
											uint8 nManip);

#ifdef ROT_BLT_USED
	typedef void (GraphicsSystem::*BltRotateFunc)(void * pData, 
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

public:

	//! Raster format
	enum eRasterFormat
	{
		ERF_NATIVE,			//!< Native. Pixel format in phone screen format.
		ERF_NATIVE_PAL8,	//!< 256 colors palette. Pixel format in phone screen format.
		ERF_NATIVE_PAL4,	//!< 16 colors palette. Pixel format in phone screen format.
		ERF_NATIVE_PAL2,	//!< 4 colors palette. Pixel format in phone screen format.
		ERF_4444,			//!< Non palette. Pixel format is 4444 RGBA.
		ERF_4444_PAL8,		//!< 256 colors palette. Pixel format is 4444 RGBA.
		ERF_NATIVE_PAL4A4,	//!< 16 colors palette. Pixel format in phone screen format.

		ERF_FORMATS_COUNT
	};

	//! Blending modes for sprites
	enum eBlendMode
	{
		//! @brief No blending.
		EBM_NONE,
		//! @brief Alpha blending.
		//! Transparency level is set in @ref SetAlpha()
		EBM_ALPHA,

	    //! @brief Grayscale 
		//! Works for ERF_4444 format
		EBM_GRAYSCALE,
		
		//! @brief Brightness
		//! РWorks for ERF_4444 format
		//! Function @ref SetBrightness() sets brightness coefficient
		EBM_BRIGHTNESS
	};

	//! @brief Sprite manipulations.
	//! Several manipulations can be applied together.
	enum eFlipMode
	{
		EFM_HFLIP = 0x01,//!< Horizontal flip.
		EFM_VFLIP = 0x02,//!< Vertical flip.
		EFM_ROTATE90 = 0x04//!< Rotate 90 degrees CW.
	};

	//! @brief Surface struct
	struct Surface
	{
		void * pData;//!< Pointer to first scan line.
		uint16 width;//!< Width.
		uint16 height;//!< Height.
		uint32 depth;//!< Depth.
		Application::eColorScheme colorScheme;//!< Color scheme.
	};

	//! @brief Constructor
	//! @param[in] app - Pointer to Application.
	GraphicsSystem(Application * app);
	//! @brief Destructor. 
	~GraphicsSystem();

	void InitBltModes()
	{
		if (deviceSurfaceArray[0].depth == 16)
		{
#ifndef GRAPHICS16_REDUCE
#ifdef USE_BLT_NATIVE
			InitBltNativeG16();
#endif
#ifdef USE_BLT_NATIVE_PAL8
			InitBltPal8G16();
#endif
#ifdef USE_BLT_NATIVE_PAL4A4
			InitBltPal4A4G16();
#endif
#ifdef USE_BLT_NATIVE_PAL4
			InitBltPal4G16();
#endif
#ifdef USE_BLT_NATIVE_PAL2
			InitBltPal2G16();
#endif
#ifdef USE_BLT_4444
			InitBlt4444G16();
#endif
#ifdef USE_BLT_4444_PAL8
			InitBlt4444Pal8G16();
#endif


#ifdef USE_ROT_BLT_NATIVE
			InitBltRotateNativeG16();
#endif
#ifdef USE_ROT_BLT_NATIVE_PAL8
			InitBltRotatePal8G16();
#endif
#ifdef USE_ROT_BLT_NATIVE_PAL4
			InitBltRotatePal4G16();
#endif
#ifdef USE_ROT_BLT_NATIVE_PAL2
			InitBltRotatePal2G16();
#endif
#ifdef USE_ROT_BLT_4444
			InitBltRotate4444G16();
#endif
#ifdef USE_ROT_BLT_4444_PAL8
			InitBltRotate4444Pal8G16();
#endif
#endif//GRAPHICS16_REDUCE
		}
		else if (deviceSurfaceArray[0].depth == 32)
		{
#ifndef GRAPHICS32_REDUCE
#ifdef USE_BLT_NATIVE
			InitBltNativeG32();
#endif
#ifdef USE_BLT_NATIVE_PAL8
			InitBltPal8G32();
#endif
#ifdef USE_BLT_NATIVE_PAL4A4
			InitBltPal4A4G32();
#endif
#ifdef USE_BLT_NATIVE_PAL4
			InitBltPal4G32();
#endif
#ifdef USE_BLT_NATIVE_PAL2
			InitBltPal2G32();
#endif
#ifdef USE_BLT_4444
			InitBlt4444G32();
#endif
#ifdef USE_BLT_4444_PAL8
			InitBlt4444Pal8G32();
#endif


#ifdef USE_ROT_BLT_NATIVE
			InitBltRotateNativeG32();
#endif
#ifdef USE_ROT_BLT_NATIVE_PAL8
			InitBltRotatePal8G32();
#endif
#ifdef USE_ROT_BLT_NATIVE_PAL4
			InitBltRotatePal4G32();
#endif
#ifdef USE_ROT_BLT_NATIVE_PAL2
			InitBltRotatePal2G32();
#endif
#ifdef USE_ROT_BLT_4444
			InitBltRotate4444G32();
#endif
#ifdef USE_ROT_BLT_4444_PAL8
			InitBltRotate4444Pal8G32();
#endif
#endif//GRAPHICS32_REDUCE
		}
	}
	




	//! @brief Detect native pixel format.
	//! @param[in] Pixel in native format which has maximal brightness in red component.
	//! @param[in] Pixel in native format which has maximal brightness in green component.
	//! @param[in] Pixel in native format which has maximal brightness in blue component.
	void DetectNativeFormat(uint32 colorR, uint32 colorG, uint32 colorB);

	// ***************************************************
	//! \brief    	SetPaletteColor - Set palette color.
	//! 
	//! \param      pPalette - palette.
	//! \param      index - index.
	//! \param      r - red component.
	//! \param      g - green component.
	//! \param      b - blue component.
	//! \return   	void
	// ***************************************************
	virtual void SetPaletteColor(uint16 * pPalette, uint8 index, uint8 r, uint8 g, uint8 b) = 0;

	//! @brief Set current drawing color.
	//! @param[in] r,g,b - color value.
	virtual void	SetColor(uint8 r, uint8 g, uint8 b) = 0;

	//! @brief Convert RGB components to native color pixel.
	uint32 Rgb2Native(uint8 r, uint8 g, uint8 b);

	//! @brief Converts native pixel to RGB components.
	void Native2Rgb(uint32 color, uint8 & r, uint8 & g, uint8 & b);

	//! @brief Set brightness coefficient.
	//! @param[in] brightness - coefficient which is added to current RGB components.
	//! Pixel calculation formula.
	//! r = CLAMP(r + brightness, 0, 255) 
	//!	g = CLAMP(g + brightness, 0, 255)
	//!	b = CLAMP(b + brightness, 0, 255)
	void		SetBrightness(int8 brightness);

	//! @brief Set transparent coefficient.
	//! @param[in] alpha - 0 - fully transparency, MAX_ALPHA - fully opaque.
	//! Value clamped between 0 and MAX_ALPHA
	void		SetAlpha(uint8 alpha);

	//! @brief Set clipping rect.
	//! All drawing operations valid only in this rect.
	void		SetClip(int32 x = 0,int32 y = 0,int32 width  = 10000, int32 height = 10000);

	//! @brief Set clip intersection.
	//! All drawing operations valid only in the intersection of rectangles.
	void		SetClipIntersect(int32 x, int32 y, int32 width, int32 height);

	//! @brief Get current clipping rect.
	void		GetClip(int32 * x, int32 * y, int32 * width, int32 * height);

	//! @brief Set blending modes.
	void		SetBlendMode(eBlendMode blendMode);

	//! @brief Clear the screen with the current color.
	virtual void	Clear() = 0;

	//! @brief Drawing a line with the current color.
	virtual void	DrawLine(int32 x1, int32 y1, int32 x2, int32 y2, int8 width = 1) = 0;

	//! @brief Drawing a rect with the current color.
	virtual void	DrawRect(int32 x, int32 y, int32 width, int32 height) = 0;

	//! @brief Filling a rect with the current color.
	virtual void	FillRect(int32 x, int32 y, int32 width, int32 height) = 0;

	//! @brief Filling a rect with a  vertical gradient from the current color to a needed color.
	virtual void	FillGradientV(const Rect& rect, uint8 nr, uint8 ng, uint8 nb) = 0;

	//! @brief Drawing ellipse inserted into a rectangle.
	//! @param[in] x1, y1 - coordinates of left upper corner of the rect.
	//! @param[in] x2, y2 - coordinates of right bottom corner of the rect.
	virtual void	DrawEllipse(int32 x1, int32 y1, int32 x2, int32 y2) = 0;

	//! @brief Filling ellipse inserted into a rectangle.
	//! @param[in] x1, y1 - coordinates of left upper corner of the rect.
	//! @param[in] x2, y2 - coordinates of right bottom corner of the rect.
	virtual void	FillEllipse(int32 x1, int32 y1, int32 x2, int32 y2) = 0;

	//! @brief Horizontal line drawing.
	//! @param[in] x1, y1 - coordinates of first point.
	//! @param[in] x2 - coordinate x of second point.
	virtual void	DrawHLine(int32 x1, int32 y1, int32 x2) = 0;

	//! @brief Vertical line drawing.
	//! @param[in] x1, y1 - coordinates of first point.
	//! @param[in] y2 - coordinate y of second point.
	virtual void	DrawVLine(int32 x1, int32 y1, int32 y2) = 0;

	//! @brief Point drawing.
	//! @param[in] x, y - point coordinates.
	virtual void	DrawPixel(int32 x, int32 y) = 0;

	//! @brief New graphic surface creation.
	//! @param[in] width, height - surface dimensions.
	//! @return pointer to the created surface.
	virtual Surface		* CreateNativeSurface(uint16 width, uint16 height) = 0;

	//! @brief Delete graphic surface.
	//! If surface == NULL, nothing happens.
	//! @param[in] surface - pointer to the surface being deleted.
	virtual void		ReleaseNativeSurface(Surface * surface) = 0;

	//! @brief Set active screen surface. All drawing operations occur in this surface.
	//! @param[in] surface - surface which needs to be active.
	//! If surface == NULL, the current phone screen surface be comes active.
	//! Index of the current phone screen surface can be set with the help of @ref SetCurrentDeviceSurface function.
	//! All phone screen surfaces can be received with the help of @ref Application::GetDisplayArray() function.
	void				SetCurrentSurface(Surface * surface);

	//! @brief Set active phone screen surface 
	//! @param[in] screenIndex - Phone screen buffer index which must be set.
	void				SetCurrentDeviceSurface(uint32 screenIndex);	

	//! @brief Copy one surface to the other surface
	//! If src == dest, copying doesn't occur
	//! @param[in] src - source surface
	//! @param[in] dest - destination surface
	//! @param[in] xDst, yDst - coordinates for copying to dest
	//! @param[in] xSrc, ySrc - coordinates of copying source
	//! @param[in] copyWidth, copyHeight - dimensions of the rectangle to be copied
	virtual void		CopySurface(Surface * src, Surface * dest, int32 xDst, int32 yDst, int32 xSrc, int32 ySrc, int32 copyWidth, int32 copyHeight) = 0;

	//! @brief Get screen buffer width with the set index
	//! @param[in] screenIndex - phone screen buffer index for which we are getting the width
	uint16 GetWidth(uint32 screenIndex = 0);

	//! @brief Get screen buffer height with the set index
	//! @param[in] screenIndex - phone screen buffer index for which we are getting the height
	uint16 GetHeight(uint32 screenIndex = 0);

	//! @brief Get pointer to the physical phone screen surface
	Surface * GetDeviceSurface(uint32 screenIndex = 0);
	
	//! @brief Convert 565 RGB pixel to the phone native format
	//! param[in] pRaster - pointer to the color buffer
	//! param[in] numOfColors - number of colors
	//! @return - Pointer to the color buffer if the native screen depth is 32 bits
	//! NULL - if the native screen depth is 16 bits, converted colors are in the old buffer
	virtual uint32 * Convert565ToNative(uint16 * pData, uint32 numOfColors) = 0;


	// ***************************************************
	//! \brief    	DrawSurface - Draw surface in the current screen surface
	//! 
	//! \param      srcSurface - source surface
	//! \param      dstX - destination x coordinate
	//! \param      dstY - destination y coordinate
	//! \param      transparent - draw transparentColor or not
	//! \param      transparentColor - transparency color
	//! \param      nManip - manipulation code
	//! \return   	void
	// ***************************************************
	void DrawSurface(Surface * srcSurface, 
					int32 dstX, 
					int32 dstY, 
					bool transparent, 
					uint32 transparentColor, 
					uint8 manip);

	//! @brief Raster area copying to the current surface
	//! @param[in] rasterFormat - raster format
	//! @param[in] pData - pointer to the raster
	//! @param[in] pPal - pointer to the palette (if the raster is palette)
	//! @param[in] dstX, dstY - destination surface coordinates
	//! @param[in] width, height - width and height of the copied area
	//! @param[in] transparent - draw transparentColor or not
	//! @param[in] transparentColor - transparency color
	//! @param[in] nManip - manipulation code
	void BitBlt(eRasterFormat rasterFormat,
					void * pData, 
					void * pPal, 
					int32 dstX, 
					int32 dstY,
					int32 width,
					int32 height,
					bool transparent,
					uint32 transparentColor,
					uint8 nManip);

#ifdef ROT_BLT_USED
	//! @brief Copy raster area to the current surface with rotation and scale operations.
	//! @param[in] rasterFormat - raster format
	//! @param[in] pData - pointer to the raster
	//! @param[in] pPal - pointer to the palette (if the raster is palette)
	//! @param[in] dstX, dstY - copy destination coordinates
	//! @param[in] xVector, yVector - rotation center movement
	//! @param[in] width, height - width and height of the area being copied
	//! @param[in] transparent - draw transparentColor or not
	//! @param[in] transparentColor - transparency color
	//! @param[in] angel - rotation angle
	//! @param[in] scale - scaling coefficient
	void BitBltRotate(eRasterFormat rasterFormat,
					void * pData, 
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

protected:

#ifdef USE_BLT_NATIVE
	#ifndef GRAPHICS16_REDUCE
	void InitBltNativeG16();
	#endif
	#ifndef GRAPHICS32_REDUCE
	void InitBltNativeG32();
	#endif
#else
	void InitBltNativeG16(){};
	void InitBltNativeG32(){};
#endif

#ifdef USE_BLT_NATIVE_PAL8
	#ifndef GRAPHICS16_REDUCE
	void InitBltPal8G16();
	#endif
	#ifndef GRAPHICS32_REDUCE
	void InitBltPal8G32();
	#endif
#else
	void InitBltPal8G16(){};
	void InitBltPal8G32(){};
#endif

#ifdef USE_BLT_NATIVE_PAL4A4
	#ifndef GRAPHICS16_REDUCE
	void InitBltPal4A4G16();
	#endif
	#ifndef GRAPHICS32_REDUCE
	void InitBltPal4A4G32();
	#endif
#else
	void InitBltPal4A4G16(){};
	void InitBltPal4A4G32(){};
#endif

#ifdef USE_BLT_NATIVE_PAL4
	#ifndef GRAPHICS16_REDUCE
	void InitBltPal4G16();
	#endif
	#ifndef GRAPHICS32_REDUCE
	void InitBltPal4G32();
	#endif
#else
	void InitBltPal4G16(){};
	void InitBltPal4G32(){};
#endif

#ifdef USE_BLT_NATIVE_PAL2
	#ifndef GRAPHICS16_REDUCE
	void InitBltPal2G16();
	#endif
	#ifndef GRAPHICS32_REDUCE
	void InitBltPal2G32();
	#endif
#else
	void InitBltPal2G16(){};
	void InitBltPal2G32(){};
#endif

#ifdef USE_BLT_4444
	#ifndef GRAPHICS16_REDUCE
	void InitBlt4444G16();
	#endif
	#ifndef GRAPHICS32_REDUCE
	void InitBlt4444G32();
	#endif
#else
	void InitBlt4444G16(){};
	void InitBlt4444G32(){};
#endif

#ifdef USE_BLT_4444_PAL8
	#ifndef GRAPHICS16_REDUCE
	void InitBlt4444Pal8G16();
	#endif
	#ifndef GRAPHICS32_REDUCE
	void InitBlt4444Pal8G32();
	#endif
#else
	void InitBlt4444Pal8G16(){};
	void InitBlt4444Pal8G32(){};
#endif


#ifdef USE_ROT_BLT_NATIVE
	#ifndef GRAPHICS16_REDUCE
	void InitBltRotateNativeG16();
	#endif
	#ifndef GRAPHICS32_REDUCE
	void InitBltRotateNativeG32();
	#endif
#else
	void InitBltRotateNativeG16(){};
	void InitBltRotateNativeG32(){};
#endif

#ifdef USE_ROT_BLT_NATIVE_PAL8
	#ifndef GRAPHICS16_REDUCE
	void InitBltRotatePal8G16();
	#endif
	#ifndef GRAPHICS32_REDUCE
	void InitBltRotatePal8G32();
	#endif
#else
	void InitBltRotatePal8G16(){};
	void InitBltRotatePal8G32(){};
#endif

#ifdef USE_ROT_BLT_NATIVE_PAL4
	#ifndef GRAPHICS16_REDUCE
	void InitBltRotatePal4G16();
	#endif
	#ifndef GRAPHICS32_REDUCE
	void InitBltRotatePal4G32();
	#endif
#else
	void InitBltRotatePal4G16(){};
	void InitBltRotatePal4G32(){};
#endif

#ifdef USE_ROT_BLT_NATIVE_PAL2
	#ifndef GRAPHICS16_REDUCE
	void InitBltRotatePal2G16();
	#endif
	#ifndef GRAPHICS32_REDUCE
	void InitBltRotatePal2G32();
	#endif
#else
	void InitBltRotatePal2G16(){};
	void InitBltRotatePal2G32(){};
#endif

#ifdef USE_ROT_BLT_4444
	#ifndef GRAPHICS16_REDUCE
	void InitBltRotate4444G16();
	#endif
	#ifndef GRAPHICS32_REDUCE
	void InitBltRotate4444G32();
	#endif
#else
	void InitBltRotate4444G16(){};
	void InitBltRotate4444G32(){};
#endif

#ifdef USE_ROT_BLT_4444_PAL8
	#ifndef GRAPHICS16_REDUCE
	void InitBltRotate4444Pal8G16();
	#endif
	#ifndef GRAPHICS32_REDUCE
	void InitBltRotate4444Pal8G32();
	#endif
#else
	void InitBltRotate4444Pal8G16(){};
	void InitBltRotate4444Pal8G32(){};
#endif





	virtual void FindShifts(uint32 color, uint32 *pShiftLeft, uint32 *pShiftRight, uint32 *pShiftAlign,uint32 *pShiftBack) = 0;
 
	BltFunc BitBltFuncArray[ERF_FORMATS_COUNT];
#ifdef ROT_BLT_USED
	BltRotateFunc BitBltRotateFuncArray[ERF_FORMATS_COUNT];
#endif

	bool Clipping(int32 & srcOffset, 
		int32 & dstOffset, 
		int32 & x, 
		int32 & y, 
		int32 & nWidth, 
		int32 & nHeight, 
		int32 & yAddSrc, 
		int32 & xAddDst, 
		int32 & yAddDst, 
		uint8 & nManip);

#ifdef ROT_BLT_USED
	bool GraphicsSystem::ClippingRotate(int32 & dstX, 
		int32 & dstY, 
		int32 & xVectorDestWidth, // на вход xVector на выходе destWidth
		int32 & yVectorDestHeight, // на вход yVector на выходе destHeight
		Fixed & srcX, 
		Fixed & srcY, 
		Fixed srcWidth, 
		Fixed srcHeight, 
		Fixed & angleDdx, // на вход angle на выходе ddx
		Fixed & scaleSdy); // на вход scale на выходе ddy
#endif


	void * activeBufferData;
	int32 activeBufferWidth;
	int32 activeBufferHeight;

	int32 clipStartX;
	int32 clipEndX;
	int32 clipStartY;
	int32 clipEndY;

	uint8 blendMode;
	uint8 alpha;
	int8 brightness;

	Surface * deviceSurfaceArray;
	uint8 currentDeviceSurface;
	uint8 deviceSurfaceCount;

	uint32 rShiftLeft;
	uint32 rShiftRight;
	uint32 rShiftAlign;
	uint32 rShiftBack;
	uint32 gShiftLeft;
	uint32 gShiftRight;
	uint32 gShiftAlign;
	uint32 gShiftBack;
	uint32 bShiftLeft;
	uint32 bShiftRight;
	uint32 bShiftAlign;
	uint32 bShiftBack;

	uint8 currentR;
	uint8 currentG;
	uint8 currentB;

	uint8 alphaTable[17][256];
};
#endif //__FRAMEWORK_GRAPHICS_H__