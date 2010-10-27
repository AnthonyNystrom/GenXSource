#include "PhotoLibraryBrew.h"
#include "LibPhotoItem.h"
#include "Application.h"
#include "FileSystem.h"

#include "AEEAppGen.h"
#include "AEEStdLib.h"


PhotoLibraryBrew::PhotoLibraryBrew()
{

}

PhotoLibraryBrew::~PhotoLibraryBrew()
{
	while (freeItems.Size())
	{
		LibPhotoItem *pi = (LibPhotoItem*)(*freeItems.Begin());
		SAFE_DELETE(pi);
		freeItems.Erase(freeItems.Begin());
	}
}

const LibPhotoItem * PhotoLibraryBrew::GetFirstPhoto( ePhotoSourceType sourceType )
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


const LibPhotoItem * PhotoLibraryBrew::GetNextPhoto()
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

File* PhotoLibraryBrew::GetPhotoFile( const LibPhotoItem *item )
{
	FileSystem *pFs = GetApplication()->GetFileSystem();

	return pFs->Open(item->GetID(), File::EFM_READ);
}


void PhotoLibraryBrew::RecreatePhotoList( ePhotoSourceType sourceType )
{

	for (VList::Iterator it = photoItems[sourceType].Begin(); it != photoItems[sourceType].End(); it++)
	{
		LibPhotoItem *pi = (LibPhotoItem*)(*it);
		pi->SetState(EPIS_UNCONFIRMED);
	}

	AEEApplet * app = (AEEApplet*)GET_APP_INSTANCE();
	UTILS_LOG(EDMP_DEBUG, "PhotoLibraryBrew: ----------- RecreatePhotoList ------------------");

	int32 res;
	res = ISHELL_CreateInstance(app->m_pIShell, AEECLSID_FILEMGR,(void **)&fileMgr);
	if (!fileMgr || res != SUCCESS)
	{
		UTILS_LOG(EDMP_DEBUG, "PhotoLibraryBrew: Failed to create file manager");
	}


	Utils::StrCpy(tempPath, pathDir[sourceType]);

	ScanForFiles(sourceType);


	IFILEMGR_Release((IFileMgr*)fileMgr);

	ClearUncomfirmed(sourceType);
	needRefresh[sourceType] = false;
}

void PhotoLibraryBrew::ScanForFiles(ePhotoSourceType sourceType)
{
	int32 tlen = Utils::StrLen(tempPath);
	if (tempPath[tlen - 1] != '/')
	{
		tempPath[tlen] = '/';
		tempPath[tlen + 1] = 0;
	}
	UTILS_LOG(EDMP_DEBUG, "ScanForFiles: init for files %s",  tempPath);
	if (!Utils::StrCmp(tempPath, "fs:/mod/"))
	{
		UTILS_LOG(EDMP_DEBUG, "ScanForFiles: skipping mod dir");
		return;
	}
	if (IFILEMGR_EnumInit(fileMgr, tempPath, false) == SUCCESS)
	{

		FileInfo info;
		while (IFILEMGR_EnumNext(fileMgr, &info))
		{
			char8 *pointPtr = info.szName + Utils::StrLen(info.szName);
			UTILS_LOG(EDMP_DEBUG, "ScanForFiles: file %s",  info.szName);
			while (pointPtr != info.szName)
			{
				pointPtr--;
				if ('.' == *pointPtr)
				{
					pointPtr++;
					break;
				}
			}
			if (pointPtr == info.szName || (Utils::StrCmp(pointPtr, "jpg") && Utils::StrCmp(pointPtr, "JPG")))
			{
				continue;
			}

			bool isPresent = false;
			for (VList::Iterator it = photoItems[sourceType].Begin(); it != photoItems[sourceType].End(); it++)
			{
				LibPhotoItem *pi = (LibPhotoItem*)(*it);
				if (pi->GetSize() == info.dwSize && !Utils::StrCmp(pi->GetID(), info.szName))
				{
					pi->SetState(EPIS_PHOTO);
					isPresent = true;
					break;
				}
			}
			if (!isPresent)
			{
				UTILS_LOG(EDMP_DEBUG, "ScanForFiles: add to list %s",  info.szName);
				char8 *namePtr = info.szName + Utils::StrLen(info.szName) - 1;
				while (namePtr != info.szName)
				{
					namePtr--;
					if ('/' == *namePtr)
					{
						namePtr++;
						break;
					}
				}
				LibPhotoItem *pi = GetFreeItem();
				pi->SetState(EPIS_PHOTO);
				pi->SetID(info.szName, Utils::StrLen(info.szName) + 1);
				pi->SetSize(info.dwSize);
				pi->SetName(namePtr);
				JulianType jd;
				GETJULIANDATE(info.dwCreationDate, &jd);
				PhotoDate pd;
				pd.day = (int8)jd.wDay;
				pd.hour = (int8)jd.wHour;
				pd.minute = (int8)jd.wMinute;
				pd.month = (int8)jd.wMonth;
				pd.year = jd.wYear;
				pi->SetDate(pd);
				photoItems[sourceType].PushBack(pi);
			}
		}
	}
	else
	{
		UTILS_LOG(EDMP_DEBUG, "ScanForFiles: init FAILED",  tempPath);
	}



	UTILS_LOG(EDMP_DEBUG, "ScanForFiles: init for directories %s",  tempPath);
	if (IFILEMGR_EnumInit(fileMgr, tempPath, true) == SUCCESS)
	{
		FileInfo info;
		bool isReady = true;
		while (IFILEMGR_EnumNext(fileMgr, &info))
		{
			UTILS_LOG(EDMP_DEBUG, "ScanForFiles: directory %s",  info.szName);
			if (isReady)
			{
				Utils::StrCpy(tempPath, info.szName);
				ScanForFiles(sourceType);
				Utils::StrCpy(oldPath, tempPath);
				oldPath[Utils::StrLen(oldPath) - 1] = 0;
				char8 *namePtr = tempPath + Utils::StrLen(tempPath) - 1;
				while (namePtr != tempPath)
				{
					namePtr--;
					if ('/' == *namePtr)
					{
						namePtr++;
						*namePtr = 0;
						break;
					}
				}
				UTILS_LOG(EDMP_DEBUG, "ScanForFiles: return to directory %s",  tempPath);
				if (IFILEMGR_EnumInit(fileMgr, tempPath, true) != SUCCESS)
				{
					UTILS_LOG(EDMP_DEBUG, "ScanForFiles: init FAILED",  tempPath);
					return;
				}
				isReady = false;
			}
			else
			{
				if (!Utils::StrCmp(info.szName, oldPath))
				{
					isReady = true;
				}
			}
		}
	}
	else
	{
		UTILS_LOG(EDMP_DEBUG, "ScanForFiles: init FAILED",  tempPath);
	}
}


