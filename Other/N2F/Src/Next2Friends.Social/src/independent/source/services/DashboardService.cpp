#include "DashboardService.h"
#include "Server.h"
#include "PacketStream.h"
#include "AskQuestionConfirm.h"
#include "AskQuestion.h"
#include "AskComment.h"
#include "AskResponse.h"
#include "StringArray.h"
#include "ArrayOfDashboardVideo.h"
#include "DashboardVideo.h"
#include "DashboadMedia.h"
#include "ArrayOfDashboardPhoto.h"
#include "DashboardPhoto.h"
#include "ArrayOfDashboardWallComment.h"
#include "DashboardWallComment.h"
#include "ArrayOfDashboardNewFriend.h"
#include "DashboardNewFriend.h"


DashboardService::DashboardService( Server *server )
: BaseService(server)
{
}

DashboardService::~DashboardService()
{

}

bool DashboardService::GetNewFriends(IAnswer *answerCB, char16* nickname, char16* password)
{
    if (!StartRequest(answerCB, DashboardService::EPS_GETNEWFRIENDS))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(18);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);

    return isOk && EndRequest();

}

ArrayOfDashboardNewFriend *DashboardService::OnGetNewFriends()
{
  
  ArrayOfDashboardNewFriend *res = new ArrayOfDashboardNewFriend();
    res->ReadFromStream(answerStream);

    return res;
}


bool DashboardService::GetPhotos(IAnswer *answerCB, char16* nickname, char16* password)
{
    if (!StartRequest(answerCB, DashboardService::EPS_GETPHOTOS))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(19);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);

    return isOk && EndRequest();

}

ArrayOfDashboardPhoto *DashboardService::OnGetPhotos()
{
  
  ArrayOfDashboardPhoto *res = new ArrayOfDashboardPhoto();
    res->ReadFromStream(answerStream);

    return res;
}


bool DashboardService::GetVideos(IAnswer *answerCB, char16* nickname, char16* password)
{
    if (!StartRequest(answerCB, DashboardService::EPS_GETVIDEOS))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(20);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);

    return isOk && EndRequest();

}

ArrayOfDashboardVideo *DashboardService::OnGetVideos()
{
  
  ArrayOfDashboardVideo *res = new ArrayOfDashboardVideo();
    res->ReadFromStream(answerStream);

    return res;
}


bool DashboardService::GetWallComments(IAnswer *answerCB, char16* nickname, char16* password)
{
    if (!StartRequest(answerCB, DashboardService::EPS_GETWALLCOMMENTS))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(21);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);

    return isOk && EndRequest();

}

ArrayOfDashboardWallComment *DashboardService::OnGetWallComments()
{
  
  ArrayOfDashboardWallComment *res = new ArrayOfDashboardWallComment();
    res->ReadFromStream(answerStream);

    return res;
}


