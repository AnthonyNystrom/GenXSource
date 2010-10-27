#ifndef __DASHBOARDSERVICE_H__
#define __DASHBOARDSERVICE_H__

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

class DashboardService : public BaseService
{
public:

enum eDashboardService
{
  
    EPS_GETNEWFRIENDS = 0 ,  
    EPS_GETPHOTOS ,  
    EPS_GETVIDEOS ,  
    EPS_GETWALLCOMMENTS
};

DashboardService( Server *server );
~DashboardService();


  
    bool    GetNewFriends(IAnswer *answerCB, char16* nickname, char16* password);
    ArrayOfDashboardNewFriend *OnGetNewFriends();
  
    bool    GetPhotos(IAnswer *answerCB, char16* nickname, char16* password);
    ArrayOfDashboardPhoto *OnGetPhotos();
  
    bool    GetVideos(IAnswer *answerCB, char16* nickname, char16* password);
    ArrayOfDashboardVideo *OnGetVideos();
  
    bool    GetWallComments(IAnswer *answerCB, char16* nickname, char16* password);
    ArrayOfDashboardWallComment *OnGetWallComments();

};
#endif // __DASHBOARDSERVICE_H__