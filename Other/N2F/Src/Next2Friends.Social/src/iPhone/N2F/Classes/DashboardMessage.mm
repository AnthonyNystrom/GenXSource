
#import "DashboardMessage.h"
#import "N2FAppDelegate.h"

@implementation DashboardMessage

@synthesize date;
@synthesize text;

- (IBAction) onBack:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.dashboardController];
}

@end
