/*
============================================================================
Name        : Supuploadserviceprovider.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Upload images stuff controller class implementation
============================================================================
*/

// INCLUDE FILES
#include <bautils.h>
#include <PathInfo.h>
#include <senserviceconnection.h>
#include <senxmlservicedescription.h>
#include <senservicepattern.h>
#include <sensoapmessage.h>
#include <aknquerydialog.h>
#include <eikenv.h>
#include <ICL\ImageCodecData.h>
#include <s32strm.h>
#include "Supuploadserviceprovider.h"
#include "Suploginserviceprovider.h"
#include "Supappimagecartview.h"
#include "Supimagehandler.h"
#include "Supapputils.h"
#include "Supappui.h"
#include "sup.pan"

// TFileDescription class implementation
void TFileDescription::ExternalizeL(RWriteStream& aStream) const
{
	__LOGSTR_TOFILE("TFileDescription::ExternalizeL() begins");

	// iFileEntry
	aStream.WriteUint32L(iFileEntry.iAtt);
	aStream.WriteInt32L(iFileEntry.iSize);
	
	// TUidType
	for (TInt i = 0; i < KMaxCheckedUid; i++)
	{
		aStream.WriteInt32L(iFileEntry.iType[i].iUid);
	}

	aStream.WriteReal64L(iFileEntry.iModified.Int64());

	// Filename
	aStream << iFileEntry.iName;
	
	// iFileFolder
	aStream << iFileFolder;

	// iMarkedToUpload
	if (iMarkedToUpload)
	{
		aStream.WriteInt8L(1);
	}
	else
	{
		aStream.WriteInt8L(0);
	}

	__LOGSTR_TOFILE("TFileDescription::ExternalizeL() ends");
}

void TFileDescription::InternalizeL(RReadStream& aStream)
{
	__LOGSTR_TOFILE("TFileDescription::InternalizeL() begins");

	// iFileEntry
	iFileEntry.iAtt = aStream.ReadUint32L();

	__LOGSTR_TOFILE("TFileDescription::InternalizeL() step 01");

	iFileEntry.iSize = aStream.ReadInt32L();

	__LOGSTR_TOFILE("TFileDescription::InternalizeL() step 02");

	// TUidType
	TUid* uids = new (ELeave)TUid[KMaxCheckedUid];

	for (TInt i = 0; i < KMaxCheckedUid; i++)
	{
		uids[i].iUid = aStream.ReadInt32L();
	}

	__LOGSTR_TOFILE("TFileDescription::InternalizeL() step 03");

	iFileEntry.iType = TUidType(uids[0], uids[1], uids[2]);

	__LOGSTR_TOFILE("TFileDescription::InternalizeL() step 03");

	delete [] uids;
	uids = NULL;

	__LOGSTR_TOFILE("TFileDescription::InternalizeL() step 04");

	// File time
	iFileEntry.iModified = aStream.ReadReal64L();

	__LOGSTR_TOFILE("TFileDescription::InternalizeL() step 05");

	// File name
	HBufC* tempValue = NULL;	

	__LOGSTR_TOFILE("TFileDescription::InternalizeL() step 06");

	tempValue = HBufC::NewLC(aStream, KMaxFileName);
	if (tempValue)
	{
		__LOGSTR_TOFILE1("TFileDescription::InternalizeL() file name == %S", tempValue);

		TPtr desPtr = iFileEntry.iName.Des();

		desPtr.Copy(*tempValue);

		__LOGSTR_TOFILE1("TFileDescription::InternalizeL() TBufC file name == %S", &iFileEntry.iName);
	}

	__LOGSTR_TOFILE("TFileDescription::InternalizeL() step 07");

	CleanupStack::PopAndDestroy(tempValue);

	// iFileFolder
	__LOGSTR_TOFILE("TFileDescription::InternalizeL() step 08");

	tempValue = HBufC::NewLC(aStream, KMaxPath);
	if (tempValue)
	{
		__LOGSTR_TOFILE1("TFileDescription::InternalizeL() folder name == %S", tempValue);

		TPtr desPtr = iFileFolder.Des();

		desPtr.Copy(*tempValue);

		__LOGSTR_TOFILE1("TFileDescription::InternalizeL() TBufC folder name == %S", &iFileFolder);
	}

	__LOGSTR_TOFILE("TFileDescription::InternalizeL() step 09");

	CleanupStack::PopAndDestroy(tempValue);

	__LOGSTR_TOFILE("TFileDescription::InternalizeL() step 10");

	// iMarkedToUpload
	TInt boolValue = 0;

	if (iMarkedToUpload)
	{
		boolValue = aStream.ReadInt8L();
	}
	else
	{
		boolValue = aStream.ReadInt8L();
	}

	__LOGSTR_TOFILE("TFileDescription::InternalizeL() step 11");

	iMarkedToUpload = TBool(boolValue);

	__LOGSTR_TOFILE("TFileDescription::InternalizeL() ends");
}

