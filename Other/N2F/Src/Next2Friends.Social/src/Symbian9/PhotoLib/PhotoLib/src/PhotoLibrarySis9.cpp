#include <PathInfo.h>
#include "e32cmn.h"
#include <f32file.h>
#include "eikappui.h"
#include "eikapp.h"
#include "aknutils.h"

#include "Application.h"
#include "FileSystem.h"
#include "PhotoLibrarySis9.h"
#include "LibPhotoItem.h"



PhotoLibrarySis::PhotoLibrarySis()
{

}

PhotoLibrarySis::~PhotoLibrarySis()
{
	while (freeItems.Size())
	{
		LibPhotoItem *pi = (LibPhotoItem*)(*freeItems.Begin());
		SAFE_DELETE(pi);
		freeItems.Erase(freeItems.Begin());
	}
}

const LibPhotoItem * PhotoLibrarySis::GetFirstPhoto( ePhotoSourceType sourceType )
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


const LibPhotoItem * PhotoLibrarySis::GetNextPhoto()
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

File* PhotoLibrarySis::GetPhotoFile( const LibPhotoItem *item )
{
	FileSystem *pFs = GetApplication()->GetFileSystem();
	char8 fileName[MAX_PATH];
	Utils::WstrToStr((char16*)item->GetID(), fileName, item->GetIDSize() * 2);

	return pFs->Open(fileName, File::EFM_READ);
}


void PhotoLibrarySis::RecreatePhotoList( ePhotoSourceType sourceType )
{

	for (VList::Iterator it = photoItems[sourceType].Begin(); it != photoItems[sourceType].End(); it++)
	{
		LibPhotoItem *pi = (LibPhotoItem*)(*it);
		pi->SetState(EPIS_UNCONFIRMED);
	}





	char16 searchPath[MAX_PATH];
	if (sourceType == EPST_PHONE)
	{
		Utils::WSPrintf(searchPath, MAX_PATH * 2, (char16*)L"%s\\%s", PathInfo::PhoneMemoryRootPath().Ptr(), PathInfo::ImagesPath().Ptr());
	}
	else
	{
		Utils::WSPrintf(searchPath, MAX_PATH * 2, (char16*)L"%s\\%s", PathInfo::MemoryCardRootPath().Ptr(), PathInfo::ImagesPath().Ptr());
	}
	ScanForFiles(sourceType, searchPath);



	ClearUncomfirmed(sourceType);
	needRefresh[sourceType] = false;
	
}

void PhotoLibrarySis::ScanForFiles(ePhotoSourceType sourceType, char16* sourcePath)
{
	
	UTILS_LOG(EDMP_DEBUG, "ScanForFiles: init for files %s",  sourcePath);
	char16 findPath[MAX_PATH];
	if (sourcePath[Utils::WStrLen(sourcePath) - 1] == (char16)L'\\')
	{
		Utils::WSPrintf(findPath, MAX_PATH * 2, (char16*)L"%s", sourcePath);
	}
	else
	{
		Utils::WSPrintf(findPath, MAX_PATH * 2, (char16*)L"%s\\", sourcePath);
	}

	CDir* iFileList;
	CDir* iDirList;
	RFs *pFS = &CCoeEnv::Static()->FsSession();

	TInt result = pFS->GetDir(TPtrC((TUint16 *)findPath),
		KEntryAttNormal, //| KEntryAttHidden | KEntryAttSystem,
		ESortByName | EDirsFirst | EAscending,
		iFileList,
		iDirList);

	for (int i = 0; i < iFileList->Count(); i++)
	{
		char16 *name = (char16*)(iFileList->operator[](i).iName.Ptr());
		int32 fileSize = (iFileList->operator[](i).iSize);

		char16 *pointPtr = name + Utils::WStrLen(name);
		UTILS_LOG(EDMP_DEBUG, "ScanForFiles: file %S",  name);
		while (pointPtr != name)
		{
			pointPtr--;
			if (L'.' == *pointPtr)
			{
				pointPtr++;
				break;
			}
		}

		if (pointPtr != name && (!Utils::WStrCmp(pointPtr, (char16*)L"jpg") || !Utils::WStrCmp(pointPtr, (char16*)L"JPG")))
		{
			bool isPresent = false;
			for (VList::Iterator it = photoItems[sourceType].Begin(); it != photoItems[sourceType].End(); it++)
			{
				LibPhotoItem *pi = (LibPhotoItem*)(*it);
				if (pi->GetSize() == fileSize && !Utils::WStrCmp((const char16*)pi->GetID(), name))
				{
					pi->SetState(EPIS_PHOTO);
					isPresent = true;
					break;
				}
			}
			if (!isPresent)
			{
				UTILS_LOG(EDMP_DEBUG, "ScanForFiles: add to list %S",  name);

				LibPhotoItem *pi = GetFreeItem();
				pi->SetState(EPIS_PHOTO);
				char16 newID[MAX_PATH];
				//if (sourcePath[Utils::WStrLen(sourcePath) - 1] == (char16)L'\\')
				//{
				//	Utils::WSPrintf(newID, MAX_PATH * 2, (char16*)L"%s%s", sourcePath, (char16*)findInfo.cFileName);
				//}
				//else
				//{
				//	Utils::WSPrintf(newID, MAX_PATH * 2, (char16*)L"%s\\%s", sourcePath, (char16*)findInfo.cFileName);
				//}
				pi->SetID((char8*)name, Utils::WStrLen(name) * 2 + 2);
				pi->SetSize(fileSize);
				char16* namePtr = name + Utils::WStrLen(name);
				while (namePtr != name)
				{
					namePtr--;
					if ((char16)L'\\' == *namePtr)
					{
						namePtr++;
						break;
					}
				}
				pi->SetName(name);
				
				TTime fileTime = (iFileList->operator[](i).iModified);
				TDateTime fileDT = fileTime.DateTime();
				PhotoDate pd;
				pd.day = (int8)fileDT.Day();
				pd.hour = (int8)fileDT.Hour();
				pd.minute = (int8)fileDT.Minute();
				pd.month = (int8)fileDT.Month();
				pd.year = fileDT.Year();
				pi->SetDate(pd);
				photoItems[sourceType].PushBack(pi);
			}
		}
	}
	delete iFileList;

	for (int i = 0; i < iDirList->Count(); i++)
	{
		char16 *name = (char16*)(iFileList->operator[](i).iName.Ptr());
		ScanForFiles(sourceType, name);
	}
	delete iDirList;

}


