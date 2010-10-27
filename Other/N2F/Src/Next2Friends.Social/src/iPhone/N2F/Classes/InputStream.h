#import "DashboardNewFriend.h"
#import "DashboardPhoto.h"
#import "DashboardVideo.h"
#import "DashboardWallComment.h"
#import "AskComment.h"
#import "AskQuestionStruct.h"
#import "AskResponse.h"
#import "AskQuestionConfirm.h"

@interface InputStream : NSObject
{
	NSInputStream * is;
}

- (id) initWithData:(NSData*)data;

- (int) readByte;
- (int) readByteArray:(char**)data;

- (BOOL) readBool;
- (int) readBoolArray: (BOOL**)data;

- (short) readShort;
- (int) readShortArray: (short**)data;

- (int) readInt;
- (int) readIntArray: (int**)data;

- (long long) readLong;
- (int) readLongArray: (long long**)data;

- (NSString*) readString;
- (int) readStringArray: (NSString***)data;

- (DashboardNewFriend*) readDashboardnewfriend;
- (NSArray*) readDashboardnewfriendArray;

- (DashboardWallComment*) readDashboardwallcomment;
- (NSArray*) readDashboardwallcommentArray;

- (DashboardPhoto*) readDashboardphoto;
- (NSArray*) readDashboardphotoArray;

- (DashboardVideo*) readDashboardvideo;
- (NSArray*) readDashboardvideoArray;

- (AskComment*) readAskcomment;
- (AskQuestionStruct*) readAskquestionstruct;
- (AskResponse*) readAskresponse;
- (AskQuestionConfirm*) readAskquestionconfirm;

@end
