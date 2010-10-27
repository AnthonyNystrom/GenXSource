/*
============================================================================
Name        : Supfilebrowserengine.cpp
Author      : Vitaly Vinogradov
Version     : 1.0.0
Copyright   : (c) Next2Friends, 2008
Description : File browser engine
============================================================================
*/

// INCLUDE FILES
#include <bautils.h>
#include <PathInfo.h>
#include <eikenv.h>
#include <bafindf.h>
#include <stringloader.h>
#include <documenthandler.h> // for previewing selected item
#include "Supfilebrowserengine.h"
#include "Sup.hrh"     // enumerations
#include "Sup.pan"
#include "common.h"

// ================= Constants =======================
// Number, name and file size
_LIT(KStringSize, "%d\t%S\t%d bytes");
// Number, name and date modified
_LIT(KStringDate, "%d\t%S\t%S");
// Upper level
_LIT(KStringUp, "%d\t...\t\t");

#define KDirSounds    PathInfo::DigitalSoundsPath()
#define KDirPictures  PathInfo::ImagesPath()
#define KDirVideos    PathInfo::VideosPath()

const TInt KDateLength( 30 );

// ================= MEMBER FUNCTIONS =======================

// ---------------------------------------------------------
// CFileBrowserEngine::ConstructL(const TRect& aRect)
// Symbian two phased constructor
// ---------------------------------------------------------
//
void CFileBrowserEngine::ConstructL(CEikProcess* aProcess /*= NULL*/)
{
#ifdef __SERIES60_3X__
	aProcess = NULL;
	iDocHandler = CDocumentHandler::NewL();
#else
	iDocHandler = CDocumentHandler::NewL(aProcess);
#endif
}

// Destructor
CFileBrowserEngine::~CFileBrowserEngine()
{
	delete iDocHandler;
	iFileList.Close();
	iDirList.Close();
	iCurrentFolders.Close();
	iCurrentFiles.Close();
}

// ---------------------------------------------------------
// CFileBrowserEngine::StartFileList()
// This Method gets the specific directory by using PathInfo.
// ---------------------------------------------------------
//
TInt CFileBrowserEngine::StartFileList()
{
	iFileList.Reset();
	iDirList.Reset();

	// Connect to file-server
	TInt error = iFsSession.Connect();
	if(error != KErrNone)
	{
		return error;
	}

	TFileName path1 =  PathInfo::PhoneMemoryRootPath();
	TFileName path2 = PathInfo::MemoryCardRootPath();
	switch (iDirectory)
	{
	case  EFileBrowserPictures:
		path1.Append(KDirPictures);
		path2.Append(KDirPictures);
		break;
	case  EFileBrowserVideos:
		path1.Append(KDirVideos);
		path2.Append(KDirVideos);
		break;
	case  EFileBrowserSounds:
	default:
		path1.Append(KDirSounds);
		path2.Append(KDirSounds);
		break;
	}

	TRAPD( err, GetResourceFilesL( path1 ));
	TRAPD( err2, GetResourceFilesL( path2 ));
	return err;
}

TInt CFileBrowserEngine::StartCurrentList()
{
	return iFsSession.Connect();
}

// ---------------------------------------------------------
// CFileBrowserEngine::GetResourceFilesL ( TFileName &aBasePath )
// The method get all files including files into subfolders.
// ---------------------------------------------------------
//
void CFileBrowserEngine::GetResourceFilesL ( TFileName &aBasePath )
{
	CDir* dirs = 0;
	CDir* files = 0;

	if(! BaflUtils::PathExists(iFsSession, aBasePath))
	{
		return;
	}

	// Get dir. KEntryAttNormal means that no hidden files or directories are included
	User::LeaveIfError(iFsSession.GetDir(aBasePath, KEntryAttNormal, ESortByName, files, dirs));

	CleanupStack::PushL(dirs);
	CleanupStack::PushL(files);

	//get files in base path
	for( TInt i = 0; i < files->Count(); i++ )
	{
		iFileList.Append( (*files)[i] );
		iDirList.Append( aBasePath );
	}

	//get subfolders
	for( TInt i = 0; i < dirs->Count(); i++ )
	{
		// Exclude directories beginning with '_' as they can
		// contain system-generated thumbnails etc.
		if( (*dirs)[i].iName[0] != '_')
		{
			TFileName nextDir = aBasePath;
			nextDir.Append( (*dirs)[i].iName );
			nextDir.Append( KSlash );
			GetResourceFilesL( nextDir );
		}
	}
	// Number 2 cannot be considered a "magic number" in this case,
	// and PopAndDestroy does get a parameter. Thus, the CSIs.
	CleanupStack::PopAndDestroy( 2, dirs );  // CSI: 47,12 #
}

