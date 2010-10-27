#ifndef __GUI_SELECTOR_BUTTON__
#define __GUI_SELECTOR_BUTTON__

#include "GUILayoutBox.h"

class GUIImage;
class GUIControlContainer;

class GUISelector :
	public GUILayoutBox
{
public:
	GUISelector(GUIControlContainer *parent = NULL, const ControlRect &rc = ControlRect());
	virtual ~GUISelector(void);

	virtual void Update();

	virtual void SetDrawType(eDrawType newType);
	virtual void SetSelectedDrawType(eDrawType newType);
	//virtual void Draw();

	virtual void SetLeftArrow(int16 selectedID, int16 unselectedID, int16 noneID);
	virtual void SetRightArrow(int16 selectedID, int16 unselectedID, int16 noneID);
	virtual void SetChildsDrawType(eDrawType selected, eDrawType unselected);

	virtual void OnSetFocus();
	virtual void OnLostFocus();

	virtual void SetRect(const Rect &rc);

	virtual void AddChild(GUIControl *child);
	virtual void RemoveChild(GUIControl *child);
	virtual void InsertChild(GUIControl *child, GUIControl *next);

	GUIControl*	GetSelected();
	int32		GetSelectedIndex();
	void		SetSelected(GUIControl* pSel);
	void		SetSelectedIndex(int32 selected);

	void		SetCycling(bool cycling) {isCycling = cycling;}

	virtual VList *GetSelectorChilds();

private:
	void RecalcArrows();

	void SetImage(int16 imageID, GUIImage *img);


	GUILayoutBox *pLayout;
	GUIControlContainer *pSelectorContainer;
	GUIControl *pSelected;
	bool isLeft;
	bool isRight;


	GUIImage *leftArrow;
	GUIImage *rightArrow;

	int16 leftSelectedID;
	int16 rightSelectedID;
	int16 leftUnselectedID;
	int16 rightUnselectedID;
	int16 leftNoneID;
	int16 rightNoneID;

	eDrawType selectedDrawType;
	eDrawType unselectedDrawType;

	eDrawType selectedChildType;
	eDrawType unselectedChildType;


	int32 realX;

	bool isCycling;

};
#endif