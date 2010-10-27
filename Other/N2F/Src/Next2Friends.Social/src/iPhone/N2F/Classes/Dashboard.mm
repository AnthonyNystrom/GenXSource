
#import "Dashboard.h"
#import "N2FAppDelegate.h"
#import "Preferences.h"
#import "DashboardCell.h"

@implementation Dashboard

@synthesize tableView;

- (IBAction) onBack:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.mainMenuController];
}

NSInteger dashSort(id num1, id num2, void *context)
{
	long long  cell1 = [((DashboardCell*)num1).date longLongValue];
	long long cell2 = [((DashboardCell*)num2).date longLongValue];
	if (cell1 > cell2)
		return NSOrderedAscending;
	else if (cell1 < cell2)
		return NSOrderedDescending;
	else
		return NSOrderedSame;
}

- (void) onShow
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	self.tableView.delegate = delegate.dashController;
	self.tableView.dataSource = delegate.dashController;
	[self.tableView reloadData];
	
	//sort dashboard
	NSArray * newItems =  [delegate.dashController.items sortedArrayUsingFunction:dashSort context:0];
	[delegate.dashController.items removeAllObjects];
	[delegate.dashController.items addObjectsFromArray:newItems];
}

@end