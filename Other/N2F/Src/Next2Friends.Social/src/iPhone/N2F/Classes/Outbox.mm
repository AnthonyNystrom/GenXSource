
#import "Outbox.h"
#import "N2FAppDelegate.h"

@implementation Outbox

@synthesize tableView;

- (IBAction) onBack:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.mainMenuController];
}

- (void) onShow
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	self.tableView.delegate = delegate.draController;
	self.tableView.dataSource = delegate.draController;
	[self.tableView reloadData];
}


@end