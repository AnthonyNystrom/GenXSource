/*
============================================================================
Name        : Supapputils.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Sup miscellaneous utils
============================================================================
*/

//#include <cntfield.h>
#include <cntdef.h> 
#include <cntitem.h>
#include <cntfldst.h>
#include <cntdb.h>
#include <coemain.h>
#include <apmrec.h>
#include <apgcli.h>
#include <imcvcodc.h>
#include "Supapputils.h"


HBufC8* CSupUtils::Base64EncodeLC(const TDesC8 &aSourceBuf)
{
	__LOGSTR_TOFILE("CSupUtils::Base64EncodeLC() begins");

	TImCodecB64 B64;
	//Using base64 the size is increased by 1/3
	HBufC8* buffer = HBufC8::NewL(aSourceBuf.Length() + aSourceBuf.Length()/3);
	B64.Initialise();
	TPtr8 buffPtr = buffer->Des();
	B64.Encode(aSourceBuf, buffPtr);

	__LOGSTR_TOFILE("CSupUtils::Base64EncodeLC() ends");

	return  buffer;
}


HBufC8* CSupUtils::Base64EncodeFileLC(const TDesC &aFileName)
{
	__LOGSTR_TOFILE1("CSupUtils::Base64EncodeFileLC() begins with aFileName == %S", &aFileName);

	TEntry fileEntry;
	HBufC8* retValue = NULL;

	// Get file description
	if(CCoeEnv::Static()->FsSession().Entry(aFileName, fileEntry) == KErrNone)
	{
		if(!fileEntry.IsDir())
		{
			// File size
			TInt fileSize = fileEntry.iSize;		
			// File buffer
			HBufC8* fileBuffer = NULL;

			// Install read file session
			RFs fsSession;			
			TInt err = fsSession.Connect();

			if (err == KErrNone)
			{
				// Try to open specified file
				RFile file;
				err = file.Open(fsSession, aFileName, EFileRead);

				// If file was open successfully
				if (err == KErrNone)
				{
					fileBuffer = HBufC8::NewLC(fileSize);

					TPtr8 bufPtr = fileBuffer->Des();

					file.Read(bufPtr);

					// If file content has been read successfully
					if (bufPtr.Length() == fileSize)
					{
						// Convert read buffer to Base64 string
						retValue = CSupUtils::Base64EncodeLC(*fileBuffer);
					}
				}

				// Free allocated memory
				if (fileBuffer)
				{
					delete fileBuffer;
					fileBuffer = NULL;
				}

				__LOGSTR_TOFILE("CSupUtils::Base64EncodeFileLC() just before file and fsSession destruction");

				// Close file
				file.Close();
			}
			
			// Close file session
			fsSession.Close();
		}
	}

	__LOGSTR_TOFILE("CSupUtils::Base64EncodeFileLC() ends");

	return retValue;
}

HBufC8* CSupUtils::Base64DecodeLC(const TDesC8 &aSourceBuf)
{
	__LOGSTR_TOFILE("CSupUtils::Base64DecodeLC() begins");

	TImCodecB64 B64;
	HBufC8* buffer = HBufC8::NewLC(aSourceBuf.Length());
	B64.Initialise();
	TPtr8 buffPtr = buffer->Des();
	B64.Decode(aSourceBuf, buffPtr);

	__LOGSTR_TOFILE("CSupUtils::Base64DecodeLC() ends");

	return  buffer;
}
char* CSupUtils::DescriptorToStringL(const TDesC& aDescriptor)
{
	TInt length = aDescriptor.Length();

	HBufC8* buffer = HBufC8::NewLC(length);
	buffer->Des().Copy(aDescriptor);

	char* str = new(ELeave) char[length + 1];
	Mem::Copy(str, buffer->Ptr(), length);
	str[length] = '\0';

	CleanupStack::PopAndDestroy(buffer);

	return str;
}

char* CSupUtils::DescriptorToString8L(const TDesC8& aDescriptor)
{
	TInt length = aDescriptor.Length();

	HBufC8* buffer = HBufC8::NewLC(length);
	buffer->Des().Copy(aDescriptor);

	char* str = new(ELeave) char[length + 1];
	Mem::Copy(str, buffer->Ptr(), length);
	str[length] = '\0';

	CleanupStack::PopAndDestroy(buffer);

	return str;
}

HBufC8* CSupUtils::StringToDescriptor8LC(const char* aString)
{
	TPtrC8 ptr(reinterpret_cast<const TUint8*>(aString));

	HBufC8* buffer = HBufC8::NewLC(ptr.Length());
	buffer->Des().Copy(ptr);

	return buffer;
}

HBufC* CSupUtils::StringToDescriptorLC(const char* aString)
{
	TPtrC8 ptr(reinterpret_cast<const TUint8*>(aString));

	HBufC* buffer = HBufC::NewLC(ptr.Length());
	buffer->Des().Copy(ptr);

	return buffer;
}

// End of file