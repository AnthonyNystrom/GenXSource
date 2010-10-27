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
#include <pathinfo.h>
#include <senserviceconnection.h>
#include <senxmlservicedescription.h>
#include <senservicepattern.h>
#include <sensoapmessage.h>
#include <aknquerydialog.h>
#include <eikenv.h>
#include <ICL\ImageCodecData.h>
#include <s32strm.h>
#include "Aafuploadserviceprovider.h"
#include "Aafloginserviceprovider.h"
#include "Aafappimagecartview.h"
#include "Aafimagehandler.h"
#include "Aafapputils.h"
#include "Aafappui.h"
#include "Aaf.pan"

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
CAafUploadServiceProvider::CAafUploadServiceProvider()
:CCoeStatic(KUidAafUploadController)
{
	// Set cancel status
	iCancelStatus = EFalse;

	// Set image conversion stage
	iConversionStage = KImagePreparing;
	
	iSerializingFile = TFileName(KNullDesC);
}

CAafUploadServiceProvider::~CAafUploadServiceProvider()
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

	if (iWaitDialog)
	{
		delete iWaitDialog;
		iWaitDialog = NULL;
	}
}

void CAafUploadServiceProvider::SetIAPL(TUint32 aIap)
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

TInt CAafUploadServiceProvider::StartUploadL(MRequestObserver* aObserver, const TInt &aFileIndex, const TDesC8 &aWebAskId)
{
	__LOGSTR_TOFILE("CAafUploadServiceProvider::StartUploadL() begins");

	// If current operation should be cancelled
	if (iCancelStatus)
		return KErrCancel;

	// Return value
	TInt retValue = KErrNone;

	CAafLoginServiceProvider* loginProvider = CAafLoginServiceProvider::GetInstanceL();

	// If credential data is valid
	if (loginProvider->GetMemberID() || loginProvider->GetPassword())
	{
		StartWaitDialog();

		// Set current web ask id
		TPtr8 webIdPtr = iWebAskId.Des();
		webIdPtr.Copy(aWebAskId);

		// Set request observer
		__ASSERT_ALWAYS(aObserver, Panic(ENoObserver));

		iRequestObserver = aObserver;

		// If file index is not valid
		if (aFileIndex < 0 || aFileIndex >= iSelectionList.Count())
		{
			return KErrUnknown;
		}

		__LOGSTR_TOFILE("CAafUploadServiceProvider::StartUploadL() start to prepare image for uploading");

		iFileIndex = aFileIndex;

		PrepareUpload();
	}
	else
	{
		retValue = KErrUnknown;
	}

	__LOGSTR_TOFILE("CAafUploadServiceProvider::StartUploadL() ends");

	return retValue;
}

void CAafUploadServiceProvider::ConstructL()
{
	__LOGSTR_TOFILE("CAafUploadServiceProvider::ConstructL() begins");

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
	iImageHandler = new (ELeave) CAafImageHandler(*iBitmap, CEikonEnv::Static()->FsSession(), *this);
	iImageHandler->ConstructL();

	__LOGSTR_TOFILE("CAafUploadServiceProvider::ConstructL() ends");
}

