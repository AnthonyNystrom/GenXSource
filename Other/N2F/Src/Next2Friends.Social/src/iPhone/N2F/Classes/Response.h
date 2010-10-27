
#import <UIKit/UIKit.h>
#import "BaseScreen.h"

@interface Response : BaseScreen
{
	IBOutlet UIImageView * image;
	IBOutlet UILabel		* a;
	IBOutlet UILabel		* b;
	IBOutlet UILabel	* comments;
}
@property (assign)  UIImageView * image;
@property (assign) UILabel		* a;
@property (assign) UILabel		* b;
@property (assign) UILabel	* comments;

- (IBAction) onBack:(id)sender;
- (IBAction) onWrite:(id)sender;
- (IBAction) onUpdate:(id)sender;

- (void) onShow;
- (void) updateComments;

- (void) networkThread:(id)object;

@end