//////////////////////////////////////////////////////////////////////////
void CFileBrowserEngine::GetEntriesL(TFileName &aFolder)
{	
	__LOGSTR_TOFILE1("CFileBrowserEngine::GetEntriesL() begins with aFolder == %S", &aFolder);

	// If passed argument is empty
	// retrieve first level folders (i.e. memory root and flash images start folders)
	if (aFolder.Length() == 0)
	{
		iCurrentDirectory = TFileName(KNullDesC);

		iCurrentFiles.Reset();
		iCurrentFolders.Reset();

		TFileName phoneRoot =  PathInfo::PhoneMemoryRootPath();
		TFileName cardRoot = PathInfo::MemoryCardRootPath();

		phoneRoot.Append(KDirPictures);
		cardRoot.Append(KDirPictures);


		if (BaflUtils::PathExists(iFsSession, phoneRoot))
		{
			RDebug::Print(_L("Phone images root: %S"), &phoneRoot);

			TEntry phoneEntry;

			iFsSession.Entry(phoneRoot, phoneEntry);

			iCurrentFolders.Append(phoneEntry);
		}

		if (BaflUtils::PathExists(iFsSession, cardRoot))
		{
			RDebug::Print(_L("Card images root: %S"), &cardRoot);

			TEntry cardEntry;

			iFsSession.Entry(cardRoot, cardEntry);

			iCurrentFolders.Append(cardEntry);
		}
	}
	// Otherwise
	else
	{
		// If specified folder doesn't exist - just return
		if(!BaflUtils::PathExists(iFsSession, aFolder))
		{
			__LOGSTR_TOFILE1("CFileBrowserEngine::GetEntriesL() specifed folder doesn't exist: %S", &aFolder);

			RDebug::Print(_L("Specified folder doesn't exist: %S"), &aFolder);

			return;
		}

		// Otherwise retrieve lists of directories and files inside
		CDir* folders = 0;
		CDir* files = 0;

		iCurrentFolders.Reset();
		iCurrentFiles.Reset();

		// Get directory KEntryAttNormal means that no hidden files or directories are included
		User::LeaveIfError(iFsSession.GetDir(aFolder, KEntryAttNormal, ESortByName, files, folders));

		CleanupStack::PushL(folders);
		CleanupStack::PushL(files);

		// Get folders in the specified folder
		for( TInt i = 0; i < folders->Count(); i++ )
		{
			// Exclude directories beginning with '_' as they can
			// contain system-generated thumbnails etc.
			if( (*folders)[i].iName[0] != '_')
			{
				iCurrentFolders.Append( (*folders)[i] );
			}
		}

		// Get files in the specified folder
		for( TInt i = 0; i < files->Count(); i++ )
		{
#if USE_FILEBROWSER_FILTER
			if ((*files)[i].iName.Right(4) == KPngExt || (*files)[i].iName.Right(4) == KJpgExt)
			{
#endif
				iCurrentFiles.Append( (*files)[i] );

#if USE_FILEBROWSER_FILTER
			}
#endif
		}

		CleanupStack::PopAndDestroy( 2, folders);

		iCurrentDirectory = aFolder;
	}	

	__LOGSTR_TOFILE("CFileBrowserEngine::GetEntriesL() ends");
}

