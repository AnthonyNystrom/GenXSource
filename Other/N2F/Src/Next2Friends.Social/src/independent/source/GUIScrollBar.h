#ifndef __GUI_SCROLLBAR_H__
#define __GUI_SCROLLBAR_H__

#include "GUIControl.h"

enum eScrollOrientation
{
	ESO_UPTODOWN			=	0,
	ESO_LEFTTORIGHT			=	1
};

class GUIScrollBar : public GUIControl
{
	enum eConst
	{
		MIN_BUTTON_SIZE		=	4,
		MARGIN_SIZE			=	1
	};
		
private:

	eScrollOrientation		orientation;

	int32					viewSize;
	int32					fullSize;
	int32					position;
	int32					barMinSize;
	int32					barMaxSize;


	//! rect for drawing
	int32 scrollMargin;

	Rect rcTop;
	Rect rcBottom;
	Rect rcMiddle;

	eDrawType topDrawType;
	eDrawType bottomDrawType;
	eDrawType middleDrawType;

public:

	GUIScrollBar(GUIControlContainer *parent = NULL, const ControlRect &cr = ControlRect(),
		eScrollOrientation _orientation = ESO_UPTODOWN);

	virtual ~GUIScrollBar();

	virtual void	Draw();

	virtual void	SetRect(const Rect &rc);
	virtual void	RecalcDrawRect();

	void			SetSize(uint32 _viewSize, uint32 fullSize);
	void			SetPos(uint32 _pos);

	void			SetElementDrawType(eDrawType top, eDrawType bottom, eDrawType middle);

	void			SetScrollMargin(int32 margin);

private:
//	void			CalcRects(const Rect &rc);
	void			Recalculate();
};

#endif // __GUI_SCROLLBAR_H__
