#ifndef __FRAMEWORK_ENCODER_OBJECT_BREW__
#define __FRAMEWORK_ENCODER_OBJECT_BREW__

#include "PhotoLibrary.h"
#include "VList.h"
#include "AEEFile.h"



class PhotoLibraryBrew : public PhotoLibrary
{
public:
	PhotoLibraryBrew();
	virtual ~PhotoLibraryBrew();

	virtual const LibPhotoItem *GetFirstPhoto(ePhotoSourceType sourceType);
	virtual const LibPhotoItem *GetNextPhoto();
	virtual File* GetPhotoFile(const LibPhotoItem *item);
	virtual const LibPhotoItem *GetByID(const char8 *id, int32 sizeInBytes);

	virtual LibPhotoItem *MoveToLibrary(ePhotoSourceType target, char8 *fileName);


private:
	void RecreatePhotoList(ePhotoSourceType sourceType);
	void ClearUncomfirmed(ePhotoSourceType sourceType);
	LibPhotoItem *GetFreeItem();
	void ScanForFiles(ePhotoSourceType sourceType);

	IFileMgr *fileMgr;
	char8 tempPath[256];
	char8 oldPath[256];

	VList freeItems;
	VList photoItems[2];

	VList::Iterator itemIterator;

	ePhotoSourceType currentSource;
	int32 currentList;


};

#ifdef	AEE_SIMULATOR
const static char8 pathDir[2][30] = {"fs:/~/phone", "fs:/~/card"};
#else
const static char8 pathDir[2][30] = {"fs:/user", "fs:/card0"};
#endif

#endif


