#include "N2FNewsManager.h"
#include "ApplicationManager.h"
#include "DashboardService.h"
#include "ArrayOfDashboardNewFriend.h"
#include "ArrayOfDashboardWallComment.h"
#include "ArrayOfDashboardPhoto.h"
#include "ArrayOfDashboardVideo.h"
#include "N2FMessage.h"
#include "StringWrapper.h"
#include "stringres.h"


N2FNewsManager::N2FNewsManager( ApplicationManager *appManager )
: N2FMessageOwner(EOT_DRAFT_MANAGER, appManager)
{

}

N2FNewsManager::~N2FNewsManager()
{
	while (newMessages.Size())
	{
		((N2FMessage *)*newMessages.Begin())->SetOwner(pManager);
		newMessages.Erase(newMessages.Begin());
	}
}

void N2FNewsManager::OnMessageChanged( N2FMessage *pMsg )
{

}

void N2FNewsManager::OnMessageWillSwitchOwner( N2FMessage *pMsg )
{

}

void N2FNewsManager::OnMessageAdded( N2FMessage *pMsg )
{

}

void N2FNewsManager::ReciveAll()
{
	if (isWorking || !pManager->IsUserLoggedIn())
	{
		return;
	}
	isWorking = true;
	//while (GetFirstMessage())
	//{
	//	GetFirstMessage()->SetOwner(pManager);
	//}
	UTILS_LOG(EDMP_DEBUG, "N2FNewsManager::ReciveAll");
	pManager->ShowDashboard(true);
	pManager->GetDashboardService()->GetNewFriends(this
		, pManager->GetCore()->GetApplicationData()->login
		, pManager->GetCore()->GetApplicationData()->password);

}

void N2FNewsManager::OnSuccess( int32 packetId )
{
	switch(packetId)
	{
	case DashboardService::EPS_GETNEWFRIENDS:
		{
			ArrayOfDashboardNewFriend *friends = pManager->GetDashboardService()->OnGetNewFriends();
			UTILS_LOG(EDMP_DEBUG, "N2FNewsManager::OnSuccess EPS_GETNEWFRIENDS");
			
			DashboardNewFriend *p = friends->pBuffer;
			for (int i = 0; i < friends->length; i++)
			{
				N2FMessage *msg = pManager->GetFreeMessage();
				msg->InitAsNews(EIT_DASHBOARD_FIREND);
				msg->SetText(p->Nickname1, ETT_DASHBOARD_NICKNAME1);
				msg->SetText(p->Nickname2, ETT_DASHBOARD_NICKNAME2);
				msg->SetDateTime(p->DateTime);
				msg->SetOwner(NULL);
				newMessages.PushBack(msg);
				p++;
			}


			SAFE_DELETE(friends);

			UTILS_LOG(EDMP_DEBUG, "N2FNewsManager::OnSuccess done");

			pManager->GetDashboardService()->GetWallComments(this
				, pManager->GetCore()->GetApplicationData()->login
				, pManager->GetCore()->GetApplicationData()->password);
		}
		break;
	case DashboardService::EPS_GETWALLCOMMENTS:
		{
			UTILS_LOG(EDMP_DEBUG, "N2FNewsManager::OnSuccess EPS_GETWALLCOMMENTS");
			ArrayOfDashboardWallComment *walls = pManager->GetDashboardService()->OnGetWallComments();
			
			DashboardWallComment *p = walls->pBuffer;
			for (int i = 0; i < walls->length; i++)
			{
				N2FMessage *msg = pManager->GetFreeMessage();
				msg->InitAsNews(EIT_DASHBOARD_WALL);
				msg->SetText(p->Nickname1, ETT_DASHBOARD_NICKNAME1);
				msg->SetText(p->Nickname2, ETT_DASHBOARD_NICKNAME2);
				msg->SetText(p->Text, ETT_DASHBOARD_TEXT);
				msg->SetDateTime(p->DateTime);
				msg->SetOwner(NULL);
				newMessages.PushBack(msg);
				p++;
			}

			SAFE_DELETE(walls);
			pManager->GetDashboardService()->GetPhotos(this
				, pManager->GetCore()->GetApplicationData()->login
				, pManager->GetCore()->GetApplicationData()->password);

			UTILS_LOG(EDMP_DEBUG, "N2FNewsManager::OnSuccess done");
		}
		break;
	case DashboardService::EPS_GETPHOTOS:
		{
			UTILS_LOG(EDMP_DEBUG, "N2FNewsManager::OnSuccess EPS_GETPHOTOS");
			ArrayOfDashboardPhoto *photos = pManager->GetDashboardService()->OnGetPhotos();

			DashboardPhoto *p = photos->pBuffer;
			for (int i = 0; i < photos->length; i++)
			{
				N2FMessage *msg = pManager->GetFreeMessage();
				msg->InitAsNews(EIT_DASHBOARD_PHOTO);
				msg->SetText(p->Nickname, ETT_DASHBOARD_NICKNAME1);
				msg->SetText(p->Title, ETT_DASHBOARD_TITLE);
				msg->SetText(p->Text, ETT_DASHBOARD_TEXT);
				msg->SetDateTime(p->DateTime);
				msg->SetOwner(NULL);
				newMessages.PushBack(msg);
				p++;
			}


			SAFE_DELETE(photos);
			pManager->GetDashboardService()->GetVideos(this
				, pManager->GetCore()->GetApplicationData()->login
				, pManager->GetCore()->GetApplicationData()->password);

			UTILS_LOG(EDMP_DEBUG, "N2FNewsManager::OnSuccess done");
		}
		break;
	case DashboardService::EPS_GETVIDEOS:
		{
			UTILS_LOG(EDMP_DEBUG, "N2FNewsManager::OnSuccess EPS_GETVIDEOS");
			ArrayOfDashboardVideo *video = pManager->GetDashboardService()->OnGetVideos();

			DashboardVideo *p = video->pBuffer;
			for (int i = 0; i < video->length; i++)
			{
				N2FMessage *msg = pManager->GetFreeMessage();
				msg->InitAsNews(EIT_DASHBOARD_VIDEO);
				msg->SetText(p->Nickname, ETT_DASHBOARD_NICKNAME1);
				msg->SetText(p->Title, ETT_DASHBOARD_TITLE);
				msg->SetText(p->Text, ETT_DASHBOARD_TEXT);
				msg->SetDateTime(p->DateTime);
				msg->SetOwner(NULL);
				newMessages.PushBack(msg);
				p++;
			}


			SAFE_DELETE(video);
			UTILS_LOG(EDMP_DEBUG, "N2FNewsManager::OnSuccess merging");

			//MergeLists(&newMessages);
			while (messages->Size())
			{
				((N2FMessage *)*messages->Begin())->SetOwner(pManager);
			}
			while (newMessages.Size())
			{
				((N2FMessage *)*newMessages.Begin())->SetOwner(this);
				newMessages.Erase(newMessages.Begin());
			}
			SortByDate();
			pManager->ShowDashboard(false);
			isWorking = false;
			UTILS_LOG(EDMP_DEBUG, "N2FNewsManager::OnSuccess done");
		}
		break;
	}
}

