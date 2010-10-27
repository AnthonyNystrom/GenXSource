//
//  MainMenu.h
//  N2F
//
//  Created by Dizz on 8/5/08.
//  Copyright 2008 1. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "BaseScreen.h"

@interface MainMenu : BaseScreen 
{
	IBOutlet UIButton * inbox;
	IBOutlet UIButton * dashboard;
	IBOutlet UIButton * outbox;
	IBOutlet UIButton * drafts;
}

@property (assign) UIButton * inbox;
@property (assign) UIButton * outbox;
@property (assign) UIButton * drafts;
@property (assign) UIButton * dashboard;

- (IBAction) onOptions:(id)sender;
- (IBAction) onDashboard:(id)sender;
- (IBAction) onGo:(id)sender;
- (IBAction) onDrafts:(id)sender;
- (IBAction) onOutbox:(id)sender;
- (IBAction) onInbox:(id)sender;
- (IBAction) onUpdate:(id)sender;

- (void) onShow;
- (void) updateTitles;

@end
