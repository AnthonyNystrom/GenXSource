#import <UIKit/UIKit.h>

@class AskQuestionStruct;
@interface InboxCell : UITableViewCell
{
	int id;
}

@property (assign) int id;

- (void) onDetails:(id)sender;
- (void) setup;



@end