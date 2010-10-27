
#import "MainMenu.h"
#import "N2FAppDelegate.h"
#import "Question.h"

@implementation MainMenu

@synthesize inbox;
@synthesize outbox;
@synthesize drafts;
@synthesize dashboard;

- (IBAction) onOptions:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.optionsController];
}

- (IBAction) onDashboard:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.dashboardController];
}

- (IBAction) onGo:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.goController];

}

- (IBAction) onDrafts:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.draftsController];
}

- (IBAction) onOutbox:(id)sender
{
	//N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	//[delegate changeScreen:delegate.outboxController];
}

- (IBAction) onInbox:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.inboxController];
}

- (IBAction) onUpdate:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate runBackNetwork];
}

- (void) onShow
{
	[self updateTitles];
}

- (void) updateTitles
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	
	int count = [delegate.draftQuestions count];
	NSString * title = [NSString stringWithFormat:@"Drafts (%d)", count];
	[drafts setTitle: title forState:UIControlStateNormal];
	[drafts setTitle: title forState:UIControlStateHighlighted];
	
	count = [delegate.unsentQuestions count];
	title = [NSString stringWithFormat:@"Outbox (%d)", count];
	[outbox setTitle: title forState:UIControlStateNormal];
	[outbox setTitle: title forState:UIControlStateHighlighted];
	
	count = [delegate.dashController.items count];
	title = [NSString stringWithFormat:@"Dashboard (%d)", count];
	[dashboard setTitle: title forState:UIControlStateNormal];
	[dashboard setTitle: title forState:UIControlStateHighlighted];
	
	count = [delegate.inboxQuestions count];
	title = [NSString stringWithFormat:@"Inbox (%d)", count];
	[inbox setTitle: title forState:UIControlStateNormal];
	[inbox setTitle: title forState:UIControlStateHighlighted];		 
}

@end
