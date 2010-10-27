
#import "Go.h"
#import "N2FAppDelegate.h"
#import "Question.h"

@implementation Go

- (IBAction) onBack:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.mainMenuController];
}

- (IBAction) onQuestion:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	
	delegate.currentQuestion = [Question new];
	[delegate changeScreen:delegate.askQuestionController];

}

@end
