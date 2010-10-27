
#import "DashboardController.h"
#import "Dashboard.h"
#import "DashboardCell.h"


@implementation DashboardController

@synthesize items;

- (id)init
{
	items = [[NSMutableArray alloc] init];
	
	return self;
}

- (void) dealloc
{
	[items release];
	
	[super dealloc];
}

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section 
{
    return [self.items count];
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath 
{
	DashboardCell * cell = (DashboardCell *)[items objectAtIndex:indexPath.row];
	 
	return cell;
}



@end
