
#import <UIKit/UIKit.h>
#import "BaseScreen.h"

@interface AskQuestion : BaseScreen
{
	IBOutlet UITabBarItem * itemQuestion;
	IBOutlet UITabBarItem * itemOptions;
	IBOutlet UITabBarItem * itemImages;
	IBOutlet UITabBar	 * tabBar;
	IBOutlet UITextView	* text;

}
@property (assign) UITabBarItem * itemQuestion;
@property (assign) UITabBarItem * itemOptions;
@property (assign) UITabBarItem * itemImages;
@property (assign) UITabBar * tabBar;
@property (assign) UITextView * text;

- (IBAction) onBack:(id)sender;
- (IBAction) done:(id)sender;

- (void) onShow;
- (void) onHide;

@end

