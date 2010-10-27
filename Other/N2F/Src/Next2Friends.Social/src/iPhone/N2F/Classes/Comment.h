
#import <UIKit/UIKit.h>
#import "BaseScreen.h"

@interface WriteComment : BaseScreen
{
	IBOutlet UITextView	* comment;
}
@property (assign) UITextView	* comment;

- (IBAction) onBack:(id)sender;
- (IBAction) onSend:(id)sender;

- (void) networkThread:(id)object;


@end