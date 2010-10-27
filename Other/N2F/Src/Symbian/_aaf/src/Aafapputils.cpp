/*
============================================================================
Name        : Aafapputils.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Aaf miscellaneous utils
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
#include "Aafapputils.h"
#include "aaf.pan"


/*
CAafImageReader::CAafImageReader(MRequestObserver* aNotify)
:CActive(0),iNotify(aNotify)
{
}

CAafImageReader::~CAafImageReader()
{
Cancel();
delete iFrame;
}


void CAafImageReader::ConstructL(const TDesC& aFileName)
{
CActiveScheduler::Add(this);

iImageName.Copy(aFileName);

TBuf8<255> ImageType;
GetFileType(aFileName, ImageType);

if(ImageType.Length() && iImageName.Length())
{		
iImageDecoder = CImageDecoder::FileNewL(CCoeEnv::Static()->FsSession(), aFileName, ImageType);

delete iFrame;
iFrame = NULL;
iFrame = new (ELeave)CFbsBitmap();
iFrame->Create(iImageDecoder->FrameInfo(0).iOverallSizeInPixels, iImageDecoder->FrameInfo(0).iFrameDisplayMode);

iImageDecoder->Convert(&iStatus, *iFrame, 0);
SetActive();
}
else
{
TRequestStatus* status = &iStatus;
User::RequestComplete(status, KErrNotSupported);
SetActive();
}
}

void CAafImageReader::GetFileType(const TDesC& aFileName, TDes8& aFileType)
{
TEntry FileEntry;

if(CCoeEnv::Static()->FsSession().Entry(aFileName, FileEntry) == KErrNone)
{
TBuf8<255> FileBuffer;

if(!FileEntry.IsDir())
{
TInt FileSize = FileEntry.iSize;

if(FileSize > 255)
{
FileSize = 255;
}

if(CCoeEnv::Static()->FsSession().ReadFileSection(aFileName, 0, FileBuffer, FileSize) == KErrNone)
{
RApaLsSession RSession;
if(RSession.Connect() == KErrNone)
{	
TDataRecognitionResult FileDataType;

RSession.RecognizeData(aFileName, FileBuffer, *&FileDataType);

//if(FileDataType.iConfidence > aResult.iConfidence)
//{
aFileType.Copy(FileDataType.iDataType.Des8());
//}

RSession.Close();
}
}
}
}
}

void CAafImageReader::DoCancel()
{
iImageDecoder->Cancel();
}

CFbsBitmap* CAafImageReader::Bitmap()
{
return iFrame;
} 

void CAafImageReader::RunL()
{
iNotify->HandleRequestCompletedL(iStatus.Int());
}
*/

HBufC8* CAafUtils::Base64EncodeLC(const TDesC8 &aSourceBuf)
{
	__LOGSTR_TOFILE("CAafUtils::Base64EncodeLC() begins");

	TImCodecB64 B64;
	//Using base64 the size is increased by 1/3
	HBufC8* buffer = HBufC8::NewL(aSourceBuf.Length() + aSourceBuf.Length()/3);
	B64.Initialise();
	TPtr8 buffPtr = buffer->Des();
	B64.Encode(aSourceBuf, buffPtr);

	__LOGSTR_TOFILE("CAafUtils::Base64EncodeLC() ends");

	return  buffer;
}


HBufC8* CAafUtils::Base64EncodeFileLC(const TDesC &aFileName)
{
	__LOGSTR_TOFILE1("CAafUtils::Base64EncodeFileLC() begins with aFileName == %S", &aFileName);

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
			User::LeaveIfError(fsSession.Connect());
			CleanupClosePushL( fsSession); // Make it leave safe

			// Try to open specified file
			RFile file;
			TInt err = file.Open(fsSession, aFileName, EFileRead);

			CleanupClosePushL( file ); // Make it leave safe

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
					retValue = CAafUtils::Base64EncodeLC(*fileBuffer);
				}
			}

			// Free allocated memory
			if (fileBuffer)
			{
				delete fileBuffer;
				fileBuffer = NULL;
			}
			
			file.Close();
			fsSession.Close();
			//CleanupStack::Pop(2); // file and fsSession
			//CleanupStack::PopAndDestroy(&file);
			//CleanupStack::PopAndDestroy(&fsSession);
		}
	}

	__LOGSTR_TOFILE("CAafUtils::Base64EncodeFileLC() ends");

	return retValue;
}

HBufC8* CAafUtils::Base64DecodeLC(const TDesC8 &aSourceBuf)
{
	__LOGSTR_TOFILE("CAafUtils::Base64DecodeLC() begins");

	TImCodecB64 B64;
	HBufC8* buffer = HBufC8::NewLC(aSourceBuf.Length());
	B64.Initialise();
	TPtr8 buffPtr = buffer->Des();
	B64.Decode(aSourceBuf, buffPtr);

	__LOGSTR_TOFILE("CAafUtils::Base64DecodeLC() ends");

	return  buffer;
}

char* CAafUtils::DescriptorToStringL(const TDesC& aDescriptor)
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

char* CAafUtils::DescriptorToString8L(const TDesC8& aDescriptor)
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

HBufC8* CAafUtils::StringToDescriptor8LC(const char* aString)
{
	TPtrC8 ptr(reinterpret_cast<const TUint8*>(aString));

	HBufC8* buffer = HBufC8::NewLC(ptr.Length());
	buffer->Des().Copy(ptr);

	return buffer;
}

HBufC* CAafUtils::StringToDescriptorLC(const char* aString)
{
	TPtrC8 ptr(reinterpret_cast<const TUint8*>(aString));

	HBufC* buffer = HBufC::NewLC(ptr.Length());
	buffer->Des().Copy(ptr);

	return buffer;
}

// End of file