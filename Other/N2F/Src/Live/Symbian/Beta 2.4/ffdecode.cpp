
#include "stdafx.h"
#include "ffdecode.h"
#include "MMediaObj.h"

extern "C" {
#include "../gd/include/gd.h"
}

CFFDecode::CFFDecode()
	: m_codec(NULL)
	, m_c(NULL)
{
}


CFFDecode::~CFFDecode()
{
	if (m_c != NULL) {
		avcodec_close(m_c);
		av_free(m_c);
		m_c = NULL;
	}
	if(tmp_picture)
		av_free(tmp_picture);
}


int CFFDecode::Init(int codec_id, int width, int height, int fps, char *pFileNameThumbBmp)
{
	if ((width & 1) == 1) return E_INVALIDARG;
	if ((height & 1) == 1) return E_INVALIDARG;

	strcpy_s(m_fileNamePic, pFileNameThumbBmp);
	DbgOut(m_fileNamePic);
	m_codec = avcodec_find_decoder((CodecID)codec_id);
	if (m_codec == NULL) return E_FAIL;

	m_c = avcodec_alloc_context(); 
	if (m_c == NULL) return E_OUTOFMEMORY;

	avcodec_get_context_defaults2(m_c, CODEC_TYPE_VIDEO);
	avcodec_get_frame_defaults(&m_picture);
	
	m_c->codec_id = (CodecID) codec_id;
	m_c->codec_type = CODEC_TYPE_VIDEO;
	m_c->strict_std_compliance = 0;

	/* put sample parameters */
	//m_c->bit_rate = bitrate;
	/* resolution must be a multiple of two */
	m_c->width = width;
	m_c->height = height;
	/* time base: this is the fundamental unit of time (in seconds) in terms
	   of which frame timestamps are represented. for fixed-fps content,
	   timebase should be 1/framerate and timestamp increments should be
	   identically 1. */
	m_c->time_base.den = fps;
	m_c->time_base.num = 1;
	m_c->gop_size = 12; /* emit one intra frame every twelve frames at most */
	m_c->pix_fmt = PIX_FMT_YUV420P;
	if (m_c->codec_id == CODEC_ID_MPEG2VIDEO) {
		/* just for testing, we also add B frames */
		m_c->max_b_frames = 2;
	}
	if (m_c->codec_id == CODEC_ID_MPEG1VIDEO){
		/* needed to avoid using macroblocks in which some coeffs overflow
		   this doesnt happen with normal video, it just happens here as the
		   motion of the chroma plane doesnt match the luma plane */
		m_c->mb_decision=2;
	}

	/* open the codec */
	if (avcodec_open(m_c, m_codec) < 0) {
		return E_FAIL;
	}

	tmp_picture = NULL;
	
	return S_OK;
}

// size pDataOut for YUV (width*height*3)/2
int CFFDecode::DecodePic(unsigned char* pData, long lsize, long pts, AVFrame **ppFrame, bool *pInterlaced)
{
	if (ppFrame == NULL) return E_POINTER;
	if (m_c->pix_fmt != PIX_FMT_YUV420P) return E_FAIL;
	*ppFrame = NULL;

	//{
	//	int size = m_c->width * m_c->height;
	//	/*DbgOutInt("size:",size);

	//	m_picture->data[0] = pData;
	//	m_picture->data[1] = pData+size;
	//	m_picture->data[2] = pData+size+(m_c->width/2)*(m_c->height/2);*/

	//	m_picture->data[0] = pDataOut;
	//	m_picture->data[1] = m_picture->data[0] + size;
	//	m_picture->data[2] = m_picture->data[1] + size / 4;
	//	m_picture->linesize[0] = m_c->width;
	//	m_picture->linesize[1] = m_c->width / 2;
	//	m_picture->linesize[2] = m_c->width / 2;
	//}

	int got_picture = 0;
	int ret = 0;

	try {
		ret = avcodec_decode_video(m_c, &m_picture, &got_picture, pData, lsize);
		DbgOutInt("ret decode:",ret);
		DbgOutInt("lsize decode:",lsize);
	} catch (...) {
		ret = 0;
		got_picture = 0;
	}

	if (got_picture == 0) return S_FALSE;

	m_picture.quality = 1;
	*ppFrame = &m_picture;
	if (pInterlaced != NULL) 
		*pInterlaced = (bool) m_picture.interlaced_frame;

	if (!tmp_picture){ // one time jpg conversion
		img_convert_ctx = sws_getContext(m_c->width, m_c->height, PIX_FMT_YUV420P,320, 240,PIX_FMT_RGB32, SWS_FAST_BILINEAR,NULL,NULL,NULL);
		if (img_convert_ctx){
			tmp_picture = alloc_picture(PIX_FMT_RGB32, 320, 240);
			//fill_yuv_image(tmp_picture, 0, m_c->width, m_c->height);
			int ires=sws_scale(img_convert_ctx, m_picture.data,  m_picture.linesize,0, m_c->height, tmp_picture->data, tmp_picture->linesize);
			SaveFrame(tmp_picture, 320, 240);
		}
	}
	

	return S_OK;
}

