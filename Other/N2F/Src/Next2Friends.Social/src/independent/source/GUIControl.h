#ifndef __GUI_CONTROL__
#define __GUI_CONTROL__

#include "GUITypes.h"
#include "BaseTypes.h"
#include "GUIEventDispatcher.h"

class GUIControl;
class VList;
class GUIControlContainer;

//! Control rectangle structure
struct ControlRect
{
	enum Const
	{
		MIN_D	=	5,
		MAX_D	=	10000
	};

	//! @brief Constructor
	ControlRect(int32 _x, int32 _y, int32 _minDx, int32 _minDy, int32 _maxDx, int32 _maxDy)
		: x(_x), y(_y), minDx(_minDx), minDy(_minDy), maxDx(_maxDx), maxDy(_maxDy)	{};
	ControlRect(int32 _x, int32 _y, int32 _dx, int32 _dy)
		: x(_x), y(_y), minDx(_dx), minDy(_dy), maxDx(_dx), maxDy(_dy)				{};
	ControlRect()
		: x(0), y(0), minDx(MIN_D), minDy(MIN_D), maxDx(MAX_D), maxDy(MAX_D)		{};

	int32 x, y;//!< Left/top point
	int32 minDx, minDy;//!< Min width/height
	int32 maxDx, maxDy;//!< Max width/height
};

class GUIControl : public GUIEventDispatcher
{
	friend class GUISystem;
	friend class GUILayoutBox;
	friend class GUIControlContainer;

public:

	enum eAlign
	{
        EA_LEFT		=	1,
		EA_RIGHT	=	2,
		EA_HCENTRE	=	4,
		EA_TOP		=	8,
		EA_BOTTOM	=	16,
		EA_VCENTRE	=	32
	};

public:

	GUIControl(GUIControlContainer *parent = NULL, const ControlRect &cr = ControlRect(), bool selectable = false);

	virtual ~GUIControl(void);

	virtual void Update();
	virtual void Draw();
	virtual void DrawFinished();

	virtual void SetParent(GUIControlContainer *parent, GUIControl *nextBy = NULL);
	virtual GUIControl * GetParent();

	virtual void AddChild(GUIControl *child);
	virtual void RemoveChild(GUIControl *child);

	virtual void SetRect(const Rect &rc);
	virtual Rect GetRect();

	virtual void SetControlRect(const ControlRect &cr);
	virtual ControlRect GetControlRect();


	void SetID(int32 newID);
	int32 GetID();
	virtual GUIControl* GetByID(int32 theID);

	virtual Rect GetOutputRect();
	virtual void GetScreenRect(Rect &rc);

	virtual void SetDrawType(eDrawType newType);
	virtual eDrawType GetDrawType();

	virtual void SetSelectable(bool selectable);
	virtual bool IsSelectable();

	virtual void Invalidate();

	virtual void OnSetFocus();
	virtual void OnLostFocus();

	virtual const VList *GetConstChilds(void);

	void	SetAlignType(int32 a);
	int32	GetAlignType();
	bool	IsAlignType(int32 a);

	virtual void	SetMargins(int32 left, int32 right, int32 top, int32 bottom);

	int32	GetMarginLeft();
	int32	GetMarginRight();
	int32	GetMarginTop();
	int32	GetMarginBottom();

protected:

	virtual VList *GetChilds(void);

	virtual void RecalcDrawRect();


private:

	virtual void CalcDrawRect(Rect &rc);

private:

	ControlRect controlRect;
	Rect rect;
	int32 id;
	GUIControlContainer *pParent;
	eDrawType drawType;
	bool isSelectable;

	int32 leftMargin;
	int32 rightMargin;
	int32 topMargin;
	int32 bottomMargin;


	int32 align;

	Rect outputRect;
};

#endif
