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
		ES_NONE = 0,		// ��� �������
		ES_1,				// ���� ������
		ES_H3,				// ��� ������� �������������
		ES_V3,				// ��� �����������
		ES_9,				// "�������"
		ES_9_FILL,			// "c �������� � ������"
		ES_9_GRADIENT,		// "c ���������� � ������"
		ES_9_TOP,			// "�������"
		ES_9_BOTTOM,		// "�������"
		ES_9_LEFT,			// "�������"
		ES_9_RIGHT,			// "�������"
		ES_9_BORDER,		// "�������"
		ES_FILL,			// "�������"
		ES_LINE_BOTTOM		// "����� �����"
	};


	enum eSkinScheme1
	{
		ESS1_1	=	0,

		ESS1__NUMBER // ����������
	};

	// ������� �������� �������������� "������".
	enum eSkinSchemeH3
	{
		ESSH3_LEFT	=	0,
		ESSH3_CENTER,
		ESSH3_RIGHT,

		ESSH3__NUMBER // ����������
	};

	// ������� �������� ������������ "������".
	enum eSkinSchemeV3
	{
		ESSV3_TOP	=	0,
		ESSV3_CENTER,
		ESSV3_BOTTOM,

		ESSV3__NUMBER // ����������
	};

	// ������� �������� "�������".
	enum eSkinScheme9
	{
		//	Left,						Center,							Right
		ESS9_TOP_LEFT	=	0,			ESS9_TOP_CENTER	=	1,			ESS9_TOP_RIGHT	=	2,	// Top
		ESS9_CENTER_LEFT	=	3,		ESS9_CENTER_CENTER	=	4,		ESS9_CENTER_RIGHT	=	5,	// Center
		ESS9_BOTTOM_LEFT	=	6,		ESS9_BOTTOM_CENTER	=	7,		ESS9_BOTTOM_RIGHT	=	8,	// Bottom

		ESS9__NUMBER // ����������
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
		Font *pFont;		//����� ��� ��������� ���������
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
	// \brief    	SetTiles - ��������� ���� ������������ ��� ����������� state ����������� drawType
	// 
	// \param      drawType - ��� ��������� �� \see eDrawType
	// \param      state - ����� ��� �������� ������������ ������ ����� \see eControlState (0 - default 
	//				���� ���������� ����� ��� state = 0, �� ���� ����� ����� ������������� ��� ���� ������� 
	//				��� ������� ����� �� ���������� ����� �������)
	// \param      spriteArray - ��������� �� ������ �������� (���-�� �������� ������� �� \see drawScheme)
	// \param      frame - ����� ������ ������� ����� ������������ � ��������
	// \param      drawScheme - ����� ��������� �� \see eDrawScheme
	// \param      selectMethod - ����� ������� ������ �� \see eDrawSelectMethod
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