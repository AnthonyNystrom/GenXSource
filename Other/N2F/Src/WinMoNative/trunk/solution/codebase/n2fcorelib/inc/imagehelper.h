#pragma once

//#include <n2fcore.h>

#include <imaging.h>

#define IMAGE_KEY_TYPE	int				/*!< macro for image keys type definition */
#define INVALID_IMAGE_KEY_VALUE	(0)		/*!< macro for invalid image key value */


//! ImageHelper class

/*!
	ImageHelper class provides images handling & management functionality.
	ImageHelper is a singleton.
*/

class ImageHelper
{
protected:

	//! ImageHelper class constructor
	/*! 
		ImageHelper constructor is protected due to singleton pattern.
		Use CreateInstance()/GetInstance() to obtain ImageHelper instance.
		\see CreateInstance()
		\see GetInstance()
	*/
	N2FCORE_API	ImageHelper();

	//! ImageHelper class destructor
	/*! 
		ImageHelper destructor is protected due to singleton pattern.
		Use DeleteInstance() to free ImageHelper instance.
		\see DeleteInstance()
	*/
	N2FCORE_API	virtual ~ImageHelper();

public:

	//! Create new ImageHelper instance
	/*! If ImageHelper exists already, it will be deleted, and the new one will be created */
	N2FCORE_API	static	ImageHelper	*CreateInstance();

	//! Obtain ImageHelper instance
	/*! If ImageHelper wasn't created yet, it will be created */
	N2FCORE_API	static	ImageHelper	*GetInstance();

	//! Free ImageHelper instance
	N2FCORE_API	static	void	DeleteInstance();

	//! Free specified image resource
	/*!
		In success case, ImageHelper's hr value is set to S_OK, otherwise - E_FAIL.
		\param keyImage is key of image, which should be freed
		\return TRUE - if success, FALSE - if failure
	*/
	N2FCORE_API	BOOL		FreeImage(IMAGE_KEY_TYPE keyImage);

	//! Returns current value of ImageHelper's hr member
	/*! Most of ImageHelper's methods set this value due it's execution result, or an error code in case of failure.
		\return HRESULT value	
	*/
	N2FCORE_API	HRESULT		GetLastHResult();

	//! Load image from specified file
	/*! Specified image file is loaded, and image is stored in ImageHelper's list
		\param csFileName specified image file to be loaded
		\param keyImage [out] is set to loaded image key, or INVALID_IMAGE_KEY_VALUE in case of failure
		\see IMAGE_KEY_TYPE
		\see INVALID_IMAGE_KEY_VALUE
		\return TRUE - if success, FALSE - if failure
	*/
	N2FCORE_API	BOOL		GetImageFromFile(CString& csFileName, IMAGE_KEY_TYPE &keyImage);

	//! Load image from specified stream
	/*! Specified image file is loaded, and image is stored in ImageHelper's list
		\param pStream pointer to IStream-compatible instance
		\param keyImage [out] is set to loaded image key, or INVALID_IMAGE_KEY_VALUE in case of failure
		\see IMAGE_KEY_TYPE
		\see INVALID_IMAGE_KEY_VALUE
		\return TRUE - if success, FALSE - if failure
	*/
	N2FCORE_API	BOOL		GetImageFromStream(IStream *pStream, IMAGE_KEY_TYPE &keyImage);
	
	//! Resolve specified images' dimensions
	/*! Resolves dimensions for previously loaded by ImageHelper image, specified by image key
		\param keyImage key for desired image, previously loaded by ImageHelper
		\param szImage [out] is set specified image's dimensions, or to (0,0) in case of failure
		\see IMAGE_KEY_TYPE
		\see INVALID_IMAGE_KEY_VALUE
		\return TRUE - if success, FALSE - if failure
	*/
	N2FCORE_API	BOOL		GetImageSize(IMAGE_KEY_TYPE keyImage, SIZE &szImage);
	
	//! Resolve size in bytes for specified stream
	/*! 
		\param pStream pointer to IStream-compatible instance
		\param pulSize [out] pointer to ULONG to hold sream size in bytes
		\return TRUE - if success, FALSE - if failure
	*/
	N2FCORE_API	BOOL		GetStreamSize(IStream *pStream, ULONG *pulSize);

