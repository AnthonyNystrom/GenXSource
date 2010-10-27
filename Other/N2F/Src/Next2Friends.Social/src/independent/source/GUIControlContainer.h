#ifndef __GUI_CONTROL_CONTAINER__
#define __GUI_CONTROL_CONTAINER__

#include "guicontrol.h"
#include "VList.h"

enum eChildStretch
{
	ECS_NOT_STRETCH = 0,
	ECS_STRETCH,

	ECS_STRETCH_RECALC,

	ECS_STATES_COUNT
};

class GUIControlContainer :
	public GUIControl
{
	friend class GUISystem;
	friend class GUIControl;
public:
	GUIControlContainer(GUIControlContainer *parent = NULL, const ControlRect &cr = ControlRect());
	virtual ~GUIControlContainer(void);

	virtual void Update();

	virtual void SetRect(const Rect &rc);

	virtual void AddChild(GUIControl *child);
	virtual void InsertChild(GUIControl *child, GUIControl *next);
	virtual void RemoveChild(GUIControl *child);
	virtual void Clear();

	virtual const VList *GetConstChilds(void);

	virtual VList *GetChilds(void);

	virtual GUIControl* GetByID(int32 theID);


	void SetStretch(eChildStretch stretch);

protected:

	virtual void RecalcDrawRect();

	eChildStretch stretchState;	
	VList childs;


	void Restretch();

};

#endif