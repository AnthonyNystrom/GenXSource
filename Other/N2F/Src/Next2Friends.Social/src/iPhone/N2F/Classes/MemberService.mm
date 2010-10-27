
#import "MemberService.h"
#import "InputStream.h"
#import "OutputStream.h"

@implementation MemberService

- (BOOL) CheckUserExists:(NSString*)username password:(NSString*)password
{
	[self prepare:1];
	[os writeString:username];
	[os writeString:password];
	
	[self commit];
	
	BOOL res = [is readBool];
	
	return res;
}

@end
