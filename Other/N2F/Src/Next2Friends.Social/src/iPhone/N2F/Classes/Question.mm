
#import "Question.h"

@implementation Question 

@synthesize question;
@synthesize responseType;
@synthesize duration;
@synthesize isPrivate;
@synthesize responseA;
@synthesize responseB;
@synthesize photos;
@synthesize recordId;
@synthesize sendNow;

- (id) init
{
	question = @"";
	responseA = @"";
	responseB = @"";
	photos = [NSMutableArray new];
	return self;
}

- (void) dealloc
{
	[question release];
	[responseA release];
	[responseB release];
	[photos release];
	
	[super dealloc];
}

@end
