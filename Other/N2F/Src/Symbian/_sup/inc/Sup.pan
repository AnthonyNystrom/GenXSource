/*
============================================================================
 Name        : Sup.pan
 Author      : Vitaly Vinogradov
 Version     : 1.0.0
 Copyright   : (c) Next2Friends, 2008
 Description : Application panic codes
============================================================================
*/

#ifndef __SUP_PAN__
#define __SUP_PAN__

/** Sup application panic codes */
enum TSupPanics
	{
	ESupUi = 1,
	// add further panics here
	EFileBrowserIvalidIndex,
	EImageCartInvalidIndex,
	EProviderNotInitialiazed,
	EAlreadyActive,
	ENoObserver
	};

inline void Panic(TSupPanics aReason)
	{
	_LIT(applicationName, "Snap up application");
	User::Panic(applicationName, aReason);
	}

#endif // __SUP_PAN__
