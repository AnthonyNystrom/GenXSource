#import <UIKit/UIKit.h>

@interface DashboardCell : UITableViewCell
{
	NSString * showDate;
	NSString * date;
	NSString * content;
}
@property (assign) NSString * date;
@property (assign) NSString * content;
@property (assign) NSString * showDate;

- (void) onDetails:(id)sender;
- (void) setup;
- (void) convertDate;

@end