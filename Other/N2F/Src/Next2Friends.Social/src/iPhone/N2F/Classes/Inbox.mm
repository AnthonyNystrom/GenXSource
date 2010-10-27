
#import "Inbox.h"
#import "N2FAppDelegate.h"

@implementation Inbox

@synthesize tableView;

- (IBAction) onBack:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.mainMenuController];
}

- (void) onShow
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	self.tableView.delegate = delegate.inbController;
	self.tableView.dataSource = delegate.inbController;
	[self.tableView reloadData];
}


@end