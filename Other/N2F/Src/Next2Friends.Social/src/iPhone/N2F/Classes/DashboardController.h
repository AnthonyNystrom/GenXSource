#import <UIKit/UIKit.h>

@interface DashboardController : UIViewController  <UITableViewDelegate, UITableViewDataSource>
{
	NSMutableArray * items;
}

@property (assign) NSMutableArray *  items;

- (id)init;
- (void) dealloc;

@end
