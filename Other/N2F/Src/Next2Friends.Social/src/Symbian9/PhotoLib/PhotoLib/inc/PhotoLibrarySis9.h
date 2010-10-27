#ifndef __FRAMEWORK_PHOTO_LIBRARY_SIS__
#define __FRAMEWORK_PHOTO_LIBRARY_SIS__

#include "PhotoLibrary.h"
#include "VList.h"

#define MAX_PATH	256


class PhotoLibrarySis : public PhotoLibrary
{
public:
	PhotoLibrarySis();
	virtual ~PhotoLibrarySis();

	virtual const LibPhotoItem *GetFirstPhoto(ePhotoSourceType sourceType);
	virtual const LibPhotoItem *GetNextPhoto();
	virtual File* GetPhotoFile(const LibPhotoItem *item);
	virtual const LibPhotoItem *GetByID(const char8 *id, int32 sizeInBytes);

	virtual LibPhotoItem *MoveToLibrary(ePhotoSourceType target, char8 *fileName);


private:
	void RecreatePhotoList(ePhotoSourceType sourceType);
	void ClearUncomfirmed(ePhotoSourceType sourceType);
	LibPhotoItem *GetFreeItem();
	void ScanForFiles(ePhotoSourceType sourceType, char16* sourcePath);


	VList freeItems;
	VList photoItems[2];

	VList::Iterator itemIterator;

	ePhotoSourceType currentSource;
	int32 currentList;


};

const static char8 pathDir[2][30] = {"\\My Documents\\My Pictures", "\\My Documents\\My Pictures"};

#endif