TInt CFileBrowserEngine::OpenOrLaunchCurrentL(TInt aPosition)
{
	__LOGSTR_TOFILE1("CFileBrowserEngine::OpenOrLaunchCurretnL() begins aPosition == %d", aPosition);

	// Return value
	TInt retValue = KErrNone;

	// Shift correction to the item position,
	// for the case when '...' item is used
	TInt posShift = 0;

	if (iCurrentDirectory.Length() > 0 && aPosition > 0)
	{
		posShift = 1;
	}	

	// If passed index is valid
	if(aPosition >= 0 && (aPosition < (iCurrentFiles.Count() + iCurrentFolders.Count() + posShift) || (iCurrentFolders.Count() + iCurrentFiles.Count()) == 0))
	{
		TFileName descr = iCurrentDirectory;

		// Open the file
		if (aPosition > iCurrentFolders.Count() && aPosition <= iCurrentFiles.Count()+iCurrentFiles.Count())
		{
			__LOGSTR_TOFILE("CFileBrowserEngine::OpenOrLaunchCurretnL() opens file");

			// Add filename to be launched
			descr.Append(iCurrentFiles[aPosition-iCurrentFolders.Count()-1].iName);

			// Create nullType. This makes the DocumentHandler to figure out the posfix for us.
			TDataType nullType;
			// Launch the appropriate application for this file
			iDocHandler->OpenFileEmbeddedL(descr, nullType);
			iDocHandler->SetExitObserver(this);
			iDocEmbedded = ETrue;
		}
		// Open folder
		else
		{
			// Go to upper level
			if (aPosition == 0 && iCurrentDirectory.Length() > 0)
			{
				// If we are in the KDirPictures directory
				// Go to first level
				TFileName phoneRoot =  PathInfo::PhoneMemoryRootPath();
				TFileName cardRoot = PathInfo::MemoryCardRootPath();

				phoneRoot.Append(KDirPictures);
				cardRoot.Append(KDirPictures);

				// Open root directory
				if (iCurrentDirectory == phoneRoot || iCurrentDirectory == cardRoot)
				{
					__LOGSTR_TOFILE("CFileBrowserEngine::OpenOrLaunchCurretnL() goes upper to root directory");

					TFileName firstDirectory = TFileName(KNullDesC);
					GetEntriesL(firstDirectory);
				}
				// Otherwise
				// just go the upper level
				else
				{
					// Delete the last one directory in the path
					iCurrentDirectory.Delete(iCurrentDirectory.Length()-1, 1);

					TInt slashPos = iCurrentDirectory.LocateReverse('\\');

					if (slashPos >= 0 && slashPos < iCurrentDirectory.Length()-1)
					{
						iCurrentDirectory.Delete(slashPos+1, iCurrentDirectory.Length()-slashPos-1);
					}							

					__LOGSTR_TOFILE1("CFileBrowserEngine::OpenOrLaunchCurretnL() goes upper to directory %S", &iCurrentDirectory);

					RDebug::Print(_L("Formed folder: %S"), &iCurrentDirectory);

					GetEntriesL(iCurrentDirectory);
				}
			}
			// Open directory
			else
			{	// If the current directory is the first folder
				if (iCurrentDirectory.Length() == 0)
				{
					TFileName folderPath = TFileName(KNullDesC);

					if (aPosition == 0)
					{
						folderPath = PathInfo::PhoneMemoryRootPath();
					}
					else if (aPosition == 1)
					{
						folderPath = PathInfo::MemoryCardRootPath();										
					}

					folderPath.Append(iCurrentFolders[aPosition].iName);

					folderPath.Append(KSlash);

					__LOGSTR_TOFILE1("CFileBrowserEngine::OpenOrLaunchCurretnL() opens folder %S", &folderPath);

					RDebug::Print(_L("Formed folder: %S"), &folderPath);

					GetEntriesL(folderPath);
				}
				// Otherwise
				else
				{
					TFileName folderPath = iCurrentDirectory;

					folderPath.Append(iCurrentFolders[aPosition-1].iName);

					folderPath.Append(KSlash);

					__LOGSTR_TOFILE1("CFileBrowserEngine::OpenOrLaunchCurretnL() opens folder %S", &folderPath);

					RDebug::Print(_L("Formed folder: %S"), &folderPath);

					GetEntriesL(folderPath);
				}				
			}
		}
	}
	// Otherwise
	else
	{
		retValue = KErrUnknown;
	}	

	__LOGSTR_TOFILE("CFileBrowserEngine::OpenOrLaunchCurretnL() ends");

	return retValue;
}

