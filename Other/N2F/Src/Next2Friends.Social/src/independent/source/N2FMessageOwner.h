#ifndef _N2F_MESSAGE_OWNER_H_
#define _N2F_MESSAGE_OWNER_H_

#include "BaseTypes.h"
#include "N2FData.h"
#include "GUIEventDispatcher.h"


enum eOwnerType
{
		EOT_APP_MANAGER = 0
	,	EOT_DRAFT_MANAGER
	,	EOT_OUTBOX_MANAGER

};

class ApplicationManager;


class N2FMessageOwner : public GUIEventDispatcher
{
	friend class N2FMessage;
public:
	N2FMessageOwner(eOwnerType ownerType, ApplicationManager *manager);
	virtual ~N2FMessageOwner();



	virtual void OwnerUpdate();

	eOwnerType GetType()
	{
		return type;
	};

	int32 MessagesCount();

	N2FMessage *GetFirstMessage();
	VList *GetMessagesList()
	{
		return messages;
	};

	bool Write(const char8 *fileName, const char16 *alertMsg);
	bool Read(const char8 *fileName, const char16 *alertMsg);

	void MergeLists(VList *newMessagesList);


private:
	void MessgeChanged(N2FMessage *pMsg);
	void MessgeWillSwitchOwner(N2FMessage *pMsg);
	void AddMessage(N2FMessage *pMsg);


protected:
	virtual void OnMessageChanged(N2FMessage *pMsg) = 0;
	virtual void OnMessageWillSwitchOwner(N2FMessage *pMsg) = 0;
	virtual void OnMessageAdded(N2FMessage *pMsg) = 0;


	VList				*messages;
	eOwnerType			type;
	bool				isMessagesChanged;
	bool				isMessageCountChanged;
	ApplicationManager  *pManager;

};


#endif//_SCREEN_BASE_H_
