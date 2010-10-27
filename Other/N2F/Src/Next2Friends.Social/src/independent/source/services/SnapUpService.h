#ifndef __SNAPUPSERVICE_H__
#define __SNAPUPSERVICE_H__

#include "BaseTypes.h"

#include "BaseService.h"

class Server;
struct ArrayOfString;
struct AskQuestionConfirm;
struct AskQuestion;
struct ArrayOfInt32;
struct AskComment;
struct AskResponse;
struct StringArray;
struct ArrayOfDashboardVideo;
struct DashboardVideo;
struct DashboadMedia;
struct ArrayOfDashboardPhoto;
struct DashboardPhoto;
struct ArrayOfDashboardWallComment;
struct DashboardWallComment;
struct ArrayOfDashboardNewFriend;
struct DashboardNewFriend;
struct ArrayOfPhotoCollectionItem;
struct PhotoCollectionItem;
struct ArrayOfPhotoItem;
struct PhotoItem;
struct StringArray;

class SnapUpService : public BaseService
{
public:

enum eSnapUpService
{
  
    EPS_CREATECOLLECTION = 0 ,  
    EPS_DELETEPHOTO ,  
    EPS_DEVICEUPLOADPHOTO ,  
    EPS_GETCOLLECTIONS ,  
    EPS_GETPHOTOSBYCOLLECTION ,  
    EPS_JAVAUPLOADPHOTO ,  
    EPS_RENAMECOLLECTION ,  
    EPS_UPLOADPHOTO
};

SnapUpService( Server *server );
~SnapUpService();


  
    bool    CreateCollection(IAnswer *answerCB, char16* nickname, char16* password, char16* collectionName);
    void OnCreateCollection();
  
    bool    DeletePhoto(IAnswer *answerCB, char16* nickname, char16* password, char16* webPhotoID);
    void OnDeletePhoto();
  
    bool    DeviceUploadPhoto(IAnswer *answerCB, char16* nickname, char16* password, ArrayOfInt8 *base64StringPhoto, char16* dateTime);
    void OnDeviceUploadPhoto();
  
    bool    GetCollections(IAnswer *answerCB, char16* nickname, char16* password);
    ArrayOfPhotoCollectionItem *OnGetCollections();
  
    bool    GetPhotosByCollection(IAnswer *answerCB, char16* nickname, char16* password, char16* webPhotoCollectionID);
    ArrayOfPhotoItem *OnGetPhotosByCollection();
  
    bool    JavaUploadPhoto(IAnswer *answerCB, char16* encryptedWebMemberID, char16* webPhotoCollectionID, ArrayOfInt8 *base64StringPhoto, char16* dateTime);
    void OnJavaUploadPhoto();
  
    bool    RenameCollection(IAnswer *answerCB, char16* nickname, char16* password, char16* webPhotoCollectionID, char16* newName);
    void OnRenameCollection();
  
    bool    UploadPhoto(IAnswer *answerCB, char16* nickname, char16* password, char16* webPhotoCollectionID, ArrayOfInt8 *photoFileBytes, char16* takenDT);
    void OnUploadPhoto();

};
#endif // __SNAPUPSERVICE_H__