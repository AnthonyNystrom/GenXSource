#import "BaseService.h"

@class AskComment;
@class AskResponse;
@class AskQuestionStruct;
@class AskQuestionConfirm;
@interface AskService : BaseService
{
}

- (int)AddComment:(NSString*)nickname :(NSString*)password :(AskComment*)newComment;
- (void)AttachPhoto:(NSString*)nickname :(NSString*)password :(NSString*)askQuestionID :(int)indexOrder :(char*)photoBase64String :(int)size;
- (void)CompleteQuestion:(NSString*)nickname :(NSString*)password :(NSString*)askQuestionID;
- (AskComment*)GetComment:(NSString*)nickname :(NSString*)password :(int)commentID;
- (int)GetCommentIDs:(NSString*)nickname :(NSString*)password :(int)questionID :(int)lastCommentID :(int**)data;
- (AskQuestionStruct*)GetQuestion:(NSString*)nickname :(NSString*)password :(int)questionID;
- (int)GetQuestionIDs:(NSString*)nickname :(NSString*)password :(int**)data;
- (AskResponse*)GetResponse:(NSString*)nickname :(NSString*)password :(int)questionID;
- (BOOL)HasNewComments:(NSString*)nickname :(NSString*)password :(int)questionID :(int)lastCommentID;
- (void)SkipQuestion:(NSString*)nickname :(NSString*)password :(int)questionID;
- (AskQuestionConfirm*)SubmitQuestion:(NSString*)nickname :(NSString*)password :(NSString*)questionText :(int)numberOfPhotos :(int)responseType :(NSArray*)customResponses :(int)durationType :(BOOL)isPrivate;

@end 