
#import "BaseService.h"

@interface MemberService : BaseService
{
}

- (BOOL) CheckUserExists:(NSString*)username password:(NSString*)password;

@end
