#ifndef __SCREEN_RESPONSE__
#define __SCREEN_RESPONSE__

#include "basescreen.h"
#include "N2FData.h"
#include "ItemsListDelegate.h"
#include "VList.h"
#include "IAnswer.h"




class GUIMenuItem;
class GUIListView;
class SurfacesPool;
struct AskResponse;
struct ArrayOfInt32;
struct AskComment;

struct CommentItem 
{
	AskComment *comm;
	int32 currID;
	~CommentItem();
};

class ScreenResponse :
	public BaseScreen, public IAnswer
{
	enum eMenuItems
	{
			EMI_QUESTION_TEXT = 0
		,	EMI_PHOTO
		,	EMI_RESPONSE_1
		,	EMI_RESPONSE_2
		,	EMI_AVERAGE

		,	EMI_COUNT

	};
	//enum ePopUpItems
	//{
	//		EPI_DELETE
	//	,	EPI_BACK

	//	,	EPI_COUNT
	//};

public:
	ScreenResponse(const ControlRect & newRect, ApplicationManager * pAppMng, int32 screenID);
	virtual ~ScreenResponse(void);

	//virtual const char16	*PopUpGetTextByIndex(int32 index, int32 &id, int32 prevId);
	//virtual void			PopUpOnItemSelected(int32 index, int32 id);
	virtual bool			PopUpShouldOpen();

	virtual bool OnEvent(eEventID eventID, EventData *pData);

	virtual void OnSuccess(int32 packetId);
	virtual void OnFailed(int32 packetId, char16 *errorString);


private:

	void RebuildCommentsList();

	void ReciveNextComment();
	void ClearComments();

	GUIMenuItem *GetMenuItem();
	void ReturnMenuItem(GUIMenuItem *mi);
	void AddChildsFor(int32 commentID, int32 tab);


	GUIListView *list;
	SurfacesPool *surfPool;
	AskResponse *resp;
	ArrayOfInt32 *IDs;
	int32 currComment;
	
	N2FMessage *currentQuestion;
	N2FMessage *currentComment;

	GUIMenuItem *questionName;

	VList comments;
	VList commentGUIItems;
	int32 packetsSent;
	bool isCancel;


	//VList::Iterator newsIterator;

};
#endif
