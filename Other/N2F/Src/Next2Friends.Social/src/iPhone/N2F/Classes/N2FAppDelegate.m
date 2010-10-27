//
//  N2FAppDelegate.m
//  N2F
//
//  Created by Dizz on 8/4/08.
//  Copyright 1 2008. All rights reserved.
//

#import "N2FAppDelegate.h"
#import "Preferences.h"
#import "BaseScreen.h"
#import "Popup.h"
#import "DashboardService.h"
#import "DashboardController.h"
#import "DashboardCell.h"
#import "DashboardNewFriend.h"
#import "DashboardWallComment.h"
#import "DashboardPhoto.h"
#import "DashboardVideo.h"
#import "QuestionTabDelegate.h"
#import "AskService.h"
#import "Question.h"
#import "AskQuestionConfirm.h"
#import "AskQuestionStruct.h"


@implementation N2FAppDelegate

@synthesize window;
@synthesize loginController;
@synthesize mainMenuController;
@synthesize prefs;
@synthesize popupController;
@synthesize optionsController;
@synthesize dashboardController;
@synthesize dashboardMessageController;
@synthesize dashController;
@synthesize goController;
@synthesize askQuestionController;
@synthesize questionOptionsController;
@synthesize quTabDelegate;
@synthesize questionImagesController;
@synthesize pickerController;
@synthesize currentQuestion;
@synthesize unsentQuestions;
@synthesize inboxQuestions;
@synthesize draftQuestions;
@synthesize draftsController;
@synthesize draController;
@synthesize inbController;
@synthesize outboxController;
@synthesize inboxController;
@synthesize getSend;
@synthesize responseController;
@synthesize currentResponse;
@synthesize commentController;
@synthesize commentsText;

- (void)applicationDidFinishLaunching:(UIApplication *)application 
{
	prefs = [Preferences new];
	dashController = [DashboardController new];
	draController = [DraftsController new];
	inbController = [InboxController new];
	pickerController = [PickerController new];
	quTabDelegate = [QuestionTabDelegate new];
	unsentQuestions = [NSMutableArray new];
	draftQuestions = [NSMutableArray new];
	inboxQuestions = [NSMutableArray new];
	
	
	loginController = [[UIViewController alloc] initWithNibName:@"LoginView" bundle:[NSBundle mainBundle]];
	mainMenuController = [[UIViewController alloc] initWithNibName:@"MainMenuView" bundle:[NSBundle mainBundle]];
	popupController = [[UIViewController alloc] initWithNibName:@"Popup" bundle:[NSBundle mainBundle]];
	optionsController = [[UIViewController alloc] initWithNibName:@"OptionsView" bundle:[NSBundle mainBundle]];
	dashboardController = [[UIViewController alloc] initWithNibName:@"DashboardView" bundle:[NSBundle mainBundle]];
	dashboardMessageController = [[UIViewController alloc] initWithNibName:@"DasboardMessageView" bundle:[NSBundle mainBundle]];
	goController = [[UIViewController alloc] initWithNibName:@"GoView" bundle:[NSBundle mainBundle]];
	askQuestionController = [[UIViewController alloc] initWithNibName:@"AskQuestionView" bundle:[NSBundle mainBundle]];
	questionOptionsController = [[UIViewController alloc] initWithNibName:@"QuestionOptionsView" bundle:[NSBundle mainBundle]];
	questionImagesController = [[UIViewController alloc] initWithNibName:@"QuestionImagesView" bundle:[NSBundle mainBundle]];
	draftsController = [[UIViewController alloc] initWithNibName:@"DraftsView" bundle:[NSBundle mainBundle]];
	outboxController = [[UIViewController alloc] initWithNibName:@"OutboxView" bundle:[NSBundle mainBundle]];
	inboxController = [[UIViewController alloc] initWithNibName:@"Inbox" bundle:[NSBundle mainBundle]];
	responseController = [[UIViewController alloc] initWithNibName:@"Response" bundle:[NSBundle mainBundle]];
	commentController = [[UIViewController alloc] initWithNibName:@"Comment" bundle:[NSBundle mainBundle]];
 

	[self changeScreen:loginController];
	
	[window makeKeyAndVisible];
}


- (void)dealloc 
{
	[inboxQuestions release];
	[draftQuestions release];
	[unsentQuestions release];
	[quTabDelegate release];
	[window release];
	[super dealloc];
}

- (void) runBackNetwork
{
	[NSThread detachNewThreadSelector:@selector(networkThread:) toTarget:self withObject:nil];
}


