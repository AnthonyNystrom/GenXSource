
#import <UIKit/UIKit.h>
#import "BaseScreen.h"

@interface Inbox : BaseScreen
{
	IBOutlet UITableView * tableView;
}
@property (assign) UITableView * tableView;

- (IBAction) onBack:(id)sender;

- (void) onShow;

@end