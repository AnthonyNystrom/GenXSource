/*!
@file	Font.h
@brief	Font class
*/

#ifndef __FRAMEWORK_FONT_H__
#define __FRAMEWORK_FONT_H__

#include "Sprite.h"
#include "Binary.h"

//! Class work with graphic fonts
class Font : public Resource
{
	friend class ResourceSystem;

private:
	
	Font(Sprite * sprite, Binary * arrayList, ResourceSystem * resourceSystem,
		int16 resourceID, char8 * resourceName);

	virtual ~Font();

	class IndexList
	{
	public:

		IndexList()
		:	symbol	(0)
		,	frame	(0)
		,	next	(0)
		{
		}

		uint16		symbol;
		uint16		frame;
		IndexList	*next;
	};

public:

	//! @brief Text alignment
	enum eAnchPoint
	{
		EAP_LEFT		= 1,	//!< Left alignment
		EAP_RIGHT		= 2,	//!< Right alignment
		EAP_HCENTER		= 4,	//!< Horizontal center alignment
		EAP_TOP			= 8,	//!< Top alignment
		EAP_BOTTOM		= 16,	//!< Bottom alignment
		EAP_POINTS		= 32,	//!< Need "..." if text does not fit
		EAP_VCENTER		= 64	//!< Vertical center alignment
	};

	//! @brief Draws string
	//! @param[in] pString		- pointer on UNICODE string.
	//! @param[in] x			- coordinate X.
	//! @param[in] y			- coordinate Y.
	//! @param[in] charsCount	- number of characters to be drawn, or -1 for whole string drawing.
	//! @param[in] anchor		- text alignment.
	//! @param[in] monoSpaced	- type of the mono spaced mode: 
	//! -1 - width of all characters is equal to the sprite frame width,
	//! 0 - mono spaced mode is switched off,
	//! >0 - width of all characters is equal to the monoSpaced value
	void DrawString(const char16 * pString, int32 x, int32 y, int32 charCount = -1,
		int32 anchor = EAP_LEFT, int16 monoSpaced = 0);

	//! @brief Gets pixel width of the specified string.
	//! @param[in] pString		- pointer on UNICODE string.
	//! @param[in] charsCount	- number of characters to be calculated, or -1 for whole string calculating.
	//! @return Pixel width of the specified string.
	int32 GetStringWidth(const char16 * pString, int32 charCount = -1);

	//! @brief Gets font height.
	//! @return Font height.
	uint16 GetHeight() { return fontHeight; }

	//! @brief Gets font sprite frame width.
	//! @return Font sprite frame width.
	uint16 GetWidth() { return sprite->GetWidth(); };

	//! @brief Change current font palette color.
	//! @param[in] colorNum		- Palette color index.
	//! @param[in] r, g, b		- New palette color.
	void SetColor(uint8 colorNum, uint8 r, uint8 g, uint8 b);

	//! @brief Set current palette.
	void SetCurrentPalette(uint8 paletteNum);

	// @brief Sets spacing between characters (does not work for mono spaced mode).
	// @param[in] spacing	- Spacing between characters.
	void SetSpacing(int8 spacing);

	// @brief Gets frame number for specified character.
	// @param[in] symbol	- character.
	uint16 GetNumFrame(char16 symbol);

private:

	IndexList * indexListPool;
	uint16 indexListSize;

	typedef IndexList* IndexListPtr;
	IndexList ** hashList;

	Sprite * sprite;
	Binary * arrayList;

	uint8 fontHeight;
	uint8 * fontWidths;
	uint8 spaceSize;
	uint8 pointsSize;

	uint16 hashCoeff;
	uint16 hashListSize;

	int8 spacing;

	Font::IndexList * GetFreeIndexList();
	uint16 FindHashCoeff(uint16 size, uint16 hashListSize, uint16 hashCoeff);
	void ReleaseInter();
};

#endif //__FRAMEWORK_FONT_H__