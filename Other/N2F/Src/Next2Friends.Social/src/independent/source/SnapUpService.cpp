#include "SnapUpService.h"
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
#include "ArrayOfPhotoCollectionItem.h"
#include "PhotoCollectionItem.h"
#include "ArrayOfPhotoItem.h"
#include "PhotoItem.h"
#include "StringArray.h"


SnapUpService::SnapUpService( Server *server )
: BaseService(server)
{
}

SnapUpService::~SnapUpService()
{

}

bool SnapUpService::CreateCollection(IAnswer *answerCB, char16* nickname, char16* password, char16* collectionName)
{
    if (!StartRequest(answerCB, SnapUpService::EPS_CREATECOLLECTION))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(22);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  str->WriteWString(collectionName);

    return isOk && EndRequest();

}

void SnapUpService::OnCreateCollection()
{
}


bool SnapUpService::DeletePhoto(IAnswer *answerCB, char16* nickname, char16* password, char16* webPhotoID)
{
    if (!StartRequest(answerCB, SnapUpService::EPS_DELETEPHOTO))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(23);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  str->WriteWString(webPhotoID);

    return isOk && EndRequest();

}

void SnapUpService::OnDeletePhoto()
{
}


bool SnapUpService::DeviceUploadPhoto(IAnswer *answerCB, char16* nickname, char16* password, ArrayOfInt8 *base64StringPhoto, char16* dateTime)
{
    if (!StartRequest(answerCB, SnapUpService::EPS_DEVICEUPLOADPHOTO))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(24);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  base64StringPhoto->WriteToStream(str);
    isOk = isOk &&  str->WriteWString(dateTime);

    return isOk && EndRequest();

}

void SnapUpService::OnDeviceUploadPhoto()
{
}


bool SnapUpService::GetCollections(IAnswer *answerCB, char16* nickname, char16* password)
{
    if (!StartRequest(answerCB, SnapUpService::EPS_GETCOLLECTIONS))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(25);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);

    return isOk && EndRequest();

}

ArrayOfPhotoCollectionItem *SnapUpService::OnGetCollections()
{
  
  ArrayOfPhotoCollectionItem *res = new ArrayOfPhotoCollectionItem();
    res->ReadFromStream(answerStream);

    return res;
}


bool SnapUpService::GetPhotosByCollection(IAnswer *answerCB, char16* nickname, char16* password, char16* webPhotoCollectionID)
{
    if (!StartRequest(answerCB, SnapUpService::EPS_GETPHOTOSBYCOLLECTION))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(26);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  str->WriteWString(webPhotoCollectionID);

    return isOk && EndRequest();

}

ArrayOfPhotoItem *SnapUpService::OnGetPhotosByCollection()
{
  
  ArrayOfPhotoItem *res = new ArrayOfPhotoItem();
    res->ReadFromStream(answerStream);

    return res;
}


bool SnapUpService::JavaUploadPhoto(IAnswer *answerCB, char16* encryptedWebMemberID, char16* webPhotoCollectionID, ArrayOfInt8 *base64StringPhoto, char16* dateTime)
{
    if (!StartRequest(answerCB, SnapUpService::EPS_JAVAUPLOADPHOTO))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(27);//request id

  
    isOk = isOk &&  str->WriteWString(encryptedWebMemberID);
    isOk = isOk &&  str->WriteWString(webPhotoCollectionID);
    isOk = isOk &&  base64StringPhoto->WriteToStream(str);
    isOk = isOk &&  str->WriteWString(dateTime);

    return isOk && EndRequest();

}

void SnapUpService::OnJavaUploadPhoto()
{
}


bool SnapUpService::RenameCollection(IAnswer *answerCB, char16* nickname, char16* password, char16* webPhotoCollectionID, char16* newName)
{
    if (!StartRequest(answerCB, SnapUpService::EPS_RENAMECOLLECTION))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(28);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  str->WriteWString(webPhotoCollectionID);
    isOk = isOk &&  str->WriteWString(newName);

    return isOk && EndRequest();

}

void SnapUpService::OnRenameCollection()
{
}


bool SnapUpService::UploadPhoto(IAnswer *answerCB, char16* nickname, char16* password, char16* webPhotoCollectionID, ArrayOfInt8 *photoFileBytes, char16* takenDT)
{
    if (!StartRequest(answerCB, SnapUpService::EPS_UPLOADPHOTO))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(29);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  str->WriteWString(webPhotoCollectionID);
    isOk = isOk &&  photoFileBytes->WriteToStream(str);
    isOk = isOk &&  str->WriteWString(takenDT);

    return isOk && EndRequest();

}

void SnapUpService::OnUploadPhoto()
{
}


