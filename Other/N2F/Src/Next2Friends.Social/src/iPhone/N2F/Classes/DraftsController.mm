
#import "DraftsController.h"
#import "N2FAppDelegate.h"
#import "DraftCell.h"
#import "Question.h"

@implementation DraftsController

- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section 
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	return [delegate.draftQuestions count];
}                                                                                                                                                                                                                                                                                                                                                                  

- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath 
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;

	DraftCell * cell = [DraftCell new];
	[cell setup];
	Question * q = (Question*)[delegate.draftQuestions objectAtIndex:indexPath.row];
	cell.q = q;
	cell.text = q.question;
	
	return cell; 
	
}


@end
