#ifndef __MEMBERSERVICE_H__
#define __MEMBERSERVICE_H__

#include "BaseTypes.h"

#include "BaseService.h"

class Server;

class MemberService : public BaseService
{
public:

enum eMemberService
{
  
    EPS_CHECKUSEREXISTS = 0 ,  
    EPS_GETENCRYPTIONKEY ,  
    EPS_GETMEMBERID ,  
    EPS_GETMEMBERSTATUSTEXT ,  
    EPS_GETPROFILEPHOTOURL ,  
    EPS_GETTAGID ,  
    EPS_REMINDPASSWORD ,  
    EPS_SETMEMBERSTATUSTEXT
};

MemberService( Server *server );
~MemberService();


  
    bool    CheckUserExists(IAnswer *answerCB, char16* nickname, char16* password);
    bool OnCheckUserExists();
  
    bool    GetEncryptionKey(IAnswer *answerCB, char16* nickname, char16* password);
    char16* OnGetEncryptionKey();
  
    bool    GetMemberID(IAnswer *answerCB, char16* nickname, char16* password);
    char16* OnGetMemberID();
  
    bool    GetMemberStatusText(IAnswer *answerCB, char16* nickname, char16* password);
    char16* OnGetMemberStatusText();
  
    bool    GetProfilePhotoUrl(IAnswer *answerCB, char16* nickname);
    char16* OnGetProfilePhotoUrl();
  
    bool    GetTagID(IAnswer *answerCB, char16* nickname, char16* password);
    char16* OnGetTagID();
  
    bool    RemindPassword(IAnswer *answerCB, char16* emailAddress);
    void OnRemindPassword();
  
    bool    SetMemberStatusText(IAnswer *answerCB, char16* nickname, char16* password, char16* statusText);
    void OnSetMemberStatusText();

};
#endif // __MEMBERSERVICE_H__