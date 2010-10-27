
#import <UIKit/UIKit.h>
#import "BaseScreen.h"
#import "ImagePickerDelegate.h"

@interface QuestionImages : BaseScreen
{
	IBOutlet UITabBarItem * itemQuestion;
	IBOutlet UITabBarItem * itemOptions;
	IBOutlet UITabBarItem * itemImages;
	IBOutlet UITabBar	 * tabBar;
	
	IBOutlet UIImageView  * imageView1;
	IBOutlet UIImageView  * imageView2;
	IBOutlet UIImageView  * imageView3;
	UIImageView		 * currentImageIndex;
	
	UIImage			* realImage1;
	UIImage			* realImage2;
	UIImage			* realImage3;
	UIImage			** currentRealImage;
	
	
	ImagePickerDelegate	  * pickerDelegate;
	UIImagePickerController * pickerController;
	
}
@property (assign) UITabBarItem * itemQuestion;
@property (assign) UITabBarItem * itemOptions;
@property (assign) UITabBarItem * itemImages;
@property (assign) UITabBar * tabBar;
@property (assign) UIImageView * imageView1;
@property (assign) UIImageView * imageView2;
@property (assign) UIImageView * imageView3;
@property (assign) UIImageView * currentImageIndex;
@property (assign) UIImage * realImage1;
@property (assign) UIImage * realImage2;
@property (assign) UIImage * realImage3;
@property (assign) UIImage ** currentRealImage;

- (IBAction) onBack:(id)sender;
- (IBAction) fromCamera1:(id)sender;
- (IBAction) fromFile1:(id)sender;
- (IBAction) fromCamera2:(id)sender;
- (IBAction) fromFile2:(id)sender;
- (IBAction) fromCamera3:(id)sender;
- (IBAction) fromFile3:(id)sender;
- (IBAction) delete1:(id)sender;
- (IBAction) delete2:(id)sender;
- (IBAction) delete3:(id)sender;
- (IBAction) onSave:(id)sender;
- (IBAction) onSend:(id)sender;

- (void) onShow;
- (void) onHide;


@end