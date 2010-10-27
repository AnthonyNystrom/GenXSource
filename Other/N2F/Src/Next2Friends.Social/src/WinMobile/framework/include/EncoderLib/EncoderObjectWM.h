#ifndef __FRAMEWORK_ENCODER_OBJECT_WM__
#define __FRAMEWORK_ENCODER_OBJECT_WM__

#include "EncoderObject.h"

#define INNER_BUFFER_SIZE		(1024 * 10)
#define DECOMPRESS_PER_UPDATE	1024 * 10 //10K pixels of RGB image will be read per update

struct jpeg_decompress_struct;
struct jpeg_compress_struct;
struct jpeg_error_mgr;
struct jpeg_source_mgr;
struct jpeg_destination_mgr;


enum eEncodingState
{
	EES_IDLE = 0,
	EES_DECOMPRESS,
	EES_DECOMPRESS_CANCELED,
	EES_COMPRESS,
	EES_COMPRESS_CANCELED,
	EES_RESIZE,
	EES_RESIZE_CANCELED
};

class EncoderObjectWM : public EncoderObject
{
public:
	EncoderObjectWM(/*EncoderObjectListener *aListener*/);
	virtual ~EncoderObjectWM();

	virtual void Update();
	virtual void Cancel();
	virtual bool IsFree();

	virtual const ImageInfo *GetInfo(char8 *dataBuffer, int32 bufferSize);
	virtual const ImageInfo *GetInfo(File *file);
	virtual bool GetSurface(char8 *srcBuffer, int32 bufferSize, GraphicsSystem::Surface *destSurface, ImageInfo *pDestProps = NULL);
	virtual bool GetSurface(File *srcFile, GraphicsSystem::Surface *destSurface, ImageInfo *pDestProps = NULL);
	virtual bool GetData(GraphicsSystem::Surface *sourceSurface, char8 *destBuffer, int32 bufferSize);
	virtual bool Resize(File *srcFile, char8 *destBuffer, int32 bufferSize, ImageInfo *pDestProps);

	File *currentFile;
	uint8 innerBuffer[INNER_BUFFER_SIZE];
	bool isOk;

private:
	void LoadData(GraphicsSystem::Surface *destSurface, ImageInfo *img);
	void DecompressLines();
	void ResizeLines();
	void SetSourceToBuffer(char8 *buffer, int32 size);
	void SetSourceToFile(File *file);
	jpeg_decompress_struct	*decompressInfo;
	jpeg_compress_struct	*compressInfo;
	jpeg_error_mgr			*jpegErr;
	jpeg_source_mgr			*jsrc;
	jpeg_destination_mgr	*jdst;


//	=========   thread simulation data   =========
	GraphicsSystem::Surface *surface;
	int32 outWidth;
	int32 outHeight;
	Fixed fxHeight;
	Fixed widthDx;
	Fixed heightDx;
	int32 yBorder;
	void	*jBuffer;
	void	*jOutBuffer;
	int32 xAdd;
	int32 yAdd;
	int32 compressBufferSize;
//================================================

	ImageInfo info;


	eEncodingState state;



};
#endif