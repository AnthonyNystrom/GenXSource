/*
============================================================================
 Name        : CameraLib.pan
 Author      : 
 Version     :
 Copyright   : Your copyright notice
 Description : Panic codes
============================================================================
*/

#ifndef __CAMERALIB_PAN__
#define __CAMERALIB_PAN__

/** CameraLib panic codes */
enum TCameraLibPanic
    {
    ECameraLibNullPointer
    };

inline void Panic(TCameraLibPanic aReason)
    {
	_LIT(applicationName,"Camera");
	User::Panic(applicationName, aReason);
    }

#endif  // __CAMERALIB_PAN__


