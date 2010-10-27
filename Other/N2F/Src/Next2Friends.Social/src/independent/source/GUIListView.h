#ifndef __GUI_LISTVIEW_H_
#define __GUI_LISTVIEW_H_

#include "GUIControlContainer.h"
#include "GUIScrollBar.h"
#include "GUILayoutBox.h"

class GUIListView :	public GUIControlContainer
{


protected:
	
	GUIControlContainer	*	layoutContainer;
	GUILayoutBox	*	layout;

	GUIScrollBar	*	scrollBar;

public:
	GUIListView(GUIControlContainer * _parent = NULL, const ControlRect &cr = ControlRect());
	virtual ~GUIListView();

	virtual void AddChild(GUIControl *child);
	virtual void InsertChild(GUIControl *child, GUIControl *next);
	virtual void RemoveChild(GUIControl *child);
	virtual void RemoveAllItems();

	void ForceRecalc();

	virtual VList *GetListChilds(void);


	virtual void SetRect(const Rect &rc);

	virtual void SetInnerMargins(int32 left, int32 right, int32 top, int32 bottom);
	virtual void SetPrescroll(int newValue);
	virtual void SetScrollSpeedModifer(int newValue);
	virtual void Reset();

	virtual void	SetChildsMargins(int32 left, int32 right, int32 top, int32 bottom);


	virtual void Scroll(int32 delta);
	virtual void ScrollToFocus();
	virtual void MoveToBottom();


	virtual void Update();

	enum eStartPosition
	{
		ESP_TOP = 0,
		ESP_BOTTOM
	};
	void SetStartPosition(eStartPosition startPos);
	void SetListAlign(eStartPosition align);


private:
	void UpdateScrollSize();
	void UpdateScrollPos();
	void CheckFocused();

	void RecalcViewRect();

	int32 targetY;

	int32 innerMarginLeft;
	int32 innerMarginRight;
	int32 innerMarginTop;
	int32 innerMarginBottom;
	int32 prescroll;
	int32 speedModifer;

	eStartPosition itemsAlign;
	bool isScrolled;

};

#endif // __GUI_LISTVIEW_H_
