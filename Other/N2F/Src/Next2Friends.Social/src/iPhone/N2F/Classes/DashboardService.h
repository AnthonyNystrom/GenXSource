#import "BaseService.h"

@interface DashboardService : BaseService
{
}

- (NSArray*)GetNewFriends:(NSString*)nickname :(NSString*)password;
- (NSArray*)GetPhotos:(NSString*)nickname :(NSString*)password;
- (NSArray*)GetVideos:(NSString*)nickname :(NSString*)password;
- (NSArray*)GetWallComments:(NSString*)nickname :(NSString*)password;

@end 