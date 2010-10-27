#ifndef __GUI_PHOTO_IMAGE__
#define __GUI_PHOTO_IMAGE__

#include "GUIControlContainer.h"
#include "Graphics.h"
#include "EncoderObject.h"

class GUIImage;
class ApplicationManager;
class LibPhotoItem;
class SurfacesPool;

class GUIPhotoImage : public GUIControlContainer, EncoderListener
{
public:
	GUIPhotoImage(int16 waitImageID, int16 badImageID, SurfacesPool *surfPool, ApplicationManager *manager, GUIControlContainer *parent = NULL, const ControlRect &rc = ControlRect());
	virtual ~GUIPhotoImage(void);

	virtual void Update();
	virtual void Draw();
	virtual void DrawFinished();

	void SetPhotoItem(const LibPhotoItem *newPhotoItem);
	void SetPhotoBuffer(uint8 *pBuffer, int32 len);

	void CancelEncoding();

	void NeedUnload(bool isNeed);

	bool IsError()
	{
		return isError;
	}

protected:
	virtual void OnEncodingSuccess(int32 size);
	virtual void OnEncodingCanceled();
	virtual void OnEncodingFailed();

private:
	GUIImage *image;
	SurfacesPool *pool;
	GraphicsSystem::Surface *surf;
	const LibPhotoItem *item;
	uint8	*buffer;
	int32 length;
	ApplicationManager *pManager;

	int16 waitID;
	int16 badID;
	File *curFile;
	bool isEncoding;
	bool isError;
	int32 appearCounter;

	int32 oldSize;
	bool needUnload;


};


#endif//__GUI_BUTTON__