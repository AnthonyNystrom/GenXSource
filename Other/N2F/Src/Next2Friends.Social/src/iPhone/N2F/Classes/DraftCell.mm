
#import "DraftCell.h"
#import "Question.h"
#import  "N2FAppDelegate.h"

@implementation DraftCell

@synthesize q;

- (void) onDetails:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	delegate.currentQuestion = q;
	[delegate changeScreen:delegate.askQuestionController];
	
}

- (void) setup
{
	self.accessoryType = UITableViewCellAccessoryDetailDisclosureButton;
	self.font = [UIFont systemFontOfSize: 12];
	self.target = self;
	self.accessoryAction = @selector(onDetails:);
}

@end
