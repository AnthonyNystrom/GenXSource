#ifndef __GUI_SYSTEM_H__
#define __GUI_SYSTEM_H__

#include "GUITypes.h"
#include "BaseTypes.h"
#include "VList.h"
#include "Application.h"
#include "GUIEventDispatcher.h"
#include "Graphics.h"

#include "GUIIndicator.h"


class GUISkinLocal;
class GUIControl;
class ResourceSystem;
class Sprite;

//#define DEBUG_DRAW
//#define DEBUG_INPUT
#define DRAW_DEBUG_COUNTER	10


enum eTransitionType
{
	ETT_NONE = 0,
	ETT_FROM_UP,
	ETT_FROM_DOWN,
	ETT_FROM_LEFT,
	ETT_FROM_RIGHT,
	ETT_ALPHA
};

class GUISystem : public GUIEventDispatcher
{
	enum eFocusMoveDir
	{
		EFD_LEFT,
		EFD_RIGHT,
		EFD_UP,
		EFD_DOWN
	};

	struct InvalidRect
	{
		Rect rect;
		bool isOnActive;
	};

public:
	static const int32 maxTempRects = 40;

	GUISystem(void);
	virtual ~GUISystem(void);

	void AddControl(GUIControl *newControl);
	void RemoveControl(GUIControl *control);
	void MoveControlFront(GUIControl *control);
	void SetActiveControl(GUIControl *control);
	GUIControl *GetActiveControl(){return activeWindow;};

	void ChangeControl(GUIControl *from, GUIControl *to, eTransitionType transition = ETT_NONE);

	GUIControl * SetFocus(GUIControl *control, bool recalcCursor = true, bool forceSet = false);
	GUIControl * GetFocus()
	{
		return  focused;
	}

	GUIControl * GetByID(int32 id);

	GUIControl * GetRoot(GUIControl *child);

	void LockKeyboard();//запрещает автоматическое премещение фокуса с клавиатуры
	void UnlockKeyboard();//разрешает автоматическое перемещение фокуса с клавиатуры
	void ProcessKey(Application::eKeyCode code);//автоматически перемещает курсор в зависимости от полученного кода

	void RecalcCursor();
	void NeedToRecalcCursor();

	void OnChar(char16 ch);

	void SetStringSystem(ResourceSystem *stringResSys)
	{
		strResSys = stringResSys;
	}
	ResourceSystem *GetStringSystem()
	{
		return strResSys;
	}

	void SetResourceSystem(ResourceSystem *graphicResSys)
	{
		grResSys = graphicResSys;
	}
	ResourceSystem *GetResourceSystem()
	{
		return grResSys;
	}

	void SendEventToActive(eEventID eventID, EventData *pData);
	void SendEventToAll(eEventID eventID, EventData *pData);
	void SendEventToControl(GUIControl *control, eEventID eventID, EventData *pData);
	void SendEventToSystem(eEventID eventID, EventData *pData);
	void SendEventToAllActive(eEventID eventID, EventData *pData);


	void FreezeBackground(const Rect &rect);
	void UnfreezeBackground();


	void Update();
	void Draw();

	void InvalidateRect(const Rect & rect, GUIControl *owner);
	void InvalidateAll();
	void ForceInvalidate();

	void SetCycling(bool cycle) {isCycling = cycle;}

	GUISkinLocal *GetSkin();

	void SetBackground(Sprite *pSprite);
	Sprite * GetBackground(){return pBackSprite;};
	void SetBackground(GraphicsSystem::Surface *pSurface);

	static GUISystem* Instance();

	Application	*	GetApp()	const	{ return pApp; }


	GUIIndicator *GetIndicator(GUIIndicator::eIndicatorType type);
	void SetIndicator(GUIIndicator::eIndicatorType type, GUIIndicator *indicator);



private:
	void ProcessKeyboard();//автоматическая обработка кнопок, если включена
	GUIControl *FindNextControl(eFocusMoveDir dir, GUIControl *parent);
	void ProcessDir(eFocusMoveDir dir, eEventID borderEvent);


private:

	void DrawBackground();

	void DrawControl(GUIControl *control, VList *pRects);

	void UpdateFocus();

	GUISkinLocal *pSkin;

	VList invalidActiveRects;
	VList invalidInactiveRects;
	InvalidRect tempRects[maxTempRects];
	Rect screenRect;
	int32 tempRectCounter;
	bool isAllInvalid;

	Sprite *pBackSprite;
	GraphicsSystem::Surface *pBackSurface;
	bool isBackSurface;

	GUIControl *focused;
	GUIControl *pNewFocus;
	bool isNeedToRecalcCursor;
	Rect focusRect;

	int32 cursorX;
	int32 cursorY;
	int32 tempCursorX;
	int32 tempCursorY;


	int32 isKeyboardLocked;

	VList windows;
	GUIControl * activeWindow;
	VList::Iterator activeIt;

	Application *pApp;
	ResourceSystem *strResSys;
	ResourceSystem *grResSys;

	GraphicsSystem *graphicSystem;
	GraphicsSystem::Surface *pBuffer;

//=== Transition ===

	void TransitionUpdate();
	void TransitionDraw();
	bool isTransition;
	eTransitionType transitionType;
	GUIControl* controlFrom;
	GUIControl* controlTo;
	InvalidRect transInvalidRect;
	VList transitionList;
	VList tempList;

	GraphicsSystem::Surface *pSurfFrom;
	GraphicsSystem::Surface *pSurfTo;

	int32 trX;
	int32 trY;
	int32 trAlpha;
//===            ===

//=== Freeze ===

	bool isFreeze;
	Rect freezeRect;
	int32 freezeCounter;
	static const int32 FREEZE_ITERATIONS = 12;
	void ProcessFreeze();
//===            ===

	int32 deepInput;

	volatile eEventID waitForEvent;
	bool isCycling;


	GUIIndicator *pIndicators[GUIIndicator::EIT_COUNT];

#ifdef DEBUG_DRAW
	int32 debugDrawCounter;
#endif

};
#endif