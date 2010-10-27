//
//  Preferences.m
//  iDicto
//
//  Created by Andrew Karpiuk on 6/16/08.
//  Copyright 2008 __MyCompanyName__. All rights reserved.
//

#import "Preferences.h"
#import <objc/runtime.h>
#import <Foundation/NSObjCRuntime.h>

@implementation Preferences

@synthesize mainMenuDifficulty;
@synthesize username;
@synthesize password;

- (void) loadIntForKey: (NSString*) variableName
{
	// firstly check if this variable available
	NSString * variableExistsName = [variableName stringByAppendingString: @"Exists"];
	NSInteger variableExists = [[NSUserDefaults standardUserDefaults] integerForKey:variableExistsName];
	
	NSInteger returnValue;
	if (variableExists) // if exists use from prefs
	{
		returnValue = [[NSUserDefaults standardUserDefaults] integerForKey: variableName];
	}else // if standart return var default state
	{
		NSNumber * number = [self valueForKey: variableName];
		returnValue = number.integerValue;
	}
	
	[self setValue:[NSNumber numberWithInteger:returnValue] forKey:variableName];
}

- (void) saveInt:(NSInteger)value forKey:(NSString*) variableName
{
	// firstly check if this variable available
	NSString * variableExistsName = [variableName stringByAppendingString: @"Exists"];
	[[NSUserDefaults standardUserDefaults] setInteger:1	forKey:variableExistsName];
	[[NSUserDefaults standardUserDefaults] setInteger:value	forKey:variableName];
}

- (void) loadStringForKey: (NSString*) variableName
{
	NSString * returnValue = [[NSUserDefaults standardUserDefaults] stringForKey: variableName];
	if(!returnValue)
	{
		returnValue = [self valueForKey:variableName];
	}
	[self setValue:returnValue forKey:variableName];
}

- (void) saveString:(NSString*)value forKey:(NSString*) variableName
{
	[[NSUserDefaults standardUserDefaults] setObject:value	 forKey:variableName];
}

- (void) loadFloatForKey: (NSString*) variableName
{
	// firstly check if this variable available
	NSString * variableExistsName = [variableName stringByAppendingString: @"Exists"];
	NSInteger variableExists = [[NSUserDefaults standardUserDefaults] integerForKey:variableExistsName];
	
	float returnValue;
	if (variableExists) // if exists use from prefs
	{
		returnValue = [[NSUserDefaults standardUserDefaults] floatForKey: variableName];
	}else // if standart return var default state
	{
		NSNumber * number = [self valueForKey: variableName];
		returnValue = number.floatValue;
	}

	[self setValue:[NSNumber numberWithFloat:returnValue] forKey:variableName];
}

- (void) saveFloat:(float)value forKey:(NSString*) variableName
{
	// firstly check if this variable available
	NSString * variableExistsName = [variableName stringByAppendingString: @"Exists"];
	[[NSUserDefaults standardUserDefaults] setInteger:1	forKey:variableExistsName];
	[[NSUserDefaults standardUserDefaults] setFloat:value forKey:variableName];
}

- (void) registerObserver: (NSString *)propertyName withType: (int) type
{
	[self addObserver:self
			forKeyPath:propertyName
			options:(NSKeyValueObservingOptionNew)
			context: (void*)(type)];	
}

- (id)init
{
	if (self = [super init])
	{
		// setting default values
				
		// implementing loading on start of application
		
		Class cls = object_getClass(self);
		unsigned int varsSize;
		Ivar * vars = class_copyIvarList(cls, &varsSize);
		for(int i = 0; i < varsSize; ++i)
		{
			Ivar var = vars[i];
			int type;
			
			const char * typeEncoding = ivar_getTypeEncoding(var);
			if (typeEncoding[0] == 'i')type = PreferencesInt;
			else if (typeEncoding[0] == 'f')type = PreferencesFloat;
			else if (!strcmp(typeEncoding, "@\"NSString\"")) type = PreferencesString;
			
			NSString * property = [NSString stringWithUTF8String:ivar_getName(var)];
			
			switch(type)
			{
				case PreferencesInt:
					[self loadIntForKey:property];
					break;
				case PreferencesFloat:
					[self loadFloatForKey:property];
					break;
				case PreferencesString:
					[self loadStringForKey:property];
					break;
			}
			// after loading of properties register observer
			[self registerObserver: property withType: type];	
		}
		free(vars);
	}
	return self;
}

- (void)observeValueForKeyPath:(NSString *)keyPath
					  ofObject:(id)object
                        change:(NSDictionary *)change
                       context:(void *)context
{
	int type = (int)context;
	id newKey = [change objectForKey:NSKeyValueChangeNewKey];
	
	switch(type)
	{
		case PreferencesInt:
			[self saveInt: ((NSNumber*)newKey).integerValue forKey: keyPath];
			break;
		case PreferencesFloat:
			[self saveFloat: ((NSNumber*)newKey).floatValue forKey: keyPath];
			break;
		case PreferencesString:
			[self saveString:(NSString*)newKey forKey:keyPath];
			break;
	};
}

@end

