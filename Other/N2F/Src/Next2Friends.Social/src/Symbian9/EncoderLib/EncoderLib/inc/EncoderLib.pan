/*
============================================================================
 Name        : EncoderLib.pan
 Author      : 
 Version     :
 Copyright   : Your copyright notice
 Description : Panic codes
============================================================================
*/

#ifndef __ENCODERLIB_PAN__
#define __ENCODERLIB_PAN__

/** EncoderLib panic codes */
enum TEncoderLibPanic
    {
    EEncoderLibNullPointer
    };

inline void Panic(TEncoderLibPanic aReason)
    {
	_LIT(applicationName,"Encoder");
	User::Panic(applicationName, aReason);
    }

#endif  // __ENCODERLIB_PAN__


