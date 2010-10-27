/*
============================================================================
Name        : Supuploadserviceprovider.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : Upload images stuff controller class
============================================================================
*/

#ifndef __AAFUPLOADCONTROLLER_H__
#define __AAFUPLOADCONTROLLER_H__

#include <coemain.h>
#include <e32cmn.h>
#include <aknlists.h>
#include <eiklbx.h>
#include <aknprogressdialog.h> // For MProgressDialogCallback
#include <msenserviceconsumer.h>
#include "common.h"

// FORWARD DECLARATION
class CSenServiceConnection;
class CAafImageHandler;

struct TFileDescription
{
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
	

	TEntry iFileEntry;

	TBufC<KMaxFileName> iFileFolder;

	TBool iMarkedToUpload;
};


// Singleton class
class CAafUploadServiceProvider: public CCoeStatic, public MSenServiceConsumer, public MRequestObserver, public MProgressDialogCallback
{
	friend class CAafAppUi;
	
public:
	/**
	* Static method returns an instance of the CAafUploadServiceProvider class
	*/
	static CAafUploadServiceProvider* GetInstanceL();

	/**
	* Virtual destructor
	*/
	virtual ~CAafUploadServiceProvider();

	/**
	* Set IAP value to be used by WSF
	*/
	void SetIAPL(TUint32 aIap);

	/**
	* Initiate upload process
	*/
	TInt StartUploadL(MRequestObserver* aObserver, const TInt &aFileIndex, const TDesC8 &aWebAskId);

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

	// Get's called when a dialog is dismissed
	// From MProgressDialogCallback
	void DialogDismissedL(TInt aButtonId);

	// New functions
	// Starts wait dialog
	void StartWaitDialog();

	// Stops the wait dialog
	void StopWaitDialog();

private:
	/**
	* Default constructor
	*/
	CAafUploadServiceProvider();

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
	TInt UploadImage(const TDesC8 &aFileBuffer);

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

	CAafImageHandler* iImageHandler; // Image handler instance

	CFbsBitmap* iBitmap; // Currently processed bitmap

	HBufC8* iImageBuffer; // Currently processed image buffer

	TBufC8<50> iWebAskId; // Web ask id of the question to which currently processed image should be attached

	CAknWaitDialog* iWaitDialog; // Wait dialog
	
	TFileName iSerializingFile; // Path to the file which is used 
};

#endif // __AAFUPLOADCONTROLLER_H__

// End of File