#ifndef _N2F_NEWS_MANAGER_H_
#define _N2F_NEWS_MANAGER_H_

#include "N2FMessageOwner.h"
#include "IAnswer.h"
#include "VList.h"

class ApplicationManager;

class N2FNewsManager : public N2FMessageOwner, public IAnswer
{
public:
	N2FNewsManager(ApplicationManager *appManager);
	virtual ~N2FNewsManager();

	void ReciveAll();

	virtual void OnSuccess(int32 packetId);
	virtual void OnFailed(int32 packetId, char16 *errorString);

	virtual void OwnerUpdate();

	bool IsWorking()
	{
		return isWorking;
	}

	void Init();

protected:	
	virtual void OnMessageChanged(N2FMessage *pMsg);
	virtual void OnMessageWillSwitchOwner(N2FMessage *pMsg);
	virtual void OnMessageAdded(N2FMessage *pMsg);

	void SortByDate();
	bool IsHigher(N2FMessage *oldMsg, N2FMessage *newMsg);

private:

	bool isWorking;
	VList newMessages;

};


#endif//_SCREEN_BASE_H_
