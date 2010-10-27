#ifndef __FRAMEWORK_ENCODER_OBJECT_WM__
#define __FRAMEWORK_ENCODER_OBJECT_WM__

#include "PhotoLibrary.h"
#include "VList.h"

//Image img = pictureBox1.Image
//MemoryStream ms = new MemoryStream
//img.Save(ms, ImageFormat.Jpeg) 'not that you can save it as a different format if you'd like
//Byte buf[]= ms.GetBuffer()
//ms.Close()


class PhotoLibraryWM : public PhotoLibrary
{
public:
	PhotoLibraryWM();
	virtual ~PhotoLibraryWM();

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