// Upload service provider implementation
CSupUploadServiceProvider::CSupUploadServiceProvider()
: CCoeStatic( KUidSupUploadController )
{
	// Set cancel status
	iCancelStatus = EFalse;

	// Set image conversion stage
	iConversionStage = KImagePreparing;

	iSerializingFile = TFileName(KNullDesC);
}

CSupUploadServiceProvider::~CSupUploadServiceProvider()
{
	// Free allocated resources
	DeleteArrayObjects();

	iFileList.Close();

	if (iConnection)
	{
		iConnection->Cancel();

		delete iConnection;
		iConnection = NULL;
	}

	if (iBitmap)
	{
		delete iBitmap;
		iBitmap = NULL;
	}
	if (iImageHandler)
	{
		delete iImageHandler;
		iImageHandler = NULL;
	}
}

TInt CSupUploadServiceProvider::StartUploadL(MRequestObserver* aObserver, const TInt &aFileIndex)
{
	__LOGSTR_TOFILE("CSupUploadServiceProvider::StartUploadL() begins");

	// If current operation should be cancelled
	if (iCancelStatus)
		return KErrCancel;

	// Return value
	TInt retValue = KErrNone;

	CSupLoginServiceProvider* loginProvider = CSupLoginServiceProvider::InstanceL();

	// If credential data is valid
	if (loginProvider->GetMemberID() || loginProvider->GetPassword())
	{
		// Set request observer
		__ASSERT_ALWAYS(aObserver, Panic(ENoObserver));

		iRequestObserver = aObserver;

		// If file index is not valid
		if (aFileIndex < 0 || aFileIndex >= iFileList.Count())
		{
			return KErrUnknown;
		}

		__LOGSTR_TOFILE("CSupUploadServiceProvider::StartUploadL() start to prepare image for uploading");

		iFileIndex = aFileIndex;

		PrepareUpload();
	}
	else
	{
		retValue = KErrUnknown;
	}

	__LOGSTR_TOFILE("CSupUploadServiceProvider::StartUploadL() ends");

	return retValue;
}

void CSupUploadServiceProvider::ConstructL()
{
	__LOGSTR_TOFILE("CSupUploadServiceProvider::ConstructL() begins");

	// Form full path to image cart settings file
	RFs &fs = CCoeEnv::Static()->FsSession();

	TFileName privatePath;
	fs.PrivatePath(privatePath);
	TParse parser;
	TFileName processFileName(RProcess().FileName());
	User::LeaveIfError(parser.Set(KImageCartSettingsFilename, &privatePath, &processFileName));
	iSerializingFile = parser.FullName();

	// Read data from file
	DeserializeDataL();
	
	// Code should be added later
	// Discover and retrieve description of the web service
	CSenXmlServiceDescription* serviceDesc = CSenXmlServiceDescription::NewLC( KServiceEndpointPhotoOrganise, KNullDesC8 );

	serviceDesc->SetFrameworkIdL(KDefaultBasicWebServicesFrameworkID);

	// Create connection to web service
	iConnection = CSenServiceConnection::NewL(*this, *serviceDesc);

	CleanupStack::PopAndDestroy(); // serviceDesc

	// Bitmap
	iBitmap = new(ELeave) CFbsBitmap();
	iBitmap->Create(TSize(0,0), EColor256);

	// Image handler
	iImageHandler = new (ELeave) CSupImageHandler(*iBitmap, CEikonEnv::Static()->FsSession(), *this);
	iImageHandler->ConstructL();

	__LOGSTR_TOFILE("CSupUploadServiceProvider::ConstructL() ends");
}

