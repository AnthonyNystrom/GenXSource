#include "PhotoLibraryWM.h"
#include "LibPhotoItem.h"
#include "Application.h"
#include "FileSystem.h"
#include <WinBase.h>
#include <shellapi.h>



PhotoLibraryWM::PhotoLibraryWM()
{

}

PhotoLibraryWM::~PhotoLibraryWM()
{
	while (freeItems.Size())
	{
		LibPhotoItem *pi = (LibPhotoItem*)(*freeItems.Begin());
		SAFE_DELETE(pi);
		freeItems.Erase(freeItems.Begin());
	}
}

const LibPhotoItem * PhotoLibraryWM::GetFirstPhoto( ePhotoSourceType sourceType )
{
	currentSource = sourceType;
	if (currentSource == EPST_BOTH)
	{
		currentList = 0;
		for (int i = 0; i < 2; i++)
		{
			if (needRefresh[i])
			{
				RecreatePhotoList((ePhotoSourceType)i);
			}
		}
	}
	else
	{
		if (needRefresh[currentSource])
		{
			RecreatePhotoList(currentSource);
		}
	}

	if (currentSource != EPST_BOTH)
	{
		currentList = currentSource;
	}

	itemIterator = photoItems[currentList].Begin();
	if (itemIterator == photoItems[currentList].End())
	{
		if (currentList == EPST_PHONE && currentSource == EPST_BOTH)
		{
			currentList++;
			itemIterator = photoItems[currentList].Begin();
			if (itemIterator == photoItems[currentList].End())
			{
				return NULL;
			}
		}
		else
		{
			return NULL;
		}
	}

	return (LibPhotoItem*)(*itemIterator);
}


const LibPhotoItem * PhotoLibraryWM::GetNextPhoto()
{
	itemIterator++;
	if (itemIterator == photoItems[currentList].End())
	{
		if (currentList == EPST_PHONE && currentSource == EPST_BOTH)
		{
			currentList++;
			itemIterator = photoItems[currentList].Begin();
			if (itemIterator == photoItems[currentList].End())
			{
				return NULL;
			}
		}
		else
		{
			return NULL;
		}
	}

	return (LibPhotoItem*)(*itemIterator);
}

File* PhotoLibraryWM::GetPhotoFile( const LibPhotoItem *item )
{
	FileSystem *pFs = GetApplication()->GetFileSystem();
	char8 fileName[MAX_PATH];
	Utils::WstrToStr((char16*)item->GetID(), fileName, item->GetIDSize() * 2);

	return pFs->Open(fileName, File::EFM_READ);
}


void PhotoLibraryWM::RecreatePhotoList( ePhotoSourceType sourceType )
{

	for (VList::Iterator it = photoItems[sourceType].Begin(); it != photoItems[sourceType].End(); it++)
	{
		LibPhotoItem *pi = (LibPhotoItem*)(*it);
		pi->SetState(EPIS_UNCONFIRMED);
	}





	char16 searchPath[MAX_PATH];
	if (sourceType == EPST_PHONE)
	{
		SHGetSpecialFolderPath( NULL, (LPTSTR) searchPath, CSIDL_MYPICTURES, 1);
	}
	else
	{
		SHGetSpecialFolderPath( NULL, (LPTSTR) searchPath, CSIDL_MYPICTURES, 1);
	}
	ScanForFiles(sourceType, searchPath);



	ClearUncomfirmed(sourceType);
	needRefresh[sourceType] = false;
}

