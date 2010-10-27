/*
============================================================================
Name        : Supapputils.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Sup miscellaneous utils
============================================================================
*/

#ifndef __SUPAPPUTILS_H__
#define __SUPAPPUTILS_H__

#include <e32base.h>
#include <coecntrl.h>
#include <w32std.h>
#include <e32std.h>
#include <cntdef.h>
#include <cntdb.h> 
#include <ImageConversion.h>
#include "common.h"

// FORWARD DECLARATION

class CSupUtils: public CBase
{
public:
	static CSupUtils* NewL();

	static CSupUtils* NewLC();

	virtual ~CSupUtils();

	// Encode buffer to Base64 string
	static HBufC8* Base64EncodeLC(const TDesC8 & aSourceBuf);

	// Encode specified file to Base64 string
	static HBufC8* Base64EncodeFileLC(const TDesC &aFileName);

	// Decode buffer from Base64 string
	static HBufC8* Base64DecodeLC(const TDesC8 & aSourceBuf);

	// Convert const TDesC to char*,
	// allocates memory dynamically, so don't forget to free it later
	static char* DescriptorToStringL(const TDesC& aDescriptor);

	// Convert const TDesC8 to char*,
	// allocates memory dynamically, so don't forget to free it later
	static char* DescriptorToString8L(const TDesC8& aDescriptor);

	// Convert char* to HBufC8*
	static HBufC8* StringToDescriptor8LC(const char* aString);

	// Convert char to HBufC*
	static HBufC* StringToDescriptorLC(const char* aString);


private:

	void ConstructL();

	CSupUtils();

private:
	/**
	* Determines the number of ticks for 0:00.00 01/01/1970.
	*/
	//static TInt32 initialTicks = 621355968000000000L;
};

#endif // __SUPAPPUTILS_H__

// End of File