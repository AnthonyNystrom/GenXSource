
#import "Popup.h"

@implementation Popup

@synthesize text;
@synthesize isLocked;

- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
{
	if(!isLocked)
	{
		[self removeFromSuperview];
	}
}

@end
