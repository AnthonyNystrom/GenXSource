
#import "ImagePickerDelegate.h"
#import "N2FAppDelegate.h"
#import "QuestionImages.h"

@implementation ImagePickerDelegate

- (void)dismiss
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	int index = [delegate.window.subviews count];
	UIView * view = (UIView*)[delegate.window.subviews objectAtIndex:index-1];
	[view removeFromSuperview];
	
}

- (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker
{
	[self dismiss];
}

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingImage:(UIImage *)image editingInfo:(NSDictionary *)editingInfo
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	
	UIImage ** pRealView = ((QuestionImages*)delegate.questionImagesController.view).currentRealImage;
//	if(*pRealView != nil)
//	{
//		//[*pRealView release];
//	}
	*pRealView = image;
	
	UIImageView * imageView = ((QuestionImages*)delegate.questionImagesController.view).currentImageIndex;
	//imageView.contentMode = UIViewContentModeScaleAspectFill;
	imageView.image = image;
	
	[self dismiss];
}

@end
