
#import "Options.h"
#import "N2FAppDelegate.h"
#import "Preferences.h"

@implementation Options

@synthesize login;
@synthesize password;

- (void) onShow
{
	Preferences * prefs = ((N2FAppDelegate*)([UIApplication sharedApplication].delegate)).prefs;

	[login setText:prefs.username];
	[password setText:prefs.password];
}

- (void) onHide
{
}

- (IBAction)onSave:(id)sender
{
	Preferences * prefs = ((N2FAppDelegate*)([UIApplication sharedApplication].delegate)).prefs;

	prefs.username = login.text;
	prefs.password = password.text;
	
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.mainMenuController];
}

- (IBAction)onCancel:(id)sender
{
	N2FAppDelegate * delegate = [UIApplication sharedApplication].delegate;
	[delegate changeScreen:delegate.mainMenuController];
}

@end