void CSupUploadServiceProvider::PrepareUpload()
{
	__LOGSTR_TOFILE("CSupUploadServiceProvider::PrepareUpload() begins");


	switch(iConversionStage)
	{
	case KImagePreparing:
		{
			// If current operation should be cancelled
			if (iCancelStatus)
				return;

			// If file index is correct
			if (iFileIndex>= 0 && iFileIndex < iFileList.Count())
			{
				// Form file full path
				TFileName fileFullName(KNullDesC);

				fileFullName.Copy(iFileList[iFileIndex]->iFileFolder);

				fileFullName.Append(iFileList[iFileIndex]->iFileEntry.iName);			

				// Start file loading
				iImageHandler->LoadFileL(fileFullName);

				// Update image conversion status
				iConversionStage = KImageReading;
			}
		}
		break;
		
	case KImageReading:
		{
			// If current operation should be cancelled
			if (iCancelStatus)
				return;
			
			// Free allocated memory
			if (iImageBuffer)
			{
				delete iImageBuffer;
				iImageBuffer = NULL;
			}

			iImageHandler->SaveImageToBufferL(iImageBuffer, KJpegMimeType);

			// Update image conversion status
			iConversionStage = KImageConverting;
		}
		break;
	
	case KImageConverting:
		{
			// Get file date and time
			HBufC8* fileDateTime;

			// Retrieve file date/time
			TTime fileTime = iFileList[iFileIndex]->iFileEntry.iModified;

			// Microseconds from January 1st, 0 AD
			TTimeIntervalMicroSeconds timeInMicroSeconds = fileTime.MicroSecondsFrom(TDateTime(0, EJanuary, 0, 0, 0, 0, 0)); 

			// Retrieves time in dotnet ticks
			TInt64 dotNetTicks = timeInMicroSeconds.Int64() * TInt64(10);

			__LOGSTR_TOFILE1("CSupUploadServiceProvider::PrepareUpload() date and time is %d", dotNetTicks);

			fileDateTime = HBufC8::NewL(18);
			TPtr8 bufDatePtr(fileDateTime->Des());
			bufDatePtr.AppendNum(dotNetTicks);
			
			// Converting image to base64 string
			HBufC8* imageBuffer = CSupUtils::Base64EncodeLC(*iImageBuffer);

			// Free allocated memory
			if (iImageBuffer)
			{
				delete iImageBuffer;
				iImageBuffer = NULL;
			}

			// Start uploading
			UploadImage(*imageBuffer, *fileDateTime);

			if (imageBuffer)
			{
				delete imageBuffer;
				imageBuffer = NULL;
			}

			if (fileDateTime)
			{
				delete fileDateTime;
				fileDateTime = NULL;
			}

			iConversionStage = KImagePreparing;
		}
		break;
	}

	__LOGSTR_TOFILE("CSupUploadServiceProvider::PrepareUpload() ends");
}

TInt CSupUploadServiceProvider::UploadImage(const TDesC8 &aFileBuffer, const TDesC8 &aFileDate)
{	
	__LOGSTR_TOFILE("CSupUploadServiceProvider::UploadImage() begins");

	// If current operation should be cancelled
	if (iCancelStatus)
		return KErrCancel;

	// Otherwise
	TInt retValue = KErrUnknown;

	CSupLoginServiceProvider* loginProvider = CSupLoginServiceProvider::InstanceL();

	if (loginProvider)
	{
		// Create empty SOAP message to hold the controller application
		CSenSoapMessage* soapRequest = CSenSoapMessage::NewL();
		CleanupStack::PushL(soapRequest);

		// Set SOAP action HTTP header
		soapRequest->SetSoapActionL(KSoapActionUploadPhoto);

		// Get handle to SOAP body element
		CSenElement& messageBody = soapRequest->BodyL();

		// Add new GetMemberID element into SOAP body
		CSenElement& memberidRequest =  messageBody.AddElementL(KServiceXmlns,
			_L8("DeviceUploadPhoto"));

		// Add memberid child element into the SOAP body
		CSenElement& usernameString = memberidRequest.AddElementL(_L8("WebMemberID"));
		usernameString.SetContentL( *loginProvider->GetMemberID() );

		// Add password element to the request
		CSenElement& passwordString = memberidRequest.AddElementL(_L8("WebPassword"));
		passwordString.SetContentL( *loginProvider->GetPassword() );

		// Add image file base64 string buffer
		CSenElement& imageString = memberidRequest.AddElementL(_L8("Base64StringPhoto"));
		imageString.SetContentL( aFileBuffer );

		// Add image file date
		CSenElement& imageDate = memberidRequest.AddElementL(_L8("DateTime"));
		imageDate.SetContentL( aFileDate);

		__LOGSTR_TOFILE1("CSupUploadServiceProvider::UploadImage() with aFileDate == %S", &aFileDate);

		// Submit SOAP async request		
		TRAP(retValue, iConnection->SendL(*soapRequest));

		__LOGSTR_TOFILE("CSupUploadServiceProvider::UploadImage() after SendL()");

		CleanupStack::PopAndDestroy(); // soapRequest
	}

	__LOGSTR_TOFILE("CSupUploadServiceProvider::UploadImage() ends");

	return retValue;
}

