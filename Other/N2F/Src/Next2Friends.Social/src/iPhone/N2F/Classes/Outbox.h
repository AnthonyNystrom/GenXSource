
#import <UIKit/UIKit.h>
#import "BaseScreen.h"

@interface Outbox : BaseScreen
{
	IBOutlet UITableView * tableView;
}
@property (assign) UITableView * tableView;

- (IBAction) onBack:(id)sender;

- (void) onShow;

@end