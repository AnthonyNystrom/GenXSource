
#import "InboxCell.h"
#import "Response.h"
#import  "N2FAppDelegate.h"
#import "AskService.h"
#import "Preferences.h"
#import "AskComment.h"


@implementation InboxCell

@synthesize id;

- (void) onDetails:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	delegate.currentResponse = id;
	[delegate changeScreen:delegate.responseController];
}

- (void) setup
{
	self.accessoryType = UITableViewCellAccessoryDetailDisclosureButton;
	self.font = [UIFont systemFontOfSize: 12];
	self.target = self;
	self.accessoryAction = @selector(onDetails:);
}

@end
