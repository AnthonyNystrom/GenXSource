#import <UIKit/UIKit.h>

@class Question;
@interface DraftCell : UITableViewCell
{
	Question * q;
}

@property (assign) Question * q;

- (void) onDetails:(id)sender;
- (void) setup;

@end
