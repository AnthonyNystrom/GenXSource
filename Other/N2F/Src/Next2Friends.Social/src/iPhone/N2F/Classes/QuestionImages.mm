
#import "QuestionImages.h"
#import "N2FAppDelegate.h"
#import "AskQuestion.h"
#import "QuestionOptions.h"
#import "Question.h"

@implementation QuestionImages

@synthesize itemQuestion;
@synthesize itemOptions;
@synthesize itemImages;
@synthesize tabBar;
@synthesize imageView1;
@synthesize imageView2;
@synthesize imageView3;
@synthesize currentImageIndex;
@synthesize realImage1;
@synthesize realImage2;
@synthesize realImage3;
@synthesize currentRealImage;



- (IBAction) onBack:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.goController];
}

- (void) onShow
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	
	tabBar.selectedItem = self->itemImages;
	tabBar.delegate = delegate.quTabDelegate;
	
	pickerDelegate = [ImagePickerDelegate new];
	
	imageView1.contentMode = UIViewContentModeScaleAspectFill;
	imageView2.contentMode = UIViewContentModeScaleAspectFill;
	imageView3.contentMode = UIViewContentModeScaleAspectFill;
	Question * q = delegate.currentQuestion;
	@try 
	{
		imageView1.image = (UIImage*)[q.photos objectAtIndex:0];
	}
	@catch (NSException * e) 
	{
		imageView1.image = [UIImage imageNamed:@"noimages.png"];
	}
	@try 
	{
		imageView2.image = (UIImage*)[q.photos objectAtIndex:1];
	}
	@catch (NSException * e) 
	{
		imageView2.image = [UIImage imageNamed:@"noimages.png"];
	}
	@try 
	{
		imageView3.image = (UIImage*)[q.photos objectAtIndex:2];
	}
	@catch (NSException * e) 
	{
		imageView3.image = [UIImage imageNamed:@"noimages.png"];
	}
}

- (void) onHide
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	Question * q = delegate.currentQuestion;
	[q.photos removeAllObjects];
	if(realImage1)
	{
		[q.photos addObject:realImage1];
	}
	if(realImage2)
	{
		[q.photos addObject:realImage2];
	}
	if(realImage3)
	{
		[q.photos addObject:realImage3];
	}
}

- (void) startImageController:(BOOL)fromCamera //else memory
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	pickerController = [[UIImagePickerController alloc] init];
	if(fromCamera)
		pickerController.sourceType = UIImagePickerControllerSourceTypeCamera;
	else
		pickerController.sourceType = UIImagePickerControllerSourceTypeSavedPhotosAlbum;
	pickerController.delegate = pickerDelegate;
	[delegate.window addSubview:pickerController.view];
}

- (IBAction) fromCamera1:(id)sender
{
	currentImageIndex = imageView1;
	currentRealImage = &realImage1;
	[self startImageController:true];
}

- (IBAction) fromFile1:(id)sender
{
	currentImageIndex = imageView1;
	currentRealImage = &realImage1;
	[self startImageController:false];
}

- (IBAction) fromCamera2:(id)sender
{
	currentImageIndex = imageView2;
	currentRealImage = &realImage2;
	[self startImageController:true];
}

- (IBAction) fromFile2:(id)sender
{
	currentImageIndex = imageView2;
	currentRealImage = &realImage2;
	[self startImageController:false];
}

- (IBAction) fromCamera3:(id)sender
{
	currentImageIndex = imageView3;
	currentRealImage = &realImage3;
	[self startImageController:true];
}

- (IBAction) fromFile3:(id)sender
{
	currentImageIndex = imageView3;
	currentRealImage = &realImage3;
	[self startImageController:false];
}

- (IBAction) delete1:(id)sender
{
	if(realImage1 != nil)
	{
		//[realImage1 release];
		realImage1 = nil;
	}
	imageView1.image = [UIImage imageNamed:@"noimages.png"];
}

- (IBAction) delete2:(id)sender
{
	if(realImage2 != nil)
	{
		//[realImage2 release];
		realImage2 = nil;
	}
	imageView2.image = [UIImage imageNamed:@"noimages.png"];

}

- (IBAction) delete3:(id)sender
{
	if(realImage3 != nil)
	{
		//[realImage3 release];
		realImage3 = nil;
	}
	imageView3.image = [UIImage imageNamed:@"noimages.png"];

}

- (IBAction) onSave:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	AskQuestion * a = (AskQuestion*)delegate.askQuestionController.view;
	QuestionOptions * o = (QuestionOptions*)delegate.questionOptionsController.view;
	Question * q = delegate.currentQuestion;
	
	q.sendNow = false;
	
	q.question = a.text.text;
	q.responseType = delegate.pickerController.responseType;
	q.duration = delegate.pickerController.duration;
	q.responseA = o.a.text;
	q.responseB = o.b.text;
	q.isPrivate = !o.isPrivate.selectedSegmentIndex;
	
	if(realImage1)
	{
		[q.photos addObject:realImage1];
		//[realImage1 release];
	}
	if(realImage2)
	{
		[q.photos addObject:realImage2];
		//[realImage2 release];
	}
	if(realImage3)
	{
		[q.photos addObject:realImage3];
		//[realImage3 release];
	}
	
	[delegate.draftQuestions addObject:q];
	[q release];
	[delegate changeScreen:delegate.mainMenuController];
}

- (IBAction) onSend:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	AskQuestion * a = (AskQuestion*)delegate.askQuestionController.view;
	QuestionOptions * o = (QuestionOptions*)delegate.questionOptionsController.view;
	Question * q = delegate.currentQuestion;
	
	q.sendNow = false;
	
	q.question = a.text.text;
	q.responseType = delegate.pickerController.responseType;
	q.duration = delegate.pickerController.duration;
	q.responseA = o.a.text;
	q.responseB = o.b.text;
	q.isPrivate = !o.isPrivate.selectedSegmentIndex;
	
	[q.photos removeAllObjects];
	if(realImage1)
	{
		[q.photos addObject:realImage1];
		//[realImage1 release];
	}
	if(realImage2)
	{
		[q.photos addObject:realImage2];
		//[realImage2 release];
	}
	if(realImage3)
	{
		[q.photos addObject:realImage3];
		//[realImage3 release];
	}
	
	//TODO: uncomment
	
//	if([q.photos count] == 0)
//	{
//		[delegate showPopup:@"Please add a photo" lock:false];
//		return;
//	}
	
	[delegate.unsentQuestions addObject:q];
	[q release];
	[delegate changeScreen:delegate.mainMenuController];
}

@end


