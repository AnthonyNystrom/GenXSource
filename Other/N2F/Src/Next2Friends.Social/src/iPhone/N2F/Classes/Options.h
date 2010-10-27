
#import <UIKit/UIKit.h>
#import "BaseScreen.h"


@interface Options : BaseScreen 
{
	IBOutlet UITextField	* login;
	IBOutlet UITextField	* password;
}
@property (retain) UITextField * login;
@property (retain) UITextField * password;

- (IBAction)onSave:(id)sender;
- (IBAction)onCancel:(id)sender;

- (void) onShow;
- (void) onHide;

@end