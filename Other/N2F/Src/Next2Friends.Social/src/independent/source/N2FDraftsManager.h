#ifndef _N2F_DRAFTS_MANAGER_H_
#define _N2F_DRAFTS_MANAGER_H_

#include "N2FMessageOwner.h"

class ApplicationManager;

class N2FDraftsManager : public N2FMessageOwner
{
public:
	N2FDraftsManager(ApplicationManager *appManager);
	virtual ~N2FDraftsManager();

	void Init();
	virtual void OwnerUpdate();

protected:	
	virtual void OnMessageChanged(N2FMessage *pMsg);
	virtual void OnMessageWillSwitchOwner(N2FMessage *pMsg);
	virtual void OnMessageAdded(N2FMessage *pMsg);

private:

};


#endif//_SCREEN_BASE_H_