void PhotoLibraryWM::ScanForFiles(ePhotoSourceType sourceType, char16* sourcePath)
{
	//int32 tlen = Utils::StrLen(tempPath);
	//if (tempPath[tlen - 1] != '/')
	//{
	//	tempPath[tlen] = '/';
	//	tempPath[tlen + 1] = 0;
	//}
	UTILS_LOG(EDMP_DEBUG, "ScanForFiles: init for files %s",  sourcePath);
	char16 findPath[MAX_PATH];
	if (sourcePath[Utils::WStrLen(sourcePath) - 1] == (char16)L'\\')
	{
		Utils::WSPrintf(findPath, MAX_PATH * 2, (char16*)L"%s*", sourcePath);
	}
	else
	{
		Utils::WSPrintf(findPath, MAX_PATH * 2, (char16*)L"%s\\*", sourcePath);
	}

	WIN32_FIND_DATA findInfo;
	HANDLE searchHandle = FindFirstFile((LPCTSTR)findPath, &findInfo);
	if (searchHandle != INVALID_HANDLE_VALUE)
	{
		do
		{
			if (findInfo.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
			{//when we find folder we must scan it
				char16 newPath[MAX_PATH];
				if (sourcePath[Utils::WStrLen(sourcePath) - 1] == (char16)L'\\')
				{
					Utils::WSPrintf(newPath, MAX_PATH * 2, (char16*)L"%s%s", sourcePath, (char16*)findInfo.cFileName);
				}
				else
				{
					Utils::WSPrintf(newPath, MAX_PATH * 2, (char16*)L"%s\\%s", sourcePath, (char16*)findInfo.cFileName);
				}
				ScanForFiles(sourceType, newPath);
			}
			else
			{
				char16 *pointPtr = (char16*)findInfo.cFileName + Utils::WStrLen((char16*)findInfo.cFileName);
				UTILS_LOG(EDMP_DEBUG, "ScanForFiles: file %S",  (char16*)findInfo.cFileName);
				while (pointPtr != (char16*)findInfo.cFileName)
				{
					pointPtr--;
					if (L'.' == *pointPtr)
					{
						pointPtr++;
						break;
					}
				}
				if (pointPtr != (char16*)findInfo.cFileName && (!Utils::WStrCmp(pointPtr, (char16*)L"jpg") || !Utils::WStrCmp(pointPtr, (char16*)L"JPG")))
				{
					bool isPresent = false;
					for (VList::Iterator it = photoItems[sourceType].Begin(); it != photoItems[sourceType].End(); it++)
					{
						LibPhotoItem *pi = (LibPhotoItem*)(*it);
						if (pi->GetSize() == (findInfo.nFileSizeHigh * MAXDWORD+1) + findInfo.nFileSizeLow && !Utils::WStrCmp((const char16*)pi->GetID(), (char16*)findInfo.cFileName))
						{
							pi->SetState(EPIS_PHOTO);
							isPresent = true;
							break;
						}
					}
					if (!isPresent)
					{
						UTILS_LOG(EDMP_DEBUG, "ScanForFiles: add to list %S",  (char16*)findInfo.cFileName);

						LibPhotoItem *pi = GetFreeItem();
						pi->SetState(EPIS_PHOTO);
						char16 newID[MAX_PATH];
						if (sourcePath[Utils::WStrLen(sourcePath) - 1] == (char16)L'\\')
						{
							Utils::WSPrintf(newID, MAX_PATH * 2, (char16*)L"%s%s", sourcePath, (char16*)findInfo.cFileName);
						}
						else
						{
							Utils::WSPrintf(newID, MAX_PATH * 2, (char16*)L"%s\\%s", sourcePath, (char16*)findInfo.cFileName);
						}
						pi->SetID((char8*)newID, Utils::WStrLen(newID) * 2 + 2);
						pi->SetSize((findInfo.nFileSizeHigh * MAXDWORD+1) + findInfo.nFileSizeLow);
						pi->SetName((char16*)findInfo.cFileName);
						SYSTEMTIME tm;
						FileTimeToSystemTime(&findInfo.ftLastWriteTime, &tm);
						PhotoDate pd;
						pd.day = (int8)tm.wDay;
						pd.hour = (int8)tm.wHour;
						pd.minute = (int8)tm.wMinute;
						pd.month = (int8)tm.wMonth;
						pd.year = tm.wYear;
						pi->SetDate(pd);
						photoItems[sourceType].PushBack(pi);
					}
				}

			}

		}
		while(FindNextFile(searchHandle, &findInfo));
		
		FindClose(searchHandle);
		searchHandle = NULL;
	}
	else
	{
		UTILS_LOG(EDMP_DEBUG, "ScanForFiles: init FAILED");
	}

}


LibPhotoItem * PhotoLibraryWM::GetFreeItem()
{
	for (VList::Iterator it = freeItems.Begin(); it != freeItems.End(); it++)
	{
		LibPhotoItem *pi = (LibPhotoItem*)(*it);
		if (pi->GetState() == EPIS_EMPTY)
		{
			return pi;
		}
	}

	LibPhotoItem *pi = new LibPhotoItem();
	freeItems.PushBack(pi);

	return pi;
}

void PhotoLibraryWM::ClearUncomfirmed(ePhotoSourceType sourceType)
{
	for (VList::Iterator it = photoItems[sourceType].Begin(); it != photoItems[sourceType].End(); it++)
	{
		LibPhotoItem *pi = (LibPhotoItem*)(*it);
		if (pi->GetState() == EPIS_UNCONFIRMED)
		{
			pi->SetState(EPIS_EMPTY);
			photoItems[sourceType].Erase(it);
			it = photoItems[sourceType].Begin();
		}
	}
}

const LibPhotoItem * PhotoLibraryWM::GetByID(const char8 *id, int32 sizeInBytes)
{
	for (int i = 0; i< 2; i++)
	{
		for (VList::Iterator it = photoItems[i].Begin(); it != photoItems[i].End(); it++)
		{
			LibPhotoItem *pi = (LibPhotoItem*)(*it);
			if (!Utils::WStrCmp((char16*)pi->GetID(), (char16*)id))
			{
				return pi;
			}
		}
	}
	char16 *namePtr = (char16*)id + Utils::WStrLen((char16*)id) - 1;
	while (namePtr != (char16*)id)
	{
		namePtr--;
		if ('\\' == *namePtr)
		{
			namePtr++;
			break;
		}
	}
	LibPhotoItem *pi = GetFreeItem();
	pi->SetState(EPIS_PHOTO);
	pi->SetID(id, sizeInBytes);
	pi->SetSize(0);
	pi->SetName(namePtr);
	PhotoDate pd;
	pd.day = (int8)0;
	pd.hour = (int8)0;
	pd.minute = (int8)0;
	pd.month = (int8)0;
	pd.year = 0;
	pi->SetDate(pd);
	photoItems[EPST_PHONE].PushBack(pi);

	return pi;
}

LibPhotoItem * PhotoLibraryWM::MoveToLibrary( ePhotoSourceType target, char8 *fileName )
{
	//gets full path to the file
	WCHAR fullPath[MAX_PATH];
	const char *ptr = fileName + Utils::StrLen(fileName);
	while (ptr != fileName)
	{
		if (*ptr == '\\')
		{
			ptr++;
			break;
		}
		ptr--;
	}

	if (ptr != fileName)
	{
		MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, fileName, -1, fullPath, MAX_FILENAME_SIZE);
	}
	else
	{
		GetModuleFileName(NULL, fullPath, MAX_FILENAME_SIZE);
		int i = wcslen(fullPath);
		do
		{
			if(fullPath[i-1]==TEXT('\\'))
			{
				break;
			}
			i--;
		}
		while(i>=0);

		MultiByteToWideChar(CP_ACP, MB_PRECOMPOSED, fileName, -1, &fullPath[i], MAX_FILENAME_SIZE-i);
	}

	//initialize path in library
	WCHAR newName[256];
	bool isPresent = true;
	while (isPresent)
	{
		isPresent = false;
		Utils::WSPrintf((char16*)newName, 256 * 2, (char16*)L"n2f%0.5dimg.jpg", photoCounter);
		for (VList::Iterator it = photoItems[target].Begin(); it != photoItems[target].End(); it++)
		{
			LibPhotoItem *pi = (LibPhotoItem*)(*it);
			if (!Utils::WStrCmp((char16*)pi->GetName(), (char16*)newName))
			{
				isPresent = true;
				photoCounter++;
				break;
			}
		}
	}
	WCHAR newPath[MAX_PATH];
	SHGetSpecialFolderPath( NULL, (LPTSTR) newPath, CSIDL_MYPICTURES, 1);
	WCHAR outPath[MAX_PATH];
	if (newPath[Utils::WStrLen((char16*)newPath) - 1] == (char16)L'\\')
	{
		Utils::WSPrintf((char16*)outPath, MAX_PATH * 2, (char16*)L"%s%s", (char16*)newPath, (char16*)newName);
	}
	else
	{
		Utils::WSPrintf((char16*)outPath, MAX_PATH * 2, (char16*)L"%s\\%s", (char16*)newPath, (char16*)newName);
	}

	//saves file to library
	MoveFile(fullPath, outPath);

	//initialize new LibPhotoItem
	LibPhotoItem *pi = GetFreeItem();
	pi->SetState(EPIS_PHOTO);
	pi->SetID((char8*)outPath, Utils::WStrLen((char16*)outPath) * 2 + 2);
	pi->SetName((char16*)newName);

	HANDLE hFile = CreateFile(	outPath,				// file to open
		GENERIC_READ,			// open for reading
		FILE_SHARE_READ,				// share for reading
		NULL,					// default security
		OPEN_EXISTING,		// existing file only
		FILE_ATTRIBUTE_NORMAL,	// normal file
		NULL);


	BY_HANDLE_FILE_INFORMATION info;
	GetFileInformationByHandle(hFile, &info);
	pi->SetSize((info.nFileSizeHigh * MAXDWORD+1) + info.nFileSizeLow);
	SYSTEMTIME tm;
	FileTimeToSystemTime(&info.ftLastWriteTime, &tm);
	PhotoDate pd;
	pd.day = (int8)tm.wDay;
	pd.hour = (int8)tm.wHour;
	pd.minute = (int8)tm.wMinute;
	pd.month = (int8)tm.wMonth;
	pd.year = tm.wYear;
	pi->SetDate(pd);
	photoItems[target].PushBack(pi);
	CloseHandle(hFile);
	return pi;
}






