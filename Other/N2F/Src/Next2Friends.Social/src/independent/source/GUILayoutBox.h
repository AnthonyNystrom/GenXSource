#ifndef __GUI_LAYOUT_BOX__
#define __GUI_LAYOUT_BOX__

#include "guicontrolcontainer.h"

enum eGrowDirection
{
	EGD_INCREASE = 0,
	EGD_DECREASE,
	EGD_BOTH,
	EGD_NONE
};

class GUILayoutBox : public GUIControlContainer
{
public:

	GUILayoutBox(bool isVertical = true, GUIControlContainer *parent = NULL, const ControlRect &cr = ControlRect(), eGrowDirection growDir = EGD_INCREASE);
	virtual ~GUILayoutBox(void);

	virtual void SetMargins(int32 left, int32 right, int32 top, int32 bottom);

	virtual void Draw();
	virtual void Update();

	virtual void AddChild(GUIControl *child);
	virtual void InsertChild(GUIControl *child, GUIControl *next);
	virtual void RemoveChild(GUIControl *child);
	
	virtual void	SetChildsMargins(int32 left, int32 right, int32 top, int32 bottom);

	virtual void	SetChildsAlignType(int32 a)	{	childsAlign = a;				}
	virtual int32	GetChildsAlignType()		{	return childsAlign;				}
	virtual bool	IsChildsAlignType(int32 a)	{	return (childsAlign & a) == a;	}

	virtual void SetControlRect(const ControlRect &cr);

	virtual void SetGrowDirection(eGrowDirection growDir);

	void Recalc();

	void NeedRecalc()
	{
		recalc = true;
	}

	void ForceRecalc()
	{
		NeedRecalc();
		Recalc();
	}

	virtual void SetDirection(bool isVertical);

private:

	bool recalc;
	bool vertical;

	int32 childsAlign;

	int32 childsLeftMargin;
	int32 childsRightMargin;
	int32 childsTopMargin;
	int32 childsBottomMargin;

	eGrowDirection growDirection;
};

#endif // __GUI_LAYOUT_BOX__