TBool CFileBrowserEngine::IsDir(TInt aPosition)
{
	if (aPosition == 0)
		return ETrue;

	if (aPosition > 0 && aPosition <= iCurrentFolders.Count())
		return ETrue;

	if (aPosition > iCurrentFolders.Count() && aPosition <= iCurrentFolders.Count()+iCurrentFiles.Count() )
		return EFalse;

	return EFalse;
}

void CFileBrowserEngine::GetCurrentItemsL(CDesCArray* aItems)
{
	__LOGSTR_TOFILE("CFileBrowserEngine::GetCurrentItemsL() begins");

	aItems->Reset();

	// If the current directory is not the first one,
	// add '...' entry as the first element of the listbox
	if (iCurrentDirectory.Length() > 0)
	{
		TFileName upperLevel(KNullDesC);
		upperLevel.Format(KStringUp, 2);
		aItems->AppendL(upperLevel);
	}

	// Retrieve folders names
	TFileName folderName(KNullDesC);

	if (iCurrentDirectory.Length() == 0)
	{
		// Get CCoeEnv instance
		CEikonEnv* eikonEnv = CEikonEnv::Static();

		HBufC* stringHolder1 = StringLoader::LoadL(R_FILEBROWSER_PHONEMEMORY, eikonEnv );
		HBufC* stringHolder2 = StringLoader::LoadL(R_FILEBROWSER_CARDMEMORY, eikonEnv );

		for (TInt i = 0; i < iCurrentFolders.Count(); i++)
		{
			// As icon we use folder icon (index 0)
			if ( i == 0)
			{
				TFileName fileName = iCurrentFolders[i].iName;

				fileName.Append(*stringHolder1);

				folderName.Format(KStringIcon, 3, &fileName);
			}
			else
			{
				TFileName fileName = iCurrentFolders[i].iName;

				fileName.Append(*stringHolder2);

				folderName.Format(KStringIcon, 4, &fileName);
			}

			aItems->AppendL(folderName);
		}

		delete stringHolder2;
		stringHolder2 = NULL;

		delete stringHolder1;
		stringHolder1 = NULL;
	}
	else
	{
		for (TInt i = 0; i < iCurrentFolders.Count(); i++)
		{
			// As icon we use folder icon (index 0)
			folderName.Format(KStringIcon, 0, &iCurrentFolders[i].iName);
			aItems->AppendL(folderName);
		}

	}

	// Retrieve files names
	TFileName fileName(KNullDesC);

	for (TInt i = 0; i < iCurrentFiles.Count(); i++)
	{
		// As folder we use file icon (index 1)
		fileName.Format(KStringIcon, 1, &iCurrentFiles[i].iName);
		aItems->AppendL(fileName);
	}

	__LOGSTR_TOFILE("CFileBrowserEngine::GetCurrentItemsL() ends");
}

TBool CFileBrowserEngine::RemoveCurrentItems(CDesCArray* aItems)
{
	__LOGSTR_TOFILE("CFileBrowserEngine::RemoveCurrentItemsL() begins");

	if (iCurrentFolders.Count()+iCurrentFiles.Count())
	{
		if (iCurrentDirectory.Length() == 0)
		{
			aItems->Delete(0, (iCurrentFolders.Count()+iCurrentFiles.Count()));
		}
		else
		{
			aItems->Delete(0, (iCurrentFolders.Count()+iCurrentFiles.Count()+1));
		}

		__LOGSTR_TOFILE("CFileBrowserEngine::RemoveCurrentItemsL() returns ETrue");

		return ETrue;
	}

	__LOGSTR_TOFILE("CFileBrowserEngine::RemoveCurrentItemsL() returns EFalse");

	return EFalse;
}

