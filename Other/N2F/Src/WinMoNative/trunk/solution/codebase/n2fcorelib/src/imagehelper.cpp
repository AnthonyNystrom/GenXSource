#include "stdafx.h"
#include "imagehelper.h"



//#include <imaging.h>
#include <initguid.h>
#include <imgguids.h>

#include <urlmon.h>
// TODO: add comments!!!


ImageHelper* ImageHelper::iHelperInstance = NULL;

N2FCORE_API	ImageHelper::ImageHelper():
	iLastUsedImageKey(INVALID_IMAGE_KEY_VALUE),
	iJpegCodecFound(FALSE)
{
	HRESULT hr = E_FAIL;
	hr = iImagingFactory.CoCreateInstance(CLSID_ImagingFactory, NULL, CLSCTX_INPROC);
	if ( iImagingFactory )
	{
		UINT uEncodersCount = 0;
		ImageCodecInfo *pCodecInfo = NULL;
		hr = iImagingFactory->GetInstalledEncoders(&uEncodersCount, &pCodecInfo);
		if ( SUCCEEDED(hr) && pCodecInfo )
		{
			for ( UINT i = 0; i < uEncodersCount; i++ )
			{
				if ( wcsicmp(pCodecInfo[i].MimeType, CFSTR_MIME_JPEG) == 0 )
				{
					iJpegCodecFound = TRUE;
					iJpegCodecClsid = pCodecInfo[i].Clsid;
					break;
				}
			}
			CoTaskMemFree(pCodecInfo);
		}
	}
}

N2FCORE_API	ImageHelper::~ImageHelper()
{
	int cntImages = iImagesStorage.GetSize();

	for ( int i = 0; i < cntImages; i++ )
	{
		IImage *pImage = iImagesStorage.GetValueAt(i);
		if ( pImage )
		{
			pImage->Release();
		}
	}
	iImagesStorage.RemoveAll();
}

N2FCORE_API	ImageHelper	* ImageHelper::CreateInstance()
{
	iHelperInstance = new ImageHelper;

	if ( NULL == iHelperInstance->iImagingFactory )
	{
		ASSERT(FALSE);
		delete iHelperInstance;
		iHelperInstance = NULL;
	}

	return iHelperInstance;
}

N2FCORE_API	ImageHelper	* ImageHelper::GetInstance()
{
	if ( iHelperInstance == NULL )
		ImageHelper::CreateInstance();

	ASSERT(NULL != iHelperInstance);

	return iHelperInstance;
}

N2FCORE_API	void ImageHelper::DeleteInstance()
{
	if ( iHelperInstance )
	{
		delete iHelperInstance;
		iHelperInstance = NULL;
	}
}

N2FCORE_API	IMAGE_KEY_TYPE ImageHelper::GetNextFreeImageKey()
{
	int idxFound = -1;
	IMAGE_KEY_TYPE savedValue = iLastUsedImageKey;

	do {
		++iLastUsedImageKey;

		if ( savedValue == iLastUsedImageKey )
		{
			// we do have cycling - no more free image keys
			ASSERT(FALSE);
			break;
		}

		idxFound = this->iImagesStorage.FindKey(iLastUsedImageKey);
	} while ( (idxFound != -1) && (INVALID_IMAGE_KEY_VALUE != iLastUsedImageKey) );

	return ( savedValue != iLastUsedImageKey )? iLastUsedImageKey: INVALID_IMAGE_KEY_VALUE;
}

N2FCORE_API	HRESULT ImageHelper::GetLastHResult()
{
	return hr;
}

N2FCORE_API	BOOL ImageHelper::FreeImage( IMAGE_KEY_TYPE keyImage )
{
	BOOL res = FALSE;
	hr = E_FAIL;

	int idxFound = iImagesStorage.FindKey(keyImage);
	if ( idxFound != -1 )
	{
		IImage *pImage = iImagesStorage.GetValueAt(idxFound);
		if ( pImage )
		{
			pImage->Release();
		}
		iImagesStorage.RemoveAt(idxFound);
		res = TRUE;
		hr = S_OK;
	}

	return res;
}

N2FCORE_API	BOOL ImageHelper::GetImageSize( IMAGE_KEY_TYPE keyImage, SIZE &szImage )
{
	BOOL res = FALSE;
	hr = E_FAIL;
	szImage.cx = szImage.cy = 0;

	IImage *pImage = NULL;

	if ( this->GetImageByKey(keyImage, &pImage) == TRUE && pImage )
	{
		ImageInfo imgInfo = {0};
		hr = pImage->GetImageInfo(&imgInfo);
		if ( SUCCEEDED(hr) )
		{
			szImage.cx = imgInfo.Width;
			szImage.cy = imgInfo.Height;

			res = TRUE;
		}

		pImage->Release();
	}

	return res;
}


