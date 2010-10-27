
#import "DashboardCell.h"
#import "N2FAppDelegate.h"
#import "DashboardMessage.h"

NSArray * months = [[NSArray alloc] initWithObjects: @"Jan", @"Feb", @"Mar", @"Apr", @"May", @"Jun", @"Jul", @"Aug", @"Sep", @"Oct", @"Nov", @"Dec", nil];

@implementation DashboardCell

@synthesize date;
@synthesize content;
@synthesize showDate;

- (void) convertDate
{
	NSRange rng;
	rng.location = 0;
	rng.length = 4;
	//int year = [[date substringWithRange:rng] intValue];
	rng.location = 4;
	rng.length = 2;
	int month = [[date substringWithRange:rng] intValue];
	rng.location = 6;
	rng.length = 2;
	//int day = [[date substringWithRange:rng] intValue];
	NSMutableString * newDate = [[NSMutableString alloc] init];
	[newDate appendString:[months objectAtIndex:month-1]];
	[newDate appendString:@" "];
	[newDate appendString:[date substringWithRange:rng]];
	
	self->showDate = newDate;
}

- (void) onDetails:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.dashboardMessageController];
	((DashboardMessage*)delegate.dashboardMessageController.view).date.text = self->showDate;
	((DashboardMessage*)delegate.dashboardMessageController.view).text.text = self->content;
}

- (void) setup
{
	self.accessoryType = UITableViewCellAccessoryDetailDisclosureButton;
	self.font = [UIFont systemFontOfSize: 12];
	self.target = self;
	self.accessoryAction = @selector(onDetails:);
}



@end
