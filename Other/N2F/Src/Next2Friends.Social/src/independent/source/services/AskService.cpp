#include "AskService.h"
#include "Server.h"
#include "PacketStream.h"
#include "AskQuestionConfirm.h"
#include "AskQuestion.h"
#include "AskComment.h"
#include "AskResponse.h"
#include "StringArray.h"


AskService::AskService( Server *server )
: BaseService(server)
{
}

AskService::~AskService()
{

}

bool AskService::AddComment(IAnswer *answerCB, char16* nickname, char16* password, AskComment *newComment)
{
    if (!StartRequest(answerCB, AskService::EPS_ADDCOMMENT))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(6);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  newComment->WriteToStream(str);

    return isOk && EndRequest();

}

int32 AskService::OnAddComment()
{
  
    int32 res;
    answerStream->ReadInt32(res);

    return res;
}


bool AskService::AttachPhoto(IAnswer *answerCB, char16* nickname, char16* password, char16* askQuestionID, int32 indexOrder, ArrayOfInt8 *photoBase64String)
{
    if (!StartRequest(answerCB, AskService::EPS_ATTACHPHOTO))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(7);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  str->WriteWString(askQuestionID);
    isOk = isOk &&  str->WriteInt32(indexOrder);
    isOk = isOk &&  photoBase64String->WriteToStream(str);

    return isOk && EndRequest();

}

void AskService::OnAttachPhoto()
{
}


bool AskService::CompleteQuestion(IAnswer *answerCB, char16* nickname, char16* password, char16* askQuestionID)
{
    if (!StartRequest(answerCB, AskService::EPS_COMPLETEQUESTION))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(8);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  str->WriteWString(askQuestionID);

    return isOk && EndRequest();

}

void AskService::OnCompleteQuestion()
{
}


bool AskService::GetComment(IAnswer *answerCB, char16* nickname, char16* password, int32 commentID)
{
    if (!StartRequest(answerCB, AskService::EPS_GETCOMMENT))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(9);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  str->WriteInt32(commentID);

    return isOk && EndRequest();

}

AskComment *AskService::OnGetComment()
{
  
  AskComment *res = new AskComment();
    res->ReadFromStream(answerStream);

    return res;
}


bool AskService::GetCommentIDs(IAnswer *answerCB, char16* nickname, char16* password, int32 questionID, int32 lastCommentID)
{
    if (!StartRequest(answerCB, AskService::EPS_GETCOMMENTIDS))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(10);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  str->WriteInt32(questionID);
    isOk = isOk &&  str->WriteInt32(lastCommentID);

    return isOk && EndRequest();

}

ArrayOfInt32 *AskService::OnGetCommentIDs()
{
  
  ArrayOfInt32 *res = new ArrayOfInt32();
    res->ReadFromStream(answerStream);

    return res;
}


bool AskService::GetQuestion(IAnswer *answerCB, char16* nickname, char16* password, int32 questionID)
{
    if (!StartRequest(answerCB, AskService::EPS_GETQUESTION))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(11);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  str->WriteInt32(questionID);

    return isOk && EndRequest();

}

AskQuestion *AskService::OnGetQuestion()
{
  
  AskQuestion *res = new AskQuestion();
    res->ReadFromStream(answerStream);

    return res;
}


bool AskService::GetQuestionIDs(IAnswer *answerCB, char16* nickname, char16* password)
{
    if (!StartRequest(answerCB, AskService::EPS_GETQUESTIONIDS))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(12);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);

    return isOk && EndRequest();

}

ArrayOfInt32 *AskService::OnGetQuestionIDs()
{
  
  ArrayOfInt32 *res = new ArrayOfInt32();
    res->ReadFromStream(answerStream);

    return res;
}


bool AskService::GetResponse(IAnswer *answerCB, char16* nickname, char16* password, int32 questionID)
{
    if (!StartRequest(answerCB, AskService::EPS_GETRESPONSE))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(13);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  str->WriteInt32(questionID);

    return isOk && EndRequest();

}

AskResponse *AskService::OnGetResponse()
{
  
  AskResponse *res = new AskResponse();
    res->ReadFromStream(answerStream);

    return res;
}


bool AskService::HasNewComments(IAnswer *answerCB, char16* nickname, char16* password, int32 questionID, int32 lastCommentID)
{
    if (!StartRequest(answerCB, AskService::EPS_HASNEWCOMMENTS))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(14);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  str->WriteInt32(questionID);
    isOk = isOk &&  str->WriteInt32(lastCommentID);

    return isOk && EndRequest();

}

bool AskService::OnHasNewComments()
{
  
    int8 res;
    answerStream->ReadInt8(res);

    return res;
}


bool AskService::SkipQuestion(IAnswer *answerCB, char16* nickname, char16* password, int32 questionID)
{
    if (!StartRequest(answerCB, AskService::EPS_SKIPQUESTION))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(15);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  str->WriteInt32(questionID);

    return isOk && EndRequest();

}

void AskService::OnSkipQuestion()
{
}


bool AskService::SubmitQuestion(IAnswer *answerCB, char16* nickname, char16* password, char16* questionText, int32 numberOfPhotos, int32 responseType, ArrayOfString *customResponses, int32 durationType, bool isPrivate)
{
    if (!StartRequest(answerCB, AskService::EPS_SUBMITQUESTION))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(16);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  str->WriteWString(questionText);
    isOk = isOk &&  str->WriteInt32(numberOfPhotos);
    isOk = isOk &&  str->WriteInt32(responseType);
    isOk = isOk &&  customResponses->WriteToStream(str);
    isOk = isOk &&  str->WriteInt32(durationType);
    isOk = isOk &&  str->WriteInt8((int8)isPrivate);

    return isOk && EndRequest();

}

AskQuestionConfirm *AskService::OnSubmitQuestion()
{
  
  AskQuestionConfirm *res = new AskQuestionConfirm();
    res->ReadFromStream(answerStream);

    return res;
}


bool AskService::VoteForQuestion(IAnswer *answerCB, char16* nickname, char16* password, int32 questionID, int32 result)
{
    if (!StartRequest(answerCB, AskService::EPS_VOTEFORQUESTION))
    {
        return false;
    }

    PacketStream *str = pServer->GetStream();
    bool isOk = true;

    isOk = isOk && str->WriteInt32(17);//request id

  
    isOk = isOk &&  str->WriteWString(nickname);
    isOk = isOk &&  str->WriteWString(password);
    isOk = isOk &&  str->WriteInt32(questionID);
    isOk = isOk &&  str->WriteInt32(result);

    return isOk && EndRequest();

}

void AskService::OnVoteForQuestion()
{
}


