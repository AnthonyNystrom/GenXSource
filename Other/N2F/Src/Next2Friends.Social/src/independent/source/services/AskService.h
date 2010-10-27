#ifndef __ASKSERVICE_H__
#define __ASKSERVICE_H__

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

class AskService : public BaseService
{
public:

enum eAskService
{
  
    EPS_ADDCOMMENT = 0 ,  
    EPS_ATTACHPHOTO ,  
    EPS_COMPLETEQUESTION ,  
    EPS_GETCOMMENT ,  
    EPS_GETCOMMENTIDS ,  
    EPS_GETQUESTION ,  
    EPS_GETQUESTIONIDS ,  
    EPS_GETRESPONSE ,  
    EPS_HASNEWCOMMENTS ,  
    EPS_SKIPQUESTION ,  
    EPS_SUBMITQUESTION ,  
    EPS_VOTEFORQUESTION
};

AskService( Server *server );
~AskService();


  
    bool    AddComment(IAnswer *answerCB, char16* nickname, char16* password, AskComment *newComment);
    int32 OnAddComment();
  
    bool    AttachPhoto(IAnswer *answerCB, char16* nickname, char16* password, char16* askQuestionID, int32 indexOrder, ArrayOfInt8 *photoBase64String);
    void OnAttachPhoto();
  
    bool    CompleteQuestion(IAnswer *answerCB, char16* nickname, char16* password, char16* askQuestionID);
    void OnCompleteQuestion();
  
    bool    GetComment(IAnswer *answerCB, char16* nickname, char16* password, int32 commentID);
    AskComment *OnGetComment();
  
    bool    GetCommentIDs(IAnswer *answerCB, char16* nickname, char16* password, int32 questionID, int32 lastCommentID);
    ArrayOfInt32 *OnGetCommentIDs();
  
    bool    GetQuestion(IAnswer *answerCB, char16* nickname, char16* password, int32 questionID);
    AskQuestion *OnGetQuestion();
  
    bool    GetQuestionIDs(IAnswer *answerCB, char16* nickname, char16* password);
    ArrayOfInt32 *OnGetQuestionIDs();
  
    bool    GetResponse(IAnswer *answerCB, char16* nickname, char16* password, int32 questionID);
    AskResponse *OnGetResponse();
  
    bool    HasNewComments(IAnswer *answerCB, char16* nickname, char16* password, int32 questionID, int32 lastCommentID);
    bool OnHasNewComments();
  
    bool    SkipQuestion(IAnswer *answerCB, char16* nickname, char16* password, int32 questionID);
    void OnSkipQuestion();
  
    bool    SubmitQuestion(IAnswer *answerCB, char16* nickname, char16* password, char16* questionText, int32 numberOfPhotos, int32 responseType, ArrayOfString *customResponses, int32 durationType, bool isPrivate);
    AskQuestionConfirm *OnSubmitQuestion();
  
    bool    VoteForQuestion(IAnswer *answerCB, char16* nickname, char16* password, int32 questionID, int32 result);
    void OnVoteForQuestion();

};
#endif // __ASKSERVICE_H__