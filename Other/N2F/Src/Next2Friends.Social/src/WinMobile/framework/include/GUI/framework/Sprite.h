/*!
@file	Sprite.h
@brief	Class Sprite
*/
#ifndef __FRAMEWORK_SPRITE_H__
#define __FRAMEWORK_SPRITE_H__

#include "Resource.h"
#include "FixedMath.h"
#include "Config.h"

class Graphics;
class Binary;

//! Class for working with sprites
class Sprite : public Resource
{
	friend class GraphicsSystem;
	friend class ResourceSystem;
	friend class Font;

private:

	//! Sprite colors formats
	enum eColorFormat
	{
		ECF_565,			//!< RGB sprite in 565 format
		ECF_4444,			//!< ARGB sprite in 4444 format
		ECF_565A4			//!< ARGB sprite in 4 format
	};

	//! Sprite pixel formats
	enum ePixelFormat
	{
		EPF_16BPP,			//!< 16 bits per dot
		EPF_8BPP,			//!< 8 bits per dot
		EPF_4BPP,			//!< 4 bits per dot
		EPF_2BPP			//!< 2 bits per dot
	};

	Sprite(Binary *binary, ResourceSystem *pResSystem, int16 resourceId, char8 * resourceName, GraphicsSystem * graphicsSys);
	virtual ~Sprite();

	bool Create(void *pData);

	Binary * binary;

	GraphicsSystem * graphicsSystem;

	uint32 currentFrame;

	uint32 transparentColor;

	void *pSpriteData;

	uint8 colorFormat;
	uint8 pixelFormat;
	uint8 transparence;

	uint16 numberOfColors;
	uint8 numberOfPalettes;
	uint32 palleteSize;
	bool needDelete;

	uint16 numberOfFrames;
	uint16 fullFrameWidth;
	uint16 fullFrameHeight;

	uint8 *pXOffsets;
	uint8 *pYOffsets;
	uint16 *pWidths;
	uint16 *pHeights;
	uint16 *pReferences;
	uint8 *pManipulations;
	uint32 *pOffsets;

	uint16 *pPalettes;
	uint8 currentPalette;

	uint32 rasterDataSize;

	uint16 *pRasterData;

	uint8 rasterFormat;

public:
#ifdef ROT_BLT_USED
	//! @brief Draw 0 frame with the set rotation and scale.
	//! @param[in] x - X coordinate
	//! @param[in] y - Y coordinate
	//! @param[in] angel - rotate angle
	//! @param[in] scale - scaling coefficient
	void DrawRotate(int32 x, int32 y, Fixed angle, Fixed scale);

	//! @brief Draw 0 frame with the set rotation and scale.
	//! @param[in] x - X coordinate
	//! @param[in] y - Y coordinate
	//! @param[in] angel - rotate angle
	//! @param[in] scale - scaling coefficient
	//! @param[in] frame - frame number
	void DrawRotate(int32 x, int32 y, Fixed angle, Fixed scale, int32 frame);
#endif

	//! @brief Draw 0 frame
	//! @param[in] x - X coordinate
	//! @param[in] y - Y coordinate
	void Draw(int32 x, int32 y);

	//! @brief Draw 0 frame
	//! @param[in] x - X coordinate
	//! @param[in] y - Y coordinate
	//! @param[in] frame - frame number
	void Draw(int32 x, int32 y, int32 frame);

	//! @brief Draw 0 frame
	//! @param[in] x - X coordinate
	//! @param[in] y - Y coordinate
	//! @param[in] frame - frame number
	//! @param[in] nManip - manipulation code
	void Draw(int32 x, int32 y, int32 frame, uint8 nManip);

	//! @brief Get sprite width
	uint16 GetWidth();
	//! @brief Get sprite height
	uint16 GetHeight();

	//! @brief Get frame width of the area being drawn
	//! @param[in] frame - frame number
	uint16 GetFrameWidth(int32 frame);
	//! @brief Get frame height of the area being drawn
	//! @param[in] frame - frame number
	uint16 GetFrameHeight(int32 frame);
	//! @brief Get the amount of frames in a sprite
	//! @param[in] frame - frame number
	uint16 GetFrameCount();

	//! @brief Set the current palette (if there are several of them and the sprite format is palette)
	void SetCurrentPalette(uint8 paletteNum);

	//! @brief Set the color in the current palette
	//! @param[in] colorNum - color index in the palette
	//! @param[in] R, G, B components
	void SetColor(uint8 colorNum, uint8 r, uint8 g, uint8 b);
};

#endif //__FRAMEWORK_SPRITE_H__
