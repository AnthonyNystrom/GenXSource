/*
============================================================================
Name        : Supuploadserviceprovider.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Upload images stuff controller class
============================================================================
*/

#ifndef __SUPUPLOADCONTROLLER_H__
#define __SUPUPLOADCONTROLLER_H__

#include <coemain.h>
#include <e32cmn.h>
#include <aknlists.h>
#include <eiklbx.h>
#include <msenserviceconsumer.h>
#include "common.h"

// FORWARD DECLARATION
class CSenServiceConnection;
class CSupImageHandler;
class CSupAppUi;
class RWriteStream;
class RReadStream;

class TFileDescription
{
public:
	/**
	* Constructor with params
	*/
	TFileDescription(const TEntry &aFileEntry, const TFileName &aFolder, const TBool aMarked = EFalse)
	{
		iFileEntry = aFileEntry;
		iFileFolder = aFolder;
		iMarkedToUpload = aMarked;
	}

	/**
	* Default constructor
	*/
	TFileDescription()
	{
		iFileFolder = KNullDesC;
		iMarkedToUpload = EFalse;
	}

	/**
	* Static matching method, used in collections etc
	*/
	static TBool MatchDescriptions(const TFileDescription &aFirstEntry, const TFileDescription &aSecondEntry)
	{
		return ( aFirstEntry.iFileEntry.iName == aSecondEntry.iFileEntry.iName && aFirstEntry.iFileFolder == aSecondEntry.iFileFolder);
	}

	/**
	* Static comparing method, used in collections etc
	*/
	static TInt CompareDescriptions(const TFileDescription &aFirstEntry, const TFileDescription &aSecondEntry)
	{
		TInt ret = (aFirstEntry.iFileEntry.iName).Compare(aSecondEntry.iFileEntry.iName);

		if (ret > 0)
			return 1;

		if (ret < 0)
			return -1;

		return 0;
	}

	/**
	* Saves object to file
	*/
	void ExternalizeL(RWriteStream& aStream) const;

	/**
	* Read object from file
	*/
	void InternalizeL(RReadStream& aStream);

public:
	TEntry iFileEntry;

	TBufC<KMaxFileName> iFileFolder;

	TBool iMarkedToUpload;
};


// Singleton implementation based on CCoeStatic
// to ensure that created object will be stored by CCoeEnv
class CSupUploadServiceProvider: public CCoeStatic, public MSenServiceConsumer, public MRequestObserver
{
	friend class CSupAppUi;

public:
	/**
	* Static method returns an instance of the CSupUploadServiceProvider class
	*/
	static CSupUploadServiceProvider* InstanceL();

	/**
	* Virtual destructor
	*/
	virtual ~CSupUploadServiceProvider();

	/**
	* Initiate upload process
	*/
	TInt StartUploadL(MRequestObserver* aObserver, const TInt &aFileIndex);

	/**
	* Add file
	*/
	void AddFile(const TEntry &aFileEntry, const TFileName &aFolder);

	/**
	* Get count of items
	*/
	TInt GetFilesCount();

	/**
	* Reset pointer array
	*/
	void ResetArray();

	/**
	* Delete item at the specified position
	*/
	void DeleteItem(TInt aPosition);

	/**
	* Get list of items
	*/
	void GetFileListItemsL(CDesCArray* aItems);

	/**
	*
	*/
	TFileName GetFilePath(TInt aPosition);

	/**
	* Mark/unmark entry for uploading
	*/
	void SetItemMarked(TInt aPosition, TBool aMark = ETrue);

	/**
	* Determine whether item is marked for uploading
	*/
	TBool IsItemMarked(TInt aPosition);

	/**
	* Get array of selected items indexes
	*/
	RArray<TInt>& GetSelectionArray();

	/**
	* Cancel upload process
	*/
	void CancelUpload();

	/**
	* Get current connection status
	*/
	TInt GetStatus();

	/**
	* Set IAP value to be used by WSF
	*/
	void SetIAPL(TUint32 aIap);


protected: // from MSenServiceConsumer - for performing asynchronous requests to web service
	/**
	*
	*/
	void HandleMessageL(const TDesC8 &aResponce);

	/**
	*
	*/
	void HandleErrorL(const TInt aErrorCode, const TDesC8 &aMessage);

	/**
	*
	*/
	void SetStatus(const TInt aStatus);

	/**
	* From MRequestObserver
	*/
	void HandleRequestCompletedL(const TInt& aError);

private:
	/**
	* Default constructor
	*/
	CSupUploadServiceProvider();

	/**
	* Two-phase constructor
	*/
	void ConstructL();

private:
	/**
	* Retrieves file buffer and date by index
	*/
	void PrepareUpload();

	/**
	* Send appropriate soap message (async request)
	*/
	TInt UploadImage(const TDesC8 &aFileBuffer, const TDesC8 &aFileDate);

	/**
	* Remove memory allocated by objects of the RPointerArray
	*/
	void DeleteArrayObjects();

	/**
	* Save image cart data to file
	*/
	TBool SerializeDataL();

	/**
	* Read image cart data from file
	*/
	TBool DeserializeDataL();

private: // Class member variables

	// Enum for internal use,
	// indicates current image handling step
	enum EConvCurrentStep
	{
		KImagePreparing = -1,
		KImageReading,
		KImageConverting
	};

	EConvCurrentStep iConversionStage; // Current image converting stage

	TInt iFileIndex; // Currently processed image order index

	CSenServiceConnection* iConnection;

	MRequestObserver* iRequestObserver;

	RPointerArray<TFileDescription> iFileList; // List of the files (user's added to image cart)

	RArray<TInt> iSelectionList; // List of indexes of the marked items

	TInt iConectionStatus; // Current connection status

	TBool iCancelStatus; // ETrue if current operation has been cancelled

	CSupImageHandler* iImageHandler; // Image handler instance

	CFbsBitmap* iBitmap; // Currently processed bitmap

	HBufC8* iImageBuffer; // Currently processed image buffer

	TFileName iSerializingFile; // Path to the file which is used 
								// for serialization/deserialization image cart data
};

#endif // __SUPUPLOADCONTROLLER_H__

// End of File