N2FCORE_API	BOOL ImageHelper::GetImageFromFile( CString& csFileName, IMAGE_KEY_TYPE &keyImage )
{
	BOOL res = FALSE;
	hr = E_FAIL;
	keyImage = INVALID_IMAGE_KEY_VALUE;

	if ( csFileName.GetLength() > 0 && iImagingFactory )
	{
		IImage *pImage = NULL;
		hr = iImagingFactory->CreateImageFromFile(csFileName, &pImage);
		if ( SUCCEEDED(hr) && pImage )
		{

			if ( this->AddImageToStorage(pImage, keyImage) && keyImage != INVALID_IMAGE_KEY_VALUE )
			{
				res = TRUE;
			}

			pImage->Release();
			pImage = NULL;
		}
	}

	return res;
}

N2FCORE_API	BOOL ImageHelper::GetImageFromStream( IStream *pStream, IMAGE_KEY_TYPE &keyImage )
{
	BOOL res = FALSE;
	hr = E_FAIL;
	keyImage = 0;

	if ( pStream == NULL )
		return res;

	IImage			*pImage = NULL;

	if ( iImagingFactory )
	{
		hr = iImagingFactory->CreateImageFromStream(pStream, &pImage);
		if ( SUCCEEDED(hr) && pImage )
		{
			if ( this->AddImageToStorage(pImage, keyImage) )
			{
				res = TRUE;
			}

			hr = pStream->Release();
			pImage->Release();
			pImage = NULL;
		}
	}

	return res;
}

N2FCORE_API	BOOL ImageHelper::GetStreamSize( IStream *pStream, ULONG *pulSize )
{
	hr = E_FAIL;
	BOOL res = FALSE;
	*pulSize = 0;

	LARGE_INTEGER  li = {0};
	ULARGE_INTEGER uliZero = {0};
	ULARGE_INTEGER uli;

	if ( pStream == NULL )
		return res;

	hr = pStream->Seek(li, STREAM_SEEK_END, &uli);
	if (FAILED(hr))
	{
		*pulSize = 0;
		return res;
	}
	*pulSize = uli.LowPart;
	// Move the stream back to the beginning of the file
	hr = pStream->Seek(li, STREAM_SEEK_SET, &uliZero);
	if ( SUCCEEDED(hr) )
	{
		res = TRUE;
	}

	return res;
}

N2FCORE_API	BOOL ImageHelper::GetImageByKey( IMAGE_KEY_TYPE keyImage, IImage **pImage )
{
	BOOL res = FALSE;
	hr = E_FAIL;

	int idxFound = iImagesStorage.FindKey(keyImage);
	if ( idxFound != -1 )
	{
		IImage *pFoundImage = iImagesStorage.GetValueAt(idxFound);
		if ( pFoundImage )
		{
			*pImage = pFoundImage;
			(*pImage)->AddRef();

			hr = S_OK;
			res = TRUE;
		}
	}

	return res;
}

N2FCORE_API	BOOL ImageHelper::AddImageToStorage( IImage *pImage, IMAGE_KEY_TYPE &newImageKey )
{
	BOOL res = FALSE;
	hr = E_FAIL;
	newImageKey = INVALID_IMAGE_KEY_VALUE;

	if ( pImage )
	{
		newImageKey = this->GetNextFreeImageKey();

		pImage->AddRef();
		iImagesStorage.Add(newImageKey, pImage);

		res = TRUE;
		hr = S_OK;
	}

	return res;
}

N2FCORE_API	BOOL ImageHelper::GetThumbnailImageForImage( IMAGE_KEY_TYPE keySrcImage, UINT &uMaxWidth, UINT &uMaxHeight, IMAGE_KEY_TYPE &keyNewImage )
{
	BOOL res = FALSE;
	hr = E_FAIL;
	keyNewImage = 0;

	IImage *pSrcImage = NULL;
	if ( this->GetImageByKey(keySrcImage, &pSrcImage) == TRUE && pSrcImage )
	{
		ImageInfo imgInfo = {0};
		hr = pSrcImage->GetImageInfo(&imgInfo);
		if ( SUCCEEDED(hr) )
		{
			this->ScaleSizesProportional(uMaxWidth, uMaxHeight, &imgInfo.Width, &imgInfo.Height);

			IImage *pThumbImage = NULL;
			hr = pSrcImage->GetThumbnail(imgInfo.Width, imgInfo.Height, &pThumbImage);
			if ( SUCCEEDED(hr) && pThumbImage )
			{
				this->AddImageToStorage(pThumbImage, keyNewImage);

				pThumbImage->Release();
				pThumbImage = NULL;

				res = TRUE;
			}

		}

		pSrcImage->Release();
	}

	return res;
}

