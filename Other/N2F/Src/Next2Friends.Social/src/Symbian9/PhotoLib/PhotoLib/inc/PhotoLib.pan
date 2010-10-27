/*
============================================================================
 Name        : PhotoLib.pan
 Author      : 
 Version     :
 Copyright   : Your copyright notice
 Description : Panic codes
============================================================================
*/

#ifndef __PHOTOLIB_PAN__
#define __PHOTOLIB_PAN__

/** PhotoLib panic codes */
enum TPhotoLibPanic
    {
    EPhotoLibNullPointer
    };

inline void Panic(TPhotoLibPanic aReason)
    {
	_LIT(applicationName,"Photo");
	User::Panic(applicationName, aReason);
    }

#endif  // __PHOTOLIB_PAN__


