/*
============================================================================
Name        : Supfilebrowserengine.h
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : File browser engine
============================================================================
*/

#ifndef __SUPFILEBROWSERENGINE_H__
#define __SUPFILEBROWSERENGINE_H__

#include <coecntrl.h>
#include <f32file.h>        // for CDir
#include <badesca.h>        // for CDesCArray
#include <apmstd.h>         // for TDataType
#ifdef __SERIES60_3X__
#include <aknserverapp.h>
#else
#include <apparc.h>
#endif

class CDocumentHandler;
class CEikProcess;


class CFileBrowserEngine : public CCoeControl,
#ifdef __SERIES60_3X__
	public MAknServerAppExitObserver
#else
	public MApaEmbeddedDocObserver
#endif
{
public: // // Constructors and destructor

	/**
	* Symbian default constructor.
	*/
	void ConstructL(CEikProcess* aProcess = NULL);

	/**
	* Destructor.
	*/
	~CFileBrowserEngine();

public: // New functions

	/*
	* CFileBrowserEngine::StartFileList()
	* This Method gets the specific directory.
	* In the 1st edition use harcoded paths and the S60 2nd and 3rd edition
	* get the specific path using PathInfo.
	*/
	TInt StartFileList();

	/**
	* Opens file-server
	*/
	TInt StartCurrentList();
	
	/*
	* CFileBrowserEngine::GetFileListItems(CDesCArray* aItems)
	* This Method constructs the listbox items with directory
	* information
	*/
	void GetFileListItemsL(CDesCArray* iItems);

	/*
	* CFileBrowserEngine::SetDirectory(TInt aDirectory)
	* This Method sets which directory to list.
	*/
	void SetDirectory(TInt aDirectory);

	/*
	* CFileBrowserEngine::SetSizeDate(TInt aSizeDate)
	* This Method sets whether modification date or file size
	* is shown. There is also option for toggling the status.
	*/
	void SetSizeDate(TInt aSizeDate);

	/*
	* CFileBrowserEngine::EndFileList()
	* This Method ends the FileList session
	*/
	void EndFileList();

	/*
	* CFileBrowserEngine::RemoveItems(CDesCArray* aItems)
	* This Method removes all listbox items when a new list
	* needs to be shown.
	*/
	TBool RemoveItems(CDesCArray* aItems);

	/*
	* CFileBrowserEngine::LaunchCurrent(TInt aPosition)
	* This Method launches selected item with DocumentHandler.
	* DocumentHandler will launch correct application depending
	* on the file type.
	* Note that all the extensions do not work on emulator.
	*/
	void LaunchCurrentL(TInt aPosition);

	/**
	* Launch document by specified file path
	*/
	void LaunchCurrentL(const TFileName &aFilePath);

	/*
	* CFileBrowserEngine::IsDirListEmpty()
	*/
	TBool IsDirListEmpty();

	/*
	* CFileBrowserEngine::GetResourceFilesL ( TFileName &aBasePath )
	* The method get all files including files into subfolders.
	*/
	void GetResourceFilesL ( TFileName &aBasePath );

	/**
	* CFileBrowserEngine::GetFilesAndFoldersL(TFileName &aFolder)
	* Retrieves list of files and folders in the specified 
	*/
	void GetEntriesL(TFileName &aFolder);

	/**
	* CFileBrowserEngine::OpenOrLaunchCurrentL(TInt aPosition)
	* Opens directory or opens file preview
	*/
	TInt OpenOrLaunchCurrentL(TInt aPosition);

	/*
	* CFileBrowserEngine::IsDir(TInt aPosition)
	* Determine whether item at the specified position is listbox
	*/
	TBool IsDir(TInt aPosition);

	/*
	* CFileBrowserEngine::GetCurrentItems(CDesCArray* aItems)
	* This Method constructs the listbox items with directory
	* information
	*/
	void GetCurrentItemsL(CDesCArray* aItems);

	/**
	*
	*/
	TBool RemoveCurrentItems(CDesCArray* aItems);

	/**
	*
	*/
	void SetCurrentDirectory(TFileName &aDirectory)
	{
		iCurrentDirectory = aDirectory;
	}

	/**
	*
	*/
	TFileName& GetCurrentDirectory()
	{
		return iCurrentDirectory;
	}

	/**
	*
	*/
	TFileName GetFileNameL(TInt aPosition);

	/**
	*
	*/
	void GetFileDescriptionL(const TInt &aPosition, TEntry &aFileEntry, TFileName &aFolder);

	/**
	* Retrieves file description (view with folders)
	*/
	void GetFileDescription2L(const TInt &aPosition, TEntry &aFileEntry, TFileName &aFolder);
	
#ifdef __SERIES60_3X__
	void HandleServerAppExit(TInt aReason);
#else
	void NotifyExit(TExitMode aMode);
#endif

private: //Data
	/**
	*
	*/
	RArray<TEntry> iFileList;
	/**
	*
	*/
	RArray<TFileName> iDirList;

	/**
	* File-server session
	*/
	RFs iFsSession;

	/**
	* Currently opened directory
	*/
	TFileName iCurrentDirectory;

	// In the listbox we would display items as sum of folders list and files list
	// e.g.
	// ...
	// folder1
	// folder1
	// folder3
	// file1
	// file2
	// file3
	RArray<TEntry> iCurrentFolders; // List of directories in the current directory

	/**
	* List of files in the current directory
	*/
	RArray<TEntry> iCurrentFiles;
	

private:
	/**
	* Which directory to show
	*/
	TInt iDirectory;
	
	/**
	* Show date or size
	*/
	TInt iSizeDate;

	/**
	*
	*/
	TBool iDocEmbedded;

	/**
	*
	*/
	CDocumentHandler *iDocHandler;
};


#endif // __SUPFILEBROWSERENGINE_H__

// End of File