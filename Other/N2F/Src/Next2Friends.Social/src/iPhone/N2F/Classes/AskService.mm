#import "AskService.h"
#import "InputStream.h"
#import "OutputStream.h"
#import "AskComment.h"
#import "AskQuestionConfirm.h"
#import "AskQuestionStruct.h"
#import "AskResponse.h"

@implementation AskComment
@end
@implementation AskQuestionConfirm
@end
@implementation AskQuestionStruct
@end
@implementation AskResponse
@end


@implementation AskService

- (int)AddComment:(NSString*)nickname :(NSString*)password :(AskComment*)newComment 
{
    [self prepare:6];

    [os writeString:nickname];
    [os writeString:password];
    [os writeAskcomment:newComment];

    [self commit];

  
    int res = [is readInt];
    return res;
}

- (void)AttachPhoto:(NSString*)nickname :(NSString*)password :(NSString*)askQuestionID :(int)indexOrder :(char*)photoBase64String :(int)size
{
    [self prepare:7];

    [os writeString:nickname];
    [os writeString:password];
    [os writeString:askQuestionID];
    [os writeInt:indexOrder];
    [os writeByteArray:photoBase64String size:size];

    [self commit];

}

- (void)CompleteQuestion:(NSString*)nickname :(NSString*)password :(NSString*)askQuestionID 
{
    [self prepare:8];

    [os writeString:nickname];
    [os writeString:password];
    [os writeString:askQuestionID];

    [self commit];

}

- (AskComment*)GetComment:(NSString*)nickname :(NSString*)password :(int)commentID 
{
    [self prepare:9];

    [os writeString:nickname];
    [os writeString:password];
    [os writeInt:commentID];

    [self commit];

  
    AskComment* res = [is readAskcomment];
    return res;
}

- (int)GetCommentIDs:(NSString*)nickname :(NSString*)password :(int)questionID :(int)lastCommentID :(int**)data
{
    [self prepare:10];

    [os writeString:nickname];
    [os writeString:password];
    [os writeInt:questionID];
    [os writeInt:lastCommentID];

    [self commit];

    int count = [is readIntArray:data];
    return count;
}

- (AskQuestionStruct*)GetQuestion:(NSString*)nickname :(NSString*)password :(int)questionID 
{
    [self prepare:11];

    [os writeString:nickname];
    [os writeString:password];
    [os writeInt:questionID];

    [self commit];

  
    AskQuestionStruct* res = [is readAskquestionstruct];
    return res;
}

- (int)GetQuestionIDs:(NSString*)nickname :(NSString*)password :(int**)data;
{
    [self prepare:12];

    [os writeString:nickname];
    [os writeString:password];

    [self commit];

    int count = [is readIntArray:data];
    return count;
}

- (AskResponse*)GetResponse:(NSString*)nickname :(NSString*)password :(int)questionID 
{
    [self prepare:13];

    [os writeString:nickname];
    [os writeString:password];
    [os writeInt:questionID];

    [self commit];

  
    AskResponse* res = [is readAskresponse];
    return res;
}

- (BOOL)HasNewComments:(NSString*)nickname :(NSString*)password :(int)questionID :(int)lastCommentID 
{
    [self prepare:14];

    [os writeString:nickname];
    [os writeString:password];
    [os writeInt:questionID];
    [os writeInt:lastCommentID];

    [self commit];

  
    BOOL res = [is readBool];
    return res;
}

- (void)SkipQuestion:(NSString*)nickname :(NSString*)password :(int)questionID 
{
    [self prepare:15];

    [os writeString:nickname];
    [os writeString:password];
    [os writeInt:questionID];

    [self commit];

}

- (AskQuestionConfirm*)SubmitQuestion:(NSString*)nickname :(NSString*)password :(NSString*)questionText :(int)numberOfPhotos :(int)responseType :(NSArray*)customResponses :(int)durationType :(BOOL)isPrivate 
{
    [self prepare:16];

    [os writeString:nickname];
    [os writeString:password];
    [os writeString:questionText];
    [os writeInt:numberOfPhotos];
    [os writeInt:responseType];
	
    NSString ** customresponses = new NSString*[2];
    customresponses[0] = [customResponses objectAtIndex:0];
    customresponses[1] = [customResponses objectAtIndex:1];
    [os writeStringArray:customresponses size:2];
    [os writeInt:durationType];
    [os writeBool:isPrivate];

    [self commit];

  
    AskQuestionConfirm* res = [is readAskquestionconfirm];
    delete[] customresponses;
    return res;
}

 

@end