
#import "QuestionOptions.h"
#import "N2FAppDelegate.h"
#import "Question.h"

@implementation QuestionOptions

@synthesize itemQuestion;
@synthesize itemOptions;
@synthesize itemImages;
@synthesize tabBar;
@synthesize picker;
@synthesize a;
@synthesize b;
@synthesize isPrivate;

- (IBAction) onBack:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.goController];
}

- (void) onShow
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	
	tabBar.selectedItem = itemOptions;
	tabBar.delegate = delegate.quTabDelegate;
	
	picker.dataSource = delegate.pickerController;
	picker.delegate = delegate.pickerController;
	
	isPrivate.tintColor = [UIColor colorWithRed: 0.42 green: 0.69 blue: 1.0 alpha: 1.0];
	isPrivate.selectedSegmentIndex = !delegate.currentQuestion.isPrivate;
	a.text = delegate.currentQuestion.responseA;
	b.text = delegate.currentQuestion.responseB;
	[picker selectRow: delegate.currentQuestion.responseType inComponent: 0 animated:false];
	[picker selectRow: delegate.currentQuestion.duration inComponent: 1 animated:false];
}

- (void) onHide
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	Question * q = delegate.currentQuestion;
	
	q.isPrivate = !isPrivate.selectedSegmentIndex;
	q.responseA = a.text;
	q.responseB = b.text;
	q.responseType = delegate.pickerController.responseType;
	q.duration = delegate.pickerController.duration;
}



- (IBAction) textEditEnder:(id) sender
{
	[a resignFirstResponder];
	[b resignFirstResponder];
}

@end




