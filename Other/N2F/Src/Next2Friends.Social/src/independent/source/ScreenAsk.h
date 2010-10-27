#ifndef __SCREEN_ASK__
#define __SCREEN_ASK__

#include "basescreen.h"
#include "ItemsListDelegate.h"

class GUITabList;
class SurfacesPool;
class GUIItemsList;
class GUIMultiString;
class N2FMessage;

class ScreenAsk :
	public BaseScreen, public ItemsListDelegate
{
	enum eMenuItems
	{
			EMI_PAGE_1 = 0
		,	EMI_QUESTION_TEXT
		,	EMI_PAGE_2
		,	EMI_TYPE_SELECTOR
		,	EMI_DURATION_SELECTOR
		,	EMI_TYPE_LAYOUT
		,	EMI_TYPE_FIRST
		,	EMI_TYPE_YES_NO = EMI_TYPE_FIRST
		,	EMI_TYPE_A_OR_B
		,	EMI_TYPE_RATE
		,	EMI_DURATION_LAYOUT
		,	EMI_DURATION_FIRST
		,	EMI_DURATION_5MIN = EMI_DURATION_FIRST
		,	EMI_DURATION_10MIN
		,	EMI_DURATION_60MIN
		,	EMI_DURATION_24HRS
		,	EMI_PRIVATE_QUESTION
		,	EMI_PRIVATE_QUESTION_CHECKBOX
		,	EMI_EDIT_TEXT_A
		,	EMI_EDIT_TEXT_B
		,	EMI_PAGE_3


		,	EMI_COUNT
	};
	enum ePopUpItems
	{
			EPI_QUESTION_TEXT = 0
		,	EPI_QUESTION_OPTIONS
		,	EPI_ATTACH_PHOTOS
		,	EPI_ATTACH_FROM_CAMERA
		,	EPI_ATTACH_FROM_FILE
		,	EPI_TEMPLATES
		,	EPI_REMOVE_PHOTO
		,	EPI_SAVE_TO_DRAFTS
		,	EPI_SUBMIT
		,	EPI_VIEW
		,	EPI_BACK


		,	EPI_COUNT
	};

public:
	ScreenAsk(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenAsk(void);

	virtual bool OnEvent(eEventID eventID, EventData *pData);


	virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId);
	virtual void			PopUpOnItemSelected(int32 index, int32 id);

	void	OnItem(int32 index);

	virtual void	BackToPrevSrceen();

	//void	InsertTemplate(const char16 *text);

	void	SetWorkingMessage(N2FMessage *pMsg);



	virtual void			ItemsListOnItem(GUIItemsList *forList, void *owner);

	virtual GUIControl*		ItemsListCreateListItem(GUIItemsList *forList);

	virtual bool			ItemsListIsOwnerValid(GUIItemsList *forList, void *owner);
	virtual void			*ItemsListGetFirstOwner(GUIItemsList *forList);
	virtual void			*ItemsListGetNextOwner(GUIItemsList *forList);

	virtual void			ItemsListTuneItemByOwner(GUIItemsList *forList, GUIControl *item, void *owner);
	virtual void			ItemsListCorrectItemByOwner(GUIItemsList *forList, GUIControl *item, void *owner);

private:

	void ShowAB();
	void HideAB();

	void PackMessage();

	GUITabList *tabList;

	GUILayoutBox *aLayout;
	GUILayoutBox *bLayout;

	GUIItemsList *photoList;

	N2FMessage *workMsg;
	N2FMessage *saveMsg;

	SurfacesPool *surfPool;

	GUIMultiString *submitReq;

	int32 photoCounter;

	bool isAB;
};
#endif