LibPhotoItem * PhotoLibraryBrew::GetFreeItem()
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

void PhotoLibraryBrew::ClearUncomfirmed(ePhotoSourceType sourceType)
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

const LibPhotoItem * PhotoLibraryBrew::GetByID(const char8 *id, int32 sizeInBytes)
{
	for (int i = 0; i< 2; i++)
	{
		for (VList::Iterator it = photoItems[i].Begin(); it != photoItems[i].End(); it++)
		{
			LibPhotoItem *pi = (LibPhotoItem*)(*it);
			if (!Utils::StrCmp(pi->GetID(), id))
			{
				return pi;
			}
		}
	}
	char8 *namePtr = (char8*)id + Utils::StrLen(id) - 1;
	while (namePtr != id)
	{
		namePtr--;
		if ('/' == *namePtr)
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

LibPhotoItem * PhotoLibraryBrew::MoveToLibrary( ePhotoSourceType target, char8 *fileName )
{
	char16 newName[256];
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
	char8 outPath[256];
	char8 outName[256];
	Utils::WstrToStr(newName, outName, 256*2);

	Utils::SPrintf(outPath, "%s/%s", pathDir[target], outName);

	AEEApplet * app = (AEEApplet*)GET_APP_INSTANCE();
	int32 res;
	res = ISHELL_CreateInstance(app->m_pIShell, AEECLSID_FILEMGR,(void **)&fileMgr);
	if (!fileMgr || res != SUCCESS)
	{
		UTILS_LOG(EDMP_DEBUG, "PhotoLibraryBrew: Failed to create file manager");
		return NULL;
	}

	IFILEMGR_Rename(fileMgr, fileName, outPath);

	LibPhotoItem *pi = GetFreeItem();
	pi->SetState(EPIS_PHOTO);
	pi->SetID(outPath, Utils::StrLen(outPath) + 1);
	pi->SetName(newName);
	FileInfo info;
	IFILEMGR_GetInfo(fileMgr, outPath, &info);
	pi->SetSize(info.dwSize);
	JulianType jd;
	GETJULIANDATE(info.dwCreationDate, &jd);
	PhotoDate pd;
	pd.day = (int8)jd.wDay;
	pd.hour = (int8)jd.wHour;
	pd.minute = (int8)jd.wMinute;
	pd.month = (int8)jd.wMonth;
	pd.year = jd.wYear;
	pi->SetDate(pd);
	photoItems[target].PushBack(pi);


	IFILEMGR_Release((IFileMgr*)fileMgr);

	return NULL;
}