LibPhotoItem * PhotoLibrarySis::GetFreeItem()
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

void PhotoLibrarySis::ClearUncomfirmed(ePhotoSourceType sourceType)
{
	/*
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
	*/
}

const LibPhotoItem * PhotoLibrarySis::GetByID(const char8 *id, int32 sizeInBytes)
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
		if ((char16)L'\\' == *namePtr)
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

LibPhotoItem * PhotoLibrarySis::MoveToLibrary( ePhotoSourceType target, char8 *fileName )
{
	/*
	//gets full path to the file
	char16 fullPath[MAX_PATH];
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

	RFs *pFS = &CCoeEnv::Static()->FsSession();
	if (ptr != fileName)
	{
		Utils::StrToWstr(fileName, fullPath, MAX_PATH * 2);
	}
	else
	{

		TFileName sSesPath;
		pFS->SessionPath(sSesPath);
		char16 fName[MAX_PATH];
		Utils::StrToWstr(fileName, fName, MAX_PATH * 2);
		Utils::WSPrintf(fullPath, MAX_PATH * 2, (char16*)L"%s\\%s", sSesPath.Ptr(), fName);
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
	if (target == EPST_PHONE)
	{
		Utils::WSPrintf(newPath, MAX_PATH * 2, (char16*)L"%s\\%s", PathInfo::PhoneMemoryRootPath().Ptr(), PathInfo::ImagesPath().Ptr());
	}
	else
	{
		Utils::WSPrintf(newPath, MAX_PATH * 2, (char16*)L"%s\\%s", PathInfo::MemoryCardRootPath().Ptr(), PathInfo::ImagesPath().Ptr());
	}
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
	pFS->Rename(TPtrC((TUint *)fullPath), TPtrC((TUint *)outPath));

	//initialize new LibPhotoItem
	LibPhotoItem *pi = GetFreeItem();
	pi->SetState(EPIS_PHOTO);
	pi->SetID((char8*)outPath, Utils::WStrLen((char16*)outPath) * 2 + 2);
	pi->SetName((char16*)newName);

	TEntry ent;
	pFS->Entry(TPtrC((TUint *)outPath), ent);



	pi->SetSize(ent.iSize);
	TTime fileTime = ent.iModified;
	TDateTime fileDT = fileTime.DateTime();
	PhotoDate pd;
	pd.day = (int8)fileDT.Day();
	pd.hour = (int8)fileDT.Hour();
	pd.minute = (int8)fileDT.Minute();
	pd.month = (int8)fileDT.Month();
	pd.year = fileDT.Year();
	pi->SetDate(pd);
	photoItems[target].PushBack(pi);
	return pi;
	*/
	return NULL;
}