N2FCORE_API	BOOL ImageHelper::ScaleSizesProportional( UINT uFitToWidth, UINT uFitToHeight, UINT *puWidthToScale, UINT *puHeightToScale )
{
	BOOL res = FALSE;
	hr = E_FAIL;

	if ( puWidthToScale == NULL || puHeightToScale == NULL )
	{
		return res;
	}

	// Scale (*puWidthToScale, *puHeightToScale) to fit within (uFitToWidth, uFitToHeight), while
	// maintaining the aspect ratio
	int nScaledWidth = ::MulDiv(*puWidthToScale, uFitToHeight, *puHeightToScale);

	// If we didn't overflow and the scaled width does not exceed bounds
	if (nScaledWidth >= 0 && nScaledWidth <= (int)uFitToWidth)
	{
		*puWidthToScale  = nScaledWidth;
		*puHeightToScale = uFitToHeight;

		hr = S_OK;
		res = TRUE;
	}
	else
	{
		*puHeightToScale = ::MulDiv(*puHeightToScale, uFitToWidth, *puWidthToScale);

		// The height *must* be within the bounds [0, uFitToHeight] since we overflowed
		// while fitting to height
		if (*puHeightToScale >= 0 && *puHeightToScale <= uFitToHeight)
		{
			*puWidthToScale  = uFitToWidth;
			hr = S_OK;
			res = TRUE;
		}
	}

	return res;
}

N2FCORE_API	BOOL ImageHelper::DrawImageInDC( IMAGE_KEY_TYPE keyImage, HDC hDC, RECT rcDest )
{
	BOOL res = FALSE;
	hr = E_FAIL;
	RECT rcTemp = rcDest;

	IImage *pImage = NULL;
	if ( this->GetImageByKey(keyImage, &pImage) == TRUE && pImage )
	{
		SIZE szImage = {0};
		if ( this->GetImageSize(keyImage, szImage) == TRUE )
		{
			int diffWidth = rcTemp.right - rcTemp.left - szImage.cx;
			int diffHeight = rcTemp.bottom - rcTemp.top - szImage.cy;

			if ( diffWidth > 0 )
			{
				int off = diffWidth / 2;
				rcTemp.left += off;
				rcTemp.right -= off;
			}

			if ( diffHeight > 0 )
			{
				int off = diffHeight / 2;
				rcTemp.top += off;
				rcTemp.bottom -= off;
			}
		}

		hr = pImage->Draw(hDC, &rcTemp, NULL);
		if ( SUCCEEDED(hr) )
		{
			res = TRUE;
		}

		pImage->Release();
	}

	return res;
}

N2FCORE_API	void ImageHelper::DrawRect( HDC hdc, LPRECT prc, COLORREF clr )
{
	COLORREF clrSave = SetBkColor(hdc, clr);
	ExtTextOut(hdc,0,0,ETO_OPAQUE,prc,NULL,0,NULL);
	SetBkColor(hdc, clrSave);
}

N2FCORE_API	HBITMAP ImageHelper::HBITMAPFromImage( IMAGE_KEY_TYPE keyImage, COLORREF crBackColor )
{
	hr = E_FAIL;

	int idxFound = iImagesStorage.FindKey(keyImage);
	if ( idxFound == -1 )
	{
		return NULL;
	}


	SIZE	szImage = {0}; 
	HDC hDC = NULL;
	HBITMAP hBitmapNew = NULL;
	BOOL bDrawSucceeded = FALSE;

	if ( this->GetImageSize(keyImage, szImage) == TRUE && szImage.cx > 0 && szImage.cy > 0 )
	{
		HDC hdcScreen = ::GetDC(NULL);
		hDC = ::CreateCompatibleDC(hdcScreen);
		if ( hDC == NULL )
			return NULL;

		hBitmapNew = ::CreateCompatibleBitmap(hdcScreen, szImage.cx, szImage.cy);
		if ( hBitmapNew )
		{
			HGDIOBJ hOldBitmap = ::SelectObject(hDC, hBitmapNew);
			RECT rect = {0, 0, szImage.cx, szImage.cy};

			this->DrawRect(hDC, &rect, crBackColor);
			if ( this->DrawImageInDC(keyImage, hDC, rect) == TRUE )
			{
				bDrawSucceeded = TRUE;
			}

			::SelectObject(hDC, hOldBitmap);

			::DeleteDC(hDC);

			if ( FALSE == bDrawSucceeded )
			{
				::DeleteObject(hBitmapNew);
				hBitmapNew = NULL;
			}
		}

		if ( hdcScreen )
			::ReleaseDC(NULL, hdcScreen);
	}

	return hBitmapNew;
}