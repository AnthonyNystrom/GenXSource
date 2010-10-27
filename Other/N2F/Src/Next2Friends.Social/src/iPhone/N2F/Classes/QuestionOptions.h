
#import <UIKit/UIKit.h>
#import "BaseScreen.h"



@interface QuestionOptions : BaseScreen
{
	IBOutlet UITabBarItem	* itemQuestion;
	IBOutlet UITabBarItem	* itemOptions;
	IBOutlet UITabBarItem	* itemImages;
	IBOutlet UITabBar	* tabBar;
	IBOutlet UIPickerView	* picker;
	IBOutlet UITextField	* a;
	IBOutlet UITextField	* b;
	IBOutlet UISegmentedControl * isPrivate;
	
	
}
@property (assign) UITabBarItem * itemQuestion;
@property (assign) UITabBarItem * itemOptions;
@property (assign) UITabBarItem * itemImages;
@property (assign) UIPickerView   * picker;
@property (assign) UITabBar * tabBar;
@property (assign) UITextField * a;
@property (assign) UITextField * b;
@property (assign) UISegmentedControl * isPrivate;

- (IBAction) onBack:(id)sender;
- (IBAction) textEditEnder:(id) sender;

- (void) onShow;
- (void) onHide;

@end