//////////////////////////////////////////////////////////////////////////

TFileName CFileBrowserEngine::GetFileNameL(TInt aPosition)
{
	__LOGSTR_TOFILE1("CFileBrowserEngine::GetFileNameL() begins with aPosition == %d", aPosition);

	TFileName filePath(KNullDesC);//= iDirList[aPosition];

	if(aPosition <= iFileList.Count())
	{
		// Get specified file's full path
		filePath.Append(iFileList[aPosition].iName);
	}
	else
	{
		__LOGSTR_TOFILE("CFileBrowserEngine::GetFileNameL() ends with panic == EFileBrowserInvalidIndex");

		Panic(EFileBrowserIvalidIndex);
	}

	__LOGSTR_TOFILE("CFileBrowserEngine::GetFileNameL() ends");

	return filePath;
}

void CFileBrowserEngine::GetFileDescriptionL(const TInt &aPosition, TEntry &aFileEntry, TFileName &aFolder)
{
	__LOGSTR_TOFILE("CFileBrowserEngine::GetFileDescriptionL() begins");

	if(aPosition <= iFileList.Count())
	{
		// Get specified file description
		aFileEntry = iFileList[aPosition];
		aFolder.Copy(iDirList[aPosition]);
	}
	else
	{
		__LOGSTR_TOFILE("CFileBrowserEngine::GetFileDescriptionL() ends with panic EFileBrowserInvalidIndex");

		Panic(EFileBrowserIvalidIndex);
	}

	__LOGSTR_TOFILE("CFileBrowserEngine::GetFileDescriptionL() ends");
}

void CFileBrowserEngine::GetFileDescription2L(const TInt &aPosition, TEntry &aFileEntry, TFileName &aFolder)
{
	__LOGSTR_TOFILE("CFileBrowserEngine::GetFileDescription2L() begins");

	if(aPosition >= iCurrentFolders.Count() && aPosition <= (iCurrentFolders.Count() + iCurrentFiles.Count()))
	{
		// Get specified file description
		if (iCurrentDirectory.Length() == 0)
			aFileEntry = iCurrentFiles[aPosition-iCurrentFolders.Count()];
		else
			aFileEntry = iCurrentFiles[aPosition-iCurrentFolders.Count()-1];

		aFolder.Copy(iCurrentDirectory);
	}
	else
	{
		__LOGSTR_TOFILE("CFileBrowserEngine::GetFileDescription2L() ends with panic EFileBrowserInvalidIndex");

		Panic(EFileBrowserIvalidIndex);
	}

	__LOGSTR_TOFILE("CFileBrowserEngine::GetFileDescription2L() ends");
}

// ---------------------------------------------------------
// CFileBrowserEngine::GetFileListItems(CDesCArray* aItems)
// This Method constructs the listbox items with directory
// information
// ---------------------------------------------------------
//
void CFileBrowserEngine::GetFileListItemsL(CDesCArray* aItems)
{

	for (TInt i = 0; i < iFileList.Count(); i++)
	{
		TFileName filename(KNullDesC);
		if(iSizeDate == EFileBrowserSize)
		{
			// Show file size
			filename.Format(KStringSize,i+1, &iFileList[i].iName, iFileList[i].iSize);
		}
		else
		{
			// Fix the date and time string of last modification
			TBuf<KDateLength> dateString;
			_LIT(KDateString, "%D%M%Y%/0%1%/1%2%/2%3%/3 %-B%:0%J%:1%T%:2%S%:3%+B");
			iFileList[i].iModified.FormatL(dateString, KDateString);

			// Show date modified
			filename.Format(KStringDate, i+1, &iFileList[i].iName, &dateString);
		}
		aItems->AppendL(filename);
	}
}

