//
//  N2FAppDelegate.h
//  N2F
//
//  Created by Dizz on 8/4/08.
//  Copyright 1 2008. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "Login.h"
#import "MainMenu.h"
#import "DashboardController.h"
#import "DraftsController.h"
#import "QuestionTabDelegate.h"
#import "PickerController.h"
#import "InboxController.h"

@class Preferences;
@class Popup;
@class Question;
@class AskResponse;

@interface N2FAppDelegate : NSObject <UIApplicationDelegate> 
{
	IBOutlet UIWindow * window;
	
	UIViewController	* loginController;
	UIViewController	* mainMenuController;
	UIViewController* popupController;
	UIViewController* optionsController;
	UIViewController* dashboardController;
	UIViewController* dashboardMessageController;
	UIViewController* goController;
	UIViewController* askQuestionController;
	UIViewController* questionOptionsController;
	UIViewController* questionImagesController;
	UIViewController* draftsController;
	UIViewController* outboxController;
	UIViewController* inboxController;
	UIViewController* responseController;
	UIViewController* commentController;
	
	DashboardController	* dashController;
	DraftsController		* draController;
	InboxController		* inbController;
	
	PickerController		* pickerController;
	QuestionTabDelegate * quTabDelegate;
	Question			* currentQuestion;
	int				currentResponse;
	NSMutableArray		* unsentQuestions;
	NSMutableArray		* draftQuestions;
	NSMutableArray		* inboxQuestions;
	
	Preferences		* prefs;
	
	BOOL				getSend;
	
	NSString			* commentsText;
}

@property (nonatomic, retain) UIWindow		* window;
@property (nonatomic, assign) UIViewController	* loginController;
@property (nonatomic, assign) UIViewController	* mainMenuController;
@property (nonatomic, assign) UIViewController	* popupController;
@property (nonatomic, assign) UIViewController	* optionsController;
@property (nonatomic, assign) UIViewController	* dashboardController;
@property (nonatomic, assign) UIViewController	* dashboardMessageController;
@property (nonatomic, assign) UIViewController	* goController;
@property (nonatomic, assign) UIViewController	* askQuestionController;
@property (nonatomic, assign) UIViewController	* questionOptionsController;
@property (nonatomic, assign) UIViewController	* questionImagesController;
@property (nonatomic, assign) UIViewController	* draftsController;
@property (nonatomic, assign) UIViewController	* outboxController;
@property (nonatomic, assign) UIViewController	* inboxController;
@property (nonatomic, assign) UIViewController	* responseController;
@property (nonatomic, assign) UIViewController	* commentController;

@property (readonly, assign) Preferences		* prefs;
@property (assign) DashboardController		* dashController;
@property (assign) DraftsController			* draController;
@property (assign) InboxController			* inbController;
@property (assign) PickerController			* pickerController;
@property (assign) QuestionTabDelegate		* quTabDelegate;
@property (assign) Question				* currentQuestion;
@property (assign) int						currentResponse;
@property (readonly, assign) NSMutableArray	* unsentQuestions;
@property (readonly, assign) NSMutableArray	* draftQuestions;
@property (readonly, assign) NSMutableArray	* inboxQuestions;
@property (assign) BOOL					getSend;

@property (assign) NSString					* commentsText;

- (void) runBackNetwork;
- (void) networkThread:(id)object;

- (void) changeScreen:(UIViewController*)newScreen;
- (void) showPopup:(NSString*)text lock:(BOOL)lock;
- (void) hidePopup;

@end