void CSupUploadServiceProvider::DeleteArrayObjects()
{
	__LOGSTR_TOFILE("CSupUploadServiceProvider::DeleteArrayObjects() begins");

	for (TInt i = 0; i < iFileList.Count(); i++)
	{		
		delete iFileList[i];
		iFileList[i] = NULL;
	}

	__LOGSTR_TOFILE("CSupUploadServiceProvider::DeleteArrayObjects() ends");
}

TBool CSupUploadServiceProvider::SerializeDataL()
{
	__LOGSTR_TOFILE("CSupUploadServiceProvider::SerializeDataL() begins");

	TBool retValue = ETrue;

	__LOGSTR_TOFILE("CSupUploadServiceProvider::SerializeDataL() step -01");

	RFs fsSession;
	RFileWriteStream writeStream; // Write file stream

	// Install write file session
	User::LeaveIfError(fsSession.Connect());
	CleanupClosePushL(fsSession);

	__LOGSTR_TOFILE("CSupUploadServiceProvider::SerializeDataL() step -02");

	// Open file stream
	// if already exists - replace with newer version
	TInt err = writeStream.Replace(fsSession, iSerializingFile, EFileStream | EFileWrite | EFileShareExclusive);
	CleanupClosePushL(writeStream);

	// Return EFalse if failed to open stream
	if (err != KErrNone)
	{
		retValue = EFalse;

		__LOGSTR_TOFILE("CSupUploadServiceProvider::SerializeDataL() failed to open file");
	}

	if (retValue)
	{
		__LOGSTR_TOFILE("CCSupUploadServiceProvider::SerializeDataL() succeed to open the file");

		// Write data
		// Items count
		writeStream.WriteInt32L(iFileList.Count());
		
		for (TInt i = 0; i < iFileList.Count(); i++)
		{
			iFileList[i]->ExternalizeL(writeStream);
		}

		// Just to ensure that any buffered data is written to the stream
		writeStream.CommitL();
	}		

	// Free resource handlers
	CleanupStack::PopAndDestroy(&writeStream);
	CleanupStack::PopAndDestroy(&fsSession);
	
	__LOGSTR_TOFILE("CSupUploadServiceProvider::SerializeDataL() ends");

	return retValue;
}

TBool CSupUploadServiceProvider::DeserializeDataL()
{
	__LOGSTR_TOFILE("CSupUploadServiceProvider::DeserializeDataL() begins");

	TBool retValue = ETrue;

	// If file list is empty,
	// load it from file
	if (!iFileList.Count())
	{
		TBool retValue = ETrue;

		RFs fsSession;
		RFileReadStream readStream; // Read stream from file

		// Install read file session
		User::LeaveIfError(fsSession.Connect());
		CleanupClosePushL(fsSession);

		TInt err = readStream.Open(fsSession, iSerializingFile, EFileStream | EFileRead | EFileShareExclusive);
		CleanupClosePushL(readStream);

		// If file does not exist - return EFalse
		if (err != KErrNone)
		{
			retValue = EFalse;

			__LOGSTR_TOFILE("CSupUploadServiceProvider::DeserializeDataL() failed to open");
		}

		if (retValue)
		{
			TInt filesCount = 0;
			HBufC8* fileItem = NULL;	

			// Files count
			filesCount = readStream.ReadInt32L();

			__LOGSTR_TOFILE1("CSupUploadServiceProvider::DeserializeDataL() files count == %d", filesCount);
			
			for (TInt i = 0; i < filesCount; i++)
			{
				TFileDescription* fileDescription = new (ELeave)TFileDescription;

				fileDescription->InternalizeL(readStream);
				
				TFileName filePath(KNullDesC);
				filePath.Copy(fileDescription->iFileFolder);
				filePath.Append(fileDescription->iFileEntry.iName);

				// Ensure whether specified file actually exists
				if (BaflUtils::FileExists(fsSession, filePath))
				{
					__LOGSTR_TOFILE("CSupUploadServiceProvider::DeserializeDataL() adding new file to array");

					TIdentityRelation<TFileDescription> relation(TFileDescription::MatchDescriptions); // used for comparing

					if (iFileList.Find(fileDescription, relation) == KErrNotFound)
					{
						// Add element in sorted order
						TLinearOrder<TFileDescription> order(TFileDescription::CompareDescriptions);

						iFileList.InsertInOrder(fileDescription, order);

						// Add item to the selection list if necessary
						if (fileDescription->iMarkedToUpload)
							iSelectionList.Append(iFileList.Count()-1);
					}
				}
			}			
		}

		__LOGSTR_TOFILE("CSupUploadServiceProvider::DeserializeDataL() before cleaning memory");

		// Free resource handlers
		CleanupStack::PopAndDestroy(&readStream);
		CleanupStack::PopAndDestroy(&fsSession);		
	}

	__LOGSTR_TOFILE("CSupUploadServiceProvider::DeserializeDataL() ends");

	return retValue;
}

