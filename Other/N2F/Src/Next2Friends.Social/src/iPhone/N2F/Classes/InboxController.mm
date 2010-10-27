
#import "InboxController.h"
#import "N2FAppDelegate.h"
#import "InboxCell.h"
#import "AskQuestionStruct.h"

@implementation InboxController

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section 
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	return [delegate.inboxQuestions count];
}

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath 
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	
	InboxCell * cell = [InboxCell new];
	[cell setup];
	AskQuestionStruct * q = (AskQuestionStruct*)[delegate.inboxQuestions objectAtIndex:indexPath.row];
	cell.id = q->id;
	cell.text = q->question;
	
	return cell;
}


@end