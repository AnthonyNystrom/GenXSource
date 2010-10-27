//// =================================================================
/*!	\file GUISkinTiled.h

	Revision History:

	\par [9.8.2007]	13:44 by Alexey Prosin
	File created.
*/// ==================================================================

#ifndef __GUISKINLOCAL_H__
#define __GUISKINLOCAL_H__

//#define SKIN_DEBUG_DRAW


#include "Sprite.h"
#include "VList.h"
#include "GUITypes.h"


class GraphicsSystem;


//  ***************************************************
//! \brief Skin for drawing all base controls
//  ***************************************************
class GUISkinLocal
{
	friend class GUISystem;
public:


	enum eDrawScheme
	{
		ES_NONE = 0,		// нет спрайта
		ES_1,				// один спрайт
		ES_H3,				// три спрайта горизонтально
		ES_V3,				// три вертикально
		ES_9,				// "девятка"
		ES_9_FILL,			// "c заливкой в центре"
		ES_9_GRADIENT,		// "c градиентом в центре"
		ES_9_TOP,			// "девятка"
		ES_9_BOTTOM,		// "девятка"
		ES_9_LEFT,			// "девятка"
		ES_9_RIGHT,			// "девятка"
		ES_9_BORDER,		// "девятка"
		ES_FILL,			// "заливка"
		ES_LINE_BOTTOM		// "линия снизу"
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

	struct Gradient 
	{
		Gradient(uint8 stR, uint8 stG, uint8 stB, uint8 enR, uint8 enG, uint8 enB, uint8 alph)
			: startR(stR), startG(stG), startB(stB), endR(enR), endG(enG), endB(enB), alpha(alph){};
		uint8 startR;
		uint8 startG;
		uint8 startB;
		uint8 endR;
		uint8 endG;
		uint8 endB;
		uint8 alpha;
	};

	struct Line 
	{
		Line(uint8 R, uint8 G, uint8 B, uint8 offset, uint8 alph)
			: r(R), g(G), b(B), bottomOffset(offset), alpha(alph){};
		uint8 r;
		uint8 g;
		uint8 b;
		uint8 bottomOffset;
		uint8 alpha;
	};



	struct TypeFontStyle
	{
		eDrawType drawType;
		Font *pFont;		//шрифт для отрисовки состояния
		uint8 palette;
	};

	struct TypeTilesStyle
	{
		eDrawType drawType;
		uint8 scheme;
		Sprite **spriteArray;
		uint8 frame;
		uint8 palette;
	};


	GUISkinLocal();

	virtual ~GUISkinLocal();



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
	void SetTiles(eDrawType drawType, Sprite **spriteArray, uint32 frame, GUISkinLocal::eDrawScheme drawScheme, uint8 palNumber = 0);

	void SetFont(eDrawType drawType, Font *font, uint8 palNumber = 0);

	Font *GetFont(eDrawType drawType, uint8 &palette);


	// ***************************************************
	//! \brief    	Draw control on screen
	//! \param      _control - control to draw
	//! \param		_clipRect - rect for clip.
	// ***************************************************
	void DrawControl(eDrawType drawType, const Rect & rect);

	TypeTilesStyle *GetTiles(eDrawType drawType);

	// ***************************************************
	//! \brief    	Returns sprite for selected draw type
	//! \param      drawType - drawType
	//! \param		schemeID - sprite position in draw scheme
	//! \return		required sprite or NULL if sprite or draw type don't exist
	// ***************************************************
	Sprite	*GetSprite(eDrawType drawType, int32 schemeID);


protected:

	//Font * GetFont(const GUIControl * _control);

	//virtual void DrawCursor(const Point &topPoint, Font *font, bool isTextEntered);

	void DrawTiled (TypeTilesStyle *controlStyle, const Rect &rect);

private:

	GraphicsSystem * graphicsSystem;

	uint32 cursorCounter;

	VList controlStyleList;
	VList fontList;


};

static const int32 drawSchemeCount[14] = 
{
	0
	, GUISkinLocal::ESS1__NUMBER

	, GUISkinLocal::ESSH3__NUMBER

	, GUISkinLocal::ESSV3__NUMBER

	, GUISkinLocal::ESS9__NUMBER
	, GUISkinLocal::ESS9__NUMBER
	, GUISkinLocal::ESS9__NUMBER
	, GUISkinLocal::ESS9__NUMBER
	, GUISkinLocal::ESS9__NUMBER
	, GUISkinLocal::ESS9__NUMBER
	, GUISkinLocal::ESS9__NUMBER
	, GUISkinLocal::ESS9__NUMBER

	, GUISkinLocal::ESS9__NUMBER//FILL

	, GUISkinLocal::ESS9__NUMBER//LINE BOTTOM
};

#endif // __FRAMEWORK_GUISKINTILED_H__