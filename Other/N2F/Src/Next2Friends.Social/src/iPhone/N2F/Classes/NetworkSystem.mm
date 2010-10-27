
#import "NetworkSystem.h"
#import "InputStream.h"
#import "OutputStream.h"
#import <Foundation/NSURL.h>

@implementation NetworkSystem

+ (InputStream*)makeRequestTo:(NSString*)url from:(OutputStream*)os
{
	NSMutableURLRequest * request = [NSMutableURLRequest requestWithURL: [NSURL URLWithString:url]];
	NSHTTPURLResponse * response = nil;
	NSError * error = nil;
	
	[request setHTTPMethod:@"POST"];
	[request setHTTPBody:[os getData]];
	NSData * data = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&error];
	
	const void* bytes = [data bytes];
	
	InputStream * is =  [[InputStream alloc] initWithData:data];

	//[response release];
	//[request release];
	//[data release];
	
	return is;
}

@end
