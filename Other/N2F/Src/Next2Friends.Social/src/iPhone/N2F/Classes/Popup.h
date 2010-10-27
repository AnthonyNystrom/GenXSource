#import <UIKit/UIKit.h>

@interface Popup : UIView
{
	IBOutlet UILabel	* text;
	BOOL			isLocked;
}

@property(assign) UILabel * text;
@property(assign) BOOL isLocked;

- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event;


@end