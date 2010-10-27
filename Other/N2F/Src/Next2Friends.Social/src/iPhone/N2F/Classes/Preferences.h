//
//  Preferences.h
//  iDicto
//
//  Created by Vitaliy Borodovsky on 6/16/08.
//  Copyright 2008 Dava Consulting. All rights reserved.
//

#import <UIKit/UIKit.h>

enum
{
	PreferencesInt = 0,
	PreferencesFloat,
	PreferencesString
};

@interface Preferences : NSObject
{
	NSInteger	mainMenuDifficulty;
	NSString	* username;
	NSString	* password;
};

@property (nonatomic, assign) NSInteger	mainMenuDifficulty;
@property (nonatomic, copy) NSString	* username;
@property (nonatomic, copy) NSString	* password;

- (void) registerObserver: (NSString *)propertyName withType: (int) type;

@end

