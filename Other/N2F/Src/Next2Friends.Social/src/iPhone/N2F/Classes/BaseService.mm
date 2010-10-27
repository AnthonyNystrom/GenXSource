
#import "BaseService.h"
#import "OutputStream.h"
#import "InputStream.h"
#import "NetworkSystem.h"

@implementation BaseService

@synthesize is;
@synthesize os;

- (id) init
{
	url = @"http://69.21.114.101:100/soap2bin.handler";
	return self;
}

- (void) prepare: (NSInteger) requestId
{
	os = [[OutputStream alloc] init];
	[os writeShort:1];
	[os writeInt: requestId];
}

- (void) commit
{
	is = [NetworkSystem makeRequestTo:url from: os];
	
	short errorCode = [is readShort];
	if(errorCode != 1)
	{
		NSLog(@"BaseService::commit error: remote service returned %d", errorCode);
	}
}

- (void) dealloc
{
	[os release];
	[is release];
	
	[super dealloc];
}

@end