	//! Resize image (mainly shrinking)
	/*! Specified image is resized according specified dimensions and is stored in ImageHelper's storage
		\param keySrcImage source image key
		\param uMaxWidth maximum width of resized image in pixels
		\param uMaxHeight maximum height of resized image in pixels
		\param keyNewImage [out] is key for resized image, or INVALID_IMAGE_KEY_VALUE in case of failure
		\see IMAGE_KEY_TYPE
		\see INVALID_IMAGE_KEY_VALUE
		\return TRUE - if success, FALSE - if failure
	*/
	N2FCORE_API	BOOL		GetThumbnailImageForImage(int keySrcImage, UINT &uMaxWidth, UINT &uMaxHeight, int &keyNewImage);

	//! Counts size values for specified conditions
	/*! \param uFitToWidth maximum width can be used
		\param uFitToHeight maximum height can be used
		\param [out] puWidthToScale resulting width
		\param [out] puHeightToScale resulting height
		\return TRUE - if success, FALSE - if failure
	*/
	N2FCORE_API	BOOL		ScaleSizesProportional(UINT uFitToWidth, UINT uFitToHeight, UINT *puWidthToScale, UINT *puHeightToScale);

	//! Draw specified image in DC
	/*! \param keyImage key image specified
		\param hDC DC to draw image in
		\param rcDest destination rectangle, where image should be drawn
		\return TRUE - if success, FALSE - if failure
	*/
	N2FCORE_API	BOOL		DrawImageInDC(IMAGE_KEY_TYPE keyImage, HDC hDC, RECT rcDest);

	//! Resolve GDI HBITMAP object from specified image, stored by ImageHelper
	/*! \param keyImage key image specified
		\param crBackColor color for background of image
		\return valid HBITMAP handle if successful, NULL in case of failure
	*/
	N2FCORE_API	HBITMAP		HBITMAPFromImage(IMAGE_KEY_TYPE keyImage, COLORREF crBackColor);

	

protected:

	HRESULT		hr;	/*!< holds HRESULT value for last method complition result */
	IMAGE_KEY_TYPE	iLastUsedImageKey;	/*!< stores maximum used image key value */

	//! Generates new unique image key value
	/*! 
		\return generated image key value
	*/
	N2FCORE_API	IMAGE_KEY_TYPE GetNextFreeImageKey();
	
	//! Adds already existing IImage-compatible object to ImageHelper's storage
	/*! 
		\param pImage pointer to IImage-compatible instance
		\param keyImage [out] is set to loaded image key, or INVALID_IMAGE_KEY_VALUE in case of failure
		\see IMAGE_KEY_TYPE
		\see INVALID_IMAGE_KEY_VALUE
		\return TRUE - if success, FALSE - if failure
	*/
	N2FCORE_API	BOOL		AddImageToStorage(IImage *pImage, IMAGE_KEY_TYPE &newImageKey);

	//! Resolves IImage pointer by specified image key value
	/*! 
		\param keyImage specified image key value
		\param pImage [out] is set to pointer of IImage instance, or NULL in case of failure
		\see IMAGE_KEY_TYPE
		\return TRUE - if success, FALSE - if failure
	*/
	N2FCORE_API	BOOL		GetImageByKey(IMAGE_KEY_TYPE keyImage, IImage **pImage);

	N2FCORE_API	void		DrawRect(HDC hdc, LPRECT prc, COLORREF clr);

	

	CComPtr<IImagingFactory> iImagingFactory;	/*!< Smart pointer for storing IImagingFactory instance */
	CSimpleMap<IMAGE_KEY_TYPE, IImage*>	iImagesStorage;	/*!< Map collection for storing images, associated with keys */

	CLSID		iJpegCodecClsid;	/*!< CLSID for jpeg codec */
	BOOL		iJpegCodecFound;	/*!< shows if jpeg codec was found */
	
	static	ImageHelper	*iHelperInstance;	/*!< stores current ImageHelper instance */
	
};
