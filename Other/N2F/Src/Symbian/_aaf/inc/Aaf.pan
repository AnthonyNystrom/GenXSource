/*
============================================================================
 Name        : Aaf.pan
 Author      : Vitaly Vinogradov
 Version     :
 Copyright   : (c) Next2Friends, 2008
 Description : Application panic codes
============================================================================
*/

#ifndef __AAF_PAN__
#define __AAF_PAN__

/** Aaf application panic codes */
enum TAafPanics
	{
	EAafUi = 1,
	// add further panics here
	EFileBrowserIvalidIndex,
	EImageCartInvalidIndex,
	EProviderNotInitialiazed,
	EAlreadyActive,
	EInvalidString,
	EInvalidArgument,
	ENoObserver
	};

inline void Panic(TAafPanics aReason)
	{
	_LIT(applicationName,"Aaf");
	User::Panic(applicationName, aReason);
	}

#endif // __AAF_PAN__
