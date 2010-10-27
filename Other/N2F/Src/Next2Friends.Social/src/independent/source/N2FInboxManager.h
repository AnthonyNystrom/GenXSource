#ifndef _N2F_INBOX_MANAGER_H_
#define _N2F_INBOX_MANAGER_H_

#include "N2FMessageOwner.h"
#include "IAnswer.h"

class ApplicationManager;

struct ArrayOfInt32;

class N2FInboxManager : public N2FMessageOwner, public IAnswer
{
public:
	N2FInboxManager(ApplicationManager *appManager);
	virtual ~N2FInboxManager();

	void ReciveAll();

	virtual void OnSuccess(int32 packetId);
	virtual void OnFailed(int32 packetId, char16 *errorString);

	virtual void OwnerUpdate();

	void Init();


protected:	
	virtual void OnMessageChanged(N2FMessage *pMsg);
	virtual void OnMessageWillSwitchOwner(N2FMessage *pMsg);
	virtual void OnMessageAdded(N2FMessage *pMsg);

private:

	void GetNextQuestion();

	bool isWorking;

	bool isReciving;

	ArrayOfInt32	*IDs;
	int32 msgCounter;
	VList newMessages;

};


#endif//_SCREEN_BASE_H_