void N2FNewsManager::OnFailed( int32 packetId, char16 *errorString )
{
	UTILS_LOG(EDMP_DEBUG, "N2FNewsManager::OnFailed");
	pManager->ShowDashboard(false);
	isWorking = false;
	while (newMessages.Size())
	{
		((N2FMessage *)*newMessages.Begin())->SetOwner(pManager);
		newMessages.Erase(newMessages.Begin());
	}

}

void N2FNewsManager::Init()
{
	Read("dashboard.dat", pManager->GetStringWrapper()->GetStringText(IDS_DASHBOARD_INVALID_LOAD));
}

void N2FNewsManager::OwnerUpdate()
{
	if (isMessageCountChanged || isMessagesChanged)
	{
		Write("dashboard.dat", pManager->GetStringWrapper()->GetStringText(IDS_DASHBOARD_INVALID_SAVE));
	}
	N2FMessageOwner::OwnerUpdate();
}

void N2FNewsManager::SortByDate()
{
	VList tempList;
	while (messages->Size())
	{
		tempList.PushBack(*(messages->Begin()));
		messages->Erase(messages->Begin());
	}

	while (tempList.Size())
	{
		VList::Iterator maxIt;
		N2FMessage *curMax = NULL;
		for (VList::Iterator it = tempList.Begin(); it != tempList.End(); it++)
		{
			N2FMessage *tmp = (N2FMessage*)(*it);
			if (!curMax || IsHigher(curMax, tmp))
			{
				curMax = tmp;
				maxIt = it;
			}
		}
		messages->PushBack(curMax);
		tempList.Erase(maxIt);
	}
}


bool N2FNewsManager::IsHigher( N2FMessage *oldMsg, N2FMessage *newMsg )
{
	//for (int i = 0; i < 12; i++)
	{
		int32 oldVal  = oldMsg->GetDateForSort();
		int32 newVal = newMsg->GetDateForSort();
		if ( oldVal < newVal )
		{
			return true;
		}
		if (oldVal > newVal)
		{
			return false;
		}
		oldVal  = oldMsg->GetTimeForSort();
		newVal = newMsg->GetTimeForSort();
		if ( oldVal < newVal )
		{
			return true;
		}
		if (oldVal > newVal)
		{
			return false;
		}
	}
	return false;

}














