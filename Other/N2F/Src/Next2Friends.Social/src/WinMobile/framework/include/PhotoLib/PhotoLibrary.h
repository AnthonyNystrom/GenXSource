#ifndef __FRAMEWORK_PHOTO_LIBRARY__
#define __FRAMEWORK_PHOTO_LIBRARY__

#include "BaseTypes.h"
#include "FileSystem.h"


enum ePhotoSourceType
{
	EPST_PHONE = 0,
	EPST_CARD,
	EPST_BOTH
};

class LibPhotoItem;

class PhotoLibrary
{
public:
	static PhotoLibrary *Create();

	virtual void Refresh();
	
	virtual const LibPhotoItem *GetFirstPhoto(ePhotoSourceType sourceType) = 0;
	virtual const LibPhotoItem *GetNextPhoto() = 0;
	virtual File *GetPhotoFile(const LibPhotoItem *item) = 0;
	virtual const LibPhotoItem *GetByID(const char8 *id, int32 sizeInBytes) = 0;

	virtual LibPhotoItem *MoveToLibrary(ePhotoSourceType target, char8 *fileName) = 0;


	virtual ~PhotoLibrary();

protected:
	PhotoLibrary();

	bool needRefresh[2];
	int32 photoCounter;
private:


};
#endif