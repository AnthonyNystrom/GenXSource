
#import "AskQuestion.h"
#import "N2FAppDelegate.h"
#import "Question.h"

@implementation AskQuestion

@synthesize itemQuestion;
@synthesize itemOptions;
@synthesize itemImages;
@synthesize tabBar;
@synthesize text;

- (IBAction) onBack:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.goController];
}

- (IBAction) done:(id)sender
{
	[text resignFirstResponder];
}

- (void) onShow
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;

	tabBar.selectedItem = self->itemQuestion;
	tabBar.delegate = delegate.quTabDelegate;
	
	text.text = delegate.currentQuestion.question;
}

- (void) onHide
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	Question * q = delegate.currentQuestion;
	
	q.question = text.text;
}

@end