void CFFDecode::SaveFrame(AVFrame *img, int width, int height)
{
	// Bitmap file header 
	BITMAPFILEHEADER *m_header = (BITMAPFILEHEADER *) malloc(sizeof(BITMAPFILEHEADER));
	m_header->bfType = 'MB'; 
	m_header->bfSize = 0;
	m_header->bfReserved1 = 0;
	m_header->bfReserved2 = 0;
	m_header->bfOffBits = sizeof(BITMAPFILEHEADER) + sizeof(BITMAPINFOHEADER);

	// Bitmap infoheader 
	BITMAPINFOHEADER *m_DIB = (BITMAPINFOHEADER *) malloc(sizeof(BITMAPINFOHEADER));
	m_DIB->biSize = sizeof (BITMAPINFOHEADER);
	m_DIB->biWidth = width;
	m_DIB->biHeight = height;
	m_DIB->biPlanes = 1; 
	m_DIB->biBitCount = 32;
	m_DIB->biCompression = BI_RGB; 
	m_DIB->biSizeImage = 0;
	m_DIB->biXPelsPerMeter = 0;
	m_DIB->biYPelsPerMeter = 0;
	m_DIB->biClrUsed = 0;
	m_DIB->biClrImportant = 0;

	unsigned char *pByteImg = img->data[0];
	int isizesample=4;
	if (m_DIB->biBitCount==24)
		isizesample--;
	int iSizeBmp=width*height*isizesample;

	// Open the file 
	HANDLE hFile = CreateFileA(m_fileNamePic, GENERIC_WRITE, 0, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
	if (hFile == INVALID_HANDLE_VALUE)
	return;

	// Write BITMAPFILEHEADER
	DWORD dwBytesWritten;
	WriteFile(hFile, m_header, sizeof(BITMAPFILEHEADER), &dwBytesWritten, NULL);

	// Write BITMAPINFOHEADER
	WriteFile(hFile, m_DIB, sizeof(BITMAPINFOHEADER), &dwBytesWritten, NULL);

	//WriteFile(hFile, pByteImg, iSizeBmp, &dwBytesWritten, NULL);

	// Write bitmap data
	unsigned char *p=pByteImg;
	for (int ii=0; ii<height; ii++){
		p=(pByteImg+iSizeBmp-(width*isizesample))-(ii*width*isizesample);
		for (int i=0; i<width; i++){
			WriteFile(hFile, (LPCVOID)p, 1, &dwBytesWritten, NULL);p++;
			WriteFile(hFile, (LPCVOID)p, 1, &dwBytesWritten, NULL);p++;
			WriteFile(hFile, (LPCVOID)p, 1, &dwBytesWritten, NULL);p++;
			if (m_DIB->biBitCount==32){
				WriteFile(hFile, (LPCVOID)p, 1, &dwBytesWritten, NULL);p++;}
		}
	}
	

	// Close the file 
	CloseHandle(hFile);




/*
  gdImagePtr jpegImg;
  FILE       *jpegFile;
  int        *srcImg;
  char       jpegFilename[256];
  int        x,y;

  // allochiamo l'immagine 
  jpegImg = gdImageCreateTrueColor(width, height);
  // recuperiamo il piano immagine (memorizzato nel primo vettore dell'AVFrame.data[...])
  srcImg = (int *)img->data[0];
  // impostiamo i pixel dell'immagine GD
  for(y=0; y < height; y++) {
    for(x=0; x < width; x++)
	gdImageSetPixel(jpegImg, x, y, srcImg[x] & 0x00FFFFFF);
    
     srcImg += width;
  }
  // creiamo il nome del file dal numero sequenziale di frame
  sprintf(jpegFilename, "c:\\tmp\\frame1.jpg");
  // apriamo il file in scrittura
  jpegFile = fopen(jpegFilename, "wb");
  // scriviamo l'immagine in formato JPEG
  gdImageJpeg(jpegImg,jpegFile,-1);
  fclose(jpegFile);

  gdImageDestroy(jpegImg);*/

}


/* prepare a dummy image */
void CFFDecode::fill_yuv_image(AVFrame *pict, int frame_index, int width, int height)
{
    int x, y, i;

    i = frame_index;

    /* Y */
    for(y=0;y<height;y++) {
        for(x=0;x<width;x++) {
            pict->data[0][y * pict->linesize[0] + x] = x + y + i * 3;
        }
    }

    /* Cb and Cr */
    for(y=0;y<height/2;y++) {
        for(x=0;x<width/2;x++) {
            pict->data[1][y * pict->linesize[1] + x] = 128 + y + i * 2;
            pict->data[2][y * pict->linesize[2] + x] = 64 + x + i * 5;
        }
    }
}
AVFrame *CFFDecode::alloc_picture(int pix_fmt, int width, int height)
{
    AVFrame *picture;
    uint8_t *picture_buf;
    int size;

    picture = avcodec_alloc_frame();
    if (!picture)
        return NULL;
    size = avpicture_get_size(pix_fmt, width, height);
    picture_buf = (uint8_t *)av_malloc(size);
    if (!picture_buf) {
        av_free(picture);
        return NULL;
    }
    avpicture_fill((AVPicture *)picture, picture_buf,
                   pix_fmt, width, height);
    return picture;
}

