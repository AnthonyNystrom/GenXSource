
#import <UIKit/UIKit.h>
#import "BaseScreen.h"
#import "DashboardController.h"

@interface Dashboard : BaseScreen 
{
	IBOutlet UITableView * tableView;
}
@property (assign) UITableView * tableView;

- (IBAction) onBack:(id)sender;

- (void) onShow;


@end