void CAafUploadServiceProvider::PrepareUpload()
{
	__LOGSTR_TOFILE("CAafUploadServiceProvider::PrepareUpload() begins");


	switch(iConversionStage)
	{
	case KImagePreparing:
		{
			// If current operation should be cancelled
			if (iCancelStatus)
				return;

			// If file index is correct
			if (iFileIndex>= 0 && iFileIndex < iSelectionList.Count())
			{
				// Form file full path
				TFileName fileFullName(KNullDesC);

				fileFullName.Copy(iFileList[iSelectionList[iFileIndex]]->iFileFolder);

				fileFullName.Append(iFileList[iSelectionList[iFileIndex]]->iFileEntry.iName);			

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
			// Converting image to base64 string
			HBufC8* imageBuffer = CAafUtils::Base64EncodeLC(*iImageBuffer);

			// Free allocated memory
			if (iImageBuffer)
			{
				delete iImageBuffer;
				iImageBuffer = NULL;
			}

			// Start uploading
			UploadImage(*imageBuffer);

			if (imageBuffer)
			{
				delete imageBuffer;
				imageBuffer = NULL;
			}

			iConversionStage = KImagePreparing;
		}
		break;
	}

	__LOGSTR_TOFILE("CAafUploadServiceProvider::PrepareUpload() ends");
}

TInt CAafUploadServiceProvider::UploadImage(const TDesC8 &aFileBuffer)
{	
	__LOGSTR_TOFILE("CAafUploadServiceProvider::UploadImage() begins");

	// If current operation should be cancelled
	if (iCancelStatus)
		return KErrCancel;

	// Otherwise
	TInt retValue = KErrUnknown;

	CAafLoginServiceProvider* loginProvider = CAafLoginServiceProvider::GetInstanceL();

	if (loginProvider)
	{
		// Create empty SOAP message to hold the controller application
		CSenSoapMessage* soapRequest = CSenSoapMessage::NewL();
		CleanupStack::PushL(soapRequest);

		// Set SOAP action HTTP header
		soapRequest->SetSoapActionL(KSoapActionAttachPhoto);

		// Get handle to SOAP body element
		CSenElement& messageBody = soapRequest->BodyL();

		// Add new GetPrivateAAFQuestion element into SOAP body
		CSenElement& photoSubmit =  messageBody.AddElementL(KServiceXmlns,
			_L8("AttachPhoto"));

		// Add username child element into the SOAP body
		CSenElement& usernameString = photoSubmit.AddElementL(_L8("WebMemberID"));
		usernameString.SetContentL( *loginProvider->GetMemberID() );

		// Add password element to the request
		CSenElement& passwordString = photoSubmit.AddElementL(_L8("WebPassword"));
		passwordString.SetContentL( *loginProvider->GetPassword());

		// Add web ask a friend id
		CSenElement& webaskidString = photoSubmit.AddElementL(_L8("WebAskAFriendId"));
		webaskidString.SetContentL( iWebAskId );

		// Set current photo index
		CSenElement& indexorderString = photoSubmit.AddElementL(_L8("IndexOrder"));
		if (iFileIndex == 1)
			indexorderString.SetContentL( _L8("1") );
		else if (iFileIndex == 2)
			indexorderString.SetContentL( _L8("2") );
		else
			indexorderString.SetContentL( _L8("3") );

		// Set photo as a base64 string buffer
		CSenElement& photoString = photoSubmit.AddElementL(_L8("PhotoBase64String"));
		photoString.SetContentL( aFileBuffer );

		// Submit SOAP async request
		retValue = iConnection->SendL(*soapRequest);

		CleanupStack::PopAndDestroy(); // soapRequest
	}

	__LOGSTR_TOFILE("CAafUploadServiceProvider::UploadImage() ends");

	return retValue;
}

void CAafUploadServiceProvider::DeleteArrayObjects()
{
	__LOGSTR_TOFILE("CAafUploadServiceProvider::DeleteArrayObjects() begins");

	for (TInt i = 0; i < iFileList.Count(); i++)
	{		
		delete iFileList[i];
		iFileList[i] = NULL;
	}

	__LOGSTR_TOFILE("CAafUploadServiceProvider::DeleteArrayObjects() ends");
}

TBool CAafUploadServiceProvider::SerializeDataL()
{
	__LOGSTR_TOFILE("CAafUploadServiceProvider::SerializeDataL() begins");

	TBool retValue = ETrue;


	__LOGSTR_TOFILE("CAafUploadServiceProvider::SerializeDataL() step -01");

	RFs fsSession;
	RFileWriteStream writeStream; // Write file stream

	// Install write file session
	User::LeaveIfError(fsSession.Connect());
	CleanupClosePushL(fsSession);

	__LOGSTR_TOFILE("CAafUploadServiceProvider::SerializeDataL() step -02");

	// Open file stream
	// if already exists - replace with newer version
	TInt err = writeStream.Replace(fsSession, iSerializingFile, EFileStream | EFileWrite | EFileShareExclusive);
	CleanupClosePushL(writeStream);

	// Return EFalse if failed to open stream
	if (err != KErrNone)
	{
		retValue = EFalse;

		__LOGSTR_TOFILE("CAafUploadServiceProvider::SerializeDataL() failed to open file");
	}

	if (retValue)
	{
		__LOGSTR_TOFILE("CAafUploadServiceProvider::SerializeDataL() succeed to open the file");

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

	__LOGSTR_TOFILE("CAafUploadServiceProvider::SerializeDataL() ends");

	return retValue;
}

TBool CAafUploadServiceProvider::DeserializeDataL()
{
	__LOGSTR_TOFILE("CAafUploadServiceProvider::DeserializeDataL() begins");

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

			__LOGSTR_TOFILE("CAafUploadServiceProvider::DeserializeDataL() failed to open");
		}

		if (retValue)
		{
			TInt filesCount = 0;
			HBufC8* fileItem = NULL;	

			// Files count
			filesCount = readStream.ReadInt32L();

			__LOGSTR_TOFILE1("CAafUploadServiceProvider::DeserializeDataL() files count == %d", filesCount);
			
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
					__LOGSTR_TOFILE("CAafUploadServiceProvider::DeserializeDataL() adding new file to array");

					TIdentityRelation<TFileDescription> relation(TFileDescription::MatchDescriptions); // used for comparing

					if (iFileList.Find(fileDescription, relation) == KErrNotFound)
					{
						// Add element in sorted order
						TLinearOrder<TFileDescription> order(TFileDescription::CompareDescriptions);

						iFileList.InsertInOrder(fileDescription, order);

						// Add item to selection list if necessary
						if (fileDescription->iMarkedToUpload)
							iSelectionList.Append(iFileList.Count()-1);
					}
				}
			}			
		}

		__LOGSTR_TOFILE("CAafUploadServiceProvider::DeserializeDataL() before cleaning memory");

		// Free resource handlers
		CleanupStack::PopAndDestroy(&readStream);
		CleanupStack::PopAndDestroy(&fsSession);		
	}

	__LOGSTR_TOFILE("CAafUploadServiceProvider::DeserializeDataL() ends");

	return retValue;
}

