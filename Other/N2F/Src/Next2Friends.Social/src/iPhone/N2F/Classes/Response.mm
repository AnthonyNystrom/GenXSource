#import "Response.h"
#import "N2FAppDelegate.h"
#import "AskService.h"
#import "AskResponse.h"
#import "Preferences.h"
#import "AskComment.h"

@implementation Response

@synthesize image;
@synthesize a;
@synthesize b;
@synthesize comments;

- (IBAction) onBack:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.inboxController];
}

- (IBAction) onWrite:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.commentController];
}

- (void) onShow
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate showPopup:@"Please wait" lock:true];
	[NSThread detachNewThreadSelector:@selector(networkThread:) toTarget:self withObject:nil];
	comments.text = @"Comments";
}

- (void) updateComments
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	comments.text = delegate.commentsText;
}

- (void) networkThread:(id)object
{
	NSAutoreleasePool *pool = [NSAutoreleasePool new];
	
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	AskService * service = [AskService new];
	AskResponse * r = [service GetResponse:delegate.prefs.username :delegate.prefs.password :delegate.currentResponse];
	NSData * imageData = [[NSData alloc] initWithBytes: r->photobase64binary length: r->size];
	UIImage * newImage = [[UIImage alloc] initWithData: imageData];
	image.image = newImage;
	[newImage release];
	[imageData release];
	
	switch(r->responsetype+1)
	{
		case 1://yesno
			a.text = [NSString stringWithFormat:@"Yes: %d",  r->responsevalues[0]];
			b.text = [NSString stringWithFormat:@"No: %d",  r->responsevalues[1]];
			break;
		case 2://ab
		{
			a.text = [NSString stringWithFormat:@"%s: %d", [r->customresponses objectAtIndex:0],  r->responsevalues[0]];
			b.text = [NSString stringWithFormat:@"%s: %d", [r->customresponses objectAtIndex:1],  r->responsevalues[1]];		
		}
			break;
		case 3://rate
			float av = (float)((r->average)>>16) +(float)((r->average)&0xffff)/65536;;
			a.text = [NSString stringWithFormat:@"Average: %.1f", av];
			b.text = @"";
			break;
		default://multiple:
			a.text = @"";
			b.text = @"";
			break;
	}
	
	delegate.commentsText = @"";
	int * ids;
	int commentsCount = [service GetCommentIDs:delegate.prefs.username :delegate.prefs.password :delegate.currentResponse :0 :&ids];
	for(int i = 0; i < commentsCount; ++i)
	{
		AskComment * c = [service GetComment:delegate.prefs.username :delegate.prefs.password :ids[i]];
		NSString * oldText = delegate.commentsText;
		NSString * newText = [[oldText stringByAppendingFormat:@"%@: %@\n",  c->nickname, c->text]retain];
		delegate.commentsText = newText;
	}
	
	[r release];
	[service release];
	[delegate hidePopup];
	[pool release];
}

- (IBAction) onUpdate:(id)sender
{
	[self updateComments];
}


@end