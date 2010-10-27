#include "N2FDraftsManager.h"
#include "ApplicationManager.h"
#include "stringres.h"
#include "StringWrapper.h"

N2FDraftsManager::N2FDraftsManager( ApplicationManager *appManager )
: N2FMessageOwner(EOT_DRAFT_MANAGER, appManager)
{
}

N2FDraftsManager::~N2FDraftsManager()
{

}

void N2FDraftsManager::OnMessageChanged( N2FMessage *pMsg )
{

}

void N2FDraftsManager::OnMessageWillSwitchOwner( N2FMessage *pMsg )
{

}

void N2FDraftsManager::OnMessageAdded( N2FMessage *pMsg )
{

}

void N2FDraftsManager::OwnerUpdate()
{
	if (isMessageCountChanged || isMessagesChanged)
	{
		Write("drafts.dat", pManager->GetStringWrapper()->GetStringText(IDS_DRAFTS_INVALID_SAVE));
	}
	N2FMessageOwner::OwnerUpdate();

}

void N2FDraftsManager::Init()
{
	Read("drafts.dat", pManager->GetStringWrapper()->GetStringText(IDS_DRAFTS_INVALID_LOAD));
}

