#import <UIKit/UIKit.h>

@interface PickerController: NSObject <UIPickerViewDataSource, UIPickerViewDelegate>
{
	NSInteger responseType;
	NSInteger duration;
}

@property (assign) NSInteger responseType;
@property (assign) NSInteger duration;

@end
