#include "MemberService.h"
#include "Server.h"
#include "PacketStream.h"


MemberService::MemberService( Server *server )
: BaseService(server)
{
}

MemberService::~MemberService()
{

}

bool MemberService::CheckUserExists(IAnswer *answerCB, char16* nickname, char16* password)
{
    if (!StartRequest(answerCB, MemberService::EPS_CHECKUSEREXISTS))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(1);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);

    return isOk && EndRequest();

}

bool MemberService::OnCheckUserExists()
{
  
    int8 res;
    answerStream->ReadInt8(res);

    return res;
}


bool MemberService::GetEncryptionKey(IAnswer *answerCB, char16* nickname, char16* password)
{
    if (!StartRequest(answerCB, MemberService::EPS_GETENCRYPTIONKEY))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(2);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);

    return isOk && EndRequest();

}

char16* MemberService::OnGetEncryptionKey()
{
  
    char16 *res = answerStream->GetWString();

    return res;
}


bool MemberService::GetMemberID(IAnswer *answerCB, char16* nickname, char16* password)
{
    if (!StartRequest(answerCB, MemberService::EPS_GETMEMBERID))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(3);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);

    return isOk && EndRequest();

}

char16* MemberService::OnGetMemberID()
{
  
    char16 *res = answerStream->GetWString();

    return res;
}


bool MemberService::GetMemberStatusText(IAnswer *answerCB, char16* nickname, char16* password)
{
    if (!StartRequest(answerCB, MemberService::EPS_GETMEMBERSTATUSTEXT))
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

char16* MemberService::OnGetMemberStatusText()
{
  
    char16 *res = answerStream->GetWString();

    return res;
}


bool MemberService::GetProfilePhotoUrl(IAnswer *answerCB, char16* nickname)
{
    if (!StartRequest(answerCB, MemberService::EPS_GETPROFILEPHOTOURL))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(5);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);

    return isOk && EndRequest();

}

char16* MemberService::OnGetProfilePhotoUrl()
{
  
    char16 *res = answerStream->GetWString();

    return res;
}


bool MemberService::GetTagID(IAnswer *answerCB, char16* nickname, char16* password)
{
    if (!StartRequest(answerCB, MemberService::EPS_GETTAGID))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(6);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);

    return isOk && EndRequest();

}

char16* MemberService::OnGetTagID()
{
  
    char16 *res = answerStream->GetWString();

    return res;
}


bool MemberService::RemindPassword(IAnswer *answerCB, char16* emailAddress)
{
    if (!StartRequest(answerCB, MemberService::EPS_REMINDPASSWORD))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(7);//request id

  
    isOk = isOk &&  str->WriteWString(emailAddress);

    return isOk && EndRequest();

}

void MemberService::OnRemindPassword()
{
}


bool MemberService::SetMemberStatusText(IAnswer *answerCB, char16* nickname, char16* password, char16* statusText)
{
    if (!StartRequest(answerCB, MemberService::EPS_SETMEMBERSTATUSTEXT))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(24);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  str->WriteWString(statusText);

    return isOk && EndRequest();

}

void MemberService::OnSetMemberStatusText()
{
}


