//
//  Login.h
//  N2F
//
//  Created by Dizz on 8/4/08.
//  Copyright 2008 1. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "BaseScreen.h"


@interface Login : BaseScreen 
{
	IBOutlet UITextField	* login;
	IBOutlet UITextField	* password;
}
@property (retain) UITextField * login;
@property (retain) UITextField * password;

- (IBAction)onLogin:(id)sender;

- (void) onShow;
- (void) onHide;
- (void) loginThread:(id)object;

@end


