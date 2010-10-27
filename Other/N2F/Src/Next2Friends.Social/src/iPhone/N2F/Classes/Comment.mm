
#import "Comment.h"
#import "N2FAppDelegate.h"
#import "AskService.h"
#import "AskComment.h"
#import "Preferences.h"

@implementation WriteComment

@synthesize comment;

- (IBAction) onBack:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.responseController];
}


- (IBAction) onSend:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate showPopup:@"Please wait" lock:true];
	[NSThread detachNewThreadSelector:@selector(networkThread:) toTarget:self withObject:nil];
	
}

- (void) networkThread:(id)object
{
	NSAutoreleasePool *pool = [NSAutoreleasePool new];
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	
	AskService * service = [AskService new];
	AskComment * newComment = [AskComment new];
	newComment->id = 0;
	newComment->askquestionid = delegate.currentResponse;
	newComment->nickname = delegate.prefs.username;
	newComment->text = comment.text;
	newComment->dtcreated = @"";

	[service AddComment:delegate.prefs.username :delegate.prefs.password :newComment];
	
	 [comment release];
	[service release];
	[delegate hidePopup];
	[delegate changeScreen:delegate.mainMenuController];
	[pool release];
}


@end
