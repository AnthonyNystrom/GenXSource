#ifndef __FRAMEWORK_ENCODER_OBJECT__
#define __FRAMEWORK_ENCODER_OBJECT__

#include "BaseTypes.h"
#include "Graphics.h"


enum eDataFormat
{
	EDF_JPEG = 0,
	EDF_PNG,
	EDF_GIF,
	EDF_BMP
};


struct ImageInfo 
{
	int32 width;
	int32 height;
};

class EncoderListener
{
	friend class EncoderObject;
protected:
	virtual void OnEncodingSuccess(int32 size) = 0;
	virtual void OnEncodingCanceled() = 0;
	virtual void OnEncodingFailed() = 0;
};


class EncoderObject
{
public:
	static EncoderObject *Create();

	void SetListener(EncoderListener *newLisener);

	virtual void Update() = 0;
	virtual void Cancel() = 0;
	virtual bool IsFree() = 0;

	virtual const ImageInfo *GetInfo(char8 *dataBuffer, int32 bufferSize) = 0;
	virtual const ImageInfo *GetInfo(File *file) = 0;
	virtual bool GetSurface(char8 *srcBuffer, int32 bufferSize, GraphicsSystem::Surface *destSurface, ImageInfo *pDestProps = NULL) = 0;
	virtual bool GetSurface(File *srcFile, GraphicsSystem::Surface *destSurface, ImageInfo *pDestProps = NULL) = 0;
	virtual bool GetData(GraphicsSystem::Surface *sourceSurface, char8 *destBuffer, int32 bufferSize) = 0;
	virtual bool Resize(File *srcFile, char8 *destBuffer, int32 bufferSize, ImageInfo *pDestProps) = 0;

	virtual ~EncoderObject();

protected:
	EncoderObject();
	void OnEncodingSuccess(int32 size);
	void OnEncodingCanceled();
	void OnEncodingFailed();
private:

	EncoderListener *listener;


};
#endif