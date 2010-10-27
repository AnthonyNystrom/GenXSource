//// =================================================================
/*!	\file GUISkinTiled.h

	Revision History:

	\par [9.8.2007]	13:44 by Alexey Prosin
	File created.
*/// ==================================================================

#ifndef __FRAMEWORK_GUISKINTILED_H__
#define __FRAMEWORK_GUISKINTILED_H__

//#define SKIN_DEBUG_DRAW


#include "GUISkin.h"
#include "Sprite.h"

class GraphicsSystem;

//  ***************************************************
//! \brief Skin for drawing all base controls
//  ***************************************************
class GUISkinTiled : public GUISkin
{
public:

	enum eDrawScheme
	{
		ES_NONE,	// нет спрайта
		ES_1,		// один спрайт
		ES_H3,		// три спрайта горизонтально
		ES_V3,		// три вертикально
		ES_9		// "девятка"
	};


	enum eSkinScheme1
	{
		ESS1_1	=	0,

		ESS1__NUMBER // Количество
	};

	// Индексы спрайтов горизонтальной "тройки".
	enum eSkinSchemeH3
	{
		ESSH3_LEFT	=	0,
		ESSH3_CENTER,
		ESSH3_RIGHT,

		ESSH3__NUMBER // Количество
	};

	// Индексы спрайтов вертикальной "тройки".
	enum eSkinSchemeV3
	{
		ESSV3_TOP	=	0,
		ESSV3_CENTER,
		ESSV3_BOTTOM,

		ESSV3__NUMBER // Количество
	};

	// Индексы спрайтов "девятки".
	enum eSkinScheme9
	{
		//	Left,						Center,							Right
		ESS9_TOP_LEFT	=	0,			ESS9_TOP_CENTER	=	1,			ESS9_TOP_RIGHT	=	2,	// Top
		ESS9_CENTER_LEFT	=	3,		ESS9_CENTER_CENTER	=	4,		ESS9_CENTER_RIGHT	=	5,	// Center
		ESS9_BOTTOM_LEFT	=	6,		ESS9_BOTTOM_CENTER	=	7,		ESS9_BOTTOM_RIGHT	=	8,	// Bottom

		ESS9__NUMBER // Количество
	};

	struct StateFontStyle
	{
		uint8 state;		//состояние для отрисовки
		Font *pFont;		//шрифт для отрисовки состояния
		uint8 fontPalette;	//номер палитры у шрифта
		uint8 drawSelectMethod;
	};

	struct TypeFontStyle
	{
		List<StateFontStyle> *stateFontStyle;	//список возможных состояний для отрисовки
		eDrawType drawType;						//тип отрисовки для контрола
	};

	struct StateTilesStyle 
	{
		uint8 state;
		uint8 scheme;
		Sprite **spriteArray;
		uint8 frame;
		uint8 drawSelectMethod;
	};

	struct TypeTilesStyle
	{
		List<StateTilesStyle> *stateControlStyle;
		eDrawType drawType;
	};

	//! State selection type for DrawType
	enum eDrawSelectMethod
	{
		EDSM_EXCLUSIVE, //!< current control state flags should be exclusively contained in the state. For example
		//!< if state sets to the ECS_FOCUSED, current draw type would be used only 
		//!< for drawing controls with one ECS_FOCUSED state flag enabled and do not 
		//!< be used for something like ECS_FOCUSED | ECS_ENABLED.
		EDSM_INCLUSIVE	//!< state flags should be fully included into the current control state
		//!< but in the control may be enabled other flags too. For example if ECS_FOCUSED will be set
		//!< as a state for the current tiling draw, all controls with 
		//!< states like ECS_FOCUSED | ECS_ENABLED | ECS_VISIBLE 
		//!< , ECS_FOCUSED | ECS_ENABLED | ECS_VISIBLE | ECS_CHECKED would be shown by this draw type.
	};

	GUISkinTiled();

	virtual ~GUISkinTiled();