void CSupUploadServiceProvider::HandleMessageL(const TDesC8 &/*aResponce*/)
{
	__LOGSTR_TOFILE("CSupUploadServiceProvider::HandleMessageL() begins");

	iRequestObserver->HandleRequestCompletedL(KErrNone);

	__LOGSTR_TOFILE("CSupUploadServiceProvider::HandleMessageL() ends");
}

void CSupUploadServiceProvider::HandleErrorL(const TInt aErrorCode, const TDesC8 &aMessage)
{
	__LOGSTR_TOFILE2("CSupUploadServiceProvider::HandleErrorL() begins with aErrorCode == %d and aMessage == %S", aErrorCode, &aMessage);

	iRequestObserver->HandleRequestCompletedL(aErrorCode);

	__LOGSTR_TOFILE("CSupUploadServiceProvider::HandleErrorL() ends");
}

void CSupUploadServiceProvider::SetStatus(const TInt aStatus)
{
	iConectionStatus = aStatus;
}

void CSupUploadServiceProvider::HandleRequestCompletedL(const TInt& aError)
{
	PrepareUpload();
}

CSupUploadServiceProvider* CSupUploadServiceProvider::InstanceL()
{
	__LOGSTR_TOFILE("CSupUploadServiceProvider::InstanceL() begins");

	CSupUploadServiceProvider* controllerInstance = static_cast<CSupUploadServiceProvider*>
		( CCoeEnv::Static( KUidSupUploadController ) );

	if (!controllerInstance)
	{
		controllerInstance = new (ELeave)CSupUploadServiceProvider;
		CleanupStack::PushL( controllerInstance );
		controllerInstance->ConstructL();
		CleanupStack::Pop();
	}

	__LOGSTR_TOFILE("CSupUploadServiceProvider::InstanceL() ends");

	return controllerInstance;
}

void CSupUploadServiceProvider::AddFile(const TEntry &aFileEntry, const TFileName &aFolder)
{
	__LOGSTR_TOFILE("CSupUploadServiceProvider::AddFileL() begins");

	// Add to files list
	TFileDescription* fileDesc = new (ELeave)TFileDescription(aFileEntry, aFolder);
	TIdentityRelation<TFileDescription> relation(TFileDescription::MatchDescriptions); // used for comparing

	if (iFileList.Find(fileDesc, relation) == KErrNotFound)
	{
		RDebug::Print(_L("Added image to upload service: %S"), &aFileEntry.iName);

		// Add element in sorted order
		TLinearOrder<TFileDescription> order(TFileDescription::CompareDescriptions);

		iFileList.InsertInOrder(fileDesc, order);
	}

	__LOGSTR_TOFILE("CSupUploadServiceProvider::AddFileL() ends");
}

TInt CSupUploadServiceProvider::GetFilesCount()
{
	return iFileList.Count();
}

void CSupUploadServiceProvider::ResetArray()
{
	__LOGSTR_TOFILE("CSupUploadServiceProvider::ResetArray() begins");

	DeleteArrayObjects();

	iFileList.Reset();

	__LOGSTR_TOFILE("CSupUploadServiceProvider::ResetArray() ends");
}

void CSupUploadServiceProvider::DeleteItem(TInt aPosition)
{
	__LOGSTR_TOFILE("CSupUploadServiceProvider::DeleteItem() begins");

	if (aPosition >= 0 && aPosition < iFileList.Count())
	{
		// Delete file description object
		delete iFileList[aPosition];
		iFileList[aPosition] = NULL;

		// Delete pointer from the pointers array
		iFileList.Remove(aPosition);

		// Remove element from selection list
		iSelectionList.Remove(iSelectionList.Find(aPosition));
	}

	__LOGSTR_TOFILE("CSupUploadServiceProvider::DeleteItem() ends");
}