- (void) updateDashboard
{
	
	
	DashboardService * dashService = [[DashboardService alloc] init];
	NSArray * newFriends = [dashService GetNewFriends:prefs.username:prefs.password];
	
	for(int i = 0; i < [newFriends count]; ++i)
	{
		DashboardNewFriend * newFriend = (DashboardNewFriend*)[newFriends objectAtIndex:i];
		DashboardCell * cell = [DashboardCell new];
		[cell setup];
		cell.date = newFriend->datetime;
		[cell convertDate];
		
		//header
		NSMutableString * text = [NSMutableString new];
		[text appendString:newFriend->nickname1];
		[text appendString:@" on "];
		[text appendString:cell.showDate];
		cell.text = text;
		[text release];
		
		//content
		text = [NSMutableString new];
		[text appendString:newFriend->nickname1];
		[text appendString:@" and "];
		[text appendString:newFriend->nickname2];
		[text appendString:@" are now friends!"];
		cell.content = text;
		
		[self.dashController.items addObject:cell];
		[cell release];
	}
	[newFriends release];
	
	NSArray * wallComments =  [dashService GetWallComments:prefs.username:prefs.password];
	for(int i = 0; i < [wallComments count]; ++i)
	{
		DashboardWallComment * wallComment = (DashboardWallComment*)[wallComments objectAtIndex:i];
		DashboardCell * cell = [DashboardCell new];
		[cell setup];
		cell.date = wallComment->datetime;
		[cell convertDate];
		
		//header
		NSMutableString * text = [NSMutableString new];
		[text appendString:wallComment->nickname1];
		[text appendString:@" on "];
		[text appendString:cell.showDate];
		cell.text = text;
		[text release];
		
		//content
		text = [NSMutableString new];
		[text appendString:wallComment->nickname1];
		[text appendString:@" has written on "];
		[text appendString:wallComment->nickname2];
		[text appendString:@"'s wall: \""];
		[text appendString:wallComment->text];
		[text appendString:@"\""];
		cell.content = text;
		
		[self.dashController.items addObject:cell];
		[cell release];
	}
	[wallComments release];
	
	NSArray * photos =  [dashService GetPhotos:prefs.username:prefs.password];
	for(int i = 0; i < [photos count]; ++i)
	{
		DashboardPhoto * photo = (DashboardPhoto*)[photos objectAtIndex:i];
		DashboardCell * cell = [DashboardCell new];
		[cell setup];
		cell.date = photo->datetime;
		[cell convertDate];
		
		//header
		NSMutableString * text = [NSMutableString new];
		[text appendString:photo->nickname];
		[text appendString:@" on "];
		[text appendString:cell.showDate];
		cell.text = text;
		[text release];
		
		//content
		text = [NSMutableString new];
		[text appendString:photo->nickname];
		[text appendString:@" has added a new photo"];
		cell.content = text;
		
		[self.dashController.items addObject:cell];
		[cell release];
	}
	[photos release];
	
	NSArray * videos =  [dashService GetVideos:prefs.username:prefs.password];
	for(int i = 0; i < [videos count]; ++i)
	{
		DashboardVideo * video = (DashboardVideo*)[videos objectAtIndex:i];
		DashboardCell * cell = [DashboardCell new];
		[cell setup];
		cell.date = video->datetime;
		[cell convertDate];
		
		//header
		NSMutableString * text = [NSMutableString new];
		[text appendString:video->nickname];
		[text appendString:@" on "];
		[text appendString:cell.showDate];
		cell.text = text;
		[text release];
		
		//content
		text = [NSMutableString new];
		[text appendString:video->nickname];
		[text appendString:@" has added a new video"];
		cell.content = text;
		
		[self.dashController.items addObject:cell];
		[cell release];
	}
	[videos release];
	
	[dashService release];
	[(MainMenu*)mainMenuController.view updateTitles];
	
}

- (void) updateInbox
{
	AskService * service = [AskService new];
	int * ids;
	int size = [service GetQuestionIDs:prefs.username :prefs.password :&ids];
	for(int i = 0; i < size; ++i)
	{
		AskQuestionStruct * s = [service GetQuestion:prefs.username :prefs.password :ids[i]];
		[inboxQuestions addObject:s];
	}
	[service release];
	[(MainMenu*)mainMenuController.view updateTitles];
}

- (void) networkThread:(id)object
{
	NSAutoreleasePool *pool = [NSAutoreleasePool new];
	
	@try
	{
		[self updateDashboard];
	}
	@catch (NSException * ex) 
	{
		NSLog(@"Exception in updateDashboard:%s %s", ex.name, ex.reason);
	}
	@try
	{
		[self updateInbox];
	}
	@catch (NSException * ex) 
	{
		NSLog(@"Exception in updateInbox:%s %s", ex.name, ex.reason);
	}
	while(true)
	{
		AskService * service = [AskService new];
		
		int unsentOutbox = [unsentQuestions count];
		if(unsentOutbox > 0)
		{
			Question * q = [unsentQuestions objectAtIndex:0];
			NSArray * customResponses = [[NSArray alloc] initWithObjects: q.responseA, q.responseB, 0];
			AskQuestionConfirm * confirm = [service SubmitQuestion:prefs.username :prefs.password
			:q.question :[q.photos count] :q.responseType :customResponses :q.duration :q.isPrivate];
			[customResponses release];
			
			NSString * questionId = confirm->askquestionid;
			for(int i = 0; i < [q.photos count]; ++i)
			{
				NSData * data = UIImageJPEGRepresentation([q.photos objectAtIndex:i], 1.0f);
				int size = [data length];
				const void * buf = [data bytes];
				[service AttachPhoto:prefs.username :prefs.password :questionId :i+1 :(char*)buf :size];
			}
			
			[service CompleteQuestion:prefs.username :prefs.password :questionId];
			[unsentQuestions removeObjectAtIndex:0];
		}
		
		[service release];
		[NSThread sleepForTimeInterval:0.1f];
	}
	[(MainMenu*)mainMenuController.view updateTitles];
	[pool release];
}

- (void)changeScreen:(UIViewController*)newScreen
{
	NSInteger size = [window.subviews count];
	if(size)
	{
		BaseScreen * oldScreen = [window.subviews objectAtIndex:0];
		[oldScreen onHide];
		[oldScreen removeFromSuperview];
	}
	
	BaseScreen *screen = (BaseScreen*)newScreen.view;
	[window addSubview:screen];
	[screen onShow];
	
}

- (void) showPopup:(NSString*)text lock:(BOOL)lock
{
	Popup * popup = (Popup*)popupController.view;
	[popup.text setText:text];
	popup.isLocked = lock;
	[window addSubview:popup];
	
}


- (void) hidePopup
{
	[popupController.view removeFromSuperview];
}


@end
