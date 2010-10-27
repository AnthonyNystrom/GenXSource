#import "DashboardService.h"
#import "InputStream.h"
#import "OutputStream.h"

@implementation  DashboardNewFriend
@end
@implementation  DashboardWallComment
@end
@implementation  DashboardPhoto
@end
@implementation  DashboardVideo
@end

@implementation DashboardService
- (NSArray*)GetNewFriends:(NSString*)nickname :(NSString*)password 
{
    [self prepare:18];

    [os writeString:nickname];
    [os writeString:password];

    [self commit];

  
    NSArray* res = [is readDashboardnewfriendArray];
    return res;
}

- (NSArray*)GetPhotos:(NSString*)nickname :(NSString*)password 
{
    [self prepare:19];

    [os writeString:nickname];
    [os writeString:password];

    [self commit];

  
    NSArray* res = [is readDashboardphotoArray];
    return res;
}

- (NSArray*)GetVideos:(NSString*)nickname :(NSString*)password 
{
    [self prepare:20];

    [os writeString:nickname];
    [os writeString:password];

    [self commit];

  
    NSArray* res = [is readDashboardvideoArray];
    return res;
}

- (NSArray*)GetWallComments:(NSString*)nickname :(NSString*)password 
{
    [self prepare:21];

    [os writeString:nickname];
    [os writeString:password];

    [self commit];

  
    NSArray* res = [is readDashboardwallcommentArray];
    return res;
}

 

@end