	virtual void DrawString(GUIControl *pControl, const Rect & _clipRect, const char16* pStr, int32 x, int32 y, int32 lenght);
	virtual void DrawTextCursor(const GUIControl * pControl, const Rect & _clipRect, int32 x, int32 y);

	virtual int32 GetFontHeight(const GUIControl *pControl);

	virtual int32 GetStringWidth(const GUIControl *pControl, const char16 *pStr, int32 length = -1);

	// ***************************************************
	//! \brief    	Draw control on screen
	//! \param      _control - control to draw
	//! \param		_clipRect - rect for clip.
	// ***************************************************
	virtual void DrawControl(const GUIControl * _control, const Rect & _clipRect);

	// ***************************************************
	//! \brief    	Draw focus
	//! \param      _control - focused control.
	//! \param		_clipRect - current clip rect.
	// ***************************************************
	virtual void DrawFocus(const GUIControl * _control, const Rect & _clipRect);

	// ***************************************************
	// \brief    	Set font for defined state of defined drawType
	// 
	// \param      drawType - draw type from \see eDrawType
	// \param      state - стэйт для которого выставляется данный шрифт \see eControlState (0 - default 
	//				если устанвоить шрифт для state = 0, то этот шрифт будет использоватся для всех стейтов 
	//				для которых шрифт не установлен явным образом)
	// \param      pFont - указатель на шрифт
	// \param      paletteNumber - номер палитры шрифта
	// \param      selectMethod - метод выборки стейта см \see eDrawSelectMethod
	// \return   	void 
	// ***************************************************
	void SetFont(eDrawType drawType, uint32 state, Font *pFont, uint32 paletteNumber, eDrawSelectMethod selectMethod);

	// ***************************************************
	// \brief    	SetTiles - установта типа затайливания для конкретного state конкретного drawType
	// 
	// \param      drawType - тип отрисовки см \see eDrawType
	// \param      state - стэйт для которого выставляется данный шрифт \see eControlState (0 - default 
	//				если устанвоить шрифт для state = 0, то этот шрифт будет использоватся для всех стейтов 
	//				для которых шрифт не установлен явным образом)
	// \param      spriteArray - указатель на массив спрайтов (кол-во спрайтов зависит от \see drawScheme)
	// \param      frame - номер фрейма который нужно использовать в спрайтах
	// \param      drawScheme - схема отрисовки см \see eDrawScheme
	// \param      selectMethod - метод выборки стейта см \see eDrawSelectMethod
	// \return   	void 
	// ***************************************************
	void SetTiles(eDrawType drawType, uint32 state, Sprite **spriteArray, uint32 frame, eDrawScheme drawScheme, eDrawSelectMethod selectMethod);

protected:

	Font * GetFont(const GUIControl * _control);

	virtual void DrawCursor(const Point &topPoint, Font *font, bool isTextEntered);

	StateTilesStyle *GetTiles(const GUIControl * _control);
	StateTilesStyle *GetTilesByDrawType(const GUIControl *_control, TypeTilesStyle &style);

	void DrawTiled (StateTilesStyle *controlStyle, const Rect &rect, const Rect &clipRect);


private:

	GraphicsSystem * graphicsSystem;

	uint32 cursorCounter;

	List<TypeFontStyle> fontStyleList;
	List<TypeTilesStyle> controlStyleList;

	uint32 fontPalette;


#ifdef SKIN_DEBUG_DRAW
	struct SkinRect 
	{
		int8 life;
		int x;
		int y;
		int frame;
		Sprite *pSprite;
	};

	List<SkinRect> debugRects;
	List<Rect> invalidRects;

public:
	void AddDebugRect(int x, int y, int frame, Sprite* pSpr);
	void DrawDebugRects();

#endif

};

static const int32 drawSchemeCount[] = {0, GUISkinTiled::ESS1__NUMBER, GUISkinTiled::ESSH3__NUMBER, GUISkinTiled::ESSV3__NUMBER, GUISkinTiled::ESS9__NUMBER};

#endif // __FRAMEWORK_GUISKINTILED_H__