void CAafUploadServiceProvider::HandleMessageL(const TDesC8 &/*aResponce*/)
{
	__LOGSTR_TOFILE("CAafUploadServiceProvider::HandleMessageL() begins");

	StopWaitDialog();

	iRequestObserver->HandleRequestCompletedL(KErrNone);

	__LOGSTR_TOFILE("CAafUploadServiceProvider::HandleMessageL() ends");
}

void CAafUploadServiceProvider::HandleErrorL(const TInt aErrorCode, const TDesC8 &aMessage)
{
	__LOGSTR_TOFILE2("CAafUploadServiceProvider::HandleErrorL() begins with aErrorCode == %d and aMessage == %S", aErrorCode, &aMessage);

	StopWaitDialog();

	iRequestObserver->HandleRequestCompletedL(aErrorCode);

	__LOGSTR_TOFILE("CAafUploadServiceProvider::HandleErrorL() ends");
}

void CAafUploadServiceProvider::SetStatus(const TInt aStatus)
{
	iConectionStatus = aStatus;
}

void CAafUploadServiceProvider::HandleRequestCompletedL(const TInt& aError)
{
	PrepareUpload();
}

void CAafUploadServiceProvider::StartWaitDialog()
{
	if(iWaitDialog)
	{
		delete iWaitDialog;
		iWaitDialog = NULL;
	}

	// For the wait dialog
	//create instance
	iWaitDialog = new (ELeave) CAknWaitDialog(REINTERPRET_CAST(CEikDialog**, &iWaitDialog)); 

	iWaitDialog->SetCallback(this);

	// Get CCoeEnv instance
	CEikonEnv* eikonEnv = CEikonEnv::Static();

	HBufC* stringHolder = StringLoader::LoadL(R_UPLOAD_WAITNOTE, eikonEnv );

	iWaitDialog->SetTextL(*stringHolder);

	iWaitDialog->ExecuteLD(R_WAITNOTE);

	delete stringHolder;
	stringHolder = NULL;
}

void CAafUploadServiceProvider::StopWaitDialog()
{
	// For wait dialog
	if (iWaitDialog)
	{
		iWaitDialog->ProcessFinishedL(); 
		iWaitDialog = NULL;
	}	
}

void CAafUploadServiceProvider::DialogDismissedL(TInt aButtonId)
{
	StopWaitDialog();
}