// ---------------------------------------------------------
// CFileBrowserEngine::SetDirectory(TInt aDirectory)
// This Method sets which directory to list.
// ---------------------------------------------------------
//
void CFileBrowserEngine::SetDirectory(TInt aDirectory)
{
	if (aDirectory != EFileBrowserDirNoChange)
		iDirectory = aDirectory;
}

// ---------------------------------------------------------
// CFileBrowserEngine::LaunchCurrent(TInt aPosition)
// This Method launches selected item with DocumentHandler.
// DocumentHandler will launch correct application depending
// on the file type.
// Note that all the extensions do not work on emulator.
// ---------------------------------------------------------
//
void CFileBrowserEngine::LaunchCurrentL(TInt aPosition)
{
	TFileName descr = iDirList[aPosition];

	if(aPosition <= iFileList.Count())
	{
		// Add filename to be launched
		descr.Append(iFileList[aPosition].iName);
	}
	else
	{
		Panic(EFileBrowserIvalidIndex);
	}

	// Create nullType. This makes the DocumentHandler to figure out the postfix for us.
	TDataType nullType;
	// Launch the appropriate application for this file
	iDocHandler->OpenFileEmbeddedL(descr, nullType);
	iDocHandler->SetExitObserver(this);
	iDocEmbedded = ETrue;
};

void CFileBrowserEngine::LaunchCurrentL(const TFileName &aFilePath)
{	
	// If specified file exists
	if (BaflUtils::FileExists(iFsSession, aFilePath))
	{
		// Create nullType. This makes the DocumentHandler to figure out the postfix for us.
		TDataType nullType;
		// Launch the appropriate application for this file
		iDocHandler->OpenFileEmbeddedL(aFilePath, nullType);

		iDocHandler->SetExitObserver(this);

		iDocEmbedded = ETrue;
	}
}

// ---------------------------------------------------------
// CFileBrowserEngine::RemoveItems(CDesCArray* aItems)
// This Method removes all listbox items when a new list
// needs to be shown.
// ---------------------------------------------------------
//
TBool CFileBrowserEngine::RemoveItems(CDesCArray* aItems)
{
	if (iFileList.Count())
	{
		aItems->Delete(0, (iFileList.Count()));
		return ETrue;
	}
	return EFalse;
};

// ---------------------------------------------------------
// CFileBrowserEngine::IsDirListEmpty()
// ---------------------------------------------------------
//
TBool CFileBrowserEngine::IsDirListEmpty()
{
	if(iFileList.Count())
	{
		return EFalse;
	}
	return ETrue;
};

// ---------------------------------------------------------
// CFileBrowserEngine::SetSizeDate(TInt aSizeDate)
// This Method sets whether modification date or file size
// is shown. There is also option for toggling the status.
// ---------------------------------------------------------
//
void CFileBrowserEngine::SetSizeDate(TInt aSizeDate)
{
	if (aSizeDate == EFileBrowserToggle)
		if (iSizeDate == EFileBrowserSize)
			iSizeDate = EFileBrowserDate;
		else
			iSizeDate = EFileBrowserSize;
	else
		if (aSizeDate != EFileBrowserSizeDateNoChange)
			iSizeDate = aSizeDate;
};

// ---------------------------------------------------------
// CFileBrowserEngine::EndFileList()
// This Method ends the FileList session
// ---------------------------------------------------------
//
void CFileBrowserEngine::EndFileList()
{
	// Close the file server session
	iFsSession.Close();
};

#ifdef __SERIES60_3X__
void CFileBrowserEngine::HandleServerAppExit(TInt aReason)
{
	iDocEmbedded = EFalse;
	MAknServerAppExitObserver::HandleServerAppExit(aReason);
}
#else
void CFileBrowserEngine::NotifyExit(TExitMode /*aMode*/)
{
	iDocEmbedded = EFalse;
}
#endif