void CSupUploadServiceProvider::GetFileListItemsL(CDesCArray *aItems)
{
	__LOGSTR_TOFILE("CSupUploadServiceProvider::GetFileListItemsL() begins");

	TFileName fileName(KNullDesC);

	for (TInt i = 0; i < iFileList.Count(); i++)
	{
		fileName.Format(KStringIcon, 1, &iFileList[i]->iFileEntry.iName);
		aItems->AppendL(fileName);				
	}

	__LOGSTR_TOFILE("CSupUploadServiceProvider::GetFileListItemsL() ends");
}

TFileName CSupUploadServiceProvider::GetFilePath(TInt aPosition)
{
	__LOGSTR_TOFILE("CSupUploadServiceProvider::GetFilePath() begins");

	TFileName filePath(KNullDesC);

	if (aPosition >= 0 && aPosition < iFileList.Count())
	{
		filePath.Copy(iFileList[aPosition]->iFileFolder);
		filePath.Append(iFileList[aPosition]->iFileEntry.iName);

		RDebug::Print(_L("CSupUploadServiceProvider::GetFilePathL() formed value is %S"), &filePath);
	}
	else
	{
		__LOGSTR_TOFILE("CSupUploadServiceProvider::GetFilePath() panic: invalid index");

		Panic(EImageCartInvalidIndex);
	}

	__LOGSTR_TOFILE("CSupUploadServiceProvider::GetFilePath() ends");

	return filePath;
}

void CSupUploadServiceProvider::SetItemMarked(TInt aPosition, TBool aMark /* = ETrue */)
{
	__LOGSTR_TOFILE("CSupUploadServiceProvider::SetItemMarked() begins");

	if (aPosition >= 0 && aPosition < iFileList.Count())
	{
		// If mark sign is changed to opposite value
		// perform necessary operations with selection list
		if (iFileList[aPosition]->iMarkedToUpload == !aMark)
		{
			// If ETrue
			// add to selection list
			if (aMark)
			{
				iSelectionList.Append(aPosition);
			}
			// Otherwise
			// remove from selection list
			else
			{
				for (TInt i = 0; i < iSelectionList.Count(); i++)
				{
					if (iSelectionList[i] == aPosition)
					{
						iSelectionList.Remove(i);
						break;
					}
				}
			}
		}

		iFileList[aPosition]->iMarkedToUpload = aMark;
	}

	__LOGSTR_TOFILE("CSupUploadServiceProvider::SetItemMarked() ends");
}

TBool CSupUploadServiceProvider::IsItemMarked(TInt aPosition)
{
	__LOGSTR_TOFILE("CSupUploadServiceProvider::IsItemMarked() begins");

	if (aPosition >= 0 && aPosition < iFileList.Count())
	{
		return iFileList[aPosition]->iMarkedToUpload;
	}

	return EFalse;

	__LOGSTR_TOFILE("CSupUploadServiceProvider::IsItemMarked() ends");
}

RArray<TInt>& CSupUploadServiceProvider::GetSelectionArray()
{
	return iSelectionList;
}

void CSupUploadServiceProvider::CancelUpload()
{
	__LOGSTR_TOFILE("CSupUploadServiceProvider::CancelUpload() begins");

	// Set cancel status
	iCancelStatus = ETrue;

	if (iConnection)
	{
		iConnection->Cancel();
	}

	if (iImageHandler)
	{
		iImageHandler->Cancel();
	}

	__LOGSTR_TOFILE("CSupUploadServiceProvider::CancelUpload() ends");
}

TInt CSupUploadServiceProvider::GetStatus()
{
	// Here we reset cancel flag and return current connection readiness status
	iCancelStatus = EFalse;

	return iConectionStatus;
}

void CSupUploadServiceProvider::SetIAPL(TUint32 aIap)
{
	CSenServicePattern* pattern = CSenServicePattern::NewLC(KServiceEndpointPhotoOrganise, KNullDesC8);
	pattern->SetConsumerIapIdL(aIap);
	pattern->SetFrameworkIdL(KDefaultBasicWebServicesFrameworkID);

	// Recreate connection instance with new IAP value
	if (iConnection)
	{
		delete iConnection;
		iConnection = NULL;
	}

	// Create connection to web service
	iConnection = CSenServiceConnection::NewL(*this, *pattern);

	CleanupStack::PopAndDestroy(pattern);
}