CAafUploadServiceProvider* CAafUploadServiceProvider::GetInstanceL()
{
	__LOGSTR_TOFILE("CAafUploadServiceProvider::GetInstanceL() begins");

	CAafUploadServiceProvider* controllerInstance = static_cast<CAafUploadServiceProvider*>
		( CCoeEnv::Static( KUidAafUploadController ) );

	if (!controllerInstance)
	{
		// Create instance
		controllerInstance = new (ELeave)CAafUploadServiceProvider;
		CleanupStack::PushL( controllerInstance );
		controllerInstance->ConstructL();
		CleanupStack::Pop();
	}

	__LOGSTR_TOFILE("CAafUploadServiceProvider::GetInstanceL() ends");

	return controllerInstance;
}

void CAafUploadServiceProvider::AddFile(const TEntry &aFileEntry, const TFileName &aFolder)
{
	__LOGSTR_TOFILE("CAafUploadServiceProvider::AddFileL() begins");

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

	__LOGSTR_TOFILE("CAafUploadServiceProvider::AddFileL() ends");
}

TInt CAafUploadServiceProvider::GetFilesCount()
{
	return iFileList.Count();
}

void CAafUploadServiceProvider::ResetArray()
{
	__LOGSTR_TOFILE("CAafUploadServiceProvider::ResetArray() begins");

	DeleteArrayObjects();

	iFileList.Reset();

	__LOGSTR_TOFILE("CAafUploadServiceProvider::ResetArray() ends");
}

void CAafUploadServiceProvider::DeleteItem(TInt aPosition)
{
	__LOGSTR_TOFILE("CAafUploadServiceProvider::DeleteItem() begins");

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

	__LOGSTR_TOFILE("CAafUploadServiceProvider::DeleteItem() ends");
}

void CAafUploadServiceProvider::GetFileListItemsL(CDesCArray *aItems)
{
	__LOGSTR_TOFILE("CAafUploadServiceProvider::GetFileListItemsL() begins");

	TFileName fileName(KNullDesC);

	for (TInt i = 0; i < iFileList.Count(); i++)
	{
		fileName.Format(KListboxWithIconPattern, 1, &iFileList[i]->iFileEntry.iName);
		aItems->AppendL(fileName);				
	}

	__LOGSTR_TOFILE("CAafUploadServiceProvider::GetFileListItemsL() ends");
}

TFileName CAafUploadServiceProvider::GetFilePath(TInt aPosition)
{
	__LOGSTR_TOFILE("CAafUploadServiceProvider::GetFilePath() begins");

	TFileName filePath(KNullDesC);

	if (aPosition >= 0 && aPosition < iFileList.Count())
	{
		filePath.Copy(iFileList[aPosition]->iFileFolder);
		filePath.Append(iFileList[aPosition]->iFileEntry.iName);

		RDebug::Print(_L("CAafUploadServiceProvider::GetFilePathL() formed value is %S"), &filePath);
	}
	else
	{
		__LOGSTR_TOFILE("CAafUploadServiceProvider::GetFilePath() panic: invalid index");

		Panic(EImageCartInvalidIndex);
	}

	__LOGSTR_TOFILE("CAafUploadServiceProvider::GetFilePath() ends");

	return filePath;
}

void CAafUploadServiceProvider::SetItemMarked(TInt aPosition, TBool aMark /* = ETrue */)
{
	__LOGSTR_TOFILE("CAafUploadServiceProvider::SetItemMarked() begins");

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

	__LOGSTR_TOFILE("CAafUploadServiceProvider::SetItemMarked() ends");
}

TBool CAafUploadServiceProvider::IsItemMarked(TInt aPosition)
{
	__LOGSTR_TOFILE("CAafUploadServiceProvider::IsItemMarked() begins");

	if (aPosition >= 0 && aPosition < iFileList.Count())
	{
		return iFileList[aPosition]->iMarkedToUpload;
	}

	return EFalse;

	__LOGSTR_TOFILE("CAafUploadServiceProvider::IsItemMarked() ends");
}

RArray<TInt>& CAafUploadServiceProvider::GetSelectionArray()
{
	return iSelectionList;
}

void CAafUploadServiceProvider::CancelUpload()
{
	__LOGSTR_TOFILE("CAafUploadServiceProvider::CancelUpload() begins");

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

	__LOGSTR_TOFILE("CAafUploadServiceProvider::CancelUpload() ends");
}

TInt CAafUploadServiceProvider::GetStatus()
{
	// Here we reset cancel flag and return current connection readiness status
	iCancelStatus = EFalse;

	return iConectionStatus;
}