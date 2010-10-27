//
//  Login.m
//  N2F
//
//  Created by Dizz on 8/4/08.
//  Copyright 2008 1. All rights reserved.
//

#import "Login.h"
#import "N2FAppDelegate.h"
#import "Preferences.h"
#import "MemberService.h"

@implementation Login

@synthesize login;
@synthesize password;

- (IBAction)onLogin:(id)sender
{
	[login resignFirstResponder];
	[password resignFirstResponder];
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	
	//[delegate changeScreen:delegate.mainMenuController];
	[delegate showPopup:@"Please wait" lock:true];
	
	[NSThread detachNewThreadSelector:@selector(loginThread:) toTarget:self withObject:nil];
}
	
- (void) loginThread:(id)object
{
	NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
	
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	
	MemberService * service = [[MemberService alloc] init];
	BOOL res = [service CheckUserExists:login.text password:password.text];
	[service release];
	[delegate hidePopup];
	
	if(res)
	{
		[delegate runBackNetwork];
		[delegate changeScreen:delegate.mainMenuController];
	}
	else
	{
		[delegate showPopup:@"Wrong login/password" lock:false];
	}
	
	[pool release];
	
}

- (void) onShow
{
	Preferences * prefs = ((N2FAppDelegate*)([UIApplication sharedApplication].delegate)).prefs;
	[login setText:prefs.username];
	[password setText:prefs.password];
}

- (void) onHide
{
	Preferences * prefs = ((N2FAppDelegate*)([UIApplication sharedApplication].delegate)).prefs;
	prefs.username = login.text;
	prefs.password = password.text;
}

@end
